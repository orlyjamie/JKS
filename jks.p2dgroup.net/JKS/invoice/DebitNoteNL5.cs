using System;
using System.Data;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using CBSolutions.Architecture.Data;

namespace CBSolutions.ETH.Web.ETC.Invoice
{
    public class DebitNoteNL : ActiveReport
    {
        DataSet ds2 = null;

        public DebitNoteNL()
        {
            InitializeReport();
        }

        public DebitNoteNL(DataSet ds)
        {
            ds2 = ds;
            InitializeReport();
        }

        private void DebitNoteNL_DataInitialize(object sender, System.EventArgs eArgs)
        {

            Fields.Add("SupplierComapany");
            Fields.Add("BuyerComapany");
            Fields.Add("InvoiceID");
            Fields.Add("DebitNoteNo");
            Fields.Add("AssociatedInvoiceNo");
            Fields.Add("PaymentDueDate");
            Fields.Add("New_PaymentDate");
            Fields.Add("New_PaymentMethod");
            Fields.Add("New_DiscountGiven");
            Fields.Add("DocumentDate");
            Fields.Add("NetTotal");
            Fields.Add("VATAmt");
            Fields.Add("TotalAmt");
            Fields.Add("InvoiceAddress");
            Fields.Add("DeliveryAddress");
            Fields.Add("SupplierAddress");
            Fields.Add("CurrencyTypeID");
            Fields.Add("PurOrderNo");
            Fields.Add("PurOrderLineNo");
            Fields.Add("Quantity");
            Fields.Add("Description");
            Fields.Add("Rate");
            Fields.Add("BuyersProdCode");
            Fields.Add("Color");
            Fields.Add("New_NettValue");
            Fields.Add("H_VATAmt");
            Fields.Add("H_TotalAmt");

            Fields["DebitNoteNo"].Value = ds2.Tables[0].Rows[0]["Document_No"];
            Fields["AssociatedInvoiceNo"].Value = ds2.Tables[0].Rows[0]["Associated_Invoice_No"];

            Fields["SupplierComapany"].Value = ds2.Tables[0].Rows[0]["VendorID"];
            Fields["BuyerComapany"].Value = ds2.Tables[0].Rows[0]["AP_Company_ID"];
            Fields["DocumentDate"].Value = ds2.Tables[0].Rows[0]["Document_Date"];

            Fields["PaymentDueDate"].Value = ds2.Tables[0].Rows[0]["New_PaymentDueDate"];



            Fields["NetTotal"].Value = ds2.Tables[0].Rows[0]["Nett_Amount"];
            Fields["H_VATAmt"].Value = ds2.Tables[0].Rows[0]["Tax_Amount"];
            Fields["H_TotalAmt"].Value = ds2.Tables[0].Rows[0]["Gross_Amount"];
            Fields["New_DiscountGiven"].Value = ds2.Tables[0].Rows[0]["New_DiscountGiven"];
            Fields["New_PaymentMethod"].Value = ds2.Tables[0].Rows[0]["New_PaymentMethod"];
            Fields["New_PaymentDate"].Value = ds2.Tables[0].Rows[0]["New_PaymentDate"];




            Invoice objInvoice = new Invoice();
            if (ds2.Tables[0].Rows[0]["CurrencyTypeID"] != DBNull.Value)
            {
                txtCurrency.Text = objInvoice.GetCurrencyCode(Convert.ToInt32(ds2.Tables[0].Rows[0]["CurrencyTypeID"]));
            }

            Fields["DeliveryAddress"].Value = ds2.Tables[0].Rows[0]["deliveryadd"];
            Fields["InvoiceAddress"].Value = ds2.Tables[0].Rows[0]["invadd"];
            Fields["SupplierAddress"].Value = ds2.Tables[0].Rows[0]["supplierAdd"];
        }

        private void DebitNoteNL_FetchData(object sender, DataDynamics.ActiveReports.ActiveReport.FetchEventArgs eArgs)
        {
            int i = 0;

            for (i = 0; i < ds2.Tables[1].Rows.Count; i++)
            {
                if (ds2.Tables[1].Rows[i]["PurOrderNo"] != DBNull.Value)
                    Fields["PurOrderNo"].Value = ds2.Tables[1].Rows[i]["PurOrderNo"];
                if (ds2.Tables[1].Rows[i]["debitNoteLineNo"] != DBNull.Value)
                    Fields["PurOrderLineNo"].Value = ds2.Tables[1].Rows[i]["debitNoteLineNo"];
                if (ds2.Tables[1].Rows[i]["Quantity"] != DBNull.Value)
                    Fields["Quantity"].Value = ds2.Tables[1].Rows[i]["Quantity"];
                if (ds2.Tables[1].Rows[i]["DDescription"] != DBNull.Value)
                    Fields["Description"].Value = ds2.Tables[1].Rows[i]["DDescription"];
                if (ds2.Tables[1].Rows[i]["NettValue"] != DBNull.Value)
                    Fields["New_NettValue"].Value = ds2.Tables[1].Rows[i]["NettValue"];
                if (ds2.Tables[1].Rows[i]["New_Definable1"] != DBNull.Value)
                    Fields["Color"].Value = ds2.Tables[1].Rows[i]["New_Definable1"];

                if (ds2.Tables[1].Rows[i]["Price"] != DBNull.Value)
                    Fields["Rate"].Value = ds2.Tables[1].Rows[i]["Price"];
                if (ds2.Tables[1].Rows[i]["BuyersProdCode"] != DBNull.Value)
                    Fields["BuyersProdCode"].Value = ds2.Tables[1].Rows[i]["BuyersProdCode"];
            }

        }

