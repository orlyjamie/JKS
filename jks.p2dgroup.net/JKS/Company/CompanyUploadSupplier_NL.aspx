<%@ Page Language="c#" CodeBehind="CompanyUploadSupplier_NL.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.CompanyUploadSupplier_NL" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>CompanyUploadSupplier_NL</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
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
        function HideBuyerRow() {
            if ('<%=bTradingRelationFlag%>' == 'True') {
                document.getElementById("trSelectABuyer").style.display = "none";
            }
        }
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0" onload="javascript:HideBuyerRow();">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel2" Style="z-index: 102; left: 0px" runat="server" Width="100%">
    </asp:Panel>
    <table width="100%">
        <tr>
            <td valign="top" width="150" bgcolor="gainsboro">
                <table style="width: 144px; height: 232px">
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table width="100%" border="0">
                    <tr>
                        <td style="width: 46px; height: 21px" width="46">
                        </td>
                        <td colspan="2" class="PageHeader">
                            <asp:Label ID="lblHeader" runat="server">Upload Suppliers</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trSelectABuyer">
                        <td style="width: 46px; height: 16px" width="46">
                        </td>
                        <td class="NormalBody" style="width: 126px; height: 16px" width="126">
                            Select A Buyer
                        </td>
                        <td style="width: 202px; height: 16px" width="202">
                            <asp:DropDownList ID="cboBuyer" runat="server" Width="312px" CssClass="MyInput" Height="24px">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 16px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" style="width: 125px">
                        </td>
                        <td style="width: 202px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" style="width: 125px">
                            Upload File&nbsp;<font color="red">*</font>
                        </td>
                        <td style="width: 202px">
                            <input id="fileUploadSuppliers" type="file" name="fileUploadSuppliers" runat="server"
                                onkeydown="javascript:handler();" onmousedown="right();" onmouseup="right();"
                                oncut="return NoEdit();" onpaste="return NoEdit();">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" style="width: 125px">
                        </td>
                        <td style="width: 202px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td align="center" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnSubmit" TabIndex="6" runat="server" CssClass="SubmitButton" Width="91px"
                                BorderStyle="None" Height="23px"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td align="center" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td align="left" colspan="3">
                            <asp:Label ID="lblErr" runat="server" ForeColor="Red" Font-Size="Smaller"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" colspan="6">
                            <font color="red">* = Mandatory Field</font>
                        </td>
                    </tr>
                </table>
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
