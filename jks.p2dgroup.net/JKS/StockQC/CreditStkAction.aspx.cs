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
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using JKS;
using System.IO;
public partial class JKS_StockQC_CreditStkAction_n : System.Web.UI.Page
{
    #region variables
    public int invoiceID = 0;
    private int InvoiceID = 0;
    protected decimal dTotalReceived = 0;
    protected decimal dTotalInvoiced = 0;
    protected decimal dVerience = 0;
    protected int iApproverStatusID = 0;

    protected double dTotalAmount = 0;
    //		double dNetAmt=0;

    #endregion
    #region Sql Variables
    public string ConsString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
    #endregion
	
    protected void Page_Load(object sender, EventArgs e)
    {
        //added by kuntalkarar on 12thJanuary2017 to stop multiple additoin in the "Purchase Order No" table
        dTotalReceived = 0;
        dTotalInvoiced = 0;

        btnCancel.Attributes.Add("onclick", "javascript:windowclose();");
        if (Request.QueryString["InvoiceID"] != null)
        {
            InvoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            Session["eInvoiceID"] = InvoiceID;
        }
        if (!Page.IsPostBack)
        {
            GetStockDocumentDetails(InvoiceID);
            GetRejectionCode();
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
        this.grdInvCur.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdInvCur_ItemDataBound);
        this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
        this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
        this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
        this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion
    #region GetStockDocumentDetails
    public void GetStockDocumentDetails(int InvID)
    {
        StockDal oStockDal = new StockDal();
        DataSet dsStock = new DataSet();
        dsStock = oStockDal.GetStockDocumentDetails(InvID, "CRN");
        if (dsStock != null)
        {
            if (dsStock.Tables.Count > 0)
            {
                #region Tables[0].Rows.Count
                if (dsStock.Tables[0].Rows.Count > 0)
                {
                    lblDocumentNo.Text = dsStock.Tables[0].Rows[0]["InvoiceNo"].ToString();
                    lblinvoicedate.Text = dsStock.Tables[0].Rows[0]["InvoiceDate"].ToString();
                    lblsupplier.Text = dsStock.Tables[0].Rows[0]["SupplierCompanyName"].ToString();
                    lblBuyerName.Text = dsStock.Tables[0].Rows[0]["BuyerCompanyName"].ToString();
                    try
                    {
                        Session["BuyerID_STK"] = dsStock.Tables[0].Rows[0]["BuyerID"].ToString();
                        Session["SupplierID_STK"] = dsStock.Tables[0].Rows[0]["SupplierID"].ToString();
                    }
                    catch { }
                    lblCurrency.Text = dsStock.Tables[0].Rows[0]["CurrencyCode"].ToString();
                    #region
                    oStockDal.CurrencyType = dsStock.Tables[0].Rows[0]["CurrencyCode"].ToString();
                    Session["CurrencyType"] = dsStock.Tables[0].Rows[0]["CurrencyCode"].ToString(); //kk
                    #endregion
                    lblinvoicestatus.Text = dsStock.Tables[0].Rows[0]["Status"].ToString();
                    ViewState["StatusID"] = dsStock.Tables[0].Rows[0]["StatusID"].ToString();
                    try
                    {
                        lblCreditNo.Text = dsStock.Tables[0].Rows[0]["CreditInvoiceNo"].ToString();
                    }
                    catch { }
                    try
                    {
                        lblDepartmant.Text = dsStock.Tables[0].Rows[0]["Department"].ToString();
                        lblNominal.Text = dsStock.Tables[0].Rows[0]["NominalCodeID"].ToString();
                    }
                    catch { }
                    try
                    {
                        lblinvoicetype.Text = dsStock.Tables[0].Rows[0]["DocType"].ToString();
                        Session["Doctype_STK"] = dsStock.Tables[0].Rows[0]["DocType"].ToString();//added by kuntalkarar on 2ndMarch2017
                    }
                    catch { }
                    lblNet.Text = dsStock.Tables[0].Rows[0]["Net"].ToString();
                    lblVat.Text = dsStock.Tables[0].Rows[0]["Vat"].ToString();
                    lblGross.Text = dsStock.Tables[0].Rows[0]["Total"].ToString();
                }
                #endregion

                #region Tables[0].Rows.Count
                if (dsStock.Tables[1].Rows.Count > 0)
                {
                    grdInvCur.DataSource = dsStock.Tables[1];
                    grdInvCur.DataBind();
                }
                #endregion
            }
        }
    }
    #endregion
    #region GetRejectionCode
    public void GetRejectionCode()
    {
        CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval objApproval = new CBSolutions.ETH.Web.NewBuyer.ApprovalPath.Approval();
        DataSet ds = new DataSet();
        string Fields = "RejectionCodeID, RejectionCode";
        string Table = "RejectionCode";
        string Criteria = "BuyerCompanyID =(SELECT ParentCompanyID FROM Company WHERE CompanyID =" + System.Convert.ToInt32(Session["BuyerID_STK"]) + ")";		//Session["CompanyID"]  Session["BuyerCID"] Session["DropDownCompanyID"]
        ds = objApproval.GetGlobalDropDowns(Fields, Table, Criteria);

        ddlRejectionCode.DataValueField = "RejectionCodeID";
        ddlRejectionCode.DataTextField = "RejectionCode";
        ddlRejectionCode.DataSource = ds;
        ddlRejectionCode.DataBind();
        ddlRejectionCode.Items.Insert(0, "Select Comments");
    }
    #endregion 
    #region grdInvCur_ItemDataBound
    private void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item)
            {
                // string customerId = grdInvCur.DataKeys[e.Item.ItemIndex].ToString();
                GridView gvOrders = e.Item.FindControl("gvOrders") as GridView;
                gvOrders.DataSource = GetInvalidPONumberFor();
                gvOrders.DataBind();
            }
        }
        catch { }

        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblTotReceived")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(((Label)e.Item.FindControl("lblTotReceived")).Text)));
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblTotInvoiced")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(((Label)e.Item.FindControl("lblTotInvoiced")).Text)));
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("Label2")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(((Label)e.Item.FindControl("Label2")).Text)));
            if (((Label)e.Item.FindControl("lblTotReceived")).Text != "")
            {
                dTotalReceived = dTotalReceived + Convert.ToDecimal(((System.Web.UI.WebControls.Label)e.Item.FindControl("lblTotReceived")).Text.Trim());
            }
            if (((Label)e.Item.FindControl("lblTotInvoiced")).Text != "")
            {
                dTotalInvoiced = dTotalInvoiced + Convert.ToDecimal(((System.Web.UI.WebControls.Label)e.Item.FindControl("lblTotInvoiced")).Text.Trim());
            }

        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblFooterTotalReceived")).Text = Convert.ToString(dTotalReceived);
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblFooterTotInvoiced")).Text = Convert.ToString(dTotalInvoiced);
            dVerience = dTotalInvoiced - dTotalReceived;
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblFooterVarience")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(dVerience)));

            StockDal oSDal = new StockDal();
            oSDal.PropInvoiced = dTotalInvoiced;
            oSDal.PropReceived = dTotalReceived;
            oSDal.PropVarience = dVerience;
        }
    }

    public DataTable GetInvalidPONumberFor()
    {
        string existscheck = "";
        DataSet Dst = new DataSet();
        DataTable dt = new DataTable();
        SqlDataAdapter sqlDA = null;
        SqlConnection sqlConn = null;
        sqlConn = new SqlConnection(ConsString);
        sqlDA = new SqlDataAdapter("getInvalidPO_JKS", sqlConn);
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(Request["InvoiceID"]));
        sqlDA.SelectCommand.Parameters.Add("@Type", "CRN");

        try
        {
            sqlConn.Open();
            sqlDA.Fill(dt);
        }
        catch (Exception ex) { string ss = ex.Message.ToString(); }
        finally
        {
            sqlDA.Dispose();
            sqlConn.Close();
        }
        /*if (Dst.Tables.Count > 0)
        {
            if (Dst.Tables[0].Rows.Count > 0)
            {
                existscheck = Convert.ToString(Dst.Tables[0].Rows[0]["Exists"]);
            }
        }*/
        return dt;
    }
    #endregion
    #region GetPOActionURL
    // <A href='#' onclick="<%#GetPOActionURL(DataBinder.Eval(Container.DataItem,"POID"))%>">
    // <A href='GrdRcdDetail.aspx?POID=<%#DataBinder.Eval(Container.DataItem,"POID")%>'>
    protected string GetPOActionURL(object oPoID, object oGoodsRecdID)
    {
        string strPoID = Convert.ToString(oPoID);
        int iGoodsRecdID = Convert.ToInt32(oGoodsRecdID);

        string strURL = "";

        strURL = "javascript:window.open('GrdRcdDetail.aspx?POID=" + strPoID + "&GRID=" + iGoodsRecdID + "','popupwindow','width=1400,height=550,scrollbars=1,resizable=1,target=_new');";

        return (strURL);
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
        catch { }
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
            takeAction("CRN", Convert.ToInt32(Request["InvoiceID"]), 1);
        }
        else if (iActionType == 0)
        {
            takeAction("CRN", Convert.ToInt32(Request["InvoiceID"]), 0);
        }
    }
    #endregion
    #region btndelete_Click
    private void btndelete_Click(object sender, System.EventArgs e)
    {
        if (tbComments.Text.Trim() == "")
        {
            Response.Write("<script>alert('Please enter a comment.');</script>");
            return;
        }
        else
        {
           Invoice objinvoice = new Invoice();
            lblMessege.Text = "";
            lblMessege.Visible = true;
            iApproverStatusID = 7;
            string strComments = tbComments.Text.Trim();

            //	int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
            int StatusUpdate = UpdateSTOCKCreditNoteCOMMON_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), strComments, 7);
            if (StatusUpdate >= 1)
            {
                //		objinvoice.UpdateInvoiceStatusLogApproverWise( System.Convert.ToInt32(Session["eInvoiceID"]),  System.Convert.ToInt32(Session["UserID"]), UserTypeID,  iApproverStatusID,  strComments,"");
                lblMessege.Text = "CreditNote Deleted Successfully";
                doAction(0);
                Response.Write("<script>alert('CreditNote Deleted Successfully.');</script>");
                Response.Write("<script>opener.location.reload(true);</script>");
                Response.Write("<script>self.close();</script>");
            }
            else
            {
                Response.Write("<script>alert('CreditNote cannot be deleted');</script>");
            }
        }
    }
    #endregion
    #region btnReject_Click
    private void btnReject_Click(object sender, System.EventArgs e)
    {
        //			bool retVal = true;
        iApproverStatusID = 6;
        if (tbComments.Text.Trim() == "")
        {
            Response.Write("<script>alert('Please enter a rejection comment.');</script>");
            //				retVal=false;
            return;
        }

        if (ddlRejectionCode.SelectedIndex == 0)
        {
            Response.Write("<script>alert('Please select rejection code.');</script>");
            //				retVal=false;
            return;
        }

        lblMessege.Text = "";
        string strComments = tbComments.Text.Trim();

        int Result = UpdateSTOCKCreditNoteCOMMON_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), strComments, 6);
        if (Result >= 1)
        {
            doAction(0);
            Response.Write("<script>alert('CreditNote Rejected Successfully');</script>");
            Response.Write("<script>opener.location.reload(true);</script>");
            Response.Write("<script>self.close();</script>");
        }
        else
        {
            Response.Write("<script>alert('CreditNote Already Rejected');</script>");
        }
    }
    #endregion
    #region btnApprove_Click
    private void btnApprove_Click(object sender, System.EventArgs e)
    {
        if (tbComments.Text.Trim() == "")
        {
            Response.Write("<script>alert('Please enter comments.');</script>");
            return;
        }

        if (dVerience > 0)
        {
            Response.Write("<script>return confirm('Are you sure that you want to approve?');</script>");
            return;
        }


        string strComments = tbComments.Text.Trim();

        iApproverStatusID = 19;
        int UpdateStatus = UpdateSTOCKCreditNoteCOMMON_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), strComments, 19);
        if (UpdateStatus >= 1)
        {
            doAction(0);
            Response.Write("<script>alert('CreditNote Approved Successfully');</script>");
            Response.Write("<script>opener.location.reload(true);</script>");
            Response.Write("<script>self.close();</script>");
        }
        else if (UpdateStatus == -1)
            Response.Write("<script>alert('Error in approving.');</script>");

    }
    #endregion
    #region GetAfterDecimalCalculatedValue(decimal _Value)
    public decimal GetAfterDecimalCalculatedValue(decimal _Value)
    {
        decimal originalVal = 0;
        string strValue = "";
        if (_Value < 0)
        {
            originalVal = Math.Abs(_Value);
            originalVal = HelpGetAfterDecimalCalculatedValue(originalVal);
            strValue = "-" + Convert.ToString(originalVal);
            _Value = Convert.ToDecimal(strValue);
        }
        else
        {
            _Value = HelpGetAfterDecimalCalculatedValue(_Value);
        }
        return _Value;
    }


    public decimal HelpGetAfterDecimalCalculatedValue(decimal _Value)
    {
        int Count = 0;
        double t = 0;
        decimal x = 0;
        int IntValue = 0;
        int DecVal = 0;
        string ReturnAmnt = "";
        decimal retVal = 0;
        decimal yy = 0;
        /*----------Make string in two decimal-----------*/
        IntValue = System.Convert.ToInt32(Math.Floor(Convert.ToDouble(_Value)));
        yy = (System.Convert.ToDecimal(_Value) - System.Convert.ToDecimal(IntValue));
        x = yy * 100;
        if (x < 10)
        {
            Count = Microsoft.VisualBasic.Strings.Len(Convert.ToString(yy));
            if (Count >= 4)
            {
                ReturnAmnt = "." + Microsoft.VisualBasic.Strings.Mid(yy.ToString(), 3, 4);
                t = Convert.ToDouble(ReturnAmnt);
                ReturnAmnt = "." + Microsoft.VisualBasic.Strings.Mid(t.ToString("#0.00"), 3, 2);
                retVal = Convert.ToDecimal(ReturnAmnt);
            }
            if (Count == 1)
                ReturnAmnt = "." + "00";

            retVal = Convert.ToDecimal(System.Convert.ToString(IntValue) + ReturnAmnt);
        }

        if (x >= 10)
        {
            string z = Microsoft.VisualBasic.Strings.Mid(x.ToString(), 4, 1);
            if (Microsoft.VisualBasic.Strings.Mid(x.ToString(), 4, 1) == "5")
                yy = yy + Convert.ToDecimal(0.01 - 0.005);
            yy = yy * 100;
            DecVal = Convert.ToInt32(yy);
            ReturnAmnt = IntValue.ToString() + "." + DecVal.ToString();
            retVal = Convert.ToDecimal(Convert.ToDouble(ReturnAmnt).ToString("#.00"));
        }

        return (retVal);
    }
    #endregion
    #region UpdateSTOCKCreditNoteCOMMON_Generic
    public int UpdateSTOCKCreditNoteCOMMON_Generic(int CreditNoteID, string Comments, int ApproverStatus)
    {
        int iCount = 0;
        SqlParameter sqlOutputParam = null;
        SqlCommand sqlCmd = null;
        SqlConnection sqlConn = new SqlConnection(ConsString);
        sqlCmd = new SqlCommand("stp_UpdateSTOCKCreditNoteCOMMON_Generic", sqlConn);
        sqlCmd.CommandType = CommandType.StoredProcedure;
        sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"]));
        sqlCmd.Parameters.Add("@CreditNoteID", CreditNoteID);
        sqlCmd.Parameters.Add("@Comments", Comments);
        sqlCmd.Parameters.Add("@ApproverStatus", ApproverStatus);
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
}