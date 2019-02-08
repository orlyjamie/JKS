using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CombindWindow : System.Web.UI.Page
{

    #region : Global Variables
    string strID = "0";
    string strDocType = string.Empty;
    string strDDCompanyID = "0";
    string strNewVendorClass = string.Empty;
    string strRowID = "0";
    string strRelationType = string.Empty;
    string striVat = string.Empty;
    string striGross = string.Empty;


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
        strDDCompanyID = System.Convert.ToString(Request.QueryString["DDCompanyID"]);
        strNewVendorClass = System.Convert.ToString(Request.QueryString["NewVendorClass"]);
        strRowID = System.Convert.ToString(Request.QueryString["RowID"]);
        // Set Url For TiffViewer Window
        //  TiffUrl = "../../TiffViewerDefault.aspx?ID=" + strID + "&Type=" + strDocType;
        //  TiffWindow.Attributes.Add("src", TiffUrl);
        //

        if (strDocType == "INV")
        {
            strRelationType = System.Convert.ToString(Request.QueryString["RelationType"]);
            striVat = System.Convert.ToString(Request.QueryString["iVat"]);
            striGross = System.Convert.ToString(Request.QueryString["iGross"]);

            ActionWindowUrl = "../Invoice/InvoiceActionTiffViewer.aspx?InvoiceID=" + strID + "&DDCompanyID= " + strDDCompanyID + "&NewVendorClass= " + strNewVendorClass + "&RelationType=" + strRelationType + "&iVat=" + striVat + "&iGross=" + striGross + "&RowID=" + strRowID;
        }
        else if (strDocType == "CRN")
        {
            ActionWindowUrl = "../CreditNotes/ActionCreditTiffViewer.aspx?InvoiceID=" + strID + "&DDCompanyID= " + strDDCompanyID + "&NewVendorClass= " + strNewVendorClass + "&RowID=" + strRowID;

        }

        //   TiffWindow.Attributes.Add("src", TiffUrl);
        ActionWindow.Attributes.Add("src", ActionWindowUrl);
    }
    #endregion

}