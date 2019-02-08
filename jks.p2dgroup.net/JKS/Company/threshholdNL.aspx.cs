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
using System.Configuration;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

namespace JKS 
{  
    /// <summary>
    /// Summary description for threshholdNL.
    /// </summary>
    public class threshholdNL : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.TextBox txtTimeLimit;
        protected System.Web.UI.WebControls.RequiredFieldValidator rvTimeLimit;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.Label lblMessage;
        #endregion

        #region Variable Declaration
        protected Company objCompany = new Company();

        protected string strAmountLimit = "";
        protected string strTimeLimit = "";

        protected int iCompanyID = 0;
        protected int iTimeLimit = 0;
        protected System.Web.UI.WebControls.Label lblHeader;

        protected decimal dAmountLimit = 0;
        #endregion
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            if (!Page.IsPostBack)
            {
                iCompanyID = Convert.ToInt32(Session["CompanyID"]);
                GetThresHoldValues(iCompanyID);
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
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                iTimeLimit = Convert.ToInt32(txtTimeLimit.Text.Trim());
                SaveThresHoldValues(Convert.ToInt32(Session["CompanyID"]), iTimeLimit);
            }
        }
        #endregion
        // ==========================================================================================================
        #region GetThresHoldValues
        private void GetThresHoldValues(int iCompanyID)
        {
            strTimeLimit = "";
            objCompany.GetThresHoldValues(iCompanyID, out strTimeLimit);
            txtTimeLimit.Text = strTimeLimit;
        }
        #endregion
        // ==========================================================================================================
        #region SaveThresHoldValues
        public void SaveThresHoldValues(int iCompanyID, int iTimeLimit)
        {
            if (objCompany.SaveThresHoldValues(iCompanyID, iTimeLimit))
            {
                RefreshForm();
                lblMessage.Text = "Record(s) saved successfully.";
            }
            else
            {
                lblMessage.Text = "Error saving record(s).";
            }
        }
        #endregion
        // ==========================================================================================================
        #region RefreshForm
        private void RefreshForm()
        {

            txtTimeLimit.Text = "";
        }
        #endregion

    }
}
