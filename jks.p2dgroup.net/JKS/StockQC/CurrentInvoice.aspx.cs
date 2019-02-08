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
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Text.RegularExpressions;
using CBSolutions.ETH.Web;


namespace JKS
{
    [ScriptService]
    public partial class CurrentInvoice : CBSolutions.ETH.Web.ETC.VSPage
    {

        #region User Defined Variables
        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;

        protected DataSet ds = null;
        protected DataTable objDataTable = null;
        protected DataRow drInvoiceHeader = null;
        protected DataRow drInvoiceInvoiceLog = null;
        protected DataRow dr = null;

        JKS.Invoice objInvoice = new JKS.Invoice();
        protected string strInvoiceDocumentDownloadPath = ConfigurationManager.AppSettings["InvoiceDocPath"];
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

        int currentYear = 0;
        private int iLoadFlag = 0;
        private string strFromDate = "";
        private string strToDate = "";

        private decimal FromPrice;
        private decimal ToPrice;


        // Added by Mrinal on 28th January 2015
        DataTable dtRepeater = new DataTable();
        protected System.Web.UI.WebControls.Repeater rptAttachment;

        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            lblMsg.Visible = false;
            lblMsg1.Visible = false;
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            if (!IsPostBack)
            {
                Utility.makeDefaultButton(txtInvoiceNo, btnSearch);
                Utility.makeDefaultButton(textRange1, btnSearch);
                Utility.makeDefaultButton(textRange2, btnSearch);

                Session["ApproveForm"] = 0;
                Session["SelectedPage"] = "PurchaseInvoiceLog";
                iLoadFlag++;
                Session["DropDownCompanyID"] = null;
                btnSearch.Attributes.Add("onclick", "javascript:return fn_Validate();");
                //SetCompanyID(Session["CompanyID"].ToString());
                String str1 = "BuyerID=" + ((int)Session["CompanyID"]).ToString() + " and StatusId = 7";
                String str2 = "BuyerID=" + ((int)Session["CompanyID"]).ToString() + " and (StatusId != 7 or StatusId is null)";
                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
                LoadDate();

                //if (Session["DropDownCompanyID"] != null)
                //    LoadData(Convert.ToInt32(Session["DropDownCompanyID"]));
                //else
                //    LoadData(Convert.ToInt32(Session["CompanyID"]));

                //cbSupplier.Checked = true;
                //cbInvoiceNo.Checked = true;
                //divP2DLogo.Visible = true;
                //Blocked by Mrinal on 31st December 2013
                if (Convert.ToInt32(Session["UserTypeID"]) != 2 && Convert.ToInt32(Session["UserTypeID"]) != 3)
                {
                    if (Session["DropDownCompanyID"] != null)
                        LoadData(Convert.ToInt32(Session["DropDownCompanyID"]));
                    else
                        LoadData(Convert.ToInt32(Session["CompanyID"]));
                }
                if (Convert.ToInt32(Session["UserTypeID"]) == 3 || Convert.ToInt32(Session["UserTypeID"]) == 2)
                {
                    cbSupplier.Checked = true;
                    cbInvoiceNo.Checked = true;
                    divP2DLogo.Visible = true;

                }
                //modified by kuntal on 28.03.2015 pt.46-----
                //populateVendorClass(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                //------------------------------------------
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
            {

                cbSupplier.Checked = true;
                cbSupplier.Disabled = true;

            }

            if (Convert.ToInt32(Session["UserTypeID"]) == 3)
            {
                cbSupplier.Checked = true;
                cbSupplier.Disabled = true;


            }

            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                cbSupplier.Checked = true;
                cbSupplier.Disabled = true;


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
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
            this.grdInvCur.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.grdInvCur_PageIndexChanged);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.Load += new System.EventHandler(this.Page_Load);

