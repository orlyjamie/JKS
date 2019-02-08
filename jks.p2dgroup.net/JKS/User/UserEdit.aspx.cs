using System;
using System.IO;
using System.Data.SqlTypes;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;

namespace JKS
{
    /// <summary>
    /// Summary description for UserEdit.
    /// </summary>
    public partial class UserEdit : CBSolutions.ETH.Web.ETC.VSPage
    {
        //#region WebControls
        //protected System.Web.UI.WebControls.Panel Panel2;
        //protected System.Web.UI.WebControls.TextBox txtUserName;
        //protected System.Web.UI.WebControls.TextBox txtPassword;
        //protected System.Web.UI.WebControls.TextBox txtFirstName;
        //protected System.Web.UI.WebControls.TextBox txtSurname;
        //protected System.Web.UI.WebControls.TextBox txtTelephone;
        //protected System.Web.UI.WebControls.TextBox txtMobile;
        //protected System.Web.UI.WebControls.TextBox txtEmail;
        //protected System.Web.UI.WebControls.DropDownList UserTypeDropDown;
        //protected System.Web.UI.WebControls.DropDownList cboRole;
        //protected System.Web.UI.WebControls.TextBox txtRole;
        //protected System.Web.UI.WebControls.DropDownList ddlSubCompany;
        //protected System.Web.UI.WebControls.TextBox txtUserCode;
        //protected System.Web.UI.WebControls.Label lblMessage;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdGridFlag;
        //protected System.Web.UI.WebControls.DropDownList ddlGroup;
        //#endregion

        #region User Defined Variable
        private Users objUser = new Users();
        DataTable objDT = new DataTable();
        DataRow objDR = null;
        DataRow objDR1 = null;
        DataRow objDR2 = null;
        DataTable objDT1 = new DataTable();
        DataTable objDT2 = new DataTable();
        DataTable dtVendorClass = new DataTable();
        protected int userID = 0;

        EncryptJKS objEncrypt = new EncryptJKS();



        //protected System.Web.UI.HtmlControls.HtmlGenericControl spanGroup;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnUserType;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnDeptExst;
        //protected System.Web.UI.WebControls.Label lblHeader;
        protected int iETC = 0;
        //protected System.Web.UI.WebControls.Button btnSubmit;
        //protected System.Web.UI.WebControls.Label Label2;
        //protected System.Web.UI.WebControls.DropDownList ddlChildBuyerCompany;
        //protected System.Web.UI.WebControls.ImageButton ingAddCmpany;
        //protected System.Web.UI.WebControls.DataGrid grdCompany;
        //protected System.Web.UI.WebControls.Label Label1;
        //protected System.Web.UI.WebControls.DropDownList ddlBuyerCompany;
        //protected System.Web.UI.WebControls.DropDownList ddlDepartment;
        //protected System.Web.UI.WebControls.ImageButton imgAddDept;
        //protected System.Web.UI.WebControls.DataGrid grdUser;
        //protected System.Web.UI.HtmlControls.HtmlGenericControl spanDepartment;
        //protected System.Web.UI.WebControls.Label Label3;
        //protected System.Web.UI.HtmlControls.HtmlGenericControl Span1;
        //protected System.Web.UI.WebControls.DropDownList ddlChildBuyerCompany2;
        //protected System.Web.UI.WebControls.DropDownList ddlBusinessUnit;
        //protected System.Web.UI.WebControls.DataGrid grdBusinessUnit;
        //protected System.Web.UI.WebControls.ImageButton imgBusinessUnit;
        private CBSolutions.ETH.Web.ETC.downloadDB.downloadDB objdownloadDB = new CBSolutions.ETH.Web.ETC.downloadDB.downloadDB();
        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {

            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);
            Session["SelectedPage"] = "UserBrowse";
            if (Request.QueryString["UserID"] != null)
            {
                userID = System.Convert.ToInt32(Request.QueryString["UserID"]);
                Session.Add("SelectedUserID", userID);
                ViewState["UserID"] = userID;
            }
            else if (Session["SelectedUserID"] != null)
                userID = (int)Session["SelectedUserID"];
            else
                userID = 0;


            if (!this.IsPostBack)
            {
                btnSubmit.Attributes.Add("onclick", "javascript:return ValidateFormSubmission();");

                lblMessage.Text = "";
                lblMessage.Visible = false;
                ddlDepartment.Items.Insert(0, new ListItem("Department", "0"));
                imgBusinessUnit.Attributes.Add("onclick", "javascript:return Validation1();");
                ingAddCmpany.Attributes.Add("onclick", "javascript:return Validation2();");
                if (Convert.ToInt32(Session["UserTypeID"]) == 2)
                    btnSubmit.Attributes.Add("onclick", "javascript:if(ValidatePassword()){return ValidAPUserCode();}else{ return false;}");
                else
                    btnSubmit.Attributes.Add("onclick", "javascript:return ValidatePassword();");

                imgAddDept.Attributes.Add("onclick", "javascript:return CheckCompany();");
                LoadData();
                makeCart();
                makeCart1();
                makeCart2();
                makeVendorClass();
                GetSubCompanyNames((int)Session["CompanyID"]);
                // Added by Mrinal on 3rd March 2015
                PopulateVendorClass();
                //
                GEtBusinessUnit();
                if (Request.QueryString["UserID"] != null)
                {
                    PopulateData();
                    getDetails(0, Convert.ToInt32(userID), 0, 3);
                    getDetailsBusinessUnit(0, Convert.ToInt32(userID), 0, 3);
                    getDetailsDepartment(0, Convert.ToInt32(userID), 0, 3);
                    getUserVendorClassRelationDetails(Convert.ToInt32(userID), "", 3);
                }
                else
                {   //Added by kuntalkarar on 27thMay2016
                    txtUserName.Text = " ";
                    //--------------------------------------------
                    txtUserName.Text = txtUserName.Text.Trim();  //Added by Koushik Das on 14thSep2016
                }
                if (Convert.ToInt32(Session["ETC"]) == 1)
                {
                    ViewState["ETC"] = "1";
                    iETC = 1;
                }
                else
                    ViewState["ETC"] = "0";
            }
            if (Request.QueryString["UserID"] != null)
                lblHeader.Text = "Edit User";
            else
                lblHeader.Text = "Add a new User";

