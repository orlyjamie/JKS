using System;
using System.Collections;
using System.Configuration;
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
using System.Data.SqlClient;
using System.IO;
using System.Web.Mail;

namespace JKS
{

    public class CurrentAction : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WEBCONTROLS

        protected System.Web.UI.WebControls.Label lbldocumentdate;
        protected System.Web.UI.WebControls.Label lblsuppliername;
        protected System.Web.UI.WebControls.Label lbldocumentstatus;
        protected System.Web.UI.WebControls.Label lblauthstring;
        protected System.Web.UI.WebControls.Label lblassociatedno;
        protected System.Web.UI.WebControls.Label lbldepartment;
        protected System.Web.UI.WebControls.TextBox txtcomment;
        protected System.Web.UI.WebControls.DropDownList ddlactivitycode;
        protected System.Web.UI.WebControls.DropDownList ddlaccountcategory;
        protected System.Web.UI.WebControls.Button btnapprove;
        protected System.Web.UI.WebControls.Button btnreject;
        protected System.Web.UI.WebControls.Button btnreopen;
        protected System.Web.UI.WebControls.Button btndelete;
        protected System.Web.UI.WebControls.Label lblassociatedinvoiceno;
        protected System.Web.UI.WebControls.DropDownList ddlreject;
        protected System.Web.UI.WebControls.Label lblrejectioncomment;
        protected System.Web.UI.WebControls.TextBox txtrejcomment;
        protected System.Web.UI.WebControls.Label lblmessage;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvRejectionCode;
        protected System.Web.UI.WebControls.Label lblreasonforreject;
        protected System.Web.UI.WebControls.TextBox txtCreditNoteNo;
        protected System.Web.UI.WebControls.Label lblCreditNoteNo;
        protected System.Web.UI.WebControls.Label lblErrMsg;

        protected System.Web.UI.WebControls.Label lbldocumentno;
        protected System.Web.UI.WebControls.Label lblVoucherno;
        protected System.Web.UI.WebControls.Label lblCrno;
        protected System.Web.UI.HtmlControls.HtmlGenericControl SpanCrn;
        protected System.Web.UI.WebControls.DataGrid GDlineinfo;
        #endregion

        #region object and sql variables
        Invoice.Invoice objinvoice = new Invoice.Invoice();

        protected SqlConnection SqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected SqlConnection sqlConn = null;
        protected SqlCommand sqlCmd = null;
        protected SqlDataReader sqlDR = null;
        protected SqlParameter sqlReturnParam = null;
        #endregion

        #region User Defined Variables
        int iApproverStatusID = 0;
        string strNew_AccountCategory = "";
        string strNew_ActivityCode = "";
        string strComments = "";
        protected string AuthorisationStringToolTips = "";

        protected double dNetAmount = 0;
        protected double dVatAmount = 0;
        protected double dGrossAmount = 0;

        protected string strNetAmount = "";
        protected string strVatAmount = "";
        protected string strGrossAmount = "";

        #endregion

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {
            btnapprove.Attributes.Add("onclick", "javascript:return confirm('Are you sure you wish to Approve?');doHourglass();");
            btnreject.Attributes.Add("onclick", "javascript:doHourglass();");
            btnreopen.Attributes.Add("onclick", "javascript:doHourglass();");
            btndelete.Attributes.Add("onclick", "javascript:doHourglass();");
            int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
            if (UserTypeID == 3 || UserTypeID == 2)
            {
                int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
                int iStatusID = objinvoice.GetInvoiceStatus(iInvoiceID);
                ShowIfInvoiceRejected();
            }
            else
            {
                lblCreditNoteNo.Visible = false;
                txtCreditNoteNo.Visible = false;

            }

            string sUserID = Session["UserID"].ToString();
            Session["eInvoiceID"] = Request.QueryString["InvoiceID"].ToString();
            Session["eDocType"] = Request.QueryString["DocType"].ToString();
            Session["eInvoiceDate"] = Request.QueryString["InvoiceDate"].ToString();
            Session["eDocStatus"] = Request.QueryString["DocStatus"].ToString();
            Session["eVoucherNumber"] = Request.QueryString["VoucherNumber"].ToString();

            if (Convert.ToString(Session["eDocStatus"]) == "Reopened")
            {
                this.FindControl("SpanCrn").Visible = true;
                ((Label)((HtmlGenericControl)this.FindControl("SpanCrn")).FindControl("lblCrno")).Text = CheckCreditNoteAgainstInvoice();
            }
            else
            {
                this.FindControl("SpanCrn").Visible = false;
            }
            if (!Page.IsPostBack)
            {
                LoadData(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), System.Convert.ToInt32(sUserID));
                doAction(1);
            }

