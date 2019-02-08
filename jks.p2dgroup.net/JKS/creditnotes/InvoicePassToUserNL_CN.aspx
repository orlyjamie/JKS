<%@ Page Language="c#" CodeBehind="InvoicePassToUserNL_CN.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.CreditNotes.InvoicePassToUserNL_CN" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>InvoicePassToUserNL_CN</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function CloseWindow() {
            if ('<%=iCurrentStatusID%>' == '17' || '<%=iCurrentStatusID%>' == '18') {
                alert('Sorry, cannot manipulate this credit note, it is completed.')
                window.close();
            }
        }

        function SubmitForm() {
            window.opener.document.forms[0].submit();
        }
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0" onload="javascript:CloseWindow();">
    <form id="Form1" method="post" runat="server">
    <table style="width: 967px; height: 520px" width="967">
        <tr>
            <td width="100%" valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td class="PageHeader" style="height: 18px" colspan="5">
                            <asp:Label ID="lblConfirmation" runat="server" Visible="True" CssClass="PageHeader"> Pass Credit Note To User</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 9px" colspan="5">
                            <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody">Credit Note No. : </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 181px; height: 9px">
                            <asp:Label ID="lblAssociatedInvoice" runat="server" CssClass="NormalBody">Associated Invoice No. : </asp:Label>
                        </td>
                        <td style="width: 375px; height: 9px">
                            <asp:TextBox ID="txtAssociatedInvoice" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="181" class="NormalBody" style="width: 181px; height: 21px">
                            Supplier&nbsp;
                        </td>
                        <td colspan="2" style="width: 506px; height: 21px">
                            <asp:Label ID="lblSupplier" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td width="185" class="NormalBody" style="width: 187px; height: 21px">
                            Buyer
                        </td>
                        <td width="123" style="height: 21px">
                            <asp:Label ID="lblBuyer" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 181px">
                            Credit Note Date
                        </td>
                        <td colspan="2" style="width: 506px">
                            <asp:Label ID="lblInvoiceDate" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                        <td class="NormalBody" style="display: none; width: 187px">
                            Despatch Date
                        </td>
                        <td>
                            <asp:Label ID="lblDespatchDate" runat="server" CssClass="NormalBody" Style="display: none"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px; height: 22px">
                            Current Status
                        </td>
                        <td colspan="2" style="width: 506px; height: 22px">
                            <asp:Label ID="lblCurrentStatus" runat="server" CssClass="NormalBody" Font-Bold="True"></asp:Label>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 22px">
                            <asp:Label ID="lblOverdue" runat="server" Visible="False" CssClass="NormalBody" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="height: 22px">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px; height: 10px">
                            Action
                        </td>
                        <td colspan="2" style="width: 506px; height: 10px">
                            <asp:DropDownList ID="cboStatus" Width="200px" runat="server" AutoPostBack="True"
                                Font-Size="XX-Small" OnSelectedIndexChanged="cboStatus_SelectedIndexChanged"
                                DataTextField="Status" DataValueField="StatusID">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 10px">
                            <asp:Label ID="lblCompanyCode" runat="server" Visible="False" CssClass="NormalBody"
                                Font-Bold="True">Company Code</asp:Label>
                        </td>
                        <td style="height: 10px">
                            <asp:DropDownList ID="ddlBuyerCompanyCode" Width="200px" runat="server" Visible="False"
                                Font-Size="XX-Small" OnSelectedIndexChanged="cboStatus_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px; height: 18px">
                        </td>
                        <td colspan="2" style="width: 506px; height: 18px">
                            <asp:DropDownList ID="cboYearDate" Width="72px" runat="server" Visible="False" CssClass="MyInput"
                                Height="16px">
                            </asp:DropDownList>
                            <asp:DropDownList ID="cboMonthDate" Width="64px" runat="server" Visible="False" CssClass="MyInput">
                            </asp:DropDownList>
                            <asp:DropDownList ID="cboDayDate" Width="56px" runat="server" Visible="False" CssClass="MyInput">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 18px">
                            <asp:Label ID="lblApprovelMessage" Width="85px" runat="server" Visible="False" CssClass="NormalBody"
                                Font-Bold="True" ForeColor="Red">Approval Completed.</asp:Label>
                        </td>
                        <td style="height: 18px">
                        </td>
                    </tr>
                    <tr id="trUsers" runat="server">
                        <td class="NormalBody" style="width: 193px">
                            Users
                        </td>
                        <td colspan="2" style="width: 506px">
                            <asp:DropDownList ID="cboUsers" Width="470px" runat="server" Font-Size="XX-Small"
                                DataTextField="UserName" DataValueField="UserID">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px">
                            <asp:CheckBox ID="chkCeo" runat="server" Visible="False" Text="CEO" Enabled="False">
                            </asp:CheckBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px">
                            Comment
                        </td>
                        <td colspan="2" style="width: 506px">
                            <asp:TextBox ID="txtComment" Width="248px" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td class="NormalBody" style="display: none; width: 187px">
                            Net&nbsp;Amount
                        </td>
                        <td style="display: none">
                            <asp:Label ID="lblTotalAmount" runat="server" CssClass="NormalBody"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px">
                            Department
                        </td>
                        <td colspan="2" style="width: 506px">
                            <asp:DropDownList ID="ddlDepartment" Width="240px" runat="server" Font-Size="XX-Small"
                                OnSelectedIndexChanged="cboStatus_SelectedIndexChanged" DataTextField="Department"
                                DataValueField="DepartmentID" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px">
                            Project
                        </td>
                        <td class="NormalBody" colspan="4">
                            <asp:DropDownList ID="ddlProject" Width="200px" runat="server" Font-Size="XX-Small"
                                OnSelectedIndexChanged="cboStatus_SelectedIndexChanged" DataTextField="ProjectName"
                                DataValueField="ProjectID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px; height: 3px">
                            Nominal Code
                        </td>
                        <td colspan="2" style="width: 506px; height: 3px">
                            <asp:DropDownList ID="ddlNominalCode" Width="240px" runat="server" Font-Size="XX-Small"
                                OnSelectedIndexChanged="cboStatus_SelectedIndexChanged" DataTextField="NominalCode"
                                DataValueField="NominalCodeID" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 3px">
                        </td>
                        <td class="NormalBody" style="height: 3px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 193px; height: 15px">
                            Coding Description
                        </td>
                        <td colspan="2" style="width: 506px; height: 15px">
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
                        <td class="NormalBody" style="width: 193px; height: 3px" valign="top">
                            Description<font color="red"> *</font>
                        </td>
                        <td colspan="2" style="width: 506px; height: 3px">
                            <asp:TextBox ID="txtDescription" Width="248px" runat="server" Height="48px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 3px">
                        </td>
                        <td class="NormalBody" style="height: 3px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 193px; height: 3px">
                        </td>
                        <td colspan="2" class="NormalBody" style="width: 506px; height: 3px">
                            <asp:RequiredFieldValidator ID="rfv_FOR_Description" runat="server" Enabled="False"
                                ErrorMessage="Please enter description." ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
                        </td>
                        <td class="NormalBody" style="width: 187px; height: 3px">
                        </td>
                        <td class="NormalBody" style="height: 3px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" align="left" colspan="5" class="NormalBody">
                            <font color="red">* = Mandatory Field</font>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom" style="width: 193px">
                            <asp:Button ID="btnBack" TabIndex="13" Width="100px" runat="server" Visible="False"
                                CssClass="BackButton" Height="23px" BorderStyle="None" BorderWidth="0px"></asp:Button>&nbsp;&nbsp;
                            <input id="hdWindowFlag" type="hidden" value="0" name="hdWindowFlag" runat="server">
                            <asp:CustomValidator ID="CustomValidator_FOR_Status" runat="server" ControlToValidate="cboStatus"></asp:CustomValidator>
                        </td>
                        <td width="375" valign="bottom" style="width: 375px">
                            <a onclick="javascript:window.close();" href="#">
                                <img src="../../images/cancel1.gif" border="0"></a>
                        </td>
                        <td width="403" valign="bottom">
                            <asp:Button ID="btnSubmit" TabIndex="13" Width="100px" runat="server" CssClass="SubmitButton"
                                Height="23px" BorderStyle="None" BorderWidth="0px"></asp:Button>
                        </td>
                        <td valign="middle">
                            &nbsp;
                        </td>
                        <td valign="middle">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="5">
                            &nbsp;&nbsp;
                            <asp:Label ID="OutError" Width="839px" runat="server" Visible="False" CssClass="NormalBody"
                                Font-Size="Smaller" ForeColor="Red">Please Select an User</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="5">
                            <asp:Button ID="btnPassInvoice" TabIndex="13" Width="100px" runat="server" CssClass="ConfirmInvoice"
                                Height="23px" BorderStyle="None" BorderWidth="0px"></asp:Button>
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
