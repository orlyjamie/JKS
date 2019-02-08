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

using System.IO;

#endregion

namespace CBSolutions.ETH.Web.ETC.creditnotes
{
    /// <summary>
    /// Summary description for ActionCredit.
    /// </summary>
    public class ActionCredit : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WEb Controls
        protected System.Web.UI.WebControls.Label lblConfirmation;
        protected System.Web.UI.WebControls.Label lblRefernce;
        protected System.Web.UI.WebControls.Label lblCurrentStatus;
        protected System.Web.UI.WebControls.Label lblInvoiceDate;
        protected System.Web.UI.WebControls.Label lblSupplier;
        protected System.Web.UI.WebControls.Label lblDepartment;
        protected System.Web.UI.WebControls.Label lblBuyer;
        protected System.Web.UI.WebControls.Label lblCRn;
        protected System.Web.UI.WebControls.Label lblcreditnoteno;
        protected System.Web.UI.WebControls.Label lblApprovelMessage;
        protected System.Web.UI.WebControls.DataGrid grdList;
        protected System.Web.UI.WebControls.Button btnAddNew;
        protected System.Web.UI.WebControls.Button btnDelLine;
        protected System.Web.UI.WebControls.Label lblErrorMsg;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.Button btnReject;
        #endregion

        #region  objects declarations
        protected CBSolutions.ETH.Web.Invoice objInvoice = new CBSolutions.ETH.Web.Invoice();
        protected Company objCompany = new Company();
        CreditNotes.Invoice_NL_CN objCN = new CBSolutions.ETH.Web.ETC.CreditNotes.Invoice_NL_CN();
        private ETC.User.Users objUser = new ETC.User.Users();
        #endregion

        #region  variables
        protected string AuthorisationStringToolTips = "";
        Invoice.Invoice objinvoice = new Invoice.Invoice();
        public int invoiceID = 0;
        protected int iApproverStatusID = 0;
        protected int iCurrentStatusID = 0;
        protected int TypeUser = 1;
        protected int UserTypeID = 1;
        protected int StatusUpdate = 0;
        protected int DocStatus = 0;
        protected string DocType = "INV";
        protected double dTotalAmount = 0;
        double dNetAmt = 0;
        string strComments = "";

        #endregion

        protected System.Web.UI.WebControls.Button btnApprove;
        protected System.Web.UI.WebControls.Button btndelete;
        protected System.Web.UI.WebControls.TextBox txtComment;
        protected System.Web.UI.WebControls.DropDownList ddldept;
        protected System.Web.UI.WebControls.Button btnOpen;
        protected System.Web.UI.WebControls.TextBox tbcreditnoteno;
        protected System.Web.UI.WebControls.Button btnEditAssociatedInvoiceNo;
        protected System.Web.UI.WebControls.Label lblBusinessUnit;
        protected System.Web.UI.WebControls.DataGrid grdFile;
        protected System.Web.UI.WebControls.Label lblMsg;
        protected System.Web.UI.WebControls.Button btnCancel;
        //protected System.Web.UI.WebControls.Button btnReject;
        protected System.Web.UI.HtmlControls.HtmlAnchor lnkVariance;


