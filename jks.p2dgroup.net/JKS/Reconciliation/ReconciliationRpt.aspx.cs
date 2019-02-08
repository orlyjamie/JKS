using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
    /// Summary description for ReconciliationRpt.
    /// </summary>
    public class ReconciliationRpt : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Label lbl;
        protected System.Web.UI.WebControls.Label Label1;
        protected System.Web.UI.WebControls.Label Label2;
        protected System.Web.UI.WebControls.Label lblIPErejections;
        protected System.Web.UI.WebControls.Label lblBalanceofrejections;
        protected System.Web.UI.WebControls.Label lblChangefromprevious;
        protected System.Web.UI.WebControls.Label lblTotal;
        protected System.Web.UI.WebControls.Label lblScannedDocsIn_st;
        protected System.Web.UI.WebControls.Label lblScannedDocsIn_ex;
        protected System.Web.UI.WebControls.Label lblScannedDocsIn_tot;
        protected System.Web.UI.WebControls.Label lblEdocsIn_st;
        protected System.Web.UI.WebControls.Label lblEdocsIn_ex;
        protected System.Web.UI.WebControls.Label lblEdocsIn_tot;
        protected System.Web.UI.WebControls.Label lblInvUpload_st;
        protected System.Web.UI.WebControls.Label lblInvUpload_ex;
        protected System.Web.UI.WebControls.Label InvUpload_tot;
        protected System.Web.UI.WebControls.Label lblDebitNotes_st;
        protected System.Web.UI.WebControls.Label lblDebitNotes_ex;
        protected System.Web.UI.WebControls.Label lblDebitNotes_tot;
        protected System.Web.UI.WebControls.Label lblTotal1;
        protected System.Web.UI.WebControls.Label lblTotal2;
        protected System.Web.UI.WebControls.Label lblTotal3;
        protected System.Web.UI.WebControls.Label lblReceived_st;
        protected System.Web.UI.WebControls.Label lblReceived_ex;
        protected System.Web.UI.WebControls.Label lblReceived_tot;
        protected System.Web.UI.WebControls.Label lblApproved_st;
        protected System.Web.UI.WebControls.Label lblApproved_ex;
        protected System.Web.UI.WebControls.Label lblApproved_tot;
        protected System.Web.UI.WebControls.Label lblDeleted_st;
        protected System.Web.UI.WebControls.Label lblDeleted_ex;
        protected System.Web.UI.WebControls.Label lblDeleted_tot;
        protected System.Web.UI.WebControls.Label lblRejected_st;
        protected System.Web.UI.WebControls.Label lblRejected_ex;
        protected System.Web.UI.WebControls.Label lblRejected_tot;
        protected System.Web.UI.WebControls.Label lblTotal4;
        protected System.Web.UI.WebControls.Label lblTotal5;
        protected System.Web.UI.WebControls.Label lblTotal6;
        protected System.Web.UI.WebControls.Label lblTotal7;
        protected System.Web.UI.WebControls.Label lblTotal8;
        protected System.Web.UI.WebControls.Label lblTotal9;
        protected System.Web.UI.WebControls.Label lblRegistration_st;
        protected System.Web.UI.WebControls.Label lblRegistration_ex;
        protected System.Web.UI.WebControls.Label lblRegistration_tot;
        protected System.Web.UI.WebControls.Label lblTotal10;
        protected System.Web.UI.WebControls.Label lblTotal11;
        protected System.Web.UI.WebControls.Label lblTotal12;
        protected System.Web.UI.WebControls.Label lblDocsApprovedRejected_st;
        protected System.Web.UI.WebControls.Label lblDocsApprovedRejected_ex;
        protected System.Web.UI.WebControls.Label lblTotal13;
        protected System.Web.UI.WebControls.Label lblAuthorization_st;
        protected System.Web.UI.WebControls.Label lblAuthorization_ex;
        protected System.Web.UI.WebControls.Label lblTotal14;
        protected System.Web.UI.WebControls.Label lblTotal15;
        protected System.Web.UI.WebControls.Label lblTotal16;
        protected System.Web.UI.WebControls.Label lblTotal17;
        protected System.Web.UI.WebControls.DropDownList ddlday;
        protected System.Web.UI.WebControls.DropDownList ddlMonth;
        protected System.Web.UI.WebControls.DropDownList ddlYear;
        protected System.Web.UI.WebControls.Label lblMsg;
        protected System.Web.UI.WebControls.Button btnGetReconciliation;
        #endregion

        string strDate = "";

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            if (!IsPostBack)
            {
                LoadDate();
            }

        }
        private void BindTopData(string sDate)
        {
            int z_data = 0;

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlConn.Open();

            SqlCommand sqlCmd = new SqlCommand("usp_GetReconciliationReport", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@ReportDate", DateTime.Parse(sDate));

            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
            mySqlDataAdapter.SelectCommand = sqlCmd;
            DataSet oDataSet = new DataSet();
            try
            {
                mySqlDataAdapter.Fill(oDataSet);
            }
            catch (Exception ex) { string ss = ex.Message.ToString(); }
            finally
            {
                oDataSet.Dispose();
                sqlConn.Dispose();
            }

            for (int i = 0; i <= oDataSet.Tables[0].Rows.Count - 1; i++)
            {
                if (oDataSet.Tables[0].Rows[i]["docType"] != null)
                {
                    string d = oDataSet.Tables[0].Rows[i]["docType"].ToString();
                    if (oDataSet.Tables[0].Rows[i]["docType"].ToString().ToUpper().Trim() == "S")
                    {
                        if (oDataSet.Tables[0].Rows[i]["a_IPE_XML"] != null)
                        {
                            lblScannedDocsIn_st.Text = oDataSet.Tables[0].Rows[i]["a_IPE_XML"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["b_E_Docs"] != null)
                        {
                            lblEdocsIn_st.Text = oDataSet.Tables[0].Rows[i]["b_E_Docs"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["c_InvoiceUpload"] != null)
                        {
                            lblInvUpload_st.Text = oDataSet.Tables[0].Rows[i]["c_InvoiceUpload"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["d_DebitNoteUpload"] != null)
                        {
                            lblDebitNotes_st.Text = oDataSet.Tables[0].Rows[i]["d_DebitNoteUpload"].ToString();
                        }
                        //**********
                        if (oDataSet.Tables[0].Rows[i]["e_Recieved"] != null)
                        {
                            lblReceived_st.Text = oDataSet.Tables[0].Rows[i]["e_Recieved"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["f_Approved"] != null)
                        {
                            lblApproved_st.Text = oDataSet.Tables[0].Rows[i]["f_Approved"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["g_Deleted"] != null)
                        {
                            lblDeleted_st.Text = oDataSet.Tables[0].Rows[i]["g_Deleted"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["h_Rejected"] != null)
                        {
                            lblRejected_st.Text = oDataSet.Tables[0].Rows[i]["h_Rejected"].ToString();
                        }
                        //*****************
                        if (oDataSet.Tables[0].Rows[i]["i_RegDocs"] != null)
                        {
                            lblRegistration_st.Text = oDataSet.Tables[0].Rows[i]["i_RegDocs"].ToString();
                        }
                        //*****************
                        if (oDataSet.Tables[0].Rows[i]["j_ApvRejWorkFlow"] != null)
                        {
                            lblDocsApprovedRejected_st.Text = oDataSet.Tables[0].Rows[i]["j_ApvRejWorkFlow"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["k_AuthDoc"] != null)
                        {
                            lblAuthorization_st.Text = oDataSet.Tables[0].Rows[i]["k_AuthDoc"].ToString();
                        }

                    }
                    else if (oDataSet.Tables[0].Rows[i]["docType"].ToString().ToUpper().Trim() == "E")
                    {
                        if (oDataSet.Tables[0].Rows[i]["a_IPE_XML"] != null)
                        {
                            lblScannedDocsIn_ex.Text = oDataSet.Tables[0].Rows[i]["a_IPE_XML"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["b_E_Docs"] != null)
                        {
                            lblEdocsIn_ex.Text = oDataSet.Tables[0].Rows[i]["b_E_Docs"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["c_InvoiceUpload"] != null)
                        {
                            lblInvUpload_ex.Text = oDataSet.Tables[0].Rows[i]["c_InvoiceUpload"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["d_DebitNoteUpload"] != null)
                        {
                            lblDebitNotes_ex.Text = oDataSet.Tables[0].Rows[i]["d_DebitNoteUpload"].ToString();
                        }
                        //*****************
                        if (oDataSet.Tables[0].Rows[i]["e_Recieved"] != null)
                        {
                            lblReceived_ex.Text = oDataSet.Tables[0].Rows[i]["e_Recieved"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["f_Approved"] != null)
                        {
                            lblApproved_ex.Text = oDataSet.Tables[0].Rows[i]["f_Approved"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["g_Deleted"] != null)
                        {
                            lblDeleted_ex.Text = oDataSet.Tables[0].Rows[i]["g_Deleted"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["h_Rejected"] != null)
                        {
                            lblRejected_ex.Text = oDataSet.Tables[0].Rows[i]["h_Rejected"].ToString();
                        }
                        //*****************
                        if (oDataSet.Tables[0].Rows[i]["i_RegDocs"] != null)
                        {
                            lblRegistration_ex.Text = oDataSet.Tables[0].Rows[i]["i_RegDocs"].ToString();
                        }
                        //*****************
                        if (oDataSet.Tables[0].Rows[i]["j_ApvRejWorkFlow"] != null)
                        {
                            lblDocsApprovedRejected_ex.Text = oDataSet.Tables[0].Rows[i]["j_ApvRejWorkFlow"].ToString();
                        }
                        if (oDataSet.Tables[0].Rows[i]["k_AuthDoc"] != null)
                        {
                            lblAuthorization_ex.Text = oDataSet.Tables[0].Rows[i]["k_AuthDoc"].ToString();
                        }
                    }
                }
            }

            lblScannedDocsIn_tot.Text = Convert.ToString(Convert.ToInt32(lblScannedDocsIn_st.Text) + Convert.ToInt32(lblScannedDocsIn_ex.Text));
            lblEdocsIn_tot.Text = Convert.ToString(Convert.ToInt32(lblEdocsIn_st.Text) + Convert.ToInt32(lblEdocsIn_ex.Text));
            InvUpload_tot.Text = Convert.ToString(Convert.ToInt32(lblInvUpload_st.Text) + Convert.ToInt32(lblInvUpload_ex.Text));
            lblDebitNotes_tot.Text = Convert.ToString(Convert.ToInt32(lblDebitNotes_st.Text) + Convert.ToInt32(lblDebitNotes_ex.Text));
            lblTotal1.Text = Convert.ToString(Convert.ToInt32(lblScannedDocsIn_st.Text) + Convert.ToInt32(lblEdocsIn_st.Text) + Convert.ToInt32(lblInvUpload_st.Text) + Convert.ToInt32(lblDebitNotes_st.Text));
            lblTotal2.Text = Convert.ToString(Convert.ToInt32(lblScannedDocsIn_ex.Text) + Convert.ToInt32(lblEdocsIn_ex.Text) + Convert.ToInt32(lblInvUpload_ex.Text) + Convert.ToInt32(lblDebitNotes_ex.Text));
            lblTotal3.Text = Convert.ToString(Convert.ToInt32(lblScannedDocsIn_tot.Text) + Convert.ToInt32(lblEdocsIn_tot.Text) + Convert.ToInt32(InvUpload_tot.Text) + Convert.ToInt32(lblDebitNotes_tot.Text)); ;
            //*************
            lblReceived_tot.Text = Convert.ToString(Convert.ToInt32(lblReceived_st.Text) + Convert.ToInt32(lblReceived_ex.Text));
            lblApproved_tot.Text = Convert.ToString(Convert.ToInt32(lblApproved_st.Text) + Convert.ToInt32(lblApproved_ex.Text));
            lblDeleted_tot.Text = Convert.ToString(Convert.ToInt32(lblDeleted_st.Text) + Convert.ToInt32(lblDeleted_ex.Text));
            lblRejected_tot.Text = Convert.ToString(Convert.ToInt32(lblRejected_st.Text) + Convert.ToInt32(lblRejected_ex.Text));
            lblTotal4.Text = Convert.ToString(Convert.ToInt32(lblReceived_st.Text) + Convert.ToInt32(lblApproved_st.Text) + Convert.ToInt32(lblDeleted_st.Text));
            lblTotal5.Text = Convert.ToString(Convert.ToInt32(lblReceived_ex.Text) + Convert.ToInt32(lblApproved_ex.Text) + Convert.ToInt32(lblDeleted_ex.Text) + Convert.ToInt32(lblRejected_ex.Text));

            lblTotal7.Text = Convert.ToString(Convert.ToInt32(lblTotal1.Text) - Convert.ToInt32(lblTotal4.Text));
            lblTotal8.Text = Convert.ToString(Convert.ToInt32(lblTotal2.Text) - Convert.ToInt32(lblTotal5.Text));
            lblTotal9.Text = Convert.ToString(Convert.ToInt32(lblTotal3.Text) - (Convert.ToInt32(lblReceived_tot.Text) + Convert.ToInt32(lblApproved_tot.Text) + Convert.ToInt32(lblDeleted_tot.Text) + Convert.ToInt32(lblRejected_tot.Text)));

            lblTotal10.Text = Convert.ToString((Convert.ToInt32(lblRegistration_st.Text) - Convert.ToInt32(lblScannedDocsIn_st.Text)) - (Convert.ToInt32(lblEdocsIn_st.Text) + Convert.ToInt32(lblReceived_st.Text) + Convert.ToInt32(lblDeleted_st.Text)));
            lblTotal11.Text = Convert.ToString(Convert.ToInt32(lblRegistration_ex.Text) - Convert.ToInt32(lblReceived_ex.Text));

            lblTotal16.Text = Convert.ToString(Convert.ToInt32(lblDocsApprovedRejected_ex.Text) - Convert.ToInt32(lblAuthorization_ex.Text));

            for (int i = 0; i <= oDataSet.Tables[1].Rows.Count - 1; i++)
            {
                lblIPErejections.Text = Convert.ToString(oDataSet.Tables[1].Rows[i]["w_Unprocessed"]);
                lblBalanceofrejections.Text = Convert.ToString(oDataSet.Tables[1].Rows[i]["x_bal_unprocessed"]);
                lblChangefromprevious.Text = Convert.ToString(oDataSet.Tables[1].Rows[i]["Y_ChangeOfBal_X"]);

            }
            lblTotal.Text = Convert.ToString((Convert.ToInt32(lblScannedDocsIn_st.Text) + Convert.ToInt32(lblScannedDocsIn_ex.Text)) - Convert.ToInt32(lblChangefromprevious.Text) - Convert.ToInt32(z_data));

            oDataSet.Dispose();
            sqlConn.Dispose();
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
            this.btnGetReconciliation.Click += new System.EventHandler(this.btnGetReconciliation_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btnGetReconciliation_Click(object sender, System.EventArgs e)
        {
            strDate = "";
            CheckDate(out strDate);
            if (strDate.Trim() != "")
                BindTopData(strDate);

        }
        #region LoadDate
        private void LoadDate()
        {
            ddlYear.Items.Insert(0, new ListItem("Year", "0"));
            ddlMonth.Items.Insert(0, new ListItem("Month", "0"));
            ddlday.Items.Insert(0, new ListItem("Day", "0"));

            int currentYear = Microsoft.VisualBasic.DateAndTime.Year(System.DateTime.Now);
            for (int i = (currentYear - 1); i <= currentYear; i++)
                ddlYear.Items.Add(i.ToString());
            for (int i = 1; i < 13; i++)
            {
                ddlMonth.Items.Add(new ListItem(Microsoft.VisualBasic.DateAndTime.MonthName(i, true), (i.ToString())));
            }
            for (int i = 1; i < 32; i++)
            {
                ddlday.Items.Add(i.ToString());
            }

        }
        #endregion

        #region CheckDate
        private void CheckDate(out string strDate)
        {
            strDate = "";
            if (Convert.ToInt32(ddlYear.SelectedValue.Trim()) == 0 || Convert.ToInt32(ddlMonth.SelectedValue.Trim()) == 0 ||
                Convert.ToInt32(ddlday.SelectedValue.Trim()) == 0)
            {
                lblMsg.Text = "Please select date to view report.";
                lblMsg.Visible = true;
                return;
            }
            else
            {
                if (Convert.ToInt32(ddlMonth.SelectedValue.Trim()) == Microsoft.VisualBasic.DateAndTime.Month(System.DateTime.Now))
                {
                    if (Convert.ToInt32(ddlday.SelectedValue.Trim()) <= Microsoft.VisualBasic.DateAndTime.Day(System.DateTime.Now))
                    {
                        lblMsg.Text = "";
                        lblMsg.Visible = false;
                        strDate = ddlMonth.SelectedValue.Trim() + "/" + ddlday.SelectedValue.Trim() + "/" + ddlYear.SelectedValue.Trim();
                    }
                    else
                    {
                        lblMsg.Text = "Please select a date before today.";
                        lblMsg.Visible = true;
                    }
                }
                else if (Convert.ToInt32(ddlMonth.SelectedValue.Trim()) < Microsoft.VisualBasic.DateAndTime.Month(System.DateTime.Now))
                {

                    lblMsg.Text = "";
                    lblMsg.Visible = false;
                    strDate = ddlMonth.SelectedValue.Trim() + "/" + ddlday.SelectedValue.Trim() + "/" + ddlYear.SelectedValue.Trim();

                }
                else
                {
                    lblMsg.Text = "Please select a date before today.";
                    lblMsg.Visible = true;
                }

            }
        }
        #endregion
    }
}
