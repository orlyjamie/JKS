using System;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Data.SqlClient;
using System.Data;

namespace CBSolutions.ETH.Web.DebitNote
{
    public class NewlookDebitNote : ActiveReport
    {

        #region RecordSet Objects
        RecordSet rsInvoiceHeader = null;
        RecordSet rsInvoiceDetail = null;
        #endregion

        #region NewlookDebitNote CONSTRUCTOR OVERLOADED
        public NewlookDebitNote()
        {
            InitializeReport();
        }

        public NewlookDebitNote(RecordSet rsHeader, RecordSet rsDetail)
        {
            rsInvoiceHeader = rsHeader;
            rsInvoiceDetail = rsDetail;
            InitializeReport();
        }
        #endregion

        #region NewlookDebitNote_DataInitialize
        private void NewlookDebitNote_DataInitialize(object sender, System.EventArgs eArgs)
        {
            #region Add Fields
            //DebitNote header fields	

            Fields.Add("DebitNoteNo");
            Fields.Add("AssociatedInvoiceNo");
            Fields.Add("DocumentDate");
            Fields.Add("New_PaymentDate");
            Fields.Add("New_PaymentMethod");

            Fields.Add("PaymentDueDate");
            Fields.Add("New_DiscountGiven");
            Fields.Add("CurrencyTypeID");
            Fields.Add("Currency");
            Fields.Add("InvoiceAddress");

            Fields.Add("BuyerComapany");
            Fields.Add("DeliveryAddress");

            Fields.Add("SupplierComapany");
            Fields.Add("SupplierAddress");

            Fields.Add("InvoiceID");

            Fields.Add("NetTotal");
            Fields.Add("VATAmt");
            Fields.Add("TotalAmt");


            //DebitNote detail fields
            Fields.Add("PurOrderNo");
            Fields.Add("PurOrderLineNo");
            Fields.Add("Description");
            Fields.Add("BuyersProdCode");
            Fields.Add("Color");

            Fields.Add("Quantity");
            Fields.Add("Rate");

            Fields.Add("New_NettValue");
            Fields.Add("H_VATAmt");
            Fields.Add("H_TotalAmt");


            Fields["DebitNoteNo"].Value = rsInvoiceHeader["Document_No"];
            Fields["AssociatedInvoiceNo"].Value = rsInvoiceHeader["Associated_Invoice_No"];
            Fields["DocumentDate"].Value = rsInvoiceHeader["Document_Date"];
            Fields["New_PaymentDate"].Value = rsInvoiceHeader["New_PaymentDate"];
            Fields["New_PaymentMethod"].Value = rsInvoiceHeader["New_PaymentMethod"];

            Fields["PaymentDueDate"].Value = rsInvoiceHeader["New_PaymentDueDate"];
            Fields["New_DiscountGiven"].Value = rsInvoiceHeader["New_DiscountGiven"];
            Fields["Currency"].Value = rsInvoiceHeader["Currency"];
            Fields["InvoiceAddress"].Value = rsInvoiceHeader["invadd"];

            Fields["BuyerComapany"].Value = rsInvoiceHeader["AP_Company_ID"];
            Fields["DeliveryAddress"].Value = rsInvoiceHeader["deliveryadd"];

            Fields["SupplierComapany"].Value = rsInvoiceHeader["VendorID"];
            Fields["SupplierAddress"].Value = rsInvoiceHeader["supplierAdd"];


            Fields["NetTotal"].Value = rsInvoiceHeader["Nett_Amount"];
            Fields["H_VATAmt"].Value = rsInvoiceHeader["Tax_Amount"];
            Fields["H_TotalAmt"].Value = rsInvoiceHeader["Gross_Amount"];


            #endregion
        }
        #endregion

        #region NewlookDebitNote_FetchData
        private void NewlookDebitNote_FetchData(object sender, DataDynamics.ActiveReports.ActiveReport.FetchEventArgs eArgs)
        {

            if (rsInvoiceDetail.EOF())
            {
                eArgs.EOF = true;
                return;
            }

            Fields["PurOrderNo"].Value = rsInvoiceDetail["PurOrderNo"];
            Fields["PurOrderLineNo"].Value = rsInvoiceDetail["debitNoteLineNo"];
            Fields["Quantity"].Value = rsInvoiceDetail["Quantity"];
            Fields["Description"].Value = rsInvoiceDetail["DDescription"];
            Fields["New_NettValue"].Value = rsInvoiceDetail["NettValue"];
            Fields["Color"].Value = rsInvoiceDetail["New_Definable1"];
            Fields["Rate"].Value = rsInvoiceDetail["Price"];
            Fields["BuyersProdCode"].Value = rsInvoiceDetail["BuyersProdCode"];

            eArgs.EOF = false;
            rsInvoiceDetail.MoveNext();
        }
        #endregion

        private void Detail_Format(object sender, System.EventArgs eArgs)
        {

        }

