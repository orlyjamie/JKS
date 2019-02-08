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

namespace JKS
{
    public partial class ETC_History_ActionHistoryNew : CBSolutions.ETH.Web.ETC.VSPage
    {

        #region Webcontrols
        //protected System.Web.UI.WebControls.Label lblConfirmation;
        //protected System.Web.UI.WebControls.Label lblRefernce;
        //protected System.Web.UI.WebControls.Label lblCurrentStatus;
        //protected System.Web.UI.WebControls.Label lblInvoiceDate;
        protected System.Web.UI.WebControls.Label lblApprovalPath;
        // protected System.Web.UI.WebControls.Label lblSupplier;
        // protected System.Web.UI.WebControls.Label lblDepartment;
        // protected System.Web.UI.WebControls.Label lblBuyer;
        // protected System.Web.UI.WebControls.Label lblCRn;
        // protected System.Web.UI.WebControls.Label lblcreditnoteno;
        // protected System.Web.UI.WebControls.Label lblApprovelMessage;
        //// protected System.Web.UI.WebControls.DataGrid grdList;
        // protected System.Web.UI.WebControls.Repeater grdList;
        // protected System.Web.UI.WebControls.TextBox txtComment;
        // protected System.Web.UI.WebControls.Button btndelete;
        // protected System.Web.UI.WebControls.Label lblErrorMsg;
        // protected System.Web.UI.WebControls.Button btnCancel;
        // protected System.Web.UI.WebControls.Label lblNominal;
        // protected System.Web.UI.WebControls.Label lblBusinessUnit;


        // protected System.Web.UI.WebControls.Label lblNetInvoiceTotal;
        // protected System.Web.UI.WebControls.Label lblNetVal;

        #endregion
        #region  objects declarations
        protected CBSolutions.ETH.Web.Invoice objInvoice = new CBSolutions.ETH.Web.Invoice();
        // added by kd
        public int ChkUserID = 0;
        protected Company objCompany = new Company();
        #endregion
        #region  variables
        protected string AuthorisationStringToolTips = "";
        JKS.Invoice objinvoice = new JKS.Invoice();
        public int invoiceID = 0;
        protected int iApproverStatusID = 0;
        protected int iCurrentStatusID = 0;
        protected int TypeUser = 1;
        protected int UserTypeID = 1;
        protected int StatusUpdate = 0;
        protected int DocStatus = 0;
        protected string DocType = "";
        protected double dTotalAmount = 0;
        double dNetAmt = 0;
        double dCodingVat = 0;
        string strComments = "";
        protected int iSupplierCompanyID = 0;
        #endregion
       
        #region Sql Variables
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

        SqlConnection sqlConn;
        SqlCommand sqlCmd;
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");
            btnCancel.Attributes.Add("onclick", "javascript:window.close();");

            if (Request["DocStatus"] != null)
                ViewState["DocStatus"] = Request["DocStatus"].ToString();
            if (Request["DocType"] != null)
                ViewState["DocType"] = Request["DocType"].ToString();

            DocType = Request["DocType"].ToString();
            //added by rimi on 17thJuly2015
            if (DocType == "INV")
            {
                lblConfirmation.Text = "Invoice History";
            }
            else if (DocType == "CRN")
            {
                lblConfirmation.Text = "Credit Note History";
            }
            //-------------------------------
            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
                ViewState["InvoiceID"] = invoiceID.ToString();
                Session["eInvoiceID"] = invoiceID.ToString();
                Session["InvoiceBuyerCompany"] = GetInvoiceBuyerCompanyID(Convert.ToInt32(ViewState["InvoiceID"]));
            }
            TypeUser = Convert.ToInt32(Session["UserTypeID"]);

