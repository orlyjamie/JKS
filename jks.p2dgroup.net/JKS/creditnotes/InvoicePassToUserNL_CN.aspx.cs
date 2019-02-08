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
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;
using DataDynamics.ActiveReports.Export;
using DataDynamics.ActiveReports.Design;
using System.Windows.Forms;

namespace CBSolutions.ETH.Web.ETC.CreditNotes
{
    /// <summary>
    /// Summary description for InvoicePassToUserNL_CN.
    /// </summary>
    public class InvoicePassToUserNL_CN : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region Web Controls
        protected System.Web.UI.WebControls.Label Label6;
        protected System.Web.UI.WebControls.Label lblInvoiceNo;
        protected System.Web.UI.WebControls.Button btnPDFGenerate;
        protected System.Web.UI.WebControls.Panel Panel4;
        protected System.Web.UI.WebControls.Label lblRefernce;
        protected System.Web.UI.WebControls.Label lblVATRegNo;
        protected System.Web.UI.WebControls.Label lblPaymentDueDAte;
        protected System.Web.UI.WebControls.Label lblTermsDiscount;
        protected System.Web.UI.WebControls.Label lblCustomerAccNo;
        protected System.Web.UI.WebControls.Label lblCurrency;
        protected System.Web.UI.WebControls.Label lblInvoiceDate;
        protected System.Web.UI.WebControls.Label lblPaymentTerms;
        protected System.Web.UI.WebControls.Label lblDespatchNoteNo;
        protected System.Web.UI.WebControls.Label lblDespatchDate;
        protected System.Web.UI.WebControls.Label lblTotal;
        protected System.Web.UI.WebControls.Label lblDeliveryAddress;
        protected System.Web.UI.WebControls.Label lblInvoiceAddress;
        protected System.Web.UI.WebControls.Label lblConfirmation;
        protected System.Web.UI.WebControls.DataGrid grdInvoiceDetails;
        protected System.Web.UI.WebControls.Button btnGenerateText;
        protected System.Web.UI.WebControls.Label lblSupplierAddress;
        protected System.Web.UI.WebControls.Label lblVAT;
        protected System.Web.UI.WebControls.Label lblNetTotal;
        protected System.Web.UI.WebControls.Label lblSupplier;
        protected System.Web.UI.WebControls.Label lblBuyer;
        protected System.Web.UI.WebControls.DropDownList cboStatus;
        protected System.Web.UI.WebControls.Button btnBack;
        protected System.Web.UI.WebControls.Button btnPassInvoice;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.Label OutError;
        protected System.Web.UI.WebControls.DropDownList cboUsers;
        protected System.Web.UI.WebControls.TextBox txtComment;
        protected System.Web.UI.WebControls.CheckBox chkCeo;
        protected System.Web.UI.WebControls.Label lblOverdue;
        protected System.Web.UI.WebControls.DropDownList ddlDepartment;
        protected System.Web.UI.WebControls.DropDownList ddlProject;
        protected System.Web.UI.WebControls.DropDownList ddlNominalCode;
        protected System.Web.UI.WebControls.DropDownList cboMonthDate;
        protected System.Web.UI.WebControls.DropDownList cboYearDate;
        protected System.Web.UI.WebControls.DropDownList cboDayDate;
        protected System.Web.UI.WebControls.Label lblTotalAmount;
        protected System.Web.UI.WebControls.DropDownList ddlBuyerCompanyCode;
        protected System.Web.UI.WebControls.Label lblCompanyCode;
        protected System.Web.UI.WebControls.TextBox txtDescription;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_FOR_Description;
        protected System.Web.UI.WebControls.Label lblApprovelMessage;
        protected System.Web.UI.WebControls.Label lblCurrentStatus;
        protected System.Web.UI.WebControls.CustomValidator CustomValidator_FOR_Status;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdWindowFlag;
        protected System.Web.UI.HtmlControls.HtmlTableRow trUsers;
        protected System.Web.UI.WebControls.Label lblAssociatedInvoice;
        protected System.Web.UI.WebControls.TextBox txtAssociatedInvoice;
        #endregion
        // =========================================================================================================
        #region User Variable Declaration
        RecordSet rsInvoiceHead = null;
        RecordSet rsInvoiceDetail = null;
        int invoiceID = 0;  //240505
        protected Invoice_CN objInvoice = new Invoice_CN();
        protected Invoice.Invoice oInvoice = new Invoice.Invoice();
        protected Company objCompany = new Company();
        private double dGrossAmount = 0;
        private bool bExceedLimit = false;
        private bool bApprovedDirectorFlag = false;
        protected double dTotalAmount = 0;
        private int statusID = 0;
        private int iTimeLimitInHours = 48;
        private string strStatus = "";
        private int iPreviousStatusID = 0;
        private int iPreviousPassedToUserID = 0;
        private int iCounter = 1;
        protected int iCurrentStatusID = 0;
        private bool bCompareFlag = false;
        protected System.Web.UI.WebControls.DropDownList ddlCodingDescription;
        protected string oldComment = "";
        #endregion
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");

