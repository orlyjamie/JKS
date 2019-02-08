<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActionHistoryNew.aspx.cs"
    Inherits=" JKS.ETC_History_ActionHistoryNew" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>P2D Network - History Action</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <!----LightBox------>
    <link rel="stylesheet" href="../../LightBoxScripts/style.css" />
    <script type="text/javascript" src="../../LightBoxScripts/tinybox.js"></script>
    <!----->
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <%--Added by Mainak 2018-04-06--%>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("This will change the document status to Rejected thereby allowing you to Reopen it for approval in the CURRENT folder. Are you sure you want to continue?")) {
                var StatusId = document.getElementById('statusId').value;
                if (StatusId == '29') {
                    if (confirm("WARNING: This document has already been exported. Are you SURE you want to continue?")) {
                        confirm_value.value = "Yes";
                    }
                    else {
                        //confirm_value.value = "No";
                        return false;
                    }
                }
                confirm_value.value = "Yes";
            }
            else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script language="javascript">
     <%--Added by kuntalkarar on 4thJanuary2017 --%>
    function GoToStockQC()
		{
      
			var docType1='';
			docType1='<%=Request.QueryString["DocType"]%>';

			var strInvoiceID='';
			strInvoiceID=<%=Request.QueryString["InvoiceID"]%>;
            if(docType1 == "INV")
			{	
			window.open('../../JKS/StockQC/InvoiceAction.aspx?InvoiceID='+strInvoiceID,'a','width=810,height=680,scrollbars=1,resizable=1');
            }
            else
			{				
				window.open('../../JKS/StockQC/CreditStkAction.aspx?InvoiceID='+strInvoiceID,'a','width=810,height=680,scrollbars=1,resizable=1');
          }
		}


 <%-- -------------------------------------------- --%>


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
            //window.close();
            parent.window.close();
        }

        function SubmitForm() {
            window.opener.document.forms[0].submit();
        }

        function CaptureClose() {
            document.body.style.cursor = 'wait';
            // window.opener.doRefesh();
            // opener.location.reload(true);            
        }
    </script>
    <%-- <style type="text/css">
        .box
        {
            display: none;
            width: 100%;
        }
        
        a:hover + .box, .box:hover
        {
            display: block;
            position: absolute;
            z-index: 100;
            left: -370px;
            top: -200px;
        }
    </style>--%>
    <style type="text/css">
        .box
        {
            display: none;
            width: 100%;
        }
        
        a:hover + .box, .box:hover
        {
            display: block;
            position: absolute;
            z-index: 100;
            left: -380px;
            top: 20px;
        }
    </style>


     <%--Added By KD 07.12.2018--%>
     <style type="text/css">
        body
        {
            font-family: Arial;
            
        }
        /*table
        {
            border: 1px solid #ccc;
        }
        table th
        {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
        }
        table th, table td
        {
            padding: 5px;
            border-color: #ccc;
        }*/
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border: 3px solid #0DA9D0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            text-align: center;
            padding: 5px;
        }
        .modalPopup .footer
        {
            padding: 3px;
        }
        /*.modalPopup .button
        {
            height: 23px;
            color: White;
            line-height: 23px;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
            background-color: #9F9F9F;
            border: 1px solid #5C5C5C;
        }*/
        /*added kd on 06-12-2018--%>*/
.modalPopup .button {
    color: White;
    line-height: 23px;
    text-align: center;
    font-weight: bold;
    cursor: pointer;
    background: url(../images/close.png) no-repeat;
    position: absolute;
    top: -12px;
    left: -12px;
    width: 30px;
    height: 30px;
    cursor: pointer;
    border: none;
}
  
  
  .modalPopup {
    
    width: 545px;
     height: 390px;
}      
        .modalPopup td
        {
            text-align: center;
        }
        .PopUpHeader
        {
            text-align: left !important;
        }
    </style>
