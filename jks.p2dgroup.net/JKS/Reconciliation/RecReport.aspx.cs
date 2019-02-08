#region Directives
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;

#endregion

namespace JKS
{
    /// <summary>
    /// Summary description for RecReport.
    /// </summary>
    public partial class RecReport : CBSolutions.ETH.Web.ETC.VSPage
    {


        #region variable  declaration for pager
        int iCount = 0;
        int iEndIdx = 10;
        string strDirName = "";
        private string Group = "";
        private string Tables = "[CLIENT BATCHES]";
        private string PK = "[BATCH ID]";
        private string Fields = "(select [CompanyName] from Company where CompanyID = [CLIENT BATCHES].[CLIENT ID]) as Company,[BATCH ID] as BATCHID,[BATCH DOC TYPE] as BATCHDOCTYPE,[BATCH NAME] as BATCHNAME,[UPLOAD DATE] as UPLOADDATE,[NO FILES IN DIRECTORY] as NOFILESINDIRECTORY,[NUM OF INVOICES COPIED TO IPE INPUT] as NUMOFINVOICESCOPIEDTOIPEINPUT,[NUM OF INVOICES ARCHIVED BY QC] as NUMOFINVOICESARCHIVEDBYQC,[NUM OF INVOICES DELETED] as NUMOFINVOICESDELETED,ISNULL(ISNULL([NUM OF INVOICES COPIED TO IPE INPUT],0)-ISNULL([NUM OF INVOICES ARCHIVED BY QC],0) - ISNULL([NUM OF INVOICES DELETED],0),0) AS QCBALANCE,[BATCH NAME] as AWAITINGIMPORT";
        private string Filter = "[CLIENT ID] in	(66801,66802,69765)";
        protected int IsDropDownPostBack = 0;

        #endregion

        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            IsDropDownPostBack = 0;
            // Put user code to initialize the page here
            if (Session["UserID"] == null)
            {
                Response.Redirect("../close_win.aspx");
            }
            baseUtil.keepAlive(this);
            if (!IsPostBack)
            {
                PopulateBuyerCompany();
                Pager1.CurrentIndex = 1;
                BuildSql(1);
            }

        }
        #endregion

        #region private void BuildSql(int PageNo)
        private void BuildSql(int PageNo)
        {
            string cFilter = "";
            //Filter += " AND [UPLOAD DATE] BETWEEN CONVERT(DATETIME,'" + txtFromDate.Value + "',103) AND CONVERT(DATETIME,'" + txtToDate.Value + "',103)"; 
            cFilter = Filter;
            if (PageNo > 0)
            {
                Pager1.CurrentIndex = PageNo;
            }
            BindPagingGrid(Tables, PK, "[UPLOAD DATE]", PageNo.ToString(), iEndIdx.ToString(), Fields, cFilter, Group, "DESC");
        }
        #endregion

