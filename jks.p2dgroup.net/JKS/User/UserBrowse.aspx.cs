#region Directives
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

using System.Configuration;
using System.Data.SqlClient;
using CBSolutions.ETH.Web;
using System.Web.Services;
#endregion

namespace JKS
{
    /// <summary>
    /// Summary description for UserBrowse.
    /// </summary>
    public partial class UserBrowse : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        //protected System.Web.UI.WebControls.HyperLink HyperLink1;
        //protected System.Web.UI.WebControls.Panel Panel3;
        //protected System.Web.UI.WebControls.DataGrid grdUser;
        //protected System.Web.UI.WebControls.Button btnSubmit;
        //protected System.Web.UI.WebControls.ImageButton imgButtonSendMailInfo;
        //protected System.Web.UI.WebControls.Label lblMessage;
        #endregion
        #region User Define Variables
        private bool pageLoaded = false;
        JKS.Users objUsers = new JKS.Users();
        //protected System.Web.UI.WebControls.HyperLink HyperLink2;
        //protected System.Web.UI.WebControls.ImageButton imgChangePass;
        //protected System.Web.UI.WebControls.Button imgChangePass;
        //protected System.Web.UI.WebControls.DropDownList ddlGroup;
        //protected System.Web.UI.WebControls.Button btnSearch;
        protected System.Web.UI.WebControls.Label Label1;
        //protected System.Web.UI.WebControls.DropDownList ddlDepartment;

        //protected System.Web.UI.WebControls.DropDownList ddlCompany;
        //protected System.Web.UI.WebControls.DropDownList ddlBusinessUnit;
        ////protected System.Web.UI.WebControls.TextBox txtUserName;
        //protected System.Web.UI.WebControls.DropDownList ddlUser;



        DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {

            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            Session["SelectedPage"] = "UserBrowse";

            if (Request.QueryString["CompanyID"] != null)
                ViewState["CompanyID"] = Convert.ToInt32(Request.QueryString["CompanyID"]);
            else
                ViewState["CompanyID"] = Convert.ToInt32(Session["CompanyID"]);




            if (!IsPostBack)//don't do this if we are logging off
            {
                btnSearch.Attributes.Add("onclick", "javascript:doHourglass();");

                //GetDepartmentList();

                LoadCompanyDropDown();
                LoadDepartment(Convert.ToInt32(ddlCompany.SelectedValue));
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue));
                PopulateGroup();
                GetUserList();
                //GetUserDetails( Convert.ToInt32(ViewState["CompanyID"]));
                PopulateSearchResult();

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
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdUser.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdUser_ItemDataBound);
            //this.imgButtonSendMailInfo.Click += new System.Web.UI.ImageClickEventHandler(this.imgButtonSendMailInfo_Click);

            this.imgButtonSendMailInfo.Click += new System.EventHandler(imgButtonSendMailInfo_Click);
            this.imgChangePass.Click += new System.EventHandler(imgChangePass_Click);

            //this.imgChangePass.Click += new System.Web.UI.ImageClickEventHandler(this.imgChangePass_Click);
            this.Load += new System.EventHandler(this.Page_Load);
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);

        }


        #endregion
        #region grdUser_ItemCreated
        private void grdUser_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (pageLoaded)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HyperLink ctrl = (HyperLink)e.Item.FindControl("HyperlinkGridCol");
                    if (ctrl != null)
                    {
                        DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                        string navigateURL = "UserEdit.aspx?" + "UserID=" + dr["UserID"].ToString();
                        ctrl.NavigateUrl = navigateURL;
                    }
                }
            }
        }
        #endregion
        #region imgButtonSendMailInfo_Click

        private void imgButtonSendMailInfo_Click(object sender, EventArgs e)
        {
            int iflag = 0;
            foreach (DataGridItem dgItem in grdUser.Items)
            {
                CheckBox chk = (CheckBox)dgItem.FindControl("chkMail");
                if (chk.Checked)
                {
                    iflag = 1;
                    break;
                }
            }
            if (iflag == 1)
            {
                objUsers.SendUserNamePassword(grdUser);


                lblMessage.Text = "User login info. has been sent to selected user(s).";
            }
            else
                lblMessage.Text = "Please select an user to send user info.";
        }
        //private void imgButtonSendMailInfo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    int iflag	= 0;
        //    foreach (DataGridItem dgItem in grdUser.Items)
        //    {
        //        CheckBox chk = (CheckBox) dgItem.FindControl("chkMail");
        //        if (chk.Checked)
        //        {	
        //            iflag = 1;
        //            break;
        //        }
        //    }
        //    if(iflag == 1)
        //    {
        //        objUsers.SendUserNamePassword(grdUser);
        //        lblMessage.Text = "User login info. has been sent to selected user(s).";
        //    }
        //    else
        //        lblMessage.Text = "Please select an user to send user info.";
        //}
        #endregion
        #region Datagrid_Click
        protected void Datagrid_Click(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DELETERECORD")
            {
                if (objUsers.DeleteUserRecord_New(Convert.ToInt32(e.CommandArgument), Convert.ToInt32(Session["UserID"])))
                {
                    lblMessage.Text = "Record(s) deleted successfully.";
                    GetUserDetails(Convert.ToInt32(ViewState["CompanyID"]));
                }
                else
                {
                    lblMessage.Text = "Error deleting record(s).";
                }
            }
        }
        #endregion
        #region GetUserDetails
        private void GetUserDetails(int iCompanyID)
        {
            RecordSet rs = null;
            RecordSet rsCompany = null;
            try
            {
                rs = da.ExecuteQuery("vwUserList_NL", "CompanyID = " + iCompanyID + " AND UserDeleted = 0");
                rsCompany = Company.GetCompanyData(iCompanyID);
                Session.Add("SelectedCompanyTypeID", rsCompany["CompanyTypeID"]);
                Session.Add("SelectedCompanyID", rsCompany["CompanyID"]);
                pageLoaded = true;
                grdUser.DataSource = rs.ParentTable;
                grdUser.DataBind();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }
            finally
            {
                rs = null;
                rsCompany = null;
            }
        }
        #endregion
        #region grdUser_ItemDataBound
        private void grdUser_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                #region Blocked by Koushik Das as on 04-Apr-2017
                //if (Convert.ToInt32(Session["UserTypeID"]) == 2)
                //{
                //    string strUserGroup = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "New_UserGroup"));
                //    if (strUserGroup == "G2")
                //        ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:alert('Sorry, only Admin users can edit G2 approvers'); return false;");
                //    else
                //        ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirm('Are you sure to delete this record ?')");
                //}
                //else
                //{
                //    ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirm('Are you sure to delete this record ?')");
                //}
                #endregion

                #region Added by Koushik Das as on 04-Apr-2017
                string userid = grdUser.DataKeys[e.Item.ItemIndex].ToString();
                JKS.Users objUsers = new JKS.Users();

                int iUserID = Convert.ToInt32(userid);
                int iParentCompanyID = (Int32)HttpContext.Current.Session["CompanyID"];
                int UID = objUsers.CheckIfUserInUse(iUserID, iParentCompanyID);

                if (Convert.ToInt32(Session["UserTypeID"]) == 2)
                {
                    string strUserGroup = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "New_UserGroup"));

                    if (strUserGroup == "G2")
                        ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:alert('Sorry, only Admin users can edit G2 approvers'); return false;");
                    else
                    {
                        if (UID > 0)
                            ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirmDelete();");
                        else
                            ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirm('Are you sure to delete this record ?')");
                    }
                }
                else
                {
                    if (UID > 0)
                        ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirmDelete();");
                    else
                        ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirm('Are you sure to delete this record ?')");
                }
                #endregion
            }
        }
        #endregion
        #region imgChangePass_Click
        private void imgChangePass_Click(object sender, EventArgs e)
        {
            int iflag = 0;
            foreach (DataGridItem dgItem in grdUser.Items)
            {
                CheckBox chk = (CheckBox)dgItem.FindControl("chkChangePassword");

                if (chk.Checked)
                {
                    iflag = 1;
                    break;
                }
            }
            if (iflag == 1)
            {
                //string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                //SqlConnection sqlConn = new SqlConnection(ConsString);
                //sqlConn.Open();
                //SqlCommand sqlCmd = new SqlCommand("SELECT isnull(lockout,0)  FROM Users INNER JOIN Company  ON Company.CompanyID = Users.CompanyID WHERE Users.UserName='" + txtUserName.Text + "'  AND Company.NetworkID ='" + txtNetworkID.Text + "'  AND Users.UserDeleted = 0", sqlConn);
                //iLogStatusCount = Convert.ToInt32(sqlCmd.ExecuteScalar());
                objUsers.PasswordChangeRequired(grdUser);


                lblMessage.Text = "Password change-required is applied to selected user(s).";
            }
            else
                lblMessage.Text = "Please select a user";
        }
        //private void imgChangePass_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    int iflag	= 0;
        //    foreach (DataGridItem dgItem in grdUser.Items)
        //    {
        //        CheckBox chk = (CheckBox) dgItem.FindControl("chkChangePassword");

        //        if (chk.Checked)
        //        {	
        //            iflag = 1;
        //            break;
        //        }
        //    }
        //    if(iflag == 1)
        //    {
        //        objUsers.PasswordChangeRequired(grdUser);
        //        lblMessage.Text = "Password change-required is applied to selected user(s).";
        //    }
        //    else
        //        lblMessage.Text = "Please select an user";
        //}	
        #endregion
        #region GetDepartmentList
        private void GetDepartmentList()
        {
            JKS.Users oUser = new JKS.Users();
            DataSet ds = new DataSet();
            try
            {
                string Fields = "DepartmentID,Department";
                string Table = "Department";
                string Criteria = "BuyerCompanyID = " + System.Convert.ToInt32(Session["CompanyID"]);
                ds = oUser.GetGlobalDropDowns(Fields, Table, Criteria);
                ddlDepartment.DataSource = ds;
                ddlDepartment.DataBind();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }
            finally
            {
                ddlDepartment.Items.Insert(0, "Select");
                ds = null;
            }
        }
        #endregion
        #region PopulateGroup
        private void PopulateGroup()
        {
            JKS.Users oUser = new JKS.Users();
            DataSet ds = new DataSet();
            try
            {
                int dept = 0;
                if (Convert.ToString(ddlDepartment.SelectedValue) != "Select")
                    dept = Convert.ToInt32(ddlDepartment.SelectedValue);
                string Fields = "DISTINCT UserGroupName,UserGroupID";
                string Table = "UserGroup";
                string Criteria = "";
                Criteria = "companyID = " + System.Convert.ToInt32(Session["CompanyID"]) + "";
                ////				if(Convert.ToString(ddlCompany.SelectedValue)!="Select" && Convert.ToInt32(ddlCompany.SelectedIndex)!=0)
                ////				{
                ////					Criteria = "companyID = "+System.Convert.ToInt32(ddlCompany.SelectedValue)+"";
                ////				}
                ////				else
                ////				{
                ////					Criteria = "companyID = "+System.Convert.ToInt32(Session["CompanyID"])+"";
                ////				}
                ds = oUser.GetGlobalDropDowns(Fields, Table, Criteria);
                ddlGroup.Items.Clear();
                if (ds != null)
                {
                    ddlGroup.DataSource = ds;
                    ddlGroup.DataBind();
                }
                else
                {
                    ddlGroup.DataSource = null;
                    ddlGroup.DataBind();
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }
            finally
            {
                ddlGroup.Items.Insert(0, "Select Group");
                ds = null;

            }
        }
        #endregion
        #region btnSearch_Click
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            PopulateSearchResult();
            //			if(ddlDepartment.SelectedIndex != 0 || ddlGroup.SelectedIndex != 0)
            //			{				
            //				if(ddlGroup.SelectedIndex == 0)
            //				{
            //					PopulateData( Convert.ToInt32(ddlDepartment.SelectedValue.Trim()),"",Convert.ToInt32(ViewState["CompanyID"]));
            //				}
            //				else if (ddlDepartment.SelectedIndex == 0)
            //				{
            //					PopulateData( 0,Convert.ToString(ddlGroup.SelectedItem.Text),Convert.ToInt32(ViewState["CompanyID"]));
            //					
            //				}
            //				else					
            //				{
            //				  PopulateData( Convert.ToInt32(ddlDepartment.SelectedValue.Trim()),Convert.ToString(ddlGroup.SelectedItem.Text),Convert.ToInt32(ViewState["CompanyID"]));
            //					
            //				}
            //			}
            //			else
            //			{
            //			 PopulateData( 0,"",Convert.ToInt32(ViewState["CompanyID"]));
            //			//	GetUserDetails( Convert.ToInt32(ViewState["CompanyID"]));
            //			//	lblMessage.Text = "Please select a dropdown to get specific data.";				
            //			}
        }
        #endregion
        #region PopulateData
        private void PopulateData(int iDepartmentID, string iGroupID, int iCompanyID)
        {
            try
            {
                DataTable dtbl = new DataTable();
                grdUser.Visible = true;
                dtbl = objUsers.GetUserList_Generic(iDepartmentID, iGroupID);
                grdUser.DataSource = dtbl;
                grdUser.DataBind();

                if (grdUser.Items.Count > 0)
                {
                    lblMessage.Text = "";
                    grdUser.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Sorry, no record(s) found.";
                    grdUser.Visible = false;
                }
                RecordSet rsCompany = null;
                rsCompany = Company.GetCompanyData(iCompanyID);
                Session.Add("SelectedCompanyTypeID", rsCompany["CompanyTypeID"]);
                Session.Add("SelectedCompanyID", rsCompany["CompanyID"]);
                pageLoaded = true;
            }
            catch { }
        }

        private void PopulateSearchResult()
        {
            try
            {
                DataTable dtSearchResult = new DataTable();
                grdUser.Visible = true;
                string strUserGroup = string.Empty;
                int iCompanyID = 0;
                if (Convert.ToString(ddlCompany.SelectedValue) != "Select Company" && Convert.ToInt32(ddlCompany.SelectedIndex) != 0)
                {
                    iCompanyID = System.Convert.ToInt32(ddlCompany.SelectedValue);
                }
                else
                {
                    iCompanyID = System.Convert.ToInt32(Session["CompanyID"]);

                }
                if (Convert.ToString(ddlGroup.SelectedValue) != "Select Group" && Convert.ToInt32(ddlGroup.SelectedIndex) != 0)
                {
                    strUserGroup = Convert.ToString(ddlGroup.SelectedItem.Text);
                }
                //	dtSearchResult = objUsers.GetUserList_SearchResult(Convert.ToInt32(iCompanyID),Convert.ToInt32(ddlBusinessUnit.SelectedValue.Trim()),Convert.ToInt32(ddlDepartment.SelectedValue.Trim()),strUserGroup,Convert.ToString(txtUserName.Text.Trim()));
                dtSearchResult = objUsers.GetUserList_SearchResult(Convert.ToInt32(iCompanyID), Convert.ToInt32(ddlBusinessUnit.SelectedValue.Trim()), Convert.ToInt32(ddlDepartment.SelectedValue.Trim()), strUserGroup, Convert.ToInt32(ddlUser.SelectedValue.Trim()));

                if (dtSearchResult != null && dtSearchResult.Rows.Count > 0)
                {
                    grdUser.DataSource = dtSearchResult;
                    grdUser.DataBind();
                }
                else
                {
                    grdUser.DataSource = null;
                    grdUser.DataBind();
                }

                if (grdUser.Items.Count > 0)
                {
                    lblMessage.Text = "";
                    grdUser.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Sorry, no record(s) found.";
                    grdUser.Visible = false;
                }
                //				RecordSet rsCompany = null;			
                //				rsCompany = Company.GetCompanyData(iCompanyID);
                //				Session.Add("SelectedCompanyTypeID", rsCompany["CompanyTypeID"]);
                //				Session.Add("SelectedCompanyID", rsCompany["CompanyID"]);
                pageLoaded = true;
            }
            catch (Exception Ex)
            {
                Ex.ToString();
            }
        }
        //GetUserList_SearchResult
        #endregion
        //Added by kuntalkarar on 20thSeptember2016
        [WebMethod]
        public static void SetSession()
        {
            HttpContext.Current.Session["UserEditClick_kk"] = "False";
            //return @"~/user/UserEdit.aspx";

        }
        #region GetURLEdit
        protected string GetURLEdit(object oUserID, object oNewUserGroup)
        {
            Session["UserEditClick_kk"] = "True";//Added by kuntalkarar on 20thSeptember2016
            JKS.Invoice objinvoice = new JKS.Invoice();
            string strUserID = Convert.ToString(oUserID);
            string strNewUserGroup = Convert.ToString(oNewUserGroup);
            string strURL = "";
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                if (strNewUserGroup == "G2")
                    strURL = "alert('Sorry, only Admin users can edit G2 approvers'); return false;";
                else
                    strURL = "location.href='UserEdit.aspx?UserID=" + strUserID + "'";
            }
            else
                strURL = "location.href='UserEdit.aspx?UserID=" + strUserID + "'";

            return (strURL);
        }

        #endregion


        #region: New Implementation on 25th February 2014
        protected void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            LoadDepartment(Convert.ToInt32(ddlCompany.SelectedValue));
            GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue));
            GetUserList();
            PopulateGroup();

        }
        public void LoadCompanyDropDown()
        {
            JKS.Approval objApproval = new JKS.Approval();
            DataSet ds = new DataSet();
            try
            {
                ds = objApproval.GetCompanyDropDownGeneric(System.Convert.ToInt32(Session["CompanyID"]));
                ddlCompany.DataSource = ds;
                ddlCompany.DataBind();
                ddlCompany.DataTextField = "CompanyName";
                ddlCompany.DataValueField = "CompanyID";
            }
            catch (Exception EX)
            {
                EX.ToString();
            }
            finally
            {
                ds = null;
                ddlCompany.Items.Insert(0, new ListItem("Select Company", "0"));
                //objApproval.Dispose();
            }
        }
        private void LoadDepartment(int CompanyID)
        {
            ddlDepartment.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentList_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(CompanyID));
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
        private void GetBusinessUnit(int CompanyID)
        {
            ddlBusinessUnit.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetBusinessUnit", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(CompanyID));
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


        private void GetUserList()
        {
            int CompanyID = 0;
            ddlUser.Items.Clear();
            DataTable dtUser = new DataTable();
            try
            {

                if (Convert.ToString(ddlCompany.SelectedValue) != "0" && Convert.ToInt32(ddlCompany.SelectedIndex) > 0)
                {
                    CompanyID = System.Convert.ToInt32(ddlCompany.SelectedValue);
                }
                else
                {
                    CompanyID = System.Convert.ToInt32(Session["CompanyID"]);
                }
                dtUser = objUsers.GetUserListCompanyWise(System.Convert.ToInt32(CompanyID));
                ddlUser.DataSource = dtUser;
                ddlUser.DataTextField = "UserName";
                ddlUser.DataValueField = "UserID";
                ddlUser.DataBind();

            }
            catch (Exception EX)
            {
                EX.ToString();
            }
            finally
            {
                dtUser = null;
                ddlUser.Items.Insert(0, new ListItem("Select User", "0"));

            }
        }



        #endregion
    }
}