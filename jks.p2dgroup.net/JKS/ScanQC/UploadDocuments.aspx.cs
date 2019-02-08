using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;
using System.IO;

namespace NewLook
{
    public partial class NewLook_ScanQC_UploadDocuments : System.Web.UI.Page
    {
        #region Page Events
        DataCenter DC = null;
        bool test = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["test"].ToString());

        protected void Page_Init(object sender, EventArgs e)
        {
            ddlCompany.AutoPostBack = true;
            ddlCompany.SelectedIndexChanged += new EventHandler(ddlCompany_SelectedIndexChanged);

            //ddlBatchType.AutoPostBack = true;
            ddlBatchType.SelectedIndexChanged += new EventHandler(ddlBatchType_SelectedIndexChanged);

            btnUpload.Click += new EventHandler(btnUpload_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
            btnUploader.Click += new EventHandler(btnUploader_Click);

            btnUpload.OnClientClick = "ShowUploading();";
            btnUploader.OnClientClick = "ShowUploading();";

            if (test)
                btnUploader.CssClass = "GrayButton shown";
            else
                btnUploader.CssClass = "GrayButton hidden";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DC = new DataCenter(sqlConn);

            if (!IsPostBack)
            {
                int sComID = (int)Session["CompanyID"];
                DC.PopulateChildCompany(sComID, ddlCompany);

                string v = ddlCompany.Items[0].Value;
                ddlCompany.SelectedValue = v;
                ddlCompany_SelectedIndexChanged(sender, e);
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);
            DC.PopulateBatchType(CompanyID, ddlBatchType);
        }

        protected void ddlBatchType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (ddlBatchType.Items.Count == 0 || ddlBatchType.SelectedValue == "0")
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Please select a Batch Type.');", true);
                return;
            }

