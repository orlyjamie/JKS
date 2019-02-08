using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


public partial class tiny_mce_plugins_advimage_ImageBrowser : System.Web.UI.Page
{
    public string strDone = "";
    public string strURL = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        string mainpath = Server.MapPath("~").ToString() + "\\PageContentImages\\";
        foreach (string f in Request.Files.AllKeys)
        {
            HttpPostedFile file = Request.Files[f];
            string flnameonly = file.FileName.ToString();
            String mext1 = Path.GetExtension(file.FileName).ToString();
            String ext1 = mext1.ToLower();
            String strfilename = Path.GetFileName(file.FileName).ToString();

            string sFilename = "img_" + DateTime.Now.ToString("yyyyMMdd_HHmmssttt") + ext1;
            if ((ext1 == ".jpg") || (ext1 == ".bmp") || (ext1 == ".gif") || (ext1 == ".png"))
            {
                file.SaveAs(mainpath + sFilename);
            }

            //Page.RegisterClientScriptBlock("Add Script", "tinyMCEPopup.onInit.add(FileBrowserDialogue.init, FileBrowserDialogue);");
            strDone = "tinyMCEPopup.onInit.add(FileBrowserDialogue.init, FileBrowserDialogue);";

            //strURL = "http://72.167.62.83/OWT/PageContentImages/" + sFilename;
            strURL = "http://" + Request.ServerVariables["HTTP_HOST"]+"/Benlegel/PageContentImages/" + sFilename;
        }

    }

   
}
