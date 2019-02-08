using System;
using System.Web.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

namespace CBSolutions.ETH.Web.DebitNote
{
    /// <summary>
    /// Summary description for DebitNote.
    /// </summary>
    public class DebitNote
    {
        public DebitNote()
        {
            //
            // TODO: Add constructor logic here
            //
        }
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

        #region GetSalesInvoiceLogForSupplier
        public DataTable GetSalesInvoiceLogForSupplier(int iSupplierCompanyID, int iBuyerCompanyID,
             string strInvoiceNo, int iOption, int iInvoiceCreatedBy, string strStatusId, int iParentSupp)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            try
            {
                sqlConn.Open();
                sqlDA = new SqlDataAdapter("usp_GetSalesDeitNoteLogForSupplier", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", iSupplierCompanyID);
                if (iBuyerCompanyID == 0)
                    sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", iBuyerCompanyID);

                if (strInvoiceNo == "")
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", strInvoiceNo);

                sqlDA.SelectCommand.Parameters.Add("@Option", iOption);
                sqlDA.SelectCommand.Parameters.Add("@InvoiceCreatedBy", iInvoiceCreatedBy);
                if (strStatusId == "")
                    sqlDA.SelectCommand.Parameters.Add("@dbStatusId", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@dbStatusId", strStatusId);

                if (iParentSupp == 0)
                    sqlDA.SelectCommand.Parameters.Add("@SuppParentID", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@SuppParentID", iParentSupp);

                ds = new DataSet();
                sqlDA.Fill(ds);
            }
            catch { }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            return (ds.Tables[0]);
        }
        #endregion

        #region GetDebitNoteHeadDetail
        public static RecordSet GetDebitNoteHeadDetail(int iDebitNoteID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("usp_GetInvoiceConfirmationNL_DN", iDebitNoteID);
            return rs;
        }
        #endregion

        #region GetStatusList
        public DataTable GetStatusList()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            try
            {
                sqlConn.Open();

                sqlDA = new SqlDataAdapter("sp_GetStatusList", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                ds = new DataSet();

                sqlDA.Fill(ds);
            }
            catch { }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return (ds.Tables[0]);
        }
        #endregion

        #region GetInvoiceHead
        public static RecordSet GetInvoiceHead(int invoiceID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("rpt_GetNewLookDebitNoteHeader", invoiceID);
            return rs;
        }
        #endregion
        #region GetInvoiceDetail
        public static RecordSet GetInvoiceDetail(int invoiceID)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("rpt_GetNewLookDebitNoteDetail", invoiceID);
            return rs;
        }
        #endregion



    }
}