            //Added By Kd 07.12.2018
            JKS.Invoice objInvoice = new JKS.Invoice();
           ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            dgSalesCallDetails_INV.CurrentPageIndex = 0;
            dgSalesCallDetails.CurrentPageIndex = 0;
            if (!Page.IsPostBack)
            {

                // Added by Mrinal on 29th December 2014
                dNetAmt = 0;
                if (invoiceID != 0)
                {
                    GetDocumentDetails(invoiceID);
                    // Blocked by Mrinal on 28th March 2015
                    /*
                    string strStatusLogLink = GetInvoiceStatusLog();
                    strStatusLogLink = GetInvoiceStatusLog();
                    iframeInvoiceStatusLog.Attributes.Add("src", strStatusLogLink);
                     */
                    string strStatusLogLink = GetInvoiceStatusLog();
                    strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:545,height:410,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                    
                    //Blocked By Kd 07.12.2018
                    //aInvoiceStatusLog.Attributes.Add("onclick", strStatusLogLink);

                    InvoiceCrnIsDuplicate();
                    IsAutorisedtoEditData();
                }

                CheckInvoiceExist();
            }
            if (TypeUser >= 2)
                btndelete.Visible = true;
            else
                btndelete.Visible = false;

            if (Convert.ToString(ViewState["DocStatus"]) == "Delete/Archive")
                btndelete.Visible = false;


            //added by kuntalkarar on 6thJanuary2017
            if (Request.QueryString["NewVendorClass"] != null)
            {
                if (Convert.ToString(Request.QueryString["NewVendorClass"]).Trim() != "PO")
                {
                    //lnkVariance.Visible = false;
                }
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            GetVatAmount();
            CalculateTotal();
            DataSet ds = GetDocumentDetails(invoiceID, Convert.ToString(ViewState["DocType"]));
            Boolean Duplicate = Convert.ToBoolean(ds.Tables[0].Rows[0]["Duplicate"]);
            if (Duplicate == false)
            {
                lblDuplicate.Visible = false;
            }
            else
            {
                lblDuplicate.Visible = true;
            }

        }


        //Added by KD , 07.12.2018
        protected void Popup_Click(object sender, EventArgs e)
        {
            string strDocType = Convert.ToString(Request.QueryString["DocType"]);

            string strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);