        private string GetAddressLine(string s)
        {
            if (s == null || s.Trim() == "")
                return "";
            else
                return s.Trim() + "\r\n";
        }

        private PageHeader PageHeader = null;
        private Label Label23 = null;
        private GroupHeader GroupHeader1 = null;
        private Shape Shape1 = null;
        private Label Label27 = null;
        private TextBox txtInvoiceNo = null;
        private Label Label2 = null;
        private TextBox txtInvoiceDate = null;
        private Label Label9 = null;
        private TextBox txtCurrency = null;
        private Label lblPaymentMethod = null;
        private Label lblPaymentDate = null;
        private Label Label5 = null;
        private TextBox txtPaymentDueDate = null;
        private Label txtPaymentDate = null;
        private Label txtPaymentMethod = null;
        private Label lblDiscountGiven = null;
        private Label txtDiscountGiven = null;
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
        private Label Label31 = null;
        private Label Label12 = null;
        private Label Label28 = null;
        private Label Label11 = null;
        private Label Label30 = null;
        private Label Label18 = null;
        private Label Label15 = null;
        private Label Label75 = null;
        private Label Label17 = null;
        private Label Label76 = null;
        private TextBox txtAssociatedInvoiceNo = null;
        private Label Label77 = null;
        private Detail Detail = null;
        private TextBox txtPONo = null;
        private TextBox txtPODate = null;
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
        private Label Label24 = null;
        private TextBox txtTotal = null;
        private TextBox txtVATTotal = null;
        private Label Label25 = null;
        private Label Label26 = null;
        private TextBox txtNetTotal = null;
        private PageFooter PageFooter = null;
        private Label Label71 = null;
        public void InitializeReport()
        {
            this.LoadLayout(this.GetType(), "CBSolutions.ETH.Web.ETC.invoice.DebitNoteNL.rpx");
            this.PageHeader = ((DataDynamics.ActiveReports.PageHeader)(this.Sections["PageHeader"]));
            this.GroupHeader1 = ((DataDynamics.ActiveReports.GroupHeader)(this.Sections["GroupHeader1"]));
            this.Detail = ((DataDynamics.ActiveReports.Detail)(this.Sections["Detail"]));
            this.GroupFooter1 = ((DataDynamics.ActiveReports.GroupFooter)(this.Sections["GroupFooter1"]));
            this.PageFooter = ((DataDynamics.ActiveReports.PageFooter)(this.Sections["PageFooter"]));
            this.Label23 = ((DataDynamics.ActiveReports.Label)(this.PageHeader.Controls[0]));
            this.Shape1 = ((DataDynamics.ActiveReports.Shape)(this.GroupHeader1.Controls[0]));
            this.Label27 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[1]));
            this.txtInvoiceNo = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[2]));
            this.Label2 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[3]));
            this.txtInvoiceDate = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[4]));
            this.Label9 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[5]));
            this.txtCurrency = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[6]));
            this.lblPaymentMethod = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[7]));
            this.lblPaymentDate = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[8]));
            this.Label5 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[9]));
            this.txtPaymentDueDate = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[10]));
            this.txtPaymentDate = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[11]));
            this.txtPaymentMethod = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[12]));
            this.lblDiscountGiven = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[13]));
            this.txtDiscountGiven = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[14]));
            this.Label21 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[15]));
            this.txtInvoiceAddr = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[16]));
            this.lblSupplierCompany = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[17]));
            this.txtSupplierCompany = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[18]));
            this.Label19 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[19]));
            this.txtSupplierAddr = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[20]));
            this.lblBuyerCompany = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[21]));
            this.txtBuyerCompany = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[22]));
            this.Label20 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[23]));
            this.txtDeliveryAddr = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[24]));
            this.Label22 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[25]));
            this.Label10 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[26]));
            this.Label31 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[27]));
            this.Label12 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[28]));
            this.Label28 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[29]));
            this.Label11 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[30]));
            this.Label30 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[31]));
            this.Label18 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[32]));
            this.Label15 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[33]));
            this.Label75 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[34]));
            this.Label17 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[35]));
            this.Label76 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[36]));
            this.txtAssociatedInvoiceNo = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[37]));
            this.Label77 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[38]));
            this.txtPONo = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[0]));
            this.txtPODate = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[1]));
            this.txtPOLineNo = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[2]));
            this.txtBuyerCode = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[3]));
            this.txtDescription = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[4]));
            this.txtPrice = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[5]));
            this.txtQuantity = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[6]));
            this.txtNetValue = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[7]));
            this.txtVAT = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[8]));
            this.txtGrossValue = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[9]));
            this.txtColor = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[10]));
            this.Label24 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[0]));
            this.txtTotal = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[1]));
            this.txtVATTotal = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[2]));
            this.Label25 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[3]));
            this.Label26 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[4]));
            this.txtNetTotal = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[5]));
            this.Label71 = ((DataDynamics.ActiveReports.Label)(this.PageFooter.Controls[0]));

            this.DataInitialize += new System.EventHandler(this.DebitNoteNL_DataInitialize);
            this.FetchData += new DataDynamics.ActiveReports.ActiveReport.FetchEventHandler(this.DebitNoteNL_FetchData);
        }


    }
}
