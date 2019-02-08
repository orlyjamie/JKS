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
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using Microsoft.Data.Odbc;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using System.Configuration;
using System.IO;

namespace JKS
{
    /// <summary>
    /// Summary description for upOrders.
    /// </summary>
    public class upOrders : CBSolutions.ETH.Web.ETC.VSPage
    {
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.Label Label1;
        protected System.Web.UI.HtmlControls.HtmlInputFile fileUpload;
        protected System.Web.UI.WebControls.Button btnUpload;
        protected System.Web.UI.WebControls.Label lblMessage;
        string strSupplierCode = "";
        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
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
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btnUpload_Click(object sender, System.EventArgs e)
        {
            lblMessage.Text = "";

            if (fileUpload.Value.Trim() == "")
            {
                lblMessage.Text = "Please upload an xml or a csv file.";
                return;
            }
            int iReturnValue = 0;
            iReturnValue = UploadFile();

            if (iReturnValue == 1)
            {
                if (ValidateSupplier(ConfigurationManager.AppSettings["DalkiaDataFileIN"] + @"\" + Path.GetFileName(fileUpload.PostedFile.FileName)))
                {
                    lblMessage.Text = "File uploaded successfully.";
                }
                else
                {
                    FileInfo fi2 = new FileInfo(ConfigurationManager.AppSettings["DalkiaDataFileIN"] + @"\" + Path.GetFileName(fileUpload.PostedFile.FileName));
                    if (fi2.Exists)
                    { fi2.Delete(); }
                    lblMessage.Text = "Sorry, one or more SupplierCodeAgainstBuyer is invalid.";
                }
            }
            else if (iReturnValue == -101)
                lblMessage.Text = "Sorry, invalid file path.";
            else if (iReturnValue == -102)
                lblMessage.Text = "Please upload an xml or a csv file.";
        }

        #region UploadFile Method Overloaded
        #region UploadFile which do not receive any parameter.
        private int UploadFile()
        {
            int iFlag = 0;

            if (fileUpload.PostedFile.ContentLength > 0)
            {
                if (fileUpload.PostedFile.FileName.Trim().Length > 0)
                {
                    if (ValidateFileName(Path.GetFileName(fileUpload.PostedFile.FileName)))
                        if (UploadFile(Path.GetFileName(fileUpload.PostedFile.FileName)))
                            iFlag = 1; // File uploaded successfully.
                        else
                            iFlag = -103; // Error uploading file.
                    else
                        iFlag = -102; // Please upload an xml or a csv file.
                }
                else
                {
                    iFlag = -101; // -101 means invalid path.
                }
            }
            else
            {
                iFlag = -101; // -101 means invalid path.
            }

            return (iFlag);
        }
        #endregion
        #region UploadFile accepts a string variable.
        private bool UploadFile(string strFileName)
        {
            //			string strFileNameToAttach = "";
            string strPathToSaveUploadedFile = "";
            bool bSaveFlag = false;
            try
            {
                FileInfo fi = new FileInfo(ConfigurationManager.AppSettings["DalkiaDataFileIN"] + @"\" + strFileName);

                if (fi.Exists)
                    fi.Delete();

                strPathToSaveUploadedFile = ConfigurationManager.AppSettings["DalkiaDataFileIN"];
                fileUpload.PostedFile.SaveAs(strPathToSaveUploadedFile + @"\" + strFileName);
                bSaveFlag = true;
                fi = null;

            }
            catch (Exception ex) { string err = ex.Message.ToString(); bSaveFlag = false; }

            return (bSaveFlag);
        }
        #endregion
        #endregion

        #region ValidateFileName
        private bool ValidateFileName(string strFileName)
        {
            bool bExtensionFlag = false;
            if (Path.GetExtension(strFileName).ToUpper().Equals(".XML") || Path.GetExtension(strFileName).ToUpper().Equals(".CSV"))
            {
                if (Path.GetExtension(strFileName).ToUpper().Equals(".XML"))
                    ViewState["FileExtension"] = "XML";
                else if (Path.GetExtension(strFileName).ToUpper().Equals(".CSV"))
                    ViewState["FileExtension"] = "CSV";

                strFileName = strFileName.ToUpper().Replace(".XML", "").Replace(".CSV", "");
                ViewState["FileName"] = strFileName;

                bExtensionFlag = true;
            }
            else
            {
                bExtensionFlag = false;
            }

            return (bExtensionFlag);
        }
        #endregion

        private bool ValidateSupplier(string strFileName)
        {
            bool iVal = true;
            //			StreamReader sr = null;
            FileInfo fi = null;
            DataSet dsCsv = new DataSet();
            fi = new FileInfo(strFileName);
            string connectionString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};DBQ=" + ConfigurationManager.AppSettings["DalkiaDataFileIN"];
            string SQL = "SELECT * FROM " + "[" + Path.GetFileName(strFileName).ToString().Trim() + "]";
            Microsoft.Data.Odbc.OdbcConnection connODBC = new OdbcConnection(connectionString);
            connODBC.Open();
            Microsoft.Data.Odbc.OdbcDataAdapter da = new OdbcDataAdapter(SQL, connODBC);
            da.Fill(dsCsv);

            if (dsCsv != null)
            {
                if (dsCsv.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i <= dsCsv.Tables[0].Rows.Count - 1; i++)
                    {
                        strSupplierCode = dsCsv.Tables[0].Rows[i].ItemArray[0].ToString();
                        strSupplierCode = strSupplierCode.Replace("/", "").Replace("\"", "");
                        if (strSupplierCode != "")
                        {
                            int iRetVal = CheckTradingRelationBySupplierCodeAgainstBuyer(strSupplierCode, 180918);//Boots Plc BuyerCompanyID=41707//124529 for AnchorSafety changed to 180918 for JKS
                            if (iRetVal == 0)
                            {
                                iVal = false;
                                break;
                            }

                        }
                    }
                }
            }

            return iVal;
        }

        #region CheckTradingRelationBySupplierCodeAgainstBuyer()
        public int CheckTradingRelationBySupplierCodeAgainstBuyer(string strSupplierCode, int strRecipientHubID)
        {
            //			bool isRecExist=false;
            int iReturnValue = 0;
            try
            {
                DataAccess da = new DataAccess(CBSAppUtils.PrimaryConnectionString.Trim());
                RecordSet rs = da.ExecuteQuery("TradingRelation", "SupplierCodeAgainstBuyer='" + strSupplierCode + "' and BuyerCompanyID='" + strRecipientHubID + "'");
                while (!rs.EOF())
                {
                    //					isRecExist=true;
                    iReturnValue = Convert.ToInt32(rs["SupplierCompanyID"]);
                    break;
                }
            }
            catch
            {
            }

            return iReturnValue;
        }
        #endregion
    }
}
