using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace JKS
{
    /// <summary>
    /// Summary description for DataAccessLayer
    /// </summary>
    public class DataAccessLayer
    {
        public DataAccessLayer()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        static String constr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        SqlConnection sqlConn = new SqlConnection(constr);
        public DataTable InsertUpdateDelete(DataTable Parameters, String SqlStatement)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            SqlTransaction myTrans = con.BeginTransaction();
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Transaction = myTrans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SqlStatement;

            for (int i = 0; i < Parameters.Rows.Count; i++)
            {

                cmd.Parameters.AddWithValue(Parameters.Rows[i]["ParameterName"].ToString(), Parameters.Rows[i]["ParameterValue"].ToString());


            }
            int commit = cmd.ExecuteNonQuery();
            myTrans.Commit();
            return Parameters;
        }
        public DataTable SelectWParameter(DataTable Parameters, String SqlStatement)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(SqlStatement, con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i < Parameters.Rows.Count; i++)
            {
                da.SelectCommand.Parameters.AddWithValue(Parameters.Rows[i]["ParameterName"].ToString(), Parameters.Rows[i]["ParameterValue"].ToString());

            }
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }
        public DataTable SelectWOParameter(String SqlStatement)
        {
            SqlConnection con = new SqlConnection(constr);

            SqlDataAdapter da = new SqlDataAdapter(SqlStatement, con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public DataSet returndataset(String SqlStatement)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql = SqlStatement;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                da.Dispose();
                return ds;
            }

        }


        //added by kd on 03/09/2018
        //added by kd on 02-08-2018 for INV
        public void INVVatUpdateBeforeExport(int invID)
        {

            try
            {
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand("Sp_ExportInvoice_JKS_VatUpdateBeforeExport", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@invID", SqlDbType.Int).Value = invID;
                sqlCmd.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

                //App_Code.EventLog.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 6);
            }
            finally
            {

            }


        }
        public DataTable ReturnInvoiceNo(int BuyerCompanyID)
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_JKS_ReturnInvoice", sqlConn);

            DataTable DT = new DataTable();
            try
            {
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", BuyerCompanyID);

                sqlDA.Fill(DT);
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

               // App_Code.EventLog.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 6);
            }
            finally
            {
                sqlDA.Dispose();
                DT.Dispose();
            }

            return DT;
        }
        // added by kd on 06-08-2018 for CRN
        public void CRNVatUpdateBeforeExport(int invID)
        {

            try
            {
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand("Sp_ExportInvoice_JKS_CRNvatUpdateBeforeExport", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@invID", SqlDbType.Int).Value = invID;
                sqlCmd.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

              //  App_Code.EventLog.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 6);
            }
            finally
            {

            }


        }
        public DataTable ReturnCRNNo(int BuyerCompanyID)
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_JKS_ReturnCRN", sqlConn);

            DataTable DT = new DataTable();
            try
            {
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", BuyerCompanyID);

                sqlDA.Fill(DT);
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

               // App_Code.EventLog.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 6);
            }
            finally
            {
                sqlDA.Dispose();
                DT.Dispose();
            }

            return DT;
        }
        public DataTable ReturnSageDataTable(int BuyerCompanyID)
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_Sage_JKS_New", sqlConn);

            DataTable DT = new DataTable();
            try
            {
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", BuyerCompanyID);
                sqlDA.SelectCommand.Parameters.AddWithValue("@UserID", 0);

                sqlDA.Fill(DT);
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

               // App_Code.EventLog.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 6);
            }
            finally
            {
                sqlDA.Dispose();
                DT.Dispose();
            }

            return DT;
        }
        public DataTable ReturnChildCompanies(int ParentCompanyID)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable DT = new DataTable();

            try
            {
                //string qry = "SELECT CompanyID, CompanyName FROM Company " +
                //            "WHERE (ParentCompanyID = @ParentCompanyID OR CompanyID = @ParentCompanyID) AND Active = 1 " +
                //            "ORDER BY CompanyName;";

                //string qry = "SELECT CompanyID, CompanyName FROM Company " +
                //            "WHERE (ParentCompanyID = @ParentCompanyID) AND Active = 1 " +
                //            "ORDER BY CompanyName;";

                string qry = "SELECT CompanyID, CompanyName FROM Company " +
                            "WHERE (ParentCompanyID = @ParentCompanyID AND CompanyID <> @ParentCompanyID) AND Active = 1 " +
                            "ORDER BY CompanyName;";

                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@ParentCompanyID", SqlDbType.Int).Value = ParentCompanyID;

                sqlDA = new SqlDataAdapter(sqlCmd);
                sqlDA.Fill(DT);
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

               // App_Code.EventLog.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 7);
            }
            finally
            {
                sqlDA.Dispose();
                DT.Dispose();
                sqlCmd.Dispose();
            }

            return DT;
        }
        public DataTable ReturnSageDataTableNONPO(int BuyerCompanyID)
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_Sage_JKS_New_NONPO", sqlConn);

            DataTable DT = new DataTable();
            try
            {
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", BuyerCompanyID);
                sqlDA.SelectCommand.Parameters.AddWithValue("@UserID", 0);

                sqlDA.Fill(DT);
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

                // App_Code.EventLog.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 6);
            }
            finally
            {
                sqlDA.Dispose();
                DT.Dispose();
            }

            return DT;
        }
        public void GetUpdateJKS_Matching()
        {
           
            sqlConn.Open();
            SqlCommand cmd = new SqlCommand("up_GetUpdateJKS_Matching", sqlConn);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

                //App_Code.EventLog.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 6);
            }
            finally
            {
            }

        }


    }
}