using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;

namespace JKS
{
    public class PassInvoice : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region Web Form Controls
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.DataGrid grdInvCur;
        protected System.Web.UI.WebControls.DropDownList ddlCompany;
        protected System.Web.UI.WebControls.DropDownList ddlSupplier;
        protected System.Web.UI.WebControls.DropDownList ddlActionStatus;
        protected System.Web.UI.WebControls.DropDownList ddlDocStatus;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.DropDownList ddlUsers;
        protected System.Web.UI.WebControls.DropDownList ddlInvoiceNo;
        protected System.Web.UI.WebControls.DropDownList ddlYear;
        protected System.Web.UI.WebControls.DropDownList ddlMonth;
        protected System.Web.UI.WebControls.DropDownList ddlday;
        protected System.Web.UI.WebControls.DropDownList ddlYear1;
        protected System.Web.UI.WebControls.DropDownList ddlMonth1;
        protected System.Web.UI.WebControls.DropDownList ddlday1;
        protected System.Web.UI.WebControls.Label lblMsg;
        protected System.Web.UI.WebControls.TextBox textRange1;
        protected System.Web.UI.WebControls.TextBox textRange2;
        protected System.Web.UI.WebControls.DropDownList ddldept;
        #endregion
        #region User Defined Variables
        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected DataSet ds = null;
        protected DataTable objDataTable = null;
        protected DataRow drInvoiceHeader = null;
        protected DataRow drInvoiceInvoiceLog = null;
        protected DataRow dr = null;
        private int iLoadFlag = 0;
        protected string strInvoiceDocumentDownloadPath = ConfigurationManager.AppSettings["InvoiceDocPath"];
        int currentYear = 0;
        private string strFromDate = Convert.ToString(1, 10) + "/" + Convert.ToString(1, 10) + "/" + Convert.ToString(2001, 10);
        private string strToDate = Convert.ToString(12, 10) + "/" + Convert.ToString(31, 10) + "/" + Convert.ToString(2010, 10);
        private decimal FromPrice = 0;
        private decimal ToPrice = 100000;
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");
            if (!IsPostBack)
            {
                Session["ApproveForm"] = 0;
                Session["SelectedPage"] = "SalesInvoiceLog";
                iLoadFlag++;
                Session["DropDownCompanyID"] = null;
                btnSearch.Attributes.Add("onclick", "javascript:return fn_Validate();");
                SetCompanyID(Session["CompanyID"].ToString());
                string str1 = "";
                string str2 = "";
                str1 = "(BuyerID=" + ((int)Session["CompanyID"]).ToString() + " OR ParentCompanyID = " + ((int)Session["CompanyID"]).ToString() + ")";
                str2 = "(BuyerID=" + ((int)Session["CompanyID"]).ToString() + " OR ParentCompanyID = " + ((int)Session["CompanyID"]).ToString() + ")";
                ViewState["str1"] = str1;
                ViewState["str2"] = str2;
                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
                LoadDate();
            }

