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
using Microsoft.Win32;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Xml;

namespace JKS
{
    /// <summary>
    /// Summary description for ImgForm.
    /// </summary>
    public class ImgForm : CBSolutions.ETH.Web.ETC.VSPage //System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button pbReset;
        protected System.Web.UI.WebControls.Button pbClose1;

        private void close_validation_session()
        {
            string a_string = (String)Session["session_started"];
            if (a_string == "ended" || a_string == null) //Last case no session started or documents saved
                return;
            Session["session_started"] = "ended";
            object doc_id_string = Session["doc_id"];
            int clerk_id = Convert.ToInt32((String)Session["clerkid"]);

            if (doc_id_string != null)
            {
                int doc_id = Convert.ToInt32(doc_id_string);
            }
            int session_id = Convert.ToInt32(Session["sessionid"]);
            //			int no_stores_to_ipe_output=0;
            //			int no_stores_to_archive=0;
            //			int no_deletes=0;
            DateTime today_date_time = new DateTime();
            today_date_time = DateTime.Now;
        }


        private void pbClose1_Click(object sender, System.EventArgs e)
        {
            /* Amazingly this does not work when put into InvView.aspx.cs,
             only here, called indirectly from InvView.htm. Note similarity
             to pbReset which seems not to have a callback, but which is
             also a dummy asp button.*/
            //			int sdfdsf=1;
            close_validation_session();
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(1));

            if (!IsPostBack)
            {
            }
            string docid = Page.Request.Params["docid"];
            if (docid != null && docid != "null")
            {
                //				string filepath_prefix;
                string page = Page.Request.Params["page"];
                string client_name = (string)Session["clientname"];
                string clerk_id1 = (string)Session["clerkid"];
                string destination_prefix = (string)Session["Path to IIS Server"];

                string filename = "";
                DirectoryInfo directory_dir = new DirectoryInfo(destination_prefix);
                long max_ticks = 0;
                string found_name = "";
                string name = "DOC" + clerk_id1 + "_" + page + "_*.gif";
                int index;
                string suffix;
                //Find highest cache entry belonging to this clerk
                foreach (FileInfo nextFile2 in directory_dir.GetFiles(name))
                {
                    filename = nextFile2.Name;
                    index = filename.IndexOf("_");
                    index = filename.IndexOf("_", index + 1);
                    suffix = filename.Substring(index + 1);
                    index = suffix.IndexOf(".");
                    suffix = suffix.Substring(0, index);
                    if (Convert.ToInt64(suffix) > max_ticks)
                    {
                        max_ticks = Convert.ToInt64(suffix);
                        found_name = filename;
                    }
                }
                pbReset.Attributes.Add("ImgUrl", "work\\" + found_name); //Storing cache name for retrieval at client side
                Session["ticks_in_cache_filepath"] = max_ticks.ToString();
                pbReset.Attributes.Add("ImgPage", page);
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
            this.pbClose1.Click += new System.EventHandler(this.pbClose1_Click);


        }
        #endregion
    }
}
