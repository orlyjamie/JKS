using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
//using C1.C1Zip;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;

namespace CBSolutions.ETH.Web.Utilities
{
    /// <summary>
    /// Summary description for import.
    /// </summary>
    public class import : System.Web.UI.Page
    {
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.HtmlControls.HtmlInputFile fileImport;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.Label lblErr;
        #endregion
        // =========================================================================================================		
        #region UserDefined Variables
        string strMsg = "";
        bool valid = true;
        int strBuyerCompanyID = 0;
        //200905 SURAJIT
        int iCount = 0;
        int totCount = 0;
        //200905 SURAJIT
        #endregion
        // =========================================================================================================
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");
        }
        #endregion
        // =========================================================================================================
        #region ImportData
        private void ImportData()
        {

            strBuyerCompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (fileImport.PostedFile.ContentLength > 0)
            {
                if (fileImport.PostedFile.FileName.Length > 0)
                {
                    //230905 SURAJIT
                    String fnm = fileImport.PostedFile.FileName.ToUpper();
                    String fnExt = fnm.Substring(fnm.Length - 4, 4);
                    if (fnExt != ".XLS" && fnExt != ".CSV")
                    //230905 SURAJIT
                    {
                        valid = false;
                        lblErr.Text = "Please select an excel or csv file.";
                    }
                }
                else
                {
                    valid = false;
                    lblErr.Text = "Records have NOT been imported. File is already exclusively open by another user. Please close the file and try again."; //230905 SURAJIT
                    return;  //230905 SURAJIT
                }

                if (valid == true)
                {
                    int iNum = 0;
                    string strFileName = UploadFile();

                    if (strFileName.Trim() != "")
                    {
                        if (Path.GetExtension(strFileName).ToUpper().Equals(".XML"))
                        {
                            iNum = SaveDataFromXMLFile(strFileName);
                        }
                        else if (Path.GetExtension(strFileName).ToUpper().Equals(".CSV")) //ch 010905 SURAJIT  //.XLS"))
                        {
                            iNum = ImportRecordFromFile(strFileName);
                        }
                    }
                    //210905 SURAJIT
                    if (iNum > 0)
                    {
                        //210905 SURAJIT
                        strMsg = " Record(s) imported : " + iNum;
                        if (totCount - iNum - 1 > 0)
                        {
                            strMsg += ".                            Record(s) NOT imported : " + Convert.ToString(totCount - iNum - 1);
                        }
                        //210905 SURAJIT
                    }
                    else
                        strMsg = "Records have NOT been imported.";
                    //210905 SURAJIT
                    lblErr.Visible = true;
                    lblErr.Text = strMsg;
                }
            }
            else
            {
                lblErr.Text = "Please select a valid file path.";
            }
        }
        #endregion
        // =========================================================================================================
        #region UploadFile
        private string UploadFile()
        {
            string sFname = "";
            if (fileImport.PostedFile.FileName.Length > 0)
            {
                sFname = Path.GetFileName(fileImport.PostedFile.FileName);
                FileInfo fi = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + sFname);
                if (fi.Exists)
                {
                    fi.Delete();
                    fi = null;
                }
                string fileto = Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]);
                fileImport.PostedFile.SaveAs(fileto + "\\" + sFname);
            }
            return sFname;
        }
        #endregion
        // =========================================================================================================
        #region SaveDataFromXMLFile
        private int SaveDataFromXMLFile(string strFileName)
        {
            int iCount = 0;

            if (Session["UserID"] != null)
            {
                FileStream myFs = new FileStream(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + strFileName, FileMode.Open, FileAccess.Read);
                DataSet ds = new DataSet();
                ds.ReadXml(myFs);
                SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                sqlConn.Open();

                string strUserID = Session["UserID"].ToString().Trim();

                try
                {
                    for (int counter = 0; counter < ds.Tables[0].Rows.Count; counter++)
                    {
                    }
                    myFs.Close();
                    ds.Dispose();
                    sqlConn.Close();
                }
                catch { myFs.Close(); ds.Dispose(); sqlConn.Close(); }
            }
            return (iCount);
        }
        #endregion
        // =========================================================================================================
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
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);

        }
        #endregion
        // =========================================================================================================
        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            ImportData();
        }
        #endregion
        // =========================================================================================================
        #region ImportRecordFromFile
        private int ImportRecordFromFile(string fname)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");

            iCount = 0;  //ch 200905

            //231105 SURAJIT
            String strSucess = "";
            String strFailure = "";
            //231105 SURAJIT

            if (Session["UserID"] != null)
            {
                string strUserID = Session["UserID"].ToString().Trim();
                SqlConnection objConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                StreamReader MyFileStream;
                //230905 SURAJIT
                try
                {
                    String fileName = RollDice().ToString().Trim() + ".CSV";
                    FileInfo fi = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + fname);
                    fi.CopyTo(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + fileName);
                    FileInfo fi1 = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + fileName);
                    if (fi1.Exists)
                    {
                        MyFileStream = File.OpenText(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + fileName);
                        //230905 SURAJIT
                        string MyLine;
                        int i = 1;
                        while (MyFileStream.Peek() != -1)
                        {
                            i++;
                            MyLine = MyFileStream.ReadLine();

                            string strDocumentType = "";
                            string strGMGCompany = "";
                            string strVendor = "";
                            string strInvoiceDate = "";
                            string strPONumber = "";
                            string strInvoiceNumber = "";
                            string strNetAmount = "";
                            string strVatAmount = "";
                            string strTotalAmount = "";
                            string strCurrency = "";
                            string strTimePaymentDue = "";
                            string strRepositoryDocumentID = "";

                            if (i > 0)
                            {
                                //MyLine = MyLine.Replace("\\","").Replace(" ","").Replace("\"","");  //291105
                                String[] MyArray = MyLine.Split(',');
                                strDocumentType = MyArray[3].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strGMGCompany = MyArray[5].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strVendor = MyArray[7].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strInvoiceDate = MyArray[9].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                string invdate1 = "";
                                string invdate2 = "";
                                String[] invdate = strInvoiceDate.Split('/');
                                if (invdate[0].Length < 2) invdate[0] = '0' + invdate[0];
                                if (invdate.Length != 3)  //011205 SURAJIT
                                {
                                    //strInvoiceDate
                                }
                                else
                                {
                                    if (invdate.Length > 1)  //301105 SURAJIT
                                    {
                                        if (invdate[1].Length < 2) invdate[1] = '0' + invdate[1];
                                        invdate1 = invdate[1];
                                    }
                                    else
                                        invdate1 = "";  //301105 SURAJIT

                                    if (invdate.Length > 2)  //301105 SURAJIT
                                    {
                                        if (invdate[2].Length < 4) invdate[2] = "20" + invdate[2];
                                        invdate2 = invdate[2];
                                    }
                                    else
                                        invdate2 = "";  //301105 SURAJIT

                                    strInvoiceDate = invdate[0] + '/' + invdate1 + '/' + invdate2;
                                }

                                strPONumber = MyArray[11].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strInvoiceNumber = MyArray[13].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strNetAmount = MyArray[15].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strVatAmount = MyArray[17].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strTotalAmount = MyArray[19].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strCurrency = MyArray[21].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");

                                strTimePaymentDue = MyArray[23].ToString().Trim().Replace("\\", "").Replace(" ", "").Replace("\"", "");
                                strRepositoryDocumentID = MyArray[24].ToString().Trim(); //.Replace("\\","").Replace(" ","").Replace("\"","");

                                strRepositoryDocumentID = strRepositoryDocumentID.ToLower().Replace("\\\\off-gmg-fin\\invoicerep\\postprocess\\", "");  //http://invoices.gmgradio.com//");
                                strRepositoryDocumentID = strRepositoryDocumentID.ToLower().Replace("\\\\off-gmg-fin\\invoicerep\\preprocess\\", "");  //http://invoices.gmgradio.com//");


                                //230905 SURAJIT
                                {
                                    //230905 SURAJIT

                                    SqlCommand objComm = new SqlCommand("stpAddNewRecordFromExcelIntoInvoice", objConn);
                                    objComm.CommandType = CommandType.StoredProcedure;

                                    objComm.Parameters.Add("@DocumentType", strDocumentType);  //030905 SURAJIT									
                                    objComm.Parameters.Add("@GMGCompany", strGMGCompany);
                                    objComm.Parameters.Add("@Vendor", strVendor);
                                    objComm.Parameters.Add("@InvoiceDate", strInvoiceDate);
                                    objComm.Parameters.Add("@PONumber", strPONumber);

                                    objComm.Parameters.Add("@InvoiceNumber", strInvoiceNumber);

                                    objComm.Parameters.Add("@NetAmount", strNetAmount);
                                    objComm.Parameters.Add("@VatAmount", strVatAmount);
                                    objComm.Parameters.Add("@TotalAmount", strTotalAmount);
                                    objComm.Parameters.Add("@Currency", strCurrency);

                                    objComm.Parameters.Add("@TimePaymentDue", strTimePaymentDue);
                                    objComm.Parameters.Add("@RepositoryDocumentID", strRepositoryDocumentID);
                                    objComm.Parameters.Add("@UserID", strUserID);


                                    SqlParameter paramReturnValue = objComm.Parameters.Add("ReturnValue", SqlDbType.Int);
                                    paramReturnValue.Direction = ParameterDirection.ReturnValue;


                                    //291105
                                    int intRecordCount = 0;
                                    String msgErr = "";
                                    if (strGMGCompany != "" && invdate.Length > 2)  //ch 011205 SURAJIT
                                    {
                                        try
                                        {
                                            //291105

                                            objConn.Open();
                                            objComm.ExecuteNonQuery();
                                        }
                                        catch (System.Exception Ex0)
                                        {
                                            msgErr = Ex0.Message;
                                            intRecordCount = 0;
                                        }  //291105

                                        intRecordCount = Convert.ToInt32(objComm.Parameters["ReturnValue"].Value);
                                    }
                                    objComm.Dispose();
                                    objConn.Close();
                                    if (intRecordCount > 0)
                                    {
                                        iCount = iCount + 1;
                                        strSucess += strInvoiceNumber + ",";   //231105 SURAJIT
                                    }
                                    //231105 SURAJIT
                                    else
                                    {
                                        strFailure += strInvoiceNumber + ",";
                                        CreateLog(fi, fileName, i, strDocumentType, strUserID, strGMGCompany, strInvoiceDate, strVendor, strInvoiceNumber);

                                    }
                                    //231105 SURAJIT
                                }//230905 SURAJIT
                            }
                            totCount = i;  //200905 SURAJIT
                        }
                        MyFileStream.Close();
                        //230905 SURAJIT
                        if (fi.Exists) fi.Delete();
                        if (fi1.Exists) fi1.Delete();
                    }
                    else
                    {
                        lblErr.Text = "File is already exclusively open by another user. Please close the file and try again.";
                    }

                }
                catch (System.Exception Ex1)
                {
                    lblErr.Text = Ex1.Message;
                }
                //230905 SURAJIT
            }
            return iCount;
        }
        #endregion
        // =========================================================================================================
        #region CreateLog
        //231105 SURAJIT
        private void CreateLog(FileInfo fLog1, String filename, int nRecNo, String strDocumentType, String strUserID, String strGMGCompany, String strInvoiceDate, String strVendor, String strInvoiceNumber)
        {
            string errmsglog = "";
            string LogText = "";
            //StreamReader MyFileStream;
            Stream fs;
            StreamWriter sw;
            int nPos = filename.ToString().IndexOf(".") - 1;
            String flnm = filename.ToString().Trim().Substring(0, nPos) + "_ImportLog.CSV";
            flnm = Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + flnm;
            fLog1 = new FileInfo(flnm);
            try   //301105 SURAJIT
            {
                if (fLog1.Exists)
                {
                    fs = File.Open(flnm, System.IO.FileMode.Append);  //291105 SURAJIT
                }
                else
                {
                    fs = File.Create(flnm);  //291105 SURAJIT
                    LogText += "\n";
                    LogText += "CSV File Name: " + filename + "\n";
                    LogText += "" + "\n\n";
                    LogText += "List of Record(s) not imported: " + "\n";
                    LogText += "=================================" + "\n\n";

                }
                LogText += "Record Number: " + Convert.ToString(nRecNo).Trim() + "\n";
                LogText += "Doc Type: " + strDocumentType + "\n";
                LogText += "Doc Date: " + strInvoiceDate + "\n";
                LogText += "Invoice No.: " + strInvoiceNumber + "\n";
                LogText += "User Name: " + GetUserName(strUserID) + "\n";
                LogText += "Company: " + GetCompName(strGMGCompany) + "\n";
                LogText += "Vendor: " + GetCompName(strVendor) + "\n";

                sw = new StreamWriter(fs);
                sw.WriteLine(LogText);
                sw.Close();
                fs.Close();
                fs = null;
            }
            catch (System.Exception exLog)
            {
                errmsglog = exLog.Message;
            }
            finally
            {
                fs = null;
            }
        }
        //231105 SURAJIT
        #endregion
        // =========================================================================================================
        #region GetUserName
        //301105 SURAJIT
        private string GetUserName(string uid)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rsU = da.ExecuteQuery("Users", "UserID= " + Convert.ToInt32(uid.ToString().Trim()));
            return Convert.ToString(rsU["UserName"]);
        }
        #endregion
        // =========================================================================================================
        #region GetCompName
        private string GetCompName(string compcode)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rsC = da.ExecuteQuery("Company", "CompanyCode= '" + compcode + "'");
            return Convert.ToString(rsC["CompanyName"]);
        }
        //301105 SURAJIT
        #endregion
        // =========================================================================================================
        #region RollDice
        //230905 SURAJIT
        public int RollDice()
        {
            int i = 0;
            Random r = new Random();
            i = r.Next(999999);//Get another random number
            return i;
        }
        #endregion
        // =========================================================================================================
        #region TradingRelation
        public bool TradingRelation(int buid, int vendid)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteSP("stp_GetTradingRelation", buid, vendid);
            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    if (rs["TradRelation"] != DBNull.Value)
                        return Convert.ToBoolean(rs["TradRelation"]);
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }
        #endregion
        // =========================================================================================================	
        #region GetCompID
        public int GetCompID(String vendCode)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("Company", "CompanyCode='" + vendCode + "'");
            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    return Convert.ToInt32(rs["CompanyID"]);
                }
                else
                    return -1;
            }
            else
                return -1;
        }
        //230905 SURAJIT
        #endregion
    }
}
