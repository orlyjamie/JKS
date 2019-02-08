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
using CBSolutions.Architecture.Data;

namespace JKS
{
    /// <summary>
    /// Summary description for CompanyRedirect.
    /// </summary>
    public class CompanyRedirect : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");

            RecordSet rsAccess = (RecordSet)Session["Access"];
            rsAccess.Filter = "ActionID = " + (int)Actions.AddNewCompany;

            if (Convert.ToInt32(Session["UserTypeID"]) > 2)
            {
                Response.Redirect("CompanyBrowse.aspx");
            }
            else
                Response.Redirect("CompanyEdit.aspx?CompanyID=" + (int)Session["CompanyID"]);
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
        }
        #endregion
    }
}
