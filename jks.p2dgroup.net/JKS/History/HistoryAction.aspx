<%@ Page Language="c#" CodeBehind="HistoryAction.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.History.HistoryAction" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>HistoryAction</title>
    <meta content="False" name="vs_showGrid">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == variable) {
                    return pair[1];
                }
            }
        }
        function CaptureCloseBtn(sInvoiceID, sDocType) {
            var id = getQueryVariable("InvoiceID");
            var docType = getQueryVariable("DocType");
            var sValue = "id=" + id + "|docType=" + docType + "|IsDeleted=" + document.all.hdIsDeleted.value;
            window.opener.__doPostBack('btnCloseAction', sValue);
            window.close();
        }
        function CaptureClose(sInvoiceID, sDocType) {
            var id = getQueryVariable("InvoiceID");
            var docType = getQueryVariable("DocType");
            var sValue = "id=" + id + "|docType=" + docType + "|IsDeleted=" + document.all.hdIsDeleted.value;
            window.opener.__doPostBack('btnCloseAction', sValue);
            //window.close();
        }
		
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0" onunload="javascript:CaptureClose();">
    <form id="Form1" method="post" runat="server">
    <table>
        <tr>
            <td valign="top">
                <table width="100%">
                    <tr>
                        <td class="PageHeader" style="height: 18px" colspan="5">
                            USER ACTION HISTORY
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td style="width: 170px">
                            <font class="NormalBody" size="2">Document No</font>
                        </td>
                        <td>
                            <asp:Label ID="lbldocumentno" CssClass="NormalBody" runat="server"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px; height: 10px">
                            <font class="NormalBody" size="2">Voucher No:</font>
                        </td>
                        <td style="height: 10px">
                            <asp:Label ID="lblVoucherno" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px">
                            <font class="NormalBody" size="2">Document Date</font>
                        </td>
                        <td>
                            <asp:Label ID="lbldocumentdate" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px; height: 15px">
                            <font class="NormalBody" size="2">Supplier Name</font>
                        </td>
                        <td style="height: 15px">
                            <asp:Label ID="lblsuppliername" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px">
                            <font class="NormalBody" size="2">Document Status</font>
                        </td>
                        <td>
                            <asp:Label ID="lbldocumentstatus" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px">
                            <font class="NormalBody" size="2">Authorisation String</font>
                        </td>
                        <td>
                            <asp:Label ID="lblauthstring" CssClass="NormalBody" runat="server"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px">
                            <font class="NormalBody" size="2">Department</font>
                        </td>
                        <td>
                            <asp:Label ID="lbldepartment" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <% if (visiblelable == 1)
                       { %>
                    <tr>
                        <td style="width: 170px; height: 14px">
                            <font size="2">
                                <asp:Label ID="lblassociatedinvoiceno" CssClass="NormalBody" runat="server" Visible="False"></asp:Label></font>
                        </td>
                        <td style="height: 14px">
                            <asp:Label ID="lblassociatedno" CssClass="NormalBody" runat="server" Visible="False"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <% } %>
                    <% if (visiblelable1 == 1)
                       { %>
                    <tr>
                        <td style="width: 170px">
                            <font size="2">
                                <asp:Label ID="lblCreditNoteNo" CssClass="NormalBody" runat="server" Visible="False"></asp:Label></font>
                        </td>
                        <td>
                            <asp:Label ID="lbltextCrnNo" CssClass="NormalBody" runat="server" Visible="False"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <% } %>
                </table>
                <table>
                    <tr>
                        <td valign="top" colspan="2">
                            <asp:DataGrid ID="GDlineinfo" runat="server" Font-Size="10pt" Font-Names="verdana,Tahoma,Arial"
                                AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                BorderColor="#999999" PageSize="15" BorderStyle="None" BackColor="White" Width="100%">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="LineNo" HeaderText="Line No">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GLCompanyID" HeaderText="GL CompanyID">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AccountingUnit" HeaderText="Accounting Unit">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Account" HeaderText="Account">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SubAccount" HeaderText="Sub Account">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="NETAMT" HeaderText="Net">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="TAXAMT" HeaderText="Vat">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="GROSSAMT" HeaderText="Gross">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 158px" valign="top">
                            <font class="NormalBody" size="2">
                                <asp:Label ID="lblComment" runat="server">Comment</asp:Label></font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcomment" CssClass="NormalBody" runat="server" Width="256px" Rows="6"
                                Columns="50" Height="65px"></asp:TextBox></FONT>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center" colspan="2">
                            <table>
                                <tr>
                                    <td valign="middle">
                                        <a onclick="javascript:CaptureCloseBtn();">
                                            <img src="../../images/close.gif" style="border-top-style: none; border-right-style: none;
                                                border-left-style: none; border-bottom-style: none"></a>
                                    </td>
                                    <td valign="top">
                                        <asp:Button ID="btndelete" CssClass="ButtonCss" runat="server" BorderStyle="None"
                                            Height="23px" Width="100px" Text="Delete/Archive"></asp:Button><a onclick="javascript:window.close();"
                                                href="#"></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblmessage" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label><input
                    id="hdIsDeleted" type="hidden" value="0" runat="server">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
