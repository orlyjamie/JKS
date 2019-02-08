using System;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
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

namespace JKS
{
    /// <summary>
    /// Summary description for CompanyUploadSupplier_NL.
    /// </summary>
    public class CompanyUploadSupplier_NL : CBSolutions.ETH.Web.ETC.VSPage
    {
        #region SqlClient's objects
        protected SqlCommand objComm = null;
        protected SqlConnection objConn = null;
        protected SqlConnection sqlConn = null;
        protected SqlDataAdapter sqlDA = null;
        protected SqlCommand sqlCmd = null;
        protected DataSet ds = null;
        #endregion
        #region WebControls
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Label lblHeader;
        protected System.Web.UI.HtmlControls.HtmlInputFile fileUploadSuppliers;
        protected System.Web.UI.WebControls.Label lblErr;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.DropDownList cboBuyer;
        #endregion
        #region VariableDeclaration
        private string strBuyerCompanyID = "0";
        private string strTradingRelationBuyerCompanyID = "0";
        protected bool bTradingRelationFlag = false;
        #endregion
        #region Page_Load
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../../close_win.aspx");

            baseUtil.keepAlive(this);

            if (Request["TRDBCID"] != null)
            {
                strTradingRelationBuyerCompanyID = Request["TRDBCID"].Trim();
                bTradingRelationFlag = true;
            }

