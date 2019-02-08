using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using CBSolutions.Architecture.Core;
using System.Globalization;
namespace JKS
{
    public partial class JKS_downloadDB_d_options : System.Web.UI.Page
    {
        private downloadDB objdownloadDB = new downloadDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetBuyerCompanyListDropDown();
            }
        }

        //public void GenerateAccrualsReports()
        //{

        //   // string strFileName = "AccrualsReports_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".csv";//InvoiceCreditNotes
        //    string strFileName = "InvoiceDownload" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".csv";
        //    string fpath = Server.MapPath("~") + "\\Temp\\" + strFileName;
        //  string strFromDate = DateTime.ParseExact(txtFromDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        //  string strToDate = DateTime.ParseExact(txtToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

        //    SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
        //    SqlDataAdapter dap = new SqlDataAdapter("downloadInvoice_GRH_kk", sqlConn);
        //    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
        //    dap.SelectCommand.Parameters.Add("@CompanyID",Convert.ToInt32(ddlCompany.SelectedValue));// Convert.ToInt32("180918"));//124529 for AnchorSafety changed to 180918 for JKS
        //    dap.SelectCommand.Parameters.Add("@FromDate", strFromDate);
        //    dap.SelectCommand.Parameters.Add("@ToDate", strToDate);
        //    if (chkCurrentOnly.Checked == true)
        //    {
        //        dap.SelectCommand.Parameters.Add("@Currentonly", "Yes");
        //    }
        //    else
        //    {
        //        dap.SelectCommand.Parameters.Add("@Currentonly", "No");
        //    }
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
        //                csvText = "H:Company,H:Supplier_Name,H:Vendor_ID,H:Doc_Type,H:Invoice_No,H:Invoice_Date,H:Currency,H:Invoice_Status,H:Net,H:VAT,H:Total,H:Attachments,L:Line_No,L:Company_Name,L:Business_Unit,L:Department,L:Nominal_Code,L:Nominal_Name,L:DESCRIPTION,L:PO_No,L:Net,L:VAT,DocumentID,Approval_Path,Passed_To,Approval_Node,Date_Exported,RejectionCode \n";


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
        //                    csvText = csvText + ds.Tables[0].Rows[i]["LineVAT"].ToString() + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["DocumentID"].ToString() + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["Approval_Path"].ToString() + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["Passed_To"].ToString() + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["Approval_Node"].ToString() + ","; 
        //                    csvText = csvText + ds.Tables[0].Rows[i]["Date_Exported"].ToString() + ",";
        //                    csvText = csvText + ds.Tables[0].Rows[i]["RejectionCode"].ToString() + " ";
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
        //            csvText = "H:Company,H:Supplier_Name,H:Vendor_ID,H:Doc_Type,H:Invoice_No,H:Invoice_Date,H:Currency,H:Invoice_Status,H:Net,H:VAT,H:Total,H:Attachments,L:Line_No,L:Company_Name,L:Business_Unit,L:Department,L:Nominal_Code,L:Nominal_Name,L:DESCRIPTION,L:PO_No,L:Net,L:VAT,DocumentID,Approval_Path,Passed_To,Approval_Node,Date_Exported,RejectionCode \n";
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

        //    DownloadReports(fpath, strFileName);


        //}
        public void GenerateAccrualsReports()
        {

            // string strFileName = "AccrualsReports_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".csv";//InvoiceCreditNotes
            string strFileName = "InvoiceDownload" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".csv";
            string fpath = Server.MapPath("~") + "\\Temp\\" + strFileName;


            //string fpath = Server.MapPath("~") + "\\Temp\\" + strFileName;

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //SqlDataAdapter dap = new SqlDataAdapter("downloadInvoice_GRH_latest", sqlConn);
            SqlDataAdapter dap = new SqlDataAdapter("downloadInvoice_JKS", sqlConn);


            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));// Convert.ToInt32("180918"));//124529 for AnchorSafety changed to 180918 for JKS
            dap.SelectCommand.Parameters.Add("@FromDate", txtFromDate.Value);
            dap.SelectCommand.Parameters.Add("@ToDate", txtToDate.Value);
            if (chkCurrentOnly.Checked == true)
            {
                dap.SelectCommand.Parameters.Add("@Currentonly", "Yes");
            }
            else
            {
                dap.SelectCommand.Parameters.Add("@Currentonly", "No");
            }
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
                        csvText = "H:Company,H:Supplier_Name,H:Vendor_ID,H:Doc_Type,H:Invoice_No,H:Invoice_Date,H:Currency,H:Invoice_Status,H:Net,H:VAT,H:Total,H:Attachments,L:Line_No,L:Company_Name,L:Business_Unit,L:Department,L:Nominal_Code,L:Nominal_Name,L:DESCRIPTION,L:PO_No,L:Net,L:VAT,DocumentID,Approval_Path,Passed_To,Approval_Node,Date_Exported,RejectionCode,H:ScanDate \n";


                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                             #region old code on 16thFebruary2017
                        /* csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Company"].ToString() + "\"" + ",";
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
                            csvText = csvText + ds.Tables[0].Rows[i]["LineVAT"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["DocumentID"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Approval_Path"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Passed_To"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Approval_Node"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Date_Exported"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["RejectionCode"].ToString() + " ";*/
                             #endregion
                            //new code added on 16thFebruary2017
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Company"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Supplier_Name"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Vendor_ID"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Doc_Type"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + ds.Tables[0].Rows[i]["Invoice_No"].ToString() + "\"" + ",";
                            csvText = csvText + "\"" + Convert.ToDateTime(ds.Tables[0].Rows[i]["Invoice_Date"].ToString()).ToString("dd-MMM-yyyy") + "\"" + ",";
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
                            csvText = csvText + ds.Tables[0].Rows[i]["LineVAT"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["DocumentID"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Approval_Path"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Passed_To"].ToString() + ",";
                            csvText = csvText + ds.Tables[0].Rows[i]["Approval_Node"].ToString() + ",";
                            if (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Date_Exported"].ToString()))
                            {
                                csvText = csvText + " " + ",";
                            }
                            else
                            {
                                csvText = csvText + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date_Exported"].ToString()).ToString("dd-MMM-yyyy") + ",";

                            }
                            csvText = csvText + ds.Tables[0].Rows[i]["RejectionCode"].ToString() + ",";
                            if (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["IPEDate"].ToString()))
                            {
                                csvText = csvText + " " + "";
                            }
                            else
                            {
                                csvText = csvText + Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[i]["IPEDate"]).ToString()).ToString("dd-MMM-yyyy") + " ";

                            }
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
                    csvText = "H:Company,H:Supplier_Name,H:Vendor_ID,H:Doc_Type,H:Invoice_No,H:Invoice_Date,H:Currency,H:Invoice_Status,H:Net,H:VAT,H:Total,H:Attachments,L:Line_No,L:Company_Name,L:Business_Unit,L:Department,L:Nominal_Code,L:Nominal_Name,L:DESCRIPTION,L:PO_No,L:Net,L:VAT,DocumentID,Approval_Path,Passed_To,Approval_Node,Date_Exported,RejectionCode,H:ScanDate \n";
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



        private void GetBuyerCompanyListDropDown()
        {
            DataTable dtbl = null;
            dtbl = objdownloadDB.GetBuyerCompanyListDropDown(Convert.ToInt32(Session["CompanyID"]));

            if (dtbl != null)
            {
                ddlCompany.DataSource = dtbl;
                ddlCompany.DataBind();
                ddlCompany.Items.Insert(0, new ListItem("---Select Company---", "0"));
            }
        }
        protected void btnInvoiceCredit_Click(object sender, EventArgs e)
        {


        }
        private string ValidationMessage()
        {


            string Validationmsg = "";


            if (ddlCompany.SelectedItem.Text == "---Select Company---")
            {

                Response.Write("<script language=javascript>alert('Please Select Buyer Company')</script>");
            }
            else if (txtFromDate.Value == "")
            {

                Response.Write("<script language=javascript>alert('Please Select Date Range')</script>");
            }
            else if (txtToDate.Value == "")
            {

                Response.Write("<script language=javascript>alert('Please Select Date Range')</script>");
            }

            return Validationmsg;
        }

        protected void btnDownloadAttachment_Click(object sender, EventArgs e)
        {
            string msg = ValidationMessage();
            try
            {



                if (ddlCompany.SelectedItem.Text != "---Select Company---" && Request.Form[txtFromDate.UniqueID] != "" && Request.Form[txtToDate.UniqueID] != "")
                {

                    GenerateAccrualsReports();

                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}