<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="bannercoop.ascx.cs"
    Inherits="CBSolutions.ETH.Web.Utilities.bannercoop" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!--#include file="../includes/topCoop.aspx"-->
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr bordercolor="dimgray">
        <td class="Banner nobg" style="padding-right: 4px; padding-left: 4px; padding-bottom: 4px;
            padding-top: 4px">
            Welcome
            <asp:Label ID="lblUser" runat="server" CssClass="Banner nobg">User Name</asp:Label>
            <br>
            <asp:Label ID="lblCompany" runat="server" CssClass="Banner nobg">Company</asp:Label>
        </td>
        <td align="right" class="Banner nobg">
            <asp:HyperLink ID="hypLogOff" runat="server" ImageUrl="../images/btnLogOff.jpg" NavigateUrl="../coopdefault.aspx">Log Off</asp:HyperLink>
        </td>
    </tr>
</table>