            if (Session["DropDownCompanyID"] != null)
                LoadData(Convert.ToInt32(Session["DropDownCompanyID"]));
            else
                LoadData(Convert.ToInt32(Session["CompanyID"]));
            if (Convert.ToInt32(Session["UserTypeID"]) == 1 && iLoadFlag == 1)
            {
                LoadDataAtLogin(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]));
                iLoadFlag++;
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
            this.Load += new System.EventHandler(this.Page_Load);
            this.grdInvCur.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.grdInvCur_PageIndexChanged);
        }
        #endregion
        #region grdInvCur_PageIndexChanged
        public void grdInvCur_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < grdInvCur.PageCount)
            {
                grdInvCur.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                grdInvCur.CurrentPageIndex = grdInvCur.PageCount;
            }
            LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
        }
        #endregion
        #region GetURL
        protected string GetURL(object oInvoiceID)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strURL = "";
            strURL = "javascript:window.open('InvoicePassToUserNL.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "','abb','width=975,height=450');";
            return (strURL);
        }
        #endregion
        #region GetData
        private void GetData(string strCondition)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rsCur = da.ExecuteQuery("vPurchaseInvoiceHistory_NL", strCondition);
            grdInvCur.DataSource = rsCur.ParentTable;
            grdInvCur.DataBind();
        }
        #endregion
        #region CreateTable
        private void CreateTable()
        {
            objDataTable = new DataTable("TotalInvoiceDetails");
            objDataTable.Columns.Add("InvoiceID");
            objDataTable.Columns.Add("ReferenceNo");
            objDataTable.Columns.Add("SupplierCode");
            objDataTable.Columns.Add("Supplier");
            objDataTable.Columns.Add("VendorID");
            objDataTable.Columns.Add("InvoiceDate");
            objDataTable.Columns.Add("DeliveryDate");
            objDataTable.Columns.Add("Net");
            objDataTable.Columns.Add("VAT");
            objDataTable.Columns.Add("Total");
            objDataTable.Columns.Add("DocStatus");
            objDataTable.Columns.Add("ActionStatus");
            objDataTable.Columns.Add("User");
            objDataTable.Columns.Add("Comment");
            objDataTable.Columns.Add("ActionDate");
            objDataTable.Columns.Add("DocAttachments");
            objDataTable.Columns.Add("DocType");
            objDataTable.Columns.Add("ParentRowFlag");
            objDataTable.Columns.Add("BranchCode");
        }
        #endregion
        #region LoadData
        private void LoadData(int iCompanyID)
        {
            Invoice objInvoice = new Invoice();
            string strStatus = "";
            string strUserName = "";
            string strEmail = "";
            CreateTable();

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("stpGetPurchaseInvoiceHistoryForPassInvoicePage_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", ddlSupplier.SelectedValue.Trim());
            sqlDA.SelectCommand.Parameters.Add("@ActionStatusID", ddlActionStatus.SelectedValue.Trim());
            sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
            sqlDA.SelectCommand.Parameters.Add("@UserID", ddlUsers.SelectedValue.Trim());
            sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", ddlInvoiceNo.SelectedValue.Trim());
            sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);
            sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);
            sqlDA.SelectCommand.Parameters.Add("@FromPrice", FromPrice);
            sqlDA.SelectCommand.Parameters.Add("@ToPrice", ToPrice);

            if (Convert.ToInt32(Session["UserTypeID"]) > 1)
            {
                sqlDA.SelectCommand.Parameters.Add("@PassedToUserID", DBNull.Value);
                sqlDA.SelectCommand.Parameters.Add("@Option", 1);
            }
            else
            {
                sqlDA.SelectCommand.Parameters.Add("@PassedToUserID", Session["UserID"].ToString().Trim());
                sqlDA.SelectCommand.Parameters.Add("@Option", DBNull.Value);
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
                dr["VendorID"] = drInvoiceHeader["VendorID"];
                dr["InvoiceDate"] = drInvoiceHeader["InvoiceDate"];
                dr["DeliveryDate"] = drInvoiceHeader["DeliveryDate"];
                dr["Net"] = drInvoiceHeader["Net"];
                dr["VAT"] = drInvoiceHeader["VAT"];
                dr["Total"] = drInvoiceHeader["Total"];

                objInvoice.GetCurrentStatus(Convert.ToInt32(drInvoiceHeader["StatusID"]), out strStatus);
                dr["DocStatus"] = strStatus;
                dr["ActionStatus"] = drInvoiceHeader["ActionStatus"];

                objInvoice.GetUserName(Convert.ToInt32(drInvoiceHeader["ModUserID"]), out strUserName, out strEmail);
                dr["User"] = strUserName;
                dr["Comment"] = strUserName;
                dr["ActionDate"] = drInvoiceHeader["ModDate"];
                dr["DocAttachments"] = drInvoiceHeader["Document"];
                dr["DocType"] = drInvoiceHeader["New_DocumentType"];
                dr["ParentRowFlag"] = "1";
                if (drInvoiceHeader["BranchCode"] != DBNull.Value)
                    dr["BranchCode"] = drInvoiceHeader["BranchCode"];
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
                    grdInvCur.Visible = true;

                    grdInvCur.DataSource = dtbl;
                    grdInvCur.DataBind();
                }
                else
                {
                    grdInvCur.Visible = false;
                }
            }
            catch { grdInvCur.CurrentPageIndex = 0; LoadData(Convert.ToInt32(ddlCompany.SelectedValue)); }
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
        #region GetCompanyListForPurchaseInvoiceLog
        private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int iAction)
        {
            if (iAction == 1)
            {
                Company objCompany = new Company();
                ddlCompany.DataSource = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(iCompanyID), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                ddlCompany.DataBind();
                ddlCompany.Items.Insert(0, new ListItem("Select Company", "0"));
                ddlActionStatus.Items.Insert(0, new ListItem("Select Action Status", "0"));
                ddlActionStatus.Items.Insert(1, new ListItem("Pending", "P"));
                ddlActionStatus.Items.Insert(2, new ListItem("Completed", "C"));
                ddlActionStatus.Items.Insert(3, new ListItem("OverDue", "O"));

            }
            Invoice objInvoice = new Invoice();
            ddlSupplier.DataSource = objInvoice.GetSuppliersList(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
            ddlSupplier.DataBind();
            ddlDocStatus.DataSource = objInvoice.GetStatusList();
            ddlDocStatus.DataBind();
            ddlUsers.DataSource = objInvoice.GetUsersList(iCompanyID);
            ddlUsers.DataBind();
            ddlInvoiceNo.DataSource = objInvoice.GetInvoiceNo(iCompanyID, 1, Convert.ToInt32(Session["UserTypeID"]));
            ddlInvoiceNo.DataBind();
            if (Convert.ToInt32(Session["UserTypeID"]) > 1)
            {
                ddldept.DataSource = objInvoice.GetDepartmentListDropDown(1, 0);
                ddldept.DataBind();
                ddldept.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddldept.DataSource = objInvoice.GetDepartmentListDropDown(0, Convert.ToInt32(Session["UserID"]));
                ddldept.DataBind();
                ddldept.Items.Insert(0, new ListItem("Select", "0"));
            }
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
            ddlDocStatus.Items.Insert(0, new ListItem("Select Doc Status", "0"));
            ddlUsers.Items.Insert(0, new ListItem("Select User", "0"));
            ddlInvoiceNo.Items.Insert(0, new ListItem("Select Doc No", "0"));

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
            if (Convert.ToInt32(ddlYear.SelectedValue.Trim()) == 0 || Convert.ToInt32(ddlYear1.SelectedValue.Trim()) == 0 ||
                Convert.ToInt32(ddlMonth.SelectedValue.Trim()) == 0 || Convert.ToInt32(ddlMonth1.SelectedValue.Trim()) == 0 ||
                Convert.ToInt32(ddlday.SelectedValue.Trim()) == 0 || Convert.ToInt32(ddlday1.SelectedValue.Trim()) == 0)
            {
                lblMsg.Text = "Please select a valid date.";
                lblMsg.Visible = true;
                return;
            }
            else
            {
                if (Convert.ToInt32(ddlYear.SelectedValue.Trim()) > Convert.ToInt32(ddlYear1.SelectedValue.Trim()))
                {
                    lblMsg.Text = " 'From Date' can not be greater than 'To Date'";
                    return;
                }
                else if (Convert.ToInt32(ddlYear.SelectedValue.Trim()) == Convert.ToInt32(ddlYear1.SelectedValue.Trim()))
                {
                    if (Convert.ToInt32(ddlMonth.SelectedValue.Trim()) > Convert.ToInt32(ddlMonth1.SelectedValue.Trim()))
                    {
                        lblMsg.Text = " 'From Date' can not be greater than 'To Date";
                        return;
                    }
                    else if (Convert.ToInt32(ddlMonth.SelectedValue.Trim()) == Convert.ToInt32(ddlMonth1.SelectedValue.Trim()))
                    {
                        if (Convert.ToInt32(ddlday.SelectedValue.Trim()) > Convert.ToInt32(ddlday1.SelectedValue.Trim()))
                        {
                            lblMsg.Text = " 'From Date' can not be greater than 'To Date'";
                            return;
                        }
                    }
                }
                else
                {
                    lblMsg.Text = "";
                    strFromDate = ddlMonth.SelectedValue.Trim() + "/" + ddlday.SelectedValue.Trim() + "/" + ddlYear.SelectedValue.Trim();
                    strToDate = ddlMonth1.SelectedValue.Trim() + "/" + ddlday1.SelectedValue.Trim() + "/" + ddlYear1.SelectedValue.Trim();
                }
            }
            if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
            {
                FromPrice = Convert.ToDecimal(0);
                ToPrice = Convert.ToDecimal(1000000);
            }
            else
            {
                if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                {
                    lblMsg.Text = "From Range cannot greater than To Range";
                }
                else
                {
                    lblMsg.Text = "";
                    FromPrice = Convert.ToDecimal(textRange1.Text);
                    ToPrice = Convert.ToDecimal(textRange2.Text);

                }
            }

            try
            {
                grdInvCur.CurrentPageIndex = 0;
            }
            catch { }
            LoadDataSearch(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
        }
        #endregion
        #region ddlCompany_SelectedIndexChanged
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                grdInvCur.CurrentPageIndex = 0;
            }
            catch { }
            if (ddlCompany.SelectedIndex != 0)
            {
                try
                {
                    objDataTable.Dispose();
                    grdInvCur.Dispose();

                }
                catch { }
                Session["DropDownCompanyID"] = ddlCompany.SelectedValue;
                LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), 0);

            }
        }
        #endregion
        #region SetCompanyID
        private void SetCompanyID(string strCompanyID)
        {
            ddlCompany.SelectedValue = strCompanyID.Trim();
        }
        #endregion
        #region grdInvCur_ItemDataBound
        private void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    if (e.Item.Cells[6].Text.Trim().Equals("Debited") || e.Item.Cells[6].Text.Trim().Equals("Approved 2") || e.Item.Cells[6].Text.Trim().Equals("Delete/Archive"))
                        e.Item.Visible = false;
                }
                catch { }
            }
        }
        #endregion
        #region LoadDataAtLogin
        private void LoadDataAtLogin(int iCompanyID, int iPassedToUserID)
        {
            Invoice objInvoice = new Invoice();
            string strStatus = "";
            string strUserName = "";
            string strEmail = "";

            CreateTable();

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("stpGetInvoiceDetailsAtLoginNL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", DBNull.Value);
            sqlDA.SelectCommand.Parameters.Add("@PassedToUserID", iPassedToUserID);
            sqlDA.TableMappings.Add("InvoiceDetails", "InvoiceHeader");

            ds = new DataSet();
            sqlDA.Fill(ds, "InvoiceDetails");

            sqlDA.Dispose();
            sqlConn.Close();

            foreach (DataRow drInvoiceHeader in ds.Tables["InvoiceHeader"].Rows)
            {
                dr = objDataTable.NewRow();
                dr["InvoiceID"] = drInvoiceHeader["InvoiceID"];
                dr["ReferenceNo"] = drInvoiceHeader["ReferenceNo"];
                dr["SupplierCode"] = drInvoiceHeader["SupplierCode"];
                dr["Supplier"] = drInvoiceHeader["Supplier"];
                dr["VendorID"] = drInvoiceHeader["VendorID"];
                dr["InvoiceDate"] = drInvoiceHeader["InvoiceDate"];
                dr["DeliveryDate"] = drInvoiceHeader["DeliveryDate"];
                dr["Net"] = drInvoiceHeader["Net"];
                dr["VAT"] = drInvoiceHeader["VAT"];
                dr["Total"] = drInvoiceHeader["Total"];
                objInvoice.GetCurrentStatus(Convert.ToInt32(drInvoiceHeader["StatusID"]), out strStatus);
                dr["DocStatus"] = strStatus;
                dr["ActionStatus"] = drInvoiceHeader["ActionStatus"];
                objInvoice.GetUserName(Convert.ToInt32(drInvoiceHeader["ModUserID"]), out strUserName, out strEmail);
                dr["User"] = strUserName;
                dr["Comment"] = strUserName;
                dr["ActionDate"] = drInvoiceHeader["ModDate"];
                dr["DocAttachments"] = drInvoiceHeader["Document"];
                dr["DocType"] = "INV";
                dr["ParentRowFlag"] = "1";

                if (drInvoiceHeader["BranchCode"] != DBNull.Value)
                    dr["BranchCode"] = drInvoiceHeader["BranchCode"];
                objDataTable.Rows.Add(dr);
            }
            ViewState["objDataTable"] = objDataTable;
            PopulateGrid();
            CheckDuplicateValues();
        }
        #endregion
        #region GetStatusURL
        protected string GetStatusURL(object oInvoiceID)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strURL = "";
            strURL = "javascript:window.open('invoicestatuslogNL.aspx?InvoiceID=" + strInvoiceID + "','a','width=700,height=450');";
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
                    if ((grdInvCur.Items[i].Cells[5].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[5].Text.Trim())) && (grdInvCur.Items[i].Cells[6].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[6].Text.Trim())))
                    {
                        grdInvCur.Items[i - 1].BackColor = Color.Red;
                        grdInvCur.Items[i].BackColor = Color.Red;
                    }
                }
            }
        }
        #endregion
        #region ddlSupplier_SelectedIndexChanged
        private void ddlSupplier_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlSupplier.SelectedIndex != 0)
            {
                try
                {
                    grdInvCur.CurrentPageIndex = 0;

                }
                catch { }
                Invoice objInvoice = new Invoice();
                try
                {
                    ddlInvoiceNo.ClearSelection();

                }
                catch { }
                ddlInvoiceNo.DataSource = objInvoice.GetInvoiceNo(Convert.ToInt32(ddlSupplier.SelectedValue), 2, Convert.ToInt32(Session["UserTypeID"]));
                ddlInvoiceNo.DataBind();
                ddlInvoiceNo.Items.Insert(0, new ListItem("Select Doc No", "0"));
                try
                {
                    ddldept.ClearSelection();
                }
                catch
                {
                    if (Convert.ToInt32(Session["UserTypeID"]) > 1)
                    {
                        ddldept.DataSource = objInvoice.GetDepartmentListDropDown(1, 0);
                        ddldept.DataBind();
                        ddldept.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ddldept.DataSource = objInvoice.GetDepartmentListDropDown(0, Convert.ToInt32(Session["UserID"]));
                        ddldept.DataBind();
                        ddldept.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
            }
        }
        #endregion
        #region LoadDate
        private void LoadDate()
        {

            ddlYear.Items.Insert(0, new ListItem("Year", "0"));
            ddlYear1.Items.Insert(0, new ListItem("Year", "0"));
            ddlMonth.Items.Insert(0, new ListItem("Month", "0"));
            ddlMonth1.Items.Insert(0, new ListItem("Month", "0"));
            ddlday.Items.Insert(0, new ListItem("Day", "0"));
            ddlday1.Items.Insert(0, new ListItem("Day", "0"));
            currentYear = Microsoft.VisualBasic.DateAndTime.Year(System.DateTime.Now);
            for (int i = (currentYear - 5); i <= (currentYear + 5); i++)
                ddlYear.Items.Add(i.ToString());
            for (int i = 1; i < 13; i++)
            {
                ddlMonth.Items.Add(new ListItem(Microsoft.VisualBasic.DateAndTime.MonthName(i, true), (i.ToString())));
                ddlMonth1.Items.Add(new ListItem(Microsoft.VisualBasic.DateAndTime.MonthName(i, true), (i.ToString())));
            }
            for (int i = 1; i < 32; i++)
            {
                ddlday.Items.Add(i.ToString());
                ddlday1.Items.Add(i.ToString());
            }
            for (int i = (currentYear - 5); i <= (currentYear + 5); i++)
                ddlYear1.Items.Add(i.ToString());
        }

        #endregion

        #region Company
        private void LoadDepartment()
        {
            Invoice objInvoice = new Invoice();
            if (Convert.ToInt32(Session["UserTypeID"]) > 1)
            {
                ddldept.DataSource = objInvoice.GetDepartmentListDropDown(1, 0);
                ddldept.DataBind();
                ddldept.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddldept.DataSource = objInvoice.GetDepartmentListDropDown(0, Convert.ToInt32(Session["UserID"]));
                ddldept.DataBind();
                ddldept.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        #endregion

        #region SearchData


        private void LoadDataSearch(int iCompanyID)
        {
            Invoice objInvoice = new Invoice();
            string strStatus = "";
            string strUserName = "";
            string strEmail = "";
            CreateTable();
            if (Convert.ToInt32(ddldept.SelectedValue.Trim(), 10) == 0)
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlDA = new SqlDataAdapter("stpGetPurchaseInvoiceHistoryForPassInvoicePage_NL", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
                sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", ddlSupplier.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@ActionStatusID", ddlActionStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@UserID", ddlUsers.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", ddlInvoiceNo.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);
                sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);
                sqlDA.SelectCommand.Parameters.Add("@FromPrice", FromPrice);
                sqlDA.SelectCommand.Parameters.Add("@ToPrice", ToPrice);
            }
            else
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlDA = new SqlDataAdapter("stpGetPurchaseInvoiceHistoryForPassInvoicePageSearch_NL", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
                sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", ddlSupplier.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@ActionStatusID", ddlActionStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@UserID", ddlUsers.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", ddlInvoiceNo.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);
                sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);
                sqlDA.SelectCommand.Parameters.Add("@FromPrice", FromPrice);
                sqlDA.SelectCommand.Parameters.Add("@ToPrice", ToPrice);
                sqlDA.SelectCommand.Parameters.Add("@DepartmentID", ddldept.SelectedValue.Trim());
            }

            if (Convert.ToInt32(Session["UserTypeID"]) > 1)
            {
                sqlDA.SelectCommand.Parameters.Add("@PassedToUserID", DBNull.Value);
                sqlDA.SelectCommand.Parameters.Add("@Option", 1);
            }
            else
            {
                sqlDA.SelectCommand.Parameters.Add("@PassedToUserID", Session["UserID"].ToString().Trim());
                sqlDA.SelectCommand.Parameters.Add("@Option", DBNull.Value);
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
                dr["VendorID"] = drInvoiceHeader["VendorID"];
                dr["InvoiceDate"] = drInvoiceHeader["InvoiceDate"];
                dr["DeliveryDate"] = drInvoiceHeader["DeliveryDate"];
                dr["Net"] = drInvoiceHeader["Net"];
                dr["VAT"] = drInvoiceHeader["VAT"];
                dr["Total"] = drInvoiceHeader["Total"];
                objInvoice.GetCurrentStatus(Convert.ToInt32(drInvoiceHeader["StatusID"]), out strStatus);
                dr["DocStatus"] = strStatus;
                dr["ActionStatus"] = drInvoiceHeader["ActionStatus"];
                objInvoice.GetUserName(Convert.ToInt32(drInvoiceHeader["ModUserID"]), out strUserName, out strEmail);
                dr["User"] = strUserName;
                dr["Comment"] = strUserName;
                dr["ActionDate"] = drInvoiceHeader["ModDate"];
                dr["DocAttachments"] = drInvoiceHeader["Document"];
                dr["DocType"] = "INV";
                dr["ParentRowFlag"] = "1";
                if (drInvoiceHeader["BranchCode"] != DBNull.Value)
                    dr["BranchCode"] = drInvoiceHeader["BranchCode"];
                objDataTable.Rows.Add(dr);
            }
            ViewState["objDataTable"] = objDataTable;
            PopulateGrid();
            CheckDuplicateValues();
        }
        #endregion

    }
}