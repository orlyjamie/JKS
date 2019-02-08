<%@ Page Language="c#" CodeFile="InvoiceStatusLogNL.aspx.cs" AutoEventWireup="false"
    Inherits="JKS.InvoiceStatusLogNL" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>InvoiceStatusLogNB</title>
    <meta name="vs_showGrid" content="True">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <!-- Main Content Panel Starts-->
    <form id="Form1" method="post" runat="server">
    <table class="NormalBody">
        <tr>
            <td valign="top" align="right" colspan="3">
                <a href="#" onclick="javascript:window.close();"></a>
            </td>
        </tr>
        <tr>
            <td style="padding: 1px 0;">
                Approval Path:
                <asp:Label ID="lblauthstring" CssClass="NormalBody" runat="server"></asp:Label>
            </td>
            <td style="padding: 10px 0;">
                Department:
                <asp:Label ID="lblDepartment" runat="server" CssClass="NormalBody"></asp:Label>
            </td>
            <td style="padding: 10px 0;">
                Business Unit:
                <asp:Label ID="lblBusinessUnit" runat="server" CssClass="NormalBody"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="3">
                <asp:DataGrid ID="dgSalesCallDetails" runat="server" CellPadding="0" CellSpacing="0"
                    GridLines="Vertical" AutoGenerateColumns="False" PageSize="8" AllowPaging="True"
                    CssClass="listingArea">
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                    <ItemStyle></ItemStyle>
                    <HeaderStyle CssClass="tableHd"></HeaderStyle>
                    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="RID" HeaderText="RID">
                            <HeaderStyle Width="120px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="InvoiceID" HeaderText="InvoiceID">
                            <HeaderStyle Width="300px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="InvoiceNo" HeaderText="InvoiceNo" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="UserName" HeaderText="User Name"></asp:BoundColumn>
                        <asp:BoundColumn DataField="UserCode" HeaderText="UserID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="GroupName" HeaderText="Group Name"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Status" HeaderText="Action Status"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="UserTypeID" HeaderText="UserTypeID">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ActionDate" HeaderText="Action Date"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DocStatus" HeaderText="Doc Status"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Comments" HeaderText="Comments"></asp:BoundColumn>
                        <asp:BoundColumn DataField="rejectioncode" HeaderText="Rejection Code"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
                <!-- Main Content Panel Ends-->
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="3" style="padding-top: 40px">
                <%--<a onclick="javascript:window.close();" href="#" id="aClose" class="allbtn" runat="server" style="float:none !important; display:block;">Close--%>
                <%--changed on 280215--%>
                <a onclick="javascript:window.close();" href="#" id="aClose" class="allbtninvoice"
                    runat="server" style="float: none !important; display: block;">Close
                    <%-- <img id="imgClose" alt="" src="../../images/close.gif" border="0">--%></a>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
