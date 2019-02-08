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
using CBSolutions.Architecture.Data;
using CBSolutions.ETH.Web;

namespace JKS
{
    /// <summary>
    /// Summary description for UserBrowse.
    /// </summary>
    public class UserCompanyBrowse : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Label Label1;
        protected System.Web.UI.WebControls.Panel Panel1;
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Label Label2;
        protected System.Web.UI.WebControls.DataGrid dgUsers;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.Label lblSelectCompany;
        protected System.Web.UI.WebControls.DropDownList ddlCompany;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdHubAdminFlag;
        #endregion
        #region User Defined Variables
        private Users objUsers = new Users();
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            if (!Page.IsPostBack)
            {
                if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                {
                    MakeDropDownVisible();
                }
                else
                {
                    GetSupplierCompanyUsersByCompanyID(Convert.ToInt32(Session["CompanyID"]));
                }
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
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
            this.dgUsers.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUsers_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        #region GetSupplierCompanyUsersByCompanyID
        private void GetSupplierCompanyUsersByCompanyID(int iSupplierCompanyID)
        {
            DataTable dtbl = new DataTable();

            dtbl = objUsers.GetSupplierCompanyUsersByCompanyID(iSupplierCompanyID);

            dgUsers.DataSource = dtbl;
            dgUsers.DataBind();

            if (dgUsers.Items.Count > 0)
            {
                dgUsers.Visible = true;
                lblMessage.Text = "";
                Label2.Visible = true;
            }
            else
            {
                lblMessage.Text = "Sorry, no record(s) found.";
                dgUsers.Visible = false;
                Label2.Visible = true;
            }
        }
        #endregion
        #region Datagrid_Click
        protected void Datagrid_Click(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DELETERECORD")
            {
                if (objUsers.DeleteSupplierUserRecord(Convert.ToInt32(e.CommandArgument)))
                {
                    lblMessage.Text = "Record(s) deleted successfully.";

                    if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                        GetSupplierCompanyUsersByCompanyID(Convert.ToInt32(Session["SelectedCompanyID"]));
                    else
                        GetSupplierCompanyUsersByCompanyID(Convert.ToInt32(Session["CompanyID"]));
                }
                else
                {
                    lblMessage.Text = "Error deleting company. References exists.";
                }
            }
        }
        #endregion
        #region dgUsers_ItemDataBound
        private void dgUsers_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((ImageButton)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirm('Are you sure to delete this record ?')");
            }
        }
        #endregion
        #region MakeDropDownVisible
        private void MakeDropDownVisible()
        {
            GetCompanyListForHubAdmin();
            ddlCompany.Visible = true;
            lblSelectCompany.Visible = true;
            hdHubAdminFlag.Value = "1";
        }
        #endregion
        #region GetCompanyListForHubAdmin
        private void GetCompanyListForHubAdmin()
        {
            ddlCompany.DataSource = objUsers.GetCompanyListForHubAdmin();
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("Select Company", "0"));
        }
        #endregion
        #region ddlCompany_SelectedIndexChanged
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GetSupplierCompanyUsersByCompanyID(Convert.ToInt32(ddlCompany.SelectedValue));

            if (ddlCompany.SelectedIndex != 0)
            {
                Session["SelectedCompanyID"] = ddlCompany.SelectedValue.Trim();
                Session["SelectedCompanyTypeID"] = objUsers.GetCompanyType(Convert.ToInt32(ddlCompany.SelectedValue));
            }
        }
        #endregion
    }
}