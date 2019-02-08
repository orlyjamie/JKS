<%@ Page Language="c#" CodeBehind="InvoiceConfirmation_PO.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.SalesOrders.InvoiceConfirmation_PO" %>

<%@ Register TagPrefix="uc1" TagName="menuUser" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Create Sales Orders (Confirmation)</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function ShowHideGBPEquiValentAmountRow() {
            if (document.all.hdGBPEquiFlag.value == '1')
                document.getElementById("trGBPEquiAmt").style.display = "";
            else
                document.getElementById("trGBPEquiAmt").style.display = "none";
        }
        function OpenURL(iInvoiceID) {
            var strURL = null;
            strURL = "ecvat.aspx?InvoiceID=" + iInvoiceID
            window.open(strURL);
        }
        function ShowMessage() {
            alert('This VAT amount has been calculated automatically by the system using arithmetic rounding. However, if you use a different rounding method please change the VAT amount manually. ');
        }
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0" onload="javascript:ShowHideGBPEquiValentAmountRow();">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel4" Style="z-index: 102; left: 0px" runat="server" CssClass="Banner"
        Width="100%">
        <uc1:banner ID="Banner2" runat="server"></uc1:banner>
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
                        <td style="width: 1px; height: 16px">
                        </td>
                        <td class="PageHeader" style="height: 16px" colspan="4">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader">Step 4 of 4 : Confirm Invoice Transmission</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td style="height: 16px" colspan="4">
                            <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody">Order No. : </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Supplier&nbsp;
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblSupplier" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Customer
                        </td>
                        <td style="height: 16px">
                            <asp:Label ID="lblBuyer" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Supplier Account No.
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblSupplierAccNo" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Customer Account No.
                        </td>
                        <td style="height: 16px">
                            <asp:Label ID="lblCustomerAccNo" runat="server" CssClass="NormalBody" Width="128px"
                                Height="16px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Supplier Contact Name
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblSuppContName" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Customer Contact Name
                        </td>
                        <td style="height: 16px">
                            <asp:Label ID="lblCustomerContactName" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            VAT Reg No.
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblVATRegNo" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Settlement&nbsp;Discount %
                        </td>
                        <td style="height: 16px">
                            <asp:Label ID="lblTermsDiscount" runat="server" CssClass="NormalBody" Width="128px"
                                Height="8px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Order Date
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblInvoiceDate" runat="server" CssClass="NormalBody" Height="15px"></asp:Label>
                        </td>
                        <td style="width: 177px; height: 16px">
                        </td>
                        <td style="height: 16px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Currency
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblCurrency" runat="server" CssClass="NormalBody" Width="188px"></asp:Label>
                        </td>
                        <td style="width: 177px; height: 16px">
                        </td>
                        <td style="height: 16px">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                        </td>
                        <td style="width: 155px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Settlement Discount Days
                        </td>
                        <td style="height: 16px" valign="middle">
                            <asp:Label ID="lblSettlementDays" runat="server" CssClass="NormalBody" Width="128px"
                                Height="16px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Payment Terms
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblPaymentTerms" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            2nd Settlement&nbsp;Discount %
                        </td>
                        <td style="height: 16px" valign="middle">
                            <asp:Label ID="lblSecondSettlementDiscount" runat="server" CssClass="NormalBody"
                                Width="128px" Height="16px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Payment Due Date
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblPaymentDueDAte" runat="server" CssClass="NormalBody" Width="122px"
                                Height="16px"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            2nd Settlement Discount Days
                        </td>
                        <td style="height: 17px" valign="middle">
                            <asp:Label ID="lblSecondSettlementDays" runat="server" CssClass="NormalBody" Width="128px"
                                Height="16px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Tax Point Date
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblTaxPointDate" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                        </td>
                        <td style="height: 13px">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                            Overall Discount %
                        </td>
                        <td style="width: 155px; height: 16px">
                            <asp:Label ID="lblOverAllDiscount" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                        </td>
                        <td style="height: 16px">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 16px">
                        </td>
                        <td style="width: 155px; height: 16px">
                        </td>
                        <td class="GridCaption" style="width: 177px; height: 16px">
                            Status
                        </td>
                        <td style="height: 16px">
                            <asp:Label ID="lblStatus" runat="server" CssClass="NormalBody" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 4px">
                            Payment Date
                        </td>
                        <td style="width: 155px; height: 4px">
                            <asp:Label ID="lblpaymentdate" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 4px">
                            Payment Method
                        </td>
                        <td style="width: 155px; height: 4px">
                            <asp:Label ID="lblPaymentMethod" CssClass="NormalBody" Width="187px" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 4px">
                            Discount Given
                        </td>
                        <td style="width: 155px; height: 4px">
                            <asp:Label ID="lblDiscount" CssClass="NormalBody" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 15px">
                        </td>
                        <td class="NormalBody" style="width: 142px; height: 15px">
                            <img height="4" src="../images/blank.gif" width="130"><span style="display: none">Delivery
                                Note No.</span>
                        </td>
                        <td style="width: 134px; height: 15px">
                            <asp:Label ID="lblDespatchNoteNo" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="NormalBody" style="width: 175px; height: 15px">
                            <span style="display: none">Delivery Date</span>
                        </td>
                        <td style="height: 15px">
                            <asp:Label ID="lblDespatchDate" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 27px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 27px" valign="top">
                            Supplier&nbsp;Address
                        </td>
                        <td style="height: 27px" colspan="3">
                            <asp:Label ID="lblSupplierAddress" runat="server" CssClass="NormalBody" Width="587px"
                                Height="26px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 7px">
                        </td>
                        <td style="width: 142px; height: 7px">
                        </td>
                        <td style="height: 7px" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 27px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 27px" valign="top">
                            Delivery Address
                        </td>
                        <td style="height: 27px" colspan="3">
                            <asp:Label ID="lblDeliveryAddress" runat="server" CssClass="NormalBody" Width="587px"
                                Height="26px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 9px">
                        </td>
                        <td style="width: 142px; height: 9px">
                        </td>
                        <td style="height: 9px" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 30px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 30px" valign="top">
                            Buyer Address
                        </td>
                        <td style="height: 30px" colspan="3">
                            <asp:Label ID="lblInvoiceAddress" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 27px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 27px" valign="top">
                            Customer Activity Code
                        </td>
                        <td style="height: 27px" colspan="3">
                            <asp:Label ID="lblActivityCode" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 4px" valign="top">
                            <font class="GridCaption" size="2">Customer Account Category</font>
                        </td>
                        <td style="height: 4px" colspan="3">
                            <asp:Label ID="lblAccountCat" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 4px" valign="top">
                            Notes
                        </td>
                        <td style="height: 4px" colspan="3">
                            <asp:Label ID="lblNotes" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 4px" valign="top">
                            Terms
                        </td>
                        <td style="height: 4px" colspan="3">
                            <asp:Label ID="lblTerms" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 4px">
                        </td>
                        <td style="width: 142px; height: 4px">
                        </td>
                        <td style="height: 4px" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 12px">
                        </td>
                        <td class="GridCaption" style="font-weight: bold; width: 142px; height: 12px" valign="top">
                            Order Name
                        </td>
                        <td style="font-weight: bold; height: 12px" colspan="3">
                            <asp:Label ID="lblInvoiceName" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <% if (strInvoiceDocument != "")
                       { %>
                    <tr>
                        <td style="width: 16px; height: 12px">
                        </td>
                        <td class="GridCaption" style="width: 142px; height: 12px" valign="top">
                            Invoice Document
                        </td>
                        <td style="height: 12px" colspan="3">
                            <a class="NormalBody" href="<%=strInvoiceDocumentDownloadPath%><%=strInvoiceDocument%>"
                                target="_blank">Download Scanned Image </a>
                        </td>
                    </tr>
                    <% } %>
                    <tr style="display: none">
                        <td style="width: 16px; height: 10px">
                        </td>
                        <td class="GridCaption" style="height: 12px" colspan="4">
                            <a onclick="javascript:OpenURL('<%=invoiceID%>')" href="#">EC VAT Details</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16px; height: 2px">
                        </td>
                        <td class="GridCaption" style="height: 2px" colspan="4">
                            Order Details
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="padding-top: 16px" colspan="4">
                            <asp:DataGrid ID="grdInvoiceDetails" runat="server" Width="100%" PageSize="100" Font-Size="10pt"
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
                                    <asp:BoundColumn DataField="PurOrderNo" ReadOnly="True" HeaderText="PO No." Visible="False">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="PurOrderDate" HeaderText="PO Date" DataFormatString="{0:dd-MM-yyyy}"
                                        Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="PurOrderLineNo" ReadOnly="True" HeaderText="PO Line No.">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_DespatchDate" HeaderText="Despatch Date" DataFormatString="{0:dd-MM-yyyy}"
                                        Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_DespatchNoteNumber" HeaderText="Despatch Note No"
                                        Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="BuyersProdCode" HeaderText="Buyers Prod Code" Visible="False">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SuppliersProdCode" HeaderText="Suppliers Prod Code" Visible="False">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Description" ReadOnly="True" HeaderText="Description">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="New_Type" ReadOnly="True" HeaderText="Item Type" Visible="False">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Color" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcolor" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Rate" HeaderText="Price" DataFormatString="{0:N2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Quantity" ReadOnly="True" HeaderText="Quantity" DataFormatString="{0:N2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="UOM" ReadOnly="True" HeaderText="UOM" Visible="False">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Discount" ReadOnly="True" HeaderText="Disc %" DataFormatString="{0:N2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_DiscountPercent2" ReadOnly="True" HeaderText="2nd Disc%"
                                        DataFormatString="{0:N2}" Visible="False">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_NettValue" ReadOnly="True" HeaderText="Net Value"
                                        DataFormatString="{0:N2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="VATRate" ReadOnly="True" HeaderText="VAT Rate" DataFormatString="{0:N2}"
                                        Visible="False">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="VATAmt" HeaderText="VAT" DataFormatString="{0:N2}" Visible="False">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="TotalAmt" ReadOnly="True" HeaderText="Gross Value" DataFormatString="{0:N2}"
                                        Visible="False">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                </PagerStyle>
                            </asp:DataGrid><input id="hdGBPEquiFlag" type="hidden" value="0" name="hdGBPEquiFlag"
                                runat="server">&nbsp;<input id="hdSaveFlag" type="hidden" value="0" name="hdSaveFlag"
                                    runat="server">&nbsp;<input id="hdHideBack" type="hidden" value="0" name="hdHideBack"
                                        runat="server">
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
                                                        <img src="../images/back_2.gif" border="0"></a></span>
                                                </td>
                                                <td width="10">
                                                    <asp:Button ID="btnConfirmInvoice" TabIndex="13" runat="server" Width="139px" CssClass="ConfirmInvoice_2"
                                                        Height="23px" BorderStyle="None" BorderWidth="0px"></asp:Button>
                                                </td>
                                                <td width="169">
                                                    <asp:Button ID="btnGeneratePDF" TabIndex="13" runat="server" Width="139px" CssClass="GeneratePDF_2"
                                                        Height="23px" BorderStyle="None" BorderWidth="0px"></asp:Button>
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
                                                    <asp:Label ID="lblNetTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridCaption" style="width: 170px; height: 15px" align="right">
                                                    VAT
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:ShowMessage()"
                                                        ImageUrl="../images/i.jpg" Visible="False"></asp:HyperLink>
                                                </td>
                                                <td style="height: 15px" align="right">
                                                    <asp:Label ID="lblVAT" runat="server" CssClass="NormalBody" Visible="False"></asp:Label><asp:TextBox
                                                        ID="txtVATAmt" runat="server" Width="67" CssClass="MYINPUTFORVAT" Height="19"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="GridCaption" style="width: 170px; height: 15px" align="right">
                                                    Total
                                                </td>
                                                <td style="height: 15px" align="right">
                                                    <asp:Label ID="lblTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trGBPEquiAmt" style="display: none">
                                                <td class="GridCaption" style="width: 170px" align="right">
                                                    Vat&nbsp;in GBP&nbsp;
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblGBPEquiAmt" runat="server" CssClass="NormalBody"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trInputSterlingEquiAmt" runat="server">
                                                <td class="GridCaption" style="width: 170px" align="right">
                                                    Sterling equivalent VAT <font color="red">*</font>
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtSterlingEquivalent" runat="server" Width="67" Height="19" Visible="False"></asp:TextBox>
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
    <script language="javascript">
        if (document.getElementById("hdHideBack").value == '1') {
            document.getElementById("sp_Back").style.display = "none";
        }
        if (document.getElementById("hdSaveFlag").value == '1') {
            alert('Invoice sent successfully.');
            window.location.href = 'InvoiceConfirmation.aspx?hd=1';
        } 
    </script>
    </form>
</body>
</html>