        #region BindPagingGrid
        public void BindPagingGrid(string sTables, string sPK, string sSort, string sPageNumber, string siEndIdx, string sFields, string scFilter, string sGroup, string sColOrder)
        {
            SqlDataReader dr = null;
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //SqlCommand sqlCmd = new SqlCommand("sp_GenericRecReport_ETC", sqlConn);

            //Add new SP as per adding new filtration with NewVendorClass (modified by Subha Das 26th Dec 2014)
            SqlCommand sqlCmd = new SqlCommand("sp_GenericRecReport_GRH_NEW", sqlConn);

            scFilter = scFilter.ToString().Replace("\t", " ");
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@Tables", sTables);
            sqlCmd.Parameters.Add("@PK", sPK);
            sqlCmd.Parameters.Add("@Sort", sSort);
            sqlCmd.Parameters.Add("@PageNumber", sPageNumber);
            sqlCmd.Parameters.Add("@PageSize", siEndIdx);
            sqlCmd.Parameters.Add("@Fields", sFields);
            sqlCmd.Parameters.Add("@Filter", scFilter);
            sqlCmd.Parameters.Add("@Group", sGroup);
            sqlCmd.Parameters.Add("@ColOrder", sColOrder);
            sqlCmd.Parameters.Add("@CompanyID", "180918");//124529 for AnchorSafety changed to 180918 for JKS
            if (ddlBuyerCompany.SelectedIndex != 0)
            {
                sqlCmd.Parameters.Add("@ChildCompanyID", Convert.ToInt32(ddlBuyerCompany.SelectedValue));
            }
            else
            {
                sqlCmd.Parameters.Add("@ChildCompanyID", DBNull.Value);
            }


            try
            {
                sqlConn.Open();
                dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    iCount = Convert.ToInt32(dr[0]);
                }
                Pager1.ItemCount = 100;
                dr.NextResult();
                grdInvCur.DataSource = dr;
                grdInvCur.DataBind();

                if ((!IsPostBack) || (IsDropDownPostBack == 1))
                {
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lbltotalupload.Text = dr["NOFILESINDIRECTORY"].ToString();
                        lblTotalImported.Text = dr["NUMOFINVOICESCOPIEDTOIPEINPUT"].ToString();
                        lblTotalProcessed.Text = dr["NUMOFINVOICESARCHIVEDBYQC"].ToString();
                        lbltotalDel.Text = dr["NUMOFINVOICESDELETED"].ToString();
                        lblTotQCBal.Text = dr["QCBALTotal"].ToString();
                        totAwaitReport.Text = Convert.ToString(CountTotalAwitingToImport_ETC());
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Scanneddocimported.Text = dr["Stock_ScannedInvoice"].ToString();
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Scanneddocimported.Text = dr["Exp_ScannedInvoice"].ToString();
                    }
                    lbltot_Scanneddocimported.Text = Convert.ToString(Convert.ToInt64(lblStock_Scanneddocimported.Text) + Convert.ToInt64(lblExp_Scanneddocimported.Text));
                    lblrec.Text = Convert.ToString(Convert.ToInt64(lblTotalProcessed.Text) - (Convert.ToInt64(totAwaitReport.Text) + Convert.ToInt64(lbltot_Scanneddocimported.Text)));


                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Edocimported.Text = dr["Stock_EDOCS"].ToString();
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Edocimported.Text = dr["Exp_EDOCS"].ToString();
                    }
                    lblTotal_Edocimported.Text = Convert.ToString(Convert.ToInt64(lblStock_Edocimported.Text) + Convert.ToInt64(lblExp_Edocimported.Text));

