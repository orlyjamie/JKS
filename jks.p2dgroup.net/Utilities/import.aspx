<%@ Page Language="c#" CodeBehind="import.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.Utilities.import" %>

<%@ Register TagPrefix="uc1" TagName="banner" Src="../Utilities/banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUser" Src="../Utilities/menuUser.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Import Data</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        // ===============================================================
        function handler(e) {
            e = window.event;
            alert('Please select a file from open file dialog');
            e.returnValue = false;
        }
        // ======================================================================
        function right(e) {
            var msg = "Sorry, you don't have permission to right-click.";
            if (navigator.appName == 'Netscape' && e.which == 3) {
                alert(msg);
                return false;
            }
            if (navigator.appName == 'Microsoft Internet Explorer' && event.button == 2) {
                alert(msg);
                return false;
            }
            else return true;
        }
        // ======================================================================
        function NoEdit() {
            alert("Sorry, you cannot cut or paste text.");
            return false;
        }
        // ======================================================================

    </script>
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
                <!--
						<P><asp:hyperlink id="Hyperlink3" runat="server" NavigateUrl="import.aspx?exp=3">Click here to import</asp:hyperlink></P>
						
						-->
                <input onmouseup="right();" oncut="return NoEdit();" onmousedown="right();" onpaste="return NoEdit();"
                    id="fileImport" onkeydown="javascript:handler();" type="file" name="fileImport"
                    runat="server">
                <p>
                    <asp:Button ID="btnSubmit" TabIndex="6" runat="server" CssClass="SubmitButton" Width="91px"
                        BorderStyle="None" Height="23px"></asp:Button>
                    <asp:Label ID="lblErr" runat="server" Width="672px" ForeColor="Red"></asp:Label></p>
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
