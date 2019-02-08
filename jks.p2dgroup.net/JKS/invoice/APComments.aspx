<%@ Page Language="c#" CodeBehind="APComments.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.invoice.APComments" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>APComments</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">

        function CheckComment() {
            if (document.getElementById('txtComments').value == '') {
                alert('You must add comments');
                return false;
            }
            else {
                return true;
            }
        }
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
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
        function CaptureClose(sInvoiceID, sDocType) {
            document.body.style.cursor = 'wait';
            var id = getQueryVariable("InvoiceID");
            var docType = getQueryVariable("DocType");
            var sValue = "id=" + id + "|docType=" + docType + "|IsDeleted=" + document.all.hdIsDeleted.value;
            /* window.opener.__doPostBack('btnCloseAction',sValue);					*/
            window.opener.__doPostBack('btnProcess', sValue);
        }
    </script>
</head>
<body onunload="javascript:CaptureClose();">
    <form id="Form1" method="post" runat="server">
    <table style="width: 520px; height: 169px" width="520">
        <tr>
            <td valign="top" align="left" class="PageHeader">
                AP Comments
            </td>
            <tr>
                <td valign="top" align="left">
                    <font class="NormalBody" size="2">Document No:</font>
                    <asp:Label ID="lblDocNo" runat="server" CssClass="NormalBody"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <font class="NormalBody" size="2">Doc Status:</font>
                    <asp:Label ID="lblDocStatus" runat="server" CssClass="NormalBody"></asp:Label><asp:DataGrid
                        ID="dgSalesCallDetails" runat="server" AllowPaging="True" PageSize="8" Font-Size="10pt"
                        Font-Names="verdana" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3"
                        BorderWidth="1px" BorderStyle="None" BackColor="White" Height="88px" Width="680px"
                        BorderColor="#999999">
                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                        <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                        </ItemStyle>
                        <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                            BackColor="#3399CC"></HeaderStyle>
                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="APCommentsID" HeaderText="RID">
                                <HeaderStyle Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="DocumentID" HeaderText="InvoiceID">
                                <HeaderStyle Width="300px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="UserName" HeaderText="UserName"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ActionDate" HeaderText="Action Date"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Comments" HeaderText="Comments"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                        </PagerStyle>
                    </asp:DataGrid>
                    <!-- Main Content Panel Ends-->
                </td>
            </tr>
            <tr>
                <td valign="top" align="center">
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <%--Modified by kuntalKarar on 30thNovember2016--%>
                        <%if (UserTypeID > 2)
                          { %>
                        <tr>
                            <td class="NormalBody" width="25%">
                                Add Comments :
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="550px" Height="80px"
                                    Style="border: 1px solid #6b6969"></asp:TextBox>
                            </td>
                        </tr>
                        <tr height="10">
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                        </tr>
                        <tr>
                            <td class="NormalBody" width="25%">
                                <asp:CheckBox ID="chkHold" runat="server" Text="Hold?"></asp:CheckBox>
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                        </tr>
                        <% } %>
                        <tr>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                        </tr>
                        <tr>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                        </tr>
                        <tr>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="25%">
                                <asp:Button ID="btnCancel" runat="server" CssClass="allbtn" BorderWidth="0px" BorderStyle="None"
                                    Text="Cancel"></asp:Button>
                            </td>
                            <td width="25%">
                            </td>
                            <td width="25%">
                                <input id="hdIsDeleted" type="hidden" value="0" runat="server" name="hdIsDeleted">
                            </td>
                            <td width="25%">
                                <%--Modified by kuntalKarar on 30thNovember2016--%>
                                <%if (UserTypeID > 2)
                                  { %>
                                <asp:Button ID="btnSubmit" runat="server" CssClass="allbtn" BorderWidth="0px" BorderStyle="None"
                                    Text="Submit"></asp:Button>
                                <% } %>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center">
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center">
                    <a onclick="javascript:window.close();" href="#"></a>
                </td>
            </tr>
    </table>
    </form>
</body>
</html>