        #region Sql Variables
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            btnCancel.Attributes.Add("onclick", "javascript:window.close();");
            btnOpen.Attributes.Add("onclick", "javascript:return CheckOpenValid();");
            if (Request["DDCompanyID"] != null)
                ViewState["DDCompanyID"] = Request["DDCompanyID"].ToString();
            if (Request["DocType"] != null)
                ViewState["DocType"] = Request["DocType"].ToString();
            else
                ViewState["DocType"] = "CRN";

            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
                ViewState["InvoiceID"] = invoiceID.ToString();
                Session["eInvoiceID"] = invoiceID.ToString();
            }
            TypeUser = Convert.ToInt32(Session["UserTypeID"]);

            if (!Page.IsPostBack)
            {
                ViewState["approvalpath"] = "";
                if (invoiceID != 0)
                    GetDocumentDetails(invoiceID);

                GetDepartMentDropDwons();

                CheckInvoiceExist();

                if (TypeUser >= 2 && Convert.ToInt32(ViewState["StatusID"]) == 20)
                    lblDepartment.Visible = false;

                if (TypeUser < 2)
                {
                    tbcreditnoteno.Visible = false;
                    btnEditAssociatedInvoiceNo.Visible = false;
                }
                ShowFiles(Convert.ToInt32(Session["eInvoiceID"]));
                ButtonRejectVisibility();
            }


            if (Request.QueryString["NewVendorClass"] != null)
            {
                if (Convert.ToString(Request.QueryString["NewVendorClass"]).Trim() != "PO")
                {
                    lnkVariance.Visible = false;
                }
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEditAssociatedInvoiceNo.Click += new System.EventHandler(this.btnEditAssociatedInvoiceNo_Click);
            this.grdList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdList_ItemDataBound);
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            this.btnDelLine.Click += new System.EventHandler(this.btnDelLine_Click);
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            this.grdFile.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdFile_ItemCommand);
            this.grdFile.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdFile_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);


        }
        #endregion

        #region GetDocumentDetails(int iinvoiceID)
        private void GetDocumentDetails(int iinvoiceID)
        {
            DataSet DsInv = new DataSet();
            DsInv = GetDocumentDetails(iinvoiceID, "CRN");
            if (DsInv != null)
            {
                if (DsInv.Tables.Count > 0)
                {
                    if (DsInv.Tables[0].Rows.Count > 0)
                    {
                        lblRefernce.Text = DsInv.Tables[0].Rows[0]["InvoiceNo"].ToString();
                        lblInvoiceDate.Text = DsInv.Tables[0].Rows[0]["InvoiceDate"].ToString();
                        lblSupplier.Text = DsInv.Tables[0].Rows[0]["SupplierCompanyName"].ToString();
                        lblBuyer.Text = DsInv.Tables[0].Rows[0]["BuyerCompanyName"].ToString();

                        ViewState["SupplierCompanyID"] = DsInv.Tables[0].Rows[0]["SupplierCompanyID"].ToString();
                        ViewState["BuyerCompanyID"] = DsInv.Tables[0].Rows[0]["BuyerCompanyID"].ToString();

                        Session["BuyerCID"] = DsInv.Tables[0].Rows[0]["BuyerCompanyID"].ToString();

                        lblCurrentStatus.Text = DsInv.Tables[0].Rows[0]["Status"].ToString();
                        lblBusinessUnit.Text = Convert.ToString(DsInv.Tables[0].Rows[0]["BusinessUnit"]);
                        try
                        {
                            ViewState["approvalpath"] = DsInv.Tables[0].Rows[0]["ApprovalPath"].ToString();


                            lblcreditnoteno.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString(); ;
                            tbcreditnoteno.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString();

                            ViewState["AssociatedStatus"] = DsInv.Tables[0].Rows[0]["AssociatedStatus"].ToString();

                        }
                        catch { }

                        try
                        {
                            lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();
                            ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();
                        }
                        catch { }
                        ViewState["StatusID"] = DsInv.Tables[0].Rows[0]["StatusID"].ToString();
                        iCurrentStatusID = Convert.ToInt32(DsInv.Tables[0].Rows[0]["StatusID"]);
                        ViewState["OriginalNetAmount"] = DsInv.Tables[0].Rows[0]["Net"].ToString();
                        Session["eDocType"] = DsInv.Tables[0].Rows[0]["DocType"].ToString();
                    }
                }
            }
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

                //				if(TypeUser==1)
                //					dt= objCompany.GetCompanyListForPurchaseInvoiceLogGMG(Convert.ToInt32(Session["CompanyID"]),Convert.ToInt32(Session["UserID"]));	
                //				else
                dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataTextField = "CompanyName";
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataValueField = "CompanyID";
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "--Select--");

                GetAllComboCodesFirstTime();

                try
                {
                    if (Session["BuyerCID"] != null)
                        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = Session["BuyerCID"].ToString().Trim();
                }
                catch { }

                ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Clear();
                DataSet dsBusinessUnit = new DataSet();
                dsBusinessUnit = GetBusinessUnit(Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));
                if (dsBusinessUnit.Tables[0].Rows.Count > 0)
                {

                    ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataSource = dsBusinessUnit;
                    ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataTextField = "BusinessUnitName";
                    ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataValueField = "BusinessUnitID";
                    ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataBind();

                }
                ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");

                ((DropDownList)e.Item.FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                //((DropDownList)	e.Item.FindControl("ddlDepartment1")).Items.Insert(0,"--Select--");
                ((DropDownList)e.Item.FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                //((DropDownList)	e.Item.FindControl("ddlProject1")).Items.Insert(0,"--Select--");

                if (((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim() != "")
                {
                    dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim());
                }



                if (((TextBox)e.Item.FindControl("txtPoNumber")).Text != "")
                {
                    if (GetPONumberForSupplierBuyer(((TextBox)e.Item.FindControl("txtPoNumber")).Text) != "Y")
                    {

                        ((TextBox)e.Item.FindControl("txtPoNumber")).ForeColor = Color.Red;

                    }
                    else
                    {
                        ((TextBox)e.Item.FindControl("txtPoNumber")).ForeColor = Color.Black;
                    }
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

        #region CheckInvoiceExist
        private void CheckInvoiceExist()
        {
            int RowCnt = 1;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);

            SqlDataAdapter sqlDA = new SqlDataAdapter("ups_GetGenericCodingChange", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(Request.QueryString["InvoiceID"]));
            sqlDA.SelectCommand.Parameters.Add("@Type", "CRN");
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

                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")), ds.Tables[1].Rows[i]["BusinessUnitID"].ToString());

                if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex > 0)
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedIndex = -1;
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ds.Tables[1].Rows[i]["CodingDescriptionID"].ToString());

                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedIndex = -1;
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ds.Tables[1].Rows[i]["NominalCodeID"].ToString());

                    //					((DropDownList)	grdList.Items[i].FindControl("ddlProject1")).SelectedIndex=-1;
                    //					SetValueForCombo(((DropDownList) grdList.Items[i].FindControl("ddlProject1")),ds.Tables[1].Rows[i]["ProjectCodeID"].ToString());
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

        #region doAction  AND  takeAction
        private void takeAction(string docType, int ID, int iOperation)
        {
            SqlConnection sqlConn = new SqlConnection(ConsString);

            SqlCommand sqlCmd = new SqlCommand("UPD_UpdateStatus", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@docType", docType);
            sqlCmd.Parameters.Add("@ID", ID);
            sqlCmd.Parameters.Add("@Operation", iOperation);
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {

                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }
        private void doAction(int iActionType)
        {
            // for invoice only
            if (iActionType == 1)
            {
                takeAction("INV", Convert.ToInt32(Request["InvoiceID"]), 1);
            }
            else if (iActionType == 0)
            {
                takeAction("INV", Convert.ToInt32(Request["InvoiceID"]), 0);
            }
        }
        #endregion

        #region protected DataTable GetBlankTable(int iNoofRow)
        //		protected DataTable GetBlankTable(int iNoofRow)
        //		{
        //			DataTable tbl=null;
        //			int InvoiceID=0;
        //			double dtmpNetAmt=0;
        //			InvoiceID=Convert.ToInt32(Request["InvoiceID"]);
        //			dtmpNetAmt=GetNetAmt(InvoiceID);
        //			ViewState["NetAmt"]=dtmpNetAmt;
        //
        //			if(iNoofRow<=1)
        //			{
        //				tbl = new DataTable();
        //				DataRow nRow;
        //				tbl.Columns.Add("NetValue");
        //				for (int i = 0; i < iNoofRow; i++) 
        //				{
        //					nRow = tbl.NewRow();
        //					nRow["NetValue"]=dtmpNetAmt;
        //					tbl.Rows.Add(nRow);
        //				}
        //			}
        //			else
        //			{	
        //				
        //				DataSet ds=((DataSet) ViewState["populate"]);
        //				if(ds.Tables[1].Rows.Count > 0)
        //				{
        //					tbl = new DataTable();
        //					DataRow nRow;
        //					tbl.Columns.Add("NetValue");
        //					for (int i = 0; i < iNoofRow; i++) 
        //					{
        //						nRow = tbl.NewRow();
        //						nRow["NetValue"]=ds.Tables[1].Rows[i]["netvalue"];
        //						tbl.Rows.Add(nRow);
        //					}
        //				}
        //			}
        //			return tbl;
        //		}
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
                DataSet ds = ((DataSet)ViewState["populate"]);
                tbl = new DataTable();
                DataRow nRow;
                tbl.Columns.Add("NetValue");
                tbl.Columns.Add("PurOrderNo");
                for (int i = 0; i < iNoofRow; i++)
                {
                    nRow = tbl.NewRow();
                    nRow["NetValue"] = dtmpNetAmt;
                    nRow["PurOrderNo"] = ds.Tables[1].Rows[i]["PurOrderNo"];
                    tbl.Rows.Add(nRow);
                }
            }
            else
            {
                // here i need to write code for opening in edit mode
                DataSet ds = ((DataSet)ViewState["populate"]);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    tbl = new DataTable();
                    DataRow nRow;
                    tbl.Columns.Add("NetValue");
                    tbl.Columns.Add("PurOrderNo");
                    for (int i = 0; i < iNoofRow; i++)
                    {
                        nRow = tbl.NewRow();
                        nRow["NetValue"] = ds.Tables[1].Rows[i]["netvalue"];
                        nRow["PurOrderNo"] = ds.Tables[1].Rows[i]["PurOrderNo"];
                        tbl.Rows.Add(nRow);
                    }
                }
            }
            return tbl;
        }

        #endregion

        #region GetAllComboCodes
        private void GetAllComboCodes()
        {
            int compid = 0;
            DataTable dt = null;
            string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlNominalCode1 = "";
            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
                //ddlProject1=((DropDownList) grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
                ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

                if (compid != 0)
                {
                    //					if(TypeUser==1)
                    //						dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]),compid);
                    //					else	
                    //						dt= objInvoice.GetGridDepartmentList(compid);

                    DataSet ds = LoadDepartment(compid);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = ds;
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);
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
                else
                    Response.Write("<script>alert('Please select a company');</script>");
            }
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

        #region SelectedIndexChanged_ddlCodingDescription
        protected void SelectedIndexChanged_ddlCodingDescription(Object sender, System.EventArgs e)
        {
            string ddlDepartment1 = "", ddlNominalCode1 = "";
            int compid = 0;
            DataTable dt = new DataTable();
            DataSet dsDeptNom = new DataSet();
            DropDownList ddl = ((DropDownList)sender);
            int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
            if (TypeUser == 1)	//NORMAL USER
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue == "--Select--")
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
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

                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                DataSet ds = LoadDepartment(Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = ds;
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                }
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Items.Clear();
                DataSet dsBusinessUnit = new DataSet();
                dsBusinessUnit = GetBusinessUnit(Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));
                if (dsBusinessUnit.Tables[0].Rows.Count > 0)
                {

                    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).DataSource = dsBusinessUnit;
                    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).DataTextField = "BusinessUnitName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).DataValueField = "BusinessUnitID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).DataBind();

                }
                ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");
            }
            else
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");

            }

        }
        #endregion

        #region  SelectedIndexChanged_ddlNominalCode
        protected void SelectedIndexChanged_ddlNominalCode(object sender, System.EventArgs e)
        {

            int iddlDept = 0;
            int iNomin = 0;
            int compid = 0;
            string CodingDescription = "--Select--";
            //			string strProjectName="";
            //			string strProjectID="";

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
                    if (dsCODES.Tables.Count > 0 && dsCODES.Tables[0].Rows.Count > 0)
                    {
                        CodingDescription = dsCODES.Tables[0].Rows[0]["CodingDescriptionID"].ToString();
                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = CodingDescription;

                }
            }
        }
        #endregion

        #region GetAllComboCodesForNominalRefresh
        private void GetAllComboCodesForNominalRefresh()
        {
            int compid = 0;
            DataTable dt = null;
            string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlNominalCode1 = "";
            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
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
                else
                    Response.Write("<script>alert('Please select a company');</script>");
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

        #region GetAllComboCodesAddNew
        private void GetAllComboCodesAddNew()
        {
            int compid = 0;
            DataTable dt = null;
            string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlNominalCode1 = "";
            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {

                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
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
                        ddlNominalCode1 = dsDeptNom.Tables[1].Rows[0]["NominalCodeID"].ToString();//Sudhir on 08-07-2009
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dsDeptNom.Tables[1];
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);
                    }
                }

                else
                {

                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                }


            }
        }
        #endregion

        #region private void ShowFiles(int InvoiceID)
        private void ShowFiles(int InvoiceID)
        {
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            //sqlConn=new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetUploadFileDetailsCreditNote_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            grdFile.DataSource = ds.Tables[0];
            grdFile.DataBind();
            if (grdFile.Items.Count > 0)
            {
                lblMsg.Visible = false;
            }
            else
            {
                lblMsg.Visible = true;
            }

        }
        #endregion


        #region private void grdFile_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        private void grdFile_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (DataBinder.Eval(e.Item.DataItem, "archiveImagePath") != DBNull.Value)
                {
                    if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")) != "")
                    {
                        ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")).Trim());
                    }
                }
                else
                {
                    if (DataBinder.Eval(e.Item.DataItem, "ImagePath") != DBNull.Value)
                    {
                        if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")) != "")
                        {
                            ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")).Trim());
                        }
                        else
                        {
                            ((Label)e.Item.FindControl("lblPath")).Text = "N/A";
                        }
                    }
                }
            }
        }
        #endregion

        #region private void grdFile_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        private void grdFile_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            bool bFound = false;
            int DocumentID = 0;
            lblMsg.Text = "";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DocumentID = Convert.ToInt32(((Label)e.Item.FindControl("lblDocID")).Text);
                if (Convert.ToString(e.CommandArgument) == "DOW")
                {
                    string sDownLoadPath = ((Label)e.Item.FindControl("lblHidePath")).Text;
                    sDownLoadPath = sDownLoadPath.Replace("I:", "C:\\P2D");
                    sDownLoadPath = sDownLoadPath.Replace("\\90104-server2", "C:\\P2D");
                    sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
                    if (sDownLoadPath.Trim() != "")
                    {
                        if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                        {
                            try
                            {
                                bFound = ForceDownload(sDownLoadPath, 0);
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            bFound = ForceDownload(sDownLoadPath, 0);
                        }
                    }
                    if (bFound == false)
                    {
                        sDownLoadPath = ((Label)e.Item.FindControl("lblArchPath")).Text;
                        sDownLoadPath = sDownLoadPath.Replace("\\90107-server3", @"C:\File Repository");
                        sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);

                        if (sDownLoadPath.Trim() != "")
                        {
                            if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                            {
                                try
                                {

                                    bFound = ForceDownload(sDownLoadPath, 1);
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                bFound = ForceDownload(sDownLoadPath, 1);
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region private string GetTrimFirstSlash(string sVal)
        private string GetTrimFirstSlash(string sVal)
        {
            string sFName = sVal;
            if (sVal != "" & sVal != null)
            {

                string sInfo = sVal;
                sInfo.Replace(@"\", @"\\");
                string[] delValue = sInfo.Split(new char[] { '\\' });
                sFName = "";
                for (int x = 0; x < delValue.Length; x++)
                {
                    if (delValue[x] != "")
                    {
                        sFName = sFName + delValue[x];
                        if (x != delValue.Length - 1)
                        {
                            sFName = sFName + @"\";
                        }
                    }
                }
            }
            return sFName;
        }
        #endregion

        #region private bool ForceDownload(string sPath,int iStep)
        private bool ForceDownload(string sPath, int iStep)
        {
            bool bRetVal = false;
            string sFileName = sPath;
            if (iStep == 0)
            {
                System.IO.FileStream fs1 = null;
                try
                {
                    WEBRef.FileDownload objService = new WEBRef.FileDownload();
                    objService.Url = GetURL();
                    byte[] bytBytes = objService.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        Response.ContentType = "application//octet-stream";
                        Response.BinaryWrite(bytBytes);
                        Response.Flush();
                        Response.End();
                        fs1.Close();
                        fs1 = null;
                        bRetVal = true;
                    }
                }
                catch
                {
                }
            }
            else if (iStep == 1)
            {
                System.IO.FileStream fs1 = null;
                try
                {
                    WEBRef2.FileDownload objService2 = new WEBRef2.FileDownload();
                    objService2.Url = GetURL2();
                    byte[] bytBytes = objService2.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        Response.ContentType = "application//octet-stream";
                        Response.BinaryWrite(bytBytes);
                        Response.Flush();
                        Response.End();
                        fs1.Close();
                        fs1 = null;
                        bRetVal = true;
                    }
                }
                catch
                {

                }
            }
            return bRetVal;
        }
        #endregion

        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }

        private string GetURL2()
        {

            return ConfigurationManager.AppSettings["ServiceURLNew"];
        }

        #region btnAddNew_Click
        private void btnAddNew_Click(object sender, System.EventArgs e)
        {
            Populate(0, "a");
        }

        private void Populate(int irow, string acttype)
        {
            int i;
            int j;
            string[] strValue = new string[1];
            DataRow dr;
            ArrayList arrLstComp = new ArrayList();
            ArrayList arrLstCode = new ArrayList();
            ArrayList arrLstDept = new ArrayList();
            ArrayList arrLstNomi = new ArrayList();
            ArrayList arrLstBusinessUnit = new ArrayList();
            ArrayList PurchaseOrderNO = new ArrayList();
            DataSet ds = ((DataSet)(ViewState["data"]));
            ds.Tables[0].Rows.Clear();

            for (i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (irow == 0 && acttype == "a")
                {
                    arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);
                    arrLstCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedItem.Value);
                    arrLstDept.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedItem.Value);
                    arrLstNomi.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedItem.Value);
                    //arrLstProjCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))).SelectedItem.Value);
                    arrLstBusinessUnit.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))).SelectedItem.Value);
                    PurchaseOrderNO.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text);

                    strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
                    dr = ds.Tables[0].NewRow();
                    for (j = 0; j < 1; j++)
                    {
                        dr[j] = strValue[j].ToString();
                    }
                    ds.Tables[0].Rows.Add(dr);
                }
                else if (irow != i && acttype == "d")
                {
                    arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);
                    arrLstCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedItem.Value);
                    arrLstDept.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedItem.Value);
                    arrLstNomi.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedItem.Value);
                    //arrLstProjCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))).SelectedItem.Value);
                    arrLstBusinessUnit.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))).SelectedItem.Value);
                    PurchaseOrderNO.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text);


                    strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
                    dr = ds.Tables[0].NewRow();
                    for (j = 0; j < 1; j++)
                    {
                        dr[j] = strValue[j].ToString();
                    }
                    ds.Tables[0].Rows.Add(dr);
                }
            }
            if (irow == 0 && acttype == "a")
            {
                dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            ViewState["data"] = ds;
            BindGrid(1);

            dNetAmt = 0;

            i = 0;
            for (i = 0; i <= arrLstComp.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedIndex = -1;
                ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).Items.FindByValue(arrLstComp[i].ToString()).Selected = true;
            }
            if (acttype == "d")
                GetAllComboCodesForNominalRefresh();
            else
            {
                GetAllComboCodesFirstTime();
            }

            for (i = 0; i <= arrLstCode.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedIndex = -1;
                SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))), arrLstCode[i].ToString());
            }

            for (i = 0; i <= arrLstDept.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedIndex = -1;
                SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))), arrLstDept[i].ToString());
            }

            for (i = 0; i <= arrLstNomi.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedIndex = -1;
                SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))), arrLstNomi[i].ToString());
            }

            for (i = 0; i <= arrLstBusinessUnit.Count - 1; i++)
            {
                SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))), arrLstBusinessUnit[i].ToString());
            }

            for (i = 0; i <= PurchaseOrderNO.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text = PurchaseOrderNO[i].ToString();
                string PurchaseorderNo = "";
                System.Web.UI.WebControls.TextBox txtPoNumber = (System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtPoNumber");
                PurchaseorderNo = txtPoNumber.Text;

                if (((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text != "")
                {
                    if (GetPONumberForSupplierBuyer(PurchaseorderNo.Trim()) != "Y")
                    {
                        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).ForeColor = Color.Red;
                    }
                    else
                    {
                        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).ForeColor = Color.Black;
                    }
                }
            }



        }
        #endregion

        #region btnDelLine_Click
        private void btnDelLine_Click(object sender, System.EventArgs e)
        {
            int i = 0;
            DataSet ds = ((DataSet)(ViewState["data"]));

            string numbers = "";
            int LineNo = 0;

            for (i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (((System.Web.UI.WebControls.CheckBox)grdList.Items[i].FindControl("chkBox")).Checked)
                {
                    numbers += grdList.Items[i].ItemIndex + "/";

                    LineNo = Convert.ToInt32(grdList.Items[i].Cells[0].Text);
                }
            }
            string[] strArr = numbers.Split('/');
            DataSet ds1 = ((DataSet)(ViewState["data"]));
            for (int j = 0; j <= strArr.Length - 1; j++)
            {
                ds1 = ((DataSet)(ViewState["data"]));

                if (strArr[j] != "" && ds1.Tables[0].Rows.Count - 1 > 0)
                {
                    if (Convert.ToInt32(strArr[j]) <= ds1.Tables[0].Rows.Count - 1)
                    {
                        ds1.Tables[0].Rows[Convert.ToInt32(strArr[j])].Delete();

                        Populate(Convert.ToInt32(strArr[j]), "d");


                    }
                    else if (ds1.Tables[0].Rows.Count != 1)
                    {
                        int iPass = 0;
                        iPass = ds1.Tables[0].Rows.Count - 1;
                        ds1.Tables[0].Rows[ds1.Tables[0].Rows.Count - 1].Delete();

                        Populate(iPass, "d");
                    }
                }
            }
        }
        #endregion

        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            //			int iRet = 0;
            bool ret = SaveDetailData();
            if (ret == true)
            {
                //int InvID = Convert.ToInt32(Session["eInvoiceID"]);
                int ival = ChangeStatus(Convert.ToInt32(Session["eInvoiceID"]), Convert.ToString(txtComment.Text), 19);
                if (ival > 0)
                {
                    lblErrorMsg.Text = "Record(s) saved successfully.";
                    Response.Write("<script>alert('Record(s) saved successfully.');</script>");
                    //					Response.Write("<script>opener.location.reload(true);</script>");
                    Response.Write("<script>self.close();</script>");
                    Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");
                }
            }
        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            //			Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");
            Response.Write("<script>self.close();</script>");
            Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");
        }
        #endregion

        #region SaveDetailData()
        private bool SaveDetailData()
        {
            #region variables
            int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            int CompanyID = 0;
            int CodingDescriptionID = 0;
            int NominalCodeID = 0;
            int BusinessUnitID = 0;
            int DepartmentID = 0;
            int iValidFlag = 0;
            decimal NetValue = 0;
            double NetVal = 0;
            string PurOrderNo = "";
            bool retval = false;
            lblErrorMsg.Visible = false;
            #endregion

            for (int j = 0; j <= grdList.Items.Count - 1; j++)
            {
                if (grdList.Items[j].ItemType == ListItemType.Item || grdList.Items[j].ItemType == ListItemType.AlternatingItem)
                {
                    #region Validations
                    if (((DropDownList)grdList.Items[j].FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim() == "--Select--")
                    {
                        Response.Write("<script>alert('Please select company code.');</script>");
                        iValidFlag = 1;
                        break;
                    }
                    if (((DropDownList)grdList.Items[j].FindControl("ddlCodingDescription1")).SelectedValue.Trim() == "--Select--")
                    {
                        Response.Write("<script>alert('Please select coding.');</script>");
                        iValidFlag = 1;
                        break;
                    }
                    if (((DropDownList)grdList.Items[j].FindControl("ddlDepartment1")).SelectedValue.Trim() == "--Select--")
                    {
                        Response.Write("<script>alert('Please select coding.');</script>");
                        iValidFlag = 1;
                        break;
                    }
                    if (((DropDownList)grdList.Items[j].FindControl("ddlNominalCode1")).SelectedValue.Trim() == "--Select--")
                    {
                        Response.Write("<script>alert('Please select nominal code.');</script>");
                        iValidFlag = 1;
                        break;
                    }

                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[j].FindControl("txtNetVal")).Text.Trim() == "")
                    {
                        Response.Write("<script>alert('Please enter Net Value for coding at line(s).');</script>");
                        iValidFlag = 1;
                        break;
                    }
                    //po validation start Sougata
                    if (Convert.ToString(Request.QueryString["NewVendorClass"]).Trim() == "PO")
                    {

                        if (GetPONumberForSupplierBuyer(((System.Web.UI.WebControls.TextBox)grdList.Items[j].FindControl("txtPoNumber")).Text) != "Y")
                        {
                            Response.Write("<script>alert('Invalid PO Number entered');</script>");
                            iValidFlag = 1;
                            break;
                        }

                    }
                    // po validation end Sougata
                    #endregion
                }
            }
            if (iValidFlag == 1)
            {
                return false;
            }
            else
            {
                //				for(int i=0;i<=grdList.Items.Count-1;i++)
                //				{
                //					if(grdList.Items[i].ItemType==ListItemType.Item || grdList.Items[i].ItemType==ListItemType.AlternatingItem)
                //					{
                //						NetVal=NetVal+Convert.ToDouble(((System.Web.UI.WebControls.TextBox) grdList.Items[i].FindControl("txtNetVal")).Text);
                //					}
                //				}

                // Added by Mrinal on 19th February 2014
                decimal NetValConvert = 0.00m;
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {
                    if (grdList.Items[i].ItemType == ListItemType.Item || grdList.Items[i].ItemType == ListItemType.AlternatingItem)
                    {
                        NetValConvert = Convert.ToDecimal(NetValConvert + Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text));

                        //NetVal=NetVal+Convert.ToDouble(((System.Web.UI.WebControls.TextBox) grdList.Items[i].FindControl("txtNetVal")).Text);
                        // Added by Mrinal on 19th February 2014
                        NetVal = decimal.ToDouble(NetValConvert);
                        //NetVal= (double) NetValConvert ;

                    }
                }

                DataSet dsXML = new DataSet();
                DataTable dtXML = new DataTable();
                dtXML.Columns.Add("SlNo");
                dtXML.Columns.Add("InvoiceID");
                dtXML.Columns.Add("InvoiceType");
                dtXML.Columns.Add("CompanyID");
                dtXML.Columns.Add("CodingDescriptionID");
                dtXML.Columns.Add("DepartmentID");
                dtXML.Columns.Add("NominalCodeID");
                dtXML.Columns.Add("BusinessUnitID");
                dtXML.Columns.Add("NetValue");
                dtXML.Columns.Add("CodingValue");
                dtXML.Columns.Add("PurOrderNo");//ss				
                DataRow DR = null;

                StringBuilder sb = new StringBuilder();
                sb.Append("<Root>");
                if (Convert.ToDouble(ViewState["OriginalNetAmount"]) == NetVal)
                {
                    for (int i = 0; i <= grdList.Items.Count - 1; i++)
                    {
                        #region Getting DropDown Values
                        if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
                        {
                            CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                        }

                        if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue) > 0)
                        {
                            CodingDescriptionID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue);
                        }

                        if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue) > 0)
                        {
                            DepartmentID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                        }

                        if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue) > 0)
                        {
                            NominalCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);
                        }

                        if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
                        {
                            BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
                        }

                        PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);

                        NetValue = 0;
                        if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
                        {
                            if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
                            {
                                NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                            }
                        }
                        #endregion

                        if (NetValue > 0)
                        {
                            DR = dtXML.NewRow();
                            dtXML.Rows.Add(DR);
                            sb.Append("<Rowss>");
                            sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
                            sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
                            sb.Append("<InvoiceType>").Append("CRN").Append("</InvoiceType>");
                            sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
                            sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
                            sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
                            sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
                            if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
                                sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
                            else
                                sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</BusinessUnitID>");

                            sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
                            sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
                            sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");

                            sb.Append("</Rowss>");
                        }
                    }
                    dsXML.Tables.Add(dtXML);
                    sb.Append("</Root>");
                    string strXmlText = sb.ToString();
                    sb = null;


                    int retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
                    if (retvalalue > 0)
                        retval = true;
                    else
                        retval = false;
                }
                else
                {
                    lblErrorMsg.Visible = false;
                    Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
                }
            }
            return retval;
        }
        #endregion

        #region  public DataSet GetDocumentDetails(int InvoiceID,string Type)
        public DataSet GetDocumentDetails(int InvoiceID, string Type)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("GetDocumentDetails_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@Type", Type);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return ds;
        }
        #endregion

        #region GetInvoiceBuyerCompanyID
        public int GetInvoiceBuyerCompanyID(int iInvoiceID)
        {
            int BuyerID = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT BuyerCompanyID,SupplierCompanyID FROM CreditNote WHERE InvoiceID=" + iInvoiceID;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables.Count > 0)
                    BuyerID = Convert.ToInt32(Dst.Tables[0].Rows[0]["BuyerCompanyID"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return BuyerID;
        }
        #endregion

        #region GetNetAmt(int InvoiceID)
        private double GetNetAmt(int InvoiceID)
        {
            double NetAmt = 0;
            string sSql = "select nettotal from CreditNote where CreditNoteid=" + InvoiceID;
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

        #region GetCodingDescriptionAgainstDepartmentANDNominal
        public DataSet GetCodingDescriptionAgainstDepartmentANDNominal(int iDepartmentCodeID, int iNominal, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE isnull(APAdminOnly,0) <> 1 AND DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + "";
            else
                sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + "";
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

        #region GetProjectCodeAgainstDepartmentANDNominal
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

        #region GetDepartmentANDNominalAgainstCodingDescID(int iCodingID,int iCompID)
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

        #region GetNominalCodeAgainstDepartmentANDCompany
        public DataSet GetNominalCodeAgainstDepartmentANDCompany(int iDepartmentCodeID, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in(SELECT NominalCodeID FROM CodingDescription WHERE isnull(APAdminOnly,0)<>1 and DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ")";
            else
                sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in(SELECT NominalCodeID FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ")";
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

        #region InsertCodingChangeValuesByDeleting
        public int InsertCodingChangeValuesByDeleting(string XMLText, int invoiceID)
        {
            int RetVal = 0;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);

            try
            {
                sqlCmd = new SqlCommand("sp_InsertCodingChangeValuesByDeleting_Akkeron", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@CodingChangeXml", XMLText);
                sqlCmd.Parameters.Add("@InvoiceID", invoiceID);
                sqlConn.Open();
                RetVal = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); RetVal = -1; }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (RetVal);
        }
        #endregion
        private int ChangeStatus(int CreditNoteID, string Comments, int ApproverStatus)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            sqlCmd = new SqlCommand("Sp_UpdateStatusCreditNote_GRH", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"]));
            sqlCmd.Parameters.Add("@CreditNoteID", CreditNoteID);
            sqlCmd.Parameters.Add("@Comments", Comments);
            sqlCmd.Parameters.Add("@ApproverStatus", ApproverStatus);
            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string dd = ex.Message.ToString(); iCount = -1; }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iCount);
        }
        #region ChangeStatusOFCreditNote(int invoiceID,int StatusID,string Comments )
        public int ChangeStatusOFCreditNote(int invoiceID, int StatusID, string Comments)
        {
            int RetVal = 0;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);

            try
            {
                sqlCmd = new SqlCommand("sp_ChangeStatusOFCreditNote", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@CreditNoteID", invoiceID);
                sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"].ToString()));
                sqlCmd.Parameters.Add("@Status", StatusID);
                sqlCmd.Parameters.Add("@Comments", Comments);
                sqlConn.Open();
                RetVal = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); RetVal = -1; }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (RetVal);
        }
        #endregion

        #region btnApprove_Click
        private void btnApprove_Click(object sender, System.EventArgs e)
        {
            for (int j = 0; j <= grdList.Items.Count - 1; j++)
            {
                if (((DropDownList)grdList.Items[j].FindControl("ddlDepartment1")).SelectedValue.Trim() == "--Select--")
                {
                    Response.Write("<script>alert('Please select coding.');</script>");
                    return;
                }
            }
            if (Convert.ToString(ViewState["AssociatedStatus"]) != "" && Convert.ToInt32(ViewState["AssociatedStatus"]) != 19 && Convert.ToInt32(ViewState["AssociatedStatus"]) != 23)
            {
                Response.Write("<script>alert('The associated invoice must be in Approved or Paid status.');</script>");
                return;
            }
            bool ret = SaveDetailData();
            if (ret == true)
            {
                iApproverStatusID = 19;
                int UpdateStatus = UpdateSTOCKCreditNoteCOMMON_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), strComments, 19);
                if (UpdateStatus >= 1)
                {
                    doAction(0);
                    Page.RegisterStartupScript("Reg", "<script>ApproveClose();</script>");

                }
                else if (UpdateStatus == -1)
                    Response.Write("<script>alert('Error in approving.');</script>");
            }

        }
        #endregion

        #region UpdateSTOCKCreditNoteCOMMON_Generic
        public int UpdateSTOCKCreditNoteCOMMON_Generic(int CreditNoteID, string Comments, int ApproverStatus)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            sqlCmd = new SqlCommand("stp_UpdateSTOCKCreditNoteCOMMON_Generic_GRH", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"]));
            sqlCmd.Parameters.Add("@CreditNoteID", CreditNoteID);
            sqlCmd.Parameters.Add("@Comments", Comments);
            sqlCmd.Parameters.Add("@ApproverStatus", ApproverStatus);
            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string dd = ex.Message.ToString(); iCount = -1; }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iCount);
        }
        #endregion

        #region btndelete_Click
        private void btndelete_Click(object sender, System.EventArgs e)
        {
            if (txtComment.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter a comment.');</script>");
                return;
            }
            else
            {
                Invoice.Invoice objinvoice = new Invoice.Invoice();

                lblErrorMsg.Text = "";
                lblErrorMsg.Visible = false;
                iApproverStatusID = 7;
                string strComments = txtComment.Text.Trim();
                int DeptUpdate = UpdateDepartmentAgainstCreditNoteID();
                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                int StatusUpdate = objinvoice.UpdateCrnStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                if (StatusUpdate == 1)
                {

                    doAction(0);
                    Page.RegisterStartupScript("Reg", "<script>CaptureClose();</script>");
                    //					Response.Write("<script>alert('Invoice Deleted Successfully.');</script>");	
                    //					Response.Write("<script>self.close();</script>");
                    //					Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");

                }
                else
                {
                    Response.Write("<script>alert('Invoice cannot be deleted');</script>");
                }
            }
        }
        #endregion

        #region UpdateDepartmentAgainstCreditNoteID()
        private int UpdateDepartmentAgainstCreditNoteID()
        {
            int DeptID = 0;
            if (Convert.ToInt32(ddldept.SelectedIndex) > 0)
                DeptID = Convert.ToInt32(ddldept.SelectedValue);
            string strcomments = txtComment.Text.Trim();

            int iretval = 0;
            string sSql = "";
            if (DeptID > 0)
            {
                sSql = "UPDATE creditnote SET departmentid =" + DeptID + " ,Comment ='" + strcomments + "' WHERE creditnoteID =" + Convert.ToInt32(Session["eInvoiceID"]);
            }
            else
            {
                sSql = "UPDATE creditnote SET Comment ='" + strcomments + "' WHERE creditnoteID =" + Convert.ToInt32(Session["eInvoiceID"]);

            }
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

        #region btnEditAssociatedInvoiceNo_Click
        private void btnEditAssociatedInvoiceNo_Click(object sender, System.EventArgs e)
        {
            string strMessage = "";
            int RetVal = 0;
            string sQuery = "";
            string sCreditNoteId = "";
            SqlCommand sqlCmd = null;
            RetVal = IsValiedAssociatedInvoiceNo();
            if (RetVal == -101)
            {
                strMessage = "Invalid Invoice Entered.";
            }
            else if (RetVal == -102)
                strMessage = "INVOICE DOES NOT EXISTS.";
            else if (RetVal == 0)
                strMessage = "Please enter invoice no.";
            if (strMessage == "")
            {
                RetVal = 0;
                SqlConnection sqlConn = new SqlConnection(ConsString);
                try
                {
                    if (ViewState["InvoiceID"] != null)
                        sCreditNoteId = ViewState["InvoiceID"].ToString();
                    sQuery = "update CreditNote set CreditInvoiceNo='" + tbcreditnoteno.Text.Trim() + "' where CreditNoteid=" + sCreditNoteId;
                    sqlCmd = new SqlCommand(sQuery, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlConn.Open();
                    RetVal = sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); RetVal = -1; }
                finally
                {
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
                if (RetVal > 0)
                {
                    Response.Write("<script>alert('Associated Invoice No Updated Successfully.');</script>");
                    GetDocumentDetails(invoiceID);
                }
            }
            else
                Response.Write("<script>alert('" + strMessage + "');</script>");
        }
        #endregion

        #region IsValiedAssociatedInvoiceNo
        public int IsValiedAssociatedInvoiceNo()
        {
            int RetVal = 0;
            int ibuyerComId = 0;
            int iSuppliercomId = 0;
            Invoice_CN objInvoice_CN = new Invoice_CN();
            try
            {
                if (ViewState["SupplierCompanyID"] != null && ViewState["BuyerCompanyID"] != null)
                {
                    ibuyerComId = Convert.ToInt32(ViewState["BuyerCompanyID"].ToString());
                    iSuppliercomId = Convert.ToInt32(ViewState["SupplierCompanyID"].ToString());
                    if (tbcreditnoteno.Text != "")
                    {
                        RetVal = objInvoice_CN.CheckInvoiceExistsAgainstCompanyIDSAndInvoiceNo(tbcreditnoteno.Text.Trim(), ibuyerComId, iSuppliercomId);
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            return RetVal;
        }
        #endregion

        #region GetDepartMentDropDwons()
        public void GetDepartMentDropDwons()
        {
            CBSolutions.ETH.Web.ETC.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.ETC.ApprovalPath.Approval();
            DataSet ds = new DataSet();
            string Fields = "DepartmentID , Department";
            string Table = "Department";
            string Criteria = "BuyerCompanyID = " + System.Convert.ToInt32(Session["BuyerCID"]);
            ds = objApproval.GetGlobalDropDowns(Fields, Table, Criteria);
            ddldept.DataSource = ds;
            ddldept.DataBind();
            ddldept.Items.Insert(0, "Select");
        }
        #endregion

        #region btnOpen_Click
        private void btnOpen_Click(object sender, System.EventArgs e)
        {
            if (ddldept.SelectedItem.Text != "Select")
            {
                bool ret = SaveDetailDataForGMG();
                int i = SetDropDownValuesOnOpen_CRN(System.Convert.ToInt32(Session["UserID"].ToString()), System.Convert.ToInt32(ddldept.SelectedValue));
                int DeptUpdate = UpdateDepartmentAgainstCreditNoteID();
                Response.Write("<script>alert('Credit Note opened successfully.');</script>");
                Response.Write("<script>self.close();</script>");
                Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");


            }
            else
                Response.Write("<script>alert('Department has not been added.');</script>");
        }
        #endregion

        #region SetDropDownValuesOnOpen_CRN
        private int SetDropDownValuesOnOpen_CRN(int iUserID, int DepartmentID)
        {
            //			string NewApprover1 ="";
            //			string NewApprover2 ="";
            //			string NewApprover3 ="";
            //			string NewApprover4 ="";
            //			string NewApprover5 ="";

            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnOpen_CRN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            sqlCmd.Parameters.Add("@UserID", iUserID);
            if (txtComment.Text == "")
                sqlCmd.Parameters.Add("@Comment", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());

            sqlCmd.Parameters.Add("@Description", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
            sqlCmd.Parameters.Add("@Department", DepartmentID);
            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iReturnValue = -1; }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region SaveDetailDataForGMG()
        private bool SaveDetailDataForGMG()
        {
            #region variables
            int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            int CompanyID = 0;
            int CodingDescriptionID = 0;
            int NominalCodeID = 0;
            int BusinessUnitID = 0;
            int DepartmentID = 0;
            //			int iValidFlag=0;
            decimal NetValue = 0;
            bool flag = false;
            double NetVal = 0;
            bool retval = false;
            lblErrorMsg.Visible = false;
            string PurOrderNo = "";
            #endregion

            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (grdList.Items[i].ItemType == ListItemType.Item || grdList.Items[i].ItemType == ListItemType.AlternatingItem)
                {
                    NetVal = NetVal + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                }
            }
            DataSet dsXML = new DataSet();
            DataTable dtXML = new DataTable();
            dtXML.Columns.Add("SlNo");
            dtXML.Columns.Add("InvoiceID");
            dtXML.Columns.Add("InvoiceType");
            dtXML.Columns.Add("CompanyID");
            dtXML.Columns.Add("CodingDescriptionID");
            dtXML.Columns.Add("DepartmentID");
            dtXML.Columns.Add("NominalCodeID");
            dtXML.Columns.Add("BusinessUnitID");
            dtXML.Columns.Add("NetValue");
            dtXML.Columns.Add("CodingValue");
            dtXML.Columns.Add("PurOrderNo");
            DataRow DR = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            if (Convert.ToDouble(ViewState["OriginalNetAmount"]) == NetVal)
            {
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {
                    retval = true;
                    #region Getting DropDown Values
                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) != "--Select--")
                    {
                        CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                    }
                    else
                        retval = false;

                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue) != "--Select--")
                    {
                        CodingDescriptionID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue);
                    }
                    else
                        retval = false;

                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue) != "--Select--")
                    {
                        DepartmentID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                    }
                    else
                        retval = false;

                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue) != "--Select--")
                    {
                        NominalCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);
                    }
                    else
                        retval = false;

                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
                    {

                        BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
                    }
                    PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);

                    NetValue = 0;
                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim() != "")
                    {
                        if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
                        {
                            NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                        }
                        else
                            retval = false;
                    }
                    else
                        retval = false;
                    #endregion

                    if (NetValue > 0 && retval == true)
                    {
                        flag = true;
                        DR = dtXML.NewRow();
                        dtXML.Rows.Add(DR);
                        sb.Append("<Rowss>");
                        sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
                        sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
                        sb.Append("<InvoiceType>").Append("CRN").Append("</InvoiceType>");
                        sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
                        sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
                        sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
                        sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
                        if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
                            sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
                        else
                            sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</ProjectCodeID>");

                        sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
                        sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
                        sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");

                        sb.Append("</Rowss>");
                    }
                }
                dsXML.Tables.Add(dtXML);
                sb.Append("</Root>");
                string strXmlText = sb.ToString();
                sb = null;
                int retvalalue = 0;
                if (flag == true)
                    retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
            }
            else
            {
                flag = false;
                lblErrorMsg.Visible = false;
                Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
            }
            return flag;
        }
        #endregion

        private DataSet GetBusinessUnit(int companyid)
        {

            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetBusinessUnit", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(companyid));
            sqlDA.SelectCommand.Parameters.Add("@UsrID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
            }
            return ds;
        }

        #region GetAllComboCodesFirstTime
        private void GetAllComboCodesFirstTime()
        {
            int compid = 0;
            DataTable dt = null;
            string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlNominalCode1 = "";
            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
                //ddlProject1=((DropDownList) grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
                ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

                if (compid != 0)
                {
                    //					if(TypeUser==1)
                    //						dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]),compid);
                    //					else
                    //						dt= objInvoice.GetGridDepartmentList(compid);

                    DataSet ds = LoadDepartment(compid);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = ds;
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);
                    }
                    int iddlDept = 0;
                    int iNomin = 0;
                    string CodingDescription = "--Select--";
                    //					string strProjectName="";
                    //					string strProjectID="";
                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue) != "--Select--")
                    {
                        if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--")
                        {

                            iddlDept = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                            iNomin = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);

                            DataSet dsCODES = new DataSet();
                            dsCODES = GetCodingDescriptionAgainstDepartmentANDNominal(iddlDept, iNomin, compid);
                            if (dsCODES.Tables.Count > 0 && dsCODES.Tables[0].Rows.Count > 0)
                            {
                                CodingDescription = dsCODES.Tables[0].Rows[0]["CodingDescriptionID"].ToString();

                            }
                            ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = CodingDescription;

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
                else
                    Response.Write("<script>alert('Please select a company');</script>");
            }
        }
        #endregion
        private DataSet LoadDepartment(int companyid)
        {
            //ddldept.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentList_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(companyid));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataBind();
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                //ds=null;
            }
            //ddldept.Items.Insert(0, new ListItem("Select Department", "0"));
            return ds;
        }


        #region: Add by Mrinal on 5th March 2013
        private void btnReject_Click(object sender, EventArgs e)
        {
            if (!(txtComment.Text.Trim().Length > 0))
            {
                Response.Write("<script>alert('Please enter comments.');</script>");
                return;
            }
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            int CreditNoteID = 0;
            if (Session["eInvoiceID"] != null)
            {
                CreditNoteID = System.Convert.ToInt32(Session["eInvoiceID"]);
            }
            if (CreditNoteID != 0 && Session["eInvoiceID"] != null)
            {
                try
                {
                    //sqlCmd = new SqlCommand("sp_SetCreditNoteStatus", sqlConn);
                    sqlCmd = new SqlCommand("sp_SetCreditNoteStatus_BBR", sqlConn);

                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add("@CreditNoteID", CreditNoteID);
                    sqlCmd.Parameters.Add("@Comments", txtComment.Text);
                    if (Session["UserID"] == null)
                    {
                        sqlCmd.Parameters.Add("@UserID", DBNull.Value);
                    }
                    else
                    {
                        sqlCmd.Parameters.Add("@UserID", Convert.ToString(Session["UserID"]));
                    }
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();

                    Response.Write("<script>alert('Credit Note rejected successfully.');</script>");
                    Response.Write("<script>windowclose();</script>");
                    Response.Write("<script>self.close();</script>");

                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();

                }
                finally
                {
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
            }

        }

        private void ButtonRejectVisibility()
        {
            int TypeUser = Convert.ToInt32(Session["UserTypeID"]);
            if (TypeUser == 1)
            {

                if (Convert.ToInt32(ViewState["StatusID"]) == 21)  // When Registered
                {
                    btnReject.Visible = true;
                }

                else
                {
                    btnReject.Visible = false;
                }

            }
            else
            {
                btnReject.Visible = false;
            }

        }
        #endregion



        private string GetPONumberForSupplierBuyer(string Ponumber)
        {
            string existscheck = "";
            int invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter("sp_PoNumberForSupplerAnainstBuyerAkkeron", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", invoiceID);
            sqlDA.SelectCommand.Parameters.Add("@PoNumber", Ponumber);
            sqlDA.SelectCommand.Parameters.Add("@Type", "CRN");


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
            if (Dst.Tables.Count > 0)
            {
                if (Dst.Tables[0].Rows.Count > 0)
                {
                    existscheck = Convert.ToString(Dst.Tables[0].Rows[0]["Exists"]);
                }
            }
            return existscheck;

        }




    }
}
