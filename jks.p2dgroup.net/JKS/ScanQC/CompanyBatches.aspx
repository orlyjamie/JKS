<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyBatches.aspx.cs" Inherits="NewLook.NewLook_ScanQC_CompanyBatches" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>

<!DOCTYPE HTML <%--PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"--%> >

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
        <meta charset="utf-8" />
        <meta name="CODE_LANGUAGE" content="C#" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <meta name="vs_defaultClientScript" content="JavaScript" />
        <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
        <meta http-equiv="refresh" content="300" />
        <title>Company Batches</title>
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
        <style type="text/css">
            .company{ font-size: 18px; font-weight: bold; line-height: 50px; }
            .center-table{ margin: 0 auto; width: 100%; height: auto; margin-bottom: 20px; }
            .row-style{ border-right: 1px solid #ccc; color: #333; font: 10px Verdana; padding: 4px; text-decoration: none; cursor: pointer; height: 25px; border-bottom: 1px solid #ccc; }
            /*.row-style:hover{ background-color: #afc8ff; font-weight:bold !important; color: Black; }*/
            .head-style{ border: 0.5px solid #333; height: 33px; padding-left: 2px; font-size: 12px !important;  }
            .head-style th{ border: 1px solid #ccc !important; padding-left: 4px; }
            .total{ font-weight: bold !important; }
        </style>
        <script type="text/javascript">
            function PopupPage(url, w, h, bt) {
                //don't use these
                //h = $(window).height();
                //w = $(window).width();
                url += "&winH=" + h;
                var winl = (screen.width - w) / 2;
                var wint = (screen.height - h) / 2;

                if (bt == "CH") {
                    h = (h * 86.5) / 100;
                    w = (w * 99) / 100;

                    var winl = 0;
                    var wint = 0;
                }

                winprops = 'resizable=1,height=' + h + ',width=' + w + ',top=' + wint + ',left=' + winl + ",scrollbars=yes"; //,toolbar=yes
                window.open(url, "ActionWindow", winprops);
            };
        </script>
    </head>
    <body>
        <form id="form1" runat="server">
            <asp:HiddenField ID="hdnHeight" runat="server" />
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
                                        <div class="col-xs-12 col-sm-12">
                                            <div class="form-group form-group2">
                                                <div class="col-xs-12 col-sm-12">
                                                    <div class="row">
                                                        <asp:Label ID="lblCompanyName" runat="server" CssClass="company"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="PageHeader">
                                        Batches In Query
                                    </div>
                                    <div class="col-xs-12 col-sm-12">
                                        <div class="form-group form-group2">
                                            <div class=""><%--col-xs-12 col-sm-12--%>
                                                <div class="row">
                                                    <asp:GridView ID="gvBatchesInQuery" runat="server" AutoGenerateColumns="false" 
                                                        CssClass="listingArea center-table" DataKeyNames="BATCH ID" ShowFooter="true" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Batch ID" DataField="BATCH ID" ItemStyle-Width="7%" />
                                                            <asp:BoundField HeaderText="Batch Name" DataField="BATCH NAME" ItemStyle-Width="21%" />
                                                            <asp:BoundField HeaderText="Batch Type" DataField="BATCH TYPE NAME" ItemStyle-Width="10%" />
                                                            <asp:BoundField HeaderText="Scan Date" DataField="UPLOAD DATE" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ItemStyle-Width="13%" />
                                                            <asp:BoundField HeaderText="Batch Total" DataField="BATCH TOTAL" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" FooterStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField HeaderText="Processed" DataField="PROCESSED" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" FooterStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField HeaderText="Deleted" DataField="DELETED" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" FooterStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField HeaderText="In Query" DataField="IN QUERY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" FooterStyle-HorizontalAlign="Right" />
                                                            <asp:TemplateField HeaderText="Details" ItemStyle-Width="6%">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnDetails" runat="server" Text="Details" OnClick="lbtnDetails1_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="6%">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnAction" runat="server" Text="Action" OnClick="lbtnAction1_Click"></asp:LinkButton>
                                                                    <asp:Label ID="lblBeingEdited" runat="server" Text='<%#Eval("BEING EDITED COUNT")%>' CssClass='<%#(test.ToString().ToLower() == "true") ? "shown" : "hidden"%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle CssClass="row-style"></RowStyle>
                                                        <HeaderStyle CssClass="tableHd head-style"></HeaderStyle>
                                                        <FooterStyle ForeColor="Black" BackColor="#ffffff"></FooterStyle>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="PageHeader">
                                        Batches In Progress
                                    </div>
                                    <div class="col-xs-12 col-sm-12">
                                        <div class="form-group form-group2">
                                            <div class=""><%--col-xs-12 col-sm-12--%>
                                                <div class="row">
                                                    <asp:GridView ID="gvBatchesInProgress" runat="server" AutoGenerateColumns="false" 
                                                        CssClass="listingArea center-table" DataKeyNames="BATCH ID" ShowFooter="true" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Batch ID" DataField="BATCH ID" ItemStyle-Width="7%" />
                                                            <asp:BoundField HeaderText="Batch Name" DataField="BATCH NAME" ItemStyle-Width="21%" />
                                                            <asp:BoundField HeaderText="Batch Type" DataField="BATCH TYPE NAME" ItemStyle-Width="10%" />
                                                            <asp:BoundField HeaderText="Scan Date" DataField="UPLOAD DATE" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ItemStyle-Width="13%" />
                                                            <asp:BoundField HeaderText="Batch Total" DataField="BATCH TOTAL" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" FooterStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField HeaderText="Processed" DataField="PROCESSED" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" FooterStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField HeaderText="Deleted" DataField="DELETED" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" FooterStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField HeaderText="In Progress" DataField="IN PROGRESS" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%" FooterStyle-HorizontalAlign="Right" />
                                                            <asp:TemplateField HeaderText="Details" ItemStyle-Width="6%">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnDetails" runat="server" Text="Details" OnClick="lbtnDetails2_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="6%">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnAction" runat="server" Text="Action" OnClick="lbtnAction2_Click"></asp:LinkButton>
                                                                    <asp:Label ID="lblBeingEdited" runat="server" Text='<%#Eval("BEING EDITED COUNT")%>' CssClass='<%#(test.ToString().ToLower() == "true") ? "shown" : "hidden"%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle CssClass="row-style"></RowStyle>
                                                        <HeaderStyle CssClass="tableHd head-style"></HeaderStyle>
                                                        <FooterStyle ForeColor="Black" BackColor="#ffffff"></FooterStyle>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group form-group2">
                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group form-group2">
                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="sub_down btn-primary btn-group-justified cus-btn-display" />
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
                //SelectTab("ScanQCDoc");
            </script>
        </form>
    </body>
</html>