            if (!Page.IsPostBack)
            {
                LoadData();
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
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        #region btnSubmit_Click
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            string strMsg = "";
            bool valid = true;

            if (bTradingRelationFlag)
                strBuyerCompanyID = strTradingRelationBuyerCompanyID;
            else
                strBuyerCompanyID = cboBuyer.SelectedValue.Trim();

            if (fileUploadSuppliers.PostedFile.ContentLength > 0)
            {
                if (fileUploadSuppliers.PostedFile.FileName.Length > 0)
                {
                    if (!(Path.GetExtension(fileUploadSuppliers.PostedFile.FileName).ToUpper().Equals(".CSV")) && !(Path.GetExtension(fileUploadSuppliers.PostedFile.FileName).ToUpper().Equals(".XML")))
                    {
                        valid = false;
                        lblErr.Text = "Please select a csv or xml file.";
                    }
                }
                else
                {
                    valid = false;
                    lblErr.Text = "Please select a csv or xml file.";
                }

                if (valid == true)
                {
                    int iNum = 0;
                    string strFileName = UploadFile();

                    if (strFileName.Trim() != "")
                    {
                        if (Path.GetExtension(strFileName).ToUpper().Equals(".XML"))
                        {
                            iNum = SaveDataFromXMLFile(strFileName);
                        }
                        else if (Path.GetExtension(strFileName).ToUpper().Equals(".CSV"))
                        {
                            iNum = ImportRecordFromFile(strFileName);
                        }
                    }

                    strMsg = iNum + " Records imported";
                    lblErr.Visible = true;
                    lblErr.Text = strMsg;
                    cboBuyer.SelectedIndex = 0;
                }
            }
            else
            {
                lblErr.Text = "Please select a valid file path.";
            }
        }
        #endregion
        #region ImportRecordFromFile
        private int ImportRecordFromFile(string fname)
        {
            string strCompanyCode = "";
            string strCompanyName = "";
            string strCompanyTypeID = "";
            string strMemberTypeID = "";
            string strNetworkID = "";
            string strUserID = "";

            string strSecVendorID = "";
            string strBuyerCompanyTypeID = "";
            string strBuyerCompanyName = "";
            string strVendorClass = "";
            string strVendorGroup = "";

            string strVendorAdd1 = "";
            string strVendorAdd2 = "";
            string strVendorAdd3 = "";
            string strVendorAdd4 = "";
            string strPostalCode = "";
            string strVendorRegNum = "";
            string strPhoneNumber = "";
            string strEmailID = "";
            string strAccountCurrency = "";

            int iCount = 0;

            strCompanyTypeID = "2";
            strMemberTypeID = "2";

            if (Session["UserID"] != null)
            {
                strUserID = Session["UserID"].ToString().Trim();

                SqlConnection objConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                objConn.Open();

                StreamReader MyFileStream = null;
                strBuyerCompanyID = "180918";//"20944";//124529 for AnchorSafety changed to 180918 for JKS
                string fileName = "";
                fileName = fname;

                FileInfo fi = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + fname);

                if (fi.Exists)
                {
                    MyFileStream = File.OpenText(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + fileName);

                    string MyLine;
                    int i = 1;
                    while (MyFileStream.Peek() != -1)
                    {
                        i++;
                        MyLine = MyFileStream.ReadLine();

                        if (i > 0)
                        {
                            MyLine = MyLine.Replace("\\", "").Replace("\"", "");
                            String[] MyArray = MyLine.Split(',');

                            strCompanyCode = MyArray[0].ToString().Trim();
                            strSecVendorID = MyArray[1].ToString().Trim();
                            strBuyerCompanyTypeID = MyArray[2].ToString().Trim();
                            strBuyerCompanyName = MyArray[3].ToString().Trim();
                            strVendorClass = MyArray[4].ToString().Trim();
                            strVendorGroup = MyArray[5].ToString().Trim();
                            strCompanyName = MyArray[6].ToString().Trim();
                            strVendorAdd1 = MyArray[7].ToString().Trim();
                            strVendorAdd2 = MyArray[8].ToString().Trim();
                            strVendorAdd3 = MyArray[9].ToString().Trim();
                            strVendorAdd4 = MyArray[10].ToString().Trim();
                            strPostalCode = MyArray[11].ToString().Trim();
                            strVendorRegNum = MyArray[12].ToString().Trim();
                            strPhoneNumber = MyArray[13].ToString().Trim();
                            strEmailID = MyArray[14].ToString().Trim();
                            strAccountCurrency = MyArray[15].ToString().Trim();


                            if (strCompanyName.Trim() != "" && strCompanyName.ToUpper().Trim().IndexOf("VENDOR_NAME") == -1)
                            {
                                strNetworkID = System.Guid.NewGuid().ToString().Substring(0, 11).Trim();
                                int intRecordCount = 0;
                                sqlCmd = new SqlCommand("sp_ImportRecordsFromURL_NL", sqlConn);
                                sqlCmd.CommandType = CommandType.StoredProcedure;

                                sqlCmd.Parameters.Add("@CompanyCode", strCompanyCode);
                                sqlCmd.Parameters.Add("@CompanyName", strCompanyName);
                                sqlCmd.Parameters.Add("@CompanyTypeID", strCompanyTypeID);
                                sqlCmd.Parameters.Add("@MemberTypeID", strMemberTypeID);
                                sqlCmd.Parameters.Add("@NetworkID", strNetworkID);
                                sqlCmd.Parameters.Add("@BuyerCompanyID", strBuyerCompanyID);
                                sqlCmd.Parameters.Add("@UserID", 0);
                                sqlCmd.Parameters.Add("@SecVendorID", strSecVendorID);
                                sqlCmd.Parameters.Add("@BuyerCompanyTypeID", strBuyerCompanyTypeID);
                                sqlCmd.Parameters.Add("@BuyerCompanyName", strBuyerCompanyName);
                                sqlCmd.Parameters.Add("@VendorClass", strVendorClass);
                                sqlCmd.Parameters.Add("@VendorGroup", strVendorGroup);
                                sqlCmd.Parameters.Add("@VendorAdd1", strVendorAdd1);
                                sqlCmd.Parameters.Add("@VendorAdd2", strVendorAdd2);
                                sqlCmd.Parameters.Add("@VendorAdd3", strVendorAdd3);
                                sqlCmd.Parameters.Add("@VendorAdd4", strVendorAdd4);
                                sqlCmd.Parameters.Add("@PostalCode", strPostalCode);
                                sqlCmd.Parameters.Add("@VendorRegNum", strVendorRegNum);
                                sqlCmd.Parameters.Add("@PhoneNumber", strPhoneNumber);
                                sqlCmd.Parameters.Add("@EmailID", strEmailID);
                                sqlCmd.Parameters.Add("@AccountCurrency", strAccountCurrency);
                                SqlParameter paramReturnValue = objComm.Parameters.Add("ReturnValue", SqlDbType.Int);
                                paramReturnValue.Direction = ParameterDirection.ReturnValue;
                                try
                                {
                                    objComm.ExecuteNonQuery();
                                    intRecordCount = Convert.ToInt32(objComm.Parameters["ReturnValue"].Value);
                                }
                                catch (Exception ex) { string ss = ex.Message.ToString(); }
                                finally
                                {
                                    objComm.Dispose();
                                }

                                if (intRecordCount > 0)
                                {
                                    iCount = iCount + 1;
                                }

                                strNetworkID = "";
                            }
                        }
                    }
                }

                MyFileStream.Close();
                if (fi.Exists)
                {
                    fi.Delete();
                    fi = null;
                }
            }

