using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;
using System.Net;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CBSolutions.ETH.Web;//by kuntal

namespace JKS 
{
    /// <summary>
    /// Summary description for ETChistory.
    /// </summary>
    /// 
    [ScriptService]
    public partial class ETChistory : SVSPage //was CBSolutions.ETH.Web.ETC.VSPage
    {
        #region User Defined Variables
        //added by kuntal karar---on 27.02.2015------
        public string lstSUPclicked = "";
        public string lstDOCclicked = "";
        //------------------------------
        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected bool PaginFlag = true;
        protected DataSet ds = null;
        protected DataTable objDataTable = null;
        protected DataRow drInvoiceHeader = null;
        protected DataRow drInvoiceInvoiceLog = null;
        protected DataRow dr = null;
        JKS.Invoice objInvoice = new JKS.Invoice();
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        protected string strInvoiceDocumentDownloadPath = ConfigurationManager.AppSettings["InvoiceDocPath"];
        int currentYear = 0;
        private int iLoadFlag = 0;

        private string strFromDate = "";
        private string strToDate = "";
        private decimal FromPrice;
        private decimal ToPrice;
        public static string strSortOrder = "";
        public static string strSortField = "";

        // Added by Mrinal on 28th January 2015
        DataTable dtRepeater = new DataTable();
        protected System.Web.UI.WebControls.Repeater rptAttachment;
        protected int iNeedRefreshToBottom = 0;

        //-----------------created by kuntal-------------------------------
        Boolean HasSearchCriteria = false;
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();

            //       Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //       Response.Cache.SetNoStore();
            //       Response.Cache.SetExpires(DateTime.MinValue);

