using System;
using System.Configuration;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;
using DataDynamics.ActiveReports.Export;
using DataDynamics.ActiveReports.Document;
using CBSolutions.ETH.Web.NewBuyer;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using CBSolutions.ETH.Web;
using System.Linq;



namespace JKS
{
    /// <summary>
    /// Summary description for InvoiceEdit.
    /// </summary>
    /// 
    [ScriptService]
    public partial class InvoiceEdit : SVSPage//CBSolutions.ETH.Web.ETC.VSPage
    {
        #region Web Form Controls
        protected System.Web.UI.WebControls.Panel Panel4;
        #endregion

        #region User Defined Variables
        RecordSet rsInvoiceHead = null;
        RecordSet rsInvoiceDetail = null;
        //Added by Mainak 2018-03-19
        public string ConsString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        protected string strInvoiceDocumentDownloadPath = ConfigurationManager.AppSettings["InvoiceDocPath"];
        protected string strInvoiceDocument = "";
        protected string iInvID = "";
        Invoice objInvoice = new Invoice();
        protected string strInvoiceID = "";
        protected string strInvoiceNo = "";
        protected string strSupplierCompanyID = "";
        protected string strBuyerCompanyID = "";

        protected string strDepartmentID = "";//Added by Mainak 2017-12-19
        protected string strBusinessUnitID = "";//Added by Mainak 2017-12-19

        protected string strInvoiceDate = "";
        protected string strTaxPointDate = "";
        protected string strCurrency = "";
        protected decimal decNetTotal = 0;
        protected decimal decVATAmt = 0;
        protected decimal decTotalAmt = 0;

        protected string strSupplierAddress1 = "";
        protected string strSupplierAddress2 = "";
        protected string strSupplierAddress3 = "";
        protected string strSupplierAddress4 = "";
        protected string strSupplierAddress5 = "";
        protected string strSupplierCountry = "";
        protected string strSupplierZIP = "";
        protected string strSellerVATNo = "";

        protected System.Web.UI.HtmlControls.HtmlInputHidden Hidden1;
        protected System.Web.UI.WebControls.Button btnAddLine;
        protected int invoiceID = 0;
        protected string strXmlforUpdate = "";

        protected System.Web.UI.WebControls.HyperLink HyperLink1;
        protected System.Web.UI.WebControls.TextBox txtVATAmt;
        protected System.Web.UI.WebControls.Label lblVAT;

        public string strforDelete = "";

        int currentYear = 0;

        DateTime InvDate;
        DateTime TaxDate;

        protected int iPassToJS = 0;
        protected string iUrlJS = "";
        protected int iPressCalculate = 0;

        public int vRFlag = 0;//Added by Mainak 2018-09-10
        Invoice_NL_CN objCN = new Invoice_NL_CN();//Added by Mainak 2018-08-11
        #endregion

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {

            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            btnDelete.Attributes.Add("onclick", "if(CheckDeleteAll()){ return confirm('Are you sure you want to DELETE the line(s)?'); }else{ return false; }");

            //btnSave.Attributes.Add("onclick", "if(CheckComment()){ return confirm('Are you sure you wish to amend the data? If so, after saving you should double-check that the invoice values match to the original scanned image, particularly the VAT rounding.'); }else{ return false; }");

            baseUtil.keepAlive(this);

            if (Request["hd"] != null)
            {
                hdHideBack.Value = "1";
            }

            Session.Remove("DuplicateInvoice");

            invoiceID = 0;
            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
                ViewState["INID"] = invoiceID.ToString();
                iInvID = invoiceID.ToString();
                hdHideBack.Value = "1";

                try
                {
                    Session["StrVATAmt"] = null;

                }
                catch { }
            }

