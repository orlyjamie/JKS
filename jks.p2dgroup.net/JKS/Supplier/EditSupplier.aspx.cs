using System;
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
using System.Text;
using System.Web.Mail;
using System.IO;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Net;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace JKS
{
    /// <summary>
    /// Summary description for EditSupplier.
    /// </summary>
    /// 
    [ScriptService]
    public partial class EditSupplier : CBSolutions.ETH.Web.ETC.VSPage
    {

        private MailFormat _mailFormat = MailFormat.Html;
        private MailPriority _mailPriority = MailPriority.High;
        int iTradingID = 0;
        //Boolean approvalneed = false;
        private void Page_Load(object sender, System.EventArgs e)
        {
            btnSave.Attributes.Add("onclick", "return fn_Validate();");
            if (Request.QueryString["ComID"] != null)
                iTradingID = Convert.ToInt32(Request.QueryString["ComID"]);


            if (!Page.IsPostBack)
            {
                ddlPreApprove.SelectedValue = "0";
                GetDropDowns();
                if (iTradingID != 0)
                    GetAllrecordsFromFromCompanyAndTrading(iTradingID);

                //Added by Mainak 2018-09-21
                if (iTradingID == 0)
                {
                    LoadDepartment();
                }

            }
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
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region DropDowns
        //Added by Mainak 2018-09-21
        private void LoadDepartment()
        {
            JKS.Supplier oSupplier = new JKS.Supplier();
            ddlExpDept.Items.Clear();

            DataSet ds = new DataSet();

            if (Convert.ToInt32(ddlCompany.SelectedValue) != 0)
            {
                ds = oSupplier.LoadDepartment(Convert.ToInt32(ddlCompany.SelectedValue), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
            }
            else
            {
                ds = oSupplier.LoadDepartment(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
            }
            ddlExpDept.DataSource = ds;

            ddlExpDept.DataTextField = "Department";
            ddlExpDept.DataValueField = "DepartmentID";
            ddlExpDept.DataBind();
            ddlExpDept.Items.Insert(0, new ListItem("Select Department", "0"));
        }
        /*--------------Added by kuntalkarar on 27thMay2016-------------------*/
        private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int iAction)
        {
            if (iAction == 1)
            {
                Company objCompany = new Company();
                ddlCompany.Items.Clear();
                DataTable dt = new DataTable();
                //try
                //{

                //if (dt.Rows.Count > 0)
                //{
                //    ddlCompany.DataSource = dt;
                //    ddlCompany.DataBind();

                //    //--------------- Set Default Selected value  "Select Company Name"  by Subha,04-02-2015
                if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, "Select Company Name");

                    ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                }

                //modified by kuntal on 29thApril2015
                else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    string LogInCompanyid = "";
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, "Select Company Name");
                    PasswordReset objPasswordReset = new PasswordReset();
                    List<PasswordReset> lstSaltedPassword = objPasswordReset.GetLogInCompanyId(Convert.ToInt32(Session["UserID"]));//strResetAnswer
                    if (lstSaltedPassword.Count > 0)
                    {
                        LogInCompanyid = lstSaltedPassword[0].LoginCompanyId;
                    }


                    ddlCompany.SelectedValue = LogInCompanyid;

                }
                //else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                //{
                //    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                //    ddlCompany.DataSource = dt;
                //    ddlCompany.DataBind();
                //    ddlCompany.Items.Insert(0, "Select Company Name");

                //}
                //------------------------------------------------------------
                else
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, "Select Company Name");

                    ddlCompany.SelectedValue = dt.Rows[0][0].ToString();
                    Session["DropDownCompanyID"] = ddlCompany.SelectedValue.ToString();
                }
                //}
            }
            //catch { }
            //finally
            //{
            //    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

            //}
            //}




        }
        //---------------------------------------------------------------------
        #region GetDropDowns
        public void GetDropDowns()
        {
            /*Blocked by kuntalkarar on 27thMay2016*/
            //GetCompanyDropDown();
            /*Added by kuntalkarar on 27thMay2016*/
            GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
            //--------------------------------------
            GetCountyList();
            GetCountryList();
            GetCurrencyType();
            GETNominalCode();

        }
        #endregion
        #region GETNominalCode()
        public void GETNominalCode()
        {
            //ddlNominalCode1.Items.Clear();
            //ddlNominalCode2.Items.Clear();
            //Supplier oSupplier = new Supplier();
            //DataSet ds = new DataSet();
            //try
            //{
            //    ds = oSupplier.GETNominalCode(System.Convert.ToInt32(ddlCompany.SelectedValue));
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        ddlNominalCode1.DataSource = ds;
            //        ddlNominalCode1.DataTextField = "NominalCode";
            //        ddlNominalCode1.DataValueField = "NominalCodeID";
            //        ddlNominalCode1.DataBind();
            //        ddlNominalCode2.DataSource = ds;
            //        ddlNominalCode2.DataTextField = "NominalCode";
            //        ddlNominalCode2.DataValueField = "NominalCodeID";
            //        ddlNominalCode2.DataBind();
            //    }
            //}
            //catch { }
            //finally
            //{
            //    ds = null;
            //    ddlNominalCode1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select F&B NominalCode", "0"));
            //    ddlNominalCode2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select EXP NominalCode", "0"));
            //}

        }
        #endregion
        #region GetCompanyDropDown()
        public void GetCompanyDropDown()
        {
            Supplier oSupplier = new Supplier();
            DataSet ds = new DataSet();
            try
            {
                ds = oSupplier.GetCompanyDropDownGeneric(System.Convert.ToInt32(Session["CompanyID"]));
                ddlCompany.DataSource = ds;
                ddlCompany.DataBind();
                ddlCompany.DataTextField = "CompanyName";
                ddlCompany.DataValueField = "CompanyID";
                ddlCompany.Items.Insert(0, new ListItem("Select Buyer Company", "0"));

            }

            catch { }
            finally
            {
                ds = null;
            }


        }
        #endregion

        #region GetCountyList()
        public void GetCountyList()
        {
            DataTable dtbl = new DataTable();
            Supplier oSupplier = new Supplier();
            try
            {
                dtbl = oSupplier.GetCountyList();
                ddlCounty.DataSource = dtbl;
                ddlCounty.DataBind();
            }
            catch { }
            finally
            {
                dtbl = null;
                ddlCounty.Items.Insert(0, "Select County");
            }

        }
        #endregion

        #region GetCountryList()
        public void GetCountryList()
        {
            DataTable dtbl = new DataTable();
            Supplier oSupplier = new Supplier();
            try
            {
                dtbl = oSupplier.GetCountryList();
                ddlCountry.DataSource = dtbl;
                ddlCountry.DataBind();
            }
            catch { }
            finally
            {
                dtbl = null;
                ddlCountry.Items.Insert(0, "Select Country");
            }
        }
        #endregion

        #region GetCurrencyType()
        public void GetCurrencyType()
        {
            RecordSet rs = null;
            try
            {
                rs = CBSolutions.ETH.Web.Invoice.GetCurrencyTypesList();
                while (!rs.EOF())
                {
                    ListItem listItem = new ListItem(rs["CurrencyCode"].ToString(), rs["CurrencyTypeID"].ToString());
                    cboCurrencyType.Items.Add(listItem);
                    rs.MoveNext();
                }
            }
            catch { }
            finally
            {
                rs = null;
                cboCurrencyType.Items.Insert(0, "Select Currency");
            }
        }
        #endregion
        #endregion

        #region GetAllrecordsFromFromCompanyAndTrading(int tradingid)
        private void GetAllrecordsFromFromCompanyAndTrading(int tradingid)
        {
            DataSet ds = new DataSet();
            Supplier objSupp = new Supplier();
            //Added by Mainak 2018-09-21
            ddlExpDept.Items.Clear();
            try
            {
                ds = objSupp.GetAllrecordsFromFromCompanyAndTrading(tradingid);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txtSupplier.Text = ds.Tables[1].Rows[0]["CompanyName"].ToString();
                    txtAddress1.Text = ds.Tables[1].Rows[0]["Address1"].ToString();
                    txtAddress2.Text = ds.Tables[1].Rows[0]["Address2"].ToString();
                    txtAddress3.Text = ds.Tables[1].Rows[0]["Address3"].ToString();
                    thtPostCode.Text = ds.Tables[1].Rows[0]["PostCode"].ToString();
                    ddlCounty.SelectedItem.Text = ds.Tables[1].Rows[0]["CountyID"].ToString();
                    ddlCountry.SelectedItem.Text = ds.Tables[1].Rows[0]["CountryID"].ToString();
                    txtTelephone.Text = ds.Tables[1].Rows[0]["PhoneNumber1"].ToString();
                    txtEmail.Text = ds.Tables[1].Rows[0]["EmailID"].ToString();
                    txtApCompanyID.Text = ds.Tables[1].Rows[0]["New_APCompanyID"].ToString();
                    txtVatNo.Text = ds.Tables[1].Rows[0]["VatRegNo"].ToString();
                    thtVendorGroup.Text = ds.Tables[1].Rows[0]["New_VendorGroup"].ToString();
                    if (ds.Tables[1].Rows[0]["isActive"].ToString() == "false" || ds.Tables[1].Rows[0]["isActive"].ToString() == "False")
                        ddlStatus.SelectedIndex = 1;
                    else
                        ddlStatus.SelectedIndex = 0;

                    ViewState["NetWorkID"] = ds.Tables[1].Rows[0]["NetworkID"].ToString();

                    //---------------------------------------------------

                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string appr = ds.Tables[0].Rows[0]["ApprovalNeeded"].ToString();
                    if (appr == "0" || appr == "False")
                    {
                        ddlFnBNeed.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlFnBNeed.SelectedIndex = 1;
                    }
                    ddlCompany.SelectedValue = ds.Tables[0].Rows[0]["BuyerCompanyID"].ToString();
                    ddlCompany.Enabled = false;
                    GETNominalCode();
                    TxtVendorID.Text = ds.Tables[0].Rows[0]["SupplierCodeAgainstBuyer"].ToString();
                    thtVendorClass.Text = ds.Tables[0].Rows[0]["New_VendorClass"].ToString();
                    cboCurrencyType.SelectedItem.Text = ds.Tables[0].Rows[0]["New_CurrencyCode"].ToString();
                    /*if(ds.Tables[0].Rows[0]["Nominal1"]!=DBNull.Value && Convert.ToString(ds.Tables[0].Rows[0]["Nominal1"])!="")
                    {
                        //ddlNominalCode1.SelectedValue=Convert.ToString(ds.Tables[0].Rows[0]["Nominal1"]);

                        //-- added by Subha das 9th January 2015
                        txtNominal1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Nominal1"]);     
                    }
                    if(ds.Tables[0].Rows[0]["Nominal2"]!=DBNull.Value && Convert.ToString(ds.Tables[0].Rows[0]["Nominal2"])!="")
                    {
                        //ddlNominalCode2.SelectedValue=Convert.ToString(ds.Tables[0].Rows[0]["Nominal2"]);

                        //-- added by Subha das 9th January 2015
                        txtNominal2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Nominal2"]);
                    }*/
                    if (ds.Tables[0].Rows[0]["NominalCode1Details"] != DBNull.Value && Convert.ToString(ds.Tables[0].Rows[0]["NominalCode1Details"]) != "")
                    {
                        txtNominal1.Text = Convert.ToString(ds.Tables[0].Rows[0]["NominalCode1Details"]);
                        hdNominalCodeId1.Value = Convert.ToString(ds.Tables[0].Rows[0]["NominalCodeID1"]);
                    }
                    if (ds.Tables[0].Rows[0]["NominalCode2Details"] != DBNull.Value && Convert.ToString(ds.Tables[0].Rows[0]["NominalCode2Details"]) != "")
                    {
                        txtNominal2.Text = Convert.ToString(ds.Tables[0].Rows[0]["NominalCode2Details"]);
                        hdNominalCodeId2.Value = Convert.ToString(ds.Tables[0].Rows[0]["NominalCodeID2"]);
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["PreApprove"]) == "True")
                    {
                        ddlPreApprove.SelectedValue = "1";
                    }
                    else
                    {
                        ddlPreApprove.SelectedValue = "0";
                    }


                    //Added by Mainak 2018-09-21
                    try
                    {
                        DataSet dsL = new DataSet();

                        dsL = objSupp.LoadDepartment(Convert.ToInt32(ds.Tables[0].Rows[0]["BuyerCompanyID"].ToString()), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));

                        ddlExpDept.DataSource = dsL;

                        ddlExpDept.DataTextField = "Department";
                        ddlExpDept.DataValueField = "DepartmentID";
                        ddlExpDept.DataBind();
                        ddlExpDept.Items.Insert(0, new ListItem("Select Department", "0"));
                        ddlExpDept.SelectedValue = ds.Tables[0].Rows[0]["EXPDepartmentID"].ToString();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                string ErrMsg = ex.Message.ToString();
            }
            finally
            {
                ds = null;
            }
        }
        #endregion

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            Supplier_New objAddApproval = new Supplier_New();

            int IretVal = 0;
            string iCounty = "";
            string iCountry = "";
            string NetWorkID = "";
            string sVendorClass = "";
            string Currency = "GBP";
            //blocked by kuntal karar on 03.03.2015---------------------
            // Supplier objSupp = new Supplier();

            //------------------Added by kuntal karar on 03.03.2015-----for pt. no. 27-----------------------------------


            //---------------------------------------------------------------------------------------------
            if (iTradingID == 0)
            {
                NetWorkID = System.Guid.NewGuid().ToString().Substring(0, 11).Trim();
                ViewState["NetWorkID"] = NetWorkID;
            }
            sVendorClass = thtVendorClass.Text.Trim();
            iCounty = ddlCounty.SelectedItem.Text.Trim();
            iCountry = ddlCountry.SelectedItem.Text.Trim();
            if (Convert.ToString(cboCurrencyType.SelectedItem.Text) != "Select Currency")
                Currency = Convert.ToString(cboCurrencyType.SelectedItem.Text);

            //----adding by Subha Das 9th January 2015
            string NominalCodeID1 = "";
            string NominalCodeID2 = "";

            if (txtNominal1.Text.ToString() != "")
            {
                NominalCodeID1 = txtNominal1.Text.Trim().Substring(0, txtNominal1.Text.Trim().IndexOf('/'));
                // NominalCodeID1 = txtNominal1.Text.Trim();
            }

            if (txtNominal2.Text.ToString() != "")
                NominalCodeID2 = txtNominal2.Text.Trim().Substring(0, txtNominal2.Text.Trim().IndexOf('/'));

            //IretVal = objSupp.SaveCompanyDetailsAndTradingRelations(iTradingID, Convert.ToInt32(ddlCompany.SelectedValue), txtSupplier.Text.Trim(), NetWorkID, txtAddress1.Text.Trim(),
            //    txtAddress2.Text.ToString(), txtAddress3.Text.ToString(), thtPostCode.Text.ToString(), iCounty, iCountry, txtTelephone.Text.ToString(), txtEmail.Text.ToString(), txtVatNo.Text.ToString(),
            //    TxtVendorID.Text.ToString(), thtVendorClass.Text.ToString(), thtVendorGroup.Text.ToString(), txtApCompanyID.Text.ToString(), Currency, Convert.ToInt32(ddlStatus.SelectedValue), Convert.ToInt32(Session["UserID"]), Convert.ToString(ddlNominalCode1.SelectedValue), Convert.ToString(ddlNominalCode2.SelectedValue), Convert.ToInt32(ddlPreApprove.SelectedValue));



            //----------------------blocked by kuntal karar on 03.03.2015----------------------------------

            //IretVal = objAddApproval.SaveCompanyDetailsAndTradingRelations(iTradingID, Convert.ToInt32(ddlCompany.SelectedValue), txtSupplier.Text.Trim(), NetWorkID, txtAddress1.Text.Trim(),
            //    txtAddress2.Text.ToString(), txtAddress3.Text.ToString(), thtPostCode.Text.ToString(), iCounty, iCountry, txtTelephone.Text.ToString(), txtEmail.Text.ToString(), txtVatNo.Text.ToString(),
            //    TxtVendorID.Text.ToString(), thtVendorClass.Text.ToString(), thtVendorGroup.Text.ToString(), txtApCompanyID.Text.ToString(), Currency, Convert.ToInt32(ddlStatus.SelectedValue), Convert.ToInt32(Session["UserID"]),
            //    NominalCodeID1, NominalCodeID2, Convert.ToInt32(ddlPreApprove.SelectedValue));
            //----------------------------------------------------------------------------------------------
            //Modified by Mainak  2018-09-21
            //IretVal = objAddApproval.SaveCompanyDetailsAndTradingRelations(iTradingID, Convert.ToInt32(ddlCompany.SelectedValue), txtSupplier.Text.Trim(), NetWorkID, txtAddress1.Text.Trim(),
            //    txtAddress2.Text.ToString(), txtAddress3.Text.ToString(), thtPostCode.Text.ToString(), iCounty, iCountry, txtTelephone.Text.ToString(), txtEmail.Text.ToString(), txtVatNo.Text.ToString(),
            //    TxtVendorID.Text.ToString(), thtVendorClass.Text.ToString(), thtVendorGroup.Text.ToString(), txtApCompanyID.Text.ToString(), Currency, Convert.ToInt32(ddlStatus.SelectedValue), Convert.ToInt32(Session["UserID"]),
            //    NominalCodeID1, NominalCodeID2, Convert.ToInt32(ddlPreApprove.SelectedValue), Convert.ToInt32(ddlFnBNeed.SelectedValue));

            IretVal = objAddApproval.SaveCompanyDetailsAndTradingRelations(iTradingID, Convert.ToInt32(ddlCompany.SelectedValue), txtSupplier.Text.Trim(), NetWorkID, txtAddress1.Text.Trim(),
                txtAddress2.Text.ToString(), txtAddress3.Text.ToString(), thtPostCode.Text.ToString(), iCounty, iCountry, txtTelephone.Text.ToString(), txtEmail.Text.ToString(), txtVatNo.Text.ToString(),
                TxtVendorID.Text.ToString(), thtVendorClass.Text.ToString(), thtVendorGroup.Text.ToString(), txtApCompanyID.Text.ToString(), Currency, Convert.ToInt32(ddlStatus.SelectedValue), Convert.ToInt32(Session["UserID"]),
                NominalCodeID1, NominalCodeID2, Convert.ToInt32(ddlPreApprove.SelectedValue), Convert.ToInt32(ddlFnBNeed.SelectedValue), Convert.ToInt32(ddlExpDept.SelectedValue));

             //Response.Write(
             //   "<br />TradingID: " + iTradingID
             //   + "<br />ddlCompany.SelectedValue : " + ddlCompany.SelectedValue
             //   + "<br />txtSupplier.Text: " + txtSupplier.Text.Trim()
             //   + "<br />NetWorkID: " + NetWorkID
             //   + "<br />txtAddress1.Text: " + txtAddress1.Text.Trim()
             //   + "<br />txtAddress2.Text: " + txtAddress2.Text.ToString()
             //   + "<br />txtAddress3.Text: " + txtAddress3.Text.ToString()
             //   + "<br />thtPostCode.Text: " + thtPostCode.Text.ToString()
             //   + "<br />iCounty: " + iCounty
             //   + "<br />iCountry: " + iCountry
             //   + "<br />txtTelephone.Text: " + txtTelephone.Text.ToString()
             //   + "<br />txtEmail.Text: " + txtEmail.Text.ToString()
             //   + "<br />txtVatNo.Text: " + txtVatNo.Text.ToString()
             //   + "<br />TxtVendorID.Text: " + TxtVendorID.Text.ToString()
             //   + "<br />thtVendorClass.Text: " + thtVendorClass.Text.ToString()
             //   + "<br />thtVendorGroup.Text: " + thtVendorGroup.Text.ToString()
             //   + "<br />txtApCompanyID.Text: " + txtApCompanyID.Text.ToString()
             //   + "<br />txtApCompanyID.Text: " + Currency
             //   + "<br />ddlStatus.SelectedValue : " + ddlStatus.SelectedValue
             //   + "<br />Session[\"UserID\"]: " + Session["UserID"]
             //   + "<br />NominalCodeID1: " + NominalCodeID1
             //   + "<br />NominalCodeID2: " + NominalCodeID2
             //   + "<br />ddlPreApprove.SelectedValue: " + ddlPreApprove.SelectedValue
             //   + "<br />ddlFnBNeed.SelectedValue: " + ddlFnBNeed.SelectedValue
             //   );

            //If SAVE is pressed in the page below when Active = ‘No’ then the system 
            //should replicate the same functions as if the user had pressed the ‘Delete’ 
            //button, as mentioned above.
            if (ddlStatus.SelectedIndex == 1 && Request.QueryString["ComID"] != null)
            {
                objAddApproval.DeleteSupplierRecord(iTradingRelationID: iTradingID, iModUserID: Convert.ToInt32(Session["UserID"]));
            }

            //Response.Write("<br /><br /><br />IretVal: " + IretVal);

            if (IretVal > 0)
            {
                if (IretVal == 1)
                {
                    lblMessage.Text = "Error in saving data.";
                    Response.Write("<script>alert('Error in saving data.');</script>");
                }
                if (IretVal == 2)
                {
                    lblMessage.Text = "Record(s) saved successfully.";
                    Response.Write("<script>alert('Record(s) save successfully.');</script>");
                    Response.Redirect("BrowseSupplier.aspx");
                }
                if (IretVal > 10)
                {
                    lblMessage.Text = " company supplies stock.";
                    //blocked by kuntalkarar on 10thMay2017
                    //SendEmail();
                    Response.Redirect("BrowseSupplier.aspx");
                }
            }
            else
            {
                string str = "";

                if (IretVal == -101)
                    str = "Vendorid already exists against this buyer and supplier.";
                else if (IretVal == -102)
                    str = "Company Name already exists against this buyer.";
                else if (IretVal == 0)
                    str = "An unexpected error occured.";

                lblMessage.Text = str;
                Response.Write("IretVal: "+ IretVal + ",  " + str);
                Response.Write("<script>alert('" + str + "');</script>");
            }
        }

        #region SendEmail
        private void SendEmail()
        {
            try
            {
                MailMessage _mailMSG = new MailMessage();
                _mailMSG.From = "support@p2dgroup.com";
                _mailMSG.To = "P2d@vnsinfo.com.au";
                _mailMSG.Cc = "P2d@vnsinfo.com.au";
                _mailMSG.Bcc = "P2d@vnsinfo.com.au";
                _mailMSG.Subject = "a new company registered.";
                _mailMSG.Body = GetMailBody();
                _mailMSG.Priority = _mailPriority;
                _mailMSG.BodyFormat = _mailFormat;
                SmtpMail.SmtpServer = "";
                SmtpMail.Send(_mailMSG);
            }
            catch (Exception ex) { string ss = ex.Message.Trim(); }
        }
        #endregion

        #region string GetMailBody()
        public string GetMailBody()
        {
            string StreamReaderStrLine = "";
            StreamReader StmRdr = null;
            FileInfo fi = null;
            string strFileName = Request.PhysicalApplicationPath + "NewBuyer\\Supplier\\AddCompany.htm";
            fi = new FileInfo(strFileName);
            if (fi.Exists)
            {
                try
                {
                    StmRdr = File.OpenText(strFileName);
                    StreamReaderStrLine = StmRdr.ReadToEnd();
                    StreamReaderStrLine = StreamReaderStrLine.Replace("<COMPANY>", txtSupplier.Text.Trim()).Replace("<COMPEMAIL>", txtEmail.Text.Trim()).Replace("<PHONE>", txtTelephone.Text).Replace("\"", "'").Replace("<BUYER>", ddlCompany.SelectedItem.Text.Trim()).Replace("<VANDORID>", TxtVendorID.Text).Replace("<VENDORCLASS>", thtVendorClass.Text).Replace("<VENDORGROUP>", thtVendorGroup.Text).Replace("<APCOMPANY>", txtApCompanyID.Text).Replace("<CURRENCY>", cboCurrencyType.SelectedItem.Text).Replace("<ADDRESS>", txtAddress1.Text + "  " + txtAddress2.Text + "  " + txtAddress3.Text).Replace("<VAT>", txtVatNo.Text).Replace("<COUNTRY>", ddlCountry.SelectedItem.Text);
                    StreamReaderStrLine = StreamReaderStrLine.Replace("<NETWORK>", Convert.ToString(ViewState["NetWorkID"]));
                    StreamReaderStrLine = StreamReaderStrLine.Replace("\r\n", "");
                    StreamReaderStrLine = StreamReaderStrLine.Replace("<!--body", ".body").Replace("}-->", "}").Replace("\t", "");
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
            }
            return StreamReaderStrLine;
        }
        #endregion

        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            txtNominal1.Text = "";
            txtNominal2.Text = "";
            hdNominalCodeId1.Value = "";
            hdNominalCodeId2.Value = "";
            GETNominalCode();

            //Added by Mainak 2018-09-21
            LoadDepartment();
        }


        [WebMethod]
        public static string[] GetNominalName(string CompanyID, string UserString)
        {
            DataSet dsNominalName = new DataSet();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetNominalName", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(CompanyID));
            sqlDA.SelectCommand.Parameters.Add("@NominalStr", UserString);

            try
            {
                sqlConn.Open();
                sqlDA.Fill(dsNominalName);
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
            if (dsNominalName != null && dsNominalName.Tables.Count > 0 && dsNominalName.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsNominalName.Tables[0].Rows)
                {
                    myList.Add(string.Format("{0}#{1}", row["NominalCodeID"].ToString(), row["label"].ToString()));
                    // myList.Add(string.Format("{0}", row["label"].ToString()));
                }
                return myList.ToArray();
                // return JsonConvert.SerializeObject(dsSupplier.Tables[0]);
            }
            else
                return null;

        }
    }
}