            return iCount;
        }
        #endregion
        #region UploadFile
        private string UploadFile()
        {
            string sFname = "";
            if (fileUploadSuppliers.PostedFile.FileName.Length > 0)
            {
                sFname = Path.GetFileName(fileUploadSuppliers.PostedFile.FileName);
                FileInfo fi = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + sFname);
                if (fi.Exists)
                {
                    fi.Delete();
                    fi = null;
                }

                string fileto = Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]);
                fileUploadSuppliers.PostedFile.SaveAs(fileto + "\\" + sFname);

            }
            return sFname;
        }
        #endregion
        #region SaveDataFromXMLFile
        private int SaveDataFromXMLFile(string strFileName)
        {
            int iCount = 0;

            if (Session["UserID"] != null)
            {
                FileStream myFs = new FileStream(Server.MapPath(ConfigurationManager.AppSettings["UploadExcel"]) + strFileName, FileMode.Open, FileAccess.Read);

                ds = new DataSet();
                ds.ReadXml(myFs);

                sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);


                string strUserID = Session["UserID"].ToString().Trim();

                try
                {
                    sqlConn.Open();
                    string strCompanyCode = "";
                    string strCompanyName = "";
                    string iCompanyTypeID = "";
                    string iMemberTypeID = "";

                    string strAddress1 = "";
                    string strAddress2 = "";
                    string strAddress3 = "";
                    string strCounty = "";

                    string strCountry = "";

                    string strPostCode = "";
                    string strPhoneNumber1 = "";

                    string strEmailID = "";

                    string strNetworkID = "";

                    for (int counter = 0; counter < ds.Tables[0].Rows.Count; counter++)
                    {
                        strCompanyCode = ds.Tables[0].Rows[counter]["CompanyCode"].ToString().Trim();
                        strCompanyName = ds.Tables[0].Rows[counter]["CompanyName"].ToString().Trim();

                        iCompanyTypeID = "2";
                        iMemberTypeID = "2";

                        strAddress1 = ds.Tables[0].Rows[counter]["Address1"].ToString().Trim();
                        strAddress2 = ds.Tables[0].Rows[counter]["Address2"].ToString().Trim();
                        strAddress3 = ds.Tables[0].Rows[counter]["Address3"].ToString().Trim();
                        strCounty = ds.Tables[0].Rows[counter]["County"].ToString().Trim();

                        strCountry = ds.Tables[0].Rows[counter]["Country"].ToString().Trim();

                        strPostCode = ds.Tables[0].Rows[counter]["PostCode"].ToString().Trim();
                        strPhoneNumber1 = ds.Tables[0].Rows[counter]["PhoneNumber1"].ToString().Trim();

                        try
                        {
                            strEmailID = ds.Tables[0].Rows[counter]["Email"].ToString().Trim();
                        }
                        catch { }

                        if (strCompanyCode != "")
                        {
                            strNetworkID = System.Guid.NewGuid().ToString().Substring(0, 11).Trim();
                            int intRecordCount = 0;
                            sqlCmd = new SqlCommand("stpAddNewRecordFromExcel", sqlConn);
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.Add("@CompanyCode", strCompanyCode);
                            sqlCmd.Parameters.Add("@CompanyName", strCompanyName);
                            sqlCmd.Parameters.Add("@CompanyTypeID", iCompanyTypeID);
                            sqlCmd.Parameters.Add("@MemberTypeID", iMemberTypeID);
                            sqlCmd.Parameters.Add("@VatRegNo", null);
                            sqlCmd.Parameters.Add("@Address1", strAddress1);
                            sqlCmd.Parameters.Add("@Address2", strAddress2);
                            sqlCmd.Parameters.Add("@Address3", strAddress3);
                            sqlCmd.Parameters.Add("@County", strCounty);
                            sqlCmd.Parameters.Add("@Country", strCountry);
                            sqlCmd.Parameters.Add("@PostCode", strPostCode);
                            sqlCmd.Parameters.Add("@PhoneNumber1", strPhoneNumber1);
                            sqlCmd.Parameters.Add("@UserID", strUserID);
                            sqlCmd.Parameters.Add("@NetworkID", strNetworkID);
                            sqlCmd.Parameters.Add("@BuyerCompanyID", strBuyerCompanyID);
                            sqlCmd.Parameters.Add("@EmailID", strEmailID);
                            SqlParameter paramReturnValue = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                            paramReturnValue.Direction = ParameterDirection.ReturnValue;
                            try
                            {
                                sqlCmd.ExecuteNonQuery();
                                intRecordCount = Convert.ToInt32(sqlCmd.Parameters["ReturnValue"].Value);
                            }
                            catch (Exception ex) { string ss = ex.Message.ToString(); }
                            finally
                            {
                                sqlCmd.Dispose();
                                myFs.Close();
                                ds.Dispose();
                            }

                            if (intRecordCount > 0)
                            {
                                iCount = iCount + 1;
                            }

                            strNetworkID = "";
                        }
                    }
                }
                catch { myFs.Close(); ds.Dispose(); sqlConn.Close(); }
                finally
                {

                    myFs.Close();
                    ds.Dispose();
                    sqlConn.Close();
                }
            }

            return (iCount);
        }
        #endregion
        #region RollDice
        public int RollDice()
        {
            int i = 0;

            Random r = new Random();

            i = r.Next(999999);

            return i;
        }
        #endregion
        #region LoadData
        private void LoadData()
        {
            cboBuyer.Items.Clear();
            RecordSet rs = Company.GetBuyerCompanyList();
            while (!rs.EOF())
            {
                ListItem listItem = new ListItem(rs["CompanyName"].ToString(), rs["CompanyID"].ToString());
                cboBuyer.Items.Add(listItem);
                rs.MoveNext();
            }

            cboBuyer.Items.Insert(0, new ListItem("Select Buyer Company", "0"));
            rs = null;
        }
        #endregion
    }
}
