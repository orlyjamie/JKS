<%@ Page Language="c#" CodeFile="InvoiceEdit_CN.aspx.cs" AutoEventWireup="false"
    Inherits="JKS.InvoiceEdit_CN" %>

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
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
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
    <link href="../css/jquery-ui.css" rel="stylesheet">
    <script src="../js/jquery-1.11.0.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../js/jquery-ui.js"></script>
    <script language="javascript">
        //============================== Added By Rimi On 17th July 2015=========================
        function getYScroll() {
            var yScroll;
            if (window.innerHeight && window.scrollMaxY) {
                yScroll = window.innerHeight + window.scrollMaxY;
            } else if (document.body.scrollHeight > document.body.offsetHeight) {
                yScroll = document.body.scrollHeight;
            } else {
                yScroll = document.body.offsetHeight;
            }
            return yScroll;
        }
        function scrollToBottom() {
            window.scrollTo(getYScroll(), getYScroll());
        }

        //============================== Added By Rimi On 17th July 2015=========================

        // ==========================================================================================================
        function ShowHideGBPEquiValentAmountRow() {
            if (document.all.hdGBPEquiFlag.value == '1')
                document.getElementById("trGBPEquiAmt").style.display = "";
            else
                document.getElementById("trGBPEquiAmt").style.display = "none";
        }
        // ==========================================================================================================
        function OpenURL(iInvoiceID) {
            var strURL = null;
            strURL = "ecVatNL.aspx?InvoiceID=" + iInvoiceID
            window.open(strURL);
        }
        // ==========================================================================================================
        function ShowMessage() {
            alert('This VAT amount has been calculated automatically by the system which rounds down to the nearest penny. However, if you use a different rounding method please change the VAT amount manually. ');
        }
        // ==========================================================================================================
        // =========Sourayan04-12-2008-Start========= //
        function AddConfirm() {
            //if(confirm('Are you sure you want to ADD a new line ?')==true)
            //{ 
            window.open('InvoiceDetailAdd_CN.aspx?InvoiceID=<%=invoiceID%>', 'hhh', 'left=100,top=50,width=840,height=600,resizable=1,scrollbars=1');
            //} 
        }
        function CheckDeleteAll() {
            var allCHKCount = 0;
            var CheckedCount = 0;
            for (i = 0; i < document.Form1.elements.length; i++) {
                //document.write("The field name is: " + document.Form1.elements[i].name + " and it’s value is: " + document.Form1.elements[i].value )
                if (document.Form1.elements[i].type == "checkbox") {
                    allCHKCount++;
                    var strChkName = document.Form1.elements[i].name;



                    //alert(strChkName.indexOf("c"));

                    if (document.Form1.elements[i].checked) {
                        CheckedCount++;

                    }
                }
            }
            if (CheckedCount == 0) {
                alert('Please select Checkboxes to Delete');
                return false;
            }
            if (allCHKCount == CheckedCount) {
                alert('At least one line should be present');
                return false;
            }
            if (allCHKCount > CheckedCount) {

                return true;
            }
        }
        // =========Sourayan04-12-2008-End========= //
        function CheckComment() {
            //            if (document.getElementById('txtComments').value == '') {
            //                alert('You must add comments to describe the changes made');
            //                return false;
            //            }
            //            else {
            //                return true;
            //            }
        }
    </script>
    <style>
        table#grdInvoiceDetails.listingArea td, table.listingArea tr.redcolor td
        {
            padding: 6px;
        }
    </style>
