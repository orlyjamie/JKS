<%@ Page Language="c#" CodeFile="history.aspx.cs" AutoEventWireup="true" Inherits="JKS.ETChistory"
    EnableEventValidation="false" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="C#" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <title>History</title>
    <!----LightBox------>
    <link rel="stylesheet" href="../../LightBoxScripts/style.css" />
    <script type="text/javascript" src="../../LightBoxScripts/tinybox.js"></script>
    <!----->
    <!-- Bootstrap core CSS -->
    <link href="../custom_css/bootstrap.min.css" rel="stylesheet" />
    <!-- Custom Global Style -->
    <link href="../custom_css/screen.css" rel="stylesheet" />
    <link href='https://fonts.googleapis.com/css?family=Lato:400,300italic,300,100italic,100,400italic,700,700italic,900,900italic'
        rel='stylesheet' type='text/css'>
    <!-- Custom Font Icon Style -->
    <link href="../custom_css/font-awesome.css" rel="stylesheet" />
    <!-- Custom Responsive Style -->
    <link href="../custom_css/responsive.css" rel="stylesheet" />
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />
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
            left: -370px;
            top: -200px;
        }
    </style>
    <script language="javascript">
        function fn_Validate() {
            /*
            if (document.all.ddlActionStatus.selectedIndex != 0 && document.all.ddlDocStatus.selectedIndex != 0)
            {
            alert ('Please select either doc status or action status.');
            return (false);
            } 
            document.body.style.cursor = 'wait';
            */
        }
        function openBrWindow(theURL, winName, features) {
            window.open(theURL, winName, features);
        }
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }

        
    </script>
    <script type="text/javascript">
        function IsOneDecimalPoint(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode;
            var parts = evt.srcElement.value.split('.');
            //blocked by soumyajit---on 12.03.2015
            //alert(parts);
            if (parts.length > 1 && charCode == 46) {
                return false;
                alert("Only One Dot Is Acceptable here!");
            }
            else {
                return true;
            }
        }
        
    </script>




    <%--Added By KD 05.12.2018--%>
     <style type="text/css">
        body
        {
            font-family: Arial;
            
        }
       /* table
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
    top: -15px;
    left: -15px;
    width: 30px;
    height: 30px;
    cursor: pointer;
    border: none;

}
  
  
  .modalPopup {
    
    width: 580px;
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
<body>
    <form id="Form1" method="post" runat="server">
    <asp:Button ID="btnProcess" runat="server" Visible="False"></asp:Button>
    <!-------------- START: Site Wrapper ------------------------------------------------->
    <div class="site">
        <div class="container mainWrapper nopadding">
            <div class="white_bg mainWrapper">
                <!------------------------------ START: Header ------------------------------>
                <header id="header">
        <div class="container">
        <!-------------------- START: Top Section -------------------->
        <div class="row h_top">
            <div class="col-md-6 h_logo"><a href="javascript:void(0)" target="_self" title="P2D PAPER 2 DATA">
             <img src="../images/JKS_logo.png" alt="JKS" width="110px" /></a></div>
                        
        </div>
        <!-------------------- END: Top Section -------------------->
        </div>
    </header>
                <!------------------------------ END: Header ------------------------------>
                <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                <asp:Button ID="btnCloseAction" runat="server" Visible="False"></asp:Button>
                <div class="login_bg">
                    <div class="current_comp">
                        <div class="form-horizontal form_section">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Company Name</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlCompany" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        AutoPostBack="True" DataValueField="CompanyID" DataTextField="CompanyName">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Supplier Name</label>
                                            <div class="col-lg-6">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlSupplier" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        DataValueField="CompanyID" DataTextField="CompanyName" Visible="false">
                                                    </asp:DropDownList>
                                                    <input type="text" id="txtSupplier" runat="server" class="form-control inpit_select" />
                                                    <asp:HiddenField ID="HdSupplierId" runat="server" Value="" />
                                                    <asp:HiddenField ID="HdSupplierName" runat="server" Value="" />
                                                    <asp:HiddenField ID="HdSupplierStatus" runat="server" Value="" />
                                                    <asp:HiddenField ID="HdDocStatus" runat="server" Value="" />
                                                </div>
                                            </div>
                                            <input id="cbSupplier" type="checkbox" runat="server" style="margin-top: 3px; float: left;
                                                margin-left: 10px;" title="wildcard search" />
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Vendor Class</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlVendorClass" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        AutoPostBack="True" DataValueField="New_VendorClass" DataTextField="New_VendorClass">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Doc Type</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlDocType" runat="server" CssClass="form-control inpit_select">
                                                        <asp:ListItem Value="" Selected="True">Select Doc Type</asp:ListItem>
                                                        <asp:ListItem Value="INV">Invoice</asp:ListItem>
                                                        <asp:ListItem Value="CRN">Credit Note</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Doc Status</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlDocStatus" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        DataValueField="StatusID" DataTextField="Status">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Action Status
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlActionStatus" TabIndex="4" runat="server" CssClass="form-control inpit_select">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                User Name</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlUsers" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        DataTextField="UserName" DataValueField="UserID">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <asp:Label ID="lblMsg" runat="server" CssClass="col-lg-12 control-label" BorderStyle="None"
                                                ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lblMessage" runat="server" BorderStyle="None" CssClass="col-lg-12 control-label"
                                                ForeColor="Red"></asp:Label>
                                        </div>
                                        <%-- Added by Mainak 2018-11-29--%>
                                        <div style="display: none;">
                                            <asp:Button ID="btnMarckPaidNoneSelected" CssClass="btnpns" runat="server" Text="Button"
                                                OnClick="btnMarckPaidNoneSelected_Click" />
                                            <asp:Button ID="btnMarckPaid" runat="server" CssClass="sub_down btn-primary btn-group-justified"
                                                OnClick="btnMarckPaid_Click" Text="btnMorethan1selected" BorderStyle="None" />
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                &nbsp;</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Button ID="btnDelete" runat="server" CssClass="sub_down btn-primary btn-group-justified"
                                                        Text="Delete" BorderStyle="None" Style="background-color: #FF0000 !important;
                                                        border-color: #d43f3a !important;" />
                                                </div>
                                            </div>
                                        </div>
                                        <%-- Ended by Mainak 2018-11-29--%>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Doc No.</label>
                                            <div class="col-lg-6">
                                                <div class="row">
                                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                    <asp:HiddenField ID="hdInvoiceNo" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdInvoiceNoTxt" runat="server" Value="" />
                                                </div>
                                            </div>
                                            <input id="cbInvoiceNo" runat="server" type="checkbox" style="margin-top: 3px; float: left;
                                                margin-left: 10px;" title="wildcard search" disabled="disabled" />
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                PO No.</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtPONo" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Business Unit</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="form-control inpit_select">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Department</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddldept" runat="server" CssClass="form-control inpit_select"
                                                        DataValueField="DepartmentID" DataTextField="Department">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Nominal</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <input type="text" id="txtNominal" runat="server" class="form-control inpit_select" />
                                                    <asp:HiddenField ID="hdNominalCodeId" runat="server" Value="" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                &nbsp;</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <%--<asp:Button ID="btnSearch" runat="server" OnClientClick="fn_ValidCheckForWildCard()" CssClass="sub_down btn-primary btn-group-justified"
                                                        Text="Search" BorderStyle="None" />--%>
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="sub_down btn-primary btn-group-justified"
                                                        Text="Search" BorderStyle="None" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Doc Date From</label>
                                            <div class="col-lg-7">
                                                <div class="row cal_img">
                                                    <input type="text" id="txtFromDate" class="form-control inpit_select" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Doc Date To</label>
                                            <div class="col-lg-7">
                                                <div class="row cal_img">
                                                    <input type="text" id="txtToDate" class="form-control inpit_select" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Net Total From</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="textRange1" runat="server" CssClass="form-control inpit_select"
                                                        onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="textRange1" ValidationExpression="^\d{1,4}(\.\d{1,2})?$" runat="server" ErrorMessage="only one dot allowed!" SetFocusOnError="True"></asp:RegularExpressionValidator>--%>
                                                    <asp:Label ID="lblMsg1" runat="server" BorderStyle="None" ForeColor="Red" Visible="false"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Net Total To</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="textRange2" runat="server" CssClass="form-control inpit_select"> </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                &nbsp;</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    &nbsp;
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                &nbsp;</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Button ID="btnDownloadAttachment" runat="server" CssClass="sub_down btn-primary btn-group-justified"
                                                        Text="Download" BorderStyle="None"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="clearfix">
                </div>
                <div class="container" style="padding: 0 !important;">
                    <div class="row">
                        <div class="col-lg-12">
                            <div id="divP2DLogo" runat="server" class="p2d_logo" visible="false">
                                <asp:Image ID="imgP2DLogo" runat="server" ImageUrl="~/JKS/images/p2d_logo.png" ImageAlign="Middle" />
                            </div>
                            <asp:DataGrid ID="grdInvCur" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                GridLines="None" CellPadding="3" PageSize="15" AllowPaging="True" CssClass="listingArea"
                                Width="100%">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle></ItemStyle>
                                <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Doc No." SortExpression="ReferenceNo">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <%#Getredirecturl(DataBinder.Eval(Container.DataItem,"NewDocType"),DataBinder.Eval(Container.DataItem,"ReferenceNo"),DataBinder.Eval(Container.DataItem,"InvoiceID"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn Visible="False" HeaderText="History">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetLogURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>">
                                                <b>Invoice Log History </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Action" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceID" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceID")
    %>'></asp:Label>
                                            <asp:Label ID="lblDocType" Visible="False" runat="server" Text='<%#
    DataBinder.Eval(Container, "DataItem.NewDocType") %>'></asp:Label>
                                            <asp:Label ID="lblInvoiceDate" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceDate")
    %>'></asp:Label>
                                            <asp:Label ID="lblDocStatus" Visible="False" runat="server" Text='<%#
    DataBinder.Eval(Container, "DataItem.DocStatus") %>'></asp:Label>
                                            <asp:Label ID="lblVoucherNumber" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VoucherNumber")
    %>'></asp:Label>
                                            <asp:LinkButton ID="hpAct" runat="server" CommandName="ACT"><b>Action</b></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <%--------Added by kuntal karar on 6thJanuary2016- Visible="false"---------%>
                                    <asp:TemplateColumn Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendorClass" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"New_VendorClass")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Action">
                                        <ItemTemplate>
                                            <%--------DataItem.New_VendorClass Added by kuntal karar on 6thJanuary2017------------%>
                                            <a href='#' onclick="<%#GetURLTest(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"NewDocType"),DataBinder.Eval(Container,
    "DataItem.InvoiceDate"),DataBinder.Eval(Container, "DataItem.DocStatus"),DataBinder.Eval(Container,
    "DataItem.VoucherNumber"),DataBinder.Eval(Container,"DataItem.New_VendorClass"))%>"><b>
        Action </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status History">
                                        <ItemTemplate>
                                            <%--------Added by kuntal karar on 1stApril2015----------%>
                                            <%--<a id="aStatusHistory" runat="server" href='#'><b>Status History </b></a>--%> <%--------Blocked by Kd 05.12.2018----------%>

                                            <%--Added By Kd 05.12.2018--%>
                                            <asp:LinkButton ID="aStatusHistory" runat="server"  CommandName="Status"  CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InvoiceID") +","+ DataBinder.Eval(Container.DataItem,"NewDocType")%>'><b>Status History</b></asp:LinkButton>
                                            <%--<a href='#' onclick="<%#GetStatusURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"NewDocType"))%>">
                                                    <b>Status History </b></a>--%>
                                            <%-----------------------------------------------------------------%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="NewDocType" SortExpression="NewDocType" HeaderText="Doc Type">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ScanDate" SortExpression="ScanDate1" HeaderText="Scan Date">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="InvoiceDate" SortExpression="InvoiceDate1" HeaderText="Invoice Date">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Supplier" SortExpression="Supplier" HeaderText="Supplier">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Buyer" SortExpression="Buyer" HeaderText="Buyer"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="VendorID" SortExpression="VendorID" HeaderText="VendorID">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DocStatus" SortExpression="DocStatus" HeaderText="Doc Status">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ActionStatus" HeaderText="Action Status" Visible="false">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Attachments">
                                        <ItemStyle HorizontalAlign="Left" Wrap="True" CssClass="Width110"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Repeater ID="rptAttachment" runat="server" OnItemDataBound="rptAttachment_ItemDataBound">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttachmentImagePath" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem,"ImagePath")%>'></asp:Label>
                                                    <asp:Label ID="lblAttachmentArchiveImagePath" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem,"ArchiveImagePath")%>'></asp:Label>
                                                    <asp:Button ID="btnAttachment" runat="server" Text="Button" CssClass="repeaterItem"
                                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem,"DocumentID")%>' OnClick="btnrptAttachment_Click" />
                                                    <asp:Label ID="lblDocumentID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem,"DocumentID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Currency" HeaderText="Currency" SortExpression="Currency">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Net" HeaderText="Net" SortExpression="Net" ItemStyle-HorizontalAlign="Right">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="VAT" HeaderText="VAT" SortExpression="VAT" ItemStyle-HorizontalAlign="Right">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Total" HeaderText="Gross" SortExpression="Total" ItemStyle-HorizontalAlign="Right">
                                    </asp:BoundColumn>
                                    <%--<asp:BoundColumn Visible="False" DataField="Comment" HeaderText="Comment" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>--%>
                                    <asp:BoundColumn Visible="False" DataField="VoucherNumber" HeaderText="VoucherNumber">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn Visible="False" HeaderText="Link to Image">
                                        <ItemTemplate>
                                            <a href='#'><b>Scanned Image</b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="InvoiceDate" SortExpression="InvoiceDate"
                                        HeaderText="Invoice Date" DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>
                                    <%--<asp:BoundColumn Visible="False" DataField="DeliveryDate" SortExpression="DeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>--%>
                                    <asp:BoundColumn DataField="ReferenceNo" Visible="false"></asp:BoundColumn>
                                    <asp:TemplateColumn Visible="False" HeaderText="Pass">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetCommentURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>">
                                                <b>Show History </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Upload Docs">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hpDoc" runat="server"><b>Upload</b></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDownload" runat="server" AutoPostBack="true" OnCheckedChanged="chkDownload_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="">
                                        <HeaderStyle></HeaderStyle>
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <a href='#' onclick="<%# GetAPCommentsURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"NewDocType"),DataBinder.Eval(Container.DataItem,"ReferenceNo"),DataBinder.Eval(Container.DataItem,"DocStatus"))%>">
                                                <img id="imgComment" runat="server" border="0"></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="IsDuplicate" HeaderText="IsDuplicate" Visible="false">
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                                    Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                            <a id="aRefreshToBottom" href="Javascript:void(0);" name="aRefreshToBottom"></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>



        <%--Added by kd 5/12/2018--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div >
                <asp:LinkButton Text="" ID="lnkFake" runat="server" />
                <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake"
                    CancelControlID="btnClose" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none;">
                    <div class="header">
                        Details
                    </div>
                    <div class="body" style="overflow: auto; height: 350px;">
                <table class="NormalBody">
        <tr>
            <td style="padding: 1px 0;" class="PopUpHeader">
                Approval Path:
                <asp:Label ID="lblauthstring" CssClass="NormalBody" runat="server"></asp:Label>
            </td>
            <td style="padding: 10px 0;" class="PopUpHeader">
                Department:
                <asp:Label ID="lblDepartment" runat="server" CssClass="NormalBody"></asp:Label>
            </td>
            <td style="padding: 10px 0;" class="PopUpHeader">
                Business Unit:
                <asp:Label ID="lblBusinessUnit" runat="server" CssClass="NormalBody"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="3">
                <asp:DataGrid ID="dgSalesCallDetails" runat="server" PageSize="8" AllowPaging="True"
  AutoGenerateColumns="False" GridLines="Vertical"
                   CellPadding="0" CellSpacing="0" BorderWidth="1px" BorderStyle="None" 
                    Width="100%" OnPageIndexChanged="dgSalesCallDetails_PageIndexChanged1"   CssClass="listingArea">
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                    <ItemStyle >
                    </ItemStyle>
                    <HeaderStyle  CssClass="tableHd"></HeaderStyle>
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
  AutoGenerateColumns="False" GridLines="Vertical"
                   CellPadding="0" CellSpacing="0" BorderWidth="1px" BorderStyle="None" 
                    Width="100%" OnPageIndexChanged="dgSalesCallDetails_PageIndexChanged2"   CssClass="listingArea">
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                    <ItemStyle >
                    </ItemStyle>
                    <HeaderStyle  CssClass="tableHd"></HeaderStyle>
                    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="RID" HeaderText="RID">
                            <HeaderStyle Width="120px"></HeaderStyle>
                        </asp:BoundColumn>
                        <%--<asp:BoundColumn Visible="False" DataField="CreditNoteID" HeaderText="Credit Note ID">
                            <HeaderStyle Width="300px"></HeaderStyle>
                        </asp:BoundColumn>--%>
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
                <asp:Label ID="Label1" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
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


        <script language="javascript">		
    if(<%=iNeedRefreshToBottom%>==1) 
    {
    document.getElementById("aRefreshToBottom").focus();               
    }
        </script>
        <script src="../js/jquery-1.11.0.min.js"></script>
        <script src="../js/bootstrap.min.js"></script>
        <script src="../js/jquery-ui.js"></script>
        <script type="text/javascript">
            jQuery('#textRange1').on('keydown', function (e) {
                if (e.which >= 48 && e.which <= 57)
                    return true;
                else if (e.which == 190)
                    return true;
                else if (e.which >= 96 && e.which <= 105)
                    return true;
                else if (e.which == 110)
                    return true;
                else if (e.which == 8 || e.which == 46)
                    return true;
                else
                    return false;
            });
        </script>
        <script type="text/javascript">
            jQuery('#textRange2').on('keydown', function (e) {
                if (e.which >= 48 && e.which <= 57)
                    return true;
                else if (e.which == 190)
                    return true;
                else if (e.which >= 96 && e.which <= 105)
                    return true;
                else if (e.which == 110)
                    return true;
                else if (e.which == 8 || e.which == 46)
                    return true;
                else
                    return false;

            });

        </script>
        <script type="text/javascript">
            jQuery('#txtFromDate').on('keydown', function (e) {
                if (e.which == 8 || e.which == 46)
                    return true;
                else
                    return false;
            });
        </script>
        <script type="text/javascript">
            jQuery('#txtToDate').on('keydown', function (e) {
                if (e.which == 8 || e.which == 46)
                    return true;
                else
                    return false;
            });
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                Date.format = 'dd/mm/yyyy';
                $("#txtFromDate").datepicker({
                    dateFormat: 'dd/mm/yy',
                    numberOfMonths: 1,
                    changeMonth: true,
                    changeYear: true,
                    showOn: "button",
                    buttonImage: "../images/DatePick.png",
                    buttonImageOnly: true,
                    buttonText: "Select date",
                    onSelect: function (selected) {
                        $("#txtToDate").datepicker("option", "minDate", selected)
                    }
                });
                $("#txtToDate").datepicker({
                    dateFormat: 'dd/mm/yy',
                    numberOfMonths: 1,
                    changeMonth: true,
                    changeYear: true,
                    showOn: "button",
                    buttonImage: "../images/DatePick.png",
                    buttonImageOnly: true,
                    buttonText: "Select date",
                    onSelect: function (selected) {
                        $("#txtFromDate").datepicker("option", "maxDate", selected)
                    }
                });
            });

        </script>
        <script type="text/javascript">
            $(function () {
                var ID;

                $("#txtSupplier").autocomplete({
                    source: function (request, response) {

                        var varCompanyID = document.getElementById("ddlCompany").value;
                        var usrString = document.getElementById('txtSupplier').value;
                        var varuserId = '<%= Session["userId"]%>';
                        var varUserTypeID = '<%= Session["UserTypeID"]%>';
                        ID = this.element.attr("id");
                        var param = { CompanyID: varCompanyID, userId: varuserId, userTypeID: varUserTypeID, UserString: $('#txtSupplier').val() };
                        $.ajax({
                            url: "history.aspx/GetSupplier",
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
                        //modified on 27.02.2015 by kuntal karar-------------
                        lstSUPclicked = 1;

                        document.getElementById('<%=HdSupplierStatus.ClientID%>').value = lstSUPclicked;
                        //-------------------------------------------------
                        document.getElementById('<%=HdSupplierId.ClientID%>').value = i.item.CompanyID;
                        document.getElementById('<%=HdSupplierName.ClientID%>').value = i.item.label;
                    },
                    change: function (event, ui) {
                        var aCheckbox = document.getElementById('cbSupplier');
                        if (aCheckbox.checked) {
                            if (!ui.item) {
                                document.getElementById('<%=HdSupplierId.ClientID%>').value = "";
                            }

                        }
                        if ((document.getElementById('cbSupplier').checked == false) && (document.getElementById('<%=HdSupplierId.ClientID%>').value == "")) {
                            document.getElementById('<%=txtSupplier.ClientID%>').value = "";
                        }

                    },
                    minLength: 2 //This is the Char length of inputTextBox  
                });
            });        
        </script>
        <script type="text/javascript">
            $(function () {
                var ID;

                $("#txtInvoiceNo").autocomplete({
                    source: function (request, response) {

                        var varCompanyID = document.getElementById("ddlCompany").value;
                        var varDocType = document.getElementById("ddlDocType").value;
                        var usrString = document.getElementById('txtInvoiceNo').value;
                        ID = this.element.attr("id");

                        var param = { CompanyID: varCompanyID, DocType: varDocType, UserString: $('#txtInvoiceNo').val() };
                        $.ajax({
                            url: "history.aspx/FetchInvoiceNo",
                            data: JSON.stringify(param),
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataFilter: function (data) { return data; },
                            success: function (data) {
                                //blocked by soumyajit---on 12.03.2015
                                //alert("Data:"+Data);
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item
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
                        //modified on 27.02.2015 by kuntal karar-------------
                        listDOCclicked = 1;
                        document.getElementById('<%=HdDocStatus.ClientID%>').value = listDOCclicked;
                        //-------------------------------------------------
                        document.getElementById('<%=hdInvoiceNo.ClientID%>').value = i.item.label;
                        document.getElementById('<%=hdInvoiceNoTxt.ClientID%>').value = i.item.label;
                    },
                    change: function (event, ui) {
                        var aCheckbox = document.getElementById('cbInvoiceNo');
                        if (aCheckbox.checked) {
                            if (!ui.item) {
                                document.getElementById('<%=hdInvoiceNo.ClientID%>').value = "";
                            }

                        }
                        if ((document.getElementById('cbInvoiceNo').checked == false) && (document.getElementById('<%=hdInvoiceNo.ClientID%>').value == "")) {
                            document.getElementById('<%=txtInvoiceNo.ClientID%>').value = "";
                        }

                    },
                    minLength: 2 //This is the Char length of inputTextBox  
                });
            });        
        </script>
        <script type="text/javascript">
            $(function () {
                var ID;

                $("#txtNominal").autocomplete({
                    source: function (request, response) {

                        // var varCompanyID = e.options[e.selectedIndex].value;
                        var varCompanyID = document.getElementById("ddlCompany").value;
                        var usrString = document.getElementById('txtNominal').value;
                        ID = this.element.attr("id");
                        var param = { CompanyID: varCompanyID, UserString: $('#txtNominal').val() };
                        $.ajax({
                            url: "history.aspx/GetNominalName",
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
                                        NominalCodeID: item.split('#')[0]
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

                        document.getElementById('<%=hdNominalCodeId.ClientID%>').value = i.item.NominalCodeID;
                        //                     alert(document.getElementById('<%=hdNominalCodeId.ClientID%>').value);

                    },
                    change: function (event, ui) {
                        if (!ui.item) {
                            document.getElementById('<%=hdNominalCodeId.ClientID%>').value = "";
                            document.getElementById('<%=txtNominal.ClientID%>').value = "";
                        }
                    },
                    minLength: 2 //This is the Char length of inputTextBox  
                });
            });        
        </script>
        <script type="text/javascript">
            SelectTab("History");
        </script>
        <%--added by Koushik Das as on 27-June-2017 for setting page index--%>
        <script type="text/javascript">
            $(document).ready(function (e) {
                var $tbl = $(this).find("#<%=grdInvCur.ClientID%>");
                //alert($tbl.html());
                var $tr = $tbl.find("tr").last();
                //alert($tr.html());
                //$tr.find("td").find("a").attr("href", "");
                $tr.find("td").find("a").click(function (e1) {
                    var val = parseInt($(this).html()) - 1;

                    if ($(this).html() == '...') {
                        var x = $tr.find("td").find("a").last().index();
                        val = parseInt($tr.find("td").find("a").eq(x - 1).html());
                    }

                    //alert(val);
                    $.ajax({
                        type: 'post',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        url: 'history.aspx/SetPageIndex',
                        data: '{"val":"' + val + '"}'
                    });
                });

                //Added by Mainak 2018-11-29
                $('#btnDelete').click(function (e) {
                    var $grdInvCur = $("#<%=grdInvCur.ClientID %>");
                    var chkcount = 0;
                    chkcount = $grdInvCur.find("input[type='checkbox']:checked").length;

                    if (chkcount > 0) {
                        var conditoinReturn = window.confirm("All ticked lines will be changed to Delete/Archive status. Do you want to continue");
                        if (conditoinReturn) {
                            document.getElementById('btnMarckPaid').click(); return false;
                        }
                        else {
                            return false;
                        }

                    }
                    else {
                        var BlankConfirmationReturn = window.confirm("No lines have been ticked. Proceeding will therefore change ALL invoices in your search to Delete/Archive status. Do you want to continue?");

                        if (BlankConfirmationReturn) {
                            document.getElementById('btnMarckPaidNoneSelected').click(); return false;
                        }
                        else {
                            return false;
                        }
                    }
                });
            });

            
        </script>
        <%--added by Koushik Das as on 27-June-2017 for setting page index--%>
    </form>
</body>
</html>
