using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using CBSolutions.Architecture.Core;
using CBSolutions.Architecture.Data;

namespace CBSolutions.ETH.Web.Utilities
{
    /// <summary>
    /// Summary description for DownloadDB.
    /// </summary>
    public class DownloadDB : System.Web.UI.Page
    {
        #region Webform Controls
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Label Label1;
        protected System.Web.UI.WebControls.HyperLink HyperLink1;
        protected System.Web.UI.WebControls.HyperLink Hyperlink2;
        protected System.Web.UI.WebControls.HyperLink Hyperlink3;
        protected System.Web.UI.WebControls.DropDownList cboCompany;
        protected System.Web.UI.WebControls.Label outError;
        protected System.Web.UI.HtmlControls.HtmlInputFile downloadexp;
        #endregion
        // ==========================================================================================================
        #region User Defined Variables
        protected string strInvoiceDocumentExportPath = ConfigurationManager.AppSettings["ExportDocPath"];
        protected string exp = "";
        protected int nCompanyId = -1;		//140905 SURAJIT
        #endregion
        // ==========================================================================================================
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../close_win.aspx");

            RecordSet rs;
            if (Convert.ToInt32(Session["exported"]) == 1)
            {
                Session["exported"] = 0;
                Response.Redirect("export.aspx?exp=");
            }
            DirectoryInfo di = new DirectoryInfo("\\export");
            if (!di.Exists)
                di.Create();

            if (Convert.ToInt32(Session["compid"]) != nCompanyId || (Convert.ToInt32(Session["compid"]) == nCompanyId && Convert.ToInt32(Session["exported"]) != 1))
            {
                exp = (string)Request.QueryString["exp"];
                if (nCompanyId == -1 && (Convert.ToInt32(Session["compid"]) != nCompanyId && Convert.ToInt32(Session["exported"]) != 1))
                    nCompanyId = Convert.ToInt32(Session["compid"]);
            }
            else
            {
                exp = "";
                nCompanyId = -1;
            }
            Session["exported"] = 0;
            if ((!Page.IsPostBack && (exp == "" || exp == null || exp == "9")) || nCompanyId == -1)  //ch 150905 SURAJIT
            {
                DataTable dt = GetBuyerCompanyList(Convert.ToInt32(Session["CompanyID"]));  //ch 200905 SURAJIT //para added
                cboCompany.DataSource = dt;
                cboCompany.DataBind();
            }

            try
            {
                if (exp == "1")
                {
                    DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);
                    if (nCompanyId == -1 && Convert.ToInt32(Session["exported"]) == 1)
                        return;

                    if (nCompanyId == -1)
                    {
                        outError.Text = "Please select a company";
                        return;
                    }
                    else
                        outError.Text = "";


                    if (Convert.ToInt32(Session["CompanyTypeID"]) == 0 || nCompanyId == 14 || nCompanyId == 0) //HUB ADMIN or GMG  //140905 SURAJIT 
                    {
                        if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                            if (Convert.ToInt32(Session["CompanyID"]) != 13074)
                                rs = da.ExecuteQuery("Company", "Active=1", "CompanyID");

                            else
                                rs = da.ExecuteQuery("Company", "Active=1 and (CompanyID =" + nCompanyId + " OR ParentCompanyID=" + nCompanyId + ")", "CompanyID");


                        else
                            rs = da.ExecuteQuery("Company", "Active=1 and (CompanyID =" + nCompanyId + " OR ParentCompanyID=" + nCompanyId + ")", "CompanyID");


                        if (rs.RecordCount > 0)
                        {
                            rs.MoveFirst();
                            while (!rs.EOF())
                            {
                                if (ExportDocForReg(Convert.ToString(rs["CompanyCode"]) + "_Reg.CSV", Convert.ToInt32(rs["CompanyID"]))) //141005 SURAJIT
                                {
                                    rs.MoveNext();
                                }
                            }
                        }

                        exp = "9";
                        Session["exported"] = 1;
                        nCompanyId = -1;

                    }
                    else
                    {

                        rs = da.ExecuteQuery("Company", "Active=1 and (CompanyID =" + nCompanyId + ")", "CompanyID");
                        if (rs.RecordCount > 0)
                        {
                            if (ExportDocForReg(Convert.ToString(rs["CompanyCode"]) + "_Reg.CSV", nCompanyId))
                            {
                            }
                        }
                    }

                    exp = "";
                    Session["exported"] = 1;
                    nCompanyId = -1;
                }
                if (exp == "2")//auth
                {
                    DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString);

