<%@ Page Language="c#" CodeBehind="InvoiceConfirmationNL_DN.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.invoice.InvoiceConfirmationNL_DN" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="bannerNB" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Create Invoice (Confirmation)</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script lang="JavaScript">
        function ShowMessage() {
            alert('This VAT amount has been calculated automatically by the system which rounds down to the nearest penny. However, if you use a different rounding method please change the VAT amount manually. ');
        }
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
    <asp:Panel ID="Panel4" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerNB ID="BannerNB1" runat="server"></uc1:bannerNB>
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
                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td style="width: 1px; height: 16px">
                        </td>
                        <td class="PageHeader" style="height: 16px" colspan="4">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td style="height: 16px" colspan="4" class="NormalBody">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 20px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 20px">
                            DebitNote No.&nbsp;&nbsp;
                        </td>
                        <td style="width: 170px; height: 20px">
                            <asp:Label ID="lblDocument_No" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 20px">
                            Buyer
                        </td>
                        <td style="height: 20px">
                            <asp:Label ID="lblAP_Company_ID" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 16px">
                            Associated Invoice No.
                        </td>
                        <td style="width: 170px; height: 16px">
                            <asp:Label ID="lblAssociated_Invoice_No" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Supplier
                        </td>
                        <td style="height: 16px">
                            <asp:Label ID="lblVendorID" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 16px">
                            Invoice Date
                        </td>
                        <td style="width: 170px; height: 16px">
                            <asp:Label ID="lblDocument_Date" runat="server" CssClass="NormalBody" Height="15px"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Status
                        </td>
                        <td style="height: 16px">
                            <asp:Label ID="lblStatus" runat="server" CssClass="NormalBody" Font-Bold="True">lblStatus</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 16px">
                            Currency
                        </td>
                        <td style="width: 170px; height: 16px">
                            <asp:Label ID="lblCurrency" runat="server" CssClass="NormalBody" Width="188px"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Discount Given
                        </td>
                        <td style="height: 16px">
                            <asp:Label ID="lblNew_DiscountGiven" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 4px">
                            Payment Date
                        </td>
                        <td style="width: 170px; height: 4px">
                            <asp:Label ID="lblpaymentdate" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 19px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 19px">
                            Payment Method
                        </td>
                        <td style="width: 170px; height: 19px">
                            <asp:Label ID="lblNew_PaymentMethod" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 4px">
                            PaymentDueDate
                        </td>
                        <td style="width: 170px; height: 4px">
                            <asp:Label ID="lblNew_PaymentDueDate" runat="server" Width="122px" CssClass="NormalBody"
                                Height="16px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="NormalBody" style="width: 156px; height: 4px">
                        </td>
                        <td style="width: 170px; height: 4px">
                        </td>
                        <td class="NormalBody" style="width: 175px; height: 4px" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 27px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 27px" valign="top">
                            Supplier&nbsp;Address
                        </td>
                        <td style="height: 27px" colspan="3">
                            <asp:Label ID="lblSupplierAddress" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td style="width: 156px; height: 4px">
                        </td>
                        <td style="height: 4px" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 27px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 27px" valign="top">
                            Delivery Address
                        </td>
                        <td style="height: 27px" colspan="3">
                            <asp:Label ID="lblDeliveryAddress" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td style="width: 156px; height: 4px">
                        </td>
                        <td style="height: 4px" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 27px">
                        </td>
                        <td class="GridCaption" style="width: 156px; height: 27px" valign="top">
                            Invoice Address
                        </td>
                        <td style="height: 27px" colspan="3">
                            <asp:Label ID="lblInvoiceAddress" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td style="width: 156px; height: 4px">
                        </td>
                        <td style="height: 4px" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 12px">
                        </td>
                        <td class="GridCaption" style="height: 12px" colspan="4">
                            Invoice Details
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="padding-top: 16px" colspan="4">
                            <asp:DataGrid ID="grdInvoiceDetails" runat="server" Width="100%" Font-Size="10pt"
                                Font-Names="verdana,Tahoma,Arial" AutoGenerateColumns="False" GridLines="Vertical"
                                CellPadding="3" BorderWidth="1px" BorderStyle="None" BackColor="White" BorderColor="#999999">
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
                            <table width="100%">
                                <tr>
                                    <td width="43%">
                                        <table cellspacing="0" cellpadding="2" width="50%" border="0">
                                            <tr>
                                                <td width="39%">
                                                    <span id="sp_Back"><a onclick="javascript:history.back();" href="#">
                                                        <img src="../../images/back_2.gif" border="0"></a></span>
                                                </td>
                                                <td width="10">
                                                </td>
                                                <td width="169">
                                                    <asp:Button ID="btnGeneratePDF" TabIndex="13" runat="server" CssClass="GeneratePDF_2"
                                                        Width="139px" Height="23px" BorderWidth="0px" BorderStyle="None"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" align="right" width="57%">
                                        <table width="300">
                                            <tr>
                                                <td class="GridCaption" style="width: 170px; height: 16px" align="right">
                                                    Net Total
                                                </td>
                                                <td style="height: 16px" align="right">
                                                    <asp:Label ID="lblNett_Amount" runat="server" CssClass="NormalBody"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridCaption" style="width: 170px; height: 14px" align="right">
                                                    VAT
                                                    <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="../../images/i.jpg" NavigateUrl="javascript:ShowMessage()"></asp:HyperLink>
                                                </td>
                                                <td style="height: 14px" align="right">
                                                    <asp:Label ID="lblTax_Amoun" runat="server" CssClass="NormalBody" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridCaption" style="width: 170px" align="right">
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
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>
    </form>
    </TR></TBODY></TABLE>
</body>
</html>
