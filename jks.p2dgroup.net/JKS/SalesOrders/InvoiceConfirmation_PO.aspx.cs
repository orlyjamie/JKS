#region Directives
using System;
using System.Configuration;
using System.IO;
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
using Microsoft.VisualBasic;

using CBSolutions.ETH.Web.SalesOrders;
using System.Web.Mail;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net;
#endregion

namespace JKS
{

    public class InvoiceConfirmation_PO : CBSolutions.ETH.Web.ETC.VSPage
    {
        protected System.Web.UI.WebControls.Panel Panel4;
        protected System.Web.UI.WebControls.Label lblConfirmation;
        protected System.Web.UI.WebControls.Label lblRefernce;
        protected System.Web.UI.WebControls.Label lblSupplier;
        protected System.Web.UI.WebControls.Label lblBuyer;
        protected System.Web.UI.WebControls.Label lblVATRegNo;
        protected System.Web.UI.WebControls.Label lblTermsDiscount;
        protected System.Web.UI.WebControls.Label lblInvoiceDate;
        protected System.Web.UI.WebControls.Label lblSettlementDays;
        protected System.Web.UI.WebControls.Label lblPaymentTerms;
        protected System.Web.UI.WebControls.Label lblSecondSettlementDiscount;
        protected System.Web.UI.WebControls.Label lblPaymentDueDAte;
        protected System.Web.UI.WebControls.Label lblSecondSettlementDays;
        protected System.Web.UI.WebControls.Label lblTaxPointDate;
        protected System.Web.UI.WebControls.Label lblCustomerAccNo;
        protected System.Web.UI.WebControls.Label lblOverAllDiscount;
        protected System.Web.UI.WebControls.Label lblCustomerContactName;
        protected System.Web.UI.WebControls.Label lblCurrency;
        protected System.Web.UI.WebControls.Label lblStatus;
        protected System.Web.UI.WebControls.Label lblpaymentdate;
        protected System.Web.UI.WebControls.Label lblPaymentMethod;
        protected System.Web.UI.WebControls.Label lblDiscount;
        protected System.Web.UI.WebControls.Label lblDespatchNoteNo;
        protected System.Web.UI.WebControls.Label lblDespatchDate;
        protected System.Web.UI.WebControls.Label lblSupplierAddress;
        protected System.Web.UI.WebControls.Label lblDeliveryAddress;
        protected System.Web.UI.WebControls.Label lblInvoiceAddress;
        protected System.Web.UI.WebControls.Label lblActivityCode;
        protected System.Web.UI.WebControls.Label lblAccountCat;
        protected System.Web.UI.WebControls.Label lblInvoiceName;
        protected System.Web.UI.WebControls.DataGrid grdInvoiceDetails;
        protected System.Web.UI.WebControls.Button btnConfirmInvoice;
        protected System.Web.UI.WebControls.Button btnGeneratePDF;
        protected System.Web.UI.WebControls.Label lblNetTotal;
        protected System.Web.UI.WebControls.HyperLink HyperLink1;
        protected System.Web.UI.WebControls.Label lblVAT;
        protected System.Web.UI.WebControls.TextBox txtVATAmt;
        protected System.Web.UI.WebControls.Label lblTotal;
        protected System.Web.UI.WebControls.Label lblGBPEquiAmt;
        protected System.Web.UI.WebControls.TextBox txtSterlingEquivalent;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdGBPEquiFlag;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdSaveFlag;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdHideBack;
        protected System.Web.UI.HtmlControls.HtmlTableRow trInputSterlingEquiAmt;

        #region User Defined Variables
        RecordSet rsInvoiceHead = null;
        RecordSet rsInvoiceDetail = null;
        protected string strInvoiceDocumentDownloadPath = ConfigurationManager.AppSettings["InvoiceDocPath"];
        protected string strInvoiceDocument = "";
        protected string iInvID = "";
        protected string strBuyerVATPrefix = "";
        protected string strSupplierVATPrefix = "";
        protected System.Web.UI.WebControls.Label lblSupplierAccNo;
        protected System.Web.UI.WebControls.Label lblSuppContName;
        protected System.Web.UI.WebControls.Label lblNotes;
        protected System.Web.UI.WebControls.Label lblTerms;
        protected int invoiceID = 0;
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");

            if (Request["hd"] != null)
            {
                hdHideBack.Value = "1";
            }

