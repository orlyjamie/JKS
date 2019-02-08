<%@ Page Language="c#" CodeFile="RecReport.aspx.cs" AutoEventWireup="false" Inherits="JKS.RecReport" %>

<%@ Register TagPrefix="cc2" Namespace="CutePager" Assembly="ASPnetPagerV2Netfx1_1" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Import Namespace="System.IO" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;" />
    <meta name="description" content="">
    <meta name="author" content="">
    <title>ReconciliationReport</title>
    <script language="javascript" src="../../WinOpener.js"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
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
    <link href="../css/jquery-ui.css" rel="stylesheet">
    <link href="../custom_css/responsive.css" rel="stylesheet">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <style type="text/css">
        .PagerContainerTable
        {
            border-bottom: #333333 0px solid;
            border-left: #333333 0px solid;
            background-color: white;
            color: #d1d1e1;
            border-top: #333333 0px solid;
            border-right: #333333 0px solid;
            float: left;
            margin-bottom: 5px;
        }
        .PagerInfoCell
        {
            background-color: #3399cc;
            font-variant: small-caps;
            font-family: verdana, Tahoma, Arial;
            color: white;
            font-size: 10pt;
            font-weight: bolder;
            padding: 8px;
            float: left;
            line-height: 18px;
        }
        .PagerInfoCell:link
        {
            color: #ffcc66;
            text-decoration: none;
        }
        .PagerInfoCell:visited
        {
            color: #ffcc66;
            text-decoration: none;
        }
        .PagerCurrentPageCell
        {
            background-color: #555555;
            color: #ffffff;
            padding: 8px;
            float: left;
            line-height: 18px;
        }
        .PagerOtherPageCells
        {
            background-color: #076da1;
            color: #ffffff;
            padding: 8px;
            float: left;
            line-height: 18px;
        }
        .PagerSSCCells
        {
            background-color: #444444;
        }
        .PagerHyperlinkStyle
        {
            font: 11px arial, verdana, geneva, lucida, 'lucida grande' , arial, helvetica, sans-serif;
        }
        .PagerHyperlinkStyle:hover
        {
            font: 11px arial, verdana, geneva, lucida, 'lucida grande' , arial, helvetica, sans-serif;
            color: #ffcc66;
            text-decoration: none;
        }
        .PagerHyperlinkStyle:link
        {
            font: 11px arial, verdana, geneva, lucida, 'lucida grande' , arial, helvetica, sans-serif;
            color: #ffcc66;
            text-decoration: none;
        }
        .PagerHyperlinkStyle:visited
        {
            font: 11px arial, verdana, geneva, lucida, 'lucida grande' , arial, helvetica, sans-serif;
            color: #ffcc66;
            text-decoration: none;
        }
        .PagerHyperlinkStyle:active
        {
            font: 11px arial, verdana, geneva, lucida, 'lucida grande' , arial, helvetica, sans-serif;
            color: #ffcc66;
            text-decoration: none;
        }
    </style>
