using System;
using System.Web.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

/// <summary>
/// Summary description for ManagePageContent
/// </summary>
/// 
namespace JKS
{


    public class ManagePageContent
    {
        #region: Global Variables
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());

        protected SqlDataAdapter sqlDA = null;
        protected SqlCommand SqlCmd = null;
        protected SqlParameter sqlReturnParam = null;
        #endregion

        #region: Private Veriable



        /// <summary>
        /// private veriable PageID
        /// </summary>
        private int _PageID;

        /// <summary>
        /// private veriable PageID
        /// </summary>
        private int _PageContentID;


        /// <summary>
        /// private veriable PageTitle
        /// </summary>
        private string _PageTitle;


        /// <summary>
        /// private veriable NavigateUrl
        /// </summary>
        private string _NavigateUrl;

        /// <summary>
        /// private veriable Contents
        /// </summary>
        private string _Contents;

        /// <summary>
        /// private veriable MetaKey
        /// </summary>
        private string _MetaKey;

        /// <summary>
        /// private veriable MetaTitle
        /// </summary>
        private string _MetaTitle;

        /// <summary>
        /// private veriable MetaTitle
        /// </summary>
        private string _MetaDesc;


        /// <summary>
        /// private veriable IsParentPage
        /// </summary>
        private int _IsParentPage;


        /// <summary>
        /// private veriable ParentPageID
        /// </summary>
        private int _ParentPageID;


        /// <summary>
        /// private veriable PageOrder
        /// </summary>
        private int _PageOrder;


        /// <summary>
        /// private veriable ShowOnFooter
        /// </summary>
        private int _ShowOnFooter;


        /// <summary>
        /// private veriable ShowOnHeader
        /// </summary>
        private int _ShowOnHeader;


        /// <summary>
        /// private veriable IsActive
        /// </summary>
        private int _IsActive;


        /// <summary>
        /// private veriable IsDeleted
        /// </summary>
        private int _IsDeleted;


        /// <summary>
        /// private veriable AddedBy
        /// </summary>
        private int _AddedBy;

        private string _ExceptionMessage;


        #endregion

        #region: Public  Properties



        /// <summary>
        /// Get/Set Values for PageID
        /// </summary>
        public int PageID
        {
            get
            {
                return this._PageID;
            }
            set
            {
                this._PageID = value;
            }
        }

        /// <summary>
        /// Get/Set Values for PageContentID
        /// </summary>
        public int PageContentID
        {
            get
            {
                return this._PageContentID;
            }
            set
            {
                this._PageContentID = value;
            }
        }


        /// <summary>
        /// Get/Set Values for PageTitle
        /// </summary>
        public string PageTitle
        {
            get
            {
                return this._PageTitle;
            }
            set
            {
                this._PageTitle = value;
            }
        }

        /// <summary>
        /// Get/Set Values for Contents
        /// </summary>
        public string Contents
        {
            get
            {
                return this._Contents;
            }
            set
            {
                this._Contents = value;
            }
        }


        /// <summary>
        /// Get/Set Values for MetaDesc
        /// </summary>
        public string MetaDesc
        {
            get
            {
                return this._MetaDesc;
            }
            set
            {
                this._MetaDesc = value;
            }
        }

        /// <summary>
        /// Get/Set Values for Contents
        /// </summary>
        public string MetaTitle
        {
            get
            {
                return this._MetaTitle;
            }
            set
            {
                this._MetaTitle = value;
            }
        }

        /// <summary>
        /// Get/Set Values for NavigateUrl
        /// </summary>
        public string NavigateUrl
        {
            get
            {
                return this._NavigateUrl;
            }
            set
            {
                this._NavigateUrl = value;
            }
        }

        /// <summary>
        /// Get/Set Values for MetaKey
        /// </summary>
        public string MetaKey
        {
            get
            {
                return this._MetaKey;
            }
            set
            {
                this._MetaKey = value;
            }
        }


