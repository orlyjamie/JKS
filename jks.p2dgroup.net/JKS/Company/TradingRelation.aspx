<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>

<%@ Page Language="c#" CodeBehind="TradingRelation.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.TradingRelation" %>

<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Trading Relationship</title>
    <meta content="True" name="vs_showGrid">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function GoToURL() {
            if (document.all.cboBuyer.selectedIndex != 0) {
                var iBuyerCompanyID = null;
                iBuyerCompanyID = document.all.cboBuyer.value;
                window.location.href = "CompanyUploadSupplier_NL.aspx?TRDBCID=" + iBuyerCompanyID;
            }
            else
                document.getElementById("sp_SelectCompany").innerHTML = "Please select a buyer company.";
        }
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
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
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table width="100%">
                    <tr>
                        <td class="NormalBody" style="width: 223px">
                            Select a Buyer Company
                        </td>
                        <td>
                            <asp:DropDownList ID="cboBuyer" runat="server" CssClass="MyInput" Width="312px" DataValueField="CompanyID"
                                DataTextField="CompanyName" Height="24px">
                            </asp:DropDownList>
                            &nbsp;<asp:Button ID="Button1" runat="server" CssClass="ButtonCss" Width="91px" Height="23px"
                                BorderStyle="None" BorderWidth="0px" Text="Submit" ToolTip="Submit"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 223px">
                            <asp:HyperLink ID="hypAdd" runat="server" Visible="False" ImageUrl="../../images/AddTradeRelation.jpg"
                                NavigateUrl="TradingRelationAdd.aspx">Add New Trading Relation</asp:HyperLink>&nbsp;
                            <a onclick="javascript:GoToURL();" href="#">
                                <img src="../../images/uploadsupplier_m.gif" border="0">
                            </a>
                        </td>
                        <td class="NormalBody" style="height: 19px">
                            <span id="sp_SelectCompany" style="color: red"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" align="center" colspan="2">
                            <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:DataGrid ID="grdCompany" runat="server" Width="100%" Height="88px" BorderStyle="None"
                                BorderWidth="1px" Visible="False" Font-Size="10pt" Font-Names="verdana,Tahoma,Arial"
                                AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3" BackColor="White"
                                BorderColor="#999999" AllowPaging="True">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <EditItemStyle CssClass="NormalBody"></EditItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" CssClass="NormalBody"
                                    BackColor="White"></ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    CssClass="NormalBody" BackColor="#3399CC"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Delete">
                                        <HeaderStyle Width="50px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hyperlinkDelete" runat="server" ImageUrl="../../images/GridDelete.jpg">Delete</asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Supplier" HeaderText="Supplier"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="EmailID" HeaderText="EmailID"></asp:BoundColumn>
                                    <asp:TemplateColumn Visible="False" HeaderText="Send Mail">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkMail" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" PageButtonCount="5"
                                    Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 223px">
                            <asp:ImageButton ID="imgButtonSendMailInfo" runat="server" Visible="False" ImageUrl="../../images/sendlogin_m.gif">
                            </asp:ImageButton>
                        </td>
                        <td>
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
