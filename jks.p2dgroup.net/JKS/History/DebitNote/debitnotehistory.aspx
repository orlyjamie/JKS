<%@ Import Namespace="System.Data" %>

<%@ Page Language="c#" CodeBehind="debitnotehistory.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.DebitNote.debitnotehistory" %>

<%@ Register TagPrefix="uc1" TagName="menuUser" Src="~/JKS/Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="~/Utilities/banneretc.ascx" %>
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
                <table width="100%">
                    <tr>
                        <td class="NormalBody" width="213">
                        </td>
                        <td class="NormalBody">
                        </td>
                        <td class="NormalBody">
                        </td>
                    </tr>
                    <!--	<TR>
								<td class="NormalBody" width="213">Buyer Company Name</td>
								<td class="NormalBody"><asp:dropdownlist id="ddlBuyerCompany" runat="server" CssClass="NormalBody" Width="248px" DataValueField="BuyerCompanyID"
										DataTextField="CompanyName"></asp:dropdownlist></td>
								<td class="NormalBody"></td>
							</TR>  -->
                    <tr>
                        <td class="NormalBody" width="213">
                            Invoice Status
                        </td>
                        <td class="NormalBody">
                            <asp:DropDownList ID="ddlDocStatus" runat="server" Width="248px" CssClass="NormalBody"
                                DataTextField="Status" DataValueField="StatusID">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="213">
                            DebitNote&nbsp;Number
                        </td>
                        <!--<td class="NormalBody"><asp:dropdownlist id="ddlInvoiceNo" runat="server" Width="248px" CssClass="NormalBody" DataTextField="InvoiceNo"
										DataValueField="InvoiceNo"></asp:dropdownlist></td>-->
                        <td class="NormalBody">
                            <asp:TextBox ID="txtDebitNoteNo" runat="server" Width="248px" CssClass="NormalBody"></asp:TextBox>
                        </td>
                        <td class="NormalBody">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="213">
                        </td>
                        <td class="NormalBody">
                        </td>
                        <td class="NormalBody">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="213">
                        </td>
                        <td class="NormalBody">
                            <asp:Button ID="btnSearch" runat="server" Text="Search"></asp:Button>
                        </td>
                        <td class="NormalBody">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="213">
                        </td>
                        <td class="NormalBody">
                        </td>
                        <td class="NormalBody">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" width="213">
                        </td>
                        <td class="NormalBody">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td class="NormalBody">
                        </td>
                    </tr>
                </table>
                <table id="Table1" width="100%">
                    <tr>
                        <td class="PageHeader" width="100%">
                            DebitNote&nbsp;Log
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="grdInvCur" runat="server" Width="100%" OnSortCommand="Sort_GridCur"
                                OnItemDataBound="Grid_ItemDataBound" OnPageIndexChanged="grdInvCur_PageIndexChanged"
                                AllowSorting="True" BorderColor="#999999" Height="88px" BackColor="White" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False"
                                Font-Names="Verdana,Tahoma,Arial" Font-Size="10pt" AllowPaging="True" PageSize="15">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Select">
                                        <ItemTemplate>
                                            <a href='InvoiceConfirmation_DN.aspx?DebitNoteID=<%#DataBinder.Eval(Container.DataItem,"DebitNoteID")%>'>
                                                <img src="../images/GridSelect.jpg" border="0"></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Document_No" HeaderText="DebitNote No." DataFormatString="{0:N}">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Buyer" HeaderText="Buyer"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Document_Date" HeaderText="DebitNote Date" DataFormatString="{0:dd-MM-yyyy}">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Currency" HeaderText="Currency"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Nett_amount" HeaderText="Net">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="tax_amount" HeaderText="VAT">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Gross_amount" HeaderText="Total">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="new_PaymentDueDate" HeaderText="Payment Due Date" DataFormatString="{0:dd-MM-yyyy}">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Status" HeaderText="Doc Status"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="new_PaymentDate" ItemStyle-Wrap="False" HeaderText="Payment Date"
                                        DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="new_DiscountGiven" HeaderText="Discount Given">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="new_PaymentMethod" HeaderText="Payment Method" ItemStyle-Wrap="False">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Upload/Download Document">
                                        <ItemTemplate>
                                            <table cellpadding="2" width="100%" border="0">
                                                <tr>
                                                    <td width="80%" class="NormalBodylink" align="center">
                                                        <asp:HyperLink ID="hpDoc" runat="server">Document</asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                                    Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
