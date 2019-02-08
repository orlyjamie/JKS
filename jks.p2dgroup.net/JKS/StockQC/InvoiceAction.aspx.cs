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
public partial class JKS_StockQC_InvoiceAction : System.Web.UI.Page
{
    #region variables
    public int invoiceID = 0;
    private int InvoiceID = 0;
    protected decimal dTotalReceived = 0;
    protected decimal dTotalInvoiced = 0;
    protected decimal dVerience = 0;
    protected int iApproverStatusID = 0;
    protected double dTotalAmount = 0;
    double dNetAmt = 0;

    #endregion
    #region Sql Variables
    public string ConsString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
    #endregion
    #region  objects declarations
    protected CBSolutions.ETH.Web.Invoice objInvoice = new CBSolutions.ETH.Web.Invoice();
    //protected System.Web.UI.WebControls.Button btnApprove;
    //protected System.Web.UI.WebControls.Button btnReject;
    //protected System.Web.UI.WebControls.Button btndelete;
    //protected System.Web.UI.WebControls.Button btnAddNew;
    //protected System.Web.UI.WebControls.Button btnDelLine;
    //protected System.Web.UI.WebControls.Button btnCancel;
    protected Company objCompany = new Company();
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
            GetStockDocumentDetails(InvoiceID);//InvoiceID 38875
            GetRejectionCode();
           
        }
        if (!Page.IsPostBack)
        {
            CheckInvoiceExist();
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
        this.grdList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdList_ItemDataBound);
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
        dsStock = oStockDal.GetStockDocumentDetails(InvID, "INV");  //sp "GetStockDocumentDetails"
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
                    Session["CurrencyType"] = dsStock.Tables[0].Rows[0]["CurrencyCode"].ToString();
                    ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), " invoiceAction>>oStockDal.CurrencyType >" + Convert.ToString(dsStock.Tables[0].Rows[0]["CurrencyCode"]));
                    #endregion
                    lblinvoicestatus.Text = dsStock.Tables[0].Rows[0]["Status"].ToString();
                    ViewState["StatusID"] = dsStock.Tables[0].Rows[0]["StatusID"].ToString();
                    try
                    {
                        //lblCreditNo.Text	= dsStock.Tables[0].Rows[0]["Status"].ToString();
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

                #region Tables[1].Rows.Count
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
        /*
        ddlRejectionCode.Items.Insert(0," Select Comments ");
        ddlRejectionCode.Items.Insert(1," Invoice Error-CreditNote Requested ");
        ddlRejectionCode.Items.Insert(2," Goods/Services not rec'd-CreditNote Requested ");
        ddlRejectionCode.Items.Insert(3," Not my responsibility (Incorrect auth string/dept only)");
        ddlRejectionCode.Items.Insert(4," Please change account coding");
        ddlRejectionCode.Items.Insert(5," Invoice Error (Data does not match image or doc to be deleted)");	
        */
    }
    #endregion 
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
        sqlDA.SelectCommand.Parameters.Add("@Type", "INV");

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

    /*public void getInvalidPO()
    {
        DataTable dt1 = new DataTable();
        dt1 = GetInvalidPONumberFor();
        if (dt1.Rows.Count > 0)
        {
            for(int i=0;i<dt1.Rows.Count;i++)
            {
                lblInvalidPO.Text =lblInvalidPO.Text+", "+ Convert.ToString(dt1.Rows[i]["PurchanseOrderNo"]);
            }
        }
        //gvOrders.DataSource = GetInvalidPONumberFor();
        //gvOrders.DataBind();
    }*/

    #region  grdInvCur_ItemDataBound
    private void grdInvCur_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        try
        {
            //added by kuntalkarar on 28thMarch2017
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
            //*************************
            //				System.Web.UI.WebControls.Label lblTotal=(System.Web.UI.WebControls.Label)e.Item.FindControl("lblTotReceived");
            //				decimal total=GetAfterDecimalCalculatedValue(Convert.ToDecimal(lblTotal.Text));
            //				
            //				if(total==0)
            //				{
            //					lblTotal.Text="0.00";
            //				}
            //				else
            //
            //				{
            //					lblTotal.Text=GetAfterDecimalCalculatedValue(Convert.ToDecimal(lblTotal.Text)).ToString();
            //				}
            //**************************** sougata for 0 0.00*********************************
            decimal lblTotReceived__11 = Convert.ToDecimal(((System.Web.UI.WebControls.Label)e.Item.FindControl("lblTotReceived")).Text);
            if (lblTotReceived__11 == 0)
            {
                ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblTotReceived")).Text = "0.00";
            }

            //****************************end sougata ********************************************




            //*********************************
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
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblFooterTotalReceived")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(dTotalReceived));
            //**************************************sougata for showing 0 to 0.00******************************//
            decimal lblFooterTotalReceived_11 = Convert.ToDecimal(((System.Web.UI.WebControls.Label)e.Item.FindControl("lblFooterTotalReceived")).Text);
            if (lblFooterTotalReceived_11 == 0)
            {
                ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblFooterTotalReceived")).Text = "0.00";
            }
            //***************************************end sougata*************************//

            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblFooterTotInvoiced")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(dTotalInvoiced));
            dVerience = dTotalInvoiced - dTotalReceived;
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblFooterVarience")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(dVerience));

            StockDal oSDal = new StockDal();
            oSDal.PropInvoiced = dTotalInvoiced;

            oSDal.PropReceived = dTotalReceived;
            oSDal.PropVarience = dVerience;
        }
    }
    #endregion
    // ==============================================================================================================

    #region GetPOActionURL
    // <A href='#' onclick="<%#GetPOActionURL(DataBinder.Eval(Container.DataItem,"POID"))%>">
    // <A href='GrdRcdDetail.aspx?POID=<%#DataBinder.Eval(Container.DataItem,"POID")%>'>
    protected string GetPOActionURL(object oPoID, object oGoodsRecdID)
    {
        string strPoID = Convert.ToString(oPoID);
        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "1).oPoID >" + strPoID);
        int iGoodsRecdID = Convert.ToInt32(oGoodsRecdID);

        string strURL = "";

        strURL = "javascript:window.open('GrdRcdDetail.aspx?POID=" + strPoID + "&GRID=" + iGoodsRecdID + "&Type=INV' ,'popupwindow','width=1400,height=550,scrollbars=1,resizable=1,target=_new');";
        ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "2).strURL >" + strURL);
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
            takeAction("INV", Convert.ToInt32(Request["InvoiceID"]), 1);
        }
        else if (iActionType == 0)
        {
            takeAction("INV", Convert.ToInt32(Request["InvoiceID"]), 0);
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
            Response.Write("<script>return confirm('Are you sure that you want to delete?');</script>");
            return;
        }

        Invoice objinvoice = new Invoice();
        string strComments = tbComments.Text.Trim();

        //			int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
        //			if(UserTypeID == 3 || UserTypeID ==2)
        //			{	
        //	bool ret=	SaveDetailData();		
        iApproverStatusID = 19;
        int UpdateStatus = UpdateSTOCKInvoiceCOMMON_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), strComments, 19);
        if (UpdateStatus >= 1)
        {
            //	objinvoice.UpdateInvoiceStatusLogApproverWise( System.Convert.ToInt32(Session["eInvoiceID"]),  System.Convert.ToInt32(Session["UserID"]), UserTypeID,  iApproverStatusID,  tbComments.Text.Trim(),"");
            //	objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(iRetValForUpdate), System.Convert.ToInt32(Session["UserID"]), 3, 19, txtComment.Text.Trim());
            doAction(0);
            Response.Write("<script>alert('Invoice Approved Successfully');</script>");
            Response.Write("<script>opener.location.reload(true);</script>");
            Response.Write("<script>self.close();</script>");
        }
        else if (UpdateStatus == -1)
            Response.Write("<script>alert('Error in approving.');</script>");

        //			}
        /*		else
                {	
                    iApproverStatusID =19;
                    int ApprovedStatus = objinvoice.GetApprovedStatus(System.Convert.ToInt32(Session["eInvoiceID"].ToString()),System.Convert.ToInt32(Session["UserID"].ToString()),strNew_AccountCategory,strNew_ActivityCode);	
                    if(ApprovedStatus ==1)
                        Response.Write("<script>alert('Invoice Cannot be Approved');</script>"); 
                    else
                    {
                        objinvoice.UpdateInvoiceStatusLogApproverWise( System.Convert.ToInt32(Session["eInvoiceID"]),  System.Convert.ToInt32(Session["UserID"]), UserTypeID,  iApproverStatusID,  strComments,"");
                //		objinvoice.UpdateInvoiceStatusLogApproverWise_CN(System.Convert.ToInt32(Session["eInvoiceID"]), System.Convert.ToInt32(Session["UserID"]), 1, 19, txtComment.Text.Trim());	
                        doAction(0);
                        lblMessege.Text = "Invoice Approved Successfully";
                        Response.Write("<script>alert('Invoice Approved Successfully');</script>");
                        Response.Write("<script>opener.location.reload(true);</script>");
                        Response.Write("<script>self.close();</script>");
                    }
                }	*/
    }
    #endregion
    #region btnReject_Click
    private void btnReject_Click(object sender, System.EventArgs e)
    {
        //			bool retVal = true;
        iApproverStatusID = 6;
        Invoice objinvoice = new Invoice();
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
        //	int Result = objinvoice.UpdateStatusToReject(System.Convert.ToInt32(Session["eInvoiceID"].ToString()),ddlRejectionCode.SelectedItem.Text.ToString(),strComments,Convert.ToInt32(ddlRejectionCode.SelectedIndex),System.Convert.ToInt32(Session["UserID"].ToString()),"");

        //			if(Result==2)
        //			{	
        //				lblMessege.Text="Invoice Rejected Successfully";
        //				int UserTypeID = objinvoice.GetUserType(System.Convert.ToInt32(Session["UserID"].ToString()));
        //				objinvoice.UpdateInvoiceStatusLogApproverWise( System.Convert.ToInt32(Session["eInvoiceID"]),  System.Convert.ToInt32(Session["UserID"]), UserTypeID,  iApproverStatusID,  strComments,ddlRejectionCode.SelectedItem.Text.Trim());
        //				doAction(0);
        //				Response.Write("<script>alert('Invoice Rejected Successfully');</script>");
        //				Response.Write("<script>opener.location.reload(true);</script>");
        //				Response.Write("<script>self.close();</script>");
        //
        //			}
        int Result = UpdateSTOCKInvoiceCOMMON_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), strComments, 6);
        if (Result >= 1)
        {
            doAction(0);
            Response.Write("<script>alert('Invoice Rejected Successfully');</script>");
            Response.Write("<script>opener.location.reload(true);</script>");
            Response.Write("<script>self.close();</script>");
        }
        else
        {
            Response.Write("<script>alert('Invoice Already Rejected');</script>");
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
            int StatusUpdate = UpdateSTOCKInvoiceCOMMON_Generic(System.Convert.ToInt32(Session["eInvoiceID"]), strComments, 7);
            if (StatusUpdate >= 1)
            {
                //		objinvoice.UpdateInvoiceStatusLogApproverWise( System.Convert.ToInt32(Session["eInvoiceID"]),  System.Convert.ToInt32(Session["UserID"]), UserTypeID,  iApproverStatusID,  strComments,"");
                lblMessege.Text = "Invoice Deleted Successfully";
                doAction(0);
                Response.Write("<script>alert('Invoice Deleted Successfully.');</script>");
                Response.Write("<script>opener.location.reload(true);</script>");
                Response.Write("<script>self.close();</script>");
            }
            else
            {
                Response.Write("<script>alert('Invoice cannot be deleted');</script>");
            }
        }
    }
    #endregion

    //==========================================================================================
    //==========================================================================================
    //==========================================================================================
    //==========================================================================================

    #region SelectedIndexChanged_ddlCodingDescription
    protected void SelectedIndexChanged_ddlCodingDescription(Object sender, System.EventArgs e)
    {
        int compid = 0;
        DataTable dt = new DataTable();
        DropDownList ddl = ((DropDownList)sender);
        int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;

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
                //	SetValueForCombo(((DropDownList) grdList.Items[i].FindControl("ddlDepartment1")),ddlDepartment1);

                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
            }
            return;
        }

        int CodingDescriptionID = Convert.ToInt32(ddl.SelectedValue);

        if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
        {
            if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue == "--Select--")
            {
                GetAllComboCodesAddNew();
            }
        }
    }
    #endregion
    #region SelectedIndexChanged_ddlBuyerCompanyCode
    protected void SelectedIndexChanged_ddlBuyerCompanyCode(object sender, System.EventArgs e)		//am object
    {
        int i = 0;
        DropDownList ddl = ((DropDownList)sender);
        i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;

        if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            GetAllComboCodesForNominalRefresh();
        else
        {
            ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Clear();
            ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");

            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

            ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
            ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");

            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Clear();
            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
        }

    }
    #endregion
    #region SelectedIndexChanged_ddlDepartment
    protected void SelectedIndexChanged_ddlDepartment(object sender, System.EventArgs e)		//am object
    {
        int inominalCodeID = 0, iDepartmentCodeID = 0;
        int iDCompID = 0;
        DropDownList ddl = ((DropDownList)sender);
        int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
        if (Convert.ToString(ddl.SelectedValue) == "--Select--")
        {
            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

            //				((DropDownList)	grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
            //				((DropDownList)	grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0,"--Select--");
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

            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Clear();
            ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");

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
            //				((DropDownList)	grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
            //				((DropDownList)	grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0,"--Select--");
        }
        else
            inominalCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);

        if (Convert.ToString(ddl.SelectedValue) != "--Select--" && ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue == "--Select--")
        {
            DataSet Dst = new DataSet();
            Dst = GetNominalCodeAgainstDepartmentANDCompany(iDepartmentCodeID, iDCompID);
            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = Dst;
            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
            ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

        }

    }
    #endregion
    #region  SelectedIndexChanged_ddlNominalCode
    protected void SelectedIndexChanged_ddlNominalCode(object sender, System.EventArgs e)//am object
    {
        //			int inominalCodeID=0;
        //			int iDepartmentCodeID=0;
        //			int iCodingDescriptionID=0;
        DropDownList ddl = ((DropDownList)sender);
        int i = ((DataGridItem)ddl.Parent.Parent).ItemIndex;
        if (Convert.ToString(ddl.SelectedValue) != "--Select--")
        {
            if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue != "--Select--")
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue == "--Select--")
                    GetAllComboCodesAddNew();
            }
        }

    }
    #endregion
    #region GetAllComboCodesForNominalRefresh
    private void GetAllComboCodesForNominalRefresh()
    {
        int compid = 0;
        DataTable dt = null;
        string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlProject1 = "", ddlNominalCode1 = "";
        for (int i = 0; i <= grdList.Items.Count - 1; i++)
        {
            if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            {
                compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
            }
            ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
            ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
            ddlProject1 = ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
            ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

            if (compid != 0)
            {
                dt = objInvoice.GetGridDepartmentList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);

                dt = objInvoice.GetGridProjectList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);

                dt = objInvoice.GetGridNominalCodeList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);

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
    #region CheckInvoiceExist
    private void CheckInvoiceExist()
    {
        int RowCnt = 1;
        string ConsString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        SqlConnection sqlConn = new SqlConnection(ConsString);

        SqlDataAdapter sqlDA = new SqlDataAdapter("ups_GetGenericCodingChange", sqlConn);
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(Request.QueryString["InvoiceID"]));
        sqlDA.SelectCommand.Parameters.Add("@Type", "INV");
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
        // here i need to use dataset instead of datareader
        if (ds.Tables[0].Rows.Count > 0)
        {
            //	RowCnt=Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            RowCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["cnt"].ToString());
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
        BindGrid(RowCnt);


        for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
        {
            ((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedIndex = -1;
            SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")), ds.Tables[1].Rows[i]["CompanyID"].ToString());
        }
        //			if(ds.Tables[1].Rows.Count > 0)
        GetAllComboCodes();
        //			else
        //				GetAllComboCodesAddNew();

        for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
        {
            ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex = -1;
            SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ds.Tables[1].Rows[i]["DepartmentID"].ToString());

            if (((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedIndex > 0)
            {
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedIndex = -1;
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ds.Tables[1].Rows[i]["CodingDescriptionID"].ToString());

                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedIndex = -1;
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ds.Tables[1].Rows[i]["NominalCodeID"].ToString());

                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedIndex = -1;
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ds.Tables[1].Rows[i]["ProjectCodeID"].ToString());
            }
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
    #region protected DataTable GetBlankTable(int iNoofRow)
    protected DataTable GetBlankTable(int iNoofRow)
    {
        DataTable tbl = null;
        int InvoiceID = 0;
        double dtmpNetAmt = 0;
        InvoiceID = Convert.ToInt32(Request["InvoiceID"]);
        dtmpNetAmt = GetNetAmt(InvoiceID);
        ViewState["NetAmt"] = dtmpNetAmt;

        if (iNoofRow <= 1)
        {
            tbl = new DataTable();
            DataRow nRow;
            tbl.Columns.Add("NetValue");
            for (int i = 0; i < iNoofRow; i++)
            {
                nRow = tbl.NewRow();
                nRow["NetValue"] = dtmpNetAmt;
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
                for (int i = 0; i < iNoofRow; i++)
                {
                    nRow = tbl.NewRow();
                    nRow["NetValue"] = ds.Tables[1].Rows[i]["netvalue"];
                    tbl.Rows.Add(nRow);
                }
            }
        }
        return tbl;
    }
    #endregion
    #region GetAllComboCodes
    private void GetAllComboCodes()
    {
        int compid = 0;
        DataTable dt = null;
        string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlProject1 = "", ddlNominalCode1 = "";
        for (int i = 0; i <= grdList.Items.Count - 1; i++)
        {
            if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            {
                if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "")
                    compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                else
                    compid = 0;
            }
            ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
            ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
            ddlProject1 = ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
            ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

            if (compid != 0)
            {
                dt = objInvoice.GetGridDepartmentList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);


                dt = objInvoice.GetGridProjectList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);



                dt = objInvoice.GetGridNominalCodeList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);



                dt = objInvoice.GetGridCodingDescriptionList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataTextField = "DDescription";
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataValueField = "CodingDescriptionID";
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")), ddlCodingDescription1);


            }
            //				else	//Commented by Sourayan	12-03-2009
            //					Response.Write("<script>alert('Please select a company');</script>");
        }
    }
    #endregion
    #region GetAllComboCodesAddNew
    private void GetAllComboCodesAddNew()
    {
        int compid = 0;
        DataTable dt = null;
        string ddlCodingDescription1 = "", ddlDepartment1 = "", ddlProject1 = "", ddlNominalCode1 = "";
        for (int i = 0; i <= grdList.Items.Count - 1; i++)
        {

            if (((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "--Select--")
            {
                compid = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
            }
            ddlCodingDescription1 = ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue.ToString().Trim();
            ddlDepartment1 = ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue.ToString().Trim();
            ddlProject1 = ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue.ToString().Trim();
            ddlNominalCode1 = ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue.ToString().Trim();

            //				if(compid!=0)
            //				{
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

                    dt = objInvoice.GetGridProjectList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);

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

                    dt = objInvoice.GetGridProjectList(compid);
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);
                }
            }

            else if (((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue != "--Select--")
            {
                DataSet dsDeptNom = new DataSet();
                int iCoding = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue);
                dsDeptNom = GetDepartmentANDNominalAgainstCodingDescID(iCoding, compid);
                if (dsDeptNom.Tables[0].Rows.Count > 0)
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataSource = dsDeptNom.Tables[0];
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataTextField = "Department";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataValueField = "DepartmentID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")), ddlDepartment1);
                }
                if (dsDeptNom.Tables[1].Rows.Count > 0)
                {
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataSource = dsDeptNom.Tables[1];
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataTextField = "NominalCode";
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataValueField = "NominalCodeID";
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).DataBind();
                    ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
                    SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")), ddlNominalCode1);
                }

                dt = objInvoice.GetGridProjectList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);
            }
            //				else if(	((DropDownList)	grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue == "--Select--")
            //				{
            //					GetAllComboCodes();
            //				}
            else
            {
                dt = objInvoice.GetGridProjectList(compid);
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataSource = dt;
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataTextField = "ProjectName";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataValueField = "ProjectID";
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).DataBind();
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
                SetValueForCombo(((DropDownList)grdList.Items[i].FindControl("ddlProject1")), ddlProject1);

                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");

                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Clear();
                ((DropDownList)grdList.Items[i].FindControl("ddlProject1")).Items.Insert(0, "--Select--");
            }

            //				}
            //				else
            //					Response.Write("<script>alert('Please select a company');</script>");
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
    #region btnAddNew_Click
    private void btnAddNew_Click(object sender, System.EventArgs e)
    {
        Populate(0, "a");
    }

    private void Populate(int irow, string acttype)
    {
        int i;
        int j;
        string[] strValue = new string[1];
        DataRow dr;
        ArrayList arrLstComp = new ArrayList();
        ArrayList arrLstCode = new ArrayList();
        ArrayList arrLstDept = new ArrayList();
        ArrayList arrLstNomi = new ArrayList();
        ArrayList arrLstProjCode = new ArrayList();
        DataSet ds = ((DataSet)(ViewState["data"]));
        ds.Tables[0].Rows.Clear();

        for (i = 0; i <= grdList.Items.Count - 1; i++)
        {
            if (irow == 0 && acttype == "a")
            {
                arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);
                arrLstCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedItem.Value);
                arrLstDept.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedItem.Value);
                arrLstNomi.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedItem.Value);
                arrLstProjCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))).SelectedItem.Value);

                strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
                dr = ds.Tables[0].NewRow();
                for (j = 0; j < 1; j++)
                {
                    dr[j] = strValue[j].ToString();
                }
                ds.Tables[0].Rows.Add(dr);
            }
            else if (irow != i && acttype == "d")
            {
                arrLstComp.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedItem.Value);
                arrLstCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedItem.Value);
                arrLstDept.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedItem.Value);
                arrLstNomi.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedItem.Value);
                arrLstProjCode.Add(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))).SelectedItem.Value);


                strValue[0] = ((System.Web.UI.WebControls.TextBox)(grdList.Items[i].FindControl("txtNetVal"))).Text;
                dr = ds.Tables[0].NewRow();
                for (j = 0; j < 1; j++)
                {
                    dr[j] = strValue[j].ToString();
                }
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

        i = 0;
        for (i = 0; i <= arrLstComp.Count - 1; i++)
        {
            ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).SelectedIndex = -1;
            ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlBuyerCompanyCode"))).Items.FindByValue(arrLstComp[i].ToString()).Selected = true;
        }
        if (acttype == "d")
            GetAllComboCodesForNominalRefresh();
        else
            GetAllComboCodes();

        for (i = 0; i <= arrLstCode.Count - 1; i++)
        {
            ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))).SelectedIndex = -1;
            SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlCodingDescription1"))), arrLstCode[i].ToString());
        }

        for (i = 0; i <= arrLstDept.Count - 1; i++)
        {
            ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))).SelectedIndex = -1;
            SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlDepartment1"))), arrLstDept[i].ToString());
        }

        for (i = 0; i <= arrLstNomi.Count - 1; i++)
        {
            ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))).SelectedIndex = -1;
            SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlNominalCode1"))), arrLstNomi[i].ToString());
        }

        for (i = 0; i <= arrLstProjCode.Count - 1; i++)
        {
            ((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))).SelectedIndex = -1;
            SetValueForCombo(((System.Web.UI.WebControls.DropDownList)(grdList.Items[i].FindControl("ddlProject1"))), arrLstProjCode[i].ToString());
        }

    }
    #endregion
    #region btnDelLine_Click
    private void btnDelLine_Click(object sender, System.EventArgs e)
    {
        int i = 0;
        //			if(Convert.ToInt32(Convert.ToInt32(Session["UserTypeID"]))!= 1)
        //			{
        for (i = 0; i <= grdList.Items.Count - 1; i++)
        {
            if (((System.Web.UI.WebControls.CheckBox)grdList.Items[i].FindControl("chkBox")).Checked)
            {
                if (grdList.Items.Count - 1 > 0)
                {
                    DataSet ds = ((DataSet)(ViewState["data"]));
                    int rowid = int.Parse(grdList.Items[i].Cells[0].Text);
                    //	DataRow row;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (i <= grdList.Items.Count - 1)
                        {
                            if (grdList.Items[i].ItemIndex + 1 == rowid)
                            {
                                if (((System.Web.UI.WebControls.DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue.ToString() != "-1")
                                {
                                    row.Delete();
                                    Populate(grdList.Items[i].ItemIndex, "d");
                                    break;
                                }
                            }
                        }
                        i += 1;
                    }
                }
                else
                {
                    DataRow dr;
                    DataSet ds = ((DataSet)(ViewState["data"]));
                    ds.Tables[0].Rows.Clear();
                    dr = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(dr);
                    BindGrid(1);
                }
            }
        }
        //			}
    }
    #endregion
    public void ErrorLog(string sPathName, string sErrMsg)
    {
        StreamWriter sw = new StreamWriter(sPathName, true);
        sw.WriteLine(DateTime.Now + ": " + sErrMsg);
        sw.Flush();
        sw.Close();
    }
    #region grdList_ItemDataBound
    private void grdList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            int j = e.Item.DataSetIndex + 1;
            e.Item.Cells[0].Text = j.ToString();
            ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), Session["CompanyID"].ToString());
            DataTable dt = objCompany.GetCompanyListForPurchaseInvoiceLog(Convert.ToInt32(Session["CompanyID"]));		//Session["CompanyID"]  Session["BuyerCID"]
            ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataSource = dt;
            ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataTextField = "CompanyName";
            ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataValueField = "CompanyID";
            ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).DataBind();
            ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).Items.Insert(0, "--Select--");
            //	((DropDownList)	e.Item.FindControl("ddlBuyerCompanyCode")).Enabled=false;

            GetAllComboCodes();

            try
            {
                if (Session["BuyerID_STK"] != null)
                    ((DropDownList)e.Item.FindControl("ddlBuyerCompanyCode")).SelectedValue = Session["BuyerID_STK"].ToString().Trim();		//Request["DDCompanyID"].Trim();
            }
            catch { }

            ((DropDownList)e.Item.FindControl("ddlCodingDescription1")).Items.Insert(0, "--Select--");
            ((DropDownList)e.Item.FindControl("ddlDepartment1")).Items.Insert(0, "--Select--");
            ((DropDownList)e.Item.FindControl("ddlNominalCode1")).Items.Insert(0, "--Select--");
            ((DropDownList)e.Item.FindControl("ddlProject1")).Items.Insert(0, "--Select--");

            if (((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim() != "")
            {
                dNetAmt = dNetAmt + Convert.ToDouble(((System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtNetVal")).Text.Trim());
            }
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetVal")).Text = dNetAmt.ToString();
            if (ViewState["NetAmt"] != null)
            {
                ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetInvoiceTotal")).Text = Convert.ToDouble(ViewState["NetAmt"].ToString()).ToString();
            }
        }
    }
    #endregion
    private void btnCancel_Click(object sender, System.EventArgs e)
    {

    }
    #region SaveDetailData()
    private bool SaveDetailData()
    {
        #region variables
        int InvID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
        int CompanyID = 0;
        int CodingDescriptionID = 0;
        int NominalCodeID = 0;
        int ProjectCodeID = 0;
        int DepartmentID = 0;
        int iValidFlag = 0;
        decimal NetValue = 0;
        double NetVal = 0;
        bool retval = false;
        #endregion

        for (int j = 0; j <= grdList.Items.Count - 1; j++)
        {
            if (grdList.Items[j].ItemType == ListItemType.Item || grdList.Items[j].ItemType == ListItemType.AlternatingItem)
            {
                #region Validations
                if (((DropDownList)grdList.Items[j].FindControl("ddlBuyerCompanyCode")).SelectedValue.Trim() == "--Select--")
                {
                    lblMessege.Visible = true;
                    lblMessege.Text = "Please select company code.";
                    Response.Write("<script>alert('Please select company code.');</script>");
                    iValidFlag = 1;
                    break;
                }
                if (((DropDownList)grdList.Items[j].FindControl("ddlCodingDescription1")).SelectedValue.Trim() == "--Select--")
                {
                    lblMessege.Visible = true;
                    lblMessege.Text = "Please select coding.";
                    Response.Write("<script>alert('Please select coding.');</script>");
                    iValidFlag = 1;
                    break;
                }
                if (((DropDownList)grdList.Items[j].FindControl("ddlDepartment1")).SelectedValue.Trim() == "--Select--")
                {
                    lblMessege.Visible = true;
                    lblMessege.Text = "Please select department name.";
                    Response.Write("<script>alert('Please select department name.');</script>");
                    iValidFlag = 1;
                    break;
                }
                if (((DropDownList)grdList.Items[j].FindControl("ddlNominalCode1")).SelectedValue.Trim() == "--Select--")
                {
                    lblMessege.Visible = true;
                    lblMessege.Text = "Please select nominal code.";
                    Response.Write("<script>alert('Please select nominal code.');</script>");
                    iValidFlag = 1;
                    break;
                }
                /*	if( ((DropDownList)grdList.Items[j].FindControl("ddlProject1")).SelectedValue.Trim() == "--Select--")
                    {
                        lblErrorMsg.Visible=true;
                        lblErrorMsg.Text ="please select project code.";
                        Response.Write("<script>alert('please select project code.');</script>");
                        iValidFlag=1;
                        break;
                    }
                */
                if (((System.Web.UI.WebControls.TextBox)grdList.Items[j].FindControl("txtNetVal")).Text.Trim() == "")
                {
                    lblMessege.Visible = true;
                    lblMessege.Text = "Please enter Net Value for coding at line(s).";
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
            dtXML.Columns.Add("ProjectCodeID");
            dtXML.Columns.Add("NetValue");
            dtXML.Columns.Add("CodingValue");
            DataRow DR = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            if (Convert.ToDouble(ViewState["OriginalNetAmount"]) == NetVal)
            {
                for (int i = 0; i <= grdList.Items.Count - 1; i++)
                {
                    #region Getting DropDown Values
                    if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue) > 0)
                    {
                        CompanyID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlBuyerCompanyCode")).SelectedValue);
                    }

                    if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue) > 0)
                    {
                        CodingDescriptionID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlCodingDescription1")).SelectedValue);
                    }

                    if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue) > 0)
                    {
                        DepartmentID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlDepartment1")).SelectedValue);
                    }

                    if (Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue) > 0)
                    {
                        NominalCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlNominalCode1")).SelectedValue);
                    }

                    if (Convert.ToString(((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue) != "--Select--")
                    {
                        ProjectCodeID = Convert.ToInt32(((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue);
                    }
                    else
                        ProjectCodeID = 0;

                    NetValue = 0;
                    if (((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text != "")
                    {
                        if (Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text) > 0)
                        {
                            NetValue = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdList.Items[i].FindControl("txtNetVal")).Text);
                        }
                    }
                    #endregion

                    if (NetValue > 0)
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
                        //		sb.Append("<ProjectCodeID>").Append(Convert.ToString(ProjectCodeID)).Append("</ProjectCodeID>");
                        if (((DropDownList)grdList.Items[i].FindControl("ddlProject1")).SelectedValue.Trim() == "--Select--")
                            sb.Append("<ProjectCodeID>").Append(Convert.ToString("0")).Append("</ProjectCodeID>");
                        else
                            sb.Append("<ProjectCodeID>").Append(Convert.ToString(ProjectCodeID)).Append("</ProjectCodeID>");

                        sb.Append("<NetValue>").Append(Convert.ToString(NetValue)).Append("</NetValue>");
                        sb.Append("<CodingValue>").Append(Convert.ToString(ViewState["OriginalNetAmount"])).Append("</CodingValue>");
                        sb.Append("</Rowss>");
                    }
                }
                dsXML.Tables.Add(dtXML);
                sb.Append("</Root>");
                string strXmlText = sb.ToString();
                sb = null;


                int retvalalue = InsertCodingChangeValuesByDeleting(strXmlText, InvID);
                if (retvalalue > 0)
                    retval = true;
                else
                    retval = false;
            }
            else
            {
                lblMessege.Visible = true;
                lblMessege.Text = "Total Net Value for Coding and Net Invoice Total not equal";
                Response.Write("<script>alert('Total Net Value for Coding and Net Invoice Total not equal.');</script>");
            }
        }

        return retval;
    }
    #endregion
    //==========================================================================================
    //==========================================================================================
    #region GetNetAmt(int InvoiceID)
    private double GetNetAmt(int InvoiceID)
    {
        double NetAmt = 0;
        string sSql = "select nettotal from Invoice where Invoiceid=" + InvoiceID;
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
                    NetAmt = Convert.ToDouble(dr[0]);
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
        return NetAmt;
    }
    #endregion
    #region GetNominalCodeAgainstDepartmentANDCompany
    public DataSet GetNominalCodeAgainstDepartmentANDCompany(int iDepartmentCodeID, int iDCompID)
    {
        DataSet Dst = new DataSet();
        SqlDataAdapter sqlDA = null;
        SqlConnection sqlConn = null;
        string sSql = "SELECT NominalCodeID,NominalCode FROM NominalCode WHERE NominalCodeID in(SELECT NominalCodeID FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND BuyerCompanyID=" + iDCompID + ")";
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
        //	string sSql="SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE DepartmentCodeID="+iDepartmentCodeID+" AND NominalCodeID ="+iNominal+" AND BuyerCompanyID="+iDCompID+"";
        sqlConn = new SqlConnection(ConsString);
        sqlDA = new SqlDataAdapter("sp_GetDepartmentANDNominalAgainstCodingDescID", sqlConn);
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        sqlDA.SelectCommand.Parameters.Add("@CodingDescriptionID", iCodingID);
        sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompID);
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
    #region GetCodingDescriptionAgainstDepartmentANDNominal
    public DataSet GetCodingDescriptionAgainstDepartmentANDNominal(int iDepartmentCodeID, int iNominal, int iDCompID)
    {
        DataSet Dst = new DataSet();
        SqlDataAdapter sqlDA = null;
        SqlConnection sqlConn = null;
        string sSql = "SELECT CodingDescriptionID,DDescription FROM CodingDescription WHERE DepartmentCodeID=" + iDepartmentCodeID + " AND NominalCodeID =" + iNominal + " AND BuyerCompanyID=" + iDCompID + "";
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
    #region UpdateSTOCKInvoiceCOMMON_Generic
    public int UpdateSTOCKInvoiceCOMMON_Generic(int InvoiceID, string Comments, int ApproverStatus)
    {
        int iCount = 0;
        SqlParameter sqlOutputParam = null;
        SqlCommand sqlCmd = null;
        SqlConnection sqlConn = new SqlConnection(ConsString);
        sqlCmd = new SqlCommand("stp_UpdateSTOCKInvoiceCOMMON_Generic", sqlConn);
        sqlCmd.CommandType = CommandType.StoredProcedure;
        sqlCmd.Parameters.Add("@UserID", System.Convert.ToInt32(Session["UserID"]));
        sqlCmd.Parameters.Add("@InvoiceID", InvoiceID);
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
    #region InsertCodingChangeValuesByDeleting
    public int InsertCodingChangeValuesByDeleting(string XMLText, int invoiceID)
    {
        int RetVal = 0;
        SqlCommand sqlCmd = null;
        SqlConnection sqlConn = new SqlConnection(ConsString);

        try
        {
            sqlCmd = new SqlCommand("sp_InsertCodingChangeValuesByDeleting", sqlConn);
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
    #region GetAfterDecimalCalculatedValue(decimal _Value)	//Amitava
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
}