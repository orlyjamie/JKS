using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ETC_StockQC_MatchingCombindWindow : System.Web.UI.Page
{
    /// <summary>
    /// Developed by Mrinal Chakravorty on 20th January 2014
    /// </summary>
    #region : Global Variables
    string strID = "0";
    string strDocType = string.Empty;

    //  string InvoiceDate = string.Empty;
    //  string DocStatus = string.Empty;
    //  string VoucherNumber = string.Empty;

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
        strID = System.Convert.ToString(Request.QueryString["InvoiceID"]);
        strDocType = System.Convert.ToString(Request.QueryString["DocType"]);


        // Set Url For TiffViewer Window---------------
        TiffUrl = "../../TiffViewerDefault.aspx?ID=" + strID + "&Type=" + strDocType;
        TiffWindow.Attributes.Add("src", TiffUrl);
        // Set Url For Action Widow---------------

        if (strDocType == "INV")
        {
            ActionWindowUrl = "InvoiceAction.aspx?SQ=true&InvoiceID=" + strID;
        }
        else if (strDocType == "CRN")
        {
            ActionWindowUrl = "CreditStkAction.aspx?SQ=true&InvoiceID=" + strID;
        }

        ActionWindow.Attributes.Add("src", ActionWindowUrl);
    }
    #endregion
}