using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Drawing;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;

namespace NewLook
{
    public partial class NewLook_ScanQC_TiffViewerScanQC : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        bool test = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["test"].ToString());
        DataCenter DC = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DC = new DataCenter(sqlConn);

            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    int comID = Convert.ToInt32(Request.QueryString["cid"]);
                    int docID = Convert.ToInt32(Request.QueryString["did"]);

                    DownloadAndSetTiffImage(comID, docID);
                }

                //#region DBCon
                //if (string.Equals(Convert.ToString(Session["type"]), "old"))
                //{
                //    ctlTiff.CacheZoom = 100;
                //    ctlTiff.ViewerWidth = 750;
                //    ctlTiff.ThumbWidth = 45;
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "zoom", "objTiffViewer.Zoom(25);", true);
                //    //ctlTiff.DefaultZoom = 100;
                //}
                //else
                //{
                //    ctlTiff.CacheZoom = 200;
                //    ctlTiff.ViewerWidth = 0;
                //    ctlTiff.ThumbWidth = 65;
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "zoom", "objTiffViewer.Zoom(50);", true);
                //    //ctlTiff.DefaultZoom = 200;
                //}
                //#endregion
            }

            Page.LoadComplete += new EventHandler(Page_LoadComplete);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            int c = ctlTiff.TotalPages;
            if (Session["pages"] != null)
                Session.Remove("pages");

            Session["pages"] = c;

            //#region DBCon
            //if (string.Equals(Convert.ToString(Session["type"]), "old"))
            //{
            //    ctlTiff.CacheZoom = 100;
            //    ctlTiff.ViewerWidth = 750;
            //    ctlTiff.ThumbWidth = 45;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "zoom", "objTiffViewer.Zoom(25);", true);
            //    //ctlTiff.DefaultZoom = 100;
            //}
            //else
            //{
            //    ctlTiff.CacheZoom = 200;
            //    ctlTiff.ViewerWidth = 0;
            //    ctlTiff.ThumbWidth = 65;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "zoom", "objTiffViewer.Zoom(50);", true);
            //    //ctlTiff.DefaultZoom = 200;
            //}
            //#endregion
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void DownloadAndSetTiffImage(int comID, int docID)
        {
            //Response.Write("<br />Is Test: " + test);
            if (test == true)
            {
                string imgURL = @"C:\P2D\FTP Upload\Default Company\Default Batch\Default.tif";//Default.tif

                //Response.Write("<br />File Exists: " + File.Exists(imgURL));

                if (File.Exists(imgURL))
                {
                    try
                    {
                        //ctlTiff.LoadTiff(imgURL);

                        System.IO.FileStream fs1 = System.IO.File.Open(imgURL, FileMode.Open, FileAccess.Read);
                        byte[] b1 = new byte[fs1.Length];
                        fs1.Read(b1, 0, (int)fs1.Length);
                        fs1.Close();

                        //Response.Write("<br />converted to byte.");

                        Stream stream = new MemoryStream(b1);
                        ctlTiff.LoadTiff(stream);

                        //Response.Write("<br />applied.");
                    }
                    catch (Exception ex)
                    {
                        string ss = "Error:<br />Message: " + ex.Message + "<br />Source: " + ex.Source + "<br />StackTrace: " + ex.StackTrace + "<br />TargetSite: " + ex.TargetSite + "<br />InnerException: " + ex.InnerException + "<br />Data: " + ex.Data;
                        Response.Write(ss);
                    }
                }
                else
                {
                    Response.Write("<br />No Image retrieved.");
                }
            }
            else
            {
                string companyName = "";
                string batchName = "";
                string originalName = "";

                DataTable DT = DC.PathInformationTable(comID, docID);

                int c = DT.Rows.Count;

                //Response.Write("<br />DT Row Count in LoadTopData: " + c);

                if (c > 0)
                {
                    DataRow DR = DT.Rows[0];

                    companyName = DR["CompanyName"].ToString();
                    batchName = DR["BATCH NAME"].ToString();
                    originalName = DR["ORIGINAL NAME"].ToString();
                }
                else
                {
                    companyName = "";
                    batchName = "";
                    originalName = "";
                }

                //Response.Write("companyName = " + companyName + " batchName = " + batchName + " originalName = " + originalName + "<br />");

                try
                {
                    //string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceURLNew"];
                    //CBSolutions.ETH.Web.WebRefNew.DownloadNew objService = new CBSolutions.ETH.Web.WebRefNew.DownloadNew();
                    //objService.Url = serviceUrl;

                    string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceURL"];
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = serviceUrl;

                    string downloadURL = "//90104-server2/FTP Upload/" + companyName + "/" + batchName + "/" + originalName;
                    //string downloadURL = "C:\\P2D\\FTP Upload\\" + companyName + "\\" + batchName + "\\" + originalName;

                    downloadURL = downloadURL.Replace("/", @"\");

                    byte[] bytes = objService.DownloadFile(downloadURL);
                    //Response.Write("Download URL: " + downloadURL + "<br />Service URL: " + serviceUrl);

                    if (bytes != null)
                    {
                        Stream stream = new MemoryStream(bytes);
                        ctlTiff.LoadTiff(stream);

                        //string imgURL = "D:\\P2D\\IPE Output\\" + companyName + "\\" + originalName;

                        //File.WriteAllBytes(imgURL, bytes);
                        //ctlTiff.LoadTiff(imgURL);

                        //Response.Write(imgURL);
                    }
                    else
                    {
                        Response.Write("No Image retrieved.");
                    }
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                    Response.Write("Error in DownloadAndSetTiffImage: " + ss);
                }
            }
        }
    }

    class DataCenter
    {
        SqlConnection sqlConn = null;

        /// <summary>
        /// Constructor of DataCenter Class takes SqlConnection Object
        /// </summary>
        /// <param name="sqlConn">SqlConnection Object</param>
        public DataCenter(SqlConnection sqlConn)
        {
            this.sqlConn = sqlConn;
        }

        /// <summary>
        /// Takes a int value of Doc ID and returns the DataTable with columns like [ORIGINAL NAME], [CLIENT ID], [CompanyName] 
        /// </summary>
        /// <param name="comID">comID the Selected company's id as int.</param>
        /// <param name="docID">docID the Document No as int.</param>
        /// <returns>Returns DataTable</returns>
        public DataTable PathInformationTable(int comID, int docID)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "SELECT dp.[ORIGINAL NAME], cb.[CLIENT ID], cb.[BATCH NAME], c.CompanyName " +
                "FROM [DOCUMENT PROGRESS] dp, [CLIENT BATCHES] cb, [Company] c " +
                "WHERE dp.[BATCH ID] = cb.[BATCH ID] AND cb.[CLIENT ID] = c.CompanyID " +
                "AND cb.[CLIENT ID] = @comID AND dp.[DOC ID]= @docID;";

                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.Add("@comID", SqlDbType.Int).Value = comID;
                sqlCmd.Parameters.Add("@docID", SqlDbType.Int).Value = docID;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("document");
                DA.Fill(DT);

                return DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
                HttpContext.Current.Response.Write("<br />Error in PathInformationTable: " + ss);

                return DT;
            }
            finally
            {
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }
    }
}