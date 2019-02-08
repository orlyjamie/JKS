<%@ Page Language="c#" CodeBehind="CMSHistroy.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.CMS.CMSHistroy" %>

<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>WTR History</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">

        function fn_CMSValidation() {
            if (document.all.ddlCompany.selectedIndex == "0") {
                alert('Please select a Company.');
                return false;
            }
            if (document.all.ddlDepartment.selectedIndex == "0") {
                alert('Please select a Department.');
                return false;
            }
            if (document.all.ddlWeekStartDate.selectedIndex == "0") {
                alert('Please select a Week Start Date.');
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
    <table id="Table1" width="600">
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
                <table class="listingArea02" id="Table3" width="300" border="0">
                    <tr>
                        <td class="PageHeader" style="width: 300px" colspan="3">
                            <asp:Label ID="lblHeader" runat="server" Width="100%">WTR History</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 100px; height: 23px">
                            Company
                        </td>
                        <td style="width: 200px; height: 23px">
                            <asp:DropDownList ID="ddlCompany" Style="width: 200px" AutoPostBack="True" DataValueField="CompanyID"
                                DataTextField="CompanyName" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 100px">
                            Department
                        </td>
                        <td style="width: 200px">
                            <asp:DropDownList ID="ddlDepartment" Style="width: 200px" AutoPostBack="True" DataValueField="DepartmentID"
                                DataTextField="Department" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 100px">
                            Week Start Date
                        </td>
                        <td style="width: 200px">
                            <asp:DropDownList ID="ddlWeekStartDate" Style="width: 200px" DataValueField="weekStartDateID"
                                DataTextField="weekStartDate" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
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
