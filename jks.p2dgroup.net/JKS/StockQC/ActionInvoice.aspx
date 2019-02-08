<%@ Page Language="c#" CodeBehind="ActionInvoice.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.StockQC.ActionInvoice" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ActionInvoice</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table style="width: 464px; height: 261px" width="464">
        <tr>
            <td valign="top" width="100%" align="center">
                <!-- Main Content Panel Starts-->
                <table id="Table1" style="width: 464px; height: 242px" cellspacing="1" cellpadding="1"
                    width="464" border="0">
                    <tr>
                        <td class="PageHeader" style="height: 18px" align="center" colspan="2">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader" Visible="True"> Update Invoice Status</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal Body" style="width: 181px; height: 14px">
                            <font size="2">
                                <asp:Label ID="lblType" CssClass="NormalBody" runat="server"></asp:Label></font>
                        </td>
                        <td style="height: 14px" colspan="5">
                            <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal Body" style="width: 181px">
                            <font class="NormalBody" size="2">Invoice Type</font>
                        </td>
                        <td class="Normal Body" style="width: 145px">
                            <asp:Label ID="lblinvoicetype" CssClass="NormalBody" runat="server"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 181px; height: 21px" width="181">
                            <font class="NormalBody" size="2">Supplier&nbsp;</font>
                        </td>
                        <td style="width: 274px; height: 21px">
                            <asp:Label ID="lblsupplier" CssClass="NormalBody" runat="server"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 181px">
                            <font size="2">
                                <asp:Label ID="lbldate" CssClass="NormalBody" runat="server"></asp:Label></font>
                        </td>
                        <td style="width: 274px">
                            <asp:Label ID="lblinvoicedate" CssClass="NormalBody" runat="server"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px; height: 22px">
                            <font size="2">
                                <asp:Label ID="lblstatus" CssClass="NormalBody" runat="server"></asp:Label></font>
                        </td>
                        <td style="width: 274px; height: 22px">
                            <asp:Label ID="lblinvoicestatus" CssClass="NormalBody" runat="server"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px; height: 10px">
                            <font class="NormalBody" size="2">VatCode</font>
                        </td>
                        <td style="width: 274px; height: 10px">
                            <asp:Label ID="lblvatcode" CssClass="NormalBody" runat="server"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px; height: 18px">
                            <font class="NormalBody" size="1">AP CompanyID</font>
                        </td>
                        <td style="width: 274px; height: 18px" colspan="2">
                            <asp:Label ID="lblAPCompanyID" CssClass="NormalBody" runat="server"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 8px">
                        </td>
                        <td style="width: 274px; height: 8px">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 8px" colspan="2">
                            <asp:DataGrid ID="grdInvCur" runat="server" Width="100%" BorderStyle="None" BorderColor="#999999"
                                BackColor="White" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False"
                                Font-Names="verdana,Tahoma,Arial" Font-Size="10pt">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Line No">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblNo" Text='<%# DataBinder.Eval(Container, "DataItem.SerialNo") %>'>
                                            </asp:Label>
                                            <asp:Label Visible="False" runat="server" ID="lblInvDtlId" Text='<%# DataBinder.Eval(Container, "DataItem.sID") %>'>
                                            </asp:Label>
                                            <asp:Label Visible="False" runat="server" ID="lblBuyersProdCode" Text='<%# DataBinder.Eval(Container, "DataItem.BuyersProdCode") %>'>
                                            </asp:Label>
                                            <asp:Label Visible="False" runat="server" ID="lblColor" Text='<%# DataBinder.Eval(Container, "DataItem.New_Definable1") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Order No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Product Code">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlProdCode" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Color">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlColor" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                                    Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 8px">
                        </td>
                        <td style="width: 274px; height: 8px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnsubmit" CssClass="ButtonCss" runat="server" Width="100px" Height="24px"
                                BorderStyle="None" Text="Submit" ToolTip="Submit"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btndelete" CssClass="ButtonCss" runat="server" Width="100px" Height="24px"
                                BorderStyle="None" Text="Delete" ToolTip="Delete"></asp:Button>
                        </td>
                    </tr>
                </table>
                <!-- Main Content Panel Ends-->
                <asp:Label ID="lblMessege" runat="server" CssClass="NormalBody" BorderStyle="None"
                    ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
