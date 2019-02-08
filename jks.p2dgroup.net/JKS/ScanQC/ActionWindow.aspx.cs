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
using System.Configuration;
using System.Web.Services;
using System.Runtime.Remoting.Contexts;

namespace NewLook
{
    public partial class NewLook_ScanQC_ActionWindow : System.Web.UI.Page
    {
        public static int windowHeight = 0;
        public string docID = "0";


        //[WebMethod]
        //public static void checkViewType(string userId)
        //{
        //    //string userId = Convert.ToString(userId);
        //    #region DBCon
        //    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].Trim());
        //    SqlCommand cmd = new SqlCommand("SP_getScanQCForUserID", con1);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@userID", Convert.ToInt32(userId));
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    HttpContext context = HttpContext.Current;
        //    context.Response.Clear();
        //    context.Response.Write(Convert.ToString(ds.Tables[0].Rows[0]["ScanQCView"]));
        //    context.Response.End();
        //    #endregion
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count == 0)
            {
                if (Request.UrlReferrer == null)
                    Response.Redirect("ScanQCDoc.aspx", true);
                else if (Request.UrlReferrer.PathAndQuery.Contains("ActionScanQC") == false)
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_close", "window.close();", true);
            }
            else
            {
                if (Session["UserID"] == null)
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_close", "window.close();", true);

                docID = Request.QueryString["did"];

                if (!IsPostBack)
                    FetchQueryStringValue();
            }
        }

        public void FetchQueryStringValue()
        {
            string comID = Request.QueryString["cid"];
            string docID = Request.QueryString["did"];
            string winH = Request.QueryString["winH"];

            string userId = Convert.ToString(Session["UserID"]);
            #region DBCon
            SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].Trim());
            SqlCommand cmd = new SqlCommand("SP_getScanQCForUserID", con1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userID", Convert.ToInt32(userId));
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            #endregion

            string queryString = "?cid=" + comID + "&did=" + docID + "&winH=" + winH;

            string TiffViewerUrl = string.Empty;
            string ActionWindowUrl = string.Empty;
            if (string.Equals(Convert.ToString(Session["type"]), "old"))
            {
                ActionWindowUrl = ("ActionScanQC.aspx" + queryString);
                TiffViewerUrl = "TiffViewerScanQC.aspx" + queryString;
                TiffWindow.Attributes.Add("class", "left");
                ActionWindow.Attributes.Add("class", "right");
                TiffWindow.Attributes.CssStyle.Remove("width");
                ActionWindow.Attributes.CssStyle.Remove("width");
            }
            else
            {
                ActionWindowUrl = ("ActionScanQCNew.aspx" + queryString);
                TiffViewerUrl = "TiffViewerScanQCNew.aspx" + queryString;
                TiffWindow.Attributes.Remove("class");
                ActionWindow.Attributes.Remove("class");
                TiffWindow.Attributes.CssStyle.Add("width", "100%");
                ActionWindow.Attributes.CssStyle.Add("width", "100%");
                //TiffWindow.Attributes.Add("class", "tiffHeight");
                ActionWindow.Attributes.Add("class", "actionHeight");
                //TiffWindow.Attributes.CssStyle.Remove("height");
                //windowHeight = Convert.ToInt32(Request.QueryString["winH"]) - 300;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "mySetHeight();", true);
            }
            //string ActionWindowUrl = "ActionScanQC.aspx" + queryString;

            TiffWindow.Attributes.Add("src", TiffViewerUrl);
            ActionWindow.Attributes.Add("src", ActionWindowUrl);
        }

        [System.Web.Services.WebMethod]
        public static string PageUnload(string docID)
        {
            string ret = "";
            string isNext = (HttpContext.Current.Session["IsNextDoc"] != null) ? HttpContext.Current.Session["IsNextDoc"].ToString() : "False";

            docID = string.IsNullOrEmpty(docID) ? "0" : docID;

            try
            {
                SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                DataCenter DC = new DataCenter(sqlConn);

                bool tf = DC.UpdateBeingEdited("NO", Convert.ToInt32(docID));

                if (tf)
                {
                    #region Set BEING EDITED COUNT value to 0
                    //Session of SingleRowTable is updated with newly edited table

                    DataTable DT = new DataTable();
                    string colName = "BEING EDITED COUNT";

                    if (HttpContext.Current.Session["SingleRowTable"] != null)
                        DT = (DataTable)HttpContext.Current.Session["SingleRowTable"];

                    for (int i = 0; i < DT.Columns.Count; i++)
                    {
                        if (DT.Columns[i].ColumnName == colName)
                        {
                            DT.Rows[0][i] = 0;
                            break;
                        }
                    }

                    if (HttpContext.Current.Session["SingleRowTable"] != null)
                        HttpContext.Current.Session.Remove("SingleRowTable");

                    HttpContext.Current.Session["SingleRowTable"] = DT;

                    if (isNext == false.ToString())
                    {
                        if (HttpContext.Current.Session["BatchIDList"] != null)
                            HttpContext.Current.Session.Remove("BatchIDList");

                        if (HttpContext.Current.Session["IsSingleDoc"] != null)
                            HttpContext.Current.Session.Remove("IsSingleDoc");

                        if (HttpContext.Current.Session["IsSingleBatch"] != null)
                            HttpContext.Current.Session.Remove("IsSingleBatch");

                        if (HttpContext.Current.Session["IsNextDoc"] != null)
                            HttpContext.Current.Session.Remove("IsNextDoc");
                    }
                    #endregion
                }

                ret = "True";
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

            return ret;
        }

        //protected void Page_UnLoad(object sender, EventArgs e)
        //{
        //    string docID = Request.QueryString["did"];
        //    string x = Session["IsNextDoc"].ToString();
        //    bool tf = Convert.ToBoolean(x);

        //    if (tf)
        //    {
        //        docID = string.IsNullOrEmpty(docID) ? "0" : docID;

        //        try
        //        {
        //            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
        //            DataCenter DC = new DataCenter(sqlConn);

        //            tf = DC.UpdateBeingEdited("NO", Convert.ToInt32(docID));

        //            if (tf)
        //            {
        //                if (Session["BatchIDList"] != null)
        //                    Session.Remove("BatchIDList");

        //                if (Session["IsSingleDoc"] != null)
        //                    Session.Remove("IsSingleDoc");

        //                #region Set BEING EDITED COUNT value to 0
        //                //Session of SingleRowTable is updated with newly edited table

        //                DataTable DT = new DataTable();
        //                string colName = "BEING EDITED COUNT";

        //                if (Session["SingleRowTable"] != null)
        //                    DT = (DataTable)Session["SingleRowTable"];

        //                for (int i = 0; i < DT.Columns.Count; i++)
        //                {
        //                    if (DT.Columns[i].ColumnName == colName)
        //                    {
        //                        DT.Rows[0][i] = 0;
        //                        break;
        //                    }
        //                }

        //                if (Session["SingleRowTable"] != null)
        //                    Session.Remove("SingleRowTable");

        //                Session["SingleRowTable"] = DT;
        //                #endregion
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string ss = ex.Message;
        //        }
        //    }
        //    else
        //    {
        //    }
        //}
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
        /// Update being edited value as per the doc id in [document progress] table.
        /// </summary>
        /// <param name="status">Status 'Yes' or 'NO' as string.</param>
        /// <param name="docID">Document ID as int.</param>
        /// <returns></returns>
        public bool UpdateBeingEdited(string status, int docID)
        {
            bool TF = false;
            SqlCommand sqlCmd = null;

            try
            {
                string qry = "UPDATE [DOCUMENT PROGRESS] SET [BEING EDITED] = '' + @input1 + '' WHERE [DOC ID] = @input2;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input1", SqlDbType.VarChar).Value = status.Trim();
                sqlCmd.Parameters.AddWithValue("@input2", SqlDbType.Int).Value = docID;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                TF = true;
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

            return TF;
        }
    }
}