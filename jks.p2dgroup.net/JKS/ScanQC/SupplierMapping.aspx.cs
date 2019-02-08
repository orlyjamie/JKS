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

namespace NewLook
{
    public partial class NewLook_ScanQC_SupplierMapping : System.Web.UI.Page
    {
        SqlConnection sqlConn;

        protected void Page_Init(object sender, EventArgs e)
        {
            btnDeleteLines.Click += new EventHandler(btnDeleteLines_Click);
            btnAddLine.Click += new EventHandler(btnAddLine_Click);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            btnClose.Click += new EventHandler(btnClose_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);

            if (!IsPostBack)
            {
                if (Request.QueryString != null)
                {
                    int bcid = Convert.ToInt32(Request.QueryString[0]);
                    int scid = Convert.ToInt32(Request.QueryString[1]);

                    populateRecords(bcid, scid);
                }
            }
        }

        void populateRecords(int bcid, int scid)
        {
            SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_supplier_mapping_info", sqlConn);
            sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDA.SelectCommand.Parameters.Add("@bcid", bcid);
            sqlDA.SelectCommand.Parameters.Add("@scid", scid);//29571

            DataSet DS = new DataSet();
            DataTable DT = new DataTable();
            DataRow DR = null;
            try
            {
                sqlConn.Open();
                sqlDA.Fill(DS);

                DT = DS.Tables[0];
                DR = DT.Rows[0];
                lblBuyer.Text = DR[0].ToString();

                DT = DS.Tables[1];
                DR = DT.Rows[0];
                lblSupplier.Text = DR[0].ToString();

                DT = DS.Tables[2];
                if (DT.Rows.Count == 0)
                {
                    DT.Rows.Add(bcid, scid, "", lblSupplier.Text, 0);
                }

                gvList.DataSource = DT;
                gvList.DataBind();

                ViewState["DTSUPPLIERMAPPING"] = DT;
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                DR = null;
                DT.Dispose();
                DS.Dispose();
                sqlDA.Dispose();
                sqlConn.Close();
            }
        }

        protected void btnDeleteLines_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DT = (DataTable)ViewState["DTSUPPLIERMAPPING"];
                int c = DT.Rows.Count;

                for (int i = c - 1; i > 0; i--)
                {
                    GridViewRow GVR = gvList.Rows[i];
                    CheckBox cbSelect = (CheckBox)GVR.Cells[0].FindControl("cbSelect");
                    if (cbSelect.Checked == true)
                    {
                        int mapid = (int)gvList.DataKeys[GVR.RowIndex].Values[0];
                        bool tf = DelSupplierMapping(mapid);//delete from database
                        if (tf)
                            DT.Rows.RemoveAt(i);
                    }
                }

                gvList.DataSource = DT;
                gvList.DataBind();

                ViewState["DTSUPPLIERMAPPING"] = DT;

                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Mappings deleted successfully.');", true);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                int bcid = Convert.ToInt32(Request.QueryString[0]);
                int scid = Convert.ToInt32(Request.QueryString[1]);

                DataTable DT = (DataTable)ViewState["DTSUPPLIERMAPPING"];

                DataRow DR = null;

                foreach (GridViewRow GVR in gvList.Rows)
                {
                    DR = DT.Rows[GVR.RowIndex];
                    TextBox txtLHS = (TextBox)GVR.Cells[1].FindControl("txtLHS");
                    DR["LHS"] = txtLHS.Text;
                }

                //foreach (DataColumn dc in DT.Columns)
                //{
                //    Response.Write(dc.ColumnName + " | ");
                //}

                DR = DT.NewRow();

                DR["MAPPING ID"] = 0;
                DR["ClientID"] = bcid;
                DR["VendorID"] = scid;
                DR["LHS"] = "";
                DR["RHS"] = lblSupplier.Text;

                DT.Rows.InsertAt(DR, DT.Rows.Count);

                gvList.DataSource = DT;
                gvList.DataBind();

                ViewState["DTSUPPLIERMAPPING"] = DT;
            }
            catch (Exception ex)
            {
                Response.Write("Help Link: " + ex.HelpLink);
                Response.Write("<br /><br />");
                Response.Write("Inner Exception: " + ex.InnerException);
                Response.Write("<br /><br />");
                Response.Write("Message: " + ex.Message);
                Response.Write("<br /><br />");
                Response.Write("Source: " + ex.Source);
                Response.Write("<br /><br />");
                Response.Write("StackTrace: " + ex.StackTrace);
                Response.Write("<br /><br />");
                Response.Write("TargetSite: " + ex.TargetSite);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow GVR in gvList.Rows)
            {
                TextBox txtLHS = (TextBox)GVR.Cells[1].FindControl("txtLHS");
                //TextBox txtRHS = (TextBox)GVR.Cells[1].FindControl("txtRHS");

                if (txtLHS.Text.Length == 0)//|| txtRHS.Text.Length == 0
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('LHS cannot be blank.');", true);
                    return;
                }
            }

            int bcid = Convert.ToInt32(Request.QueryString[0]);
            int scid = Convert.ToInt32(Request.QueryString[1]);

            bool isUpdate = false;

            foreach (GridViewRow GVR in gvList.Rows)
            {
                TextBox txtLHS = (TextBox)GVR.Cells[1].FindControl("txtLHS");
                TextBox txtRHS = (TextBox)GVR.Cells[1].FindControl("txtRHS");

                int mapid = (int)gvList.DataKeys[GVR.RowIndex].Values[0];

                isUpdate = SetSupplierMapping(bcid, scid, txtLHS.Text, txtRHS.Text, mapid);
            }

            if (isUpdate)
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Mappings updated successfully.');", true);
            else
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_msg", "alert('Mappings update was not successful.');", true);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_close", "JavaScript:window.close();", true);
        }

        bool SetSupplierMapping(int bcid, int scid, string lhs, string rhs, int mapid)
        {
            bool tf = false;

            SqlCommand sqlCmd = new SqlCommand("sp_set_supplier_mapping", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@bcid", bcid);
            sqlCmd.Parameters.Add("@scid", scid);
            sqlCmd.Parameters.Add("@lhs", lhs);
            sqlCmd.Parameters.Add("@rhs", rhs);
            sqlCmd.Parameters.Add("@mapid", mapid);

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                tf = true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
            }
            finally
            {
                if (sqlCmd != null)
                    sqlCmd.Dispose();
                if (sqlConn != null)
                    sqlConn.Close();
            }

            return tf;
        }

        bool DelSupplierMapping(int mapid)
        {
            bool TF = false;

            if (mapid == 0)
                return true;

            SqlCommand sqlCmd = null;
            try
            {
                string qry = "DELETE FROM [SUPPLIER MAPPING] WHERE [MAPPING ID] = @mapid;";
                sqlCmd = new SqlCommand(qry, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.Add("@mapid", SqlDbType.Int).Value = mapid;
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();

                TF = true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                sqlConn.Dispose();
                sqlCmd.Dispose();
            }

            return TF;
        }
    }
}