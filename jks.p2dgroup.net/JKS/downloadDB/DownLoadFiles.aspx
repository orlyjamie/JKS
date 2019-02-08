<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownLoadFiles.aspx.cs" Inherits="JKS.JKS_downloadDB_DownLoadFiles" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
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
    <title>P2D Network - DownLoad Files</title>
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
    <link href="../custom_css/responsive.css" rel="stylesheet">
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="../js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="../js/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" language="javascript">
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
        $(document).ready(function(e) {
            SizeDownloadList();
        });
        $(window).resize(function(e) {
            SizeDownloadList();
        });
        function SizeDownloadList() {
            var $xDiv = $("#<%=grdDownLoad.ClientID%>").parent();
            var h = $(".current_comp").height() - 20;
            //alert(h);
            $xDiv.height(h);
            $xDiv.css("overflow", "auto");
        }
    </script>
</head>
<body onunload="javascript:doHourglass();" ms_positioning="GridLayout">
    <form id="Form2" method="post" runat="server">
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
                <div class="login_bg">
                    <div class="current_comp fixed_height">
                        <div class="form-horizontal form_section">
                            <div class="row">
                                <%-- <p Class="PageHeader"><asp:label id="Label1" runat="server" width="100%" > Downloads</asp:label></p>--%>
                                <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                <div class="col-md-12" align="center">
                                    <asp:DataGrid ID="grdDownLoad" runat="server" Width="95%" AutoGenerateColumns="false"
                                        GridLines="Vertical" CellPadding="3" PageSize="15" CssClass="listingArea">
                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                        <ItemStyle></ItemStyle>
                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="FileName" HeaderText="File Name"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Download">
                                                <HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="35%"></ItemStyle>
                                                <ItemTemplate>
                                                    <a id="lnkDelete" href='<%# DataBinder.Eval(Container, "DataItem.FileName") %>' onserverclick="DeleteItem"
                                                        runat="server"><strong>Download</strong></a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                        </PagerStyle>
                                    </asp:DataGrid>
                                    &nbsp;
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
    </form>
</body>
</html>