</head>
<body onload="javascript:ShowHideGBPEquiValentAmountRow();">
    <form id="Form1" method="post" runat="server">
    <script type="text/javascript">


       

    </script>
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
                                <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader">Edit Credit Note Data</asp:Label>
                                <div class="col-xs-12 col-sm-6">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2" style="display: none">
                                            <label for="inputEmail" class="col-lg-12 control-label label_text">
                                                Associated Invoice No.</label>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text ">
                                                Associated Invoice No.</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblInvoiceNo" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Credit Note No. :</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Supplier</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:DropDownList ID="ddlSupplier" runat="server" Width="200px" CssClass="NormalBody"
                                                    AutoPostBack="True" DataTextField="CompanyName" DataValueField="CompanyID" Visible="false">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtSupplier" runat="server" class="form-control inpit_select" />
                                                <asp:HiddenField ID="HdSupplierId" runat="server" Value="" />
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Buyer</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control inpit_select"
                                                    DataTextField="CompanyName" DataValueField="CompanyID" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Invoice Date</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:DropDownList ID="cboYearInvoiceDate" runat="server" Width="71px" Style="display: inline-block;"
                                                    CssClass="form-control inpit_select">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cboMonthInvoiceDate" runat="server" Width="62px" Style="display: inline-block;"
                                                    CssClass="form-control inpit_select">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cboDayInvoiceDate" runat="server" Width="62px" Style="display: inline-block;"
                                                    CssClass="form-control inpit_select">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblErrorInvoiceDate" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Tax Point Date</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:DropDownList ID="cboYearTaxPointDate" runat="server" Width="71px" Style="display: inline-block;"
                                                    CssClass="form-control inpit_select">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cboMonthTaxPointDate" runat="server" Width="62px" Style="display: inline-block;"
                                                    CssClass="form-control inpit_select">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cboDayTaxPointDate" runat="server" Width="62px" Style="display: inline-block;"
                                                    CssClass="form-control inpit_select">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblErrorTaxPointDate" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <label class="col-xs-12 col-sm-4 control-label label_text GridCaption">
                                                Supplier
                                            </label>
                                            <div class="col-xs-12 col-sm-8">
                                                <asp:Label ID="lblSupplier" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                VAT Reg No.</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <div class="row">
                                                    <asp:Label ID="lblVATRegNo" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Payment Terms</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblPaymentTerms" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Payment Due Date</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblPaymentDueDAte" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Overall Discount %</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblOverAllDiscount" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <label class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Currency</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblCurrency" runat="server" Width="188px" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                                <asp:Label ID="lblstrDelete" runat="server" Visible="False"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Currency</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:DropDownList ID="cboCurrencyType" runat="server" CssClass="form-control inpit_select">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Payment Date</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblpaymentdate" CssClass="col-lg-12 control-label label_text" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Payment Method</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblPaymentMethod" CssClass="col-lg-12 control-label label_text" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Discount Given</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblDiscount" CssClass="col-lg-12 control-label label_text" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <img style="width: 64px; height: 4px" height="4" src="../../images/blank.gif" width="64">
                                            <label for="inputEmail" class="col-xs-12 col-sm-4 control-label label_text GridCaption">
                                                Delivery Note No.</label>
                                            <div class="col-xs-12 col-sm-8">
                                                <asp:Label ID="lblDespatchNoteNo" runat="server" CssClass="NormalBody"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Supplier Address</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblSupplierAddress" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Delivery Address</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblDeliveryAddress" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Invoice Address</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblInvoiceAddress" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Customer Activity Code</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblActivityCode" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Customer Account Category</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblAccountCat" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2 ">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption ">
                                                Invoice Name</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblInvoiceName" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <% if (strInvoiceDocument != "")
                                           { %>
                                        <div class="form-group form-group2 ">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption ">
                                                Invoice Document</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <a class="NormalBody" href="<%=strInvoiceDocumentDownloadPath%><%=strInvoiceDocument%>"
                                                    target="_blank">Download Scanned Image </a>
                                            </div>
                                        </div>
                                        <% } %>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            &nbsp;
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Settlement Discount %</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblTermsDiscount" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Settlement Discount Days</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblSettlementDays" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                2nd Settlement Discount %</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblSecondSettlementDiscount" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                2nd Settlement Discount Days</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblSecondSettlementDays" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Customer Account No.</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblCustomerAccNo" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Customer Contact Name</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblCustomerContactName" runat="server" CssClass="col-lg-12 control-label label_text"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Status</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:Label ID="lblStatus" runat="server" CssClass="col-lg-12 control-label label_text"
                                                    Font-Bold="True" Style="font-weight: bold !important;">lblStatus</asp:Label>
                                            </div>
                                        </div>
                                        <%--Addeed by Mainak 2018-03-19--%>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Business Unit</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="form-control inpit_select">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text GridCaption">
                                                Department</label>
                                            <div class="col-xs-12 col-sm-7">
                                                <asp:DropDownList ID="ddldept" runat="server" CssClass="form-control inpit_select"
                                                    DataValueField="DepartmentID" DataTextField="Department">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <%--Addition ended by Mainak 2018-03-19--%>
                                        <div class="form-group form-group2" style="display: none">
                                            <label for="inputEmail" class="col-xs-12 col-sm-4 control-label label_text GridCaption">
                                                Delivery Date</label>
                                            <div class="ccol-xs-12 col-sm-8">
                                                <asp:Label ID="lblDespatchDate" runat="server" CssClass="NormalBody"></asp:Label>
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
                            <!-- Main Content Panel Starts-->
                            <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <%-- <td class="GridCaption" style="height: 12px" colspan="4">
                                        <a onclick="javascript:OpenURL('<%=invoiceID%>')" href="#">EC VAT Details</a>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td class="GridCaption" style="height: 12px" colspan="4">
                                        Invoice Details
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 16px" colspan="4">
                                        <asp:GridView ID="grdInvoiceDetails" runat="server" Width="100%" CellPadding="0"
                                            CellSpacing="0" GridLines="None" AutoGenerateColumns="False" CssClass="listingArea"
                                            DataKeyNames="CreditNoteDetailID">
                                            <SelectedRowStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedRowStyle>
                                            <AlternatingRowStyle BackColor="LightCyan"></AlternatingRowStyle>
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <RowStyle BackColor="White"></RowStyle>
                                            <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                            <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="PO No." ItemStyle-Width="123px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPurOrderNo" CssClass="form-control inpit_select classPONumber"
                                                            runat="server"></asp:TextBox><%--classPONumber Added by Mainak 2018-10-02--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PurOrderDate" HeaderText="PO Date" DataFormatString="{0:dd-MM-yyyy}"
                                                    Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="PurOrderLineNo" ReadOnly="True" HeaderText="PO Line No."
                                                    Visible="false"></asp:BoundField>
                                                <%--Added by Mainak 2018-08-09--%>
                                                <asp:TemplateField HeaderText="PO Line No." ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPOLineNo" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="New_DespatchDate" HeaderText="Despatch Date" DataFormatString="{0:dd-MM-yyyy}"
                                                    Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="New_DespatchNoteNumber" HeaderText="Despatch Note No"
                                                    Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="BuyersProdCode" HeaderText="Buyers Prod Code" Visible="false">
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Buyers Prod Code" ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBuyersProdCode" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="SuppliersProdCode" HeaderText="Suppliers Prod Code" Visible="false">
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="New_Type" ReadOnly="True" HeaderText="Item Type" Visible="false">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="New_Definable1" ReadOnly="True" HeaderText="Color" Visible="false">
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Price" ItemStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control inpit_select" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control inpit_select"
                                                            Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="UOM" ReadOnly="True" HeaderText="UOM" Visible="false">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Discount" ReadOnly="True" HeaderText="Disc %" DataFormatString="{0:N2}"
                                                    Visible="false">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="New_DiscountPercent2" ReadOnly="True" HeaderText="2nd Disc%"
                                                    Visible="false" DataFormatString="{0:N2}">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Net Value" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNetValue" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT Rate" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="dpVATRate" runat="server" CssClass="form-control inpit_select">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtVAT" runat="server" CssClass="form-control inpit_select" Style="width: 65px;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gross Value" ItemStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGrossValue" CssClass="form-control inpit_select" runat="server"
                                                            Style="width: 65px;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="30px">
                                                    <ItemStyle HorizontalAlign="Center" BorderStyle="None"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDelete" CssClass="form-control inpit_select" runat="server"
                                                            BorderStyle="None"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField Visible="False" DataField="CreditNoteDetailID" HeaderText="CreditNoteDetailID">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--<asp:GridView ID="grdInvoiceDetails" runat="server" Width="100%" CellPadding="0"
                                            CellSpacing="0" GridLines="None" AutoGenerateColumns="False" DataKeyNames="CreditNoteDetailID"
                                            CssClass="listingArea" >
                                            <SelectedRowStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedRowStyle>
                                        
                                             <AlternatingRowStyle BackColor="LightCyan"></AlternatingRowStyle>


                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />


                                           <RowStyle BackColor="White"></RowStyle>


                                            <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                            <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                            <Columns>
                                       
                                                <asp:TemplateField HeaderText="PO No." ItemStyle-Width="123px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPurOrderNo" CssClass="form-control inpit_select" Width="90px"
                                                            runat="server"></asp:TextBox>
                                                    </ItemTemplate>

