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
// Added By Mrinal
using System.Configuration;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;

namespace CBSolutions.ETH.Web
{

    public partial class AboutUs : System.Web.UI.Page
    {
        #region: Web Controls
        /*
        protected System.Web.UI.WebControls.TextBox txtNetworkID;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtPassword;
        protected System.Web.UI.WebControls.Button btnSalesLoginSubmit;
       */
        #endregion
        // ==========================================================================================================	
        #region: User Defined Variables
        private int iParentCompanyID = 0;
        private Users objUser = new Users();
        #endregion
        // ==========================================================================================================
        #region: Page Events
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Error"] != null && Request.QueryString["Error"].ToString() == "1")
                {
                    // Response.Write("<script language=javascript>alert('Invalid Security Info! Please try again.'); </script>");
                    this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Invalid Security Info! Please try again.'); </script>");
                }
            }
            // Put user code to initialize the page here
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
            this.Load += new System.EventHandler(this.Page_Load);
            this.btnSalesLoginSubmit.Click += new System.EventHandler(btnSalesLoginSubmit_Click);
        }
        #endregion
        #region: Button Events
        public void btnSalesLoginSubmit_Click(object sender, EventArgs e)
        {

            if (txtNetworkID.Text.Trim().Length == 0 || txtUserName.Text.Trim().Length == 0 || txtPassword.Text.Trim().Length == 0)
            {
                //lblValidateMessage.Visible = true; 
                txtNetworkID.Text = "";
                txtUserName.Text = "";
                txtPassword.Text = "";
                //  Response.Write("<script language=javascript>alert('Invalid Security Info! Please try again.'); </script>");
                this.RegisterClientScriptBlock("clientScript", "<script language=javascript>alert('Invalid Security Info! Please try again.'); </script>");
            }
            else
            {
                // Added By Mrinal
                string NetworkID = txtNetworkID.Text.Trim().ToString();
                string UserName = txtUserName.Text.Trim().ToString();
                string Password = txtPassword.Text.Trim().ToString();


                //Response.Redirect(ConfigurationManager.AppSettings["P2DIntermediate"].Trim() + "?NetworkID=" + NetworkID + "&UserName=" + UserName + "&Password=" + Password);  
                Response.Redirect(ConfigurationManager.AppSettings["P2DIntermediate"].Trim() + "?NetworkID=" + NetworkID + "&UserName=" + UserName + "&Password=" + Password);

                // Adition End By Mrinal



                /*  ---BLocked by Mrinal on 30th June 2014------------------------


                CBSAppUtils.PrimaryConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                RecordSet rsLogin = da.ExecuteSP("up_security_Login_Encrpyt", txtNetworkID.Text, txtUserName.Text, txtPassword.Text, Encrypt.EncryptData(txtPassword.Text));

           
                rsLogin.ParentTable.TableName = "Users";
                if (rsLogin.ParentDataSet.Tables.Count > 1)
                {
                    //proceed
                    RecordSet rsUser = new RecordSet(rsLogin.ParentDataSet, 1);
                    //store all fields in the session variables
                    for (int i = 0, j = rsUser.ColumnCount; i < j; i++)
                    {
                        string columnName = rsUser.Columns[i].ColumnName;
                        Session.Add(columnName, rsUser[columnName]);
                    }
                    //get the user's security access information and load that into session
                    RecordSet rsAccess = new RecordSet(rsLogin.ParentDataSet, 2);
                    Session.Add("Access", rsAccess);


                    CBSAppUtils.AppUserId = (int)Session["UserID"];
                    Session["GMG"] = 0;
                    RecordSet rsComp = da.ExecuteQuery("vUserCompany", "UserID= " + CBSAppUtils.AppUserId);
                    if (rsComp.RecordCount > 0)
                    {
                        if (rsComp["ParentCompanyID"] == DBNull.Value)
                            iParentCompanyID = 0;
                        else
                            iParentCompanyID = Convert.ToInt32(rsComp["ParentCompanyID"]);

                        if (rsComp["CompanyName"].ToString().ToUpper().Trim() == "GMG" || iParentCompanyID == 14)
                        {
                            Session["CompanyID"] = Convert.ToInt32(rsComp["CompanyID"]);
                            Session["GMG"] = 1;
                        }
                        if (rsComp["ParentCompanyID"] == DBNull.Value)
                            Session["ParentCompanyID"] = 0;
                        else
                            Session["ParentCompanyID"] = Convert.ToInt32(rsComp["ParentCompanyID"]);

                        if (rsComp["UserTypeID"] == DBNull.Value)
                            Session["UserTypeID"] = 1;
                        else
                            Session["UserTypeID"] = Convert.ToInt32(rsComp["UserTypeID"]);

                        if (rsComp["CompanyTypeID"] != DBNull.Value)
                        {
                            Session["CompanyID"] = Convert.ToInt32(rsComp["CompanyID"]);
                            Session["CompanyTypeID"] = Convert.ToInt32(rsComp["CompanyTypeID"]);
                        }
                        else
                            Session["CompanyTypeID"] = 0;
                    }

                    da.CloseConnection();

                    if (objUser.CheckFirstLogin(Convert.ToInt32(Session["UserID"])))
                        Response.Redirect(ConfigurationManager.AppSettings["UserMainPage"].Trim());
                    else
                        Response.Redirect(ConfigurationManager.AppSettings["FirstLoginPage"].Trim());


                }
                else
                {
                    //login failed! to inspect what went wrong, we need 
                    //to extract information from the first table inside
                    //the rsLogin, but we don't need to do that now.
                    //lblValidateMessage.Visible = true;

                    txtNetworkID.Text = "";
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                    Response.Write("<script language=javascript>alert('Invalid Security Info! Please try again.'); </script>");
                }
                 * */
                //da.Connection.Close(); 
            }

        }
        #endregion
    }
}