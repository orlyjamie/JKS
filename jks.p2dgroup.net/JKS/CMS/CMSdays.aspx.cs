using System;
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
    /// Created by : Rinku Santra
    /// Created date: 01-06-2012
    /// Summary description for CMSdays.
    /// </summary>
    public class CMSdays : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Label lblCovers1;
        protected System.Web.UI.WebControls.Label lblCovers2;
        protected System.Web.UI.WebControls.Label lblCovers3;
        protected System.Web.UI.WebControls.Label lblFiled1;
        protected System.Web.UI.WebControls.Label lblField8;
        protected System.Web.UI.WebControls.Label lblField2;
        protected System.Web.UI.WebControls.Label lblField9;
        protected System.Web.UI.WebControls.Label lblField3;
        protected System.Web.UI.WebControls.Label lblField10;
        protected System.Web.UI.WebControls.Label lblField4;
        protected System.Web.UI.WebControls.Label lblField11;
        protected System.Web.UI.WebControls.Label lblField5;
        protected System.Web.UI.WebControls.Label lblField12;
        protected System.Web.UI.WebControls.Label lblField6;
        protected System.Web.UI.WebControls.Label lblField13;
        protected System.Web.UI.WebControls.Label lblField7;
        protected System.Web.UI.WebControls.Label lblField14;
        protected System.Web.UI.WebControls.Label lblField15;
        protected System.Web.UI.WebControls.Label lblPettycash1;
        protected System.Web.UI.WebControls.Label lblPettycash2;
        protected System.Web.UI.WebControls.Label lblPettycash3;
        protected System.Web.UI.WebControls.Label lblPettycash4;
        protected System.Web.UI.WebControls.Label lblPettycash5;
        protected System.Web.UI.WebControls.Label lblPettycash6;
        protected System.Web.UI.WebControls.Label lblPettycash7;
        protected System.Web.UI.WebControls.Label lblPettycash8;
        protected System.Web.UI.WebControls.Label lblPettycash9;
        protected System.Web.UI.WebControls.Label lblPettycash10;
        protected System.Web.UI.WebControls.Label lblPettycash11;
        protected System.Web.UI.WebControls.Label lblPettycash12;
        protected System.Web.UI.WebControls.Label lblPettycash13;
        protected System.Web.UI.WebControls.Label lblPettycash14;
        protected System.Web.UI.WebControls.Label lblPettycash15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtCompany;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWeekNo;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMonday;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtDepartment;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPeriodNo;
        protected System.Web.UI.HtmlControls.HtmlInputText txtDate;
        protected System.Web.UI.HtmlControls.HtmlInputText txtCovers1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtCovers2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtCovers3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalCovers;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtNetTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtVAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotal1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotal2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStatusCal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash1Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash1VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash1Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash1Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash2Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash2VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash2Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash2Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash3Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash3VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash3Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash3Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash4Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash4VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash4Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash4Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash5Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash5VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash5Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash5Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash6Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash6VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash6Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash6Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash7Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash7VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash7Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash7Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash8Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash8VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash8Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash8Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash9Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash9VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash9Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash9Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash10Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash10VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash10Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash10Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash11Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash11VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash11Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash11Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash12Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash12VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash12Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash12Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash13Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash13VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash13Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash13Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash14Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash14VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash14Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash14Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash15Net;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash15VAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash15Gross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPettycash15Description;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalVAT;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtVariance;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnBack;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnSave;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSavedBy;
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaveDate;
        protected System.Web.UI.HtmlControls.HtmlInputText perVat;
        protected string strPettycash1Info = "";
        protected string strPettycash2Info = "";
        protected string strPettycash3Info = "";
        protected string strPettycash4Info = "";
        protected string strPettycash5Info = "";
        protected string strPettycash6Info = "";
        protected string strPettycash7Info = "";
        protected string strPettycash8Info = "";
        protected string strPettycash9Info = "";
        protected string strPettycash10Info = "";
        protected string strPettycash11Info = "";
        protected string strPettycash12Info = "";
        protected string strPettycash13Info = "";
        protected string strPettycash14Info = "";
        protected string strPettycash15Info = "";

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Page.RegisterStartupScript("reg", "<script>alert('Sorry,your Session is expired');</script>");
                Response.Redirect("../../etcdefault.aspx");
            }
            if (!IsPostBack)
            {

                if (Request.QueryString["dep"] == null)
                    Response.Redirect("CMS.aspx");
                if (Request.QueryString["icon"] == null)
                    Response.Redirect("WeeklySummary.aspx?dep=" + Convert.ToInt32(Request.QueryString["dep"]));

                if (Convert.ToString(Request.QueryString["dep"]) != "")
                {
                    int dayid = Convert.ToInt32(Request.QueryString["icon"]);
                    int depID = 0;
                    try
                    {
                        depID = Convert.ToInt32(CMScode.DecryptString(Request.QueryString["dep"]));
                    }
                    catch (Exception ex)
                    {
                        CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "DecryptString : <br /> " + ex.Message.ToString());
                        if (Session["weekstartdate"] == null)
                            Response.Redirect("CMS.aspx");
                        else
                            Response.Redirect("CMSHistroy.aspx");
                    }
                    finally
                    {
                        LoadLabelName(depID);
                        LoadData(depID, dayid);
                    }
                }
                else
                {
                    if (Session["weekstartdate"] == null)
                        Response.Redirect("CMS.aspx");
                    else
                        Response.Redirect("CMSHistroy.aspx");
                }
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
            this.btnBack.ServerClick += new System.EventHandler(this.btnBack_ServerClick);
            this.btnSave.ServerClick += new System.EventHandler(this.btnSave_ServerClick);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btnSave_ServerClick(object sender, System.EventArgs e)
        {
            if (perVat.Value.ToString() == "" || Convert.ToDecimal(perVat.Value.ToString().Substring(0, perVat.Value.Length - 1)) < 15 || Convert.ToDecimal(perVat.Value.ToString().Substring(0, perVat.Value.Length - 1)) > 20)
            {
                Page.RegisterStartupScript("reg", "<script>FinalCal2();</script>");
                return;
            }
            else
            {
                int iReturnValue = 0;
                SqlCommand sqlCmd = null;
                SqlParameter sqlReturnParam = null;
                string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                SqlConnection sqlConn = new SqlConnection(ConsString);
                sqlCmd = new SqlCommand("Sp_SaveCMSdays", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (Convert.ToString(ViewState["CMSDataID"]) != "")
                    sqlCmd.Parameters.Add("@CMSDataID", Convert.ToInt32(ViewState["CMSDataID"]));
                else
                    sqlCmd.Parameters.Add("@CMSDataID", Convert.ToInt32(0));

                if (Convert.ToString(Session["CMSWeekID"]) != "")
                    sqlCmd.Parameters.Add("@CMSWeekID", Convert.ToInt32(Session["CMSWeekID"]));
                else
                    sqlCmd.Parameters.Add("@CMSWeekID", Convert.ToInt32(0));
                if (txtDate.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Date", txtDate.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Date", DBNull.Value);

                sqlCmd.Parameters.Add("@SavedBy", Convert.ToInt32(Session["UserID"]));
                if (txtCovers1.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Covers1", txtCovers1.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Covers1", DBNull.Value);
                if (txtCovers2.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Covers2", txtCovers2.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Covers2", DBNull.Value);
                if (txtCovers3.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Covers3", txtCovers3.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Covers3", DBNull.Value);
                if (txtField1.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field1", txtField1.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field1", DBNull.Value);
                if (txtField2.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field2", txtField2.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field2", DBNull.Value);
                if (txtField3.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field3", txtField3.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field3", DBNull.Value);
                if (txtField4.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field4", txtField4.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field4", DBNull.Value);

                if (txtField5.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field5", txtField5.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field5", DBNull.Value);
                if (txtField6.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field6", txtField6.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field6", DBNull.Value);
                if (txtField7.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field7", txtField7.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field7", DBNull.Value);
                if (txtVAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@vat", txtVAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@vat", DBNull.Value);

                if (txtField8.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field8", txtField8.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field8", DBNull.Value);
                if (txtField9.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field9", txtField9.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field9", DBNull.Value);
                if (txtField10.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field10", txtField10.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field10", DBNull.Value);
                if (txtField11.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field11", txtField11.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field11", DBNull.Value);
                if (txtField12.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field12", txtField12.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field12", DBNull.Value);
                if (txtField13.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field13", txtField13.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field13", DBNull.Value);
                if (txtField14.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field14", txtField14.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field14", DBNull.Value);
                if (txtField15.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@Field15", txtField15.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@Field15", DBNull.Value);
                if (txtPettyCashTotal.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashTotal", txtPettyCashTotal.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashTotal", DBNull.Value);

                if (txtPettycash1Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet1", txtPettycash1Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet1", DBNull.Value);
                if (txtPettycash1VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT1", txtPettycash1VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT1", DBNull.Value);
                if (txtPettycash1Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross1", txtPettycash1Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross1", DBNull.Value);
                if (txtPettycash1Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription1", txtPettycash1Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription1", DBNull.Value);
                if (txtPettycash2Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet2", txtPettycash2Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet2", DBNull.Value);
                if (txtPettycash2VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT2", txtPettycash2VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT2", DBNull.Value);
                if (txtPettycash2Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross2", txtPettycash2Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross2", DBNull.Value);
                if (txtPettycash2Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription2", txtPettycash2Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription2", DBNull.Value);

                if (txtPettycash3Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet3", txtPettycash3Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet3", DBNull.Value);
                if (txtPettycash3VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT3", txtPettycash3VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT3", DBNull.Value);
                if (txtPettycash3Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross3", txtPettycash3Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross3", DBNull.Value);
                if (txtPettycash3Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription3", txtPettycash3Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription3", DBNull.Value);

                if (txtPettycash4Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet4", txtPettycash4Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet4", DBNull.Value);
                if (txtPettycash4VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT4", txtPettycash4VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT4", DBNull.Value);
                if (txtPettycash4Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross4", txtPettycash4Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross4", DBNull.Value);
                if (txtPettycash4Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription4", txtPettycash4Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription4", DBNull.Value);

                if (txtPettycash5Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet5", txtPettycash5Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet5", DBNull.Value);
                if (txtPettycash5VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT5", txtPettycash5VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT5", DBNull.Value);
                if (txtPettycash5Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross5", txtPettycash5Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross5", DBNull.Value);
                if (txtPettycash5Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription5", txtPettycash5Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription5", DBNull.Value);

                if (txtPettycash6Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet6", txtPettycash6Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet6", DBNull.Value);
                if (txtPettycash6VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT6", txtPettycash6VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT6", DBNull.Value);
                if (txtPettycash6Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross6", txtPettycash6Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross6", DBNull.Value);
                if (txtPettycash6Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription6", txtPettycash6Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription6", DBNull.Value);

                if (txtPettycash7Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet7", txtPettycash7Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet7", DBNull.Value);
                if (txtPettycash7VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT7", txtPettycash7VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT7", DBNull.Value);
                if (txtPettycash7Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross7", txtPettycash7Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross7", DBNull.Value);
                if (txtPettycash7Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription7", txtPettycash7Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription7", DBNull.Value);

                if (txtPettycash8Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet8", txtPettycash8Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet8", DBNull.Value);
                if (txtPettycash8VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT8", txtPettycash8VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT8", DBNull.Value);
                if (txtPettycash8Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross8", txtPettycash8Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross8", DBNull.Value);
                if (txtPettycash8Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription8", txtPettycash8Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription8", DBNull.Value);
                if (txtPettycash9Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet9", txtPettycash9Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet9", DBNull.Value);
                if (txtPettycash9VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT9", txtPettycash9VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT9", DBNull.Value);
                if (txtPettycash9Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross9", txtPettycash9Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross9", DBNull.Value);
                if (txtPettycash9Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription9", txtPettycash9Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription9", DBNull.Value);
                if (txtPettycash10Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet10", txtPettycash10Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet10", DBNull.Value);
                if (txtPettycash10VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT10", txtPettycash10VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT10", DBNull.Value);
                if (txtPettycash10Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross10", txtPettycash10Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross10", DBNull.Value);
                if (txtPettycash10Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription10", txtPettycash10Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription10", DBNull.Value);

                if (txtPettycash11Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet11", txtPettycash11Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet11", DBNull.Value);
                if (txtPettycash11VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT11", txtPettycash11VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT11", DBNull.Value);
                if (txtPettycash11Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross11", txtPettycash11Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross11", DBNull.Value);
                if (txtPettycash11Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription11", txtPettycash11Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription11", DBNull.Value);

                if (txtPettycash12Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet12", txtPettycash12Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet12", DBNull.Value);
                if (txtPettycash12VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT12", txtPettycash12VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT12", DBNull.Value);
                if (txtPettycash12Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross12", txtPettycash12Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross12", DBNull.Value);
                if (txtPettycash12Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription12", txtPettycash12Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription12", DBNull.Value);

                if (txtPettycash13Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet13", txtPettycash13Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet13", DBNull.Value);
                if (txtPettycash13VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT13", txtPettycash13VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT13", DBNull.Value);
                if (txtPettycash13Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross13", txtPettycash13Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross13", DBNull.Value);
                if (txtPettycash13Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription13", txtPettycash13Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription13", DBNull.Value);

                if (txtPettycash14Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet14", txtPettycash14Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet14", DBNull.Value);
                if (txtPettycash14VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT14", txtPettycash14VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT14", DBNull.Value);
                if (txtPettycash14Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross14", txtPettycash14Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross14", DBNull.Value);
                if (txtPettycash14Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription14", txtPettycash14Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription14", DBNull.Value);

                if (txtPettycash15Net.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashNet15", txtPettycash15Net.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashNet15", DBNull.Value);
                if (txtPettycash15VAT.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashVAT15", txtPettycash15VAT.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashVAT15", DBNull.Value);
                if (txtPettycash15Gross.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashGross15", txtPettycash15Gross.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashGross15", DBNull.Value);
                if (txtPettycash15Description.Value.Trim() != "")
                    sqlCmd.Parameters.Add("@PettyCashDescription15", txtPettycash15Description.Value.Trim());
                else
                    sqlCmd.Parameters.Add("@PettyCashDescription15", DBNull.Value);

                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                try
                {
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
                }
                catch (Exception ex)
                {
                    ETH.Web.ETC.CMS.CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS Day SaveData : <br /> " + ex.Message.ToString());
                }
                finally
                {
                    sqlReturnParam = null;
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
                if (iReturnValue > 0)
                {
                    if (iReturnValue > 1)
                        Response.Write("<script>alert('Updated successfully');</script>");
                    else
                        Response.Write("<script>alert('Saved successfully');</script>");

                    LoadData(Convert.ToInt32(CMScode.DecryptString(Request.QueryString["dep"])), Convert.ToInt32(Request.QueryString["icon"]));
                }
                else
                {
                    Response.Write("<script>alert('Error in saved');</script>");
                }
            }

        }

        private void btnBack_ServerClick(object sender, System.EventArgs e)
        {
            //Response.Redirect("WeeklySummary.aspx?dep="+Convert.ToString(Request.QueryString["dep"]));
        }

        private void LoadLabelName(int depID)
        {
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlcon = new SqlConnection(ConsString);
            SqlDataAdapter dap = new SqlDataAdapter("Sp_CMSFieldNames", sqlcon);
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.Add("@DepartmentID", depID);
            DataSet ds = new DataSet();
            try
            {
                sqlcon.Open();
                dap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblCovers1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Covers1"]);
                    lblCovers2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Covers2"]);
                    lblCovers3.Text = Convert.ToString(ds.Tables[0].Rows[0]["Covers3"]);
                    lblFiled1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field1"]);
                    lblField2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field2"]);
                    lblField3.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field3"]);
                    lblField4.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field4"]);
                    lblField5.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field5"]);
                    lblField6.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field6"]);
                    lblField7.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field7"]);
                    lblField8.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field8"]);
                    lblField9.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field9"]);
                    lblField10.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field10"]);
                    lblField11.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field11"]);
                    lblField12.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field12"]);
                    lblField13.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field13"]);
                    lblField14.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field14"]);
                    lblField15.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field15"]);
                    lblPettycash1.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash1"]);
                    lblPettycash2.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash2"]);
                    lblPettycash3.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash3"]);
                    lblPettycash4.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash4"]);
                    lblPettycash5.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash5"]);
                    lblPettycash6.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash6"]);
                    lblPettycash7.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash7"]);
                    lblPettycash8.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash8"]);
                    lblPettycash9.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash9"]);
                    lblPettycash10.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash10"]);
                    lblPettycash11.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash11"]);
                    lblPettycash12.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash12"]);
                    lblPettycash13.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash13"]);
                    lblPettycash14.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash14"]);
                    lblPettycash15.Text = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash15"]);

                    strPettycash1Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash1info"]);
                    strPettycash2Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash2info"]);
                    strPettycash3Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash3info"]);
                    strPettycash4Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash4info"]);
                    strPettycash5Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash5info"]);
                    strPettycash6Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash6info"]);
                    strPettycash7Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash7info"]);
                    strPettycash8Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash8info"]);
                    strPettycash9Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash9info"]);
                    strPettycash10Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash10info"]);
                    strPettycash11Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash11info"]);
                    strPettycash12Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash12info"]);
                    strPettycash13Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash13info"]);
                    strPettycash14Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash14info"]);
                    strPettycash15Info = Convert.ToString(ds.Tables[0].Rows[0]["PettyCash15info"]);

                }
            }
            catch (Exception ex)
            {
                ETH.Web.ETC.CMS.CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS Day LabelName : <br /> " + ex.Message.ToString());
            }
            finally
            {
                ds = null;
                sqlcon.Close();
            }

        }

        private void LoadData(int depID, int dayid)
        {

            if (dayid == 1)
                txtMonday.Value = "MONDAY";
            else if (dayid == 2)
                txtMonday.Value = "TUESDAY";
            else if (dayid == 3)
                txtMonday.Value = "WEDNESDAY";
            else if (dayid == 4)
                txtMonday.Value = "THURSDAY";
            else if (dayid == 5)
                txtMonday.Value = "FRIDAY";
            else if (dayid == 6)
                txtMonday.Value = "SATURDAY";
            else if (dayid == 7)
                txtMonday.Value = "SUNDAY";

            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlcon = new SqlConnection(ConsString);
            SqlDataAdapter dap = new SqlDataAdapter("Sp_CMSdays", sqlcon);
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.Add("@CMSWeekID", Convert.ToInt32(Session["CMSWeekID"]));
            dap.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            dap.SelectCommand.Parameters.Add("@DepartmentID", depID);
            dap.SelectCommand.Parameters.Add("@daydiff", dayid);
            DataSet ds = new DataSet();
            try
            {
                sqlcon.Open();
                dap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtCompany.Value = Convert.ToString(ds.Tables[0].Rows[0]["CompanyName"]);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txtDepartment.Value = Convert.ToString(ds.Tables[1].Rows[0]["Department"]);
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    if (Convert.ToString(ds.Tables[2].Rows[0]["WeekNo"]) != "")
                        txtWeekNo.Value = Convert.ToString(ds.Tables[2].Rows[0]["WeekNo"]);
                    if (Convert.ToString(ds.Tables[2].Rows[0]["Period"]) != "")
                        txtPeriodNo.Value = Convert.ToString(ds.Tables[2].Rows[0]["Period"]);
                    if (Convert.ToString(ds.Tables[2].Rows[0]["WeekStartDate"]) != "")
                        txtDate.Value = Convert.ToDateTime(ds.Tables[2].Rows[0]["WeekStartDate"]).AddDays(dayid - 1).ToString("dd/MM/yyyy");
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    if (Convert.ToString(ds.Tables[3].Rows[0]["CMSDataID"]) != "")
                        ViewState["CMSDataID"] = Convert.ToString(ds.Tables[3].Rows[0]["CMSDataID"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["DateSaved"]) != "")
                        txtSaveDate.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["DateSaved"]).ToString("dd/MM/yyyy HH:mm");
                    if (Convert.ToString(ds.Tables[3].Rows[0]["SavedBy"]) != "")
                        txtSavedBy.Value = Convert.ToString(ds.Tables[3].Rows[0]["SavedBy"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Date"]) != "")
                        txtDate.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Covers1"]) != "")
                        txtCovers1.Value = Convert.ToString(ds.Tables[3].Rows[0]["Covers1"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Covers2"]) != "")
                        txtCovers2.Value = Convert.ToString(ds.Tables[3].Rows[0]["Covers2"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Covers3"]) != "")
                        txtCovers3.Value = Convert.ToString(ds.Tables[3].Rows[0]["Covers3"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field1"]) != "")
                        txtField1.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field1"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field2"]) != "")
                        txtField2.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field2"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field3"]) != "")
                        txtField3.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field3"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field4"]) != "")
                        txtField4.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field4"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field5"]) != "")
                        txtField5.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field5"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field6"]) != "")
                        txtField6.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field6"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field7"]) != "")
                        txtField7.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field7"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field8"]) != "")
                        txtField8.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field8"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field9"]) != "")
                        txtField9.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field9"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field10"]) != "")
                        txtField10.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field10"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field11"]) != "")
                        txtField11.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field11"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field12"]) != "")
                        txtField12.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field12"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field13"]) != "")
                        txtField13.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field13"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field14"]) != "")
                        txtField14.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field14"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Field15"]) != "")
                        txtField15.Value = Convert.ToString(ds.Tables[3].Rows[0]["Field15"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashTotal"]) != "")
                        txtPettyCashTotal.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashTotal"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["vat"]) != "")
                        txtVAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["vat"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet1"]) != "")
                        txtPettycash1Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet1"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT1"]) != "")
                        txtPettycash1VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT1"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross1"]) != "")
                        txtPettycash1Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross1"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription1"]) != "")
                        txtPettycash1Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription1"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet2"]) != "")
                        txtPettycash2Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet2"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT2"]) != "")
                        txtPettycash2VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT2"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross2"]) != "")
                        txtPettycash2Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross2"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription2"]) != "")
                        txtPettycash2Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription2"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet3"]) != "")
                        txtPettycash3Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet3"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT3"]) != "")
                        txtPettycash3VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT3"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross3"]) != "")
                        txtPettycash3Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross3"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription3"]) != "")
                        txtPettycash3Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription3"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet4"]) != "")
                        txtPettycash4Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet4"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT4"]) != "")
                        txtPettycash4VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT4"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross4"]) != "")
                        txtPettycash4Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross4"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription4"]) != "")
                        txtPettycash4Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription4"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet5"]) != "")
                        txtPettycash5Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet5"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT5"]) != "")
                        txtPettycash5VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT5"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross5"]) != "")
                        txtPettycash5Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross5"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription5"]) != "")
                        txtPettycash5Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription5"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet6"]) != "")
                        txtPettycash6Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet6"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT6"]) != "")
                        txtPettycash6VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT6"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross6"]) != "")
                        txtPettycash6Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross6"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription6"]) != "")
                        txtPettycash6Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription6"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet7"]) != "")
                        txtPettycash7Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet7"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT7"]) != "")
                        txtPettycash7VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT7"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross7"]) != "")
                        txtPettycash7Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross7"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription7"]) != "")
                        txtPettycash7Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription7"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet8"]) != "")
                        txtPettycash8Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet8"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT8"]) != "")
                        txtPettycash8VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT8"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross8"]) != "")
                        txtPettycash8Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross8"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription8"]) != "")
                        txtPettycash8Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription8"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet9"]) != "")
                        txtPettycash9Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet9"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT9"]) != "")
                        txtPettycash9VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT9"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross9"]) != "")
                        txtPettycash9Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross9"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription9"]) != "")
                        txtPettycash9Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription9"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet10"]) != "")
                        txtPettycash10Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet10"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT10"]) != "")
                        txtPettycash10VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT10"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross10"]) != "")
                        txtPettycash10Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross10"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription10"]) != "")
                        txtPettycash10Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription10"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet11"]) != "")
                        txtPettycash11Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet11"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT11"]) != "")
                        txtPettycash11VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT11"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross11"]) != "")
                        txtPettycash11Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross11"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription11"]) != "")
                        txtPettycash11Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription11"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet12"]) != "")
                        txtPettycash12Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet12"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT12"]) != "")
                        txtPettycash12VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT12"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross12"]) != "")
                        txtPettycash12Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross12"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription12"]) != "")
                        txtPettycash12Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription12"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet13"]) != "")
                        txtPettycash13Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet13"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT13"]) != "")
                        txtPettycash13VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT13"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross13"]) != "")
                        txtPettycash13Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross13"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription13"]) != "")
                        txtPettycash13Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription13"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet14"]) != "")
                        txtPettycash14Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet14"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT14"]) != "")
                        txtPettycash14VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT14"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross14"]) != "")
                        txtPettycash14Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross14"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription14"]) != "")
                        txtPettycash14Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription14"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet15"]) != "")
                        txtPettycash15Net.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashNet15"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT15"]) != "")
                        txtPettycash15VAT.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashVAT15"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross15"]) != "")
                        txtPettycash15Gross.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashGross15"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription15"]) != "")
                        txtPettycash15Description.Value = Convert.ToString(ds.Tables[3].Rows[0]["PettyCashDescription15"]);

                }
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS Day LoadData : <br /> " + ex.Message.ToString());
            }
            finally
            {
                ds = null;
                dap.Dispose();
                sqlcon.Close();
                Page.RegisterStartupScript("reg", "<script>FinalCal();</script>");
            }


        }
    }
}
