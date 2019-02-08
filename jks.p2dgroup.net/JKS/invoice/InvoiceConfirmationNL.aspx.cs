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
using System.Data.SqlClient;
using CBSolutions.ETH.Web;
using CBSolutions.ETH.Web.twinfield.Invoice;

namespace JKS
{
    /// <summary>
    /// Summary description for InvoiceConfirmationNL.
    /// </summary>
    public partial class InvoiceConfirmationNL : SVSPage//was CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        //protected System.Web.UI.WebControls.Panel Panel4;
        //protected System.Web.UI.WebControls.Label Label6;
        //protected System.Web.UI.WebControls.Label lblInvoiceNo;		
        //protected System.Web.UI.WebControls.Button btnPDFGenerate;
        //protected System.Web.UI.WebControls.Label lblRefernce;
        //protected System.Web.UI.WebControls.Label lblVATRegNo;
        //protected System.Web.UI.WebControls.Label lblTermsDiscount;
        //protected System.Web.UI.WebControls.Label lblInvoiceDate;
        //protected System.Web.UI.WebControls.Label lblPaymentTerms;
        //protected System.Web.UI.WebControls.Label lblTotal;
        //protected System.Web.UI.WebControls.Label lblDeliveryAddress;
        //protected System.Web.UI.WebControls.Label lblInvoiceAddress;
        //protected System.Web.UI.WebControls.Button btnConfirmInvoice;
        //protected System.Web.UI.WebControls.Button btnGeneratePDF;
        //protected System.Web.UI.WebControls.Label lblConfirmation;
        //protected System.Web.UI.WebControls.DataGrid grdInvoiceDetails;
        //protected System.Web.UI.WebControls.Button btnGenerateText;
        //protected System.Web.UI.WebControls.Label lblSupplierAddress;
        //protected System.Web.UI.WebControls.Label lblNetTotal;
        //protected System.Web.UI.WebControls.Label lblSupplier;
        //protected System.Web.UI.WebControls.Label lblBuyer;
        //protected System.Web.UI.WebControls.Label lblGBPEquiAmt;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdGBPEquiFlag;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdSaveFlag;
        //protected System.Web.UI.WebControls.Label lblCustomerContactName;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdHideBack;
        #endregion

        #region User Defined Variables
        RecordSet rsInvoiceHead = null;
        RecordSet rsInvoiceDetail = null;
        protected string strInvoiceDocumentDownloadPath = ConfigurationManager.AppSettings["InvoiceDocPath"];
        protected string strInvoiceDocument = "";
        protected string iInvID = "";
        //protected System.Web.UI.WebControls.Label lblSettlementDays;
        //protected System.Web.UI.WebControls.Label lblCustomerAccNo;
        //protected System.Web.UI.WebControls.Label lblInvoiceName;
        //protected System.Web.UI.WebControls.Label lblSecondSettlementDiscount;
        //protected System.Web.UI.WebControls.Label lblSecondSettlementDays;
        //protected System.Web.UI.WebControls.Label lblPaymentDueDAte;
        //protected System.Web.UI.WebControls.Label lblTaxPointDate;
        //protected System.Web.UI.WebControls.Label lblOverAllDiscount;
        //protected System.Web.UI.WebControls.Label lblCurrency;
        //protected System.Web.UI.WebControls.Label lblStatus;
        //protected System.Web.UI.WebControls.Label lblDespatchNoteNo;
        //protected System.Web.UI.WebControls.Label lblDespatchDate;
        //protected System.Web.UI.WebControls.TextBox txtSterlingEquivalent;
        //protected System.Web.UI.WebControls.Label lblMessage;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden Hidden1;
        //protected System.Web.UI.HtmlControls.HtmlTableRow trInputSterlingEquiAmt;
        //protected System.Web.UI.WebControls.TextBox txtVATAmt;
        //protected System.Web.UI.WebControls.HyperLink HyperLink1;
        //protected System.Web.UI.WebControls.Label lblVAT;
        //protected System.Web.UI.WebControls.Label lblpaymentdate;
        //protected System.Web.UI.WebControls.Label lblPaymentMethod;
        //protected System.Web.UI.WebControls.Label lblDiscount;
        //protected System.Web.UI.WebControls.Label lblActivityCode;
        //protected System.Web.UI.WebControls.Label lblAccountCat;
        //protected System.Web.UI.WebControls.Button btnEdit;
        //protected System.Web.UI.WebControls.Label lblInvNo;
        protected int invoiceID = 0;
        #endregion

