<%@ Page Language="c#" CodeBehind="CurrentAction.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.Current.CurrentAction" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>CurrentAction</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function DoValidation(CreditInvoiceNo) {
            if (document.all["txtCreditNoteNo"].value != CreditInvoiceNo) {
                document.all["lblErrMsg"].value = "Please enter correct Credit Note No.";
                alert(document.all["lblErrMsg"].value);
                return false;
            }
            else
                return true;
        }

        function roll(img_name, img_src) {
            document.getElementById(img_name).src = img_src;
        }
        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == variable) {
                    return pair[1];
                }
            }
        }
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }

        function CaptureClose(sInvoiceID, sDocType) {
            document.body.style.cursor = 'wait';
            var id = getQueryVariable("InvoiceID");
            var docType = getQueryVariable("DocType");
            var sValue = "id=" + id + "|docType=" + docType;
            window.opener.__doPostBack('btnCloseAction', sValue);

        }
    </script>
</head>
<body onbeforeunload="doHourglass();" onunload="javascript:CaptureClose();" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table>
        <tr>
            <td style="color: red" valign="top">
                <table style="height: 22px" width="100%">
                    <tr>
                        <td class="PageHeader" style="height: 18px" colspan="5">
                            Current Action To User
                        </td>
                    </tr>
                </table>
                <table style="height: 193px" width="100%">
                    <tr>
                        <td class="NormalBody" style="width: 165px">
                            <font class="NormalBody" size="2">Document No</font>
                        </td>
                        <td>
                            <asp:Label ID="lbldocumentno" runat="server" CssClass="NormalBody"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 165px; height: 17px">
                            <font class="NormalBody" size="2">Voucher No:</font>
                        </td>
                        <td style="height: 17px">
                            <asp:Label ID="lblVoucherno" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 165px">
                            <font class="NormalBody" size="2">Document Date</font>
                        </td>
                        <td>
                            <asp:Label ID="lbldocumentdate" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 165px; height: 15px">
                            <font class="NormalBody" size="2">Supplier Name</font>
                        </td>
                        <td style="height: 15px">
                            <asp:Label ID="lblsuppliername" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 165px">
                            <font class="NormalBody" size="2">Document Status:</font>
                        </td>
                        <td>
                            <asp:Label ID="lbldocumentstatus" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 165px">
                            <font class="NormalBody" size="2">Authorisation String:</font>
                        </td>
                        <td>
                            <asp:Label ID="lblauthstring" runat="server" CssClass="NormalBody"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 165px">
                            <font class="NormalBody" size="2">Department</font>
                        </td>
                        <td>
                            <asp:Label ID="lbldepartment" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 165px">
                            <font size="2">
                                <asp:Label ID="lblassociatedinvoiceno" runat="server" CssClass="NormalBody" Visible="False"></asp:Label></font>
                        </td>
                        <td>
                            <asp:Label ID="lblassociatedno" runat="server" CssClass="NormalBody" Visible="False"></asp:Label><font
                                size="2"></font>
                        </td>
                    </tr>
                    <span id="SpanCrn" runat="server">
                        <tr>
                            <td class="NormalBody" style="width: 165px">
                                Credit Note Number
                            </td>
                            <td class="NormalBody">
                                <asp:Label ID="lblCrno" runat="server" CssClass="NormalBody"></asp:Label>
                            </td>
                        </tr>
                    </span>
                </table>
                <table width="100%">
                    <tr>
                        <td valign="top">
                            <asp:DataGrid ID="GDlineinfo" runat="server" ShowFooter="True" Width="100%" PageSize="15"
                                BorderColor="#999999" BackColor="White" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False" Font-Names="verdana,Tahoma,Arial"
                                Font-Size="10pt">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <FooterStyle Font-Size="Smaller" Font-Names="Verdana" HorizontalAlign="Right" ForeColor="Black"
                                    BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="LineNo" HeaderText="Line No">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GLCompanyID" HeaderText="GL CompanyID">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AccountingUnit" HeaderText="Accounting Unit">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Account" HeaderText="Account">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SubAccount" HeaderText="Sub Account" FooterText="Total :">
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Net">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetValue" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"NETAMT","{0:N2}")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNetTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn Visible="False" HeaderText="Vat">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaxValue" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TAXAMT","{0:N2}")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTaxTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn Visible="False" HeaderText="Gross">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossValue" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"GROSSAMT","{0:N2}")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrossTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td style="width: 242px">
                            <font class="NormalBody" size="2">Comment Box</font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcomment" runat="server" CssClass="NormalBody" Width="424px" Rows="6"
                                Columns="50" Height="65px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 242px; height: 33px">
                            <font class="NormalBody" size="2">Activity Code</font>
                        </td>
                        <td style="height: 33px">
                            <asp:DropDownList ID="ddlactivitycode" runat="server" CssClass="NormalBody" Width="424px"
                                DataTextField="ActivityCode" DataValueField="ActivityCodeID" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 242px; height: 20px">
                            <font class="NormalBody" size="2">Account Category</font>
                        </td>
                        <td style="height: 20px">
                            <asp:DropDownList ID="ddlaccountcategory" runat="server" CssClass="NormalBody" Width="424px"
                                DataTextField="AccountCategory" DataValueField="AccountCategoryID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 242px; height: 14px" valign="top">
                            <asp:Label ID="lblCreditNoteNo" runat="server" CssClass="NormalBody" Text="Associated Credit Note No."></asp:Label></FONT>
                        </td>
                        <td style="height: 14px">
                            <asp:TextBox ID="txtCreditNoteNo" runat="server" Width="424px" AutoPostBack="True"
                                EnableViewState="False"></asp:TextBox><asp:Label ID="lblErrMsg" runat="server" CssClass="NormalBody"
                                    Width="256px" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style="height: 13px" width="100%">
                    <tr>
                        <td style="width: 242px; height: 9px">
                            <font size="2">
                                <asp:Label ID="lblreasonforreject" runat="server" CssClass="NormalBody" Visible="False"></asp:Label></font>
                        </td>
                        <td style="height: 9px">
                            <asp:DropDownList ID="ddlreject" runat="server" CssClass="NormalBody" Visible="False"
                                Width="424px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvRejectionCode" runat="server" CssClass="NormalBody"
                                Width="175px" Enabled="False" ControlToValidate="ddlreject" ErrorMessage="Please select a rejection code."></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 242px">
                            <font size="2">
                                <asp:Label ID="lblrejectioncomment" runat="server" CssClass="NormalBody" Visible="False"
                                    Width="104px"></asp:Label></font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtrejcomment" runat="server" CssClass="NormalBody" Visible="False"
                                Width="424px" Height="65px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnapprove" runat="server" CssClass="ButtonCss" Width="100px" BorderStyle="None"
                                Height="23px" Text="Approve" ToolTip="Approve"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btnreject" runat="server" CssClass="ButtonCss" Width="100px" BorderStyle="None"
                                Height="23px" Text="Reject" ToolTip="Reject"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btnreopen" runat="server" CssClass="ButtonCss" Width="100px" BorderColor="#0081C5"
                                BorderStyle="None" Height="23px" Text="Reopen" ToolTip="Reopen"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btndelete" runat="server" CssClass="ButtonCss" Width="100px" BorderStyle="None"
                                Height="23px" Text="Delete" ToolTip="Delete" CausesValidation="False"></asp:Button>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblmessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
