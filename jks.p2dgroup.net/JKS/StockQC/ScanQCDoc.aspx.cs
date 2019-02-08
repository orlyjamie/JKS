using System;
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
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;

namespace JKS 
{ 
    /// <summary>
    /// Summary description for ScanQCDoc.
    /// </summary>
    public class ScanQCDoc : CBSolutions.ETH.Web.ETC.VSPage
    {
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.DropDownList ddlComp;
        protected System.Web.UI.WebControls.Label lblMsg;
        protected System.Web.UI.WebControls.Panel Panel3;

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            if (!IsPostBack)
            {
                btnSubmit.Attributes.Add("onclick", "javascript:doHourglass();");
                lblMsg.Visible = false;
                int iCompanyID = Convert.ToInt32(Session["CompanyID"]);
                LoadCompany();

            }
        }
        private void LoadCompany()
        {
            Company objCompany = new Company();
            ddlComp.Items.Clear();
            DataTable dt = new DataTable();
            dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
            if (dt.Rows.Count > 0)
            {
                ddlComp.DataSource = dt;
                ddlComp.DataTextField = "CompanyName";
                ddlComp.DataValueField = "CompanyID";
                ddlComp.DataBind();
            }
            ddlComp.Items.Insert(0, new ListItem("Select Company Name", "0"));
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

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (ddlComp.SelectedIndex > 0)
            {
                string sWin = "";
                string sURL = System.Configuration.ConfigurationManager.AppSettings["ScanQcURL"];
                string sUrl;
                if (System.Configuration.ConfigurationManager.AppSettings["Cypher on"] == "YES")
                {
                    string sCipher = Utility.encryptdate(1, DateTime.Now);
                    sUrl = sURL + "?cipher=" + sCipher;
                    sUrl = sUrl + "&user_id=" + Session["UserID"];
                    sUrl = sUrl + "&co_id=" + Convert.ToInt32(ddlComp.SelectedItem.Value.ToString());
                }
                else
                {
                    sUrl = sURL;
                }
                sWin = "<script language='javascript'>";
                sWin = sWin + "javascript:window.open('" + sUrl + "','_blank','SCANQC','width=700,height=700');";
                sWin = sWin + "</script>";
                Page.RegisterStartupScript("Scrpt", sWin);
                lblMsg.Visible = false;
            }
            else
            {
                lblMsg.Text = "Please select a company.";
                lblMsg.Visible = true;
            }
        }
    }
}