</head>
<body onbeforeunload="doHourglass(); " bgcolor="#ffffff" leftmargin="0" topmargin="0"
    onload="javascript:CloseWindow();" onunload="javascript:CaptureClose();">
    <script language="javascript" src="../../ETC/Supplier/wz_tooltip.js"></script>
    <form id="Form2" style="z-index: 102; left: 0px" method="post" runat="server">
    <%--Added by Mainak 2018-04-06--%>
    <input type="hidden" id="statusId" runat="server" value="" />
    <table style="width: 100%; height: 495px;">
        <tr>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0" style="width: 100%;">
                    <tr>
                        <td class="PageHeaderAction" style="height: 21px">
                            <div class="tophead">
                                <asp:Label ID="lblConfirmation" runat="server" Visible="True" CssClass="PageHeaderAction"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <table class="tlbborder" style="width: 100%; height: 64px">
                                <tr class="NewBoldText">
                                    <td class="NormalBody">
                                        <b>Document No</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody">
                                        <b>Current Status</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCurrentStatus" runat="server" CssClass="NormalBody" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <!--<td style="width: 43px">
                                    </td>-->
                                    <td class="NormalBody">
                                        <b>Document Date</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblInvoiceDate" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody">
                                        <%--<b>Business Order</b>--%>
                                        <b>Internal Order</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBusinessUnit" runat="server" CssClass="NormalBody" Style="cursor: hand"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody">
                                        <b>Supplier Name</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSupplier" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody">
                                        <b>Department</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDepartment" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody">
                                        <b>Buyer Name</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBuyer" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody">
                                        <b>
                                            <asp:Label ID="lblCRn" runat="server" CssClass="NormalBody">Credit Note No</asp:Label>
                                        </b>
                                    </td>
                                    <td class="NormalBody">
                                        <asp:Label ID="lblcreditnoteno" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td class="NormalBody">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="NormalBody" style>
                                        <b><b>Nominal Code</b></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNominal" runat="server" CssClass="NormalBody"></asp:Label>
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
                        <td class="NormalBody" valign="top" colspan="5" height="100%">
                            <asp:Repeater runat="server" ID="grdList">
                                <HeaderTemplate>
                                    <table class="head-box" width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <thead>
                                            <tr>
                                                <th align="center" width="13%" colspan="1">
                                                    Line No.
                                                </th>
                                                <th align="center" width="29%" colspan="1">
                                                    Company
                                                </th>
                                                <th align="center" width="29%" colspan="1">
                                                    <%--Business Unit--%>
                                                    Internal Order
                                                </th>
                                                <th align="center" width="15%" colspan="1">
                                                    PO No.
                                                </th>
                                                <th align="center" width="15%" colspan="1">
                                                    Delete Lines
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                    <div style="border: 1px solid #ccc; width: 99.6%;">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table class="tablebox" width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tbody class="custom_listingArea">
                                            <tr class="tableHd rowset">
                                                <td width="10%" colspan="1" style="text-align: center;">
                                                    <asp:Label ID="lblLineNo" runat="server"></asp:Label>
                                                </td>
                                                <td width="30%" colspan="1">
                                                    <asp:DropDownList ID="ddlBuyerCompanyCode" AutoPostBack="True" CssClass="textfields-option"
                                                        runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="30%" colspan="1">
                                                    <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="textfields-option">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="15%" colspan="1">
                                                    <asp:TextBox ID="txtPoNumber" CssClass="textfields" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'
                                                        runat="server" ReadOnly="true" Enabled="false">
                                                    </asp:TextBox>
                                                </td>
                                                <td width="15%" colspan="1" align="center" style="text-align: center;">
                                                    <asp:CheckBox ID="chkBox" runat="server" Enabled="false"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr class="rowset">
                                                <td align="center" width="10%">
                                                    <span class="coding">Coding:</span>
                                                </td>
                                                <td align="left" width="60%" colspan="2">
                                                    <asp:TextBox ID="txtAutoCompleteCodingDescription" CssClass="input_Auto_Complete textfields-option"
                                                        runat="server" Width="99.5%" ReadOnly="true" Enabled="false">
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdnCodingDescriptionID" runat="server" />
                                                    <asp:HiddenField ID="hdnDepartmentCodeID" runat="server" />
                                                    <asp:HiddenField ID="hdnNominalCodeID" runat="server" />
                                                </td>
                                                <td align="center" width="15%" colspan="1" class="value" style="padding-left: 10px;">
                                                    Net:
                                                </td>
                                                <td align="center" width="15%" colspan="1">
                                                    <asp:TextBox ID="txtNetVal" CssClass="textfields-option align-right" Text='<%# GetDecimalFormattedValue(DataBinder.Eval(Container, "DataItem.NetValue")) %>'
                                                        runat="server" ReadOnly="true" Enabled="false">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <!---Addition Start on 25th March 2015--->
                                            <tr class="rowset">
                                                <td align="center" width="10%">
                                                    <span class="coding">Description:</span>
                                                </td>
                                                <td align="left" width="60%" colspan="2">
                                                    <asp:TextBox ID="txtLineDescription" CssClass="textfields-option" runat="server"
                                                        Width="99.5%">
                                                    </asp:TextBox>
                                                </td>
                                                <td align="center" width="15%" colspan="1" class="value" style="padding-left: 10px;">
                                                    VAT:
                                                </td>
                                                <td align="center" width="15%" colspan="1">
                                                    <asp:TextBox ID="txtLineVAT" CssClass="textfields-option align-right" runat="server"
                                                        ReadOnly="true" Enabled="false" Text='<%# GetDecimalFormattedValue(DataBinder.Eval(Container, "DataItem.VAT")) %>'>
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <!-- Addition End on 25th March 2015-->
                                        </tbody>
                                    </table>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <table class="tablebox colored" width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tbody class="custom_listingArea">
                                            <tr class="tableHd rowset">
                                                <td align="center" width="10%" style="text-align: center;">
                                                    <asp:Label ID="lblLineNo" runat="server"></asp:Label>
                                                </td>
                                                <td align="center" width="30%">
                                                    <asp:DropDownList ID="ddlBuyerCompanyCode" AutoPostBack="True" runat="server" CssClass="textfields-option">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center" width="30%">
                                                    <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="textfields-option">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center" width="15%">
                                                    <asp:TextBox ID="txtPoNumber" CssClass="textfields" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'
                                                        runat="server" ReadOnly="true" Enabled="false">
                                                    </asp:TextBox>
                                                </td>
                                                <td align="center" width="15%" style="text-align: center;">
                                                    <asp:CheckBox ID="chkBox" runat="server" Enabled="false"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr class="rowset">
                                                <td align="center" width="10%">
                                                    <span class="coding">Coding:</span>
                                                </td>
                                                <td align="left" width="60%" colspan="2">
                                                    <asp:TextBox ID="txtAutoCompleteCodingDescription" CssClass="input_Auto_Complete textfields-option"
                                                        runat="server" Width="99.5%" ReadOnly="true" Enabled="false">
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdnCodingDescriptionID" runat="server" />
                                                    <asp:HiddenField ID="hdnDepartmentCodeID" runat="server" />
                                                    <asp:HiddenField ID="hdnNominalCodeID" runat="server" />
                                                </td>
                                                <td align="center" width="15%" colspan="1" class="value" style="padding-left: 10px;">
                                                    Net:
                                                </td>
                                                <td align="center" width="15%" colspan="1">
                                                    <asp:TextBox ID="txtNetVal" CssClass="textfields-option align-right" Text='<%# GetDecimalFormattedValue(DataBinder.Eval(Container, "DataItem.NetValue")) %>'
                                                        runat="server" ReadOnly="true" Enabled="false">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <!---Addition Start on 25th March 2015--->
                                            <tr class="rowset">
                                                <td align="center" width="10%">
                                                    <span class="coding">Description:</span>
                                                </td>
                                                <td align="left" width="60%" colspan="2">
                                                    <asp:TextBox ID="txtLineDescription" CssClass="textfields-option" runat="server"
                                                        Width="99.5%">
                                                    </asp:TextBox>
                                                </td>
                                                <td align="center" width="15%" colspan="1" class="value" style="padding-left: 10px;">
                                                    VAT:
                                                </td>
                                                <td align="center" width="15%" colspan="1">
                                                    <asp:TextBox ID="txtLineVAT" CssClass="textfields-option align-right" runat="server"
                                                        ReadOnly="true" Enabled="false" Text='<%# GetDecimalFormattedValue(DataBinder.Eval(Container, "DataItem.VAT")) %>'>
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <!-- Addition End on 25th March 2015-->
                                        </tbody>
                                    </table>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <thead>
                                    <tr>
                                        <%-------modified by kuntal karar on 23.03.2015------------pt.49--%>
                                        <td colspan="1" style="padding: 3px 2px;" nowrap="nowrap">
                                            <%--class="bottomBdr"--%>
                                            <asp:Label ID="Label2" runat="server" CssClass="NormalBody"><b>Invoice Net:</b></asp:Label>
                                        </td>
                                        <td colspan="1" nowrap="nowrap" class="colored" style="text-align: right;">
                                            <%--class="colored bottomBdr"--%>
                                            <asp:Label ID="lblNetInvoiceTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                        </td>
                                        <td width="25%;" style="padding: 3px 2px; text-align: right;" nowrap="nowrap">
                                            <%--class="bottomBdr" --%>
                                            <asp:Label ID="Label1" runat="server" CssClass="NormalBody"><b>Total Coding Net:</b></asp:Label>
                                        </td>
                                        <td colspan="1" nowrap="nowrap" style="text-align: center;">
                                            <%--class="bottomBdr"--%>
                                            <asp:Label ID="lblNetVal" runat="server" CssClass="NormalBody"></asp:Label>
                                        </td>
                                        <td width="25%;" style="padding: 3px 55px 3px 0; text-align: right" nowrap="nowrap">
                                            <%--class="bottomBdr"--%>
                                            <asp:Label ID="lblVarianceHeading" runat="server" CssClass="NormalBody"><b>Net Variance:</b></asp:Label>
                                        </td>
                                        <td colspan="1" nowrap="nowrap" style="text-align: right; padding-right: 6px;">
                                            <%--class="bottomBdr"--%>
                                            <asp:Label ID="lblVariance" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="1" class="noBorder" style="padding: 3px 2px;" nowrap="nowrap">
                                            <asp:Label ID="lblVatHeading" runat="server" CssClass="NormalBody"><b>VAT:</b></asp:Label>
                                        </td>
                                        <td colspan="1" nowrap="nowrap" class="colored bottomBdr" style="text-align: right;">
                                            <asp:Label ID="lblVat" runat="server" CssClass="NormalBody"></asp:Label>
                                        </td>
                                        <!--Added by Mrinal on 25th March 2015---->
                                        <td width="25%;" style="padding: 3px 2px; text-align: right;" nowrap>
                                            <asp:Label ID="lblTotalCodingVAT" runat="server" CssClass="NormalBody"><b>Total Coding VAT:</b></asp:Label>
                                        </td>
                                        <td colspan="1" nowrap="nowrap" style="text-align: center;">
                                            <asp:Label ID="lblTotalCodingVATValue" runat="server" CssClass="NormalBody"></asp:Label>
                                        </td>
                                        <td width="25%;" style="padding: 3px 55px 3px 0; text-align: right" nowrap>
                                            <asp:Label ID="lblVATVarianceHeading" runat="server" CssClass="NormalBody"><b>VAT Variance:</b></asp:Label>
                                        </td>
                                        <td colspan="1" nowrap="nowrap" style="text-align: right; padding-right: 6px;">
                                            <asp:Label ID="lblVATVariance" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                        </td>
                                        <!---Addition End----->
                                    </tr>
                                    <tr>
                                        <td class="noBorder" style="padding: 3px 2px;" nowrap="nowrap">
                                            <asp:Label ID="lblTotalHeading" runat="server" CssClass="NormalBody"><b>Total:</b></asp:Label>
                                        </td>
                                        <td colspan="1" nowrap="nowrap" class="colored" style="text-align: right;">
                                            <%--class="colored bottomBdr"--%>
                                            <asp:Label ID="lblTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                        </td>
                                        <%-----------------------modificatioin ends------------------%>
                                        <td colspan="1" align="left">
                                            <asp:Label ID="lblCurrencyCode" runat="server" CssClass="NormalBody gbp"></asp:Label>
                                        </td>
                                        <td colspan="1" align="left">
                                            <asp:Label ID="lblDuplicate" runat="server" CssClass="dupval" Text="DUPLICATE" ForeColor="Red"></asp:Label>
                                        </td>
                                        <%--<td colspan="2" align="left" style="text-align: right; position: relative">
                                            <a id="aInvoiceStatusLog" href='#' runat="server" style="font-size: 11px; font-weight: 600;
                                                padding: 0 8px; color: #3399cc;" class="st-hist">Status History</a>
                                            <div class="box">
                                                <iframe id="iframeInvoiceStatusLog" runat="server" src="" width="530px" height="400px"
                                                    scrolling="No"></iframe>
                                            </div>
                                        </td>--%>
                                        <td colspan="2" align="left" style="text-align: right; position: relative">

                                        <!--Blocked By Kd 07.12.2018 -->
                                           <%-- <a id="aInvoiceStatusLog" href='#' runat="server" style="font-size: 11px; font-weight: 600;
                                                padding: 0 8px; color: #3399cc;" class="st-hist">Status History</a>--%>

                                            <!--  Added By Kd 07.12.2018 -->
                                           <asp:LinkButton ID="aStatusHistory" OnClick="Popup_Click"  runat="server" style="font-size: 11px; font-weight: 600;
                                                                    padding: 0 8px; color: #3399cc;" class="st-hist"><b>Status History</b></asp:LinkButton>



                                            <%--                                            <div class="box">
                                                <iframe id="iframeInvoiceStatusLog" runat="server" src="" style="width: 545px; height: 350px;margin-left:10px;"
                                                    scrolling="No"></iframe>
                                            </div>--%>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="True" Width="314px" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr height="10">
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" height="30px;">
                                <tr>
                                    <td style="width: 100px" align="left" colspan="1">
                                        <a id="aEditData" href='#' runat="server" style="font-size: 11px; font-weight: 600;
                                            padding: 0 8px; color: #3399cc;" target="_blank">Edit Data</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="10">
                        <td style="height: 59px" align="center">
                            <div style="border: 1px solid; width: 100%;">
                                <table class="tlbborder" style="height: 32px">
                                    <% if (TypeUser > 1)
                                       {%>
                                    <tr>
                                        <td style="height: 42px" align="right">
                                            <b class="NormalBody">Comments&nbsp; :<font color="red">*</font></b>
                                        </td>
                                        <td style="height: 42px" align="left">
                                            <asp:TextBox ID="txtComment" runat="server" Width="432px" Height="38px" TextMode="MultiLine"
                                                MaxLength="200" Style="height: 38px; border: 1px solid #ccc;"></asp:TextBox>
                                        </td>
                                        <td style="height: 42px">
                                        </td>
                                    </tr>
                                    <% }%>
                                    <tr>
                                        <td colspan="3" valign="top" style="text-align: center">
                                            &nbsp;
                                            <div style="width: 80%; margin: 0 auto;">
                                                <asp:Button ID="btnCancel" runat="server" CssClass="allbtn_ActionWindow" BorderStyle="None"
                                                    BorderWidth="0px" CausesValidation="False" Text="Cancel"></asp:Button>
                                                <asp:Button ID="btndelete" CssClass="btnDelete_ActionWindow" BorderStyle="None" Text="Delete"
                                                    CausesValidation="False" ToolTip="Delete" runat="server"></asp:Button>
                                                <%--Added by Mainak 2018-04-05--%>
                                                <% if (TypeUser == 2 || TypeUser == 3)
                                                   {%>
                                                <%--Added by Mainak 2018-03-22--%>
                                                <%--<asp:Button ID="btnReset" runat="server" CssClass="btnReset_ActionWindow" BorderStyle="None"
                                                    BorderWidth="0px" CausesValidation="False" Text="Reset" OnClick="btnReset_Click"
                                                    OnClientClick="if ( !confirm('This will change the document status to Rejected thereby allowing you to Reopen it for approval in the CURRENT folder. Are you sure you want to continue?')) return false;">
                                                </asp:Button>--%>
                                                <asp:Button ID="btnReset" runat="server" CssClass="btnReset_ActionWindow" BorderStyle="None"
                                                    BorderWidth="0px" CausesValidation="False" Text="Reset" OnClick="btnReset_Click"
                                                    OnClientClick="Confirm()"></asp:Button>
                                                <% }%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr valign="bottom">
                        <td align="center" width="20%">
                            <a href='#' onclick="GoToStockQC();" id="lnkVariance" runat="server"><b>Variance against
                                PO </b></a>
                        </td>
                    </tr>
                </table>
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>

     <%--Added by kd 07/12/2018--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <asp:LinkButton Text="" ID="lnkFake" runat="server" />
                <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake"
                    CancelControlID="btnClose" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        Details
                    </div>
                    <div class="body">
                        <table class="NormalBody">
                            <tr>
                                <td valign="top" align="right" colspan="3">
                                    <a href="#" onclick="javascript:window.close();"></a>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 1px 0;" class="PopUpHeader">
                                    Approval Path:
                                    <asp:Label ID="lblauthstring" CssClass="NormalBody" runat="server"></asp:Label>
                                </td>
                                <td style="padding: 10px 0;" class="PopUpHeader">
                                    Department:
                                    <asp:Label ID="Label3" runat="server" CssClass="NormalBody"></asp:Label>
                                </td>
                                <td style="padding: 10px 0;" class="PopUpHeader">
                                    Business Unit:
                                    <asp:Label ID="Label4" runat="server" CssClass="NormalBody"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" colspan="3">
                                    <asp:DataGrid ID="dgSalesCallDetails" runat="server" PageSize="8" AllowPaging="True"
                                        AutoGenerateColumns="False" GridLines="Vertical" CellPadding="0" CellSpacing="0"
                                        BorderWidth="1px" BorderStyle="None" OnPageIndexChanged="dgSalesCallDetails_PageIndexChanged1"
                                        Width="100%" CssClass="listingArea">
                                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                        <ItemStyle></ItemStyle>
                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="RID" HeaderText="RID">
                                                <HeaderStyle Width="120px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="CreditNoteID" HeaderText="Credit Note ID">
                                                <HeaderStyle Width="300px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="InvoiceNo" HeaderText="Credit Note No" Visible="False">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="UserName" HeaderText="User Name"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="UserCode" HeaderText="UserID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="GroupName" HeaderText="Group Name"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Status" HeaderText="Action Status"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="UserTypeID" HeaderText="UserType ID">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ActionDate" HeaderText="Action Date" ItemStyle-Width="69px">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="DocStatus" HeaderText="Doc Status"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Comments" HeaderText="Comments"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="rejectioncode" HeaderText="Rejection Code"></asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                        </PagerStyle>
                                    </asp:DataGrid>
                                    <asp:DataGrid ID="dgSalesCallDetails_INV" runat="server" PageSize="8" AllowPaging="True"
                                        AutoGenerateColumns="False" GridLines="Vertical" OnPageIndexChanged="dgSalesCallDetails_PageIndexChanged2"
                                        CellPadding="0" CellSpacing="0" BorderWidth="1px" BorderStyle="None" Width="100%"
                                        CssClass="listingArea">
                                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                        <ItemStyle></ItemStyle>
                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="RID" HeaderText="RID">
                                                <HeaderStyle Width="120px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="InvoiceID" HeaderText="InvoiceID">
                                                <HeaderStyle Width="300px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="InvoiceNo" HeaderText="Credit Note No" Visible="False">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="UserName" HeaderText="User Name"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="UserCode" HeaderText="UserID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="GroupName" HeaderText="Group Name"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Status" HeaderText="Action Status"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="UserTypeID" HeaderText="UserType ID">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ActionDate" HeaderText="Action Date" ItemStyle-Width="69px">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="DocStatus" HeaderText="Doc Status"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Comments" HeaderText="Comments"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="rejectioncode" HeaderText="Rejection Code"></asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                        </PagerStyle>
                                    </asp:DataGrid>
                                    <!-- Main Content Panel Ends-->
                                    <asp:Label ID="Label5" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <!-- Main Content Panel Ends-->
                    </div>
                    <div class="footer" align="center">
                        <asp:Button ID="btnClose" runat="server" Text="" CssClass="button" />
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    </form>
</body>
</html>
