<%@ Import Namespace="System.IO" %>

<%@ Page Language="c#" CodeBehind="InvoiceFileManager_NL.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.invoice.InvoiceFileManager_NL" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta charset="utf-8">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;" />
    <meta name="description" content="">
    <meta name="author" content="">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <title>P2D Network - Sales Invoice Log</title>
    <!-- Bootstrap core CSS -->
    <link href="../custom_css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom Global Style -->
    <link href="../custom_css/screen.css" rel="stylesheet">
    <link href='https://fonts.googleapis.com/css?family=Lato:400,300italic,300,100italic,100,400italic,700,700italic,900,900italic'
        rel='stylesheet' type='text/css'>
    <!-- Custom Font Icon Style -->
    <link href="../custom_css/font-awesome.css" rel="stylesheet">
    <!-- Custom Responsive Style -->
    <link href="../custom_css/responsive.css" rel="stylesheet">
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="../js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript" src="../../WinOpener.js"></script>
    <script language="javascript">
        function PopupPic(DocumentID, w, h) {

            var winl = (screen.width - w) / 2;
            var wint = (screen.height - h) / 2;
            winprops = 'resizable=1,height=' + h + ',width=' + w + ',top=' + wint + ',left=' + winl
            window.open("ImgPopup_NL_INV.aspx?DocumentID=" + DocumentID, "Image", winprops);
            history.go(-1);

        }



        function ValidateFormSubmission() {

            if (document.Form2.File1.value.match(",") == ",") {
                alert("Please remove commas from the file name and re-upload");
                document.Form2.File1.focus();
                return (false);
            }
            if (document.Form2.File2.value.match(",") == ",") {
                alert("Please remove commas from the file name and re-upload");
                document.Form2.File2.focus();
                return (false);
            }
            if (document.Form2.File3.value.match(",") == ",") {
                alert("Please remove commas from the file name and re-upload");
                document.Form2.File3.focus();
                return (false);
            }
            if (document.Form2.File4.value.match(",") == ",") {
                alert("Please remove commas from the file name and re-upload");
                document.Form2.File4.focus();
                return (false);
            }
            if (document.Form2.File5.value.match(",") == ",") {
                alert("Please remove commas from the file name and re-upload");
                document.Form2.File5.focus();
                return (false);
            }


            if (document.Form2.File1.value.match("'") == "'") {
                alert("Please remove apostrophe from the file name and re-upload");
                document.Form2.File1.focus();
                return (false);
            }
            if (document.Form2.File2.value.match("'") == "'") {
                alert("Please remove apostrophe from the file name and re-upload");
                document.Form2.File2.focus();
                return (false);
            }
            if (document.Form2.File3.value.match("'") == "'") {
                alert("Please remove apostrophe from the file name and re-upload");
                document.Form2.File3.focus();
                return (false);
            }
            if (document.Form2.File4.value.match("'") == "'") {
                alert("Please remove apostrophe from the file name and re-upload");
                document.Form2.File4.focus();
                return (false);
            }
            if (document.Form2.File5.value.match("'") == "'") {
                alert("Please remove apostrophe from the file name and re-upload");
                document.Form2.File5.focus();
                return (false);
            }

            return (true);
        }
		
    </script>
