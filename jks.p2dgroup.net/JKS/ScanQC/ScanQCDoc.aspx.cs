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
using System.Drawing;

namespace NewLook
{
    public partial class NewLook_ScanQC_ScanQCDoc : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            gvScanQCDocs.RowDataBound += new GridViewRowEventHandler(gvScanQCDocs_RowDataBound);
            gvScanQCDocs.SelectedIndexChanged += new EventHandler(gvScanQCDocs_SelectedIndexChanged);
            btnUploadFiles.Click += new EventHandler(btnUploadFiles_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            if (!IsPostBack)
            {
                int companyID = (int)Session["CompanyID"];
                //Response.Write(companyID);

                int UserTypeID = (int)Session["UserTypeID"];
                //Response.Write(UserTypeID);

                int UserID = (int)Session["UserID"];
                //Response.Write(UserID);

                PopulateGrid(companyID, UserID);
            }

            //foreach (string str in Session.Contents)
            //{
            //    Response.Write("<br />" + str);
            //}
        }

        private void PopulateGrid(int companyID, int UserID)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_return_ScanQCDoc", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", companyID);
            sqlDA.SelectCommand.Parameters.Add("@UserID", UserID);
            DataTable dt = new DataTable();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(dt);

                gvScanQCDocs.DataSource = dt;
                gvScanQCDocs.DataBind();
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }

        protected void gvScanQCDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = gvScanQCDocs.SelectedIndex;

            string id = gvScanQCDocs.DataKeys[i].Values["CompanyID"].ToString();
            Response.Redirect("CompanyBatches.aspx?id=" + id, true);
            //this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('" + i + "')", true);
        }

        protected void gvScanQCDocs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvScanQCDocs, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select this row.";
            }
        }

        protected void btnUploadFiles_Click(object sender, EventArgs e)
        {
            Response.Redirect("UploadDocuments.aspx", true);
        }
    }
}