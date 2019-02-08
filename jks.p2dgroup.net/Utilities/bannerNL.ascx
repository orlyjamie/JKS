<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="bannerNL.ascx.cs" Inherits="CBSolutions.ETH.Web.Utilities.bannerNL"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!--#include file="../includes/top2.aspx"-->
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr bordercolor="dimgray">
        <td class="BannerNL" style="padding-right: 4px; padding-left: 4px; padding-bottom: 4px;
            padding-top: 4px">
            Welcome
            <asp:Label ID="lblUser" runat="server" CssClass="BannerNL">User Name</asp:Label>
            <br>
            <asp:Label ID="lblCompany" runat="server" CssClass="BannerNL">Company</asp:Label>
        </td>
        <td align="right" class="BannerNL">
            <asp:HyperLink ID="hypLogOff" Visible="False" runat="server" CssClass="ButtonCssNL"
                NavigateUrl="../newlookdefault.aspx">Log out</asp:HyperLink>
            <input type="button" class="ButtonCssNL" value="Log out" style="width: 100px; height: 24px"
                onclick="location.href='../../newlookdefault.aspx';">
        </td>
    </tr>
</table>
