<%@ Control Language="c#" AutoEventWireup="true" CodeBehind="banneretc.ascx.cs" Inherits="CBSolutions.ETH.Web.Utilities.banneretc"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr bordercolor="dimgray">
        <td class="Banner" style="padding-right: 4px; padding-left: 4px; padding-bottom: 4px;
            padding-top: 4px">
            <asp:Label ID="lblUser" runat="server" Visible="false" CssClass="Banner">User Name</asp:Label>
            <br />
            <asp:Label ID="lblCompany" runat="server" Visible="false" CssClass="Banner">Company</asp:Label>
        </td>
        <td align="right" class="Banner">
            <asp:HyperLink ID="hypLogOff" runat="server" ImageUrl="../images/btnLogOff.jpg" NavigateUrl="../etcdefault.aspx">Log Off</asp:HyperLink>
        </td>
    </tr>
</table>
