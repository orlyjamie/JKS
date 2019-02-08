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
using System.Globalization;

namespace NewLook
{
    public partial class NewLook_ScanQC_SingleBatchDetails : System.Web.UI.Page
    {
        public bool test = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["test"].ToString());
        SqlConnection sqlConn = null;
        string UserName = "";
        int SeletedRowIndex = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            btnBack.Click += new EventHandler(btnBack_Click);
            gvSummary.RowDataBound += new GridViewRowEventHandler(gvSummary_RowDataBound);
            gvSummary.SelectedIndexChanged += new EventHandler(gvSummary_SelectedIndexChanged);
            gvDetails.RowDataBound += new GridViewRowEventHandler(gvDetails_RowDataBound);
            gvDetails.SelectedIndexChanged += new EventHandler(gvDetails_SelectedIndexChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = 0;

            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");
            else
                userID = (int)Session["UserID"];

            if (Session["SelectedBatchRow"] != null)
                Session.Remove("SelectedBatchRow");

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            UserName = ReturnUserName(userID);

            if (!IsPostBack)
            {
                if (Request.QueryString.Count == 0)
                {
                    Response.Redirect("CompanyBatches.aspx", true);
                    return;
                }

                if (Request.QueryString["id"] != null && Request.QueryString["batch_id"] != null)
                {
                    int companyID = Convert.ToInt32(Request.QueryString["id"]);
                    int batchID = Convert.ToInt32(Request.QueryString["batch_id"]);

                    CompanyName(companyID);
                    PopulateSummaryGrid(companyID, batchID);                    
                    PopulateDetailsGrid(batchID);
                }
            }
        }

        private void CompanyName(int companyID)
        {
            SqlCommand sqlCmd = null;
            try
            {
                string qry = "SELECT ISNULL([CompanyName], 0) FROM [Company] WHERE [CompanyID] = " + companyID;
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;

                sqlConn.Open();
                string x = sqlCmd.ExecuteScalar().ToString();
                lblCompanyName.Text = x;
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
                lblMsg.Text = "Error: " + ex.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }

        protected void lbtnAction1_Click(object sender, EventArgs e)//Summary Section
        {
            GridViewRow GVR;

            if (sender is GridView)
                GVR = ((GridView)sender).Rows[SeletedRowIndex];
            else
                GVR = (GridViewRow)((LinkButton)sender).NamingContainer;

            Label lblBeingEdited = (Label)GVR.FindControl("lblBeingEdited");

            int InCount = Convert.ToInt32(GVR.Cells[7].Text);
            int batchID = Convert.ToInt32(GVR.Cells[0].Text);
            int countBeingEdited = 0;

            if (!test)
            {
                if (gvSummary.HeaderRow.Cells[7].Text.ToLower() == "in progress")
                    countBeingEdited = Convert.ToInt32(lblBeingEdited.Text);

                if (countBeingEdited > 0 && !UserName.Contains("P2D"))
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Sorry, you cannot access batches currently being processed.')", true);
                    return;
                }
            }
            else
            {
                countBeingEdited = Convert.ToInt32(lblBeingEdited.Text);

                if (countBeingEdited > 0)
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Sorry, you cannot access batches currently being processed.')", true);
                    return;
                }
            }

