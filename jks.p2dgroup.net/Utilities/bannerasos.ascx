<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="bannerasos.ascx.cs"
    Inherits="CBSolutions.ETH.Web.Utilities.bannerasos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr bordercolor="dimgray">
        <td class="Banner nobg" style="padding-right: 4px; padding-left: 4px; padding-bottom: 4px;
            padding-top: 4px;">
            Welcome
            <asp:Label ID="Label3" runat="server" CssClass="Banner nobg">User Name</asp:Label>
            <br>
            <asp:Label ID="Label4" runat="server" CssClass="Banner nobg">Company</asp:Label>
        </td>
        <td align="right" class="Banner nobg">
            <asp:HyperLink ID="HyperLink2" runat="server" ImageUrl="../ImagesAsos/btnLogOff.jpg"
                NavigateUrl="../asosdefault.aspx">Log Off</asp:HyperLink>
        </td>
    </tr>
</table>
