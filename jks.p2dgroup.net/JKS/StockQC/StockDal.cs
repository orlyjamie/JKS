using System;
using System.Web.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace JKS
{
    /// <summary>
    /// Summary description for StockDal.
    /// </summary>
    public class StockDal
    {
        # region Variable Declaration
        //		private string errorMessage = null;
        private string strConn = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        #endregion

        #region  default constractor
        public StockDal()
        {
            //
            // TODO: Add constructor logic here
            //
        }
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

        private static decimal dInvoiced = 0;
        private static decimal dReceived = 0;
        private static decimal dVarience = 0;
        private static string Currency = "";

        public decimal PropInvoiced
        {
            get
            {
                return dInvoiced;
            }
            set
            {
                dInvoiced = value;
            }

        }

        public decimal PropReceived
        {
            get
            {
                return dReceived;
            }
            set
            {
                dReceived = value;
            }
        }

        public decimal PropVarience
        {
            get
            {
                return dVarience;
            }
            set
            {
                dVarience = value;
            }
        }

        public string CurrencyType
        {
            get
            {
                return Currency;
            }
            set
            {
                Currency = value;
            }
        }

        #region  public DataSet GetStockDocumentDetails(int InvoiceID,string Type)
        public DataSet GetStockDocumentDetails(int InvoiceID, string Type)
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            sqlDA = new SqlDataAdapter("GetStockDocumentDetailsETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@Type", Type);
            try
            {
                sqlConn.Open();
                ds = new DataSet();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return ds;
        }
        #endregion

        #region GetGoodsRecdDetailByGoodsRecdID
        public DataSet GetGoodsRecdDetailByGoodsRecdID(int iGoodsRecd, string IPONO)
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            //sqlDA = new SqlDataAdapter("GetGoodsRecdDetailByGoodsRecdID",sqlConn);
            sqlDA = new SqlDataAdapter("GetGoodsRecdDetailByGoodsRecdIDETC_New", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@GoodsRecdID", iGoodsRecd);
            sqlDA.SelectCommand.Parameters.Add("@POID", IPONO);
            try
            {
                sqlConn.Open();
                ds = new DataSet();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return ds;
        }
        #endregion

        #region GetStockDocumentDetails1
        public DataSet GetStockDocumentDetails1(int POID, string Type)
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            sqlDA = new SqlDataAdapter("GetStockDocumentDetails", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", POID);
            sqlDA.SelectCommand.Parameters.Add("@Type", Type);
            try
            {
                sqlConn.Open();
                ds = new DataSet();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return ds;
        }
        #endregion

    }
}
