<%@ Page Language="c#" CodeBehind="export.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.Utilities.DownloadDB" %>

<%@ Register TagPrefix="uc1" TagName="menuUser" Src="../Utilities/menuUser.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../Utilities/banner.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Export Data</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../Utilities/ETH.css" type="text/css" rel="stylesheet">
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form2" method="post" runat="server">
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
                            <uc1:menuUser ID="Menuuser2" runat="server"></uc1:menuUser>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <p>
                </p>
                <p>
                    <table>
                        <tr>
                            <td>
                                Select a buyer company
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="cboCompany" Width="336px" runat="server" DataTextField="CompanyName"
                                    DataValueField="CompanyID" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </p>
                <p>
                </p>
                <p>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="export.aspx?exp=1">Click here to export (Registration)</asp:HyperLink>
                </p>
                <p>
                    <p>
                        <asp:HyperLink ID="Hyperlink2" runat="server" NavigateUrl="export.aspx?exp=2">Click here to export (Authorisation)</asp:HyperLink></p>
                    <p>
                        <asp:Label ID="outError" runat="server" Width="672px" ForeColor="Red"></asp:Label></p>
                    <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
