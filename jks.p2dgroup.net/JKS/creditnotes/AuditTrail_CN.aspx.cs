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
using CBSolutions.ETH.Web.ETC;

namespace CBSolutions.ETH.Web.ETC.creditnotes
{
    /// <summary>
    /// Summary description for AuditTrail_CN.
    /// </summary>
    public class AuditTrail_CN : CBSolutions.ETH.Web.ETC.VSPage
    {
        protected System.Web.UI.WebControls.Label lblauthstring;
        protected System.Web.UI.WebControls.Label lblDepartment;
        protected System.Web.UI.WebControls.DataGrid dgSalesCallDetails;
        protected System.Web.UI.WebControls.Label lblMessage;

        #region User Defined Variables
        private DataTable dtbl = new DataTable();
        private salescall objSales = new salescall();
        private CBSolutions.ETH.Web.dalkia.Invoice.Invoice objInvoice = new CBSolutions.ETH.Web.dalkia.Invoice.Invoice();
        private int iInvoiceID = 0;
        private int ChkUserID = 0;
        protected string DocType = "";
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Request["UserID"] != "1470")
            {
                if (Session["UserID"] == null)
                    Response.Redirect("../close_win.aspx");
            }
            DocType = Request.QueryString["DocType"].ToString();
            if (Request["InvoiceID"] != null)
            {
                iInvoiceID = Convert.ToInt32(Request["InvoiceID"].Trim());
                ViewState["InvoiceID"] = iInvoiceID.ToString().Trim();
            }
            if (!Page.IsPostBack)
            {
                GetAuditTrailDetails(iInvoiceID, DocType);
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
            this.dgSalesCallDetails.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgSalesCallDetails_PageIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion

        #region GetAuditTrailDetails
        private void GetAuditTrailDetails(int iInvoiceID, string DocType)
        {
            if (DocType == "INV")
            {
                lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "INV");
                lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "INV");
            }
            else
            {
                lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "CRN");
                lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "CRN");
            }

            ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)
                dtbl = objInvoice.GetAuditTrail(iInvoiceID, DocType);

            else
                dtbl = objInvoice.GetAuditTrail(iInvoiceID, DocType);

            if (dtbl.Rows.Count > 0)
            {

                dgSalesCallDetails.Visible = true;
                dgSalesCallDetails.DataSource = dtbl;
                dgSalesCallDetails.DataBind();
            }
            else
            {
                dgSalesCallDetails.Visible = false;
                lblMessage.Text = "Sorry, this document has no previous actions.";
            }
        }
        #endregion
        #region dgSalesCallDetails_PageIndexChanged
        private void dgSalesCallDetails_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < dgSalesCallDetails.PageCount)
            {
                dgSalesCallDetails.CurrentPageIndex = e.NewPageIndex;
                GetAuditTrailDetails(Convert.ToInt32(ViewState["InvoiceID"]), DocType);
            }
            else
            {
                dgSalesCallDetails.CurrentPageIndex = dgSalesCallDetails.PageCount;
            }
        }
        #endregion
    }
}
