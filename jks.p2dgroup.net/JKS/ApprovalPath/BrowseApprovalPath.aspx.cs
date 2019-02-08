using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
    /// Summary description for BrowseApprovalPath.
    /// </summary>
    /// 
    [ScriptService]
    public partial class BrowseApprovalPath : System.Web.UI.Page
    {
        #region webControls
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Label Label1;

        //protected System.Web.UI.WebControls.TextBox txtNetFrom;
        //protected System.Web.UI.WebControls.TextBox txtNetTo;		

        public DataSet globds = null;
        public static string strSortOrder = "";
        protected System.Web.UI.WebControls.DropDownList ddlBusinessUnitID;
        public static string strSortField = "";
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            GetAllDropDowns();
            if (!IsPostBack)
            {
                //added by Koushik das as on 16-Mar-2017
                string Current = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                string Referer = (Request.UrlReferrer != null) ? System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath) : Current;

                if (Referer == Current)
                    Session.Remove("ApprovalPathEditCompanyID");

                if (Session["ApprovalPathEditCompanyID"] != null)
                    ddlCompany.SelectedValue = (string)Session["ApprovalPathEditCompanyID"];
                else
                    ddlCompany.SelectedValue = "0";//Convert.ToString(Session["CompanyID"]);

                ddlCompany_SelectedIndexChanged(sender, e);

                btnSearch_Click(sender, e);
                //added by Koushik das as on 16-Mar-2017

                //GetDataGridValues(System.Convert.ToInt32(ddlCompany.SelectedValue),0,"","","");
                GetDataGridValues(System.Convert.ToInt32(ddlCompany.SelectedValue), 0, "", 0, 0);
                if (strSortField != "" && strSortOrder != "")
                    GetAllCategoryByAdmin();

                #region Added by Koushik Das as on 06-Apr-2017 (to fetch from 'DeleteRedirectionSearchValues' Session)
                /*
                 * When you DELETE an approval path please refresh back to the 
                 * previous search like what you did with the other changes.
                 */
                if (Session["DeleteRedirectionSearchValues"] != null)
                {
                    Dictionary<string, dynamic> Values = (Dictionary<string, dynamic>)Session["DeleteRedirectionSearchValues"];

                    ddlCompany.SelectedValue = Values["CompanyID"];
                    ddlCompany_SelectedIndexChanged(sender, e);

                    txtSupplier.Value = Values["SupplierName"];
                    HdSupplierId.Value = Values["SupplierID"];
                    HdSupplierName.Value = Values["HdSupplierName"];
                    ddlVendor.SelectedValue = Values["VendorClassID"];
                    ddlBusinessUnit.SelectedValue = Values["BusinessUnitID"];
                    ddlDepartment.SelectedValue = Values["DepartmentID"];

                    btnSearch_Click(sender, e);

                    Session.Remove("DeleteRedirectionSearchValues");
                }
                #endregion
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
            this.ddlSupplier.SelectedIndexChanged += new System.EventHandler(this.ddlSupplier_SelectedIndexChanged);
            this.btnCoding.Click += new System.EventHandler(this.btnCoding_Click);
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdApprover.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.grdApprover_SortCommand);
            this.Load += new System.EventHandler(this.Page_Load);

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

                GetSupplierDropDown();
                GetVendorClassDropDwons();
                // Added by Mrinal on 24th Feb 2014
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                LoadDepartment();
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
                    //ddlCompany.Items.Insert(0, "Select Company Name");
                    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

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
                    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));
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
                    //ddlCompany.Items.Insert(0, "Select Company Name");   
                    ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));

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

            //blocked for this page as this function was used in Current folder, so no use of supplie list population
            /*JKS.Invoice objInvoice = new JKS.Invoice();
            ddlSupplier.Items.Clear();
            if (ddlCompany.SelectedItem.Text != "Select Company Name")
            {
                ddlSupplier.DataSource = objInvoice.GetSuppliersList(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
            }
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));

            JKS.Invoice_New objInv1 = new JKS.Invoice_New();*/

            GetVendorClassAgainstSupplierANDBuyer();//Added extra in this function for VendorClass

        }
        //---------------------------------------------------------------------
        #region GetCompanyDropDown()
        public void GetCompanyDropDown()
        {
            Approval oApproval = new Approval();
            DataSet ds = new DataSet();
            ds = oApproval.GetCompanyDropDownGeneric(System.Convert.ToInt32(Session["CompanyID"]));
            ddlCompany.DataSource = ds;
            ddlCompany.DataBind();
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyID";
            GetVendorClassAgainstSupplierANDBuyer();
        }
        #endregion

        #region GetSupplierDropDown()
        public void GetSupplierDropDown()
        {
            //Approval oApproval = new Approval();
            //DataSet ds = new DataSet();

            //ds = oApproval.GetSupplierFromTradingRelation(System.Convert.ToInt32(ddlCompany.SelectedValue));
            //ddlSupplier.DataSource=ds;
            //ddlSupplier.DataBind();	
            //ddlSupplier.Items.Insert(0,"Select");
        }
        #endregion

        #region GetVendorClassDropDwons()
        public void GetVendorClassDropDwons()
        {

        }
        #endregion
        #region GetDataGridValues
        //public void GetDataGridValues(int iCompanyID,int iSuppCompanyID,string StrVClass,string strNetFrom,string strNetTo)
        public void GetDataGridValues(int iCompanyID, int iSuppCompanyID, string StrVClass, int strDepartment, int strBusinessUnit)
        {
            Approval objApproval = new Approval();
            DataSet ds = new DataSet();
            //ds = oApproval.GetDetailsFromApprovalPaths_NB(iCompanyID,iSuppCompanyID,StrVClass,strNetFrom,strNetTo);
            ds = objApproval.GetSearchDetailsApprovalPath(iCompanyID, iSuppCompanyID, StrVClass, strDepartment, strBusinessUnit);
            globds = ds;
            //			if(ds != null && ds.Tables[0].Rows.Count>0)
            //			{
            //				globds = ds;
            //				grdApprover.DataSource=ds;
            //				grdApprover.DataBind();	
            //			}
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    grdApprover.DataSource = ds;
                    grdApprover.DataBind();
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
        #region btnSearch_Click
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            SearchResult();
        }
        #endregion

        #region btnAddNew_Click
        private void btnAddNew_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("EditApproval.aspx?ApproverID=" + 0);
        }
        #endregion
        #region btnCoding_Click
        private void btnCoding_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("EditCoding.aspx");
        }
        #endregion

        #region protected void DeleteItem(object sender, System.EventArgs e)
        protected void DeleteItem(object sender, System.EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            int iAppID = Convert.ToInt32(lnkDelete.HRef);
            Approval oApproval = new Approval();

            int iRetValue = oApproval.DeleteItem(iAppID);
            if (iRetValue == 1)
            {
                #region Added by Koushik Das as on 06-Apr-2017 (to create 'DeleteRedirectionSearchValues' Session)
                Dictionary<string, dynamic> Values = new Dictionary<string, dynamic>();

                Values.Add("CompanyID", ddlCompany.SelectedValue);
                Values.Add("SupplierName", txtSupplier.Value);
                Values.Add("SupplierID", HdSupplierId.Value);
                Values.Add("HdSupplierName", HdSupplierName.Value);
                Values.Add("VendorClassID", ddlVendor.SelectedValue);
                Values.Add("BusinessUnitID", ddlBusinessUnit.SelectedValue);
                Values.Add("DepartmentID", ddlDepartment.SelectedValue);

                Session["DeleteRedirectionSearchValues"] = Values;
                #endregion

                Response.Write("<script>alert('Records deleted successfully.'); window.location.href='BrowseApprovalPath.aspx';</script>");
                lblMessage.Text = "Records deleted successfully.";
            }
            else
                lblMessage.Text = "Error deleting records.";
        }
        #endregion

        private void ddlSupplier_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
        #region GetVendorClassAgainstSupplierANDBuyer
        private void GetVendorClassAgainstSupplierANDBuyer()
        {
            int iBuyerCID = 0;
            //			int iSupplierCID = 0;
            if (ddlCompany.Text != "Select Company Name")//Added by Mainak 2018-11-29
            {

                iBuyerCID = Convert.ToInt32(ddlCompany.SelectedValue);
                Approval oApproval = new Approval();
                DataSet ds = new DataSet();
                string Fields = "DISTINCT New_VendorClass AS  VendorClass";
                string Table = "TradingRelation";
                string Criteria = "BuyerCompanyID  = " + iBuyerCID + " AND New_VendorClass IS NOT NULL";
                ds = oApproval.GetGlobalDropDowns(Fields, Table, Criteria);
                ddlVendor.DataSource = ds;
                ddlVendor.DataBind();
                ddlVendor.Items.Insert(0, "Select");
            }
        }
        #endregion

        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlCompany.Text != "Select Company Name")//Added by Mainak 2018-11-29
            {
                txtSupplier.Value = "";
                GetSupplierDropDown();
                GetVendorClassAgainstSupplierANDBuyer();
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                LoadDepartment();
                SearchResult();
                //added by Koushik das as on 16-Mar-2017
                Session["ApprovalPathEditCompanyID"] = ddlCompany.SelectedValue;
                //added by Koushik das as on 16-Mar-2017
            }
        }


        #region GetAllCategoryByAdmin()
        public void GetAllCategoryByAdmin()
        {
            try
            {
                DataView dvgrdInvCur = new DataView(globds.Tables[0]);
                dvgrdInvCur.Sort = strSortField + " " + strSortOrder;
                grdApprover.DataSource = dvgrdInvCur;
                grdApprover.DataBind();
                grdApprover.Visible = true;
            }
            catch (Exception EX)
            {
                Response.Write("<script>alert('ERROR : " + EX.Message.ToString() + "')</script>");
            }
        }
        #endregion

        #region Sorting
        protected void grdApprover_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            if (strSortOrder == "ASC")
                strSortOrder = "DESC";
            else
                strSortOrder = "ASC";

            strSortField = e.SortExpression;
            GetDataGridValues(System.Convert.ToInt32(ddlCompany.SelectedValue), 0, "", 0, 0);
            GetAllCategoryByAdmin();
        }
        #endregion

        private void SearchResult()
        {
            int iSuppCompanyID = 0;

            //if(Convert.ToString(ddlSupplier.SelectedValue)=="Select")
            //    iSuppCompanyID =0;
            //else 
            //    iSuppCompanyID = Convert.ToInt32(ddlSupplier.SelectedValue);

            // if (txtSupplier.Value.ToString().Trim().Length > 0)
            // {
            if (HdSupplierId.Value != "")
                iSuppCompanyID = Convert.ToInt32(HdSupplierId.Value);
            //  }
            else
                iSuppCompanyID = 0;

            string StrVClass = "";
            if (Convert.ToString(ddlVendor.SelectedItem.Value) != "Select")
                StrVClass = Convert.ToString(ddlVendor.SelectedItem.Text);


            //	string strNetFrom = txtNetFrom.Text.Trim();
            //	string strNetTo = txtNetTo.Text.Trim();

            //int ICompID = System.Convert.ToInt32(Session["CompanyID"]);
            int ICompID = Convert.ToInt32(ddlCompany.SelectedValue);

            int DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
            int BusinessUnitID = Convert.ToInt32(ddlBusinessUnit.SelectedValue);


            GetDataGridValues(ICompID, iSuppCompanyID, StrVClass, DepartmentID, BusinessUnitID);
        }

        #region: New Implementation on 24thFebruary 2014
        private void LoadDepartment()
        {
            ddlDepartment.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentList_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDepartment.DataSource = ds;
                    ddlDepartment.DataBind();
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
                ddlDepartment.Items.Insert(0, new ListItem("Select Department", "0"));
            }

        }
        private void GetBusinessUnit(int companyid)
        {
            ddlBusinessUnit.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetBusinessUnit", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(companyid));
            sqlDA.SelectCommand.Parameters.Add("@UsrID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBusinessUnit.DataSource = ds;
                    ddlBusinessUnit.DataValueField = "BusinessUnitID";
                    ddlBusinessUnit.DataTextField = "BusinessUnitName";
                    ddlBusinessUnit.DataBind();
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
            }
            ddlBusinessUnit.Items.Insert(0, new ListItem("Select Business Unit", "0"));
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

                }
                return myList.ToArray();
            }
            else
                return null;
            // return "";
        }
    }
}