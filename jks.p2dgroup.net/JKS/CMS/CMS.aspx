<%@ Page Language="c#" CodeBehind="CMS.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.CMS.CMS" %>

<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>WTR</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">

        function fn_Error() {
            alert('This department has not been set up for CMS');
        }
        function fn_CMSValidation() {

            if (document.all.ddlCompany.selectedIndex == "0") {
                alert('Please select a Company.');
                return false;
            }
            if (document.all.ddlDepartment.selectedIndex == "0") {
                alert('Please select a Department.');
                return false;
            }

            return true;
        }
	
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:Panel ID="Panel2" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
    </asp:Panel>
    <table width="600" id="Table1">
        <tr>
            <td valign="top" width="150" bgcolor="gainsboro">
                <table id="Table2">
                    <tr>
                        <td>
                            <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table width="300" border="0" id="Table3" class="listingArea02">
                    <tr>
                        <td colspan="3" style="width: 300px" class="PageHeader">
                            <asp:Label ID="lblHeader" runat="server" Width="100%">WTR</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 100px; height: 25px">
                            Company
                        </td>
                        <td style="width: 200px; height: 25px">
                            <asp:DropDownList ID="ddlCompany" runat="server" DataTextField="CompanyName" DataValueField="CompanyID"
                                AutoPostBack="True" Style="width: 200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 100px">
                            Department
                        </td>
                        <td style="width: 200px">
                            <asp:DropDownList ID="ddlDepartment" runat="server" DataTextField="Department" DataValueField="DepartmentID"
                                Style="width: 200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSubmit" TabIndex="6" runat="server" CssClass="SubmitButton_ETC_CMS">
                            </asp:Button>
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
