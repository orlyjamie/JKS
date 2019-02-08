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
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using CBSolutions.ETH.Web.UniversalMusic.downloadDB;
using CBSolutions.ETH.Web;
using System.Globalization;
using System.Text;
using JKS;
using System.Diagnostics;

public partial class ETC_History_ExportInvoiceNew : System.Web.UI.Page
{
    #region User Defined Variables
    protected SqlConnection sqlConn = null;
    protected SqlDataAdapter sqlDA = null;
    protected DataSet ds = null;
    protected DataTable objDataTable = null;
   // downloadDB objdownloadDB = new downloadDB();
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
    //added by kd on 03/9/2018
    public int ParentCompanyID = 180918;
     public string ApplicationName = "";
        private DataAccessLayer DA = null;
        //ParentCompanyID = 180918;//JKS parent company id 
        public ETC_History_ExportInvoiceNew()
        {
            DA = new DataAccessLayer();
        }
    private void LoadData()
    {
        DataTable dtbl = null;
        sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

       // dtbl = objdownloadDB.GetBuyerCompanyListDropDown(Convert.ToInt32(Session["CompanyID"]));
  //commented by kd
        //if (dtbl != null)
        //{
        //    ddlBuyerCompany.DataSource = dtbl;
        //    ddlBuyerCompany.DataBind();

        //}
        //ddlBuyerCompany.Items.Insert(0, new ListItem("Select Company", "0"));

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

    public void ErrorLog(string sPathName, string sErrMsg)
    {
        StreamWriter sw = new StreamWriter(sPathName, true);
        sw.WriteLine(DateTime.Now + ": " + sErrMsg);
        sw.Flush();
        sw.Close();
    }

    protected void btnExportNew_Click(object sender, EventArgs e)
    {
        //string fpath = ConfigurationManager.AppSettings["InvoiceExportPath_JKS"].Trim() + @"\" + ddlBuyerCompany.SelectedItem.ToString() + Convert.ToDateTime(DateTime.Now.ToLongDateString()).ToString("ddMMyyyy") + Convert.ToDateTime(DateTime.Now.ToLongTimeString()).ToString("HHmmss") + Session["UserID"].ToString() + ".csv";
        //Stream fs = File.Create(fpath);
        //string FileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fpath);
        //fs.Close();
        //Stream fs1 = File.Open(fpath, FileMode.Open, FileAccess.ReadWrite);

        ///*------Duplicate Copy Path details----------*/
        //#region Duplicate Copy Path Details
        //string fpath2 = ConfigurationManager.AppSettings["InvoiceExportPathDuplicate_JKS"].Trim() + @"\" + ddlBuyerCompany.SelectedItem.ToString() + Convert.ToDateTime(DateTime.Now.ToLongDateString()).ToString("ddMMyyyy") + Convert.ToDateTime(DateTime.Now.ToLongTimeString()).ToString("HHmmss") + Session["UserID"].ToString() + ".csv";
        //Stream fs2 = File.Create(fpath2);
        //string FileNameWithoutExtension2 = System.IO.Path.GetFileNameWithoutExtension(fpath2);
        //fs2.Close();
        //Stream fs3 = File.Open(fpath2, FileMode.Open, FileAccess.ReadWrite);
        //#endregion
        ///*-------------------------------------------*/
     

        //string csvTxt = "";
        //string ConString = ConfigurationManager.AppSettings["ConnectionString"].Trim();
        //SqlConnection sqlConn = new SqlConnection(ConString);

        //#region sage200

        ///*SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_JKS", sqlConn);//Sp_ExportInvoice_CTB_NewSystem
        //sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        //sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(ddlBuyerCompany.SelectedValue));
        //sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
        //DataSet ds = new DataSet();
        //try
        //{
        //    sqlConn.Open();
        //    sqlDA.Fill(ds);
        //    //added by kuntalkarar on 25thJanuary2017
        //    //csvTxt = "Type,Supplier a/c ref,Nominal a/c ref,Dept Code,Date,Reference,Details,Net Amount,Tax Code,Tax Amount,Exchange Rate,Extra Ref,User,Project Ref,Cost code Ref\n";
        //    //        a           b              c            d       e      f       g          h        i         j            k            l      m      n            o  
        //    csvTxt = "AccountNumber,CBAccountNumber,DaysDiscountValid,DiscountValue,DiscountPercentage,DueDate,GoodsValueInAccountCurrency,PurControlValueInBaseCurrency,DocumentToBaseCurrencyRate,DocumentToAccountCurrencyRate,PostedDate,QueryCode,TransactionReference,SecondReference,Source,SYSTraderTranType,TransactionDate,UniqueReferenceNumber,UserNumber,TaxValue,TaxDiscountValue,SYSTraderGenerationReasonType,GoodsValueInBaseCurrency,NominalAnalysisTransactionValue/1,NominalAnalysisNominalAccountNumber/1,NominalAnalysisNominalCostCentre/1,NominalAnalysisNominalDepartment/1,NominalAnalysisNominalAnalysisNarrative/1,NominalAnalysisTransactionAnalysisCode/1,NominalAnalysisTransactionValue/2,NominalAnalysisNominalAccountNumber/2,NominalAnalysisNominalCostCentre/2,NominalAnalysisNominalDepartment/2,NominalAnalysisNominalAnalysisNarrative/2,TaxAnalysisTaxRate/1,TaxAnalysisGoodsValueBeforeDiscount/1,TaxAnalysisDiscountValue/1,TaxAnalysisDiscountPercentage/1,TaxAnalysisTaxOnGoodsValue/1,TaxAnalysisTaxDiscountValue/1,TaxAnalysisTaxRate/2,TaxAnalysisGoodsValueBeforeDiscount/2,TaxAnalysisDiscountValue/2,TaxAnalysisDiscountPercentage/2,TaxAnalysisTaxOnGoodsValue/2,TaxAnalysisTaxDiscountValue/2,ChequeCurrencyName,ChequeToBankExchangeRate,ChequeValueInChequeCurrency,IsSettledImmediately,IsVATAdjustmentDocumentExpected\n";

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //       // ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "Count:-"+Convert.ToString(ds.Tables[0].Rows.Count));
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Supplier a/c ref"].ToString() + "\"" + ",";//a
        //            csvTxt = csvTxt + ""   + ",";//b
        //            csvTxt = csvTxt +  "" + ",";//c
        //            csvTxt = csvTxt +  "" + ",";//d
        //            csvTxt = csvTxt +  "" + ",";//e
        //            csvTxt = csvTxt +   "" + ",";//f  // "\"" added so the file does not get corrupted if there is a comma in the text.
        //           // ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "1");
        //            string columnGnH = "";
        //            decimal ntamt = 0,txAmt=0;
        //            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Net Amount"].ToString()))// == null || ds.Tables[0].Rows[i]["Net Amount"].ToString() == "")
        //            {
                       
        //                ntamt = 0;
        //            }
        //            else
        //            {
        //                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "netAMt-"+Convert.ToString(ds.Tables[0].Rows[i]["Net Amount"].ToString()));
        //                ntamt=Convert.ToDecimal((string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Net Amount"].ToString())?"0" :ds.Tables[0].Rows[i]["Net Amount"].ToString()));
        //            }


        //           if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Tax Amount"].ToString()))// == "" || ds.Tables[0].Rows[i]["Tax Amount"].ToString() == null)
        //            {
        //               // ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "3");
        //                txAmt = 0;
        //            }
        //            else
        //            {
        //                txAmt = Convert.ToDecimal(ds.Tables[0].Rows[i]["Tax Amount"].ToString());
        //            }

        //           columnGnH = Convert.ToString(ntamt + txAmt);
        //          // ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "4");
        //            csvTxt = csvTxt + columnGnH + ",";//g
        //            csvTxt = csvTxt + columnGnH + ",";//h
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Exchange Rate"].ToString() + ",";//i
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Exchange Rate"].ToString() + ",";//j
        //            csvTxt = csvTxt + Convert.ToString(System.DateTime.Now.ToString("dd/MM/yy")) + ",";//k
        //            csvTxt = csvTxt + "" +",";//l//ds.Tables[0].Rows[i]["Extra Ref"].ToString()
        //            csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Reference"].ToString() + "\"" + ",";//m    "\"" + FileNameWithoutExtension + "-" + ds.Tables[0].Rows[i]["User"].ToString()+ "\""
        //            csvTxt = csvTxt + "" +",";//n//ds.Tables[0].Rows[i]["Project Ref"].ToString()
        //            csvTxt = csvTxt +"2"+ ",";//o// ds.Tables[0].Rows[i]["Cost code Ref"].ToString() 

        //            if (ds.Tables[0].Rows[i]["Type"].ToString() == "PI")
        //            {
        //                csvTxt = csvTxt + "4" + ",";//p
        //            }
        //            else if (ds.Tables[0].Rows[i]["Type"].ToString() == "PC")
        //            {
        //                csvTxt = csvTxt + "5" + ",";//p
        //            }
        //            else
        //            {
        //                csvTxt = csvTxt + ds.Tables[0].Rows[i]["Type"].ToString() + ",";//p
        //            }

        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Date"].ToString() + ",";//q
        //            csvTxt = csvTxt + "" + ",";//r
        //            csvTxt = csvTxt + "" + ",";//s
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Tax Amount"].ToString() + ",";//t
        //            csvTxt = csvTxt + "" + ",";//u
        //            csvTxt = csvTxt + "" + ",";//v
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Net Amount"].ToString() + ",";//w
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Net Amount"].ToString() + ",";//x
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Nominal a/c ref"].ToString() + ",";//y
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Dept Code"].ToString() + ",";//z
        //            csvTxt = csvTxt + "" + ",";//aa
        //            csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Details"].ToString() + "\"" + ",";//ab
        //            csvTxt = csvTxt + "" + ",";//ac
        //            csvTxt = csvTxt + "" + ",";//ad
        //            csvTxt = csvTxt + "" + ",";//ae
        //            csvTxt = csvTxt + "" + ",";//af
        //            csvTxt = csvTxt + "" + ",";//ag
        //            csvTxt = csvTxt + "" + ",";//ah

        //            if (ds.Tables[0].Rows[i]["Tax Code"].ToString() == "T0")
        //            {
        //                csvTxt = csvTxt + "2" + ",";//ai
        //            }
        //            else if (ds.Tables[0].Rows[i]["Tax Code"].ToString() == "T1")
        //            {
        //                csvTxt = csvTxt + "1" + ",";//ai
        //            }
        //            else
        //            {
        //                csvTxt = csvTxt + ds.Tables[0].Rows[i]["Tax Code"].ToString() + ",";//ai
        //            }



        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Net Amount"].ToString() + ",";//aj
        //            csvTxt = csvTxt + "" + ",";//ak
        //            csvTxt = csvTxt + "" + ",";//al
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Tax Amount"].ToString() + ",";//am
        //            csvTxt = csvTxt + "" + ",";//an
        //            csvTxt = csvTxt + "" + ",";//ao
        //            csvTxt = csvTxt + "" + ",";//ap
        //            csvTxt = csvTxt + "" + ",";//aq
        //            csvTxt = csvTxt + "" + ",";//ar
        //            csvTxt = csvTxt + "" + ",";//as
        //            csvTxt = csvTxt + "" + ",";//at
        //            csvTxt = csvTxt + "" + ",";//au
        //            csvTxt = csvTxt + "" + ",";//av
        //            csvTxt = csvTxt + "" + ",";//aw
        //            csvTxt = csvTxt + "" + ",";//ax
        //            csvTxt = csvTxt + "" + "";//ay


        //            StreamWriter SW;
        //            SW = new StreamWriter(fs1);
        //            SW.WriteLine(csvTxt);
        //            SW.Flush();
        //            csvTxt = "";
        //        }
        //    }
        //    //addition ends  by kuntalkarar on 25thJanuary2017
        //}*/
        ////blocked by kuntalkarar on 25thJanuary2017
        //#endregion
     
        ////SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_GRH", sqlConn);
        //SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_Sage_JKS", sqlConn);
        
        //sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        //sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(ddlBuyerCompany.SelectedValue));
        //sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
        //DataSet ds = new DataSet();
        //try
        //{
        //    sqlConn.Open(); 
        //    sqlDA.Fill(ds);
        //    //csvTxt = "Type,Supplier a/c ref,Nominal a/c ref,Dept Code,Date,Reference,Details,Net Amount,Tax Code,Tax Amount,H:DocumentID,H:Document Type,H:Supplier Code,H:Invoice No.,H:Invoice Date,H:Net Value,H:VAT,H:Total Value,H:URL,L:Purchase Order No.,L:Product Code,L:Quantity,L:Price,L:Amount,L:VAT\n";
        //    csvTxt = "H:DocumentID,H:Document Type,H:Supplier Code,H:Invoice No.,H:Invoice Date,H:Net Value,H:VAT,H:Total Value,H:URL,L:Purchase Order No.,L:Product Code,L:Quantity,L:Price,L:Amount,L:VAT";

        //    StreamWriter SW1;
        //    SW1 = new StreamWriter(fs1);
        //    SW1.WriteLine(csvTxt);
        //    SW1.Flush();

        //    DataTable DT = ds.Tables[0];

        //    if (DT.Rows.Count > 0)
        //    {
        //        int c = DT.Rows.Count;

        //        for (int i = 0; i < c; i++)
        //        {
        //            #region Unused Code
        //            /*csvTxt = csvTxt + ds.Tables[0].Rows[i]["Type"].ToString() + ",";
        //            csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Supplier a/c ref"].ToString() + "\"" + ",";
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Nominal a/c ref"].ToString() + ",";
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Dept Code"].ToString() + ",";
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Date"].ToString() + ",";
        //            csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Reference"].ToString() + "\"" + ",";  // "\"" added so the file does not get corrupted if there is a comma in the text.
        //            csvTxt = csvTxt + "\"" + ds.Tables[0].Rows[i]["Details"].ToString() + "\"" + ",";
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Net Amount"].ToString() + ",";
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Tax Code"].ToString() + ",";
        //            csvTxt = csvTxt + ds.Tables[0].Rows[i]["Tax Amount"].ToString() + ",";*/
        //            #endregion

        //            DataRow DR = DT.Rows[i];

        //            csvTxt = DR["DocumentID"].ToString() + ",";//H:DocumentID
        //            csvTxt += DR["Type"].ToString() + ",";//H:Document Type
        //            csvTxt += DR["Supplier a/c ref"].ToString().Replace(",", ";") + ",";//H:Supplier Code
        //            csvTxt += DR["Reference"].ToString().Replace(",", ";") + ",";//H:Invoice No.
        //            csvTxt += DR["Date"].ToString() + ",";//H:Invoice Date
        //            csvTxt += DR["Net Value"].ToString() + ",";//H:Net Value
        //            csvTxt += DR["VAT"].ToString() + ",";//H:VAT
        //            csvTxt += DR["Total Value"].ToString() + ",";//H:Total Value
        //            csvTxt += DR["URL"].ToString() + ",";//H:URL
        //            csvTxt += DR["Purchase Order No."].ToString().Replace(",", ";") + ",";//L:Purchase Order No.
        //            csvTxt += DR["Product Code"].ToString().Replace(",", ";") + ",";//L:Product Code
        //            csvTxt += DR["Quantity"].ToString() + ",";//L:Quantity
        //            csvTxt += DR["Price"].ToString() + ",";//L:Price
        //            csvTxt += DR["Amount"].ToString() + ",";//L:Amount
        //            csvTxt += DR["VAT2"].ToString() + ",";//L:VAT  

        //            #region Unused Block
        //            //csvTxt = csvTxt + ds.Tables[0].Rows[i]["Exchange Rate"].ToString() + ",";
        //            //csvTxt = csvTxt + ds.Tables[0].Rows[i]["Extra Ref"].ToString() + ",";
        //            //csvTxt = csvTxt + "\"" + FileNameWithoutExtension + "-" + ds.Tables[0].Rows[i]["User"].ToString() + "\"" + ",";
        //            //csvTxt = csvTxt + ds.Tables[0].Rows[i]["Project Ref"].ToString() + ",";
        //            //csvTxt = csvTxt + ds.Tables[0].Rows[i]["Cost code Ref"].ToString() + "";
        //            #endregion

        //            StreamWriter SW;
        //            SW = new StreamWriter(fs1);
        //            SW.WriteLine(csvTxt);
        //            SW.Flush();

        //            //#region Unused Block
        //            StreamWriter SW2;
        //            SW2 = new StreamWriter(fs3);
        //            SW2.WriteLine(csvTxt);
        //            SW2.Flush();
        //            csvTxt = "";
        //            //#endregion

        //        }
        //    }


        //}
        //catch (Exception ex)
        //{
        //    //string err = ex.Message.ToString();
        //    // SendEmail(err);
        //    //string err = ex.Message.ToString();
        //    string ss = "Message: " + ex.Message + "<br />Source: " + ex.Source + "<br />StackTrace: " + ex.StackTrace + "<br />TargetSite: " + ex.TargetSite + "<br />InnerException: " + ex.InnerException + "<br />Data: " + ex.Data;
        //    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), ss);

        //}
        //finally
        //{
        //    sqlDA.Dispose();
        //    sqlConn.Close();
        //    fs1.Close();
        //    fs3.Close();
        //}
        //Response.Redirect("../downloadDB/DownLoadFiles.aspx");

        // string sess = Session["UserID"].ToString();
        //Response.Redirect("DownLoadFiles.aspx");
        //Response.Redirect("grhSessiontest.aspx");
        // Page.RegisterStartupScript("reg", "<script>FnCompleted();</script>");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            DA.GetUpdateJKS_Matching();
            Page.RegisterStartupScript("reg", "<script> alert('Operation performed successfully.');</script>");
        }
        catch  (Exception ex)
        {
             string msg=ex.ToString();
        }
    }
    protected void btnPO_Click(object sender, EventArgs e)
    {
        ApplicationName = "JKSPOINVExportWinService";
        EventLogNew.Write(ApplicationName, "Operation has started.", EventLogEntryType.Information, 4);
        Start();
        EventLogNew.Write(ApplicationName, "Operation has ended.", EventLogEntryType.Information, 4);
        
    }
    
