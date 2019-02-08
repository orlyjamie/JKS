using System;
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
    /// Summary description for BranchEditNL.
    /// </summary>
    public class BranchEditNL : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Label lblHeader;
        protected System.Web.UI.WebControls.TextBox txtName;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
        protected System.Web.UI.WebControls.TextBox txtBranchCode;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator4;
        protected System.Web.UI.WebControls.Label lblErrorDuplicateBranchCode;
        protected System.Web.UI.WebControls.TextBox txtAddress1;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
        protected System.Web.UI.WebControls.TextBox txtAddress2;
        protected System.Web.UI.WebControls.TextBox txtAddress3;
        protected System.Web.UI.WebControls.TextBox txtAddress4;
        protected System.Web.UI.WebControls.TextBox txtAddress5;
        protected System.Web.UI.WebControls.DropDownList cboCounty;
        protected System.Web.UI.WebControls.TextBox txtCounty;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_FOR_County;
        protected System.Web.UI.WebControls.DropDownList cboCountry;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_FOR_Country;
        protected System.Web.UI.WebControls.TextBox txtPostCode;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator3;
        protected System.Web.UI.WebControls.CheckBox chkInvoice;
        protected System.Web.UI.WebControls.CheckBox chkDelivery;
        protected System.Web.UI.WebControls.TextBox txtTelephone;
        protected System.Web.UI.WebControls.TextBox txtFax;
        protected System.Web.UI.WebControls.TextBox txtPContact;
        protected System.Web.UI.WebControls.TextBox txtPEmail;
        protected System.Web.UI.WebControls.TextBox txtSalesVolume;
        protected System.Web.UI.WebControls.TextBox txtPurchaseVolume;
        protected System.Web.UI.WebControls.TextBox txtSupplierCount;
        protected System.Web.UI.WebControls.TextBox txtCustomerCount;
        protected System.Web.UI.WebControls.TextBox txtTurnover;
        protected System.Web.UI.WebControls.TextBox txtWebsite;
        protected System.Web.UI.WebControls.Button btnSubmit;

        #endregion

        #region User Defined Variables
        private Invoice.Invoice objInvoice = new Invoice.Invoice();
        private int branchID = 0;
        protected string strCompanyTypeID = "";
        #endregion

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            Session["SelectedPage"] = "BranchManagement";

            if (Session["CompanyTypeID"].ToString().Trim().Equals("2"))
                strCompanyTypeID = "2";

            if (Request.QueryString["BranchID"] != null)
            {
                branchID = System.Convert.ToInt32(Request.QueryString["BranchID"]);
                ViewState["BranchID"] = Request.QueryString["BranchID"].Trim();
            }

            if (Request["CID"] != null)
                ViewState["CID"] = Request["CID"].Trim();

            if (!IsPostBack)
            {

                LoadData();
                if (Request.QueryString["BranchID"] != null)
                {
                    PopulateData();
                }
            }
            if (branchID == 0)
                lblHeader.Text = "Add a new Branch";
            else
                lblHeader.Text = "Edit Branch";

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
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            RecordSet rs = BranchNL.GetCountyList();
            while (!rs.EOF())
            {
                ListItem listItem = new ListItem(rs["County"].ToString(), rs["CountyID"].ToString());
                cboCounty.Items.Add(listItem);
                rs.MoveNext();
            }
            rs = BranchNL.GetCountryList();
            while (!rs.EOF())
            {
                ListItem listItem = new ListItem(rs["Country"].ToString(), rs["CountryID"].ToString());
                cboCountry.Items.Add(listItem);
                rs.MoveNext();
            }
            cboCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            cboCounty.Items.Insert(0, new ListItem("Select County", "0"));
        }
        #endregion
        #region PopulateData
        private void PopulateData()
        {
            branchID = System.Convert.ToInt32(Request.QueryString["BranchID"]);
            RecordSet rs = BranchNL.GetBranchData(branchID);

            if (rs["Branch"] != DBNull.Value)
                txtName.Text = rs["Branch"].ToString();

            if (rs["BranchCode"] != DBNull.Value)
                txtBranchCode.Text = rs["BranchCode"].ToString();

            txtAddress1.Text = rs["Address1"].ToString();
            txtAddress2.Text = rs["Address2"].ToString();
            txtAddress3.Text = rs["Address3"].ToString();
            txtAddress4.Text = rs["Address4"].ToString();
            try
            {
                if (rs["CountryID"] != DBNull.Value)
                    cboCountry.SelectedValue = System.Convert.ToString(rs["CountryID"]);

            }
            catch { }
            try
            {
                if (rs["CountyID"] != DBNull.Value)
                    cboCounty.SelectedValue = System.Convert.ToString(rs["CountyID"]);

            }
            catch { }
            txtPostCode.Text = rs["PostCode"].ToString();
            txtTelephone.Text = rs["Telephone"].ToString();
            txtFax.Text = rs["Fax"].ToString();

            txtAddress5.Text = rs["Address5"].ToString();
            txtPContact.Text = rs["PContact"].ToString();
            txtPEmail.Text = rs["PEmail"].ToString();
            if (rs["IsInvoiceLocation"] != DBNull.Value)
                chkInvoice.Checked = (bool)rs["IsInvoiceLocation"];

            if (rs["IsDeliveryLocation"] != DBNull.Value)
                chkDelivery.Checked = (bool)rs["IsDeliveryLocation"];
            txtPurchaseVolume.Text = rs["PurchaseInvoiceVolume"].ToString();
            txtSupplierCount.Text = rs["ActiveSupplierCount"].ToString();
            txtCustomerCount.Text = rs["ActiveCustomerCount"].ToString();
            txtSalesVolume.Text = rs["SalesInvoiceVolume"].ToString();
            txtTurnover.Text = rs["ApproxTurnover"].ToString();
            txtWebsite.Text = rs["Website"].ToString();
        }
        #endregion
        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            BranchNL branch = new BranchNL();
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);

            if (ViewState["BranchID"] == null)
            {
                if (objInvoice.CheckDuplicateBranchCode(Convert.ToInt32(ViewState["BranchID"]), txtBranchCode.Text.Trim(), Convert.ToInt32(Session["CompanyID"])))
                {
                    lblErrorDuplicateBranchCode.Visible = true;
                    return;
                }
            }

            RecordSet rs = null;
            if (branchID == 0)
                rs = da.CreateInsertBuffer("Branch");
            else
            {
                rs = BranchNL.GetBranchData(branchID);
            }

            if (ViewState["CID"] != null)
                rs["CompanyID"] = ViewState["CID"].ToString().Trim();
            else
                rs["CompanyID"] = Session["CompanyID"].ToString();

            rs["Branch"] = txtName.Text;
            rs["BranchCode"] = txtBranchCode.Text.Trim();
            rs["Address1"] = txtAddress1.Text;
            rs["Address2"] = txtAddress2.Text;
            rs["Address3"] = txtAddress3.Text;
            rs["Address4"] = txtAddress4.Text;
            rs["Address5"] = txtAddress5.Text;
            rs["PostCode"] = txtPostCode.Text;
            rs["Telephone"] = txtTelephone.Text;
            rs["Fax"] = txtFax.Text;

            if (cboCountry.SelectedIndex != 0)
                rs["CountryID"] = cboCountry.SelectedValue;
            else
                rs["CountryID"] = DBNull.Value;

            if (cboCounty.SelectedIndex != 0)
                rs["CountyID"] = cboCounty.SelectedValue;
            else
                rs["CountyID"] = DBNull.Value;

            rs["IsInvoiceLocation"] = chkInvoice.Checked;
            rs["IsDeliveryLocation"] = chkDelivery.Checked;

            rs["CurrencyTypeID"] = DBNull.Value;
            rs["PContact"] = txtPContact.Text;
            rs["PEmail"] = txtPEmail.Text;
            rs["PurchaseInvoiceVolume"] = txtPurchaseVolume.Text == "" ? 0 : System.Convert.ToDecimal(txtPurchaseVolume.Text);
            rs["ActiveSupplierCount"] = txtSupplierCount.Text == "" ? 0 : System.Convert.ToInt32(txtSupplierCount.Text);
            rs["ActiveCustomerCount"] = txtCustomerCount.Text == "" ? 0 : System.Convert.ToInt32(txtCustomerCount.Text);
            rs["SalesInvoiceVolume"] = txtSalesVolume.Text == "" ? 0 : System.Convert.ToDecimal(txtSalesVolume.Text);
            rs["ApproxTurnover"] = txtTurnover.Text == "" ? 0 : System.Convert.ToDecimal(txtTurnover.Text);
            rs["Website"] = txtWebsite.Text;
            rs["ModUserId"] = Session["UserID"].ToString();

            if (branchID == 0)
            {
                if (branch.InsertBranchData(rs) == 0)
                    Response.Write(branch.ErrorMessage);
                else
                    Response.Redirect("BranchBrowseNL.aspx");
            }
            else
            {
                if (!branch.UpdateBranchData(rs))
                    Response.Write(branch.ErrorMessage);
                else
                    Response.Redirect("BranchBrowseNL.aspx");
            }
        }
        #endregion

    }
}
