using System;
using System.Web.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

namespace CBSolutions.ETH.Web.ETC.Invoice
{
    /// <summary>
    /// Summary description for Invoice.
    /// </summary>
    public class Invoice
    {
        #region Mail variable declaration
        private MailFormat _mailFormat = MailFormat.Html;
        private MailPriority _mailPriority = MailPriority.High;
        #endregion
        // ==========================================================================================================		
        #region SqlClient's objects
        protected SqlCommand objComm = null;
        protected SqlConnection objConn = null;

        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected SqlCommand sqlCmd = null;
        protected SqlDataReader sqlDR = null;
        protected SqlParameter sqlReturnParam = null;
        protected SqlParameter sqlOutputParam = null;
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

        protected DataSet ds = null;
        #endregion
        // ==========================================================================================================		
        #region Variable declaration
        private string errorMessage = null;
        #endregion
        // ==========================================================================================================		
        #region Default Constructor
        public Invoice()
        {
        }
        #endregion
        // ==========================================================================================================
        #region Property Declaration
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }
        #endregion
        // ==========================================================================================================
        #region GetTradingPartnerList
        public static RecordSet GetTradingPartnerList(int supplierCompanyID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("vSupplierTradingRelationList", "SupplierCompanyID = " + System.Convert.ToString(supplierCompanyID));
            return rs;
        }
        #endregion
        // ==========================================================================================================
        #region GetCurrencyTypesList
        public static RecordSet GetCurrencyTypesList()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("CurrencyTypes", "", "CurrencyCode");
            return rs;
        }
        #endregion
        // ==========================================================================================================
        #region GetVATTypesList
        public static RecordSet GetVATTypesList()
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("VATTypes");
            return rs;
        }
        #endregion
        // ==========================================================================================================
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
        // ==========================================================================================================
        #region InsertInvoiceDetailData
        public Boolean InsertInvoiceDetailData(int invoiceID, RecordSet rs, DataAccess da)
        {
            errorMessage = "";
            rs.MoveFirst();
            int PKID = 0;
            while (!rs.EOF())
            {
                rs["InvoiceID"] = invoiceID;
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
        // ==========================================================================================================
        #region GetInvoiceHead
        public static RecordSet GetInvoiceHead(int invoiceID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("stpGetInvoiceHeader", invoiceID);
            return rs;
        }
        #endregion
        // ==========================================================================================================
        #region GetInvoiceDetail
        public static RecordSet GetInvoiceDetail(int invoiceID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("InvoiceDetail", "InvoiceID = " + invoiceID);
            return rs;
        }
        #endregion
        // ==========================================================================================================
        #region GetStatus
        //160505 SURAJIT
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
        //160505 SURAJIT
        #endregion
        // ==========================================================================================================
        #region XML related code

        #endregion
        // ==========================================================================================================
        #region GetCodingDescriptionList
        public DataTable GetCodingDescriptionList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("usp_GetCodingDescriptionList", sqlConn);
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
        // ==========================================================================================================
        #region GetDepartmentList
        public DataTable GetDepartmentList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

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
        // ==========================================================================================================
        #region GetProjectList
        public DataTable GetProjectList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

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
        // ==========================================================================================================
        #region GetNominalCodeList
        public DataTable GetNominalCodeList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

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
        // ==========================================================================================================
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
        // ==========================================================================================================
        #region GetDocumentType
        public string GetDocumentType(int BuyerID, int SupplierID)
        {
            string strDocumentType = "";
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sGetDocumentType_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@BuyerID", BuyerID);
            sqlCmd.Parameters.Add("@SupplierID", SupplierID);

            sqlOutputParam = sqlCmd.Parameters.Add("@DocumentType", SqlDbType.VarChar, 100);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strDocumentType = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (strDocumentType);
        }
        #endregion
        //===========================================================================================================
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

            /*
            if (mailBody == "")
            {
                strMailBody = ConfigurationManager.AppSettings["InvoiceMailBody"].Trim();
            }
            else
            {
            */
            strMailBody = mailBody;

            //}

            //strMailBody = strMailBody.Replace("@@INVNO@@", strInvoiceNo).Replace("@@APPROVALSTATUS@@", strApprovalType);

            SendEmail(strMailFrom, strMailTo, strCCMail, strBCCMail, strSubject, strMailBody);
        }
        #endregion
        // ==========================================================================================================
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
        // ==========================================================================================================
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
            return (strApproverStatus);
        } // up_Invoice_Overdue
        #endregion
        // ==========================================================================================================
        #region GetOverDueStatus
        public static bool GetOverDueStatus(int iInvoiceID, int iTimeLimitInHours)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("up_Invoice_Overdue", iInvoiceID, iTimeLimitInHours);

            bool bOverDueFlag = false;

            if (rs.RecordCount > 0)
            {
                bOverDueFlag = true;
            }

            rs = null;
            da.CloseConnection();
            da = null;

            return (bOverDueFlag);
        }
        #endregion
        // ==========================================================================================================
        #region GetInvoiceStatusList
        public DataTable GetInvoiceStatusList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("stpGetInvoiceStatusList", sqlConn);
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
        // ==========================================================================================================
        #region UpdateInvoiceDocument
        public bool UpdateInvoiceDocument(int iInvoiceID, string strFileName)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stpUpdateInvoiceDocument", sqlConn);
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
        // =========================================================================================================
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
        // =========================================================================================================
        #region CheckPassedToUserID
        public int CheckPassedToUserID(int iInvoiceID, int iUserID)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stpCheckPassedToUserID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
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
        // =========================================================================================================
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
            da.CloseConnection();
            rs = null;
            da = null;
        }
        #endregion
        // =========================================================================================================		
        #region CheckDuplicateInvoiceNo
        public bool CheckDuplicateInvoiceNo(string strInvoiceNo, int iSupplierCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckDuplicateInvoiceNo", sqlConn);
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
        // =========================================================================================================
        #region CheckDuplicateBranchCode
        public bool CheckDuplicateBranchCode(int iBranchID, string strBranchCode, int iCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = false;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckDuplicateBranchCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@BranchCode", strBranchCode);
            sqlCmd.Parameters.Add("@BranchID", iBranchID);
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

            if (iReturnValue == -101)
            {
                bRetVal = true;
            }

            return (bRetVal);
        }
        #endregion
        // =========================================================================================================
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
            return (strComments);
        }
        #endregion
        // =========================================================================================================
        #region GetInvoiceComments Commented
        /*
		public string GetInvoiceComments(int iInvoiceID)
		{
			RecordSet rs = GetInvoiceHead(iInvoiceID);
			string strComments = "";

			if (rs.RecordCount > 0 )
			{
				rs.MoveFirst();

				while (! rs.EOF())
				{
					if (rs["Comment"] != DBNull.Value)
						strComments = rs["Comment"].ToString();
					else
						strComments = "";

					rs.MoveNext();
				}
			}

			return (strComments);
		}
		*/
        #endregion
        // =========================================================================================================
        #region GetSupplierName
        public string GetSupplierName(int iInvoiceID)
        {
            string strSupplierName = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetSupplierName", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

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
        // =========================================================================================================
        #region GetAPCompanyID

        public string GetAPCompanyID(int InvoiceID, string Category)
        {
            string strApCompany = "";
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand("sGetAPCompany", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@Category", Category);

            sqlOutputParam = sqlCmd.Parameters.Add("@APCompanyID", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();

                sqlCmd.ExecuteNonQuery();
                strApCompany = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strApCompany);


        }
        #endregion
        //==========================================================================================================
        #region GetVatCode
        public string GetVatCode(int InvoiceID, string Category)
        {
            string strVatCode = "";
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_GetVatCode_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@Category", Category);

            sqlOutputParam = sqlCmd.Parameters.Add("@VatCode", SqlDbType.VarChar, 100);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();

                sqlCmd.ExecuteNonQuery();
                strVatCode = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strVatCode);
        }
        #endregion
        //===========================================================================================================
        #region GetOrderNo
        public DataSet GetData(int InvoiceID, string sType)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sGetOrderNo", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@Type", sType);

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


            return (ds);
        }
        #endregion
        //==========================================================================================================
        #region GetOrderNo
        public DataTable GetProdCode_Color(string InvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sGetOrderNo", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);

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
        //==========================================================================================================
        #region GetBuyerProdCode
        public DataTable GetBuyerProdCode(string OrderNo)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("usp_GetBuyerProdCode", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@OrderNo", OrderNo);

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
        //==========================================================================================================
        #region GetColor
        public DataTable GetColor(string BuyerProdCode)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("usp_GetColor", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@BuyerProdCode", BuyerProdCode);

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
        //==========================================================================================================
        #region GetPreviousStatusID
        public void GetPreviousStatusID(int iInvoiceID, out int iPreviousStatusID, out int iPreviousPassedToUserID)
        {
            iPreviousStatusID = 0;
            iPreviousPassedToUserID = 0;

            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Invoice", "InvoiceID = " + iInvoiceID);

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
        // =========================================================================================================
        #region UpdatePreviousIDWhenUnderQuery
        public bool UpdatePreviousIDWhenUnderQuery(int iInvoiceID, int iPreviousStatusID, int iPreviousPassedToUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdatePreviousIDWhenUnderQuery", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
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
        // =========================================================================================================
        #region GetUsersForCompany
        public DataTable GetUsersForCompany(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

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
        // =========================================================================================================
        #region GetCompanyCodeForInvoice
        public string GetCompanyCodeForInvoice(int iInvoiceID)
        {
            string strCompanyCode = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand("sp_GetCompanyCodeForInvoice", sqlConn);
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
        // =========================================================================================================
        #region UpdateChangedCompanyCodeForInvoice
        public void UpdateChangedCompanyCodeForInvoice(int iInvoiceID, string strChangedCompanyCode)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand("sp_UpdateChangedCompanyCodeForInvoice", sqlConn);
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
        // =========================================================================================================
        #region GetStatusID
        public void GetStatusID(int iInvoiceID, out int iFirstStatus)
        {
            iFirstStatus = 0;

            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Invoice", "InvoiceID = " + iInvoiceID);

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
        // =========================================================================================================
        #region UpdateDescriptionForInvoice
        public void UpdateDescriptionForInvoice(int iInvoiceID, string strDescription)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateDescriptionForInvoice", sqlConn);
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
        // =========================================================================================================
        #region GetInvoiceLogCommentsHistory
        public DataTable GetInvoiceLogCommentsHistory(int iInvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sp_GetInvoiceLogCommentsHistory", sqlConn);
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
        // =========================================================================================================
        #region GetSuppliersList
        public DataTable GetSuppliersList(int iBuyerCompanyID, int UserID, int UserTypeID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetSuppliersList_Akkeron", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);
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
        #region GetSuppliersList
        public DataTable GetSuppliersListNEW(int iBuyerCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sp_GetSuppliersListFromTradingRelationNEW", sqlConn);
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
        // =========================================================================================================
        #region GetStatusList
        public DataTable GetStatusList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

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
        // =========================================================================================================
        #region GetStatusListNL
        public DataTable GetStatusListNL()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("Sp_GetStatusList_AkkeronETC", sqlConn);
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
        //=============================================================================================================
        #region GetUsersList
        public DataTable GetUsersList(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

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
        // =========================================================================================================
        #region GetInvoiceNo_NL
        public DataTable GetInvoiceNo(int iCompanyID, int iOption, int iUserID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sp_GetInvoiceNoForCurrent_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@Option", iOption);
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
        // =========================================================================================================
        #region GetInvoiceNoForHistory
        public DataTable GetInvoiceNoForHistory(int iCompanyID, int iOption, int iUserID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sp_GetInvoiceNoForHistory_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@Option", iOption);
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
        // =========================================================================================================
        #region GetInvoiceNoForSalesInvoice
        public DataTable GetInvoiceNoForSalesInvoice(int iBuyerCompanyID, int iSupplierCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("usp_GetInvoiceNoForSalesInvoice", sqlConn);
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

        // =========================================================================================================

        // =========================================================================================================
        #region UpdateInvoiceStatusLogApproverWise
        public bool UpdateInvoiceStatusLogApproverWise(int iInvoiceID, int iApproverID, int iUserTypeID, int iApproverStatusID, string strComments, string sRetCode)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateInvoiceStatusLogApproverWise_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlCmd.Parameters.Add("@ApproverID", iApproverID);
            //sqlCmd.Parameters.Add("@PassedToUserID", iPassedToUserID);
            sqlCmd.Parameters.Add("@UserTypeID", iUserTypeID);
            sqlCmd.Parameters.Add("@ApproverStatusID", iApproverStatusID);

            if (strComments.Trim() != "")
                sqlCmd.Parameters.Add("@Comments", strComments);
            else
                sqlCmd.Parameters.Add("@Comments", DBNull.Value);

            if (sRetCode.Trim() != "")
            {
                sqlCmd.Parameters.Add("@RejectionCode", sRetCode.Trim());
            }
            else
            {
                sqlCmd.Parameters.Add("@RejectionCode", DBNull.Value.ToString());
            }

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
        // =========================================================================================================
        #region GetInvoiceLogStatusApproverWise
        public DataTable GetInvoiceLogStatusApproverWise(int iInvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();
            sqlDA = new SqlDataAdapter("sp_GetInvoiceLogStatusApproverWise_Generic_GMG", sqlConn);
            //	sqlDA = new SqlDataAdapter("sp_GetInvoiceLogStatusApproverWise_NL", sqlConn);
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

        // =========================================================================================================
        #region GetInvoiceLogStatusApproverWise
        public DataTable GetInvoiceLogStatusApproverWiseForSupplier(int iInvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();
            sqlDA = new SqlDataAdapter("sp_CheckUserTypeBySessionUserIDForSupplier_Generic_GMG", sqlConn);
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
        // =================================================================================================
        #region	private int GetCheckUserType(int iUserId)
        public int GetCheckUserType(int iUserId)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckUserTypeByUserIDForSupplier", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@UserID", iUserId);

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
        #region GetCurrentStatus
        public string GetCurrentStatus(int iInvoiceID)
        {
            string strStatusID = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetCurrentStatus", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

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
        // =========================================================================================================
        #region CompareUserIDForInvoice
        public bool CompareUserIDForInvoice(int iUserID, int iInvoiceID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CompareUserIDForInvoice", sqlConn);
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
        // =========================================================================================================
        #region GetOldPassedToUserID
        public int GetOldPassedToUserID(int iInvoiceID)
        {
            int iOldPassedToUserID = 0;

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_GetOldPassedToUserID", sqlConn);
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
        // =========================================================================================================
        #region CheckCreditNoteCreditedForInvoice
        public int CheckCreditNoteCreditedForInvoice(int iInvoiceID)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckCreditNoteCreditedForInvoice", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

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

            return (iReturnValue);
        }
        #endregion
        // =========================================================================================================		
        #region UpdateInvoiceStatusAsDebited
        public bool UpdateInvoiceStatusAsDebited(int iInvoiceID, int iStatusID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateInvoiceStatusAsDebited", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
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
        // =========================================================================================================
        #region CheckApproverChainUser
        public bool CheckApproverChainUser(int iInvoiceID, int iUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckApproverChainUser", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
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
        // =========================================================================================================
        #region UpdateCodes
        public bool UpdateCodes(int iInvoiceID, int iDepartmentID, int iProjectID, int iNominalCodeID, string strDescription)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateCodes", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
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
        // =========================================================================================================
        #region GetCurrencyName
        public string GetCurrencyName(int iCurrencyTypeID)
        {
            string strCurrencyName = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


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
        // =========================================================================================================
        #region UpdateSterlingAmoutnIfCurrencyNotGBP
        public void UpdateSterlingAmoutnIfCurrencyNotGBP(int iInvoiceID, decimal dSterlingVATAmount)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateSterlingAmoutnIfCurrencyNotGBP", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
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
        // =========================================================================================================
        #region UpdateVATAmount
        public void UpdateVATAmount(int iInvoiceID, decimal dVATAmount)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_UpdateVATAmount", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
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
        // =========================================================================================================
        #region GetGBPEquivalentAmount
        public Double GetGBPEquivalentAmount(int iInvoiceID)
        {
            Double dGBPEquivalentAmount = 0;

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetGBPEquivalentAmount", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@GBPEquivalentAmount", SqlDbType.Float);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                dGBPEquivalentAmount = Convert.ToDouble(sqlOutputParam.Value);
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
        // =========================================================================================================
        #region GetCurrencyCode
        public string GetCurrencyCode(int iCurrencyTypeID)
        {
            string strCurrencyCode = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetCurrencyCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CurrencyTypeID", iCurrencyTypeID);

            sqlOutputParam = sqlCmd.Parameters.Add("@CurrencyCode", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strCurrencyCode = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (strCurrencyCode);
        }
        #endregion
        // =========================================================================================================
        #region SaveXMLImportInvoices METHOD OVERLOAD
        // =========================================================================================================
        #region SaveXMLImportInvoices
        public int SaveXMLImportInvoices(string strSenderHubId, string strRecipientHubId, /*string strDocumentType,*/
            string strInvoiceNumber, string strInvoiceDate, /*string strInvoiceName,*/
            string strInvoiceAddress1,
            string strLineNo, string strDescription, decimal dQuantity, decimal dPrice)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_SaveXMLImportInvoices", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@SenderHubId", strSenderHubId);
            sqlCmd.Parameters.Add("@RecipientHubId", strRecipientHubId);
            //sqlCmd.Parameters.Add("@DocumentType", strDocumentType);
            sqlCmd.Parameters.Add("@InvoiceNumber", strInvoiceNumber);
            sqlCmd.Parameters.Add("@InvoiceDate", strInvoiceDate);
            //sqlCmd.Parameters.Add("@InvoiceName", strInvoiceName);
            sqlCmd.Parameters.Add("@InvoiceAddress1", strInvoiceAddress1);

            if (strLineNo.Trim() != "")
                sqlCmd.Parameters.Add("@LineNo", strLineNo);
            else
                sqlCmd.Parameters.Add("@LineNo", DBNull.Value);

            sqlCmd.Parameters.Add("@Description", strDescription);
            sqlCmd.Parameters.Add("@Quantity", dQuantity);
            sqlCmd.Parameters.Add("@Price", dPrice);

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
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
        // =========================================================================================================
        #region SaveXMLImportInvoices
        public int SaveXMLImportInvoices(string strSenderHubID, string strRecipientHubID, string strDocType,
            string strInvoiceNumber, string strInvoiceDate, string strInvoiceName,
            string strInvoiceAddress1, string strInvoiceAddress2, string strInvoiceAddress3, string strInvoiceAddress4,
            string strInvoicePostcode, string strInvoiceCountryCode, string strInvoiceContact,
            string strDeliveryAccount, string strDeliveryName,
            string strDeliveryAddress1, string strDeliveryAddress2, string strDeliveryAddress3, string strDeliveryAddress4,
            string strDeliveryPostcode, string strDeliveryCountryCode,
            string strDespatchContact, string strTerms, string strDueDate, string strCurrencyCode,
            string strTaxCountryCode1, string strTradersReference, string strTaxCountryNumber, string strTaxRegistrationNumber,
            string strOverallDiscountPercent, string strOverallDiscountAmount, string strNettAmount,
            string strSettlementDays1, string strSettlementPercent1, string strSettlementAmount1,
            string strSettlementDays2, string strSettlementPercent2, string strSettlementAmount2,
            string strTaxAmount, string strGBPTaxAmount, string strGrossAmount, string strInvoiceTaxPointDate,

            string strLineNo, string strOrderNo, string strOrderLineNo, string strOrderDate,
            string strManufacturerCode, string strBuyerCode,
            string strDespatchNoteNumber, string strDespatchDate,
            string strDescription, string strQuantity,
            string strUnitofMeasure, string strPrice, string strExtendedValue,
            string strDiscountPercent1, string strDiscountValue1,
            string strDiscountPercent2, string strDiscountValue2,
            string strPostDiscountValue, string strOverallDiscountValue, string strNettValue,
            string strTaxCode, string strTaxPercent, string strTaxValue, string strGrossValue,
            string strModeOfTransport, string strNatureOfTransaction, string strTermsOfDelivery,
            string strCountryOfOrigin, string strCommodityCode, string strSupplementaryUnits,
            string strNettMass,
            string strType, string strDefinable1, string strDefinable2)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_SaveXMLImportInvoices", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            // **************************************************************************************
            sqlCmd.Parameters.Add("@SenderHubId", strSenderHubID);
            sqlCmd.Parameters.Add("@RecipientHubId", strRecipientHubID);
            sqlCmd.Parameters.Add("@DocumentType", strDocType);

            sqlCmd.Parameters.Add("@InvoiceNumber", strInvoiceNumber);

            if (strInvoiceDate.Equals(""))
                sqlCmd.Parameters.Add("@InvoiceDate", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@InvoiceDate", strInvoiceDate);

            sqlCmd.Parameters.Add("@InvoiceName", strInvoiceName);

            sqlCmd.Parameters.Add("@InvoiceAddress1", strInvoiceAddress1);
            sqlCmd.Parameters.Add("@InvoiceAddress2", strInvoiceAddress2);
            sqlCmd.Parameters.Add("@InvoiceAddress3", strInvoiceAddress3);
            sqlCmd.Parameters.Add("@InvoiceAddress4", strInvoiceAddress4);
            sqlCmd.Parameters.Add("@InvoicePostcode", strInvoicePostcode);
            sqlCmd.Parameters.Add("@InvoiceCountryCode", strInvoiceCountryCode);
            sqlCmd.Parameters.Add("@InvoiceContact", strInvoiceContact);

            sqlCmd.Parameters.Add("@DeliveryAccount", strDeliveryAccount);
            sqlCmd.Parameters.Add("@DeliveryName", strDeliveryName);
            sqlCmd.Parameters.Add("@DeliveryAddress1", strDeliveryAddress1);
            sqlCmd.Parameters.Add("@DeliveryAddress2", strDeliveryAddress2);
            sqlCmd.Parameters.Add("@DeliveryAddress3", strDeliveryAddress3);
            sqlCmd.Parameters.Add("@DeliveryAddress4", strDeliveryAddress4);
            sqlCmd.Parameters.Add("@DeliveryPostcode", strDeliveryPostcode);
            sqlCmd.Parameters.Add("@DeliveryCountryCode", strDeliveryCountryCode);

            sqlCmd.Parameters.Add("@DespatchContact", strDespatchContact);
            sqlCmd.Parameters.Add("@Terms", strTerms);

            if (strDueDate.Equals(""))
                sqlCmd.Parameters.Add("@DueDate", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@DueDate", strDueDate);

            sqlCmd.Parameters.Add("@CurrencyCode", strCurrencyCode);

            sqlCmd.Parameters.Add("@TaxCountryCode1", strTaxCountryCode1);
            sqlCmd.Parameters.Add("@TradersReference", strTradersReference);
            sqlCmd.Parameters.Add("@TaxCountryNumber", strTaxCountryNumber);
            sqlCmd.Parameters.Add("@TaxRegistrationNumber", strTaxRegistrationNumber);

            sqlCmd.Parameters.Add("@OverallDiscountPercent", strOverallDiscountPercent);
            sqlCmd.Parameters.Add("@OverallDiscountAmount", strOverallDiscountAmount);
            sqlCmd.Parameters.Add("@NettAmount", strNettAmount);

            sqlCmd.Parameters.Add("@SettlementDays1", strSettlementDays1);
            sqlCmd.Parameters.Add("@SettlementPercent1", strSettlementPercent1);
            sqlCmd.Parameters.Add("@SettlementAmount1", strSettlementAmount1);
            sqlCmd.Parameters.Add("@SettlementDays2", strSettlementDays2);
            sqlCmd.Parameters.Add("@SettlementPercent2", strSettlementPercent2);
            sqlCmd.Parameters.Add("@SettlementAmount2", strSettlementAmount2);

            sqlCmd.Parameters.Add("@TaxAmount", strTaxAmount);
            sqlCmd.Parameters.Add("@GBPTaxAmount", strGBPTaxAmount);
            sqlCmd.Parameters.Add("@GrossAmount", strGrossAmount);

            if (strInvoiceTaxPointDate.Equals(""))
                sqlCmd.Parameters.Add("@InvoiceTaxPointDate", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@InvoiceTaxPointDate", strInvoiceTaxPointDate);

            // INVOICE DETAIL PARAMETERS.
            sqlCmd.Parameters.Add("@LineNo", strLineNo);
            sqlCmd.Parameters.Add("@OrderNo", strOrderNo);
            sqlCmd.Parameters.Add("@OrderLineNo", strOrderLineNo);
            sqlCmd.Parameters.Add("@OrderDate", strOrderDate);
            sqlCmd.Parameters.Add("@ManufacturerCode", strManufacturerCode);
            sqlCmd.Parameters.Add("@BuyerCode", strBuyerCode);
            sqlCmd.Parameters.Add("@DespatchNoteNumber", strDespatchNoteNumber);
            sqlCmd.Parameters.Add("@DespatchDate", strDespatchDate);
            sqlCmd.Parameters.Add("@Description", strDescription);
            sqlCmd.Parameters.Add("@Quantity", strQuantity);
            sqlCmd.Parameters.Add("@UnitofMeasure", strUnitofMeasure);
            sqlCmd.Parameters.Add("@Price", strPrice);
            sqlCmd.Parameters.Add("@ExtendedValue", strExtendedValue);
            sqlCmd.Parameters.Add("@DiscountPercent1", strDiscountPercent1);
            sqlCmd.Parameters.Add("@DiscountValue1", strDiscountValue1);
            sqlCmd.Parameters.Add("@DiscountPercent2", strDiscountPercent2);
            sqlCmd.Parameters.Add("@DiscountValue2", strDiscountValue2);
            sqlCmd.Parameters.Add("@PostDiscountValue", strPostDiscountValue);
            sqlCmd.Parameters.Add("@OverallDiscountValue", strOverallDiscountValue);
            sqlCmd.Parameters.Add("@NettValue", strNettValue);
            sqlCmd.Parameters.Add("@TaxCode", strTaxCode);
            sqlCmd.Parameters.Add("@TaxPercent", strTaxPercent);
            sqlCmd.Parameters.Add("@TaxValue", strTaxValue);
            sqlCmd.Parameters.Add("@GrossValue", strGrossValue);
            sqlCmd.Parameters.Add("@ModeOfTransport", strModeOfTransport);
            sqlCmd.Parameters.Add("@NatureOfTransaction", strNatureOfTransaction);
            sqlCmd.Parameters.Add("@TermsOfDelivery", strTermsOfDelivery);
            sqlCmd.Parameters.Add("@CountryOfOrigin", strCountryOfOrigin);
            sqlCmd.Parameters.Add("@CommodityCode", strCommodityCode);
            sqlCmd.Parameters.Add("@SupplementaryUnits", strSupplementaryUnits);
            sqlCmd.Parameters.Add("@NettMass", strNettMass);

            sqlCmd.Parameters.Add("@Type", strType);
            sqlCmd.Parameters.Add("@Definable1", strDefinable1);
            sqlCmd.Parameters.Add("@Definable2", strDefinable2);
            // **************************************************************************************
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
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
        // =========================================================================================================
        #endregion SaveXMLImportInvoices METHOD OVERLOADED
        // =========================================================================================================
        #region GetSalesInvoiceLogForSupplier
        public DataTable GetSalesInvoiceLogForSupplier(int iSupplierCompanyID, int iBuyerCompanyID,
            int iInvoiceStatusID, string strInvoiceNo, int iOption, int iInvoiceCreatedBy)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("usp_GetSalesInvoiceLogForSupplier", sqlConn);
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
            sqlDA.SelectCommand.Parameters.Add("@InvoiceCreatedBy", iInvoiceCreatedBy);

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
        #region GetDepartmentListDropDown
        public DataTable GetDepartmentListDropDown(int iOption, int iUserID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sGetDepartmentdetails", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@Option", iOption);
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
        // =========================================================================================================
        #region CheckLogin
        public bool CheckLoginInfo(string UserName, string Password, string NetworkID)
        {

            int iReturn = 0;


            CBSAppUtils.PrimaryConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("CheckNewLookLogin", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@UserName", UserName);
            sqlCmd.Parameters.Add("@Password", Password);
            sqlCmd.Parameters.Add("@NetworkID", NetworkID);

            sqlReturnParam = sqlCmd.Parameters.Add("@flag", SqlDbType.Int, 10);
            sqlReturnParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                iReturn = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();

                sqlConn.Close();
            }

            if (iReturn == 1)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        #endregion
        // =========================================================================================================
        #region GetUserID

        public int GetUserID(string UserName, string Password)
        {

            CBSAppUtils.PrimaryConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("GetNewLookUserID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@UserName", UserName);
            sqlCmd.Parameters.Add("@Password", Password);

            sqlReturnParam = sqlCmd.Parameters.Add("@UserID", SqlDbType.Int, 10);
            sqlReturnParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlConn.Close();
            }
            return Int32.Parse(sqlCmd.Parameters["@UserID"].Value.ToString());


        }

        #endregion
        // =========================================================================================================
        #region GetNewLookCompanies
        public DataTable GetNewLookCompanyList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sGetNewLookCompanies", sqlConn);
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
        // =========================================================================================================
        #region GetUpdate
        public string GetUpdate(int InvoiceID, int InvoiceDetailID, string OrderNo, string InvoiceNo, string ProductCode, string InvoiceType, string Colour, int BuyerCompanyID)
        {
            string iReturnValue = "";
            //bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sGetStockUpdate", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@InvoiceDetailID", InvoiceDetailID);
            sqlCmd.Parameters.Add("@OrderNo", OrderNo);
            sqlCmd.Parameters.Add("@InvoiceNo", InvoiceNo);
            if (ProductCode.Trim() != "")
            {
                sqlCmd.Parameters.Add("@ProductCode", ProductCode);
            }
            else
            {
                sqlCmd.Parameters.Add("@ProductCode", DBNull.Value);
            }


            sqlCmd.Parameters.Add("@InvoiceType", InvoiceType);
            if (Colour.Trim() != "")
            {
                sqlCmd.Parameters.Add("@Colour", Colour);
            }
            else
            {
                sqlCmd.Parameters.Add("@Colour", DBNull.Value);
            }
            sqlCmd.Parameters.Add("@BuyerID", BuyerCompanyID);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.VarChar);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToString(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            if (Utility.IsNumeric(iReturnValue))
            {
                if (Convert.ToInt32(iReturnValue) == 0)
                {
                    return "";
                }
                else
                {
                    return iReturnValue;
                }
            }
            else
            {
                return "";
            }
        }

        #endregion
        // =========================================================================================================
        #region GetCreditNoteUpdate
        public bool GetCreditNoteUpdate(int CreditNoteID, string OrderNo, string CreditNoteNo, string ProductCode, string InvoiceType, string Colour, int BuyerCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sGetStockCreditNoteUpdate_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", CreditNoteID);
            sqlCmd.Parameters.Add("@OrderNo", OrderNo);
            sqlCmd.Parameters.Add("@InvoiceNo", CreditNoteNo);
            sqlCmd.Parameters.Add("@ProductCode", ProductCode);
            sqlCmd.Parameters.Add("@InvoiceType", InvoiceType);
            sqlCmd.Parameters.Add("@Colour", Colour);
            sqlCmd.Parameters.Add("@BuyerID", BuyerCompanyID);
            //sqlCmd.Parameters.Add("@SupplierID",SupplierCompanyID);

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

            //			if (iReturnValue == -101)
            //			{
            //				bRetVal = false;
            //			}

            return (bRetVal);
        }

        #endregion
        // =========================================================================================================
        #region GetUpdateDebitNote
        public bool GetUpdateDebitNote(int DebiteNoteID, string OrderNo, string DebiteNoteNo, string ProductCode, string InvoiceType, string Colour, int BuyerCompanyID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_UpdateDebiteNote_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", DebiteNoteID);
            sqlCmd.Parameters.Add("@OrderNo", OrderNo);
            sqlCmd.Parameters.Add("@InvoiceNo", DebiteNoteNo);
            sqlCmd.Parameters.Add("@ProductCode", ProductCode);
            sqlCmd.Parameters.Add("@InvoiceType", InvoiceType);
            sqlCmd.Parameters.Add("@Colour", Colour);
            sqlCmd.Parameters.Add("@BuyerID", BuyerCompanyID);
            //sqlCmd.Parameters.Add("@SupplierID",SupplierCompanyID);


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
        // =========================================================================================================
        #region Deleteinvoicecreditnote
        public bool Deleteinvoicecreditnote(int InvoiceID, string Type)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sGetStockDelete_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@Type", Type);

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
        // =========================================================================================================
        #region GetSupplierName

        public string GetSupplierName(int iInvoiceID, string DocType)
        {
            string strSupplierName = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetSupplierNameNL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlCmd.Parameters.Add("@DocType", DocType);

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
        // =========================================================================================================
        #region GetDepartment

        public string GetDepartment(int iInvoiceID, string DocType)
        {
            string strDepartmentName = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetDepartmentNameNL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlCmd.Parameters.Add("@DocType", DocType);

            sqlOutputParam = sqlCmd.Parameters.Add("@DepartmentName", SqlDbType.VarChar, 100);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strDepartmentName = sqlOutputParam.Value.ToString();

            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
                sqlOutputParam = null;
            }
            return (strDepartmentName);
        }


        #endregion
        // =========================================================================================================
        #region GetLineInformation_NL
        public DataTable GetLineInformation(int InvoiceID, string DocType)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("GetLineInformation_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);

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
        #region GetAuthorisationString
        public string GetAuthorisationString(int iInvoiceID, string DocType)
        {
            string strAutorisationString = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetAuthString_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlCmd.Parameters.Add("@DocType", DocType);

            sqlOutputParam = sqlCmd.Parameters.Add("@AuthorisationString", SqlDbType.VarChar, 100);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strAutorisationString = sqlOutputParam.Value.ToString();
            }

            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }


            return (strAutorisationString);
        }

        #endregion

        #region GetAuthorisationStringToolTips
        public string GetAuthorisationStringToolTips(string sqlString)
        {
            string strAutorisationStringToolTips = "";
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand(sqlString, sqlConn);
            sqlCmd.CommandType = CommandType.Text;
            try
            {
                sqlConn.Open();
                strAutorisationStringToolTips = Convert.ToString(sqlCmd.ExecuteScalar());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (strAutorisationStringToolTips);
        }

        #endregion

        #region GetAssociatedCreditInvoiceNo
        public string GetAssociatedCreditInvoiceNo(int iInvoiceID, string DocType)
        {
            string strAssociatedInvNo = "";

            SqlParameter sqlOutputParam = null;
            if (DocType == "INV")
            {
                //strAssociatedInvNo="Invoice Type";
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


                sqlCmd = new SqlCommand("sp_GetCreditNoteNoAgainstInvoiceID", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
                //sqlCmd.Parameters.Add("@DocType", DocType);

                sqlOutputParam = sqlCmd.Parameters.Add("@CreditNoteNo", SqlDbType.VarChar, 50);
                sqlOutputParam.Direction = ParameterDirection.Output;
                try
                {
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();

                    strAssociatedInvNo = sqlOutputParam.Value.ToString();
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
                finally
                {
                    sqlOutputParam = null;
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
            }
            else
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                //				sqlConn.Open();

                sqlCmd = new SqlCommand("sp_GetInvoiceNoForCreditNote_NL", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@CreditNoteID", iInvoiceID);
                //sqlCmd.Parameters.Add("@DocType", DocType);

                sqlOutputParam = sqlCmd.Parameters.Add("@CreditInvoiceNo", SqlDbType.VarChar, 100);
                sqlOutputParam.Direction = ParameterDirection.Output;

                try
                {
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();

                    strAssociatedInvNo = sqlOutputParam.Value.ToString();
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
                finally
                {
                    sqlOutputParam = null;
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
            }

            return (strAssociatedInvNo);
        }

        #endregion
        #region GetRelationType
        public string GetRelationType(int InvoiceID)
        {
            string strRelationType = "";
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_GetRelationType_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@RelationType", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strRelationType = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (strRelationType);
        }
        #endregion
        #region UpdateCrnStatusToDelete
        public int UpdateCrnStatusToDelete(int CreditNoteID)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_UpdateCNStatusToDelete_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", CreditNoteID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Count", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }

        #endregion
        #region GetUserType

        public int GetUserType(int UserID)
        {
            int UserTypeID = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetUserTypeID_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@UserID", UserID);

            sqlOutputParam = sqlCmd.Parameters.Add("@UserTypeID", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                UserTypeID = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (UserTypeID);
        }
        #endregion
        #region UpdateInvStatusToDelete

        public int UpdateInvStatusToDelete(int InvoiceID)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_UpdateInvStatusToDelete_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Count", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }
        #endregion
        #region UpdateApproveInvoiceByAPAdmin
        public int UpdateApproveByAPAdmin(int InvoiceID, string New_AccountCategory, string New_ActivityCode)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlCommand sqlCmd = new SqlCommand("stp_UpdateInvStatusToApproveAPAdmin_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            if (New_AccountCategory != "")
            {
                sqlCmd.Parameters.Add("@New_AccountCategory", New_AccountCategory);
            }
            else
            {
                sqlCmd.Parameters.Add("@New_AccountCategory", DBNull.Value);
            }

            if (New_ActivityCode != "")
            {
                sqlCmd.Parameters.Add("@New_ActivityCode", New_ActivityCode);
            }
            else
            {
                sqlCmd.Parameters.Add("@New_ActivityCode", DBNull.Value);
            }
            sqlOutputParam = sqlCmd.Parameters.Add("@Count", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }
        #endregion

        #region UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve
        public int UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(int InvoiceID, string strCreditNoteNo)
        {
            int ReturnVal = 0;
            SqlParameter sqlReturnParam = null;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                ReturnVal = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
                sqlReturnParam = null;
            }
            return (ReturnVal);
        }
        #endregion

        #region UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen  //amitava 300707
        public int UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen(int InvoiceID, string strCreditNoteNo)
        {
            int ReturnVal = 0;
            SqlParameter sqlReturnParam = null;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                ReturnVal = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
                sqlReturnParam = null;
            }
            return (ReturnVal);
        }
        #endregion

        #region DeleteCreditInvoiceNOByCreditNoteiD  //amitava 300707
        public int DeleteCreditInvoiceNOByCreditNoteiD(int iCreditNoteId)
        {
            int ReturnVal = 0;
            SqlParameter sqlReturnParam = null;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_DeleteCreditInvoiceNOByCreditNoteiD", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CreditNoteId", iCreditNoteId);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                ReturnVal = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
                sqlReturnParam = null;
            }
            return (ReturnVal);
        }
        #endregion

        // =========================================================================================================
        #region UpdateApproveCreditNoteByAPAdmin

        public int UpdateApproveCreditNoteByAPAdmin(int InvoiceID)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_UpdateCreditNoteStatusToApproveAPAdmin_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Count", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }
        #endregion
        // =========================================================================================================
        #region UpdateInvoiceReopenAPAdmin
        public int UpdateInvoiceReopenAPAdmin(int InvoiceID, int CurrentCreditNoteID)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_UpdateInvStatusToReopenAPAdmin_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@CreditNoteID", CurrentCreditNoteID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Count", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }
        #endregion
        // =========================================================================================================
        #region GetActivityCode
        public DataTable GetActivityCode(int sUserID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();
            sqlDA = new SqlDataAdapter("stpGetActivityCode_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@UserID", sUserID);
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
        #region GetAccountCategory
        public DataTable GetAccountCategory(int sUserID, int iActivityCode)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("stpGetAccountCategory_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@UserID", sUserID);
            sqlDA.SelectCommand.Parameters.Add("@ActivityCode", iActivityCode);

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
        #region GetApprovedStatus
        public int GetApprovedStatus(int InvoiceID, int UserID, string New_AccountCategory, string New_ActivityCode)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_ApproverStatusforUser_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            if (New_AccountCategory != "")
            {
                sqlCmd.Parameters.Add("@New_AccountCategory", New_AccountCategory);
            }
            else
            {
                sqlCmd.Parameters.Add("@New_AccountCategory", DBNull.Value);
            }

            if (New_ActivityCode != "")
            {
                sqlCmd.Parameters.Add("@New_ActivityCode", New_ActivityCode);
            }
            else
            {
                sqlCmd.Parameters.Add("@New_ActivityCode", DBNull.Value);
            }

            sqlCmd.Parameters.Add("@UserID", UserID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Type", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }
        #endregion
        // =========================================================================================================
        #region UpdateStatusToReject
        public int UpdateStatusToReject(int InvoiceID, string RejectionCode, string RejectionComment, int RejectionCodeID, int iUserID, string Comments)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_UpdateInvoiceToReject_Generic", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@RejectionCode", RejectionCode);
            sqlCmd.Parameters.Add("@RejectionComment", RejectionComment);
            sqlCmd.Parameters.Add("@RejectionCodeID", RejectionCodeID);
            sqlCmd.Parameters.Add("@UserID", iUserID);
            sqlCmd.Parameters.Add("@Comments", Comments);

            sqlOutputParam = sqlCmd.Parameters.Add("@Type", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }
        #endregion
        // =========================================================================================================

        #region CheckCreditNoteAmountAndCurrencyByInvoiceID

        public int CheckCreditNoteAmountAndCurrencyByInvoiceID(int InvoiceID)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_CheckCreditNoteAmountAndCurrencyByInvoiceID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Flag", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }
        #endregion
        // =========================================================================================================
        #region GetRejectionCodeID

        public int GetRejectionCodeID(int InvoiceID)
        {
            int iCode = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_GetRejectionCodeID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Flag", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCode = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCode);
        }
        #endregion
        // =========================================================================================================
        #region UpdateInvoiceStatusLogApproverWise_CN
        public bool UpdateInvoiceStatusLogApproverWise_CN(int iInvoiceID, int iApproverID, int iUserTypeID, int iApproverStatusID, string strComments)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateInvoiceStatusLogApproverWiseNL_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CreditNoteID", iInvoiceID);
            sqlCmd.Parameters.Add("@ApproverID", iApproverID);
            //sqlCmd.Parameters.Add("@PassedToUserID", iPassedToUserID);
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
        // =========================================================================================================
        #region GetCrediteLogStatusApproverWise
        public DataTable GetCrediNoteLogStatusApproverWise(int iInvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

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
        // =========================================================================================================

        // =========================================================================================================
        #region UpdateDebitNoteStatusToDelete
        public int UpdateDebitNoteStatusToDelete(int DebitNoteID)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_UpdateDebitNoteStatusToDelete_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@DebitNoteID", DebitNoteID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Count", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCount);
        }
        #endregion
        // =========================================================================================================
        #region UpdateInvoiceStatusLogApproverWise_DN
        public bool UpdateInvoiceStatusLogApproverWise_DN(int iInvoiceID, int iApproverID, int iUserTypeID, int iApproverStatusID, string strComments)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_UpdateInvoiceStatusLogApproverWiseDN_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteID", iInvoiceID);
            sqlCmd.Parameters.Add("@ApproverID", iApproverID);
            //sqlCmd.Parameters.Add("@PassedToUserID", iPassedToUserID);
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
        // =========================================================================================================
        #region GetDebitNoteLogStatusApproverWise
        public DataTable GetDebitNoteLogStatusApproverWise(int iInvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("sp_GetInvoiceLogStatusApproverWiseDN_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@DebitNoteID", iInvoiceID);

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
        #region GetInvoiceStatus
        public int GetInvoiceStatus(int iInvoiceID)
        {
            int iStatusID = 0;
            string strStatusID = "";

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetCurrentStatus", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@StatusID", SqlDbType.VarChar, 3);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlCmd.ExecuteNonQuery();

                strStatusID = sqlOutputParam.Value.ToString();
                iStatusID = Convert.ToInt32(strStatusID);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return iStatusID;
        }
        #endregion
        // =========================================================================================================
        #region GetDebitNoteHeadDetail
        public static RecordSet GetDebitNoteHeadDetail(int iDebitNoteID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("usp_GetInvoiceConfirmationNL_DN", iDebitNoteID);
            return rs;
        }
        #endregion
        // =========================================================================================================
        #region GetDocumentNoByDocID
        public int GetDocumentNoByDocID(int iDocID, string strDocType, out string strDocNo)
        {
            int iReturnValue = 0;
            strDocNo = "";

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetDocumentNoByDocID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@iDocID", iDocID);
            sqlCmd.Parameters.Add("@strDocType", strDocType);

            sqlOutputParam = sqlCmd.Parameters.Add("@strDocNo", SqlDbType.VarChar, 50);
            sqlOutputParam.Direction = ParameterDirection.Output;

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                strDocNo = Convert.ToString(sqlOutputParam.Value);
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
        // =========================================================================================================
        #region GetIsOpened COMMENTED
        //		public int GetIsOpened(int iDocID,int iUserID,int iUserTypeID,string strDocType)
        //		{
        //			int IsOpened=0;
        //			SqlParameter sqlOutputParam = null;
        //
        //			sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
        //			sqlConn.Open();
        //
        //			sqlCmd = new SqlCommand("sp_GetIsOpened_NL",sqlConn);
        //			sqlCmd.CommandType = CommandType.StoredProcedure;
        //
        //			sqlCmd.Parameters.Add("@iDocID",iDocID);
        //			sqlCmd.Parameters.Add("@iUserID",iUserID);
        //			sqlCmd.Parameters.Add("@iUserTypeID",iUserTypeID);
        //			sqlCmd.Parameters.Add("@strDocType",strDocType);
        //			
        //			sqlOutputParam = sqlCmd.Parameters.Add("@iIsOpened",SqlDbType.Int);
        //			sqlOutputParam.Direction = ParameterDirection.Output;
        //
        //			sqlCmd.ExecuteNonQuery();
        //			IsOpened = Convert.ToInt32(sqlOutputParam.Value);
        //
        //			sqlOutputParam = null;
        //			sqlCmd.Dispose();
        //			sqlConn.Close();
        //
        //			return IsOpened;
        //
        //		}
        #endregion
        // =========================================================================================================
        #region GetRejectionCodeID For NewLook

        public int GetRejectionCodeID_NL(int InvoiceID)
        {
            int iCode = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("stp_GetRejectionCodeID_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@Flag", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCode = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iCode);
        }
        #endregion
        // =========================================================================================================
        #region PermitToTakeAction
        public int PermitToTakeAction(int iDocID, int iUserID, int iUserTypeID, string strDocType)
        {
            int iIsPermit = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_CheckTimeLastActioned", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@iDocID", iDocID);
            sqlCmd.Parameters.Add("@iUserID", iUserID);
            sqlCmd.Parameters.Add("@iUserTypeID", iUserTypeID);
            sqlCmd.Parameters.Add("@strDocType", strDocType);

            sqlOutputParam = sqlCmd.Parameters.Add("@iIsPermit", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iIsPermit = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return iIsPermit;
        }
        #endregion

        // =========================================================================================================
        #region PermitToTakeActionGeneric
        public int PermitToTakeActionGeneric(int iDocID, int iUserID, string strDocType)
        {
            int iIsPermit = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_CheckTimeLastActionedGeneric", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@iDocID", iDocID);
            sqlCmd.Parameters.Add("@iUserID", iUserID);
            //	sqlCmd.Parameters.Add("@iUserTypeID",iUserTypeID);
            sqlCmd.Parameters.Add("@strDocType", strDocType);

            sqlOutputParam = sqlCmd.Parameters.Add("@iIsPermit", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iIsPermit = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return iIsPermit;
        }
        #endregion

        #region CheckActivityRequired
        public int CheckActivityRequired(int iDocID)
        {
            int iActivityRequired = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_CheckActivityRequired_NL", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@iDocID", iDocID);
            //			sqlCmd.Parameters.Add("@iUserID",iUserID);
            //			sqlCmd.Parameters.Add("@iUserTypeID",iUserTypeID);
            //			sqlCmd.Parameters.Add("@strDocType",strDocType);

            sqlOutputParam = sqlCmd.Parameters.Add("@iActivityRequired", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iActivityRequired = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return iActivityRequired;

        }
        #endregion

        // ==========================================================================================================
        // =========================================================================================================
        #region GetGlobalDropDowns(string  strFields , string strCriteria)
        public DataSet GetGlobalDropDowns(string strFields, string strTable, string strCriteria)
        {
            sqlConn = new SqlConnection(ConsString);
            try
            {
                sqlDA = new SqlDataAdapter("sp_GetGlobalDropDowns", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@Fields", strFields);
                sqlDA.SelectCommand.Parameters.Add("@Table", strTable);
                sqlDA.SelectCommand.Parameters.Add("@Criteria", strCriteria);
                ds = new DataSet();
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return (ds);
        }
        #endregion
        // ==========================================================================================================

        //========Sourayan27-11-2008Start=========//
        #region SaveInvoiceDatatoDataBase
        public int SaveInvoiceDatatoDataBase(int UserID, int InvoiceID, string strInvoiceNo, string strSupplierCompanyID, string strBuyerCompanyID,
            DateTime strInvoiceDate, DateTime strTaxPointDate, string strCurrency, string strSupplierAddress1,
            string strSupplierAddress2, string strSupplierAddress3, string strSupplierAddress4, string strSupplierAddress5,
            string strSupplierCountry, string strSupplierZIP, string strSellerVATNo, string strComments, string str22, decimal decNetTotal, decimal decVATAmt, decimal decTotalAmt, string strDeleteIDs)
        {
            //			SqlTransaction sqlTRANS=null;
            string strExceptionMessage = "";
            int iReturnValue = 0;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand("usp_EditInvoiceTestBuyer", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                //				sqlCmd.Transaction = sqlTRANS;
                // **************************************************************************************
                sqlCmd.Parameters.Add("@UserID", Convert.ToInt32(UserID));
                sqlCmd.Parameters.Add("@strInvoiceID", Convert.ToInt32(InvoiceID));
                sqlCmd.Parameters.Add("@strInvoiceNo", strInvoiceNo);
                sqlCmd.Parameters.Add("@strSupplierCompanyID", strSupplierCompanyID);
                sqlCmd.Parameters.Add("@strBuyerCompanyID", strBuyerCompanyID);
                sqlCmd.Parameters.Add("@strInvoiceDate", strInvoiceDate);
                //				if(strTaxPointDate!="")
                {
                    sqlCmd.Parameters.Add("@strTaxPointDate", strTaxPointDate);
                }
                //				else
                //				{	sqlCmd.Parameters.Add("@strTaxPointDate", DBNull.Value);	}
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
                //				sqlReturnParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                //				sqlReturnParam.Direction = ParameterDirection.Output;

                sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                sqlOutputParam.Direction = ParameterDirection.Output;

                sqlCmd.ExecuteNonQuery();
                //				iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);



            }
            catch (Exception ex)
            {
                strExceptionMessage = ex.Message.Trim();
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
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
        //========Sourayan27-11-2008End=========//

        //========Sourayan14-12-2008Start=========//
        #region GetSupplierAddress
        public DataSet GetSupplierAddress(int iCompanyID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("up_GetSupplierAddressFromBranch", sqlConn);
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

            return (ds);
        }
        #endregion
        //========Sourayan14-12-2008End=========//
        #region GetAuditTrail
        public DataTable GetAuditTrail(int iInvoiceID, string DocType)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();
            sqlDA = new SqlDataAdapter("sp_GetAuditTrail", sqlConn);

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);

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

        #region PermitToTakeActionDalkia
        public int PermitToTakeActionDalkia(int iDocID, int iUserID, string strDocType)
        {
            int iIsPermit = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("usp_CheckTimeLastActionedDalkia", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@iDocID", iDocID);
            sqlCmd.Parameters.Add("@iUserID", iUserID);
            //	sqlCmd.Parameters.Add("@iUserTypeID",iUserTypeID);
            sqlCmd.Parameters.Add("@strDocType", strDocType);

            sqlOutputParam = sqlCmd.Parameters.Add("@iIsPermit", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iIsPermit = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return iIsPermit;
        }
        #endregion

        #region GetAPCommentsGMG
        public DataTable GetAPCommentsGMG(int iInvoiceID, string iDocType)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();
            sqlDA = new SqlDataAdapter("sp_GetAPComments_GMG", sqlConn);

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", iDocType);

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

        #region GetAPCommLinkColor
        public int GetAPCommLinkColor(int iInvoiceID, string iDocType)
        {
            int iRetValue = 0;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();
            sqlDA = new SqlDataAdapter("sp_GetAPComments_GMG", sqlConn);

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", iDocType);

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

            if (ds.Tables[1].Rows.Count > 0)
                iRetValue = Convert.ToInt32(ds.Tables[1].Rows[0]["Hold"]);
            else
                iRetValue = 2;
            return (iRetValue);
        }
        #endregion

        #region SaveAPCommentsGMG
        public int SaveAPCommentsGMG(int InvoiceID, string strComments, string DocType, int Hold, int UserID)
        {
            //			SqlTransaction sqlTRANS=null;
            string strExceptionMessage = "";
            int iReturnValue = 0;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand("sp_SaveAPComments_GMG", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                //				sqlCmd.Transaction = sqlTRANS;
                // **************************************************************************************

                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(InvoiceID));
                sqlCmd.Parameters.Add("@Comments", strComments);
                sqlCmd.Parameters.Add("@DocType", DocType);
                sqlCmd.Parameters.Add("@Hold", Hold);
                sqlCmd.Parameters.Add("@UserID", UserID);

                sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                sqlOutputParam.Direction = ParameterDirection.Output;

                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex)
            {
                strExceptionMessage = ex.Message.Trim();

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

        #region GetLineInformation_NL_NEW
        public DataTable GetLineInformation_NL_NEW(int InvoiceID, string DocType)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //			sqlConn.Open();

            sqlDA = new SqlDataAdapter("GetLineInformation_NL_NEW", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);

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

        #region GetBusinessUnitName
        public string GetBusinessUnitName(int iInvoiceID, string DocType)
        {
            string strBusinessUnitName = "";
            SqlParameter sqlOutputParam = null;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlCmd = new SqlCommand("sp_GetBusinessUnitName", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlCmd.Parameters.Add("@DocType", DocType);
            sqlOutputParam = sqlCmd.Parameters.Add("@BusinessUnitName", SqlDbType.VarChar, 20);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                strBusinessUnitName = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
                sqlOutputParam = null;
            }
            return (strBusinessUnitName);
        }
        #endregion







    }
}
