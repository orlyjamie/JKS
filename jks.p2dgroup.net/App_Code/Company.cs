using System;
using System.Web.Mail;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.ETH.Web;

namespace JKS
{
    /// <summary>
    /// Summary description for Company.
    /// </summary>
    public class Company : System.Web.UI.Page
    {
        #region Mail variable declaration
        private MailFormat _mailFormat = MailFormat.Html;
        private MailPriority _mailPriority = MailPriority.High;
        #endregion
        #region SqlClient's objects
        protected SqlCommand objComm = null;
        protected SqlConnection objConn = null;
        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected SqlCommand sqlCmd = null;
        protected SqlParameter sqlReturnParam = null;
        protected DataSet ds = null;
        #endregion
        #region Variable Declaretion
        private string errorMessage = null;
        private string strlinkPath = ConfigurationManager.AppSettings["SitePathForLink"].Trim();
        #endregion
        #region Default Constructor
        public Company()
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
        #region GetCompanyList
        /// <summary>
        /// Gets the list of existing Companies
        /// </summary>
        /// <returns></returns>
        public static RecordSet GetCompanyList()
        {
            RecordSet rs = null;
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);

            rs = da.ExecuteQuery("vCompany", " Active = 1");

            return rs;
        }
        #endregion
        #region GetSubCompanyList
        /// <summary>
        /// Gets the list of existing Companies
        /// </summary>
        /// <returns></returns>
        public static RecordSet GetSubCompanyList(int iParentCompanyID)
        {
            RecordSet rs = null;
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);

            rs = da.ExecuteQuery("vwSubCompany", "(ParentCompanyID = " + iParentCompanyID + " AND Active=1) OR CompanyID = " + iParentCompanyID);  // CH 040705 BALJIT SINGH

