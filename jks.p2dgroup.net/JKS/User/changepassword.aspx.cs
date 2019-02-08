using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using CBSolutions.ETH.Web;
using JKS;

namespace JKS
{
    /// <summary>
    /// Summary description for GenGuid.
    /// </summary>
    public partial class changepassword : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region Web Form Controls
        protected System.Web.UI.WebControls.Panel Panel2;
        /*
        protected System.Web.UI.WebControls.Label lblHeader;
        protected System.Web.UI.WebControls.Button btnSubmit;
		
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.TextBox tbCurrentPassword;
        protected System.Web.UI.WebControls.TextBox tbNewPassword;
        protected System.Web.UI.WebControls.TextBox tbConfirmPassword;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_NewPassword;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_ConfirmPassword;
        protected System.Web.UI.WebControls.CompareValidator cmv_Password;
        protected System.Web.UI.WebControls.TextBox tbUserName;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_CurrentPassword;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfv_UserName;
         */
        #endregion

        #region Variable Declaration
        protected JKS.Users objUsers = new JKS.Users();
        string strUserName = "";
        int iModuleCompanyID = 180918;//124529 for AnchorSafety changed to 180918 for JKS 
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            PopulateUserName();


            if (!IsPostBack)
            {
                System.GC.Collect();
            }
            // Added by Mrinal on 2nd February 2015
            if (!IsPostBack)
            {
                btnSubmit.Attributes.Add("onclick", "javascript:return ValidateFormSubmission();");
                CBSAppUtils.PrimaryConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                BindSecurityCombo();
            }
            int SecurityInfo = IsSecuritySettingsEnterd(Convert.ToInt32(Session["UserID"]));
            if (SecurityInfo > 0)
            {
                lblErrorMessage.Text = "Your security question and answer have been saved. You do not need to re-enter them, nor will you need to re-enter them again when you reset/change your password - your security Q&A will remain the same.";
            }
            else
            {
                lblErrorMessage.Text = "";
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
        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                ChangePassword(Convert.ToInt32(Session["UserID"]), tbUserName.Text.Trim(), tbCurrentPassword.Text.Trim(), tbNewPassword.Text.Trim());
            }
        }
        #endregion
        EncryptJKS objEncrypt = new EncryptJKS();
        #region ChangePassword
        private void ChangePassword(int iUserID, string strUserName, string strCurrentPassword, string strNewPassword)
        {
            int iReturnVal = 0;

            //blocked by kuntalkarar on 28thMay2016
            //iReturnVal = objUsers.ChangePassword(iUserID, strUserName, EncryptJKS.EncryptData(strCurrentPassword), EncryptJKS.EncryptData(strNewPassword));
            //added by kuntalkarar on 28thMay2016
            iReturnVal = objUsers.ChangePassword(iUserID, strUserName, objEncrypt.RijndaelEncription(strCurrentPassword), objEncrypt.RijndaelEncription(strNewPassword));

            if (iReturnVal == -101)
                //lblMessage.Text = "Error changing password.";
                lblMessage.Text = "ERROR CHANGING PASSWORD.";
            else
                //lblMessage.Text = "Password changed successfully.";
                lblMessage.Text = "PASSWORD CHANGED SUCCESSFULLY .";
        }
        #endregion
        #region PopulateUserName
        public void PopulateUserName()
        {
            objUsers.GetUserNameByUserID(Convert.ToInt32(Session["UserID"]), out strUserName);
            tbUserName.Text = strUserName.Trim();
        }
        #endregion
        #region: Added by Mrinal on 12th September 2014
        protected void btnSecurity_Click(object sender, EventArgs e)
        {
            if (((ddlSecurityQuestion.SelectedValue != "--Select--" && ddlSecurityQuestion.SelectedValue != "0") && txtSecurityQuestion.Text.Trim().Length != 0))
            {
                this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Please either select a question from the dropdown OR enter one yourself.'); </script>");
                return;
            }
            if (((ddlSecurityQuestion.SelectedValue == "--Select--" || ddlSecurityQuestion.SelectedValue == "0") && txtSecurityQuestion.Text.Trim().Length == 0))
            {
                this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Please either select a question from the dropdown or enter one yourself.'); </script>");
            }
            else if (txtSecurityAnswer.Text.Trim().Length == 0)
            {
                this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Please enter security Answer.'); </script>");
            }
            else
            {
                CheckUserEmail();
            }
            //objUsers.GetUserNameByUserID(Convert.ToInt32(Session["UserID"]), out strUserName);
            int SecurityInfo = IsSecuritySettingsEnterd(Convert.ToInt32(Session["UserID"]));
            if (SecurityInfo > 0)
            {
                //lblErrorMessage.Text="Your security question and answer have been saved. You do not need to re-enter them.";
                lblErrorMessage.Text = "Your security question and answer have been saved. You do not need to re-enter them, nor will you need to re-enter them again when you reset/change your password - your security Q&A will remain the same.";
            }
            else
            {
                lblErrorMessage.Text = "";
            }
        }

        protected void CheckUserEmail()
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("CheckUserEmail", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(iModuleCompanyID));


            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string Message = string.Empty;
                    string strFetchedEmail = Convert.ToString(ds.Tables[0].Rows[0]["Email"]);
                    if (strFetchedEmail.Trim().Length == 0)
                    {
                        strFetchedEmail = "No Email held";
                    }
                    if (strFetchedEmail == "No Email held" || !IsValidEmail(Convert.ToString(ds.Tables[0].Rows[0]["Email"])))
                    {
                        Message = "The email address held for you in the system is invalid (" + strFetchedEmail + "). Please contact your system administrator to correct it.";
                        this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('" + Message + "'); </script>");
                        return;
                    }
                    //if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsDuplicateEmail"]) > 0)
                    //{                      
                    //    Message = "The email address held for you in the system is also being used by another active user ( " + strFetchedEmail + " ). Please contact the IS Helpdesk on 0330 606 1844 to update it.";
                    //    this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('" + Message + "'); </script>");

                    //    return;
                    //}
                    #region: Save

                    string strSecurityQuestion = string.Empty;

                    if (ddlSecurityQuestion.SelectedValue != "--Select--" && ddlSecurityQuestion.SelectedValue != "0")
                    {
                        strSecurityQuestion = ddlSecurityQuestion.SelectedValue.ToString();
                    }
                    else
                    {
                        strSecurityQuestion = txtSecurityQuestion.Text.Trim();
                    }
                    int iReturnValue = 0;
                    string strSecurityQuestionAnswer = string.Empty;
                    SimpleHash objSimpleHash = new SimpleHash();
                    string salt = ConfigurationManager.AppSettings["SaltingKey"].Trim().ToString();
                    strSecurityQuestionAnswer = objSimpleHash.ComputeHash(txtSecurityAnswer.Text.Trim().ToUpper(), "SHA1", Encoding.ASCII.GetBytes(salt));

                    SqlConnection sqlConnInner = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                    SqlCommand sqlCmdInner = new SqlCommand("UserSecurityInfo", sqlConnInner);
                    sqlCmdInner.CommandType = CommandType.StoredProcedure;
                    sqlCmdInner.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
                    sqlCmdInner.Parameters.Add("@ResetQuestion", strSecurityQuestion);
                    //blocked by kuntalkarar on 26thMay2016
                    //sqlCmdInner.Parameters.Add("@ResetAnswer", txtSecurityAnswer.Text);
                    //added by kuntalkarar on 26thMay2016
                    sqlCmdInner.Parameters.Add("@ResetAnswer", strSecurityQuestionAnswer);

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
                    if (iReturnValue == 1)
                    {
                        ddlSecurityQuestion.SelectedValue = "0";
                        txtSecurityAnswer.Text = "";
                        txtSecurityQuestion.Text = "";
                        this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Security question and answer saved successfully.'); </script>");
                        return;
                    }
                    else if (iReturnValue == -501)
                    {
                        ddlSecurityQuestion.SelectedValue = "0";
                        txtSecurityAnswer.Text = "";
                        txtSecurityQuestion.Text = "";
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = "Your security question and answer have been saved. You do not need to re-enter them, nor will you need to re-enter them again when you reset/change your password - your security Q&A will remain the same.";
                    }
                    else
                    {
                        lblErrorMessage.Visible = false;
                        lblErrorMessage.Text = "";
                    }
                    // Security Question and Answer saved successfully

                    //return (iReturnValue);


                    #endregion


                }

            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();

            }
            finally
            {
                if (sqlDA != null)
                    sqlDA.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }
        }

        protected void BindSecurityCombo()
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            SqlDataAdapter dap = new SqlDataAdapter("SELECT ResetQuestion,CompanyID FROM ResetQuestions Where CompanyID=" + iModuleCompanyID, sqlConn);
            DataSet dsP = new DataSet();
            try
            {
                sqlConn.Open();
                dap.Fill(dsP);
                if (dsP.Tables[0].Rows.Count > 0)
                {
                    ddlSecurityQuestion.DataSource = dsP;
                    ddlSecurityQuestion.DataTextField = "ResetQuestion";
                    ddlSecurityQuestion.DataValueField = "ResetQuestion";
                    ddlSecurityQuestion.DataBind();
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                dap.Dispose();
                sqlConn.Close();
            }

            ddlSecurityQuestion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));

        }

        protected bool IsValidEmail(string strIn)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            return Regex.IsMatch(strIn, MatchEmailPattern);

        }
        public int IsSecuritySettingsEnterd(int iUserID)
        {
            int iReturnValue = 0;

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlCommand sqlCmd = new SqlCommand("IsSecuritySettingsEnterd", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@UserID", iUserID);
            SqlParameter sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex)
            {
                string strExceptionMessage = ex.Message.Trim();
            }
            finally
            {
                sqlReturnParam = null;
                if (sqlCmd != null)
                    sqlCmd.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
    }
}