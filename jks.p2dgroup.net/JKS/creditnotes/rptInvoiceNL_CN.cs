using System;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using CBSolutions.Architecture.Data;

namespace CBSolutions.ETH.Web.ETC.Invoice
{
	public class rptInvoiceNL_CN : ActiveReport
	{
		#region RecordSet Objects
		RecordSet rsInvoiceHeader = null;
		RecordSet rsInvoiceDetail = null;
		#endregion

		#region rptInvoice_CN CONSTRUCTOR OVERLOADED
		public rptInvoiceNL_CN()
		{	
			InitializeReport();
		}

		public rptInvoiceNL_CN(RecordSet rsHeader, RecordSet rsDetail)
		{	
			rsInvoiceHeader = rsHeader;
			rsInvoiceDetail = rsDetail;
			InitializeReport();
		}
		#endregion
		
		#region GetAddressLine
		private string GetAddressLine(string s)
		{
			if(s==null || s.Trim() == "")
				return "";
			else
				return s.Trim() + "\r\n";
		}
		#endregion

		#region rptInvoice_DataInitialize
		private void rptInvoice_DataInitialize(object sender, System.EventArgs eArgs)
		{
			
			RecordSet rsSupplier	= Company.GetCompanyData( System.Convert.ToInt32( rsInvoiceHeader["SupplierCompanyID"]));		
			RecordSet rsBuyer		= Company.GetCompanyData( System.Convert.ToInt32( rsInvoiceHeader["BuyerCompanyID"]));
			
			Fields.Add("SupplierComapany");
			Fields.Add("BuyerComapany");			
			Fields.Add("New_InvoiceName"); 
			Fields.Add("CreditNoteID") ;
			Fields.Add("InvoiceNo") ;
			//Fields.Add("BranchID") ;
			Fields.Add("PaymentDueDate") ;
			Fields.Add("DiscountPercent");
			Fields.Add("CustomerAccNo")  ;
			Fields.Add("InvoiceDate")    ;
			Fields.Add("PaymentTerm")    ;
			
			Fields.Add("NetTotal")   ;
			Fields.Add("H_VATAmt")   ;
			Fields.Add("H_TotalAmt")   ;
			Fields.Add("InvoiceAddress")   ;
			Fields.Add("DeliveryAddress")   ;
			Fields.Add("SupplierAddress")   ;
			Fields.Add("SellerVATNo")   ;
			Fields.Add("New_InvoiceContact");
			Fields.Add("New_OverallDiscountPercent");
			Fields.Add("New_SettlementDays1");
			Fields.Add("New_SettlementDays2");
			Fields.Add("New_SettlementPercent2");
			//
			Fields.Add("New_PaymentDate");
			Fields.Add("New_PaymentMethod");
			Fields.Add("New_DiscountGiven");
			//invoice detail fields
			Fields.Add("PurOrderNo"); 
			Fields.Add("PurOrderLineNo")    ;
			Fields.Add("PurOrderDate")    ;
			Fields.Add("PurInfo")    ;
			Fields.Add("Quantity")    ;
			Fields.Add("Description") ;
			Fields.Add("UOM")   ;      
			Fields.Add("VATRate")    ; 
			Fields.Add("Discount")    ; 
			Fields.Add("Rate")   ;
			Fields.Add("BuyersProdCode")   ;
			Fields.Add("SuppliersProdCode")   ;
			Fields.Add("New_DespatchNoteNumber") ;
			Fields.Add("New_DespatchDate")   ;
			Fields.Add("New_DiscountPercent2");
			Fields.Add("New_NettValue") ;
			Fields.Add("VATAmt") ;
			Fields.Add("TotalAmt") ;
			Fields.Add("New_Type") ;
			Fields.Add("New_Definable1") ;			

			Fields["CreditNoteID"].Value 	 = rsInvoiceHeader["CreditNoteID"] 	 ;
			Fields["InvoiceNo"].Value 	 = rsInvoiceHeader["InvoiceNo"] 	 ;
			//Fields["BranchID"].Value 	 = rsInvoiceHeader["BranchID"] 	 ;
			Fields["PaymentDueDate"].Value = rsInvoiceHeader["PaymentDueDate"] ;
			Fields["DiscountPercent"].Value = rsInvoiceHeader["DiscountPercent"];
			Fields["New_SettlementPercent2"].Value  = rsInvoiceHeader["New_SettlementPercent2"]  ;
			Fields["CustomerAccNo"].Value  = rsInvoiceHeader["CustomerAccNo"]  ;
			Fields["New_InvoiceContact"].Value  = rsInvoiceHeader["New_InvoiceContact"]  ;
			Fields["New_OverallDiscountPercent"].Value  = rsInvoiceHeader["New_OverallDiscountPercent"]  ;
			Fields["New_SettlementDays1"].Value  = rsInvoiceHeader["New_SettlementDays1"];
			Fields["New_SettlementDays2"].Value  = rsInvoiceHeader["New_SettlementDays2"];
			Fields["InvoiceDate"].Value    = rsInvoiceHeader["InvoiceDate"]    ;
			Fields["PaymentTerm"].Value    = rsInvoiceHeader["PaymentTerm"]    ;
			Fields["NetTotal"].Value = rsInvoiceHeader["NetTotal"]  ;
			Fields["H_VATAmt"].Value = rsInvoiceHeader["VATAmt"]  ;
			Fields["H_TotalAmt"].Value  = rsInvoiceHeader["TotalAmt"]  ;

			
			Fields["SupplierComapany"].Value = rsSupplier["CompanyName"];
			Fields["BuyerComapany"].Value = rsBuyer["CompanyName"];
			rsSupplier = null;
			rsBuyer = null;
			

			Fields["New_InvoiceName"].Value = rsInvoiceHeader["New_InvoiceName"]; 

			Invoice objInvoice = new Invoice();
			Invoice_CN objInvoice_CN = new Invoice_CN();
			if (rsInvoiceHeader["CurrencyTypeID"] != DBNull.Value)
			{
				TextBox9.Text = objInvoice.GetCurrencyCode(Convert.ToInt32(rsInvoiceHeader["CurrencyTypeID"]));
			}
			Double dGBPEquivalentAmount = 0;
			dGBPEquivalentAmount = objInvoice_CN.GetGBPEquivalentAmount(Convert.ToInt32(rsInvoiceHeader["CreditNoteID"]));

			if (dGBPEquivalentAmount > 0)
			{
				lblGBPEquivalentAmount.Visible = true;
				tblGBPEquivalentAmount.Text = dGBPEquivalentAmount.ToString();
			}

			if(rsInvoiceHeader["CreditNoteID"] != DBNull.Value)
			{
				
				txtAssociatedInvoiceNo.Text = objInvoice_CN.GetAssociatedInvoiceNo(Convert.ToInt32(rsInvoiceHeader["CreditNoteID"]));
			}

			string s = GetAddressLine(rsInvoiceHeader["DeliveryAddress1"].ToString());
			s += GetAddressLine(rsInvoiceHeader["DeliveryAddress2"].ToString());
			s += GetAddressLine(rsInvoiceHeader["DeliveryAddress3"].ToString());
			s += GetAddressLine(rsInvoiceHeader["DeliveryAddress4"].ToString());
			s += GetAddressLine(rsInvoiceHeader["DeliveryAddress5"].ToString());
			s += GetAddressLine(rsInvoiceHeader["DeliveryCountry"].ToString());
			s += GetAddressLine(rsInvoiceHeader["DeliveryZIP"].ToString());
			Fields["DeliveryAddress"].Value  = s;

			s = GetAddressLine(rsInvoiceHeader["InvoiceAddress1"].ToString());
			s += GetAddressLine(rsInvoiceHeader["InvoiceAddress2"].ToString());
			s += GetAddressLine(rsInvoiceHeader["InvoiceAddress3"].ToString());
			s += GetAddressLine(rsInvoiceHeader["InvoiceAddress4"].ToString());
			s += GetAddressLine(rsInvoiceHeader["InvoiceAddress5"].ToString());
			s += GetAddressLine(rsInvoiceHeader["InvoiceCountry"].ToString());
			s += GetAddressLine(rsInvoiceHeader["InvoiceZIP"].ToString());
			Fields["InvoiceAddress"].Value  = s;

			s = GetAddressLine(rsInvoiceHeader["SupplierAddress1"].ToString());
			s += GetAddressLine(rsInvoiceHeader["SupplierAddress2"].ToString());
			s += GetAddressLine(rsInvoiceHeader["SupplierAddress3"].ToString());
			s += GetAddressLine(rsInvoiceHeader["SupplierAddress4"].ToString());
			s += GetAddressLine(rsInvoiceHeader["SupplierAddress5"].ToString());
			s += GetAddressLine(rsInvoiceHeader["SupplierCountry"].ToString());
			s += GetAddressLine(rsInvoiceHeader["SupplierZIP"].ToString());
			Fields["SupplierAddress"].Value = s;			
			Fields["SellerVATNo"].Value  = rsInvoiceHeader["SellerVATNo"]  ;
		}
		#endregion

