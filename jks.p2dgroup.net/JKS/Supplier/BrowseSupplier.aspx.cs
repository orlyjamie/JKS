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
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;
using System.Data.SqlClient;


namespace JKS
{
    /// <summary>
    /// Summary description for BrowseSupplier.
    /// </summary>
    /// 
    [ScriptService]
    public partial class BrowseSupplier : CBSolutions.ETH.Web.ETC.VSPage
    {


        #region User Define Variables
        Supplier_New objSupplier = new Supplier_New();
        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            if (!IsPostBack)
            {
                GetAllDropDowns();

                //added by Koushik das as on 16-Mar-2017
                string Current = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                string Referer = (Request.UrlReferrer != null) ? System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath) : Current;

                if (Referer == Current)
                    Session.Remove("SupplierEditCompanyID");

                if (Session["SupplierEditCompanyID"] != null)
                    ddlCompany.SelectedValue = (string)Session["SupplierEditCompanyID"];
                else
                    ddlCompany.SelectedValue = "0";//Convert.ToString(Session["CompanyID"]);

                ddlCompany_SelectedIndexChanged(sender, e);

                btnSearch_Click(sender, e);
                //added by Koushik das as on 16-Mar-2017
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
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);

            this.grdApprover.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdApprover_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region grdApprover_ItemDataBound
        private void grdApprover_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((Button)e.Item.FindControl("imgBtnDelete")).Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to delete this record?')");
            }
        }
        #endregion

        #region GetAllDropDowns()
        public void GetAllDropDowns()
        {
            if (!Page.IsPostBack)
            {
                /*Blocked by kuntalkarar on 27thMay2016*/
                //GetCompanyDropDown();
                /*Added by kuntalkarar on 27thMay2016*/
                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
                //--------------------------------------

                GetDataGridValues(System.Convert.ToInt32(ddlCompany.SelectedValue), 0, "", "", 1);
            }

        }
        #endregion
        /*--------------Added by kuntalkarar on 27thMay2016-------------------*/
        private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int iAction)
        {
            if (iAction == 1)
            {
                Company objCompany = new Company();
                ddlCompany.Items.Clear();
                DataTable dt = new DataTable();
                //try
                //{

                //if (dt.Rows.Count > 0)
                //{
                //    ddlCompany.DataSource = dt;
                //    ddlCompany.DataBind();

                //    //--------------- Set Default Selected value  "Select Company Name"  by Subha,04-02-2015
                if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    // ddlCompany.Items.Insert(0, "Select Company Name");

                    ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                }

                //modified by kuntal on 29thApril2015
                else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    string LogInCompanyid = "";
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    //ddlCompany.Items.Insert(0, "Select Company Name");
                    PasswordReset objPasswordReset = new PasswordReset();
                    List<PasswordReset> lstSaltedPassword = objPasswordReset.GetLogInCompanyId(Convert.ToInt32(Session["UserID"]));//strResetAnswer
                    if (lstSaltedPassword.Count > 0)
                    {
                        LogInCompanyid = lstSaltedPassword[0].LoginCompanyId;
                    }


                    ddlCompany.SelectedValue = LogInCompanyid;

                }
                //else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                //{
                //    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                //    ddlCompany.DataSource = dt;
                //    ddlCompany.DataBind();
                //    ddlCompany.Items.Insert(0, "Select Company Name");

                //}
                //------------------------------------------------------------
                else
                {
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    // ddlCompany.Items.Insert(0, "Select Company Name");

                    ddlCompany.SelectedValue = dt.Rows[0][0].ToString();
                    Session["DropDownCompanyID"] = ddlCompany.SelectedValue.ToString();
                }
                //}
            }
            //catch { }
            //finally
            //{
            //    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

            //}
            //}
            /* JKS.Invoice objInvoice = new JKS.Invoice();
             ddlSupplier.Items.Clear();
             if (ddlCompany.SelectedItem.Text != "Select Company Name")
             {
                 ddlSupplier.DataSource = objInvoice.GetSuppliersList(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
             }
             ddlSupplier.DataBind();
             ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));

             JKS.Invoice_New objInv1 = new JKS.Invoice_New();*/
            GetVendorClassAgainstSupplierANDBuyer();


        }
        //---------------------------------------------------------------------

        #region GetCompanyDropDown()
        public void GetCompanyDropDown()
        {
            JKS.Supplier oSupplier = new JKS.Supplier();
            DataSet ds = new DataSet();
            ds = oSupplier.GetCompanyDropDownGeneric(System.Convert.ToInt32(Session["CompanyID"]));
            ddlCompany.DataSource = ds;
            ddlCompany.DataBind();
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyID";
            GetVendorClassAgainstSupplierANDBuyer();
        }
        #endregion
        #region GetVendorClassAgainstSupplierANDBuyer
        private void GetVendorClassAgainstSupplierANDBuyer()
        {
            int iBuyerCID = 0;
            iBuyerCID = Convert.ToInt32(ddlCompany.SelectedValue);
            Supplier oSupplier = new Supplier();
            DataSet ds = new DataSet();
            string Fields = "DISTINCT SupplierCompanyID AS CompanyID ,Company.CompanyName AS CompanyName,TradingRelationID,SupplierCodeAgainstBuyer AS VendorClass,New_VendorClass";
            string Table = "TradingRelation, Company";
            string Criteria = "BuyerCompanyID  = " + iBuyerCID + " AND New_VendorClass IS NOT NULL AND TradingRelation.SupplierCompanyID = Company.CompanyID order by CompanyName";
            ds = oSupplier.GetGlobalDropDowns(Fields, Table, Criteria);
            //ddlSupplier.DataSource = ds;
            //ddlSupplier.DataBind();
            //ddlSupplier.Items.Insert(0,"Select");

            ddlVendor.DataSource = ds;
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, "Select");

            DataSet ds1 = new DataSet();
            string Fields1 = "DISTINCT New_VendorClass";
            string Table1 = "TradingRelation";
            string Criteria1 = "BuyerCompanyID  = " + iBuyerCID + " AND New_VendorClass IS NOT NULL";
            ds1 = oSupplier.GetGlobalDropDowns(Fields1, Table1, Criteria1);

            ddlVClass.DataSource = ds1;
            ddlVClass.DataBind();
            ddlVClass.Items.Insert(0, "Select");
        }
        #endregion
        #region GetDataGridValues
        public void GetDataGridValues(int iCompanyID, int iSuppCompanyID, string StrVClass, string VendorID, int Status)
        {
            //Supplier oSupplier = new Supplier();
            Supplier_New objSupplier = new Supplier_New();
            DataSet ds = new DataSet();
            //ds = oSupplier.GetSupplierStatusFromTradingRelation(iCompanyID,iSuppCompanyID,StrVClass,VendorID,Status);
            ds = objSupplier.GetSupplierStatusFromTradingRelation(iCompanyID, iSuppCompanyID, StrVClass, VendorID, Status);
            grdApprover.DataSource = ds;
            grdApprover.DataBind();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        grdApprover.Visible = true;
                        lblMessage.Text = "";
                    }
                    else
                    {
                        grdApprover.Visible = false;
                        lblMessage.Text = "Sorry no records found.";
                    }
                }
                else
                {
                    grdApprover.Visible = false;
                    lblMessage.Text = "Sorry no records found.";
                }
            }
            else
            {
                grdApprover.Visible = false;
                lblMessage.Text = "Sorry no records found.";
            }

        }
        #endregion
        #region ddlCompany_SelectedIndexChanged
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GetVendorClassAgainstSupplierANDBuyer();

            int iSuppid = 0;
            string VClass = "";
            string VID = "";
            int iStatus = 1;
            txtSupplier.Value = "";

            //if(ddlSupplier.SelectedValue !="Select")
            //    iSuppid = Convert.ToInt32(ddlSupplier.SelectedValue);

            if (txtSupplier.Value.ToString().Trim().Length > 0)
            {
                if (HdSupplierId.Value != "")
                    iSuppid = Convert.ToInt32(HdSupplierId.Value);
            }
            else
                iSuppid = 0;

            if (ddlVendor.SelectedValue != "Select")
                VID = Convert.ToString(ddlVendor.SelectedItem.Text.Trim());
            if (ddlVClass.SelectedValue != "Select")
                VClass = Convert.ToString(ddlVClass.SelectedItem.Text.Trim());
            if (Convert.ToInt32(ddlStatus.SelectedValue) == 0)
                iStatus = 0;

            //added by Koushik das as on 16-Mar-2017
            Session["SupplierEditCompanyID"] = ddlCompany.SelectedValue;
            //added by Koushik das as on 16-Mar-2017

            GetDataGridValues(Convert.ToInt32(ddlCompany.SelectedValue), iSuppid, VClass, VID, iStatus);
        }
        #endregion
        #region btnSearch_Click
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            int iSuppid = 0;
            string VClass = "";
            string VID = "";
            int iStatus = 1;

            //if(ddlSupplier.SelectedValue !="Select")
            //    iSuppid = Convert.ToInt32(ddlSupplier.SelectedValue);

            if (HdSupplierId.Value != "")
                iSuppid = Convert.ToInt32(HdSupplierId.Value);

            if (ddlVendor.SelectedValue != "Select")
                VID = Convert.ToString(ddlVendor.SelectedItem.Text.Trim());
            if (ddlVClass.SelectedValue != "Select")
                VClass = Convert.ToString(ddlVClass.SelectedItem.Text.Trim());
            if (Convert.ToInt32(ddlStatus.SelectedValue) == 0)
                iStatus = 0;

            GetDataGridValues(Convert.ToInt32(ddlCompany.SelectedValue), iSuppid, VClass, VID, iStatus);
        }
        #endregion
        #region btnAddSupplier_Click
        private void btnAddSupplier_Click(object sender, System.EventArgs e)
        {
            Server.Transfer("EditSupplier.aspx?ComID=0");
        }
        #endregion

        #region Datagrid_Click
        protected void Datagrid_Click(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "DELETERECORD")
            {
                if (objSupplier.DeleteSupplierRecord(Convert.ToInt32(e.CommandArgument), Convert.ToInt32(Session["UserID"])))
                {
                    btnSearch_Click(null, null);
                    lblMessage.Text = "Supplier deleted successfully.";
                }
                else
                {
                    lblMessage.Text = "Sorry, you cannot delete this supplier as there are already invoices in the system.";
                }
            }

        }
        #endregion

        [WebMethod]
        public static string[] GetSupplier(string CompanyID, string UserString)
        {
            DataSet dsSupplier = new DataSet();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetSuppliersListFromTradingRelation_New", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyString", UserString);

            try
            {
                sqlConn.Open();
                sqlDA.Fill(dsSupplier);
            }
            catch (Exception ex)
            {
                string strExceptionMessage = ex.Message.Trim();
            }
            finally
            {
                if (sqlDA != null)
                    sqlDA.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }

            List<string> myList = new List<string>();
            if (dsSupplier != null && dsSupplier.Tables.Count > 0 && dsSupplier.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsSupplier.Tables[0].Rows)
                {
                    myList.Add(string.Format("{0}#{1}", row["CompanyID"].ToString(), row["label"].ToString()));
                    //myList.Add(string.Format("{0}", row["label"].ToString()));
                }
                return myList.ToArray();
                // return JsonConvert.SerializeObject(dsSupplier.Tables[0]);
            }
            else
                return null;
            // return "";
        }


    }
}
