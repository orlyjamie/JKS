using System;
using System.Web.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data ;
using CBSolutions.Architecture.Core ;

namespace JKS
{
	/// <summary>
	/// Summary description for Supplier.
	/// </summary>
	public class Supplier
	{
		#region default constractor
		public Supplier()
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
		private string ConSt = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
		#endregion						
		#region GetCompanyDropDownGeneric(int CompanyID)
		public DataSet GetCompanyDropDownGeneric(int CompanyID)
		{					
			sqlConn = new SqlConnection(ConSt);				
			try
			{
				sqlDA = new SqlDataAdapter("GetCompanyDropDownGeneric", sqlConn);
				sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;					
				sqlDA.SelectCommand.Parameters.Add("@CompanyID",CompanyID);				
				ds = new DataSet();
				sqlConn.Open();
				sqlDA.Fill(ds);				
			}
			catch(Exception ex){string ss=ex.Message.ToString();}
			finally
			{
				sqlDA.Dispose();
				sqlConn.Close();
			}					
			return (ds);
		}
		#endregion
		#region GetGlobalDropDowns(string  strFields , string strCriteria)
		public DataSet GetGlobalDropDowns(string  strFields ,string strTable, string strCriteria)
		{					
			sqlConn = new SqlConnection(ConSt);				
			try
			{
				sqlDA = new SqlDataAdapter("sp_GetGlobalDropDowns", sqlConn);
				sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;					
				sqlDA.SelectCommand.Parameters.Add("@Fields",strFields);
				sqlDA.SelectCommand.Parameters.Add("@Table",strTable);
				sqlDA.SelectCommand.Parameters.Add("@Criteria",strCriteria);
				ds = new DataSet();
				sqlConn.Open();
				sqlDA.Fill(ds);				
			}
			catch(Exception ex){string ss=ex.Message.ToString();}
			finally
			{
				sqlDA.Dispose();
				sqlConn.Close();
			}					
			return (ds);
		}
		#endregion
		#region public DataSet GetSupplierStatusFromTradingRelation(int iCompanyID,int iSuppCompanyID,string StrVClass,string iVendorID,int Status)
		public DataSet GetSupplierStatusFromTradingRelation(int iCompanyID,int iSuppCompanyID,string StrVClass,string iVendorID,int Status)
		{					
			sqlConn = new SqlConnection(ConSt);					
			try
			{
                sqlDA = new SqlDataAdapter("sp_GetSupplierStatusFromTradingRelation_New", sqlConn);               
                
				sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;	
				if(iCompanyID==0)
					sqlDA.SelectCommand.Parameters.Add("@CompanyID",DBNull.Value);
				else
					sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
				
				if(iSuppCompanyID==0)
					sqlDA.SelectCommand.Parameters.Add("@SupplierID",DBNull.Value);
				else
					sqlDA.SelectCommand.Parameters.Add("@SupplierID", iSuppCompanyID);

				if(StrVClass=="")
					sqlDA.SelectCommand.Parameters.Add("@VendorClass",DBNull.Value);
				else
					sqlDA.SelectCommand.Parameters.Add("@VendorClass", StrVClass);

				if(iVendorID=="")
					sqlDA.SelectCommand.Parameters.Add("@VendorID",DBNull.Value);
				else
					sqlDA.SelectCommand.Parameters.Add("@VendorID", iVendorID);

				if(Status==0)
					sqlDA.SelectCommand.Parameters.Add("@Status","0");
				else
					sqlDA.SelectCommand.Parameters.Add("@Status", "1");

				
				ds = new DataSet();
				sqlConn.Open();
				sqlDA.Fill(ds);				
			}
			catch(Exception ex){string ss=ex.Message.ToString();}
			finally
			{
				sqlDA.Dispose();
				sqlConn.Close();
			}		
			return (ds);
		}
		#endregion

		#region GetCountryList
		public DataTable GetCountryList()
		{
			sqlConn=new SqlConnection(ConSt);
			
			sqlDA = new SqlDataAdapter("sp_GetCountryList", sqlConn);
			sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;		

			ds = new DataSet();
			sqlConn.Open();
			sqlDA.Fill(ds);	
		
			sqlDA.Dispose();
			sqlConn.Close();
			
			return (ds.Tables[0]);
		}
		#endregion
		#region GetCountyList
		public DataTable GetCountyList()
		{
			sqlConn=new SqlConnection(ConSt);
			sqlDA = new SqlDataAdapter("sp_GetCountyList", sqlConn);
			sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;		

			ds = new DataSet();
			sqlConn.Open();
			sqlDA.Fill(ds);
			
			sqlDA.Dispose();
			sqlConn.Close();
			
			return (ds.Tables[0]);
		}
		#endregion
		
