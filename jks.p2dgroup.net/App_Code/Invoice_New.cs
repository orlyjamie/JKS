using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Collections.Generic;

/// <summary>
/// Summary description for Invoice_New
/// </summary>
namespace JKS
{
    public partial class Invoice_New
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

        public Invoice_New()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region GetSuppliersList
        public DataTable GetSuppliersListForSearch(int iBuyerCompanyID, int UserID, int UserTypeID, string BuyerCompanyString)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("sp_GetSuppliersList_Akkeron", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);
            sqlDA.SelectCommand.Parameters.Add("@UserID", UserID);
            sqlDA.SelectCommand.Parameters.Add("@USerTypeID", UserTypeID);
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyString", BuyerCompanyString);
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

            if (ds != null && ds.Tables.Count > 0)
            {
                return (ds.Tables[0]);
            }
            else
            {
                return new DataTable();
            }
        }
        #endregion

        #region GetStatusListNL_Current
        public DataTable GetStatusListNL_Current()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter("Sp_GetStatusList_AkkeronETC_New", sqlConn);
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

    }
}