    public void Start()
    {
        try
        {
            int ParentCompanyID = 180918;
            ApplicationName = "JKSPOINVExportWinService";
            DataTable DT1 = DA.ReturnChildCompanies(ParentCompanyID);
            //DataTable DT2 = DA.ReturnUserIDTable(ParentCompanyID);
            // added by kd on 01/08/2018 
            foreach (DataRow DR1 in DT1.Rows)
            {
                int BuyerCompanyID = Convert.ToInt32(DR1["CompanyID"].ToString());
                DataTable DT = DA.ReturnInvoiceNo(BuyerCompanyID);
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow DR2 in DT.Rows)
                    {
                        DA.INVVatUpdateBeforeExport(Convert.ToInt32(DR2["InvoiceID"].ToString()));
                    }
                }

            }
            foreach (DataRow DR1 in DT1.Rows)
            {
                int BuyerCompanyID = Convert.ToInt32(DR1["CompanyID"].ToString());
                DataTable DT = DA.ReturnCRNNo(BuyerCompanyID);
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow DR2 in DT.Rows)
                    {
                        DA.CRNVatUpdateBeforeExport(Convert.ToInt32(DR2["CreditNoteID"].ToString()));
                    }
                }

            }
            foreach (DataRow DR1 in DT1.Rows)
            {
                string BuyerCompanyName = DR1["CompanyName"].ToString();
                int BuyerCompanyID = Convert.ToInt32(DR1["CompanyID"].ToString());

                EventLogNew.Write(ApplicationName, "BuyerCompanyName: " + BuyerCompanyName, EventLogEntryType.Information, 9);
                EventLogNew.Write(ApplicationName, "BuyerCompanyID: " + BuyerCompanyID, EventLogEntryType.Information, 9);

                //foreach (DataRow DR2 in DT2.Rows)
                //{
                string DateTimeString = Convert.ToDateTime(DateTime.Now.ToLongDateString()).ToString("ddMMyyyy") + Convert.ToDateTime(DateTime.Now.ToLongTimeString()).ToString("HHmmss");

                string UserID = "";//DR2[0].ToString();

                EventLogNew.Write(ApplicationName, "UserID: " + UserID, EventLogEntryType.Information, 9);

                DataTable DT = DA.ReturnSageDataTable(BuyerCompanyID);

                if (DT.Rows.Count > 0)
                {
                    EventLogNew.Write(ApplicationName, "DT.Rows.Count: " + DT.Rows.Count, EventLogEntryType.Information, 9);

                    #region Export Path
                    string fpath1 = ConfigurationManager.AppSettings["InvoiceExportPath_JKS"].Trim() + @"\" + "POINV_" + DateTimeString + "_0" + ".csv";

                    Stream fsA = File.Create(fpath1);
                    fsA.Close();

                    Stream fs1 = File.Open(fpath1, FileMode.Open, FileAccess.ReadWrite);

                    EventLogNew.Write(ApplicationName, "Export Path: " + fpath1, EventLogEntryType.Information, 9);
                    #endregion

                    #region Duplicate Path
                    string fpath2 = ConfigurationManager.AppSettings["InvoiceExportPathDuplicate_JKS"].Trim() + @"\" + "POINV_" + DateTimeString + "_0" + ".csv";

                    Stream fsB = File.Create(fpath2);
                    fsB.Close();

                    Stream fs2 = File.Open(fpath2, FileMode.Open, FileAccess.ReadWrite);

                    EventLogNew.Write(ApplicationName, "Duplicate Path: " + fpath2, EventLogEntryType.Information, 9);
                    #endregion

                    //string csvTxt = "H:DocumentID,H:Document Type,H:Supplier Code,H:Invoice No.,H:Invoice Date,H:Net Value,H:VAT,H:Total Value,H:URL,L:Purchase Order No.,L:Product Code,L:Quantity,L:Price,L:Amount,L:VAT,H:Currency";
                    string csvTxt = "H:DocumentID,H:Type,H:CompanyCode,H:SupplierCode,H:DocType,H:InvoiceNumber,H:InvoiceDate,H:Currency,H:NetValue,H:VAT,H:Total,H:URL,L:PONumber,L:POLineNo,L:GRN,L:Quantity,L:UOM,L:Price,L:NetValue,L:VAT,L:TaxCode,L:Total,L:Description,L:Nominal,L:CostCentre,L:InternalOrderNo,L:Profit Centre";

                    StreamWriter SW;
                    StreamWriter SW1;
                    StreamWriter SW2;

                    SW = new StreamWriter(fs1);
                    SW.WriteLine(csvTxt);
                    SW.Flush();

                    SW = new StreamWriter(fs2);
                    SW.WriteLine(csvTxt);
                    SW.Flush();
                    string value = "";
                    foreach (DataRow DR in DT.Rows)
                    {
                        csvTxt = DR["DocumentID"].ToString() + ",";//H:DocumentID
                        //added BY kd on 06-08-2018
                        csvTxt += "PO" + ",";//H:Type
                        csvTxt += DR["CompanyCode"].ToString() + ",";//H:CompanyCode
                        //Blocked By sonali 19.6.2018
                        // csvTxt += DR["Supplier a/c ref"].ToString().Replace(",", ";") + ",";//H:Supplier Code
                        //added BY soanli 19.6.2018
                        if (DR["Supplier a/c ref"].ToString().Contains(","))
                            value = '"' + DR["Supplier a/c ref"].ToString() + '"';
                        else
                            value = DR["Supplier a/c ref"].ToString();
                        csvTxt += value + ",";//H:SupplierCode
                        csvTxt += DR["Type"].ToString() + ",";//H:DocType
                        //Blocked By sonali 19.6.2018
                        // csvTxt += DR["Supplier a/c ref"].ToString().Replace(",", ";") + ",";//H:Supplier Code

                        //Blocked By sonali 19.6.2018
                        //   csvTxt += DR["Reference"].ToString().Replace(",", ";") + ",";//H:Invoice No.
                        //added BY sonali 19.6.2018

                        value = DR["Reference"].ToString();

                        value = value.Replace(",", "");

                        //string txt=value.Replace(",","");
                        // csvTxt += value.Replace(",", "") + ",";//H:Invoice No.Replace("-","/");
                        if (value.Length >= 16)
                        {
                            csvTxt += value.Substring(0, 16) + ",";
                        }
                        else
                        {
                            csvTxt += value + ",";
                        }
                        // csvTxt += Convert.ToDateTime(DR["Date"].ToString()).ToString("ddMMyyyy") + ",";//H:InvoiceDate  Convert.ToDateTime(DR["Date"].ToString()).ToString("ddMMyyyy");
                        value = DR["Date"].ToString();
                        value = value.Replace("/", "");
                        csvTxt += value + ",";
                        csvTxt += DR["Currency"].ToString() + ",";//H:Currency 
                        decimal NetValue = Convert.ToDecimal(DR["Net Value"]);
                        csvTxt += NetValue.ToString("0.00") + ",";//H:NetValue
                        csvTxt += DR["VAT"].ToString() + ",";//H:VAT
                        csvTxt += DR["Total Value"].ToString() + ",";//H:Total
                        csvTxt += DR["URL"].ToString() + ",";//H:URL
                        //Blocked By sonali 19.6.2018
                        // csvTxt += DR["Purchase Order No."].ToString().Replace(",", ";") + ",";//L:Purchase Order No.
                        //added BY sonali 19.6.2018
                        if (DR["Purchase Order No."].ToString().Contains(","))
                            value = '"' + DR["Purchase Order No."].ToString() + '"';
                        else
                            value = DR["Purchase Order No."].ToString();
                        csvTxt += value.Replace(",", "") + ",";//L:PONumber
                        //Blocked By sonali 19.6.2018
                        // csvTxt += DR["Product Code"].ToString().Replace(",", ";") + ",";//L:Product Code
                        //added BY sonali 19.6.2018//Blocked by kd on 07-08-2018
                        //if (DR["Product Code"].ToString().Contains(","))
                        //    value = '"' + DR["Product Code"].ToString() + '"';
                        //else
                        //    value = DR["Product Code"].ToString();
                        //csvTxt += value + ",";//L:Product Code                              
                        //L:POLineNo,L:GRN,L:Quantity,L:Price,L:NetValue,L:VAT,L:TaxCode,L:Total,L:Description,L:Nominal,L:CostCentre,L:InternalOrderNo,L:Profit Centre";                             
                        //added BY kd on 06-08-2018
                        csvTxt += DR["PurOrderLineNo"].ToString() + ",";//L:POLineNo
                        csvTxt += DR["GRN_No"].ToString().Replace(",", "") + ",";//L:GRN
                        csvTxt += DR["Quantity"].ToString() + ",";//L:Quantity
                        csvTxt += DR["UOM"].ToString() + ",";//L:UOM
                        csvTxt += DR["Price"].ToString() + ",";//L:Price
                        csvTxt += DR["Amount"].ToString() + ",";//L:NetValue 
                        decimal value1;
                        decimal value2;
                        decimal VATvalue;
                        if (DR["TaxRate"].ToString() != "")
                        {
                            value1 = Convert.ToDecimal(DR["TaxRate"]);
                            value2 = Convert.ToDecimal(DR["Amount"]);
                            VATvalue = value1 * value2;//L:VAT  
                            csvTxt += VATvalue.ToString("0.00") + ",";//L:VAT  
                        }
                        else
                        {
                            value = DR["VAT2"].ToString();
                            csvTxt += value + ",";//L:VAT  
                        }

                        //  modified by kd on 26-10-2018
                        if (DR["vatcodeFromGRD"].ToString() != "")
                        {
                            csvTxt += DR["vatcodeFromGRD"].ToString() + ",";//L:TaxCode 
                        }
                        else
                        {
                            csvTxt += DR["vatcode"].ToString() + ",";//L:TaxCode
                        }
                        // modified by kd on 22-09-2018
                        if (DR["TaxRate"].ToString() != "")
                        {
                            value1 = Convert.ToDecimal(DR["TaxRate"]);
                            value2 = Convert.ToDecimal(DR["Amount"]);
                            VATvalue = value1 * value2;//L:VAT
                            decimal value3 = Convert.ToDecimal(DR["Amount"]);
                            decimal value4 = VATvalue;
                            decimal VATvalue1;
                            VATvalue1 = value3 + value4;
                            csvTxt += VATvalue1.ToString("0.00") + ",";//L:Total =L:VAT + L:NetValue
                        }
                        else
                        {
                            decimal value3 = Convert.ToDecimal(DR["Amount"]);
                            decimal value4 = Convert.ToDecimal(DR["VAT2"]);
                            decimal VATvalue2;
                            VATvalue2 = value3 + value4;
                            csvTxt += VATvalue2.ToString("0.00") + ",";//L:Total =L:VAT + L:NetValue
                        }      
                        csvTxt += " " + ",";//L:Description
                        csvTxt += " " + ",";//L:Nominal
                        csvTxt += " " + ",";//L:CostCentre
                        csvTxt += " " + ",";//L:InternalOrderNo
                        csvTxt += " " + ",";//L:ProfitCentre
                        SW1 = new StreamWriter(fs1);
                        SW1.WriteLine(csvTxt);
                        SW1.Flush();

                        SW2 = new StreamWriter(fs2);
                        SW2.WriteLine(csvTxt);
                        SW2.Flush();

                        csvTxt = "";
                    }

                    fs1.Close();
                    fs2.Close();
                }
                //}
            }
            Page.RegisterStartupScript("reg", "<script> alert('Operation performed successfully.');</script>");
        }
        catch (Exception ex)
        {
            string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

            EventLogNew.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 5);
        }
           
        finally { }
    }


    protected void btnNONPO_Click(object sender, EventArgs e)
    {
        try
        {
            ApplicationName = "JKSNONPOINVExportWinService";
            EventLogNew.Write(ApplicationName, "Operation has started.", EventLogEntryType.Information, 4);
            DataTable DT1 = DA.ReturnChildCompanies(ParentCompanyID);
            //DataTable DT2 = DA.ReturnUserIDTable(ParentCompanyID);            
            foreach (DataRow DR1 in DT1.Rows)
            {
                string BuyerCompanyName = DR1["CompanyName"].ToString();
                int BuyerCompanyID = Convert.ToInt32(DR1["CompanyID"].ToString());

                EventLogNew.Write(ApplicationName, "BuyerCompanyName: " + BuyerCompanyName, EventLogEntryType.Information, 9);
                EventLogNew.Write(ApplicationName, "BuyerCompanyID: " + BuyerCompanyID, EventLogEntryType.Information, 9);

                //foreach (DataRow DR2 in DT2.Rows)
                //{
                string DateTimeString = Convert.ToDateTime(DateTime.Now.ToLongDateString()).ToString("ddMMyyyy") + Convert.ToDateTime(DateTime.Now.ToLongTimeString()).ToString("HHmmss");

                string UserID = "";//DR2[0].ToString();

                EventLogNew.Write(ApplicationName, "UserID: " + UserID, EventLogEntryType.Information, 9);

                DataTable DT = DA.ReturnSageDataTableNONPO(BuyerCompanyID);

                if (DT.Rows.Count > 0)
                {
                    EventLogNew.Write(ApplicationName, "DT.Rows.Count: " + DT.Rows.Count, EventLogEntryType.Information, 9);

                    #region Export Path
                    string fpath1 = ConfigurationManager.AppSettings["InvoiceExportPath_JKS"].Trim() + @"\" + "NONPOINV_" + DateTimeString + "_0" + ".csv";

                    Stream fsA = File.Create(fpath1);
                    fsA.Close();

                    Stream fs1 = File.Open(fpath1, FileMode.Open, FileAccess.ReadWrite);

                    EventLogNew.Write(ApplicationName, "Export Path: " + fpath1, EventLogEntryType.Information, 9);
                    #endregion

                    #region Duplicate Path
                    string fpath2 = ConfigurationManager.AppSettings["InvoiceExportPathDuplicate_JKS"].Trim() + @"\" + "NONPOINV_" + DateTimeString + "_0" + ".csv";

                    Stream fsB = File.Create(fpath2);
                    fsB.Close();

                    Stream fs2 = File.Open(fpath2, FileMode.Open, FileAccess.ReadWrite);

                    EventLogNew.Write(ApplicationName, "Duplicate Path: " + fpath2, EventLogEntryType.Information, 9);
                    #endregion

                    //string csvTxt = "H:DocumentID,H:Document Type,H:Supplier Code,H:Invoice No.,H:Invoice Date,H:Net Value,H:VAT,H:Total Value,H:URL,L:Purchase Order No.,L:Product Code,L:Quantity,L:Price,L:Amount,L:VAT,H:Currency";
                    string csvTxt = "H:DocumentID,H:Type,H:CompanyCode,H:SupplierCode,H:DocType,H:InvoiceNumber,H:InvoiceDate,H:Currency,H:NetValue,H:VAT,H:Total,H:URL,L:PONumber,L:POLineNo,L:GRN,L:Quantity,L:UOM,L:Price,L:NetValue,L:VAT,L:TaxCode,L:Total,L:Description,L:Nominal,L:CostCentre,L:InternalOrderNo,L:Profit Centre";

                    StreamWriter SW;
                    StreamWriter SW1;
                    StreamWriter SW2;

                    SW = new StreamWriter(fs1);
                    SW.WriteLine(csvTxt);
                    SW.Flush();

                    SW = new StreamWriter(fs2);
                    SW.WriteLine(csvTxt);
                    SW.Flush();
                    string value = "";
                    foreach (DataRow DR in DT.Rows)
                    {
                        csvTxt = DR["DocumentID"].ToString() + ",";//H:DocumentID
                        //added BY kd on 27-08-2018
                        csvTxt += "NONPO" + ",";//H:Type
                        csvTxt += DR["CompanyCode"].ToString() + ",";//H:CompanyCode                               
                        if (DR["Supplier a/c ref"].ToString().Contains(","))
                            value = '"' + DR["Supplier a/c ref"].ToString() + '"';
                        else
                            value = DR["Supplier a/c ref"].ToString();
                        csvTxt += value + ",";//H:SupplierCode
                        csvTxt += DR["Type"].ToString() + ",";//H:DocType

                        value = DR["Reference"].ToString();//H:Invoice No.
                        value = value.Replace(",", "");
                        if (value.Length >= 16)
                        {
                            csvTxt += value.Substring(0, 16) + ",";
                        }
                        else
                        {
                            csvTxt += value + ",";
                        }
                        //csvTxt += Convert.ToDateTime(DR["Date"].ToString()).ToString("ddMMyyyy") + ",";//H:InvoiceDate
                        //csvTxt += DR["Date"].ToString() + ",";
                        value = DR["Date"].ToString();
                        value = value.Replace("/", "");
                        csvTxt += value + ",";
                        csvTxt += DR["Currency"].ToString() + ",";//H:Currency 
                        csvTxt += DR["Net Value"].ToString() + ",";//H:NetValue
                        csvTxt += DR["VAT"].ToString() + ",";//H:VAT
                        csvTxt += DR["Total Value"].ToString() + ",";//H:Total
                        csvTxt += DR["URL"].ToString() + ",";//H:URL
                        //--------------------------------------------------------------------- End of Header values
                        csvTxt += "" + ",";//L:PONumber

                        //added BY kd on 27-08-2018
                        csvTxt += "" + ",";//L:POLineNo
                        csvTxt += "" + ",";//L:GRN
                        csvTxt += "1" + ",";//L:Quantity
                        csvTxt += "EA" + ",";//L:UOM
                        csvTxt += DR["Price"].ToString() + ",";//L:Price
                        csvTxt += DR["NetValue"].ToString() + ",";//L:NetValue                               
                        csvTxt += DR["GVAT"].ToString() + ",";//L:VAT  
                        csvTxt += DR["vatcode"].ToString() + ",";//L:TaxCode
                        decimal value3 = Convert.ToDecimal(DR["NetValue"]);
                        decimal value4 = Convert.ToDecimal(DR["GVAT"]);
                        csvTxt += Convert.ToString(value3 + value4) + ",";//L:Total
                        //added by kd on 14-09-2018
                        value = DR["Description"].ToString();//L:Description  
                        value = value.Replace(",", "");

                        if (value.Length >= 50)
                        {
                            value = value.Substring(0, 50);
                        }

                        if (DR["Description"].ToString() != "")
                        {
                            csvTxt += value + ",";//L:Description                                    
                        }
                        else
                        {
                            csvTxt += DR["NominalName"].ToString() + ",";//L:Description
                        }
                        csvTxt += DR["NominalCode"].ToString() + ",";//L:NominalCode 
                        if (DR["BalanceSheet"].ToString() == "1")
                        {
                            csvTxt += " " + ",";//L:CostCentre CostCentre
                        }
                        else
                        {
                            csvTxt += DR["CostCentre"].ToString() + ",";//L:CostCentre 
                        }

                        csvTxt += DR["BusinessUnitCode"].ToString() + ",";//L:InternalOrderNo
                        //Modified by kd on 14-09-2018
                        if (DR["BalanceSheet"].ToString() == "0" || DR["BalanceSheet"].ToString() == "")
                        {
                            csvTxt += " " + ",";//L:ProfitCentre
                        }
                        else
                        {
                            csvTxt += DR["CostCentre"].ToString() + ",";//L:ProfitCentre
                        }
                        SW1 = new StreamWriter(fs1);
                        SW1.WriteLine(csvTxt);
                        SW1.Flush();

                        SW2 = new StreamWriter(fs2);
                        SW2.WriteLine(csvTxt);
                        SW2.Flush();

                        csvTxt = "";
                    }

                    fs1.Close();
                    fs2.Close();
                }
                //}
            }
            Page.RegisterStartupScript("reg", "<script> alert('Operation performed successfully.');</script>");
        }
        catch (Exception ex)
        {
            string ss = ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.InnerException + "\r\n" + ex.Source;

            EventLogNew.Write(ApplicationName, "Error: " + ss, EventLogEntryType.Error, 5);
        }
        finally {
            EventLogNew.Write(ApplicationName, "Operation has ended.", EventLogEntryType.Information, 4);
        }
    }
}