using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;

namespace JKS
{
    /// <summary>
    /// Created by : Rinku Santra
    /// Created date: 01-06-2012
    /// Summary description for WeeklySummary.
    /// </summary>
    public class WeeklySummary : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region HtmlControls
        protected System.Web.UI.HtmlControls.HtmlInputText txtCompany;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWeekNo;
        protected System.Web.UI.HtmlControls.HtmlInputText txtDateClosed;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtDepartment;
        protected System.Web.UI.HtmlControls.HtmlInputText txtPeriodNo;
        protected System.Web.UI.HtmlControls.HtmlInputText txtClosedBy;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnMonday;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnTuesday;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnWednessday;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnThursday;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnFriday;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnSaturday;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnSunday;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnSave;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMonday;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesday;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednessday;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTursday;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFriday;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturday;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSunday;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednessdayStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayCover2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayCover1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayCover1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayCover1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayCover1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayCover1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayCover1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalCover1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayCover2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayCover2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayCover2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayCover2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayCover2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayCover2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalCover2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayCover3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayCover3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayCover3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayCover3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayCover3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayCover3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayCover3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalCover3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayTotalCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayTotalCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayTotalCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayTotalCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayTotalCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayTotalCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayTotalCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayAverageCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayAverageCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayAverageCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayAverageCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayAverageCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayAverageCoverame;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayAverageCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtAverageCover;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField6;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField7;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalTotalNet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayVat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayVat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayVat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayVat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayVat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayVat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayVat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalVat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalTotalGross;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField8;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField9;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField10;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField11;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField12;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField13;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField14;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalField15;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalPettyCashTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayFieldTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayFieldTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayFieldTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayFieldTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayFieldTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayFieldTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayFieldTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalFieldTotal;
        protected System.Web.UI.HtmlControls.HtmlInputText txtMondayFieldVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayFieldVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtWednesdayFieldVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtThursdayFieldVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFridayFieldVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSaturdayFieldVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSundayFieldVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalFieldVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock1Opening;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock1Closing;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock1Movement;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock2Opening;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock2Closing;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock2Movement;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock3Opening;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock3Closing;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock3Movement;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock4Opening;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock4Closing;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStock4Movement;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalOpening;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalClosing;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStockTotalMovement;
        protected System.Web.UI.HtmlControls.HtmlInputText txtLabour1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtLabour4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtNationalInsurance1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtNationalInsurance4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalLabour1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalLabour4;
        protected System.Web.UI.HtmlControls.HtmlInputText txtLabour2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtLabour5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtNationalInsurance2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtNationalInsurance5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalLabour2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalLabour5;
        protected System.Web.UI.HtmlControls.HtmlInputText txtLabour3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalLabour;
        protected System.Web.UI.HtmlControls.HtmlInputText txtNationalInsurance3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalNationalInsurance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalLabour3;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalLabourCost;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTotalLabourPercentage;
        protected System.Web.UI.HtmlControls.HtmlInputText txtStartersinWeek;
        protected System.Web.UI.HtmlControls.HtmlInputText txtLeaversinWeek;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFloatOpening;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSafeOpening;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFloatIncrease;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSafeIncrease;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFloatClosing;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSafeClosing;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFixedFloat;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFixedSafe;
        protected System.Web.UI.HtmlControls.HtmlInputText txtFloatVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSafeVariance;
        protected System.Web.UI.HtmlControls.HtmlInputText txtSavedBy;
        protected System.Web.UI.HtmlControls.HtmlInputText SaveDate;
        protected System.Web.UI.WebControls.Label lblCovers1;
        protected System.Web.UI.WebControls.Label lblCovers2;
        protected System.Web.UI.WebControls.Label lblCovers3;
        protected System.Web.UI.WebControls.Label lblFiled1;
        protected System.Web.UI.WebControls.Label lblField2;
        protected System.Web.UI.WebControls.Label lblField3;
        protected System.Web.UI.WebControls.Label lblField4;
        protected System.Web.UI.WebControls.Label lblField5;
        protected System.Web.UI.WebControls.Label lblField6;
        protected System.Web.UI.WebControls.Label lblField7;
        protected System.Web.UI.WebControls.Label lblField8;
        protected System.Web.UI.WebControls.Label lblField9;
        protected System.Web.UI.WebControls.Label lblField10;
        protected System.Web.UI.WebControls.Label lblField11;
        protected System.Web.UI.WebControls.Label lblField12;
        protected System.Web.UI.WebControls.Label lblField13;
        protected System.Web.UI.WebControls.Label lblField14;
        protected System.Web.UI.WebControls.Label lblField15;
        protected System.Web.UI.WebControls.Label lblStock1;
        protected System.Web.UI.WebControls.Label lblStock2;
        protected System.Web.UI.WebControls.Label lblStock3;
        protected System.Web.UI.WebControls.Label lblStock4;
        protected System.Web.UI.WebControls.Label lblLabour1;
        protected System.Web.UI.WebControls.Label lblLabour4;
        protected System.Web.UI.WebControls.Label lblLabour2;
        protected System.Web.UI.WebControls.Label lblLabour5;
        protected System.Web.UI.WebControls.Label lblLabour3;
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Button btnClosed2;
        protected System.Web.UI.HtmlControls.HtmlInputText txtTuesdayCover1;
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Page.RegisterStartupScript("reg", "<script>alert('Sorry,your Session is expired');</script>");
                Response.Redirect("../../etcdefault.aspx");
            }

            if (!IsPostBack)
            {
                btnClosed2.Attributes.Add("onclick", "JavaScript:return fn_Closed();");
                ViewState["chkVariance"] = "0";
                if (Request.QueryString["dep"] != null && Convert.ToString(Request.QueryString["dep"]) != "")
                {
                    int depID = 0;
                    try
                    {
                        depID = Convert.ToInt32(CMScode.DecryptString(Request.QueryString["dep"]));
                    }
                    catch (Exception ex)
                    {
                        CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "DecryptString : <br /> " + ex.Message.ToString());
                        if (Session["weekstartdate"] == null)
                            Response.Redirect("CMS.aspx");
                        else
                            Response.Redirect("CMSHistroy.aspx");
                    }
                    finally
                    {
                        LoadLabelName(depID);
                        LoadData(depID);
                    }
                }
                else
                {
                    if (Session["weekstartdate"] == null)
                        Response.Redirect("CMS.aspx");
                    else
                        Response.Redirect("CMSHistroy.aspx");
                }
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
            this.btnClosed2.Click += new System.EventHandler(this.btnClosed2_Click);
            this.btnMonday.ServerClick += new System.EventHandler(this.btnMonday_ServerClick);
            this.btnTuesday.ServerClick += new System.EventHandler(this.btnTuesday_ServerClick);
            this.btnWednessday.ServerClick += new System.EventHandler(this.btnWednessday_ServerClick);
            this.btnThursday.ServerClick += new System.EventHandler(this.btnThursday_ServerClick);
            this.btnFriday.ServerClick += new System.EventHandler(this.btnFriday_ServerClick);
            this.btnSaturday.ServerClick += new System.EventHandler(this.btnSaturday_ServerClick);
            this.btnSunday.ServerClick += new System.EventHandler(this.btnSunday_ServerClick);
            this.btnSave.ServerClick += new System.EventHandler(this.btnSave_ServerClick);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion


        private void btnSave_ServerClick(object sender, System.EventArgs e)
        {
            int iReturnValue = 0;
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            sqlCmd = new SqlCommand("Sp_SaveDataWeeklySummary_CMS", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToInt32(ViewState["CMSWeekID"]) > 0)
                sqlCmd.Parameters.Add("@CMSWeekID", Convert.ToInt32(ViewState["CMSWeekID"]));
            else
                sqlCmd.Parameters.Add("@CMSWeekID", Convert.ToInt32(0));

            sqlCmd.Parameters.Add("@SavedBy", Convert.ToInt32(Session["UserID"]));
            if (txtStock1Opening.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Stock1Opening", txtStock1Opening.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Stock1Opening", DBNull.Value);
            if (txtStock1Closing.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Stock1Closing", txtStock1Closing.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Stock1Closing", DBNull.Value);
            if (txtStock2Opening.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Stock2Opening", txtStock2Opening.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Stock2Opening", DBNull.Value);
            if (txtStock2Closing.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Stock2Closing", txtStock2Closing.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Stock2Closing", DBNull.Value);

            if (txtStock3Opening.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Stock3Opening", txtStock3Opening.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Stock3Opening", DBNull.Value);
            if (txtStock3Closing.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Stock3Closing", txtStock3Closing.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Stock3Closing", DBNull.Value);

            if (txtStock4Opening.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Stock4Opening", txtStock4Opening.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Stock4Opening", DBNull.Value);
            if (txtStock4Closing.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Stock4Closing", txtStock4Closing.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Stock4Closing", DBNull.Value);

            if (txtLabour1.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Labour1", txtLabour1.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Labour1", DBNull.Value);
            if (txtNationalInsurance1.Value.Trim() != "")
                sqlCmd.Parameters.Add("@NI1", txtNationalInsurance1.Value.Trim());
            else
                sqlCmd.Parameters.Add("@NI1", DBNull.Value);

            if (txtLabour2.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Labour2", txtLabour2.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Labour2", DBNull.Value);
            if (txtNationalInsurance2.Value.Trim() != "")
                sqlCmd.Parameters.Add("@NI2", txtNationalInsurance2.Value.Trim());
            else
                sqlCmd.Parameters.Add("@NI2", DBNull.Value);
            if (txtLabour3.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Labour3", txtLabour3.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Labour3", DBNull.Value);
            if (txtNationalInsurance3.Value.Trim() != "")
                sqlCmd.Parameters.Add("@NI3", txtNationalInsurance3.Value.Trim());
            else
                sqlCmd.Parameters.Add("@NI3", DBNull.Value);
            if (txtLabour4.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Labour4", txtLabour4.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Labour4", DBNull.Value);
            if (txtNationalInsurance4.Value.Trim() != "")
                sqlCmd.Parameters.Add("@NI4", txtNationalInsurance4.Value.Trim());
            else
                sqlCmd.Parameters.Add("@NI4", DBNull.Value);
            if (txtLabour5.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Labour5", txtLabour5.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Labour5", DBNull.Value);
            if (txtNationalInsurance5.Value.Trim() != "")
                sqlCmd.Parameters.Add("@NI5", txtNationalInsurance5.Value.Trim());
            else
                sqlCmd.Parameters.Add("@NI5", DBNull.Value);

            if (txtStartersinWeek.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Starters", txtStartersinWeek.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Starters", DBNull.Value);
            if (txtLeaversinWeek.Value.Trim() != "")
                sqlCmd.Parameters.Add("@Leavers", txtLeaversinWeek.Value.Trim());
            else
                sqlCmd.Parameters.Add("@Leavers", DBNull.Value);
            if (txtFloatOpening.Value.Trim() != "")
                sqlCmd.Parameters.Add("@FloatOpening", txtFloatOpening.Value.Trim());
            else
                sqlCmd.Parameters.Add("@FloatOpening", DBNull.Value);
            if (txtFloatIncrease.Value.Trim() != "")
                sqlCmd.Parameters.Add("@FloatIncrDecr", txtFloatIncrease.Value.Trim());
            else
                sqlCmd.Parameters.Add("@FloatIncrDecr", DBNull.Value);
            if (txtFixedFloat.Value.Trim() != "")
                sqlCmd.Parameters.Add("@FixedFloat", txtFixedFloat.Value.Trim());
            else
                sqlCmd.Parameters.Add("@FixedFloat", DBNull.Value);

            if (txtSafeOpening.Value.Trim() != "")
                sqlCmd.Parameters.Add("@SafeOpening", txtSafeOpening.Value.Trim());
            else
                sqlCmd.Parameters.Add("@SafeOpening", DBNull.Value);
            if (txtSafeIncrease.Value.Trim() != "")
                sqlCmd.Parameters.Add("@SafeIncrDecr", txtSafeIncrease.Value.Trim());
            else
                sqlCmd.Parameters.Add("@SafeIncrDecr", DBNull.Value);
            if (txtFixedSafe.Value.Trim() != "")
                sqlCmd.Parameters.Add("@FixedSafe", txtFixedSafe.Value.Trim());
            else
                sqlCmd.Parameters.Add("@FixedSafe", DBNull.Value);

            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS Weekly SaveData : <br /> " + ex.Message.ToString());
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            if (iReturnValue > 0)
            {
                Response.Write("<script>alert('Saved successfully');</script>");
                LoadData(Convert.ToInt32(CMScode.DecryptString(Request.QueryString["dep"])));
            }
            else
            {
                Response.Write("<script>alert('Error in saved');</script>");
            }
        }

        private void btnMonday_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("CMSdays.aspx?icon=1&dep=" + Convert.ToString(Request.QueryString["dep"]));
        }

        private void btnTuesday_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("CMSdays.aspx?icon=2&dep=" + Convert.ToString(Request.QueryString["dep"]));
        }

        private void btnWednessday_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("CMSdays.aspx?icon=3&dep=" + Convert.ToString(Request.QueryString["dep"]));
        }

        private void btnThursday_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("CMSdays.aspx?icon=4&dep=" + Convert.ToString(Request.QueryString["dep"]));
        }

        private void btnFriday_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("CMSdays.aspx?icon=5&dep=" + Convert.ToString(Request.QueryString["dep"]));
        }

        private void btnSunday_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("CMSdays.aspx?icon=7&dep=" + Convert.ToString(Request.QueryString["dep"]));
        }

        private void btnSaturday_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("CMSdays.aspx?icon=6&dep=" + Convert.ToString(Request.QueryString["dep"]));
        }
        private void LoadLabelName(int depID)
        {
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlcon = new SqlConnection(ConsString);
            SqlDataAdapter dap = new SqlDataAdapter("Sp_CMSFieldNames", sqlcon);
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.Add("@DepartmentID", depID);
            DataSet ds = new DataSet();
            try
            {
                sqlcon.Open();
                dap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblCovers1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Covers1"]);
                    lblCovers2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Covers2"]);
                    lblCovers3.Text = Convert.ToString(ds.Tables[0].Rows[0]["Covers3"]);
                    lblStock1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stock1"]);
                    lblStock2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stock2"]);
                    lblStock3.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stock3"]);
                    lblStock4.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stock4"]);
                    lblFiled1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field1"]);
                    lblField2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field2"]);
                    lblField3.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field3"]);
                    lblField4.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field4"]);
                    lblField5.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field5"]);
                    lblField6.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field6"]);
                    lblField7.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field7"]);
                    lblField8.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field8"]);
                    lblField9.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field9"]);
                    lblField10.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field10"]);
                    lblField11.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field11"]);
                    lblField12.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field12"]);
                    lblField13.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field13"]);
                    lblField14.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field14"]);
                    lblField15.Text = Convert.ToString(ds.Tables[0].Rows[0]["Field15"]);
                    lblLabour1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Labour1"]);
                    lblLabour2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Labour2"]);
                    lblLabour3.Text = Convert.ToString(ds.Tables[0].Rows[0]["Labour3"]);
                    lblLabour4.Text = Convert.ToString(ds.Tables[0].Rows[0]["Labour4"]);
                    lblLabour5.Text = Convert.ToString(ds.Tables[0].Rows[0]["Labour5"]);
                }
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS Weekly LabelName : <br /> " + ex.Message.ToString());
            }
            finally
            {
                ds = null;
                sqlcon.Close();
            }

        }

        private void LoadData(int depID)
        {
            string weekstartdate = "";
            ViewState["chkVariance"] = "0";
            //			if(Session["weekstartdate"]!=null && Convert.ToString(Session["weekstartdate"])!="")
            //				weekstartdate=Convert.ToString(Session["weekstartdate"]);

            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlcon = new SqlConnection(ConsString);
            SqlDataAdapter dap = new SqlDataAdapter("Sp_WeeklySummary", sqlcon);
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.Add("@UserID", Convert.ToInt32(Session["UserID"]));
            dap.SelectCommand.Parameters.Add("@DepartmentID", depID);
            if (weekstartdate != "")
                dap.SelectCommand.Parameters.Add("@weekstartdate", weekstartdate);
            else
                dap.SelectCommand.Parameters.Add("@weekstartdate", DBNull.Value);
            DataSet ds = new DataSet();
            try
            {
                sqlcon.Open();
                dap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtCompany.Value = Convert.ToString(ds.Tables[0].Rows[0]["CompanyName"]);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txtDepartment.Value = Convert.ToString(ds.Tables[1].Rows[0]["Department"]);
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    txtStock1Opening.Value = Convert.ToString(ds.Tables[2].Rows[0]["Stock1Closing"]);
                    txtStock2Opening.Value = Convert.ToString(ds.Tables[2].Rows[0]["Stock2Closing"]);
                    txtStock3Opening.Value = Convert.ToString(ds.Tables[2].Rows[0]["Stock3Closing"]);
                    txtStock4Opening.Value = Convert.ToString(ds.Tables[2].Rows[0]["Stock4Closing"]);
                    txtFloatOpening.Value = Convert.ToString(ds.Tables[2].Rows[0]["FloatClosing"]);
                    txtSafeOpening.Value = Convert.ToString(ds.Tables[2].Rows[0]["SafeClosing"]);
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    if (Convert.ToString(ds.Tables[3].Rows[0]["CMSWeekID"]) != "")
                    {
                        Session["CMSWeekID"] = Convert.ToInt32(ds.Tables[3].Rows[0]["CMSWeekID"]);
                        ViewState["CMSWeekID"] = Convert.ToInt32(ds.Tables[3].Rows[0]["CMSWeekID"]);
                    }
                    if (Convert.ToString(ds.Tables[3].Rows[0]["WeekNo"]) != "")
                        txtWeekNo.Value = Convert.ToString(ds.Tables[3].Rows[0]["WeekNo"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["WeekStartDate"]) != "")
                    {
                        txtMonday.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["WeekStartDate"]).ToString("dd/MM/yyyy");
                        txtTuesday.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["WeekStartDate"]).AddDays(1).ToString("dd/MM/yyyy");
                        txtWednessday.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["WeekStartDate"]).AddDays(2).ToString("dd/MM/yyyy");
                        txtTursday.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["WeekStartDate"]).AddDays(3).ToString("dd/MM/yyyy");
                        txtFriday.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["WeekStartDate"]).AddDays(4).ToString("dd/MM/yyyy");
                        txtSaturday.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["WeekStartDate"]).AddDays(5).ToString("dd/MM/yyyy");
                        txtSunday.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["WeekStartDate"]).AddDays(6).ToString("dd/MM/yyyy");
                    }
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Period"]) != "")
                        txtPeriodNo.Value = Convert.ToString(ds.Tables[3].Rows[0]["Period"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Status"]) != "")
                        txtStatus.Value = Convert.ToString(ds.Tables[3].Rows[0]["Status"]).ToUpper();
                    if (Convert.ToString(ds.Tables[3].Rows[0]["DateClosed"]) != "")
                        txtDateClosed.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["DateClosed"]).ToString("dd/MM/yyyy HH:mm");
                    if (Convert.ToString(ds.Tables[3].Rows[0]["ClosedBy"]) != "")
                        txtClosedBy.Value = Convert.ToString(ds.Tables[3].Rows[0]["ClosedBy"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["DateSaved"]) != "")
                        SaveDate.Value = Convert.ToDateTime(ds.Tables[3].Rows[0]["DateSaved"]).ToString("dd/MM/yyyy HH:mm");
                    if (Convert.ToString(ds.Tables[3].Rows[0]["SavedBy"]) != "")
                        txtSavedBy.Value = Convert.ToString(ds.Tables[3].Rows[0]["SavedBy"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Stock1Opening"]) != "")
                        txtStock1Opening.Value = Convert.ToString(ds.Tables[3].Rows[0]["Stock1Opening"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Stock1Closing"]) != "")
                        txtStock1Closing.Value = Convert.ToString(ds.Tables[3].Rows[0]["Stock1Closing"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Stock2Opening"]) != "")
                        txtStock2Opening.Value = Convert.ToString(ds.Tables[3].Rows[0]["Stock2Opening"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Stock2Closing"]) != "")
                        txtStock2Closing.Value = Convert.ToString(ds.Tables[3].Rows[0]["Stock2Closing"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Stock3Opening"]) != "")
                        txtStock3Opening.Value = Convert.ToString(ds.Tables[3].Rows[0]["Stock3Opening"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Stock3Closing"]) != "")
                        txtStock3Closing.Value = Convert.ToString(ds.Tables[3].Rows[0]["Stock3Closing"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Stock4Opening"]) != "")
                        txtStock4Opening.Value = Convert.ToString(ds.Tables[3].Rows[0]["Stock4Opening"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Stock4Closing"]) != "")
                        txtStock4Closing.Value = Convert.ToString(ds.Tables[3].Rows[0]["Stock4Closing"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Labour1"]) != "")
                        txtLabour1.Value = Convert.ToString(ds.Tables[3].Rows[0]["Labour1"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["NI1"]) != "")
                        txtNationalInsurance1.Value = Convert.ToString(ds.Tables[3].Rows[0]["NI1"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Labour2"]) != "")
                        txtLabour2.Value = Convert.ToString(ds.Tables[3].Rows[0]["Labour2"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["NI2"]) != "")
                        txtNationalInsurance2.Value = Convert.ToString(ds.Tables[3].Rows[0]["NI2"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Labour3"]) != "")
                        txtLabour3.Value = Convert.ToString(ds.Tables[3].Rows[0]["Labour3"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["NI3"]) != "")
                        txtNationalInsurance3.Value = Convert.ToString(ds.Tables[3].Rows[0]["NI3"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Labour4"]) != "")
                        txtLabour4.Value = Convert.ToString(ds.Tables[3].Rows[0]["Labour4"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["NI4"]) != "")
                        txtNationalInsurance4.Value = Convert.ToString(ds.Tables[3].Rows[0]["NI4"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Labour5"]) != "")
                        txtLabour5.Value = Convert.ToString(ds.Tables[3].Rows[0]["Labour5"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["NI5"]) != "")
                        txtNationalInsurance5.Value = Convert.ToString(ds.Tables[3].Rows[0]["NI5"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Starters"]) != "")
                        txtStartersinWeek.Value = Convert.ToString(ds.Tables[3].Rows[0]["Starters"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["Leavers"]) != "")
                        txtLeaversinWeek.Value = Convert.ToString(ds.Tables[3].Rows[0]["Leavers"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["FloatOpening"]) != "")
                        txtFloatOpening.Value = Convert.ToString(ds.Tables[3].Rows[0]["FloatOpening"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["SafeOpening"]) != "")
                        txtSafeOpening.Value = Convert.ToString(ds.Tables[3].Rows[0]["SafeOpening"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["FloatIncrDecr"]) != "")
                        txtFloatIncrease.Value = Convert.ToString(ds.Tables[3].Rows[0]["FloatIncrDecr"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["SafeIncrDecr"]) != "")
                        txtSafeIncrease.Value = Convert.ToString(ds.Tables[3].Rows[0]["SafeIncrDecr"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["FixedFloat"]) != "")
                        txtFixedFloat.Value = Convert.ToString(ds.Tables[3].Rows[0]["FixedFloat"]);
                    if (Convert.ToString(ds.Tables[3].Rows[0]["FixedSafe"]) != "")
                        txtFixedSafe.Value = Convert.ToString(ds.Tables[3].Rows[0]["FixedSafe"]);

                }
                if (ds.Tables[4].Rows.Count > 0)
                {

                    if (Convert.ToString(ds.Tables[4].Rows[0]["Date"]) != "")
                        txtMonday.Value = Convert.ToDateTime(ds.Tables[4].Rows[0]["Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Covers1"]) != "")
                        txtMondayCover1.Value = Convert.ToString(ds.Tables[4].Rows[0]["Covers1"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Covers2"]) != "")
                        txtMondayCover2.Value = Convert.ToString(ds.Tables[4].Rows[0]["Covers2"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Covers3"]) != "")
                        txtMondayCover3.Value = Convert.ToString(ds.Tables[4].Rows[0]["Covers3"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field1"]) != "")
                        txtMondayField1.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field1"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field2"]) != "")
                        txtMondayField2.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field2"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field3"]) != "")
                        txtMondayField3.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field3"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field4"]) != "")
                        txtMondayField4.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field4"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field5"]) != "")
                        txtMondayField5.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field5"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field6"]) != "")
                        txtMondayField6.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field6"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field7"]) != "")
                        txtMondayField7.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field7"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field8"]) != "")
                        txtMondayField8.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field8"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field9"]) != "")
                        txtMondayField9.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field9"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field10"]) != "")
                        txtMondayField10.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field10"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field11"]) != "")
                        txtMondayField11.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field11"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field12"]) != "")
                        txtMondayField12.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field12"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field13"]) != "")
                        txtMondayField13.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field13"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field14"]) != "")
                        txtMondayField14.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field14"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Field15"]) != "")
                        txtMondayField15.Value = Convert.ToString(ds.Tables[4].Rows[0]["Field15"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["PettyCashTotal"]) != "")
                        txtMondayPettyCashTotal.Value = Convert.ToString(ds.Tables[4].Rows[0]["PettyCashTotal"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["vat"]) != "")
                        txtMondayVat.Value = Convert.ToString(ds.Tables[4].Rows[0]["vat"]);
                    if (Convert.ToString(ds.Tables[4].Rows[0]["Variance"]) != "0")
                        ViewState["chkVariance"] = "1";
                }
                if (ds.Tables[5].Rows.Count > 0)
                {

                    if (Convert.ToString(ds.Tables[5].Rows[0]["Date"]) != "")
                        txtTuesday.Value = Convert.ToDateTime(ds.Tables[5].Rows[0]["Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Covers1"]) != "")
                        txtTuesdayCover1.Value = Convert.ToString(ds.Tables[5].Rows[0]["Covers1"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Covers2"]) != "")
                        txtTuesdayCover2.Value = Convert.ToString(ds.Tables[5].Rows[0]["Covers2"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Covers3"]) != "")
                        txtTuesdayCover3.Value = Convert.ToString(ds.Tables[5].Rows[0]["Covers3"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field1"]) != "")
                        txtTuesdayField1.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field1"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field2"]) != "")
                        txtTuesdayField2.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field2"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field3"]) != "")
                        txtTuesdayField3.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field3"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field4"]) != "")
                        txtTuesdayField4.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field4"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field5"]) != "")
                        txtTuesdayField5.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field5"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field6"]) != "")
                        txtTuesdayField6.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field6"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field7"]) != "")
                        txtTuesdayField7.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field7"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field8"]) != "")
                        txtTuesdayField8.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field8"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field9"]) != "")
                        txtTuesdayField9.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field9"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field10"]) != "")
                        txtTuesdayField10.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field10"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field11"]) != "")
                        txtTuesdayField11.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field11"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field12"]) != "")
                        txtTuesdayField12.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field12"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field13"]) != "")
                        txtTuesdayField13.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field13"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field14"]) != "")
                        txtTuesdayField14.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field14"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Field15"]) != "")
                        txtTuesdayField15.Value = Convert.ToString(ds.Tables[5].Rows[0]["Field15"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["PettyCashTotal"]) != "")
                        txtTuesdayPettyCashTotal.Value = Convert.ToString(ds.Tables[5].Rows[0]["PettyCashTotal"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["vat"]) != "")
                        txtTuesdayVat.Value = Convert.ToString(ds.Tables[5].Rows[0]["vat"]);
                    if (Convert.ToString(ds.Tables[5].Rows[0]["Variance"]) != "0")
                        ViewState["chkVariance"] = "1";

                }
                if (ds.Tables[6].Rows.Count > 0)
                {

                    if (Convert.ToString(ds.Tables[6].Rows[0]["Date"]) != "")
                        txtWednessday.Value = Convert.ToDateTime(ds.Tables[6].Rows[0]["Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Covers1"]) != "")
                        txtWednesdayCover1.Value = Convert.ToString(ds.Tables[6].Rows[0]["Covers1"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Covers2"]) != "")
                        txtWednesdayCover2.Value = Convert.ToString(ds.Tables[6].Rows[0]["Covers2"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Covers3"]) != "")
                        txtWednesdayCover3.Value = Convert.ToString(ds.Tables[6].Rows[0]["Covers3"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field1"]) != "")
                        txtWednesdayField1.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field1"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field2"]) != "")
                        txtWednesdayField2.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field2"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field3"]) != "")
                        txtWednesdayField3.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field3"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field4"]) != "")
                        txtWednesdayField4.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field4"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field5"]) != "")
                        txtWednesdayField5.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field5"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field6"]) != "")
                        txtWednesdayField6.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field6"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field7"]) != "")
                        txtWednesdayField7.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field7"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field8"]) != "")
                        txtWednesdayField8.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field8"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field9"]) != "")
                        txtWednesdayField9.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field9"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field10"]) != "")
                        txtWednesdayField10.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field10"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field11"]) != "")
                        txtWednesdayField11.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field11"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field12"]) != "")
                        txtWednesdayField12.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field12"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field13"]) != "")
                        txtWednesdayField13.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field13"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field14"]) != "")
                        txtWednesdayField14.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field14"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Field15"]) != "")
                        txtWednesdayField15.Value = Convert.ToString(ds.Tables[6].Rows[0]["Field15"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["PettyCashTotal"]) != "")
                        txtWednesdayPettyCashTotal.Value = Convert.ToString(ds.Tables[6].Rows[0]["PettyCashTotal"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["vat"]) != "")
                        txtWednesdayVat.Value = Convert.ToString(ds.Tables[6].Rows[0]["vat"]);
                    if (Convert.ToString(ds.Tables[6].Rows[0]["Variance"]) != "0")
                        ViewState["chkVariance"] = "1";

                }
                if (ds.Tables[7].Rows.Count > 0)
                {

                    if (Convert.ToString(ds.Tables[7].Rows[0]["Date"]) != "")
                        txtTursday.Value = Convert.ToDateTime(ds.Tables[7].Rows[0]["Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Covers1"]) != "")
                        txtThursdayCover1.Value = Convert.ToString(ds.Tables[7].Rows[0]["Covers1"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Covers2"]) != "")
                        txtThursdayCover2.Value = Convert.ToString(ds.Tables[7].Rows[0]["Covers2"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Covers3"]) != "")
                        txtThursdayCover3.Value = Convert.ToString(ds.Tables[7].Rows[0]["Covers3"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field1"]) != "")
                        txtThursdayField1.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field1"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field2"]) != "")
                        txtThursdayField2.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field2"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field3"]) != "")
                        txtThursdayField3.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field3"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field4"]) != "")
                        txtThursdayField4.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field4"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field5"]) != "")
                        txtThursdayField5.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field5"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field6"]) != "")
                        txtThursdayField6.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field6"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field7"]) != "")
                        txtThursdayField7.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field7"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field8"]) != "")
                        txtThursdayField8.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field8"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field9"]) != "")
                        txtThursdayField9.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field9"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field10"]) != "")
                        txtThursdayField10.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field10"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field11"]) != "")
                        txtThursdayField11.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field11"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field12"]) != "")
                        txtThursdayField12.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field12"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field13"]) != "")
                        txtThursdayField13.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field13"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field14"]) != "")
                        txtThursdayField14.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field14"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Field15"]) != "")
                        txtThursdayField15.Value = Convert.ToString(ds.Tables[7].Rows[0]["Field15"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["PettyCashTotal"]) != "")
                        txtThursdayPettyCashTotal.Value = Convert.ToString(ds.Tables[7].Rows[0]["PettyCashTotal"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["vat"]) != "")
                        txtThursdayVat.Value = Convert.ToString(ds.Tables[7].Rows[0]["vat"]);
                    if (Convert.ToString(ds.Tables[7].Rows[0]["Variance"]) != "0")
                        ViewState["chkVariance"] = "1";

                }
                if (ds.Tables[8].Rows.Count > 0)
                {

                    if (Convert.ToString(ds.Tables[8].Rows[0]["Date"]) != "")
                        txtFriday.Value = Convert.ToDateTime(ds.Tables[8].Rows[0]["Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Covers1"]) != "")
                        txtFridayCover1.Value = Convert.ToString(ds.Tables[8].Rows[0]["Covers1"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Covers2"]) != "")
                        txtFridayCover2.Value = Convert.ToString(ds.Tables[8].Rows[0]["Covers2"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Covers3"]) != "")
                        txtFridayCover3.Value = Convert.ToString(ds.Tables[8].Rows[0]["Covers3"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field1"]) != "")
                        txtFridayField1.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field1"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field2"]) != "")
                        txtFridayField2.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field2"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field3"]) != "")
                        txtFridayField3.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field3"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field4"]) != "")
                        txtFridayField4.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field4"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field5"]) != "")
                        txtFridayField5.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field5"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field6"]) != "")
                        txtFridayField6.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field6"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field7"]) != "")
                        txtFridayField7.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field7"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field8"]) != "")
                        txtFridayField8.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field8"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field9"]) != "")
                        txtFridayField9.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field9"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field10"]) != "")
                        txtFridayField10.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field10"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field11"]) != "")
                        txtFridayField11.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field11"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field12"]) != "")
                        txtFridayField12.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field12"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field13"]) != "")
                        txtFridayField13.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field13"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field14"]) != "")
                        txtFridayField14.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field14"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Field15"]) != "")
                        txtFridayField15.Value = Convert.ToString(ds.Tables[8].Rows[0]["Field15"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["PettyCashTotal"]) != "")
                        txtFridayPettyCashTotal.Value = Convert.ToString(ds.Tables[8].Rows[0]["PettyCashTotal"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["vat"]) != "")
                        txtFridayVat.Value = Convert.ToString(ds.Tables[8].Rows[0]["vat"]);
                    if (Convert.ToString(ds.Tables[8].Rows[0]["Variance"]) != "0")
                        ViewState["chkVariance"] = "1";

                }
                if (ds.Tables[9].Rows.Count > 0)
                {

                    if (Convert.ToString(ds.Tables[9].Rows[0]["Date"]) != "")
                        txtSaturday.Value = Convert.ToDateTime(ds.Tables[9].Rows[0]["Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Covers1"]) != "")
                        txtSaturdayCover1.Value = Convert.ToString(ds.Tables[9].Rows[0]["Covers1"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Covers2"]) != "")
                        txtSaturdayCover2.Value = Convert.ToString(ds.Tables[9].Rows[0]["Covers2"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Covers3"]) != "")
                        txtSaturdayCover3.Value = Convert.ToString(ds.Tables[9].Rows[0]["Covers3"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field1"]) != "")
                        txtSaturdayField1.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field1"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field2"]) != "")
                        txtSaturdayField2.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field2"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field3"]) != "")
                        txtSaturdayField3.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field3"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field4"]) != "")
                        txtSaturdayField4.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field4"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field5"]) != "")
                        txtSaturdayField5.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field5"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field6"]) != "")
                        txtSaturdayField6.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field6"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field7"]) != "")
                        txtSaturdayField7.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field7"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field8"]) != "")
                        txtSaturdayField8.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field8"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field9"]) != "")
                        txtSaturdayField9.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field9"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field10"]) != "")
                        txtSaturdayField10.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field10"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field11"]) != "")
                        txtSaturdayField11.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field11"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field12"]) != "")
                        txtSaturdayField12.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field12"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field13"]) != "")
                        txtSaturdayField13.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field13"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field14"]) != "")
                        txtSaturdayField14.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field14"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Field15"]) != "")
                        txtSaturdayField15.Value = Convert.ToString(ds.Tables[9].Rows[0]["Field15"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["PettyCashTotal"]) != "")
                        txtSaturdayPettyCashTotal.Value = Convert.ToString(ds.Tables[9].Rows[0]["PettyCashTotal"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["vat"]) != "")
                        txtSaturdayVat.Value = Convert.ToString(ds.Tables[9].Rows[0]["vat"]);
                    if (Convert.ToString(ds.Tables[9].Rows[0]["Variance"]) != "0")
                        ViewState["chkVariance"] = "1";

                }
                if (ds.Tables[10].Rows.Count > 0)
                {

                    if (Convert.ToString(ds.Tables[10].Rows[0]["Date"]) != "")
                        txtSunday.Value = Convert.ToDateTime(ds.Tables[10].Rows[0]["Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Covers1"]) != "")
                        txtSundayCover1.Value = Convert.ToString(ds.Tables[10].Rows[0]["Covers1"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Covers2"]) != "")
                        txtSundayCover2.Value = Convert.ToString(ds.Tables[10].Rows[0]["Covers2"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Covers3"]) != "")
                        txtSundayCover3.Value = Convert.ToString(ds.Tables[10].Rows[0]["Covers3"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field1"]) != "")
                        txtSundayField1.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field1"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field2"]) != "")
                        txtSundayField2.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field2"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field3"]) != "")
                        txtSundayField3.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field3"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field4"]) != "")
                        txtSundayField4.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field4"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field5"]) != "")
                        txtSundayField5.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field5"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field6"]) != "")
                        txtSundayField6.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field6"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field7"]) != "")
                        txtSundayField7.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field7"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field8"]) != "")
                        txtSundayField8.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field8"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field9"]) != "")
                        txtSundayField9.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field9"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field10"]) != "")
                        txtSundayField10.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field10"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field11"]) != "")
                        txtSundayField11.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field11"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field12"]) != "")
                        txtSundayField12.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field12"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field13"]) != "")
                        txtSundayField13.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field13"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field14"]) != "")
                        txtSundayField14.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field14"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Field15"]) != "")
                        txtSundayField15.Value = Convert.ToString(ds.Tables[10].Rows[0]["Field15"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["PettyCashTotal"]) != "")
                        txtSundayPettyCashTotal.Value = Convert.ToString(ds.Tables[10].Rows[0]["PettyCashTotal"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["vat"]) != "")
                        txtSundayVat.Value = Convert.ToString(ds.Tables[10].Rows[0]["vat"]);
                    if (Convert.ToString(ds.Tables[10].Rows[0]["Variance"]) != "0")
                        ViewState["chkVariance"] = "1";

                }
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS Weekly LoadData : <br /> " + ex.Message.ToString());
            }
            finally
            {
                ds = null;
                dap.Dispose();
                sqlcon.Close();
                Page.RegisterStartupScript("reg", "<script>FinalTotalCalculation();</script>");
            }

        }
        private void clear()
        {
            #region
            txtCompany.Value = "";
            txtWeekNo.Value = "";
            txtDateClosed.Value = "";
            txtStatus.Value = "";
            txtDepartment.Value = "";
            txtPeriodNo.Value = "";
            txtClosedBy.Value = "";
            txtMonday.Value = "";
            txtTuesday.Value = "";
            txtWednessday.Value = "";
            txtTursday.Value = "";
            txtFriday.Value = "";
            txtSaturday.Value = "";
            txtSunday.Value = "";
            txtMondayStatus.Value = "";
            txtTuesdayStatus.Value = "";
            txtWednessdayStatus.Value = "";
            txtThursdayStatus.Value = "";
            txtFridayStatus.Value = "";
            txtSaturdayStatus.Value = "";
            txtSundayStatus.Value = "";
            txtMondayCover2.Value = "-";
            txtMondayCover1.Value = "-";
            txtTuesdayCover1.Value = "-";
            txtWednesdayCover1.Value = "-";
            txtThursdayCover1.Value = "-";
            txtFridayCover1.Value = "-";
            txtSaturdayCover1.Value = "-";
            txtSundayCover1.Value = "-";
            txtTotalCover1.Value = "0";
            txtTuesdayCover2.Value = "-";
            txtWednesdayCover2.Value = "-";
            txtThursdayCover2.Value = "-";
            txtFridayCover2.Value = "-";
            txtSaturdayCover2.Value = "-";
            txtSundayCover2.Value = "-";
            txtTotalCover2.Value = "0";
            txtMondayCover3.Value = "-";
            txtTuesdayCover3.Value = "-";
            txtWednesdayCover3.Value = "-";
            txtThursdayCover3.Value = "-";
            txtFridayCover3.Value = "-";
            txtSaturdayCover3.Value = "-";
            txtSundayCover3.Value = "-";
            txtTotalCover3.Value = "0";
            txtMondayTotalCover.Value = "0";
            txtTuesdayTotalCover.Value = "0";
            txtWednesdayTotalCover.Value = "0";
            txtThursdayTotalCover.Value = "0";
            txtFridayTotalCover.Value = "0";
            txtSaturdayTotalCover.Value = "0";
            txtSundayTotalCover.Value = "0";
            txtTotalCover.Value = "0";
            txtMondayAverageCover.Value = "-";
            txtTuesdayAverageCover.Value = "-";
            txtWednesdayAverageCover.Value = "-";
            txtThursdayAverageCover.Value = "-";
            txtFridayAverageCover.Value = "-";
            txtSaturdayAverageCoverame.Value = "-";
            txtSundayAverageCover.Value = "-";
            txtAverageCover.Value = "0";
            txtMondayField1.Value = "-";
            txtTuesdayField1.Value = "-";
            txtWednesdayField1.Value = "-";
            txtThursdayField1.Value = "-";
            txtFridayField1.Value = "-";
            txtSaturdayField1.Value = "-";
            txtSundayField1.Value = "-";
            txtTotalField1.Value = "0";
            txtMondayField2.Value = "-";
            txtTuesdayField2.Value = "-";
            txtWednesdayField2.Value = "-";
            txtThursdayField2.Value = "-";
            txtFridayField2.Value = "-";
            txtSaturdayField2.Value = "-";
            txtSundayField2.Value = "-";
            txtTotalField2.Value = "0";
            txtMondayField3.Value = "-";
            txtTuesdayField3.Value = "-";
            txtWednesdayField3.Value = "-";
            txtThursdayField3.Value = "-";
            txtFridayField3.Value = "-";
            txtSaturdayField3.Value = "-";
            txtSundayField3.Value = "-";
            txtTotalField3.Value = "0";
            txtMondayField4.Value = "-";
            txtTuesdayField4.Value = "-";
            txtWednesdayField4.Value = "-";
            txtThursdayField4.Value = "-";
            txtFridayField4.Value = "-";
            txtSaturdayField4.Value = "-";
            txtSundayField4.Value = "-";
            txtTotalField4.Value = "0";
            txtMondayField5.Value = "-";
            txtTuesdayField5.Value = "-";
            txtWednesdayField5.Value = "-";
            txtThursdayField5.Value = "-";
            txtFridayField5.Value = "-";
            txtSaturdayField5.Value = "-";
            txtSundayField5.Value = "-";
            txtTotalField5.Value = "-";
            txtMondayField6.Value = "-";
            txtTuesdayField6.Value = "-";
            txtWednesdayField6.Value = "-";
            txtThursdayField6.Value = "-";
            txtFridayField6.Value = "-";
            txtSaturdayField6.Value = "-";
            txtSundayField6.Value = "-";
            txtTotalField6.Value = "0";
            txtMondayField7.Value = "-";
            txtTuesdayField7.Value = "-";
            txtWednesdayField7.Value = "-";
            txtThursdayField7.Value = "-";
            txtFridayField7.Value = "-";
            txtSaturdayField7.Value = "-";
            txtSundayField7.Value = "-";
            txtTotalField7.Value = "0";
            txtMondayTotalNet.Value = "0";
            txtTuesdayTotalNet.Value = "0";
            txtWednesdayTotalNet.Value = "0";
            txtThursdayTotalNet.Value = "0";
            txtFridayTotalNet.Value = "0";
            txtSaturdayTotalNet.Value = "0";
            txtSundayTotalNet.Value = "0";
            txtTotalTotalNet.Value = "0";
            txtMondayVat.Value = "-";
            txtTuesdayVat.Value = "-";
            txtWednesdayVat.Value = "-";
            txtThursdayVat.Value = "-";
            txtFridayVat.Value = "-";
            txtSaturdayVat.Value = "-";
            txtSundayVat.Value = "-";
            txtTotalVat.Value = "0";
            txtMondayTotalGross.Value = "0";
            txtTuesdayTotalGross.Value = "0";
            txtWednesdayTotalGross.Value = "0";
            txtThursdayTotalGross.Value = "0";
            txtFridayTotalGross.Value = "0";
            txtSaturdayTotalGross.Value = "0";
            txtSundayTotalGross.Value = "0";
            txtTotalTotalGross.Value = "0";
            txtMondayField8.Value = "-";
            txtTuesdayField8.Value = "-";
            txtWednesdayField8.Value = "-";
            txtThursdayField8.Value = "-";
            txtFridayField8.Value = "-";
            txtSaturdayField8.Value = "-";
            txtSundayField8.Value = "-";
            txtTotalField8.Value = "-";
            txtMondayField9.Value = "-";
            txtTuesdayField9.Value = "-";
            txtWednesdayField9.Value = "-";
            txtThursdayField9.Value = "-";
            txtFridayField9.Value = "-";
            txtSaturdayField9.Value = "-";
            txtSundayField9.Value = "-";
            txtTotalField9.Value = "0";
            txtMondayField10.Value = "-";
            txtTuesdayField10.Value = "-";
            txtWednesdayField10.Value = "-";
            txtThursdayField10.Value = "-";
            txtFridayField10.Value = "-";
            txtSaturdayField10.Value = "-";
            txtSundayField10.Value = "-";
            txtTotalField10.Value = "0";
            txtMondayField11.Value = "-";
            txtTuesdayField11.Value = "-";
            txtWednesdayField11.Value = "-";
            txtThursdayField11.Value = "-";
            txtFridayField11.Value = "-";
            txtSaturdayField11.Value = "-";
            txtSundayField11.Value = "-";
            txtTotalField11.Value = "0";
            txtMondayField12.Value = "-";
            txtTuesdayField12.Value = "-";
            txtWednesdayField12.Value = "-";
            txtThursdayField12.Value = "-";
            txtFridayField12.Value = "-";
            txtSaturdayField12.Value = "-";
            txtSundayField12.Value = "-";
            txtTotalField12.Value = "0";
            txtMondayField13.Value = "-";
            txtTuesdayField13.Value = "-";
            txtWednesdayField13.Value = "-";
            txtThursdayField13.Value = "-";
            txtFridayField13.Value = "-";
            txtSaturdayField13.Value = "-";
            txtSundayField13.Value = "-";
            txtTotalField13.Value = "0";
            txtMondayField14.Value = "-";
            txtTuesdayField14.Value = "-";
            txtWednesdayField14.Value = "-";
            txtThursdayField14.Value = "-";
            txtFridayField14.Value = "-";
            txtSaturdayField14.Value = "-";
            txtSundayField14.Value = "-";
            txtTotalField14.Value = "0";
            txtMondayField15.Value = "-";
            txtTuesdayField15.Value = "-";
            txtWednesdayField15.Value = "-";
            txtThursdayField15.Value = "-";
            txtFridayField15.Value = "-";
            txtSaturdayField15.Value = "-";
            txtSundayField15.Value = "-";
            txtTotalField15.Value = "0";
            txtMondayPettyCashTotal.Value = "-";
            txtTuesdayPettyCashTotal.Value = "-";
            txtWednesdayPettyCashTotal.Value = "-";
            txtThursdayPettyCashTotal.Value = "-";
            txtFridayPettyCashTotal.Value = "-";
            txtSaturdayPettyCashTotal.Value = "-";
            txtSundayPettyCashTotal.Value = "-";
            txtTotalPettyCashTotal.Value = "0";
            txtMondayFieldTotal.Value = "0";
            txtTuesdayFieldTotal.Value = "0";
            txtWednesdayFieldTotal.Value = "0";
            txtThursdayFieldTotal.Value = "0";
            txtFridayFieldTotal.Value = "0";
            txtSaturdayFieldTotal.Value = "0";
            txtSundayFieldTotal.Value = "0";
            txtTotalFieldTotal.Value = "0";
            txtMondayFieldVariance.Value = "0";
            txtTuesdayFieldVariance.Value = "0";
            txtWednesdayFieldVariance.Value = "0";
            txtThursdayFieldVariance.Value = "0";
            txtFridayFieldVariance.Value = "0";
            txtSaturdayFieldVariance.Value = "0";
            txtSundayFieldVariance.Value = "0";
            txtTotalFieldVariance.Value = "0";
            txtStock1Opening.Value = "";
            txtStock1Closing.Value = "";
            txtStock1Movement.Value = "0";
            txtStock2Opening.Value = "";
            txtStock2Closing.Value = "";
            txtStock2Movement.Value = "0";
            txtStock3Opening.Value = "";
            txtStock3Closing.Value = "";
            txtStock3Movement.Value = "0";
            txtStock4Opening.Value = "";
            txtStock4Closing.Value = "";
            txtStock4Movement.Value = "0";
            txtTotalOpening.Value = "";
            txtTotalClosing.Value = "";
            txtStockTotalMovement.Value = "0";
            txtLabour1.Value = "";
            txtLabour4.Value = "";
            txtNationalInsurance1.Value = "";
            txtNationalInsurance4.Value = "";
            txtTotalLabour1.Value = "0";
            txtTotalLabour4.Value = "0";
            txtLabour2.Value = "";
            txtLabour5.Value = "";
            txtNationalInsurance2.Value = "";
            txtNationalInsurance5.Value = "";
            txtTotalLabour2.Value = "0";
            txtTotalLabour5.Value = "0";
            txtLabour3.Value = "";
            txtTotalLabour.Value = "0";
            txtNationalInsurance3.Value = "";
            txtTotalNationalInsurance.Value = "0";
            txtTotalLabour3.Value = "0";
            txtTotalLabourCost.Value = "0";
            txtTotalLabourPercentage.Value = "0";
            txtStartersinWeek.Value = "";
            txtLeaversinWeek.Value = "";
            txtFloatOpening.Value = "";
            txtSafeOpening.Value = "";
            txtFloatIncrease.Value = "";
            txtSafeIncrease.Value = "";
            txtFloatClosing.Value = "0";
            txtSafeClosing.Value = "0";
            txtFixedFloat.Value = "";
            txtFixedSafe.Value = "";
            txtFloatVariance.Value = "0";
            txtSafeVariance.Value = "0";
            txtSavedBy.Value = "";
            SaveDate.Value = "";
            #endregion
        }

        private void btnClosed2_Click(object sender, System.EventArgs e)
        {
            int iReturnValue = 0;
            SqlCommand sqlCmd = null;
            SqlParameter sqlReturnParam = null;
            string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConsString);
            sqlCmd = new SqlCommand("Sp_ClosedWeeklySummary_CMS", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToInt32(ViewState["CMSWeekID"]) > 0)
                sqlCmd.Parameters.Add("@CMSWeekID", Convert.ToInt32(ViewState["CMSWeekID"]));
            else
                sqlCmd.Parameters.Add("@CMSWeekID", Convert.ToInt32(0));

            sqlCmd.Parameters.Add("@ClosedBy", Convert.ToInt32(Session["UserID"]));
            sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            sqlReturnParam.Direction = ParameterDirection.ReturnValue;
            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
                iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
                if (iReturnValue > 0)
                {
                    Response.Write("<script>alert('Closed successfully');</script>");
                }
            }
            catch (Exception ex)
            {
                CMScode.SendEmail("support@p2dgroup.com", "rjaiswal@vnsinfo.com.au", "", "", "CMS ERROR", "CMS Weekly ClosedWeek : <br /> " + ex.Message.ToString());
            }
            finally
            {
                sqlReturnParam = null;
                sqlCmd.Dispose();
                sqlConn.Close();
                clear();
                LoadData(Convert.ToInt32(CMScode.DecryptString(Request.QueryString["dep"])));
            }


        }

    }
}
