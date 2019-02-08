using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mail;
using System.Configuration;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

namespace CBSolutions.ETH.WebN
{
    /// <summary>
    /// Summary description for User.
    /// </summary>
    public class Users : System.Web.UI.Page
    {
        #region SqlClient's objects
        protected SqlCommand objComm = null;
        protected SqlConnection objConn = null;
        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected SqlCommand sqlCmd = null;
        protected SqlParameter sqlReturnParam = null;
        protected DataSet ds = null;
        #endregion

        #region Variable Daclaration
        private string errorMessage = null;
        #endregion

        #region Mail variable declaration
        private MailFormat _mailFormat = MailFormat.Html;
        private MailPriority _mailPriority = MailPriority.High;
        #endregion

        #region Default Constructor
        public Users()
        {

        }
        #endregion

        #region Property Declaration
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }
        #endregion

        #region GetRolesList
        /// <summary>
        /// Gets the list of Roles available
        /// </summary>
        /// <returns></returns>
        public static RecordSet GetRolesList(int companyTypeID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Roles", "CompanyTypeID = " + companyTypeID);
            return rs;
        }
        #endregion

        #region GetUserTypeList
        /// <summary>
        /// Gets the list of User Types 
        /// </summary>
        /// <returns></returns>
        public static RecordSet GetUserTypeList(int CompanyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("sp_UserType", CompanyID);
            return rs;
        }
        #endregion

