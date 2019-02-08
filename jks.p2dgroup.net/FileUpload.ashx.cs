using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace TestTiff
{
    /// <summary>
    /// Tiff File Upload
    /// </summary>
    public class FileUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "multipart/form-data";
            context.Response.Expires = -1;
            try
            {
                string relativePath = "~/files/saved/";
                HttpPostedFile postedFile = context.Request.Files["file"];
                string savepath = HttpContext.Current.Server.MapPath(relativePath);
                var extension = Path.GetExtension(postedFile.FileName);

                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);

                var id = Guid.NewGuid() + extension;
                if (extension != null)
                {
                    postedFile.SaveAs(savepath + "/" + id);
                    context.Response.StatusCode = 200;
                    context.Response.Write("{\"Uploaded\":\"" + relativePath + "/" + id + "\", \"StatusCode\":\"OK\"}");                   
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}