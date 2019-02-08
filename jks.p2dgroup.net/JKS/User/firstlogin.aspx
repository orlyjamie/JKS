<%@ Page Language="c#" CodeFile="firstlogin.aspx.cs" AutoEventWireup="false" Inherits="JKS.firstlogin" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
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
    <title>P2D Network - Company Add/Edit</title>
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
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="../js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        // =========================================================================
        function trim(s) {
            while (s.substring(0, 1) == ' ') {
                s = s.substring(1, s.length);
            }
            while (s.substring(s.length - 1, s.length) == ' ') {
                s = s.substring(0, s.length - 1);
            }
            return s;
        }
        // =========================================================================
        function ValidateFormSubmission() {
            if (trim(document.all.tbNewPassword.value).length < 6) {
                alert('Please enter at-least 6 characters for password.');
                return (false);
            }
            else if (trim(document.all.tbCurrentPassword.value) == trim(document.all.tbNewPassword.value)) {
                alert('New password cannot be same as old password.');
                return (false);
            }
            else {
                return (true);
            }
        }
        // =========================================================================
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form2" method="post" runat="server" onsubmit="javascript:return ValidateFormSubmission();">
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
                                <%--<div class="PageHeader">
                                    <asp:Label ID="lblHeader" runat="server" CssClass="PageHeader" Width="100%">Change Password</asp:Label></div>--%>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6">
                                            <div class="col-md-12">
                                                <div class="form-group form-group2">
                                                    <label class="col-xs-12 col-sm-7 control-label label_text">
                                                        User Name (case sensitive) <font color="red">&nbsp;*</font>
                                                    </label>
                                                    <div class="col-xs-12 col-sm-5">
                                                        <div class="row">
                                                            <asp:TextBox ID="tbUserName" TabIndex="1" runat="server" CssClass="form-control inpit_select"
                                                                MaxLength="20">
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfv_UserName" runat="server" ErrorMessage="Please enter user name."
                                                                Display="Dynamic" ControlToValidate="tbUserName"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label class="col-xs-12 col-sm-7 control-label label_text">
                                                        Current Password <font color="red">&nbsp;*</font>
                                                    </label>
                                                    <div class="col-xs-12 col-sm-5">
                                                        <div class="row">
                                                            <asp:TextBox ID="tbCurrentPassword" TabIndex="1" runat="server" CssClass="form-control inpit_select"
                                                                MaxLength="100" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfv_CurrentPassword" runat="server" ErrorMessage="Please enter current password."
                                                                Display="Dynamic" ControlToValidate="tbCurrentPassword"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label class="col-xs-12 col-sm-7 control-label label_text">
                                                        New Password<font color="red">&nbsp;*</font>
                                                    </label>
                                                    <div class="col-xs-12 col-sm-5">
                                                        <div class="row">
                                                            <asp:TextBox ID="tbNewPassword" TabIndex="2" runat="server" CssClass="form-control inpit_select"
                                                                MaxLength="100" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfv_NewPassword" runat="server" ControlToValidate="tbNewPassword"
                                                                Display="Dynamic" ErrorMessage="Please enter new password."></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label class="col-xs-12 col-sm-7 control-label label_text">
                                                        Confirm Password<font color="red">&nbsp;*</font>
                                                    </label>
                                                    <div class="col-xs-12 col-sm-5">
                                                        <div class="row">
                                                            <asp:TextBox ID="tbConfirmPassword" TabIndex="2" runat="server" CssClass="form-control inpit_select"
                                                                MaxLength="100" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfv_ConfirmPassword" runat="server" ControlToValidate="tbNewPassword"
                                                                Display="Dynamic" ErrorMessage="Please enter new password."></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cmv_Password" runat="server" ControlToValidate="tbConfirmPassword"
                                                                Display="Dynamic" ErrorMessage="Confirm password does not match with new password."
                                                                ControlToCompare="tbNewPassword"></asp:CompareValidator>
                                                            <input id="hdProceedFlag" type="hidden" value="0" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label class="col-xs-12 col-sm-7  control-label label_text">
                                                        &nbsp;
                                                    </label>
                                                    <div class="col-xs-12 col-sm-5">
                                                        <div class="row">
                                                            <asp:Button ID="btnSubmit" TabIndex="6" runat="server" Text="Submit" CssClass="sub_down btn-primary btn-group-justified">
                                                            </asp:Button>
                                                            <span id="spProceed" style="display: none">
                                                                <%-- <a href="../Current/CurrentStatus.aspx">--%>
                                                                <%--<a href="UserMain.aspx">
                                                                <img id="imgProceed" alt="" src="..\..\images\proceed.gif" name="imgProceed" border="0">
                                                            </a>--%>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label class="col-xs-12 col-sm-7  control-label label_text">
                                                        <font color="red">* = Mandatory Field</font>
                                                    </label>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <asp:Label ID="lblMessage" runat="server" class="col-xs-12 col-sm-7  control-label "
                                                        ForeColor="Red" style="font-weight: bold;"></asp:Label> <%--Style added by Mainak 2018-03-15--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6">
                                            <div>
                                                &nbsp;</div>
                                            <div>
                                                &nbsp;
                                            </div>
                                            <div>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbNewPassword"
                                                    ValidationExpression="(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"
                                                    ErrorMessage="Password must contain a minimum of: 8 characters, 1 upper case letter, 1 lower case letter and 1 number"
                                                    ForeColor="Red" />
                                            </div>
                                            <div>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tbConfirmPassword"
                                                    ValidationExpression="(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"
                                                    ErrorMessage="Password must contain a minimum of: 8 characters, 1 upper case letter, 1 lower case letter and 1 number"
                                                    ForeColor="Red" />
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
        <%-- <script language="javascript">
            if (document.all.hdProceedFlag.value == '1') {                
                document.getElementById("spProceed").style.display = "";                
            }
        </script>--%>
    </form>
</body>
</html>
