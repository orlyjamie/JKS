<%@ Page Language="c#" CodeBehind="InvoiceConfirmation_DN.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.DebitNote.InvoiceConfirmation_DN" %>

<%@ Register TagPrefix="uc1" TagName="menuUser" Src="~/JKS/Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc2" TagName="banner" Src="~/Utilities/banneretc.ascx" %>
<html>
<head>
    <title>P2D Network - DebitNote Log</title>
    <script language="javascript" src="../WinOpener.js"></script>
    <script language="javascript">
		
    </script>
    <link href="../Utilities/ETH.css" type="text/css" rel="stylesheet">
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%"
        CssClass="Banner">
        <uc2:banner ID="Banner2" runat="server"></uc2:banner>
    </asp:Panel>
    <table width="100%">
        <tr>
            <td valign="top" width="150" bgcolor="gainsboro">
                <table>
                    <tr>
                        <td>
                            <uc1:menuUser ID="Menuuser2" runat="server"></uc1:menuUser>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td width="1" height="16">
                        </td>
                        <td class="PageHeader" colspan="4" height="16">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="16">
                        </td>
                        <td class="NormalBody" colspan="4" height="16">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="20">
                        </td>
                        <td class="GridCaption" width="156" height="20">
                            DebitNote No.&nbsp;&nbsp;
                        </td>
                        <td width="170" height="20">
                            <asp:Label ID="lblDocument_No" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" width="177" height="20">
                            Buyer
                        </td>
                        <td height="20">
                            <asp:Label ID="lblAP_Company_ID" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="16">
                        </td>
                        <td class="GridCaption" width="156" height="16">
                            Associated Invoice No.
                        </td>
                        <td width="170" height="16">
                            <asp:Label ID="lblAssociated_Invoice_No" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" width="177" height="16">
                            Supplier
                        </td>
                        <td height="16">
                            <asp:Label ID="lblVendorID" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="16">
                        </td>
                        <td class="GridCaption" width="156" height="16">
                            Invoice Date
                        </td>
                        <td width="170" height="16">
                            <asp:Label ID="lblDocument_Date" runat="server" CssClass="NormalBody" Height="15px"></asp:Label>
                        </td>
                        <td class="GridCaption" width="177" height="16">
                            Status
                        </td>
                        <td height="16">
                            <asp:Label ID="lblStatus" runat="server" CssClass="NormalBody" Font-Bold="True">lblStatus</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="16">
                        </td>
                        <td class="GridCaption" width="156" height="16">
                            Currency
                        </td>
                        <td width="170" height="16">
                            <asp:Label ID="lblCurrency" runat="server" Width="188px" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" width="177" height="16">
                            Discount Given
                        </td>
                        <td height="16">
                            <asp:Label ID="lblNew_DiscountGiven" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="4">
                        </td>
                        <td class="GridCaption" width="156" height="4">
                            Payment Date
                        </td>
                        <td width="170" height="4">
                            <asp:Label ID="lblpaymentdate" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="19">
                        </td>
                        <td class="GridCaption" width="156" height="19">
                            Payment Method
                        </td>
                        <td width="170" height="19">
                            <asp:Label ID="lblNew_PaymentMethod" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="4">
                        </td>
                        <td class="GridCaption" width="156" height="4">
                            PaymentDueDate
                        </td>
                        <td width="170" height="4">
                            <asp:Label ID="lblNew_PaymentDueDate" runat="server" Width="122px" CssClass="NormalBody"
                                Height="16px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="4">
                        </td>
                        <td class="NormalBody" width="156" height="4">
                        </td>
                        <td width="170" height="4">
                        </td>
                        <td class="NormalBody" width="175" colspan="2" height="4">
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="27">
                        </td>
                        <td class="GridCaption" valign="middle" width="156" height="27">
                            Supplier&nbsp;Address
                        </td>
                        <td colspan="3" height="27">
                            <asp:Label ID="lblSupplierAddress" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="4">
                        </td>
                        <td width="156" height="4">
                        </td>
                        <td colspan="3" height="4">
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="27">
                        </td>
                        <td class="GridCaption" valign="middle" width="156" height="27">
                            Delivery Address
                        </td>
                        <td colspan="3" height="27">
                            <asp:Label ID="lblDeliveryAddress" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="4">
                        </td>
                        <td width="156" height="4">
                        </td>
                        <td colspan="3" height="4">
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="26">
                        </td>
                        <td class="GridCaption" valign="middle" width="156" height="26">
                            Invoice Address
                        </td>
                        <td colspan="3" height="26">
                            <asp:Label ID="lblInvoiceAddress" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="4">
                        </td>
                        <td width="156" height="4">
                        </td>
                        <td colspan="3" height="4">
                        </td>
                    </tr>
                    <tr>
                        <td width="16" height="12">
                        </td>
                        <td class="GridCaption" colspan="4" height="12">
                            Invoice Details
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="4">
                            <asp:DataGrid ID="grdInvoiceDetails" runat="server" Width="100%" BorderColor="#999999"
                                BackColor="White" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                AutoGenerateColumns="False" Font-Names="verdana,Tahoma,Arial" Font-Size="10pt">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="debitnotelineno" HeaderText="DebitNote Line No">
                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DDescription" HeaderText="Description"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="BuyersProdCode" HeaderText="Buyers Product Code"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="PurOrderNo" HeaderText="P.O. No"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_Definable1" HeaderText="Colour"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Quantity" HeaderText="Qty">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Price" HeaderText="Price">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="NettValue" HeaderText="Net Total">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                </PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td valign="top" align="left" colspan="4">
                            <table id="Table2" width="100%">
                                <tr>
                                    <td width="43%">
                                        <table id="Table3" cellspacing="0" cellpadding="2" width="50%" border="0">
                                            <tr>
                                                <td width="39%">
                                                    <span id="sp_Back"><a onclick="javascript:history.back();" href="#">
                                                        <img src="../images/back_2.gif" border="0"></a></span>
                                                </td>
                                                <td width="10">
                                                </td>
                                                <td width="169">
                                                    <asp:Button ID="btnGeneratePDF" TabIndex="13" runat="server" Width="139px" CssClass="GeneratePDF_2"
                                                        Height="23px" BorderStyle="None" BorderWidth="0px"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" align="right" width="57%">
                                        <table id="Table4" width="300">
                                            <tr>
                                                <td class="GridCaption" align="right" width="170" height="16">
                                                    Net Total
                                                </td>
                                                <td align="right" height="16">
                                                    <asp:Label ID="lblNett_Amount" runat="server" CssClass="NormalBody"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridCaption" align="right" width="170" height="14">
                                                    VAT
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:ShowMessage()"
                                                        ImageUrl="../images/i.jpg"></asp:HyperLink>
                                                </td>
                                                <td align="right" height="14">
                                                    <asp:Label ID="lblTax_Amoun" runat="server" CssClass="NormalBody"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridCaption" align="right" width="170">
                                                    Total
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblGross_Amount" runat="server" CssClass="NormalBody"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
