using System;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

namespace JKS
{
    /// <summary>
    /// Summary description for InvoiceScan.
    /// </summary>
    public class InvoiceScan : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region web controls
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.PlaceHolder MenuHolder;
        protected System.Web.UI.WebControls.Label lblMsg;
        protected System.Web.UI.WebControls.Button btnUp;
        protected System.Web.UI.WebControls.Button btnBack;
        protected System.Web.UI.HtmlControls.HtmlInputFile File1;
        protected System.Web.UI.HtmlControls.HtmlInputFile File2;
        protected System.Web.UI.HtmlControls.HtmlInputFile File3;
        protected System.Web.UI.HtmlControls.HtmlInputFile File4;
        protected System.Web.UI.HtmlControls.HtmlInputFile File5;
        protected System.Web.UI.WebControls.DropDownList ddlBatchTypes;
        #endregion
        #region sql variables
        SqlCommand sqlCmd;
        SqlConnection sqlConn;
        ArrayList arr;
        private String strBaseDirectory = ConfigurationManager.AppSettings["FilesLocation"].ToString();
        protected System.Web.UI.WebControls.Button btnUploadAll;

        string folderName = "";
        string CompanyName = "";
        string BatchNo = "";
        string BatchTypeID = "";
        string ScanDate = "";
        string ScanTime = "";
        string sSessionID = "";
        string sDBSessionID = "";
        string FName = "";
        protected System.Web.UI.WebControls.Label lbl1Msg;
        protected System.Web.UI.WebControls.Label lbl2Msg;
        protected System.Web.UI.WebControls.Label lbl3Msg;
        protected System.Web.UI.WebControls.Label lbl4Msg;
        protected System.Web.UI.WebControls.Label lbl5Msg;
        protected System.Web.UI.WebControls.DataGrid grdFile;

        protected string sFileName;
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            btnUploadAll.Attributes.Add("onclick", "return ValidateFormSubmission();");
            btnUp.Attributes.Add("onclick", "return ValidateFormSubmission();");
            // Put user code to initialize the page here 
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");
            baseUtil.keepAlive(this);