            Session.Remove("DuplicateInvoice");
            btnGeneratePDF.Visible = false;
            invoiceID = 0;
            if (Request.QueryString["PurchaseOrderID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["PurchaseOrderID"]);
                ViewState["INID"] = invoiceID.ToString();
                iInvID = invoiceID.ToString();
                hdHideBack.Value = "1";

                try
                {
                    Session["StrVATAmt"] = null;

                }
                catch { }
            }
            if (invoiceID == 0)
            {
                if (Session["InvoiceID"] != null)
                    invoiceID = (int)Session["InvoiceID"];
            }
            if (invoiceID == 0)
            {
                //prepare invoice head recordset from XML
                if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                    ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                    rsInvoiceHead = new RecordSet(ds);

                }
                //prepare invoice detail recordset from XML
                if (System.IO.File.Exists(Session["XSDInvoiceDetailFile"].ToString()))
                {
                    DataSet ds = new DataSet();

                    ds.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                    ds.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                    rsInvoiceDetail = new RecordSet(ds);

                }
                if (!IsPostBack)
                {
                    PopulateData();
                }
            }
            else
                PopulateData(invoiceID);
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
            this.grdInvoiceDetails.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvoiceDetails_ItemDataBound);
            this.btnConfirmInvoice.Click += new System.EventHandler(this.btnConfirmInvoice_Click);
            this.btnGeneratePDF.Click += new System.EventHandler(this.btnGeneratePDF_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region PopulateData
        public void PopulateData(int invoiceID)
        {
            rsInvoiceHead = SalesOrder.GetInvoiceHead(invoiceID);
            rsInvoiceDetail = SalesOrder.GetInvoiceDetail(invoiceID);
            lblConfirmation.Visible = false;
            btnConfirmInvoice.Visible = false;
            btnGeneratePDF.Visible = true;
            PopulateData();
        }
        #endregion

        #region GetAddressLine
        private string GetAddressLine(string s)
        {
            if (s == null || s.Trim() == "")
                return "";
            else
                return s.Trim() + ",  ";
        }
        #endregion

        #region PopulateData
        public void PopulateData()
        {

            SalesOrder objSalesOrder = new SalesOrder();
            CBSolutions.ETH.Web.NewBuyer.Invoice.Invoice objInvoice1 = new CBSolutions.ETH.Web.NewBuyer.Invoice.Invoice();
            this.grdInvoiceDetails.DataSource = rsInvoiceDetail.ParentTable;
            this.grdInvoiceDetails.DataBind();
            lblRefernce.Text = lblRefernce.Text + rsInvoiceHead["PurchaseOrderNo"].ToString();
            if (rsInvoiceHead["Document"] != DBNull.Value)
                strInvoiceDocument = rsInvoiceHead["Document"].ToString();

            if (rsInvoiceHead["New_ActivityCode"] != DBNull.Value)
            {
                lblActivityCode.Text = Convert.ToString(rsInvoiceHead["New_ActivityCode"]);
            }
            else
            {
                lblActivityCode.Text = "";
            }

            if (rsInvoiceHead["New_AccountCategory"] != DBNull.Value)
            {
                lblAccountCat.Text = Convert.ToString(rsInvoiceHead["New_AccountCategory"]);
            }
            else
            {
                lblAccountCat.Text = "";
            }

            if (rsInvoiceHead["PaymentDueDate"] != DBNull.Value)
            {
                lblPaymentDueDAte.Text = Convert.ToDateTime(rsInvoiceHead["PaymentDueDate"]).ToString("dd/MM/yyyy");
            }
            else
            {
                lblPaymentDueDAte.Text = "";
            }
            if (rsInvoiceHead["DiscountPercent"] != DBNull.Value)
            {
                lblTermsDiscount.Text = Convert.ToDouble(rsInvoiceHead["DiscountPercent"]).ToString("#0.00");
            }
            else
            {
                lblTermsDiscount.Text = "0";
            }

            if (rsInvoiceHead["New_SettlementPercent2"] != DBNull.Value)
            {
                lblSecondSettlementDiscount.Text = Convert.ToDouble(rsInvoiceHead["New_SettlementPercent2"]).ToString("#0.00");
            }
            else
            {
                lblSecondSettlementDiscount.Text = "0";
            }

            if (rsInvoiceHead["CustomerAccNo"] != DBNull.Value)
                lblCustomerAccNo.Text = rsInvoiceHead["CustomerAccNo"].ToString();

            if (rsInvoiceHead["OrderContact"] != DBNull.Value)
                lblCustomerContactName.Text = rsInvoiceHead["OrderContact"].ToString();

            if (rsInvoiceHead["New_SettlementDays1"] != DBNull.Value)
                lblSettlementDays.Text = rsInvoiceHead["New_SettlementDays1"].ToString();
            if (rsInvoiceHead["New_SettlementDays2"] != DBNull.Value)
                lblSecondSettlementDays.Text = rsInvoiceHead["New_SettlementDays2"].ToString();

            if (rsInvoiceHead["OrderName"] != DBNull.Value)
                lblInvoiceName.Text = rsInvoiceHead["OrderName"].ToString();

            try
            {
                if (rsInvoiceHead["CurrencyTypeID"] != DBNull.Value)
                    lblCurrency.Text = objInvoice1.GetCurrencyName(Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]));

            }
            catch { }

