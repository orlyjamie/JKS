<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>

<%@ Page Language="c#" CodeBehind="InvoiceScan.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.invoice.InvoiceScan" %>

<%@ Register TagPrefix="uc1" TagName="banner" Src="../../Utilities/bannerETC.ascx" %>
<%@ Import Namespace="System.IO" %>
<html>
<head>
    <title>P2D Network - Sales Invoice Log</title>
    <script language="javascript" src="../../WinOpener.js"></script>
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function PopupPic(DocumentID, w, h) {
            var winl = (screen.width - w) / 2;
            var wint = (screen.height - h) / 2;
            winprops = 'resizable=1,height=' + h + ',width=' + w + ',top=' + wint + ',left=' + winl
            window.open("ImgPopup_NL_INV.aspx?DocumentID=" + DocumentID, "Image", winprops)
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

            return (true);
        }
		
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form2" name="Form2" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" CssClass="Banner"
        Width="100%">
        <uc1:banner ID="Banner2" runat="server"></uc1:banner>
    </asp:Panel>
    <table width="100%">
        <tr>
            <td valign="top" width="150" bgcolor="gainsboro">
                <table>
                    <tr>
                        <td>
                            <asp:PlaceHolder ID="MenuHolder" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                </table>
                <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
            </td>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="2" cellpadding="2" width="100%">
                    <tr>
                        <td class="PageHeader" width="100%">
                            Upload and download files
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="grdFile" runat="server" Width="100%" BorderColor="#999999" BackColor="White"
                                BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False"
                                Font-Names="Verdana,Tahoma,Arial" Visible="False">
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Invoice No.">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblInvNo" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceNo") %>'>
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblInvID" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="File Name">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="70%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.DocumentID") %>'>
                                            </asp:Label>
                                            <asp:Label ID="lblPath" runat="server"></asp:Label>
                                            <asp:Label ID="lblHidePath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ImagePath") %>'>
                                            </asp:Label>
                                            <asp:Label ID="lblArchPath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.archiveImagePath") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Document">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="HpDownload" CommandArgument="DOW" BorderWidth="0" runat="server"
                                                ImageUrl="../../images/open.gif"></asp:ImageButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Remove">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="HpDel" CommandArgument="DEL" BorderWidth="0" runat="server"
                                                ImageUrl="../../images/delete.gif"></asp:ImageButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                                    Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p class="normalbody" align="center">
                                <asp:Label ID="lblMsg" runat="server" CssClass="ErrMsg">No Files Found</asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td height="1">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td class="NormalBody" nowrap width="86">
                                            <strong></strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="153">
                                        </td>
                                        <td nowrap width="70%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="86">
                                            <strong></strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="153">
                                            <p align="left">
                                                <strong>Select Batch Types :</strong></p>
                                        </td>
                                        <td nowrap width="70%">
                                            <asp:DropDownList ID="ddlBatchTypes" runat="server" Width="264px" DataValueField="BatchTypeID"
                                                DataTextField="BatchDocType">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="86" height="20">
                                            <strong><strong></strong></strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="153" height="20">
                                            <p align="left">
                                                <strong></strong>&nbsp;</p>
                                        </td>
                                        <td nowrap width="70%" height="20">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="86">
                                            <strong><strong></strong></strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="153">
                                            <p align="left">
                                                <strong></strong>&nbsp;</p>
                                        </td>
                                        <td nowrap width="70%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="86" height="8">
                                            <strong><strong></strong></strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="153" height="8">
                                            <p align="left">
                                                <strong></strong>&nbsp;</p>
                                        </td>
                                        <td nowrap width="70%" height="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="86">
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="153">
                                        </td>
                                        <td nowrap width="70%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="70%" colspan="3">
                                            <p align="center">
                                                &nbsp;</p>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td class="NormalBody" nowrap width="15%">
                                            <strong>File 1</strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="5%">
                                            <p align="left">
                                                <strong>:</strong></p>
                                        </td>
                                        <td nowrap width="70%">
                                            <input id="File1" type="file" size="50" name="File1" runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="15%">
                                            <strong>File 2</strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="5%">
                                            <p align="left">
                                                <strong>:</strong></p>
                                        </td>
                                        <td nowrap width="70%">
                                            <input id="File2" type="file" size="50" name="File2" runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="15%" height="20">
                                            <strong><strong>File 3</strong></strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="5%" height="20">
                                            <p align="left">
                                                <strong>:</strong></p>
                                        </td>
                                        <td nowrap width="70%" height="20">
                                            <input id="File3" type="file" size="50" name="File3" runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="15%">
                                            <strong><strong>File 4</strong></strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="5%">
                                            <p align="left">
                                                <strong>:</strong></p>
                                        </td>
                                        <td nowrap width="70%">
                                            <input id="File4" type="file" size="50" name="File4" runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="15%" height="8">
                                            <strong><strong>File 5</strong></strong>
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="5%" height="8">
                                            <p align="left">
                                                <strong>:</strong></p>
                                        </td>
                                        <td nowrap width="70%" height="8">
                                            <input id="File5" type="file" size="50" name="File5" runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="15%">
                                        </td>
                                        <td class="NormalBody" nowrap align="left" width="5%">
                                        </td>
                                        <td nowrap width="70%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" nowrap width="70%" colspan="3">
                                            <p align="center">
                                                <asp:Button ID="btnBack" runat="server" Width="49px" Text="Back"></asp:Button>&nbsp;<asp:Button
                                                    ID="btnUp" runat="server" Text="Upload" DESIGNTIMEDRAGDROP="80"></asp:Button>
                                                <asp:Button ID="btnUploadAll" runat="server" Text="Button"></asp:Button></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