        #region GetUserList
        /// <summary>
        /// Gets the list of Users for the specified company
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public static RecordSet GetUserList(string companyCode)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("up_GetCompanyUser", companyCode);
            return rs;
        }
        #endregion

        #region GetUserListFromCompanyID
        /// <summary>
        /// Gets the list of Users for the specified company
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public static RecordSet GetUserListFromCompanyID(int iCompanyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("up_GetCompanyUserFromCompID", iCompanyID);
            return rs;
        }
        #endregion

        #region GetUserListOnID
        /// <summary>
        /// Gets the list of Users for the specified company on the basis of company id
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>		

        public static RecordSet GetUserListOnID(int companyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("vCompanyUsers", "CompanyID = " + System.Convert.ToString(companyID));
            return rs;
        }
        #endregion

        #region GetUserListOnSBID
        public static RecordSet GetUserListOnSBID(int iBuyerCompanyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("vCompanyUserApprove", "CompanyID=" + iBuyerCompanyID);
            return rs;
        }
        #endregion

        #region GetUserData
        /// <summary>
        /// gets the detail of a specified user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static RecordSet GetUserData(int userID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Users", "UserID = " + System.Convert.ToString(userID));
            return rs;
        }
        #endregion

        #region GetCompanyType
        public int GetCompanyType(int iCompanyID)
        {
            int iCompanyTypeID = 0;
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Company", "CompanyID = " + System.Convert.ToString(iCompanyID));
            try
            {
                if (rs.RecordCount > 0)
                {
                    while (!rs.EOF())
                    {
                        iCompanyTypeID = Convert.ToInt32(rs["CompanyTypeID"]);
                        rs.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {

                rs = null;
                da.CloseConnection();
                da = null;
            }

            return (iCompanyTypeID);
        }
        #endregion

        #region GetRolesList
        /// <summary>
        /// Gets the list of Roles available
        /// </summary>
        /// <returns></returns>
        public static RecordSet GetRolesList()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Roles");
            return rs;
        }
        #endregion

        #region InsertUserData
        /// <summary>
        /// Adds a single record to the user table
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="da"></param>
        /// <returns></returns>
        public int InsertUserData(RecordSet rs, DataAccess da)
        {
            errorMessage = "";
            int UserID = 0;
            if (!da.InsertRow(rs, ref UserID))
                errorMessage = da.ErrorMessage;
            return UserID;
        }
        #endregion

        #region InsertUserData
        /// <summary>
        /// Adds a single record to the user table in a single transaction context
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public int InsertUserData(RecordSet rs)
        {
            errorMessage = "";
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            int UserID = 0;
            da.BeginTransaction();
            if (!da.InsertRow(rs, ref UserID))
            {
                da.RollbackTransaction();
                errorMessage = da.ErrorMessage;
            }
            else
                da.CommitTransaction();
            da.CloseConnection();
            da = null;
            return UserID;
        }
        #endregion

        #region UpdateUserData
        public Boolean UpdateUserData(RecordSet rs)
        {
            errorMessage = "";
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            da.BeginTransaction();
            if (!da.UpdateRow(rs))
            {
                da.RollbackTransaction();
                errorMessage = da.ErrorMessage;
                return false;
            }
            da.CommitTransaction();
            da.CloseConnection();
            da = null;

            return true;
        }
        #endregion

        #region SendEmail
        /// <summary>
        /// Method to send mail
        /// </summary>
        /// <param name="_from"></param>
        /// <param name="_to"></param>
        /// <param name="_cc"></param>
        /// <param name="_bcc"></param>
        /// <param name="_subject"></param>
        /// <param name="_body"></param>
        private void SendEmail(string _from, string _to, string _cc, string _bcc, string _subject, string _body)
        {
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
        #endregion

        #region SendUserNamePassword
        public void SendUserNamePassword(DataGrid dgGrid)
        {
            int iCounter = 0;
            string strUserName = "";
            string strPassWord = "";
            string strMailFrom = "";
            string strCCMail = "";
            string strBCCMail = "";
            string strSubject = "";
            string strMailBody = "";
            strMailFrom = ConfigurationManager.AppSettings["MailFrom"].Trim();
            strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
            strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
            strSubject = ConfigurationManager.AppSettings["MailSubject"].Trim();
            strMailBody = ConfigurationManager.AppSettings["MailBody"].Trim();

            foreach (DataGridItem dgItem in dgGrid.Items)
            {
                CheckBox chk = (CheckBox)dgItem.FindControl("chkMail");
                if (chk.Checked)
                {
                    if (iCounter != Convert.ToInt32(dgItem.Cells[5].Text.Trim()))
                    {
                        iCounter = Convert.ToInt32(dgItem.Cells[5].Text.Trim());
                        GetUserNamePassword(Convert.ToInt32(dgItem.Cells[5].Text.Trim()), out  strUserName, out strPassWord);
                        strMailBody = strMailBody.Replace("@@UserName@@", strUserName).Replace("@@Password@@", strPassWord);
                        SendEmail(strMailFrom, dgItem.Cells[3].Text.Trim(), strCCMail, strBCCMail, strSubject, strMailBody);
                    }
                }
            }
        }
        #endregion

        #region SendUserNamePassword
        public bool SendUserNamePassword(string strUserName, string strPassword, string strEmail)
        {
            bool bSendMailFlag = true;

            try
            {
                string strMailFrom = "";
                string strCCMail = "";
                string strBCCMail = "";
                string strSubject = "";
                string strMailBody = "";

                strMailFrom = ConfigurationManager.AppSettings["MailFrom"].Trim();
                strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
                strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
                strSubject = ConfigurationManager.AppSettings["MailSubject"].Trim();

                strMailBody = "Dear User, <BR><BR><BR><BR> <B> <U> <font face='Verdana'> Your registration is successfull. </font> </U> </B> <BR><BR> " +
                    "<blockquote> Your UserName is : " + strUserName + "<BR>" +
                    "Your Password is : " + strPassword + "</blockquote><BR><BR><BR><BR>" +
                    "Thanks<BR>P2D Support Team.";

                SendEmail(strMailFrom, strEmail, strCCMail, strBCCMail, strSubject, strMailBody);

            }
            catch { bSendMailFlag = false; }

            return (bSendMailFlag);
        }
        #endregion

        #region GetUserNamePassword
        public void GetUserNamePassword(int iUserID, out string strUserName, out string strPassWord)
        {
            strUserName = "";
            strPassWord = "";
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Users", "UserID = " + System.Convert.ToString(iUserID));
            try
            {
                while (!rs.EOF())
                {
                    strUserName = rs["UserName"].ToString();
                    if (Convert.ToString(rs["EncryptPassword"]) != "")
                        strPassWord = Encrypt.DecryptString(rs["EncryptPassword"].ToString());
                    else
                        strPassWord = rs["UserPassword"].ToString();

                    rs.MoveNext();
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }
            finally
            {
                rs = null;
                da.CloseConnection();
                da = null;
            }
        }
        #endregion

        #region GetUserNamesForCompanyApproverRelation
        public DataTable GetUserNamesForCompanyApproverRelation(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            try
            {
                sqlConn.Open();

                sqlDA = new SqlDataAdapter("stpGetUserNamesForCompanyApproverRelation", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);

                ds = new DataSet();

                sqlDA.Fill(ds);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();

            }
            return (ds.Tables[0]);
        }
        #endregion

        #region GetCompanyApproverListRelation
        public DataTable GetCompanyApproverListRelation(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            try
            {
                sqlConn.Open();

                sqlDA = new SqlDataAdapter("stpGetCompanyApproverListRelation", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);

                ds = new DataSet();

                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {

                sqlDA.Dispose();
                sqlConn.Close();

            }
            return (ds.Tables[0]);
        }
        #endregion

        #region SaveCompanyApproverRelation
        public bool SaveCompanyApproverRelation(int iCompanyApproverID, int iCompanyID,
                        int iApproverID_1, int iApprover_1_StandByID_1, int iApprover_1_StandByID_2,
                        int iApproverID_2_1, int iApproverID_2_2,
                        int iApprover_2_StandByID_1, int iApprover_2_StandByID_2)
        {
            int iReturnValue = 0;
            bool bRetVal = true;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("stpSaveCompanyApproverRelation", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@CompanyApproverID", iCompanyApproverID);
                sqlCmd.Parameters.Add("@CompanyID", iCompanyID);

                sqlCmd.Parameters.Add("@ApproverID_1", iApproverID_1);
                sqlCmd.Parameters.Add("@Approver_1_StandByID_1", iApprover_1_StandByID_1);
                sqlCmd.Parameters.Add("@Approver_1_StandByID_2", iApprover_1_StandByID_2);


                sqlCmd.Parameters.Add("@ApproverID_2_1", iApproverID_2_1);
                sqlCmd.Parameters.Add("@ApproverID_2_2", iApproverID_2_2);
                sqlCmd.Parameters.Add("@Approver_2_StandByID_1", iApprover_2_StandByID_1);
                sqlCmd.Parameters.Add("@Approver_2_StandByID_2", iApprover_2_StandByID_2);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();

                if (iReturnValue == -101)
                {
                    bRetVal = false;
                }

            }
            return (bRetVal);
        }
        #endregion

        #region DeleteUserRecord
        public bool DeleteUserRecord(int iUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("stpDeleteUserRecord", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@UserID", iUserID);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();


                if (iReturnValue == -101)
                {
                    bRetVal = false;
                }

            }
            return (bRetVal);
        }
        #endregion

        #region GetSubCompanyNames
        public DataTable GetSubCompanyNames(int iParentCompanyID)
        {
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlDA = new SqlDataAdapter("stpGetSubCompanyNames", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDA.SelectCommand.Parameters.Add("@ParentCompanyID", iParentCompanyID);

                ds = new DataSet();

                sqlDA.Fill(ds);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {

                sqlDA.Dispose();
                sqlConn.Close();

            }
            return (ds.Tables[0]);
        }
        #endregion

        #region ChangePassword
        public int ChangePassword(int iUserID, string strUserName, string strCurrentPassword, string strNewPassword)
        {
            int iReturnValue = 0;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("sp_ChangePassword_Encrypt", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;


                sqlCmd.Parameters.Add("@UID", iUserID);
                sqlCmd.Parameters.Add("@UName", strUserName);
                sqlCmd.Parameters.Add("@CPword", strCurrentPassword);
                sqlCmd.Parameters.Add("@NPassword", strNewPassword);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region CheckFirstLogin
        public bool CheckFirstLogin(int iUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("sp_CheckFirstLogin", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;


                sqlCmd.Parameters.Add("@UserID", iUserID);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();

                if (iReturnValue == -101)
                {
                    bRetVal = false;
                }

            }
            return (bRetVal);
        }
        #endregion

        #region RecordFirstLogin
        public bool RecordFirstLogin(int iUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("sp_RecordFirstLogin", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;


                sqlCmd.Parameters.Add("@UserID", iUserID);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();

                if (iReturnValue == -101)
                {
                    bRetVal = false;
                }

            }
            return (bRetVal);
        }
        #endregion

        #region CheckDuplicateApprover1
        public bool CheckDuplicateApprover1(int iCompanyApproverID, int iCompanyID, int iApproverID1)
        {
            int iReturnValue = 0;
            bool bRetVal = false;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("sp_CheckDuplicateApprover1", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@CompanyApproverID", iCompanyApproverID);
                sqlCmd.Parameters.Add("@CompanyID", iCompanyID);
                sqlCmd.Parameters.Add("@ApproverID1", iApproverID1);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();

                if (iReturnValue == -101)
                {
                    bRetVal = true;
                }

            }
            return (bRetVal);
        }
        #endregion

        #region GetSupplierCompanyUsersByCompanyID
        public DataTable GetSupplierCompanyUsersByCompanyID(int iSupplierCompanyID)
        {
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlDA = new SqlDataAdapter("sp_GetSupplierCompanyUsersByCompanyId", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);

                ds = new DataSet();

                sqlDA.Fill(ds);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();

            }
            return (ds.Tables[0]);
        }
        #endregion

        #region DeleteSupplierUserRecord
        public bool DeleteSupplierUserRecord(int iUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("sp_DeleteSupplierUserRecord", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@UserID", iUserID);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);



            }
            catch { bRetVal = false; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
                if (iReturnValue == -101)
                {
                    bRetVal = false;
                }

            }
            return (bRetVal);
        }
        #endregion

        #region CheckDuplicateUserName
        public bool CheckDuplicateUserName(int iUserID, string strUserName, int iCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = false;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("sp_CheckDuplicateUserNameNew", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@UserID", iUserID);
                sqlCmd.Parameters.Add("@UserName", strUserName);
                sqlCmd.Parameters.Add("@CompanyID", iCompanyID);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();

                if (iReturnValue == -101)
                {
                    bRetVal = true;
                }

            }
            return (bRetVal);
        }
        #endregion

        #region GetCompanyListForHubAdmin
        public DataTable GetCompanyListForHubAdmin()
        {
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                sqlDA = new SqlDataAdapter("sp_GetCompanyListForHubAdmin", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                ds = new DataSet();

                sqlDA.Fill(ds);

            }
            catch (Exception ex)
            {
                string er = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();

            }
            return (ds.Tables[0]);
        }
        #endregion

        #region SendUserNamePassword
        public bool SendUserNamePassword(string strUserName, string strPassword, string strEmail, string strMethod)
        {
            bool bSendMailFlag = true;

            try
            {
                string strMailFrom = "";
                string strCCMail = "";
                string strBCCMail = "";
                string strSubject = "";
                string strMailBody = "";

                strMailFrom = ConfigurationManager.AppSettings["MailFrom"].Trim();
                strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
                strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
                strSubject = ConfigurationManager.AppSettings["MailSubject"].Trim();

                strMailBody = "<Font face='Verdana'>Welcome to the P2D Document Network.<BR><BR><BR>" +
                                "You are now activated to begin sending invoices electronically to your client(s).<BR>" +
                                "Before you get started, you will need to speak with the person in your company who has signed up to the system,<BR>" +
                                "in order to obtain your company’s unique Network ID (if this has not already been passed to you). NB It is the same as your Agreement number.<BR>" +
                                "For security reasons, this is not provided to you in this e-mail.<BR>" +
                                "If you do not know who this is, please request the name by responding to this e-mail.<BR>" +
                                "Using this Network ID and the following username and password,<BR>" +
                                "please log onto the User Log-In section of www.p2dgroup.com/default.aspx<BR><BR>" +
                                "Username = " + strUserName + "<BR>" +
                                "Password = " + strPassword + "<BR><BR>" +
                                "Again, for security you will be asked to reset your password.<BR>" +
                                "Please then follow the guidelines for the " + strMethod + " option.<BR>" +

                                "Kind regards,<BR><BR>" +

                                "P2D Support Services<BR>" +
                                "support@p2dgroup.com<BR>" +
                                "01189 255550</Font><BR>";


                SendEmail(strMailFrom, strEmail, strCCMail, strBCCMail, strSubject, strMailBody);

            }
            catch { bSendMailFlag = false; }

            return (bSendMailFlag);
        }
        #endregion

        #region SendNetworkIDDetailsEmail
        public bool SendNetworkIDDetailsEmail(string strMailBody, string strToEmail)
        {
            bool bSendMailFlag = true;

            try
            {
                string strMailFrom = "";
                string strCCMail = "";
                string strBCCMail = "";
                string strSubject = "";

                strMailFrom = ConfigurationManager.AppSettings["MailFrom"].Trim();
                strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
                strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
                strSubject = ConfigurationManager.AppSettings["MailSubject"].Trim();

                Users objUsers = new Users();
                objUsers.SendWorkProgressMailToRelationshipManager(Session["SPNetworkID"].ToString(), Session["SCompanyName"].ToString().Trim(), 2);

                SendEmail(strMailFrom, strToEmail, strCCMail, strBCCMail, strSubject, strMailBody);

            }
            catch { bSendMailFlag = false; }

            return (bSendMailFlag);
        }
        #endregion

        #region SendWorkProgressMailToRelationshipManager
        public bool SendWorkProgressMailToRelationshipManager(string strNetworkID, string strCompanyName, int iOption)
        {
            bool bSendMailFlag = true;

            try
            {
                salescall objSales = new salescall();

                string strMailFrom = "";
                string strToMail = "";
                string strCCMail = "";
                string strBCCMail = "";
                string strSubject = "";
                string strMailBody = "";

                strToMail = objSales.GetRelationShipManagerEmailAddress(strNetworkID);
                strMailFrom = ConfigurationManager.AppSettings["MailFrom"].Trim();
                strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
                strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
                strSubject = ConfigurationManager.AppSettings["MailSubject"].Trim();

                if (iOption == 1)
                    strMailBody = strCompanyName + "<BR><BR>Contract Returned";
                else if (iOption == 2)
                    strMailBody = strCompanyName + "<BR><BR>Live on Network";
                else if (iOption == 3)
                    strMailBody = strCompanyName + "<BR><BR>Agreement Created";

                SendEmail(strMailFrom, strToMail, strCCMail, strBCCMail, strSubject, strMailBody);

            }
            catch { bSendMailFlag = false; }

            return (bSendMailFlag);
        }
        #endregion

        #region SendMailToP2DIfDirectIntegration
        public bool SendMailToP2DIfDirectIntegration(string strCompanyName)
        {
            bool bSendMailFlag = true;

            try
            {
                string strMailFrom = "";
                string strToMail = "";
                string strCCMail = "";
                string strBCCMail = "";
                string strSubject = "";
                string strMailBody = "";
                strToMail = "rjaiswal@vnsinfo.com.au";
                strMailFrom = ConfigurationManager.AppSettings["MailFrom"].Trim();
                strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
                strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
                strSubject = ConfigurationManager.AppSettings["MailSubject"].Trim();

                strMailBody = strCompanyName + "<BR><BR>Direct integration required";

                SendEmail(strMailFrom, strToMail, strCCMail, strBCCMail, strSubject, strMailBody);
            }
            catch { bSendMailFlag = false; }

            return (bSendMailFlag);
        }
        #endregion

    }
}