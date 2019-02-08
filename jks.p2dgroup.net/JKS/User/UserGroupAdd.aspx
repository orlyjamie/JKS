<%@ Page Language="c#" CodeFile="UserGroupAdd.aspx.cs" AutoEventWireup="false" Inherits="JKS.UserGroupAdd" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
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
    <title>User Group Add</title>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
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
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">		
    </script>
</head>
<body>
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
                        <div class="col-md-6 h_logo"><a href="javascript:void(0)" target="_self" title="P2D PAPER 2 DATA">
                        <img src="../images/JKS_logo.png" alt="JKS" width="110px" /></a></div>
                        
                    </div>
                    <!-------------------- END: Top Section -------------------->
                    </div>
                </header>
                <!------------------------------ END: Header ------------------------------>
                <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                <div class="login_bg">
                    <div class="current_comp fixed_height">
                        <%--<div class="PageHeader">
                            <asp:Label ID="lblHeader" runat="server">Add User Group</asp:Label>
                        </div>--%>
                        <div class="col-xs-12 col-sm-6">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <div class="col-md-12">
                                <div class="form-group form-group2">
                                    <label for="inputEmail" class="col-xs-12 col-sm-5 control-label label_text">
                                        Group Name <font color="red">*</font></label>
                                    <div class="col-xs-12 col-sm-7">
                                        <div class="row">
                                            <asp:TextBox ID="tbGroupName" runat="server" Width="216px" MaxLength="100" CssClass="form-control inpit_select"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6">
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-xs-12 control-label label_text">
                                            <font color="red">* = Mandatory Field</font></label>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group form-group2 widht_floart">
                                        <div class="col-lg-12">
                                            <div class="row">
                                                <asp:Button ID="imgBtnSave" runat="server" Text="Save" CssClass="sub_down btn-primary btn-group-justified center_alin" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2 widht_floart">
                                        <div class="col-lg-12">
                                            <div class="row">
                                                <a class="sub_down btn-primary btn-group-justified center_alin" onclick="javascript:window.location.href='../User/UserBrowse.aspx';">
                                                    Back</a>
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
                            <asp:DataGrid ID="grdUser" runat="server" CssClass="listingArea" Width="100%" OnItemCommand="Datagrid_Click"
                                AutoGenerateColumns="False" GridLines="None" CellPadding="3" BorderStyle="None">
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle></ItemStyle>
                                <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDelete" CssClass="grid_btn" runat="server" AlternateText="Delete"
                                                CommandName="DELETERECORD" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"UserGroupID")%>'>
                                            </asp:ImageButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="UserGroupName" HeaderText="Group Name"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="UserGroupID" HeaderText="GroupID"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                </PagerStyle>
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
    <table width="100%">
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
                <!-- Main Content
    Panel Starts-->
                <!-- Main Content Panel Ends-->
                <tr>
                    <td class="NormalBody">
                    </td>
                    <td class="NormalBody">
                    </td>
                </tr>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
