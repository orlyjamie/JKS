using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class JKS_invoice_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WSCheckExistingFileFolder obj = new WSCheckExistingFileFolder();

        string urlStr = System.Configuration.ConfigurationManager.AppSettings["WSCheckExistingFileFolder"].ToString();
        obj.Url = urlStr;

        string FilePath = @"C:\P2D\IPE Output\DOC1399.xml";

        bool tf = obj.ReturnIfFileExists(FilePath);

        Response.Write(tf.ToString());

        Response.Redirect("InvView.aspx");
    }
}