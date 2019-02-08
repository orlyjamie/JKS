using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using System.Configuration;
using Microsoft.Data.Odbc;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace JKS
{
    /// <summary>
    /// Summary description for ImgPopup.
    /// </summary>
    public class ImgPopup_NL_INV : System.Web.UI.Page
    {
        protected string imgSrc = "";
        private void Page_Load(object sender, System.EventArgs e)
        {
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
            if (sDownLoadPath != null && sDownLoadPath.Trim() != "")
            {
                try
                {
                    bFound = ForceDownload(sDownLoadPath, 0);
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(Server.MapPath(@"..\..\Logs\ErrorLog.txt"), ex.Message);
                }
            }
            if (bFound == false)
            {
                sDownLoadPath = "";
                imgArchPath = imgArchPath.Replace("\\90107-server3", @"C:\File Repository");
                sDownLoadPath = imgArchPath;
                sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
                if (sDownLoadPath != null && sDownLoadPath.Trim() != "")
                {
                    try
                    {
                        bFound = ForceDownload(sDownLoadPath, 1);
                    }
                    catch (Exception ex)
                    {
                        Utility.ErrorLog(Server.MapPath(@"..\..\Logs\ErrorLog.txt"), ex.Message);
                    }
                }
            }

        }

        #region GetImagePath
        private void GetImagePath(int DocumentID, out string imgHidePath, out string imgArchPath)
        {
            imgHidePath = "";
            imgArchPath = "";
            string sSql = "select ImagePath,ArchiveImagePath from UploadDocument where DocumentID=" + DocumentID;
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
        #endregion

        #region ForceDownload
        private bool ForceDownload(string sPath, int iStep)
        {
            bool bRetVal = false;
            string sFileName = sPath;
            if (iStep == 0)
            {
                try
                {
                    Session["sFile"] = sFileName.ToString();
                    Session["sFileType"] = "S2";
                    WEBRef.FileDownload objService = new WEBRef.FileDownload();
                    if (objService != null)
                    {
                        objService.Url = GetURL();
                        Session["sPages"] = Convert.ToString(objService.GetPageCount(sFileName));
                        Response.Redirect("ImgViewNew.htm");
                    }
                    else
                    {
                        Utility.ErrorLog(Server.MapPath(@"..\..\Logs\ErrorLog.txt"), "Webservice not accessible");
                    }
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(Server.MapPath(@"..\..\Logs\ErrorLog.txt"), ex.Message);
                }
            }
            else if (iStep == 1)
            {
                try
                {
                    Session["sFile"] = sFileName.ToString();
                    Session["sFileType"] = "S3";
                    WebRefNew.DownloadNew objService2 = new WebRefNew.DownloadNew();
                    if (objService2 != null)
                    {
                        objService2.Url = GetURLNew();
                        Session["sPages"] = Convert.ToString(objService2.GetPageCount(sFileName));
                    }

                    Response.Redirect("ImgViewNew.htm");
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(Server.MapPath(@"..\..\Logs\ErrorLog.txt"), ex.Message);
                }
            }
            return bRetVal;
        }
        #endregion

        #region GetURL()
        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }
        #endregion

        #region GetURL2()
        private string GetURL2()
        {
            return ConfigurationManager.AppSettings["ServiceURL2"];
        }
        #endregion

        #region GetURLNew()
        private string GetURLNew()
        {
            return ConfigurationManager.AppSettings["ServiceURLNew"];
        }
        #endregion

        #region GetTrimFirstSlash(string sVal)
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

        #region ConvertStreamToByteBuffer
        public byte[] ConvertStreamToByteBuffer(System.IO.Stream theStream)
        {
            int b1;
            System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
            while ((b1 = theStream.ReadByte()) != -1)
            {
                tempStream.WriteByte(((byte)b1));
            }
            return tempStream.ToArray();
        }

        #endregion

        #region getEncoderInfo
        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        #endregion

    }
}
