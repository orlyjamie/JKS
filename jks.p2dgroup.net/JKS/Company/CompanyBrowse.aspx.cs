using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;

namespace JKS
{
    /// <summary>
    /// Summary description for UserBrowse.
    /// </summary>
    public class CompanyBrowse : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.DataGrid grdCompany;
        protected System.Web.UI.WebControls.HyperLink Hyperlink2;
        protected System.Web.UI.WebControls.Panel Panel3;
        #endregion
        #region Variable Declaration
        private int iUserTypeID = 0;
        protected int iParentCompanyID = 0;
        #endregion
        #region Object Declaration
        private Company objCompany = new Company();
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");


            if (!Page.IsPostBack)
            {
                GetCompanyDetails();
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
            this.grdCompany.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdCompany_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        #region Datagrid_Click
        protected void Datagrid_Click(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DELETERECORD")
            {
                if (objCompany.DeleteCompanyRecord(Convert.ToInt32(e.CommandArgument)))
                {
                    lblMessage.Text = "Record(s) deleted successfully.";
                    GetCompanyDetails();
                }
                else
                {
                    lblMessage.Text = "Error deleting company. References exists.";
                }
            }
        }
        #endregion
        #region grdCompany_ItemDataBound
        private void grdCompany_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((ImageButton)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirm('Are you sure to delete this record ?')");
            }
        }
        #endregion
        #region GetCompanyDetails
        private void GetCompanyDetails()
        {
            RecordSet rs = null;
            Session["SelectedPage"] = "CompanyBrowse";

            iUserTypeID = Convert.ToInt32(Session["UserTypeID"]);
            iParentCompanyID = Convert.ToInt32(Session["CompanyID"]);

            if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
            {
                rs = Company.GetCompanyList();
            }
            else
            {
                rs = Company.GetSubCompanyList(iParentCompanyID);
            }

            grdCompany.DataSource = rs.ParentTable;
            grdCompany.DataBind();
        }
        #endregion
        #region CalculateExecutionTime
        private void CalculateExecutionTime(string strOption, DateTime dtStartTime, DateTime dtEndTime)
        {
            if (strOption.Equals("START"))
            {
                Response.Write("<BR> Start Time of Execution : " + dtStartTime.Hour + ":" + dtStartTime.Minute + ":" + dtStartTime.Second + ":" + dtStartTime.Millisecond);
            }
            else
            {
                Response.Write("<BR> End Time of Execution : " + dtEndTime.Hour + ":" + dtEndTime.Minute + ":" + dtEndTime.Second + ":" + dtEndTime.Millisecond);
                TimeSpan tsp = dtEndTime.Subtract(dtStartTime);
                Response.Write("<BR> Total Time of Execution : " + tsp.Hours + ":" + tsp.Minutes + ":" + tsp.Seconds + ":" + tsp.Milliseconds);
            }
        }
        #endregion
    }
}