<%@ Page Language="c#" CodeBehind="PurchaseInvoiceHistory.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.Invoice.BuyerInvoiceHistory" %>

<%@ Register TagPrefix="uc1" TagName="bannerNB" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Purchase Invoice Log</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function fn_Validate() {
            if (document.all.ddlActionStatus.selectedIndex != 0 && document.all.ddlDocStatus.selectedIndex != 0) {
                alert('Please select either doc status or action status.');
                return (false);
            }
        }		
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%">
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
                <table width="100%">
                    <tr>
                        <td class="PageHeader" width="100%">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody">
                            Select Company Name To View Invoice Log &nbsp;
                            <asp:DropDownList ID="ddlCompany" TabIndex="4" runat="server" Width="200px" CssClass="MyInput"
                                AutoPostBack="True" DataValueField="CompanyID" DataTextField="CompanyName">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="height: 1px">
                            Supplier Name&nbsp;
                            <asp:DropDownList ID="ddlSupplier" TabIndex="4" runat="server" Width="200px" CssClass="MyInput"
                                DataValueField="CompanyID" DataTextField="CompanyName" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="height: 14px">
                            Action Status&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlActionStatus" TabIndex="4" runat="server" Width="200px"
                                CssClass="MyInput">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody">
                            Doc Status&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlDocStatus" TabIndex="4" runat="server" Width="200px" CssClass="MyInput"
                                DataValueField="StatusID" DataTextField="Status">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody">
                            User Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlUsers" TabIndex="4" runat="server" CssClass="MyInput" Width="200px"
                                DataTextField="UserName" DataValueField="UserID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody">
                            Doc No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlInvoiceNo" TabIndex="4" runat="server" CssClass="MyInput"
                                Width="200px" DataTextField="InvoiceNo" DataValueField="InvoiceNo">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" valign="top">
                            Doc Date Ranges &nbsp;&nbsp;From
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="MyInput">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="MyInput">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlday" runat="server" CssClass="MyInput">
                            </asp:DropDownList>
                            <br>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            TO
                            <asp:DropDownList ID="ddlYear1" runat="server" CssClass="MyInput">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="MyInput">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlday1" runat="server" CssClass="MyInput">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody">
                            Net Total Ranges &nbsp;
                            <asp:TextBox ID="textRange1" runat="server"></asp:TextBox>-<asp:TextBox ID="textRange2"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody">
                            Department&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddldept" runat="server" CssClass="MyInput" AutoPostBack="True"
                                DataValueField="DepartmentID" DataTextField="Department" Width="200px">
                            </asp:DropDownList>
                            &nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblMessage" runat="server" CssClass="MyInput" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <!-- Main Content Panel Ends-->
                <table id="Table1" width="100%">
                    <tr>
                        <td class="PageHeader" width="100%">
                            Purchase Invoice Log History
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="grdInvCur" runat="server" Width="100%" AllowSorting="True" Font-Size="10pt"
                                Font-Names="verdana,Tahoma,Arial" AutoGenerateColumns="False" GridLines="Vertical"
                                CellPadding="3" BorderWidth="1px" BorderStyle="None" BackColor="White" Height="88px"
                                BorderColor="#999999" PageSize="15" AllowPaging="True">
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
                                            <a href='InvoiceConfirmationNL.aspx?InvoiceID=<%#DataBinder.Eval(Container.DataItem,"InvoiceID")%>'>
                                                <%# DataBinder.Eval(Container.DataItem,"ReferenceNo")%>
                                                <!--<img src="../images/GridSelect.jpg" border="0"> -->
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn Visible="False" HeaderText="History">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetLogURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>">
                                                <b>Invoice Log History </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Action">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>"><b>
                                                Action </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status History">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetStatusURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>">
                                                <b>Status History </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="DocType" HeaderText="Doc Type"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Supplier" SortExpression="Supplier" HeaderText="Supplier">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="VendorID" SortExpression="VendorID" HeaderText="VendorID">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DocStatus" HeaderText="Doc Status"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ActionStatus" HeaderText="Action Status"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ActionDate" HeaderText="Date Actioned" DataFormatString="{0:dd-MM-yyyy}">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="ParentRowFlag" HeaderText="ParentRowFlag">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="True" DataField="User" HeaderText="User"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Currency" HeaderText="Currency"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Net" SortExpression="Net" HeaderText="Net"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="VAT" SortExpression="VAT" HeaderText="VAT"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Total" SortExpression="Total" HeaderText="Gross"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Comment" HeaderText="Comment"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Doc Attachments">
                                        <ItemTemplate>
                                            <%#GetDocumentWithPath(DataBinder.Eval(Container.DataItem,"DocAttachments"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="InvoiceDate" SortExpression="InvoiceDate"
                                        HeaderText="Invoice Date" DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DeliveryDate" SortExpression="DeliveryDate"
                                        HeaderText="Delivery Date" DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="BranchCode" HeaderText="Company"></asp:BoundColumn>
                                    <asp:TemplateColumn Visible="False" HeaderText="Pass">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetCommentURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>">
                                                <b>Show History </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
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
