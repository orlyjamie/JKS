using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;
using System.Data.SqlClient;
using JKS;
using System.Text;
using System.IO;
public partial class JKSDefault : System.Web.UI.Page
{

    #region User Defined Variables
    private int iParentCompanyID = 0;
    private JKS.Users objUser = new JKS.Users();
    protected System.Web.UI.WebControls.Label Label1;
    private int iSession = 0;
    EncryptJKS objEncrypt = new EncryptJKS();
    int cnt = 0;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        Session.Clear();
        //clean up temporary files
        string[] files = System.IO.Directory.GetFiles(Server.MapPath("JKS/Files/"), "WF-" + Session.SessionID + "*.*");
        for (int i = 0; i < files.Length; i++)
            System.IO.File.Delete(files[i]);

        #region Session["XMLInvoiceHeadFile"] & Session["XMLInvoiceHeadFile_CN"]
        Session["XMLInvoiceHeadFile"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "-1.xml";
        Session["XSDInvoiceHeadFile"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "-1.xsd";
        Session["XMLInvoiceDetailFile"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "-2.xml";
        Session["XSDInvoiceDetailFile"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "-2.xsd";
        Session["InvoicePDF"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "Invoice.pdf";
        Session["InvoiceTxt"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "Invoice.txt";
        //setup file names and store in Session
        Session["XMLInvoiceHeadFile_CN"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "_CN-1.xml";
        Session["XSDInvoiceHeadFile_CN"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "_CN-1.xsd";
        Session["XMLInvoiceDetailFile_CN"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "_CN-2.xml";
        Session["XSDInvoiceDetailFile_CN"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "_CN-2.xsd";
        Session["InvoicePDF_CN"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "_CN_Invoice.pdf";
        Session["InvoiceTxt_CN"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "_CN_Invoice.txt";
        //setup file names and store in Session
        Session["DebitNotePDF_NL"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "_DebitNoteNL.pdf";
        Session["DebitNoteTxt_NL"] = Server.MapPath("JKS/Files/") + "WF-" + Session.SessionID + "_DebitNoteNL.txt";
        #endregion
        if (!IsPostBack)
        {
            iSession = 0;
            Session.Abandon();
        }
        if (IsPostBack)
        {
            //int iLogStatusCount = 0;
            //txtNetworkID.Text = Convert.ToString("8088402a-36");//GRH-131cb96a-90

            //// Session["networkID"] = txtNetworkID.Text;// blocked By Rimi on 8th August 2015
            //Session["networkID"] = Convert.ToString("8088402a-36");
            //if (txtNetworkID.Text.Length == 0 || txtUserName.Text.Length == 0 || txtPassword.Text.Length == 0)
            //{
            //    lblValidateMessage.Visible = true;
            //}
            //else
            //{
            //    CBSAppUtils.PrimaryConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            //    DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
            //    //Modified by kuntalkarar on 28thMay2016 for Rijndael + LOCKOUT system as COOP/WELL
            //    RecordSet rsLogin = da.ExecuteSP("up_security_Login_Encrpyt", txtNetworkID.Text, txtUserName.Text, txtPassword.Text,objEncrypt.RijndaelEncription(txtPassword.Text));// Commenetd By Rimi on 22nd July 2015


            //    //blocked by kuntalkarar on 28thMay2016 for Rijndael
            //    //RecordSet rsLogin = da.ExecuteSP("userLogInGRH", txtNetworkID.Text, txtUserName.Text, txtPassword.Text, EncryptJKS.EncryptData(txtPassword.Text));// Added By Rimi on 22nd July 2015
            //    //Added by kuntalkarar on 28thMay2016 for Rijndael
            //    //RecordSet rsLogin = da.ExecuteSP("userLogInGRH", txtNetworkID.Text, txtUserName.Text, txtPassword.Text, objEncrypt.RijndaelEncription(txtPassword.Text));// Added By Rimi on 22nd July 2015

            //    /* we will have two resultsets - the first one containing the LoginStatus
            //       and the other one containing the user details in case of successful login	
            //    */


            //    if (rsLogin.ParentDataSet != null)//Added By Rimi on 24thJuly2015
            //    {
            //        rsLogin.ParentTable.TableName = "Users";
            //        if (rsLogin.ParentDataSet.Tables.Count > 1)
            //        {
            //            RecordSet rsUser = new RecordSet(rsLogin.ParentDataSet, 1);
            //            for (int i = 0, j = rsUser.ColumnCount; i < j; i++)
            //            {
            //                string columnName = rsUser.Columns[i].ColumnName;
            //                Session.Add(columnName, rsUser[columnName]);
            //            }
            //            //get the user's security access information and load that into session
            //            RecordSet rsAccess = new RecordSet(rsLogin.ParentDataSet, 2);
            //            Session.Add("Access", rsAccess);
            //            // CBSAppUtils.AppUserId = (int)Session["UserID"];// Commented By Rimi on 24thJuly2015
            //            //========================Added By Rimi on 24thJuly2015================================= 
            //            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            //            {
            //                CBSAppUtils.AppUserId = (int)Session["UserID"];
            //            }
            //            else
            //            {
            //                CBSAppUtils.AppUserId = 0;
            //            }
            //            //========================Added By Rimi on 24thJuly2015=================================
            //            Session["JKS"] = 0;
            //            RecordSet rsComp = da.ExecuteQuery("vUserCompany", "UserID= " + CBSAppUtils.AppUserId);
            //            if (rsComp.RecordCount > 0)
            //            {
            //                if (rsComp["ParentCompanyID"] == DBNull.Value)
            //                    iParentCompanyID = 0;
            //                else
            //                    iParentCompanyID = Convert.ToInt32(rsComp["ParentCompanyID"]);

            //                // iParentCompanyID = 116065;

            //                if (rsComp["CompanyName"].ToString().ToUpper().Trim() == "JKS" || iParentCompanyID == 116065)
            //                {
            //                    Session["CompanyID"] = Convert.ToInt32(rsComp["CompanyID"]);
            //                    //  Session["CompanyID"] = 116065;
            //                    Session["JKS"] = 1;
            //                }
            //                if (rsComp["ParentCompanyID"] == DBNull.Value)
            //                    Session["ParentCompanyID"] = 0;
            //                else
            //                    Session["ParentCompanyID"] = Convert.ToInt32(rsComp["ParentCompanyID"]);

            //                if (rsComp["UserTypeID"] == DBNull.Value)
            //                    Session["UserTypeID"] = 1;
            //                else
            //                    Session["UserTypeID"] = Convert.ToInt32(rsComp["UserTypeID"]);





            //                if (iSession == 0)
            //                {

            //                    // Added By Mrinal on 30th December 2013
            //                    if (Session["UserTypeID"] != null)
            //                    {
            //                        int utid = Convert.ToInt32(Session["UserTypeID"]);
            //                        if (utid == 3)
            //                        {
            //                            Session.Timeout = 300;
            //                            iSession = 1;
            //                        }
            //                        else
            //                        {
            //                            Session.Timeout = 300;

            //                        }
            //                    }
            //                }
            //                if (rsComp["CompanyTypeID"] != DBNull.Value)
            //                {
            //                    Session["CompanyID"] = Convert.ToInt32(rsComp["CompanyID"]);
            //                    //  Session["CompanyID"] = 116065;
            //                    Session["CompanyTypeID"] = Convert.ToInt32(rsComp["CompanyTypeID"]);
            //                }
            //                else
            //                    Session["CompanyTypeID"] = 0;

            //                if (rsComp["New_UserGroup"] != DBNull.Value)
            //                    Session["UserGroupCode"] = rsComp["New_UserGroup"];
            //            }
            //            da.CloseConnection();
            //            if (Convert.ToInt32(Session["UserID"]) != 0)//========================Added By Rimi on 24thJuly2015=================================
            //            {
            //                if (objUser.CheckFirstLogin(Convert.ToInt32(Session["UserID"])))
            //                {
            //                    if (Convert.ToInt32(Session["UserTypeID"]) == 11)
            //                        Response.Redirect(ConfigurationManager.AppSettings["CMS_JKS"].Trim());
            //                    else
            //                        Response.Redirect(ConfigurationManager.AppSettings["UserMainPage_JKS"].Trim());
            //                }
            //                else
            //                    Response.Redirect(ConfigurationManager.AppSettings["FirstLoginPage_JKS"].Trim());
            //            }
            //            //========================Added By Rimi on 24thJuly2015=================================
            //            else
            //            {
            //                lblValidateMessage.Visible = true;
            //            }
            //            //========================Added By Rimi on 24thJuly2015=================================

            //        }
            //        else
            //        {
            //            //login failed! to inspect what went wrong, we need 
            //            //to extract information from the first table inside
            //            //the rsLogin, but we don't need to do that now.
            //            //blocked by kuntalkarar on 30thMay2016
            //            //lblValidateMessage.Visible = true;

            //            //addedby kuntalkarar on 30thMay2016
            //            DataSet ds = new DataSet();
            //            DataTable dtnew = new DataTable();
            //            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            //            SqlDataAdapter sqlDA = new SqlDataAdapter("CheckFailedLogIn", sqlConn);
            //            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            //            sqlDA.SelectCommand.Parameters.Add("@username", Convert.ToString(txtUserName.Text));
            //            sqlDA.SelectCommand.Parameters.Add("@networkId", Convert.ToString(txtNetworkID.Text));
            //            sqlConn.Open();
            //            sqlDA.Fill(dtnew);
            //            iLogStatusCount = Convert.ToInt32(dtnew.Rows[0]["lockout"].ToString());
            //            //if (ds.Tables[0].Rows.Count > 0)
            //            //{

            //            //}




            //            //string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            //            //SqlConnection sqlConn = new SqlConnection(ConsString);
            //            //sqlConn.Open();
            //            //SqlCommand sqlCmd = new SqlCommand("SELECT isnull(lockout,0)  FROM Users INNER JOIN Company  ON Company.CompanyID = Users.CompanyID WHERE Users.UserName='" + txtUserName.Text + "'  AND Company.NetworkID ='" + txtNetworkID.Text + "'  AND Users.UserDeleted = 0", sqlConn);
            //            //iLogStatusCount = Convert.ToInt32(sqlCmd.ExecuteScalar());
            //            sqlConn.Close();
            //            if (iLogStatusCount >= 3)
            //            {

            //                //string sScript = "<SCRIPT language='javascript'>alert('Your account has been locked out following 3 successive failed log-ins. Please contact your business nominated IS Helpdesk and request a password reset'); </SCRIPT>";
            //                //Page.RegisterStartupScript("Focus",sScript);	
            //                Page.RegisterStartupScript("Reg", "<script>LoginFailureMessage();</script>");
            //            }
            //            else
            //            {
            //                lblValidateMessage.Visible = true;
            //            }


            //        }
            //        //========================Added By Rimi on 24thJuly2015================================= 
            //    }
            //    else
            //    {
            //        lblValidateMessage.Visible = true;
            //    }
            //    //========================Added By Rimi on 24thJuly2015================================= 
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
        this.btnlogin.Click += new System.EventHandler(this.btnlogin_Click);
        this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion
    protected void btnlogin_Click(object sender, EventArgs e)
    {

        if (cnt == 0)
        {
            if (IsPostBack)
            {
                int iLogStatusCount = 0;
                txtNetworkID.Text = Convert.ToString("9ae44765-e9");//Changed 9ae44765-e9 of JKS from 0e8e82fa-14 of BBR

                // Session["networkID"] = txtNetworkID.Text;// blocked By Rimi on 8th August 2015
                Session["networkID"] = Convert.ToString("9ae44765-e9");//Changed 9ae44765-e9 of JKS from 0e8e82fa-14 of BBR
                if (txtNetworkID.Text.Length == 0 || txtUserName.Text.Length == 0 || txtPassword.Text.Length == 0)
                {
                    lblValidateMessage.Visible = true;
                }
                else
                {
                    CBSAppUtils.PrimaryConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                    DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                    //Modified by kuntalkarar on 28thMay2016 for Rijndael + LOCKOUT system as COOP/WELL
                    RecordSet rsLogin = da.ExecuteSP("up_security_Login_Encrpyt_JKS", txtNetworkID.Text, txtUserName.Text, txtPassword.Text, objEncrypt.RijndaelEncription(txtPassword.Text));// Commenetd By Rimi on 22nd July 2015


                    //blocked by kuntalkarar on 28thMay2016 for Rijndael
                    //RecordSet rsLogin = da.ExecuteSP("userLogInGRH", txtNetworkID.Text, txtUserName.Text, txtPassword.Text, EncryptJKS.EncryptData(txtPassword.Text));// Added By Rimi on 22nd July 2015
                    //Added by kuntalkarar on 28thMay2016 for Rijndael
                    //RecordSet rsLogin = da.ExecuteSP("userLogInGRH", txtNetworkID.Text, txtUserName.Text, txtPassword.Text, objEncrypt.RijndaelEncription(txtPassword.Text));// Added By Rimi on 22nd July 2015

                    /* we will have two resultsets - the first one containing the LoginStatus
                       and the other one containing the user details in case of successful login	
                    */


                    if (rsLogin.ParentDataSet != null)//Added By Rimi on 24thJuly2015
                    {
                        rsLogin.ParentTable.TableName = "Users";
                        if (rsLogin.ParentDataSet.Tables.Count > 1)
                        {
                            RecordSet rsUser = new RecordSet(rsLogin.ParentDataSet, 1);
                            for (int i = 0, j = rsUser.ColumnCount; i < j; i++)
                            {
                                string columnName = rsUser.Columns[i].ColumnName;
                                Session.Add(columnName, rsUser[columnName]);
                            }
                            //get the user's security access information and load that into session
                            RecordSet rsAccess = new RecordSet(rsLogin.ParentDataSet, 2);
                            Session.Add("Access", rsAccess);
                            // CBSAppUtils.AppUserId = (int)Session["UserID"];// Commented By Rimi on 24thJuly2015
                            //========================Added By Rimi on 24thJuly2015================================= 
                            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
                            {
                                CBSAppUtils.AppUserId = (int)Session["UserID"];
                            }
                            else
                            {
                                CBSAppUtils.AppUserId = 0;
                            }
                            //========================Added By Rimi on 24thJuly2015=================================
                            Session["JKS"] = 0;
                            RecordSet rsComp = da.ExecuteQuery("vUserCompany", "UserID= " + CBSAppUtils.AppUserId);
                            if (rsComp.RecordCount > 0)
                            {
                                if (rsComp["ParentCompanyID"] == DBNull.Value)
                                    iParentCompanyID = 0;
                                else
                                    iParentCompanyID = Convert.ToInt32(rsComp["ParentCompanyID"]);

                                // iParentCompanyID = 116065;

                                if (rsComp["CompanyName"].ToString().ToLower().Trim() == "JKS" || iParentCompanyID == 116065)
                                {
                                    Session["CompanyID"] = Convert.ToInt32(rsComp["CompanyID"]);
                                    //  Session["CompanyID"] = 116065;
                                    Session["JKS"] = 1;
                                }
                                if (rsComp["ParentCompanyID"] == DBNull.Value)
                                    Session["ParentCompanyID"] = 0;
                                else
                                    Session["ParentCompanyID"] = Convert.ToInt32(rsComp["ParentCompanyID"]);

                                if (rsComp["UserTypeID"] == DBNull.Value)
                                    Session["UserTypeID"] = 1;
                                else
                                    Session["UserTypeID"] = Convert.ToInt32(rsComp["UserTypeID"]);





                                if (iSession == 0)
                                {

                                    // Added By Mrinal on 30th December 2013
                                    if (Session["UserTypeID"] != null)
                                    {
                                        int utid = Convert.ToInt32(Session["UserTypeID"]);
                                        if (utid == 3)
                                        {
                                            Session.Timeout = 300;
                                            iSession = 1;
                                        }
                                        else
                                        {
                                            Session.Timeout = 300;

                                        }
                                    }
                                }
                                if (rsComp["CompanyTypeID"] != DBNull.Value)
                                {
                                    Session["CompanyID"] = Convert.ToInt32(rsComp["CompanyID"]);
                                    //  Session["CompanyID"] = 116065;
                                    Session["CompanyTypeID"] = Convert.ToInt32(rsComp["CompanyTypeID"]);
                                }
                                else
                                    Session["CompanyTypeID"] = 0;

                                if (rsComp["New_UserGroup"] != DBNull.Value)
                                    Session["UserGroupCode"] = rsComp["New_UserGroup"];
                            }
                            da.CloseConnection();
                            if (Convert.ToInt32(Session["UserID"]) != 0)//========================Added By Rimi on 24thJuly2015=================================
                            {
                                if (objUser.CheckFirstLogin(Convert.ToInt32(Session["UserID"])))
                                {
                                    if (Convert.ToInt32(Session["UserTypeID"]) == 11)
                                        Response.Redirect(ConfigurationManager.AppSettings["CMS_JKS"].Trim());
                                    else
                                        Response.Redirect(ConfigurationManager.AppSettings["UserMainPage_JKS"].Trim());
                                }
                                else
                                    Response.Redirect(ConfigurationManager.AppSettings["FirstLoginPage_JKS"].Trim());
                            }
                            //========================Added By Rimi on 24thJuly2015=================================
                            else
                            {
                                lblValidateMessage.Visible = true;
                            }
                            //========================Added By Rimi on 24thJuly2015=================================

                        }
                        else
                        {
                            //login failed! to inspect what went wrong, we need 
                            //to extract information from the first table inside
                            //the rsLogin, but we don't need to do that now.

                            //blocked by kuntalkarar on 30thMay2016
                            //lblValidateMessage.Visible = true;

                            //addedby kuntalkarar on 30thMay2016 for lockout msg after 6 failed log in
                            DataSet ds = new DataSet();
                            DataTable dtnew = new DataTable();
                            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                            SqlDataAdapter sqlDA = new SqlDataAdapter("CheckFailedLogIn", sqlConn);
                            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                            sqlDA.SelectCommand.Parameters.Add("@username", Convert.ToString(txtUserName.Text));
                            sqlDA.SelectCommand.Parameters.Add("@networkId", Convert.ToString(txtNetworkID.Text));
                            sqlConn.Open();
                            sqlDA.Fill(dtnew);
                            if (dtnew.Rows.Count > 0)
                            {
                                iLogStatusCount = Convert.ToInt32(dtnew.Rows[0]["lockout"].ToString());
                                sqlConn.Close();
                                if (iLogStatusCount >= 6)
                                {

                                    //string sScript = "<SCRIPT language='javascript'>alert('Your account has been locked out following 3 successive failed log-ins. Please contact your business nominated IS Helpdesk and request a password reset'); </SCRIPT>";
                                    //Page.RegisterStartupScript("Focus",sScript);	
                                    Page.RegisterStartupScript("Reg", "<script>LoginFailureMessage();</script>");
                                }
                                else
                                {
                                    lblValidateMessage.Visible = true;
                                }
                            }

                            else
                            {
                                lblValidateMessage.Visible = true;
                            }
                            //addition ends by  kuntalkarar on 2ndJune2016 for lockout msg after 6 failed log in

                        }
                        //========================Added By Rimi on 24thJuly2015================================= 
                    }
                    else
                    {
                        lblValidateMessage.Visible = true;
                    }
                    //========================Added By Rimi on 24thJuly2015================================= 
                }
            }
            cnt += 1;
        }
    }
}