</head>
<body>
    <form id="Form2" name="Form2" method="post" runat="server">
    <div class="site">
        <div class="container mainWrapper nopadding">
            <div class="white_bg mainWrapper">
                <!------------------------------ START: Header------------------------------>
                <header id="header"> <div class="container"> 
                <!--------------------   START: Top Section --------------------> 
                <div class="row h_top"> <div class="col-md-6 h_logo">
                <a href="javascript:void(0)" target="_self" title="P2D PAPER 2 DATA">
                <img src="../images/JKS_logo.png" alt="JKS" width="110px" /></a></div> </div>
                 <!--------------------  END: Top Section --------------------> 
                 </div> 
                 </header>
                <!------------------------------ END: Header ------------------------------>
                <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                <div class="login_bg">
                    <div class="current_comp fixed_height">
                        <div class="form-horizontal form_section">
                            <div class="row">
                                <%-- <div class="PageHeader">
                                    <label>
                                        View and Upload Attachments</label>
                                </div>--%>
                                <div class="col-md-12">
                                    <asp:DataGrid ID="grdFile" runat="server" CellPadding="0" Width="100%" CellSpacing="0"
                                        GridLines="none" AutoGenerateColumns="False" CssClass="listingArea">
                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                        <ItemStyle BackColor="White"></ItemStyle>
                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Invoice No.">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblInvNo" Text='<%# DataBinder.Eval(Container,
    "DataItem.InvoiceNo") %>'> </asp:Label>
                                                    <asp:Label runat="server" ID="lblInvID" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceID") %>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="File Name">
                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="70%"></HeaderStyle>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocID" runat="server" Visible="False" Text='<%#
    DataBinder.Eval(Container, "DataItem.DocumentID") %>'> </asp:Label>
                                                    <asp:Label ID="lblPath" runat="server"></asp:Label>
                                                    <asp:Label ID="lblHidePath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ImagePath") %>'> </asp:Label>
                                                    <asp:Label ID="lblArchPath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,
    "DataItem.archiveImagePath") %>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Open">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Button ID="HpDownload" CommandArgument="DOW" BorderWidth="0" runat="server"
                                                        CssClass="allbtn_ActionWindow" Text="Open"></asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Delete">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Button ID="HpDel" CommandArgument="DEL" BorderWidth="0" runat="server" CssClass="btnDelete_ActionWindow"
                                                        Text="Delete"></asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                                            Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                    <table width="100%">
                                        <tr>
                                            <td valign="top">
                                                <!-- Main Content Panel Starts-->
                                                <table id="Table1" cellspacing="2" cellpadding="2" width="100%">
                                                    <tr>
                                                        <td>
                                                            <p class="normalbody" align="center">
                                                                <asp:Label ID="lblMsg" runat="server" CssClass="ErrMsg">No Files Found</asp:Label></p>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                                <tr>
                                                                    <td colspan="5">
                                                                        <div class="file_medi" style="width: 60%;">
                                                                            <div class="col-lg-12">
                                                                                <div class="form-group form-group2">
                                                                                    <label for="inputEmail" class="col-lg-3 control-label label_text">
                                                                                        <strong>File 1 :</strong></label>
                                                                                    <div class="col-lg-9">
                                                                                        <div class="row">
                                                                                            <input id="File1" type="file" size="50" name="File1" runat="server" style="width: 90%;
                                                                                                height: 30px; margin-bottom: 5px" class="file_border" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-12">
                                                                                <div class="form-group form-group2">
                                                                                    <label for="inputEmail" class="col-lg-3 control-label label_text">
                                                                                        <strong>File 2 :</strong></label>
                                                                                    <div class="col-lg-9">
                                                                                        <div class="row">
                                                                                            <input id="File2" type="file" size="50" name="File2" runat="server" style="width: 90%;
                                                                                                height: 30px; margin-bottom: 5px" class="file_border" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-12">
                                                                                <div class="form-group form-group2">
                                                                                    <label for="inputEmail" class="col-lg-3 control-label label_text">
                                                                                        <strong>File 3 :</strong></label>
                                                                                    <div class="col-lg-9">
                                                                                        <div class="row">
                                                                                            <input id="File3" type="file" size="50" name="File3" runat="server" style="width: 90%;
                                                                                                height: 30px; margin-bottom: 5px" class="file_border" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-12">
                                                                                <div class="form-group form-group2">
                                                                                    <label for="inputEmail" class="col-lg-3 control-label
    label_text">
                                                                                        <strong>File 4 :</strong></label>
                                                                                    <div class="col-lg-9">
                                                                                        <div class="row">
                                                                                            <input id="File4" type="file" size="50" name="File4" runat="server" style="width: 90%;
                                                                                                height: 30px; margin-bottom: 5px" class="file_border" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-12">
                                                                                <div class="form-group form-group2">
                                                                                    <label for="inputEmail" class="col-lg-3 control-label label_text">
                                                                                        <strong>File 5 :</strong></label>
                                                                                    <div class="col-lg-9">
                                                                                        <div class="row">
                                                                                            <input id="File5" type="file" size="50" name="File5" runat="server" style="width: 90%;
                                                                                                height: 30px; margin-bottom: 5px" class="file_border" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="NormalBody" nowrap width="15%">
                                                                    </td>
                                                                    <td class="NormalBody" nowrap align="left" width="5%">
                                                                    </td>
                                                                    <td nowrap width="5%">
                                                                    </td>
                                                                    <td nowrap width="50%" align="center">
                                                                        <asp:Button ID="btnBack" runat="server" Width="49px" Text="Back" Visible="False"
                                                                            Style="float: none"></asp:Button>
                                                                        <div class="col-lg-2">
                                                                        </div>
                                                                        <div class="col-lg-8">
                                                                            <div class="row">
                                                                                <asp:Button ID="btnUp" runat="server" Text="Upload" CssClass="sub_down btn-primary btn-group-justified"
                                                                                    Style="float: left !important; margin-bottom: 10px; margin-right: 15px; width: 150px;
                                                                                    padding-left: 20px; padding-right: 20px;"></asp:Button>
                                                                                <input id="btnb" onclick="history.back();" type="button" value="Back" name="btnb"
                                                                                    class="sub_down btn-primary btn-group-justified" style="float: left !important;
                                                                                    padding-left: 20px !important; padding-right: 20px !important; margin-bottom: 10px;
                                                                                    width: 150px; margin-right: 13px;" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td nowrap width="15%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="NormalBody" nowrap width="5%" colspan="3">
                                                                        <p align="center">
                                                                            &nbsp;</p>
                                                                    </td>
                                                                    <td class="NormalBody" nowrap width="60%" align="center">
                                                                    </td>
                                                                    <td class="NormalBody" nowrap width="15%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="NormalBody" nowrap width="5%" colspan="3">
                                                                    </td>
                                                                    <td class="NormalBody" nowrap width="60%" align="center">
                                                                    </td>
                                                                    <td class="NormalBody" nowrap width="15%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="NormalBody" nowrap width="5%" colspan="3">
                                                                        LINKED DOCUMENTS :
                                                                    </td>
                                                                    <td class="NormalBody" nowrap width="60%" align="center">
                                                                    </td>
                                                                    <td class="NormalBody" nowrap width="15%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%" colspan="3" height="50" valign="middle" class="NormalBody">
                                                                        <asp:Label ID="lblcreditnoteno" runat="server" Visible="False" CssClass="NormalBody"
                                                                            ForeColor="Red"></asp:Label>
                                                                        <font style="color: red">
                                                                            <%=GetCreditLinks()%>
                                                                        </font>
                                                                    </td>
                                                                    <td class="NormalBody" valign="middle" width="60%" height="50" align="center">
                                                                    </td>
                                                                    <td class="NormalBody" valign="middle" width="15%" height="50">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
