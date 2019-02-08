<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportInvoiceNew.aspx.cs"
    Inherits="ETC_History_ExportInvoiceNew" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
    <title>Export Invoices</title>
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
    <script type="text/javascript">
        function validation() {
            if (document.getElementById("ddlBuyerCompany").selectedIndex == 0) {
                alter("Please select a Child company.");
                return false;
            }
            else
                return true;
        }
        function FnCompleted() {
            alert('Completed.');
            window.location.href = 'DownLoadFiles.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
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
                    <div class="current_comp" style="height: 550px; padding: 5% 6% 3% !important;">
                            <div class="row">
                                  <div class="col-md-4 col-lg-4 col-sm-4">
                                <div class="form-group form-group2">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <%--<asp:HyperLink ID="hypDownloadInvoice" runat="server" CssClass="sub_down btn-primary btn-group-justified center_alin"
                                                NavigateUrl="d_options.aspx?Type=INVOICE" Style="width: 100%;">RUN MATCH</asp:HyperLink>--%>
                                                 <asp:Button ID="Button1" runat="server" Style="float: left !important; width: 100%;"
                                                CssClass="sub_down btn-primary btn-group-justified" BorderStyle="None" ToolTip="RUN MATCH"
                                                Text="RUN MATCH" onclick="Button1_Click" ></asp:Button>
                                            <br />
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-lg-4 col-sm-4">
                                <div class="form-group form-group2">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <%-- <a class="sub_down btn-primary btn-group-justified center_alin" href="DownLoadFiles.aspx"
                                                style="width: 100%;">PO INV EXPORT</a>--%>
                                                 <asp:Button ID="btnPO" runat="server" Style="float: left !important; width: 100%;"
                                                CssClass="sub_down btn-primary btn-group-justified" BorderStyle="None" ToolTip="PO INV EXPORT"
                                                Text="PO INV EXPORT" onclick="btnPO_Click" ></asp:Button>
                                                <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Added by Mrinal on 2nd March 2015--->
                            <div class="col-md-4 col-lg-4 col-sm-4">
                                <div class="form-group form-group2">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <asp:Button ID="btnNONPO" runat="server" Style="float: left !important; width: 100%;"
                                                CssClass="sub_down btn-primary btn-group-justified" BorderStyle="None" ToolTip="NONPO INV EXPORT"
                                                Text="NONPO INV EXPORT" onclick="btnNONPO_Click"></asp:Button>
                                                <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                       
                            <div class="form-horizontal form_section">
                                <table style="width: 840px; height: 67px" width="840">
                                    <tr>
                                        <td class="NormalBody" valign="top">
                                            <!-- Main Content Panel Starts-->
                                            <p>
                                                <%--<asp:HyperLink ID="hypDownloadDebitNote" runat="server" ImageUrl="" NavigateUrl="d_options.aspx?Type=DEBIT">Download DebitNote</asp:HyperLink>--%>                                                <%--<a href="upOrders.aspx">Upload Orders</a>--%>
                                            </p>
                                            <!-- Main Content Panel Ends-->
                                            <asp:image id="Image1" ImageUrl="" runat="server"/>
                                        </td>
                                        <td class="NormalBody" valign="top">
                                            <!-- Main Content Panel Starts-->
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
    </div>
    </div>
    <script src="../js/jquery-1.11.0.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../js/jquery-ui.js"></script>
    <script type="text/javascript">
        SelectTab("ExportInvoice");
    </script>
    <script type="text/javascript">
        function fnExportInvoice() {
            alert('The export has started. Depending on the number of invoices it may take up to 30 minutes to complete. Please do NOT press the export button again during this period.');
            window.location.href = '../History/ExportInvoiceNew.aspx';
        }
    </script>
    </form>
</body>
</html>
