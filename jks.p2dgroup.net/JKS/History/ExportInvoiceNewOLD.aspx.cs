using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using CBSolutions.ETH.Web.UniversalMusic.downloadDB;
using CBSolutions.ETH.Web;
using System.Globalization;

public partial class ETC_History_ExportInvoiceNew : System.Web.UI.Page
{
    #region User Defined Variables
    protected SqlConnection sqlConn = null;
    protected SqlDataAdapter sqlDA = null;
    protected DataSet ds = null;
    protected DataTable objDataTable = null;
    downloadDB objdownloadDB = new downloadDB();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
            Response.Redirect("../../close_win.aspx");
        if (!IsPostBack)
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        DataTable dtbl = null;
        sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

        dtbl = objdownloadDB.GetBuyerCompanyListDropDown(Convert.ToInt32(Session["CompanyID"]));

        if (dtbl != null)
        {
            ddlBuyerCompany.DataSource = dtbl;
            ddlBuyerCompany.DataBind();

        }
        ddlBuyerCompany.Items.Insert(0, new ListItem("Select Company", "0"));

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        DateTime TimeNow = System.DateTime.Now;
        int Hours = TimeNow.Hour;
        int Minute = TimeNow.Minute;
        string CAppAqillaServiceHour = ConfigurationManager.AppSettings["CAppAqillaServiceTiming"].ToString();// ConfigurationManager.AppSettings["CAppAqillaServiceTiming"].ToString();
        string[] CAppAqillaServiceHours = CAppAqillaServiceHour.Split(',');
        for (int i = 0; i < CAppAqillaServiceHours.Length; i++)
        {
            if (Hours == Convert.ToInt32(CAppAqillaServiceHours[i].ToString()) && Minute < 45)
            {
                Page.RegisterStartupScript("reg", "<script> alert('The export has already been executed.');</script>");
                return;
            }
        }
        if (Application["ETCExecutionTime"] != null)
        {
            DateTime LastExecutionTime = (DateTime)Application["ETCExecutionTime"];
            DateTime CurrentTime = System.DateTime.Now;
            TimeSpan Span = CurrentTime - LastExecutionTime;
            double TotalMinutes = Span.TotalMinutes;
            if (TotalMinutes > 45)
            {
                Application["ETCExecutionTime"] = System.DateTime.Now;
                Page.RegisterStartupScript("reg", "<script>fnExportInvoice();</script>");
                //Response.Redirect("../History/ExportInvoice.aspx");
            }
            else
            {
                Page.RegisterStartupScript("reg", "<script> alert('The export has already been executed.');</script>");
            }
            //TimeDiff=Convert.ToInt32(Application["ExecutionTime"]);
        }
        else
        {
            Application["ETCExecutionTime"] = System.DateTime.Now;
            Page.RegisterStartupScript("reg", "<script>fnExportInvoice();</script>");
            //Response.Redirect("../History/ExportInvoice.aspx");
        }

    }

    protected void btnExportNew_Click(object sender, EventArgs e)
    {
        string fpath = ConfigurationManager.AppSettings["InvoiceExportPath_GRH"].Trim() + @"\" + ddlBuyerCompany.SelectedItem.ToString() + Convert.ToDateTime(DateTime.Now.ToLongDateString()).ToString("ddMMyyyy") + Convert.ToDateTime(DateTime.Now.ToLongTimeString()).ToString("HHmmss") + Session["UserID"].ToString() + ".csv";
        Stream fs = File.Create(fpath);

        string FileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fpath);
        fs.Close();
        Stream fs1 = File.Open(fpath, FileMode.Open, FileAccess.ReadWrite);
        string csvTxt = "";
        string ConString = ConfigurationManager.AppSettings["ConnectionString"].Trim();
        SqlConnection sqlConn = new SqlConnection(ConString);
        //SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_ETC", sqlConn);
        SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_GRH", sqlConn);
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(ddlBuyerCompany.SelectedValue));
        sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
        DataSet ds = new DataSet();
        try
        {
            sqlConn.Open();
            sqlDA.Fill(ds);
            csvTxt = "Type,Supplier a/c ref,Nominal a/c ref,Dept Code,Date,Reference,Details,Net Amount,Tax Code,Tax Amount\n";//,Exchange Rate,Extra Ref,User,Project Ref,Cost code Ref\n
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    csvTxt = csvTxt + ds.Tables[0].Rows[i]["Type"].ToString() + ",";
                    csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Supplier a/c ref"].ToString() + "\"" + ",";
                    csvTxt = csvTxt + ds.Tables[0].Rows[i]["Nominal a/c ref"].ToString() + ",";
                    csvTxt = csvTxt + ds.Tables[0].Rows[i]["Dept Code"].ToString() + ",";
                    csvTxt = csvTxt + ds.Tables[0].Rows[i]["Date"].ToString() + ",";
                    csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Reference"].ToString() + "\"" + ",";  // "\"" added so the file does not get corrupted if there is a comma in the text.
                    csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Details"].ToString() + "\"" + ",";
                    csvTxt = csvTxt + ds.Tables[0].Rows[i]["Net Amount"].ToString() + ",";
                    csvTxt = csvTxt + ds.Tables[0].Rows[i]["Tax Code"].ToString() + ",";
                    csvTxt = csvTxt + ds.Tables[0].Rows[i]["Tax Amount"].ToString() + ",";
                    //csvTxt = csvTxt + ds.Tables[0].Rows[i]["Exchange Rate"].ToString() + ",";
                    //csvTxt = csvTxt + ds.Tables[0].Rows[i]["Extra Ref"].ToString() + ",";
                    //csvTxt = csvTxt + "\"" + FileNameWithoutExtension + "-" + ds.Tables[0].Rows[i]["User"].ToString() + "\"" + ",";
                    //csvTxt = csvTxt + ds.Tables[0].Rows[i]["Project Ref"].ToString() + ",";
                    //csvTxt = csvTxt + ds.Tables[0].Rows[i]["Cost code Ref"].ToString() + "";
                    StreamWriter SW;
                    SW = new StreamWriter(fs1);
                    SW.WriteLine(csvTxt);
                    SW.Flush();
                    csvTxt = "";

                }
            }


        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            // SendEmail(err);

        }
        finally
        {
            sqlDA.Dispose();
            sqlConn.Close();
            fs1.Close();
        }
        Response.Redirect("../downloadDB/DownLoadFiles.aspx");

        // string sess = Session["UserID"].ToString();
        //Response.Redirect("DownLoadFiles.aspx");
        //Response.Redirect("grhSessiontest.aspx");
        // Page.RegisterStartupScript("reg", "<script>FnCompleted();</script>");
    }
}