#region directives
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
using System.Net;
using System.Web.Mail;
using System.Collections.Generic;
#endregion

namespace JKS
{
    /// <summary>
    /// Summary description for EditCoding.
    /// </summary>
    public partial class EditCoding : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region Weebcontrols
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Label Label1;

        public int SCounter = 0;

        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            btnSubmit.Attributes.Add("onclick", "javascript:fn_Validate();");
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");
            if (!Page.IsPostBack)
            {
                GetAllDropDowns();
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);

            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);

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
                    ddlCompany.Items.Insert(0, "Select Company Name");

                    ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                }

                //modified by kuntal on 29thApril2015
                else if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    string LogInCompanyid = "";
                    dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, "Select Company Name");
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
                    ddlCompany.Items.Insert(0, "Select Company Name");

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

            // GetVendorClassAgainstSupplierANDBuyer();//Added extra in this function for VendorClass

        }
        //---------------------------------------------------------------------
        #region GetAllDropDowns()
        public void GetAllDropDowns()
        {
            GetCompanyDropDown();
            /*Added by kuntalkarar on 27thMay2016*/
            GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
            //--------------------------------------
        }
        #endregion

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

        }
        #endregion

        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            int RetVal = 0;
            int Company = 0;
            string Department = "";
            string Nominal = "";
            string Coding = "";
            string Project = "";
            Approval oApproval = new Approval();

            Company = Convert.ToInt32(ddlCompany.SelectedValue);
            Department = txtDept.Text.ToString();
            Nominal = txtNominal.Text.ToString();
            Coding = txtCodingDes.Text.ToString();
            Project = txtProject.Text.ToString();

            if (Department == "" && Nominal == "" && Coding == "" && Project == "")
            {
                lblMessage.Text = "Please insert atleast one more field.";
                Response.Write("<script>alert('Please insert atleast one more field.');</script>");
                return;
            }
            if (Department != "" && Nominal != "" && Coding == "")
            {
                lblMessage.Text = "Please enter a Coding Description.";
                Response.Write("<script>alert('Please enter a Coding Description.');</script>");
                return;
            }
            if (Department == "" && Nominal == "" && Coding != "")
            {
                lblMessage.Text = "Please enter Department AND Nominal.";
                Response.Write("<script>alert('Please enter Department AND Nominal.');</script>");
                return;
            }
            if (Department != "" && Nominal == "" && Coding != "")
            {
                lblMessage.Text = "Please enter Department AND Nominal.";
                Response.Write("<script>alert('Please enter Department AND Nominal.');</script>");
                return;
            }
            if (Department == "" && Nominal != "" && Coding != "")
            {
                lblMessage.Text = "Please enter Department AND Nominal.";
                Response.Write("<script>alert('Please enter Department AND Nominal.');</script>");
                return;
            }
            int Count = 0;
            Count = oApproval.DataExistsInTable(Company, Department, Nominal);
            if (Count > 0)
            {
                Page.RegisterStartupScript("Reg", "<script>ConfirmResubmit();</script>");
            }
            else
            {
                btnConfirm_Click(sender, e);
            }



            //			if(Count >0)
            //			{
            //				if(ViewState["COUNT"]== null)
            //				{
            //					lblMessage.Text = "This code combination is already in the system. Please continue if you wish to change the Coding Description.";
            //					Response.Write("<script>alert('This code combination is already in the system. Please continue if you wish to change the Coding Description.');</script>");
            //					ViewState["COUNT"]="1";
            //					return;
            //				}
            //			}
            //
            //			RetVal = oApproval.InsertDataIntoCodingDescription(Company,Department,Nominal,Project,Coding,ddlAPAdmin.SelectedValue.ToString());
            //			if(RetVal == 101)
            //			{
            //				lblMessage.Text = "Project/Capex inserted successfully.";
            //				Response.Write("<script>alert('Project/Capex inserted successfully.');</script>");				
            //			}
            //			else if(RetVal == 1011)
            //			{
            //				lblMessage.Text = "Project/Capex already exists.";
            //				Response.Write("<script>alert('Project/Capex already exists.');</script>");				
            //			}
            //			else if(RetVal == 102)
            //			{
            //				lblMessage.Text = "Nominal Code inserted successfully.";
            //				Response.Write("<script>alert('Nominal Code inserted successfully.');</script>");				
            //			}
            //			else if(RetVal == 1021)
            //			{
            //				lblMessage.Text = "Nominal Code already exists.";
            //				Response.Write("<script>alert('Nominal Code already exists.');</script>");				
            //			}
            //			else if(RetVal == 103)
            //			{
            //				lblMessage.Text = "Department inserted successfully.";
            //				Response.Write("<script>alert('Department inserted successfully.');</script>");				
            //			}
            //			else if(RetVal == 1031)
            //			{
            //				lblMessage.Text = "Department already exists.";
            //				Response.Write("<script>alert('Department already exists.');</script>");				
            //			}
            //			else if(RetVal == 105)
            //			{
            //				lblMessage.Text = "Record(s) saved successfully.";
            //				Response.Write("<script>alert('Record(s) saved successfully.');</script>");				
            //			}
            //			else if(RetVal == 106)
            //			{
            //				lblMessage.Text = "Existing record(s) updated successfully.";
            //				Response.Write("<script>alert('Existing record(s) updated successfully.');</script>");				
            //			}

        }
        #endregion

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Server.Transfer("BrowseApprovalPath.aspx");
        }


        public void btnConfirm_Click(object sender, System.EventArgs e)
        {



            if (SCounter > 0)
            {
                return;
            }
            SCounter++;
            int RetVal = 0;
            int Company = 0;
            string Department = "";
            string Nominal = "";
            string Coding = "";
            string Project = "";
            Approval oApproval = new Approval();

            Company = Convert.ToInt32(ddlCompany.SelectedValue);
            Department = txtDept.Text.ToString();
            Nominal = txtNominal.Text.ToString();
            Coding = txtCodingDes.Text.ToString();
            Project = txtProject.Text.ToString();


            RetVal = oApproval.InsertDataIntoCodingDescription(Company, Department, Nominal, Project, Coding, ddlAPAdmin.SelectedValue.ToString());
            if (RetVal == 101)
            {
                lblMessage.Text = "Project/Capex inserted successfully.";
                Response.Write("<script>alert('Project/Capex inserted successfully.');</script>");
            }
            else if (RetVal == 1011)
            {
                lblMessage.Text = "Project/Capex already exists.";
                Response.Write("<script>alert('Project/Capex already exists.');</script>");
            }
            else if (RetVal == 102)
            {
                lblMessage.Text = "Nominal Code inserted successfully.";
                Response.Write("<script>alert('Nominal Code inserted successfully.');</script>");
            }
            else if (RetVal == 1021)
            {
                lblMessage.Text = "Nominal Code already exists.";
                Response.Write("<script>alert('Nominal Code already exists.');</script>");
            }
            else if (RetVal == 103)
            {
                lblMessage.Text = "Department inserted successfully.";
                Response.Write("<script>alert('Department inserted successfully.');</script>");
            }
            else if (RetVal == 1031)
            {
                lblMessage.Text = "Department already exists.";
                Response.Write("<script>alert('Department already exists.');</script>");
            }
            else if (RetVal == 105)
            {
                lblMessage.Text = "Record(s) saved successfully.";
                Response.Write("<script>alert('Record(s) saved successfully.');</script>");
            }
            else if (RetVal == 106)
            {
                lblMessage.Text = "Existing record(s) updated successfully.";
                Response.Write("<script>alert('Existing record(s) updated successfully.');</script>");
            }


        }


    }
}
