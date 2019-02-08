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
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using DataDynamics.ActiveReports.Export;
using DataDynamics.ActiveReports.Document;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web.Mail;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using CBSolutions.ETH.Web;
using System.Linq;


namespace JKS
{
    [ScriptService]
    public partial class InvoiceActionTiffViewer : System.Web.UI.Page
    {

        #region WebControls
        protected CBSolutions.ETH.Web.Invoice objInvoice = new CBSolutions.ETH.Web.Invoice();
        protected Company objCompany = new Company();
        Invoice_NL_CN objCN = new Invoice_NL_CN();//Added by Mainak 2018-08-11
        private JKS.Users objUser = new JKS.Users();
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        protected SqlDataAdapter sqlDA = null;//Added Mainak 2018-08-10
        SqlConnection sqlConn;
        SqlCommand sqlCmd;
        JKS.Invoice objinvoice = new JKS.Invoice();
        protected string AuthorisationStringToolTips = "";
        public int invoiceID = 0;
        int iApproverStatusID = 0;
        string strNewUserCode = "";
        string strNewUserGroup = "";
        string strPassedNewUserCode = "";
        string strPassedNewUserGroup = "";
        string sdApprover1 = "";
        string sdApprover2 = "";
        string sdApprover3 = "";
        string sdApprover4 = "";
        string sdApprover5 = "";
        protected int iCurrentStatusID = 0;
        public int status = 0;
        protected int TypeUser = 1;
        protected int RejectOpenFields = 0;
        protected int ReopenAtApprover = 0;
        protected string oldComment = "";
        protected string strAmountLimit = "";
        protected string strTimeLimit = "";
        protected double dTotalAmount = 0;
        /*  protected System.Web.UI.WebControls.Button btnCancel;*/

        protected System.Web.UI.HtmlControls.HtmlGenericControl Description;
        double dNetAmt = 0;
        double dCodingVat = 0;
        protected int iSupplierCompanyID = 0;
        protected string G2App = "";
        protected string InvoiceID = "";
        int BuyerID = 0;
        private String strChildBuyer = "";
        protected string CreditNoteID = "";
        protected string strBuyerCompanyID = "";
        protected string strSupplierCompanyID = "";
        protected string sDisplay = "none";
        // public string strStatusLogLink = "";


        string strRelationType = "";


        protected int IsCalledFromOpenButton = 0;
        #endregion
        // added by kd
        public int ChkUserID = 0;

        int Check = 0;
        public int vRFlag = 0;//Added by Mainak 2018-09-10

        bool test = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["test"].ToString());

        #region Page_Load
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //Added by Mainak 2018-08-16
            overlay1.Visible = false;
            dialog1.Visible = false;

            //Added by Mainak 2018-09-10

            overlayApprove.Visible = false;
            dialogApprove.Visible = false;