            if (!FileUpload1.HasFile && !FileUpload2.HasFile && !FileUpload3.HasFile && !FileUpload4.HasFile && !FileUpload5.HasFile)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('No file(s) selected to upload.');", true);
                return;
            }

            bool tf = true;

            if (tf)
                tf = IfFileExtensionsMatched(FileUpload1);
            if (tf)
                tf = IfFileExtensionsMatched(FileUpload2);
            if (tf)
                tf = IfFileExtensionsMatched(FileUpload3);
            if (tf)
                tf = IfFileExtensionsMatched(FileUpload4);
            if (tf)
                tf = IfFileExtensionsMatched(FileUpload5);

            if (!tf)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Please only upload pdf, tif, jpeg, png and zip files that contain only those formats.');", true);
            }
            else
            {
                tf = false;
                if (!tf)
                    tf = IfFileExtensionIsZIP(FileUpload1);
                if (!tf)
                    tf = IfFileExtensionIsZIP(FileUpload2);
                if (!tf)
                    tf = IfFileExtensionIsZIP(FileUpload3);
                if (!tf)
                    tf = IfFileExtensionIsZIP(FileUpload4);
                if (!tf)
                    tf = IfFileExtensionIsZIP(FileUpload5);

                if (tf)
                {
                    if (Session["ListOfFiles"] != null)
                        Session.Remove("ListOfFiles");

                    List<object> ListOfFiles = new List<object>();

                    SetFileSession(FileUpload1, ListOfFiles);
                    SetFileSession(FileUpload2, ListOfFiles);
                    SetFileSession(FileUpload3, ListOfFiles);
                    SetFileSession(FileUpload4, ListOfFiles);
                    SetFileSession(FileUpload5, ListOfFiles);

                    Session["ListOfFiles"] = ListOfFiles;

                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_conf", "ConfirmZip();", true);
                }
                else
                {
                    try
                    {
                        tf = true;
                        if (tf)
                            tf = FileUploader(FileUpload1);
                        if (tf)
                            tf = FileUploader(FileUpload2);
                        if (tf)
                            tf = FileUploader(FileUpload3);
                        if (tf)
                            tf = FileUploader(FileUpload4);
                        if (tf)
                            tf = FileUploader(FileUpload5);

                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Uploaded successfully.');", true);
                    }
                    catch (Exception ex)
                    {
                        string ss = ex.Message + " " + ex.Source + " " + ex.StackTrace + " " + ex.TargetSite + " " + ex.InnerException;
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + ss + ".');", true);
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScanQCDoc.aspx");
        }

        protected void btnUploader_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ListOfFiles"] != null)
                {
                    byte[] objFile = null;
                    string fileName = "";

                    List<object> ListOfFiles = (List<object>)Session["ListOfFiles"];

                    Session.Remove("ListOfFiles");

                    int i = 0;
                    bool tf = true;
                    List<bool> TFList = new List<bool>();
                    for (i = 0; i < ListOfFiles.Count; i = i + 2)
                    {
                        objFile = (byte[])ListOfFiles[i];
                        fileName = (string)ListOfFiles[i + 1];
                        tf = FileUploader(objFile, fileName);
                        TFList.Add(tf);
                    }

                    if (TFList[0] && TFList[1] && TFList[2] && TFList[3] && TFList[4])
                    {
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Uploaded successfully.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message + " " + ex.Source + " " + ex.StackTrace + " " + ex.TargetSite + " " + ex.InnerException;
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + ss + ".');", true);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checkes if file selected in FileUpload control has the extension mentioned in a list
        /// </summary>
        /// <param name="fuControl">FileUpload Control</param>
        /// <returns>True/False</returns>
        bool IfFileExtensionsMatched(FileUpload fuControl)
        {
            bool tf = false;

            if (string.IsNullOrEmpty(fuControl.FileName))
                return true;

            string[] arr = { "zip", "tif", "tiff", "jpeg", "jpg", "png", "pdf" };

            string fileNameWithExtension = Path.GetFileName(fuControl.FileName);
            string fileName = Path.GetFileNameWithoutExtension(fuControl.FileName);
            string ext = fileNameWithExtension.Replace(fileName + ".", "").ToLower();

            foreach (string str in arr)
            {
                if (str == ext)
                {
                    tf = true;
                    break;
                }
            }

            return tf;
        }

        /// <summary>
        /// checks if file selected in file upload control has zip file extension
        /// </summary>
        /// <param name="fuControl">FileUpload Control</param>
        /// <returns>True/Flase</returns>
        bool IfFileExtensionIsZIP(FileUpload fuControl)
        {
            bool tf = false;
            string str = "zip";

            string fileNameWithExtension = Path.GetFileName(fuControl.FileName);
            string fileName = Path.GetFileNameWithoutExtension(fuControl.FileName);
            string ext = fileNameWithExtension.Replace(fileName + ".", "");

            if (str == ext)
                tf = true;

            return tf;
        }

        /// <summary>
        /// Used to upload files
        /// </summary>
        /// <param name="fuControl">FileUpload Control</param>
        /// <returns>True/False</returns>
        protected bool FileUploader(FileUpload fuControl)
        {
            try
            {
                if (fuControl.HasFile)
                {
                    string strFileName = fuControl.FileName.TrimStart().TrimEnd();
                    string strReturn = "";

                    byte[] objFile = ReturnFileUploadContentAsBytes(fuControl);

                    string CompanyName = ddlCompany.SelectedItem.Text;
                    string CompanyID = ddlCompany.SelectedValue;
                    string BatchTypeID = ddlBatchType.SelectedValue;

                    string output_path = "C:\\P2D\\Email Processing\\" + CompanyName + "\\000;" + CompanyID + ";" + BatchTypeID + "\\" + strFileName;

                    //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + output_path + ".');", true);

                    if (test)
                    {
                        //string baseDirectory = Path.GetDirectoryName(output_path);
                        //if (!Directory.Exists(baseDirectory))
                        //    Directory.CreateDirectory(baseDirectory);
                        //System.IO.File.WriteAllBytes(output_path, objFile);

                        string serviceUrl = "http://localhost:2752/ReprocessingRequests/ReprocessingRequests.asmx";
                        CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                        objService.Url = serviceUrl;

                        string[] arr = output_path.Split('.');
                        int i = arr.Length - 1;
                        string ext = arr.GetValue(i).ToString();

                        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + ext + ".');", true);

                        if (ext.ToLower() == "zip")
                            strReturn = "unzip";

                        objService.UploadFile(output_path, objFile, strReturn);
                    }
                    else
                    {
                        string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ReprocessingRequestsWS"];
                        CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                        objService.Url = serviceUrl;

                        string[] arr = output_path.Split('.');
                        int i = arr.Length - 1;
                        string ext = arr.GetValue(i).ToString();

                        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + ext + ".');", true);

                        if (ext.ToLower() == "zip")
                            strReturn = "unzip";

                        objService.UploadFile(output_path, objFile, strReturn);

                        //Response.Write(strReturn);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + ss + ".');", true);

                return false;
            }
        }

        /// <summary>
        /// Overloaded method Used to upload files
        /// </summary>
        /// <param name="objFile">byte[] object of a file selected in file upload control</param>
        /// <param name="strFileName">File name as string</param>
        /// <returns>True/False</returns>
        protected bool FileUploader(byte[] objFile, string strFileName)
        {
            try
            {
                string strReturn = "";

                string CompanyName = ddlCompany.SelectedItem.Text;
                string CompanyID = ddlCompany.SelectedValue;
                string BatchTypeID = ddlBatchType.SelectedValue;

                string output_path = "C:\\P2D\\Email Processing\\" + CompanyName + "\\000;" + CompanyID + ";" + BatchTypeID + "\\" + strFileName;

                //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + output_path + ".');", true);

                if (test)
                {
                    //string baseDirectory = Path.GetDirectoryName(output_path);
                    //if (!Directory.Exists(baseDirectory))
                    //    Directory.CreateDirectory(baseDirectory);
                    //System.IO.File.WriteAllBytes(output_path, objFile);

                    string serviceUrl = "http://localhost:2752/ReprocessingRequests/ReprocessingRequests.asmx";
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = serviceUrl;

                    string[] arr = output_path.Split('.');
                    int i = arr.Length - 1;
                    string ext = arr.GetValue(i).ToString();

                    //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + ext + ".');", true);

                    if (ext.ToLower() == "zip")
                        strReturn = "unzip";

                    objService.UploadFile(output_path, objFile, strReturn);
                }
                else
                {
                    string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ReprocessingRequestsWS"];
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = serviceUrl;

                    string[] arr = output_path.Split('.');
                    int i = arr.Length - 1;
                    string ext = arr.GetValue(i).ToString();

                    //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + ext + ".');", true);

                    if (ext.ToLower() == "zip")
                        strReturn = "unzip";

                    objService.UploadFile(output_path, objFile, strReturn);

                    //Response.Write(strReturn);
                }

                return true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + ss + ".');", true);

                return false;
            }
        }

        /// <summary>
        /// Return the Return FileUpload Content as bytes
        /// </summary>
        /// <param name="fuControl">FileUpload Control</param>
        /// <returns>byte[]</returns>
        protected byte[] ReturnFileUploadContentAsBytes(FileUpload fuControl)
        {
            long FileLen = fuControl.PostedFile.ContentLength;
            byte[] objFile = new byte[FileLen];
            Stream oStream = fuControl.PostedFile.InputStream;
            oStream.Read(objFile, 0, (int)FileLen);

            return objFile;
        }

        /// <summary>
        /// Set session for accumulated file list
        /// </summary>
        /// <param name="fuControl">FileUpload Control</param>
        /// <param name="ListOfFiles">List of file as List<object></param>
        protected void SetFileSession(FileUpload fuControl, List<object> ListOfFiles)
        {
            byte[] objFile = null;
            string fileName = "";

            if (fuControl.HasFile)
            {
                objFile = ReturnFileUploadContentAsBytes(fuControl);
                fileName = fuControl.FileName;
            }

            ListOfFiles.Add(objFile);
            ListOfFiles.Add(fileName);
        }
        #endregion
    }

    #region DataCenter Class for all data access functions
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
        /// Loads Company list in a dropdown list
        /// </summary>
        /// <param name="ParentCompanyID">The parent company id as int.</param>
        /// <param name="ddlCompany">Company drop down list.</param>
        public void PopulateChildCompany(int ParentCompanyID, DropDownList ddlCompany)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "SELECT [CompanyID], [CompanyName] FROM [Company] WHERE [CompanyID] <> @ParentCompanyID AND [ParentCompanyID] = @ParentCompanyID ORDER BY [CompanyName];";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@ParentCompanyID", SqlDbType.Int).Value = ParentCompanyID;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("Company");
                DA.Fill(DT);

                ddlCompany.DataSource = DT;
                ddlCompany.DataValueField = DT.Columns[0].ToString();
                ddlCompany.DataTextField = DT.Columns[1].ToString();
                ddlCompany.DataBind();
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
                HttpContext.Current.Response.Write("<br />Error in PopulateChildCompany: " + ss);
            }
            finally
            {
                //ddlCompany.Items.Insert(0, new ListItem("Select Company", "0"));
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }

        /// <summary>
        /// Loads Batch Type list in a dropdown list
        /// </summary>
        /// <param name="CompanyID">Company ID as int</param>
        /// <param name="ddlBatchType">Batch Type drop down list</param>
        public void PopulateBatchType(int CompanyID, DropDownList ddlBatchType)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "SELECT [BatchTypeID], [BatchTypeName] FROM [ScanBatchTypes] WHERE [CompanyID] = @CompanyID;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@CompanyID", SqlDbType.Int).Value = CompanyID;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("ScanBatchTypes");
                DA.Fill(DT);

                ddlBatchType.DataSource = DT;
                ddlBatchType.DataValueField = DT.Columns[0].ToString();
                ddlBatchType.DataTextField = DT.Columns[1].ToString();
                ddlBatchType.DataBind();
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
                HttpContext.Current.Response.Write("<br />Error in PopulateBatchType: " + ss);
            }
            finally
            {
                ddlBatchType.Items.Insert(0, new ListItem("Select Batch Type", "0"));
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }
    }
    #endregion
}
