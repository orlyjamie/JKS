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
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;

namespace JKS
{
    /// <summary>
    /// Summary description for debitnotehistory.
    /// </summary>
    public class debitnotehistory : System.Web.UI.Page
    {
        #region  web controls declarations
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.DropDownList ddlBuyerCompany;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.TextBox txtDebitNoteNo;
        protected System.Web.UI.WebControls.DropDownList ddlDocStatus;
        protected System.Web.UI.WebControls.DataGrid grdInvCur;
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
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region Data Access Variable Declaration
        private DataTable dtbl = null;
        private DebitNote oDebitNote = new DebitNote();
        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["TempCompanyID"] != null)
                Session["CompanyID"] = Session["TempCompanyID"];

            Session["ApproveForm"] = 0;
            Session["SelectedPage"] = "DebitNoteLog";

            if (!Page.IsPostBack)
            {
                PopulateDropDownList();
                GetSalesInvoiceLogForSupplier(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(ddlBuyerCompany.SelectedValue), txtDebitNoteNo.Text.Trim(), Convert.ToInt32(Session["UserID"]), "0");

            }
        }
        #endregion



        #region grdInvCur_PageIndexChanged
        public void grdInvCur_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < grdInvCur.PageCount)
                grdInvCur.CurrentPageIndex = e.NewPageIndex;
            else
                grdInvCur.CurrentPageIndex = grdInvCur.PageCount;

            if (ViewState["dtbl"] != null)
            {
                dtbl = (DataTable)ViewState["dtbl"];
                PopulateDataGrid(dtbl);
            }
        }
        #endregion

        #region Sort_GridCur
        public void Sort_GridCur(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            DataView dv = null;
            try
            {
                string str2 = "";

                str2 = "SupplierID=" + ((int)Session["CompanyID"]).ToString() + " AND StatusId != 7 OR StatusId IS NULL";

                if ((int)Session["UserTypeID"] == 1)
                    str2 = str2 + " and ModUserID=" + (int)Session["UserID"];

                if (ViewState["dtbl"] != null)
                    dtbl = (DataTable)ViewState["dtbl"];

                dv = dtbl.DefaultView;
                dv.Sort = e.SortExpression;
                PopulateDataGrid(dv);
            }
            catch { }
            finally
            {
                dv = null;
            }
        }
        #endregion

        #region PopulateDataGrid METHOD OVERLOADED
        private void PopulateDataGrid(DataView dView)
        {
            grdInvCur.DataSource = dView;
            grdInvCur.DataBind();
            dView.Dispose();
        }

        private void PopulateDataGrid(DataTable dtbl)
        {
            if (dtbl.Rows.Count > 0)
            {
                grdInvCur.Visible = true;
                lblMessage.Text = "";

                grdInvCur.DataSource = dtbl;
                grdInvCur.DataBind();
                dtbl.Dispose();
            }
            else
            {
                grdInvCur.Visible = false;
                lblMessage.Text = "Sorry, no record(s) found.";
            }
        }
        #endregion

        #region GetSalesInvoiceLogForSupplier
        private void GetSalesInvoiceLogForSupplier(int iSupplierCompanyID, int iBuyerCompanyID, string strInvoiceNo, int iInvoiceCreatedBy, string strStatusId)
        {
            DataTable dtbl = null;
            try
            {
                int iParentSupp = 0;
                if (Session["ParentCompanyID"] != null && System.Convert.ToInt32(Session["ParentCompanyID"]) != 0)
                {
                    if (iSupplierCompanyID == Convert.ToInt32(Session["ParentCompanyID"]))
                        iParentSupp = Convert.ToInt32(Session["ParentCompanyID"]);
                }

                if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                    dtbl = oDebitNote.GetSalesInvoiceLogForSupplier(iSupplierCompanyID, iBuyerCompanyID, strInvoiceNo, 0, iInvoiceCreatedBy, strStatusId, iParentSupp);
                else
                    dtbl = oDebitNote.GetSalesInvoiceLogForSupplier(iSupplierCompanyID, iBuyerCompanyID, strInvoiceNo, 1, iInvoiceCreatedBy, strStatusId, iParentSupp);

                ViewState["dtbl"] = dtbl;
                PopulateDataGrid(dtbl);
            }
            catch { }
            finally
            {
                dtbl = null;
            }
        }
        #endregion

        #region PopulateDropDownList
        private void PopulateDropDownList()
        {
            RecordSet rs = null;
            try
            {
                rs = Invoice.GetTradingPartnerList(System.Convert.ToInt32(Session["CompanyID"]));

                ddlBuyerCompany.DataSource = rs.ParentTable;
                ddlBuyerCompany.DataBind();
                ddlBuyerCompany.Items.Insert(0, new ListItem("Select", "0"));

                ddlDocStatus.DataSource = oDebitNote.GetStatusList();
                ddlDocStatus.DataBind();
                ddlDocStatus.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch { }
            finally
            {
                rs = null;
            }
        }
        #endregion

        #region btnSearch_Click
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            grdInvCur.CurrentPageIndex = 0;
            GetSalesInvoiceLogForSupplier(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(ddlBuyerCompany.SelectedValue), txtDebitNoteNo.Text.Trim(), Convert.ToInt32(Session["UserID"]), Convert.ToString(ddlDocStatus.SelectedValue));

        }
        #endregion


        public void Grid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
            }
        }


    }


}
