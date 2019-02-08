using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using CBSolutions.Architecture.Core;
using System.Data;
using CBSolutions.ETH.Web;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.IO;
using CBSolutions.Architecture.Data;

namespace JKS
{
    public partial class Communicorp_creditnotes_CreditnoteFileManager_NL : System.Web.UI.Page
    {
        #region webcontrols declarations
        //protected System.Web.UI.WebControls.Panel Panel3;
        //protected System.Web.UI.WebControls.PlaceHolder MenuHolder;
        //protected System.Web.UI.WebControls.DataGrid grdFile;
        //protected System.Web.UI.WebControls.Label lblMsg;
        //protected System.Web.UI.WebControls.Button btnUp;
        //protected System.Web.UI.HtmlControls.HtmlInputFile File1;
        //protected System.Web.UI.HtmlControls.HtmlInputFile File2;
        //protected System.Web.UI.HtmlControls.HtmlInputFile File3;
        //protected System.Web.UI.HtmlControls.HtmlInputFile File4;
        //protected System.Web.UI.HtmlControls.HtmlInputFile File5;
        //protected System.Web.UI.WebControls.Button btnBack;
        #endregion


        SqlCommand sqlCmd;
        SqlConnection sqlConn;
        ArrayList arr;
        //modified by kuntal karar on 23rdjuly2015
        //private String strBaseDirectory = @"C:\File Repository\FTP Archive\Communicorp UK Ltd\";
        private String strBaseDirectory = @"C:\File Repository\FTP Archive\";
        //
        //private String strBaseDirectory = @"\\90107-server3\FTP Archive\Communicorp UK Ltd\000;85882;702;010715;062500_77949\";
        private String strChildBuyer = "";
        int BuyerID = 0;
        protected string InvoiceID = "";
        protected string sDisplay = "none";
        protected string strInvoiceNo = "";

        private void Page_Load(object sender, System.EventArgs e)
        {
            GetInvoiceBuyerCompanyID(Convert.ToInt32(Request["CreditNoteID"]));
            if (Request.QueryString["CreditNoteID"] != null)
                getInvoiceNo(Request.QueryString["CreditNoteID"].ToString());
            if (Request["ReturnUrl"] != null && Request["ReturnUrl"] == "../Creditnotes/Associated_Creditnote.aspx")
            {
                sDisplay = "none";
            }


            //strBaseDirectory += strChildBuyer + @"\";

            btnUp.Attributes.Add("onclick", "return ValidateFormSubmission();");
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);


            //			Control ctrlMenu;
            if (Request["From"] == "ETC")
            {
            }
            else if (Request["From"] == "SUPPLIER")
            {
            }

            if (!IsPostBack)
            {
                lblMsg.Text = "";
                ShowFiles(Convert.ToInt32(Request["CreditNoteID"]));
            }
        }

        #region ShowFiles(int InvoiceID)
        private void ShowFiles(int InvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetUploadFileDetailsCN_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CreditNoteID", InvoiceID);
            DataSet ds = new DataSet();

            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            grdFile.DataSource = ds.Tables[0];
            grdFile.DataBind();
            if (grdFile.Items.Count > 0)
            {
                lblMsg.Visible = false;
            }
            else
            {
                lblMsg.Visible = true;
            }
        }
        #endregion


