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
using CBSolutions.ETH.Web;
using JKS;

namespace JKS
{
    /// <summary>
    /// Summary description for _default.
    /// </summary>
    public partial class firstlogin : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebForm Controls
        //protected System.Web.UI.WebControls.Panel Panel2;
        //protected System.Web.UI.WebControls.Label lblHeader;
        //protected System.Web.UI.WebControls.TextBox tbUserName;
        //protected System.Web.UI.WebControls.RequiredFieldValidator rfv_UserName;
        //protected System.Web.UI.WebControls.TextBox tbCurrentPassword;
        //protected System.Web.UI.WebControls.RequiredFieldValidator rfv_CurrentPassword;
        //protected System.Web.UI.WebControls.TextBox tbNewPassword;
        //protected System.Web.UI.WebControls.RequiredFieldValidator rfv_NewPassword;
        //protected System.Web.UI.WebControls.TextBox tbConfirmPassword;
        //protected System.Web.UI.WebControls.RequiredFieldValidator rfv_ConfirmPassword;
        //protected System.Web.UI.WebControls.CompareValidator cmv_Password;
        //protected System.Web.UI.WebControls.Button btnSubmit;
        //protected System.Web.UI.WebControls.Label lblMessage;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdProceedFlag;
        #endregion

        #region Variable Declaration
        protected Users objUsers = new Users();
        string strUserName = "";
        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);
            Session["FirstLoginPageVisited"] = "Yes";
            PopulateUserName();
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
                if (hdProceedFlag.Value == "1")
                {
                    //blocked by kuntalkarar on 1stJune2016 for #15
                    //Response.Redirect("JKS/Current/CurrentStatus.aspx");
                }
            }

        }
        #endregion

        #region ChangePassword
        private void ChangePassword(int iUserID, string strUserName, string strCurrentPassword, string strNewPassword)
        {
            int iReturnVal = 0;
            EncryptJKS objEncrypt = new EncryptJKS();
            //blocked by kuntalkarar on 31stMay2016
            //iReturnVal = objUsers.ChangePassword(iUserID, strUserName, EncryptJKS.EncryptData(strCurrentPassword), EncryptJKS.EncryptData(strNewPassword));
            //Added by kuntalkarar on 31stMay2016
            iReturnVal = objUsers.ChangePassword(iUserID, strUserName, objEncrypt.RijndaelEncription(strCurrentPassword), objEncrypt.RijndaelEncription(strNewPassword));

            if (iReturnVal == -101)
                //Modified by Mainak 2018-03-15
                //lblMessage.Text = "Error changing password.";
                lblMessage.Text = "Incorrect Current Password";
            else
            {
                if (objUsers.RecordFirstLogin(Convert.ToInt32(Session["UserID"])))
                {
                    lblMessage.Text = "Password changed successfully.";
                    Session["FirstLoginPageVisited"] = "No";
                    hdProceedFlag.Value = "1";
                }
                else
                    lblMessage.Text = "Password changed successfully. Error recording first login.";
            }
        }
        #endregion

        #region PopulateUserName
        public void PopulateUserName()
        {
            objUsers.GetUserNameByUserID(Convert.ToInt32(Session["UserID"]), out strUserName);
            tbUserName.Text = strUserName.Trim();
        }
        #endregion
    }
}