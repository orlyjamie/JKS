using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewLook
{
    public partial class NewLook_ScanQC_Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string browser = Request.Browser.Type.ToUpper();

            Response.Write(browser);

            string x = ",0,8";

            x = x.Substring(1, x.Length - 1);

            Response.Write(x);

            Response.Redirect("ScanQCDoc.aspx", true);
        }
    }
}