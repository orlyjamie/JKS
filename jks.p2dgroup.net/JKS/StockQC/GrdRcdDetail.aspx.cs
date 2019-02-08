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
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;
using Microsoft.VisualBasic;
using JKS;
using System.Text;
using System.IO;
public partial class JKS_StockQC_GrdRcdDetail_New : System.Web.UI.Page
{
    #region variables
    protected string IPONO = "";
    protected int GoodsRecgID = 0;
    protected decimal dTotalReceived = 0;
    protected decimal dTotalInvoiced = 0;
    protected decimal dVerience = 0;
    protected string Currency = "";
    protected decimal dTotRecd = 0;
    protected decimal dTotInvd = 0;
    //Start: Added ky Koushik Das 21-Dec-2017
    protected decimal dTotVat = 0;
    protected decimal dTotVatX = 0;
    //End: Added ky Koushik Das 21-Dec-2017
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //added by kuntalkarar on 12thJanuary2017 to stop multiple additoin in the  "Invoice / PO Rec" table
        dTotInvd = 0;
        dTotRecd = 0;

        //Start: Added ky Koushik Das 21-Dec-2017
        dTotVat = 0;
        dTotVatX = 0;
        //End: Added ky Koushik Das 21-Dec-2017

        //Response.Write("<script>alert('GrdRcdDetail page reached'); self.close();</script>");
        if (Request.QueryString["POID"] != null)
        {
            IPONO = Convert.ToString(Request.QueryString["POID"]);
            if (IPONO != "")
                GetPODetailValues(IPONO);
            GoodsRecgID = Convert.ToInt32(Request.QueryString["GRID"]);
            //	if(GoodsRecgID != 0 )
            GetGoodsRecdDetailByGoodsRecdID(GoodsRecgID, IPONO);
        }
    }

    //Added by Mainak 2018-05-28
    //protected void Page_LoadComplete(object sender, EventArgs e)
    //{
    //    if (Request.QueryString["POID"] != null)
    //    {
    //        IPONO = Convert.ToString(Request.QueryString["POID"]);
    //        GoodsRecgID = Convert.ToInt32(Request.QueryString["GRID"]);
    //        DataSet dsPODetail = new DataSet();
    //        StockDal objStockDal = new StockDal();
    //        dsPODetail = objStockDal.GetGoodsRecdDetailByGoodsRecdID(GoodsRecgID, IPONO);
    //        int grdGoodsRecdCount = dsPODetail.Tables[0].Rows.Count;


    //        for (int i = 0; i < grdGoodsRecdCount; i++)
    //        {
    //            string PurOrderLineNoGR = dsPODetail.Tables[0].Rows[i]["PurOrderLineNo"].ToString();
    //            string PurOrderNoGR = dsPODetail.Tables[0].Rows[i]["PurOrderNo"].ToString();
    //            string SupplierCompanyIDGR = dsPODetail.Tables[0].Rows[i]["SupplierCompanyID"].ToString();
    //            string BuyerCompanyIDGR = dsPODetail.Tables[0].Rows[i]["BuyerCompanyID"].ToString();
    //            string QuantityGR = dsPODetail.Tables[0].Rows[i]["Quantity"].ToString();
    //            string RateGR = dsPODetail.Tables[0].Rows[i]["Rate"].ToString();

    //            string PurOrderLineNoINV = dsPODetail.Tables[1].Rows[i]["PurOrderLineNo"].ToString();
    //            string PurOrderNoINV = dsPODetail.Tables[1].Rows[i]["PurOrderNo"].ToString();
    //            string SupplierCompanyIDINV = dsPODetail.Tables[1].Rows[i]["SupplierCompanyID"].ToString();
    //            string BuyerCompanyIDINV = dsPODetail.Tables[1].Rows[i]["BuyerCompanyID"].ToString();
    //            string QuantityINV = dsPODetail.Tables[1].Rows[i]["Quantity"].ToString();
    //            string RateINV = dsPODetail.Tables[1].Rows[i]["Rate"].ToString();

    //            DataGridItem dgi1 = dgInvoiced.Items[i];
    //            if ((PurOrderLineNoGR != PurOrderLineNoINV) || (PurOrderNoGR != PurOrderNoINV)
    //                || (SupplierCompanyIDGR != SupplierCompanyIDINV) || (BuyerCompanyIDGR != BuyerCompanyIDINV)
    //                || (QuantityGR != QuantityINV))
    //            {
    //                //dgi1.Cells[5].ForeColor = Color.Red;

    //                Label Label3 = dgi1.Cells[5].FindControl("Label3") as Label;
    //                Label3.Style.Add("color", "red !important;");

    //            }

    //            if ((PurOrderLineNoGR != PurOrderLineNoINV) || (PurOrderNoGR != PurOrderNoINV)
    //                || (SupplierCompanyIDGR != SupplierCompanyIDINV) || (BuyerCompanyIDGR != BuyerCompanyIDINV)
    //                || (RateGR != RateINV))
    //            {
    //                Label Label4 = dgi1.Cells[6].FindControl("Label4") as Label;
    //                Label4.Style.Add("color", "red !important;");
    //            }


    //        }


    //        //foreach (DataGridItem dgi in grdGoodsRecd.Items)
    //        //{
    //        //    foreach (DataGridItem dgi1 in dgInvoiced.Items)
    //        //    {
    //        //        Label LeftHandQty = dgi.FindControl("lblRCVQty") as Label;
    //        //        Label RightHandQty = dgi1.FindControl("Label3") as Label;

    //        //        if (dgi.Cells[0].Text == "")
    //        //        {
    //        //            dgi1.Cells[5].ForeColor = Color.Red;
    //        //        }
    //        //    }
    //        //}
    //    }
    //}

    //Modified by Mainak 2018-08-02
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (Request.QueryString["POID"] != null)
        {
            IPONO = Convert.ToString(Request.QueryString["POID"]);
            GoodsRecgID = Convert.ToInt32(Request.QueryString["GRID"]);
            DataSet dsPODetail = new DataSet();
            StockDal objStockDal = new StockDal();
            dsPODetail = objStockDal.GetGoodsRecdDetailByGoodsRecdID(GoodsRecgID, IPONO);
            int grdGoodsRecdCount = dsPODetail.Tables[0].Rows.Count;
            int grdInvoiceCount = dsPODetail.Tables[1].Rows.Count;

            for (int i = 0; i < grdGoodsRecdCount; i++)
            {
                string PurOrderLineNoGR = dsPODetail.Tables[0].Rows[i]["PurOrderLineNo"].ToString();
                string PurOrderNoGR = dsPODetail.Tables[0].Rows[i]["PurOrderNo"].ToString();
                string SupplierCompanyIDGR = dsPODetail.Tables[0].Rows[i]["SupplierCompanyID"].ToString();
                string BuyerCompanyIDGR = dsPODetail.Tables[0].Rows[i]["BuyerCompanyID"].ToString();
                string QuantityGR = dsPODetail.Tables[0].Rows[i]["Quantity"].ToString();
                string RateGR = dsPODetail.Tables[0].Rows[i]["Rate"].ToString();
                for (int j = 0; j < grdInvoiceCount; j++)
                {
                    string PurOrderLineNoINV = dsPODetail.Tables[1].Rows[j]["PurOrderLineNo"].ToString();
                    string PurOrderNoINV = dsPODetail.Tables[1].Rows[j]["PurOrderNo"].ToString();
                    string SupplierCompanyIDINV = dsPODetail.Tables[1].Rows[j]["SupplierCompanyID"].ToString();
                    string BuyerCompanyIDINV = dsPODetail.Tables[1].Rows[j]["BuyerCompanyID"].ToString();
                    string QuantityINV = dsPODetail.Tables[1].Rows[j]["Quantity"].ToString();
                    string RateINV = dsPODetail.Tables[1].Rows[j]["Rate"].ToString();

                    DataGridItem dgi1 = dgInvoiced.Items[j];

                    if (PurOrderLineNoGR == PurOrderLineNoINV)
                    {
                        // if ((SupplierCompanyIDGR != SupplierCompanyIDINV) || (BuyerCompanyIDGR != BuyerCompanyIDINV)
                        // || (QuantityGR != QuantityINV))
                        // {
                        //     //dgi1.Cells[5].ForeColor = Color.Red;

                        //     Label Label3 = dgi1.Cells[5].FindControl("Label3") as Label;
                        //     Label3.Style.Add("color", "red !important;");

                        // }
                        // if ((SupplierCompanyIDGR != SupplierCompanyIDINV) || (BuyerCompanyIDGR != BuyerCompanyIDINV)
                        //|| (RateGR != RateINV))
                        // {
                        //     Label Label4 = dgi1.Cells[6].FindControl("Label4") as Label;
                        //     Label4.Style.Add("color", "red !important;");
                        // }

                        if (QuantityGR != QuantityINV)
                        {
                            Label Label3 = dgi1.Cells[5].FindControl("Label3") as Label;
                            Label3.Style.Add("color", "red !important;");
                        }
                        if (RateGR != RateINV)
                        {
                            Label Label4 = dgi1.Cells[6].FindControl("Label4") as Label;
                            Label4.Style.Add("color", "red !important;");
                        }

                    }
                   

                }
            }

            for (int i = 0; i < grdInvoiceCount; i++)
            {
                int found = 0;
                string PurOrderLineNoINV = dsPODetail.Tables[1].Rows[i]["PurOrderLineNo"].ToString();

                for (int j = 0; j < grdGoodsRecdCount; j++)
                {
                    string PurOrderLineNoGR = dsPODetail.Tables[0].Rows[j]["PurOrderLineNo"].ToString();

                    if (PurOrderLineNoINV == PurOrderLineNoGR)
                    {
                        found = 1;
                    }
                }
                DataGridItem dgi1 = dgInvoiced.Items[i];
                if (found == 0)
                {
                    Label Label3 = dgi1.Cells[5].FindControl("Label3") as Label;
                    Label3.Style.Add("color", "red !important;");
                    Label Label4 = dgi1.Cells[6].FindControl("Label4") as Label;
                    Label4.Style.Add("color", "red !important;");
                }
            }

            //foreach (DataGridItem dgi in grdGoodsRecd.Items)
            //{
            //    foreach (DataGridItem dgi1 in dgInvoiced.Items)
            //    {
            //        Label LeftHandQty = dgi.FindControl("lblRCVQty") as Label;
            //        Label RightHandQty = dgi1.FindControl("Label3") as Label;

            //        if (dgi.Cells[0].Text == "")
            //        {
            //            dgi1.Cells[5].ForeColor = Color.Red;
            //        }
            //    }
            //}
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
        this.grdGoodsRecd.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdGoodsRecd_ItemDataBound);
        this.dgInvoiced.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgInvoiced_ItemDataBound);
        this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion

    public void ErrorLog(string sPathName, string sErrMsg)
    {
        StreamWriter sw = new StreamWriter(sPathName, true);
        sw.WriteLine(DateTime.Now + ": " + sErrMsg);
        sw.Flush();
        sw.Close();
    }
    #region GetP2DFTPURL

    protected string GetP2DFTPURL()
    {
        // string strPoID = Convert.ToString(oPoID);
        //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "1).oPoID >" + strPoID);
        //int iGoodsRecdID = Convert.ToInt32(oGoodsRecdID);

        string strURL = "";

        strURL = "javascript:window.open('http://ftp.p2dgroup.com/secureaccessclaims/ShowFiles.aspx?freetext=" + Session["Doctype_STK"].ToString() + "%3b" + Session["BuyerID_STK"].ToString() + "%3b" + Session["SupplierID_STK"].ToString() + "%3b" + Session["eInvoiceID"].ToString() + "' ,'popupwindow','width=920,height=550,scrollbars=1,resizable=1,target=_new');";
        //ErrorLog(Server.MapPath("Logs/ErrorLog.txt"), "2).strURL >" + strURL);
        return (strURL);

    }
    #endregion
    public void GetPODetailValues(string IPONO)
    {
        DataSet dsPO = new DataSet();
        StockDal objStockDal = new StockDal();
        dTotalReceived = objStockDal.PropReceived;
        dTotalInvoiced = objStockDal.PropInvoiced;
        dVerience = objStockDal.PropVarience;
        Currency = objStockDal.CurrencyType;

        lblDocumentNo.Text = IPONO.ToString();
        //kk
        Currency = Convert.ToString(Session["CurrencyType"].ToString());
        lblCurrency.Text = Currency.ToString();
    }
    private void GetGoodsRecdDetailByGoodsRecdID(int iGoodRecdID, string IPONO)
    {
        DataSet dsPODetail = new DataSet();
        // StockDal objStockDal = new StockDal();
        StockDal objStockDal = new StockDal();
        dsPODetail = objStockDal.GetGoodsRecdDetailByGoodsRecdID(iGoodRecdID, IPONO);
        if (dsPODetail != null)
        {
            if (dsPODetail.Tables[0].Rows.Count > 0)
            {
                grdGoodsRecd.DataSource = dsPODetail.Tables[0];
                grdGoodsRecd.DataBind();
            }
            if (dsPODetail.Tables[1].Rows.Count > 0)
            {
                dgInvoiced.DataSource = dsPODetail.Tables[1];
                dgInvoiced.DataBind();
            }
            if (dsPODetail.Tables[2].Rows.Count > 0)
            {
                try
                {
                    lblTotalRecvd.Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(dsPODetail.Tables[2].Rows[0]["TotalInvoiced"].ToString())));
                    lblInvoiced.Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(dsPODetail.Tables[2].Rows[0]["TotalRecd"].ToString())));
                    lblVarience.Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(dsPODetail.Tables[2].Rows[0]["Varience"].ToString())));
                }
                catch (Exception ex) { string ss = ex.Message.ToString(); }
            }
        }
    }
    private void grdGoodsRecd_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblRCVPrice")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(((Label)e.Item.FindControl("lblRCVPrice")).Text)));
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblRCVNet")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(((Label)e.Item.FindControl("lblRCVNet")).Text)));

            if (((Label)e.Item.FindControl("lblRCVNet")).Text != "")
            {
                dTotRecd = dTotRecd + Convert.ToDecimal(((System.Web.UI.WebControls.Label)e.Item.FindControl("lblRCVNet")).Text.Trim());
            }

            //Start: Added ky Koushik Das 21-Dec-2017
            Label lblVAT = e.Item.FindControl("lblVAT") as Label;
            //End: Added ky Koushik Das 21-Dec-2017

            //Start: Added ky Koushik Das 21-Dec-2017
            if (lblVAT.Text.Length == 0)
                lblVAT.Text = "0.00";

            dTotVat += Convert.ToDecimal(lblVAT.Text);
            //End: Added ky Koushik Das 21-Dec-2017
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblNetTot")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(dTotRecd));

            //Start: Added ky Koushik Das 21-Dec-2017
            Label lblVatTot = e.Item.FindControl("lblVatTot") as Label;

            lblVatTot.Text = dTotVat.ToString();
            //End: Added ky Koushik Das 21-Dec-2017
        }
    }
    private void dgInvoiced_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("Label4")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(((Label)e.Item.FindControl("Label4")).Text)));
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblInvCredit")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(Convert.ToDecimal(((Label)e.Item.FindControl("lblInvCredit")).Text)));

            if (((Label)e.Item.FindControl("lblInvCredit")).Text != "")
            {
                if (((Label)e.Item.FindControl("Label8")).Text == "CRN")
                {
                    ((Label)e.Item.FindControl("Label4")).Text = "-" + Convert.ToString(((Label)e.Item.FindControl("Label4")).Text);
                    ((Label)e.Item.FindControl("lblInvCredit")).Text = "-" + Convert.ToString(((Label)e.Item.FindControl("lblInvCredit")).Text);
                }
                dTotInvd = dTotInvd + Convert.ToDecimal(((System.Web.UI.WebControls.Label)e.Item.FindControl("lblInvCredit")).Text.Trim());
            }

            //Start: Added ky Koushik Das 21-Dec-2017
            Label lblVAT = e.Item.FindControl("lblVAT") as Label;
            //End: Added ky Koushik Das 21-Dec-2017

            //Start: Added ky Koushik Das 21-Dec-2017
            if (lblVAT.Text.Length == 0)
                lblVAT.Text = "0.00";

            dTotVatX += Convert.ToDecimal(lblVAT.Text);
            //End: Added ky Koushik Das 21-Dec-2017
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            ((System.Web.UI.WebControls.Label)e.Item.FindControl("lblInvCrd")).Text = Convert.ToString(GetAfterDecimalCalculatedValue(dTotInvd));

            //Start: Added ky Koushik Das 21-Dec-2017
            Label lblVatTot = e.Item.FindControl("lblVatTot") as Label;

            lblVatTot.Text = dTotVatX.ToString();
            //End: Added ky Koushik Das 21-Dec-2017
        }
    }
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
            string ss = Convert.ToString(yy);
            // Count = ss.Length;
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