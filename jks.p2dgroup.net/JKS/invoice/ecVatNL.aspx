<%@ Page Language="c#" CodeBehind="ecVatNL.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.Invoice.ecVatNL" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ecVatNL</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:Panel ID="Panel4" Style="z-index: 102; left: 0px" runat="server" Width="100%">
    </asp:Panel>
    <table width="100%">
        <tr>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td class="PageHeader" colspan="4">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader">EC Vat Details</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="25%">
                            Seller VAT Registration No.
                        </td>
                        <td class="NormalBody" width="25%">
                            <%=strSellerVatRegNo%>
                        </td>
                        <td class="NormalBody" width="25%">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="25%">
                            Traders Reference
                        </td>
                        <td class="NormalBody" width="25%">
                            <%=strTradersReference%>
                        </td>
                        <td class="NormalBody" width="25%">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="25%">
                            Country Tax No.&nbsp;
                        </td>
                        <td class="NormalBody" width="25%">
                            <%=strCountryTaxNo%>
                        </td>
                        <td class="NormalBody" width="25%">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="25%">
                            Invoice No.&nbsp;
                        </td>
                        <td class="NormalBody" width="25%">
                            <%=strInvoiceNo%>
                        </td>
                        <td class="NormalBody" width="25%">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="25%">
                            Invoice Date
                        </td>
                        <td class="NormalBody" width="25%">
                            <%=strInvoiceDate%>
                        </td>
                        <td class="NormalBody" width="25%">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="25%">
                            Currency
                        </td>
                        <td class="NormalBody" width="25%">
                            <%=strCurrency%>
                        </td>
                        <td class="NormalBody" width="25%">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="GridCaption" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:DataGrid ID="grdInvoiceDetails" runat="server" Width="960px" Font-Size="10pt"
                                Font-Names="verdana,Tahoma,Arial" AutoGenerateColumns="False" GridLines="Vertical"
                                CellPadding="3" BorderWidth="1px" BorderStyle="None" BackColor="White" BorderColor="#999999"
                                Height="88px" ShowFooter="True">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <FooterStyle Font-Size="Smaller" ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="SerialNo" ReadOnly="True" HeaderText="Line No." FooterText="Total:">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Net">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"New_NettValue","{0:N2}")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNetAmount_Sum" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Vat">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVatAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VatAmt","{0:N2}")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblVatAmount_Sum" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Gross">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalAmt","{0:N2}")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrossAmount_Sum" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="New_NettValue" HeaderText="Net" DataFormatString="{0:N2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="VatAmt" ReadOnly="True" HeaderText="Vat"
                                        DataFormatString="{0:N2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="TotalAmt" HeaderText="Gross" DataFormatString="{0:N2}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_ModeOfTransport" HeaderText="Mode of Transport">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_NatureOfTransaction" ReadOnly="True" HeaderText="Nature of Transaction">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_TermsOfDelivery" ReadOnly="True" HeaderText="Terms of Delivery">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_CountryOfOrigin" HeaderText="Country of Origin">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_CommodityCode" ReadOnly="True" HeaderText="Commodity Code">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_SupplementaryUnits" ReadOnly="True" HeaderText="Supplementary Units">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_NettMass" ReadOnly="True" HeaderText="Net Mass">
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                </PagerStyle>
                            </asp:DataGrid>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody">
                            Vat in GBP =&nbsp;<%=strVatInGBP%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
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