        #region btnUp_Click
        private void btnUp_Click(object sender, System.EventArgs e)
        {
            int retval = 0;
            lblMsg.Visible = false;
            if (IsValidFile() == true)
            {
                //modified by kuntal karar onn 23rd july2015
                strBaseDirectory = strBaseDirectory + strChildBuyer;
                //--------------------------------------------------
                for (int i = 1; i <= 5; i++)
                {
                    string sFileName = Convert.ToString(((HtmlInputFile)FindControl("File" + i.ToString())).PostedFile.FileName);
                    if (sFileName != "")
                    {
                        retval = 1;
                        try
                        {
                            if (strBaseDirectory.EndsWith("\\") == false)
                            {
                                //modified by kuntal karar on 23rd july2015
                                //strBaseDirectory += "\\";
                                strBaseDirectory = strBaseDirectory + "\\";
                                //-------------------------------------------
                            }
                            sFileName = strBaseDirectory + Convert.ToInt32(Request["CreditNoteID"]) + "_" + Path.GetFileName(sFileName);
                            int iRetVal = SaveFileInDisk(sFileName, ((HtmlInputFile)FindControl("File" + i.ToString())));
                            if (iRetVal == 1)
                            {
                                SaveFileDataInDb(sFileName, Convert.ToInt32(Request["CreditNoteID"]));
                                lblMsg.Visible = false;
                                ShowFiles(Convert.ToInt32(Request["CreditNoteID"]));
                            }
                            else
                            {
                                lblMsg.Text = "Sorry. server not found.";
                                lblMsg.Visible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = ex.Message;
                            lblMsg.Visible = true;
                            break;
                        }
                        if (retval == 0)
                        {
                            lblMsg.Text = "Please select file before upload";
                            lblMsg.Visible = true;
                        }
                    }
                }

                if (retval == 0)
                {
                    lblMsg.Text = "Please select file before upload";
                    lblMsg.Visible = true;
                }
            }
            else
            {
                string sErrfileName = null;
                IEnumerator myEnumerator = arr.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    sErrfileName = sErrfileName + "<tr><td align=left>";
                    sErrfileName = sErrfileName + myEnumerator.Current.ToString();
                    sErrfileName = sErrfileName + "</td></tr>";
                }

                lblMsg.Text = "<table border=0 cellpadding=2 class=ErrMsg><tr><td align=left><b>File already exist...</b></td></tr>" + sErrfileName + "</table>";
                lblMsg.Visible = true;
            }

        }
        #endregion