<ItemStyle Width="123px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PurOrderDate" HeaderText="PO Date" DataFormatString="{0:dd-MM-yyyy}"
                                                    Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="PurOrderLineNo" ReadOnly="True" HeaderText="PO Line No."
                                                    Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="New_DespatchDate" HeaderText="Despatch Date" DataFormatString="{0:dd-MM-yyyy}"
                                                    Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="New_DespatchNoteNumber" HeaderText="Despatch Note No"
                                                    Visible="false"></asp:BoundField>


                                                <asp:BoundField DataField="BuyersProdCode" HeaderText="Buyers Prod Code" Visible="false">
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Buyers Prod Code" ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBuyersProdCode" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                    </ItemTemplate>

<ItemStyle Width="90px"></ItemStyle>
                                                </asp:TemplateField>



                                                <asp:BoundField DataField="SuppliersProdCode" HeaderText="Suppliers Prod Code" Visible="false">
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="New_Type" ReadOnly="True" HeaderText="Item Type" Visible="false">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="New_Definable1" ReadOnly="True" HeaderText="Color" Visible="false">
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Price" ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control inpit_select" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>

<ItemStyle Width="90px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control inpit_select"
                                                            Width="100%"></asp:TextBox>
                                                    </ItemTemplate>

<ItemStyle Width="70px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="UOM" ReadOnly="True" HeaderText="UOM" Visible="false">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Discount" ReadOnly="True" HeaderText="Disc %" DataFormatString="{0:N2}"
                                                    Visible="false">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="New_DiscountPercent2" ReadOnly="True" HeaderText="2nd Disc%"
                                                    DataFormatString="{0:N2}" Visible="false">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Net Value" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNetValue" runat="server" CssClass="form-control inpit_select"
                                                            Width="50px"></asp:TextBox>
                                                    </ItemTemplate>

