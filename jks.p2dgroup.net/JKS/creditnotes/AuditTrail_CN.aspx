<%@ Page Language="c#" CodeBehind="AuditTrail_CN.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.creditnotes.AuditTrail_CN" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Audit Trail</title>
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
    <table width="520" style="width: 520px; height: 169px">
        <tr>
            <td valign="top" align="right">
                <a href="#" onclick="javascript:window.close();"></a>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left">
                <font class="NormalBody" size="2">Authorisation String:</font>
                <asp:Label ID="lblauthstring" CssClass="NormalBody" runat="server"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <font class="NormalBody" size="2">Department:
                    <asp:Label ID="lblDepartment" runat="server"></asp:Label></font>
                <asp:DataGrid ID="dgSalesCallDetails" runat="server" BorderColor="#999999" Width="680px"
                    Height="88px" BackColor="White" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                    GridLines="Vertical" AutoGenerateColumns="False" Font-Names="verdana" Font-Size="10pt"
                    PageSize="8" AllowPaging="True">
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                    <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                    </ItemStyle>
                    <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                        BackColor="#3399CC"></HeaderStyle>
                    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="" HeaderText="RID">
                            <HeaderStyle Width="120px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="InvoiceID" HeaderText="InvoiceID">
                            <HeaderStyle Width="300px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="InvoiceNo" HeaderText="Credit Note No"></asp:BoundColumn>
                        <asp:BoundColumn DataField="UserName" HeaderText="UserName"></asp:BoundColumn>
                        <asp:BoundColumn DataField="UserID" HeaderText="UserID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="GroupName" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Action Status" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="" HeaderText="UserTypeID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="EditDate" HeaderText="Edit Date"></asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Doc Status" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Comments" HeaderText="Comments"></asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="Rejection Code" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
                <!-- Main Content Panel Ends-->
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <a onclick="javascript:window.close();" href="#" class="allbtn">Close
                    <%--<IMG id="imgClose" alt="" src="../../images/close.gif" border="0">--%>
                </a>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
