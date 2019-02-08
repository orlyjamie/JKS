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
#endregion

namespace JKS
{
    /// <summary>
    /// Created date:02-02-2012
    /// Created By: Rinku Santra
    /// Summary description for InvoiceActionNew.
    /// </summary>
    public class InvoiceActionNew : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Label lblConfirmation;
        protected System.Web.UI.WebControls.Label lblRefernce;
        protected System.Web.UI.WebControls.Label lblCurrentStatus;
        protected System.Web.UI.WebControls.Label lblInvoiceDate;
        protected System.Web.UI.WebControls.Label lblApprovalPath;
        protected System.Web.UI.WebControls.Label lblSupplier;
        protected System.Web.UI.WebControls.Label lblDepartment;
        protected System.Web.UI.WebControls.Label lblBuyer;
        protected System.Web.UI.WebControls.Label lblcreditnoteno;
        protected System.Web.UI.WebControls.Label lblApprovelMessage;
        protected System.Web.UI.WebControls.Label lblCRn;
        protected System.Web.UI.WebControls.Label lblErrorMsg;
        protected System.Web.UI.WebControls.Label lblTextReopen;
        protected System.Web.UI.WebControls.DataGrid grdList;
        protected System.Web.UI.WebControls.Button btnRetrieve;
        protected System.Web.UI.WebControls.Button btnAddNew;
        protected System.Web.UI.WebControls.Button btnDelLine;
        protected System.Web.UI.WebControls.Button btndelete;
        protected System.Web.UI.WebControls.TextBox txtComment;
        protected System.Web.UI.WebControls.TextBox txtDescription;
        protected System.Web.UI.WebControls.TextBox tbRejection;
        protected System.Web.UI.WebControls.CheckBox chbOpen;
        protected System.Web.UI.WebControls.DropDownList ddldept;
        protected System.Web.UI.WebControls.DropDownList ddlApprover1;
        protected System.Web.UI.WebControls.DropDownList ddlRejection;
        protected System.Web.UI.WebControls.DropDownList ddlApprover2;
        protected System.Web.UI.WebControls.DropDownList ddlApprover3;
        protected System.Web.UI.WebControls.DropDownList ddlApprover4;

        protected System.Web.UI.WebControls.TextBox txtCreditNoteNo;
        protected System.Web.UI.WebControls.LinkButton lnkCrn;
        protected System.Web.UI.WebControls.DropDownList ddlApprover5;
        protected System.Web.UI.WebControls.Button btnReopen;
        protected System.Web.UI.WebControls.Button btnApprove;
        protected System.Web.UI.WebControls.Button btnReject;
        protected System.Web.UI.WebControls.Button btnOpen;

        #endregion

        #region  objects declarations
        protected CBSolutions.ETH.Web.Invoice objInvoice = new CBSolutions.ETH.Web.Invoice();
        protected Company objCompany = new Company();
        private Akkeron.User.Users objUser = new Akkeron.User.Users();
        #endregion

        #region Sql Variables
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        #endregion

        #region  variables
        Invoice.Invoice objinvoice = new Invoice.Invoice();
        protected string AuthorisationStringToolTips = "";
        public int invoiceID = 0;
        //		private double dGrossAmount = 0;
        //		private bool bExceedLimit = false;
        //		private bool bCompareFlag = false;	
        //		private bool bApprovedDirectorFlag = false;

        int iApproverStatusID = 0;
        string strNewUserCode = "";
        string strNewUserGroup = "";
        string strPassedNewUserCode = "";
        string strPassedNewUserGroup = "";

        string sdApprover1 = "";
        string sdApprover2 = "";
        string sdApprover3 = "";
        string sdApprover4 = "";
        string sdApprover5 = "";

        //		private int iTimeLimitInHours = 48;			
        protected int iCurrentStatusID = 0;
        protected int TypeUser = 1;
        protected int RejectOpenFields = 0;
        protected string oldComment = "";
        protected string strAmountLimit = "";
        protected string strTimeLimit = "";

        protected double dTotalAmount = 0;
        protected System.Web.UI.WebControls.Button btnCancel;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Description;

