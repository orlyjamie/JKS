using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using CBSolutions.ETH.Web;
using System.Web.Mail;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using JKS;
namespace JKS
{
    public partial class SecurityInfo : System.Web.UI.Page
    {
        PasswordReset objPasswordReset = new PasswordReset();
        EncryptJKS objEncrypt = new EncryptJKS();
        protected void Page_Load(object sender, EventArgs e)
        {
            int UserID = 0;
            if (!IsPostBack)
            {
                System.GC.Collect();

                lblResetQuestion.Text = "";
                string x = Guid.NewGuid().ToString().Substring(0, 8);

                if (Request.QueryString["UserID"] != null)
                {
                    UserID = Convert.ToInt32(Request.QueryString["UserID"]);
                    BindSecurityQuestion(UserID);
                }
            }


        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //added by kuntal karar on 26thMay 2016to make user force to change password
            JKS.Users objUsers = new JKS.Users();

            string strResetAnswer = string.Empty;
            if (txtResetQuestionAnswer.Text.Trim().Length == 0)
            {
                this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Please enter Answer.'); </script>");
                return;
            }
            else
            {
                // Salting Password Needed

                SimpleHash objSimpleHash = new SimpleHash();
                string salt = ConfigurationManager.AppSettings["SaltingKey"].Trim().ToString();
                strResetAnswer = objSimpleHash.ComputeHash(txtResetQuestionAnswer.Text.Trim().ToString().ToUpper(), "SHA1", System.Text.Encoding.ASCII.GetBytes(salt));

                //	strResetAnswer=txtResetQuestionAnswer.Text.Trim().ToString().ToUpper();
            }

            int UserID = 0;
            if (Request.QueryString["UserID"] != null)
            {
                UserID = Convert.ToInt32(Request.QueryString["UserID"]);

            }


            int iReturnValue = 0;

            //blocked by kuntalkarar on 26thMay2016
            // List<PasswordReset> lstSaltedPassword = objPasswordReset.checkSaltedPassword(UserID, txtResetQuestionAnswer.Text);//strResetAnswer
            //added by kuntalkarar on 26thMay2016
            List<PasswordReset> lstSaltedPassword = objPasswordReset.checkSaltedPassword(UserID, strResetAnswer);
            if (lstSaltedPassword.Count > 0)
            {
                iReturnValue = lstSaltedPassword[0].iReturnValue;
            }




            if (iReturnValue == 1)
            {
                string strPassword = Guid.NewGuid().ToString().Substring(0, 8);

                int strDbUserID = Convert.ToInt32(Request.QueryString["UserID"]);
                ChangePassword(strDbUserID, strPassword);
                // Change Password Section
                int iReturnVal = 0;

                //blocked by kuntal karar on 28thMay 2016 for RijnDael encryption.
                //iReturnVal = ForgotChangePassword(strDbUserID, EncryptJKS.EncryptData(strPassword));

                //Added by kuntal karar on 28thMay 2016 for RijnDael encryption.
                iReturnVal = ForgotChangePassword(strDbUserID, objEncrypt.RijndaelEncription(strPassword));

                if (iReturnVal == -101)
                {
                    this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Error Changing Password.'); </script>");
                    return;
                }
                else
                {
                    //added by kuntal karar on 26thMay 2016 to make user force to change password
                    objUsers.PasswordChangeRequired(strDbUserID);

                }

                string Email = FetchUserEmail(strDbUserID);
                SendMailInfo(strDbUserID, Email, strPassword);
                Response.Redirect("JKSSecurityIntermediate.aspx"); // need to create this page..

            }
            else if (iReturnValue == -501)
            {
                Page.RegisterStartupScript("Reg", "<script>PopulateMessage(-501);</script>");
                return;
            }
            else if (iReturnValue == -500)
            {
                Page.RegisterStartupScript("Reg", "<script>PopulateMessage(-500);</script>");
                return;
            }


        }



        private void ChangePassword(int iUserID, string strNewPassword)
        {
            int iReturnVal = 0;
            iReturnVal = ForgotChangePassword(iUserID, EncryptJKS.EncryptData(strNewPassword));
            if (iReturnVal == -101)
                this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Error Changing Password.'); </script>");
            else
            {
                //this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('A new temporary password has been emailed to you. You will be required to change it when you login.'); </script>");
            }
        }