            if (UserTypeID == 1 && Convert.ToString(Session["eDocStatus"]) == "Rejected")
            {
                btnapprove.Visible = false;
                btnreject.Visible = false;
            }
            if (UserTypeID == 1 && Convert.ToString(Session["eDocStatus"]) == "Rejected")
                btnapprove.Visible = false;
            else
                btnapprove.Visible = true;
        }
        #endregion

        #region doAction  AND  takeAction
        private void takeAction(string docType, int ID, int iOperation)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

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
            this.GDlineinfo.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GDlineinfo_ItemDataBound);
            this.ddlactivitycode.SelectedIndexChanged += new System.EventHandler(this.ddlactivitycode_SelectedIndexChanged);
            this.btnapprove.Click += new System.EventHandler(this.btnapprove_Click);
            this.btnreject.Click += new System.EventHandler(this.btnreject_Click);
            this.btnreopen.Click += new System.EventHandler(this.btnreopen_Click);
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region LoadCombo

        private void LoadCombo(int InvoiceID, DropDownList ddlACode)
        {
            int NewActivityCodeID = 0;
            int i = 0;
            string sCatCode = "", sAcCode = "";
            SqlDataReader dr = null;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            sqlCmd = new SqlCommand("usp_GetActCodeAccCat", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
            try
            {
                sqlConn.Open();
                dr = sqlCmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["new_activitycode"] != DBNull.Value && Convert.ToString(dr["new_activitycode"]) != "NULL" && Convert.ToString(dr["new_activitycode"]) != "")
                        {
                            try
                            {
                                sAcCode = Convert.ToString(dr["new_activitycode"]);
                            }
                            catch { }
                        }

                        if (sAcCode != "")
                        {
                            for (i = 0; i < ddlACode.Items.Count; i++)
                            {
                                if (ddlACode.Items[i].Text.Trim() == sAcCode.Trim())
                                {
                                    try
                                    {
                                        ddlACode.SelectedIndex = i;
                                    }
                                    catch { }
                                }
                            }
                            try
                            {
                                NewActivityCodeID = GetActivityCodeIDByActivityCode(Convert.ToString(sAcCode.Trim()));
                            }
                            catch { }
                        }
                        Gettingddlaccountcategory(InvoiceID, Convert.ToInt32(NewActivityCodeID));
                        if (dr["new_accountcategory"] != DBNull.Value && Convert.ToString(dr["new_accountcategory"]) != "NULL" && Convert.ToString(dr["new_accountcategory"]) != "")
                        {
                            try
                            {
                                sCatCode = Convert.ToString(dr["new_accountcategory"]);
                            }
                            catch { }
                        }
                        if (sCatCode != "")
                        {
                            for (i = 0; i < ddlaccountcategory.Items.Count; i++)
                            {
                                if (ddlaccountcategory.Items[i].Text.Trim() == sCatCode.Trim())
                                {
                                    try
                                    {
                                        ddlaccountcategory.SelectedIndex = i;
                                    }
                                    catch { }
                                }
                            }
                        }

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

        }
        #endregion

        #region LoadData
        private void LoadData(int InvoiceID, int UserID)
        {
            string strDocNo = "";
            string DocType = Session["eDocType"].ToString();
            Invoice.Invoice objinvoice = new Invoice.Invoice();
            objinvoice.GetDocumentNoByDocID(InvoiceID, DocType, out strDocNo);
            lbldocumentno.Text = strDocNo;
            lblVoucherno.Text = Session["eVoucherNumber"].ToString();
            lbldocumentdate.Text = Session["eInvoiceDate"].ToString();
            lbldocumentstatus.Text = Session["eDocStatus"].ToString();
            lblsuppliername.Text = objinvoice.GetSupplierName(InvoiceID, DocType);
            lblauthstring.Text = objinvoice.GetAuthorisationString(InvoiceID, DocType);
            if (lblauthstring.Text != "")
            {
                AuthorisationStringToolTips = objinvoice.GetAuthorisationStringToolTips("select dbo.fn_GetApprovalPathToolTips_Generic('" + lblauthstring.Text.Trim() + "'," + InvoiceID + ")");
                lblauthstring.ToolTip = AuthorisationStringToolTips;
            }
            lbldepartment.Text = objinvoice.GetDepartment(InvoiceID, DocType);

            int UTypeID = objinvoice.GetUserType(Convert.ToInt32(Session["UserID"]));
            if (UTypeID < 3)
            {
                btndelete.Visible = false;
            }

            if (DocType == "CRN")
            {

                lblassociatedinvoiceno.Text = "Associated Invoice No";
                lblassociatedinvoiceno.Visible = true;

                lblassociatedno.Text = objinvoice.GetAssociatedCreditInvoiceNo(InvoiceID, DocType);
                lblassociatedno.Visible = true;
            }

            GDlineinfo.DataSource = objinvoice.GetLineInformation(InvoiceID, DocType);
            GDlineinfo.DataBind();

            ddlactivitycode.DataSource = objinvoice.GetActivityCode(System.Convert.ToInt32(InvoiceID));
            ddlactivitycode.DataBind();
            ddlactivitycode.Items.Insert(0, new ListItem("--Select--", DBNull.Value.ToString()));
            ddlactivitycode.SelectedIndex = -1;

            LoadCombo(InvoiceID, ddlactivitycode);

            int UserTypeID = objinvoice.GetUserType(UserID);
            if (UserTypeID == 1)
            {
                ddlreject.Items.Insert(0, new ListItem("Select Comments", "0"));
                ddlreject.Items.Insert(1, new ListItem("Invoice Error-CreditNote Requested", "1"));
                ddlreject.Items.Insert(2, new ListItem("Goods/Services not rec'd-CreditNote Requested", "2"));
                ddlreject.Items.Insert(3, new ListItem("Not my responsibility (Incorrect auth string/dept only)", "3"));
                ddlreject.Items.Insert(4, new ListItem("Please change account coding or Change coding AND approval path", "4"));
                ddlreject.Items.Insert(5, new ListItem("Invoice Error (Data does not match image or doc to be deleted)", "6"));
            }
            else
            {
                ddlreject.Items.Insert(0, new ListItem("Select Comments", "0"));
                ddlreject.Items.Insert(1, new ListItem("Invoice Error-CreditNote Requested", "1"));
                ddlreject.Items.Insert(2, new ListItem("Goods/Services not rec'd-CreditNote Requested", "2"));
                ddlreject.Items.Insert(3, new ListItem("Not my responsibility (Incorrect auth string/dept only)", "3"));
                ddlreject.Items.Insert(4, new ListItem("Please change account coding or Change coding AND approval path", "4"));
                ddlreject.Items.Insert(5, new ListItem("Invoice Error (Data does not match image or doc to be deleted)", "6"));
                ddlreject.Items.Insert(6, new ListItem("Cancelled by AP", "5"));
            }

            lblrejectioncomment.Text = "Rejection Comment";
            lblreasonforreject.Text = "Rejection Code";


            if (UserTypeID == 1)
            {
                btnreopen.Visible = false;
                btndelete.Visible = false;

                btnreject.Visible = true;

                lblreasonforreject.Visible = true;
                lblrejectioncomment.Visible = true;
                txtrejcomment.Visible = true;
                ddlreject.Visible = true;
                rfvRejectionCode.Enabled = true;

            }
            else
            {
                if (Session["eDocStatus"].ToString() == "Rejected")
                {
                    checkApproveButtonAppear();
                    ViewState["StatusID"] = "6";
                    if (UserTypeID == 3)
                        btndelete.Visible = true;

                    btnapprove.Visible = true;
                    btnreopen.Visible = true;

                    btnreject.Visible = false;

                    lblreasonforreject.Visible = false;
                    lblrejectioncomment.Visible = false;
                    txtrejcomment.Visible = false;
                    ddlreject.Visible = false;
                    rfvRejectionCode.Enabled = false;

                }
                else if (Session["eDocStatus"].ToString() == "Registered")
                {
                    if (UserTypeID == 3)
                        btndelete.Visible = true;

                    btnreopen.Visible = false;
                    btnreject.Visible = true;
                    lblreasonforreject.Visible = true;
                    lblrejectioncomment.Visible = true;
                    txtrejcomment.Visible = true;
                    ddlreject.Visible = true;
                    rfvRejectionCode.Enabled = true;
                }
                else
                {
                    if (UserTypeID == 3)
                        btndelete.Visible = true;

                    btnreopen.Visible = false;
                    btnreject.Visible = false;
                    lblreasonforreject.Visible = false;
                    lblrejectioncomment.Visible = false;
                    txtrejcomment.Visible = false;
                    ddlreject.Visible = false;
                    rfvRejectionCode.Enabled = false;
                }
            }
        }
        #endregion

        #region private bool IsFlagOn(int InvoiceID)
        private bool IsFlagOn(int InvoiceID)
        {
            bool isRecExist = false;
            try
            {
                DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                RecordSet rs = da.ExecuteQuery("vwActionFlag", "InvoiceID=" + InvoiceID);
                while (!rs.EOF())
                {
                    if (Convert.ToInt32(rs["Flag"]) == 0)
                    {
                        isRecExist = true;
                        break;
                    }
                    else
                    {
                        isRecExist = false;
                        break;
                    }
                }
            }
            catch
            {
                isRecExist = false;
            }
            return isRecExist;
        }
        #endregion

        #region btnapprove_Click
        private void btnapprove_Click(object sender, System.EventArgs e)
        {
            btnapprove.Visible = false;
            btnapprove.Enabled = false;
            bool isSel = false;
            if (ddlaccountcategory.SelectedItem.Text != "--Select--")
            {
                strNew_AccountCategory = ddlaccountcategory.SelectedItem.Text;
            }

            if (ddlactivitycode.SelectedItem.Text != "--Select--")
            {
                strNew_ActivityCode = ddlactivitycode.SelectedItem.Text;
            }


            if (IsFlagOn(Convert.ToInt32(Request.QueryString["InvoiceID"].ToString())) == false)
            {
                if (ddlaccountcategory.SelectedItem.Value != "" && ddlactivitycode.SelectedItem.Value != "")
                {
                    isSel = true;
                }

                if (isSel == false)
                {
                    Response.Write("<script>alert('You must enter an activity & account category combination');</script>");
                    return;
                }
            }
            else
            {
                if (ddlaccountcategory.SelectedItem.Value != "" || ddlactivitycode.SelectedItem.Value != "")
                {
                    isSel = true;
                }
                if (isSel == true)
                {
                    Response.Write("<script>alert('Sorry, you cannot select Activity Code/Account Category for this Account Code combination');</script>");
                    return;
                }

                lblmessage.Visible = false;
            }

            if (Convert.ToString(ViewState["StatusID"]) == "6")
            {
                if (txtCreditNoteNo.Text.Trim() == "")
                {
                    Response.Write("<script>alert('Please enter valid credit note number.');</script>");
                    return;
                }
            }
            if (Convert.ToInt32(ddlreject.SelectedValue) > 0)
            {
                Response.Write("<script>alert('Sorry,Invoice cannot be Approved. You have selected RejectionCode.');</script>");
                return;
            }

            strComments = txtcomment.Text.Trim();
            int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
            if (UserTypeID == 3 || UserTypeID == 2)
            {
                string strCreditInvoiceNoTemp = txtCreditNoteNo.Text.Trim();
                if (txtCreditNoteNo.Visible == true)
                {
                    string strCreditInvoiceNo = CheckCreditNoteAgainstInvoice();

                    int iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Approve(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNoTemp);
                    if (iRetValForUpdate == -101)
                    {
                        Response.Write("<script>alert('The credit note must be in Registered status.');</script>");
                        return;
                    }
                    if (iRetValForUpdate == -102)
                    {
                        Response.Write("<script>alert('The credit note must be in Registered status.');</script>");
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
                        lblErrMsg.Visible = false;
                        iApproverStatusID = 19;

                        int UpdateStatus = objinvoice.UpdateApproveByAPAdmin(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), strNew_AccountCategory, strNew_ActivityCode);
                        if (UpdateStatus == 1)
                        {
                            objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                            objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(iRetValForUpdate), System.Convert.ToInt32(Session["UserID"]), 3, 19, txtcomment.Text.Trim());
                            doAction(0);
                            Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                            Response.Write("<script>self.close();</script>");
                        }
                        else
                            Response.Write("<script>alert('Invoice status cannot be updated');</script>");
                    }

                }
                else
                {
                    lblErrMsg.Visible = false;
                    iApproverStatusID = 19;
                    int UpdateStatus = objinvoice.UpdateApproveByAPAdmin(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), strNew_AccountCategory, strNew_ActivityCode);
                    if (UpdateStatus == 1)
                    {
                        int iRetValForUpdate = UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNoTemp);
                        objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                        doAction(0);
                        lblmessage.Text = "Invoice Approved Successfully";
                        Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                        Response.Write("<script>self.close();</script>");
                    }
                    else
                        Response.Write("<script>alert('Invoice status cannot be updated');</script>");
                }
            }
            else
            {
                iApproverStatusID = 19;
                int ApprovedStatus = objinvoice.GetApprovedStatus(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), System.Convert.ToInt32(Session["UserID"].ToString()), strNew_AccountCategory, strNew_ActivityCode);
                if (ApprovedStatus == 1)
                    Response.Write("<script>alert('Invoice Cannot be Approved');</script>");
                else
                {
                    string sFiletoWrite = ConfigurationManager.AppSettings["TestPath"].Trim() + @"\" + DateTime.Now.ToShortDateString().Replace("/", "_") + "_Log.txt";
                    int iInvNO = System.Convert.ToInt32(Session["eInvoiceID"]);
                    int iUSERID = System.Convert.ToInt32(Session["UserID"]);
                    StreamWriter oSWriter = null;
                    oSWriter = new StreamWriter(sFiletoWrite, true);
                    oSWriter.WriteLine("InvoiceID - " + iInvNO);
                    oSWriter.WriteLine("USERID - " + iUSERID);
                    oSWriter.WriteLine("================================= ");
                    oSWriter.Close();
                    objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                    objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), 1, 19, txtcomment.Text.Trim());
                    doAction(0);
                    lblmessage.Text = "Invoice Approved Successfully";
                    Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                    Response.Write("<script>self.close();</script>");
                }
            }
        }
        #endregion

        #region btnreopen_Click
        private void btnreopen_Click(object sender, System.EventArgs e)
        {
            lblErrMsg.Visible = true;
            if (Convert.ToString(ViewState["StatusID"]) == "6")
            {
                if (txtCreditNoteNo.Text.Trim() == "")
                {
                    int iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                    if (iRejectionCode != 6)
                    {
                        Response.Write("<script>alert('Please enter valid credit note number');</script>");
                        return;
                    }
                }
            }

            if (txtcomment.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter a comment.');</script>");
                return;
            }
            else
            {
                lblmessage.Text = "";
                iApproverStatusID = 22;
                strComments = txtcomment.Text.Trim();
                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                int iRejectionCode = objinvoice.GetRejectionCodeID_NL(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                if (UserTypeID == 3 || UserTypeID == 2)
                {
                    string strCreditInvoiceNo = "";
                    int iRetValForUpdate = 0;
                    int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
                    strCreditInvoiceNo = txtCreditNoteNo.Text.Trim();

                    if (strCreditInvoiceNo.Trim().ToUpper() != "REOPEN")
                    {
                        iRetValForUpdate = objinvoice.UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo_Reopen(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNo);
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
                            Response.Write("<script>alert('There is no creditnote against the given no.');</script>");
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
                    if ((txtCreditNoteNo.Text.Trim() == "" && iRejectionCode == 6) || txtCreditNoteNo.Text.Trim().ToUpper() == "REOPEN")
                    {
                        int UpdateStatus = objinvoice.UpdateInvoiceReopenAPAdmin(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), iRetValForUpdate);
                        if (UpdateStatus == 1)
                        {
                            lblErrMsg.Visible = false;
                            objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                            objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(iRetValForUpdate), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, txtcomment.Text.Trim());
                            doAction(0);
                            Response.Write("<script>alert('Invoice Reopened Successfully');</script>");
                            Response.Write("<script>self.close();</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Invoice status cannot be reopened');</script>");
                            return;
                        }
                    }
                    else
                    {
                        int CurrentCreditNoteID = GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo(System.Convert.ToInt32(Session["eInvoiceID"]), strCreditInvoiceNo);
                        int UpdateStatus = objinvoice.UpdateInvoiceReopenAPAdmin(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), CurrentCreditNoteID);
                        if (UpdateStatus == 1)
                        {
                            objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                            objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(iRetValForUpdate), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, txtcomment.Text.Trim());
                            lblErrMsg.Visible = false;
                            doAction(0);
                            Response.Write("<script>alert('Invoice Reopened Successfully');</script>");
                            Response.Write("<script>self.close();</script>");
                        }
                    }
                }
            }
        }
        #endregion

        #region	btndelete_Click
        private void btndelete_Click(object sender, System.EventArgs e)
        {
            if (txtcomment.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter a comment.');</script>");
                return;
            }
            else
            {
                lblmessage.Text = "";
                iApproverStatusID = 7;
                strComments = txtcomment.Text.Trim();
                int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                int StatusUpdate = objinvoice.UpdateInvStatusToDelete(System.Convert.ToInt32(Session["eInvoiceID"].ToString()));
                if (StatusUpdate == 1)
                {
                    objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, "");
                    lblmessage.Text = "Invoice Deleted Successfully";
                    doAction(0);
                    Response.Write("<script>alert('Invoice Deleted Successfully.');</script>");

                    Response.Write("<script>self.close();</script>");
                }
                else
                {
                    Response.Write("<script>alert('Invoice cannot be deleted');</script>");
                }
            }
        }
        #endregion

        #region btnreject_Click
        private void btnreject_Click(object sender, System.EventArgs e)
        {

            bool retVal = true;
            iApproverStatusID = 6;
            if (ddlreject.SelectedIndex == 0 || txtrejcomment.Text == "")
            {
                if (ddlreject.SelectedIndex == 0)
                {
                    Response.Write("<script>alert('Please select rejection code.');</script>");

                    retVal = false;
                    return;
                }
                else
                {
                    lblmessage.Visible = false;
                }
            }
            if (txtrejcomment.Text.Trim() == "")
            {
                Response.Write("<script>alert('Please enter required coding in rejection comments.');</script>");

                retVal = false;
                return;
            }
            else
            {
                lblmessage.Visible = false;
            }


            if (retVal == true)
            {
                lblmessage.Text = "";
                strComments = txtrejcomment.Text.Trim();
                int Result = objinvoice.UpdateStatusToReject(System.Convert.ToInt32(Session["eInvoiceID"].ToString()), ddlreject.SelectedItem.Text.ToString(), txtrejcomment.Text, Convert.ToInt32(ddlreject.SelectedValue), System.Convert.ToInt32(Session["UserID"].ToString()), "");

                if (Result == 2)
                {
                    lblmessage.Text = "Invoice Rejected Successfully";
                    int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
                    objinvoice.UpdateInvoiceStatusLogApproverWise(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), UserTypeID, iApproverStatusID, strComments, ddlreject.SelectedItem.Text.Trim());
                    doAction(0);

                    Response.Write("<script>alert('Invoice Rejected Successfully');</script>");

                    Response.Write("<script>self.close();</script>");

                }
                else
                {
                    Response.Write("<script>alert('Invoice Already Rejected');</script>");
                }
            }
        }
        #endregion

        #region IsRejected function
        private void ShowIfInvoiceRejected()
        {
            int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
            int iStatusID = objinvoice.GetInvoiceStatus(iInvoiceID);
            string strCreditInvoiceNo = CheckCreditNoteAgainstInvoice();
            if (strCreditInvoiceNo.Trim() != "")
                txtCreditNoteNo.Text = strCreditInvoiceNo.Trim();

            if (iStatusID == 6)
            {
                lblCreditNoteNo.Visible = true;
                txtCreditNoteNo.Visible = true;
                btnapprove.Visible = true;

            }
            else
            {
                lblCreditNoteNo.Visible = false;
                txtCreditNoteNo.Visible = false;
                btnapprove.Visible = false;
            }
        }
        #endregion

        #region checkApproveButtonAppear
        private void checkApproveButtonAppear()
        {
            int iCode;
            int InvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
            iCode = objinvoice.GetRejectionCodeID(InvoiceID);
            if (iCode == 0)
            {
                int iCount;
                int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
                iCount = objinvoice.CheckCreditNoteAmountAndCurrencyByInvoiceID(iInvoiceID);
                if (iCount == 0)
                {
                    btnapprove.Visible = true;
                    btnreopen.Visible = false;
                }
                else if (iCount == 3)
                {
                    btnapprove.Visible = false;
                    btnreopen.Visible = true;
                }
                else
                {
                    btnapprove.Visible = false;
                    btnreopen.Visible = false;
                }
            }
            else
            {
                btnapprove.Visible = false;
                btnreopen.Visible = false;
            }

        }
        #endregion

        #region CheckIsFullCreditNote
        private int CheckIsFullCreditNote()
        {
            int iReturnValue = 0;
            int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());
            SqlParameter sqlRetParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_IsFullCreditNote", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

            sqlRetParam = sqlCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            sqlRetParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlRetParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return iReturnValue;
        }
        #endregion

        #region private void GDlineinfo_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        private void GDlineinfo_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                strNetAmount = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "NETAMT"));
                strVatAmount = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "TAXAMT"));
                strGrossAmount = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "GROSSAMT"));

                if (!strNetAmount.Trim().Equals(""))
                    dNetAmount = (dNetAmount + Convert.ToDouble(strNetAmount.Trim()));
                if (!strVatAmount.Trim().Equals(""))
                    dVatAmount = (dVatAmount + Convert.ToDouble(strVatAmount.Trim()));
                if (!strGrossAmount.Trim().Equals(""))
                    dGrossAmount = (dGrossAmount + Convert.ToDouble(strGrossAmount.Trim()));
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblNetTotal = null;
                Label lblTaxTotal = null;
                Label lblGrossTotal = null;

                lblNetTotal = (Label)(e.Item.FindControl("lblNetTotal"));
                lblTaxTotal = (Label)(e.Item.FindControl("lblTaxTotal"));
                lblGrossTotal = (Label)(e.Item.FindControl("lblGrossTotal"));

                lblNetTotal.Text = string.Format("{0:n2}", dNetAmount);
                lblTaxTotal.Text = string.Format("{0:n2}", dVatAmount);
                lblGrossTotal.Text = string.Format("{0:n2}", dGrossAmount);
            }
        }
        #endregion

        #region CheckCreditNoteAgainstInvoice
        public int CheckCreditNoteAgainstInvoice(int iInvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckCreditNoteAgainstInvoice", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);
            sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion

        #region UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo
        public int UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo(int InvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlConn.Open();

            sqlCmd = new SqlCommand("sp_UpdateCreditInvoiceNOByInvoiceIDAgainstCreditNoteNo", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);

            sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion

        #region GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo
        public int GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo(int InvoiceID, string strCreditNoteNo)
        {
            int iReturnValue = 0;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetCurrentCreditNoteIDByInvoiceIDAndCreditNoteNo", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);

            sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion

        #region CheckCreditNoteAgainstInvoice
        private string CheckCreditNoteAgainstInvoice()
        {
            string InvoiceNo = "";
            int iInvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"].ToString());

            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetCreditNoteByInvoiceID", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@InvoiceID", iInvoiceID);

            sqlOutputParam = sqlCmd.Parameters.Add("@CreditInvoiceNo", SqlDbType.VarChar, 20);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                InvoiceNo = sqlOutputParam.Value.ToString();
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return InvoiceNo;
        }
        #endregion

        #region GetActivityCodeIDByActivityCode
        private int GetActivityCodeIDByActivityCode(string ActivityCode)
        {
            int ActivityCodeID = 0;
            SqlParameter sqlOutputParam = null;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_GetActivityCodeIDByActivityCode", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@ActivityCode", ActivityCode);

            sqlOutputParam = sqlCmd.Parameters.Add("@ActivityCodeID", SqlDbType.Int);
            sqlOutputParam.Direction = ParameterDirection.Output;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                ActivityCodeID = Convert.ToInt32(sqlOutputParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlOutputParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return ActivityCodeID;
        }
        #endregion

        #region CheckCreditNoteStatusAgainstGivenCreditNoteNo
        public int CheckCreditNoteStatusAgainstGivenCreditNoteNo(string strCreditNoteNo)
        {
            int iReturnValue = 0;
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


            sqlCmd = new SqlCommand("sp_CheckCreditNoteStatusAgainstGivenCreditNoteNo", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CreditNoteNo", strCreditNoteNo);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }

            return (iReturnValue);
        }
        #endregion

        #region ddlactivitycode_SelectedIndexChanged
        private void ddlactivitycode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlactivitycode.SelectedItem.Text == "--Select--")
            {
                ddlaccountcategory.ClearSelection();
            }
            else
                Gettingddlaccountcategory(System.Convert.ToInt32(Session["eInvoiceID"]), Convert.ToInt32(ddlactivitycode.SelectedValue));
        }
        #endregion

        #region Gettingddlaccountcategory(int InvoiceId ,int activitycode)
        private void Gettingddlaccountcategory(int InvoiceId, int activitycode)
        {
            ddlaccountcategory.DataSource = objinvoice.GetAccountCategory(InvoiceId, activitycode);
            ddlaccountcategory.DataBind();
            ddlaccountcategory.Items.Insert(0, new ListItem("--Select--", DBNull.Value.ToString()));
            ddlaccountcategory.SelectedIndex = -1;
        }
        #endregion
        #region private void SendEmail()
        private void SendEmail()
        {
            try
            {
                string StrInvoiceNo = "", strBuyerCompanyName = "", strSupplierCompanyName = "", strSupplierCodeAgainstBuyer = "";
                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                if (Session["eDocType"].ToString().Trim() == "")
                {
                    SqlDataAdapter dap = new SqlDataAdapter("SP_RejectionMail_gmg", sqlConn);
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(Session["eInvoiceID"]));
                    dap.SelectCommand.Parameters.Add("@DocType", Session["eDocType"].ToString().ToUpper());

                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StrInvoiceNo = ds.Tables[0].Rows[0]["InvoiceNO"].ToString();
                        strBuyerCompanyName = ds.Tables[0].Rows[0]["buyerCompanyName"].ToString();
                        strSupplierCompanyName = ds.Tables[0].Rows[0]["supplierCompanyName"].ToString();
                        strSupplierCodeAgainstBuyer = ds.Tables[0].Rows[0]["SupplierCodeAgainstBuyer"].ToString();
                    }
                }
                MailMessage _mailMSG = new MailMessage();

                _mailMSG.Priority = MailPriority.High;
                _mailMSG.BodyFormat = MailFormat.Html;
                _mailMSG.Subject = "Invoice Rejection (" + StrInvoiceNo + ")";
                _mailMSG.From = ConfigurationManager.AppSettings["MailFrom"];

                _mailMSG.Bcc = ConfigurationManager.AppSettings["MailBCC"];


                string sBody = "<BR><BR>The following invoice has been rejected:" +


                    "<BR><BR>Child Buyer Company Name : " + strBuyerCompanyName + " " +
                    "<BR><BR>Supplier Name            : " + strSupplierCompanyName + " " +
                    "<BR><BR>SupplierCodeAgainstBuyer : " + strSupplierCodeAgainstBuyer + " " +
                    "<BR><BR>Invoice Number           : " + StrInvoiceNo + " " +

                    "<BR><BR>Kind regards, " +
                    "<BR><BR>P2D Support Team";

                sBody = "<FONT face=Verdana Size =2pt>" + sBody + "</FONT>";

                _mailMSG.Body = sBody;
                SmtpMail.SmtpServer = ConfigurationManager.AppSettings["MailServer"].Trim();
                SmtpMail.Send(_mailMSG);


            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

        }
        #endregion
    }
}
