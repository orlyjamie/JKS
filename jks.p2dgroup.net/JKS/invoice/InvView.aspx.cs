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
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Xml;
//using System.Web;
using Microsoft.Win32;
//using System.ComponentModel;
//using System.Collections.Specialized;
//using Microsoft.Win32;

namespace JKS
{
    /// <summary>
    /// Summary description for InvView.
    /// </summary>
    public class InvView : CBSolutions.ETH.Web.ETC.VSPage
    {
        //		private int year=7774;
        protected System.Web.UI.WebControls.TextBox tbSupplierCode;
        protected System.Web.UI.WebControls.TextBox tbInvoiceNo;
        protected System.Web.UI.WebControls.TextBox tbInvoiceDate;
        protected System.Web.UI.WebControls.TextBox tbVATRegNo;
        protected System.Web.UI.WebControls.Table ItemTable;
        protected System.Web.UI.WebControls.TextBox tbData;
        protected System.Web.UI.WebControls.Button pbSubmit;
        protected System.Web.UI.WebControls.Button pbSave;
        protected System.Web.UI.WebControls.Button pbDiscard2;

        protected System.Web.UI.WebControls.Button pbUpdate;
        protected System.Web.UI.WebControls.Button pbPermanentSave;
        protected System.Web.UI.WebControls.Button pbClose1;
        protected System.Web.UI.WebControls.Button pbDelete;
        //		private int month=66; 
        protected System.Web.UI.WebControls.TextBox tbNet;
        protected System.Web.UI.WebControls.TextBox tbDiscount;
        protected System.Web.UI.WebControls.TextBox tbVat;
        protected System.Web.UI.WebControls.TextBox tbGross;
        protected System.Web.UI.WebControls.TextBox ddDocType;
        protected System.Web.UI.WebControls.TextBox ddCurrency;
        protected System.Web.UI.WebControls.TextBox tbTaxPoint;
        protected System.Web.UI.WebControls.TextBox tbInvoiceAddress;
        protected System.Web.UI.WebControls.TextBox tbSuppAddress;
        protected System.Web.UI.WebControls.TextBox tbDelAddress;
        protected System.Web.UI.WebControls.Table AnalysisTable;
        protected System.Web.UI.WebControls.TextBox Textbox1;
        public double lastNet;
        public double lastVat;
        public double lastsumTotal;
        public int toggle_vat;
        public int just_saved;
        protected System.Web.UI.WebControls.TextBox TextBox2;
        public String updated;
        public string clientname = "Client B";
        protected System.Web.UI.WebControls.TextBox tbPONo;
        protected System.Web.UI.WebControls.TextBox tbVendorRef;
        public int clientid = -1;
        protected System.Web.UI.WebControls.Label Label1;
        protected System.Web.UI.WebControls.TextBox TextBox3;
        protected System.Web.UI.WebControls.TextBox TextBox4;
        protected System.Web.UI.WebControls.TextBox TextBoxGlobals2;
        protected System.Web.UI.WebControls.TextBox TextBoxResolved;
        protected System.Web.UI.WebControls.TextBox TextBoxGlobals;
        protected System.Web.UI.WebControls.TextBox TextBox5;
        protected System.Web.UI.WebControls.TextBox TextboxClerkId;
        protected System.Web.UI.WebControls.Label Label2;
        protected System.Web.UI.WebControls.Button pbreallyclosing;
        //		private bool delete_all;
        protected System.Web.UI.WebControls.Button Button20;
        protected System.Web.UI.WebControls.TextBox Textbox10;
        public int clerk_id = -1;

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                tbData.Attributes.Add("ImgType", ".jpg");
                tbData.Attributes.Add("ImgDoc", "doc_id_no_E");
                tbData.Attributes.Add("ImgPath", "work\\");
                tbData.Attributes.Add("ImgPage", "1");
                tbData.Attributes.Add("ImgPages", Session["sPages"].ToString());
                tbData.Attributes.Add("ImgLines", "0");
                tbData.Attributes.Add("ImgHeight", "100");
                tbData.Attributes.Add("ImgWidth", "100");
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            // First one JCB
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
