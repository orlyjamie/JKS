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
using System.Data.SqlClient;
using System.Configuration;

using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;

namespace JKS
{
    /// <summary>
    /// Summary description for EditApproval.
    /// </summary>
    /// 
    [ScriptService]
    public partial class EditApproval : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region webcontrols
        protected System.Web.UI.WebControls.Panel Panel3;

        protected int iApproverID = 0;
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            btnSave.Attributes.Add("onclick", "javascript:return fn_Validate();");
            if (Convert.ToString(Request.QueryString["ApproverID"]) != "" || Request.QueryString["ApproverID"] != null)
            {
                iApproverID = Convert.ToInt32(Request.QueryString["ApproverID"]);
            }
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            if (!IsPostBack)
            {
                GetAllDropDowns();
                GetApproverDropDowns();
                LoadDepartment(Convert.ToInt32(ddlCompany.SelectedValue));
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue));
                if (iApproverID > 0)
                    GetRecordsFromApproval();
            }
            // Added By Mrinal on 24th February 2014
            //ddlCompany.SelectedItem.Attribute.add("Style","color:red");
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
            this.ddlSupplier.SelectedIndexChanged += new System.EventHandler(this.ddlSupplier_SelectedIndexChanged);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
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
                    //ddlCompany.Items.Insert(0, "Select Company Name");
                    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

                    ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                }

                //modified by kuntal on 29thApril2015
                else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    string LogInCompanyid = "";
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    //ddlCompany.Items.Insert(0, "Select Company Name");
                    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));
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
                    //ddlCompany.Items.Insert(0, "Select Company Name");
                    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

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
        #region GetAllDropDowns()
        public void GetAllDropDowns()
        {
            if (!Page.IsPostBack)
            {
                /*Blocked by kuntalkarar on 27thMay2016*/
                //GetCompanyDropDown();
                /*Added by kuntalkarar on 27thMay2016*/
                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
                //--------------------------------------
                GetSupplierDropDown();
                GetVendorClassDropDwons();
                GetVendorClassAgainstSupplierANDBuyer();

            }
        }
        #endregion

        #region GetCompanyDropDown()
        public void GetCompanyDropDown()
        {
            Approval oApproval = new Approval();
            DataSet ds = new DataSet();
            try
            {
                ds = oApproval.GetCompanyDropDownGeneric(System.Convert.ToInt32(Session["CompanyID"]));
                ddlCompany.DataSource = ds;
                ddlCompany.DataBind();
                ddlCompany.DataTextField = "CompanyName";
                ddlCompany.DataValueField = "CompanyID";
            }
            catch { }
            finally
            {
                ds = null;
            }
        }
        #endregion

        #region GetSupplierDropDown()
        public void GetSupplierDropDown()
        {
            Approval oApproval = new Approval();
            DataSet ds = new DataSet();
            try
            {
                ds = oApproval.GetSupplierFromTradingRelation(System.Convert.ToInt32(ddlCompany.SelectedValue));
                ddlSupplier.DataSource = ds;
                ddlSupplier.DataBind();
                ddlSupplier.Items.Insert(0, "Select");
            }
            catch { }
            finally
            {
                ds = null;
            }
        }
        #endregion

        #region GetVendorClassDropDwons()
        public void GetVendorClassDropDwons()
        {
            ddlVendor.Items.Insert(0, "Select");

        }
        #endregion

        #region GetApproverDropDowns()
        public void GetApproverDropDowns()
        {
            Approval oApproval = new Approval();
            DataSet ds = new DataSet();
            DataSet dsGroup = new DataSet();
            try
            {
                //int iBuyCompID = Convert.ToInt32(ddlCompany.SelectedValue);	
                JKS.Users oUser = new JKS.Users();

                dsGroup = oUser.GetApproversETC(Convert.ToInt32(Session["CompanyID"]));
                ddlApprover1.DataSource = dsGroup;
                ddlApprover1.DataTextField = "UserGroupName";
                ddlApprover1.DataValueField = "UserGroupName";
                ddlApprover1.DataBind();
                ddlApprover1.Items.Insert(0, "Select");
                ddlApprover2.DataSource = dsGroup;
                ddlApprover2.DataTextField = "UserGroupName";
                ddlApprover2.DataValueField = "UserGroupName";
                ddlApprover2.DataBind();
                ddlApprover2.Items.Insert(0, "Select");
                ddlApprover3.DataSource = dsGroup;
                ddlApprover3.DataTextField = "UserGroupName";
                ddlApprover3.DataValueField = "UserGroupName";
                ddlApprover3.DataBind();
                ddlApprover3.Items.Insert(0, "Select");

                ddlApprover4.DataSource = dsGroup;
                ddlApprover4.DataTextField = "UserGroupName";
                ddlApprover4.DataValueField = "UserGroupName";
                ddlApprover4.DataBind();
                ddlApprover4.Items.Insert(0, "Select");

                ddlApprover5.DataSource = dsGroup;
                ddlApprover5.DataTextField = "UserGroupName";
                ddlApprover5.DataValueField = "UserGroupName";
                ddlApprover5.DataBind();
                ddlApprover5.Items.Insert(0, "Select");
                ddlApprover6.DataSource = dsGroup;
                ddlApprover6.DataTextField = "UserGroupName";
                ddlApprover6.DataValueField = "UserGroupName";
                ddlApprover6.DataBind();
                ddlApprover6.Items.Insert(0, "Select");
                ddlApprover7.DataSource = dsGroup;
                ddlApprover7.DataTextField = "UserGroupName";
                ddlApprover7.DataValueField = "UserGroupName";
                ddlApprover7.DataBind();
                ddlApprover7.Items.Insert(0, "Select");

                ddlApprover8.DataSource = dsGroup;
                ddlApprover8.DataTextField = "UserGroupName";
                ddlApprover8.DataValueField = "UserGroupName";
                ddlApprover8.DataBind();
                ddlApprover8.Items.Insert(0, "Select");

                ddlApprover9.DataSource = dsGroup;
                ddlApprover9.DataTextField = "UserGroupName";
                ddlApprover9.DataValueField = "UserGroupName";
                ddlApprover9.DataBind();
                ddlApprover9.Items.Insert(0, "Select");
            }
            catch { }
            finally
            {
                ds = null;
                dsGroup = null;
            }

        }
        #endregion

        #region GetRecordsFromApproval
        public void GetRecordsFromApproval()
        {
            Approval oApproval = new Approval();
            DataSet ds = new DataSet();
            try
            {
                ds = oApproval.GetRecordsFromApproval(iApproverID);
                if (ds.Tables.Count > 0)
                {
                    try
                    {



                        if (ds.Tables[0].Rows[0]["BuyerCompanyID"].ToString() != "")
                            ddlCompany.SelectedValue = ds.Tables[0].Rows[0]["BuyerCompanyID"].ToString();

                        LoadDepartment(Convert.ToInt32(ds.Tables[0].Rows[0]["BuyerCompanyID"]));
                        GetBusinessUnit(Convert.ToInt32(ds.Tables[0].Rows[0]["BuyerCompanyID"]));
                        GetSupplierDropDown();
                        try
                        {
                            if (ds.Tables[0].Rows[0]["SupplierCompanyID"].ToString() != "")
                            {
                                ddlSupplier.SelectedValue = ds.Tables[0].Rows[0]["SupplierCompanyID"].ToString();

                                /*Added by kuntalkarar on 31stMay2017*/
                                RecordSet rs = new RecordSet();
                               rs=Company.GetCompanyData(Convert.ToInt32(ds.Tables[0].Rows[0]["SupplierCompanyID"].ToString()));
                               txtSupplier.Value = rs.ParentTable.Rows[0]["CompanyName"].ToString();
                               HdSupplierId.Value = ds.Tables[0].Rows[0]["SupplierCompanyID"].ToString();
                            }
                        }
                        catch { }

                        try
                        {
                            if (ds.Tables[0].Rows[0]["New_VendorClass"].ToString() != "")
                                ddlVendor.SelectedItem.Text = ds.Tables[0].Rows[0]["New_VendorClass"].ToString();
                        }
                        catch { }

                        if (ds.Tables[0].Rows[0]["NetFrom"].ToString() != "")
                            txtNetFrom.Text = ds.Tables[0].Rows[0]["NetFrom"].ToString();

                        if (ds.Tables[0].Rows[0]["NetTo"].ToString() != "")
                            txtNetTo.Text = ds.Tables[0].Rows[0]["NetTo"].ToString();

                        if (Convert.ToString(ds.Tables[0].Rows[0]["Department"]) != "")
                            ddlDepartment.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Department"]);

                        if (Convert.ToString(ds.Tables[0].Rows[0]["BusinessUnitID"]) != "")
                            ddlBusinessUnit.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["BusinessUnitID"]);
                    }
                    catch (Exception ex) { string ss = ex.Message.ToString(); }
                    try
                    {
                        if (ds.Tables[0].Rows[0]["Approver1"].ToString() != "")
                        {
                            ddlApprover1.SelectedValue = ds.Tables[0].Rows[0]["Approver1"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["Approver2"].ToString() != "")
                        {
                            ddlApprover2.SelectedValue = ds.Tables[0].Rows[0]["Approver2"].ToString();
                        }

                        if (ds.Tables[0].Rows[0]["Approver3"].ToString() != "")
                        {
                            ddlApprover3.SelectedValue = ds.Tables[0].Rows[0]["Approver3"].ToString();
                        }

                        if (ds.Tables[0].Rows[0]["Approver4"].ToString() != "")
                        {
                            ddlApprover4.SelectedValue = ds.Tables[0].Rows[0]["Approver4"].ToString();
                        }

                        if (ds.Tables[0].Rows[0]["Approver5"].ToString() != "")
                        {
                            ddlApprover5.SelectedValue = ds.Tables[0].Rows[0]["Approver5"].ToString();
                        }

                        if (ds.Tables[0].Rows[0]["Approver6"].ToString() != "")
                        {
                            ddlApprover6.SelectedValue = ds.Tables[0].Rows[0]["Approver6"].ToString();
                        }

                        if (ds.Tables[0].Rows[0]["Approver7"].ToString() != "")
                        {
                            ddlApprover7.SelectedValue = ds.Tables[0].Rows[0]["Approver7"].ToString();
                        }

                        if (ds.Tables[0].Rows[0]["Approver8"].ToString() != "")
                        {
                            ddlApprover8.SelectedValue = ds.Tables[0].Rows[0]["Approver8"].ToString();
                        }

                        if (ds.Tables[0].Rows[0]["Approver9"].ToString() != "")
                        {
                            ddlApprover9.SelectedValue = ds.Tables[0].Rows[0]["Approver9"].ToString();
                        }
                    }
                    catch (Exception ex) { string ss = ex.Message.ToString(); }
                }
            }
            catch { }
            finally
            {
                ds = null;
            }
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            bool Check = true;
            /*
            if(Convert.ToString(ddlVendor.SelectedValue)=="Select")
            {
                //Check=false;
            }
            if(Convert.ToInt32(ddlDepartment.SelectedValue)==0 && Convert.ToInt32(ddlBusinessUnit.SelectedValue)==0)
            {
                //Check=false;
            }
            */
            if (Convert.ToString(ddlApprover1.SelectedValue) == "Select" && Convert.ToString(ddlApprover2.SelectedValue) == "Select" &&
                Convert.ToString(ddlApprover3.SelectedValue) == "Select" && Convert.ToString(ddlApprover4.SelectedValue) == "Select" &&
                Convert.ToString(ddlApprover5.SelectedValue) == "Select" && Convert.ToString(ddlApprover6.SelectedValue) == "Select" &&
                Convert.ToString(ddlApprover7.SelectedValue) == "Select" && Convert.ToString(ddlApprover8.SelectedValue) == "Select" &&
                Convert.ToString(ddlApprover9.SelectedValue) == "Select")
            {
                Check = false;
            }

            if (Check)
            {
                #region variables
                Approval oApproval = new Approval();
                int iReturnval = 0;
                int iSuppID = 0;
                int iBuyerID = 0;
                decimal iNetF = 0;
                decimal iNetT = 0;
                string VendorClass = "";
                string strApprover1 = "";
                string strApprover2 = "";
                string strApprover3 = "";
                string strApprover4 = "";
                string strApprover5 = "";
                string strApprover6 = "";
                string strApprover7 = "";
                string strApprover8 = "";
                string strApprover9 = "";

                #endregion

                #region values
                //if(Convert.ToString(ddlSupplier.SelectedValue)!="Select")
                //    iSuppID = Convert.ToInt32(ddlSupplier.SelectedValue);
                //else
                //    iSuppID = 0;

                if (txtSupplier.Value.ToString().Trim().Length > 0)
                {
                    if (HdSupplierId.Value != "")
                        iSuppID = Convert.ToInt32(HdSupplierId.Value);
                }
                else
                    iSuppID = 0;


                if (ddlCompany.SelectedIndex >= 0)
                    iBuyerID = Convert.ToInt32(ddlCompany.SelectedValue);
                else
                    iBuyerID = Convert.ToInt32(Session["CompanyID"]);


                if (Convert.ToString(ddlVendor.SelectedValue) == "Select")
                    VendorClass = DBNull.Value.ToString();
                else
                    VendorClass = Convert.ToString(ddlVendor.SelectedItem.Text);


                if (Convert.ToString(ddlApprover1.SelectedValue) == "Select")
                    strApprover1 = DBNull.Value.ToString();
                else
                    strApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);

                if (Convert.ToString(ddlApprover2.SelectedValue) == "Select")
                    strApprover2 = DBNull.Value.ToString();
                else
                    strApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);

                if (Convert.ToString(ddlApprover3.SelectedValue) == "Select")
                    strApprover3 = DBNull.Value.ToString();
                else
                    strApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);

                if (Convert.ToString(ddlApprover4.SelectedValue) == "Select")
                    strApprover4 = DBNull.Value.ToString();
                else
                    strApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);

                if (Convert.ToString(ddlApprover5.SelectedValue) == "Select")
                    strApprover5 = DBNull.Value.ToString();
                else
                    strApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);

                if (Convert.ToString(ddlApprover6.SelectedValue) == "Select")
                    strApprover6 = DBNull.Value.ToString();
                else
                    strApprover6 = Convert.ToString(ddlApprover6.SelectedItem.Text);

                if (Convert.ToString(ddlApprover7.SelectedValue) == "Select")
                    strApprover7 = DBNull.Value.ToString();
                else
                    strApprover7 = Convert.ToString(ddlApprover7.SelectedItem.Text);

                if (Convert.ToString(ddlApprover8.SelectedValue) == "Select")
                    strApprover8 = DBNull.Value.ToString();
                else
                    strApprover8 = Convert.ToString(ddlApprover8.SelectedItem.Text);

                if (Convert.ToString(ddlApprover9.SelectedValue) == "Select")
                    strApprover9 = DBNull.Value.ToString();
                else
                    strApprover9 = Convert.ToString(ddlApprover9.SelectedItem.Text);

                if (txtNetFrom.Text != "")
                {
                    if (IsDecimal(txtNetFrom.Text.Trim()))
                        iNetF = Convert.ToDecimal(txtNetFrom.Text.Trim());
                    else
                    {
                        Response.Write("<script>alert('Please Enter Numeric Values.');</script>");
                        lblMessage.Text = "Please Enter Numeric Values.";
                        return;
                    }
                }
                else
                    iNetF = 0;

                if (txtNetTo.Text != "")
                {
                    if (IsDecimal(txtNetTo.Text.Trim()))
                        iNetT = Convert.ToDecimal(txtNetTo.Text.Trim());
                    else
                    {
                        Response.Write("<script>alert('Please Enter Numeric Values.');</script>");
                        lblMessage.Text = "Please Enter Numeric Values.";
                        return;
                    }
                }
                else
                    iNetT = 0;


                #endregion

                iReturnval = oApproval.SaveApprovalPaths(iApproverID, iBuyerID, iSuppID, VendorClass,
                    iNetF, iNetT, strApprover1, strApprover2, strApprover3, strApprover4, strApprover5,
                    strApprover6, strApprover7, strApprover8, strApprover9,
                    Convert.ToInt32(ddlDepartment.SelectedValue), Convert.ToInt32(ddlBusinessUnit.SelectedValue), Convert.ToInt32(Session["UserID"]));

                if (iReturnval > 0)
                    Response.Write("<script>alert('Records saved successfully.');top.location.replace('BrowseApprovalPath.aspx');</script>");
                else
                {
                    Response.Write("<script>alert('Error saving records.');</script>");
                    lblMessage.Text = "Error saving records.";
                }
            }
        }
        #endregion


        #region ddlSupplier_SelectedIndexChanged
        protected void ddlSupplier_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GetVendorClassAgainstSupplierANDBuyer();
        }
        #endregion

        #region GetVendorClassAgainstSupplierANDBuyer
        private void GetVendorClassAgainstSupplierANDBuyer()
        {
            int iBuyerCID = 0;
            int iSupplierCID = 0;

            iBuyerCID = Convert.ToInt32(ddlCompany.SelectedValue);

            //if(Convert.ToString(ddlSupplier.SelectedValue)!="Select" )
            //    iSupplierCID = Convert.ToInt32(ddlSupplier.SelectedValue);
            if (Convert.ToString(HdSupplierId.Value) != "")
            {
                iSupplierCID = Convert.ToInt32(HdSupplierId.Value);
            }
            else
            {
                iSupplierCID = 0;
            }

            Approval oApproval = new Approval();
            DataSet ds = new DataSet();
            try
            {
                string Fields = "DISTINCT New_VendorClass AS  VendorClass";
                string Table = "TradingRelation";
                //  string Criteria = "BuyerCompanyID  = "+iBuyerCID+ " AND New_VendorClass IS NOT NULL AND SupplierCompanyID="+iSupplierCID;
                string Criteria = "BuyerCompanyID  = " + iBuyerCID + " AND New_VendorClass IS NOT NULL";// AND SupplierCompanyID=" + HdSupplierId;
                ds = oApproval.GetGlobalDropDowns(Fields, Table, Criteria);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlVendor.DataSource = ds;
                    ddlVendor.DataBind();
                }
                ddlVendor.Items.Insert(0, "Select");
            }
            catch { }
            finally
            {
                ds = null;
            }
        }
        #endregion

        #region ddlCompany_SelectedIndexChanged
        protected void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlCompany.Text != "Select Company Name")//Added by Mainak 2018-11-29
            {
                txtSupplier.Value = "";
                GetSupplierDropDown();
                GetVendorClassAgainstSupplierANDBuyer();
                LoadDepartment(Convert.ToInt32(ddlCompany.SelectedValue));
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue));
            }

        }
        #endregion


        #region IsDecimal(string sText)
        public bool IsDecimal(string sText)
        {
            decimal iVal = 0;
            bool Retval = true;
            try
            {
                iVal = Convert.ToDecimal(sText.Trim());
                Retval = true;
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); Retval = false; }
            return Retval;
        }
        #endregion

        #region  btnCancel_Click
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            //blocked and added by Koushik das as on 16-Mar-2017
            //Server.Transfer("BrowseApprovalPath.aspx");
            Response.Redirect("BrowseApprovalPath.aspx", true);
            //added by Koushik das as on 16-Mar-2017
        }
        #endregion

        private void LoadDepartment(int companyid)
        {
            ddlDepartment.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
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
                    ddlDepartment.DataSource = ds;
                    ddlDepartment.DataBind();
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
            ddlDepartment.Items.Insert(0, new ListItem("Select Department", "0"));
        }
        private void GetBusinessUnit(int companyid)
        {
            ddlBusinessUnit.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
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
            ddlBusinessUnit.Items.Insert(0, new ListItem("Select Business Unit", "0"));
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


    }
}
