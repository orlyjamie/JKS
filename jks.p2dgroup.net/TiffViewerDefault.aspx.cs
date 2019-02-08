using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Drawing;
using System.Xml;
// Added By Mrinal Chakravorty 11th August 2014
using System.Data;
using System.Data.SqlClient;
using CBSolutions.Architecture.Data;
using CBSolutions.Architecture.Core;
using CBSolutions.ETH.Web;

namespace TestTiff
{
    public partial class TiffViewerDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("close_win.aspx");
            // This code is for IE 8 annotations, you can safely delete below code
            if ((Request.Browser.IsBrowser("IE") && Request.Browser.MajorVersion == 8))
            {
                this.dt.Text = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=EmulateIE7\" />";
            }
            if (!Page.IsPostBack)
            {
                FetchTiffFile();
                // Load an initial file, you can have your own!
                //    string token = ctlTiff.LoadTiff(Server.MapPath("~/Files/sample.tif"));

                // You can also open tiff file from file stream, use overload of above method
                // byte[] buff = ForceDownload(dtAttachment.Rows[i]["ImagePath"].ToString(), dtAttachment.Rows[i]["ArchiveImagePath"].ToString(), 0);
                //  Stream stream = new MemoryStream(byteArray);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /*  protected void btnSave_Click(object sender, EventArgs e)
          {
              string saveFile = Server.MapPath("~/files/saved/" + DateTime.Now.Ticks.ToString() + ".tif");
              MemoryStream msReturn = ctlTiff.SaveTiff(0, 0, saveFile, chkAnn.Checked, chkCompress.Checked);

              // You can pass "" to SaveTiff method if you do not want to physically create a file

              if (null != msReturn)
              {
                  // Do further processing
                  ctlTiff.LoadTiff(msReturn);
              }
          }


          protected void btnCloseTif_Click(object sender, EventArgs e)
          {
              ctlTiff.CloseTiff();
              ctlTiff.LoadTiff(Server.MapPath("~/files/sample.tif")); // following code opens same file, you may choose another!
          }

          protected void btnFastMode_Click(object sender, EventArgs e)
          {
              ctlTiff.CloseTiff();
              ctlTiff.FastMode = false;
              ctlTiff.LoadTiff(Server.MapPath("~/files/sample.tif"));
          }*/

        protected void btnLoadAnnotations_Click(object sender, EventArgs e)
        {
            // Load a previously saved annotation data
            // ctlTiff.LoadAnnotationData("1~YW5uSWQ6cmVjdGFuZ2xlXzAsYW5uVHlwZTpyZWN0YW5nbGUsaW5kZXg6MSxsZWZ0OjE3MSx0b3A6NTcsd2lkdGg6NDQ2LGhlaWdodDoxMDkscm90YXRlOjAsY2FuUm90YXRlOnRydWUsYmFja0NvbG9yOnRyYW5zcGFyZW50LGJvcmRlckNvbG9yOiMwMDAwMDAsYm9yZGVyV2lkdGg6MixzaG93Qm9yZGVyOnRydWUsb3BhY2l0eToxMDAsdGl0bGU6LHNob3dUaXRsZTpmYWxzZSx0aXRsZUNvbG9yOmJsdWUsdGl0bGVGb250U2l6ZToxMixub3RlOixzaG93Tm90ZTpmYWxzZSxidXJuOnRydWUsbG9ja2VkOmZhbHNlLHJlc2l6ZVJhdGlvOjAsUHdpZHRoOjc4MCxQaGVpZ2h0OjEwNzIsUGxlZnQ6MjExLFB0b3A6NzcsU2xlZnQ6MCxTdG9wOjA=^2~YW5uSWQ6c3F1YXJlXzAsYW5uVHlwZTpzcXVhcmUsaW5kZXg6MSxsZWZ0OjMzOSx0b3A6Mjcsd2lkdGg6NjcsaGVpZ2h0OjY3LHJvdGF0ZTowLGNhblJvdGF0ZTp0cnVlLGJhY2tDb2xvcjojMDAwMGZmLGJvcmRlckNvbG9yOiMwMDAwMDAsYm9yZGVyV2lkdGg6MixzaG93Qm9yZGVyOnRydWUsb3BhY2l0eTo1MCx0aXRsZTpzcXVhcmVfMCxzaG93VGl0bGU6ZmFsc2UsdGl0bGVDb2xvcjpibHVlLHRpdGxlRm9udFNpemU6MTIsbm90ZTosc2hvd05vdGU6ZmFsc2UsYnVybjp0cnVlLGxvY2tlZDpmYWxzZSxyZXNpemVSYXRpbzowLFB3aWR0aDo3ODAsUGhlaWdodDoxMDcyLFBsZWZ0OjIxMSxQdG9wOjc3LFNsZWZ0OjAsU3RvcDow^3~YW5uSWQ6cmVjdGFuZ2xlXzAsYW5uVHlwZTpyZWN0YW5nbGUsaW5kZXg6MSxsZWZ0OjcyNix0b3A6MjI0LHdpZHRoOjUwLGhlaWdodDoxNDkscm90YXRlOjAsY2FuUm90YXRlOnRydWUsYmFja0NvbG9yOiMwMGZmMDAsYm9yZGVyQ29sb3I6IzAwMDAwMCxib3JkZXJXaWR0aDoyLHNob3dCb3JkZXI6dHJ1ZSxvcGFjaXR5OjYwLHRpdGxlOnJlY3RhbmdsZV8wLHNob3dUaXRsZTpmYWxzZSx0aXRsZUNvbG9yOmJsdWUsdGl0bGVGb250U2l6ZToxMixub3RlOixzaG93Tm90ZTpmYWxzZSxidXJuOnRydWUsbG9ja2VkOmZhbHNlLHJlc2l6ZVJhdGlvOjAsUHdpZHRoOjc4MCxQaGVpZ2h0OjYwMSxQbGVmdDoyMTEsUHRvcDozNSxTbGVmdDowLFN0b3A6NDI=");

            // OR, comment above two lines and uncomment below three lines

            // Load from XML string or document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\"?><Pages><Page Number=\"1\"><Annotations><Annotation><ID>rectangle_0_57</ID><Type>rectangle</Type><ParentWidth>980</ParentWidth><ParentHeight>1348</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>211</Left><Top>70</Top><Width>548</Width><Height>138</Height><BackColor>#993300</BackColor><BorderColor>#000000</BorderColor><BorderWidth>2</BorderWidth><Title>rectangle_0_57</Title><Note></Note><Opacity>50</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>true</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>true</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize></Annotation><Annotation><ID>circle_1_31</ID><Type>circle</Type><ParentWidth>980</ParentWidth><ParentHeight>1348</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>602</Left><Top>266</Top><Width>173</Width><Height>173</Height><BackColor>#000000FF</BackColor><BorderColor>#000000</BorderColor><BorderWidth>2</BorderWidth><Title></Title><Note></Note><Opacity>100</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>false</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>true</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize></Annotation><Annotation><ID>triangle_2_45</ID><Type>triangle</Type><ParentWidth>980</ParentWidth><ParentHeight>1348</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>293</Left><Top>536</Top><Width>151</Width><Height>150</Height><BackColor>#000000FF</BackColor><BorderColor>#000000</BorderColor><BorderWidth>2</BorderWidth><Title></Title><Note></Note><Opacity>100</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>false</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>true</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize></Annotation><Annotation><ID>ellipse_3_29</ID><Type>ellipse</Type><ParentWidth>980</ParentWidth><ParentHeight>1348</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>802</Left><Top>105</Top><Width>144</Width><Height>55</Height><BackColor>#000000FF</BackColor><BorderColor>#000000</BorderColor><BorderWidth>2</BorderWidth><Title></Title><Note></Note><Opacity>100</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>false</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>true</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize></Annotation><Annotation><ID>stamp_4_91</ID><Type>stamp</Type><ParentWidth>980</ParentWidth><ParentHeight>1348</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>322</Left><Top>339</Top><Width>253</Width><Height>82</Height><BackColor>#000000FF</BackColor><BorderColor>#800000</BorderColor><BorderWidth>3</BorderWidth><Title>STAMP</Title><Note></Note><Opacity>100</Opacity><Rotate>-15</Rotate><ShowTitle>false</ShowTitle><ShowBorder>true</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>true</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>49.199999999999996</TitleFontSize></Annotation></Annotations></Page><Page Number=\"2\"><Annotations><Annotation><ID>rectangle_0_55</ID><Type>rectangle</Type><ParentWidth>980</ParentWidth><ParentHeight>1348</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>263</Left><Top>35</Top><Width>320</Width><Height>142</Height><BackColor>#000000FF</BackColor><BorderColor>#000000</BorderColor><BorderWidth>2</BorderWidth><Title></Title><Note></Note><Opacity>100</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>true</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>true</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize></Annotation><Annotation><ID>freehand_1_70</ID><Type>freehand</Type><ParentWidth>980</ParentWidth><ParentHeight>1348</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>693</Left><Top>27</Top><Width>186</Width><Height>102</Height><BackColor>#000000FF</BackColor><BorderColor>#000000</BorderColor><BorderWidth>2</BorderWidth><Title></Title><Note></Note><Opacity>100</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>true</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>true</CanRotate><Burn>true</Burn><Locked>true</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize><FreeHandData>MzYhMzYhMzUhMzUhMzUhMzUhMzchMzghNDAhNDIhNDYhNTAhNTchNjMhNjghNzEhNzMhNzQhNzYhNzchNzchNzchNzUhNzMhNzAhNjYhNjEhNTchNTQhNTIhNTEhNTAhNDkhNDkhNDkhNDkhNTEhNTQhNTYhNjEhNjQhNjkhNzIhNzUhNzghNzkhODEhODIhODMhODMhODMhODMhODMhODMhODMhODQhODUhODYhODchODghOTAhOTIhOTQhOTUhOTchOTchOTchOTchOTghOTghOTkhMTAxITEwMiExMDQhMTA1ITEwNiExMDchMTA3ITEwNyExMDghMTA4ITEwOSExMTAhMTEwITExMSExMTIhMTEyITExNCExMTUhMTE3ITExOSExMTkhMTIzITEyNSExMjghMTI5ITEzMSExMzEhMTMxITEzMiExMzUhMTM3ITEzOCExNDAhMTQzITE0NSExNDchMTQ3ITE0OCExNDghMTQ4ITE0NyExNDYhMTQ1ITE0NCExNDMhMTQxITEzOSExMzghMTM2ITEzNiExMzUhMTM1ITEzNSExMzUhMTM1ITEzNiExMzghMTQxITE0MiExNDQhMTQ3ITE0OCExNDkhMTUxITE1MiExNTMhMTU0ITE1NCFAfjQzITQyITQwITM3ITM1ITMyITMyITMwITI5ITI4ITI3ITI3ITI4ITMyITM2ITQxITQ1ITQ5ITUzITU3ITYwITY0ITY2ITY5ITczITc2ITc4ITgwITgxITgyITgyITgyITgyITc5ITc1ITcxITY3ITU4ITUyITQ4ITQzITM5ITM3ITM1ITMzITMyITMxITMwITMwITMxITMzITM1ITM5ITQ0ITQ4ITUwITUzITUzITUzITUxITQ5ITQ2ITQ0ITQxITQwITM4ITM3ITM2ITM4ITQwITQyITQ1ITQ2ITQ3ITQ4ITQ4ITQ3ITQ1ITQzITQxITM5ITM3ITM3ITM2ITM2ITM3ITM4ITM5ITM5ITM5ITM5ITM4ITM3ITM1ITMzITMyITMwITI4ITI3ITMwITM1ITQxITQ3ITUxITYwITY0ITcwITc1ITc4ITgwITgxITgyITgyITgyITgyITgyITgwITc3ITc1ITcyITcwITY2ITY0ITYxITU5ITU2ITUzITUwITQ4ITQ3ITQ2ITQ2ITQ2ITQ2ITQ2ITQ2ITQ2ITQ2ITQ3IUA=</FreeHandData></Annotation></Annotations></Page><Page Number=\"3\"><Annotations><Annotation><ID>circle_0_97</ID><Type>circle</Type><ParentWidth>980</ParentWidth><ParentHeight>755</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>352</Left><Top>578</Top><Width>116</Width><Height>116</Height><BackColor>#000000FF</BackColor><BorderColor>#000000</BorderColor><BorderWidth>2</BorderWidth><Title></Title><Note></Note><Opacity>100</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>false</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>true</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize></Annotation><Annotation><ID>arrow_1_56</ID><Type>arrow</Type><ParentWidth>980</ParentWidth><ParentHeight>755</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>653</Left><Top>32</Top><Width>254</Width><Height>74</Height><BackColor>#000000FF</BackColor><BorderColor>#000000</BorderColor><BorderWidth>4</BorderWidth><Title></Title><Note></Note><Opacity>100</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>false</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>false</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize><ArrowDirection>NE</ArrowDirection></Annotation><Annotation><ID>line_2_9</ID><Type>line</Type><ParentWidth>980</ParentWidth><ParentHeight>755</ParentHeight><ParentLeft>211</ParentLeft><ParentTop>83</ParentTop><ScrollTop>0</ScrollTop><ScrollLeft>0</ScrollLeft><Left>664</Left><Top>539</Top><Width>143</Width><Height>8</Height><BackColor>#000000FF</BackColor><BorderColor>#000000</BorderColor><BorderWidth>4</BorderWidth><Title></Title><Note></Note><Opacity>100</Opacity><Rotate>0</Rotate><ShowTitle>false</ShowTitle><ShowBorder>false</ShowBorder><ShowNote>false</ShowNote><TextAlign>left</TextAlign><CanRotate>false</CanRotate><Burn>true</Burn><Locked>false</Locked><ResizeRatio>0</ResizeRatio><TitleColor>blue</TitleColor><TitleFontSize>12</TitleFontSize><LineVertical>false</LineVertical></Annotation></Annotations></Page></Pages>");
            ctlTiff.LoadAnnotationXML(xmlDoc);
        }

        protected void btnAnnotationData_Click(object sender, EventArgs e)
        {
            // Get data as string
            string annData = ctlTiff.GetAnnotationData();

            // Get data as XML
            XmlDocument annXml = ctlTiff.GetAnnotationXML();

            // Get XML as string
            string xmlAsString = annXml.OuterXml;
        }


        // Following is the server side method to insert a tiff file
        /*
        protected void Upload_Insert_Click(object sender, EventArgs e)
        {
            if (txtUpload.HasFile)
            {
                string fileName = txtUpload.FileName;

                string uploadedFile = Server.MapPath("~/files/saved/" + DateTime.Now.Ticks.ToString() + "-" + fileName);
                txtUpload.SaveAs(uploadedFile);

                using (FileStream newFile = new FileStream(uploadedFile, FileMode.Open))
                {
                    using (MemoryStream newStream = new MemoryStream())
                    {
                        byte[] bytes = new byte[newFile.Length];
                        newFile.Read(bytes, 0, (int)newFile.Length);
                        newStream.Write(bytes, 0, (int)newFile.Length);

                        ctlTiff.InsertTiff(newStream, Convert.ToInt32(txtIndex.Text), true, chkAnn.Checked, chkCompress.Checked, true);
                    }
                }
            }
        }
       */
        #region: New Implementation

        private string GetURL()
        {
            return ConfigurationManager.AppSettings["ServiceURL"];
        }
        private string GetURL2()
        {
            return ConfigurationManager.AppSettings["ServiceURLNew"];
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
        private byte[] ForceDownload(string ImagePath, string ArchPath, int iStep)
        {
            bool bRetVal = false;
            byte[] bytBytesFinal = null;
            string sFileName = string.Empty;
            if (iStep == 0)
            {
                sFileName = ImagePath;
                sFileName = sFileName.Replace("I:", "C:\\P2D");
                sFileName = sFileName.Replace("\\90104-server2", "C:\\P2D");
                sFileName = GetTrimFirstSlash(sFileName);
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef.FileDownload objService = new CBSolutions.ETH.Web.WEBRef.FileDownload();
                    objService.Url = GetURL();
                    byte[] bytBytes = objService.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        bytBytesFinal = bytBytes;

                    }
                    else
                    {
                        bytBytesFinal = ForceDownload(ImagePath, ArchPath, 1);
                    }
                }
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            else if (iStep == 1)
            {
                sFileName = ArchPath;
                sFileName = sFileName.Replace("\\90107-server3", @"C:\File Repository");
                sFileName = GetTrimFirstSlash(sFileName);
                System.IO.FileStream fs1 = null;
                try
                {
                    CBSolutions.ETH.Web.WEBRef2.FileDownload objService2 = new CBSolutions.ETH.Web.WEBRef2.FileDownload();
                    objService2.Url = GetURL2();
                    byte[] bytBytes = objService2.DownloadFile(sFileName);
                    if (bytBytes != null)
                    {
                        bytBytesFinal = bytBytes;
                    }
                }
                catch (Exception Ex)
                {
                    string Error = Ex.ToString();
                }
            }
            return bytBytesFinal;
        }

        private void FetchTiffFile()
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["Type"] != null)
            {
                int ID = 0;
                string Type = string.Empty;
                if (Request.QueryString["ID"] != null)
                {
                    ID = Convert.ToInt32(Request.QueryString["ID"]);
                }
                if (Request.QueryString["Type"] != null)
                {
                    Type = Convert.ToString(Request.QueryString["Type"]);
                }
                if (ID > 0)
                {
                    DataSet ds = new DataSet();
                    SqlConnection sqlConn = new SqlConnection(CBSAppUtils.PrimaryConnectionString);
                    SqlDataAdapter sqlDA = new SqlDataAdapter("TiffViewer_GetUploadFileDetails", sqlConn);
                    sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand.Parameters.Add("@InvoiceID", Convert.ToInt32(ID));
                    sqlDA.SelectCommand.Parameters.Add("@Type", Convert.ToString(Type));

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
                        if (sqlDA != null)
                            sqlDA.Dispose();
                        if (sqlConn != null)
                            sqlConn.Close();
                    }
                    int CloseFlag = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            byte[] byteArray = ForceDownload(ds.Tables[0].Rows[0]["ImagePath"].ToString(), ds.Tables[0].Rows[0]["ArchiveImagePath"].ToString(), 0);
                            if (byteArray != null)
                            {
                                Stream stream = new MemoryStream(byteArray);
                                string token = ctlTiff.LoadTiff(stream);
                            }
                            else
                            {

                                CloseFlag = 1;
                            }
                        }
                        else
                        {
                            CloseFlag = 1;
                        }

                    }
                    else
                    {
                        CloseFlag = 1;
                    }
                    if (CloseFlag == 1)
                    {
                        if (!Page.IsPostBack)
                        {
                            // this.RegisterClientScriptBlock("clientScript", "<script language=javascript> window.close(); </script>");
                            Response.Write("<script>self.close();</script>");
                        }
                    }
                }
                else
                {
                    if (!Page.IsPostBack)
                    {
                        // this.RegisterClientScriptBlock("clientScript", "<script language=javascript> window.close(); </script>");
                        Response.Write("<script>self.close();</script>");
                    }
                }

            }

        }
        #endregion
    }
}