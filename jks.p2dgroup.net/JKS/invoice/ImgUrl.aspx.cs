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
    /// Summary description for ImgUrl.
    /// </summary>
    public class ImgUrl : CBSolutions.ETH.Web.ETC.VSPage
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["sFileType"].ToString() == "S3")
                {
                    WebRefNew.DownloadNew objService2 = new WebRefNew.DownloadNew();
                    if (objService2 != null)
                    {
                        objService2.Url = GetURLNew();
                        byte[] byteArray = objService2.GetMultiPages(Session["sFile"].ToString(), Convert.ToInt32(Request.QueryString["iPageNo"]));
                        System.IO.Stream _stream = new MemoryStream(byteArray);

                        Response.ContentType = "image/jpeg";
                        Response.BinaryWrite(byteArray);
                    }
                }
                else
                {
                    WEBRef.FileDownload objService = new WEBRef.FileDownload();
                    if (objService != null)
                    {
                        objService.Url = GetURL();
                        byte[] byteArray = objService.GetMultiPages(Session["sFile"].ToString(), Convert.ToInt32(Request.QueryString["iPageNo"]));
                        System.IO.Stream _stream = new MemoryStream(byteArray);

                        Response.ContentType = "image/jpeg";
                        Response.BinaryWrite(byteArray);
                    }
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
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region GetURLNew()
        private string GetURLNew()
        {
            return ConfigurationManager.AppSettings["ServiceURLNew"];
        }
        #endregion

        #region GetURL()
        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }
        #endregion
    }
}
