<%@ Page Language="c#" CodeBehind="WTRExport.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.CMS.WTRExport" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>WTR Export</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <script type="text/javascript" src="jquery-1.7.2.js"></script>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../../Utilities/ETH.css">
    <script language="javascript">

        function fn_CMSValidation() {
            if (document.all.ddlCompany.selectedIndex == "0") {
                alert('Please select a Company.');
                return false;
            }
            if (document.all.ddlWeekStartDate.selectedIndex == "0") {
                alert('Please select a Week Start Date.');
                return false;
            }
            var DepartmentName = '<%=ViewState["DepartmentName"]%>';
            if (DepartmentName != "") {
                var result = confirm('Departments  ' + DepartmentName + '  are not closed, and will be excluded from the export file. Do you still want to proceed?');
                return result;
            }
            return true;
        }
        
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:Panel Style="z-index: 102; left: 0px" ID="Panel2" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
    </asp:Panel>
    <table style="width: 920px; height: 169px" id="Table1" width="920">
        <tr>
            <td bgcolor="gainsboro" valign="top" width="150">
                <table id="Table2">
                    <tr>
                        <td>
                            <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width: 616px" id="Table3" class="listingArea02" border="0" width="616">
                    <tr>
                        <td style="width: 600px" class="PageHeader" colspan="4">
                            <asp:Label ID="lblHeader" runat="server" Width="100%">WTR Export</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 23px" class="NormalBody">
                            Company
                        </td>
                        <td style="width: 207px; height: 23px">
                            <asp:DropDownList ID="ddlCompany" Width="200" runat="server" DataTextField="CompanyName"
                                DataValueField="CompanyID" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 129px">
                        </td>
                        <td style="width: 200px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px" class="NormalBody">
                            Week Start Date
                        </td>
                        <td style="width: 207px">
                            <asp:DropDownList ID="ddlWeekStartDate" Width="200" runat="server" DataTextField="weekStartDate"
                                DataValueField="weekStartDateID" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 129px">
                        </td>
                        <td style="width: 200px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 310px" colspan="2" align="center">
                            <asp:Button Style="z-index: 0" ID="btnSubmit" TabIndex="6" runat="server" CssClass="SubmitButton_ETC_CMS">
                            </asp:Button>
                        </td>
                        <td style="width: 129px">
                        </td>
                        <td style="width: 200px" align="center">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
