using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.IO;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CBSolutions.Architecture.Data;

namespace JKS
{
    /// <summary>
    /// Summary description for ExportInvoice.
    /// </summary>
    public class ExportInvoice : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Panel Panel3;
        protected System.Web.UI.WebControls.DropDownList ddlBuyerCompany;
        protected System.Web.UI.WebControls.Button btnDwnloadDB;
        private downloadDB objdownloadDB = new downloadDB();

        private void Page_Load(object sender, System.EventArgs e)
        {
            btnDwnloadDB.Attributes.Add("onclick", "return validation();");
            if (!IsPostBack)
            {
                populate();
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
            this.btnDwnloadDB.Click += new System.EventHandler(this.btnDwnloadDB_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btnDwnloadDB_Click(object sender, System.EventArgs e)
        {
            string fpath = ConfigurationManager.AppSettings["InvoiceExportPath_ETC"].Trim() + @"\" + ddlBuyerCompany.SelectedItem.ToString() + Convert.ToDateTime(DateTime.Now.ToLongDateString()).ToString("ddMMyyyy") + Convert.ToDateTime(DateTime.Now.ToLongTimeString()).ToString("HHmmss") + Session["UserID"].ToString() + ".csv";
            Stream fs = File.Create(fpath);
            fs.Close();
            Stream fs1 = File.Open(fpath, FileMode.Open, FileAccess.ReadWrite);
            string csvTxt = "";
            string ConString = ConfigurationManager.AppSettings["ConnectionString"].Trim();
            SqlConnection sqlConn = new SqlConnection(ConString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_ExportInvoice_ETC", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@BuyerCompanyID", Convert.ToInt32(ddlBuyerCompany.SelectedValue));
            DataSet ds = new DataSet();
            try
            {
                sqlConn.Open();
                sqlDA.Fill(ds);
                csvTxt = "Reference,Document Date,Originator Reference,Supplier Code,Currency,Currency Rate,Invoice Total,,Description,Units,Quantity,Unit Price,	Discount %,Vat Rate,VAT Amount,Gross,Account Code,Department,Cost Centre,hyperlink \n";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["Reference"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["DocumentDate"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["OriginatorReference"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["SupplierCode"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["Currency"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["CurrencyRate"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["InvoiceTotal"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["blank"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["Description"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["Units"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["Quantity"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["UnitPrice"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["Discount"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["VatRate"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["VATAmount"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["Gross"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["AccountCode"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["Department"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["CostCentre"].ToString() + ",";
                        csvTxt = csvTxt + ds.Tables[0].Rows[i]["hyperlink"].ToString() + "";
                        StreamWriter SW;
                        SW = new StreamWriter(fs1);
                        SW.WriteLine(csvTxt);
                        SW.Flush();
                        csvTxt = "";
                    }
                }


            }
            catch (Exception ex)
            { string err = ex.Message.ToString(); }
            finally
            {
                sqlDA.Dispose();
                sqlConn.Close();
                fs1.Close();
            }
            Page.RegisterStartupScript("reg", "<script>FnCompleted();</script>");
        }

        private void populate()
        {
            DataTable dtbl = null;
            dtbl = objdownloadDB.GetBuyerCompanyListDropDown(Convert.ToInt32(Session["CompanyID"]));

            if (dtbl != null)
            {
                ddlBuyerCompany.DataSource = dtbl;
                ddlBuyerCompany.DataBind();

            }
            ddlBuyerCompany.Items.Insert(0, new ListItem("Select Company", "0"));
        }
    }
}
