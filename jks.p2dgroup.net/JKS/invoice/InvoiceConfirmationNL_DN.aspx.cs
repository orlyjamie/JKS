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
using DataDynamics.ActiveReports.Export;
using DataDynamics.ActiveReports.Document;

namespace JKS
{
    /// <summary>
    /// Summary description for InvoiceConfirmationNL_DN.
    /// </summary>
    public class InvoiceConfirmationNL_DN : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel4;
        protected System.Web.UI.WebControls.Label lblConfirmation;
        protected System.Web.UI.WebControls.Label lblDocument_No;
        protected System.Web.UI.WebControls.Label lblAssociated_Invoice_No;
        protected System.Web.UI.WebControls.Label lblCurrency;
        protected System.Web.UI.WebControls.Label lblpaymentdate;
        protected System.Web.UI.WebControls.Label lblSupplierAddress;
        protected System.Web.UI.WebControls.Label lblDeliveryAddress;
        protected System.Web.UI.WebControls.Label lblInvoiceAddress;
        protected System.Web.UI.WebControls.DataGrid grdInvoiceDetails;
        protected System.Web.UI.WebControls.Button btnGeneratePDF;
        protected System.Web.UI.WebControls.HyperLink HyperLink1;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.Label lblVendorID;
        protected System.Web.UI.WebControls.Label lblAP_Company_ID;
        protected System.Web.UI.WebControls.Label lblStatus;
        protected System.Web.UI.WebControls.Label lblDocument_Date;
        protected System.Web.UI.WebControls.Label lblNew_PaymentMethod;
        protected System.Web.UI.WebControls.Label lblNew_DiscountGiven;
        protected System.Web.UI.WebControls.Label lblNew_PaymentDueDate;
        protected System.Web.UI.WebControls.Label lblTax_Amoun;
        protected System.Web.UI.WebControls.Label lblNett_Amount;
        protected System.Web.UI.WebControls.Label lblGross_Amount;
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
            this.btnGeneratePDF.Click += new System.EventHandler(this.btnGeneratePDF_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        int iDebitNoteID = 0;

        #region  Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            if (Request.QueryString["InvoiceID"] != null)
            {
                iDebitNoteID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
            }

            if (!IsPostBack)
            {
                LoadData(Convert.ToInt32(Request.QueryString["InvoiceID"]));
            }
        }
        #endregion

        #region LoadData
        private void LoadData(int DebitNoteID)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            SqlCommand sqlCmd = new SqlCommand("usp_GetInvoiceConfirmationNL_DN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@DebitNoteID", DebitNoteID);
            SqlDataReader sqlDR = null;
            try
            {
                sqlConn.Open();
                sqlDR = sqlCmd.ExecuteReader();

                if (sqlDR.HasRows)
                {
                    while (sqlDR.Read())
                    {
                        if (sqlDR["Document_No"] != DBNull.Value)
                        {
                            lblDocument_No.Text = Convert.ToString(sqlDR["Document_No"]);
                        }

                        if (sqlDR["Associated_Invoice_No"] != DBNull.Value)
                        {
                            lblAssociated_Invoice_No.Text = Convert.ToString(sqlDR["Associated_Invoice_No"]);
                        }

                        if (sqlDR["AP_Company_ID"] != DBNull.Value)
                        {
                            lblAP_Company_ID.Text = Convert.ToString(sqlDR["AP_Company_ID"]);
                        }

                        if (sqlDR["VendorID"] != DBNull.Value)
                        {
                            lblVendorID.Text = Convert.ToString(sqlDR["VendorID"]);
                        }

                        if (sqlDR["Document_Date"] != DBNull.Value)
                        {
                            lblDocument_Date.Text = DateTime.Parse(Convert.ToString(sqlDR["Document_Date"])).ToString("dd/MM/yyyy");
                        }

                        if (sqlDR["Status"] != DBNull.Value)
                        {
                            lblStatus.Text = Convert.ToString(sqlDR["Status"]);
                        }

                        if (sqlDR["Currency"] != DBNull.Value)
                        {
                            lblCurrency.Text = Convert.ToString(sqlDR["Currency"]);
                        }

                        if (sqlDR["New_PaymentDate"] != DBNull.Value)
                        {
                            lblpaymentdate.Text = Convert.ToString(sqlDR["New_PaymentDate"]);
                        }

                        if (sqlDR["New_PaymentMethod"] != DBNull.Value)
                        {
                            lblNew_PaymentMethod.Text = Convert.ToString(sqlDR["New_PaymentMethod"]);
                        }

                        if (sqlDR["New_DiscountGiven"] != DBNull.Value)
                        {
                            lblNew_DiscountGiven.Text = Convert.ToString(sqlDR["New_DiscountGiven"]);
                        }

                        if (sqlDR["New_PaymentDueDate"] != DBNull.Value)
                        {
                            lblNew_PaymentDueDate.Text = Convert.ToString(sqlDR["New_PaymentDueDate"]);
                        }

                        if (sqlDR["supplierAdd"] != DBNull.Value)
                        {
                            lblSupplierAddress.Text = Convert.ToString(sqlDR["supplierAdd"]);
                        }

                        if (sqlDR["deliveryadd"] != DBNull.Value)
                        {
                            lblDeliveryAddress.Text = Convert.ToString(sqlDR["deliveryadd"]);
                        }

                        if (sqlDR["invadd"] != DBNull.Value)
                        {
                            lblInvoiceAddress.Text = Convert.ToString(sqlDR["invadd"]);
                        }

                        if (sqlDR["Nett_Amount"] != DBNull.Value)
                        {
                            lblNett_Amount.Text = Convert.ToString(sqlDR["Nett_Amount"]);
                        }

                        if (sqlDR["Tax_Amount"] != DBNull.Value)
                        {
                            lblTax_Amoun.Text = Convert.ToString(sqlDR["Tax_Amount"]);
                        }

                        if (sqlDR["Gross_Amount"] != DBNull.Value)
                        {
                            lblGross_Amount.Text = Convert.ToString(sqlDR["Gross_Amount"]);
                        }
                    }
                }
                sqlDR.NextResult();
                grdInvoiceDetails.DataSource = sqlDR;
                grdInvoiceDetails.DataBind();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDR.Close();
                sqlConn.Dispose();
            }
        }
        #endregion


        private void btnGeneratePDF_Click(object sender, System.EventArgs e)
        {
            DataSet ds = Invoice.Invoice.GetDebitNoteHeadDetail(iDebitNoteID).ParentDataSet;
            Invoice.DebitNoteNL rpt = new Invoice.DebitNoteNL(ds);
            rpt.PageSettings.Orientation = PageOrientation.Landscape;
            DataDynamics.ActiveReports.Export.Pdf.PdfExport pdf = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
            string pdfFile = (string)Session["DebitNotePDF_NL"];
            pdfFile = "../Files/" + System.IO.Path.GetFileName(pdfFile);
            rpt.Run();
            pdf.Export(rpt.Document, Server.MapPath(pdfFile));
            Response.Redirect(pdfFile);

        }
    }
}
