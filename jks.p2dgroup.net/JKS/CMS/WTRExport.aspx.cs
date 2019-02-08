using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace JKS
{
    /// <summary>
    /// Rinku Santra
    /// Summary description for WTRExport.
    /// </summary>
    public class WTRExport : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Label lblHeader;
        protected System.Web.UI.WebControls.DropDownList ddlCompany;
        protected System.Web.UI.WebControls.DropDownList ddlWeekStartDate;
        protected System.Web.UI.WebControls.Button btnSubmit;

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Page.RegisterStartupScript("reg", "<script>alert('Sorry,your Session is expired');</script>");
                Response.Redirect("../../etcdefault.aspx");
            }
            if (!IsPostBack)
            {
                ViewState["DepartmentName"] = "";
                btnSubmit.Attributes.Add("onclick", "return fn_CMSValidation();");
                LoadCompany();
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
            this.ddlWeekStartDate.SelectedIndexChanged += new System.EventHandler(this.ddlWeekStartDate_SelectedIndexChanged);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            string flname = "WTR" + ddlCompany.SelectedItem.ToString().Replace(" ", "") + Convert.ToDateTime(DateTime.Now.ToLongDateString()).ToString("ddMMyyyy") + Convert.ToDateTime(DateTime.Now.ToLongTimeString()).ToString("HHmmss") + ".csv";
            string fpath = ConfigurationManager.AppSettings["WTRExportPath_ETC"].Trim() + flname;
            Stream fs = File.Create(fpath);
            fs.Close();
            Stream fs1 = File.Open(fpath, FileMode.Open, FileAccess.ReadWrite);
            string csvTxt = "";
            string csvTxtHead = "";
            string csvTxtBody = "";
            string csvTxt1 = "," + ddlWeekStartDate.SelectedValue.ToString() + ",,Sales Postings,,,,";
            string csvTxt2 = ",,,,,,,";
            string ConString = ConfigurationManager.AppSettings["ConnectionString"].Trim();
            SqlConnection sqlConn = new SqlConnection(ConString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Ep_WTRExport", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            sqlDA.SelectCommand.Parameters.Add("@StartWeekDate", Convert.ToString(ddlWeekStartDate.SelectedValue));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                csvTxtHead = "Header,,,,,,,Details,,,,, \n";
                csvTxtHead += "Reference,Document Date,Originator Reference,Customer Code,Currency,Currency Rate,,Description,Quantity,Unit Price,Vat Rate,Account Code,Cost Centre  \n";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field1name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field1name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field1"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field1Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field1name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field1name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field1"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field1Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field2name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field2name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field2"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field2Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field2name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field2name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field2"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field2Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field3name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field3name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field3"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field3Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field3name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field3name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field3"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field3Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field4name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field4name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field4"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field4Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field4name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field4name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field4"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field4Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field5name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field5name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field5"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field5Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field5name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field5name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field5"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field5Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field6name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field6name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field6"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field6Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field6name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field6name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field6"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field6Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field7name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field7name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field7"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field7Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field7name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field7name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field7"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field7Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                csvTxtBody = csvTxt1 + "Sales VAT,1," + ds.Tables[0].Rows[i]["vat"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["SalesVATNominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                csvTxtBody += csvTxt2 + "Sales VAT,1," + ds.Tables[0].Rows[i]["vat"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["SalesVATNominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field8name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field8name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field8"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field8Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field8name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field8name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field8"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field8Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field9name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field9name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field9"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field9Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field9name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field9name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field9"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field9Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field10name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field10name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field10"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field10Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field10name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field10name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field10"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field10Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field11name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field11name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field11"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field11Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field11name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field11name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field11"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field11Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field12name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field12name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field12"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field12Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["field12name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field12name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field12"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field12Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["Field13name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field13name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field13"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field13Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["Field13name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field13name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field13"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field13Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["Field14name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field14name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field14"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field14Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["Field14name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field14name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field14"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field14Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["Field15name"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["Field15name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field15"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field15Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["Field15name"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field15name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field15"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field15Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }


                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash1"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash1"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet1"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash1Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash1"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash1"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet1"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash1Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            //--------------------------------------------------
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash2"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash2"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet2"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash2Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash2"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash2"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet2"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash2Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash3"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash3"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet3"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash3Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash3"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash3"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet3"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash3Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash4"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash4"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet4"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash4Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash4"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash4"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet4"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash4Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash5"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash5"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet5"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash5Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash5"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash5"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet5"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash5Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash6"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash6"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet6"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash6Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash6"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash6"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet6"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash6Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash7"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash7"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet7"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash7Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash7"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash7"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet7"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash7Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash8"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash8"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet8"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash8Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash8"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash8"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet8"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash8Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash9"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash9"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet9"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash9Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash9"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash9"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet9"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash9Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash10"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash10"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet10"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash10Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash10"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash10"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet10"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash10Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash11"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash11"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet11"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash11Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash11"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash11"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet11"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash11Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash12"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash12"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet12"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash12Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash12"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash12"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet12"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash12Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash13"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash13"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet13"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash13Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash13"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash13"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet13"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash13Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash14"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash14"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet14"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash14Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash14"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash14"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet14"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash14Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash15"]) != "")
                                    csvTxtBody = csvTxt1 + ds.Tables[0].Rows[i]["PettyCash15"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet15"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash15Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash15"]) != "")
                                    csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash15"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet15"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash15Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            if (csvTxtBody == "")
                            {
                                csvTxtBody = csvTxt1 + "Petty Cash VAT,1," + ds.Tables[0].Rows[i]["PettyCashVAT"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCashVATNominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            }
                            else
                            {
                                csvTxtBody += csvTxt2 + "Petty Cash VAT,1," + ds.Tables[0].Rows[i]["PettyCashVAT"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCashVATNominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "";
                            }
                            csvTxt = csvTxtHead + csvTxtBody;
                        }
                        else
                        {

                            if (Convert.ToString(ds.Tables[0].Rows[i]["Field1name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field1name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field1"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field1Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            if (Convert.ToString(ds.Tables[0].Rows[i]["Field2name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field2name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field2"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field2Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";
                            if (Convert.ToString(ds.Tables[0].Rows[i]["Field3name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field3name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field3"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field3Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field4name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field4name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field4"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field4Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field5name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field5name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field5"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field5Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field6name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field6name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field6"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field6Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field7name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field7name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field7"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field7Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";



                            csvTxtBody += csvTxt2 + "Sales VAT,1," + ds.Tables[0].Rows[i]["vat"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["SalesVATNominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field8name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field8name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field8"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field8Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field9name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field9name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field9"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field9Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field10name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field10name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field10"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field10Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field11name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field11name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field11"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field11Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["field12name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field12name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field12"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field12Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["Field13name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field13name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field13"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field13Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["Field14name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field14name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field14"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field14Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["Field15name"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["Field15name"].ToString() + ",1," + ds.Tables[0].Rows[i]["field15"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["Field15Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";




                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash1"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash1"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet1"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash1Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash2"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash2"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet2"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash2Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash3"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash3"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet3"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash3Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash4"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash4"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet4"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash4Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash5"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash5"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet5"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash5Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash6"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash6"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet6"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash6Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash7"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash7"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet7"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash7Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash8"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash8"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet8"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash8Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash9"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash9"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet9"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash9Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash10"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash10"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet10"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash10Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash11"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash11"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet11"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash11Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash12"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash12"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet12"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash12Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash13"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash13"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet13"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash13Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash14"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash14"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet14"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash14Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            if (Convert.ToString(ds.Tables[0].Rows[i]["PettyCash15"]) != "")
                                csvTxtBody += csvTxt2 + ds.Tables[0].Rows[i]["PettyCash15"].ToString() + ",1," + ds.Tables[0].Rows[i]["PettyCashNet15"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCash15Nominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "\n";

                            csvTxtBody += csvTxt2 + "Petty Cash VAT,1," + ds.Tables[0].Rows[i]["PettyCashVAT"].ToString() + ",Zero," + ds.Tables[0].Rows[i]["PettyCashVATNominal"].ToString() + "," + ds.Tables[0].Rows[i]["Department"].ToString() + "";


                            csvTxt = csvTxtBody;
                        }
                        csvTxtBody = "";
                        StreamWriter SW;
                        SW = new StreamWriter(fs1);
                        SW.WriteLine(csvTxt);
                        SW.Flush();
                        csvTxt = "";
                    }
                }
                else
                {
                    csvTxtBody = csvTxt1 + ",,,,, \n";
                    csvTxt = csvTxtHead + csvTxtBody;
                    csvTxtBody = "";
                    StreamWriter SW;
                    SW = new StreamWriter(fs1);
                    SW.WriteLine(csvTxt);
                    SW.Flush();
                    csvTxt = "";
                }
            }
            catch (Exception ex)
            { string err = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
                fs1.Close();
                ds = null;
                DownloadWTRExport(flname);
                //DeleteFile(flname);
            }
        }
        public void DownloadWTRExport(string flnm)
        {
            string fpath = ConfigurationManager.AppSettings["WTRExportPath_ETC"].Trim() + flnm;
            try
            {
                Context.Response.ContentType = "application/save";
                Context.Response.AddHeader("content-disposition", "attachment; filename=" + flnm);
                FileStream fs = new FileStream(fpath, FileMode.Open, FileAccess.Read);
                long FileSize = fs.Length;
                byte[] getContent = new byte[(int)FileSize];

                fs.Read(getContent, 0, (int)fs.Length);
                fs.Close();

                Context.Response.BinaryWrite(getContent);
                Context.Response.Flush();
                Context.Response.Close();
            }
            catch (Exception ex) { throw (ex); }
        }
        public void DeleteFile(string flnm)
        {
            string fpath = ConfigurationManager.AppSettings["WTRExportPath_ETC"].Trim() + flnm;
            if (System.IO.File.Exists(fpath))
            {
                try
                {
                    System.IO.File.Delete(fpath);
                }
                catch (System.IO.IOException e)
                {
                    return;
                }
            }
        }
        private void LoadCompany()
        {
            Company objCompany = new Company();
            DataTable dt = new DataTable();
            try
            {
                ddlCompany.Items.Clear();
                dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                if (dt.Rows.Count > 0)
                {
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.SelectedValue = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "WTR Export  LoadCompany : <br /> " + ex.Message.ToString());
            }
            finally
            {
                dt = null;
                ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));
                ddlWeekStartDate.Items.Insert(0, new ListItem("Select Week Start Date", "0"));
                ddlCompany.SelectedValue = "0";
            }
        }
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadWeekSartDate();
        }
        private void LoadWeekSartDate()
        {
            ddlWeekStartDate.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_WeekSartDateNew", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlWeekStartDate.DataSource = ds;
                    ddlWeekStartDate.DataBind();
                }
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "WTR Export   LoadWeekSartDate: <br /> " + ex.Message.ToString());
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
                ddlWeekStartDate.Items.Insert(0, new ListItem("Select Week Start Date", "0"));
            }
        }

        private void ddlWeekStartDate_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ViewState["DepartmentName"] = "";
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentListExport_CMS", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            sqlDA.SelectCommand.Parameters.Add("@StartWeekDate", Convert.ToString(ddlWeekStartDate.SelectedValue));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(ViewState["DepartmentName"]).Trim() != "")
                            ViewState["DepartmentName"] = Convert.ToString(ViewState["DepartmentName"]).Trim() + ";" + Convert.ToString(ds.Tables[0].Rows[i]["Department"]);
                        else
                            ViewState["DepartmentName"] = Convert.ToString(ds.Tables[0].Rows[i]["Department"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ETH.Web.ETC.CMS.CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "WTR Export   ddlWeekStartDate_SelectedIndexChanged: <br /> " + ex.Message.ToString());
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
            }
        }
    }
}
