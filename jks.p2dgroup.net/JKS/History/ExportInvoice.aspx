<%@ Page Language="c#" CodeBehind="ExportInvoice.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.History.ExportInvoice" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
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
</head>
<body ms_positioning="GridLayout">
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
                                <table width="100%" id="Table1">
                                    <tr>
                                        <td valign="top" style="padding-left: ">
                                            <!-- Main Content Panel Starts-->
                                            <table width="100%" border="0" id="Table3" class="listingArea02">
                                                <tr>
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
    </div>
    <script src="../js/jquery-1.11.0.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../js/jquery-ui.js"></script>
    <script type="text/javascript">
        SelectTab("ExportInvoice");
    </script>
    </form>
</body>
</html>
