using System;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using CBSolutions.Architecture.Core;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

[ScriptService]
public partial class ETC_AutoCompletePopup : System.Web.UI.Page
{
    // AutoComplete 
    public string listFilter = null;
    public int IsShowFlag = 1;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    // [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static List<DescriptionData> GetCombinedDescription(string Filter)
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    // public static string GetCombinedDescription(string Filter)
    public static string[] GetCombinedDescription(string Filter)
    {
        int strUserTypeID = 1;
        if (HttpContext.Current.Session["UserTypeID"] != null)
        {
            int.TryParse(HttpContext.Current.Session["UserTypeID"].ToString(), out strUserTypeID);
        }
        //75958
        int BuyerCompanyID = 0;
        if (HttpContext.Current.Session["DropDownCompanyID"] != null)
        {
            int.TryParse(HttpContext.Current.Session["DropDownCompanyID"].ToString(), out BuyerCompanyID);
        }
        SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
        SqlDataAdapter sqlDA = new SqlDataAdapter("Fetch_CodingDescriptionNominalCodeDepartment", sqlConn);
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", BuyerCompanyID);
        sqlDA.SelectCommand.Parameters.Add("@UserTypeID", strUserTypeID);
        sqlDA.SelectCommand.Parameters.Add("@Filter", Filter);
        DataSet ds = new DataSet();
        try
        {
            sqlConn.Open();
            sqlDA.Fill(ds);
        }
        catch (Exception ex)
        {
            string strExceptionMessage = ex.Message.Trim();

        }
        finally
        {
            if (sqlDA != null)
                sqlDA.Dispose();
            if (sqlConn != null)
                sqlConn.Close();
        }

        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    /*DataTable to List conversion*/

        //    string xx = JsonConvert.SerializeObject(ds.Tables[0]);
        //    // return JsonConvert.SerializeObject(ds.Tables[0]);
        //    List<DescriptionData> myList = new List<DescriptionData>();
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {

        //        myList.Add(new DescriptionData { CodingDescription = row["CodingDescription"].ToString(), CodingDescriptionID = Convert.ToInt32(row["CodingDescriptionID"].ToString()), APAdminOnly = Convert.ToInt32(row["APAdminOnly"].ToString()) });

        //    }
        //    return myList;
        //////if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //////{
        //////    StringBuilder output = new StringBuilder();
        //////    output.Append("[");
        //////    for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
        //////    {
        //////        output.Append("\"" + ds.Tables[0].Rows[i]["CodingDescription"].ToString() + "\"");

        //////        if (i != (ds.Tables[0].Rows.Count - 1))
        //////        {
        //////            output.Append(",");
        //////        }
        //////    }
        //////    output.Append("];");
        //////    return output.ToString();
        //////}

        //////else
        //////{
        //////    return "";
        //////}

        List<string> myList = new List<string>();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                myList.Add(string.Format("{0}#{1}", row["CodingDescription"].ToString(), Convert.ToInt32(row["CodingDescriptionID"].ToString())));
            }
            return myList.ToArray();
        }
        else
            return null;
    }

    public class DescriptionData
    {

        public string CodingDescription { get; set; }
        public int CodingDescriptionID { get; set; }
        public int APAdminOnly { get; set; }

    }

}