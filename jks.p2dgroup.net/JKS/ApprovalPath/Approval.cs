using System;
using System.Web.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

namespace CBSolutions.ETH.Web.ETC.ApprovalPath
{
    /// <summary>
    /// Summary description for Approval.
    /// </summary>
    public class Approval
    {
        #region default constractor
        public Approval()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion
        #region Mail variable declaration
        //		private MailFormat _mailFormat = MailFormat.Html;
        //		private MailPriority _mailPriority = MailPriority.High;
        #endregion
        #region SqlClient's objects
        protected SqlCommand objComm = null;
        protected SqlConnection objConn = null;

        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected SqlCommand sqlCmd = null;
        protected SqlDataReader sqlDR = null;
        protected SqlParameter sqlReturnParam = null;

        protected DataSet ds = null;
        #endregion
        #region Variable declaration
        //		private string errorMessage = null;
        private string ConSt = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        #endregion

        #region GetNominalCodeDropDown_NB
        public DataSet GetNominalCodeDropDown_NB(int iCompanyID)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlDA = new SqlDataAdapter("sp_GetNominalCodeDropDowns_NB", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
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
        #region GetRecordsFromApproval
        public DataSet GetRecordsFromApproval(int iApproverID)
        {
            sqlConn = new SqlConnection(ConSt);

            try
            {
                string qry = "select * from ApprovalPaths where ApprovalPathsTableID =" + iApproverID;
                sqlDA = new SqlDataAdapter(qry, sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.Text;
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

        #region GetSupplierDropDown
        public DataSet GetSupplierDropDown(int iCompanyID)
        {
            sqlConn = new SqlConnection(ConSt);

            try
            {
                sqlDA = new SqlDataAdapter("sp_GetSuppliersList", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iCompanyID);
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
        #region public DataSet GetDetailsFromApprovalPaths_NB(int iCompanyID,int iSuppCompanyID,string StrVClass,string strDept,string strNominal,string strNetFrom,string strNetTo)
        public DataSet GetDetailsFromApprovalPaths_NB(int iCompanyID, int iSuppCompanyID, string StrVClass, string strNetFrom, string strNetTo)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlDA = new SqlDataAdapter("GetDetailsFromApprovalPaths_NB_AkkeronETC", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (iCompanyID == 0)
                    sqlDA.SelectCommand.Parameters.Add("@CompanyID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);

                if (iSuppCompanyID == 0)
                    sqlDA.SelectCommand.Parameters.Add("@SupplierID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@SupplierID", iSuppCompanyID);

                if (StrVClass == "")
                    sqlDA.SelectCommand.Parameters.Add("@VendorClass", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@VendorClass", StrVClass);

                if (strNetFrom == "")
                    sqlDA.SelectCommand.Parameters.Add("@NetFrom", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@NetFrom", strNetFrom);

                if (strNetTo == "")
                    sqlDA.SelectCommand.Parameters.Add("@NetTo", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@NetTo", strNetTo);


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

        #region GetApproverDropDowns
        public DataSet GetApproverDropDownsByDepartment(int CompanyID, int DeptID)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlDA = new SqlDataAdapter("Sp_GetApproverListsANDGroups_AkkeronETC", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@DeptID", DeptID);
                sqlDA.SelectCommand.Parameters.Add("@CompID", CompanyID);
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
        #region GetApproverDropDowns
        public DataSet GetApproverDropDowns(int CompanyID, int iDeptID)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                string sqlQry = "SELECT GroupID,GroupName FROM GenericGroup WHERE CompanyID =" + CompanyID + " AND StatusID =1 AND Department =" + iDeptID;
                sqlDA = new SqlDataAdapter("sp_GetApproverListsANDGroups", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@DeptID", iDeptID);
                sqlDA.SelectCommand.Parameters.Add("@CompID", CompanyID);
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
        #region public int SaveApprovalPaths
        public int SaveApprovalPaths(int iApprovalPathsID, int iBuyerCompanyID, int iSupplierCompanyID, string StrVClass,
            decimal dNetFrom, decimal dNetTo, string iApprover1, string iApprover2, string iApprover3, string iApprover4,
            string iApprover5, string iApprover6, string iApprover7, string iApprover8, string iApprover9, int iDepartment, int iBusinessUnit)
        {
            int iReturnValue = 0;
            SqlParameter sqlOutputParam = null;
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlCmd = new SqlCommand("sp_SaveApprovalPaths_New", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@ApprovalPathsTableID", iApprovalPathsID);
                sqlCmd.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);
                sqlCmd.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
                sqlCmd.Parameters.Add("@New_VendorClass", StrVClass);
                sqlCmd.Parameters.Add("@NetFrom", dNetFrom);
                sqlCmd.Parameters.Add("@NetTo", dNetTo);
                if (iApprover1 != null)
                    sqlCmd.Parameters.Add("@Approver1", iApprover1);
                else
                    sqlCmd.Parameters.Add("@Approver1", DBNull.Value);

                if (iApprover2 != null)
                    sqlCmd.Parameters.Add("@Approver2", iApprover2);
                else
                    sqlCmd.Parameters.Add("@Approver2", DBNull.Value);

                if (iApprover3 != null)
                    sqlCmd.Parameters.Add("@Approver3", iApprover3);
                else
                    sqlCmd.Parameters.Add("@Approver3", DBNull.Value);

                if (iApprover4 != null)
                    sqlCmd.Parameters.Add("@Approver4", iApprover4);
                else
                    sqlCmd.Parameters.Add("@Approver4", DBNull.Value);

                if (iApprover5 != null)
                    sqlCmd.Parameters.Add("@Approver5", iApprover5);
                else
                    sqlCmd.Parameters.Add("@Approver5", DBNull.Value);

                if (iApprover6 != null)
                    sqlCmd.Parameters.Add("@Approver6", iApprover6);
                else
                    sqlCmd.Parameters.Add("@Approver6", DBNull.Value);

                if (iApprover7 != null)
                    sqlCmd.Parameters.Add("@Approver7", iApprover7);
                else
                    sqlCmd.Parameters.Add("@Approver7", DBNull.Value);

                sqlCmd.Parameters.Add("@Approver8", iApprover8);
                sqlCmd.Parameters.Add("@Approver9", iApprover9);
                sqlCmd.Parameters.Add("@DepartmentID", iDepartment);
                sqlCmd.Parameters.Add("@BusinessUnitID", iBusinessUnit);


                sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                sqlOutputParam.Direction = ParameterDirection.Output;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
        #region GetGlobalDropDowns(string  strFields , string strCriteria)
        public DataSet GetGlobalDropDowns(string strFields, string strTable, string strCriteria)
        {
            sqlConn = new SqlConnection(ConSt);
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
        #region GetCompanyDropDownGeneric(int CompanyID)
        public DataSet GetCompanyDropDownGeneric(int CompanyID)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlDA = new SqlDataAdapter("GetCompanyDropDownGeneric", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", CompanyID);
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
        #region DeleteItem
        public int DeleteItem(int iItemID)
        {
            int retVal = 0;
            sqlConn = new SqlConnection(ConSt);
            try
            {
                string qry = "update ApprovalPaths set IsDeleted =1 where ApprovalPathsTableID =" + iItemID;
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;

                sqlConn.Open();
                retVal = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (retVal);
        }
        #endregion
        #region GetNominalCodeDropDown_NB
        public DataSet GetNominalDropDownsAfterDepartmentSelect(int iCompanyID, int iDept)
        {
            string sQry = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in(SELECT NominalCodeID FROM CodingDescription WHERE DepartmentCodeID =" + iDept + " AND BuyerCompanyID=" + iCompanyID + ")";
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlDA = new SqlDataAdapter(sQry, sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.Text;
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
        #region DataExistsInTable
        public int DataExistsInTable(int CompanyID, string DeptID, string Nominal)
        {
            int RetVal = 0;
            sqlConn = new SqlConnection(ConSt);
            try
            {
                string sqlQry = "SELECT COUNT(*)AS CNT FROM CodingDescription WHERE BuyerCompanyID =" + CompanyID + " AND NominalCodeID = (SELECT TOP 1 NominalCodeID FROM NominalCode WHERE NominalCode ='" + Nominal + "'AND BuyerCompanyID=" + CompanyID + ") AND DepartmentCodeID = (SELECT TOP 1 DepartmentID FROM Department WHERE Department ='" + DeptID + "' AND BuyerCompanyID=" + CompanyID + ")";
                sqlDA = new SqlDataAdapter(sqlQry, sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.Text;
                sqlDA.SelectCommand.CommandType = CommandType.Text;
                ds = new DataSet();
                sqlConn.Open();
                sqlDA.Fill(ds);
                RetVal = Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return RetVal;
        }
        #endregion

        #region public int InsertDataIntoCodingDescription
        public int InsertDataIntoCodingDescription(int CompanyID, string Department, string NominalCode, string Project, string CodingDesc, string strAPAdminOnly)
        {
            int iReturnValue = 0;
            SqlParameter sqlOutputParam = null;
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlCmd = new SqlCommand("sp_InsertDataIntoCodingDescriptionCTBAkkeronETGMG", sqlConn);

                //sqlCmd = new SqlCommand("sp_InsertDataIntoCodingDescriptionGMG", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@Company", CompanyID);
                if (Department != null)
                    sqlCmd.Parameters.Add("@Department", Department);
                else
                    sqlCmd.Parameters.Add("@Department", DBNull.Value);

                if (NominalCode != null)
                    sqlCmd.Parameters.Add("@NominalCode", NominalCode);
                else
                    sqlCmd.Parameters.Add("@NominalCode", DBNull.Value);

                //				if(Project!=null)
                //					sqlCmd.Parameters.Add("@Project", Project);
                //				else
                //					sqlCmd.Parameters.Add("@Project",DBNull.Value);

                if (CodingDesc != null)
                    sqlCmd.Parameters.Add("@CodingDesc", CodingDesc);
                else
                    sqlCmd.Parameters.Add("@CodingDesc", DBNull.Value);

                if (strAPAdminOnly.ToUpper().IndexOf("SELECT") == -1)
                    sqlCmd.Parameters.Add("@APAdminOnly", strAPAdminOnly);
                else
                    sqlCmd.Parameters.Add("@APAdminOnly", DBNull.Value);

                sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                sqlOutputParam.Direction = ParameterDirection.Output;

                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion
        #region GetSupplierFromTradingRelation
        public DataSet GetSupplierFromTradingRelation(int iCompanyID)
        {
            sqlConn = new SqlConnection(ConSt);

            try
            {
                sqlDA = new SqlDataAdapter("sp_GetSuppliersListFromTradingRelation", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iCompanyID);
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

        #region Get2ndApproverDropDownAkkeronETC
        public DataSet Get2ndApproverDropDownAkkeronETC(int CompanyID, int iDeptID)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlDA = new SqlDataAdapter("Sp_GetApproverListsANDGroups_AkkeronETC", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@DeptID", iDeptID);
                sqlDA.SelectCommand.Parameters.Add("@CompID", CompanyID);
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
        #region Get2ndApproverDropDownAkkeronETC1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="iDeptID"></param>
        /// <returns></returns>
        public DataSet Get2ndApproverDropDownAkkeronETC1(int CompanyID, int iDeptID)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlDA = new SqlDataAdapter("Sp_GetApproverListsANDGroups2_AkkeronETC", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@DeptID", iDeptID);
                sqlDA.SelectCommand.Parameters.Add("@CompID", CompanyID);
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
        #region: Search Result (Created By Mrinal)
        public DataSet GetSearchDetailsApprovalPath(int iCompanyID, int iSuppCompanyID, string StrVClass, int DepartmentID, int BusinessUnitID)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                //sqlDA = new SqlDataAdapter("GetDetailsFromApprovalPaths_NB_AkkeronETC", sqlConn);
                sqlDA = new SqlDataAdapter("GetSearchDetailsApprovalPath", sqlConn);

                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (iCompanyID == 0)
                    sqlDA.SelectCommand.Parameters.Add("@CompanyID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);

                if (iSuppCompanyID == 0)
                    sqlDA.SelectCommand.Parameters.Add("@SupplierID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@SupplierID", iSuppCompanyID);

                if (StrVClass == "")
                    sqlDA.SelectCommand.Parameters.Add("@VendorClass", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@VendorClass", StrVClass);


                if (DepartmentID == 0)
                    sqlDA.SelectCommand.Parameters.Add("@DepartmentID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@DepartmentID", DepartmentID);

                if (BusinessUnitID == 0)
                    sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", BusinessUnitID);


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

    }
}
