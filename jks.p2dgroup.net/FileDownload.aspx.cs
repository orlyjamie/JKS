using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using CBSolutions.Architecture.Core;
using System.Configuration;

public partial class FileDownload : System.Web.UI.Page
{

    private void InitializeComponent()
    {

        this.grdFile.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdFile_ItemCommand);
        this.grdFile.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdFile_ItemDataBound);
        this.Load += new System.EventHandler(this.Page_Load);


    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void ShowFiles(int InvoiceID)
    {
        SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
        SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetUploadFileDetails_NL", sqlConn);
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        sqlDA.SelectCommand.Parameters.Add("@InvoiceID", InvoiceID);
        DataSet ds = new DataSet();
        try
        {
            sqlConn.Open();
            sqlDA.Fill(ds);
        }
        catch (Exception ex) { string ss = ex.Message.ToString(); }
        finally
        {
            sqlDA.Dispose();
            sqlConn.Close();
        }
        grdFile.DataSource = ds.Tables[0];
        grdFile.DataBind();
        if (grdFile.Items.Count > 0)
        {
            lblMsg.Visible = false;
        }
        else
        {
            lblMsg.Visible = true;
        }

    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        ShowFiles(Convert.ToUInt16(txtinvoiceid.Text));
    }
    protected void grdFile_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        bool bFound = false;
        int DocumentID = 0;
        lblMsg.Text = "";

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DocumentID = Convert.ToInt32(((Label)e.Item.FindControl("lblDocID")).Text);
            if (Convert.ToString(e.CommandArgument) == "DOW")
            {
                string sDownLoadPath = ((Label)e.Item.FindControl("lblHidePath")).Text;
                sDownLoadPath = sDownLoadPath.Replace("I:", "C:\\P2D");
                sDownLoadPath = sDownLoadPath.Replace("\\90104-server2", "C:\\P2D");
                sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);
                if (sDownLoadPath.Trim() != "")
                {
                    if (System.IO.Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                    {
                        try
                        {
                            bFound = ForceDownload(sDownLoadPath, 0);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        bFound = ForceDownload(sDownLoadPath, 0);
                    }
                }
                if (bFound == false)
                {
                    sDownLoadPath = ((Label)e.Item.FindControl("lblArchPath")).Text;
                    sDownLoadPath = sDownLoadPath.Replace("\\90107-server3", @"C:\File Repository");
                    sDownLoadPath = GetTrimFirstSlash(sDownLoadPath);

                    if (sDownLoadPath.Trim() != "")
                    {
                        if (System.IO.Path.GetExtension(sDownLoadPath).ToUpper() != ".TIF")
                        {
                            try
                            {

                                bFound = ForceDownload(sDownLoadPath, 1);
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            bFound = ForceDownload(sDownLoadPath, 1);
                        }
                    }
                }
            }
        }
    }
    protected void grdFile_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (DataBinder.Eval(e.Item.DataItem, "archiveImagePath") != DBNull.Value)
            {
                if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")) != "")
                {
                    ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "archiveImagePath")).Trim());
                }
            }
            else
            {
                if (DataBinder.Eval(e.Item.DataItem, "ImagePath") != DBNull.Value)
                {
                    if (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")) != "")
                    {
                        ((Label)e.Item.FindControl("lblPath")).Text = System.IO.Path.GetFileName(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImagePath")).Trim());
                    }
                    else
                    {
                        ((Label)e.Item.FindControl("lblPath")).Text = "N/A";
                    }
                }
            }
        }
    }

    private string GetTrimFirstSlash(string sVal)
    {
        string sFName = sVal;
        if (sVal != "" & sVal != null)
        {

            string sInfo = sVal;
            sInfo.Replace(@"\", @"\\");
            string[] delValue = sInfo.Split(new char[] { '\\' });
            sFName = "";
            for (int x = 0; x < delValue.Length; x++)
            {
                if (delValue[x] != "")
                {
                    sFName = sFName + delValue[x];
                    if (x != delValue.Length - 1)
                    {
                        sFName = sFName + @"\";
                    }
                }
            }
        }
        return sFName;
    }
    private bool ForceDownload(string sPath, int iStep)
    {
        bool bRetVal = false;
        string sFileName = sPath;
        if (iStep == 0)
        {
            System.IO.FileStream fs1 = null;
            try
            {
                CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                objService.Url = GetURL();
                byte[] bytBytes = objService.DownloadFile(sFileName);
                if (bytBytes != null)
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=" + System.IO.Path.GetFileName(sFileName));
                    Response.ContentType = "application//octet-stream";
                    Response.BinaryWrite(bytBytes);
                    Response.Flush();
                    Response.End();
                    fs1.Close();
                    fs1 = null;
                    bRetVal = true;
                }
            }
            catch
            {
            }
        }
        else if (iStep == 1)
        {
            System.IO.FileStream fs1 = null;
            try
            {
                CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
                objService2.Url = GetURL2();
                byte[] bytBytes = objService2.DownloadFile(sFileName);
                if (bytBytes != null)
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=" + System.IO.Path.GetFileName(sFileName));
                    Response.ContentType = "application//octet-stream";
                    Response.BinaryWrite(bytBytes);
                    Response.Flush();
                    Response.End();
                    fs1.Close();
                    fs1 = null;
                    bRetVal = true;
                }
            }
            catch
            {

            }
        }
        return bRetVal;
    }

    private string GetURL()
    {
        return ConfigurationManager.AppSettings["ServiceURL"];
    }
    private string GetURL2()
    {

        return ConfigurationManager.AppSettings["ServiceURLNew"];
    }

}