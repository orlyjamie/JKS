<%@ Page Language="c#" CodeBehind="ExportInvoice.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.downloadDB.ExportInvoice" %>

<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>download Options</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
        function validation() {
            if (document.getElementById("ddlBuyerCompany").selectedIndex == 0) {
                alter("Please select a Child company.");
                return false;
            }
            else
                return true;
        }
        function FnCompleted() {
            alert('Completed.');
            window.location.href = 'DownLoadFiles.aspx';
        }
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="d_option" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
    </asp:Panel>
    <table class="NormalBody" style="width: 720px; height: 175px">
        <tr>
            <td valign="top" width="150" bgcolor="gainsboro">
                <table>
                    <tr>
                        <td>
                            <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="NormalBody" valign="top">
                <p class="PageHeader">
                    Invoice Export</p>
                <table class="NormalBody" id="Table3" cellspacing="2" cellpadding="2" border="0"
                    style="width: 559px; height: 121px">
                    <tr>
                        <td class="NormalBody">
                            Child company name :
                            <asp:DropDownList ID="ddlBuyerCompany" runat="server" Width="264px" CssClass="MyInput"
                                DataValueField="CompanyID" DataTextField="CompanyName">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnDwnloadDB" runat="server" Width="81px" CssClass="ButtonCss" Height="24px"
                                BorderStyle="None" ToolTip="Submit" Text="Submit"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
