﻿<%@ Page Language="C#" AutoEventWireup="false" CodeFile="CurrentStatusNew.aspx.cs"
    Inherits="JKS.ETC_Current_CurrentStatusNew" EnableEventValidation="false" %>

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
    <title>CurrentStatus</title>
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
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <asp:Button ID="btnCloseAction" runat="server" Visible="False"></asp:Button>
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
                        <div class="col-md-6 h_logo"><a href="javascript:void(0)" target="_self" title="P2D PAPER 2 DATA"><img src="../../images/ETC_logoNewLogin1.jpg" alt=""></a></div>
                        
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
                                                </div>
                                            </div>
                                            <input id="cbSupplier" type="checkbox" runat="server" style="margin-top: 3px; float: left;
                                                margin-left: 10px;" title="Tick for wildcard search" />
                                            <%--<input id="cbSupplier" type="checkbox" style="margin-top: 3px; float: left; margin-left: 10px;"
                                                title="Tick for wildcard search" onclick="showHide();" />--%>
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
                                                        <asp:ListItem Value="0" Selected="True">Select Doc Type</asp:ListItem>
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
                                        <div class="form-group form-group2">
                                            <asp:Label ID="lblMsg" runat="server" CssClass="col-lg-12 control-label" BorderStyle="None"
                                                ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lblMessage" runat="server" BorderStyle="None" CssClass="col-lg-12 control-label"
                                                ForeColor="Red"></asp:Label>
                                        </div>
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
                                                margin-left: 10px;" title="Tick for wildcard search" />
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
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="sub_down btn-primary btn-group-justified"
                                                        Text="Search" BorderStyle="None"></asp:Button>
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
                                                    <input type="text" id="txtFromDate" class="form-control inpit_select " runat="server" />
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
                                                    <asp:TextBox ID="textRange1" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
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
                </div>
                <div class="clearfix">
                </div>
                <div class="container" style="padding: 0 !important;">
                    <div class="row">
                        <div class="col-lg-12">
                            <div id="divP2DLogo" runat="server" class="p2d_logo" visible="false">
                                <asp:Image ID="imgP2DLogo" runat="server" ImageUrl="~/ETC/images/p2d_logo.png" ImageAlign="Middle" />
                            </div>
                            <asp:DataGrid ID="grdInvCur" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                GridLines="None" CellPadding="0" CellSpacing="0" PageSize="15" AllowPaging="True"
                                OnItemDataBound="grdInvCurItem_Bound" CssClass="listingArea" Width="100%">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle></ItemStyle>
                                <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Doc No." SortExpression="ReferenceNo">
                                        <ItemTemplate>
                                            <%#Getredirecturl(DataBinder.Eval(Container.DataItem,"DocType"),DataBinder.Eval(Container.DataItem,"ReferenceNo"),DataBinder.Eval(Container.DataItem,"InvoiceID"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Pass To History" Visible="False">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%# GetLogURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>">
                                                <b>Invoice Log History </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Action" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceID" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceID") %>'></asp:Label>
                                            <asp:Label ID="lblDocType" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DocType") %>'></asp:Label>
                                            <asp:Label ID="lblInvoiceDate" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceDate") %>'></asp:Label>
                                            <asp:Label ID="lblDocStatus" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DocStatus") %>'></asp:Label>
                                            <asp:Label ID="lblVoucherNumber" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VoucherNumber") %>'></asp:Label>
                                            <asp:LinkButton ID="hpAct" runat="server" CommandName="ACT"><b>Action</b></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Action">
                                        <%--
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetURLTest(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"),DataBinder.Eval(Container.DataItem,"VAT"),DataBinder.Eval(Container.DataItem,"Total"),DataBinder.Eval(Container.DataItem,"New_VendorClass"),DataBinder.Eval(Container.DataItem,"RowID"))%>">
                                                <b>Action </b></a>
                                        </ItemTemplate>
                                        --%>
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#IFrameWindow(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"),DataBinder.Eval(Container.DataItem,"VAT"),DataBinder.Eval(Container.DataItem,"Total"),DataBinder.Eval(Container.DataItem,"New_VendorClass"),DataBinder.Eval(Container.DataItem,"RowID"))%>">
                                                <b>Action </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status History">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%# GetStatusURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"))%>">
                                                <b>Status History </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="DocType" SortExpression="DocType" HeaderText="Doc Type">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ScanDate" SortExpression="ScanDates" HeaderText="Scan Date">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="InvoiceDate" SortExpression="InvoiceDates" HeaderText="Invoice Date">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ScanDates" SortExpression="ScanDates" Visible="False"
                                        HeaderText="Scan Date"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Supplier" SortExpression="Supplier" HeaderText="Supplier">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Buyer" SortExpression="Buyer" HeaderText="Buyer"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="VendorID" SortExpression="VendorID" HeaderText="VendorID">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DocStatus" SortExpression="DocStatus" HeaderText="Doc Status">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="ActionStatus" HeaderText="Action Status">
                                    </asp:BoundColumn>
                                    <%--  <asp:BoundColumn DataField="PaymentDueDate" SortExpression="PaymentDueDate" HeaderText="Payment Due Date"
                                        DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>--%>
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
                                    <asp:BoundColumn DataField="ParentRowFlag" Visible="False" HeaderText="ParentRowFlag">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="User" HeaderText="User" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Currency" SortExpression="Currency" HeaderText="Currency">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Net" SortExpression="Net1" HeaderText="Net" ItemStyle-HorizontalAlign="Right">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="VAT" SortExpression="VAT1" HeaderText="VAT" ItemStyle-HorizontalAlign="Right">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Total" SortExpression="Total1" HeaderText="Gross" ItemStyle-HorizontalAlign="Right">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Comment" HeaderText="Comment" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="VoucherNumber" HeaderText="VoucherNumber">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Link to Image" Visible="False">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetDocumentWithPath(DataBinder.Eval(Container.DataItem,"DocAttachments"))%>">
                                                <b>Scanned Image</b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="InvoiceDate" SortExpression="InvoiceDate"
                                        HeaderText="Invoice Date" DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DeliveryDate" SortExpression="DeliveryDate"
                                        HeaderText="Delivery Date" DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="History" Visible="False">
                                        <ItemTemplate>
                                            <a href='#' onclick="<%#GetCommentURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>">
                                                <b>Show History </b></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ReferenceNo" Visible="false"></asp:BoundColumn>
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
                                    <asp:BoundColumn DataField="ActivityCode" HeaderText="ActivityCode" Visible="False">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="">
                                        <HeaderStyle></HeaderStyle>
                                        <ItemStyle BackColor="White"></ItemStyle>
                                        <ItemTemplate>
                                            <a href='#' onclick="<%# GetAPCommentsURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"),DataBinder.Eval(Container.DataItem,"ReferenceNo"),DataBinder.Eval(Container.DataItem,"DocStatus"))%>">
                                                <img id="imgComment" runat="server" border="0"></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <%-- <asp:TemplateColumn HeaderText="Tiff">
                                    <HeaderStyle></HeaderStyle>
                                    <ItemStyle BackColor="White"></ItemStyle>
                                    <ItemTemplate>
                                        <a href='#' onclick="<%# GetTiffViewersURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"))%>">
                                            <img id="imgTiff" alt="ViewTiff" runat="server" border="0" src="~/images/Add_Tiff.png"
                                                style="width: 25px; height: 25px;" />
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>--%>
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

