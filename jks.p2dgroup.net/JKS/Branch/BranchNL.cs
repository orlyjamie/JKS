using System;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;

namespace CBSolutions.ETH.Web.ETC
{
	/// <summary>
	/// Summary description for BranchNL.
	/// </summary>
	public class BranchNL
    {
		# region Variable Declaration
		private string errorMessage = null;
		#endregion
		
		public BranchNL()
		{			
		}
		
		#region SqlClient's objects
		protected SqlConnection sqlConn = null;	
		protected SqlDataAdapter sqlDA = null;
		private DataTable dtbl = new DataTable();
		protected DataSet ds = null;
		#endregion

		#region Property Declaration
		public string ErrorMessage
		{
			get
			{
				return errorMessage ;
			}
		}
		#endregion

		#region Enum
		public enum BranchAddressType
		{
			DeliveryAddress = 1,
			InvoiceAddress = 2
		}
		#endregion

		#region GetCountryList
		public static RecordSet GetCountryList()
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteQuery( "Country");
			return rs;
		}
		#endregion

		#region GetCountyList
		public static RecordSet GetCountyList()
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteQuery( "County");
			return rs;
		}
		#endregion

		#region GetBranchRelationListForBuyer
		public static RecordSet GetBranchRelationListForBuyer(int buyerCompanyID)
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteQuery( "vBranchRelationForBuyer", "BuyerCompanyID=" + buyerCompanyID );
			return rs;
		}
		#endregion

		#region GetBranchRelationAddList
		public static RecordSet GetBranchRelationAddList(int buyerID)
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteSP("up_GetBranchRelationAddList", buyerID) ;
			return rs;
		}
		#endregion

		#region GetBranchList
		/// <summary>
		/// Gets the list of Branch for the specified company
		/// </summary>
		/// <param name="companyID"></param>
		/// <returns></returns>
		public static RecordSet GetBranchList(int companyID)
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteQuery( "vw_BranchNL", "CompanyID = '" + System.Convert.ToString(companyID) + "'" );
			return rs;
		}
		#endregion

		#region GetBranchAddressList
		public static RecordSet GetBranchAddressList(int companyID)
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteSP("up_GetBranchAddress",companyID);
			return rs;
		}
		#endregion

		#region GetBranchAddressList
		public static RecordSet GetBranchAddressList(int companyID, BranchAddressType AddressType )
		{
			string branchAddressType;
			if (AddressType == BranchAddressType.DeliveryAddress )
				branchAddressType = "IsDeliveryLocation = 1" ;
			else
				branchAddressType = "IsInvoiceLocation = 1" ;

			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteSP("up_GetBranchAddress",companyID);
			rs.Filter = branchAddressType;			
			return rs;
		}
		#endregion

		#region GetBranchData
		/// <summary>
		/// gets the detail of a specified Branch
		/// </summary>
		/// <param name="branchID"></param>
		/// <returns></returns>
		public static RecordSet GetBranchData(int branchID)
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteQuery( "Branch", "BranchID = " + System.Convert.ToString(branchID ));
			return rs;
		}
		#endregion

		#region GetBranchAddressDetails
		public static RecordSet GetBranchAddressDetails(int branchID)
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteQuery( "vBranchAddressDetails", "BranchID = " + System.Convert.ToString(branchID ));
			return rs;
		}
		#endregion

		#region InsertBranchData
		/// <summary>
		/// Adds a single record to the Branch table in a transaction context
		/// </summary>
		/// <param name="rs"></param>
		/// <returns></returns>
		public int InsertBranchData(RecordSet rs)
		{
			errorMessage = "";
			DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString );
			int UserID = 0;
			da.BeginTransaction();
			if (!da.InsertRow( rs, ref UserID))
			{
				da.RollbackTransaction();
				errorMessage = da.ErrorMessage ;
			}
			else
				da.CommitTransaction() ;

			da.CloseConnection();
			da = null;

			return UserID ;
		}
		#endregion

		#region InsertBranchData
		/// <summary>
		/// Adds a single record to the Branch table
		/// </summary>
		/// <param name="rs"></param>
		/// <returns></returns>
		public int InsertBranchData(RecordSet rs, DataAccess da)
		{
			errorMessage = "";
			int UserID = 0;
			if (!da.InsertRow( rs, ref UserID))
				errorMessage = da.ErrorMessage ;

			return UserID ;
		}
		#endregion

		#region UpdateBranchData
		public Boolean UpdateBranchData(RecordSet rs)
		{
			errorMessage = "";
			DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString );
			da.BeginTransaction();
			if (!da.UpdateRow(rs))
			{
				da.RollbackTransaction();
				errorMessage = da.ErrorMessage ;
				return false;
			}
			da.CommitTransaction() ;

			da.CloseConnection();
			da = null;

			return true;
		}
		#endregion

		#region BranchRelation Related code
		public static RecordSet GetBranchRelationList( int branchID)
		{
			DataAccess da = new DataAccess( CBSAppUtils.PrimaryConnectionString );
			RecordSet rs = da.ExecuteQuery("BranchRelation", "BuyerBranchID = " + System.Convert.ToString( branchID)) ;
			return rs ; 
		}
		#endregion

		#region GetInvoiceDeliveryAddress
		public void GetInvoiceDeliveryAddress(string strRecipientHubID,
			out string 	strInvoiceAddress1, out string strInvoiceAddress2, out string strInvoiceAddress3, out string strInvoiceAddress4, 
			out string strInvoicePostcode, out string strInvoiceCountryCode,			
			out string strDeliveryAddress1, out string strDeliveryAddress2, out string strDeliveryAddress3, out string strDeliveryAddress4, 
			out string strDeliveryPostcode, out string strDeliveryCountryCode)
		{
			strInvoiceAddress1		= "";
			strInvoiceAddress2		= "";
			strInvoiceAddress3		= "";
			strInvoiceAddress4		= "";
			strInvoicePostcode		= "";
			strInvoiceCountryCode	= "";		
			strDeliveryAddress1		= "";
			strDeliveryAddress2		= "";
			strDeliveryAddress3		= "";
			strDeliveryAddress4		= "";
			strDeliveryPostcode		= "";
			strDeliveryCountryCode	= "";

			sqlConn=new SqlConnection(CBSAppUtils.PrimaryConnectionString);
			sqlConn.Open();
			sqlDA = new SqlDataAdapter("usp_GetInvoiceDeliveryAddress", sqlConn);
			sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
			sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyNetworkID", strRecipientHubID);
			ds = new DataSet();
			sqlDA.Fill(ds);
			sqlDA.Dispose();
			sqlConn.Close();
			dtbl = ds.Tables[0];
			for (int iRowCounter = 0; iRowCounter < dtbl.Rows.Count; iRowCounter ++)
			{
				if (Convert.ToInt32(dtbl.Rows[iRowCounter]["IsInvoiceLocation"]) == 1)
				{
					strDeliveryAddress1 = dtbl.Rows[iRowCounter]["Address1"].ToString().Trim();
					strDeliveryAddress2 = dtbl.Rows[iRowCounter]["Address2"].ToString().Trim();
					strDeliveryAddress3 = dtbl.Rows[iRowCounter]["Address3"].ToString().Trim();
					strDeliveryAddress4 = dtbl.Rows[iRowCounter]["Address4"].ToString().Trim(); 
					strDeliveryPostcode = dtbl.Rows[iRowCounter]["PostCode"].ToString().Trim();
					strDeliveryCountryCode = dtbl.Rows[iRowCounter]["Country"].ToString().Trim();
				}

				if (Convert.ToInt32(dtbl.Rows[iRowCounter]["IsDeliveryLocation"]) == 1)
				{
					strInvoiceAddress1 = dtbl.Rows[iRowCounter]["Address1"].ToString().Trim();
					strInvoiceAddress2 = dtbl.Rows[iRowCounter]["Address2"].ToString().Trim();
					strInvoiceAddress3 = dtbl.Rows[iRowCounter]["Address3"].ToString().Trim();
					strInvoiceAddress4 = dtbl.Rows[iRowCounter]["Address4"].ToString().Trim(); 
					strInvoicePostcode = dtbl.Rows[iRowCounter]["PostCode"].ToString().Trim();
					strInvoiceCountryCode = dtbl.Rows[iRowCounter]["Country"].ToString().Trim();
				}
			}
		
			dtbl.Dispose();
			ds.Dispose();			
		}
		#endregion
	}
}
