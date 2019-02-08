#region Directives
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using DataDynamics.ActiveReports.Export;
using DataDynamics.ActiveReports.Document;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
#endregion

namespace JKS
{
    public class JKSCod : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.DataGrid grdList;

        #region  objects declarations
        protected CBSolutions.ETH.Web.Invoice objInvoice = new CBSolutions.ETH.Web.Invoice();
        protected Company objCompany = new Company();
        private Users objUser = new Users();
        #endregion

        #region Sql Variables
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        #endregion

        #region  variables
        JKS.Invoice objinvoice = new JKS.Invoice();
        protected string AuthorisationStringToolTips = "";
        public int invoiceID = 0;

        protected int iCurrentStatusID = 0;
        protected int TypeUser = 1;
        protected int RejectOpenFields = 0;
        protected int ReopenAtApprover = 0;
        protected string oldComment = "";
        protected string strAmountLimit = "";
        protected string strTimeLimit = "";
        protected double dTotalAmount = 0;
        protected System.Web.UI.WebControls.Button btnCancel;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Description;
        double dNetAmt = 0;
        protected System.Web.UI.WebControls.Button Button1;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.Button btnDelete;
        protected int iSupplierCompanyID = 0;
        #endregion

        #region Page_Load(object sender, System.EventArgs e)
        private void Page_Load(object sender, System.EventArgs e)
        {
            TypeUser = Convert.ToInt32(Session["UserTypeID"]);
            if (!IsPostBack)
            {

                CheckInvoiceExist();
            }
        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.grdList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdList_ItemDataBound);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region CheckInvoiceExist
        private void CheckInvoiceExist()
        {
            int RowCnt = 1;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);

