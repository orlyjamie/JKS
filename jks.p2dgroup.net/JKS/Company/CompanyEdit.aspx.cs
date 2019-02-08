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
    /// Summary description for CompanyEdit.
    /// </summary>
    public class CompanyEdit : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.TextBox txtCompanyName;
        protected System.Web.UI.WebControls.RequiredFieldValidator rvCompanyName;
        protected System.Web.UI.WebControls.TextBox txtCompanyCode;
        protected System.Web.UI.WebControls.DropDownList cboCompanyType;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.DropDownList cboMemberType;
        protected System.Web.UI.WebControls.Label lblHeader;
        protected System.Web.UI.WebControls.TextBox txtVAT;
        protected System.Web.UI.WebControls.RequiredFieldValidator rvVAT;
        protected System.Web.UI.WebControls.TextBox txtEmail;
        protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator_FOR_Email;
        protected System.Web.UI.WebControls.HyperLink Hyperlink2;

        protected System.Web.UI.WebControls.TextBox tbAddress1;
        protected System.Web.UI.WebControls.TextBox tbAddress2;
        protected System.Web.UI.WebControls.TextBox tbAddress3;
        protected System.Web.UI.WebControls.DropDownList ddlCounty;
        protected System.Web.UI.WebControls.DropDownList ddlCountry;
        protected System.Web.UI.WebControls.RequiredFieldValidator RFV_FOR_Country;
        protected System.Web.UI.WebControls.RequiredFieldValidator RFV_FOR_County;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_FOR_Address1;
        protected System.Web.UI.WebControls.TextBox tbPostCode;
        protected System.Web.UI.WebControls.TextBox tbPhoneNo;
        #endregion
        #region User Defined Variables
        protected int companyID = 0;
        protected int iSubCompanyID = 0;
        protected int iParentCompanyID = 0;
        private int companyPKID = 0;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.DropDownList cboVat;
        protected System.Web.UI.WebControls.TextBox txtVatNo;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_VATNo;
        protected System.Web.UI.WebControls.TextBox tbDeliveryAddress1;
        protected System.Web.UI.WebControls.TextBox tbDeliveryAddress2;
        protected System.Web.UI.WebControls.TextBox tbDeliveryAddress3;
        protected System.Web.UI.WebControls.DropDownList ddlCountryDelivery;
        protected System.Web.UI.WebControls.DropDownList ddlCountyDelivery;
        protected System.Web.UI.WebControls.TextBox tbDeliveryPostCode;
        protected System.Web.UI.WebControls.TextBox tbDeliveryPhoneNo;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_FOR_DeliveryAddress1;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_FOR_DeliveryCounty;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_FOR_DeliveryCountry;
        protected System.Web.UI.WebControls.DropDownList ddlCountryTaxNo;
        protected System.Web.UI.WebControls.TextBox txtTradersReference;
        private Company objCompany = new Company();
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            Session["SelectedPage"] = "CompanyBrowse";

            if (Request["PID"] != null)
                iParentCompanyID = Convert.ToInt32(Request["PID"]);

            if (Request["CompanyID"] != null)
            {
                iSubCompanyID = Convert.ToInt32(Request["CompanyID"]);
                ViewState["SubCompanyID"] = iSubCompanyID.ToString();
            }

            if (Request.QueryString["CompanyID"] != null)
                companyID = System.Convert.ToInt32(Request.QueryString["CompanyID"]);

            if (!IsPostBack)
            {
                PopulateCountryDropDown();
                LoadData();

                if (Request.QueryString["CompanyID"] != null)
                {
                    PopulateData();
                }

                if (companyID == 0)
                {
                    ViewState["Mode"] = "ADD";
                    lblHeader.Text = "Add a new Company";
                }
                else
                {
                    ViewState["Mode"] = "EDIT";
                    lblHeader.Text = "Edit Company";
                }

                if (Session["NewLook"].ToString() == "1" && (ViewState["Mode"].ToString().Equals("ADD") || ViewState["Mode"].ToString().Equals("EDIT")))
                {
                    EnableRequiredFieldValidatorsForNewLook(true);
                }
            }
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
            RecordSet rs = Company.GetCompanyTypeList();

            while (!rs.EOF())
            {
                ListItem listItem = new ListItem(rs["CompanyType"].ToString(), rs["CompanyTypeID"].ToString());
                cboCompanyType.Items.Add(listItem);
                rs.MoveNext();
            }

            rs = Company.GetMemberTypeList();

            while (!rs.EOF())
            {
                if ((int)rs["MemberTypeID"] != 1)
                {
                    ListItem listItem = new ListItem(rs["MemberType"].ToString(), rs["MemberTypeID"].ToString());
                    cboMemberType.Items.Add(listItem);
                }
                rs.MoveNext();
            }
            rs = null;
        }
        #endregion
        #region PopulateData
        private void PopulateData()
        {
            objCompany = new Company();

            RecordSet rs = Company.GetCompanyData(companyID);

            txtCompanyCode.Text = rs["CompanyCode"].ToString();
            txtCompanyName.Text = rs["CompanyName"].ToString();

            cboCompanyType.SelectedValue = rs["CompanyTypeID"].ToString();
            cboMemberType.SelectedValue = rs["MemberTypeID"].ToString();
            txtEmail.Text = rs["EmailID"].ToString();

            try
            {
                string[] strVatArray = rs["VATRegNo"].ToString().Split('-');
                txtVAT.Text = strVatArray[0].Trim();
                txtVatNo.Text = strVatArray[1].Trim();

                cboVat.SelectedValue = strVatArray[0].Trim();

            }
            catch { rs = null; }
            rs = null;
        }
        #endregion
        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {

                if (CheckDuplicateCompanyName(Convert.ToInt32(Request["CompanyID"]), txtCompanyName.Text.Trim()) == -101)
                {
                    lblMessage.Text = "Sorry, company name already exist.";
                    return;
                }

                Company company = new Company();
                DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);

                RecordSet rs = null;

                string strPasswordGUID = System.Guid.NewGuid().ToString().Substring(0, 8).Trim();

                if (companyID == 0)
                {
                    string strNetworkGUID = System.Guid.NewGuid().ToString().Substring(0, 11).Trim();

                    rs = da.CreateInsertBuffer("Company");

                    rs["CreateDate"] = DateTime.Now;
                    rs["NetworkID"] = strNetworkGUID;
                }
                else
                {
                    rs = Company.GetCompanyData(companyID);
                }

                if (cboCompanyType.SelectedValue.Trim().Equals("2"))
                    rs["CompanyCode"] = DBNull.Value;
                else
                    rs["CompanyCode"] = txtCompanyCode.Text;

                rs["CompanyName"] = txtCompanyName.Text;
                rs["VATRegNo"] = txtVAT.Text.Trim() + "-" + txtVatNo.Text.Trim();

                #region New_TaxCountryNo MODIFIED BY TARAKESHWAR DATE 26-JAN-2006 (11:42 AM)
                if (ddlCountryTaxNo.SelectedValue.Trim().Equals("Select"))
                    rs["New_TaxCountryNo"] = DBNull.Value;
                else
                    rs["New_TaxCountryNo"] = ddlCountryTaxNo.SelectedValue;
                #endregion

                #region New_TradersReference MODIFIED BY TARAKESHWAR DATE 26-JAN-2006 (2:42 PM)
                if (txtTradersReference.Text.Trim() != "")
                    rs["New_TradersReference"] = txtTradersReference.Text.Trim();
                else
                    rs["New_TradersReference"] = DBNull.Value;
                #endregion

                rs["CompanyTypeID"] = cboCompanyType.SelectedItem.Value;
                rs["MemberTypeID"] = cboMemberType.SelectedItem.Value;
                rs["ModUserId"] = Session["UserID"].ToString();
                rs["EmailID"] = txtEmail.Text.Trim();

                if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                    rs["ParentCompanyID"] = DBNull.Value;
                else
                    rs["ParentCompanyID"] = iParentCompanyID;

                if (Session["NewLook"].ToString() == "1")
                {
                    if (ViewState["Mode"].ToString() == "ADD")
                    {
                        if (tbAddress1.Text.Trim() != "")
                            rs["Address1"] = tbAddress1.Text.Trim();
                        else
                            rs["Address1"] = DBNull.Value;

                        if (tbAddress2.Text.Trim() != "")
                            rs["Address2"] = tbAddress2.Text.Trim();
                        else
                            rs["Address2"] = DBNull.Value;

                        if (tbAddress3.Text.Trim() != "")
                            rs["Address3"] = tbAddress3.Text.Trim();
                        else
                            rs["Address3"] = DBNull.Value;

                        if (ddlCounty.SelectedIndex != 0)
                            rs["CountyID"] = ddlCounty.SelectedItem.Text.Trim();
                        else
                            rs["CountyID"] = DBNull.Value;

                        if (ddlCountry.SelectedIndex != 0)
                            rs["CountryID"] = ddlCountry.SelectedItem.Text.Trim();
                        else
                            rs["CountryID"] = DBNull.Value;

                        if (tbPostCode.Text.Trim() != "")
                            rs["PostCode"] = tbPostCode.Text.Trim();
                        else
                            rs["PostCode"] = DBNull.Value;

                        if (tbPhoneNo.Text.Trim() != "")
                            rs["PhoneNumber1"] = tbPhoneNo.Text.Trim();
                        else
                            rs["PhoneNumber1"] = DBNull.Value;

                        rs["ModDate"] = DateTime.Now;
                    }
                }
                else
                {
                    rs["Address1"] = DBNull.Value;
                    rs["Address2"] = DBNull.Value;
                    rs["Address3"] = DBNull.Value;
                    rs["CountyID"] = DBNull.Value;
                    rs["CountryID"] = DBNull.Value;
                    rs["PostCode"] = DBNull.Value;
                    rs["PhoneNumber1"] = DBNull.Value;
                    rs["ModDate"] = DateTime.Now;
                }

                if (companyID == 0)
                {

                    BranchNL branch = new BranchNL();
                    RecordSet rsBranch = null;

                    if (Session["NewLook"].ToString() != "1")
                    {
                        rsBranch = da.CreateInsertBuffer("Branch");

                        rsBranch["Branch"] = txtCompanyName.Text;
                        rsBranch["ModUserId"] = Session["UserID"].ToString();
                    }


                    da.BeginTransaction();
                    companyPKID = company.InsertCompanyData(rs, da);

                    if (companyPKID > 0)
                    {
                        if (Session["NewLook"].ToString() != "1")
                        {
                            rsBranch["CompanyID"] = companyPKID;
                            branch.InsertBranchData(rsBranch, da);
                        }


                    }
                    if (da.ErrorCode != DataAccessErrors.Successful)
                    {
                        da.RollbackTransaction();
                        Response.Write(da.ErrorMessage);
                    }
                    else
                    {
                        da.CommitTransaction();
                        lblMessage.Text = "Record(s) saved successfully.";
                    }
                }
                else
                {
                    if (!company.UpdateCompanyData(rs))
                    {
                        Response.Write(company.ErrorMessage);
                    }
                    else
                    {
                        companyPKID = companyID;
                        lblMessage.Text = "Record(s) saved successfully.";
                    }
                }

                if (Session["NewLook"].ToString() == "1" && ViewState["Mode"].ToString() == "ADD")
                {
                    if (companyPKID > 0)
                    {
                        bool bBranchInsertFlag = false;

                        if (objCompany.SaveBranchForNewLook(companyPKID, txtCompanyName.Text.Trim(), txtCompanyCode.Text.Trim(),
                            tbAddress1.Text.Trim(), tbAddress2.Text.Trim(), tbAddress3.Text.Trim(),
                            tbPostCode.Text.Trim(), tbPhoneNo.Text.Trim(),
                            Convert.ToInt32(ddlCounty.SelectedValue), Convert.ToInt32(ddlCountry.SelectedValue),
                            txtEmail.Text.Trim(), Convert.ToInt32(Session["UserID"]), "I"))
                        {
                            bBranchInsertFlag = true;
                        }
                        else
                        {
                            bBranchInsertFlag = false;
                        }

                        if (objCompany.SaveBranchForNewLook(companyPKID, txtCompanyName.Text.Trim(), txtCompanyCode.Text.Trim(),
                            tbDeliveryAddress1.Text.Trim(), tbDeliveryAddress2.Text.Trim(), tbDeliveryAddress3.Text.Trim(),
                            tbDeliveryPostCode.Text.Trim(), tbDeliveryPhoneNo.Text.Trim(),
                            Convert.ToInt32(ddlCountyDelivery.SelectedValue), Convert.ToInt32(ddlCountryDelivery.SelectedValue),
                            txtEmail.Text.Trim(), Convert.ToInt32(Session["UserID"]), "D"))
                        {
                            bBranchInsertFlag = true;
                        }
                        else
                        {
                            bBranchInsertFlag = false;
                        }

                        if (bBranchInsertFlag)
                        {
                            lblMessage.Text = "Record(s) saved successfully.";
                        }
                        else
                        {
                            lblMessage.Text = "Error saving record(s).";
                        }
                    }
                }
                else if (Session["NewLook"].ToString() == "1" && ViewState["Mode"].ToString().Equals("EDIT"))
                {
                    bool bBranchInsertFlag = false;

                    if (objCompany.UpdateDeliveryAddressBranchForNewLook(Convert.ToInt32(ViewState["SubCompanyID"]), txtCompanyName.Text.Trim(), txtCompanyCode.Text.Trim(),
                        tbDeliveryAddress1.Text.Trim(), tbDeliveryAddress2.Text.Trim(), tbDeliveryAddress3.Text.Trim(),
                        tbDeliveryPostCode.Text.Trim(), tbDeliveryPhoneNo.Text.Trim(),
                        Convert.ToInt32(ddlCountyDelivery.SelectedValue), Convert.ToInt32(ddlCountryDelivery.SelectedValue),
                        txtEmail.Text.Trim(), Convert.ToInt32(Session["UserID"]), "D"))
                    {
                        bBranchInsertFlag = true;
                    }
                    else
                    {
                        bBranchInsertFlag = false;
                    }

                    if (objCompany.UpdateDeliveryAddressBranchForNewLook(Convert.ToInt32(ViewState["SubCompanyID"]), txtCompanyName.Text.Trim(), txtCompanyCode.Text.Trim(),
                        tbAddress1.Text.Trim(), tbAddress2.Text.Trim(), tbAddress3.Text.Trim(),
                        tbPostCode.Text.Trim(), tbPhoneNo.Text.Trim(),
                        Convert.ToInt32(ddlCounty.SelectedValue), Convert.ToInt32(ddlCountry.SelectedValue),
                        txtEmail.Text.Trim(), Convert.ToInt32(Session["UserID"]), "I"))
                    {
                        bBranchInsertFlag = true;
                    }
                    else
                    {
                        bBranchInsertFlag = false;
                    }

                    if (bBranchInsertFlag)
                    {
                        lblMessage.Text = "Record(s) saved successfully.";
                    }
                    else
                    {
                        lblMessage.Text = "Error saving record(s).";
                    }
                }
            }
        }
        #endregion
        #region RollDice
        public int RollDice()
        {
            int i = 0;

            Random r = new Random();

            i = r.Next(999999);

            return i;
        }
        #endregion
        #region PopulateCountryDropDown
        private void PopulateCountryDropDown()
        {
            ddlCountry.DataSource = objCompany.GetCountryList();
            ddlCountry.DataBind();

            ddlCounty.DataSource = objCompany.GetCountyList();
            ddlCounty.DataBind();

            cboVat.DataSource = objCompany.GetVatList();
            cboVat.DataBind();

            ddlCountryDelivery.DataSource = objCompany.GetCountryList();
            ddlCountryDelivery.DataBind();

            ddlCountyDelivery.DataSource = objCompany.GetCountyList();
            ddlCountyDelivery.DataBind();

            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            ddlCounty.Items.Insert(0, new ListItem("Select County", "0"));
            ddlCountryDelivery.Items.Insert(0, new ListItem("Select Country", "0"));
            ddlCountyDelivery.Items.Insert(0, new ListItem("Select County", "0"));
            cboVat.Items.Insert(0, new ListItem("Select Vat", "0"));
        }
        #endregion
        #region EnableRequiredFieldValidatorsForNewLook
        private void EnableRequiredFieldValidatorsForNewLook(bool bFlag)
        {

            rfv_FOR_DeliveryAddress1.Enabled = bFlag;
            rfv_FOR_DeliveryCountry.Enabled = bFlag;
            rfv_FOR_DeliveryCounty.Enabled = bFlag;

            if (!ViewState["Mode"].ToString().Equals("ADD"))
            {
                GetInvoiceAndDeliveryAddressForCompanyToEdit();
            }

        }
        #endregion

        #region GetInvoiceAndDeliveryAddressForCompanyToEdit
        private void GetInvoiceAndDeliveryAddressForCompanyToEdit()
        {
            DataTable dtbl = objCompany.GetInvoiceAddressForCompanyToEdit(Convert.ToInt32(ViewState["SubCompanyID"]));

            if (dtbl.Rows.Count > 0)
            {
                tbAddress1.Text = dtbl.Rows[0]["Address1"].ToString().Trim();
                tbAddress2.Text = dtbl.Rows[0]["Address2"].ToString().Trim();
                tbAddress3.Text = dtbl.Rows[0]["Address3"].ToString().Trim();
                tbPostCode.Text = dtbl.Rows[0]["PostCode"].ToString().Trim();
                tbPhoneNo.Text = dtbl.Rows[0]["TelePhone"].ToString().Trim();

                try
                {
                    ddlCounty.SelectedValue = dtbl.Rows[0]["CountyID"].ToString().Trim();

                }
                catch { }

                try
                {
                    ddlCountry.SelectedValue = dtbl.Rows[0]["CountryID"].ToString().Trim();

                }
                catch { }
            }
            dtbl.Dispose();
            dtbl = objCompany.GetDeliveryAddressForCompanyToEdit(Convert.ToInt32(ViewState["SubCompanyID"]));

            if (dtbl.Rows.Count > 0)
            {
                tbDeliveryAddress1.Text = dtbl.Rows[0]["Address1"].ToString().Trim();
                tbDeliveryAddress2.Text = dtbl.Rows[0]["Address2"].ToString().Trim();
                tbDeliveryAddress3.Text = dtbl.Rows[0]["Address3"].ToString().Trim();
                tbDeliveryPostCode.Text = dtbl.Rows[0]["PostCode"].ToString().Trim();
                tbDeliveryPhoneNo.Text = dtbl.Rows[0]["TelePhone"].ToString().Trim();

                try
                {
                    ddlCountyDelivery.SelectedValue = dtbl.Rows[0]["CountyID"].ToString().Trim();
                }
                catch { }
                try
                {
                    ddlCountryDelivery.SelectedValue = dtbl.Rows[0]["CountryID"].ToString().Trim();
                }
                catch { }
            }
        }
        #endregion
        #region CheckDuplicateCompanyName
        public int CheckDuplicateCompanyName(int iCompanyID, string strCompanyName)
        {
            objCompany = new Company();
            int iReturnValue = 0;
            iReturnValue = objCompany.CheckDuplicateCompanyName(iCompanyID, strCompanyName);
            return (iReturnValue);
        }
        #endregion

    }
}