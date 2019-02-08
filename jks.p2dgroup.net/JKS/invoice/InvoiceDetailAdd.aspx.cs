using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Xml;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Web.Mail;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Configuration;

namespace JKS
{
    /// <summary>
    /// Summary description for InvoiceDetailAdd.
    /// </summary>
    public partial class InvoiceDetailAdd : CBSolutions.ETH.Web.ETC.VSPage
    {
        //Commented ,Mainak 2018-11-1
        //protected System.Web.UI.WebControls.Panel Panel4;
        //protected System.Web.UI.WebControls.Label lblSerialNo;
        //protected System.Web.UI.WebControls.TextBox txtQuantity;
        //protected System.Web.UI.WebControls.DropDownList ddlModeOfTransport;
        //protected System.Web.UI.WebControls.TextBox txtRate;
        //protected System.Web.UI.WebControls.DropDownList ddlNatureOfTransaction;
        //protected System.Web.UI.WebControls.TextBox txtDescription;
        //protected System.Web.UI.WebControls.DropDownList ddlDeliveryTerms;
        //protected System.Web.UI.WebControls.DropDownList ddlItemType;
        //protected System.Web.UI.WebControls.DropDownList ddlCountryOfOrigin;
        //protected System.Web.UI.WebControls.TextBox txtUOM;
        //protected System.Web.UI.WebControls.TextBox tbCommodityCode;
        //protected System.Web.UI.WebControls.TextBox txtDiscountPercent;
        //protected System.Web.UI.WebControls.TextBox tbSupplementaryUnits;
        //protected System.Web.UI.WebControls.TextBox txtSecondDiscountPercent;
        //protected System.Web.UI.WebControls.TextBox tbNetMassInKilos;
        //protected System.Web.UI.WebControls.DropDownList cboVATRate;
        //protected System.Web.UI.WebControls.DropDownList ddlcolor;
        //protected System.Web.UI.WebControls.TextBox txtBuyersProdCode;
        //protected System.Web.UI.WebControls.TextBox txtSupplierProdCode;
        //protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator5;
        //protected System.Web.UI.WebControls.DropDownList cboYearPODate;
        //protected System.Web.UI.WebControls.DropDownList cboMonthPODate;
        //protected System.Web.UI.WebControls.DropDownList cboDayPODate;
        //protected System.Web.UI.WebControls.Label lblErrorPODate;
        //protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator3;
        //protected System.Web.UI.WebControls.TextBox txtPurOrderNo;
        //protected System.Web.UI.WebControls.TextBox txtPurOrderLineNo;
        //protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator8;
        //protected System.Web.UI.WebControls.TextBox tbDespatchNoteNo;
        //protected System.Web.UI.WebControls.CompareValidator CompareValidator3;
        //protected System.Web.UI.WebControls.DropDownList cboYearDeliveryDate;
        //protected System.Web.UI.WebControls.DropDownList cboMonthDeliveryDate;
        //protected System.Web.UI.WebControls.DropDownList cboDayDeliveryDate;
        //protected System.Web.UI.WebControls.Label lblErrorDeliveryDate;
        //protected System.Web.UI.WebControls.Button btnSubmit;
        //protected System.Web.UI.WebControls.Button btnContinue;
        //protected System.Web.UI.WebControls.Label lblBPCode;
        //protected System.Web.UI.WebControls.DataGrid grdInvoiceDetail;
        //protected System.Web.UI.HtmlControls.HtmlGenericControl BuyersProdCode;
        //protected System.Web.UI.HtmlControls.HtmlGenericControl PurOrderNo;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdContinueFlag;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnType;