            if (!IsPostBack)
            {
                GetBatchTypes();
            }
        }
        #endregion
        #region private void GetBatchTypes()
        private void GetBatchTypes()
        {
            DataSet ds = new DataSet();
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            string strSql = "select BatchTypeID,BatchDocType from ScanBatchTypes where CompanyID= " + Convert.ToInt32(Session["CompanyID"]);
            SqlDataAdapter sqlDA = null;

            try
            {
                sqlDA = new SqlDataAdapter(strSql, sqlConn);
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            ddlBatchTypes.DataSource = ds.Tables[0];
            ddlBatchTypes.DataBind();
            ddlBatchTypes.DataValueField = "BatchTypeID";
            ddlBatchTypes.DataTextField = "BatchDocType";
        }
        #endregion
        #region private bool IsValidFile()
        private bool IsValidFile()
        {
            arr = new ArrayList();
            for (int i = 1; i <= 5; i++)
            {
                if (Path.GetFileName(Convert.ToString(((HtmlInputFile)FindControl("File" + i.ToString())).PostedFile.FileName)).Trim() != "")
                {
                    string sFileName = Convert.ToInt32(Request["InvoiceID"]) + "_" + Path.GetFileName(Convert.ToString(((HtmlInputFile)FindControl("File" + i.ToString())).PostedFile.FileName));
                    try
                    {
                        DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                        RecordSet rs = da.ExecuteQuery("vwUploadDocument", "InvoiceID=" + Convert.ToInt32(Request["InvoiceID"]) + " and ImagePath like '%" + sFileName + "%'");
                        while (!rs.EOF())
                        {
                            arr.Add(rs["ImagePath"]);
                            rs.MoveNext();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            if (arr.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
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
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            this.btnUploadAll.Click += new System.EventHandler(this.btnUploadAll_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        #region private void btnBack_Click(object sender, System.EventArgs e)
        private void btnBack_Click(object sender, System.EventArgs e)
        {
            if (Request["ReturnUrl"] != null && Request["ReturnUrl"] != "")
            {
                Response.Redirect(Request["ReturnUrl"]);
            }
        }
        #endregion
        #region btnUploadAll_Click
        private void btnUploadAll_Click(object sender, System.EventArgs e)
        {
            for (int i = 1; i <= 5; i++)
            {
                string sFileName1 = Convert.ToString(((HtmlInputFile)FindControl("File" + i.ToString())).PostedFile.FileName);
                if (sFileName != "")
                {
                    if (IsValidFile() == true)
                    {

                        if (strBaseDirectory.EndsWith("\\") == false)
                            strBaseDirectory += "\\";
                        string originialFileName = Path.GetFileName(sFileName1);
                        sFileName1 = strBaseDirectory + Path.GetFileName(sFileName1);
                        lblMsg.Visible = true;
                        try
                        {
                            FName = GenerateFoldername();
                            //							string strFolderPath ="D:/XML_Uploads/CSV Generic";		
                            string uploadedpath = Convert.ToString(ConfigurationManager.AppSettings["TestPath1"].ToString()) + "/" + CompanyName + "/" + FName;
                            if (checkfolderexists(uploadedpath))
                            {
                                ((HtmlInputFile)FindControl("File" + i.ToString())).PostedFile.SaveAs(uploadedpath + Path.GetFileName(sFileName1));
                                InsertScanDataIntoDB();
                                lblMsg.Text = originialFileName + " uploaded successfully.";
                            }
                        }
                        catch (Exception ex)
                        {
                            string ss = ex.Message.ToString();
                            lblMsg.Text = "Error in " + originialFileName + " file upload.";
                            lblMsg.Text = ex.Message.ToString();
                        }
                        finally
                        {
                            if (i == 1 && originialFileName != "")
                                lbl1Msg.Text = lblMsg.Text;
                            else if (i == 2 && originialFileName != "")
                                lbl2Msg.Text = lblMsg.Text;
                            else if (i == 3 && originialFileName != "")
                                lbl3Msg.Text = lblMsg.Text;
                            else if (i == 4 && originialFileName != "")
                                lbl4Msg.Text = lblMsg.Text;
                            else if (i == 5 && originialFileName != "")
                                lbl5Msg.Text = lblMsg.Text;
                            lblMsg.Text = "";
                        }
                    }
                }
            }
        }
        #endregion
        #region GenerateFoldername()
        public string GenerateFoldername()
        {
            #region sqlvariables
            DataSet ds = new DataSet();
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = null;
            #endregion

            #region Connection
            try
            {
                sqlDA = new SqlDataAdapter("sp_GetScanUploadFileDetails_Generic", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(Session["CompanyID"]));
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            #endregion

            try
            {
                if (ds.Tables.Count > 0)
                {
                    ScanDate = ds.Tables[0].Rows[0]["ScanDate"].ToString();
                    ScanTime = ds.Tables[0].Rows[0]["ScanTime"].ToString();
                    ScanTime = ScanTime.Replace(":", "");
                    ScanTime = DateTime.Now.Hour.ToString();
                    ScanTime = ScanTime + "0000";

                    sSessionID = Session.SessionID.ToString();
                    sDBSessionID = ds.Tables[0].Rows[0]["SessionID"].ToString();
                    BatchNo = ds.Tables[0].Rows[0]["BatchID"].ToString();
                    CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    string New_BatchTypeID = ds.Tables[0].Rows[0]["BatchTypeID"].ToString();


                    if (New_BatchTypeID != Convert.ToString(ddlBatchTypes.SelectedValue))
                    {
                        BatchTypeID = Convert.ToString(ddlBatchTypes.SelectedValue);
                        folderName = BatchNo + ";" + Convert.ToInt32(Session["CompanyID"]) + ";" + BatchTypeID + ";" + ScanDate + ";" + ScanTime + "\\";
                        ViewState["folderName"] = folderName;
                    }
                    else
                    {
                        folderName = Convert.ToString(ViewState["folderName"]);
                        BatchTypeID = New_BatchTypeID;
                    }
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }

            return folderName;
        }
        #endregion
        #region checkfolderexists
        bool checkfolderexists(string sFolder)
        {
            char[] delimiterChars = { '\\' };
            string[] sFolderSplit = sFolder.Split(delimiterChars);
            string strFolder = "";
            try
            {
                for (Int16 x = 0; x <= sFolderSplit.GetUpperBound(0) - 1; x++)
                {
                    strFolder += sFolderSplit[x] + "\\";
                    if (Directory.Exists(strFolder) == false)
                    {
                        Directory.CreateDirectory(strFolder);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region GetRecentBatchNo(string BatchNo)
        public string GetRecentBatchNo(string BatchNo)
        {
            string New_BatchNo = "";
            int length = 0;
            int BatchID = Convert.ToInt32(BatchNo);
            if (BatchID == 999)
                BatchID = 1;

            BatchID = BatchID + 1;
            length = BatchNo.Length;
            if (length > 0)
            {
                if (length == 1)
                    New_BatchNo = "00" + BatchNo;
                if (length == 2)
                    New_BatchNo = "0" + BatchNo;
                else
                    New_BatchNo = Convert.ToString(BatchID);
            }
            return New_BatchNo;
        }
        #endregion
        #region InsertScanDataIntoDB()
        public void InsertScanDataIntoDB()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            try
            {
                sqlCmd = new SqlCommand("sp_InsertScanDataIntoDB", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@CompanyID", Convert.ToInt32(Session["CompanyID"]));
                sqlCmd.Parameters.Add("@BatchNo", BatchNo);
                sqlCmd.Parameters.Add("@BatchTypeID", BatchTypeID);
                sqlCmd.Parameters.Add("@FolderName", folderName);
                sqlCmd.Parameters.Add("@SessionID", Session.SessionID.ToString());

                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }
        #endregion

        private void btnUp_Click(object sender, System.EventArgs e)
        {

        }

    }
}
