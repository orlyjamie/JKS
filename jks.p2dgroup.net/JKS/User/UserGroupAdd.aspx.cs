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
using CBSolutions.ETH.Web;

namespace JKS
{
    /// <summary>
    /// Summary description for UserGroupAdd.
    /// </summary>
    public partial class UserGroupAdd : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region web Controls
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Label lblHeader;
        //protected System.Web.UI.WebControls.Label lblMessage;
        //protected System.Web.UI.WebControls.DataGrid grdUser;
        //protected System.Web.UI.WebControls.TextBox tbGroupName;
        //protected System.Web.UI.WebControls.ImageButton imgBtnSave;
        #endregion

        private Users objUser = new Users();
        DataTable objDT = new DataTable();

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            if (!this.IsPostBack)
                PopulateGrid(Convert.ToInt32(Session["UserID"]));
        }

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
            //this.imgBtnSave.Click += new System.Web.UI.ImageClickEventHandler(this.imgBtnSave_Click);
            this.imgBtnSave.Click += new EventHandler(imgBtnSave_Click);
            this.grdUser.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.Datagrid_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region SaveGroup
        private void SaveGroup(string strAName, int iUserID)
        {
            if (objUser.SaveGroupDalkia(strAName, iUserID))
            {
                lblMessage.Text = "Record(s) saved successfully.";
            }
            else
                lblMessage.Text = "Group Already Exist.";
        }
        #endregion

        private void imgBtnSave_Click(object sender, EventArgs e)
        {
            if (tbGroupName.Text.Trim() == "")
                lblMessage.Text = "Please enter a groupname.";
            else
            {
                SaveGroup(tbGroupName.Text.Trim(), Convert.ToInt32(Session["UserID"]));
                PopulateGrid(Convert.ToInt32(Session["UserID"]));
            }
        }

        //private void imgBtnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    if(tbGroupName.Text.Trim() == "")
        //        lblMessage.Text = "Please enter a groupname.";
        //    else
        //    {
        //        SaveGroup(tbGroupName.Text.Trim(),Convert.ToInt32(Session["UserID"]));	
        //        PopulateGrid(Convert.ToInt32(Session["UserID"]));
        //    }
        //}

        #region PopulateGrid
        public void PopulateGrid(int iUserID)
        {
            objDT = objUser.GetUserGroupDalkia(iUserID);
            grdUser.DataSource = objDT;
            grdUser.DataBind();
        }
        #endregion

        #region Datagrid_Click
        protected void Datagrid_Click(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DELETERECORD")
            {
                if (objUser.DeleteUserGroupDalkia(Convert.ToInt32(e.CommandArgument), Convert.ToInt32(Session["UserID"])))
                {
                    lblMessage.Text = "Record(s) deleted successfully.";
                    PopulateGrid(Convert.ToInt32(Session["UserID"]));
                }
                else
                {
                    lblMessage.Text = "Error deleting record(s) or this group is still assigned to user(s).";
                }
            }
        }
        #endregion
    }
}
