using System;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Configuration;
using System.Web.Mail;

namespace CBSolutions.ETH.Web.ETC.CreditNotes
{
    /// <summary>
    /// Summary description for Invoice_NL_CN.
    /// </summary>
    public class Invoice_NL_CN
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
        protected SqlDataReader sqlDR = null;
        protected SqlParameter sqlReturnParam = null;
        protected SqlParameter sqlOutputParam = null;

        protected DataSet ds = null;
        #endregion
        #region Variable Declaration
        private string errorMessage = null;
        #endregion
        public Invoice_NL_CN()
        {
        }

        #region Property Declaration
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }
        #endregion
        #region GetTradingPartnerList
        public static RecordSet GetTradingPartnerList(int supplierCompanyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("vSupplierTradingRelationList", "SupplierCompanyID = " + System.Convert.ToString(supplierCompanyID));
            return rs;
        }
        #endregion
        #region GetCurrencyTypesList
        public static RecordSet GetCurrencyTypesList()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("CurrencyTypes", "", "CurrencyCode");
            return rs;
        }
        #endregion
        #region GetVATTypesList
        public static RecordSet GetVATTypesList()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("VATTypes");
            return rs;
        }
        #endregion
        #region InsertInvoiceHeadData
        public int InsertInvoiceHeadData(RecordSet rs, DataAccess da)
        {
            errorMessage = "";
            int PKID = 0;
            decimal VATAmt = System.Convert.ToDecimal(rs["VATAmt"]);
            rs["VATAmt"] = VATAmt;
            if (!da.InsertRow(rs, ref PKID))
                errorMessage = da.ErrorMessage;
            return PKID;
        }
        #endregion
        #region InsertInvoiceDetailData
        public Boolean InsertInvoiceDetailData(int invoiceID, RecordSet rs, DataAccess da)
        {
            errorMessage = "";
            rs.MoveFirst();
            int PKID = 0;
            while (!rs.EOF())
            {
                rs["CreditNoteID"] = invoiceID;
                if (!da.InsertRow(rs, ref PKID))
                {
                    errorMessage = da.ErrorMessage;
                    return false;
                }
                rs.MoveNext();
            }
            return true;
        }
        #endregion
        #region GetInvoiceHead
        public static RecordSet GetInvoiceHead(int invoiceID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("stpGetInvoiceHeader_CN", invoiceID);
            return rs;
        }
        #endregion

        #region GetInvoiceDetail
        public static RecordSet GetInvoiceDetail(int invoiceID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("CreditNoteDetail", "CreditNoteID = " + invoiceID);
            return rs;
        }
        #endregion

        #region GetStatus

        public static String GetStatus(int statusId)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("InvCNStatus", "statusId = " + statusId);
            if (rs.RecordCount > 0)
            {
                if (rs["Status"] != DBNull.Value)
                    return rs["Status"].ToString();
                else
                    return "";
            }
            else
                return "";
        }

        #endregion

        #region XML related code

        #endregion

        #region GetDepartmentList
        public DataTable GetDepartmentList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("stpGetDepartmentList", sqlConn);
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

        #region GetProjectList
        public DataTable GetProjectList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("stpGetProjectList", sqlConn);
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

        #region GetNominalCodeList
        public DataTable GetNominalCodeList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlDA = new SqlDataAdapter("stpGetNominalCodeList", sqlConn);
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

        #region GetUserName
        public void GetUserName(int iUserID, out string strUserName, out string strEmail)
        {
            strUserName = "";
            strEmail = "";

            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Users", "UserID = " + System.Convert.ToString(iUserID));

            while (!rs.EOF())
            {
                strUserName = rs["UserName"].ToString();
                strEmail = rs["Email"].ToString();
                rs.MoveNext();
            }

            rs = null;
            da.CloseConnection();
            da = null;
        }
        #endregion

        #region SendEmailInfo
        public void SendEmailInfo(string strUseName, string strMailTo, string strInvoiceNo, string strApprovalType, string mailBody)
        {
            string strMailFrom = "";
            string strCCMail = "";
            string strBCCMail = "";
            string strSubject = "";
            string strMailBody = "";

            strMailFrom = ConfigurationManager.AppSettings["MailFrom"].Trim();
            strCCMail = ConfigurationManager.AppSettings["CCMail"].Trim();
            strBCCMail = ConfigurationManager.AppSettings["BCCMail"].Trim();
            strSubject = ConfigurationManager.AppSettings["InvoiceMailSubject"].Trim();

            if (mailBody == "")
            {
                strMailBody = ConfigurationManager.AppSettings["InvoiceMailBody"].Trim();
            }
            else
            {
                strMailBody = mailBody;
            }

            strMailBody = strMailBody.Replace("@@INVNO@@", strInvoiceNo).Replace("@@APPROVALSTATUS@@", strApprovalType);

            SendEmail(strMailFrom, strMailTo, strCCMail, strBCCMail, strSubject, strMailBody);
        }
        #endregion

        #region SendEmail
        private void SendEmail(string _from, string _to, string _cc, string _bcc, string _subject, string _body)
        {
            try
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
            catch { }
        }
        #endregion

        #region GetApproverStatus
        public static string GetApproverStatus(int iInvoiceID)
        {
            RecordSet rs = GetInvoiceHead(iInvoiceID);
            string strApproverStatus = "";

            if (rs.RecordCount > 0)
            {
                rs.MoveFirst();

                while (!rs.EOF())
                {
                    if (Convert.ToInt32(rs["Approved1"]) == 1 || Convert.ToInt32(rs["Approved1SB1"]) == 1 || Convert.ToInt32(rs["Approved1SB2"]) == 1)
                    {
                        strApproverStatus = "Approver1";
                    }
                    else if (Convert.ToInt32(rs["Approved2_1"]) == 1 || Convert.ToInt32(rs["Approved2_2"]) == 1 || Convert.ToInt32(rs["Approved2SB1"]) == 1 || Convert.ToInt32(rs["Approved2SB2"]) == 1)
                    {
                        strApproverStatus = "Approver2";
                    }
                    else if (Convert.ToInt32(rs["ApprovedCEO"]) == 1)
                    {
                        strApproverStatus = "CEO";
                    }

                    rs.MoveNext();
                }
            }

            rs = null;

            return (strApproverStatus);
        }
        #endregion

        #region GetOverDueStatus
        public static bool GetOverDueStatus(int iInvoiceID, int iTimeLimitInHours)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("up_Invoice_Overdue_CN", iInvoiceID, iTimeLimitInHours);

            bool bOverDueFlag = false;

            if (rs.RecordCount > 0)
            {
                bOverDueFlag = true;
            }

            da.CloseConnection();
            rs = null;
            da = null;

            return (bOverDueFlag);
        }
        #endregion

        #region GetInvoiceStatusList
        public DataTable GetInvoiceStatusList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlDA = new SqlDataAdapter("stpGetInvoiceStatusList_CN", sqlConn);
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

        #region UpdateInvoiceDocument
        public bool UpdateInvoiceDocument(int iInvoiceID, string strFileName)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stpUpdateInvoiceDocument_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlCmd.Parameters.Add("@Document", strFileName);

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
        #region GetCurrentStatus
        public void GetCurrentStatus(int iStatusID, out string strStatus)
        {
            strStatus = "";

            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("InvCnStatus", "StatusID = " + iStatusID);

            while (!rs.EOF())
            {
                strStatus = rs["Status"].ToString();
                rs.MoveNext();
            }

            rs = null;
            da.CloseConnection();
            da = null;
        }
        #endregion
        #region GetCurrentStatus_CN
        public string GetCurrentStatus_CN(int iCreditNoteID)
        {
            string strStatusID = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetCurrentStatus_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);

            sqlOutputParam = sqlCmd.Parameters.Add("@StatusID", SqlDbType.VarChar, 3);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strStatusID = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strStatusID);
        }
        #endregion
        #region CheckPassedToUserID
        public int CheckPassedToUserID(int iInvoiceID, int iUserID)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stpCheckPassedToUserID_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iInvoiceID);
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

            return (iReturnValue);
        }
        #endregion
        #region GetTimeLimitForCompany
        public void GetTimeLimitForCompany(int iCompanyID, out int iTimeLimitInHours)
        {
            iTimeLimitInHours = 0;

            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("ThresHold", "CompanyID = " + iCompanyID);

            if (rs.RecordCount > 0)
            {
                while (!rs.EOF())
                {
                    if (rs["TimeLimit"] != DBNull.Value)
                        iTimeLimitInHours = Convert.ToInt32(rs["TimeLimit"]);

                    rs.MoveNext();
                }
            }
            else
            {
                iTimeLimitInHours = 48;
            }

            rs = null;
            da.CloseConnection();
            da = null;
        }
        #endregion
        #region CheckDuplicateInvoiceNo
        public bool CheckDuplicateInvoiceNo(string strInvoiceNo, int iSupplierCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckDuplicateInvoiceNo_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceNo", strInvoiceNo);
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
        #region CheckDuplicateBranchCode
        public bool CheckDuplicateBranchCode(string strBranchCode)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand("sp_CheckDuplicateBranchCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@BranchCode", strBranchCode);

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
        #region GetInvoiceComments
        public string GetInvoiceComments(int iInvoiceID)
        {
            RecordSet rs = GetInvoiceHead(iInvoiceID);
            string strComments = "";

            if (rs.RecordCount > 0)
            {
                rs.MoveFirst();

                while (!rs.EOF())
                {
                    if (rs["Comment"] != DBNull.Value)
                        strComments = rs["Comment"].ToString();
                    else
                        strComments = "";

                    rs.MoveNext();
                }
            }

            rs = null;

            return (strComments);
        }
        #endregion

        #region GetSupplierName
        public string GetSupplierName(int iInvoiceID)
        {
            string strSupplierName = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetSupplierName_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iInvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@SupplierName", SqlDbType.VarChar, 100);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strSupplierName = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strSupplierName);
        }
        #endregion
        #region GetPreviousStatusID
        public void GetPreviousStatusID(int iInvoiceID, out int iPreviousStatusID, out int iPreviousPassedToUserID)
        {
            iPreviousStatusID = 0;
            iPreviousPassedToUserID = 0;

            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("CreditNote", "CreditNoteID = " + iInvoiceID);

            while (!rs.EOF())
            {
                if (rs["PreviousStatusID"] != DBNull.Value)
                    iPreviousStatusID = Convert.ToInt32(rs["PreviousStatusID"]);

                if (rs["PreviousPassedToUserID"] != DBNull.Value)
                    iPreviousPassedToUserID = Convert.ToInt32(rs["PreviousPassedToUserID"]);

                rs.MoveNext();
            }

            rs = null;
            da.CloseConnection();
            da = null;
        }
        #endregion
        #region UpdatePreviousIDWhenUnderQuery_CN
        public bool UpdatePreviousIDWhenUnderQuery_CN(int iInvoiceID, int iPreviousStatusID, int iPreviousPassedToUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdatePreviousIDWhenUnderQuery_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iInvoiceID);
            sqlCmd.Parameters.Add("@StatusID", iPreviousStatusID);
            sqlCmd.Parameters.Add("@PassedToUserID", iPreviousPassedToUserID);

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
        #region GetUsersForCompany
        public DataTable GetUsersForCompany(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlDA = new SqlDataAdapter("sp_GetUsersForCompany", sqlConn);
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
        #region GetCompanyCodeForInvoice
        public string GetCompanyCodeForInvoice(int iInvoiceID)
        {
            string strCompanyCode = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetCompanyCodeForInvoice_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 150);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strCompanyCode = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {

                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strCompanyCode);
        }
        #endregion
        #region UpdateChangedCompanyCodeForInvoice
        public void UpdateChangedCompanyCodeForInvoice(int iInvoiceID, string strChangedCompanyCode)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateChangedCompanyCodeForInvoice_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

            if (strChangedCompanyCode.Trim() != "")
                sqlCmd.Parameters.Add("@ChangedCompanyCode", strChangedCompanyCode);
            else
                sqlCmd.Parameters.Add("@ChangedCompanyCode", DBNull.Value);
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
        #region GetStatusID
        public void GetStatusID(int iInvoiceID, out int iFirstStatus)
        {
            iFirstStatus = 0;

            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("CreditNote", "CreditNoteID = " + iInvoiceID);

            while (!rs.EOF())
            {
                if (rs["StatusID"] != DBNull.Value)
                    iFirstStatus = Convert.ToInt32(rs["StatusID"]);

                rs.MoveNext();
            }

            rs = null;
            da.CloseConnection();
            da = null;
        }
        #endregion
        #region UpdateDescriptionForInvoice
        public void UpdateDescriptionForInvoice(int iInvoiceID, string strDescription)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand("sp_UpdateDescriptionForInvoice_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlCmd.Parameters.Add("@Description", strDescription);
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
        #region GetInvoiceLogCommentsHistory
        public DataTable GetInvoiceLogCommentsHistory(int iInvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlDA = new SqlDataAdapter("sp_GetInvoiceLogCommentsHistory_CN", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", iInvoiceID);

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
        #region GetSuppliersList
        public DataTable GetSuppliersList(int iBuyerCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetSuppliersList_CN", sqlConn);
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
        #region GetStatusList
        public DataTable GetStatusList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetStatusList", sqlConn);
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
        #region GetUsersList
        public DataTable GetUsersList(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetUsersList", sqlConn);
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
        #region GetInvoiceNo
        public DataTable GetInvoiceNo(int iCompanyID, int iOption)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetInvoiceNo_CN", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
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

            return (ds.Tables[0]);
        }
        #endregion
        #region GetThresholdAmount
        public double GetThresholdAmount(int iBuyerCompanyID)
        {
            double dThresholdAmount = 0;

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetThresholdAmount", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);

            sqlOutputParam = sqlCmd.Parameters.Add("@ThresholdAmount", SqlDbType.Decimal);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                dThresholdAmount = Convert.ToDouble(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (dThresholdAmount);
        }
        #endregion
        #region UpdateInvoiceStatusLogApproverWise_CN
        public bool UpdateInvoiceStatusLogApproverWise_CN(int iInvoiceID, int iApproverID, int iPassedToUserID, int iUserTypeID, int iApproverStatusID, string strComments)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateInvoiceStatusLogApproverWise_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iInvoiceID);
            sqlCmd.Parameters.Add("@ApproverID", iApproverID);
            sqlCmd.Parameters.Add("@PassedToUserID", iPassedToUserID);
            sqlCmd.Parameters.Add("@UserTypeID", iUserTypeID);
            sqlCmd.Parameters.Add("@ApproverStatusID", iApproverStatusID);

            if (strComments.Trim() != "")
                sqlCmd.Parameters.Add("@Comments", strComments);
            else
                sqlCmd.Parameters.Add("@Comments", DBNull.Value);

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
        #region GetInvoiceLogStatusApproverWise
        public DataTable GetInvoiceLogStatusApproverWise(int iInvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetInvoiceLogStatusApproverWiseNL_CN", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@CreditNoteID", iInvoiceID);

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
        #region CompareUserIDForInvoice_CN
        public bool CompareUserIDForInvoice_CN(int iUserID, int iInvoiceID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CompareUserIDForInvoice_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@UserID", iUserID);
            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

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
        #region GetOldPassedToUserID_CN
        public int GetOldPassedToUserID_CN(int iInvoiceID)
        {
            int iOldPassedToUserID = 0;

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_GetOldPassedToUserID_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@OldPassedToUserID", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                iOldPassedToUserID = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iOldPassedToUserID);
        }
        #endregion
        #region CheckBuyerCompanyIDForInvoiceNo_CN
        public int CheckBuyerCompanyIDForInvoiceNo_CN(string strInvoiceNo, int iBuyerCompanyID)
        {
            int iReturnValue = 0;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckBuyerCompanyIDForInvoiceNo_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceNo", strInvoiceNo);
            sqlCmd.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);
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
        #region UpdateInvoiceNoForCreditNote
        public bool UpdateInvoiceNoForCreditNote(int iCreditNoteID, string strCreditInvoiceNo)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlConn.Open();

            sqlCmd = new SqlCommand("sp_UpdateInvoiceNoForCreditNote", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);
            sqlCmd.Parameters.Add("@CreditInvoiceNo", strCreditInvoiceNo);


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
        #region UpdateCreditNoteStatusAsCredited
        public bool UpdateCreditNoteStatusAsCredited(int iCreditNoteID, int iStatusID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateCreditNoteStatusAsCredited", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);
            sqlCmd.Parameters.Add("@StatusID", iStatusID);

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
        #region CheckApproverChainUserForCreditNote
        public bool CheckApproverChainUserForCreditNote(int iCreditNoteID, int iUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckApproverChainUserForCreditNote", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);
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
            if (iReturnValue == -101)
            {
                bRetVal = false;
            }



            return (bRetVal);
        }
        #endregion
        #region UpdateCodes_CN
        public bool UpdateCodes_CN(int iCreditNoteID, int iDepartmentID, int iProjectID, int iNominalCodeID, string strDescription)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateCodes_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);
            sqlCmd.Parameters.Add("@DepartmentID", iDepartmentID);
            sqlCmd.Parameters.Add("@ProjectID", iProjectID);
            sqlCmd.Parameters.Add("@NominalCodeID", iNominalCodeID);
            sqlCmd.Parameters.Add("@Description", strDescription);

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
        #region UpdateSterlingAmoutnIfCurrencyNotGBP
        public void UpdateSterlingAmoutnIfCurrencyNotGBP(int iCreditNoteID, decimal dSterlingVATAmount)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateSterlingAmountIfCurrencyNotGBP_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);
            sqlCmd.Parameters.Add("@AmountIfNotGBPCurrency", dSterlingVATAmount);


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
        #region UpdateVATAmount_CN
        public void UpdateVATAmount(int iCreditNoteID, decimal dVATAmount)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("usp_UpdateVATAmount_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);
            sqlCmd.Parameters.Add("@VATAmt", dVATAmount);


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
        #region GetGBPEquivalentAmount
        public decimal GetGBPEquivalentAmount(int iCreditNoteID)
        {
            decimal dGBPEquivalentAmount = 0;

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetGBPEquivalentAmount_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);

            sqlOutputParam = sqlCmd.Parameters.Add("@GBPEquivalentAmount", SqlDbType.Decimal);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                dGBPEquivalentAmount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (dGBPEquivalentAmount);
        }
        #endregion
        #region GetCreditNoteLogForSupplier_CN NEW COMMENTED & OLD ONE IS IN USE.
        public DataTable GetCreditNoteLogForSupplier_CN(int iSupplierCompanyID, int iModUserID, int iOption, int iBuyerCompanyID, int iInvoiceStatusID, string strInvoiceNo)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetCreditNoteLogForSupplier_CN", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);

            if (iBuyerCompanyID == 0)
                sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);

            if (iInvoiceStatusID == 0)
                sqlDA.SelectCommand.Parameters.Add("@DocStatus", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@DocStatus", iInvoiceStatusID);

            if (strInvoiceNo.Equals("0"))
                sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", DBNull.Value);
            else
                sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", strInvoiceNo);

            sqlDA.SelectCommand.Parameters.Add("@Option", iOption);
            sqlDA.SelectCommand.Parameters.Add("@ModUserID", iModUserID);

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
        #region CompareCreditNoteInvoiceCurrencyCode
        public bool CompareCreditNoteInvoiceCurrencyCode(string strInvoiceNo, string strCurrencyCode)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_Compare_CreditNoteInvoice_CurrencyCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteInvoiceNo", strInvoiceNo);
            sqlCmd.Parameters.Add("@CreditNoteCurrencyCode", strCurrencyCode);

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
                bRetVal = false;

            return (bRetVal);
        }
        #endregion
        #region GetInvoiceNoForSalesInvoice
        public DataTable GetCreditNoteNoForSalesInvoice(int iBuyerCompanyID, int iSupplierCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("usp_GetCreditNoteNoForSalesInvoice", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);

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
        // =========================================================================================================
        #region GetAssociatedInvoiceNo
        public string GetAssociatedInvoiceNo(int iCreditNoteID)
        {
            string strAssociatedInvoiceNo = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_GetAssociatedInvoiceNo", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);

            sqlOutputParam = sqlCmd.Parameters.Add("@CreditInvoiceNo", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strAssociatedInvoiceNo = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {

                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strAssociatedInvoiceNo);
        }
        #endregion
        // =========================================================================================================
        #region CompareCreditNoteAmountWithInvoiceAmount
        public int CompareCreditNoteAmountWithInvoiceAmount(string strInvoiceNo, decimal dCreditNoteAmount)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_CompareCreditNoteAmountWithInvoiceAmount", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceNo", strInvoiceNo);
            sqlCmd.Parameters.Add("@CreditNoteAmount", dCreditNoteAmount);

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
        // =========================================================================================================
        #region CheckExistenceOfInvoiceAgainstSupplierAndBuyer
        public int CheckExistenceOfInvoiceAgainstSupplierAndBuyer(string strInvoiceNo, string strSupplierNetWorkID, string strBuyerNetworkID)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_CheckExistenceOfInvoiceAgainstSupplierAndBuyer", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceNo", strInvoiceNo);
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
        // =========================================================================================================
        #region CompareTotalAmount_CNWithInvoiceTotalAmount
        public bool CompareTotalAmount_CNWithInvoiceTotalAmount(string strInvoiceNo, decimal dCreditNoteAmount)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_CompareCreditNoteAmountWithInvoiceAmount", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceNo", strInvoiceNo);
            sqlCmd.Parameters.Add("@CreditNoteAmount", dCreditNoteAmount);

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
                bRetVal = false;

            return (bRetVal);
        }
        #endregion

        #region GetDepartmentListDropDown
        public DataTable GetDepartmentListDropDown()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sGetDepartmentdetails", sqlConn);
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

        #region GetCurrencyName
        public string GetCurrencyName(int iCurrencyTypeID)
        {
            string strCurrencyName = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //		

            sqlCmd = new SqlCommand("sp_GetCurrencyName", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CurrencyTypeID", iCurrencyTypeID);

            sqlOutputParam = sqlCmd.Parameters.Add("@CurrencyName", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strCurrencyName = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strCurrencyName);
        }
        #endregion

        #region SaveInvoiceDatatoDataBase
        public int SaveInvoiceDatatoDataBase(int UserID, int InvoiceID, string strInvoiceNo, string strSupplierCompanyID, string strBuyerCompanyID,
            DateTime strInvoiceDate, DateTime strTaxPointDate, string strCurrency, string strSupplierAddress1,
            string strSupplierAddress2, string strSupplierAddress3, string strSupplierAddress4, string strSupplierAddress5,
            string strSupplierCountry, string strSupplierZIP, string strSellerVATNo, string strComments, string str22, decimal decNetTotal, decimal decVATAmt, decimal decTotalAmt, string strDeleteIDs)
        {
            string strExceptionMessage = "";
            int iReturnValue = 0;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand("usp_EditInvoiceTestBuyer_CN", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@UserID", Convert.ToInt32(UserID));
                sqlCmd.Parameters.Add("@strInvoiceID", Convert.ToInt32(InvoiceID));
                sqlCmd.Parameters.Add("@strInvoiceNo", strInvoiceNo);
                sqlCmd.Parameters.Add("@strSupplierCompanyID", strSupplierCompanyID);
                sqlCmd.Parameters.Add("@strBuyerCompanyID", strBuyerCompanyID);
                sqlCmd.Parameters.Add("@strInvoiceDate", strInvoiceDate);
                sqlCmd.Parameters.Add("@strTaxPointDate", strTaxPointDate);
                sqlCmd.Parameters.Add("@strCurrency", strCurrency);

                sqlCmd.Parameters.Add("@strSupplierAddress1", strSupplierAddress1);
                sqlCmd.Parameters.Add("@strSupplierAddress2", strSupplierAddress2);
                sqlCmd.Parameters.Add("@strSupplierAddress3", strSupplierAddress3);
                sqlCmd.Parameters.Add("@strSupplierAddress4", strSupplierAddress4);
                sqlCmd.Parameters.Add("@strSupplierAddress5", strSupplierAddress5);
                sqlCmd.Parameters.Add("@strSupplierCountry", strSupplierCountry);
                sqlCmd.Parameters.Add("@strSupplierZIP", strSupplierZIP);
                sqlCmd.Parameters.Add("@strSellerVATNo", strSellerVATNo);
                sqlCmd.Parameters.Add("@strComments", strComments);

                sqlCmd.Parameters.Add("@strNetTotal", decNetTotal);
                sqlCmd.Parameters.Add("@strVATAmt", decVATAmt);
                sqlCmd.Parameters.Add("@strTotalAmt", decTotalAmt);

                sqlCmd.Parameters.Add("@strDetailXml", str22);
                sqlCmd.Parameters.Add("@strDeleteIDs", strDeleteIDs);

                sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                sqlOutputParam.Direction = ParameterDirection.Output;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex)
            {
                strExceptionMessage = ex.Message.Trim();
                sqlOutputParam = null;

                //sqlCmd.Dispose(); 
                //sqlConn.Close(); 
            }
            finally
            {
                sqlOutputParam = null;

                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion

    }
}
