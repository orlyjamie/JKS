<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CMSEntry.aspx.cs" Inherits="SiteManager_CMSEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>CMSEntry</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../Utilities/ETH.css" type="text/css" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%">
        <tr>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" border="0" cellspacing="1" cellpadding="1" width="100%">
                    <tr>
                        <td style="height: 16px" class="PageHeader" colspan="4">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader">
								Add Page
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16%; height: 14px" class="GridCaption">
                            Page Name:
                        </td>
                        <td style="height: 14px">
                            <asp:TextBox ID="txtPageName" runat="server" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16%; height: 14px" class="GridCaption">
                            Page Url:
                        </td>
                        <td style="height: 14px">
                            <asp:TextBox ID="txtPageUrl" runat="server" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16%; height: 14px" class="GridCaption">
                            Is Parent Page:
                        </td>
                        <td style="height: 14px">
                            <asp:CheckBox ID="chkIsParent" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" class="GridCaption">
                            Parent Page
                        </td>
                        <td style="height: 14px">
                            <asp:DropDownList ID="ddlParentPage" runat="server" CssClass="NormalBody" Width="200"
                                AutoPostBack="True" DataTextField="PageTitle" DataValueField="PageID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16%; height: 14px" class="GridCaption">
                            Page Order :
                        </td>
                        <td style="height: 14px">
                            <asp:TextBox ID="txtPageOrder" runat="server" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" class="GridCaption">
                            ShowOnHeader :
                        </td>
                        <td style="height: 14px">
                            <asp:CheckBox ID="chkHeader" runat="server" />
                            <small>Check if it will show on header</small>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" class="GridCaption">
                            ShowOnFooter :
                        </td>
                        <td style="height: 14px">
                            <asp:CheckBox ID="chkFooter" runat="server" />
                            <small>Check if it will show on footer</small>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" class="GridCaption">
                            IsActive :
                        </td>
                        <td style="height: 14px">
                            <asp:CheckBox ID="chkIsActive" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" class="GridCaption">
                        </td>
                        <td style="height: 14px">
                            <asp:Button ID="btnSubmit" runat="server" Style="borderstyle: None" CssClass="ButtonCss"
                                Text="Submit" Width="100" Height="24"></asp:Button>
                            <asp:Button ID="btnReset" runat="server" Style="borderstyle: None" CssClass="ButtonCss"
                                Text="Reset" Width="100" Height="24"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Label runat="server" ID="lblMessage"></asp:Label>
    <div>
        <table>
            <tr>
                <td>
                    <asp:DataGrid ID="dgvCMSEntry" runat="server" Width="896px" BorderStyle="None" Height="88px"
                        BorderColor="#999999" BackColor="White" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                        AutoGenerateColumns="False" Font-Names="verdana" Font-Size="10pt">
                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                        <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                        </ItemStyle>
                        <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                            BackColor="#3399CC"></HeaderStyle>
                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="PageID" HeaderText="PageID">
                                <HeaderStyle Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PageTitle" HeaderText="Page Title"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NavigateUrl" HeaderText="Navigate Url"></asp:BoundColumn>
                            <asp:BoundColumn DataField="PageOrder" HeaderText="Page Order">
                                <HeaderStyle Width="300px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Is Parent Page">
                                <ItemTemplate>
                                    <asp:Label ID="lblIsParentPage" runat="server" Text='<%#GetStatus(DataBinder.Eval(Container.DataItem, "IsParentPage").ToString())%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Show On Footer">
                                <ItemTemplate>
                                    <asp:Label ID="lblShowOnFooter" runat="server" Text='<%#GetStatus(DataBinder.Eval(Container.DataItem, "ShowOnFooter").ToString())%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Show On Header">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%#GetStatus(DataBinder.Eval(Container.DataItem, "ShowOnHeader").ToString())%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnEdit" runat="server" CommandName="ED" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PageID")%>'>Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnDelete" runat="server" CommandName="DEL" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PageID")%>'>Delete</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                        </PagerStyle>
                    </asp:DataGrid>
                </td>
                </TD>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
