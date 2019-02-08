using System;
using System.IO;
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


namespace JKS
{
    /// <summary>
    /// Summary description for UserBrowse.
    /// </summary>
    public class TradingRelation : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebForm Controls
        protected System.Web.UI.WebControls.DropDownList cboBuyer;
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Button Button1;
        protected System.Web.UI.WebControls.DataGrid grdCompany;
        protected System.Web.UI.WebControls.HyperLink hypAdd;
        protected System.Web.UI.WebControls.ImageButton imgButtonSendMailInfo;
        protected System.Web.UI.WebControls.Label lblMessage;
        #endregion
        #region Object Declaration
        private Company objCompany = new Company();
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            Session["SelectedPage"] = "TradingRelation";

            if (!Page.IsPostBack)
            {
                if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                    LoadData();
                else
                    GetBuyerCompanyListForTradingRelation(Convert.ToInt32(Session["CompanyID"]));

                ShowSuppliersForThisBuyer();
            }
        }
        #endregion
        #region LoadData
        private void LoadData()
        {
            cboBuyer.Items.Clear();
            cboBuyer.Items.Insert(0, new ListItem("Select", "0"));
            RecordSet rs = Company.GetBuyerCompanyList();
            while (!rs.EOF())
            {
                ListItem listItem = new ListItem(rs["CompanyName"].ToString(), rs["CompanyID"].ToString());
                cboBuyer.Items.Add(listItem);
                rs.MoveNext();
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
            this.Button1.Click += new System.EventHandler(this.btnSelectBuyer_Click);
            this.grdCompany.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdCompany_ItemCreated);
            this.grdCompany.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this._PageIndexChanged);
            this.imgButtonSendMailInfo.Click += new System.Web.UI.ImageClickEventHandler(this.imgButtonSendMailInfo_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        #region grdCompany_ItemCreated
        private void grdCompany_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink ctrl = (HyperLink)e.Item.FindControl("hyperlinkDelete");
                if (ctrl != null)
                {
                    if (e.Item.DataItem != null)
                    {
                        DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                        string navigateURL = "TradingRelationDelete.aspx?" + "TradingRelationID=" + dr["TradingRelationID"].ToString();
                        ctrl.NavigateUrl = navigateURL;
                    }
                }
            }
        }
        #endregion
        #region btnSelectBuyer_Click
        private void btnSelectBuyer_Click(object sender, System.EventArgs e)
        {
            if (cboBuyer.SelectedIndex != 0)
            {
                Session["SelectedBuyerID"] = cboBuyer.SelectedValue;
                Session["SelectedBuyerName"] = cboBuyer.SelectedItem.Text;
                PopulateData(Convert.ToInt32(cboBuyer.SelectedValue));
            }
            else
                lblMessage.Text = "Please select a buyer company.";
        }
        #endregion
        #region PopulateData
        private void PopulateData(int iBuyerCompanyID)
        {
            try
            {
                hypAdd.Visible = true;
                grdCompany.Visible = true;
                RecordSet rs = Company.GetBuyersTradingCompanyList(iBuyerCompanyID);
                grdCompany.DataSource = rs.ParentTable;
                grdCompany.DataBind();

                if (grdCompany.Items.Count > 0)
                {
                    lblMessage.Text = "";
                    grdCompany.Visible = true;
                    imgButtonSendMailInfo.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Sorry, no record(s) found.";
                    grdCompany.Visible = false;
                    imgButtonSendMailInfo.Visible = false;
                }

                imgButtonSendMailInfo.Visible = false;

            }
            catch { }
        }
        #endregion
        #region imgButtonSendMailInfo_Click
        private void imgButtonSendMailInfo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                objCompany.SendEmailInfo(grdCompany);
                lblMessage.Text = "Mail sent successfully.";

            }
            catch { lblMessage.Text = "Error sending mail."; }
        }
        #endregion
        #region GetBuyerCompanyListForTradingRelation
        private void GetBuyerCompanyListForTradingRelation(int iCompanyID)
        {
            try
            {
                cboBuyer.DataSource = objCompany.GetBuyerCompanyListForTradingRelation(iCompanyID);
                cboBuyer.DataBind();

                cboBuyer.Items.Insert(0, new ListItem("Select", "0"));

            }
            catch { }
        }
        #endregion
        #region _PageIndexChanged
        public void _PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < grdCompany.PageCount)
            {
                grdCompany.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                grdCompany.CurrentPageIndex = grdCompany.PageCount;
            }
            PopulateData(Convert.ToInt32(cboBuyer.SelectedValue));
        }
        #endregion
        private void ShowSuppliersForThisBuyer()
        {
            if (Session["SelectedBuyerID"] != null)
            {
                cboBuyer.SelectedValue = Session["SelectedBuyerID"].ToString().Trim();
                PopulateData(Convert.ToInt32(cboBuyer.SelectedValue));
            }
        }
    }
}