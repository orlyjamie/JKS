<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>

<%@ Page Language="c#" CodeBehind="BranchBrowseNL.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.BranchBrowseNL" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Browse Branches</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function GoToURL(iBranchID) {
            if ('<%=Session["CompanyTypeID"]%>' == '0') {
                var iCompanyID = null;
                iCompanyID = document.all.hdCompanyID.value;

                if (iCompanyID != '0') {
                    window.location.href = "BranchEditNL.aspx?BranchID=" + iBranchID + "&CID=" + iCompanyID;
                }
                else {
                    alert('Please select a company name.');
                }
            }
            else {
                window.location.href = "BranchEditNL.aspx?BranchID=" + iBranchID;
            }
        }
        function ShowHideTableRows() {
            if ('<%=Session["CompanyTypeID"]%>' != '0') {
                document.getElementById("trHidden").style.display = "none";
                document.getElementById("trDropDown").style.display = "none";
            }
        }
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
    </script>
</head>
<body onbeforeunload="javascript:doHourglass();" bgcolor="#ffffff" leftmargin="0"
    topmargin="0" onload="javascript:ShowHideTableRows();" onunload="javascript:doHourglass();">
    <form id="Form1" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
    </asp:Panel>
    <table width="100%">
        <tr>
            <td valign="top" width="150" bgcolor="gainsboro">
                <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
            </td>
            <td>
                <table style="width: 976px; height: 272px">
                    <tr>
                        <td valign="top" colspan="8">
                            <asp:ImageButton ID="imgBtnAddBranch" onmouseover="this.src='../../images/AddBranch_.jpg';"
                                onmouseout="this.src='../../images/AddBranch.jpg';" runat="server" ImageUrl="../../images/AddBranch.jpg">
                            </asp:ImageButton>
                        </td>
                    </tr>
                    <tr id="trMessage">
                        <td valign="top" align="center" colspan="8">
                            <asp:Label ID="lblMessage" runat="server" BorderStyle="None" ForeColor="Red" CssClass="MyInput"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trHidden">
                        <td class="NormalBody" valign="top" colspan="8">
                            <input id="hdCompanyID" type="hidden" value="0" name="hdCompanyID" runat="server">
                        </td>
                    </tr>
                    <tr id="trDropDown">
                        <td class="NormalBody" valign="top" colspan="8">
                            Select Company Name&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlCompany" TabIndex="4" runat="server" Width="200px" CssClass="MyInput"
                                DataTextField="CompanyName" DataValueField="CompanyID" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:DataGrid ID="grdBranch" runat="server" Width="976px" CssClass="listingArea"
                                OnItemCommand="Datagrid_Click" BorderColor="#999999" Height="56px" CellPadding="3"
                                GridLines="Vertical" AutoGenerateColumns="False">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle></ItemStyle>
                                <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Edit">
                                        <ItemTemplate>
                                            <a onclick='GoToURL(<%#DataBinder.Eval(Container.DataItem,"BranchID")%>)' href="#">
                                                <img src="../../images/GridEdit.jpg" border="0">
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="../../images/GridDelete.jpg"
                                                CommandName="DELETERECORD" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"BranchID")%>'>
                                            </asp:ImageButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Branch" HeaderText="Branch"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="County" HeaderText="County"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Country" HeaderText="Country"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="PostCode" HeaderText="Post Code"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Contact" HeaderText="Contact"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Email" HeaderText="Email"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                </PagerStyle>
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
