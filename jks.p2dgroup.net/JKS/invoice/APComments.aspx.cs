using System;
using System.Net;
using System.IO;
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
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

namespace JKS
{
    /// <summary>
    /// Summary description for APComments.
    /// </summary>
    public class APComments : CBSolutions.ETH.Web.ETC.VSPage
    {
        protected System.Web.UI.WebControls.Label lblDocNo;
        protected System.Web.UI.WebControls.Label lblDocStatus;
        protected System.Web.UI.WebControls.TextBox txtComments;
        protected System.Web.UI.WebControls.Button btnCancel;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.Label lblMessage;

        #region User Defined Variables
        private DataTable dtbl = new DataTable();
        private salescall objSales = new salescall();
        private Invoice.Invoice objInvoice = new Invoice.Invoice();
        private int iInvoiceID = 0;
        protected System.Web.UI.WebControls.DataGrid dgSalesCallDetails;
        //		private int ChkUserID=0;
        protected System.Web.UI.WebControls.CheckBox chkHold;
        private string sDocType = "";
        protected string strDocNo = "";
        protected string strDocStatus = "";
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdIsDeleted;
        protected int UserTypeID = 0;
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Added by Mrinal on 31st December 2014
            Session["RowID"] = null;

            btnSubmit.Attributes.Add("onclick", "return CheckComment();");
            if (Request["UserID"] != "1470")
            {
                if (Session["UserID"] == null)
                    Response.Redirect("../close_win.aspx");
            }

            if (Request["InvoiceID"] != null)
            {
                iInvoiceID = Convert.ToInt32(Request["InvoiceID"].Trim());
                ViewState["InvoiceID"] = iInvoiceID.ToString().Trim();
                sDocType = Request["DocType"].Trim();
            }
            if (Request["DocNo"] != null)
                strDocNo = Request["DocNo"].ToString();
            if (Request["DocStatus"] != null)
                strDocStatus = Request["DocStatus"].ToString();

            UserTypeID = Convert.ToInt32(Session["UserTypeID"]);

            if (!Page.IsPostBack)
            {

                lblDocNo.Text = strDocNo.ToString();
                lblDocStatus.Text = strDocStatus.ToString();

                GetInvoiceStatusDetails(iInvoiceID, sDocType);
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Write("<script>self.close();</script>");
        }

        #region GetInvoiceStatusDetails
        private void GetInvoiceStatusDetails(int iInvoiceID, string iDocType)
        {
            dtbl = objInvoice.GetAPCommentsGMG(iInvoiceID, iDocType);
            int sHold = objInvoice.GetAPCommLinkColor(iInvoiceID, iDocType);
            if (sHold == 1)
                chkHold.Checked = true;

            if (dtbl.Rows.Count > 0)
            {
                dgSalesCallDetails.Visible = true;
                dgSalesCallDetails.DataSource = dtbl;
                dgSalesCallDetails.DataBind();
            }
            else
            {
                dgSalesCallDetails.Visible = false;
                lblMessage.Text = "Sorry, this document has no previous AP Comments.";
            }
        }
        #endregion
        #region dgSalesCallDetails_PageIndexChanged
        private void dgSalesCallDetails_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < dgSalesCallDetails.PageCount)
            {
                dgSalesCallDetails.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                dgSalesCallDetails.CurrentPageIndex = dgSalesCallDetails.PageCount;
            }
            GetInvoiceStatusDetails(Convert.ToInt32(ViewState["InvoiceID"]), sDocType);
        }
        #endregion

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            int sHold = 0;
            if (chkHold.Checked)
                sHold = 1;
            else
                sHold = 0;

            int iRetValue = objInvoice.SaveAPCommentsGMG(iInvoiceID, txtComments.Text, sDocType, sHold, Convert.ToInt32(Session["UserID"]));
            if (iRetValue > 0)
            {
                Response.Write("<script>self.close();</script>");
            }
        }
    }
}
