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
    /// Summary description for ImgPopNew.
    /// </summary>
    public class ImgPopNew : CBSolutions.ETH.Web.ETC.VSPage //System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button pbReset;

        protected System.Web.UI.WebControls.Button pbClose1;
        protected string strImg = "";

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                strImg = "ImgUrl.aspx?sFile=" + Session["sFile"].ToString() + "&iPageNo=" + Request.QueryString["iPageNo"];
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
    }
}
