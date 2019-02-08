<%@ Page Language="c#" CodeBehind="CompanyEdit.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.CompanyEdit" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Company Add/Edit</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function ShowHideCompanyCodeRow() {
            if (document.getElementById("cboCompanyType").value != '2')
                document.getElementById("trCompanyCode").style.display = "";
            else
                document.getElementById("trCompanyCode").style.display = "none";
        }

        function ShowHideRow() {
            if ('<%=Session["NewLook"]%>' == '1') {
                if ('<%=ViewState["Mode"]%>' == 'ADD' || '<%=ViewState["Mode"]%>' == 'EDIT') {
                    document.getElementById("trAddress1").style.display = "";
                    document.getElementById("trAddress2").style.display = "";
                    document.getElementById("trAddress3").style.display = "";
                    document.getElementById("trCounty").style.display = "";
                    document.getElementById("trCountry").style.display = "";
                    document.getElementById("trPostCode").style.display = "";
                    document.getElementById("trPhoneNumber").style.display = "";

                    document.getElementById("trDeliveryAddress1").style.display = "";
                    document.getElementById("trDeliveryAddress2").style.display = "";
                    document.getElementById("trDeliveryAddress3").style.display = "";
                    document.getElementById("trDeliveryCounty").style.display = "";
                    document.getElementById("trDeliveryCountry").style.display = "";
                    document.getElementById("trDeliveryPostCode").style.display = "";
                    document.getElementById("trDeliveryPhoneNumber").style.display = "";

                    document.getElementById("trInvoice").style.display = "";
                    document.getElementById("trDelivery").style.display = "";
                }
                else {
                    document.getElementById("trAddress1").style.display = "none";
                    document.getElementById("trAddress2").style.display = "none";
                    document.getElementById("trAddress3").style.display = "none";
                    document.getElementById("trCounty").style.display = "none";
                    document.getElementById("trCountry").style.display = "none";
                    document.getElementById("trPostCode").style.display = "none";
                    document.getElementById("trPhoneNumber").style.display = "none";

                    document.getElementById("trDeliveryAddress1").style.display = "none";
                    document.getElementById("trDeliveryAddress2").style.display = "none";
                    document.getElementById("trDeliveryAddress3").style.display = "none";
                    document.getElementById("trDeliveryCounty").style.display = "none";
                    document.getElementById("trDeliveryCountry").style.display = "none";
                    document.getElementById("trDeliveryPostCode").style.display = "none";
                    document.getElementById("trDeliveryPhoneNumber").style.display = "none";

                    document.getElementById("trInvoice").style.display = "none";
                    document.getElementById("trDelivery").style.display = "none";
                }
            }
            else {
                document.getElementById("trAddress1").style.display = "none";
                document.getElementById("trAddress2").style.display = "none";
                document.getElementById("trAddress3").style.display = "none";
                document.getElementById("trCounty").style.display = "none";
                document.getElementById("trCountry").style.display = "none";
                document.getElementById("trPostCode").style.display = "none";
                document.getElementById("trPhoneNumber").style.display = "none";

                document.getElementById("trDeliveryAddress1").style.display = "none";
                document.getElementById("trDeliveryAddress2").style.display = "none";
                document.getElementById("trDeliveryAddress3").style.display = "none";
                document.getElementById("trDeliveryCounty").style.display = "none";
                document.getElementById("trDeliveryCountry").style.display = "none";
                document.getElementById("trDeliveryPostCode").style.display = "none";
                document.getElementById("trDeliveryPhoneNumber").style.display = "none";

                document.getElementById("trInvoice").style.display = "none";
                document.getElementById("trDelivery").style.display = "none";
            }
        }

        function SetVatAbbreviation() {
            if (document.all.cboVat.selectedIndex != 0) {
                document.all.txtVAT.value = document.all.cboVat.value;
            }
        }

        function fn_Validate() {
            if (document.getElementById("cboCompanyType").value != "2" && document.getElementById("txtCompanyCode").value == '') {
                document.getElementById("sp_CompanyCode").innerHTML = "Please enter the company code.";
                return (false);
            }
            else {
                return (true);
            }
        }
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0" onload="javascript:ShowHideRow();ShowHideCompanyCodeRow();">
    <form id="Form2" onsubmit="javascript:return fn_Validate();" method="post" runat="server">
    <asp:Panel ID="Panel2" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
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
                        <td style="width: 46px; height: 21px" width="45">
                        </td>
                        <td colspan="5" class="PageHeader">
                            <asp:Label ID="lblHeader" runat="server">Label</asp:Label>
                        </td>
                        <td width="413">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px; height: 21px" width="45">
                        </td>
                        <td align="center" colspan="5">
                            <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px" width="45">
                        </td>
                        <td class="NormalBody" style="width: 136px" width="135">
                            Company&nbsp;Name<font color="red"> *&nbsp;</font>
                        </td>
                        <td style="width: 202px" colspan="4">
                            <asp:TextBox ID="txtCompanyName" TabIndex="1" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rvCompanyName" runat="server" CssClass="NormalBody"
                                ErrorMessage="Please enter company name." ControlToValidate="txtCompanyName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trCompanyCode">
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" style="width: 136px">
                            Company Code <font color="red">*</font>
                        </td>
                        <td style="width: 202px" colspan="4">
                            <asp:TextBox ID="txtCompanyCode" TabIndex="1" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <span class="NormalBody" id="sp_CompanyCode" style="color: red"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px; height: 17px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 17px">
                            Company Type<font color="red"> *</font>
                        </td>
                        <td style="width: 202px; height: 17px" colspan="4">
                            <asp:DropDownList ID="cboCompanyType" TabIndex="1" runat="server" Width="200px" CssClass="Input"
                                Height="24px" onChange="javascript:ShowHideCompanyCodeRow();">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 17px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" style="width: 136px">
                            Member Type<font color="red"> *</font>
                        </td>
                        <td style="width: 202px" colspan="4">
                            <asp:DropDownList ID="cboMemberType" TabIndex="1" runat="server" Width="200px" CssClass="Input">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px; height: 19px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 19px">
                            Select
                        </td>
                        <td style="width: 202px; height: 19px" colspan="4">
                            <asp:DropDownList ID="cboVat" TabIndex="1" runat="server" Width="200px" CssClass="Input"
                                DataValueField="VatAbbreviation" DataTextField="VatName" onChange="javascript:SetVatAbbreviation();">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 19px">
                            <asp:RequiredFieldValidator ID="rvVAT" runat="server" CssClass="NormalBody" ErrorMessage="Please select vat option."
                                ControlToValidate="cboVat" InitialValue="0"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            VAT Reg No<font color="red"> *</font>
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="txtVAT" TabIndex="1" runat="server" Width="48px" ReadOnly="True"></asp:TextBox>&nbsp;
                            <asp:TextBox ID="txtVatNo" TabIndex="1" runat="server" Width="136px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                            <asp:RequiredFieldValidator ID="rfv_VATNo" runat="server" CssClass="NormalBody" ErrorMessage="Please enter vat reg. no."
                                ControlToValidate="txtVatNo"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Country Tax No.
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:DropDownList ID="ddlCountryTaxNo" runat="server" Width="200px">
                                <asp:ListItem Value="Select" Selected="True">Select</asp:ListItem>
                                <asp:ListItem Value="600">600</asp:ListItem>
                                <asp:ListItem Value="091">091</asp:ListItem>
                                <asp:ListItem Value="064">064</asp:ListItem>
                                <asp:ListItem Value="063">063</asp:ListItem>
                                <asp:ListItem Value="061">061</asp:ListItem>
                                <asp:ListItem Value="060">060</asp:ListItem>
                                <asp:ListItem Value="055">055</asp:ListItem>
                                <asp:ListItem Value="054">054</asp:ListItem>
                                <asp:ListItem Value="046">046</asp:ListItem>
                                <asp:ListItem Value="038">038</asp:ListItem>
                                <asp:ListItem Value="030">030</asp:ListItem>
                                <asp:ListItem Value="016">016</asp:ListItem>
                                <asp:ListItem Value="017">017</asp:ListItem>
                                <asp:ListItem Value="018">018</asp:ListItem>
                                <asp:ListItem Value="011">011</asp:ListItem>
                                <asp:ListItem Value="009">009</asp:ListItem>
                                <asp:ListItem Value="007">007</asp:ListItem>
                                <asp:ListItem Value="005">005</asp:ListItem>
                                <asp:ListItem Value="003">003</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Traders Reference
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="txtTradersReference" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Email
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="txtEmail" TabIndex="1" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator_FOR_Email" runat="server"
                                ErrorMessage="Please enter a valid email format." ControlToValidate="txtEmail"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trInvoice" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            <strong><u>INVOICE ADDRESS :</u></strong>
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trAddress1" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Address1 <font color="red">*</font>
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbAddress1" TabIndex="1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                            <asp:RequiredFieldValidator ID="rfv_FOR_Address1" runat="server" CssClass="NormalBody"
                                ErrorMessage="Please enter invoice address1." ControlToValidate="tbAddress1"
                                Enabled="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trAddress2" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Address2
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbAddress2" TabIndex="1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trAddress3" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Address3
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbAddress3" TabIndex="1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trCounty" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            County <font color="red">*</font>
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:DropDownList ID="ddlCounty" TabIndex="1" runat="server" Width="200px" CssClass="Input"
                                DataValueField="CountyID" DataTextField="County">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 21px">
                            <asp:RequiredFieldValidator ID="RFV_FOR_County" runat="server" CssClass="NormalBody"
                                ErrorMessage="Please select invoice county name." ControlToValidate="ddlCounty"
                                InitialValue="0" Enabled="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trCountry" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Country <font color="red">*</font>
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:DropDownList ID="ddlCountry" TabIndex="1" runat="server" Width="200px" CssClass="Input"
                                DataValueField="CountryID" DataTextField="Country">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 21px">
                            <asp:RequiredFieldValidator ID="RFV_FOR_Country" runat="server" CssClass="NormalBody"
                                ErrorMessage="Please select invoice country name." ControlToValidate="ddlCountry"
                                InitialValue="0" Enabled="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trPostCode" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Post Code
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbPostCode" TabIndex="1" runat="server" Width="200px" MaxLength="15"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trPhoneNumber" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Phone Number
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbPhoneNo" TabIndex="1" runat="server" Width="200px" MaxLength="15"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trDelivery" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            <strong><u>DELIVERY ADDRESS :</u></strong>
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trDeliveryAddress1" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Address1 <font color="red">*</font>
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbDeliveryAddress1" TabIndex="1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                            <asp:RequiredFieldValidator ID="rfv_FOR_DeliveryAddress1" runat="server" CssClass="NormalBody"
                                ErrorMessage="Please enter delivery address1." ControlToValidate="tbDeliveryAddress1"
                                Enabled="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trDeliveryAddress2" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Address2
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbDeliveryAddress2" TabIndex="1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trDeliveryAddress3" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Address3
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbDeliveryAddress3" TabIndex="1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trDeliveryCounty" style="display: none">
                        <td style="width: 46px; height: 14px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 14px">
                            County <font color="red">*</font>
                        </td>
                        <td style="width: 202px; height: 14px" colspan="4">
                            <asp:DropDownList ID="ddlCountyDelivery" TabIndex="1" runat="server" Width="200px"
                                CssClass="Input" DataValueField="CountyID" DataTextField="County">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 14px">
                            <asp:RequiredFieldValidator ID="rfv_FOR_DeliveryCounty" runat="server" CssClass="NormalBody"
                                ErrorMessage="Please select delivery county name." ControlToValidate="ddlCountyDelivery"
                                InitialValue="0" Enabled="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trDeliveryCountry" style="display: none">
                        <td style="width: 46px; height: 30px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 30px">
                            Country <font color="red">*</font>
                        </td>
                        <td style="width: 202px; height: 30px" colspan="4">
                            <asp:DropDownList ID="ddlCountryDelivery" TabIndex="1" runat="server" Width="200px"
                                CssClass="Input" DataValueField="CountryID" DataTextField="Country">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 30px">
                            <asp:RequiredFieldValidator ID="rfv_FOR_DeliveryCountry" runat="server" CssClass="NormalBody"
                                ErrorMessage="Please select delivery country name." ControlToValidate="ddlCountryDelivery"
                                InitialValue="0" Enabled="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trDeliveryPostCode" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Post Code
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbDeliveryPostCode" TabIndex="1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr id="trDeliveryPhoneNumber" style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                            Phone Number
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                            <asp:TextBox ID="tbDeliveryPhoneNo" TabIndex="1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 46px; height: 21px">
                        </td>
                        <td class="NormalBody" style="width: 136px; height: 21px">
                        </td>
                        <td style="width: 202px; height: 21px" colspan="4">
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td align="center" colspan="6">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td style="width: 178px" valign="top" align="right">
                        </td>
                        <td valign="top" align="right" colspan="2" style="width: 178px">
                            <asp:Button ID="btnSubmit" TabIndex="6" runat="server" Width="91px" CssClass="SubmitButton"
                                Height="23px" BorderStyle="None"></asp:Button>
                        </td>
                        <td valign="top" align="left" width="114" colspan="2" style="width: 114px">
                            <a href="CompanyBrowse.aspx">
                                <img id="imgBack" alt="" src="../../images/Back.jpg" border="0"></a><a href="CompanyBrowse.aspx"></a>
                        </td>
                        <td valign="top" align="left" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 46px">
                        </td>
                        <td class="NormalBody" colspan="6">
                            <font color="red">* = Mandatory Field</font>
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
