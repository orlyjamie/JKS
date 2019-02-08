using System;
using System.Web.Mail;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.ETH.Web.UniversalMusic.downloadDB;

namespace JKS
{
    /// <summary>
    /// Summary description for downloadDB.
    /// </summary>
    public class downloadDB : System.Web.UI.Page
    {
        #region SqlClient's objects
        protected SqlCommand objComm = null;
        protected SqlConnection objConn = null;
        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected SqlCommand sqlCmd = null;
        protected SqlParameter sqlReturnParam = null;
        protected SqlParameter sqlOutputParam = null;
        protected DataSet ds = null;
        #endregion

        #region Default Constructor
        public downloadDB()
        {

        }
        #endregion

        #region GetBuyerCompanyListDropDown
        public DataTable GetBuyerCompanyListDropDown(int iCompanyID)
        {
            try
            {
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlDA = new SqlDataAdapter("sp_GetBuyerCompanyForDownloadDB", sqlConn);
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
            catch { return (null); }
        }
        #endregion

        #region ExecuteSqlServerPackage
        public string ExecuteSqlServerPackage(int iBuyerCompanyID, string strFromDate, string strToDate, string strType, int iCurrentOnly)
        {
            #region Variable Declaration
            string strServerName = "";
            string strUserName = "";
            string strPassword = "";
            string strDTSPackageName = "";
            string strMessage = "";
            JKS.DTSManager oDTSManager = null;
            oDTSManager = new DTSManager();
            #endregion

            strServerName = ConfigurationManager.AppSettings["SNM"].Trim();
            strUserName = ConfigurationManager.AppSettings["SUN"].Trim();
            strPassword = ConfigurationManager.AppSettings["SPWD"].Trim();
            if (strType == "INVOICE")
            {
                if (iCurrentOnly == 1)
                    strDTSPackageName = ConfigurationManager.AppSettings["DTS_Package_InvoiceD_Current"].Trim();
                else
                    strDTSPackageName = ConfigurationManager.AppSettings["DTS_Package_InvoiceD"].Trim();
            }
            else if (strType == "CREDIT")
            {
                if (iCurrentOnly == 1)
                    strDTSPackageName = ConfigurationManager.AppSettings["DTS_Package_CreditD_Current"].Trim();
                else
                    strDTSPackageName = ConfigurationManager.AppSettings["DTS_Package_CreditD"].Trim();
            }
            else
                strDTSPackageName = ConfigurationManager.AppSettings["DTS_Package_DebitD"].Trim();

            oDTSManager.ServerName = strServerName;
            oDTSManager.ServerUsername = strUserName;
            oDTSManager.ServerPassword = strPassword;
            oDTSManager.PackageName = strDTSPackageName;
            strMessage = oDTSManager.ExecuteSqlServerPackage(iBuyerCompanyID, strFromDate, strToDate, strType);
            oDTSManager = null;
            return (strMessage);
        }
        #endregion
    }
}