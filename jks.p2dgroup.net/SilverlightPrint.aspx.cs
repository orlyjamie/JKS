using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestTiff
{
    public partial class SilverlightPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlPrint.Token = Request.QueryString["token"];
                ctlPrint.TotalPages = Convert.ToInt32(Request.QueryString["printtotal"]);
                ctlPrint.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath;
                ctlPrint.DocumentName = Request.QueryString["docName"];
            }
        }
    }
}