using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace JKS
{
    /// <summary>
    /// Summary description for CMSHistoryNew.
    /// </summary>
    public class CMSHistoryNew : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Label lblHeader;
        protected System.Web.UI.WebControls.DropDownList ddlCompany;
        protected System.Web.UI.WebControls.ListBox listSiteFrom;
        protected System.Web.UI.WebControls.ListBox listSiteTo;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.Button btnNext;
        protected System.Web.UI.WebControls.Button btnNextAll;
        protected System.Web.UI.WebControls.Button btnBack;
        protected System.Web.UI.WebControls.Button btnBackAll;
        protected System.Web.UI.WebControls.DropDownList ddlWeekStartDate;

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Page.RegisterStartupScript("reg", "<script>alert('Sorry,your Session is expired');</script>");
                Response.Redirect("../../etcdefault.aspx");
            }
            if (!IsPostBack)
            {
                btnSubmit.Attributes.Add("onclick", "return fn_CMSValidation();");
                LoadCompany();
            }
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
            this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
            this.ddlWeekStartDate.SelectedIndexChanged += new System.EventHandler(this.ddlWeekStartDate_SelectedIndexChanged);
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            this.btnNextAll.Click += new System.EventHandler(this.btnNextAll_Click);
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.btnBackAll.Click += new System.EventHandler(this.btnBackAll_Click);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion


        private void LoadCompany()
        {
            Company objCompany = new Company();
            DataTable dt = new DataTable();
            try
            {
                ddlCompany.Items.Clear();
                dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                if (dt.Rows.Count > 0)
                {
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.SelectedValue = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS LoadCompany : <br /> " + ex.Message.ToString());
            }
            finally
            {
                dt = null;
                ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));
                ddlWeekStartDate.Items.Insert(0, new ListItem("Select Week Start Date", "0"));
                ddlCompany.SelectedValue = "0";
            }
        }
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadWeekSartDate();
        }
        private void LoadWeekSartDate()
        {
            ddlWeekStartDate.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_WeekSartDateNew", sqlConn);
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
                    ddlWeekStartDate.DataSource = ds;
                    ddlWeekStartDate.DataBind();
                }
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMSHistory.Aspx   LoadWeekSartDate: <br /> " + ex.Message.ToString());
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
                ddlWeekStartDate.Items.Insert(0, new ListItem("Select Week Start Date", "0"));
            }
        }

        private void ddlWeekStartDate_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadDepartment();
        }
        private void LoadDepartment()
        {
            listSiteFrom.Items.Clear();
            listSiteTo.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentList_CMS", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            sqlDA.SelectCommand.Parameters.Add("@StartWeekDate", Convert.ToString(ddlWeekStartDate.SelectedValue));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    listSiteFrom.DataSource = ds;
                    listSiteFrom.DataTextField = "Department";
                    listSiteFrom.DataValueField = "DepartmentID";
                    listSiteFrom.DataBind();
                    listSiteTo.DataSource = null;
                    listSiteTo.DataTextField = "Department";
                    listSiteTo.DataValueField = "DepartmentID";
                    listSiteTo.DataBind();
                }
            }
            catch (Exception ex)
            {
                ETH.Web.ETC.CMS.CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS.Aspx   LoadDepartment: <br /> " + ex.Message.ToString());
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
            }
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                string DepID = "";
                Session["DepartmentID"] = "";
                Session["weekstartdate"] = Convert.ToString(ddlWeekStartDate.SelectedValue);
                if (listSiteTo.Items.Count > 0)
                {
                    for (int i = 0; i < listSiteTo.Items.Count; i++)
                    {
                        DepID += listSiteTo.Items[i].Value + ",";
                    }
                    Session["DepartmentID"] = DepID.Substring(0, DepID.Length - 1);
                    Response.Redirect("WeeklySummaryNew.aspx");
                }
                else
                {
                    Page.RegisterStartupScript("reg", "<script>alert('Please select atleast one Department');</script>");
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            if (listSiteFrom.SelectedIndex > -1)
            {
                listSiteTo.Items.Add(new ListItem(listSiteFrom.SelectedItem.Text, listSiteFrom.SelectedItem.Value));
                listSiteFrom.Items.Remove(new ListItem(listSiteFrom.SelectedItem.Text, listSiteFrom.SelectedItem.Value));
            }
        }

        private void btnNextAll_Click(object sender, System.EventArgs e)
        {
            int listcount = listSiteFrom.Items.Count;
            for (int i = listcount - 1; i >= 0; i--)
            {
                listSiteFrom.Items[i].Selected = true;
                listSiteTo.Items.Add(new ListItem(listSiteFrom.SelectedItem.Text, listSiteFrom.SelectedItem.Value));
                listSiteFrom.Items.Remove(new ListItem(listSiteFrom.SelectedItem.Text, listSiteFrom.SelectedItem.Value));

            }
        }

        private void btnBackAll_Click(object sender, System.EventArgs e)
        {
            int listcount = listSiteTo.Items.Count;
            for (int i = listcount - 1; i >= 0; i--)
            {
                listSiteTo.Items[i].Selected = true;
                listSiteFrom.Items.Add(new ListItem(listSiteTo.SelectedItem.Text, listSiteTo.SelectedItem.Value));
                listSiteTo.Items.Remove(new ListItem(listSiteTo.SelectedItem.Text, listSiteTo.SelectedItem.Value));
            }
        }

        private void btnBack_Click(object sender, System.EventArgs e)
        {
            if (listSiteTo.SelectedIndex > -1)
            {
                listSiteFrom.Items.Add(new ListItem(listSiteTo.SelectedItem.Text, listSiteTo.SelectedItem.Value));
                listSiteTo.Items.Remove(new ListItem(listSiteTo.SelectedItem.Text, listSiteTo.SelectedItem.Value));
            }
        }
    }
}
