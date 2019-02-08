<%--<%@ Page Language="c#" CodeBehind="InvoiceDetailAdd_CN.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.creditnotes.InvoiceDetailAdd_CN" %>--%>
    <%@ Page Language="c#" codefile="InvoiceDetailAdd_CN.aspx.cs" AutoEventWireup="false"
    Inherits="JKS.InvoiceDetailAdd_CN" %>

<%@ Register TagPrefix="uc1" TagName="banner" Src="~/Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUser" Src="~/Utilities/menuUser.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Create Credit Notes (Details)</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        // ========================================================================================================
        function trim(s) {
            while (s.substring(0, 1) == ' ') {
                s = s.substring(1, s.length);
            }
            while (s.substring(s.length - 1, s.length) == ' ') {
                s = s.substring(0, s.length - 1);
            }
            return s;
        }
        // ==========================================================================================================
        function fn_Continue() {
            if (document.getElementById("hdContinueFlag").value == '1') {
                if (document.getElementById("hdContinueFlag").value == '2') {
                    return (true);
                }
                else {
                    if (trim(document.getElementById("txtQuantity").value) != '' || trim(document.getElementById("txtRate").value) != '' || trim(document.getElementById("txtDescription").value) != '') {
                        document.getElementById("sp_ContinueMessage").innerHTML = 'You have entered data for a new line. Please submit the line before continuing.';
                        document.getElementById("hdContinueFlag").value = "2";
                        return (false);
                    }
                    else {
                        return (true);
                    }
                }
            }
        }
        // ========================================================================================================
        function fnValidate() {
            var bSaveFlag = null;
            bSaveFlag = false;

            document.getElementById("sp_ContinueMessage").innerHTML = '';

            if (trim(document.all.txtQuantity.value) != '') {
                if (isNaN(document.all.txtQuantity.value)) {
                    document.getElementById("sp_Quantity").innerHTML = "Please enter a numeric value for quantity.";
                    return (false);
                }
                else {
                    bSaveFlag = true;
                }
            }
            else {
                document.getElementById("sp_Quantity").innerHTML = "Please enter quantity.";
                bSaveFlag = false;
            }

            if (trim(document.all.txtRate.value) != '') {
                if (isNaN(document.all.txtRate.value)) {
                    document.getElementById("sp_NetPriceEach").innerHTML = "Please enter a numeric value for rate.";
                    return (false);
                }
                else {
                    bSaveFlag = true;
                }
            }
            else {
                document.getElementById("sp_NetPriceEach").innerHTML = "Please enter rate.";
                bSaveFlag = false;
            }

            if (trim(document.all.txtDiscountPercent.value) != '') {
                if (isNaN(document.all.txtDiscountPercent.value)) {
                    document.getElementById("sp_DiscountInNumeric").innerHTML = "Please enter a numeric value for discount.";
                    return (false);
                }
                else {
                    bSaveFlag = true;
                }
            }
            if (trim(document.all.txtSecondDiscountPercent.value) != '') {
                if (isNaN(document.all.txtSecondDiscountPercent.value)) {
                    document.getElementById("sp_SecondDiscountInNumeric").innerHTML = "Please enter a numeric value for discount.";
                    return (false);
                }
                else {
                    bSaveFlag = true;
                }
            }
            if (trim(document.all.tbCommodityCode.value) != '') {
                if (isNaN(document.all.tbCommodityCode.value)) {
                    document.getElementById("sp_CommodityCode").innerHTML = "Please enter a numeric value for comodity code.";
                    return (false);
                }
                else if (trim(document.all.tbCommodityCode.value).length < 8) {
                    document.getElementById("sp_CommodityCode").innerHTML = "Please enter 8 digit numeric value for comodity code.";
                    return (false);
                }
                else {
                    bSaveFlag = true;
                }
            }

            if (trim(document.all.tbNetMassInKilos.value) != '') {
                if (isNaN(document.all.tbNetMassInKilos.value)) {
                    document.getElementById("sp_NetMassInKilos").innerHTML = "Please enter a numeric value for net mass.";
                    return (false);
                }
                else {
                    bSaveFlag = true;
                }
            }
            if (trim(document.all.txtDescription.value) == '') {
                document.getElementById("sp_Description").innerHTML = "Please enter description.";
                bSaveFlag = false;
            }
            //Anupam 06.07.2006 
            if (document.all.ddlItemType.value != 'Product/Service' && document.all.ddlItemType.value != 'Delivery Charge') {
                document.getElementById("sp_ItemType").innerHTML = "Please select an item type.";
                bSaveFlag = false;
            }

            /*******/
            if (trim(document.getElementById('hdnType').value) == 1) {
                if (document.getElementById('ddlItemType').value == 'Product/Service') {

                    if (trim(document.getElementById('txtBuyersProdCode').value) == '') {
                        document.getElementById("sp_ProductCode").innerHTML = "Please enter buyer's product no.";
                        document.getElementById("BuyersProdCode").innerHTML = "&nbsp;<FONT color=#ff3333>*</FONT>";
                        bSaveFlag = false;
                    }
                    else {
                        document.getElementById("sp_ProductCode").innerHTML = "";
                        document.getElementById("BuyersProdCode").innerHTML = "";

                        //bSaveFlag = true;
                    }


                    if (trim(document.getElementById('txtPurOrderNo').value) == '') {
                        document.getElementById("sp_PONumber").innerHTML = "Please enter purchase order no.";
                        document.getElementById("PurOrderNo").innerHTML = "&nbsp;<FONT color=#ff3333>*</FONT>";
                        bSaveFlag = false;
                    }
                    else {
                        document.getElementById("sp_PONumber").innerHTML = "";
                        document.getElementById("PurOrderNo").innerHTML = "";
                        //bSaveFlag = true;
                    }
                }
            }

            /*******/

            if (bSaveFlag) {
                return (bSaveFlag);
            }
            else {
                return (false);
            }
        }
        // ========================================================================================================
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel4" runat="server" Width="100%" CssClass="Banner">
    </asp:Panel>
    <table style="width: 100%">
        <tbody>
            <tr>
                <td valign="top">
                    <!-- Main Content Panel Starts-->
                    <table id="Table1" style="width: 100%" cellspacing="1" cellpadding="1" border="0">
                        <tbody>
                            <tr>
                                <td class="PageHeader" style="padding-left: 4px; width: 575px" colspan="3">
                                    Step&nbsp;3 of 4 :&nbsp;Enter Credit Notes Details
                                </td>
                                <td class="PageHeader" colspan="2">
                                    <strong>Enter EC Vat Details</strong>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" width="167" height="28">
                                    Associated Invoice No.
                                </td>
                                <td class="NormalBody" style="width: 238px" width="254">
                                    <asp:Label ID="lblInvoiceNo" runat="server">Label</asp:Label>
                                </td>
                                <td class="NormalBody" width="152">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td class="NormalBody" style="height: 14px" width="158">
                                </td>
                                <td class="NormalBody" width="144">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Credit Notes Line No.
                                </td>
                                <td class="NormalBody" style="width: 238px; height: 21px">
                                    <asp:Label ID="lblSerialNo" runat="server">Label</asp:Label>
                                </td>
                                <td class="NormalBody">
                                    <span id="sp_ContinueMessage" style="color: red"></span>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                                <td class="NormalBody">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Quantity <font color="red">*</font>
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtQuantity" TabIndex="1" runat="server" Width="216px" CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <font color="red"><span id="sp_Quantity"></span></font>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    Mode of Transport
                                </td>
                                <td class="NormalBody">
                                    <asp:DropDownList ID="ddlModeOfTransport" runat="server" Width="136px" CssClass="MyInput">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="0">0</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="7">7</asp:ListItem>
                                        <asp:ListItem Value="8">8</asp:ListItem>
                                        <asp:ListItem Value="9">9</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Net Price Each<font color="red"> *</font>
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtRate" TabIndex="2" runat="server" Width="216px" CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <font color="red"><span id="sp_NetPriceEach"></span></font>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    Nature of Transaction
                                </td>
                                <td class="NormalBody">
                                    <asp:DropDownList ID="ddlNatureOfTransaction" runat="server" Width="136px" CssClass="MyInput">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="16">16</asp:ListItem>
                                        <asp:ListItem Value="17">17</asp:ListItem>
                                        <asp:ListItem Value="18">18</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="30">30</asp:ListItem>
                                        <asp:ListItem Value="37">37</asp:ListItem>
                                        <asp:ListItem Value="38">38</asp:ListItem>
                                        <asp:ListItem Value="40">40</asp:ListItem>
                                        <asp:ListItem Value="50">50</asp:ListItem>
                                        <asp:ListItem Value="60">60</asp:ListItem>
                                        <asp:ListItem Value="70">70</asp:ListItem>
                                        <asp:ListItem Value="77">77</asp:ListItem>
                                        <asp:ListItem Value="78">78</asp:ListItem>
                                        <asp:ListItem Value="80">80</asp:ListItem>
                                        <asp:ListItem Value="87">87</asp:ListItem>
                                        <asp:ListItem Value="88">88</asp:ListItem>
                                        <asp:ListItem Value="90">90</asp:ListItem>
                                        <asp:ListItem Value="97">97</asp:ListItem>
                                        <asp:ListItem Value="98">98</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Description<font color="red"> *</font>
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtDescription" TabIndex="3" runat="server" Width="216px" CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <font color="red"><span id="sp_Description"></span></font>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    Delivery Terms
                                </td>
                                <td class="NormalBody" style="height: 11px">
                                    <asp:DropDownList ID="ddlDeliveryTerms" runat="server" Width="136px" CssClass="MyInput">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="EXW">EXW</asp:ListItem>
                                        <asp:ListItem Value="FCA">FCA</asp:ListItem>
                                        <asp:ListItem Value="FAS">FAS</asp:ListItem>
                                        <asp:ListItem Value="FOB">FOB</asp:ListItem>
                                        <asp:ListItem Value="CFR">CFR</asp:ListItem>
                                        <asp:ListItem Value="CIF">CIF</asp:ListItem>
                                        <asp:ListItem Value="CPT">CPT</asp:ListItem>
                                        <asp:ListItem Value="CIP">CIP</asp:ListItem>
                                        <asp:ListItem Value="DAF">DAF</asp:ListItem>
                                        <asp:ListItem Value="DES">DES</asp:ListItem>
                                        <asp:ListItem Value="DEQ">DEQ</asp:ListItem>
                                        <asp:ListItem Value="DDU">DDU</asp:ListItem>
                                        <asp:ListItem Value="DDP">DDP</asp:ListItem>
                                        <asp:ListItem Value="XXX">XXX</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" style="width: 138px" height="28">
                                    Item Type <font color="red">*</font>
                                </td>
                                <td class="NormalBody" valign="middle">
                                    <asp:DropDownList ID="ddlItemType" TabIndex="33" runat="server" Width="216px" CssClass="MyInput">
                                        <asp:ListItem Value="Select" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="Product/Service">Product/Service</asp:ListItem>
                                        <asp:ListItem Value="Delivery Charge">Delivery Charge</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="NormalBody">
                                    <span id="sp_ItemType" style="color: red"></span>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    Country of Origin
                                </td>
                                <td class="NormalBody" style="height: 15px">
                                    <asp:DropDownList ID="ddlCountryOfOrigin" runat="server" Width="136px" CssClass="MyInput"
                                        DataValueField="VatAbbreviation" DataTextField="VatName">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Unit of Measure
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtUOM" TabIndex="5" runat="server" Width="216px" CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    Commodity Code
                                </td>
                                <td class="NormalBody">
                                    <asp:TextBox ID="tbCommodityCode" runat="server" Width="136px" CssClass="MyInput"
                                        MaxLength="8"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Discount %
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtDiscountPercent" TabIndex="6" runat="server" Width="216px" CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <font color="red"><span id="sp_DiscountInNumeric"></span></font>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    Supplementary Units
                                </td>
                                <td class="NormalBody">
                                    <asp:TextBox ID="tbSupplementaryUnits" runat="server" Width="136px" CssClass="MyInput"
                                        MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Second Discount %
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtSecondDiscountPercent" TabIndex="6" runat="server" Width="216px"
                                        CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <span id="sp_SecondDiscountInNumeric" style="color: red"></span>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    Net Mass (In Kilos)
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    <asp:TextBox ID="tbNetMassInKilos" runat="server" Width="136px" CssClass="MyInput"
                                        MaxLength="8"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" style="height: 14px" height="28">
                                    VAT Rate %
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:DropDownList ID="cboVATRate" TabIndex="4" runat="server" Width="216px" CssClass="MyInput"
                                        Height="24px">
                                    </asp:DropDownList>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" style="width: 138px; height: 2px" height="2">
                                    Color
                                </td>
                                <td class="NormalBody" valign="middle" colspan="3" style="height: 2px">
                                    <asp:DropDownList ID="ddlcolor" TabIndex="4" runat="server" CssClass="MyInput" Width="216px"
                                        Height="24px">
                                    </asp:DropDownList>
                                </td>
                                <td class="NormalBody" style="height: 2px">
                                </td>
                                <td class="NormalBody" style="width: 141px; height: 2px">
                                </td>
                                <td class="NormalBody" style="height: 2px">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Buyer's Product Code<span id="BuyersProdCode" runat="server">&nbsp;<font color="#ff3300">*</font></span>
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtBuyersProdCode" TabIndex="7" runat="server" Width="216px" CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <span id="sp_ProductCode" style="color: red"></span>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    <span id="sp_CommodityCode" style="color: red"></span>
                                </td>
                                <td class="NormalBody">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Supplier's Product Code
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtSupplierProdCode" TabIndex="8" runat="server" Width="216px" CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="NormalBody"
                                        ControlToValidate="txtSupplierProdCode" ErrorMessage="Supplier Code is required"
                                        Enabled="False"></asp:RequiredFieldValidator>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                                <td class="NormalBody">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    PO Date
                                </td>
                                <td class="NormalBody" style="width: 238px; height: 13px">
                                    <asp:DropDownList ID="cboYearPODate" runat="server" Width="64px" CssClass="MyInput"
                                        Height="19px">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="cboMonthPODate" runat="server" Width="71px" CssClass="MyInput"
                                        Height="19px">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="cboDayPODate" runat="server" Width="69px" CssClass="MyInput"
                                        Height="19">
                                    </asp:DropDownList>
                                </td>
                                <td class="NormalBody">
                                    <asp:Label ID="lblErrorPODate" runat="server" CssClass="NormalBody" ForeColor="Red"
                                        Visible="False" Enabled="False">Invalid Date</asp:Label>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                    <span id="sp_NetMassInKilos" style="color: red"></span>
                                </td>
                                <td class="NormalBody" style="height: 13px">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="NormalBody"
                                        ControlToValidate="txtDescription" ErrorMessage="Description is required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    PO Number<span id="PurOrderNo" runat="server">&nbsp;<font color="#ff3333">*</font></span>
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtPurOrderNo" TabIndex="10" runat="server" Width="216px" CssClass="MyInput"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <span id="sp_PONumber" style="color: red"></span>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                                <td class="NormalBody">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    PO Line Number
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="txtPurOrderLineNo" TabIndex="11" runat="server" Width="216px" CssClass="MyInput"
                                        MaxLength="2"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator8" runat="server" Width="152px"
                                        CssClass="NormalBody" ControlToValidate="txtPurOrderLineNo" ErrorMessage="PO Line No. is required"
                                        Enabled="False"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator3"
                                            runat="server" CssClass="NormalBody" ControlToValidate="txtPurOrderLineNo" ErrorMessage="Invalid Line No"
                                            ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                                <td class="NormalBody" style="height: 20px">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Delivery Note Number
                                </td>
                                <td class="NormalBody" style="width: 216px">
                                    <asp:TextBox ID="tbDespatchNoteNo" runat="server" Width="216px" CssClass="MyInput"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                                <td class="NormalBody">
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                                <td class="NormalBody">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" height="28">
                                    Delivery Date
                                </td>
                                <td class="NormalBody" style="width: 238px; height: 14px">
                                    <asp:DropDownList ID="cboYearDeliveryDate" runat="server" Width="64px" CssClass="MyInput"
                                        Height="19px">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="cboMonthDeliveryDate" runat="server" Width="71px" CssClass="MyInput"
                                        Height="19px">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="cboDayDeliveryDate" runat="server" Width="69px" CssClass="MyInput"
                                        Height="19">
                                    </asp:DropDownList>
                                </td>
                                <td class="NormalBody">
                                    <asp:Label ID="lblErrorDeliveryDate" runat="server" CssClass="NormalBody" ForeColor="Red"
                                        Visible="False" Enabled="False">Please select a valid date.</asp:Label>
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                                <td class="NormalBody" style="height: 14px">
                                </td>
                            </tr>
                            <tr>
                                <td height="28">
                                    <img height="1" src="../images/c.gif" width="152">
                                </td>
                                <td class="NormalBody" colspan="2">
                                    <table cellspacing="0" cellpadding="2" width="50%" border="0">
                                        <tr>
                                            <td valign="top" width="45%">
                                                <asp:Button ID="btnSubmit" TabIndex="12" runat="server" Width="85px" CssClass="ButtonCssNew"
                                                    Height="23px" BorderStyle="None" BorderWidth="0px" Text="Submit"></asp:Button>
                                            </td>
                                            <td style="padding-left: 4px" valign="top" width="44%">
                                                <a href="InvoiceOtherInfo_CN.aspx">
                                                    <img style="width: 85px; height: 23px" height="23" src="../images/back_new_2.gif"
                                                        border="0"></a>
                                            </td>
                                            <td style="padding-left: 4px" valign="top">
                                                <asp:Button ID="btnContinue" TabIndex="14" runat="server" CssClass="ButtonCssNew"
                                                    CausesValidation="False" Text="Continue" Visible="False"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                    <!--
										<asp:button id="btnBack" tabIndex="13" runat="server" CssClass="BackButton" Width="92px" Height="23px"
												BorderStyle="None" BorderWidth="0px" CausesValidation="False"></asp:button>
										-->
                                </td>
                                <td class="NormalBody">
                                    <input id="hdContinueFlag" type="hidden" value="0" name="hdContinueFlag" runat="server">
                                </td>
                                <td class="NormalBody" style="height: 12px">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" style="color: red; height: 21px">
                                    * = Mandatory Field
                                </td>
                                <td class="NormalBody" valign="middle" colspan="2">
                                    <font color="red">
                                        <input id="hdnType" type="hidden" value="0" name="hdnType" runat="server"></font>
                                </td>
                                <td>
                                    <img height="1" src="../images/c.gif" width="124">
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBody" style="color: red; height: 21px">
                                </td>
                                <td class="NormalBody" valign="middle" align="center" colspan="2">
                                    <asp:Label ID="lblBPCode" runat="server" CssClass="NormalBody" ForeColor="Red" Visible="False">Please Enter valid Buyer's Product Code or PO Number</asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br>
                </td>
            </tr>
        </tbody>
    </table>
    <table id="Table2" style="width: 100%" cellspacing="1" cellpadding="1" width="1256"
        border="0">
        <tr>
            <td style="width: 0px" width="0">
            </td>
            <td width="100%">
                <asp:DataGrid ID="grdInvoiceDetail" runat="server" Width="100%" BorderStyle="None"
                    BorderWidth="1px" AutoGenerateColumns="False" CellPadding="3" GridLines="Vertical"
                    BorderColor="#999999" BackColor="White" Font-Size="10pt" Font-Names="verdana,Tahoma,Arial">
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                    <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                    </ItemStyle>
                    <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                        CssClass="NormalBody" BackColor="#3399CC"></HeaderStyle>
                    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="Edit">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperlinkgridCol" runat="server" ImageUrl="../images/GridEdit.jpg">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Delete">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperlinkDelete" runat="server" BorderWidth="0px" BorderStyle="None"
                                    ImageUrl="../images/GridDelete.jpg">Delete</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="PurOrderNo" ReadOnly="True" HeaderText="PO No.">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PurOrderDate" HeaderText="PO Date" DataFormatString="{0:dd-MM-yyyy}">
                            <HeaderStyle Width="100px" VerticalAlign="Top"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PurOrderLineNo" ReadOnly="True" HeaderText="PO Line No.">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="New_DespatchDate" HeaderText="Despatch Date" DataFormatString="{0:dd-MM-yyyy}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="New_DespatchNoteNumber" HeaderText="Despatch Note No">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="BuyersProdCode" HeaderText="Buyers Prod Code">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SuppliersProdCode" HeaderText="Suppliers Prod Code">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Description" ReadOnly="True" HeaderText="Description">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="New_Type" ReadOnly="True" HeaderText="Item Type">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="New_Definable1" HeaderText="Color">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Rate" HeaderText="Price" DataFormatString="{0:N2}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Quantity" ReadOnly="True" HeaderText="Quantity" DataFormatString="{0:N2}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="UOM" ReadOnly="True" HeaderText="UOM">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Discount" ReadOnly="True" HeaderText="Disc%" DataFormatString="{0:N2}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="New_DiscountPercent2" ReadOnly="True" HeaderText="2nd Disc%"
                            DataFormatString="{0:N2}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="New_NettValue" ReadOnly="True" HeaderText="Net Value"
                            DataFormatString="{0:N2}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="VATRate" ReadOnly="True" HeaderText="VAT Rate" DataFormatString="{0:N2}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="VATAmt" HeaderText="VAT" DataFormatString="{0:N2}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TotalAmt" ReadOnly="True" HeaderText="Gross Value" DataFormatString="{0:N2}">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="SerialNo" HeaderText="Srl No.">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