            if (Request["DDCompanyID"] != null)
                Session["DropDownCompanyID"] = Request["DDCompanyID"].ToString();

            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
                ViewState["InvoiceID"] = invoiceID.ToString();
            }
            if (!IsPostBack)
            {
                // CHECKING THE USERTYPE ID AND THEN DISABLE THE SUBMIT BUTTON IF THE USER ID IS NOT PRESENT IN PASSEDTOUSERID COLUMN OF INVOICE TABLE.
                if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    int iRetVal = 0;
                    iRetVal = objInvoice.CheckPassedToUserID(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"]));
                    if (iRetVal == 0)
                    {
                        btnSubmit.Enabled = false;
                    }
                }
                objInvoice.GetTimeLimitForCompany(Convert.ToInt32(Session["CompanyID"]), out iTimeLimitInHours);

                if (Invoice_CN.GetOverDueStatus(Convert.ToInt32(ViewState["InvoiceID"]), iTimeLimitInHours))
                {
                    lblOverdue.Visible = true;
                    lblOverdue.Text = "Over Due";
                }
                OutError.Visible = false;
                Session.Remove("DuplicateInvoice");
                btnSubmit.Visible = true;

                SetSupplierName(Convert.ToInt32(ViewState["InvoiceID"]));
                PopulateApproverDropDown();
                LoadData();  //240505

                if (invoiceID == 0)
                {
                    if (Session["InvoiceID"] != null)
                        invoiceID = (int)Session["InvoiceID"];

                    //prepare invoice head recordset from XML
                    if (System.IO.File.Exists(Session["XMLInvoiceHeadFile_CN"].ToString()))
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXmlSchema(Session["XSDInvoiceHeadFile_CN"].ToString());
                        ds.ReadXml(Session["XMLInvoiceHeadFile_CN"].ToString());
                        rsInvoiceHead = new RecordSet(ds);
                    }

                    //prepare invoice detail recordset from XML
                    if (System.IO.File.Exists(Session["XSDInvoiceDetailFile_CN"].ToString()))
                    {
                        DataSet ds = new DataSet();

                        ds.ReadXmlSchema(Session["XSDInvoiceDetailFile_CN"].ToString());
                        ds.ReadXml(Session["XMLInvoiceDetailFile_CN"].ToString());
                        rsInvoiceDetail = new RecordSet(ds);
                    }
                    if (!IsPostBack)
                        PopulateData();
                }
                else
                {
                    if (!IsPostBack)  //240505
                        PopulateData(invoiceID);
                }
            }
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
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            this.ddlDepartment.SelectedIndexChanged += new System.EventHandler(this.ddlDepartment_SelectedIndexChanged);
            this.ddlNominalCode.SelectedIndexChanged += new System.EventHandler(this.ddlNominalCode_SelectedIndexChanged);
            this.CustomValidator_FOR_Status.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(this.CustomValidator_FOR_Status_ServerValidate);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.ddlCodingDescription.SelectedIndexChanged += new System.EventHandler(this.ddlCodingDescription_SelectedIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion

        #region PopulateData
        public void PopulateData(int invoiceID)
        {
            rsInvoiceHead = Invoice_CN.GetInvoiceHead(invoiceID);
            rsInvoiceDetail = Invoice_CN.GetInvoiceDetail(invoiceID);
            bExceedLimit = CheckGross(rsInvoiceHead);
            lblConfirmation.Visible = false;
            btnPassInvoice.Visible = false;
            btnSubmit.Visible = true;
            PopulateData();
        }
        #endregion
        // =========================================================================================================
        #region GetAddressLine
        private string GetAddressLine(string s)
        {
            if (s == null || s.Trim() == "")
                return "";
            else
                return s.Trim() + "<BR>";
        }
        #endregion
        // =========================================================================================================
        #region PopulateData FOR CREDIT NOTES

        public void PopulateData()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = null;
            //			this.grdInvoiceDetails.DataSource = rsInvoiceDetail.ParentTable;
            //			this.grdInvoiceDetails.DataBind();
            lblRefernce.Text = "Credit Note No.: " + rsInvoiceHead["InvoiceNo"].ToString();
            //lblPaymentDueDAte.Text = ((System.DateTime)rsInvoiceHead["PaymentDueDate"]).ToString("dd/MM/yyyy");
            if (rsInvoiceHead["InvoiceDate"] != DBNull.Value)
                lblInvoiceDate.Text = ((System.DateTime)rsInvoiceHead["InvoiceDate"]).ToString("dd/MM/yyyy");

            if (rsInvoiceHead["DeliveryDate"] != DBNull.Value)
                lblDespatchDate.Text = ((System.DateTime)rsInvoiceHead["DeliveryDate"]).ToString("dd/MM/yyyy");

            if (rsInvoiceHead["BuyerCompanyID"] != DBNull.Value)
                rs = Company.GetCompanyData(System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]));

            if (rsInvoiceHead["BuyerCompanyID"] != DBNull.Value)
                ViewState["BuyerCompanyID"] = rsInvoiceHead["BuyerCompanyID"].ToString();

            if (rs["CompanyName"] != DBNull.Value)
                lblBuyer.Text = rs["CompanyName"].ToString();

            #region POPULATE ASSOCIATED INVOICE NO.--TARAKESHWAR DATE 02-FEB-2006
            if (rsInvoiceHead["CreditInvoiceNo"] != DBNull.Value)
                txtAssociatedInvoice.Text = rsInvoiceHead["CreditInvoiceNo"].ToString();
            #endregion

            if (lblBuyer.Text.ToUpper().Trim().Equals("ETC"))
            {
                lblBuyer.Text = lblBuyer.Text + " Company";
            }

            //RecordSet rs1=null;
            RecordSet rsUsers = null;
            //int statusID=1;		

            //250505
            if (cboStatus.SelectedIndex != -1)
            {
                if (cboStatus.SelectedItem.Text.ToString().ToUpper().IndexOf("APPROVED", 0) > 0)
                    Session["ApproveForm"] = 0;
                else
                    Session["ApproveForm"] = 1;
            }
            //250505

            statusID = (int)rsInvoiceHead["statusID"];

            ViewState["StatusIDForHoldStatus"] = statusID.ToString();

            if (rsInvoiceHead["PassedToUserID"] != DBNull.Value)
                ViewState["VPassedToUserID"] = rsInvoiceHead["PassedToUserID"].ToString();
            else
                ViewState["VPassedToUserID"] = "0";

            // IF CREDITNOTE TABLE CONTAINS 08 IN CHANGEDCOMPANYCODE COLUMN THEN SET IT DEFAULT ON THE BUYER COMPANY CODE DROP DOWN
            if (rsInvoiceHead["ChangedCompanyCode"] != DBNull.Value)
            {
                try
                {
                    if (rsInvoiceHead["ChangedCompanyCode"].ToString().Trim().Equals("08"))
                    {
                        ddlBuyerCompanyCode.SelectedValue = "08";
                    }
                    else
                    {
                        if (ViewState["ActualCompanyCode"] != null)
                            ddlBuyerCompanyCode.SelectedValue = ViewState["ActualCompanyCode"].ToString().Trim();
                    }
                }
                catch { }
            }

            if (objInvoice.CompareUserIDForInvoice_CN(Convert.ToInt32(Session["UserID"]), Convert.ToInt32(rsInvoiceHead["CreditNoteID"])) || Convert.ToInt32(Session["UserTypeID"]) > 1)
                bCompareFlag = true;


            if (statusID == 3)
                statusID = 10;

            if (rsInvoiceHead["statusID"] != DBNull.Value)
            {
                iCurrentStatusID = Convert.ToInt32(rsInvoiceHead["statusID"]);

                strStatus = "";
                objInvoice.GetCurrentStatus(statusID, out strStatus);
                lblCurrentStatus.Text = strStatus;
            }

            int iPassedToUserID = 0;
            bool bPreviousFlag = false;


            if (statusID == 2) // FOR ONHOLD
            {
                #region CheckPassedToUserID
                if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    int iRetVal = 0;

                    iRetVal = objInvoice.CheckPassedToUserID(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"]));

                    if (iRetVal == 0)
                    {
                        btnSubmit.Enabled = false;
                        trUsers.Visible = false;
                        OutError.Visible = true;
                        OutError.Text = "Sorry, this invoice is on hold. Cannot process invoice.";
                    }
                    else if (iRetVal == 1)
                    {
                        statusID = 8;
                        btnSubmit.Enabled = true;
                        trUsers.Visible = true;
                    }


                }
                #endregion

                statusID = 8;
            }

            if (statusID == 15) // FOR ONHOLD BEFORE DIRECTOR'S APPROVAL
            {
                #region CheckPassedToUserID
                if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    int iRetVal = 0;

                    iRetVal = objInvoice.CheckPassedToUserID(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"]));

                    if (iRetVal == 0)
                    {
                        btnSubmit.Enabled = false;
                        trUsers.Visible = false;
                        OutError.Visible = true;
                        OutError.Text = "Sorry, this invoice is on hold. Cannot process invoice.";
                    }
                    else if (iRetVal == 1)
                    {
                        statusID = 8;
                        btnSubmit.Enabled = true;
                        trUsers.Visible = true;
                    }


                }
                #endregion

                statusID = 16;
            }


            if (statusID == 13) // FOR ONHOLD BEFORE 2ND APPROVAL
            {
                #region CheckPassedToUserID
                if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    int iRetVal = 0;

                    iRetVal = objInvoice.CheckPassedToUserID(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"]));

                    if (iRetVal == 0)
                    {
                        btnSubmit.Enabled = false;
                        trUsers.Visible = false;
                        OutError.Visible = true;
                        OutError.Text = "Sorry, this invoice is on hold. Cannot process invoice.";
                    }
                    else if (iRetVal == 1)
                    {
                        statusID = 8;
                        btnSubmit.Enabled = true;
                        trUsers.Visible = true;
                    }


                }
                #endregion

                statusID = 14;
            }

            if (statusID == 8 || statusID == 14 || statusID == 16)
            {
                int _previousStatusID = 0;
                int _previousPassedToUserID = 0;

                objInvoice.GetPreviousStatusID(Convert.ToInt32(ViewState["InvoiceID"]), out _previousStatusID, out _previousPassedToUserID);

                iPassedToUserID = _previousPassedToUserID;

                bPreviousFlag = true;
            }

            if (rsInvoiceHead["DDescription"] != DBNull.Value)
                txtDescription.Text = rsInvoiceHead["DDescription"].ToString();

            if (statusID == 5)
            {
                bApprovedDirectorFlag = true;
                bExceedLimit = false;
                statusID = 1;
            }

            if (lblOverdue.Text == "")
            {
                ValidateDropDownOptions(statusID);
            }
            else
            {
                cboStatus.DataSource = objInvoice.GetInvoiceStatusList();
                cboStatus.DataBind();
            }

            if (statusID == 8)
                statusID = 9;

            if (statusID == 14)
                statusID = 10;

            if (bApprovedDirectorFlag == true)
            {
                statusID = 5;
            }

            if ((statusID == 1) || (statusID == 9) || (statusID == 3) || (statusID == 11))
            {

                if (bExceedLimit)
                {
                    chkCeo.Enabled = true;
                    chkCeo.Checked = true;

                    if ((int)rsInvoiceHead["StatusID"] == 9)
                    {
                        if (Convert.ToBoolean(rsInvoiceHead["ApprovedCEO"]) == false)
                        {
                            rsInvoiceHead["StatusID"] = 11;
                            cboStatus.SelectedValue = "11";
                            statusID = 11;
                        }

                        if ((int)Session["ApproveForm"] == 1)
                        {
                            chkCeo.Enabled = false;
                        }
                    }
                }
                else
                {
                    cboUsers.Enabled = true;
                    chkCeo.Checked = false;
                    chkCeo.Enabled = false;
                }
            }

            string ApproverType = GetApproverType(rsInvoiceHead);


            if (bPreviousFlag == false)
            {
                if (rsInvoiceHead["PassedToUserID"] != DBNull.Value)
                    iPassedToUserID = Convert.ToInt32(rsInvoiceHead["PassedToUserID"]);
                else
                    iPassedToUserID = 0;
            }

            if (rsInvoiceHead["PassedToUserID"] != DBNull.Value && rsInvoiceHead["PreviousPassedToUserID"] != DBNull.Value)
            {
                if (Convert.ToInt32(rsInvoiceHead["PassedToUserID"]) == Convert.ToInt32(rsInvoiceHead["PreviousPassedToUserID"]))
                    iPassedToUserID = objInvoice.GetOldPassedToUserID_CN(Convert.ToInt32(rsInvoiceHead["CreditNoteID"]));
            }

            if (lblOverdue.Text == "")
            {
                if ((Convert.ToInt32(rsInvoiceHead["StatusID"]) == 8 || Convert.ToInt32(rsInvoiceHead["StatusID"]) == 2) && rsInvoiceHead["PreviousPassedToUserID"] == DBNull.Value)
                {
                    rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), 0, "", 1);  // *********
                }
                else if (statusID == 3 || statusID == 10 || statusID == 4)
                {
                    if (Convert.ToInt32(Session["UserTypeID"]) > 2 && (Convert.ToInt32(rsInvoiceHead["PassedToUserID"]) != Convert.ToInt32(Session["UserID"])))
                    {
                        RemoveStatusForManagementUsers();
                    }

                    rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), iPassedToUserID, ApproverType, statusID);
                }
                else if (statusID == 8 || statusID == 2)
                {
                    try
                    {
                        int iStatID = Convert.ToInt32(GetApproverStatus(Convert.ToInt32(ViewState["InvoiceID"])));

                        rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), iPassedToUserID, ApproverType, iStatID);

                    }
                    catch { }
                }
                else if (statusID == 14 || statusID == 13)
                {
                    try
                    {
                        int iStatID = Convert.ToInt32(GetApproverStatus(Convert.ToInt32(ViewState["InvoiceID"])));

                        rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), iPassedToUserID, ApproverType, iStatID);

                    }
                    catch { }
                }
                else if (statusID == 11 || statusID == 5)
                {
                    rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), 0, ApproverType, statusID);
                }
                else if ((statusID == 15 || statusID == 16) && bExceedLimit)
                {
                    statusID = 11;
                    rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), 0, ApproverType, statusID);
                }
                else
                {
                    if (bExceedLimit)
                    {
                        if (rsInvoiceHead["ApprovedCEO"] == DBNull.Value)
                        {
                            rsInvoiceHead["ApprovedCEO"] = 0;
                        }

                        if (Convert.ToInt32(rsInvoiceHead["ApprovedCEO"]) == 1 && statusID == 9)
                        {
                            if (Convert.ToInt32(rsInvoiceHead["StatusID"]) != 8)
                                statusID = 10;

                            rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), iPassedToUserID, ApproverType, statusID);
                        }
                        else
                        {
                            if (bCompareFlag == false)
                            {
                                if (statusID == 9 && Convert.ToInt32(rsInvoiceHead["StatusID"]) != 8 && Convert.ToInt32(rsInvoiceHead["StatusID"]) != 2)
                                    statusID = 10;
                            }

                            if (rsInvoiceHead["ApprovedCEO"] != DBNull.Value)
                            {
                                if (Convert.ToInt32(rsInvoiceHead["ApprovedCEO"]) == 1)
                                {
                                    rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), 0, "", statusID);
                                }
                                else
                                {
                                    rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), -1, "", statusID);
                                }
                            }
                            else
                            {
                                rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), 0, "", statusID);
                            }
                        }
                    }
                    else
                    {
                        if (bCompareFlag == false)
                        {
                            if (statusID == 9 && Convert.ToInt32(rsInvoiceHead["StatusID"]) != 8 && Convert.ToInt32(rsInvoiceHead["StatusID"]) != 2)
                                statusID = 10;
                        }
                        else if (statusID == 9 && Convert.ToInt32(Session["UserTypeID"]) > 2)
                        {
                            if (statusID == 9 && Convert.ToInt32(rsInvoiceHead["StatusID"]) == 2)
                                statusID = 9;
                            else if (statusID == 9 && Convert.ToInt32(rsInvoiceHead["StatusID"]) == 8)
                                statusID = 9;
                            else if (statusID == 9 && Convert.ToInt32(Session["UserTypeID"]) > 2 && (Convert.ToInt32(rsInvoiceHead["PassedToUserID"]) != Convert.ToInt32(Session["UserID"])))
                            {
                                statusID = 9;
                                RemoveStatusForManagementUsers();
                            }
                            else
                                statusID = 10;
                        }
                        else if (rsInvoiceHead["StatusID"] != DBNull.Value && rsInvoiceHead["PreviousStatusID"] != DBNull.Value)
                        {
                            if (Convert.ToInt32(rsInvoiceHead["StatusID"]) == 9 && Convert.ToInt32(rsInvoiceHead["PreviousStatusID"]) == 9 && Convert.ToInt32(Session["UserTypeID"]) == 2)
                                statusID = 9;
                            else if (Convert.ToInt32(rsInvoiceHead["StatusID"]) == 9 && Convert.ToInt32(rsInvoiceHead["PreviousStatusID"]) == 9)
                                statusID = 10;
                        }

                        rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), iPassedToUserID, ApproverType, statusID);  // *********
                    }
                }
            }
            else
            {
                if (bCompareFlag == false)
                {
                    if (statusID == 9)
                        statusID = 10;
                }

                rsUsers = da.ExecuteSP("up_GetPassingUsers_CN", Convert.ToInt32(ViewState["InvoiceID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), 0, "", statusID);
            }
            try
            {
                cboUsers.Items.Clear();
                rsUsers.MoveFirst();

                while (!rsUsers.EOF())
                {
                    ListItem listItem = null;
                    listItem = new ListItem(rsUsers["Name"].ToString(), rsUsers["UserId"].ToString());

                    cboUsers.Items.Add(listItem);
                    rsUsers.MoveNext();
                }

            }
            catch { }


            if (rsInvoiceHead["DepartmentID"] != DBNull.Value)
            {
                try
                {
                    ddlDepartment.SelectedValue = rsInvoiceHead["DepartmentID"].ToString();

                }
                catch { }
            }

            if (rsInvoiceHead["ProjectID"] != DBNull.Value)
            {
                try
                {
                    ddlProject.SelectedValue = rsInvoiceHead["ProjectID"].ToString();

                }
                catch { }
            }

            if (rsInvoiceHead["NominalCodeID"] != DBNull.Value)
            {
                try
                {
                    ddlNominalCode.SelectedValue = rsInvoiceHead["NominalCodeID"].ToString();

                }
                catch { }
            }

            if (rsInvoiceHead["DepartmentID"] != DBNull.Value)
            {
                try
                {
                    ddlDepartment.SelectedValue = rsInvoiceHead["DepartmentID"].ToString();

                }
                catch { }
            }


            if (rsInvoiceHead["New_CodingDescriptionID"] != DBNull.Value)
            {
                try
                {
                    ddlCodingDescription.SelectedValue = rsInvoiceHead["New_CodingDescriptionID"].ToString();
                }
                catch { }
            }


            if ((int)Session["ApproveForm"] == 1)
            {
                cboStatus.Enabled = true;
            }
            else
            {
                cboUsers.Enabled = true;
            }

            if (rsInvoiceHead["Comment"] != DBNull.Value)
                if (rsInvoiceHead["Comment"].ToString().Trim() != "")
                    txtComment.Text = rsInvoiceHead["Comment"].ToString();
            oldComment = txtComment.Text;
            ViewState["oldComment"] = txtComment.Text;


            if (rsInvoiceHead["PassedToUserID"] != DBNull.Value)
                SetPassedToUserIDOnDropDown(Convert.ToInt32(rsInvoiceHead["PassedToUserID"]));


            if (statusID == 4)
                DisableUserDropDownWhenApproved2();

            txtComment.Text = "";

            DisableUserDropDown();

            RemoveStatusBasedOnUserTypeID();
        }
        #endregion
        // =========================================================================================================
        #region SetStatus
        private int SetStatus(int statusID)
        {
            int stid = 0;
            switch (statusID)
            {
                case 1: //unapproved
                    if (bExceedLimit)
                    {
                        stid = 11;
                    }
                    else
                    {
                        stid = 9;
                    }
                    break;
                case 3: //approved1
                    stid = 10;
                    break;
                case 9: // for approval 1
                    stid = 3;
                    break;
                case 10: // for approval 2
                    stid = 4;  //approved2
                    break;
                case 2: //on hold
                    stid = 2;
                    break;
                case 8: //under query
                    stid = 8;
                    break;
                case 4: // approved2
                    stid = 4;  //approved2
                    break;
            }
            return stid;
        }
        #endregion
        // =========================================================================================================
        #region btnCofirmInvoice_Click
        private void btnCofirmInvoice_Click(object sender, System.EventArgs e)
        {
            Invoice_CN invoice = new Invoice_CN();
            //save the invoice head and detail data in a single transaction context
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            da.BeginTransaction();

            int invoicePKID = 0;

            rsInvoiceHead["SupplierCompanyID"] = (int)Session["CompanyID"];


            if (rsInvoiceHead["statusID"] == DBNull.Value)
                rsInvoiceHead["statusID"] = 20;    //Pending

            invoicePKID = invoice.InsertInvoiceHeadData(rsInvoiceHead, da);
            if (da.SPReturnValue == 2)
            {
                Session.Add("DuplicateInvoice", "1");
                rsInvoiceHead["statusID"] = null;
                da.RollbackTransaction();
                Response.Redirect("InvoiceOtherInfo.aspx");
            }
            //save invoice detail data
            if (invoicePKID > 0)
                invoice.InsertInvoiceDetailData(invoicePKID, rsInvoiceDetail, da);
            //if error rollback
            if (da.ErrorCode != DataAccessErrors.Successful)
            {
                da.RollbackTransaction();
                rsInvoiceHead["statusID"] = null;
                Response.Write(da.ErrorMessage);
            }
            else //else commit transaction
            {
                da.CommitTransaction();
                Session["InvoiceID"] = invoicePKID;
                Response.Redirect("InvoicePassToUser.aspx", true);
            }
        }
        #endregion
        // =========================================================================================================
        #region btnBack_Click
        private void btnBack_Click(object sender, System.EventArgs e)
        {

            if (cboStatus.SelectedItem.Text.ToString().ToUpper().IndexOf("APPROVED", 0) > 0)
                Session["ApproveForm"] = 0;
            else
                Session["ApproveForm"] = 1;

            if ((int)Session["ApproveForm"] == 1)
                Response.Redirect("ApproveSalesInvoice.aspx");
            else
                Response.Redirect("PassInvoice.aspx");
        }
        #endregion
        // =========================================================================================================
        #region btnSubmit_Click
        public void btnSubmit_Click(object sender, System.EventArgs e)
        {
            OutError.Visible = false;
            if (ddlDepartment.SelectedIndex == 0 && (cboStatus.SelectedValue.Equals("4") || cboStatus.SelectedValue.Equals("18")))
            {
                OutError.Text = "Please select a department.";
                OutError.Visible = true;
                return;
            }

            if (ddlNominalCode.SelectedIndex == 0 && (cboStatus.SelectedValue.Equals("4") || cboStatus.SelectedValue.Equals("18")))
            {
                OutError.Text = "Please select a nominal code.";
                OutError.Visible = true;
                return;
            }

            #region VALIDATION FOR CODING DESCRIPTION
            if (ddlCodingDescription.SelectedIndex == 0 && (cboStatus.SelectedValue.Equals("4") || cboStatus.SelectedValue.Equals("17")))
            {
                OutError.Text = "Please select a coding description.";
                OutError.Visible = true;
                return;
            }
            #endregion

            if (txtDescription.Text.Trim() == "")
            {
                OutError.Text = "Please enter description.";
                OutError.Visible = true;
                return;
            }
            Invoice_CN invoice = new Invoice_CN();
            //save the invoice head and detail data in a single transaction context
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);


            int invoicePKID = invoiceID;

            rsInvoiceHead = Invoice_CN.GetInvoiceHead(invoiceID);

            if (rsInvoiceHead["PassedToUserID"] != DBNull.Value)
            {
                if (Convert.ToInt32(cboStatus.SelectedValue) == 3 && Convert.ToInt32(cboUsers.SelectedValue) == Convert.ToInt32(rsInvoiceHead["PassedToUserID"]) && Convert.ToInt32(Session["UserTypeID"]) <= 3)
                {
                    OutError.Text = "Sorry, you are not authorised to pass a credit note to yourself for 2nd approval.";
                    OutError.Visible = true;
                    return;
                }
            }

            if (Convert.ToInt32(cboStatus.SelectedValue) == 6)
            {
                if (Convert.ToInt32(cboStatus.SelectedValue) == 6 && txtComment.Text.Trim() != "")
                {
                    if (objCompany.UpdateRejectStatus(Convert.ToInt32(rsInvoiceHead["CreditNoteID"]), Convert.ToInt32(cboStatus.SelectedValue), "C"))
                    {
                        ShowMessage("Status updated successfully.");
                        hdWindowFlag.Value = "1";
                        objInvoice.UpdateInvoiceStatusLogApproverWise_CN(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"]), 0, Convert.ToInt32(Session["UserTypeID"]), Convert.ToInt32(cboStatus.SelectedValue), txtComment.Text.Trim());
                        objInvoice.UpdateCodes_CN(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(ddlDepartment.SelectedValue), Convert.ToInt32(ddlProject.SelectedValue), Convert.ToInt32(ddlNominalCode.SelectedValue), txtDescription.Text.Trim());
                        return;
                    }
                }
                else
                {
                    ShowMessage("Please add comments for the current action.");
                    return;
                }
            }

            if (Convert.ToInt32(cboStatus.SelectedValue) == 18)
            {
                if (Convert.ToInt32(Session["UserTypeID"]) > 1 && Convert.ToInt32(rsInvoiceHead["StatusID"]) == 4 && Convert.ToInt32(cboStatus.SelectedValue) == 18)
                {
                    if (objInvoice.UpdateCreditNoteStatusAsCredited(Convert.ToInt32(rsInvoiceHead["CreditNoteID"]), Convert.ToInt32(cboStatus.SelectedValue)))
                    {
                        ShowMessage("Status updated successfully.");
                        hdWindowFlag.Value = "1";
                        objInvoice.UpdateInvoiceStatusLogApproverWise_CN(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"]), 0, Convert.ToInt32(Session["UserTypeID"]), Convert.ToInt32(cboStatus.SelectedValue), txtComment.Text.Trim());
                        objInvoice.UpdateCodes_CN(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(ddlDepartment.SelectedValue), Convert.ToInt32(ddlProject.SelectedValue), Convert.ToInt32(ddlNominalCode.SelectedValue), txtDescription.Text.Trim());
                        return;
                    }
                }
                else
                {
                    ShowMessage("Sorry, this credit note cannot be credited because it has not yet completed approval 2.");
                    return;
                }
            }

            if ((Convert.ToInt32(rsInvoiceHead["StatusID"]) == 1 || Convert.ToInt32(rsInvoiceHead["StatusID"]) == 5) && cboUsers.SelectedItem.Text.IndexOf("StandBy") != -1)
            {
                OutError.Text = "Sorry, cannot pass credit note to a stanby at this stage.";
                OutError.Visible = true;
                return;
            }

            if (Convert.ToInt32(Session["UserTypeID"]) == 2 && (cboStatus.SelectedValue.Equals("2") || cboStatus.SelectedValue.Equals("8")))
            {
                OutError.Text = "Sorry, you are not authorized to put an invoice, under query or on hold.";
                OutError.Visible = true;
                return;
            }

            if ((Convert.ToInt32(rsInvoiceHead["StatusID"]) == 8 || Convert.ToInt32(rsInvoiceHead["StatusID"]) == 14 || Convert.ToInt32(rsInvoiceHead["StatusID"]) == 16) && (cboStatus.SelectedValue.Equals("8") || cboStatus.SelectedValue.Equals("14") || cboStatus.SelectedValue.Equals("16")))
            {
                OutError.Text = "Sorry, cannot put the invoice under query again.";
                OutError.Visible = true;
                return;
            }

            if (txtComment.Text.Equals(ViewState["oldComment"].ToString()) && (cboStatus.SelectedValue.Equals("2") || cboStatus.SelectedValue.Equals("6") || cboStatus.SelectedValue.Equals("8") || cboStatus.SelectedValue.Equals("13") || cboStatus.SelectedValue.Equals("14") || cboStatus.SelectedValue.Equals("15") || cboStatus.SelectedValue.Equals("16")
                || (cboStatus.SelectedValue.Equals("9") && Convert.ToInt32(rsInvoiceHead["StatusID"]) == 8)
                || (cboStatus.SelectedValue.Equals("8") && Convert.ToInt32(rsInvoiceHead["StatusID"]) == 9))) // Convert.ToInt32(rsInvoiceHead["PreviousStatuID"])==1))  //changed 010905 SURAJIT
            {
                OutError.Text = "Please add comments for the current action.";
                OutError.Visible = true;
                return;
            }

            if (rsInvoiceHead["Comment"] != DBNull.Value)
            {
                if ((Convert.ToInt32(rsInvoiceHead["StatusID"]) == 8 || Convert.ToInt32(rsInvoiceHead["StatusID"]) == 14 || Convert.ToInt32(rsInvoiceHead["StatusID"]) == 16) && (txtComment.Text.Trim().Equals("")))
                {
                    OutError.Text = "Please add comments for the current action.";
                    OutError.Visible = true;
                    return;
                }
            }

            if (Page.IsValid)
            {
                string strCurrentStatusID = "";
                strCurrentStatusID = objInvoice.GetCurrentStatus_CN(Convert.ToInt32(ViewState["InvoiceID"]));

                if (bExceedLimit && (cboStatus.SelectedValue != "11" || cboStatus.SelectedValue != "5"))
                {
                    OutError.Text = "This invoice exceeds threshold amount. Please select any of the director's option from the list.";
                    OutError.Visible = true;
                    return;
                }

                if (cboUsers.SelectedIndex == -1 && !chkCeo.Checked)
                {
                    OutError.Text = "Please select a user";
                    OutError.Visible = true;
                    return;
                }

                OutError.Visible = false;

                //validate date
                if (CheckDate(System.Convert.ToInt32(cboYearDate.SelectedValue),
                    System.Convert.ToInt32(cboMonthDate.SelectedValue),
                    System.Convert.ToInt32(cboDayDate.SelectedValue)) == false)
                {
                    OutError.Visible = true;
                    OutError.Text = "Enter valid date";
                    return;
                }

                OutError.Visible = false;

                DateTime dt = new DateTime(Convert.ToInt32(cboYearDate.SelectedValue), Convert.ToInt32(cboMonthDate.SelectedValue), Convert.ToInt32(cboDayDate.SelectedValue));


                if (cboStatus.SelectedValue == "8" || cboStatus.SelectedValue == "2" || cboStatus.SelectedValue == "13" || cboStatus.SelectedValue == "14")
                {
                    iPreviousStatusID = Convert.ToInt32(rsInvoiceHead["statusID"]);

                    if (rsInvoiceHead["PassedToUserID"] != DBNull.Value)
                        iPreviousPassedToUserID = Convert.ToInt32(rsInvoiceHead["PassedToUserID"]);
                    else
                        iPreviousPassedToUserID = 0;
                }

                if (cboStatus.SelectedIndex != -1)
                    rsInvoiceHead["statusID"] = cboStatus.SelectedValue;
                else
                    rsInvoiceHead["statusID"] = 1;


                if (cboStatus.SelectedItem.Text.ToString().ToUpper().IndexOf("APPROVED", 0) > 0)
                    Session["ApproveForm"] = 0;
                else
                    Session["ApproveForm"] = 1;


                if ((int)Session["ApproveForm"] == 1)
                    rsInvoiceHead["ApproveDate"] = dt;
                else
                    rsInvoiceHead["PassDate"] = dt;

                if (cboUsers.SelectedIndex != -1)
                    rsInvoiceHead["PassedToUserID"] = cboUsers.SelectedValue;
                else
                    rsInvoiceHead["PassedToUserID"] = DBNull.Value;


                int stsId = 0;
                stsId = Convert.ToInt32(cboStatus.SelectedValue);

                if ((stsId == 2 || stsId == 6 || stsId == 8 || stsId == 7 || stsId == 13 || stsId == 14) && txtComment.Text.Trim() == "")
                {
                    OutError.Text = "Please enter comments.";
                    OutError.Visible = true;
                    return;
                }
                rsInvoiceHead["DepartmentID"] = Convert.ToInt32(ddlDepartment.SelectedValue);
                rsInvoiceHead["ProjectID"] = Convert.ToInt32(ddlProject.SelectedValue);
                rsInvoiceHead["NominalCodeID"] = Convert.ToInt32(ddlNominalCode.SelectedValue);
                rsInvoiceHead["New_CodingDescriptionID"] = Convert.ToInt32(ddlCodingDescription.SelectedValue);
                if (cboStatus.SelectedValue != "8" && cboStatus.SelectedValue != "2" && cboStatus.SelectedValue != "13" && cboStatus.SelectedValue != "14")
                {
                    rsInvoiceHead["Approved1"] = 0;
                    rsInvoiceHead["Approved1SB1"] = 0;
                    rsInvoiceHead["Approved1SB2"] = 0;
                    rsInvoiceHead["Approved2_1"] = 0;
                    rsInvoiceHead["Approved2_2"] = 0;
                    rsInvoiceHead["Approved2SB1"] = 0;
                    rsInvoiceHead["Approved2SB2"] = 0;
                }

                if (stsId != 9)
                {
                    rsInvoiceHead["ApprovedCEO"] = 0;
                }

                if (cboStatus.SelectedValue != "8" && cboStatus.SelectedValue != "2" && cboStatus.SelectedValue != "13" && cboStatus.SelectedValue != "14")
                {
                    string[] strApproverType = cboUsers.SelectedItem.Text.Split('-');

                    if (strApproverType.Length > 1)
                    {
                        if (strApproverType[1].Trim() == "Approver 1")
                        {
                            rsInvoiceHead["Approved1"] = 1;
                        }

                        if (strApproverType[1].Trim() == "Approver 1 StandBy 1")
                        {
                            rsInvoiceHead["Approved1SB1"] = 1;
                        }

                        if (strApproverType[1].Trim() == "Approver 1 StandBy 2")
                        {
                            rsInvoiceHead["Approved1SB2"] = 1;
                        }

                        if (strApproverType[1].Trim() == "Approver 2 : 1")
                        {
                            rsInvoiceHead["Approved2_1"] = 1;
                        }

                        if (strApproverType[1].Trim() == "Approver 2 : 2")
                        {
                            rsInvoiceHead["Approved2_2"] = 1;
                        }

                        if (strApproverType[1].Trim() == "Approver 2 StandBy 1")
                        {
                            rsInvoiceHead["Approved2SB1"] = 1;
                        }

                        if (strApproverType[1].Trim() == "Approver 2 StandBy 2")
                        {
                            rsInvoiceHead["Approved2SB2"] = 1;
                        }

                        if (strApproverType[1].Trim() == " ")
                        {
                            rsInvoiceHead["ApprovedCEO"] = 1;
                        }
                    }
                }
                OutError.Visible = false;

                ViewState["PToUID"] = rsInvoiceHead["PassedToUserID"].ToString();

                int Ret1 = da.ExecuteNonQuery("up_Invoice_updStatus_CN", invoicePKID, CBSAppUtils.AppUserId, stsId, rsInvoiceHead["PassedToUserID"], txtComment.Text, rsInvoiceHead["PassDate"], rsInvoiceHead["ApproveDate"], rsInvoiceHead["DepartmentID"], rsInvoiceHead["ProjectID"], rsInvoiceHead["NominalCodeID"],
                    rsInvoiceHead["Approved1"], rsInvoiceHead["Approved1SB1"], rsInvoiceHead["Approved1SB2"],
                    rsInvoiceHead["Approved2_1"], rsInvoiceHead["Approved2_2"], rsInvoiceHead["Approved2SB1"],
                    rsInvoiceHead["Approved2SB2"], rsInvoiceHead["ApprovedCEO"], txtDescription.Text.Trim(), txtAssociatedInvoice.Text.Trim(), Convert.ToInt32(ddlCodingDescription.SelectedValue));

                SendMailInfo();

                if (da.SPReturnValue == 2)
                {
                    Session.Add("DuplicateInvoice", "1");
                }
                //save invoice detail data
                if (da.ErrorCode != DataAccessErrors.Successful)
                {
                    Response.Write(da.ErrorMessage);
                }
                else //else commit transaction
                {
                    hdWindowFlag.Value = "1";
                    OutError.Text = "Data submitted";
                    OutError.Visible = true;
                    UpdateChangedCompanyCodeForInvoice(Convert.ToInt32(ViewState["InvoiceID"]));

                    if (cboStatus.SelectedValue == "8" || cboStatus.SelectedValue == "2" || cboStatus.SelectedValue == "13" || cboStatus.SelectedValue == "14")
                    {
                        objInvoice.UpdatePreviousIDWhenUnderQuery_CN(Convert.ToInt32(ViewState["InvoiceID"]), iPreviousStatusID, iPreviousPassedToUserID);
                    }

                    if (cboStatus.SelectedValue != "8" || cboStatus.SelectedValue != "2")
                    {
                        if (objInvoice.UpdateInvoiceStatusLogApproverWise_CN(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(ViewState["PToUID"]), Convert.ToInt32(Session["UserTypeID"]), stsId, txtComment.Text.Trim()))
                        {
                            OutError.Visible = true;
                            OutError.Text = "Invoice log updated successfully.";
                        }
                    }
                }
            }
        }
        #endregion
        // =========================================================================================================
        #region CheckDate
        public bool CheckDate(int year, int month, int day)
        {
            bool retValue = true;
            try
            {
                DateTime dt = new DateTime(year, month, day);
            }
            catch
            {
                retValue = false;
            }
            return retValue;
        }
        #endregion
        // =========================================================================================================
        #region LoadData
        private void LoadData()
        {
            //populate date combos
            int currentYear = Microsoft.VisualBasic.DateAndTime.Year(System.DateTime.Now);
            for (int i = currentYear; i < (currentYear + 5); i++)
            {
                cboYearDate.Items.Add(i.ToString());
            }

            for (int i = 1; i < 13; i++)
            {
                cboMonthDate.Items.Add(new ListItem(
                    Microsoft.VisualBasic.DateAndTime.MonthName(i, true), i.ToString()));
            }

            for (int i = 1; i < 32; i++)
            {
                cboDayDate.Items.Add(i.ToString());
            }
            int iDay = 0;
            int iMonth = 0;
            int iYear = 0;

            iDay = DateTime.Today.Day;
            iMonth = DateTime.Today.Month;
            iYear = DateTime.Today.Year;

            cboDayDate.SelectedValue = iDay.ToString();
            cboMonthDate.SelectedValue = iMonth.ToString();
            cboYearDate.SelectedValue = iYear.ToString();
        }
        #endregion
        // =========================================================================================================		
        #region cboStatus_SelectedIndexChanged
        public void cboStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cboStatus.SelectedValue.Equals("8") || cboStatus.SelectedValue.Equals("14") || cboStatus.SelectedValue.Equals("16"))
            {
                cboUsers.Enabled = true;
                Invoice_CN objInvoice = new Invoice_CN();

                cboUsers.DataSource = objInvoice.GetUsersForCompany(Convert.ToInt32(Session["CompanyID"]));
                cboUsers.DataBind();
            }
            else if (cboStatus.SelectedValue.Equals("2") || cboStatus.SelectedValue.Equals("13") || cboStatus.SelectedValue.Equals("15"))
            {
                trUsers.Visible = false;
            }
            else
            {
                trUsers.Visible = true;
            }
        }
        #endregion
        // =========================================================================================================
        #region PopulateDropDownControls
        private void PopulateApproverDropDown()
        {
            DataTable dt = null;

            dt = objInvoice.GetDepartmentList();

            ddlDepartment.DataSource = dt;
            ddlDepartment.DataBind();

            dt = objInvoice.GetProjectList();

            ddlProject.DataSource = dt;
            ddlProject.DataBind();

            dt = objInvoice.GetNominalCodeList();

            ddlNominalCode.DataSource = dt;
            ddlNominalCode.DataBind();

            #region POPULATE CODING DESCRIPTION
            dt = oInvoice.GetCodingDescriptionList();
            ddlCodingDescription.DataSource = dt;
            ddlCodingDescription.DataBind();
            ddlCodingDescription.Items.Insert(0, new ListItem("Select", "0"));
            #endregion


            ddlDepartment.Items.Insert(0, new ListItem("Select", "0"));
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));
            ddlNominalCode.Items.Insert(0, new ListItem("Select", "0"));
        }
        #endregion
        // =========================================================================================================
        #region SetApprover
        private void SetApprover()
        {
            int statusID = 0;
            RecordSet rsUsers = null;
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);

            if (cboStatus.SelectedIndex != -1)
            {
                if (cboStatus.SelectedItem.Text.ToString().ToUpper().IndexOf("APPROVED", 0) > 0)
                    Session["ApproveForm"] = 0;
                else
                    Session["ApproveForm"] = 1;
            }
            //250505
            statusID = SetStatus(Convert.ToInt32(cboStatus.SelectedValue));
            rsUsers = da.ExecuteSP("up_GetPassingUsers", System.Convert.ToInt32(ViewState["BuyerCompanyID"]), statusID);

            cboUsers.Items.Clear();
            rsUsers.MoveFirst();
            string strHeadApprover = "";
            int iCountHeadApprover = 0;

            while (!rsUsers.EOF())
            {
                ListItem listItem = null;

                if (rsUsers["Name"].ToString().Trim().IndexOf("StandBy") != -1)
                {
                    listItem = new ListItem(rsUsers["Name"].ToString() + " - FOR - " + strHeadApprover, rsUsers["UserId"].ToString());
                }
                else
                {
                    iCountHeadApprover++;

                    if (iCountHeadApprover == 1)
                        strHeadApprover = strHeadApprover + " " + rsUsers["Name"].ToString().Trim().Substring(0, rsUsers["Name"].ToString().Trim().LastIndexOf("-"));
                    if (iCountHeadApprover == 2)
                        strHeadApprover = strHeadApprover + "/" + rsUsers["Name"].ToString().Trim().Substring(0, rsUsers["Name"].ToString().Trim().LastIndexOf("-"));
                    listItem = new ListItem(rsUsers["Name"].ToString(), rsUsers["UserId"].ToString());
                }
                cboUsers.Items.Add(listItem);
                rsUsers.MoveNext();
            }
            cboUsers.DataBind();
        }
        #endregion
        // ============================================================================================================
        #region GetApproverType
        private string GetApproverType(RecordSet rsInvoice)
        {
            string strApproverType = "";

            if (rsInvoice["Approved1"] != DBNull.Value)
                if (Convert.ToInt32(rsInvoice["Approved1"]) == 1)
                    strApproverType = "Approver 1";

            if (rsInvoice["Approved1SB1"] != DBNull.Value)
                if (Convert.ToInt32(rsInvoice["Approved1SB1"]) == 1)
                    strApproverType = "Approver 1 StandBy 1";

            if (rsInvoice["Approved1SB2"] != DBNull.Value)
                if (Convert.ToInt32(rsInvoice["Approved1SB2"]) == 1)
                    strApproverType = "Approver 1 StandBy 2";

            if (rsInvoice["Approved2_1"] != DBNull.Value)
                if (Convert.ToInt32(rsInvoice["Approved2_1"]) == 1)
                    strApproverType = "Approver 2 : 1";

            if (rsInvoice["Approved2_2"] != DBNull.Value)
                if (Convert.ToInt32(rsInvoice["Approved2_2"]) == 1)
                    strApproverType = "Approver 2 : 2";

            if (rsInvoice["Approved2SB1"] != DBNull.Value)
                if (Convert.ToInt32(rsInvoice["Approved2SB1"]) == 1)
                    strApproverType = "Approver 2 StandBy 1";

            if (rsInvoice["Approved2SB2"] != DBNull.Value)
                if (Convert.ToInt32(rsInvoice["Approved2SB2"]) == 1)
                    strApproverType = "Approver 2 StandBy 2";

            return (strApproverType);
        }
        #endregion
        // ============================================================================================================
        #region CheckGross
        private bool CheckGross(RecordSet rsInvoiceHead)
        {
            double gross = 0;
            bool bThresholdFlag = false;

            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rsGrs = da.ExecuteSP("up_Invoice_Gross", Convert.ToInt32(rsInvoiceHead["CreditNoteID"]));

            if (rsGrs != null)
            {
                if (rsGrs.RecordCount > 0)
                {
                    gross = Convert.ToDouble(rsGrs["Gross"]);
                    lblTotalAmount.Text = gross.ToString();
                }
            }
            dGrossAmount = objInvoice.GetThresholdAmount(Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]));
            if (dGrossAmount != 0)
            {
                if (gross > dGrossAmount)
                {
                    bThresholdFlag = true;
                }
            }
            return (bThresholdFlag);
        }
        #endregion
        // ============================================================================================================
        #region SendMailInfo
        private void SendMailInfo()
        {
            int iStatus = 0;
            iStatus = Convert.ToInt32(cboStatus.SelectedValue);

            if (iStatus == 9 || iStatus == 10 || iStatus == 11 || iStatus == 8 || iStatus == 14 || iStatus == 16)
            {
                string strMailBody = "Dear User,<BR><BR><BR><BR> " +
                    "An invoice/credit note requires your attention for approval or <BR> " +
                    "provision of comments. Please log on to www.p2dgroup.com with your <BR> " +
                    "Network ID, User Name and Password in order to action it. The action <BR> " +
                    "required of you will fall overdue if not completed within 48 hours of this e-mail.<BR><BR> " +

                    "If you have forgotten your password, please contact Andy Bray or Rob <BR> " +
                    "Kirkby in Finance.<BR><BR> " +

                    "If you have a query with the system please refer to your user guide or <BR> " +
                    "call the helpdesk on 01189 255550 or e-mail support@p2dgroup.com .<BR><BR> " +

                    "Many thanks,<BR><BR> " +

                    "Stuart Kilby<BR> " +
                    "Finance Director<BR> " +
                    "ETC<BR>";

                string strApproverType = "";
                string strUserName = "";
                string strEmail = "";
                string strInvoiceNo = "";
                int iUserID = 0;

                iUserID = Convert.ToInt32(cboUsers.SelectedValue);
                objInvoice.GetUserName(iUserID, out strUserName, out strEmail);
                strInvoiceNo = lblRefernce.Text.Trim();

                objInvoice.SendEmailInfo(strUserName, strEmail, strInvoiceNo, strApproverType, strMailBody);
            }
        }
        #endregion
        // ==========================================================================================================
        #region CustomValidator_FOR_Status_ServerValidate
        private void CustomValidator_FOR_Status_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (Convert.ToInt32(Session["UserTypeID"]) < 3 && cboStatus.SelectedValue == "7")
            {
                OutError.Text = "Sorry, you are not authorized to archive / delete invoice.";
                OutError.Visible = true;
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
        #endregion
        // ==========================================================================================================
        #region ValidateDropDownOptions
        private void ValidateDropDownOptions(int iStatusID)
        {
            string strApproverStatus = "";

            if (iStatusID == 6) // REJECTED
            {
                PopulateStatusDropDown("17,18", 0, iStatusID);
            }
            else if (Convert.ToInt32(Session["UserTypeID"]) == 2 && iStatusID == 9) // Forwarded for 1st Approval
            {
                PopulateStatusDropDown("2,8,6,7,9,17,18", 0, iStatusID);
            }
            else if (Convert.ToInt32(Session["UserTypeID"]) == 2 && iStatusID == 10) // Forwarded for 2nd Approval
            {
                PopulateStatusDropDown("13,14,6,7,10,17,18", 0, iStatusID);
            }

            else if (Convert.ToInt32(Session["UserTypeID"]) > 2 && iStatusID == 9) // Forwarded for 1st Approval 
            {
                if (Convert.ToInt32(ViewState["VPassedToUserID"]) != Convert.ToInt32(Session["UserID"]))
                    PopulateStatusDropDown("2,8,6,7,9,17,18", 0, iStatusID);
                else
                    PopulateStatusDropDown("2,8,6,7,3,17,18", 0, iStatusID);
            }
            else if (Convert.ToInt32(Session["UserTypeID"]) > 2 && iStatusID == 10) // Forwarded for 2nd Approval
            {
                if (Convert.ToInt32(ViewState["VPassedToUserID"]) != Convert.ToInt32(Session["UserID"]))
                    PopulateStatusDropDown("13,14,6,7,10,17,18", 0, iStatusID);
                else
                    PopulateStatusDropDown("13,14,6,7,4,17,18", 0, iStatusID);
            }

            else
            {
                if (iStatusID == 1) // Unapproved
                {
                    if (bExceedLimit)
                    {
                        PopulateStatusDropDown("15,16,6,7,17,18", 0, iStatusID);

                    }
                    else
                    {
                        PopulateStatusDropDown("9,2,8,6,7,17,18", 0, iStatusID);
                    }
                }
                else if (iStatusID == 3) // Approved 1
                {
                    PopulateStatusDropDown("10,2,8,6,7,17,18", 0, iStatusID);
                }
                else if (iStatusID == 4) // Approved 2
                {
                    //PopulateStatusDropDown("2,8,6,7",0,iStatusID);
                    PopulateStatusDropDown("13,14,6,7,17,18", 0, iStatusID);
                    lblApprovelMessage.Visible = true;
                }
                else if (iStatusID == 5) // Approved Director
                {
                    PopulateStatusDropDown("10,2,8,6,7,17,18", 0, iStatusID);
                }
                else if (iStatusID == 9) // Forwarded for 1st Approval
                {
                    PopulateStatusDropDown("3,2,8,6,7,17,18", 0, iStatusID);
                }
                else if (iStatusID == 10) // Forwarded for 2nd Approval
                {
                    PopulateStatusDropDown("4,13,14,6,7,17,18", 0, iStatusID);
                }
                else if (iStatusID == 11) // Forwarded for Director's Approval
                {
                    PopulateStatusDropDown("5,15,16,6,7,17,18", 0, iStatusID);
                }
                else if (iStatusID == 8) // Under Query before 1st approval.
                {
                    if (bExceedLimit)
                    {
                        PopulateStatusDropDown("5,2,8,6,7,17,18", 0, iStatusID);
                    }
                    else
                    {
                        strApproverStatus = GetApproverStatus(Convert.ToInt32(ViewState["InvoiceID"]));
                        PopulateStatusDropDown("2,8,6,7,9,17,18", 8, iStatusID);
                    }
                }
                else if (iStatusID == 16) // Under Query before director's approval.
                {
                    PopulateStatusDropDown("11,15,16,6,7,17,18", 0, iStatusID);
                }
                else if (iStatusID == 14) // Under Query before 2nd approval.
                {
                    strApproverStatus = GetApproverStatus(Convert.ToInt32(ViewState["InvoiceID"]));
                    PopulateStatusDropDown("13,14,6,7,10,17,18", 14, iStatusID);
                }
                else if (iStatusID == 2) // On Hold
                {
                    strApproverStatus = GetApproverStatus(Convert.ToInt32(ViewState["InvoiceID"]));
                    PopulateStatusDropDown("2,8,6,17,18," + strApproverStatus, 2, iStatusID);
                }
            }
        }
        #endregion
        // ==========================================================================================================
        #region PopulateStatusDropDown
        private void PopulateStatusDropDown(string strList, int iDefaultValueToSelect, int iCurrentStatusID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs1 = null;

            int iUserTypeID = 0;
            int iRsStatusID = 0;
            int iCounter = 0;

            if (iCurrentStatusID == 1 || iCurrentStatusID == 9)
                iCounter = 1;

            string[] strListArray = strList.Split(',');

            iUserTypeID = Convert.ToInt32(Session["UserTypeID"]);

            cboStatus.Items.Clear();
            rs1 = da.ExecuteQuery("InvCNStatus", "StatusID IN (" + strList + ")", "Status");
            rs1.MoveFirst();

            while (!rs1.EOF())
            {
                ListItem listItem = new ListItem(rs1["Status"].ToString(), rs1["statusID"].ToString());
                iRsStatusID = Convert.ToInt32(rs1["statusID"]);

                for (int counter = 0; counter < strListArray.Length; counter++)
                {
                    if (iRsStatusID == 7 && iUserTypeID > 2)
                    {
                        cboStatus.Items.Add(listItem);
                        break;
                    }

                    if (iRsStatusID == Convert.ToInt32(strListArray[counter].Trim()))
                    {
                        cboStatus.Items.Add(listItem);
                        break;
                    }

                    if (iCurrentStatusID == 1 && iCounter == 1)
                    {
                        iCounter++;

                        if (bExceedLimit == true)
                            PopulateStatusDropDown(11);
                    }

                    if (iCurrentStatusID == 9 && iCounter == 1)
                    {
                        iCounter++;

                        if (bExceedLimit == true && Convert.ToBoolean(rsInvoiceHead["ApprovedCEO"]) == false)
                            PopulateStatusDropDown(11);
                    }
                }

                rs1.MoveNext();
            }

            if (iDefaultValueToSelect != 0)
            {
                cboStatus.SelectedValue = iDefaultValueToSelect.ToString();
            }
        }
        #endregion
        // ==========================================================================================================
        #region PopulateStatusDropDown
        private void PopulateStatusDropDown(int iStatusID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs1 = null;

            int iRsStatusID = 0;
            rs1 = da.ExecuteQuery("InvCNStatus");
            rs1.MoveFirst();

            while (!rs1.EOF())
            {
                ListItem listItem = new ListItem(rs1["Status"].ToString(), rs1["statusID"].ToString());
                iRsStatusID = Convert.ToInt32(rs1["statusID"]);

                if (iRsStatusID == iStatusID)
                {
                    cboStatus.Items.Add(listItem);
                    break;
                }

                rs1.MoveNext();
            }

            //cboStatus.DataBind();
        }
        #endregion
        // ==========================================================================================================
        #region GetApproverStatus
        private string GetApproverStatus(int iInvoiceID)
        {
            string strApproverStatus = "";

            strApproverStatus = Invoice_CN.GetApproverStatus(iInvoiceID);

            if (strApproverStatus == "Approver1")
                strApproverStatus = "3";

            else if (strApproverStatus == "Approver2")
                strApproverStatus = "4";

            else if (strApproverStatus == "CEO")
                strApproverStatus = "11";

            return (strApproverStatus);
        }
        #endregion
        // ==========================================================================================================
        #region SetSupplierName
        private void SetSupplierName(int iInvoiceID)
        {
            GetStatusID(iInvoiceID);
            string strCompanyCode = "";
            lblSupplier.Text = objInvoice.GetSupplierName(iInvoiceID);
            try
            {
                if (Session["ETC"].ToString() == "1")
                {
                    lblCompanyCode.Visible = true;
                    ddlBuyerCompanyCode.Visible = true;
                    if (iCounter == 1)
                    {
                        strCompanyCode = objInvoice.GetCompanyCodeForInvoice(iInvoiceID);
                        ViewState["ActualCompanyCode"] = strCompanyCode;
                        ddlBuyerCompanyCode.Items.Insert(0, new ListItem(strCompanyCode, strCompanyCode));
                        ddlBuyerCompanyCode.Items.Insert(1, new ListItem("08", "08"));
                    }
                    iCounter++;
                }
            }
            catch { }
        }
        #endregion
        // ==========================================================================================================
        #region UpdateChangedCompanyCodeForInvoice
        private void UpdateChangedCompanyCodeForInvoice(int iInvoiceID)
        {
            if (Session["ETC"].ToString() == "1")
            {
                UpdateDescriptionForInvoice(iInvoiceID);
                string strChangedCompanyCode = "";
                if (ddlBuyerCompanyCode.SelectedItem.Text.Trim() == "08")
                    strChangedCompanyCode = "08";

                objInvoice.UpdateChangedCompanyCodeForInvoice(iInvoiceID, strChangedCompanyCode);
            }
        }
        #endregion
        // ==========================================================================================================
        #region GetStatusID
        private void GetStatusID(int iInvoiceID)
        {
            if (Session["ETC"].ToString() == "1")
            {
                int iFirstStatusID = 0;
                objInvoice.GetStatusID(iInvoiceID, out iFirstStatusID);
                if (iFirstStatusID == 1)
                {
                    txtDescription.ReadOnly = false;
                    rfv_FOR_Description.Enabled = true;
                }
            }
        }
        #endregion
        // ==========================================================================================================
        #region UpdateDescriptionForInvoice
        private void UpdateDescriptionForInvoice(int iInvoiceID)
        {
            objInvoice.UpdateDescriptionForInvoice(iInvoiceID, txtDescription.Text.Trim());
        }
        #endregion
        // ==========================================================================================================
        #region RemoveStatusBasedOnUserTypeID
        private void RemoveStatusBasedOnUserTypeID()
        {
            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
            {
                cboStatus.Items.Remove(new ListItem("Credited", "18"));
                cboStatus.Items.Remove(new ListItem("Delete/Archive", "7"));
            }
            else if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                cboStatus.ClearSelection();

                cboStatus.Items.Remove(new ListItem("On Hold before 1st approval", "2"));
                cboStatus.Items.Remove(new ListItem("Under Query before 1st approval", "8"));

                cboStatus.Items.Remove(new ListItem("Under Query before 2nd approval", "14"));
                cboStatus.Items.Remove(new ListItem("On Hold before 2nd approval", "13"));

                cboStatus.Items.Remove(new ListItem("On Hold before Director's approval", "15"));
                cboStatus.Items.Remove(new ListItem("Under Query before Director's approval", "16"));

                cboStatus.Items.Remove(new ListItem("Delete/Archive", "7"));
            }
            else if (Convert.ToInt32(Session["UserTypeID"]) == 3)
            {
                cboStatus.ClearSelection();
                cboStatus.Items.Remove(new ListItem("Delete/Archive", "7"));
            }

            if (Convert.ToInt32(Session["UserTypeID"]) <= 1)
            {
                if (objInvoice.CheckApproverChainUserForCreditNote(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"])) == false)
                {
                    cboStatus.ClearSelection();

                    cboStatus.Items.Remove(new ListItem("Rejected", "6"));

                    cboStatus.Items.Remove(new ListItem("On Hold before 1st approval", "2"));
                    cboStatus.Items.Remove(new ListItem("Under Query before 1st approval", "8"));

                    cboStatus.Items.Remove(new ListItem("Under Query before 2nd approval", "14"));
                    cboStatus.Items.Remove(new ListItem("On Hold before 2nd approval", "13"));

                    cboStatus.Items.Remove(new ListItem("On Hold before Director's approval", "15"));
                    cboStatus.Items.Remove(new ListItem("Under Query before Director's approval", "16"));

                    cboStatus.Items.Remove(new ListItem("Delete/Archive", "7"));

                    cboStatus.Items.Remove(new ListItem("Debited", "17"));
                    cboStatus.Items.Remove(new ListItem("Credited", "18"));
                }
            }

            cboStatus.Items.Remove(new ListItem("Debited", "17"));
            cboStatus.Items.Remove(new ListItem("OverDue", "12"));

            if (Convert.ToInt32(Session["UserTypeID"]) != 2)
            {
                if (ViewState["StatusIDForHoldStatus"] != null)
                {
                    if (Convert.ToInt32(ViewState["StatusIDForHoldStatus"]) == 2 || Convert.ToInt32(ViewState["StatusIDForHoldStatus"]) == 8)
                    {
                        cboStatus.Items.Remove(new ListItem("On Hold before 1st approval", "2"));
                        cboStatus.Items.Remove(new ListItem("Under Query before 1st approval", "8"));
                        cboStatus.Items.Remove(new ListItem("Rejected", "6"));
                        cboStatus.Items.Remove(new ListItem("Credited", "18"));
                        cboStatus.Items.Remove(new ListItem("Delete/Archive", "7"));
                    }
                    else if (Convert.ToInt32(ViewState["StatusIDForHoldStatus"]) == 13 || Convert.ToInt32(ViewState["StatusIDForHoldStatus"]) == 14)
                    {
                        cboStatus.Items.Remove(new ListItem("Under Query before 2nd approval", "14"));
                        cboStatus.Items.Remove(new ListItem("On Hold before 2nd approval", "13"));
                        cboStatus.Items.Remove(new ListItem("Rejected", "6"));
                        cboStatus.Items.Remove(new ListItem("Credited", "18"));
                        cboStatus.Items.Remove(new ListItem("Delete/Archive", "7"));
                    }
                    else if (Convert.ToInt32(ViewState["StatusIDForHoldStatus"]) == 15 || Convert.ToInt32(ViewState["StatusIDForHoldStatus"]) == 16)
                    {
                        cboStatus.Items.Remove(new ListItem("On Hold before Director's approval", "15"));
                        cboStatus.Items.Remove(new ListItem("Under Query before Director's approval", "16"));
                        cboStatus.Items.Remove(new ListItem("Rejected", "6"));
                        cboStatus.Items.Remove(new ListItem("Delete/Archive", "7"));
                        cboStatus.Items.Remove(new ListItem("Credited", "18"));

                    }

                    try
                    {
                        cboStatus.Items.Remove(new ListItem("Debited", "17"));

                    }
                    catch { }
                }
            }

            if (ViewState["StatusIDForHoldStatus"] != null)
            {
                if (Convert.ToInt32(ViewState["StatusIDForHoldStatus"]) == 4)
                {
                    try
                    {
                        cboStatus.Items.Remove(new ListItem("On Hold before 1st approval", "2"));
                        cboStatus.Items.Remove(new ListItem("Under Query before 1st approval", "8"));

                        cboStatus.Items.Remove(new ListItem("Under Query before 2nd approval", "14"));
                        cboStatus.Items.Remove(new ListItem("On Hold before 2nd approval", "13"));

                        cboStatus.Items.Remove(new ListItem("On Hold before Director's approval", "15"));
                        cboStatus.Items.Remove(new ListItem("Under Query before Director's approval", "16"));

                        cboStatus.Items.Remove(new ListItem("Rejected", "6"));
                        cboStatus.Items.Remove(new ListItem("Delete/Archive", "7"));

                    }
                    catch { }
                }
            }
        }
        #endregion
        // ==========================================================================================================
        #region SetPassedToUserIDOnDropDown
        private void SetPassedToUserIDOnDropDown(int iUserID)
        {
            try
            {
                cboUsers.SelectedValue = iUserID.ToString();

            }
            catch { }
        }
        #endregion
        // ==========================================================================================================
        #region DisableUserDropDownWhenApproved2
        private void DisableUserDropDownWhenApproved2()
        {
            cboUsers.Enabled = false;
        }
        #endregion
        // ==========================================================================================================
        #region DisableUserDropDown
        private void DisableUserDropDown()
        {
            for (int i = 0; i < cboStatus.Items.Count; i++)
            {
                if (cboStatus.Items[i].Value == "4")
                {
                    cboUsers.Enabled = false;
                    break;
                }
            }
        }
        #endregion
        // ==========================================================================================================
        #region ShowMessage
        private void ShowMessage(string strMessage)
        {
            OutError.Visible = true;
            OutError.Text = strMessage;
        }
        #endregion
        // ==========================================================================================================
        #region RemoveStatusForManagementUsers
        private void RemoveStatusForManagementUsers()
        {
            try
            {
                cboStatus.ClearSelection();

            }
            catch { }

            try
            {
                cboStatus.Items.Remove(new ListItem("On Hold before 1st approval", "2"));
                cboStatus.Items.Remove(new ListItem("Under Query before 1st approval", "8"));

            }
            catch { }

            try
            {
                cboStatus.Items.Remove(new ListItem("Under Query before 2nd approval", "14"));
                cboStatus.Items.Remove(new ListItem("On Hold before 2nd approval", "13"));

            }
            catch { }

            try
            {
                cboStatus.Items.Remove(new ListItem("On Hold before Director's approval", "15"));
                cboStatus.Items.Remove(new ListItem("Under Query before Director's approval", "16"));

            }
            catch { }
        }
        #endregion
        // ==========================================================================================================
        #region ddlCodingDescription_SelectedIndexChanged
        public void ddlCodingDescription_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string strNominalCodeID = "";
            string strDepartmentCodeID = "";
            int iCodingDescription = 0;

            if (Convert.ToInt32(ddlCodingDescription.SelectedValue.Trim()) != 0)
            {
                iCodingDescription = Convert.ToInt32(ddlCodingDescription.SelectedValue.Trim());
                objCompany.GetNomDepCodeAgainstCodingDescriptionID(iCodingDescription, out strNominalCodeID, out strDepartmentCodeID);
                try
                {
                    ddlDepartment.SelectedValue = strDepartmentCodeID.Trim();
                }
                catch { }
                try
                {
                    ddlNominalCode.SelectedValue = strNominalCodeID.Trim();
                }
                catch { }
            }
        }
        #endregion
        // ==========================================================================================================
        #region ddlDepartment_SelectedIndexChanged
        public void ddlDepartment_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int iNominalCodeID = 0;
            int iDepartmentCodeID = 0;
            string strCodingDescription = "";

            if (Convert.ToInt32(ddlDepartment.SelectedValue.Trim()) != 0)
            {
                iDepartmentCodeID = Convert.ToInt32(ddlDepartment.SelectedValue.Trim());

                if (Convert.ToInt32(ddlNominalCode.SelectedValue.Trim()) != 0)
                {
                    iNominalCodeID = Convert.ToInt32(ddlNominalCode.SelectedValue.Trim());
                }
                strCodingDescription = objCompany.GetCodingDescriptionAgainstNomDepCode(iNominalCodeID, iDepartmentCodeID);
                try
                {
                    ddlCodingDescription.SelectedValue = strCodingDescription.Trim();
                }
                catch { }
            }
        }
        #endregion
        // ==========================================================================================================
        #region ddlNominalCode_SelectedIndexChanged
        private void ddlNominalCode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int iNominalCodeID = 0;
            int iDepartmentCodeID = 0;
            string strCodingDescription = "";

            if (Convert.ToInt32(ddlNominalCode.SelectedValue.Trim()) != 0)
            {
                iNominalCodeID = Convert.ToInt32(ddlNominalCode.SelectedValue.Trim());
                if (Convert.ToInt32(ddlDepartment.SelectedValue.Trim()) != 0)
                {
                    iDepartmentCodeID = Convert.ToInt32(ddlDepartment.SelectedValue.Trim());
                }
                strCodingDescription = objCompany.GetCodingDescriptionAgainstNomDepCode(iNominalCodeID, iDepartmentCodeID);
                try
                {
                    ddlCodingDescription.SelectedValue = strCodingDescription.Trim();
                }
                catch { }
            }
        }
        #endregion

    }
}