        double dNetAmt = 0;
        protected int iSupplierCompanyID = 0;

        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            btnCancel.Attributes.Add("onclick", "javascript:windowclose();");
            btnApprove.Attributes.Add("onclick", "return CheckInvoiceDesc();");
            btnOpen.Attributes.Add("onclick", "return CheckOpenValid();");
            if (Request["DDCompanyID"] != null)
                Session["DropDownCompanyID"] = Request["DDCompanyID"].ToString();
            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
                ViewState["InvoiceID"] = invoiceID.ToString();
                Session["eInvoiceID"] = invoiceID.ToString();
                Session["InvoiceBuyerCompany"] = GetInvoiceBuyerCompanyID(Convert.ToInt32(Session["eInvoiceID"]));
            }
            TypeUser = Convert.ToInt32(Session["UserTypeID"]);

            if (!Page.IsPostBack)
            {
                PopulateDropDowns();
                if (invoiceID != 0)
                    GetDocumentDetails(invoiceID);

                Session["loaded"] = "0";
                CheckInvoiceExist();
            }

            ButtonVisibility();
            GetUserCodesAndUserGroupsByUserID();
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
            this.lnkCrn.Click += new System.EventHandler(this.lnkCrn_Click);
            this.grdList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdList_ItemDataBound);
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            this.btnDelLine.Click += new System.EventHandler(this.btnDelLine_Click);
            this.ddldept.SelectedIndexChanged += new System.EventHandler(this.ddldept_SelectedIndexChanged);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            this.btnReopen.Click += new System.EventHandler(this.btnReopen_Click);
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            this.Load += new System.EventHandler(this.Page_Load);
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
                GetAllComboCodes();
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

        #region GetDocumentDetails(int iinvoiceID)
        private void GetDocumentDetails(int iinvoiceID)
        {
            DataSet DsInv = new DataSet();
            DsInv = GetDocumentDetails(iinvoiceID, "INV");
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
                        try
                        {
                            lblApprovalPath.Text = DsInv.Tables[0].Rows[0]["ApprovalPath"].ToString().Replace(" ", "").Replace("	", "");
                            if (lblApprovalPath.Text != "")
                            {
                                AuthorisationStringToolTips = objinvoice.GetAuthorisationStringToolTips("select dbo.fn_GetApprovalPathToolTips_Generic('" + lblApprovalPath.Text.Trim() + "'," + iinvoiceID + ")");
                            }
                        }
                        catch { }
                        try
                        {
                            lblcreditnoteno.Text = GetMultipleCreditNotes();
                            lnkCrn.Text = lblcreditnoteno.Text.ToString();
                        }
                        catch { }

                        try
                        {
                            lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();
                            ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();
                            ddldept.SelectedValue = Convert.ToString(ViewState["DepartmentID"]);
                            int dept = 0;
                            dept = Convert.ToInt32(ddldept.SelectedValue);
                            if (dept != 0)
                                GetApproverDropDownsAgainstDepartment(dept);

                        }
                        catch { }
                        txtDescription.Text = DsInv.Tables[0].Rows[0]["Comment"].ToString();
                        ViewState["StatusID"] = DsInv.Tables[0].Rows[0]["StatusID"].ToString();
                        ViewState["OriginalNetAmount"] = DsInv.Tables[0].Rows[0]["Net"].ToString();
                        Session["eDocType"] = DsInv.Tables[0].Rows[0]["DocType"].ToString();
                    }
                }
            }
        }
        #endregion

        #region PopulateDropDowns
        private void PopulateDropDowns()
        {
            GetApproverDropDowns();
            GetDepartMentDropDwons();
            PopulateRejectionCode();
        }
        #endregion

        #region PopulateRejectionCode
        private void PopulateRejectionCode()
        {
            CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval();
            DataSet ds = new DataSet();
            string Fields = "RejectionCodeID, RejectionCode";
            string Table = "RejectionCode";
            string Criteria = "BuyerCompanyID =(SELECT ParentCompanyID FROM Company WHERE CompanyID =" + System.Convert.ToInt32(Session["InvoiceBuyerCompany"]) + ")";
            ds = objApproval.GetGlobalDropDowns(Fields, Table, Criteria);
            ddlRejection.DataSource = ds;
            ddlRejection.DataBind();
            ddlRejection.Items.Insert(0, "Select Comments");

        }
        #endregion

        #region GetApproverDropDowns()
        public void GetApproverDropDowns()
        {
            ddlApprover1.Items.Insert(0, "Select");
            ddlApprover2.Items.Insert(0, "Select");
            ddlApprover3.Items.Insert(0, "Select");
            ddlApprover4.Items.Insert(0, "Select");
            ddlApprover5.Items.Insert(0, "Select");

        }
        #endregion

        #region GetDepartMentDropDwons()
        public void GetDepartMentDropDwons()
        {
            CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval();
            DataSet ds = new DataSet();
            string Fields = "DepartmentID , Department";
            string Table = "Department";
            string Criteria = "BuyerCompanyID = " + System.Convert.ToInt32(Session["InvoiceBuyerCompany"]);
            ds = objApproval.GetGlobalDropDowns(Fields, Table, Criteria);
            ddldept.DataSource = ds;
            ddldept.DataBind();
            ddldept.Items.Insert(0, "Select");
        }
        #endregion


        #region GetUserCodesAndUserGroupsByUserID
        private void GetUserCodesAndUserGroupsByUserID()
        {
            DataSet ds = new DataSet();
            ds = GetUserCodeANDUserGroup(System.Convert.ToInt32(Session["UserID"]));
            strNewUserCode = ds.Tables[0].Rows[0]["New_UserCode"].ToString();
            strNewUserGroup = ds.Tables[0].Rows[0]["New_UserGroup"].ToString();
            if ((sdApprover1 == strNewUserCode) || (sdApprover1 == strNewUserGroup))
            {
                if (sdApprover1 == strNewUserCode)
                    strPassedNewUserCode = sdApprover1;
                else
                    strPassedNewUserGroup = sdApprover1;
            }
            else if ((sdApprover2 == strNewUserCode) || (sdApprover2 == strNewUserGroup))
            {
                if (sdApprover2 == strNewUserCode)
                    strPassedNewUserCode = sdApprover2;
                else
                    strPassedNewUserGroup = sdApprover2;
            }
            else if ((sdApprover3 == strNewUserCode) || (sdApprover3 == strNewUserGroup))
            {
                if (sdApprover3 == strNewUserCode)
                    strPassedNewUserCode = sdApprover3;
                else
                    strPassedNewUserGroup = sdApprover3;
            }
            else if ((sdApprover4 == strNewUserCode) || (sdApprover4 == strNewUserGroup))
            {
                if (sdApprover4 == strNewUserCode)
                    strPassedNewUserCode = sdApprover4;
                else
                    strPassedNewUserGroup = sdApprover4;
            }
            else if ((sdApprover5 == strNewUserCode) || (sdApprover5 == strNewUserGroup))
            {
                if (sdApprover5 == strNewUserCode)
                    strPassedNewUserCode = sdApprover5;
                else
                    strPassedNewUserGroup = sdApprover5;
            }

        }
        #endregion
        #region ButtonVisibility()
        private void ButtonVisibility()
        {
            if (TypeUser == 1)
            {
                if (Convert.ToInt32(ViewState["StatusID"]) == 20)  // when Received
                {
                    btndelete.Visible = false;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                }
                else if (Convert.ToInt32(ViewState["StatusID"]) == 6)  // when Received
                {
                    btndelete.Visible = false;
                    btnReopen.Visible = true;
                    btnOpen.Visible = true;
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    RejectOpenFields = 1;
                }
                else
                {
                    btndelete.Visible = false;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnReject.Visible = true;
                    btnApprove.Visible = true;
                }

            }
            if (TypeUser == 2)
            {
                if (Convert.ToInt32(ViewState["StatusID"]) == 20)  // when Received
                {
                    btndelete.Visible = false;
                    btnReopen.Visible = false;
                    btnOpen.Visible = true;
                    btnApprove.Visible = false;
                    btnReject.Visible = true;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 6)  // when Rejected
                {
                    btndelete.Visible = false;
                    btnReopen.Visible = true;
                    btnOpen.Visible = false;
                    btnApprove.Visible = true;
                    btnReject.Visible = false;
                    RejectOpenFields = 1;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 1)  // when Unapproved
                {
                    btndelete.Visible = false;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                    RejectOpenFields = 1;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 21)  // when Registered
                {
                    btndelete.Visible = true;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    RejectOpenFields = 0;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 22)  // when Reopened
                {
                    btndelete.Visible = true;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    RejectOpenFields = 0;
                }

            }
            if (TypeUser > 2)
            {
                if (Convert.ToInt32(ViewState["StatusID"]) == 20)  // when Received
                {
                    btndelete.Visible = true;
                    btnReopen.Visible = false;
                    btnOpen.Visible = true;
                    btnApprove.Visible = false;
                    btnReject.Visible = true;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 6)  // when Rejected
                {
                    btndelete.Visible = true;
                    btnReopen.Visible = true;
                    btnOpen.Visible = false;
                    btnApprove.Visible = true;
                    btnReject.Visible = false;
                    RejectOpenFields = 1;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 1)  // when Unapproved
                {
                    btndelete.Visible = false;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                    RejectOpenFields = 1;
                }

                if (Convert.ToInt32(ViewState["StatusID"]) == 21)  // when Registered
                {
                    btndelete.Visible = true;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    RejectOpenFields = 0;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 22)  // when Reopened
                {
                    btndelete.Visible = true;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    RejectOpenFields = 0;
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
            sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);

                // here i need to use dataset instead of datareader
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

            GetAllComboCodes();

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
                // here i need to write code for opening in edit mode
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


        #region GetAllComboCodes
        private void GetAllComboCodes()
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


                    dt = objInvoice.GetGridProjectList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);

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

        #region GetAllComboCodesForNominalRefresh
        private void GetAllComboCodesForNominalRefresh()
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

                    if (TypeUser == 1)
                    { }
                    else
                    {
                        dt = objInvoice.GetGridProjectList(compid);
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);
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
            //			string ddlCodingDescription1="";
            string ddlDepartment1 = "", ddlProject1 = "", ddlNominalCode1 = "";
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
            {	//AP/Admin
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
                GetAllComboCodesForNominalRefresh();

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

        #region  SelectedIndexChanged_ddlNominalCode
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
                    if (dsCODES.Tables.Count > 0 && dsCODES.Tables[0].Rows.Count > 0)
                    {
                        CodingDescription = dsCODES.Tables[0].Rows[0]["CodingDescriptionID"].ToString();
                        strProjectName = dsCODES.Tables[0].Rows[0]["ProjectName"].ToString();
                        strProjectID = dsCODES.Tables[0].Rows[0]["ProjectID"].ToString();
                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = CodingDescription;
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dsCODES;
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), strProjectID);
                }
            }
        }
        #endregion

        #region  btnAddNew_Click
        private void btnAddNew_Click(object sender, System.EventArgs e)
        {
            Populate(0, "a");
        }
        #endregion

        #region Populate
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
            ArrayList arrLstProjCode = new ArrayList();
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
                    arrLstProjCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))).SelectedItem.Value);

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
                    arrLstProjCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))).SelectedItem.Value);

                    string strddd = ((System.Web.UI.WebControls.TextBox)(grdList.Items[0].FindControl("txtNetVal"))).Text;

                    strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
                    dr = ds.Tables[0].NewRow();
                    for (j = 0; j < 1; j++)
                    {
                        dr[j] = strValue[j].ToString();
                    }

                    ds.Tables[0].Rows.Add(dr);
                    ds.WriteXml(@"D:\Inetpub\wwwroot\ETH\Files\fff.xml");
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
                GetAllComboCodes();

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

            for (i = 0; i <= arrLstProjCode.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))).SelectedIndex = -1;
                SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))), arrLstProjCode[i].ToString());
            }

        }
        #endregion

        #region btnRetrieve_Click
        private void btnRetrieve_Click(object sender, System.EventArgs e)
        {
            GetAllComboCodesAddNew();
        }
        #endregion

        #region btnDelLine_Click
        private void btnDelLine_Click(object sender, System.EventArgs e)
        {
            int i = 0;
            DataSet ds = ((DataSet)(ViewState["data"]));
            ds.WriteXml(@"D:\Inetpub\wwwroot\ETH\Files\fff.xml");
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
                        ds1.WriteXml(@"D:\Inetpub\wwwroot\ETH\Files\fff.xml");
                    }
                    else if (ds1.Tables[0].Rows.Count != 1)
                    {
                        int iPass = 0;
                        iPass = ds1.Tables[0].Rows.Count - 1;
                        ds1.Tables[0].Rows[ds1.Tables[0].Rows.Count - 1].Delete();
                        Populate(iPass, "d");
                        ds1.WriteXml(@"D:\Inetpub\wwwroot\ETH\Files\fff.xml");
                    }

                }
            }
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
            int ProjectCodeID = 0;
            int DepartmentID = 0;
            int iValidFlag = 0;
            decimal NetValue = 0;
            double NetVal = 0;
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
                        Response.Write("<script>alert('Please select department name.');</script>");
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

                    #endregion
                }
            }
            if (iValidFlag == 1)
            {
                return false;
            }
            else
            {
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
                dtXML.Columns.Add("ProjectCodeID");
                dtXML.Columns.Add("NetValue");
                dtXML.Columns.Add("CodingValue");
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

                        if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue) != "--Select--")
                        {
                            ProjectCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue);
                        }
                        else
                            ProjectCodeID = 0;

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
                            sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
                            sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
                            sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
                            sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
                            sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
                            if (((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue.Trim() == "--Select--")
                                sb.Append("<ProjectCodeID>").Append(Convert.ToString("0")).Append("</ProjectCodeID>");
                            else
                                sb.Append("<ProjectCodeID>").Append(Convert.ToString(ProjectCodeID)).Append("</ProjectCodeID>");

                            sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
                            sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
                            sb.Append("</Rowss>");
                        }
                    }
                    dsXML.Tables.Add(dtXML);
                    sb.Append("</Root>");
                    string strXmlText = sb.ToString();
                    sb = null;


                    int retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
                    if (retvalalue > 0)
                    {
                        retval = true;

                    }
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

        #region SaveDetailDataForGMG()
        private bool SaveDetailDataForGMG()
        {

            int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            int CompanyID = 0;
            int CodingDescriptionID = 0;
            int NominalCodeID = 0;
            int ProjectCodeID = 0;
            int DepartmentID = 0;
            //			int iValidFlag=0;
            decimal NetValue = 0;
            bool flag = false;
            double NetVal = 0;
            bool retval = false;
            lblErrorMsg.Visible = false;

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
            dtXML.Columns.Add("ProjectCodeID");
            dtXML.Columns.Add("NetValue");
            dtXML.Columns.Add("CodingValue");
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

                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue) != "--Select--")
                    {
                        retval = true;
                        ProjectCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue);
                    }

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
                        sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
                        sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
                        sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
                        sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
                        sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
                        if (((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue.Trim() == "--Select--")
                            sb.Append("<ProjectCodeID>").Append(Convert.ToString("0")).Append("</ProjectCodeID>");
                        else
                            sb.Append("<ProjectCodeID>").Append(Convert.ToString(ProjectCodeID)).Append("</ProjectCodeID>");

                        sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
                        sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
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
        #region cboStatus_SelectedIndexChanged
        public void cboStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
        #endregion

        #region ddldept_SelectedIndexChanged
        protected void ddldept_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int dept = 0;
            if (Convert.ToString(ddldept.SelectedValue) != "Select")
                dept = Convert.ToInt32(ddldept.SelectedValue);
            GetApproverDropDownsAgainstDepartment(dept);
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

                int DeptUpdate = UpdateDepartmentAgainstInvoiceID();
                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                int StatusUpdate = objinvoice.UpdateInvStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                if (StatusUpdate == 1)
                {

                    objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                    doAction(0);
                    Response.Write("<script>alert('Invoice Deleted Successfully.');</script>");
                    Response.Write("<script>opener.location.reload(true);</script>");
                    Response.Write("<script>self.close();</script>");
                }
                else
                {
                    Response.Write("<script>alert('Invoice cannot be deleted');</script>");
                }

            }
        }
        #endregion

        #region btnApprove_Click
        private void btnApprove_Click(object sender, System.EventArgs e)
        {
            lblErrorMsg.Text = "";
            if (lblcreditnoteno.Text != "")
            {
                string[] arrCrnNos1 = lblcreditnoteno.Text.Split('/');
                int CrnIDss = 0;
                for (int i = 0; i < arrCrnNos1.Length; i++)
                {
                    CrnIDss = GetCreditNoteIDByCreditNoteNo(Convert.ToString(arrCrnNos1[i]));

                    int iCheckMultipleCoding = CheckCodingINVCRN(CrnIDss, "CRN");
                    if (iCheckMultipleCoding == 0)
                    {
                        Response.Write("<script>alert('Please apply coding to the Associated Credit Note');</script>");
                        return;
                    }
                }
            }
            if (Convert.ToString(ViewState["StatusID"]) == "6")
            {
                if (txtCreditNoteNo.Text.Trim() == "" && lblcreditnoteno.Text == "")
                {
                    Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                    return;
                }
            }
            if (Convert.ToInt32(ddlRejection.SelectedIndex) > 0)
            {
                Response.Write("<script>alert('Sorry,Invoice cannot be Approved. You have selected RejectionCode.');</script>");
                return;
            }
            Invoice.Invoice objinvoice = new Invoice.Invoice();
            string strComments = txtComment.Text.Trim();
            int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
            if (UserTypeID == 3 || UserTypeID == 2)
            {
                string strCreditInvoiceNoTemp = txtCreditNoteNo.Text.Trim();
                if (txtCreditNoteNo.Visible == true && txtCreditNoteNo.Text.Trim() != "")
                {
                    string strCreditInvoiceNo = CheckCreditNoteAgainstInvoice();

                    int iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNoTemp);
                    if (iRetValForUpdate == -101)
                    {
                        Response.Write("<script>alert('The credit note must be in Registered status.');</script>");
                        return;
                    }
                    if (iRetValForUpdate == -102)
                    {
                        Response.Write("<script>alert('The credit note must be in Registered status.');</script>");
                        return;
                    }
                    if (iRetValForUpdate == -103)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                        return;
                    }

                    int retVal = CheckIsFullCreditNote();
                    if (retVal == 0)
                    {
                        Response.Write("<script>alert('Sorry, you cannot approve a partial credit.');</script>");
                        int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                        return;
                    }
                    else
                    {
                        int iCheckCoding = CheckCodingINVCRN(iRetValForUpdate, "CRN");
                        if (iCheckCoding == 0)
                        {
                            Response.Write("<script>alert('Please apply coding to the Associated Credit Note');</script>");
                            return;
                        }

                        bool ret = SaveDetailData();
                        if (ret == true)
                        {
                            int DeptUpdate = UpdateDepartmentAgainstInvoiceID();

                            int ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), 1, txtComment.Text.Trim(), iRetValForUpdate, txtDescription.Text.Trim());
                            if (ApprovedStatus == 1)
                            {
                                doAction(0);
                                Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                                Response.Write("<script>opener.location.reload(true);</script>");
                                Response.Write("<script>self.close();</script>");
                            }
                            else if (ApprovedStatus == -111)
                            {
                                Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                                return;
                            }
                            else
                                Response.Write("<script>alert('Data not saved properly.');</script>");

                        }
                    }

                }
                else
                {
                    bool ret = SaveDetailData();
                    if (ret == true)
                    {
                        int DeptUpdate = UpdateDepartmentAgainstInvoiceID();
                        int ICrnID = 0;
                        ICrnID = GetCreditNoteIDAgainstInvoiceIDANDCompanyID();

                        int ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), 1, txtComment.Text.Trim(), ICrnID, txtDescription.Text.Trim());
                        if (ApprovedStatus == 1)
                        {
                            doAction(0);

                            Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                            Response.Write("<script>opener.location.reload(true);</script>");
                            Response.Write("<script>self.close();</script>");
                        }
                        else if (ApprovedStatus == -111)
                        {
                            Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                            return;
                        }
                        else
                            Response.Write("<script>alert('Data not saved properly.');</script>");
                    }
                }
            }
            else
            {
                int RetStatus = CheckPermissionToTakeAction(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), System.Convert.ToInt32(Session["UserID"]));
                if (RetStatus > 0)
                {
                    iApproverStatusID = 19;
                    bool ret = SaveDetailData();
                    if (ret == true)
                    {
                        int ICrnID = 0;
                        ICrnID = GetCreditNoteIDAgainstInvoiceIDANDCompanyID();
                        int ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), 1, txtComment.Text.Trim(), ICrnID, txtDescription.Text.Trim());
                        if (ApprovedStatus == 1)
                        {
                            doAction(0);
                            lblErrorMsg.Text = "Invoice Approved Successfully";
                            Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                            Response.Write("<script>opener.location.reload(true);</script>");
                            Response.Write("<script>self.close();</script>");
                        }
                        else if (ApprovedStatus == -112)
                        {
                            Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                            return;
                        }
                        else if (ApprovedStatus == -111)
                        {
                            Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                            return;
                        }
                        else
                            Response.Write("<script>alert('Data not saved properly.');</script>");

                    }
                }
                else
                {
                    Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                    return;
                }
            }
        }
        #endregion

        #region btnReject_Click
        private void btnReject_Click(object sender, System.EventArgs e)
        {
            bool retVal = true;
            lblErrorMsg.Visible = false;
            ViewState["iApproverStatusID"] = "6";
            iApproverStatusID = 6;
            Invoice.Invoice objinvoice = new Invoice.Invoice();
            if (tbRejection.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter a rejection comment.');</script>");
                retVal = false;
                return;
            }

            if (ddlRejection.SelectedIndex == 0)
            {
                Response.Write("<script>alert('Please select rejection code.');</script>");
                retVal = false;
                return;
            }
            int DeptUpdate = UpdateDepartmentAgainstInvoiceID();
            if (retVal == true)
            {

                int RetStatus = CheckPermissionToTakeAction(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), System.Convert.ToInt32(Session["UserID"]));
                if (RetStatus > 0)
                {

                    lblErrorMsg.Text = "";
                    string strComments = tbRejection.Text.Trim();

                    int Result = 0;
                    bool ret = SaveDetailDataForGMG();
                    Result = objinvoice.UpdateStatusToReject(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), ddlRejection.SelectedItem.Text.ToString(), strComments, Convert.ToInt32(ddlRejection.SelectedValue), System.Convert.ToInt32(Session["UserID"].ToString()), txtComment.Text.ToString());
                    if (Result == 2)
                    {
                        doAction(0);
                        Response.Write("<script>alert('Invoice Rejected Successfully');</script>");
                        Response.Write("<script>opener.location.reload(true);</script>");
                        Response.Write("<script>self.close();</script>");

                    }
                    else if (Result == 3)
                    {
                        Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                        return;
                    }
                    else
                    {
                        Response.Write("<script>alert('Invoice Already Rejected');</script>");
                    }

                }
                else
                {
                    Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                    return;
                }
            }


        }
        #endregion

        #region btnReopen_Click
        private void btnReopen_Click(object sender, System.EventArgs e)
        {
            lblErrorMsg.Visible = false;
            Invoice.Invoice objinvoice = new Invoice.Invoice();
            if (Convert.ToString(ViewState["StatusID"]) == "6")
            {
                if (txtCreditNoteNo.Text.Trim() == "")
                {
                    int iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (iRejectionCode == 1)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                    if (iRejectionCode == 2)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                }
                if (txtCreditNoteNo.Text.Trim().ToUpper() == "REOPEN")
                {
                    int iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (iRejectionCode == 1)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                    if (iRejectionCode == 2)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                }
            }

            if (txtComment.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter a comment.');</script>");
                return;
            }
            else
            {
                lblErrorMsg.Text = "";
                iApproverStatusID = 22;
                string strComments = txtComment.Text.Trim();
                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                int iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                if (UserTypeID == 3 || UserTypeID == 2)
                {

                    string strCreditInvoiceNo = "";
                    int iRetValForUpdate = 0;
                    int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
                    strCreditInvoiceNo = txtCreditNoteNo.Text.Trim();

                    if (strCreditInvoiceNo.Trim().ToUpper() != "REOPEN")
                    {

                        if (strCreditInvoiceNo.Trim() == "" && lblcreditnoteno.Text.Trim() != "")
                        {

                        }
                        else if (strCreditInvoiceNo.Trim() != "")
                        {

                            iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNo);
                            if (iRetValForUpdate == -101)
                            {
                                Response.Write("<script>alert('The credit note must be in Received or Registered status.');</script>");
                                return;
                            }
                            if (iRetValForUpdate == -102)
                            {
                                Response.Write("<script>alert('The credit note must be in Received or Registered status.');</script>");
                                return;
                            }
                            if (iRetValForUpdate == -103)
                            {
                                Response.Write("<script>alert('There is no creditnote against the given no.');</script>");
                                return;
                            }

                            int iReturnValue = CheckCreditNoteAgainstInvoice(iInvoiceID, strCreditInvoiceNo);
                            if (iReturnValue == -101)
                            {
                                Response.Write("<script>alert('The credit note does not match to the supplier or invoice currency. ');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            else if (iReturnValue == -103)
                            {
                                Response.Write("<script>alert('The credit note must be less than the value of the invoice.');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            else if (iReturnValue == -102 && txtCreditNoteNo.Text.Trim() != "reopen")
                            {
                                Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            //						}
                        }
                        else if (strCreditInvoiceNo.Trim() != "" && strCreditInvoiceNo.Trim() != lblcreditnoteno.Text.Trim())
                        {

                            iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNo);
                            if (iRetValForUpdate == -101)
                            {
                                Response.Write("<script>alert('The credit note must be in Received or Registered status.');</script>");
                                return;
                            }
                            if (iRetValForUpdate == -102)
                            {
                                Response.Write("<script>alert('The credit note must be in Received or Registered status.');</script>");
                                return;
                            }
                            if (iRetValForUpdate == -103)
                            {
                                Response.Write("<script>alert('There is no creditnote against the given no.');</script>");
                                return;
                            }

                            int iReturnValue = CheckCreditNoteAgainstInvoice(iInvoiceID, strCreditInvoiceNo);
                            if (iReturnValue == -101)
                            {
                                Response.Write("<script>alert('The credit note does not match to the supplier or invoice currency. ');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            else if (iReturnValue == -103)
                            {
                                Response.Write("<script>alert('The credit note must be less than the value of the invoice.');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            else if (iReturnValue == -102 && txtCreditNoteNo.Text.Trim() != "reopen")
                            {
                                Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }

                        }
                    }

                    int DeptUpdate = UpdateDepartmentAgainstInvoiceID();

                    if ((txtCreditNoteNo.Text.Trim() == "" && iRejectionCode == 6) || txtCreditNoteNo.Text.Trim().ToUpper() == "REOPEN")
                    {
                        int iret = 0;
                        bool ret = SaveDetailDataForGMG();

                        if (chbOpen.Checked == true)
                            iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 1);
                        else
                            iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 0);

                        if (iret == 1)
                        {
                            lblErrorMsg.Visible = false;
                            doAction(0);
                            //AP-Admin Reopens
                            Response.Write("<script>alert('Invoice Reopened Successfully'); window.opener.location.href = '../Current/CurrentStatus.aspx'; self.close();</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Invoice status cannot be reopened');</script>");
                            return;
                        }
                    }
                    else if (strCreditInvoiceNo.Trim() == "" && lblcreditnoteno.Text.Trim() != "")
                    {
                        int iret = 0;
                        bool ret = SaveDetailDataForGMG();

                        if (chbOpen.Checked == true)
                            iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 1);
                        else
                            iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 0);

                        if (iret == 1)
                        {
                            lblErrorMsg.Visible = false;
                            doAction(0);
                            //AP-Admin Reopens
                            Response.Write("<script>alert('Invoice Reopened Successfully'); window.opener.location.href = '../Current/CurrentStatus.aspx'; self.close();</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Invoice status cannot be reopened');</script>");
                            return;
                        }
                    }
                    else
                    {
                        int iret = 0;

                        bool ret = SaveDetailDataForGMG();

                        if (chbOpen.Checked == true)
                            iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 1);
                        else
                            iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 0);

                        if (iret == 1)
                        {
                            lblErrorMsg.Visible = false;
                            doAction(0);
                            Response.Write("<script>alert('Invoice Reopened Successfully');  window.opener.location.href = '../Current/CurrentStatus.aspx'; self.close();</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Invoice status cannot be reopened');</script>");
                            return;
                        }
                    }
                }
            }
        }
        #endregion

        #region btnOpen_Click
        private void btnOpen_Click(object sender, System.EventArgs e)
        {

            bool ret = SaveDetailDataForGMG();

            int i = SetDropDownValuesOnOpen(System.Convert.ToInt32(Session["UserID"].ToString()));
            if (i > 0)
            {
                int DeptUpdate = UpdateDepartmentAgainstInvoiceID();
                Response.Write("<script>alert('Invoice passed to user successfully.');</script>");
                Response.Write("<script>opener.location.reload(true);</script>");
                Response.Write("<script>self.close();</script>");
            }

        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Write("<script>opener.location.reload(true);</script>");
            Response.Write("<script>self.close();</script>");
        }
        #endregion

        #region SetDropDownValuesOnOpenButtonClick
        private int SetDropDownValuesOnOpenButtonClick(int iUserID)
        {
            string NewApprover1 = "";
            string NewApprover2 = "";
            string NewApprover3 = "";
            string NewApprover4 = "";
            string NewApprover5 = "";

            if (Convert.ToString(ddlApprover1.SelectedItem.Text) != "Select")
                NewApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);
            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
                NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
                NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
                NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);



            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnOpenButtonClick", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            sqlCmd.Parameters.Add("@UserID", iUserID);
            if (txtComment.Text == "")
                sqlCmd.Parameters.Add("@Comment", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());

            if (txtDescription.Text == "")
                sqlCmd.Parameters.Add("@Description", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Description", txtDescription.Text.Trim());

            if (NewApprover1 == "")
                sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover1", NewApprover1);

            if (NewApprover2 == "")
                sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);

            if (NewApprover3 == "")
                sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover3", NewApprover3);

            if (NewApprover4 == "")
                sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover4", NewApprover4);

            if (NewApprover5 == "")
                sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover5", NewApprover5);

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

        #region SetDropDownValuesOnPressingReopen
        private int SetDropDownValuesOnPressingReopen(int iUserID, int iChecked)
        {
            string NewApprover1 = "";
            string NewApprover2 = "";
            string NewApprover3 = "";
            string NewApprover4 = "";
            string NewApprover5 = "";

            if (Convert.ToString(ddlApprover1.SelectedItem.Text) != "Select")
                NewApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);
            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
                NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
                NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
                NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);



            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnPressingReopen", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            sqlCmd.Parameters.Add("@UserID", iUserID);
            if (txtComment.Text == "")
                sqlCmd.Parameters.Add("@Comment", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());

            if (txtDescription.Text == "")
                sqlCmd.Parameters.Add("@Description", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Description", txtDescription.Text.Trim());

            if (NewApprover1 == "")
                sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover1", NewApprover1);

            if (NewApprover2 == "")
                sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);

            if (NewApprover3 == "")
                sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover3", NewApprover3);

            if (NewApprover4 == "")
                sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover4", NewApprover4);

            if (NewApprover5 == "")
                sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover5", NewApprover5);

            sqlCmd.Parameters.Add("@IsTicked", iChecked);
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

        #region lnkCrn_Click
        private void lnkCrn_Click(object sender, System.EventArgs e)
        {
        }
        #endregion

        #region GetApproverDropDownsAgainstDepartment(int iDeptID)
        public void GetApproverDropDownsAgainstDepartment(int iDeptID)
        {
            CBSolutions.ETH.Web.dalkia.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.dalkia.ApprovalPath.Approval();
            DataSet ds = new DataSet();

            ds = objApproval.GetApproverDropDownsByDepartment(System.Convert.ToInt32(Session["InvoiceBuyerCompany"]), iDeptID);
            if (ds.Tables[0].Rows.Count > 0)
            {

                #region Approvers
                ddlApprover1.ClearSelection();
                ddlApprover1.DataSource = ds;
                ddlApprover1.DataBind();
                ddlApprover1.Items.Insert(0, "Select");

                ddlApprover2.ClearSelection();
                ddlApprover2.DataSource = ds;
                ddlApprover2.DataBind();
                ddlApprover2.Items.Insert(0, "Select");

                ddlApprover3.ClearSelection();
                ddlApprover3.DataSource = ds;
                ddlApprover3.DataBind();
                ddlApprover3.Items.Insert(0, "Select");

                ddlApprover4.ClearSelection();
                ddlApprover4.DataSource = ds;
                ddlApprover4.DataBind();
                ddlApprover4.Items.Insert(0, "Select");

                ddlApprover5.ClearSelection();
                ddlApprover5.DataSource = ds;
                ddlApprover5.DataBind();
                ddlApprover5.Items.Insert(0, "Select");
                #endregion

                string NewApprover = lblApprovalPath.Text;
                try
                {
                    String[] arrApprover = NewApprover.Split('/');
                    sdApprover1 = arrApprover[0].ToString();
                    sdApprover2 = arrApprover[1].ToString();
                    sdApprover3 = arrApprover[2].ToString();
                    sdApprover4 = arrApprover[3].ToString();
                    sdApprover5 = arrApprover[4].ToString();
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }

                try
                {
                    if (sdApprover1 != "")
                    {
                        ddlApprover1.SelectedValue = sdApprover1;
                        ViewState["sdApprover1"] = sdApprover1;
                    }
                    if (sdApprover2 != "")
                    {
                        ddlApprover2.SelectedValue = sdApprover2;
                        ViewState["sdApprover2"] = sdApprover2;
                    }
                    if (sdApprover3 != "")
                    {
                        ddlApprover3.SelectedValue = sdApprover3;
                        ViewState["sdApprover3"] = sdApprover3;
                    }
                    if (sdApprover4 != "")
                    {
                        ddlApprover4.SelectedValue = sdApprover4;
                        ViewState["sdApprover4"] = sdApprover4;
                    }
                    if (sdApprover5 != "")
                    {
                        ddlApprover5.SelectedValue = sdApprover5;
                        ViewState["sdApprover5"] = sdApprover5;
                    }
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
            }
            else
            {
                #region Clear And Insert Approvers
                ddlApprover1.Items.Clear();
                ddlApprover2.Items.Clear();
                ddlApprover3.Items.Clear();
                ddlApprover4.Items.Clear();
                ddlApprover5.Items.Clear();

                ddlApprover1.Items.Insert(0, "Select");
                ddlApprover2.Items.Insert(0, "Select");
                ddlApprover3.Items.Insert(0, "Select");
                ddlApprover4.Items.Insert(0, "Select");
                ddlApprover5.Items.Insert(0, "Select");
                #endregion
            }
        }
        #endregion

        #region SetDropDownValuesOnOpen
        private int SetDropDownValuesOnOpen(int iUserID)
        {
            string NewApprover1 = "";
            string NewApprover2 = "";
            string NewApprover3 = "";
            string NewApprover4 = "";
            string NewApprover5 = "";

            if (Convert.ToString(ddlApprover1.SelectedItem.Text) != "Select")
                NewApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);
            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
                NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
                NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
                NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);


            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnOpen", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            sqlCmd.Parameters.Add("@UserID", iUserID);
            if (txtComment.Text == "")
                sqlCmd.Parameters.Add("@Comment", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());

            if (txtDescription.Text == "")
                sqlCmd.Parameters.Add("@Description", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Description", txtDescription.Text.Trim());

            if (NewApprover1 == "")
                sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover1", NewApprover1);

            if (NewApprover2 == "")
                sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);

            if (NewApprover3 == "")
                sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover3", NewApprover3);

            if (NewApprover4 == "")
                sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover4", NewApprover4);

            if (NewApprover5 == "")
                sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover5", NewApprover5);


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


        #region RetValAppronalPathChange
        private int RetValAppronalPathChange()
        {
            int iRetVal = 0;
            string NewApprover1 = "";
            string NewApprover2 = "";
            string NewApprover3 = "";
            string NewApprover4 = "";
            string NewApprover5 = "";

            if (Convert.ToString(ddlApprover1.SelectedItem.Text) != "Select")
                NewApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);
            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
                NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
                NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
                NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);

            if (NewApprover1 == Convert.ToString(ViewState["sdApprover1"]) && NewApprover2 == Convert.ToString(ViewState["sdApprover2"]) && NewApprover3 == Convert.ToString(ViewState["sdApprover3"]))
            {
                iRetVal = 1;
            }
            else
            {
                SqlConnection sqlConn = new SqlConnection(ConsString);
                SqlCommand sqlCmd = null;
                SqlParameter sqlReturnParam = null;

                sqlCmd = new SqlCommand("sp_SetDropDownValuesOnREJECTION", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
                sqlCmd.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
                if (txtComment.Text == "")
                    sqlCmd.Parameters.Add("@Comment", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());

                if (txtDescription.Text == "")
                    sqlCmd.Parameters.Add("@Description", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@Description", txtDescription.Text.Trim());

                if (NewApprover1 == "")
                    sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover1", NewApprover1);

                if (NewApprover2 == "")
                    sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);

                if (NewApprover3 == "")
                    sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover3", NewApprover3);

                if (NewApprover4 == "")
                    sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover4", NewApprover4);

                if (NewApprover5 == "")
                    sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover5", NewApprover5);


                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                try
                {
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    iRetVal = Convert.ToInt32(sqlReturnParam.Value);
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); iRetVal = -1; }
                finally
                {
                    sqlReturnParam = null;
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
            }
            return iRetVal;
        }
        #endregion

        #region  public DataSet GetDocumentDetails(int InvoiceID,string Type)
        public DataSet GetDocumentDetails(int InvoiceID, string Type)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("GetDocumentDetails", sqlConn);
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

        #region GetNetAmt(int InvoiceID)
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

        #region InsertCodingChangeValuesByDeleting
        public int InsertCodingChangeValuesByDeleting(string XMLText, int invoiceID)
        {
            int RetVal = 0;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);

            try
            {
                sqlCmd = new SqlCommand("sp_InsertCodingChangeValuesByDeleting", sqlConn);
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

        #region CheckCreditNoteAgainstInvoice
        public int CheckCreditNoteAgainstInvoice(int iInvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;

            try
            {
                sqlCmd = new SqlCommand("sp_CheckCreditNoteAgainstInvoice", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
                sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iReturnValue = -1; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve
        public int UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(int InvoiceID, string strCreditNoteNo)
        {
            int ReturnVal = 0;
            SqlParameter sqlReturnParam = null;
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            try
            {
                sqlConn = new SqlConnection(ConsString);
                sqlCmd = new SqlCommand("sp_UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
                sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                ReturnVal = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch { ReturnVal = -1; }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
                sqlReturnParam = null;
            }
            return (ReturnVal);
        }
        #endregion

        #region CheckIsFullCreditNote
        private int CheckIsFullCreditNote()
        {
            int iReturnValue = 0;
            int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
            SqlParameter sqlRetParam = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            try
            {

                sqlCmd = new SqlCommand("sp_IsFullCreditNoteGMG", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
                sqlRetParam = sqlCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                sqlRetParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlRetParam.Value);
            }
            catch { iReturnValue = -1; }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return iReturnValue;
        }
        #endregion

        #region CheckCreditNoteAgainstInvoice
        private string CheckCreditNoteAgainstInvoice()
        {
            string InvoiceNo = "";
            int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
            SqlParameter sqlOutputParam = null;
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            sqlConn = new SqlConnection(ConsString);
            try
            {
                sqlCmd = new SqlCommand("sp_GetCreditNoteByInvoiceID", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
                sqlOutputParam = sqlCmd.Parameters.Add("@CreditInvoiceNo", SqlDbType.VarChar, 20);
                sqlOutputParam.Direction = ParameterDirection.Output;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                InvoiceNo = sqlOutputParam.Value.ToString();
            }
            catch { }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return InvoiceNo;
        }
        #endregion

        #region UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo
        public int UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo(int InvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;
            try
            {
                sqlCmd = new SqlCommand("sp_UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
                sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch { iReturnValue = -1; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region GetUserCodeANDUserGroup(int iUserID)
        public DataSet GetUserCodeANDUserGroup(int iUserID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT New_UserCode,New_UserGroup FROM Users WHERE UserID =" + iUserID;
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

        #region GetCreditNoteIDAgainstInvoiceIDANDCompanyID
        public int GetCreditNoteIDAgainstInvoiceIDANDCompanyID()
        {
            int IRetVal = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT ISNULL(CreditNoteID,0) AS CreditNoteID FROM CreditNote C WHERE CreditInvoiceNo =(SELECT InvoiceNo From Invoice I Where InvoiceID =" + System.Convert.ToInt32(Session["eInvoiceID"]) + " AND C.BuyerCompanyID = I.BuyerCompanyID AND C.SupplierCompanyID = I.SupplierCompanyID)";
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                IRetVal = Convert.ToInt32(Dst.Tables[0].Rows[0]["CreditNoteID"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); IRetVal = -201; }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return IRetVal;
        }
        #endregion

        #region GetNominalCodeAgainstDepartmentANDCompany
        public DataSet GetNominalCodeAgainstDepartmentANDCompany(int iDepartmentCodeID, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in(SELECT NominalCodeID FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ")";
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

        #region GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo
        public int GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo(int InvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;
            sqlConn = new SqlConnection(ConsString);
            try
            {
                sqlCmd = new SqlCommand("sp_GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
                sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch { iReturnValue = -1; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
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
                sSql = "SELECT CodingDescriptionID,DDescription,ISNULL(ProjectID,0) AS ProjectID,(select ProjectName from Project where ProjectID=CodingDescription.ProjectID) as ProjectName FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " and DepartmentCodeID in (select DepartmentCodeID from userdeptrelation where UserID = " + Convert.ToInt32(Session["UserID"]) + ") and ProjectID <> 0 and ProjectID is not null";
            else
                sSql = "SELECT CodingDescriptionID,DDescription,ISNULL(ProjectID,0) AS ProjectID,(select ProjectName from Project where ProjectID=CodingDescription.ProjectID) as ProjectName FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " and ProjectID <> 0 and ProjectID is not null";

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
            sqlDA = new SqlDataAdapter("sp_GetDepartmentANDNominalAgainstCodingDescID", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CodingDescriptionID", iCodingID);
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompID);
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


        #region UpdatePassedToApproverAgainstInvoiceID(int invoiceID)
        private int UpdatePassedToApproverAgainstInvoiceID(int invoiceID)
        {
            int iretval = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = new SqlCommand("usp_UpdatePassedToApproverAgainstInvoiceID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", invoiceID);
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

        #region UpdateDepartmentAgainstInvoiceID()
        private int UpdateDepartmentAgainstInvoiceID()
        {
            int DeptID = 0;
            if (Convert.ToInt32(ddldept.SelectedIndex) > 0)
                DeptID = Convert.ToInt32(ddldept.SelectedValue);
            string strcomments = txtDescription.Text.Trim();

            int iretval = 0;
            string sSql = "UPDATE Invoice SET departmentid =" + DeptID + " ,Comment ='" + strcomments + "' WHERE InvoiceID =" + Convert.ToInt32(Session["eInvoiceID"]);

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

        #region GetInvoiceBuyerCompanyID
        public int GetInvoiceBuyerCompanyID(int iInvoiceID)
        {
            int BuyerID = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT BuyerCompanyID,SupplierCompanyID FROM Invoice WHERE InvoiceID=" + iInvoiceID;
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

        #region bool GetInvoiceRejectionCodeID(int iInvoiceID)
        public bool GetInvoiceRejectionCodeID(int iInvoiceID)
        {
            int BuyerID = 0;
            bool iret = true;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT ISNULL(New_RejectionCodeID,0)AS New_RejectionCodeID FROM Invoice WHERE Invoiceid =" + iInvoiceID;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables.Count > 0)
                    BuyerID = Convert.ToInt32(Dst.Tables[0].Rows[0]["New_RejectionCodeID"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            iret = false;
            return iret;
        }
        #endregion



        #region GetCreditNoteIDByCreditNoteNo(string strCreditNoteNo)
        public int GetCreditNoteIDByCreditNoteNo(string strCreditNoteNo)
        {
            int CreditNoteID = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT TOP 1 CreditNoteID FROM CreditNote WHERE InvoiceNo like '%" + strCreditNoteNo.Trim() + "%' AND BuyerCompanyID =" + Convert.ToInt32(Session["BuyerCID"]);
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables.Count > 0)
                    CreditNoteID = Convert.ToInt32(Dst.Tables[0].Rows[0]["CreditNoteID"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); CreditNoteID = 0; }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return CreditNoteID;

        }
        #endregion

        #region ButtonApprovePress_Generic
        public int ButtonApprovePress_Generic(int InvoiceID, int iUserID, int iUserTypeID, string Comments, int iCreditNoteID, string strInvoiceDesc)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = new SqlCommand("stp_ButtonApprovePress_Generic_GMG", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@UserID", iUserID);
            sqlCmd.Parameters.Add("@UserTypeID", iUserTypeID);
            sqlCmd.Parameters.Add("@Comments", Comments);
            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);
            sqlCmd.Parameters.Add("@strInvoiceDesc", strInvoiceDesc);

            sqlOutputParam = sqlCmd.Parameters.Add("@IRetVALUE", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iCount);
        }
        #endregion

        #region CheckPermissionToTakeAction
        public int CheckPermissionToTakeAction(int InvoiceID, int UserID)
        {
            int ReturnVal = 0;
            SqlParameter sqlReturnParam = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;

            sqlCmd = new SqlCommand("up_CheckPermissionToTakeAction", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@UserID", UserID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                ReturnVal = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
                sqlReturnParam = null;
            }
            return (ReturnVal);
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

        #region CheckCodingINVCRN
        public int CheckCodingINVCRN(int invoiceID, string DocType)
        {
            int IRetVal = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT Invoiceid AS Invoiceid FROM GenericCodingChange WHERE Invoiceid =" + invoiceID + " AND InvoiceType = '" + DocType + "'";
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                IRetVal = Convert.ToInt32(Dst.Tables[0].Rows[0]["Invoiceid"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return IRetVal;
        }
        #endregion

        #region GetMultipleCreditNotes()
        public string GetMultipleCreditNotes()
        {
            string strMultipleCreditNo = "";
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT INVOICENO FROM CREDITNOTE WHERE StatusID <> 7 AND CREDITINVOICENO = "
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
                    strMultipleCreditNo += "/";
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

        #region GetCreditLinks
        public string GetCreditLinks()
        {
            string strRet = "";
            string strNewWin = "";
            if (lblcreditnoteno.Text != "")
            {
                string[] arrCrnNos = lblcreditnoteno.Text.Split('/');
                int CrnNoPopUps = 0;
                for (int i = 0; i < arrCrnNos.Length; i++)
                {
                    CrnNoPopUps = GetCreditNoteIDByCreditNoteNo(Convert.ToString(arrCrnNos[i]));
                    strNewWin = "window.open('../CreditNotes/ActionCredit.aspx?InvoiceID=" + CrnNoPopUps + "&DDCompanyID=" + Convert.ToInt32(Session["DropDownCompanyID"]) + "','crnpopups','width=940,height=380,scrollbars=1,resizable=1');";
                    if (arrCrnNos[i].ToString() != "")
                        strRet += "<a href='#' style='COLOR: red' onclick=" + strNewWin + ">" + arrCrnNos[i] + "</a><br>";
                }
            }
            return strRet;
        }
        #endregion

    }
}