		#region rptInvoice_FetchData
		private void rptInvoice_FetchData(object sender, DataDynamics.ActiveReports.ActiveReport.FetchEventArgs eArgs)
		{
			if(rsInvoiceDetail.EOF())
			{
				eArgs.EOF = true;
				return;
			}
			
			Fields["PurOrderNo"].Value = rsInvoiceDetail["PurOrderNo"] ;       
			string purInfo = "";
			if (rsInvoiceDetail["PurOrderNo"] != DBNull.Value )
				purInfo = rsInvoiceDetail["PurOrderNo"].ToString() + "\r\t" ;
			if (rsInvoiceDetail["PurOrderDate"] != DBNull.Value )
				purInfo += System.Convert.ToDateTime(rsInvoiceDetail["PurOrderDate"]).ToString("dd-MM-yyyy") ;

			Fields["PurInfo"].Value =  purInfo;       
			Fields["PurOrderLineNo"].Value = rsInvoiceDetail["PurOrderLineNo"];
			Fields["Quantity"].Value = rsInvoiceDetail["Quantity"] ;           
			Fields["Description"].Value = rsInvoiceDetail["Description"] ;     
			Fields["UOM"].Value = rsInvoiceDetail["UOM"] ;
			Fields["VATAmt"].Value   = rsInvoiceDetail["VATAmt"]   ;         
			Fields["VATRate"].Value = rsInvoiceDetail["VATRate"] ;  
			Fields["New_NettValue"].Value = rsInvoiceDetail["New_NettValue"] ; 
			Fields["TotalAmt"].Value = rsInvoiceDetail["TotalAmt"] ;            
			Fields["Discount"].Value = rsInvoiceDetail["Discount"] ;           
			Fields["Rate"].Value = rsInvoiceDetail["Rate"] ;             
			Fields["PurOrderDate"].Value = rsInvoiceDetail["PurOrderDate"] ;           
			Fields["BuyersProdCode"].Value = rsInvoiceDetail["BuyersProdCode"] ;           
			Fields["SuppliersProdCode"].Value = rsInvoiceDetail["SuppliersProdCode"] ;           
			Fields["New_DespatchNoteNumber"].Value = rsInvoiceDetail["New_DespatchNoteNumber"] ;
			Fields["New_DespatchDate"].Value   = rsInvoiceDetail["New_DespatchDate"]   ;
			Fields["New_Type"].Value   = rsInvoiceDetail["New_Type"]   ;
			Fields["New_Definable1"].Value=Convert.ToString(rsInvoiceDetail["New_Definable1"]);
			Fields["New_DiscountPercent2"].Value = rsInvoiceDetail["New_DiscountPercent2"];
            
			eArgs.EOF = false;
			rsInvoiceDetail.MoveNext();
		}

