<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="banner.ascx.cs" Inherits="CBSolutions.ETH.Web.Utilities.banner"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!-- #include file="../includes/topSupp.aspx" -->
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td class="Banner">
            Welcome
            <asp:Label ID="lblUser" runat="server" CssClass="Banner">User Name</asp:Label>
            <br>
            <asp:Label ID="lblCompany" runat="server" CssClass="Banner">Company</asp:Label>
        </td>
        <td align="right" class="Banner">
            <asp:HyperLink ID="hypLogOff" runat="server" ImageUrl="../images/btnLogOff.jpg" NavigateUrl="../default.aspx">HyperLink</asp:HyperLink>
        </td>
    </tr>
</table>
