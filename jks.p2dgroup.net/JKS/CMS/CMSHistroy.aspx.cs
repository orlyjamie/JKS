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
    /// Summary description for CMSHistroy.
    /// </summary>
    public class CMSHistroy : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Label lblHeader;
        protected System.Web.UI.WebControls.DropDownList ddlCompany;
        protected System.Web.UI.WebControls.DropDownList ddlDepartment;
        protected System.Web.UI.WebControls.DropDownList ddlWeekStartDate;
        protected System.Web.UI.WebControls.Button btnSubmit;

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
            this.ddlDepartment.SelectedIndexChanged += new System.EventHandler(this.ddlDepartment_SelectedIndexChanged);
            this.ddlWeekStartDate.SelectedIndexChanged += new System.EventHandler(this.ddlWeekStartDate_SelectedIndexChanged);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

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
                ETH.Web.ETC.CMS.CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS.Aspx   LoadDepartment: <br /> " + ex.Message.ToString());
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
                ds = null;
                ddlDepartment.Items.Insert(0, new ListItem("Select Department", "0"));
            }
        }
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
                ddlDepartment.Items.Insert(0, new ListItem("Select Department", "0"));
                ddlWeekStartDate.Items.Insert(0, new ListItem("Select Week Start Date", "0"));
                ddlCompany.SelectedValue = "0";
            }
        }
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            Session["weekstartdate"] = Convert.ToString(ddlWeekStartDate.SelectedValue);
            Response.Redirect("WeeklySummary.aspx?dep=" + CMScode.EncryptData(ddlDepartment.SelectedValue));
        }
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadDepartment();
        }
        private void LoadWeekSartDate()
        {
            ddlWeekStartDate.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_WeekSartDate", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@DepartmentID", Convert.ToInt32(ddlDepartment.SelectedValue));
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
        }
        private void ddlDepartment_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadWeekSartDate();
        }
    }
}
