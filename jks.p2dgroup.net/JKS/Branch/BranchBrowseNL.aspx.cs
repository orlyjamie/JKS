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

namespace JKS
{
    /// <summary>
    /// Summary description for BranchBrowseNL.
    /// </summary>
    public class BranchBrowseNL : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.ImageButton imgBtnAddBranch;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.DropDownList ddlCompany;
        protected System.Web.UI.WebControls.DataGrid grdBranch;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdCompanyID;
        #endregion

        #region User defined Variable
        private Company objCompany = new Company();
        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            Session["SelectedPage"] = "BranchManagement";

            if (!Page.IsPostBack)
            {
                if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                {
                    GetCompanyListForBranch();
                    GetBranchNameCompanyWiseForHubAdmin(14);
                }
                else
                {
                    RecordSet rs = BranchNL.GetBranchList(System.Convert.ToInt32(Session["CompanyID"]));

                    grdBranch.DataSource = rs.ParentTable;
                    grdBranch.DataBind();
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
            this.imgBtnAddBranch.Click += new System.Web.UI.ImageClickEventHandler(this.imgBtnAddBranch_Click);
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
            this.grdBranch.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdBranch_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region ddlCompany_SelectedIndexChanged
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlCompany.SelectedIndex != 0)
            {
                GetBranchNameCompanyWiseForHubAdmin(Convert.ToInt32(ddlCompany.SelectedValue));
                hdCompanyID.Value = ddlCompany.SelectedValue.Trim();
            }
            else
            {
                hdCompanyID.Value = "0";
            }
        }
        #endregion
        #region GetCompanyListForBranch
        private void GetCompanyListForBranch()
        {
            ddlCompany.DataSource = objCompany.GetCompanyListForBranch();
            ddlCompany.DataBind();

            ddlCompany.Items.Insert(0, new ListItem("Select Company", "0"));

            try
            {
                ddlCompany.SelectedValue = "14";

            }
            catch { }
        }
        #endregion
        #region GetBranchNameCompanyWiseForHubAdmin
        private void GetBranchNameCompanyWiseForHubAdmin(int iCompanyID)
        {
            grdBranch.DataSource = objCompany.GetBranchNameCompanyWiseForHubAdmin(iCompanyID);
            grdBranch.DataBind();
        }
        #endregion
        #region imgBtnAddBranch_Click
        private void imgBtnAddBranch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
            {
                if (ddlCompany.SelectedIndex != 0)
                {
                    Response.Redirect("BranchEditNL.aspx?CID=" + ddlCompany.SelectedValue.Trim());
                }
                else
                {
                    lblMessage.Text = "Please select a company name.";
                }
            }
            else
            {
                Response.Redirect("BranchEditNL.aspx");
            }
        }
        #endregion
        #region Datagrid_Click
        protected void Datagrid_Click(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DELETERECORD")
            {
                lblMessage.Visible = true;
                if (objCompany.DeleteBranchRecord(Convert.ToInt32(e.CommandArgument)))
                {
                    lblMessage.Text = "Record(s) deleted successfully.";

                    if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                    {
                        GetCompanyListForBranch();
                        GetBranchNameCompanyWiseForHubAdmin(14);
                    }
                    else
                    {
                        RecordSet rs = BranchNL.GetBranchList(System.Convert.ToInt32(Session["CompanyID"]));
                        grdBranch.DataSource = rs.ParentTable;
                        grdBranch.DataBind();
                    }
                }
                else
                {
                    lblMessage.Text = "Error deleting branch. References exists in invoice.";
                }
            }
        }
        #endregion
        #region grdBranch_ItemDataBound
        private void grdBranch_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((ImageButton)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript: return confirm ('Are you sure to delete this record ?');");
            }
        }
        #endregion
    }
}
