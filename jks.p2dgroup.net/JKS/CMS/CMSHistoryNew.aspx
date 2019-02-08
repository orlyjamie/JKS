<%@ Page Language="c#" CodeBehind="CMSHistoryNew.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.CMS.CMSHistoryNew" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>WTR History</title>
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
        function ToDestination() {
            var strHTML = '';
            jQuery("#listSiteFrom option:selected").each(function () {
                strHTML += "<option value='" + jQuery(this).val() + "'>" + jQuery(this).text() + "</option>";
                jQuery(this).remove();
            });
            jQuery("#listSiteTo").append(strHTML);
        }

        function ToSource() {
            var strHTML = '';
            jQuery('#listSiteTo option:selected').each(function () {
                strHTML += "<option value='" + jQuery(this).val() + "'>" + jQuery(this).text() + "</option>";
                jQuery(this).remove();
            });
            jQuery("#listSiteFrom").append(strHTML);
        }

        function AllToDestination() {
            var strHTML = '';
            strHTML = jQuery("#listSiteFrom").html();
            jQuery("#listSiteFrom").html('');
            jQuery("#listSiteTo").append(strHTML);
        }

        function AllToSource() {
            var strHTML = '';
            strHTML = jQuery("#listSiteTo").html();
            jQuery("#listSiteTo").html('');
            jQuery("#listSiteFrom").append(strHTML);
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
                <!-- Main Content Panel Starts-->
                <table style="width: 616px; height: 328px" id="Table3" class="listingArea02" border="0"
                    width="616">
                    <tr>
                        <td style="width: 600px" class="PageHeader" colspan="4">
                            <asp:Label ID="lblHeader" runat="server" Width="100%">WTR History</asp:Label>
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
                        <td style="width: 100px" class="NormalBody">
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
                        <td style="width: 100px" class="NormalBody" valign="top">
                            Department
                        </td>
                        <td style="width: 207px">
                            <asp:ListBox Style="z-index: 0; border-bottom: #3399cc 1px solid; border-left: #3399cc 1px solid;
                                border-top: #3399cc 1px solid; border-right: #3399cc 1px solid" ID="listSiteFrom"
                                runat="server" Width="200" Height="200" SelectionMode="Multiple"></asp:ListBox>
                            &nbsp;
                        </td>
                        <td align="center" valign="top">
                            <!--
									<div class="useradd">
										<ul>
											<li onclick="javascript:ToDestination();">
												<A href="javascript:void(0)"><IMG style="BORDER-BOTTOM: medium none; BORDER-LEFT: medium none; OUTLINE-STYLE: none; OUTLINE-COLOR: invert; OUTLINE-WIDTH: medium; BORDER-TOP: medium none; BORDER-RIGHT: medium none"
														alt="" src="../../images/next_l.gif"  id="IMG1"> </A>
											</li>
											<li onclick="javascript:AllToDestination();">
												<A href="javascript:void(0)"><IMG style="BORDER-BOTTOM: medium none; BORDER-LEFT: medium none; OUTLINE-STYLE: none; OUTLINE-COLOR: invert; OUTLINE-WIDTH: medium; BORDER-TOP: medium none; BORDER-RIGHT: medium none"
														alt="" src="../../images/next_l_all.gif" > </A>
											</li>
											<li onclick="javascript:ToSource();">
												<A href="javascript:void(0)"><IMG style="BORDER-BOTTOM: medium none; BORDER-LEFT: medium none; OUTLINE-STYLE: none; OUTLINE-COLOR: invert; OUTLINE-WIDTH: medium; BORDER-TOP: medium none; BORDER-RIGHT: medium none"
														alt="" src="../../images/next_r.gif" ></A>
											</li>
											<li onclick="javascript:AllToSource();">
												<A href="javascript:void(0)"><IMG style="BORDER-BOTTOM: medium none; BORDER-LEFT: medium none; OUTLINE-STYLE: none; OUTLINE-COLOR: invert; OUTLINE-WIDTH: medium; BORDER-TOP: medium none; BORDER-RIGHT: medium none"
														alt="" src="../../images/next_r_all.gif" ></A>
											</li>
										</ul>
									</div>
									-->
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnNext" runat="server" CssClass="btnNext"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnNextAll" runat="server" CssClass="btnNextAll"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnBack" runat="server" CssClass="btnPrev"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnBackAll" runat="server" CssClass="btnPrevAll"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 200px">
                            <asp:ListBox Style="z-index: 0; border-bottom: #3399cc 1px solid; border-left: #3399cc 1px solid;
                                border-top: #3399cc 1px solid; border-right: #3399cc 1px solid" ID="listSiteTo"
                                runat="server" Width="200" Height="200" SelectionMode="Multiple" DataTextField="Department"
                                DataValueField="DepartmentID"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 310px" colspan="2" align="center">
                        </td>
                        <td style="width: 129px">
                        </td>
                        <td style="width: 200px" align="center">
                            <asp:Button Style="z-index: 0" ID="btnSubmit" TabIndex="6" runat="server" CssClass="SubmitButton_ETC_CMS">
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