		#endregion
		
		#region ActiveReports Designer generated code
		private PageHeader PageHeader = null;
		private Label Label23 = null;
		private GroupHeader GroupHeader1 = null;
		private Shape Shape1 = null;
		private TextBox TextBox1 = null;
		private TextBox TextBox2 = null;
		private TextBox TextBox3 = null;
		private Label Label1 = null;
		private Label Label2 = null;
		private Label Label3 = null;
		private Label Label4 = null;
		private Label Label5 = null;
		private Label Label6 = null;
		private Label Label7 = null;
		private Label Label8 = null;
		private Label Label9 = null;
		private TextBox TextBox6 = null;
		private TextBox TextBox7 = null;
		private TextBox TextBox8 = null;
		private TextBox TextBox9 = null;
		private Label Label10 = null;
		private Label Label11 = null;
		private Label Label12 = null;
		private Label Label13 = null;
		private Label Label14 = null;
		private Label Label15 = null;
		private Label Label16 = null;
		private Label Label17 = null;
		private Label Label18 = null;
		private Label Label19 = null;
		private Label Label20 = null;
		private Label Label21 = null;
		private TextBox TextBox19 = null;
		private TextBox TextBox20 = null;
		private TextBox TextBox21 = null;
		private Label Label22 = null;
		private Label Label27 = null;
		private TextBox TextBox25 = null;
		private Label Label28 = null;
		private Label Label29 = null;
		private Label Label30 = null;
		private Label Label31 = null;
		private Label Label32 = null;
		private TextBox txtAssociatedInvoiceNo = null;
		private Label lblSecondDisc = null;
		private Label lblSettlementDay = null;
		private TextBox txtSettlementDay = null;
		private Label lblOverallDiscount = null;
		private TextBox txtOverallDiscount = null;
		private Label lblBuyerCompany = null;
		private TextBox txtBuyerCompany = null;
		private Label lblSupplierCompany = null;
		private TextBox txtSupplierCompany = null;
		private TextBox txtCustomerContact = null;
		private Label lblCustomerContact = null;
		private Label lblInvoiceName = null;
		private TextBox txtInvoiceName = null;
		private Label lblSecondSettlementDays = null;
		private TextBox txtSecondSettlementDays = null;
		private Label Label73 = null;
		private TextBox txtSettlementDiscount2 = null;
		private Label Label74 = null;
		private Label lblPaymentDate = null;
		private Label lblPaymentMethod = null;
		private Label txtPaymentDate = null;
		private Label txtPaymentMethod = null;
		private Label lblDiscountGiven = null;
		private Label txtDiscountGiven = null;
		private Label Label75 = null;
		private Line Line3 = null;
		private Line Line4 = null;
		private Label Label76 = null;
		private Detail Detail = null;
		private TextBox TextBox10 = null;
		private TextBox TextBox11 = null;
		private TextBox TextBox12 = null;
		private TextBox TextBox13 = null;
		private TextBox TextBox14 = null;
		private TextBox TextBox15 = null;
		private TextBox TextBox16 = null;
		private TextBox TextBox17 = null;
		private TextBox TextBox18 = null;
		private TextBox TextBox26 = null;
		private TextBox TextBox27 = null;
		private TextBox TextBox28 = null;
		private TextBox txtDespatchNoteNo = null;
		private TextBox txtDespatchDate = null;
		private TextBox txtSecondDiscount = null;
		private TextBox TextBox29 = null;
		private TextBox TextBox30 = null;
		private TextBox txtColor = null;
		private GroupFooter GroupFooter1 = null;
		private Label Label24 = null;
		private TextBox TextBox22 = null;
		private TextBox TextBox23 = null;
		private Label Label25 = null;
		private Label Label26 = null;
		private TextBox TextBox24 = null;
		private Line Line2 = null;
		private Label lblGBPEquivalentAmount = null;
		private TextBox tblGBPEquivalentAmount = null;
		private PageFooter PageFooter = null;
		private Label Label71 = null;
		public void InitializeReport()
		{
			this.LoadLayout(this.GetType(), "CBSolutions.ETH.Web.newlook.creditnotes.rptInvoiceNL_CN.rpx");
			this.PageHeader = ((DataDynamics.ActiveReports.PageHeader)(this.Sections["PageHeader"]));
			this.GroupHeader1 = ((DataDynamics.ActiveReports.GroupHeader)(this.Sections["GroupHeader1"]));
			this.Detail = ((DataDynamics.ActiveReports.Detail)(this.Sections["Detail"]));
			this.GroupFooter1 = ((DataDynamics.ActiveReports.GroupFooter)(this.Sections["GroupFooter1"]));
			this.PageFooter = ((DataDynamics.ActiveReports.PageFooter)(this.Sections["PageFooter"]));
			this.Label23 = ((DataDynamics.ActiveReports.Label)(this.PageHeader.Controls[0]));
			this.Shape1 = ((DataDynamics.ActiveReports.Shape)(this.GroupHeader1.Controls[0]));
			this.TextBox1 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[1]));
			this.TextBox2 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[2]));
			this.TextBox3 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[3]));
			this.Label1 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[4]));
			this.Label2 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[5]));
			this.Label3 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[6]));
			this.Label4 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[7]));
			this.Label5 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[8]));
			this.Label6 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[9]));
			this.Label7 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[10]));
			this.Label8 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[11]));
			this.Label9 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[12]));
			this.TextBox6 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[13]));
			this.TextBox7 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[14]));
			this.TextBox8 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[15]));
			this.TextBox9 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[16]));
			this.Label10 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[17]));
			this.Label11 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[18]));
			this.Label12 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[19]));
			this.Label13 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[20]));
			this.Label14 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[21]));
			this.Label15 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[22]));
			this.Label16 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[23]));
			this.Label17 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[24]));
			this.Label18 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[25]));
			this.Label19 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[26]));
			this.Label20 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[27]));
			this.Label21 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[28]));
			this.TextBox19 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[29]));
			this.TextBox20 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[30]));
			this.TextBox21 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[31]));
			this.Label22 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[32]));
			this.Label27 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[33]));
			this.TextBox25 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[34]));
			this.Label28 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[35]));
			this.Label29 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[36]));
			this.Label30 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[37]));
			this.Label31 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[38]));
			this.Label32 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[39]));
			this.txtAssociatedInvoiceNo = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[40]));
			this.lblSecondDisc = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[41]));
			this.lblSettlementDay = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[42]));
			this.txtSettlementDay = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[43]));
			this.lblOverallDiscount = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[44]));
			this.txtOverallDiscount = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[45]));
			this.lblBuyerCompany = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[46]));
			this.txtBuyerCompany = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[47]));
			this.lblSupplierCompany = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[48]));
			this.txtSupplierCompany = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[49]));
			this.txtCustomerContact = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[50]));
			this.lblCustomerContact = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[51]));
			this.lblInvoiceName = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[52]));
			this.txtInvoiceName = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[53]));
			this.lblSecondSettlementDays = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[54]));
			this.txtSecondSettlementDays = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[55]));
			this.Label73 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[56]));
			this.txtSettlementDiscount2 = ((DataDynamics.ActiveReports.TextBox)(this.GroupHeader1.Controls[57]));
			this.Label74 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[58]));
			this.lblPaymentDate = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[59]));
			this.lblPaymentMethod = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[60]));
			this.txtPaymentDate = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[61]));
			this.txtPaymentMethod = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[62]));
			this.lblDiscountGiven = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[63]));
			this.txtDiscountGiven = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[64]));
			this.Label75 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[65]));
			this.Line3 = ((DataDynamics.ActiveReports.Line)(this.GroupHeader1.Controls[66]));
			this.Line4 = ((DataDynamics.ActiveReports.Line)(this.GroupHeader1.Controls[67]));
			this.Label76 = ((DataDynamics.ActiveReports.Label)(this.GroupHeader1.Controls[68]));
			this.TextBox10 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[0]));
			this.TextBox11 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[1]));
			this.TextBox12 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[2]));
			this.TextBox13 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[3]));
			this.TextBox14 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[4]));
			this.TextBox15 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[5]));
			this.TextBox16 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[6]));
			this.TextBox17 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[7]));
			this.TextBox18 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[8]));
			this.TextBox26 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[9]));
			this.TextBox27 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[10]));
			this.TextBox28 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[11]));
			this.txtDespatchNoteNo = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[12]));
			this.txtDespatchDate = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[13]));
			this.txtSecondDiscount = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[14]));
			this.TextBox29 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[15]));
			this.TextBox30 = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[16]));
			this.txtColor = ((DataDynamics.ActiveReports.TextBox)(this.Detail.Controls[17]));
			this.Label24 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[0]));
			this.TextBox22 = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[1]));
			this.TextBox23 = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[2]));
			this.Label25 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[3]));
			this.Label26 = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[4]));
			this.TextBox24 = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[5]));
			this.Line2 = ((DataDynamics.ActiveReports.Line)(this.GroupFooter1.Controls[6]));
			this.lblGBPEquivalentAmount = ((DataDynamics.ActiveReports.Label)(this.GroupFooter1.Controls[7]));
			this.tblGBPEquivalentAmount = ((DataDynamics.ActiveReports.TextBox)(this.GroupFooter1.Controls[8]));
			this.Label71 = ((DataDynamics.ActiveReports.Label)(this.PageFooter.Controls[0]));
			// Attach Report Events
			this.DataInitialize += new System.EventHandler(this.rptInvoice_DataInitialize);
			this.FetchData += new DataDynamics.ActiveReports.ActiveReport.FetchEventHandler(this.rptInvoice_FetchData);
		}

		#endregion
	}
}
