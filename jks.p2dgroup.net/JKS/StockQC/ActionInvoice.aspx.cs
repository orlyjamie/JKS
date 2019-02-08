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

namespace JKS
{
    /// <summary>
    /// Summary description for WebForm1.
    /// </summary>
    public class ActionInvoice : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region web controls
        protected System.Web.UI.WebControls.Label lblConfirmation;
        protected System.Web.UI.WebControls.Label lblType;
        protected System.Web.UI.WebControls.Label lblRefernce;
        protected System.Web.UI.WebControls.Label lblinvoicetype;
        protected System.Web.UI.WebControls.Label lblsupplier;
        protected System.Web.UI.WebControls.Label lbldate;
        protected System.Web.UI.WebControls.Label lblinvoicedate;
        protected System.Web.UI.WebControls.Label lblstatus;
        protected System.Web.UI.WebControls.Label lblinvoicestatus;
        protected System.Web.UI.WebControls.Label lblvatcode;
        protected System.Web.UI.WebControls.Label lblAPCompanyID;
        protected System.Web.UI.WebControls.Button btnsubmit;
        protected System.Web.UI.WebControls.Button btndelete;
        protected System.Web.UI.WebControls.Label lblMessege;
        #endregion
        #region UserDefined Variable
        int InvoiceDetailID = 0;
        string Category = "";
        string OrderNo = "";
        string ProductCode = "";
        string Colour = "";
        protected System.Web.UI.WebControls.DataGrid grdInvCur;
        protected System.Web.UI.WebControls.DropDownList ddlOrderNo;
        protected System.Web.UI.WebControls.DropDownList ddlBuyerProdCode;
        protected System.Web.UI.WebControls.DropDownList ddlColor;
        Invoice.Invoice objinvoice = new Invoice.Invoice();
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btndelete.Attributes.Add("onclick", "javascript:return confirm('Are you sure you wish to delete?');");
                Session["oInvoiceID"] = Request.QueryString["InvoiceID"].ToString();
                string InvoiceNo = "";
                string Category = Request.QueryString["InvoiceType"];
                objinvoice.GetDocumentNoByDocID(Convert.ToInt32(Session["oInvoiceID"]), Category, out InvoiceNo);
                lblRefernce.Text = InvoiceNo;
                Session["oInvoiceNo"] = InvoiceNo;
                LoadData(InvoiceNo);
                PopulateGrid(Convert.ToInt32(Session["oInvoiceID"]));
            }
        }


        private void PopulateGrid(int iInvoiceID)
        {

            if (Category == "INV")
            {
                grdInvCur.DataSource = objinvoice.GetData(iInvoiceID, "INV");
                grdInvCur.DataBind();
            }
            else if (Category == "CRN")
            {
                grdInvCur.DataSource = objinvoice.GetData(iInvoiceID, "CRN");
                grdInvCur.DataBind();
            }
        }

        private void PopulateDropDown(string sPurOrderNo, DropDownList refddlProdCode, DropDownList refddlColor, string sBuyersProdCode, string sColor)
        {
            DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            RecordSet rs = da.ExecuteQuery("vwProdCodeColor", "purorderno = '" + sPurOrderNo + "'");
            refddlProdCode.Items.Clear();
            refddlColor.Items.Clear();
            refddlProdCode.Items.Add(new ListItem("--Select--", DBNull.Value.ToString()));
            refddlColor.Items.Add(new ListItem("--Select--", DBNull.Value.ToString()));

            while (!rs.EOF())
            {
                refddlProdCode.Items.Add(new ListItem(Convert.ToString(rs["BuyerProdCode"]), Convert.ToString(rs["ProdCodeOrderID"])));
                refddlColor.Items.Add(new ListItem(Convert.ToString(rs["Color"])));
                rs.MoveNext();
            }
        }
        private void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string sPurOrderNo = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "PurOrderNo"));
                string sBuyersProdCode = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "BuyersProdCode"));
                string sColor = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "New_Definable1"));

                DropDownList ddlPCode = ((DropDownList)e.Item.FindControl("ddlProdCode"));
                DropDownList ddlCCode = ((DropDownList)e.Item.FindControl("ddlColor"));

                PopulateDropDown(sPurOrderNo, ddlPCode, ddlCCode, sBuyersProdCode, sColor);
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.grdInvCur.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvCur_ItemDataBound);
            this.btnsubmit.Click += new System.EventHandler(this.btnsubmit_Click);
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region LoadData

        private void LoadData(string InvoiceNo)
        {
            string SupplierID = Request.QueryString["SupplierID"];
            Category = Request.QueryString["InvoiceType"].ToString();
            if (Category == "INV")
            {
                lbldate.Text = "Invoice Date";
                lblstatus.Text = "Invoice Status";
                lblType.Text = "Invoice No";
            }
            else
            {
                lbldate.Text = "Invoice Date";
                lblstatus.Text = "Invoice Status";
                lblType.Text = "Invoice No";
            }
            lblinvoicedate.Text = Request.QueryString["Date"].ToString();
            lblinvoicestatus.Text = Request.QueryString["Status"].ToString();
            lblinvoicetype.Text = Category;
            lblsupplier.Text = objinvoice.GetSupplierName(Convert.ToInt32(Session["oInvoiceID"]), Category);
            lblvatcode.Text = objinvoice.GetVatCode(Convert.ToInt32(Request.QueryString["InvoiceID"].ToString()), Category);
            lblAPCompanyID.Text = objinvoice.GetAPCompanyID(Convert.ToInt32(Request.QueryString["InvoiceID"].ToString()), Category);
        }
        #endregion

        #region btnSubmit
        private void btnsubmit_Click(object sender, System.EventArgs e)
        {
            int i = 0;
            string sErrMsg = "";
            bool isRecSaved = false, isSelected = true;
            Category = Request.QueryString["InvoiceType"];
            if (Category == "INV")
            {
                for (i = 0; i <= grdInvCur.Items.Count - 1; i++)
                {
                    InvoiceDetailID = Convert.ToInt32(((Label)grdInvCur.Items[i].FindControl("lblInvDtlId")).Text.Trim());
                    OrderNo = ((Label)grdInvCur.Items[i].FindControl("lblOrderNo")).Text;

                    if (OrderNo != "")
                    {
                        if (((DropDownList)grdInvCur.Items[i].FindControl("ddlProdCode")).SelectedIndex > 0)
                        {
                            ProductCode = ((DropDownList)grdInvCur.Items[i].FindControl("ddlProdCode")).SelectedItem.Text;
                        }
                        else
                        {
                            isSelected = false;
                            break;

                        }
                        if (((DropDownList)grdInvCur.Items[i].FindControl("ddlColor")).SelectedIndex > 0)
                        {
                            Colour = ((DropDownList)grdInvCur.Items[i].FindControl("ddlColor")).SelectedItem.Text;
                        }
                        else
                        {
                            isSelected = false;
                            break;

                        }
                    }

                    int iInvoiceID = Convert.ToInt32(Session["oInvoiceID"].ToString());
                    string strInvoiceNo = Session["oInvoiceNo"].ToString();

                    sErrMsg = objinvoice.GetUpdate(iInvoiceID, InvoiceDetailID, OrderNo, strInvoiceNo, ProductCode, lblinvoicetype.Text.Trim().ToString(), Colour, Convert.ToInt32(lblAPCompanyID.Text));

                    if (sErrMsg.Trim() == "")
                    {
                        isRecSaved = true;
                    }
                    else
                    {
                        isRecSaved = false;
                    }

                }

            }
            else if (Category == "CRN")
            {
                for (i = 0; i <= grdInvCur.Items.Count - 1; i++)
                {
                    int CreditNoteID = Convert.ToInt32(Session["oInvoiceID"].ToString());
                    string CreditNoteNo = Session["oInvoiceNo"].ToString();
                    OrderNo = ((Label)grdInvCur.Items[i].FindControl("lblOrderNo")).Text;
                    ProductCode = ((DropDownList)grdInvCur.Items[i].FindControl("ddlProdCode")).SelectedItem.Text;
                    Colour = ((DropDownList)grdInvCur.Items[i].FindControl("ddlColor")).SelectedItem.Text;

                    if (objinvoice.GetCreditNoteUpdate(CreditNoteID, OrderNo, CreditNoteNo, ProductCode, lblinvoicetype.Text.Trim().ToString(), Colour, Convert.ToInt32(lblAPCompanyID.Text)))
                    {
                        isRecSaved = true;
                    }
                    else
                    {
                        isRecSaved = false;
                    }
                }
            }

            if (isRecSaved)
            {
                if (isSelected)
                {
                    lblMessege.Text = " Record saved successfully";
                    lblMessege.Visible = true;
                    Response.Write("<script>alert('Invoice submitted successfully');</script>");
                    Response.Write("<script>opener.location.reload(true);</script>");
                    Response.Write("<script>self.close();</script>");
                }
                else
                {
                    lblMessege.Text = "Please select dropdowns to save successfully.";
                    lblMessege.Visible = true;
                }
            }
            else
            {
                lblMessege.Text = " Record not saved successfully";
                lblMessege.Visible = true;
            }
        }
        #endregion

        #region btnDeleteClick
        private void btndelete_Click(object sender, System.EventArgs e)
        {
            int InvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
            string Category = Request.QueryString["InvoiceType"];
            Invoice.Invoice objinvoice = new Invoice.Invoice();

            if (objinvoice.Deleteinvoicecreditnote(InvoiceID, Category))
            {
                lblMessege.Text = "Deleted successfully";
                Response.Write("<script>alert('Invoice deleted successfully');</script>");
                Response.Write("<script>opener.location.reload(true);</script>");
                Response.Write("<script>self.close();</script>");
            }

            else
                lblMessege.Text = "Error in deleting document.";
        }
        #endregion
    }
}
