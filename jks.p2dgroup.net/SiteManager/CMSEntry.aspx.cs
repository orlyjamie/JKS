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
using CBSolutions.Architecture.Core;

public partial class SiteManager_CMSEntry : System.Web.UI.Page
{
    //protected void Page_Load(object sender, EventArgs e)
    //{

    //}
    #region: Global Variables
    JKS.ManagePage objManagePage = new JKS.ManagePage();
    #endregion
    //#region: Web Controls
    //protected System.Web.UI.WebControls.Label lblConfirmation;
    //protected System.Web.UI.WebControls.TextBox txtPageName;
    //protected System.Web.UI.WebControls.TextBox txtPageUrl;
    //protected System.Web.UI.WebControls.CheckBox chkIsParent;
    //protected System.Web.UI.WebControls.TextBox txtPageOrder;
    //protected System.Web.UI.WebControls.CheckBox chkHeader;
    //protected System.Web.UI.WebControls.CheckBox chkFooter;
    //protected System.Web.UI.WebControls.DropDownList ddlParentPage;
    //protected System.Web.UI.WebControls.Button btnSubmit;
    //protected System.Web.UI.WebControls.Button btnReset;
    //protected System.Web.UI.WebControls.CheckBox chkIsActive;
    //protected System.Web.UI.WebControls.DataGrid dgvCMSEntry;
    //protected System.Web.UI.WebControls.Label lblMessage;
    //#endregion
    #region: Page Events
    private void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PageID"] = null;
            lblMessage.Text = "";
            BindCombo();
            this.LoadData();
        }
    }
    #endregion
    #region: Web Form Designer generated code
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
        this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
        this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
        //this.dgvCMSEntry.SelectedIndexChanged += new System.EventHandler(this.dgvCMSEntry_SelectedIndexChanged);
        this.Load += new System.EventHandler(this.Page_Load);
        this.dgvCMSEntry.ItemCommand += new DataGridCommandEventHandler(dgvCMSEntry_ItemCommand);

    }
    #endregion
    #region: Button Events
    private void btnSubmit_Click(object sender, System.EventArgs e)
    {
        Save();
    }

    private void btnReset_Click(object sender, System.EventArgs e)
    {
        this.ResetControls();

    }
    #endregion
    #region: Methods

    private void ResetControls()
    {
        this.txtPageName.Text = "";
        this.txtPageUrl.Text = "";
        this.ddlParentPage.SelectedIndex = -1;
        this.txtPageOrder.Text = "";
        this.chkHeader.Checked = false;
        this.chkFooter.Checked = false;
        this.chkIsActive.Checked = false;
        ViewState["PageID"] = null;
        this.chkIsParent.Checked = false;

    }

    public bool Save()
    {
        bool Success = false;
        try
        {
            objManagePage.PageTitle = Convert.ToString(txtPageName.Text.Trim());
            objManagePage.NavigateUrl = Convert.ToString(txtPageUrl.Text.Trim());


            if (Convert.ToString(ddlParentPage.SelectedValue) == "0" || ddlParentPage.SelectedValue.Trim() == "")
            {
                objManagePage.ParentPageID = 0;
            }
            else
            {
                objManagePage.ParentPageID = Convert.ToInt32(ddlParentPage.SelectedValue);
            }

            objManagePage.IsParentPage = Convert.ToInt32(chkIsParent.Checked);
            objManagePage.ShowOnFooter = Convert.ToInt32(chkFooter.Checked);
            objManagePage.ShowOnHeader = Convert.ToInt32(chkHeader.Checked);
            objManagePage.PageOrder = Convert.ToInt32(txtPageOrder.Text.Trim());
            objManagePage.IsActive = Convert.ToInt32(chkIsActive.Checked);
            objManagePage.IsDeleted = Convert.ToInt32(0);
            objManagePage.AddedBy = 1;

            if (ViewState["PageID"] != null)
            {
                objManagePage.PageID = Convert.ToInt32(ViewState["PageID"].ToString());
            }
            else
            {
                objManagePage.PageID = 0;
            }
            if (objManagePage.Save())
            {
                Success = true;
                LoadData();
                this.ResetControls();
                lblMessage.Text = "Data Saved Successfully";
            }
            else
            {
                Success = false;
                lblMessage.Text = objManagePage.ExceptionMessage;
                objManagePage.ExceptionMessage = "";
            }
        }
        catch (Exception ex)
        {
            //ShowMessage(lblMessageScript, MessageType.Error, ex.Message);
            //ShowHidePanel(lblJavaScript, PanelType.Entry);
            lblMessage.Text = ex.ToString();
        }

        return Success;

    }

    public void BindCombo()
    {
        DataTable dtParentPage = objManagePage.GetAll(" IsDeleted=0 And IsActive=1 And ISNULL(IsParentPage,0)=1");
        if (dtParentPage != null && dtParentPage.Rows.Count > 0)
        {
            ddlParentPage.DataTextField = "PageTitle";
            ddlParentPage.DataValueField = "PageID";
            ddlParentPage.DataSource = dtParentPage;
            ddlParentPage.DataBind();
        }
        ddlParentPage.Items.Insert(0, new ListItem("SELECT", "0"));
    }

    public void LoadData()
    {
        DataTable dtPage = objManagePage.GetAll(" IsDeleted=0 And IsActive=1 ");
        if (dtPage != null && dtPage.Rows.Count > 0)
        {
            //this.GetStatus();
            dgvCMSEntry.DataSource = dtPage;
            dgvCMSEntry.DataBind();
        }

    }

    protected string GetStatus(string Value)
    {
        string Status = "NO";
        if (Convert.ToString(Value) == "1")
        {
            Status = "YES";
        }
        return Status;

    }



    #endregion
    private void dgvCMSEntry_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string PageID = "0";
        if (e.CommandName == "ED")
        {
            try
            {
                PageID = e.CommandArgument.ToString();
                ViewState["PageID"] = PageID.ToString();
                DataTable dtPage = objManagePage.GetAll(" IsDeleted=0 And IsActive=1 AND  PageID=" + Convert.ToInt32(PageID));
                if (dtPage != null && dtPage.Rows.Count > 0)
                {
                    this.txtPageName.Text = Convert.ToString(dtPage.Rows[0]["PageTitle"]);
                    this.txtPageUrl.Text = Convert.ToString(dtPage.Rows[0]["NavigateUrl"]);
                    this.txtPageOrder.Text = Convert.ToString(dtPage.Rows[0]["PageOrder"]);
                    this.chkFooter.Checked = Convert.ToBoolean(dtPage.Rows[0]["ShowOnFooter"]);
                    this.chkHeader.Checked = Convert.ToBoolean(dtPage.Rows[0]["ShowOnHeader"]);
                    this.chkIsActive.Checked = Convert.ToBoolean(dtPage.Rows[0]["IsActive"]);
                    this.chkIsParent.Checked = Convert.ToBoolean(dtPage.Rows[0]["IsParentPage"]);
                    this.ddlParentPage.SelectedValue = Convert.ToString(dtPage.Rows[0]["PageTitle"]);
                }


            }
            catch (Exception Ex)
            {
                string Error = Ex.ToString();
            }



        }
        else if (e.CommandName == "DEL")
        {
            PageID = e.CommandArgument.ToString();
            objManagePage.DeleteCMSPage(Convert.ToInt32(PageID), 1);
            this.lblMessage.Text = "Data deleted successfully";
            LoadData();

        }

    }
}