            SqlDataAdapter sqlDA = new SqlDataAdapter("ups_GetGenericCodingChange", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(Request.QueryString["InvoiceID"]));
            sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RowCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    if (RowCnt == 0)
                    {
                        RowCnt = 1;
                        ViewState["Exist"] = "0";
                    }
                    else
                    {
                        ViewState["Exist"] = "1";
                    }
                }
                ViewState["populate"] = ds;
                BindGrid(RowCnt);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedIndex = -1;
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")), ds.Tables[1].Rows[i]["CompanyID"].ToString());
            }

            GetAllComboCodesFirstTime();

            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex = -1;
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ds.Tables[1].Rows[i]["DepartmentID"].ToString());

                if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex > 0)
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedIndex = -1;
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ds.Tables[1].Rows[i]["CodingDescriptionID"].ToString());

                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedIndex = -1;
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ds.Tables[1].Rows[i]["NominalCodeID"].ToString());

                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedIndex = -1;

                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ds.Tables[1].Rows[i]["ProjectCodeID"].ToString());
                }
            }
        }
        #endregion

        #region BindGrid
        private void BindGrid(int iNoofRow)
        {
            DataSet ds;
            if (ViewState["data"] == null)
            {
                CreateDataSet(iNoofRow);
            }
            ds = ((DataSet)(ViewState["data"]));
            grdList.DataSource = ds.Tables[0];
            grdList.DataBind();
        }
        #endregion

        #region CreateDataSet
        private void CreateDataSet(int iNoofRow)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(GetBlankTable(iNoofRow));
            ViewState["data"] = ds;
        }
        #endregion

        #region protected DataTable GetBlankTable(int iNoofRow)
        protected DataTable GetBlankTable(int iNoofRow)
        {
            DataTable tbl = null;
            int InvoiceID = 0;
            double dtmpNetAmt = 0;
            InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
            dtmpNetAmt = GetNetAmt(InvoiceID);
            ViewState["NetAmt"] = dtmpNetAmt;

            if (iNoofRow <= 1)
            {
                tbl = new DataTable();
                DataRow nRow;
                tbl.Columns.Add("NetValue");
                for (int i = 0; i < iNoofRow; i++)
                {
                    nRow = tbl.NewRow();
                    nRow["NetValue"] = dtmpNetAmt;
                    tbl.Rows.Add(nRow);
                }
            }
            else
            {
                DataSet ds = ((DataSet)ViewState["populate"]);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    tbl = new DataTable();
                    DataRow nRow;
                    tbl.Columns.Add("NetValue");
                    for (int i = 0; i < iNoofRow; i++)
                    {
                        nRow = tbl.NewRow();
                        nRow["NetValue"] = ds.Tables[1].Rows[i]["netvalue"];
                        tbl.Rows.Add(nRow);
                    }
                }
            }
            return tbl;
        }
        #endregion

        #region private void SetValueForCombo(System.Web.UI.WebControls.DropDownList ddlSrc,string sVal)
        private void SetValueForCombo(System.Web.UI.WebControls.DropDownList ddlSrc, string sVal)
        {
            int i = 0;
            ddlSrc.SelectedIndex = -1;
            for (i = 0; i <= ddlSrc.Items.Count - 1; i++)
            {
                if (ddlSrc.Items[i].Value.Trim() == sVal.Trim())
                {
                    ddlSrc.SelectedIndex = i;
                    break;
                }
            }
        }
        #endregion

        #region GetAllComboCodesFirstTime
        private void GetAllComboCodesFirstTime()
        {
            int compid = 0;
            DataTable dt = null;
            string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlProject1 = "", ddlNominalCode1 = "";
            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
                ddlProject1 = ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
                ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

                if (compid != 0)
                {
                    if (TypeUser == 1)
                        dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]), compid);
                    else
                        dt = objInvoice.GetGridDepartmentList(compid);

                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);


                    int iddlDept = 0;
                    int iNomin = 0;
                    string CodingDescription = "--Select--";
                    string strProjectName = "";
                    string strProjectID = "";
                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue) != "--Select--")
                    {
                        if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--")
                        {

                            iddlDept = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                            iNomin = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);

                            DataSet dsCODES = new DataSet();
                            dsCODES = GetCodingDescriptionAgainstDepartmentANDNominal(iddlDept, iNomin, compid);
                            DataSet dsProject = new DataSet();
                            dsProject = GetProjectCodeAgainstDepartmentANDNominal(iddlDept, iNomin, compid);
                            if (dsCODES.Tables.Count > 0 && dsCODES.Tables[0].Rows.Count > 0)
                            {
                                CodingDescription = dsCODES.Tables[0].Rows[0]["CodingDescriptionID"].ToString();

                            }
                            if (dsProject.Tables.Count > 0 && dsProject.Tables[0].Rows.Count > 0)
                            {

                                strProjectName = dsProject.Tables[0].Rows[0]["ProjectName"].ToString();
                                strProjectID = dsProject.Tables[0].Rows[0]["ProjectID"].ToString();
                            }
                            ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = CodingDescription;

                            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dsProject;
                            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");

                            SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), strProjectID);
                        }
                    }

                    dt = objInvoice.GetGridNominalCodeList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);

                    if (TypeUser == 1)
                        dt = objInvoice.GetGridCodingDescriptionListByUserID(Convert.ToInt32(Session["UserID"]), compid);
                    else
                        dt = objInvoice.GetGridCodingDescriptionList(compid);

                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ddlCodingDescription1);
                }
            }
        }
        #endregion

        #region SelectedIndexChanged_ddlCodingDescription
        protected void SelectedIndexChanged_ddlCodingDescription(Object sender, System.EventArgs e)
        {
            //			string ddlCodingDescription1="";
            string ddlDepartment1 = "", ddlProject1 = "", ddlNominalCode1 = "";
            int compid = 0;
            DataTable dt = new DataTable();
            DataSet dsDeptNom = new DataSet();
            DropDownList ddl = ((DropDownList)sender);
            int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
            if (TypeUser == 1)
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue == "--Select--")
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                }
                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
                {
                    dsDeptNom = GetDepartmentANDNominalAgainstCodingDescID(Convert.ToInt32(ddl.SelectedValue), compid);

                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                    if (dsDeptNom.Tables[0].Rows.Count > 0)
                    {
                        ddlDepartment1 = dsDeptNom.Tables[0].Rows[0]["DepartmentID"].ToString();


                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dsDeptNom.Tables[0];
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();

                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);

                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                    if (dsDeptNom.Tables[1].Rows.Count > 0)
                    {
                        ddlNominalCode1 = dsDeptNom.Tables[1].Rows[0]["NominalCodeID"].ToString();

                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dsDeptNom.Tables[1];
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();

                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);

                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Clear();
                    if (dsDeptNom.Tables[2].Rows.Count > 0)
                    {
                        ddlProject1 = dsDeptNom.Tables[2].Rows[0]["ProjectID"].ToString();

                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dsDeptNom.Tables[2];
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();

                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);
                }
            }
            else
            {
                if (Convert.ToString(ddl.SelectedValue) == "--Select--")
                {
                    if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                    {
                        compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                    }
                    if (compid != 0)
                    {
                        dt = objInvoice.GetGridDepartmentList(compid);

                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");

                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                    }
                    return;
                }

                int CodingDescriptionID = Convert.ToInt32(ddl.SelectedValue);

                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex = 0;
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedIndex = 0;
                    GetAllComboCodesAddNew();

                }
            }
        }
        #endregion

        #region SelectedIndexChanged_ddlBuyerCompanyCode
        protected void SelectedIndexChanged_ddlBuyerCompanyCode(object sender, System.EventArgs e)
        {
            int i = 0;
            DropDownList ddl = ((DropDownList)sender);
            i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;

            if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            {
                GetAllComboCodesFirstTime();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue = "--Select--";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue = "--Select--";
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = "--Select--";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue = "--Select--";
            }
            else
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
            }

        }
        #endregion

        #region SelectedIndexChanged_ddlDepartment
        protected void SelectedIndexChanged_ddlDepartment(object sender, System.EventArgs e)
        {
            int inominalCodeID = 0, iDepartmentCodeID = 0;
            int iDCompID = 0;
            DropDownList ddl = ((DropDownList)sender);
            int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
            if (Convert.ToString(ddl.SelectedValue) == "--Select--")
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    iDCompID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }

                DataTable dtcoding = new DataTable();
                dtcoding = objInvoice.GetGridCodingDescriptionList(iDCompID);
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dtcoding;
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");

                return;
            }


            if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            {
                iDCompID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
            }

            iDepartmentCodeID = Convert.ToInt32(ddl.SelectedValue);
            if (((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue == "--Select--")
            {
                inominalCodeID = 0;
            }
            else
                inominalCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);

            if (Convert.ToString(ddl.SelectedValue) != "--Select--")
            {

                DataSet Dst = new DataSet();
                Dst = GetNominalCodeAgainstDepartmentANDCompany(iDepartmentCodeID, iDCompID);
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = Dst;
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedIndex = 0;

            }
            else
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedIndex = 0;
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedIndex = 0;
            }


        }
        #endregion

        #region SelectedIndexChanged_ddlNominalCode
        protected void SelectedIndexChanged_ddlNominalCode(object sender, System.EventArgs e)
        {

            int iddlDept = 0;
            int iNomin = 0;
            int compid = 0;
            string CodingDescription = "--Select--";
            string strProjectName = "";
            string strProjectID = "";

            DropDownList ddl = ((DropDownList)sender);
            int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
            if (Convert.ToString(ddl.SelectedValue) != "--Select--")
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--")
                {

                    iddlDept = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                    iNomin = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);

                    DataSet dsCODES = new DataSet();
                    dsCODES = GetCodingDescriptionAgainstDepartmentANDNominal(iddlDept, iNomin, compid);
                    DataSet dsProject = new DataSet();
                    dsProject = GetProjectCodeAgainstDepartmentANDNominal(iddlDept, iNomin, compid);

                    if (dsCODES.Tables.Count > 0 && dsCODES.Tables[0].Rows.Count > 0)
                    {
                        CodingDescription = dsCODES.Tables[0].Rows[0]["CodingDescriptionID"].ToString();

                    }
                    if (dsProject.Tables.Count > 0 && dsProject.Tables[0].Rows.Count > 0)
                    {

                        strProjectName = dsProject.Tables[0].Rows[0]["ProjectName"].ToString();
                        strProjectID = dsProject.Tables[0].Rows[0]["ProjectID"].ToString();
                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = CodingDescription;

                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dsProject;
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), strProjectID);
                }
            }

        }
        #endregion

        #region GetCodingDescriptionAgainstDepartmentANDNominal
        public DataSet GetCodingDescriptionAgainstDepartmentANDNominal(int iDepartmentCodeID, int iNominal, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE isnull(APAdminOnly,0) <> 1 AND DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " and DepartmentCodeID in (select DepartmentID from userdeptrelation where UserID = " + Convert.ToInt32(Session["UserID"]) + ") ";
            else
                sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " ";

            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region
        public DataSet GetProjectCodeAgainstDepartmentANDNominal(int iDepartmentCodeID, int iNominal, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT ISNULL(ProjectID,0) AS ProjectID,(select ProjectName from Project where ProjectID=CodingDescription.ProjectID) as ProjectName FROM CodingDescription WHERE isnull(APAdminOnly,0) <> 1 AND DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " and DepartmentCodeID in (select DepartmentID from userdeptrelation where UserID = " + Convert.ToInt32(Session["UserID"]) + ") and ProjectID <> 0 and ProjectID is not null";
            else
                sSql = "SELECT ISNULL(ProjectID,0) AS ProjectID,(select ProjectName from Project where ProjectID=CodingDescription.ProjectID) as ProjectName FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " and ProjectID <> 0 and ProjectID is not null";

            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region grdList_ItemDataBound
        private void grdList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                int j = e.Item.DataSetIndex + 1;
                e.Item.Cells[0].Text = j.ToString();
                DataTable dt = null;
                if (TypeUser == 1)
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLogGMG(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]));
                else
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));




                /*--------------Added by kuntalkarar on 27thMay2016-------------------*/
                if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "Select Company Name");

                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = Session["CompanyID"].ToString();
                }


                else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    string LogInCompanyid = "";
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "Select Company Name");
                    PasswordReset objPasswordReset = new PasswordReset();
                    List<PasswordReset> lstSaltedPassword = objPasswordReset.GetLogInCompanyId(Convert.ToInt32(Session["UserID"]));//strResetAnswer
                    if (lstSaltedPassword.Count > 0)
                    {
                        LogInCompanyid = lstSaltedPassword[0].LoginCompanyId;
                    }


                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = LogInCompanyid;

                }

                else
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "Select Company Name");

                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = dt.Rows[0][0].ToString();
                    Session["DropDownCompanyID"] = ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString();
                }

                //===================================================================================

                /*((DropDownList)	e.Item.FindControl("ddlBuyerCompanyCode")).DataSource=dt;
                ((DropDownList)	e.Item.FindControl("ddlBuyerCompanyCode")).DataTextField="CompanyName";
                ((DropDownList)	e.Item.FindControl("ddlBuyerCompanyCode")).DataValueField="CompanyID";
                ((DropDownList)	e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
                ((DropDownList)	e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0,"--Select--");*/

                GetAllComboCodesFirstTime();
                try
                {
                    if (Request["DDCompanyID"] != null)
                        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = Session["BuyerCID"].ToString().Trim();
                }
                catch { }

                ((DropDownList)e.Item.FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlProject1")).Items.Insert(0, "--Select--");

                if (((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim() != "")
                {
                    dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim());
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetVal")).Text = dNetAmt.ToString();
                if (ViewState["NetAmt"] != null)
                {
                    ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetInvoiceTotal")).Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString();
                }
            }
        }
        #endregion

        #region GetNetAmt
        private double GetNetAmt(int InvoiceID)
        {
            double NetAmt = 0;
            string sSql = "select nettotal from invoice where invoiceid=" + InvoiceID;
            SqlDataReader dr = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);


            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                dr = sqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr[0] != DBNull.Value)
                    {
                        NetAmt = Convert.ToDouble(dr[0]);
                    }
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr.Close();
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return NetAmt;
        }
        #endregion

        #region GetDepartmentANDNominalAgainstCodingDescID
        public DataSet GetDepartmentANDNominalAgainstCodingDescID(int iCodingID, int iCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter("sp_GetDepartmentANDNominalAgainstCodingDescID_GMG", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CodingDescriptionID", iCodingID);
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompID);
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", TypeUser);

            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region GetAllComboCodesAddNew
        private void GetAllComboCodesAddNew()
        {
            int compid = 0;
            DataTable dt = null;
            string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlProject1 = "", ddlNominalCode1 = "";
            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {

                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
                ddlProject1 = ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
                ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

                if (TypeUser == 1)
                    dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]), compid);
                else
                    dt = objInvoice.GetGridDepartmentList(compid);

                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);

                if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--")
                {
                    if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--" && ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue != "--Select--")
                    {
                        int iddlDept = 0;
                        int iNomin = 0;

                        iddlDept = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                        iNomin = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);

                        DataSet dsCODES = new DataSet();

                        dsCODES = GetCodingDescriptionAgainstDepartmentANDNominal(iddlDept, iNomin, compid);
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dsCODES;
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ddlCodingDescription1);

                        dt = objInvoice.GetGridProjectList(compid);
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);

                    }
                    else if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
                    {

                        dt = objInvoice.GetGridNominalCodeList(compid);
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);

                        dt = objInvoice.GetGridProjectList(compid);
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);
                    }
                }

                else if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
                {
                    DataSet dsDeptNom = new DataSet();
                    int iCoding = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue);
                    dsDeptNom = GetDepartmentANDNominalAgainstCodingDescID(iCoding, compid);
                    if (dsDeptNom.Tables[0].Rows.Count > 0)
                    {
                        ddlDepartment1 = dsDeptNom.Tables[0].Rows[0]["DepartmentID"].ToString();
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);
                    }
                    if (dsDeptNom.Tables[1].Rows.Count > 0)
                    {
                        ddlNominalCode1 = dsDeptNom.Tables[1].Rows[0]["NominalCodeID"].ToString();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dsDeptNom.Tables[1];
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);
                    }
                    if (dsDeptNom.Tables[2].Rows.Count > 0)
                    {
                        ddlProject1 = dsDeptNom.Tables[2].Rows[0]["ProjectID"].ToString();
                    }
                    dt = objInvoice.GetGridProjectList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);
                }
                else
                {
                    dt = objInvoice.GetGridProjectList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);

                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");

                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                }
            }
        }
        #endregion

        #region GetNominalCodeAgainstDepartmentANDCompany
        public DataSet GetNominalCodeAgainstDepartmentANDCompany(int iDepartmentCodeID, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in (SELECT NominalCodeID FROM CodingDescription WHERE isnull(APAdminOnly,0)<>1 and DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ")";
            else
                sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in (SELECT NominalCodeID FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ")";
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region UpdateAPAdminOnly
        private int UpdateAPAdminOnly(int CodeDescID, int APAdminFlag)
        {


            int iretval = 0;
            string sSql = "UPDATE CodingDescription SET APAdminOnly =" + APAdminFlag + " WHERE CodingDescriptionID =" + CodeDescID;

            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandType = CommandType.Text;
            try
            {
                sqlConn.Open();
                iretval = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iretval = -1; }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }
            return iretval;
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int CodeDescID = 0;
            int APAdminFlag = 0;
            int sRet = 0;
            int CompanyIndex = 0;
            int DeptIndex = 0;
            int NominalIndex = 0;
            int CodeDescIndex = 0;
            //			int ProjectIndex = 0;
            int APAdminOnlyIndex = 0;

            CompanyIndex = ((DropDownList)grdList.Items[0].FindControl("ddlBuyerCompanyCode")).SelectedIndex;
            DeptIndex = ((DropDownList)grdList.Items[0].FindControl("ddlDepartment1")).SelectedIndex;
            NominalIndex = ((DropDownList)grdList.Items[0].FindControl("ddlNominalCode1")).SelectedIndex;
            CodeDescIndex = ((DropDownList)grdList.Items[0].FindControl("ddlCodingDescription1")).SelectedIndex;
            APAdminOnlyIndex = ((DropDownList)grdList.Items[0].FindControl("ddlCapexReq")).SelectedIndex;

            if (CompanyIndex == 0 || DeptIndex == 0 || NominalIndex == 0 || CodeDescIndex == 0)
            {
                Response.Write("<script>alert('Please select a valid coding combination');</script>");
                return;
            }
            if (APAdminOnlyIndex == 0)
            {
                Response.Write("<script>alert('Please select AP Admin only flag');</script>");
                return;
            }
            if (((DropDownList)grdList.Items[0].FindControl("ddlCodingDescription1")).SelectedValue.ToUpper().IndexOf("SELECT") == -1)
                CodeDescID = Convert.ToInt32(((DropDownList)grdList.Items[0].FindControl("ddlCodingDescription1")).SelectedValue);

            if (((DropDownList)grdList.Items[0].FindControl("ddlCapexReq")).SelectedValue.ToUpper().IndexOf("SELECT") == -1)
                APAdminFlag = Convert.ToInt32(((DropDownList)grdList.Items[0].FindControl("ddlCapexReq")).SelectedValue);

            if (CodeDescID > 0)
                sRet = UpdateAPAdminOnly(CodeDescID, APAdminFlag);

            if (sRet > 0)
                Response.Write("<script>alert('Change applied successfully'); self.close();</script>");
        }
        #endregion

        #region btnDelete_Click
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            int sRet = 0;
            int CodeDescID = 0;
            int CompanyIndex = 0;
            int DeptIndex = 0;
            int NominalIndex = 0;
            int CodeDescIndex = 0;
            //			int ProjectIndex = 0;

            CompanyIndex = ((DropDownList)grdList.Items[0].FindControl("ddlBuyerCompanyCode")).SelectedIndex;
            DeptIndex = ((DropDownList)grdList.Items[0].FindControl("ddlDepartment1")).SelectedIndex;
            NominalIndex = ((DropDownList)grdList.Items[0].FindControl("ddlNominalCode1")).SelectedIndex;
            CodeDescIndex = ((DropDownList)grdList.Items[0].FindControl("ddlCodingDescription1")).SelectedIndex;


            if (CompanyIndex == 0 || DeptIndex == 0 || NominalIndex == 0 || CodeDescIndex == 0)
            {
                Response.Write("<script>alert('Please select a valid coding combination');</script>");
                return;
            }

            if (((DropDownList)grdList.Items[0].FindControl("ddlCodingDescription1")).SelectedValue.ToUpper().IndexOf("SELECT") == -1)
                CodeDescID = Convert.ToInt32(((DropDownList)grdList.Items[0].FindControl("ddlCodingDescription1")).SelectedValue);

            if (CodeDescID > 0)
                sRet = DeleteCombination(CodeDescID);

            if (sRet > 0)
                Response.Write("<script>alert('Coding combination successfully deleted'); self.close();</script>");
        }
        #endregion

        #region DeleteCombination()
        private int DeleteCombination(int CodeDescID)
        {
            int iretval = 0;
            string sSql = "DELETE FROM CodingDescription WHERE CodingDescriptionID =" + CodeDescID;

            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandType = CommandType.Text;
            try
            {
                sqlConn.Open();
                iretval = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iretval = -1; }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }
            return iretval;
        }
        #endregion
    }
}