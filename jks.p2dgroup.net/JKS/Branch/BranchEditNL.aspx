<%@ Page Language="c#" CodeBehind="BranchEditNL.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.BranchEditNL" %>

<%@ Register TagPrefix="uc2" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Branch Add/Edit</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function ShowHideRows() {
            if ('<%=strCompanyTypeID%>' == '2') {
                document.getElementById("trInvoiceLocation").style.display = "none";
                document.getElementById("trDeliveryLocation").style.display = "none";
            }
        }
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
			
    </script>
</head>
<body onbeforeunload="javascript:doHourglass();" onunload="javascript:doHourglass();"
    bgcolor="#ffffff" leftmargin="0" topmargin="0" onload="javascript:ShowHideRows();">
    <form id="Form1" method="post" runat="server">
    <asp:Panel ID="Panel2" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc2:bannerUM ID="bannerUM1" runat="server"></uc2:bannerUM>
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
                <table width="100%" border="0">
                    <tr>
                        <td style="width: 46px; height: 21px" width="46">
                        </td>
                        <td style="width: 312px" colspan="2">
                            <asp:Label ID="lblHeader" runat="server" CssClass="PageHeader" Width="100%">Label</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Branch Name <font color="red">*</font>
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtName" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="NormalBody"
                                ControlToValidate="txtName" ErrorMessage="Please enter branch name."></asp:RequiredFieldValidator>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 6px">
                        </td>
                        <td class="NormalBody" style="width: 153px; height: 6px">
                            Branch&nbsp;Code<font color="red"> *</font>
                        </td>
                        <td style="width: 170px; height: 6px">
                            <asp:TextBox ID="txtBranchCode" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td class="NormalBody" style="height: 6px" align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="NormalBody"
                                ControlToValidate="txtBranchCode" ErrorMessage="Please enter branch code."></asp:RequiredFieldValidator><asp:Label
                                    ID="lblErrorDuplicateBranchCode" runat="server" ForeColor="Red" Visible="False">Duplicate branch code.</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Address<font color="red"> *</font>
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtAddress1" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="NormalBody"
                                ControlToValidate="txtAddress1" ErrorMessage="Please enter address"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 153px; height: 21px">
                        </td>
                        <td style="width: 170px; height: 21px">
                            <asp:TextBox ID="txtAddress2" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtAddress3" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtAddress4" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtAddress5" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            County <font color="red">*</font>
                        </td>
                        <td style="width: 170px">
                            <asp:DropDownList ID="cboCounty" runat="server" CssClass="MyInput" Width="150px">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtCounty" runat="server" CssClass="MyInput" Width="150px" Visible="False"
                                MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfv_FOR_County" runat="server" CssClass="NormalBody"
                                ControlToValidate="cboCounty" ErrorMessage="Please select a county." InitialValue="0"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px">
                        </td>
                        <td class="NormalBody" style="width: 153px; height: 20px">
                            Country <font color="red">*</font>
                        </td>
                        <td style="width: 170px; height: 20px">
                            <asp:DropDownList ID="cboCountry" runat="server" CssClass="MyInput" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 20px">
                            <asp:RequiredFieldValidator ID="rfv_FOR_Country" runat="server" CssClass="NormalBody"
                                ControlToValidate="cboCountry" ErrorMessage="Please select a country." InitialValue="0"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Post Code<font color="red"> *</font>
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="NormalBody"
                                ControlToValidate="txtPostCode" ErrorMessage="Please enter post code."></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trInvoiceLocation">
                        <td style="height: 22px">
                        </td>
                        <td class="NormalBody" style="width: 153px; height: 22px">
                            Invoice Location
                        </td>
                        <td style="width: 170px; height: 22px">
                            <asp:CheckBox ID="chkInvoice" runat="server" CssClass="MyInput" ForeColor="White">
                            </asp:CheckBox>
                        </td>
                        <td style="height: 22px">
                        </td>
                    </tr>
                    <tr id="trDeliveryLocation">
                        <td style="height: 22px">
                        </td>
                        <td class="NormalBody" style="width: 153px; height: 22px">
                            Delivery Location
                        </td>
                        <td style="width: 170px; height: 22px">
                            <asp:CheckBox ID="chkDelivery" runat="server" CssClass="MyInput" ForeColor="White">
                            </asp:CheckBox>
                        </td>
                        <td style="height: 22px">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 153px; height: 21px">
                            Telephone
                        </td>
                        <td style="width: 170px; height: 21px">
                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                            <p>
                                &nbsp;</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Fax
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtFax" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Contact
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtPContact" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="width: 153px">
                            <p class="NormalBody">
                                Email</p>
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtPEmail" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="height: 14px">
                        </td>
                        <td class="NormalBody" style="width: 153px; height: 14px">
                            Sales Invoice Volume
                        </td>
                        <td style="width: 170px; height: 14px">
                            <asp:TextBox ID="txtSalesVolume" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td style="height: 14px">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 153px; height: 21px">
                            Purchase Volume
                        </td>
                        <td style="width: 170px; height: 21px">
                            <asp:TextBox ID="txtPurchaseVolume" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Active Supplier Count
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtSupplierCount" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Active Customer Count
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtCustomerCount" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Approx. Turnover
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtTurnover" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                        </td>
                        <td class="NormalBody" style="width: 153px">
                            Web site
                        </td>
                        <td style="width: 170px">
                            <asp:TextBox ID="txtWebsite" runat="server" CssClass="MyInput" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnSubmit" CssClass="ButtonCss" runat="server" Width="100px" Height="24px"
                                BorderStyle="None" Text="Submit" ToolTip="Submit"></asp:Button>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="NormalBody" colspan="2">
                            <font color="red">* = Mandatory Field</font>
                        </td>
                        <td>
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
