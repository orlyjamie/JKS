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
using System.Linq;
using System.Text;

namespace JKS
{
    [ScriptService]
    public partial class CurrentStatus_VS2010 : SVSPage
    {
        string Sort_Direction = "RowID Asc"; // Added By Subhrajyoti on 21st July 2015
        public int ChkUserID = 0;
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

            //=============================Commneted By Subhrajyoti on 21st July 2015=========================================
            //this.grdInvCur.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.grdInvCur_PageIndexChanged);
            //this.grdInvCur.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvCur_ItemDataBound);
            //this.grdInvCur.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.grdInvCur_SortCommand);
            //this.grdInvCur.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdInvCur_ItemCommand);
            //=============================Commneted By Subhrajyoti on 21st July 2015 End=========================================
            this.Load += new System.EventHandler(this.Page_Load);
            this.btnDownloadAttachment.Click += new System.EventHandler(this.btnDownloadAttachment_Click);
            //  this.btnProcess.Click += new EventHandler(btnProcess_Click);
        }
        #endregion

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {          
            ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            //Addded by Subhrajyoti on 21st July 2015
            if (Session["SortExpr"] != null)
            {
                ViewState["SortExpr"] = Session["SortExpr"];
            }
            else
            {
                ViewState["SortExpr"] = Sort_Direction;
            }
            //Session["sorted"] = "";
            // Session["InvoiceID"] = "";
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");
            iNeedRefreshToBottom = 0;
            CBSolutions.ETH.Web.baseUtil.keepAlive(this);

            lblMsg1.Visible = false;
            lblMsg.Visible = false;
            //  doAction();
            Session["creninv"] = "";
            Session["invcren"] = "";
            //  ViewState["SortExpr"] = Sort_Direction;
            //Addded by Subhrajyoti on 21st July 2015
            Session["invcren"] = "0";
            Session["creninv"] = "0";//Added By Rimi on 6th August 2015

            if (!string.IsNullOrEmpty(Session["det"] as string))
            {
                //if (Convert.ToInt32(Session["UserTypeID"]) != 2 && Convert.ToInt32(Session["UserTypeID"]) != 3)
                //{
				 if (Session["DropDownCompanyID"] != null && Session["DropDownCompanyID"].ToString() != "  Select Company Name")
                {
                    // btnSearch_Click(null, null);
                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "CurrentPage Log 1:-" + Convert.ToString(Session["DropDownCompanyID"].ToString()));
                    LoadData(Convert.ToInt32(Session["DropDownCompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));
                }
                else
                {
                    LoadData(Convert.ToInt32(Session["CompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));
                }
                Session["dtdvEmployee"] = null;
                GridView1.DataSource = Getdata();
                GridView1.DataBind();
            }
            else
            {
                ViewState["return"] = "0";
                // ddlCompany.SelectedValue = Session["CompanyID"].ToString();
            }

            dgSalesCallDetails_CRN.CurrentPageIndex = 0;
            dgSalesCallDetails.CurrentPageIndex = 0;
            if (!IsPostBack)
            {
                // Added By Mrinal 22nd September 2014
                Session["dtTiffViewer"] = null;
                ViewState["dtCheckAttachment"] = null;
                // Addition End on 22nd September 2014
                lblMessage.Text = "";
                lblMessage.Visible = false;

                //added by kuntalkarar on 1stJune2017
                txtSupplier.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                txtPONo.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                txtNominal.Attributes.Add("onkeydown", "return (event.keyCode!=13);");


                CBSolutions.ETH.Web.Utility.makeDefaultButton(txtInvoiceNo, btnSearch);
                CBSolutions.ETH.Web.Utility.makeDefaultButton(textRange1, btnSearch);
                CBSolutions.ETH.Web.Utility.makeDefaultButton(textRange2, btnSearch);

                Session["ApproveForm"] = 0;
                Session["SelectedPage"] = "PurchaseInvoiceLog";
                iLoadFlag++;
                Session["DropDownCompanyID"] = null;
                btnSearch.Attributes.Add("onclick", "javascript:return fn_Validate();");


                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
                //LoadDate();
                if (ddlCompany.SelectedItem.Text != "Select Company Name")
                {
                    GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                }

                LoadDepartment();

                //Blocked by Mrinal on 31st December 2013
                if (Convert.ToInt32(Session["UserTypeID"]) != 2 && Convert.ToInt32(Session["UserTypeID"]) != 3)
                {
                    //if (Session["DropDownCompanyID"] != null)


                 //blocked by Kuntalkarar on 4thSeptember2017
                //if (!string.IsNullOrEmpty(Session["DropDownCompanyID"] as string))
                //added by kuntalkarar on 4thSeptember2017
                if (!string.IsNullOrEmpty(Session["DropDownCompanyID"] as string) && Session["DropDownCompanyID"] != "" && Session["DropDownCompanyID"] != null && Session["DropDownCompanyID"].ToString() != "  Select Company Name" && Session["DropDownCompanyID"].ToString().Trim() != "Select Company Name")
                 
                    {
                        // btnSearch_Click(null, null);
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "CurrentPage Log 2:-" + Convert.ToString(Session["DropDownCompanyID"].ToString()));
                        LoadData(Convert.ToInt32(Session["DropDownCompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));
                    }
                    else
                    {
                        LoadData(Convert.ToInt32(Session["CompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));
                    }
                    Session["dtdvEmployee"] = null;
                    GridView1.DataSource = Getdata();
                    GridView1.DataBind();
                }


                if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    string LogInCompanyid = "";

                    PasswordReset objPasswordReset = new PasswordReset();
                    List<PasswordReset> lstSaltedPassword = objPasswordReset.GetLogInCompanyId(Convert.ToInt32(Session["UserID"]));//strResetAnswer
                    if (lstSaltedPassword.Count > 0)
                    {
                        LogInCompanyid = lstSaltedPassword[0].LoginCompanyId;
                        int userId = Convert.ToInt32(Session["UserID"]);
                        btnPopup.Visible = true;
                        PasswordReset objPasswordReset1 = new PasswordReset();
                        grdApprovals.DataSource = objPasswordReset.GetApprovals(userId);
                        grdApprovals.DataBind();
                    }


                    ddlCompany.SelectedValue = LogInCompanyid;
                    LoadData(Convert.ToInt32(LogInCompanyid), GetDocType(), Convert.ToInt32(Session["UserID"]));
                    cbSupplier.Checked = true;
                    cbSupplier.Disabled = true;
                    cbInvoiceNo.Checked = true;
                    //PasswordReset objPasswordReset = new PasswordReset();
                    grdApprovals.DataSource = objPasswordReset.GetApprovals(Convert.ToInt32(Session["UserID"]));
                    grdApprovals.DataBind();

                    btnPopup.Visible = true;
                }

                if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                {
                    cbSupplier.Checked = true;
                    cbSupplier.Disabled = true;
                    cbInvoiceNo.Checked = true;
                    divP2DLogo.Visible = true;
                    btnPopup.Visible = false;
                }

                if (Convert.ToInt32(Session["UserTypeID"]) == 2)
                {
                    cbSupplier.Checked = true;
                    cbSupplier.Disabled = true;
                }

                //----------modified by kuntal on 18th mar2015---pt.46-------
                if (ddlCompany.SelectedItem.Text != "Select Company Name")
                {
                    populateVendorClass(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                }
                //-----------------------------------------------------------

                //Modified by Koushik Das on 30-MAR-2017
                ApplySessionWhenReturnedFromEditPage();
                //Modified by Koushik Das on 30-MAR-2017
            }
        }
        #endregion

        #region takeAction  ,  doAction
        private void takeAction(string docType, int ID, int iOperation)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            SqlCommand sqlCmd = new SqlCommand("UPD_UpdateStatus", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@docType", docType);
            sqlCmd.Parameters.AddWithValue("@ID", ID);
            sqlCmd.Parameters.AddWithValue("@Operation", iOperation);
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

        public void ErrorLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName, true);
            sw.WriteLine(DateTime.Now + ": " + sErrMsg);
            sw.Flush();
            sw.Close();
        }
		
        #region LoadData
        private void LoadData(int iCompanyID, string sDocType, int iUserID)
        {

            //Added by kuntalkarar on 17thMarch2017
            if (textRange1.Text.Length > 0 && textRange2.Text.Length > 0)
            {
                if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                {
                    //lblMsg1.Text = "From Range cannot be greater than To Range";
                    //lblMsg1.Visible = true;
                }
                else
                {
                    //lblMsg1.Text = "";
                    FromPrice = Convert.ToDecimal(textRange1.Text);
                    ToPrice = Convert.ToDecimal(textRange2.Text);
                }
            }
            //--------------------------------------------------

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
            //modified by rimi on 11thJune2015
            if (string.IsNullOrEmpty(CurrentCompanyID.ToString()))
            {
                CurrentCompanyID = Convert.ToInt32(Session["CompanyID"]);
            }
            else if (CurrentCompanyID == 0)
            {
                CurrentCompanyID = Convert.ToInt32(Session["CompanyID"]);
            }
            //if (CurrentCompanyID == 0)

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //blocked by kuntal on 23rdFeb2016
            //if (Convert.ToString(ddldept.SelectedValue) == "0")
            //Added by kuntalkarar on 23rdFeb2016
            if (Convert.ToString(ddldept.SelectedValue) == "0" || Convert.ToString(ddldept.SelectedValue) == "" || string.IsNullOrEmpty(Convert.ToString(ddldept.SelectedValue)))
            {
                ErrorLog(Server.MapPath("ErrorLog.txt"), "NewBuyerCurrent_ETC_SampleNew_JKS");
                ErrorLog(Server.MapPath("ErrorLog.txt"), "CurrentCompanyID:-" + CurrentCompanyID);

                sqlDA = new SqlDataAdapter("NewBuyerCurrent_ETC_SampleNew_GRH_NewJKS", sqlConn);//Added By Subhrajyoti on 18thJuly2015
                //sqlDA = new SqlDataAdapter("NewBuyerCurrent_ETC_SampleNew_GRH", sqlConn);

                //Response.Write("<Br />SP: NewBuyerCurrent_ETC_SampleNew_GRH");
            }
            else
            {
                sqlDA = new SqlDataAdapter("NewBuyerCurrent_ETC_SampleNew_FilterByDeptID_JKS", sqlConn);
                //Response.Write("<Br />SP: NewBuyerCurrent_ETC_SampleNew_FilterByDeptID");
            }

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.AddWithValue("@UserID", iUserID);
            //Response.Write("<Br />@UserID: " + iUserID);
            sqlDA.SelectCommand.Parameters.AddWithValue("@CompanyID", CurrentCompanyID);
            //Response.Write("<Br />@CompanyID: " + CurrentCompanyID);
            sqlDA.SelectCommand.Parameters.AddWithValue("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
            //Response.Write("<Br />@DocStatusID: " + ddlDocStatus.SelectedValue.Trim());

            if (strFromDate.Trim() == "")
                sqlDA.SelectCommand.Parameters.AddWithValue("@FromDate", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@FromDate", strFromDate);

            //Response.Write("<Br />@FromDate: " + strFromDate);

            if (strToDate.Trim() == "")
                sqlDA.SelectCommand.Parameters.AddWithValue("@ToDate", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@ToDate", strToDate);

            //Response.Write("<Br />@ToDate: " + strToDate);

            if (txtPONo.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.AddWithValue("@PONo", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@PONo", txtPONo.Text.Trim());

            //Response.Write("<Br />@PONo: " + txtPONo.Text.Trim());

            if (Convert.ToString(ddlBusinessUnit.SelectedValue) == "0")
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@BusinessUnitID", DBNull.Value);
            }
            else if (ddlBusinessUnit.SelectedValue != "")
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@BusinessUnitID", Convert.ToInt32(ddlBusinessUnit.SelectedValue));
            }

            //Response.Write("<Br />@BusinessUnitID: " + ddlBusinessUnit.SelectedValue);

            if (textRange1.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.AddWithValue("@FromPrice", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@FromPrice", Convert.ToDecimal(FromPrice));

            //Response.Write("<Br />@FromPrice: " + FromPrice);

            if (textRange2.Text.Trim() == "")
                sqlDA.SelectCommand.Parameters.AddWithValue("@ToPrice", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@ToPrice", Convert.ToDecimal(ToPrice));

            //Response.Write("<Br />@ToPrice: " + ToPrice);

            if (Convert.ToString(ddldept.SelectedValue) != "Select Department")
                sqlDA.SelectCommand.Parameters.AddWithValue("@DepartmentID", ddldept.SelectedValue.Trim());
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@DepartmentID", DBNull.Value);

            //Response.Write("<Br />@DepartmentID: " + ddldept.SelectedValue.Trim());

            if (sDocType != "")
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@DocType", sDocType);
            }
            else
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@DocType", DBNull.Value);
            }

            //Response.Write("<Br />@DocType: " + sDocType);

            if (Convert.ToInt32(Session["UserTypeID"]) == 3)
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@PassedToUserID", DBNull.Value);
                sqlDA.SelectCommand.Parameters.AddWithValue("@PassedToGroupCode", DBNull.Value);
                iOption = 1;
            }
            else
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@PassedToUserID", Session["UserID"].ToString().Trim());
                if (Session["UserGroupCode"] == null || Session["UserGroupCode"].ToString() == "NULL")
                    sqlDA.SelectCommand.Parameters.AddWithValue("@PassedToGroupCode", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.AddWithValue("@PassedToGroupCode", Session["UserGroupCode"].ToString().Trim());

                iOption = 0;
            }

            //Response.Write("<Br />@PassedToUserID: " + Session["UserID"].ToString().Trim());
            //Response.Write("<Br />@PassedToGroupCode: " + Session["UserGroupCode"].ToString().Trim());

            int IsSentHdId = 0;
            if ((txtSupplier.Value.ToString().Trim() != "") && (cbSupplier.Checked))
            {
                if (HdSupplierId.Value.ToString().Trim() != "")
                {
                    sqlDA.SelectCommand.Parameters.AddWithValue("@SupplierCompanyID", Convert.ToInt32(HdSupplierId.Value));
                    IsSentHdId = 1;
                }
                else
                    sqlDA.SelectCommand.Parameters.AddWithValue("@Filter", txtSupplier.Value.ToString().Trim());
            }
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@Filter", DBNull.Value);

            if (IsSentHdId == 0)
            {
                if ((HdSupplierId.Value.ToString().Trim() != "") && (cbSupplier.Checked == false))
                    sqlDA.SelectCommand.Parameters.AddWithValue("@SupplierCompanyID", Convert.ToInt32(HdSupplierId.Value));
                else
                    sqlDA.SelectCommand.Parameters.AddWithValue("@SupplierCompanyID", 0);
            }

            //Response.Write("<Br />@SupplierCompanyID: " + HdSupplierId.Value);
            //Response.Write("<Br />@Filter: " + txtSupplier.Value.ToString().Trim());

            //------InvoiceNo Wild Card
            IsSentHdId = 0;
            if ((txtInvoiceNo.Text.Trim() != "") && (cbInvoiceNo.Checked))
            {
                if (hdInvoiceNo.Value.ToString().Trim() != "")
                {
                    sqlDA.SelectCommand.Parameters.AddWithValue("@InvoiceNo", hdInvoiceNo.Value.Trim().ToString());
                    IsSentHdId = 1;
                }
                else
                    sqlDA.SelectCommand.Parameters.AddWithValue("@InvoiceNoStr", txtInvoiceNo.Text.Trim());
            }

            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@InvoiceNoStr", DBNull.Value);

            if (IsSentHdId == 0)
            {
                if ((hdInvoiceNo.Value.ToString().Trim() != "") && (cbInvoiceNo.Checked == false))
                    sqlDA.SelectCommand.Parameters.AddWithValue("@InvoiceNo", hdInvoiceNo.Value.Trim().ToString());
                else
                    sqlDA.SelectCommand.Parameters.AddWithValue("@InvoiceNo", DBNull.Value);
            }

            //Response.Write("<Br />@InvoiceNoStr: " + txtInvoiceNo.Text.Trim());
            //Response.Write("<Br />@InvoiceNo: " + hdInvoiceNo.Value.Trim().ToString());

            IsSentHdId = 0;

            //-------------Add Filter For New Vendor Class and Nomina (Subha Das 2nd January 2015)
            if (Convert.ToString(ddlVendorClass.SelectedValue) == "0")
                sqlDA.SelectCommand.Parameters.AddWithValue("@New_VendorClass", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@New_VendorClass", ddlVendorClass.SelectedValue.Trim());

            //Response.Write("<Br />@New_VendorClass: " + ddlVendorClass.SelectedValue.Trim());

            if (hdNominalCodeId.Value != "")
                sqlDA.SelectCommand.Parameters.AddWithValue("@NominalCodeID", Convert.ToInt32(hdNominalCodeId.Value));
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@NominalCodeID", DBNull.Value);

            //Response.Write("<Br />@NominalCodeID: " + hdNominalCodeId.Value);

            // Adding section End 
            ErrorLog(Server.MapPath("ErrorLog.txt"), "iOption:-" + iOption);
            sqlDA.SelectCommand.Parameters.AddWithValue("@Option", iOption);
            sqlDA.SelectCommand.CommandTimeout = 0;

            //Response.Write("<Br />@Option: " + iOption);

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
            //  Session["InvoiceID"] = "";
            List<INVS> lst = new List<INVS>();
            # region:	loop
            DataTable dtRec = ds.Tables["InvoiceHeader"];

            try
            {
                if (dtRec != null)
                {
                    //Response.Write("<Br />dtRec.Rows.Count: " + dtRec.Rows.Count);
                    foreach (DataRow drInvoiceHeader in dtRec.Rows)
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
                        //dr["Currency"] = objInvoice.GetCurrencyCode(Convert.ToInt32(drInvoiceHeader["CurrencyTypeID"]));// Commented By Rimi on 10thJuly2015

                        //============ Added By Rimi on 10thJuly2015========================
                        if (drInvoiceHeader["CurrencyTypeID"] != DBNull.Value)
                        {
                            dr["Currency"] = objInvoice.GetCurrencyCode(Convert.ToInt32(drInvoiceHeader["CurrencyTypeID"]));
                        }
                        else
                        {
                            dr["Currency"] = 0;
                        }
                        //======================== Added By Rimi on 10thJuly2015=======================

                        dr["Net"] = drInvoiceHeader["Net"];
                        dr["VAT"] = drInvoiceHeader["VAT"];
                        dr["Total"] = drInvoiceHeader["Total"];
                        objInvoice.GetCurrentStatus(Convert.ToInt32(drInvoiceHeader["StatusID"]), out strStatus);
                        dr["DocStatus"] = strStatus;

                        INVS objINVS = new INVS();
                        objINVS.InvoiceID = Convert.ToString(dr["InvoiceID"]);
                        objINVS.DocType = Convert.ToString(drInvoiceHeader["New_DocumentType"]);
                        lst.Add(objINVS);
                        Session.Add("InvoiceID", lst);

                        List<INVS> lstInvoiceS = (List<INVS>)Session["InvoiceID"];
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
                }
            # endregion


                objDataTable.AcceptChanges();
                ViewState["objDataTable"] = objDataTable;
                Session["objDataTable"] = objDataTable;//Added By Subhrajyoti on 21st July 2015
                #region: Tiff Viewer Implementation
                if (objDataTable != null && objDataTable.Rows.Count > 0)
                {
                    DataTable dtTiffViewer = objDataTable;
                    Session["dtTiffViewer"] = dtTiffViewer;
                }
                #endregion
                PopulateGrid();

                //==================================Added By Subhrajyoti on 21st July 2015=====================
                // ViewState["SortExpr"] = Sort_Direction;
                ViewState["SortExpr"] = Session["SortExpr"];
                DataView dvEmployee = Getdata();
                GridView1.DataSource = objDataTable;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
                ErrorLog(Server.MapPath("ErrorLog.txt"), "Current-->loadData-->Catch1:-:-" + ss);
            }
            //==================================Added By Subhrajyoti on 21st July 2015 End=====================
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
                    //=============================Commneted By Subhrajyoti on 21st July 2015 =========================================
                    //grdInvCur.Visible = true;
                    //grdInvCur.DataSource = dtbl;
                    //grdInvCur.DataBind();
                    //=============================Commneted By Subhrajyoti on 21st July 2015 End=========================================
                }
                else
                {
                    //grdInvCur.Visible = false; //=============================Commneted By Subhrajyoti on 21st July 2015 End=========================================
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
            Session["dtdvEmployee"] = null;
            //--------ASSING SUPPLIER NAME TO TEXT BOX . BY Subha Das (02/01/2015) 
            txtSupplier.Value = "";
            HdSupplierId.Value = "";

            txtInvoiceNo.Text = "";
            hdInvoiceNo.Value = "";

            txtNominal.Value = "";
            hdNominalCodeId.Value = "";

            //GordonRamsay.Invoice objInvoice = new GordonRamsay.Invoice();
            JKS.Invoice_New objInvoice = new JKS.Invoice_New();

            //ddlSupplier.Items.Clear();
            //ddlSupplier.DataSource = objInvoice.GetSuppliersListForSearch(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]), "");
            //ddlSupplier.DataBind();
            //ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
            if (ddlCompany.SelectedItem.Text != "Select Company Name")
            {
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            }
            LoadDepartment();

            // Added for Vendor Class (Subha Das 18th Dec 2014)
            if (ddlCompany.SelectedItem.Text != "Select Company Name")
            {
                populateVendorClass(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            }
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
                sqlDA.SelectCommand.Parameters.AddWithValue("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));

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
            if (ddlCompany.SelectedItem.Text != "Select Company Name")
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
            }

            sqlDA.SelectCommand.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.AddWithValue("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
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
        //private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int iAction)
        //{
        //    if (iAction == 1)
        //    {
        //        Company objCompany = new Company();
        //        ddlCompany.Items.Clear();
        //        DataTable dt = new DataTable();
        //        //try
        //        //{

        //            //if (dt.Rows.Count > 0)
        //            //{
        //            //    ddlCompany.DataSource = dt;
        //            //    ddlCompany.DataBind();

        //            //    //--------------- Set Default Selected value  "Select Company Name"  by Subha,04-02-2015
        //        if (Convert.ToInt32(Session["UserTypeID"]) == 3)
        //        {
        //            dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
        //            ddlCompany.DataSource = dt;
        //            ddlCompany.DataBind();
        //            ddlCompany.Items.Insert(0, "Select Company Name");

        //           ddlCompany.SelectedValue = Session["CompanyID"].ToString();
        //        }
        //        else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
        //        {
        //            dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
        //            ddlCompany.DataSource = dt;
        //            ddlCompany.DataBind();
        //            ddlCompany.Items.Insert(0, "Select Company Name");


        //        }
        //        else
        //        {
        //            dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
        //            ddlCompany.DataSource = dt;
        //            ddlCompany.DataBind();
        //            ddlCompany.Items.Insert(0, "Select Company Name");

        //            ddlCompany.SelectedValue = dt.Rows[0][0].ToString();
        //            Session["DropDownCompanyID"] = ddlCompany.SelectedValue.ToString();
        //        }
        //            //}
        //        }
        //        //catch { }
        //        //finally
        //        //{
        //        //    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

        //        //}
        //    //}
        //    GordonRamsay.Invoice objInvoice = new GordonRamsay.Invoice();
        //    ddlSupplier.Items.Clear();
        //    if (ddlCompany.SelectedItem.Text != "Select Company Name")
        //    {
        //        ddlSupplier.DataSource = objInvoice.GetSuppliersList(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
        //    }
        //         ddlSupplier.DataBind();
        //    ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));

        //    GordonRamsay.Invoice_New objInv1 = new GordonRamsay.Invoice_New();
        //    ddlDocStatus.Items.Clear();
        //    //ddlDocStatus.DataSource = objInvoice.GetStatusListNL();

        //    //----Changes as per requiermnt : Removing  Approved, Approved s.t. AP, Delete/Archive, Exported, Paid  these status form Doc Status List  only for Current .by Subha 05022015
        //    ddlDocStatus.DataSource = objInv1.GetStatusListNL_Current();
        //    ddlDocStatus.DataBind();
        //    ddlDocStatus.Items.Insert(0, new ListItem("Select Doc Status", "0"));

        //}

        private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int iAction)
        {
            if (iAction == 1)
            {
                Company objCompany = new Company();
                ddlCompany.Items.Clear();
                DataTable dt = new DataTable();
                //try
                //{

                //if (dt.Rows.Count > 0)
                //{
                //    ddlCompany.DataSource = dt;
                //    ddlCompany.DataBind();

                //    //--------------- Set Default Selected value  "Select Company Name"  by Subha,04-02-2015
                if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, "Select Company Name");

                    ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                }

                    //modified by kuntal on 29thApril2015
                else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    string LogInCompanyid = "";
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, "Select Company Name");
                    PasswordReset objPasswordReset = new PasswordReset();
                    List<PasswordReset> lstSaltedPassword = objPasswordReset.GetLogInCompanyId(Convert.ToInt32(Session["UserID"]));//strResetAnswer
                    if (lstSaltedPassword.Count > 0)
                    {
                        LogInCompanyid = lstSaltedPassword[0].LoginCompanyId;
                    }


                    ddlCompany.SelectedValue = LogInCompanyid;

                }
                //else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                //{
                //    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                //    ddlCompany.DataSource = dt;
                //    ddlCompany.DataBind();
                //    ddlCompany.Items.Insert(0, "Select Company Name");
                //  //modified by 

                //}
                //------------------------------------------------------------
                else
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, "Select Company Name");

                    ddlCompany.SelectedValue = dt.Rows[0][0].ToString();
                    Session["DropDownCompanyID"] = ddlCompany.SelectedValue.ToString();
                }
                //}
            }
            //catch { }
            //finally
            //{
            //    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

            //}
            //}
            JKS.Invoice objInvoice = new JKS.Invoice();
            ddlSupplier.Items.Clear();
            if (ddlCompany.SelectedItem.Text != "Select Company Name")
            {
                ddlSupplier.DataSource = objInvoice.GetSuppliersList(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
            }
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));

            JKS.Invoice_New objInv1 = new JKS.Invoice_New();
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
            #region Modified by Koushik Das on 22-JUNE-2017 for setting the search information
            string CompanyID, SupplierName, SupplierID, SupplierNameHD, isSupChecked, VendorClass, DocType, DocStatus, InvoiceNo, InvoiceID, InvoiceNoHD, isInvoice, PONo, BusinessUnit, Department, Nominal, NominalCodeId, FromDate, ToDate, NetFrom, NetTo;

            CompanyID = ddlCompany.SelectedValue;
			SupplierName = txtSupplier.Value;
			SupplierID = HdSupplierId.Value;
			SupplierNameHD = HdSupplierName.Value;
			isSupChecked = cbSupplier.Checked.ToString();
			VendorClass = ddlVendorClass.SelectedValue;
			DocType = ddlDocType.SelectedValue;
			DocStatus = ddlDocStatus.SelectedValue;
			InvoiceNo = txtInvoiceNo.Text;
			InvoiceID = hdInvoiceNo.Value;
			InvoiceNoHD = hdInvoiceNoTxt.Value;
			isInvoice = cbInvoiceNo.Checked.ToString();
			PONo = txtPONo.Text;
			BusinessUnit = ddlBusinessUnit.SelectedValue;
			Department = ddldept.SelectedValue;
			Nominal = txtNominal.Value;
			NominalCodeId = hdNominalCodeId.Value;
			FromDate = txtFromDate.Value;
			ToDate = txtToDate.Value;
			NetFrom = textRange1.Text;
            NetTo = textRange2.Text;

            StoreEditSession(CompanyID, SupplierName, SupplierID, SupplierNameHD, isSupChecked, VendorClass, DocType, DocStatus, InvoiceNo, InvoiceID, InvoiceNoHD, isInvoice, PONo, BusinessUnit, Department, Nominal, NominalCodeId, FromDate, ToDate, NetFrom, NetTo);
            #endregion

            //Session["PageIndex"] = null; // blocked by Koushik Das as on 22-June-2017
            Session["dtdvEmployee"] = null;
            //Session["DropDownCompanyID"] = ddlCompany.SelectedValue.ToString();//added by kuntalkarar on 16thMay2017
            GridView1.PageIndex = 0;
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
                //  grdInvCur.CurrentPageIndex = 0;
            }
            catch { }
            ViewState["dtCheckAttachment"] = null;
            //if(ddlCompany.SelectedIndex==0)


            //================Added By Rimi On 30.06.2015===============================================
            if (Convert.ToString(ViewState["return"]) != "1")
            {
                if (ddlCompany.SelectedItem.Text == "Select Company Name")
                {
                    LoadData(180918, GetDocType(), Convert.ToInt32(Session["UserID"]));//124529 for AnchorSafety changed to 180918 for JKS
                }

                else
                {
                    LoadData(Convert.ToInt32(ddlCompany.SelectedValue), GetDocType(), Convert.ToInt32(Session["UserID"]));
                    Session["det"] = null;
                }
            }	
            else if (Convert.ToString(ViewState["return"]) == "1")
            {
                //ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                LoadData(Convert.ToInt32(Session["CompanyID"]), GetDocType(), Convert.ToInt32(Session["UserID"]));
                if (Convert.ToInt32(Session["UserTypeID"]) != 1)//Added By Rimi on 01.07.2015
                {//Added By Rimi on 01.07.2015
                    ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                }//Added By Rimi on 01.07.2015
                ViewState["return"] = "0";
                ViewState["flag_return"] = "0";
                // Session["det"] = null;
            }
            //================Added By Rimi On 30.06.2015 End===============================================  


            //================Added By Subhrajyotui On 21st July 2015=============================================== 
            ViewState["Sort_Flag"] = "0";
            ViewState["SortExpr"] = Sort_Direction;
            DataView dvEmployee = Getdata();
            GridView1.DataSource = dvEmployee;
            GridView1.DataBind();
            //================Added By Subhrajyotui On 21st July 2015===============================================  
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



        //Blocked By Kd 05.12.2018
        //#region GetStatusURL
        //protected string GetStatusURL(object oInvoiceID, object oDocType)
        //{
        //    string strInvoiceID = Convert.ToString(oInvoiceID);
        //    string strDocumentType = Convert.ToString(oDocType);
        //    string strURL = "";
        //    if (strDocumentType.Trim() == "CRN")
        //    {
        //        // strURL = "javascript:window.open('../../ETC/CreditNotes/InvoiceStatusLogNL_CN.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL_CN','width=550,height=450,scrollbars=1');";
        //        strURL = "../../JKS/CreditNotes/InvoiceStatusLogNL_CN.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
        //    }
        //    else
        //    {
        //        // strURL = "javascript:window.open('../../ETC/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL','width=705,height=450,scrollbars=1');";
        //        strURL = "../../JKS/invoice/InvoiceStatusLogNL.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
        //    }
        //    return (strURL);
        //}
        //#endregion

        //=============================Commneted By Subhrajyoti on 21st July 2015 =========================================
        //#region CheckDuplicateValues
        //private void CheckDuplicateValues()
        //{
        //    for (int i = 0; i < grdInvCur.Items.Count; i++)
        //    {
        //        if (i > 0)
        //        {
        //            if ((grdInvCur.Items[i].Cells[1].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[1].Text.Trim())) && (grdInvCur.Items[i].Cells[4].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[4].Text.Trim())) && (grdInvCur.Items[i].Cells[5].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[5].Text.Trim())) && (grdInvCur.Items[i].Cells[2].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[2].Text.Trim())))
        //            {

        //            }
        //        }
        //    }
        //}
        //#endregion
        //=============================Commneted By Subhrajyoti on 21st July 2015 End=========================================

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
                URL = "<a href='../../JKS/CreditNotes/InvoiceConfirmationNL_CN.aspx?InvoiceID=" + invoiceID + "&AllowEdit=Current'>" + ReferenceNo + "</a>";
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
                URL = "<a href='../../JKS/invoice/InvoiceConfirmationNL.aspx?InvoiceID=" + invoiceID + "&AllowEdit=Current'>" + ReferenceNo + "</a>";
            }
            return (URL);

        }

        #endregion

        //=============================Commneted By Subhrajyoti on 21st July 2015 End=========================================
        //#region grdInvCur_ItemCommand
        //private void grdInvCur_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        if (e.CommandName.ToUpper() == "ACT")
        //        {
        //            int IsPermit = 0;
        //            string sWinParam = "", sHeightWidth = "";

        //            string strInvoiceID = ((Label)e.Item.FindControl("lblInvoiceID")).Text;
        //            string strDocType = ((Label)e.Item.FindControl("lblDocType")).Text;
        //            string strInvoiceDate = ((Label)e.Item.FindControl("lblInvoiceDate")).Text;
        //            string strDocStatus = ((Label)e.Item.FindControl("lblDocStatus")).Text;
        //            string strVoucherNumber = ((Label)e.Item.FindControl("lblVoucherNumber")).Text;

        //            string strURL = "";
        //            Communicorp.Invoice objinvoice = new Communicorp.Invoice();
        //            string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

        //            strRelationType = RelationType.ToString();
        //            IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), strDocType);
        //            if (IsPermit == 0)
        //            {
        //                if (RelationType == "STF" || RelationType == "STU")
        //                {
        //                    sWinParam = "../../Communicorp/History/HistoryAction.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
        //                    sHeightWidth = "width=570,height=600,scrollbars=1,resizable=1";
        //                    strURL = "<script language=javascript>javascript:openBrWindow('" + sWinParam + "','','" + sHeightWidth + "')</script>";
        //                    this.RegisterClientScriptBlock("script", strURL);
        //                }
        //                else
        //                {
        //                    if (strDocType == "CRN")
        //                    {
        //                        sWinParam = "ExpCreditNoteAction.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
        //                        sHeightWidth = "width=570,height=600,scrollbars=1,resizable=1";
        //                        strURL = "<script language=javascript>javascript:openBrWindow('" + sWinParam + "','','" + sHeightWidth + "')</script>";
        //                        this.RegisterClientScriptBlock("script", strURL);
        //                    }
        //                    if (strDocType == "INV")
        //                    {
        //                        sWinParam = "CurrentAction.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
        //                        sHeightWidth = "width=570,height=590,scrollbars=1,resizable=1";
        //                        strURL = "<script language=javascript>javascript:openBrWindow('" + sWinParam + "','','" + sHeightWidth + "')</script>";
        //                        this.RegisterClientScriptBlock("script", strURL);
        //                    }
        //                }
        //            }
        //            else if (IsPermit == 1)
        //            {
        //                if (strDocType == "INV")
        //                {
        //                    strURL = "<script language=javascript>alert('Another user has just actioned this Invoice and it has been locked out for 10 minutes. If after refreshing Internet Explorer you can still see the Invoice in your intray, it may then be actioned again after 10 minutes has elapsed.')</script>";
        //                    this.RegisterClientScriptBlock("script", strURL);
        //                    this.Page_Load(source, e);
        //                }
        //                else if (strDocType == "CRN")
        //                {
        //                    strURL = "<script language=javascript>alert('Another user has just actioned this CreditNote and it has been locked out for 10 minutes. If after refreshing Internet Explorer you can still see the CreditNote in your intray, it may then be actioned again after 10 minutes has elapsed.')</script>";
        //                    this.RegisterClientScriptBlock("script", strURL);
        //                    this.Page_Load(source, e);
        //                }
        //            }
        //        }
        //    }
        //}
        //#endregion
        //#region grdInvCur_ItemDataBound
        //private void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //{
        //    string sRetUrl = "../Current/CurrentStatus.aspx";
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        int iInvID = 0;
        //        iInvID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
        //        rptAttachment = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptAttachment");
        //        rptAttachment.DataSource = null;
        //        rptAttachment.DataBind();
        //        dtRepeater = GetAttachmentDetails(iInvID, Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")));
        //        if (dtRepeater.Rows.Count > 0)
        //        {
        //            rptAttachment.DataSource = dtRepeater;
        //            rptAttachment.DataBind();
        //        }

        //        // int iInvID = 0;
        //        //  iInvID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
        //        if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")).ToUpper() == "INV")
        //        {
        //            ((HyperLink)e.Item.FindControl("hpDoc")).NavigateUrl = "../invoice/InvoiceFileManager_NL.aspx?From=ETC&InvoiceID=" + iInvID + "&ReturnUrl=" + sRetUrl;
        //        }
        //        else if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")).ToUpper() == "CRN")
        //        {
        //            ((HyperLink)e.Item.FindControl("hpDoc")).NavigateUrl = "../creditnotes/CreditnoteFileManager_NL.aspx?From=ETC&CreditNoteID=" + iInvID + "&ReturnUrl=" + sRetUrl;
        //        }

        //        int sHold = objInvoice.GetAPCommLinkColor(iInvID, Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")));

        //        if (sHold == 1)
        //        {
        //            ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/red_hold.gif";

        //        }
        //        else if (sHold == 0)
        //        {
        //            ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/yellow_hold.gif";
        //        }
        //        else
        //        {
        //            ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/green_hold.gif";
        //        }
        //        if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "New_VendorClass")).Equals("PO"))
        //        {
        //            e.Item.BackColor = System.Drawing.Color.Red;
        //        }
        //        //if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IsDuplicate"))==1)
        //        //{
        //        //    e.Item.CssClass = "ColorDuplicateRow td";
        //        //}
        //        bool IsDuplicate = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsDuplicate"));
        //        if (IsDuplicate)
        //        {
        //            e.Item.CssClass = "ColorDuplicateRow td";
        //        }
        //        //if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IsDuplicate")) == 1)
        //        //{
        //        //    e.Item.CssClass = "ColorDuplicateRow td";
        //        //}

        //        if (ViewState["dtCheckAttachment"] != null)
        //        {
        //            DataTable dtAttachmentCheck = (DataTable)ViewState["dtCheckAttachment"];
        //            DataView dvSelectedAttachment = new DataView(dtAttachmentCheck);
        //            dvSelectedAttachment.Sort = "InvoiceID ASC";
        //            dvSelectedAttachment.RowFilter = "InvoiceID=" + Convert.ToInt32(iInvID) + " And DocType='" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "DocType")).ToUpper() + "'";
        //            if (dvSelectedAttachment.ToTable().Rows.Count > 0)
        //            {
        //                ((CheckBox)e.Item.FindControl("chkDownload")).Checked = true;
        //            }
        //        }
        //        //<a href='#' onclick="<%# GetStatusURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"))%>">

        //        string strStatusLogLink = GetStatusURL(DataBinder.Eval(e.Item.DataItem, "InvoiceID"), DataBinder.Eval(e.Item.DataItem, "DocType"));
        //        strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:560,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
        //        System.Web.UI.HtmlControls.HtmlAnchor aStatusHistory = ((System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("aStatusHistory"));
        //        aStatusHistory.Attributes.Add("onclick", strStatusLogLink);

        //    }

        //}
        //#endregion

        //=============================Commneted By Subhrajyoti on 21st July 2015 End=========================================


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

            strURL = "javascript:window.open('../../JKS/invoice/APComments.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocumentType + "&DocNo=" + strDocNo + "&DocStatus=" + strDocStatus + "','InvoiceStatusLogNL','width=700,height=450,scrollbars=1');";

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



                        Session["dtTiffViewer"] = dtTiffViewer;

                    }
                }
                dtSorted.AcceptChanges();

                //grdInvCur.DataSource = dtSorted;
                //grdInvCur.DataBind();
                ViewState["objDataTable"] = dtSorted;
                // grdInvCur.Visible = true;
                Session.Add("sorted", dtSorted);


                List<INVS> lstInvoiceid = new List<INVS>();
                //  Session["InvoiceID"] = "";

                for (int i = 0; i < ((DataTable)Session["sorted"]).Rows.Count; i++)
                {

                    //if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                    //{
                    //    if (Convert.ToString(((DataTable)Session["sorted"]).Rows[i]["DocStatus"]) != "Rejected")
                    //    {
                    //        INVS objINVS = new INVS();
                    //        objINVS.InvoiceID = Convert.ToString(((DataTable)Session["sorted"]).Rows[i]["Invoiceid"]);
                    //        objINVS.DocType = Convert.ToString(((DataTable)Session["sorted"]).Rows[i]["DocType"]);
                    //        lstInvoiceid.Add(objINVS);
                    //        Session.Add("InvoiceID", lstInvoiceid);
                    //    }
                    //}
                    //else
                    //{
                    INVS objINVS = new INVS();
                    objINVS.InvoiceID = Convert.ToString(((DataTable)Session["sorted"]).Rows[i]["Invoiceid"]);
                    objINVS.DocType = Convert.ToString(((DataTable)Session["sorted"]).Rows[i]["DocType"]);
                    lstInvoiceid.Add(objINVS);
                    Session.Add("InvoiceID", lstInvoiceid);
                    //}
                }
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
            Session["Sorted"] = "1";
        }
        #endregion
		
        private void GetBusinessUnit(int companyid)
        {
            ddlBusinessUnit.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetBusinessUnit", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.AddWithValue("@CompanyID", Convert.ToInt32(companyid));
            sqlDA.SelectCommand.Parameters.AddWithValue("@UsrID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.AddWithValue("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
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
            sqlDA.SelectCommand.Parameters.AddWithValue("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.AddWithValue("@DocType", DocType);
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
            Session["NewVendorClass"] = strNewVendorClass;
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
                    strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','InvoiceAction','width=540,height=450,top=100,left=805,scrollbars=0,resizable=1');";//rimi
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
                    //strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=' + screen.height + ',width=' + screen.width +',scrollbars=no,top=0,left=0,resizable=0');";//rimi commented by
                    strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=790px,width=1360px,scrollbars=no,top=0,left=0,resizable=0');";//rimi//commented by kuntal

                }
                else if (DocType == "INV")
                {
                    // strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','fullscreen,scrollbars');";
                    //strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=' + screen.height + ',width=' + screen.width +',scrollbars=no,top=0,left=0,resizable=0');";//rimi commented by
                    strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=790px,width=1360px,scrollbars=no,top=0,left=0,resizable=0');";//rimi//commented by kuntal
                }
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", strURL, true);
            return (strURL);
        }

        //#region :  Process Button
        //public void btnProcess_Click(object sender, EventArgs e)
        //{


        //    //if (Session["UserID"] == null)
        //    //{
        //    //    Response.Redirect("../../close_win.aspx");
        //    //}
        //    //else
        //    //{
        //    //    GetURLProcess();
        //    //}
        //}

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
                //GetCompanyListForPurchaseInvoiceLog(42413, 1);
                //================Added By Rimi On 30.06.2015===============================================
                if (Convert.ToString(ViewState["flag_return"]) != "0")
                {

                    ViewState["return"] = "1";
                }
                //================Added By Rimi On 30.06.2015===============================================
                //ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                btnSearch_Click(null, null);
            }


        }
        //    #endregion
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
            sqlDA.SelectCommand.Parameters.AddWithValue("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.AddWithValue("@DocType", DocType);
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
            JKS.Invoice_New objInv = new JKS.Invoice_New();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetSuppliersList_GRH", sqlConn);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetSuppliersList2_JKS", sqlConn);

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (CompanyID != "Select Company Name")
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            }
            sqlDA.SelectCommand.Parameters.AddWithValue("@UserID", Convert.ToInt32(userId));
            sqlDA.SelectCommand.Parameters.AddWithValue("@USerTypeID", Convert.ToInt32(userTypeID));
            sqlDA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyString", UserString);

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
            if (CompanyID != "Select Company Name")
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@CompanyID", Convert.ToInt32(CompanyID));
            }
            sqlDA.SelectCommand.Parameters.AddWithValue("@NominalStr", UserString);

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
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_FetchInvoiceNo_Current_GRH", sqlConn);
            //-----------------------------------------------------------------------
            //SqlDataAdapter sqlDA = new SqlDataAdapter("sp_FetchInvoiceNo", sqlConn);

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (CompanyID != "Select Company Name")
            {
                sqlDA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            }
            if (DocType != "")
                sqlDA.SelectCommand.Parameters.AddWithValue("@DocType", DocType.Trim());
            else
                sqlDA.SelectCommand.Parameters.AddWithValue("@DocType", DBNull.Value);
            sqlDA.SelectCommand.Parameters.AddWithValue("@InvoiceString", UserString.Trim());
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
            //blocked by kuntalkarar on  19thFeb2016
            // ((Label)grdItem.FindControl("lblInvoiceID")).Text.ToString();
            //Added by kuntalkarar on 19thFeb2016
            GridViewRow row = (GridViewRow)chkDownload.NamingContainer;
            Label lblID = (Label)row.FindControl("lblInvoiceID");

