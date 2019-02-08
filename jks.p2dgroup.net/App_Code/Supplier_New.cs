using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

/// <summary>
/// Summary description for Supplier_New
/// </summary>
/// 
namespace JKS
{
    public class Supplier_New
    {
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
        private string ConSt = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        #endregion

        public Supplier_New()
        {

        }

        //added by kuntal karar on 03.03.2015----------------------------
        public int SaveCompanyDetailsAndTradingRelations(int tradingid, int BuyerID, string SupplierName, string NetWorkID, string Address1,
                string Address2, string Address3, string PostCode, string iCounty, string iCountry, string Telephone, string Email, string VatNo,
                string VendorID, string VendorClass, string VendorGroup, string ApCompanyID, string Currency, int Status,
                int UserID, string NominalCode1, string NominalCode2, int PreApprove, int approvNeed, int EXPDepartmentID)// int EXPDepartmentID, Added by Mainak 2018-09-21
        {
            int iReturnValue = 0;
            sqlConn = new SqlConnection(ConSt);
            SqlParameter sqlOutputParam = null;
            sqlCmd = new SqlCommand("Sp_SaveCompanyDetailsAndTradingRelations_AkkeroETC_New_JKS", sqlConn);//Replaced Sp_SaveCompanyDetailsAndTradingRelations_New
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@TradingID", tradingid);
            sqlCmd.Parameters.Add("@BuyerID", BuyerID);
            sqlCmd.Parameters.Add("@SupplierName", SupplierName);
            sqlCmd.Parameters.Add("@NetWorkID", NetWorkID);
            sqlCmd.Parameters.Add("@Address1", Address1);
            sqlCmd.Parameters.Add("@Address2", Address2);
            sqlCmd.Parameters.Add("@Address3", Address3);
            sqlCmd.Parameters.Add("@PostCode", PostCode);
            if (iCounty == "")
                sqlCmd.Parameters.Add("@iCounty", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@iCounty", iCounty);

            if (iCountry == "")
                sqlCmd.Parameters.Add("@Country", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Country", iCountry);

            if (Telephone == "")
                sqlCmd.Parameters.Add("@Telephone", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Telephone", Telephone);

            if (Email == "")
                sqlCmd.Parameters.Add("@Email", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Email", Email);

            sqlCmd.Parameters.Add("@VatNo", VatNo);
            sqlCmd.Parameters.Add("@VendorID", VendorID);
            sqlCmd.Parameters.Add("@VendorClass", VendorClass);
            sqlCmd.Parameters.Add("@VendorGroup", VendorGroup);
            sqlCmd.Parameters.Add("@ApCompanyID", ApCompanyID);
            sqlCmd.Parameters.Add("@Currency", Currency);
            if (Status == 0)
                sqlCmd.Parameters.Add("@Status", "0");
            else
                sqlCmd.Parameters.Add("@Status", "1");

            sqlCmd.Parameters.Add("@UserID", UserID);
            sqlCmd.Parameters.Add("@NominalCode1", NominalCode1);
            sqlCmd.Parameters.Add("@NominalCode2", NominalCode2);
            sqlCmd.Parameters.Add("@PreApprove", PreApprove);
            //added by kuntal karar on 03.03.2015------------------------------
            sqlCmd.Parameters.Add("@ApprovalNeeded", approvNeed);
            //----------------------------------------------------------------
            //Added by Mainak 2018-09-21
            sqlCmd.Parameters.Add("@EXPDepartmentID", EXPDepartmentID);

            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex)
            {
                string ss = "Error:<br />";
                ss += "Message: " + ex.Message + "<br />";
                ss += "Data: " + ex.Data + "<br />";
                ss += "Source: " + ex.Source + "<br />";
                ss += "StackTrace: " + ex.StackTrace + "<br />";
                ss += "HelpLink: " + ex.HelpLink + "<br />";
                ss += "TargetSite: " + ex.TargetSite + "<br />";
                ss += "InnerException: " + ex.InnerException + "<br />";

                HttpContext.Current.Response.Write(ss);
            }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return iReturnValue;
        }
        //----------------------------------------------------------------


        //-------Method for Deleting Supplier record .Created By Subha Das on 12th Dec , 2014
        #region DeleteSupplierRecord
        public bool DeleteSupplierRecord(int iTradingRelationID, int iModUserID)
        {
            int iReturnValue = 0;
            bool bRetVal = true;
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlCmd = new SqlCommand("usp_DeleteSupplierRecord_New", sqlConn);//was usp_DeleteSupplierRecord
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@TradingRelationID", iTradingRelationID);
                sqlCmd.Parameters.Add("@ModUserID", iModUserID);
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
            }
            catch { bRetVal = false; }

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            return (bRetVal);
        }
        #endregion
        
        #region public DataSet GetSupplierStatusFromTradingRelation(int iCompanyID,int iSuppCompanyID,string StrVClass,string iVendorID,int Status)
        public DataSet GetSupplierStatusFromTradingRelation(int iCompanyID, int iSuppCompanyID, string StrVClass, string iVendorID, int Status)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                sqlDA = new SqlDataAdapter("sp_GetSupplierStatusFromTradingRelation_New", sqlConn);

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

                if (iVendorID == "")
                    sqlDA.SelectCommand.Parameters.Add("@VendorID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@VendorID", iVendorID);
                //========Uncommented By Rimi on 31stDec2015================
                if (Status == 0)
                    sqlDA.SelectCommand.Parameters.Add("@Status", "0");
                else
               //========Uncommented By Rimi on 31stDec2015 End==============
                sqlDA.SelectCommand.Parameters.Add("@Status", "1");


                ds = new DataSet();
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = "Error:<br />";
                ss += "Message: " + ex.Message + "<br />";
                ss += "Data: " + ex.Data + "<br />";
                ss += "Source: " + ex.Source + "<br />";
                ss += "StackTrace: " + ex.StackTrace + "<br />";
                ss += "HelpLink: " + ex.HelpLink + "<br />";
                ss += "TargetSite: " + ex.TargetSite + "<br />";
                ss += "InnerException: " + ex.InnerException + "<br />";

                HttpContext.Current.Response.Write(ss);
            }
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