            lblVATRegNo.Text = rsInvoiceHead["SellerVATNo"].ToString();

            if (rsInvoiceHead["OrderDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(rsInvoiceHead["OrderDate"]).ToString("dd/MM/yyyy");
            }
            else
            {
                lblInvoiceDate.Text = "";
            }

            lblPaymentTerms.Text = rsInvoiceHead["PaymentTerm"].ToString();
            lblDespatchNoteNo.Text = rsInvoiceHead["DespatchNoteNo"].ToString();

            if (rsInvoiceHead["DeliveryDate"] != DBNull.Value)
            {
                lblDespatchDate.Text = Convert.ToDateTime(rsInvoiceHead["DeliveryDate"]).ToString("dd/MM/yyyy");
            }
            else
            {
                lblDespatchDate.Text = "";
            }

            if (rsInvoiceHead["TaxPointDate"] != DBNull.Value)
                lblTaxPointDate.Text = Convert.ToDateTime(rsInvoiceHead["TaxPointDate"]).ToString("dd/MM/yyyy");
            else
                lblTaxPointDate.Text = "";

            lblNetTotal.Text = System.Convert.ToDouble(rsInvoiceHead["NetTotal"]).ToString("#0.00");
            lblTotal.Text = System.Convert.ToDouble(rsInvoiceHead["TotalAmt"]).ToString("#0.00");
            txtVATAmt.Text = System.Convert.ToDouble(rsInvoiceHead["VATAmt"]).ToString("#0.00");
            lblVAT.Text = System.Convert.ToDouble(rsInvoiceHead["VATAmt"]).ToString("#0.00");

            string s = "";

            s = GetAddressLine(rsInvoiceHead["DeliveryAddress1"].ToString());
            s += GetAddressLine(rsInvoiceHead["DeliveryAddress2"].ToString());
            s += GetAddressLine(rsInvoiceHead["DeliveryAddress3"].ToString());
            s += GetAddressLine(rsInvoiceHead["DeliveryAddress4"].ToString());
            s += GetAddressLine(rsInvoiceHead["DeliveryAddress5"].ToString());
            s += GetAddressLine(rsInvoiceHead["DeliveryCountry"].ToString());
            s += GetAddressLine(rsInvoiceHead["DeliveryZIP"].ToString());
            try
            {
                s = s.Substring(0, s.Length - 3);
            }
            catch { }

            if (s == "" && Request.QueryString["PurchaseOrderID"] == null)
            {
                s = Convert.ToString(Session["SuppAddForInvHead"]);
            }

            lblDeliveryAddress.Text = s;

            if (rsInvoiceHead["New_OverallDiscountPercent"] != DBNull.Value)
                lblOverAllDiscount.Text = rsInvoiceHead["New_OverallDiscountPercent"].ToString().Trim();


            s = GetAddressLine(rsInvoiceHead["OrderAddress1"].ToString());
            s += GetAddressLine(rsInvoiceHead["OrderAddress2"].ToString());
            s += GetAddressLine(rsInvoiceHead["OrderAddress3"].ToString());
            s += GetAddressLine(rsInvoiceHead["OrderAddress4"].ToString());
            s += GetAddressLine(rsInvoiceHead["OrderAddress5"].ToString());
            s += GetAddressLine(rsInvoiceHead["OrderCountry"].ToString());
            s += GetAddressLine(rsInvoiceHead["OrderZIP"].ToString());
            try
            {
                s = s.Substring(0, s.Length - 3);
            }
            catch { }

            if (s == "" && Request.QueryString["PurchaseOrderID"] == null)
            {
                s = Convert.ToString(Session["SuppAddForInvHead"]);
            }

            lblInvoiceAddress.Text = s;

            if (rsInvoiceHead["New_PaymentDate"] != DBNull.Value)
                lblpaymentdate.Text = System.Convert.ToDateTime(rsInvoiceHead["New_PaymentDate"]).ToString("dd/MM/yyyy");//Amitava 301007
            else
                lblpaymentdate.Text = "";
            if (rsInvoiceHead["New_DiscountGiven"] != DBNull.Value)
                lblDiscount.Text = System.Convert.ToString(rsInvoiceHead["New_DiscountGiven"].ToString());
            else
                lblDiscount.Text = "";
            if (rsInvoiceHead["New_PaymentMethod"] != DBNull.Value)
                lblPaymentMethod.Text = System.Convert.ToString(rsInvoiceHead["New_PaymentMethod"].ToString());
            else
                lblPaymentMethod.Text = "";


            if (rsInvoiceHead["SupplierAccNo"] != DBNull.Value)
            {
                lblSupplierAccNo.Text = Convert.ToString(rsInvoiceHead["SupplierAccNo"]);
            }
            else
            {
                lblSupplierAccNo.Text = "";
            }
            if (rsInvoiceHead["SupplierContact"] != DBNull.Value)
            {
                lblSuppContName.Text = Convert.ToString(rsInvoiceHead["SupplierContact"]);
            }
            else
            {
                lblSuppContName.Text = "";
            }
            if (rsInvoiceHead["Notes"] != DBNull.Value)
            {
                lblNotes.Text = Convert.ToString(rsInvoiceHead["Notes"]);
            }
            else
            {
                lblNotes.Text = "";
            }
            if (rsInvoiceHead["OrderTerms"] != DBNull.Value)
            {
                lblTerms.Text = Convert.ToString(rsInvoiceHead["OrderTerms"]);
            }
            else
            {
                lblTerms.Text = "";
            }

            s = GetAddressLine(rsInvoiceHead["SupplierAddress1"].ToString());
            s += GetAddressLine(rsInvoiceHead["SupplierAddress2"].ToString());
            s += GetAddressLine(rsInvoiceHead["SupplierAddress3"].ToString());
            s += GetAddressLine(rsInvoiceHead["SupplierAddress4"].ToString());
            s += GetAddressLine(rsInvoiceHead["SupplierAddress5"].ToString());
            s += GetAddressLine(rsInvoiceHead["SupplierCountry"].ToString());
            s += GetAddressLine(rsInvoiceHead["SupplierZIP"].ToString());
            try
            {
                s = s.Substring(0, s.Length - 3);
            }
            catch { }

            if (s == "")
            {

                if (Session["CompanyID"] != null)
                {
                    int iCompanyID = 0;
                    iCompanyID = Convert.ToInt32(Session["CompanyID"]);
                    CBSolutions.ETH.Web.Invoice oInvoice = new CBSolutions.ETH.Web.Invoice();
                    DataTable dtbl = null;
                    dtbl = oInvoice.GetSupplierAddressofCompany(iCompanyID);
                    s = Convert.ToString(dtbl.Rows[0]["SupplierAddress"]);
                }

            }
            lblSupplierAddress.Text = s;

            RecordSet rs = Company.GetCompanyData(System.Convert.ToInt32(rsInvoiceHead["SupplierCompanyID"]));
            lblSupplier.Text = rs["CompanyName"].ToString();


            string[] arrSupplierVATPrefix = rs["VATRegNo"].ToString().Split('-');
            strSupplierVATPrefix = arrSupplierVATPrefix[0].ToString().Trim();
            ViewState["SupplierVATPrefix"] = strSupplierVATPrefix;


            rs = Company.GetCompanyData(System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]));
            lblBuyer.Text = rs["CompanyName"].ToString();


            string[] arrBuyerVATPrefix = rs["VATRegNo"].ToString().Split('-');
            strBuyerVATPrefix = arrBuyerVATPrefix[0].ToString().Trim();
            ViewState["BuyerVATPrefix"] = strBuyerVATPrefix;


            if (rsInvoiceHead["StatusId"] != DBNull.Value)
                lblStatus.Text = CBSolutions.ETH.Web.Invoice.GetStatus((int)rsInvoiceHead["StatusId"]);
            else
                lblStatus.Text = "Pending";

            if (ViewState["INID"] != null)
            {
                Double dGBPEquivalentAmount = 0;
                dGBPEquivalentAmount = objInvoice1.GetGBPEquivalentAmount(Convert.ToInt32(ViewState["INID"]));
                if (dGBPEquivalentAmount != 0)
                {
                    lblGBPEquiAmt.Visible = true;
                    hdGBPEquiFlag.Value = "1";
                    lblGBPEquiAmt.Text = dGBPEquivalentAmount.ToString();
                }
                txtVATAmt.Visible = false;
                lblVAT.Visible = true;
            }
            if (Session["StrVATAmt"] != null)
            {
                lblGBPEquiAmt.Visible = true;
                hdGBPEquiFlag.Value = "1";
                lblGBPEquiAmt.Text = Session["StrVATAmt"].ToString();
                txtSterlingEquivalent.Visible = false;
            }
            if (Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]) != 22 && Session["StrVATAmt"] == null && ViewState["INID"] == null)
            {

                if (strBuyerVATPrefix.ToUpper().Trim() == "GB" && strSupplierVATPrefix.ToUpper().Trim() == "GB")
                {
                    txtSterlingEquivalent.Visible = true;
                    trInputSterlingEquiAmt.Visible = true;
                }
                else
                    trInputSterlingEquiAmt.Visible = false;

            }
            else
                trInputSterlingEquiAmt.Visible = false;
        }
        #endregion

        #region btnConfirmInvoice_Click
        private void btnConfirmInvoice_Click(object sender, System.EventArgs e)
        {
            CBSolutions.ETH.Web.Invoice invoice = new CBSolutions.ETH.Web.Invoice();
            //save the invoice head and detail data in a single transaction context
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            da.BeginTransaction();

            int invoicePKID = 0;
            //save invoice head data
            rsInvoiceHead["SupplierCompanyID"] = Convert.ToInt32(Session["CompanyID"]);

            if (rsInvoiceHead["StatusID"] == DBNull.Value)
                rsInvoiceHead["StatusID"] = 20;

            invoicePKID = invoice.InsertInvoiceHeadData(rsInvoiceHead, da);
            if (da.SPReturnValue == 2)
            {
                Session.Add("DuplicateInvoice", "1");
                da.RollbackTransaction();
                rsInvoiceHead["StatusID"] = null;
                Response.Redirect("InvoiceOtherInfo.aspx");
            }

            if (invoicePKID > 0)
                invoice.InsertInvoiceDetailData(invoicePKID, rsInvoiceDetail, da);

            if (da.ErrorCode != DataAccessErrors.Successful)
            {
                da.RollbackTransaction();
                rsInvoiceHead["StatusID"] = null;
                Response.Write(da.ErrorMessage);
            }
            else //else commit transaction
            {
                // CHECKING IF CURRENCY CODE IS NOT GBP.
                if (Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]) != 22 && ViewState["BuyerVATPrefix"].ToString().ToUpper().Trim() == "GB" && ViewState["SupplierVATPrefix"].ToString().ToUpper().Trim() == "GB")
                {
                    if (txtSterlingEquivalent.Text.Trim() == "")
                    {
                        lblMessage.Text = "Please enter the sterling equivalent vat amount because the currency code is not GBP.";
                        da.RollbackTransaction();
                        rsInvoiceHead["StatusID"] = null;
                        return;
                    }
                    else
                    {
                        if (IsNumericValue(txtSterlingEquivalent.Text.Trim()))
                        {
                            Session["StrVATAmt"] = txtSterlingEquivalent.Text.Trim();
                            da.CommitTransaction();
                        }
                        else
                        {
                            lblMessage.Text = "Please enter a numeric value for sterling equivalent vat amount.";
                            rsInvoiceHead["StatusID"] = null;
                            da.RollbackTransaction();
                            return;
                        }
                    }
                }
                else
                {
                    try
                    {
                        Session["StrVATAmt"] = null;
                        da.CommitTransaction();
                    }
                    catch { }
                }


                if (Session["StrVATAmt"] != null)
                {
                    if (Utility.IsNumeric(Session["StrVATAmt"].ToString().Trim()))
                    {
                        Session["StrVATAmt"] = Convert.ToString(Math.Round(Convert.ToDouble(Session["StrVATAmt"].ToString().Trim()), 2));
                    }
                    invoice.UpdateSterlingAmoutnIfCurrencyNotGBP(invoicePKID, Convert.ToDecimal(Session["StrVATAmt"]));
                }

                if (txtVATAmt.Text.Trim() != null)
                    invoice.UpdateVATAmount(invoicePKID, Convert.ToDecimal(txtVATAmt.Text.Trim()));
                //rsInvoiceHead["VATAmt"] = txtVATAmt.Text.Trim();

                CopyFile(invoicePKID.ToString());
                Session["InvoiceID"] = invoicePKID;
                hdSaveFlag.Value = "1";
                //Response.Redirect("InvoiceConfirmation.aspx",true);
                hdHideBack.Value = "1";

                if (invoicePKID > 0)
                    invoice.GetUpdateStockWithDepartmentANDNominalCode(invoicePKID, Convert.ToDecimal(lblTotal.Text));

                #region RECONCILIATION REPORT
                try
                {
                    invoice.UpdateReconciliationReport(System.Convert.ToInt32(rsInvoiceHead["SupplierCompanyID"]), System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]), "b_E_Docs");
                }
                catch { }
                #endregion
            }
        }
        #endregion

        #region CopyFile
        private void CopyFile(string iInvoiceID)
        {
            string sFname = "";
            string strFname = "";

            try
            {
                sFname = Session["InvoiceDoc"].ToString();
                strFname = sFname;
            }
            catch { }

            if (sFname.Trim() != "")
            {
                FileInfo fi = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["TempInvoicePath"]) + sFname);

                string[] strFileNameArray = sFname.Split('^');

                sFname = strFileNameArray[0];

                if (fi.Exists)
                {
                    fi.CopyTo(Server.MapPath(ConfigurationManager.AppSettings["InvoiceDocPath"]) + "\\" + iInvoiceID + "_" + sFname, true);
                    CBSolutions.ETH.Web.Invoice objInvoice = new CBSolutions.ETH.Web.Invoice();
                    objInvoice.UpdateInvoiceDocument(Convert.ToInt32(iInvoiceID.Trim()), iInvoiceID + "_" + sFname);
                    fi = null;
                    FileInfo fInfo = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["TempInvoicePath"]) + strFname.Trim());
                    if (fInfo.Exists)
                    {
                        fInfo.Delete();
                        fInfo = null;
                    }
                }
            }
        }
        #endregion

        #region btnGeneratePDF_Click
        private void btnGeneratePDF_Click(object sender, System.EventArgs e)
        {
            rptInvoice rpt = new rptInvoice(rsInvoiceHead, rsInvoiceDetail);
            rpt.PageSettings.Orientation = PageOrientation.Landscape;
            rpt.Run();
            DataDynamics.ActiveReports.Export.Pdf.PdfExport pdf = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
            string pdfFile = (string)Session["InvoicePDF"];
            pdfFile = "../Files/" + System.IO.Path.GetFileName(pdfFile);
            pdf.Export(rpt.Document, Server.MapPath(pdfFile));
            Response.Redirect(pdfFile);
        }
        #endregion

        #region IsNumericValue
        private bool IsNumericValue(string strValue)
        {
            decimal dValue = 0;
            bool bRetValue = false;
            try
            {
                dValue = Convert.ToDecimal(strValue.Trim());
                bRetValue = true;
            }
            catch { bRetValue = false; }
            return (bRetValue);
        }
        #endregion

        #region grdInvoiceDetails_ItemDataBound
        private void grdInvoiceDetails_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "New_Definable1") != DBNull.Value)
                {
                    string sColor = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "New_Definable1"));
                    ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblcolor")).Text = sColor;

                }

                string strDesc = "";
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Description") != DBNull.Value)
                {
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Description"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_LiftNo") != DBNull.Value)
                {
                    strDesc = strDesc + "Lifts/Week=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_LiftNo"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_Container") != DBNull.Value)
                {
                    strDesc = strDesc + "; Container=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_Container"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_ContractNo") != DBNull.Value)
                {
                    strDesc = strDesc + "; Contract No=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_ContractNo"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_WasteType") != DBNull.Value)
                {
                    strDesc = strDesc + "; WasteType=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_WasteType"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_ServiceDate") != DBNull.Value)
                {
                    strDesc = strDesc + "; ServiceDate=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_ServiceDate"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_StartDate") != DBNull.Value)
                {
                    strDesc = strDesc + "; StartDate=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_StartDate"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_EndDate") != DBNull.Value)
                {
                    strDesc = strDesc + "; EndDate=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_EndDate"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_Transport") != DBNull.Value)
                {
                    strDesc = strDesc + "; Transport=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_Transport"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_MinTonnage") != DBNull.Value)
                {
                    strDesc = strDesc + "; MinTonnage=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_MinTonnage"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_Disposal") != DBNull.Value)
                {
                    strDesc = strDesc + "; Disposal=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_Disposal"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_Thereafter") != DBNull.Value)
                {
                    strDesc = strDesc + "; Thereafter=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_Thereafter"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_RentalAmount") != DBNull.Value)
                {
                    strDesc = strDesc + "; RentalAmount=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_RentalAmount"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_RentalPeriod") != DBNull.Value)
                {
                    strDesc = strDesc + "; RentalPeriod=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_RentalPeriod"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_OtherAmount") != DBNull.Value)
                {
                    strDesc = strDesc + "; OtherAmount=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_OtherAmount"));
                }
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_OtherPeriod") != DBNull.Value)
                {
                    strDesc = strDesc + "; OtherPeriod=";
                    strDesc = strDesc + Convert.ToString(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Desc_OtherPeriod"));
                }

                ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblDesc")).Text = strDesc;

            }
        }
        #endregion
        #region GetColorName(string sColor)
        private string GetColorName(string sColor)
        {
            string sRetColCode = "";
            SqlDataReader dr = null;
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlConn.Open();

            SqlCommand sqlCmd = new SqlCommand("usp_GetColorDescription", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@ColorCode", sColor);
            try
            {
                dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0] != DBNull.Value)
                        sRetColCode = dr[0].ToString();
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr.Close();
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return sRetColCode;
        }
        #endregion

        #region GetAfterDecimalCalculatedValue(decimal _Value)
        public decimal GetAfterDecimalCalculatedValue(decimal _Value)
        {
            decimal originalVal = 0;
            string strValue = "";
            if (_Value < 0)
            {
                originalVal = Math.Abs(_Value);
                originalVal = HelpGetAfterDecimalCalculatedValue(originalVal);
                strValue = "-" + Convert.ToString(originalVal);
                _Value = Convert.ToDecimal(strValue);
            }
            else
            {
                _Value = HelpGetAfterDecimalCalculatedValue(_Value);
            }
            return _Value;
        }


        public decimal HelpGetAfterDecimalCalculatedValue(decimal _Value)
        {
            int Count = 0;
            double t = 0;
            decimal x = 0;
            int IntValue = 0;
            int DecVal = 0;
            string ReturnAmnt = "";
            decimal retVal = 0;
            decimal yy = 0;

            IntValue = System.Convert.ToInt32(Math.Floor(Convert.ToDouble(_Value)));
            yy = (System.Convert.ToDecimal(_Value) - System.Convert.ToDecimal(IntValue));
            x = yy * 100;
            if (x < 10)
            {
                Count = Microsoft.VisualBasic.Strings.Len(Convert.ToString(yy));
                if (Count >= 4)
                {
                    ReturnAmnt = "." + Microsoft.VisualBasic.Strings.Mid(yy.ToString(), 3, 4);
                    t = Convert.ToDouble(ReturnAmnt);
                    ReturnAmnt = "." + Microsoft.VisualBasic.Strings.Mid(t.ToString("#0.00"), 3, 2);
                    retVal = Convert.ToDecimal(ReturnAmnt);
                }
                if (Count == 1)
                    ReturnAmnt = "." + "00";

                retVal = Convert.ToDecimal(System.Convert.ToString(IntValue) + ReturnAmnt);
            }

            if (x >= 10)
            {
                string z = Microsoft.VisualBasic.Strings.Mid(x.ToString(), 4, 1);
                if (Microsoft.VisualBasic.Strings.Mid(x.ToString(), 4, 1) == "5")
                    yy = yy + Convert.ToDecimal(0.01 - 0.005);
                yy = yy * 100;
                DecVal = Convert.ToInt32(yy);
                ReturnAmnt = IntValue.ToString() + "." + DecVal.ToString();
                retVal = Convert.ToDecimal(Convert.ToDouble(ReturnAmnt).ToString("#.00"));
            }

            return (retVal);
        }
        #endregion
    }
}