                    if (nCompanyId == -1)
                    {
                        outError.Text = "Please select a company";
                        return;
                    }
                    else
                        outError.Text = "";

                    if (nCompanyId == -1 && Convert.ToInt32(Session["exported"]) == 1) return;

                    if (Convert.ToInt32(Session["CompanyTypeID"]) == 0 || nCompanyId == 14 || nCompanyId == 0) //HUB ADMIN or GMG  //140905 SURAJIT 
                    {
                        if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
                            if (Convert.ToInt32(Session["CompanyID"]) != 13074)
                                rs = da.ExecuteQuery("Company", "Active=1", "CompanyID");

                            else
                                rs = da.ExecuteQuery("Company", "Active=1 and (CompanyID =" + nCompanyId + " OR ParentCompanyID=" + nCompanyId + ")", "CompanyID");

                        else
                            rs = da.ExecuteQuery("Company", "Active=1 and (CompanyID =" + nCompanyId + " OR ParentCompanyID=" + nCompanyId + ")", "CompanyID");


                        if (rs.RecordCount > 0)
                        {
                            rs.MoveFirst();
                            while (!rs.EOF())
                            {
                                if (ExportDocForAuth(Convert.ToString(rs["CompanyCode"]) + "_Auth.CSV", Convert.ToInt32(rs["CompanyID"]))) //141005 SURAJIT
                                {
                                    rs.MoveNext();
                                }
                            }

                        }

                        exp = "9";
                        Session["exported"] = 1;
                        nCompanyId = -1;

                    }
                    else
                    {
                        rs = da.ExecuteQuery("Company", "Active=1 and (CompanyID =" + nCompanyId + ")", "CompanyID");
                        if (rs.RecordCount > 0)
                        {
                            if (ExportDocForAuth(Convert.ToString(rs["CompanyCode"]) + "_Auth.CSV", nCompanyId))
                            {
                            }
                        }
                    }
                    exp = "";
                    Session["exported"] = 1;
                    nCompanyId = -1;
                }

