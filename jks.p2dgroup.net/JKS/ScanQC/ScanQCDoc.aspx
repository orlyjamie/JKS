<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScanQCDoc.aspx.cs" Inherits="NewLook.NewLook_ScanQC_ScanQCDoc" EnableEventValidation="false" %>
<%--don't remove EnableEventValidation="false"--%>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>

<!DOCTYPE HTML <%--PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"--%> >

<html lang="en">
    <head runat="server">
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
        <title>Scan QC Doc</title>
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
        <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet" />
        <style type="text/css">
            .center-table{ margin: 0 auto; }
            .row-style{ border-right: 1px solid #ccc; color: #333; font: 10px Verdana; padding: 4px; text-decoration: none; cursor: pointer; height: 25px; border-bottom: 1px solid #ccc; }
            .row-style:hover{ background-color: #afc8ff; font-weight:bold !important; color: Black;}
            .head-style{ border: 0.5px solid #333; height: 33px; padding-left: 2px; font-size: 11px !important; }
            .head-style th{ border: 1px solid #ccc !important; padding-left: 4px; }
        </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <div class="site" style="height: auto !important">
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
                            <div class="current_comp fixed_height">
                                <div class="form-horizontal form_section">
                                    <div class="row">
                                        <%--<div class="PageHeader">
                                            <asp:Label ID="Label1" runat="server">Scan QC</asp:Label>
                                        </div>--%>
                                        <div class="col-xs-12 col-sm-12">
                                            <div class="col-md-12">
                                                <div class="form-group form-group2">
                                                    <div class="col-xs-12 col-sm-12">
                                                        <div style="float: right; margin-left: -10%;">
                                                            <asp:Button ID="btnUploadFiles" runat="server" Text="Upload Files" CssClass="sub_down btn-primary btn-group-justified cus-btn-display"></asp:Button>
                                                        </div>
                                                        <div class="row">
                                                            <asp:GridView ID="gvScanQCDocs" runat="server" AutoGenerateColumns="false" 
                                                                CssClass="listingArea center-table" DataKeyNames="CompanyID" GridLines="None"
                                                                EmptyDataText="No Records Found">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Company" DataField="CompanyName" ItemStyle-Width="280" HeaderStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField HeaderText="No. in Progress" DataField="BATCHES_IN_PROGRESS" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120" />
                                                                    <asp:BoundField HeaderText="No. in Query" DataField="BATCHES_IN_QUERY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120" />
                                                                </Columns>
                                                                <RowStyle CssClass="row-style"></RowStyle>
                                                                <HeaderStyle CssClass="tableHd head-style"></HeaderStyle>
                                                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                                            </asp:GridView>
                                                            <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
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
                SelectTab("ScanQCDoc");
            </script>
        </form>
    </body>
</html>