//            var str = document.getElementById('<%=textRange2.ClientID%>').value;
//            var decimalOnly = /^\s*-?[1-9]\d*(\.\d{1,2})?\s*$/;
//           

//            if (decimalOnly.test(str)) {
//                alert('It is GOOD!');
//            }
//            else {
//                alert('invalid code');
//            }

                        else if (e.which >= 48 && e.which <= 57)
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
            $("#txtToDate").datepicker({ dateFormat: 'dd/mm/yyyy' });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            var ID;

            $("#txtSupplier").autocomplete({
                source: function (request, response) {

                    // var varCompanyID = e.options[e.selectedIndex].value;
                    var varCompanyID = document.getElementById("ddlCompany").value;
                    var usrString = document.getElementById('txtSupplier').value;
                    var varuserId = '<%= Session["userId"]%>';
                    var varUserTypeID = '<%= Session["UserTypeID"]%>';
                    ID = this.element.attr("id");
                    var param = { CompanyID: varCompanyID, userId: varuserId, userTypeID: varUserTypeID, UserString: $('#txtSupplier').val() };
                    $.ajax({
                        url: "CurrentStatusNew.aspx/GetSupplier",
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
                    // $("#HdSupplierId").val = i.item.CompanyID;
                    // alert(i.item.CompanyID);

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

                    var e = document.getElementById('ddlDocType');
                    var strDropDownValue = e.options[e.selectedIndex].value;
                    if (strDropDownValue == "0" || strDropDownValue == "--Select--") {
                        // alert("Please select Company");
                        // document.getElementById(ID).value = "";
                        // return;
                        varDocType = "";
                    }

                    var param = { CompanyID: varCompanyID, DocType: varDocType, UserString: $('#txtInvoiceNo').val() };
                    $.ajax({
                        url: "CurrentStatusNew.aspx/FetchInvoiceNo",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {

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
                        url: "CurrentStatusNew.aspx/GetNominalName",
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
        SelectTab("Current");
    </script>
    </form>
</body>
</html>