        private CBSolutions.ETH.Web.Company objCompany = new CBSolutions.ETH.Web.Company();
        private DataTable dtbl = null;
        int currentYear = 0;
        SqlCommand sqlCmd;
        SqlConnection sqlConn;


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
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            this.grdInvoiceDetail.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvoiceDetail_ItemCreated);
            this.grdInvoiceDetail.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdInvoiceDetail_DeleteCommand);
            this.Load += new System.EventHandler(this.Page_Load);

        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");

            Session["SelectedPage"] = "CreateInvoice";
            Session.Remove("DuplicateInvoice");
            btnContinue.Enabled = false;
            this.grdInvoiceDetail.Visible = false;
            btnContinue.CssClass = "ContinueDisableButton_2";


            if (!IsPostBack)
            {
                LoadColor();
                lblBPCode.Visible = false;
                btnSubmit.Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to ADD line ?'); return fnValidate();");
                btnContinue.Attributes.Add("onclick", "javascript:return fn_Continue();");

                if (Session["BCompanyID"] != null)
                {
                    IsGMGCompany(Convert.ToInt32(Session["BCompanyID"]));
                }
                LoadData();
                PopulateData();
                GetVatList();

                if (GetStockType(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["BCompanyID"])) == 1)
                {
                    hdnType.Value = "1";
                    BuyersProdCode.Visible = true;
                    PurOrderNo.Visible = true;
                }
                else
                {
                    hdnType.Value = "0";
                    BuyersProdCode.Visible = false;
                    PurOrderNo.Visible = false;
                }
                Click_Submit();
            }

        }

        private void LoadColor()
        {
            SqlDataReader dr = null;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_LoadColor", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            try
            {
                sqlConn.Open();
                dr = sqlCmd.ExecuteReader();
                ddlcolor.Items.Clear();
                while (dr.Read())
                {
                    ddlcolor.Items.Add(new ListItem(dr["ColorCode"].ToString() + " - " + dr["ColorDescription"].ToString(), dr["ColorCode"].ToString()));
                }
                ddlcolor.Items.Insert(0, new ListItem("--Select--", "-1"));
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr.Close();
                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }

        private int GetStockType(int CompanyID, int BCompanyID)
        {
            int _rCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckNewLookSupplier", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@BuyerCompanyID", BCompanyID);
            sqlCmd.Parameters.Add("@SupplierCompanyID", CompanyID);

            sqlOutputParam = sqlCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                _rCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {

                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (_rCount);
        }

        public string GetDocumentType(int BuyerID, int SupplierID)
        {
            string strDocumentType = "";
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sGetDocumentType_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@BuyerID", BuyerID);
            sqlCmd.Parameters.Add("@SupplierID", SupplierID);

            sqlOutputParam = sqlCmd.Parameters.Add("@DocumentType", SqlDbType.VarChar, 100);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strDocumentType = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strDocumentType);
        }

        private void LoadData()
        {
            //populate date combos
            currentYear = Microsoft.VisualBasic.DateAndTime.Year(System.DateTime.Now);
            for (int i = currentYear; i < (currentYear + 10); i++)
            {
                cboYearPODate.Items.Add(i.ToString());
                cboYearDeliveryDate.Items.Add(i.ToString());
            }
            for (int i = 1; i < 13; i++)
            {
                cboMonthPODate.Items.Add(new ListItem(
                    Microsoft.VisualBasic.DateAndTime.MonthName(i, true), i.ToString()));
                cboMonthDeliveryDate.Items.Add(new ListItem(
                    Microsoft.VisualBasic.DateAndTime.MonthName(i, true), i.ToString()));
            }
            for (int i = 1; i < 32; i++)
            {
                cboDayPODate.Items.Add(i.ToString());
                cboDayDeliveryDate.Items.Add(i.ToString());
            }
            //load the VATTypes combo
            RecordSet rs = CBSolutions.ETH.Web.Invoice.GetVATTypesList();
            cboDayPODate.Items.Insert(0, new ListItem("Day", "0"));
            cboMonthPODate.Items.Insert(0, new ListItem("Month", "0"));
            cboYearPODate.Items.Insert(0, new ListItem("Year", "0"));

            cboDayDeliveryDate.Items.Insert(0, new ListItem("Day", "0"));
            cboMonthDeliveryDate.Items.Insert(0, new ListItem("Month", "0"));
            cboYearDeliveryDate.Items.Insert(0, new ListItem("Year", "0"));

            while (!rs.EOF())
            {
                ListItem listItem = new ListItem(rs["VATType"].ToString(), rs["VATTypeID"].ToString() + "*" + rs["Rate"].ToString());
                cboVATRate.Items.Add(listItem);
                rs.MoveNext();
            }
            cboVATRate.Items.FindByText("VAT at 15%").Selected = true;

        }

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

        private void PopulateData()
        {
            if (System.IO.File.Exists(Session["XSDInvoiceDetailFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceDetailFile"].ToString());

                grdInvoiceDetail.DataSource = ds.Tables[0];
                grdInvoiceDetail.DataBind();

                if ((Request.QueryString["SerialNo"] != null) &&
                    (string)Request.QueryString["Action"] == "delete")
                {

                    int srl = System.Convert.ToInt32(Request.QueryString["SerialNo"]);

                    DataTable dt = ds.Tables[0];
                    for (int i = 0, j = dt.Rows.Count; i < j; i++)
                    {
                        if (dt.Rows[i]["SerialNo"].ToString() == (string)Request.QueryString["SerialNo"])
                        {
                            DeleteInvoiceLineFromXML(i);
                            break;
                        }
                    }
                }
                else if (Request.QueryString["SerialNo"] != null)
                {
                    RecordSet rs = new RecordSet(ds);
                    int srl = System.Convert.ToInt32(Request.QueryString["SerialNo"]);
                    rs.Filter = "SerialNo = " + srl;

                    if (rs["SerialNo"] != DBNull.Value)
                        lblSerialNo.Text = rs["SerialNo"].ToString();
                    if (rs["Description"] != DBNull.Value)
                        txtDescription.Text = rs["Description"].ToString();
                    if (rs["Quantity"] != DBNull.Value)
                        txtQuantity.Text = rs["Quantity"].ToString();
                    if (rs["Rate"] != DBNull.Value)
                        txtRate.Text = rs["Rate"].ToString();
                    if (rs["UOM"] != DBNull.Value)
                        txtUOM.Text = rs["UOM"].ToString();
                    if (rs["Discount"] != DBNull.Value)
                        txtDiscountPercent.Text = rs["Discount"].ToString();
                    if (rs["New_DiscountPercent2"] != DBNull.Value)
                        txtSecondDiscountPercent.Text = rs["New_DiscountPercent2"].ToString();
                    if (rs["PurOrderNo"] != DBNull.Value)
                        txtPurOrderNo.Text = rs["PurOrderNo"].ToString();
                    if (rs["PurOrderLineNo"] != DBNull.Value)
                        txtPurOrderLineNo.Text = rs["PurOrderLineNo"].ToString();
                    DateTime dt;
                    if (rs["PurOrderDate"] != DBNull.Value)
                    {
                        dt = System.Convert.ToDateTime(rs["PurOrderDate"]);
                        cboYearPODate.SelectedIndex = (dt.Year - currentYear) + 1;
                        cboMonthPODate.SelectedIndex = dt.Month;
                        cboDayPODate.SelectedIndex = dt.Day;
                    }
                    if (rs["BuyersProdCode"] != DBNull.Value)
                        txtBuyersProdCode.Text = rs["BuyersProdCode"].ToString();
                    if (rs["SuppliersProdCode"] != DBNull.Value)
                        txtSupplierProdCode.Text = rs["SuppliersProdCode"].ToString();
                    if (rs["VATTypeId"] != DBNull.Value && rs["VATRate"] != DBNull.Value)
                        cboVATRate.SelectedValue = rs["VATTypeId"].ToString() + "*" + rs["VATRate"].ToString();
                    try
                    {
                        ddlItemType.SelectedValue = rs["New_Type"].ToString();
                    }
                    catch { }
                    if (rs["New_Definable1"] != DBNull.Value)
                        ddlcolor.Items.FindByValue(rs["New_Definable1"].ToString()).Selected = true;


                    if (rs["New_ModeOfTransport"] != DBNull.Value)
                        ddlModeOfTransport.SelectedValue = rs["New_ModeOfTransport"].ToString().Trim();

                    if (rs["New_NatureOfTransaction"] != DBNull.Value)
                        ddlNatureOfTransaction.SelectedValue = rs["New_NatureOfTransaction"].ToString().Trim();

                    if (rs["New_TermsOfDelivery"] != DBNull.Value)
                        ddlDeliveryTerms.SelectedValue = rs["New_TermsOfDelivery"].ToString().Trim();

                    if (rs["New_CountryOfOrigin"] != DBNull.Value)
                        ddlCountryOfOrigin.SelectedValue = rs["New_CountryOfOrigin"].ToString().Trim();

                    if (rs["New_CommodityCode"] != DBNull.Value)
                        tbCommodityCode.Text = rs["New_CommodityCode"].ToString().Trim();

                    if (rs["New_SupplementaryUnits"] != DBNull.Value)
                        tbSupplementaryUnits.Text = rs["New_SupplementaryUnits"].ToString().Trim();

                    if (rs["New_NettMass"] != DBNull.Value)
                        tbNetMassInKilos.Text = rs["New_NettMass"].ToString().Trim();

                    if (rs["New_DespatchNoteNumber"] != DBNull.Value)
                        tbDespatchNoteNo.Text = rs["New_DespatchNoteNumber"].ToString().Trim();

                    if (rs["New_DespatchDate"] != DBNull.Value)
                    {
                        dt = System.Convert.ToDateTime(rs["New_DespatchDate"]);

                        cboYearDeliveryDate.SelectedIndex = (dt.Year - currentYear) + 1;
                        cboMonthDeliveryDate.SelectedIndex = dt.Month;
                        cboDayDeliveryDate.SelectedIndex = dt.Day;
                    }

                }
                else
                {

                    int lastRowIndex = ds.Tables[0].Rows.Count - 1;
                    //Commented, Mainak 2018-11-1
                    //if (lastRowIndex >= 0)
                    //    lblSerialNo.Text = System.Convert.ToString(
                    //        System.Convert.ToInt32(
                    //        ds.Tables[0].Rows[lastRowIndex]["SerialNo"]) + 1);
                    //else
                        lblSerialNo.Text = "1";
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.grdInvoiceDetail.Visible = true;
                    btnContinue.Enabled = true;
                    hdContinueFlag.Value = "1";
                    btnContinue.CssClass = "ContinueButton_2";
                }
                else
                {
                    btnContinue.Enabled = false;
                    this.grdInvoiceDetail.Visible = false;
                    btnContinue.CssClass = "ContinueDisableButton_2";
                }
            }
            else
            {
                btnContinue.Enabled = false;
                lblSerialNo.Text = "1";
            }
        }

        private void DeleteInvoiceLineFromXML(int dataTableItemIndex)
        {
            RecordSet rs = null;
            //If XML file exists load XML into recordset
            if (System.IO.File.Exists(Session["XMLInvoiceDetailFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                rs = new RecordSet(ds);
            }
            //delete the specified line from the recorset
            DataTable dt = rs.ParentTable;
            dt.Rows.RemoveAt(dataTableItemIndex);
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
                decimal totalAmt = 0;
                decimal VATAmt = 0;
                decimal dNetValue = 0;
                decimal dOverallDiscountAmount = 0;
                rs.MoveFirst();
                while (!rs.EOF())
                {
                    if (rs["New_OverallDiscountValue"] != DBNull.Value)
                        dOverallDiscountAmount = dOverallDiscountAmount + System.Convert.ToDecimal(rs["New_OverallDiscountValue"]);
                    if (rs["New_NettValue"] != DBNull.Value)
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
            Response.Redirect("invoiceDetailAdd.aspx");
        }

        private void btnContinue_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("InvoiceConfirmation.aspx");
        }

        private bool CheckBPcodePO()
        {
            bool isRecExist = false;
            try
            {
                DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                RecordSet rs = da.ExecuteQuery("vwValidateBProdCodePurOrderNo", "buyerprodcode='" + txtBuyersProdCode.Text + "' and purorderno='" + txtPurOrderNo.Text + "'");
                while (!rs.EOF())
                {
                    isRecExist = true;
                    break;
                }
            }
            catch
            {
            }
            return isRecExist;
        }

        private bool CheckColorCodeAgainstBuyerProdCode()
        {
            bool sRetVal = false;
            SqlDataReader dr = null;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlConn.Open();

            sqlCmd = new SqlCommand("usp_CheckColorCodeAgainstBuyerProdCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@Color", ddlcolor.SelectedValue);
            sqlCmd.Parameters.Add("@BuyerProdCode", txtBuyersProdCode.Text.Trim());
            sqlCmd.Parameters.Add("@PurOrderNo", txtPurOrderNo.Text.Trim());
            dr = sqlCmd.ExecuteReader();

            while (dr.Read())
            {
                if (Convert.ToInt32(dr[0]) <= 0)
                {
                    sRetVal = false;
                }
                else
                {
                    sRetVal = true;
                }
            }

            dr.Close();
            sqlCmd.Dispose();
            sqlConn.Close();
            return sRetVal;
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (hdnType.Value == "1")
            {
                if (ddlItemType.SelectedItem.Text == "Product/Service")
                {
                    if (txtBuyersProdCode.Text.Trim() == "" || txtPurOrderNo.Text.Trim() == "")
                    {
                        lblBPCode.Visible = true;
                        return;
                    }
                    else
                    {
                        if (CheckBPcodePO() == false)
                        {
                            lblBPCode.Visible = true;
                            return;
                        }
                        else
                        {
                            lblBPCode.Visible = false;
                        }
                    }
                }

                if (ddlcolor.SelectedItem.Value != "-1")
                {
                    if (CheckColorCodeAgainstBuyerProdCode() == false)
                    {
                        lblBPCode.Text = "Color code does not match with Buyer's Product Code and PO Number";
                        lblBPCode.Visible = true;
                        return;
                    }
                    else
                    {
                        lblBPCode.Visible = false;
                    }
                }

            }

            if (CheckDate(System.Convert.ToInt32(cboYearPODate.SelectedValue),
                System.Convert.ToInt32(cboMonthPODate.SelectedValue),
                System.Convert.ToInt32(cboDayPODate.SelectedValue)) == false)
            {
                lblErrorPODate.Visible = true;
                return;
            }


            if (CheckDate(System.Convert.ToInt32(cboYearDeliveryDate.SelectedValue),
                System.Convert.ToInt32(cboMonthDeliveryDate.SelectedValue),
                System.Convert.ToInt32(cboDayDeliveryDate.SelectedValue)) == false)
            {
                lblErrorDeliveryDate.Visible = true;
                return;
            }
            RecordSet rs = null;
            //If XML file exists load XML into recordset
            if (System.IO.File.Exists(Session["XMLInvoiceDetailFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                rs = new RecordSet(ds);
            }

            else
            {
                DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                rs = da.CreateInsertBuffer("InvoiceDetail");
            }
            if (Request.QueryString["SerialNo"] != null)
            {
                int srl = System.Convert.ToInt32(Request.QueryString["SerialNo"]);
                rs.Filter = "SerialNo = " + srl;
            }
            else
            {

                rs.MoveLast();
                rs.MovePrevious();
                int srl = 0;
                if (rs["SerialNo"] != DBNull.Value)
                {
                    srl = System.Convert.ToInt32(rs["SerialNo"]);
                    rs.AddNew();
                }
                rs["SerialNo"] = System.Convert.ToInt32(lblSerialNo.Text);
            }


            rs["InvoiceDetailID"] = "0";
            rs["InvoiceID"] = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            rs["ModDate"] = DateTime.Now;
            //rs["ModStamp"] = DBNull.Value;
            rs["New_TaxCode"] = "S";
            rs["Description"] = txtDescription.Text;

            decimal quantity = txtQuantity.Text == "" ? 0 : System.Convert.ToDecimal(txtQuantity.Text);
            rs["Quantity"] = quantity;
            decimal rate = txtRate.Text == "" ? 0 : System.Convert.ToDecimal(txtRate.Text);
            rs["Rate"] = rate;



            decimal dNew_ExtendedValue = 0;
            dNew_ExtendedValue = GetAfterDecimalCalculatedValue(Convert.ToDecimal(quantity * rate));
            rs["GrossAmt"] = dNew_ExtendedValue;


            decimal discountPercent = txtDiscountPercent.Text == "" ? 0 : System.Convert.ToDecimal(txtDiscountPercent.Text);
            rs["Discount"] = discountPercent;
            decimal dSecondDiscountPercent = txtSecondDiscountPercent.Text == "" ? 0 : System.Convert.ToDecimal(txtSecondDiscountPercent.Text);
            rs["New_DiscountPercent2"] = GetAfterDecimalCalculatedValue(dSecondDiscountPercent);
            decimal dNew_DiscountValue1 = 0;
            if (discountPercent == 0)
                dNew_DiscountValue1 = 0;
            else
                dNew_DiscountValue1 = GetAfterDecimalCalculatedValue(System.Convert.ToDecimal(System.Convert.ToDecimal(rs["GrossAmt"]) * (discountPercent / 100))); //amitava 241007;

            if (dNew_DiscountValue1 != 0)
                rs["New_DiscountValue1"] = GetAfterDecimalCalculatedValue(dNew_DiscountValue1);
            else
                rs["New_DiscountValue1"] = DBNull.Value;

            decimal dNew_DiscountValue2 = 0;

            if (dSecondDiscountPercent == 0)
                dNew_DiscountValue2 = 0;
            else
                dNew_DiscountValue2 = GetAfterDecimalCalculatedValue((dNew_ExtendedValue - dNew_DiscountValue1) * (dSecondDiscountPercent / 100));  //amitava 241007;

            if (dNew_DiscountValue2 != 0)
                rs["New_DiscountValue2"] = dNew_DiscountValue2;
            else
                rs["New_DiscountValue2"] = DBNull.Value;

            decimal dNew_PostDiscountValue = 0;
            dNew_PostDiscountValue = dNew_ExtendedValue - dNew_DiscountValue1 - dNew_DiscountValue2;
            rs["New_PostDiscountValue"] = dNew_PostDiscountValue;

            string[] s = cboVATRate.SelectedItem.Value.Split('*');
            rs["VATTypeID"] = s[0];
            decimal VATRate = System.Convert.ToDecimal(s[1]);
            rs["VATRate"] = GetAfterDecimalCalculatedValue(VATRate);
            rs["UOM"] = txtUOM.Text;

            if (txtPurOrderNo.Text.Trim() != "")
                rs["PurOrderNo"] = txtPurOrderNo.Text;
            else
                rs["PurOrderNo"] = DBNull.Value;

            if (txtPurOrderLineNo.Text.Trim() != "")
                rs["PurOrderLineNo"] = txtPurOrderLineNo.Text;
            else
                rs["PurOrderLineNo"] = DBNull.Value;

            if (cboYearPODate.SelectedValue == "0" || cboMonthPODate.SelectedValue == "0" || cboDayPODate.SelectedValue == "0")
            {
                rs["PurOrderDate"] = DBNull.Value;
            }
            else
            {
                rs["PurOrderDate"] = cboYearPODate.SelectedValue + "/" + cboMonthPODate.SelectedValue + "/" + cboDayPODate.SelectedValue;
            }

            if (txtBuyersProdCode.Text.Trim() != "")
                rs["BuyersProdCode"] = txtBuyersProdCode.Text;
            else
                rs["BuyersProdCode"] = DBNull.Value;

            if (txtSupplierProdCode.Text.Trim() != "")
                rs["SuppliersProdCode"] = txtSupplierProdCode.Text;
            else
                rs["SuppliersProdCode"] = DBNull.Value;

            rs["New_Type"] = ddlItemType.SelectedValue;

            if (ddlcolor.SelectedItem.Value != "-1")
                rs["New_Definable1"] = ddlcolor.SelectedValue.ToString();
            else
                rs["New_Definable1"] = DBNull.Value;

            if (ddlModeOfTransport.SelectedValue.Trim() != "0")
                rs["New_ModeOfTransport"] = Convert.ToInt32(ddlModeOfTransport.SelectedValue.Trim());
            else
                rs["New_ModeOfTransport"] = DBNull.Value;

            if (ddlNatureOfTransaction.SelectedValue.Trim() != "0")
                rs["New_NatureOfTransaction"] = Convert.ToInt32(ddlNatureOfTransaction.SelectedValue.Trim());
            else
                rs["New_NatureOfTransaction"] = DBNull.Value;

            if (ddlDeliveryTerms.SelectedValue.Trim() != "0")
                rs["New_TermsOfDelivery"] = ddlDeliveryTerms.SelectedValue.Trim();
            else
                rs["New_TermsOfDelivery"] = DBNull.Value;

            if (ddlCountryOfOrigin.SelectedValue.Trim() != "0")
                rs["New_CountryOfOrigin"] = ddlCountryOfOrigin.SelectedValue.Trim();
            else
                rs["New_CountryOfOrigin"] = DBNull.Value;

            if (tbCommodityCode.Text.Trim() != "")
                rs["New_CommodityCode"] = Convert.ToInt32(tbCommodityCode.Text);
            else
                rs["New_CommodityCode"] = DBNull.Value;

            if (tbSupplementaryUnits.Text.Trim() != "")
                rs["New_SupplementaryUnits"] = tbSupplementaryUnits.Text.Trim();
            else
                rs["New_SupplementaryUnits"] = DBNull.Value;

            if (tbNetMassInKilos.Text.Trim() != "")
                rs["New_NettMass"] = Convert.ToDecimal(tbNetMassInKilos.Text);
            else
                rs["New_NettMass"] = DBNull.Value;

            if (tbDespatchNoteNo.Text.Trim() != "")
                rs["New_DespatchNoteNumber"] = tbDespatchNoteNo.Text.Trim();
            else
                rs["New_DespatchNoteNumber"] = DBNull.Value;

            if (cboYearDeliveryDate.SelectedValue != "0" && cboMonthDeliveryDate.SelectedValue != "0" && cboDayDeliveryDate.SelectedValue != "0")
                rs["New_DespatchDate"] = cboYearDeliveryDate.SelectedValue + "/" + cboMonthDeliveryDate.SelectedValue + "/" + cboDayDeliveryDate.SelectedValue;
            else
                rs["New_DespatchDate"] = DBNull.Value;

            if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead = new RecordSet(ds);

                decimal dNew_OverallDiscountValue = 0;
                if (rsHead["New_OverallDiscountPercent"] != DBNull.Value)
                    dNew_OverallDiscountValue = (dNew_PostDiscountValue * System.Convert.ToDecimal(rsHead["New_OverallDiscountPercent"])) / 100;
                else
                    dNew_OverallDiscountValue = 0;

                if (dNew_OverallDiscountValue != 0)
                    rs["New_OverallDiscountValue"] = dNew_OverallDiscountValue;
                else
                    rs["New_OverallDiscountValue"] = DBNull.Value;

                decimal dNew_NettValue = 0;
                dNew_NettValue = dNew_PostDiscountValue - dNew_OverallDiscountValue;
                if (dNew_NettValue != 0)
                    rs["New_NettValue"] = GetAfterDecimalCalculatedValue(dNew_NettValue);
                else
                    rs["New_NettValue"] = DBNull.Value;

                decimal dVatAmt = 0;
                if (VATRate != 0)
                {
                    if (rsHead["DiscountPercent"] != DBNull.Value)
                        dVatAmt = GetAfterDecimalCalculatedValue(dNew_NettValue * ((100 - System.Convert.ToDecimal(rsHead["DiscountPercent"])) / 100) * (VATRate / 100)); //Amitava
                    else
                        dVatAmt = GetAfterDecimalCalculatedValue(dNew_NettValue * (VATRate / 100));
                }
                else
                    dVatAmt = 0;

                rs["VATAmt"] = dVatAmt;
                rs["TotalAmt"] = GetAfterDecimalCalculatedValue(dNew_NettValue + dVatAmt);

                decimal totalAmt = 0;
                decimal VATAmt = 0;
                decimal dOverallDiscountAmount = 0;
                decimal dNetValue = 0;

                rs.MoveFirst();

                DataSet dsInvoicedetail = new DataSet();
                dsInvoicedetail = rs.ParentDataSet;
                dsInvoicedetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                dsInvoicedetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());

                totalAmt = 0;
                VATAmt = 0;
                dOverallDiscountAmount = 0;
                dNetValue = 0;

                for (int iCounter = 0; iCounter < dsInvoicedetail.Tables[0].Rows.Count; iCounter++)
                {
                    if (dsInvoicedetail.Tables[0].Rows[iCounter]["TotalAmt"] != DBNull.Value)
                        totalAmt = GetAfterDecimalCalculatedValue(totalAmt) + GetAfterDecimalCalculatedValue(Convert.ToDecimal(dsInvoicedetail.Tables[0].Rows[iCounter]["TotalAmt"]));
                    else
                        totalAmt = totalAmt + 0;

                    if (dsInvoicedetail.Tables[0].Rows[iCounter]["VATAmt"] != DBNull.Value)
                        VATAmt = GetAfterDecimalCalculatedValue(VATAmt + Convert.ToDecimal(dsInvoicedetail.Tables[0].Rows[iCounter]["VATAmt"]));
                    else
                        VATAmt = (VATAmt + 0);

                    if (dsInvoicedetail.Tables[0].Rows[iCounter]["New_NettValue"] != DBNull.Value)
                        dNetValue = GetAfterDecimalCalculatedValue(dNetValue + Convert.ToDecimal(dsInvoicedetail.Tables[0].Rows[iCounter]["New_NettValue"]));
                    else
                        dNetValue = (dNetValue + 0);

                    if (dsInvoicedetail.Tables[0].Rows[iCounter]["New_OverallDiscountValue"] != DBNull.Value)
                        dOverallDiscountAmount = GetAfterDecimalCalculatedValue(dOverallDiscountAmount + Convert.ToDecimal(dsInvoicedetail.Tables[0].Rows[iCounter]["New_OverallDiscountValue"]));
                    else
                        dOverallDiscountAmount = (dOverallDiscountAmount + 0);
                }

                rsHead["NetTotal"] = dNetValue;//This is NetAmount in InvoiceHeader
                rsHead["VATAmt"] = VATAmt;	 //This is TaxAmount in InvoiceHeader
                rsHead["TotalAmt"] = totalAmt; //This is GrossAmount in InvoiceHeader
                rsHead["New_OveralldiscountAmount"] = dOverallDiscountAmount;


                if (rsHead["DiscountPercent"] != DBNull.Value)
                    rsHead["New_SettlementAmount1"] = (System.Convert.ToDecimal(rsHead["DiscountPercent"]) / 100) * dNetValue;
                else
                    rsHead["New_SettlementAmount1"] = DBNull.Value;

                if (rsHead["New_SettlementPercent2"] != DBNull.Value)
                    rsHead["New_SettlementAmount2"] = (System.Convert.ToDecimal(rsHead["New_SettlementPercent2"]) / 100) * dNetValue;
                else
                    rsHead["New_SettlementAmount2"] = DBNull.Value;

                DataSet dsInvoiceHead = new DataSet();
                dsInvoiceHead = rsHead.ParentDataSet;
                dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());



            }
            rs.MoveFirst();
            DataSet dsInvoicedetailNEW = new DataSet();
            dsInvoicedetailNEW = rs.ParentDataSet;
            dsInvoicedetailNEW.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
            dsInvoicedetailNEW.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
            Response.Write("<script language=javascript>window.close(); window.opener.location.reload( false ); window.opener.focus();</script>");
        }

        private void grdInvoiceDetail_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            DeleteInvoiceLineFromXML(e.Item.DataSetIndex);
            Response.Redirect("invoiceDetailAdd.aspx");
        }

        private void grdInvoiceDetail_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink ctrl = (HyperLink)e.Item.FindControl("HyperlinkGridCol");
                if (ctrl != null)
                {
                    if (e.Item.DataItem != null)
                    {
                        DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                        string navigateURL = "InvoiceDetailAdd.aspx?" + "Action=edit&SerialNo=" + dr["SerialNo"].ToString();
                        ctrl.NavigateUrl = navigateURL;
                    }
                }
                ctrl = (HyperLink)e.Item.FindControl("HyperlinkDelete");
                if (ctrl != null)
                {
                    if (e.Item.DataItem != null)
                    {
                        DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                        string navigateURL = "InvoiceDetailAdd.aspx?" + "Action=delete&SerialNo=" + dr["SerialNo"].ToString();
                        ctrl.NavigateUrl = navigateURL;
                    }
                }
            }
        }

        private void ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink ctrl = (HyperLink)e.Item.FindControl("HyperlinkGridCol");
                if (ctrl != null)
                {
                    if (e.Item.DataItem != null)
                    {
                        DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                        string navigateURL = "InvoiceDetailAdd.aspx?" + "Action=edit&SerialNo=" + dr["SerialNo"].ToString();
                        ctrl.NavigateUrl = navigateURL;
                    }
                }
                ctrl = (HyperLink)e.Item.FindControl("HyperlinkDelete");
                if (ctrl != null)
                {
                    if (e.Item.DataItem != null)
                    {
                        DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                        string navigateURL = "InvoiceDetailAdd.aspx?" + "Action=delete&SerialNo=" + dr["SerialNo"].ToString();
                        ctrl.NavigateUrl = navigateURL;
                    }
                }
            }
        }

        private void DisableRequiredFieldControls()
        {
        }

        private void IsGMGCompany(int iCompanyID)
        {
            if (iCompanyID == 14 || objCompany.IsGMGCompany(iCompanyID))
                DisableRequiredFieldControls();
        }

        public void GetVatList()
        {
            Company oCompany = new Company();
            dtbl = oCompany.GetVatList();

            ddlCountryOfOrigin.DataSource = dtbl;
            ddlCountryOfOrigin.DataBind();

            ddlCountryOfOrigin.Items.Insert(0, new ListItem("Select", "0"));

            dtbl.Dispose();
        }

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

        private void WriteToFile(string sFileName, int sTag)
        {
            string sFiletoWrite = "D:/P2D/IPE Archive/New Look Retailers Ltd" + @"\" + DateTime.Now.ToString().Replace("/", "_") + "_Log.txt";
            StreamWriter oSWriter = null;
            oSWriter = new StreamWriter(sFiletoWrite, true);
            if (sTag == 1)
            {
                oSWriter.WriteLine("File Successfully processed - " + sFileName);
            }
            else
            {
                oSWriter.WriteLine("File not Successfully processed - " + sFileName);
            }
            oSWriter.Close();
        }

        void InsertNewLinetoDB(RecordSet rsInvoiceDetail)
        {
            CBSolutions.ETH.Web.Invoice invoice = new CBSolutions.ETH.Web.Invoice();
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            invoice.InsertInvoiceDetailData(Convert.ToInt32(Request.QueryString["invoiceID"]), rsInvoiceDetail, da);
        }

        void Click_Submit()
        {
            if (hdnType.Value == "1")
            {
                if (ddlItemType.SelectedItem.Text == "Product/Service")
                {
                    if (txtBuyersProdCode.Text.Trim() == "" || txtPurOrderNo.Text.Trim() == "")
                    {
                        lblBPCode.Visible = true;
                        return;
                    }
                    else
                    {
                        if (CheckBPcodePO() == false)
                        {
                            lblBPCode.Visible = true;
                            return;
                        }
                        else
                        {
                            lblBPCode.Visible = false;
                        }
                    }
                }

                if (ddlcolor.SelectedItem.Value != "-1")
                {
                    if (CheckColorCodeAgainstBuyerProdCode() == false)
                    {
                        lblBPCode.Text = "Color code does not match with Buyer's Product Code and PO Number";
                        lblBPCode.Visible = true;
                        return;
                    }
                    else
                    {
                        lblBPCode.Visible = false;
                    }
                }

            }

            if (CheckDate(System.Convert.ToInt32(cboYearPODate.SelectedValue),
                System.Convert.ToInt32(cboMonthPODate.SelectedValue),
                System.Convert.ToInt32(cboDayPODate.SelectedValue)) == false)
            {
                lblErrorPODate.Visible = true;
                return;
            }


            if (CheckDate(System.Convert.ToInt32(cboYearDeliveryDate.SelectedValue),
                System.Convert.ToInt32(cboMonthDeliveryDate.SelectedValue),
                System.Convert.ToInt32(cboDayDeliveryDate.SelectedValue)) == false)
            {
                lblErrorDeliveryDate.Visible = true;
                return;
            }
            RecordSet rs = null;
            if (System.IO.File.Exists(Session["XMLInvoiceDetailFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceDetailFile"].ToString());
                rs = new RecordSet(ds);
            }
            else
            {
                DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                rs = da.CreateInsertBuffer("InvoiceDetail");
            }
            if (Request.QueryString["SerialNo"] != null)
            {
                int srl = System.Convert.ToInt32(Request.QueryString["SerialNo"]);
                rs.Filter = "SerialNo = " + srl;
            }
            else
            {
                rs.MoveLast();
                rs.MovePrevious();
                // commented by Mainak, 2018-11-5
                //int srl = 0;
                //if (rs["SerialNo"] != DBNull.Value)
                //{
                //    srl = System.Convert.ToInt32(rs["SerialNo"]);
                    rs.AddNew();
                    // commented by Mainak, 2018-11-5
                //}
                rs["SerialNo"] = System.Convert.ToInt32(lblSerialNo.Text);
            }
            rs["InvoiceDetailID"] = "0";
            rs["InvoiceID"] = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            rs["ModDate"] = DateTime.Now;
            //rs["ModStamp"] = DBNull.Value;
            rs["New_TaxCode"] = "S";
            rs["Description"] = txtDescription.Text;
            decimal quantity = txtQuantity.Text == "" ? 0 : System.Convert.ToDecimal(txtQuantity.Text);
            rs["Quantity"] = quantity;
            decimal rate = txtRate.Text == "" ? 0 : System.Convert.ToDecimal(txtRate.Text);
            rs["Rate"] = rate;
            decimal dNew_ExtendedValue = 0;
            dNew_ExtendedValue = GetAfterDecimalCalculatedValue(Convert.ToDecimal(quantity * rate));

            rs["GrossAmt"] = dNew_ExtendedValue;

            decimal discountPercent = txtDiscountPercent.Text == "" ? 0 : System.Convert.ToDecimal(txtDiscountPercent.Text);
            rs["Discount"] = discountPercent;
            decimal dSecondDiscountPercent = txtSecondDiscountPercent.Text == "" ? 0 : System.Convert.ToDecimal(txtSecondDiscountPercent.Text);
            rs["New_DiscountPercent2"] = GetAfterDecimalCalculatedValue(dSecondDiscountPercent);	// amitava 241007;


            decimal dNew_DiscountValue1 = 0;

            if (discountPercent == 0)
                dNew_DiscountValue1 = 0;
            else
                dNew_DiscountValue1 = GetAfterDecimalCalculatedValue(System.Convert.ToDecimal(System.Convert.ToDecimal(rs["GrossAmt"]) * (discountPercent / 100))); //amitava 241007;

            if (dNew_DiscountValue1 != 0)
                rs["New_DiscountValue1"] = GetAfterDecimalCalculatedValue(dNew_DiscountValue1);
            else
                rs["New_DiscountValue1"] = DBNull.Value;

            decimal dNew_DiscountValue2 = 0;

            if (dSecondDiscountPercent == 0)
                dNew_DiscountValue2 = 0;
            else
                dNew_DiscountValue2 = GetAfterDecimalCalculatedValue((dNew_ExtendedValue - dNew_DiscountValue1) * (dSecondDiscountPercent / 100));  //amitava 241007;

            if (dNew_DiscountValue2 != 0)
                rs["New_DiscountValue2"] = dNew_DiscountValue2;
            else
                rs["New_DiscountValue2"] = DBNull.Value;

            decimal dNew_PostDiscountValue = 0;
            dNew_PostDiscountValue = dNew_ExtendedValue - dNew_DiscountValue1 - dNew_DiscountValue2;
            rs["New_PostDiscountValue"] = dNew_PostDiscountValue;

            string[] s = cboVATRate.SelectedItem.Value.Split('*');
            rs["VATTypeID"] = s[0];
            decimal VATRate = System.Convert.ToDecimal(s[1]);
            rs["VATRate"] = GetAfterDecimalCalculatedValue(VATRate);
            rs["UOM"] = txtUOM.Text;

            if (txtPurOrderNo.Text.Trim() != "")
                rs["PurOrderNo"] = txtPurOrderNo.Text;
            else
                rs["PurOrderNo"] = DBNull.Value;

            if (txtPurOrderLineNo.Text.Trim() != "")
                rs["PurOrderLineNo"] = txtPurOrderLineNo.Text;
            else
                rs["PurOrderLineNo"] = DBNull.Value;

            if (cboYearPODate.SelectedValue == "0" || cboMonthPODate.SelectedValue == "0" || cboDayPODate.SelectedValue == "0")
            {
                rs["PurOrderDate"] = DBNull.Value;
            }
            else
            {
                rs["PurOrderDate"] = cboYearPODate.SelectedValue + "/" + cboMonthPODate.SelectedValue + "/" + cboDayPODate.SelectedValue;
            }

            if (txtBuyersProdCode.Text.Trim() != "")
                rs["BuyersProdCode"] = txtBuyersProdCode.Text;
            else
                rs["BuyersProdCode"] = DBNull.Value;

            if (txtSupplierProdCode.Text.Trim() != "")
                rs["SuppliersProdCode"] = txtSupplierProdCode.Text;
            else
                rs["SuppliersProdCode"] = DBNull.Value;

            rs["New_Type"] = "";

            if (ddlcolor.SelectedItem.Value != "-1")
                rs["New_Definable1"] = ddlcolor.SelectedValue.ToString();
            else
                rs["New_Definable1"] = DBNull.Value;


            if (ddlModeOfTransport.SelectedValue.Trim() != "0")
                rs["New_ModeOfTransport"] = Convert.ToInt32(ddlModeOfTransport.SelectedValue.Trim());
            else
                rs["New_ModeOfTransport"] = DBNull.Value;

            if (ddlNatureOfTransaction.SelectedValue.Trim() != "0")
                rs["New_NatureOfTransaction"] = Convert.ToInt32(ddlNatureOfTransaction.SelectedValue.Trim());
            else
                rs["New_NatureOfTransaction"] = DBNull.Value;

            if (ddlDeliveryTerms.SelectedValue.Trim() != "0")
                rs["New_TermsOfDelivery"] = ddlDeliveryTerms.SelectedValue.Trim();
            else
                rs["New_TermsOfDelivery"] = DBNull.Value;

            if (ddlCountryOfOrigin.SelectedValue.Trim() != "0")
                rs["New_CountryOfOrigin"] = ddlCountryOfOrigin.SelectedValue.Trim();
            else
                rs["New_CountryOfOrigin"] = DBNull.Value;

            if (tbCommodityCode.Text.Trim() != "")
                rs["New_CommodityCode"] = Convert.ToInt32(tbCommodityCode.Text);
            else
                rs["New_CommodityCode"] = DBNull.Value;

            if (tbSupplementaryUnits.Text.Trim() != "")
                rs["New_SupplementaryUnits"] = tbSupplementaryUnits.Text.Trim();
            else
                rs["New_SupplementaryUnits"] = DBNull.Value;

            if (tbNetMassInKilos.Text.Trim() != "")
                rs["New_NettMass"] = Convert.ToDecimal(tbNetMassInKilos.Text);
            else
                rs["New_NettMass"] = DBNull.Value;

            if (tbDespatchNoteNo.Text.Trim() != "")
                rs["New_DespatchNoteNumber"] = tbDespatchNoteNo.Text.Trim();
            else
                rs["New_DespatchNoteNumber"] = DBNull.Value;

            if (cboYearDeliveryDate.SelectedValue != "0" && cboMonthDeliveryDate.SelectedValue != "0" && cboDayDeliveryDate.SelectedValue != "0")
                rs["New_DespatchDate"] = cboYearDeliveryDate.SelectedValue + "/" + cboMonthDeliveryDate.SelectedValue + "/" + cboDayDeliveryDate.SelectedValue;
            else
                rs["New_DespatchDate"] = DBNull.Value;

            if (System.IO.File.Exists(Session["XMLInvoiceHeadFile"].ToString()))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                ds.ReadXml(Session["XMLInvoiceHeadFile"].ToString());
                RecordSet rsHead = new RecordSet(ds);


                decimal dNew_OverallDiscountValue = 0;
                if (rsHead["New_OverallDiscountPercent"] != DBNull.Value)
                    dNew_OverallDiscountValue = (dNew_PostDiscountValue * System.Convert.ToDecimal(rsHead["New_OverallDiscountPercent"])) / 100;
                else
                    dNew_OverallDiscountValue = 0;

                if (dNew_OverallDiscountValue != 0)
                    rs["New_OverallDiscountValue"] = dNew_OverallDiscountValue;
                else
                    rs["New_OverallDiscountValue"] = DBNull.Value;

                decimal dNew_NettValue = 0;
                dNew_NettValue = dNew_PostDiscountValue - dNew_OverallDiscountValue;
                if (dNew_NettValue != 0)
                    rs["New_NettValue"] = GetAfterDecimalCalculatedValue(dNew_NettValue);
                else
                    rs["New_NettValue"] = DBNull.Value;

                decimal dVatAmt = 0;
                if (VATRate != 0)
                {
                    if (rsHead["DiscountPercent"] != DBNull.Value)
                        dVatAmt = GetAfterDecimalCalculatedValue(dNew_NettValue * ((100 - System.Convert.ToDecimal(rsHead["DiscountPercent"])) / 100) * (VATRate / 100)); //Amitava
                    else
                        dVatAmt = GetAfterDecimalCalculatedValue(dNew_NettValue * (VATRate / 100));
                }
                else
                    dVatAmt = 0;

                rs["VATAmt"] = dVatAmt;

                rs["TotalAmt"] = GetAfterDecimalCalculatedValue(dNew_NettValue + dVatAmt);


                decimal totalAmt = 0;
                decimal VATAmt = 0;
                decimal dOverallDiscountAmount = 0;
                decimal dNetValue = 0;

                rs.MoveFirst();


                DataSet dsInvoicedetail = new DataSet();
                dsInvoicedetail = rs.ParentDataSet;
                dsInvoicedetail.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
                dsInvoicedetail.WriteXml(Session["XMLInvoiceDetailFile"].ToString());

                totalAmt = 0;
                VATAmt = 0;
                dOverallDiscountAmount = 0;
                dNetValue = 0;
                for (int iCounter = 0; iCounter < dsInvoicedetail.Tables[0].Rows.Count; iCounter++)
                {
                    if (dsInvoicedetail.Tables[0].Rows[iCounter]["TotalAmt"] != DBNull.Value)
                        totalAmt = GetAfterDecimalCalculatedValue(totalAmt) + GetAfterDecimalCalculatedValue(Convert.ToDecimal(dsInvoicedetail.Tables[0].Rows[iCounter]["TotalAmt"]));
                    else
                        totalAmt = totalAmt + 0;

                    if (dsInvoicedetail.Tables[0].Rows[iCounter]["VATAmt"] != DBNull.Value)
                        VATAmt = GetAfterDecimalCalculatedValue(VATAmt + Convert.ToDecimal(dsInvoicedetail.Tables[0].Rows[iCounter]["VATAmt"]));
                    else
                        VATAmt = (VATAmt + 0);

                    if (dsInvoicedetail.Tables[0].Rows[iCounter]["New_NettValue"] != DBNull.Value)
                        dNetValue = GetAfterDecimalCalculatedValue(dNetValue + Convert.ToDecimal(dsInvoicedetail.Tables[0].Rows[iCounter]["New_NettValue"]));
                    else
                        dNetValue = (dNetValue + 0);

                    if (dsInvoicedetail.Tables[0].Rows[iCounter]["New_OverallDiscountValue"] != DBNull.Value)
                        dOverallDiscountAmount = GetAfterDecimalCalculatedValue(dOverallDiscountAmount + Convert.ToDecimal(dsInvoicedetail.Tables[0].Rows[iCounter]["New_OverallDiscountValue"]));
                    else
                        dOverallDiscountAmount = (dOverallDiscountAmount + 0);
                }

                rsHead["NetTotal"] = dNetValue;//This is NetAmount in InvoiceHeader
                rsHead["VATAmt"] = VATAmt;	 //This is TaxAmount in InvoiceHeader
                rsHead["TotalAmt"] = totalAmt; //This is GrossAmount in InvoiceHeader
                rsHead["New_OveralldiscountAmount"] = dOverallDiscountAmount;


                if (rsHead["DiscountPercent"] != DBNull.Value)
                    rsHead["New_SettlementAmount1"] = (System.Convert.ToDecimal(rsHead["DiscountPercent"]) / 100) * dNetValue;
                else
                    rsHead["New_SettlementAmount1"] = DBNull.Value;

                if (rsHead["New_SettlementPercent2"] != DBNull.Value)
                    rsHead["New_SettlementAmount2"] = (System.Convert.ToDecimal(rsHead["New_SettlementPercent2"]) / 100) * dNetValue;
                else
                    rsHead["New_SettlementAmount2"] = DBNull.Value;

                DataSet dsInvoiceHead = new DataSet();
                dsInvoiceHead = rsHead.ParentDataSet;
                dsInvoiceHead.WriteXmlSchema(Session["XSDInvoiceHeadFile"].ToString());
                dsInvoiceHead.WriteXml(Session["XMLInvoiceHeadFile"].ToString());

            }
            rs.MoveFirst();
            DataSet dsInvoicedetailNEW = new DataSet();
            dsInvoicedetailNEW = rs.ParentDataSet;
            dsInvoicedetailNEW.WriteXmlSchema(Session["XSDInvoiceDetailFile"].ToString());
            dsInvoicedetailNEW.WriteXml(Session["XMLInvoiceDetailFile"].ToString());
            string strUrl = "AllowEdit=" + Request.QueryString["AllowEdit"].ToString() + "&";
            strUrl = strUrl.Replace("&", "");
            strUrl += "&#ss";
            Response.Redirect("InvoiceEdit.aspx?InvoiceID=" + Request.QueryString["invoiceID"] + "&" + strUrl);

        }

    }
}