            if (InCount <= 0)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Batch complete. Please load another batch.')", true);
            }
            else
            {
                /*g.If the batch is accessed from the details page then the window should close after processing only 
                 * that batch, and not move onto the next batch in the list. Thus SetbatchIDLilst is disabled. */
                //SetBatchIDLilst(GVR);
                SetSingleRowTableSession();

                if (Session["IsSingleBatch"] != null)
                    Session.Remove("IsSingleBatch");

                Session["IsSingleBatch"] = true;//is this single batch only?

                if (Session["IsSingleDoc"] != null)
                    Session.Remove("IsSingleDoc");

                Session["IsSingleDoc"] = false;//is this single invoice only?

                int docID = ReturnFirstAvailableDocumentID(batchID);//Convert.ToInt32(gvDetails.Rows[0].Cells[0].Text);

                if (docID > 0)
                {
                    int comID = Convert.ToInt32(Request.QueryString["id"]);

                    string url = "ActionWindow.aspx?cid=" + comID + "&did=" + docID;

                    //Response.Redirect(url, true);

                    string browser = ReturnBrowser();
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_popUP", "PopupPage('" + url + "', screen.width, screen.height, '" + browser + "');", true);
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('No document is available to Action.')", true);
                }
            }
        }

        protected void lbtnAction2_Click(object sender, EventArgs e)//Details Section
        {
            int batchID = Convert.ToInt32(gvSummary.Rows[0].Cells[0].Text);
            SetBeingEdited(batchID);

            GridViewRow GVR;

            if (sender is GridView)
                GVR = ((GridView)sender).Rows[SeletedRowIndex];
            else
                GVR = (GridViewRow)((LinkButton)sender).NamingContainer;

            //previously checking was for test is false after it was edited to allow this feature even if test is true.
            if (!test || test)
            {
                string status = GVR.Cells[2].Text.Trim();

                if (status.ToLower() != "pending")
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('This invoice has already been processed.');", true);
                    return;
                }
            }

            string processing = GVR.Cells[3].Text.Trim();

            if (processing.ToLower() == "yes")
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Sorry, you cannot access batches currently being processed.');", true);
                return;
            }

            if (Session["IsSingleBatch"] != null)
                Session.Remove("IsSingleBatch");

            Session["IsSingleBatch"] = true;//is this single batch only?

            if (Session["IsSingleDoc"] != null)
                Session.Remove("IsSingleDoc");

            Session["IsSingleDoc"] = true;//is this single invoice only?

            string docID = GVR.Cells[0].Text;
            int comID = Convert.ToInt32(Request.QueryString["id"]);

            string url = "ActionWindow.aspx?cid=" + comID + "&did=" + docID;

            SetSingleRowTableSession();

            //Response.Redirect(url, true);

            string browser = ReturnBrowser();
            this.ClientScript.RegisterStartupScript(this.GetType(), "_popUP", "PopupPage('" + url + "', screen.width, screen.height, '" + browser + "');", true);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            int companyID = Convert.ToInt32(Request.QueryString["id"]);
            Response.Redirect("CompanyBatches.aspx?id=" + companyID, true);
        }

        #region Populate Summary Grid View
        private void PopulateSummaryGrid(int companyID, int batchID)
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_company_batches", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", companyID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);

                string BatchType = "";

                if (Session["BatchType"] != null)
                    BatchType = Session["BatchType"].ToString();
                else
                    btnBack_Click(new object(), new EventArgs());

                switch (BatchType)
                {
                    case "inQuery":
                        //batches in query section
                        dt = ds.Tables[0];
                        break;
                    case "inProgress":
                        //batches in progress section
                        dt = ds.Tables[1];
                        break;
                }

                int i = dt.Columns.Count - 1;//last column's index

                string colName = dt.Columns[i].ColumnName;
                //Changing the column name of data-table to show in grid view
                //as column name will be different when it is received i.e. InProgress or InQuery
                //so maiking it similar to population purpose.
                dt.Columns[i].ColumnName = "IN VALUE";

                dt = dt.Select(" [BATCH ID] = " + batchID).CopyToDataTable();

                gvSummary.DataSource = dt;
                gvSummary.DataBind();

                //setting the Header name as per the column name.

                switch (colName.ToUpper())
                {
                    case "IN QUERY":
                        colName = "In Query";
                        break;
                    case "IN PROGRESS":
                        colName = "In Progress";
                        break;
                }
                
                gvSummary.HeaderRow.Cells[7].Text = colName;  
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
                ds.Dispose();
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }
        #endregion

        #region Populate Details Grid View
        private void PopulateDetailsGrid(int batchID)
        {            
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_return_ScanQC_Details", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.AddWithValue("@BatchID", batchID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);

                dt = ds.Tables[0];

                gvDetails.DataSource = dt;
                gvDetails.DataBind();
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
                ds.Dispose();
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }
        #endregion

        private int ReturnFirstAvailableDocumentID(int batchID)
        {
            int firstDocID = 0;

            if (batchID > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    //select only pending document
                    //string qry = "SELECT TOP(1) [DOC ID] FROM [DOCUMENT PROGRESS] WHERE (ISNULL([BEING EDITED], 'NO') = 'NO' OR [BEING EDITED] = '' OR [BEING EDITED] LIKE '% %') AND [FINAL ARCHIVING DATE] IS NULL AND [DELETE DATE] IS NULL AND [BATCH ID] = @batchID ORDER BY [DOC ID] ASC;";
                    string qry = "SELECT TOP(1) [DOC ID] FROM [DOCUMENT PROGRESS] " +
                                " WHERE UPPER(ISNULL([BEING EDITED], 'NO')) <> 'YES' " +
                                " AND [FINAL ARCHIVING DATE] IS NULL AND [DELETE DATE] IS NULL " +
                                " AND [BATCH ID] = @batchID ORDER BY [DOC ID] ASC; ";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.Add("@batchID", SqlDbType.Int).Value = batchID;
                    sqlConn.Open();
                    firstDocID = (int)sqlCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return firstDocID;
        }

        private bool IsBatchInProgres(int batchID)
        {
            bool tf = false;

            if (batchID > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    string qry = "SELECT COUNT([DOC ID]) FROM [DOCUMENT PROGRESS] WHERE [BEING EDITED] = 'YES' AND [BATCH ID] = @batchID;";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.Add("@batchID", SqlDbType.Int).Value = batchID;
                    sqlConn.Open();
                    int c = (int)sqlCmd.ExecuteScalar();

                    if (c > 0)
                        tf = true;
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return tf;
        }

        private string ReturnUserName(int userID)
        {
            string userName = "";

            if (userID > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    string qry = "SELECT [UserName] FROM [Users] WHERE [UserID] = @UserID;";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                    sqlConn.Open();
                    userName = sqlCmd.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return userName;
        }

        private void SetBatchIDLilst(GridViewRow GVR)
        {
            if (Session["BatchIDList"] != null)
                Session.Remove("BatchIDList");

            GridView GV = (GridView)GVR.NamingContainer;

            List<int[]> BatchIDList = new List<int[]>();

            foreach (GridViewRow gvr in GV.Rows)
            {
                int batchID = Convert.ToInt32(gvr.Cells[0].Text);
                int docID = ReturnFirstAvailableDocumentID(batchID);
                int[] arr = { batchID, docID };
                BatchIDList.Add(arr);
            }

            if (GV.Rows.Count > 0)
                Session["BatchIDList"] = BatchIDList;
            else
                Session["BatchIDList"] = null;
        }

        #region Set BEING EDITED COUNT value to 1
        private void SetSingleRowTableSession()
        {
            //Session of SingleRowTable is updated with newly edited table

            DataTable DT = new DataTable();
            string colName = "BEING EDITED COUNT";

            if (Session["SingleRowTable"] != null)
                DT = (DataTable)Session["SingleRowTable"];

            for (int i = 0; i < DT.Columns.Count; i++)
            {
                if (DT.Columns[i].ColumnName == colName)
                {
                    DT.Rows[0][i] = 1;
                    break;
                }
            }

            Session.Remove("SingleRowTable");

            Session["SingleRowTable"] = DT;
        }
        #endregion       

        #region Take to action window when a row is clicked.
        protected void gvSummary_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeletedRowIndex = gvSummary.SelectedIndex;

            lbtnAction1_Click(sender, e);

            //this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('" + SeletedRowIndex + "')", true);
        }

        protected void gvSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvSummary, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to Action this Batch.";
            }
        }

        protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeletedRowIndex = gvDetails.SelectedIndex;

            lbtnAction2_Click(sender, e);

            //this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('" + SeletedRowIndex + "')", true);           
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvDetails, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to Action this document.";

                if (e.Row.Cells[2].Text.ToLower() == "pending")
                {
                    e.Row.Style.Add("border", "2px solid #000000");
                    e.Row.Style.Add("background-color", "#ff0000");

                    int i = 0;
                    for (i = 0; i < e.Row.Cells.Count; i++)
                        e.Row.Cells[i].Style.Add("color", "white");

                    LinkButton lbtnAction = (LinkButton)e.Row.Cells[i - 1].FindControl("lbtnAction");
                    lbtnAction.Style.Add("color", "white");
                }
            }
        }
        #endregion

        private void SetBeingEdited(int batchID)
        {
            if (batchID > 0)
            {
                SqlCommand sqlCmd = null;
                SqlDataAdapter DA = new SqlDataAdapter();
                DataTable DT = new DataTable();
                try
                {
                    //select only pending document
                    string qry = "SELECT [DOC ID],[BEING EDITED] FROM [DOCUMENT PROGRESS] WHERE [BATCH ID] = @batchID;";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.Add("@batchID", SqlDbType.Int).Value = batchID;
                    sqlConn.Open();
                    DA = new SqlDataAdapter(sqlCmd);
                    DA.Fill(DT);
                    int i = 0;
                    foreach (GridViewRow GVR in gvDetails.Rows)
                    {
                        string xDocID = GVR.Cells[0].Text;
                        string yDocID = DT.Rows[GVR.RowIndex][0].ToString();
                        if (xDocID == yDocID)
                            GVR.Cells[3].Text = DT.Rows[GVR.RowIndex][1].ToString();
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
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

        string ReturnBrowser()
        {
            string browser = Request.Browser.Type.ToUpper();

            if (browser.Contains("IE") || browser.Contains("INTERNETEXPLORER"))
                browser = "IE";
            if (browser.Contains("FIREFOX"))
                browser = "FF";
            if (browser.Contains("CHROME"))
                browser = "CH";

            return browser;
        }
    }
}