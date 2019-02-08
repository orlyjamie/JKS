<%@ Page Language="c#" CodeBehind="CompanyBrowse.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.CompanyBrowse" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Browse Companies</title>
    <meta name="vs_showGrid" content="True">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
    </script>
</head>
<body onbeforeunload="javascript:doHourglass();" onunload="javascript:doHourglass();"
    bgcolor="#ffffff" topmargin="0" leftmargin="0">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
    </asp:Panel>
    <table width="100%">
        <tr>
            <td width="150" bgcolor="gainsboro" valign="top">
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
                <a href='CompanyEdit.aspx?PID=<%=iParentCompanyID%>'>
                    <img src="../../images/AddCompany.jpg" border="0"></a>
                <asp:HyperLink ID="Hyperlink2" runat="server" Width="96px" Height="24px" NavigateUrl="CompanyUploadSupplier_NL.aspx"
                    ImageUrl="../../images/uploadsupplier_m.gif">Upload Suppliers</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                <br>
                <asp:DataGrid ID="grdCompany" runat="server" BorderColor="#999999" Width="100%" Height="88px"
                    BackColor="White" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                    AutoGenerateColumns="False" Font-Names="verdana" Font-Size="10pt" OnItemCommand="Datagrid_Click">
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                    <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                    </ItemStyle>
                    <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                        BackColor="#3399CC"></HeaderStyle>
                    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="Edit">
                            <HeaderStyle Width="50px"></HeaderStyle>
                            <ItemTemplate>
                                <a href='CompanyEdit.aspx?CompanyID=<%#DataBinder.Eval(Container.DataItem,"CompanyID")%>&amp;PID=<%=iParentCompanyID%>'>
                                    <img src="../../images/GridEdit.jpg" border="0">
                                </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="../../images/GridDelete.jpg"
                                    CommandName="DELETERECORD" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"CompanyID")%>'>
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CompanyCode" HeaderText="Company Code">
                            <HeaderStyle Width="120px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Company">
                            <HeaderStyle Width="300px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="NetworkID" HeaderText="NetworkID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="SignUpDate" HeaderText="Sign-up Date" DataFormatString="{0:dd-MM-yyyy}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CreateDate" HeaderText="Created" DataFormatString="{0:dd-MM-yyyy}">
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
