<%@ Page Language="c#" CodeBehind="upOrders.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.downloadDB.upOrders" %>

<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - DownLoad Files</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }	
    </script>
    <script language="javascript">
        function handler(e) {
            e = window.event;
            alert('Please select a file from open file dialog');
            e.returnValue = false;
        }
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
        function NoEdit() {
            alert("Sorry, you cannot cut or paste text.");
            return false;
        }

    </script>
</head>
<body onbeforeunload="javascript:doHourglass();" bgcolor="#ffffff" leftmargin="0"
    topmargin="0" onunload="javascript:doHourglass();" ms_positioning="GridLayout">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
    </asp:Panel>
    <table width="100%">
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
            <td class="NormalBody" valign="top" align="left">
                <table width="100%">
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label1" runat="server" Width="100%" CssClass="PageHeader">Upload Orders</asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td class="NormalBody" colspan="2">
                            &nbsp;
                        </td>
                        <td class="NormalBody">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" align="left" colspan="5">
                            <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left" class="NormalBody">
                            <!-- Main Content Panel Starts-->
                            <br>
                            Upload File&nbsp;<font color="red">*
                                <input onmouseup="right();" oncut="return NoEdit();" onmousedown="right();" onpaste="return NoEdit();"
                                    id="fileUpload" onkeydown="javascript:handler();" type="file" name="fileUpload"
                                    runat="server"></font>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3">
                            <asp:Button ID="btnUpload" TabIndex="6" runat="server" Width="91px" CssClass="SubmitButton"
                                BorderStyle="None" Height="23px"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