        /// <summary>
        /// Get/Set Values for IsParentPage
        /// </summary>
        public int IsParentPage
        {
            get
            {
                return this._IsParentPage;
            }
            set
            {
                this._IsParentPage = value;
            }
        }


        /// <summary>
        /// Get/Set Values for ParentPageID
        /// </summary>
        public int ParentPageID
        {
            get
            {
                return this._ParentPageID;
            }
            set
            {
                this._ParentPageID = value;
            }
        }


        /// <summary>
        /// Get/Set Values for PageOrder
        /// </summary>
        public int PageOrder
        {
            get
            {
                return this._PageOrder;
            }
            set
            {
                this._PageOrder = value;
            }
        }


        /// <summary>
        /// Get/Set Values for ShowOnFooter
        /// </summary>
        public int ShowOnFooter
        {
            get
            {
                return this._ShowOnFooter;
            }
            set
            {
                this._ShowOnFooter = value;
            }
        }


        /// <summary>
        /// Get/Set Values for ShowOnHeader
        /// </summary>
        public int ShowOnHeader
        {
            get
            {
                return this._ShowOnHeader;
            }
            set
            {
                this._ShowOnHeader = value;
            }
        }


        /// <summary>
        /// Get/Set Values for IsActive
        /// </summary>
        public int IsActive
        {
            get
            {
                return this._IsActive;
            }
            set
            {
                this._IsActive = value;
            }
        }


        /// <summary>
        /// Get/Set Values for IsDeleted
        /// </summary>
        public int IsDeleted
        {
            get
            {
                return this._IsDeleted;
            }
            set
            {
                this._IsDeleted = value;
            }
        }


        /// <summary>
        /// Get/Set Values for AddedBy
        /// </summary>
        public int AddedBy
        {
            get
            {
                return this._AddedBy;
            }
            set
            {
                this._AddedBy = value;
            }
        }

        /// <summary>
        /// Get/Set Values for ExceptionMessage
        /// </summary>
        public string ExceptionMessage
        {
            get
            {
                return this._ExceptionMessage;
            }
            set
            {
                this._ExceptionMessage = value;
            }
        }


        #endregion


        public ManagePageContent()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region:Save

