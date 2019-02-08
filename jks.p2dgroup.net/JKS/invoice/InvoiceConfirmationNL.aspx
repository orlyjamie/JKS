<%@ Page Language="c#" CodeFile="InvoiceConfirmationNL.aspx.cs" AutoEventWireup="false"
    Inherits="JKS.InvoiceConfirmationNL" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta charset="utf-8">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;" />
    <meta name="description" content="">
    <meta name="author" content="">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <title>P2D Network - Create Invoice (Confirmation)</title>
    <!-- Bootstrap core CSS -->
    <link href="../custom_css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom Global Style -->
    <link href="../custom_css/screen.css" rel="stylesheet">
    <link href='https://fonts.googleapis.com/css?family=Lato:400,300italic,300,100italic,100,400italic,700,700italic,900,900italic'
        rel='stylesheet' type='text/css'>
    <!-- Custom Font Icon Style -->
    <link href="../custom_css/font-awesome.css" rel="stylesheet">
    <!-- Custom Responsive Style -->
    <link href="../custom_css/responsive.css" rel="stylesheet">
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="../js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
		// ==========================================================================================================
		function ShowHideGBPEquiValentAmountRow()
		{
			if (document.all.hdGBPEquiFlag.value == '1')
					document.getElementById("trGBPEquiAmt").style.display = "";
			else
				document.getElementById("trGBPEquiAmt").style.display = "none";
		}
		// ==========================================================================================================
		function OpenURL(iInvoiceID)
		{
			var strURL = null;
			strURL = "ecVatNL.aspx?InvoiceID=" + iInvoiceID 
			window.open(strURL);
		}
		// ==========================================================================================================
		function ShowMessage()
		{
			alert('This VAT amount has been calculated automatically by the system which rounds down to the nearest penny. However, if you use a different rounding method please change the VAT amount manually. ');
		}
		// ==========================================================================================================
		function openAuditTrail()
		{
			var strInvoiceID='';
			strInvoiceID=<%=Request.QueryString["InvoiceID"]%>;
			//window.open('../../dalkia/StockQC/InvoiceAction.aspx?InvoiceID='+strInvoiceID,'a','width=900,height=680,scrollbars=1,resizable=1');
			window.open('AuditTrail.aspx?InvoiceID='+strInvoiceID+'&DocType=INV','a','width=810,height=280,scrollbars=1,resizable=1');
		}
    </script>
