using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;


namespace CBSolutions.ETH.Web.ETC.creditnotes
{
    /// <summary>
    /// Summary description for ImgPopup.
    /// </summary>
    public class ImgPopup_NL_CN : CBSolutions.ETH.Web.ETC.VSPage
    {

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            string imgHidePath = "";
            string imgArchPath = "";
            bool bFound = false;
            int DocumentID = Convert.ToInt32(Request["DocumentID"]);
            GetImagePath(DocumentID, out imgHidePath, out imgArchPath);
            imgHidePath = imgHidePath.Replace("I:", "C:\\P2D");
            imgHidePath = imgHidePath.Replace("\\90104-server2", "C:\\P2D");
            string sDownLoadPath = "";
            sDownLoadPath = imgHidePath;
            sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
            if (sDownLoadPath.Trim() != "")
            {
                try
                {
                    bFound = ForceDownload(sDownLoadPath, 0);
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(@"D:\inetpub\wwwroot\p2dgroup\Logs\ErrorLog.txt", ex.Message);
                }
            }
            if (bFound == false)
            {
                sDownLoadPath = "";
                imgArchPath = imgArchPath.Replace("\\90107-server3", @"C:\File Repository");
                sDownLoadPath = imgArchPath;
                sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
                if (sDownLoadPath.Trim() != "")
                {
                    try
                    {
                        bFound = ForceDownload(sDownLoadPath, 1);
                    }
                    catch (Exception ex)
                    {
                        Utility.ErrorLog(@"D:\inetpub\wwwroot\p2dgroup\Logs\ErrorLog.txt", ex.Message);
                    }
                }
            }

            if (bFound == false)
            {
                Response.Write("<table border=0 cellpadding=2 class=ErrMsg width=100% height=100%><tr><td align=center valign=middle><b>File does not exist...</b></td></tr>  " + sDownLoadPath + "  </table>");
            }
        }
        private void GetImagePath(int DocumentID, out string imgHidePath, out string imgArchPath)
        {
            imgHidePath = "";
            imgArchPath = "";
            string sSql = "select ImagePath,ArchiveImagePath from UploadDocument_CN where DocumentID=" + DocumentID;
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
            sqlCmd.CommandType = CommandType.Text;
            SqlDataReader sqlDR = null;
            try
            {
                sqlConn.Open();
                sqlDR = sqlCmd.ExecuteReader();

                if (sqlDR.HasRows)
                {
                    while (sqlDR.Read())
                    {
                        if (sqlDR["ImagePath"] != DBNull.Value)
                        {
                            imgHidePath = Convert.ToString(sqlDR["ImagePath"]);
                        }

                        if (sqlDR["ArchiveImagePath"] != DBNull.Value)
                        {
                            imgArchPath = Convert.ToString(sqlDR["ArchiveImagePath"]);
                        }

                    }
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDR.Close();
                sqlConn.Dispose();
            }
        }
        private bool ForceDownload(string sPath, int iStep)
        {
            bool bRetVal = false;
            string sFileName = sPath;
            if (iStep == 0)
            {
                try
                {
                    WEBRef.FileDownload objService = new WEBRef.FileDownload();
                    objService.Url = GetURL();
                    byte[] bytBytes = objService.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        Response.Clear();
                        Response.ContentType = "image/tiff";
                        Response.BinaryWrite(bytBytes);
                        Response.End();
                        objService.Dispose();
                        bRetVal = true;
                    }
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(@"D:\inetpub\wwwroot\p2dgroup\Logs\ErrorLog.txt", ex.Message);
                }
            }
            else if (iStep == 1)
            {
                try
                {
                    WEBRef2.FileDownload objService2 = new WEBRef2.FileDownload();
                    objService2.Url = GetURL2();
                    byte[] bytBytes = objService2.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        Response.Clear();
                        Response.ContentType = "image/tiff";
                        Response.BinaryWrite(bytBytes);
                        Response.End();
                        objService2.Dispose();
                        bRetVal = true;
                    }
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(@"D:\inetpub\wwwroot\p2dgroup\Logs\ErrorLog.txt", ex.Message);
                }
            }
            return bRetVal;
        }

        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }

        private string GetURL2()
        {
            return ConfigurationManager.AppSettings["ServiceURL2"];
        }

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
    }
}
