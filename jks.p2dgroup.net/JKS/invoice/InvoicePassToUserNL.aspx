<%@ Page Language="c#" CodeBehind="InvoicePassToUserNL.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.Invoice.InvoicePassToUserNL" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>InvoicePassToUserNL</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function CloseWindow() {
            if ('<%=iCurrentStatusID%>' == '4') {
                alert('Sorry, cannot manipulate this invoice, it is completed.')
                window.close();
            }
        }

        function SubmitForm() {
            window.opener.document.forms[0].submit();
        }
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table style="width: 100%; height: 495px" width="100%">
        <tr>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td class="PageHeader" style="height: 18px" colspan="5">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader" Visible="True"> Pass Invoice To User</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 9px" colspan="5">
                            <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody">Invoice No. : </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 9px" colspan="5">
                            <asp:Label ID="lblVoucher" runat="server" CssClass="NormalBody">Voucher No.:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 21px" width="185">
                            Supplier&nbsp;
                        </td>
                        <td style="width: 506px; height: 21px" colspan="2">
                            <asp:Label ID="lblSupplier" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 21px" width="255">
                            Buyer
                        </td>
                        <td style="height: 21px" width="102">
                            <asp:Label ID="lblBuyer" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px">
                            Invoice Date
                        </td>
                        <td style="width: 506px" colspan="2">
                            <asp:Label ID="lblInvoiceDate" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="NormalBody" style="display: none; width: 187px">
                            Despatch Date
                        </td>
                        <td>
                            <asp:Label ID="lblDespatchDate" Style="display: none" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 22px">
                            Current Status
                        </td>
                        <td style="width: 506px; height: 22px" colspan="2">
                            <asp:Label ID="lblCurrentStatus" runat="server" CssClass="NormalBody" Font-Bold="True"></asp:Label>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 22px">
                            <asp:Label ID="lblOverdue" runat="server" CssClass="NormalBody" Visible="False" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="height: 22px">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 10px">
                            Action
                        </td>
                        <td style="width: 506px; height: 10px" colspan="2">
                            <asp:DropDownList ID="cboStatus" runat="server" Width="200px" DataValueField="StatusID"
                                DataTextField="Status" OnSelectedIndexChanged="cboStatus_SelectedIndexChanged"
                                Font-Size="XX-Small" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 10px">
                            <asp:Label ID="lblCompanyCode" runat="server" CssClass="NormalBody" Visible="False"
                                Font-Bold="True">Company Code</asp:Label>
                        </td>
                        <td style="height: 10px">
                            <asp:DropDownList ID="ddlBuyerCompanyCode" runat="server" Width="200px" Visible="False"
                                OnSelectedIndexChanged="cboStatus_SelectedIndexChanged" Font-Size="XX-Small">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 10px">
                            Account
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 10px">
                            SubAccount
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 18px">
                        </td>
                        <td style="width: 506px; height: 18px" colspan="2">
                            <asp:DropDownList ID="cboYearDate" runat="server" Width="72px" CssClass="MyInput"
                                Visible="False" Height="16px">
                            </asp:DropDownList>
                            <asp:DropDownList ID="cboMonthDate" runat="server" Width="64px" CssClass="MyInput"
                                Visible="False">
                            </asp:DropDownList>
                            <asp:DropDownList ID="cboDayDate" runat="server" Width="56px" CssClass="MyInput"
                                Visible="False">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 18px">
                            <asp:Label ID="lblApprovelMessage" runat="server" Width="85px" CssClass="NormalBody"
                                Visible="False" Font-Bold="True" ForeColor="Red">Approval Completed.</asp:Label>
                        </td>
                        <td style="height: 18px">
                        </td>
                    </tr>
                    <tr id="trUsers" runat="server">
                        <td class="NormalBody" style="width: 185px">
                            Users
                        </td>
                        <td style="width: 506px" colspan="2">
                            <asp:DropDownList ID="cboUsers" runat="server" Width="470px" DataValueField="UserID"
                                DataTextField="UserName" Font-Size="XX-Small">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px">
                            <asp:CheckBox ID="chkCeo" runat="server" Visible="False" Enabled="False" Text="CEO">
                            </asp:CheckBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px">
                            Comment
                        </td>
                        <td style="width: 506px" colspan="2">
                            <asp:TextBox ID="txtComment" runat="server" Width="248px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td class="NormalBody" style="display: none; width: 187px">
                            Net&nbsp;Amount
                        </td>
                        <td style="display: none; width: 187px">
                            <asp:Label ID="lblTotalAmount" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 26px">
                            Department
                        </td>
                        <td style="width: 506px; height: 26px" colspan="2">
                            <asp:DropDownList ID="ddlDepartment" runat="server" Width="240px" DataValueField="DepartmentID"
                                DataTextField="Department" OnSelectedIndexChanged="cboStatus_SelectedIndexChanged"
                                Font-Size="XX-Small" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 26px">
                            Project
                        </td>
                        <td class="NormalBody" style="height: 26px" colspan="4">
                            <asp:DropDownList ID="ddlProject" runat="server" Width="200px" DataValueField="ProjectID"
                                DataTextField="ProjectName" OnSelectedIndexChanged="cboStatus_SelectedIndexChanged"
                                Font-Size="XX-Small">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 11px">
                            Nominal Code
                        </td>
                        <td style="width: 506px; height: 11px" colspan="2">
                            <asp:DropDownList ID="ddlNominalCode" runat="server" Width="240px" DataValueField="NominalCodeID"
                                DataTextField="NominalCode" OnSelectedIndexChanged="cboStatus_SelectedIndexChanged"
                                Font-Size="XX-Small" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 11px">
                        </td>
                        <td class="NormalBody" style="height: 11px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 15px">
                            Coding Description
                        </td>
                        <td style="width: 506px; height: 15px" colspan="2">
                            <asp:DropDownList ID="ddlCodingDescription" runat="server" Width="240px" DataValueField="CodingDescriptionID"
                                DataTextField="DDescription" Font-Size="XX-Small" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 15px">
                        </td>
                        <td class="NormalBody" style="height: 15px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 3px" valign="top">
                            Description <font color="red">*</font>
                        </td>
                        <td style="width: 506px; height: 3px" colspan="2">
                            <asp:TextBox ID="txtDescription" runat="server" Width="248px" Height="48px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 3px">
                        </td>
                        <td class="NormalBody" style="height: 3px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 185px; height: 3px">
                        </td>
                        <td class="NormalBody" style="width: 506px; height: 3px" colspan="2">
                            <asp:RequiredFieldValidator ID="rfv_FOR_Description" runat="server" Enabled="False"
                                ControlToValidate="txtDescription" ErrorMessage="Please enter description."></asp:RequiredFieldValidator>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 3px">
                        </td>
                        <td class="NormalBody" style="height: 3px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="height: 15px" colspan="5">
                            <font color="red">* = Mandatory Field</font>
                        </td>
                    </tr>
                    <tr valign="bottom">
                        <td style="width: 185px" height="40">
                            <asp:Button ID="btnBack" TabIndex="13" runat="server" Width="100px" CssClass="BackButton"
                                Visible="False" Height="23px" BorderWidth="0px" BorderStyle="None"></asp:Button><input
                                    id="hdWindowFlag" type="hidden" value="0" name="hdWindowFlag" runat="server">
                            <asp:CustomValidator ID="CustomValidator_FOR_Status" runat="server" ControlToValidate="cboStatus"></asp:CustomValidator>
                        </td>
                        <td width="110" height="40">
                            <img src="../../images/cancel1.gif"><a onclick="javascript:window.close();" href="#"></a>
                        </td>
                        <td width="321">
                            <asp:Button ID="btnSubmit" TabIndex="13" runat="server" Width="100px" CssClass="SubmitButton"
                                Height="23px" BorderWidth="0px" BorderStyle="None"></asp:Button>&nbsp;&nbsp;
                        </td>
                        <td height="40">
                        </td>
                        <td height="40">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="5">
                            &nbsp;&nbsp;
                            <asp:Label ID="OutError" runat="server" Width="839px" CssClass="NormalBody" Visible="False"
                                Font-Size="Smaller" ForeColor="Red">Please Select an User</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="5">
                            <asp:Button ID="btnPassInvoice" TabIndex="13" runat="server" Width="100px" CssClass="ConfirmInvoice"
                                Height="23px" BorderWidth="0px" BorderStyle="None"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="5">
                        </td>
                    </tr>
                </table>
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>
    <script language="javascript">
        if (document.all.hdWindowFlag.value == '1') {
            window.opener.document.forms[0].submit();
            window.close();
        }
    </script>
    </form>
</body>
</html>
