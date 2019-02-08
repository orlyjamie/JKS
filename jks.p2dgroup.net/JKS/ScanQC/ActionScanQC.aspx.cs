using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Net;

namespace JKS
{
    public partial class JKS_ScanQC_ActionScanQC : System.Web.UI.Page
    {
        #region Variable declaration and initialization
        SqlConnection sqlConn = null;
        DataCenterActionScanQC DC = null;
        bool test = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["test"].ToString());
        public string NewSupplierLink = "";
        public string buyercompanyid = "0";
        public string suppliercompanyid = "0";

        //the following setting is only for test purpose, don't consider it for live work
        string testXMLFileName = "DOC15062017.xml";//"DOC1000005.xml";//"DOC1805450.xml";//"DOC1807954.xml";//"DOC1000001.xml";//"DOC1000001 - Copy.xml"; //"DOC1698385.xml";
        #endregion

        #region Events


        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            string test = hdnRowNo.Value;
            string[] arrIndex = test.Split(',');
            arrIndex = arrIndex.Take(arrIndex.Count() - 1).ToArray();
            //DataTable dt = ViewState["LineItemsDT"] as DataTable;
            DataTable dt = (ViewState["LineItemsDT"] as DataTable).Clone();
            //Updated by Mainak 2017-11-21
            foreach (GridViewRow row in gvLists.Rows)
            {
                DataRow newRow = dt.NewRow();
                newRow["PONO"] = (row.FindControl("txtPONO") as TextBox).Text;
                newRow["BUYERCODE"] = (row.FindControl("txtBuyerCode") as TextBox).Text;
                newRow["DESC"] = (row.FindControl("txtDescription") as TextBox).Text;
                newRow["QTY"] = (row.FindControl("txtQTY") as TextBox).Text;
                newRow["PRICE"] = (row.FindControl("txtPRICE") as TextBox).Text;
                newRow["VALUE"] = (row.FindControl("txtVALUE") as TextBox).Text;
                newRow["GOODSRECDDETAILID"] = (row.FindControl("lblGoodsRecdDetailID") as Label).Text;
                newRow["DEPARTMENTID"] = (row.FindControl("lblDeptID") as Label).Text;
                newRow["NOMINALCODEID"] = (row.FindControl("lblNominalCodeID") as Label).Text;
                newRow["BUSINESSUNITID"] = (row.FindControl("lblBusinessUnitID") as Label).Text;
                newRow["PROJECTCODE"] = (row.FindControl("lblProjectCode") as Label).Text;
                newRow["PURORDERLINENO"] = (row.FindControl("lblPurOrderLineNo") as Label).Text;//Added by Mainak 2018-05-31
                dt.Rows.Add(newRow);
            }

            ViewState["LineItemsDT"] = dt;

            //Commented By Mainak 2017-11-28
            //if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[dt.Rows.Count - 1]["PONO"])))
            //{
            //    dt.Rows.RemoveAt(dt.Rows.Count - 1);
            //}