</head>
<body ms_positioning="GridLayout">
    <form id="RecReportNew" method="post" runat="server">
    <!-------------- START: Site Wrapper ------------------------------------------------->
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
                <div class="second_part">
                    <div class="form-horizontal form_section">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="col-md-12">
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-4 control-label label_text" style="margin-left: 20px;">
                                            Company Name :</label>
                                        <div class="col-lg-6">
                                            <div class="row">
                                                <asp:DropDownList ID="ddlBuyerCompany" runat="server" CssClass="form-control inpit_select"
                                                    DataValueField="CompanyID" DataTextField="CompanyName" AutoPostBack="True" OnSelectedIndexChanged="ddlBuyerCompany_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="col-md-12">
                                    <div class="form-group form-group2" style="display: none">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                            Date From :</label>
                                        <div class="col-lg-6">
                                            <div class="row">
                                                <input type="text" id="txtFromDate" class="form-control inpit_select" runat="server"
                                                    readonly="readonly" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2" style="display: none">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                            Date To :</label>
                                        <div class="col-lg-6">
                                            <div class="row">
                                                <input type="text" id="txtToDate" class="form-control inpit_select" runat="server"
                                                    readonly="readonly" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                            &nbsp;</label>
                                        <div class="col-lg-7">
                                            <div class="row">
                                                <asp:Button ID="btnSubmit" runat="server" CssClass="sub_down btn-primary btn-group-justified"
                                                    Text="Submit" BorderStyle="None" OnClick="btnSubmit_Click" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                    <div class="col-lg-12">
                        <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td class="PageHeader blue_headinggap" width="100%">
                                        ScanQC Reconciliation Report&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <cc2:Pager ID="Pager1" runat="server" PageSize="10" NotCompactedPageCount="10"></cc2:Pager>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        <asp:DataGrid ID="grdInvCur" runat="server" CellPadding="3" GridLines="None" Width="100%"
                                            AutoGenerateColumns="False" CssClass="listingArea">
                                            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                            <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                            <ItemStyle></ItemStyle>
                                            <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                            <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateColumn Visible="False" HeaderText="BATCHID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBATCHID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BATCHID") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Company">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCompany" Text='<%# DataBinder.Eval(Container, "DataItem.Company") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Doc Type" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDocType" Text='<%# DataBinder.Eval(Container, "DataItem.BATCHDOCTYPE") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Batch Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBatchName" Text='<%# DataBinder.Eval(Container, "DataItem.BATCHNAME") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Upload Date">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblUploadDate" Text='<%# DataBinder.Eval(Container, "DataItem.UPLOADDATE", "{0:dd-MM-yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Uploaded">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblUploaded" Text='<%# DataBinder.Eval(Container, "DataItem.NOFILESINDIRECTORY") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Imported">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblImported" Text='<%# DataBinder.Eval(Container, "DataItem.NUMOFINVOICESCOPIEDTOIPEINPUT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Processed">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblProcessed" Text='<%# DataBinder.Eval(Container, "DataItem.NUMOFINVOICESARCHIVEDBYQC") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Deleted">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDeleted" Text='<%# DataBinder.Eval(Container, "DataItem.NUMOFINVOICESDELETED") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="QC Balance">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblQCBAL" Text='<%# DataBinder.Eval(Container, "DataItem.QCBALANCE") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Awaiting Import">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAwaitImport" Text='<%# GetAwaitingToImport(DataBinder.Eval(Container, "DataItem.AWAITINGIMPORT"),DataBinder.Eval(Container, "DataItem.BATCHID")) %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        <table class="normalbody" id="Table3" cellspacing="1" cellpadding="1" width="100%"
                                            border="0">
                                            <tr>
                                                <td nowrap>
                                                    <strong>Total Uploads :</strong>
                                                </td>
                                                <td nowrap>
                                                    <asp:Label ID="lbltotalupload" runat="server"></asp:Label>
                                                </td>
                                                <td class="normalbody" nowrap>
                                                    <strong>Total Processed :</strong>
                                                </td>
                                                <td nowrap>
                                                    <asp:Label ID="lblTotalProcessed" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap>
                                                    <strong>Total&nbsp;Imported :</strong>
                                                </td>
                                                <td nowrap>
                                                    <asp:Label ID="lblTotalImported" runat="server"></asp:Label>
                                                </td>
                                                <td nowrap>
                                                    <strong>Total Deleted :</strong>
                                                </td>
                                                <td nowrap>
                                                    <asp:Label ID="lbltotalDel" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap>
                                                    <strong>Total QC Balance :</strong>
                                                </td>
                                                <td nowrap>
                                                    <asp:Label ID="lblTotQCBal" runat="server"></asp:Label>
                                                </td>
                                                <td nowrap>
                                                    <strong>Total&nbsp;Awaiting Import :</strong>
                                                </td>
                                                <td nowrap>
                                                    <asp:Label ID="totAwaitReport" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="PageHeader" width="100%">
                                        Hub Reconciliation Report&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        <table class="NormalBody" id="Table2" cellspacing="1" cellpadding="1" width="100%"
                                            border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 219px; height: 15px" nowrap align="center">
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px; height: 15px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="height: 15px" nowrap align="right">
                                                        <%--<strong>Stock</strong>--%>
                                                        <strong>Stock</strong>
                                                    </td>
                                                    <td style="width: 127px; height: 15px" nowrap align="right">
                                                        <strong>Expense</strong>
                                                    </td>
                                                    <td style="height: 15px" nowrap align="right">
                                                        <strong>Total</strong>
                                                    </td>
                                                    <td style="height: 15px" nowrap align="right">
                                                        <strong>Rec</strong>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Scanned docs imported</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Scanneddocimported" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Scanneddocimported" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbltot_Scanneddocimported" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblrec" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>E-docs imported</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Edocimported" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Edocimported" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotal_Edocimported" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr class="Banner">
                                                    <td style="width: 219px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap align="right">
                                                        <strong>Sub-total</strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblstsubtot" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblexsubtot" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblalltot" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td style="width: 127px" nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>InvUpload</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblst_InvUpload" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblexp_InvUpload" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbltot_InvUpload" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr class="Banner">
                                                    <td style="width: 219px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap align="right">
                                                        <strong>Total</strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblst_total" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblex_total" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbltot_total" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td style="width: 127px" nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Debit Notes</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblst_dbt" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblex_dbt" runat="server">N/A</asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTot_dbt" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr class="Banner">
                                                    <td style="width: 219px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap align="right">
                                                        <strong>Grand Total</strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblst_Gtotal" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblex_Gtotal" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbltot_Gtotal" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td style="width: 127px" nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Deleted before Registration</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Delete_Before_Registered" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Delete_Before_Registered" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbllbltot_Delete_Before_Registered" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Exported for Registration</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Exported_for_Registration" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Exported_for_Registration" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbltot_Exported_for_Registration" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Awaiting Export for Registration</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Awaiting_for_Registration" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblexp_Awaiting_for_Registration" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbltot_Awaiting_for_Registration" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr class="Banner">
                                                    <td style="width: 219px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap align="right">
                                                        <strong>Total</strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblstgtot_Awaiting" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblexgtot_Awaiting" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbltot_Awaiting" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td style="width: 127px" nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Received</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Recieved" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Recieved" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotal_Recieved" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Registered</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Registered" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Registered" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotal_Registered" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Rejected</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Rejected" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Rejected" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotal_Rejected" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Reopened</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Reopened" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Reopened" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotal_Reopened" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Approved</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Approved" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Approved" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotal_Approved" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Paid</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Paid" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Paid" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotal_Paid" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Deleted/Archived</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Deleted" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Deleted" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotal_Deleted" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Received*</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStockUnmatched" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExpUnmatched" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotalUnmatched" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Matching</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStockUQuery" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExpUQuery" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotalUQuery" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Exported</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStockExported" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExpExported" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotalExported" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Approved s.t. AP</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStockSTAP" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExpSTAP" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotalSTAP" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <%--Added by Koushik Das as on 06-Apr-2017--%>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Archived</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStockArchived" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExpArchived" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTotalArchived" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <%--Added by Koushik Das as on 06-Apr-2017--%>
                                                <tr class="Banner">
                                                    <td style="width: 219px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap align="right">
                                                        <strong>Total</strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_Total" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_Total" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lbl_Total" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 219px" nowrap>
                                                        <strong>Debit Notes</strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblst_dbt1" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="Label2" runat="server">N/A</asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblTot_dbt1" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                                <tr class="Banner">
                                                    <td style="width: 219px" nowrap>
                                                        <strong></strong>
                                                    </td>
                                                    <td style="width: 78px" nowrap align="right">
                                                        <strong>Grand Total</strong>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblStock_GrandTotal" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 127px" nowrap align="right">
                                                        <asp:Label ID="lblExp_GrandTotal" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap align="right">
                                                        <asp:Label ID="lblGrand_Total" runat="server"></asp:Label>
                                                    </td>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script src="../js/jquery-1.11.0.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromDate").datepicker({
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
        SelectTab("RecReport");
    </script>
</body>
</html>
