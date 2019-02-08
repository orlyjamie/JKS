using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;
using System.Diagnostics;

namespace JKS
{
    /// <summary>
    /// Summary description for d_main.
    /// </summary>
    public partial class d_main : CBSolutions.ETH.Web.ETC.VSPage
    {
        //protected System.Web.UI.WebControls.HyperLink hypDownloadInvoice;
        //protected System.Web.UI.WebControls.HyperLink hypDownloadDebitNote;
        //protected System.Web.UI.WebControls.HyperLink hypDownloadCreitNote;
        //protected System.Web.UI.WebControls.Panel Panel3;

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);
            // btnAccruals.Click += new EventHandler(btnAccruals_Click);
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
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        #region: Button Events

        protected void btnAccruals_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            //Stored Procedure FetchAccrualsReports
            GenerateAccrualsReports();
        }

        #endregion
        #region:  GenerateReport
        //public void GenerateAccrualsReports()
        //{
        //   // string fpath = ConfigurationManager.AppSettings["ExportFilePathDestination"].Trim();
        //    string strFileName = "AccrualsReports_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".csv";
        //    string fpath = Server.MapPath("~") + "\\Temp\\" + strFileName;

        //    SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
        //    SqlDataAdapter dap = new SqlDataAdapter("FetchAccrualsReports_GRH", sqlConn);
        //    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
        //    dap.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32("180918"));//124529 for AnchorSafety changed to 180918 for JKS
        //    dap.SelectCommand.CommandTimeout = 0;

        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        sqlConn.Open();
        //        dap.Fill(ds);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {

        //            Stream fs = File.Create(fpath);
        //            fs.Close();
        //            Stream fs1 = File.Open(fpath, FileMode.Open, FileAccess.ReadWrite);
        //            string csvText = "";
        //            try
        //            {
        //                csvText = "H:Company,H:Supplier_Name,H:Vendor_ID,H:Doc_Type,H:Invoice_No,H:Invoice_Date,H:Currency,H:Invoice_Status,H:Net,H:VAT,H:Total,H:Attachments,L:Line_No,L:Company_Name,L:Business_Unit,L:Department,L:Nominal_Code,L:Nominal_Name,L:DESCRIPTION,L:PO_No,L:Net,L:VAT \n";


        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Company"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Supplier_Name"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Vendor_ID"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Doc_Type"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Invoice_No"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Invoice_Date"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Currency"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Invoice_Status"].ToString() + "\"" + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["Net"].ToString() + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["VAT"].ToString() + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["Total"].ToString() + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Attachments"].ToString() + "\"" + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["Line_No"].ToString() + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Company_Name"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Business_Unit"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Department"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Nominal_Code"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Nominal_Name"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["DESCRIPTION"].ToString() + "\"" + ",";
        //                    csvText = csvText + "\"" + ds.Tables[0].Rows[i]["PO_No"].ToString() + "\"" + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["LineNet"].ToString() + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["LineVAT"].ToString() + " ";
        //                    StreamWriter SW;
        //                    SW = new StreamWriter(fs1);
        //                    SW.WriteLine(csvText);
        //                    SW.Flush();
        //                    csvText = "";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                string ss = ex.Message.ToString();

        //            }
        //            finally
        //            {
        //                fs1.Close();

        //            }

        //        }
        //        else
        //        {
        //            Stream fs = File.Create(fpath);
        //            fs.Close();
        //            Stream fs1 = File.Open(fpath, FileMode.Open, FileAccess.ReadWrite);
        //            string csvText = "";
        //            csvText = "H:Company,H:Supplier_Name,H:Vendor_ID,H:Doc_Type,H:Invoice_No,H:Invoice_Date,H:Currency,H:Invoice_Status,H:Net,H:VAT,H:Total,H:Attachments,L:Line_No,L:Company_Name,L:Business_Unit,L:Department,L:Nominal_Code,L:Nominal_Name,L:DESCRIPTION,L:PO_No,L:Net,L:VAT \n";
        //            StreamWriter SW;
        //            SW = new StreamWriter(fs1);
        //            SW.WriteLine(csvText);
        //            SW.Flush();
        //            fs1.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string ss = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        ds = null;
        //        sqlConn.Close();
        //    }

        //    DownloadReports(fpath,strFileName);
        //}
        public void GenerateAccrualsReports()
        {
            // string fpath = ConfigurationManager.AppSettings["ExportFilePathDestination"].Trim();
            string strFileName = "AccrualsReports_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".csv";
            string fpath = Server.MapPath("~") + "\\Temp\\" + strFileName;

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //SqlDataAdapter dap = new SqlDataAdapter("FetchAccrualsReports_GRH_latest", sqlConn);
            SqlDataAdapter dap = new SqlDataAdapter("FetchAccrualsReports_JKS", sqlConn);
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32("180918"));//124529 for AnchorSafety changed to 180918 for JKS
            dap.SelectCommand.CommandTimeout = 0;

            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                dap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    Stream fs = File.Create(fpath);
                    fs.Close();
                    Stream fs1 = File.Open(fpath, FileMode.Open, FileAccess.ReadWrite);
                    string csvText = "";
                    try
                    {
                        csvText = "H:Company,H:Supplier_Name,H:Vendor_ID,H:Doc_Type,H:Invoice_No,H:Invoice_Date,H:Currency,H:Invoice_Status,H:Net,H:VAT,H:Total,H:Attachments,L:Line_No,L:Company_Name,L:Business_Unit,L:Department,L:Nominal_Code,L:Nominal_Name,L:DESCRIPTION,L:PO_No,L:Net,L:VAT \n";


                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Company"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Supplier_Name"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Vendor_ID"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Doc_Type"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Invoice_No"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Invoice_Date"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Currency"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Invoice_Status"].ToString() + "\"" + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Net"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["VAT"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Total"].ToString() + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Attachments"].ToString() + "\"" + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Line_No"].ToString() + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Company_Name"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Business_Unit"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Department"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Nominal_Code"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Nominal_Name"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["DESCRIPTION"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["PO_No"].ToString() + "\"" + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["LineNet"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["LineVAT"].ToString() + " ";
                            StreamWriter SW;
                            SW = new StreamWriter(fs1);
                            SW.WriteLine(csvText);
                            SW.Flush();
                            csvText = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        string ss = ex.Message.ToString();

                    }
                    finally
                    {
                        fs1.Close();

                    }

                }
                else
                {
                    Stream fs = File.Create(fpath);
                    fs.Close();
                    Stream fs1 = File.Open(fpath, FileMode.Open, FileAccess.ReadWrite);
                    string csvText = "";
                    csvText = "H:Company,H:Supplier_Name,H:Vendor_ID,H:Doc_Type,H:Invoice_No,H:Invoice_Date,H:Currency,H:Invoice_Status,H:Net,H:VAT,H:Total,H:Attachments,L:Line_No,L:Company_Name,L:Business_Unit,L:Department,L:Nominal_Code,L:Nominal_Name,L:DESCRIPTION,L:PO_No,L:Net,L:VAT \n";
                    StreamWriter SW;
                    SW = new StreamWriter(fs1);
                    SW.WriteLine(csvText);
                    SW.Flush();
                    fs1.Close();
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                ds = null;
                sqlConn.Close();
            }

            DownloadReports(fpath, strFileName);
        }
        public void DownloadReports(string Path, string FileName)
        {
            string filepath = Path;
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/csv";
                //Context.Response.AddHeader("content-disposition","attachment; filename="+Path.GetFileName(Path));
                Context.Response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                long FileSize = fs.Length;
                byte[] getContent = new byte[(int)FileSize];
                fs.Read(getContent, 0, (int)fs.Length);
                fs.Close();
                Context.Response.BinaryWrite(getContent);
                Context.Response.Flush();
                Context.Response.End();

            }
            catch (Exception ex) { throw (ex); }

        }
        #endregion
        protected void btnduplicate_Click(object sender, EventArgs e)
        {
            string strddmmyy = "";
            strddmmyy = DateTime.Today.Date.AddDays(0).ToString("dd/MM/yy").Replace("/", "");
            ExportDuplicatesFileFor_JKS(strddmmyy, "", 0, "");
            Response.Redirect("DownLoadFiles.aspx");
        }
        #region ExportDuplicatesFileFor_JKS
        public bool ExportDuplicatesFileFor_JKS(string flnm, string strBuyerName, int BuyerID, string strBuyerPrefix)
        {
            // EventLog.WriteEntry("DUPLICATES JKS");
            int iStockRegCounter = 0;
            int iInvoiceID = 0;
            bool retval = true;
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            SqlCommand sqlCmd = null;
            SqlDataReader sqlDR = null;
            try
            {

                string csvText = "";
                //sqlCmd = new SqlCommand("usp_GetDuplicateInvoiceData_GRH", sqlConn);
                sqlCmd = new SqlCommand("usp_GetDuplicateInvoiceData_JKS", sqlConn);

                sqlCmd.Parameters.Add("@ChildBuyerID", BuyerID);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.Open();

                sqlDR = sqlCmd.ExecuteReader();
                csvText = "Company,Vendor,Vendor ID" + "," + "Document Type" + "," + "Document No" + "," + "DocumentID" + "," + "DocStatus,Net, VAT, Gross, Invoice Date \n";

                while (sqlDR.Read())
                {
                    for (int i = 0; i <= sqlDR.FieldCount - 1; i++)
                    {
                        csvText = csvText + sqlDR.GetValue(i).ToString().Replace("\n", " ").Replace("\r", " ") + ",";
                    }
                    csvText = csvText.Substring(0, csvText.Length - 1) + "\n";
                    iStockRegCounter++;
                }

                sqlCmd.Dispose();
                sqlDR.Close();
                if (iStockRegCounter > 0)
                {
                    //string filepath = ConfigurationManager.AppSettings["GRHOUT"].Trim() + @"\" + "Duplicates" + flnm + ".csv";
                    //string strfolderpath = ConfigurationManager.AppSettings["GRHOUT"].Trim();

                    string filepath = ConfigurationManager.AppSettings["InvoiceExportPath_JKS"].Trim() + @"\" + "Duplicates" + flnm + ".csv";
                    string strfolderpath = ConfigurationManager.AppSettings["InvoiceExportPath_JKS"].Trim();
                    Stream fs = File.Create(filepath, 5000);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(csvText);
                    sw.Close();
                    fs.Close();

                    fs = null;

                    DirectoryInfo dir2 = new DirectoryInfo(strfolderpath.Trim());
                    FileInfo file = new FileInfo(strfolderpath.Trim());

                    FileInfo[] f2 = null;
                    f2 = dir2.GetFiles("*.*");
                    for (int i = 0; i < f2.Length; i++)
                    {
                        string sFileName = f2[i].FullName.Trim();
                        string sShortName = System.IO.Path.GetFileName(sFileName).ToString().ToUpper();
                        if (sShortName.IndexOf("DUPLICATES") != -1 && sShortName != "DUPLICATES" + flnm + ".CSV")
                        {
                            f2[i].Delete();
                        }
                    }
                }
                sqlConn.Close();
                sqlConn.Dispose();

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("DUPLICATES JKS", ex.Message.ToString());
                retval = false;
            }
            finally { sqlDR.Close(); sqlCmd.Dispose(); sqlConn.Close(); }

            return retval;
        }
        #endregion
    }
}
