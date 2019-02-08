<%@ Page Language="c#" CodeBehind="threshholdNL.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.threshholdNL" %>

<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>threshholdNL</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function ValidateFormSubmission() {

            if (isNaN(document.all.txtTimeLimit.value)) {
                alert('Time limit should be a numeric value.');
                return (false);
            }
            else {
                return (true);
            }
        }
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form1" onsubmit="javascript:return ValidateFormSubmission();" method="post"
    runat="server">
    <asp:Panel ID="Panel2" Style="z-index: 102; left: 0px" runat="server" Width="100%">
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
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table width="100%" border="0">
                    <tr>
                        <td style="width: 46px; height: 21px" width="46">
                        </td>
                        <td style="width: 340px" colspan="2" class="PageHeader">
                            <asp:Label ID="lblHeader" runat="server">Label</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" style="width: 132px">
                            Time Limit (Hrs)<font color="red"> *</font>
                        </td>
                        <td style="width: 204px">
                            <asp:TextBox ID="txtTimeLimit" TabIndex="2" runat="server" Width="72px" MaxLength="2"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rvTimeLimit" runat="server" Font-Size="Smaller" ControlToValidate="txtTimeLimit"
                                ErrorMessage="Please enter time limit."></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" style="width: 132px">
                        </td>
                        <td style="width: 204px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" style="width: 132px">
                        </td>
                        <td style="width: 204px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnSubmit" TabIndex="6" runat="server" Width="91px" CssClass="ButtonCss"
                                BorderStyle="None" Height="23px" Text="Submit" ToolTip="Submit"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" colspan="3">
                            <font class="NormalBody">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label></font>
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
