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
    public partial class NewLook_ScanQC_SplitAndReprocess : System.Web.UI.Page
    {
        bool test = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["test"].ToString());
        DataCenter DC = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            ddlBatchType.AutoPostBack = true;
            ddlBatchType.SelectedIndexChanged += new EventHandler(ddlBatchType_SelectedIndexChanged);
            ddlCompany.AutoPostBack = true;
            ddlCompany.SelectedIndexChanged += new EventHandler(ddlCompany_SelectedIndexChanged);

            btnReProcess.OnClientClick = "return ReProcessValidation();";
            btnReProcess.Click += new EventHandler(btnReProcess_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            if (Request.QueryString.Count == 0)
                btnCancel_Click(sender, e);

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DC = new DataCenter(sqlConn);

            if (!IsPostBack)
            {
                int comID = Convert.ToInt32(Request.QueryString["cid"]);
                int docID = Convert.ToInt32(Request.QueryString["did"]);
                int sComID = (int)Session["CompanyID"];

                DC.PopulateChildCompany(sComID, ddlCompany);
                ddlCompany.SelectedValue = DC.ReturnClientCompanyID(docID).ToString();
                ddlCompany_SelectedIndexChanged(sender, e);
                ddlBatchType_SelectedIndexChanged(sender, e);
            }

            int c = 0;
            if (Session["pages"] != null)
                c = Convert.ToInt32(Session["pages"]);
            LoadCheckBoxes(c);
        }

        protected void ddlBatchType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            DC.PopulateBatchType(Convert.ToInt32(ddlCompany.SelectedValue), ddlBatchType);
            int docID = Convert.ToInt32(Request.QueryString["did"]);

            try
            {
                ddlBatchType.SelectedValue = DC.ReturnBatchTypeID(docID).ToString();
            }
            catch
            {
                ddlBatchType.SelectedValue = "0";
            }
        }

        protected void btnReProcess_Click(object sender, EventArgs e)
        {
            //Response.Write("<script>alert('Re Process function.');</script>");
            try
            {
                string docID = Request.QueryString["did"].ToString();

                string strFileName = "";
                byte[] objFile = null;
                string strReturn = "";

                int cid = Convert.ToInt32(Request.QueryString["cid"].ToString());
                string CompanyName = DC.ReturnCompanyNameByID(cid);

                #region image_path
                string OriginalName = "";
                string BatchID = "";
                string BatchName = "";

                DataTable dt = DC.ReturnTopDataTable(Convert.ToInt32(docID));
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    OriginalName = dr["ORIGINAL NAME"].ToString();
                    BatchID = dr["BATCH ID"].ToString();
                    BatchName = dr["BATCH NAME"].ToString();
                }

                string image_path = @"""C:\P2D\FTP Upload\" + CompanyName + @"\" + BatchName + @"\" + OriginalName + @"""";

                //Added for testing to check different server
                //string image_path2 = @"""\\90107-server3\FTP Archive\" + CompanyName + @"\" + BatchName + "_" + BatchID + @"\" + OriginalName + @"""";
                #endregion

                #region output_path
                CompanyName = ddlCompany.SelectedItem.Text;
                string CompanyID = ddlCompany.SelectedValue;
                string BatchTypeID = ddlBatchType.SelectedValue;

                string output_path = @"""C:\P2D\Email Processing\" + CompanyName + @"\000;" + CompanyID + ";" + BatchTypeID + @"""";
                #endregion

                #region page_numbers
                string page_numbers = "";
                int i = 1;
                foreach (Panel pnl in pnlSelectPages.Controls)
                {
                    CheckBox cb = pnl.Controls[1] as CheckBox;

                    if (cb.Checked)
                        page_numbers += i + ",";

                    i++;
                }
                i = page_numbers.LastIndexOf(',');
                page_numbers = @"""" + page_numbers.Remove(i, 1) + @"""";
                #endregion

                #region template
                string template = @"""{Title}-{Date}{Time}{MSec}.{Type}""";
                #endregion

                #region Create Temp File
                DateTime DT_Now = DateTime.Now;

                string strDate = DT_Now.ToString("ddMMyy");
                string strTime = DT_Now.ToString("hhmmss");

                string txtFileName = "DOC" + docID + "_" + strDate + "_" + strTime + ".txt";
                string TempFilePath = Server.MapPath("~") + "\\Temp\\" + txtFileName;
                string fileInnerText = "2tiff.exe -src " + image_path + " -dst " + output_path + " -options pages:" + page_numbers + " template:" + template + " -tiff bpp:1 compression:fax4";

                //Added for testing to check different server
                //fileInnerText += "\n2tiff.exe -src " + image_path2 + " -dst " + output_path + " -options pages:" + page_numbers + " template:" + template + " -tiff bpp:1 compression:fax4";
                
                File.WriteAllText(TempFilePath, fileInnerText);
                objFile = File.ReadAllBytes(TempFilePath);
                File.Delete(TempFilePath);
                #endregion

                if (test)
                {
                    strFileName = @"C:\P2D\Reprocessing Requests\" + txtFileName;
                    File.WriteAllBytes(strFileName, objFile);
                }
                else
                {
                    string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ReprocessingRequestsWS"];
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = serviceUrl;

                    strFileName = @"C:\P2D\Reprocessing Requests\" + txtFileName;
                    objService.UploadFile(strFileName, objFile, strReturn);

                    Response.Write(strReturn);
                }

                Response.Write("<script>alert('Re-process request successfully executed. If you have finished re-processing, please DELETE the document in the previous window.');</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('The re-process has failed. Please try again or contact support@p2dgroup.com to report the issue.');</script>");
                string ss = ex.Message + " " + ex.Source + " " + ex.StackTrace + " " + ex.TargetSite + " " + ex.InnerException;
                Response.Write("<script>alert('" + ss + "');</script>");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_close", "window.close();", true);
        }

        void LoadCheckBoxes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Panel pnl = new Panel();

                Label lbl = new Label();
                CheckBox cb = new CheckBox();

                cb.ID = "cb" + i;
                lbl.ID = "lbl" + i;
                lbl.Text = "Page" + (i + 1);

                pnl.ID = "pnl" + i;
                pnl.Controls.AddAt(0, lbl);
                pnl.Controls.AddAt(1, cb);

                pnlSelectPages.Controls.AddAt(i, pnl);
            }
        }
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

        /// <summary>
        /// Returns client company id, as int, from client batches table by doc id
        /// </summary>
        /// <param name="doc_id">document id as int.</param>
        /// <returns></returns>
        public int ReturnClientCompanyID(int doc_id)
        {
            int id = 0;

            if (doc_id > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    string qry = "SELECT [CLIENT ID] FROM [CLIENT BATCHES] WHERE [BATCH ID] IN(SELECT [BATCH ID] FROM [DOCUMENT PROGRESS] WHERE [DOC ID] = @doc_id);;";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@doc_id", SqlDbType.Int).Value = doc_id;
                    sqlConn.Open();
                    id = (int)sqlCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                    HttpContext.Current.Response.Write("<br />Error in ReturnClientCompanyID: " + ss);
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return id;
        }

        /// <summary>
        /// Returns batch type id, as int, from client batches table by doc id
        /// </summary>
        /// <param name="doc_id">document id as int.</param>
        /// <returns></returns>
        public int ReturnBatchTypeID(int doc_id)
        {
            int id = 0;

            if (doc_id > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    string qry = "SELECT [BATCH TYPE ID] FROM [CLIENT BATCHES] WHERE [BATCH ID] IN(SELECT [BATCH ID] FROM [DOCUMENT PROGRESS] WHERE [DOC ID] = @doc_id);";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@doc_id", SqlDbType.Int).Value = doc_id;
                    sqlConn.Open();
                    id = (int)sqlCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                    HttpContext.Current.Response.Write("<br />Error in ReturnBatchTypeID: " + ss);
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return id;
        }

        /// <summary>
        /// Returns DataTable for top section
        /// </summary>
        /// <param name="docID"></param>
        /// <returns>DataTable</returns>
        public DataTable ReturnTopDataTable(int docID)
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                sqlDA = new SqlDataAdapter("sp_return_scanQC_top_value", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("@doc_id", docID);

                sqlDA.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
                HttpContext.Current.Response.Write("<br />Error in ReturnTopDataTable: " + ss);
                return dt;
            }
            finally
            {
                dt.Dispose();
                sqlDA.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public string ReturnCompanyNameByID(int CompanyID)
        {
            string CompanyName = "";

            if (CompanyID > 0)
            {
                SqlCommand sqlCmd = new SqlCommand();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                DataTable DT = new DataTable();

                try
                {
                    string qry = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = @CompanyID;";

                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@CompanyID", SqlDbType.Int).Value = CompanyID;
                    sqlConn.Open();
                    //id = (int)sqlCmd.ExecuteScalar();

                    sqlDA = new SqlDataAdapter(sqlCmd);
                    sqlDA.Fill(DT);

                    if (DT.Rows.Count > 0)
                    {
                        DataRow DR = DT.Rows[0];
                        CompanyName = DR[0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                    HttpContext.Current.Response.Write("<br />Error in ReturnCompanyNameByID: " + ss);
                }
                finally
                {
                    sqlDA.Dispose();
                    DT.Dispose();
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return CompanyName;
        }
    }
    #endregion
}