        public bool SaveCMSPageContent()
        {
            int iReturnValue = 0;
            bool bRetVal = true;
            try
            {
                int Mode = (PageContentID == 0) ? 1 : 2;
                SqlCon.Open();
                SqlCmd = new SqlCommand("P2D_SP_InsertUpdateManageCMSPageContent", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add("@Mode", Mode);
                SqlCmd.Parameters.Add("@PageID", PageID);
                SqlCmd.Parameters.Add("@PageContentID", PageContentID);
                SqlCmd.Parameters.Add("@PageOrder", PageOrder);
                SqlCmd.Parameters.Add("@Contents", Contents);
                SqlCmd.Parameters.Add("@MetaKey", MetaKey);
                SqlCmd.Parameters.Add("@MetaTitle", MetaTitle);
                SqlCmd.Parameters.Add("@MetaDesc", MetaDesc);
                SqlCmd.Parameters.Add("@IsActive", IsActive);
                SqlCmd.Parameters.Add("@IsDeleted", IsDeleted);
                SqlCmd.Parameters.Add("@User", AddedBy);

                sqlReturnParam = SqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                SqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.ToString();
                throw (ex);

            }
            finally
            {
                sqlReturnParam = null;
                SqlCmd.Dispose();
                SqlCon.Close();
            }

            return (bRetVal);
        }

        #endregion

        #region: Methods

        /// <summary>
        ///This Method is use to get all the data from Database
        /// </summary>
        /// <returns>Return DataTable of all Record</returns>
        public DataTable GetAll()
        {
            return GetAll(String.Empty);
        }
        /// <summary>
        /// This Method is use to get all the data from Database against given condition
        /// </summary>
        /// <param name="condition">condition to fetch record , no where clause, just condition</param>
        /// <returns>Return DataTable of all Record</returns>
        public DataTable GetAll(string condition)
        {
            return GetAll("*", condition, "P2D_ManageCMSPageContentView");
        }
        /// <summary>
        /// Get Specific collums against given condiotiion from given table
        /// </summary>
        /// <param name="fields">Fields will be fetch</param>
        /// <param name="condition">Condition of the query</param>
        /// <param name="tableName">Name of the DB Table</param>
        /// <returns>DataTable as Result</returns>
        public DataTable GetAll(string fields, string condition, string tableName)
        {
            return GetAll(fields, condition, tableName, String.Empty);
        }
        /// <summary>
        /// Get Specific Collumns against given condition and given order from given table
        /// </summary>
        /// <param name="fields">Fields will be fetch</param>
        /// <param name="condition">Condition of the query</param>
        /// <param name="tableName">Name of the DB Table</param>
        /// <param name="orderBy">Order by Column Name</param>
        /// <returns>DataTable as Result</returns>
        public DataTable GetAll(string fields, string condition, string tableName, string orderBy)
        {
            return GetAll(fields, condition, tableName, orderBy, "ASC");
        }
        /// <summary>
        /// Get Specific Collumns against given condition and given order from given table
        /// </summary>
        /// <param name="fields">Fields will be fetch</param>
        /// <param name="condition">Condition of the query</param>
        /// <param name="tableName">Name of the DB Table</param>
        /// <param name="orderBy">Order by Column Name</param>
        /// <param name="orderDirection">Order Direction  ASC or DESC</param>
        /// <returns>DataTable as Result</returns>
        public DataTable GetAll(string fields, string condition, string tableName, string orderBy, string orderDirection)
        {
            if (orderDirection.Trim().Length <= 0)
                orderDirection = "ASC";
            string sql = String.Empty;
            if (condition.Trim().Length > 0)
            {
                if (orderBy.Trim().Length > 0)
                {
                    sql = "select " + fields + " from " + tableName + " where " + condition + " order by " + orderBy + " " + orderDirection;
                }
                else
                {
                    sql = "select " + fields + " from " + tableName + " where " + condition;
                }
            }
            else
            {
                if (orderBy.Trim().Length > 0)
                {
                    sql = "select " + fields + " from " + tableName + " order by " + orderBy + " " + orderDirection;
                }
                else
                {
                    sql = "select " + fields + " from " + tableName;
                }
            }
            try
            {
                sqlDA = new SqlDataAdapter(sql, SqlCon);
                sqlDA.SelectCommand.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                SqlCon.Open();
                sqlDA.Fill(dt);
                return dt;
            }
            catch (Exception exception)
            {
                string ss = exception.Message.ToString();
                return new DataTable();
            }
            finally
            {
                sqlDA.Dispose();
                SqlCon.Close();
            }
        }


        #endregion

        #region: Delete

        public bool DeleteCMSPageContent(int iPageContentID, int ModifiedBy)
        {
            int iReturnValue = 0;
            bool bRetVal = true;

            //	sqlConn=new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlCon.Open();

            SqlCmd = new SqlCommand("sp_DeleteCMSPageContentData", SqlCon);
            SqlCmd.CommandType = CommandType.StoredProcedure;

            SqlCmd.Parameters.Add("@PageContentID", iPageContentID);
            SqlCmd.Parameters.Add("@ModifiedBy", ModifiedBy);

            sqlReturnParam = SqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;

            SqlCmd.ExecuteNonQuery();

            iReturnValue = Convert.ToInt32(sqlReturnParam.Value);

            if (iReturnValue == -101)
            {
                bRetVal = false;
            }

            sqlReturnParam = null;
            SqlCmd.Dispose();
            SqlCon.Close();

            return (bRetVal);
        }

        #endregion

    }
}