</head>
<body onload="javascript:ShowHideGBPEquiValentAmountRow();">
    <form id="Form1" method="post" runat="server">
    <div class="site">
        <div class="container mainWrapper nopadding">
            <div class="white_bg mainWrapper">
                <!------------------------------ START: Header ------------------------------>
                <header id="header">
                    <div class="container">
                    <!-------------------- START: Top Section -------------------->
                  	<div class="row h_top">
                        <div class="col-md-6 h_logo"><a href="javascript:void(0)" target="_self" title="P2D PAPER 2 DATA"><img src="../images/JKS_logo.png" alt="JKS" width="110px" /></a></div>
                        
                    </div>
                    <!-------------------- END: Top Section -------------------->
                    </div>
                </header>
                <!------------------------------ END: Header ------------------------------>
                <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                <div class="login_bg">
                    <div class="current_comp">
                        <div class="form-horizontal form_section">
                            <div class="row">
                                <div class="col-xs-12 col-sm-6">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader">Step 4 of 4 : Confirm Invoice Transmission</asp:Label>
                                        <div class="form-group form-group2">
                                            <asp:Label ID="lblRefernce" runat="server" CssClass="col-xs-12 col-sm-5 control-label label_text">
                                            Invoice No. : </asp:Label>
                                            <div class="col-xs-12 col-sm-7">
                                                <div class="row">
                                                    <asp:Label ID="lblInvNo" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Supplier</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <div class="row">
                                                    <asp:Label ID="lblSupplier" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                VAT Reg No.</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <div class="row">
                                                    <asp:Label ID="lblVATRegNo" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Invoice Date</label>
                                            <div class="row">
                                                <asp:Label ID="lblInvoiceDate" runat="server" CssClass="col-lg-5 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Payment Terms</label>
                                            <div class="row">
                                                <asp:Label ID="lblPaymentTerms" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Payment Due Date</label>
                                            <div class="row">
                                                <asp:Label ID="lblPaymentDueDAte" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Tax Point Date</label>
                                            <div class="row">
                                                <asp:Label ID="lblTaxPointDate" runat="server" CssClass="col-lg-5 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Overall Discount %</label>
                                            <div class="row">
                                                <asp:Label ID="lblOverAllDiscount" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Currency
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblCurrency" runat="server" CssClass="col-lg-5 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Payment Date
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblpaymentdate" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Payment Method
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblPaymentMethod" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Discount Given
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblDiscount" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <span class="col-lg-5 control-label label_text GridCaption">Delivery Note No.</span>
                                            <div class="row">
                                                <asp:Label ID="lblDespatchNoteNo" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Delivery Date
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblDespatchDate" runat="server" CssClass="col-lg-5 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Supplier Address
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblSupplierAddress" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Delivery Address
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblDeliveryAddress" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Invoice Address
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblInvoiceAddress" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Customer Activity Code
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblActivityCode" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <font class="GridCaption" size="2">Customer Account Category</font>
                                            <div class="row">
                                                <asp:Label ID="lblAccountCat" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Invoice Name
                                            </label>
                                            <div class="row">
                                                <asp:Label ID="lblInvoiceName" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                &nbsp;</label>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Buyer</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Label ID="lblBuyer" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Settlement Discount %</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Label ID="lblTermsDiscount" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Settlement Discount Days</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Label ID="lblSettlementDays" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                2nd Settlement Discount %</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Label ID="lblSecondSettlementDiscount" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                2nd Settlement Discount Days</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Label ID="lblSecondSettlementDays" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Customer Account No.</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Label ID="lblCustomerAccNo" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Customer Contact Name</label>
                                            <div class="row">
                                                <asp:Label ID="lblCustomerContactName" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text GridCaption">
                                                Status</label>
                                            <div class="row">
                                                <asp:Label ID="lblStatus" runat="server" CssClass="col-lg-5 control-label label_text"
                                                    Font-Bold="True">lblStatus</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container" style="padding: 0 !important;">
                    <div class="row">
                        <div class="col-lg-12">
                            <% if (strInvoiceDocument != "")
                               { %>
                            <div class="form-group form-group2  GridCaption">
                                <label class="col-lg-5 control-label label_text">
                                    Invoice Document</label>
                                <div class="row">
                                    <a class="NormalBody" href="<%=strInvoiceDocumentDownloadPath%><%=strInvoiceDocument%>"
                                        target="_blank">Download Scanned Image </a>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <% } %>
                            <%--<div class="form-group form-group2  GridCaption">
                                <a onclick="javascript:OpenURL('<%=invoiceID%>')" href="#">EC VAT Details</a>
                            </div>--%>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <!-- Main Content Panel Starts-->
                                        <table id="Table1" cellspacing="1" cellpadding="1" border="0" style="width: 100%">
                                            <tr>
                                                <td class="GridCaption" colspan="4">
                                                    Invoice Details
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:DataGrid ID="grdInvoiceDetails" runat="server" Width="100%" CellPadding="0"
                                                        CellSpacing="0" GridLines="None" AutoGenerateColumns="False" CssClass="listingArea">
                                                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                                        <ItemStyle BackColor="White"></ItemStyle>
                                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="PurOrderNo" ReadOnly="True" HeaderText="PO No."></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PurOrderDate" HeaderText="PO Date" DataFormatString="{0:dd-MM-yyyy}">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PurOrderLineNo" ReadOnly="True" HeaderText="PO Line No.">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="New_DespatchDate" HeaderText="Despatch Date" DataFormatString="{0:dd-MM-yyyy}">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="New_DespatchNoteNumber" HeaderText="Despatch Note No">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="BuyersProdCode" HeaderText="Buyers Prod Code"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SuppliersProdCode" HeaderText="Suppliers Prod Code">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Description" ReadOnly="True" HeaderText="Description">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="New_Type" ReadOnly="True" HeaderText="Item Type"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="New_Definable1" ReadOnly="True" HeaderText="Color"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Rate" HeaderText="Price" DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Quantity" ReadOnly="True" HeaderText="Quantity" DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="UOM" ReadOnly="True" HeaderText="UOM"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Discount" ReadOnly="True" HeaderText="Disc %" DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="New_DiscountPercent2" ReadOnly="True" HeaderText="2nd Disc%"
                                                                DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="New_NettValue" ReadOnly="True" HeaderText="Net Value"
                                                                DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="VATRate" ReadOnly="True" HeaderText="VAT Rate" DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="VATAmt" HeaderText="VAT" DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="TotalAmt" ReadOnly="True" HeaderText="Gross Value" DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPurOrederNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'>
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                                        </PagerStyle>
                                                    </asp:DataGrid><input id="hdGBPEquiFlag" type="hidden" value="0" name="hdGBPEquiFlag"
                                                        runat="server">&nbsp;<input id="hdSaveFlag" type="hidden" value="0" name="hdSaveFlag"
                                                            runat="server">&nbsp;<input id="hdHideBack" type="hidden" value="0" runat="server"
                                                                name="hdHideBack">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="4">
                                                    <table width="100%">
                                                        <tr valign="top" align="left">
                                                            <td>
                                                                <div class="col-md-6">
                                                                    <div class="col-md-12">
                                                                        <div class="form-group form-group2 widht_floart">
                                                                            <div class="col-lg-12">
                                                                                <div class="row">
                                                                                    <asp:Button ID="btnGeneratePDF" TabIndex="13" runat="server" Text="Generate PDF"
                                                                                        CssClass="sub_down btn-primary btn-group-justified center_alin" Style="width: 100% !important;">
                                                                                    </asp:Button>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="clearfix">
                                                                        </div>
                                                                        <div class="form-group form-group2 widht_floart">
                                                                            <div class="col-lg-12">
                                                                                <div class="row">
                                                                                    <asp:Button ID="btnEdit" TabIndex="13" runat="server" Text="Edit" CssClass="sub_down btn-primary btn-group-justified center_alin"
                                                                                        Visible="False" Style="width: 100% !important;"></asp:Button>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group form-group2 widht_floart">
                                                                            <div class="col-lg-12">
                                                                                <div class="row" style="text-align: center; margin-bottom: 20px;">
                                                                                    <a href="#" onclick="openAuditTrail();" style="color: #0081c5; font-size: 11px">Audit
                                                                                        Trail</a>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <table cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td class="NormalBody">
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="sp_Back"><a onclick="javascript:history.back();" href="#">
                                                                                <img src="../../images/back_2.gif" border="0">
                                                                            </a></span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnConfirmInvoice" TabIndex="13" runat="server" Width="139px" CssClass="ConfirmInvoice_2"
                                                                                Height="23px" BorderStyle="None" BorderWidth="0px"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="right">
                                                                <table width="300" style="margin-right: 10px;">
                                                                    <tr>
                                                                        <td class="GridCaption" style="width: 170px; height: 16px" align="right">
                                                                            Net Total
                                                                        </td>
                                                                        <td style="height: 16px" align="right">
                                                                            <asp:Label ID="lblNetTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="GridCaption" style="width: 170px; height: 15px" align="right">
                                                                            VAT
                                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:ShowMessage()"
                                                                                ImageUrl="../../images/i.jpg"></asp:HyperLink>
                                                                        </td>
                                                                        <td style="height: 15px" align="right">
                                                                            <asp:Label ID="lblVAT" runat="server" CssClass="NormalBody" Visible="False"></asp:Label>
                                                                            <asp:TextBox ID="txtVATAmt" runat="server" CssClass="MyInput" Width="67" Height="19"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="GridCaption" style="width: 170px" align="right">
                                                                            Total
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trGBPEquiAmt" style="display: none">
                                                                        <td class="GridCaption" style="width: 170px" align="right">
                                                                            Vat&nbsp;in GBP&nbsp;
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblGBPEquiAmt" runat="server" CssClass="NormalBody"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trInputSterlingEquiAmt" runat="server">
                                                                        <td class="GridCaption" style="width: 170px" align="right">
                                                                            Sterling equivalent VAT <font color="red">*</font>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:TextBox ID="txtSterlingEquivalent" runat="server" Width="67" Height="19" Visible="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- Main Content Panel Ends-->
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script language="javascript">
        if (document.getElementById("hdHideBack").value == '1') {
            document.getElementById("sp_Back").style.display = "none";
        }
        if (document.getElementById("hdSaveFlag").value == '1') {
            alert('Invoice sent successfully.');
            window.location.href = 'InvoiceConfirmation.aspx?hd=1';
        } 
    </script>
    </form>
    <%--<script type="text/javascript">
        window.onbeforeunload = RefreshParent;
        function RefreshParent() {
            alert("Checking");
            if (window.opener != null && !window.opener.closed) {
                window.opener.location.reload();
            }
        }
     
    </script>--%>
</body>
</html>
