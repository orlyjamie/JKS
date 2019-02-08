using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Text.RegularExpressions;

public partial class JKSForgotPasswordaspx : System.Web.UI.Page
{
    private int iCompanyID = 180918;//148053 for BBR changed to 180918 for JKS
    protected void Page_Load(object sender, EventArgs e)
    {
        // Put user code to initialize the page here
        if (!IsPostBack)
        {
            System.GC.Collect();
        }
        if (IsPostBack)
        {
            CBSAppUtils.PrimaryConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtUserName.Text.Trim().Length == 0 || txtEmail.Text.Trim().Length == 0)
        {
            this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Please enter All details .'); </script>");
        }
        else
        {
            CheckForgotPasswordSettings(txtUserName.Text.Trim().ToString(), txtEmail.Text.Trim().ToString());
        }
    }
    protected void CheckForgotPasswordSettings(string strUserName, string strEmail)
    {
        int UserID = 0;
        DataSet ds = new DataSet();
        SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
        // SqlDataAdapter sqlDA = new SqlDataAdapter("CheckForgotPasswordSettings_ETC", sqlConn);//Commeneted By Rimi on 8th August 2015
        SqlDataAdapter sqlDA = new SqlDataAdapter("CheckForgotPasswordSettings_ETC_sub", sqlConn);//Added By Rimi on 8th August 2015
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        sqlDA.SelectCommand.Parameters.Add("@UserName", strUserName);
        sqlDA.SelectCommand.Parameters.Add("@Email", strEmail);
        sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
        //blocked by kuntalkarar on 26thMay2016
        //sqlDA.SelectCommand.Parameters.Add("@networkid", "131cb96a-90");// Added By Rimi on 8th August 2015
        //Added by kuntalkarar on 26thMay2016
        sqlDA.SelectCommand.Parameters.Add("@networkid", "9ae44765-e9");//Changed 9ae44765-e9 of JKS from 0e8e82fa-14 of BBR

        try
        {
            sqlConn.Open();
            sqlDA.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                Page.RegisterStartupScript("Reg", "<script>PopulateMessage(1);</script>");
                return;
            }
            else if (ds.Tables[0].Rows.Count > 0)
            {

                //if (Convert.ToInt32(ds.Tables[0].Rows[0]["ResetQuestion"]) == 0 || Convert.ToInt32(ds.Tables[0].Rows[0]["ResetAnswer"]) == 0)//Commeneted By Rimi on 8th August 2015
                if (Convert.ToString(ds.Tables[0].Rows[0]["ResetQuestion"]) == "" || Convert.ToString(ds.Tables[0].Rows[0]["ResetAnswer"]) == "")//Added By Rimi on 8th August 2015
                {

                    Page.RegisterStartupScript("Reg", "<script>PopulateMessage(2);</script>");
                    return;
                }
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["IsAccountLocked"]) >= 3)
                {
                    Page.RegisterStartupScript("Reg", "<script>PopulateMessage(3);</script>");
                    return;
                }
                string Message = string.Empty;
                string strFetchedEmail = Convert.ToString(ds.Tables[0].Rows[0]["Email"]);
                if (strFetchedEmail.Trim().Length == 0)
                {
                    strFetchedEmail = "No Email held";
                }
                if (strFetchedEmail == "No Email held" || !IsValidEmail(Convert.ToString(ds.Tables[0].Rows[0]["Email"])))
                {
                    //  Message = "The email address held for you in the system is invalid (" + strFetchedEmail + "). Please contact the IS Helpdesk on 0330 606 1844 to correct it, and request a password reset.";
                    Message = "The email address held for you in the system is invalid (" + strFetchedEmail + "). Please contact your system administrator to correct it, and request a password reset.";
                    this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('" + Message + "'); </script>");
                    return;
                }
                UserID = Convert.ToInt32(ds.Tables[0].Rows[0]["UserID"]);
            }

        }
        catch (Exception ex)
        {
            string ss = ex.Message.ToString();

        }
        finally
        {
            if (sqlDA != null)
                sqlDA.Dispose();
            if (sqlConn != null)
                sqlConn.Close();
        }
        Server.Transfer("JKSSecurityInfo.aspx?UserID=" + UserID);
    }

    protected bool IsValidEmail(string strIn)
    {
        string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        return Regex.IsMatch(strIn, MatchEmailPattern);

    }
}