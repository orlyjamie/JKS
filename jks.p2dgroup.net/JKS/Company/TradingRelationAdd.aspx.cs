using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;
using CBSolutions.ETH.Web.newlook;

namespace JKS
{
    /// <summary>
    /// Summary description for UserBrowse.
    /// </summary>
    public class TradingRelationAdd : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebFormControls
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.DropDownList cboBuyer;
        protected System.Web.UI.WebControls.Button btnSelectBuyer;
        protected System.Web.UI.WebControls.HyperLink hypAdd;
        #endregion

        #region Variable Declaration
        string buyerID = "0";
        Company oCompany = new Company();
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Label lblHeader;
        protected System.Web.UI.WebControls.DataGrid grdCompany;
        protected System.Web.UI.WebControls.Button btnAdd;
        protected System.Web.UI.WebControls.Button Button1;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.TextBox tbKeyWord;
        RecordSet rsGrid;
        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            Session["SelectedPage"] = "TradingRelation";
            buyerID = (string)Session["SelectedBuyerID"];
            lblHeader.Text = "Add Suppliers for " + (string)Session["SelectedBuyerName"];
            rsGrid = Company.GetSupplierCompanyList(System.Convert.ToInt32(buyerID));
            if (!IsPostBack)
                PopulateData();
        }
        #endregion

        #region PopulateData
        private void PopulateData()
        {
            grdCompany.DataSource = rsGrid.ParentTable.DefaultView;
            grdCompany.DataBind();
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
            this.grdCompany.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.grdCompany_PageIndexChanged);
            this.grdCompany.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdCompany_ItemDataBound);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        #region grdCompany_PageIndexChanged
        private void grdCompany_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < grdCompany.PageCount)
            {
                grdCompany.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                grdCompany.CurrentPageIndex = grdCompany.PageCount;
            }
            PopulateData();

        }
        #endregion
        #region btnAdd_Click
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            string strErrorMessage = "";
            int iDatagridRowCount = 0;
            iDatagridRowCount = grdCompany.Items.Count;

            if (CheckBlankCompanyCodeForCheckedRows(iDatagridRowCount, out strErrorMessage) == false)
            {
                lblMessage.Text = strErrorMessage;
                return;
            }


            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.CreateInsertBuffer("TradingRelation");
            TextBox tbV_Code = null;
            TextBox tbV_Class = null;
            DropDownList ddlCurrency = null;

            for (int i = 0, j = grdCompany.Items.Count; i < j; i++)
            {
                if (this.grdCompany.Items[i].FindControl("chkRelation").ID == "chkRelation")
                {
                    CheckBox chk = (CheckBox)this.grdCompany.Items[i].FindControl("chkRelation");
                    if (chk.Checked)
                    {
                        tbV_Code = (TextBox)grdCompany.Items[i].FindControl("tbCompanyCode");
                        tbV_Class = (TextBox)grdCompany.Items[i].FindControl("tbVendorClass");
                        ddlCurrency = (DropDownList)grdCompany.Items[i].FindControl("ddlCurrencyType");
                        rs.AddNew();
                        int dtIndex = grdCompany.Items[i].DataSetIndex;
                        int supplierID = System.Convert.ToInt32(rsGrid.ParentTable.Rows[dtIndex]["CompanyID"]);
                        rs["BuyerCompanyID"] = System.Convert.ToInt32(Session["SelectedBuyerID"]);
                        rs["SupplierCompanyID"] = supplierID;
                        rs["SupplierCodeAgainstBuyer"] = tbV_Code.Text.Trim();
                        rs["New_CurrencyTypeID"] = ddlCurrency.SelectedValue.Trim();
                        rs["New_VendorClass"] = tbV_Class.Text.Trim();
                        int pkID = 0;
                        da.InsertRow(rs, ref pkID);
                    }
                }
            }

            rs = null;
            Response.Redirect("TradingRelation.aspx");
        }
        #endregion

        public bool CheckBlankCompanyCodeForCheckedRows(int iRows, out string strErrorMessage)
        {
            bool bBlankCompanyCode = true;
            strErrorMessage = "";

            for (int iCounter = 0; iCounter < iRows; iCounter++)
            {
                Label lbl = null;
                CheckBox chk = null;
                TextBox tb = null;
                TextBox tb1 = null;

                lbl = (Label)grdCompany.Items[iCounter].FindControl("lblSupplierCompanyName");
                chk = (CheckBox)grdCompany.Items[iCounter].FindControl("chkRelation");
                tb = (TextBox)grdCompany.Items[iCounter].FindControl("tbCompanyCode");
                tb1 = (TextBox)grdCompany.Items[iCounter].FindControl("tbVendorClass");

                if (chk.Checked == true && (tb.Text.Trim().Equals("") || tb1.Text.Trim().Equals("")))
                {
                    strErrorMessage = "Please enter all the fields for [" + lbl.Text + "] as it is selected." + Environment.NewLine;
                    bBlankCompanyCode = false;
                    break;
                }
            }
            return (bBlankCompanyCode);
        }


        protected void ddlCurrencyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            TableCell cell = list.Parent as TableCell;
            DataGridItem item = cell.Parent as DataGridItem;

            int index = item.ItemIndex;
            string content = item.Cells[0].Text;

        }


        private void grdCompany_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DropDownList list = (DropDownList)e.Item.FindControl("ddlCurrencyType");
                list.DataSource = oCompany.GetCurrencyTypes();
                list.DataBind();
                list.SelectedValue = "22";
            }
        }


        #region SearchCompanyForTradingRelation
        private void SearchCompanyForTradingRelation(string strKeyWord)
        {
            try
            {
                grdCompany.DataSource = oCompany.SearchCompanyForTradingRelation(strKeyWord);
                grdCompany.DataBind();

                if (grdCompany.Items.Count > 0)
                {
                    grdCompany.Visible = true;
                }
                else
                {
                    grdCompany.Visible = false;
                    lblMessage.Text = "Sorry, no record(s) found.";
                }
            }
            catch { }
        }
        #endregion

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            grdCompany.CurrentPageIndex = 0;
            SearchCompanyForTradingRelation(tbKeyWord.Text.Trim());
        }

    }
}