<ItemStyle Width="80px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT Rate" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="dpVATRate" runat="server" CssClass="form-control inpit_select">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>

<ItemStyle Width="100px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtVAT" runat="server" CssClass="form-control inpit_select" Style="width: 65px;"></asp:TextBox>
                                                    </ItemTemplate>

<ItemStyle Width="100px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gross Value" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGrossValue" Style="width: 75px;" CssClass="form-control inpit_select"
                                                            runat="server"></asp:TextBox>
                                                    </ItemTemplate>

<ItemStyle Width="100px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="30px">
                                                    <ItemStyle HorizontalAlign="Center" BorderStyle="None"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDelete" CssClass="form-control inpit_select" runat="server"
                                                          BorderStyle="None"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField Visible="true" DataField="CreditNoteDetailID" HeaderText="CreditNoteDetailID">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                            
                                        </asp:GridView>--%>
                                        <input id="hdGBPEquiFlag" type="hidden" value="0" name="hdGBPEquiFlag" runat="server" />
                                        <input id="hdSaveFlag" type="hidden" value="0" name="hdSaveFlag" runat="server">
                                        <input id="hdHideBack" type="hidden" value="0" name="hdHideBack" runat="server">
                                        <asp:Label ID="lblError" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td width="50%">
                                                    <div class="col-lg-12" style="margin-top: 15px;">
                                                        <div class="form-group form-group2">
                                                            <label for="inputEmail" class="col-xs-12 col-sm-3 control-label label_text">
                                                                Comments :<font color="red">*</font></label>
                                                            <div class="col-xs-12 col-sm-8 form-group2">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtComments" runat="server" CssClass="form-control inpit_select"
                                                                        Style="height: 80px !important;" TextMode="MultiLine"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </div>
                                                </td>
                                                <td width="50%">
                                                    <table cellspacing="0" cellpadding="2" width="100%" border="0">
                                                        <tr>
                                                            <td valign="top" align="right" width="50%">
                                                                <table width="300" style="margin-right: 10px;">
                                                                    <tr>
                                                                        <td class="GridCaption" style="width: 182px; height: 16px" align="right">
                                                                            Net Total
                                                                        </td>
                                                                        <td style="height: 16px;" align="right">
                                                                            <asp:TextBox ID="txtNetTotal" runat="server" Width="67px" CssClass="form-control inpit_select"
                                                                                Height="19px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="GridCaption" style="width: 182px; height: 15px" align="right">
                                                                            VAT
                                                                            <asp:HyperLink ID="Hyperlink2" runat="server" ImageUrl="../../images/i.jpg" NavigateUrl="javascript:ShowMessage()"></asp:HyperLink>
                                                                        </td>
                                                                        <td style="height: 15px;" align="right">
                                                                            <asp:TextBox ID="txtVATAmtNew" runat="server" Width="67px" CssClass="form-control inpit_select"
                                                                                Height="19px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="GridCaption" style="width: 182px" align="right">
                                                                            Total
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:TextBox ID="txtTotAmt" runat="server" Width="67px" CssClass="form-control inpit_select"
                                                                                Height="19px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trGBPEquiAmt" style="display: none">
                                                                        <td class="GridCaption" style="width: 182px" align="right">
                                                                            Vat in GBP
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblGBPEquiAmt" runat="server" CssClass="col-lg-4 control-label label_text"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trInputSterlingEquiAmt" runat="server">
                                                                        <td class="GridCaption" style="width: 182px" align="right">
                                                                            Sterling equivalent VAT <font color="red">*</font>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:TextBox ID="txtSterlingEquivalent" runat="server" CssClass="form-control inpit_select"
                                                                                Width="67" Height="19"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label><a
                                                                    name="ss"></a><a id="qq" href="<%=iUrlJS%>" name="qq"></a>
                                                            </td>
                                                        </tr>
                                                        <%--<td width="39%">
                                                                            <span id="sp_Back"><a onclick="javascript:history.back();" href="#">
                                                                                <img src="../../images/back_2.gif" border="0"></a></span>
                                                                        </td>
                                                                        <td width="10">
                                                                            <input class="add_newline" id="inpAddLine" style="width: 139px; height: 23px" type="button"
                                                                                name="inpAddLine" runat="server">
                                                                        </td>
                                                                        <td width="169">
                                                                            <asp:Button ID="btnDelete" runat="server" CssClass="delete_lines" BorderStyle="None"
                                                                                Height="23px" Width="139px" Text=""></asp:Button>
                                                                        </td>
                                                                        <td width="169">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="39%">
                                                                        </td>
                                                                        <td width="10">
                                                                            <asp:Button ID="btnCancel" runat="server" CssClass="cancel_new" BorderStyle="None"
                                                                                Height="23px" Width="139px"></asp:Button>
                                                                        </td>
                                                                        <td width="169">
                                                                            <asp:Button ID="btnSave" runat="server" CssClass="save" Height="23px" Width="139px"
                                                                                Text=""></asp:Button>
                                                                        </td>
                                                                        <td width="169">
                                                                            <asp:Button ID="btnCalculate" runat="server" Width="139px" CssClass="for_calculate"
                                                                                Height="23px"></asp:Button>
                                                                        </td>
                                                                    </tr>--%>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <!-- Main Content Panel Ends-->
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                    <div class="clearfix">
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-1 form-group2">
                                <span id="sp_Back"><a onclick="javascript:history.back();" href="#" class="sub_down btn-primary btn-group-justified center_alin">
                                    Back </a></span>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="col-md-2 form-group2">
                                <input class="sub_down btn-primary btn-group-justified center_alin" id="inpAddLine"
                                    value="Add Line" type="button" name="inpAddLine" runat="server">
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="col-md-2 form-group2">
                                <asp:Button ID="btnDelete" runat="server" CssClass="sub_down btn-primary btn-group-justified center_alin"
                                    BorderStyle="None" Text="Delete Lines"></asp:Button>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="col-md-2 form-group2">
                                <asp:Button ID="btnCancel" runat="server" CssClass="sub_down btn-primary btn-group-justified center_alin"
                                    BorderStyle="None" Text="Cancel"></asp:Button>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="col-md-2 form-group2">
                                <asp:Button ID="btnSave" runat="server" CssClass="sub_down btn-primary btn-group-justified center_alin"
                                    Text="Save"></asp:Button>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="col-md-2 form-group2">
                                <asp:Button ID="btnCalculate" runat="server" CssClass="sub_down btn-primary btn-group-justified center_alin"
                                    Text="Calculate"></asp:Button>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="col-md-1 form-group2">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../js/jquery-1.11.0.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../js/jquery-ui.js"></script>
    <script language="javascript">
			if(document.getElementById("hdHideBack").value == '1')
			{
				document.getElementById("sp_Back").style.display = "none";
			}
			if(document.getElementById("hdSaveFlag").value == '1' )
			{
				alert('Invoice sent successfully.');
				window.location.href='InvoiceConfirmation.aspx?hd=1';
			} 
			
			if(<%=iPassToJS%>==1) 
			{
				//document.getElementById("qq").click();//commented by Rimi on 17thJuly2015
				scrollToBottom();// Added By Rimi on 17th July 2015
			}
    </script>
    <script type="text/javascript">
        $(function () {
            var ID;

            $("#txtSupplier").autocomplete({
                source: function (request, response) {

                    // var varCompanyID = e.options[e.selectedIndex].value;
                    var varCompanyID = document.getElementById("ddlCompany").value;
                    var usrString = document.getElementById('txtSupplier').value;

                    ID = this.element.attr("id");
                    var param = { CompanyID: varCompanyID, UserString: $('#txtSupplier').val() };
                    $.ajax({
                        url: "InvoiceEdit_CN.aspx/GetSupplier",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {

                            var IsValidSource = JSON.stringify(data.d);

                            if (IsValidSource == 'null') {
                                $(".ui-autocomplete").empty();
                                $(".ui-autocomplete").hide();
                            }

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('#')[1],
                                    CompanyID: item.split('#')[0]
                                }
                            }))
                        },

                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message)
                            // console.log("Ajax Error!");  
                        }
                    });
                },
                select: function (e, i) {
                    document.getElementById('<%=HdSupplierId.ClientID%>').value = i.item.CompanyID;

                },
                change: function (event, ui) {
                    if (!ui.item) {
                        document.getElementById('<%=HdSupplierId.ClientID%>').value = "";
                        document.getElementById('<%=txtSupplier.ClientID%>').value = "";
                    }
                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });

        //Mainak, AutoComplete for PO Number on 2018-10-02
        $(".classPONumber").each(function () {
            //alert("ABCD");
            var thisElement = $(this);
            $(this).autocomplete({
                source: function (request, response) {
                    //alert('HAHAHHAHAHHA');
                    var buyerID = $('#<%=ddlCompany.ClientID%>').children("option").filter(":selected").val();
                    var supplierID = $('#HdSupplierId').val();
                    var poNoPartial = thisElement.val();
                    //var v =buyerID + "()" + supplierID + "()" + poNoPartial;
                    //alert(v);
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "InvoiceEdit_CN.aspx/ListPONumber",
                        data: '{"buyerID":"' + buyerID + '","supplierID":"' + supplierID + '","poNoPartial":"' + poNoPartial + '"}',
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    //label: item
                                    label: item.split('^')[0],
                                    val: item.split('^')[1]
                                }
                            }))

                        },
                        error: function (result) {
                        }
                    });
                },
                select: function (e, i) {
                },
                open: function () {
                    var $txt = thisElement;
                    var w = $txt.width() + 15;
                    var t = $txt.offset().top + $txt.height() + 4;
                    var l = $txt.offset().left;
                    var h = 150;

                    $txt.autocomplete("widget").css("cssText", "height: " + h + "px !important; left: " + l + "px !important; top: " + t + "px !important; width: " + w + "px !important;");
                },
                minLength: 1
            });               
    </script>
    <%--Added by Mainak 2018-10-08--%>
    <style type="text/css">
        .sub_down
        {
            /*width:50% !important;
	        padding-left:15px;
	        padding-right:15px;
	        float:right !important;
	        font-weight:bold !important;
            color:#fff !important;
	
	        min-width:150px;*/
            background-color: #428bca;
            border: 0 none;
            color: #fff !important;
            cursor: pointer;
            float: right;
            font-family: 'franklin_gothic_medium_condRg';
            font-size: 16px;
            font-weight: 400; /*margin-right: 20px;*/
            padding: 6px 30px;
            text-transform: uppercase;
            width: 130px;
            text-decoration: none !important;
            text-align: center !important;
            padding-left: 10px;
            padding-right: 10px;
            margin-right: 16px;
        }
        
        .header-thick
        {
            padding: 6px;
        }
        .footerClass
        {
            border-top: solid;
            border-top-width: 1px;
            border-top-color: inherit;
        }
        
        .cus-modal-dialog
        {
            width: 830px !important;
        }
    </style>
    <div class="modal fade" id="addModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
        aria-hidden="true" style="display: none;">
        <%--width: 80%--%>
        <div class="modal-dialog cus-modal-dialog">
            <div class="modal-content">
                <div class="callModal">
                    <h1 id="dataToDisplay">
                    </h1>
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button> style="width: 70%; margin-top: 10px"
                </div>--%>
                <div class="row" style="padding: 5px 30px;">
                    <div class="PageHeader header-thick">
                        Goods Received / Order Details<button type="button" class="close" data-dismiss="modal" style="opacity:1; color:#fff;">&times;</button>
                    </div>
                    <div style="width: 100%">
                        <table style="width: 100%">
                            <tr cellpadding="10">
                                <td>
                                    <b>PO Number</b>
                                </td>
                                <td>
                                    <b>Company</b>
                                </td>
                                <td>
                                    <b>Supplier</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPO" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCompany" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSupplier1" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <b>Date</b>
                                </td>
                                <td>
                                    <b>Currency</b>
                                </td>
                                <td>
                                    <b>Buyer</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCurrency1" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblBuyer" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                    <div class="col-xs-12 col-sm-12">
                        <div class="form-group form-group2">
                            <div class="">
                                <%--col-xs-12 col-sm-12--%>
                                <div class="row">
                                    <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="false" CssClass="listingArea center-table"
                                        DataKeyNames="GoodsRecdDetailID" ShowFooter="true" GridLines="None" Style="width: 100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Product Code" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProdCode" Text='<%# Bind("ProductCode")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesc" Text='<%# Bind("Description")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right"
                                                FooterStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQty" Text='<%# Bind("Quantity")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblQtySum" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrice" Text='<%# Bind("Rate")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right"
                                                FooterStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblValue" Text='<%# Bind("NetAmt")%>' runat="server"></asp:Label>
                                                    <asp:Label ID="lblBuyerCode" runat="server" Text='<%# Bind("BuyerCode")%>' Visible="false" />
                                                    <asp:Label ID="lblGoodsRecdDetailID" runat="server" Text='<%# Bind("GoodsRecdDetailID")%>'
                                                        Visible="false" />
                                                    <asp:Label ID="lblDeptID" runat="server" Text='<%# Bind("DepartmentID")%>' Visible="false" />
                                                    <asp:Label ID="lblNominalCodeID" runat="server" Text='<%# Bind("NominalCodeID")%>'
                                                        Visible="false" />
                                                    <asp:Label ID="lblBusinessUnitID" runat="server" Text='<%# Eval("BusinessUnitID")%>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProjectCode" runat="server" Text='<%# Eval("ProjectCode")%>' Visible="false"></asp:Label><%--Added by Mainak 2017-11-21--%>
                                                    <asp:Label ID="lblPurOrderLineNo" runat="server" Text='<%# Eval("PurOrderLineNo")%>'
                                                        Visible="false"></asp:Label><%--Added by Mainak 2018-05-31--%>
                                                    <asp:Label ID="lblVatRate" runat="server" Text='<%# Eval("TaxRate")%>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblVat" runat="server" Text='<%# Eval("VAT")%>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblGrossValue" runat="server" Text='<%# Eval("GrossValue")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblValueSum" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="10%">
                                                <HeaderTemplate>
                                                    <input type="checkbox" class="check-header" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:CheckBox ID="chkProdCode" runat="server" CssClass="prodCodeSelector"/>--%>
                                                    <input type="checkbox" class="prodCodeSelector check-all" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <input type="checkbox" class="check-header" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="row-style"></RowStyle>
                                        <HeaderStyle CssClass="tableHd head-style"></HeaderStyle>
                                        <FooterStyle CssClass="footerClass"></FooterStyle>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <div>
                                    <%--  Modified By Mainak Date 2018-10-28  --%>
                                    <asp:Button ID="btnAddModal" runat="server" Text="ADD" CssClass="sub_down" OnClick="btnAddModal_Click"
                                        OnClientClick="javascript: return getSelectedRowNo();" />
                                    <asp:Button ID="btnReplaceModal" runat="server" Text="REPLACE" CssClass="sub_down"
                                        OnClick="btnReplaceModal_Click" OnClientClick="javascript: return getSelectedRowNo();" />
                                    <%--End Modification--%>
                                </div>
                            </td>
                        </tr>
                        <%--<br />--%>
                        <tr>
                            <td>
                                <div style="color: red;">
                                    ADD will add these items onto the existing line items on the previous page
                                    <br />
                                    REPLACE will replace the existing line items on the previous page with those selected
                                    here
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
    <asp:Button ID="btnOpenModal" runat="server" OnClick="btnOpenModal_Click" UseSubmitBehavior="false" />
    <asp:HiddenField ID="hdnPurOrderNo" runat="server" />
    <asp:HiddenField ID="hdnRowNo" runat="server" />
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%=btnOpenModal.ClientID%>").style.display = "none";

        });

        $(".classPONumber").on('dblclick', function (e) {
            //alert('abbb');
            //$('#addModal').modal('show');
            $(".callModal #dataToDisplay").text($(e.target).val());
            var poNo = $(e.target).val();
            if (poNo != "") {
                $('#<%=hdnPurOrderNo.ClientID%>').val(poNo);
                $('#<%=btnOpenModal.ClientID%>').click();
                //return false;
                //e.preventDefault();
                //return false;
            }
            else {
                alert("Please select a 'PO Number' before double click");
            }
        });


        function getSelectedRowNo() {         
            var lastVal = "";
            var count = 0;
            $('.prodCodeSelector').each(function () {
                if (this.checked) {
                    lastVal += count + ',';
                }
                count++;
            });
            $("#<%=hdnRowNo.ClientID%>").val(lastVal);
        }

        $(".check-header").change(function (e) {
            var tf = $(this).is(':checked');
            $(".check-all").prop('checked', tf);
            $(".check-header").prop('checked', tf);
        });

    </script>
</body>
</html>
