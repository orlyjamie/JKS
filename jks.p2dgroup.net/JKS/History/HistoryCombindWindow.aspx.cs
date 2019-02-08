using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ETC_History_HistoryCombindWindow : System.Web.UI.Page
{
    #region : Global Variables
    string strID = "0";
    string strDocType = string.Empty;

    string InvoiceDate = string.Empty;
    string DocStatus = string.Empty;
    string VoucherNumber = string.Empty;

    #endregion

    #region : Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
            Response.Redirect("../../close_win.aspx");
        if (!IsPostBack)
        {
            FetchQueryStringValue();
        }
    }
    #endregion

    #region : Methods
    public void FetchQueryStringValue()
    {
        string TiffUrl = string.Empty;
        string ActionWindowUrl = string.Empty;

             string NewVendorClass=string.Empty;
             NewVendorClass = System.Convert.ToString(Request.QueryString["NewVendorClass"]);

        strID = System.Convert.ToString(Request.QueryString["InvoiceID"]);
        strDocType = System.Convert.ToString(Request.QueryString["DocType"]);


   
        //-------------------------------

        InvoiceDate = System.Convert.ToString(Request.QueryString["InvoiceDate"]);
        DocStatus = System.Convert.ToString(Request.QueryString["DocStatus"]);
        VoucherNumber = System.Convert.ToString(Request.QueryString["VoucherNumber"]);

        //----------------------------     
       

        // Set Url For TiffViewer Window---------------
        TiffUrl = "../../TiffViewerDefault.aspx?ID=" + strID + "&Type=" + strDocType;
        TiffWindow.Attributes.Add("src", TiffUrl);
        // Set Url For Action Widow---------------
        //NewVendorClass added by kuntalkarar on 6thJanuary2017
        ActionWindowUrl = "ActionHistoryNew.aspx?NewVendorClass=" + NewVendorClass + "&InvoiceID=" + strID + "&DocType=" + strDocType + "&InvoiceDate= " + InvoiceDate + "&DocStatus=" + DocStatus + "&VoucherNumber=" + VoucherNumber;

        ActionWindow.Attributes.Add("src", ActionWindowUrl);
    }
    #endregion
}