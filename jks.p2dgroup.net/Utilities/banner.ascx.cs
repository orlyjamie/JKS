namespace CBSolutions.ETH.Web.Utilities
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;

    /// <summary>
    ///		Summary description for banner.
    /// </summary>
    public class banner : System.Web.UI.UserControl
    {
        #region WebControls
        protected System.Web.UI.WebControls.Label lblCompany;
        protected System.Web.UI.WebControls.HyperLink hypLogOff;
        protected System.Web.UI.WebControls.Label lblUser;
        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            lblCompany.Text = (string)Session["CompanyName"];
            lblUser.Text = (string)Session["FirstName"] + " " + (string)Session["LastName"];
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
    }
}