            if (!IsPostBack)
            {

                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]));
                PopulateData(invoiceID);

            }

        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            //currentYear = Microsoft.VisualBasic.DateAndTime.Year(System.DateTime.Now);
            currentYear = System.DateTime.Now.Year;
            for (int i = 1980; i <= (currentYear + 10); i++)
            {
                cboYearInvoiceDate.Items.Add(i.ToString());
                cboYearTaxPointDate.Items.Add(i.ToString());
            }
            //for(int i=1;i<13;i++)
            for (int i = 0; i < 12; i++)
            {
                cboMonthInvoiceDate.Items.Add(new ListItem(
                    //Microsoft.VisualBasic.DateAndTime.MonthName(i,true), i.ToString())
                    // System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i]));
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[i], (i + 1).ToString()));
                cboMonthTaxPointDate.Items.Add(new ListItem(
                    //Microsoft.VisualBasic.DateAndTime.MonthName(i,true), i.ToString()));
                    // System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i]));
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[i], (i + 1).ToString()));
            }
            for (int i = 1; i < 32; i++)
            {
                cboDayInvoiceDate.Items.Add(i.ToString());
                cboDayTaxPointDate.Items.Add(i.ToString());
            }

            cboDayInvoiceDate.Items.Insert(0, new ListItem("Day", "0"));
            cboDayTaxPointDate.Items.Insert(0, new ListItem("Day", "0"));

            cboMonthInvoiceDate.Items.Insert(0, new ListItem("Month", "0"));
            cboMonthTaxPointDate.Items.Insert(0, new ListItem("Month", "0"));

            cboYearInvoiceDate.Items.Insert(0, new ListItem("Year", "0"));
            cboYearTaxPointDate.Items.Insert(0, new ListItem("Year", "0"));

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
            this.ddlSupplier.SelectedIndexChanged += new System.EventHandler(this.ddlSupplier_SelectedIndexChanged);
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
            //this.grdInvoiceDetails.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvoiceDetails_ItemDataBound);
            this.grdInvoiceDetails.RowDataBound += new GridViewRowEventHandler(grdInvoiceDetails_RowDataBound);

            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            this.inpAddLine.ServerClick += new System.EventHandler(this.inpAddLine_ServerClick);
            this.Load += new System.EventHandler(this.Page_Load);
            this.txtSupplier.TextChanged += new EventHandler(this.txtSupplier_TextChanged);

            //this.btnAddModal.Click +=new System.EventHandler(this.btnAddModal_Click);
            this.btnReplaceModal.Click += new System.EventHandler(this.btnReplaceModal_Click);

        }
        #endregion

        protected void txtSupplier_TextChanged(object sender, EventArgs e)
        {
            Company objCompany = new Company();
            if (HdSupplierId.Value != "")
            {
                lblVATRegNo.Text = objCompany.GetSupplierVatNo(Convert.ToInt32(HdSupplierId.Value));
                PopulateSuppAddress(Convert.ToInt32(HdSupplierId.Value), lblVATRegNo.Text);
            }
        }

        #region PopulateData
        public void PopulateData(int invoiceID)
        {
            rsInvoiceHead = Invoice.GetInvoiceHead(invoiceID);
            rsInvoiceDetail = Invoice.GetInvoiceDetail(invoiceID);
            lblConfirmation.Visible = false;

            //Added by Mainak 2018-03-19
            //Load Business unit Dropdown
            GetBusinessUnit(Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]));
            //Load Department Dropdown
            LoadDepartment(Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]));

            LoadData();
            PopulateHeaderFirstTime();
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

        #region PopulateHeaderFirstTime
        public void PopulateHeaderFirstTime()
        {
            DataSet dsInvoiceHead = new DataSet();
            dsInvoiceHead = rsInvoiceHead.ParentDataSet;
            dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
            dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());

            DateTime dt;

            txtInvoiceNo.Text = rsInvoiceHead["InvoiceNo"].ToString();

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

            if (rsInvoiceHead["New_InvoiceContact"] != DBNull.Value)
                lblCustomerContactName.Text = rsInvoiceHead["New_InvoiceContact"].ToString();
            if (rsInvoiceHead["New_SettlementDays1"] != DBNull.Value)
                lblSettlementDays.Text = rsInvoiceHead["New_SettlementDays1"].ToString();
            if (rsInvoiceHead["New_SettlementDays2"] != DBNull.Value)
                lblSecondSettlementDays.Text = rsInvoiceHead["New_SettlementDays2"].ToString();

            if (rsInvoiceHead["New_InvoiceName"] != DBNull.Value)
                lblInvoiceName.Text = rsInvoiceHead["New_InvoiceName"].ToString();


            RecordSet rsCurrency = Invoice.GetCurrencyTypesList();
            while (!rsCurrency.EOF())
            {
                ListItem listItem = new ListItem(rsCurrency["CurrencyCode"].ToString(), rsCurrency["CurrencyTypeID"].ToString());
                cboCurrencyType.Items.Add(listItem);
                rsCurrency.MoveNext();
            }

            try
            {
                cboCurrencyType.Items.Insert(0, new ListItem("Select Currency Code", "0"));

            }
            catch { }

            try
            {
                if (rsInvoiceHead["CurrencyTypeID"] != DBNull.Value)
                {
                    lblCurrency.Text = objInvoice.GetCurrencyName(Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]));
                    cboCurrencyType.SelectedValue = rsInvoiceHead["CurrencyTypeID"].ToString();
                }
            }
            catch { }

            lblVATRegNo.Text = rsInvoiceHead["SellerVATNo"].ToString();

            if (rsInvoiceHead["InvoiceDate"] != DBNull.Value)
            {
                dt = System.Convert.ToDateTime(rsInvoiceHead["InvoiceDate"]);
                cboYearInvoiceDate.SelectedValue = dt.Year.ToString();
                cboMonthInvoiceDate.SelectedIndex = dt.Month;
                cboDayInvoiceDate.SelectedIndex = dt.Day;
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
            {
                dt = System.Convert.ToDateTime(rsInvoiceHead["TaxPointDate"]);
                cboYearTaxPointDate.SelectedValue = dt.Year.ToString();
                cboMonthTaxPointDate.SelectedIndex = dt.Month;
                cboDayTaxPointDate.SelectedIndex = dt.Day;
            }

            txtNetTotal.Text = System.Convert.ToDouble(rsInvoiceHead["NetTotal"]).ToString("#0.00");

            txtTotAmt.Text = System.Convert.ToDouble(rsInvoiceHead["TotalAmt"]).ToString("#0.00");


            txtVATAmtNew.Text = System.Convert.ToDouble(rsInvoiceHead["VATAmt"]).ToString("#0.00");

            if (rsInvoiceHead["amountifnotgbpcurrency"] != DBNull.Value)
            {
                lblGBPEquiAmt.Text = System.Convert.ToDouble(rsInvoiceHead["amountifnotgbpcurrency"]).ToString("#0.00");
                lblGBPEquiAmt.Visible = true;
            }

            string s = GetAddressLine(rsInvoiceHead["DeliveryAddress1"].ToString());
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
            lblDeliveryAddress.Text = s;

            if (rsInvoiceHead["New_OverallDiscountPercent"] != DBNull.Value)
                lblOverAllDiscount.Text = rsInvoiceHead["New_OverallDiscountPercent"].ToString().Trim();

            s = GetAddressLine(rsInvoiceHead["InvoiceAddress1"].ToString());
            s += GetAddressLine(rsInvoiceHead["InvoiceAddress2"].ToString());
            s += GetAddressLine(rsInvoiceHead["InvoiceAddress3"].ToString());
            s += GetAddressLine(rsInvoiceHead["InvoiceAddress4"].ToString());
            s += GetAddressLine(rsInvoiceHead["InvoiceAddress5"].ToString());
            s += GetAddressLine(rsInvoiceHead["InvoiceCountry"].ToString());
            s += GetAddressLine(rsInvoiceHead["InvoiceZIP"].ToString());
            try
            {
                s = s.Substring(0, s.Length - 3);
            }
            catch { }
            if (rsInvoiceHead["New_PaymentDate"] != DBNull.Value)
                lblpaymentdate.Text = System.Convert.ToDateTime(rsInvoiceHead["New_PaymentDate"]).ToString();
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

            lblInvoiceAddress.Text = s;

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
            lblSupplierAddress.Text = s;
            /*
                        RecordSet rs = Company.GetCompanyData( System.Convert.ToInt32( rsInvoiceHead["BuyerCompanyID"]));
		
                        ddlCompany.SelectedValue=rsInvoiceHead["BuyerCompanyID"].ToString();
                        if(rsInvoiceHead["BuyerCompanyID"].ToString()!="")
                        {
                            ddlSupplier.DataSource = objInvoice.GetSuppliersListNEW(Convert.ToInt32(ddlCompany.SelectedValue));
                            ddlSupplier.DataBind();
                        }
			
                        rs =Company.GetCompanyData( System.Convert.ToInt32( rsInvoiceHead["SupplierCompanyID"]));
                        lblSupplier.Text  = rs["CompanyName"].ToString() ;
                        ddlSupplier.SelectedValue=rsInvoiceHead["SupplierCompanyID"].ToString();	
                        */
            /*********************************** Add by Rinku Santra 10-10-2011 **********************************************************/
            RecordSet rs = Company.GetCompanyData(System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]));

            ddlCompany.SelectedValue = rsInvoiceHead["BuyerCompanyID"].ToString();
            if (rsInvoiceHead["BuyerCompanyID"].ToString() != "")
            {
                //ddlSupplier.DataSource = objInvoice.GetSuppliersListNEW(Convert.ToInt32(ddlCompany.SelectedValue));
                //ddlSupplier.DataBind();
                //ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
            }

            ViewState["BuyerCID"] = rsInvoiceHead["BuyerCompanyID"].ToString();

            rs = Company.GetCompanyData(System.Convert.ToInt32(rsInvoiceHead["SupplierCompanyID"]));
            lblSupplier.Text = rs["CompanyName"].ToString();

            //----- Added by Subha Das. 8th January 2015 for Autocomplete supplier 
            txtSupplier.Text = rs["CompanyName"].ToString();
            HdSupplierId.Value = rsInvoiceHead["SupplierCompanyID"].ToString();
            //-----Added section end.

            if (ddlSupplier.Items.FindByValue(Convert.ToString(rsInvoiceHead["SupplierCompanyID"])) != null)
            {
                // ddlSupplier.SelectedValue = rsInvoiceHead["SupplierCompanyID"].ToString();
            }

            /*********************************** Add by Rinku Santra 10-10-2011 **********************************************************/


            if (rsInvoiceHead["StatusId"] != DBNull.Value)
                lblStatus.Text = Invoice.GetStatus((int)rsInvoiceHead["StatusId"]);
            else
                lblStatus.Text = "Pending";


            //Added by Mainak 2017-12-18           
            ddldept.SelectedValue = rsInvoiceHead["DepartmentID"].ToString();
            ddlBusinessUnit.SelectedValue = rsInvoiceHead["BusinessUnitID"].ToString();

            if (ViewState["INID"] != null)
            {
                Double dGBPEquivalentAmount = 0;
                dGBPEquivalentAmount = objInvoice.GetGBPEquivalentAmount(Convert.ToInt32(ViewState["INID"]));
                if (dGBPEquivalentAmount != 0)
                {
                    lblGBPEquiAmt.Visible = true;
                    hdGBPEquiFlag.Value = "1";
                    lblGBPEquiAmt.Text = dGBPEquivalentAmount.ToString();
                }
            }
            if (Session["StrVATAmt"] != null)
            {
                lblGBPEquiAmt.Visible = true;
                hdGBPEquiFlag.Value = "1";
                lblGBPEquiAmt.Text = Session["StrVATAmt"].ToString();
                txtSterlingEquivalent.Visible = false;
            }

            if (string.IsNullOrEmpty(rsInvoiceHead["CurrencyTypeID"].ToString()))
            {
                rsInvoiceHead["CurrencyTypeID"] = 0;
            }

            if (Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]) != 22 && Session["StrVATAmt"] == null && ViewState["INID"] == null)
            {
                txtSterlingEquivalent.Visible = true;
                trInputSterlingEquiAmt.Visible = true;
            }
            else
                trInputSterlingEquiAmt.Visible = false;
        }
        #endregion

        #region PopulateData
        public void PopulateData()
        {
            Invoice objInvoice = new Invoice();
            #region DATATABLE
            DataTable DT = new DataTable("LineItems");

            DT.Columns.Add("PurOrderNo", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("BuyersProdCode", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("Description", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("Quantity", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("Rate", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_NettValue", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("VATRate", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("VATAmt", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("TotalAmt", typeof(decimal));//Modified by Mainak 2018-10-26


            //DT.Columns.Add("POS", typeof(int));//position
            //DT.Columns.Add("PAGE", typeof(int));//page
            //DT.Columns.Add("GOODSRECDDETAILID", typeof(int));
            //DT.Columns.Add("DEPARTMENTID", typeof(int));
            //DT.Columns.Add("NOMINALCODEID", typeof(int));
            //DT.Columns.Add("BUSINESSUNITID", typeof(int));
            //DT.Columns.Add("PROJECTCODE", typeof(int)); //Added by Mainak 2017-11-21





            DT.Columns.Add("PurOrderLineNo", typeof(int)); //Added by Mainak 2018-05-31


            DT.Columns.Add("New_OverallDiscountValue", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("GrossAmt", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("SerialNo", typeof(int));//Modified by Mainak 2018-10-26
            DT.Columns.Add("PurOrderDate", typeof(DateTime));   //Modified by Mainak 2018-10-26
            DT.Columns.Add("New_DespatchDate", typeof(DateTime));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_DespatchNoteNumber", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("SuppliersProdCode", typeof(string));//Modified by Mainak 2018-10-26




            DT.Columns.Add("VATTypeID", typeof(byte));//Modified by Mainak 2018-10-26
            DT.Columns.Add("UOM", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_Type", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_Definable1", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_Definable2", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_DiscountValue1", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_DiscountPercent2", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_DiscountValue2", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_OverAllDiscountValue", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_ModeOfTransport", typeof(int));//Modified by Mainak 2018-10-26





            DT.Columns.Add("New_NatureOfTransaction", typeof(int));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_TermsOfDelivery", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_CommodityCode", typeof(int));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_SupplementaryUnits", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_NettMass", typeof(decimal));//Modified by Mainak 2018-10-26
            //DT.Columns.Add("New_DespatchNoteNumber");
            DT.Columns.Add("InvoiceID", typeof(int));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_PostDiscountValue", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_TaxCode", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_AccountingUnit", typeof(string));//Modified by Mainak 2018-10-26



            DT.Columns.Add("New_GLCompanyID", typeof(int));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_InvoiceSubAccount", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("New_Account", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("VATCode", typeof(string));//Modified by Mainak 2018-10-26
            DT.Columns.Add("Discount", typeof(decimal));//Modified by Mainak 2018-10-26
            DT.Columns.Add("InvoiceDetailID", typeof(int));//Modified by Mainak 2018-10-26 
            DT.Columns.Add("ModDate", typeof(DateTime));
            DT.Columns.Add("New_CountryOfOrigin", typeof(string));
            //DT.Columns.Add("ModStamp", typeof(string));
            #endregion

            RecordSet rs2 = null;
            if (System.IO.File.Exists(Session["XMLInvoiceDetailFile"].ToString()) && Convert.ToInt32(Session["EditInvID"]) == Convert.ToInt32(Request.QueryString["InvoiceID"]))
            {

                DataSet ds2 = new DataSet();
                ds2.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds2.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                rs2 = new RecordSet(ds2);

                this.grdInvoiceDetails.DataSource = ds2;
                this.grdInvoiceDetails.DataBind();
                ViewState["LineItemsDT"] = DT;//Added by Mainak 2018-10-15
            }
            else
            {
                DataSet ds = new DataSet();
                ds = rsInvoiceDetail.ParentDataSet;
                ds.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
                this.grdInvoiceDetails.DataSource = ds;
                this.grdInvoiceDetails.DataBind();
                ViewState["LineItemsDT"] = DT;//Added by Mainak 2018-10-15
                Session["EditInvID"] = Request.QueryString["InvoiceID"];

                string sXMLPath = Session["XMLInvoiceDetailFile"].ToString();
                StringBuilder oBuilder = new StringBuilder();
                StringWriter oStringWriter = new StringWriter(oBuilder);
                XmlTextReader oXmlReader = new XmlTextReader(sXMLPath);
                XmlTextWriter oXmlWriter = new XmlTextWriter(oStringWriter);
                while (oXmlReader.Read())
                {
                    oXmlWriter.WriteNode(oXmlReader, true);
                }
                oXmlReader.Close();
                oXmlWriter.Close();

                Session["strXmlforUpdate"] = oBuilder.ToString();
            }
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

        #region CheckDate
        public bool CheckDate(int year, int month, int day)
        {
            bool retValue = true;
            if (year != 0 && month != 0 && day != 0)
            {
                try
                {
                    DateTime dt = new DateTime(year, month, day);
                }
                catch
                {
                    retValue = false;
                }
            }
            return retValue;
        }
        #endregion

        #region CheckInvoiceDate
        public bool CheckInvoiceDate(int year, int month, int day)
        {
            bool retValue = true;

            try
            {
                DateTime dt = new DateTime(year, month, day);
            }
            catch { retValue = false; }
            return retValue;
        }
        #endregion

        #region GetCompanyListForPurchaseInvoiceLog
        private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID)
        {
            Company objCompany = new Company();
            ddlCompany.DataSource = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(iCompanyID), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
            ddlCompany.DataBind();
            ddlSupplier.DataSource = objInvoice.GetSuppliersListNEW(Convert.ToInt32(Session["CompanyID"]));
            ddlSupplier.DataBind();

        }
        #endregion

        #region grdInvoiceDetails_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        private void grdInvoiceDetails_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlVATRate = (DropDownList)e.Row.FindControl("dpVATRate");
                if (ddlVATRate != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        RecordSet rs = CBSolutions.ETH.Web.Invoice.GetVATTypesList();
                        while (!rs.EOF())
                        {
                            ListItem listItem = new ListItem(rs["Rate"].ToString(), rs["Rate"].ToString());
                            ddlVATRate.Items.Add(listItem);
                            rs.MoveNext();
                        }
                        ddlVATRate.Items.Insert(0, new ListItem("Select", "Select"));
                    }
                }

                //Added by Mainak 2018-08-09 
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PurOrderLineNo") != DBNull.Value)
                {
                    string strPOlineNONo = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PurOrderLineNo"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtPOLineNo")).Text = strPOlineNONo.ToString();
                }

                //Added by kuntalkarar on 9thMarch2017
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "BuyersProdCode") != DBNull.Value)
                {
                    string strOrderNo = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "BuyersProdCode"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtBuyersProdCode")).Text = strOrderNo.ToString();
                }



                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PurOrderNo") != DBNull.Value)
                {
                    string strOrderNo = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PurOrderNo"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtPurOrderNo")).Text = strOrderNo.ToString();
                }
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Description") != DBNull.Value)
                {
                    string strDesc = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Description"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtDesc")).Text = strDesc.ToString();
                }
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Rate") != DBNull.Value)
                {
                    string strRate = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Rate"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtPrice")).Text = String.Format(strRate, "0:N2");
                }
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Quantity") != DBNull.Value)
                {
                    string strQuantity = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Quantity"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtQuantity")).Text = String.Format(strQuantity, "0:N2");
                }
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "New_NettValue") != DBNull.Value)
                {
                    string strNew_NettValue = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "New_NettValue"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtNetValue")).Text = String.Format(strNew_NettValue, "0:N2");
                }
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "VATRate") != DBNull.Value)
                {
                    string strVAT = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "VATRate"));
                    ((System.Web.UI.WebControls.DropDownList)e.Row.FindControl("dpVATRate")).SelectedValue = String.Format(strVAT, "0:N2");
                }
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "VATAmt") != DBNull.Value)
                {
                    string strVAT = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "VATAmt"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtVAT")).Text = String.Format(strVAT, "0:N2");
                }
                if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "TotalAmt") != DBNull.Value)
                {
                    string strTotalAmt = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                    ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtGrossValue")).Text = String.Format(strTotalAmt, "0:N2");
                }
            }
        }

        #endregion

        #region btnSave_Click(object sender, System.EventArgs e)
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (HdSupplierId.Value == "")
            {
                Response.Write("<script>alert('Please select supplier.');</script>");
                txtSupplier.Focus();
                return;
            }

            strInvoiceID = Request.QueryString["InvoiceID"];
            int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            strInvoiceNo = txtInvoiceNo.Text;
            //strSupplierCompanyID = ddlSupplier.SelectedValue;

            //----- Added by Subha Das. 8th January 2015 for Autocomplete supplier 
            strSupplierCompanyID = HdSupplierId.Value.Trim().ToString();
            //-----Added section end.

            strBuyerCompanyID = ddlCompany.SelectedValue;

            strDepartmentID = ddldept.SelectedValue;//Added by Mainak 2018-03-19
            strBusinessUnitID = ddlBusinessUnit.SelectedValue;//Added by Mainak 2018-03-19

            lblErrorInvoiceDate.Text = "";
            lblErrorTaxPointDate.Text = "";
            if (CheckInvoiceDate(System.Convert.ToInt32(cboYearInvoiceDate.SelectedValue),
                System.Convert.ToInt32(cboMonthInvoiceDate.SelectedValue),
                System.Convert.ToInt32(cboDayInvoiceDate.SelectedValue)) == false)
            {
                lblErrorInvoiceDate.Text = "Invalid Date Format";
                return;
            }
            if (CheckDate(System.Convert.ToInt32(cboYearTaxPointDate.SelectedValue),
                System.Convert.ToInt32(cboMonthTaxPointDate.SelectedValue),
                System.Convert.ToInt32(cboDayTaxPointDate.SelectedValue)) == false)
            {
                lblErrorTaxPointDate.Text = "Invalid Date Format";
                return;
            }


            if (cboYearInvoiceDate.SelectedValue != "0" && cboMonthInvoiceDate.SelectedValue != "0" && cboDayInvoiceDate.SelectedValue != "0")
            {
                InvDate = new DateTime(Convert.ToInt32(cboYearInvoiceDate.SelectedValue), Convert.ToInt32(cboMonthInvoiceDate.SelectedValue), Convert.ToInt32(cboDayInvoiceDate.SelectedValue));
            }

            if (cboYearTaxPointDate.SelectedValue != "0" && cboMonthTaxPointDate.SelectedValue != "0" && cboDayTaxPointDate.SelectedValue != "0")
            {
                TaxDate = new DateTime(Convert.ToInt32(cboYearTaxPointDate.SelectedValue), Convert.ToInt32(cboMonthTaxPointDate.SelectedValue), Convert.ToInt32(cboDayTaxPointDate.SelectedValue));
            }
            else
            {
                TaxDate = new DateTime(1900, 1, 1);
            }

            strCurrency = cboCurrencyType.SelectedValue;
            decNetTotal = Convert.ToDecimal(txtNetTotal.Text);
            decVATAmt = Convert.ToDecimal(txtVATAmtNew.Text);
            decTotalAmt = Convert.ToDecimal(txtTotAmt.Text);

            if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead = new RecordSet(ds);

                strSupplierAddress1 = rsHead["SupplierAddress1"].ToString();
                strSupplierAddress2 = rsHead["SupplierAddress2"].ToString();
                strSupplierAddress3 = rsHead["SupplierAddress3"].ToString();
                strSupplierAddress4 = rsHead["SupplierAddress4"].ToString();
                strSupplierAddress5 = rsHead["SupplierAddress5"].ToString();
                strSupplierCountry = rsHead["SupplierCountry"].ToString();
                strSupplierZIP = rsHead["SupplierZIP"].ToString();
                strSellerVATNo = lblVATRegNo.Text.ToString();
            }
            string strEDITEDdetailXML = UpdateInvoiceDetailinXML();
            if (strEDITEDdetailXML == "")
            {
                return;
            }

            if (lblstrDelete.Text != "")
            {
                /*Blocked by kuntalkarar on 30thMay2017*/
                //lblstrDelete.Text = lblstrDelete.Text.Substring(0, lblstrDelete.Text.Length - 1);
                /*Added by kuntalkarar on 30thMay2017*/
                //lblstrDelete.Text = lblstrDelete.Text + grdInvoiceDetails.DataKeys[0].Value.ToString();
                strforDelete = lblstrDelete.Text.Substring(0, lblstrDelete.Text.Length - 1);

            }
            /*Added by kuntalkarar on 25thJuly2017*/
            else if (Session["DeletedInvoiceIds"] != null && !String.IsNullOrEmpty(Session["DeletedInvoiceIds"].ToString()))
            {
                strforDelete = Session["DeletedInvoiceIds"].ToString().Substring(0, Session["DeletedInvoiceIds"].ToString().Length - 1);
            }
            //------------------------------------
            else
            {
                strforDelete = "0";
            }

            //Added by Mainak 2018-09-28
            int SID = 0;
            if (HdSupplierId.Value != "")
            {
                SID = Convert.ToInt32(HdSupplierId.Value);
            }



            string getvendorClass = "SELECT TradingRelation.New_VendorClass FROM TradingRelation INNER JOIN Invoice ON TradingRelation.BuyerCompanyID = Invoice.BuyerCompanyID AND TradingRelation.SupplierCompanyID = Invoice.SupplierCompanyID where Invoice.invoiceId=" + InvID + " and Invoice.BuyerCompanyID=" + Convert.ToInt32(ViewState["BuyerCID"].ToString()) + " and Invoice.SupplierCompanyID=" + SID + "";
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter(getvendorClass, sqlConn);
            DataTable dtNewVc = new DataTable();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(dtNewVc);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }


            decimal NetValue = 0;
            decimal LineVAT = 0;
            decimal VatRate = 0;

            int vFlag = 0;
            int vRInfiFlag = 0;
            int vRinfinityFlag = 0;

            DataTable dtLVt = objCN.GetLineVatByBuyerCompanyId(Convert.ToInt32(ViewState["BuyerCID"]));
            DataTable dtTXRt = objCN.GetTaxRateByBuyerCompanyId(Convert.ToInt32(ViewState["BuyerCID"]));


            for (int i = 0; i <= grdInvoiceDetails.Rows.Count - 1; i++)
            {
                NetValue = 0;
                if (((System.Web.UI.WebControls.TextBox)grdInvoiceDetails.Rows[i].FindControl("txtNetValue")).Text != "")
                {
                    if ((Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdInvoiceDetails.Rows[i].FindControl("txtNetValue")).Text) > 0) || (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdInvoiceDetails.Rows[i].FindControl("txtNetValue")).Text) < 0))
                    {
                        NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdInvoiceDetails.Rows[i].FindControl("txtNetValue")).Text);
                    }
                }

                LineVAT = 0;

                if (((System.Web.UI.WebControls.TextBox)grdInvoiceDetails.Rows[i].FindControl("txtVAT")).Text != "")
                {
                    if ((Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdInvoiceDetails.Rows[i].FindControl("txtVAT")).Text) > 0) || (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdInvoiceDetails.Rows[i].FindControl("txtVAT")).Text) < 0))
                    {
                        LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdInvoiceDetails.Rows[i].FindControl("txtVAT")).Text);
                    }
                }

                if (NetValue != 0)
                {
                    VatRate = Math.Round((LineVAT / NetValue), 2);

                }
                else
                {
                    vRinfinityFlag++;
                }

                if (vRinfinityFlag == 0)
                {
                    if (dtTXRt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtTXRt.Rows.Count; j++)
                        {
                            if ((VatRate == Convert.ToDecimal(dtTXRt.Rows[j]["TaxRate"])) || ((VatRate == Convert.ToDecimal(dtTXRt.Rows[j]["TaxRate"])) && (vFlag > 0)))
                            {
                                vFlag = 0;
                                break;
                            }
                            else
                                vFlag++;

                        }
                        if (vFlag > 0)
                        {
                            vRFlag++;
                        }
                    }
                }

                if (vRinfinityFlag > 0)
                {
                    vRInfiFlag++;
                    vRinfinityFlag = 0;
                }
            }

            if (dtLVt.Rows.Count > 0)
            {
                if ((dtLVt.Rows[0]["LineVAT"].ToString()).ToLower() == "true")
                {
                    vRFlag = vRFlag;
                }
                else
                {
                    vRFlag = 0;
                }
            }
            else
            {
                vRFlag = 0;
            }
            int iRetValue = 0;
            if (vRInfiFlag == 0)
            {
                if (dtNewVc.Rows.Count > 0)
                {
                    if (dtNewVc.Rows[0]["New_VendorClass"].ToString() == "PO")
                    {
                        if (vRFlag == 0)
                        {
                            #region btnSave

                            iRetValue = objInvoice.SaveInvoiceDatatoDataBase(Convert.ToInt32(Session["UserID"]), InvID, strInvoiceNo, strSupplierCompanyID, strBuyerCompanyID,
                                       InvDate, TaxDate, strCurrency, strSupplierAddress1, strSupplierAddress2, strSupplierAddress3, strSupplierAddress4, strSupplierAddress5, strSupplierCountry,
                                       strSupplierZIP, strSellerVATNo, txtComments.Text.ToString(), strEDITEDdetailXML, decNetTotal, decVATAmt, decTotalAmt, strforDelete, strDepartmentID, strBusinessUnitID);//Modified by Mainak 2018-03-19

                            if (iRetValue > 0)
                            {

                                if (Request.QueryString["IsParentReload"] != null && Convert.ToInt32(Request.QueryString["IsParentReload"].ToString()) == 1)
                                {
                                    // If Condition Added by Mrinal on 3rd January 2015
                                    Response.Write("<script>opener.location.reload();</script>");
                                    Response.Write("<script>self.close();</script>");
                                }
                                else
                                {
                                    if (Request.QueryString["AllowEdit"].ToString() == "Current")
                                    {

                                        /*Added by kuntalkarar on 1stJune2017*/
                                        DataTable dt = new DataTable();
                                        dt = objInvoice.GetInvoiceDetails(InvID);
                                        grdInvoiceDetails.DataSource = dt;
                                        grdInvoiceDetails.DataBind();

                                        //blocked by kuntalkarar on 25thJuly2017
                                        /*Added by kuntalkarar on 2ndJune2017*/
                                        /*DataSet dstempInvoiceDetail = new DataSet();
                                        dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                        int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                        dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                        dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                        //-------------------------------------------------------------------------

                                        /*Added by kuntalkarar on 25thJuly2017*/
                                        DataSet dstempInvoiceDetail = new DataSet();
                                        dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.Tables[0].Merge(dt);
                                        dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());

                                        /*Added by kuntalkarar on 25thJuly2017*/
                                        Session["DeletedInvoiceIds"] = "";
                                        Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../Current/CurrentStatus.aspx';</script>");

                                    }
                                    else if (Request.QueryString["AllowEdit"].ToString() == "StockQC")
                                        Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../StockQC/CurrentInvoice.aspx';</script>");
                                    else if (Request.QueryString["AllowEdit"].ToString() == "History")
                                    {
                                        /*Added by kuntalkarar on 6thJune2017*/
                                        DataTable dt = new DataTable();
                                        dt = objInvoice.GetInvoiceDetails(InvID);
                                        grdInvoiceDetails.DataSource = dt;
                                        grdInvoiceDetails.DataBind();

                                        //blocked by kuntalkarar on 25thJuly2017
                                        /*Added by kuntalkarar on 6thJune2017*/
                                        /*DataSet dstempInvoiceDetail = new DataSet();
                                        dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                        int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                        dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                        dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                        //-------------------------------------------------------------------------
                                        /*Added by kuntalkarar on 25thJuly2017*/
                                        DataSet dstempInvoiceDetail2 = new DataSet();
                                        dstempInvoiceDetail2.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail2.Tables[0].Merge(dt);
                                        dstempInvoiceDetail2.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
                                        /*Added by kuntalkarar on 25thJuly2017*/
                                        Session["DeletedInvoiceIds"] = "";
                                        Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../History/history.aspx';</script>");
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            string message = "alert('VAT ÷ Net does not equal a valid tax rate.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                    }
                    else
                    {
                        #region btnSave

                        iRetValue = objInvoice.SaveInvoiceDatatoDataBase(Convert.ToInt32(Session["UserID"]), InvID, strInvoiceNo, strSupplierCompanyID, strBuyerCompanyID,
                                   InvDate, TaxDate, strCurrency, strSupplierAddress1, strSupplierAddress2, strSupplierAddress3, strSupplierAddress4, strSupplierAddress5, strSupplierCountry,
                                   strSupplierZIP, strSellerVATNo, txtComments.Text.ToString(), strEDITEDdetailXML, decNetTotal, decVATAmt, decTotalAmt, strforDelete, strDepartmentID, strBusinessUnitID);//Modified by Mainak 2018-03-19

                        if (iRetValue > 0)
                        {

                            if (Request.QueryString["IsParentReload"] != null && Convert.ToInt32(Request.QueryString["IsParentReload"].ToString()) == 1)
                            {
                                // If Condition Added by Mrinal on 3rd January 2015
                                Response.Write("<script>opener.location.reload();</script>");
                                Response.Write("<script>self.close();</script>");
                            }
                            else
                            {
                                if (Request.QueryString["AllowEdit"].ToString() == "Current")
                                {

                                    /*Added by kuntalkarar on 1stJune2017*/
                                    DataTable dt = new DataTable();
                                    dt = objInvoice.GetInvoiceDetails(InvID);
                                    grdInvoiceDetails.DataSource = dt;
                                    grdInvoiceDetails.DataBind();

                                    //blocked by kuntalkarar on 25thJuly2017
                                    /*Added by kuntalkarar on 2ndJune2017*/
                                    /*DataSet dstempInvoiceDetail = new DataSet();
                                    dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                    int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                    dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                    dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                    //-------------------------------------------------------------------------

                                    /*Added by kuntalkarar on 25thJuly2017*/
                                    DataSet dstempInvoiceDetail = new DataSet();
                                    dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.Tables[0].Merge(dt);
                                    dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());

                                    /*Added by kuntalkarar on 25thJuly2017*/
                                    Session["DeletedInvoiceIds"] = "";
                                    Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../Current/CurrentStatus.aspx';</script>");

                                }
                                else if (Request.QueryString["AllowEdit"].ToString() == "StockQC")
                                    Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../StockQC/CurrentInvoice.aspx';</script>");
                                else if (Request.QueryString["AllowEdit"].ToString() == "History")
                                {
                                    /*Added by kuntalkarar on 6thJune2017*/
                                    DataTable dt = new DataTable();
                                    dt = objInvoice.GetInvoiceDetails(InvID);
                                    grdInvoiceDetails.DataSource = dt;
                                    grdInvoiceDetails.DataBind();

                                    //blocked by kuntalkarar on 25thJuly2017
                                    /*Added by kuntalkarar on 6thJune2017*/
                                    /*DataSet dstempInvoiceDetail = new DataSet();
                                    dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                    int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                    dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                    dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                    //-------------------------------------------------------------------------
                                    /*Added by kuntalkarar on 25thJuly2017*/
                                    DataSet dstempInvoiceDetail2 = new DataSet();
                                    dstempInvoiceDetail2.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail2.Tables[0].Merge(dt);
                                    dstempInvoiceDetail2.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
                                    /*Added by kuntalkarar on 25thJuly2017*/
                                    Session["DeletedInvoiceIds"] = "";
                                    Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../History/history.aspx';</script>");
                                }
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    #region btnSave

                    iRetValue = objInvoice.SaveInvoiceDatatoDataBase(Convert.ToInt32(Session["UserID"]), InvID, strInvoiceNo, strSupplierCompanyID, strBuyerCompanyID,
                               InvDate, TaxDate, strCurrency, strSupplierAddress1, strSupplierAddress2, strSupplierAddress3, strSupplierAddress4, strSupplierAddress5, strSupplierCountry,
                               strSupplierZIP, strSellerVATNo, txtComments.Text.ToString(), strEDITEDdetailXML, decNetTotal, decVATAmt, decTotalAmt, strforDelete, strDepartmentID, strBusinessUnitID);//Modified by Mainak 2018-03-19

                    if (iRetValue > 0)
                    {

                        if (Request.QueryString["IsParentReload"] != null && Convert.ToInt32(Request.QueryString["IsParentReload"].ToString()) == 1)
                        {
                            // If Condition Added by Mrinal on 3rd January 2015
                            Response.Write("<script>opener.location.reload();</script>");
                            Response.Write("<script>self.close();</script>");
                        }
                        else
                        {
                            if (Request.QueryString["AllowEdit"].ToString() == "Current")
                            {

                                /*Added by kuntalkarar on 1stJune2017*/
                                DataTable dt = new DataTable();
                                dt = objInvoice.GetInvoiceDetails(InvID);
                                grdInvoiceDetails.DataSource = dt;
                                grdInvoiceDetails.DataBind();

                                //blocked by kuntalkarar on 25thJuly2017
                                /*Added by kuntalkarar on 2ndJune2017*/
                                /*DataSet dstempInvoiceDetail = new DataSet();
                                dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                //-------------------------------------------------------------------------

                                /*Added by kuntalkarar on 25thJuly2017*/
                                DataSet dstempInvoiceDetail = new DataSet();
                                dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.Tables[0].Merge(dt);
                                dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());

                                /*Added by kuntalkarar on 25thJuly2017*/
                                Session["DeletedInvoiceIds"] = "";
                                Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../Current/CurrentStatus.aspx';</script>");

                            }
                            else if (Request.QueryString["AllowEdit"].ToString() == "StockQC")
                                Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../StockQC/CurrentInvoice.aspx';</script>");
                            else if (Request.QueryString["AllowEdit"].ToString() == "History")
                            {
                                /*Added by kuntalkarar on 6thJune2017*/
                                DataTable dt = new DataTable();
                                dt = objInvoice.GetInvoiceDetails(InvID);
                                grdInvoiceDetails.DataSource = dt;
                                grdInvoiceDetails.DataBind();

                                //blocked by kuntalkarar on 25thJuly2017
                                /*Added by kuntalkarar on 6thJune2017*/
                                /*DataSet dstempInvoiceDetail = new DataSet();
                                dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                //-------------------------------------------------------------------------
                                /*Added by kuntalkarar on 25thJuly2017*/
                                DataSet dstempInvoiceDetail2 = new DataSet();
                                dstempInvoiceDetail2.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail2.Tables[0].Merge(dt);
                                dstempInvoiceDetail2.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
                                /*Added by kuntalkarar on 25thJuly2017*/
                                Session["DeletedInvoiceIds"] = "";
                                Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../History/history.aspx';</script>");
                            }
                        }
                    }

                    #endregion
                }
            }
            else
            {
                if (dtNewVc.Rows.Count > 0)
                {
                    if (dtNewVc.Rows[0]["New_VendorClass"].ToString() == "PO")
                    {
                        if (vRFlag == 0)
                        {
                            #region btnSave

                            iRetValue = objInvoice.SaveInvoiceDatatoDataBase(Convert.ToInt32(Session["UserID"]), InvID, strInvoiceNo, strSupplierCompanyID, strBuyerCompanyID,
                                       InvDate, TaxDate, strCurrency, strSupplierAddress1, strSupplierAddress2, strSupplierAddress3, strSupplierAddress4, strSupplierAddress5, strSupplierCountry,
                                       strSupplierZIP, strSellerVATNo, txtComments.Text.ToString(), strEDITEDdetailXML, decNetTotal, decVATAmt, decTotalAmt, strforDelete, strDepartmentID, strBusinessUnitID);//Modified by Mainak 2018-03-19

                            if (iRetValue > 0)
                            {

                                if (Request.QueryString["IsParentReload"] != null && Convert.ToInt32(Request.QueryString["IsParentReload"].ToString()) == 1)
                                {
                                    // If Condition Added by Mrinal on 3rd January 2015
                                    Response.Write("<script>opener.location.reload();</script>");
                                    Response.Write("<script>self.close();</script>");
                                }
                                else
                                {
                                    if (Request.QueryString["AllowEdit"].ToString() == "Current")
                                    {

                                        /*Added by kuntalkarar on 1stJune2017*/
                                        DataTable dt = new DataTable();
                                        dt = objInvoice.GetInvoiceDetails(InvID);
                                        grdInvoiceDetails.DataSource = dt;
                                        grdInvoiceDetails.DataBind();

                                        //blocked by kuntalkarar on 25thJuly2017
                                        /*Added by kuntalkarar on 2ndJune2017*/
                                        /*DataSet dstempInvoiceDetail = new DataSet();
                                        dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                        int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                        dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                        dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                        //-------------------------------------------------------------------------

                                        /*Added by kuntalkarar on 25thJuly2017*/
                                        DataSet dstempInvoiceDetail = new DataSet();
                                        dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.Tables[0].Merge(dt);
                                        dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());

                                        /*Added by kuntalkarar on 25thJuly2017*/
                                        Session["DeletedInvoiceIds"] = "";
                                        Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../Current/CurrentStatus.aspx';</script>");

                                    }
                                    else if (Request.QueryString["AllowEdit"].ToString() == "StockQC")
                                        Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../StockQC/CurrentInvoice.aspx';</script>");
                                    else if (Request.QueryString["AllowEdit"].ToString() == "History")
                                    {
                                        /*Added by kuntalkarar on 6thJune2017*/
                                        DataTable dt = new DataTable();
                                        dt = objInvoice.GetInvoiceDetails(InvID);
                                        grdInvoiceDetails.DataSource = dt;
                                        grdInvoiceDetails.DataBind();

                                        //blocked by kuntalkarar on 25thJuly2017
                                        /*Added by kuntalkarar on 6thJune2017*/
                                        /*DataSet dstempInvoiceDetail = new DataSet();
                                        dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                        int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                        dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                        dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                        //-------------------------------------------------------------------------
                                        /*Added by kuntalkarar on 25thJuly2017*/
                                        DataSet dstempInvoiceDetail2 = new DataSet();
                                        dstempInvoiceDetail2.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                        dstempInvoiceDetail2.Tables[0].Merge(dt);
                                        dstempInvoiceDetail2.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
                                        /*Added by kuntalkarar on 25thJuly2017*/
                                        Session["DeletedInvoiceIds"] = "";
                                        Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../History/history.aspx';</script>");
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            string message = "alert('VAT ÷ Net does not equal a valid tax rate.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                    }
                    else
                    {
                        #region btnSave

                        iRetValue = objInvoice.SaveInvoiceDatatoDataBase(Convert.ToInt32(Session["UserID"]), InvID, strInvoiceNo, strSupplierCompanyID, strBuyerCompanyID,
                                   InvDate, TaxDate, strCurrency, strSupplierAddress1, strSupplierAddress2, strSupplierAddress3, strSupplierAddress4, strSupplierAddress5, strSupplierCountry,
                                   strSupplierZIP, strSellerVATNo, txtComments.Text.ToString(), strEDITEDdetailXML, decNetTotal, decVATAmt, decTotalAmt, strforDelete, strDepartmentID, strBusinessUnitID);//Modified by Mainak 2018-03-19

                        if (iRetValue > 0)
                        {

                            if (Request.QueryString["IsParentReload"] != null && Convert.ToInt32(Request.QueryString["IsParentReload"].ToString()) == 1)
                            {
                                // If Condition Added by Mrinal on 3rd January 2015
                                Response.Write("<script>opener.location.reload();</script>");
                                Response.Write("<script>self.close();</script>");
                            }
                            else
                            {
                                if (Request.QueryString["AllowEdit"].ToString() == "Current")
                                {

                                    /*Added by kuntalkarar on 1stJune2017*/
                                    DataTable dt = new DataTable();
                                    dt = objInvoice.GetInvoiceDetails(InvID);
                                    grdInvoiceDetails.DataSource = dt;
                                    grdInvoiceDetails.DataBind();

                                    //blocked by kuntalkarar on 25thJuly2017
                                    /*Added by kuntalkarar on 2ndJune2017*/
                                    /*DataSet dstempInvoiceDetail = new DataSet();
                                    dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                    int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                    dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                    dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                    //-------------------------------------------------------------------------

                                    /*Added by kuntalkarar on 25thJuly2017*/
                                    DataSet dstempInvoiceDetail = new DataSet();
                                    dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.Tables[0].Merge(dt);
                                    dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());

                                    /*Added by kuntalkarar on 25thJuly2017*/
                                    Session["DeletedInvoiceIds"] = "";
                                    Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../Current/CurrentStatus.aspx';</script>");

                                }
                                else if (Request.QueryString["AllowEdit"].ToString() == "StockQC")
                                    Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../StockQC/CurrentInvoice.aspx';</script>");
                                else if (Request.QueryString["AllowEdit"].ToString() == "History")
                                {
                                    /*Added by kuntalkarar on 6thJune2017*/
                                    DataTable dt = new DataTable();
                                    dt = objInvoice.GetInvoiceDetails(InvID);
                                    grdInvoiceDetails.DataSource = dt;
                                    grdInvoiceDetails.DataBind();

                                    //blocked by kuntalkarar on 25thJuly2017
                                    /*Added by kuntalkarar on 6thJune2017*/
                                    /*DataSet dstempInvoiceDetail = new DataSet();
                                    dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                    int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                    dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                    dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                    //-------------------------------------------------------------------------
                                    /*Added by kuntalkarar on 25thJuly2017*/
                                    DataSet dstempInvoiceDetail2 = new DataSet();
                                    dstempInvoiceDetail2.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                    dstempInvoiceDetail2.Tables[0].Merge(dt);
                                    dstempInvoiceDetail2.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
                                    /*Added by kuntalkarar on 25thJuly2017*/
                                    Session["DeletedInvoiceIds"] = "";
                                    Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../History/history.aspx';</script>");
                                }
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    #region btnSave

                    iRetValue = objInvoice.SaveInvoiceDatatoDataBase(Convert.ToInt32(Session["UserID"]), InvID, strInvoiceNo, strSupplierCompanyID, strBuyerCompanyID,
                               InvDate, TaxDate, strCurrency, strSupplierAddress1, strSupplierAddress2, strSupplierAddress3, strSupplierAddress4, strSupplierAddress5, strSupplierCountry,
                               strSupplierZIP, strSellerVATNo, txtComments.Text.ToString(), strEDITEDdetailXML, decNetTotal, decVATAmt, decTotalAmt, strforDelete, strDepartmentID, strBusinessUnitID);//Modified by Mainak 2018-03-19

                    if (iRetValue > 0)
                    {

                        if (Request.QueryString["IsParentReload"] != null && Convert.ToInt32(Request.QueryString["IsParentReload"].ToString()) == 1)
                        {
                            // If Condition Added by Mrinal on 3rd January 2015
                            Response.Write("<script>opener.location.reload();</script>");
                            Response.Write("<script>self.close();</script>");
                        }
                        else
                        {
                            if (Request.QueryString["AllowEdit"].ToString() == "Current")
                            {

                                /*Added by kuntalkarar on 1stJune2017*/
                                DataTable dt = new DataTable();
                                dt = objInvoice.GetInvoiceDetails(InvID);
                                grdInvoiceDetails.DataSource = dt;
                                grdInvoiceDetails.DataBind();

                                //blocked by kuntalkarar on 25thJuly2017
                                /*Added by kuntalkarar on 2ndJune2017*/
                                /*DataSet dstempInvoiceDetail = new DataSet();
                                dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                //-------------------------------------------------------------------------

                                /*Added by kuntalkarar on 25thJuly2017*/
                                DataSet dstempInvoiceDetail = new DataSet();
                                dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.Tables[0].Merge(dt);
                                dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());

                                /*Added by kuntalkarar on 25thJuly2017*/
                                Session["DeletedInvoiceIds"] = "";
                                Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../Current/CurrentStatus.aspx';</script>");

                            }
                            else if (Request.QueryString["AllowEdit"].ToString() == "StockQC")
                                Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../StockQC/CurrentInvoice.aspx';</script>");
                            else if (Request.QueryString["AllowEdit"].ToString() == "History")
                            {
                                /*Added by kuntalkarar on 6thJune2017*/
                                DataTable dt = new DataTable();
                                dt = objInvoice.GetInvoiceDetails(InvID);
                                grdInvoiceDetails.DataSource = dt;
                                grdInvoiceDetails.DataBind();

                                //blocked by kuntalkarar on 25thJuly2017
                                /*Added by kuntalkarar on 6thJune2017*/
                                /*DataSet dstempInvoiceDetail = new DataSet();
                                dstempInvoiceDetail.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                                int lastrowIndext = dstempInvoiceDetail.Tables[0].Rows.Count - 1;
                                dstempInvoiceDetail.Tables[0].Rows[lastrowIndext]["InvoiceDetailId"] = dt.Rows[lastrowIndext]["InvoiceDetailId"].ToString();
                                dstempInvoiceDetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());*/
                                //-------------------------------------------------------------------------
                                /*Added by kuntalkarar on 25thJuly2017*/
                                DataSet dstempInvoiceDetail2 = new DataSet();
                                dstempInvoiceDetail2.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                                dstempInvoiceDetail2.Tables[0].Merge(dt);
                                dstempInvoiceDetail2.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
                                /*Added by kuntalkarar on 25thJuly2017*/
                                Session["DeletedInvoiceIds"] = "";
                                Response.Write("<script language=javascript>alert('Changes saved successfully'); top.location.href='../History/history.aspx';</script>");
                            }
                        }
                    }

                    #endregion
                }

            }


        }
        #endregion

        #region UpdateInvoiceHeaderinXML
        public void UpdateInvoiceHeaderinXML(int InvID, string strInvoiceNo, string strSupplierCompanyID, string strBuyerCompanyID,
            string strInvoiceDate, string strTaxPointDate, string strCurrency, decimal dNetValue, decimal VATAmt, decimal totalAmt)
        {
            UpdateOthersinHeaderXML(InvID, strInvoiceNo, strSupplierCompanyID, strBuyerCompanyID,
            strInvoiceDate, strTaxPointDate, strCurrency);

            UpdateAmountsinHeaderXML(dNetValue, VATAmt, totalAmt);
        }
        #endregion

        #region UpdateOthersinHeaderXML
        public void UpdateOthersinHeaderXML(int InvID, string strInvoiceNo, string strSupplierCompanyID, string strBuyerCompanyID,
            string strInvoiceDate, string strTaxPointDate, string strCurrency)
        {
            if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead = new RecordSet(ds);

                rsHead["InvoiceNo"] = strInvoiceNo;
                rsHead["SupplierCompanyID"] = strSupplierCompanyID;
                rsHead["BuyerCompanyID"] = strBuyerCompanyID;
                rsHead["InvoiceDate"] = strInvoiceDate;
                rsHead["TaxPointDate"] = strTaxPointDate;
                rsHead["CurrencyTypeID"] = strCurrency;

                DataSet dsInvoiceHead = new DataSet();
                dsInvoiceHead = rsHead.ParentDataSet;
                dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());

                DataSet ds1 = new DataSet();
                ds1.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds1.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead1 = new RecordSet(ds1);
            }
        }
        #endregion

        #region UpdateInvoiceDetailinXML
        public string UpdateInvoiceDetailinXML()
        {
            RecordSet rs = null;
            if (System.IO.File.Exists(Session["XMLInvoiceDetailFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                rs = new RecordSet(ds);
            }
            DataTable dt = rs.ParentTable;
            DataRow myRow;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                myRow = dt.Rows[j];
                myRow.BeginEdit();

                //Added by Mainak 2018-08-09
                TextBox tbPOLineNo = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtPOLineNo");
                if ((tbPOLineNo.Text != "" && !IsNumericValue(tbPOLineNo.Text)) || tbPOLineNo.Text == "")
                {
                    myRow["PurOrderLineNo"] = DBNull.Value;//Added by Mainak 2018-08-31
                }
                else
                {
                    myRow["PurOrderLineNo"] = Convert.ToString(tbPOLineNo.Text);
                }


                //Added by kuntalkarar on 9thMarch2017
                TextBox tbBuyersProdCode = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtBuyersProdCode");
                myRow["BuyersProdCode"] = Convert.ToString(tbBuyersProdCode.Text);

                TextBox tbDesc = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtDesc");
                myRow["Description"] = Convert.ToString(tbDesc.Text);
                TextBox tbPurOrderNo = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtPurOrderNo");
                myRow["PurOrderNo"] = Convert.ToString(tbPurOrderNo.Text);
                TextBox tbPrice = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtPrice");

                if ((tbPrice.Text != "" && !IsNumericValue(tbPrice.Text)) || tbPrice.Text == "")
                {
                    lblError.Text = "Check Price in Line items and click Calculate";
                    return "";
                }
                else
                {
                    lblError.Text = "";
                    myRow["Rate"] = Convert.ToString(tbPrice.Text);
                }

                TextBox tbQuantity = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtQuantity");
                if ((tbQuantity.Text != "" && !IsNumericValue(tbQuantity.Text)) || tbQuantity.Text == "")
                {
                    lblError.Text = "Check Quantity in Line items and click Calculate";
                    return "";
                }
                else
                {
                    lblError.Text = "";
                    myRow["Quantity"] = Convert.ToString(tbQuantity.Text);
                }

                TextBox tbNetValue = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtNetValue");
                if ((tbNetValue.Text != "" && !IsNumericValue(tbNetValue.Text)) || tbNetValue.Text == "")
                {
                    lblError.Text = "Please enter line details and press Calculate";
                    return "";
                }
                else
                {
                    lblError.Text = "";
                    myRow["New_NettValue"] = Convert.ToString(tbNetValue.Text);
                }

                TextBox tbVAT = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtVAT");
                if ((tbVAT.Text != "" && !IsNumericValue(tbVAT.Text)) || tbVAT.Text == "")
                {
                    // lblError.Text = "Check VAT Amount in Line items";
                    // return "";
                    lblError.Text = "";
                    myRow["VATAmt"] = Convert.ToString("0.00");
                }
                else
                {
                    lblError.Text = "";
                    myRow["VATAmt"] = Convert.ToString(tbVAT.Text);
                }

                TextBox tbGrossValue = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtGrossValue");

                if ((tbGrossValue.Text != "" && !IsNumericValue(tbGrossValue.Text)) || tbGrossValue.Text == "")
                {
                    // lblError.Text = "Please press Calculate to populate line level VAT and Gross before saving";
                    //  return "";
                    lblError.Text = "";
                    myRow["TotalAmt"] = Convert.ToString("0.00");
                }
                else
                {
                    lblError.Text = "";
                    myRow["TotalAmt"] = Convert.ToString(tbGrossValue.Text);
                }


                DropDownList ddlVATRate = (DropDownList)grdInvoiceDetails.Rows[j].FindControl("dpVATRate");
                try
                {
                    decimal PVATRate = Convert.ToDecimal(ddlVATRate.SelectedItem.Text);
                    myRow["VATRate"] = Convert.ToString(ddlVATRate.SelectedItem.Text);
                }
                catch
                {
                    myRow["VATRate"] = Convert.ToString("0.00");
                    //  lblError.Text = "Please select a VAT Rate for each line item";
                    // return "";
                }

                //myRow["VATRate"] = Convert.ToString(ddlVATRate.SelectedItem.Text);


                myRow.AcceptChanges();
                dt.AcceptChanges();
                myRow.EndEdit();
            }
            rs = new RecordSet(dt.DataSet);
            DataSet dsInvoicedetail = new DataSet();
            dsInvoicedetail = dt.DataSet;
            dsInvoicedetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
            dsInvoicedetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
            StringBuilder oBuilder = new StringBuilder();
            StringWriter oStringWriter = new StringWriter(oBuilder);
            XmlTextReader oXmlReader = new XmlTextReader(Session["XMLInvoiceDetailFile"].ToString());
            XmlTextWriter oXmlWriter = new XmlTextWriter(oStringWriter);
            while (oXmlReader.Read())
            {
                oXmlWriter.WriteNode(oXmlReader, true);
            }
            oXmlReader.Close();
            oXmlWriter.Close();

            string strEditedXML = oBuilder.ToString();
            strEditedXML = strEditedXML.Replace("standalone=\"yes\"", "encoding=\"ISO-8859-1\"");

            return strEditedXML;
        }

        #endregion

        #region btnDelete_Click
        private void btnDelete_Click(object sender, System.EventArgs e)
        {

            for (int j = grdInvoiceDetails.Rows.Count - 1; j >= 0; j--)
            {
                CheckBox chk1 = (CheckBox)grdInvoiceDetails.Rows[j].FindControl("chkDelete");
                if (chk1.Checked)
                {
                    /*Added by kuntalkarar on 30thMay2017*/
                    //lblstrDelete.Text = lblstrDelete.Text + grdInvoiceDetails.DataKeys[j].Value.ToString();   //commented by Mainak 30 10-2018
                    //------------------------------------
                    DeleteInvoiceLineFromXML(j);
                }
            }
            PopulateData();
            AutoUpdateOtherFieldsinXML();

            iPassToJS = 1;
            string strUrl = Request.Url.ToString();
            strUrl = strUrl.Replace("&#ss", "");
            strUrl = strUrl.Replace("&%23ss", "");
            strUrl = strUrl.Replace("AllowEdit=" + Request.QueryString["AllowEdit"].ToString() + "&", "AllowEdit=" + Request.QueryString["AllowEdit"].ToString());
            strUrl += "&#ss";
            iUrlJS = strUrl;
        }

        #endregion

        #region DeleteInvoiceLineFromXML
        private void DeleteInvoiceLineFromXML(int dataTableItemIndex)
        {
            //			string strDeleteIDs="";

            RecordSet rs = null;
            if (System.IO.File.Exists(Session["XMLInvoiceDetailFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                rs = new RecordSet(ds);
            }
            DataTable dt = rs.ParentTable;
            if (grdInvoiceDetails.Rows.Count > 1)
            {

                //blocked by kuntalkarar on 30thMay2017
                //lblstrDelete.Text = lblstrDelete.Text + grdInvoiceDetails.Rows[dataTableItemIndex].Cells[20].Text.ToString();

                dt.Rows.RemoveAt(dataTableItemIndex);

                /*DataSet dsSupplier = new DataSet();

               SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
               SqlDataAdapter sqlDA = new SqlDataAdapter("sp_deleteInvoiceDeailLine", sqlConn);
               sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
               sqlDA.SelectCommand.Parameters.Add("@invoiceDetailsId", Convert.ToInt32(lblstrDelete.Text));              
              
               try
               {
                   sqlConn.Open();
                   sqlDA.Fill(dsSupplier);
               }
               catch (Exception ex)
               {
                   string strExceptionMessage = ex.Message.Trim();
               }
               finally
               {

                   if (sqlDA != null)
                       sqlDA.Dispose();
                   if (sqlConn != null)
                       sqlConn.Close();
               }*/

            }
            else
            {

                lblError.Text = "At least one line should be present";
                return;
            }

            if (lblstrDelete.Text.Length > 0)
            {
                lblstrDelete.Text = lblstrDelete.Text + ",";
            }

            rs = new RecordSet(dt.DataSet);
            DataSet dsInvoicedetail = new DataSet();
            dsInvoicedetail = dt.DataSet;
            dsInvoicedetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
            dsInvoicedetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
            if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead = new RecordSet(ds);
                decimal totalAmt = 0;
                decimal VATAmt = 0;
                decimal dNetValue = 0;
                decimal dOverallDiscountAmount = 0;
                rs.MoveFirst();
                while (!rs.EOF())
                {
                    if (rs["New_OverallDiscountValue"] != DBNull.Value)
                        dOverallDiscountAmount = dOverallDiscountAmount + System.Convert.ToDecimal(rs["New_OverallDiscountValue"]);
                    if (rs["New_NettValue"] != "" && rs["New_NettValue"] != DBNull.Value)
                        dNetValue = dNetValue + System.Convert.ToDecimal(rs["New_NettValue"]);
                    if (rs["TotalAmt"] != DBNull.Value)
                        totalAmt = totalAmt + System.Convert.ToDecimal(rs["TotalAmt"]);
                    if (rs["VATAmt"] != DBNull.Value)
                        VATAmt = VATAmt + System.Convert.ToDecimal(rs["VATAmt"]);
                    rs.MoveNext();
                }
                rsHead["NetTotal"] = dNetValue;
                rsHead["VATAmt"] = VATAmt;
                rsHead["TotalAmt"] = totalAmt;
                rsHead["New_OveralldiscountAmount"] = dOverallDiscountAmount;
                decimal headDiscountPercent = 0;
                if (rsHead["DiscountPercent"] != DBNull.Value)
                    headDiscountPercent = System.Convert.ToDecimal(rsHead["DiscountPercent"]);
                DataSet dsInvoiceHead = new DataSet();
                dsInvoiceHead = rsHead.ParentDataSet;
                dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());
            }
        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            //Destroy Previous Files
            System.IO.File.Delete(Session["XSDInvoiceHeadFile"].ToString());
            System.IO.File.Delete(Session["XMLInvoiceHeadFile"].ToString());
            System.IO.File.Delete(Session["XSDInvoiceDetailFile"].ToString());
            System.IO.File.Delete(Session["XMLInvoiceDetailFile"].ToString());

            System.IO.File.Delete(Session["XSDInvoiceHeadFile_CN"].ToString());
            System.IO.File.Delete(Session["XMLInvoiceHeadFile_CN"].ToString());
            System.IO.File.Delete(Session["XSDInvoiceDetailFile_CN"].ToString());
            System.IO.File.Delete(Session["XMLInvoiceDetailFile_CN"].ToString());

            /*Blocked by Koushik Das as on 04-Apr-2017*/
            /*if (Request.QueryString["AllowEdit"].ToString() == "Current")
            {
                //Response.Write("<script language=javascript>top.location.href='../Current/CurrentStatus.aspx';</script>");
                //---------------------------Commented by Rimi on 17.07.2015--------------------------------------------
                // Response.Write("<script>opener.location.reload();</script>");// Added by Rimi on 20.06.2015
                //Response.Write("<script>self.close();</script>");// Added by Rimi on 20.06.2015

                //---------------------------Added by Rimi on 17.07.2015--------------------------------------------
                if (Session["PageRedirect"] == "yes")
                {

                    Response.Write("<script>opener.location.reload();</script>");
                    Response.Write("<script>self.close();</script>");
                    Session["PageRedirect"] = "No";
                }
                else
                {
                    Response.Write("<script language=javascript>top.location.href='../Current/CurrentStatus.aspx';</script>");
                }
                //---------------------------Added by Rimi on 17.07.2015 End--------------------------------------------


                //---------------------------Commented by Rimi on 17.07.2015--------------------------------------------
            }
            else if (Request.QueryString["AllowEdit"].ToString() == "StockQC")
                Response.Write("<script language=javascript>top.location.href='../StockQC/CurrentInvoice.aspx';</script>");
            else if (Request.QueryString["AllowEdit"].ToString() == "History")
                Response.Write("<script language=javascript>top.location.href='../History/history.aspx';</script>");*/
            /*Blocked by Koushik Das as on 04-Apr-2017*/

            /*Applied by Koushik Das as on 04-Apr-2017*/
            if (Request.QueryString["IsParentReload"] != null && Convert.ToInt32(Request.QueryString["IsParentReload"].ToString()) == 1)
            {
                // If Condition Added by Mrinal on 3rd January 2015
                Response.Write("<script>opener.location.reload();</script>");
                Response.Write("<script>self.close();</script>");
            }
            else
            {
                if (Request.QueryString["AllowEdit"].ToString() == "Current")
                    Response.Write("<script language=javascript>top.location.href='../Current/CurrentStatus.aspx';</script>");
                else if (Request.QueryString["AllowEdit"].ToString() == "StockQC")
                    Response.Write("<script language=javascript>top.location.href='../StockQC/CurrentInvoice.aspx';</script>");
                else if (Request.QueryString["AllowEdit"].ToString() == "History")
                    Response.Write("<script language=javascript>top.location.href='../History/history.aspx';</script>");
            }
            /*Applied by Koushik Das as on 04-Apr-2017*/
        }

        #endregion

        #region btnCalculate_Click
        private void btnCalculate_Click(object sender, System.EventArgs e)
        {
            AutoUpdateOtherFieldsinXML();
            PopulateData();
            iPassToJS = 1;
        }

        #endregion

        #region AutoUpdateOtherFieldsinXML
        private void AutoUpdateOtherFieldsinXML()
        {
            decimal PVat = 0;
            RecordSet rs = null;
            if (System.IO.File.Exists(Session["XMLInvoiceDetailFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                rs = new RecordSet(ds);
            }
            DataTable dt = rs.ParentTable;
            decimal totalAmt = 0;
            decimal VATAmt = 0;
            decimal dNetValue = 0;
            decimal dOverallDiscountAmount = 0;
            decimal PVATRate = 0;
            decimal PPrice = 0;
            decimal Qty = 0;
            string Desc = "";
            string PurOrdNo = "";
            string BuyersProdCode = "";
            string PurOrderLineNo = "";//Added by Mainak 2018-08-09
            for (int j = 0; j < grdInvoiceDetails.Rows.Count; j++)
            {
                DataRow myRow;
                myRow = dt.Rows[j];
                myRow.BeginEdit();

                TextBox tbQuantity = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtQuantity");
                if ((tbQuantity.Text != "" && !IsNumericValue(tbQuantity.Text)) || tbQuantity.Text == "")
                {
                    lblError.Text = "Invalid Value for Quantity in Line items";
                    return;
                }
                else
                {
                    lblError.Text = "";
                }
                Qty = Convert.ToDecimal(tbQuantity.Text);
                myRow["Quantity"] = Convert.ToString(Qty);

                TextBox tbPrice = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtPrice");
                if ((tbPrice.Text != "" && !IsNumericValue(tbPrice.Text)) || tbPrice.Text == "")
                {
                    lblError.Text = "Invalid Value for Price in Line items";
                    return;
                }
                else
                {
                    lblError.Text = "";
                }
                PPrice = Convert.ToDecimal(tbPrice.Text);
                myRow["Rate"] = Convert.ToString(PPrice);

                DropDownList ddlVATRate = (DropDownList)grdInvoiceDetails.Rows[j].FindControl("dpVATRate");
                try
                {
                    if (ddlVATRate.SelectedItem.Text == "Select")
                    {
                        PVATRate = 0;
                    }
                    else
                    {
                        PVATRate = Convert.ToDecimal(ddlVATRate.SelectedItem.Text);
                    }
                }
                catch
                {
                    lblError.Text = "Please select a VAT Rate for each line item";
                    return;
                }
                myRow["VATAmt"] = Convert.ToDecimal(Qty * PPrice * PVATRate / 100).ToString("#0.00");
                PVat = Qty * PPrice * PVATRate / 100;

                myRow["New_NettValue"] = Convert.ToDecimal(Qty * PPrice).ToString("#0.00");
                myRow["TotalAmt"] = Convert.ToDecimal(Qty * PPrice + Qty * PPrice * PVATRate / 100).ToString("#0.00");
                myRow["GrossAmt"] = Convert.ToDecimal(Qty * PPrice).ToString("#0.00");
                myRow["VATRate"] = PVATRate;

                //Added by Mainak 2018-08-09
                TextBox tbPOLineNo = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtPOLineNo");
                //Modified by Mainak 2018-09-14
                //PurOrderLineNo = tbPOLineNo.Text;
                //myRow["PurOrderLineNo"] = PurOrderLineNo;
                if ((tbPOLineNo.Text != "" && !IsNumericValue(tbPOLineNo.Text)) || tbPOLineNo.Text == "")
                {
                    myRow["PurOrderLineNo"] = DBNull.Value;//Added by Mainak 2018-08-31
                }
                else
                {
                    myRow["PurOrderLineNo"] = Convert.ToString(tbPOLineNo.Text);
                }


                //Added by kuntalkarar on 9thMarch2017
                TextBox tbBuyersProdCode = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtBuyersProdCode");
                BuyersProdCode = tbBuyersProdCode.Text;
                myRow["BuyersProdCode"] = BuyersProdCode;

                TextBox tbDesc = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtDesc");
                Desc = tbDesc.Text;
                myRow["Description"] = Desc.ToString();
                TextBox tbPurOrderNo = (TextBox)grdInvoiceDetails.Rows[j].FindControl("txtPurOrderNo");
                PurOrdNo = tbPurOrderNo.Text;
                myRow["PurOrderNo"] = PurOrdNo.ToString();

                myRow.AcceptChanges();
                dt.AcceptChanges();
                myRow.EndEdit();
                dNetValue = dNetValue + PPrice * Qty;
                totalAmt = totalAmt + PPrice * Qty + PVat;
                VATAmt = VATAmt + PVat;
            }



            rs = new RecordSet(dt.DataSet);
            //save the invoice detail recorset in XML format
            DataSet dsInvoicedetail = new DataSet();
            dsInvoicedetail = dt.DataSet;
            dsInvoicedetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
            dsInvoicedetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
            //update the invoice head XML
            if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead = new RecordSet(ds);
                //update the fields
                decimal totalAmt1 = 0;
                decimal VATAmt1 = 0;
                decimal dNetValue1 = 0;
                decimal dOverallDiscountAmount1 = 0;
                rs.MoveFirst();
                while (!rs.EOF())
                {
                    if (rs["New_OverallDiscountValue"] != DBNull.Value)
                        dOverallDiscountAmount1 = dOverallDiscountAmount1 + System.Convert.ToDecimal(rs["New_OverallDiscountValue"]);
                    if (rs["New_NettValue"] != DBNull.Value)
                        dNetValue1 = dNetValue1 + Qty * PPrice + System.Convert.ToDecimal(rs["New_NettValue"]);
                    if (rs["TotalAmt"] != DBNull.Value)
                        totalAmt1 = totalAmt1 + Qty * PPrice + PVat + System.Convert.ToDecimal(rs["TotalAmt"]);
                    if (rs["VATAmt"] != DBNull.Value)
                        VATAmt1 = VATAmt1 + PVat + System.Convert.ToDecimal(rs["VATAmt"]);
                    rs.MoveNext();
                }
                rsHead["NetTotal"] = dNetValue;
                rsHead["VATAmt"] = VATAmt;
                rsHead["TotalAmt"] = totalAmt;
                rsHead["New_OveralldiscountAmount"] = dOverallDiscountAmount;
                rsHead["InvoiceNo"] = txtInvoiceNo.Text;
                if (Convert.ToString(ddlSupplier.SelectedValue) != "")
                {
                    rsHead["SupplierCompanyID"] = Convert.ToInt32(ddlSupplier.SelectedValue);
                }
                else
                {
                    rsHead["SupplierCompanyID"] = Convert.ToInt32(0);
                }
                rsHead["BuyerCompanyID"] = ddlCompany.SelectedValue;
                rsHead["CurrencyTypeID"] = cboCurrencyType.SelectedValue;
                DateTime dtInvDt;
                dtInvDt = new DateTime(Convert.ToInt32(cboYearInvoiceDate.SelectedValue), Convert.ToInt32(cboMonthInvoiceDate.SelectedValue), Convert.ToInt32(cboDayInvoiceDate.SelectedValue));
                rsHead["InvoiceDate"] = System.Convert.ToDateTime(dtInvDt);
                DateTime dtTaxDt;
                if (cboYearTaxPointDate.SelectedValue != "0" && cboMonthTaxPointDate.SelectedValue != "0" && cboDayTaxPointDate.SelectedValue != "0")
                    dtTaxDt = new DateTime(Convert.ToInt32(cboYearTaxPointDate.SelectedValue), Convert.ToInt32(cboMonthTaxPointDate.SelectedValue), Convert.ToInt32(cboDayTaxPointDate.SelectedValue));
                else
                    dtTaxDt = new DateTime(1900, 1, 1);

                rsHead["TaxPointDate"] = System.Convert.ToDateTime(dtTaxDt);


                decimal headDiscountPercent = 0;
                if (rsHead["DiscountPercent"] != DBNull.Value)
                    headDiscountPercent = System.Convert.ToDecimal(rsHead["DiscountPercent"]);

                DataSet dsInvoiceHead = new DataSet();
                dsInvoiceHead = rsHead.ParentDataSet;
                dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());
            }
            UpdateAmountsinHeaderXML(dNetValue, VATAmt, totalAmt);

        }
        #endregion

        #region UpdateAmountsinHeaderXML
        public void UpdateAmountsinHeaderXML(decimal dNetValue, decimal VATAmt, decimal totalAmt)
        {
            if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead = new RecordSet(ds);
                rsHead["NetTotal"] = dNetValue;
                rsHead["VATAmt"] = VATAmt;
                rsHead["TotalAmt"] = totalAmt;
                DataSet dsInvoiceHead = new DataSet();
                dsInvoiceHead = rsHead.ParentDataSet;
                dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());
                DataSet ds1 = new DataSet();
                ds1.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds1.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead1 = new RecordSet(ds1);
                txtNetTotal.Text = System.Convert.ToDouble(rsHead1["NetTotal"]).ToString("#0.00");
                txtTotAmt.Text = System.Convert.ToDouble(rsHead1["TotalAmt"]).ToString("#0.00");
                txtVATAmtNew.Text = System.Convert.ToDouble(rsHead1["VATAmt"]).ToString("#0.00");
            }
        }
        #endregion

        #region Click_Submit
        void Click_Submit()
        {
        }
        #endregion

        #region inpAddLine_ServerClick
        private void inpAddLine_ServerClick(object sender, System.EventArgs e)
        {

            /*Added by kuntalkarar on 25thJuly2017*/
            Session["DeletedInvoiceIds"] = lblstrDelete.Text;
            try
            {
                if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                    ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                    RecordSet rsHead = new RecordSet(ds);
                    rsHead["InvoiceNo"] = txtInvoiceNo.Text;
                    if (ddlSupplier.SelectedValue != "")
                    {
                        rsHead["SupplierCompanyID"] = Convert.ToInt32(ddlSupplier.SelectedValue);
                    }
                    else
                    {
                        rsHead["SupplierCompanyID"] = Convert.ToInt32(0);
                    }
                    rsHead["BuyerCompanyID"] = Convert.ToInt32(ddlCompany.SelectedValue);
                    rsHead["CurrencyTypeID"] = Convert.ToInt32(cboCurrencyType.SelectedValue);

                    if (cboYearInvoiceDate.SelectedValue != "0" && cboMonthInvoiceDate.SelectedValue != "0" && cboDayInvoiceDate.SelectedValue != "0")
                    {
                        rsHead["InvoiceDate"] = Convert.ToDateTime(cboYearInvoiceDate.SelectedValue + "/" + cboMonthInvoiceDate.SelectedValue + "/" + cboDayInvoiceDate.SelectedValue);
                    }

                    if (cboYearTaxPointDate.SelectedValue != "0" && cboMonthTaxPointDate.SelectedValue != "0" && cboDayTaxPointDate.SelectedValue != "0")
                    {
                        rsHead["TaxPointDate"] = Convert.ToDateTime(cboYearTaxPointDate.SelectedValue + "/" + cboMonthTaxPointDate.SelectedValue + "/" + cboDayTaxPointDate.SelectedValue);
                    }

                    DataSet dsInvoiceHead = new DataSet();
                    dsInvoiceHead = rsHead.ParentDataSet;
                    dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                    dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());
                }
                AutoUpdateOtherFieldsinXML();
                Response.Redirect("InvoiceDetailAdd.aspx?InvoiceID=" + invoiceID + "&AllowEdit=" + Request.QueryString["AllowEdit"].ToString());


            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
        }
        #endregion

        #region ddlSupplier_SelectedIndexChanged
        private void ddlSupplier_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //Company objCompany = new Company();
            //lblVATRegNo.Text = objCompany.GetSupplierVatNo(Convert.ToInt32(ddlSupplier.SelectedValue));
            //PopulateSuppAddress(Convert.ToInt32(ddlSupplier.SelectedValue), lblVATRegNo.Text);
        }

        #endregion

        #region PopulateSuppAddress
        private void PopulateSuppAddress(int CompanyID, string VATRegNo)
        {
            DataTable dtbl = new DataTable();
            DataSet DSADD = new DataSet();
            DSADD = objInvoice.GetSupplierAddress(CompanyID);
            if (DSADD.Tables[0].Rows.Count > 0)
            {
                dtbl = DSADD.Tables[0];
                ViewState["selectionBrID"] = "1";
            }
            else
            {
                dtbl = DSADD.Tables[1];
                ViewState["selectionBrID"] = "2";
            }
            DataRow drAdress;
            drAdress = dtbl.Rows[0];

            if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead = new RecordSet(ds);

                rsHead["SupplierAddress1"] = drAdress["Address1"].ToString();
                rsHead["SupplierAddress2"] = drAdress["Address2"].ToString();
                rsHead["SupplierAddress3"] = drAdress["Address3"].ToString();
                rsHead["SupplierAddress4"] = drAdress["Address4"].ToString();
                rsHead["SupplierAddress5"] = drAdress["Address5"].ToString();
                rsHead["SupplierCountry"] = drAdress["Country"].ToString();
                rsHead["SupplierZIP"] = drAdress["PostCode"].ToString();
                rsHead["SellerVATNo"] = VATRegNo.ToString();

                DataSet dsInvoiceHead = new DataSet();
                dsInvoiceHead = rsHead.ParentDataSet;
                dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());
            }
            string s = "";
            s = GetAddressLine(drAdress["Address1"].ToString());
            s += GetAddressLine(drAdress["Address2"].ToString());
            s += GetAddressLine(drAdress["Address3"].ToString());
            s += GetAddressLine(drAdress["Address4"].ToString());
            s += GetAddressLine(drAdress["Address5"].ToString());
            s += GetAddressLine(drAdress["Country"].ToString());
            s += GetAddressLine(drAdress["PostCode"].ToString());
            try
            {
                s = s.Substring(0, s.Length - 3);
            }
            catch { }
            lblSupplierAddress.Text = s;
        }
        #endregion

        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            HdSupplierId.Value = "";
            txtSupplier.Text = "";

            if (Convert.ToInt32(ddlCompany.SelectedValue) > 0)
            {
                //ddlSupplier.DataSource = objInvoice.GetSuppliersListNEW(Convert.ToInt32(ddlCompany.SelectedValue));
                //ddlSupplier.DataBind();

                //Added by Mainak 2018-13-19
                //Load Business unit Dropdown
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                //Load Department Dropdown
                LoadDepartment(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            }
            else
            {
                Page.RegisterStartupScript("reg", "<script>alert('Please select a valid buyer.')</script>");
            }
        }


        //Added by Mainak 2018-13-19
        private void GetBusinessUnit(int companyid)
        {
            ddlBusinessUnit.Items.Clear();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetBusinessUnit", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(companyid));
            sqlDA.SelectCommand.Parameters.Add("@UsrID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBusinessUnit.DataSource = ds;
                    ddlBusinessUnit.DataValueField = "BusinessUnitID";
                    ddlBusinessUnit.DataTextField = "BusinessUnitName";
                    ddlBusinessUnit.DataBind();
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
            }
            ddlBusinessUnit.Items.Insert(0, new ListItem("Select Business Unit", ""));
        }

        //Added by Mainak 2018-13-19
        private void LoadDepartment(int companyid)
        {
            ddldept.Items.Clear();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentList_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(companyid));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataBind();
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
                ddldept.Items.Insert(0, new ListItem("Select Department", "0"));
            }

        }

        [WebMethod]
        public static string[] GetSupplier(string CompanyID, string UserString)
        {
            DataSet dsSupplier = new DataSet();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetSuppliersListFromTradingRelation_New", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyString", UserString);
            sqlDA.SelectCommand.CommandTimeout = 0;
            try
            {
                sqlConn.Open();
                sqlDA.Fill(dsSupplier);
            }
            catch (Exception ex)
            {
                string strExceptionMessage = ex.Message.Trim();
            }
            finally
            {

                if (sqlDA != null)
                    sqlDA.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }

            List<string> myList = new List<string>();
            if (dsSupplier != null && dsSupplier.Tables.Count > 0 && dsSupplier.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsSupplier.Tables[0].Rows)
                {
                    myList.Add(string.Format("{0}#{1}", row["CompanyID"].ToString(), row["label"].ToString()));

                }
                return myList.ToArray();
            }
            else
                return null;
            // return "";
        }

        ///Added by Mainak 2018-10-02
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string[] ListPONumber(string buyerID, string supplierID, string poNoPartial)
        {
            List<string> list = new List<string>();
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataTable DT = new DataTable();
            try
            {
                //The supplier name string containing '' is giving issue thus removed.
                poNoPartial = poNoPartial.Replace("[", "{").Replace("]", "}").Replace("''", "'");

                //DT = DC.ReturnSupplierDataTable(input1, Convert.ToInt32(input2), input1);
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("SP_GetPONoList_JKS", sqlConn);
                cmd.Parameters.AddWithValue("@BuyerID", Convert.ToInt32(buyerID));
                cmd.Parameters.AddWithValue("@SupplierId", Convert.ToInt32(supplierID));
                cmd.Parameters.AddWithValue("@PurOrderNo", poNoPartial);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(DT);
                sqlConn.Close();

                foreach (DataRow DR in DT.Rows)
                {
                    list.Add(DR[0].ToString() + "^" + DR[0].ToString());
                }
            }
            catch (Exception ex)
            {
                string ss = "Message: " + ex.Message
                    + "<br />Source: " + ex.Source
                    + "<br />StackTrace: " + ex.StackTrace
                    + "<br />TargetSite: " + ex.TargetSite
                    + "<br />InnerException: " + ex.InnerException
                    + "<br />Data: " + ex.Data;
                HttpContext.Current.Response.Write("<br />Error in ListSupplierNames WebMethod: " + ss);
                list = new List<string>();
            }
            finally
            {
                DT.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return list.ToArray();
        }

        protected void btnOpenModal_Click(object sender, EventArgs e)
        {
            try
            {
                string buyercompanyid = ddlCompany.SelectedValue.ToString();//Added by MAinak 2018-07-18
                SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                DataSet DS = new DataSet();

                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("SP_getValuesForModalPONo_JKS_New", sqlConn);//Modified by MAinak 2018-07-18
                cmd.Parameters.AddWithValue("@PurOrderNo", hdnPurOrderNo.Value);
                cmd.Parameters.AddWithValue("@CompanyID", buyercompanyid);//Added by MAinak 2018-07-18


                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(DS);
                sqlConn.Close();

                lblPO.Text = hdnPurOrderNo.Value;
                lblCompany.Text = (DS.Tables[0].Rows[0]["Company"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["Company"]);
                lblSupplier1.Text = (DS.Tables[0].Rows[0]["Supplier"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["Supplier"]);
                lblDate.Text = (DS.Tables[0].Rows[0]["Date"] as object == DBNull.Value) ? "" : (Convert.ToDateTime(DS.Tables[0].Rows[0]["Date"])).ToString("dd/MM/yyyy");
                lblCurrency1.Text = (DS.Tables[0].Rows[0]["Currency"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["Currency"]);
                lblBuyer.Text = (DS.Tables[0].Rows[0]["Buyer"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["Buyer"]);
                //hdnBuyerCode.Value = (DS.Tables[0].Rows[0]["BuyerCode"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["BuyerCode"]);


                gvProduct.DataSource = DS.Tables[1];
                gvProduct.DataBind();

                double sumQ = Convert.ToDouble(DS.Tables[1].Compute("SUM(Quantity)", string.Empty));
                double sumV = Convert.ToDouble(DS.Tables[1].Compute("SUM(NetAmt)", string.Empty));

                //DataRow[] dr1 = DS.Tables[1].Select("SUM(Quantity)");
                //Blocked By sonali 25.6.2018 
                // (gvProduct.FooterRow.FindControl("lblQtySum") as Label).Text = Convert.ToString(sumQ);
                //Added By sonali 25.6.2018
                (gvProduct.FooterRow.FindControl("lblQtySum") as Label).Text = sumQ.ToString("0.00");

                //Modified by Mainak 2017-11-29
                //(gvProduct.FooterRow.FindControl("lblValueSum") as Label).Text = Convert.ToString(sumV);
                (gvProduct.FooterRow.FindControl("lblValueSum") as Label).Text = sumV.ToString("0.00");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "$('#addModal').modal('show');", true);
            }
            catch
            {
                //do nothing
            }
        }

        //protected void btnAddModal_Click(object sender, EventArgs e)
        //{
        //    string test = hdnRowNo.Value;
        //    string[] arrIndex = test.Split(',');
        //    arrIndex = arrIndex.Take(arrIndex.Count() - 1).ToArray();
        //    //DataTable dt = ViewState["LineItemsDT"] as DataTable;
        //    DataTable dt = (ViewState["LineItemsDT"] as DataTable).Clone();
        //    //Updated by Mainak 2017-11-21
        //    foreach (GridViewRow row in grdInvoiceDetails.Rows)
        //    {
        //        DataRow newRow = dt.NewRow();
        //        newRow["PONO"] = (row.FindControl("txtPONO") as TextBox).Text;
        //        newRow["BUYERCODE"] = (row.FindControl("txtBuyerCode") as TextBox).Text;
        //        newRow["DESC"] = (row.FindControl("txtDescription") as TextBox).Text;
        //        newRow["QTY"] = (row.FindControl("txtQTY") as TextBox).Text;
        //        newRow["PRICE"] = (row.FindControl("txtPRICE") as TextBox).Text;
        //        newRow["VALUE"] = (row.FindControl("txtVALUE") as TextBox).Text;
        //        newRow["GOODSRECDDETAILID"] = (row.FindControl("lblGoodsRecdDetailID") as Label).Text;
        //        newRow["DEPARTMENTID"] = (row.FindControl("lblDeptID") as Label).Text;
        //        newRow["NOMINALCODEID"] = (row.FindControl("lblNominalCodeID") as Label).Text;
        //        newRow["BUSINESSUNITID"] = (row.FindControl("lblBusinessUnitID") as Label).Text;
        //        newRow["PROJECTCODE"] = (row.FindControl("lblProjectCode") as Label).Text;
        //        newRow["PURORDERLINENO"] = (row.FindControl("lblPurOrderLineNo") as Label).Text;//Added by Mainak 2018-05-31
        //        dt.Rows.Add(newRow);
        //    }

        //    ViewState["LineItemsDT"] = dt;

        //    //Commented By Mainak 2017-11-28
        //    //if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[dt.Rows.Count - 1]["PONO"])))
        //    //{
        //    //    dt.Rows.RemoveAt(dt.Rows.Count - 1);
        //    //}

        //    foreach (var index in arrIndex)
        //    {
        //        DataRow newRow = dt.NewRow();
        //        newRow["PONO"] = lblPO.Text;
        //        newRow["BUYERCODE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBuyerCode") as Label).Text;
        //        newRow["DESC"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDesc") as Label).Text;
        //        newRow["QTY"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblQty") as Label).Text;
        //        newRow["PRICE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPrice") as Label).Text;
        //        newRow["VALUE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblValue") as Label).Text;
        //        newRow["GOODSRECDDETAILID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblGoodsRecdDetailID") as Label).Text;
        //        newRow["DEPARTMENTID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDeptID") as Label).Text;
        //        newRow["NOMINALCODEID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblNominalCodeID") as Label).Text;
        //        newRow["BUSINESSUNITID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBusinessUnitID") as Label).Text;
        //        newRow["PROJECTCODE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblProjectCode") as Label).Text;
        //        newRow["PURORDERLINENO"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPurOrderLineNo") as Label).Text;//Added by Mainak 2018-05-31
        //        dt.Rows.Add(newRow);
        //    }

        //    ViewState["LineItemsDT"] = dt;
        //    grdInvoiceDetails.DataSource = dt;
        //    grdInvoiceDetails.DataBind();
        //}


        //Added By Mainak  2018-10-26
        protected void btnReplaceModal_Click(object sender, EventArgs e)
        {
            string test = hdnRowNo.Value;
            if (test != "")
            {
                string[] arrIndex = test.Split(',');
                arrIndex = arrIndex.Take(arrIndex.Count() - 1).ToArray();
                DataTable dt = (ViewState["LineItemsDT"] as DataTable).Clone();

                foreach (var index in arrIndex)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["PurOrderNo"] = lblPO.Text;
                    newRow["BuyersProdCode"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBuyerCode") as Label).Text;
                    newRow["Description"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDesc") as Label).Text;
                    newRow["Quantity"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblQty") as Label).Text;
                    newRow["Rate"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPrice") as Label).Text;
                    newRow["New_NettValue"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblValue") as Label).Text;
                    newRow["VATRate"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblVatRate") as Label).Text;
                    newRow["VATAmt"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblVat") as Label).Text;
                    newRow["TotalAmt"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblGrossValue") as Label).Text;
                    //Commented By Mainak 2018-10-26
                    //newRow["GOODSRECDDETAILID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblGoodsRecdDetailID") as Label).Text;
                    //newRow["DEPARTMENTID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDeptID") as Label).Text;
                    //newRow["NOMINALCODEID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblNominalCodeID") as Label).Text;
                    //newRow["BUSINESSUNITID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBusinessUnitID") as Label).Text;
                    //newRow["PROJECTCODE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblProjectCode") as Label).Text;
                    newRow["PurOrderLineNo"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPurOrderLineNo") as Label).Text;//Added by Mainak 2018-05-31
                    dt.Rows.Add(newRow);
                }

                ViewState["LineItemsDT"] = dt;
                grdInvoiceDetails.DataSource = dt;
                grdInvoiceDetails.DataBind();


                double NetTotal = 0;
                double Vat = 0;
                double TotalAmount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    NetTotal += Convert.ToDouble(dr["New_NettValue"].ToString());
                    Vat += Convert.ToDouble(dr["VATAmt"].ToString());
                    TotalAmount = NetTotal + Vat;
                }
                txtNetTotal.Text = Convert.ToString(NetTotal);
                txtVATAmtNew.Text = Convert.ToString(Vat);
                txtTotAmt.Text = Convert.ToString(TotalAmount);
            }
            else
            {
                string message = "alert('Please tick line(s)')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
        }

        //Added By Mainak  2018-10-26
        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            string test = hdnRowNo.Value;
            if (test != "")
            {
                string[] arrIndex = test.Split(',');
                arrIndex = arrIndex.Take(arrIndex.Count() - 1).ToArray();
                //DataTable dt = ViewState["LineItemsDT"] as DataTable;
                DataTable dt = (ViewState["LineItemsDT"] as DataTable).Clone();

                //Updated by Mainak 2017-11-21
                foreach (GridViewRow row in grdInvoiceDetails.Rows)
                {
                    DataRow newRow = dt.NewRow();

                    string PurOrderNo_temp = (row.FindControl("txtPurOrderNo") as TextBox).Text;
                    if (PurOrderNo_temp == "" || PurOrderNo_temp == null)
                    {
                        PurOrderNo_temp = "";
                    }
                    newRow["PurOrderNo"] = PurOrderNo_temp;

                    string BuyersProdCode_temp = (row.FindControl("txtBuyersProdCode") as TextBox).Text;
                    if (BuyersProdCode_temp == "" || BuyersProdCode_temp == null)
                    {
                        BuyersProdCode_temp = "";
                    }
                    newRow["BuyersProdCode"] = BuyersProdCode_temp;

                    newRow["Description"] = (row.FindControl("txtDesc") as TextBox).Text;

                    string Quantity_temp = (row.FindControl("txtQuantity") as TextBox).Text;
                    if (Quantity_temp == "" || Quantity_temp == null)
                    {
                        Quantity_temp = "0";
                    }
                    newRow["Quantity"] = Quantity_temp;

                    string Rate_temp = (row.FindControl("txtPrice") as TextBox).Text;
                    if (Rate_temp == "" || Rate_temp == null)
                    {
                        Rate_temp = "0";
                    }
                    newRow["Rate"] = Rate_temp;

                    //newRow["Rate"] = (row.FindControl("txtPrice") as TextBox).Text;
                    //Added By mainak, 2018-11-5
                    string New_NettValue_temp = (row.FindControl("txtNetValue") as TextBox).Text;
                    if (New_NettValue_temp == "" || New_NettValue_temp == null)
                    {
                        New_NettValue_temp = "0";
                    }
                    newRow["New_NettValue"] = Convert.ToDecimal(New_NettValue_temp);

                    string VATRate_temp = (row.FindControl("dpVATRate") as DropDownList).SelectedValue;
                    if (VATRate_temp == "" || VATRate_temp == null || VATRate_temp == "Select")
                    {
                        VATRate_temp = "0";
                    }
                    newRow["VATRate"] = VATRate_temp;

                    //newRow["VATAmt"] = (row.FindControl("txtVAT") as TextBox).Text;

                    string VATAmt_temp = (row.FindControl("txtVAT") as TextBox).Text;
                    if (VATAmt_temp == "" || VATAmt_temp == null)
                    {
                        VATAmt_temp = "0";
                    }
                    newRow["VATAmt"] = Convert.ToDecimal(VATAmt_temp);

                    string txtGrossValue_temp = (row.FindControl("txtGrossValue") as TextBox).Text;
                    if (txtGrossValue_temp == "" || txtGrossValue_temp == null)
                    {
                        txtGrossValue_temp = "0";
                    }
                    newRow["TotalAmt"] = Convert.ToDecimal(txtGrossValue_temp);
                    //Commented By Mainak 2018-10-26
                    //newRow["GOODSRECDDETAILID"] = (row.FindControl("lblGoodsRecdDetailID") as Label).Text;
                    //newRow["DEPARTMENTID"] = (row.FindControl("lblDeptID") as Label).Text;
                    //newRow["NOMINALCODEID"] = (row.FindControl("lblNominalCodeID") as Label).Text;
                    //newRow["BUSINESSUNITID"] = (row.FindControl("lblBusinessUnitID") as Label).Text;
                    //newRow["PROJECTCODE"] = (row.FindControl("lblProjectCode") as Label).Text;
                    string PurOrderLineNo_temp = (row.FindControl("txtPOLineNo") as TextBox).Text;
                    if (PurOrderLineNo_temp == "" || PurOrderLineNo_temp == null)
                    {
                        PurOrderLineNo_temp = "0";
                    }
                    newRow["PurOrderLineNo"] = PurOrderLineNo_temp;//Added by Mainak 2018-05-31
                    dt.Rows.Add(newRow);
                }

                ViewState["LineItemsDT"] = dt;

                //Commented By Mainak 2017-11-28
                //if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[dt.Rows.Count - 1]["PONO"])))
                //{
                //    dt.Rows.RemoveAt(dt.Rows.Count - 1);
                //}

                foreach (var index in arrIndex)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["PurOrderNo"] = lblPO.Text;
                    newRow["BuyersProdCode"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBuyerCode") as Label).Text;
                    newRow["Description"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDesc") as Label).Text;
                    newRow["Quantity"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblQty") as Label).Text;
                    newRow["Rate"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPrice") as Label).Text;
                    newRow["New_NettValue"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblValue") as Label).Text;
                    newRow["VATRate"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblVatRate") as Label).Text;
                    newRow["VATAmt"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblVat") as Label).Text;
                    newRow["TotalAmt"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblGrossValue") as Label).Text;
                    //Commented By Mainak 2018-10-26
                    //newRow["GOODSRECDDETAILID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblGoodsRecdDetailID") as Label).Text;
                    //newRow["DEPARTMENTID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDeptID") as Label).Text;
                    //newRow["NOMINALCODEID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblNominalCodeID") as Label).Text;
                    //newRow["BUSINESSUNITID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBusinessUnitID") as Label).Text;
                    //newRow["PROJECTCODE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblProjectCode") as Label).Text;
                    newRow["PurOrderLineNo"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPurOrderLineNo") as Label).Text;//Added by Mainak 2018-05-31
                    dt.Rows.Add(newRow);




                }
                //DataSet dsInvoicedetail = new DataSet();
                //dsInvoicedetail = dt.DataSet;


                ViewState["LineItemsDT"] = dt;
                grdInvoiceDetails.DataSource = dt;
                grdInvoiceDetails.DataBind();
                dt.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                dt.WriteXml(Session["XMLInvoiceDetailFile"].ToString());


                double NetTotal = 0;
                double Vat = 0;
                double TotalAmount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    NetTotal += Convert.ToDouble(dr["New_NettValue"] == "" ? 0 : dr["New_NettValue"]);
                    Vat += Convert.ToDouble(dr["VATAmt"] == "" ? 0 : dr["VATAmt"]);
                    TotalAmount = NetTotal + Vat;
                }
                txtNetTotal.Text = Convert.ToString(NetTotal);
                txtVATAmtNew.Text = Convert.ToString(Vat);
                txtTotAmt.Text = Convert.ToString(TotalAmount);
            }
            else
            {
                string message = "alert('Please tick line(s)')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }

        }
    }


    /*
        public static MonthName(int month)
        {
            return MonthName(month, false);
        }
        public static MonthName(int month, bool abbreviate)
        {
            if ((month < 1) || (month > 12))
           {
                throw new ArgumentOutOfRangeException("month");
           }
            if (abbreviate)
            {
                return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[month-1];
            }
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[month-1];
        }
    */

}