            string strInvoiceID = lblID.Text;
            //------------------------------------------------------
            //blocked by kuntalkarar on  19thFeb2016
            //string strDocType = ((Label)grdItem.FindControl("lblDocType")).Text.ToString();
            //Added by kuntalkarar on 19thFeb2016

            Label lblDocType = (Label)row.FindControl("lblDocType");
            string strDocType = lblDocType.Text;
            //------------------------------------------------------
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
        protected void btnPopup_Click(object sender, ImageClickEventArgs e)
        {
            PasswordReset objPasswordReset = new PasswordReset();
            grdApprovals.DataSource = objPasswordReset.GetApprovals(Convert.ToInt32(Session["UserID"]));
            grdApprovals.DataBind();
        }


        //=================Added By Subhrajyoti on 21st June For Indexing===================================


        private DataView Getdata()
        {
            //Session["dtdvEmployee"]
            DataTable dt = (DataTable)(Session["objDataTable"]);
            DataTable dts = new DataTable();
            DataView dvEmp = new DataView(dt);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(ViewState["Sort_Flag"]) == "1")
                    {
                        dvEmp.Sort = ViewState["SortExpr"].ToString();
                    }
                    dts = dvEmp.ToTable();
                }
            }

            List<INVS> lstInvoiceid = new List<INVS>();
            //Session["InvoiceID"] = "";//Modified by Subhrajyoti on 06-07-2015

            for (int i = 0; i < dts.Rows.Count; i++)
            {
                INVS objINVS = new INVS();
                objINVS.InvoiceID = Convert.ToString(dts.Rows[i]["Invoiceid"]);
                objINVS.DocType = Convert.ToString(dts.Rows[i]["DocType"]);
                lstInvoiceid.Add(objINVS);
                Session.Add("InvoiceID", lstInvoiceid);
            }
            return dvEmp;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Session["PageIndex"] = e.NewPageIndex;
            //DataView dvEmployee = Getdata();
            DataView dvEmployee;
            DataView dtEmp = (DataView)Session["dtdvEmployee"];

            if (dtEmp != null)
                dvEmployee = dtEmp;
            else
                dvEmployee = Getdata();

            GridView1.DataSource = dvEmployee;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Added By Kd 07.01.2019
               if(Session["NewInvoiceId"]!=null)
                    {
                        Session["NewInvoiceId"] = null;
                    }
            int index = 0;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["objDataTable"];
            if (Convert.ToString(e.CommandArgument) != "Net1" && Convert.ToString(e.CommandArgument) != "Currency" && Convert.ToString(e.CommandArgument) != "DocStatus"
                && Convert.ToString(e.CommandArgument) != "ReferenceNo" && Convert.ToString(e.CommandArgument) != "DocType" && Convert.ToString(e.CommandArgument) != "ScanDates"
                && Convert.ToString(e.CommandArgument) != "InvoiceDates" && Convert.ToString(e.CommandArgument) != "Supplier" && Convert.ToString(e.CommandArgument) != "Buyer"
                && Convert.ToString(e.CommandArgument) != "VendorID" && Convert.ToString(e.CommandArgument) != "VAT1" && Convert.ToString(e.CommandArgument) != "Total1")
            {
                //Blocked By Kd 05.12.2018
                //index = Convert.ToInt32(e.CommandArgument);

            }
            if (e.CommandName == "Action")
            {
                //Added By Kd 05.12.2018
                index = Convert.ToInt32(e.CommandArgument);
                if (index >= 15)
                {
                    //for (int i = 0; i < GridView1.Rows.Count; i++)
                    //{

                    int j = index - (15 * Convert.ToInt32(Session["PageIndex"]));
                    int i = 0;
                    if (j >= 15)
                    {
                        i = j - 15;
                    }
                    else
                    {
                        i = j;
                    }


                    string strInvoiceID = ((Label)GridView1.Rows[i].FindControl("lblInvoiceID")).Text;
                    string strDocType = ((Label)GridView1.Rows[i].FindControl("lblDocType")).Text;
                    string strInvoiceDate = ((Label)GridView1.Rows[i].FindControl("lblInvoiceDate")).Text;
                    string strDocStatus = ((Label)GridView1.Rows[i].FindControl("lblDocStatus")).Text;
                    string strVoucherNumber = ((Label)GridView1.Rows[i].FindControl("lblVoucherNumber")).Text;
                    string strVAT = ((Label)GridView1.Rows[i].FindControl("lblVAT")).Text;
                    string strTotal = ((Label)GridView1.Rows[i].FindControl("lblTot")).Text;
                    string strVendorClass = ((Label)GridView1.Rows[i].FindControl("lblVendorClass")).Text;
                    IFrameWindow(strInvoiceID, strDocType, strVAT, strTotal, strVendorClass, index);
                    // }

                }
                else
                {
                    string strInvoiceID1 = ((Label)GridView1.Rows[index].FindControl("lblInvoiceID")).Text;
                    string strDocType1 = ((Label)GridView1.Rows[index].FindControl("lblDocType")).Text;
                    string strInvoiceDate1 = ((Label)GridView1.Rows[index].FindControl("lblInvoiceDate")).Text;
                    string strDocStatus1 = ((Label)GridView1.Rows[index].FindControl("lblDocStatus")).Text;
                    string strVoucherNumber1 = ((Label)GridView1.Rows[index].FindControl("lblVoucherNumber")).Text;
                    string strVAT1 = ((Label)GridView1.Rows[index].FindControl("lblVAT")).Text;
                    string strTotal1 = ((Label)GridView1.Rows[index].FindControl("lblTot")).Text;
                    string strVendorClass1 = ((Label)GridView1.Rows[index].FindControl("lblVendorClass")).Text;
                    IFrameWindow(strInvoiceID1, strDocType1, strVAT1, strTotal1, strVendorClass1, index);
                }
            }



            //Added By Kd 05.12.2018
            else if (e.CommandName == "Status")
            {
                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string invoiceID = Convert.ToString(commandArgs[0]);
                ViewState["InvoiceID"] = invoiceID.Trim();
                string doctype = Convert.ToString(commandArgs[1]);
                if (doctype.Trim() == "CRN")
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





            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (e.CommandName.ToUpper() == "ACT")
                {
                    int IsPermit = 0;
                    string sWinParam = "", sHeightWidth = "";

                    string strInvoiceID = ((Label)GridView1.Rows[i].FindControl("lblInvoiceID")).Text;
                    string strDocType = ((Label)GridView1.Rows[i].FindControl("lblDocType")).Text;
                    string strInvoiceDate = ((Label)GridView1.Rows[i].FindControl("lblInvoiceDate")).Text;
                    string strDocStatus = ((Label)GridView1.Rows[i].FindControl("lblDocStatus")).Text;
                    string strVoucherNumber = ((Label)GridView1.Rows[i].FindControl("lblVoucherNumber")).Text;
                    string strVAT = ((Label)GridView1.Rows[i].FindControl("lblVAT")).Text;
                    string strTotal = ((Label)GridView1.Rows[i].FindControl("lblTot")).Text;
                    string strVendorClass = ((Label)GridView1.Rows[i].FindControl("lblVendorClass")).Text;

                    string strURL = "";
                    JKS.Invoice objinvoice = new JKS.Invoice();
                    string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

                    strRelationType = RelationType.ToString();
                    IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), strDocType);
                    if (IsPermit == 0)
                    {
                        if (RelationType == "STF" || RelationType == "STU")
                        {
                            sWinParam = "../../JKS/History/HistoryAction.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
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
                            // this.Page_Load(source, e);
                        }
                        else if (strDocType == "CRN")
                        {
                            strURL = "<script language=javascript>alert('Another user has just actioned this CreditNote and it has been locked out for 10 minutes. If after refreshing Internet Explorer you can still see the CreditNote in your intray, it may then be actioned again after 10 minutes has elapsed.')</script>";
                            this.RegisterClientScriptBlock("script", strURL);
                            //this.Page_Load(source, e);
                        }
                    }

                }
            }
        }



        //Added By Kd 04.12.2018

        #region GetInvoiceStatusDetails_INV
        private void GetInvoiceStatusDetails_INV(int iInvoiceID)
        {
            DataTable dtbl = new DataTable();


            lblauthstring.Text = "";
            lblDepartment.Text = "";
            lblBusinessUnit.Text = "";


            //lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "INV");
            //lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "INV");
            //lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "INV");
          // ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)

                //int invid = Convert.ToInt32(iInvoiceID);

                dtbl = objInvoice.GetInvoiceLogStatusApproverWise(iInvoiceID);

            else
                dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier(iInvoiceID);

            if (dtbl.Rows.Count > 0)
            {

                dgSalesCallDetails_CRN.Visible = false;
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
                lblMessage.Text = "Sorry, this document has no previous actions.";
            }
        }
        #endregion

       
        private DataTable dtbl = new DataTable();

        //Added By Kd 05.12.2018

        #region GetInvoiceStatusDetails_CRN
        private void GetInvoiceStatusDetails_CRN(int iInvoiceID)
        {

            lblauthstring.Text = "";
            lblDepartment.Text = "";
            lblBusinessUnit.Text = "";


            JKS.Invoice objInvoice = new JKS.Invoice();
            //lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "CRN");
            //lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "CRN");
            //lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "CRN");

            ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            else
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            //	dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier_CN(iInvoiceID);


            if (dtbl.Rows.Count > 0)
            {

                dgSalesCallDetails.Visible = false;

                dgSalesCallDetails_CRN.Visible = true;
                dgSalesCallDetails_CRN.DataSource = dtbl;
                dgSalesCallDetails_CRN.DataBind();
                lblauthstring.Text = dtbl.Rows[0]["AuthorisationString"].ToString();
                lblDepartment.Text = dtbl.Rows[0]["DepartmentName"].ToString();
                lblBusinessUnit.Text = dtbl.Rows[0]["BusinessUnitName"].ToString();
            }
            else
            {
                dgSalesCallDetails.Visible = false;
                lblMessage.Text = "Sorry, this document has no previous actions.";
            }
        }
        #endregion


        //Added By Kd 05.12.2018
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
            GetInvoiceStatusDetails_INV(Convert.ToInt32(ViewState["InvoiceID"]));
        }

        //Added By Kd 05.12.2018
        protected void dgSalesCallDetails_PageIndexChanged2(object source, DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < dgSalesCallDetails_CRN.PageCount)
            {
                mpe.Show();
                this.dgSalesCallDetails_CRN.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                mpe.Show();
                this.dgSalesCallDetails_CRN.CurrentPageIndex = dgSalesCallDetails_CRN.PageCount;
            }
            GetInvoiceStatusDetails_CRN(Convert.ToInt32(ViewState["InvoiceID"]));
        }




        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            int i = 0;
            int z = GridView1.Rows.Count;
            string sRetUrl = "../Current/CurrentStatus.aspx";
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            int iInvID = 0;
            iInvID = Convert.ToInt32(DataBinder.Eval(rowView, "InvoiceID"));



            if (e.Row.RowType == DataControlRowType.DataRow)// Bind nested grid view with parent grid view
            {




                i = e.Row.RowIndex;
                // find repeater grid view from paretn grid veiw
                Repeater rptAttachment = (Repeater)e.Row.FindControl("rptAttachment");

                rptAttachment.DataSource = null;
                rptAttachment.DataBind();
                dtRepeater = GetAttachmentDetails(iInvID, Convert.ToString(DataBinder.Eval(rowView, "DocType")));
                if (dtRepeater.Rows.Count > 0)
                {
                    rptAttachment.DataSource = dtRepeater;
                    rptAttachment.DataBind();
                }


                if (Convert.ToString(DataBinder.Eval(rowView, "DocType")).ToUpper() == "INV")
                {
                    ((HyperLink)e.Row.FindControl("hpDoc")).NavigateUrl = "../invoice/InvoiceFileManager_NL.aspx?From=ETC&InvoiceID=" + iInvID + "&ReturnUrl=" + sRetUrl;
                }
                else if (Convert.ToString(DataBinder.Eval(rowView, "DocType")).ToUpper() == "CRN")
                {
                    ((HyperLink)e.Row.FindControl("hpDoc")).NavigateUrl = "../creditnotes/CreditnoteFileManager_NL.aspx?From=ETC&CreditNoteID=" + iInvID + "&ReturnUrl=" + sRetUrl;
                }


                int sHold = objInvoice.GetAPCommLinkColor(iInvID, Convert.ToString(DataBinder.Eval(rowView, "DocType")));

                if (sHold == 1)
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("imgComment")).Src = "../../images/red_hold.gif";

                }
                else if (sHold == 0)
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("imgComment")).Src = "../../images/yellow_hold.gif";
                }
                else
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("imgComment")).Src = "../../images/green_hold.gif";
                }
                if (Convert.ToString(DataBinder.Eval(rowView, "New_VendorClass")).Equals("PO"))
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }

                bool IsDuplicate = Convert.ToBoolean(DataBinder.Eval(rowView, "IsDuplicate"));
                if (IsDuplicate)
                {
                    e.Row.CssClass = "ColorDuplicateRow td";
                }

                if (ViewState["dtCheckAttachment"] != null)
                {
                    DataTable dtAttachmentCheck = (DataTable)ViewState["dtCheckAttachment"];
                    DataView dvSelectedAttachment = new DataView(dtAttachmentCheck);
                    dvSelectedAttachment.Sort = "InvoiceID ASC";
                    dvSelectedAttachment.RowFilter = "InvoiceID=" + Convert.ToInt32(iInvID) + " And DocType='" + Convert.ToString(DataBinder.Eval(rowView, "DocType")).ToUpper() + "'";
                    if (dvSelectedAttachment.ToTable().Rows.Count > 0)
                    {
                        ((CheckBox)e.Row.FindControl("chkDownload")).Checked = true;
                    }
                }

                //Blocked By Kd 05.12.2018
                //string strStatusLogLink = GetStatusURL(DataBinder.Eval(rowView, "InvoiceID"), DataBinder.Eval(rowView, "DocType"));
                //strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:530,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                //System.Web.UI.HtmlControls.HtmlAnchor aStatusHistory = ((System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("aStatusHistory"));
                //aStatusHistory.Attributes.Add("onclick", strStatusLogLink);


            }
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtdvEmployee = new DataTable();
            string SortOrder = Convert.ToString(ViewState["Direction"]);
            ViewState["Sort_Flag"] = "1";

            if (SortOrder == "Asc")
            {
                ViewState["SortExpr"] = e.SortExpression + " " + "DESC";
                ViewState["Direction"] = "DESC";
            }
            else
            {
                ViewState["SortExpr"] = e.SortExpression + " " + "ASC";
                ViewState["Direction"] = "Asc";
            }
            Session["SortExpr"] = ViewState["SortExpr"];
            GridView1.DataSource = Getdata();
            GridView1.DataBind();
            DataView dvEmployee = Getdata();
            // dtdvEmployee =dvEmployee.Table;
            //ViewState["dvEmployee"] = dtdvEmployee;
            Session["dtdvEmployee"] = dvEmployee;


        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region Modified by Koushik Das on 22-JUNE-2017
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="SupplierName"></param>
        /// <param name="SupplierID"></param>
        /// <param name="SupplierNameHD"></param>
        /// <param name="isSupChecked"></param>
        /// <param name="VendorClass"></param>
        /// <param name="DocType"></param>
        /// <param name="DocStatus"></param>
        /// <param name="InvoiceNo"></param>
        /// <param name="InvoiceID"></param>
        /// <param name="InvoiceNoHD"></param>
        /// <param name="isInvoice"></param>
        /// <param name="PONo"></param>
        /// <param name="BusinessUnit"></param>
        /// <param name="Department"></param>
        /// <param name="Nominal"></param>
        /// <param name="NominalCodeId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="NetFrom"></param>
        /// <param name="NetTo"></param>
        private void StoreEditSession(string CompanyID, string SupplierName, string SupplierID, string SupplierNameHD, string isSupChecked, string VendorClass, string DocType, string DocStatus, string InvoiceNo, string InvoiceID, string InvoiceNoHD, string isInvoice, string PONo, string BusinessUnit, string Department, string Nominal, string NominalCodeId, string FromDate, string ToDate, string NetFrom, string NetTo)
        {
            Dictionary<string, dynamic> CurrentStatusEditReturnInfo = new Dictionary<string, dynamic>();

            CurrentStatusEditReturnInfo.Add("CompanyID", CompanyID);
            CurrentStatusEditReturnInfo.Add("SupplierName", SupplierName);
            CurrentStatusEditReturnInfo.Add("SupplierID", SupplierID);
            CurrentStatusEditReturnInfo.Add("SupplierNameHD", SupplierNameHD);
            CurrentStatusEditReturnInfo.Add("isSupChecked", isSupChecked);
            CurrentStatusEditReturnInfo.Add("VendorClass", VendorClass);
            CurrentStatusEditReturnInfo.Add("DocType", DocType);
            CurrentStatusEditReturnInfo.Add("DocStatus", DocStatus);
            CurrentStatusEditReturnInfo.Add("InvoiceNo", InvoiceNo);
            CurrentStatusEditReturnInfo.Add("InvoiceID", InvoiceID);
            CurrentStatusEditReturnInfo.Add("InvoiceNoHD", InvoiceNoHD);
            CurrentStatusEditReturnInfo.Add("isInvoice", isInvoice);
            CurrentStatusEditReturnInfo.Add("PONo", PONo);
            CurrentStatusEditReturnInfo.Add("BusinessUnit", BusinessUnit);
            CurrentStatusEditReturnInfo.Add("Department", Department);
            CurrentStatusEditReturnInfo.Add("Nominal", Nominal);
            CurrentStatusEditReturnInfo.Add("NominalCodeId", NominalCodeId);
            CurrentStatusEditReturnInfo.Add("FromDate", FromDate);
            CurrentStatusEditReturnInfo.Add("ToDate", ToDate);
            CurrentStatusEditReturnInfo.Add("NetFrom", NetFrom);
            CurrentStatusEditReturnInfo.Add("NetTo", NetTo);

            Session["CurrentStatusEditReturnInfo"] = CurrentStatusEditReturnInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ApplySessionWhenReturnedFromEditPage()
        {
            if (Request.UrlReferrer != null && Session["CurrentStatusEditReturnInfo"] != null)
            {
                string referal = Path.GetFileName(Request.UrlReferrer.AbsolutePath).ToLower();
                if (referal == "invoiceedit.aspx" || referal == "invoiceedit_cn.aspx" || referal == "currentstatus.aspx")
                {
                    Dictionary<string, dynamic> CurrentStatusEditReturnInfo = (Dictionary<string, dynamic>)Session["CurrentStatusEditReturnInfo"];

                    ddlCompany.SelectedValue = CurrentStatusEditReturnInfo["CompanyID"];
                    ddlCompany_SelectedIndexChanged(ddlCompany, new EventArgs());
                    txtSupplier.Value = CurrentStatusEditReturnInfo["SupplierName"];
                    HdSupplierId.Value = CurrentStatusEditReturnInfo["SupplierID"];
                    HdSupplierName.Value = CurrentStatusEditReturnInfo["SupplierNameHD"];
                    cbSupplier.Value = CurrentStatusEditReturnInfo["isSupChecked"];
                    ddlVendorClass.SelectedValue = CurrentStatusEditReturnInfo["VendorClass"];
                    //Commented By Mainak, 2018-11-06
                    //ddlDocType.SelectedValue = CurrentStatusEditReturnInfo["DocType"];
                    ddlDocStatus.SelectedValue = CurrentStatusEditReturnInfo["DocStatus"];
                    txtInvoiceNo.Text = CurrentStatusEditReturnInfo["InvoiceNo"];
                    hdInvoiceNo.Value = CurrentStatusEditReturnInfo["InvoiceID"];
                    hdInvoiceNoTxt.Value = CurrentStatusEditReturnInfo["InvoiceNoHD"];
                    cbInvoiceNo.Value = CurrentStatusEditReturnInfo["isInvoice"];
                    txtPONo.Text = CurrentStatusEditReturnInfo["PONo"];
                    ddlBusinessUnit.SelectedValue = CurrentStatusEditReturnInfo["BusinessUnit"];
                    ddldept.SelectedValue = CurrentStatusEditReturnInfo["Department"];
                    txtNominal.Value = CurrentStatusEditReturnInfo["Nominal"];
                    hdNominalCodeId.Value = CurrentStatusEditReturnInfo["NominalCodeId"];
                    txtFromDate.Value = CurrentStatusEditReturnInfo["FromDate"];
                    txtToDate.Value = CurrentStatusEditReturnInfo["ToDate"];
                    textRange1.Text = CurrentStatusEditReturnInfo["NetFrom"];
                    textRange2.Text = CurrentStatusEditReturnInfo["NetTo"];

                    btnSearch_Click(btnSearch, new EventArgs());

                    //--start-- added by Koushik Das as on 22-June-2017
                    if (Session["PageIndex"] != null)
                    {
                        GridView1.PageIndex = Convert.ToInt32(Session["PageIndex"]);
                        DataView dvEmployee;
                        DataView dtEmp = (DataView)Session["dtdvEmployee"];

                        if (dtEmp != null)
                            dvEmployee = dtEmp;
                        else
                            dvEmployee = Getdata();

                        GridView1.DataSource = dvEmployee;
                        GridView1.DataBind();
                    }
                    //--end-- added by Koushik Das as on 22-June-2017
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public static void SetPageIndex(int val)
        {
            HttpContext.Current.Session["PageIndex"] = val;
        }
        #endregion
    }
}