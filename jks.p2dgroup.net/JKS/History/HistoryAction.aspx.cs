using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
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
    public class HistoryAction : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WEBCONTROLS

        protected System.Web.UI.WebControls.Label lbldocumentdate;
        protected System.Web.UI.WebControls.Label lblsuppliername;
        protected System.Web.UI.WebControls.Label lbldocumentstatus;
        protected System.Web.UI.WebControls.Label lblauthstring;
        protected System.Web.UI.WebControls.Label lbldepartment;
        protected System.Web.UI.WebControls.Label lblassociatedinvoiceno;
        protected System.Web.UI.WebControls.Label lblassociatedno;
        protected System.Web.UI.WebControls.Button btndelete;
        protected System.Web.UI.WebControls.TextBox txtcomment;
        protected System.Web.UI.WebControls.Label lblmessage;
        protected System.Web.UI.WebControls.Label lblComment;
        protected System.Web.UI.WebControls.Label lbldocumentno;
        protected System.Web.UI.WebControls.Label lblVoucherno;
        protected System.Web.UI.WebControls.DataGrid GDlineinfo;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdIsDeleted;
        protected System.Web.UI.WebControls.Label lblCreditNoteNo;
        protected System.Web.UI.WebControls.Label lbltextCrnNo;
        #endregion

        #region User Defined Controls
        int StatusUpdate = 0;
        int UserTypeID = 0;
        int iApproverStatusID = 0;
        string strComments = "";
        string DocType = "";
        protected int visiblelable = 0;
        protected int visiblelable1 = 0;
        Invoice.Invoice objinvoice = new Invoice.Invoice();
        #endregion

        #region private void Page_Load(object sender, System.EventArgs e)
        private void Page_Load(object sender, System.EventArgs e)
        {
            Session["eInvoiceID"] = Request.QueryString["InvoiceID"].ToString();
            Session["eDocType"] = Request.QueryString["DocType"].ToString();
            Session["eInvoiceDate"] = Request.QueryString["InvoiceDate"].ToString();
            Session["eDocStatus"] = Request.QueryString["DocStatus"].ToString();
            Session["eVoucherNumber"] = Request.QueryString["VoucherNumber"].ToString();
            DocType = Session["eDocType"].ToString();

            if (!Page.IsPostBack)
            {
                LoadData(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
            }

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
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region private void LoadData(int InvoiceID)
        private void LoadData(int InvoiceID)
        {
            string strDocNo = "";
            string DocType = Session["eDocType"].ToString();
            objinvoice.GetDocumentNoByDocID(InvoiceID, DocType, out strDocNo);
            lbldocumentno.Text = strDocNo;
            lblVoucherno.Text = Session["eVoucherNumber"].ToString();
            lbldocumentdate.Text = Session["eInvoiceDate"].ToString();
            lbldocumentstatus.Text = Session["eDocStatus"].ToString();
            lblsuppliername.Text = objinvoice.GetSupplierName(InvoiceID, DocType);
            lblauthstring.Text = objinvoice.GetAuthorisationString(InvoiceID, DocType);
            lbldepartment.Text = objinvoice.GetDepartment(InvoiceID, DocType);


            int UserTypeID = objinvoice.GetUserType(Convert.ToInt32(Session["UserID"]));
            if (UserTypeID < 3)
            {
                btndelete.Visible = false;
                txtcomment.Visible = false;
                lblComment.Visible = false;
                lblCreditNoteNo.Visible = false;
                lbltextCrnNo.Visible = false;
            }

            if (DocType == "CRN")
            {
                visiblelable = 1;
                lblassociatedinvoiceno.Text = "Associated Invoice No";
                lblassociatedinvoiceno.Visible = true;

                lblassociatedno.Text = objinvoice.GetAssociatedCreditInvoiceNo(InvoiceID, DocType);
                lblassociatedno.Visible = true;

            }
            if (DocType == "INV")
            {
                lblCreditNoteNo.Text = "Associated CreditNote No";
                lbltextCrnNo.Text = objinvoice.GetAssociatedCreditInvoiceNo(InvoiceID, DocType);
                if (lbltextCrnNo.Text.Trim() != "")
                {
                    visiblelable1 = 1;
                    lblCreditNoteNo.Visible = true;
                    lbltextCrnNo.Visible = true;
                }
            }

            GDlineinfo.DataSource = objinvoice.GetLineInformation(InvoiceID, DocType);
            GDlineinfo.DataBind();

            if (lbldocumentstatus.Text == "Delete/Archive")
                btndelete.Visible = false;
        }
        #endregion

        #region private void btndelete_Click(object sender, System.EventArgs e)
        private void btndelete_Click(object sender, System.EventArgs e)
        {
            if (txtcomment.Text.Trim() == "")
            {
                lblmessage.Text = "Please enter a comment.";
                return;
            }
            else
            {
                lblmessage.Text = "";
                iApproverStatusID = 7;
                strComments = txtcomment.Text.Trim();
                UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));

                if (DocType == "INV")
                {
                    StatusUpdate = objinvoice.UpdateInvStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (StatusUpdate == 1)
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                        lblmessage.Text = "Invoice deleted successfully";
                        hdIsDeleted.Value = "1";
                        Response.Write("<script>alert('Invoice Deleted Sussessfully');</script>");
                        Response.Write("<script>self.close();</script>");
                    }
                    else
                        lblmessage.Text = "Invoice cannot be deleted";
                }
                else if (DocType == "CRN")
                {
                    StatusUpdate = objinvoice.UpdateCrnStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (StatusUpdate == 1)
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments);
                        lblmessage.Text = "CreditNote deleted successfully";
                        hdIsDeleted.Value = "1";
                        Response.Write("<script>alert('Credit Note Deleted Sussessfully');</script>");

                        Response.Write("<script>self.close();</script>");

                    }
                    else
                        lblmessage.Text = "CreditNote cannot be deleted";
                }
                else
                {
                    StatusUpdate = objinvoice.UpdateDebitNoteStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (StatusUpdate == 1)
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise_DN(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments);
                        lblmessage.Text = "CreditNote deleted successfully";
                        hdIsDeleted.Value = "1";
                        Response.Write("<script>alert('Credit Note Deleted Sussessfully');</script>");

                        Response.Write("<script>self.close();</script>");

                    }
                    else
                        lblmessage.Text = "CreditNote cannot be deleted";
                }
            }
        }
        #endregion
    }
}