            if (grdUser.Items.Count > 0)
                hdnDeptExst.Value = "1";
            else
                hdnDeptExst.Value = "0";

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
            //this.ingAddCmpany.Click += new System.Web.UI.ImageClickEventHandler(this.ingAddCmpany_Click);
            this.ingAddCmpany.Click += new EventHandler(ingAddCmpany_Click);
            this.grdCompany.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdCompany_DeleteCommand);
            this.ddlBuyerCompany.SelectedIndexChanged += new System.EventHandler(this.ddlBuyerCompany_SelectedIndexChanged);
            //this.imgAddDept.Click += new System.Web.UI.ImageClickEventHandler(this.imgAddDept_Click);
            this.imgAddDept.Click += new EventHandler(imgAddDept_Click);
            this.grdUser.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdUser_DeleteCommand);
            this.ddlChildBuyerCompany2.SelectedIndexChanged += new System.EventHandler(this.ddlChildBuyerCompany2_SelectedIndexChanged);
            //this.imgBusinessUnit.Click += new System.Web.UI.ImageClickEventHandler(this.imgBusinessUnit_Click);
            this.imgBusinessUnit.Click += new EventHandler(imgBusinessUnit_Click);
            this.grdBusinessUnit.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdBusinessUnit_DeleteCommand);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);
            // Added by Mrinal on 3rd March 2015
            this.grdVendorClass.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(grdVendorClass_DeleteCommand);
            this.btnAddVendorClass.Click += new EventHandler(btnAddVendorClass_Click);
        }


        #endregion


        #region PopulateDepartments
        private void PopulateDepartments()
        {
            DataSet ds = new DataSet();
            try
            {
                ddlDepartment.Items.Clear();
                Users objUsers = new Users();
                string Fields = "DepartmentID,Department";
                string Table = "Department";
                string Criteria = "";
                if (ddlBuyerCompany.SelectedIndex > 0)
                {
                    Criteria = "BuyerCompanyID = " + Convert.ToInt32(ddlBuyerCompany.SelectedValue);
                    // Added by Mrinal on 26th Feb 2014
                    Criteria += " Order By Department";
                    ds = objUsers.GetGlobalDropDowns(Fields, Table, Criteria);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlDepartment.DataSource = ds;
                        ddlDepartment.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrMsg = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                ddlDepartment.Items.Insert(0, new ListItem("Department", "0"));
            }
        }
        #endregion

        #region PopulateGroup
        private void PopulateGroup()
        {
            ddlGroup.Items.Clear();
            DataSet ds = new DataSet();
            try
            {
                JKS.Users oUser = new JKS.Users();
                string Fields = "UserGroupID,UserGroupName";
                string Table = "UserGroup";
                string Criteria = "companyID = " + System.Convert.ToInt32(Session["CompanyID"]);
                ds = oUser.GetGlobalDropDowns(Fields, Table, Criteria);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlGroup.DataSource = ds;
                    ddlGroup.DataBind();
                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                ddlGroup.Items.Insert(0, new ListItem("Select Group", "0"));
            }
        }
        #endregion

        #region imgAddDept_Click

        private void imgAddDept_Click(object sender, EventArgs e)
        {
            if (ddlDepartment.SelectedIndex != 0)
            {
                objDT = (DataTable)ViewState["Cart"];
                string Department = ddlDepartment.SelectedItem.Text;
                objDR = objDT.NewRow();
                objDR["Department"] = ddlDepartment.SelectedItem.Text;
                objDR["DepartmentID"] = ddlDepartment.SelectedValue.Trim();
                objDR["CompanyName"] = ddlBuyerCompany.SelectedItem.Text.Trim();
                objDT.Rows.Add(objDR);
                ViewState["Cart"] = objDT;
                grdUser.DataSource = objDT;
                grdUser.DataBind();
                hdGridFlag.Value = "1";
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please select a department.";
                return;
            }
        }
        //private void imgAddDept_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    if(ddlDepartment.SelectedIndex != 0)
        //    {
        //        objDT = (DataTable)ViewState["Cart"];
        //        string Department = ddlDepartment.SelectedItem.Text;
        //        objDR = objDT.NewRow();
        //        objDR["Department"] = ddlDepartment.SelectedItem.Text;
        //        objDR["DepartmentID"] = ddlDepartment.SelectedValue.Trim();
        //        objDR["CompanyName"] = ddlBuyerCompany.SelectedItem.Text.Trim();
        //        objDT.Rows.Add(objDR);
        //        ViewState["Cart"] = objDT;					
        //        grdUser.DataSource = objDT;
        //        grdUser.DataBind();
        //        hdGridFlag.Value = "1";
        //    }
        //    else
        //    {
        //        lblMessage.Visible = true;
        //        lblMessage.Text = "Please select a department.";
        //        return;
        //    }			
        //}
        #endregion
        #region LoadData
        private void LoadData()
        {
            RecordSet rs = null;
            try
            {
                if (Session["SelectedCompanyTypeID"] != null)
                    rs = Users.GetRolesList((int)Session["SelectedCompanyTypeID"]);
                else
                    rs = Users.GetRolesList((int)Session["CompanyTypeID"]);

                while (!rs.EOF())
                {
                    ListItem listItem = new ListItem(rs["Role"].ToString(), rs["RoleID"].ToString());
                    cboRole.Items.Add(listItem);
                    rs.MoveNext();
                }


            }
            catch (Exception ex)
            {
                string Err = ex.Message.ToString();
            }
            PopulateDropDown();
            GetBuyerCompanyListDropDown();
            PopulateGroup();
        }
        #endregion
        #region PopulateDropDown
        public void PopulateDropDown()
        {
            UserTypeDropDown.Items.Clear();
            DataSet ds = new DataSet();
            try
            {
                Users objUsers = new Users();
                ds = objUsers.GetUserType(Convert.ToInt32(Session["CompanyID"]));
                if (ds != null)
                {
                    UserTypeDropDown.DataSource = ds;
                    UserTypeDropDown.DataBind();

                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                UserTypeDropDown.Items.Insert(0, new ListItem("Select User Type", "0"));
            }
        }
        #endregion
        #region PopulateData
        private void PopulateData()
        {
            try
            {
                RecordSet rs = Users.GetUserData(userID);
                cboRole.SelectedItem.Value = rs["RoleID"].ToString();
                txtFirstName.Text = rs["FirstName"].ToString();
                txtSurname.Text = rs["LastName"].ToString();
                txtUserName.Text = rs["UserName"].ToString();
                //txtPassword.Attributes["Value"] = rs["UserPassword"].ToString();
                string strPassword = "";
                //ENCRYPTION modified  by kuntalkarar on 31stMay2016
                if (Convert.ToString(rs["EncryptPassword"]) != "")
                    strPassword = objEncrypt.RijndaelDecryption(Convert.ToString(rs["EncryptPassword"]));
                //Encrypt.DecryptString(Convert.ToString(rs["EncryptPassword"])).ToString();
                else
                    strPassword = Convert.ToString(rs["UserPassword"]);

                txtPassword.Attributes["Value"] = strPassword.ToString();

                txtTelephone.Text = rs["Telephone"].ToString();
                txtRole.Text = rs["Designation"].ToString();
                txtMobile.Text = rs["Mobile"].ToString();
                txtEmail.Text = rs["EMail"].ToString();
                UserTypeDropDown.SelectedValue = rs["UserTypeId"].ToString();
                //Added by Mrinal on 4th Feb 2015
                string strEditInvoice = rs["EditInvoice"].ToString();
                if (Convert.ToBoolean(strEditInvoice))
                {
                    drpEditInvRightDropDown.SelectedValue = "1";
                }
                else
                {
                    drpEditInvRightDropDown.SelectedValue = "0";
                }

                if (Convert.ToInt32(rs["UserTypeId"]) == 1)
                    if (rs["New_UserGroup"].ToString().ToUpper() == "SELECT GROUP")
                    {
                        ddlGroup.SelectedIndex = -1;
                    }
                    else
                    {
                        ddlGroup.SelectedIndex = -1;
                        string sVal = Convert.ToString(rs["New_UserGroup"]).Trim();
                        if (sVal.ToUpper() != "NULL" && sVal.Trim() != "")
                            ddlGroup.Items.FindByValue(sVal).Selected = true;
                    }

                txtUserCode.Text = rs["New_UserCode"].ToString();
                if (Convert.ToInt32(Session["ETC"]) == 1)
                {
                    try
                    {
                        ddlSubCompany.SelectedValue = rs["CompanyID"].ToString();

                    }
                    catch { }
                }

                string iUserTypeID = rs["UserTypeId"].ToString();
                if (iUserTypeID == "1")
                {
                    hdnUserType.Value = "1";
                    spanDepartment.Visible = true;
                    spanGroup.Visible = true;
                }
                else
                {
                    hdnUserType.Value = "0";
                    spanDepartment.Visible = false;
                    spanGroup.Visible = false;
                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message.ToString();
            }
            finally
            {

            }

        }
        #endregion
        #region void GetSubCompanyNames(int iParentCompanyID)
        private void GetSubCompanyNames(int iParentCompanyID)
        {
            ddlChildBuyerCompany.Items.Clear();
            ddlChildBuyerCompany2.Items.Clear();
            ddlSubCompany.Items.Clear();
            DataTable dt = new DataTable();
            try
            {
                Users objUsers = new Users();
                dt = objUsers.GetSubCompanyNames(iParentCompanyID);
                if (dt.Rows.Count > 0)
                {
                    ddlSubCompany.DataSource = dt;
                    ddlSubCompany.DataBind();

                    ddlChildBuyerCompany.DataSource = dt;
                    ddlChildBuyerCompany.DataBind();

                    ddlChildBuyerCompany2.DataSource = dt;
                    ddlChildBuyerCompany2.DataBind();
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                dt = null;
                ddlChildBuyerCompany.Items.Insert(0, new ListItem("Buyer Company", "0"));
                ddlChildBuyerCompany2.Items.Insert(0, new ListItem("Buyer Company", "0"));
                ddlSubCompany.Items.Insert(0, new ListItem("Select Company", "0"));
            }
        }
        #endregion

        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)//added by kuntalkarar on 1stJune2016
            {
                //  if( Convert.ToInt32(ViewState["UserID"]) == 0)
                #region : Existing Code


                try
                {
                    Users users = new Users();
                    DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);

                    RecordSet rs = null;
                    int iCCompanyID = 0;

                    if (Session["SelectedCompanyID"] != null)
                        iCCompanyID = Convert.ToInt32(Session["SelectedCompanyID"]);
                    else
                        iCCompanyID = Convert.ToInt32(Session["CompanyID"]);

                    if (Convert.ToInt32(ViewState["UserID"]) == 0)
                    {
                        if (objUser.CheckDuplicateUserName(Convert.ToInt32(ViewState["UserID"]), txtUserName.Text.Trim(), iCCompanyID, txtUserCode.Text.Trim()))
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "Sorry, user name / user code already exists for this company.";
                            return;
                        }
                    }

                    if (Convert.ToInt32(ViewState["UserID"]) == 0)
                    {
                        rs = da.CreateInsertBuffer("Users");
                        if (Session["SelectedCompanyID"] != null)
                            rs["CompanyID"] = Convert.ToInt32(Session["SelectedCompanyID"]);
                        else
                            rs["CompanyID"] = Convert.ToInt32(Session["CompanyID"]);
                    }
                    else
                    {
                        rs = Users.GetUserData(userID);
                    }

                    rs["RoleID"] = System.Convert.ToInt32(cboRole.SelectedItem.Value);
                    rs["FirstName"] = txtFirstName.Text;
                    rs["LastName"] = txtSurname.Text;
                    rs["UserName"] = txtUserName.Text;
                    //rs["UserPassword"]= txtPassword.Text.Trim();

                    //blocked by kuntalkarar on 28thMay2016
                    //string EncryptPwd = Encrypt.EncryptData(txtPassword.Text.Trim());
                    //added by kuntalkarar on 28thMay2016
                    string EncryptPwd = objEncrypt.RijndaelEncription(txtPassword.Text.Trim());


                    rs["EncryptPassword"] = EncryptPwd.ToString();

                    rs["Designation"] = txtRole.Text;
                    rs["Telephone"] = txtTelephone.Text;
                    rs["Mobile"] = txtMobile.Text;
                    rs["Email"] = txtEmail.Text;
                    rs["UserDeleted"] = 0;
                    rs["ModUserId"] = Session["UserID"].ToString();
                    rs["UserTypeId"] = UserTypeDropDown.SelectedItem.Value;
                    rs["New_UserGroup"] = (ddlGroup.SelectedItem.Text.Trim() == "Select Group" ? "NULL" : ddlGroup.SelectedItem.Text.Trim());
                    rs["New_UserCode"] = txtUserCode.Text;
                    rs["EditInvoice"] = Convert.ToInt32(drpEditInvRightDropDown.SelectedValue);



                    if (Convert.ToInt32(ViewState["UserID"]) == 0)
                    {
                        da.BeginTransaction();
                        int iUserID = 0;
                        iUserID = users.InsertUserData(rs);
                        if (iUserID != 0)
                        {
                            int iCounter = 0;
                            int iValueInsert = 0;
                            foreach (DataGridItem dgItem in grdUser.Items)
                            {
                                if (iCounter <= Convert.ToInt32(grdUser.Items.Count))
                                {
                                    iValueInsert = Convert.ToInt32(((Label)grdUser.Items[iCounter].FindControl("lblDepartmentID")).Text);
                                    getDetailsDepartment(0, iUserID, Convert.ToInt32(iValueInsert), 1);
                                }
                                iCounter++;
                            }
                            /***********************************/
                            iCounter = 0;
                            iValueInsert = 0;
                            foreach (DataGridItem dgItem in grdCompany.Items)
                            {
                                if (iCounter <= Convert.ToInt32(grdCompany.Items.Count))
                                {

                                    iValueInsert = Convert.ToInt32(((Label)grdCompany.Items[iCounter].FindControl("lblUserCompanyID")).Text);
                                    getDetails(0, iUserID, Convert.ToInt32(iValueInsert), 1);
                                }
                                iCounter++;
                            }
                            /***********************************/
                            iCounter = 0;
                            iValueInsert = 0;
                            foreach (DataGridItem dgItem in grdBusinessUnit.Items)
                            {
                                if (iCounter <= Convert.ToInt32(grdBusinessUnit.Items.Count))
                                {

                                    iValueInsert = Convert.ToInt32(((Label)grdBusinessUnit.Items[iCounter].FindControl("lblBusinessUnitID")).Text);
                                    getDetailsBusinessUnit(0, iUserID, Convert.ToInt32(iValueInsert), 1);
                                }
                                iCounter++;
                            }

                            /***********************************/
                            iCounter = 0;
                            // iValueInsert = 0;
                            string strVendorClass = string.Empty;
                            foreach (DataGridItem dgItem in grdVendorClass.Items)
                            {
                                if (iCounter <= Convert.ToInt32(grdVendorClass.Items.Count))
                                {

                                    strVendorClass = Convert.ToString(((Label)grdVendorClass.Items[iCounter].FindControl("lblVendorClass")).Text);
                                    getUserVendorClassRelationDetails(iUserID, Convert.ToString(strVendorClass), 1);
                                }
                                iCounter++;
                            }
                            /***********************************/


                        }
                        da.CommitTransaction();

                        if (iUserID == 0)
                        {
                            Response.Write(users.ErrorMessage);
                        }
                        else
                        {
                            UpdateUserEditInvoiceRights(iUserID);
                            int retval = Encrypt.UpdateEncrypt(iUserID, Convert.ToInt32(Session["CompanyID"]), EncryptPwd);
                            userID = System.Convert.ToInt32(iUserID);
                            Session.Add("SelectedUserID", userID);
                            ViewState["UserID"] = userID;
                            try
                            {

                                lblMessage.Visible = true;
                                lblMessage.Text = "Record(s) saved successfully.";
                                Response.Write("<script>alert('Record(s) saved successfully.');top.location.replace('UserBrowse.aspx');</script>");
                            }
                            catch
                            {
                                lblMessage.Visible = true;
                                lblMessage.Text = "Record(s) saved successfully. Error sending login details.";
                                Response.Write("<script>alert('Record(s) saved successfully. Error sending login details.');top.location.replace('UserBrowse.aspx');</script>");
                            }
                        }
                    }
                    else
                    {
                        //Edit mode
                        if (!users.UpdateUserData(rs))
                            Response.Write(users.ErrorMessage);
                        else
                        {
                            UpdateUserEditInvoiceRights(Convert.ToInt32(ViewState["UserID"]));
                            int retval = Encrypt.UpdateEncrypt(Convert.ToInt32(ViewState["UserID"]), Convert.ToInt32(Session["CompanyID"]), EncryptPwd);
                            if (retval < 1)
                            {
                                Page.RegisterStartupScript("reg", "<script>alert('Failed to Update Encrypted Password.');</script>");
                            }
                            if (Convert.ToInt32(ViewState["UserID"]) != 0)
                            {
                                /***********************************/
                                int iCounter = 0;
                                int iValueInsert = 0;
                                getDetailsDepartment(0, Convert.ToInt32(ViewState["UserID"]), 0, 2);
                                foreach (DataGridItem dgItem in grdUser.Items)
                                {
                                    if (iCounter <= Convert.ToInt32(grdUser.Items.Count))
                                    {

                                        iValueInsert = Convert.ToInt32(((Label)grdUser.Items[iCounter].FindControl("lblDepartmentID")).Text);
                                        getDetailsDepartment(0, userID, Convert.ToInt32(iValueInsert), 1);
                                    }
                                    iCounter++;
                                }
                                /***********************************/
                                iCounter = 0;
                                iValueInsert = 0;
                                getDetails(0, Convert.ToInt32(ViewState["UserID"]), 0, 2);
                                foreach (DataGridItem dgItem in grdCompany.Items)
                                {
                                    if (iCounter <= Convert.ToInt32(grdCompany.Items.Count))
                                    {

                                        iValueInsert = Convert.ToInt32(((Label)grdCompany.Items[iCounter].FindControl("lblUserCompanyID")).Text);
                                        getDetails(0, Convert.ToInt32(ViewState["UserID"]), Convert.ToInt32(iValueInsert), 1);
                                    }
                                    iCounter++;
                                }
                                /***********************************/
                                iCounter = 0;
                                iValueInsert = 0;
                                getDetailsBusinessUnit(0, Convert.ToInt32(ViewState["UserID"]), 0, 2);
                                foreach (DataGridItem dgItem in grdBusinessUnit.Items)
                                {
                                    if (iCounter <= Convert.ToInt32(grdBusinessUnit.Items.Count))
                                    {

                                        iValueInsert = Convert.ToInt32(((Label)grdBusinessUnit.Items[iCounter].FindControl("lblBusinessUnitID")).Text);
                                        getDetailsBusinessUnit(0, Convert.ToInt32(ViewState["UserID"]), Convert.ToInt32(iValueInsert), 1);
                                    }
                                    iCounter++;
                                }

                                /***********************************/
                                iCounter = 0;

                                string strVendorClass = string.Empty;
                                getUserVendorClassRelationDetails(Convert.ToInt32(ViewState["UserID"]), Convert.ToString(strVendorClass), 2);
                                foreach (DataGridItem dgItem in grdVendorClass.Items)
                                {
                                    if (iCounter <= Convert.ToInt32(grdVendorClass.Items.Count))
                                    {

                                        strVendorClass = Convert.ToString(((Label)grdVendorClass.Items[iCounter].FindControl("lblVendorClass")).Text);
                                        getUserVendorClassRelationDetails(Convert.ToInt32(ViewState["UserID"]), Convert.ToString(strVendorClass), 1);
                                    }
                                    iCounter++;
                                }
                                /***********************************/
                            }
                            //added by kuntalkarar on 30thMay2016
                            SqlCommand sqlCmd = null;
                            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                            sqlCmd = new SqlCommand("usp_resetLockout_JKS", sqlConn);
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.Add("@UserID", Convert.ToInt32(ViewState["UserID"].ToString()));
                            sqlConn.Open();
                            sqlCmd.ExecuteNonQuery();
                            //addition ends by kuntalkarar on 30thMay2016

                            Session.Remove("SelectedUserID");
                            lblMessage.Visible = true;
                            lblMessage.Text = "Record(s) updated successfully.";
                            getDetails(0, Convert.ToInt32(ViewState["UserID"]), 0, 3);
                            getDetailsBusinessUnit(0, Convert.ToInt32(ViewState["UserID"]), 0, 3);
                            getDetailsDepartment(0, Convert.ToInt32(ViewState["UserID"]), 0, 3);
                            getUserVendorClassRelationDetails(Convert.ToInt32(ViewState["UserID"]), "", 3);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Err = ex.Message.ToString();
                }
                finally
                {
                }
                #endregion
            }
        }
        #endregion



        #region GetBuyerCompanyListDropDown
        private void GetBuyerCompanyListDropDown()
        {
            DataTable dtbl = null;
            ddlBuyerCompany.Items.Clear();
            try
            {
                dtbl = objdownloadDB.GetBuyerCompanyListDropDown(Convert.ToInt32(Session["CompanyID"]));
                if (dtbl.Rows.Count > 0)
                {
                    ddlBuyerCompany.DataSource = dtbl;
                    ddlBuyerCompany.DataBind();
                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message.ToString();
            }
            finally
            {
                dtbl = null;
                ddlBuyerCompany.Items.Insert(0, new ListItem("Buyer Company", "0"));
            }
        }
        #endregion

        #region void ddlBuyerCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        private void ddlBuyerCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            PopulateDepartments();
        }
        #endregion

        private void ingAddCmpany_Click(object sender, EventArgs e)
        {
            if (ddlChildBuyerCompany.SelectedIndex != 0)
            {
                objDT1 = (DataTable)ViewState["Cart1"];
                string Department = ddlDepartment.SelectedItem.Text;
                objDR = objDT1.NewRow();
                objDR["CompanyID"] = ddlChildBuyerCompany.SelectedValue.Trim();
                objDR["CompanyName"] = ddlChildBuyerCompany.SelectedItem.Text.Trim();
                objDT1.Rows.Add(objDR);
                ViewState["Cart1"] = objDT1;
                grdCompany.DataSource = objDT1;
                grdCompany.DataBind();
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please select a department.";
                return;
            }
        }

        private void getDetails(int UserCompanyID, int userID1, int CompanyID, int type)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_UserCompanyRelation", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@UserCompanyID", Convert.ToInt32(UserCompanyID));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(userID1));
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(CompanyID));
            sqlDA.SelectCommand.Parameters.Add("@type", Convert.ToInt32(type));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(objDT1);
                if (type == 3)
                {
                    if (objDT1.Rows.Count > 0)
                    {
                        ViewState["Cart1"] = objDT1;
                        grdCompany.DataSource = objDT1;
                        grdCompany.DataBind();
                    }
                    else
                    {
                        grdCompany.DataSource = null;
                        grdCompany.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }

        private void grdCompany_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (userID == 0)
            {
                objDT1 = (DataTable)ViewState["Cart1"];
                if (objDT1.Rows.Count != 0)
                {
                    objDT1.Rows[e.Item.ItemIndex].Delete();
                }
                ViewState["Cart1"] = objDT1;
                grdCompany.DataSource = objDT1;
                grdCompany.DataBind();
            }
            else
            {
                objDT1 = (DataTable)ViewState["Cart1"];
                DataRow dro = objDT1.Rows[e.Item.ItemIndex];
                objDT1.Rows.Remove(dro);
                //				if(grdCompany.Items.Count != 0)
                //				{
                //					int x=objDT1.Rows.Count;	
                //					objDT1=null;
                //					objDT1=Repopulate1(e.Item.ItemIndex);
                //				}
                //				int i = objDT1.Rows.Count;
                ViewState["Cart1"] = objDT1;
                grdCompany.DataSource = objDT1;
                grdCompany.DataBind();
            }

        }

        private void ddlChildBuyerCompany2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GEtBusinessUnit();
        }
        private void GEtBusinessUnit()
        {
            ddlBusinessUnit.Items.Clear();
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetBusinessUnit", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlChildBuyerCompany2.SelectedValue));
            sqlDA.SelectCommand.Parameters.Add("@UsrID", Convert.ToInt32(0));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(3));
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
                string ss = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                sqlDA.Dispose();
                sqlConn.Close();
                ddlBusinessUnit.Items.Insert(0, new ListItem("Business Unit", "0"));
            }

        }



        private void imgBusinessUnit_Click(object sender, EventArgs e)
        {
            if (ddlBusinessUnit.SelectedIndex != 0)
            {
                objDT2 = (DataTable)ViewState["Cart2"];
                string BusinessUnit = ddlBusinessUnit.SelectedItem.Text;

                objDR = objDT2.NewRow();
                objDR["BusinessUnitID"] = ddlBusinessUnit.SelectedValue.Trim();
                objDR["BusinessUnitName"] = ddlBusinessUnit.SelectedItem.Text;
                objDR["CompanyName"] = ddlChildBuyerCompany2.SelectedItem.Text.Trim();

                objDT2.Rows.Add(objDR);
                ViewState["Cart2"] = objDT2;

                grdBusinessUnit.DataSource = objDT2;
                grdBusinessUnit.DataBind();
                //hdGridFlag.Value = "1";
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please select a BusinessUnit.";
                return;
            }
            //getDetailsBusinessUnit(0,userID,Convert.ToInt32(ddlBusinessUnit.SelectedValue),1);
        }



        private void getDetailsBusinessUnit(int UserBusinessUnitID, int userID1, int BusinessUnitID, int type)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_UserBusinessUnitRelation", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@UserBusinessUnitID", Convert.ToInt32(UserBusinessUnitID));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(userID1));
            sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", Convert.ToInt32(BusinessUnitID));
            sqlDA.SelectCommand.Parameters.Add("@type", Convert.ToInt32(type));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(objDT2);
                if (type == 3)
                {
                    if (objDT2.Rows.Count > 0)
                    {
                        ViewState["Cart2"] = objDT2;
                        grdBusinessUnit.DataSource = objDT2;
                        grdBusinessUnit.DataBind();
                    }
                    else
                    {
                        grdBusinessUnit.DataSource = null;
                        grdBusinessUnit.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                sqlDA.Dispose();
                sqlConn.Close();
            }

        }

        private void grdBusinessUnit_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (userID == 0)
            {
                objDT2 = (DataTable)ViewState["Cart2"];
                if (objDT2.Rows.Count != 0)
                {
                    objDT2.Rows[e.Item.ItemIndex].Delete();

                }
                ViewState["Cart2"] = objDT2;
                grdBusinessUnit.DataSource = objDT2;
                grdBusinessUnit.DataBind();
            }
            else
            {
                objDT2 = (DataTable)ViewState["Cart2"];
                DataRow dro = objDT2.Rows[e.Item.ItemIndex];
                objDT2.Rows.Remove(dro);
                //				if(grdUser.Items.Count != 0)
                //				{
                //					int x=objDT2.Rows.Count;	
                //					objDT2=null;
                //					objDT2=Repopulate2(e.Item.ItemIndex);
                //				}
                //				int i = objDT2.Rows.Count;
                ViewState["Cart2"] = objDT2;
                grdBusinessUnit.DataSource = objDT2;
                grdBusinessUnit.DataBind();
            }
        }

        private void grdUser_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            if (userID == 0)
            {
                objDT = (DataTable)ViewState["Cart"];
                if (objDT.Rows.Count != 0)
                {
                    objDT.Rows[e.Item.ItemIndex].Delete();

                }
                ViewState["Cart"] = objDT;
                grdUser.DataSource = objDT;
                grdUser.DataBind();
            }
            else
            {
                objDT = (DataTable)ViewState["Cart"];
                DataRow dro = objDT.Rows[e.Item.ItemIndex];
                objDT.Rows.Remove(dro);
                //				if(grdUser.Items.Count != 0)
                //				{
                //					int x=objDT.Rows.Count;	
                //					objDT=null;
                //					objDT=Repopulate(e.Item.ItemIndex);
                //				}
                //				int i = objDT.Rows.Count;
                ViewState["Cart"] = objDT;
                grdUser.DataSource = objDT;
                grdUser.DataBind();
            }
        }

        private void getDetailsDepartment(int UserDeptID, int UserID1, int DepartmentID, int type)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_UserDeptRelation_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@UserDeptID", Convert.ToInt32(UserDeptID));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(UserID1));
            sqlDA.SelectCommand.Parameters.Add("@DepartmentID", Convert.ToInt32(DepartmentID));
            sqlDA.SelectCommand.Parameters.Add("@type", Convert.ToInt32(type));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(objDT);
                if (type == 3)
                {
                    if (objDT.Rows.Count > 0)
                    {
                        ViewState["Cart"] = objDT;
                        grdUser.DataSource = objDT;
                        grdUser.DataBind();
                    }
                    else
                    {
                        grdUser.DataSource = null;
                        grdUser.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }


        #region makeCart
        public void makeCart()
        {
            objDT = new System.Data.DataTable("Cart");
            objDT.Columns.Add("Department", Type.GetType("System.String"));
            objDT.Columns.Add("DepartmentID", Type.GetType("System.String"));
            objDT.Columns.Add("CompanyName", Type.GetType("System.String"));

            ViewState["Cart"] = objDT;
        }
        #endregion

        #region makeCart1
        public void makeCart1()
        {
            objDT1 = new System.Data.DataTable("Cart1");
            objDT1.Columns.Add("CompanyID", Type.GetType("System.String"));
            objDT1.Columns.Add("CompanyName", Type.GetType("System.String"));

            ViewState["Cart1"] = objDT1;
        }
        #endregion

        #region makeCart2
        public void makeCart2()
        {
            objDT2 = new System.Data.DataTable("Cart2");
            objDT2.Columns.Add("BusinessUnitID", Type.GetType("System.String"));
            objDT2.Columns.Add("BusinessUnitName", Type.GetType("System.String"));
            objDT2.Columns.Add("CompanyName", Type.GetType("System.String"));

            ViewState["Cart2"] = objDT2;
        }
        #endregion
        #region DataTable Repopulate(int i)
        private DataTable Repopulate(int i)
        {
            DataTable objDataTbl = new DataTable();
            objDataTbl.Columns.Add("Department", Type.GetType("System.String"));
            objDataTbl.Columns.Add("DepartmentID", Type.GetType("System.String"));
            objDataTbl.Columns.Add("CompanyName", Type.GetType("System.String"));
            for (int y = 0; y <= grdUser.Items.Count - 1; y++)
            {
                if (grdUser.Items[y].ItemIndex != i)
                {
                    objDR = objDataTbl.NewRow();
                    objDR["Department"] = ((Label)grdUser.Items[y].FindControl("lblDepartment")).Text;
                    objDR["DepartmentID"] = ((Label)grdUser.Items[y].FindControl("lblDepartmentID")).Text;
                    objDR["CompanyName"] = ((Label)grdUser.Items[y].FindControl("lblCompany")).Text;
                    objDataTbl.Rows.Add(objDR);
                }
            }
            return objDataTbl;
        }
        #endregion

        #region DataTable Repopulate1(int i)
        private DataTable Repopulate1(int i)
        {
            DataTable objDataTbl = new DataTable();
            objDataTbl.Columns.Add("CompanyID", Type.GetType("System.String"));
            objDataTbl.Columns.Add("CompanyName", Type.GetType("System.String"));
            for (int y = 0; y <= grdCompany.Items.Count - 1; y++)
            {
                if (grdCompany.Items[y].ItemIndex != i)
                {
                    objDR1 = objDataTbl.NewRow();
                    objDR1["CompanyID"] = ((Label)grdCompany.Items[y].FindControl("lblUserCompanyID")).Text;
                    objDR1["CompanyName"] = ((Label)grdCompany.Items[y].FindControl("lblCompanyName")).Text;
                    objDataTbl.Rows.Add(objDR1);
                }
            }
            return objDataTbl;
        }
        #endregion
        #region DataTable Repopulate2(int i)
        private DataTable Repopulate2(int i)
        {
            DataTable objDataTbl = new DataTable();
            objDataTbl.Columns.Add("BusinessUnitID", Type.GetType("System.String"));
            objDataTbl.Columns.Add("BusinessUnitName", Type.GetType("System.String"));
            objDataTbl.Columns.Add("CompanyName", Type.GetType("System.String"));
            for (int y = 0; y <= grdBusinessUnit.Items.Count - 1; y++)
            {
                if (grdBusinessUnit.Items[y].ItemIndex != i)
                {
                    objDR2 = objDataTbl.NewRow();
                    objDR2["BusinessUnitID"] = ((Label)grdUser.Items[y].FindControl("lblBusinessUnitID")).Text;
                    objDR2["BusinessUnitName"] = ((Label)grdUser.Items[y].FindControl("lblBusinessUnitName")).Text;
                    objDR2["CompanyName"] = ((Label)grdUser.Items[y].FindControl("lblCompanyName2")).Text;
                    objDataTbl.Rows.Add(objDR2);
                }
            }
            return objDataTbl;
        }
        #endregion
        protected void UpdateUserEditInvoiceRights(int UserID)
        {
            try
            {

                #region: Save
                int iReturnValue = 0;

                // rs["EditInvoice"] = Convert.ToInt32(drpEditInvRightDropDown.SelectedValue);
                SqlConnection sqlConnInner = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                SqlCommand sqlCmdInner = new SqlCommand("UpdateUserEditInvoiceRights", sqlConnInner);
                sqlCmdInner.CommandType = CommandType.StoredProcedure;
                sqlCmdInner.Parameters.Add("@UserID", Convert.ToInt32(UserID));
                sqlCmdInner.Parameters.Add("@EditInvoice", Convert.ToInt32(drpEditInvRightDropDown.SelectedValue));
                SqlParameter sqlReturnParam = sqlCmdInner.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                try
                {
                    sqlConnInner.Open();
                    sqlCmdInner.ExecuteNonQuery();
                    iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
                }
                catch (Exception ex)
                {
                    string strExceptionMessage = ex.Message.Trim();
                }
                finally
                {
                    sqlReturnParam = null;
                    if (sqlCmdInner != null)
                        sqlCmdInner.Dispose();
                    if (sqlConnInner != null)
                        sqlConnInner.Close();
                }
                #endregion




            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();

            }
            finally
            {
            }
        }

        #region : Added by Mrinal on 3rd March 2015
        private void PopulateVendorClass()
        {
            int iCompanyID = 0;
            if (Session["CompanyID"] != null)
            {
                iCompanyID = (int)Session["CompanyID"];
            }
            ddlVendorClass.Items.Clear();
            if (iCompanyID != 0)
            {
                SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetVendorClass_NewVersion", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(iCompanyID));

                DataSet ds = new DataSet();
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlVendorClass.DataSource = ds;
                        ddlVendorClass.DataBind();
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
                    ddlVendorClass.Items.Insert(0, new ListItem("Vendor Class", "0"));
                }
            }

        }
        public void makeVendorClass()
        {
            dtVendorClass = new System.Data.DataTable("VendorClass");
            dtVendorClass.Columns.Add("UserID", Type.GetType("System.String"));
            dtVendorClass.Columns.Add("VendorClass", Type.GetType("System.String"));

            ViewState["VendorClass"] = dtVendorClass;
        }
        private void grdVendorClass_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            // throw new NotImplementedException();
            if (userID == 0)
            {
                dtVendorClass = (DataTable)ViewState["VendorClass"];
                if (dtVendorClass.Rows.Count != 0)
                {
                    dtVendorClass.Rows[e.Item.ItemIndex].Delete();
                }
                ViewState["VendorClass"] = dtVendorClass;
                grdVendorClass.DataSource = dtVendorClass;
                grdVendorClass.DataBind();
            }
            else
            {
                dtVendorClass = (DataTable)ViewState["VendorClass"];
                DataRow dro = dtVendorClass.Rows[e.Item.ItemIndex];
                dtVendorClass.Rows.Remove(dro);
                ViewState["VendorClass"] = dtVendorClass;
                grdVendorClass.DataSource = dtVendorClass;
                grdVendorClass.DataBind();
            }

        }

        private void btnAddVendorClass_Click(object sender, EventArgs e)
        {
            if (ddlVendorClass.SelectedIndex != 0)
            {
                dtVendorClass = (DataTable)ViewState["VendorClass"];
                objDR = dtVendorClass.NewRow();
                objDR["UserID"] = userID;
                objDR["VendorClass"] = ddlVendorClass.SelectedItem.Text.Trim();
                dtVendorClass.Rows.Add(objDR);
                ViewState["VendorClass"] = dtVendorClass;
                grdVendorClass.DataSource = dtVendorClass;
                grdVendorClass.DataBind();
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please select a Vendor Class.";
                return;
            }
        }
        private void getUserVendorClassRelationDetails(int iuserID, string VendorClass, int type)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Fetch_UserVendorClassRelation", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(iuserID));
            sqlDA.SelectCommand.Parameters.Add("@VendorClass", Convert.ToString(VendorClass));
            sqlDA.SelectCommand.Parameters.Add("@type", Convert.ToInt32(type));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(dtVendorClass);
                if (type == 3)
                {
                    if (dtVendorClass.Rows.Count > 0)
                    {
                        ViewState["VendorClass"] = dtVendorClass;
                        grdVendorClass.DataSource = dtVendorClass;
                        grdVendorClass.DataBind();
                    }
                    else
                    {
                        grdVendorClass.DataSource = null;
                        grdVendorClass.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }


        #endregion
    }
}

