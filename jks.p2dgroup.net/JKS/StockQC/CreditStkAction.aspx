<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditStkAction.aspx.cs"
    Inherits="JKS_StockQC_CreditStkAction_n" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Credit Action</title>
    <%--   <LINK href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">--%>
    <link href="../../Utilities/ETH.css" rel="stylesheet" type="text/css" />
    <script language="javascript">

        function windowclose() {
            window.close();
        }	
			
    </script>
    <style type="text/css">
        .tlbborder
        {
            border-top: 1px solid #468CB7;
            border-right: 1px solid #468CB7;
            border-bottom: 1px solid #468CB7;
            border-left: 1px solid #468CB7;
        }
    </style>
</head>
<body ms_positioning="GridLayout">
    <form id="form1" runat="server" method="post">
    <table style="width: 692px; height: 678px" width="692">
        <tr>
            <td valign="top" align="center" width="100%">
                <!-- Main Content Panel Starts-->
                <table id="Table1" style="width: 508px; height: 448px" cellspacing="1" cellpadding="1"
                    width="508" border="0">
                    <tr>
                        <td class="PageHeader" style="height: 18px" align="center" colspan="2">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader" Visible="True">Credit Note Workflow</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 140px" valign="top" align="center">
                            <table class="tlbborder" style="width: 696px; height: 120px" border="0">
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b>Document No</b>
                                    </td>
                                    <td style="width: 271px">
                                        <asp:Label ID="lblDocumentNo" runat="server" CssClass="NormalBody" Width="224px"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 178px">
                                        <b>Current Status</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblinvoicestatus" CssClass="NormalBody" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b>Document Date</b>
                                    </td>
                                    <td style="width: 271px">
                                        <asp:Label ID="lblinvoicedate" CssClass="NormalBody" runat="server"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 178px">
                                        <b>Invoice No</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCreditNo" CssClass="NormalBody" runat="server" Width="120px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b>Supplier Name</b>
                                    </td>
                                    <td style="width: 271px">
                                        <asp:Label ID="lblsupplier" CssClass="NormalBody" runat="server"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 178px">
                                        <b>Department</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDepartmant" runat="server" CssClass="NormalBody" Width="128px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b>Buyer Name</b>
                                    </td>
                                    <td style="width: 271px">
                                        <asp:Label ID="lblBuyerName" runat="server" CssClass="NormalBody" Width="224px"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 178px">
                                        <b>Nominal Code</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNominal" runat="server" CssClass="NormalBody" Width="128px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b>Currency</b>
                                    </td>
                                    <td style="width: 271px">
                                        <asp:Label ID="lblCurrency" runat="server" CssClass="NormalBody" Width="144px"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 178px">
                                        <b>Document Type</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblinvoicetype" CssClass="NormalBody" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b><font size="2"><asp:label id="lblstatus" CssClass="NormalBody" Runat="server"></asp:label></font>
                                        </b>
                                    </td>
                                    <td style="width: 271px">
                                        <asp:Label ID="lblvatcode" CssClass="NormalBody" runat="server"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 178px">
                                        <b><font size="2"><asp:label id="lbldate" CssClass="NormalBody" Runat="server"></asp:label></font>
                                        </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAPCompanyID" CssClass="NormalBody" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <font size="2">
										<asp:label id="lblType" CssClass="NormalBody" Runat="server"></asp:label><asp:label id="lblRefernce" runat="server" CssClass="NormalBody"></asp:label></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 65px" align="center">
                            <div id="values">
                                <table class="tlbborder" style="width: 456px; height: 40px">
                                    <tr>
                                        <td style="width: 21px">
                                        </td>
                                        <td class="NormalBody" style="width: 106px">
                                            <b>This Credit Note</b>
                                        </td>
                                        <td style="width: 115px" align="right">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 21px">
                                        </td>
                                        <td class="NormalBody" style="width: 106px">
                                        </td>
                                        <td class="NormalBody" style="width: 115px" align="right">
                                            <b>Net Total</b>
                                        </td>
                                        <td class="NormalBody" style="width: 106px" align="right">
                                            <b>Vat Total</b>
                                        </td>
                                        <td class="NormalBody" style="width: 106px" align="right">
                                            <b>Gross</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 21px">
                                        </td>
                                        <td class="NormalBody" style="width: 106px">
                                        </td>
                                        <td class="NormalBody" style="width: 115px" align="right">
                                            <b>
                                                <asp:Label ID="lblNet" runat="server" CssClass="NormalBody" Width="87px"></asp:Label></b>
                                        </td>
                                        <td class="NormalBody" style="width: 106px" align="right">
                                            <b>
                                                <asp:Label ID="lblVat" runat="server" CssClass="NormalBody" Width="85px"></asp:Label></b>
                                        </td>
                                        <td class="NormalBody" style="width: 106px" align="right">
                                            <b>
                                                <asp:Label ID="lblGross" runat="server" CssClass="NormalBody" Width="84px"></asp:Label></b>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 430px" align="center">
                            <asp:DataGrid ID="grdInvCur" runat="server" Width="450px" BorderStyle="Solid" BorderColor="#999999"
                                ShowFooter="True" BackColor="White" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                AutoGenerateColumns="False" Font-Names="verdana,Tahoma,Arial" Font-Size="10pt"
                                Height="148px">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="White"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Purchase Order No">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetPOActionURL(DataBinder.Eval(Container.DataItem,"PurOrderNo"),DataBinder.Eval(Container.DataItem,"GoodsRecdID"))%>">
                                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'>
                                                </asp:Label>
                                            </a>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="NormalBody">&nbsp;</span>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Total Invoiced" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<table align="right">
														<tr>
															<td class="NormalBody" align="right">--%>
                                            <asp:Label ID="lblTotInvoiced" runat="server" CssClass="NormalBody" Style="text-align: right;
                                                width: 100%; display: inline-block;" Text='<%# DataBinder.Eval(Container, "DataItem.TotalInvoiced") %>'>
                                            </asp:Label>
                                            <%--</td>
														</tr>
													</table>--%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%--<table align="right">
														<tr>
															<td class="NormalBody">--%><hr style="width: 50%; float: right">
                                            <asp:Label ID="lblFooterTotInvoiced" runat="server" class="NormalBody" Style="text-align: right;
                                                width: 100%; display: inline-block;" Text=''></asp:Label>
                                            <%--</td>
														</tr>
													</table>--%>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Total Expected" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<table align="right">
														<tr>
															<td class="NormalBody">--%>
                                            <asp:Label ID="lblTotReceived" runat="server" class="NormalBody" Style="text-align: right;
                                                width: 100%; display: inline-block;" Text='<%# DataBinder.Eval(Container, "DataItem.TotalRecd") %>'>
                                            </asp:Label>
                                            <%--</td>
														</tr>
													</table>--%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%--<table align="right">
														<tr>
															<td class="NormalBody">--%><hr style="width: 50%; float: right">
                                            <asp:Label ID="lblFooterTotalReceived" runat="server" class="NormalBody" Style="text-align: right;
                                                width: 100%; display: inline-block;" Text=''></asp:Label>
                                            <%--</td>
														</tr>
													</table>--%>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Variance" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<table align="right">
														<tr>
															<td class="NormalBody">--%>
                                            <asp:Label ID="Label2" runat="server" class="NormalBody" Style="text-align: right;
                                                width: 100%; display: inline-block;" Text='<%# DataBinder.Eval(Container, "DataItem.Varience") %>'>
                                            </asp:Label>
                                            <%--</td>
														</tr>
													</table>--%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%--<table align="right">
														<tr>
															<td class="NormalBody">--%><hr style="width: 50%; float: right">
                                            <asp:Label ID="lblFooterVarience" runat="server" class="NormalBody" Style="text-align: right;
                                                width: 100%; display: inline-block;" Text=''></asp:Label>
                                            <%--</td>
														</tr>
													</table>--%>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Invalid PO" HeaderStyle-HorizontalAlign="Center">
                                        <FooterTemplate>
                                            <span class="NormalBody">&nbsp;</span></FooterTemplate>
                                        <ItemTemplate>
                                            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                                                BorderWidth="0" Font-Size="11px">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-CssClass="gridItemStyle" ItemStyle-Width="150px" DataField="PurchanseOrderNo"
                                                        ItemStyle-ForeColor="Red" NullDisplayText="--" ItemStyle-Font-Size="11px" ItemStyle-BorderWidth="0" />
                                                </Columns>
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <div id="total">
                                <table id="Table2" style="width: 456px; height: 10px">
                                    <tr>
                                        <td class="NormalBody" style="width: 106px">
                                            <b></b>
                                        </td>
                                        <td style="width: 115px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="comments">
                                <table id="Table3" style="width: 694px; height: 123px">
                                    <tr>
                                        <td class="NormalBody" style="width: 42px">
                                            <b></b>
                                        </td>
                                        <td style="width: 133px">
                                        </td>
                                        <td style="width: 426px">
                                            <asp:Label ID="lblMessege" runat="server" CssClass="NormalBody" BorderStyle="None"
                                                ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr <%if(Request.QueryString["SQ"]==null){%> style="display: none" <%}%>>
                                        <td class="NormalBody" style="width: 42px">
                                        </td>
                                        <td class="NormalBody" style="width: 133px">
                                            <b>Comments :</b>
                                        </td>
                                        <td class="NormalBody" style="width: 426px">
                                            <b>
                                                <asp:TextBox ID="tbComments" runat="server" CssClass="NormalBody" Width="424px" Height="36px"></asp:TextBox></b>
                                        </td>
                                        <td class="NormalBody" style="width: 106px">
                                            <b></b>
                                        </td>
                                        <td class="NormalBody" style="width: 106px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" style="width: 42px; height: 2px">
                                        </td>
                                        <td class="NormalBody" style="width: 133px; height: 2px">
                                        </td>
                                        <td class="NormalBody" style="width: 426px; height: 2px">
                                        </td>
                                        <td class="NormalBody" style="width: 106px; height: 2px">
                                        </td>
                                        <td class="NormalBody" style="width: 106px; height: 2px">
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="NormalBody" style="width: 42px; height: 3px">
                                        </td>
                                        <td class="NormalBody" style="width: 133px; height: 3px">
                                            <b>Rejection Code :</b>
                                        </td>
                                        <td class="NormalBody" style="width: 426px; height: 3px">
                                            <asp:DropDownList ID="ddlRejectionCode" runat="server" CssClass="NormalBody" Width="424px"
                                                Height="20px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="NormalBody" style="width: 106px; height: 3px">
                                        </td>
                                        <td class="NormalBody" style="width: 106px; height: 3px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" style="width: 42px">
                                        </td>
                                        <td class="NormalBody" style="width: 133px">
                                        </td>
                                        <td class="NormalBody" style="width: 426px">
                                        </td>
                                        <td class="NormalBody" style="width: 106px">
                                        </td>
                                        <td class="NormalBody" style="width: 106px">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="Coding">
                            </div>
                            <div id="buttons">
                                <table style="width: 694px; height: 78px">
                                    <tr>
                                        <td style="width: 300px">
                                        </td>
                                        <td style="width: 300px">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 273px">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 300px">
                                        </td>
                                        <td style="width: 300px">
                                        </td>
                                        <td style="width: 273px">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 300px" align="center">
                                            <asp:Button ID="btnCancel" runat="server" CssClass="allbtn_ActionWindow btnCenter"
                                                BorderWidth="0px" BorderStyle="None" CausesValidation="False" Text="Cancel">
                                            </asp:Button>
                                        </td>
                                        <td style="width: 300px; display: none">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnApprove" CssClass="ButtonCssNew" runat="server" BorderStyle="None"
                                                ToolTip="Approve" Text="Approve"></asp:Button>
                                        </td>
                                        <td style="width: 273px; display: none">
                                            <asp:Button ID="btnReject" CssClass="ButtonCssNew" runat="server" BorderStyle="None"
                                                ToolTip="Reject" Text="Reject"></asp:Button>
                                        </td>
                                        <td style="display: none">
                                            <asp:Button ID="btndelete" CssClass="ButtonCssNew" runat="server" BorderStyle="None"
                                                ToolTip="Delete" Text="Delete"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
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
