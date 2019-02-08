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

public partial class SiteManager_CMSPageContent : System.Web.UI.Page
{

    #region: Global Variables
    JKS.ManagePageContent objManageCMSPageContent = new JKS.ManagePageContent();
    JKS.ManagePage objManagePage = new JKS.ManagePage();
    #endregion
    #region :Web Controls
    //protected System.Web.UI.WebControls.Label lblConfirmation;
    //protected System.Web.UI.WebControls.DropDownList ddlPageName;
    //protected System.Web.UI.WebControls.TextBox txtPageOrder;
    //protected System.Web.UI.WebControls.CheckBox chkIsActive;
    //protected System.Web.UI.WebControls.Button btnSubmit;
    //protected System.Web.UI.WebControls.Button btnReset;
    //protected System.Web.UI.WebControls.TextBox txtContents;
    //protected System.Web.UI.WebControls.DataGrid dgvCMSPageContent;
    //protected System.Web.UI.WebControls.Label lblMessage;
    #endregion


    private void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCombo();
            this.LoadData();
        }

        // Put user code to initialize the page here
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
        this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
        this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
        this.dgvCMSPageContent.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgvCMSPageContent_ItemCommand_1);
        this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion

    #region "Button Events"

    private void btnSubmit_Click(object sender, System.EventArgs e)
    {
        this.Save();
        this.LoadData();
        this.ClearControlsData();

    }

    private void btnReset_Click(object sender, System.EventArgs e)
    {
        this.ClearControlsData();
    }

    private void dgvCMSPageContent_ItemCommand_1(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        string PageContentID = "0";
        if (e.CommandName == "ED")
        {
            try
            {
                PageContentID = e.CommandArgument.ToString();
                ViewState["PageContentID"] = PageContentID.ToString();
                DataTable dtPage = objManageCMSPageContent.GetAll(" IsDeleted=0 And IsActive=1 AND  PageContentID=" + Convert.ToInt32(PageContentID));
                if (dtPage != null && dtPage.Rows.Count > 0)
                {
                    this.txtContents.Text = Convert.ToString(dtPage.Rows[0]["Contents"]);
                    this.txtPageOrder.Text = Convert.ToString(dtPage.Rows[0]["PageOrder"]);
                    this.chkIsActive.Checked = Convert.ToBoolean(dtPage.Rows[0]["IsActive"]);
                    this.ddlPageName.SelectedValue = Convert.ToString(dtPage.Rows[0]["PageName"]);
                }
            }
            catch (Exception Ex)
            {
                string Error = Ex.ToString();
            }

        }
        else if (e.CommandName == "DEL")
        {
            PageContentID = e.CommandArgument.ToString();
            objManageCMSPageContent.DeleteCMSPageContent(Convert.ToInt32(PageContentID), 1);
            this.lblMessage.Text = "Data deleted successfully";
            this.LoadData();

        }
    }




    public bool Save()
    {
        bool Success = false;
        try
        {
            if (Convert.ToString(ddlPageName.SelectedValue) == "0" || ddlPageName.SelectedValue.Trim() == "")
            {
                objManageCMSPageContent.PageID = 0;
            }
            else
            {
                objManageCMSPageContent.PageID = Convert.ToInt32(ddlPageName.SelectedValue);
            }
            objManageCMSPageContent.PageOrder = Convert.ToInt32(txtPageOrder.Text.Trim());
            objManageCMSPageContent.Contents = Convert.ToString(txtContents.Text.Trim());
            objManageCMSPageContent.MetaKey = "";
            objManageCMSPageContent.MetaDesc = "";
            objManageCMSPageContent.MetaTitle = "";

            objManageCMSPageContent.IsActive = Convert.ToInt32(chkIsActive.Checked);
            objManageCMSPageContent.IsDeleted = Convert.ToInt32(0);
            objManageCMSPageContent.AddedBy = 1;

            if (ViewState["PageContentID"] != null)
            {
                objManageCMSPageContent.PageContentID = Convert.ToInt32(ViewState["PageContentID"].ToString());
            }
            else
            {
                objManageCMSPageContent.PageContentID = 0;
            }
            if (objManageCMSPageContent.SaveCMSPageContent())
            {
                Success = true;
                //					LoadData();
                //					this.ResetControls();
                lblMessage.Text = "Data Saved Successfully";
            }
            else
            {
                Success = false;
                lblMessage.Text = objManageCMSPageContent.ExceptionMessage;
                objManageCMSPageContent.ExceptionMessage = "";
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


    #endregion

    #region: Methods/Functions

    public void BindCombo()
    {
        DataTable dtParentPage = objManagePage.GetAll(" IsDeleted=0 And IsActive=1 ");
        if (dtParentPage != null && dtParentPage.Rows.Count > 0)
        {
            this.ddlPageName.DataTextField = "PageTitle";
            ddlPageName.DataValueField = "PageID";
            ddlPageName.DataSource = dtParentPage;
            ddlPageName.DataBind();
        }
        ddlPageName.Items.Insert(0, new ListItem("SELECT", "0"));
    }


    public void LoadData()
    {
        DataTable dtPage = objManageCMSPageContent.GetAll(" IsDeleted=0 And IsActive=1 ");
        if (dtPage != null && dtPage.Rows.Count > 0)
        {
            //this.GetStatus();
            this.dgvCMSPageContent.DataSource = dtPage;
            this.dgvCMSPageContent.DataBind();
        }

    }


    private void ClearControlsData()
    {
        this.txtPageOrder.Text = "";
        this.txtPageOrder.Text = "";
        this.ddlPageName.SelectedIndex = -1;
        this.txtPageOrder.Text = "";
        this.chkIsActive.Checked = false;
        ViewState["PageContentID"] = null;
    }

    #endregion
}