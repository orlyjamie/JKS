<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>

<%@ Page Language="c#" CodeFile="UserBrowse.aspx.cs" AutoEventWireup="false" Inherits="JKS.UserBrowse" %>

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
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <title>P2D Network - Browse Users</title>
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
    <%--ADDNewUserClick() added by kuntalkarar on 20thSeptember2016--%>
    <script language="javascript">
        function ADDNewUserClick() {

            $.ajax({
                type: "POST",
                url: "UserBrowse.aspx/SetSession",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // alert(response.d);
                    //windows.location.href = response.d;
                }
            });
        }	
    </script>
    <script language="javascript">
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }	
    </script>
</head>
<body onbeforeunload="javascript:doHourglass();" onunload="javascript:doHourglass();"
    topmargin="0">
    <form id="Form2" method="post" runat="server">
    <div class="site">
        <div class="container mainWrapper nopadding">
            <div class="mainWrapper">
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
                                <%--<div class="PageHeader">
                                    <asp:Label ID="Label1" runat="server" Width="100%">User Management</asp:Label>
                                </div>--%>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="col-md-12">
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        Company Name</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlCompany" TabIndex="1" runat="server" CssClass="form-control inpit_select"
                                                                DataTextField="CompanyName" DataValueField="CompanyID" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        Business Unit</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlBusinessUnit" TabIndex="2" runat="server" CssClass="form-control inpit_select"
                                                                DataTextField="BusinessUnitName" DataValueField="BusinessUnitID">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        Department</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlDepartment" TabIndex="3" runat="server" CssClass="form-control inpit_select"
                                                                DataTextField="Department" DataValueField="DepartmentID">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        Group</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlGroup" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                                DataTextField="UserGroupName" DataValueField="UserGroupID">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        User Name</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlUser" TabIndex="5" runat="server" CssClass="form-control inpit_select"
                                                                DataTextField="UsersName" DataValueField="UserID">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        &nbsp;</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:Button ID="btnSearch" runat="server" CssClass="grid_btn2 rgt_nomargin" BorderStyle="None"
                                                                Text="Search"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <div class="col-lg-12">
                                                        <div class="row">
                                                            <%--       <asp:ImageButton ID="imgChangePass" runat="server" CssClass="grid_btn2" AlternateText="Change Password"
                                                                BorderStyle="None"></asp:ImageButton>--%>
                                                            <%--<asp:ImageButton ID="imgButtonSendMailInfo" runat="server" CssClass="grid_btn2 rgt_nomargin" AlternateText="Send Login Info"
                                                                BorderStyle="None"></asp:ImageButton>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group2">
                                                <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                    &nbsp;
                                                </label>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <asp:HyperLink ID="HyperLink1" runat="server" onclick="ADDNewUserClick();" NavigateUrl="UserEdit.aspx"
                                                            CssClass="grid_btn2">Add new User</asp:HyperLink>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group form-group2">
                                                <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                    &nbsp;
                                                </label>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="UserGroupAdd.aspx" CssClass="grid_btn2">Add User Group</asp:HyperLink>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group form-group2">
                                                <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                    &nbsp;
                                                </label>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <asp:Button ID="imgChangePass" runat="server" CssClass="grid_btn2" Text="Change Password"
                                                            BorderStyle="None" style="color: #ffb400 !important;"></asp:Button>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group form-group2">
                                                <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                    &nbsp;
                                                </label>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <asp:Button ID="imgButtonSendMailInfo" runat="server" CssClass="grid_btn2" Text="Send Login Info"
                                                            BorderStyle="None" style="color: #90ff06 !important;"></asp:Button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" Visible="true" ForeColor="Red"></asp:Label>
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
                        <asp:DataGrid ID="grdUser" runat="server" CellPadding="3" GridLines="None" AutoGenerateColumns="False"
                            CssClass="listingArea" OnItemCommand="Datagrid_Click" Width="100%" DataKeyField="UserID">
                            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                            <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                            <ItemStyle></ItemStyle>
                            <HeaderStyle CssClass="tableHd"></HeaderStyle>
                            <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                            <Columns>
                                <asp:TemplateColumn HeaderText="Edit" HeaderStyle-Width="5%" ItemStyle-Width="5%" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a href='#' class="grid_btn" onclick="<%#GetURLEdit(DataBinder.Eval(Container.DataItem,"UserID"),DataBinder.Eval(Container.DataItem,"New_UserGroup"))%>">
                                            Select </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%--<asp:Button ID="imgBtnDelete" runat="server" CssClass="grid_btn" Text="Delete" BorderStyle="None"--%>
                                      <%--  Changed by Soumyajit on 9.3.15--%>
                                        <asp:Button ID="imgBtnDelete" runat="server" CssClass="grid_btnDel" Text="Delete" BorderStyle="None"
                                            CommandName="DELETERECORD" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"UserID")%>'>
                                        </asp:Button>
                                        <%--<asp:ImageButton ID="imgBtnDelete" runat="server" CssClass="grid_btn" AlternateText="Delete"
                                            BorderStyle="None" CommandName="DELETERECORD" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"UserID")%>'>
                                        </asp:ImageButton>--%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="Designation" HeaderText="Role" HeaderStyle-Width="15%" ItemStyle-Width="15%" FooterStyle-Width="15%"></asp:BoundColumn>
                                <asp:BoundColumn DataField="UserType" HeaderText="User Type" HeaderStyle-Width="8%" ItemStyle-Width="8%" FooterStyle-Width="8%"></asp:BoundColumn>
                                <asp:BoundColumn DataField="New_UserCode" HeaderText="User Code" HeaderStyle-Width="8%" ItemStyle-Width="8%" FooterStyle-Width="8%"></asp:BoundColumn>
                                <asp:BoundColumn DataField="UsersName" HeaderText="User Name" HeaderStyle-Width="8%" ItemStyle-Width="8%" FooterStyle-Width="8%"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Department" HeaderText="Department" Visible="false" HeaderStyle-Width="8%" ItemStyle-Width="8%" FooterStyle-Width="8%"></asp:BoundColumn>
                                <asp:BoundColumn DataField="New_UserGroup" HeaderText="Group" HeaderStyle-Width="8%" ItemStyle-Width="8%" FooterStyle-Width="8%"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Change Password" HeaderStyle-Width="5%" ItemStyle-Width="5%" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkChangePassword" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                    <HeaderStyle ForeColor="#ffb400" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="EMail" HeaderText="Email" HeaderStyle-Width="20%" ItemStyle-Width="20%" FooterStyle-Width="20%"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Send Login" HeaderStyle-Width="5%" ItemStyle-Width="5%" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkMail" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                    <HeaderStyle ForeColor="#90ff06" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn Visible="False" DataField="UserID" HeaderText="UserID" HeaderStyle-Width="5%" ItemStyle-Width="5%" FooterStyle-Width="5%"></asp:BoundColumn>
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
    <script src="../js/jquery-1.11.0.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../js/jquery-ui.js"></script>
    <script type="text/javascript">
        SelectTab("User");
    </script>
    <%--Added by Koushik Das as on 04-Apr-2017--%>
    <script type="text/javascript">
        function confirmDelete() {
            var ret = confirm('This user is part of an active Approval Path.' +
                              '\nPlease make sure you also update the path accordingly' +
                              '\nand Reopen any invoices that are due to be approved by' +
                              '\n this user, to prevent invoices being unassigned.' +
                              '\nYou can find out which invoices need to be reopened' +
                              '\nby running a data download (tick Current Only).' +
                              '\n\nPlease contact support@p2dgroup.com for help.' +
                              '\nAre you sure you want to delete this user?');

            return ret;
        }
    </script>
    <%--Added by Koushik Das as on 04-Apr-2017--%>
    </form>
</body>
</html>
