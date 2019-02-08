using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestTiff
{
    public partial class Ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnTiff_Click(object sender, EventArgs e)
        {
            string token = ctlTiff.LoadTiff(Server.MapPath("~/files/sample.tif"));

            if (token.Length > 0)
            {
                string script = ctlTiff.GetInitScript(true) + " Wait(false); ";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "InitViewer", script, true);
            }


        }
    }
}