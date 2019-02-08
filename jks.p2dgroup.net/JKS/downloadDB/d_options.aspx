<%@ Page Language="C#" AutoEventWireup="true" CodeFile="d_options.aspx.cs" Inherits="JKS.JKS_downloadDB_d_options" %>

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
    <!----LightBox------>
    <link rel="stylesheet" href="../../LightBoxScripts/style.css" />
    <script type="text/javascript" src="../../LightBoxScripts/tinybox.js"></script>
    <!----->
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
    <style type="text/css">
        .mycheckbox input[type="checkbox"]
        {
            margin-right: 5px;
        }
    </style>
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
                        <div class="col-md-6 h_logo"><a href="javascript:void(0)" target="_self" title="P2D PAPER 2 DATA"> <img src="../images/JKS_logo.png" alt="JKS" width="110px" /></a></div>
                        
                    </div>
                    <!-------------------- END: Top Section -------------------->
                    </div>
                </header>
                <!------------------------------ END: Header ------------------------------>
                <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                <div class="login_bg">
                    <div class="current_comp" style="height: 450px;">
                        <div class="form-horizontal form_section fixed_height">
                            <div class="row">
                                <div class="col-md-4 col-md-offset-4">
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
                                                <asp:CheckBox ID="chkCurrentOnly" runat="server" Text=" Current Only" CssClass="mycheckbox" />
                                            </label>
                                            &nbsp;<div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Button ID="btnDownloadAttachment" runat="server" CssClass="sub_down btn-primary btn-group-justified"
                                                        Text="Download" BorderStyle="None" OnClick="btnDownloadAttachment_Click"></asp:Button>
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
                                <asp:Image ID="imgP2DLogo" runat="server" ImageUrl="~/JKS/images/p2d_logo.png"
                                    ImageAlign="Middle" />
                            </div>
                            <a id="aRefreshToBottom" href="Javascript:void(0);" name="aRefreshToBottom"></a>
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
        SelectTab("DataFile");
    </script>
    </form>
</body>
</html>