                else
                {
                    if (Convert.ToInt32(Session["exported"]) == 1)
                    {
                        nCompanyId = -1;
                        exp = "";
                    }
                    else
                    {
                        if (cboCompany.SelectedValue != null)
                        {
                            if (cboCompany.SelectedValue != "")
                            {
                                nCompanyId = Convert.ToInt32(cboCompany.SelectedValue);
                                Session["compid"] = nCompanyId;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        // ==========================================================================================================
        #region ExportDocForReg
        public bool ExportDocForReg(string flnm, Int32 compID)
        {
            bool retval = true;
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString); ;
            SqlCommand sqlCmd = new SqlCommand();
            SqlCommand sqlCmd1 = new SqlCommand();
            SqlDataReader sqlDR = null;

            try
            {
                string csvText = "";
                sqlCmd = new SqlCommand("stp_GetDocToExportReg_New", sqlConn);  // before sp name stp_GetDocToExportReg_New
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@buyerCompanyID", compID);
                sqlConn.Open();
                sqlDR = sqlCmd.ExecuteReader();
                //csvText = "DocumentType" +"," +"GMG Company" +"," +"Vendor" +"," +"DocumentDate" +"," +"PO Number" +"," +"DocumentNumber" + "," +"NetAmount" +"," +"VatAmount" +"," +"TotalAmount" +"," +"Currency" +"," +"Description" +"," +"TimePaymentDue" +"," +"Other";  //141005 SURAJIT
                csvText = "DocumentType,Vendor,DocumentDate,PO Number,DocumentNumber,NetAmount,VatAmount,TotalAmount,Currency,TimePaymentDue,Description,Other,GMG Company,NominalCode,Department,Project,CreditNoteInvoiceNumber"; //Amitava 
                csvText = csvText + "\n";
                while (sqlDR.Read())
                {
                    for (int i = 0; i <= sqlDR.FieldCount - 1; i++)
                    {
                        csvText = csvText + sqlDR.GetValue(i).ToString().Replace("\n", " ").Replace("\r", " ") + ",";  //141005 SURAJIT
                    }
                    csvText = csvText.Substring(0, csvText.Length - 1) + "\n";
                }

                sqlCmd.Dispose();
                sqlDR.Close();

                string filepath = Server.MapPath("..\\export\\") + flnm;
                Stream fs = File.Create(filepath);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(csvText);
                sw.Close();
                fs.Close();

                fs = null;

                ////080905 SURAJIT
                sqlCmd1 = new SqlCommand("stp_UpdatetDocToExportReg", sqlConn);
                sqlCmd1.CommandType = CommandType.StoredProcedure;

                sqlCmd1.Parameters.Add("@buyerCompanyID", compID);  //ch 120905 SURAJIT

                sqlCmd1.ExecuteScalar();
                sqlCmd1.Dispose();
                sqlConn.Close();
                DownloadUserData(flnm);
            }
            catch (Exception ex) { retval = false; throw (ex); }
            finally { sqlDR.Close(); sqlCmd.Dispose(); sqlConn.Close(); }

            return retval;
        }
        #endregion
        // ==========================================================================================================
        #region ExportDocForAuth
        public bool ExportDocForAuth(string flnm, Int32 compID)  //ch 080905 Surajit
        {
            bool retval = true;
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString); ;
            SqlCommand sqlCmd = new SqlCommand();
            SqlCommand sqlCmd1 = new SqlCommand();
            SqlDataReader sqlDR = null;
            try
            {
                string csvText = "";
                sqlCmd = new SqlCommand("stp_GetDocToExportAuth", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@buyerCompanyID", compID);
                sqlConn.Open();
                sqlDR = sqlCmd.ExecuteReader();
                csvText = "DocumentType,Vendor,DocumentDate,PO Number,DocumentNumber,NetAmount,VatAmount,TotalAmount,Currency,TimePaymentDue,Description,Other,GMG Company,ACTNUMBR_2,ACTNUMBR_3,ACTNUMBR_4,CreditNoteInvoiceNumber"; //Amitava 
                csvText = csvText + "\n";
                while (sqlDR.Read())
                {
                    for (int i = 0; i <= sqlDR.FieldCount - 1; i++)
                    {
                        csvText = csvText + sqlDR.GetValue(i).ToString().Replace("\n", " ").Replace("\r", " ") + ",";  //271005 SURAJIT
                    }
                    csvText = csvText.Substring(0, csvText.Length - 1) + "\n";
                }
                sqlCmd.Dispose();
                sqlDR.Close();
                //rem 150905 SURAJIT
                string filepath = Server.MapPath("..\\export\\") + flnm;  //ch 080905 Surajit //"DocumentReg.XLS";			
                Stream fs = File.Create(filepath);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(csvText);
                sw.Close();
                fs.Close();
                fs = null;

                ////080905 SURAJIT
                sqlCmd1 = new SqlCommand("stp_UpdatetDocToExportAuth", sqlConn);
                sqlCmd1.CommandType = CommandType.StoredProcedure;
                sqlCmd1.Parameters.Add("@buyerCompanyID", compID);  //ch 120905 SURAJIT
                sqlCmd1.ExecuteScalar();
                sqlCmd1.Dispose();
                sqlConn.Close();
                DownloadUserData(flnm);
            }
            catch (Exception ex) { retval = false; throw (ex); }
            finally { sqlDR.Close(); sqlCmd.Dispose(); sqlConn.Close(); }

            return retval;
        }
        #endregion
        // ==========================================================================================================
        #region GetBuyerCompanyList
        public DataTable GetBuyerCompanyList(int compid)
        {
            SqlConnection sqlConn;
            SqlDataAdapter sqlDA;
            DataSet ds;

            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            sqlConn.Open();

            sqlDA = new SqlDataAdapter("stpGetBuyerCompanyList", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@id", compid);

            ds = new DataSet();

            sqlDA.Fill(ds);
            sqlConn.Close();

            return (ds.Tables[0]);
        }
        #endregion
        // ==========================================================================================================
        #region cboCompany_SelectedIndexChanged
        private void cboCompany_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            nCompanyId = Convert.ToInt32(cboCompany.SelectedValue);
            Session["compid"] = nCompanyId;
        }
        //140905 SURAJIT
        #endregion
        // =========================================================================================================
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
            this.cboCompany.SelectedIndexChanged += new System.EventHandler(this.cboCompany_SelectedIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        // =========================================================================================================
        #region DownloadUserData
        public void DownloadUserData(string flnm)
        {
            try
            {
                string filepath = Server.MapPath("..\\export\\") + flnm;

                Context.Response.ContentType = "application/save";
                Context.Response.AddHeader("content-disposition", "attachment; filename=" + flnm);
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                long FileSize = fs.Length;
                byte[] getContent = new byte[(int)FileSize];

                fs.Read(getContent, 0, (int)fs.Length);
                fs.Close();

                Context.Response.BinaryWrite(getContent);
                Context.Response.Flush();
                Context.Response.Close();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        // =========================================================================================================
    }
}