                    lblstsubtot.Text = Convert.ToString(Convert.ToInt64(lblStock_Scanneddocimported.Text) + Convert.ToInt64(lblStock_Edocimported.Text));
                    lblexsubtot.Text = Convert.ToString(Convert.ToInt64(lblExp_Scanneddocimported.Text) + Convert.ToInt64(lblExp_Edocimported.Text));
                    lblalltot.Text = Convert.ToString(Convert.ToInt64(lbltot_Scanneddocimported.Text) + Convert.ToInt64(lblTotal_Edocimported.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblst_InvUpload.Text = dr["Stock_INVOICEUPLOAD"].ToString();
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblexp_InvUpload.Text = dr["Exp_INVOICEUPLOAD"].ToString();
                    }
                    lbltot_InvUpload.Text = Convert.ToString(Convert.ToInt64(lblst_InvUpload.Text) + Convert.ToInt64(lblexp_InvUpload.Text));
                    lblst_total.Text = Convert.ToString(Convert.ToInt64(lblstsubtot.Text) + Convert.ToInt64(lblst_InvUpload.Text));
                    lblex_total.Text = Convert.ToString(Convert.ToInt64(lblexsubtot.Text) + Convert.ToInt64(lblexp_InvUpload.Text));
                    lbltot_total.Text = Convert.ToString(Convert.ToInt64(lblalltot.Text) + Convert.ToInt64(lbltot_InvUpload.Text));


                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblst_dbt.Text = dr["DebitNote"].ToString();
                        lblst_dbt1.Text = dr["DebitNote"].ToString();
                    }
                    lblTot_dbt.Text = Convert.ToInt64(lblst_dbt.Text).ToString();
                    lblTot_dbt1.Text = Convert.ToInt64(lblst_dbt.Text).ToString();
                    lblst_Gtotal.Text = Convert.ToString(Convert.ToInt64(lblst_total.Text) + Convert.ToInt64(lblst_dbt.Text));
                    lblex_Gtotal.Text = lblex_total.Text;
                    lbltot_Gtotal.Text = Convert.ToString(Convert.ToInt64(lblst_Gtotal.Text) + Convert.ToInt64(lblex_Gtotal.Text));


                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Delete_Before_Registered.Text = dr["Stock_Delete_Before_Registered"].ToString();
                    }


                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Delete_Before_Registered.Text = dr["Exp_Delete_Before_Registered"].ToString();
                    }
                    lbllbltot_Delete_Before_Registered.Text = Convert.ToString(Convert.ToInt64(lblStock_Delete_Before_Registered.Text) + Convert.ToInt64(lblExp_Delete_Before_Registered.Text));


                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Exported_for_Registration.Text = dr["Stock_Exported_for_Registration"].ToString();
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Exported_for_Registration.Text = dr["Exp_Exported_for_Registration"].ToString();
                    }
                    lbltot_Exported_for_Registration.Text = Convert.ToString(Convert.ToInt64(lblExp_Exported_for_Registration.Text) + Convert.ToInt64(lblStock_Exported_for_Registration.Text));


                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Awaiting_for_Registration.Text = dr["Stock_Awaiting_for_Registration"].ToString();
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblexp_Awaiting_for_Registration.Text = dr["Exp_Awaiting_for_Registration"].ToString();
                    }
                    lbltot_Awaiting_for_Registration.Text = Convert.ToString(Convert.ToInt64(lblStock_Awaiting_for_Registration.Text) + Convert.ToInt64(lblexp_Awaiting_for_Registration.Text));

                    lblstgtot_Awaiting.Text = Convert.ToString(Convert.ToInt64(lblStock_Delete_Before_Registered.Text) + Convert.ToInt64(lblStock_Exported_for_Registration.Text) + Convert.ToInt64(lblStock_Awaiting_for_Registration.Text));
                    lblexgtot_Awaiting.Text = Convert.ToString(Convert.ToInt64(lblExp_Delete_Before_Registered.Text) + Convert.ToInt64(lblExp_Exported_for_Registration.Text) + Convert.ToInt64(lblexp_Awaiting_for_Registration.Text));
                    lbltot_Awaiting.Text = Convert.ToString(Convert.ToInt64(lbllbltot_Delete_Before_Registered.Text) + Convert.ToInt64(lbltot_Exported_for_Registration.Text) + Convert.ToInt64(lbltot_Awaiting_for_Registration.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Recieved.Text = dr["Stock_Recieved"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Recieved.Text = dr["Exp_Recieved"].ToString();
                    }

                    lblTotal_Recieved.Text = Convert.ToString(Convert.ToInt64(lblStock_Recieved.Text) + Convert.ToInt64(lblExp_Recieved.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Registered.Text = dr["Stock_Registered"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Registered.Text = dr["Exp_Registered"].ToString();
                    }

                    lblTotal_Registered.Text = Convert.ToString(Convert.ToInt64(lblStock_Registered.Text) + Convert.ToInt64(lblExp_Registered.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Rejected.Text = dr["Stock_Rejected"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Rejected.Text = dr["Exp_Rejected"].ToString();
                    }

                    lblTotal_Rejected.Text = Convert.ToString(Convert.ToInt64(lblStock_Rejected.Text) + Convert.ToInt64(lblExp_Rejected.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Reopened.Text = dr["Stock_Reopened"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Reopened.Text = dr["Exp_Reopened"].ToString();
                    }

                    lblTotal_Reopened.Text = Convert.ToString(Convert.ToInt64(lblStock_Reopened.Text) + Convert.ToInt64(lblExp_Reopened.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Approved.Text = dr["Stock_Approved"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Approved.Text = dr["Exp_Approved"].ToString();
                    }

                    lblTotal_Approved.Text = Convert.ToString(Convert.ToInt64(lblStock_Approved.Text) + Convert.ToInt64(lblExp_Approved.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Paid.Text = dr["Stock_Paid"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Paid.Text = dr["Exp_Paid"].ToString();
                    }

                    lblTotal_Paid.Text = Convert.ToString(Convert.ToInt64(lblStock_Paid.Text) + Convert.ToInt64(lblExp_Paid.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStock_Deleted.Text = dr["Stock_Deleted"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExp_Deleted.Text = dr["Exp_Deleted"].ToString();
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStockUnmatched.Text = dr["Stock_ReceivedStar"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExpUnmatched.Text = dr["Exp_ReceivedStar"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStockUQuery.Text = dr["Stock_UnderQuery"].ToString();

                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExpUQuery.Text = dr["Exp_UnderQuery"].ToString();

                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStockExported.Text = dr["Stock_Exported"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExpExported.Text = dr["Exp_Exported"].ToString();
                    }

                    lblTotalExported.Text = Convert.ToString(Convert.ToInt64(lblStockExported.Text) + Convert.ToInt64(lblExpExported.Text));

                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStockSTAP.Text = dr["Stock_ApprovedSTAP"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExpSTAP.Text = dr["Exp_ApprovedSTAP"].ToString();
                    }

                    #region entry for ‘Archived’ (statusid = 36) Added by Koushik Das as on 06-Apr-2017
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblStockArchived.Text = dr["Stock_Archived"].ToString();
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lblExpArchived.Text = dr["Exp_Archived"].ToString();
                    }
                    lblTotalArchived.Text = Convert.ToString(Convert.ToInt64(lblStockArchived.Text) + Convert.ToInt64(lblExpArchived.Text));

                    //added + Convert.ToInt64(lblStockArchived.Text) with lblStock_Total.Text's rest
                    //added + Convert.ToInt64(lblExpArchived.Text) with lblExp_Total.Text's rest
                    #endregion

                    lblTotalSTAP.Text = Convert.ToString(Convert.ToInt64(lblStockSTAP.Text) + Convert.ToInt64(lblExpSTAP.Text));
                    lblTotal_Deleted.Text = Convert.ToString(Convert.ToInt64(lblStock_Deleted.Text) + Convert.ToInt64(lblExp_Deleted.Text));
                    lblTotalUnmatched.Text = Convert.ToString(Convert.ToInt64(lblStockUnmatched.Text) + Convert.ToInt64(lblExpUnmatched.Text));
                    lblTotalUQuery.Text = Convert.ToString(Convert.ToInt64(lblStockUQuery.Text) + Convert.ToInt64(lblExpUQuery.Text));
                    lblStock_Total.Text = Convert.ToString(Convert.ToInt64(lblStock_Recieved.Text) + Convert.ToInt64(lblStock_Registered.Text) + Convert.ToInt64(lblStock_Rejected.Text) + Convert.ToInt64(lblStock_Reopened.Text) + Convert.ToInt64(lblStock_Approved.Text) + Convert.ToInt64(lblStock_Paid.Text) + Convert.ToInt64(lblStock_Deleted.Text) + Convert.ToInt64(lblStockUnmatched.Text) + Convert.ToInt64(lblStockUQuery.Text) + Convert.ToInt64(lblStockExported.Text) + Convert.ToInt64(lblStockSTAP.Text) + Convert.ToInt64(lblStockArchived.Text));
                    lblExp_Total.Text = Convert.ToString(Convert.ToInt64(lblExp_Recieved.Text) + Convert.ToInt64(lblExp_Registered.Text) + Convert.ToInt64(lblExp_Rejected.Text) + Convert.ToInt64(lblExp_Reopened.Text) + Convert.ToInt64(lblExp_Approved.Text) + Convert.ToInt64(lblExp_Paid.Text) + Convert.ToInt64(lblExp_Deleted.Text) + Convert.ToInt64(lblExpUnmatched.Text) + Convert.ToInt64(lblExpUQuery.Text) + Convert.ToInt64(lblExpExported.Text) + Convert.ToInt64(lblExpSTAP.Text) + Convert.ToInt64(lblExpArchived.Text));
                    lbl_Total.Text = Convert.ToString(Convert.ToInt64(lblStock_Total.Text) + Convert.ToInt64(lblExp_Total.Text));
                    lblStock_GrandTotal.Text = Convert.ToString(Convert.ToInt64(lblStock_Total.Text) + Convert.ToInt64(lblst_dbt1.Text));
                    lblExp_GrandTotal.Text = Convert.ToString(Convert.ToInt64(lblExp_Total.Text));
                    lblGrand_Total.Text = Convert.ToString(Convert.ToInt64(lblStock_GrandTotal.Text) + Convert.ToInt64(lblExp_GrandTotal.Text));

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
            this.Pager1.Command += new System.Web.UI.WebControls.CommandEventHandler(this.Pager1_Command);
            this.Load += new System.EventHandler(this.Page_Load);
            this.ddlBuyerCompany.SelectedIndexChanged += new EventHandler(ddlBuyerCompany_SelectedIndexChanged);

        }
        #endregion

        #region private void Pager1_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        private void Pager1_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            Pager1.CurrentIndex = currnetPageIndx;
            BuildSql(currnetPageIndx);

        }
        #endregion

        #region CountTotalAwitingToImport_ETC
        private int CountTotalAwitingToImport_ETC()
        {
            int iTotalAwitingToImport = 0;
            int iBalanceDocs1 = 0;
            int iBalanceDocs2 = 0;
            int iBalanceDocs3 = 0;
            //string[] arrBuyerFolders={"Konditor and Cook Ltd","Caffe Fratelli Ltd"};	
            Company objCompany = new Company();
            DataTable dt = new DataTable();
            //			try
            //			{							
            //				dt=objCompany.GetBuyerCompanyListForTradingRelation(Convert.ToInt32(Session["CompanyID"]));	
            //			}
            //			catch{}
            try
            {	// Added by Mrinal on 20th January 2014
                if (ddlBuyerCompany.SelectedIndex != 0)
                {
                    //sqlCmd.Parameters.Add("@ChildCompanyID",Convert.ToInt32(ddlBuyerCompany.SelectedValue));
                    dt.Columns.Add("CompanyName");
                    dt.Columns.Add("CompanyID");
                    dt.AcceptChanges();
                    DataRow dr = dt.NewRow();
                    dr["CompanyID"] = Convert.ToString(Convert.ToInt32(ddlBuyerCompany.SelectedValue));
                    dr["CompanyName"] = Convert.ToString(ddlBuyerCompany.SelectedItem.Text);
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                else
                {
                    dt = objCompany.GetBuyerCompanyListForTradingRelation(Convert.ToInt32(Session["CompanyID"]));
                }


            }
            catch (Exception Ex)
            {
                string Error = Ex.ToString();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strUnProcessed_Generic = @ConfigurationManager.AppSettings["AwaitingReportFolder"].Trim() + "\\" + @"UnProcessed\";
                string strBuyerName = @ConfigurationManager.AppSettings["AwaitingReportFolder"].Trim() + "\\";
                string strIPEDuff_Generic = @ConfigurationManager.AppSettings["AwaitingReportFolderForIPEDUFF"].Trim() + "\\" + @"IPE Duff\";
                strUnProcessed_Generic += dt.Rows[i]["CompanyName"].ToString();
                strBuyerName += dt.Rows[i]["CompanyName"].ToString();
                strIPEDuff_Generic += dt.Rows[i]["CompanyName"].ToString();

                DirectoryInfo dir1 = new DirectoryInfo(strUnProcessed_Generic);
                FileInfo file1 = new FileInfo(strUnProcessed_Generic);
                DirectoryInfo dir2 = new DirectoryInfo(strBuyerName);
                FileInfo file2 = new FileInfo(strBuyerName);
                DirectoryInfo dir3 = new DirectoryInfo(strIPEDuff_Generic);
                FileInfo file3 = new FileInfo(strIPEDuff_Generic);
                if (dir1.Exists)
                {
                    DirectoryInfo[] d1 = null;
                    d1 = dir1.GetDirectories();

                    for (int j = 0; j < d1.Length; j++)
                    {
                        string strDirectoryName = d1[j].Name.Trim();
                        FileInfo[] f1 = null;
                        f1 = d1[j].GetFiles("*.xml");
                        if (f1.Length > 0)
                        {
                            iBalanceDocs1 = iBalanceDocs1 + f1.Length;
                        }
                    }
                }
                if (dir2.Exists)
                {
                    DirectoryInfo[] d2 = null;
                    d2 = dir2.GetDirectories();

                    for (int j = 0; j < d2.Length; j++)
                    {
                        string strDirectoryName = d2[j].Name.Trim();
                        FileInfo[] f2 = null;
                        f2 = d2[j].GetFiles("*.xml");
                        if (f2.Length > 0)
                        {
                            iBalanceDocs2 = iBalanceDocs2 + f2.Length;
                        }
                    }
                }
                if (dir3.Exists)
                {
                    DirectoryInfo[] d3 = null;
                    d3 = dir3.GetDirectories();

                    for (int j = 0; j < d3.Length; j++)
                    {
                        string strDirectoryName = d3[j].Name.Trim();
                        FileInfo[] f3 = null;
                        f3 = d3[j].GetFiles("*.xml");
                        if (f3.Length > 0)
                        {
                            iBalanceDocs3 = iBalanceDocs3 + f3.Length;
                        }
                    }
                }

            }

            iTotalAwitingToImport = iBalanceDocs1 + iBalanceDocs2 + iBalanceDocs3;
            return (iTotalAwitingToImport);
        }
        #endregion

        #region GetAwaitingToImport
        protected int GetAwaitingToImport(object oAWAITINGIMPORT, object oBATCHID)
        {
            strDirName = Convert.ToString(oAWAITINGIMPORT) + "_" + Convert.ToString(oBATCHID);
            int iBalanceDocs1 = 0;
            int iBalanceDocs2 = 0;
            int iBalanceDocs3 = 0;
            int iBalanceDocs4 = 0;
            int iBalanceDocs5 = 0;
            Company objCompany = new Company();
            DataTable dt = new DataTable();
            try
            {
                dt = objCompany.GetBuyerCompanyListForTradingRelation(Convert.ToInt32(Session["CompanyID"]));
            }
            catch { }

            //string[] arrBuyerFolders={"Konditor and Cook Ltd","Caffe Fratelli Ltd"};
            #region Read File Names in Directory
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strUnProcessed_Generic = @ConfigurationManager.AppSettings["AwaitingReportFolder"].Trim() + "\\" + @"UnProcessed\";
                string strBuyerName = @ConfigurationManager.AppSettings["AwaitingReportFolder"].Trim() + "\\";
                string strIPEDuff_Generic = @ConfigurationManager.AppSettings["AwaitingReportFolderForIPEDUFF"].Trim() + "\\" + @"IPE Duff\";
                string IPEUnProcessed_Generic = @ConfigurationManager.AppSettings["AwaitingReportFolder"].Trim() + "\\" + @"UnProcessed\";

                strUnProcessed_Generic += dt.Rows[i]["CompanyName"].ToString() + "\\" + strDirName.Trim();
                strBuyerName += dt.Rows[i]["CompanyName"].ToString() + "\\" + strDirName.Trim();
                strIPEDuff_Generic += dt.Rows[i]["CompanyName"].ToString() + "\\" + strDirName.Trim();
                IPEUnProcessed_Generic += strDirName.Trim();
                DirectoryInfo dir1 = new DirectoryInfo(strUnProcessed_Generic);
                FileInfo file1 = new FileInfo(strUnProcessed_Generic);
                DirectoryInfo dir2 = new DirectoryInfo(strBuyerName);
                FileInfo file2 = new FileInfo(strBuyerName);
                DirectoryInfo dir3 = new DirectoryInfo(strIPEDuff_Generic);
                FileInfo file3 = new FileInfo(strIPEDuff_Generic);
                DirectoryInfo dir4 = new DirectoryInfo(IPEUnProcessed_Generic);
                FileInfo file4 = new FileInfo(IPEUnProcessed_Generic);
                if (dir1.Exists)
                {
                    string strDirectoryName = strDirName.Trim();
                    FileInfo[] f1 = null;
                    f1 = dir1.GetFiles("*.xml");
                    int iAwitingUnprocessed = f1.Length;
                    if (f1.Length > 0)
                        iBalanceDocs1 = f1.Length;
                }
                if (dir2.Exists)
                {
                    string strDirectoryName = strDirName.Trim();
                    FileInfo[] f2 = null;
                    f2 = dir2.GetFiles("*.xml");
                    int iAwitingUnprocessed = f2.Length;
                    if (f2.Length > 0)
                        iBalanceDocs2 = f2.Length;
                }
                if (dir3.Exists)
                {
                    string strDirectoryName = strDirName.Trim();
                    FileInfo[] f3 = null;
                    f3 = dir3.GetFiles("*.xml");
                    int iAwitingUnprocessed = f3.Length;
                    if (f3.Length > 0)
                        iBalanceDocs3 = f3.Length;
                }
                if (dir4.Exists)
                {
                    string strDirectoryName = strDirName.Trim();
                    FileInfo[] f4 = null;
                    f4 = dir4.GetFiles("*.xml");
                    int iAwitingUnprocessed = f4.Length;
                    if (f4.Length > 0)
                        iBalanceDocs4 = f4.Length;
                }
            }
            #endregion
            return (iBalanceDocs1 + iBalanceDocs2 + iBalanceDocs3 + iBalanceDocs4 + iBalanceDocs5);
        }
        #endregion

        #region: New Development on 17th January 2014
        private void PopulateBuyerCompany()
        {
            DataTable dtBuyerCompany = null;
            dtBuyerCompany = GetBuyerCompanyListDropDown(Convert.ToInt32(Session["CompanyID"]));

            if (dtBuyerCompany != null)
            {
                ddlBuyerCompany.DataSource = dtBuyerCompany;
                ddlBuyerCompany.DataBind();

            }
            ddlBuyerCompany.Items.Insert(0, new ListItem("Select Company", "0"));
        }


        protected void ddlBuyerCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsDropDownPostBack = 1;
            Pager1.CurrentIndex = 1;
            BuildSql(1);

        }
        public DataTable GetBuyerCompanyListDropDown(int iCompanyID)
        {

            // The Stored Procedure also used in Download DB Class So be Carefull to change any thing in this procedure.
            DataSet ds = new DataSet();
            try
            {
                SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetBuyerCompanyForDownloadDB", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.Add("@CompanyID", iCompanyID);
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
                return (ds.Tables[0]);
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
                return new DataTable();

            }
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            IsDropDownPostBack = 1;
            Pager1.CurrentIndex = 1;
            BuildSql(1);
        }

    }
}
