namespace JKS
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using CBSolutions.Architecture.Core;
    using CBSolutions.Architecture.Data;
    using System.Configuration;
    /// <summary>
    ///		Summary description for menuUserNL.
    /// </summary>
    public class menuUserNL : System.Web.UI.UserControl
    {
        #region WebControls
        protected System.Web.UI.WebControls.HyperLink hypSupplierRelation;
        protected System.Web.UI.WebControls.HyperLink hypDownloadDatabase;
        protected System.Web.UI.WebControls.HyperLink hypChangePassword;
        protected System.Web.UI.WebControls.HyperLink hypUserManagement;
        protected System.Web.UI.WebControls.HyperLink hypCompanyManagement;
        protected System.Web.UI.WebControls.HyperLink hypThreshold;
        protected System.Web.UI.WebControls.HyperLink hypBranchManagement;
        protected System.Web.UI.WebControls.HyperLink hypStockQC;
        protected System.Web.UI.WebControls.HyperLink hypCurrent;
        protected System.Web.UI.WebControls.HyperLink hypHistory;
        protected System.Web.UI.WebControls.HyperLink hypTradingRelation;
        protected System.Web.UI.WebControls.HyperLink imgScanQC;
        protected System.Web.UI.WebControls.HyperLink hypUserTradingRelation;
        protected System.Web.UI.WebControls.Button btnExportInvoice;
        #endregion

        #region User Defined Variables
        //private int iUserID = 0;

        //private int iSupplierCompanyID = 0;
        #endregion

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {

        }
        #endregion

        #region UpdateOverDueStatus
        private void UpdateOverDueStatus()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
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
            this.btnExportInvoice.Click += new EventHandler(this.btnExportInvoice_Click);

        }
        #endregion

        private void btnExportInvoice_Click(object sender, EventArgs e)
        {
            DateTime TimeNow = System.DateTime.Now;
            int Hours = TimeNow.Hour;
            int Minute = TimeNow.Minute;
            string CAppAqillaServiceHour = ConfigurationManager.AppSettings["CAppAqillaServiceTiming"].ToString();// ConfigurationManager.AppSettings["CAppAqillaServiceTiming"].ToString();
            string[] CAppAqillaServiceHours = CAppAqillaServiceHour.Split(',');
            for (int i = 0; i < CAppAqillaServiceHours.Length; i++)
            {
                if (Hours == Convert.ToInt32(CAppAqillaServiceHours[i].ToString()) && Minute < 45)
                {
                    Page.RegisterStartupScript("reg", "<script> alert('The export has already been executed.');</script>");
                    return;
                }
            }
            if (Application["ETCExecutionTime"] != null)
            {
                DateTime LastExecutionTime = (DateTime)Application["ETCExecutionTime"];
                DateTime CurrentTime = System.DateTime.Now;
                TimeSpan Span = CurrentTime - LastExecutionTime;
                double TotalMinutes = Span.TotalMinutes;
                if (TotalMinutes > 45)
                {
                    Application["ETCExecutionTime"] = System.DateTime.Now;
                    Page.RegisterStartupScript("reg", "<script>fnExportInvoice();</script>");
                    //Response.Redirect("../History/ExportInvoice.aspx");
                }
                else
                {
                    Page.RegisterStartupScript("reg", "<script> alert('The export has already been executed.');</script>");
                }
                //TimeDiff=Convert.ToInt32(Application["ExecutionTime"]);
            }
            else
            {
                Application["ETCExecutionTime"] = System.DateTime.Now;
                Page.RegisterStartupScript("reg", "<script>fnExportInvoice();</script>");
                //Response.Redirect("../History/ExportInvoice.aspx");
            }

        }
    }
}
