<%@ Page Language="c#" CodeBehind="CodingFlags.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.ApprovalPath.CodingFlags" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>CodingFlags</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="https://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table>
        <tr>
            <td colspan="4">
                <asp:DataGrid ID="grdList" runat="server" ShowFooter="True" AutoGenerateColumns="False"
                    GridLines="Vertical" CellPadding="2" CssClass="listingArea" Width="100%">
                    <FooterStyle ForeColor="Black" BackColor="White"></FooterStyle>
                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                    <ItemStyle></ItemStyle>
                    <HeaderStyle CssClass="tableHd"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn HeaderText="Line No" Visible="False">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Company Name">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlBuyerCompanyCode" CssClass="form-control inpit_select" AutoPostBack="True"
                                    OnSelectedIndexChanged="SelectedIndexChanged_ddlBuyerCompanyCode" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Department">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDepartment1" OnSelectedIndexChanged="SelectedIndexChanged_ddlDepartment"
                                    AutoPostBack="True" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Nominal Code">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged_ddlNominalCode"
                                    ID="ddlNominalCode1">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Code Description">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlCodingDescription1" OnSelectedIndexChanged="SelectedIndexChanged_ddlCodingDescription"
                                    AutoPostBack="True" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Project Code">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlProject1" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Net value for coding" Visible="False">
                            <ItemTemplate>
                                <asp:TextBox ID="txtNetVal" Text='<%# DataBinder.Eval(Container, "DataItem.NetValue") %>'
                                    runat="server">
                                </asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td nowrap>
                                            <asp:Label ID="lblNetVal" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>
                                            <br>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>
                                            <asp:Label ID="lblNetInvoiceTotal" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Delete Lines" Visible="False">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkBox" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="AP Admin Only">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlCapexReq" runat="server">
                                    <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                        Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <input class="allbtn" type="button" value="Cancel" onclick="window.close();">
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="allbtn"></asp:Button>
            </td>
            <td>
                <%--<asp:Button ID="btnDelete" runat="server" CssClass="allbtn" Text="Delete">--%>
                <%-- Changed by Soumyajit on 9.3.15--%>
                <asp:Button ID="btnDelete" runat="server" CssClass="allbtnDel" Text="Delete"></asp:Button>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