        #region Sql Variables
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        #endregion

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            if (Request["hd"] != null)
            {
                hdHideBack.Value = "1";
            }

            Session.Remove("DuplicateInvoice");
            btnGeneratePDF.Visible = false;
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
            this.btnConfirmInvoice.Click += new System.EventHandler(this.btnConfirmInvoice_Click);
            this.btnGeneratePDF.Click += new System.EventHandler(this.btnGeneratePDF_Click);
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.Load += new System.EventHandler(this.Page_Load);
            this.grdInvoiceDetails.ItemDataBound += new DataGridItemEventHandler(grdInvoiceDetails_ItemDataBound);

        }
        #endregion

        #region PopulateData
        public void PopulateData(int invoiceID)
        {
            rsInvoiceHead = Invoice.GetInvoiceHead(invoiceID);
            rsInvoiceDetail = Invoice.GetInvoiceDetail(invoiceID);
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
            Invoice objInvoice = new Invoice();
            this.grdInvoiceDetails.DataSource = rsInvoiceDetail.ParentTable;
            this.grdInvoiceDetails.DataBind();
            lblInvNo.Text = rsInvoiceHead["InvoiceNo"].ToString();

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

            try
            {
                if (rsInvoiceHead["CurrencyTypeID"] != DBNull.Value)
                    lblCurrency.Text = objInvoice.GetCurrencyName(Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]));

            }
            catch { }

            lblVATRegNo.Text = rsInvoiceHead["SellerVATNo"].ToString();

            if (rsInvoiceHead["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(rsInvoiceHead["InvoiceDate"]).ToString("dd/MM/yyyy");
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

            RecordSet rs = Company.GetCompanyData(System.Convert.ToInt32(rsInvoiceHead["SupplierCompanyID"]));
            lblSupplier.Text = rs["CompanyName"].ToString();
            rs = Company.GetCompanyData(System.Convert.ToInt32(rsInvoiceHead["BuyerCompanyID"]));
            lblBuyer.Text = rs["CompanyName"].ToString();
            if (rsInvoiceHead["StatusId"] != DBNull.Value)
                lblStatus.Text = Invoice.GetStatus((int)rsInvoiceHead["StatusId"]);
            else
                lblStatus.Text = "Pending";


            if (rsInvoiceHead["StatusId"].ToString() == "6" || rsInvoiceHead["StatusId"].ToString() == "26" || rsInvoiceHead["StatusId"].ToString() == "25")
            {
                btnEdit.Visible = true;
            }
            else
            {
                btnEdit.Visible = false;
            }
            if (Request.QueryString["AllowEdit"] != null)
            {
                btnEdit.Visible = true;
            }
            else
            {
                btnEdit.Visible = false;
            }

            if (Convert.ToInt32(Session["UserTypeID"]) >= 2)
            {
                btnEdit.Visible = true;
            }
            else
            {
                btnEdit.Visible = false;
            }

            // Added by Sougata on 10th December 2014
            //if (Convert.ToInt32(Session["EditInvoice"]) > 0)
            //{
            //    btnEdit.Visible = true;
            //}

            // Added by Mrinal on 4th Feb 2015
            IsAutorisedtoEditData();


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

            //=============Added By Rimi on 7thAugust2015==================

            if (rsInvoiceHead["CurrencyTypeID"] == "" || rsInvoiceHead["CurrencyTypeID"] == null || rsInvoiceHead["CurrencyTypeID"] == System.DBNull.Value)
            {
                rsInvoiceHead["CurrencyTypeID"] = 0;
            }

            //=============Added By Rimi on 7thAugust2015 End==================

            if (Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]) != 22 && Session["StrVATAmt"] == null && ViewState["INID"] == null)
            {
                txtSterlingEquivalent.Visible = true;
                trInputSterlingEquiAmt.Visible = true;
            }
            else
                trInputSterlingEquiAmt.Visible = false;
        }
        #endregion

        #region btnConfirmInvoice_Click
        private void btnConfirmInvoice_Click(object sender, System.EventArgs e)
        {
            Invoice invoice = new Invoice();
            //save the invoice head and detail data in a single transaction context
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            da.BeginTransaction();

            int invoicePKID = 0;
            //save invoice head data
            rsInvoiceHead["SupplierCompanyID"] = (int)Session["CompanyID"];

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
            //save invoice detail data
            if (invoicePKID > 0)
                invoice.InsertInvoiceDetailData(invoicePKID, rsInvoiceDetail, da);
            //if error rollback
            if (da.ErrorCode != DataAccessErrors.Successful)
            {
                da.RollbackTransaction();
                rsInvoiceHead["StatusID"] = null;
                Response.Write(da.ErrorMessage);
            }
            else //else commit transaction
            {
                // CHECKING IF CURRENCY CODE IS NOT GBP.
                if (Convert.ToInt32(rsInvoiceHead["CurrencyTypeID"]) != 22)
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
                            Session["StrVATAmt"] = Math.Round(Convert.ToDouble(txtSterlingEquivalent.Text.Trim()), 2);
                            da.CommitTransaction();

                        }
                        else
                        {
                            lblMessage.Text = "Please enter a numeric value for sterling equivalent vat amount.";
                            da.RollbackTransaction();
                            rsInvoiceHead["StatusID"] = null;
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
                    if (Utility.IsNumeric(Session["StrVATAmt"].ToString().Trim()))
                    {
                        Session["StrVATAmt"] = Convert.ToString(Math.Round(Convert.ToDouble(Session["StrVATAmt"].ToString().Trim()), 2));
                    }
                invoice.UpdateSterlingAmoutnIfCurrencyNotGBP(invoicePKID, Convert.ToDecimal(Session["StrVATAmt"]));
                if (txtVATAmt.Text.Trim() != null)
                    invoice.UpdateVATAmount(invoicePKID, Convert.ToDecimal(txtVATAmt.Text.Trim()));


                CopyFile(invoicePKID.ToString());
                Session["InvoiceID"] = invoicePKID;
                hdSaveFlag.Value = "1";

                hdHideBack.Value = "1";
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
                    Invoice objInvoice = new Invoice();
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
            rptInvoiceNL3 rpt = new rptInvoiceNL3(rsInvoiceHead, rsInvoiceDetail);
            rpt.PageSettings.Orientation = PageOrientation.Landscape;

            DataDynamics.ActiveReports.Export.Pdf.PdfExport pdf = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
            string pdfFile = (string)Session["InvoicePDF"];
            pdfFile = "../Files/" + System.IO.Path.GetFileName(pdfFile);
            rpt.Run();
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

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (Request.QueryString["AllowEdit"] != null)
                Response.Redirect("InvoiceEdit.aspx?InvoiceID=" + invoiceID + "&AllowEdit=" + Request.QueryString["AllowEdit"].ToString());
        }

        private void grdInvoiceDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                if (GetPONumberForSupplierBuyer(((TextBox)e.Item.FindControl("txtPurOrederNo")).Text) != "Y")
                {
                    e.Item.BackColor = Color.Red;
                }
                else
                {

                }
            }
        }
        private string GetPONumberForSupplierBuyer(string Ponumber)
        {
            string existscheck = "";
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter("sp_PoNumberForSupplerAnainstBuyerAkkeron", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", invoiceID);
            sqlDA.SelectCommand.Parameters.Add("@PoNumber", Ponumber);
            sqlDA.SelectCommand.Parameters.Add("@Type", "INV");


            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            if (Dst.Tables.Count > 0)
            {
                if (Dst.Tables[0].Rows.Count > 0)
                {
                    existscheck = Convert.ToString(Dst.Tables[0].Rows[0]["Exists"]);
                }
            }
            return existscheck;

        }

        #region : Added by Mrinal on 4th Feb 2015 For Edit Invoice Requirement
        protected void IsAutorisedtoEditData()
        {
            if (Convert.ToInt32(Session["UserTypeID"]) == 2 || Convert.ToInt32(Session["UserTypeID"]) == 3)
            {
                btnEdit.Visible = true;
                return;
            }
            else
            {
                btnEdit.Visible = false;
            }

            if (Session["UserID"] != null)
            {
                SqlConnection sqlConn = null;
                sqlConn = new SqlConnection(ConsString);
                string strUserID = Convert.ToString(Session["UserID"]);
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("sp_IsAuthorisedToEditData", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@UserID", strUserID);
                DataSet ds = new DataSet();
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(ds);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int IsAuthorised = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                        if (IsAuthorised > 0)
                        {

                            btnEdit.Visible = true;
                        }
                        else
                        {

                            btnEdit.Visible = false;
                        }
                    }
                    else
                    {
                        btnEdit.Visible = false;
                    }
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
                finally
                {
                    sqlDA.Dispose();
                    sqlConn.Close();
                }
            }
            else
            {
            }
        }
        #endregion
    }
}