            this.grdInvCur.ItemDataBound += new DataGridItemEventHandler(grdInvCur_ItemDataBound);
            this.btnProcess.Click += new EventHandler(btnProcess_Click);

        }
        public void btnProcess_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("../../close_win.aspx");
            }
            else
            {
                this.btnSearch_Click(null, null);
            }
        }
        #endregion

        #region CreateTable
        private void CreateTable()
        {
            objDataTable = new DataTable("InvoiceDetails");

            objDataTable.Columns.Add("InvoiceID");
            objDataTable.Columns.Add("ReferenceNo");
            objDataTable.Columns.Add("SupplierCode");
            objDataTable.Columns.Add("Supplier");
            objDataTable.Columns.Add("VendorCode");
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
            objDataTable.Columns.Add("PaymentDueDate");
            objDataTable.Columns.Add("DocAttachments");
            objDataTable.Columns.Add("DocType");
            objDataTable.Columns.Add("ParentRowFlag");
            objDataTable.Columns.Add("DocumentID");
            objDataTable.Columns.Add("IsDuplicate");
        }
        #endregion
        #region GetFormattedDate
        protected string GetFormattedDate(object strDate)
        {
            string retDate = "";
            if (Convert.ToString(strDate) != "")
            {
                retDate = DateTime.Parse(Convert.ToString(strDate)).ToString("dd/MM/yyyy").Replace("/", ".");

            }
            return retDate;
        }
        #endregion

        #region LoadData
        private void LoadData(int iCompanyID)
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

            string strStatus = "";
            string strUserName = "";
            CreateTable();
            CheckDate();

            int CurrentCompanyID = 0;
            CurrentCompanyID = iCompanyID;
            if (CurrentCompanyID == 0)
            {
                CurrentCompanyID = Convert.ToInt32(Session["CompanyID"]);
            }


            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //sqlDA = new SqlDataAdapter("sp_StockDocumentDtls_GMG", sqlConn);//sp_StockDocumentDtls_Akkeron

            //sqlDA = new SqlDataAdapter("sp_StockDocumentDtls_ETC", sqlConn);

            sqlDA = new SqlDataAdapter("sp_StockDocumentDtls_ETC_New", sqlConn);

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.CommandTimeout = 0;
            //sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", CurrentCompanyID);
            //sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", ddlSupplier.SelectedValue.Trim());
            sqlDA.SelectCommand.Parameters.Add("@ActionStatusID", ddlActionStatus.SelectedValue.Trim());
            sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());

            //if(txtInvoiceNo.Text.Trim() == "")
            //    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", DBNull.Value);
            //else
            //    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", txtInvoiceNo.Text.Trim());

            if (txtPONo.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@PONo", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@PONo", txtPONo.Text.Trim());

            if (strFromDate.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@FromDate", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);

            if (strToDate.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@ToDate", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);
            if (textRange1.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@FromPrice", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@FromPrice", Convert.ToDecimal(FromPrice));
            if (textRange2.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.Add("@ToPrice", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@ToPrice", Convert.ToDecimal(ToPrice));

            //sqlDA.SelectCommand.Parameters.Add("@DepartmentID", Convert.ToInt32(ddldept.SelectedValue.Trim()));

            // -------------Add Filter For Wild Card search on supplier ( Subha Das 13th January 2015)

            //-------------Department
            if (Convert.ToString(ddldept.SelectedValue) != "0")
                sqlDA.SelectCommand.Parameters.Add("@DepartmentID", Convert.ToInt32(ddldept.SelectedValue.Trim()));
            else
                sqlDA.SelectCommand.Parameters.Add("@DepartmentID", DBNull.Value);

            //-------BusinessUnit 
            if (Convert.ToString(ddlBusinessUnit.SelectedValue) == "0")
                sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", Convert.ToString(ddlBusinessUnit.SelectedValue));

            //-------------Supplier Wild Card
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


            ////-------------InvoiceNo Wild Card
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

            //-------Nominal           
            if (hdNominalCodeId.Value != "")
                sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", Convert.ToInt32(hdNominalCodeId.Value));
            else
                sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", DBNull.Value);

            // Adding section End     


            sqlDA.TableMappings.Add("InvoiceDetails", "InvoiceHeader");

            ds = new DataSet();
            try
            {
                sqlDA.Fill(ds, "InvoiceDetails");
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }


            foreach (DataRow drInvoiceHeader in ds.Tables["InvoiceHeader"].Rows)
            {

                dr = objDataTable.NewRow();
                dr["InvoiceID"] = drInvoiceHeader["InvoiceID"];
                dr["ReferenceNo"] = drInvoiceHeader["ReferenceNo"];
                dr["SupplierCode"] = drInvoiceHeader["SupplierCode"];
                dr["Supplier"] = drInvoiceHeader["Supplier"];
                dr["VendorCode"] = drInvoiceHeader["VendorCode"];
                dr["InvoiceDate"] = drInvoiceHeader["InvoiceDate"];
                dr["DeliveryDate"] = drInvoiceHeader["DeliveryDate"];
                dr["Currency"] = objInvoice.GetCurrencyCode(Convert.ToInt32(drInvoiceHeader["CurrencyTypeID"]));
                dr["Net"] = drInvoiceHeader["Net"];
                dr["VAT"] = drInvoiceHeader["VAT"];
                dr["Total"] = drInvoiceHeader["Total"];
                objInvoice.GetCurrentStatus(Convert.ToInt32(drInvoiceHeader["StatusID"]), out strStatus);
                dr["DocStatus"] = strStatus;
                dr["ActionStatus"] = drInvoiceHeader["ActionStatus"];
                dr["Comment"] = strUserName;
                dr["PaymentDueDate"] = drInvoiceHeader["PaymentDueDate"];
                dr["DocAttachments"] = drInvoiceHeader["Document"];
                dr["DocType"] = drInvoiceHeader["New_DocumentType"];
                dr["DocumentID"] = drInvoiceHeader["DocumentID"];

                dr["IsDuplicate"] = drInvoiceHeader["IsDuplicate"];

                dr["ParentRowFlag"] = "1";
                objDataTable.Rows.Add(dr);

            }

            ViewState["objDataTable"] = objDataTable;
            PopulateGrid();
            CheckDuplicateValues();
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
                    divP2DLogo.Visible = false;
                    grdInvCur.Visible = true;
                    grdInvCur.DataSource = dtbl;
                    grdInvCur.DataBind();

                }
                else
                {
                    divP2DLogo.Visible = true;
                    grdInvCur.Visible = false;
                    lblMessage.Text = "Sorry, no record(s) found.";
                }
            }
            catch (Exception Ex)
            {
                string strError = Ex.ToString();
                grdInvCur.CurrentPageIndex = 0;
                LoadData(Convert.ToInt32(ddlCompany.SelectedValue));
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
            PopulateGrid();
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
        #region GetURL
        protected string GetURL(object oInvoiceID, object oDocType, object oSupplier, object oInvoiceDate, object oDocStatus)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strDocType = Convert.ToString(oDocType);
            string strSupplier = Convert.ToString(oSupplier);
            string strInvoiceDate = Convert.ToString(oInvoiceDate);
            string strDocStatus = Convert.ToString(oDocStatus);
            string strURL = "";
            strURL = "javascript:window.open('ActionInvoice.aspx?InvoiceID=" + strInvoiceID + "&InvoiceType=" + strDocType + "&Date=" + strInvoiceDate + "&Status=" + strDocStatus + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "','abb','width=500,height=400,scrollbars=1,resizable=1');";
            return (strURL);
        }
        #endregion
        // ==============================================================================================================
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
            txtSupplier.Value = "";
            HdSupplierId.Value = "";

            txtInvoiceNo.Text = "";
            hdInvoiceNo.Value = "";

            try
            {
                grdInvCur.CurrentPageIndex = 0;
            }
            catch { }
            try
            {
                objDataTable.Dispose();
                grdInvCur.Dispose();
            }
            catch { }

            Session["DropDownCompanyID"] = ddlCompany.SelectedValue;
            LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), 0);

            // Added for Vendor Class (Subha Das 18th Dec 2014)
            populateVendorClass(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));

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



        #region GetBusinessUnit
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
        #endregion

        #region GetCompanyListForPurchaseInvoiceLog
        private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int iAction)
        {
            if (iAction == 1)
            {
                //				Company objCompany = new Company();
                //				ddlCompany.DataSource =	objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(iCompanyID),Convert.ToInt32(Session["UserID"]),Convert.ToInt32(Session["UserTypeID"]));
                //				ddlCompany.DataBind();

                try
                {

                    Company objCompany = new Company();
                    DataTable dtCompany = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(iCompanyID), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.DataSource = dtCompany;
                        ddlCompany.DataTextField = "CompanyName";
                        ddlCompany.DataValueField = "CompanyID";
                        ddlCompany.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.ToString();
                }
                ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

                ddlActionStatus.Items.Insert(0, new ListItem("Select Action Status", "0"));
                ddlActionStatus.Items.Insert(1, new ListItem("Pending", "P"));
                ddlActionStatus.Items.Insert(2, new ListItem("Completed", "C"));
                ddlActionStatus.Items.Insert(3, new ListItem("OverDue", "O"));
            }

            //ddlSupplier.DataSource = objInvoice.GetSuppliersList(0,Convert.ToInt32(Session["UserID"]),Convert.ToInt32(Session["UserTypeID"]));
            //ddlSupplier.DataBind();
            //---------------------------------------------------------------------------------------------------------------------------
            //string sSql = "SELECT StatusID,Status FROM InvCNStatus WHERE StatusID in (28,6,25,26)";done by subha
            string sSql = "SELECT StatusID,Status FROM InvCNStatus WHERE StatusID in (28,25)";//modified by kuntal karar
            //---------------------------------------------------------------------------------------------------------------------------
            ddlDocStatus.DataSource = GetDatasetAgainstSQL(sSql);
            ddlDocStatus.DataBind();
            //ddlInvoiceNo.DataSource = objInvoice.GetInvoiceNo(iCompanyID, 1,Convert.ToInt32(Session["UserTypeID"]));
            //ddlInvoiceNo.DataBind();

            JKS.Invoice objectInvoice = new JKS.Invoice();
            DataSet ds = new DataSet();
            string sQry = "SELECT DepartmentID ,Department FROM Department WHERE BuyerCompanyID IN (SELECT CompanyID FROM Company WHERE ParentCompanyID =(SELECT ParentCompanyID FROM Company WHERE CompanyID=" + System.Convert.ToInt32(Session["CompanyID"]) + "))";
            ds = GetDatasetAgainstSQL(sQry);
            ddldept.DataSource = ds;
            ddldept.DataBind();
            ddldept.Items.Insert(0, new ListItem("Select Department", "0"));

            //ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));			
            ddlDocStatus.Items.Insert(0, new ListItem("Select Doc Status", "0"));
            //ddlInvoiceNo.Items.Insert(0, new ListItem("Select Doc No", "0"));			
            try
            {
                ddlDocStatus.Items.Remove(new ListItem("Overdue", "12"));
            }
            catch { }
        }
        #endregion
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

            if (Convert.ToInt32(ddldept.SelectedValue.Trim()) == 0 || textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
            {
                //CheckDate();
                if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
                {
                    FromPrice = Convert.ToDecimal(0);
                    ToPrice = Convert.ToDecimal(100000);
                }
                else
                {
                    if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                    {
                        lblMsg1.Text = "'From Range' cannot be greater than 'To Range'";
                        lblMsg1.Visible = true;
                    }
                    else
                    {
                        lblMsg.Text = "";
                        FromPrice = Convert.ToDecimal(textRange1.Text);
                        ToPrice = Convert.ToDecimal(textRange2.Text);

                    }
                }
            }
            else
            {
                if (Convert.ToInt32(ddldept.SelectedValue.Trim()) == 0)
                {
                    //CheckDate();
                    if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
                    {
                        FromPrice = Convert.ToDecimal(0);
                        ToPrice = Convert.ToDecimal(100000);
                    }
                    else
                    {
                        if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                        {
                            lblMsg1.Text = "'From Range' cannot be greater than 'To Range'";
                            lblMsg1.Visible = true;
                        }
                        else
                        {
                            lblMsg.Text = "";
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
                        FromPrice = Convert.ToDecimal(0);
                        ToPrice = Convert.ToDecimal(100000);
                    }
                    else
                    {
                        if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                        {
                            lblMsg1.Text = "'From Range' cannot be greater than 'To Range'";
                            lblMsg1.Visible = true;
                        }
                        else
                        {
                            lblMsg.Text = "";
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
            LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
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

        #region SetCompanyID
        private void SetCompanyID(string strCompanyID)
        {
            ddlCompany.SelectedValue = strCompanyID.Trim();
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
                //strURL = "javascript:window.open('../../JKS/CreditNotes/InvoiceStatusLogNL_CN.aspx?InvoiceID=" + strInvoiceID + "','a','width=550,height=450,scrollbars=1');";
                strURL = "../../JKS/CreditNotes/InvoiceStatusLogNL_CN.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
            }
            else
            {
                //strURL = "javascript:window.open('../../JKS/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','a','width=700,height=450');";
                strURL = "../../JKS/invoice/InvoiceStatusLogNL.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
            }
            return (strURL);
        }
        #endregion
        #region GetPOActionURL
        protected string GetPOActionURL(object oInvoiceID, object oDocType, object oSupplier)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strDocumentType = Convert.ToString(oDocType);
            string strSupplier = Convert.ToString(oSupplier);

            string strURL = "";
            if (strDocumentType.Trim() == "CRN")
            {
                strURL = "javascript:window.open('../../JKS/StockQC/CreditStkAction.aspx?SQ=true&InvoiceID=" + strInvoiceID + "','a','width=780,height=680,scrollbars=1,resizable=1');";

            }
            else
            {
                // strURL = "javascript:window.open('../../JKS/StockQC/InvoiceAction.aspx?SQ=true&InvoiceID=" + strInvoiceID + "','a','width=810,height=680,scrollbars=1,resizable=1');";
                //strURL = "javascript:window.open('MatchingCombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocumentType + "','MatchingCombindWindow','fullscreen,scrollbars');";
                strURL = "javascript:window.open('MatchingCombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocumentType + "','MatchingCombindWindow','height=' + screen.height + ',width=' + screen.width +',scrollbars,top=0,left=0,resizable=0');";
            }
            // strURL = "javascript:window.open('MatchingCombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocumentType+"','MatchingCombindWindow','fullscreen,scrollbars');";
            return (strURL);
        }
        #endregion
        #region CheckDuplicateValues
        private void CheckDuplicateValues()
        {
            //            for (int i=0; i<grdInvCur.Items.Count; i++)
            //            {
            //                if (i > 0)
            //                {
            //                    if ((grdInvCur.Items[i].Cells[5].Text.Trim().Equals(grdInvCur.Items[i-1].Cells[5].Text.Trim())) && (grdInvCur.Items[i].Cells[6].Text.Trim().Equals(grdInvCur.Items[i-1].Cells[6].Text.Trim())))
            //                    {	
            //                        //====Changed By Gargi on 10 July 2013

            ////						grdInvCur.Items[i-1].BackColor =Color.Red;
            ////						grdInvCur.Items[i].BackColor =Color.Red;
            //                    }
            //                }
            //            }
        }
        #endregion
        #region LoadDate
        private void LoadDate()
        {
            /*ddlYear.Items.Insert(0,new ListItem("Year","0"));
            ddlYear1.Items.Insert(0,new ListItem("Year","0"));
            ddlMonth.Items.Insert(0,new ListItem("Month","0"));
            ddlMonth1.Items.Insert(0,new ListItem("Month","0"));
            ddlday.Items.Insert(0,new ListItem("Day","0"));
            ddlday1.Items.Insert(0,new ListItem("Day","0"));
			
            currentYear=Microsoft.VisualBasic.DateAndTime.Year(System.DateTime.Now);
            for(int i=(currentYear-5);i<=(currentYear+5);i++)
                ddlYear.Items.Add(i.ToString());
            for(int i=1;i<13;i++)
            {
                ddlMonth.Items.Add(new ListItem(Microsoft.VisualBasic.DateAndTime.MonthName(i,true),(i.ToString())));
                ddlMonth1.Items.Add(new ListItem(Microsoft.VisualBasic.DateAndTime.MonthName(i,true),(i.ToString())));
            }
            for(int i=1;i<32;i++)
            {
                ddlday.Items.Add(i.ToString());
                ddlday1.Items.Add(i.ToString());
            }
            for(int i=(currentYear-5); i<=(currentYear+5);i++)
                ddlYear1.Items.Add(i.ToString());
              */
        }

        #endregion
        #region SearchData
        private void LoadDataSearch(int iCompanyID)
        {
            string strStatus = "";
            string strUserName = "";
            CreateTable();
            CheckDate();
            if (Convert.ToInt32(ddldept.SelectedValue.Trim(), 10) == 0)
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlDA = new SqlDataAdapter("stpGetStockCurrentSearch_NL", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
                sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", ddlSupplier.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@ActionStatusID", ddlActionStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", txtInvoiceNo.Text.Trim());
                sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);
                sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);
                sqlDA.SelectCommand.Parameters.Add("@FromPrice", Convert.ToDecimal(FromPrice));
                sqlDA.SelectCommand.Parameters.Add("@ToPrice", Convert.ToDecimal(ToPrice));

            }
            else
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlDA = new SqlDataAdapter("stpGetStockCurrentSearch_NL", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
                sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", ddlSupplier.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@ActionStatusID", ddlActionStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", txtInvoiceNo.Text.Trim());
                sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);
                sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);
                sqlDA.SelectCommand.Parameters.Add("@FromPrice", Convert.ToDecimal(FromPrice));
                sqlDA.SelectCommand.Parameters.Add("@ToPrice", Convert.ToDecimal(ToPrice));
                sqlDA.SelectCommand.Parameters.Add("@DepartmentID", ddldept.SelectedValue.Trim());

            }
            sqlDA.TableMappings.Add("InvoiceDetails", "InvoiceHeader");
            ds = new DataSet();
            try
            {
                sqlDA.Fill(ds, "InvoiceDetails");
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            foreach (DataRow drInvoiceHeader in ds.Tables["InvoiceHeader"].Rows)
            {
                dr = objDataTable.NewRow();
                dr["InvoiceID"] = drInvoiceHeader["InvoiceID"];
                dr["ReferenceNo"] = drInvoiceHeader["ReferenceNo"];
                dr["SupplierCode"] = drInvoiceHeader["SupplierCode"];
                dr["Supplier"] = drInvoiceHeader["Supplier"];
                dr["InvoiceDate"] = drInvoiceHeader["InvoiceDate"];
                dr["DeliveryDate"] = drInvoiceHeader["DeliveryDate"];
                dr["Net"] = drInvoiceHeader["Net"];
                dr["VAT"] = drInvoiceHeader["VAT"];
                dr["Total"] = drInvoiceHeader["Total"];
                objInvoice.GetCurrentStatus(Convert.ToInt32(drInvoiceHeader["StatusID"]), out strStatus);
                dr["DocStatus"] = strStatus;
                dr["ActionStatus"] = drInvoiceHeader["ActionStatus"];
                dr["Comment"] = strUserName;
                dr["PaymentDueDate"] = drInvoiceHeader["PaymentDueDate"];
                dr["DocAttachments"] = drInvoiceHeader["Document"];
                dr["DocType"] = drInvoiceHeader["New_DocumentType"];
                dr["ParentRowFlag"] = "1";
                objDataTable.Rows.Add(dr);
            }
            ViewState["objDataTable"] = objDataTable;
            PopulateGrid();
            CheckDuplicateValues();
        }
        #endregion
        #region Getredirecturl
        public string Getredirecturl(object DocType, object ReferenceNo, object InvoiceNo)
        {
            string URL = "";
            string Reference;
            string DocumentType = "";
            DocumentType = Convert.ToString(DocType);
            Reference = Convert.ToString(ReferenceNo);
            int invoiceID = Convert.ToInt32(InvoiceNo);
            if (DocumentType == "CRN")
            {
                URL = "<a href='../../JKS/CreditNotes/InvoiceConfirmationNL_CN.aspx?InvoiceID=" + invoiceID + "&AllowEdit=StockQC'>" + ReferenceNo + "</a>";
            }
            else if (DocumentType == "DBN")
            {
                URL = "<a href='../../JKS/invoice/InvoiceConfirmationNL.aspx?InvoiceID=" + invoiceID + "'>" + ReferenceNo + "</a>";
            }
            else if (DocumentType == "PO")
            {
                URL = "<a href='../../JKS/SalesOrders/InvoiceConfirmation_PO.aspx?PurchaseOrderID=" + invoiceID + "'>" + ReferenceNo + "</a>";
            }
            else
            {
                URL = "<a href='../../JKS/invoice/InvoiceConfirmationNL.aspx?InvoiceID=" + invoiceID + "&AllowEdit=StockQC'>" + ReferenceNo + "</a>";
            }
            return (URL);

        }

        #endregion
        #region CheckDate
        protected void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            int DocumentID = 0;
            int iInvID = 0;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                // Added by Mrinal on 28th January 2015

                iInvID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
                rptAttachment = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptAttachment");
                rptAttachment.DataSource = null;
                rptAttachment.DataBind();
                dtRepeater = GetAttachmentDetails(iInvID, Convert.ToString(DataBinder.Eval(e.Item.DataItem, "doctype")));
                if (dtRepeater.Rows.Count > 0)
                {
                    rptAttachment.DataSource = dtRepeater;
                    rptAttachment.DataBind();
                }

                int IsDuplicate = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IsDuplicate"));
                // bool IsDuplicate = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsDuplicate"));
                if (IsDuplicate == 1)
                {
                    e.Item.CssClass = "ColorDuplicateRow td";
                }
                //if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IsDuplicate")) == 1)
                //{
                //    e.Item.CssClass = "ColorDuplicateRow td";
                //}

                // Addition end by Mrinal on 28th January 2015






                string sRetUrl = "../StockQC/CurrentInvoice.aspx";
                iInvID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
                if (DataBinder.Eval(e.Item.DataItem, "doctype") != DBNull.Value && Convert.ToString(DataBinder.Eval(e.Item.DataItem, "doctype")) != "")
                {
                    if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "doctype")).ToUpper() == "INV")
                    {
                        ((HyperLink)e.Item.FindControl("hpDoc")).NavigateUrl = "../invoice/InvoiceFileManager_NL.aspx?From=Akkeron&InvoiceID=" + iInvID + "&ReturnUrl=" + sRetUrl;

                        if (DataBinder.Eval(e.Item.DataItem, "DocumentID") != DBNull.Value && Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocumentID")) != "")
                        {
                            DocumentID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DocumentID"));
                        }
                    }
                    else if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "doctype")).ToUpper() == "CRN")
                    {
                        ((HyperLink)e.Item.FindControl("hpDoc")).NavigateUrl = "../creditnotes/CreditnoteFileManager_NL.aspx?From=Akkeron&CreditNoteID=" + iInvID + "&ReturnUrl=" + sRetUrl;

                        if (DataBinder.Eval(e.Item.DataItem, "DocumentID") != DBNull.Value && Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocumentID")) != "")
                        {
                            DocumentID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DocumentID"));
                        }

                    }
                }




                int sHold = objInvoice.GetAPCommLinkColor(Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID")), Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")));

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


                string strStatusLogLink = GetStatusURL(DataBinder.Eval(e.Item.DataItem, "InvoiceID"), DataBinder.Eval(e.Item.DataItem, "DocType"));
                strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:530,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                System.Web.UI.HtmlControls.HtmlAnchor aStatusHistory = ((System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("aStatusHistory"));
                aStatusHistory.Attributes.Add("onclick", strStatusLogLink);

            }

        }

        private void CheckDate()
        {
            if (txtFromDate.Text != "" || txtToDate.Text != "")
            {
                if ((txtFromDate.Text.ToString().Trim().Length == 10) && (txtToDate.Text.ToString().Trim().Length == 10))
                {
                    strFromDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    strToDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    Response.Write(@"<script language='javascript'>alert('Date must be in dd/mm/yyyy format. Please select date from the calendar.');</script>");

                    if (txtFromDate.Text.ToString().Trim().Length < 10)
                    {
                        txtFromDate.Text = "";
                        strFromDate = "";
                    }
                    if (txtToDate.Text.ToString().Trim().Length < 10)
                    {
                        txtToDate.Text = "";
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

        protected string GetAPCommentsURL(object oInvoiceID, object oDocType, object oDocNo, object oDocStatus)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strDocumentType = Convert.ToString(oDocType);
            string strDocNo = Convert.ToString(oDocNo);
            string strDocStatus = Convert.ToString(oDocStatus);

            string strURL = "";

            strURL = "javascript:window.open('../../JKS/invoice/APComments.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocumentType + "&DocNo=" + strDocNo + "&DocStatus=" + strDocStatus + "','InvoiceStatusLogNL','width=700,height=450,scrollbars=1');";

            return (strURL);
        }


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
                    // myList.Add(string.Format("{0}", row["label"].ToString()));

                }
                return myList.ToArray();
                // return JsonConvert.SerializeObject(dsSupplier.Tables[0]);
            }
            else
                return null;
            // return "";
        }

        [WebMethod]
        public static List<string> FetchInvoiceNo(string CompanyID, string DocType, string UserString)
        {
            DataSet dsInvoiceNo = new DataSet();
            Invoice_New objInv = new Invoice_New();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_FetchInvoiceNo", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            if (DocType != "")
                sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);
            else
                sqlDA.SelectCommand.Parameters.Add("@DocType", DBNull.Value);
            sqlDA.SelectCommand.Parameters.Add("@InvoiceString", UserString);
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
    }
}