		public int SaveCompanyDetailsAndTradingRelations(int tradingid,int BuyerID,string SupplierName,string NetWorkID,string Address1,
			string Address2,string Address3,string PostCode,string iCounty,string iCountry,string Telephone,string Email,string VatNo,
			string VendorID,string VendorClass,string VendorGroup,string ApCompanyID, string Currency ,int Status ,
			int UserID,string NominalCode1,string NominalCode2,int PreApprove )
		{
			int iReturnValue =0;
			SqlConnection conn = new SqlConnection(ConSt);
			SqlParameter sqlOutputParam = null;
			SqlCommand cmd = new SqlCommand("Sp_SaveCompanyDetailsAndTradingRelations_AkkeroETC",conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@TradingID",tradingid);
			cmd.Parameters.Add("@BuyerID",BuyerID);
			cmd.Parameters.Add("@SupplierName",SupplierName);
			cmd.Parameters.Add("@NetWorkID",NetWorkID);
			cmd.Parameters.Add("@Address1",Address1);
			cmd.Parameters.Add("@Address2",Address2);
			cmd.Parameters.Add("@Address3",Address3);
			cmd.Parameters.Add("@PostCode",PostCode);
			if(iCounty =="")
				cmd.Parameters.Add("@iCounty",DBNull.Value);
			else
				cmd.Parameters.Add("@iCounty",iCounty);

			if(iCountry =="")
				cmd.Parameters.Add("@Country",DBNull.Value);
			else
				cmd.Parameters.Add("@Country",iCountry);

			if(Telephone =="")
				cmd.Parameters.Add("@Telephone",DBNull.Value);
			else
				cmd.Parameters.Add("@Telephone",Telephone);

			if(Email =="")
				cmd.Parameters.Add("@Email",DBNull.Value);
			else
				cmd.Parameters.Add("@Email",Email);

			cmd.Parameters.Add("@VatNo",VatNo);
			cmd.Parameters.Add("@VendorID",VendorID);
			cmd.Parameters.Add("@VendorClass",VendorClass);
			cmd.Parameters.Add("@VendorGroup",VendorGroup);
			cmd.Parameters.Add("@ApCompanyID",ApCompanyID);
			cmd.Parameters.Add("@Currency",Currency);
			if(Status ==0)
				cmd.Parameters.Add("@Status","0");
			else
				cmd.Parameters.Add("@Status","1");

			cmd.Parameters.Add("@UserID",UserID);
			cmd.Parameters.Add("@NominalCode1",NominalCode1);
			cmd.Parameters.Add("@NominalCode2",NominalCode2);
			cmd.Parameters.Add("@PreApprove",PreApprove);
					
			sqlOutputParam = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
			sqlOutputParam.Direction = ParameterDirection.Output;

			try
			{
				conn.Open();
				cmd.ExecuteNonQuery();
				iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
			}
			catch(Exception ex)
			{
				string ss = ex.Message.ToString();
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
			}
			
			return iReturnValue;		
		}

		#region GETNominalCode(int CompanyID)
		public DataSet GETNominalCode(int CompanyID)
		{					
			sqlConn = new SqlConnection(ConSt);				
			try
			{
				sqlDA = new SqlDataAdapter("Sp_GetNominalCode", sqlConn);
				sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;					
				sqlDA.SelectCommand.Parameters.Add("@buyerCompanyID",CompanyID);				
				ds = new DataSet();
				sqlConn.Open();
				sqlDA.Fill(ds);				
			}
			catch(Exception ex){string ss=ex.Message.ToString();}
			finally
			{
				sqlDA.Dispose();
				sqlConn.Close();
			}					 
			return (ds);
		}
		#endregion

		#region DataSet GetAllrecordsFromFromCompanyAndTrading(int  tradingid)
		public DataSet GetAllrecordsFromFromCompanyAndTrading(int  tradingid)
		{					
			sqlConn = new SqlConnection(ConSt);				
			try
			{
                sqlDA = new SqlDataAdapter("sp_GetAllrecordsFromFromCompanyAndTrading_New", sqlConn);
				sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;					
				sqlDA.SelectCommand.Parameters.Add("@TradingID",tradingid);
				
				ds = new DataSet();
				sqlConn.Open();
				sqlDA.Fill(ds);				
			}
			catch(Exception ex){string ss=ex.Message.ToString();}
			finally
			{
				sqlDA.Dispose();
				sqlConn.Close();
			}					
			return (ds);
		}
		#endregion

        //Added by Mainak 2018-09-21
        #region LoadDepartment()
        public DataSet LoadDepartment(int CompanyID, int userID, int UserTypeID)
        {
            sqlConn = new SqlConnection(ConSt);
            try
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentList_AkkeronETC", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", CompanyID);
                sqlDA.SelectCommand.Parameters.Add("@UserID", userID);
                sqlDA.SelectCommand.Parameters.Add("@UserTypeID", UserTypeID);
                ds = new DataSet();
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                //sqlDA.Dispose();
                sqlConn.Close();
            }
            return (ds);
        }
        #endregion
        
    }
}