        #region ActiveReports Designer generated code
        private PageHeader PageHeader = null;
        private Label LabelInvoice = null;
        private GroupHeader GroupHeader1 = null;
        private Label Label27 = null;
        private TextBox txtInvoiceNo = null;
        private TextBox txtAssociatedInvoiceNo = null;
        private Label Label76 = null;
        private TextBox txtInvoiceDate = null;
        private Label Label2 = null;
        private Label Label5 = null;
        private TextBox txtPaymentDueDate = null;
        private Label lblDiscountGiven = null;
        private Label txtDiscountGiven = null;
        private TextBox txtCurrency = null;
        private Label Label9 = null;
        private Label lblPaymentDate = null;
        private Label txtPaymentDate = null;
        private Label txtPaymentMethod = null;
        private Label lblPaymentMethod = null;
        private Label Label21 = null;
        private TextBox txtInvoiceAddr = null;
        private Label lblSupplierCompany = null;
        private TextBox txtSupplierCompany = null;
        private Label Label19 = null;
        private TextBox txtSupplierAddr = null;
        private Label lblBuyerCompany = null;
        private TextBox txtBuyerCompany = null;
        private Label Label20 = null;
        private TextBox txtDeliveryAddr = null;
        private Label Label22 = null;
        private Label Label10 = null;
        private Label Label12 = null;
        private Label Label28 = null;
        private Label Label11 = null;
        private Label Label30 = null;
        private Label Label18 = null;
        private Label Label15 = null;
        private Label Label77 = null;
        private Line Line1 = null;
        private Shape Shape1 = null;
        private Detail Detail = null;
        private TextBox txtPONo = null;
        private TextBox txtPOLineNo = null;
        private TextBox txtBuyerCode = null;
        private TextBox txtDescription = null;
        private TextBox txtPrice = null;
        private TextBox txtQuantity = null;
        private TextBox txtNetValue = null;
        private TextBox txtVAT = null;
        private TextBox txtGrossValue = null;
        private TextBox txtColor = null;
        private GroupFooter GroupFooter1 = null;
        private Label Label78 = null;
        private Label Label79 = null;
        private Label Label80 = null;
        private TextBox txtNetTotal = null;
        private TextBox txtVATTotal = null;
        private TextBox txtTotal = null;
        private PageFooter PageFooter = null;
        private Label Label71 = null;
        public void InitializeReport()
        {
            this.LoadLayout(this.GetType(), "CBSolutions.ETH.Web.DebitNote.NewlookDebitNote.rpx");
            this.PageHeader = ((DataDynamics.ActiveReports.PageHeader)(this.Sections["PageHeader"]));
            this.GroupHeader1 = ((DataDynamics.ActiveReports.GroupHeader)(this.Sections["GroupHeader1"]));
            this.Detail = ((DataDynamics.ActiveReports.Detail)(this.Sections["Detail"]));
            this.GroupFooter1 = ((DataDynamics.ActiveReports.GroupFooter)(this.Sections["GroupFooter1"]));
            this.PageFooter = ((DataDynamics.ActiveReports.PageFooter)(this.Sections["PageFooter"]));
            this.LabelInvoice = ((DataDynamics.ActiveReports.Label)(this.PageHeader.Controls[0]));
            this.Label27 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[0]));
            this.txtInvoiceNo = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[1]));
            this.txtAssociatedInvoiceNo = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[2]));
            this.Label76 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[3]));
            this.txtInvoiceDate = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[4]));
            this.Label2 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[5]));
            this.Label5 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[6]));
            this.txtPaymentDueDate = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[7]));
            this.lblDiscountGiven = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[8]));
            this.txtDiscountGiven = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[9]));
            this.txtCurrency = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[10]));
            this.Label9 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[11]));
            this.lblPaymentDate = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[12]));
            this.txtPaymentDate = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[13]));
            this.txtPaymentMethod = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[14]));
            this.lblPaymentMethod = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[15]));
            this.Label21 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[16]));
            this.txtInvoiceAddr = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[17]));
            this.lblSupplierCompany = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[18]));
            this.txtSupplierCompany = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[19]));
            this.Label19 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[20]));
            this.txtSupplierAddr = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[21]));
            this.lblBuyerCompany = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[22]));
            this.txtBuyerCompany = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[23]));
            this.Label20 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[24]));
            this.txtDeliveryAddr = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[25]));
            this.Label22 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[26]));
            this.Label10 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[27]));
            this.Label12 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[28]));
            this.Label28 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[29]));
            this.Label11 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[30]));
            this.Label30 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[31]));
            this.Label18 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[32]));
            this.Label15 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[33]));
            this.Label77 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[34]));
            this.Line1 = ((DataDynamics.ActiveReports.Line)(this.GroupHeader1.Controls[35]));
            this.Shape1 = ((DataDynamics.ActiveReports.Shape)(this.GroupHeader1.Controls[36]));
            this.txtPONo = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[0]));
            this.txtPOLineNo = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[1]));
            this.txtBuyerCode = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[2]));
            this.txtDescription = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[3]));
            this.txtPrice = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[4]));
            this.txtQuantity = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[5]));
            this.txtNetValue = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[6]));
            this.txtVAT = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[7]));
            this.txtGrossValue = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[8]));
            this.txtColor = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[9]));
            this.Label78 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[0]));
            this.Label79 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[1]));
            this.Label80 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[2]));
            this.txtNetTotal = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[3]));
            this.txtVATTotal = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[4]));
            this.txtTotal = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[5]));
            this.Label71 = ((DataDynamics.ActiveReports.Label)(this.PageFooter.Controls[0]));
            // Attach Report Events
            this.DataInitialize += new System.EventHandler(this.NewlookDebitNote_DataInitialize);
            this.FetchData += new DataDynamics.ActiveReports.ActiveReport.FetchEventHandler(this.NewlookDebitNote_FetchData);
            this.Detail.Format += new System.EventHandler(this.Detail_Format);
        }

        #endregion
    }
}