        private bool IsValidFile()
        {
            arr = new ArrayList();
            for (int i = 1; i <= 5; i++)
            {
                if (Path.GetFileName(Convert.ToString(((HtmlInputFile)FindControl("File" + i.ToString())).PostedFile.FileName)).Trim() != "")
                {
                    string sFileName = Convert.ToInt32(Request["CreditNoteID"]) + "_" + Path.GetFileName(Convert.ToString(((HtmlInputFile)FindControl("File" + i.ToString())).PostedFile.FileName));
                    try
                    {
                        DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                        RecordSet rs = da.ExecuteQuery("vwUploadDocument_CN", "CreditNoteID=" + Convert.ToInt32(Request["CreditNoteID"]) + " and ImagePath like '%" + sFileName + "%'");
                        while (!rs.EOF())
                        {
                            arr.Add(rs["Imagepath"]);
                            rs.MoveNext();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            if (arr.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region SaveFileInDisk
        private int SaveFileInDisk(string sFileName, HtmlInputFile httpFile)
        {
            bool iRetVal = false;
            int iRetValue = 0;
            if (sFileName != "")
            {
                iRetVal = UploadFile(sFileName, httpFile);
                if (iRetVal == false)
                    iRetValue = 0;
                else
                    iRetValue = 1;
            }
            return iRetValue;
        }
        #endregion

        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }
        private string GetURL2()
        {
            return ConfigurationManager.AppSettings["ServiceURLNew"];
        }

        #region UploadFile
        private bool UploadFile(string sFileName, HtmlInputFile httpFile)
        {
            Stream oStream;
            int FileLen;
            string strReturn = "";
            bool bolResult = false;
            CBSolutions.ETH.Web.WebRefNew.DownloadNew objService2 = new CBSolutions.ETH.Web.WebRefNew.DownloadNew();
            objService2.Url = GetURL2();

            try
            {
                FileLen = httpFile.PostedFile.ContentLength;
                byte[] oFileByte = new byte[FileLen];
                oStream = httpFile.PostedFile.InputStream;
                oStream.Read(oFileByte, 0, FileLen);
                bolResult = objService2.UploadFileNew(sFileName, oFileByte, strReturn, strChildBuyer);
                if (bolResult == false)
                {
                    throw new Exception(strReturn);
                }

            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
            finally
            {
                objService2 = null;
            }
            return bolResult;
        }
        #endregion

        #region  SaveFileDataInDb
        private void SaveFileDataInDb(string sFileName, int InvoiceID)
        {
            string sWebSrvPath = sFileName;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand("upd_InvoiceDocument_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CreditNoteID", InvoiceID);
            sqlCmd.Parameters.Add("@CompanyID", Convert.ToInt32(Session["CompanyID"]));
            sqlCmd.Parameters.Add("@Path", sWebSrvPath);
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }
        #endregion

        #region RemoveFile
        private bool RemoveFile(string sPath, int iStep)
        {
            string sFileName = "";
            bool bRetVal = false;
            if (iStep == 0)
            {
                try
                {
                    sFileName = sPath.Replace("I:", "C:\\P2D");
                    sFileName = sFileName.Replace("\\90104-server2", "C:\\P2D");
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = GetURL();
                    bRetVal = objService.RemoveFile(sFileName);
                }
                catch
                {

                }
            }
            if (iStep == 1)
            {
                try
                {
                    CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
                    objService2.Url = GetURL2();
                    sFileName = sPath.Replace("\\90107-server3", @"C:\File Repository");
                    sFileName = GetTrimFirstSlash(sFileName);
                    bRetVal = objService2.RemoveFile(sFileName);
                }
                catch
                {

                }
            }
            return bRetVal;
        }
        #endregion

        private void grdFile_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (DataBinder.Eval(e.Item.DataItem, "archiveImagePath") != DBNull.Value)
                {
                    if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")) != "")
                    {
                        ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")).Trim());
                    }
                }
                else
                {
                    if (DataBinder.Eval(e.Item.DataItem, "ImagePath") != DBNull.Value)
                    {
                        if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")) != "")
                        {
                            ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")).Trim());
                        }
                        else
                        {
                            ((Label)e.Item.FindControl("lblPath")).Text = "N/A";
                        }
                    }
                }
            }
        }


        private void grdFile_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            bool bFound = false;
            int DocumentID = 0;
            //			string sScrpt="";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                lblMsg.Text = "";
                DocumentID = Convert.ToInt32(((Label)e.Item.FindControl("lblDocID")).Text);
                if (Convert.ToString(e.CommandArgument) == "DOW")
                {
                    string sDownLoadPath = ((Label)e.Item.FindControl("lblHidePath")).Text;
                    sDownLoadPath = sDownLoadPath.Replace("I:", "C:\\P2D");
                    sDownLoadPath = sDownLoadPath.Replace("\\90104-server2", "C:\\P2D");
                    sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
                    if (sDownLoadPath.Trim() != "")
                    {
                        if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                        {
                            try
                            {
                                bFound = ForceDownload(sDownLoadPath, 0);
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            bFound = ForceDownload(sDownLoadPath, 0);

                        }
                    }
                    if (bFound == false)
                    {
                        sDownLoadPath = ((Label)e.Item.FindControl("lblArchPath")).Text;
                        sDownLoadPath = sDownLoadPath.Replace("\\90107-server3", @"C:\File Repository");
                        sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
                        if (sDownLoadPath.Trim() != "")
                        {
                            if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                            {
                                try
                                {
                                    bFound = ForceDownload(sDownLoadPath, 1);
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                bFound = ForceDownload(sDownLoadPath, 1);
                            }

                        }
                    }
                }

                if (Convert.ToString(e.CommandArgument) == "DEL")
                {
                    int iDocID = 0, iCrnID = 0;
                    string sPath = "";
                    bool isDeleted = false;

                    int iCreditNoteID = GetCurrentCreditNoteIDByCreditNoteNo(((Label)e.Item.FindControl("lblCreditNoteID")).Text);	//amitava18072007

                    iCrnID = iCreditNoteID;
                    iDocID = Convert.ToInt32(((Label)e.Item.FindControl("lblDocID")).Text);
                    sPath = ((Label)e.Item.FindControl("lblHidePath")).Text;

                    if (sPath != null && sPath.Trim() != "")
                    {
                        try
                        {
                            isDeleted = RemoveFile(((Label)e.Item.FindControl("lblHidePath")).Text, 0);
                        }
                        catch
                        {

                        }
                    }

                    if (isDeleted == false)
                    {
                        sPath = ((Label)e.Item.FindControl("lblArchPath")).Text;
                        if (sPath != null && sPath.Trim() != "")
                        {
                            sPath = GetTrimFirstSlash(sPath);
                            try
                            {
                                RemoveFromDB(iDocID, iCrnID, sPath);
                                isDeleted = RemoveFile(sPath, 1);
                            }
                            catch
                            {
                            }
                        }
                        if (isDeleted == true)
                        {
                            RemoveFromDB(iDocID, iCrnID, sPath);
                        }
                        else
                        {
                            //lblMsg.Text = "File not found...";
                            //lblMsg.Visible = true;
                        }
                    }
                    else
                    {
                        RemoveFromDB(iDocID, iCrnID, sPath);
                    }
                }
            }
        }


        #region GetTrimFirstSlash
        private string GetTrimFirstSlash(string sVal)
        {
            string sFName = sVal;
            if (sVal != "" & sVal != null)
            {

                string sInfo = sVal;
                sInfo.Replace(@"\", @"\\");
                string[] delValue = sInfo.Split(new char[] { '\\' });
                sFName = "";
                for (int x = 0; x < delValue.Length; x++)
                {
                    if (delValue[x] != "")
                    {
                        sFName = sFName + delValue[x];
                        if (x != delValue.Length - 1)
                        {
                            sFName = sFName + @"\";
                        }
                    }
                }
            }
            return sFName;
        }
        #endregion


        #region private void RemoveFromDB(int DocID,int CreditNoteID,string sPath)
        private void RemoveFromDB(int DocID, int CreditNoteID, string sPath)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand("Del_InvoiceDocument_CN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@DocID", DocID);
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            ShowFiles(CreditNoteID);
        }
        #endregion


        #region ForceDownload(string sPath,int iStep)
        private bool ForceDownload(string sPath, int iStep)
        {
            bool bRetVal = false;
            string sFileName = sPath;
            if (iStep == 0)
            {
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = GetURL();
                    byte[] bytBytes = objService.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        Response.ContentType = "application//octet-stream";
                        Response.BinaryWrite(bytBytes);
                        Response.Flush();
                        Response.End();
                        fs1.Close();
                        fs1 = null;
                        bRetVal = true;
                    }
                }
                catch
                {
                }
            }
            else if (iStep == 1)
            {
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
                    objService2.Url = GetURL2();
                    byte[] bytBytes = objService2.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        Response.ContentType = "application//octet-stream";
                        Response.BinaryWrite(bytBytes);
                        Response.Flush();
                        Response.End();
                        fs1.Close();
                        fs1 = null;
                        bRetVal = true;
                    }
                }
                catch
                {
                }
            }
            return bRetVal;
        }
        #endregion


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
            this.grdFile.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdFile_ItemCommand);
            this.grdFile.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdFile_ItemDataBound);
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion


        #region btnBack_Click
        private void btnBack_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(Request["ReturnUrl"]);
        }
        #endregion


        #region GetCurrentCreditNoteIDByCreditNoteNo
        public int GetCurrentCreditNoteIDByCreditNoteNo(string strCreditNoteNo)
        {
            int iReturnValue = 0;
            SqlParameter sqlReturnParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetCurrentCreditNoteIDByCreditNoteNo", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion

        #region GetInvoiceBuyerCompanyID
        public void GetInvoiceBuyerCompanyID(int iInvoiceID)
        {

            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT creditnote.BuyerCompanyID,creditnote.SupplierCompanyID,Company.CompanyName FROM creditnote,Company WHERE creditnote.BuyerCompanyID=Company.CompanyID and creditnoteID=" + iInvoiceID;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables.Count > 0)
                {
                    BuyerID = Convert.ToInt32(Dst.Tables[0].Rows[0]["BuyerCompanyID"].ToString());
                    strChildBuyer = Dst.Tables[0].Rows[0]["CompanyName"].ToString();
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            //			return BuyerID;
        }
        #endregion

        protected void getInvoiceNo(string CreditNotsID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            string sQuery = "select i.InvoiceId,i.InvoiceNo from Invoice i,creditnote c  where i.BuyerCompanyID=c.BuyerCompanyID ";
            sQuery += " And i.SupplierCompanyID=c.SupplierCompanyID And c.CreditNoteID=" + CreditNotsID;
            sQuery += " and c.CreditInvoiceNo = i.InvoiceNo ";

            sqlCmd = new SqlCommand(sQuery, sqlConn);

            SqlDataReader dr1 = null;
            try
            {
                sqlConn.Open();
                dr1 = sqlCmd.ExecuteReader();

                while (dr1.Read())
                {
                    InvoiceID = Convert.ToString(dr1["InvoiceId"]);
                    strInvoiceNo = Convert.ToString(dr1["InvoiceNo"]);
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr1.Close();
                sqlConn.Close();
            }
            if (InvoiceID != "")
                sDisplay = "block";

        }
    }
}