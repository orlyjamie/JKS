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
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Text;
using System.Collections.Specialized;
using iTextSharp.text;
using System.Collections.Generic;

namespace JKS
{
    public partial class JKS_downloadDB_DownLoadFiles : CBSolutions.ETH.Web.ETC.VSPage
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Label Label1;
        //  protected System.Web.UI.WebControls.Label lblMessage;
        //  protected System.Web.UI.WebControls.DataGrid grdDownLoad;
        #endregion

        #region Objects and datatypes
        DataTable dtblFiles = new DataTable();
        DataSet ds = new DataSet();
        string Todaysdaye = DateTime.Today.Date.ToString("dd/MM/yy").Replace("/", "");
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            if (!IsPostBack)
            {
                CreateTable();
                GetFilesFromServer1();

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

        #region GetFilesFromServer()
        public void GetFilesFromServer()
        {
            //			string recent20Days="";
            string strXmlFiles = "";

            DirectoryInfo dir = new DirectoryInfo("D:\\JKS Data Files\\OUT\\");
            FileInfo[] f1 = null;
            f1 = dir.GetFiles("*.*");
            ListBox listBox = new ListBox();

            SortAndFillListBox(dir, listBox);

            strXmlFiles += "<?xml version=\"1.0\" standalone=\"yes\"?>";
            strXmlFiles += "<?xml-stylesheet type=\"text/xsl\" href=\"test.xsl\"?>";
            strXmlFiles += "<NewDataSet>";

            for (int i = 0; i < f1.Length; i++)
            {
                string sFileName = f1[i].FullName.Trim();
                string[] arrFileNames = new string[20];
                if (Path.GetExtension(sFileName).ToUpper() == ".CSV")
                {
                    DataRow Dr = dtblFiles.NewRow();

                    if (Session["FileCreateTime"] != null)
                    {
                        if (Convert.ToInt32(Session["FileCreateTime"]) < Convert.ToInt32(Convert.ToDateTime(f1[i].CreationTime).Day))
                            continue;
                    }
                    strXmlFiles += "<Table1>";

                    Dr["FileName"] = Path.GetFileName(sFileName).ToString();
                    strXmlFiles += "<FileName>" + Dr["FileName"] + "</FileName>";
                    Dr["FilePath"] = "D:\\JKS Data Files\\OUT\\" + Path.GetFileName(sFileName).ToString();
                    strXmlFiles += "<FilePath>" + Dr["FilePath"] + "</FilePath>";
                    DateTime AsDate = DateTime.Parse("Fri, 08 Jun 2007 10:15:37 GMT");
                    AsDate = f1[i].CreationTime;
                    Dr["FileCreateDate"] = AsDate.ToString("dd-MMM-yy");
                    strXmlFiles += "<FileCreateDate>" + Dr["FileCreateDate"] + "</FileCreateDate>";
                    Dr["DateDay"] = Convert.ToDateTime(f1[i].CreationTime).Day;
                    strXmlFiles += "<DateDay>" + Dr["DateDay"] + "</DateDay>";
                    Session["FileCreateTime"] = Dr["DateDay"];
                    Dr["DateMonth"] = Convert.ToDateTime(f1[i].CreationTime).Month;
                    strXmlFiles += "<DateMonth>" + Dr["DateMonth"] + "</DateMonth>";
                    Dr["DateYear"] = Convert.ToDateTime(f1[i].CreationTime).Year;
                    strXmlFiles += "<DateYear>" + Dr["DateYear"] + "</DateYear>";
                    Dr["DateHHMMSS"] = Convert.ToDateTime(f1[i].CreationTime).Ticks;
                    strXmlFiles += "<DateHHMMSS>" + Dr["DateHHMMSS"] + "</DateHHMMSS>";

                    strXmlFiles += "</Table1>";

                    dtblFiles.Rows.Add(Dr);
                }

            }
            strXmlFiles += "</NewDataSet>";

            Stream fs = File.Create(@"D:\Inetpub\wwwroot\JKS Data Files\Files\test.xml", 5000);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(strXmlFiles);
            sw.Close();
            fs.Close();
            fs = null;

            ds.Tables.Add(dtblFiles);
            DataSet ds2 = new DataSet();
            ds2.ReadXml(@"D:\Inetpub\wwwroot\JKS Data Files\Files\test.xml");
            XmlDataDocument mydata = new XmlDataDocument();
            mydata.DataSet.ReadXml(@"D:\Inetpub\wwwroot\JKS Data Files\Files\test.xml");

        }
        #endregion

        #region CreateTable()
        private void CreateTable()
        {
            try
            {
                dtblFiles.Columns.Add("FileName");
                dtblFiles.Columns.Add("FilePath");
            }
            catch { }

        }
        #endregion

        #region GetFilesFromServer1()
        public void GetFilesFromServer1()
        {
            DirectoryInfo dir = new DirectoryInfo("D:\\JKS Data Files\\OUT\\");
            FileInfo[] f1 = null;
            f1 = dir.GetFiles("*.*");
            ListBox listBox = new ListBox();
            SortAndFillListBox(dir, listBox);
        }
        #endregion
        #region protected void DeleteItem(object sender, System.EventArgs e)
        protected void DeleteItem(object sender, System.EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            string filename = Convert.ToString(lnkDelete.HRef);
            DownloadUserData(filename);
        }
        #endregion

        #region DownloadUserData(string flnm)
        public void DownloadUserData(string flnm)
        {
            string filepath = "D:\\JKS Data Files\\OUT\\" + flnm;
            try
            {
                Context.Response.ContentType = "application/save";
                Context.Response.AddHeader("content-disposition", "attachment; filename=" + flnm);
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                long FileSize = fs.Length;
                byte[] getContent = new byte[(int)FileSize];

                fs.Read(getContent, 0, (int)fs.Length);
                fs.Close();

                Context.Response.BinaryWrite(getContent);
                Context.Response.Flush();
                //Context.Response.Close();
                Context.Response.End();
            }
            catch (Exception ex) { throw (ex); }

        }
        #endregion

        public void SortAndFillListBox(DirectoryInfo dir, ListBox listBox)
        {
            List<ServerFiles> lst = new List<ServerFiles>();
            FileInfo[] files = dir.GetFiles();
            FileComparer myComparer = new FileComparer();
            Array.Sort(files, myComparer);
            StringCollection sc = new StringCollection();
            //  int iCount = 0;
            //foreach (FileInfo file in files)
            //{
            //    sc.Add(file.Name.Remove(file.Name.Length - file.Extension.Length, file.Extension.Length));
            //    DataRow Dr = dtblFiles.NewRow();
            //    Dr["FileName"] = file.Name.ToString();
            //    Dr["FilePath"] = file.FullName.ToString();

            //    dtblFiles.Rows.Add(Dr);
            //    iCount++;
            //    if (iCount > 19)
            //        break;
            //}
            //listBox.DataSource = sc;




            //grdDownLoad.DataSource = dtblFiles;
            //grdDownLoad.DataBind();

            //Coded by Subhrajyoti Chatterjee on 11/04/2015

            foreach (FileInfo file in files)
            {
                ServerFiles objServerFiles = new ServerFiles();
                objServerFiles.FileName = file.Name.ToString();
                objServerFiles.FilePath = file.FullName.ToString();
                lst.Add(objServerFiles);
            }
            grdDownLoad.DataSource = lst;
            grdDownLoad.DataBind();

            //Coded by Subhrajyoti Chatterjee on 11/04/2015

        }
        public class FileComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DateTime xCreationTime = ((FileInfo)x).CreationTime;
                DateTime yCreationTime = ((FileInfo)y).CreationTime;

                if (xCreationTime < yCreationTime)
                    return 1;
                else if (xCreationTime > yCreationTime)
                    return -1;
                else
                    return 0;
            }
        }


    }

    public class ServerFiles
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }



}