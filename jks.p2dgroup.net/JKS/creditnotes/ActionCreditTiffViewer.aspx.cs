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


namespace JKS
{
    [ScriptService]
    public partial class ActionCreditTiffViewer : System.Web.UI.Page
    {

        #region:  Objects Declarations
        protected CBSolutions.ETH.Web.Invoice objInvoice = new CBSolutions.ETH.Web.Invoice();
        protected Company objCompany = new Company();
        Invoice_NL_CN objCN = new Invoice_NL_CN();
        //private ETC.User.Users objUser = new ETC.User.Users();
        private JKS.Users objUser = new JKS.Users();
        #endregion

        #region  variables
        protected string AuthorisationStringToolTips = "";
        //Invoice.Invoice objinvoice = new Invoice.Invoice();
        JKS.Invoice objinvoice = new JKS.Invoice();
        public int invoiceID = 0;
        protected int iApproverStatusID = 0;
        protected int iCurrentStatusID = 0;
        protected int TypeUser = 1;
        protected int UserTypeID = 1;
        protected int StatusUpdate = 0;
        protected int DocStatus = 0;
        protected string DocType = "INV";
        protected double dTotalAmount = 0;
        double dNetAmt = 0;
        double dCodingVat = 0;
        string strComments = "";

        //===============Added By Rimi on 25th Nov 2015=========================
        protected int RejectOpenFields = 0;
        protected int ReopenAtApprover = 0;
        protected string G2App = "";
        string sdApprover1 = "";
        string sdApprover2 = "";
        string sdApprover3 = "";
        string sdApprover4 = "";
        string sdApprover5 = "";

        public int vRFlag = 0;//Added by Mainak 2018-09-10

        //===============Added By Rimi on 25th Nov 2015 End====================
        #endregion
        // added by kd
        public int ChkUserID = 0;
        #region Sql Variables
        public string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        SqlConnection sqlConn;
        protected SqlDataAdapter sqlDA = null;//Added Mainak 2018-08-10
        #endregion
        CBSolutions.ETH.Web.ETC.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.ETC.ApprovalPath.Approval();
        bool test = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["test"].ToString());
        #region Page Events
        private void Page_Load(object sender, System.EventArgs e)
        {
            //Added by Mainak 2018-08-16
            overlay1.Visible = false;
            dialog1.Visible = false;
            overlayApprove.Visible = false;
            dialogApprove.Visible = false;
            //=============Added on Rimi on 22nd July 2015============================
            if (Request.QueryString["MsgFlag"] == "1")
            {
                if (Request.QueryString["MSG"] == "Reject")
                {
                    // Request.QueryString["MSG"]="";
                    string message = "alert('Invoice Rejected Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                if (Request.QueryString["MSG"] == "Approve")
                {
                    // Request.QueryString["MSG"]="";
                    string message = "alert('Invoice Approved Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }

                if (Request.QueryString["MSG"] == "Open")
                {
                    //Request.QueryString["MSG"]="";
                    string message = "alert('Invoice passed to user successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }

                if (Request.QueryString["MSG"] == "ReOpen")
                {
                    // Request.QueryString["MSG"]="";
                    string message = "alert('Invoice Reopened Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                if (Request.QueryString["MSG"] == "Delete")
                {
                    // Request.QueryString["MSG"]="";
                    string message = "alert('Invoice Deleted Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                System.Reflection.PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                isreadonly.SetValue(this.Request.QueryString, false, null);

                this.Request.QueryString.Remove("MSG");
                this.Request.QueryString.Remove("MsgFlag");
            }
            //=============Added on Rimi on 22nd July 2015 End============================

            Session["PageRedirect"] = "yes"; // Added By Rimi On 17th July 2015
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            //  btnCancel.Attributes.Add("onclick", "javascript:windowclose();");
            // btnCancel.Attributes.Add("onclick", "javascript:window.close();");
            //btnOpen.Attributes.Add("onclick", "javascript:return CheckOpenValid();");// Commented By Rimi on 23rd July 2015
            if (Request["DDCompanyID"] != null)
            {
                ViewState["DDCompanyID"] = Request["DDCompanyID"].ToString();
            }
            //else
            //{
            //    ViewState["DDCompanyID"] = Session["DDCompanyID"].ToString();
            //}
            if (Request["DocType"] != null)
                ViewState["DocType"] = Request["DocType"].ToString();
            else
                ViewState["DocType"] = "CRN";

            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
                //added by kuntalkarar on 12thJanuary2017
                Session["CreditnoteId_GoToStockQC"] = invoiceID;
                //----------------------------------------
                ViewState["InvoiceID"] = invoiceID.ToString();
                //blocked by kuntalkarar on 1stMarch2016 for test
                Session["eInvoiceID"] = invoiceID.ToString();
            }
            TypeUser = Convert.ToInt32(Session["UserTypeID"]);
            Session["det"] = "1";
            //Added By Kd 06.12.2018
            JKS.Invoice objInvoice = new JKS.Invoice();
            ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            dgSalesCallDetails_CRN.CurrentPageIndex = 0;
            
            if (!Page.IsPostBack)
            {
                //changed by kuntal karar on 21stOctober2016- Sessions created here
                Session["NextInvoiceID"] = "0";
                Session["NextBuyerCompanyID"] = "0";
                LoadDownloadFiles();
                //===============Added By Subhrajyoti on 3rd August 2015===========================
                //DataSet dss = GetDocumentDetails(Convert.ToInt32(Session["eInvoiceID"]), "CRN");
                //Int32 StatusId = Convert.ToInt32(dss.Tables[0].Rows[0]["StatusId"]);

                //Added by kuntalkarar on 1stMarch2016 for test
                //Session["eInvoiceID_Reopenbtn"] = invoiceID.ToString();

                Int32 StatusId = 0;
                DataSet dss = new DataSet();
                if (Session["invcren"] != null && Session["invcren"] != "0")
                {
                    dss = GetDocumentDetails(Convert.ToInt32(Session["invcren"]), "CRN");

                }
                else
                {
                    dss = GetDocumentDetails(Convert.ToInt32(Session["eInvoiceID"]), "CRN");
                }

                StatusId = Convert.ToInt32(dss.Tables[0].Rows[0]["StatusId"]);

                if (Convert.ToInt32(StatusId) != 20 && Convert.ToInt32(StatusId) != 21 && Convert.ToInt32(StatusId) != 22 && Convert.ToInt32(StatusId) != 6)
                {
                    Response.Redirect("../Current/CRNclosewindows.aspx");
                }

                //===============Added By Subhrajyoti on 3rd August 2015===========================
                //if (Session["invcren"] != "")// Commeneted By Rimi on 5th August 2015
                if (Session["invcren"] != null && Convert.ToString(Session["invcren"].ToString()) != "0" && Convert.ToString(Session["invcren"].ToString()) != "")// Added By Rimi on 5th August 2015
                {
                    PopulateDropDowns();
                    MoveToInvoice(Convert.ToInt32(Session["invcren"]));
                    ViewState["InvoiceID"] = Convert.ToInt32(Session["invcren"]);

                    //26-06-20156
                    ViewState["Counter"] = Session["IndexforCRN"];
                    //26-06-20156
                }

                else
                {



                    PopulateDropDowns();
                    ViewState["CheckList"] = 0;
                    // Added by Mrinal on 8th January 2015
                    Session["IsProcessed"] = null;
                    // Added by Mrinal on 10th November 2014
                    dNetAmt = 0;

                    ViewState["approvalpath"] = "";
                    if (invoiceID != 0)
                    {
                        GetDocumentDetails(invoiceID);
                        /*
                        string strStatusLogLink = GetInvoiceStatusLog();
                        iframeInvoiceStatusLog.Attributes.Add("src", strStatusLogLink);
                     //   aInvoiceStatusLog.Attributes.Add("onclick", GetInvoiceStatusLog());
                         * 
                         */




                        DataSet ds = GetDocumentDetails(invoiceID, "CRN");
                        Boolean Duplicate = Convert.ToBoolean(ds.Tables[0].Rows[0]["Duplicate"]);
                        if (Duplicate == false)
                        {
                            lblDuplicate.Visible = false;
                        }
                        else
                        {
                            lblDuplicate.Visible = true;
                        }









                        string strStatusLogLink = GetInvoiceStatusLog();
                        strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:530,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                        //aInvoiceStatusLog.Attributes.Add("onclick", strStatusLogLink);

                        //  InvoiceCrnIsDuplicate();
                        IsAutorisedtoEditData();
                    }
                    GetVatAmount();
                    CalculateTotal();
                    // GetDepartMentDropDwons();
                    CheckInvoiceExist();


                    if (TypeUser >= 2 && Convert.ToInt32(ViewState["StatusID"]) == 20)
                        lblDepartment.Visible = false;
                    //modified by Subhrajyoti on 27.03.2015
                    if (TypeUser < 1)
                    {
                        tbcreditnoteno.Visible = false;
                        btnEditAssociatedInvoiceNo.Visible = false;
                    }
                    //modified by Subhrajyoti on 27.03.2015
                    ShowFiles(Convert.ToInt32(Session["eInvoiceID"]));
                    ButtonRejectVisibility();
                    string TiffUrl = "../../TiffViewerDefault.aspx?ID=" + invoiceID.ToString() + "&Type=" + "CRN";
                    TiffWindow.Attributes.Add("src", TiffUrl);



                }

            }


            //if (Request.QueryString["NewVendorClass"] != null)
            //{
            //    if (Convert.ToString(Request.QueryString["NewVendorClass"]).Trim() != "PO")
            //    {
            //        //lnkVariance.Visible = false;
            //        btnRematch.Visible = true;//Added by Mainak 2018-05-24
            //    }
            //}

        }
        //-------------Addiontion for PO-LINK implementation by KuntalKarar on 20thOctober2016----------------------
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

            if (Session["button_clicked_Creditenote"] == "1")// || ViewState["button_Reopened"] == "1")
            {
                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "=====================================================================================");
                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS-->>LoadDownloadFiles()-->>Session[button_clicked_Creditenote] == 1");
                InvoiceID = Session["NextInvoiceID"].ToString().Trim();
                CompanyID = Session["NextBuyerCompanyID"].ToString().Trim();
                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>CreditNoteID :- " + InvoiceID);
                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>CompanyID :- " + CompanyID);
                Session["button_clicked_Creditenote"] = "2";
                // ViewState["button_Reopened"] = "2";
            }
            else
            {
                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "=====================================================================================");
                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS-->>LoadDownloadFiles()-->>button_clicked_Creditenote=1 ELSE");
                InvoiceID = Request.QueryString["InvoiceID"].ToString().Trim();
                CompanyID = Request.QueryString["DDCompanyID"].ToString().Trim();
                //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>CreditNoteID :- " + InvoiceID);
                // ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>CompanyID :- " + CompanyID);

            }

            //-------------------------------------------------------------------------------

            string CompanyName = "";

            try
            {
                //Modified for Normal user where no company selected from DROPDOWN by kuntalkarar on 20thOctober2016
                CompanyName = ReturnParentCompanyNameBySubCompanyID(Convert.ToInt64(CompanyID));//"JKS GROUP";//
            }
            catch
            {
                CompanyName = "Urban Leisure GROUP";
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
                    //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>if (tf) :- ");
                    //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>if (tf)_FileName " + fileName);
                    //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "JKS_LoadDownloadFiles()-->>if (tf)_FilePath " + filePath);
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
        #endregion
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
                    string qry = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " +
                                "(SELECT [ParentCompanyID] FROM [Company] WHERE [CompanyID] = @CompanyID);";

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
        //-------End for Addiontion for PO-LINK implementation by KuntalKarar on 20thOctober2016



        //Added By Kd 06.12.2018
        protected void Popup_Click(object sender, EventArgs e)
        {
            string strInvoiceID = "";
            if (Session["NewInvoiceId"] != null)
            {
                strInvoiceID = Session["NewInvoiceId"].ToString(); // modified by kd on 04/01/2019
            }
            if (strInvoiceID !="")
            {
                mpe.Show();
                this.GetInvoiceStatusDetails_CRN(Convert.ToInt32(strInvoiceID));
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
            this.GetInvoiceStatusDetails_CRN(Convert.ToInt32(invoiceID));
            }


        }



       

        //Added By Kd 06.12.2018
        #region GetInvoiceStatusDetails_CRN
       // private int ChkUserID = 0;
        private DataTable dtbl = new DataTable();
        private void GetInvoiceStatusDetails_CRN(int iInvoiceID)
        {

            lblauthstring.Text = "";
            lblDepartment.Text = "";
            lblBusinessUnit.Text = "";


            JKS.Invoice objInvoice = new JKS.Invoice();
            //lblauthstring.Text = objInvoice.GetAuthorisationString(iInvoiceID, "CRN");
            //lblDepartment.Text = objInvoice.GetDepartment(iInvoiceID, "CRN");
            //lblBusinessUnit.Text = objInvoice.GetBusinessUnitName(iInvoiceID, "CRN");

           // ChkUserID = objInvoice.GetCheckUserType(Convert.ToInt32(Session["UserID"]));
            if (ChkUserID == 1)
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            else
                dtbl = objInvoice.GetCrediNoteLogStatusApproverWise(iInvoiceID);
            //	dtbl = objInvoice.GetInvoiceLogStatusApproverWiseForSupplier_CN(iInvoiceID);


            if (dtbl.Rows.Count > 0)
            {

                //dgSalesCallDetails.Visible = false;

                dgSalesCallDetails_CRN.Visible = true;
                dgSalesCallDetails_CRN.DataSource = dtbl;
                dgSalesCallDetails_CRN.DataBind();
                lblauthstring.Text = dtbl.Rows[0]["AuthorisationString"].ToString();
                lblDepartment.Text = dtbl.Rows[0]["DepartmentName"].ToString();
                lblBusinessUnit.Text = dtbl.Rows[0]["BusinessUnitName"].ToString();
            }
            else
            {
                dgSalesCallDetails_CRN.Visible = false;
                //lblMessage.Text = "Sorry, this document has no previous actions.";
            }
        }
        #endregion



        //Added By Kd 06.12.2018
        protected void dgSalesCallDetails_PageIndexChanged2(object source, DataGridPageChangedEventArgs e)
        {
            if (e.NewPageIndex < dgSalesCallDetails_CRN.PageCount)
            {
                mpe.Show();
                this.dgSalesCallDetails_CRN.CurrentPageIndex = e.NewPageIndex;
            }
            else
            {
                mpe.Show();
                this.dgSalesCallDetails_CRN.CurrentPageIndex = dgSalesCallDetails_CRN.PageCount;
            }
            GetInvoiceStatusDetails_CRN(Convert.ToInt32(ViewState["InvoiceID"]));
        }
        


        //==============Added By Rimi on 25th Nov 2015================
        private void PopulateDropDowns()
        {
            GetApproverDropDowns();
            GetDepartMentDropDwons();

        }

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

        //Added by kuntalkarar on 10thMarch2016----------------------------
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int dept = 0;
            if (Convert.ToString(DropDownList1.SelectedValue) != "Select")
                dept = Convert.ToInt32(DropDownList1.SelectedValue);
            GetApproverDropDownsAgainstDepartment(dept);
            string strCtrl = DropDownList1.ClientID;
            setFocus(strCtrl);
        }
        //------------------------------------------------------------------
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

        #region GetApproverDropDownsAgainstDepartment(int iDeptID)
        public void GetApproverDropDownsAgainstDepartment(int iDeptID)
        {

            DataSet dsDDL = new DataSet();
            DataSet dsDDL1 = new DataSet();
            if (TypeUser == 2 || TypeUser == 3)
            {
                dsDDL = objApproval.GetApproverDropDownsByDepartment(System.Convert.ToInt32(Session["InvoiceBuyerCompany"]), iDeptID);
                dsDDL1 = objApproval.Get2ndApproverDropDownAkkeronETC1(System.Convert.ToInt32(Session["InvoiceBuyerCompany"]), iDeptID);
                if (dsDDL.Tables[0].Rows.Count > 0)
                {
                    ddlApprover1.ClearSelection();
                    ddlApprover1.Items.Clear();
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
                    ddlApprover2.Items.Clear();
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
                    ddlApprover1.Items.Clear();
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
                    ddlApprover2.Items.Clear();
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
                    if (arrApprover[0].ToString() != null && arrApprover[0].ToString() != "" && arrApprover[0].ToString() != "NULL")
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
                    if (arrApprover[1].ToString() != null && arrApprover[1].ToString() != "" && arrApprover[1].ToString() != "NULL")
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
                    ddlApprover1.ClearSelection();
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
                ViewState["PopulateDrop"] = "1";
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
        }

        #endregion

        #region setFocus(string strCtrl)

        protected void setFocus(string strCtrl)
        {
            string sScript = "<SCRIPT language='javascript'>document.getElementById('" + strCtrl + "').focus(); </SCRIPT>";
            Page.RegisterStartupScript("Focus", sScript);
        }
        #endregion

        #region UpdateDepartmentAgainstInvoiceID()
        private int UpdateDepartmentAgainstInvoiceID()
        {
            int DeptID = 0;
            string sSql = "";
            if (Convert.ToInt32(DropDownList1.SelectedIndex) > 0)
                DeptID = Convert.ToInt32(DropDownList1.SelectedValue);


            int iretval = 0;
            if (DeptID > 0)
            {
                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {

                    //blocked by kuntalkarar on 8thMarch2016
                    //sSql = "UPDATE CreditNote SET DepartmentID =" + DeptID + "  WHERE CreditNoteID =" + Convert.ToInt32(Session["eInvoiceID"]);
                    //Added by kuntalkarar on 8thMarch2016
                    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                    {

                        sSql = "UPDATE CreditNote SET DepartmentID =" + DeptID + "  WHERE CreditNoteID =" + Convert.ToInt32(Session["eInvoiceID"]);
                    }
                    else
                    {
                        sSql = "UPDATE CreditNote SET DepartmentID =" + DeptID + "  WHERE CreditNoteID =" + Convert.ToInt32(ViewState["CheckList"]);
                    }

                }
                else
                {
                    sSql = "UPDATE CreditNote SET DepartmentID =" + DeptID + "  WHERE CreditNoteID =" + Convert.ToInt32(ViewState["InvoiceChecking"]);

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

        public void ReopenExecuted_Successfully()
        {

        }

        //==============Added By Rimi on 25th Nov 2015 End================


        protected void Page_PreRender(object sender, EventArgs e)
        {


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
            this.btnEditAssociatedInvoiceNo.Click += new System.EventHandler(this.btnEditAssociatedInvoiceNo_Click);
            // this.grdList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdList_ItemDataBound);
            this.grdList.ItemDataBound += new RepeaterItemEventHandler(grdList_ItemDataBound);
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            this.btnDelLine.Click += new System.EventHandler(this.btnDelLine_Click);
            this.btnSaveLine.Click += new EventHandler(this.btnSaveLine_Click);
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            this.grdFile.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdFile_ItemCommand);
            this.grdFile.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdFile_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);


        }
        #endregion


        //private void GetDocumentDetails(int iinvoiceID)
        //{
        //    DataSet DsInv = new DataSet();
        //    DsInv = GetDocumentDetails(iinvoiceID, "CRN");
        //    if (DsInv != null)
        //    {
        //        if (DsInv.Tables.Count > 0)
        //        {
        //            if (DsInv.Tables[0].Rows.Count > 0)
        //            {
        //                lblRefernce.Text = DsInv.Tables[0].Rows[0]["InvoiceNo"].ToString();
        //                lblInvoiceDate.Text = DsInv.Tables[0].Rows[0]["InvoiceDate"].ToString();
        //                lblSupplier.Text = DsInv.Tables[0].Rows[0]["SupplierCompanyName"].ToString();
        //                lblBuyer.Text = DsInv.Tables[0].Rows[0]["BuyerCompanyName"].ToString();

        //                ViewState["SupplierCompanyID"] = DsInv.Tables[0].Rows[0]["SupplierCompanyID"].ToString();
        //                ViewState["BuyerCompanyID"] = DsInv.Tables[0].Rows[0]["BuyerCompanyID"].ToString();

        //                Session["BuyerCID"] = DsInv.Tables[0].Rows[0]["BuyerCompanyID"].ToString();

        //                lblCurrentStatus.Text = DsInv.Tables[0].Rows[0]["Status"].ToString();
        //                lblBusinessUnit.Text = Convert.ToString(DsInv.Tables[0].Rows[0]["BusinessUnit"]);
        //                try
        //                {
        //                    ViewState["approvalpath"] = DsInv.Tables[0].Rows[0]["ApprovalPath"].ToString();


        //                    lblcreditnoteno.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString(); ;
        //                    tbcreditnoteno.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString();
        //                    ViewState["AssociatedStatus"] = DsInv.Tables[0].Rows[0]["AssociatedStatus"].ToString();

        //                }
        //                catch { }

        //                try
        //                {
        //                    lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();
        //                    ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();
        //                }
        //                catch { }
        //                ViewState["StatusID"] = DsInv.Tables[0].Rows[0]["StatusID"].ToString();
        //                iCurrentStatusID = Convert.ToInt32(DsInv.Tables[0].Rows[0]["StatusID"]);
        //                ViewState["OriginalNetAmount"] = DsInv.Tables[0].Rows[0]["Net"].ToString();
        //                Session["eDocType"] = DsInv.Tables[0].Rows[0]["DocType"].ToString();
        //            }
        //        }
        //    }
        //}

        //Added by Kuntalkarar on 29thFeb2016-----------------------------
        public void GetApprovalPathsForCreditnote(int InvoiceID, string Type)
        {

            JKS.Invoice objInvoice = new JKS.Invoice();
            ViewState["approvalpath"] = objInvoice.GetAuthorisationString(InvoiceID, "CRN").Trim();

        }
        //----------------------------------------------------------------
        public void ErrorLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName, true);
            sw.WriteLine(DateTime.Now + ": " + sErrMsg);
            sw.Flush();
            sw.Close();
        }





        //Added by Kuntalkarar on 29thFeb2016-----------------------------
        public int checkDepatmentIdMismatchOrNot(int creditnoteId)
        {

            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_IsDepartmentMissmatchOrNot", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@creditNoteId", creditnoteId);
            //if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            //{

            //    sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            //}
            //else
            //{
            //    sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["CheckList"]));
            //}


            sqlOutputParam = sqlCmd.Parameters.Add("@IsExists", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlOutputParam.Value);
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(iReturnValue));
                ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(creditnoteId));
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
        //----------------------------------------------------------------


        #region GetDocumentDetails(int iinvoiceID)
        private void GetDocumentDetails(int iinvoiceID)
        {
            //Start: added by koushik das as on 18Dec2017
            Session["AttInvoice"] = iinvoiceID;
            Session["eInvoiceID"] = iinvoiceID;
            //End: added by koushik das as on 18Dec2017

            //added by kuntalkarar on 14thMarch2016
            int IsDepartmentMatchesOrNot = 0;

            DataSet DsInv = new DataSet();
            DsInv = GetDocumentDetails(iinvoiceID, "CRN");
            if (DsInv != null)
            {
                if (DsInv.Tables.Count > 0)
                {
                    if (DsInv.Tables[0].Rows.Count > 0)
                    {
                        lblRefernce.Text = DsInv.Tables[0].Rows[0]["InvoiceNo"].ToString();
                        lblInvoiceDate.Text = DsInv.Tables[0].Rows[0]["InvoiceDate"].ToString();
                        lblSupplier.Text = DsInv.Tables[0].Rows[0]["SupplierCompanyName"].ToString();
                        lblBuyer.Text = DsInv.Tables[0].Rows[0]["BuyerCompanyName"].ToString();

                        ViewState["SupplierCompanyID"] = DsInv.Tables[0].Rows[0]["SupplierCompanyID"].ToString();
                        ViewState["BuyerCompanyID"] = DsInv.Tables[0].Rows[0]["BuyerCompanyID"].ToString();

                        Session["BuyerCID"] = DsInv.Tables[0].Rows[0]["BuyerCompanyID"].ToString();
                        Session["InvoiceBuyerCompany"] = Session["BuyerCID"].ToString(); // Added By RImi on 8th Sept 2015
                        lblCurrentStatus.Text = DsInv.Tables[0].Rows[0]["Status"].ToString();
                        lblBusinessUnit.Text = Convert.ToString(DsInv.Tables[0].Rows[0]["BusinessUnit"]);

                        //Added by kuntalkarar on 10thJanuary2017
                        string getvendorClass = "SELECT TradingRelation.New_VendorClass FROM TradingRelation INNER JOIN Creditnote ON TradingRelation.BuyerCompanyID = Creditnote.BuyerCompanyID AND TradingRelation.SupplierCompanyID = Creditnote.SupplierCompanyID where Creditnote.CreditnoteId=" + iinvoiceID + " and Creditnote.BuyerCompanyID=" + Convert.ToInt32(Session["BuyerCID"].ToString()) + " and Creditnote.SupplierCompanyID=" + Convert.ToInt32(ViewState["SupplierCompanyID"].ToString()) + "";
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
                        string getPassedtoGroupCode = "SELECT PassedtoGroupCode from CreditNote  where CreditNoteID=" + iinvoiceID + " ";
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
                        string getRejectionCode = "SELECT RejectionCode from CreditNoteStatus where CreditNoteID=" + iinvoiceID + "  order by DisplaySequence desc";
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

                        //Blocked by Mainak 2018-06-30
                        //if (TypeUser == 2 || TypeUser == 3)// when User type is either Admin or Ap
                        //{
                        //    btnRematch.Visible = true;                            

                        //}
                        //else
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

                            //if (dt.Rows[0]["New_VendorClass"].ToString() == "PO")//Modified by Mainak 2018-08-06
                            //{
                            //    btnApprove.Visible = false;
                            //    btnSubmit.Visible = false;
                            //}
                            //else
                            //{
                            //    btnApprove.Visible = true;
                            //    btnSubmit.Visible = true;
                            //}

                            if (dt.Rows[0]["New_VendorClass"].ToString() == "PO")
                            {
                                btnApprove.Visible = false;
                                btnSubmit.Visible = false;
                            }
                            else
                            {
                                if (Convert.ToInt32(ViewState["StatusID"]) == 22 || Convert.ToInt32(ViewState["StatusID"]) == 21)
                                {
                                    btnApprove.Visible = true;
                                    btnSubmit.Visible = true;
                                }
                                else
                                {
                                    btnApprove.Visible = false;
                                    btnSubmit.Visible = false;
                                }

                            }
                        }
                        else
                        {
                            //Modified by Mainak 2018-09-19
                            btnApprove.Visible = false;
                            btnSubmit.Visible = false;
                        }
                        //else
                        //{
                        //    btnRematch.Visible = false;
                        //}

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
                        try
                        {
                            //Added by Kuntalkarar on 29thFeb2016
                            GetApprovalPathsForCreditnote(iinvoiceID, "CRN");
                            //Blocked  by Kuntalkarar on 29thFeb2016
                            //ViewState["approvalpath"] = DsInv.Tables[0].Rows[0]["ApprovalPath"].ToString();


                            lblcreditnoteno.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString(); ;
                            tbcreditnoteno.Text = DsInv.Tables[0].Rows[0]["CreditNoteNo"].ToString();
                            ViewState["AssociatedStatus"] = DsInv.Tables[0].Rows[0]["AssociatedStatus"].ToString();

                        }
                        catch { }

                        //==============Added By Rimi on 25th Nov 2015==================

                        try
                        {
                            if (DsInv.Tables[0].Rows[0]["Department"].ToString() != "")
                            {
                                lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();
                                ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();
                                //Added by kuntalkarar on 4thMarch2016
                                GetDepartMentDropDwons();


                                //blocked by kuntalkarar on 9thMarch2016
                                //ddldept.SelectedValue = Convert.ToString(ViewState["DepartmentID"]);
                                //blocked by kuntalkarar on 14thMarch2016
                                /*if (  Convert.ToString(Session["IsFromEDITDATAPage"]) == "yes")
                                {                                    
                                    ddldept.SelectedIndex = 0;
                                    DropDownList1.SelectedIndex = 0;
                                }
                                else
                                {
                                    ddldept.SelectedValue = Convert.ToString(ViewState["DepartmentID"]);
                                    DropDownList1.SelectedValue = Convert.ToString(ViewState["DepartmentID"]);
                                }*/
                                //added by kuntalkarar on 14thMarch2016---------------------
                                IsDepartmentMatchesOrNot = checkDepatmentIdMismatchOrNot(iinvoiceID);
                                if (IsDepartmentMatchesOrNot > 0)
                                {
                                    ddldept.SelectedValue = Convert.ToString(ViewState["DepartmentID"]);
                                    DropDownList1.SelectedValue = Convert.ToString(ViewState["DepartmentID"]);
                                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "IsDepartmentMatchesOrNot > 0");
                                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(iinvoiceID));
                                }
                                else
                                {
                                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "IsDepartmentMatchesOrNot > 0 ELSE");
                                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Convert.ToString(iinvoiceID));
                                    ddldept.SelectedIndex = 0;
                                    DropDownList1.SelectedIndex = 0;
                                }
                                //-----------------------------------------------------------
                            }
                            else
                            {
                                ViewState["DepartmentID"] = 0;
                            }
                            int dept = 0;
                            dept = Convert.ToInt32(Convert.ToString(ViewState["DepartmentID"]));

                            if (dept != 0)
                            {
                                GetApproverDropDownsAgainstDepartment(dept);
                            }
                            //  

                        }
                        catch { }
                        //==============Added By Rimi on 25th Nov 2015==================


