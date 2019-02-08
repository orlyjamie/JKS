<%@ Page Language="c#" CodeBehind="ActionHistory.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.History.ActionHistory" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - History Action</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function CloseWindow() {
            if ('<%=iCurrentStatusID%>' == '4') {
                alert('Sorry, cannot manipulate this invoice, it is completed.')
                window.close();
            }
        }

        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
        function windowclose() {
            window.close();
        }

        function SubmitForm() {
            window.opener.document.forms[0].submit();
        }

        function CaptureClose() {

            document.body.style.cursor = 'wait';

            window.opener.doRefesh();

            opener.location.reload(true);
            alert('ffff');
        }
    </script>
</head>
<body onbeforeunload="doHourglass(); " bgcolor="#ffffff" leftmargin="0" topmargin="0"
    onload="javascript:CloseWindow();" onunload="javascript:CaptureClose();">
    <script language="javascript" src="../../dalkia/Supplier/wz_tooltip.js"></script>
    <form id="Form2" style="z-index: 102; left: 0px" method="post" runat="server">
    <table style="width: 901px; height: 500px" width="901">
        <tr>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="1" cellpadding="1" width="894" border="0" style="width: 894px;
                    height: 482px">
                    <tr>
                        <td class="PageHeader" style="height: 21px" colspan="5">
                            <asp:Label ID="lblConfirmation" runat="server" Visible="True" CssClass="PageHeader"> Invoice History</asp:Label>
                        </td>
                    </tr>
                    <tr width="100%">
                        <td style="height: 111px" width="100%">
                            <table class="tlbborder" style="width: 888px; height: 111px">
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                    </td>
                                    <td style="width: 250px">
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="NewBoldText">
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Document No</b>
                                    </td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b>Current Status</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCurrentStatus" runat="server" CssClass="NormalBody" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Document Date</b>
                                    </td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblInvoiceDate" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <%--<b>Business Unit</b>--%>
                                        <b>Internal Order</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBusinessUnit" runat="server" CssClass="NormalBody" Style="cursor: hand"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Supplier Name</b>
                                    </td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblSupplier" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b>Department</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDepartment" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Buyer Name</b>
                                    </td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblBuyer" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b><b>Nominal Code</b></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNominal" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                    </td>
                                    <td style="width: 250px">
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <asp:Label ID="lblCRn" runat="server" CssClass="NormalBody" Font-Bold="True" Width="104px">Credit Note No</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcreditnoteno" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 161px; height: 3px">
                            <asp:Label ID="lblApprovelMessage" runat="server" Visible="False" CssClass="NormalBody"
                                Font-Bold="True" Width="136px" ForeColor="Red">Approval Completed.</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" valign="top" colspan="5" height="174" style="height: 174px">
                            <asp:DataGrid ID="grdList" runat="server" Width="888px" ShowFooter="True" Font-Names="Verdana,Tahoma,Arial"
                                AutoGenerateColumns="False" GridLines="Vertical" CellPadding="2" BackColor="White"
                                BorderColor="#999999" BorderWidth="1px" BorderStyle="None">
                                <FooterStyle ForeColor="Black" BackColor="White"></FooterStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="X-Small" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Sl No">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Font-Bold="True"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Company Name">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlBuyerCompanyCode" AutoPostBack="True" runat="server" Font-Size="XX-Small">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Department">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDepartment1" AutoPostBack="True" Font-Size="XX-Small" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Nominal Code">
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" AutoPostBack="True" Font-Size="XX-Small" ID="ddlNominalCode1">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Code Description">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCodingDescription1" AutoPostBack="True" Font-Size="XX-Small"
                                                runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Business Unit">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlBusinessUnit" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr>
                                                    <td nowrap>
                                                        <asp:Label ID="Label1" runat="server" CssClass="NormalBody">	Total Net Value for Coding:</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>
                                                        <br>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>
                                                        <asp:Label ID="Label2" runat="server" CssClass="NormalBody">Net Invoice Total:</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="PO Number">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPoNumber" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'
                                                ReadOnly="True" runat="server" Width="70px">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Net value for coding">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNetVal" Text='<%# DataBinder.Eval(Container, "DataItem.NetValue") %>'
                                                runat="server" ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr>
                                                    <td nowrap>
                                                        <asp:Label ID="lblNetVal" runat="server" CssClass="NormalBody"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>
                                                        <br>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>
                                                        <asp:Label ID="lblNetInvoiceTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                                    Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                            <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="True" Width="314px" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr height="10">
                        <td style="height: 59px" align="center">
                            <table class="tlbborder" style="width: 877px; height: 32px">
                                <% if (TypeUser > 1)
                                   {%>
                                <tr>
                                    <td style="width: 206px; height: 45px" align="right">
                                        <b class="NormalBody">Comments&nbsp; :<font color="red">*</font></b>
                                    </td>
                                    <td style="width: 474px; height: 45px" align="left">
                                        <asp:TextBox ID="txtComment" runat="server" Width="432px" Height="38px" TextMode="MultiLine"
                                            MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td style="width: 138px; height: 45px">
                                    </td>
                                </tr>
                                <% }%>
                                <tr>
                                    <td style="width: 206px" align="right">
                                    </td>
                                    <td style="width: 474px" align="center" valign="top">
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" CssClass="ButtonCssNew" BorderStyle="None"
                                            BorderWidth="0px" CausesValidation="False" Text="Cancel"></asp:Button>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btndelete" CssClass="ButtonCssNew" BorderStyle="None" Text="Delete/Archive"
                                            CausesValidation="False" ToolTip="Delete" runat="server"></asp:Button>
                                    </td>
                                    <td style="width: 138px" align="left">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="bottom">
                        <td align="center" width="20%">
                        </td>
                    </tr>
                </table>
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>
    </form>
    </TD></TR></TBODY></TABLE>
</body>
</html>