            foreach (var index in arrIndex)
            {
                DataRow newRow = dt.NewRow();
                newRow["PONO"] = lblPO.Text;
                newRow["BUYERCODE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBuyerCode") as Label).Text;
                newRow["DESC"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDesc") as Label).Text;
                newRow["QTY"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblQty") as Label).Text;
                newRow["PRICE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPrice") as Label).Text;
                newRow["VALUE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblValue") as Label).Text;
                newRow["GOODSRECDDETAILID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblGoodsRecdDetailID") as Label).Text;
                newRow["DEPARTMENTID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDeptID") as Label).Text;
                newRow["NOMINALCODEID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblNominalCodeID") as Label).Text;
                newRow["BUSINESSUNITID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBusinessUnitID") as Label).Text;
                newRow["PROJECTCODE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblProjectCode") as Label).Text;
                newRow["PURORDERLINENO"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPurOrderLineNo") as Label).Text;//Added by Mainak 2018-05-31
                dt.Rows.Add(newRow);
            }

            ViewState["LineItemsDT"] = dt;
            gvLists.DataSource = dt;
            gvLists.DataBind();
        }


        protected void btnReplaceModal_Click(object sender, EventArgs e)
        {
            string test = hdnRowNo.Value;
            string[] arrIndex = test.Split(',');
            arrIndex = arrIndex.Take(arrIndex.Count() - 1).ToArray();
            DataTable dt = (ViewState["LineItemsDT"] as DataTable).Clone();

            foreach (var index in arrIndex)
            {
                DataRow newRow = dt.NewRow();
                newRow["PONO"] = lblPO.Text;
                newRow["BUYERCODE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBuyerCode") as Label).Text;
                newRow["DESC"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDesc") as Label).Text;
                newRow["QTY"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblQty") as Label).Text;
                newRow["PRICE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPrice") as Label).Text;
                newRow["VALUE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblValue") as Label).Text;
                newRow["GOODSRECDDETAILID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblGoodsRecdDetailID") as Label).Text;
                newRow["DEPARTMENTID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblDeptID") as Label).Text;
                newRow["NOMINALCODEID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblNominalCodeID") as Label).Text;
                newRow["BUSINESSUNITID"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblBusinessUnitID") as Label).Text;
                newRow["PROJECTCODE"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblProjectCode") as Label).Text;
                newRow["PURORDERLINENO"] = (gvProduct.Rows[Convert.ToInt32(index)].FindControl("lblPurOrderLineNo") as Label).Text;//Added by Mainak 2018-05-31
                dt.Rows.Add(newRow);
            }

            ViewState["LineItemsDT"] = dt;
            gvLists.DataSource = dt;
            gvLists.DataBind();
        }


        protected void btnOpenModal_Click(object sender, EventArgs e)
        {
            try
            {
                string buyercompanyid = ddlCompany.SelectedValue.ToString() ;//Added by MAinak 2018-07-18
                SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                DataSet DS = new DataSet();

                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("SP_getValuesForModalPONo_JKS", sqlConn);//Modified by MAinak 2018-07-18
                cmd.Parameters.AddWithValue("@PurOrderNo", hdnPurOrderNo.Value);
                cmd.Parameters.AddWithValue("@CompanyID", buyercompanyid);//Added by MAinak 2018-07-18
                

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(DS);
                sqlConn.Close();

                lblPO.Text = hdnPurOrderNo.Value;
                lblCompany.Text = (DS.Tables[0].Rows[0]["Company"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["Company"]);
                lblSupplier.Text = (DS.Tables[0].Rows[0]["Supplier"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["Supplier"]);
                lblDate.Text = (DS.Tables[0].Rows[0]["Date"] as object == DBNull.Value) ? "" : (Convert.ToDateTime(DS.Tables[0].Rows[0]["Date"])).ToString("dd/MM/yyyy");
                lblCurrency.Text = (DS.Tables[0].Rows[0]["Currency"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["Currency"]);
                lblBuyer.Text = (DS.Tables[0].Rows[0]["Buyer"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["Buyer"]);
                //hdnBuyerCode.Value = (DS.Tables[0].Rows[0]["BuyerCode"] as object == DBNull.Value) ? "" : Convert.ToString(DS.Tables[0].Rows[0]["BuyerCode"]);


                gvProduct.DataSource = DS.Tables[1];
                gvProduct.DataBind();

                double sumQ = Convert.ToDouble(DS.Tables[1].Compute("SUM(Quantity)", string.Empty));
                double sumV = Convert.ToDouble(DS.Tables[1].Compute("SUM(NetAmt)", string.Empty));

                //DataRow[] dr1 = DS.Tables[1].Select("SUM(Quantity)");
                //Blocked By sonali 25.6.2018 
                // (gvProduct.FooterRow.FindControl("lblQtySum") as Label).Text = Convert.ToString(sumQ);
                //Added By sonali 25.6.2018
                (gvProduct.FooterRow.FindControl("lblQtySum") as Label).Text = sumQ.ToString("0.00"); 

                //Modified by Mainak 2017-11-29
                //(gvProduct.FooterRow.FindControl("lblValueSum") as Label).Text = Convert.ToString(sumV);
                (gvProduct.FooterRow.FindControl("lblValueSum") as Label).Text = sumV.ToString("0.00");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "$('#addModal').modal('show');", true);
            }
            catch
            {
                //do nothing
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            ddlCompany.AutoPostBack = true;
            ddlCompany.SelectedIndexChanged += new EventHandler(ddlCompany_SelectedIndexChanged);

            ddlBatchType.AutoPostBack = true;
            ddlBatchType.SelectedIndexChanged += new EventHandler(ddlBatchType_SelectedIndexChanged);

            btnFind.Click += new EventHandler(btnFind_Click);
            btnCalc.Click += new EventHandler(btnCalc_Click);

            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnDiscard.Click += new EventHandler(btnDiscard_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnProcess.Click += new EventHandler(btnProcess_Click);
            btnClose.Click += new EventHandler(btnClose_Click);

            btnAddLines.Click += new EventHandler(btnAddLines_Click);
            btnDeleteLines.Click += new EventHandler(btnDeleteLines_Click);
            btnCalculate.Click += new EventHandler(btnCalculate_Click);

            gvLists.RowDataBound += new GridViewRowEventHandler(gvLists_RowDataBound);

            Page.LoadComplete += new EventHandler(Page_LoadComplete);

            lbtnDnldPrev.Click += new EventHandler(lbtnDnldPrev_Click);
            lbtnDnldThis.Click += new EventHandler(lbtnDnldThis_Click);
            lbtnDnldNext.Click += new EventHandler(lbtnDnldNext_Click);

            lbtnOriginalDocument.Click += new EventHandler(lbtnOriginalDocument_Click);

            lbtnAtcPrev.Click += new EventHandler(lbtnAtcPrev_Click);
            lbtnAtcNext.Click += new EventHandler(lbtnAtcNext_Click);

            lbtnSplitAndReprocess.Click += new EventHandler(lbtnSplitAndReprocess_Click);

            btnAddModal.Click += btnAddModal_Click;
            btnReplaceModal.Click += btnReplaceModal_Click;

            //foreach (string s in Session.Keys)
            //{
            //    Response.Write(s+"<br>");
            //}

            SetNewSupplierLink();

            btnDelete.Attributes.Add("onclick", "this.disabled = true; " + ClientScript.GetPostBackEventReference(btnDelete, null) + ";");
            btnDiscard.Attributes.Add("onclick", "this.disabled = true; " + ClientScript.GetPostBackEventReference(btnDiscard, null) + ";");
            btnSave.Attributes.Add("onclick", "this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            btnProcess.Attributes.Add("onclick", "this.disabled = true; " + ClientScript.GetPostBackEventReference(btnProcess, null) + ";");
            btnClose.Attributes.Add("onclick", "this.disabled = true; " + ClientScript.GetPostBackEventReference(btnClose, null) + ";");
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DC = new DataCenterActionScanQC(sqlConn);

            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    int comID = Convert.ToInt32(Request.QueryString["cid"]);
                    int docID = Convert.ToInt32(Request.QueryString["did"]);
                    int sComID = (int)Session["CompanyID"];

                    LoadTopData(docID);
                    DC.PopulateChildCompany(sComID, ddlCompany);
                    DC.PopulateCurrency(ddlCurrency);

                    ddlCompany.SelectedValue = DC.ReturnClientCompanyID(docID).ToString();
                    ddlCompany_SelectedIndexChanged(sender, e);
                    ddlBatchType_SelectedIndexChanged(sender, e);
                }

                SetJavaScriptConfirmBox();
            }

            SetCompanyID();
        }


        protected void btnFind_Click(object sender, EventArgs e)
        {
            SetCompanyID();
        }


        protected void btnCalc_Click(object sender, EventArgs e)
        {
            //header section's update
            string netS = "";
            string vatS = "";

            netS = txtNet.Text.Trim().Replace("&nbsp;", "").Replace(" ", "");//Net Amount
            vatS = txtVAT.Text.Trim().Replace("&nbsp;", "").Replace(" ", "");//VAT Amount

            netS = (netS.Length == 1 && netS == ".") ? "" : netS;//Net Amount
            vatS = (vatS.Length == 1 && vatS == ".") ? "" : vatS;//VAT Amount

            netS = (string.IsNullOrEmpty(netS)) ? "0.00" : netS;//Net Amount
            vatS = (string.IsNullOrEmpty(vatS)) ? "0.00" : vatS;//VAT Amount

            netS = Math.Round(Convert.ToDecimal(netS), 2).ToString();//Net Amount
            vatS = Math.Round(Convert.ToDecimal(vatS), 2).ToString();//VAT Amount

            netS = (netS.Contains(".")) ? netS : netS + ".00";//Net Amount
            vatS = (vatS.Contains(".")) ? vatS : vatS + ".00";//VAT Amount

            txtNet.Text = netS;
            txtVAT.Text = vatS;

            decimal net = Convert.ToDecimal(netS);
            decimal vat = Convert.ToDecimal(vatS);

            decimal tot = net + vat;

            txtTotal.Text = Math.Round(tot, 2).ToString();

            SetAccountsCurrency();

            ReSetRedBorders();
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int docID = Convert.ToInt32(lblDocID.Text);
            if (CheckFinalArchivingDate(docID: docID))
            {

                string username = DC.ReturnUserName((int)Session["UserID"]);

                bool tf = username.ToLower().Contains("scan");

                if (tf)
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('You do not have access rights to delete invoices.');", true);
                }
                else
                {
                    //int docID = Convert.ToInt32(lblDocID.Text);
                    int batchID = Convert.ToInt32(lblBatchID.Text);

                    if (!test)
                    {
                        tf = DeleteRelatedFiles();

                        if (tf)
                        {
                            //following collects data before the current doc is deleted.
                            DataTable DT = DC.ReturnDOCIDs_DT(batchID, false);

                            tf = DC.MakeDocumentDeleted(docID, batchID);

                            if (tf)
                            {
                                LoadNext(DT);

                                int userID = Convert.ToInt32(Session["UserID"]);
                                DC.UpdateClerkIdEditing(docID, userID);
                            }
                        }
                    }
                    else
                    {
                        //following collects data before the current doc is deleted.
                        DataTable DT = DC.ReturnDOCIDs_DT(batchID, false);

                        //tf = DC.MakeDocumentDeleted(docID, batchID);

                        if (tf)
                            LoadNext(DT);
                    }
                }
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('This document has already been processed. Please press CLOSE to allow you to continue')", true);
            }
        }


        protected void btnDiscard_Click(object sender, EventArgs e)
        {
            //1.Reset BEING EDITED to False in DOCUMENT PROGRESS table
            //2.Load next invoice without saving changes to the current invoice
            LoadNext(null);
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            /*after 12-04-2016: If SAVE is pressed but there is no corresponding XML in the 
             * IPE Output folder then automatically perform DISCARD operation instead.*/

            int docID = Convert.ToInt32(lblDocID.Text);

            if (CheckFinalArchivingDate(docID: docID))
            {
                string xmlPath = "";

                if (test)
                {
                    string xmlDir = "C:\\P2D\\IPE Output\\DefaultCompany";
                    xmlPath = xmlDir + "\\" + testXMLFileName;
                }
                else
                {
                    string xmlDir = "D:\\P2D\\IPE Output\\" + ddlCompany.SelectedItem.Text;
                    xmlPath = xmlDir + "\\DOC" + lblDocID.Text + ".xml";
                }

                //Response.Write(xmlPath);
                //Response.Write("<br />");
                //Response.Write(File.Exists(xmlPath));

                /*after 12-04-2016*/
                if (!File.Exists(xmlPath))//if xml not found
                {
                    btnDiscard_Click(sender, e);
                }
                else//if xml found
                {
                    bool tf = false;

                    tf = UpdateXMLWhenSaved(xmlPath);

                    if (tf)
                    {
                        //int docID = Convert.ToInt32(lblDocID.Text);
                        int userID = Convert.ToInt32(Session["UserID"]);
                        DC.UpdateClerkIdEditing(docID, userID);

                        LoadNext(null);
                    }
                }
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('This document has already been processed. Please press CLOSE to allow you to continue')", true);
            }
        }


        protected void btnProcess_Click(object sender, EventArgs e)
        {
            //validation checking is done by JavaScript function validation() as name
            //written in the aspx page.

            int docID = Convert.ToInt32(lblDocID.Text);
            int batchID = Convert.ToInt32(lblBatchID.Text);

            if (CheckFinalArchivingDate(docID: docID))
            {
                bool TF = false;

                SetAccountsCurrency();

                ReSetRedBorders();

                DataTable DT = null;

                //if not test then live action is performed
                if (!test)
                {
                    TF = SaveDataToXMLWhenProcessed();
                    //Response.Write("<br />SaveDataToXMLWhenProcessed: " + TF);

                    if (TF)
                    {
                        //following collects data before the current doc is processed.
                        DT = DC.ReturnDOCIDs_DT(batchID, false);
                    }

                    if (TF)
                        TF = DC.UpdateFinalArchivingDate(docID);
                    //Response.Write("<br />UpdateFinalArchivingDate: " + TF);

                    if (TF)
                        TF = DC.UpdateNumOfInvoicesArchivedByQC(batchID);
                    //Response.Write("<br />UpdateNumOfInvoicesArchivedByQC: " + TF);

                    if (TF)
                        TF = DeleteRelatedFiles();
                    //Response.Write("<br />DeleteRelatedFiles: " + TF);

                    //TF = false;

                    if (TF)
                    {
                        LoadNext(DT);

                        int userID = Convert.ToInt32(Session["UserID"]);
                        DC.UpdateClerkIdEditing(docID, userID);
                    }
                }
                else
                {
                    TF = SaveDataToXMLWhenProcessed();

                    //if (TF)
                    //{
                    //    //following collects data before the current doc is processed.
                    //    DT = DC.ReturnDOCIDs_DT(batchID, false);
                    //}

                    //if (TF)
                    //    TF = DC.UpdateFinalArchivingDate(docID);

                    if (TF)
                        LoadNext(DT);
                }

                //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('code executed.');", true);
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('This document has already been processed. Please press CLOSE to allow you to continue')", true);
            }
        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            //close the page
            int docID = Convert.ToInt32(lblDocID.Text);
            DC.UpdateBeingEdited("NO", docID);

            string url = "ActionWindow.aspx";
            Redirection(url);
        }


        protected void btnAddLines_Click(object sender, EventArgs e)
        {
            //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_all", "alert('" + ddlAddLines.Text + "')", true);
            DataTable DT = (DataTable)ViewState["LineItemsDT"];

            int i = 0;

            //get existing data from grid view
            DT = ReturnExistingDataAsDataTable(DT);

            if (DT != null)
            {
                int c = DT.Rows.Count;

                DataRow DR = DT.NewRow();

                bool isSel = false;
                int x = 0;

                //search each row to find check box and add a row at top or at buttom.
                foreach (GridViewRow GVR in gvLists.Rows)
                {
                    DR = DT.NewRow();

                    //iterate columns and set value to row
                    for (i = 0; i < (DT.Columns.Count - 1); i++)
                        DR[i] = "";

                    DR[i] = "NOT FOUND/NOT FOUND";
                    int lci = i + 1;//take the list column index

                    CheckBox cbSelect = (CheckBox)GVR.FindControl("cbSelect");
                    i = 0;

                    //if the check box is checked.
                    if (cbSelect.Checked == true)
                    {
                        //the selected row.
                        GridViewRow GVR1 = (GridViewRow)cbSelect.NamingContainer;

                        int sri = GVR1.RowIndex;//selected row index

                        DR[lci] = DT.Rows[sri][lci];

                        switch (ddlAddLines.Text)
                        {
                            case "Above":
                                if (GVR1.RowIndex == 0)
                                    i = GVR1.RowIndex;
                                else
                                    i = GVR1.RowIndex + x;
                                DT.Rows.InsertAt(DR, i);
                                x++;
                                break;
                            case "Below":
                                if (GVR1.RowIndex == 0)
                                    i = GVR1.RowIndex + 1;
                                else
                                    i = GVR1.RowIndex + x + 1;
                                DT.Rows.InsertAt(DR, i);
                                x++;
                                break;
                        }

                        isSel = true;
                    }
                }

                //when no check box is checked then insert to the top of the gird
                if (isSel == false)
                {
                    int ci = DT.Columns.Count - 1;
                    int ri = 0;

                    switch (ddlAddLines.Text)
                    {
                        case "Above":
                            ri = 0;
                            DR[ci] = DT.Rows[ri][ci];
                            DT.Rows.InsertAt(DR, 0);
                            break;
                        case "Below":
                            ri = DT.Rows.Count - 1;
                            DR[ci] = DT.Rows[ri][ci];
                            DT.Rows.InsertAt(DR, c);
                            break;
                    }
                }

                ViewState["LineItemsDT"] = DT;

                gvLists.DataSource = DT;
                gvLists.DataBind();
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Can not add Row to an Empty Table.')", true);
                //Response.Write("<script>alert('what's up?')</script>");
            }
        }


        protected void btnDeleteLines_Click(object sender, EventArgs e)
        {
            DataTable DT = (DataTable)ViewState["LineItemsDT"];

            //get existing data from grid view
            DT = ReturnExistingDataAsDataTable(DT);

            if (DT != null)
            {
                int c = DT.Rows.Count;

                for (int i = c - 1; i >= 0; i--)
                {
                    GridViewRow GVR = gvLists.Rows[i];

                    CheckBox cbSelect = (CheckBox)GVR.FindControl("cbSelect");

                    if (cbSelect.Checked == true)
                    {
                        DT.Rows.RemoveAt(i);
                    }
                }

                if (DT.Rows.Count == 0)
                {
                    DataRow DR = DT.NewRow();
                    for (int i = 0; i < 5; i++)
                        DR[i] = "";
                    DT.Rows.InsertAt(DR, 0);
                }

                ViewState["LineItemsDT"] = DT;

                gvLists.DataSource = DT;
                gvLists.DataBind();
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('Can not delete Row from an Empty Table.')", true);
                //Response.Write("<script>alert('kempoy')</script>");
            }
        }


        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            /*Automatically updates Total field (in Header tab) by recalculating value of Net + VAT fields; 
             * AND automatically updates Value field for all line items based on Price x Quantity (in Line Items tab);
             * AND line item sub-totals; and updates ‘Account Currency’ field. 
             * I.e. this button performs the same function as Calc. Button in Header tab.*/

            /*after 4th August, 2016
            I have tested again and Issue #3b seems OK however for #1 I notice that Calc. button in Header tab is also performing recalculation of line items – please can you stop this from happening too.

            04/08/2016 Koushik’s Reply :
            The requirement was to make Calc. of Header and CALC. of LineItems to perform exactly same. Thus it is recalculating when Calc. of Header is clicked.

            Do you want me to make these two separate?

            04/08/2016 James’s Reply :
            Yes please I think they need to be separated.*/

            //Line Items section's update
            if (gvLists.Rows.Count > 0)
            {
                decimal totQty, totVat, totVal;
                totQty = totVat = totVal = 0;

                TextBox txtQTY = new TextBox();
                TextBox txtPRICE = new TextBox();
                TextBox txtVATx = new TextBox();
                TextBox txtVALUE = new TextBox();

                string qtyStr = "";
                string prcStr = "";
                string vatStr = "";

                decimal qty = Convert.ToDecimal("0.00");//quantity
                decimal prc = Convert.ToDecimal("0.00");//unit price
                decimal vatx = Convert.ToDecimal("0.00");//VAT
                decimal val = Convert.ToDecimal("0.00");//value calculation

                foreach (GridViewRow GVR in gvLists.Rows)
                {
                    txtQTY = (TextBox)GVR.FindControl("txtQTY");
                    txtPRICE = (TextBox)GVR.FindControl("txtPRICE");
                    txtVATx = (TextBox)GVR.FindControl("txtVAT");
                    txtVALUE = (TextBox)GVR.FindControl("txtVALUE");

                    qtyStr = (string.IsNullOrEmpty(txtQTY.Text)) ? "" : txtQTY.Text;//quantity
                    prcStr = (string.IsNullOrEmpty(txtPRICE.Text)) ? "" : txtPRICE.Text;//unit price
                    vatStr = (string.IsNullOrEmpty(txtVATx.Text)) ? "" : txtVATx.Text;//VAT

                    qtyStr = qtyStr.Trim().Replace("&nbsp;", "").Replace(" ", "");//quantity
                    prcStr = prcStr.Trim().Replace("&nbsp;", "").Replace(" ", "");//unit price
                    vatStr = vatStr.Trim().Replace("&nbsp;", "").Replace(" ", "");//VAT

                    qtyStr = (qtyStr.Length == 1 && qtyStr == ".") ? "" : qtyStr;//quantity
                    prcStr = (prcStr.Length == 1 && prcStr == ".") ? "" : prcStr;//unit price
                    vatStr = (vatStr.Length == 1 && vatStr == ".") ? "" : vatStr;//VAT

                    qtyStr = (string.IsNullOrEmpty(qtyStr)) ? "0.00" : qtyStr;//quantity
                    prcStr = (string.IsNullOrEmpty(prcStr)) ? "0.00" : prcStr;//unit price
                    vatStr = (string.IsNullOrEmpty(vatStr)) ? "0.00" : vatStr;//VAT

                    qtyStr = Math.Round(Convert.ToDecimal(qtyStr), 2).ToString();//quantity round up
                    prcStr = Math.Round(Convert.ToDecimal(prcStr), 2).ToString();//unit price round up
                    vatStr = Math.Round(Convert.ToDecimal(vatStr), 2).ToString();//VAT round up

                    qtyStr = (qtyStr.Contains(".")) ? qtyStr : qtyStr + ".00";//quantity
                    prcStr = (prcStr.Contains(".")) ? prcStr : prcStr + ".00";//unit
                    vatStr = (vatStr.Contains(".")) ? vatStr : vatStr + ".00";//VAT

                    qtyStr = (qtyStr.Split('.')[1].Length < 2) ? qtyStr + "0" : qtyStr;//quantity
                    prcStr = (prcStr.Split('.')[1].Length < 2) ? prcStr + "0" : prcStr;//unit
                    vatStr = (vatStr.Split('.')[1].Length < 2) ? vatStr + "0" : vatStr;//VAT

                    qty = Convert.ToDecimal(qtyStr);//quantity
                    prc = Convert.ToDecimal(prcStr);//unit price
                    vatx = Convert.ToDecimal(vatStr);//VAT

                    val = (qty * prc) + vatx;//value calculation

                    txtQTY.Text = Math.Round(qty, 2).ToString();
                    txtPRICE.Text = Math.Round(prc, 2).ToString();
                    txtVATx.Text = Math.Round(vatx, 2).ToString();
                    txtVALUE.Text = Math.Round(val, 2).ToString();

                    totQty += qty;
                    totVat += vatx;
                    totVal += val;
                }

                gvLists.FooterRow.Cells[3].Text = Math.Round(totQty, 2).ToString();
                gvLists.FooterRow.Cells[6].Text = Math.Round(totVat, 2).ToString();
                gvLists.FooterRow.Cells[5].Text = Math.Round(totVal, 2).ToString();

                txtSum.Text = Math.Round(totVal, 2).ToString();

                //Update currency code
                //string curCode = DC.ReturnCurrencyCodeByID(Convert.ToInt32(ddlCurrency.SelectedValue));
                //lblAccCur.Text = curCode;
            }

            ReSetRedBorders();
        }


        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);
                DC.PopulateBatchType(CompanyID, ddlBatchType);
                //Response.Write("<br />ddlCompany.SelectedValue in ddlCompany_SelectedIndexChanged: " + ddlCompany.SelectedValue);
                //Response.Write("<br />lblDocID.Text in ddlCompany_SelectedIndexChanged: " + lblDocID.Text);

                int docID = Convert.ToInt32(lblDocID.Text);
                //Response.Write("<br />lblDocID.Text in ddlCompany_SelectedIndexChanged: " + lblDocID.Text);
                //Response.Write("<br />docID in ddlCompany_SelectedIndexChanged: " + docID);

                try
                {
                    string batchTypeID = DC.ReturnBatchTypeID(docID).ToString();
                    ddlBatchType.SelectedValue = batchTypeID;
                }
                catch
                {
                    ddlBatchType.SelectedValue = "0";
                }

                //Response.Write("<br />ddlBatchType.SelectedValue in ddlCompany_SelectedIndexChanged: " + ddlBatchType.SelectedValue);

                SetAccountsCurrency();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in ddlCompany_SelectedIndexChanged: " + ss;
            }

            if (IsPostBack)
                txtSupplier.Text = "Select Supplier";
        }


        protected void ddlBatchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Set text for use in javascript, if supplier is valid or not; for validation() function
            int BatchTypeID = Convert.ToInt32(ddlBatchType.SelectedValue);

            DataTable DT = DC.ReturnValidity_DT(BatchTypeID);

            if (DT.Rows.Count > 0)
            {
                DataRow DR = DT.Rows[0];
                txtIsLnItm.Text = DR["isLineItem"].ToString();
                txtIsPO.Text = DR["isPO"].ToString();
                txtIsDesc.Text = DR["isDescription"].ToString();

                txtIsLnItm.Text = (string.IsNullOrEmpty(txtIsLnItm.Text)) ? "False" : txtIsLnItm.Text;
                txtIsPO.Text = (string.IsNullOrEmpty(txtIsPO.Text)) ? "False" : txtIsPO.Text;
                txtIsDesc.Text = (string.IsNullOrEmpty(txtIsDesc.Text)) ? "False" : txtIsDesc.Text;
            }
            else
            {
                txtIsLnItm.Text = "False";
                txtIsPO.Text = "False";
                txtIsDesc.Text = "False";
            }

            //applicable for testing purpose
            if (test)
            {
                txtIsLnItm.Text = "True";
                txtIsPO.Text = "False";
                txtIsDesc.Text = "False";
            }
            #endregion

            #region Data from XML, Formating and Coloruing
            int comID = Convert.ToInt32(Request.QueryString["cid"]);
            //Response.Write("<br />comID in ddlCompany_SelectedIndexChanged: " + comID);

            int docID = Convert.ToInt32(lblDocID.Text);
            //Response.Write("<br />lblDocID.Text in ddlCompany_SelectedIndexChanged: " + lblDocID.Text);
            //Response.Write("<br />docID in ddlCompany_SelectedIndexChanged: " + docID);

            string xmlPath = "";
            if (test)
            {
                SaveTestXML(comID, docID);
                xmlPath = "C:\\P2D\\IPE Output\\DefaultCompany\\" + testXMLFileName;
                pnl1.CssClass = "shown";
            }
            else
            {
                string companyName = DC.ReturnCompanyNameByID(comID);
                //string companyName = ddlCompany.SelectedItem.Text;
                xmlPath = "D:\\P2D\\IPE Output\\" + companyName + "\\DOC" + lblDocID.Text + ".xml";
                pnl1.CssClass = "hidden";
            }

            //Response.Write("<br />xmlPath in ddlCompany_SelectedIndexChanged: " + xmlPath);

            //29-06-2016: when the action window loads, if there is no XML then the 
            //system automatically creates one
            if (!File.Exists(xmlPath))
            {
                try
                {
                    string emptyXML = Server.MapPath("..") + "\\ScanQC\\DOCempty.xml";
                    File.Copy(emptyXML, xmlPath);
                }
                catch
                {
                }
            }

            LoadHeaderSectionDataFromXML(xmlPath);

            LoadLineItemsSectionDataFromXML(xmlPath);

            ReSetRedBorders();
            #endregion

            #region Set text for use in javascript, if supplier is valid or not; for validation() function
            string Supplier = CommonFunctions.TrimSupplierName(txtSupplier.Text.Trim());
            int buyerID = Convert.ToInt32(ddlCompany.SelectedValue);
            Supplier = DC.ReturnValidSupplierCompanyBySupplierName(Supplier, buyerID);

            if (Supplier.Length > 0)
                txtValidSupplier.Text = true.ToString();
            if (Supplier.Length == 0)
                txtValidSupplier.Text = false.ToString();
            #endregion
        }

        decimal qtyT = 0;//Quantity Total
        decimal prcT = 0;//Rate/Price Total
        decimal vatT = 0;//VAT Total
        decimal valT = 0;//Value Total


        protected void gvLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow GVR = e.Row;

            string qtyS = "0";
            string prcS = "0";
            string vatS = "0";
            string valS = "0";

            TextBox txtQTY = new TextBox();
            TextBox txtPRICE = new TextBox();
            TextBox txtVATx = new TextBox();
            TextBox txtVALUE = new TextBox();

            if (GVR.RowType == DataControlRowType.DataRow)
            {
                txtQTY = (TextBox)GVR.FindControl("txtQTY");
                txtPRICE = (TextBox)GVR.FindControl("txtPRICE");
                txtVATx = (TextBox)GVR.FindControl("txtVAT");
                txtVALUE = (TextBox)GVR.FindControl("txtVALUE");

                qtyS = txtQTY.Text;
                prcS = txtPRICE.Text;
                vatS = txtVATx.Text;
                valS = txtVALUE.Text;

                qtyS = qtyS.Replace("&nbsp;", "");
                prcS = prcS.Replace("&nbsp;", "");
                vatS = vatS.Replace("&nbsp;", "");
                valS = valS.Replace("&nbsp;", "");

                qtyS = (qtyS.Length == 1 && qtyS == ".") ? "" : qtyS;
                prcS = (prcS.Length == 1 && prcS == ".") ? "" : prcS;
                vatS = (vatS.Length == 1 && vatS == ".") ? "" : vatS;
                valS = (valS.Length == 1 && valS == ".") ? "" : valS;

                qtyS = (string.IsNullOrEmpty(qtyS)) ? "0.00" : qtyS;
                prcS = (string.IsNullOrEmpty(prcS)) ? "0.00" : prcS;
                vatS = (string.IsNullOrEmpty(vatS)) ? "0.00" : vatS;
                valS = (string.IsNullOrEmpty(valS)) ? "0.00" : valS;

                qtyS = Math.Round(Convert.ToDecimal(qtyS), 2).ToString();//quantity round up
                prcS = Math.Round(Convert.ToDecimal(prcS), 2).ToString();//unit price round up
                vatS = Math.Round(Convert.ToDecimal(vatS), 2).ToString();//VAT round up
                valS = Math.Round(Convert.ToDecimal(valS), 2).ToString();//Value round up

                qtyS = (qtyS.Contains(".")) ? qtyS : qtyS + ".00";//quantity
                prcS = (prcS.Contains(".")) ? prcS : prcS + ".00";//unit price
                vatS = (vatS.Contains(".")) ? vatS : vatS + ".00";//VAT
                valS = (valS.Contains(".")) ? valS : valS + ".00";//Value
                //valS = ((Convert.ToDecimal(qtyS) * Convert.ToDecimal(prcS)) + Convert.ToDecimal(vatS)).ToString();

                txtVALUE.Text = Math.Round(Convert.ToDecimal(valS), 2).ToString();

                qtyT += Convert.ToDecimal(qtyS) * (decimal)1.0;
                prcT += Convert.ToDecimal(prcS) * (decimal)1.0;
                vatT += Convert.ToDecimal(vatS) * (decimal)1.0;
                valT += Convert.ToDecimal(valS) * (decimal)1.0;

                qtyT = Math.Round(qtyT, 2);
                prcT = Math.Round(prcT, 2);
                vatT = Math.Round(vatT, 2);
                valT = Math.Round(valT, 2);
            }

            /*
             * Changed on 29-July-2016 after instruction by James:
             * Pressing ADD LINE, DELETE LINE and SAVE also executes CALC. 
             * in the Line Items tab. Please stop this from happening. CALC. 
             * should only happen when CALC. is pressed. 
            */
            if (GVR.RowType == DataControlRowType.Footer)
            {
                GVR.Cells[3].Text = qtyT.ToString();
                //GVR.Cells[4].Text = prcT.ToString();
                GVR.Cells[6].Text = vatT.ToString();
                GVR.Cells[5].Text = valT.ToString();
            }

            txtSum.Text = valT.ToString();
        }


        protected void lbtnDnldPrev_Click(object sender, EventArgs e)
        {
            int comID = Convert.ToInt32(Request.QueryString["cid"]);
            int docID = Convert.ToInt32(Request.QueryString["did"]);

            int batchID = Convert.ToInt32(lblBatchID.Text);
            DataTable DT = DC.ReturnAllDOCIDsDTInBatch(batchID);

            int RowCount = 0;

            foreach (DataRow DR in DT.Rows)
            {
                string str = DR[0].ToString();

                RowCount++;

                if (str == docID.ToString())
                    break;
            }

            if (RowCount == 1)
            {
                string msg = "This is the 1st image in the batch";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + msg + "');", true);
                //Response.Write("<script>alert('" + msg + "');</script>");
            }
            else
            {
                docID = Convert.ToInt32(DT.Rows[RowCount - 2][0].ToString());

                DownloadImage(comID, docID);
            }
        }


        protected void lbtnDnldThis_Click(object sender, EventArgs e)
        {
            int comID = Convert.ToInt32(Request.QueryString["cid"]);
            int docID = Convert.ToInt32(Request.QueryString["did"]);

            DownloadImage(comID, docID);
        }


        protected void lbtnDnldNext_Click(object sender, EventArgs e)
        {
            int comID = Convert.ToInt32(Request.QueryString["cid"]);
            int docID = Convert.ToInt32(Request.QueryString["did"]);

            int batchID = Convert.ToInt32(lblBatchID.Text);
            DataTable DT = DC.ReturnAllDOCIDsDTInBatch(batchID);

            int RowCount = 0;

            foreach (DataRow DR in DT.Rows)
            {
                string str = DR[0].ToString();

                RowCount++;

                if (str == docID.ToString())
                    break;
            }

            if (RowCount < DT.Rows.Count)
            {
                docID = Convert.ToInt32(DT.Rows[RowCount][0].ToString());

                DownloadImage(comID, docID);
            }
            else
            {
                string msg = "This is the last image in the batch";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('" + msg + "');", true);
                //Response.Write("<script>alert('" + msg + "');</script>");
            }
        }


        protected void lbtnOriginalDocument_Click(object sender, EventArgs e)
        {
            try
            {
                string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceURL"];
                CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                objService.Url = serviceUrl;

                string[] arr = { "pdf", "tif", "jpeg", "png", "jpg" };

                string companyName = ddlCompany.SelectedItem.Text;

                string fileName = lblFileName.Text;
                int i = fileName.Split('.').Length;
                string fileExt = fileName.Split('.')[i - 1];
                fileName = fileName.Replace("." + fileExt, "");

                string originalName = "";
                string downloadURL = "";
                bool isFound = false;
                byte[] bytes = null;

                foreach (string str in arr)
                {
                    try
                    {
                        downloadURL = "//90104-server2/Email Processing Archive/" + companyName + "/";
                        //downloadURL = "C:/P2D/Email Processing Archive/" + companyName + "/";

                        fileExt = str;
                        originalName = fileName + "." + fileExt;
                        downloadURL += originalName;
                        downloadURL = downloadURL.Replace("/", @"\");

                        //Response.Write("<br />downloadURL: " + downloadURL);

                        bytes = objService.DownloadFile(downloadURL);

                        if (bytes != null)
                        {
                            string tempFilePath = Server.MapPath("~") + "\\Temp\\" + originalName;
                            tempFilePath = tempFilePath.Replace("\\", @"\");

                            File.WriteAllBytes(tempFilePath, bytes);

                            //Response.Write("<br />" + tempFilePath);

                            DownloadFile(tempFilePath, originalName, fileExt);

                            isFound = true;

                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (!isFound)
                    this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('No file(s) found to download.')", true);
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in lbtnOriginalDocument_Click: " + ss;
            }
        }


        protected void lbtnAtcPrev_Click(object sender, EventArgs e)
        {
            int BatchID = Convert.ToInt32(lblBatchID.Text);
            int DocID = Convert.ToInt32(lblDocID.Text);
            string ret = DC.DocumentPosition(BatchID, DocID);

            if (ret == "First")
            {
                //if the document is first document of the batch then following message is shown.
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('This is the 1st image in the batch so it cannot be attached to the previous document.');", true);
            }
            else
            {
                //
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_conf", "ValildateDocument('prev');", true);
            }
        }


        protected void lbtnAtcNext_Click(object sender, EventArgs e)
        {
            int BatchID = Convert.ToInt32(lblBatchID.Text);
            int DocID = Convert.ToInt32(lblDocID.Text);
            string ret = DC.DocumentPosition(BatchID, DocID);

            if (ret == "Last")
            {
                //
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('This is the last image in the batch so it cannot be attached to the next document.');", true);
            }
            else
            {
                //
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_conf", "ValildateDocument('next');", true);
            }
        }


        protected void lbtnSplitAndReprocess_Click(object sender, EventArgs e)
        {
            string script = "PopupPage('SplitAndReprocess.aspx?cid=" + Request.QueryString["cid"] + "&did=" + Request.QueryString["did"] + "', 'SplitAndReprocess', 1200, 600, 'Link3');";
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_popup", script, true);
        }


        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            ReSetRedBorders();

            if (!IsPostBack && Session["isNewBatch"] != null)
            {
                Session.Remove("isNewBatch");

                string url = Request.Url.ToString();
                url = (url.Contains("ActionScanQCNew")) ? (url.Replace("ActionScanQCNew", "ActionWindow")) : (url.Replace("ActionScanQC", "ActionWindow"));

                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_showNewBatchMsg", "showNewBatchMsg('" + url + "')", true);
            }
        }
        #endregion

        #region Member Functions/Methods

        /// <summary>
        /// to check whether Final Archive Date of the Document Progress table is populated or not.
        /// </summary>
        /// <param name="docID"></param>
        /// <returns>returns true if not populated yet, otherwise false</returns>
        bool CheckFinalArchivingDate(int docID)
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                DataSet DS = new DataSet();

                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("SP_checkFinalArchivingDate", sqlConn);
                cmd.Parameters.AddWithValue("@DocID", docID);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(DS);
                sqlConn.Close();

                if (((DS.Tables[0].Rows[0]["FAD"]) as object == DBNull.Value) || string.IsNullOrEmpty(Convert.ToString(DS.Tables[0].Rows[0]["FAD"])))
                {
                    return true;
                }
                else
                {
                    return false;
                }

                //return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Load data in top section.
        /// </summary>
        /// <param name="docID">input docID as int.</param>
        void LoadTopData(int docID)
        {
            int c = 0;

            try
            {
                DataTable dt = DC.ReturnTopDataTable(docID);
                DataRow dr = null;
                c = dt.Rows.Count;

                // "<br />DT Row Count in LoadTopData: " + c);

                lblDocID.Text = docID.ToString();

                if (c > 0)
                {
                    dr = dt.Rows[0];

                    lblFileName.Text = dr["ORIGINAL NAME"].ToString();
                    lblBatchID.Text = dr["BATCH ID"].ToString();
                    lblBatchName.Text = dr["BATCH NAME"].ToString();
                }
                else
                {
                    lblFileName.Text = "";
                    lblBatchID.Text = "0";
                    lblBatchName.Text = "";
                }

                Session.Add("pBatchID", lblBatchID.Text);
                Session.Add("pDocID", lblDocID.Text);

                /*added on 03-June-2016*/
                /*
                If the batch name is prefixed ‘000’, add another link called ‘original document’ 
                */
                string[] arr = lblBatchName.Text.Split(';');
                string FstElmnt = arr.GetValue(0).ToString();

                if (!test)
                {
                    if (FstElmnt == "000")
                        lbtnOriginalDocument.Visible = true;
                    else
                        lbtnOriginalDocument.Visible = false;
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in LoadTopData: " + ss;
            }
            finally
            {
            }

            if (c <= 0)
            {
                //close the page
                string url = "ActionWindow.aspx";
                //Redirection(url);
            }
            else
            {
                DC.UpdateBeingEdited("YES", docID);
            }
        }


        /// <summary>
        /// Loads xml file's data in header section
        /// </summary>
        /// <param name="xmlFilePath">input xml file name as string.</param>
        void LoadHeaderSectionDataFromXML(string xmlFilePath)
        {
            try
            {
                if (File.Exists(xmlFilePath))
                {
                    XmlDocument xd = new XmlDocument();

                    xd.Load(xmlFilePath);

                    XmlNode n = xd.ChildNodes[1];

                    string val = "";

                    foreach (XmlNode xn in n.ChildNodes)
                    {
                        val = "";

                        switch (xn.Name)
                        {
                            case "BatchTypeId":
                                val = (xn.ChildNodes.Count > 0) ? xn.ChildNodes[0].Value : "";
                                if (val != null) val = val.Trim();
                                if (val != "" && !IsPostBack)
                                {
                                    if (ddlBatchType.Items.Contains(new ListItem(val)))
                                        ddlBatchType.SelectedValue = val;
                                }
                                break;
                            case "CompanyID":
                                val = (xn.ChildNodes.Count > 0) ? xn.ChildNodes[0].Value : "";
                                if (val != null) val = val.Trim();
                                if (val != "" && !IsPostBack)
                                {
                                    ddlCompany.SelectedValue = val;
                                    int CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);
                                    DC.PopulateBatchType(CompanyID, ddlBatchType);
                                }
                                break;
                            case "DocumentType"://7th item, index 6
                                val = (xn.ChildNodes.Count > 0) ? xn.ChildNodes[0].Value : "";
                                if (val != null) val = val.Trim();
                                if (val.ToLower().Contains("credit"))
                                    ddlDocType.SelectedValue = "CRN";
                                else
                                    ddlDocType.SelectedValue = "INV";
                                break;
                            case "Supplier"://8th item, index 7
                                val = (xn.ChildNodes.Count > 0) ? xn.ChildNodes[0].Value : "";
                                if (val != null) val = val.Trim();
                                if (string.IsNullOrEmpty(val) == true || val.ToUpper() == "NOT FOUND" || val.ToUpper() == "NOT_FOUND" || val.ToUpper() == "CANCELLED")
                                {
                                    txtSupplier.Text = "";
                                    txtSupplier.Style.Add("border-color", "red !important");
                                }
                                else
                                {
                                    string Supplier = val.Trim();
                                    int buyerID = Convert.ToInt32(ddlCompany.SelectedValue);
                                    string SupplierNamWithCode = DC.ReturnValidSupplierCompanyBySupplierName(Supplier, buyerID);
                                    string code = CommonFunctions.TrimSupplierCode(SupplierNamWithCode);
                                    string SupplierID = DC.ReturnSupplierID(Supplier, buyerID, code).ToString();

                                    if (Supplier.Length > 0)
                                    {
                                        txtSupplier.Text = SupplierNamWithCode;
                                        hfSupplierID.Value = SupplierID;
                                        txtSupplier.Style.Add("border-color", "#ccc !important");
                                    }
                                    else
                                    {
                                        txtSupplier.Style.Add("border-color", "red !important");
                                        hfSupplierID.Value = "";
                                    }
                                }
                                break;
                            case "Date"://9th item, index 8
                                val = (xn.ChildNodes.Count > 0) ? xn.ChildNodes[0].Value : "";
                                if (val != null) val = val.Trim();

                                if (string.IsNullOrEmpty(val) == true || val.ToUpper() == "NOT FOUND" || val.ToUpper() == "NOT_FOUND" || val.ToUpper() == "CANCELLED")
                                {
                                    txtDocDate.Text = "";
                                    txtDocDate.Style.Add("border-color", "red !important");
                                }
                                else
                                {
                                    txtDocDate.Text = val;
                                    txtDocDate.Style.Add("border-color", "#ccc !important");
                                }
                                break;
                            case "InvoiceNumber"://11th item, index 10
                                val = (xn.ChildNodes.Count > 0) ? xn.ChildNodes[0].Value : "";
                                if (val != null) val = val.Trim();

                                if (string.IsNullOrEmpty(val) == true || val.ToUpper() == "NOT FOUND" || val.ToUpper() == "NOT_FOUND" || val.ToUpper() == "CANCELLED")
                                {
                                    txtDocNumber.Text = "";
                                    txtDocNumber.Style.Add("border-color", "red !important");
                                }
                                else
                                {
                                    txtDocNumber.Text = val;
                                    txtDocNumber.Style.Add("border-color", "#ccc !important");
                                }
                                break;
                            case "PurchaseOrder"://12th item, index 11
                                val = (xn.ChildNodes.Count > 0) ? xn.ChildNodes[0].Value : "";
                                if (val != null) val = val.Trim();
                                txtPONumber.Text = val;
                                if (string.IsNullOrEmpty(val) == false)
                                {
                                    string pono = txtPONumber.Text;

                                    DataTable DT = DC.ReturnSupplierDataTableByPONumber(pono);

                                    if (DT.Rows.Count == 1)
                                    {
                                        txtPOSupplier.Text = DT.Rows[0][0].ToString();
                                    }
                                }
                                else
                                {
                                    txtPOSupplier.Text = "";
                                }
                                if (string.IsNullOrEmpty(val) == true || val.ToUpper() == "NOT FOUND" || val.ToUpper() == "NOT_FOUND" || val.ToUpper() == "CANCELLED")
                                {
                                    txtPONumber.Text = "";
                                    if (txtIsPO.Text.ToLower() == "true")
                                    {
                                        txtPONumber.Style.Add("border-color", "red !important");
                                    }
                                    else
                                    {
                                        txtPONumber.Style.Add("border-color", "#cccccc !important");
                                    }
                                }
                                break;
                            case "Currency"://14th item, index 13
                                val = (xn.ChildNodes.Count > 0) ? xn.ChildNodes[0].Value : "";
                                if (val != null) val = val.Trim();
                                if (val.ToUpper() == "CANCELLED" || val.ToUpper() == "NOT FOUND" || val.ToUpper() == "NOT_FOUND")
                                {
                                    val = "";
                                }
                                else
                                {
                                    try
                                    {
                                        val = DC.ReturnCurrencyIDByCode(val).ToString();
                                    }
                                    catch
                                    {
                                        val = "0";
                                    }
                                }
                                ddlCurrency.SelectedValue = val;
                                if (val == "0")
                                    ddlCurrency.Style.Add("border-color", "red !important");
                                else
                                    ddlCurrency.Style.Add("border-color", "#ccc !important");
                                break;
                            case "NOTES"://19th item, index 18
                                foreach (XmlNode cn in xn.ChildNodes)
                                {
                                    switch (cn.Name)
                                    {
                                        case "InvoiceSubtotal":
                                            val = (cn.ChildNodes.Count > 0) ? cn.ChildNodes[0].Value : "";
                                            if (val != null) val = val.Trim();

                                            if (string.IsNullOrEmpty(val) == true || val.ToUpper() == "NOT FOUND" || val.ToUpper() == "NOT_FOUND" || val.ToUpper() == "CANCELLED")
                                            {
                                                txtNet.Text = "";
                                                txtNet.Style.Add("border-color", "red !important");
                                            }
                                            else
                                            {
                                                txtNet.Text = val;
                                                txtNet.Style.Add("border-color", "#ccc !important");
                                            }
                                            if (val == "0")
                                                txtNet.Text = "0.00";
                                            break;
                                        case "InvoiceVatTotal":
                                            val = (cn.ChildNodes.Count > 0) ? cn.ChildNodes[0].Value : "";
                                            if (val != null) val = val.Trim();

                                            if (string.IsNullOrEmpty(val) == true || val.ToUpper() == "NOT FOUND" || val.ToUpper() == "NOT_FOUND" || val.ToUpper() == "CANCELLED")
                                            {
                                                txtVAT.Text = "";
                                                txtVAT.Style.Add("border-color", "red !important");
                                            }
                                            else
                                            {
                                                txtVAT.Text = val;
                                                txtVAT.Style.Add("border-color", "#ccc !important");
                                            }
                                            if (val == "0")
                                                txtVAT.Text = "0.00";
                                            break;
                                        case "InvoiceTotal":
                                            val = (cn.ChildNodes.Count > 0) ? cn.ChildNodes[0].Value : "";
                                            if (val != null) val = val.Trim();

                                            if (string.IsNullOrEmpty(val) == true || val.ToUpper() == "NOT FOUND" || val.ToUpper() == "NOT_FOUND" || val.ToUpper() == "CANCELLED")
                                            {
                                                txtTotal.Text = "";
                                                txtTotal.Style.Add("border-color", "red !important");
                                            }
                                            else
                                            {
                                                txtTotal.Text = val;
                                                txtTotal.Style.Add("border-color", "#ccc !important");
                                            }
                                            if (val == "0")
                                                txtTotal.Text = "0.00";
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in LoadHeaderSectionDataFromXML: " + ss;
            }
            finally { }

            SetAccountsCurrency();
        }


        /// <summary>
        /// Set's the currency value in a field for further checking.
        /// </summary>
        void SetAccountsCurrency()
        {
            string supplierName = CommonFunctions.TrimSupplierName(txtSupplier.Text);
            string code = CommonFunctions.TrimSupplierCode(txtSupplier.Text);

            int BuyerID = Convert.ToInt32(ddlCompany.SelectedValue);
            int SupplierID = DC.ReturnSupplierID(supplierName, BuyerID, code);
            string Old_CurrencyCode = ddlCurrency.SelectedItem.Text;
            string New_CurrencyCode = DC.ReturnNewCurrencyCode(SupplierID, BuyerID);

            lblAccCur.Text = New_CurrencyCode;

            if (Old_CurrencyCode.Contains(New_CurrencyCode))
                lblAccCur.CssClass = "cur-same";
            else
                lblAccCur.CssClass = "cur-diff";
        }

        /// <summary>
        /// Loads List Items section data from xml file
        /// </summary>
        /// <param name="xmlFilePath">input xml file name as string.</param>
        void LoadLineItemsSectionDataFromXML(string xmlFilePath)
        {
            try
            {
                XmlDocument xd = new XmlDocument();

                if (File.Exists(xmlFilePath))
                {
                    xd.Load(xmlFilePath);

                    XmlNode xnInvoice = xd.ChildNodes[1];
                    XmlNode xnResolution = null;

                    foreach (XmlNode xn in xnInvoice.ChildNodes)
                    {
                        if (xn.Name == "Resolution")
                        {
                            xnResolution = xn;
                            break;
                        }
                    }

                    #region DATATABLE
                    DataTable DT = new DataTable("LineItems");

                    DT.Columns.Add("PONO");
                    DT.Columns.Add("BUYERCODE");
                    DT.Columns.Add("DESC");
                    DT.Columns.Add("QTY");
                    DT.Columns.Add("PRICE");
                    DT.Columns.Add("VAT");
                    DT.Columns.Add("VALUE");
                    DT.Columns.Add("POS");//position
                    DT.Columns.Add("PAGE");//page
                    DT.Columns.Add("GOODSRECDDETAILID");
                    DT.Columns.Add("DEPARTMENTID");
                    DT.Columns.Add("NOMINALCODEID");
                    DT.Columns.Add("BUSINESSUNITID");
                    DT.Columns.Add("PROJECTCODE"); //Added by Mainak 2017-11-21
                    DT.Columns.Add("PURORDERLINENO"); //Added by Mainak 2018-05-31
                    #endregion

                    if (xnResolution != null)
                    {
                        string val = "";
                        int i = 0;

                        foreach (XmlNode xnPage in xnResolution.ChildNodes)
                        {
                            foreach (XmlNode xn in xnPage.ChildNodes)
                            {
                                DataRow DR = DT.NewRow();
                                string pos = "";

                                if (xn.Name == "ItemDetail")
                                {
                                    foreach (XmlNode xn1 in xn.ChildNodes)
                                    {
                                        val = "";
                                        switch (xn1.Name)
                                        {
                                            case "ItemPoNumber":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "" : val;
                                                DR["PONO"] = val.Trim();
                                                //position
                                                pos += (xn1.ChildNodes.Count > 1) ? xn1.ChildNodes[1].InnerText : "NOT FOUND";
                                                break;
                                            case "ItemDescription":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "" : val;
                                                DR["DESC"] = val.Trim();
                                                break;
                                            case "ItemQty":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "0" : val;
                                                DR["QTY"] = val.Trim();
                                                break;
                                            case "ItemUnitPrice":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "0.00" : val;
                                                DR["PRICE"] = val.Trim();
                                                break;
                                            case "ItemVat":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "0.00" : val;
                                                DR["VAT"] = val.Trim();
                                                break;
                                            case "ItemValue":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "0.00" : val;
                                                DR["VALUE"] = val.Trim();
                                                //position
                                                pos += (xn1.ChildNodes.Count > 1) ? xn1.ChildNodes[1].InnerText + "/" : "NOT FOUND/";
                                                break;
                                            case "BuyerCode":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "0.00" : val;
                                                DR["BUYERCODE"] = val.Trim();
                                                break;
                                            case "GoodsRecdDetailID":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "" : val;
                                                DR["GOODSRECDDETAILID"] = val.Trim();
                                                break;
                                            case "DepartmentID":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "" : val;
                                                DR["DEPARTMENTID"] = val.Trim();
                                                break;
                                            case "NominalCodeID":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "" : val;
                                                DR["NOMINALCODEID"] = val.Trim();
                                                break;
                                            case "BusinessUnitID":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "" : val;
                                                DR["BUSINESSUNITID"] = val.Trim();
                                                break;
                                            //Added by Mainak 2017-11-21
                                            case "ProjectCode":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "" : val;
                                                DR["PROJECTCODE"] = val.Trim();
                                                break;
                                            //Added by Mainak 2018-05-31
                                            case "PurOrderLineNo":
                                                val = (xn1.ChildNodes.Count > 0) ? xn1.ChildNodes[0].Value : "";
                                                val = (string.IsNullOrEmpty(val) == true) ? "" : val;
                                                DR["PURORDERLINENO"] = val.Trim();
                                                break;
                                        }
                                    }
                                    //position value
                                    DR["POS"] = pos;
                                    try
                                    {
                                        DR["PAGE"] = xnPage.Attributes["Number"].InnerText;
                                    }
                                    catch
                                    {
                                        DR["PAGE"] = xnPage.Attributes["number"].InnerText;
                                    }
                                }

                                DT.Rows.InsertAt(DR, i);

                                i++;
                            }
                        }
                    }

                    XmlNode xnPage1 = null;

                    if (xnResolution != null)
                        xnPage1 = (xnResolution.ChildNodes.Count > 0) ? xnResolution.ChildNodes[0] : null;

                    if (xnPage1 == null || DT.Rows.Count == 0)
                    {
                        DT.Rows.Add("", "", "", "", "", "", "", "NOT FOUND/NOT FOUND", "0", "", "", "", "", "", "");//Modified by Mainak 2018-05-31
                    }

                    ViewState["LineItemsDT"] = DT;

                    gvLists.DataSource = DT;
                    //Added By Mainak 31.12.2018
                    hdnPurOrderNo.Value = DT.Rows[0]["PONO"].ToString();

                    gvLists.DataBind();

                    //Check Page_LoadComplete() function for "Not found" and blank checking
                }
                else
                {
                    DataTable DT = new DataTable("LineItems");

                    DT.Columns.Add("PONO");
                    DT.Columns.Add("BUYERCODE");
                    DT.Columns.Add("DESC");
                    DT.Columns.Add("QTY");
                    DT.Columns.Add("PRICE");
                    DT.Columns.Add("VAT");
                    DT.Columns.Add("VALUE");
                    DT.Columns.Add("POS");//position
                    DT.Columns.Add("PAGE");//page
                    DT.Columns.Add("GOODSRECDDETAILID");
                    DT.Columns.Add("DEPARTMENTID");
                    DT.Columns.Add("NOMINALCODEID");
                    DT.Columns.Add("BUSINESSUNITID");
                    DT.Columns.Add("PROJECTCODE");//Added by Mainak 2017-11-21
                    DT.Columns.Add("PURORDERLINENO");//Added by Mainak 2018-05-31

                    DT.Rows.Add("", "", "", "", "", "", "", "NOT FOUND", "0", "", "", "", "", "", "");//Modified by Mainak 2018-05-31

                    ViewState["LineItemsDT"] = DT;
                    //Added By Mainak 31.12.2018
                    hdnPurOrderNo.Value = DT.Rows[0]["PONO"].ToString();
                    gvLists.DataSource = DT;
                    gvLists.DataBind();
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in LoadLineItemsSectionDataFromXML: " + ss;
            }
            finally { }
        }

        /// <summary>
        /// Loads the next document id of current or first document id of next batch id.
        /// </summary>
        /// <param name="isFullRecord">Boolean type; true: does not check [BEING EDITED] false: checks whether [BEING EDITED] is 'NO'.</param>
        void LoadNext(DataTable DT)
        {
            try
            {
                /*g.If the batch is accessed from the details page then the window
              should close after processing only that batch, and not move onto the next batch in the list.
                i.If a specific invoice is selected in the details page:
                    1.Launch error message “This invoice has already been processed” if Status = Deleted or Processed
                    2.The action window should close after processing only that invoice, and not move onto the next available invoice in the list. */

                bool IsSingleDoc = true;
                bool isNextDoc = true;
                bool IsSingleBatch = false;

                string docID = lblDocID.Text;
                string comID = Convert.ToString(Request.QueryString["cid"]);

                bool tf = DC.UpdateBeingEdited("NO", Convert.ToInt32(docID));

                string url = "";

                if (tf)
                {
                    //is this is a single batch only?
                    if (Session["IsSingleBatch"] != null)
                        IsSingleBatch = (bool)Session["IsSingleBatch"];

                    //is this is a single document only?
                    if (Session["IsSingleDoc"] != null)
                        IsSingleDoc = (bool)Session["IsSingleDoc"];

                    /*
                     * after 1st April 2016
                     * Yes it was not clear in the original requirements. After performing an 
                     * action (like DISCARD, PROCESS, DELETE, SAVE) the next invoice in Pending 
                     * status where BEING PROCESSED? = NO should automatically load
                     * The sequencing is not working properly for the action window. 
                     * For the batch shown below I would expect doc 1773265 to load, 
                     * then 1773330, then 1773331, then 1773332, then 1773336 
                     * (i.e. the 5 docs in Pending status)
                     * thus above codition is not needed.
                     */

                    // "<br /> IsSingleDoc: " + IsSingleDoc);

                    //Line 1488 to 1494 and 
                    //

                    //if (IsSingleDoc)
                    //{
                    ////closes the ActionWindow after that invoice is processed.
                    //url = "ActionWindow.aspx";
                    //}
                    //else if (!IsSingleDoc)
                    //{
                    //actually loads next document
                    int batchID = Convert.ToInt32(lblBatchID.Text);

                    //The DT is required as parameter because the current docID is getting removed from list of ids
                    //well before the list of id is populated here. Thus the current doc id will not be available here
                    //anymore when searched. With isFullRecord all the doc ids are listed here.

                    if (DT == null)
                        DT = DC.ReturnDOCIDs_DT(batchID, true);

                    //Response.Write(DT.Rows.Count);
                    //return;

                    //count the number of rows from DocID's table till it matches with the current document id.
                    int RowCount = 0;

                    foreach (DataRow DR in DT.Rows)
                    {
                        string str = DR[0].ToString();

                        RowCount++;

                        if (str == docID.ToString())
                            break;
                    }

                    //if RowCount = DT.Rows.Count that means it is last document of the batch.
                    if (RowCount == DT.Rows.Count)
                    {
                        isNextDoc = false;

                        //when requested from CompanyBatches.aspx page the session will not be null
                        //if not null then next batch will be loaded upon the completion of action of
                        //current batch's all documents.
                        // and this is not for the single batch
                        //Response.Write(IsSingleBatch);
                        if (Session["BatchIDList"] != null && IsSingleBatch == false)
                        {
                            List<int[]> BatchIDList = (List<int[]>)Session["BatchIDList"];

                            int NextBatchIndex = 0;
                            int c = BatchIDList.Count;

                            for (int j = 0; j < c; j++)
                            {
                                int[] arr = BatchIDList[j];
                                NextBatchIndex = j + 1;

                                if (arr[0] == batchID)
                                    break;
                            }

                            for (int j = NextBatchIndex; j < c; j++)
                            {
                                int[] arr = BatchIDList[j];
                                NextBatchIndex = j;

                                batchID = arr[0];//get batch id
                                docID = arr[1].ToString();//get the first doc id of the above selected batch

                                DT = DC.ReturnDOCIDs_DT(batchID, false);

                                if (DT.Rows.Count == 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            //if NextBatchIndex is smaller than count of BatchIDs in the BatchIDList array
                            if (NextBatchIndex < BatchIDList.Count)
                            {
                                isNextDoc = true;

                                if (DT != null && DT.Rows.Count > 0)
                                {
                                    docID = DT.Rows[0][0].ToString();//value of first row and first column.
                                }
                                else
                                {
                                    docID = null;
                                }

                                docID = (string.IsNullOrEmpty(docID)) ? "0" : docID;

                                if (Convert.ToInt32(docID) > 0)
                                {
                                    //this session is used on page loade complete to fire a script to show
                                    //new batch message.
                                    Session["isNewBatch"] = true;

                                    url = "ActionWindow.aspx?cid=" + comID + "&did=" + docID + "&winH=" + Convert.ToString(Request.QueryString["winH"]);
                                }
                                else
                                {
                                    this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('All batches are completed.')", true);

                                    url = "ActionWindow.aspx";
                                }
                            }
                            if (NextBatchIndex == BatchIDList.Count)
                            {
                                isNextDoc = false;
                                //close action window
                                this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('All batches are completed.')", true);
                                url = "ActionWindow.aspx";
                            }
                        }
                        else
                        {
                            isNextDoc = false;

                            //when requested from SingleBatchDetails.aspx the session will be null, 
                            //which will close the ActionWindow
                            this.ClientScript.RegisterStartupScript(this.GetType(), "_msg", "alert('This batch has been completed.')", true);
                            url = "ActionWindow.aspx";
                        }
                    }
                    else
                    {
                        isNextDoc = true;
                        //reload action window
                        docID = DT.Rows[RowCount][0].ToString();
                        url = "ActionWindow.aspx?cid=" + comID + "&did=" + docID + "&winH=" + Convert.ToString(Request.QueryString["winH"]);
                    }
                    //}
                }
                else
                {
                    isNextDoc = true;
                    //reload action window
                    url = "ActionWindow.aspx?cid=" + comID + "&did=" + docID + "&winH=" + Convert.ToString(Request.QueryString["winH"]);
                }

                //is next document requested?
                if (Session["IsNextDoc"] != null)
                    Session.Remove("IsNextDoc");

                Session["IsNextDoc"] = isNextDoc;

                Redirection(url);
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in LoadNext: " + ss;
            }
        }

        /// <summary>
        /// Delete related files that is downloaded.
        /// </summary>
        /// <returns></returns>
        bool DeleteRelatedFiles()
        {
            //code to Delete files from D:\P2D\IPE Output\CompanyName that relate to this DOC ID i.e. DOC123456... (e.g. DOC123456.xml, doc123456.txt, doc123456_1.gif, doc123456_2.gif etc.)
            //DirectoryPath = "C:\\P2D\\IPE Output\\DefaultCompany\\";
            string DirectoryPath = "";
            bool TF = false;

            if (ddlCompany.SelectedItem.Text == "")
                return false;

            int comID = Convert.ToInt32(Request.QueryString["cid"]);
            string CompanyName = DC.ReturnCompanyNameByID(comID);
            //string CompanyName = ddlCompany.SelectedItem.Text;

            DirectoryPath = "D:\\P2D\\IPE Output\\" + CompanyName + "\\";

            //Response.Write("<br />" + DirectoryPath);

            try
            {
                //Directory.Delete(DirectoryPath, true);
                DirectoryInfo di = new DirectoryInfo(DirectoryPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Name.Contains(lblDocID.Text))
                        File.Delete(fi.FullName);
                }
                foreach (DirectoryInfo di1 in di.GetDirectories())
                {
                    if (di1.Name.Contains(lblDocID.Text))
                        Directory.Delete(di1.FullName);
                }
                TF = true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in DeleteRelatedFiles: " + ss;
            }

            return TF;
        }

        /// <summary>
        /// Save data to an XML file when process button is clicked
        /// </summary>
        /// <returns></returns>



        public void ErrorLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName, true);
            sw.WriteLine(DateTime.Now + ": " + sErrMsg);
            sw.Flush();
            sw.Close();
        }


        bool SaveDataToXMLWhenProcessed()
        {
            //code to save in xml file

            bool tf = false;
            string xmlDir = "";

            string COMPANY_NAME = DC.ReturnCompanyNameByBatchID(Convert.ToInt32(lblBatchID.Text));
            string BATCH_NAME = lblBatchName.Text;
            string ORIGINAL_NAME = lblFileName.Text;
            string BatchName_BatchID = lblBatchName.Text + "_" + lblBatchID.Text;

            if (test)
                xmlDir = "C:\\P2D\\IPE Archive\\" + ddlCompany.SelectedItem.Text + "\\" + BatchName_BatchID;
            else
                xmlDir = "D:\\P2D\\IPE Archive\\" + ddlCompany.SelectedItem.Text + "\\" + BatchName_BatchID;

            if (!Directory.Exists(xmlDir))
                Directory.CreateDirectory(xmlDir);

            string xmlPath = xmlDir + "\\DOC" + lblDocID.Text + ".xml";

            if (File.Exists(xmlPath))
                File.Delete(xmlPath);

            XmlDocument xd = new XmlDocument();

            try
            {
                #region XML Declaration
                XmlNode xmlDec = xd.CreateXmlDeclaration("1.0", "UTF-8", "");
                xd.AppendChild(xmlDec);
                #endregion

                #region Root (P2DXML) Element
                XmlNode P2DXML = xd.CreateElement("P2DXML");
                XmlAttribute Documents = xd.CreateAttribute("Documents");
                Documents.Value = "1";
                P2DXML.Attributes.Append(Documents);
                XmlAttribute Version = xd.CreateAttribute("Version");
                Version.Value = "1.05.67";
                P2DXML.Attributes.Append(Version);
                xd.AppendChild(P2DXML);
                #endregion

                #region Sender Element
                string supplier = CommonFunctions.TrimSupplierName(txtSupplier.Text);
                string code = CommonFunctions.TrimSupplierCode(txtSupplier.Text);
                int buyerID = Convert.ToInt32(ddlCompany.SelectedValue);

                XmlNode Sender = xd.CreateElement("Sender");
                XmlAttribute Documents1 = xd.CreateAttribute("Documents");
                Documents1.Value = "1";
                Sender.Attributes.Append(Documents1);
                XmlAttribute CompanyName = xd.CreateAttribute("CompanyName");
                CompanyName.Value = supplier;// CommonFunctions.ReturnParsedStringForXML(supplier);<-this is not required when a node is appended
                Sender.Attributes.Append(CompanyName);
                XmlAttribute HubId = xd.CreateAttribute("HubId");
                HubId.Value = DC.ReturnSupplierID(supplier, buyerID, code).ToString();//supplier id is the hub id
                Sender.Attributes.Append(HubId);
                P2DXML.AppendChild(Sender);
                #endregion

                #region Recipient Element
                XmlNode Recipient = xd.CreateElement("Recipient");
                XmlAttribute Documents2 = xd.CreateAttribute("Documents");
                Documents2.Value = "1";
                Recipient.Attributes.Append(Documents2);
                XmlAttribute HubId1 = xd.CreateAttribute("HubId");
                HubId1.Value = ddlCompany.SelectedItem.Value;
                Recipient.Attributes.Append(HubId1);
                Sender.AppendChild(Recipient);
                #endregion

                #region Document Element
                XmlNode Document = xd.CreateElement("Document");
                XmlAttribute Type = xd.CreateAttribute("Type");
                string DocType = (ddlDocType.SelectedValue.ToString() == "INV") ? "Sales Invoice" : "Sales Credit";
                Type.Value = DocType;//ddlDocType.SelectedItem.Text;
                Document.Attributes.Append(Type);
                XmlAttribute Transactions = xd.CreateAttribute("Transactions");
                Transactions.Value = gvLists.Rows.Count.ToString();
                Document.Attributes.Append(Transactions);
                Recipient.AppendChild(Document);
                #endregion

                //Under Document
                #region ImagePath Element
                XmlNode ImagePath = xd.CreateElement("ImagePath");
                ImagePath.InnerText = @"\\90104-server2\FTP Upload\" + COMPANY_NAME + @"\" + BATCH_NAME + @"\" + ORIGINAL_NAME;
                Document.AppendChild(ImagePath);
                #endregion

                #region ArchiveImagePath Element
                XmlNode ArchiveImagePath = xd.CreateElement("ArchiveImagePath");
                ArchiveImagePath.InnerText = @"\\90107-server3\FTP Archive\" + COMPANY_NAME + @"\" + BatchName_BatchID + @"\" + ORIGINAL_NAME;
                Document.AppendChild(ArchiveImagePath);
                #endregion


                //Modified by Mainak 2017-11-21
                #region Type Element
                XmlNode Type1 = xd.CreateElement("Type");
                Type1.InnerText = DC.ReturnBatchDocTypeValue(BatchTypeID: Convert.ToInt32(ddlBatchType.SelectedValue), BuyerCompanyID: ddlCompany.SelectedValue, OrderNo: txtPONumber.Text);
                Document.AppendChild(Type1);
                #endregion

                #region InvoiceNumber Element
                XmlNode InvoiceNumber = xd.CreateElement("InvoiceNumber");
                InvoiceNumber.InnerText = txtDocNumber.Text;
                Document.AppendChild(InvoiceNumber);
                #endregion

                #region InvoiceDate Element
                XmlNode InvoiceDate = xd.CreateElement("InvoiceDate");
                InvoiceDate.InnerText = txtDocDate.Text;
                Document.AppendChild(InvoiceDate);
                #endregion

                #region InvoiceTaxPointDate Element
                XmlNode InvoiceTaxPointDate = xd.CreateElement("InvoiceTaxPointDate");
                InvoiceTaxPointDate.InnerText = txtDocDate.Text;
                Document.AppendChild(InvoiceTaxPointDate);
                #endregion

                #region CurrencyCode Element
                XmlNode CurrencyCode = xd.CreateElement("CurrencyCode");
                CurrencyCode.InnerText = ddlCurrency.SelectedItem.Text;
                Document.AppendChild(CurrencyCode);
                #endregion

                #region NetAmount Element
                XmlNode NetAmount = xd.CreateElement("NetAmount");
                NetAmount.InnerText = string.IsNullOrEmpty(txtNet.Text) ? "0" : txtNet.Text;
                Document.AppendChild(NetAmount);
                #endregion

                #region TaxAmount Element
                XmlNode TaxAmount = xd.CreateElement("TaxAmount");
                TaxAmount.InnerText = string.IsNullOrEmpty(txtVAT.Text) ? "0" : txtVAT.Text;
                Document.AppendChild(TaxAmount);
                #endregion

                #region GrossAmount Element
                XmlNode GrossAmount = xd.CreateElement("GrossAmount");
                GrossAmount.InnerText = string.IsNullOrEmpty(txtTotal.Text) ? "0" : txtTotal.Text;
                Document.AppendChild(GrossAmount);
                #endregion

                //Under Document

                txtNet.Text = (string.IsNullOrEmpty(txtNet.Text)) ? "0" : txtNet.Text;
                txtSum.Text = (string.IsNullOrEmpty(txtSum.Text)) ? "0" : txtSum.Text;

                if (Convert.ToDecimal(txtNet.Text) == Convert.ToDecimal(txtSum.Text) || txtIsLnItm.Text == true.ToString())
                {
                    #region Transaction Element (If ‘Net’ on Header web page tab equals sum of ‘Value’ fields in Line Items; OR LineItems = TRUE)
                    TextBox txtPONO = new TextBox();
                    TextBox txtBuyerCode = new TextBox();
                    TextBox txtDescription = new TextBox();
                    TextBox txtQTY = new TextBox();
                    TextBox txtPRICE = new TextBox();
                    TextBox txtVALUE = new TextBox();
                    TextBox txtVATx = new TextBox();
                    Label lblGoodsRecdDetailID = new Label();
                    Label lblDeptID = new Label();
                    Label lblNominalCodeID = new Label();
                    Label lblBusinessUnitID = new Label();
                    Label lblProjectCode = new Label();
                    Label lblPurOrderLineNo = new Label();// Added by Mainak 2018-05-31


                    foreach (GridViewRow GVR in gvLists.Rows)
                    {
                        XmlNode Transaction = xd.CreateElement("Transaction");
                        Document.AppendChild(Transaction);

                        //Under Transaction
                        XmlNode LineNo = xd.CreateElement("LineNo");
                        LineNo.InnerXml = (GVR.RowIndex + 1).ToString();
                        Transaction.AppendChild(LineNo);

                        XmlNode OrderNo = xd.CreateElement("OrderNo");
                        txtPONO = (TextBox)GVR.FindControl("txtPONO");
                        string pono = txtPONO.Text.Trim().Replace("&nbsp;", "");
                        pono = ((pono.Length == 0) ? txtPONumber.Text : pono);
                        pono = pono.Replace("cancelled", "").Replace("not found", "").Replace("not_found", "").Replace("CANCELLED", "").Replace("NOT FOUND", "").Replace("NOT_FOUND", "");
                        OrderNo.InnerXml = (pono.Contains(";") ? "" : pono);
                        Transaction.AppendChild(OrderNo);

                        XmlNode buyerCode = xd.CreateElement("BuyerCode");
                        txtBuyerCode = (TextBox)GVR.FindControl("txtBuyerCode");
                        string BC = txtBuyerCode.Text.Trim().Replace("&nbsp;", "");
                        BC = CommonFunctions.ReturnParsedStringForXML(BC);
                        buyerCode.InnerXml = BC;
                        Transaction.AppendChild(buyerCode);

                        XmlNode Description = xd.CreateElement("Description");
                        txtDescription = (TextBox)GVR.FindControl("txtDescription");
                        string desc = txtDescription.Text.Trim().Replace("&nbsp;", "");
                        desc = CommonFunctions.ReturnParsedStringForXML(desc);
                        Description.InnerXml = desc;
                        Transaction.AppendChild(Description);

                        XmlNode Quantity = xd.CreateElement("Quantity");
                        txtQTY = (TextBox)GVR.FindControl("txtQTY");
                        string qty = txtQTY.Text.Trim().Replace("&nbsp;", "");
                        qty = ((qty.Length == 0) ? "0.00" : qty);
                        Quantity.InnerXml = qty;
                        Transaction.AppendChild(Quantity);

                        XmlNode Price = xd.CreateElement("Price");
                        txtPRICE = (TextBox)GVR.FindControl("txtPRICE");
                        string price = txtPRICE.Text.Trim().Replace("&nbsp;", "");
                        price = ((price.Length == 0) ? "0.00" : price);
                        Price.InnerXml = price;
                        Transaction.AppendChild(Price);

                        XmlNode NettValue = xd.CreateElement("NettValue");
                        txtVALUE = (TextBox)GVR.FindControl("txtVALUE");
                        string nval = txtVALUE.Text.Trim().Replace("&nbsp;", "");
                        nval = ((nval.Length == 0) ? "0.00" : nval);
                        NettValue.InnerXml = nval;
                        Transaction.AppendChild(NettValue);

                        //XmlNode TaxValue = xd.CreateElement("TaxValue");
                        //txtVATx = (TextBox)GVR.FindControl("txtVAT");
                        //string tval = txtVATx.Text.Trim().Replace("&nbsp;", "");
                        //tval = ((tval.Length == 0) ? "0.00" : tval);
                        //TaxValue.InnerXml = tval;
                        //Transaction.AppendChild(TaxValue);

                        XmlNode GoodsRecdDetailID = xd.CreateElement("GoodsRecdDetailID");
                        lblGoodsRecdDetailID = (Label)GVR.FindControl("lblGoodsRecdDetailID");
                        //lblGoodsRecdDetailID.Text = "";//Added by Mainak 2017-11-17
                        string grID = lblGoodsRecdDetailID.Text.Trim().Replace("&nbsp;", "");
                        //grID = ((grID.Length == 0) ? "0" : grID);
                        //GoodsRecdDetailID.InnerXml = grID;
                        if (!(grID.Length == 0 || grID.Equals("0.00")))
                        {
                            GoodsRecdDetailID.InnerXml = grID;
                        }
                        else
                        {
                            grID = "0";
                        }
                        Transaction.AppendChild(GoodsRecdDetailID);

                        XmlNode DeptID = xd.CreateElement("DepartmentID");
                        lblDeptID = (Label)GVR.FindControl("lblDeptID");
                        string dID = lblDeptID.Text.Trim().Replace("&nbsp;", "");
                        //dID = ((dID.Length == 0) ? "0" : dID);
                        //DeptID.InnerXml = dID;
                        if (!(dID.Length == 0 || dID.Equals("0.00")))
                        {
                            DeptID.InnerXml = dID;
                        }
                        else
                        {
                            dID = "0";
                        }
                        Transaction.AppendChild(DeptID);

                        XmlNode NominalCodeID = xd.CreateElement("NominalCodeID");
                        lblNominalCodeID = (Label)GVR.FindControl("lblNominalCodeID");
                        string ncID = lblNominalCodeID.Text.Trim().Replace("&nbsp;", "");
                        //ncID = ((ncID.Length == 0) ? "0" : ncID);
                        //NominalCodeID.InnerXml = ncID;
                        if (!(ncID.Length == 0 || ncID.Equals("0.00")))
                        {
                            NominalCodeID.InnerXml = ncID;
                        }
                        else
                        {
                            ncID = "0";
                        }
                        Transaction.AppendChild(NominalCodeID);

                        XmlNode BusinessUnitID = xd.CreateElement("BusinessUnitID");
                        lblBusinessUnitID = (Label)GVR.FindControl("lblBusinessUnitID");
                        string buID = lblBusinessUnitID.Text.Trim().Replace("&nbsp;", "");
                        if (!(buID.Length == 0 || buID.Equals("0.00")))
                        {
                            BusinessUnitID.InnerXml = buID;
                        }
                        else
                        {
                            buID = "0";
                        }
                        Transaction.AppendChild(BusinessUnitID);

                        //Added by Mainak 2017-11-21
                        XmlNode projectCode = xd.CreateElement("ProjectCode");
                        lblProjectCode = (Label)GVR.FindControl("lblProjectCode");
                        string PC = lblProjectCode.Text.Trim().Replace("&nbsp;", "");
                        projectCode.InnerXml = PC;
                        Transaction.AppendChild(projectCode);

                        //Added by Mainak 2018-05-31
                        XmlNode purOrderLineNo = xd.CreateElement("PurOrderLineNo");
                        lblPurOrderLineNo = (Label)GVR.FindControl("lblPurOrderLineNo");
                        string PLNO = lblPurOrderLineNo.Text.Trim().Replace("&nbsp;", "");
                        purOrderLineNo.InnerXml = PLNO;
                        Transaction.AppendChild(purOrderLineNo);

                        //Modified by Mainak 2018-05-31
                        /*int grdID = Convert.ToInt32(grID);
                        if (grdID != 0)
                        {
                            DataTable dtGR = new DataTable();
                            using (SqlConnection con = new SqlConnection(CBSAppUtils.PrimaryConnectionString))
                            {
                                con.Open();
                                SqlCommand cmd = new SqlCommand("SP_getTaxRateForGoodsRecdDeailID", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@GoodsRecdDetailID", grdID);
                                SqlDataAdapter da = new SqlDataAdapter();
                                da.SelectCommand = cmd;
                                da.Fill(dtGR);
                                con.Close();
                            }*/

                        //Modified by Mainak 2018-07-23
                        //int purorderlineno = Convert.ToInt32(PLNO);                        
                        int purorderlineno = 0;
                        if (PLNO != "")
                        {
                            purorderlineno = Convert.ToInt32(PLNO);
                        }                       

                        
                        if (purorderlineno != 0)
                        {
                            DataTable dtGR = new DataTable();
                            using (SqlConnection con = new SqlConnection(CBSAppUtils.PrimaryConnectionString))
                            {
                                con.Open();
                                SqlCommand cmd = new SqlCommand("SP_getTaxRateForPurOrderLineNo", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@PurOrderLineNo", purorderlineno);
                                cmd.Parameters.AddWithValue("@PurOrderNo", hdnPurOrderNo.Value);
                                SqlDataAdapter da = new SqlDataAdapter();
                                da.SelectCommand = cmd;
                                da.Fill(dtGR);
                                con.Close();
                            }
                            //Ended by Mainak 2018-05-31
                            XmlNode TaxValue = xd.CreateElement("TaxValue");
                            decimal taxVal = Convert.ToDecimal(nval) * Convert.ToDecimal(((dtGR.Rows[0]["TaxRate"] as object == DBNull.Value) ? 0 : dtGR.Rows[0]["TaxRate"]));
                            string finalVal = Math.Round(Convert.ToDecimal(taxVal), 2).ToString();
                            TaxValue.InnerXml = finalVal;
                            Transaction.AppendChild(TaxValue);

                            XmlNode QtyMatch = xd.CreateElement("QtyMatch");
                            string qtyMatch = Convert.ToString((Convert.ToDecimal(qty) > Convert.ToDecimal(((dtGR.Rows[0]["Quantity"] as object == DBNull.Value) ? 0 : dtGR.Rows[0]["Quantity"]))) ? 0 : 1);
                            QtyMatch.InnerXml = qtyMatch;
                            Transaction.AppendChild(QtyMatch);

                            XmlNode PriceMatch = xd.CreateElement("PriceMatch");
                            string priceMatch = Convert.ToString((Convert.ToDecimal(price) > Convert.ToDecimal(((dtGR.Rows[0]["Rate"] as object == DBNull.Value) ? 0 : dtGR.Rows[0]["Rate"]))) ? 0 : 1);
                            PriceMatch.InnerXml = priceMatch;
                            Transaction.AppendChild(PriceMatch);
                        }
                        //Added by Mainak 2017-11-17
                        else
                        {
                            //Added by Mainak 2017-11-20
                            XmlNode TaxValue = xd.CreateElement("TaxValue");
                            string taxVal = (GVR.FindControl("txtVAT") as TextBox).Text;
                            string finalVal = string.IsNullOrEmpty(taxVal) ? "" : taxVal;
                            TaxValue.InnerXml = finalVal;
                            Transaction.AppendChild(TaxValue);
                            //Ended by Mainak 2017-11-20

                            XmlNode QtyMatch = xd.CreateElement("QtyMatch");
                            string qtyMatch = "";
                            QtyMatch.InnerXml = qtyMatch;
                            Transaction.AppendChild(QtyMatch);

                            XmlNode PriceMatch = xd.CreateElement("PriceMatch");
                            string priceMatch = "";
                            PriceMatch.InnerXml = priceMatch;
                            Transaction.AppendChild(PriceMatch);
                        }
                        //End by Mainak 2017-11-17
                        //Under Transaction
                    }
                    #endregion
                }

                if (Convert.ToDecimal(txtNet.Text) != Convert.ToDecimal(txtSum.Text) && txtIsLnItm.Text == false.ToString())
                {
                    #region Transaction Element (If ‘Net’ on Header web page tab not equals sum of ‘Value’ fields in Line Items; OR LineItems = FALSE)
                    XmlNode Transaction = xd.CreateElement("Transaction");
                    Document.AppendChild(Transaction);

                    XmlNode LineNo = xd.CreateElement("LineNo");
                    LineNo.InnerXml = "1";
                    Transaction.AppendChild(LineNo);

                    XmlNode OrderNo = xd.CreateElement("OrderNo");
                    string pono = txtPONumber.Text.Replace("cancelled", "").Replace("not found", "").Replace("not_found", "").Replace("CANCELLED", "").Replace("NOT FOUND", "").Replace("NOT_FOUND", "");
                    OrderNo.InnerXml = pono.Contains(";") ? "" : pono;
                    Transaction.AppendChild(OrderNo);

                    XmlNode buyerCode = xd.CreateElement("BuyerCode");
                   // buyerCode.InnerXml = "Buyer Code";
                    //Added by kd 2018-10-04
                    string buyerCode1 = string.Empty;
                    buyerCode.InnerXml = ((buyerCode1.Length == 0) ? "" : buyerCode1);
                    Transaction.AppendChild(buyerCode);

                    XmlNode Description = xd.CreateElement("Description");
                    Description.InnerXml = "Net Amount";
                    Transaction.AppendChild(Description);

                    XmlNode Quantity = xd.CreateElement("Quantity");
                    string qty = "1";
                    Quantity.InnerXml = qty;
                    Transaction.AppendChild(Quantity);

                    XmlNode Price = xd.CreateElement("Price");
                    string price = txtNet.Text;
                    price = ((price.Length == 0) ? "0.00" : price);
                    Price.InnerXml = price;
                    Transaction.AppendChild(Price);

                    XmlNode NettValue = xd.CreateElement("NettValue");
                    string nval = txtNet.Text;
                    nval = ((nval.Length == 0) ? "0.00" : nval);
                    NettValue.InnerXml = nval;
                    Transaction.AppendChild(NettValue);

                    XmlNode TaxValue = xd.CreateElement("TaxValue");
                    string tval = txtVAT.Text;
                    tval = ((tval.Length == 0) ? "0.00" : tval);
                    TaxValue.InnerXml = tval;
                    Transaction.AppendChild(TaxValue);

                    XmlNode GoodsRecdDetailID = xd.CreateElement("GoodsRecdDetailID");
                    //lblGoodsRecdDetailID = (Label)GVR.FindControl("lblGoodsRecdDetailID");
                    string grID = string.Empty;
                    grID = ((grID.Length == 0) ? "" : grID);
                    GoodsRecdDetailID.InnerXml = grID;
                    Transaction.AppendChild(GoodsRecdDetailID);

                    XmlNode DeptID = xd.CreateElement("DepartmentID");
                    //lblDeptID = (Label)GVR.FindControl("lblDeptID");
                    string dID = string.Empty;
                    dID = ((dID.Length == 0) ? "" : dID);
                    DeptID.InnerXml = dID;
                    Transaction.AppendChild(DeptID);

                    XmlNode NominalCodeID = xd.CreateElement("NominalCodeID");
                    //lblNominalCodeID = (Label)GVR.FindControl("lblNominalCodeID");
                    string ncID = string.Empty;
                    ncID = ((ncID.Length == 0) ? "" : ncID);
                    NominalCodeID.InnerXml = ncID;
                    Transaction.AppendChild(NominalCodeID);

                    XmlNode BusinessUnitID = xd.CreateElement("BusinessUnitID");
                    //lblBusinessUnitID = (Label)GVR.FindControl("lblBusinessUnitID");
                    string buID = string.Empty;
                    buID = ((buID.Length == 0) ? "" : buID);
                    BusinessUnitID.InnerXml = buID;
                    Transaction.AppendChild(BusinessUnitID);

                    //Added by Mainak 2017-11-21
                    XmlNode projectCode = xd.CreateElement("ProjectCode");
                   // projectCode.InnerXml = "Project Code";
                    //Added by kd 2018-10-04
                    string projectCode1 = string.Empty;
                    projectCode.InnerXml = ((projectCode1.Length == 0) ? "" : projectCode1);
                    Transaction.AppendChild(projectCode);

                    //Added by Mainak 2018-05-31                   
                    XmlNode purOrderLineNo = xd.CreateElement("PurOrderLineNo");
                    //purOrderLineNo.InnerXml = "PO Line No";
                    //Added by kd 2018-10-04
                    string purOrderLineNo1 = string.Empty;
                    purOrderLineNo.InnerXml = ((purOrderLineNo1.Length == 0) ? "" : purOrderLineNo1);
                    Transaction.AppendChild(purOrderLineNo);
                  
                    #endregion
                }

                xd.Save(xmlPath);

                tf = true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in SaveDataToXMLWhenProcessed: " + ss;
            }
            return tf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        bool UpdateXMLWhenSaved(string xmlFilePath)
        {
            bool tf = false;
            string val = "";
            int i = 0;

            decimal vat, sub, tot;
            vat = sub = tot = 0;

            vat = (gvLists.FooterRow != null) ? Convert.ToDecimal(gvLists.FooterRow.Cells[6].Text) : 0;//vat cell
            tot = (gvLists.FooterRow != null) ? Convert.ToDecimal(gvLists.FooterRow.Cells[5].Text) : 0;//tot cell
            sub = tot - vat;

            //try
            //{
            DataTable DT = (DataTable)ViewState["LineItemsDT"];
            DT = ReturnExistingDataAsDataTable(DT);

            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlNode xnInvoice = xd.ChildNodes[1];//the second node is our target node

            string COMPANY_NAME = DC.ReturnCompanyNameByBatchID(Convert.ToInt32(lblBatchID.Text));
            string BATCH_NAME = lblBatchName.Text;
            string ORIGINAL_NAME = lblFileName.Text;
            string BatchName_BatchID = lblBatchName.Text + "_" + lblBatchID.Text;
            string FilePath = "";

            foreach (XmlNode xn in xnInvoice.ChildNodes)
            {
                switch (xn.Name)
                {
                    case "DocFilePath":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        if (string.IsNullOrEmpty(val))
                            FilePath = @"\\90104-server2\FTP Upload\" + COMPANY_NAME + @"\" + BATCH_NAME + @"\" + ORIGINAL_NAME;
                        else
                            FilePath = val;
                        val = retrunPositon(val);
                        xnInvoice.ChildNodes[i].InnerXml = FilePath + val;
                        break;
                    case "ArchiveFilePath":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        if (string.IsNullOrEmpty(val))
                            FilePath = @"\\90107-server3\FTP Archive\" + COMPANY_NAME + @"\" + BatchName_BatchID + @"\" + ORIGINAL_NAME;
                        else
                            FilePath = val;
                        val = retrunPositon(val);
                        xnInvoice.ChildNodes[i].InnerXml = FilePath + val;
                        break;
                    case "BatchTypeName":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        string batchType = ddlBatchType.SelectedItem.Text;
                        xnInvoice.ChildNodes[i].InnerXml = CommonFunctions.ReturnParsedStringForXML(batchType) + val;
                        break;
                    case "BatchTypeId":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        xnInvoice.ChildNodes[i].InnerXml = ddlBatchType.SelectedValue + val;
                        break;
                    case "CompanyID":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        xnInvoice.ChildNodes[i].InnerXml = ddlCompany.SelectedValue + val;
                        break;
                    case "BatchDocType":
                        //val = xnInvoice.ChildNodes[i].InnerXml;
                        //val = retrunPositon(val);
                        //xnInvoice.ChildNodes[i].InnerXml = "NA" + val;
                        break;
                    case "DocumentType":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        string DocType = (ddlDocType.SelectedValue.ToString() == "INV") ? "Sales Invoice" : "Sales Credit";//ddlDocType.SelectedItem.Text;
                        xnInvoice.ChildNodes[i].InnerXml = DocType + val;
                        break;
                    case "Supplier":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        string sup = CommonFunctions.TrimSupplierName(txtSupplier.Text);
                        sup = CommonFunctions.ReturnParsedStringForXML(sup);
                        xnInvoice.ChildNodes[i].InnerXml = sup + val;
                        break;
                    case "Date":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        DateTime fromDateValue;
                        bool x = DateTime.TryParseExact(txtDocDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateValue);
                        if (x)
                            xnInvoice.ChildNodes[i].InnerXml = txtDocDate.Text + val;
                        else
                            xnInvoice.ChildNodes[i].InnerXml = "" + val;
                        break;
                    case "TaxPoint":
                        //val = xnInvoice.ChildNodes[i].InnerXml;
                        //val = retrunPositon(val);
                        //xnInvoice.ChildNodes[i].InnerXml = "NA" + val;
                        break;
                    case "InvoiceNumber":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        xnInvoice.ChildNodes[i].InnerXml = txtDocNumber.Text + val;
                        break;
                    case "PurchaseOrder":
                        string pono = "";
                        if (string.IsNullOrEmpty(txtPONumber.Text) == false)
                        {
                            pono = txtPONumber.Text;
                        }
                        //else //for multiple po numbers
                        //{
                        //    foreach (DataRow DR in DT.Rows)
                        //        pono += DR["PONO"].ToString() + "; ";

                        //    string x = pono.Substring(pono.Length - 2, 2);

                        //    if (x == "; ")
                        //        pono = pono.Substring(0, pono.Length - 2);
                        //}

                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        xnInvoice.ChildNodes[i].InnerXml = pono + val;
                        break;
                    case "VATRegistrationNo":
                        //val = xnInvoice.ChildNodes[i].InnerXml;
                        //val = retrunPositon(val);
                        //xnInvoice.ChildNodes[i].InnerXml = "NA" + val;
                        break;
                    case "Currency":
                        val = xnInvoice.ChildNodes[i].InnerXml;
                        val = retrunPositon(val);
                        xnInvoice.ChildNodes[i].InnerXml = ddlCurrency.SelectedItem.Text + val;
                        break;
                    case "Error":
                        //val = xnInvoice.ChildNodes[i].InnerXml;
                        //val = retrunPositon(val);
                        //xnInvoice.ChildNodes[i].InnerXmle = "NA" + val;
                        break;
                    case "ErrorMessage":
                        //val = xnInvoice.ChildNodes[i].InnerXml;
                        //val = retrunPositon(val);
                        //xnInvoice.ChildNodes[i].InnerXml = "NA" + val;
                        break;
                    case "FilePath":
                        //val = xnInvoice.ChildNodes[i].InnerXml;
                        //val = retrunPositon(val);
                        //xnInvoice.ChildNodes[i].InnerXml = "NA" + val;
                        break;
                    case "NOTES":
                        #region Notes
                        int j = 0;
                        XmlNode xnNotes = xnInvoice.ChildNodes[i];
                        foreach (XmlNode xnV in xnNotes.ChildNodes)
                        {
                            switch (xnV.Name)
                            {
                                case "InvoiceTotal":
                                    val = xnNotes.ChildNodes[j].InnerXml;
                                    val = retrunPositon(val);
                                    xnNotes.ChildNodes[j].InnerXml = txtTotal.Text + val;
                                    break;
                                case "InvoiceSubtotal":
                                    val = xnNotes.ChildNodes[j].InnerXml;
                                    val = retrunPositon(val);
                                    xnNotes.ChildNodes[j].InnerXml = txtNet.Text + val;
                                    break;
                                case "Discount":
                                    //val = xnNotes.ChildNodes[j].InnerXml;
                                    //val = retrunPositon(val);
                                    //xnNotes.ChildNodes[j].InnerXml = "NA" + val;
                                    break;
                                case "InvoiceVatTotal":
                                    val = xnNotes.ChildNodes[j].InnerXml;
                                    val = retrunPositon(val);
                                    xnNotes.ChildNodes[j].InnerXml = txtVAT.Text + val;
                                    break;
                            }
                            j++;
                        }
                        #endregion
                        break;
                    case "Resolution":
                        #region Resolution
                        XmlNode xnResolution = xn;

                        XmlNode xnPagePrev = xnResolution.ChildNodes[0];
                        //Added by Mainak 2017-11-20
                        string height = (xnPagePrev == null) ? "0" : ((xnPagePrev.Attributes["height"] as object == null) ? "0" : xnPagePrev.Attributes["height"].InnerText);
                        string width = (xnPagePrev == null) ? "0" : ((xnPagePrev.Attributes["width"] as object == null) ? "0" : xnPagePrev.Attributes["width"].InnerText);
                        //Ended by Mainak 2017-11-20

                        xnResolution.InnerXml = "";
                        xd.Save(xmlFilePath);

                        int vi = 0;

                        foreach (DataRow DR in DT.Rows)
                        {
                            string pos = DR["POS"].ToString();
                            pos = (pos.Length == 0) ? "/" : pos;
                            string[] arr = pos.Split('/');

                            XmlNode xnPage = xd.CreateElement("Page");
                            XmlAttribute xaNumber = xd.CreateAttribute("Number");
                            xaNumber.Value = vi.ToString();
                            xnPage.Attributes.Append(xaNumber);
                            XmlAttribute xaHeight = xd.CreateAttribute("height");
                            xaHeight.Value = height;
                            xnPage.Attributes.Append(xaHeight);
                            XmlAttribute xaWidth = xd.CreateAttribute("width");
                            xaWidth.Value = width;
                            xnPage.Attributes.Append(xaWidth);
                            vi++;
                            xnResolution.AppendChild(xnPage);

                            XmlNode ItemDetail = xd.CreateElement("ItemDetail");
                            xnPage.AppendChild(ItemDetail);

                            XmlNode ItemValue = xd.CreateElement("ItemValue");
                            ItemValue.InnerXml = DR["VALUE"].ToString() + "<Position>" + arr.GetValue(0) + "</Position>";
                            ItemDetail.AppendChild(ItemValue);

                            XmlNode ItemVat = xd.CreateElement("ItemVat");
                            ItemVat.InnerXml = DR["VAT"].ToString();// +"<Position>0:0:0:0:0</Position>";
                            ItemDetail.AppendChild(ItemVat);

                            XmlNode ItemPoNumber = xd.CreateElement("ItemPoNumber");
                            ItemPoNumber.InnerXml = DR["PONO"].ToString() + "<Position>" + arr.GetValue(1) + "</Position>";
                            ItemDetail.AppendChild(ItemPoNumber);

                            XmlNode ItemCurrency = xd.CreateElement("ItemCurrency");
                            ItemCurrency.InnerXml = ddlCurrency.SelectedItem.Text;
                            ItemDetail.AppendChild(ItemCurrency);

                            XmlNode ItemDescription = xd.CreateElement("ItemDescription");
                            string desc = DR["DESC"].ToString();
                            ItemDescription.InnerXml = CommonFunctions.ReturnParsedStringForXML(desc);
                            ItemDetail.AppendChild(ItemDescription);

                            XmlNode ItemQty = xd.CreateElement("ItemQty");
                            ItemQty.InnerXml = DR["QTY"].ToString();
                            ItemDetail.AppendChild(ItemQty);

                            XmlNode ItemUnitPrice = xd.CreateElement("ItemUnitPrice");
                            ItemUnitPrice.InnerXml = DR["PRICE"].ToString();
                            ItemDetail.AppendChild(ItemUnitPrice);

                            XmlNode BuyerCode = xd.CreateElement("BuyerCode");
                            BuyerCode.InnerXml = Convert.ToString(DR["BUYERCODE"]);
                            ItemDetail.AppendChild(BuyerCode);

                            XmlNode GoodsRecdDetailID = xd.CreateElement("GoodsRecdDetailID");
                            GoodsRecdDetailID.InnerXml = Convert.ToString(DR["GOODSRECDDETAILID"]);
                            ItemDetail.AppendChild(GoodsRecdDetailID);

                            XmlNode DepartmentID = xd.CreateElement("DepartmentID");
                            DepartmentID.InnerXml = Convert.ToString(DR["DEPARTMENTID"]);
                            ItemDetail.AppendChild(DepartmentID);

                            XmlNode NominalCodeID = xd.CreateElement("NominalCodeID");
                            NominalCodeID.InnerXml = Convert.ToString(DR["NOMINALCODEID"]);
                            ItemDetail.AppendChild(NominalCodeID);

                            XmlNode BusinessUnitID = xd.CreateElement("BusinessUnitID");
                            BusinessUnitID.InnerXml = Convert.ToString(DR["BUSINESSUNITID"]);
                            ItemDetail.AppendChild(BusinessUnitID);

                            XmlNode ProjectCode = xd.CreateElement("ProjectCode");
                            ProjectCode.InnerXml = Convert.ToString(DR["PROJECTCODE"]);
                            ItemDetail.AppendChild(ProjectCode);

                            //Added by Mainak 2018-05-31
                            XmlNode PurOrderLineNo = xd.CreateElement("PurOrderLineNo");
                            PurOrderLineNo.InnerXml = Convert.ToString(DR["PURORDERLINENO"]);
                            ItemDetail.AppendChild(PurOrderLineNo);
                        }
                        #endregion
                        break;
                    case "InvoiceStatus":
                        //xnInvoice.ChildNodes[i].FirstChild.Value = NA
                        break;
                }
                i++;
            }

            xd.Save(xmlFilePath);

            tf = true;
            //}
            //catch (Exception ex)
            //{
            //    string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
            //    lblMsg.Text = "\nError in UpdateXMLWhenSaved: " + ss;
            //    tf = false;
            //}

            return tf;
        }

        /// <summary>
        /// Set Java Script confirm dialog for btnProcess
        /// </summary>
        void SetJavaScriptConfirmBox()
        {
            btnProcess.UseSubmitBehavior = true;
            btnProcess.OnClientClick = "return validation();";
            //btnProcess.Attributes.Add("onclick", "return validation(" + l + ");");
            btnDelete.UseSubmitBehavior = true;
            btnDelete.OnClientClick = "return DoDelete();";
        }

        /// <summary>
        /// Set Company ids to be applied in html links
        /// </summary>
        void SetCompanyID()
        {
            buyercompanyid = ddlCompany.SelectedValue.ToString();
            string supplier = CommonFunctions.TrimSupplierName(txtSupplier.Text);
            string code = CommonFunctions.TrimSupplierCode(txtSupplier.Text);
            int buyerID = Convert.ToInt32(buyercompanyid);
            suppliercompanyid = DC.ReturnSupplierID(supplier, buyerID, code).ToString();
        }

        /// <summary>
        /// Java Script implementation to redirect the url
        /// </summary>
        /// <param name="url"></param>
        void Redirection(string url)
        {
            if (url.ToLower() == "actionwindow.aspx")
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_close", "CloseParent();", true);
            else
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_redirect", "redirect('" + url + "');", true);
        }

        /// <summary>
        /// Download XML File as per document id and company id to test folder
        /// </summary>
        /// <param name="comID"></param>
        /// <param name="docID"></param>
        void SaveTestXML(int comID, int docID)
        {
            string xmlSrc = @"C:\P2D\XML DNLD\" + testXMLFileName;

            if (File.Exists(xmlSrc))
            {
                string dir = @"C:\P2D\IPE Output\DefaultCompany\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string xmlDst = dir + testXMLFileName;
                if (!File.Exists(xmlDst))
                {
                    File.Copy(xmlSrc, xmlDst);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void ReSetRedBorders()
        {
            #region remove red border from the fields of Header items
            if (string.IsNullOrEmpty(txtSupplier.Text) == false)
                txtSupplier.Style.Add("border-color", "#ccc !important");
            else
                txtSupplier.Style.Add("border-color", "red !important");

            if (string.IsNullOrEmpty(txtDocNumber.Text) == false)
                txtDocNumber.Style.Add("border-color", "#ccc !important");
            else
                txtDocNumber.Style.Add("border-color", "red !important");

            if (string.IsNullOrEmpty(txtPONumber.Text) == true && txtIsPO.Text.ToLower() == "true")
                txtPONumber.Style.Add("border-color", "red !important");
            else
                txtPONumber.Style.Add("border-color", "#ccc !important");

            //PO Supplier field should not have a red border in any circumstances.
            //if (string.IsNullOrEmpty(txtPOSupplier.Text) == false)
            //    txtPOSupplier.Style.Add("border-color", "#ccc !important");
            //else
            //    txtPOSupplier.Style.Add("border-color", "red !important");

            if (string.IsNullOrEmpty(txtDocDate.Text) == false)
                txtDocDate.Style.Add("border-color", "#ccc !important");
            else
                txtDocDate.Style.Add("border-color", "red !important");

            if (string.IsNullOrEmpty(ddlCurrency.SelectedItem.Text) == false)
                ddlCurrency.Style.Add("border-color", "#ccc !important");
            else
                ddlCurrency.Style.Add("border-color", "red !important");

            if (string.IsNullOrEmpty(txtNet.Text) == false)
                txtNet.Style.Add("border-color", "#ccc !important");
            else
                txtNet.Style.Add("border-color", "red !important");

            if (string.IsNullOrEmpty(txtVAT.Text) == false)
                txtVAT.Style.Add("border-color", "#ccc !important");
            else
                txtVAT.Style.Add("border-color", "red !important");

            if (string.IsNullOrEmpty(txtTotal.Text) == false)
                txtTotal.Style.Add("border-color", "#ccc !important");
            else
                txtTotal.Style.Add("border-color", "red !important");

            #endregion

            #region Conditional Linte Items Colouring
            TextBox txtPONO = new TextBox();
            TextBox txtBuyerCode = new TextBox();
            TextBox txtDescription = new TextBox();
            TextBox txtQTY = new TextBox();
            TextBox txtPRICE = new TextBox();
            TextBox txtVATx = new TextBox();
            TextBox txtVALUE = new TextBox();

            //Response.Write("<br /> gvLists.Rows.Count in Page_LoadComplete: " + gvLists.Rows.Count);
            foreach (GridViewRow GVR in gvLists.Rows)
            {
                txtPONO = (TextBox)GVR.FindControl("txtPONO");
                txtBuyerCode = (TextBox)GVR.FindControl("txtBuyerCode");
                txtDescription = (TextBox)GVR.FindControl("txtDescription");
                txtQTY = (TextBox)GVR.FindControl("txtQTY");
                txtPRICE = (TextBox)GVR.FindControl("txtPRICE");
                txtVATx = (TextBox)GVR.FindControl("txtVAT");
                txtVALUE = (TextBox)GVR.FindControl("txtVALUE");

                #region border of txtPONO
                if (Convert.ToBoolean(txtIsPO.Text) == true)
                {
                    if (string.IsNullOrEmpty(txtPONO.Text) == true || txtPONO.Text.ToLower() == "not found" || txtPONO.Text.ToLower() == "not_found" || txtPONO.Text.ToLower() == "cancelled")
                    {
                        txtPONO.Text = "";
                        txtPONO.Style.Add("border", "solid 1px red");
                    }
                    else
                    {
                        txtPONO.Style.Add("border", "none");
                    }
                }
                else
                {
                    txtPONO.Style.Add("border", "none");
                }
                #endregion

                #region border of txtDescription
                if (Convert.ToBoolean(txtIsDesc.Text) == true)
                {
                    if (string.IsNullOrEmpty(txtDescription.Text) == true || txtDescription.Text.ToLower() == "not found" || txtDescription.Text.ToLower() == "not_found" || txtDescription.Text.ToLower() == "cancelled")
                    {
                        txtDescription.Text = "";
                        txtDescription.Style.Add("border", "solid 1px red");
                    }
                    else
                    {
                        txtDescription.Style.Add("border", "none");
                    }
                }
                else
                {
                    txtDescription.Style.Add("border", "none");
                }
                #endregion

                #region border of txtQTY, txtPRICE, txtVALUE
                if (Convert.ToBoolean(txtIsLnItm.Text) == true)
                {
                    if (string.IsNullOrEmpty(txtQTY.Text) == true || txtQTY.Text.ToLower() == "not found" || txtQTY.Text.ToLower() == "not_found" || txtQTY.Text.ToLower() == "cancelled")
                    {
                        txtQTY.Text = "";
                        txtQTY.Style.Add("border", "solid 1px red");
                    }
                    else
                    {
                        txtQTY.Style.Add("border", "none");
                    }

                    if (string.IsNullOrEmpty(txtPRICE.Text) == true || txtPRICE.Text.ToLower() == "not found" || txtPRICE.Text.ToLower() == "not_found" || txtPRICE.Text.ToLower() == "cancelled")
                    {
                        txtPRICE.Text = "";
                        txtPRICE.Style.Add("border", "solid 1px red");
                    }
                    else
                    {
                        txtPRICE.Style.Add("border", "none");
                    }

                    if (string.IsNullOrEmpty(txtVALUE.Text) == true || txtVALUE.Text.ToLower() == "not found" || txtVALUE.Text.ToLower() == "not_found" || txtVALUE.Text.ToLower() == "cancelled")
                    {
                        txtVALUE.Text = "";
                        txtVALUE.Style.Add("border", "solid 1px red");
                    }
                    else
                    {
                        txtVALUE.Style.Add("border", "none");
                    }
                }
                else
                {
                    txtQTY.Style.Add("border", "none");
                    txtPRICE.Style.Add("border", "none");
                    txtVALUE.Style.Add("border", "none");
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        /// <returns></returns>
        DataTable ReturnExistingDataAsDataTable(DataTable DT)
        {
            int i = 0;
            //get existing data from grid view
            TextBox txtPONO = new TextBox();
            TextBox txtBuyerCode = new TextBox();
            TextBox txtDesc = new TextBox();
            TextBox txtQTY = new TextBox();
            TextBox txtPRICE = new TextBox();
            TextBox txtVATx = new TextBox();
            TextBox txtVALUE = new TextBox();
            TextBox txtPOS = new TextBox();
            TextBox txtPAGE = new TextBox();
            Label lblGoodsRecdDetailID = new Label();
            Label lblDeptID = new Label();
            Label lblNominalCodeID = new Label();
            Label lblBusinessUnitID = new Label();
            Label lblProjectCode = new Label();//Added by Mainak 2017-11-21
            Label lblPurOrderLineNo = new Label();//Added by Mainak 2018-05-31

            for (i = 0; i < gvLists.Rows.Count; i++)
            {
                DataRow DR = DT.Rows[i];
                GridViewRow GVR = gvLists.Rows[i];

                txtPONO = (TextBox)GVR.FindControl("txtPONO");
                txtBuyerCode = (TextBox)GVR.FindControl("txtBuyerCode");
                txtDesc = (TextBox)GVR.FindControl("txtDescription");
                txtQTY = (TextBox)GVR.FindControl("txtQTY");
                txtPRICE = (TextBox)GVR.FindControl("txtPRICE");
                txtVATx = (TextBox)GVR.FindControl("txtVAT");
                txtVALUE = (TextBox)GVR.FindControl("txtVALUE");
                txtPOS = (TextBox)GVR.FindControl("txtPOS");
                txtPAGE = (TextBox)GVR.FindControl("txtPAGE");
                lblGoodsRecdDetailID = GVR.FindControl("lblGoodsRecdDetailID") as Label;
                lblDeptID = GVR.FindControl("lblDeptID") as Label;
                lblNominalCodeID = GVR.FindControl("lblNominalCodeID") as Label;
                lblBusinessUnitID = GVR.FindControl("lblBusinessUnitID") as Label;
                lblProjectCode = GVR.FindControl("lblProjectCode") as Label;//Added by Mainak 2017-11-21
                lblPurOrderLineNo = GVR.FindControl("lblPurOrderLineNo") as Label;//Added by Mainak 2018-05-31

                DR["PONO"] = txtPONO.Text;
                DR["BUYERCODE"] = txtBuyerCode.Text;
                DR["DESC"] = txtDesc.Text;
                DR["QTY"] = txtQTY.Text;
                DR["PRICE"] = txtPRICE.Text;
                DR["VAT"] = txtVATx.Text;
                DR["VALUE"] = txtVALUE.Text;
                DR["POS"] = txtPOS.Text;
                DR["PAGE"] = txtPAGE.Text;
                DR["GOODSRECDDETAILID"] = lblGoodsRecdDetailID.Text;
                DR["DEPARTMENTID"] = lblDeptID.Text;
                DR["NOMINALCODEID"] = lblNominalCodeID.Text;
                DR["BUSINESSUNITID"] = lblBusinessUnitID.Text;
                DR["PROJECTCODE"] = lblProjectCode.Text;//Added by Mainak 2017-11-21
                DR["PURORDERLINENO"] = lblPurOrderLineNo.Text;//Added by Mainak 2018-05-31
            }

            return DT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        string retrunPositon(string val)
        {
            string[] arr = val.Split(new string[] { "<Position>" }, StringSplitOptions.None);

            if (arr.Length > 1)
                val = "<Position>" + arr.GetValue(1);
            else
                val = "";

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comID">Company ID</param>
        /// <param name="docID">Document ID</param>
        void DownloadImage(int comID, int docID)
        {
            if (test)
            {
                string filePath = @"C:\P2D\FTP Upload\Default Company\Default Batch\Default.tif";//Default.tif

                if (File.Exists(filePath))
                {
                    string fileName = Path.GetFileName(filePath);
                    int i = fileName.Split('.').Length;
                    string fileExt = fileName.Split('.')[i - 1];

                    DownloadFile(filePath, fileName, fileExt);
                }
            }
            else
            {
                string companyName = "";
                string batchName = "";
                string originalName = "";

                DataTable DT = DC.PathInformationTable(comID, docID);

                int c = DT.Rows.Count;

                //Response.Write("<br />DT Row Count in LoadTopData: " + c);

                if (c > 0)
                {
                    DataRow DR = DT.Rows[0];

                    companyName = DR["CompanyName"].ToString();
                    batchName = DR["BATCH NAME"].ToString();
                    originalName = DR["ORIGINAL NAME"].ToString();
                }
                else
                {
                    companyName = "";
                    batchName = "";
                    originalName = "";
                }

                //Response.Write("companyName = " + companyName + " batchName = " + batchName + " originalName = " + originalName + "<br />");

                try
                {
                    string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceURL"];
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = serviceUrl;

                    string downloadURL = "//90104-server2/FTP Upload/" + companyName + "/" + batchName + "/" + originalName;

                    downloadURL = downloadURL.Replace("/", @"\");

                    byte[] bytes = objService.DownloadFile(downloadURL);

                    if (bytes != null)
                    {
                        string fileName = originalName;
                        string tempFilePath = Server.MapPath("~") + "\\Temp\\" + fileName;

                        int i = fileName.Split('.').Length;
                        string fileExt = fileName.Split('.')[i - 1];

                        File.WriteAllBytes(tempFilePath, bytes);

                        DownloadFile(tempFilePath, fileName, fileExt);
                    }
                    else
                    {
                        Response.Write("No Image retrieved.");
                    }
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                    Response.Write("Error in DownloadAndSetTiffImage: " + ss);
                }
            }
        }

        private void DownloadFile(string filePath, string fileName, string fileType)
        {
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/" + fileType;
                Context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                long FileSize = fs.Length;
                byte[] getContent = new byte[(int)FileSize];
                fs.Read(getContent, 0, (int)fs.Length);
                fs.Close();
                Context.Response.BinaryWrite(getContent);
                Context.Response.Flush();
                Context.Response.End();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.InnerException;
                lblMsg.Text = "\nError in DownloadFile: " + ss;
            }
            finally
            {
                if (!test)
                    DeleteFile(filePath);
            }
        }

        protected void DeleteFile(string filePath)
        {
            System.Threading.Thread th = null;

            th = new System.Threading.Thread(delegate()
            {
                bool isDel = false;
                while (isDel == false)
                {
                    try
                    {
                        File.Delete(filePath);
                        isDel = true;
                        th.Abort();
                    }
                    catch
                    {
                        isDel = false;
                    }
                }
            });

            th.Start();
            th.Join();
        }

        /// <summary>
        /// 
        /// </summary>
        void SetNewSupplierLink()
        {
            //Response.Write(Request.Url.AbsolutePath + "<br />");
            //Response.Write(Request.Url.AbsoluteUri + "<br />");
            //Response.Write(Request.Url.PathAndQuery + "<br />");
            string aburi = Request.Url.AbsoluteUri;
            string paqry = Request.Url.PathAndQuery;

            string domainName = aburi.Replace(paqry, "");
            //Response.Write("<br />domainName in Page_init: " + domainName);

            if (domainName.Contains("localhost"))
                domainName += "/p2d_website";

            string partURL = aburi.Replace(domainName, "");

            string[] arr = partURL.Split('/');

            string workDir = arr.GetValue(1).ToString();

            domainName += "/" + workDir;

            NewSupplierLink = domainName + "/Supplier/BrowseSupplier.aspx";
        }
        #endregion

        #region Web Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1">the value in the supplier text box</param>
        /// <param name="input2">the selected company id or buyer id</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string[] ListSupplierNames(string input1, string input2)
        {
            //input1: the value in the supplier text box
            //input2: the selected company id or buyer id

            List<string> list = new List<string>();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();
            try
            {
                //The supplier name string containing '' is giving issue thus removed.
                input1 = input1.Replace("[", "{").Replace("]", "}").Replace("''", "'");

                DT = DC.ReturnSupplierDataTable(input1, Convert.ToInt32(input2), input1);

                foreach (DataRow DR in DT.Rows)
                {
                    list.Add(DR[0].ToString() + "^" + DR[1].ToString());
                }
            }
            catch (Exception ex)
            {
                string ss = "Message: " + ex.Message
                    + "<br />Source: " + ex.Source
                    + "<br />StackTrace: " + ex.StackTrace
                    + "<br />TargetSite: " + ex.TargetSite
                    + "<br />InnerException: " + ex.InnerException
                    + "<br />Data: " + ex.Data;
                HttpContext.Current.Response.Write("<br />Error in ListSupplierNames WebMethod: " + ss);
                list = new List<string>();
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string[] ListPONumber(string batchTypeID, string buyerID, string supplierID, string poNoPartial)
        {

            List<string> list = new List<string>();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();
            try
            {
                //The supplier name string containing '' is giving issue thus removed.
                poNoPartial = poNoPartial.Replace("[", "{").Replace("]", "}").Replace("''", "'");

                //DT = DC.ReturnSupplierDataTable(input1, Convert.ToInt32(input2), input1);
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("SP_GetPONoForBuyerSupplier_JKS", sqlConn); //SP_GetPONoForBuyerSupplier has been replaced by kd
                cmd.Parameters.AddWithValue("@BatchTypeID", Convert.ToInt32(batchTypeID));
                cmd.Parameters.AddWithValue("@BuyerID", Convert.ToInt32(buyerID));
                cmd.Parameters.AddWithValue("@SupplierId", Convert.ToInt32(supplierID));
                cmd.Parameters.AddWithValue("@PurOrderNo", poNoPartial);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(DT);
                sqlConn.Close();

                foreach (DataRow DR in DT.Rows)
                {
                    list.Add(DR[0].ToString() + "^" + DR[0].ToString());
                }
            }
            catch (Exception ex)
            {
                string ss = "Message: " + ex.Message
                    + "<br />Source: " + ex.Source
                    + "<br />StackTrace: " + ex.StackTrace
                    + "<br />TargetSite: " + ex.TargetSite
                    + "<br />InnerException: " + ex.InnerException
                    + "<br />Data: " + ex.Data;
                HttpContext.Current.Response.Write("<br />Error in ListSupplierNames WebMethod: " + ss);
                list = new List<string>();
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1">the supplier company name</param>
        /// <param name="input2">the selected company id or buyer id</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string CheckValidSupplier(string input1, string input2)
        {
            //input1: the supplier company name
            //input2: the selected company id or buyer id

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            //change ' to '' for sql query
            input1 = input1.Replace("'", "''");
            string inputValue = input1;

            try
            {
                input1 = CommonFunctions.TrimSupplierName(input1);

                //change [ and ] with { and } for sql query; [ and ] are not recognised by sql for search string
                input1 = input1.Replace("[", "{").Replace("]", "}");

                DT = DC.ReturnSupplierDataTable(input1, Convert.ToInt32(input2));

                DT = DT.Select("[SupplierCompanyNameWithCodeAgainstBuyer] = '" + inputValue + "'").CopyToDataTable();

                int val = DT.Rows.Count;

                if (val > 0)
                    return "True";
                else
                    return "False";
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in CheckValidSupplier WebMethod: " + ss);
                return "False";
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1">the supplier name for supplier id</param>
        /// <param name="input2">the selected company id or the buyer id</param>
        /// <param name="input3">the selected currency</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetCurrencyStatus(string input1, string input2, string input3)
        {
            //input1: the supplier name for supplier id
            //input2: the selected company id or the buyer id
            //input3: the selected currency

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            string ret = "";

            try
            {
                string supplierName = CommonFunctions.TrimSupplierName(input1);
                string code = CommonFunctions.TrimSupplierCode(input1);


                int buyerID = Convert.ToInt32(input2);
                int SupplierID = DC.ReturnSupplierID(supplierName, buyerID, code);
                string Old_CurrencyCode = input3;
                string New_CurrencyCode = DC.ReturnNewCurrencyCode(SupplierID, buyerID);

                ret = New_CurrencyCode + "/";

                if (Old_CurrencyCode.Contains(New_CurrencyCode))
                    ret += "cur-same";
                else
                    ret += "cur-diff";

                return ret;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in GetCurrencyStatus WebMethod: " + ss);
                return "";
            }
            finally
            {
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1">OPNumber from Header section</param>
        /// <param name="input2">PONumber(s) from LineItems section</param>
        /// <param name="input3">The Name of the Supplier to get the ID</param>
        /// <param name="input4">The Selected company id or buyer id</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string CheckValidPurchaseOrder(string input1, string input2, string input3, string input4)
        {
            //input1: OPNumber from Header section
            //input2: PONumber(s) from LineItems section
            //input3: The Name of the Supplier to get the ID
            //input4: The Selected company id or buyer id

            string ret = "";

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable RDT = new DataTable();
            DataTable DT = new DataTable();

            try
            {
                bool tf = false;
                string x = "";
                int i = 0;

                #region Check if PONo belongs to datatable
                int countMatchedPO = 0;//no of PO numbers matched.
                int countEnteredPO = 0;//no of PO numbers entered.

                DT = DC.ReturnBuyerProdCodeTableForPONumber(input1);

                if (DT.Rows.Count > 0)
                {
                    countMatchedPO++;

                    try
                    {
                        //DT = DT.Select("[CompanyID] = " + input4).CopyToDataTable();

                        RDT = DT.Copy();
                    }
                    catch
                    {
                    }
                }
                else
                {
                    //if entered POs does not belong to database then list the PONos. which are not found.
                    if (!string.IsNullOrEmpty(input1))
                        x += input1 + ";";
                }

                string[] arr = input2.Split(',');
                foreach (string str in arr)
                {
                    DT = DC.ReturnBuyerProdCodeTableForPONumber(str);

                    if (DT.Rows.Count > 0)
                    {
                        countMatchedPO++;

                        try
                        {
                            //DT = DT.Select("[CompanyID] = " + input4).CopyToDataTable();

                            RDT.Merge(DT);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        //if entered POs does not belong to database then list the PONos. which are not found.
                        if (!string.IsNullOrEmpty(str))
                            x += str + ";";
                    }
                }

                //Check if count of available po nos is equal to RDT's row count or not
                //this is to check if all the entered POs are available in the database or not

                //one count for the Header PO Number
                if (input1 != "")
                    countEnteredPO++;

                //multiple count for the LineItems PO Number
                foreach (string str in arr)
                {
                    if (str != "")
                        countEnteredPO++;
                }

                if (countEnteredPO == countMatchedPO)
                    tf = true;
                else
                    tf = false;

                ret = tf.ToString();

                i = x.LastIndexOf(";");
                if (i != -1)
                    x = x.Substring(0, i);

                if (string.IsNullOrEmpty(x))
                    x = "NA";

                ret += "-" + x;
                #endregion

                #region Check if PONo has the entered Supplier
                string supplierName = CommonFunctions.TrimSupplierName(input3);
                string code = CommonFunctions.TrimSupplierCode(input3);
                int buyerID = Convert.ToInt32(input4);
                string supplierID = DC.ReturnSupplierID(supplierName, buyerID, code).ToString();

                x = "";
                try
                {
                    DT = RDT.Select("[Supplier] = " + supplierID).CopyToDataTable();
                }
                catch
                {
                    DT = new DataTable();
                }

                countMatchedPO = DT.Rows.Count;

                if (countMatchedPO == countEnteredPO)
                    tf = true;
                else
                    tf = false;

                if (!tf)
                {
                    try
                    {
                        DT = RDT.Select("[Supplier] <> " + supplierID).CopyToDataTable();

                        foreach (DataRow DR in DT.Rows)
                        {
                            string pono = DR["PurOrderNo"].ToString();
                            if (!x.Contains(pono))
                                x += pono + ";";
                        }
                    }
                    catch
                    {
                        DT = new DataTable();
                    }
                }
                else
                {
                    x = "NA";
                }

                ret += "-" + tf.ToString(); ;

                i = x.LastIndexOf(";");
                if (i != -1)
                    x = x.Substring(0, i);

                if (string.IsNullOrEmpty(x))
                    x = "NA";

                ret += "-" + x;
                #endregion
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in CheckValidPurchaseOrder WebMethod: " + ss);
            }
            finally
            {
                RDT.Dispose();
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetPOSuppliers(string input1)
        {
            //input1: is the po number as input
            string ret = "";

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                string pono = input1;

                DT = DC.ReturnSupplierDataTableByPONumber(pono);

                if (DT.Rows.Count > 0)
                {
                    ret += "<table id='dataTable1'>";
                    foreach (DataRow DR in DT.Rows)
                    {
                        ret += "<tr><td><span>" + DR[0].ToString() + "</span><span style='display:none;'>" + DR[1].ToString() + "</span></td></tr>";
                    }
                    ret += "</table>";
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in GetPOSuppliers WebMethod: " + ss);
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return ret;
        }

        //Added by Mainak 2017-11-25
        //Get Supplier records by Po Number and Buyer ID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1">PO Number</param>
        /// <param name="input2">Buyer ID</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetSupplierRecordByPONOBID(string input1, int input2)
        {
            //input1: is the po number as input
            //input2: is the buyer ID as input
            string ret = "";

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                string pono = input1;
                int buyerId = input2;

                DT = DC.ReturnSupplierDataTableByPONumberBuyerID(pono, buyerId);

                if (DT.Rows.Count > 0)
                {
                    ret += "<table id='dataTable1'>";
                    foreach (DataRow DR in DT.Rows)
                    {
                        ret += "<tr><td><span>" + DR[0].ToString() + "</span><span style='display:none;'>" + DR[1].ToString() + "</span></td></tr>";
                    }
                    ret += "</table>";
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in GetSupplierRecordByPONOBID WebMethod: " + ss);
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string AttachDocument(string type)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                int pBatchID = Convert.ToInt32(HttpContext.Current.Session["pBatchID"]);
                int pDocID = Convert.ToInt32(HttpContext.Current.Session["pDocID"]);

                int FetchedDocID = CommonFunctions.ReturnPreviousOrNextDocID(pBatchID, pDocID, type);

                DT = DC.AvailableDocumentTable(FetchedDocID);

                bool tf = Convert.ToBoolean(DT.Rows[0]["DocState"].ToString());

                if (!tf)
                    return "NotFound";
                else
                    return "Found";
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in Atc WebMethod: " + ss);
                return "Error";
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchID"></param>
        /// <param name="BatchName"></param>
        /// <param name="FileName"></param>
        /// <param name="DocID"></param>
        /// <param name="CompanyID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string SaveDocumentData(string BatchID, string BatchName, string FileName, string DocID, string CompanyID, string type)
        {
            string ret = "";

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                string COMPANY_NAME = DC.ReturnCompanyNameByBatchID(Convert.ToInt32(BatchID));
                string BATCH_NAME = BatchName;
                string ORIGINAL_NAME = FileName;
                string BatchName_BatchID = BatchName + "_" + BatchID;

                int PNDocID = CommonFunctions.ReturnPreviousOrNextDocID(Convert.ToInt32(BatchID), Convert.ToInt32(DocID), type);
                DT = DC.AvailableDocumentTable(PNDocID);

                string DocType = DT.Rows[0]["DocType"].ToString();
                string NPDocID = DT.Rows[0]["NPDocID"].ToString();
                string ImagePath = @"\\90104-server2\FTP Upload\" + COMPANY_NAME + @"\" + BATCH_NAME + @"\" + ORIGINAL_NAME;
                string ArchiveImagePath = @"\\90107-server3\FTP Archive\" + COMPANY_NAME + @"\" + BatchName_BatchID + @"\" + ORIGINAL_NAME;

                bool tf = DC.SaveDocumentInformation(DocType, NPDocID, CompanyID, ImagePath, ArchiveImagePath);

                //bool tf = true;

                ret = tf.ToString();
            }
            catch
            {
                ret = false.ToString();
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1">BuyerCompanyID</param>
        /// <param name="input2">SupplierCompanyID</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string DefaultsDataSet(string input1, string input2)
        {
            //input1: BuyerCompanyID
            //input2: SupplierCompanyID

            CommonFunctions.CreateLog("Entered into DefaultsDataSet.", "DefaultsLog.txt");

            string ret = "";

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                string BuyerCompanyID = input1;
                string SupplierCompanyID = input2;

                CommonFunctions.CreateLog("BuyerCompanyID: " + BuyerCompanyID, "DefaultsLog.txt");
                CommonFunctions.CreateLog("SupplierCompanyID: " + SupplierCompanyID, "DefaultsLog.txt");

                DT = DC.ReturnDefaultsData(BuyerCompanyID, SupplierCompanyID);

                CommonFunctions.CreateLog("DT.Rows.Count: " + DT.Rows.Count, "DefaultsLog.txt");

                if (DT.Rows.Count > 0)
                {
                    DataRow DR = DT.Rows[0];

                    ret = DR["BuyerCompany"].ToString() + "^" + DR["SupplierCompany"].ToString() + "^" + DR["SupplierCodeAgainstBuyer"].ToString() + "^" + DR["New_VendorClass"].ToString() + "^" + DR["Nominal1"].ToString() + "^" + DR["Nominal2"].ToString() + "^" + DR["New_CurrencyCode"].ToString() + "^" + DR["Nominal1Name"].ToString() + "^" + DR["Nominal2Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                ret = "";
                string ss = "Error:<br />Message: " + ex.Message + "<br />Source: " + ex.Source + "<br />StackTrace: " + ex.StackTrace + "<br />TargetSite: " + ex.TargetSite + "<br />InnerException: " + ex.InnerException + "<br />Data: " + ex.Data;
                HttpContext.Current.Response.Write("<br />Error in DefaultsDataSet WebMethod: " + ss);
                ss = ss.Replace("<br />", "\n");
                CommonFunctions.CreateLog("Error in DefaultsDataSet WebMethod: " + ss, "DefaultsLog.txt");
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            CommonFunctions.CreateLog("Return: " + ret, "DefaultsLog.txt");

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static List<string> CurrencyForDefaults()
        {
            List<string> ret = new List<string>();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                DT = DC.ReturnCurrencyDataTable();

                foreach (DataRow DR in DT.Rows)
                {
                    ret.Add(DR["CurrencyTypeID"].ToString() + "^" + DR["CurrencyCode"].ToString());
                }
            }
            catch (Exception ex)
            {
                ret = new List<string>();
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in CurrencyForDefaults WebMethod: " + ss);
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string[] ListNominalName(string input1, string input2)
        {
            List<string> myList = new List<string>();

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                int CompanyID = Convert.ToInt32(input1.Trim());
                string UserString = input2.Trim();

                DT = DC.ReturnNominalNameDataTable(CompanyID, UserString);

                foreach (DataRow DR in DT.Rows)
                {
                    myList.Add(string.Format("{0}^{1}", DR["NominalCode"].ToString(), DR["NominalCodeName"].ToString()));
                }
            }
            catch (Exception ex)
            {
                myList = new List<string>();
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in ListNominalName WebMethod: " + ss);
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return myList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BuyerCompanyID"></param>
        /// <param name="SupplierCompanyID"></param>
        /// <param name="SupplierCompanyName"></param>
        /// <param name="UserID"></param>
        /// <param name="CurrencyTypeID"></param>
        /// <param name="CurrencyCode"></param>
        /// <param name="VendorID"></param>
        /// <param name="VendorClass"></param>
        /// <param name="Active"></param>
        /// <param name="Nominal1"></param>
        /// <param name="Nominal2"></param>
        /// <param name="PreApprove"></param>
        /// <param name="ApprovalNeeded"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string SaveSupplierDefaults(string BuyerCompanyID, string SupplierCompanyID, string SupplierCompanyName, string UserID, string CurrencyTypeID, string CurrencyCode, string VendorID, string VendorClass, string Active, string Nominal1, string Nominal2, string PreApprove, string ApprovalNeeded)
        {
            string ret = "";

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                bool tf = DC.SaveSupplierDefaults(Convert.ToInt32(BuyerCompanyID), Convert.ToInt32(SupplierCompanyID), SupplierCompanyName, Convert.ToInt32(UserID), Convert.ToInt32(CurrencyTypeID), CurrencyCode, VendorID, VendorClass, Convert.ToBoolean(Active), Nominal1, Nominal2, Convert.ToBoolean(PreApprove), Convert.ToInt32(ApprovalNeeded));

                if (tf)
                    ret = "Success^Saved successfully.";
                else
                    ret = "Failure^Could not save.";
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                ret = "Error^" + ss;
                HttpContext.Current.Response.Write("<br />Error in SaveSupplierDefaults WebMethod: " + ss);
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetAqillaSupplierURL(string input1)
        {
            string ret = "";

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);
            DataTable DT = new DataTable();

            try
            {
                bool tf = CommonFunctions.isNumaric(input1);

                if (tf)
                {
                    int CompanyID = Convert.ToInt32(input1);
                    DT = DC.ReturnAqillaSupplierDetails(CompanyID);

                    if (DT.Rows.Count > 0)
                    {
                        DataRow DR = DT.Rows[0];
                        ret = DR[0].ToString() + "^" + DR[1].ToString() + "^" + DR[2].ToString();

                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ret);
                        ret = System.Convert.ToBase64String(plainTextBytes);

                        if (string.IsNullOrEmpty(DR[0].ToString()))
                            ret = "false^" + ret;
                        else
                            ret = "true^" + ret;
                    }
                }
                else
                    ret = "false";
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                ret = "false";
                HttpContext.Current.Response.Write("<br />Error in GetAqillaSupplierURL WebMethod: " + ss);
            }
            finally
            {
                DT.Dispose();
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1">Base64 String</param>
        /// <param name="input2">BuyerCompanyID</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string ProcessSupplierResponse(string input1, string input2)
        {
            string ret = "";

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);

            try
            {
                #region Convertion of input1
                var base64EncodedBytes = System.Convert.FromBase64String(input1);
                input1 = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                string[] arr = input1.Split('^');

                string urlStr = arr.GetValue(0).ToString();
                string usrStr = arr.GetValue(1).ToString();
                string pwdStr = arr.GetValue(2).ToString();

                CommonFunctions.CreateLog("=======================================", "ProcessSupplierResponseLog.txt");
                CommonFunctions.CreateLog("Aqilla_Supplier_URL: " + urlStr, "ProcessSupplierResponseLog.txt");
                CommonFunctions.CreateLog("Aqilla_Username: " + usrStr, "ProcessSupplierResponseLog.txt");
                CommonFunctions.CreateLog("Aqilla_Password: " + pwdStr, "ProcessSupplierResponseLog.txt");
                #endregion

                #region Request for Supplier list from Aqilla API
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlStr.Trim());
                request.ContentType = "application/xml";
                NetworkCredential nc = new NetworkCredential(usrStr.Trim(), pwdStr.Trim());
                request.Credentials = nc;
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string result = reader.ReadToEnd();

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(result);
                #endregion

                #region Parse XML and populate data in DataTable
                XmlNode dataretrieval = null;
                foreach (XmlNode xn in xd)
                {
                    if (xn.Name.ToLower() == "dataretrieval")
                    {
                        dataretrieval = xn;
                        break;
                    }
                }

                XmlNode entities = null;
                foreach (XmlNode xn in dataretrieval)
                {
                    if (xn.Name.ToLower() == "entities")
                    {
                        entities = xn;
                        break;
                    }
                }

                DataTable DT = new DataTable("Supplier");
                DT.Columns.Add("Supplier_Name");
                DT.Columns.Add("Supplier_Code");
                DT.Columns.Add("Network_ID");

                string Supplier_Name = "";
                string Supplier_Code = "";
                string Network_ID = "";

                DataRow DR;

                foreach (XmlNode xn in entities)
                {
                    if (xn.Name.ToLower() == "entity")
                    {
                        Supplier_Name = xn.ChildNodes[0].InnerText.ToString().Trim().Replace("'", "''");
                        Supplier_Code = xn.ChildNodes[1].InnerText.ToString().Trim().Replace("'", "''");
                        Network_ID = System.Guid.NewGuid().ToString().Substring(0, 11).Trim();

                        DR = DT.NewRow();

                        DR["Supplier_Name"] = Supplier_Name;
                        DR["Supplier_Code"] = Supplier_Code;
                        DR["Network_ID"] = Network_ID;

                        DT.Rows.Add(DR);
                    }
                }
                #endregion

                StringWriter sw = new StringWriter();
                DT.WriteXml(sw);
                result = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" + sw.ToString().Replace("DocumentElement>", "Suppliers>");

                sw.Close();
                sw.Dispose();

                CommonFunctions.CreateLog("Request XML: " + result, "ProcessSupplierResponseLog.txt");

                int BuyerCompanyID = (CommonFunctions.isNumaric(input2) ? Convert.ToInt32(input2) : 0);

                if (BuyerCompanyID > 0)
                {
                    var UserID = HttpContext.Current.Session["UserID"];

                    int ModUserId = (UserID != null || CommonFunctions.isNumaric(UserID)) ? Convert.ToInt32(UserID) : 0;

                    bool RetVal = DC.SAVE_AQILLA_SUPPLIERS(result, BuyerCompanyID, ModUserId);

                    ret = RetVal.ToString();
                }
                else
                {
                    ret = "False";
                }
            }
            catch
            {
                ret = "False";
            }
            finally
            {
                DC.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return ret;
        }
        #endregion
    }

    #region DataCenterActionScanQC Class for all data access functions
    class DataCenterActionScanQC
    {
        SqlConnection sqlConn = null;

        /// <summary>
        /// Constructor of DataCenterActionScanQC Class takes SqlConnection Object
        /// </summary>
        /// <param name="sqlConn">SqlConnection Object</param>
        public DataCenterActionScanQC(SqlConnection sqlConn)
        {
            this.sqlConn = sqlConn;
        }

        #region Dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        System.Runtime.InteropServices.SafeHandle handle = new Microsoft.Win32.SafeHandles.SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// Public implementation of Dispose pattern callable by user.
        /// </summary> 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
        #endregion

        /// <summary>
        /// Returns DataTable for top section
        /// </summary>
        /// <param name="docID"></param>
        /// <returns>DataTable</returns>
        public DataTable ReturnTopDataTable(int docID)
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                sqlDA = new SqlDataAdapter("sp_return_scanQC_top_value", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("@doc_id", docID);

                sqlDA.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnTopDataTable: " + ss);
                return dt;
            }
            finally
            {
                dt.Dispose();
                sqlDA.Dispose();
            }
        }

        /// <summary>
        /// Loads Company list in a dropdown list
        /// </summary>
        /// <param name="ParentCompanyID">The parent company id as int.</param>
        /// <param name="ddlCompany">Company drop down list.</param>
        public void PopulateChildCompany(int ParentCompanyID, DropDownList ddlCompany)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "SELECT [CompanyID], [CompanyName] FROM [Company] WHERE [CompanyID] <> @ParentCompanyID AND [ParentCompanyID] = @ParentCompanyID ORDER BY [CompanyName];";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@ParentCompanyID", SqlDbType.Int).Value = ParentCompanyID;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("Company");
                DA.Fill(DT);

                ddlCompany.DataSource = DT;
                ddlCompany.DataValueField = DT.Columns[0].ToString();
                ddlCompany.DataTextField = DT.Columns[1].ToString();
                ddlCompany.DataBind();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.PopulateChildCompany: " + ss);
            }
            finally
            {
                //ddlCompany.Items.Insert(0, new ListItem("Select Company", "0"));
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable ReturnCurrencyDataTable()
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                //string qry = "SELECT [CurrencyTypeID], ([CurrencyName] + ' (' + [CurrencyCode] + ')') AS [CurrencyNameCode] FROM [CurrencyTypes] ORDER BY [CurrencyCode];";
                string qry = "SELECT [CurrencyTypeID], [CurrencyCode] FROM [CurrencyTypes] ORDER BY [CurrencyCode];";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;

                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("CurrencyTypes");
                DA.Fill(DT);
            }
            catch (Exception ex)
            {
                DT = new DataTable();
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnCurrencyDataTable: " + ss);
            }
            finally
            {
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return DT;
        }

        /// <summary>
        /// Loads Currency list in a dropdown list
        /// </summary>
        /// <param name="ddlCurrency">Currency drop down list</param>
        public void PopulateCurrency(DropDownList ddlCurrency)
        {
            DataTable DT = null;

            try
            {
                DT = ReturnCurrencyDataTable();

                ddlCurrency.DataSource = DT;
                ddlCurrency.DataValueField = DT.Columns[0].ToString();
                ddlCurrency.DataTextField = DT.Columns[1].ToString();
                ddlCurrency.DataBind();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.PopulateCurrency: " + ss);
            }
            finally
            {
                ddlCurrency.Items.Insert(0, new ListItem("", "0"));
                DT.Dispose();
            }
        }

        /// <summary>
        /// Loads Batch Type list in a dropdown list
        /// </summary>
        /// <param name="CompanyID">Company ID as int</param>
        /// <param name="ddlBatchType">Batch Type drop down list</param>
        public void PopulateBatchType(int CompanyID, DropDownList ddlBatchType)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "SELECT [BatchTypeID], [BatchTypeName] FROM [ScanBatchTypes] WHERE [CompanyID] = @CompanyID;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@CompanyID", SqlDbType.Int).Value = CompanyID;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("ScanBatchTypes");
                DA.Fill(DT);

                ddlBatchType.DataSource = DT;
                ddlBatchType.DataValueField = DT.Columns[0].ToString();
                ddlBatchType.DataTextField = DT.Columns[1].ToString();
                ddlBatchType.DataBind();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.PopulateBatchType: " + ss);
            }
            finally
            {
                ddlBatchType.Items.Insert(0, new ListItem("Select Batch Type", "0"));
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }

        /// <summary>
        /// Returns client company id, as int, from client batches table by doc id
        /// </summary>
        /// <param name="doc_id">document id as int.</param>
        /// <returns></returns>
        public int ReturnClientCompanyID(int doc_id)
        {
            int id = 0;

            if (doc_id > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    string qry = "SELECT [CLIENT ID] FROM [CLIENT BATCHES] WHERE [BATCH ID] IN(SELECT [BATCH ID] FROM [DOCUMENT PROGRESS] WHERE [DOC ID] = @doc_id);;";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@doc_id", SqlDbType.Int).Value = doc_id;
                    sqlConn.Open();
                    id = (int)sqlCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnClientCompanyID: " + ss);
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return id;
        }

        /// <summary>
        /// Returns batch type id, as int, from client batches table by doc id
        /// </summary>
        /// <param name="doc_id">document id as int.</param>
        /// <returns></returns>
        public int ReturnBatchTypeID(int doc_id)
        {
            int id = 0;

            if (doc_id > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    string qry = "SELECT [BATCH TYPE ID] FROM [CLIENT BATCHES] WHERE [BATCH ID] IN(SELECT [BATCH ID] FROM [DOCUMENT PROGRESS] WHERE [DOC ID] = @doc_id);";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@doc_id", SqlDbType.Int).Value = doc_id;
                    sqlConn.Open();
                    id = (int)sqlCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnBatchTypeID: " + ss);
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SupplierCompanyName"></param>
        /// <param name="BuyerCompanyID"></param>
        /// <param name="UseOrderBy"></param>
        /// <param name="SupplierCodeAgainstBuyer"></param>
        /// <returns></returns>
        public DataTable ReturnSupplierDataTable(string SupplierCompanyName, int BuyerCompanyID, string SupplierCodeAgainstBuyer = "", bool UseOrderBy = false)
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DataTable DT = new DataTable();

            try
            {
                DA = new SqlDataAdapter("sp_SupplierCompanyNameWithCodeAgainstBuyer", sqlConn);
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DA.SelectCommand.Parameters.AddWithValue("@SupplierCompanyName", SupplierCompanyName);
                DA.SelectCommand.Parameters.AddWithValue("@SupplierCodeAgainstBuyer", SupplierCodeAgainstBuyer);
                DA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", BuyerCompanyID);
                DA.SelectCommand.Parameters.AddWithValue("@UseOrderBy", UseOrderBy);

                sqlConn.Open();
                DA.Fill(DT);

                return DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.CheckValidSupplier: " + ss);

                return new DataTable();
            }
            finally
            {
                DT.Dispose();
                DA.Dispose();
            }
        }

        /// <summary>
        /// Return, as string, valid supplier company name by input of supplier name
        /// </summary>
        /// <param name="SupplierName">input supplier name as string.</param>
        /// <returns></returns>
        public string ReturnValidSupplierCompanyBySupplierName(string SupplierName, int buyerID)
        {
            string RetSupplier = "";

            SqlCommand sqlCmd = null;
            try
            {
                string qry = "SELECT ([c].[CompanyName]+ ' (' + [tr].[SupplierCodeAgainstBuyer] + ')') AS [SupplierCompanyNameWithCodeAgainstBuyer]" +
                            " FROM [TradingRelation] [tr], [Company] [c] " +
                            " WHERE [tr].[SupplierCompanyID] = [c].[CompanyID] AND [c].[Active] = 'true' " +
                            " AND [tr].[Active] = 'true' AND [tr].[SupplierDeleted] = 'false' " +
                            " AND [c].[CompanyName] = '' + @SupplierCompanyName + '' " +
                            " AND [tr].[BuyerCompanyID] = @BuyerCompanyID;";

                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@SupplierCompanyName", SqlDbType.VarChar).Value = SupplierName.Trim();
                sqlCmd.Parameters.AddWithValue("@BuyerCompanyID", SqlDbType.Int).Value = buyerID;
                sqlConn.Open();
                RetSupplier = Convert.ToString(sqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnValidSupplierCompanyBySupplierName: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return RetSupplier;
        }

        /// <summary>
        /// Return, as string, supplier company name by po number.
        /// </summary>
        /// <param name="PONumber">input po number as string</param>
        /// <returns>DataTable</returns>
        public DataTable ReturnSupplierDataTableByPONumber(string PONumber)
        {
            if (PONumber.Length > 0)
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter("sp_ReturnSupplierByPONumber", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("@PONumber", PONumber);
                DataTable DT = new DataTable("SupplierDataTableByPONumber");
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(DT);

                    return DT;
                }
                catch (Exception ex)
                {
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnSupplierDataTableByPONumber: " + ss);
                    return new DataTable();
                }
                finally
                {
                    DT.Dispose();
                    sqlDA.Dispose();
                    sqlConn.Close();
                }
            }
            else
            {
                return new DataTable();
            }
        }

        //Added by Mainak 2017-11-25
        /// <summary>
        /// Return, as string, supplier company name by po number and buyer Id.
        /// </summary>
        /// <param name="PONumber">input po number as string and buyer Id as integer</param>
        /// <returns>DataTable</returns>
        public DataTable ReturnSupplierDataTableByPONumberBuyerID(string PONumber, int BuyerID)
        {
            if (PONumber.Length > 0)
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter("sp_ReturnSupplierByPONumberBuyerID", sqlConn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("@PONumber", PONumber);
                sqlDA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", BuyerID);
                DataTable DT = new DataTable("SupplierDataTableByPONumberBuyerID");
                try
                {
                    sqlConn.Open();
                    sqlDA.Fill(DT);

                    return DT;
                }
                catch (Exception ex)
                {
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnSupplierDataTableByPONumberBuyerID: " + ss);
                    return new DataTable();
                }
                finally
                {
                    DT.Dispose();
                    sqlDA.Dispose();
                    sqlConn.Close();
                }
            }
            else
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// Return currency id as int, by currency name.
        /// </summary>
        /// <param name="CurrencyCode">input Currency Code name.</param>
        /// <returns></returns>
        public int ReturnCurrencyIDByCode(string CurrencyCode)
        {
            int CurrencyID = 0;

            SqlCommand sqlCmd = null;
            try
            {
                if (!string.IsNullOrEmpty(CurrencyCode))
                {
                    string qry = "SELECT [CurrencyTypeID] FROM [CurrencyTypes] WHERE [CurrencyCode] = '' + @input + '';";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@input", SqlDbType.VarChar).Value = CurrencyCode.Trim();
                    sqlConn.Open();
                    CurrencyID = (int)sqlCmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnCurrencyIDByCode: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return CurrencyID;
        }

        /// <summary>
        /// Return Currency Code as string
        /// </summary>
        /// <param name="SupplierCompanyID">Supplier company id as int.</param>
        /// <param name="BuyerCompanyID">Buyer company id as int.</param>
        /// <returns></returns>
        public string ReturnNewCurrencyCode(int SupplierCompanyID, int BuyerCompanyID)
        {
            string New_CurrencyCode = "";

            SqlCommand sqlCmd = null;
            try
            {
                string qry = "SELECT [New_CurrencyCode] FROM [TradingRelation] WHERE [Active] = 'true' AND [SupplierCompanyID] = @SupplierCompanyID AND [BuyerCompanyID] = @BuyerCompanyID;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@SupplierCompanyID", SqlDbType.Int).Value = SupplierCompanyID;
                sqlCmd.Parameters.AddWithValue("@BuyerCompanyID", SqlDbType.Int).Value = BuyerCompanyID;
                sqlConn.Open();

                New_CurrencyCode = Convert.ToString(sqlCmd.ExecuteScalar());

                New_CurrencyCode = (string.IsNullOrEmpty(New_CurrencyCode) == true) ? "NA" : New_CurrencyCode;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnNewCurrencyCode: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return New_CurrencyCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrencyID"></param>
        /// <returns></returns>
        public string ReturnCurrencyCodeByID(int CurrencyID)
        {
            string CurrencyCode = "NA";

            SqlCommand sqlCmd = null;
            try
            {
                string qry = "SELECT [CurrencyCode] FROM [CurrencyTypes] WHERE [CurrencyTypeID] = @input;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input", SqlDbType.Int).Value = CurrencyID;
                sqlConn.Open();
                CurrencyCode = (string)sqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnCurrencyCodeByID: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return CurrencyCode;
        }

        /// <summary>
        /// Update being edited value as per the doc id in [document progress] table.
        /// </summary>
        /// <param name="status">Status 'Yes' or 'NO' as string.</param>
        /// <param name="docID">Document ID as int.</param>
        /// <returns></returns>
        public bool UpdateBeingEdited(string status, int docID)
        {
            bool TF = false;
            SqlCommand sqlCmd = null;

            try
            {
                string qry = "UPDATE [DOCUMENT PROGRESS] SET [BEING EDITED] = '' + @input1 + '' WHERE [DOC ID] = @input2;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input1", SqlDbType.VarChar).Value = status.Trim();
                sqlCmd.Parameters.AddWithValue("@input2", SqlDbType.Int).Value = docID;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                TF = true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.UpdateBeingEdited: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return TF;
        }

        /// <summary>
        /// Return User Name as string by user id
        /// </summary>
        /// <param name="UserID">input user id as int.</param>
        /// <returns></returns>
        public string ReturnUserName(int UserID)
        {
            string UserName = "";

            SqlCommand sqlCmd = null;
            try
            {
                string qry = "SELECT [UserName] FROM [Users] WHERE [UserID] = '' + @input + '';";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input", SqlDbType.Int).Value = UserID;
                sqlConn.Open();
                UserName = sqlCmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnUserName: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return UserName;
        }

        /// <summary>
        /// Return a table of Document IDs as per batch id where [BEING EDITED] = 'NO'
        /// </summary>
        /// <param name="batchid">input batch id as int.</param>
        /// <param name="checkBeingEditedNo">additional checking of [BEING EDITED] = 'NO'</param>
        /// <returns></returns>
        public DataTable ReturnDOCIDs_DT(int batchid, bool checkBeingEditedNo)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "";

                switch (checkBeingEditedNo)
                {
                    case true:
                        qry = "SELECT [DOC ID] FROM [DOCUMENT PROGRESS] " +
                                " WHERE ISNULL([BEING EDITED], 'NO') = 'NO' " +
                                " AND [FINAL ARCHIVING DATE] IS NULL " +
                                " AND [DELETE DATE] IS NULL " +
                                " AND [BATCH ID] = @input ORDER BY [DOC ID] ASC;";
                        break;
                    case false:
                        qry = "SELECT [DOC ID] FROM [DOCUMENT PROGRESS] " +
                                " WHERE [FINAL ARCHIVING DATE] IS NULL " +
                                " AND [DELETE DATE] IS NULL " +
                                " AND [BATCH ID] = @input ORDER BY [DOC ID] ASC;";
                        break;
                }

                //HttpContext.Current.Response.Write(qry);
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input", SqlDbType.Int).Value = batchid;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("DOCUMENT_PROGRESS");
                DA.Fill(DT);

                return DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnDOCIDs_DT: " + ss);
                return null;
            }
            finally
            {
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }

        /// <summary>
        /// Return a table of validation result set as per batch type id as input.
        /// </summary>
        /// <param name="BatchTypeID">input batch type id as int.</param>
        /// <returns></returns>
        public DataTable ReturnValidity_DT(int BatchTypeID)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "SELECT ISNULL([LineItems], 0) AS [isLineItem], ISNULL([PO], 0) AS [isPO], ISNULL([Description], 0) AS [isDescription] FROM [ScanBatchTypes] WHERE [BatchTypeID] = @input;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input", SqlDbType.Int).Value = BatchTypeID;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("DOCUMENT_PROGRESS");
                DA.Fill(DT);

                return DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnValidity_DT: " + ss);
                return DT;
            }
            finally
            {
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }

        /// <summary>
        /// Update [DOCUMENT PROGRESS] to make a document deleted virtually
        /// Set [BEING EDITED] = <'NO'>, [FINAL ARCHIVING DATE] = <current datetime>, [DELETE DATE] = <current datetime>
        /// Update no of invoice deleted in [CLIENT BATCHES] table
        /// </summary>
        /// <param name="docID">document id as int.</param>
        /// <param name="batchID">batch id as int.</param>
        /// <returns></returns>
        public bool MakeDocumentDeleted(int docID, int batchID)
        {
            bool TF = false;

            SqlCommand sqlCmd = null;
            SqlTransaction st = null;
            try
            {
                string qry = "UPDATE [DOCUMENT PROGRESS] SET [BEING EDITED] = 'NO', [FINAL ARCHIVING DATE] = GETDATE(), [DELETE DATE] = GETDATE() WHERE [DOC ID] = @input1;";
                qry += " UPDATE [CLIENT BATCHES] SET [NUM OF INVOICES DELETED] = (ISNULL([NUM OF INVOICES DELETED], 0) + 1) WHERE [BATCH ID] = @input2";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input1", SqlDbType.Int).Value = docID;
                sqlCmd.Parameters.AddWithValue("@input2", SqlDbType.Int).Value = batchID;
                sqlConn.Open();
                st = sqlConn.BeginTransaction("TransactionUpdate");
                sqlCmd.Transaction = st;
                sqlCmd.ExecuteNonQuery();
                st.Commit();

                TF = true;
            }
            catch (Exception ex)
            {
                st.Rollback();
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.MakeDocumentDeleted: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
                st.Dispose();
            }

            return TF;
        }

        /// <summary>
        /// Update [DOCUMENT PROGRESS]'s final archiving date as per doc id.
        /// </summary>
        /// <param name="docID">input doc id as int.</param>
        /// <returns></returns>
        public bool UpdateFinalArchivingDate(int docID)
        {
            bool TF = false;

            SqlCommand sqlCmd = null;
            try
            {
                string qry = "UPDATE [DOCUMENT PROGRESS] SET [FINAL ARCHIVING DATE] = GETDATE() WHERE [DOC ID] = @input;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input", SqlDbType.Int).Value = docID;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                TF = true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.UpdateFinalArchivingDate: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return TF;
        }

        /// <summary>
        /// Update Number Of Invoices Archived By QC as per the batch id.
        /// </summary>
        /// <param name="batchID">input batch id as int.</param>
        /// <returns></returns>
        public bool UpdateNumOfInvoicesArchivedByQC(int batchID)
        {
            bool TF = false;

            SqlCommand sqlCmd = null;
            try
            {
                string qry = "UPDATE [CLIENT BATCHES] SET [NUM OF INVOICES ARCHIVED BY QC] = (ISNULL([NUM OF INVOICES ARCHIVED BY QC], 0) + 1) WHERE [BATCH ID] = @input;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input", SqlDbType.Int).Value = batchID;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                TF = true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.UpdateNumOfInvoicesArchivedByQC: " + ss);
            }
            finally
            {
                sqlConn.Close();
                sqlCmd.Dispose();
            }

            return TF;
        }

        /// <summary>
        /// Return supplier id by supplier name as string.
        /// </summary>
        /// <param name="SupplierName">input supplier name as string.</param>
        /// <returns></returns>
        public int ReturnSupplierID(string SupplierName, int buyerID, string CodeAgainstBuyer)
        {
            int id = 0;

            if (SupplierName.Length > 0)
            {
                SqlCommand sqlCmd = new SqlCommand();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                DataTable DT = new DataTable();

                try
                {
                    string qry = "SELECT [tr].[SupplierCompanyID] " +
                    "FROM [TradingRelation] [tr], [Company] [c] " +
                    "WHERE [tr].[SupplierCompanyID] = [c].[CompanyID] " +
                    "AND [c].[CompanyName] = '' + @Supplier + '' " +
                    "AND [tr].[BuyerCompanyID] = @BuyerID " +
                    "AND [tr].[Active] = 'true' " +
                    "AND [tr].[SupplierDeleted] = 'false' ";

                    if (CodeAgainstBuyer.Length > 0)
                        qry += "AND [tr].[SupplierCodeAgainstBuyer] = '' + @CodeAgainstBuyer + '' ";

                    qry += ";";

                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@Supplier", SqlDbType.VarChar).Value = SupplierName;
                    sqlCmd.Parameters.AddWithValue("@BuyerID", SqlDbType.Int).Value = buyerID;
                    if (CodeAgainstBuyer.Length > 0)
                        sqlCmd.Parameters.AddWithValue("@CodeAgainstBuyer", SqlDbType.VarChar).Value = CodeAgainstBuyer;
                    sqlConn.Open();
                    //id = (int)sqlCmd.ExecuteScalar();

                    sqlDA = new SqlDataAdapter(sqlCmd);
                    sqlDA.Fill(DT);

                    if (DT.Rows.Count > 0)
                    {
                        DataRow DR = DT.Rows[0];
                        id = Convert.ToInt32(DR[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnSupplierID: " + ss);
                }
                finally
                {
                    sqlDA.Dispose();
                    DT.Dispose();
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return id;
        }

        /// <summary>
        /// Return Batch Doc Type Value as per batch type id
        /// </summary>
        /// <param name="BatchTypeID">input batch type id.</param>
        /// <returns></returns>
        public string ReturnBatchDocTypeValue(int BatchTypeID, string BuyerCompanyID = null, string OrderNo = null)
        {
            string BatchDocType = "";

            if (BatchTypeID > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    //string qry = "SELECT [BatchDocType] FROM [ScanBatchTypes] WHERE [BatchTypeID] = @BatchTypeID;";
                    //sqlCmd = new SqlCommand(qry, sqlConn);
                    //sqlCmd.CommandType = CommandType.Text;
                    //sqlCmd.Parameters.AddWithValue("@BatchTypeID", SqlDbType.Int).Value = BatchTypeID;
                    //sqlConn.Open();
                    //BatchDocType = sqlCmd.ExecuteScalar().ToString();

                    //Added by Mainak 2017-11-21
                    sqlConn.Open();
                    sqlCmd = new SqlCommand("SP_getSecondPartOfType", sqlConn);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@BatchTypeID", Convert.ToInt32(BatchTypeID));
                    sqlCmd.Parameters.AddWithValue("@BuyerCompanyID", Convert.ToInt32(BuyerCompanyID));
                    sqlCmd.Parameters.AddWithValue("@OrderNo", OrderNo);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlCmd;

                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    sqlConn.Close();

                    BatchDocType = (ds.Tables[0].Rows[0]["FinalVal"] as object == DBNull.Value) ? "" : Convert.ToString(ds.Tables[0].Rows[0]["FinalVal"]);
                }
                catch (Exception ex)
                {
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnBatchDocTypeValue: " + ss);
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return BatchDocType;
        }

        /// <summary>
        /// Takes a int value of Doc ID and returns the DataTable with columns like [ORIGINAL NAME], [CLIENT ID], [CompanyName] 
        /// </summary>
        /// <param name="comID">comID the Selected company's id as int.</param>
        /// <param name="docID">docID the Document No as int.</param>
        /// <returns>Returns DataTable</returns>
        public DataTable PathInformationTable(int comID, int docID)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "SELECT dp.[ORIGINAL NAME], cb.[CLIENT ID], cb.[BATCH NAME], c.CompanyName " +
                "FROM [DOCUMENT PROGRESS] dp, [CLIENT BATCHES] cb, [Company] c " +
                "WHERE dp.[BATCH ID] = cb.[BATCH ID] AND cb.[CLIENT ID] = c.CompanyID " +
                "AND cb.[CLIENT ID] = @comID AND dp.[DOC ID]= @docID;";

                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.Add("@comID", SqlDbType.Int).Value = comID;
                sqlCmd.Parameters.Add("@docID", SqlDbType.Int).Value = docID;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("document");
                DA.Fill(DT);

                return DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.PathInformationTable: " + ss);

                return DT;
            }
            finally
            {
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchID"></param>
        /// <returns></returns>
        public string ReturnCompanyNameByBatchID(int BatchID)
        {
            string CompanyName = "";

            if (BatchID > 0)
            {
                SqlCommand sqlCmd = null;
                try
                {
                    string qry = "SELECT [COMPANYNAME] FROM [COMPANY] WHERE [COMPANYID] = " +
                                "(SELECT [CLIENT ID] FROM [CLIENT BATCHES] WHERE [BATCH ID] = @BatchID);";
                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@BatchID", SqlDbType.Int).Value = BatchID;
                    sqlConn.Open();
                    CompanyName = sqlCmd.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnCompanyNameByBatchID: " + ss);
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return CompanyName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchid"></param>
        /// <returns></returns>
        public DataTable ReturnAllDOCIDsDTInBatch(int batchid)
        {
            SqlCommand sqlCmd = null;
            DataTable DT = null;
            SqlDataAdapter DA = null;
            try
            {
                string qry = "SELECT [DOC ID] FROM [DOCUMENT PROGRESS] WHERE [BATCH ID] = '' + @input + '';";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@input", SqlDbType.Int).Value = batchid;
                sqlConn.Open();
                DA = new SqlDataAdapter(sqlCmd);
                DT = new DataTable("DOCUMENT_PROGRESS");
                DA.Fill(DT);

                return DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnAllDOCIDsDTInBatch: " + ss);
                return null;
            }
            finally
            {
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
                sqlCmd.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PurOrderNo"></param>
        /// <returns></returns>
        public DataTable ReturnBuyerProdCodeTableForPONumber(string PurOrderNo)
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DataTable DT = new DataTable();
            try
            {
                string sqlqry = "SELECT ISNULL([ProdCodeOrderID], 0) AS [ProdCodeOrderID], ISNULL([CompanyID], 0) AS [CompanyID], [BuyerProdCode], [PurOrderNo], [Color], [ModDate], ISNULL([Supplier], 0) AS [Supplier] FROM [BuyerProdCode] WHERE [PurOrderNo] = '' + @PONumber + ''";
                DA = new SqlDataAdapter(sqlqry, sqlConn);
                DA.SelectCommand.CommandType = CommandType.Text;
                DA.SelectCommand.Parameters.AddWithValue("@PONumber", PurOrderNo);
                DA.Fill(DT);
                return DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnBuyerProdCodeTableForPONumber: " + ss);
                return new DataTable();
            }
            finally
            {
                DT.Dispose();
                DA.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchID"></param>
        /// <param name="DocID"></param>
        /// <returns></returns>
        public string DocumentPosition(int BatchID, int DocID)
        {
            string ret = "";

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter DA = new SqlDataAdapter();
            DataTable DT = new DataTable();

            try
            {
                string qry = "SELECT * FROM(" +
                            "SELECT (ROW_NUMBER() OVER (ORDER BY [DOC ID])) AS [DOC NO], [DOC ID] " +
                            ", (SELECT COUNT([DOC ID]) FROM [DOCUMENT PROGRESS] WHERE [BATCH ID] = @BatchID) AS [TOT NO]" +
                            "FROM [DOCUMENT PROGRESS] WHERE [BATCH ID] = @BatchID" +
                            ") AS [TB1] WHERE [DOC ID] = @DocID";

                cmd = new SqlCommand(qry, sqlConn);
                cmd.Parameters.AddWithValue("@BatchID", BatchID);
                cmd.Parameters.AddWithValue("@DocID", DocID);
                DA = new SqlDataAdapter(cmd);
                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    int CurRowNum = Convert.ToInt32(DT.Rows[0]["DOC NO"].ToString());
                    int TotRowNum = Convert.ToInt32(DT.Rows[0]["TOT NO"].ToString());

                    if (CurRowNum == 1)
                        ret = "First";
                    if (CurRowNum == TotRowNum)
                        ret = "Last";
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.DocumentPosition: " + ss);
            }
            finally
            {
                DT.Dispose();
                DA.Dispose();
                cmd.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// This method is used to return if docid exists in the database.
        /// </summary>
        /// <param name="DocID">Document ID</param>
        /// <returns>data table</returns>
        public DataTable AvailableDocumentTable(int DocID)
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DataTable DT = new DataTable();
            try
            {
                DA = new SqlDataAdapter("sp_Invoice_CreditNote_State", sqlConn);
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DA.SelectCommand.Parameters.AddWithValue("@DocID", DocID);

                DA.Fill(DT);
            }
            catch (Exception ex)
            {
                DT = new DataTable();
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.DocumentState: " + ss);
            }
            finally
            {
                DT.Dispose();
                DA.Dispose();
            }

            return DT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DocType"></param>
        /// <param name="DocID"></param>
        /// <param name="CompanyID"></param>
        /// <param name="ImagePath"></param>
        /// <param name="ArchiveImagePath"></param>
        /// <returns></returns>
        public bool SaveDocumentInformation(string DocType, string DocID, string CompanyID, string ImagePath, string ArchiveImagePath)
        {
            bool tf = false;
            SqlCommand cmd = new SqlCommand();

            try
            {
                string sql = "";

                switch (DocType)
                {
                    case "Invoice":
                        sql = "INSERT INTO [UploadDocument] ([InvoiceID], [CompanyID], [ImagePath], [ArchiveImagePath]) VALUES (@DocID, @CompanyID, @ImagePath, @ArchiveImagePath);";
                        break;
                    case "Credit Note":
                        sql = "INSERT INTO [UploadDocument_CN] ([CreditNoteID], [CompanyID], [ImagePath], [ArchiveImagePath]) VALUES (@DocID, @CompanyID, @ImagePath, @ArchiveImagePath);";
                        break;
                }

                cmd = new SqlCommand(sql, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@DocID", DocID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                cmd.Parameters.AddWithValue("@ArchiveImagePath", ArchiveImagePath);

                sqlConn.Open();
                cmd.ExecuteNonQuery();

                tf = true;
            }
            catch (Exception ex)
            {
                tf = false;
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.SaveDocumentInformation: " + ss);
            }
            finally
            {
                cmd.Dispose();
                sqlConn.Close();
            }

            return tf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public string ReturnCompanyNameByID(int CompanyID)
        {
            string CompanyName = "";

            if (CompanyID > 0)
            {
                SqlCommand sqlCmd = new SqlCommand();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                DataTable DT = new DataTable();

                try
                {
                    string qry = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = @CompanyID;";

                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@CompanyID", SqlDbType.Int).Value = CompanyID;
                    sqlConn.Open();
                    //id = (int)sqlCmd.ExecuteScalar();

                    sqlDA = new SqlDataAdapter(sqlCmd);
                    sqlDA.Fill(DT);

                    if (DT.Rows.Count > 0)
                    {
                        DataRow DR = DT.Rows[0];
                        CompanyName = DR[0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    CompanyName = "";
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnCompanyNameByID: " + ss);
                }
                finally
                {
                    sqlDA.Dispose();
                    DT.Dispose();
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return CompanyName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool UpdateClerkIdEditing(int docID, int userID)
        {
            SqlCommand sqlCMD = new SqlCommand();

            try
            {
                string sqlQry = "UPDATE [DOCUMENT PROGRESS] SET [CLERK ID EDITING] = @UserID WHERE [DOC ID] = @DocID;";
                sqlCMD = new SqlCommand(sqlQry, sqlConn);
                sqlCMD.CommandType = CommandType.Text;
                sqlCMD.Parameters.AddWithValue("@UserID", userID);
                sqlCMD.Parameters.AddWithValue("@DocID", docID);

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.UpdateClertIdEditing: " + ss);
                return false;
            }
            finally
            {
                sqlCMD.Dispose();
                sqlConn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BuyerCompanyID"></param>
        /// <param name="SupplierCompanyID"></param>
        /// <returns></returns>
        public DataTable ReturnDefaultsData(string BuyerCompanyID, string SupplierCompanyID)
        {
            DataTable DT = new DataTable("DEFAULTS_DATA");
            SqlDataAdapter DA = new SqlDataAdapter("PRO_RETURN_DEFAULTS_DATA", sqlConn);

            try
            {
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DA.SelectCommand.Parameters.AddWithValue("@BuyerCompanyID", BuyerCompanyID);
                DA.SelectCommand.Parameters.AddWithValue("@SupplierCompanyID", SupplierCompanyID);

                sqlConn.Open();
                DA.Fill(DT);

                return DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnDefaultsData: " + ss);
                return new DataTable();
            }
            finally
            {
                DT.Dispose();
                DA.Dispose();
                sqlConn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID">BuyerCompanyID</param>
        /// <param name="UserString">any data</param>
        /// <returns></returns>
        public DataTable ReturnNominalNameDataTable(int CompanyID, string UserString)
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DataTable DT = new DataTable();

            try
            {
                DA = new SqlDataAdapter("PRO_RETURN_NOMINAL_DATA", sqlConn);
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DA.SelectCommand.Parameters.AddWithValue("@CompanyID", CompanyID);
                DA.SelectCommand.Parameters.AddWithValue("@NominalStr", UserString);

                sqlConn.Open();
                DA.Fill(DT);
            }
            catch (Exception ex)
            {
                DT = new DataTable();
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnNominalNameDataTable: " + ss);
            }
            finally
            {
                DA.Dispose();
                DT.Dispose();
                sqlConn.Close();
            }

            return DT;
        }

        /// <summary>
        /// Saves the details to the database (TradingRelation table)
        /// </summary>
        /// <param name="BuyerCompanyID">Integer value of Buyer Company's ID.</param>
        /// <param name="SupplierCompanyID">Integer value of Supplier Company's ID.</param>
        /// <param name="SupplierCompanyName">String value of Supplier Company's Name.</param>
        /// <param name="UserID">Logged in user's ID from session.</param>
        /// <param name="CurrencyTypeID">Integer value of the selected currency's ID.</param>
        /// <param name="CurrencyCode">Sting value of the selected currency name.</param>
        /// <param name="VendorID">String value of the VendorID field.</param>
        /// <param name="VendorClass">String value of the VendorClass field.</param>
        /// <param name="Active">Set boolean true/false</param>
        /// <param name="Nominal1">String value.</param>
        /// <param name="Nominal2">String value.</param>
        /// <param name="PreApprove">true/false</param>
        /// <param name="ApprovalNeeded">Integer value 0/1.</param>
        /// <returns>Boolean (true/false)</returns>
        public bool SaveSupplierDefaults(int BuyerCompanyID, int SupplierCompanyID, string SupplierCompanyName, int UserID, int CurrencyTypeID, string CurrencyCode, string VendorID, string VendorClass, bool Active, string Nominal1, string Nominal2, bool PreApprove, int ApprovalNeeded)
        {
            bool tf = false;

            SqlCommand cmd = new SqlCommand();

            try
            {
                string sqlQuery = "PRO_SAVE_TRADINGRELATION_SUPPLIERDETAILS";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BuyerCompanyID", BuyerCompanyID);
                cmd.Parameters.AddWithValue("@SupplierCompanyID", SupplierCompanyID);
                cmd.Parameters.AddWithValue("@SupplierCompanyName", SupplierCompanyName);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@CurrencyTypeID", CurrencyTypeID);
                cmd.Parameters.AddWithValue("@CurrencyCode", CurrencyCode);
                cmd.Parameters.AddWithValue("@VendorID", VendorID);
                cmd.Parameters.AddWithValue("@VendorClass", VendorClass);
                cmd.Parameters.AddWithValue("@Active", Active);
                cmd.Parameters.AddWithValue("@Nominal1", Nominal1);
                cmd.Parameters.AddWithValue("@Nominal2", Nominal2);
                cmd.Parameters.AddWithValue("@PreApprove", PreApprove);
                cmd.Parameters.AddWithValue("@ApprovalNeeded", ApprovalNeeded);

                sqlConn.Open();
                cmd.ExecuteNonQuery();

                tf = true;
            }
            catch (Exception ex)
            {
                tf = false;
                string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.SaveSupplierDefaults: " + ss);
            }
            finally
            {
                sqlConn.Close();
            }

            return tf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public DataTable ReturnAqillaSupplierDetails(int CompanyID)
        {
            DataTable DT = new DataTable();

            if (CompanyID > 0)
            {
                SqlCommand sqlCmd = new SqlCommand();
                SqlDataAdapter sqlDA = new SqlDataAdapter();

                try
                {
                    string qry = "SELECT DISTINCT [Aqilla_Supplier_URL], [Aqilla_Username], [Aqilla_Password] FROM [Company] WHERE [CompanyID] = @CompanyID;";

                    sqlCmd = new SqlCommand(qry, sqlConn);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@CompanyID", SqlDbType.Int).Value = CompanyID;
                    sqlConn.Open();
                    //id = (int)sqlCmd.ExecuteScalar();

                    sqlDA = new SqlDataAdapter(sqlCmd);
                    sqlDA.Fill(DT);
                }
                catch (Exception ex)
                {
                    DT = new DataTable();
                    string ss = ex.Message + "<br />" + ex.Source + "<br />" + ex.StackTrace + "<br />" + ex.TargetSite + "<br />" + ex.InnerException;
                    HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.ReturnAqillaSupplierDetails: " + ss);
                }
                finally
                {
                    sqlDA.Dispose();
                    DT.Dispose();
                    sqlConn.Close();
                    sqlCmd.Dispose();
                }
            }

            return DT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SupplierXML">XML Input for supplier list.</param>
        /// <param name="BuyerCompanyID">The buyer company ID.</param>
        /// <returns></returns>
        public bool SAVE_AQILLA_SUPPLIERS(string SupplierXML, int BuyerCompanyID, int ModUserId)
        {
            bool tf = false;
            SqlCommand cmd = new SqlCommand();

            try
            {
                string sql = "PRO_SAVE_AQILLA_SUPPLIERS";
                int ReturnValue = 0;

                cmd = new SqlCommand(sql, sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@xmlstr", SupplierXML);
                cmd.Parameters.AddWithValue("@BuyerCompanyID", BuyerCompanyID);
                cmd.Parameters.AddWithValue("@ModUserId", ModUserId);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                cmd.Parameters[3].Direction = ParameterDirection.Output;

                sqlConn.Open();
                cmd.ExecuteNonQuery();
                ReturnValue = (int)cmd.Parameters[3].Value;

                if (ReturnValue == 0)
                    tf = true;
                else
                    tf = false;

                CommonFunctions.CreateLog("PRO_SAVE_AQILLA_SUPPLIERS returned: " + tf.ToString(), "ProcessSupplierResponseLog.txt");
            }
            catch (Exception ex)
            {
                tf = false;
                string ss = "Error:<br />Message: " + ex.Message + "<br />Source: " + ex.Source + "<br />StackTrace: " + ex.StackTrace + "<br />TargetSite: " + ex.TargetSite + "<br />InnerException: " + ex.InnerException + "<br />Data: " + ex.Data;
                //Response.Write(ss);

                HttpContext.Current.Response.Write("<br />Error in DataCenterActionScanQC.SAVE_AQILLA_SUPPLIERS: " + ss);

                ss = ss.Replace("<br />", "\n");
                CommonFunctions.CreateLog("Error in DataCenterActionScanQC.SAVE_AQILLA_SUPPLIERS: " + ss, "ProcessSupplierResponseLog.txt");
            }
            finally
            {
                cmd.Dispose();
                sqlConn.Close();
            }

            if (tf)
                CommonFunctions.CreateLog("SAVE_AQILLA_SUPPLIERS is Successful.", "ProcessSupplierResponseLog.txt");
            else
                CommonFunctions.CreateLog("SAVE_AQILLA_SUPPLIERS is not Successful.", "ProcessSupplierResponseLog.txt");

            CommonFunctions.CreateLog("=======================================", "ProcessSupplierResponseLog.txt");

            return tf;
        }
    }
    #endregion

    #region Common Functions Class
    static class CommonFunctions
    {
        /// <summary>
        /// Trim supplier name as it is concatinated in the txtSupplier text box.
        /// </summary>
        /// <param name="SupplierNameWithCode">value for Supplier name.</param>
        /// <returns></returns>
        public static string TrimSupplierName(string SupplierNameWithCode)
        {
            try
            {
                string v = SupplierNameWithCode;

                int x = v.LastIndexOf("(");
                int y = v.LastIndexOf(")");

                int l = y - x + 1;

                string str = v.Substring(x, l);

                str = v.Replace(" " + str, "");

                return str;
            }
            catch
            {
                return SupplierNameWithCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SupplierNameWithCode"></param>
        /// <returns></returns>
        public static string TrimSupplierCode(string SupplierNameWithCode)
        {
            try
            {
                string v = SupplierNameWithCode;

                int x = v.LastIndexOf("(");
                int y = v.LastIndexOf(")");

                x = x + 1;
                int l = y - x;

                string str = v.Substring(x, l);

                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchID"></param>
        /// <param name="docID"></param>
        /// <param name="ReturnType"></param>
        /// <returns></returns>
        public static int ReturnPreviousOrNextDocID(int batchID, int docID, string ReturnType)
        {
            int ret = 0;

            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            DataCenterActionScanQC DC = new DataCenterActionScanQC(sqlConn);

            int xDocID = 0;
            int i = 0;

            DataTable DT = DC.ReturnAllDOCIDsDTInBatch(batchID);

            for (i = 0; i < DT.Rows.Count; i++)
            {
                DataRow DR = DT.Rows[i];

                xDocID = Convert.ToInt32(DR[0].ToString());

                if (docID == xDocID)
                    break;
            }

            switch (ReturnType)
            {
                case "prev":
                    if (i > 0)
                        ret = Convert.ToInt32(DT.Rows[i - 1][0].ToString());
                    break;
                case "next":
                    if (i < DT.Rows.Count - 1)
                        ret = Convert.ToInt32(DT.Rows[i + 1][0].ToString());
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public static string ReturnParsedStringForXML(string InputString)
        {
            string[] searchArray = { "&", ">", "<", "\"", "'" };
            string[] changeArray = { "&amp;", "&gt;", "&lt;", "&quot;", "&apos;" };

            string OutputString = InputString;
            int i = 0;
            int c = searchArray.Length;

            string searchStr, changeStr;
            searchStr = changeStr = "";

            for (i = 0; i < c; i++)
            {
                searchStr = searchArray.GetValue(i).ToString();
                changeStr = changeArray.GetValue(i).ToString();

                OutputString = OutputString.Replace(searchStr, changeStr);
            }

            return OutputString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool isNumaric(dynamic val)
        {
            try
            {
                int x = Convert.ToInt32(val);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MessageText"></param>
        /// <param name="LogFileName"></param>
        public static void CreateLog(string MessageText, string LogFileName)
        {
            string strPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;

            strPath = strPath.Replace(@"/newlook.p2dgroup.net", "");

            string[] arr = strPath.Split('/');

            strPath = "";
            foreach (string str in arr)
            {
                if (str.Contains("."))
                    break;

                strPath += str + "\\";
            }

            string sPathName = System.Web.HttpContext.Current.Server.MapPath("~") + strPath + "Logs";

            if (!Directory.Exists(sPathName))
                Directory.CreateDirectory(sPathName);

            sPathName += "//" + LogFileName;

            if (!File.Exists(sPathName))
            {
                var myFile = File.Create(sPathName);
                myFile.Close();
            }

            StreamWriter sw = new StreamWriter(sPathName, true);
            sw.WriteLine(DateTime.Now + ": " + MessageText);
            sw.Flush();
            sw.Close();
        }
    }
    #endregion
}