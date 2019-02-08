namespace CBSolutions.ETH.Web
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Configuration;
    using CBSolutions.Architecture.Core;
    using CBSolutions.Architecture.Data;

    /// <summary>
    ///		Summary description for menuUser.
    /// </summary>
    public class menuUser : System.Web.UI.UserControl
    {
        #region Web Form Controls
        protected System.Web.UI.WebControls.HyperLink hypCreateInvoice;
        protected System.Web.UI.WebControls.HyperLink hypSalesInvoiceHistory;
        protected System.Web.UI.WebControls.HyperLink hypCompanyManagement;
        protected System.Web.UI.WebControls.HyperLink hypUserManagement;
        protected System.Web.UI.WebControls.HyperLink hypBranchManagement;
        protected System.Web.UI.WebControls.HyperLink hypSupplierRelation;
        protected System.Web.UI.WebControls.HyperLink hypCustomerRelation;
        protected System.Web.UI.WebControls.HyperLink hypDownloadDatabase;
        protected System.Web.UI.WebControls.HyperLink hypUserTradingRelation;
        protected System.Web.UI.WebControls.HyperLink Hyperlink2;
        protected System.Web.UI.WebControls.HyperLink hypApproveInv;
        protected System.Web.UI.WebControls.HyperLink Hyperlink1;
        protected System.Web.UI.WebControls.HyperLink Hyperlink3;
        protected System.Web.UI.WebControls.HyperLink hypThreshold;
        protected System.Web.UI.WebControls.HyperLink Hyperlink5;
        protected System.Web.UI.WebControls.HyperLink Hyperlink6;
        protected System.Web.UI.WebControls.HyperLink hypPurchaseInvoiceHistory;
        protected System.Web.UI.WebControls.HyperLink hypRelationshipManager;
        protected System.Web.UI.WebControls.HyperLink hypSalesPerson;
        protected System.Web.UI.WebControls.HyperLink hypPassInv;
        protected System.Web.UI.WebControls.HyperLink hypCreateCreditNotesHistory;
        protected System.Web.UI.WebControls.HyperLink hypCreateCreditNotes;
        protected System.Web.UI.WebControls.HyperLink Hyperlink4;
        protected System.Web.UI.WebControls.HyperLink Hyperlink7;
        protected System.Web.UI.WebControls.HyperLink Hyperlink8;
        protected System.Web.UI.WebControls.HyperLink hypCreditNotesCurrent;
        protected System.Web.UI.WebControls.HyperLink hypCreditNotesHistory;
        protected System.Web.UI.WebControls.HyperLink hypChangePassword;
        protected System.Web.UI.WebControls.HyperLink hypExport;
        protected System.Web.UI.WebControls.HyperLink hypImport;
        #endregion
        #region User Defined Variables
        private int iUserID = 0;
        protected System.Web.UI.WebControls.HyperLink HyperLink9;
        protected System.Web.UI.WebControls.HyperLink hypDebitNote;
        protected System.Web.UI.WebControls.HyperLink hypP2DInvoice;
        protected System.Web.UI.WebControls.HyperLink hypSalesOrders;
        protected System.Web.UI.WebControls.HyperLink hypDownloadData;

        private int iSupplierCompanyID = 0;
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateOverDueStatus();
            }
            string s;
            string sa = Convert.ToString(Session["Access"]);
            if (Session["Access"] == null)
            {
                Response.Write("Null");
                Response.End();
            }
            RecordSet rsAccess = (RecordSet)Session["Access"];
            /*
             * we need to enable disable menus depending on user access rights
             * as there could be 3 access states, this is how we will visually map them:
             *   0 (Hidden)		= menu disabled
             *   1 (View only)	= menu enabled
             *   2 (Active)		= menu enabled
            */

            rsAccess.Filter = "";
            rsAccess.MoveFirst();
            while (!rsAccess.EOF())
            {
                SecurityAccessTypes access = (SecurityAccessTypes)rsAccess["AccessTypeID"];
                string menuControl = (string)rsAccess["MenuControl"];
                foreach (System.Web.UI.Control ctl in this.Controls)
                {
                    if (ctl.GetType().Name == "HyperLink")
                    {

                        HyperLink hyp = (HyperLink)ctl;
                        s = hyp.ImageUrl;

                        if (hyp.ID == menuControl)
                        {

                            hyp.Visible = (access != SecurityAccessTypes.Hidden);
                            if (!hyp.Visible)
                            {
                                s = hyp.ImageUrl;
                                s = s.Replace("E.jpg", "D.jpg");
                                hyp.ImageUrl = s;
                            }
                        }
                    }
                }
                if (Session["GMG"] != null)
                {
                    if ((int)Session["GMG"] == 0)
                    {
                        hypPassInv.Visible = false;
                    }
                }
                if ((int)Session["UserTypeID"] == 1 || (int)Session["UserTypeID"] == 2)
                {
                    //hypSupplierRelation.Visible=false;
                }
                else if ((int)Session["UserTypeID"] > 2)
                {
                    //hypSupplierRelation.Visible=true;
                }
                if (Convert.ToInt32(Session["UserTypeID"]) < 3)
                {
                    hypCompanyManagement.Visible = false;
                }
                else
                {
                    hypCompanyManagement.Visible = true;
                }

                if ((int)Session["CompanyTypeID"] == 1 || (int)Session["CompanyTypeID"] == 0)
                {
                    hypCreateInvoice.Visible = false;
                    hypSalesInvoiceHistory.Visible = false;
                    hypPurchaseInvoiceHistory.Visible = true;
                    hypCreateCreditNotes.Visible = false;
                    hypCreateCreditNotesHistory.Visible = false;
                    if (Convert.ToInt32(Session["UserTypeID"]) > 2)
                        hypExport.Visible = true;
                    else
                    {
                        hypExport.Visible = false;
                        if (Convert.ToInt32(Session["UserTypeID"]) > 1)
                            hypImport.Visible = true;
                        else
                            hypImport.Visible = false;
                    }
                }

                else
                {
                    hypCreateInvoice.Visible = true;
                    hypSalesInvoiceHistory.Visible = true;
                    hypPurchaseInvoiceHistory.Visible = false;
                    hypCreateCreditNotes.Visible = true;
                    hypCreateCreditNotesHistory.Visible = true;
                    hypExport.Visible = false;
                    hypImport.Visible = false;
                    hypP2DInvoice.Visible = true;
                }
                hypBranchManagement.Visible = true;
                Console.WriteLine(hypCompanyManagement.Enabled.ToString());
                rsAccess.MoveNext();
            }

            #region SWITCH CASE.
            switch ((string)Session["SelectedPage"])
            {
                case "CompanyBrowse":
                    s = hypCompanyManagement.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    if (hypCompanyManagement.Visible)
                        s = s.Replace("D.jpg", "S.jpg");
                    hypCompanyManagement.ImageUrl = s;
                    break;
                case "UserBrowse":
                    s = Hyperlink4.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    Hyperlink4.ImageUrl = s;
                    break;
                case "BranchManagement":
                    s = Hyperlink3.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    Hyperlink3.ImageUrl = s;
                    break;
                case "TradingRelation":
                    s = hypSupplierRelation.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    hypSupplierRelation.ImageUrl = s;
                    break;
                case "CreateInvoice":
                    s = hypCreateInvoice.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    hypCreateInvoice.ImageUrl = s;
                    break;
                case "PurchaseInvoiceLog":
                    s = hypPurchaseInvoiceHistory.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    hypPurchaseInvoiceHistory.ImageUrl = s;
                    break;
                case "SalesInvoiceLog":
                    s = hypSalesInvoiceHistory.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    hypSalesInvoiceHistory.ImageUrl = s;
                    break;
                case "UserTradingRelation":
                    s = hypSupplierRelation.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    hypUserTradingRelation.ImageUrl = s;
                    break;
                case "SalesOrders":
                    //s = hypSalesOrders.ImageUrl;
                    //s = s.Replace(".jpg", "S.gif");
                    //hypSalesOrders.ImageUrl = s;
                    break;
                case "downloadDB":
                    //s = hypDownloadData.ImageUrl;
                    //s = s.Replace(".gif", "S.gif");
                    //hypDownloadData.ImageUrl = s;
                    break;
                case "CreateCreditNotes":
                    //s = hypCreateCreditNotes.ImageUrl;
                    //s = s.Replace(".gif", "S.gif");
                    //hypCreateCreditNotes.ImageUrl = s;
                    break;
                case "CreditNotesLog":
                    s = hypCreateCreditNotesHistory.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    s = s.Replace("log.gif", "logS.gif");
                    hypCreateCreditNotesHistory.ImageUrl = s;
                    break;
                case "DebitNoteLog":
                    s = hypDebitNote.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    s = s.Replace("log.gif", "logS.gif");
                    hypDebitNote.ImageUrl = s;
                    break;
                case "ChangePassword":
                    s = hypChangePassword.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    s = s.Replace("password.gif", "passwordS.gif");
                    hypChangePassword.ImageUrl = s;
                    break;
                case "P2DInvoices":
                    s = hypP2DInvoice.ImageUrl;
                    s = s.Replace("E.jpg", "S.jpg");
                    s = s.Replace("Invoice.jpg", "InvoiceS.gif");
                    hypP2DInvoice.ImageUrl = s;
                    break;
            }
            #endregion SWITCH CASE.

            hypCustomerRelation.Visible = false;
            hypDownloadDatabase.Visible = true;
            hypDownloadDatabase.Enabled = true;
            hypUserTradingRelation.Visible = true;
            hypUserTradingRelation.Enabled = true;
            hypP2DInvoice.Visible = true;
            if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                hypPurchaseInvoiceHistory.Visible = false;

            Company objCompany = new Company();


            #region CHECK IF A SUPPLIER LOGIN THEN IT SHOULD BE A SUPPLIER FOR GMG OTHERWISE THE TRADING RELATION HYPERLINK WILL NOT BE VISIBLE
            iSupplierCompanyID = Convert.ToInt32(Session["CompanyID"]);

            if (Convert.ToInt32(Session["CompanyTypeID"]) == 2)
                if (objCompany.CheckGMGSuppier(iSupplierCompanyID))
                    hypSupplierRelation.Visible = true;
                else
                    hypSupplierRelation.Visible = false;
            #endregion CHECK IF A SUPPLIER LOGIN THEN IT SHOULD BE A SUPPLIER FOR GMG OTHERWISE THE TRADING RELATION HYPERLINK WILL NOT BE VISIBLE

            #region IF GMG USERS THEN DOWNLOAD DATABASE SHOULD NOT BE VISIBLE
            iUserID = Convert.ToInt32(Session["UserID"]);

            if (objCompany.CheckGMGUser(iUserID))
                hypDownloadDatabase.Visible = false;
            else
                hypDownloadDatabase.Visible = true;
            #endregion IF GMG USERS THEN DOWNLOAD DATABASE SHOULD NOT BE VISIBLE

            #region IF A SUPPLIER COMPANY LOGIN AND USERTYPEID > 2 (MEANS USER IS ADMINISTRATOR) THEN ONLY SHOW USER MANAGEMENT AND TRADING RELATION HYPERLINKS
            if (Convert.ToInt32(Session["UserTypeID"]) > 2)
            {
                hypSupplierRelation.Visible = false;
                hypUserManagement.Visible = true;
            }
            else
            {
                hypSupplierRelation.Visible = false;
                hypUserManagement.Visible = false;
            }
            #endregion IF A SUPPLIER COMPANY LOGIN AND USERTYPEID > 2 (MEANS USER IS ADMINISTRATOR) THEN ONLY SHOW USER MANAGEMENT AND TRADING RELATION HYPERLINKS

            #region COMPANY MANAGEMENT SHOULD ONLY BE VISIBLE TO THE HUB ADMINISTRATOR.
            if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
            {
                hypCompanyManagement.Visible = true;
                hypRelationshipManager.Visible = true;
                hypSalesPerson.Visible = true;
            }
            else
                hypCompanyManagement.Visible = false;
            #endregion COMPANY MANAGEMENT SHOULD ONLY BE VISIBLE TO THE HUB ADMINISTRATOR.

            #region	COMPANY MANAGEMENT SHOULD ONLY BE VISIBLE TO THE GMG ADMINISTRATOR.
            if (Session["GMG"] != null)
            {
                if ((int)Session["GMG"] == 1)
                {
                    if (Convert.ToInt32(Session["UserTypeID"]) > 2)
                    {
                        hypCompanyManagement.Visible = true;
                    }
                }
            }
            #endregion COMPANY MANAGEMENT SHOULD ONLY BE VISIBLE TO THE GMG ADMINISTRATOR.

            #region SUPLIER ADMIN SHOULD BE ABLE TO SEE USER MANAGEMENT TAB
            if (Convert.ToInt32(Session["CompanyTypeID"]) == 2)
                if (Convert.ToInt32(Session["UserTypeID"]) > 1)
                    hypUserManagement.Visible = true;
                else
                    hypUserManagement.Visible = false;
            #endregion SUPLIER ADMIN SHOULD BE ABLE TO SEE USER MANAGEMENT TAB

            if (Convert.ToInt32(Session["CompanyTypeID"]) == 2)
            {
                hypCreditNotesCurrent.Visible = false;
                hypCreditNotesHistory.Visible = false;

                hypCreateCreditNotes.Visible = true;
                hypCreateCreditNotesHistory.Visible = true;

                hypP2DInvoice.Visible = true;
            }
            else if (Convert.ToInt32(Session["CompanyTypeID"]) == 1)
            {
                hypPassInv.Visible = true;
                hypPurchaseInvoiceHistory.Visible = true;
                hypCreditNotesCurrent.Visible = true;
                hypCreditNotesHistory.Visible = true;
                hypDownloadDatabase.Visible = true;

                hypCreateCreditNotes.Visible = false;
                hypCreateCreditNotesHistory.Visible = false;
            }
            else if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
            {
                hypPassInv.Visible = true;
                hypPurchaseInvoiceHistory.Visible = true;
                hypCreditNotesCurrent.Visible = false;
                hypCreditNotesHistory.Visible = false;
            }
            // IF NOT A BUYER COMPANY THEN NOT SHOW DOWNLOAD DATABASE 
            if (Convert.ToInt32(Session["CompanyTypeID"]) != 1)
            {
                hypDownloadDatabase.Visible = false;
            }

            #region A SUPPLIER COMPANY USER IS NOT AMONG ADMIN USERS CANNOT SEE ANY MANAGEMENT TAB
            if (Convert.ToInt32(Session["CompanyTypeID"]) == 2 && Convert.ToInt32(Session["UserTypeID"]) == 1)
            {
                hypBranchManagement.Visible = false;
                Hyperlink3.Visible = false;
            }
            #endregion A SUPPLIER COMPANY USER IS NOT AMONG ADMIN USERS CANNOT SEE ANY MANAGEMENT TAB

            #region WHEN A SUPPLIER COMPANY LOGS IN AS ADMINISTRATOR COMMENTED.
            /*
			if (Convert.ToInt32(Session["CompanyTypeID"]) == 2 && Convert.ToInt32(Session["UserTypeID"]) == 3)
			{
				WhenASuuplierCompanyLogInAsAdministrator();
			}
			*/
            #endregion WHEN A SUPPLIER COMPANY LOGS IN AS ADMINISTRATOR

            #region WHEN A INVOICE IN/OUT COMPANY LOGS AS A ADMINISTRATOR/MANAGEMENT COMMENTED.
            /*
			if (Convert.ToInt32(Session["CompanyTypeID"]) == 3 && (Convert.ToInt32(Session["UserTypeID"]) == 3 || Convert.ToInt32(Session["UserTypeID"]) == 4))
			{
				WhenAInOutCompanyLogsInAsAdministratorOrManagement();				
			}
			*/
            #endregion WHEN A INVOICE IN/OUT COMPANY LOGS AS A ADMINISTRATOR/MANAGEMENT

            #region WHEN A INVOICE IN/OUT COMPANY LOGS AS A AP COMMENTED.
            /*
			if (Convert.ToInt32(Session["CompanyTypeID"]) == 3 && Convert.ToInt32(Session["UserTypeID"]) == 2)
			{
				WhenAInOutCompanyLogsInAsAP();
			}
			*/
            #endregion WHEN A INVOICE IN/OUT COMPANY LOGS AS A AP

            #region WHEN A INVOICE IN/OUT COMPANY LOGS AS A USER COMMENTED
            /*
			if (Convert.ToInt32(Session["CompanyTypeID"]) == 3 && Convert.ToInt32(Session["UserTypeID"]) == 1)
			{
				WhenAInOutCompanyLogsInAsUser();
			}
			*/
            #endregion WHEN A INVOICE IN/OUT COMPANY LOGS AS A USER

            #region IF USERTYPE IS AP THEN UPDATE OVERDUE STATUS OF ALL INVOICES. COMMENTED
            /*
			if(Convert.ToInt32(Session["CompanyTypeID"]) == 1) //Tarakeshwar 16-jan-2006
			{
				if (Convert.ToInt32(Session["UserTypeID"]) == 2 || Convert.ToInt32(Session["UserTypeID"]) == 3 || Convert.ToInt32(Session["UserTypeID"]) == 4)
				{
					hypExport.Visible = true;
					objCompany.UpdateOverDueStatusAtAPLogin(Convert.ToInt32(Session["CompanyID"]));
					objCompany.UpdateOverDueStatusForCreditNotesAtAPLogin(Convert.ToInt32(Session["CompanyID"]));
				}
			}
			*/
            #endregion IF USERTYPE IS AP THEN UPDATE OVERDUE STATUS OF ALL INVOICES

        }
        #endregion
        #region UpdateOverDueStatus
        private void UpdateOverDueStatus()
        {
            string cos = ConfigurationManager.AppSettings["ConnectionString"].ToString();

            //DataAccess da=new DataAccess(CBSAppUtils.PrimaryConnectionString);
            DataAccess da = new DataAccess(cos);
            int retOverDue = da.ExecuteNonQuery("sp_InvokeOverDueInvoiceStatus");
        }
        #endregion

        #region UpdateOverdueInvoice COMMENTED
        //240505 by SURAJIT
        /*
        private void UpdateOverdueInvoice()
        {
            DataAccess da=new DataAccess(CBSAppUtils.PrimaryConnectionString);
            int retOverDue = da.ExecuteNonQuery("up_Invoice_Overdue");
        }
        */
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region WhenASuuplierCompanyLogInAsAdministrator COMMENTED
        /*
		private void WhenASuuplierCompanyLogInAsAdministrator()
		{
			hypCreateInvoice.Visible = true;
			hypSalesInvoiceHistory.Visible = true;
			hypCreateCreditNotes.Visible = true;
			hypCreateCreditNotesHistory.Visible = true;
			hypUserManagement.Visible = true;
			Hyperlink1.Visible = true;
			hypBranchManagement.Visible = true;
			hypChangePassword.Visible = true;			
			hypSupplierRelation.Visible = false;
			hypExport.Visible = false;
			Hyperlink5.Visible = false; 
			hypCompanyManagement.Visible = false;
			hypImport.Visible = false;
			hypSalesPerson.Visible = false;
			hypPassInv.Visible = false;
			hypPurchaseInvoiceHistory.Visible = false;
			hypApproveInv.Visible = false;
			hypDownloadDatabase.Visible = false;
			hypCreditNotesCurrent.Visible = false;
			hypCreditNotesHistory.Visible = false;
		}
		*/
        #endregion

        #region WhenAInOutCompanyLogsInAsAdministratorOrManagement COMMENTED
        /*
		private void WhenAInOutCompanyLogsInAsAdministratorOrManagement()
		{			
			hypCreateInvoice.Visible = true;
			hypSalesInvoiceHistory.Visible = true;
			hypCreateCreditNotes.Visible = true;
			hypCreateCreditNotesHistory.Visible = true;
			hypPassInv.Visible = true;
			hypPurchaseInvoiceHistory.Visible = true;
			hypCreditNotesCurrent.Visible = true;
			hypCreditNotesHistory.Visible = true;
			hypUserManagement.Visible = true;
			Hyperlink1.Visible = true; 
			Hyperlink5.Visible = true; 
			hypCompanyManagement.Visible = true;
			hypBranchManagement.Visible = true;
			hypSupplierRelation.Visible = true;
			hypChangePassword.Visible = true;
			hypThreshold.Visible = false;
			hypExport.Visible = false;
			hypImport.Visible = false;
			hypSalesPerson.Visible = false;
			hypApproveInv.Visible = false;
			hypDownloadDatabase.Visible = false;
		}
		*/
        #endregion

        #region WhenAInOutCompanyLogsInAsAP COMMENTED
        /*
		private void WhenAInOutCompanyLogsInAsAP()
		{		
			hypPassInv.Visible = true;
			hypPurchaseInvoiceHistory.Visible = true;
			hypCreditNotesCurrent.Visible = true;
			hypCreditNotesHistory.Visible = true;
			hypChangePassword.Visible = true;
			hypCreateInvoice.Visible = false;
			hypSalesInvoiceHistory.Visible = false;
			hypCreateCreditNotes.Visible = false;
			hypCreateCreditNotesHistory.Visible = false;
			hypUserManagement.Visible = false;
			Hyperlink1.Visible = false; 
			Hyperlink5.Visible = false;
			hypCompanyManagement.Visible = false;
			hypBranchManagement.Visible = false;
			hypSupplierRelation.Visible = false;
			hypExport.Visible = false;
			hypImport.Visible = false;
			hypSalesPerson.Visible = false;
			hypApproveInv.Visible = false;
			hypDownloadDatabase.Visible = false;
		}
		*/
        #endregion

        #region WhenAInOutCompanyLogsInAsUser COMMENTED
        /*
		private void WhenAInOutCompanyLogsInAsUser()
		{					
			hypCreateInvoice.Visible = true;
			hypSalesInvoiceHistory.Visible = true;
			hypCreateCreditNotes.Visible = true;
			hypCreateCreditNotesHistory.Visible = true;
			hypChangePassword.Visible = true;
			hypPassInv.Visible = false;
			hypPurchaseInvoiceHistory.Visible = false;
			hypCreditNotesCurrent.Visible = false;
			hypCreditNotesHistory.Visible = false;
			hypUserManagement.Visible = false;
			Hyperlink1.Visible = false; 
			Hyperlink5.Visible = false; 
			hypCompanyManagement.Visible = false;
			hypBranchManagement.Visible = false;
			hypSupplierRelation.Visible = false;
			hypExport.Visible = false;
			hypImport.Visible = false;
			hypSalesPerson.Visible = false;
			hypApproveInv.Visible = false;
			hypDownloadDatabase.Visible = false;
		}
		*/
        #endregion

    }
}