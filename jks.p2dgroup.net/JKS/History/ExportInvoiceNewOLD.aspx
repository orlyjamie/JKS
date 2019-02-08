<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportInvoiceNewOLD.aspx.cs"
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
                    <div class="current_comp" style="height: 450px">
                        <div class="form-horizontal form_section">
                            <div class="row">
                                <%-- <div class="PageHeader">
                                    <asp:Label ID="lblHeader" Text="Invoice Export" runat="server"></asp:Label>--%>
                            </div>
                            <div class="col-md-7 col-lg-7 col-sm-12">
                                <div class="col-md-12">
                                    <div class="form-group form-group2">
                                        &nbsp;
                                        <div class="col-lg-12">
                                            <%--Added by Kuntal karar on 20thApril2015--%>
                                            <div class="row">
                                            </div>
                                            <%--------------------------------------------------%>
                                            <div class="row">
                                                <%-- Added by Kuntal Karar on 20thAPril2015--%>
                                                <%--<asp:Button ID="btnExportNew"  CssClass="sub_down btn-primary btn-group-justified"
                                                        BorderStyle="None" Text="Export" runat="server" 
                                                        onclick="btnExportNew_Click" />--%>
                                                <asp:Button ID="btnExport" OnClick="btnExport_Click" CssClass="sub_down btn-primary btn-group-justified"
                                                    BorderStyle="None" Text="Export" runat="server" Visible="false" />
                                                <%--<p class="PageHeader">
                                                    Invoice Export</p>--%>
                                                <table class="NormalBody" id="Table3" cellspacing="2" cellpadding="2" border="0"
                                                    style="width: 559px; height: 121px">
                                                    <tr>
                                                        <td class="NormalBody">
                                                            Company :
                                                            <asp:DropDownList ID="ddlBuyerCompany" runat="server" Width="264px" DataValueField="CompanyID"
                                                                DataTextField="CompanyName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnExportNew" CssClass="sub_down btn-primary btn-group-justified"
                                                                BorderStyle="None" Text="Export" runat="server" OnClick="btnExportNew_Click"
                                                                Style="float: left" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--------------------------------------------------%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
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
