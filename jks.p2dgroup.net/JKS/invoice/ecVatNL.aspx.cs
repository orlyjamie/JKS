using System;
using System.Configuration;
using System.IO;
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
using CBSolutions.Architecture.Core;


namespace JKS
{
    /// <summary>
    /// Summary description for ecVatNL.
    /// </summary>
    public class ecVatNL : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region Controls
        protected System.Web.UI.WebControls.Panel Panel4;
        protected System.Web.UI.WebControls.Label lblConfirmation;
        protected System.Web.UI.WebControls.DataGrid grdInvoiceDetails;
        #endregion

        #region User Defined Variables
        private RecordSet rsInvoiceHead = null;
        private RecordSet rsInvoiceDetail = null;
        protected string iInvID = "";
        protected int invoiceID = 0;

        protected string strSellerVatRegNo = "";
        protected string strTradersReference = "";
        protected string strCountryTaxNo = "";
        protected string strInvoiceNo = "";
        protected string strInvoiceDate = "";
        protected string strCurrency = "";
        protected string strVatInGBP = "";

        protected decimal dNetAmount = 0;
        protected decimal dVatAmount = 0;
        protected decimal dGrossAmount = 0;

        protected string strNetAmount = "";
        protected string strVatAmount = "";
        protected string strGrossAmount = "";
        #endregion

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");


            invoiceID = 0;
            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
                ViewState["INID"] = invoiceID.ToString();
                iInvID = invoiceID.ToString();
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
        #endregion


        #region POPULATE DATA METHOD OVERLOADED
        #region PopulateData
        public void PopulateData(int invoiceID)
        {
            rsInvoiceHead = Invoice.GetInvoiceHead(invoiceID);
            rsInvoiceDetail = Invoice.GetInvoiceDetail(invoiceID);

            PopulateData();
        }
        #endregion
        #region PopulateData
        public void PopulateData()
        {
            int iCurrencyTypeID = 0;
            Invoice objInvoice = new Invoice();
            Company objCompany = new Company();

            grdInvoiceDetails.DataSource = rsInvoiceDetail.ParentTable;
            grdInvoiceDetails.DataBind();

            if (rsInvoiceHead["InvoiceNo"] != DBNull.Value)
                strInvoiceNo = rsInvoiceHead["InvoiceNo"].ToString().Trim();

            if (rsInvoiceHead["InvoiceDate"] != DBNull.Value)
                strInvoiceDate = Convert.ToDateTime(rsInvoiceHead["InvoiceDate"]).ToString("dd/MM/yyyy");

            if (rsInvoiceHead["SellerVATNo"] != DBNull.Value)
                strSellerVatRegNo = rsInvoiceHead["SellerVATNo"].ToString().Trim();

            strTradersReference = objCompany.GetTradersReference(Convert.ToInt32(rsInvoiceHead["SupplierCompanyID"]));

            if (rsInvoiceHead["New_TaxCountryNumber"] != DBNull.Value)
                strCountryTaxNo = rsInvoiceHead["New_TaxCountryNumber"].ToString().Trim();

            if (rsInvoiceHead["CurrencyTypeID"] != DBNull.Value)
            {
                iCurrencyTypeID = Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]);
                strCurrency = objInvoice.GetCurrencyName(Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]));
            }

            #region Manipulating VAT IN GBP amount
            if (Session["StrVATAmt"] != null)
            {
                if (Utility.IsNumeric(Session["StrVATAmt"].ToString().Trim()))
                {
                    strVatInGBP = Convert.ToString(Math.Round(Convert.ToDouble(Session["StrVATAmt"].ToString().Trim()), 2));
                }
            }
            else
            {
                Invoice oInvoice = new Invoice();
                Double dGBPEquivalentAmount = 0;
                dGBPEquivalentAmount = oInvoice.GetGBPEquivalentAmount(invoiceID);

                if (dGBPEquivalentAmount != 0)
                {
                    strVatInGBP = dGBPEquivalentAmount.ToString();
                    if (Utility.IsNumeric(strVatInGBP))
                    {
                        strVatInGBP = Convert.ToString(Math.Round(Convert.ToDouble(strVatInGBP), 2));
                    }
                }
                oInvoice = null;
            }
            #endregion
        }
        #endregion
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
            this.grdInvoiceDetails.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvoiceDetails_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion


        #region grdInvoiceDetails_ItemDataBound
        private void grdInvoiceDetails_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                strNetAmount = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "New_NettValue"));
                strVatAmount = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "VatAmt"));
                strGrossAmount = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "TotalAmt"));

                if (!strNetAmount.Trim().Equals(""))
                    dNetAmount = (dNetAmount + Convert.ToDecimal(strNetAmount.Trim()));
                if (!strVatAmount.Trim().Equals(""))
                    dVatAmount = (dVatAmount + Convert.ToDecimal(strVatAmount.Trim()));
                if (!strGrossAmount.Trim().Equals(""))
                    dGrossAmount = (dGrossAmount + Convert.ToDecimal(strGrossAmount.Trim()));
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblNetAmount_Sum = null;
                Label lblVatAmount_Sum = null;
                Label lblGrossAmount_Sum = null;

                lblNetAmount_Sum = (Label)(e.Item.FindControl("lblNetAmount_Sum"));
                lblVatAmount_Sum = (Label)(e.Item.FindControl("lblVatAmount_Sum"));
                lblGrossAmount_Sum = (Label)(e.Item.FindControl("lblGrossAmount_Sum"));

                lblNetAmount_Sum.Text = (Math.Round(dNetAmount, 2)).ToString();
                lblVatAmount_Sum.Text = (Math.Round(dVatAmount, 2)).ToString();
                lblGrossAmount_Sum.Text = (Math.Round(dGrossAmount, 2)).ToString();
            }
        }
        #endregion
    }
}
