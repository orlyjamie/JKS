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
using System.Xml;
using System.ServiceProcess;
using System.Web.Services.Protocols;
using System.Diagnostics;
using System.Web.Mail;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;

namespace JKS
{
    /// <summary>
    /// Summary description for ExportInvoice.
    /// </summary>
    public class ExportInvoice : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Panel Panel2;

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    ProcessStartInfo i = new ProcessStartInfo(@"D:\CAppAqilla\CAppAqilla\bin\Debug\CAppAqilla.exe", " " + CompanyID + " ");
                    Process p = new Process();
                    p.StartInfo = i;
                    p.Start();
                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
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
    }
}
