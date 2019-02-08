using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Net;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualBasic;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using CBSolutions.ETH.Web;


namespace JKS
{
    [ScriptService]
    public partial class ETC_Current_CurrentStatusNew : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region User Defined Variables
        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected DataSet ds = null;
        protected DataTable objDataTable = null;
        protected DataRow drInvoiceHeader = null;
        protected DataRow drInvoiceInvoiceLog = null;
        protected DataRow dr = null;

        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        protected string strInvoiceDocumentDownloadPath = ConfigurationManager.AppSettings["InvoiceDocPath"];
        int currentYear = 0;
        private int iLoadFlag = 0;
        private string strFromDate = "";
        private string strToDate = "";
        private decimal FromPrice;
        private decimal ToPrice;
        private int iOption = 0;

        public string strRelationType = "";

        public static string strSortOrder = "";
        public static string strSortField = "";

        JKS.Invoice objInvoice = new JKS.Invoice();
        // CheckBox chkDownload;

        // Added by Mrinal on 28th January 2015
        DataTable dtRepeater = new DataTable();
        protected System.Web.UI.WebControls.Repeater rptAttachment;
        protected int iNeedRefreshToBottom = 0;
        #endregion
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
            //  chkDownload.CheckedChanged += new EventHandler(chkDownload_CheckedChanged);
        }



        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdInvCur.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.grdInvCur_PageIndexChanged);
            this.grdInvCur.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvCur_ItemDataBound);
            this.grdInvCur.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.grdInvCur_SortCommand);
            this.grdInvCur.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdInvCur_ItemCommand);

            this.Load += new System.EventHandler(this.Page_Load);
            this.btnDownloadAttachment.Click += new System.EventHandler(this.btnDownloadAttachment_Click);
            this.btnProcess.Click += new EventHandler(btnProcess_Click);



        }
        #endregion
        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");
            iNeedRefreshToBottom = 0;
            baseUtil.keepAlive(this);

            lblMsg1.Visible = false;
            lblMsg.Visible = false;
            doAction();

            if (!IsPostBack)
            {
                // Added By Mrinal 22nd September 2014
                Session["dtTiffViewer"] = null;
                ViewState["dtCheckAttachment"] = null;
                // Addition End on 22nd September 2014
                lblMessage.Text = "";
                lblMessage.Visible = false;

                Utility.makeDefaultButton(txtInvoiceNo, btnSearch);
                Utility.makeDefaultButton(textRange1, btnSearch);
                Utility.makeDefaultButton(textRange2, btnSearch);

                Session["ApproveForm"] = 0;
                Session["SelectedPage"] = "PurchaseInvoiceLog";
                iLoadFlag++;
                Session["DropDownCompanyID"] = null;
                btnSearch.Attributes.Add("onclick", "javascript:return fn_Validate();");

                String str1 = "BuyerID=" + ((int)Session["CompanyID"]).ToString() + " and StatusId = 7";
                String str2 = "BuyerID=" + ((int)Session["CompanyID"]).ToString() + " and (StatusId != 7 or StatusId is null)";
                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
                //LoadDate();
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                LoadDepartment();

                //Blocked by Mrinal on 31st December 2013
                if (Convert.ToInt32(Session["UserTypeID"]) != 2 && Convert.ToInt32(Session["UserTypeID"]) != 3)
                {
                    if (Session["DropDownCompanyID"] != null)
                    {
                        // btnSearch_Click(null, null);
                        LoadData(Convert.ToInt32(Session["DropDownCompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));
                    }
                    else
                        LoadData(Convert.ToInt32(Session["CompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));

                }

                if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                {
                    cbSupplier.Checked = true;
                    cbInvoiceNo.Checked = true;
                    divP2DLogo.Visible = true;

                }


            }
            //----------modified by kuntal on 18th mar2015---pt.46-------
            populateVendorClass(ddlCompany.SelectedIndex);
            //-----------------------------------------------------------
        }
        #endregion
        #region takeAction  ,  doAction
        private void takeAction(string docType, int ID, int iOperation)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

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

        private void doAction()
        {
            string[] strOuterArray = null;
            string sInnerVal1 = "";
            string sInnerVal2 = "";
            string sRetValue = "", sDocType = "";
            int ID = 0;
            sRetValue = Request["__EVENTARGUMENT"];
            if (sRetValue != null && sRetValue != "")
            {
                strOuterArray = sRetValue.Split('|');
                sInnerVal1 = strOuterArray[0].Replace("id=", String.Empty);
                sInnerVal2 = strOuterArray[1].Replace("docType=", String.Empty);
                ID = Convert.ToInt32(sInnerVal1);
                sDocType = sInnerVal2;
            }
            if (ID > 0 && sDocType != "")
            {
                takeAction(sDocType, ID, 0);
                if (Session["DropDownCompanyID"] != null)
                {
                    LoadData(Convert.ToInt32(Session["DropDownCompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));
                    if (strSortField != null && strSortField != "")
                    {
                        GetAllCategoryByAdmin();
                    }
                }
                else
                {
                    LoadData(Convert.ToInt32(Session["CompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));
                }
            }
        }
        #endregion
        #region CreateTable
        private void CreateTable()
        {
            objDataTable = new DataTable("InvoiceDetails");
            //RowID
            objDataTable.Columns.Add("RowID");
            objDataTable.Columns.Add("InvStatusID");
            //
            objDataTable.Columns.Add("InvoiceID");
            objDataTable.Columns.Add("ReferenceNo");
            objDataTable.Columns.Add("SupplierCode");
            objDataTable.Columns.Add("Supplier");
            objDataTable.Columns.Add("Buyer");
            objDataTable.Columns.Add("VendorID");
            objDataTable.Columns.Add("InvoiceDate");
            objDataTable.Columns.Add("DeliveryDate");
            objDataTable.Columns.Add("Currency");
            objDataTable.Columns.Add("Net");
            objDataTable.Columns.Add("VAT");
            objDataTable.Columns.Add("Total");
            objDataTable.Columns.Add("DocStatus");
            objDataTable.Columns.Add("ActionStatus");
            objDataTable.Columns.Add("User");
            objDataTable.Columns.Add("Comment");
            objDataTable.Columns.Add("ActionDate");
            objDataTable.Columns.Add("PaymentDueDate");
            objDataTable.Columns.Add("DocAttachments");
            objDataTable.Columns.Add("DocType");
            objDataTable.Columns.Add("ParentRowFlag");
            objDataTable.Columns.Add("VoucherNumber");
            objDataTable.Columns.Add("ActivityCode");
            objDataTable.Columns.Add("ScanDate");
            objDataTable.Columns.Add("ScanDates");
            objDataTable.Columns.Add("InvoiceDates");
            objDataTable.Columns.Add("Net1", typeof(Int64));
            objDataTable.Columns.Add("VAT1", typeof(Int64));
            objDataTable.Columns.Add("Total1", typeof(Int64));
            objDataTable.Columns.Add("New_VendorClass");
            objDataTable.Columns.Add("IsDuplicate");


        }
        #endregion
        #region LoadData
        private void LoadData(int iCompanyID, string sDocType, int iUserID)
        {
            if (txtSupplier.Value.ToString().Trim().Length <= 0)
            {
                HdSupplierId.Value = "";
            }
            if (txtInvoiceNo.Text.Trim().Length <= 0)
            {
                hdInvoiceNo.Value = "";
            }
            if (txtNominal.Value.Trim().Length <= 0)
            {
                hdNominalCodeId.Value = "";
            }


            JKS.Invoice objInvoice = new JKS.Invoice();
            string strStatus = "";
            string strUserName = "";
            string strEmail = "";
            CreateTable();
            CheckDate();
            int CurrentCompanyID = 0;
            CurrentCompanyID = iCompanyID;
            if (CurrentCompanyID == 0)
            {
                CurrentCompanyID = Convert.ToInt32(Session["CompanyID"]);
            }

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //sqlDA = new SqlDataAdapter("NewBuyerCurrent_ETC_Sample", sqlConn);
            sqlDA = new SqlDataAdapter("NewBuyerCurrent_ETC_SampleNew", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@UserID", iUserID);
            //sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", CurrentCompanyID);
            // sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", ddlSupplier.SelectedValue.Trim());
            sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
            if (strFromDate.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@FromDate", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);

            if (strToDate.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@ToDate", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);
            //if (txtInvoiceNo.Text.Trim() == "")
            //    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", DBNull.Value);
            //else
            //    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", txtInvoiceNo.Text.Trim());

            if (txtPONo.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@PONo", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@PONo", txtPONo.Text.Trim());

            if (Convert.ToString(ddlBusinessUnit.SelectedValue) == "0")
            {
                sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", DBNull.Value);
            }
            else
            {
                sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", Convert.ToInt32(ddlBusinessUnit.SelectedValue));
            }

            if (textRange1.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@FromPrice", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@FromPrice", Convert.ToDecimal(FromPrice));
            if (textRange2.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@ToPrice", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@ToPrice", Convert.ToDecimal(ToPrice));

            if (Convert.ToString(ddldept.SelectedValue) != "Select Department")
                sqlDA.SelectCommand.Parameters.Add("@DepartmentID", ddldept.SelectedValue.Trim());
            else
                sqlDA.SelectCommand.Parameters.Add("@DepartmentID", DBNull.Value);


            if (sDocType != "")
            {
                sqlDA.SelectCommand.Parameters.Add("@DocType", sDocType);
            }
            else
            {
                sqlDA.SelectCommand.Parameters.Add("@DocType", DBNull.Value);
            }

            if (Convert.ToInt32(Session["UserTypeID"]) == 3)
            {
                sqlDA.SelectCommand.Parameters.Add("@PassedToUserID", DBNull.Value);
                sqlDA.SelectCommand.Parameters.Add("@PassedToGroupCode", DBNull.Value);
                iOption = 1;
            }
            else
            {
                sqlDA.SelectCommand.Parameters.Add("@PassedToUserID", Session["UserID"].ToString().Trim());
                if (Session["UserGroupCode"] == null || Session["UserGroupCode"].ToString() == "NULL")
                    sqlDA.SelectCommand.Parameters.Add("@PassedToGroupCode", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@PassedToGroupCode", Session["UserGroupCode"].ToString().Trim());

                iOption = 0;
            }

            //--------------Add Filter For Wild Card search  (Subha Das 30th Dec 2014)
            //------Supplier Wild Card
            int IsSentHdId = 0;
            if ((txtSupplier.Value.ToString().Trim() != "") && (cbSupplier.Checked))
            {
                if (HdSupplierId.Value.ToString().Trim() != "")
                {
                    sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", Convert.ToInt32(HdSupplierId.Value));
                    IsSentHdId = 1;
                }
                else
                    sqlDA.SelectCommand.Parameters.Add("@Filter", txtSupplier.Value.ToString().Trim());
            }
            else
                sqlDA.SelectCommand.Parameters.Add("@Filter", DBNull.Value);

            if (IsSentHdId == 0)
            {
                if ((HdSupplierId.Value.ToString().Trim() != "") && (cbSupplier.Checked == false))
                    sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", Convert.ToInt32(HdSupplierId.Value));
                else
                    sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", 0);
            }


            //------InvoiceNo Wild Card
            IsSentHdId = 0;
            if ((txtInvoiceNo.Text.Trim() != "") && (cbInvoiceNo.Checked))
            {
                if (hdInvoiceNo.Value.ToString().Trim() != "")
                {
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", hdInvoiceNo.Value.Trim().ToString());
                    IsSentHdId = 1;
                }
                else
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNoStr", txtInvoiceNo.Text.Trim());
            }

            else
                sqlDA.SelectCommand.Parameters.Add("@InvoiceNoStr", DBNull.Value);

            if (IsSentHdId == 0)
            {
                if ((hdInvoiceNo.Value.ToString().Trim() != "") && (cbInvoiceNo.Checked == false))
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", hdInvoiceNo.Value.Trim().ToString());
                else
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", DBNull.Value);
            }

            IsSentHdId = 0;

            //-------------Add Filter For New Vendor Class and Nomina (Subha Das 2nd January 2015)
            if (Convert.ToString(ddlVendorClass.SelectedValue) == "0")
                sqlDA.SelectCommand.Parameters.Add("@New_VendorClass", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@New_VendorClass", ddlVendorClass.SelectedValue.Trim());

            //if (txtNominal.Value == "")
            //    sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", DBNull.Value);
            //else
            //    sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", txtNominal.Value.ToString().Trim());

            if (hdNominalCodeId.Value != "")
                sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", Convert.ToInt32(hdNominalCodeId.Value));
            else
                sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", DBNull.Value);

            // Adding section End 

            sqlDA.SelectCommand.Parameters.Add("@Option", iOption);
            sqlDA.SelectCommand.CommandTimeout = 0;

            sqlDA.TableMappings.Add("InvoiceDetails", "InvoiceHeader");

            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds, "InvoiceDetails");
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                if (txtSupplier.Value.Trim().Length <= 0)
                {
                    HdSupplierId.Value = "";
                }
                sqlDA.Dispose();
                sqlConn.Close();
            }

            #region: Multiple AttachmentDownload
            //  int TableCount = ds.Tables.Count;
            if (ds.Tables.Count > 1)
            {
                DataTable dtAttachment = ds.Tables[1];
                ViewState["dtAttachment"] = dtAttachment;
            }

            #endregion


            # region:	loop
            foreach (DataRow drInvoiceHeader in ds.Tables["InvoiceHeader"].Rows)
            {

                dr = objDataTable.NewRow();
                dr["RowID"] = drInvoiceHeader["RowID"];
                dr["InvStatusID"] = drInvoiceHeader["StatusID"];
                dr["InvoiceID"] = drInvoiceHeader["InvoiceID"];
                dr["ReferenceNo"] = drInvoiceHeader["ReferenceNo"];
                dr["SupplierCode"] = drInvoiceHeader["SupplierCode"];
                dr["Supplier"] = drInvoiceHeader["Supplier"];
                dr["Buyer"] = drInvoiceHeader["Buyer"];
                dr["VendorID"] = drInvoiceHeader["VendorID"];
                dr["InvoiceDate"] = drInvoiceHeader["InvoiceDate"];
                dr["DeliveryDate"] = drInvoiceHeader["DeliveryDate"];
                dr["Currency"] = objInvoice.GetCurrencyCode(Convert.ToInt32(drInvoiceHeader["CurrencyTypeID"]));
                dr["Net"] = drInvoiceHeader["Net"];
                dr["VAT"] = drInvoiceHeader["VAT"];
                dr["Total"] = drInvoiceHeader["Total"];
                objInvoice.GetCurrentStatus(Convert.ToInt32(drInvoiceHeader["StatusID"]), out strStatus);
                dr["DocStatus"] = strStatus;
                dr["ActionStatus"] = drInvoiceHeader["ActionStatus"];
                objInvoice.GetUserName(Convert.ToInt32(drInvoiceHeader["ModUserID"]), out strUserName, out strEmail);
                dr["User"] = strUserName;
                dr["Comment"] = strUserName;
                dr["PaymentDueDate"] = drInvoiceHeader["PaymentDueDate"];
                dr["DocAttachments"] = drInvoiceHeader["Document"];
                dr["DocType"] = drInvoiceHeader["New_DocumentType"];
                dr["VoucherNumber"] = drInvoiceHeader["VoucherNumber"];
                dr["ParentRowFlag"] = "1";
                dr["ActivityCode"] = drInvoiceHeader["ActivityCode"];
                dr["ScanDate"] = drInvoiceHeader["ScanDate"];
                dr["ScanDates"] = drInvoiceHeader["ScanDates"];
                dr["InvoiceDates"] = drInvoiceHeader["InvoiceDates"];
                // ---------------Subhrajyoti on 20th March 2015----------
                dr["Net1"] = Convert.ToInt64(drInvoiceHeader["Net1"].ToString().Trim());
                dr["VAT1"] = Convert.ToInt64(drInvoiceHeader["VAT1"].ToString().Trim());
                dr["Total1"] = Convert.ToInt64(drInvoiceHeader["Total1"].ToString().Trim());
                // ---------------Subhrajyoti----------

                dr["New_VendorClass"] = drInvoiceHeader["New_VendorClass"];//ss
                dr["IsDuplicate"] = drInvoiceHeader["IsDuplicate"];//ss
                objDataTable.Rows.Add(dr);
            }
            # endregion


            objDataTable.AcceptChanges();
            ViewState["objDataTable"] = objDataTable;
            #region: Tiff Viewer Implementation
            if (objDataTable != null && objDataTable.Rows.Count > 0)
            {
                DataTable dtTiffViewer = objDataTable;
                Session["dtTiffViewer"] = dtTiffViewer;
            }
            #endregion
            PopulateGrid();
        }
        #endregion
        #region PopulateGrid
        private void PopulateGrid()
        {
            try
            {
                DataTable dtbl = (DataTable)ViewState["objDataTable"];
                if (dtbl.Rows.Count > 0)
                {
                    lblMessage.Text = "";
                    lblMessage.Visible = false;
                    divP2DLogo.Visible = false;
                    grdInvCur.Visible = true;
                    grdInvCur.DataSource = dtbl;
                    grdInvCur.DataBind();
                }
                else
                {
                    grdInvCur.Visible = false;
                    divP2DLogo.Visible = true;
                    lblMessage.Text = "Sorry, no record(s) found.";

                    lblMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                string sExp = ex.Message;
            }
        }
        #endregion


        #region HighlightDuplicate_GridView


        protected void grdInvCurItem_Bound(Object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Label lblBalance = (Label)e.Item.FindControl("dgLabel2");
            }
        }

        #endregion


        #region grdInvCur_PageIndexChanged
        private void grdInvCur_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex <= grdInvCur.PageCount)
            {
                grdInvCur.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                grdInvCur.CurrentPageIndex = grdInvCur.PageCount;
            }

            if (strSortField != "" && strSortOrder != "")
                GetAllCategoryByAdmin();
            else
            {
                PopulateGrid();
                CheckDuplicateValues();
            }
        }
        #endregion
        #region GetDocumentWithPath
        protected string GetDocumentWithPath(object oDocument)
        {
            string strURL = "";
            string strDocument = "";
            strDocument = Convert.ToString(oDocument);
            if (strDocument.Trim() != "")
            {
                strURL = "<a href='" + strInvoiceDocumentDownloadPath + strDocument + "' target='_blank'> Download Document </a>";
            }
            else
            {
                strURL = "";
            }
            return (strURL);
        }
        #endregion
        #region GetCommentURL
        protected string GetCommentURL(object oInvoiceID)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strURL = "";
            strURL = "javascript:window.open('show_comments.aspx?InvoiceID=" + strInvoiceID + "','a','width=675,height=450');";
            return (strURL);
        }
        #endregion
        #region GetLogURL
        protected string GetLogURL(object oInvoiceID)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strURL = "";
            strURL = "javascript:window.open('InvoiceLogHistory.aspx?InvoiceID=" + strInvoiceID + "','a','width=1000,height=650,scrollbars=yes');";
            return (strURL);
        }
        #endregion
        #region ddlCompany_SelectedIndexChanged
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //--------ASSING SUPPLIER NAME TO TEXT BOX . BY Subha Das (02/01/2015) 
            txtSupplier.Value = "";
            HdSupplierId.Value = "";

            txtInvoiceNo.Text = "";
            hdInvoiceNo.Value = "";

            txtNominal.Value = "";
            hdNominalCodeId.Value = "";

            //Invoice.Invoice objInvoice = new Invoice.Invoice();
            Invoice_New objInvoice = new Invoice_New();

            //ddlSupplier.Items.Clear();
            //ddlSupplier.DataSource = objInvoice.GetSuppliersListForSearch(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]), "");
            //ddlSupplier.DataBind();
            //ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
            GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            LoadDepartment();

            // Added for Vendor Class (Subha Das 18th Dec 2014)
            populateVendorClass(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            txtSupplier.Value = "";

        }
        #endregion

        // Added for Vendor Class (Subha Das 18th Dec 2014)
        #region populateVendorClass
        private void populateVendorClass(int iCompanyId)
        {
            ddlVendorClass.Items.Clear();
            if (iCompanyId != 0)
            {
                SqlConnection sqlConn = new SqlConnection(ConsString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetVendorClass_NewVersion", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));

                DataSet ds = new DataSet();
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlVendorClass.DataSource = ds;
                        ddlVendorClass.DataBind();
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
                    ds = null;
                    ddlVendorClass.Items.Insert(0, new ListItem("Select Vendor Class", "0"));
                }
            }
            else

                ddlCompany.Focus();
        }
        #endregion

        private void LoadDepartment()
        {
            ddldept.Items.Clear();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentList_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
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
                ds = null;
                ddldept.Items.Insert(0, new ListItem("Select Department", "0"));
            }

        }
        #region GetCompanyListForPurchaseInvoiceLog
        private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int iAction)
        {
            if (iAction == 1)
            {
                Company objCompany = new Company();
                ddlCompany.Items.Clear();
                DataTable dt = new DataTable();
                try
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    if (dt.Rows.Count > 0)
                    {
                        ddlCompany.DataSource = dt;
                        ddlCompany.DataBind();

                        //--------------- Set Default Selected value  "Select Company Name"  by Subha,04-02-2015
                        if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                            ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                        else
                            ddlCompany.SelectedValue = dt.Rows[0][0].ToString();

                        Session["DropDownCompanyID"] = ddlCompany.SelectedValue.ToString();
                    }
                }
                catch { }
                finally
                {
                    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

                }
            }
            JKS.Invoice objInvoice = new JKS.Invoice();
            ddlSupplier.Items.Clear();
            ddlSupplier.DataSource = objInvoice.GetSuppliersList(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));

            Invoice_New objInv1 = new Invoice_New();
            ddlDocStatus.Items.Clear();
            //ddlDocStatus.DataSource = objInvoice.GetStatusListNL();

            //----Changes as per requiermnt : Removing  Approved, Approved s.t. AP, Delete/Archive, Exported, Paid  these status form Doc Status List  only for Current .by Subha 05022015
            ddlDocStatus.DataSource = objInv1.GetStatusListNL_Current();
            ddlDocStatus.DataBind();
            ddlDocStatus.Items.Insert(0, new ListItem("Select Doc Status", "0"));

        }
        #endregion

        public bool CheckNetAmount(string Range1, string Range2)
        {
            return true;
        }

        #region btnSearch_Click
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (textRange1.Text != "")
            {
                if (!IsDecimalLimitedtoOneDecimal(textRange1.Text))
                {
                    Response.Write(@"<script language='javascript'>alert('Only One Decimal point is allowed.');</script>");
                    textRange1.Focus();
                    textRange1.Text = "";
                    return;
                }
            }
            if (textRange2.Text != "")
            {
                if (!IsDecimalLimitedtoOneDecimal(textRange2.Text))
                {
                    Response.Write(@"<script language='javascript'>alert('Only One Decimal point is allowed.');</script>");
                    textRange2.Focus();
                    textRange2.Text = "";
                    return;
                }
            }

            if ((cbSupplier.Checked == false) && (HdSupplierId.Value == ""))
            {
                txtSupplier.Value = "";
            }
            if ((cbInvoiceNo.Checked == false) && (hdInvoiceNo.Value == ""))
            {
                txtInvoiceNo.Text = "";
            }

            // Added by Mrinal on 31st December 2014
            Session["RowID"] = null;
            Session["dtTiffViewer"] = null;
            Session["RowID"] = null;
            string x = string.Empty;
            string id = HdSupplierId.Value;
            if (Convert.ToString(ddldept.SelectedValue.Trim()) == "Select Department" || textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
            {
                //CheckDate();
                if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
                {

                }
                else
                {
                    if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                    {
                        lblMsg1.Text = "From Range cannot be greater than To Range";
                        lblMsg1.Visible = true;
                    }
                    else
                    {
                        lblMsg1.Text = "";
                        FromPrice = Convert.ToDecimal(textRange1.Text);
                        ToPrice = Convert.ToDecimal(textRange2.Text);
                    }
                }
            }
            else
            {
                if (Convert.ToString(ddldept.SelectedValue.Trim()) == "Select Department")
                {
                    //CheckDate();
                    if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
                    {

                    }
                    else
                    {
                        if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                        {
                            lblMsg1.Text = "From Range cannot be greater than To Range";
                            lblMsg1.Visible = true;
                        }
                        else
                        {
                            lblMsg1.Text = "";
                            FromPrice = Convert.ToDecimal(textRange1.Text);
                            ToPrice = Convert.ToDecimal(textRange2.Text);

                        }
                    }
                }
                else
                {
                    //CheckDate();
                    if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
                    {

                    }
                    else
                    {
                        if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                        {
                            lblMsg1.Text = "From Range cannot be greater than To Range";
                            lblMsg1.Visible = true;
                        }
                        else
                        {
                            lblMsg1.Text = "";
                            FromPrice = Convert.ToDecimal(textRange1.Text);
                            ToPrice = Convert.ToDecimal(textRange2.Text);

                        }
                    }
                }

            }
            try
            {
                grdInvCur.CurrentPageIndex = 0;
            }
            catch { }
            ViewState["dtCheckAttachment"] = null;
            LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), GetDocType(), Convert.ToInt32(Session["UserID"]));
        }
        #endregion

        #region IsDecimalLimitedtoOneDecimal
        public bool IsDecimalLimitedtoOneDecimal(string strValue)
        {
            bool Success = false;
            decimal number;
            string value1 = strValue;
            if (decimal.TryParse(value1, out number))
            {
                Regex rx = new Regex(@"[0-9]*\.?[0-9]*");

                if (!rx.IsMatch(value1))
                {
                    Success = false;
                }
                else
                {
                    Success = true;
                }

            }


            return Success;


        }

        #endregion

        #region GetDocType
        private string GetDocType()
        {
            string sDocType = "";
            if (ddlDocType.SelectedItem.Value != "0")
            {
                sDocType = ddlDocType.SelectedItem.Value;
            }
            return sDocType;
        }
        #endregion
        #region SetCompanyID
        private void SetCompanyID(string strCompanyID)
        {
            if (Convert.ToInt32(Session["UserTypeID"]) == 3)
            {
                ddlCompany.SelectedValue = strCompanyID.Trim();
            }
        }
        #endregion
        #region GetStatusURL
        protected string GetStatusURL(object oInvoiceID, object oDocType)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strDocumentType = Convert.ToString(oDocType);
            string strURL = "";
            if (strDocumentType.Trim() == "CRN")
            {
                strURL = "javascript:window.open('../../ETC/CreditNotes/InvoiceStatusLogNL_CN.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL_CN','width=550,height=450,scrollbars=1');";
            }
            else
            {
                strURL = "javascript:window.open('../../ETC/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL','width=705,height=450,scrollbars=1');";
            }
            return (strURL);
        }
        #endregion

        #region CheckDuplicateValues
        private void CheckDuplicateValues()
        {
            for (int i = 0; i < grdInvCur.Items.Count; i++)
            {
                if (i > 0)
                {
                    if ((grdInvCur.Items[i].Cells[1].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[1].Text.Trim())) && (grdInvCur.Items[i].Cells[4].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[4].Text.Trim())) && (grdInvCur.Items[i].Cells[5].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[5].Text.Trim())) && (grdInvCur.Items[i].Cells[2].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[2].Text.Trim())))
                    {

                    }
                }
            }
        }
        #endregion


        #region Getredirecturl
        protected string Getredirecturl(object DocType, object ReferenceNo, object InvoiceNo)
        {
            string URL = "";
            string Reference;
            string DocumentType = "";
            DocumentType = Convert.ToString(DocType);
            Reference = Convert.ToString(ReferenceNo);
            int invoiceID = Convert.ToInt32(InvoiceNo);
            if (DocumentType == "CRN")
            {
                URL = "<a href='../../ETC/CreditNotes/InvoiceConfirmationNL_CN.aspx?InvoiceID=" + invoiceID + "&AllowEdit=Current'>" + ReferenceNo + "</a>";
            }
            else if (DocumentType == "DBN")
            {
                URL = "<a href='../../ETC/invoice/InvoiceConfirmationNL.aspx?InvoiceID=" + invoiceID + "'>" + ReferenceNo + "</a>";
            }
            else if (DocumentType == "PO")
            {
                URL = "<a href='../../ETC/SalesOrders/InvoiceConfirmation_PO.aspx?PurchaseOrderID=" + invoiceID + "'>" + ReferenceNo + "</a>";
            }
            else
            {
                URL = "<a href='../../ETC/invoice/InvoiceConfirmationNL.aspx?InvoiceID=" + invoiceID + "&AllowEdit=Current'>" + ReferenceNo + "</a>";
            }
            return (URL);

        }

        #endregion
        #region grdInvCur_ItemCommand
        private void grdInvCur_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToUpper() == "ACT")
                {
                    int IsPermit = 0;
                    string sWinParam = "", sHeightWidth = "";

                    string strInvoiceID = ((Label)e.Item.FindControl("lblInvoiceID")).Text;
                    string strDocType = ((Label)e.Item.FindControl("lblDocType")).Text;
                    string strInvoiceDate = ((Label)e.Item.FindControl("lblInvoiceDate")).Text;
                    string strDocStatus = ((Label)e.Item.FindControl("lblDocStatus")).Text;
                    string strVoucherNumber = ((Label)e.Item.FindControl("lblVoucherNumber")).Text;

                    string strURL = "";
                    JKS.Invoice objinvoice = new JKS.Invoice();
                    string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

                    strRelationType = RelationType.ToString();
                    IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), strDocType);
                    if (IsPermit == 0)
                    {
                        if (RelationType == "STF" || RelationType == "STU")
                        {
                            sWinParam = "../../ETC/History/HistoryAction.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
                            sHeightWidth = "width=570,height=600,scrollbars=1,resizable=1";
                            strURL = "<script language=javascript>javascript:openBrWindow('" + sWinParam + "','','" + sHeightWidth + "')</script>";
                            this.RegisterClientScriptBlock("script", strURL);
                        }
                        else
                        {
                            if (strDocType == "CRN")
                            {
                                sWinParam = "ExpCreditNoteAction.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
                                sHeightWidth = "width=570,height=600,scrollbars=1,resizable=1";
                                strURL = "<script language=javascript>javascript:openBrWindow('" + sWinParam + "','','" + sHeightWidth + "')</script>";
                                this.RegisterClientScriptBlock("script", strURL);
                            }
                            if (strDocType == "INV")
                            {
                                sWinParam = "CurrentAction.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
                                sHeightWidth = "width=570,height=590,scrollbars=1,resizable=1";
                                strURL = "<script language=javascript>javascript:openBrWindow('" + sWinParam + "','','" + sHeightWidth + "')</script>";
                                this.RegisterClientScriptBlock("script", strURL);
                            }
                        }
                    }
                    else if (IsPermit == 1)
                    {
                        if (strDocType == "INV")
                        {
                            strURL = "<script language=javascript>alert('Another user has just actioned this Invoice and it has been locked out for 10 minutes. If after refreshing Internet Explorer you can still see the Invoice in your intray, it may then be actioned again after 10 minutes has elapsed.')</script>";
                            this.RegisterClientScriptBlock("script", strURL);
                            this.Page_Load(source, e);
                        }
                        else if (strDocType == "CRN")
                        {
                            strURL = "<script language=javascript>alert('Another user has just actioned this CreditNote and it has been locked out for 10 minutes. If after refreshing Internet Explorer you can still see the CreditNote in your intray, it may then be actioned again after 10 minutes has elapsed.')</script>";
                            this.RegisterClientScriptBlock("script", strURL);
                            this.Page_Load(source, e);
                        }
                    }
                }
            }
        }
        #endregion
        #region grdInvCur_ItemDataBound
        private void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            string sRetUrl = "../Current/CurrentStatusNew.aspx";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int iInvID = 0;
                iInvID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
                rptAttachment = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptAttachment");
                rptAttachment.DataSource = null;
                rptAttachment.DataBind();
                dtRepeater = GetAttachmentDetails(iInvID, Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")));
                if (dtRepeater.Rows.Count > 0)
                {
                    rptAttachment.DataSource = dtRepeater;
                    rptAttachment.DataBind();
                }



                // int iInvID = 0;
                //  iInvID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
                if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")).ToUpper() == "INV")
                {
                    ((HyperLink)e.Item.FindControl("hpDoc")).NavigateUrl = "../invoice/InvoiceFileManager_NL.aspx?From=ETC&InvoiceID=" + iInvID + "&ReturnUrl=" + sRetUrl;
                }
                else if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")).ToUpper() == "CRN")
                {
                    ((HyperLink)e.Item.FindControl("hpDoc")).NavigateUrl = "../creditnotes/CreditnoteFileManager_NL.aspx?From=ETC&CreditNoteID=" + iInvID + "&ReturnUrl=" + sRetUrl;
                }

                int sHold = objInvoice.GetAPCommLinkColor(iInvID, Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")));

                if (sHold == 1)
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/red_hold.gif";

                }
                else if (sHold == 0)
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/yellow_hold.gif";
                }
                else
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/green_hold.gif";
                }
                if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "New_VendorClass")).Equals("PO"))
                {
                    e.Item.BackColor = System.Drawing.Color.Red;
                }
                //if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IsDuplicate"))==1)
                //{
                //    e.Item.CssClass = "ColorDuplicateRow td";
                //}
                bool IsDuplicate = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsDuplicate"));
                if (IsDuplicate)
                {
                    e.Item.CssClass = "ColorDuplicateRow td";
                }
                //if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IsDuplicate")) == 1)
                //{
                //    e.Item.CssClass = "ColorDuplicateRow td";
                //}

                if (ViewState["dtCheckAttachment"] != null)
                {
                    DataTable dtAttachmentCheck = (DataTable)ViewState["dtCheckAttachment"];
                    DataView dvSelectedAttachment = new DataView(dtAttachmentCheck);
                    dvSelectedAttachment.Sort = "InvoiceID ASC";
                    dvSelectedAttachment.RowFilter = "InvoiceID=" + Convert.ToInt32(iInvID) + " And DocType='" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")).ToUpper() + "'";
                    if (dvSelectedAttachment.ToTable().Rows.Count > 0)
                    {
                        ((CheckBox)e.Item.FindControl("chkDownload")).Checked = true;
                    }
                }


            }

        }
        #endregion
        #region CheckDate

        private void CheckDate()
        {
            if (txtFromDate.Value != "" || txtToDate.Value != "")
            {
                if ((txtFromDate.Value.ToString().Trim().Length == 10) && (txtToDate.Value.ToString().Trim().Length == 10))
                {
                    strFromDate = DateTime.ParseExact(txtFromDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    strToDate = DateTime.ParseExact(txtToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    Response.Write(@"<script language='javascript'>alert('Date must be in dd/mm/yyyy format. Please select date from the calendar.');</script>");

                    if (txtFromDate.Value.ToString().Trim().Length < 10)
                    {
                        txtFromDate.Value = "";
                        strFromDate = "";
                    }
                    if (txtToDate.Value.ToString().Trim().Length < 10)
                    {
                        txtToDate.Value = "";
                        strToDate = "";
                    }
                }
            }
            else
            {
                strFromDate = "";
                strToDate = "";

            }
        }

        #endregion
        #region GetDatasetAgainstSQL
        public DataSet GetDatasetAgainstSQL(string sSql)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
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


        #region GetAPCommentsURL
        protected string GetAPCommentsURL(object oInvoiceID, object oDocType, object oDocNo, object oDocStatus)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strDocumentType = Convert.ToString(oDocType);
            string strDocNo = Convert.ToString(oDocNo);
            string strDocStatus = Convert.ToString(oDocStatus);

            string strURL = "";

            strURL = "javascript:window.open('../../ETC/invoice/APComments.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocumentType + "&DocNo=" + strDocNo + "&DocStatus=" + strDocStatus + "','InvoiceStatusLogNL','width=700,height=450,scrollbars=1');";

            return (strURL);
        }
        #endregion
        #region GetAllCategoryByAdmin()
        public void GetAllCategoryByAdmin()
        {
            try
            {
                // Added by Mrinal on 26th Feb 2015
                Session["RowID"] = null;
                DataTable dt = (DataTable)ViewState["objDataTable"];
                DataView dvgrdInvCur = new DataView(dt);
                dvgrdInvCur.Sort = strSortField + " " + strSortOrder;

                DataTable dtSorted = dvgrdInvCur.ToTable();
                int Counter = 0;
                foreach (DataRow RowsItem in dtSorted.Rows)
                {
                    Counter++;
                    string strPreviousRowID = RowsItem["RowID"].ToString();
                    string strInvoiceID = RowsItem["InvoiceID"].ToString();
                    string strDocType = RowsItem["DocType"].ToString();
                    RowsItem["RowID"] = Counter.ToString();
                    if (Session["dtTiffViewer"] != null)
                    {

                        //  DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
                        //  DataView dvTiffViewer = new DataView(dtTiffViewer);

                        //  dvTiffViewer.RowFilter = "RowID =" + Convert.ToInt32(strPreviousRowID) + " AND InvoiceID= " + Convert.ToInt32(strInvoiceID) + " AND DocType='" + strDocType + "' ";
                        //  dvTiffViewer.Sort = strSortField + " " + strSortOrder;
                        //  DataTable dtTiffViewerSorted = dvTiffViewer.ToTable();
                        ////  int TiffViewerCounter = 0;
                        //  foreach (DataRow TiffViewerRowsItem in dtTiffViewerSorted.Rows)
                        //  {
                        //     // TiffViewerCounter++;
                        //      TiffViewerRowsItem["RowID"] = Counter.ToString(); 
                        //  }
                        //  dtTiffViewerSorted.AcceptChanges();
                        //  Session["dtTiffViewer"] = dtTiffViewerSorted;



                        DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
                        //dtTiffViewer.Columns["RowID"].DataType = Type.GetType("System.Int32");
                        foreach (DataRow TiffViewerRowsItem in dtTiffViewer.Rows)
                        {
                            // TiffViewerCounter++;
                            if (Convert.ToInt32(TiffViewerRowsItem["InvoiceID"]) == Convert.ToInt32(strInvoiceID) && Convert.ToInt32(TiffViewerRowsItem["RowID"]) == Convert.ToInt32(strPreviousRowID) && Convert.ToString(TiffViewerRowsItem["DocType"]) == Convert.ToString(strDocType))
                            {
                                TiffViewerRowsItem["RowID"] = Counter.ToString();
                                break;
                            }
                        }
                        //  dtTiffViewer.Sort = strSortField + " " + strSortOrder;
                        dtTiffViewer.AcceptChanges();

                        /*
                         DataView dvTiffViewer = new DataView(dtTiffViewer);
                         dvTiffViewer.Sort = strSortField + " " + strSortOrder;
                         dtTiffViewer = dvTiffViewer.ToTable();
                         */

                        Session["dtTiffViewer"] = dtTiffViewer;

                    }
                }
                dtSorted.AcceptChanges();



                //if (Session["dtTiffViewer"] != null)
                //{

                //    DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
                //    DataView dvTiffViewer = new DataView(dtTiffViewer);
                //    dvTiffViewer.Sort = strSortField + " " + strSortOrder;

                //    DataTable dtTiffViewerSorted = dvTiffViewer.ToTable();
                //    int TiffViewerCounter = 0;
                //    foreach (DataRow RowsItem in dtTiffViewerSorted.Rows)
                //    {
                //        TiffViewerCounter++;
                //        RowsItem["RowID"] = TiffViewerCounter.ToString();
                //    }
                //    dtTiffViewerSorted.AcceptChanges();
                //    Session["dtTiffViewer"] = dtTiffViewerSorted;

                //}
                // Addition End on 26th Feb 2015


                //   grdInvCur.DataSource = dvgrdInvCur;
                grdInvCur.DataSource = dtSorted;
                grdInvCur.DataBind();
                ViewState["objDataTable"] = dtSorted;
                grdInvCur.Visible = true;
            }
            catch (Exception EX)
            {
                Response.Write("<script>alert('ERROR : " + EX.Message.ToString() + "')</script>");
            }
        }
        #endregion

        #region Sorting
        protected void grdInvCur_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            if (strSortOrder == "ASC")
                strSortOrder = "DESC";
            else
                strSortOrder = "ASC";

            strSortField = e.SortExpression;
            GetAllCategoryByAdmin();
        }
        #endregion
        private void GetBusinessUnit(int companyid)
        {
            ddlBusinessUnit.Items.Clear();
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
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBusinessUnit.DataSource = ds;
                    ddlBusinessUnit.DataValueField = "BusinessUnitID";
                    ddlBusinessUnit.DataTextField = "BusinessUnitName";
                    ddlBusinessUnit.DataBind();
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
                ds = null;
            }
            ddlBusinessUnit.Items.Insert(0, new ListItem("Select Business Unit", "0"));
        }

        #region:	Multiple Attachment DownloadbtnDownloadAttachment_Click
        private void btnDownloadAttachment_Click(object sender, System.EventArgs e)
        {
            DataTable dtAttachment = new DataTable();
            DataTable dtSelectedAttachment = new DataTable();
            try
            {

                if (ViewState["dtAttachment"] != null)
                {
                    dtAttachment = (DataTable)ViewState["dtAttachment"];
                    dtSelectedAttachment = dtAttachment.Clone();
                    /*

                    foreach (DataGridItem row in grdInvCur.Items)
                    {
                        if (((CheckBox)row.FindControl("chkDownload")).Checked)
                        {
                           

                            string strInvoiceID = ((Label)row.FindControl("lblInvoiceID")).Text.ToString();
                            string strDocType = ((Label)row.FindControl("lblDocType")).Text.ToString();

                            DataView dvSelectedAttachment = new DataView(dtAttachmentCheck);
                            dvSelectedAttachment.Sort = "InvoiceID ASC";
                            dvSelectedAttachment.RowFilter = "InvoiceID=" + Convert.ToInt32(strInvoiceID) + " And New_DocumentType='" + strDocType.ToString().Trim().ToUpper() + "'";
                            foreach (DataRow Dr in dvSelectedAttachment.ToTable().Rows)
                            {
                                dtSelectedAttachment.ImportRow(Dr);
                            }
                        }
                    }*/

                    if (ViewState["dtCheckAttachment"] != null)
                    {
                        DataTable dtCheckBoxAttachment = (DataTable)ViewState["dtCheckAttachment"];

                        string strInvoiceIDs = string.Empty;
                        string strCreditNoteIDs = string.Empty;

                        if (dtCheckBoxAttachment.Rows.Count > 0)
                        {
                            DataView dvSelectedCheckBoxForInvoice = new DataView();
                            #region: Fetch InvoiceIDs
                            dvSelectedCheckBoxForInvoice = new DataView(dtCheckBoxAttachment);
                            dvSelectedCheckBoxForInvoice.Sort = "InvoiceID ASC";
                            dvSelectedCheckBoxForInvoice.RowFilter = " DocType='INV'";
                            int Counter = 0;
                            foreach (DataRow drCheckBoxForInvoice in dvSelectedCheckBoxForInvoice.ToTable().Rows)
                            {
                                Counter++;
                                // dtSelectedAttachment.ImportRow(Dr);
                                if (Counter == dvSelectedCheckBoxForInvoice.ToTable().Rows.Count)
                                {
                                    strInvoiceIDs += drCheckBoxForInvoice["InvoiceID"].ToString();
                                }
                                else
                                {
                                    strInvoiceIDs += drCheckBoxForInvoice["InvoiceID"].ToString() + " , ";

                                }
                            }
                            if (strInvoiceIDs.ToString().Trim().Length > 0)
                            {
                                DataView dvSelectedAttachmentInvoice = new DataView(dtAttachment);
                                dvSelectedAttachmentInvoice.Sort = "InvoiceID ASC";
                                dvSelectedAttachmentInvoice.RowFilter = "InvoiceID in ( " + strInvoiceIDs + " ) And New_DocumentType='INV'";
                                foreach (DataRow Dr in dvSelectedAttachmentInvoice.ToTable().Rows)
                                {
                                    dtSelectedAttachment.ImportRow(Dr);
                                }
                                dtSelectedAttachment.AcceptChanges();
                            }
                            #endregion

                            #region: Fetch CreditNoteIDs
                            dvSelectedCheckBoxForInvoice = new DataView(dtCheckBoxAttachment);
                            dvSelectedCheckBoxForInvoice.Sort = "InvoiceID ASC";
                            dvSelectedCheckBoxForInvoice.RowFilter = " DocType='CRN'";
                            Counter = 0;
                            foreach (DataRow drCheckBoxForInvoice in dvSelectedCheckBoxForInvoice.ToTable().Rows)
                            {
                                Counter++;
                                // dtSelectedAttachment.ImportRow(Dr);
                                if (Counter == dvSelectedCheckBoxForInvoice.ToTable().Rows.Count)
                                {
                                    strCreditNoteIDs += drCheckBoxForInvoice["InvoiceID"].ToString();
                                }
                                else
                                {
                                    strCreditNoteIDs += drCheckBoxForInvoice["InvoiceID"].ToString() + " , ";

                                }
                            }

                            if (strCreditNoteIDs.ToString().Trim().Length > 0)
                            {
                                DataView dvSelectedAttachmentCreditNote = new DataView(dtAttachment);
                                dvSelectedAttachmentCreditNote.Sort = "InvoiceID ASC";
                                dvSelectedAttachmentCreditNote.RowFilter = "InvoiceID in ( " + strCreditNoteIDs + " ) And New_DocumentType='CRN'";
                                foreach (DataRow Dr in dvSelectedAttachmentCreditNote.ToTable().Rows)
                                {
                                    dtSelectedAttachment.ImportRow(Dr);
                                }
                                dtSelectedAttachment.AcceptChanges();
                            }
                            #endregion
                        }

                    }


                    dtSelectedAttachment.AcceptChanges();

                    if (dtSelectedAttachment != null && dtSelectedAttachment.Rows.Count > 0)
                    {

                        GenerateDownloadFiles(dtSelectedAttachment);
                    }
                    else
                    {
                        if (ViewState["dtAttachment"] != null)
                        {
                            GenerateDownloadFiles((DataTable)ViewState["dtAttachment"]);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Ex.ToString();

            }
            finally
            {
                dtAttachment.Dispose();
                dtSelectedAttachment.Dispose();
            }


            #region: Generarate Files
            //if (ViewState["dtAttachment"] != null)
            //{
            //    DataTable dtAttachment = (DataTable)ViewState["dtAttachment"];
            //    if (dtAttachment.Rows.Count > 0)
            //    {
            //        string ZipFileName = "AttachmentFiles_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".zip";
            //        string zipFullPath = Server.MapPath("~") + "\\Temp\\" + ZipFileName;
            //        ZipOutputStream zipOut = new ZipOutputStream(File.Create(zipFullPath));
            //        for (int i = 0; i < dtAttachment.Rows.Count; i++)
            //        {                        
            //            string FileNameInfo = dtAttachment.Rows[i]["ImagePath"].ToString();
            //            if (FileNameInfo == "" && FileNameInfo.Trim().Length <= 0)
            //            {
            //                FileNameInfo = dtAttachment.Rows[i]["ArchiveImagePath"].ToString();
            //            }
            //            int index = FileNameInfo.LastIndexOf("\\");
            //            int Length = FileNameInfo.Length;
            //            FileNameInfo = FileNameInfo.Substring(index + 1, (Length - index - 1));
            //            string extension = Path.GetExtension(FileNameInfo);
            //            FileNameInfo = dtAttachment.Rows[i]["SupplierCodeAgainstBuyer"].ToString() + "_" + dtAttachment.Rows[i]["DocType"].ToString() + "_" + dtAttachment.Rows[i]["InvoiceDate"].ToString() + "_" + dtAttachment.Rows[i]["InvoiceID"].ToString() + "_" + dtAttachment.Rows[i]["DocumentID"].ToString() + extension;//DocumentID+"_"+FileNameInfo;
            //            byte[] buff = ForceDownload(dtAttachment.Rows[i]["ImagePath"].ToString(), dtAttachment.Rows[i]["ArchiveImagePath"].ToString(), 0);
            //            if (buff != null)
            //            {
            //                //FileInfo fi = new FileInfo(filePath);
            //                //ZipEntry entry = new ZipEntry(fi.Name);
            //                //entry.DateTime = fi.LastWriteTime;
            //                ZipEntry entry = new ZipEntry(FileNameInfo);
            //                entry.DateTime = System.DateTime.Now;
            //                entry.Size = buff.Length;
            //                zipOut.PutNextEntry(entry);
            //                zipOut.Write(buff, 0, buff.Length);
            //            }


            //        }
            //        zipOut.Finish();

            //        zipOut.Close();
            //        //bool IsDownloaded=ForceDownload(zipFullPath,0);
            //        DownloadZip(zipFullPath, ZipFileName);
            //    }
            //}

            #endregion
        }

        public void GenerateDownloadFiles(DataTable dtDownload)
        {

            #region: Generarate Files
            if (dtDownload != null && dtDownload.Rows.Count > 0)
            {
                DataTable dtAttachment = dtDownload;
                if (dtAttachment.Rows.Count > 0)
                {
                    string ZipFileName = "AttachmentFiles_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".zip";
                    string zipFullPath = Server.MapPath("~") + "\\Temp\\" + ZipFileName;
                    ZipOutputStream zipOut = new ZipOutputStream(File.Create(zipFullPath));
                    for (int i = 0; i < dtAttachment.Rows.Count; i++)
                    {

                        //string filePath =DownloadFile(dtAttachment.Rows[i]["ImagePath"].ToString(),dtAttachment.Rows[i]["ArchiveImagePath"].ToString());
                        string FileNameInfo = dtAttachment.Rows[i]["ImagePath"].ToString();
                        if (FileNameInfo == "" && FileNameInfo.Trim().Length <= 0)
                        {
                            FileNameInfo = dtAttachment.Rows[i]["ArchiveImagePath"].ToString();
                        }
                        int index = FileNameInfo.LastIndexOf("\\");
                        int Length = FileNameInfo.Length;
                        FileNameInfo = FileNameInfo.Substring(index + 1, (Length - index - 1));
                        string extension = Path.GetExtension(FileNameInfo);
                        FileNameInfo = dtAttachment.Rows[i]["SupplierCodeAgainstBuyer"].ToString() + "_" + dtAttachment.Rows[i]["DocType"].ToString() + "_" + dtAttachment.Rows[i]["InvoiceDate"].ToString() + "_" + dtAttachment.Rows[i]["InvoiceID"].ToString() + "_" + dtAttachment.Rows[i]["DocumentID"].ToString() + extension;//DocumentID+"_"+FileNameInfo;
                        byte[] buff = ForceDownload(dtAttachment.Rows[i]["ImagePath"].ToString(), dtAttachment.Rows[i]["ArchiveImagePath"].ToString(), 0);
                        if (buff != null)
                        {
                            //FileInfo fi = new FileInfo(filePath);
                            //ZipEntry entry = new ZipEntry(fi.Name);
                            //entry.DateTime = fi.LastWriteTime;
                            ZipEntry entry = new ZipEntry(FileNameInfo);
                            entry.DateTime = System.DateTime.Now;
                            entry.Size = buff.Length;
                            zipOut.PutNextEntry(entry);
                            zipOut.Write(buff, 0, buff.Length);
                        }


                    }
                    zipOut.Finish();

                    zipOut.Close();
                    //bool IsDownloaded=ForceDownload(zipFullPath,0);
                    DownloadZip(zipFullPath, ZipFileName);
                }
            }

            #endregion
        }
        #endregion
        #region: Multiple Attachment Download implementation
        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }
        private string GetURL2()
        {
            return ConfigurationManager.AppSettings["ServiceURLNew"];
        }
        private string DownloadFile(string ImagePath, string ArchPath)
        {
            bool bFound = false;
            //string sDownLoadPath=((Label)e.Item.FindControl("lblHidePath")).Text;
            string sDownLoadPath = ImagePath;
            sDownLoadPath = sDownLoadPath.Replace("I:", "C:\\P2D");
            sDownLoadPath = sDownLoadPath.Replace("\\90104-server2", "C:\\P2D");
            sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
            if (sDownLoadPath.Trim() != "")
            {
                if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                {
                    try
                    {
                        if (File.Exists(sDownLoadPath))
                        {
                            bFound = true;
                        }
                        else
                        {
                            bFound = false;
                        }
                        //bFound=ForceDownload(sDownLoadPath,0);
                    }
                    catch (Exception Ex)
                    {
                        string Error = Ex.ToString();
                    }
                }
                else
                {
                    if (File.Exists(sDownLoadPath))
                    {
                        bFound = true;
                    }
                    else
                    {
                        bFound = false;
                    }
                    //bFound=ForceDownload(sDownLoadPath,0);
                }
            }
            if (bFound == false)
            {
                //sDownLoadPath=((Label)e.Item.FindControl("lblArchPath")).Text;
                sDownLoadPath = ArchPath;
                sDownLoadPath = sDownLoadPath.Replace("\\90107-server3", @"C:\File Repository");
                sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);

                if (sDownLoadPath.Trim() != "")
                {
                    if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                    {
                        try
                        {
                            if (File.Exists(sDownLoadPath))
                            {
                                bFound = true;
                            }
                            else
                            {
                                bFound = false;
                            }
                            //bFound=ForceDownload(sDownLoadPath,1);
                        }
                        catch (Exception Ex)
                        {
                            string Error = Ex.ToString();
                        }
                    }
                    else
                    {
                        //bFound=ForceDownload(sDownLoadPath,1);
                        if (File.Exists(sDownLoadPath))
                        {
                            bFound = true;
                        }
                        else
                        {
                            bFound = false;
                        }
                    }
                }
            }
            return sDownLoadPath;
        }
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
        private string GetBuyerCompanyName(int iInvoiceID, string DocType)
        {
            string strBuyerCompanyName = string.Empty;

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("GetBuyerCompanyNameAgainstInvoice", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string strExceptionMessage = ex.Message.Trim();

            }
            finally
            {
                if (sqlDA != null)
                    sqlDA.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }
            //DataTable dt=new DataTable();
            if (ds.Tables.Count > 0)
            {
                strBuyerCompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
            }


            return strBuyerCompanyName;


        }

        private byte[] ForceDownload(string ImagePath, string ArchPath, int iStep)
        {
            bool bRetVal = false;
            byte[] bytBytesFinal = null;
            string sFileName = string.Empty;
            if (iStep == 0)
            {
                sFileName = ImagePath;
                sFileName = sFileName.Replace("I:", "C:\\P2D");
                sFileName = sFileName.Replace("\\90104-server2", "C:\\P2D");
                sFileName = GetTrimFirstSlash(sFileName);
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = GetURL();
                    byte[] bytBytes = objService.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        bytBytesFinal = bytBytes;
                        //						Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        //						Response.ContentType = "application//octet-stream";
                        //						Response.BinaryWrite(bytBytes);
                        //						Response.Flush();
                        //						Response.End();
                        //						fs1.Close();
                        //						fs1 = null;
                        //						bRetVal=true;

                    }
                    else
                    {
                        bytBytesFinal = ForceDownload(ImagePath, ArchPath, 1);
                    }
                }
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            else if (iStep == 1)
            {
                sFileName = ArchPath;
                sFileName = sFileName.Replace("\\90107-server3", @"C:\File Repository");
                sFileName = GetTrimFirstSlash(sFileName);
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
                    objService2.Url = GetURL2();
                    byte[] bytBytes = objService2.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        bytBytesFinal = bytBytes;
                        //						Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        //						Response.ContentType = "application//octet-stream";
                        //						Response.BinaryWrite(bytBytes);
                        //						Response.Flush();
                        //						Response.End();
                        //						fs1.Close();
                        //						fs1 = null;
                        //						bRetVal=true;
                    }
                }
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            return bytBytesFinal;
        }
        public void DownloadZip(string Path, string ZipFileName)
        {
            string filepath = Path;
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/zip";
                //Context.Response.AddHeader("content-disposition","attachment; filename="+Path.GetFileName(Path));
                Context.Response.AppendHeader("content-disposition", "attachment; filename=" + ZipFileName);
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                long FileSize = fs.Length;
                byte[] getContent = new byte[(int)FileSize];
                fs.Read(getContent, 0, (int)fs.Length);
                fs.Close();
                Context.Response.BinaryWrite(getContent);
                Context.Response.Flush();
                Context.Response.End();

            }
            catch (Exception ex) { throw (ex); }

        }

        #endregion

        protected string GetTiffViewersURL(object oID, object oDocType)
        {
            string strInvoiceID = Convert.ToString(oID);
            string strDocumentType = Convert.ToString(oDocType);
            int RowID = 0;
            if (Session["dtTiffViewer"] != null)
            {
                DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
                if (dtTiffViewer.Rows.Count > 0)
                {
                    DataView dvTiffViewer = new DataView(dtTiffViewer);

                    dvTiffViewer.Sort = "RowID ASC";
                    dvTiffViewer.RowFilter = "InvoiceID=" + Convert.ToInt32(strInvoiceID);
                    RowID = Convert.ToInt32(dvTiffViewer[0]["RowID"].ToString());
                }

            }


            string strURL = "";

            strURL = "javascript:window.open('../../TiffViewerDefault.aspx?ID=" + strInvoiceID + "&Type=" + strDocumentType + "','TiffViewer','width=650,height=450,top=100,left=150,scrollbars=1,resizable=1');";

            return (strURL);
        }
        #region GetURLTest
        protected string GetURLTest(object oInvoiceID, object oDocType, object oVat, object oTatal, object NewVendorClass, object RowID)
        {
            int IsPermit = 0;
            JKS.Invoice objinvoice = new JKS.Invoice();
            string DocType = Convert.ToString(oDocType);
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strVat = Convert.ToString(oVat);
            string strTaotal = Convert.ToString(oTatal);
            string strURL = "";
            string strNewVendorClass = Convert.ToString(NewVendorClass);
            // Added by Mrinal on 22nd September 2014
            string strRowID = Convert.ToString(RowID);
            string strTiffViewerurl = GetTiffViewersURL(strInvoiceID, DocType);

            if (DocType == "INV")
            {
                string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

                strRelationType = RelationType.Trim();
                IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
                if (IsPermit == 0)
                {
                    //strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"&RelationType="+strRelationType+"&iVat="+strVat+"&iGross="+strTaotal+"','abb','width=1150,height=750,top=100,left=100,scrollbars=1,resizable=1');";//rinku 02-02-2011
                    strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','InvoiceAction','width=540,height=450,top=100,left=805,scrollbars=1,resizable=1');";
                    // Added by Mrinal on 22nd September 2014
                    strURL = strURL + strTiffViewerurl;
                }
                else if (IsPermit == 1)
                {
                    strURL = "javascript:alert('This invoice has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
                }
                else if (IsPermit == 2)
                {
                    strURL = "javascript:alert('You cannot action a rejected invoice');";
                }
            }
            if (DocType == "CRN")
            {
                IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
                if (IsPermit == 0)
                {
                    //					if(strDocStatus=="Rejected")
                    //						strURL = "javascript:window.open('../CreditNotes/ActionForRejectedCreditNote.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"','abb','width=1080,height=750,top=100,left=100,scrollbars=1,resizable=1');";					
                    //					else

                    //strURL = "javascript:window.open('../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"','abb','width=1150,height=750,top=100,left=100,scrollbars=1,resizable=1');";	
                    //'width=1240,height=750,top=100,left=750,scrollbars=1,resizable=1'
                    strURL = "javascript:window.open('../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','CreditNoteAction','width=550,height=450,top=100,left=805,scrollbars=1,resizable=1');";
                    // Added by Mrinal on 22nd September 2014
                    strURL = strURL + strTiffViewerurl;
                }
                else
                    strURL = "javascript:alert('This credit note has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
            }
            return (strURL);
        }
        #endregion
        protected string IFrameWindow(object oInvoiceID, object oDocType, object oVat, object oTatal, object NewVendorClass, object RowID)
        {
            bool IsIFrameNeeded = false;
            int IsPermit = 0;
            JKS.Invoice objinvoice = new JKS.Invoice();
            string DocType = Convert.ToString(oDocType);
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strVat = Convert.ToString(oVat);
            string strTaotal = Convert.ToString(oTatal);
            string strURL = "";
            string strNewVendorClass = Convert.ToString(NewVendorClass);
            string strRowID = Convert.ToString(RowID);
            string strTiffViewerurl = GetTiffViewersURL(strInvoiceID, DocType);

            if (DocType == "INV")
            {
                string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

                strRelationType = RelationType.Trim();
                IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
                if (IsPermit == 0)
                {
                    IsIFrameNeeded = true;
                    //strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"&RelationType="+strRelationType+"&iVat="+strVat+"&iGross="+strTaotal+"','abb','width=1150,height=750,top=100,left=100,scrollbars=1,resizable=1');";//rinku 02-02-2011
                    strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','InvoiceAction','width=540,height=450,top=100,left=805,scrollbars=1,resizable=1');";
                    // Added by Mrinal on 22nd September 2014
                    strURL = strURL + strTiffViewerurl;
                }
                else if (IsPermit == 1)
                {
                    strURL = "javascript:alert('This invoice has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
                }
                else if (IsPermit == 2)
                {
                    strURL = "javascript:alert('You cannot action a rejected invoice');";
                }
            }
            if (DocType == "CRN")
            {
                IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
                if (IsPermit == 0)
                {
                    IsIFrameNeeded = true;
                    strURL = "javascript:window.open('../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','CreditNoteAction','width=550,height=450,top=100,left=805,scrollbars=1,resizable=1');";
                    // Added by Mrinal on 22nd September 2014
                    strURL = strURL + strTiffViewerurl;
                }
                else
                    strURL = "javascript:alert('This credit note has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
            }
            if (IsIFrameNeeded)
            {
                if (DocType == "CRN")
                {

                    //strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','fullscreen,scrollbars');";
                    strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=' + screen.height + ',width=' + screen.width +',scrollbars,top=0,left=0,resizable=0');";

                }
                else if (DocType == "INV")
                {
                    // strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','fullscreen,scrollbars');";
                    strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=' + screen.height + ',width=' + screen.width +',scrollbars,top=0,left=0,resizable=0');";
                }
            }
            return (strURL);
        }

        #region :  Process Button
        public void btnProcess_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("../../close_win.aspx");
            }
            else
            {
                GetURLProcess();
            }
        }

        protected void GetURLProcess()
        {
            int myIntUserTypeID = Convert.ToInt32(Session["UserTypeID"]);
            // ---------------------------------
            bool isWindowNeedToClose = false;

            int RowID = 0;
            if (Session["RowID"] != null)
            {
                RowID = System.Convert.ToInt32(Session["RowID"]);
                //Session["RowID"] = Convert.ToString(RowID + 1);
            }
            if (RowID != 0)
            {
                string strInvoiceID = "";
                string strVat = "";
                string strTotal = "";
                string strNewVendorClass = "";
                string strRowID = "";
                string strDocType = "INV";
                string DocType = Convert.ToString("INV");
                string strDDCompanyID = "0";
                if (Request["DDCompanyID"] != null)
                {
                    strDDCompanyID = Request["DDCompanyID"].ToString();
                }
                if (Session["dtTiffViewer"] != null)
                {
                    DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
                    if (dtTiffViewer.Rows.Count > 0)
                    {

                        DataTable dtTiffViewerModified = dtTiffViewer.Clone();
                        dtTiffViewerModified.Columns["RowID"].DataType = Type.GetType("System.Int32");

                        foreach (DataRow dr in dtTiffViewer.Rows)
                        {
                            dtTiffViewerModified.ImportRow(dr);
                        }
                        dtTiffViewerModified.AcceptChanges();
                        DataView dv = dtTiffViewerModified.DefaultView;
                        dv.Sort = "RowID ASC";

                        dtTiffViewer = dv.ToTable();



                        //dtTiffViewer.Columns["RowID"].DataType = Type.GetType("System.Int32");
                        //dtTiffViewer.DefaultView.Sort = "RowID ASC";
                        //dtTiffViewer.AcceptChanges();
                        // dvTiffViewer.Sort = "RowID ASC";

                        DataView dvTiffViewer = new DataView(dtTiffViewer);
                        DataView dvTiffViewerCreditNote = new DataView(dtTiffViewer);
                        if (myIntUserTypeID == 2 || myIntUserTypeID == 3)
                        {
                            dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND DocType='" + strDocType + "' ";
                            dvTiffViewerCreditNote.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND DocType='" + Convert.ToString("CRN") + "' ";
                        }
                        else
                        {
                            dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND InvStatusID in (21,22) AND DocType='" + strDocType + "' ";
                            dvTiffViewerCreditNote.RowFilter = "RowID >" + Convert.ToInt32(RowID) + "  AND InvStatusID in (21,22) AND DocType='" + Convert.ToString("CRN") + "' ";
                        }
                        dvTiffViewerCreditNote.Sort = "RowID ASC";
                        dtTiffViewer.Columns["RowID"].DataType = Type.GetType("System.Int32");
                        dvTiffViewer.Sort = "RowID ASC";

                        if (dvTiffViewer.Count > 0)
                        {
                            strRowID = Convert.ToString(dvTiffViewer[0]["RowID"].ToString());
                            strInvoiceID = Convert.ToString(dvTiffViewer[0]["InvoiceID"].ToString());
                            strVat = Convert.ToString(dvTiffViewer[0]["VAT"].ToString());
                            strTotal = Convert.ToString(dvTiffViewer[0]["Total"].ToString());
                            strNewVendorClass = Convert.ToString(dvTiffViewer[0]["New_VendorClass"].ToString());
                            strDocType = Convert.ToString(dvTiffViewer[0]["DocType"].ToString().ToUpper().Trim());
                            DocType = Convert.ToString("INV");

                            if (dvTiffViewerCreditNote.Count > 0)
                            {
                                int strCRNRowID = Convert.ToInt32(dvTiffViewerCreditNote[0]["RowID"].ToString());
                                if (strCRNRowID <= Convert.ToInt32(strRowID))
                                {
                                    // ------------------------

                                    strDocType = "CRN";
                                    ////dvTiffViewer = new DataView(dtTiffViewer);

                                    ////if (myIntUserTypeID == 2 || myIntUserTypeID == 3)
                                    ////{
                                    ////    dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND DocType='" + strDocType + "' ";
                                    ////}
                                    ////else
                                    ////{
                                    ////    dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND InvStatusID in (21,22) AND DocType='" + strDocType + "' ";
                                    ////}
                                    ////// dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND InvStatusID in (21,22) AND DocType='" + strDocType + "' ";
                                    if (dvTiffViewerCreditNote.Count > 0)
                                    {
                                        strRowID = Convert.ToString(dvTiffViewerCreditNote[0]["RowID"].ToString());
                                        strInvoiceID = Convert.ToString(dvTiffViewerCreditNote[0]["InvoiceID"].ToString());
                                        strVat = Convert.ToString(dvTiffViewerCreditNote[0]["VAT"].ToString());
                                        strTotal = Convert.ToString(dvTiffViewerCreditNote[0]["Total"].ToString());
                                        strNewVendorClass = Convert.ToString(dvTiffViewerCreditNote[0]["New_VendorClass"].ToString());
                                        strDocType = Convert.ToString(dvTiffViewerCreditNote[0]["DocType"].ToString().ToUpper().Trim());
                                        DocType = Convert.ToString("CRN");
                                    }
                                    else
                                    {
                                        btnSearch_Click(null, null);
                                        return;
                                    }


                                    //        -----------------------------


                                }

                            }




                        }
                        else
                        {
                            strDocType = "CRN";
                            dvTiffViewer = new DataView(dtTiffViewer);

                            if (myIntUserTypeID == 2 || myIntUserTypeID == 3)
                            {
                                dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND DocType='" + strDocType + "' ";
                            }
                            else
                            {
                                dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND InvStatusID in (21,22) AND DocType='" + strDocType + "' ";
                            }
                            // dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND InvStatusID in (21,22) AND DocType='" + strDocType + "' ";
                            if (dvTiffViewer.Count > 0)
                            {
                                strRowID = Convert.ToString(dvTiffViewer[0]["RowID"].ToString());
                                strInvoiceID = Convert.ToString(dvTiffViewer[0]["InvoiceID"].ToString());
                                strVat = Convert.ToString(dvTiffViewer[0]["VAT"].ToString());
                                strTotal = Convert.ToString(dvTiffViewer[0]["Total"].ToString());
                                strNewVendorClass = Convert.ToString(dvTiffViewer[0]["New_VendorClass"].ToString());
                                strDocType = Convert.ToString(dvTiffViewer[0]["DocType"].ToString().ToUpper().Trim());
                                DocType = Convert.ToString("CRN");
                            }
                            else
                            {
                                btnSearch_Click(null, null);
                                return;
                            }
                        }
                    }

                }


                // ---------------------------------


                int IsPermit = 0;
                JKS.Invoice objinvoice = new JKS.Invoice();
                string strURL = "";

                if (Session["IsProcessed"] != null)
                {
                    // Session["RowID"] = Convert.ToString(RowID + 1);
                    if (DocType == "INV")
                    {
                        string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

                        IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
                        if (IsPermit == 0)
                        {
                            strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID=" + strDDCompanyID + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + RelationType.Trim().ToString() + "&iVat=" + strVat + "&iGross=" + strTotal + "&RowID=" + strRowID.ToString() + "','IFRAMEWINDOW','fullscreen,scrollbars');";

                        }
                        else if (IsPermit == 1)
                        {
                            strURL = "javascript:alert('This invoice has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
                        }
                        else if (IsPermit == 2)
                        {
                            strURL = "javascript:alert('You cannot action a rejected invoice');";
                        }
                    }
                    if (DocType == "CRN")
                    {
                        IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
                        if (IsPermit == 0)
                        {
                            strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID=" + strDDCompanyID + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','fullscreen,scrollbars');";

                        }
                        else
                            strURL = "javascript:alert('This credit note has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
                    }

                    if (strURL.Trim().Length > 0)
                    {
                        Response.Write("<script>" + strURL + "</script>");
                    }
                }
                else
                {
                    btnSearch_Click(null, null);
                }
            }
            else
            {
                btnSearch_Click(null, null);
            }


        }
        #endregion
        #region: New Implementation on 28th January 2015 Developed by Mrinal
        protected void rptAttachment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnrptAttachment = (Button)e.Item.FindControl("btnAttachment");
                btnrptAttachment.Text = "Doc" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "COUNTER"));
                btnrptAttachment.Click += new EventHandler(btnrptAttachment_Click);
            }
        }

        private DataTable GetAttachmentDetails(int iInvoiceID, string DocType)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("GetUploadFileDetailsTypeWise", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string strExceptionMessage = ex.Message.Trim();

            }
            finally
            {
                if (sqlDA != null)
                    sqlDA.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }


            return new DataTable();


        }

        protected void rptAttachment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToUpper() == "DOW")
                {
                    int DocumentID = Convert.ToInt32(e.CommandArgument);

                    string strAttachmentImagePath = ((Label)e.Item.FindControl("lblAttachmentImagePath")).Text;
                    string strAttachmentArchiveImagePath = ((Label)e.Item.FindControl("lblAttachmentArchiveImagePath")).Text;
                }
            }
        }

        protected void btnrptAttachment_Click(object sender, EventArgs e)
        {
            Button btnrptAttachment = (Button)sender;
            RepeaterItem rptItem = (RepeaterItem)btnrptAttachment.NamingContainer;
            //string lblAttachmentImagePath = ((Label) rptItem.FindControl("lblAttachmentImagePath")).Text;
            //string strAttachmentArchiveImagePath = ((Label) rptItem.FindControl("lblAttachmentArchiveImagePath")).Text;
            string strDocumentID = ((Label)rptItem.FindControl("lblDocumentID")).Text;

            bool bFound = false;
            string sDownLoadPath = ((Label)rptItem.FindControl("lblAttachmentImagePath")).Text;
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
                sDownLoadPath = ((Label)rptItem.FindControl("lblAttachmentArchiveImagePath")).Text;
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
        private bool ForceDownload(string sPath, int iStep)
        {
            bool bRetVal = false;
            string sFileName = sPath;
            if (iStep == 0)
            {
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
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
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            else if (iStep == 1)
            {
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
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
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            return bRetVal;
        }

        #endregion


        [WebMethod]
        public static string[] GetSupplier(string CompanyID, string userId, string userTypeID, string UserString)
        {
            DataSet dsSupplier = new DataSet();
            Invoice_New objInv = new Invoice_New();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetSuppliersList_AkkeronNew", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(userId));
            sqlDA.SelectCommand.Parameters.Add("@USerTypeID", Convert.ToInt32(userTypeID));
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyString", UserString);

            try
            {
                sqlConn.Open();
                sqlDA.Fill(dsSupplier);
            }
            catch (Exception ex)
            {
                string strExceptionMessage = ex.Message.Trim();
            }
            finally
            {
                if (sqlDA != null)
                    sqlDA.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }

            List<string> myList = new List<string>();
            if (dsSupplier != null && dsSupplier.Tables.Count > 0 && dsSupplier.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsSupplier.Tables[0].Rows)
                {
                    myList.Add(string.Format("{0}#{1}", row["CompanyID"].ToString(), row["label"].ToString()));
                    //myList.Add(string.Format("{0}", row["label"].ToString()));
                }
                return myList.ToArray();
                // return JsonConvert.SerializeObject(dsSupplier.Tables[0]);
            }
            else
                return null;
            // return "";
        }

        [WebMethod]
        public static string[] GetNominalName(string CompanyID, string UserString)
        {
            DataSet dsNominalName = new DataSet();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetNominalName", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(CompanyID));
            sqlDA.SelectCommand.Parameters.Add("@NominalStr", UserString);

            try
            {
                sqlConn.Open();
                sqlDA.Fill(dsNominalName);
            }
            catch (Exception ex)
            {
                string strExceptionMessage = ex.Message.Trim();
            }
            finally
            {
                if (sqlDA != null)
                    sqlDA.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }

            List<string> myList = new List<string>();
            if (dsNominalName != null && dsNominalName.Tables.Count > 0 && dsNominalName.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsNominalName.Tables[0].Rows)
                {
                    myList.Add(string.Format("{0}#{1}", row["NominalCodeID"].ToString(), row["label"].ToString()));
                    // myList.Add(string.Format("{0}", row["label"].ToString()));
                }
                return myList.ToArray();
                // return JsonConvert.SerializeObject(dsSupplier.Tables[0]);
            }
            else
                return null;

        }

        [WebMethod]
        public static List<string> FetchInvoiceNo(string CompanyID, string DocType, string UserString)
        {
            DataSet dsInvoiceNo = new DataSet();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            //added by kuntal on 16.03.2015------------------------------------------
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_FetchInvoiceNo_13thMar2015", sqlConn);
            //-----------------------------------------------------------------------
            //SqlDataAdapter sqlDA = new SqlDataAdapter("sp_FetchInvoiceNo", sqlConn);

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            if (DocType != "")
                sqlDA.SelectCommand.Parameters.Add("@DocType", DocType.Trim());
            else
                sqlDA.SelectCommand.Parameters.Add("@DocType", DBNull.Value);
            sqlDA.SelectCommand.Parameters.Add("@InvoiceString", UserString.Trim());
            sqlDA.SelectCommand.CommandTimeout = 0;
            try
            {
                sqlConn.Open();
                sqlDA.Fill(dsInvoiceNo, "InvoiceNo");
            }
            catch (Exception ex)
            {
                string strExceptionMessage = ex.Message.Trim();
            }
            finally
            {
                if (sqlDA != null)
                    sqlDA.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }

            List<string> myList = new List<string>();
            if (dsInvoiceNo != null && dsInvoiceNo.Tables.Count > 0 && dsInvoiceNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsInvoiceNo.Tables[0].Rows)
                {
                    myList.Add(row["InvoiceNo"].ToString());
                }
            }
            return myList;
        }




        #region: Checkbox Events
        public void chkDownload_CheckedChanged(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            CheckBox chkDownload = (CheckBox)sender;
            DataGridItem grdItem = chkDownload.NamingContainer as DataGridItem;

            /*
            DataTable dtCheckAttachment = new DataTable();
            dtCheckAttachment.Columns.Add("InvoiceID");
            dtCheckAttachment.Columns.Add("DocType");
            dtCheckAttachment.AcceptChanges();
            foreach (DataGridItem row in grdInvCur.Items)
            {
                if (((CheckBox)row.FindControl("chkDownload")).Checked)
                {

                    string strInvoiceID = ((Label)row.FindControl("lblInvoiceID")).Text.ToString();
                    string strDocType = ((Label)row.FindControl("lblDocType")).Text.ToString();
                    DataRow dr = dtCheckAttachment.NewRow();

                    dr["InvoiceID"] = strInvoiceID;
                    dr["DocType"] = strDocType;
                    dtCheckAttachment.Rows.Add(dr);
                }
            }
            dtCheckAttachment.AcceptChanges();
            ViewState["dtCheckAttachment"] = dtCheckAttachment;
            */

            DataTable dtCheckAttachment = new DataTable();
            if (ViewState["dtCheckAttachment"] != null)
            {
                dtCheckAttachment = (DataTable)ViewState["dtCheckAttachment"];
            }
            else
            {
                dtCheckAttachment.Columns.Add("InvoiceID");
                dtCheckAttachment.Columns.Add("DocType");
                dtCheckAttachment.AcceptChanges();

            }

            string strInvoiceID = ((Label)grdItem.FindControl("lblInvoiceID")).Text.ToString();
            string strDocType = ((Label)grdItem.FindControl("lblDocType")).Text.ToString();
            if (chkDownload.Checked)
            {
                DataRow dr = dtCheckAttachment.NewRow();

                dr["InvoiceID"] = strInvoiceID;
                dr["DocType"] = strDocType;
                dtCheckAttachment.Rows.Add(dr);
            }
            else
            {
                foreach (DataRow RowsItem in dtCheckAttachment.Rows)
                {
                    //  bool IsMatched = false;
                    if (RowsItem["InvoiceID"].ToString() == strInvoiceID && RowsItem["DocType"].ToString() == strDocType)
                    {
                        dtCheckAttachment.Rows.Remove(RowsItem);
                        break;
                    }
                }
                dtCheckAttachment.AcceptChanges();

            }
            ViewState["dtCheckAttachment"] = dtCheckAttachment;
            iNeedRefreshToBottom = 1;
            //iNeedRefreshToBottom = 0;
            // chkDownload.Focus();
        }
        #endregion
    }
}