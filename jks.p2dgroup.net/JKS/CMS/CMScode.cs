using System;
using System.Configuration;
using System.Web;
using System.Web.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
namespace CBSolutions.ETH.Web.ETC.CMS
{
    /// <summary>
    /// Created By: Rinku Santra
    /// Created Date: 01-06-2012
    /// Summary description for CMScode.
    /// </summary>
    public class CMScode
    {
        public CMScode()
        {
        }
        public static string EncryptData(string Message)
        {
            byte[] Results;
            string passphrase = "Password";
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }
        public static string DecryptString(string Message)
        {
            Message = Message.Replace(" ", "+");
            byte[] Results;
            string passphrase = "Password";
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }

        public static void SendEmail(string _from, string _to, string _cc, string _bcc, string _subject, string _body)
        {
            MailMessage _mailMSG = new MailMessage();
            _mailMSG.From = _from;
            _mailMSG.To = _to;
            _mailMSG.Cc = _cc;
            _mailMSG.Bcc = _bcc;
            _mailMSG.Subject = _subject;
            _mailMSG.Body = _body;
            _mailMSG.Priority = MailPriority.High; ;
            _mailMSG.BodyFormat = MailFormat.Html;
            string _SMTPServer = ConfigurationManager.AppSettings["MailServer"].Trim();
            SmtpMail.SmtpServer = _SMTPServer;
            SmtpMail.Send(_mailMSG);
        }
        public static int checkstatusCMS(int depID)
        {
            int iReturnValue = 0;
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            sqlCmd = new SqlCommand("Sp_CheckWeeklySummary_CMS", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@depID", Convert.ToInt32(depID));
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMScode: <br /> " + ex.Message.ToString());
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return iReturnValue;
        }
    }
}