            //==============Added By Rimi on 22nd July 2015===========================================
            if (Request.QueryString["MsgFlag"] == "1")
            {
                if (Request.QueryString["MSG"] == "Reject")
                {
                    //Request.QueryString["MSG"] = "";
                    string message = "alert('Credit Note Rejected Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                if (Request.QueryString["MSG"] == "Approve")
                {
                    //Request.QueryString["MSG"] = "";
                    string message = "alert('Credit Note Approved Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }

                if (Request.QueryString["MSG"] == "Open")
                {
                    //Request.QueryString["MSG"] = "";
                    string message = "alert('Credit Note opened Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }

                if (Request.QueryString["MSG"] == "ReOpen")
                {
                    // Request.QueryString["MSG"] = "";
                    string message = "alert('Credit Note Reopened Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                if (Request.QueryString["MSG"] == "Delete")
                {
                    // Request.QueryString["MSG"] = "";
                    string message = "alert('Credit Note Deleted Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                System.Reflection.PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                isreadonly.SetValue(this.Request.QueryString, false, null);

                this.Request.QueryString.Remove("MSG");
                this.Request.QueryString.Remove("MsgFlag");

            }
            //==============Added By Rimi on 22nd July 2015 End===========================================


            Session["PageRedirect"] = "yes"; // Added By Rimi On 17th July 2015
            IsCalledFromOpenButton = 0;
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            // btnCancel.Attributes.Add("onclick", "javascript:windowclose();");//Commentd by Subhrajyoti on 0/03/2015

            if (Request["DDCompanyID"] != null)
                Session["DropDownCompanyID"] = Request["DDCompanyID"].ToString();
            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
                //added by kuntalkarar on 12thJanuary2017
                Session["InvoiceId_GoToStockQC"] = invoiceID;
                //----------------------------------------
                ViewState["InvoiceID"] = invoiceID.ToString();
                Session["eInvoiceID"] = invoiceID.ToString();
                Session["InvoiceBuyerCompany"] = GetInvoiceBuyerCompanyID1(Convert.ToInt32(Session["eInvoiceID"]));
            }
            TypeUser = Convert.ToInt32(Session["UserTypeID"]);
            lblG2App.Text = "";
            //btnReopen.Attributes.Add("onclick", "return CheckOpenValid('" + TypeUser + "');");//Commeneted By Rimi on 23rd July 2015
            // btnOpen.Attributes.Add("onclick", "return CheckOpenValid('" + TypeUser + "');");//Commeneted By Rimi on 23rd July 2015
            if (ViewState["lblG2App"] != null)
            {
                lblG2App.Text = ViewState["lblG2App"].ToString();
            }
            Session["det"] = "1";

            //blocked by kuntalkarar on 19thOctober2016
            //LoadDownloadFiles();

            //Added By Kd 06.12.2018 
            JKS.Invoice objInvoice = new JKS.Invoice();
            ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            dgSalesCallDetails.CurrentPageIndex = 0;           
            if (!Page.IsPostBack)
            {
                //changed by kuntal karar on 21stOctober2016- Sessions created here
                Session["NextInvoiceID"] = "0";
                Session["NextBuyerCompanyID"] = "0";
                //Added by kuntalkarar on 19thOctober2016
                LoadDownloadFiles();

                Int32 StatusId = 0;
                DataSet dss = new DataSet();
                if (Session["creninv"] != null && Session["creninv"] != "0")
                {
                    dss = GetDocumentDetails(Convert.ToInt32(Session["creninv"]), "INV");
                }
                else
                {
                    dss = GetDocumentDetails(Convert.ToInt32(Session["eInvoiceID"]), "INV");
                }

                StatusId = Convert.ToInt32(dss.Tables[0].Rows[0]["StatusId"]);

                if (Convert.ToInt32(StatusId) != 20 && Convert.ToInt32(StatusId) != 21 && Convert.ToInt32(StatusId) != 22 && Convert.ToInt32(StatusId) != 6)
                {
                    Response.Redirect("../Current/CRNclosewindows.aspx");
                }

                //===============Added By Subhrajyoti on 3rd August 2015===========================
                //if (Session["creninv"] != "")
                if (Session["creninv"] != "" && Session["creninv"] != "0" && Session["creninv"] != "")
                {
                    //26-06-2015
                    ViewState["CheckList"] = Convert.ToInt32(Session["creninv"]);
                    Session["eInvoiceID"] = Convert.ToInt32(Session["creninv"]);
                    Session["InvoiceBuyerCompany"] = GetInvoiceBuyerCompanyID1(Convert.ToInt32(Session["eInvoiceID"]));
                    PopulateDropDowns();
                    // ViewState["CheckList"] = 1;
                    MoveToNext(Convert.ToInt32(Session["creninv"]));
                    ViewState["InvoiceID"] = Convert.ToInt32(Session["creninv"]);
                    ViewState["Counter"] = Session["IndexforINV"];
                    //26-06-20156
                }

                else
                {
                    ViewState["InvoiceChecking"] = 0;//kk
                    ViewState["CheckList"] = 0;//Added by Rimi on 25.06.2015
                    // ViewState["CheckList"] = Check;// commented by Rimi on 25.06.2015
                    // Added by Mrinal on 24th December 2014
                    Session["IsProcessed"] = null;
                    // Added by Mrinal on 10th November 2014
                    dNetAmt = 0;
                    // ViewState["NetAmt"] = null;

                    ViewState["approvalpath"] = "";
                    ShowFiles(Convert.ToInt32(Request["InvoiceID"]));
                    //lblcreditnoteno.Text = ShowMultipleCredits(Convert.ToInt32(Request["InvoiceID"]));
                    PopulateDropDowns();
                    if (invoiceID != 0)
                    {
                        GetDocumentDetails(invoiceID);


                        DataSet ds = GetDocumentDetails(invoiceID, "INV");
                        Boolean Duplicate = Convert.ToBoolean(ds.Tables[0].Rows[0]["Duplicate"]);
                        if (Duplicate == false)
                        {
                            lblDuplicate.Visible = false;
                        }
                        else
                        {
                            lblDuplicate.Visible = true;
                        }
                        /*

                        /*
                       string strStatusLogLink = GetInvoiceStatusLog();
                       strStatusLogLink = GetInvoiceStatusLog();
                       iframeInvoiceStatusLog.Attributes.Add("src", strStatusLogLink);
                       */

                        string strStatusLogLink = GetInvoiceStatusLog();
                        strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:550,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                        //Blocked By Kd 06.12.2018
                        //aInvoiceStatusLog.Attributes.Add("onclick", strStatusLogLink);

                        InvoiceCrnIsDuplicate();
                        IsAutorisedtoEditData();
                    }

                    Session["loaded"] = "0";
                    CheckInvoiceExist();
                    //   CheckNextInvoiceExist(Convert.ToInt32(Request["InvoiceID"]));

                    if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                    {
                        int iRetVal = 0;

                        iRetVal = objInvoice.CheckPassedToUserID(Convert.ToInt32(ViewState["InvoiceID"]), Convert.ToInt32(Session["UserID"]));

                        if (iRetVal == 0)
                        {

                        }
                    }


                    string TiffUrl = "../../TiffViewerDefault.aspx?ID=" + invoiceID.ToString() + "&Type=" + "INV";
                    TiffWindow.Attributes.Add("src", TiffUrl);

                    if (ddldept.SelectedItem.Text != "Select")
                    {
                        ViewState["ddlDept_NR"] = ddldept.SelectedItem.Text;
                    }
                    else
                    {
                        ViewState["ddlDept_NR"] = "Null";
                    }
                    if (ddlApprover1.SelectedItem.Text != "Select")
                    {
                        ViewState["Approver1_NR"] = ddlApprover1.SelectedItem.Text;
                    }
                    else
                    {
                        ViewState["Approver1_NR"] = "Null";
                    }

                    if (ddlApprover2.SelectedItem.Text != "Select")
                    {
                        ViewState["Approver2_NR"] = ddlApprover2.SelectedItem.Text;
                    }
                    else
                    {
                        ViewState["Approver2_NR"] = "Null";
                    }

                    if (ddlApprover3.SelectedItem.Text != "Select")
                    {
                        ViewState["Approver3_NR"] = ddlApprover3.SelectedItem.Text;
                    }
                    else
                    {
                        ViewState["Approver3_NR"] = "Null";
                    }

                    if (ddlApprover4.SelectedItem.Text != "Select")
                    {
                        ViewState["Approver4_NR"] = ddlApprover4.SelectedItem.Text;
                    }
                    else
                    {
                        ViewState["Approver4_NR"] = "Null";
                    }
                    if (ddlApprover5.SelectedItem.Text != "Select")
                    {
                        ViewState["Approver5_NR"] = ddlApprover5.SelectedItem.Text;
                    }
                    else
                    {
                        ViewState["Approver5_NR"] = "Null";
                    }
                    ////modified on 2ndJune2015
                }

                GetVatAmount();//uncommented by Rimi on 25/06/2015
                CalculateTotal();//uncommented by Rimi on 25/06/2015

                ButtonVisibility();
                GetUserCodesAndUserGroupsByUserID();
                tabIndexSet();
                //added by kuntalkarar on 6thJanuary2017
                //if (Request.QueryString["NewVendorClass"] != null)
                //{
                //    if (Convert.ToString(Request.QueryString["NewVendorClass"]).Trim() != "PO")
                //    {
                //        //lnkVariance.Visible = false;
                //        btnRematch.Visible = true;
                //    }
                //}                
            }


        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //GetVatAmount();
            //CalculateTotal();

        }
        //blocked by subhrajyoti on 13thjune2015
        protected void grdList_ItemCreated(object source, RepeaterItemEventArgs e)
        {
            ScriptManager scriptMan = ScriptManager.GetCurrent(this);

            DropDownList ddlBuyerCompanyCode = (DropDownList)e.Item.FindControl("ddlBuyerCompanyCode");
            TextBox txtLineVAT = (TextBox)e.Item.FindControl("txtLineVAT");//Added By Rimi on 22nd July 2015
            TextBox txtNetVal = (TextBox)e.Item.FindControl("txtNetVal");//Added By Rimi on 22nd July 2015
           
            if (ddlBuyerCompanyCode != null)
            {
                ddlBuyerCompanyCode.SelectedIndexChanged += SelectedIndexChanged_ddlBuyerCompanyCode;
                scriptMan.RegisterAsyncPostBackControl(ddlBuyerCompanyCode);
                //Added By Rimi on 22nd July 2015
                txtLineVAT.TextChanged += txtLineVAT_TextChanged;
                scriptMan.RegisterAsyncPostBackControl(txtLineVAT);
                txtNetVal.TextChanged += txtNetVal_TextChanged;
                scriptMan.RegisterAsyncPostBackControl(txtNetVal);
                //Added By Rimi on 22nd July 2015 End
                //Added By Rimi on 22nd July 2015 End txtNetVal_TextChanged
               
            }

        }

        #endregion



        //Added by KD , 06.12.2018
        protected void Popup_Click(object sender, EventArgs e)
        {
            string strInvoiceID = "";
            if (Session["NewInvoiceId"] != null)
            {
                 strInvoiceID = Session["NewInvoiceId"].ToString(); // modified by kd on 04/01/2019
            }
             if (strInvoiceID != "")
             {
                 mpe.Show();
                 this.GetInvoiceStatusDetails_INV(Convert.ToInt32(strInvoiceID));
             }
             else
             {
            if (Convert.ToInt64(ViewState["InvoiceChecking"]) == 0)
            {
                strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);
            }
            else
            {
                strInvoiceID = Convert.ToString(ViewState["InvoiceChecking"]);
            }

            mpe.Show();
            this.GetInvoiceStatusDetails_INV(Convert.ToInt32(strInvoiceID));
             }
            
        }

        //Added By Kd 06.12.2018

        #region GetInvoiceStatusDetails_INV
       // private int ChkUserID = 0;
        private DataTable dtbl = new DataTable();
        private void GetInvoiceStatusDetails_INV(int iInvoiceID)
        {
            DataTable dtbl = new DataTable();


            lblauthstring.Text = "";
            lblDepartment.Text = "";
            lblBusinessUnit.Text = "";

            JKS.Invoice objInvoice = new JKS.Invoice();
            //lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "INV");
            //lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "INV");
            //lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "INV");
           // ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)

                //int invid = Convert.ToInt32(iInvoiceID);

                dtbl = objInvoice.GetInvoiceLogStatusApproverWise(iInvoiceID);

            else
                dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier(iInvoiceID);

            if (dtbl.Rows.Count > 0)
            {

                //dgSalesCallDetails_CRN.Visible = false;
                dgSalesCallDetails.Visible = true;
                dgSalesCallDetails.DataSource = dtbl;
                dgSalesCallDetails.DataBind();
                lblauthstring.Text = dtbl.Rows[0]["AuthorisationString"].ToString();
                lblDepartment.Text = dtbl.Rows[0]["DepartmentName"].ToString();
                lblBusinessUnit.Text = dtbl.Rows[0]["BusinessUnitName"].ToString();
            }
            else
            {
                dgSalesCallDetails.Visible = false;
                //lblMessage.Text = "Sorry, this document has no previous actions.";
            }
        }
        #endregion


        //Added By Kd 06.12.2018
        protected void dgSalesCallDetails_PageIndexChanged1(object source, DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < dgSalesCallDetails.PageCount)
            {
                mpe.Show();
                this.dgSalesCallDetails.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                mpe.Show();
                this.dgSalesCallDetails.CurrentPageIndex = dgSalesCallDetails.PageCount;
            }
            GetInvoiceStatusDetails_INV(Convert.ToInt32(ViewState["InvoiceID"]));
        }



        private DataSet GetBusinessUnit(int companyid)
        {

            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetBusinessUnit_JKS", sqlConn);//Modified by Mainak 2018-10-02
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(companyid));
            sqlDA.SelectCommand.Parameters.Add("@UsrID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlDA.Dispose();
            }
            return ds;
        }
        private DataSet LoadDepartment(int companyid)
        {
            //ddldept.Items.Clear();
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_DepartmentList_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(companyid));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataBind();
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
                //ds=null;
            }
            //ddldept.Items.Insert(0, new ListItem("Select Department", "0"));
            return ds;
        }
        #region TabIndexSet
        /// <summary>
        /// Create By:Rinku Santra
        /// Create Date:26-03-2011
        /// Set tabIndex of grid items and below dynamically
        /// </summary>
        private void tabIndexSet()
        {

            short count = 1;
            for (int i = 0; i < grdList.Items.Count; i++)
            {

                DropDownList ddlBuyerCompanyCode = ((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode"));
                ddlBuyerCompanyCode.TabIndex = count++;
                //DropDownList ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1"));
                //ddlDepartment1.TabIndex = count++;
                //DropDownList ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1"));
                //ddlNominalCode1.TabIndex = count++;
                //DropDownList ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1"));
                //ddlCodingDescription1.TabIndex = count++;
                DropDownList ddlBusinessUnit = ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit"));
                ddlBusinessUnit.TabIndex = count++;
                TextBox txtNetVal = ((TextBox)grdList.Items[i].FindControl("txtNetVal"));
                txtNetVal.TabIndex = count++;

            }
            ddldept.TabIndex = count++;
            ddlApprover1.TabIndex = count++;
            ddlApprover2.TabIndex = count++;
            ddlApprover3.TabIndex = count++;
            txtComment.TabIndex = count++;
            ddlRejection.TabIndex = count++;
            tbRejection.TabIndex = count++;
            txtCreditNoteNo.TabIndex = count++;

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
            this.lnkCrn.Click += new System.EventHandler(this.lnkCrn_Click);
            //this.grdList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdList_ItemDataBound);
            this.grdList.ItemDataBound += new RepeaterItemEventHandler(grdList_ItemDataBound);
            // this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            this.btnDelLine.Click += new System.EventHandler(this.btnDelLine_Click);
            this.btnSaveLine.Click += new EventHandler(this.btnSaveLine_Click);
            this.ddldept.SelectedIndexChanged += new System.EventHandler(this.ddldept_SelectedIndexChanged);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            this.btnReopen.Click += new System.EventHandler(this.btnReopen_Click);
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            //this.grdFile.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdFile_ItemCommand);
            //this.grdFile.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdFile_ItemDataBound);

            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
        #region grdList_ItemDataBound
        void grdList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                //int j = e.Item.DataSetIndex + 1;
                //e.Item.Cells[0].Text = j.ToString();
                int j = e.Item.ItemIndex + 1;

                ((Label)e.Item.FindControl("lblLineNo")).Text = j.ToString();


                // Added by Mrinal on 18th March 2015
                //  ((TextBox)e.Item.FindControl("txtLineDescription")).Text = "";
                //    ((TextBox)e.Item.FindControl("txtLineVAT")).Text = "0.00";

                // Addition End




                DataTable dt = null;


                dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataTextField = "CompanyName";
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataValueField = "CompanyID";
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "--Select--");

                GetAllComboCodesFirstTime();

                try
                {
                    if (Request["DDCompanyID"] != null)
                        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = Session["BuyerCID"].ToString().Trim();
                }
                catch { }
                ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Clear();
                DataSet dsBusinessUnit = new DataSet();



                //   dsBusinessUnit = GetBusinessUnit(Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));

                if (((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim() == "--Select--")
                {

                    dsBusinessUnit = GetBusinessUnit(Convert.ToInt32(Session["CompanyID"]));

                }
                else
                {

                    dsBusinessUnit = GetBusinessUnit(Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));
                }


                if (dsBusinessUnit.Tables[0].Rows.Count > 0)
                {

                    ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataSource = dsBusinessUnit;
                    ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataTextField = "BusinessUnitName";
                    ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataValueField = "BusinessUnitID";
                    ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataBind();

                }
                ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Insert(0, new ListItem("--Select--", "0"));



                if (((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim() != "")
                {
                    dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim());
                }


                if (((TextBox)e.Item.FindControl("txtPoNumber")).Text != "")
                {
                    if (GetPONumberForSupplierBuyer(((TextBox)e.Item.FindControl("txtPoNumber")).Text) != "Y")
                    {

                        ((TextBox)e.Item.FindControl("txtPoNumber")).ForeColor = Color.Red;

                    }
                    else
                    {
                        ((TextBox)e.Item.FindControl("txtPoNumber")).ForeColor = Color.Black;
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                GetVatAmount();
                CalculateTotal();

                //double dVariance = 0;
                //((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetVal")).Text = dNetAmt.ToString();
                //if (ViewState["NetAmt"] != null)
                //{
                //    ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetInvoiceTotal")).Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString();
                //    dVariance = Convert.ToDouble(ViewState["NetAmt"].ToString()) - dNetAmt;
                //}
                //else 
                //{
                //    dVariance = dNetAmt * (-1);
                //}
                //((System.Web.UI.WebControls.Label)e.Item.FindControl("lblVariance")).Text = dVariance.ToString();                

            }

        }

        protected void CalculateTotal()
        {
            double dVariance = 0;

            dNetAmt = 0;
            dCodingVat = 0;

            foreach (RepeaterItem ri in grdList.Items)
            {
                if (((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text.Trim() != "")
                {
                    dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text.Trim());
                    ((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text = Convert.ToDouble(((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text.Trim()).ToString("0.00");// Added By RImi on 22nd July 2015
                }
                if (((System.Web.UI.WebControls.TextBox)ri.FindControl("txtLineVAT")).Text.Trim() != "")
                {
                    double dLineVatValue = 0;
                    dLineVatValue = Convert.ToDouble(((System.Web.UI.WebControls.TextBox)ri.FindControl("txtLineVAT")).Text.Trim());
                    dCodingVat = dCodingVat + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)ri.FindControl("txtLineVAT")).Text.Trim());
                    ViewState.Add("Codingvat", dCodingVat);
                    ((System.Web.UI.WebControls.TextBox)ri.FindControl("txtLineVAT")).Text = dLineVatValue.ToString("0.00");
                    // ((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text = dNetAmt.ToString("0.00");// Added By Rimi on 22nd July2015
                }
            }




            lblNetVal.Text = dNetAmt.ToString("0.00");
            if (ViewState["NetAmt"] != null)
            {
                //lblNetInvoiceTotal.Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString("0.00");// commented by Rimi on 26.06.2015
                lblNetInvoiceTotal.Text = Convert.ToDouble(ViewState["CodingValue"].ToString()).ToString("0.00");// Added by Rimi on 26.06.2015
                //dVariance = Convert.ToDouble(ViewState["NetAmt"].ToString()) - dNetAmt;// commented by Rimi on 26.06.2015
                dVariance = Convert.ToDouble(ViewState["CodingValue"].ToString()) - dNetAmt;// Added by Rimi on 26.06.2015
            }
            else
            {
                dVariance = dNetAmt * (-1);
            }
            lblVariance.Text = dVariance.ToString("0.00");

            //// Added by Mrinal On 18th March 2015
            //lblTotalCodingVATValue.Text = dCodingVat.ToString("0.00");
            //lblVATVariance.Text = (dVariance - dCodingVat).ToString("0.00");

            // Added by Mrinal On 19th March 2015
            lblTotalCodingVATValue.Text = dCodingVat.ToString("0.00");
            double dTotalVat = 0;
            dTotalVat = Convert.ToDouble(lblVat.Text);
            lblVATVariance.Text = (dTotalVat - dCodingVat).ToString("0.00");
        }
        private void GetVatAmount()
        {
            //int InvoiceID = Convert.ToInt32(Request["InvoiceID"]);// commented by Rimi on 25.06.2015

            //Added by Rimi on 25.06.2015
            int InvoiceID = 0;
            double dVariance = 0;
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {
                InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
            }
            else
            {
                InvoiceID = Convert.ToInt32(Convert.ToInt32(ViewState["CheckList"]));

            }

            //Added by Rimi on 25.06.2015 End

            double TotalAmt = 0;
            double VATAmt = 0;
            string strCurrencyCode = string.Empty;
            string sSql = "SELECT  CONVERT(DECIMAL(18,2),TotalAmt) As TotalAmt ,CONVERT(DECIMAL(18,2),VATAmt) As VATAmt  ,CurrencyCode,Invoice.CurrencyTypeID ,CurrencyName FROM    Invoice  LEFT JOIN CurrencyTypes ON dbo.Invoice.CurrencyTypeID = dbo.CurrencyTypes.CurrencyTypeID WHERE   InvoiceID=" + InvoiceID;
            SqlDataReader dr = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);


            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                dr = sqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr[0] != DBNull.Value)
                    {
                        TotalAmt = Convert.ToDouble(dr[0]);
                        TotalAmt = Convert.ToDouble(TotalAmt.ToString("0.00"));
                    }
                    if (dr[1] != DBNull.Value)
                    {
                        VATAmt = Convert.ToDouble(dr[1]);
                        VATAmt = Convert.ToDouble(VATAmt.ToString("0.00"));
                    }
                    if (dr[2] != DBNull.Value)
                    {
                        strCurrencyCode = Convert.ToString(dr[2]);
                    }
                }
                // lblVat.Text = VATAmt.ToString("F", System.Globalization.CultureInfo.InvariantCulture); //VATAmt.ToString();
                //  lblTotal.Text = TotalAmt.ToString();
                lblVat.Text = VATAmt.ToString("0.00");
                //Added by Mainak 2018-04-16
                ViewState["lblTotalCodingVAT"] = VATAmt.ToString("0.00");
                lblTotal.Text = TotalAmt.ToString("0.00");
                lblCurrencyCode.Text = strCurrencyCode.ToString();


            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr.Close();
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            // return NetAmt;
        }


        private void GetNextVATAmount(int InvoiceID)
        {

            // int InvoiceID = Convert.ToInt32(Request["InvoiceID"]);

            double TotalAmt = 0;
            double VATAmt = 0;
            string strCurrencyCode = string.Empty;
            double InvoiceTotal = 0;
            //  string sSql = "SELECT  CONVERT(DECIMAL(18,2),TotalAmt) As TotalAmt ,CONVERT(DECIMAL(18,2),VATAmt) As VATAmt  ,CurrencyCode,Invoice.CurrencyTypeID ,CurrencyName FROM    Invoice  INNER   JOIN CurrencyTypes ON dbo.Invoice.CurrencyTypeID = dbo.CurrencyTypes.CurrencyTypeID WHERE   InvoiceID=" + InvoiceID;



            string sSql = "SELECT   CONVERT(DECIMAL(18,2),TotalAmt) As TotalAmt ,CONVERT(DECIMAL(18,2),VATAmt) As VATAmt  ,CurrencyCode,Invoice.CurrencyTypeID ,CurrencyName,CONVERT(DECIMAL(18,2),NetTotal) As NetAmt FROM    Invoice  INNER   JOIN CurrencyTypes ON dbo.Invoice.CurrencyTypeID = dbo.CurrencyTypes.CurrencyTypeID WHERE    InvoiceID=" + InvoiceID;



            SqlDataReader dr = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);


            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                dr = sqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr[0] != DBNull.Value)
                    {
                        TotalAmt = Convert.ToDouble(dr[0]);
                        TotalAmt = Convert.ToDouble(TotalAmt.ToString("0.00"));
                    }
                    if (dr[1] != DBNull.Value)
                    {
                        VATAmt = Convert.ToDouble(dr[1]);
                        VATAmt = Convert.ToDouble(VATAmt.ToString("0.00"));
                    }
                    if (dr[2] != DBNull.Value)
                    {
                        strCurrencyCode = Convert.ToString(dr[2]);
                    }
                    if (dr[3] != DBNull.Value)
                    {
                        InvoiceTotal = Convert.ToDouble(dr[5]);
                    }

                }

                // lblVat.Text = VATAmt.ToString("F", System.Globalization.CultureInfo.InvariantCulture); //VATAmt.ToString();
                //  lblTotal.Text = TotalAmt.ToString();


                lblVat.Text = VATAmt.ToString("0.00");
                lblTotal.Text = TotalAmt.ToString("0.00");
                lblCurrencyCode.Text = strCurrencyCode.ToString();
                lblNetInvoiceTotal.Text = InvoiceTotal.ToString("0.00");
                lblTotalCodingVATValue.Text = dCodingVat.ToString("0.00");
                lblNetVal.Text = InvoiceTotal.ToString("0.00");
                lblVATVariance.Text = (VATAmt - Convert.ToDouble(ViewState["Codingvat"])).ToString("0.00");
                lblVariance.Text = (Convert.ToDouble(lblNetInvoiceTotal.Text) - Convert.ToDouble(lblNetVal.Text)).ToString("0.00");
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr.Close();
                sqlCmd.Dispose();
                sqlConn.Close();
            }




        }










        protected void txtNetVal_TextChanged(object sender, EventArgs e)
        {
            TextBox txtProcess = (TextBox)sender;
            //--------------------added by kuntalkarar on 13thSeptember2016--------------------------
            if (txtProcess.Text == "0.00")
            {

                dNetAmt = 0;
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {

                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim() != "")
                    {
                        dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim());
                    }
                }
                CalculateTotal();
            }



            if (string.IsNullOrEmpty(txtProcess.Text))
            {
                txtProcess.Text = Convert.ToString("0.00");
                dNetAmt = 0;
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {

                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim() != "")
                    {
                        dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim());
                    }
                }
                CalculateTotal();
            }
            if (string.IsNullOrWhiteSpace(txtProcess.Text))
            {
                txtProcess.Text = Convert.ToString("0.00");
                dNetAmt = 0;
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {

                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim() != "")
                    {
                        dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim());
                    }
                }
                CalculateTotal();
            }

            //------------------addition ends by kuntalkarar on 13thSeptember2016----------------------------------
            //blocked by kuntal karar-on-25.03.2015----pt.48-------
            //^\d{1,16}((\.\d{1,4})|(\.))?$");
            Regex rg = new Regex(@"(^-?0\.[0-9]*[1-9]+[0-9]*$)|(^-?[1-9]+[0-9]*((\.[0-9]*[1-9]+[0-9]*$)|(\.[0-9]+)))|(^-?[1-9]+[0-9]*$)|(^0$){1}");
            //-----------------------------------------------------
            if (rg.IsMatch(txtProcess.Text))
            {
                dNetAmt = 0;
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {

                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim() != "")
                    {
                        dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim());
                    }
                }
                CalculateTotal();

            }
            else
            {
                txtProcess.Text = Convert.ToString("0.00");
            }


        }
        //private void grdList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        //    {
        //        int j = e.Item.DataSetIndex + 1;
        //        e.Item.Cells[0].Text = j.ToString();
        //        DataTable dt = null;

        //        //				if(TypeUser==1)
        //        //					dt= objCompany.GetCompanyListForPurchaseInvoiceLogGMG(Convert.ToInt32(Session["CompanyID"]),Convert.ToInt32(Session["UserID"]));	
        //        //				else
        //        dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
        //        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
        //        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataTextField = "CompanyName";
        //        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataValueField = "CompanyID";
        //        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
        //        ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "--Select--");

        //        GetAllComboCodesFirstTime();

        //        try
        //        {
        //            if (Request["DDCompanyID"] != null)
        //                ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = Session["BuyerCID"].ToString().Trim();
        //        }
        //        catch { }
        //        ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Clear();
        //        DataSet dsBusinessUnit = new DataSet();
        //        dsBusinessUnit = GetBusinessUnit(Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));
        //        if (dsBusinessUnit.Tables[0].Rows.Count > 0)
        //        {

        //            ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataSource = dsBusinessUnit;
        //            ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataTextField = "BusinessUnitName";
        //            ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataValueField = "BusinessUnitID";
        //            ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).DataBind();

        //        }
        //        ((DropDownList)e.Item.FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");
        //        ((DropDownList)e.Item.FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
        //        ((DropDownList)e.Item.FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
        //        ((DropDownList)e.Item.FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");


        //        if (((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim() != "")
        //        {
        //            dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim());
        //        }


        //        if (((TextBox)e.Item.FindControl("txtPoNumber")).Text != "")
        //        {
        //            if (GetPONumberForSupplierBuyer(((TextBox)e.Item.FindControl("txtPoNumber")).Text) != "Y")
        //            {

        //                ((TextBox)e.Item.FindControl("txtPoNumber")).ForeColor = Color.Red;

        //            }
        //            else
        //            {
        //                ((TextBox)e.Item.FindControl("txtPoNumber")).ForeColor = Color.Black;
        //            }
        //        }
        //    }
        //    else if (e.Item.ItemType == ListItemType.Footer)
        //    {
        //        ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetVal")).Text = dNetAmt.ToString();
        //        if (ViewState["NetAmt"] != null)
        //        {
        //            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetInvoiceTotal")).Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString();
        //        }
        //    }

        //}
        #endregion

        #region GetDocumentDetails(int iinvoiceID)
        private void GetDocumentDetails(int iinvoiceID)
        {
            //Start: added by koushik das as on 18Dec2017
            Session["AttInvoice"] = iinvoiceID;
            Session["eInvoiceID"] = iinvoiceID;
            //End: added by koushik das as on 18Dec2017

            DataSet DsInv = new DataSet();
            DsInv = GetDocumentDetails(iinvoiceID, "INV");
            if (DsInv != null)
            {
                if (DsInv.Tables.Count > 0)
                {
                    if (DsInv.Tables[0].Rows.Count > 0)
                    {
                        lblRefernce.Text = DsInv.Tables[0].Rows[0]["InvoiceNo"].ToString();
                        lblInvoiceDate.Text = DsInv.Tables[0].Rows[0]["InvoiceDate"].ToString();
                        lblSupplier.Text = DsInv.Tables[0].Rows[0]["SupplierCompanyName"].ToString();
                        iSupplierCompanyID = Convert.ToInt32(DsInv.Tables[0].Rows[0]["SupplierCompanyID"]);
                        //lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();
                        lblBuyer.Text = DsInv.Tables[0].Rows[0]["BuyerCompanyName"].ToString();
                        // lblcreditnoteno.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString();
                        lnkCrn.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString();
                        Session["BuyerCID"] = DsInv.Tables[0].Rows[0]["BuyerCompanyID"].ToString();
                        Session["InvoiceBuyerCompany"] = Session["BuyerCID"].ToString(); //Added By RImi on 8th Sept 2015
                        lblCurrentStatus.Text = DsInv.Tables[0].Rows[0]["Status"].ToString();
                        ViewState["StatusID"] = DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim();

                        ViewState["approvalpath"] = DsInv.Tables[0].Rows[0]["ApprovalPath"].ToString().Trim();
                        lblBusinessUnit.Text = Convert.ToString(DsInv.Tables[0].Rows[0]["BusinessUnit"]);


                        //Added by kuntalkarar on 9thJanuary2017
                        string getvendorClass = "SELECT TradingRelation.New_VendorClass FROM TradingRelation INNER JOIN Invoice ON TradingRelation.BuyerCompanyID = Invoice.BuyerCompanyID AND TradingRelation.SupplierCompanyID = Invoice.SupplierCompanyID where Invoice.invoiceId=" + iinvoiceID + " and Invoice.BuyerCompanyID=" + Convert.ToInt32(Session["BuyerCID"].ToString()) + " and Invoice.SupplierCompanyID=" + Convert.ToInt32(iSupplierCompanyID) + "";
                        SqlConnection sqlConn = new SqlConnection(ConsString);
                        SqlDataAdapter sqlDA = new SqlDataAdapter(getvendorClass, sqlConn);
                        DataTable dt = new DataTable();
                        try
                        {
                            sqlConn.Open();
                            sqlDA.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            string ss = ex.Message.ToString();
                        }
                        finally
                        {
                            sqlDA.Dispose();
                            sqlConn.Close();
                        }

                        //Added by Mainak 2018-05-24
                        string getPassedtoGroupCode = "SELECT PassedtoGroupCode from Invoice  where InvoiceID=" + iinvoiceID + " ";
                        SqlConnection sqlConn1 = new SqlConnection(ConsString);
                        SqlDataAdapter sqlDA1 = new SqlDataAdapter(getPassedtoGroupCode, sqlConn1);
                        DataTable dt1 = new DataTable();
                        try
                        {
                            sqlConn1.Open();
                            sqlDA1.Fill(dt1);
                        }
                        catch (Exception ex)
                        {
                            string ss = ex.Message.ToString();
                        }
                        finally
                        {
                            sqlDA1.Dispose();
                            sqlConn1.Close();
                        }

                        //Added by Mainak 2018-05-24 
                        string getRejectionCode = "SELECT RejectionCode from InvoiceStatus where InvoiceID=" + iinvoiceID + " order by DisplaySequence desc";
                        SqlConnection sqlConnRC = new SqlConnection(ConsString);
                        SqlDataAdapter sqlDARC = new SqlDataAdapter(getRejectionCode, sqlConnRC);
                        DataTable dtRC = new DataTable();
                        try
                        {
                            sqlConnRC.Open();
                            sqlDARC.Fill(dtRC);
                        }
                        catch (Exception ex)
                        {
                            string ss = ex.Message.ToString();
                        }
                        finally
                        {
                            sqlDARC.Dispose();
                            sqlConnRC.Close();
                        }
                        //lblRejectionCode.Text = "Price Mismatch";
                        if (dtRC.Rows.Count > 0)
                        {
                            if (dtRC.Rows.Count > 1 && dtRC.Rows[0]["RejectionCode"].ToString() == "")
                            {
                                lblRejectionCode.Text = dtRC.Rows[1]["RejectionCode"].ToString();
                            }
                            else
                            {
                                lblRejectionCode.Text = dtRC.Rows[0]["RejectionCode"].ToString();
                            }
                        }
                        else
                        {
                            lblRejectionCode.Text = "";
                        }

                        //Modified by Mainak 2018-05-24
                        if (dt.Rows[0]["New_VendorClass"].ToString() == "PO")
                        {
                            lnkVariance.Visible = true;
                            btnRematch.Visible = true;

                        }
                        else
                        {
                            lnkVariance.Visible = false;
                            btnRematch.Visible = false;

                        }

                        //Added by Mainak 2018-05-24
                        //if (TypeUser == 2 || TypeUser == 3)// when User type is either Admin or Ap
                        //{
                        //    btnRematch.Visible = true;                            
                        //}
                        if (TypeUser == 1)// when usertype is User/Approver
                        {
                            if ((dt.Rows[0]["New_VendorClass"].ToString() == "PO") && (dt1.Rows[0]["PassedtoGroupCode"].ToString() == "GI"))
                            {
                                btnRematch.Visible = true;
                            }
                            else
                            {
                                btnRematch.Visible = false;
                            }

                            if (dt.Rows[0]["New_VendorClass"].ToString() == "PO")
                            {
                                btnApprove.Visible = false;
                            }
                            else
                            {
                                if (Convert.ToInt32(ViewState["StatusID"]) == 22 || Convert.ToInt32(ViewState["StatusID"]) == 21)
                                {
                                    btnApprove.Visible = true;
                                }
                                else
                                {
                                    btnApprove.Visible = false;
                                }

                            }
                        }
                        else
                        {
                            btnApprove.Visible = false;//Modified by Mainak 2018-09-19
                        }

                        //Added by Mainak 2018-08-10
                        if (TypeUser == 2 || TypeUser == 3)
                        {
                            if (dt.Rows[0]["New_VendorClass"].ToString() == "PO")
                            {
                                btnRelease.Visible = true;
                            }
                            else
                            {
                                btnRelease.Visible = false;
                            }
                        }
                        else
                        {
                            btnRelease.Visible = false;
                        }

                        //-----------------------------------------------------------------------------------------
                        //btnRematch.Visible = true;//For test

                        try
                        {
                            //  lblcreditnoteno.Text = GetMultipleCreditNotes();
                            //  lnkCrn.Text = lblcreditnoteno.Text.ToString();
                        }
                        catch { }

                        try
                        {
                            if (DsInv.Tables[0].Rows[0]["Department"].ToString() != "")
                            {
                                lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();
                                ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();
                                ddldept.SelectedValue = Convert.ToString(ViewState["DepartmentID"]);
                            }
                            else
                            {
                                ViewState["DepartmentID"] = 0;
                            }
                            int dept = 0;
                            dept = Convert.ToInt32(Convert.ToString(ViewState["DepartmentID"]));
                            if (dept != 0)
                                GetApproverDropDownsAgainstDepartment(dept);

                        }
                        catch { }

                        ViewState["StatusID"] = DsInv.Tables[0].Rows[0]["StatusID"].ToString();
                        ViewState["OriginalNetAmount"] = DsInv.Tables[0].Rows[0]["Net"].ToString();
                        Session["eDocType"] = DsInv.Tables[0].Rows[0]["DocType"].ToString();
                    }
                }
            }
        }
        #endregion

        #region PopulateDropDowns
        private void PopulateDropDowns()
        {
            GetApproverDropDowns();
            GetDepartMentDropDwons();
            PopulateRejectionCode();
        }
        #endregion

        #region PopulateRejectionCode
        private void PopulateRejectionCode()
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetRejectionCode", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", 180918);//124529 for AnchorSafety changed to 180918 for JKS //Convert.ToInt32(Session["InvoiceBuyerCompany"]));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlRejection.DataSource = ds;
                    ddlRejection.DataBind();
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            ddlRejection.Items.Insert(0, "Select Comments");


        }
        #endregion

        #region GetApproverDropDowns()
        public void GetApproverDropDowns()
        {
            ddlApprover1.Items.Insert(0, "Select");
            ddlApprover2.Items.Insert(0, "Select");
            ddlApprover3.Items.Insert(0, "Select");
            ddlApprover4.Items.Insert(0, "Select");
            ddlApprover5.Items.Insert(0, "Select");

        }
        #endregion

        #region GetDepartMentDropDwons()
        public void GetDepartMentDropDwons()
        {
            CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval();
            DataSet ds = new DataSet();
            string Fields = "DepartmentID , Department";
            string Table = "Department";
            string Criteria = "BuyerCompanyID = " + System.Convert.ToInt32(Session["InvoiceBuyerCompany"]);
            try
            {
                ds = objApproval.GetGlobalDropDowns(Fields, Table, Criteria);

                ddldept.DataSource = ds;
                ddldept.DataBind();

                //ADded by Mainak 2018-04-06
                chkDepartment.DataSource = ds;
                chkDepartment.DataBind();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ddldept.Items.Insert(0, "Select");
                ds = null;
            }
        }
        #endregion


        #region GetUserCodesAndUserGroupsByUserID
        private void GetUserCodesAndUserGroupsByUserID()
        {
            DataSet ds = new DataSet();
            ds = GetUserCodeANDUserGroup(System.Convert.ToInt32(Session["UserID"]));
            strNewUserCode = ds.Tables[0].Rows[0]["New_UserCode"].ToString();
            strNewUserGroup = ds.Tables[0].Rows[0]["New_UserGroup"].ToString();
            if ((sdApprover1 == strNewUserCode) || (sdApprover1 == strNewUserGroup))
            {
                if (sdApprover1 == strNewUserCode)
                    strPassedNewUserCode = sdApprover1;
                else
                    strPassedNewUserGroup = sdApprover1;
            }
            else if ((sdApprover2 == strNewUserCode) || (sdApprover2 == strNewUserGroup))
            {
                if (sdApprover2 == strNewUserCode)
                    strPassedNewUserCode = sdApprover2;
                else
                    strPassedNewUserGroup = sdApprover2;
            }
            else if ((sdApprover3 == strNewUserCode) || (sdApprover3 == strNewUserGroup))
            {
                if (sdApprover3 == strNewUserCode)
                    strPassedNewUserCode = sdApprover3;
                else
                    strPassedNewUserGroup = sdApprover3;
            }
            else if ((sdApprover4 == strNewUserCode) || (sdApprover4 == strNewUserGroup))
            {
                if (sdApprover4 == strNewUserCode)
                    strPassedNewUserCode = sdApprover4;
                else
                    strPassedNewUserGroup = sdApprover4;
            }
            else if ((sdApprover5 == strNewUserCode) || (sdApprover5 == strNewUserGroup))
            {
                if (sdApprover5 == strNewUserCode)
                    strPassedNewUserCode = sdApprover5;
                else
                    strPassedNewUserGroup = sdApprover5;
            }

        }
        #endregion
        #region ButtonVisibility()
        private void ButtonVisibility()
        {
            ddlApprover3.Visible = false;

            if (TypeUser == 1)
            {
                int exId = Convert.ToInt32(ViewState["StatusID"]);
                btnReopen.Visible = false;
                btnOpen.Visible = false;
                /*Added by kuntalkarar on 15thMarch2016---------------- */
                if (Convert.ToInt32(ViewState["StatusID"]) == 21 || Convert.ToInt32(ViewState["StatusID"]) == 22)  // when REGISTERED & REOPENED
                {

                    TR_RejectionCode.Visible = true;
                    TR_RejectionComments.Visible = true;

                }
                else
                {
                    TR_RejectionCode.Visible = true;
                    TR_RejectionComments.Visible = true;
                }
                //-----------------------------------------------------


                if (Convert.ToInt32(ViewState["StatusID"]) == 20)  // when Received
                {
                    btndelete.Visible = false;
                    //btnReopen.Visible = false;
                    //Modified by Mainak 2018-07-10
                    btnOpen.Visible = true;
                    //btnApprove.Visible = false;
                    btnReject.Visible = true;
                }
                else if (Convert.ToInt32(ViewState["StatusID"]) == 6)  // when Rejected
                {
                    btndelete.Visible = false;
                    //btnReopen.Visible = true;
                    //btnOpen.Visible = true;
                    // btnApprove.Visible = false;
                    btnReject.Visible = false;
                    RejectOpenFields = 1;
                }
                //Added by Mainak 2018-07-11
                else if (Convert.ToInt32(ViewState["StatusID"]) == 22 || Convert.ToInt32(ViewState["StatusID"]) == 21)
                {
                    btndelete.Visible = false;
                    //btnReopen.Visible = false;
                    //btnOpen.Visible = false;
                    btnReject.Visible = true;
                    //btnApprove.Visible = true;//Blocked by Mainak 2018-07-11
                }
                else
                {
                    btndelete.Visible = false;
                    //btnReopen.Visible = false;
                    //btnOpen.Visible = false;
                    btnReject.Visible = true;
                    //btnApprove.Visible = true;
                }
                // Added By Mrinal on 15th July 2013
                if (Convert.ToInt32(ViewState["StatusID"]) == 22 || Convert.ToInt32(ViewState["StatusID"]) == 21 || Convert.ToInt32(ViewState["StatusID"]) == 20 || Convert.ToInt32(ViewState["StatusID"]) == 6)
                {
                    ddlApprover4.Visible = true;
                    ddlApprover5.Visible = true;
                }
                else
                {
                    ddlApprover4.Visible = false;
                    ddlApprover5.Visible = false;
                }
                // btnApprove.Visible = false;

            }

            if (TypeUser == 2)
            {

                if (Convert.ToInt32(ViewState["StatusID"]) == 20)  // when Received
                {
                    ddlApprover3.Visible = true;
                    btndelete.Visible = true;
                    btnReopen.Visible = false;
                    //Modified by Mainak 2018-07-10
                    //modified by kuntalkarar on 24thJanuary2017
                    btnOpen.Visible = true;
                    //btnOpen.Visible = false;
                    btnApprove.Visible = false;
                    //btnApprove.Visible = true;
                    //-------------------------------------------

                    btnReject.Visible = true;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 6)  // when Rejected
                {
                    btndelete.Visible = true;
                    //modified by kuntalkarar on 24thJanuary2017
                    //Modified by Mainak 2018-07-20
                    //btnReopen.Visible = false;
                    btnReopen.Visible = true;
                    //btnApprove.Visible = true;
                    btnApprove.Visible = false;
                    //------------------------------------

                    btnOpen.Visible = false;

                    btnReject.Visible = false;
                    RejectOpenFields = 1;
                    ddlApprover3.Visible = true;//Subhrajyoti

                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 1)  // when Unapproved
                {
                    btndelete.Visible = true;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                    RejectOpenFields = 1;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 21)  // when Registered
                {
                    btndelete.Visible = true;

                    btnReopen.Visible = true;
                    btnOpen.Visible = false;
                    btnApprove.Visible = false;
                    // btnReject.Visible = false;
                    btnReject.Visible = true;
                    RejectOpenFields = 0;
                    ddlApprover3.Visible = true;
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 22)  // when Reopened
                {
                    btndelete.Visible = true;
                    btnReopen.Visible = true;
                    btnOpen.Visible = false;
                    btnApprove.Visible = false;
                    //btnReject.Visible = false;
                    btnReject.Visible = true;
                    RejectOpenFields = 0;
                    ddlApprover3.Visible = true;
                }

                // Added By Mrinal on 15th July 2013
                if (Convert.ToInt32(ViewState["StatusID"]) == 22 || Convert.ToInt32(ViewState["StatusID"]) == 21 || Convert.ToInt32(ViewState["StatusID"]) == 20 || Convert.ToInt32(ViewState["StatusID"]) == 6)
                {
                    ddlApprover4.Visible = true;
                    ddlApprover5.Visible = true;
                }
                else
                {
                    ddlApprover4.Visible = false;
                    ddlApprover5.Visible = false;
                }
            }



            if (TypeUser > 2)
            {



                if (Convert.ToInt32(ViewState["StatusID"]) == 20)  // when Received
                {
                    /*Added by kuntalkarar on 15thMarch2016*/
                    TR_CreditNoteNo.Visible = false;

                    btndelete.Visible = true;
                    btnReopen.Visible = false;
                    //Modifed by Mainak 2018-07-10
                    //modified by kuntalkarar on 24thJanuary2017
                    btnOpen.Visible = true;
                    //btnOpen.Visible = false;
                    btnApprove.Visible = false;
                    //btnApprove.Visible = true;
                    //-------------------------------------------

                    btnReject.Visible = true;
                    if (TypeUser == 3)
                    {
                        ddlApprover3.Visible = true;

                        /*Added by kuntalkarar on 15thMarch2016*/
                        TR_RejectionCode.Visible = true;
                        TR_RejectionComments.Visible = true;
                    }
                    else
                    {
                        /*Added by kuntalkarar on 15thMarch2016*/
                        TR_RejectionCode.Visible = false;
                        TR_RejectionComments.Visible = false;
                    }
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 6)  // when Rejected
                {
                    btndelete.Visible = true;
                    //modified by kuntalkarar on 24thJanuary2017
                    //btnReopen.Visible = true;
                    btnReopen.Visible = false;

                    btnOpen.Visible = false;
                    btnApprove.Visible = true;
                    btnReject.Visible = false;
                    RejectOpenFields = 1;
                    if (TypeUser == 3)
                    {
                        /*Added by kuntalkarar on 15thMarch2016*/
                        TR_CreditNoteNo.Visible = true;
                        /*Added by kuntalkarar on 27thMarch2016*/
                        TR_RejectionCode.Visible = false;
                        TR_RejectionComments.Visible = false;

                        ddlApprover3.Visible = true;//rinku//Subhrajyoti
                        //modified by kuntalkarar on 24thJanuary2017
                        //Modified by Mainak 2018-07-20
                        //btnReopen.Visible = false;
                        btnReopen.Visible = true;
                        //btnApprove.Visible = true;
                        btnApprove.Visible = false;

                    }
                    else
                    {
                        /*Added by kuntalkarar on 15thMarch2016*/
                        TR_CreditNoteNo.Visible = false;
                    }
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 1)  // when Unapproved
                {
                    btndelete.Visible = false;
                    btnReopen.Visible = false;
                    btnOpen.Visible = false;
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                    RejectOpenFields = 1;
                }

                if (Convert.ToInt32(ViewState["StatusID"]) == 21)  // when Registered
                {
                    if (TypeUser == 3)
                    {
                        btndelete.Visible = true;
                        btnReopen.Visible = true;
                        btnOpen.Visible = false;
                        btnApprove.Visible = false;
                        // btnReject.Visible = false;
                        btnReject.Visible = true;
                        RejectOpenFields = 0;
                        ddlApprover3.Visible = true;


                        /*Added by kuntalkarar on 15thMarch2016*/
                        TR_RejectionCode.Visible = true;
                        TR_RejectionComments.Visible = true;
                        TR_CreditNoteNo.Visible = true;
                    }
                    else
                    {
                        /*Added by kuntalkarar on 15thMarch2016*/
                        TR_RejectionCode.Visible = false;
                        TR_RejectionComments.Visible = false;
                        TR_CreditNoteNo.Visible = false;

                        btndelete.Visible = true;
                        btnReopen.Visible = false;
                        btnOpen.Visible = false;
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        RejectOpenFields = 0;
                    }
                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 22)  // when Reopened
                {
                    if (TypeUser == 3)
                    {
                        btndelete.Visible = true;
                        btnReopen.Visible = true;
                        btnOpen.Visible = false;
                        btnApprove.Visible = false;
                        // btnReject.Visible = false;
                        btnReject.Visible = true;
                        RejectOpenFields = 0;
                        ddlApprover3.Visible = true;

                        /*Added by kuntalkarar on 15thMarch2016*/
                        TR_RejectionCode.Visible = true;
                        TR_RejectionComments.Visible = true;
                        TR_CreditNoteNo.Visible = true;
                    }
                    else
                    {
                        btndelete.Visible = true;
                        btnReopen.Visible = false;
                        btnOpen.Visible = false;
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        RejectOpenFields = 0;

                        /*Added by kuntalkarar on 15thMarch2016*/
                        TR_RejectionCode.Visible = false;
                        TR_RejectionComments.Visible = false;
                        TR_CreditNoteNo.Visible = false;
                    }
                }
                // Added By Mrinal on 15th July 2013
                if (Convert.ToInt32(ViewState["StatusID"]) == 22 || Convert.ToInt32(ViewState["StatusID"]) == 21 || Convert.ToInt32(ViewState["StatusID"]) == 20 || Convert.ToInt32(ViewState["StatusID"]) == 6)
                {
                    ddlApprover4.Visible = true;
                    ddlApprover5.Visible = true;
                }
                else
                {
                    ddlApprover4.Visible = false;
                    ddlApprover5.Visible = false;
                }
            }
            //btnApprove.Visible = true;// For testing
        }
        #endregion

        #region CheckInvoiceExist
        private void CheckInvoiceExist()
        {
            int RowCnt = 1;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);

            SqlDataAdapter sqlDA = new SqlDataAdapter("ups_GetGenericCodingChange", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            //===============Commented by Rimi on 25.06.2015====================================================
            //if (Convert.ToInt64(ViewState["InvoiceChecking"]) != 0)
            //{
            //    sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt64(ViewState["InvoiceChecking"]));
            //    sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
            //}
            //else
            //{
            //    sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt64(Request.QueryString["InvoiceID"]));
            //    sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
            //}
            //===============Commented by Rimi on 25.06.2015====================================================


            // Added by Rimi on 25/06.2015
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(Request.QueryString["InvoiceID"]));

            }
            else
            {
                sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["CheckList"]));

            }
            sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
            // Added by Rimi on 25/06.2015 End

            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                   
                    RowCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    if (RowCnt == 0)
                    {
                        RowCnt = 1;
                        ViewState["Exist"] = "0";
                    }
                    else
                    {
                        ViewState["Exist"] = "1";
                    }
                }
                ViewState["populate"] = ds;
                //   ViewState["data"] = ds;
                ViewState["data"] = null;// Added by Rimi on 25/06.2015
                BindGrid(RowCnt);


            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedIndex = -1;
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")), ds.Tables[1].Rows[i]["CompanyID"].ToString());
            }


            GetAllComboCodesFirstTime();

            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            {

                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")), ds.Tables[1].Rows[i]["BusinessUnitID"].ToString());
                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text = ds.Tables[1].Rows[i]["CodingDescription"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value = ds.Tables[1].Rows[i]["CodingDescriptionID"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value = ds.Tables[1].Rows[i]["DepartmentID"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value = ds.Tables[1].Rows[i]["NominalCodeID"].ToString();


                // Added by Mrinal on 18th March 2015
                // ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = ds.Tables[1].Rows[i]["Description"].ToString();//Commeneted By Rimi on 8th August 2015
                // Addition End

                //Added By Rimi on 8thAugust2015
                string LineDescription = ds.Tables[1].Rows[i]["Description"].ToString();
                if (LineDescription.Contains("&lt;"))
                {
                    LineDescription = LineDescription.Replace("&lt;", "<");
                }
                if (LineDescription.Contains("&gt;"))
                {
                    LineDescription = LineDescription.Replace("&gt;", ">");
                }
                if (LineDescription.Contains("&pound;"))
                {
                    LineDescription = LineDescription.Replace("&pound;", "£");
                }

                if (LineDescription.Contains("&belongsto;"))
                {
                    LineDescription = LineDescription.Replace("&belongsto;", "€");
                }

                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = LineDescription.ToString();
              
            }

        }



        ////Modification for coding change start

        //private void CheckNextInvoiceExist(int InvoiceId)
        //{
        //    int RowCnt = 1;
        //    string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        //    SqlConnection sqlConn = new SqlConnection(ConsString);

        //    SqlDataAdapter sqlDA = new SqlDataAdapter("ups_GetGenericCodingChange", sqlConn);
        //    sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        //    sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceId);
        //    sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
        //    DataSet ds = new DataSet();

        //    sqlConn.Open();
        //    sqlDA.Fill(ds);
        //    ViewState["populate"] = ds;
        //    //   ViewState["data"] = ds;
        //    grdList.DataSource = ds.Tables[1];
        //    grdList.DataBind();
        //    //if (ds.Tables[0].Rows.Count > 0)
        //    //{
        //    //    RowCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        //    //    if (RowCnt == 0)
        //    //    {
        //    //        RowCnt = 1;
        //    //        ViewState["Exist"] = "0";
        //    //    }
        //    //    else
        //    //    {
        //    //        ViewState["Exist"] = "1";
        //    //    }
        //    //}
        //    //ViewState["populate"] = ds;
        //    //BindGrid(RowCnt);


        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        RowCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        //        if (RowCnt == 0)
        //        {
        //            RowCnt = 1;
        //            ViewState["Exist"] = "0";
        //        }
        //        else
        //        {
        //            ViewState["Exist"] = "1";
        //        }
        //    }
        //    ViewState["populate"] = ds;


        //    ViewState["data"] = null;// Added by Rimi on 25/06.2015
        //    BindGrid(RowCnt);// Added by Rimi on 25/06.2015


        //    for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
        //    {
        //        ((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedIndex = -1;
        //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")), ds.Tables[1].Rows[i]["CompanyID"].ToString());
        //    }


        //    GetAllComboCodesFirstTime();

        //    for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
        //    {

        //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")), ds.Tables[1].Rows[i]["BusinessUnitID"].ToString());
        //        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text = ds.Tables[1].Rows[i]["CodingDescription"].ToString();
        //        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value = ds.Tables[1].Rows[i]["CodingDescriptionID"].ToString();
        //        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value = ds.Tables[1].Rows[i]["DepartmentID"].ToString();
        //        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value = ds.Tables[1].Rows[i]["NominalCodeID"].ToString();


        //        // Added by Mrinal on 18th March 2015
        //        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = ds.Tables[1].Rows[i]["Description"].ToString();
        //        // Addition End

        //    }
        //}







        // //Modification for coding change ens


        private void CheckNextInvoiceExist(int InvoiceId)
        {
            int RowCnt = 1;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);

            SqlDataAdapter sqlDA = new SqlDataAdapter("ups_GetGenericCodingChange", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceId);
            sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
            DataSet ds = new DataSet();

            sqlConn.Open();
            sqlDA.Fill(ds);
            ViewState["populate"] = ds;
            //   ViewState["data"] = ds;
            grdList.DataSource = ds.Tables[1];
            grdList.DataBind();
            // uncommented by Rimi on 25/06.2015
            if (ds.Tables[0].Rows.Count > 0)
            {
                RowCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (RowCnt == 0)
                {
                    RowCnt = 1;
                    ViewState["Exist"] = "0";
                }
                else
                {
                    ViewState["Exist"] = "1";
                }
            }
            ViewState["populate"] = ds;


            ViewState["data"] = null;// Added by Rimi on 25/06.2015
            BindGrid(RowCnt);// Added by Rimi on 25/06.2015

            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedIndex = -1;
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")), ds.Tables[1].Rows[i]["CompanyID"].ToString());
            }


            GetAllComboCodesFirstTime();

            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            {

                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")), ds.Tables[1].Rows[i]["BusinessUnitID"].ToString());
                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text = ds.Tables[1].Rows[i]["CodingDescription"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value = ds.Tables[1].Rows[i]["CodingDescriptionID"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value = ds.Tables[1].Rows[i]["DepartmentID"].ToString();
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value = ds.Tables[1].Rows[i]["NominalCodeID"].ToString();


                // Added by Mrinal on 18th March 2015
                // ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = ds.Tables[1].Rows[i]["Description"].ToString();//Commented By RImi on 8th August 2015
                // Addition End
                //Added By Rimi on 8thAugust2015
                string LineDescription = ds.Tables[1].Rows[i]["Description"].ToString();
                if (LineDescription.Contains("&lt;"))
                {
                    LineDescription = LineDescription.Replace("&lt;", "<");
                }
                if (LineDescription.Contains("&gt;"))
                {
                    LineDescription = LineDescription.Replace("&gt;", ">");
                }
                if (LineDescription.Contains("&pound;"))
                {
                    LineDescription = LineDescription.Replace("&pound;", "£");
                }

                if (LineDescription.Contains("&belongsto;"))
                {
                    LineDescription = LineDescription.Replace("&belongsto;", "€");
                }

                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = LineDescription.ToString();
            }
        }



        #endregion
        #region BindGrid
        private void BindGrid(int iNoofRow)
        {
            DataSet ds;
            if (ViewState["data"] == null)
            {
                CreateDataSet(iNoofRow);
            }
            ds = ((DataSet)(ViewState["data"]));

            //Added by Mainak 2018-04-17
            if (!ds.Tables[0].Columns.Contains("CodingDescription"))
                ds.Tables[0].Columns.Add("CodingDescription");

            grdList.DataSource = ds.Tables[0];
            grdList.DataBind();
        }
        #endregion

        #region CreateDataSet
        private void CreateDataSet(int iNoofRow)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(GetBlankTable(iNoofRow));
            ViewState["data"] = ds;
        }
        #endregion

        #region doAction  AND  takeAction
        private void takeAction(string docType, int ID, int iOperation)
        {
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = new SqlCommand("UPD_UpdateStatus", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@docType", docType);
            sqlCmd.Parameters.Add("@ID", ID);
            sqlCmd.Parameters.Add("@Operation", iOperation);
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();

            }
        }
        private void doAction(int iActionType)
        {
            // for invoice only
            if (iActionType == 1)
            {
                takeAction("INV", Convert.ToInt32(Request["InvoiceID"]), 1);
            }
            else if (iActionType == 0)
            {
                takeAction("INV", Convert.ToInt32(Request["InvoiceID"]), 0);
            }
        }
        #endregion

        #region protected DataTable GetBlankTable(int iNoofRow)
        protected DataTable GetBlankTable(int iNoofRow)
        {

            DataTable tbl = null;
            int InvoiceID = 0;
            double dtmpNetAmt = 0;
            //InvoiceID = Convert.ToInt32(Request["InvoiceID"]);//==Commented by Rimi
            //==========Added by Rimi on 25.06.2015====================================
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {
                InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
            }
            else
            {
                InvoiceID = Convert.ToInt32(ViewState["CheckList"]);
            }

            //==========Added by Rimi on 25.06.2015====================================
            dtmpNetAmt = GetNetAmt(InvoiceID);
            ViewState["NetAmt"] = dtmpNetAmt;

            if (iNoofRow <= 1)
            {
                DataSet ds = ((DataSet)ViewState["populate"]);
                tbl = new DataTable();
                DataRow nRow;
                tbl.Columns.Add("NetValue");
                tbl.Columns.Add("PurOrderNo");
                tbl.Columns.Add("VAT");

                for (int i = 0; i < iNoofRow; i++)
                {
                    nRow = tbl.NewRow();
                    nRow["NetValue"] = dtmpNetAmt;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        nRow["PurOrderNo"] = ds.Tables[1].Rows[i]["PurOrderNo"];
                        nRow["VAT"] = ds.Tables[1].Rows[i]["VAT"];
                    }
                    tbl.Rows.Add(nRow);
                }
            }
            else
            {
                // here i need to write code for opening in edit mode
                DataSet ds = ((DataSet)ViewState["populate"]);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    tbl = new DataTable();
                    DataRow nRow;
                    tbl.Columns.Add("NetValue");
                    tbl.Columns.Add("PurOrderNo");
                    tbl.Columns.Add("VAT");
                    for (int i = 0; i < iNoofRow; i++)
                    {
                        nRow = tbl.NewRow();
                        if (ds.Tables[1].Rows.Count > 0)
                        {

                            nRow["NetValue"] = ds.Tables[1].Rows[i]["netvalue"];
                            nRow["PurOrderNo"] = ds.Tables[1].Rows[i]["PurOrderNo"];
                            nRow["VAT"] = ds.Tables[1].Rows[i]["VAT"];
                        }
                        tbl.Rows.Add(nRow);
                    }
                }
            }
            return tbl;
        }
        #endregion

        #region GetAllComboCodesAddNew
        private void GetAllComboCodesAddNew()
        {
            int compid = 0;
            DataTable dt = null;
            string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlNominalCode1 = "";
            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {

                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
                ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();


                if (TypeUser == 1)
                    dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]), compid);
                else
                    dt = objInvoice.GetGridDepartmentList(compid);

                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);

                if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--")
                {
                    if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--" && ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue != "--Select--")
                    {
                        int iddlDept = 0;
                        int iNomin = 0;

                        iddlDept = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                        iNomin = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);

                        DataSet dsCODES = new DataSet();

                        dsCODES = GetCodingDescriptionAgainstDepartmentANDNominal(iddlDept, iNomin, compid);
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dsCODES;
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ddlCodingDescription1);


                    }
                    else if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
                    {

                        dt = objInvoice.GetGridNominalCodeList(compid);
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);


                    }
                }

                else if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
                {
                    DataSet dsDeptNom = new DataSet();
                    int iCoding = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue);
                    dsDeptNom = GetDepartmentANDNominalAgainstCodingDescID(iCoding, compid);
                    if (dsDeptNom.Tables[0].Rows.Count > 0)
                    {
                        ddlDepartment1 = dsDeptNom.Tables[0].Rows[0]["DepartmentID"].ToString();


                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);
                    }
                    if (dsDeptNom.Tables[1].Rows.Count > 0)
                    {
                        ddlNominalCode1 = dsDeptNom.Tables[1].Rows[0]["NominalCodeID"].ToString();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dsDeptNom.Tables[1];
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);
                    }

                }

                else
                {

                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                }


            }
        }
        #endregion


        #region GetAllComboCodes
        private void GetAllComboCodes()
        {
            int compid = 0;
            DataTable dt = null;
            string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlNominalCode1 = "";
            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim() != "--Select--")
                {
                    ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                }

                if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim() != "--Select--")
                {

                    ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
                }
                if (((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim() != "--Select--")
                {

                    ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();
                }

                if (compid != 0)
                {
                    //					if(TypeUser==1)
                    //						dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]),compid);
                    //					else
                    //						dt= objInvoice.GetGridDepartmentList(compid);
                    //
                    //					((DropDownList)	grdList.Items[i].FindControl("ddlDepartment1")).DataSource=dt;
                    //					((DropDownList)	grdList.Items[i].FindControl("ddlDepartment1")).DataTextField="Department";
                    //					((DropDownList)	grdList.Items[i].FindControl("ddlDepartment1")).DataValueField="DepartmentID";
                    //					((DropDownList)	grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                    //					((DropDownList)	grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0,"--Select--");
                    //					SetValueForCombo(((DropDownList) grdList.Items[i].FindControl("ddlDepartment1")),ddlDepartment1);

                    DataSet ds = LoadDepartment(compid);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = ds;
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);
                    }

                    dt = objInvoice.GetGridNominalCodeList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);

                    if (TypeUser == 1)
                        dt = objInvoice.GetGridCodingDescriptionListByUserID(Convert.ToInt32(Session["UserID"]), compid);
                    else
                        dt = objInvoice.GetGridCodingDescriptionList(compid);

                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ddlCodingDescription1);
                }
                else
                    Response.Write("<script>alert('Please select a company');</script>");
            }
        }
        #endregion

        #region GetAllComboCodesForNominalRefresh
        private void GetAllComboCodesForNominalRefresh()
        {
            //int compid = 0;
            //DataTable dt = null;
            //string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlNominalCode1 = "";
            //for (int i = 0; i <= grdList.Items.Count - 1; i++)
            //{
            //    if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            //    {
            //        compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
            //    }
            //    ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
            //    ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();

            //    ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

            //    if (compid != 0)
            //    {

            //        if (TypeUser == 1)
            //            dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]), compid);
            //        else
            //            dt = objInvoice.GetGridDepartmentList(compid);

            //        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
            //        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
            //        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
            //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);


            //        dt = objInvoice.GetGridNominalCodeList(compid);
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
            //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);



            //        if (TypeUser == 1)
            //            dt = objInvoice.GetGridCodingDescriptionListByUserID(Convert.ToInt32(Session["UserID"]), compid);
            //        else
            //            dt = objInvoice.GetGridCodingDescriptionList(compid);

            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dt;
            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
            //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ddlCodingDescription1);

            //    }
            //    else
            //        Response.Write("<script>alert('Please select a company');</script>");
            //}
        }
        #endregion

        #region private void SetValueForCombo(System.Web.UI.WebControls.DropDownList ddlSrc,string sVal)
        private void SetValueForCombo(System.Web.UI.WebControls.DropDownList ddlSrc, string sVal)
        {
            int i = 0;
            ddlSrc.SelectedIndex = -1;
            for (i = 0; i <= ddlSrc.Items.Count - 1; i++)
            {
                if (ddlSrc.Items[i].Value.Trim() == sVal.Trim())
                {
                    ddlSrc.SelectedIndex = i;
                    break;
                }
            }

        }
        #endregion


        #region SelectedIndexChanged_ddlBuyerCompanyCode
        protected void SelectedIndexChanged_ddlBuyerCompanyCode(object sender, System.EventArgs e)
        {
            int i = 0;
            DropDownList ddl = ((DropDownList)sender);
            //i = ((RepeaterItem)ddl.Parent.Parent).ItemIndex;
            RepeaterItem rptItem = (RepeaterItem)ddl.NamingContainer;
            if (((DropDownList)rptItem.FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            {
                ((DropDownList)rptItem.FindControl("ddlBusinessUnit")).Items.Clear();
                DataSet dsBusinessUnit = new DataSet();
                dsBusinessUnit = GetBusinessUnit(Convert.ToInt32(((DropDownList)rptItem.FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));
                if (dsBusinessUnit.Tables[0].Rows.Count > 0)
                {
                    ((DropDownList)rptItem.FindControl("ddlBusinessUnit")).DataSource = dsBusinessUnit;
                    ((DropDownList)rptItem.FindControl("ddlBusinessUnit")).DataTextField = "BusinessUnitName";
                    ((DropDownList)rptItem.FindControl("ddlBusinessUnit")).DataValueField = "BusinessUnitID";
                    ((DropDownList)rptItem.FindControl("ddlBusinessUnit")).DataBind();
                }
                ((DropDownList)rptItem.FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");

                ((TextBox)rptItem.FindControl("txtAutoCompleteCodingDescription")).Text = "";
                ((HiddenField)rptItem.FindControl("hdnCodingDescriptionID")).Value = "";
                ((HiddenField)rptItem.FindControl("hdnNominalCodeID")).Value = "";
                ((HiddenField)rptItem.FindControl("hdnDepartmentCodeID")).Value = "";

                string strCtrl = ((DropDownList)rptItem.FindControl("ddlBuyerCompanyCode")).ClientID;
                setFocus(strCtrl);
            }

        }
        #endregion
        #region setFocus(string strCtrl)
        /// <summary>
        /// Create by :Rinku Santra
        /// Create Date:26-03-2011
        /// This method set focus of cntrol to next control
        /// by JavaScript
        /// </summary>
        /// <param name="strCtrl"></param>
        protected void setFocus(string strCtrl)
        {
            string sScript = "<SCRIPT language='javascript'>document.getElementById('" + strCtrl + "').focus(); </SCRIPT>";
            Page.RegisterStartupScript("Focus", sScript);
        }
        #endregion

        #region  btnAddNew_Click
        private void btnAddNew_Click(object sender, System.EventArgs e)
        {
            Populate(0, "a");
           

            tabIndexSet();
            string strCtrl = "";
            int Count = 0;
            for (int i = 0; i < grdList.Items.Count; i++)
            {
                strCtrl = ((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).ClientID;
                // Count++;
            }
            // Added by Mrinal on 10th November 2014
            Count = grdList.Items.Count - 1;
            if (Count > 0)
            {
                ((TextBox)grdList.Items[Count].FindControl("txtNetVal")).Text = lblVariance.Text;
                lblVariance.Text = Convert.ToString("0.00");
                ((TextBox)grdList.Items[Count].FindControl("txtLineVAT")).Text = lblVATVariance.Text;
                lblVATVariance.Text = Convert.ToString("0.00");

                //====================Added By Rimi on 31st July 2015=================================
                //((DropDownList)grdList.Items[Count].FindControl("ddlBuyerCompanyCode")).SelectedValue = ((DropDownList)grdList.Items[Count - 1].FindControl("ddlBuyerCompanyCode")).SelectedValue;// Added By Rimi on 14thAugust2015



                ((DropDownList)grdList.Items[Count].FindControl("ddlBuyerCompanyCode")).SelectedValue = ((DropDownList)grdList.Items[Count - 1].FindControl("ddlBuyerCompanyCode")).SelectedValue;// Added By Rimi on 18thAugust2015
                ((TextBox)grdList.Items[Count].FindControl("txtAutoCompleteCodingDescription")).Text = ((TextBox)grdList.Items[Count - 1].FindControl("txtAutoCompleteCodingDescription")).Text;
                //((System.Web.UI.WebControls.HiddenField)(grdList.Items[Count].FindControl("hdnCodingDescriptionID"))).Value=((System.Web.UI.WebControls.HiddenField)(grdList.Items[Count-1].FindControl("hdnCodingDescriptionID"))).Value
                ((HiddenField)grdList.Items[Count].FindControl("hdnCodingDescriptionID")).Value = ((HiddenField)grdList.Items[Count - 1].FindControl("hdnCodingDescriptionID")).Value;
                ((HiddenField)grdList.Items[Count].FindControl("hdnDepartmentCodeID")).Value = ((HiddenField)grdList.Items[Count - 1].FindControl("hdnDepartmentCodeID")).Value;
                ((HiddenField)grdList.Items[Count].FindControl("hdnNominalCodeID")).Value = ((HiddenField)grdList.Items[Count - 1].FindControl("hdnNominalCodeID")).Value;
                ((TextBox)grdList.Items[Count].FindControl("txtLineDescription")).Text = ((TextBox)grdList.Items[Count - 1].FindControl("txtLineDescription")).Text;
                //====================Added By Rimi on 31st July 2015=================================

            }
            // Addition End 

            setFocus(strCtrl);
            CalculateTotal();//Added by Rimi on 26.06.2015
        }

        private void Populate(int irow, string acttype)
        {
            int i;
            int j;
            string[] strValue = new string[1];

            // Add by Mrinal on 18th March  2015
            string[] strVatValue = new string[1];
            ArrayList arrLineWiseDescription = new ArrayList();
            // Addition End
            DataRow dr;
            ArrayList arrLstComp = new ArrayList();
            // Add by Mrinal on 7th November 2014
            ArrayList arrLstCodeDescription = new ArrayList();
            // Addition End
            ArrayList arrLstCode = new ArrayList();
            ArrayList arrLstDept = new ArrayList();
            ArrayList arrLstNomi = new ArrayList();
            ArrayList arrLstBusinessUnit = new ArrayList();
            ArrayList PurchaseOrderNO = new ArrayList();


            DataSet ds = ((DataSet)(ViewState["data"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Rows.Clear();
            }
            for (i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (irow == 0 && acttype == "a")
                {
                    arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);

                    //arrLstCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedItem.Value);
                    //arrLstDept.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedItem.Value);
                    //arrLstNomi.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedItem.Value);

                    arrLstCodeDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text);

                    arrLstCode.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value);
                    arrLstDept.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value);
                    arrLstNomi.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value);


                    PurchaseOrderNO.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text);

                    arrLstBusinessUnit.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))).SelectedItem.Value);
                    strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
                    dr = ds.Tables[0].NewRow();
                    for (j = 0; j < 1; j++)
                    {
                        dr[j] = strValue[j].ToString();
                    }

                    // Added by Mrinal on 18th March 2015
                    arrLineWiseDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text);
                    strVatValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineVAT"))).Text;
                    for (j = 2; j < 3; j++)
                    {
                        dr[j] = strVatValue[j - 2].ToString();
                    }
                    // Addition End

                    ds.Tables[0].Rows.Add(dr);
                }
                else if (irow != i && acttype == "d")
                {
                    arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);
                    //arrLstCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedItem.Value);
                    //arrLstDept.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedItem.Value);
                    //arrLstNomi.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedItem.Value);
                    arrLstCodeDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text);
                    arrLstCode.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value);
                    arrLstDept.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value);
                    arrLstNomi.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value);
                    PurchaseOrderNO.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text);

                    arrLstBusinessUnit.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))).SelectedItem.Value);

                    strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
                    dr = ds.Tables[0].NewRow();
                    for (j = 0; j < 1; j++)
                    {
                        dr[j] = strValue[j].ToString();
                    }
                    // Added by Mrinal on 18th March 2015
                    arrLineWiseDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text);
                    strVatValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineVAT"))).Text;
                    for (j = 2; j < 3; j++)
                    {
                        dr[j] = strVatValue[j - 2].ToString();
                    }
                    // Addition End
                    ds.Tables[0].Rows.Add(dr);
                }
            }
            if (irow == 0 && acttype == "a")
            {
                dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            ViewState["data"] = ds;
            BindGrid(1);
            dNetAmt = 0;
            i = 0;
            for (i = 0; i <= arrLstComp.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedIndex = -1;
                ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).Items.FindByValue(arrLstComp[i].ToString()).Selected = true;
            }
            //if (acttype == "d")
            //    GetAllComboCodesForNominalRefresh();
            //else
            //{
            //    GetAllComboCodesFirstTime();
            //}
            //  arrLstCodeDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text);

            // Added by Mrinal on 18th March 2015
            for (i = 0; i <= arrLstCodeDescription.Count - 1; i++)
            {
                // ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = arrLineWiseDescription[i].ToString();//Commented By Rimi on 8th August 2015
                //Added By Rimi on 8thaugust2015
                string LineDescription = arrLineWiseDescription[i].ToString();
                if (LineDescription.Contains("&lt;"))
                {
                    LineDescription = LineDescription.Replace("&lt;", "<");
                }
                if (LineDescription.Contains("&gt;"))
                {
                    LineDescription = LineDescription.Replace("&gt;", ">");
                }
                if (LineDescription.Contains("&pound;"))
                {
                    LineDescription = LineDescription.Replace("&pound;", "£");
                }

                if (LineDescription.Contains("&belongsto;"))
                {
                    LineDescription = LineDescription.Replace("&belongsto;", "€");
                }

                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = LineDescription.ToString();
            }
            // Addition End

            for (i = 0; i <= arrLstCodeDescription.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text = arrLstCodeDescription[i].ToString();
            }



            for (i = 0; i <= arrLstCode.Count - 1; i++)
            {
                // ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedIndex = -1;
                // SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))), arrLstCode[i].ToString());
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value = arrLstCode[i].ToString();

            }

            for (i = 0; i <= arrLstDept.Count - 1; i++)
            {
                // ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedIndex = -1;
                //  SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))), arrLstDept[i].ToString());
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value = arrLstDept[i].ToString();
            }

            for (i = 0; i <= arrLstNomi.Count - 1; i++)
            {
                //  ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedIndex = -1;
                //  SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))), arrLstNomi[i].ToString());
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value = arrLstNomi[i].ToString();
            }

            for (i = 0; i <= arrLstBusinessUnit.Count - 1; i++)
            {
                SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))), arrLstBusinessUnit[i].ToString());
            }

            for (i = 0; i <= PurchaseOrderNO.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text = PurchaseOrderNO[i].ToString();
                string PurchaseorderNo = "";
                System.Web.UI.WebControls.TextBox txtPoNumber = (System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtPoNumber");
                PurchaseorderNo = txtPoNumber.Text;

                if (((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text != "")
                {
                    if (GetPONumberForSupplierBuyer(PurchaseorderNo.Trim()) != "Y")
                    {
                        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).ForeColor = Color.Red;
                    }
                    else
                    {
                        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).ForeColor = Color.Black;
                    }
                }
            }

        }
        #endregion

        #region btnRetrieve_Click
        //private void btnRetrieve_Click(object sender, System.EventArgs e)
        //{
        //    GetAllComboCodesAddNew();
        //}
        #endregion


        #region SetDropDownValuesOnSaveButtonClick
        private int SetDropDownValuesOnSaveButtonClick()
        {

            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            //    SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("UpdateApproverReceivedStatus", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {
                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            }
            else
            {
                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["InvoiceChecking"]));

            }




            sqlCmd.Parameters.Add("@NewApprover1", Convert.ToString(ViewState["Approver1_NR"]));

            //sqlCmd.Parameters.Add("@NewApprover1", Convert.ToString(ddlApprover1.SelectedItem.Text));


            sqlCmd.Parameters.Add("@NewApprover2", Convert.ToString(ViewState["Approver2_NR"]));
            //sqlCmd.Parameters.Add("@NewApprover2", Convert.ToString(ddlApprover2.SelectedItem.Text));



            sqlCmd.Parameters.Add("@NewApprover3", Convert.ToString(ViewState["Approver3_NR"]));
            //sqlCmd.Parameters.Add("@NewApprover3", Convert.ToString(ddlApprover3.SelectedItem.Text));



            sqlCmd.Parameters.Add("@NewApprover4", Convert.ToString(ViewState["Approver4_NR"]));
            //sqlCmd.Parameters.Add("@NewApprover4", Convert.ToString(ddlApprover4.SelectedItem.Text));



            sqlCmd.Parameters.Add("@NewApprover5", Convert.ToString(ViewState["Approver5_NR"]));
            //sqlCmd.Parameters.Add("@NewApprover5", Convert.ToString(ddlApprover5.SelectedItem.Text));


            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                // iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iReturnValue = -1; }
            finally
            {
                // sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion


        #region: btnSaveLine_Click
        //public void btnSaveLine_Click(object sender, EventArgs e)
        //{



        //    if (SaveLineData())
        //    {


        //            SetDropDownValuesOnSaveButtonClick();



        //            PasswordReset objPasswordReset = new PasswordReset();

        //            if (Convert.ToInt64(ViewState["InvoiceChecking"]) == 0)
        //            {
        //                string deptName = GetDepartmentName(Convert.ToString(ViewState["vDepartmentID"]));
        //                if (deptName != "")
        //                {
        //                    objPasswordReset.UpdateDepartmentId(Convert.ToString(Session["eInvoiceID"]), Convert.ToString(Session["InvoiceBuyerCompany"]), Convert.ToString(ViewState["vDepartmentID"]), Convert.ToString(ViewState["vCodingDescriptionID"]));
        //                }
        //            }
        //            else
        //            {




        //                    objPasswordReset.UpdateDepartmentId(Convert.ToString(Convert.ToInt64(ViewState["InvoiceChecking"])), Convert.ToString(Session["InvoiceBuyerCompany"]), Convert.ToString(ViewState["vDepartmentID"]), Convert.ToString(ViewState["vCodingDescriptionID"]));



        //            }

        //        string message = "alert('Coding saved successfully.')";
        //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

        //    }


        //}


        public void btnSaveLine_Click(object sender, EventArgs e)
        {
            GetDepartMentDropDwons(); // Added by koushik das (kd) on 22-01-2019 for prorate
            if (SaveLineData())
            {
                if (Convert.ToInt32(ViewState["StatusID"]) != 20)  // when Received
                {
                    //    //Except For Received status start 

                    //    // int DeptUpdate = UpdateDepartmentAgainstInvoiceID();

                    SetDropDownValuesOnSaveButtonClick();

                    //    //Except For Received status End 

                }


                PasswordReset objPasswordReset = new PasswordReset();

                if (Convert.ToInt64(ViewState["InvoiceChecking"]) == 0)
                {
                    string deptName = GetDepartmentName(Convert.ToString(ViewState["vDepartmentID"]));
                    if (deptName != "")
                    {
                        // objPasswordReset.UpdateDepartmentId(Convert.ToString(Session["eInvoiceID"]), Convert.ToString(Session["InvoiceBuyerCompany"]), Convert.ToString(ViewState["vDepartmentID"]), Convert.ToString(ViewState["vCodingDescriptionID"]));// Commented By Rimi on 25thAugust2015
                    }
                    DataSet DsInv = new DataSet();
                    DsInv = GetDocumentDetails(Convert.ToInt32(Convert.ToString(Session["eInvoiceID"])), "INV");

                    if (Convert.ToString(DsInv.Tables[0].Rows[0]["DepartmentID"]) != "")
                    {
                        lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();

                        ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();

                        ddldept.ClearSelection();
                        // ddldept.Items.FindByValue(Convert.ToString(ViewState["DepartmentID"])).Selected = true;



                        // ddldept.SelectedItem.Text = lblDepartment.Text;
                        // ddldept.SelectedValue = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();

                    }
                    else
                    {
                        ddldept.SelectedItem.Text = " Select ";


                    }
                    GetDocumentDetails(Convert.ToInt32(Session["eInvoiceID"]));
                    // GetApproverDropDownsAgainstDepartment(Convert.ToInt32(ViewState["DepartmentID"]));
                }
                else
                {
                    // objPasswordReset.UpdateDepartmentId(Convert.ToString(Convert.ToInt64(ViewState["InvoiceChecking"])), Convert.ToString(Session["InvoiceBuyerCompany"]), lblDepartment.Text, Convert.ToString(ViewState["vCodingDescriptionID"]));

                    // objPasswordReset.UpdateDepartmentId(Convert.ToString(Session["eInvoiceID"]), Convert.ToString(Session["InvoiceBuyerCompany"]), Convert.ToString(ViewState["vDepartmentID"]), Convert.ToString(ViewState["vCodingDescriptionID"]));


                    DataSet DsInv = new DataSet();
                    DsInv = GetDocumentDetails(Convert.ToInt32(ViewState["InvoiceChecking"]), "INV");


                    if (Convert.ToString(DsInv.Tables[0].Rows[0]["DepartmentID"]) != "")
                    {
                        lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();

                        ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();

                        ddldept.SelectedItem.Text = lblDepartment.Text;
                    }

                    else
                    {
                        ddldept.SelectedItem.Text = " select ";
                    }


                    // GetApproverDropDownsAgainstDepartment(Convert.ToInt32(ViewState["DepartmentID"]));
                    GetDocumentDetails(Convert.ToInt32(ViewState["InvoiceChecking"]));
                    // GetApproverDropDownsAgainstDepartment(Convert.ToInt32(ViewState["DepartmentID"]));
                }
                //  lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();


                //lblmessage.Text = "Coding saved successfully";

                //  string msg = "Lines Data Save Successfully.";
                //string msg = "Coding saved successfully.";
                //Page.RegisterStartupScript("Reg", "<script>AutoCloseAlert('" + msg + "');</script>");
                GetDepartMentDropDwons(); // Added by koushik das (kd) on 22-01-2019 for prorate
                string message = "alert('Coding saved successfully.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

            }
            if (Convert.ToString(ViewState["ddlCompFlag"]) == "1")
            {
                ViewState["ddlCompFlag"] = "2";
                string msg = "Please select Company.";
                Page.RegisterStartupScript("Reg", "<script>AutoCloseAlert('" + msg + "');</script>");
                string message = "alert('Please select Company.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

            }
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 29th July 2015


        }
        #endregion
        #region btnDelLine_Click
        private void btnDelLine_Click(object sender, System.EventArgs e)
        {
            lblmessage.Text = "";
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015
            int i = 0;
            // DataSet ds = ((DataSet)(ViewState["data"])); //commented By kd on 22th jan 2019 for prorate
            //added By kd on 22nd jan 2019 for prorate
		    DataSet ds;
		    if (Session["data"] != null)
            {
                 ds = ((DataSet)(Session["data"]));
                 
            }
            else
            {
                 ds = ((DataSet)(ViewState["data"]));
            }
            string numbers = "";
            int LineNo = 0;

            for (i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (((System.Web.UI.WebControls.CheckBox)grdList.Items[i].FindControl("chkBox")).Checked)
                {
                    numbers += grdList.Items[i].ItemIndex + "/";

                    //  LineNo = Convert.ToInt32(grdList.Items[i].Cells[0].Text);
                    LineNo = Convert.ToInt32(((System.Web.UI.WebControls.Label)grdList.Items[i].FindControl("lblLineNo")).Text);

                }
            }
            string[] strArr = numbers.Split('/');
            DataSet ds1 = ((DataSet)(ViewState["data"]));
            for (int j = 0; j <= strArr.Length - 1; j++)
            {
                ds1 = ((DataSet)(ViewState["data"]));

                if (strArr[j] != "" && ds1.Tables[0].Rows.Count - 1 > 0)
                {
                    if (Convert.ToInt32(strArr[j]) <= ds1.Tables[0].Rows.Count - 1)
                    {
                        ds1.Tables[0].Rows[Convert.ToInt32(strArr[j])].Delete();

                        Populate(Convert.ToInt32(strArr[j]), "d");


                    }
                    else if (ds1.Tables[0].Rows.Count != 1)
                    {
                        int iPass = 0;
                        iPass = ds1.Tables[0].Rows.Count - 1;
                        ds1.Tables[0].Rows[ds1.Tables[0].Rows.Count - 1].Delete();
                        Populate(iPass, "d");
                    }
                }
            }
            bool flag = SaveLineData();
           
        }
        #endregion

        #region SaveDetailData()

        //================================Commented By Rimi on 21st August 2015===================================
        //private bool SaveDetailData()
        //{
        //    #region variables
        //    //int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]); // Commented by Rimi on 25.06.2015

        //    //Added  by Rimi on 25.06.2015
        //    int InvID = 0;

        //    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
        //    {
        //        InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
        //    }
        //    else
        //    {
        //        InvID = Convert.ToInt32(ViewState["CheckList"]);
        //    }

        //    //Added  by Rimi on 25.06.2015 End

        //    int CompanyID = 0;
        //    int CodingDescriptionID = 0;
        //    int NominalCodeID = 0;
        //    int BusinessUnitID = 0;
        //    int DepartmentID = 0;
        //    int iValidFlag = 0;
        //    decimal NetValue = 0;
        //    double NetVal = 0;
        //    string PurOrderNo = String.Empty;
        //    // Added by Mrinal on 13th March 2015
        //    decimal LineVAT = 0;
        //    string strLineDescription = string.Empty;
        //    // Addition End on 13th March 2015 
        //    bool retval = false;
        //    lblErrorMsg.Visible = false;
        //    #endregion

        //    for (int j = 0; j <= grdList.Items.Count - 1; j++)
        //    {
        //        if (grdList.Items[j].ItemType == ListItemType.Item || grdList.Items[j].ItemType == ListItemType.AlternatingItem)
        //        {
        //            #region Validations
        //            if (((DropDownList)grdList.Items[j].FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim() == "--Select--")
        //            {
        //                Response.Write("<script>alert('Please select company code.');</script>");
        //                iValidFlag = 1;
        //                break;
        //            }
        //            if (((TextBox)grdList.Items[j].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length <= 0)
        //            {
        //                Response.Write("<script>alert('Please select coding.');</script>");
        //                iValidFlag = 1;
        //                break;

        //            }

        //            if (((System.Web.UI.WebControls.TextBox)grdList.Items[j].FindControl("txtNetVal")).Text.Trim() == "")
        //            {

        //                Response.Write("<script>alert('Please enter Net Value for coding at line(s).');</script>");
        //                iValidFlag = 1;
        //                break;
        //            }

        //            #endregion
        //        }
        //    }
        //    if (iValidFlag == 1)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        for (int i = 0; i <= grdList.Items.Count - 1; i++)
        //        {
        //            if (grdList.Items[i].ItemType == ListItemType.Item || grdList.Items[i].ItemType == ListItemType.AlternatingItem)
        //            {
        //                NetVal = NetVal + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
        //            }
        //        }
        //        DataSet dsXML = new DataSet();
        //        DataTable dtXML = new DataTable();
        //        dtXML.Columns.Add("SlNo");
        //        dtXML.Columns.Add("InvoiceID");
        //        dtXML.Columns.Add("InvoiceType");
        //        dtXML.Columns.Add("CompanyID");
        //        dtXML.Columns.Add("CodingDescriptionID");
        //        dtXML.Columns.Add("DepartmentID");
        //        dtXML.Columns.Add("NominalCodeID");
        //        dtXML.Columns.Add("BusinessUnitID");
        //        dtXML.Columns.Add("NetValue");
        //        dtXML.Columns.Add("CodingValue");
        //        dtXML.Columns.Add("PurOrderNo");//ss

        //        // Added by Mrinal on 13th March 2015
        //        dtXML.Columns.Add("LineVAT");
        //        dtXML.Columns.Add("LineDescription");
        //        // Addition End on 13th March 2015 


        //        DataRow DR = null;

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<Root>");
        //        NetVal = Math.Round(NetVal, 2);
        //        ViewState["NetAmt"] = NetVal;// Added By Rimi on 22nd July 2015
        //        #region: Variance Validation
        //        if (Convert.ToDouble(ViewState["NetAmt"].ToString()) == Convert.ToDouble(NetVal.ToString()))
        //        {
        //            for (int i = 0; i <= grdList.Items.Count - 1; i++)
        //            {
        //                #region Getting DropDown Values
        //                if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
        //                {
        //                    CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
        //                }
        //                if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
        //                {
        //                    string a = ((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim();
        //                    //Boolean res = GetCodingDetails(a.Substring(0,8));
        //                    int index = a.IndexOf("[");
        //                    if (index > 0)
        //                    {
        //                        Boolean res = GetCodingDetails(a.Substring(0, index));
        //                        if (res == false)
        //                        {

        //                            Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
        //                            return false;

        //                        }
        //                    }
        //                    else
        //                    {
        //                        Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
        //                        return false;
        //                    }
        //                    //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                    //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //                    //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //                    //=====================Modified By Rimi on 27th July 2015=============================
        //                    if (i > 0)
        //                    {
        //                        if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim() != ((TextBox)grdList.Items[i - 1].FindControl("txtAutoCompleteCodingDescription")).Text.Trim())
        //                        {
        //                            NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                            ViewState["hdnDepartmentCodeID"] = "2";
        //                        }

        //                        else
        //                        {
        //                            NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
        //                            ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = NominalCodeID.ToString();
        //                            ViewState["hdnDepartmentCodeID"] = "1";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                        ViewState["hdnDepartmentCodeID"] = "2";
        //                    }
        //                    if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
        //                    {
        //                        if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
        //                        {
        //                            DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (i > 0)
        //                        {
        //                            DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
        //                            ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
        //                        }
        //                    }
        //                    ViewState["vDepartmentID"] = DepartmentID;
        //                    if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
        //                    {
        //                        CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //                    }
        //                    else
        //                    {
        //                        if (i > 0)
        //                        {
        //                            CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
        //                            ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();
        //                        }

        //                    }

        //                    //====================Modified By Rimi on 27th July 2015 End======================
        //                }

        //                if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
        //                {
        //                    BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
        //                }


        //                PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);


        //                NetValue = 0;
        //                if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
        //                {
        //                    //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
        //                    //{
        //                        NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
        //                    //}
        //                }

        //                LineVAT = 0;

        //                if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
        //                {
        //                    //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
        //                    //{
        //                        LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
        //                    //}
        //                }
        //                strLineDescription = string.Empty;

        //                if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
        //                {
        //                    strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
        //                }
        //                //Added By RImi on 8th August 2015

        //                if (strLineDescription.ToString().Contains("<"))
        //                {
        //                    strLineDescription = strLineDescription.Replace("<", "&lt;");
        //                }
        //                if (strLineDescription.ToString().Contains(">"))
        //                {
        //                    strLineDescription = strLineDescription.Replace(">", "&gt;");
        //                }
        //                if (strLineDescription.ToString().Contains("£"))
        //                {
        //                    strLineDescription = strLineDescription.Replace("£", "&pound;");
        //                }
        //                if (strLineDescription.ToString().Contains("€"))
        //                {
        //                    strLineDescription = strLineDescription.Replace("€", "&belongsto;");
        //                }

        //                #endregion

        //                if (NetValue > -1 || (Convert.ToDecimal(Request.QueryString["iVat"]) > -1 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))
        //               // if (NetValue > 0 || (Convert.ToDecimal(Request.QueryString["iVat"]) > 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > 0))
        //                {
        //                    DR = dtXML.NewRow();
        //                    dtXML.Rows.Add(DR);
        //                    sb.Append("<Rowss>");
        //                    sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
        //                    sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
        //                    sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
        //                    sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
        //                    sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
        //                    sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
        //                    sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
        //                    int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
        //                    if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
        //                        sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
        //                    else
        //                        sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</BusinessUnitID>");

        //                    sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
        //                    sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
        //                    sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");
        //                    sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
        //                    sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");

        //                    sb.Append("</Rowss>");
        //                }
        //            }
        //            dsXML.Tables.Add(dtXML);
        //            sb.Append("</Root>");

        //            //Added By Rimi on 27th July 2015
        //            if (sb.ToString().Contains("&"))
        //            {
        //                sb = sb.Replace("&", "&amp;");
        //            }
        //            if (sb.ToString().Contains("'"))
        //            {
        //                sb = sb.Replace("'", "&apos;");
        //            }
        //            //Added By Rimi on 27th July 2015 End

        //            string strXmlText = sb.ToString();
        //            sb = null;


        //            int retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
        //            if (retvalalue > 0)
        //            {
        //                retval = true;
        //            }
        //            else
        //                retval = false;
        //        }
        //        else
        //        {
        //            lblErrorMsg.Visible = false;
        //            //Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
        //            Response.Write("<script>alert('Variance must be zero.');</script>");
        //        }
        //        #endregion
        //    }
        //    return retval;
        //}


        //=====================Added By Rimi on 21st August 2015============================

        private bool SaveDetailData()
        {
            #region variables
            //int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]); // Commented by Rimi on 25.06.2015

            //Added  by Rimi on 25.06.2015
            int InvID = 0;

            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {
                InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            }
            else
            {
                InvID = Convert.ToInt32(ViewState["CheckList"]);
            }

            //Added  by Rimi on 25.06.2015 End

            int CompanyID = 0;
            int CodingDescriptionID = 0;
            int NominalCodeID = 0;
            int BusinessUnitID = 0;
            int DepartmentID = 0;
            int iValidFlag = 0;
            decimal NetValue = 0;
            double NetVal = 0;
            string PurOrderNo = String.Empty;
            // Added by Mrinal on 13th March 2015
            decimal LineVAT = 0;
            string strLineDescription = string.Empty;
            // Addition End on 13th March 2015 
            bool retval = false;
            lblErrorMsg.Visible = false;
            #endregion

            for (int j = 0; j <= grdList.Items.Count - 1; j++)
            {
                if (grdList.Items[j].ItemType == ListItemType.Item || grdList.Items[j].ItemType == ListItemType.AlternatingItem)
                {
                    #region Validations
                    if (((DropDownList)grdList.Items[j].FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim() == "--Select--")
                    {
                        Response.Write("<script>alert('Please select company code.');</script>");
                        iValidFlag = 1;
                        break;
                    }
                    if (((TextBox)grdList.Items[j].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length <= 0)
                    {
                        Response.Write("<script>alert('Please select coding.');</script>");
                        iValidFlag = 1;
                        break;

                    }

                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[j].FindControl("txtNetVal")).Text.Trim() == "")
                    {

                        Response.Write("<script>alert('Please enter Net Value for coding at line(s).');</script>");
                        iValidFlag = 1;
                        break;
                    }

                    #endregion
                }
            }
            if (iValidFlag == 1)
            {
                return false;
            }
            else
            {
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {
                    if (grdList.Items[i].ItemType == ListItemType.Item || grdList.Items[i].ItemType == ListItemType.AlternatingItem)
                    {
                        NetVal = NetVal + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                    }
                }
                DataSet dsXML = new DataSet();
                DataTable dtXML = new DataTable();
                dtXML.Columns.Add("SlNo");
                dtXML.Columns.Add("InvoiceID");
                dtXML.Columns.Add("InvoiceType");
                dtXML.Columns.Add("CompanyID");
                dtXML.Columns.Add("CodingDescriptionID");
                dtXML.Columns.Add("DepartmentID");
                dtXML.Columns.Add("NominalCodeID");
                dtXML.Columns.Add("BusinessUnitID");
                dtXML.Columns.Add("NetValue");
                dtXML.Columns.Add("CodingValue");
                dtXML.Columns.Add("PurOrderNo");//ss

                // Added by Mrinal on 13th March 2015
                dtXML.Columns.Add("LineVAT");
                dtXML.Columns.Add("LineDescription");
                // Addition End on 13th March 2015 


                DataRow DR = null;

                StringBuilder sb = new StringBuilder();
                sb.Append("<Root>");
                NetVal = Math.Round(NetVal, 2);
                ViewState["NetAmt"] = NetVal;
                #region: Variance Validation
                //if (Convert.ToDouble(ViewState["NetAmt"].ToString()) == Convert.ToDouble(NetVal.ToString()))
                //{
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {
                    #region Getting DropDown Values
                    if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
                    {
                        CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                    }
                    if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
                    {
                        //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                        //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                        //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);

                        //=====================Modified By Rimi on 27th July 2015=============================
                        string a = ((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim();
                        //Boolean res = GetCodingDetails(a.Substring(0,8));
                        int index = a.IndexOf("[");
                        if (index > 0)
                        {
                            Boolean res = GetCodingDetails(a.Substring(0, index));
                            if (res == false)
                            {

                                Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
                                return false;

                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
                            return false;
                        }


                        if (i > 0)
                        {
                            //---------------blocked by kuntal karar on 17thAugust2015--------------
                            //if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim() != ((TextBox)grdList.Items[i - 1].FindControl("txtAutoCompleteCodingDescription")).Text.Trim())
                            //{
                            if (((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value != "")
                            {
                                NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                                ViewState["hdnDepartmentCodeID"] = "2";
                            }
                            //}

                            //else
                            //{
                            //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
                            ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = NominalCodeID.ToString();
                            //ViewState["hdnDepartmentCodeID"] = "1";
                            //}
                            //---ENDS------------blocked by kuntal karar on 17thAugust2015--------------
                        }
                        else
                        {
                            if (((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value != "")
                            {
                                NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                                ViewState["hdnDepartmentCodeID"] = "2";
                            }
                        }
                        //---------------blocked by kuntal karar on 17thAugust2015--------------
                        //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                        //{
                        if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
                        {
                            DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                        }
                        // }
                        //else
                        //{
                        //    if (i > 0)
                        //    {
                        //        DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
                        //}
                        //}
                        //----ENDS--------------blocked by kuntal karar on 17thAugust2015--------------
                        ViewState["vDepartmentID"] = DepartmentID;


                        //---------------blocked by kuntal karar on 17thAugust2015--------------
                        //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                        //{
                        if (((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value != "")
                            CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                        //}
                        //else
                        //{
                        //    if (i > 0)
                        //    {
                        //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();
                        //    }

                        //}
                        //---ENDS------------blocked by kuntal karar on 17thAugust2015--------------
                        //====================Modified By Rimi on 27th July 2015 End======================
                    }

                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
                    {
                        BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
                    }


                    PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);


                    NetValue = 0;
                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
                    {
                        //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
                        //{
                        NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                        //}
                    }

                    LineVAT = 0;

                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
                    {
                        //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
                        //{
                        LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
                        //}
                    }
                    strLineDescription = string.Empty;

                    if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
                    {
                        strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
                    }
                    //Added By RImi on 8th August 2015

                    if (strLineDescription.ToString().Contains("<"))
                    {
                        strLineDescription = strLineDescription.Replace("<", "&lt;");
                    }
                    if (strLineDescription.ToString().Contains(">"))
                    {
                        strLineDescription = strLineDescription.Replace(">", "&gt;");
                    }
                    if (strLineDescription.ToString().Contains("£"))
                    {
                        strLineDescription = strLineDescription.Replace("£", "&pound;");
                    }
                    if (strLineDescription.ToString().Contains("€"))
                    {
                        strLineDescription = strLineDescription.Replace("€", "&belongsto;");
                    }

                    #endregion

                    if (NetValue > -1 || (Convert.ToDecimal(Request.QueryString["iVat"]) > -1 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))
                    // if (NetValue > 0 || (Convert.ToDecimal(Request.QueryString["iVat"]) > 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > 0))
                    {
                        DR = dtXML.NewRow();
                        dtXML.Rows.Add(DR);
                        sb.Append("<Rowss>");
                        sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
                        sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
                        sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
                        sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
                        sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
                        sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
                        sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
                        int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                        if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
                            sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
                        else
                            sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</BusinessUnitID>");

                        sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
                        sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
                        sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");
                        sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
                        sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");

                        sb.Append("</Rowss>");
                    }
                }
                dsXML.Tables.Add(dtXML);
                sb.Append("</Root>");
                //Added By Rimi on 28th July 2015
                if (sb.ToString().Contains("&"))
                {
                    sb = sb.Replace("&", "&amp;");
                }
                if (sb.ToString().Contains("'"))
                {
                    sb = sb.Replace("'", "&apos;");
                }
                //Added By Rimi on 28th July 2015 End
                string strXmlText = sb.ToString();
                sb = null;


                int retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
                if (retvalalue > 0)
                {
                    retval = true;
                }
                else
                    retval = false;

                #endregion
            }
            return retval;
        }

        #endregion
        #region SaveDetailDataForGMG()
        //private bool SaveDetailDataForGMG()
        //{

        //    //int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);// commented by Rimi on 25.06.2015

        //    // Added by Rimi on 25.06.2015

        //    int InvID = 0;
        //    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
        //    {

        //        InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
        //    }
        //    else
        //    {
        //        InvID = Convert.ToInt32(ViewState["CheckList"]);
        //    }

        //    // Added by Rimi on 25.06.2015 End

        //    int CompanyID = 0;
        //    int CodingDescriptionID = 0;
        //    int NominalCodeID = 0;
        //    int BusinessUnitID = 0;
        //    int DepartmentID = 0;
        //    decimal NetValue = 0;
        //    bool flag = false;
        //    double NetVal = 0;
        //    // Added by Mrinal on 13th March 2015
        //    decimal LineVAT = 0;
        //    string strLineDescription = string.Empty;
        //    // Addition End on 13th March 2015 

        //    string PurOrderNo = "";

        //    bool retval = false;
        //    lblErrorMsg.Visible = false;

        //    for (int i = 0; i <= grdList.Items.Count - 1; i++)
        //    {
        //        if (grdList.Items[i].ItemType == ListItemType.Item || grdList.Items[i].ItemType == ListItemType.AlternatingItem)
        //        {
        //            if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.ToString() != "")
        //            {
        //                NetVal = NetVal + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
        //                flag = true;
        //            }
        //            else
        //            {
        //                flag = false;
        //                Response.Write("<script>alert('Please enter Net Value for coding at line(s)..');</script>");
        //                return flag;
        //            }
        //        }
        //    }
        //    DataSet dsXML = new DataSet();
        //    DataTable dtXML = new DataTable();
        //    dtXML.Columns.Add("SlNo");
        //    dtXML.Columns.Add("InvoiceID");
        //    dtXML.Columns.Add("InvoiceType");
        //    dtXML.Columns.Add("CompanyID");
        //    dtXML.Columns.Add("CodingDescriptionID");
        //    dtXML.Columns.Add("DepartmentID");
        //    dtXML.Columns.Add("NominalCodeID");
        //    dtXML.Columns.Add("BusinessUnitID");
        //    dtXML.Columns.Add("NetValue");
        //    dtXML.Columns.Add("CodingValue");
        //    dtXML.Columns.Add("PurOrderNo");

        //    // Added by Mrinal on 13th March 2015
        //    dtXML.Columns.Add("LineVAT");
        //    dtXML.Columns.Add("LineDescription");
        //    // Addition End on 13th March 2015 

        //    DataRow DR = null;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<Root>");
        //    NetVal = Math.Round(NetVal, 2);
        //    ViewState["NetValINV"] = NetVal;
        //    ViewState["NetAmt"] = NetVal;// Added By Rimi on 22nd July 2015
        //    #region: Variance Validation
        //    if (Convert.ToDouble(ViewState["NetAmt"]) == NetVal || IsCalledFromOpenButton == 1)
        //    {
        //        for (int i = 0; i <= grdList.Items.Count - 1; i++)
        //        {
        //            retval = true;
        //            #region Getting DropDown Values
        //            if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) != "--Select--")
        //            {
        //                CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
        //            }
        //            else
        //                retval = false;

        //            if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
        //            {
        //                string a = ((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim();
        //                //Boolean res = GetCodingDetails(a.Substring(0,8));
        //                int index = a.IndexOf("[");
        //                if (index > 0)
        //                {
        //                    Boolean res = GetCodingDetails(a.Substring(0, index));
        //                    if (res == false)
        //                    {

        //                        Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
        //                        return false;

        //                    }
        //                }
        //                else
        //                {
        //                    Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
        //                    return false;
        //                }
        //                //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //                //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //                //=====================Modified By Rimi on 27th July 2015=============================
        //                if (i > 0)
        //                {
        //                    if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim() != ((TextBox)grdList.Items[i - 1].FindControl("txtAutoCompleteCodingDescription")).Text.Trim())
        //                    {
        //                        NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                        ViewState["hdnDepartmentCodeID"] = "2";
        //                    }

        //                    else
        //                    {
        //                        NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
        //                        ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = NominalCodeID.ToString();
        //                        ViewState["hdnDepartmentCodeID"] = "1";
        //                    }
        //                }
        //                else
        //                {
        //                    NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                    ViewState["hdnDepartmentCodeID"] = "2";
        //                }
        //                if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
        //                {
        //                    if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
        //                    {
        //                        DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //                    }
        //                }
        //                else
        //                {
        //                    if (i > 0)
        //                    {
        //                        DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
        //                        ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
        //                    }
        //                }
        //                ViewState["vDepartmentID"] = DepartmentID;
        //                if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
        //                {
        //                    CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //                }
        //                else
        //                {
        //                    if (i > 0)
        //                    {
        //                        CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
        //                        ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();
        //                    }

        //                }

        //                //====================Modified By Rimi on 27th July 2015 End======================
        //            }
        //            else
        //            {
        //                retval = false;
        //            }



        //            if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
        //            {
        //                retval = true;
        //                BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
        //            }

        //            PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);

        //            // PO VALIDATION START SOUGATA
        //            if (Request.QueryString["NewVendorClass"] !=null)
        //            {

        //                if (Convert.ToString(Request.QueryString["NewVendorClass"]).Trim() == "PO")
        //                {

        //                    if (GetPONumberForSupplierBuyer(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text.Trim()) != "Y")
        //                    {
        //                        Response.Write("<script>alert('Invalid PO Number entered');</script>");
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (Convert.ToString(Session["NewVendorClass"]).Trim() == "PO")
        //                {

        //                    if (GetPONumberForSupplierBuyer(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text.Trim()) != "Y")
        //                    {
        //                        Response.Write("<script>alert('Invalid PO Number entered');</script>");
        //                        break;
        //                    }
        //                }
        //            }
        //            //PO VALIDATION END SOUGATA

        //            // Added by Mrinal on 13th March 2015
        //            LineVAT = 0;

        //            if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
        //            {
        //                //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
        //                //{
        //                    LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
        //               // }
        //            }
        //            strLineDescription = string.Empty;

        //            if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
        //            {
        //                strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
        //            }
        //            // Addition End on 13th March 2015 
        //            //Added By RImi on 8th August 2015

        //            if (strLineDescription.ToString().Contains("<"))
        //            {
        //                strLineDescription = strLineDescription.Replace("<", "&lt;");
        //            }
        //            if (strLineDescription.ToString().Contains(">"))
        //            {
        //                strLineDescription = strLineDescription.Replace(">", "&gt;");
        //            }
        //            if (strLineDescription.ToString().Contains("£"))
        //            {
        //                strLineDescription = strLineDescription.Replace("£", "&pound;");
        //            }
        //            if (strLineDescription.ToString().Contains("€"))
        //            {
        //                strLineDescription = strLineDescription.Replace("€", "&belongsto;");
        //            }

        //            NetValue = 0;
        //            if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim() != "")
        //            {
        //                //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
        //                //{
        //                    NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
        //                //}
        //                //else
        //                //    retval = false;
        //            }
        //            else
        //                retval = false;
        //            #endregion

        //            if (NetValue > 0 && retval == true)
        //            {
        //                flag = true;
        //                DR = dtXML.NewRow();
        //                dtXML.Rows.Add(DR);
        //                sb.Append("<Rowss>");
        //                sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
        //                sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
        //                sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
        //                sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
        //                sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
        //                sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
        //                sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
        //                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
        //                if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
        //                    sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
        //                else
        //                    sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</BusinessUnitID>");

        //                sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
        //                sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
        //                sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");
        //                sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
        //                sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");

        //                sb.Append("</Rowss>");
        //            }
        //        }
        //        dsXML.Tables.Add(dtXML);
        //        sb.Append("</Root>");
        //        //Added By Rimi on 27th July 2015
        //        if (sb.ToString().Contains("&"))
        //        {
        //            sb = sb.Replace("&", "&amp;");
        //        }
        //        if (sb.ToString().Contains("'"))
        //        {
        //            sb = sb.Replace("'", "&apos;");
        //        }
        //        //Added By Rimi on 27th July 2015 End
        //        string strXmlText = sb.ToString();
        //        sb = null;
        //        int retvalalue = 0;
        //        if (flag == true)
        //            retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
        //    }
        //    else
        //    {
        //        flag = false;
        //        lblErrorMsg.Visible = false;
        //        //Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
        //        Response.Write("<script>alert('Variance must be zero.');</script>");
        //    }
        //    #endregion
        //    return flag;
        //}


        //====Added By Rimi on 25th August 2015========

        private bool SaveDetailDataForGMG()
        {

            //int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);// commented by Rimi on 25.06.2015

            // Added by Rimi on 25.06.2015

            int InvID = 0;
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            }
            else
            {
                InvID = Convert.ToInt32(ViewState["CheckList"]);
            }

            // Added by Rimi on 25.06.2015 End

            int CompanyID = 0;
            int CodingDescriptionID = 0;
            int NominalCodeID = 0;
            int BusinessUnitID = 0;
            int DepartmentID = 0;
            decimal NetValue = 0;
            bool flag = false;
            double NetVal = 0;
            // Added by Mrinal on 13th March 2015
            decimal LineVAT = 0;
            string strLineDescription = string.Empty;
            // Addition End on 13th March 2015 

            string PurOrderNo = "";

            bool retval = false;
            lblErrorMsg.Visible = false;

            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (grdList.Items[i].ItemType == ListItemType.Item || grdList.Items[i].ItemType == ListItemType.AlternatingItem)
                {
                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.ToString() != "")
                    {
                        NetVal = NetVal + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                        Response.Write("<script>alert('Please enter Net Value for coding at line(s)..');</script>");
                        return flag;
                    }
                }
            }
            DataSet dsXML = new DataSet();
            DataTable dtXML = new DataTable();
            dtXML.Columns.Add("SlNo");
            dtXML.Columns.Add("InvoiceID");
            dtXML.Columns.Add("InvoiceType");
            dtXML.Columns.Add("CompanyID");
            dtXML.Columns.Add("CodingDescriptionID");
            dtXML.Columns.Add("DepartmentID");
            dtXML.Columns.Add("NominalCodeID");
            dtXML.Columns.Add("BusinessUnitID");
            dtXML.Columns.Add("NetValue");
            dtXML.Columns.Add("CodingValue");
            dtXML.Columns.Add("PurOrderNo");

            // Added by Mrinal on 13th March 2015
            dtXML.Columns.Add("LineVAT");
            dtXML.Columns.Add("LineDescription");
            // Addition End on 13th March 2015 

            DataRow DR = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            NetVal = Math.Round(NetVal, 2);
            ViewState["NetValINV"] = NetVal;
            ViewState["NetAmt"] = NetVal;// Added By Rimi on 22nd July 2015
            #region: Variance Validation
            if (Convert.ToDouble(ViewState["NetAmt"]) == NetVal || IsCalledFromOpenButton == 1)
            {
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {
                    retval = true;
                    #region Getting DropDown Values
                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) != "--Select--")
                    {
                        CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                    }
                    else
                        retval = false;

                    if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
                    {
                        string a = ((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim();
                        //Boolean res = GetCodingDetails(a.Substring(0,8));
                        int index = a.IndexOf("[");
                        if (index > 0)
                        {
                            Boolean res = GetCodingDetails(a.Substring(0, index));
                            if (res == false)
                            {

                                Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
                                return false;

                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
                            return false;
                        }
                        //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                        //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                        //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                        //=====================Modified By Rimi on 27th July 2015=============================
                        if (i > 0)
                        {
                            //if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim() != ((TextBox)grdList.Items[i - 1].FindControl("txtAutoCompleteCodingDescription")).Text.Trim())
                            //{
                            NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                            ViewState["hdnDepartmentCodeID"] = "2";
                            //}

                            //else
                            //{
                            //    NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
                            ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = NominalCodeID.ToString();
                            //    ViewState["hdnDepartmentCodeID"] = "1";
                            //}
                        }
                        else
                        {
                            NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                            ViewState["hdnDepartmentCodeID"] = "2";
                        }
                        //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                        //{
                        if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
                        {
                            DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                        }
                        //}
                        //else
                        //{
                        //    if (i > 0)
                        //    {
                        //        DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
                        //    }
                        //}
                        ViewState["vDepartmentID"] = DepartmentID;
                        //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                        //{
                        CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                        //}
                        //else
                        //{
                        //    if (i > 0)
                        //    {
                        //        CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();
                        //    }

                        //}

                        //====================Modified By Rimi on 27th July 2015 End======================
                    }
                    else
                    {
                        retval = false;
                    }



                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
                    {
                        retval = true;
                        BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
                    }

                    PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);

                    //Blocked by Mainak 2018-08-30
                    // PO VALIDATION START SOUGATA
                    //if (Request.QueryString["NewVendorClass"] != null)
                    //{

                    //    if (Convert.ToString(Request.QueryString["NewVendorClass"]).Trim() == "PO")
                    //    {

                    //        if (GetPONumberForSupplierBuyer(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text.Trim()) != "Y")
                    //        {
                    //            Response.Write("<script>alert('Invalid PO Number entered');</script>");
                    //            break;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (Convert.ToString(Session["NewVendorClass"]).Trim() == "PO")
                    //    {

                    //        if (GetPONumberForSupplierBuyer(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text.Trim()) != "Y")
                    //        {
                    //            Response.Write("<script>alert('Invalid PO Number entered');</script>");
                    //            break;
                    //        }
                    //    }
                    //}
                    //PO VALIDATION END SOUGATA

                    // Added by Mrinal on 13th March 2015
                    LineVAT = 0;

                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
                    {
                        //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
                        //{
                        LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
                        // }
                    }
                    strLineDescription = string.Empty;

                    if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
                    {
                        strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
                    }
                    // Addition End on 13th March 2015 
                    //Added By RImi on 8th August 2015

                    if (strLineDescription.ToString().Contains("<"))
                    {
                        strLineDescription = strLineDescription.Replace("<", "&lt;");
                    }
                    if (strLineDescription.ToString().Contains(">"))
                    {
                        strLineDescription = strLineDescription.Replace(">", "&gt;");
                    }
                    if (strLineDescription.ToString().Contains("£"))
                    {
                        strLineDescription = strLineDescription.Replace("£", "&pound;");
                    }
                    if (strLineDescription.ToString().Contains("€"))
                    {
                        strLineDescription = strLineDescription.Replace("€", "&belongsto;");
                    }

                    NetValue = 0;
                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim() != "")
                    {
                        //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
                        //{
                        NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                        //}
                        //else
                        //    retval = false;
                    }
                    else
                        retval = false;
                    #endregion

                    if (NetValue >= 0 && retval == true)
                    {
                        flag = true;
                        DR = dtXML.NewRow();
                        dtXML.Rows.Add(DR);
                        sb.Append("<Rowss>");
                        sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
                        sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
                        sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
                        sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
                        sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
                        sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
                        sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
                        int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                        if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
                            sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
                        else
                            sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</BusinessUnitID>");

                        sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
                        sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
                        sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");
                        sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
                        sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");

                        sb.Append("</Rowss>");
                    }
                }
                dsXML.Tables.Add(dtXML);
                sb.Append("</Root>");
                //Added By Rimi on 27th July 2015
                if (sb.ToString().Contains("&"))
                {
                    sb = sb.Replace("&", "&amp;");
                }
                if (sb.ToString().Contains("'"))
                {
                    sb = sb.Replace("'", "&apos;");
                }
                //Added By Rimi on 27th July 2015 End
                string strXmlText = sb.ToString();
                sb = null;
                int retvalalue = 0;
                if (flag == true)
                    retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
            }
            else
            {
                flag = false;
                lblErrorMsg.Visible = false;
                //Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
                Response.Write("<script>alert('Variance must be zero.');</script>");

            }
            #endregion
            return flag;
        }


        #endregion
        #region cboStatus_SelectedIndexChanged
        public void cboStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
        #endregion
        #region ddldept_SelectedIndexChanged
        protected void ddldept_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int dept = 0;
            if (Convert.ToString(ddldept.SelectedValue) != "Select")
                dept = Convert.ToInt32(ddldept.SelectedValue);
            GetApproverDropDownsAgainstDepartment(dept);
            string strCtrl = ddldept.ClientID;
            setFocus(strCtrl);

        }
        #endregion

        #region btndelete_Click
        private void btndelete_Click(object sender, System.EventArgs e)
        {
            Session["button_clicked"] = "1";//added by kuntalkarar on 19thOctober2016
            lblmessage.Text = "";

            if (txtComment.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter a comment.');</script>");
                return;
            }
            else
            {
                JKS.Invoice objinvoice = new JKS.Invoice();
                lblErrorMsg.Text = "";
                lblErrorMsg.Visible = false;
                iApproverStatusID = 7;
                string strComments = txtComment.Text.Trim();
                int DeptUpdate = UpdateDepartmentAgainstInvoiceID();
                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                int StatusUpdate = 0;
                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {
                    StatusUpdate = objinvoice.UpdateInvStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                }
                else
                {

                    StatusUpdate = objinvoice.UpdateInvStatusToDelete(System.Convert.ToInt32(ViewState["InvoiceChecking"]));

                }


                if (StatusUpdate == 1)
                {
                    if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                    {

                        objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                    }
                    else
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");

                    }

                    doAction(0);
                    //Response.Write("<script>alert('Invoice Deleted Successfully.'); self.close();</script>");

                    // Added by Mrinal on 22nd September 2014
                    // Response.Write("<script>alert('Invoice Deleted Successfully.');</script>");
                    // GetURLTest();

                    ViewState["MSG"] = "Delete";// Added By Rimi on 22nd July 2015
                    MoveToNextInvoice();
                    string message = "alert('Invoice Deleted Successfully')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                }
                else
                {
                    //  Response.Write("<script>alert('Invoice cannot be deleted');</script>");
                    string message = "alert('Invoice cannot be deleted')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                }

            }
            //Added by kuntalkarar on 19thOctober2016
            LoadDownloadFiles();
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015
        }
        #endregion

        //===============Added By Rimi on 7th Sept 2015===========================
        public Boolean CheckPassToUserID(string invoiceno)
        {
            DataSet ds = new DataSet();
            bool result = false;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_CheckPassToUserID", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.AddWithValue("@invoiceno", invoiceno);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            if (ds.Tables[0].Rows.Count > 0 && Convert.ToString(ds.Tables[0].Rows[0][0].ToString()) == Convert.ToString(Session["UserID"]) || ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                result = true;
            }
            return result;
        }
        //===============Added By Rimi on 7th Sept 2015 End===========================



        #region private void SendEmail()
        private void SendEmail()
        {
            try
            {
                MailMessage msg = new MailMessage();
                string sBody = "";
                if (Session["eDocType"].ToString().Trim() != "")
                {

                    //-----------------------rinku santra 04-08-2011--------------------------------------------//
                    sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                    SqlDataAdapter dap = new SqlDataAdapter("SP_RejectionMail_gmg", sqlConn);
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                    {

                        dap.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
                    }
                    else
                    {
                        dap.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["InvoiceChecking"]));

                    }


                    dap.SelectCommand.Parameters.Add("@DocType", Session["eDocType"].ToString().ToUpper());
                    DataSet ds = new DataSet();

                    try
                    {
                        sqlConn.Open();

                        dap.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            msg.Subject = "Invoice Rejection (" + ds.Tables[0].Rows[0]["InvoiceNO"].ToString() + ")";
                            sBody = "<BR><BR>The following invoice has been rejected:" +


                                "<BR><BR>Company                  : " + ds.Tables[0].Rows[0]["buyerCompanyName"].ToString() + " " +
                                "<BR><BR>Supplier Name            : " + ds.Tables[0].Rows[0]["supplierCompanyName"].ToString() + " " +
                                "<BR><BR>Vendor ID				  : " + ds.Tables[0].Rows[0]["SupplierCodeAgainstBuyer"].ToString() + " " +
                                "<BR><BR>Invoice Number           : " + ds.Tables[0].Rows[0]["InvoiceNO"].ToString() + " " +

                                "<BR><BR>Kind regards, " +
                                "<BR><BR>P2D Support Team";
                        }
                    }
                    catch (Exception ex)
                    {
                        string strError = ex.Message;
                    }
                    finally
                    {
                        sqlConn.Close();
                        dap.Dispose();
                        ds = null;
                    }

                }

                sBody = "<FONT face=Verdana Size =2pt>" + sBody + "</FONT>";
                msg.To = "P2d@vnsinfo.com.au";
                msg.Bcc = "errorvns@gmail.com,P2d@vnsinfo.com.au";
                msg.From = "support@p2dgroup.com";
                msg.Body = sBody;
                msg.Priority = MailPriority.High;
                msg.BodyFormat = MailFormat.Html;
                SmtpMail.SmtpServer = ConfigurationManager.AppSettings["MailServer"].Trim();
                SmtpMail.Send(msg);
                msg = null;

            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

        }
        #endregion
        #region btnReopen_Click
        private void btnReopen_Click(object sender, System.EventArgs e)
        {
            Session["button_clicked"] = "1";//added by kuntalkarar on 19thOctober2016
            // Commeneted By Rimi on 23rd July 2015
            if (ddldept.SelectedIndex == 0)
            {
                Response.Write("<script>alert('Please select department.');</script>");
                return;
            }

            if (ddlApprover1.SelectedIndex == 0 && ddlApprover2.SelectedIndex == 0 && ddlApprover3.SelectedIndex == 0)
            {
                Response.Write("<script>alert('Please select at least one approver.');</script>");
                return;
            }
            // Commeneted By Rimi on 23rd July 2015



            lblmessage.Text = "";
            int iRejectionCode = 0;
            lblErrorMsg.Visible = false;
            JKS.Invoice objinvoice = new JKS.Invoice();
            if (Convert.ToString(ViewState["StatusID"]) == "6")
            {
                if (txtCreditNoteNo.Text.Trim() == "")
                {
                    if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                    {

                        iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));

                    }
                    else
                    {

                        iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(ViewState["InvoiceChecking"]));

                    }

                    if (iRejectionCode == 1)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                    if (iRejectionCode == 2)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                }
                if (txtCreditNoteNo.Text.Trim().ToUpper() == "REOPEN")
                {
                    if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                    {

                        iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));

                    }
                    else
                    {
                        iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(ViewState["InvoiceChecking"]));

                    }

                    if (iRejectionCode == 1)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                    if (iRejectionCode == 2)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                }
            }

            if (txtComment.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter a comment.');</script>");
                return;
            }
            else
            {
                lblErrorMsg.Text = "";
                iApproverStatusID = 22;
                string strComments = txtComment.Text.Trim();
                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));

                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {

                    iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));

                }
                else
                {
                    iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(ViewState["InvoiceChecking"]));

                }
                if (UserTypeID == 3 || UserTypeID == 2)
                {

                    string strCreditInvoiceNo = "";
                    int iRetValForUpdate = 0;
                    int iInvoiceID = 0;


                    if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                    {
                        iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
                    }
                    else
                    {
                        iInvoiceID = Convert.ToInt32(ViewState["InvoiceChecking"]);
                    }




                    strCreditInvoiceNo = txtCreditNoteNo.Text.Trim();

                    if (strCreditInvoiceNo.Trim().ToUpper() != "REOPEN")
                    {

                        if (strCreditInvoiceNo.Trim() == "" && lblcreditnoteno.Text.Trim() != "")
                        {

                        }
                        else if (strCreditInvoiceNo.Trim() != "")
                        {
                            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                            {

                                iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNo);	//Amitava 020707
                            }
                            else
                            {

                                iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen(System.Convert.ToInt32(ViewState["InvoiceChecking"]), strCreditInvoiceNo);	//Amitava 020707

                            }




                            if (iRetValForUpdate == -101)
                            {
                                Response.Write("<script>alert('The credit note must be in Received or Registered status.');</script>");
                                return;
                            }
                            if (iRetValForUpdate == -102)
                            {
                                Response.Write("<script>alert('The credit note must be in Received or Registered status.');</script>");
                                return;
                            }
                            if (iRetValForUpdate == -103)
                            {
                                Response.Write("<script>alert('Invalid Credit Note Number.');</script>");
                                return;
                            }

                            int iReturnValue = CheckCreditNoteAgainstInvoice(iInvoiceID, strCreditInvoiceNo);
                            if (iReturnValue == -101)
                            {
                                Response.Write("<script>alert('The credit note does not match to the supplier or invoice currency. ');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            else if (iReturnValue == -103)
                            {
                                Response.Write("<script>alert('The credit note must be less than the value of the invoice.');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            else if (iReturnValue == -102 && txtCreditNoteNo.Text.Trim() != "reopen")
                            {
                                Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                        }
                        else if (strCreditInvoiceNo.Trim() != "" && strCreditInvoiceNo.Trim() != lblcreditnoteno.Text.Trim())
                        {
                            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                            {

                                iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNo);
                            }
                            else
                            {
                                iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen(System.Convert.ToInt32(ViewState["InvoiceChecking"]), strCreditInvoiceNo);

                            }



                            if (iRetValForUpdate == -101)
                            {
                                Response.Write("<script>alert('The credit note must be in Received or Registered status.');</script>");
                                return;
                            }
                            if (iRetValForUpdate == -102)
                            {
                                Response.Write("<script>alert('The credit note must be in Received or Registered status.');</script>");
                                return;
                            }
                            if (iRetValForUpdate == -103)
                            {
                                Response.Write("<script>alert('Invalid Credit Note Number.');</script>");
                                return;
                            }

                            int iReturnValue = CheckCreditNoteAgainstInvoice(iInvoiceID, strCreditInvoiceNo);
                            if (iReturnValue == -101)
                            {
                                Response.Write("<script>alert('The credit note does not match to the supplier or invoice currency. ');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            else if (iReturnValue == -103)
                            {
                                Response.Write("<script>alert('The credit note must be less than the value of the invoice.');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }
                            else if (iReturnValue == -102 && txtCreditNoteNo.Text.Trim() != "reopen")
                            {
                                Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                                int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                return;
                            }

                        }
                    }

                    int DeptUpdate = UpdateDepartmentAgainstInvoiceID();

                    if ((txtCreditNoteNo.Text.Trim() == "" && iRejectionCode == 6) || txtCreditNoteNo.Text.Trim().ToUpper() == "REOPEN")
                    {
                        int iret = 0;
                        bool ret = SaveDetailDataForGMG();
                        if (ret == true)
                        {
                            if (chbOpen.Checked == true)
                                iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 1);
                            else
                                iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 0);
                        }
                        if (iret == 1)
                        {
                            lblErrorMsg.Visible = false;
                            doAction(0);
                            //AP-Admin Reopens
                            //Response.Write("<script>alert('Invoice Reopened Successfully'); self.close();</script>");
                            // Added by Mrinal on 22nd September 2014
                            //  Response.Write("<script>alert('Invoice Reopened Successfully.');</script>");


                            ViewState["MSG"] = "ReOpen";// Added By Rimi on 22nd July 2015
                            MoveToNextInvoice();



                            string message = "alert('Invoice Reopened Successfully.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                        }
                        else
                        {
                            string message = "alert('Invoice status cannot be reopened')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                            //     Response.Write("<script>alert('Invoice status cannot be reopened');</script>");
                            return;
                        }
                    }
                    else if (strCreditInvoiceNo.Trim() == "" && lblcreditnoteno.Text.Trim() != "")
                    {
                        int iret = 0;
                        bool ret = SaveDetailDataForGMG();
                        if (ret == true)
                        {//End
                            if (chbOpen.Checked == true)
                                iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 1);
                            else
                                iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 0);
                        }
                        if (iret == 1)
                        {
                            lblErrorMsg.Visible = false;
                            doAction(0);
                            //AP-Admin Reopens
                            //Response.Write("<script>alert('Invoice Reopened Successfully'); self.close();</script>");
                            // Added by Mrinal on 22nd September 2014

                            ViewState["MSG"] = "ReOpen";// Added By Rimi on 22nd July 2015
                            MoveToNextInvoice();
                            string message = "alert('Invoice Reopened Successfully.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);


                        }
                        else
                        {

                            string message = "alert('Invoice status cannot be reopened.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);


                            //  Response.Write("<script>alert('Invoice status cannot be reopened');</script>");
                            return;
                        }
                    }
                    else
                    {
                        int iret = 0;

                        bool ret = SaveDetailDataForGMG();
                        if (ret == true)
                        {
                            if (chbOpen.Checked == true)
                                iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 1);
                            else
                                iret = SetDropDownValuesOnPressingReopen(System.Convert.ToInt32(Session["UserID"]), 0);
                        }
                        if (iret == 1)
                        {
                            lblErrorMsg.Visible = false;
                            doAction(0);
                            //Response.Write("<script>alert('Invoice Reopened Successfully'); self.close();</script>");
                            // Added by Mrinal on 22nd September 2014

                            ViewState["MSG"] = "ReOpen";// Added By Rimi on 22nd July 2015
                            MoveToNextInvoice();
                            string message = "alert('Invoice Reopened Successfully.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);


                        }
                        else
                        {
                            //   Response.Write("<script>alert('Invoice status cannot be reopened');</script>");

                            string message = "alert('Invoice status cannot be reopened')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);



                            return;
                        }
                    }
                }
            }
            //Added by kuntalkarar on 19thOctober2016
            LoadDownloadFiles();
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015
        }
        #endregion

        #region btnOpen_Click
        private void btnOpen_Click(object sender, System.EventArgs e)
        {
            Session["button_clicked"] = "1";//added by kuntalkarar on 19thOctober2016
            // Commeneted By Rimi on 23rd July 2015
            if (ddldept.SelectedIndex == 0)
            {
                Response.Write("<script>alert('Please select department.');</script>");
                return;
            }

            if (ddlApprover1.SelectedIndex == 0 && ddlApprover2.SelectedIndex == 0 && ddlApprover3.SelectedIndex == 0)
            {
                Response.Write("<script>alert('Please select at least one approver.');</script>");
                return;
            }
            // Commeneted By Rimi on 23rd July 2015


            lblmessage.Text = "";

            int StatusID = 0;
            if (ViewState["StatusID"] != null)
            {
                StatusID = Convert.ToInt32(ViewState["StatusID"]);
                if (StatusID == 20)
                {
                    IsCalledFromOpenButton = 1;
                }
                else
                {
                    IsCalledFromOpenButton = 0;
                }

            }

            bool ret = SaveDetailDataForGMG();
            if (ret == true)
            {
                if (Convert.ToDouble(ViewState["NetAmt"]) == Convert.ToDouble(ViewState["NetValINV"]) || IsCalledFromOpenButton == 1)
                {

                    int i = SetDropDownValuesOnOpen(System.Convert.ToInt32(Session["UserID"].ToString()));
                    if (i > 0)
                    {
                        int DeptUpdate = UpdateDepartmentAgainstInvoiceID();
                        //Response.Write("<script>alert('Invoice passed to user successfully.'); self.close();</script>");
                        // Added by Mrinal on 22nd September 2014
                        //   Response.Write("<script>alert('Invoice passed to user successfully.');</script>");
                        //GetURLTest();

                        ViewState["MSG"] = "Open";// Added By Rimi on 22nd July 2015
                        MoveToNextInvoice();
                        string message = "alert('Invoice passed to user successfully.')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);





                    }
                }
            }
            //Added by kuntalkarar on 19thOctober2016
            LoadDownloadFiles();
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015

        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Session["button_clicked"] = "1";//ViewState["button_Clicked"] = "1";

            ViewState["CancelFlag"] = "1";// Added by Rimi on 22nd July 2015
            ViewState["Flag_Can"] = "Cancel";
            lblmessage.Text = "";
            MoveToNextInvoice();
            //Added by kuntalkarar on 19thOctober2016
            LoadDownloadFiles();
        }
        #endregion
        #region btnReject_Click
        private void btnReject_Click(object sender, System.EventArgs e)
        {

            Session["button_clicked"] = "1";//added by kuntalkarar on 19thOctober2016

            lblmessage.Text = "";
            int RetStatus = 0;

            bool retVal = true;
            lblErrorMsg.Visible = false;
            ViewState["iApproverStatusID"] = "6";
            iApproverStatusID = 6;
            JKS.Invoice objinvoice = new JKS.Invoice();
            if (tbRejection.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter a rejection comment.');</script>");
                retVal = false;
                return;
            }

            if (ddlRejection.SelectedIndex == 0)
            {
                Response.Write("<script>alert('Please select rejection code.');</script>");
                retVal = false;
                return;
            }

            //===============Added By Rimi on 7th Sept 2015===========================

            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
            {
                Boolean res1;
                if (Convert.ToInt64(ViewState["InvoiceChecking"]) == 0)
                {
                    res1 = CheckPassToUserID(Convert.ToString(Session["eInvoiceID"]));

                }
                else
                {
                    res1 = CheckPassToUserID(Convert.ToString(ViewState["InvoiceChecking"]));

                }
                if (res1 == false)
                {
                    Response.Redirect("../Current/CRNclosewindows.aspx");
                }
            }
            //===============Added By Rimi on 7th Sept 2015===========================

            int DeptUpdate = UpdateDepartmentAgainstInvoiceID();
            if (retVal == true)
            {
                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {
                    RetStatus = CheckPermissionToTakeAction(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), System.Convert.ToInt32(Session["UserID"]));
                }
                else
                {
                    RetStatus = CheckPermissionToTakeAction(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]));

                }
                if (RetStatus > 0)
                {

                    lblErrorMsg.Text = "";
                    string strComments = tbRejection.Text.Trim();

                    int Result = 0;
                    bool ret = SaveDetailDataForGMG();
                    if (ret == true)
                    {
                        if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                        {

                            Result = objinvoice.UpdateStatusToReject(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), ddlRejection.SelectedItem.Text.ToString(), strComments, Convert.ToInt32(ddlRejection.SelectedValue), System.Convert.ToInt32(Session["UserID"].ToString()), txtComment.Text.ToString());
                        }
                        else
                        {
                            Result = objinvoice.UpdateStatusToReject(System.Convert.ToInt32(ViewState["InvoiceChecking"]), ddlRejection.SelectedItem.Text.ToString(), strComments, Convert.ToInt32(ddlRejection.SelectedValue), System.Convert.ToInt32(Session["UserID"].ToString()), txtComment.Text.ToString());

                        }


                    }
                    if (Result == 2)
                    {
                        //blocked by kuntalkarar on 10thMay2017
                        //SendEmail();
                        doAction(0);
                        //Response.Write("<script>alert('Invoice Rejected Successfully'); self.close();</script>");
                        // Added by Mrinal on 22nd September 2014
                        //  Response.Write("<script>alert('Invoice Rejected Successfully.');</script>");
                        // GetURLTest();

                        ViewState["RejectOnce"] = "1";
                        ViewState["RejectFlag"] = "yes";// Added By Rimi on 22nd July 2015
                        ViewState["MSG"] = "Reject";// Added By Rimi on 22nd July 2015
                        MoveToNextInvoice();
                        string message = "alert('Invoice Rejected Successfully.')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);


                    }
                    else if (Result == 3)
                    {

                        string message = "var r = alert('You have already actioned invoice. Please press OK to close the window to allow you to resume.');if (typeof r == 'undefined') {close();}";

                        //  string message = "alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                        // Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                        return;
                    }
                    else
                    {
                        string message = "alert('Invoice Already Rejected')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                        // Response.Write("<script>alert('Invoice Already Rejected');</script>");
                    }

                }
                else
                {
                    //   Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");


                    Response.Write("<script>var r = alert('You have already actioned invoice. Please press OK to close the window to allow you to resume.');if (typeof r == 'undefined') {close();}</script>");


                    return;
                }
            }
            //Added by kuntalkarar on 19thOctober2016
            LoadDownloadFiles();
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015

        }
        #endregion
        //added by kuntalkarar on 1stMarch2017
        #region btnRematch_Click
        protected void btnRematch_Click(object sender, EventArgs e)
        {
            Session["button_clicked"] = "1";
            string InvoiceID_Remtach = "";

            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {


                InvoiceID_Remtach = Session["eInvoiceID"].ToString();

            }

            else
            {

                InvoiceID_Remtach = ViewState["InvoiceChecking"].ToString();

            }
            int iCount = 0;
            SqlParameter sqlOutputParam = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);

            SqlCommand sqlCmd = new SqlCommand("sp_ButtonRematchPress_Generic", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID_Remtach);
            sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"]));
            sqlCmd.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));

            if (txtComment.Text.Length > 0)
            {
                sqlCmd.Parameters.Add("@Comments", Convert.ToString(txtComment.Text));
            }

            else
            {
                sqlCmd.Parameters.Add("@Comments", "Rematch");
            }

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                DataTable dt1 = GetInvoiceDetailsByInvoiceID(Convert.ToInt32(InvoiceID_Remtach));
                string docID = dt1.Rows[0]["DocID"].ToString();

                string strFileName = "";
                byte[] objFile = null;
                string strReturn = "";

                int cid = Convert.ToInt32(dt1.Rows[0]["BuyerCompanyID"]);
                string CompanyName = objCN.ReturnCompanyNameByID(cid);

                #region image_path
                string OriginalName = "";
                string BatchID = "";
                string BatchName = "";

                DataTable dt = objCN.ReturnTopDataTable(Convert.ToInt32(docID));
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    OriginalName = dr["ORIGINAL NAME"].ToString();
                    BatchID = dr["BATCH ID"].ToString();
                    BatchName = dr["BATCH NAME"].ToString();
                }

                string image_path = @"""C:\P2D\FTP Upload\" + CompanyName
                    + @"\" + BatchName + @"\" + OriginalName + @"""";

                /* file reprocesing as per James request from "CTB Issues.docx"*/
                string image_path2 = @"""\\90107-server3\C$\File Repository\FTP Archive\" + CompanyName
                    + @"\" + BatchName + "_" + BatchID + @"\" + OriginalName + @"""";

                #endregion

                #region output_path

                string CompanyID = dt1.Rows[0]["BuyerCompanyID"].ToString();
                DataTable batchTypeDetailsDT = objCN.GetBatchTypes(Convert.ToInt32(CompanyID));
                string BatchTypeID = batchTypeDetailsDT.Rows[0]["BatchTypeID"].ToString();

                string output_path = @"""C:\P2D\Email Processing\" + CompanyName + @"\000;" + CompanyID + ";" + BatchTypeID + @"""";
                #endregion

                #region page_numbers
                //string page_numbers = "";
                //int i = 1;
                //foreach (Panel pnl in pnlSelectPages.Controls)
                //{
                //    CheckBox cb = pnl.Controls[1] as CheckBox;

                //    if (cb.Checked)
                //        page_numbers += i + ",";

                //    i++;
                //}
                //i = page_numbers.LastIndexOf(',');
                //page_numbers = @"""" + page_numbers.Remove(i, 1) + @"""";
                #endregion

                #region Create Temp File
                DateTime DT_Now = DateTime.Now;

                string strDate = DT_Now.ToString("ddMMyy");
                string strTime = DT_Now.ToString("hhmmss");

                string txtFileName = "DOC" + docID + "_" + strDate + "_" + strTime + ".txt";
                string TempFilePath = Server.MapPath("~") + "\\Temp\\" + txtFileName;

                string fileInnerText = "2tiff.exe -src " + image_path + " -dst " + output_path
                    //+ " -options pages:" + page_numbers
                   + " -options "
                      + " template:{Title}-{Date}{Time}{MSec}.{Type} -tiff bpp:1 compression:fax4\n2tiff.exe –src "
                  + image_path2 + " -dst " + output_path
                    //+ " -options pages:" + page_numbers
                   + " -options "
                    + " template:{Title}-{Date}{Time}{MSec}.{Type} -tiff bpp:1 compression:fax4";

                System.IO.File.WriteAllText(TempFilePath, fileInnerText);

                objFile = System.IO.File.ReadAllBytes(TempFilePath);
                System.IO.File.Delete(TempFilePath);
                #endregion

                if (test)
                {
                    strFileName = @"C:\P2D\Reprocessing Requests\" + txtFileName;
                    System.IO.File.WriteAllBytes(strFileName, objFile);
                }
                else
                {
                    string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ReprocessingRequestsWS"];
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = serviceUrl;

                    strFileName = @"C:\P2D\Reprocessing Requests\" + txtFileName;
                    objService.UploadFile(strFileName, objFile, strReturn);

                    Response.Write(strReturn);
                }

                MoveToNextInvoice();
                LoadDownloadFiles();
                //iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }



        }
        #endregion
        #region btnApprove_Click
        private void btnApprove_Click(object sender, System.EventArgs e)
        {
            Session["button_clicked"] = "1";
            lblmessage.Text = "";
            int ApprovedStatus = 0;
            int RetStatus = 0;
            int iRetValForUpdate = 0;
            int iFlag = 1;
            string lineNO = string.Empty;
            int iOCount = 0;
            //Added by Mainak 2018-09-06
            decimal NetValue = 0;
            decimal LineVAT = 0;
            decimal VatRate = 0;

            int vFlag = 0;

            int vRinfinityFlag = 0;
            int vRInfiFlag = 0;


            //===============Added By Rimi on 7th Sept 2015===========================

            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
            {
                Boolean res1;
                if (Convert.ToInt64(ViewState["InvoiceChecking"]) == 0)
                {
                    res1 = CheckPassToUserID(Convert.ToString(Session["eInvoiceID"]));
                }
                else
                {
                    res1 = CheckPassToUserID(Convert.ToString(ViewState["InvoiceChecking"]));
                }
                if (res1 == false)
                {
                    Response.Redirect("../Current/CRNclosewindows.aspx");
                }
            }
            //===============Added By Rimi on 7th Sept 2015===========================

            //Added by Mainak 2018-09-06

            DataTable dtLVt = objCN.GetLineVatByBuyerCompanyId(Convert.ToInt32(Session["BuyerCID"]));

            DataTable dtTXRt = objCN.GetTaxRateByBuyerCompanyId(Convert.ToInt32(Session["BuyerCID"]));


            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                int compid = 0;
                int iddlDept = 0;
                int iNomin = 0;
                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
                {
                    //iNomin = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                    //iddlDept = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                    //=====================Modified By Rimi on 27th July 2015=============================
                    if (i > 0)
                    {
                        if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim() != ((TextBox)grdList.Items[i - 1].FindControl("txtAutoCompleteCodingDescription")).Text.Trim())
                        {
                            if ((((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value) != "")
                            {
                                iNomin = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                            }
                            ViewState["hdnDepartmentCodeID"] = "2";
                        }

                        else
                        {
                            if (((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value != "")
                            {
                                iNomin = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
                                ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = iNomin.ToString();
                            }
                            ViewState["hdnDepartmentCodeID"] = "1";
                        }
                    }
                    else
                    {
                        if ((((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value) != "")
                        {
                            iNomin = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                        }
                        ViewState["hdnDepartmentCodeID"] = "2";
                    }
                    if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                    {
                        if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
                        {
                            iddlDept = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                        }
                    }
                    else
                    {
                        iddlDept = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = iddlDept.ToString();
                    }


                    //====================Modified By Rimi on 27th July 2015 End======================                   

                }

                //Added by Mainak 2018-09-06
                NetValue = 0;
                if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
                {
                    if ((Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0) || (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) < 0))
                    {
                        NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                    }
                }

                LineVAT = 0;

                if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
                {
                    if ((Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0) || (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) < 0))
                    {
                        LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
                    }
                }

                if (NetValue != 0)
                {
                    VatRate = Math.Round((LineVAT / NetValue), 2);

                }
                else
                {
                    vRinfinityFlag++;
                }

                if (vRinfinityFlag == 0)
                {
                    if (dtTXRt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtTXRt.Rows.Count; j++)
                        {
                            if ((VatRate == Convert.ToDecimal(dtTXRt.Rows[j]["TaxRate"])) || ((VatRate == Convert.ToDecimal(dtTXRt.Rows[j]["TaxRate"])) && (vFlag > 0)))
                            {
                                vFlag = 0;
                                break;
                            }
                            else
                                vFlag++;

                        }
                        if (vFlag > 0)
                        {
                            vRFlag++;
                        }
                    }
                }

                if (vRinfinityFlag > 0)
                {
                    vRInfiFlag++;
                    vRinfinityFlag = 0;
                }





                int iVal = ValidationForCodingDescription(iddlDept, iNomin, compid);
                if (iVal == 0)
                {
                    iFlag = 0;
                    break;
                }

                //Added by Mainak 2018-08-23
                bool? iOrder = ValidationForCodingDescriptionByInternalOrder(iddlDept, iNomin, compid);


                if (iOrder == true && (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.ToString() == "0"))
                {
                    lineNO = lineNO + "," + (((System.Web.UI.WebControls.Label)grdList.Items[i].FindControl("lblLineNo")).Text);
                    iOCount++;
                }
                else if (iOrder == false && (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.ToString() != "0"))
                {
                    lineNO = lineNO + "," + (((System.Web.UI.WebControls.Label)grdList.Items[i].FindControl("lblLineNo")).Text);
                    iOCount++;
                }




                //Blocked by Mainak 2018-08-08
                //if (Convert.ToString(Request.QueryString["NewVendorClass"]).Trim() == "PO")// Commented By Rimi on 22nd July 2015
                //if (Convert.ToString(Request.QueryString["NewVendorClass"]) == "PO")
                //{
                //    if (GetPONumberForSupplierBuyer(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text) != "Y")
                //    {
                //        Response.Write("<script>alert('Invalid PO Number entered');</script>");
                //        return;
                //    }
                //}

            }
            //Added by Mainak 2018-08-23
            if (lineNO != "")
            {
                if (lineNO.Substring(0, 1) == ",")
                {
                    lineNO = lineNO.Remove(0, 1);
                }
            }

            if (dtLVt.Rows.Count > 0)
            {
                if ((dtLVt.Rows[0]["LineVAT"].ToString()).ToLower() == "true")
                {
                    vRFlag = vRFlag;
                }
                else
                {
                    vRFlag = 0;
                }
            }
            else
            {
                vRFlag = 0;
            }

            //Rinku 25-10-2011
            if (iFlag > 0)
            {
                if (iOCount == 0)
                {
                    if (vRInfiFlag == 0)
                    {
                        #region Approve
                        lblErrorMsg.Text = "";
                        if (lblcreditnoteno.Text != "")
                        {
                            string[] arrCrnNos1 = lblcreditnoteno.Text.Split('$');
                            int CrnIDss = 0;
                            for (int i = 0; i < arrCrnNos1.Length; i++)
                            {
                                if (arrCrnNos1[i].ToString() != "")
                                {
                                    CrnIDss = GetCreditNoteIDByCreditNoteNo(Convert.ToString(arrCrnNos1[i]));
                                    int iCheckMultipleCoding = CheckCodingINVCRN(CrnIDss, "CRN");
                                    if (iCheckMultipleCoding == 0)
                                    {
                                        Response.Write("<script>alert('Please apply coding to the Associated Credit Note');</script>");
                                        return;
                                    }
                                }
                            }
                        }
                        int UserTypeID = Convert.ToInt32(Session["UserTypeID"]);
                        if (UserTypeID < 2 && Convert.ToString(ViewState["StatusID"]) == "6")
                        {
                            if (txtCreditNoteNo.Text.Trim() == "" && lblcreditnoteno.Text == "")
                            {
                                Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                                return;
                            }
                        }


                        if (Convert.ToInt32(ddlRejection.SelectedIndex) > 0)
                        {
                            Response.Write("<script>alert('Sorry,Invoice cannot be Approved. You have selected RejectionCode.');</script>");
                            return;
                        }
                        JKS.Invoice objinvoice = new JKS.Invoice();
                        string strComments = txtComment.Text.Trim();
                        //int UserTypeID =objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                        if (UserTypeID == 3 || UserTypeID == 2)
                        {
                            string strCreditInvoiceNoTemp = txtCreditNoteNo.Text.Trim();
                            if (txtCreditNoteNo.Visible == true && txtCreditNoteNo.Text.Trim() != "")
                            {
                                string strCreditInvoiceNo = CheckCreditNoteAgainstInvoice();

                                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                                {


                                    iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNoTemp);

                                }

                                else
                                {

                                    iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(System.Convert.ToInt32(ViewState["InvoiceChecking"]), strCreditInvoiceNoTemp);

                                }


                                if (iRetValForUpdate == -101)
                                {
                                    Response.Write("<script>alert('Please apply coding to the associated credit note.');</script>");
                                    return;
                                }
                                if (iRetValForUpdate == -102)
                                {
                                    Response.Write("<script>alert('Please apply coding to the associated credit note.');</script>");
                                    return;
                                }
                                if (iRetValForUpdate == -103)
                                {
                                    Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                                    return;
                                }

                                int retVal = CheckIsFullCreditNote();
                                if (retVal == 0)
                                {
                                    Response.Write("<script>alert('Sorry, you cannot approve a partial credit.');</script>");
                                    int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                                    return;
                                }
                                else
                                {

                                    int iCheckCoding = CheckCodingINVCRN(iRetValForUpdate, "CRN");
                                    if (iCheckCoding == 0)
                                    {
                                        Response.Write("<script>alert('Please apply coding to the Associated Credit Note');</script>");
                                        return;
                                    }

                                    if (CheckVarience())
                                    {
                                        if (vRFlag == 0)
                                        {
                                            bool ret = SaveDetailData();

                                            if (ret == true)
                                            {
                                                int DeptUpdate = UpdateDepartmentAgainstInvoiceID();


                                                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                                                {
                                                    ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), iRetValForUpdate, "");
                                                }
                                                else
                                                {
                                                    ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), iRetValForUpdate, "");
                                                }
                                                if (ApprovedStatus == 1)
                                                {
                                                    doAction(0);
                                                    PasswordReset objPasswordReset = new PasswordReset();

                                                    if (Convert.ToInt64(ViewState["InvoiceChecking"]) == 0)
                                                    {

                                                        objPasswordReset.UpdateDepartmentId(Convert.ToString(Session["eInvoiceID"]), Convert.ToString(Session["InvoiceBuyerCompany"]), Convert.ToString(ViewState["vDepartmentID"]), Convert.ToString(ViewState["vCodingDescriptionID"]));

                                                    }
                                                    else
                                                    {
                                                        objPasswordReset.UpdateDepartmentId(Convert.ToString(ViewState["InvoiceChecking"]), Convert.ToString(Session["InvoiceBuyerCompany"]), Convert.ToString(ViewState["vDepartmentID"]), Convert.ToString(ViewState["vCodingDescriptionID"]));

                                                    }
                                                    GetDocumentDetails(Convert.ToInt32(Session["eInvoiceID"]));
                                                    // GetApproverDropDownsAgainstDepartment(Convert.ToInt32(ViewState["DepartmentID"]));

                                                    ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                                                    MoveToNextInvoice();
                                                    string message = "alert('Invoice Approved Successfully.')";
                                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);


                                                }
                                                else if (ApprovedStatus == -111)
                                                {

                                                    string message = "alert('The associated credit note has not been coded')";
                                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                                                    //   Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                                                    return;
                                                }
                                                else
                                                {
                                                    string message = "alert('Data not saved properly.')";
                                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                                    //  Response.Write("<script>alert('Data not saved properly.');</script>");
                                                }

                                            }
                                        }
                                        else
                                        {
                                            string message = "alert('VAT ÷ Net does not equal a valid tax rate.')";
                                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                        }
                                    }
                                    else
                                    {
                                        Response.Write("<script>alert('Variance must be zero.');</script>");
                                    }
                                }

                            }
                            else
                            {
                                if (CheckVarience())
                                {
                                    if (vRFlag == 0)
                                    {
                                        bool ret = SaveDetailData();

                                        if (ret == true)
                                        {
                                            int DeptUpdate = UpdateDepartmentAgainstInvoiceID();

                                            int ICrnID = 0;
                                            ICrnID = GetCreditNoteIDAgainstInvoiceIDANDCompanyID();
                                            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                                            {

                                                ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), ICrnID, "");
                                            }
                                            else
                                            {
                                                ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), ICrnID, "");

                                            }

                                            if (ApprovedStatus == 1)
                                            {
                                                doAction(0);
                                                //Response.Write("<script>alert('Invoice Approved Successfully'); self.close();</script>");
                                                // Added by Mrinal on 22nd September 2014
                                                //  Response.Write("<script>alert('Invoice Approved Successfully.');</script>");
                                                // GetURLTest();

                                                ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                                                MoveToNextInvoice();
                                                string message = "alert('Invoice Approved Successfully.')";
                                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                                            }
                                            else if (ApprovedStatus == -111)
                                            {
                                                //Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                                                //MoveToNextInvoice();
                                                string message = "alert('The associated credit note has not been coded')";
                                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                                return;
                                            }
                                            else
                                            {
                                                string message = "alert('Data not saved properly.')";
                                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                                //   Response.Write("<script>alert('Data not saved properly.');</script>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string message = "alert('VAT ÷ Net does not equal a valid tax rate.')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    }
                                }
                                else
                                {
                                    string message = "alert('Variance must be zero.')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    // Response.Write("<script>alert('Variance must be zero.');</script>");
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                            {
                                RetStatus = CheckPermissionToTakeAction(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), System.Convert.ToInt32(Session["UserID"]));
                            }
                            else
                            {
                                RetStatus = CheckPermissionToTakeAction(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]));

                            }

                            if (RetStatus > 0)
                            {
                                iApproverStatusID = 19;
                                if (CheckVarience())
                                {
                                    if (vRFlag == 0)
                                    {
                                        bool ret = SaveDetailData();

                                        if (ret == true)
                                        {
                                            int ICrnID = 0;
                                            ICrnID = GetCreditNoteIDAgainstInvoiceIDANDCompanyID();

                                            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                                            {
                                                ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), ICrnID, "");
                                            }
                                            else
                                            {
                                                ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), ICrnID, "");

                                            }

                                            if (ApprovedStatus == 1)
                                            {
                                                doAction(0);
                                                lblErrorMsg.Text = "Invoice Approved Successfully";
                                                //	Response.Write("<script>alert('Invoice Approved Successfully'); self.close();</script>");

                                                // Added by Mrinal on 22nd September 2014
                                                //    Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                                                //    GetURLTest();

                                                ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                                                MoveToNextInvoice();
                                                string message = "alert('Invoice Approved Successfully')";
                                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                            }
                                            else if (ApprovedStatus == -112)
                                            {
                                                // Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                                                string message = "var r = alert('You have already actioned this invoice. Please press OK to close the window to allow you to resume.');if (typeof r == 'undefined') {close();}";//Added By Subhrajyoti on 28th July 2015
                                                // string message = "alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.')";
                                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                                return;
                                            }
                                            else if (ApprovedStatus == -111)
                                            {
                                                // Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                                                string message = "alert('The associated credit note has not been coded')";
                                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                                return;
                                            }
                                            else
                                            {
                                                //   Response.Write("<script>alert('Data not saved properly.');</script>");
                                                string message = "alert('Data not saved properly.')";
                                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        string message = "alert('VAT ÷ Net does not equal a valid tax rate.')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    }
                                }
                                else
                                {
                                    //  Response.Write("<script>alert('Variance must be zero.');</script>");
                                    string message = "alert('Variance must be zero.')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }

                            }
                            else
                            {
                                // Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                                Response.Write("<script>var r = alert('You have already actioned this invoice. Please press OK to close the window to allow you to resume.');if (typeof r == 'undefined') {close();}</script>");// Added By Subhrojyoti on 28th July 2015
                                return;
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        hdnVRflag.Value = vRFlag.ToString();
                        overlayApprove.Visible = true;
                        dialogApprove.Visible = true;
                    }
                }
                else
                {
                    Response.Write("<script>alert('Internal Order should / should not have been selected on coding lines " + lineNO + ".');</script>");//Commeneted By RImi on 25thAugust2015
                }

            }
            else
            {
                //  Response.Write("<script>alert('Invalid Combination of Company/Department/Nominal.');</script>");//Commeneted By RImi on 25thAugust2015
                Response.Write("<script>alert('Coding error. Please delete the contents of the Coding field, re-select it from the pop-up list, save the coding, and then re-attempt the approval.');</script>");//Added By Rimi on 25thAugust2015
            }


            LoadDownloadFiles();
            ViewState["hdnDepartmentCodeID"] = "2";
        }
        #endregion
        private void MoveToNextInvoice()
        {
            string strDocType = "";
            try
            {
                int counter;
                if (ViewState["Counter"] == null)
                {
                    counter = 1;
                }
                else
                {
                    counter = (int)ViewState["Counter"] + 1;
                }

                ViewState["Counter"] = counter;

                int InvoiceId = Convert.ToInt32(returnInvoiceId(Convert.ToInt32(ViewState["Counter"])));
                Session["AttInvoice"] = InvoiceId;
                Session["next"] = "next";
                ViewState["CheckList"] = InvoiceId;// Added by Rimi on 25/06.2015
                dNetAmt = 0;// Added by Rimi on 25/06.2015
                ViewState["approvalpath"] = "";// Added by Rimi on 25/06.2015
                Session["NewInvoiceId"] = InvoiceId; // Added by kd on 04Jan2019


                foreach (INVS str in (List<INVS>)Session["InvoiceID"])
                {
                    if (str.InvoiceID == Convert.ToString(InvoiceId))
                    {
                        strDocType = str.DocType;
                    }
                }


                if (strDocType == "CRN")
                {
                    //26-06-20156
                    Session["IndexforCRN"] = ViewState["Counter"];
                    Session.Add("invcren", InvoiceId);
                    //Response.Redirect("../CreditNotes/ActionCreditTiffViewer.aspx");// Commeneted By Rimi on 22nd July 2015
                    string DDCompanyID = System.Convert.ToString(Request.QueryString["DDCompanyID"]);
                    // Added By Rimi on 22nd July 2015
                    if (ViewState["CancelFlag"] == "1")
                    {
                        Response.Redirect("../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + InvoiceId + "&DDCompanyID=" + DDCompanyID);
                        ViewState["CancelFlag"] = "2";
                    }
                    else
                    {
                        Response.Write("<script>opener.location.reload();</script>");
                        Response.Write("<script>self.close();</script>");
                        //string url = "../CreditNotes/ActionCreditTiffViewer.aspx?MsgFlag=1&MSG=" + ViewState["MSG"].ToString() + "&InvoiceID=" + InvoiceId + "&DDCompanyID=" + ViewState["DDCompanyID"];
                        //Response.Redirect("../CreditNotes/ActionCreditTiffViewer.aspx?MsgFlag=0&MSG=" + ViewState["MSG"].ToString() + "&InvoiceID=" + InvoiceId + "&DDCompanyID=" + ViewState["DDCompanyID"]);// Added By Rimi on 10th July 2015
                        Response.Redirect("../CreditNotes/ActionCreditTiffViewer.aspx?MsgFlag=0&MSG=Open&InvoiceID=" + InvoiceId + "&DDCompanyID=" + DDCompanyID);
                    }
                    // Added By Rimi on 22nd July 2015

                }
                else
                {
                    //========Added By Rimi on 8th Sept For Department - Approver Dropdowns issue=======

                    DataSet dsDept = new DataSet();
                    CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval();
                    string Fields = "Invoice.DepartmentID,Department.Department";
                    string Table = "dbo.Invoice INNER JOIN dbo.Department ON Department.DepartmentID=dbo.Invoice.DepartmentID";
                    string Criteria = "Invoice.InvoiceID = " + System.Convert.ToInt32(InvoiceId);
                    try
                    {
                        dsDept = objApproval.GetGlobalDropDowns(Fields, Table, Criteria);
                        //  Session["InvoiceBuyerCompany"] = dsDept.Tables[0].Rows[0][0].ToString();
                        ddldept.DataSource = dsDept;
                        ddldept.DataBind();
                        //  ddldept.Items.Insert(0, "Select");
                        dsDept = null;
                    }
                    catch (Exception ex)
                    {
                    }

                    //========Added By Rimi on 8th Sept For Department - Approver Dropdowns issue End=======
                    GetDocumentDetails(InvoiceId);
                    ButtonVisibility();
                    // GetAllComboCodesAddNew();

                    string TiffUrl = "../../TiffViewerDefault.aspx?ID=" + InvoiceId.ToString() + "&Type=" + "INV";
                    TiffWindow.Attributes.Add("src", TiffUrl);


                    ShowFiles(InvoiceId);

                    DataSet ds = GetDocumentDetails(InvoiceId, "INV");
                    Boolean Duplicate = Convert.ToBoolean(ds.Tables[0].Rows[0]["Duplicate"]);
                    if (Duplicate == false)
                    {
                        lblDuplicate.Visible = false;
                    }
                    else
                    {
                        lblDuplicate.Visible = true;
                    }


                    if (ds.Tables[0].Rows[0]["Department"].ToString().Length > 0)
                    {


                        ddldept.SelectedItem.Text = ds.Tables[0].Rows[0]["Department"].ToString();


                        GetApproverDropDownsAgainstDepartment(Convert.ToInt32(getDepartmentAgainstInvoiceID(InvoiceId)));
                    }
                    else
                    {
                        getDepartmentAgainstInvoiceID(InvoiceId);
                        GetApproverDropDownsAgainstDepartment(0);
                        lblDepartment.Text = "";
                    }

                    ViewState["InvoiceChecking"] = InvoiceId;
                    //CheckInvoiceExist();
                    CheckNextInvoiceExist(InvoiceId);
                    CheckInvoiceExist();
                    CalculateTotal();// Added by Rimi on 25/06.2015
                    GetVatAmount();// Added by Rimi on 25/06.2015
                    // GetNextVATAmount(InvoiceId);// Added by Rimi on 26.06.2015

                    IsAutorisedtoEditDataNextInvoice(InvoiceId.ToString());

                    string strStatusLogLink = GetInvoiceStatusLogNextInvoice(InvoiceId.ToString());
                    strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:550,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                    //Blocked By Kd 06.12.2018
                    //aInvoiceStatusLog.Attributes.Add("onclick", strStatusLogLink);


                    txtComment.Text = "";
                    tbRejection.Text = "";

                    PopulateRejectionCode();
                    PopulateDropDowns();
                    //Added by kuntal karar on 19thOctober2016- Sessions created here
                    Session["NextInvoiceID"] = Convert.ToString(InvoiceId);
                    Session["NextBuyerCompanyID"] = GetInvoiceBuyerCompanyID1(Convert.ToInt32(InvoiceId));
                    //added by kuntalkarar on 12thJanuary2017
                    Session["InvoiceId_GoToStockQC"] = Convert.ToString(InvoiceId);
                    //----------------------------------------
                }
                //===============Added By Subhrajyoti on 3rd August 2015===========================
                DataSet dss = GetDocumentDetails(Convert.ToInt32(InvoiceId), "INV");
                Int32 StatusId = Convert.ToInt32(dss.Tables[0].Rows[0]["StatusId"]);

                if (Convert.ToInt32(StatusId) != 20 && Convert.ToInt32(StatusId) != 21 && Convert.ToInt32(StatusId) != 22 && Convert.ToInt32(StatusId) != 6)
                {
                    Response.Redirect("../Current/closewindows.aspx");
                }

                //===============Added By Subhrajyoti on 3rd August 2015===========================

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                Response.Write("<script> parent.window.close();</script>");
            }

        }

        private void MoveToNext(Int32 InvoiceId)
        {

            try
            {

                ViewState["Counter"] = 1;
                // ViewState["CheckList"] = 1;




                GetDocumentDetails(InvoiceId);
                ButtonVisibility();
                // GetAllComboCodesAddNew();

                string TiffUrl = "../../TiffViewerDefault.aspx?ID=" + InvoiceId.ToString() + "&Type=" + "INV";
                TiffWindow.Attributes.Add("src", TiffUrl);


                ShowFiles(InvoiceId);

                DataSet ds = GetDocumentDetails(InvoiceId, "INV");
                Boolean Duplicate = Convert.ToBoolean(ds.Tables[0].Rows[0]["Duplicate"]);
                if (Duplicate == false)
                {
                    lblDuplicate.Visible = false;
                }
                else
                {
                    lblDuplicate.Visible = true;
                }


                if (ds.Tables[0].Rows[0]["Department"].ToString().Length > 0)
                {

                    ddldept.Items.Clear();
                    ddldept.Items.Insert(0, ds.Tables[0].Rows[0]["Department"].ToString());
                    // ddldept.SelectedItem.Text = ds.Tables[0].Rows[0]["Department"].ToString();


                    // GetApproverDropDownsAgainstDepartment(Convert.ToInt32(getDepartmentAgainstInvoiceID(InvoiceId)));
                }
                else
                {
                    getDepartmentAgainstInvoiceID(InvoiceId);
                    // GetApproverDropDownsAgainstDepartment(0);
                    lblDepartment.Text = "";
                }

                ViewState["InvoiceChecking"] = InvoiceId;
                //CheckInvoiceExist();
                CheckNextInvoiceExist(InvoiceId);
                CheckInvoiceExist();
                CalculateTotal();// Added by Rimi on 25/06.2015
                GetVatAmount();// Added by Rimi on 25/06.2015
                //GetNextVATAmount(InvoiceId);// commented by Rimi on 26.06.2015

                IsAutorisedtoEditDataNextInvoice(InvoiceId.ToString());

                string strStatusLogLink = GetInvoiceStatusLogNextInvoice(InvoiceId.ToString());
                strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:550,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                //Blocked By Kd 06.12.2018
                //aInvoiceStatusLog.Attributes.Add("onclick", strStatusLogLink);


                txtComment.Text = "";
                tbRejection.Text = "";

                PopulateRejectionCode();
                PopulateDropDowns();

            }



            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                Response.Write("<script> parent.window.close();</script>");
            }





        }

        protected string IFrameWindow(string oInvoiceID, string oDocType, string oVat, string oTatal, string NewVendorClass, string RowID)
        {
            bool IsIFrameNeeded = false;
            int IsPermit = 0;
            JKS.Invoice objinvoice = new JKS.Invoice();
            string DocType = Convert.ToString(oDocType);
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strVat = Convert.ToString(oVat);
            string strTaotal = Convert.ToString(oTatal);
            string strURL = "";
            string strNewVendorClass = Convert.ToString(NewVendorClass);
            string strRowID = Convert.ToString(RowID);
            //  string strTiffViewerurl = GetTiffViewersURL(strInvoiceID, DocType);

            if (DocType == "INV")
            {
                string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

                strRelationType = RelationType.Trim();
                IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
                if (IsPermit == 0)
                {
                    IsIFrameNeeded = true;
                    //strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"&RelationType="+strRelationType+"&iVat="+strVat+"&iGross="+strTaotal+"','abb','width=1150,height=750,top=100,left=100,scrollbars=1,resizable=1');";//rinku 02-02-2011
                    strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + Session["CompanyID"] + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','InvoiceAction','width=540,height=450,top=100,left=805,scrollbars=0,resizable=1');";//rimi
                    // Added by Mrinal on 22nd September 2014
                    strURL = strURL;//+ strTiffViewerurl;
                }
                else if (IsPermit == 1)
                {
                    strURL = "javascript:alert('This invoice has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
                }
                else if (IsPermit == 2)
                {
                    strURL = "javascript:alert('You cannot action a rejected invoice');";
                }
            }
            if (DocType == "CRN")
            {
                IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
                if (IsPermit == 0)
                {
                    IsIFrameNeeded = true;
                    strURL = "javascript:window.open('../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + Session["CompanyID"] + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','CreditNoteAction','width=550,height=450,top=100,left=805,scrollbars=1,resizable=1');";
                    // Added by Mrinal on 22nd September 2014
                    strURL = strURL;// + strTiffViewerurl;
                }
                else
                    strURL = "javascript:alert('This credit note has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
            }
            //if (IsIFrameNeeded)
            //{
            //    if (DocType == "CRN")
            //    {

            //        //strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','fullscreen,scrollbars');";
            //        //strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=' + screen.height + ',width=' + screen.width +',scrollbars=no,top=0,left=0,resizable=0');";//rimi commented by
            //        strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + Session["CompanyID"] + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=790px,width=1360px,scrollbars=no,top=0,left=0,resizable=0');";//rimi

            //    }
            //    else if (DocType == "INV")
            //    {
            //        // strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','fullscreen,scrollbars');";
            //        //strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=' + screen.height + ',width=' + screen.width +',scrollbars=no,top=0,left=0,resizable=0');";//rimi commented by
            //        strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + Session["CompanyID"] + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=790px,width=1360px,scrollbars=no,top=0,left=0,resizable=0');";//rimi
            //    }
            //}
            return (strURL);
        }

        protected string GetTiffViewersURL(object oID, object oDocType)
        {
            string strInvoiceID = Convert.ToString(oID);
            string strDocumentType = Convert.ToString(oDocType);
            int RowID = 0;
            if (Session["dtTiffViewer"] != null)
            {
                DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
                if (dtTiffViewer.Rows.Count > 0)
                {
                    DataView dvTiffViewer = new DataView(dtTiffViewer);

                    dvTiffViewer.Sort = "RowID ASC";
                    dvTiffViewer.RowFilter = "InvoiceID=" + Convert.ToInt32(strInvoiceID);
                    RowID = Convert.ToInt32(dvTiffViewer[0]["RowID"].ToString());
                }

            }


            string strURL = "";

            strURL = "javascript:window.open('../../TiffViewerDefault.aspx?ID=" + strInvoiceID + "&Type=" + strDocumentType + "','TiffViewer','width=650,height=450,top=100,left=150,scrollbars=1,resizable=1');";

            return (strURL);
        }

        #region getDepartmentAgainstInvoiceID
        private int getDepartmentAgainstInvoiceID(int invID)
        {



            Int32 InvoiceId = 0;
            string sSql = "select isnull(departmentid,0) as departmentid from invoice  WHERE InvoiceID =" + invID;
            DataSet ds = new DataSet();
            SqlDataAdapter sqlDA = new SqlDataAdapter(sSql, ConsString);
            SqlConnection sqlConn = new SqlConnection(ConsString);

            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    InvoiceId = Convert.ToInt32(ds.Tables[0].Rows[0]["departmentid"]);

                    if (InvoiceId == 0)
                    {

                        ddldept.Items.Clear();
                        ddldept.Items.Insert(0, "select");
                    }
                }

            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return InvoiceId;
        }

        #endregion


        //Start ActionTiff redirect

        //protected string GetTiffViewersURL(object oID, object oDocType)
        //{
        //    string strInvoiceID = Convert.ToString(oID);
        //    string strDocumentType = Convert.ToString(oDocType);
        //    int RowID = 0;
        //    if (Session["dtTiffViewer"] != null)
        //    {
        //        DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
        //        if (dtTiffViewer.Rows.Count > 0)
        //        {
        //            DataView dvTiffViewer = new DataView(dtTiffViewer);

        //            dvTiffViewer.Sort = "RowID ASC";
        //            dvTiffViewer.RowFilter = "InvoiceID=" + Convert.ToInt32(strInvoiceID);
        //            RowID = Convert.ToInt32(dvTiffViewer[0]["RowID"].ToString());
        //        }

        //    }


        //    string strURL = "";

        //    strURL = "javascript:window.open('../../TiffViewerDefault.aspx?ID=" + strInvoiceID + "&Type=" + strDocumentType + "','TiffViewer','width=650,height=450,top=100,left=150,scrollbars=1,resizable=1');";

        //    return (strURL);
        //}
        //#region GetURLTest
        //protected string GetURLTest(object oInvoiceID, object oDocType, object oVat, object oTatal, object NewVendorClass, object RowID)
        //{
        //    int IsPermit = 0;
        //    Communicorp.Invoice objinvoice = new Communicorp.Invoice();
        //    string DocType = Convert.ToString(oDocType);
        //    string strInvoiceID = Convert.ToString(oInvoiceID);
        //    string strVat = Convert.ToString(oVat);
        //    string strTaotal = Convert.ToString(oTatal);
        //    string strURL = "";
        //    string strNewVendorClass = Convert.ToString(NewVendorClass);
        //    // Added by Mrinal on 22nd September 2014
        //    string strRowID = Convert.ToString(RowID);
        //    string strTiffViewerurl = GetTiffViewersURL(strInvoiceID, DocType);

        //    if (DocType == "INV")
        //    {
        //        string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

        //        strRelationType = RelationType.Trim();
        //        IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
        //        if (IsPermit == 0)
        //        {
        //            //strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"&RelationType="+strRelationType+"&iVat="+strVat+"&iGross="+strTaotal+"','abb','width=1150,height=750,top=100,left=100,scrollbars=1,resizable=1');";//rinku 02-02-2011
        //            strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','InvoiceAction','width=540,height=450,top=100,left=805,scrollbars=1,resizable=1');";
        //            // Added by Mrinal on 22nd September 2014
        //            strURL = strURL + strTiffViewerurl;
        //        }
        //        else if (IsPermit == 1)
        //        {
        //            strURL = "javascript:alert('This invoice has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
        //        }
        //        else if (IsPermit == 2)
        //        {
        //            strURL = "javascript:alert('You cannot action a rejected invoice');";
        //        }
        //    }
        //    if (DocType == "CRN")
        //    {
        //        IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
        //        if (IsPermit == 0)
        //        {
        //            //					if(strDocStatus=="Rejected")
        //            //						strURL = "javascript:window.open('../CreditNotes/ActionForRejectedCreditNote.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"','abb','width=1080,height=750,top=100,left=100,scrollbars=1,resizable=1');";					
        //            //					else

        //            //strURL = "javascript:window.open('../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"','abb','width=1150,height=750,top=100,left=100,scrollbars=1,resizable=1');";	
        //            //'width=1240,height=750,top=100,left=750,scrollbars=1,resizable=1'
        //            strURL = "javascript:window.open('../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','CreditNoteAction','width=550,height=450,top=100,left=805,scrollbars=1,resizable=1');";
        //            // Added by Mrinal on 22nd September 2014
        //            strURL = strURL + strTiffViewerurl;
        //        }
        //        else
        //            strURL = "javascript:alert('This credit note has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
        //    }
        //    return (strURL);
        //}
        //#endregion
        //protected string IFrameWindow(object oInvoiceID, object oDocType, object oVat, object oTatal, object NewVendorClass, object RowID)
        //{
        //    bool IsIFrameNeeded = false;
        //    int IsPermit = 0;
        //    Communicorp.Invoice objinvoice = new Communicorp.Invoice();
        //    string DocType = Convert.ToString(oDocType);
        //    string strInvoiceID = Convert.ToString(oInvoiceID);
        //    string strVat = Convert.ToString(oVat);
        //    string strTaotal = Convert.ToString(oTatal);
        //    string strURL = "";
        //    string strNewVendorClass = Convert.ToString(NewVendorClass);
        //    string strRowID = Convert.ToString(RowID);
        //    string strTiffViewerurl = GetTiffViewersURL(strInvoiceID, DocType);

        //    if (DocType == "INV")
        //    {
        //        string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

        //        strRelationType = RelationType.Trim();
        //        IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
        //        if (IsPermit == 0)
        //        {
        //            IsIFrameNeeded = true;
        //            //strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() +"&RelationType="+strRelationType+"&iVat="+strVat+"&iGross="+strTaotal+"','abb','width=1150,height=750,top=100,left=100,scrollbars=1,resizable=1');";//rinku 02-02-2011
        //            strURL = "javascript:window.open('../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','InvoiceAction','width=540,height=450,top=100,left=805,scrollbars=1,resizable=1');";
        //            // Added by Mrinal on 22nd September 2014
        //            strURL = strURL + strTiffViewerurl;
        //        }
        //        else if (IsPermit == 1)
        //        {
        //            strURL = "javascript:alert('This invoice has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
        //        }
        //        else if (IsPermit == 2)
        //        {
        //            strURL = "javascript:alert('You cannot action a rejected invoice');";
        //        }
        //    }
        //    if (DocType == "CRN")
        //    {
        //        IsPermit = objinvoice.PermitToTakeActionDalkia(Convert.ToInt32(strInvoiceID), Convert.ToInt32(Session["UserID"]), DocType);
        //        if (IsPermit == 0)
        //        {
        //            IsIFrameNeeded = true;
        //            strURL = "javascript:window.open('../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + strInvoiceID + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','CreditNoteAction','width=550,height=450,top=100,left=805,scrollbars=1,resizable=1');";
        //            // Added by Mrinal on 22nd September 2014
        //            strURL = strURL + strTiffViewerurl;
        //        }
        //        else
        //            strURL = "javascript:alert('This credit note has already been actioned. Please press the refresh button on your Internet browser to remove it from your Current folder.');";
        //    }
        //    if (IsIFrameNeeded)
        //    {
        //        if (DocType == "CRN")
        //        {

        //            //strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','fullscreen,scrollbars');";
        //            strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=' + screen.height + ',width=' + screen.width +',scrollbars,top=0,left=0,resizable=0');";

        //        }
        //        else if (DocType == "INV")
        //        {
        //            // strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','fullscreen,scrollbars');";
        //            strURL = "javascript:window.open('CombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&DDCompanyID= " + ddlCompany.SelectedValue.Trim() + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + strVat + "&iGross=" + strTaotal + "&RowID=" + strRowID + "','IFRAMEWINDOW','height=' + screen.height + ',width=' + screen.width +',scrollbars,top=0,left=0,resizable=0');";
        //        }
        //    }
        //    return (strURL);
        //}








        //End ActiobnTiff redirect


        #region SetDropDownValuesOnOpenButtonClick
        private int SetDropDownValuesOnOpenButtonClick(int iUserID)
        {
            string NewApprover1 = "";
            string NewApprover2 = "";
            string NewApprover3 = "";
            string NewApprover4 = "";
            string NewApprover5 = "";

            if (Convert.ToString(ddlApprover1.SelectedItem.Text) != "Select")
                NewApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);
            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
                NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
                NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
                NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);



            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnOpenButtonClick", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {
                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            }
            else
            {
                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["InvoiceChecking"]));

            }
            sqlCmd.Parameters.Add("@UserID", iUserID);
            if (txtComment.Text == "")
                sqlCmd.Parameters.Add("@Comment", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());


            sqlCmd.Parameters.Add("@Description", DBNull.Value);

            if (NewApprover1 == "")
                sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover1", NewApprover1);

            if (NewApprover2 == "")
                sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);

            if (NewApprover3 == "")
                sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover3", NewApprover3);

            if (NewApprover4 == "")
                sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover4", NewApprover4);

            if (NewApprover5 == "")
                sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover5", NewApprover5);


            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iReturnValue = -1; }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region SetDropDownValuesOnPressingReopen
        private int SetDropDownValuesOnPressingReopen(int iUserID, int iChecked)
        {
            string NewApprover1 = "";
            string NewApprover2 = "";
            string NewApprover3 = "";
            string NewApprover4 = "";
            string NewApprover5 = "";

            if (Convert.ToString(ddlApprover1.SelectedItem.Text) != "Select")
                NewApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);
            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
                NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
                NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
                NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);



            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnPressingReopen", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {
                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            }
            else
            {
                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["InvoiceChecking"]));

            }

            sqlCmd.Parameters.Add("@UserID", iUserID);
            if (txtComment.Text == "")
                sqlCmd.Parameters.Add("@Comment", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());


            sqlCmd.Parameters.Add("@Description", DBNull.Value);


            if (NewApprover1 == "")
                sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover1", NewApprover1);

            if (NewApprover2 == "")
                sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);

            if (NewApprover3 == "")
                sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover3", NewApprover3);

            if (NewApprover4 == "")
                sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover4", NewApprover4);

            if (NewApprover5 == "")
                sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover5", NewApprover5);

            sqlCmd.Parameters.Add("@IsTicked", iChecked);

            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iReturnValue = -1; }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region lnkCrn_Click
        private void lnkCrn_Click(object sender, System.EventArgs e)
        {

        }
        #endregion

        #region GetApproverDropDownsAgainstDepartment(int iDeptID)
        public void GetApproverDropDownsAgainstDepartment(int iDeptID)
        {

            CBSolutions.ETH.Web.ETC.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.ETC.ApprovalPath.Approval();
            DataSet dsDDL = new DataSet();
            DataSet dsDDL1 = new DataSet();
            if (TypeUser == 2 || TypeUser == 3)
            {
                dsDDL = objApproval.GetApproverDropDownsByDepartment(System.Convert.ToInt32(Session["InvoiceBuyerCompany"]), iDeptID);
                dsDDL1 = objApproval.Get2ndApproverDropDownAkkeronETC1(System.Convert.ToInt32(Session["InvoiceBuyerCompany"]), iDeptID);
                if (dsDDL.Tables[0].Rows.Count > 0)
                {
                    ddlApprover1.ClearSelection();
                    ddlApprover1.DataSource = dsDDL;
                    ddlApprover1.DataBind();
                    ddlApprover1.Items.Insert(0, "Select");
                }
                else
                {
                    ddlApprover1.Items.Clear();
                    ddlApprover4.Items.Clear();
                    ddlApprover5.Items.Clear();
                    ddlApprover1.Items.Insert(0, "Select");
                    ddlApprover4.Items.Insert(0, "Select");
                    ddlApprover5.Items.Insert(0, "Select");
                }
                if (dsDDL.Tables[0].Rows.Count > 0)
                {
                    ddlApprover2.ClearSelection();
                    ddlApprover2.DataSource = dsDDL;
                    ddlApprover2.DataBind();
                    ddlApprover2.Items.Insert(0, "Select");
                }
                else
                {
                    ddlApprover2.Items.Clear();
                    ddlApprover2.Items.Insert(0, "Select");
                }
                if (dsDDL1.Tables[0].Rows.Count > 0)
                {
                    ddlApprover3.ClearSelection();
                    ddlApprover3.DataSource = dsDDL1;
                    ddlApprover3.DataBind();
                    ddlApprover3.Items.Insert(0, "Select");
                    for (int i = 0; i < dsDDL1.Tables[0].Rows.Count; i++)
                    {
                        G2App += dsDDL1.Tables[0].Rows[i]["GroupName"].ToString() + ",";
                    }
                    if (G2App.EndsWith(","))
                    {
                        G2App = G2App.Substring(0, G2App.Length - 1);
                    }
                    lblG2App.Text = G2App.ToString();
                    ViewState["lblG2App"] = G2App.ToString();
                }
                else
                {
                    ddlApprover3.Items.Clear();
                    ddlApprover3.Items.Insert(0, "Select");
                }

                // Added By Mrinal on 15th July 2013
                if (dsDDL1.Tables[0].Rows.Count > 0)
                {
                    ddlApprover4.ClearSelection();
                    ddlApprover4.DataSource = dsDDL1;
                    ddlApprover4.DataBind();
                    ddlApprover4.Items.Insert(0, "Select");

                    ddlApprover5.ClearSelection();
                    ddlApprover5.DataSource = dsDDL1;
                    ddlApprover5.DataBind();
                    ddlApprover5.Items.Insert(0, "Select");
                }
                else
                {
                    ddlApprover4.Items.Clear();
                    ddlApprover4.Items.Insert(0, "Select");
                    ddlApprover5.Items.Clear();
                    ddlApprover5.Items.Insert(0, "Select");
                }



            }
            else
            {

                dsDDL = objApproval.GetApproverDropDownsByDepartment(System.Convert.ToInt32(Session["InvoiceBuyerCompany"]), iDeptID);
                dsDDL1 = objApproval.Get2ndApproverDropDownAkkeronETC1(System.Convert.ToInt32(Session["InvoiceBuyerCompany"]), iDeptID);
                if (dsDDL.Tables[0].Rows.Count > 0)
                {
                    ddlApprover1.ClearSelection();
                    ddlApprover1.DataSource = dsDDL;
                    ddlApprover1.DataBind();
                    ddlApprover1.Items.Insert(0, "Select");
                }
                else
                {
                    ddlApprover1.Items.Clear();
                    ddlApprover3.Items.Clear();
                    ddlApprover4.Items.Clear();
                    ddlApprover5.Items.Clear();
                    ddlApprover1.Items.Insert(0, "Select");
                    ddlApprover3.Items.Insert(0, "Select");
                    ddlApprover4.Items.Insert(0, "Select");
                    ddlApprover5.Items.Insert(0, "Select");
                }
                if (dsDDL1.Tables[0].Rows.Count > 0)
                {
                    ddlApprover2.ClearSelection();
                    ddlApprover2.DataSource = dsDDL1;
                    ddlApprover2.DataBind();
                    ddlApprover2.Items.Insert(0, "Select");
                }
                else
                {
                    ddlApprover2.Items.Clear();
                    ddlApprover2.Items.Insert(0, "Select");
                }
                // Added By Mrinal on 15th July 2013
                if (dsDDL1.Tables[0].Rows.Count > 0)
                {
                    ddlApprover4.ClearSelection();
                    ddlApprover4.DataSource = dsDDL1;
                    ddlApprover4.DataBind();
                    ddlApprover4.Items.Insert(0, "Select");

                    ddlApprover5.ClearSelection();
                    ddlApprover5.DataSource = dsDDL1;
                    ddlApprover5.DataBind();
                    ddlApprover5.Items.Insert(0, "Select");
                }
                else
                {
                    ddlApprover4.Items.Clear();
                    ddlApprover4.Items.Insert(0, "Select");
                    ddlApprover5.Items.Clear();
                    ddlApprover5.Items.Insert(0, "Select");
                }
            }
            dsDDL = null;
            dsDDL1 = null;
            string NewApprover = Convert.ToString(ViewState["approvalpath"]);
            try
            {
                String[] arrApprover = NewApprover.Split('/');
                if (arrApprover.Length > 0)
                {
                    if (arrApprover[0].ToString() != null || arrApprover[0].ToString() != "")
                    {
                        sdApprover1 = arrApprover[0].ToString();
                    }
                }
                else
                {
                    sdApprover1 = "";
                }

                if (arrApprover.Length > 1)
                {
                    if (arrApprover[1].ToString() != null || arrApprover[1].ToString() != "")
                    {
                        sdApprover2 = arrApprover[1].ToString();
                    }
                }
                else
                {
                    sdApprover2 = "";
                }

                if (arrApprover.Length > 2)
                {
                    if (arrApprover[2].ToString() != null || arrApprover[2].ToString() != "")
                    {
                        sdApprover3 = arrApprover[2].ToString();
                    }
                }

                if (arrApprover.Length > 3)
                {
                    if (arrApprover[3].ToString() != null || arrApprover[3].ToString() != "")
                    {
                        sdApprover4 = arrApprover[3].ToString();
                    }
                }
                else
                {

                    sdApprover4 = "";
                }

                if (arrApprover.Length > 4)
                {

                    if (arrApprover[4].ToString() != null || arrApprover[4].ToString() != "")
                    {
                        sdApprover5 = arrApprover[4].ToString();
                    }
                }
                else
                {
                    sdApprover5 = "";
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            try
            {
                if (sdApprover1 != "")
                {

                    ddlApprover1.SelectedValue = sdApprover1;
                    ViewState["sdApprover1"] = sdApprover1;
                }
                else
                {
                    ddlApprover1.ClearSelection();
                }
                if (sdApprover2 != "")
                {

                    ddlApprover2.SelectedValue = sdApprover2;
                    ViewState["sdApprover2"] = sdApprover2;
                }
                else
                {
                    ddlApprover2.ClearSelection();
                }
                if (sdApprover3 != "")
                {

                    ddlApprover3.SelectedValue = sdApprover3;
                    ViewState["sdApprover3"] = sdApprover3;
                }
                else
                {
                    ddlApprover3.ClearSelection();
                }
                if (sdApprover4 != "")
                {

                    ddlApprover4.SelectedValue = sdApprover4;
                    ViewState["sdApprover4"] = sdApprover4;
                }
                else
                {
                    ddlApprover4.ClearSelection();
                }
                if (sdApprover5 != "")
                {

                    ddlApprover5.SelectedValue = sdApprover5;
                    ViewState["sdApprover5"] = sdApprover5;
                }
                else
                {
                    ddlApprover5.ClearSelection();
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
        }

        #endregion


        #region SetDropDownValuesOnOpen
        private int SetDropDownValuesOnOpen(int iUserID)
        {
            string NewApprover1 = "";
            string NewApprover2 = "";
            string NewApprover3 = "";
            string NewApprover4 = "";
            string NewApprover5 = "";

            if (Convert.ToString(ddlApprover1.SelectedItem.Text) != "Select")
                NewApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);
            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
                NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
                NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
                NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);


            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;

            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnOpen", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {

                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            }

            else
            {
                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["InvoiceChecking"]));


            }


            sqlCmd.Parameters.Add("@UserID", iUserID);
            if (txtComment.Text == "")
                sqlCmd.Parameters.Add("@Comment", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());


            sqlCmd.Parameters.Add("@Description", DBNull.Value);

            if (NewApprover1 == "")
                sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover1", NewApprover1);

            if (NewApprover2 == "")
                sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);

            if (NewApprover3 == "")
                sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover3", NewApprover3);

            if (NewApprover4 == "")
                sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover4", NewApprover4);

            if (NewApprover5 == "")
                sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@NewApprover5", NewApprover5);


            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iReturnValue = -1; }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region RetValAppronalPathChange
        private int RetValAppronalPathChange()
        {
            int iRetVal = 0;
            string NewApprover1 = "";
            string NewApprover2 = "";
            string NewApprover3 = "";
            string NewApprover4 = "";
            string NewApprover5 = "";

            if (Convert.ToString(ddlApprover1.SelectedItem.Text) != "Select")
                NewApprover1 = Convert.ToString(ddlApprover1.SelectedItem.Text);
            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
                NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
                NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
                NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);

            if (NewApprover1 == Convert.ToString(ViewState["sdApprover1"]) && NewApprover2 == Convert.ToString(ViewState["sdApprover2"]) && NewApprover3 == Convert.ToString(ViewState["sdApprover3"]))
            {
                iRetVal = 1;
            }
            else
            {
                SqlConnection sqlConn = new SqlConnection(ConsString);
                SqlCommand sqlCmd = null;
                SqlParameter sqlReturnParam = null;

                sqlCmd = new SqlCommand("sp_SetDropDownValuesOnREJECTION", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;


                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {

                    sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
                }
                else
                {

                    sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["InvoiceChecking"]));

                }



                sqlCmd.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
                if (txtComment.Text == "")
                    sqlCmd.Parameters.Add("@Comment", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());


                sqlCmd.Parameters.Add("@Description", DBNull.Value);


                if (NewApprover1 == "")
                    sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover1", NewApprover1);

                if (NewApprover2 == "")
                    sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);

                if (NewApprover3 == "")
                    sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover3", NewApprover3);

                if (NewApprover4 == "")
                    sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover4", NewApprover4);

                if (NewApprover5 == "")
                    sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
                else
                    sqlCmd.Parameters.Add("@NewApprover5", NewApprover5);


                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                try
                {
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    iRetVal = Convert.ToInt32(sqlReturnParam.Value);
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); iRetVal = -1; }
                finally
                {
                    sqlReturnParam = null;
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
            }
            return iRetVal;
        }
        #endregion

        #region  public DataSet GetDocumentDetails(int InvoiceID,string Type)
        public DataSet GetDocumentDetails(int InvoiceID, string Type)
        {
            //Start: added by koushik das as on 18Dec2017
            Session["AttInvoice"] = InvoiceID;
            Session["eInvoiceID"] = InvoiceID;
            //End: added by koushik das as on 18Dec2017

            DataSet ds = new DataSet();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("GetDocumentDetails_AkkeronETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@Type", Type);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return ds;
        }
        #endregion

        #region GetNetAmt(int InvoiceID)
        private double GetNetAmt(int InvoiceID)
        {
            double NetAmt = 0;
            double CodingValue = 0;// Added by Rimi on 26.06.2015
            //string sSql = "select nettotal from invoice where invoiceid=" + InvoiceID;// commented by Rimi on 26.06.2015
            string sSql = "select NetValue,CodingValue from GenericCodingChange where InvoiceType='INV' and invoiceid=" + InvoiceID;// Added by Rimi on 26.06.2015
            SqlDataReader dr = null;
            SqlDataReader dr2 = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlConnection sqlConn2 = new SqlConnection(ConsString);
            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
            string sSql2 = "select nettotal from invoice where invoiceid=" + InvoiceID;
            SqlCommand sqlCmd2 = new SqlCommand(sSql2, sqlConn2);

            try
            {
                sqlConn.Open();
                dr = sqlCmd.ExecuteReader();
                sqlConn2.Open();
                dr2 = sqlCmd2.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0] != DBNull.Value)
                    {
                        //NetAmt = Convert.ToDouble(dr[0]);// commented by Rimi on 26.06.2015
                        NetAmt += Convert.ToDouble(dr[0]);// Added by Rimi on 26.06.2015
                    }

                    // Added by Rimi on 26.06.2015
                    //if (dr[1] != DBNull.Value)
                    //{
                    //    CodingValue = Convert.ToDouble(dr[1]);
                    //}
                    // Added by Rimi on 26.06.2015 End

                }
                while (dr2.Read())
                {
                    if (dr2[0] != DBNull.Value)
                    {
                        CodingValue = Convert.ToDouble(dr2[0]);
                    }
                }
                ViewState["CodingValue"] = CodingValue;// Added by Rimi on 26.06.2015
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr.Close();
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return NetAmt;
        }
        #endregion

        #region InsertCodingChangeValuesByDeleting
        public int InsertCodingChangeValuesByDeleting(string XMLText, int invoiceID)
        {
            int RetVal = 0;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);

            try
            {
                //sqlCmd = new SqlCommand("sp_InsertCodingChangeValuesByDeleting_Akkeron", sqlConn);
                //sqlCmd = new SqlCommand("sp_InsertCodingChangeValuesByDeleting_GRH", sqlConn); //commented By Rimi on 8thJuly2015
                sqlCmd = new SqlCommand("sp_InsertCodingChangeValuesByDeleting_GRH_sub", sqlConn);//Added By Rimi on 8thJuly2015
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@CodingChangeXml", XMLText);
                sqlCmd.Parameters.Add("@InvoiceID", invoiceID);
                sqlConn.Open();
                RetVal = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); RetVal = -1; }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (RetVal);
        }
        #endregion

        #region CheckCreditNoteAgainstInvoice
        public int CheckCreditNoteAgainstInvoice(int iInvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;

            try
            {
                sqlCmd = new SqlCommand("sp_CheckCreditNoteAgainstInvoice", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
                sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iReturnValue = -1; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve
        public int UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(int InvoiceID, string strCreditNoteNo)
        {
            int ReturnVal = 0;
            SqlParameter sqlReturnParam = null;
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            try
            {
                sqlConn = new SqlConnection(ConsString);
                sqlCmd = new SqlCommand("sp_UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
                sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;

                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                ReturnVal = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch { ReturnVal = -1; }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
                sqlReturnParam = null;
            }
            return (ReturnVal);
        }
        #endregion

        #region CheckIsFullCreditNote
        private int CheckIsFullCreditNote()
        {
            int iReturnValue = 0;
            int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
            SqlParameter sqlRetParam = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            try
            {

                sqlCmd = new SqlCommand("sp_IsFullCreditNoteGMG", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
                sqlRetParam = sqlCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                sqlRetParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlRetParam.Value);
            }
            catch { iReturnValue = -1; }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return iReturnValue;
        }
        #endregion

        #region CheckCreditNoteAgainstInvoice
        private string CheckCreditNoteAgainstInvoice()
        {
            string InvoiceNo = "";
            int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
            SqlParameter sqlOutputParam = null;
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            sqlConn = new SqlConnection(ConsString);
            try
            {
                sqlCmd = new SqlCommand("sp_GetCreditNoteByInvoiceID", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
                sqlOutputParam = sqlCmd.Parameters.Add("@CreditInvoiceNo", SqlDbType.VarChar, 20);
                sqlOutputParam.Direction = ParameterDirection.Output;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                InvoiceNo = sqlOutputParam.Value.ToString();
            }
            catch { }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return InvoiceNo;
        }
        #endregion

        #region UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo
        public int UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo(int InvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;
            try
            {
                sqlCmd = new SqlCommand("sp_UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
                sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch { iReturnValue = -1; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region GetUserCodeANDUserGroup(int iUserID)
        public DataSet GetUserCodeANDUserGroup(int iUserID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT New_UserCode,New_UserGroup FROM Users WHERE UserID =" + iUserID;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region GetCreditNoteIDAgainstInvoiceIDANDCompanyID
        public int GetCreditNoteIDAgainstInvoiceIDANDCompanyID()
        {
            string sSql = "";
            int IRetVal = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {
                sSql = "SELECT ISNULL(CreditNoteID,0) AS CreditNoteID FROM CreditNote C WHERE CreditInvoiceNo =(SELECT InvoiceNo From Invoice I Where InvoiceID =" + System.Convert.ToInt32(Session["eInvoiceID"]) + " AND C.BuyerCompanyID = I.BuyerCompanyID AND C.SupplierCompanyID = I.SupplierCompanyID)";

            }
            else
            {
                sSql = "SELECT ISNULL(CreditNoteID,0) AS CreditNoteID FROM CreditNote C WHERE CreditInvoiceNo =(SELECT InvoiceNo From Invoice I Where InvoiceID =" + System.Convert.ToInt32(ViewState["InvoiceChecking"]) + " AND C.BuyerCompanyID = I.BuyerCompanyID AND C.SupplierCompanyID = I.SupplierCompanyID)";

            }

            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                IRetVal = Convert.ToInt32(Dst.Tables[0].Rows[0]["CreditNoteID"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); IRetVal = -201; }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return IRetVal;
        }
        #endregion

        #region GetNominalCodeAgainstDepartmentANDCompany
        public DataSet GetNominalCodeAgainstDepartmentANDCompany(int iDepartmentCodeID, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in (SELECT NominalCodeID FROM CodingDescription WHERE isnull(APAdminOnly,0)<>1 and DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ") order by NominalCode";
            else
                sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in (SELECT NominalCodeID FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ") order by NominalCode";
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo
        public int GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo(int InvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;
            sqlConn = new SqlConnection(ConsString);
            try
            {
                sqlCmd = new SqlCommand("sp_GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
                sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch { iReturnValue = -1; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        #region GetCodingDescriptionAgainstDepartmentANDNominal
        public DataSet GetCodingDescriptionAgainstDepartmentANDNominal(int iDepartmentCodeID, int iNominal, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE isnull(APAdminOnly,0) <> 1 AND DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " and DepartmentCodeID in (select DepartmentID from userdeptrelation where UserID = " + Convert.ToInt32(Session["UserID"]) + ") ";
            else
                sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " ";

            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region ValidationForCodingDescription rinku 25-10-2011
        private int ValidationForCodingDescription(int iDepartmentCodeID, int iNominal, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            int iReturn = 0;
            string sSql = "";

            sSql = "SELECT isnull(count(CodingDescriptionID),0) FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " ";

            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables[0].Rows.Count > 0)
                {
                    iReturn = Convert.ToInt32(Dst.Tables[0].Rows[0][0]);
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return iReturn;
        }

        //Added by Mainak 2018-08-23
        private bool? ValidationForCodingDescriptionByInternalOrder(int iDepartmentCodeID, int iNominal, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            bool? iOrder = false;
            string sSql = "";


            //sSql = "SELECT isnull(count(CodingDescriptionID),0) FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " ";
            sSql = "SELECT InternalOrder FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " ";

            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables[0].Rows.Count > 0 && Dst.Tables[0].Rows[0][0].ToString() != null)
                {
                    iOrder = Convert.ToBoolean(Dst.Tables[0].Rows[0][0].ToString());
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return iOrder;
        }
        #endregion
        #region GetProjectCodeAgainstDepartmentANDNominal
        public DataSet GetProjectCodeAgainstDepartmentANDNominal(int iDepartmentCodeID, int iNominal, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT ISNULL(ProjectID,0) AS ProjectID,(select ProjectName from Project where ProjectID=CodingDescription.ProjectID) as ProjectName FROM CodingDescription WHERE isnull(APAdminOnly,0) <> 1 AND DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " and DepartmentCodeID in (select DepartmentID from userdeptrelation where UserID = " + Convert.ToInt32(Session["UserID"]) + ") and ProjectID <> 0 and ProjectID is not null";
            else
                sSql = "SELECT ISNULL(ProjectID,0) AS ProjectID,(select ProjectName from Project where ProjectID=CodingDescription.ProjectID) as ProjectName FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + " and ProjectID <> 0 and ProjectID is not null";

            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region GetDepartmentANDNominalAgainstCodingDescID(int iCodingID,int iCompID)
        public DataSet GetDepartmentANDNominalAgainstCodingDescID(int iCodingID, int iCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter("sp_GetDepartmentANDNominalAgainstCodingDescID_GMG", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CodingDescriptionID", iCodingID);
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompID);
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", TypeUser);

            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region UpdatePassedToApproverAgainstInvoiceID(int invoiceID)
        private int UpdatePassedToApproverAgainstInvoiceID(int invoiceID)
        {
            int iretval = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = new SqlCommand("usp_UpdatePassedToApproverAgainstInvoiceID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", invoiceID);
            try
            {
                sqlConn.Open();
                iretval = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iretval = -1; }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }
            return iretval;
        }
        #endregion

        #region UpdateDepartmentAgainstInvoiceID()
        private int UpdateDepartmentAgainstInvoiceID()
        {
            int DeptID = 0;
            string sSql = "";
            if (Convert.ToInt32(ddldept.SelectedIndex) > 0)
                DeptID = Convert.ToInt32(ddldept.SelectedValue);


            int iretval = 0;
            if (DeptID > 0)
            {
                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {
                    sSql = "UPDATE Invoice SET departmentid =" + DeptID + "  WHERE InvoiceID =" + Convert.ToInt32(Session["eInvoiceID"]);
                }
                else
                {
                    sSql = "UPDATE Invoice SET departmentid =" + DeptID + "  WHERE InvoiceID =" + Convert.ToInt32(ViewState["InvoiceChecking"]);

                }
                SqlConnection sqlConn = new SqlConnection(ConsString);
                SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                try
                {
                    sqlConn.Open();
                    iretval = sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); iretval = -1; }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }
            return iretval;
        }
        #endregion

        #region GetInvoiceBuyerCompanyID1
        public int GetInvoiceBuyerCompanyID1(int iInvoiceID)
        {
            int BuyerID = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT BuyerCompanyID,SupplierCompanyID FROM Invoice WHERE InvoiceID=" + iInvoiceID;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables.Count > 0)
                    BuyerID = Convert.ToInt32(Dst.Tables[0].Rows[0]["BuyerCompanyID"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return BuyerID;
        }
        #endregion

        #region bool GetInvoiceRejectionCodeID(int iInvoiceID)
        public bool GetInvoiceRejectionCodeID(int iInvoiceID)
        {
            int BuyerID = 0;
            bool iret = true;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT ISNULL(New_RejectionCodeID,0)AS New_RejectionCodeID FROM Invoice WHERE Invoiceid =" + iInvoiceID;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables.Count > 0)
                    BuyerID = Convert.ToInt32(Dst.Tables[0].Rows[0]["New_RejectionCodeID"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            iret = false;
            return iret;
        }
        #endregion

        #region GetCreditNoteIDByCreditNoteNo(string strCreditNoteNo)
        public int GetCreditNoteIDByCreditNoteNo(string strCreditNoteNo)
        {
            int CreditNoteID = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT TOP 1 CreditNoteID FROM CreditNote WHERE InvoiceNo like '%" + strCreditNoteNo.Trim() + "%' AND BuyerCompanyID =" + Convert.ToInt32(Session["BuyerCID"]);
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables.Count > 0)
                    CreditNoteID = Convert.ToInt32(Dst.Tables[0].Rows[0]["CreditNoteID"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); CreditNoteID = 0; }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return CreditNoteID;
        }
        #endregion

        #region ButtonApprovePress_Generic
        public int ButtonApprovePress_Generic(int InvoiceID, int iUserID, int iUserTypeID, string Comments, int iCreditNoteID, string strInvoiceDesc)
        {
            int iCount = 0;
            SqlParameter sqlOutputParam = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            //SqlCommand sqlCmd = new SqlCommand("stp_ButtonApprovePress_Generic_GMG",sqlConn);
            SqlCommand sqlCmd = new SqlCommand("stp_ButtonApprovePress_Generic_ETC", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@UserID", iUserID);
            sqlCmd.Parameters.Add("@UserTypeID", iUserTypeID);
            sqlCmd.Parameters.Add("@Comments", Comments);
            sqlCmd.Parameters.Add("@CreditNoteID", iCreditNoteID);
            sqlCmd.Parameters.Add("@strInvoiceDesc", strInvoiceDesc);
            sqlOutputParam = sqlCmd.Parameters.Add("@IRetVALUE", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iCount);
        }
        #endregion

        #region CheckPermissionToTakeAction
        public int CheckPermissionToTakeAction(int InvoiceID, int UserID)
        {
            int ReturnVal = 0;
            SqlParameter sqlReturnParam = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            sqlCmd = new SqlCommand("up_CheckPermissionToTakeAction", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            sqlCmd.Parameters.Add("@UserID", UserID);
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                ReturnVal = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
                sqlReturnParam = null;
            }
            return (ReturnVal);
        }
        #endregion

        #region GetDatasetAgainstSQL
        public DataSet GetDatasetAgainstSQL(string sSql)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst;
        }
        #endregion

        #region CheckCodingINVCRN
        public int CheckCodingINVCRN(int invoiceID, string DocType)
        {
            int IRetVal = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT Invoiceid AS Invoiceid FROM GenericCodingChange WHERE Invoiceid =" + invoiceID + " AND InvoiceType = '" + DocType + "'";
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                IRetVal = Convert.ToInt32(Dst.Tables[0].Rows[0]["Invoiceid"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return IRetVal;
        }
        #endregion

        #region GetMultipleCreditNotes()
        public string GetMultipleCreditNotes()
        {
            string strMultipleCreditNo = "";
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT INVOICENO FROM CREDITNOTE WHERE StatusID <> 7 AND CREDITINVOICENO = "
                + " (SELECT  INVOICENO FROM INVOICE WHERE INVOICEID=" + invoiceID + ") "
                + " AND buyerCompanyId=" + Convert.ToInt32(Session["BuyerCID"]) + " And SupplierCompanyid=" + iSupplierCompanyID;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                for (int k = 0; k < Dst.Tables[0].Rows.Count; k++)
                {
                    strMultipleCreditNo += Dst.Tables[0].Rows[k]["INVOICENO"].ToString();
                    strMultipleCreditNo += "$";
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); strMultipleCreditNo = ""; }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return strMultipleCreditNo;

        }
        #endregion

        #region GetCreditLinks
        public string GetCreditLinks()
        {
            string strRet = "";
            string strNewWin = "";
            if (lblcreditnoteno.Text != "")
            {
                string[] arrCrnNos = lblcreditnoteno.Text.Split('$');
                int CrnNoPopUps = 0;
                for (int i = 0; i < arrCrnNos.Length; i++)
                {
                    CrnNoPopUps = GetCreditNoteIDByCreditNoteNo(Convert.ToString(arrCrnNos[i]));
                    strNewWin = "window.open('../CreditNotes/ActionCredit.aspx?InvoiceID=" + CrnNoPopUps + "&DDCompanyID=" + Convert.ToInt32(Session["DropDownCompanyID"]) + "','crnpopups','width=940,height=380,scrollbars=1,resizable=1');";
                    if (arrCrnNos[i].ToString() != "")
                        //   strRet += "<a href='#' style='COLOR: red' onclick=" + strNewWin + ">" + arrCrnNos[i] + "</a><br>";
                        strRet = arrCrnNos[i].ToString();
                }
            }
            return strRet;
        }
        #endregion
        #region GetAllComboCodesFirstTime
        private void GetAllComboCodesFirstTime()
        {
            //int compid = 0;
            //DataTable dt = null;
            //string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlNominalCode1 = "";
            //for (int i = 0; i <= grdList.Items.Count - 1; i++)
            //{
            //    if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            //    {
            //        compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
            //    }
            //    ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
            //    ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
            //    ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

            //    if (compid != 0)
            //    {
            //        //					if(TypeUser==1)
            //        //						dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]),compid);
            //        //					else
            //        //						dt= objInvoice.GetGridDepartmentList(compid);
            //        DataSet ds = LoadDepartment(compid);
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = ds;
            //            ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
            //            ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
            //            ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
            //            ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
            //            SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);
            //        }


            //        int iddlDept = 0;
            //        int iNomin = 0;
            //        string CodingDescription = "--Select--";

            //        if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue) != "--Select--")
            //        {
            //            if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--")
            //            {
            //                iddlDept = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
            //                iNomin = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);
            //                DataSet dsCODES = new DataSet();
            //                dsCODES = GetCodingDescriptionAgainstDepartmentANDNominal(iddlDept, iNomin, compid);

            //                if (dsCODES.Tables.Count > 0 && dsCODES.Tables[0].Rows.Count > 0)
            //                {
            //                    CodingDescription = dsCODES.Tables[0].Rows[0]["CodingDescriptionID"].ToString();
            //                }
            //                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = CodingDescription;

            //            }
            //        }

            //        dt = objInvoice.GetGridNominalCodeList(compid);
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
            //        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
            //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);

            //        if (TypeUser == 1)
            //            dt = objInvoice.GetGridCodingDescriptionListByUserID(Convert.ToInt32(Session["UserID"]), compid);
            //        else
            //            dt = objInvoice.GetGridCodingDescriptionList(compid);

            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dt;
            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();

            //        ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
            //        SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ddlCodingDescription1);
            //    }
            //    else
            //        Response.Write("<script>alert('Please select a company');</script>");
            //}
        }
        #endregion

        #region GetInvoiceBuyerCompanyID
        public void GetInvoiceBuyerCompanyID(int iInvoiceID)
        {

            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT Invoice.BuyerCompanyID,Invoice.SupplierCompanyID,Company.CompanyName FROM Invoice,Company WHERE Invoice.BuyerCompanyID=Company.CompanyID and InvoiceID=" + iInvoiceID;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
                if (Dst.Tables.Count > 0)
                {
                    BuyerID = Convert.ToInt32(Dst.Tables[0].Rows[0]["BuyerCompanyID"].ToString());
                    strChildBuyer = Dst.Tables[0].Rows[0]["CompanyName"].ToString();
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

        }
        #endregion

        #region void getRelatedCreditNotID(string InvoiceID)
        protected void getRelatedCreditNotID(string InvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            string sQuery = "select C.CreditNoteID,I.BuyerCompanyID,I.SupplierCompanyID from CreditNote C,Invoice I where ";
            sQuery += " C.BuyerCompanyID=I.BuyerCompanyID And C.SupplierCompanyID=I.SupplierCompanyID";
            sQuery += " And C.CreditInvoiceNo=I.InvoiceNo And I.InvoiceID= " + InvoiceID + " order by CreditNoteID desc";

            sqlCmd = new SqlCommand(sQuery, sqlConn);

            SqlDataReader dr1 = null;
            try
            {
                sqlConn.Open();
                dr1 = sqlCmd.ExecuteReader();

                while (dr1.Read())
                {
                    CreditNoteID = Convert.ToString(dr1["CreditNoteID"]);
                    strBuyerCompanyID = Convert.ToString(dr1["BuyerCompanyID"]);
                    strSupplierCompanyID = Convert.ToString(dr1["SupplierCompanyID"]);
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                dr1.Close();
                sqlConn.Close();
            }


            if (CreditNoteID != "")
                sDisplay = "block";

        }
        #endregion

        #region ShowMultipleCredits(int InvoiceID)
        private string ShowMultipleCredits(int InvoiceID)
        {
            string strMultipleCreditNo = "";
            string sQuery = "select C.CreditInvoiceNo as CreditInvoiceNo,C.CreditNoteID,c.InvoiceNo as CreditNo from CreditNote C,Invoice I ";
            sQuery += " where  C.BuyerCompanyID=I.BuyerCompanyID And C.SupplierCompanyID=I.SupplierCompanyID ";
            sQuery += " And C.InvoiceNo=I.InvoiceNo And I.InvoiceID=" + InvoiceID;
            //sQuery += " And C.CreditInvoiceNo=I.InvoiceNo And I.InvoiceID=" + InvoiceID;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter(sQuery, sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                strMultipleCreditNo += ds.Tables[0].Rows[k]["CreditNo"].ToString();
                strMultipleCreditNo += "$";
            }



            return strMultipleCreditNo;
        }
        #endregion

        #region private void ShowFiles(int InvoiceID)
        private void ShowFiles(int InvoiceID)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetUploadFileDetails_NL", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            grdFile.DataSource = ds.Tables[0];
            grdFile.DataBind();
            if (grdFile.Items.Count > 0)
            {
                lblMsg.Visible = false;
            }
            else
            {
                lblMsg.Visible = true;
            }

        }
        #endregion

        #region private void grdFile_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //private void grdFile_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        if (DataBinder.Eval(e.Item.DataItem, "archiveImagePath") != DBNull.Value)
        //        {
        //            if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")) != "")
        //            {
        //                ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")).Trim());
        //            }
        //        }
        //        else
        //        {
        //            if (DataBinder.Eval(e.Item.DataItem, "ImagePath") != DBNull.Value)
        //            {
        //                if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")) != "")
        //                {
        //                    ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")).Trim());
        //                }
        //                else
        //                {
        //                    ((Label)e.Item.FindControl("lblPath")).Text = "N/A";
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        #region private void grdFile_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //private void grdFile_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //{
        //    bool bFound = false;
        //    int DocumentID = 0;
        //    lblMsg.Text = "";

        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        DocumentID = Convert.ToInt32(((Label)e.Item.FindControl("lblDocID")).Text);
        //        if (Convert.ToString(e.CommandArgument) == "DOW")
        //        {
        //            string sDownLoadPath = ((Label)e.Item.FindControl("lblHidePath")).Text;
        //            sDownLoadPath = sDownLoadPath.Replace("I:", "C:\\P2D");
        //            sDownLoadPath = sDownLoadPath.Replace("\\90104-server2", "C:\\P2D");
        //            sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
        //            if (sDownLoadPath.Trim() != "")
        //            {
        //                if (Path.GetExtension(sDownLoadPath).ToUpper() != ".tif")
        //                {
        //                    try
        //                    {
        //                        bFound = ForceDownload(sDownLoadPath, 0);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), ex.Message.ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    bFound = ForceDownload(sDownLoadPath, 0);
        //                }
        //            }
        //            if (bFound == false)
        //            {
        //                sDownLoadPath = ((Label)e.Item.FindControl("lblArchPath")).Text;
        //                sDownLoadPath = sDownLoadPath.Replace("\\90107-server3", @"C:\File Repository");
        //                sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);

        //                if (sDownLoadPath.Trim() != "")
        //                {
        //                    if (Path.GetExtension(sDownLoadPath).ToUpper() != ".tif")
        //                    {
        //                        try
        //                        {

        //                            bFound = ForceDownload(sDownLoadPath, 1);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), ex.Message.ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        bFound = ForceDownload(sDownLoadPath, 1);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        #region private string GetTrimFirstSlash(string sVal)
        private string GetTrimFirstSlash(string sVal)
        {
            string sFName = sVal;
            if (sVal != "" & sVal != null)
            {

                string sInfo = sVal;
                sInfo.Replace(@"\", @"\\");
                string[] delValue = sInfo.Split(new char[] { '\\' });
                sFName = "";
                for (int x = 0; x < delValue.Length; x++)
                {
                    if (delValue[x] != "")
                    {
                        sFName = sFName + delValue[x];
                        if (x != delValue.Length - 1)
                        {
                            sFName = sFName + @"\";
                        }
                    }
                }
            }
            return sFName;
        }
        #endregion

        #region private bool ForceDownload(string sPath,int iStep)
        //private bool ForceDownload(string sPath, int iStep)
        //{
        //    bool bRetVal = false;
        //    string sFileName = sPath;
        //    if (iStep == 0)
        //    {
        //        System.IO.FileStream fs1 = null;
        //        try
        //        {
        //            CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
        //            objService.Url = GetURL();
        //            byte[] bytBytes = objService.DownloadFile(sFileName);
        //            ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + sFileName.ToString());
        //            if (bytBytes != null)
        //            {
        //                //Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
        //                //Response.ContentType = "application//octet-stream";
        //                //Response.BinaryWrite(bytBytes);
        //                //Response.Flush();
        //                //Response.End();
        //                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
        //                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + Path.GetFileName(sFileName).ToString());
        //                HttpContext.Current.Response.ContentType = "application//octet-stream";
        //                HttpContext.Current.Response.BinaryWrite(bytBytes);
        //                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + bytBytesbytBytes.Length);
        //                HttpContext.Current.Response.Flush();
        //                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "Completed");
        //                // HttpContext.Current.Response.End();
        //                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //                HttpContext.Current.ApplicationInstance.CompleteRequest();
        //                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "End");

        //                //fs1.Close();
        //                //fs1 = null;
        //                bRetVal = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), ex.Message.ToString() + Convert.ToString(Session["eInvoiceID"]) + sPath.ToString());
        //        }
        //    }
        //    else if (iStep == 1)
        //    {
        //        System.IO.FileStream fs1 = null;
        //        try
        //        {
        //            CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
        //            objService2.Url = GetURL2();
        //            byte[] bytBytes = objService2.DownloadFile(sFileName);
        //            ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + sFileName.ToString());
        //            if (bytBytes != null)
        //            {
        //                //Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
        //                //Response.ContentType = "application//octet-stream";
        //                //Response.BinaryWrite(bytBytes);
        //                //Response.Flush();
        //                //Response.End();
        //                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
        //                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + Path.GetFileName(sFileName).ToString());
        //                HttpContext.Current.Response.ContentType = "application//octet-stream";
        //                HttpContext.Current.Response.BinaryWrite(bytBytes);
        //                // ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + bytBytesbytBytes.Length);
        //                //Response.Flush();
        //                HttpContext.Current.Response.Flush();
        //                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "Completed");
        //                //Response.End();
        //                // HttpContext.Current.Response.End();
        //                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //                HttpContext.Current.ApplicationInstance.CompleteRequest();
        //                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "End");
        //                //fs1.Close();
        //                //fs1 = null;
        //                bRetVal = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), ex.Message.ToString() + Convert.ToString(Session["eInvoiceID"]) + sPath.ToString());
        //        }
        //    }
        //    return bRetVal;
        //}
        #endregion

        private bool ForceDownload(string sPath, int iStep)
        {
            bool bRetVal = false;
            string sFileName = sPath;
            if (iStep == 0)
            {
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = GetURL();
                    byte[] bytBytes = objService.DownloadFile(sFileName);
                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + sFileName.ToString() + iStep.ToString());
                    if (bytBytes != null)
                    {
                        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + Path.GetFileName(sFileName).ToString());
                        Response.ContentType = "application//octet-stream";
                        Response.BinaryWrite(bytBytes);
                        Response.Flush();
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "Completed");
                        //HttpContext.Current.Response.End();
                        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "End");
                        //fs1.Close();
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "Close");
                        fs1 = null;
                        bRetVal = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), ex.Message.ToString() + Convert.ToString(Session["eInvoiceID"]) + sPath.ToString());
                }
            }
            else if (iStep == 1)
            {
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
                    objService2.Url = GetURL2();
                    byte[] bytBytes = objService2.DownloadFile(sFileName);
                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + sFileName.ToString() + iStep.ToString());
                    if (bytBytes != null)
                    {
                        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(Session["eInvoiceID"]) + Path.GetFileName(sFileName).ToString());
                        Response.ContentType = "application//octet-stream";
                        Response.BinaryWrite(bytBytes);
                        Response.Flush();
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "Completed");
                        //HttpContext.Current.Response.End();
                        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "End");
                        // fs1.Close();
                        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "Close");
                        fs1 = null;
                        bRetVal = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), ex.Message.ToString() + Convert.ToString(Session["eInvoiceID"]) + sPath.ToString());
                }
            }
            return bRetVal;
        }

        public void ErrorLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName, true);
            sw.WriteLine(DateTime.Now + ": " + sErrMsg);
            sw.Flush();
            sw.Close();
        }

        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }

        private string GetURL2()
        {

            return ConfigurationManager.AppSettings["ServiceURLNew"];
        }


        private string GetPONumberForSupplierBuyer(string Ponumber)
        {
            string existscheck = "";
            int invoiceID = 0;
            //int invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {
                invoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            }
            else
            {
                invoiceID = Convert.ToInt32(ViewState["CheckList"]);
            }


            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter("sp_PoNumberForSupplerAnainstBuyerAkkeron", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", invoiceID);
            sqlDA.SelectCommand.Parameters.Add("@PoNumber", Ponumber);
            sqlDA.SelectCommand.Parameters.Add("@Type", "INV");


            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            if (Dst.Tables.Count > 0)
            {
                if (Dst.Tables[0].Rows.Count > 0)
                {
                    existscheck = Convert.ToString(Dst.Tables[0].Rows[0]["Exists"]);
                }
            }
            return existscheck;

        }

        private string GetDepartmentName(string Departmentid)
        {
            string existscheck = "";

            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter("select DepartmentID , Department from  Department  where departmentid=" + Departmentid, sqlConn);
            // sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;



            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            if (Dst.Tables.Count > 0)
            {
                if (Dst.Tables[0].Rows.Count > 0)
                {
                    existscheck = Convert.ToString(Dst.Tables[0].Rows[0]["Department"]);
                }
            }
            return existscheck;

        }

        #region: Tiff Viewer Implementation

        protected void FetchNextRecord(out string strInvoiceID, out string strVat, out string strTatal, out string strNewVendorClass, out string strRowID)
        {
            int RowID = 0;
            if (Request.QueryString["RowID"] != null)
            {
                RowID = System.Convert.ToInt32(Request.QueryString["RowID"]);
            }
            strInvoiceID = "";
            strVat = "";
            strTatal = "";
            strNewVendorClass = "";
            strRowID = "";
            string strDocType = "INV";
            if (Session["dtTiffViewer"] != null)
            {
                DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
                if (dtTiffViewer.Rows.Count > 0)
                {
                    DataView dvTiffViewer = new DataView(dtTiffViewer);

                    dvTiffViewer.Sort = "RowID ASC";
                    dvTiffViewer.RowFilter = "RowID >" + Convert.ToInt32(RowID) + " AND InvStatusID in (21, 22) AND DocType='" + strDocType + "' ";
                    if (dvTiffViewer.Count > 0)
                    {
                        strRowID = Convert.ToString(dvTiffViewer[0]["RowID"].ToString());
                        strInvoiceID = Convert.ToString(dvTiffViewer[0]["InvoiceID"].ToString());
                        strVat = Convert.ToString(dvTiffViewer[0]["VAT"].ToString());
                        strTatal = Convert.ToString(dvTiffViewer[0]["Total"].ToString());
                        strNewVendorClass = Convert.ToString(dvTiffViewer[0]["New_VendorClass"].ToString());
                    }
                }

            }

        }

        //protected string GetTiffViewersURL(object InvoiceID, object oDocType)
        //{
        //    string strInvoiceID = Convert.ToString(InvoiceID);
        //    string strDocumentType = Convert.ToString(oDocType);
        //    /* int RowID = 0;
        //     if (Session["dtTiffViewer"] != null)
        //     {
        //         DataTable dtTiffViewer = (DataTable)Session["dtTiffViewer"];
        //         if (dtTiffViewer.Rows.Count > 0)
        //         {
        //             DataView dvTiffViewer = new DataView(dtTiffViewer);

        //             dvTiffViewer.Sort = "RowID ASC";
        //             dvTiffViewer.RowFilter = "InvoiceID=" + Convert.ToInt32(strInvoiceID);
        //             RowID = Convert.ToInt32(dvTiffViewer[0]["RowID"].ToString());
        //         }

        //     }*/
        //    string strURL = "";


        //    strURL = "javascript:window.open('../../TiffViewerDefault.aspx?ID=" + strInvoiceID + "&Type=" + strDocumentType + "','TiffViewer','width=650,height=450,top=100,left=150,scrollbars=1,resizable=1');";

        //    return (strURL);
        //}

        protected void GetURLTest()
        {
            int RowID = 0;
            if (Request.QueryString["RowID"] != null)
            {
                RowID = System.Convert.ToInt32(Request.QueryString["RowID"]);
                Session["RowID"] = Convert.ToString(RowID);
            }
            Session["IsProcessed"] = "1";

            Response.Write("<script> parent.window.close();</script>");


        }
        #endregion

        #region SaveLineData()
        //private bool SaveLineData()
        //{
        //    #region variables
        //    int InvID=0;
        //    //if (string.IsNullOrEmpty(ViewState["InvoiceChecking"] as string))
        //    //if(string.IsNullOrEmpty(ViewState["InvoiceChecking"].ToString())==false)
        //     if(Convert.ToInt32(ViewState["InvoiceChecking"])!=0)//kk
        //    {
        //        InvID = Convert.ToInt32(ViewState["InvoiceChecking"]);

        //    }
        //    else
        //    {
        //        InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
        //    }
        //    int CompanyID = 0;
        //    int CodingDescriptionID = 0;
        //    int NominalCodeID = 0;
        //    int BusinessUnitID = 0;
        //    int DepartmentID = 0;
        //    int iValidFlag = 0;
        //    decimal NetValue = 0;
        //    double NetVal = 0;
        //    string PurOrderNo = String.Empty;

        //    // Added by Mrinal on 13th March 2015
        //    decimal LineVAT = 0;
        //    string strLineDescription = string.Empty;
        //    // Addition End on 13th March 2015 
        //    bool retval = false;
        //    lblErrorMsg.Visible = false;


        //    int Line1CodingDescriptionID = 0;
        //    int Line1DepartmentID = 0;
        //    #endregion

        //    #region:XML Format
        //    DataSet dsXML = new DataSet();
        //    DataTable dtXML = new DataTable();
        //    dtXML.Columns.Add("SlNo");
        //    dtXML.Columns.Add("InvoiceID");
        //    dtXML.Columns.Add("InvoiceType");
        //    dtXML.Columns.Add("CompanyID");
        //    dtXML.Columns.Add("CodingDescriptionID");
        //    dtXML.Columns.Add("DepartmentID");
        //    dtXML.Columns.Add("NominalCodeID");
        //    dtXML.Columns.Add("BusinessUnitID");
        //    dtXML.Columns.Add("NetValue");
        //    dtXML.Columns.Add("CodingValue");
        //    dtXML.Columns.Add("PurOrderNo");//ss	
        //    // Added by Mrinal on 13th March 2015
        //    dtXML.Columns.Add("LineVAT");
        //    dtXML.Columns.Add("LineDescription");
        //    // Addition End on 13th March 2015 

        //    #endregion
        //    DataRow DR = null;
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<Root>");

        //    for (int i = 0; i <= grdList.Items.Count - 1; i++)
        //    {
        //        #region Getting DropDown Values
        //        if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString()) != "--Select--" && Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
        //        {
        //            CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
        //        }
        //        else
        //        {
        //            Response.Write("<script>alert('Please select Company');</script>");
        //            return false;
        //        }
        //        //if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString()) != "--Select--")
        //        //{


        //        //}




        //        if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
        //        {
        //            NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //            DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //            ViewState["vDepartmentID"] = DepartmentID;

        //            CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //            ViewState["vCodingDescriptionID"] = CodingDescriptionID;
        //            // Added by Mrinal on 16th March 2015
        //            if (i == 0)
        //            {
        //                Line1CodingDescriptionID = CodingDescriptionID;
        //                Line1DepartmentID = DepartmentID;
        //            }
        //            // Addition End
        //        }

        //        else
        //        {
        //            NominalCodeID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //            DepartmentID = 0;//Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //            CodingDescriptionID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //            // Added by Mrinal on 16th March 2015
        //            if (i == 0)
        //            {
        //                Line1CodingDescriptionID = 0;
        //                Line1DepartmentID = 0;
        //            }

        //        }





        //        if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
        //        {
        //            BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
        //        }


        //        PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);


        //        NetValue = 0;
        //        if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
        //        {
        //            //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
        //            //{
        //            NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
        //            //}
        //        }

        //        LineVAT = 0;

        //        if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
        //        {
        //            //blocked by kuntal karar-on-25.03.2015----pt.48-------
        //            //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
        //            //{
        //            LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
        //            // }
        //            //------------------------------------------------------
        //        }
        //        strLineDescription = string.Empty;

        //        if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
        //        {
        //            strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
        //        }
        //        #endregion
        //        //-----------modified by kuntal karar-on-25thMAR2015--------------
        //        //(NetValue > 0 ||

        //        if ((Convert.ToDecimal(Request.QueryString["iVat"]) >= 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))
        //        {
        //            //---------------------------------------------------------------
        //            DR = dtXML.NewRow();
        //            dtXML.Rows.Add(DR);
        //            sb.Append("<Rowss>");
        //            sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
        //            sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
        //            sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
        //            sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
        //            if (CodingDescriptionID > 0)
        //            {
        //                sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
        //            }
        //            else
        //            {
        //                sb.Append("<CodingDescriptionID>").Append(DBNull.Value).Append("</CodingDescriptionID>");
        //            }
        //            if (DepartmentID > 0)
        //            {
        //                sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
        //            }
        //            else
        //            {
        //                sb.Append("<DepartmentID>").Append(DBNull.Value).Append("</DepartmentID>");
        //            }

        //            if (NominalCodeID > 0)
        //            {
        //                sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
        //            }
        //            else
        //            {
        //                sb.Append("<NominalCodeID>").Append(DBNull.Value).Append("</NominalCodeID>");
        //            }
        //            //sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
        //            //sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
        //            int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
        //            if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
        //                sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
        //            else
        //                sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</BusinessUnitID>");

        //            sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
        //            sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
        //            sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");

        //            sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
        //            sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");
        //            sb.Append("</Rowss>");
        //        }
        //    }
        //    dsXML.Tables.Add(dtXML);
        //    sb.Append("</Root>");
        //    string strXmlText = sb.ToString();
        //    sb = null;


        //    int retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
        //    if (retvalalue > 0)
        //    {
        //        int isConditionSatisfied = CheckNominalRoutingAgainstInvoice(InvID, Line1CodingDescriptionID, Line1DepartmentID);
        //        if (isConditionSatisfied > 0)
        //        {
        //            #region: Refresh DropDown
        //            int InvDepartmentID = 0;
        //            string sSql = " SELECT ISNULL(DepartmentID, 0) As DepartmentID    FROM    dbo.Invoice     WHERE   InvoiceID=" + InvID;
        //            SqlDataReader dr = null;
        //            SqlConnection sqlConn = new SqlConnection(ConsString);
        //            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
        //            try
        //            {
        //                sqlConn.Open();
        //                dr = sqlCmd.ExecuteReader();
        //                while (dr.Read())
        //                {
        //                    if (dr[0] != DBNull.Value)
        //                    {
        //                        InvDepartmentID = Convert.ToInt32(dr[0]);

        //                    }
        //                }
        //            }
        //            catch (Exception ex) { string ss = ex.Message.ToString(); }
        //            finally
        //            {
        //                dr.Close();
        //                sqlCmd.Dispose();
        //                sqlConn.Close();
        //            }
        //            GetApproverDropDownsAgainstDepartment(InvDepartmentID);
        //            #endregion
        //        }
        //        retval = true;
        //    }
        //    else
        //        retval = false;


        //    return retval;
        //}
        //==========Added By Rimi on 29th July 2015============
        public Boolean GetCodingDetails(string codingDesc)
        {
            DataSet ds = new DataSet();
            bool result = false;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetCodingDescription_GRH", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.AddWithValue("@coding", codingDesc);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                result = true;
            }
            return result;
        }
        //==============Added By Rimi on 29th July 2015============


        //==================Commeneted on 21st August 2015 by Rimi==============================================
        //private bool SaveLineData()
        //{
        //    #region variables
        //    int InvID = 0;
        //    //if (string.IsNullOrEmpty(ViewState["InvoiceChecking"] as string))
        //    //if(string.IsNullOrEmpty(ViewState["InvoiceChecking"].ToString())==false)
        //    if (Convert.ToInt32(ViewState["InvoiceChecking"]) != 0)//kk
        //    {
        //        InvID = Convert.ToInt32(ViewState["InvoiceChecking"]);

        //    }
        //    else
        //    {
        //        InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
        //    }
        //    int CompanyID = 0;
        //    int CodingDescriptionID = 0;
        //    int NominalCodeID = 0;
        //    int BusinessUnitID = 0;
        //    int DepartmentID = 0;
        //    int iValidFlag = 0;
        //    decimal NetValue = 0;
        //    double NetVal = 0;
        //    string PurOrderNo = String.Empty;

        //    // Added by Mrinal on 13th March 2015
        //    decimal LineVAT = 0;
        //    string strLineDescription = string.Empty;
        //    // Addition End on 13th March 2015 
        //    bool retval = false;
        //    lblErrorMsg.Visible = false;


        //    int Line1CodingDescriptionID = 0;
        //    int Line1DepartmentID = 0;
        //    #endregion

        //    #region:XML Format
        //    DataSet dsXML = new DataSet();
        //    DataTable dtXML = new DataTable();
        //    dtXML.Columns.Add("SlNo");
        //    dtXML.Columns.Add("InvoiceID");
        //    dtXML.Columns.Add("InvoiceType");
        //    dtXML.Columns.Add("CompanyID");
        //    dtXML.Columns.Add("CodingDescriptionID");
        //    dtXML.Columns.Add("DepartmentID");
        //    dtXML.Columns.Add("NominalCodeID");
        //    dtXML.Columns.Add("BusinessUnitID");
        //    dtXML.Columns.Add("NetValue");
        //    dtXML.Columns.Add("CodingValue");
        //    dtXML.Columns.Add("PurOrderNo");//ss	
        //    // Added by Mrinal on 13th March 2015
        //    dtXML.Columns.Add("LineVAT");
        //    dtXML.Columns.Add("LineDescription");
        //    // Addition End on 13th March 2015 
        //    DataSet dtCoding = new DataSet();
        //    #endregion
        //    DataRow DR = null;
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<Root>");

        //    for (int i = 0; i <= grdList.Items.Count - 1; i++)
        //    {
        //        #region Getting DropDown Values
        //        if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString()) != "--Select--" && Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
        //        {
        //            CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
        //        }
        //        else
        //        {
        //            ViewState["ddlCompFlag"] = "1";
        //            Response.Write("<script>alert('Please select Company');</script>");
        //            return false;
        //        }
        //        //if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString()) != "--Select--")
        //        //{


        //        //}




        //        if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
        //        {

        //            string a = ((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim();
        //            //Boolean res = GetCodingDetails(a.Substring(0,8));
        //            int index = a.IndexOf("[");
        //            if (index > 0)
        //            {
        //                Boolean res = GetCodingDetails(a.Substring(0, index));
        //                if (res == false)
        //                {

        //                    Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
        //                    return false;

        //                }
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
        //                return false;
        //            }

        //            //for (int j = 0; j < dtCoding.Tables[0].Rows.Count; j++)
        //            //{
        //            //    if (!dtCoding.Tables[0].Rows[j]["CodingDescription"].ToString().Contains(a.ToString()))
        //            //    {
        //            //        Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
        //            //        return false;
        //            //    }
        //            //}
        //            //================= Commeneted By Rimi on 27th July 2015======================
        //            //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //            //if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
        //            //{
        //            //    DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //            //}
        //            //ViewState["vDepartmentID"] = DepartmentID;

        //            //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);

        //            //=======================Commeneted By Rimi on 27th July 2015 End=================================


        //            //=====================Modified By Rimi on 27th July 2015=============================
        //            if (i > 0)
        //            {
        //                if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim() != ((TextBox)grdList.Items[i - 1].FindControl("txtAutoCompleteCodingDescription")).Text.Trim())
        //                {
        //                    NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                    ViewState["hdnDepartmentCodeID"] = "2";
        //                }

        //                else
        //                {
        //                    NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
        //                    ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = NominalCodeID.ToString();
        //                    ViewState["hdnDepartmentCodeID"] = "1";
        //                }
        //            }
        //            else
        //            {
        //                NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                ViewState["hdnDepartmentCodeID"] = "2";
        //            }
        //            if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
        //            {
        //                if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
        //                {
        //                    DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //                }
        //            }
        //            else
        //            {
        //                if (i > 0)
        //                {
        //                    DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
        //                    ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
        //                }
        //            }
        //            ViewState["vDepartmentID"] = DepartmentID;
        //            if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
        //            {
        //                CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //            }
        //            else
        //            {
        //                if (i > 0)
        //                {
        //                    CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
        //                    ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();
        //                }

        //            }

        //            //====================Modified By Rimi on 27th July 2015 End======================

        //            ViewState["vCodingDescriptionID"] = CodingDescriptionID;
        //            // Added by Mrinal on 16th March 2015
        //            if (i == 0)
        //            {
        //                Line1CodingDescriptionID = CodingDescriptionID;
        //                Line1DepartmentID = DepartmentID;
        //            }
        //            // Addition End
        //        }

        //        else
        //        {
        //            NominalCodeID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //            DepartmentID = 0;//Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //            CodingDescriptionID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //            // Added by Mrinal on 16th March 2015
        //            if (i == 0)
        //            {
        //                Line1CodingDescriptionID = 0;
        //                Line1DepartmentID = 0;
        //            }

        //        }





        //        if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
        //        {
        //            BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
        //        }


        //        PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);


        //        NetValue = 0;
        //        if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
        //        {
        //            //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
        //            //{
        //            NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
        //            //}
        //        }

        //        LineVAT = 0;

        //        if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
        //        {
        //            //blocked by kuntal karar-on-25.03.2015----pt.48-------
        //            //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
        //            //{
        //            LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
        //            // }
        //            //------------------------------------------------------
        //        }
        //        strLineDescription = string.Empty;

        //        if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
        //        {
        //            strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
        //        }
        //        //Added By RImi on 8th August 2015

        //        if (strLineDescription.ToString().Contains("<"))
        //        {
        //            strLineDescription = strLineDescription.Replace("<", "&lt;");
        //        }
        //        if (strLineDescription.ToString().Contains(">"))
        //        {
        //            strLineDescription = strLineDescription.Replace(">", "&gt;");
        //        }
        //        if (strLineDescription.ToString().Contains("£"))
        //        {
        //            strLineDescription = strLineDescription.Replace("£", "&pound;");
        //        }
        //        if (strLineDescription.ToString().Contains("€"))
        //        {
        //            strLineDescription = strLineDescription.Replace("€", "&belongsto;");
        //        }
        //        #endregion
        //        //-----------modified by kuntal karar-on-25thMAR2015--------------
        //        //(NetValue > 0 ||

        //        if ((Convert.ToDecimal(Request.QueryString["iVat"]) >= 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))
        //        {
        //            //---------------------------------------------------------------
        //            DR = dtXML.NewRow();
        //            dtXML.Rows.Add(DR);
        //            sb.Append("<Rowss>");
        //            sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
        //            sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
        //            sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
        //            sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
        //            if (CodingDescriptionID > 0)
        //            {
        //                sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
        //            }
        //            else
        //            {
        //                sb.Append("<CodingDescriptionID>").Append(DBNull.Value).Append("</CodingDescriptionID>");
        //            }
        //            if (DepartmentID > 0)
        //            {
        //                sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
        //            }
        //            else
        //            {
        //                sb.Append("<DepartmentID>").Append(DBNull.Value).Append("</DepartmentID>");
        //            }

        //            if (NominalCodeID > 0)
        //            {
        //                sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
        //            }
        //            else
        //            {
        //                sb.Append("<NominalCodeID>").Append(DBNull.Value).Append("</NominalCodeID>");
        //            }
        //            //sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
        //            //sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
        //            int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
        //            if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
        //                sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
        //            else
        //                sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</BusinessUnitID>");

        //            sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
        //            sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
        //            sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");

        //            sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
        //            sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");
        //            sb.Append("</Rowss>");
        //        }
        //    }
        //    dsXML.Tables.Add(dtXML);
        //    sb.Append("</Root>");
        //    if (sb.ToString().Contains("&"))
        //    {
        //        sb = sb.Replace("&", "&amp;");
        //    }
        //    if (sb.ToString().Contains("'"))
        //    {
        //        sb = sb.Replace("'", "&apos;");
        //    }
        //    string strXmlText = sb.ToString();
        //    sb = null;


        //    int retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
        //    if (retvalalue > 0)
        //    {
        //        int isConditionSatisfied = CheckNominalRoutingAgainstInvoice(InvID, Line1CodingDescriptionID, Line1DepartmentID);
        //        if (isConditionSatisfied > 0)
        //        {
        //            #region: Refresh DropDown
        //            int InvDepartmentID = 0;
        //            string sSql = " SELECT ISNULL(DepartmentID, 0) As DepartmentID    FROM    dbo.Invoice     WHERE   InvoiceID=" + InvID;
        //            SqlDataReader dr = null;
        //            SqlConnection sqlConn = new SqlConnection(ConsString);
        //            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
        //            try
        //            {
        //                sqlConn.Open();
        //                dr = sqlCmd.ExecuteReader();
        //                while (dr.Read())
        //                {
        //                    if (dr[0] != DBNull.Value)
        //                    {
        //                        InvDepartmentID = Convert.ToInt32(dr[0]);

        //                    }
        //                }
        //            }
        //            catch (Exception ex) { string ss = ex.Message.ToString(); }
        //            finally
        //            {
        //                dr.Close();
        //                sqlCmd.Dispose();
        //                sqlConn.Close();
        //            }
        //            GetApproverDropDownsAgainstDepartment(InvDepartmentID);
        //            #endregion
        //        }
        //        retval = true;
        //    }
        //    else
        //        retval = false;


        //    return retval;
        //}


        //==================Commeneted on 21st August 2015 by Rimi==============================================


        //==================Added on 21st August 2015 by Rimi==============================================

        private bool SaveLineData()
        {
            #region variables
            int InvID = 0;
            //if (string.IsNullOrEmpty(ViewState["InvoiceChecking"] as string))
            //if(string.IsNullOrEmpty(ViewState["InvoiceChecking"].ToString())==false)
            if (Convert.ToInt32(ViewState["InvoiceChecking"]) != 0)//kk
            {
                InvID = Convert.ToInt32(ViewState["InvoiceChecking"]);

            }
            else
            {
                InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            }
            int CompanyID = 0;
            int CodingDescriptionID = 0;
            int NominalCodeID = 0;
            int BusinessUnitID = 0;
            int DepartmentID = 0;
            int iValidFlag = 0;
            decimal NetValue = 0;
            double NetVal = 0;
            string PurOrderNo = String.Empty;

            // Added by Mrinal on 13th March 2015
            decimal LineVAT = 0;
            string strLineDescription = string.Empty;
            // Addition End on 13th March 2015 
            bool retval = false;
            lblErrorMsg.Visible = false;


            int Line1CodingDescriptionID = 0;
            int Line1DepartmentID = 0;
            #endregion

            #region:XML Format
            DataSet dsXML = new DataSet();
            DataTable dtXML = new DataTable();
            dtXML.Columns.Add("SlNo");
            dtXML.Columns.Add("InvoiceID");
            dtXML.Columns.Add("InvoiceType");
            dtXML.Columns.Add("CompanyID");
            dtXML.Columns.Add("CodingDescriptionID");
            dtXML.Columns.Add("DepartmentID");
            dtXML.Columns.Add("NominalCodeID");
            dtXML.Columns.Add("BusinessUnitID");
            dtXML.Columns.Add("NetValue");
            dtXML.Columns.Add("CodingValue");
            dtXML.Columns.Add("PurOrderNo");//ss	
            // Added by Mrinal on 13th March 2015
            dtXML.Columns.Add("LineVAT");
            dtXML.Columns.Add("LineDescription");
            // Addition End on 13th March 2015 

            #endregion
            DataRow DR = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");

            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {
                #region Getting DropDown Values
                if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString()) != "--Select--" && Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
                {
                    CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                else
                {
                    Response.Write("<script>alert('Please select Company');</script>");
                    return false;
                }
                //if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString()) != "--Select--")
                //{


                //}




                if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
                {
                    //====================Commeneted By Rimi on 28th July 2015==================================== 
                    //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                    //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                    //ViewState["vDepartmentID"] = DepartmentID;

                    //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);

                    //===================================================================================================

                    //=====================Modified By Rimi on 28th July 2015=============================

                    string a = ((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim();
                    //Boolean res = GetCodingDetails(a.Substring(0,8));
                    int index = a.IndexOf("[");
                    if (index > 0)
                    {
                        Boolean res = GetCodingDetails(a.Substring(0, index));
                        if (res == false)
                        {

                            Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
                            return false;

                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Please Enter Valid Coding !!');</script>");
                        return false;
                    }
                    if (i > 0)
                    {

                        //if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim() != ((TextBox)grdList.Items[i - 1].FindControl("txtAutoCompleteCodingDescription")).Text.Trim())
                        //{
                        NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                        ViewState["hdnDepartmentCodeID"] = "2";
                        //}

                        //else
                        //{
                        //    NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = NominalCodeID.ToString();
                        //    ViewState["hdnDepartmentCodeID"] = "1";
                        //}

                    }
                    else
                    {
                        if (((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value != "")
                        {
                            NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                        }
                        ViewState["hdnDepartmentCodeID"] = "2";
                    }


                    //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                    //{
                    if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
                    {
                        DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                    }
                    //}
                    //else
                    //{
                    //    if (i > 0)
                    //    {
                    //        DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
                    ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
                    //    }
                    //}

                    //-------------------------------------------------------------------------


                    ViewState["vDepartmentID"] = DepartmentID;

                    //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                    //{
                    if (((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value != "")
                    {
                        CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                    }
                    //}
                    //else
                    //{
                    //    if (i > 0)
                    //    {
                    //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
                    ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();
                    //    }

                    //}
                    //-------------------------------------------------------------------------------
                    //====================Modified By Rimi on 28th July 2015 End======================

                    ViewState["vCodingDescriptionID"] = CodingDescriptionID;
                    // Added by Mrinal on 16th March 2015
                    if (i == 0)
                    {
                        Line1CodingDescriptionID = CodingDescriptionID;
                        Line1DepartmentID = DepartmentID;
                    }
                    // Addition End
                }

                else
                {
                    NominalCodeID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                    DepartmentID = 0;//Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                    CodingDescriptionID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                    // Added by Mrinal on 16th March 2015
                    if (i == 0)
                    {
                        Line1CodingDescriptionID = 0;
                        Line1DepartmentID = 0;
                    }

                }





                if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
                {
                    BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
                }


                PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);


                NetValue = 0;
                if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
                {
                    //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
                    //{
                    NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                    //}
                }

                LineVAT = 0;

                if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
                {

                    //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
                    //{
                    LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
                    // }
                    //------------------------------------------------------
                }
                strLineDescription = string.Empty;

                if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
                {
                    strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
                }
                //Added By RImi on 8th August 2015

                if (strLineDescription.ToString().Contains("<"))
                {
                    strLineDescription = strLineDescription.Replace("<", "&lt;");
                }
                if (strLineDescription.ToString().Contains(">"))
                {
                    strLineDescription = strLineDescription.Replace(">", "&gt;");
                }
                if (strLineDescription.ToString().Contains("£"))
                {
                    strLineDescription = strLineDescription.Replace("£", "&pound;");
                }
                if (strLineDescription.ToString().Contains("€"))
                {
                    strLineDescription = strLineDescription.Replace("€", "&belongsto;");
                }
                #endregion

                //(NetValue > 0 ||

                if ((Convert.ToDecimal(Request.QueryString["iVat"]) >= 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))
                {
                    //---------------------------------------------------------------
                    DR = dtXML.NewRow();
                    dtXML.Rows.Add(DR);
                    sb.Append("<Rowss>");
                    sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
                    sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
                    sb.Append("<InvoiceType>").Append("INV").Append("</InvoiceType>");
                    sb.Append("<CompanyID>").Append(Convert.ToString(CompanyID)).Append("</CompanyID>");
                    if (CodingDescriptionID > 0)
                    {
                        sb.Append("<CodingDescriptionID>").Append(Convert.ToString(CodingDescriptionID)).Append("</CodingDescriptionID>");
                    }
                    else
                    {
                        sb.Append("<CodingDescriptionID>").Append(DBNull.Value).Append("</CodingDescriptionID>");
                    }
                    if (DepartmentID > 0)
                    {
                        sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
                    }
                    else
                    {
                        sb.Append("<DepartmentID>").Append(DBNull.Value).Append("</DepartmentID>");
                    }

                    if (NominalCodeID > 0)
                    {
                        sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
                    }
                    else
                    {
                        sb.Append("<NominalCodeID>").Append(DBNull.Value).Append("</NominalCodeID>");
                    }
                    //sb.Append("<DepartmentID>").Append(Convert.ToString(DepartmentID)).Append("</DepartmentID>");
                    //sb.Append("<NominalCodeID>").Append(Convert.ToString(NominalCodeID)).Append("</NominalCodeID>");
                    int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                    if (((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue.Trim() == "--Select--")
                        sb.Append("<BusinessUnitID>").Append(Convert.ToString("0")).Append("</BusinessUnitID>");
                    else
                        sb.Append("<BusinessUnitID>").Append(Convert.ToString(BusinessUnitID)).Append("</BusinessUnitID>");

                    sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
                    sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
                    sb.Append("<PurOrderNo>").Append(Convert.ToString(PurOrderNo)).Append("</PurOrderNo>");

                    sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
                    sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");
                    sb.Append("</Rowss>");
                }
            }
            dsXML.Tables.Add(dtXML);
            sb.Append("</Root>");
            //Added By Rimi on 28th July 2015
            if (sb.ToString().Contains("&"))
            {
                sb = sb.Replace("&", "&amp;");
            }
            if (sb.ToString().Contains("'"))
            {
                sb = sb.Replace("'", "&apos;");
            }
            //Added By Rimi on 28th July 2015 End
            string strXmlText = sb.ToString();
            sb = null;


            int retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
            if (retvalalue > 0)
            {
                int isConditionSatisfied = CheckNominalRoutingAgainstInvoice(InvID, Line1CodingDescriptionID, Line1DepartmentID);
                if (isConditionSatisfied > 0)
                {
                    #region: Refresh DropDown
                    int InvDepartmentID = 0;
                    string sSql = " SELECT ISNULL(DepartmentID, 0) As DepartmentID    FROM    dbo.Invoice     WHERE   InvoiceID=" + InvID;
                    SqlDataReader dr = null;
                    SqlConnection sqlConn = new SqlConnection(ConsString);
                    SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
                    try
                    {
                        sqlConn.Open();
                        dr = sqlCmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr[0] != DBNull.Value)
                            {
                                InvDepartmentID = Convert.ToInt32(dr[0]);

                            }
                        }
                    }
                    catch (Exception ex) { string ss = ex.Message.ToString(); }
                    finally
                    {
                        dr.Close();
                        sqlCmd.Dispose();
                        sqlConn.Close();
                    }
                    GetApproverDropDownsAgainstDepartment(InvDepartmentID);
                    #endregion
                }
                retval = true;
            }
            else
                retval = false;


            return retval;
        }

        protected string GetInvoiceStatusLog()
        {
            string strInvoiceID = "";

            if (Convert.ToInt64(ViewState["InvoiceChecking"]) == 0)
            {
                strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);
            }
            else
            {
                strInvoiceID = Convert.ToString(ViewState["InvoiceChecking"]);
            }
            string strURL = "";
            //strURL = "javascript:window.open('../../JKS/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL','width=300,height=250,scrollbars=1');";
            strURL = "InvoiceStatusLogNL.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
            return (strURL);
        }



        protected string GetInvoiceStatusLogNextInvoice(string strInvoiceID)
        {

            string strURL = "";
            //strURL = "javascript:window.open('../../JKS/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL','width=300,height=250,scrollbars=1');";
            strURL = "InvoiceStatusLogNL.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
            return (strURL);
        }

        protected void InvoiceCrnIsDuplicate()
        {
            //string strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);
            //sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //SqlDataAdapter sqlDA = new SqlDataAdapter("sp_CheckInvoiceCrnIsDuplicate", sqlConn);
            //sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            //sqlDA.SelectCommand.Parameters.Add("@InvoiceID", strInvoiceID);
            //sqlDA.SelectCommand.Parameters.Add("@DocType", "INV");
            //DataSet ds = new DataSet();
            //try
            //{
            //    sqlConn.Open();
            //    sqlDA.Fill(ds);

            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        int IsDuplicate = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

            //        if (IsDuplicate > 0)
            //        {
            //            lblDuplicate.Visible = true;
            //        }
            //        else
            //        {
            //            lblDuplicate.Visible = false;
            //        }
            //    }
            //    else
            //    {
            //        lblDuplicate.Visible = false;
            //    }
            //}
            //catch (Exception ex) { string ss = ex.Message.ToString(); }
            //finally
            //{
            //    sqlDA.Dispose();
            //    sqlConn.Close();
            //}
        }




        protected void IsAutorisedtoEditDataNextInvoice(string strInvoiceID)
        {
            if (Session["UserID"] != null)
            {

                string strUserID = Convert.ToString(Session["UserID"]);
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("sp_IsAuthorisedToEditData", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@UserID", strUserID);
                DataSet ds = new DataSet();
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(ds);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int IsAuthorised = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                        if (IsAuthorised > 0)
                        {
                            aEditData.Visible = true;
                            //  aEditData.Attributes.Add("href", "InvoiceConfirmationNL.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1");
                            //  strEditLink = "InvoiceEdit.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1";


                            aEditData.Attributes.Add("onclick", "javascript:window.open('InvoiceEdit.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1', 'CustomPopUp','width=1280, height=786,scrollbars=1,resizable=1');return false;");
                            // aEditData.Attributes.Add("href", "InvoiceEdit.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1");
                            //---------------------------------------------------------------------------------


                        }
                        else
                        {
                            // lblDuplicate.Visible = false;
                            aEditData.Visible = false;
                        }
                    }
                    else
                    {
                        aEditData.Visible = false;
                    }
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
                finally
                {
                    sqlDA.Dispose();
                    sqlConn.Close();
                }
            }
            else
            {
            }
        }





















        protected void IsAutorisedtoEditData()
        {
            if (Session["UserID"] != null)
            {
                string strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);
                string strUserID = Convert.ToString(Session["UserID"]);
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("sp_IsAuthorisedToEditData", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@UserID", strUserID);
                DataSet ds = new DataSet();
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(ds);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int IsAuthorised = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                        if (IsAuthorised > 0)
                        {
                            aEditData.Visible = true;
                            //  aEditData.Attributes.Add("href", "InvoiceConfirmationNL.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1");
                            //  strEditLink = "InvoiceEdit.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1";


                            aEditData.Attributes.Add("onclick", "javascript:window.open('InvoiceEdit.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1', 'CustomPopUp','width=1200, height=650,scrollbars=1,resizable=1');return false;");
                            // aEditData.Attributes.Add("href", "InvoiceEdit.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1");
                            //---------------------------------------------------------------------------------


                        }
                        else
                        {
                            // lblDuplicate.Visible = false;
                            aEditData.Visible = false;
                        }
                    }
                    else
                    {
                        aEditData.Visible = false;
                    }
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
                finally
                {
                    sqlDA.Dispose();
                    sqlConn.Close();
                }
            }
            else
            {
            }
        }
        #endregion

        public string GetDecimalFormattedValue(object value)
        {
            string strGetDecimalFormattedValue = Convert.ToString("0.00");
            if (value != null && value != DBNull.Value)
            {
                double strAmount = 0;
                strAmount = Convert.ToDouble(value);
                strGetDecimalFormattedValue = strAmount.ToString("0.00");
            }

            return strGetDecimalFormattedValue;

        }
        protected void txtLineVAT_TextChanged(object sender, EventArgs e)
        {
            TextBox txtProcess = (TextBox)sender;
            //--------------------added by kuntalkarar on 13thSeptember2016--------------------------
            if (txtProcess.Text == "0.00")//for <>0.00 to 0.00
            {

                CalculateTotal();
            }


            if (string.IsNullOrEmpty(txtProcess.Text))
            {
                txtProcess.Text = Convert.ToString("0.00");
                CalculateTotal();
            }
            if (string.IsNullOrWhiteSpace(txtProcess.Text))
            {
                txtProcess.Text = Convert.ToString("0.00");
                CalculateTotal();
            }
            //-----------------------------------------------------------------------------------

            //^\d{1,16}((\.\d{1,4})|(\.))?$");
            Regex rg = new Regex(@"(^-?0\.[0-9]*[1-9]+[0-9]*$)|(^-?[1-9]+[0-9]*((\.[0-9]*[1-9]+[0-9]*$)|(\.[0-9]+)))|(^-?[1-9]+[0-9]*$)|(^0$){1}");
            //----------------------------------------------------------------
            if (rg.IsMatch(txtProcess.Text))
            {
                CalculateTotal();
            }
            else
            {
                txtProcess.Text = Convert.ToString("0.00");
            }
        }
        public bool CheckVarience()
        {

            bool Success = false;
            double dNetVariance = 0;
            double dVatVariance = 0;
            dNetVariance = Convert.ToDouble(lblVariance.Text);
            dVatVariance = Convert.ToDouble(lblVATVariance.Text);

            if (dNetVariance > 0 || dNetVariance < 0 || dVatVariance > 0 || dVatVariance < 0)
            {
                Success = false;
            }
            else
            {
                Success = true;
            }
            return Success;
        }
        // Added by Mrinal on 16th March 2015
        #region: CheckNominalRoutingAgainstInvoice Section
        public int CheckNominalRoutingAgainstInvoice(int iInvoiceID, int Line1CodingDescriptionID, int Line1DepartmentID)
        {
            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;

            try
            {
                // sqlCmd = new SqlCommand("sp_CheckCreditNoteAgainstInvoice", sqlConn);
                sqlCmd = new SqlCommand("CheckNominalRoutingAgainstInvoice", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
                sqlCmd.Parameters.Add("@CodingDescriptionID", Line1CodingDescriptionID);
                sqlCmd.Parameters.Add("@DepartmentID", Line1DepartmentID);
                sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                sqlReturnParam.Direction = ParameterDirection.ReturnValue;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); iReturnValue = -1; }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iReturnValue);
        }
        #endregion

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string[] GetCombinedDescription(string Filter, string BuyerCompanyID)
        {
            int strUserTypeID = 1;
            if (HttpContext.Current.Session["UserTypeID"] != null)
            {
                int.TryParse(HttpContext.Current.Session["UserTypeID"].ToString(), out strUserTypeID);
            }
            int strUserID = 0;
            if (HttpContext.Current.Session["UserID"] != null)
            {
                int.TryParse(HttpContext.Current.Session["UserID"].ToString(), out strUserID);
            }
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Fetch_CodingDescriptionNominalCodeDepartment", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(BuyerCompanyID));
            sqlDA.SelectCommand.Parameters.Add("@UserTypeID", strUserTypeID);
            sqlDA.SelectCommand.Parameters.Add("@Filter", Filter);
            sqlDA.SelectCommand.Parameters.Add("@UserID", strUserID);
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
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
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    myList.Add(string.Format("{0}#{1}#{2}#{3}", row["CodingDescription"].ToString(), Convert.ToInt32(row["CodingDescriptionID"].ToString()), Convert.ToInt32(row["DepartmentCodeID"].ToString()), Convert.ToInt32(row["NominalCodeID"].ToString())));
                }
                return myList.ToArray();
            }
            else
                return null;
        }

        /* private List<INVS> fetchNextInvoidceId(Int32 Invoiceid)
        {

            List<INVS> lstINVS = new List<INVS>();
            Int64 InvoicID = Invoiceid;
            List<INVS> lstInvoiceS = (List<INVS>)Session["InvoiceID"];

                List<INVS> lstInvoice = new List<INVS>();



                if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                {


                    lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]) - 1);
                    Session["InvoiceID"] = "";
                    Session["InvoiceID"] = lstInvoiceS;

                    ViewState["CheckList"] = 1;
                }

                //Added End
                foreach (INVS str in lstInvoiceS)
                {
                    if (str.InvoiceID != Convert.ToString(InvoicID))
                    {
                        //26-06-2015
                        DataSet DsInv = new DataSet();
                        DsInv = GetDocumentDetails(Convert.ToInt32(str.InvoiceID), "INV");
                        if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                        {


                            if (DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim() != "6")
                            {
                                INVS objINVS = new INVS();
                                objINVS.InvoiceID = str.InvoiceID;
                                lstINVS.Add(objINVS);
                            }
                        }
                        else
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = str.InvoiceID;
                            objINVS.DocType = str.DocType;
                            lstINVS.Add(objINVS);
                        }
                        //26-06-2015
                    }
                }

                int count = lstINVS.Count;
                foreach (INVS INV in lstINVS)
                {
                    if (lstINVS.Count <= count)
                    {
                        INVS objINVS = new INVS();
                        objINVS.InvoiceID = INV.InvoiceID;
                        objINVS.DocType = INV.DocType;
                        lstInvoice.Add(objINVS);
                    }

                }


                return lstInvoice;

            }*/


        //===============Commeneted By Rimi on 4TH August 2015===============================

        //private List<INVS> fetchNextInvoidceId(Int32 Invoiceid)
        //{

        //    List<INVS> lstINVS = new List<INVS>();
        //    Int64 InvoicID = Invoiceid;
        //    List<INVS> lstInvoiceS = (List<INVS>)Session["InvoiceID"];

        //    List<INVS> lstInvoice = new List<INVS>();



        //    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
        //    {


        //        lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]) - 1);
        //        Session["InvoiceID"] = "";
        //        Session["InvoiceID"] = lstInvoiceS;

        //        ViewState["CheckList"] = 1;
        //    }

        //    //Added End
        //    foreach (INVS str in lstInvoiceS)
        //    {
        //        if (str.InvoiceID != Convert.ToString(InvoicID))
        //        {
        //            //26-06-2015
        //            DataSet DsInv = new DataSet();
        //            DsInv = GetDocumentDetails(Convert.ToInt32(str.InvoiceID), "INV");
        //            //==========================Commeneted By Rimi on 10th July2015================
        //            //if (Convert.ToInt32(Session["UserTypeID"]) == 1)
        //            /// {


        //            //  if (DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim() != "6")
        //            // {
        //            //INVS objINVS = new INVS();
        //            // objINVS.InvoiceID = str.InvoiceID;
        //            // lstINVS.Add(objINVS);
        //            //}
        //            // }
        //            // else
        //            // {
        //            // INVS objINVS = new INVS();
        //            // objINVS.InvoiceID = str.InvoiceID;
        //            // objINVS.DocType = str.DocType;
        //            // lstINVS.Add(objINVS);
        //            //}
        //            //==========================Commeneted By Rimi on 10th July2015================

        //            //========Modified By Rimi on 10th July 2015=====================================================
        //            if (Convert.ToString(ViewState["RejectFlag"]) != "yes" && Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) != "Rejected")// Added By Rimi on 9th July
        //            {
        //                if (Convert.ToInt32(Session["UserTypeID"]) == 1)
        //                {


        //                    if (DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim() != "6")
        //                    {
        //                        INVS objINVS = new INVS();
        //                        objINVS.InvoiceID = str.InvoiceID;
        //                        lstINVS.Add(objINVS);
        //                    }
        //                }
        //                else// Added By Rimi on 9th July
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                    ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
        //                }
        //            }
        //            else
        //            {
        //                if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) != "Rejected")
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                    ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
        //                }
        //            }
        //            if (Convert.ToInt32(Session["UserTypeID"]) != 1)
        //            {
        //                if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected")
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                }
        //            }
        //            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
        //            {
        //                if ((Convert.ToString(ViewState["RejectFlag"]) == "yes" && Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected"))
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                    // ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
        //                }

        //                else if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) != "Cancel" && Convert.ToString(ViewState["MSG"]) == "Approve" || Convert.ToString(ViewState["MSG"]) == "Open" || Convert.ToString(ViewState["MSG"]) == "Delete" || Convert.ToString(ViewState["MSG"]) == "Reopen")//|| Convert.ToString(ViewState["MSG"]) == "Approve" || Convert.ToString(ViewState["MSG"]) == "Open" || Convert.ToString(ViewState["MSG"]) =="Delete" || Convert.ToString(ViewState["MSG"])=="Reopen"
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                }
        //                if (Convert.ToString(ViewState["RejectOnce"]) == "1")
        //                {
        //                    if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) == "Cancel")
        //                    {
        //                        INVS objINVS = new INVS();
        //                        objINVS.InvoiceID = str.InvoiceID;
        //                        objINVS.DocType = str.DocType;
        //                        lstINVS.Add(objINVS);
        //                    }
        //                }
        //                //else if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) == "Cancel" && Convert.ToString(DsInv.Tables[0].Rows[0]["DocType"]) != "CRN")
        //                //{
        //                //    INVS objINVS = new INVS();
        //                //    objINVS.InvoiceID = str.InvoiceID;
        //                //    objINVS.DocType = str.DocType;
        //                //    lstINVS.Add(objINVS);
        //                //}
        //            }
        //            //========Modified By Rimi on 10th July 2015 End=====================================================

        //            //26-06-2015

        //        }

        //    }

        //    int count = lstINVS.Count;
        //    foreach (INVS INV in lstINVS)
        //    {
        //        if (lstINVS.Count <= count)
        //        {
        //            INVS objINVS = new INVS();
        //            objINVS.InvoiceID = INV.InvoiceID;
        //            // objINVS.DocType = INV.DocType;
        //            lstInvoice.Add(objINVS);
        //        }

        //    }


        //    return lstInvoice;

        //}

        //==============Commeneted By Rimi on 4th AUGUST 2015 End==========================


        //4thAugust 2015
        //private List<INVS> fetchNextInvoidceId(Int32 Invoiceid)
        //{

        //    List<INVS> lstINVS = new List<INVS>();
        //    Int64 InvoicID = Invoiceid;
        //    List<INVS> lstInvoiceS = (List<INVS>)Session["InvoiceID"];

        //    List<INVS> lstInvoice = new List<INVS>();



        //    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
        //    {


        //        lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]) - 1);//Commeneted By Subhrajyoti on 27thJuly2015
        //        //lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]));//Added By Subhrajyoti on 27thJuly2015
        //        Session["InvoiceID"] = "";
        //        Session["InvoiceID"] = lstInvoiceS;

        //        ViewState["CheckList"] = 1;
        //    }

        //    //Added End
        //    foreach (INVS str in lstInvoiceS)
        //    {
        //        if (str.InvoiceID != Convert.ToString(InvoicID))
        //        {
        //            //26-06-2015
        //            DataSet DsInv = new DataSet();
        //            DsInv = GetDocumentDetails(Convert.ToInt32(str.InvoiceID), "INV");
        //            //==========================Commeneted By Rimi on 10th July2015================
        //            //if (Convert.ToInt32(Session["UserTypeID"]) == 1)
        //            /// {


        //            //  if (DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim() != "6")
        //            // {
        //            //INVS objINVS = new INVS();
        //            // objINVS.InvoiceID = str.InvoiceID;
        //            // lstINVS.Add(objINVS);
        //            //}
        //            // }
        //            // else
        //            // {
        //            // INVS objINVS = new INVS();
        //            // objINVS.InvoiceID = str.InvoiceID;
        //            // objINVS.DocType = str.DocType;
        //            // lstINVS.Add(objINVS);
        //            //}
        //            //==========================Commeneted By Rimi on 10th July2015================

        //            //========Modified By Rimi on 10th July 2015=====================================================
        //            if (Convert.ToString(ViewState["RejectFlag"]) != "yes" && Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) != "Rejected")// Added By Rimi on 9th July
        //            {
        //                if (Convert.ToInt32(Session["UserTypeID"]) == 1)
        //                {


        //                    if (DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim() != "6")
        //                    {
        //                        INVS objINVS = new INVS();
        //                        objINVS.InvoiceID = str.InvoiceID;
        //                        lstINVS.Add(objINVS);
        //                    }
        //                }
        //                else// Added By Rimi on 9th July
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                    ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
        //                }
        //            }
        //            else
        //            {
        //                if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) != "Rejected")
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                    ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
        //                }
        //            }
        //            if (Convert.ToInt32(Session["UserTypeID"]) != 1)
        //            {
        //                if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected")
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                }
        //            }
        //            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
        //            {
        //                if ((Convert.ToString(ViewState["RejectFlag"]) == "yes" && Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected"))
        //                {
        //                    INVS objINVS = new INVS();
        //                    objINVS.InvoiceID = str.InvoiceID;
        //                    objINVS.DocType = str.DocType;
        //                    lstINVS.Add(objINVS);
        //                    ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
        //                }

        //                //else if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) != "Cancel")// && Convert.ToString(ViewState["MSG"]) == "Approve" || Convert.ToString(ViewState["MSG"]) == "Open" || Convert.ToString(ViewState["MSG"]) == "Delete" || Convert.ToString(ViewState["MSG"]) == "Reopen")//|| Convert.ToString(ViewState["MSG"]) == "Approve" || Convert.ToString(ViewState["MSG"]) == "Open" || Convert.ToString(ViewState["MSG"]) =="Delete" || Convert.ToString(ViewState["MSG"])=="Reopen"
        //                //{
        //                //    INVS objINVS = new INVS();
        //                //    objINVS.InvoiceID = str.InvoiceID;
        //                //    objINVS.DocType = str.DocType;
        //                //    lstINVS.Add(objINVS);
        //                //}
        //                if (Convert.ToString(ViewState["RejectOnce"]) == "1")
        //                {
        //                    if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) == "Cancel")
        //                    {
        //                        INVS objINVS = new INVS();
        //                        objINVS.InvoiceID = str.InvoiceID;
        //                        objINVS.DocType = str.DocType;
        //                        lstINVS.Add(objINVS);
        //                    }
        //                }
        //                //else if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) == "Cancel" && Convert.ToString(DsInv.Tables[0].Rows[0]["DocType"]) != "CRN")
        //                //{
        //                //    INVS objINVS = new INVS();
        //                //    objINVS.InvoiceID = str.InvoiceID;
        //                //    objINVS.DocType = str.DocType;
        //                //    lstINVS.Add(objINVS);
        //                //}
        //            }
        //            //========Modified By Rimi on 10th July 2015 End=====================================================

        //            //26-06-2015

        //        }

        //    }

        //    int count = lstINVS.Count;
        //    foreach (INVS INV in lstINVS)
        //    {
        //        if (lstINVS.Count <= count)
        //        {
        //            INVS objINVS = new INVS();
        //            objINVS.InvoiceID = INV.InvoiceID;
        //            // objINVS.DocType = INV.DocType;
        //            lstInvoice.Add(objINVS);
        //        }

        //    }


        //    return lstInvoice;

        //}

        private List<INVS> fetchNextInvoidceId(Int32 Invoiceid)
        {

            List<INVS> lstINVS = new List<INVS>();
            Int64 InvoicID = Invoiceid;
            List<INVS> lstInvoiceS = (List<INVS>)Session["InvoiceID"];

            List<INVS> lstInvoice = new List<INVS>();



            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {


                //  lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]) - 1);//Commeneted By Rimi on 9th Nov 2015
                lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]));//Added By Rimi on 9th Nov 2015
                Session["InvoiceID"] = "";
                Session["InvoiceID"] = lstInvoiceS;

                ViewState["CheckList"] = 1;
            }


            //Added End
            var queryInvoice = (from r in lstInvoiceS
                                where r.InvoiceID != Convert.ToString(InvoicID)
                                select r).Take(100);
            if (queryInvoice.ToList().Count >= 0)
            {
                for (int ii = 0; ii < queryInvoice.ToList().Count; ii++)
                // for (int ii = 0; ii <=100; ii++)
                {
                    DataSet DsInv = new DataSet();
                    //DsInv = GetDocumentDetails(Convert.ToInt32(queryInvoice.ToList()[ii].InvoiceID), "INV");//Commeneted By RImi on 7th Nov 2015

                    DsInv = GetDocumentDetails(Convert.ToInt32(queryInvoice.ToList()[ii].InvoiceID), Convert.ToString(queryInvoice.ToList()[ii].DocType));//Added By RImi on 7th Nov 2015


                    //========Modified By Rimi on 10th July 2015=====================================================
                    if (Convert.ToString(ViewState["RejectFlag"]) != "yes" && Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) != "Rejected")// Added By Rimi on 9th July
                    {
                        if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                        {


                            if (DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim() != "6")
                            {
                                INVS objINVS = new INVS();
                                objINVS.InvoiceID = queryInvoice.ToList()[ii].InvoiceID;
                                lstINVS.Add(objINVS);
                            }
                        }
                        else// Added By Rimi on 9th July
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = queryInvoice.ToList()[ii].InvoiceID;
                            objINVS.DocType = queryInvoice.ToList()[ii].DocType;
                            lstINVS.Add(objINVS);
                            ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
                        }
                    }
                    else
                    {
                        if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) != "Rejected")
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = queryInvoice.ToList()[ii].InvoiceID;
                            objINVS.DocType = queryInvoice.ToList()[ii].DocType;
                            lstINVS.Add(objINVS);
                            ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
                        }
                    }
                    if (Convert.ToInt32(Session["UserTypeID"]) != 1)
                    {
                        if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected")
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = queryInvoice.ToList()[ii].InvoiceID;
                            objINVS.DocType = queryInvoice.ToList()[ii].DocType;
                            lstINVS.Add(objINVS);
                        }
                    }
                    if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                    {
                        if ((Convert.ToString(ViewState["RejectFlag"]) == "yes" && Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected"))
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = queryInvoice.ToList()[ii].InvoiceID;
                            objINVS.DocType = queryInvoice.ToList()[ii].DocType;
                            lstINVS.Add(objINVS);
                            ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
                        }


                        if (Convert.ToString(ViewState["RejectOnce"]) == "1")
                        {
                            if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) == "Cancel")
                            {
                                INVS objINVS = new INVS();
                                objINVS.InvoiceID = queryInvoice.ToList()[ii].InvoiceID;
                                objINVS.DocType = queryInvoice.ToList()[ii].DocType;
                                lstINVS.Add(objINVS);
                            }
                        }

                    }
                }

            }



            int count = lstINVS.Count;
            foreach (INVS INV in lstINVS)
            {
                if (lstINVS.Count <= count)
                {
                    INVS objINVS = new INVS();
                    objINVS.InvoiceID = INV.InvoiceID;
                    // objINVS.DocType = INV.DocType;
                    lstInvoice.Add(objINVS);
                }

            }


            return lstInvoice;

        }

        private string returnInvoiceId(Int32 Indx)
        {
            INVS p = new INVS();
            List<INVS> InvoiceId;
            InvoiceId = fetchNextInvoidceId(Convert.ToInt32(ViewState["InvoiceId"]));
            Session.Add("Invoice", InvoiceId);
            InvoiceId = (List<INVS>)(Session["Invoice"]);
            string RtnInvoiceId = InvoiceId[Indx].InvoiceID;
            return RtnInvoiceId;
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void grdList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        protected void grdFile_ItemCommand1(object source, DataGridCommandEventArgs e)
        {
            bool bFound = false;
            int DocumentID = 0;
            lblMsg.Text = "";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DocumentID = Convert.ToInt32(((Label)e.Item.FindControl("lblDocID")).Text);
                if (Convert.ToString(e.CommandArgument) == "DOW")
                {
                    string sDownLoadPath = ((Label)e.Item.FindControl("lblHidePath")).Text;
                    sDownLoadPath = sDownLoadPath.Replace("I:", "C:\\P2D");
                    sDownLoadPath = sDownLoadPath.Replace("\\90104-server2", "C:\\P2D");
                    sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
                    if (sDownLoadPath.Trim() != "")
                    {
                        if (System.IO.Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                        {
                            try
                            {
                                bFound = ForceDownload(sDownLoadPath, 0);
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            bFound = ForceDownload(sDownLoadPath, 0);
                        }
                    }
                    if (bFound == false)
                    {
                        sDownLoadPath = ((Label)e.Item.FindControl("lblArchPath")).Text;
                        sDownLoadPath = sDownLoadPath.Replace("\\90107-server3", @"C:\File Repository");
                        sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);

                        if (sDownLoadPath.Trim() != "")
                        {
                            if (System.IO.Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                            {
                                try
                                {

                                    bFound = ForceDownload(sDownLoadPath, 1);
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                bFound = ForceDownload(sDownLoadPath, 1);
                            }
                        }
                    }
                }
            }
        }
        protected void grdFile_ItemDataBound1(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (DataBinder.Eval(e.Item.DataItem, "archiveImagePath") != DBNull.Value)
                {
                    if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")) != "")
                    {
                        ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")).Trim());
                    }
                }
                else
                {
                    if (DataBinder.Eval(e.Item.DataItem, "ImagePath") != DBNull.Value)
                    {
                        if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")) != "")
                        {
                            ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")).Trim());
                        }
                        else
                        {
                            ((Label)e.Item.FindControl("lblPath")).Text = "N/A";
                        }
                    }
                }
            }
        }

        #region Download PDF file as per the name of Purchase Order by checking the existance of that file in server
        private void LoadDownloadFiles()
        {
            //Modified by kuntalkarar on 19thOctober2016---------------------------------

            //blocked by kuntalkarr on 19thOctober2016
            /*string InvoiceID = Request.QueryString["InvoiceID"].ToString().Trim();
           string CompanyID = Request.QueryString["DDCompanyID"].ToString().Trim();*/

            //added by kuntalkarar on 19thOctober2016
            string InvoiceID = "";
            string CompanyID = "";
            if (Request.QueryString["InvoiceID"] == null || Request.QueryString["DDCompanyID"] == null)
                return;

            if (Session["button_clicked"] == "1")// || ViewState["button_Reopened"] == "1")
            {
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "=====================================================================================");
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS-->>LoadDownloadFiles()-->>button_Clicked=1");
                InvoiceID = Session["NextInvoiceID"].ToString().Trim();
                CompanyID = Session["NextBuyerCompanyID"].ToString().Trim();
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>InvoiceID :- " + InvoiceID);
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>CompanyID :- " + CompanyID);
                Session["button_Clicked"] = "2";
                // ViewState["button_Reopened"] = "2";
            }
            else
            {
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "=====================================================================================");
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS-->>LoadDownloadFiles()-->>button_Clicked=1 ELSE");
                InvoiceID = Request.QueryString["InvoiceID"].ToString().Trim();
                CompanyID = Request.QueryString["DDCompanyID"].ToString().Trim();
            }

            //-------------------------------------------------------------------------------

            string CompanyName = "";

            try
            {
                //Modified for Normal user where no company selected from DROPDOWN by kuntalkarar on 20thOctober2016
                CompanyName = ReturnParentCompanyNameBySubCompanyID(Convert.ToInt64(CompanyID));//"Urban Leisure Group";// 
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS-->>LoadDownloadFiles()-->>CompanyID:- " + CompanyID);
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS-->>LoadDownloadFiles()-->>Parent CompanyName:- " + CompanyName);
            }
            catch
            {
                CompanyName = "Urban Leisure Group";
            }

            if (test)
                return;

            #region populate file list
            string fileName = "";
            string filePath = "";

            //Response.Write("<br />" + filePath);

            DataTable DT = ReturnPurOrderNosTable(Convert.ToInt64(InvoiceID));

            DataTable DT1 = new DataTable();
            DT1.Columns.Add("FileName");
            DT1.Columns.Add("FilePath");
            DataRow DR1 = null;

            WSCheckExistingFileFolder obj = new WSCheckExistingFileFolder();
            string urlStr = System.Configuration.ConfigurationManager.AppSettings["WSCheckExistingFileFolder"].ToString();
            obj.Url = urlStr;

            foreach (DataRow DR in DT.Rows)
            {
                filePath = @"C:\File Repository\FTP Archive\" + CompanyName + @" POs\";
                fileName = DR[0].ToString() + ".pdf";
                filePath = filePath + fileName;

                //Response.Write("<br />" + filePath);

                bool tf = obj.ReturnIfFileExists(filePath);

                //Response.Write("<br />" + tf);

                if (tf)
                {
                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS-->>LoadDownloadFiles()-->>If(tf)-->>filePath:-" + filePath);
                    DR1 = DT1.NewRow();

                    DR1[0] = fileName;
                    DR1[1] = filePath;

                    DT1.Rows.Add(DR1);
                }
            }

            gvFileLinks.DataSource = DT1;
            gvFileLinks.DataBind();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SubCompanyID"></param>
        /// <returns></returns>
        private string ReturnParentCompanyNameBySubCompanyID(long SubCompanyID)
        {
            string ParentCompanyName = "";

            if (SubCompanyID > 0)
            {
                SqlConnection sqlConn = new SqlConnection(ConsString);
                SqlCommand sqlCmd = new SqlCommand();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                DataTable DT = new DataTable();

                try
                {
                    //blocked by kuntalkarar on 20thOctober2016
                    string qry = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " +
                                 "(SELECT [ParentCompanyID] FROM [Company] WHERE [CompanyID] = @CompanyID);";

                    /*string qry = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = 180918";/* +
                               "(SELECT [ParentCompanyID] FROM [Company] WHERE [CompanyID] = @CompanyID);";*/
                    //-------------------------------------------------------------------------------------------
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@CompanyID", SqlDbType.Int).Value = SubCompanyID;
                    sqlConn.Open();
                    //id = (int)sqlCmd.ExecuteScalar();

                    sqlDA = new SqlDataAdapter(sqlCmd);
                    sqlDA.Fill(DT);

                    if (DT.Rows.Count > 0)
                    {
                        DataRow DR = DT.Rows[0];
                        ParentCompanyName = DR[0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                    HttpContext.Current.Response.Write("<br />Error in ReturnParentCompanyNameBySubCompanyID: " + ss);
                }
                finally
                {
                    sqlDA.Dispose();
                    DT.Dispose();
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return ParentCompanyName;
        }

        public DataTable ReturnPurOrderNosTable(long DocumentID)
        {
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter DA = new SqlDataAdapter();
            DataTable DT = new DataTable();
            try
            {
                DA = new SqlDataAdapter("SP_LIST_PURCHASE_NO_FOR_DOCUMENT_ID", sqlConn);
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DA.SelectCommand.Parameters.AddWithValue("@DocumentID", DocumentID);

                DA.Fill(DT);
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                HttpContext.Current.Response.Write("<br />Error in ReturnPurOrderNosTable: " + ss);
            }
            finally
            {
                DT.Dispose();
                DA.Dispose();
            }

            return DT;
        }

        protected void lnkFile_Click(object sender, EventArgs e)
        {
            //Response.Write("bbbb");

            LinkButton lnkFile = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)lnkFile.NamingContainer;
            Label lblPath = (Label)gvr.Cells[0].FindControl("lblPath");

            string filePath = lblPath.Text;

            bool tf = GetDownloadFile(filePath);

            if (!tf)
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Some error occurred, file could not be downloaded.');", true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool GetDownloadFile(string filePath)
        {
            string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceURL2"];
            CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
            objService.Url = serviceUrl;

            byte[] bytes = objService.DownloadFile(filePath);

            string fileNameOnly = Path.GetFileName(filePath);

            int i = fileNameOnly.Split('.').Length;
            string fileExt = fileNameOnly.Split('.')[i - 1];

            if (bytes != null)
            {
                string tempFilePath = Server.MapPath("~") + "\\Temp\\" + fileNameOnly;
                tempFilePath = tempFilePath.Replace("\\", @"\");

                File.WriteAllBytes(tempFilePath, bytes);

                bool tf = DownloadFile(tempFilePath, fileNameOnly, fileExt);

                return tf;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">Temporary file path to download the File.</param>
        /// <param name="fileName">The file name only.</param>
        /// <param name="fileType">The extension of the file.</param>
        /// <returns>true/false</returns>
        private bool DownloadFile(string filePath, string fileName, string fileType)
        {
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/" + fileType;
                Context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                long FileSize = fs.Length;
                byte[] getContent = new byte[(int)FileSize];
                fs.Read(getContent, 0, (int)fs.Length);
                fs.Close();
                Context.Response.BinaryWrite(getContent);
                Context.Response.Flush();
                Context.Response.End();

                return true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
                lblMsg.Text = "<br />Error in DownloadFile: " + ss;
                return false;
            }
            finally
            {
                if (!test)
                    DeleteFile(filePath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">Provide delete file name.</param>
        protected void DeleteFile(string filePath)
        {
            System.Threading.Thread th = null;

            th = new System.Threading.Thread(delegate()
            {
                bool isDel = false;
                while (isDel == false)
                {
                    try
                    {
                        File.Delete(filePath);
                        isDel = true;
                        th.Abort();
                    }
                    catch
                    {
                        isDel = false;
                    }
                }
            });

            th.Start();
            th.Join();
        }
        #endregion

        //Added by Mainak 2018-04-14
        #region btnProrateSubmit_Click
        protected void btnProrateSubmit_Click(object sender, System.EventArgs e)
        {
           
            int count1 = 0;
            try
            {
               int Count = grdList.Items.Count;
                if (Count > 0)
                {
                    string description = ((TextBox)grdList.Items[0].FindControl("txtLineDescription")).Text;
                    Session["lineDescription"] = description;
                }
                CheckInvoiceExistNew(); // Added by koushik das (kd) on 22-01-2019 for prorate
                //int count = chkDepartment.Items.Count;
                DataTable tbl = null;
                int count = 0;
                ArrayList selectedDepartment = new ArrayList();
                for (var i = 0; i < chkDepartment.Items.Count; i++)
                {
                    if (chkDepartment.Items[i].Selected == true)
                    {
                        selectedDepartment.Add(chkDepartment.Items[i].Value);
                        count++;
                        count1 = count;
                    }
                }
                // count = count;
                
                DataSet ds = ((DataSet)ViewState["populate"]);              
                //Session["lineDescription"] = ds.Tables[1].Rows[0]["Description"];
                int CodingDescriptionID = 0;
                if (ds.Tables[1].Rows[0]["CodingDescriptionID"].ToString() != "")
                {
                    CodingDescriptionID = Convert.ToInt32(ds.Tables[1].Rows[0]["CodingDescriptionID"]);
                }

                if (ds.Tables.Count > 0 && count > 0)
                {
                    DataSet dCD = new DataSet();
                    decimal netValue = Convert.ToDecimal(ViewState["CodingValue"]) / count;
                    decimal vat = Convert.ToDecimal(ViewState["lblTotalCodingVAT"]) / count;
                    Session["line_netValue"] = netValue;// Added by koushik das (kd) on 28-01-2019 for prorate
                    Session["line_vat"] = vat;
                    tbl = new DataTable();
                    DataRow nRow;

                    tbl.Columns.Add("CodingDescription");
                    tbl.Columns.Add("NetValue");
                    tbl.Columns.Add("PurOrderNo");
                    tbl.Columns.Add("VAT");
                    tbl.Columns.Add("CodingDescriptionID");
                    tbl.Columns.Add("DepartmentCodeID");
                    tbl.Columns.Add("NominalCodeID");
                    for (int i = 0; i < count; i++)
                    {

                        nRow = tbl.NewRow();

                        nRow["NetValue"] = netValue;
                        nRow["PurOrderNo"] = ds.Tables[1].Rows[0]["PurOrderNo"];
                        nRow["VAT"] = vat;
                        dCD = GetCodingDescription(CodingDescriptionID, selectedDepartment[i].ToString());
                        // if (dCD.Tables[0].Rows.Count > 0) commented by kd on 09-01-2019 for prorate
                        if (dCD.Tables.Count > 0)
                        {
                            nRow["CodingDescription"] = dCD.Tables[0].Rows[0]["CodingDescription"].ToString();
                            nRow["CodingDescriptionID"] = dCD.Tables[0].Rows[0]["CodingDescriptionID"].ToString();
                            nRow["NominalCodeID"] = dCD.Tables[0].Rows[0]["NominalCodeID"].ToString();
                            
                        }
                        else
                        {                           
                            nRow["CodingDescription"] = "";
                            nRow["CodingDescriptionID"] = "0";
                            nRow["NominalCodeID"] = "0";
                        }
                        nRow["DepartmentCodeID"] = selectedDepartment[i].ToString();

                        tbl.Rows.Add(nRow);
                    }

                    grdList.DataSource = tbl;
                    grdList.DataBind();
                    
                    ds = new DataSet();// Added by koushik das (kd) on 22-01-2019 for prorate
                    ds.Tables.Add(tbl);
                    ViewState["data"] = ds;
                    Session["data"] = ds; // Added by koushik das (kd) on 22-01-2019 for prorate
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value = tbl.Rows[i]["CodingDescriptionID"].ToString();
                        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value = tbl.Rows[i]["DepartmentCodeID"].ToString();
                        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value = tbl.Rows[i]["NominalCodeID"].ToString();
                        
                    }
                }
				// Added by koushik das (kd) on 28-01-2019 for prorate
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    
                    ((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text = Session["lineDescription"].ToString();
                    
                    //((TextBox)grdList.Items[i].FindControl("txtNetVal")).Text = Session["line_netValue"].ToString();
                    //((TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text = Session["line_vat"].ToString();
                }
                
            }
            catch (Exception ex)
            {

            }
            chkDepartment.ClearSelection();// Added by koushik das (kd) on 22-01-2019 for prorate
        }

        //Get different Coding decsription by selected DepartmentCodeID
        public DataSet GetCodingDescription(int CodingDescriptionID, string DepartmentCodeID)
        {
            //string CodingDescription = "";
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter("SP_GetCodingDescription_New", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CodingDescriptionID", CodingDescriptionID);
            sqlDA.SelectCommand.Parameters.Add("@DepartmentCodeID", DepartmentCodeID);


            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }

            //if (Dst.Tables[0].Rows.Count > 0)
            //{
            //    CodingDescription = Dst.Tables[0].Rows[0]["CodingDescription"].ToString();
            //}
            return Dst;

        }
        #endregion


        //Added by Mainak 2018-08-10 copied form btnRematch
        protected void btnRelease_Click(object sender, EventArgs e)
        {

            Session["button_clicked"] = "1";
            string InvoiceID_Remtach = "";

            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {
                InvoiceID_Remtach = Session["eInvoiceID"].ToString();
            }

            else
            {
                InvoiceID_Remtach = ViewState["InvoiceChecking"].ToString();
            }

            int iCount = 0;
            iCount = objCN.checkInvoiceCrditNoteIDExist(Convert.ToInt32(InvoiceID_Remtach), "INV");
            if (iCount > 0)
            {
                overlay1.Visible = true;
                dialog1.Visible = true;

            }
            else
            {
                string message = "alert('This document has been rejected and therefore can only be released if you have corrected the data using the Edit Data function. If the PO/GRN needs to be updated to allow the invoice to match please press REMATCH instead.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
        }

        protected void btnReleaseOk_Click(object sender, EventArgs e)
        {
            Session["button_clicked"] = "1";
            string InvoiceID_Remtach = "";

            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {
                InvoiceID_Remtach = Session["eInvoiceID"].ToString();
            }
            else
            {
                InvoiceID_Remtach = ViewState["InvoiceChecking"].ToString();
            }
            SqlParameter sqlOutputParam = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);

            SqlCommand sqlCmd = new SqlCommand("sp_ButtonReleasePress_Generic", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID_Remtach);
            sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"]));
            sqlCmd.Parameters.Add("@UserTypeID", Convert.ToInt32(Session["UserTypeID"]));

            if (txtComment.Text.Length > 0)
            {
                sqlCmd.Parameters.Add("@Comments", Convert.ToString(txtComment.Text));
            }

            else
            {
                sqlCmd.Parameters.Add("@Comments", "Rematch");
            }

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                MoveToNextInvoice();
                LoadDownloadFiles();
                //iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            overlay1.Visible = false;
            dialog1.Visible = false;

        }

        protected void btnReleaseCancel_Click(object sender, EventArgs e)
        {
            overlay1.Visible = false;
            dialog1.Visible = false;
        }

        //Added by Mainak 2018-08-20  
        #region GetInvoiceDetailsByInvoiceID
        public DataTable GetInvoiceDetailsByInvoiceID(int iInvoiceID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT * FROM Invoice WHERE InvoiceID=" + iInvoiceID;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlDA = new SqlDataAdapter(sSql, sqlConn);
            try
            {
                sqlConn.Open();
                sqlDA.Fill(Dst);

            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return Dst.Tables[0];
        }
        #endregion


        public void btnApproveOk_Click(object sender, EventArgs e)
        {
            vRFlag = Convert.ToInt32(hdnVRflag.Value);
            #region Approve
            Session["button_clicked"] = "1";
            lblmessage.Text = "";
            int ApprovedStatus = 0;
            int RetStatus = 0;
            int iRetValForUpdate = 0;

            lblErrorMsg.Text = "";
            if (lblcreditnoteno.Text != "")
            {
                string[] arrCrnNos1 = lblcreditnoteno.Text.Split('$');
                int CrnIDss = 0;
                for (int i = 0; i < arrCrnNos1.Length; i++)
                {
                    if (arrCrnNos1[i].ToString() != "")
                    {
                        CrnIDss = GetCreditNoteIDByCreditNoteNo(Convert.ToString(arrCrnNos1[i]));
                        int iCheckMultipleCoding = CheckCodingINVCRN(CrnIDss, "CRN");
                        if (iCheckMultipleCoding == 0)
                        {
                            Response.Write("<script>alert('Please apply coding to the Associated Credit Note');</script>");
                            return;
                        }
                    }
                }
            }
            int UserTypeID = Convert.ToInt32(Session["UserTypeID"]);
            if (UserTypeID < 2 && Convert.ToString(ViewState["StatusID"]) == "6")
            {
                if (txtCreditNoteNo.Text.Trim() == "" && lblcreditnoteno.Text == "")
                {
                    Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                    return;
                }
            }


            if (Convert.ToInt32(ddlRejection.SelectedIndex) > 0)
            {
                Response.Write("<script>alert('Sorry,Invoice cannot be Approved. You have selected RejectionCode.');</script>");
                return;
            }
            JKS.Invoice objinvoice = new JKS.Invoice();
            string strComments = txtComment.Text.Trim();
            //int UserTypeID =objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
            if (UserTypeID == 3 || UserTypeID == 2)
            {
                string strCreditInvoiceNoTemp = txtCreditNoteNo.Text.Trim();
                if (txtCreditNoteNo.Visible == true && txtCreditNoteNo.Text.Trim() != "")
                {
                    string strCreditInvoiceNo = CheckCreditNoteAgainstInvoice();

                    if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                    {


                        iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNoTemp);

                    }

                    else
                    {

                        iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(System.Convert.ToInt32(ViewState["InvoiceChecking"]), strCreditInvoiceNoTemp);

                    }


                    if (iRetValForUpdate == -101)
                    {
                        Response.Write("<script>alert('Please apply coding to the associated credit note.');</script>");
                        return;
                    }
                    if (iRetValForUpdate == -102)
                    {
                        Response.Write("<script>alert('Please apply coding to the associated credit note.');</script>");
                        return;
                    }
                    if (iRetValForUpdate == -103)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                        return;
                    }

                    int retVal = CheckIsFullCreditNote();
                    if (retVal == 0)
                    {
                        Response.Write("<script>alert('Sorry, you cannot approve a partial credit.');</script>");
                        int retDel = objinvoice.DeleteCreditInvoiceNOByCreditNoteiD(iRetValForUpdate);
                        return;
                    }
                    else
                    {

                        int iCheckCoding = CheckCodingINVCRN(iRetValForUpdate, "CRN");
                        if (iCheckCoding == 0)
                        {
                            Response.Write("<script>alert('Please apply coding to the Associated Credit Note');</script>");
                            return;
                        }

                        if (CheckVarience())
                        {
                            if (vRFlag == 0)
                            {
                                bool ret = SaveDetailData();

                                if (ret == true)
                                {
                                    int DeptUpdate = UpdateDepartmentAgainstInvoiceID();


                                    if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                                    {
                                        ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), iRetValForUpdate, "");
                                    }
                                    else
                                    {
                                        ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), iRetValForUpdate, "");
                                    }
                                    if (ApprovedStatus == 1)
                                    {
                                        doAction(0);
                                        PasswordReset objPasswordReset = new PasswordReset();

                                        if (Convert.ToInt64(ViewState["InvoiceChecking"]) == 0)
                                        {

                                            objPasswordReset.UpdateDepartmentId(Convert.ToString(Session["eInvoiceID"]), Convert.ToString(Session["InvoiceBuyerCompany"]), Convert.ToString(ViewState["vDepartmentID"]), Convert.ToString(ViewState["vCodingDescriptionID"]));

                                        }
                                        else
                                        {
                                            objPasswordReset.UpdateDepartmentId(Convert.ToString(ViewState["InvoiceChecking"]), Convert.ToString(Session["InvoiceBuyerCompany"]), Convert.ToString(ViewState["vDepartmentID"]), Convert.ToString(ViewState["vCodingDescriptionID"]));

                                        }
                                        GetDocumentDetails(Convert.ToInt32(Session["eInvoiceID"]));
                                        // GetApproverDropDownsAgainstDepartment(Convert.ToInt32(ViewState["DepartmentID"]));

                                        ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                                        MoveToNextInvoice();
                                        string message = "alert('Invoice Approved Successfully.')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);


                                    }
                                    else if (ApprovedStatus == -111)
                                    {

                                        string message = "alert('The associated credit note has not been coded')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                                        //   Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                                        return;
                                    }
                                    else
                                    {
                                        string message = "alert('Data not saved properly.')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                        //  Response.Write("<script>alert('Data not saved properly.');</script>");
                                    }

                                }
                            }
                            else
                            {
                                string message = "alert('VAT ÷ Net does not equal a valid tax rate.')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Variance must be zero.');</script>");
                        }
                    }

                }
                else
                {
                    if (CheckVarience())
                    {
                        if (vRFlag == 0)
                        {
                            bool ret = SaveDetailData();

                            if (ret == true)
                            {
                                int DeptUpdate = UpdateDepartmentAgainstInvoiceID();

                                int ICrnID = 0;
                                ICrnID = GetCreditNoteIDAgainstInvoiceIDANDCompanyID();
                                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                                {

                                    ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), ICrnID, "");
                                }
                                else
                                {
                                    ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), ICrnID, "");

                                }

                                if (ApprovedStatus == 1)
                                {
                                    doAction(0);
                                    //Response.Write("<script>alert('Invoice Approved Successfully'); self.close();</script>");
                                    // Added by Mrinal on 22nd September 2014
                                    //  Response.Write("<script>alert('Invoice Approved Successfully.');</script>");
                                    // GetURLTest();

                                    ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                                    MoveToNextInvoice();
                                    string message = "alert('Invoice Approved Successfully.')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                                }
                                else if (ApprovedStatus == -111)
                                {
                                    //Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                                    //MoveToNextInvoice();
                                    string message = "alert('The associated credit note has not been coded')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    return;
                                }
                                else
                                {
                                    string message = "alert('Data not saved properly.')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    //   Response.Write("<script>alert('Data not saved properly.');</script>");
                                }
                            }
                        }
                        else
                        {
                            string message = "alert('VAT ÷ Net does not equal a valid tax rate.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                    }
                    else
                    {
                        string message = "alert('Variance must be zero.')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        // Response.Write("<script>alert('Variance must be zero.');</script>");
                    }
                }
            }
            else
            {
                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {
                    RetStatus = CheckPermissionToTakeAction(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), System.Convert.ToInt32(Session["UserID"]));
                }
                else
                {
                    RetStatus = CheckPermissionToTakeAction(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]));

                }

                if (RetStatus > 0)
                {
                    iApproverStatusID = 19;
                    if (CheckVarience())
                    {
                        if (vRFlag == 0)
                        {
                            bool ret = SaveDetailData();

                            if (ret == true)
                            {
                                int ICrnID = 0;
                                ICrnID = GetCreditNoteIDAgainstInvoiceIDANDCompanyID();

                                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                                {
                                    ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), ICrnID, "");
                                }
                                else
                                {
                                    ApprovedStatus = ButtonApprovePress_Generic(System.Convert.ToInt32(ViewState["InvoiceChecking"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, txtComment.Text.Trim(), ICrnID, "");

                                }

                                if (ApprovedStatus == 1)
                                {
                                    doAction(0);
                                    lblErrorMsg.Text = "Invoice Approved Successfully";
                                    //	Response.Write("<script>alert('Invoice Approved Successfully'); self.close();</script>");

                                    // Added by Mrinal on 22nd September 2014
                                    //    Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                                    //    GetURLTest();

                                    ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                                    MoveToNextInvoice();
                                    string message = "alert('Invoice Approved Successfully')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }
                                else if (ApprovedStatus == -112)
                                {
                                    // Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                                    string message = "var r = alert('You have already actioned this invoice. Please press OK to close the window to allow you to resume.');if (typeof r == 'undefined') {close();}";//Added By Subhrajyoti on 28th July 2015
                                    // string message = "alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    return;
                                }
                                else if (ApprovedStatus == -111)
                                {
                                    // Response.Write("<script>alert('The associated credit note has not been coded');</script>");
                                    string message = "alert('The associated credit note has not been coded')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    return;
                                }
                                else
                                {
                                    //   Response.Write("<script>alert('Data not saved properly.');</script>");
                                    string message = "alert('Data not saved properly.')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }

                            }
                        }
                        else
                        {
                            string message = "alert('VAT ÷ Net does not equal a valid tax rate.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                    }
                    else
                    {
                        //  Response.Write("<script>alert('Variance must be zero.');</script>");
                        string message = "alert('Variance must be zero.')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }

                }
                else
                {
                    // Response.Write("<script>alert('You have already actioned this invoice. Please press the refresh button on your Internet browser to remove it from your Current folder.');</script>");
                    Response.Write("<script>var r = alert('You have already actioned this invoice. Please press OK to close the window to allow you to resume.');if (typeof r == 'undefined') {close();}</script>");// Added By Subhrojyoti on 28th July 2015
                    return;
                }
            }

            #endregion

            overlayApprove.Visible = false;
            dialogApprove.Visible = false;

        }

        protected void btnApproveCancel_Click(object sender, EventArgs e)
        {
            overlayApprove.Visible = false;
            dialogApprove.Visible = false;
        }

        // Added by koushik das (kd) on 22-01-2019 for prorate
        private void CheckInvoiceExistNew()
        {
            int RowCnt = 1;
            //  string ConsString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("ups_GetGenericCodingChange", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {
                sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(Request.QueryString["InvoiceID"]));
            }
            else
            {
                sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["CheckList"]));
            }
            sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RowCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    if (RowCnt == 0)
                    {
                        RowCnt = 1;
                        ViewState["Exist"] = "0";
                    }
                    else
                    {
                        ViewState["Exist"] = "1";
                    }
                }
                ViewState["populate"] = ds;
                Session["data"] = ds;
                BindGrid(RowCnt);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }
        // Added by koushik das (kd) on 22-01-2019 for prorate
        private void CheckInvoiceExistDetails()
        {
            int RowCnt = 1;

            DataSet ds = new DataSet();
            try
            {
                ds = ((DataSet)ViewState["populate"]);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RowCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    if (RowCnt == 0)
                    {
                        RowCnt = 1;
                        ViewState["Exist"] = "0";
                    }
                    else
                    {
                        ViewState["Exist"] = "1";
                    }
                }

            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }

        }

    }
}