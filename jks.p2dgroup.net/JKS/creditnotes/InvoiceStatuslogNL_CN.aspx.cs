using System;
using System.Configuration;
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

namespace CBSolutions.ETH.Web.ETC.CreditNotes
{
    /// <summary>
    /// Summary description for InvoiceStatuslogNL_CN.
    /// </summary>
    public partial class InvoiceStatuslogNL_CN : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        //protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.Panel Panel3;
        //protected System.Web.UI.WebControls.DataGrid dgSalesCallDetails;	
        #endregion
        // =================================================================================================
        #region User Defined Variable
        private DataTable dtbl = new DataTable();
        private salescall objSales = new salescall();
        //private Invoice.Invoice objInvoice = new Invoice.Invoice();	
        JKS.Invoice objInvoice = new JKS.Invoice();
        //protected System.Web.UI.WebControls.Label lblauthstring;
        //protected System.Web.UI.WebControls.Label lblDepartment;
        private int iInvoiceID = 0;
        //protected System.Web.UI.WebControls.Label lblBusinessUnit;
        private int ChkUserID = 0;
        private int IsHover = 0;
        #endregion
        // =================================================================================================
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Request["UserID"] != "1470")
            {
                if (Session["UserID"] == null)
                    Response.Redirect("../close_win.aspx");
            }

            if (Request["InvoiceID"] != null)
            {
                iInvoiceID = Convert.ToInt32(Request["InvoiceID"].Trim());
                ViewState["InvoiceID"] = iInvoiceID.ToString().Trim();
            }
            if (Request["IsHover"] != null)
            {
                IsHover = Convert.ToInt32(Request["IsHover"].Trim());
                if (IsHover == 1)
                {
                    aClose.Visible = false;
                }
            }
            if (!Page.IsPostBack)
            {
                GetInvoiceStatusDetails(iInvoiceID);
            }
        }
        #endregion
        // =================================================================================================
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
        // =================================================================================================
        #region GetInvoiceStatusDetails
        private void GetInvoiceStatusDetails(int iInvoiceID)
        {
            JKS.Invoice objInvoice = new JKS.Invoice();
            lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "CRN");
            lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "CRN");
            lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "CRN");

            ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            else
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            //	dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier_CN(iInvoiceID);


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
        // =================================================================================================
        #region dgSalesCallDetails_PageIndexChanged
        //private void dgSalesCallDetails_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        //{
        //    if (e.NewPageIndex < dgSalesCallDetails.PageCount)
        //    {
        //        dgSalesCallDetails.CurrentPageIndex = e.NewPageIndex;								
        //    }
        //    else
        //    {
        //        dgSalesCallDetails.CurrentPageIndex = dgSalesCallDetails.PageCount;				
        //    }
        //    GetInvoiceStatusDetails(Convert.ToInt32(ViewState["InvoiceID"]));
        //}
        #endregion


        protected void dgSalesCallDetails_PageIndexChanged1(object source, DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < dgSalesCallDetails.PageCount)
            {
                dgSalesCallDetails.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                dgSalesCallDetails.CurrentPageIndex = dgSalesCallDetails.PageCount;
            }
            GetInvoiceStatusDetails(Convert.ToInt32(ViewState["InvoiceID"]));
        }
    }
}