        public string FetchUserEmail(int iUserID)
        {
            string strUserEmail = "";
            strUserEmail = GetEmailByUserID(iUserID);
            return strUserEmail.Trim().ToString();
        }

        private void SendMailInfo(int iUserId, string strEmailId, string Password)
        {
            string strUserName = "";
            string strMailFrom = "";
            string strCCMail = "";
            string strBCCMail = "";
            string strSubject = "";
            string strMailBody = "";
            strUserName = GetUserNameByUserID(iUserId);
            try
            {
                strMailFrom = ConfigurationManager.AppSettings["ETCLoginInfoMailFrom"].Trim();
                strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
                strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
                strSubject = ConfigurationManager.AppSettings["JKSLoginInfoMailSubject"].Trim();
                strMailBody = ConfigurationManager.AppSettings["JKSLoginInfoMailBody"].Trim();
                strMailBody = strMailBody.Replace("@@UserName@@", strUserName).Replace("@@Password@@", Password);
                strMailBody = strMailBody.Replace("$", "<BR>") + "URL = <A href= http://JKS.p2dgroup.net/>http://JKS.p2dgroup.net/</A> <BR><BR><b>This is an automated message. Please do not reply to this email address.</b> <BR><BR>Kind regards,<BR><BR>Accounts Payable";
                strMailBody = "<FONT face=Verdana Size =2pt>" + strMailBody + "</FONT>";
                SendEmail(strMailFrom, strEmailId.Trim(), strCCMail, strBCCMail, strSubject, strMailBody);


                strMailBody = "";
            }
            catch (Exception Ex)
            {
                Ex.ToString();
            }
        }

        private void SendEmail(string _from, string _to, string _cc, string _bcc, string _subject, string _body)
        {
            MailFormat _mailFormat = MailFormat.Html;
            MailPriority _mailPriority = MailPriority.High;
            MailMessage _mailMSG = new MailMessage();
            _mailMSG.From = _from;
            _mailMSG.To = _to;
            _mailMSG.Cc = _cc;
            _mailMSG.Bcc = _bcc;
            _mailMSG.Subject = _subject;
            _mailMSG.Body = _body;
            _mailMSG.Priority = _mailPriority;
            _mailMSG.BodyFormat = _mailFormat;

            string _SMTPServer = ConfigurationManager.AppSettings["MailServer"].Trim();
            SmtpMail.SmtpServer = _SMTPServer;

            SmtpMail.Send(_mailMSG);
        }

        public int ForgotChangePassword(int iUserID, string strNewPassword)
        {
            int iReturnValue = 0;
            List<PasswordReset> lstForgotChangePassword = objPasswordReset.ForgotChangePassword(iUserID, strNewPassword);
            if (lstForgotChangePassword.Count > 0)
            {
                iReturnValue = lstForgotChangePassword[0].iReturnValue;
            }

            return (iReturnValue);
        }


        public string GetEmailByUserID(int iUserID)
        {
            string strUserEmail = "";
            List<PasswordReset> lstEmailByUserID = objPasswordReset.getEmailByUserID(iUserID);
            if (lstEmailByUserID.Count > 0)
            {
                strUserEmail = lstEmailByUserID[0].EmailId;
            }

            return strUserEmail;
        }

        public string GetUserNameByUserID(int iUserID)
        {
            string strUserName = "";

            List<PasswordReset> lstUserName = objPasswordReset.GetUserNameByUserID(iUserID);
            if (lstUserName.Count > 0)
            {
                strUserName = lstUserName[0].UserName;
            }
            return strUserName;
        }


        protected void BindSecurityQuestion(int UserID)
        {

            List<PasswordReset> lstPassword = objPasswordReset.getSecurityQuestion(UserID);
            try
            {
                if (lstPassword.Count > 0)
                {
                    lblResetQuestion.Text = lstPassword[0].ResetQuestion;
                }
                else
                {
                    Page.RegisterStartupScript("Reg", "<script>PopulateMessage(-500);</script>");
                    return;
                }

            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {

            }

        }

    }
}