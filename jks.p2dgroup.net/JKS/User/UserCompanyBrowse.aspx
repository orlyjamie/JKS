<%@ Page Language="c#" CodeBehind="UserCompanyBrowse.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.UserCompanyBrowse" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc2" TagName="bannerNL" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Browse Companies</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function fn_Validate() {
            if (document.all.hdHubAdminFlag.value == '1') {
                if (document.all.ddlCompany.selectedIndex == 0) {
                    alert('Please select a company to add user.');
                }
                else {
                    window.location.href = "UserEdit.aspx";
                }
            }
            else {
                window.location.href = "UserEdit.aspx";
            }
        }
    </script>
</head>
<body bgcolor="#ffffff" topmargin="0" leftmargin="0">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc2:bannerNL ID="BannerNL1" runat="server"></uc2:bannerNL>
    </asp:Panel>
    <table width="100%">
        <tr>
            <td valign="top" width="150" bgcolor="#dcdcdc">
            </td>
            <td valign="top" align="center">
                <asp:Label ID="lblSelectCompany" runat="server" Font-Bold="True" Visible="False">Select a company to add user</asp:Label>&nbsp;
                &nbsp;
                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="NormalBody" Width="239px"
                    AutoPostBack="True" DataValueField="CompanyID" DataTextField="CompanyName" Visible="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td valign="top" width="150" bgcolor="#dcdcdc">
            </td>
            <td valign="top" align="center">
            </td>
        </tr>
        <tr>
            <td valign="top" width="150" bgcolor="#dcdcdc">
            </td>
            <td valign="top" align="center">
                <asp:Label ID="Label2" runat="server" Width="100%" CssClass="PageHeader" Visible="False">Select User To Edit </asp:Label>
            </td>
        </tr>
        <tr>
            <td width="150" bgcolor="gainsboro" valign="top">
                <table>
                    <tr>
                        <td>
                            <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" align="center">
                <!-- Main Content Panel Starts-->
                <br>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                <br>
                <p>
                    <asp:DataGrid ID="dgUsers" runat="server" Width="100%" Font-Size="10pt" Font-Names="verdana,Tahoma,Arial"
                        AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                        BorderStyle="None" BackColor="White" Height="88px" BorderColor="#999999" OnItemCommand="Datagrid_Click">
                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                        <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                        </ItemStyle>
                        <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                            BackColor="#3399CC"></HeaderStyle>
                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Select">
                                <HeaderStyle Width="50px"></HeaderStyle>
                                <ItemTemplate>
                                    <a href='UserEdit.aspx?UserID=<%#DataBinder.Eval(Container.DataItem,"UserID")%>'>
                                        <img src="../images/GridSelect.jpg" border="0">
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DELETERECORD" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"UserID")%>'
                                        ImageUrl="../images/GridDelete.jpg"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="UserID" ReadOnly="True" HeaderText="UserID">
                                <HeaderStyle Width="300px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="UserType" ReadOnly="True" HeaderText="UserType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UName" ReadOnly="True" HeaderText="Name"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UserName" HeaderText="UserName"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                        </PagerStyle>
                    </asp:DataGrid></p>
                <p>
                    <!-- Main Content Panel Ends-->
                    <a href="#" onclick="fn_Validate();">
                        <img src="../../images/AddUser.jpg" border="0"></a>&nbsp;<input id="hdHubAdminFlag"
                            type="hidden" value="0" name="hdHubAdminFlag" runat="server">
                </p>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