                        //try
                        //{
                        //    lblDepartment.Text = DsInv.Tables[0].Rows[0]["Department"].ToString();
                        //    ViewState["DepartmentID"] = DsInv.Tables[0].Rows[0]["DepartmentID"].ToString();
                        //}
                        //catch { }
                        ViewState["StatusID"] = DsInv.Tables[0].Rows[0]["StatusID"].ToString();
                        iCurrentStatusID = Convert.ToInt32(DsInv.Tables[0].Rows[0]["StatusID"]);
                        ViewState["OriginalNetAmount"] = DsInv.Tables[0].Rows[0]["Net"].ToString();
                        Session["eDocType"] = DsInv.Tables[0].Rows[0]["DocType"].ToString();
                    }
                }
            }
        }
        #endregion

        #region grdList_ItemDataBound
        private void grdList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                //int j = e.Item.DataSetIndex + 1;
                //e.Item.Cells[0].Text = j.ToString();
                int j = e.Item.ItemIndex + 1;

                ((Label)e.Item.FindControl("lblLineNo")).Text = j.ToString();

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

            }

        }

        #endregion


        //Added By Rimi on 22nd July 2015
        protected void grdList_ItemCreated(object source, RepeaterItemEventArgs e)
        {
            ScriptManager scriptMan = ScriptManager.GetCurrent(this);

            DropDownList ddlBuyerCompanyCode = (DropDownList)e.Item.FindControl("ddlBuyerCompanyCode");
            TextBox txtLineVAT = (TextBox)e.Item.FindControl("txtLineVAT");
            TextBox txtNetVal = (TextBox)e.Item.FindControl("txtNetVal");

            if (ddlBuyerCompanyCode != null)
            {
                ddlBuyerCompanyCode.SelectedIndexChanged += SelectedIndexChanged_ddlBuyerCompanyCode;
                scriptMan.RegisterAsyncPostBackControl(ddlBuyerCompanyCode);
                txtLineVAT.TextChanged += txtLineVAT_TextChanged;
                scriptMan.RegisterAsyncPostBackControl(txtLineVAT);
                txtNetVal.TextChanged += txtNetVal_TextChanged;
                scriptMan.RegisterAsyncPostBackControl(txtNetVal);
            }

        }

        //Added By Rimi on 22nd July 2015 End

        #region CheckInvoiceExist
        private void CheckInvoiceExist()
        {
            int RowCnt = 1;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
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




            sqlDA.SelectCommand.Parameters.Add("@Type", "CRN");
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
                ViewState["data"] = null;
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
                // Added by Mrinal on 19th March 2015
                // ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = ds.Tables[1].Rows[i]["Description"].ToString();//Commenetd By Rimi on 8th AUgust 2015
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

        private void CheckNextInvoiceExist(int InvoiceId)
        {
            int RowCnt = 0;
            // int RowCnt = 1;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);

            SqlDataAdapter sqlDA = new SqlDataAdapter("ups_GetGenericCodingChange", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceId);
            sqlDA.SelectCommand.Parameters.Add("@Type", "CRN");
            DataSet ds = new DataSet();

            sqlConn.Open();
            sqlDA.Fill(ds);
            ViewState["populate"] = ds;
            //  ViewState["data"] = ds;
            grdList.DataSource = ds.Tables[1];
            grdList.DataBind();


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
            ViewState["data"] = null;
            BindGrid(RowCnt);

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
                // ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = ds.Tables[1].Rows[i]["Description"].ToString();//Commented By Rimi on 8th August 2015
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
            //if (iActionType == 1)
            //{
            //    takeAction("INV", Convert.ToInt32(Request["InvoiceID"]), 1);
            //}
            //else if (iActionType == 0)
            //{
            //    takeAction("INV", Convert.ToInt32(Request["InvoiceID"]), 0);
            //}
            // for invoice only
            if (iActionType == 1)
            {
                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {

                    takeAction("CRN", Convert.ToInt32(Request["InvoiceID"]), 1);
                }
                else
                {
                    takeAction("CRN", Convert.ToInt32(ViewState["InvoiceChecking"]), 1);
                }
            }
            else if (iActionType == 0)
            {
                if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
                {

                    takeAction("CRN", Convert.ToInt32(Request["InvoiceID"]), 0);
                }
                else
                {
                    takeAction("CRN", Convert.ToInt32(ViewState["InvoiceChecking"]), 0);
                }
            }





        }
        #endregion


        protected DataTable GetBlankTable(int iNoofRow)
        {

            DataTable tbl = null;
            int InvoiceID = 0;
            double dtmpNetAmt = 0;
            // InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {
                InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
            }
            else
            {
                InvoiceID = Convert.ToInt32(ViewState["CheckList"]);
            }
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
                    if (ds != null)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            nRow["PurOrderNo"] = ds.Tables[1].Rows[i]["PurOrderNo"];
                            nRow["VAT"] = ds.Tables[1].Rows[i]["VAT"];
                        }
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
                        nRow["NetValue"] = ds.Tables[1].Rows[i]["netvalue"];
                        nRow["PurOrderNo"] = ds.Tables[1].Rows[i]["PurOrderNo"];
                        nRow["VAT"] = ds.Tables[1].Rows[i]["VAT"];
                        tbl.Rows.Add(nRow);
                    }
                }
            }
            return tbl;
        }



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
                ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
                ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
                //ddlProject1=((DropDownList) grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
                ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

                if (compid != 0)
                {
                    //					if(TypeUser==1)
                    //						dt = objUser.GetDepartmentByUserID_GMG(Convert.ToInt32(Session["UserID"]),compid);
                    //					else	
                    //						dt= objInvoice.GetGridDepartmentList(compid);

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

        #region SelectedIndexChanged_ddlCodingDescription
        protected void SelectedIndexChanged_ddlCodingDescription(Object sender, System.EventArgs e)
        {
            string ddlDepartment1 = "", ddlNominalCode1 = "";
            int compid = 0;
            DataTable dt = new DataTable();
            DataSet dsDeptNom = new DataSet();
            DropDownList ddl = ((DropDownList)sender);
            int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
            if (TypeUser == 1)	//NORMAL USER
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue == "--Select--")
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                }
                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
                {
                    dsDeptNom = GetDepartmentANDNominalAgainstCodingDescID(Convert.ToInt32(ddl.SelectedValue), compid);

                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                    if (dsDeptNom.Tables[0].Rows.Count > 0)
                    {
                        ddlDepartment1 = dsDeptNom.Tables[0].Rows[0]["DepartmentID"].ToString();


                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dsDeptNom.Tables[0];
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);

                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                    if (dsDeptNom.Tables[1].Rows.Count > 0)
                    {
                        ddlNominalCode1 = dsDeptNom.Tables[1].Rows[0]["NominalCodeID"].ToString();

                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dsDeptNom.Tables[1];
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);

                }
            }
            else
            {
                if (Convert.ToString(ddl.SelectedValue) == "--Select--")
                {
                    if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                    {
                        compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                    }
                    if (compid != 0)
                    {
                        dt = objInvoice.GetGridDepartmentList(compid);


                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                        ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");

                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                        ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                    }
                    return;
                }

                int CodingDescriptionID = Convert.ToInt32(ddl.SelectedValue);

                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex = 0;
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedIndex = 0;

                    GetAllComboCodesAddNew();

                }
            }

        }
        #endregion

        #region SelectedIndexChanged_ddlBuyerCompanyCode
        protected void SelectedIndexChanged_ddlBuyerCompanyCode(object sender, System.EventArgs e)
        {
            /*
            int i = 0;
            DropDownList ddl = ((DropDownList)sender);
            i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;

            if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            {
                GetAllComboCodesFirstTime();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue = "--Select--";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue = "--Select--";
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = "--Select--";

                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                DataSet ds = LoadDepartment(Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = ds;
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                }
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Items.Clear();
                DataSet dsBusinessUnit = new DataSet();
                dsBusinessUnit = GetBusinessUnit(Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim()));
                if (dsBusinessUnit.Tables[0].Rows.Count > 0)
                {

                    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).DataSource = dsBusinessUnit;
                    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).DataTextField = "BusinessUnitName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).DataValueField = "BusinessUnitID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).DataBind();

                }
                ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");
            }
            else
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).Items.Insert(0, "--Select--");

            }
             */
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

                // string strCtrl = ((DropDownList)rptItem.FindControl("ddlBuyerCompanyCode")).ClientID;
                //  setFocus(strCtrl);
            }

        }
        #endregion

        #region  SelectedIndexChanged_ddlNominalCode
        protected void SelectedIndexChanged_ddlNominalCode(object sender, System.EventArgs e)
        {

            int iddlDept = 0;
            int iNomin = 0;
            int compid = 0;
            string CodingDescription = "--Select--";
            //			string strProjectName="";
            //			string strProjectID="";

            DropDownList ddl = ((DropDownList)sender);
            int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
            if (Convert.ToString(ddl.SelectedValue) != "--Select--")
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--")
                {

                    iddlDept = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                    iNomin = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);

                    DataSet dsCODES = new DataSet();
                    dsCODES = GetCodingDescriptionAgainstDepartmentANDNominal(iddlDept, iNomin, compid);
                    if (dsCODES.Tables.Count > 0 && dsCODES.Tables[0].Rows.Count > 0)
                    {
                        CodingDescription = dsCODES.Tables[0].Rows[0]["CodingDescriptionID"].ToString();
                    }
                    ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue = CodingDescription;

                }
            }
        }
        #endregion

        #region GetAllComboCodesForNominalRefresh
        private void GetAllComboCodesForNominalRefresh()
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

                if (compid != 0)
                {

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

        #region SelectedIndexChanged_ddlDepartment
        protected void SelectedIndexChanged_ddlDepartment(object sender, System.EventArgs e)
        {
            int inominalCodeID = 0, iDepartmentCodeID = 0;
            int iDCompID = 0;
            DropDownList ddl = ((DropDownList)sender);
            int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
            if (Convert.ToString(ddl.SelectedValue) == "--Select--")
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
                {
                    iDCompID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }

                DataTable dtcoding = new DataTable();
                dtcoding = objInvoice.GetGridCodingDescriptionList(iDCompID);
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dtcoding;
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");



                return;
            }


            if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            {
                iDCompID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
            }

            iDepartmentCodeID = Convert.ToInt32(ddl.SelectedValue);
            if (((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue == "--Select--")
            {
                inominalCodeID = 0;
            }
            else
                inominalCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);

            if (Convert.ToString(ddl.SelectedValue) != "--Select--")
            {
                DataSet Dst = new DataSet();
                Dst = GetNominalCodeAgainstDepartmentANDCompany(iDepartmentCodeID, iDCompID);
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = Dst;
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedIndex = 0;
            }
            else
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedIndex = 0;
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedIndex = 0;
            }
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
                        ddlNominalCode1 = dsDeptNom.Tables[1].Rows[0]["NominalCodeID"].ToString();//Sudhir on 08-07-2009
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

        #region private void ShowFiles(int InvoiceID)
        private void ShowFiles(int InvoiceID)
        {
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            //sqlConn=new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetUploadFileDetailsCreditNote_AkkeronETC", sqlConn);
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
        private void grdFile_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
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
        #endregion

        #region private void grdFile_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        private void grdFile_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
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
                        sDownLoadPath = ((Label)e.Item.FindControl("lblArchPath")).Text;
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
            }
        }
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
                catch
                {
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
                catch
                {

                }
            }
            return bRetVal;
        }
        #endregion

        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }

        private string GetURL2()
        {

            return ConfigurationManager.AppSettings["ServiceURLNew"];
        }

        #region btnAddNew_Click
        private void btnAddNew_Click(object sender, System.EventArgs e)
        {
            Populate(0, "a");
            int Count = 0;
            // Added by Mrinal on 10th November 2014
            Count = grdList.Items.Count - 1;
            if (Count > 0)
            {
                ((TextBox)grdList.Items[Count].FindControl("txtNetVal")).Text = lblVariance.Text;
                lblVariance.Text = Convert.ToString("0.00");

                // Added by Mrinal on 20th ?March 2015
                ((TextBox)grdList.Items[Count].FindControl("txtLineVAT")).Text = lblVATVariance.Text;
                lblVATVariance.Text = Convert.ToString("0.00");

                //====================Added By Rimi on 31st July 2015=================================

                ((DropDownList)grdList.Items[Count].FindControl("ddlBuyerCompanyCode")).SelectedValue = ((DropDownList)grdList.Items[Count - 1].FindControl("ddlBuyerCompanyCode")).SelectedValue;// Added By Rimi on 14thAugust2015
                ((TextBox)grdList.Items[Count].FindControl("txtAutoCompleteCodingDescription")).Text = ((TextBox)grdList.Items[Count - 1].FindControl("txtAutoCompleteCodingDescription")).Text;
                //((System.Web.UI.WebControls.HiddenField)(grdList.Items[Count].FindControl("hdnCodingDescriptionID"))).Value=((System.Web.UI.WebControls.HiddenField)(grdList.Items[Count-1].FindControl("hdnCodingDescriptionID"))).Value
                ((HiddenField)grdList.Items[Count].FindControl("hdnCodingDescriptionID")).Value = ((HiddenField)grdList.Items[Count - 1].FindControl("hdnCodingDescriptionID")).Value;
                ((HiddenField)grdList.Items[Count].FindControl("hdnDepartmentCodeID")).Value = ((HiddenField)grdList.Items[Count - 1].FindControl("hdnDepartmentCodeID")).Value;
                ((HiddenField)grdList.Items[Count].FindControl("hdnNominalCodeID")).Value = ((HiddenField)grdList.Items[Count - 1].FindControl("hdnNominalCodeID")).Value;
                ((TextBox)grdList.Items[Count].FindControl("txtLineDescription")).Text = ((TextBox)grdList.Items[Count - 1].FindControl("txtLineDescription")).Text;
                //====================Added By Rimi on 31st July 2015=================================
            }
            // Addition End 
            CalculateTotal();//Added by Rimi on 26.06.2015
        }
        //private void Populate(int irow, string acttype)
        //{
        //    int i;
        //    int j;
        //    string[] strValue = new string[1];
        //    DataRow dr;
        //    // Add by Mrinal on 19th March  2015
        //    string[] strVatValue = new string[1];
        //    ArrayList arrLineWiseDescription = new ArrayList();
        //    // Addition End

        //    ArrayList arrLstComp = new ArrayList();
        //    // Add by Mrinal on 7th November 2014
        //    ArrayList arrLstCodeDescription = new ArrayList();
        //    // Addition End
        //    ArrayList arrLstCode = new ArrayList();
        //    ArrayList arrLstDept = new ArrayList();
        //    ArrayList arrLstNomi = new ArrayList();
        //    ArrayList arrLstBusinessUnit = new ArrayList();
        //    ArrayList PurchaseOrderNO = new ArrayList();
        //    DataSet ds = ((DataSet)(ViewState["data"]));

        //    if (ds!=null)
        //    {
        //        ds.Tables[0].Rows.Clear();
        //    }

        //   // ds.Tables[0].Rows.Clear();

        //    for (i = 0; i <= grdList.Items.Count - 1; i++)
        //    {
        //        if (irow == 0 && acttype == "a")
        //        {
        //            arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);
        //            arrLstCodeDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text);

        //            arrLstCode.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value);
        //            arrLstDept.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value);
        //            arrLstNomi.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value);


        //            PurchaseOrderNO.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text);

        //            arrLstBusinessUnit.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))).SelectedItem.Value);
        //            strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
        //            dr = ds.Tables[0].NewRow();
        //            for (j = 0; j < 1; j++)
        //            {
        //                dr[j] = strValue[j].ToString();
        //            }

        //            // Added by Mrinal on 19th March 2015
        //            arrLineWiseDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text);
        //            strVatValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineVAT"))).Text;
        //                for (j = 2; j < 3; j++)
        //                {
        //                    dr[j] = strVatValue[j - 2].ToString();
        //                }
        //            // Addition End

        //            ds.Tables[0].Rows.Add(dr);
        //        }
        //        else if (irow != i && acttype == "d")
        //        {
        //            arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);
        //            arrLstCodeDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text);
        //            arrLstCode.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value);
        //            arrLstDept.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value);
        //            arrLstNomi.Add(((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value);
        //            PurchaseOrderNO.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text);

        //            arrLstBusinessUnit.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))).SelectedItem.Value);

        //            strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
        //            dr = ds.Tables[0].NewRow();
        //            for (j = 0; j < 1; j++)
        //            {
        //                dr[j] = strValue[j].ToString();
        //            }
        //            // Added by Mrinal on 19th March 2015
        //            arrLineWiseDescription.Add(((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text);
        //            strVatValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineVAT"))).Text;
        //            for (j = 2; j < 3; j++)
        //            {
        //                dr[j] = strVatValue[j - 2].ToString();
        //            }
        //            // Addition End

        //            ds.Tables[0].Rows.Add(dr);
        //        }
        //    }
        //    if (irow == 0 && acttype == "a")
        //    {
        //        if (ds != null)
        //        {
        //            dr = ds.Tables[0].NewRow();
        //            ds.Tables[0].Rows.Add(dr);
        //        }
        //    }
        //    ViewState["data"] = ds;
        //    BindGrid(1);
        //    dNetAmt = 0;
        //    i = 0;
        //    for (i = 0; i <= arrLstComp.Count - 1; i++)
        //    {
        //        ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedIndex = -1;
        //        ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).Items.FindByValue(arrLstComp[i].ToString()).Selected = true;
        //    }
        //    // Added by Mrinal on 19th March 2015
        //    for (i = 0; i <= arrLstCodeDescription.Count - 1; i++)
        //    {
        //        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = arrLineWiseDescription[i].ToString();
        //    }
        //    // Addition End
        //    for (i = 0; i <= arrLstCodeDescription.Count - 1; i++)
        //    {
        //       // ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtAutoCompleteCodingDescription"))).Text = arrLstCodeDescription[i].ToString();//Commenetd By Rimi on 8th August 2015
        //         //Added By Rimi on 8thaugust2015
        //        string LineDescription = arrLineWiseDescription[i].ToString();
        //        if (LineDescription.Contains("&lt;"))
        //        {
        //            LineDescription = LineDescription.Replace("&lt;", "<");
        //        }
        //        if (LineDescription.Contains("&gt;"))
        //        {
        //            LineDescription = LineDescription.Replace("&gt;", ">");
        //        }
        //        if (LineDescription.Contains("&pound;"))
        //        {
        //            LineDescription = LineDescription.Replace("&pound;", "£");
        //        }

        //        if (LineDescription.Contains("&belongsto;"))
        //        {
        //            LineDescription = LineDescription.Replace("&belongsto;", "€");
        //        }

        //        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = LineDescription.ToString();
        //    }
        //    for (i = 0; i <= arrLstCode.Count - 1; i++)
        //    {
        //        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value = arrLstCode[i].ToString();

        //    }

        //    for (i = 0; i <= arrLstDept.Count - 1; i++)
        //    {
        //        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value = arrLstDept[i].ToString();
        //    }

        //    for (i = 0; i <= arrLstNomi.Count - 1; i++)
        //    {
        //        ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnNominalCodeID"))).Value = arrLstNomi[i].ToString();
        //    }

        //    for (i = 0; i <= arrLstBusinessUnit.Count - 1; i++)
        //    {
        //        SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBusinessUnit"))), arrLstBusinessUnit[i].ToString());
        //    }

        //    for (i = 0; i <= PurchaseOrderNO.Count - 1; i++)
        //    {
        //        ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text = PurchaseOrderNO[i].ToString();
        //        string PurchaseorderNo = "";
        //        System.Web.UI.WebControls.TextBox txtPoNumber = (System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtPoNumber");
        //        PurchaseorderNo = txtPoNumber.Text;

        //        if (((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).Text != "")
        //        {
        //            if (GetPONumberForSupplierBuyer(PurchaseorderNo.Trim()) != "Y")
        //            {
        //                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).ForeColor = Color.Red;
        //            }
        //            else
        //            {
        //                ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtPoNumber"))).ForeColor = Color.Black;
        //            }
        //        }
        //    }

        //}


        private void Populate(int irow, string acttype)
        {
            int i;
            int j;
            string[] strValue = new string[1];
            DataRow dr;
            // Add by Mrinal on 19th March  2015
            string[] strVatValue = new string[1];
            ArrayList arrLineWiseDescription = new ArrayList();
            // Addition End

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

            if (ds != null)
            {
                ds.Tables[0].Rows.Clear();
            }

            // ds.Tables[0].Rows.Clear();

            for (i = 0; i <= grdList.Items.Count - 1; i++)
            {
                if (irow == 0 && acttype == "a")
                {
                    arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);
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

                    // Added by Mrinal on 19th March 2015
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
                    // Added by Mrinal on 19th March 2015
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
                if (ds != null)
                {
                    dr = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(dr);
                }
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
            // Added by Mrinal on 19th March 2015
            for (i = 0; i <= arrLstCodeDescription.Count - 1; i++)
            {
                //((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtLineDescription"))).Text = arrLineWiseDescription[i].ToString();//Commented By Rimi on 8th August 2015
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
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnCodingDescriptionID"))).Value = arrLstCode[i].ToString();

            }

            for (i = 0; i <= arrLstDept.Count - 1; i++)
            {
                ((System.Web.UI.WebControls.HiddenField)(grdList.Items[i].FindControl("hdnDepartmentCodeID"))).Value = arrLstDept[i].ToString();
            }

            for (i = 0; i <= arrLstNomi.Count - 1; i++)
            {
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

        #region btnDelLine_Click
        private void btnDelLine_Click(object sender, System.EventArgs e)
        {
            int i = 0;
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015
            //DataSet ds = ((DataSet)(ViewState["data"])); //commented By kd on 22th jan 2019 for prorate
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


        //===============Added By Rimi on 7th Sept 2015===========================
        public Boolean CheckPassToUserID(string creditnoteid)
        {
            DataSet ds = new DataSet();
            bool result = false;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_CheckPassToUserID_CN", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.AddWithValue("@creditnoteId", creditnoteid);
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

        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            //Added by Mainak 2018-09-06
            int iFlag = 1;
            string lineNO = string.Empty;
            int iOCount = 0;
            decimal NetValue = 0;
            decimal LineVAT = 0;
            decimal VatRate = 0;
            int vFlag = 0;
            int vRinfinityFlag = 0;
            int vRInfiFlag = 0;

            Session["button_clicked_Creditenote"] = "1";//added by kuntalkarar on 20thOctober2016
            //added by kuntalkarar on 9thMarch2016
            Session["IsFromEDITDATAPage"] = "No";

            Int32 Invoiceid = 0;

            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                Invoiceid = Convert.ToInt32(Request.QueryString["InvoiceID"]);

            }
            else
            {
                Invoiceid = Convert.ToInt32(ViewState["CheckList"]);

            }
            //===============Added By Rimi on 7th Sept 2015 For normal user double approval checking======================

            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
            {
                Boolean res1;
                res1 = CheckPassToUserID(Convert.ToString(Invoiceid));
                //if (Convert.ToInt64(ViewState["CheckList"]) == 0)
                //{
                //    res1 = CheckPassToUserID(Convert.ToString(Session["eInvoiceID"]));

                //}
                //else
                //{
                //    res1 = CheckPassToUserID(Convert.ToString(ViewState["CheckList"]));

                //}
                if (res1 == false)
                {
                    Response.Redirect("../Current/CRNclosewindows.aspx");
                }
            }

            //Added by Mainak 2018-09-12


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
            //Ended by Mainak 2018-09-12



            //===============Added By Rimi on 7th Sept 2015===========================
            if (iFlag > 0)
            {
                if (iOCount == 0)
                {
                    if (vRInfiFlag == 0)
                    {
                        if (CheckVarience())
                        {
                            if (vRFlag == 0)
                            {
                                bool ret = SaveDetailData();
                                if (ret == true)
                                {
                                    //int InvID = Convert.ToInt32(Session["eInvoiceID"]);
                                    int ival = ChangeStatus(Invoiceid, Convert.ToString(txtComment.Text), 19);
                                    if (ival > 0)
                                    {
                                        ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                                        MoveToNextInvoice();
                                        string message = "alert('Credit Note Approved Successfully.')";
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
                            Response.Write("<script>alert('Variance must be zero.');</script>");
                        }
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
            //Added by kuntalkarar on 20thOctober2016
            LoadDownloadFiles();
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015

        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Session["button_clicked_Creditenote"] = "1";//added by kuntalkarar on 20thOctober2016
            //added by kuntalkarar on 9thMarch2016
            Session["IsFromEDITDATAPage"] = "No";
            ViewState["CancelFlag"] = "1";// Added by Rimi on 22nd July 2015
            ViewState["Flag_Can"] = "Cancel";// Added By Rimi on 22nd July 2015
            MoveToNextInvoice();
            //Added by kuntalkarar on 20thOctober2016
            LoadDownloadFiles();
        }
        #endregion

        /* private List<INVS> fetchNextInvoidceId(Int32 Invoiceid)
        {
           
            List<INVS> lstINVS = new List<INVS>();
            Int64 InvoicID = Invoiceid;
            List<INVS> lstInvoiceS = (List<INVS>)Session["InvoiceID"];
            List<INVS> lstInvoice = new List<INVS>();



            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {
                if (Session["IndexforCRN"] == null)
                {

                    lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]) - 1);
                    Session["InvoiceID"] = "";
                    Session["InvoiceID"] = lstInvoiceS;
                }

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


                        if (Convert.ToString(DsInv.Tables[0].Rows[0]["StatusID"]) != "6")
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
                    lstInvoice.Add(objINVS);
                }

            }


            return lstInvoice;

        }*/

        //===============Added By Rimi on 22nd July 2015===============================


        private List<INVS> fetchNextInvoidceId(Int32 Invoiceid)
        {

            List<INVS> lstINVS = new List<INVS>();
            Int64 InvoicID = Invoiceid;
            List<INVS> lstInvoiceS = (List<INVS>)Session["InvoiceID"];

            List<INVS> lstInvoice = new List<INVS>();



            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {


                //lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]) - 1);//Commented By Rimi on 4th Dec 2015

                //blocked by kuntalkarar on 18thFeb2016
                lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]));//Added By Rimi on 4th Dec 2015


                //blocked by kuntalkarar on 7thMarch2016
                //Added by  kuntalkarar on 18thJan2016
                /*if (Convert.ToInt32(Request.QueryString["RowID"].ToString()) <= 1)
                {
                    lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]));
                }
                else
                {
                    lstInvoiceS.RemoveRange(0, Convert.ToInt32(Request.QueryString["RowID"]) - 1);
                }*/

                //----------------------------------------------------
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
                    DsInv = GetDocumentDetails(Convert.ToInt32(str.InvoiceID), str.DocType);
                    //==========================Commeneted By Rimi on 10th July2015================
                    //if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                    /// {CR


                    //  if (DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim() != "6")
                    // {
                    //INVS objINVS = new INVS();
                    // objINVS.InvoiceID = str.InvoiceID;
                    // lstINVS.Add(objINVS);
                    //}
                    // }
                    // else
                    // {
                    // INVS objINVS = new INVS();
                    // objINVS.InvoiceID = str.InvoiceID;
                    // objINVS.DocType = str.DocType;
                    // lstINVS.Add(objINVS);
                    //}
                    //==========================Commeneted By Rimi on 10th July2015================

                    //========Modified By Rimi on 10th July 2015=====================================================
                    if (Convert.ToString(ViewState["RejectFlag"]) != "yes" && Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) != "Rejected")// Added By Rimi on 9th July
                    {
                        if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                        {


                            if (DsInv.Tables[0].Rows[0]["StatusID"].ToString().Trim() != "6")
                            {
                                INVS objINVS = new INVS();
                                objINVS.InvoiceID = str.InvoiceID;
                                lstINVS.Add(objINVS);
                            }
                        }
                        else// Added By Rimi on 9th July
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = str.InvoiceID;
                            objINVS.DocType = str.DocType;
                            lstINVS.Add(objINVS);
                            ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
                        }
                    }
                    else
                    {
                        if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) != "Rejected")
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = str.InvoiceID;
                            objINVS.DocType = str.DocType;
                            lstINVS.Add(objINVS);
                            ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
                        }
                    }
                    if (Convert.ToInt32(Session["UserTypeID"]) != 1)
                    {
                        if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected")
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = str.InvoiceID;
                            objINVS.DocType = str.DocType;
                            lstINVS.Add(objINVS);
                        }
                    }
                    if (Convert.ToInt32(Session["UserTypeID"]) == 1)
                    {
                        if ((Convert.ToString(ViewState["RejectFlag"]) == "yes" && Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected"))
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = str.InvoiceID;
                            objINVS.DocType = str.DocType;
                            lstINVS.Add(objINVS);
                            // ViewState["RejectFlag"] = "No";// Added By Rimi on 9th July
                        }

                        else if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) == "Cancel" && Convert.ToString(DsInv.Tables[0].Rows[0]["DocType"]) != "INV")
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = str.InvoiceID;
                            objINVS.DocType = str.DocType;
                            lstINVS.Add(objINVS);
                        }
                        else if (Convert.ToString(DsInv.Tables[0].Rows[0]["Status"]) == "Rejected" && Convert.ToString(ViewState["Flag_Can"]) != "Cancel" && Convert.ToString(ViewState["MSG"]) == "Approve" || Convert.ToString(ViewState["MSG"]) == "Open" || Convert.ToString(ViewState["MSG"]) == "Delete" || Convert.ToString(ViewState["MSG"]) == "Reopen")
                        {
                            INVS objINVS = new INVS();
                            objINVS.InvoiceID = str.InvoiceID;
                            objINVS.DocType = str.DocType;
                            lstINVS.Add(objINVS);
                        }
                    }
                    //========Modified By Rimi on 10th July 2015 End=====================================================

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
                    // objINVS.DocType = INV.DocType;
                    lstInvoice.Add(objINVS);
                }

            }


            return lstInvoice;

        }

        //===============Added By Rimi on 22nd July 2015 End===============================


        private string returnInvoiceId(Int32 Indx)
        {

            List<INVS> InvoiceId;
            InvoiceId = fetchNextInvoidceId(Convert.ToInt32(ViewState["InvoiceId"]));
            Session.Add("CreditNotes", InvoiceId);
            InvoiceId = (List<INVS>)(Session["CreditNotes"]);
            String RtnInvoiceId = InvoiceId[Indx].InvoiceID;
            return RtnInvoiceId;
        }
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
                Session["NewInvoiceId"] = InvoiceId; // Added by kd on 04 Jan2019


                foreach (INVS str in (List<INVS>)Session["InvoiceID"])
                {
                    if (str.InvoiceID == Convert.ToString(InvoiceId))
                    {
                        strDocType = str.DocType;
                    }
                }


                if (strDocType == "INV")
                {
                    //26-06-20156
                    Session["IndexforINV"] = ViewState["Counter"];
                    Session.Add("creninv", InvoiceId);
                    //Response.Redirect("../Invoice/InvoiceActionTiffViewer.aspx");// Commenetd By Rimi on 22nd July 2015

                    // Added By Rimi on 22nd July 2015
                    if (ViewState["CancelFlag"] == "1")
                    {
                        Response.Redirect("../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + InvoiceId + "&DDCompanyID=" + ViewState["DDCompanyID"]);
                        ViewState["CancelFlag"] = "2";
                    }
                    else
                    {
                        Response.Write("<script>opener.location.reload();</script>");
                        Response.Write("<script>self.close();</script>");
                        Response.Redirect("../Invoice/InvoiceActionTiffViewer.aspx?MsgFlag=1&MSG=" + ViewState["MSG"].ToString() + "&InvoiceID=" + InvoiceId + "&DDCompanyID=" + ViewState["DDCompanyID"]);
                    }
                    // Added By Rimi on 22nd July 2015

                }
                else
                {
                    ViewState["CheckList"] = InvoiceId;

                    dNetAmt = 0;

                    //========Added By Rimi on 8th Sept For Department - Approver Dropdowns issue=======
                    DataSet dsDept = new DataSet();
                    CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval();
                    string Fields = "CreditNote.DepartmentID,Department.Department";
                    string Table = "dbo.CreditNote INNER JOIN dbo.Department ON Department.DepartmentID=dbo.CreditNote.DepartmentID";
                    string Criteria = "CreditNote.CreditNoteID = " + System.Convert.ToInt32(InvoiceId);
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
                    //========Added By Rimi on 8th Sept For Department - Approver Dropdowns issue ENd=======
                    ViewState["approvalpath"] = "";
                    if (InvoiceId != 0)
                    {
                        GetDocumentDetails(InvoiceId);
                        /*
                        string strStatusLogLink = GetInvoiceStatusLog();
                        iframeInvoiceStatusLog.Attributes.Add("src", strStatusLogLink);
                     //   aInvoiceStatusLog.Attributes.Add("onclick", GetInvoiceStatusLog());
                         * 
                         */
                        DataSet ds = GetDocumentDetails(InvoiceId, "CRN");
                        Boolean Duplicate = Convert.ToBoolean(ds.Tables[0].Rows[0]["Duplicate"]);
                        if (Duplicate == false)
                        {
                            lblDuplicate.Visible = false;
                            tdDup.Style.Add("Display", "none");
                        }
                        else
                        {
                            lblDuplicate.Visible = true;
                            tdDup.Style.Add("Display", "");
                        }
                        //  string strStatusLogLink = GetInvoiceStatusLogNextInvoice(Convert.ToString(InvoiceId));

                        string strStatusLogLink = GetInvoiceStatusLog();
                        strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:530,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                        //aInvoiceStatusLog.Attributes.Add("onclick", strStatusLogLink);

                        InvoiceCrnIsDuplicate();
                        IsAutorisedtoEditData();
                    }

                    GetDepartMentDropDwons();
                    ViewState["InvoiceChecking"] = InvoiceId;//added by Mainak 2018-10-11
                    CheckNextInvoiceExist(InvoiceId);
                    //  CheckInvoiceExist();

                    CalculateTotal();
                    GetVatAmount();

                    //GetVatAmount();


                    if (TypeUser >= 2 && Convert.ToInt32(ViewState["StatusID"]) == 20)
                        lblDepartment.Visible = false;
                    //modified by Subhrajyoti on 27.03.2015
                    if (TypeUser < 1)
                    {
                        tbcreditnoteno.Visible = false;
                        btnEditAssociatedInvoiceNo.Visible = false;
                    }
                    //modified by Subhrajyoti on 27.03.2015
                    ShowFiles(Convert.ToInt32(InvoiceId));
                    ButtonRejectVisibility();
                    string TiffUrl = "../../TiffViewerDefault.aspx?ID=" + InvoiceId.ToString() + "&Type=" + "CRN";
                    TiffWindow.Attributes.Add("src", TiffUrl);

                    txtComment.Text = "";
                    //Added by kuntal karar on 20thOctober2016- Sessions created here
                    Session["NextInvoiceID"] = Convert.ToString(InvoiceId);
                    Session["NextBuyerCompanyID"] = GetInvoiceBuyerCompanyID1(Convert.ToInt32(InvoiceId));
                    //added by kuntalkarar on 12thJanuary2017
                    Session["CreditnoteId_GoToStockQC"] = Convert.ToString(InvoiceId);
                    //----------------------------------------
                }
                //===============Added By Subhrajyoti on 3rd August 2015===========================
                DataSet dss = GetDocumentDetails(Convert.ToInt32(InvoiceId), "CRN");
                Int32 StatusId = Convert.ToInt32(dss.Tables[0].Rows[0]["StatusId"]);

                if (Convert.ToInt32(StatusId) != 20 && Convert.ToInt32(StatusId) != 21 && Convert.ToInt32(StatusId) != 22 && Convert.ToInt32(StatusId) != 6)
                {
                    Response.Redirect("../Current/CRNclosewindows.aspx");
                }
                //addded by kuntalkarar on 1stMarch2016
                // Session["eInvoiceID"] = InvoiceId;
                //===============Added By Subhrajyoti on 3rd August  2015===========================

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                Response.Write("<script> parent.window.close();</script>");
            }




        }





        private void MoveToInvoice(Int32 InvoiceId)
        {
            try
            {

                ViewState["Counter"] = 1;
                ViewState["CheckList"] = InvoiceId;

                dNetAmt = 0;

                ViewState["approvalpath"] = "";
                if (InvoiceId != 0)
                {
                    GetDocumentDetails(InvoiceId);

                    DataSet ds = GetDocumentDetails(InvoiceId, "CRN");
                    Boolean Duplicate = Convert.ToBoolean(ds.Tables[0].Rows[0]["Duplicate"]);
                    if (Duplicate == false)
                    {
                        lblDuplicate.Visible = false;
                        tdDup.Style.Add("Display", "none");
                    }
                    else
                    {
                        lblDuplicate.Visible = true;
                        tdDup.Style.Add("Display", "");
                    }
                    //string strStatusLogLink = GetInvoiceStatusLogNextInvoice(Convert.ToString(InvoiceId));
                    string strStatusLogLink = GetInvoiceStatusLog();
                    strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:530,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                    //aInvoiceStatusLog.Attributes.Add("onclick", strStatusLogLink);

                    InvoiceCrnIsDuplicate();
                    IsAutorisedtoEditData();
                }

                GetDepartMentDropDwons();
                CheckNextInvoiceExist(InvoiceId);





                //     CheckInvoiceExist();

                CalculateTotal();
                GetVatAmount();

                //GetVatAmount();


                if (TypeUser >= 2 && Convert.ToInt32(ViewState["StatusID"]) == 20)
                    lblDepartment.Visible = false;
                //modified by Subhrajyoti on 27.03.2015
                if (TypeUser < 1)
                {
                    tbcreditnoteno.Visible = false;
                    btnEditAssociatedInvoiceNo.Visible = false;
                }
                //modified by Subhrajyoti on 27.03.2015
                ShowFiles(Convert.ToInt32(InvoiceId));
                ButtonRejectVisibility();
                string TiffUrl = "../../TiffViewerDefault.aspx?ID=" + InvoiceId.ToString() + "&Type=" + "CRN";
                TiffWindow.Attributes.Add("src", TiffUrl);

                txtComment.Text = "";
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                Response.Write("<script> parent.window.close();</script>");
            }




        }





        #region SaveDetailData()

        //================Commented By Rimi on 21st August 2015=======================
        //private bool SaveDetailData()
        //{
        //    #region variables
        //    // int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);      //commented  by Rimi on 25.06.2015
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
        //    // Added by Mrinal on 19th March 2015

        //    decimal LineVAT = 0;
        //    string strLineDescription = string.Empty;

        //    // Addition End on 19th March 2015 


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
        //                //int RowNominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[j].FindControl("hdnNominalCodeID")).Value);
        //                //int RowDepartmentCodeID = Convert.ToInt32(((HiddenField)grdList.Items[j].FindControl("hdnDepartmentCodeID")).Value);
        //                //int RowCodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[j].FindControl("hdnCodingDescriptionID")).Value);

        //                Response.Write("<script>alert('Please select coding.');</script>");
        //                iValidFlag = 1;
        //                break;

        //            }
        //            //if (((DropDownList)grdList.Items[j].FindControl("ddlCodingDescription1")).SelectedValue.Trim() == "--Select--")
        //            //{
        //            //    Response.Write("<script>alert('Please select coding.');</script>");
        //            //    iValidFlag = 1;
        //            //    break;
        //            //}
        //            //if (((DropDownList)grdList.Items[j].FindControl("ddlDepartment1")).SelectedValue.Trim() == "--Select--")
        //            //{
        //            //    Response.Write("<script>alert('Please select department name.');</script>");
        //            //    iValidFlag = 1;
        //            //    break;
        //            //}
        //            //if (((DropDownList)grdList.Items[j].FindControl("ddlNominalCode1")).SelectedValue.Trim() == "--Select--")
        //            //{
        //            //    Response.Write("<script>alert('Please select nominal code.');</script>");
        //            //    iValidFlag = 1;
        //            //    break;
        //            //}

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
        //        // Added by Mrinal On 19th March 2015
        //        dtXML.Columns.Add("LineVAT");
        //        dtXML.Columns.Add("LineDescription");
        //        // Addition End On 19th March 2015 

        //        DataRow DR = null;

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<Root>");
        //        NetVal = Math.Round(NetVal, 2);
        //        ViewState["NetAmt"] = NetVal;
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
        //                    //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //                    //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //                    //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
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
        //                        DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
        //                        ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
        //                    }
        //                    ViewState["vDepartmentID"] = DepartmentID;
        //                    if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
        //                    {
        //                        CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //                    }
        //                    else
        //                    {
        //                        CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
        //                        ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();

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
        //                   // }
        //                }
        //                // Added by Mrinal on 19th March 2015
        //                LineVAT = 0;
        //                if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
        //                {
        //                    //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
        //                    //{
        //                        LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
        //                   // }
        //                }
        //                strLineDescription = string.Empty;

        //                if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
        //                {
        //                    strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
        //                }
        //                 //Added By RImi on 8th August 2015

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
        //                // Addition End on 19th March 2015 
        //                #endregion




        //                //if (NetValue > 0 || (Convert.ToDecimal(Request.QueryString["iVat"]) > 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > 0))//Commeneted By RImi on 31stJuly2015
        //                if (NetValue > -1 || (Convert.ToDecimal(Request.QueryString["iVat"]) > -1 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))
        //                {
        //                    DR = dtXML.NewRow();
        //                    dtXML.Rows.Add(DR);
        //                    sb.Append("<Rowss>");
        //                    sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
        //                    sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
        //                    sb.Append("<InvoiceType>").Append("CRN").Append("</InvoiceType>");
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

        //                    // Added by Mrinal on 19th March 2015
        //                    sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
        //                    sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");
        //                    // Addition End on 19th March 2015 

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
        //           // Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
        //            Response.Write("<script>alert('Variance must be zero.');</script>");
        //        }
        //    }
        //    return retval;
        //}

        //================================Added By Rimi on 21st August 2015=====================================


        private bool SaveDetailData()
        {
            #region variables
            // int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);      //commented  by Rimi on 25.06.2015
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
            // Added by Mrinal on 19th March 2015

            decimal LineVAT = 0;
            string strLineDescription = string.Empty;

            // Addition End on 19th March 2015 


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
                        //int RowNominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[j].FindControl("hdnNominalCodeID")).Value);
                        //int RowDepartmentCodeID = Convert.ToInt32(((HiddenField)grdList.Items[j].FindControl("hdnDepartmentCodeID")).Value);
                        //int RowCodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[j].FindControl("hdnCodingDescriptionID")).Value);

                        Response.Write("<script>alert('Please select coding.');</script>");
                        iValidFlag = 1;
                        break;

                    }
                    //if (((DropDownList)grdList.Items[j].FindControl("ddlCodingDescription1")).SelectedValue.Trim() == "--Select--")
                    //{
                    //    Response.Write("<script>alert('Please select coding.');</script>");
                    //    iValidFlag = 1;
                    //    break;
                    //}
                    //if (((DropDownList)grdList.Items[j].FindControl("ddlDepartment1")).SelectedValue.Trim() == "--Select--")
                    //{
                    //    Response.Write("<script>alert('Please select department name.');</script>");
                    //    iValidFlag = 1;
                    //    break;
                    //}
                    //if (((DropDownList)grdList.Items[j].FindControl("ddlNominalCode1")).SelectedValue.Trim() == "--Select--")
                    //{
                    //    Response.Write("<script>alert('Please select nominal code.');</script>");
                    //    iValidFlag = 1;
                    //    break;
                    //}

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
                // Added by Mrinal On 19th March 2015
                dtXML.Columns.Add("LineVAT");
                dtXML.Columns.Add("LineDescription");
                // Addition End On 19th March 2015 

                DataRow DR = null;

                StringBuilder sb = new StringBuilder();
                sb.Append("<Root>");
                NetVal = Math.Round(NetVal, 2);
                ViewState["NetAmt"] = NetVal;
                if (Convert.ToDouble(ViewState["NetAmt"].ToString()) == Convert.ToDouble(NetVal.ToString()))
                {
                    for (int i = 0; i <= grdList.Items.Count - 1; i++)
                    {
                        #region Getting DropDown Values
                        if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
                        {
                            CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                        }
                        if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
                        {
                            //=========================Commeneted By Rimi on 3rd August 2015=========================================
                            //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                            //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                            //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                            //=========================Commeneted By Rimi on 3rd August 2015 End=========================================


                            //=====================Modified By Rimi on 3rd August 2015=============================
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
                                NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                                ViewState["hdnDepartmentCodeID"] = "2";
                                //}

                                //else
                                //{
                                //    NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
                                ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = NominalCodeID.ToString();
                                //    ViewState["hdnDepartmentCodeID"] = "1";
                                //}
                                //----ENDS-----------blocked by kuntal karar on 17thAugust2015--------------
                            }
                            else
                            {
                                NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                                ViewState["hdnDepartmentCodeID"] = "2";
                            }
                            //---------------blocked by kuntal karar on 17thAugust2015--------------
                            //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                            //{
                            if (((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value != "0")
                            {
                                DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                            }
                            //}
                            //else
                            //{
                            //    DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
                            ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
                            // }
                            ViewState["vDepartmentID"] = DepartmentID;
                            //---------------blocked by kuntal karar on 17thAugust2015--------------
                            //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                            //{
                            CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                            //}
                            //else
                            //{
                            //    CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
                            ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();

                            // }
                            //---ENDS------------blocked by kuntal karar on 17thAugust2015--------------
                            //====================Modified By Rimi on 3rd August 2015 End======================
                        }

                        if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue) != "--Select--")
                        {
                            BusinessUnitID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBusinessUnit")).SelectedValue);
                        }


                        PurOrderNo = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtPoNumber")).Text);


                        NetValue = 0;
                        if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
                        {
                            //blocked by kuntalkarar on 14thJan2016
                            //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
                            //{
                            NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                            //}
                        }
                        // Added by Mrinal on 19th March 2015
                        LineVAT = 0;
                        if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
                        {
                            //blocked by kuntalkarar on 14thJan2016
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
                        // Addition End on 19th March 2015 
                        #endregion
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



                        // if (NetValue > 0 || (Convert.ToDecimal(Request.QueryString["iVat"]) > 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > 0))// Commened By Rimi on 3rd August 2015
                        if (NetValue > -1 || (Convert.ToDecimal(Request.QueryString["iVat"]) > -1 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))// Addded By Rimi on 3rd August 2015
                        {
                            DR = dtXML.NewRow();
                            dtXML.Rows.Add(DR);
                            sb.Append("<Rowss>");
                            sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
                            sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
                            sb.Append("<InvoiceType>").Append("CRN").Append("</InvoiceType>");
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

                            // Added by Mrinal on 19th March 2015
                            sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
                            sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");
                            // Addition End on 19th March 2015 

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
                }
                else
                {
                    lblErrorMsg.Visible = false;
                    // Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
                    Response.Write("<script>alert('Variance must be zero.');</script>");
                }
            }
            return retval;
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

        #region GetInvoiceBuyerCompanyID
        public int GetInvoiceBuyerCompanyID(int iInvoiceID)
        {
            int BuyerID = 0;
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "SELECT BuyerCompanyID,SupplierCompanyID FROM CreditNote WHERE InvoiceID=" + iInvoiceID;
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

        #region GetNetAmt(int InvoiceID)
        private double GetNetAmt(int InvoiceID)
        {
            double NetAmt = 0;
            double CodingValue = 0;// Added by Rimi on 26.06.2015

            string sSql = "select NetValue,CodingValue from GenericCodingChange where invoiceid=" + InvoiceID;// Added by Rimi on 26.06.2015
            SqlDataReader dr = null;
            SqlDataReader dr2 = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlConnection sqlConn2 = new SqlConnection(ConsString);
            SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);


            //added by kuntal karar on 30thJune2015------------------------------------------
            string sSql2 = "select nettotal from CreditNote where CreditNoteid=" + InvoiceID;
            SqlCommand sqlCmd2 = new SqlCommand(sSql2, sqlConn2);
            //-------------------------------------------------------------------------------


            try
            {
                sqlConn.Open();
                dr = sqlCmd.ExecuteReader();
                //sqlConn.Close();
                sqlConn2.Open();
                dr2 = sqlCmd2.ExecuteReader();

                while (dr.Read())
                {
                    if (dr[0] != DBNull.Value)
                    {
                        //NetAmt = Convert.ToDouble(dr[0]);// commented by Rimi on 26.06.2015
                        NetAmt += Convert.ToDouble(dr[0]);// Added by Rimi on 26.06.2015
                    }
                    //blocked by kuntal karar on 30thJune2015----------------

                    // Added by Rimi on 26.06.2015
                    //if (dr[1] != DBNull.Value)
                    //{
                    //    CodingValue = Convert.ToDouble(dr[1]);
                    //}
                    // Added by Rimi on 26.06.2015 End

                    //-------------------------------------------------------
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

        #region GetCodingDescriptionAgainstDepartmentANDNominal
        public DataSet GetCodingDescriptionAgainstDepartmentANDNominal(int iDepartmentCodeID, int iNominal, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE isnull(APAdminOnly,0) <> 1 AND DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + "";
            else
                sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + "";
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

        #region GetNominalCodeAgainstDepartmentANDCompany
        public DataSet GetNominalCodeAgainstDepartmentANDCompany(int iDepartmentCodeID, int iDCompID)
        {
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            string sSql = "";
            if (TypeUser == 1)
                sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in(SELECT NominalCodeID FROM CodingDescription WHERE isnull(APAdminOnly,0)<>1 and DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ")";
            else
                sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in(SELECT NominalCodeID FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ")";
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

        #region InsertCodingChangeValuesByDeleting
        public int InsertCodingChangeValuesByDeleting(string XMLText, int invoiceID)
        {
            int RetVal = 0;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);

            try
            {
                sqlCmd = new SqlCommand("sp_InsertCodingChangeValuesByDeleting_Communicorp", sqlConn);

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
        private int ChangeStatus(int CreditNoteID, string Comments, int ApproverStatus)
        {
            //====Added By Rimi on 4th Dec 2015=========

            string NewApprover2 = "";

            if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
                NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);

            //===Added By Rimi on 4th Dec 2015 End======
            int iCount = 0;
            SqlParameter sqlOutputParam = null;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            //modified by kuntal karar on 25thMay2015 as per the the request of subhrajyoti
            sqlCmd = new SqlCommand("stp_UpdateSTOCKCreditNoteCOMMON_Generic_GRH", sqlConn);
            // sqlCmd = new SqlCommand("Sp_UpdateStatusCreditNote_AkkeronETC", sqlConn);

            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"]));
            sqlCmd.Parameters.Add("@CreditNoteID", CreditNoteID);
            sqlCmd.Parameters.Add("@Comments", Comments);
            sqlCmd.Parameters.Add("@ApproverStatus", ApproverStatus);
            sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);
            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string dd = ex.Message.ToString(); iCount = -1; }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iCount);
        }
        #region ChangeStatusOFCreditNote(int invoiceID,int StatusID,string Comments )
        public int ChangeStatusOFCreditNote(int invoiceID, int StatusID, string Comments)
        {
            int RetVal = 0;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);

            try
            {
                sqlCmd = new SqlCommand("sp_ChangeStatusOFCreditNote", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@CreditNoteID", invoiceID);
                sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"].ToString()));
                sqlCmd.Parameters.Add("@Status", StatusID);
                sqlCmd.Parameters.Add("@Comments", Comments);
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

        //Added by Mainak 2018-05-24
        #region btnRematch_Click
        protected void btnRematch_Click(object sender, EventArgs e)
        {
            Session["button_clicked_Creditenote"] = "1";//Modified by Mainak 2018-10-11
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

            SqlCommand sqlCmd = new SqlCommand("sp_ButtonRematchPress_GenericCRN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CreditNoteID", InvoiceID_Remtach);
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
            Session["button_clicked_Creditenote"] = "1";//added by kuntalkarar on 20thOctober2016
            //Added by kuntalkarar on 9thMarch2016
            Session["IsFromEDITDATAPage"] = "No";

            Int32 Invoiceid = 0;
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                Invoiceid = Convert.ToInt32(Request.QueryString["InvoiceID"]);

            }
            else
            {
                Invoiceid = Convert.ToInt32(ViewState["CheckList"]);

            }

            if (Convert.ToString(ViewState["AssociatedStatus"]) != "" && Convert.ToInt32(ViewState["AssociatedStatus"]) != 19 && Convert.ToInt32(ViewState["AssociatedStatus"]) != 23)
            {
                Response.Write("<script>alert('The associated invoice must be in Approved or Paid status.');</script>");
                return;
            }
            if (CheckVarience())
            {
                bool ret = SaveDetailData();
                if (ret == true)
                {
                    iApproverStatusID = 19;
                    int UpdateStatus = UpdateSTOCKCreditNoteCOMMON_Generic(Invoiceid, strComments, 19);
                    if (UpdateStatus >= 1)
                    {
                        doAction(0);
                        // Page.RegisterStartupScript("Reg", "<script>ApproveClose();</script>");
                        // Added by Mrinal on 13th October  2014
                        Response.Write("<script>alert('Credit Note Approved Successfully.');</script>");
                        ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                        MoveToNextInvoice();

                    }
                    else if (UpdateStatus == -1)
                        Response.Write("<script>alert('Error in approving.');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Variance must be zero.');</script>");
            }


            //Added by kuntalkarar on 20thOctober2016
            LoadDownloadFiles();



        }
        #endregion

        #region UpdateSTOCKCreditNoteCOMMON_Generic
        public int UpdateSTOCKCreditNoteCOMMON_Generic(int CreditNoteID, string Comments, int ApproverStatus)
        {
            string NewApprover2 = "";

            //if (Convert.ToString(ddlApprover2.SelectedItem.Text) != "Select")
            //    NewApprover2 = Convert.ToString(ddlApprover2.SelectedItem.Text);
            if (Convert.ToString(ViewState["sdApprover2"]) != null || Convert.ToString(ViewState["sdApprover2"]) != "")
            {
                NewApprover2 = Convert.ToString(ViewState["sdApprover2"]);
            }
            int iCount = 0;
            SqlParameter sqlOutputParam = null;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            //sqlCmd = new SqlCommand("stp_UpdateSTOCKCreditNoteCOMMON_Generic_Communicorp", sqlConn);
            sqlCmd = new SqlCommand("stp_UpdateSTOCKCreditNoteCOMMON_Generic_GRH", sqlConn);

            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"]));
            sqlCmd.Parameters.Add("@CreditNoteID", CreditNoteID);
            sqlCmd.Parameters.Add("@Comments", Comments);
            sqlCmd.Parameters.Add("@ApproverStatus", ApproverStatus);
            sqlCmd.Parameters.Add("@NewApprover2", NewApprover2);
            sqlOutputParam = sqlCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iCount = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string dd = ex.Message.ToString(); iCount = -1; }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return (iCount);
        }
        #endregion

        #region btndelete_Click
        private void btndelete_Click(object sender, System.EventArgs e)
        {
            Session["button_clicked_Creditenote"] = "1";//added by kuntalkarar on 20thOctober2016
            //added by kuntalkarar on 9thMarch2016
            Session["IsFromEDITDATAPage"] = "No";


            if (txtComment.Text.Trim() == "")
            {
                //   Response.Write("<script>alert('Please enter a comment.');</script>");
                string message = "alert('Please enter a comment.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                return;
            }
            else
            {
                // Invoice.Invoice objinvoice = new Invoice.Invoice();
                JKS.Invoice objinvoice = new JKS.Invoice();
                lblErrorMsg.Text = "";
                lblErrorMsg.Visible = false;
                iApproverStatusID = 7;
                string strComments = txtComment.Text.Trim();
                int DeptUpdate = UpdateDepartmentAgainstCreditNoteID();
                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));



                int StatusUpdate = 0;


                if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                {

                    StatusUpdate = objinvoice.UpdateCrnStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                }
                else
                {
                    StatusUpdate = objinvoice.UpdateCrnStatusToDelete(System.Convert.ToInt32(ViewState["CheckList"]));
                }

                if (StatusUpdate == 1)
                {

                    //Added BY Mrinal on 6th January 2015

                    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments);
                        //
                    }
                    else
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(ViewState["CheckList"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments);
                    }


                    doAction(0);
                    /*
                     //----------Blocked by Mrinal on 13th October 2014-----------------------

                    Page.RegisterStartupScript("Reg", "<script>CaptureClose();</script>");
                    
                    */
                    //					Response.Write("<script>alert('Invoice Deleted Successfully.');</script>");	
                    //					Response.Write("<script>self.close();</script>");
                    //					Response.Write("<script>window.opener.Form1.btnSearch.click();</script>");

                    // Added by Mrinal on 13th October  2014
                    //  Response.Write("<script>alert('Credit Note Deleted Successfully.');</script>");

                    ViewState["MSG"] = "Delete";// Added By Rimi on 22nd July 2015
                    MoveToNextInvoice();
                    string message = "alert('Credit Note Deleted Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                else
                {
                    //Response.Write("<script>alert('Invoice cannot be deleted');</script>");
                    string message = "alert('Invoice cannot be deleted')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
            //Added by kuntalkarar on 20thOctober2016
            LoadDownloadFiles();
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015
        }
        #endregion

        #region UpdateDepartmentAgainstCreditNoteID()
        private int UpdateDepartmentAgainstCreditNoteID()
        {
            int DeptID = 0;
            if (Convert.ToInt32(ddldept.SelectedIndex) > 0)
                DeptID = Convert.ToInt32(ddldept.SelectedValue);
            string strcomments = txtComment.Text.Trim();

            int iretval = 0;
            string sSql = "";
            if (DeptID > 0)
            {
                if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                {

                    sSql = "UPDATE creditnote SET departmentid =" + DeptID + " ,Comment ='" + strcomments + "' WHERE creditnoteID =" + Convert.ToInt32(Session["eInvoiceID"]);
                }
                else
                {
                    sSql = "UPDATE creditnote SET departmentid =" + DeptID + " ,Comment ='" + strcomments + "' WHERE creditnoteID =" + Convert.ToInt32(ViewState["CheckList"]);
                }



            }
            else
            {
                if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                {
                    sSql = "UPDATE creditnote SET Comment ='" + strcomments + "' WHERE creditnoteID =" + Convert.ToInt32(Session["eInvoiceID"]);
                }
                else
                {
                    sSql = "UPDATE creditnote SET Comment ='" + strcomments + "' WHERE creditnoteID =" + Convert.ToInt32(ViewState["CheckList"]);
                }
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
            return iretval;
        }
        #endregion

        #region btnEditAssociatedInvoiceNo_Click
        private void btnEditAssociatedInvoiceNo_Click(object sender, System.EventArgs e)
        {
            string strMessage = "";
            int RetVal = 0;
            string sQuery = "";
            string sCreditNoteId = "";
            SqlCommand sqlCmd = null;
            RetVal = IsValiedAssociatedInvoiceNo();
            if (RetVal == -101)
            {
                strMessage = "Invalid Invoice Entered.";
            }
            else if (RetVal == -102)
                //strMessage = "INVOICE DOES NOT EXISTS.";
                strMessage = "Invoice does not exist.";
            else if (RetVal == 0)
                strMessage = "Please enter invoice no.";
            if (strMessage == "")
            {
                RetVal = 0;
                SqlConnection sqlConn = new SqlConnection(ConsString);
                try
                {
                    if (ViewState["InvoiceID"] != null)
                    {
                        if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                        {
                            sCreditNoteId = ViewState["InvoiceID"].ToString();
                        }
                        else
                        {
                            sCreditNoteId = ViewState["CheckList"].ToString();
                        }
                    }
                    sQuery = "update CreditNote set CreditInvoiceNo='" + tbcreditnoteno.Text.Trim() + "' where CreditNoteid=" + sCreditNoteId;


                    sqlCmd = new SqlCommand(sQuery, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlConn.Open();
                    RetVal = sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); RetVal = -1; }
                finally
                {
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
                if (RetVal > 0)
                {
                    //  Response.Write("<script>alert('Associated Invoice No Updated Successfully.');</script>");
                    GetDocumentDetails(invoiceID);
                    string message = "alert('Associated Invoice No Updated Successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
            else
            {
                string message = "alert('" + strMessage + "')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            //  Response.Write("<script>alert('" + strMessage + "');</script>");
        }
        #endregion

        #region IsValiedAssociatedInvoiceNo
        public int IsValiedAssociatedInvoiceNo()
        {
            int RetVal = 0;
            int ibuyerComId = 0;
            int iSuppliercomId = 0;
            Invoice_CN objInvoice_CN = new Invoice_CN();
            try
            {
                if (ViewState["SupplierCompanyID"] != null && ViewState["BuyerCompanyID"] != null)
                {
                    ibuyerComId = Convert.ToInt32(ViewState["BuyerCompanyID"].ToString());
                    iSuppliercomId = Convert.ToInt32(ViewState["SupplierCompanyID"].ToString());
                    if (tbcreditnoteno.Text != "")
                    {
                        RetVal = objInvoice_CN.CheckInvoiceExistsAgainstCompanyIDSAndInvoiceNo(tbcreditnoteno.Text.Trim(), ibuyerComId, iSuppliercomId);
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            return RetVal;
        }
        #endregion

        #region GetDepartMentDropDwons()
        public void GetDepartMentDropDwons()
        {
            CBSolutions.ETH.Web.ETC.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.ETC.ApprovalPath.Approval();
            DataSet ds = new DataSet();
            string Fields = "DepartmentID , Department";
            string Table = "Department";
            // string Criteria = "";
            string Criteria = "BuyerCompanyID = " + System.Convert.ToInt32(Session["BuyerCID"]);

            //if (Session["IsFromEDITDATAPage"] == "yes")
            //{

            //     Criteria = "BuyerCompanyID = " + System.Convert.ToInt32(Session["BuyerCompanyIdFromEditdata_CN"]);
            //}
            //else
            //{
            //     Criteria = "BuyerCompanyID = " + System.Convert.ToInt32(Session["InvoiceBuyerCompany"]);

            //}




            ds = objApproval.GetGlobalDropDowns(Fields, Table, Criteria);
            ddldept.DataSource = ds;
            ddldept.DataBind();
            ddldept.Items.Insert(0, "Select");
            DropDownList1.DataSource = ds;
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, "Select");

            //ADded by Mainak 2018-04-06
            chkDepartment.DataSource = ds;
            chkDepartment.DataBind();

            //added by kuntalkarar on 9thMArch2016
            ds = null;
            // finally
            //{
            //    ddldept.Items.Insert(0, "Select");
            //    ds = null;
            //}
        }
        #endregion

        #region Open
        //Added by Mainak 2018-08-06

        protected void btnOpenNew_Click(object sender, EventArgs e)
        {
            int deptID = 0;
            if (ddldept.SelectedItem.Text != "Select")
            {
                deptID = Convert.ToInt32(ddldept.SelectedValue);
            }
            else
            {
                deptID = Convert.ToInt32(ViewState["DepartmentID"]);
            }

            if (deptID != null)
            {
                bool ret = SaveDetailDataForGMG();

                //blocked by kuntalkarar on 29thJune2017
                //int i = SetDropDownValuesOnOpen_CRN(System.Convert.ToInt32(Session["UserID"].ToString()), deptID); //added by Rimi on 30.06.2015
                //added by kuntalkarar on 29thJune2017
                int i = SetDropDownValuesOnPressingopen_NEW(System.Convert.ToInt32(Session["UserID"]), 0);
                int DeptUpdate = UpdateDepartmentAgainstCreditNoteID();
                ViewState["MSG"] = "Open";// Added By Rimi on 24th July 2015
                MoveToNextInvoice();
                string message = "alert('Credit Note opened successfully.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

            }
            else
            {
                // Response.Write("<script>alert('Department has not been added.');</script>");
                string message = "alert('Department has not been added.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 5th August 2015
        }


        private int SetDropDownValuesOnPressingopen_NEW(int iUserID, int iChecked)
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
            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            //blocked by kuntalkarar on 30thJune2017
            //sqlCmd = new SqlCommand("sp_SetDropDownValuesOnPressingReopen_CreditNote", sqlConn);
            //Added by kuntalkarar on 30thJune2017
            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnPressingReopen_CreditNote_openButton", sqlConn);

            sqlCmd.CommandType = CommandType.StoredProcedure;

            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {

                if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                {
                    sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
                }
                else
                {
                    sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["CheckList"]));
                }

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

        #region Reopen
        //private void btnOpen_Click(object sender, System.EventArgs e)
        //{

        //    int deptID = 0;
        //    if (ddldept.SelectedItem.Text != "Select")
        //    {
        //        deptID = Convert.ToInt32(ddldept.SelectedValue);
        //    }
        //    else
        //    {
        //        deptID = Convert.ToInt32(ViewState["DepartmentID"]);
        //    }

        //    if(deptID!=null)

        //    {
        //        bool ret = SaveDetailDataForGMG();
        //        int i = SetDropDownValuesOnOpen_CRN(System.Convert.ToInt32(Session["UserID"].ToString()), deptID); //added by Rimi on 30.06.2015
        //        int DeptUpdate = UpdateDepartmentAgainstCreditNoteID();
        //        ViewState["MSG"] = "Open";// Added By Rimi on 22nd July 2015
        //        MoveToNextInvoice();
        //        string message = "alert('Credit Note opened successfully.')";
        //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

        //    }
        //    else
        //    {
        //        // Response.Write("<script>alert('Department has not been added.');</script>");
        //        string message = "alert('Department has not been added.')";
        //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
        //    }
        //    ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015
        //}






        //Function added by kuntalkarar on 1stMarch2016
        private void MoveToNextInvoiceFORReopenbutton()
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



                foreach (INVS str in (List<INVS>)Session["InvoiceID"])
                {
                    if (str.InvoiceID == Convert.ToString(InvoiceId))
                    {
                        strDocType = str.DocType;
                    }
                }


                if (strDocType == "INV")
                {
                    //26-06-20156
                    Session["IndexforINV"] = ViewState["Counter"];
                    Session.Add("creninv", InvoiceId);
                    //Response.Redirect("../Invoice/InvoiceActionTiffViewer.aspx");// Commenetd By Rimi on 22nd July 2015

                    // Added By Rimi on 22nd July 2015
                    if (ViewState["CancelFlag"] == "1")
                    {
                        Response.Redirect("../Invoice/InvoiceActionTiffViewer.aspx");
                        ViewState["CancelFlag"] = "2";
                    }
                    else
                    {
                        Response.Write("<script>opener.location.reload();</script>");
                        Response.Write("<script>self.close();</script>");
                        Response.Redirect("../Invoice/InvoiceActionTiffViewer.aspx?MsgFlag=1&MSG=" + ViewState["MSG"].ToString());
                    }
                    // Added By Rimi on 22nd July 2015

                }
                else
                {
                    ViewState["CheckList"] = InvoiceId;

                    dNetAmt = 0;

                    //========Added By Rimi on 8th Sept For Department - Approver Dropdowns issue=======
                    DataSet dsDept = new DataSet();
                    CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval();
                    string Fields = "CreditNote.DepartmentID,Department.Department";
                    string Table = "dbo.CreditNote INNER JOIN dbo.Department ON Department.DepartmentID=dbo.CreditNote.DepartmentID";
                    string Criteria = "CreditNote.CreditNoteID = " + System.Convert.ToInt32(InvoiceId);
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
                    //========Added By Rimi on 8th Sept For Department - Approver Dropdowns issue ENd=======
                    ViewState["approvalpath"] = "";
                    if (InvoiceId != 0)
                    {
                        GetDocumentDetails(InvoiceId);
                        /*
                        string strStatusLogLink = GetInvoiceStatusLog();
                        iframeInvoiceStatusLog.Attributes.Add("src", strStatusLogLink);
                     //   aInvoiceStatusLog.Attributes.Add("onclick", GetInvoiceStatusLog());
                         * 
                         */
                        DataSet ds = GetDocumentDetails(InvoiceId, "CRN");
                        Boolean Duplicate = Convert.ToBoolean(ds.Tables[0].Rows[0]["Duplicate"]);
                        if (Duplicate == false)
                        {
                            lblDuplicate.Visible = false;
                            tdDup.Style.Add("Display", "none");
                        }
                        else
                        {
                            lblDuplicate.Visible = true;
                            tdDup.Style.Add("Display", "");
                        }
                        //  string strStatusLogLink = GetInvoiceStatusLogNextInvoice(Convert.ToString(InvoiceId));

                        string strStatusLogLink = GetInvoiceStatusLog();
                        strStatusLogLink = "TINY.box.show({iframe:'" + strStatusLogLink + "',boxid:'frameless',width:530,height:350,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){closeJS()}})";
                        //aInvoiceStatusLog.Attributes.Add("onclick", strStatusLogLink);

                        InvoiceCrnIsDuplicate();
                        IsAutorisedtoEditData();
                    }

                    GetDepartMentDropDwons();
                    CheckNextInvoiceExist(InvoiceId);
                    //  CheckInvoiceExist();

                    CalculateTotal();
                    GetVatAmount();

                    //GetVatAmount();


                    if (TypeUser >= 2 && Convert.ToInt32(ViewState["StatusID"]) == 20)
                        lblDepartment.Visible = false;
                    //modified by Subhrajyoti on 27.03.2015
                    if (TypeUser < 1)
                    {
                        tbcreditnoteno.Visible = false;
                        btnEditAssociatedInvoiceNo.Visible = false;
                    }
                    //modified by Subhrajyoti on 27.03.2015
                    ShowFiles(Convert.ToInt32(InvoiceId));
                    ButtonRejectVisibility();
                    string TiffUrl = "../../TiffViewerDefault.aspx?ID=" + InvoiceId.ToString() + "&Type=" + "CRN";
                    TiffWindow.Attributes.Add("src", TiffUrl);

                    txtComment.Text = "";

                }
                //===============Added By Subhrajyoti on 3rd August 2015===========================
                DataSet dss = GetDocumentDetails(Convert.ToInt32(InvoiceId), "CRN");
                Int32 StatusId = Convert.ToInt32(dss.Tables[0].Rows[0]["StatusId"]);

                if (Convert.ToInt32(StatusId) != 20 && Convert.ToInt32(StatusId) != 21 && Convert.ToInt32(StatusId) != 22 && Convert.ToInt32(StatusId) != 6)
                {
                    Response.Redirect("../Current/CRNclosewindows.aspx");
                }
                //addded by kuntalkarar on 1stMarch2016
                // Session["eInvoiceID"] = InvoiceId;
                //Session["eInvoiceID_Reopenbtn"] = InvoiceId;
                //===============Added By Subhrajyoti on 3rd August  2015===========================

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                Response.Write("<script> parent.window.close();</script>");
            }




        }


















        private void btnOpen_Click(object sender, System.EventArgs e)
        {
            Session["button_clicked_Creditenote"] = "1";//added by kuntalkarar on 20thOctober2016
            //added by kuntalkarar on9thMarch2016
            //Session["IsFromEDITDATAPage"] = "No";


            int DeptUpdate = UpdateDepartmentAgainstInvoiceID();
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


                ViewState["MSG"] = "ReOpen";// Added By Rimi on 22nd July 2015

                //blocked by kuntalkarar on 1stMarch2016
                MoveToNextInvoice();
                //Added by kuntalkarar on 1stMarch2016
                //MoveToNextInvoiceFORReopenbutton();  

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
            //Added by kuntalkarar on 20thOctober2016
            LoadDownloadFiles();
        }

        //======================Added By Rimi on 3rd December 2015========================================
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
            //if (Convert.ToString(ddlApprover3.SelectedItem.Text) != "Select")
            //    NewApprover3 = Convert.ToString(ddlApprover3.SelectedItem.Text);
            //if (Convert.ToString(ddlApprover4.SelectedItem.Text) != "Select")
            //    NewApprover4 = Convert.ToString(ddlApprover4.SelectedItem.Text);
            //if (Convert.ToString(ddlApprover5.SelectedItem.Text) != "Select")
            //    NewApprover5 = Convert.ToString(ddlApprover5.SelectedItem.Text);


            //Addedby kuntalkarar on 1stMarch2016
            //Session["eInvoiceID"] = Convert.ToString(Session["eInvoiceID_Reopenbtn"]);


            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnPressingReopen_CreditNote", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            if (Convert.ToInt32(ViewState["InvoiceChecking"]) == 0)
            {
                //blocked by kuntalkarar on 1stMarch2016--------------------------
                //sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
                //added by kuntalkarar on 1stMarch2016----------------------------
                if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                {
                    sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
                }
                else
                {
                    sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["CheckList"]));
                }
                //----------------------------------------------------------------
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
        //======================Added By Rimi on 3rd December 2015 End====================================
        #endregion

        #region SetDropDownValuesOnOpen_CRN
        private int SetDropDownValuesOnOpen_CRN(int iUserID, int DepartmentID)
        {

            int iReturnValue = 0;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            SqlCommand sqlCmd = null;
            SqlParameter sqlOutputParam = null;

            sqlCmd = new SqlCommand("sp_SetDropDownValuesOnOpen_CRN_ETC", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
            }
            else
            {
                sqlCmd.Parameters.Add("@InvoiceID", Convert.ToInt32(ViewState["CheckList"]));
            }

            sqlCmd.Parameters.Add("@UserID", iUserID);
            if (txtComment.Text == "")
                sqlCmd.Parameters.Add("@Comment", DBNull.Value);
            else
                sqlCmd.Parameters.Add("@Comment", txtComment.Text.Trim());

            sqlCmd.Parameters.Add("@Description", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover1", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover2", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover3", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover4", DBNull.Value);
            sqlCmd.Parameters.Add("@NewApprover5", DBNull.Value);
            sqlCmd.Parameters.Add("@Department", DepartmentID);
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

        #region SaveDetailDataForGMG()

        private bool SaveDetailDataForGMG()
        {
            int InvID = 0;
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            }
            else
            {
                InvID = Convert.ToInt32(ViewState["CheckList"]);
            }

            int CompanyID = 0;
            int CodingDescriptionID = 0;
            int NominalCodeID = 0;
            int BusinessUnitID = 0;
            int DepartmentID = 0;
            decimal NetValue = 0;
            bool flag = false;
            double NetVal = 0;

            string PurOrderNo = "";

            // Added by Mrinal on 19th March 2015

            decimal LineVAT = 0;
            string strLineDescription = string.Empty;

            // Addition End on 19th March 2015 


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
            // Added by Mrinal On 19th March 2015

            dtXML.Columns.Add("LineVAT");
            dtXML.Columns.Add("LineDescription");

            // Addition End On 19th March 2015 
            DataRow DR = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            NetVal = Math.Round(NetVal, 2);
            ViewState["NetValINV"] = NetVal;
            ViewState["NetAmt"] = NetVal;
            if (Convert.ToDouble(ViewState["NetAmt"]) == NetVal)
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
                        //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                        //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                        //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
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
                        //    DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
                        //}
                        ViewState["vDepartmentID"] = DepartmentID;
                        //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                        //{
                        CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                        //}
                        //else
                        //{
                        //    CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();

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
                    // Added by Mrinal on 19th March 2015
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
                    // Addition End on 19th March 2015 


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
                        sb.Append("<InvoiceType>").Append("CRN").Append("</InvoiceType>");
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
                        // Added by Mrinal on 19th March 2015
                        sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
                        sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");
                        // Addition End on 19th March 2015 

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
                {

                    retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);




                }
            }
            else
            {
                flag = false;
                lblErrorMsg.Visible = false;
                // Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
                Response.Write("<script>alert('Variance must be zero.');</script>");
            }
            return flag;
        }

        #endregion

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
            //    //ddlProject1=((DropDownList) grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
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
            //        //					string strProjectName="";
            //        //					string strProjectID="";
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


        #region: Add by Mrinal on 5th March 2013
        private void btnReject_Click(object sender, EventArgs e)
        {
            Session["button_clicked_Creditenote"] = "1";//added by kuntalkarar on 20thOctober2016
            //added by kuntalkarar on 9thMarch2016
            Session["IsFromEDITDATAPage"] = "No";


            //===============Added By Rimi on 7th Sept 2015===========================

            if (Convert.ToInt32(Session["UserTypeID"]) == 1)
            {
                Boolean res1;
                if (Convert.ToInt64(ViewState["CheckList"]) == 0)
                {
                    res1 = CheckPassToUserID(Convert.ToString(Session["eInvoiceID"]));

                }
                else
                {
                    res1 = CheckPassToUserID(Convert.ToString(ViewState["CheckList"]));

                }
                if (res1 == false)
                {
                    Response.Redirect("../Current/CRNclosewindows.aspx");
                }
            }
            //===============Added By Rimi on 7th Sept 2015===========================
            if (!(txtComment.Text.Trim().Length > 0))
            {
                //  Response.Write("<script>alert('Please enter comments.');</script>");

                string message = "alert('Please enter comments.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                return;
            }
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(ConsString);
            int CreditNoteID = 0;
            if (Session["eInvoiceID"] != null)
            {
                CreditNoteID = System.Convert.ToInt32(Session["eInvoiceID"]);
            }
            if (CreditNoteID != 0 && Session["eInvoiceID"] != null)
            {
                try
                {
                    //sqlCmd = new SqlCommand("sp_SetCreditNoteStatus", sqlConn);
                    sqlCmd = new SqlCommand("sp_SetCreditNoteStatus_JKS", sqlConn);


                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                    {

                        sqlCmd.Parameters.Add("@CreditNoteID", CreditNoteID);
                    }
                    else
                    {
                        sqlCmd.Parameters.Add("@CreditNoteID", Convert.ToInt32(ViewState["CheckList"]));

                    }
                    sqlCmd.Parameters.Add("@Comments", txtComment.Text);
                    if (Session["UserID"] == null)
                    {
                        sqlCmd.Parameters.Add("@UserID", DBNull.Value);
                    }
                    else
                    {
                        sqlCmd.Parameters.Add("@UserID", Convert.ToString(Session["UserID"]));
                    }
                    // sqlCmd.Parameters.Add("@CompanyID", Convert.ToInt32(Session["CompanyID"]));
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();



                    ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 28th July 2015
                    ViewState["RejectFlag"] = "yes";// Added By Rimi on 22nd July 2015
                    ViewState["MSG"] = "Reject";// Added By Rimi on 22nd July 2015
                    MoveToNextInvoice();
                    string message = "alert('Credit Note rejected successfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();

                }
                finally
                {
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }

            }
            //Added by kuntalkarar on 20thOctober2016
            LoadDownloadFiles();
            ViewState["hdnDepartmentCodeID"] = "2";//Added By Rimi on 4th August 2015
        }

        private void ButtonRejectVisibility()
        {
            int TypeUser = Convert.ToInt32(Session["UserTypeID"]);
            btnOpenNew.Visible = false;//Added by Mainak 2018-08-06
            if (TypeUser == 2 || TypeUser == 3)
            {

                //Added by Kuntalkarar on 29thFeb2016-------------------------------
                if (Convert.ToInt32(ViewState["StatusID"]) == 6 || Convert.ToInt32(ViewState["StatusID"]) == 22 || Convert.ToInt32(ViewState["StatusID"]) == 21)  //Credit notes in  Rejected & Re-Opened & Registered  status
                {
                    tr_ReopenAtApprover1.Visible = true;
                }
                else
                {
                    tr_ReopenAtApprover1.Visible = true;
                }
                //-----------------------------------------------------------------
                //statusId=22 added by kuntalkarar on 2ndMarch2016
                if (Convert.ToInt32(ViewState["StatusID"]) == 22 || Convert.ToInt32(ViewState["StatusID"]) == 21) //Credit notes in Registered & Received status
                {
                    btnApprove.Visible = false;
                }


                if (Convert.ToInt32(ViewState["StatusID"]) == 21)  //Credit notes in  Registered  status
                {
                    btnOpen.Visible = true;
                    RejectOpenFields = 1;

                }

                //added by kuntalkarar on 24thJanaury2017
                if (Convert.ToInt32(ViewState["StatusID"]) == 6)
                {
                    btnOpen.Visible = true;
                    btnApprove.Visible = false;//Added by Mainak 2018-07-28

                }
                if (Convert.ToInt32(ViewState["StatusID"]) == 20)
                {
                    btnApprove.Visible = true;
                    btnOpen.Visible = false;
                    btnOpenNew.Visible = true;//added by Mainak 2018-08-06
                }
                //----------------------------------------


                if (Convert.ToInt32(ViewState["StatusID"]) == 22)  //Credit notes in Reopen status
                {
                    //blocked by kuntalkarar on 2ndMarch2016
                    //btnOpen.Visible = false;
                    btnOpen.Visible = true;
                }




            }

            else
                //Added by Kuntalkarar on 29thFeb2016-------------------------------
                tr_ReopenAtApprover1.Visible = false;
            //--------------------------------------------------------------------
            if (TypeUser == 1)
            {
                btnOpen.Visible = false;
                //btnApprove.Visible = false;
                if (Convert.ToInt32(ViewState["StatusID"]) == 21 || Convert.ToInt32(ViewState["StatusID"]) == 22)  // When Registered or reopned
                {
                    btnReject.Visible = true;
                }

                else
                {
                    btnReject.Visible = false;
                }
                //Added by Kuntalkarar on 29thFeb2016-------------------------------
                tr_ReopenAtApprover1.Visible = false;
                //------------------------------------------------------------------

            }
            else
            {
                btnReject.Visible = false;
            }

            if (TypeUser == 1)
            {
                ////Added by Kuntalkarar on 29thFeb2016-------------------------------
                //tr_ReopenAtApprover1.Visible = false;
                ////------------------------------------------------------------------
                //btnOpen.Visible = false;
            }

            if (Convert.ToInt32(ViewState["StatusID"]) == 20)
            {
                //Modified  by Mainak 2018-07-10
                btnApprove.Visible = false;
                //Modified  by Mainak 2018-08-08
                btnOpen.Visible = false;
                btnOpenNew.Visible = true;
            }
            //btnSubmit.Visible = true;//For test

        }
        #endregion
        private string GetPONumberForSupplierBuyer(string Ponumber)
        {
            string existscheck = "";
            int invoiceID = System.Convert.ToInt32(Request.QueryString["InvoiceID"]);
            DataSet Dst = new DataSet();
            SqlDataAdapter sqlDA = null;
            SqlConnection sqlConn = null;
            sqlConn = new SqlConnection(ConsString);
            sqlDA = new SqlDataAdapter("sp_PoNumberForSupplerAnainstBuyerAkkeron", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", invoiceID);
            sqlDA.SelectCommand.Parameters.Add("@PoNumber", Ponumber);
            sqlDA.SelectCommand.Parameters.Add("@Type", "CRN");


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
        protected string GetTiffViewersURL(object InvoiceID, object oDocType)
        {
            string strInvoiceID = Convert.ToString(InvoiceID);
            string strDocumentType = Convert.ToString(oDocType);

            string strURL = "";
            strURL = "javascript:window.open('../../TiffViewerDefault.aspx?ID=" + strInvoiceID + "&Type=" + strDocumentType + "','TiffViewer','width=650,height=450,top=100,left=150,scrollbars=1,resizable=1');";

            return (strURL);
        }

        protected void GetURLTest()
        {
            int RowID = 0;
            if (Request.QueryString["RowID"] != null)
            {
                RowID = System.Convert.ToInt32(Request.QueryString["RowID"]);
                Session["RowID"] = Convert.ToString(RowID);
            }
            Session["IsProcessed"] = "1";
            Response.Write("<script>parent.window.close();</script>");
        }
        #endregion

        protected string GetInvoiceStatusLog()
        {


            //string strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);
            string strInvoiceID = "";
            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);

            }
            else
            {
                strInvoiceID = Convert.ToString(ViewState["CheckList"]);

            }


            string strURL = "";
            // strURL = "javascript:window.open('../../JKS/invoice/InvoiceStatusLogNL.aspx?InvoiceID=" + strInvoiceID + "','InvoiceStatusLogNL','width=300,height=250,scrollbars=1');";
            strURL = "InvoiceStatuslogNL_CN.aspx?IsHover=1&InvoiceID=" + strInvoiceID;
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

            string strInvoiceID = "";

            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);

            }
            else
            {
                strInvoiceID = Convert.ToString(ViewState["CheckList"]);

            }
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_CheckInvoiceCrnIsDuplicate", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@InvoiceID", strInvoiceID);
            sqlDA.SelectCommand.Parameters.Add("@DocType", "CRN");
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int IsDuplicate = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                    if (IsDuplicate > 0)
                    {
                        lblDuplicate.Visible = true;
                    }
                    else
                    {
                        lblDuplicate.Visible = false;
                    }
                }
                else
                {
                    lblDuplicate.Visible = false;
                }
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }
        protected void IsAutorisedtoEditData()
        {
            string strInvoiceID = "";
            if (Session["UserID"] != null)
            {

                if (Convert.ToInt32(ViewState["CheckList"]) == 0)
                {

                    strInvoiceID = Convert.ToString(Request.QueryString["InvoiceID"]);

                }
                else
                {
                    strInvoiceID = Convert.ToString(ViewState["CheckList"]);

                }





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
                            //--------------modified by kuntal Karar on 3rd April 2015-----pt(60)--------------

                            // aEditData.Attributes.Add("href", "InvoiceConfirmationNL_CN.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1");
                            aEditData.Attributes.Add("onclick", "javascript:window.open('InvoiceEdit_CN.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1', 'CustomPopUp','width=1200, height=650,scrollbars=1,resizable=1');return false;");//"href", "InvoiceEdit_CN.aspx?InvoiceID=" + strInvoiceID + "&AllowEdit=Current&IsParentReload=1,'width=1200,height=650,top=100,left=150,scrollbars=1,resizable=1'");
                            //-----------------------------------------------------------------------------------------

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
                    // ((System.Web.UI.WebControls.TextBox)ri.FindControl("txtNetVal")).Text = dNetAmt.ToString("0.00");// Added By Rimi on 22nd July 2015
                }
            }

            lblNetVal.Text = dNetAmt.ToString("0.00");
            if (ViewState["NetAmt"] != null)
            {
                //lblNetInvoiceTotal.Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString("0.00");// commented by Rimi on 26.06.2015
                //dVariance = Convert.ToDouble(ViewState["NetAmt"].ToString()) - dNetAmt;// commented by Rimi on 26.06.2015
                lblNetInvoiceTotal.Text = Convert.ToDouble(ViewState["CodingValue"].ToString()).ToString("0.00");// Added by Rimi on 26.06.2015
                dVariance = Convert.ToDouble(ViewState["CodingValue"].ToString()) - dNetAmt;// Added by Rimi on 26.06.2015
            }
            else
            {
                dVariance = dNetAmt * (-1);
            }
            lblVariance.Text = dVariance.ToString("0.00");

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
            string sSql = "SELECT  CONVERT(DECIMAL(18,2),TotalAmt) As TotalAmt ,CONVERT(DECIMAL(18,2),VATAmt) As VATAmt  ,CurrencyCode,CreditNote.CurrencyTypeID ,CurrencyName,CONVERT(DECIMAL(18,2),NetTotal) As NetAmt FROM    CreditNote  LEFT JOIN CurrencyTypes ON dbo.CreditNote.CurrencyTypeID = dbo.CurrencyTypes.CurrencyTypeID WHERE   CreditNoteID=" + InvoiceID;
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

        //private void GetVatAmount()
        //{
        //    int InvoiceID = 0;
        //    double dVariance = 0;
        //    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
        //    {
        //        InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
        //    }
        //    else
        //    {
        //        InvoiceID = Convert.ToInt32(Convert.ToInt32(ViewState["CheckList"]));

        //    }


        //    double TotalAmt = 0;
        //    double VATAmt = 0;
        //    string strCurrencyCode = string.Empty;
        //    double NetAmt=0;
        //    string sSql = "SELECT  CONVERT(DECIMAL(18,2),TotalAmt) As TotalAmt ,CONVERT(DECIMAL(18,2),VATAmt) As VATAmt  ,CurrencyCode,CreditNote.CurrencyTypeID ,CurrencyName,CONVERT(DECIMAL(18,2),NetTotal) As NetAmt FROM    CreditNote  INNER   JOIN CurrencyTypes ON dbo.CreditNote.CurrencyTypeID = dbo.CurrencyTypes.CurrencyTypeID WHERE   CreditNoteID=" + InvoiceID;
        //    SqlDataReader dr = null;
        //    SqlConnection sqlConn = new SqlConnection(ConsString);


        //    SqlCommand sqlCmd = new SqlCommand(sSql, sqlConn);
        //    try
        //    {
        //        sqlConn.Open();
        //        dr = sqlCmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            if (dr[0] != DBNull.Value)
        //            {
        //                TotalAmt = Convert.ToDouble(dr[0]);
        //                TotalAmt = Convert.ToDouble(TotalAmt.ToString("0.00"));
        //            }
        //            if (dr[1] != DBNull.Value)
        //            {
        //                VATAmt = Convert.ToDouble(dr[1]);
        //                VATAmt = Convert.ToDouble(VATAmt.ToString("0.00"));
        //            }
        //            if (dr[2] != DBNull.Value)
        //            {
        //                strCurrencyCode = Convert.ToString(dr[2]);
        //            }

        //             if (dr[2] != DBNull.Value)
        //            {
        //                strCurrencyCode = Convert.ToString(dr[2]);
        //            }

        //                if (dr[3] != DBNull.Value)
        //            {
        //                NetAmt = Convert.ToDouble(dr[5]);
        //            }
        //        }
        //        // lblVat.Text = VATAmt.ToString("F", System.Globalization.CultureInfo.InvariantCulture); //VATAmt.ToString();
        //        //  lblTotal.Text = TotalAmt.ToString();
        //        lblNetInvoiceTotal.Text = NetAmt.ToString("0.00");  //Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString("0.00");
        //        lblVat.Text = VATAmt.ToString("0.00");
        //        lblTotal.Text = TotalAmt.ToString("0.00");
        //        lblCurrencyCode.Text = strCurrencyCode.ToString();
        //        lblNetVal.Text = NetAmt.ToString("0.00");
        //    //    lblTotalCodingVAT.Text = Convert.ToDouble(ViewState["Codingvat"]).ToString("0.00");
        //        lblVATVariance.Text = (VATAmt - Convert.ToDouble(ViewState["Codingvat"])).ToString("0.00");
        //        lblVariance.Text = (Convert.ToDouble(lblNetInvoiceTotal.Text) - Convert.ToDouble(lblNetVal.Text)).ToString("0.00");


        //        //lblNetVal.Text = dNetAmt.ToString("0.00");
        //        //if (ViewState["NetAmt"] != null)
        //        //{
        //        //    lblNetInvoiceTotal.Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString("0.00");
        //        //    dVariance = Convert.ToDouble(ViewState["NetAmt"].ToString()) - dNetAmt;
        //        //}
        //        //else
        //        //{
        //        //    dVariance = dNetAmt * (-1);
        //        //}
        //        //lblVariance.Text = dVariance.ToString("0.00");

        //        //// Added by Mrinal On 19th March 2015
        //        lblTotalCodingVATValue.Text = dCodingVat.ToString("0.00");

        //        //double dTotalVat = 0;
        //        //dTotalVat = Convert.ToDouble(lblVat.Text);
        //        //lblVATVariance.Text = (dTotalVat - dCodingVat).ToString("0.00");
        //    }
        //    catch (Exception ex) { string ss = ex.Message.ToString(); }
        //    finally
        //    {
        //        dr.Close();
        //        sqlCmd.Dispose();
        //        sqlConn.Close();
        //    }

        //    // return NetAmt;
        //}
        protected void txtNetVal_TextChanged(object sender, EventArgs e)
        {
            TextBox txtProcess = (TextBox)sender;
            //--------------------added by kuntalkarar on 13thSeptember2016--------------------------
            if (txtProcess.Text == "0.00")//for <>0.00 to 0.00
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
            //^\d{1,16}((\.\d{1,4})|(\.))?$");
            Regex rg = new Regex(@"(^-?0\.[0-9]*[1-9]+[0-9]*$)|(^-?[1-9]+[0-9]*((\.[0-9]*[1-9]+[0-9]*$)|(\.[0-9]+)))|(^-?[1-9]+[0-9]*$)|(^0$){1}");

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

            //dNetAmt = 0;
            //for (int i = 0; i <= grdList.Items.Count - 1; i++)
            //{

            //    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim() != "")
            //    {
            //        dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text.Trim());
            //    }
            //}
            //CalculateTotal();
        }
        #region: btnSaveLine_Click
        public void btnSaveLine_Click(object sender, EventArgs e)
        {
            //  throw new NotImplementedException();
            // SaveDetailData();
            GetDepartMentDropDwons(); // Added by koushik das (kd) on 22-01-2019 for prorate
            if (SaveLineData())
            {


                //string msg = "Lines Data Save Successfully.";
                //string msg = "Coding saved successfully.";
                //Page.RegisterStartupScript("Reg", "<script>AutoCloseAlert('" + msg + "');</script>");
                GetDepartMentDropDwons(); // Added by koushik das (kd) on 24-01-2019 for prorate
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

        //====================Commented By Rimi on 21st August 2015=======================
        //private bool SaveLineData()
        //{
        //    int InvID =0;

        //    #region variables

        //    if (Convert.ToInt32(ViewState["CheckList"]) == 0)
        //    {
        //        InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
        //    }
        //    else
        //    {
        //        InvID = Convert.ToInt32(ViewState["CheckList"]);
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
        //    // Added by Mrinal on 19th March 2015
        //        decimal LineVAT = 0;
        //        string strLineDescription = string.Empty;
        //    // Addition End on 19th March 2015 


        //    bool retval = false;
        //    lblErrorMsg.Visible = false;
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
        //    // Added by Mrinal On 19th March 2015
        //    dtXML.Columns.Add("LineVAT");
        //    dtXML.Columns.Add("LineDescription");
        //    // Addition End On 19th March 2015 
        //    #endregion
        //    DataRow DR = null;
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<Root>");

        //    for (int i = 0; i <= grdList.Items.Count - 1; i++)
        //    {


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


        //        //if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
        //        //{
        //        //    CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
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
        //            //===================Commeneted By Rimi on 27th July 2015==========
        //            //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //            //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //            //ViewState["vDepartmentID"] = DepartmentID;

        //            //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //            //===================Commeneted By Rimi on 27th July 2015 End==========================


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
        //                DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
        //                ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
        //            }
        //            ViewState["vDepartmentID"] = DepartmentID;
        //            if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
        //            {
        //                CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //            }
        //            else
        //            {
        //                CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
        //                ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();

        //            }

        //            //====================Modified By Rimi on 27th July 2015 End======================

        //            ViewState["vCodingDescriptionID"] = CodingDescriptionID;
        //            // Added by Mrinal on 16th March 2015
        //            //if (i == 0)
        //            //{
        //            //    Line1CodingDescriptionID = CodingDescriptionID;
        //            //    Line1DepartmentID = DepartmentID;
        //            //}
        //            // Addition End
        //        }

        //        else
        //        {
        //            NominalCodeID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
        //            DepartmentID = 0;//Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
        //            CodingDescriptionID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
        //            // Added by Mrinal on 16th March 2015
        //            //if (i == 0)
        //            //{
        //            //    Line1CodingDescriptionID = 0;
        //            //    Line1DepartmentID = 0;
        //            //}

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
        //                NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
        //            //}
        //        }
        //         ViewState["NetAmt"] = NetValue; // Added by Rimi on 01.07.2015

        //        // Added by Mrinal on 19th March 2015
        //        LineVAT = 0;

        //        if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
        //        {
        //            //modified by kuntal karar--on-25.03.2015--pt.48------------------
        //            //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
        //            //{
        //                LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
        //            //}
        //            //----------------------------------------------------------------
        //        }
        //        strLineDescription = string.Empty;

        //        if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
        //        {
        //            strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
        //        }
        //         //Added By RImi on 8th August 2015

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
        //        // Addition End on 19th March 2015 


        //        #endregion


        //        //if (NetValue > 0 || (Convert.ToDecimal(Request.QueryString["iVat"]) > 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > 0))
        //        if (NetValue > -1 || (Convert.ToDecimal(Request.QueryString["iVat"]) > -1 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))
        //        {
        //            DR = dtXML.NewRow();
        //            dtXML.Rows.Add(DR);
        //            sb.Append("<Rowss>");
        //            sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
        //            sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
        //            sb.Append("<InvoiceType>").Append("CRN").Append("</InvoiceType>");
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
        //            // Added by Mrinal on 19th March 2015
        //            sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
        //            sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");
        //            // Addition End on 19th March 2015 
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
        //        retval = true;
        //    }
        //    else
        //        retval = false;


        //    return retval;
        //}


        //=============== Added By RImi on 21st August 2015====================================


        private bool SaveLineData()
        {
            int InvID = 0;

            #region variables

            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {
                InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            }
            else
            {
                InvID = Convert.ToInt32(ViewState["CheckList"]);
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
            // Added by Mrinal on 19th March 2015
            decimal LineVAT = 0;
            string strLineDescription = string.Empty;
            // Addition End on 19th March 2015 


            bool retval = false;
            lblErrorMsg.Visible = false;
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
            // Added by Mrinal On 19th March 2015
            dtXML.Columns.Add("LineVAT");
            dtXML.Columns.Add("LineDescription");
            // Addition End On 19th March 2015 
            #endregion
            DataRow DR = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");

            for (int i = 0; i <= grdList.Items.Count - 1; i++)
            {


                if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString()) != "--Select--" && Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
                {
                    CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                }
                else
                {
                    Response.Write("<script>alert('Please select Company');</script>");
                    return false;
                }


                //if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
                //{
                //    CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                //}

                if (((TextBox)grdList.Items[i].FindControl("txtAutoCompleteCodingDescription")).Text.Trim().Length > 0)
                {
                    //======Commented By Rimi on 28th July 2015
                    //NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                    //DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                    //ViewState["vDepartmentID"] = DepartmentID;

                    //CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);

                    //=====================Modified By Rimi on 3rd August 2015=============================
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
                        NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                        ViewState["hdnDepartmentCodeID"] = "2";
                        //}

                        //else
                        //{
                        // NominalCodeID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnNominalCodeID")).Value);
                        ((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value = NominalCodeID.ToString();
                        //ViewState["hdnDepartmentCodeID"] = "1";
                        // }
                        //----ENDS-------------blocked by kuntal karar on 17thAugust2015 -----------------------------------------------------
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
                    //    DepartmentID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnDepartmentCodeID")).Value);
                    ((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value = DepartmentID.ToString();
                    //}
                    ViewState["vDepartmentID"] = DepartmentID;
                    //---------------blocked by kuntal karar on 17thAugust2015--------------
                    //if (Convert.ToString(ViewState["hdnDepartmentCodeID"]) != "1")
                    //{
                    CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                    //}
                    //else
                    //{
                    //    CodingDescriptionID = Convert.ToInt32(((HiddenField)grdList.Items[i - 1].FindControl("hdnCodingDescriptionID")).Value);
                    ((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value = CodingDescriptionID.ToString();

                    // }
                    //-------ENDS------blocked by kuntal karar on 17thAugust2015------------------------------------------------------------------



                    //====================Modified By Rimi on 3rd August 2015 End======================

                    ViewState["vCodingDescriptionID"] = CodingDescriptionID;
                    // Added by Mrinal on 16th March 2015
                    //if (i == 0)
                    //{
                    //    Line1CodingDescriptionID = CodingDescriptionID;
                    //    Line1DepartmentID = DepartmentID;
                    //}
                    // Addition End
                }

                else
                {
                    NominalCodeID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnNominalCodeID")).Value);
                    DepartmentID = 0;//Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnDepartmentCodeID")).Value);
                    CodingDescriptionID = 0;// Convert.ToInt32(((HiddenField)grdList.Items[i].FindControl("hdnCodingDescriptionID")).Value);
                    // Added by Mrinal on 16th March 2015
                    //if (i == 0)
                    //{
                    //    Line1CodingDescriptionID = 0;
                    //    Line1DepartmentID = 0;
                    //}

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
                ViewState["NetAmt"] = NetValue.ToString("0.00"); // Added by Rimi on 01.07.2015

                // Added by Mrinal on 19th March 2015
                LineVAT = 0;

                if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text != "")
                {
                    //modified by kuntal karar--on-25.03.2015--pt.48------------------
                    //if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text) > 0)
                    //{
                    LineVAT = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtLineVAT")).Text);
                    //}
                    //----------------------------------------------------------------
                }
                strLineDescription = string.Empty;

                if (((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text.Trim().Length > 0)
                {
                    strLineDescription = Convert.ToString(((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text);
                }
                // Addition End on 19th March 2015 
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


                //if (NetValue > 0 || (Convert.ToDecimal(Request.QueryString["iVat"]) > 0 && Convert.ToDecimal(Request.QueryString["iGross"]) > 0))
                if (NetValue > -1 || (Convert.ToDecimal(Request.QueryString["iVat"]) > -1 && Convert.ToDecimal(Request.QueryString["iGross"]) > -1))
                {
                    DR = dtXML.NewRow();
                    dtXML.Rows.Add(DR);
                    sb.Append("<Rowss>");
                    sb.Append("<SlNo>").Append(Convert.ToString(i + 1)).Append("</SlNo>");
                    sb.Append("<InvoiceID>").Append(Convert.ToString(InvID)).Append("</InvoiceID>");
                    sb.Append("<InvoiceType>").Append("CRN").Append("</InvoiceType>");
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
                    // Added by Mrinal on 19th March 2015
                    sb.Append("<LineVAT>").Append(Convert.ToString(LineVAT)).Append("</LineVAT>");
                    sb.Append("<LineDescription>").Append(Convert.ToString(strLineDescription)).Append("</LineDescription>");
                    // Addition End on 19th March 2015 
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


            return retval;
        }




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
            //modified by kuntal karar----on-25thMAR2015--pt.48----
            //^\d{1,16}((\.\d{1,4})|(\.))?$;
            Regex rg = new Regex(@"(^-?0\.[0-9]*[1-9]+[0-9]*$)|(^-?[1-9]+[0-9]*((\.[0-9]*[1-9]+[0-9]*$)|(\.[0-9]+)))|(^-?[1-9]+[0-9]*$)|(^0$){1}");
            //-----------------------------------------------------
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

            //if (dNetVariance > 0 || dVatVariance > 0)
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

        //Added by Mainak 2018-04-14
        #region btnProrateSubmit_Click
        protected void btnProrateSubmit_Click(object sender, System.EventArgs e)
        {
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
                    }
                }
                // count = count;

                DataSet ds = ((DataSet)ViewState["populate"]);
                int CodingDescriptionID = 0;
                if (ds.Tables[1].Rows[0]["CodingDescriptionID"].ToString() != "")
                {
                    CodingDescriptionID = Convert.ToInt32(ds.Tables[1].Rows[0]["CodingDescriptionID"]);
                }

                if (ds.Tables[1].Rows.Count > 0 && count > 0)
                {
                    DataSet dCD = new DataSet();
                    decimal netValue = Convert.ToDecimal(ViewState["CodingValue"]) / count;
                    decimal vat = Convert.ToDecimal(ViewState["lblTotalCodingVAT"]) / count;
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
                        if (dCD.Tables[0].Rows.Count > 0)
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
                // Added by koushik das (kd) on 30-01-2019 for prorate
                for (int i = 0; i < tbl.Rows.Count; i++)
                {

                    ((TextBox)grdList.Items[i].FindControl("txtLineDescription")).Text = Session["lineDescription"].ToString();

                }
          
            }
            catch
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

            Session["button_clicked_Creditenote"] = "1";
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
            iCount = objCN.checkInvoiceCrditNoteIDExist(Convert.ToInt32(InvoiceID_Remtach), "CRN");
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
            Session["button_clicked_Creditenote"] = "1";
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

            SqlCommand sqlCmd = new SqlCommand("sp_ButtonReleasePress_GenericCRN", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CreditNoteID", InvoiceID_Remtach);
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
            string sSql = "SELECT * FROM CreditNote WHERE CreditNoteID=" + iInvoiceID;
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

            Session["button_clicked_Creditenote"] = "1";//added by kuntalkarar on 20thOctober2016
            //added by kuntalkarar on 9thMarch2016
            Session["IsFromEDITDATAPage"] = "No";

            Int32 Invoiceid = 0;

            if (Convert.ToInt32(ViewState["CheckList"]) == 0)
            {

                Invoiceid = Convert.ToInt32(Request.QueryString["InvoiceID"]);

            }
            else
            {
                Invoiceid = Convert.ToInt32(ViewState["CheckList"]);

            }
            if (CheckVarience())
            {
                if (vRFlag == 0)
                {
                    bool ret = SaveDetailData();
                    if (ret == true)
                    {
                        //int InvID = Convert.ToInt32(Session["eInvoiceID"]);
                        int ival = ChangeStatus(Invoiceid, Convert.ToString(txtComment.Text), 19);
                        if (ival > 0)
                        {
                            ViewState["MSG"] = "Approve";// Added By Rimi on 22nd July 2015
                            MoveToNextInvoice();
                            string message = "alert('Credit Note Approved Successfully.')";
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
                Response.Write("<script>alert('Variance must be zero.');</script>");
            }

            overlayApprove.Visible = false;
            dialogApprove.Visible = false;

        }

        protected void btnApproveCancel_Click(object sender, EventArgs e)
        {
            overlayApprove.Visible = false;
            dialogApprove.Visible = false;
        }

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

        // Added by koushik das (kd) on 24-01-2019 for prorate
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
            sqlDA.SelectCommand.Parameters.Add("@Type", "CRN");
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
        // Added by koushik das (kd) on 24-01-2019 for prorate
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