            //       Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //Response.Cache.SetValidUntilExpires(False);
            //Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

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
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdInvCur.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.grdInvCur_PageIndexChanged);
            this.grdInvCur.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvCur_ItemDataBound);
            this.grdInvCur.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdInvCur_ItemCommand);
            this.grdInvCur.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.grdInvCur_SortCommand);
            this.Load += new System.EventHandler(this.Page_Load);
            this.btnDownloadAttachment.Click += new System.EventHandler(this.btnDownloadAttachment_Click);
          


            this.btnDownloadAttachment.Click += new System.EventHandler(this.btnDownloadAttachment_Click);

        }
        public void btnProcess_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("../../close_win.aspx");
            }
            else
            {
                this.btnSearch_Click(null, null);
            }
        }
        #endregion
        public int ChkUserID = 0;
        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");
            iNeedRefreshToBottom = 0;
            baseUtil.keepAlive(this);
            doAction();
            // added by kd on 08-12-2018
            
            ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));

            dgSalesCallDetails_INV.CurrentPageIndex = 0;
            dgSalesCallDetails.CurrentPageIndex = 0;

            if (!IsPostBack)
            {
                //dgSalesCallDetails_INV.CurrentPageIndex = 0;

                ViewState["dtCheckAttachment"] = null;
                lblMessage.Text = "";
                lblMessage.Visible = false;

                //added by kuntalkarar on 31stMay2017
         		txtSupplier.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
         		txtPONo.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
         		txtNominal.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                Utility.makeDefaultButton(txtInvoiceNo, btnSearch);
                Utility.makeDefaultButton(textRange1, btnSearch);
                Utility.makeDefaultButton(textRange2, btnSearch);

                Session["ApproveForm"] = 0;
                Session["SelectedPage"] = "PurchaseInvoiceLog";
                iLoadFlag++;
                Session["DropDownCompanyID"] = null;
                btnSearch.Attributes.Add("onclick", "javascript:return fn_Validate();");

                String str1 = "BuyerID=" + ((int)Session["CompanyID"]).ToString() + " and StatusId = 7";
                String str2 = "BuyerID=" + ((int)Session["CompanyID"]).ToString() + " and (StatusId != 7 or StatusId is null)";
                GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), 1);
                LoadDate();
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
                LoadDepartment();
                SetCompanyID(Session["CompanyID"].ToString());

                cbSupplier.Checked = true;
                cbInvoiceNo.Checked = true;
                divP2DLogo.Visible = true;
                //----------modified by kuntal on 18th mar2015---pt.46-------
                populateVendorClass(ddlCompany.SelectedIndex);
                //-----------------------------------------------------------

                //Modified by Koushik Das on 30-MAR-2017
                ApplySessionWhenReturnedFromEditPage();
                //Modified by Koushik Das on 30-MAR-2017
            }

            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
            {

                cbSupplier.Checked = true;
                cbSupplier.Disabled = true;
                cbInvoiceNo.Disabled = true;
            }

            if (Convert.ToInt32(Session["UserTypeID"]) == 3)
            {
                cbSupplier.Checked = true;
                cbSupplier.Disabled = true;
                cbInvoiceNo.Disabled = true;

            }

            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                cbSupplier.Checked = true;
                cbSupplier.Disabled = true;
                cbInvoiceNo.Disabled = true;

            }

        }
        #endregion

        #region doAction
        private void doAction()
        {
            string[] strOuterArray = null;
            string sInnerVal1 = "";
            string sInnerVal2 = "";
            string sInnerVal3 = "";
            string sRetValue = "", sDocType = "";
            int ID = 0; int IsDeleted = 0;
            sRetValue = Request["__EVENTARGUMENT"];
            if (sRetValue != null && sRetValue != "")
            {
                strOuterArray = sRetValue.Split('|');
                sInnerVal1 = strOuterArray[0].Replace("id=", String.Empty);
                sInnerVal2 = strOuterArray[1].Replace("docType=", String.Empty);
                sInnerVal3 = strOuterArray[2].Replace("IsDeleted=", String.Empty);
                ID = Convert.ToInt32(sInnerVal1);
                IsDeleted = Convert.ToInt32(sInnerVal3);
                sDocType = sInnerVal2;
            }
            if (ID > 0 && sDocType != "" && IsDeleted > 0)
            {
                if (Session["DropDownCompanyID"] != null)
                {
                    sDocType = ""; ID = 0;
                    Response.Redirect("ETChistory.aspx");
                }
                else
                {
                    sDocType = ""; ID = 0;
                    Response.Redirect("ETChistory.aspx");
                }
            }
        }
        #endregion

        #region GetDocType
        private string GetDocType()
        {
            string sDocType = "";
            if (ddlDocType.SelectedItem.Value != "NULL")
            {
                sDocType = ddlDocType.SelectedItem.Value;
            }
            return sDocType;
        }
        #endregion

        #region CreateTable
        private void CreateTable()
        {
            objDataTable = new DataTable("InvoiceDetails");

            objDataTable.Columns.Add("InvoiceID");
            objDataTable.Columns.Add("ReferenceNo");

            objDataTable.Columns.Add("Supplier");

            objDataTable.Columns.Add("Buyer");

            objDataTable.Columns.Add("VendorID");
            objDataTable.Columns.Add("InvoiceDate");

            objDataTable.Columns.Add("Currency");
            objDataTable.Columns.Add("Net");
            objDataTable.Columns.Add("VAT");
            objDataTable.Columns.Add("Total");
            objDataTable.Columns.Add("DocStatus");
            objDataTable.Columns.Add("ActionStatus");

            objDataTable.Columns.Add("PaymentDueDate");

            objDataTable.Columns.Add("NewDocType");

            objDataTable.Columns.Add("VoucherNumber");
            objDataTable.Columns.Add("ScanDate");

            objDataTable.Columns.Add("InvoiceDate1", typeof(int));
            objDataTable.Columns.Add("ScanDate1", typeof(int));

        }
        #endregion

        //------modified by kuntal karar on 27.02.2015-----------------------
        #region countRow
        public int countrow(int iCompanyID, string sDocType)
        {
            int rowcount = 0;
            if (txtSupplier.Value.ToString().Trim().Length <= 0)
            {
                HdSupplierId.Value = "";
            }
            if (txtInvoiceNo.Text.Trim().Length <= 0)
            {
                hdInvoiceNo.Value = "";
            }

            if (txtNominal.Value.Trim().Length <= 0)
            {
                hdNominalCodeId.Value = "";
            }

            JKS.Invoice objInvoice = new JKS.Invoice();

            lblMsg.Visible = false;
            lblMsg1.Visible = false;
            try
            {

                int CurrentCompanyID = 0;
                CurrentCompanyID = iCompanyID;
                if (CurrentCompanyID == 0)
                {
                    CurrentCompanyID = Convert.ToInt32(Session["CompanyID"]);
                }


                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

                sqlDA = new SqlDataAdapter("sp_RecordCount_GetGenericHistoryDetails_ETC_New", sqlConn);



                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDA.SelectCommand.Parameters.Add("@CompanyID", CurrentCompanyID);

                sqlDA.SelectCommand.Parameters.Add("@ActionStatusID", ddlActionStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@UserID", Session["UserID"].ToString().Trim());


                if (strFromDate.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@FromDate", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);

                if (strToDate.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@ToDate", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);



                if (txtPONo.Text.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@PONo", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@PONo", txtPONo.Text.Trim());

                if (Convert.ToString(ddlBusinessUnit.SelectedValue) == "0")
                {
                    sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", DBNull.Value);
                }
                else
                {
                    sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", Convert.ToInt32(ddlBusinessUnit.SelectedValue));
                }

                if (textRange1.Text.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@FromPrice", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@FromPrice", Convert.ToDecimal(FromPrice));

                if (textRange2.Text.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@ToPrice", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@ToPrice", Convert.ToDecimal(ToPrice));

                if (Convert.ToString(ddldept.SelectedValue) != "Select Department")
                    sqlDA.SelectCommand.Parameters.Add("@DepartmentID", ddldept.SelectedValue.Trim());
                else
                    sqlDA.SelectCommand.Parameters.Add("@DepartmentID", DBNull.Value);

                if (sDocType != "")
                {
                    sqlDA.SelectCommand.Parameters.Add("@DocType", sDocType);
                }
                else
                {
                    sqlDA.SelectCommand.Parameters.Add("@DocType", DBNull.Value);
                }

                if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                {
                    sqlDA.SelectCommand.Parameters.Add("@Option", 1);
                }
                else
                {
                    sqlDA.SelectCommand.Parameters.Add("@Option", DBNull.Value);
                }



                //------Supplier Wild Card
                int IsSentHdId = 0;
                if ((txtSupplier.Value.ToString().Trim() != "") && (cbSupplier.Checked))
                {
                    if (HdSupplierId.Value.ToString().Trim() != "")
                    {
                        sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", Convert.ToInt32(HdSupplierId.Value));
                        IsSentHdId = 1;

                    }
                    else
                        sqlDA.SelectCommand.Parameters.Add("@Filter", txtSupplier.Value.ToString().Trim());
                }
                else
                    sqlDA.SelectCommand.Parameters.Add("@Filter", DBNull.Value);

                if (IsSentHdId == 0)
                {
                    if ((HdSupplierId.Value.ToString().Trim() != "") && (cbSupplier.Checked == false))
                    {
                        sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", Convert.ToInt32(HdSupplierId.Value));

                    }
                    else
                    {
                        sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", 0);

                    }
                }

                //------InvoiceNo Wild Card
                IsSentHdId = 0;


                if ((txtInvoiceNo.Text.Trim() != "") && (cbInvoiceNo.Checked))
                {
                    if (hdInvoiceNo.Value.ToString().Trim() != "")
                    {
                        sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", hdInvoiceNo.Value.Trim().ToString());
                        IsSentHdId = 1;
                    }
                    else
                        sqlDA.SelectCommand.Parameters.Add("@InvoiceNoStr", txtInvoiceNo.Text.Trim());
                }

                else
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNoStr", DBNull.Value);

                if (IsSentHdId == 0)
                {
                    if ((hdInvoiceNo.Value.ToString().Trim() != "") && (cbInvoiceNo.Checked == false))
                        sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", hdInvoiceNo.Value.Trim().ToString());
                    else
                        sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", DBNull.Value);
                }
                IsSentHdId = 0;
                //-------------Add Filter For New Vendor Class and Nomina (Subha Das 2nd January 2015)
                if (Convert.ToString(ddlVendorClass.SelectedIndex) == "0")
                    sqlDA.SelectCommand.Parameters.Add("@New_VendorClass", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@New_VendorClass", ddlVendorClass.SelectedValue.Trim());

                if (hdNominalCodeId.Value != "")
                    sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", Convert.ToInt32(hdNominalCodeId.Value));
                else
                    sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", DBNull.Value);



                //  sqlDA.TableMappings.Add("InvoiceDetails", "InvoiceHeader");
                ds = new DataSet();
                sqlDA.SelectCommand.CommandTimeout = 0;
                sqlDA.Fill(ds);
                DataTable DT = new DataTable();
                DT = ds.Tables[0];
                rowcount = Convert.ToInt32(DT.Rows[0]["noofrecord"].ToString());
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            return rowcount;

        }
        #endregion
        //------------------------------------------------------------------
		
        #region LoadData
        private void LoadData(int iCompanyID, string sDocType)
        {

            //------modified by kuntal karar on 27.02.2015-----------------------
            //int Rcount = countrow(iCompanyID, sDocType);

            //if (Rcount > 1000)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowStatus", "javascript:alert('Search records exceed 1,000 documents. Please add more filters and try again.');", true);
            //    return;
            //}

            //---------------------------------------------------------------------------
            if (txtSupplier.Value.ToString().Trim().Length <= 0)
            {
                HdSupplierId.Value = "";
            }
            if (txtInvoiceNo.Text.Trim().Length <= 0)
            {
                hdInvoiceNo.Value = "";
            }

            if (txtNominal.Value.Trim().Length <= 0)
            {
                hdNominalCodeId.Value = "";
            }

            JKS.Invoice objInvoice = new JKS.Invoice();

            lblMsg.Visible = false;
            lblMsg1.Visible = false;
            try
            {
                CreateTable();
                CheckDate();
                int CurrentCompanyID = 0;
                CurrentCompanyID = iCompanyID;
                if (CurrentCompanyID == 0)
                {
                    CurrentCompanyID = Convert.ToInt32(Session["CompanyID"]);
                }


                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                //sqlDA = new SqlDataAdapter("GetGenericHistoryDetails_ETC", sqlConn);
                // sqlDA = new SqlDataAdapter("GetGenericHistoryDetails_ETC_New2", sqlConn);

                //blocked by kuntalkarar on 10thJanuary2017
                // sqlDA = new SqlDataAdapter("GetGenericHistoryDetails_ETC_New", sqlConn);
                sqlDA = new SqlDataAdapter("GetGenericHistoryDetails_ETC_New_JKS", sqlConn);


                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                //sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", CurrentCompanyID);
                //sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", ddlSupplier.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@ActionStatusID", ddlActionStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@DocStatusID", ddlDocStatus.SelectedValue.Trim());
                sqlDA.SelectCommand.Parameters.Add("@UserID", Session["UserID"].ToString().Trim());


                if (strFromDate.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@FromDate", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@FromDate", strFromDate);

                if (strToDate.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@ToDate", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@ToDate", strToDate);



                if (txtPONo.Text.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@PONo", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@PONo", txtPONo.Text.Trim());

                if (Convert.ToString(ddlBusinessUnit.SelectedValue) == "0")
                {
                    sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", DBNull.Value);
                }
                else
                {
                    sqlDA.SelectCommand.Parameters.Add("@BusinessUnitID", Convert.ToInt32(ddlBusinessUnit.SelectedValue));
                }

                if (textRange1.Text.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@FromPrice", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@FromPrice", Convert.ToDecimal(FromPrice));

                if (textRange2.Text.Trim() == "")
                    sqlDA.SelectCommand.Parameters.Add("@ToPrice", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@ToPrice", Convert.ToDecimal(ToPrice));

                if (Convert.ToString(ddldept.SelectedValue) != "Select Department")
                    sqlDA.SelectCommand.Parameters.Add("@DepartmentID", ddldept.SelectedValue.Trim());
                else
                    sqlDA.SelectCommand.Parameters.Add("@DepartmentID", DBNull.Value);

                if (sDocType != "")
                {
                    sqlDA.SelectCommand.Parameters.Add("@DocType", sDocType);
                }
                else
                {
                    sqlDA.SelectCommand.Parameters.Add("@DocType", DBNull.Value);
                }

                if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                {
                    sqlDA.SelectCommand.Parameters.Add("@Option", 1);
                }
                else
                {
                    sqlDA.SelectCommand.Parameters.Add("@Option", DBNull.Value);
                }

                // -------------Add Filter For Wild Card search (Subha Das 1st January 2015)

                //------Supplier Wild Card
                int IsSentHdId = 0;
                if ((txtSupplier.Value.ToString().Trim() != "") && (cbSupplier.Checked))
                {
                    if (HdSupplierId.Value.ToString().Trim() != "")
                    {
                        sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", Convert.ToInt32(HdSupplierId.Value));
                        IsSentHdId = 1;

                    }
                    else
                        sqlDA.SelectCommand.Parameters.Add("@Filter", txtSupplier.Value.ToString().Trim());
                }
                else
                    sqlDA.SelectCommand.Parameters.Add("@Filter", DBNull.Value);

                if (IsSentHdId == 0)
                {
                    if ((HdSupplierId.Value.ToString().Trim() != "") && (cbSupplier.Checked == false))
                        sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", Convert.ToInt32(HdSupplierId.Value));
                    else
                        sqlDA.SelectCommand.Parameters.Add("@SupplierCompanyID", 0);
                }

                //------InvoiceNo Wild Card
                IsSentHdId = 0;


                if ((txtInvoiceNo.Text.Trim() != "") && (cbInvoiceNo.Checked))
                {
                    if (hdInvoiceNo.Value.ToString().Trim() != "")
                    {
                        //==============Added By Rimi on 27th August 2015=============
                        if (hdInvoiceNo.Value.ToString().Contains("&"))
                        {
                            hdInvoiceNo.Value = hdInvoiceNo.Value.ToString().Replace("&", "&#38");
                        }
                        //==============Added By Rimi on 27th August 2015 End=============
                        sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", hdInvoiceNo.Value.Trim().ToString());
                        IsSentHdId = 1;
                    }
                    else
                        //==============Added By Rimi on 27th August 2015=============
                        if (txtInvoiceNo.Text.Contains("&"))
                        {
                            txtInvoiceNo.Text = txtInvoiceNo.Text.Replace("&", "&#38");
                        }
                    //==============Added By Rimi on 27th August 2015 End=============
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNoStr", txtInvoiceNo.Text.Trim());
                }

                else
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceNoStr", DBNull.Value);

                if (IsSentHdId == 0)
                {
                    if ((hdInvoiceNo.Value.ToString().Trim() != "") && (cbInvoiceNo.Checked == false))
                        sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", hdInvoiceNo.Value.Trim().ToString());
                    else
                        sqlDA.SelectCommand.Parameters.Add("@InvoiceNo", DBNull.Value);
                }
                IsSentHdId = 0;
                //-------------Add Filter For New Vendor Class and Nomina (Subha Das 2nd January 2015)
                if (Convert.ToString(ddlVendorClass.SelectedIndex) == "0")
                    sqlDA.SelectCommand.Parameters.Add("@New_VendorClass", DBNull.Value);
                else
                    sqlDA.SelectCommand.Parameters.Add("@New_VendorClass", ddlVendorClass.SelectedValue.Trim());

                if (hdNominalCodeId.Value != "")
                    sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", Convert.ToInt32(hdNominalCodeId.Value));
                else
                    sqlDA.SelectCommand.Parameters.Add("@NominalCodeID", DBNull.Value);

                // Adding section End              

                sqlDA.TableMappings.Add("InvoiceDetails", "InvoiceHeader");
                ds = new DataSet();
                sqlDA.SelectCommand.CommandTimeout = 0;
                sqlDA.Fill(ds, "InvoiceDetails");


            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
            #region: Multiple AttachmentDownload
            int TableCount = ds.Tables.Count;
            if (ds.Tables.Count > 1)
            {
                DataTable dtAttachment = ds.Tables[1];
                ViewState["dtAttachment"] = dtAttachment;
            }

            #endregion

            ViewState["objDataTable"] = ds.Tables["InvoiceHeader"];
            //added by Koushik Das as on 22-June-2017
            Session["ViewstateobjDataTable"] = ds.Tables["InvoiceHeader"];
            PopulateGrid();
        }
        #endregion

        #region PopulateGrid
        private void PopulateGrid()
        {
            try
            {
                DataTable dtbl = (DataTable)ViewState["objDataTable"];
                if (dtbl.Rows.Count > 0)
                {
                    if (PaginFlag == true && ViewState["CurrentPageIndex"] != null)
                    {
                        if (Convert.ToInt32(ViewState["CurrentPageIndex"]) <= grdInvCur.PageCount)
                        {
                            int CurrentPage = Convert.ToInt32(ViewState["CurrentPageIndex"]);
                            grdInvCur.CurrentPageIndex = CurrentPage;
                        }
                        else
                        {
                            ViewState["CurrentPageIndex"] = grdInvCur.PageCount;
                            grdInvCur.CurrentPageIndex = grdInvCur.PageCount;
                        }
                    }

                    lblMessage.Text = "";
                    lblMessage.Visible = false;
                    divP2DLogo.Visible = false;
                    //Modified by Koushik Das as on 27-Nov-2017
                    //grdInvCur.Visible = true;
                    //grdInvCur.DataSource = dtbl;
                    //grdInvCur.DataBind();

                    DataView dvgrdInvCur = new DataView(dtbl);
                    dvgrdInvCur.Sort = "NewDocType DESC, InvoiceDate1 DESC";
                    grdInvCur.DataSource = dvgrdInvCur;
                    grdInvCur.DataBind();
                    grdInvCur.Visible = true;
                    //Modified by Koushik Das as on 27-Nov-2017
                }
                else
                {
                    divP2DLogo.Visible = true;
                    grdInvCur.Visible = false;
                    lblMessage.Visible = true;
                    lblMessage.Text = "Sorry, no record(s) found.";
                }
            }
            catch (Exception ex)
            {
                string sExp = ex.Message;
            }
        }
        #endregion

        #region grdInvCur_PageIndexChanged
        private void grdInvCur_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex <= grdInvCur.PageCount)
            {
                grdInvCur.CurrentPageIndex = e.NewPageIndex;
                ViewState["CurrentPageIndex"] = e.NewPageIndex;
            }
            else
            {
                grdInvCur.CurrentPageIndex = grdInvCur.PageCount;
                ViewState["CurrentPageIndex"] = grdInvCur.PageCount;
            }

            //added by Koushik Das on 22-JUNE-2017 for setting the search information
            Session["PageIndex"] = grdInvCur.CurrentPageIndex;
            //added by Koushik Das on 22-JUNE-2017 for setting the search information

            PaginFlag = false;

            if (strSortField != "" && strSortOrder != "")
                GetAllCategoryByAdmin();
            else
                PopulateGrid();
        }
        #endregion

        #region GetDocumentWithPath
        protected string GetDocumentWithPath(object oDocument)
        {
            string strURL = "";
            string strDocument = "";
            strDocument = Convert.ToString(oDocument);
            if (strDocument.Trim() != "")
            {
                strURL = "<a href='" + strInvoiceDocumentDownloadPath + strDocument + "' target='_blank'> Download Document </a>";
            }
            else
            {
                strURL = "";
            }
            return (strURL);
        }
        #endregion

        public string invoiceID;

        #region grdInvCur_ItemCommand
        private void grdInvCur_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToUpper() == "ACT")
                {
                    int IsPermit = 0;
                    string sWinParam = "", sHeightWidth = "";
                    string strInvoiceID = ((Label)e.Item.FindControl("lblInvoiceID")).Text;
                    string strDocType = ((Label)e.Item.FindControl("lblDocType")).Text;
                    string strInvoiceDate = ((Label)e.Item.FindControl("lblInvoiceDate")).Text;
                    string strDocStatus = ((Label)e.Item.FindControl("lblDocStatus")).Text;
                    string strVoucherNumber = ((Label)e.Item.FindControl("lblVoucherNumber")).Text;

                    string strURL = "";
                    JKS.Invoice objinvoice = new JKS.Invoice();
                    string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));

                    if (IsPermit == 0)
                    {
                        if (strDocType == "INV")
                        {
                            sWinParam = "ActionHistory.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
                        }
                        else if (strDocType == "CRN")
                        {
                            sWinParam = "ActionHistory.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber;
                        }
                        sHeightWidth = "width=930,height=450,scrollbars=1,resizable=1";
                        strURL = "<script language=javascript>javascript:openBrWindow('" + sWinParam + "','','" + sHeightWidth + "')</script>";
                        this.RegisterClientScriptBlock("script", strURL);
                    }
                    else if (IsPermit == 1)
                    {
                        if (strDocType == "INV")
                        {
                            strURL = "<script language=javascript>alert('Another user has just actioned this Invoice and it has been locked out for 10 minutes. If after refreshing Internet Explorer you can still see the Invoice in your intray, it may then be actioned again after 10 minutes has elapsed.')</script>";
                            this.RegisterClientScriptBlock("script", strURL);
                            this.Page_Load(source, e);
                        }
                        else if (strDocType == "CRN")
                        {
                            strURL = "<script language=javascript>alert('Another user has just actioned this CreditNote and it has been locked out for 10 minutes. If after refreshing Internet Explorer you can still see the CreditNote in your intray, it may then be actioned again after 10 minutes has elapsed.')</script>";
                            this.RegisterClientScriptBlock("script", strURL);
                            this.Page_Load(source, e);
                        }
                        else if (strDocType == "DBN")
                        {
                            strURL = "<script language=javascript>alert('Another user has just actioned this DebitNote and it has been locked out for 10 minutes. If after refreshing Internet Explorer you can still see the DebitNote in your intray, it may then be actioned again after 10 minutes has elapsed.')</script>";
                            this.RegisterClientScriptBlock("script", strURL);
                            this.Page_Load(source, e);
                        }

                    }
                }

                    

                 //Added By Kd 05.12.2018
                else if (e.CommandName == "Status")
                {
                    string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                      invoiceID = Convert.ToString(commandArgs[0]);
                      ViewState["InvoiceID"] = invoiceID.Trim();
                    string doctype = Convert.ToString(commandArgs[1]);
                    if (doctype.Trim() == "CRN")
                    {
                        GetInvoiceStatusDetails_CRN(Convert.ToInt32(invoiceID));
                        mpe.Show();
                        //this.GetInvoiceStatusDetails_CRN(Convert.ToInt32(invoiceID));

                    }
                    else
                    {
                        GetInvoiceStatusDetails_INV(Convert.ToInt32(invoiceID));
                        mpe.Show();
                       // this.GetInvoiceStatusDetails_INV(Convert.ToInt32(invoiceID));
                    }
                }


            }



        }
        #endregion


        //Added By Kd 05.12.2018
       
        private DataTable dtbl = new DataTable();

        //Added By Kd 05.12.2018
        #region GetInvoiceStatusDetails
        private void GetInvoiceStatusDetails_CRN(int iInvoiceID)
        {
            JKS.Invoice objInvoice = new JKS.Invoice();
            //lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "CRN");
            //lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "CRN");
            //lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "CRN");

            //ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            else
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            //	dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier_CN(iInvoiceID);


            if (dtbl.Rows.Count > 0)
            {
                dgSalesCallDetails_INV.Visible = false;
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
                lblMessage.Text = "Sorry, this document has no previous actions.";
            }
        }
        #endregion
        //Added By Kd 05.12.2018
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
            GetInvoiceStatusDetails_CRN(Convert.ToInt32(ViewState["InvoiceID"]));
        }
        //Added By Kd 05.12.2018
        #region GetInvoiceStatusDetails_INV
        private void GetInvoiceStatusDetails_INV(int iInvoiceID)
        {
            //lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "INV");
            //lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "INV");
            //lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "INV");
          
            if (ChkUserID == 1)
                dtbl = objInvoice.GetInvoiceLogStatusApproverWise(iInvoiceID);

            else
                dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier(iInvoiceID);

            if (dtbl.Rows.Count > 0)
            {
                dgSalesCallDetails.Visible = false;
                dgSalesCallDetails_INV.Visible = true;
                dgSalesCallDetails_INV.DataSource = dtbl;
                dgSalesCallDetails_INV.DataBind();
                lblauthstring.Text = dtbl.Rows[0]["AuthorisationString"].ToString();
                lblDepartment.Text = dtbl.Rows[0]["DepartmentName"].ToString();
                lblBusinessUnit.Text = dtbl.Rows[0]["BusinessUnitName"].ToString();
            }
            else
            {
                dgSalesCallDetails_INV.Visible = false;
                lblMessage.Text = "Sorry, this document has no previous actions.";
            }

        }
        #endregion
        //Added By Kd 05.12.2018
        protected void dgSalesCallDetails_PageIndexChanged2(object source, DataGridPageChangedEventArgs e)
        {
            //dgSalesCallDetails_INV.PageIndex = 0;
            if (e.NewPageIndex < dgSalesCallDetails_INV.PageCount)
            {
                mpe.Show();
                this.dgSalesCallDetails_INV.CurrentPageIndex = e.NewPageIndex;
               
            }
            else
            {
                mpe.Show();
                this.dgSalesCallDetails_INV.CurrentPageIndex = dgSalesCallDetails.PageCount;
                
            }
            GetInvoiceStatusDetails_INV(Convert.ToInt32(ViewState["InvoiceID"]));

        }



        #region GetCommentURL
        protected string GetCommentURL(object oInvoiceID)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strURL = "";
            strURL = "javascript:window.open('show_comments.aspx?InvoiceID=" + strInvoiceID + "','a','width=675,height=450');";
            return (strURL);
        }
        #endregion

        #region GetLogURL
        protected string GetLogURL(object oInvoiceID)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strURL = "";
            strURL = "javascript:window.open('InvoiceLogHistory.aspx?InvoiceID=" + strInvoiceID + "','a','width=1000,height=650,scrollbars=yes');";
            return (strURL);
        }
        #endregion

        #region ddlCompany_SelectedIndexChanged
        private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //--------ASSING SUPPLIER NAME TO TEXT BOX . BY Subha Das (02/01/2015) 
            txtSupplier.Value = "";
            HdSupplierId.Value = "";

            txtInvoiceNo.Text = "";
            hdInvoiceNo.Value = "";

            txtNominal.Value = "";
            hdNominalCodeId.Value = "";

            JKS.Invoice objInvoice = new JKS.Invoice();
            //Blocked by Mrinal Chakravorty on 7th January 2015....
            //ddlSupplier.Items.Clear();
            //ddlSupplier.DataSource = objInvoice.GetSuppliersList(Convert.ToInt32(ddlCompany.SelectedValue.Trim()),Convert.ToInt32(Session["UserID"]),Convert.ToInt32(Session["UserTypeID"]));
            //ddlSupplier.DataBind();
            // Blocked END
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
            if (ddlCompany.SelectedItem.Text != "Select Company Name")
            {
                GetBusinessUnit(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            }
            LoadDepartment();

            // Added for Vendor Class (Subha Das 18th Dec 2014)
            if (ddlCompany.SelectedItem.Text != "Select Company Name") // Added By Rimi on 10thJuly2015
            {
                populateVendorClass(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));
            }
            // populateNominalName(Convert.ToInt32(ddlCompany.SelectedValue.Trim()));

        }
        #endregion

        // Added for Vendor Class (Subha Das 18th Dec 2014)
        #region populateVendorClass
        private void populateVendorClass(int iCompanyId)
        {
            ddlVendorClass.Items.Clear();
            if (iCompanyId != 0)
            {
                SqlConnection sqlConn = new SqlConnection(ConsString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetVendorClass_NewVersion", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
                sqlDA.SelectCommand.CommandTimeout = 0;

                DataSet ds = new DataSet();
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlVendorClass.DataSource = ds;
                        ddlVendorClass.DataBind();
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
                    ddlVendorClass.Items.Insert(0, new ListItem("Select Vendor Class", "0"));
                }
            }
            else

                ddlCompany.Focus();
        }
        #endregion

        #region populateNominalName
        //private void populateNominalName(int iCompanyId)
        //{
        //    ddlNominal.Items.Clear();
        //    if (iCompanyId != 0)
        //    {
        //        SqlConnection sqlConn = new SqlConnection(ConsString);
        //        SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetNominalName", sqlConn);
        //        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        //        sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
        //        sqlDA.SelectCommand.CommandTimeout = 0;
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            sqlConn.Open();
        //            sqlDA.Fill(ds);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                ddlNominal.DataSource = ds;
        //                ddlNominal.DataBind();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message.ToString();
        //        }
        //        finally
        //        {
        //            sqlConn.Close();
        //            sqlDA.Dispose();
        //            ds = null;
        //            ddlNominal.Items.Insert(0, new ListItem("Select Nominal Name", "0"));
        //        }
        //    }
        //    else

        //        ddlCompany.Focus();
        //}
        #endregion

        private void LoadDepartment()
        {
            ddldept.Items.Clear();
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
                ds = null;
            }
            ddldept.Items.Insert(0, new ListItem("Select Department", "0"));
        }
        //----------------modified by kuntal karar on 13.02.2015----------------------start----------------------

        public bool IsDecimalLimitedtoOneDecimal(string strValue)//TextBox inputvalue)
        {
            bool Success = false;
            decimal number;
            string value1 = strValue;
            if (decimal.TryParse(value1, out number))
            {
                Regex rx = new Regex(@"[0-9]*\.?[0-9]*");

                if (!rx.IsMatch(value1))
                {
                    // Response.Write(@"<script language='javascript'>alert('Only One Decimal point is allowed in NET TOTAL FROM!');</script>");
                    // textRange1.Focus();
                    Success = false;
                }
                else
                {
                    Success = true;
                }

            }

            return Success;
        }

        #region GetCompanyListForPurchaseInvoiceLog
        private void GetCompanyListForPurchaseInvoiceLog(int iCompanyID, int iAction)
        {
            if (iAction == 1)
            {
                Company objCompany = new Company();
                ddlCompany.Items.Clear();
                DataTable dt = new DataTable();

                dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                if (dt.Rows.Count > 0)
                {
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataBind();

                    // Added by Mrinal on 13th February 2015
                    if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                    {
                        ddlCompany.SelectedValue = Session["CompanyID"].ToString();
                    }
                    else
                    {
                        ddlCompany.SelectedValue = dt.Rows[0][0].ToString();
                    }

                    Session["DropDownCompanyID"] = ddlCompany.SelectedValue.ToString();
                }

                ddlCompany.Items.Insert(0, new ListItem("Select Company Name", "0"));
            }
            JKS.Invoice objInvoice = new JKS.Invoice();

            //Modified by Koushik Das on 30-MAR-2017
            try
            {
                ddlSupplier.Items.Clear();
                ddlSupplier.DataSource = objInvoice.GetSuppliersList(Convert.ToInt32(ddlCompany.SelectedValue), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(Session["UserTypeID"]));
                ddlSupplier.DataBind();
            }
            catch { }
            finally
            {
                ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
            }

            try
            {
                ddlDocStatus.Items.Clear();
                ddlDocStatus.DataSource = objInvoice.GetStatusListNL();
                ddlDocStatus.DataBind();
            }
            catch { }
            finally
            {
                ddlDocStatus.Items.Insert(0, new ListItem("Select Doc Status", "0"));
            }
            //Modified by Koushik Das on 30-MAR-2017
        }
        #endregion

        #region btnSearch_Click
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            #region Modified by Koushik Das on 22-JUNE-2017 for setting the search information
            string CompanyID, SupplierName, SupplierID, SupplierNameHD, SupplierStatus, DocStatusHD, isSupChecked, VendorClass, DocType, DocStatus, InvoiceNo, InvoiceID, InvoiceNoHD, isInvoice, PONo, BusinessUnit, Department, Nominal, NominalCodeId, FromDate, ToDate, NetFrom, NetTo;

            CompanyID = ddlCompany.SelectedValue;
            SupplierName = txtSupplier.Value;
            SupplierID = HdSupplierId.Value;
            SupplierNameHD = HdSupplierName.Value;
            SupplierStatus = HdSupplierStatus.Value;
            DocStatusHD = HdDocStatus.Value;
            isSupChecked = cbSupplier.Checked.ToString();
            VendorClass = ddlVendorClass.SelectedValue;
            DocType = ddlDocType.SelectedValue;
            DocStatus = ddlDocStatus.SelectedValue;
            InvoiceNo = txtInvoiceNo.Text;
            InvoiceID = hdInvoiceNo.Value;
            InvoiceNoHD = hdInvoiceNoTxt.Value;
            isInvoice = cbInvoiceNo.Checked.ToString();
            PONo = txtPONo.Text;
            BusinessUnit = ddlBusinessUnit.SelectedValue;
            Department = ddldept.SelectedValue;
            Nominal = txtNominal.Value;
            NominalCodeId = hdNominalCodeId.Value;
            FromDate = txtFromDate.Value;
            ToDate = txtToDate.Value;
            NetFrom = textRange1.Text;
            NetTo = textRange2.Text;

            StoreEditSession(CompanyID, SupplierName, SupplierID, SupplierNameHD, SupplierStatus, DocStatusHD, isSupChecked, VendorClass, DocType, DocStatus, InvoiceNo, InvoiceID, InvoiceNoHD, isInvoice, PONo, BusinessUnit, Department, Nominal, NominalCodeId, FromDate, ToDate, NetFrom, NetTo);
            #endregion

            //--------------------------------------------------------
            //added by kuntal karar on 20.03.2015--------------------
            if (ddlVendorClass.SelectedItem.Text != "Select Vendor Class")
            {
                HasSearchCriteria = true;
            }
            //-------------------------------------------------------
            if (txtFromDate.Value.Length != 0)
            {
                HasSearchCriteria = true;
            }
            if (txtInvoiceNo.Text.Length != 0)
            {
                HasSearchCriteria = true;
            }
            if (txtNominal.Value.Length != 0)
            {
                HasSearchCriteria = true;
            }
            if (txtPONo.Text.Length != 0)
            {
                HasSearchCriteria = true;
            }
            if (txtSupplier.Value.Length != 0)
            {
                HasSearchCriteria = true;
            }
            if (txtToDate.Value.Length != 0)
            {
                HasSearchCriteria = true;
            }
            //if (Convert.ToString(ddlVendorClass.SelectedItem.Text) != "Select Vendor Class" || Convert.ToString(ddlVendorClass.SelectedItem.Text) != "" || Convert.ToString(ddlVendorClass.SelectedItem.Text) != null)
            //{
            //    HasSearchCriteria = true;
            //}
            if (ddlBusinessUnit.SelectedItem.Text != "Select Business Unit")
            {
                HasSearchCriteria = true;
            }
            if (ddldept.SelectedItem.Text != "Select Department")
            {
                HasSearchCriteria = true;
            }
            if (textRange1.Text.Length != 0)
            {
                HasSearchCriteria = true;
            }
            if (textRange2.Text.Length != 0)
            {
                HasSearchCriteria = true;
            }
            if (ddlDocType.SelectedItem.Text != "Select Doc Type")
            {
                HasSearchCriteria = true;
            }
            if (ddlDocStatus.SelectedItem.Text != "Select Doc Status")
            {
                HasSearchCriteria = true;
            }
            //--------------------------------------------------------
            //----------------modified by kuntal karar on 27.02.2015-------------------------
            if (txtSupplier.Value != "" && cbSupplier.Checked == false && HdSupplierStatus.Value.ToString() == "")
            {
                HdSupplierStatus.Value = "";
                return;

            }
            if (txtSupplier.Value == "" && cbSupplier.Checked == false && HdSupplierStatus.Value.ToString() == "")
            {
                HdSupplierStatus.Value = "";
                return;
            }


            if (txtInvoiceNo.Text != "" && cbInvoiceNo.Checked == false && HdDocStatus.Value.ToString() == "")
            {
                return;
            }
            if (txtInvoiceNo.Text == "" && cbInvoiceNo.Checked == false && HdDocStatus.Value.ToString() == "")
            {
                HdDocStatus.Value = "";
                return;
            }
            //-------------------------------------------------------------------------------




            if (textRange1.Text != "")
            {
                if (!IsDecimalLimitedtoOneDecimal(textRange1.Text))
                {
                    Response.Write(@"<script language='javascript'>alert('Only One Decimal point is allowed.');</script>");
                    textRange1.Focus();
                    textRange1.Text = "";
                    return;
                }
            }
            if (textRange2.Text != "")
            {
                if (!IsDecimalLimitedtoOneDecimal(textRange2.Text))
                {
                    Response.Write(@"<script language='javascript'>alert('Only One Decimal point is allowed.');</script>");
                    textRange2.Focus();
                    textRange2.Text = "";
                    return;
                }
            }


            //by kuntal karar

            //    if (! IsDecimalLimitedtoTwoDigits(textRange1.Text))
            //    {
            //    Response.Write(@"<script language='javascript'>alert('Only One Decimal point is allowed!');</script>");
            //}




            if ((cbSupplier.Checked == false) && (HdSupplierId.Value == ""))
            {
                txtSupplier.Value = "";
            }
            if ((cbInvoiceNo.Checked == false) && (hdInvoiceNo.Value == ""))
            {
                txtInvoiceNo.Text = "";
            }

            if (Convert.ToString(ddldept.SelectedValue) == "Select Department" || textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
            {
                //CheckDate();
                if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
                {
                    FromPrice = Convert.ToDecimal(0);
                    ToPrice = Convert.ToDecimal(100000);
                }
                else
                {
                    if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                    {
                        lblMsg1.Text = "`From Range` cannot greater than `To Range`";
                        lblMsg1.Visible = true;
                    }
                    else
                    {
                        lblMsg.Text = "";
                        FromPrice = Convert.ToDecimal(textRange1.Text);
                        ToPrice = Convert.ToDecimal(textRange2.Text);

                    }
                }
            }
            else
            {
                if (Convert.ToString(ddldept.SelectedValue) == "Select Department")
                {
                    // CheckDate();
                    if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
                    {
                        FromPrice = Convert.ToDecimal(0);
                        ToPrice = Convert.ToDecimal(100000);
                    }
                    else
                    {
                        if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                        {
                            lblMsg1.Text = "`From Range` cannot greater than `To Range`";
                            lblMsg1.Visible = true;
                        }
                        else
                        {
                            lblMsg.Text = "";
                            FromPrice = Convert.ToDecimal(textRange1.Text);
                            ToPrice = Convert.ToDecimal(textRange2.Text);

                        }
                    }
                }
                else
                {
                    // CheckDate();
                    if (textRange1.Text.Length == 0 || textRange2.Text.Length == 0)
                    {
                        FromPrice = Convert.ToDecimal(0);
                        ToPrice = Convert.ToDecimal(100000);
                    }
                    else
                    {
                        if (Convert.ToDecimal(textRange1.Text) > Convert.ToDecimal(textRange2.Text))
                        {
                            lblMsg1.Text = "`From Range` cannot greater than `To Range`";
                            lblMsg1.Visible = true;
                        }
                        else
                        {
                            lblMsg.Text = "";
                            FromPrice = Convert.ToDecimal(textRange1.Text);
                            ToPrice = Convert.ToDecimal(textRange2.Text);

                        }
                    }
                }

            }
            try
            {
                grdInvCur.CurrentPageIndex = 0;
                ViewState["CurrentPageIndex"] = "0";

            }
            catch { }
            ViewState["dtCheckAttachment"] = null;

            if (txtSupplier.Value != "")
            {
                if (ddlCompany.SelectedItem.Text != "Select Company Name")
                {
                    LoadData(180918, GetDocType());//124529 for AnchorSafety changed to 180918 for JKS
                }
            }

            if (ddlVendorClass.SelectedItem.Text != "Select Vendor Class")
            {
                if (ddlCompany.SelectedItem.Text != "Select Company Name")
                {
                    LoadData(180918, GetDocType());//124529 for AnchorSafety changed to 180918 for JKS
                }

            }




            if (HasSearchCriteria == true)
            {
                if (ddlCompany.SelectedItem.Text == "Select Company Name" && Convert.ToInt32(Session["UserTypeID"]) == 1)
                {
                    LoadData(180918, GetDocType());//124529 for AnchorSafety changed to 180918 for JKS
                }
                else
                    if (ddlCompany.SelectedItem.Text == "Select Company Name" && Convert.ToInt32(Session["UserTypeID"]) == 1)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),

               "RegisterClientScriptBlockMethod", "<script>alert('Please select a company')</script>");
                    }
                    else
                        if (ddlCompany.SelectedItem.Text != "Select Company Name" && Convert.ToInt32(Session["UserTypeID"]) == 1)
                        {
                            LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), GetDocType());
                        }
                        else if ((ddlCompany.SelectedItem.Text != "Select Company Name") && (Convert.ToInt32(Session["UserTypeID"]) == 2 || Convert.ToInt32(Session["UserTypeID"]) == 3))
                        {
                            LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), GetDocType());
                        }
                        else if ((ddlCompany.SelectedItem.Text == "Select Company Name") && (Convert.ToInt32(Session["UserTypeID"]) == 2 || Convert.ToInt32(Session["UserTypeID"]) == 3))
                        {
                            LoadData(180918, GetDocType());//124529 for AnchorSafety changed to 180918 for JKS
                        }

            }
            else
            {
                //msg changed by on 20th March2015 by kuntal-------pt.33-----------
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowStatus", "javascript:alert('Please select at least one other search filter in addition to company name!');", true);
                return;
                //-----------------------------------------------------------------
            }

            //added by kuntal karar on 27.02.2015--------------------
            HdSupplierStatus.Value = "";
            HdDocStatus.Value = "";
            //-------------------------------------------------

            //Response.Redirect("history.aspx", true);
        }
        #endregion

        #region SetCompanyID
        private void SetCompanyID(string strCompanyID)
        {
            if (Convert.ToInt32(Session["UserTypeID"]) == 3)
            {
                ddlCompany.SelectedValue = strCompanyID.Trim();
            }
        }
        #endregion


        //Blocked By Kd 05.12.2018
        //#region GetStatusURL
        //protected string GetStatusURL(object oInvoiceID, object oDocType)
        //{
        //    string strInvoiceID = Convert.ToString(oInvoiceID);
        //    string strDocumentType = Convert.ToString(oDocType);
        //    string strURL = "";

        //    if (strDocumentType.Trim() == "CRN")
        //    {
        //        //strURL = "javascript:window.open('../../JKS/CreditNotes/InvoiceStatusLogNL_CN.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL_CN','width=700,height=500,scrollbars=1');";
        //        strURL = "../../JKS/CreditNotes/InvoiceStatusLogNL_CN.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
        //    }
        //    else
        //    {
        //        //strURL = "javascript:window.open('../../JKS/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL','width=700,height=500,scrollbars=1');";
        //        strURL = "../../JKS/invoice/InvoiceStatusLogNL.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
        //    }

        //    return (strURL);
        //}
        //#endregion

        #region GetStatusURLNew

        protected string GetIFrame(object oInvoiceID, object oDocType)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strDocumentType = Convert.ToString(oDocType);
            string strURL = "";

            if (strDocumentType.Trim() == "CRN")
            {
                // strURL = "javascript:window.open('../../JKS/CreditNotes/InvoiceStatusLogNL_CN.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL_CN','width=700,height=500,scrollbars=1');";
                strURL = "../creditnotes/InvoiceStatuslogNL_CN.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
                return (strURL);

            }
            else
            {
                //  strURL = "javascript:window.open('../../JKS/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL','width=700,height=500,scrollbars=1');";
                strURL = "../invoice/InvoiceStatusLogNL.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
                return (strURL);
            }
            return (strURL);
        }
        #endregion

        #region CheckDuplicateValues
        private void CheckDuplicateValues()
        {
            for (int i = 0; i < grdInvCur.Items.Count; i++)
            {
                if (i > 0)
                {
                    if ((grdInvCur.Items[i].Cells[23].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[23].Text.Trim())) && (grdInvCur.Items[i].Cells[4].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[4].Text.Trim())) && (grdInvCur.Items[i].Cells[6].Text.Trim().Equals(grdInvCur.Items[i - 1].Cells[6].Text.Trim())))
                    {
                        grdInvCur.Items[i - 1].BackColor = Color.Red;
                        grdInvCur.Items[i].BackColor = Color.Red;
                        Session.Add("Duplicate", "Yes");
                    }
                }
            }
        }
        #endregion

        #region ddlSupplier_SelectedIndexChanged
        private void ddlSupplier_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlSupplier.SelectedIndex != 0)
            {
                JKS.Invoice objInvoice = new JKS.Invoice();
                try
                {
                    lblMsg.Text = "";

                }
                catch { }

                try
                {
                    ddldept.ClearSelection();
                    lblMsg.Text = "";
                }
                catch
                {
                    if (Convert.ToInt32(Session["UserTypeID"]) > 1)
                    {
                        ddldept.DataSource = objInvoice.GetDepartmentListDropDown(1, 0);
                        ddldept.DataBind();
                        ddldept.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ddldept.DataSource = objInvoice.GetDepartmentListDropDown(0, Convert.ToInt32(Session["UserID"]));
                        ddldept.DataBind();
                        ddldept.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
            }
        }
        #endregion

        #region LoadDate
        private void LoadDate()
        {
            /*ddlYear.Items.Insert(0,new ListItem("Year","0"));
            ddlYear1.Items.Insert(0,new ListItem("Year","0"));
            ddlMonth.Items.Insert(0,new ListItem("Month","0"));
            ddlMonth1.Items.Insert(0,new ListItem("Month","0"));
            ddlday.Items.Insert(0,new ListItem("Day","0"));
            ddlday1.Items.Insert(0,new ListItem("Day","0"));
			
            currentYear=Microsoft.VisualBasic.DateAndTime.Year(System.DateTime.Now);
            for(int i=(currentYear-5);i<=(currentYear+5);i++)
                ddlYear.Items.Add(i.ToString());
            for(int i=1;i<13;i++)
            {
                ddlMonth.Items.Add(new ListItem(Microsoft.VisualBasic.DateAndTime.MonthName(i,true),(i.ToString())));
                ddlMonth1.Items.Add(new ListItem(Microsoft.VisualBasic.DateAndTime.MonthName(i,true),(i.ToString())));
            }
            for(int i=1;i<32;i++)
            {
                ddlday.Items.Add(i.ToString());
                ddlday1.Items.Add(i.ToString());
            }
            for(int i=(currentYear-5); i<=(currentYear+5);i++)
                ddlYear1.Items.Add(i.ToString());
             */
        }

        #endregion

        #region Getredirecturl
        protected string Getredirecturl(object DocType, object ReferenceNo, object InvoiceNo)
        {
            string URL = "";
            string Reference;
            string DocumentType = "";
            DocumentType = Convert.ToString(DocType);
            Reference = Convert.ToString(ReferenceNo);
            int invoiceID = Convert.ToInt32(InvoiceNo);
            if (DocumentType == "CRN")
            {
                URL = "<a href='../../JKS/CreditNotes/InvoiceConfirmationNL_CN.aspx?InvoiceID=" + invoiceID + "&AllowEdit=History'>" + ReferenceNo + "</a>";
            }
            else if (DocumentType == "DBN")
            {
                URL = "<a href='../../JKS/invoice/InvoiceConfirmationNL_DN.aspx?InvoiceID=" + invoiceID + "'>" + ReferenceNo + "</a>";
            }
            else if (DocumentType == "PO")
            {
                URL = "<a href='../../JKS/SalesOrders/InvoiceConfirmation_PO.aspx?PurchaseOrderID=" + invoiceID + "'>" + ReferenceNo + "</a>";
            }
            else
            {
                URL = "<a href='../../JKS/invoice/InvoiceConfirmationNL.aspx?InvoiceID=" + invoiceID + "&AllowEdit=History'>" + ReferenceNo + "</a>";
            }
            return (URL);

        }

        #endregion

        #region CheckDate

        private void CheckDate()
        {
            if (txtFromDate.Value != "" || txtToDate.Value != "")
            {
                if ((txtFromDate.Value.ToString().Trim().Length == 10) && (txtToDate.Value.ToString().Trim().Length == 10))
                {
                    strFromDate = DateTime.ParseExact(txtFromDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    strToDate = DateTime.ParseExact(txtToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    Response.Write(@"<script language='javascript'>alert('Date must be in dd/mm/yyyy format. Please select date from the calendar.');</script>");

                    if (txtFromDate.Value.ToString().Trim().Length < 10)
                    {
                        txtFromDate.Value = "";
                        strFromDate = "";
                    }
                    if (txtToDate.Value.ToString().Trim().Length < 10)
                    {
                        txtToDate.Value = "";
                        strToDate = "";
                    }

                }
            }
            else
            {
                strFromDate = "";
                strToDate = "";

            }
        }
        #endregion

        #region private void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        private void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            string sRetUrl = "../history/ETChistory.aspx";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                // Added by Mrinal on 28th January 2015
                int iInvID = 0;
                iInvID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
                rptAttachment = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptAttachment");
                rptAttachment.DataSource = null;
                rptAttachment.DataBind();
                dtRepeater = GetAttachmentDetails(iInvID, Convert.ToString(DataBinder.Eval(e.Item.DataItem, "NewDocType")));
                if (dtRepeater.Rows.Count > 0)
                {
                    rptAttachment.DataSource = dtRepeater;
                    rptAttachment.DataBind();
                }


                bool IsDuplicate = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsDuplicate"));
                if (IsDuplicate)
                {
                    e.Item.CssClass = "ColorDuplicateRow td";
                }
                //if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "IsDuplicate")) == 1)
                //{
                //    e.Item.CssClass = "ColorDuplicateRow td";
                //}

                // Addition end by Mrinal on 28th January 2015




                int ID = 0;
                if (DataBinder.Eval(e.Item.DataItem, "NewDocType") != DBNull.Value && Convert.ToString(DataBinder.Eval(e.Item.DataItem, "NewDocType")) != "")
                {
                    if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "NewDocType")).ToUpper() == "INV")
                    {
                        ID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
                        ((HyperLink)e.Item.FindControl("hpDoc")).NavigateUrl = "../invoice/InvoiceFileManager_NL.aspx?From=ETC&InvoiceID=" + ID + "&ReturnUrl=" + sRetUrl;
                    }
                    else if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "NewDocType")).ToUpper() == "CRN")
                    {
                        ID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID"));
                        ((HyperLink)e.Item.FindControl("hpDoc")).NavigateUrl = "../creditnotes/CreditnoteFileManager_NL.aspx?From=ETC&CreditNoteID=" + ID + "&ReturnUrl=" + sRetUrl;
                    }
                }
                int sHold = objInvoice.GetAPCommLinkColor(Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "InvoiceID")), Convert.ToString(DataBinder.Eval(e.Item.DataItem, "NewDocType")));

                if (sHold == 1)
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/red_hold.gif";
                }
                else if (sHold == 0)
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/yellow_hold.gif";
                }
                else
                {
                    ((System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComment")).Src = "../../images/green_hold.gif";
                }


                if (ViewState["dtCheckAttachment"] != null)
                {
                    DataTable dtAttachmentCheck = (DataTable)ViewState["dtCheckAttachment"];
                    DataView dvSelectedAttachment = new DataView(dtAttachmentCheck);
                    dvSelectedAttachment.Sort = "InvoiceID ASC";
                    dvSelectedAttachment.RowFilter = "InvoiceID=" + Convert.ToInt32(iInvID) + " And DocType='" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "NewDocType")).ToUpper() + "'";
                    if (dvSelectedAttachment.ToTable().Rows.Count > 0)
                    {
                        ((CheckBox)e.Item.FindControl("chkDownload")).Checked = true;
                    }
                }


                //Blocked By KD 05.12.2018
                //string strStatusLogLink = GetStatusURL(DataBinder.Eval(e.Item.DataItem, "InvoiceID"), DataBinder.Eval(e.Item.DataItem, "NewDocType"));
                //strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:580,height:390,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                //System.Web.UI.HtmlControls.HtmlAnchor aStatusHistory = ((System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("aStatusHistory"));
                //aStatusHistory.Attributes.Add("onclick", strStatusLogLink);


            }
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

        #region GetURLTest
        //New_VendorClass added by kuntalkarar on 6thJanuary2017
        protected string GetURLTest(object oInvoiceID, object oDocType, object oInvDate, object oDocStatus, object oVoucherNumber, object oNew_VendorClass)
        {
            //			int	IsPermit = 0;
            JKS.Invoice objinvoice = new JKS.Invoice();
            string DocType = Convert.ToString(oDocType);
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strInvoiceDate = Convert.ToString(oInvDate);
            string strDocStatus = Convert.ToString(oDocStatus);
            string strVoucherNumber = Convert.ToString(oVoucherNumber);
            //New_VendorClass added by kuntalkarar on 6thJanuary2017
            string New_VendorClass = Convert.ToString(oNew_VendorClass);


            string strURL = "";
            if (DocType == "INV")
            {
                //New_VendorClass added by kuntalkarar on 6thJanuary2017
                string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));
                strURL = "javascript:window.open('ActionHistoryNew.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber + "&NewVendorClass=" + New_VendorClass + "','abb','width=1080,height=750,top=100,left=100,scrollbars=1,resizable=1');";
                // strURL = "javascript:window.open('ActionHistoryNew.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber + "','abb','width=' + screen.width +',height=' + screen.width +',top=100,left=100,scrollbars=1,resizable=1');";
            }
            if (DocType == "CRN")
            {
                //New_VendorClass added by kuntalkarar on 6thJanuary2017
                strURL = "javascript:window.open('ActionHistoryNew.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber + "&NewVendorClass=" + New_VendorClass + "','abb','width=1080,height=750,top=100,left=100,scrollbars=1,resizable=1');";
                // strURL = "javascript:window.open('ActionHistoryNew.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber + "','abb','width=' + screen.width +',height=' + screen.height + ',top=100,left=100,scrollbars=1,resizable=1');";  
            }
            // string RelationType = objinvoice.GetRelationType(Convert.ToInt32(strInvoiceID));
            //strURL = "javascript:window.open('HistoryCombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber + "','HistoryCombindWindow','fullscreen,scrollbars');";
            //New_VendorClass added by kuntalkarar on 6thJanuary2017
            strURL = "javascript:window.open('HistoryCombindWindow.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + DocType + "&InvoiceDate=" + strInvoiceDate + "&DocStatus=" + strDocStatus + "&VoucherNumber=" + strVoucherNumber + "&NewVendorClass=" + New_VendorClass + "','HistoryCombindWindow','height=' + screen.height + ',width=' + screen.width +',scrollbars,top=0,left=0,resizable=0');";

            return (strURL);
        }

        #endregion

        #region GetAPCommentsURL
        protected string GetAPCommentsURL(object oInvoiceID, object oDocType, object oDocNo, object oDocStatus)
        {
            string strInvoiceID = Convert.ToString(oInvoiceID);
            string strDocumentType = Convert.ToString(oDocType);
            string strDocNo = Convert.ToString(oDocNo);
            string strDocStatus = Convert.ToString(oDocStatus);

            string strURL = "";

            strURL = "javascript:window.open('../../JKS/invoice/APComments.aspx?InvoiceID=" + strInvoiceID + "&DocType=" + strDocumentType + "&DocNo=" + strDocNo + "&DocStatus=" + strDocStatus + "','InvoiceStatusLogNL','width=700,height=450,scrollbars=1');";

            return (strURL);
        }
        #endregion

        #region GetAllCategoryByAdmin()
        public void GetAllCategoryByAdmin()
        {

            try
            {

                DataTable dt = (DataTable)ViewState["objDataTable"];
                DataView dvgrdInvCur = new DataView(dt);
                dvgrdInvCur.Sort = strSortField + " " + strSortOrder;
                grdInvCur.DataSource = dvgrdInvCur;
                grdInvCur.DataBind();
                grdInvCur.Visible = true;

            }
            catch (Exception EX)
            {
                Response.Write("<script>alert('ERROR : " + EX.Message.ToString() + "')</script>");
            }
        }

        #endregion

        #region Sorting
        protected void grdInvCur_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            if (strSortOrder == "ASC")
                strSortOrder = "DESC";
            else
                strSortOrder = "ASC";


            strSortField = e.SortExpression;
            GetAllCategoryByAdmin();
        }
        #endregion

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

        #region: Multiple Attachment DownloadbtnDownloadAttachment_Click
        private void btnDownloadAttachment_Click(object sender, System.EventArgs e)
        {
            DataTable dtAttachment = new DataTable();
            DataTable dtSelectedAttachment = new DataTable();
            try
            {

                if (ViewState["dtAttachment"] != null)
                {
                    dtAttachment = (DataTable)ViewState["dtAttachment"];
                    dtSelectedAttachment = dtAttachment.Clone();


                    /*
                     foreach (DataGridItem row in grdInvCur.Items)
                      {
                          if (((CheckBox)row.FindControl("chkDownload")).Checked)
                          {
                           

                              string strInvoiceID = ((Label)row.FindControl("lblInvoiceID")).Text.ToString();
                              string strDocType = ((Label)row.FindControl("lblDocType")).Text.ToString();

                              DataView dvSelectedAttachment = new DataView(dtAttachmentCheck);
                              dvSelectedAttachment.Sort = "InvoiceID ASC";
                              dvSelectedAttachment.RowFilter = "InvoiceID=" + Convert.ToInt32(strInvoiceID) + " And NewDocType='" + strDocType.ToString().Trim().ToUpper() + "'";
                              foreach (DataRow Dr in dvSelectedAttachment.ToTable().Rows)
                              {
                                  dtSelectedAttachment.ImportRow(Dr);
                              }
                          }
                      }
                     */

                    if (ViewState["dtCheckAttachment"] != null)
                    {
                        DataTable dtCheckBoxAttachment = (DataTable)ViewState["dtCheckAttachment"];

                        string strInvoiceIDs = string.Empty;
                        string strCreditNoteIDs = string.Empty;

                        if (dtCheckBoxAttachment.Rows.Count > 0)
                        {
                            DataView dvSelectedCheckBoxForInvoice = new DataView();
                            #region: Fetch InvoiceIDs
                            dvSelectedCheckBoxForInvoice = new DataView(dtCheckBoxAttachment);
                            dvSelectedCheckBoxForInvoice.Sort = "InvoiceID ASC";
                            dvSelectedCheckBoxForInvoice.RowFilter = " DocType='INV'";
                            int Counter = 0;
                            foreach (DataRow drCheckBoxForInvoice in dvSelectedCheckBoxForInvoice.ToTable().Rows)
                            {
                                Counter++;
                                // dtSelectedAttachment.ImportRow(Dr);
                                if (Counter == dvSelectedCheckBoxForInvoice.ToTable().Rows.Count)
                                {
                                    strInvoiceIDs += drCheckBoxForInvoice["InvoiceID"].ToString();
                                }
                                else
                                {
                                    strInvoiceIDs += drCheckBoxForInvoice["InvoiceID"].ToString() + " , ";

                                }
                            }
                            if (strInvoiceIDs.ToString().Trim().Length > 0)
                            {
                                DataView dvSelectedAttachmentInvoice = new DataView(dtAttachment);
                                dvSelectedAttachmentInvoice.Sort = "InvoiceID ASC";
                                dvSelectedAttachmentInvoice.RowFilter = "InvoiceID in ( " + strInvoiceIDs + " ) And NewDocType='INV'";
                                foreach (DataRow Dr in dvSelectedAttachmentInvoice.ToTable().Rows)
                                {
                                    dtSelectedAttachment.ImportRow(Dr);
                                }
                                dtSelectedAttachment.AcceptChanges();
                            }
                            #endregion

                            #region: Fetch CreditNoteIDs
                            dvSelectedCheckBoxForInvoice = new DataView(dtCheckBoxAttachment);
                            dvSelectedCheckBoxForInvoice.Sort = "InvoiceID ASC";
                            dvSelectedCheckBoxForInvoice.RowFilter = " DocType='CRN'";
                            Counter = 0;
                            foreach (DataRow drCheckBoxForInvoice in dvSelectedCheckBoxForInvoice.ToTable().Rows)
                            {
                                Counter++;
                                // dtSelectedAttachment.ImportRow(Dr);
                                if (Counter == dvSelectedCheckBoxForInvoice.ToTable().Rows.Count)
                                {
                                    strCreditNoteIDs += drCheckBoxForInvoice["InvoiceID"].ToString();
                                }
                                else
                                {
                                    strCreditNoteIDs += drCheckBoxForInvoice["InvoiceID"].ToString() + " , ";

                                }
                            }

                            if (strCreditNoteIDs.ToString().Trim().Length > 0)
                            {
                                DataView dvSelectedAttachmentCreditNote = new DataView(dtAttachment);
                                dvSelectedAttachmentCreditNote.Sort = "InvoiceID ASC";
                                dvSelectedAttachmentCreditNote.RowFilter = "InvoiceID in ( " + strCreditNoteIDs + " ) And NewDocType='CRN'";
                                foreach (DataRow Dr in dvSelectedAttachmentCreditNote.ToTable().Rows)
                                {
                                    dtSelectedAttachment.ImportRow(Dr);
                                }
                                dtSelectedAttachment.AcceptChanges();
                            }
                            #endregion
                        }

                    }
                    dtSelectedAttachment.AcceptChanges();

                    if (dtSelectedAttachment != null && dtSelectedAttachment.Rows.Count > 0)
                    {

                        GenerateDownloadFiles(dtSelectedAttachment);
                    }
                    else
                    {
                        if (ViewState["dtAttachment"] != null)
                        {
                            GenerateDownloadFiles((DataTable)ViewState["dtAttachment"]);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Ex.ToString();

            }
            finally
            {
                dtAttachment.Dispose();
                dtSelectedAttachment.Dispose();
            }


        }

        public void GenerateDownloadFiles(DataTable dtDownload)
        {

            #region: Generarate Files
            if (dtDownload != null && dtDownload.Rows.Count > 0)
            {
                DataTable dtAttachment = dtDownload;
                if (dtAttachment.Rows.Count > 0)
                {
                    string ZipFileName = "AttachmentFiles_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".zip";
                    string zipFullPath = Server.MapPath("~") + "\\Temp\\" + ZipFileName;
                    ZipOutputStream zipOut = new ZipOutputStream(File.Create(zipFullPath));
                    for (int i = 0; i < dtAttachment.Rows.Count; i++)
                    {

                        //string filePath =DownloadFile(dtAttachment.Rows[i]["ImagePath"].ToString(),dtAttachment.Rows[i]["ArchiveImagePath"].ToString());
                        string FileNameInfo = dtAttachment.Rows[i]["ImagePath"].ToString();
                        if (FileNameInfo == "" && FileNameInfo.Trim().Length <= 0)
                        {
                            FileNameInfo = dtAttachment.Rows[i]["ArchiveImagePath"].ToString();
                        }
                        int index = FileNameInfo.LastIndexOf("\\");
                        int Length = FileNameInfo.Length;
                        FileNameInfo = FileNameInfo.Substring(index + 1, (Length - index - 1));
                        string extension = Path.GetExtension(FileNameInfo);
                        FileNameInfo = dtAttachment.Rows[i]["SupplierCodeAgainstBuyer"].ToString() + "_" + dtAttachment.Rows[i]["DocType"].ToString() + "_" + dtAttachment.Rows[i]["InvoiceDate"].ToString() + "_" + dtAttachment.Rows[i]["InvoiceID"].ToString() + "_" + dtAttachment.Rows[i]["DocumentID"].ToString() + extension;//DocumentID+"_"+FileNameInfo;
                        byte[] buff = ForceDownload(dtAttachment.Rows[i]["ImagePath"].ToString(), dtAttachment.Rows[i]["ArchiveImagePath"].ToString(), 0);
                        if (buff != null)
                        {
                            //FileInfo fi = new FileInfo(filePath);
                            //ZipEntry entry = new ZipEntry(fi.Name);
                            //entry.DateTime = fi.LastWriteTime;
                            ZipEntry entry = new ZipEntry(FileNameInfo);
                            entry.DateTime = System.DateTime.Now;
                            entry.Size = buff.Length;
                            zipOut.PutNextEntry(entry);
                            zipOut.Write(buff, 0, buff.Length);
                        }


                    }
                    zipOut.Finish();

                    zipOut.Close();
                    //bool IsDownloaded=ForceDownload(zipFullPath,0);
                    DownloadZip(zipFullPath, ZipFileName);
                }
            }

            #endregion
        }
        //private void btnDownloadAttachment_Click(object sender, System.EventArgs e)
        //{
        //    if (ViewState["dtAttachment"] != null)
        //    {
        //        DataTable dtAttachment = (DataTable)ViewState["dtAttachment"];
        //        if (dtAttachment.Rows.Count > 0)
        //        {
        //            string ZipFileName = "AttachmentFiles_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".zip";
        //            string zipFullPath = Server.MapPath("~") + "\\Temp\\" + ZipFileName;
        //            ZipOutputStream zipOut = new ZipOutputStream(File.Create(zipFullPath));
        //            for (int i = 0; i < dtAttachment.Rows.Count; i++)
        //            {

        //                //string filePath =DownloadFile(dtAttachment.Rows[i]["ImagePath"].ToString(),dtAttachment.Rows[i]["ArchiveImagePath"].ToString());
        //                string FileNameInfo = dtAttachment.Rows[i]["ImagePath"].ToString();
        //                if (FileNameInfo == "" && FileNameInfo.Trim().Length <= 0)
        //                {
        //                    FileNameInfo = dtAttachment.Rows[i]["ArchiveImagePath"].ToString();
        //                }
        //                int index = FileNameInfo.LastIndexOf("\\");
        //                int Length = FileNameInfo.Length;
        //                FileNameInfo = FileNameInfo.Substring(index + 1, (Length - index - 1));
        //                string extension = Path.GetExtension(FileNameInfo);
        //                FileNameInfo = dtAttachment.Rows[i]["SupplierCodeAgainstBuyer"].ToString() + "_" + dtAttachment.Rows[i]["DocType"].ToString() + "_" + dtAttachment.Rows[i]["InvoiceDate"].ToString() + "_" + dtAttachment.Rows[i]["InvoiceID"].ToString() + "_" + dtAttachment.Rows[i]["DocumentID"].ToString() + extension;//DocumentID+"_"+FileNameInfo;
        //                byte[] buff = ForceDownload(dtAttachment.Rows[i]["ImagePath"].ToString(), dtAttachment.Rows[i]["ArchiveImagePath"].ToString(), 0);
        //                if (buff != null)
        //                {
        //                    //FileInfo fi = new FileInfo(filePath);
        //                    //ZipEntry entry = new ZipEntry(fi.Name);
        //                    //entry.DateTime = fi.LastWriteTime;
        //                    ZipEntry entry = new ZipEntry(FileNameInfo);
        //                    entry.DateTime = System.DateTime.Now;
        //                    entry.Size = buff.Length;
        //                    zipOut.PutNextEntry(entry);
        //                    zipOut.Write(buff, 0, buff.Length);
        //                }


        //            }
        //            zipOut.Finish();

        //            zipOut.Close();
        //            //bool IsDownloaded=ForceDownload(zipFullPath,0);
        //            DownloadZip(zipFullPath, ZipFileName);
        //        }
        //    }
        //}
        #endregion

        #region: Multiple Attachment Download implementation
        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }
        private string GetURL2()
        {
            return ConfigurationManager.AppSettings["ServiceURLNew"];
        }
        private string DownloadFile(string ImagePath, string ArchPath)
        {
            bool bFound = false;
            //string sDownLoadPath=((Label)e.Item.FindControl("lblHidePath")).Text;
            string sDownLoadPath = ImagePath;
            sDownLoadPath = sDownLoadPath.Replace("I:", "C:\\P2D");
            sDownLoadPath = sDownLoadPath.Replace("\\90104-server2", "C:\\P2D");
            sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
            if (sDownLoadPath.Trim() != "")
            {
                if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                {
                    try
                    {
                        if (File.Exists(sDownLoadPath))
                        {
                            bFound = true;
                        }
                        else
                        {
                            bFound = false;
                        }
                        //bFound=ForceDownload(sDownLoadPath,0);
                    }
                    catch (Exception Ex)
                    {
                        string Error = Ex.ToString();
                    }
                }
                else
                {
                    if (File.Exists(sDownLoadPath))
                    {
                        bFound = true;
                    }
                    else
                    {
                        bFound = false;
                    }
                    //bFound=ForceDownload(sDownLoadPath,0);
                }
            }
            if (bFound == false)
            {
                //sDownLoadPath=((Label)e.Item.FindControl("lblArchPath")).Text;
                sDownLoadPath = ArchPath;
                sDownLoadPath = sDownLoadPath.Replace("\\90107-server3", @"C:\File Repository");
                sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);

                if (sDownLoadPath.Trim() != "")
                {
                    if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                    {
                        try
                        {
                            if (File.Exists(sDownLoadPath))
                            {
                                bFound = true;
                            }
                            else
                            {
                                bFound = false;
                            }
                            //bFound=ForceDownload(sDownLoadPath,1);
                        }
                        catch (Exception Ex)
                        {
                            string Error = Ex.ToString();
                        }
                    }
                    else
                    {
                        //bFound=ForceDownload(sDownLoadPath,1);
                        if (File.Exists(sDownLoadPath))
                        {
                            bFound = true;
                        }
                        else
                        {
                            bFound = false;
                        }
                    }
                }
            }
            return sDownLoadPath;
        }
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
        private string GetBuyerCompanyName(int iInvoiceID, string DocType)
        {
            string strBuyerCompanyName = string.Empty;

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("GetBuyerCompanyNameAgainstInvoice", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);
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
            //DataTable dt=new DataTable();
            if (ds.Tables.Count > 0)
            {
                strBuyerCompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
            }


            return strBuyerCompanyName;


        }

        private byte[] ForceDownload(string ImagePath, string ArchPath, int iStep)
        {
            bool bRetVal = false;
            byte[] bytBytesFinal = null;
            string sFileName = string.Empty;
            if (iStep == 0)
            {
                sFileName = ImagePath;
                sFileName = sFileName.Replace("I:", "C:\\P2D");
                sFileName = sFileName.Replace("\\90104-server2", "C:\\P2D");
                sFileName = GetTrimFirstSlash(sFileName);
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = GetURL();
                    byte[] bytBytes = objService.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        bytBytesFinal = bytBytes;
                        //						Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        //						Response.ContentType = "application//octet-stream";
                        //						Response.BinaryWrite(bytBytes);
                        //						Response.Flush();
                        //						Response.End();
                        //						fs1.Close();
                        //						fs1 = null;
                        //						bRetVal=true;

                    }
                    else
                    {
                        bytBytesFinal = ForceDownload(ImagePath, ArchPath, 1);
                    }
                }
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            else if (iStep == 1)
            {
                sFileName = ArchPath;
                sFileName = sFileName.Replace("\\90107-server3", @"C:\File Repository");
                sFileName = GetTrimFirstSlash(sFileName);
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
                    objService2.Url = GetURL2();
                    byte[] bytBytes = objService2.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        bytBytesFinal = bytBytes;
                        //						Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        //						Response.ContentType = "application//octet-stream";
                        //						Response.BinaryWrite(bytBytes);
                        //						Response.Flush();
                        //						Response.End();
                        //						fs1.Close();
                        //						fs1 = null;
                        //						bRetVal=true;
                    }
                }
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            return bytBytesFinal;
        }
        public void DownloadZip(string Path, string ZipFileName)
        {
            string filepath = Path;
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/zip";
                //Context.Response.AddHeader("content-disposition","attachment; filename="+Path.GetFileName(Path));
                Context.Response.AppendHeader("content-disposition", "attachment; filename=" + ZipFileName);
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                long FileSize = fs.Length;
                byte[] getContent = new byte[(int)FileSize];
                fs.Read(getContent, 0, (int)fs.Length);
                fs.Close();
                Context.Response.BinaryWrite(getContent);
                Context.Response.Flush();
                Context.Response.End();

            }
            catch (Exception ex) { throw (ex); }

        }

        #endregion

        #region: New Implementation on 28th January 2015 Developed by Mrinal
        protected void rptAttachment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnrptAttachment = (Button)e.Item.FindControl("btnAttachment");
                btnrptAttachment.Text = "Doc" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "COUNTER"));
                btnrptAttachment.Click += new EventHandler(btnrptAttachment_Click);
            }
        }

        private DataTable GetAttachmentDetails(int iInvoiceID, string DocType)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("GetUploadFileDetailsTypeWise", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);
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

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }


            return new DataTable();


        }

        protected void rptAttachment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToUpper() == "DOW")
                {
                    int DocumentID = Convert.ToInt32(e.CommandArgument);

                    string strAttachmentImagePath = ((Label)e.Item.FindControl("lblAttachmentImagePath")).Text;
                    string strAttachmentArchiveImagePath = ((Label)e.Item.FindControl("lblAttachmentArchiveImagePath")).Text;
                }
            }
        }

        protected void btnrptAttachment_Click(object sender, EventArgs e)
        {
            Button btnrptAttachment = (Button)sender;
            RepeaterItem rptItem = (RepeaterItem)btnrptAttachment.NamingContainer;
            //string lblAttachmentImagePath = ((Label) rptItem.FindControl("lblAttachmentImagePath")).Text;
            //string strAttachmentArchiveImagePath = ((Label) rptItem.FindControl("lblAttachmentArchiveImagePath")).Text;
            string strDocumentID = ((Label)rptItem.FindControl("lblDocumentID")).Text;

            bool bFound = false;
            string sDownLoadPath = ((Label)rptItem.FindControl("lblAttachmentImagePath")).Text;
            sDownLoadPath = sDownLoadPath.Replace("I:", "C:\\P2D");
            sDownLoadPath = sDownLoadPath.Replace("\\90104-server2", "C:\\P2D");
            sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
            if (sDownLoadPath.Trim() != "")
            {
                if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
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
                sDownLoadPath = ((Label)rptItem.FindControl("lblAttachmentArchiveImagePath")).Text;
                sDownLoadPath = sDownLoadPath.Replace("\\90107-server3", @"C:\File Repository");
                sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);

                if (sDownLoadPath.Trim() != "")
                {
                    if (Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
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
                    if (bytBytes != null)
                    {
                        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        Response.ContentType = "application//octet-stream";
                        Response.BinaryWrite(bytBytes);
                        Response.Flush();
                        Response.End();
                        fs1.Close();
                        fs1 = null;
                        bRetVal = true;
                    }
                }
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
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
                    if (bytBytes != null)
                    {
                        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(sFileName));
                        Response.ContentType = "application//octet-stream";
                        Response.BinaryWrite(bytBytes);
                        Response.Flush();
                        Response.End();
                        fs1.Close();
                        fs1 = null;
                        bRetVal = true;
                    }
                }
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            return bRetVal;
        }

        #endregion

        [WebMethod]
        public static string[] GetSupplier(string CompanyID, string userId, string userTypeID, string UserString)
        {
            DataSet dsSupplier = new DataSet();
            Invoice_New objInv = new Invoice_New();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            //SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetSuppliersList_GRH", sqlConn);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetSuppliersList2_JKS", sqlConn);

            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            sqlDA.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(userId));
            sqlDA.SelectCommand.Parameters.Add("@USerTypeID", Convert.ToInt32(userTypeID));
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
                    // myList.Add(string.Format("{0}", row["label"].ToString()));

                }
                return myList.ToArray();
                // return JsonConvert.SerializeObject(dsSupplier.Tables[0]);
            }
            else
                return null;
            // return "";
        }

        [WebMethod]
        public static List<string> FetchInvoiceNo(string CompanyID, string DocType, string UserString)
        {
            DataSet dsInvoiceNo = new DataSet();


            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_FetchInvoiceNo_GRH ", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(CompanyID));
            if (DocType != "")
                sqlDA.SelectCommand.Parameters.Add("@DocType", DocType);
            else
                sqlDA.SelectCommand.Parameters.Add("@DocType", DBNull.Value);
            sqlDA.SelectCommand.Parameters.Add("@InvoiceString", UserString);
            sqlDA.SelectCommand.CommandTimeout = 0;
            try
            {
                sqlConn.Open();
                sqlDA.Fill(dsInvoiceNo, "InvoiceNo");

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
            if (dsInvoiceNo != null && dsInvoiceNo.Tables.Count > 0 && dsInvoiceNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsInvoiceNo.Tables[0].Rows)
                {
                    //===========Added BY Rimi on 27th August 2015============
                    if (row["InvoiceNo"].ToString().Contains("&#38"))
                    {
                        row["InvoiceNo"] = row["InvoiceNo"].ToString().Replace("#38", "");
                    }
                    //===========Added BY Rimi on 27th August 2015 End============
                    myList.Add(row["InvoiceNo"].ToString());
                }
            }
            return myList;
        }

        [WebMethod]
        public static string[] GetNominalName(string CompanyID, string UserString)
        {
            DataSet dsNominalName = new DataSet();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_GetNominalName", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@CompanyID", Convert.ToInt32(CompanyID));
            sqlDA.SelectCommand.Parameters.Add("@NominalStr", UserString);

            try
            {
                sqlConn.Open();
                sqlDA.Fill(dsNominalName);
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
            if (dsNominalName != null && dsNominalName.Tables.Count > 0 && dsNominalName.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsNominalName.Tables[0].Rows)
                {
                    myList.Add(string.Format("{0}#{1}", row["NominalCodeID"].ToString(), row["label"].ToString()));
                    // myList.Add(string.Format("{0}", row["label"].ToString()));
                }
                return myList.ToArray();
                // return JsonConvert.SerializeObject(dsSupplier.Tables[0]);
            }
            else
                return null;

        }

        #region: Checkbox Events
        public void chkDownload_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkDownload = (CheckBox)sender;
            DataGridItem grdItem = chkDownload.NamingContainer as DataGridItem;

            DataTable dtCheckAttachment = new DataTable();
            if (ViewState["dtCheckAttachment"] != null)
            {
                dtCheckAttachment = (DataTable)ViewState["dtCheckAttachment"];
            }
            else
            {
                dtCheckAttachment.Columns.Add("InvoiceID");
                dtCheckAttachment.Columns.Add("DocType");
                dtCheckAttachment.AcceptChanges();
            }

            string strInvoiceID = ((Label)grdItem.FindControl("lblInvoiceID")).Text.ToString();
            string strDocType = ((Label)grdItem.FindControl("lblDocType")).Text.ToString();
            if (chkDownload.Checked)
            {
                DataRow dr = dtCheckAttachment.NewRow();

                dr["InvoiceID"] = strInvoiceID;
                dr["DocType"] = strDocType;
                dtCheckAttachment.Rows.Add(dr);
            }
            else
            {
                foreach (DataRow RowsItem in dtCheckAttachment.Rows)
                {
                    //  bool IsMatched = false;
                    if (RowsItem["InvoiceID"].ToString() == strInvoiceID && RowsItem["DocType"].ToString() == strDocType)
                    {
                        dtCheckAttachment.Rows.Remove(RowsItem);
                        break;
                    }
                }
                dtCheckAttachment.AcceptChanges();

            }
            ViewState["dtCheckAttachment"] = dtCheckAttachment;
            iNeedRefreshToBottom = 1;
        }
        #endregion

        #region Modified by Koushik Das on 22-June-2017
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="SupplierName"></param>
        /// <param name="SupplierID"></param>
        /// <param name="SupplierNameHD"></param>
        /// <param name="SupplierStatus"></param>
        /// <param name="DocStatusHD"></param>
        /// <param name="isSupChecked"></param>
        /// <param name="VendorClass"></param>
        /// <param name="DocType"></param>
        /// <param name="DocStatus"></param>
        /// <param name="InvoiceNo"></param>
        /// <param name="InvoiceID"></param>
        /// <param name="InvoiceNoHD"></param>
        /// <param name="isInvoice"></param>
        /// <param name="PONo"></param>
        /// <param name="BusinessUnit"></param>
        /// <param name="Department"></param>
        /// <param name="Nominal"></param>
        /// <param name="NominalCodeId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="NetFrom"></param>
        /// <param name="NetTo"></param>
        private void StoreEditSession(string CompanyID, string SupplierName, string SupplierID, string SupplierNameHD, string SupplierStatus, string DocStatusHD, string isSupChecked, string VendorClass, string DocType, string DocStatus, string InvoiceNo, string InvoiceID, string InvoiceNoHD, string isInvoice, string PONo, string BusinessUnit, string Department, string Nominal, string NominalCodeId, string FromDate, string ToDate, string NetFrom, string NetTo)
        {
            Dictionary<string, dynamic> HistoryEditReturnInfo = new Dictionary<string, dynamic>();

            HistoryEditReturnInfo.Add("CompanyID", CompanyID);
            HistoryEditReturnInfo.Add("SupplierName", SupplierName);
            HistoryEditReturnInfo.Add("SupplierID", SupplierID);
            HistoryEditReturnInfo.Add("SupplierNameHD", SupplierNameHD);
            HistoryEditReturnInfo.Add("SupplierStatus", SupplierStatus);
            HistoryEditReturnInfo.Add("DocStatusHD", DocStatusHD);
            HistoryEditReturnInfo.Add("isSupChecked", isSupChecked);
            HistoryEditReturnInfo.Add("VendorClass", VendorClass);
            HistoryEditReturnInfo.Add("DocType", DocType);
            HistoryEditReturnInfo.Add("DocStatus", DocStatus);
            HistoryEditReturnInfo.Add("InvoiceNo", InvoiceNo);
            HistoryEditReturnInfo.Add("InvoiceID", InvoiceID);
            HistoryEditReturnInfo.Add("InvoiceNoHD", InvoiceNoHD);
            HistoryEditReturnInfo.Add("isInvoice", isInvoice);
            HistoryEditReturnInfo.Add("PONo", PONo);
            HistoryEditReturnInfo.Add("BusinessUnit", BusinessUnit);
            HistoryEditReturnInfo.Add("Department", Department);
            HistoryEditReturnInfo.Add("Nominal", Nominal);
            HistoryEditReturnInfo.Add("NominalCodeId", NominalCodeId);
            HistoryEditReturnInfo.Add("FromDate", FromDate);
            HistoryEditReturnInfo.Add("ToDate", ToDate);
            HistoryEditReturnInfo.Add("NetFrom", NetFrom);
            HistoryEditReturnInfo.Add("NetTo", NetTo);

            Session["HistoryEditReturnInfo"] = HistoryEditReturnInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ApplySessionWhenReturnedFromEditPage()
        {
            if (Request.UrlReferrer != null && Session["HistoryEditReturnInfo"] != null)
            {
                string referal = Path.GetFileName(Request.UrlReferrer.AbsolutePath).ToLower();
                if (referal == "invoiceedit.aspx" || referal == "invoiceedit_cn.aspx" || referal == "history.aspx")
                {
                    Dictionary<string, dynamic> HistoryEditReturnInfo = (Dictionary<string, dynamic>)Session["HistoryEditReturnInfo"];

                    ddlCompany.SelectedValue = HistoryEditReturnInfo["CompanyID"];
                    ddlCompany_SelectedIndexChanged(ddlCompany, new EventArgs());
                    txtSupplier.Value = HistoryEditReturnInfo["SupplierName"];
                    HdSupplierId.Value = HistoryEditReturnInfo["SupplierID"];
                    HdSupplierName.Value = HistoryEditReturnInfo["SupplierNameHD"];
                    HdSupplierStatus.Value = HistoryEditReturnInfo["SupplierStatus"];
                    HdDocStatus.Value = HistoryEditReturnInfo["DocStatusHD"];
                    cbSupplier.Value = HistoryEditReturnInfo["isSupChecked"];
                    ddlVendorClass.SelectedValue = HistoryEditReturnInfo["VendorClass"];
                    //Commented By Mainak, 2018-11-06
                    //ddlDocType.SelectedValue = HistoryEditReturnInfo["DocType"];
                    ddlDocStatus.SelectedValue = HistoryEditReturnInfo["DocStatus"];
                    txtInvoiceNo.Text = HistoryEditReturnInfo["InvoiceNo"];
                    hdInvoiceNo.Value = HistoryEditReturnInfo["InvoiceID"];
                    hdInvoiceNoTxt.Value = HistoryEditReturnInfo["InvoiceNoHD"];
                    cbInvoiceNo.Value = HistoryEditReturnInfo["isInvoice"];
                    txtPONo.Text = HistoryEditReturnInfo["PONo"];
                    ddlBusinessUnit.SelectedValue = HistoryEditReturnInfo["BusinessUnit"];
                    ddldept.SelectedValue = HistoryEditReturnInfo["Department"];
                    txtNominal.Value = HistoryEditReturnInfo["Nominal"];
                    hdNominalCodeId.Value = HistoryEditReturnInfo["NominalCodeId"];
                    txtFromDate.Value = HistoryEditReturnInfo["FromDate"];
                    txtToDate.Value = HistoryEditReturnInfo["ToDate"];
                    textRange1.Text = HistoryEditReturnInfo["NetFrom"];
                    textRange2.Text = HistoryEditReturnInfo["NetTo"];

                    btnSearch_Click(btnSearch, new EventArgs());

                    //--start-- added by Koushik Das as on 22-June-2017
                    try
                    {
                        if (Session["PageIndex"] != null)
                        {
                            grdInvCur.CurrentPageIndex = Convert.ToInt32(Session["PageIndex"]);

                            DataTable dtbl = (DataTable)Session["ViewstateobjDataTable"];

                            grdInvCur.DataSource = dtbl;
                            grdInvCur.DataBind();
                        }
                    }
                    catch { }
                    //--end-- added by Koushik Das as on 22-June-2017
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public static void SetPageIndex(int val)
        {
            HttpContext.Current.Session["PageIndex"] = val;
        }
        #endregion

        //Added By Mainak 2018-11-29
        public int CallProc(string xmldoc)
        {
            int rows = 0;
            try
            {

                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("usp_UpdateStatusPaid_JKS", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                //Calling the procedure.
                cmd.Parameters.AddWithValue("@xmldoc", xmldoc);
                rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    sqlConn.Close();
                    string script = "<script>alert('Status updated successfully')</script>";
                    //Adding client side script to the page.
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Updated", script);

                }

            }
            catch (Exception e)
            {
                Response.Write(e.Message);
            }
            return rows;
        }
        [WebMethod]
        //Added By Mainak 2018-11-29
        protected void btnMarckPaid_Click(object sender, EventArgs e)
        {
            string xmlData;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            //Adding columns to the datatable.
            dt.Columns.Add("strInvoiceID", typeof(int));
            dt.Columns.Add("userId", typeof(string));
            dt.Columns.Add("doctype", typeof(string));
            string strInvoiceID, strdoctype;

            DataTable dtCheckAttachment = new DataTable();
            dtCheckAttachment = (DataTable)ViewState["dtCheckAttachment"];

            for (int i = 0; i < dtCheckAttachment.Rows.Count; i++)
            {
                strInvoiceID = dtCheckAttachment.Rows[i]["invoiceid"].ToString();
                strdoctype = dtCheckAttachment.Rows[i]["doctype"].ToString();
                DataRow dr = dt.NewRow();
                dr["strInvoiceID"] = strInvoiceID;
                dr["userId"] = Session["UserID"].ToString();
                dr["doctype"] = strdoctype;
                dt.Rows.Add(dr);

            }

            if (dt.Rows.Count > 0)
            {
                ds.Tables.Add(dt);
                xmlData = ds.GetXml();
                CallProc(xmlData);
                LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), GetDocType());
            }
        }
        //Added By Mainak 2018-11-29
        protected void btnMarckPaidNoneSelected_Click(object sender, EventArgs e)
        {
            string xmlData;

            DataSet ds2 = new DataSet();
            DataTable dt2 = new DataTable();
            //Adding columns to the datatable.
            dt2.Columns.Add("strInvoiceID", typeof(int));
            dt2.Columns.Add("userId", typeof(string));
            dt2.Columns.Add("doctype", typeof(string));

            DataTable dttest = new DataTable();
            dttest = (DataTable)Session["ViewstateobjDataTable"];

            if (dttest.Rows.Count > 0)
            {
                for (int i = 0; i < dttest.Rows.Count; i++)
                {
                    DataRow dr = dt2.NewRow();
                    dr["strInvoiceID"] = dttest.Rows[i]["InvoiceID"];
                    dr["userId"] = Session["UserID"].ToString();
                    dr["doctype"] = dttest.Rows[i]["newdoctype"]; ;
                    dt2.Rows.Add(dr);
                }


            }
            if (dt2.Rows.Count > 0)
            {
                ds2.Tables.Add(dt2);
                xmlData = ds2.GetXml();
                CallProc(xmlData);
                LoadData(Convert.ToInt32(ddlCompany.SelectedValue.Trim()), GetDocType());
            }

        }
    }
}
