<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GrdRcdDetail.aspx.cs" Inherits="JKS_StockQC_GrdRcdDetail_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ActionInvoice Detail</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function HistoryBack() {
            alert('a');
            history.back();
        }	
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 900px; height: 436px" width="888">
        <tr>
            <td valign="top" align="center" width="100%">
                <!-- Main Content Panel Starts-->
                <table id="Table1" style="width: 900px; height: 412px" cellspacing="1" cellpadding="1"
                    border="0">
                    <tr>
                        <td class="PageHeader" style="height: 18px" align="center" colspan="2">
                            <asp:Label ID="lblConfirmation" runat="server" Visible="True" CssClass="PageHeader"> Invoice / PO Rec</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 68px" valign="top">
                            <table class="tlbborder" style="width: 900px; height: 70px" border="0">
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 6px">
                                    </td>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b>PO No</b>
                                    </td>
                                    <td style="width: 131px">
                                        <asp:Label ID="lblDocumentNo" runat="server" CssClass="NormalBody" Width="184px"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b>Total Invoiced</b>
                                    </td>
                                    <td style="width: 116px" class="NormalBody">
                                        <asp:Label ID="lblTotalRecvd" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 6px">
                                    </td>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b>Currency</b>
                                    </td>
                                    <td style="width: 131px">
                                        <asp:Label ID="lblCurrency" runat="server" CssClass="NormalBody" Width="144px"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b>Total Expected</b>
                                    </td>
                                    <td style="width: 116px" class="NormalBody">
                                        <asp:Label ID="lblInvoiced" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" style="padding-left: 15px; width: 6px">
                                    </td>
                                    <td class="NormalBody" style="padding-left: 15px; width: 106px">
                                        <b><font size="2"></font></b>
                                    </td>
                                    <td style="width: 131px">
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b><font size="2"><b><b><b><b>Variance</b></b></b></b></font></b>
                                    </td>
                                    <td style="width: 116px" class="NormalBody">
                                        <asp:Label ID="lblVarience" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <font size="2">
                                <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody"></asp:Label></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" align="center">
                            <div id="total">
                                <table id="Table2" style="width: 900px; height: 285px">
                                    <tr>
                                        <td class="NormalBody" style="width: 14px; height: 11px">
                                        </td>
                                        <td class="NormalBody" style="width: 207px; height: 11px">
                                        </td>
                                        <td style="width: 19px; height: 11px">
                                        </td>
                                        <td style="height: 11px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" style="width: 14px; height: 16px">
                                            <asp:Label ID="lll" runat="server" CssClass="NormalBody" Width="8px"></asp:Label>
                                        </td>
                                        <td class="NormalBody" style="width: 207px; height: 16px">
                                            <b>Goods Ordered</b>
                                        </td>
                                        <td style="width: 19px; height: 16px">
                                        </td>
                                        <td class="NormalBody" style="width: 207px; height: 11px">
                                            <b><b>Invoices/Credits</b></b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" style="width: 14px; height: 187px">
                                        </td>
                                        <td class="NormalBody" style="width: 207px; height: 178px" valign="top">
                                            <asp:DataGrid ID="grdGoodsRecd" runat="server" Width="140px" BorderStyle="Solid"
                                                BorderColor="#999999" ShowFooter="True" BackColor="White" BorderWidth="1px" CellPadding="3"
                                                GridLines="Vertical" AutoGenerateColumns="False" Font-Names="verdana,Tahoma,Arial"
                                                Font-Size="11px" Height="152px">
                                                <FooterStyle ForeColor="Black" BackColor="White"></FooterStyle>
                                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                                </ItemStyle>
                                                <HeaderStyle BorderColor="#848484" BorderStyle="Solid" BorderWidth="1px"
                                                 Font-Names="verdana" Font-Size="8.5pt" Font-Bold="True" ForeColor="White" BackColor="#3399CC" Height="35px">
                                                </HeaderStyle>
                                                <Columns>
                                                    <%--Added by Mainak 2018-05-25--%>
                                                    <asp:TemplateColumn HeaderText="PO Line">
                                                        <HeaderStyle Width="10px"></HeaderStyle>
                                                        <ItemStyle Width="10px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPOLine" Width="60px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderLineNo") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--Ended by Mainak 2018-05-25--%>
                                                    <asp:TemplateColumn HeaderText="Description">
                                                        <HeaderStyle Width="100px"></HeaderStyle>
                                                        <ItemStyle Width="100px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRCVDes" Width="160px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--Start: Added by Koushik Das as on 20-Dec-2017--%>
                                                    <asp:TemplateColumn HeaderText="Product Code">
                                                        <HeaderStyle Width="100px"></HeaderStyle>
                                                        <ItemStyle Width="100px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProdCode" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BuyersProdCode") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--End: Added by Koushik Das as on 20-Dec-2017--%>
                                                    <asp:TemplateColumn HeaderText="Date">
                                                        <ItemStyle Width="10px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRCVDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ModDate") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Qty">
                                                        <ItemStyle Width="10px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRCVQty" runat="server" Style="text-align: right" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Price">
                                                        <ItemStyle Width="10px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRCVPrice" runat="server" Style="text-align: right" Text='<%# DataBinder.Eval(Container, "DataItem.Rate") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Net">
                                                        <ItemStyle Width="30px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRCVNet" runat="server" Style="text-align: right" Text='<%# DataBinder.Eval(Container, "DataItem.Net") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <table align="right">
                                                                <tr>
                                                                    <td style="font-family: verdana, Tahoma, Arial; font-size: 10px; color: #000000"> 
                                                                        <hr>
                                                                        <asp:Label ID="lblNetTot" runat="server" Text='' Style="text-align: right"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--Start: Added by Koushik Das as on 21-Dec-2017--%>
                                                    <asp:TemplateColumn HeaderText="VAT">
                                                        <HeaderStyle Width="30px"></HeaderStyle>
                                                        <ItemStyle Width="30px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVAT" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VAT") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <table align="right">
                                                                <tr>
                                                                    <td style="font-family: verdana, Tahoma, Arial; font-size: 10px; color: #000000">
                                                                        <hr>
                                                                        <asp:Label ID="lblVatTot" runat="server" Text='' Style="text-align: right"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--End: Added by Koushik Das as on 21-Dec-2017--%>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                        <td style="width: 5px; height: 178px">
                                        </td>
                                        <td style="height: 187px" valign="top">
                                            <asp:DataGrid ID="dgInvoiced" runat="server" Width="150px" BorderStyle="Solid" BorderColor="#999999"
                                                ShowFooter="True" BackColor="White" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                AutoGenerateColumns="False" Font-Names="verdana,Tahoma,Arial" Font-Size="11px"
                                                Height="152px">
                                                <FooterStyle ForeColor="Black" BackColor="White"></FooterStyle>
                                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                                </ItemStyle>
                                                <HeaderStyle BorderColor="#848484" BorderStyle="Solid" BorderWidth="1px"
                                                    Font-Names="verdana" Font-Size="8.5pt" Font-Bold="True" Height="35px" ForeColor="White" BackColor="#3399CC" >
                                                </HeaderStyle>
                                                <Columns>
                                                    <%--Added by Mainak 2018-05-25--%>
                                                    <asp:TemplateColumn HeaderText="PO Line">
                                                        <HeaderStyle Width="10px"></HeaderStyle>
                                                        <ItemStyle Width="10px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPOL" Width="60px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderLineNo") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--Ended by Mainak 2018-05-25--%>
                                                    <asp:TemplateColumn HeaderText="Doc No">
                                                        <ItemStyle Width="10px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <a href='<%# DataBinder.Eval(Container,"DataItem.FTPUrl") %>'>
                                                                <asp:Label ID="Label2" Text='<%# DataBinder.Eval(Container, "DataItem.invoiceNo") %>'
                                                                    runat="server">                                                              
                                                                </asp:Label>
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Date">
                                                        <ItemStyle Width="10px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrderDate") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Doc Type">
                                                        <ItemStyle Width="10px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DocType") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Description">
                                                        <ItemStyle Width="100px"></ItemStyle>
                                                        <HeaderStyle Width="100px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" Width="160px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--Start: Added by Koushik Das as on 20-Dec-2017--%>
                                                    <asp:TemplateColumn HeaderText="Product Code">
                                                        <HeaderStyle Width="100px"></HeaderStyle>
                                                        <ItemStyle Width="100px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProdCode" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BuyersProdCode") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--End: Added by Koushik Das as on 20-Dec-2017--%>
                                                    <asp:TemplateColumn HeaderText="Qty">
                                                        <ItemStyle Width="10px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Style="text-align: right" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Price">
                                                        <HeaderStyle Width="30px"></HeaderStyle>
                                                        <ItemStyle Width="10px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <table align="right">
                                                                <tr>
                                                                    <td style="font-family: verdana, Tahoma, Arial; font-size: 10px; color: #000000">
                                                                        <asp:Label ID="Label4" Width="40px" runat="server" Style="text-align: right" Text='<%# DataBinder.Eval(Container, "DataItem.Rate") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Net">
                                                        <HeaderStyle Width="30px"></HeaderStyle>
                                                        <ItemStyle Width="30px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="font-family: verdana, Tahoma, Arial; font-size: 10px; color: #000000">
                                                                        <asp:Label ID="lblInvCredit" Width="40px" runat="server" Style="text-align: right"
                                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Net") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <table align="right">
                                                                <tr>
                                                                    <td align="center" style="font-family: verdana, Tahoma, Arial; font-size: 10px; color: #000000">
                                                                        <hr>
                                                                        <asp:Label ID="lblInvCrd" Width="40px" runat="server" Text=''></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--Start: Added by Koushik Das as on 21-Dec-2017--%>
                                                    <asp:TemplateColumn HeaderText="VAT">
                                                        <HeaderStyle Width="30px"></HeaderStyle>
                                                        <ItemStyle Width="30px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVAT" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VATAmt") %>'><%-- Modified by Mainak 2018-07-12--%>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <table align="right">
                                                                <tr>
                                                                    <td style="font-family: verdana, Tahoma, Arial; font-size: 10px; color: #000000">
                                                                        <hr>
                                                                        <asp:Label ID="lblVatTot" runat="server" Text='' Style="text-align: right"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <%--End: Added by Koushik Das as on 21-Dec-2017--%>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" style="width: 14px">
                                        </td>
                                        <td class="NormalBody" style="width: 207px">
                                        </td>
                                        <td style="width: 19px">
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="NormalBody" style="width: 14px">
                                        </td>
                                        <td class="NormalBody" style="width: 207px">
                                        </td>
                                        <td style="width: 19px">
                                        </td>
                                        <td align="left">
                                            <input id="btnb" style="width: 100px; hight: 20px" onclick="javascript:window.close();"
                                                type="button" value="Back" name="btnb" class="allbtn_ActionWindow">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="comments">
                            </div>
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