            if (strDocType.Trim() == "CRN")
            {

                mpe.Show();
                this.GetInvoiceStatusDetails_CRN(Convert.ToInt32(invoiceID));

            }
            else
            {
                mpe.Show();
                this.GetInvoiceStatusDetails_INV(Convert.ToInt32(invoiceID));
            }

        }

        //Added By Kd 07.12.2018
       // private int ChkUserID = 0;
        private DataTable dtbl = new DataTable();

        //Added By Kd 07.12.2018
        #region GetInvoiceStatusDetails
        private void GetInvoiceStatusDetails_CRN(int iInvoiceID)
        {
            JKS.Invoice objInvoice = new JKS.Invoice();
            //lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "CRN");
            //lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "CRN");
            //lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "CRN");

           // ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            else
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            //	dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier_CN(iInvoiceID);


            if (dtbl.Rows.Count > 0)
            {
                dgSalesCallDetails_INV.Visible = false;
                dgSalesCallDetails.Visible = true;
                dgSalesCallDetails.DataSource = dtbl;
                dgSalesCallDetails.DataBind();
                lblauthstring.Text = dtbl.Rows[0]["AuthorisationString"].ToString();
                lblDepartment.Text = dtbl.Rows[0]["DepartmentName"].ToString();
                lblBusinessUnit.Text = dtbl.Rows[0]["BusinessUnitName"].ToString();
            }
            else
            {
                dgSalesCallDetails.Visible = false;
                //lblMessage.Text = "Sorry, this document has no previous actions.";
            }
        }
        #endregion
        //Added By Kd 07.12.2018
        protected void dgSalesCallDetails_PageIndexChanged1(object source, DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < dgSalesCallDetails.PageCount)
            {
                mpe.Show();
                this.dgSalesCallDetails.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                mpe.Show();
                this.dgSalesCallDetails.CurrentPageIndex = dgSalesCallDetails.PageCount;
            }
            GetInvoiceStatusDetails_CRN(Convert.ToInt32(ViewState["InvoiceID"]));
        }
        //Added By Kd 07.12.2018
        #region GetInvoiceStatusDetails_INV
        private void GetInvoiceStatusDetails_INV(int iInvoiceID)
        {
            JKS.Invoice objInvoice = new JKS.Invoice();
            lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "INV");
            lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "INV");
            lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "INV");
            ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)
                dtbl = objInvoice.GetInvoiceLogStatusApproverWise(iInvoiceID);

            else
                dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier(iInvoiceID);

            if (dtbl.Rows.Count > 0)
            {
                dgSalesCallDetails.Visible = false;
                dgSalesCallDetails_INV.Visible = true;
                dgSalesCallDetails_INV.DataSource = dtbl;
                dgSalesCallDetails_INV.DataBind();
            }
            else
            {
                dgSalesCallDetails_INV.Visible = false;
                //lblMessage.Text = "Sorry, this document has no previous actions.";
            }

        }
        #endregion
        //Added By Kd 07.12.2018
        protected void dgSalesCallDetails_PageIndexChanged2(object source, DataGridPageChangedEventArgs e)
        {
            //dgSalesCallDetails_INV.PageIndex = 0;
            if (e.NewPageIndex < dgSalesCallDetails_INV.PageCount)
            {
                mpe.Show();
                this.dgSalesCallDetails_INV.CurrentPageIndex = e.NewPageIndex;

            }
            else
            {
                mpe.Show();
                this.dgSalesCallDetails_INV.CurrentPageIndex = dgSalesCallDetails.PageCount;

            }
            GetInvoiceStatusDetails_INV(Convert.ToInt32(ViewState["InvoiceID"]));

        }



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
            //this.grdList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdList_ItemDataBound);
            this.grdList.ItemDataBound += new RepeaterItemEventHandler(grdList_ItemDataBound);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region PopulateDropDowns
        private void PopulateDropDowns()
        {
        }
        #endregion

        #region GetDocumentDetails(int iinvoiceID)
        private void GetDocumentDetails(int iinvoiceID)
        {
            DataSet DsInv = new DataSet();
            DsInv = GetDocumentDetails(iinvoiceID, ViewState["DocType"].ToString());
            if (DsInv != null)
            {
                if (DsInv.Tables.Count > 0)
                {
                    if (DsInv.Tables[0].Rows.Count > 0)
                    {
                        lblRefernce.Text = DsInv.Tables[0].Rows[0]["InvoiceNo"].ToString();
                        lblInvoiceDate.Text = DsInv.Tables[0].Rows[0]["InvoiceDate"].ToString();
                        lblSupplier.Text = DsInv.Tables[0].Rows[0]["SupplierCompanyName"].ToString();
                        iSupplierCompanyID = Convert.ToInt32(DsInv.Tables[0].Rows[0]["SupplierCompanyID"]);

                        lblBuyer.Text = DsInv.Tables[0].Rows[0]["BuyerCompanyName"].ToString();

                        Session["BuyerCID"] = DsInv.Tables[0].Rows[0]["BuyerCompanyID"].ToString();

                        lblCurrentStatus.Text = DsInv.Tables[0].Rows[0]["Status"].ToString();
                        if (ViewState["DocType"].ToString() == "INV")
                            lblCRn.Text = "Credit Note No";
                        if (ViewState["DocType"].ToString() == "CRN")
                            lblCRn.Text = "Invoice No";
                        // Added by Mrinal on 10th March 2015
                        lblcreditnoteno.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString();

                        lblBusinessUnit.Text = Convert.ToString(DsInv.Tables[0].Rows[0]["BusinessUnit"]);

                        try
                        {
                            lblNominal.Text = DsInv.Tables[0].Rows[0]["NominalCode"].ToString();
                        }
                        catch { }

                        try
                        {
                            lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();
                            ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();

                        }
                        catch { }
                        ViewState["StatusID"] = DsInv.Tables[0].Rows[0]["StatusID"].ToString();
                        //Added by Mainak 2018-04-06
                        statusId.Value = ViewState["StatusID"].ToString();
                        ViewState["OriginalNetAmount"] = DsInv.Tables[0].Rows[0]["Net"].ToString();
                        Session["eDocType"] = DsInv.Tables[0].Rows[0]["DocType"].ToString();
                    }
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
            sqlDA.SelectCommand.Parameters.Add("@Type", Convert.ToString(ViewState["DocType"]));
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


            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedIndex = -1;
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")), ds.Tables[1].Rows[i]["CompanyID"].ToString());
            }

            //  GetAllComboCodes();

            //for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            //{
            //    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex = -1;
            //    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ds.Tables[1].Rows[i]["DepartmentID"].ToString());
            //    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")), ds.Tables[1].Rows[i]["BusinessUnitID"].ToString());
            //    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Enabled = false;
            //    if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex > 0)
            //    {
            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedIndex = -1;
            //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ds.Tables[1].Rows[i]["CodingDescriptionID"].ToString());

            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedIndex = -1;
            //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ds.Tables[1].Rows[i]["NominalCodeID"].ToString());

            //    }
            //}

            // Added by Mrinal on 29th December 2014

            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            {
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")), ds.Tables[1].Rows[i]["BusinessUnitID"].ToString());
                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text = ds.Tables[1].Rows[i]["CodingDescription"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value = ds.Tables[1].Rows[i]["CodingDescriptionID"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value = ds.Tables[1].Rows[i]["DepartmentID"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value = ds.Tables[1].Rows[i]["NominalCodeID"].ToString();

                // Added by Mrinal on 25th March 2015
                // ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = ds.Tables[1].Rows[i]["Description"].ToString();
                // Addition End

                string LineDescription = ds.Tables[1].Rows[i]["Description"].ToString();
                if (LineDescription.Contains("&lt;"))
                {
                    LineDescription = LineDescription.Replace("&lt;", "<");
                }
                if (LineDescription.Contains("&gt;"))
                {
                    LineDescription = LineDescription.Replace("&gt;", ">");
                }
                if (LineDescription.Contains("&pound;"))
                {
                    LineDescription = LineDescription.Replace("&pound;", "£");
                }

                if (LineDescription.Contains("&belongsto;"))
                {
                    LineDescription = LineDescription.Replace("&belongsto;", "€");
                }

                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = LineDescription.ToString();
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
        protected DataTable GetBlankTable(int iNoofRow)
        {
            DataTable tbl = null;
            int InvoiceID = 0;
            double dtmpNetAmt = 0;
            InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
            dtmpNetAmt = GetNetAmt(InvoiceID);
            string PurOrderNo = GetPurOrderNo(InvoiceID);
            ViewState["NetAmt"] = dtmpNetAmt;

            if (iNoofRow <= 1)
            {
                DataSet ds = ((DataSet)ViewState["populate"]);
                tbl = new DataTable();
                DataRow nRow;
                tbl.Columns.Add("NetValue");
                // Added by Mrinal on 20th June 2013
                tbl.Columns.Add("PurOrderNo");
                tbl.Columns.Add("VAT");
                for (int i = 0; i < iNoofRow; i++)
                {
                    nRow = tbl.NewRow();
                    nRow["NetValue"] = dtmpNetAmt;
                    nRow["PurOrderNo"] = PurOrderNo;
                    ///
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        nRow["VAT"] = ds.Tables[1].Rows[i]["VAT"];
                    }
                    else
                    {
                        nRow["VAT"] = "0.00";
                    }
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
                    // Added by Mrinal on 20th June 2013
                    tbl.Columns.Add("PurOrderNo");
                    tbl.Columns.Add("VAT");
                    for (int i = 0; i < iNoofRow; i++)
                    {
                        nRow = tbl.NewRow();
                        nRow["NetValue"] = ds.Tables[1].Rows[i]["netvalue"];
                        nRow["PurOrderNo"] = ds.Tables[1].Rows[i]["PurOrderNo"];
                        nRow["VAT"] = ds.Tables[1].Rows[i]["VAT"];
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
                ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

                if (compid != 0)
                {
                    dt = objInvoice.GetGridDepartmentList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Enabled = false;

                    dt = objInvoice.GetGridNominalCodeList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Enabled = false;


                    dt = objInvoice.GetGridCodingDescriptionList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ddlCodingDescription1);
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Enabled = false;

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



        #region btndelete_Click
        private void btndelete_Click(object sender, System.EventArgs e)
        {
            JKS.Invoice objinvoice = new JKS.Invoice();
            if (txtComment.Text.Trim() == "")
            {
                lblErrorMsg.Text = "Please enter a comment.";
                return;
            }
            else
            {
                lblErrorMsg.Text = "";
                iApproverStatusID = 7;
                strComments = txtComment.Text.Trim();
                UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));

                if (DocType == "INV")
                {
                    StatusUpdate = objinvoice.UpdateInvStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (StatusUpdate == 1)
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                        lblErrorMsg.Text = "Invoice deleted successfully";
                        Response.Write("<script>alert('Invoice Deleted Successfully');</script>");
                        //   Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");
                        //  Response.Write("<script>self.close();</script>");
                        Response.Write("<script>parent.window.close();</script>");
                    }
                    else
                        lblErrorMsg.Text = "Invoice cannot be deleted";
                }
                else if (DocType == "CRN")
                {
                    StatusUpdate = objinvoice.UpdateCrnStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (StatusUpdate == 1)
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments);
                        lblErrorMsg.Text = "CreditNote deleted successfully";
                        Response.Write("<script>alert('Credit Note Deleted Successfully');</script>");
                        // Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");
                        // Response.Write("<script>self.close();</script>");
                        Response.Write("<script>parent.window.close();</script>");
                    }
                    else
                        lblErrorMsg.Text = "CreditNote cannot be deleted";
                }
                else
                {
                    StatusUpdate = objinvoice.UpdateDebitNoteStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (StatusUpdate == 1)
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise_DN(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments);
                        lblErrorMsg.Text = "CreditNote deleted successfully";
                        Response.Write("<script> alert('Credit Note Deleted Successfully'); </script>");
                        // Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");
                        //  Response.Write("<script> self.close(); </script>");
                        Response.Write("<script>parent.window.close();</script>");
                    }
                    else
                        lblErrorMsg.Text = "CreditNote cannot be deleted";
                }
            }
        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            //   Response.Write("<script>opener.location.reload(true);</script>");
            //   Response.Write("<script>self.close();</script>");
            Response.Write("<script>parent.window.close();</script>");
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
            sqlDA.SelectCommand.Parameters.Add("@Type", Type.Trim().ToUpper());
            sqlDA.SelectCommand.CommandTimeout = 0;
            try
            {
                if (sqlConn.State != ConnectionState.Open)
                {
                    sqlConn.Open();
                }
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
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
            string sSql = "";
            if (DocType == "INV")
            {
                sSql = "SELECT BuyerCompanyID,SupplierCompanyID FROM Invoice WHERE InvoiceID=" + iInvoiceID;
            }
            else if (DocType == "CRN")
            {
                sSql = "SELECT BuyerCompanyID,SupplierCompanyID FROM creditnote WHERE creditnoteID=" + iInvoiceID;
            }
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
            string sSql = "";
            if (DocType.ToUpper().Trim() == "INV")
            {
                sSql = "select nettotal from invoice where invoiceid=" + InvoiceID;
            }
            else if (DocType.ToUpper().Trim() == "CRN")
            {
                sSql = "select nettotal from creditnote where creditnoteid=" + InvoiceID;
            }
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

        #region GetMultipleCreditNotes()
        public string GetMultipleCreditNotes()
        {
            string strMultipleCreditNo = "";
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT INVOICENO FROM CREDITNOTE WHERE StatusID = 7 AND CREDITINVOICENO = "
                + " (SELECT  INVOICENO FROM INVOICE WHERE INVOICEID=" + invoiceID + ") "
                + " AND buyerCompanyId=" + Convert.ToInt32(Session["BuyerCID"]) + " And SupplierCompanyid=" + iSupplierCompanyID;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                for (int k = 0; k < Dst.Tables[0].Rows.Count; k++)
                {
                    strMultipleCreditNo += Dst.Tables[0].Rows[k]["INVOICENO"].ToString();
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); strMultipleCreditNo = ""; }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return strMultipleCreditNo;

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

        #region:GetPurOrderNo(int InvoiceID):--Fetch Purchase Order Added by Mrinal on 27th June 2013
        private string GetPurOrderNo(int InvoiceID)
        {
            string PurOrderNo = string.Empty;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_FetchPONumberFromGenericCodingChange", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@Type", DocType);
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    PurOrderNo = ds.Tables[0].Rows[0]["PurOrderNo"].ToString();
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
            }
            return PurOrderNo;
        }

        #endregion

        #region grdList_ItemDataBound
        private void grdList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                int j = e.Item.DataSetIndex + 1;
                e.Item.Cells[0].Text = j.ToString();
                DataTable dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataTextField = "CompanyName";
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataValueField = "CompanyID";
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Enabled = false;

                //  GetAllComboCodes();
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
                ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Enabled = false;

                ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

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
        public void grdList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                //int j = e.Item.DataSetIndex + 1;
                //e.Item.Cells[0].Text = j.ToString();
                int j = e.Item.ItemIndex + 1;

                ((Label)e.Item.FindControl("lblLineNo")).Text = j.ToString();

                //  DataTable dt = null;


                DataTable dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataTextField = "CompanyName";
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataValueField = "CompanyID";
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
                // ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, new ListItem("--Select--", "0"));

                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Enabled = false;

                //   GetAllComboCodes();
                try
                {
                    if (Session["BuyerCID"] != null)
                        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = Session["BuyerCID"].ToString().Trim();
                }
                catch { }

                try
                {
                    if (Request["DDCompanyID"] != null)
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
                ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Enabled = false;

                //((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");
                ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Insert(0, new ListItem("--Select--", "0"));

                ((CheckBox)e.Item.FindControl("chkBox")).Enabled = false;




                if (((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim() != "")
                {
                    dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim());
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                GetVatAmount();
                CalculateTotal();

            }

        }
        //protected void CalculateTotal()
        //{
        //    double dVariance = 0;
        //    lblNetVal.Text = dNetAmt.ToString("0.00");
        //    if (ViewState["NetAmt"] != null)
        //    {
        //        lblNetInvoiceTotal.Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString("0.00");
        //    }
        //    else
        //    {
        //        dVariance = dNetAmt * (-1);
        //    }

        //}
        public string GetDecimalFormattedValue(object value)
        {
            string strGetDecimalFormattedValue = Convert.ToString("0.00");
            if (value != null && value != DBNull.Value)
            {
                double strAmount = 0;
                strAmount = Convert.ToDouble(value);
                strGetDecimalFormattedValue = strAmount.ToString("0.00");
            }

            return strGetDecimalFormattedValue;

        }


        protected string GetInvoiceStatusLog()
        {
            string strDocType = Convert.ToString(Request.QueryString["DocType"]);

            string strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);
            string strURL = "";
            //strURL = "javascript:window.open('../../ETC/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL','width=300,height=250,scrollbars=1');";
            if (strDocType.Trim().ToUpper() == "INV")
            {
                strURL = "../Invoice/InvoiceStatusLogNL.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
            }
            else if (strDocType.Trim().ToUpper() == "CRN")
            {
                strURL = "../Creditnotes/InvoiceStatuslogNL_CN.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
            }
            return (strURL);
        }

        protected void InvoiceCrnIsDuplicate()
        {


            // PasswordReset objPasswordReset = new PasswordReset();
            // List<PasswordReset> lstPasswordReset = objPasswordReset.GetDuplicates(Convert.ToString(Request.QueryString["InvoiceID"]), "INV", Convert.ToString(Session["CompanyID"]), Convert.ToString(Session["SupplierCompanyID"]));
            //  if (lstPasswordReset.Count > 0)
            //{
            //if (lstPasswordReset[0].Duplicate == "Yes")
            // {
            //lblDuplicate.Visible = true;
            //}
            // else
            //{
            //lblDuplicate.Visible = false;
            //}
            //}


        }
        protected void IsAutorisedtoEditData()
        {
            if (Session["UserID"] != null)
            {
                string strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);
                string strDocType = Convert.ToString(Request.QueryString["DocType"]);
                string strUserID = Convert.ToString(Session["UserID"]);
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("sp_IsAuthorisedToEditData", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@UserID", strUserID);
                DataSet ds = new DataSet();
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(ds);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int IsAuthorised = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                        if (IsAuthorised > 0)
                        {
                            aEditData.Visible = true;
                            if (strDocType.Trim().ToUpper() == "INV")
                            {
                                //aEditData.Attributes.Add("href", "../Invoice/InvoiceEdit.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1");
                                aEditData.Attributes.Add("onclick", "javascript:window.open('../Invoice/InvoiceEdit.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1', 'CustomPopUp','width=1270, height=750,scrollbars=1,resizable=1');return false;");// Added by Rimi on 20.06.2015
                            }
                            else if (strDocType.Trim().ToUpper() == "CRN")
                            {
                                //aEditData.Attributes.Add("href", "../Creditnotes/InvoiceEdit_CN.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1");
                                aEditData.Attributes.Add("onclick", "javascript:window.open('../Creditnotes/InvoiceEdit_CN.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1', 'CustomPopUp','width=1270, height=750,scrollbars=1,resizable=1');return false;");// Added by Rimi on 20.06.2015
                            }
                        }
                        else
                        {
                            // lblDuplicate.Visible = false;
                            aEditData.Visible = false;
                        }
                    }
                    else
                    {
                        aEditData.Visible = false;
                    }
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
                finally
                {
                    sqlDA.Dispose();
                    sqlConn.Close();
                }
            }
            else
            {
            }
        }
        private void GetVatAmount()
        {
            string strDocType = Convert.ToString(Request.QueryString["DocType"]);
            int InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
            double TotalAmt = 0;
            double VATAmt = 0;
            string strCurrencyCode = string.Empty;
            string sSql = string.Empty;
            if (strDocType.Trim().ToUpper() == "INV")
            {
                sSql = "SELECT  CONVERT(DECIMAL(18,2),TotalAmt) As TotalAmt ,CONVERT(DECIMAL(18,2),VATAmt) As VATAmt  ,CurrencyCode,Invoice.CurrencyTypeID ,CurrencyName FROM    Invoice  INNER   JOIN CurrencyTypes ON dbo.Invoice.CurrencyTypeID = dbo.CurrencyTypes.CurrencyTypeID WHERE   InvoiceID=" + InvoiceID;

            }
            else if (strDocType.Trim().ToUpper() == "CRN")
            {
                sSql = "SELECT  CONVERT(DECIMAL(18,2),TotalAmt) As TotalAmt ,CONVERT(DECIMAL(18,2),VATAmt) As VATAmt  ,CurrencyCode,CreditNote.CurrencyTypeID ,CurrencyName FROM    CreditNote  INNER   JOIN CurrencyTypes ON dbo.CreditNote.CurrencyTypeID = dbo.CurrencyTypes.CurrencyTypeID WHERE   CreditNoteID=" + InvoiceID;

            }
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
                        TotalAmt = Convert.ToDouble(dr[0]);
                        TotalAmt = Convert.ToDouble(TotalAmt.ToString("0.00"));
                    }
                    if (dr[1] != DBNull.Value)
                    {
                        VATAmt = Convert.ToDouble(dr[1]);
                        VATAmt = Convert.ToDouble(VATAmt.ToString("0.00"));
                    }
                    if (dr[2] != DBNull.Value)
                    {
                        strCurrencyCode = Convert.ToString(dr[2]);
                    }
                }
                // lblVat.Text = VATAmt.ToString("F", System.Globalization.CultureInfo.InvariantCulture); //VATAmt.ToString();
                //  lblTotal.Text = TotalAmt.ToString();
                lblVat.Text = VATAmt.ToString("0.00");
                lblTotal.Text = TotalAmt.ToString("0.00");
                lblCurrencyCode.Text = strCurrencyCode.ToString();


            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr.Close();
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            // return NetAmt;
        }
        //protected void CalculateTotal()
        //{
        //    double dVariance = 0;

        //    dNetAmt = 0;

        //    foreach (RepeaterItem ri in grdList.Items)
        //    {
        //        if (((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text.Trim() != "")
        //        {
        //            dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text.Trim());
        //        }
        //    }




        //    lblNetVal.Text = dNetAmt.ToString("0.00");
        //    if (ViewState["NetAmt"] != null)
        //    {
        //        lblNetInvoiceTotal.Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString("0.00");
        //        dVariance = Convert.ToDouble(ViewState["NetAmt"].ToString()) - dNetAmt;
        //    }
        //    else
        //    {
        //        dVariance = dNetAmt * (-1);
        //    }
        //    lblVariance.Text = dVariance.ToString("0.00");
        //}
        protected void CalculateTotal()
        {
            double dVariance = 0;

            dNetAmt = 0;
            dCodingVat = 0;

            foreach (RepeaterItem ri in grdList.Items)
            {
                if (((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text.Trim() != "")
                {
                    dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text.Trim());
                }
                if (((System.Web.UI.WebControls.TextBox)ri.FindControl("txtLineVAT")).Text.Trim() != "")
                {
                    double dLineVatValue = 0;
                    dLineVatValue = Convert.ToDouble(((System.Web.UI.WebControls.TextBox)ri.FindControl("txtLineVAT")).Text.Trim());
                    dCodingVat = dCodingVat + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)ri.FindControl("txtLineVAT")).Text.Trim());
                    ((System.Web.UI.WebControls.TextBox)ri.FindControl("txtLineVAT")).Text = dLineVatValue.ToString("0.00");
                }
            }




            lblNetVal.Text = dNetAmt.ToString("0.00");
            if (ViewState["NetAmt"] != null)
            {
                lblNetInvoiceTotal.Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString("0.00");
                dVariance = Convert.ToDouble(ViewState["NetAmt"].ToString()) - dNetAmt;
            }
            else
            {
                dVariance = dNetAmt * (-1);
            }
            lblVariance.Text = dVariance.ToString("0.00");

            //// Added by Mrinal On 18th March 2015
            //lblTotalCodingVATValue.Text = dCodingVat.ToString("0.00");
            //lblVATVariance.Text = (dVariance - dCodingVat).ToString("0.00");

            // Added by Mrinal On 19th March 2015
            lblTotalCodingVATValue.Text = dCodingVat.ToString("0.00");
            double dTotalVat = 0;
            dTotalVat = Convert.ToDouble(lblVat.Text);
            lblVATVariance.Text = (dTotalVat - dCodingVat).ToString("0.00");
        }
        //Added by Mainak 2018-03-22
        protected void btnReset_Click(object sender, EventArgs e)
        {
            int statusId = Convert.ToInt32(ViewState["StatusID"].ToString());
            string strDocType = Convert.ToString(Request.QueryString["DocType"]);
            invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
            //Added by Mainak 2018-04-06
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //Update Invoice/CreditNote After Reset from History Combind Window
                objinvoice.updateInvoiceAfterReset(invoiceID, strDocType);
                Response.Write("<script>parent.window.close();</script>");//Added by Mainak 2018-04-05
            }
            else
            {
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }

        }
        //Ended by Mainak 2018-03-22
    }
}