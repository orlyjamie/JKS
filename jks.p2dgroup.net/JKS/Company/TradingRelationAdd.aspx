<%@ Page Language="c#" CodeBehind="TradingRelationAdd.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.TradingRelationAdd" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Add Trading Relationship</title>
    <meta content="True" name="vs_showGrid">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:banner ID="Banner1" runat="server"></uc1:banner>
    </asp:Panel>
    <table width="100%">
        <tbody>
            <tr>
                <td valign="top" width="150" bgcolor="gainsboro">
                    <uc1:menuUserNL ID="Menuusernl2" runat="server"></uc1:menuUserNL>
                </td>
                <td>
                    <table>
                        <tr>
                            <td class="PageHeader">
                                <asp:Label ID="lblHeader" runat="server">Label</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="right" class="NormalBody">
                                <strong>Enter company name to search&nbsp;:</strong> &nbsp;
                                <asp:TextBox ID="tbKeyWord" TabIndex="2" runat="server" CssClass="MyInput" Width="200px"
                                    MaxLength="50"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                        ID="Button1" TabIndex="6" runat="server" CssClass="SubmitButton" Width="91px"
                                        Height="23px" BorderStyle="None"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="NormalBody" valign="top" align="center">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:DataGrid ID="grdCompany" runat="server" Width="100%" Height="88px" CellPadding="3"
                                    GridLines="Vertical" AutoGenerateColumns="False" CssClass="listingArea" AllowPaging="True">
                                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                    <ItemStyle></ItemStyle>
                                    <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="Select">
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRelation" runat="server" CssClass="BorderLessInput" BorderWidth="0px"
                                                    BorderStyle="None" BorderColor="White" ForeColor="White"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="CompanyCode" HeaderText="Network ID">
                                            <HeaderStyle Width="150px"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Supplier"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Supplier">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplierCompanyName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CompanyName")%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="CompanyID" HeaderText="CompanyID"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Company Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbCompanyCode" runat="server" MaxLength="10"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Vendor Class">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbVendorClass" runat="server" MaxLength="10"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Currency Type">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlCurrencyType" runat="server" CssClass="MyInput" OnSelectedIndexChanged="ddlCurrencyType_SelectedIndexChanged"
                                                    DataTextField="CurrencyCode" DataValueField="CurrencyTypeID">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" PageButtonCount="5"
                                        Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                                <br />
                                <asp:Button ID="btnAdd" runat="server" CssClass="SubmitButton"></asp:Button>
                            </td>
                        </tr>
                    </table>
        </tbody>
        </td> </tr>
    </table>
    </form>
</body>
</html>