            return rs;
        }
        #endregion
        #region GetCompanyList
        public static RecordSet GetCompanyList(CompanyType companyType)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Company", "CompanyTypeID=" + (int)companyType);
            return rs;
        }
        #endregion
        #region GetBuyerCompanyList
        public static RecordSet GetBuyerCompanyList()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Company", "CompanyTypeID=1");
            return rs;
        }
        #endregion
        #region GetBuyerCompany
        public static RecordSet GetBuyerCompany(string CompanyCode)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Company", "CompanyTypeID=1 and CompanyCode = '" + CompanyCode + "'");
            return rs;
        }
        #endregion
        #region GetSupplierCompanyList
        public static RecordSet GetSupplierCompanyList(int buyerID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("up_getsupplieraddlist", buyerID);
            return rs;
        }
        #endregion
        #region GetBuyersTradingCompanyList
        public static RecordSet GetBuyersTradingCompanyList(int buyerCompanyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("vTradesPerson", "BuyerCompanyID=" + buyerCompanyID);
            return rs;
        }
        #endregion
        #region GetBuyerCompanyListOnID
        /// <summary>
        /// Gets the list of existing buyers on the basis of a supplier
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>

        public static RecordSet GetBuyerCompanyListOnID(int supplierID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("up_GetBuyerAddList", supplierID);
            return rs;
        }
        #endregion
        #region GetSupplierCompanyListOnID
        /// <summary>
        /// Gets the list of existing buyers on the basis of a supplier id
        /// </summary>
        /// <param name="buyerID"></param>
        /// <returns></returns>

        public static RecordSet GetSupplierCompanyListOnID(int buyerID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("sp_GetSupplierList", buyerID);
            return rs;
        }
        #endregion
        #region GetCompanyList
        /// <summary>
        /// Gets the list of existing Companies
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static RecordSet GetCompanyList(int userID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("up_GetCompanyList", userID);
            return rs;
        }
        #endregion
        #region GetCompanyListOnCondition
        /// <summary>
        /// Gets the list of existing Companies
        /// </summary>
        /// <returns></returns>
        public static RecordSet GetCompanyListOnCondition(int iBuyerCompanyID)
        {
            RecordSet rs = null;
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);

            rs = da.ExecuteQuery("vwCompanySuppliers", "BuyerCompanyID = " + iBuyerCompanyID);

            return rs;
        }
        #endregion
        #region GetCompanyData
        /// <summary>
        /// gets the detail of a specified Company by Company Code
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RecordSet GetCompanyData(string companyCode)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Company", "CompanyCode = '" + companyCode + "'");
            return rs;
        }
        #endregion
        #region GetCompanyData
        /// <summary>
        /// gets the detail of a specified Company
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RecordSet GetCompanyData(int companyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Company", "CompanyID = " + System.Convert.ToString(companyID));
            return rs;
        }
        #endregion
        #region GetSupplierCompanyData
        /// <summary>
        /// gets the detail of a specified Company by Company Code
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RecordSet GetSupplierCompanyData(string strNetworkID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Company", "NetworkID = '" + strNetworkID + "'");
            return rs;
        }
        #endregion
        #region GetSupplierContactData
        public static RecordSet GetSupplierContactData(int iCompanyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("SupplierContact", "CompanyID = " + iCompanyID);
            return rs;
        }
        #endregion
        #region GetCompanyTypeList
        /// <summary>
        /// gets the list of all the available Company Types
        /// </summary>
        /// <returns></returns>
        public static RecordSet GetCompanyTypeList()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("CompanyTypes");
            return rs;
        }
        #endregion
        #region GetMemberTypeList
        /// <summary>
        /// gets the list of all the available Member Types
        /// </summary>
        /// <returns></returns>
        public static RecordSet GetMemberTypeList()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("MemberTypes");
            return rs;
        }
        #endregion
        #region InsertCompanyData
        /// <summary>
        /// Adds a single record to the Company table
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public int InsertCompanyData(RecordSet rs, DataAccess da)
        {
            errorMessage = "";
            int companyID = 0;
            if (!da.InsertRow(rs, ref companyID))
                errorMessage = da.ErrorMessage;
            return companyID;
        }
        #endregion
        #region UpdateCompanyData
        public Boolean UpdateCompanyData(RecordSet rs)
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
        #region UpdateSupplierContact
        public Boolean UpdateSupplierContact(RecordSet rs)
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
        #region GetRoleID
        /// <summary>
        /// gets the RoleID for 
        /// </summary>
        /// <param name="companyType"></param>
        /// <param name="roleType"></param>
        /// <returns></returns>
        public static int GetRoleID(CompanyType companyType, RoleType roleType)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Roles", "CompanyType =" + (int)companyType + " and RoleType = " + (int)roleType);
            int i = (int)rs["RoleID"];

            rs = null;
            da.CloseConnection();
            da = null;

            return i;
        }
        #endregion
        #region GetRoleID
        public static int GetRoleID(int companyTypeID, RoleType roleType)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Roles", "CompanyTypeID =" + companyTypeID + " and RoleType = " + (int)roleType);
            int i = (int)rs["RoleID"];
            rs = null;
            da.CloseConnection();
            da = null;

            return i;
        }
        #endregion
        #region SendEmailInfo
        public void SendEmailInfo(DataGrid dgGrid)
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
            strMailBody = ConfigurationManager.AppSettings["MailBodySuppliers"].Trim();

            foreach (DataGridItem dgItem in dgGrid.Items)
            {
                CheckBox chk = (CheckBox)dgItem.FindControl("chkMail");

                if (chk.Checked)
                {
                    SendEmail(strMailFrom, dgItem.Cells[2].Text.Trim(), strCCMail, strBCCMail, strSubject, strMailBody);

                }
            }
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
        #region DeleteCompanyRecord
        public bool DeleteCompanyRecord(int iCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;


            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("stpDeleteCompanyRecord", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyID", iCompanyID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch { bRetVal = false; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region SaveThresHoldValues
        public bool SaveThresHoldValues(int iCompanyID, int iTimeLimit)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_SaveThresHoldValues", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyID", iCompanyID);
            sqlCmd.Parameters.Add("@TimeLimit", iTimeLimit);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region GetThresHoldValues
        public void GetThresHoldValues(int iCompanyID, out string strTimeLimit)
        {

            strTimeLimit = "";
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("ThresHold", "CompanyID = " + iCompanyID);

            while (!rs.EOF())
            {
                if (rs["TimeLimit"] != DBNull.Value)
                    strTimeLimit = rs["TimeLimit"].ToString();

                rs.MoveNext();
            }

            rs = null;
            da.CloseConnection();
            da = null;
        }
        #endregion
        #region GetSupplierList
        public DataTable GetSupplierList(string strSupplierList)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("stpGetSupplierListToSendMails", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@SupplierList", strSupplierList);
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region CheckNewLookSuppier
        public bool CheckNewLookSuppier(int iSupplierCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_CheckGMGSupplier", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue == 0)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region CheckNewLookUser
        public bool CheckNewLookUser(int iUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_CheckGMGUser", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@UserID", iUserID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue == 0)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region CheckCompanyIDInSupplierContact
        public bool CheckCompanyIDInSupplierContact(int iCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_CheckCompanyIDInSupplierContact", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyID", iCompanyID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue == 0)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region CheckDuplicateCompanyCode
        public bool CheckDuplicateCompanyCode(string strCompanyCode)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_CheckDuplicateCompanyCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyCode", strCompanyCode);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue == 0)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region CheckDuplicateUserName
        public bool CheckDuplicateUserName(string strUserName)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_CheckDuplicateUserName", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@UserName", strUserName);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue == 0)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region GetCountryList
        public DataTable GetCountryList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetCountryList", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region GetCountyList
        public DataTable GetCountyList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetCountyList", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region GetCompanyListForBranch
        public DataTable GetCompanyListForBranch()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetCompanyListForBranch", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region GetBranchNameCompanyWiseForHubAdmin
        public DataTable GetBranchNameCompanyWiseForHubAdmin(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetBranchNameCompanyWiseForHubAdmin", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region GetCompanyListForPurchaseInvoiceLog
        public DataTable GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int UserID, int UserTypeID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetCompanyListForPurchaseInvoiceLog_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@UserID", UserID);
            sqlDA.SelectCommand.Parameters.Add("@USerTypeID", UserTypeID);
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion

        //added by kuntalkarar on 11thJanuary2017
        #region GetCompanyListForPurchaseInvoiceLog
		public DataTable GetCompanyListForPurchaseInvoiceLog(int iCompanyID)
		{
			sqlConn=new SqlConnection(CBSAppUtils.PrimaryConnectionString);
			//sqlConn.Open();

			sqlDA = new SqlDataAdapter("sp_GetCompanyListForPurchaseInvoiceLog", sqlConn);
			sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
		
			sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);

			ds = new DataSet();
			try
			{
				sqlConn.Open();
				sqlDA.Fill(ds);
			}
			catch(Exception ex){string ss=ex.Message.ToString();}
			finally
			{

				sqlDA.Dispose();
				sqlConn.Close();
			}
			
			return (ds.Tables[0]);
		}
		#endregion
        #region GetBuyerCompanyListForTradingRelation
        public DataTable GetBuyerCompanyListForTradingRelation(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetBuyerCompanyListForTradingRelation", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region IsNewLookCompany
        public bool IsNewLookCompany(int iCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_IsGMGCompany", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyID", iCompanyID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue == 0)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region SaveMailContent
        public bool SaveMailContent(int iMailContentID, int iBuyerCompanyID, string strCompulsaryMailContent, string strNonCompulsaryMailContent, string strSubject, string strFromEmailID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_SaveMailContent", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@MailContentID", iMailContentID);
            sqlCmd.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);
            sqlCmd.Parameters.Add("@CompulsaryMailContent", strCompulsaryMailContent);
            sqlCmd.Parameters.Add("@NonCompulsaryMailContent", strNonCompulsaryMailContent);
            sqlCmd.Parameters.Add("@Subject", strSubject);
            sqlCmd.Parameters.Add("@FromEmailID", strFromEmailID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region GetMailContent
        public DataTable GetMailContent(int iBuyerCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetMailContent", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region SaveBranchForNewLook
        public bool SaveBranchForNewLook(int iCompanyID, string strBranch, string strBranchCode, string strAddress1,
                                        string strAddress2, string strAddress3, string strPostCode, string strTelephone,
                                        int iCountyID, int iCountryID, string strPEmail, int iModUserId, string strOption)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_SaveBranchForGMG", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyID", iCompanyID);
            if (strBranch.Trim() != "")
                sqlCmd.Parameters.Add("@Branch", strBranch);
            else
                sqlCmd.Parameters.Add("@Branch", DBNull.Value);

            if (strBranchCode.Trim() != "")
                sqlCmd.Parameters.Add("@BranchCode", strBranchCode);
            else
                sqlCmd.Parameters.Add("@BranchCode", DBNull.Value);

            if (strAddress1.Trim() != "")
                sqlCmd.Parameters.Add("@Address1", strAddress1);
            else
                sqlCmd.Parameters.Add("@Address1", DBNull.Value);

            if (strAddress2.Trim() != "")
                sqlCmd.Parameters.Add("@Address2", strAddress2);
            else
                sqlCmd.Parameters.Add("@Address2", DBNull.Value);

            if (strAddress3.Trim() != "")
                sqlCmd.Parameters.Add("@Address3", strAddress3);
            else
                sqlCmd.Parameters.Add("@Address3", DBNull.Value);

            if (strPostCode.Trim() != "")
                sqlCmd.Parameters.Add("@PostCode", strPostCode);
            else
                sqlCmd.Parameters.Add("@PostCode", DBNull.Value);

            if (strTelephone.Trim() != "")
                sqlCmd.Parameters.Add("@Telephone", strTelephone);
            else
                sqlCmd.Parameters.Add("@Telephone", DBNull.Value);

            if (iCountyID == 0)
                sqlCmd.Parameters.Add("@CountyID", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@CountyID", iCountyID);

            if (iCountryID == 0)
                sqlCmd.Parameters.Add("@CountryID", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@CountryID", iCountryID);

            if (strPEmail.Trim() != "")
                sqlCmd.Parameters.Add("@PEmail", strPEmail);
            else
                sqlCmd.Parameters.Add("@PEmail", DBNull.Value);

            sqlCmd.Parameters.Add("@ModUserId", iModUserId);

            sqlCmd.Parameters.Add("@Option", strOption);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region DeleteBranchRecord
        public bool DeleteBranchRecord(int iBranchID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;


            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("stpDeleteBranchRecord", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@BranchID", iBranchID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch { bRetVal = false; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region GetVatList
        public DataTable GetVatList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetVatList", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region GetVatName
        public string GetVatName(string strVatAbbreviation)
        {
            string strVatName = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_GetVatName", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@VatAbbreviation", strVatAbbreviation);
            sqlOutputParam = sqlCmd.Parameters.Add("@VatName", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strVatName = Convert.ToString(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strVatName);
        }
        #endregion
        #region DeleteApproverChain
        public bool DeleteApproverChain(int iCompanyApproverID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;


            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_DeleteApproverChain", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyApproverID", iCompanyApproverID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch { bRetVal = false; }

            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region GetNewLookParentCompanyAddress
        public DataTable GetNewLookParentCompanyAddress()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetGMGParentCompanyAddress", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region GetInvoiceAddressForCompanyToEdit
        public DataTable GetInvoiceAddressForCompanyToEdit(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetInvoiceAddressForCompanyToEdit", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region GetDeliveryAddressForCompanyToEdit
        public DataTable GetDeliveryAddressForCompanyToEdit(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetDeliveryAddressForCompanyToEdit", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region UpdateDeliveryAddressBranchForNewLook
        public bool UpdateDeliveryAddressBranchForNewLook(int iCompanyID, string strBranch, string strBranchCode, string strAddress1,
            string strAddress2, string strAddress3, string strPostCode, string strTelephone,
            int iCountyID, int iCountryID, string strPEmail, int iModUserId, string strOption)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateDeliveryAddressBranchForGMG", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyID", iCompanyID);

            if (strBranch.Trim() != "")
                sqlCmd.Parameters.Add("@Branch", strBranch);
            else
                sqlCmd.Parameters.Add("@Branch", DBNull.Value);

            if (strBranchCode.Trim() != "")
                sqlCmd.Parameters.Add("@BranchCode", strBranchCode);
            else
                sqlCmd.Parameters.Add("@BranchCode", DBNull.Value);

            if (strAddress1.Trim() != "")
                sqlCmd.Parameters.Add("@Address1", strAddress1);
            else
                sqlCmd.Parameters.Add("@Address1", DBNull.Value);

            if (strAddress2.Trim() != "")
                sqlCmd.Parameters.Add("@Address2", strAddress2);
            else
                sqlCmd.Parameters.Add("@Address2", DBNull.Value);

            if (strAddress3.Trim() != "")
                sqlCmd.Parameters.Add("@Address3", strAddress3);
            else
                sqlCmd.Parameters.Add("@Address3", DBNull.Value);

            if (strPostCode.Trim() != "")
                sqlCmd.Parameters.Add("@PostCode", strPostCode);
            else
                sqlCmd.Parameters.Add("@PostCode", DBNull.Value);

            if (strTelephone.Trim() != "")
                sqlCmd.Parameters.Add("@Telephone", strTelephone);
            else
                sqlCmd.Parameters.Add("@Telephone", DBNull.Value);

            if (iCountyID == 0)
                sqlCmd.Parameters.Add("@CountyID", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@CountyID", iCountyID);

            if (iCountryID == 0)
                sqlCmd.Parameters.Add("@CountryID", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@CountryID", iCountryID);

            if (strPEmail.Trim() != "")
                sqlCmd.Parameters.Add("@PEmail", strPEmail);
            else
                sqlCmd.Parameters.Add("@PEmail", DBNull.Value);

            sqlCmd.Parameters.Add("@ModUserId", iModUserId);

            sqlCmd.Parameters.Add("@Option", strOption);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region CheckCompanyCode
        public bool CheckCompanyCode(string strCompanyCode)
        {
            int iReturnValue = 0;
            bool bRetVal = true;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_CheckCompanyCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyCode", strCompanyCode);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch { bRetVal = false; }

            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (iReturnValue == 0)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region UpdateOverDueStatusAtAPLogin
        public void UpdateOverDueStatusAtAPLogin(int iBCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateOverDueStatusAtAPLogin", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@BCompanyID", iBCompanyID);
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {

                sqlCmd.Dispose();
                sqlConn.Close();
            }


        }
        #endregion
        #region UpdateOverDueStatusForCreditNotesAtAPLogin
        public void UpdateOverDueStatusForCreditNotesAtAPLogin(int iBCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateOverDueStatusForCreditNotesAtAPLogin", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@BCompanyID", iBCompanyID);
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {

                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }
        #endregion


        #region SaveNetworkSignUpDetails
        public bool SaveNetworkSignUpDetails(int iCompanyID, string strNetworkID, string strAccountingSystem,
                                                string strVersionRelease, string strMethod, string strRequirementsIfDirectIntegration,
                                                string strPaymentOption, int iAutomatedDelivery, int iAutomatedReceipt, string strCompanyRegNo)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_SaveNetworkSignUpDetails", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyID", iCompanyID);
            sqlCmd.Parameters.Add("@NetworkID", strNetworkID);
            sqlCmd.Parameters.Add("@AccountingSystem", strAccountingSystem);
            sqlCmd.Parameters.Add("@VersionRelease", strVersionRelease);
            sqlCmd.Parameters.Add("@Method", strMethod);

            if (strRequirementsIfDirectIntegration.Trim() != "")
                sqlCmd.Parameters.Add("@RequirementsIfDirectIntegration", strRequirementsIfDirectIntegration);
            else
                sqlCmd.Parameters.Add("@RequirementsIfDirectIntegration", DBNull.Value);

            sqlCmd.Parameters.Add("@PaymentOption", strPaymentOption);
            sqlCmd.Parameters.Add("@AutomatedDelivery", iAutomatedDelivery);
            sqlCmd.Parameters.Add("@AutomatedReceipt", iAutomatedReceipt);
            sqlCmd.Parameters.Add("@CompanyRegNo", strCompanyRegNo);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region UpdateSignOnlineDateForSupplierCompany
        public void UpdateSignOnlineDateForSupplierCompany(int iSupplierCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateSignOnlineDateForSupplierCompany", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {

                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }
        #endregion
        #region UpdateDirectDebitMandate
        public int UpdateDirectDebitMandate(int iSupplierCompanyID)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateDirectDebitMandateForSupplierCompany", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
        #region UpdateRejectStatus
        public bool UpdateRejectStatus(int iTID, int iStatusID, string strOption)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateRejectStatus", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@TID", iTID);
            sqlCmd.Parameters.Add("@StatusID", iStatusID);
            sqlCmd.Parameters.Add("@Option", strOption);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }


            return (bRetVal);
        }
        #endregion
        #region GetCompanyListForPurchaseInvoiceLog_CN
        public DataTable GetCompanyListForPurchaseInvoiceLog_CN(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetCompanyListForPurchaseInvoiceLog_CN", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }


            return (ds.Tables[0]);
        }
        #endregion
        #region GetAccountingSystem
        public DataTable GetAccountingSystem()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetAccountingSystem", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region SendCompulsory_NonCompulsoryEmailToSuppliers
        public bool SendCompulsory_NonCompulsoryEmailToSuppliers(DataGrid dgGrid, int iOption, int iBuyerCompanyID, int iMailGroup)
        {
            bool bSendMailFlag = false;

            string strMailFrom = "";
            string strCCMail = "";
            string strBCCMail = "";
            string strSubject = "";
            string strMailBody = "";
            strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
            strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
            strMailBody = "";

            foreach (DataGridItem dgItem in dgGrid.Items)
            {
                CheckBox chk = (CheckBox)dgItem.FindControl("chkMail");

                if (chk.Checked)
                {
                    strMailBody = "";
                    strMailBody = GetBuyerMailContent(iBuyerCompanyID, iOption, out strSubject, out strMailFrom);

                    strMailBody = strMailBody + "<BR><BR><BR><BR> Please click the link below to view your details.<BR><BR><BR> <A href= " + strlinkPath + dgItem.Cells[14].Text.Trim() + "&NID=" + dgItem.Cells[5].Text.Trim() + "> View Information </A><BR><BR><BR><BR><BR> Thanks<BR> P2D Support Team";

                    if (iMailGroup == 1)
                        SendEmail(strMailFrom, dgItem.Cells[6].Text.Trim(), strCCMail, strBCCMail, strSubject, strMailBody);
                    else
                        SendEmail(strMailFrom, dgItem.Cells[7].Text.Trim(), strCCMail, strBCCMail, strSubject, strMailBody);

                    bSendMailFlag = true;
                }
            }

            return (bSendMailFlag);
        }
        #endregion
        #region GetBuyerMailContent
        public string GetBuyerMailContent(int iBuyerCompanyID, int iOption, out string strSubject, out string strMailFrom)
        {
            string strMailContent = "";
            strSubject = "";
            strMailFrom = "";

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetMailContentForSupplier", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@Option", iOption);
            ds = new DataSet();

            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strMailContent = ds.Tables[0].Rows[0]["Content"].ToString().Trim();
                strSubject = ds.Tables[0].Rows[0]["Subject"].ToString().Trim();
                strMailFrom = ds.Tables[0].Rows[0]["FromEmailID"].ToString().Trim();
            }

            return (strMailContent);
        }
        #endregion
        #region UpdateVatRegNoForSupplierCompany
        public bool UpdateVatRegNoForSupplierCompany(int iSupplierCompanyID, string strVATRegNo)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateVatRegNoForSupplierCompany", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
            sqlCmd.Parameters.Add("@VatRegNo", strVATRegNo);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }


            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        #region GetVatRegNoForSupplierCompany
        public void GetVatRegNoForSupplierCompany(int iSupplierCompanyID, out string strVATRegNo)
        {
            strVATRegNo = "";

            SqlParameter sqlOutputParam = null;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_GetVatRegNoForSupplierCompany", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
            sqlOutputParam = sqlCmd.Parameters.Add("@VATRegNo", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strVATRegNo = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;

                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }
        #endregion
        #region CheckDuplicateCompanyName
        public int CheckDuplicateCompanyName(int iCompanyID, string strCompanyName)
        {
            int iReturnValue = 0;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("usp_CheckDuplicateCompanyName", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CompanyID", iCompanyID);
            sqlCmd.Parameters.Add("@CompanyName", strCompanyName);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
        #region GetTradersReference
        public string GetTradersReference(int iSupplierCompanyID)
        {
            string strTradersReference = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("usp_GetSupplierCompanyTradersReference", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
            sqlOutputParam = sqlCmd.Parameters.Add("@New_TradersReference", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strTradersReference = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strTradersReference);
        }
        #endregion
        #region GetCodingDescriptionAgainstNomDepCode
        public string GetCodingDescriptionAgainstNomDepCode(int iNominalCodeID, int iDepartmentCodeID)
        {
            string strCodingDescriptionID = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("usp_GetCodingDescriptionAgainstNomDepCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@NominalCodeID", iNominalCodeID);
            sqlCmd.Parameters.Add("@DepartmentCodeID", iDepartmentCodeID);
            sqlOutputParam = sqlCmd.Parameters.Add("@CodingDescriptionID", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strCodingDescriptionID = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }

            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strCodingDescriptionID);
        }
        #endregion
        #region GetNomDepCodeAgainstCodingDescriptionID
        public void GetNomDepCodeAgainstCodingDescriptionID(int iCodingDescriptionID, out string strNominalCodeID, out string strDepartmentCodeID)
        {
            SqlParameter sqlOutputParam1 = null;
            SqlParameter sqlOutputParam2 = null;

            strNominalCodeID = "";
            strDepartmentCodeID = "";

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("usp_GetNomDepCodeAgainstCodingDescriptionID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CodingDescriptionID", iCodingDescriptionID);
            sqlOutputParam1 = sqlCmd.Parameters.Add("@NominalCodeID", SqlDbType.Int);
            sqlOutputParam1.Direction = ParameterDirection.Output;
            sqlOutputParam2 = sqlCmd.Parameters.Add("@DepartmentCodeID", SqlDbType.Int);
            sqlOutputParam2.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strNominalCodeID = sqlOutputParam1.Value.ToString();
                strDepartmentCodeID = sqlOutputParam2.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam1 = null;
                sqlOutputParam2 = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }
        #endregion
        #region CheckTradingRelationForSupplierAndBuyer
        public int CheckTradingRelationForSupplierAndBuyer(string strSupplierNetWorkID, string strBuyerNetworkID)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("usp_CheckTradingRelationForSupplierAndBuyer", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@BuyerNetworkID", strBuyerNetworkID);
            sqlCmd.Parameters.Add("@SupplierNetworkID", strSupplierNetWorkID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
        #region GetSupplierVatNo
        public string GetSupplierVatNo(int iSupplierCompanyID)
        {
            string strSupplierVatNo = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("usp_GetSupplierVatNo", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
            sqlOutputParam = sqlCmd.Parameters.Add("@VATRegNo", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strSupplierVatNo = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {

                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strSupplierVatNo);
        }
        #endregion
        #region GetCurrency
        public DataTable GetCurrencyTypes()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("usp_GetCurrencyCode", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
        #region SearchCompanyForTradingRelation
        public DataTable SearchCompanyForTradingRelation(string strKeyWord)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("usp_SearchCompanyForTradingRelation", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (strKeyWord != "")
                sqlDA.SelectCommand.Parameters.Add("@KeyWord", strKeyWord);
            else
                sqlDA.SelectCommand.Parameters.Add("@KeyWord", DBNull.Value);

            ds = new DataSet();

            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion

        #region GetCompanyListForPurchaseInvoiceLogGMG
        public DataTable GetCompanyListForPurchaseInvoiceLogGMG(int iCompanyID, int iUserID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetCompanyListForPurchaseInvoiceLog_GMG", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@UserID", iUserID);

            ds = new DataSet();

            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion
    }
}