<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileDownload.aspx.cs" Inherits="FileDownload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:DataGrid ID="grdFile" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
        CellPadding="0" CellSpacing="0" CssClass="listingArea" Width="100%" OnItemCommand="grdFile_ItemCommand"
        OnItemDataBound="grdFile_ItemDataBound">
        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
        <ItemStyle></ItemStyle>
        <HeaderStyle CssClass="tableHd"></HeaderStyle>
        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Invoice No." Visible="False">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblInvNo" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceNo") %>'>
                    </asp:Label>
                    <asp:Label runat="server" ID="lblInvID" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceID") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="File Name" Visible="False">
                <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="70%"></HeaderStyle>
                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="lblDocID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.DocumentID") %>'>
                    </asp:Label>
                    <asp:Label ID="lblHidePath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ImagePath") %>'>
                    </asp:Label>
                    <asp:Label ID="lblArchPath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.archiveImagePath") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="ATTACHMENTS">
                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" CssClass="noBorder"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="HpDownload" CommandArgument="DOW" BorderWidth="0" runat="server">
                        <asp:Label ID="lblPath" runat="server"></asp:Label></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" HeaderText="Delete">
                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:ImageButton ID="HpDel" CommandArgument="DEL" BorderWidth="0" runat="server"
                        ImageUrl="../../images/delete.gif"></asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
            Mode="NumericPages"></PagerStyle>
    </asp:DataGrid>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    <asp:TextBox ID="txtinvoiceid" runat="server" Visible="False"></asp:TextBox>
    <asp:Button ID="btnCheck" runat="server" OnClick="btnCheck_Click" Text="Check" Visible="False" />
    </form>
</body>
</html>
