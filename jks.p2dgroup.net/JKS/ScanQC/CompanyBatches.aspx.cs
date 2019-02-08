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
using System.Configuration;

namespace NewLook
{
    public partial class NewLook_ScanQC_CompanyBatches : System.Web.UI.Page
    {
        public bool test = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["test"].ToString());
        SqlConnection sqlConn = null;
        string UserName = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            btnBack.Click += new EventHandler(btnBack_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = 0;

            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");
            else
                userID = (int)Session["UserID"];

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            UserName = ReturnUserName(userID);

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int companyID = Convert.ToInt32(Request.QueryString["id"]);
                    CompanyName(companyID);
                    PopulateGrid(companyID);
                }
                else
                {
                    Response.Redirect("ScanQCDoc.aspx", true);
                }

                #region DBCon
                SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].Trim());
                SqlCommand cmd = new SqlCommand("SP_getScanQCForUserID", con1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", Convert.ToInt32(Session["UserID"]));
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                if ((ds.Tables[0].Rows[0]["ScanQCView"] as object == DBNull.Value) || (Convert.ToString(ds.Tables[0].Rows[0]["ScanQCView"]).Equals("0")) || (string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["ScanQCView"]))))
                {
                    Session["type"] = "old";
                }
                else
                {
                    Session["type"] = "new";
                }
                #endregion
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScanQCDoc.aspx", true);
        }

        private void PopulateGrid(int companyID)
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

                //batches in query section
                dt = ds.Tables[0];

                gvBatchesInQuery.DataSource = dt;
                gvBatchesInQuery.DataBind();

                setFooterValues(gvBatchesInQuery);
                //batches in query section

                //batches in progress section
                dt = ds.Tables[1];

                gvBatchesInProgress.DataSource = dt;
                gvBatchesInProgress.DataBind();

                setFooterValues(gvBatchesInProgress);
                //batches in progress section
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();

                Response.Write(ss);
            }
            finally
            {
                dt.Dispose();
                ds.Dispose();
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }

        protected void setFooterValues(GridView gv)
        {
            if (gv.Rows.Count > 0)
            {
                int totBatchTotal = 0;
                int totProcessed = 0;
                int totDeleted = 0;
                int totInQuery = 0;

                foreach (GridViewRow GVR in gv.Rows)
                {
                    totBatchTotal += Convert.ToInt32(GVR.Cells[4].Text);
                    totProcessed += Convert.ToInt32(GVR.Cells[5].Text);
                    totDeleted += Convert.ToInt32(GVR.Cells[6].Text);
                    totInQuery += Convert.ToInt32(GVR.Cells[7].Text);
                }

                gv.FooterRow.Cells[4].Text = totBatchTotal.ToString();
                gv.FooterRow.Cells[5].Text = totProcessed.ToString();
                gv.FooterRow.Cells[6].Text = totDeleted.ToString();
                gv.FooterRow.Cells[7].Text = totInQuery.ToString();
            }
        }

        protected void lbtnDetails1_Click(object sender, EventArgs e)//Batches In Query
        {
            RedirectToDetailPage(sender);
        }

        protected void lbtnAction1_Click(object sender, EventArgs e)//Batches In Query
        {
            GridViewRow GVR = (GridViewRow)((LinkButton)sender).NamingContainer;
            Label lblBeingEdited = (Label)GVR.FindControl("lblBeingEdited");

            int InQueryCount = Convert.ToInt32(GVR.Cells[7].Text);
            int batchID = Convert.ToInt32(GVR.Cells[0].Text);
            int countBeingEdited = Convert.ToInt32(lblBeingEdited.Text);

            //if (countBeingEdited > 0)//&& !UserName.ToLower().Contains("p2d")
            //{
            //    this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Sorry, you cannot access batches currently being processed.')", true);
            //    return;
            //}

            if (InQueryCount <= 0)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Batch complete. Please load another batch.')", true);
            }
            else
            {
                SetBatchIDLilst(GVR);

                int docID = ReturnFirstAvailableDocumentID(batchID);

                if (docID > 0)
                {
                    int comID = Convert.ToInt32(Request.QueryString["id"]);

                    if (Session["IsSingleBatch"] != null)
                        Session.Remove("IsSingleBatch");

                    Session["IsSingleBatch"] = false;//is this single batch only?

                    if (Session["IsSingleDoc"] != null)
                        Session.Remove("IsSingleDoc");

                    Session["IsSingleDoc"] = false;//is this single invoice only?

                    string url = "ActionWindow.aspx?cid=" + comID + "&did=" + docID;
                    //Response.Redirect(url, true);

                    string browser = ReturnBrowser();
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_popUP", "PopupPage('" + url + "', screen.width, screen.height, '" + browser + "');", true);
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Sorry, you cannot access batches currently being processed.')", true);
                }
            }
        }

        protected void lbtnDetails2_Click(object sender, EventArgs e)//Batches In Progress
        {
            RedirectToDetailPage(sender);
        }

        protected void lbtnAction2_Click(object sender, EventArgs e)//Batches In Progress
        {
            GridViewRow GVR = (GridViewRow)((LinkButton)sender).NamingContainer;
            Label lblBeingEdited = (Label)GVR.FindControl("lblBeingEdited");

            int InProgressCount = Convert.ToInt32(GVR.Cells[7].Text);
            int batchID = Convert.ToInt32(GVR.Cells[0].Text);
            int countBeingEdited = Convert.ToInt32(lblBeingEdited.Text);

            //this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('" + UserName + ", " + countBeingEdited + "')", true);

            /*to prevent another user from accessing the same invoice at the same time countBeingEdited must be 0
            and check whether user does not contain p2d.*/
            if (!UserName.ToLower().Contains("p2d"))// || countBeingEdited > 0
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Sorry, you cannot access batches currently being processed.')", true);
                return;
            }

            if (InProgressCount <= 0)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Batch complete. Please load another batch.')", true);
            }
            else
            {
                SetBatchIDLilst(GVR);

                int docID = ReturnFirstAvailableDocumentID(batchID);

                if (docID > 0)
                {
                    int comID = Convert.ToInt32(Request.QueryString["id"]);

                    if (Session["IsSingleBatch"] != null)
                        Session.Remove("IsSingleBatch");

                    Session["IsSingleBatch"] = false;//is this single batch only?

                    if (Session["IsSingleDoc"] != null)
                        Session.Remove("IsSingleDoc");

                    Session["IsSingleDoc"] = false;//is this single invoice only?

                    string url = "ActionWindow.aspx?cid=" + comID + "&did=" + docID;
                    //Response.Redirect(url, true);

                    string browser = ReturnBrowser();
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_popUP", "PopupPage('" + url + "', screen.width, screen.height, '" + browser + "');", true);
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Sorry, you cannot access batches currently being processed.')", true);
                }
            }
        }

        private void RedirectToDetailPage(object sender)
        {
            string BatchType = "";

            GridViewRow GVR = (GridViewRow)((LinkButton)sender).NamingContainer;
            int i = GVR.RowIndex;
            GridView GridViewID = (GridView)GVR.NamingContainer;
            int batch_id = Convert.ToInt32(GridViewID.DataKeys[i].Values["BATCH ID"]);

            switch (GridViewID.ID.ToString())
            {
                case "gvBatchesInQuery":
                    BatchType = "inQuery";
                    break;
                case "gvBatchesInProgress":
                    BatchType = "inProgress";
                    break;
            }

            //This session is to take a value to SingleBatchDetails page,
            //to populate the Summary section.
            if (Session["BatchType"] != null)
                Session.Remove("BatchType");

            Session.Add("BatchType", BatchType);

            int companyID = Convert.ToInt32(Request.QueryString["id"]);

            Response.Redirect("SingleBatchDetails.aspx?id=" + companyID + "&batch_id=" + batch_id, true);
        }

        private string ReturnUserName(int userID)
        {
            string userName = "";

            if (userID > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    //string qry = "SELECT ISNULL([UserName], 0) FROM [Users] WHERE [UserID] = " + userID;
                    //sqlCmd = new SqlCommand(qry, sqlConn);
                    //sqlCmd.CommandType = CommandType.Text;

                    //sqlConn.Open();
                    //string x = sqlCmd.ExecuteScalar().ToString();
                    //userName = x;

                    string qry = "SELECT [UserName] FROM [Users] WHERE [UserID] = @UserID";
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

        private int ReturnFirstAvailableDocumentID(int batchID)
        {
            int firstDocID = 0;

            if (batchID > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    //string qry = "SELECT TOP(1) [DOC ID] FROM [DOCUMENT PROGRESS] WHERE (ISNULL([BEING EDITED], 'NO') = 'NO' OR [BEING EDITED] = '' OR [BEING EDITED] LIKE '% %') AND [FINAL ARCHIVING DATE] IS NULL AND [DELETE DATE] IS NULL AND  [BATCH ID] = @batchID ORDER BY [DOC ID] ASC;";
                    string qry = "SELECT TOP(1) [DOC ID] FROM [DOCUMENT PROGRESS] "+
                                " WHERE UPPER(ISNULL([BEING EDITED], 'NO')) <> 'YES' " +
                                " AND [FINAL ARCHIVING DATE] IS NULL AND [DELETE DATE] IS NULL "+
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

        private void SetBatchIDLilst(GridViewRow GVR)
        {
            GridView GV = (GridView)GVR.NamingContainer;

            List<int[]> BatchIDList = new List<int[]>();

            foreach (GridViewRow gvr in GV.Rows)
            {
                int inCount = Convert.ToInt32(gvr.Cells[7].Text);
                if (inCount > 0)
                {
                    int batchID = Convert.ToInt32(gvr.Cells[0].Text);
                    int docID = ReturnFirstAvailableDocumentID(batchID);
                    int[] arr = { batchID, docID };
                    BatchIDList.Add(arr);
                }
            }

            if (Session["IsSingleDoc"] != null)
                Session.Remove("IsSingleDoc");

            Session["IsSingleDoc"] = false;

            if (Session["BatchIDList"] != null)
                Session.Remove("BatchIDList");

            if (GV.Rows.Count > 0)
                Session["BatchIDList"] = BatchIDList;
            else
                Session["BatchIDList"] = null;
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