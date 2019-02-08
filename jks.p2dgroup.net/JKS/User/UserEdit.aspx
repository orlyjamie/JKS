<%@ Page Language="c#" CodeFile="UserEdit.aspx.cs" AutoEventWireup="false" Inherits="JKS.UserEdit" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html lang="en">
<head>
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
    <title>P2d Network - User Add/Edit</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <!-- Bootstrap core CSS -->
    <link href="../custom_css/bootstrap.min.css" rel="stylesheet" />
    <!-- Custom Global Style -->
    <link href="../custom_css/screen.css" rel="stylesheet" />
    <link href='https://fonts.googleapis.com/css?family=Lato:400,300italic,300,100italic,100,400italic,700,700italic,900,900italic'
        rel='stylesheet' type='text/css' />
    <!-- Custom Font Icon Style -->
    <link href="../custom_css/font-awesome.css" rel="stylesheet" />
    <!-- Custom Responsive Style -->
    <link href="../custom_css/responsive.css" rel="stylesheet" />
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="../js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet" />
    <script language="javascript">
        function trim(s) {
            while (s.substring(0, 1) == ' ') {
                s = s.substring(1, s.length);
            }
            while (s.substring(s.length - 1, s.length) == ' ') {
                s = s.substring(0, s.length - 1);
            }
            return s;
        }
        function OnLoadCheckGMG() {
            if ('<%=ViewState["NewLook"]%>' == '1') {
                document.getElementById("trUserType").style.display = "";
                document.getElementById("trCompany").style.display = "";
            }
            else {
                document.getElementById("trUserType").style.display = "none";
                document.getElementById("trCompany").style.display = "none";
            }
        }
        function Validation1() {
            if (document.getElementById("ddlChildBuyerCompany2").selectedIndex == 0) {
                alert('Please Buyer Company');
                return false;
            }
            if (document.getElementById("ddlBusinessUnit").selectedIndex == 0) {
                alert('Please Business Unit');
                return false;
            }
            return true;

        }
        function Validation2() {
            if (document.getElementById("ddlChildBuyerCompany").selectedIndex == 0) {
                alert('Please Buyer company');
                return false;
            }
            return true;

        }


        // ==============added by kuntalkarar on 1stJune2016===========================================================
        function ValidateFormSubmission() {



            if (trim(document.Form2.txtPassword.value).length <= 0) {
                alert('Please enter current password.');
                return (false);
            }



            if (trim(document.all.txtPassword.value).length < 8) {
                alert('Please enter at-least 8 characters for password.');
                return (false);
            }
            re = /[0-9]/;
            if (!re.test(document.Form2.txtPassword.value)) {
                alert("Error: password must contain at least one number (0-9)!");
                return false;
            }
            re = /[a-z]/;
            if (!re.test(document.Form2.txtPassword.value)) {
                alert("Error: password must contain at least one lowercase letter (a-z)!");
                return false;
            }
            re = /[A-Z]/;
            if (!re.test(document.Form2.txtPassword.value)) {
                alert("Error: password must contain at least one uppercase letter (A-Z)!");
                return false;
            }


            return (true);

        }
        // =========================================================================



        function ValidatePassword() {
            document.body.style.cursor = 'wait';

            if (document.getElementById('hdnUserType').value == "1") {
                if (document.getElementById('ddlGroup').value == '0') {
                    //alert('Please select group.');
                    //document.getElementById("ddlGroup").focus();
                    //return (false);
                }

                if (document.getElementById('ddlDepartment').value == '0') {
                    /*
                    if(document.getElementById('hdnDeptExst').value=='0')
                    {
                    alert('Please select department.');
                    document.getElementById("ddlDepartment").focus();
                    return (false);
                    }
                    */
                }
            }
            if (trim(document.all.txtUserCode.value) == '') {
                alert('Please enter a user Code.');
                document.getElementById("txtUserCode").focus();
                return (false);
            }
            else if (trim(document.all.txtUserName.value) == '') {
                alert('Please enter user name.');
                document.getElementById("txtUserName").focus();
                return (false);
            }
            else if (trim(document.all.txtUserName.value).length > 15) {
                alert('user name cannot be more than 15 characters.');
                document.getElementById("txtUserName").focus();
                return (false);
            }
            else if (trim(document.all.txtPassword.value) == '') {
                alert('Please enter password.');
                document.getElementById("txtPassword").focus();
                return (false);
            }
            /*else if (trim(document.all.txtPassword.value).length < 6) {
            alert('Password cannot be less than 6 characters.');
            document.getElementById("txtPassword").focus();
            return (false);
            }*/

            /*---Commeneted By Rimi on 31stAusgust2015 for removing 10Characters validation--*/

            /* else if (trim(document.all.txtPassword.value).length > 10) {
            alert('Password cannot be more than 10 characters.');
            document.getElementById("txtPassword").focus();
            return (false);
            }*/

            var email = /^[a-zA-Z0-9_\.\-]+@[a-zA-Z0-9\-]+\.[a-zA-Z0-9\-\.]+$/;
            if (!email.test(document.getElementById("txtEmail").value)) {
                alert("Please enter valid format for email.");
                document.getElementById("txtEmail").focus();
                return false;
            }
            else if (trim(document.all.UserTypeDropDown.value) == '0') {
                alert('Please select a user type.');
                document.getElementById("UserTypeDropDown").focus();
                return (false);
            }
            else if (document.all.UserTypeDropDown.value == '4') {
                alert('Sorry, you are not authorised to add a mangement user.');
                document.getElementById("UserTypeDropDown").focus();
                return (false);
            }
            else {
                return (true);
            }
        }
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
        function CheckCompany() {

            if (document.getElementById("ddlBuyerCompany").selectedIndex == 0) {
                alert('Please select company');
                return false;
            }
            return true;
        }

        function ValidAPUserCode() {

            if (document.getElementById("ddlGroup").value != 'G1') {
                //alert('AP users can only select G1 approvers');
                //return false;
            }
            return true;
        }
    </script>
</head>
<body onbeforeunload="javascript:doHourglass();" onunload="javascript:doHourglass();">
    <form id="Form2" method="post" runat="server">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
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
                <asp:Button ID="btnCloseAction" runat="server" Visible="False"></asp:Button>
                <div class="login_bg">
                    <div class="current_comp">
                        <div class="form-horizontal form_section">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="PageHeader blue_headinggap" colspan="2">
                                        <asp:Label ID="lblHeader" runat="server" Width="100%">Label</asp:Label>
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server" CssClass="MyInput" ForeColor="Red" BorderStyle="None"></asp:Label>
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Role</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtRole" TabIndex="1" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Group <font color="red"><span id="spanGroup" runat="server"></span></font>
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlGroup" runat="server" DataTextField="UserGroupName" DataValueField="UserGroupName"
                                                        CssClass="form-control inpit_select" TabIndex="2">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                User Code <font color="red">*</font></label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtUserCode" TabIndex="3" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                User Name <font color="red">*</font></label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtUserName" TabIndex="4" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Password <font color="red">*</font></label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtPassword" TabIndex="5" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="100" TextMode="Password"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                First Name
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtFirstName" TabIndex="6" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Surname
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtSurname" TabIndex="7" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Telephone
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtTelephone" TabIndex="8" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Mobile
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtMobile" TabIndex="9" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Email <font color="red">*</font></label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtEmail" TabIndex="10" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <label class="col-lg-5 control-label label_text">
                                                Role <font color="red">*</font></label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="cboRole" TabIndex="7" runat="server" CssClass="form-control inpit_select">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                User Type <font color="red">*</font></label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="UserTypeDropDown" TabIndex="11" runat="server" CssClass="form-control inpit_select"
                                                        DataValueField="UserTypeID" DataTextField="UserType">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <label class="col-lg-5 control-label label_text">
                                                Company
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlSubCompany" TabIndex="8" runat="server" CssClass="form-control inpit_select"
                                                        DataTextField="CompanyName" DataValueField="CompanyID">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Edit Rights? <font color="red">*</font></label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="drpEditInvRightDropDown" TabIndex="12" runat="server" CssClass="form-control inpit_select">
                                                        <asp:ListItem Value="0"> No </asp:ListItem>
                                                        <asp:ListItem Value="1"> Yes</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <font color="red">* = Mandatory Field </font>
                                        <div class="form-group form-group2">
                                            <div class="col-lg-6">
                                                <div class="row">
                                                    <a onclick="javascript:window.location.href='UserBrowse.aspx';" href="#" style="width: 100px"
                                                        class="sub_down2 btn-primary btn-group-justified center_alin txt_white">Back</a>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="row">
                                                    <asp:Button ID="btnSubmit" TabIndex="13" Text="Submit" runat="server" CssClass="sub_down3 btn-primary btn-group-justified"
                                                        Style="width: 100px"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="col-xs-12 col-sm-12  control-label "
                                                ControlToValidate="txtPassword" ValidationExpression="(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"
                                                ErrorMessage="Password must contain a minimum of: 8 characters, 1 upper case letter, 1 lower case letter and 1 number"
                                                ForeColor="Red" />
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="up1" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-3">
                                            <div class="PageHeader blue_headinggap" colspan="2">
                                                <asp:Label ID="Label2" runat="server" Width="100%">Add Company </asp:Label>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        Company <font color="red">*</font></label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlChildBuyerCompany" runat="server" DataTextField="CompanyName"
                                                                DataValueField="CompanyID" CssClass="form-control inpit_select">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        &nbsp;</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:Button ID="ingAddCmpany" runat="server" class="sub_down3 btn-primary btn-group-justified center_alin"
                                                                Text="Add Company" BorderStyle="None" Style="width: 182px;"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <asp:DataGrid ID="grdCompany" runat="server" CssClass="listingArea" AutoGenerateColumns="False"
                                                        GridLines="Vertical" CellPadding="2">
                                                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                                        <ItemStyle></ItemStyle>
                                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <%-- ------Modified by Kuntal karar on 23.03.2015----pt.51------%>
                                                                    <%--class="grid_btn"--%>
                                                                    <asp:Button ID="imgBtnDeleteCompany" runat="server" CssClass="grid_btnDel" Text="Delete"
                                                                        BorderStyle="None" CommandName="DELETE" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"CompanyID")%>'>
                                                                    </asp:Button>
                                                                    <%----------------------------------------------------------------------%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="CompanyID" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUserCompanyID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.CompanyID") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Company">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CompanyName") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                                        </PagerStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </div>
                                            <!-- New Addition--->
                                            <div class="PageHeader blue_headinggap" colspan="2" style="margin-top: 10px;">
                                                <asp:Label ID="lblPreventAccessTo" runat="server" Width="100%">Prevent Access To:</asp:Label>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        Vendor Class <font color="red">*</font></label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlVendorClass" runat="server" DataValueField="New_VendorClass"
                                                                DataTextField="New_VendorClass" CssClass="form-control inpit_select">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        &nbsp;</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:Button ID="btnAddVendorClass" runat="server" class="sub_down3 btn-primary btn-group-justified center_alin"
                                                                Text="Add Vendor Class" BorderStyle="None" Style="width: 182px;"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <asp:DataGrid ID="grdVendorClass" runat="server" CssClass="listingArea" AutoGenerateColumns="False"
                                                        GridLines="Vertical" CellPadding="2">
                                                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                                        <ItemStyle></ItemStyle>
                                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <%-- ------Modified by Kuntal karar on 23.03.2015----pt.51------%>
                                                                    <%--class="grid_btn" --%>
                                                                    <asp:Button ID="imgBtnDeleteVendorClass" runat="server" CssClass="grid_btnDel" Text="Delete"
                                                                        BorderStyle="None" CommandName="DELETE" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"VendorClass")%>'>
                                                                    </asp:Button>
                                                                    <%-----------------------------------------------------------------------%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="UserID" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVendorClassUserID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.UserID") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Vendor Class">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVendorClass" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VendorClass") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                                        </PagerStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="PageHeader blue_headinggap" colspan="2">
                                                <asp:Label ID="Label1" runat="server" Width="100%">Add Department</asp:Label>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group form-group2">
                                                    <label class="col-lg-5 control-label label_text">
                                                        Company <font color="red">*</font></label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlBuyerCompany" runat="server" AutoPostBack="True" DataTextField="CompanyName"
                                                                DataValueField="CompanyID" CssClass="form-control inpit_select">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label class="col-lg-5 control-label label_text">
                                                        Department <font color="red"><span id="spanDepartment" runat="server">*</span></font></label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlDepartment" runat="server" DataTextField="Department" DataValueField="DepartmentID"
                                                                CssClass="form-control inpit_select">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        &nbsp;</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:Button ID="imgAddDept" runat="server" class="sub_down3 btn-primary btn-group-justified center_alin"
                                                                Text="Add Department" BorderStyle="None" Style="width: 182px;"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group">
                                                    <asp:DataGrid ID="grdUser" runat="server" CssClass="listingArea" AutoGenerateColumns="False"
                                                        GridLines="Vertical" CellPadding="2">
                                                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                                        <ItemStyle></ItemStyle>
                                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <%-- ------Modified by Kuntal karar on 23.03.2015----pt.51------%>
                                                                    <%-- class="grid_btn" --%><asp:Button ID="imgBtnDelete" runat="server" CssClass="grid_btnDel"
                                                                        Text="Delete" BorderStyle="None" CommandName="DELETE" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"DepartmentID")%>'>
                                                                    </asp:Button>
                                                                    <%------------------------------------------------------------------------%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Department" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDepartmentID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DepartmentID") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Department">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Department") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Company">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CompanyName") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                                        </PagerStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="PageHeader blue_headinggap" colspan="2">
                                                <asp:Label ID="Label5" runat="server" Width="100%">Add Business Unit </asp:Label>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group form-group2">
                                                    <label class="col-lg-6 control-label label_text">
                                                        Company
                                                        <%--<font color="red">*</font>--%></label>
                                                    <div class="col-lg-6">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlChildBuyerCompany2" runat="server" AutoPostBack="True" DataTextField="CompanyName"
                                                                DataValueField="CompanyID" CssClass="form-control inpit_select">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label class="col-lg-6 control-label label_text">
                                                        Business Unit
                                                        <%--<font color="red">*</font>--%></label>
                                                    <div class="col-lg-6">
                                                        <div class="row">
                                                            <asp:DropDownList ID="ddlBusinessUnit" runat="server" DataTextField="BusinessUnitName"
                                                                DataValueField="BusinessUnitID" CssClass="form-control inpit_select">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                        &nbsp;</label>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <asp:Button ID="imgBusinessUnit" runat="server" class="sub_down3 btn-primary btn-group-justified center_alin"
                                                                Text="Add Business Unit" BorderStyle="None" Style="width: 182px;"></asp:Button></div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group2">
                                                    <asp:DataGrid ID="grdBusinessUnit" runat="server" CssClass="listingArea" AutoGenerateColumns="False"
                                                        GridLines="Vertical" CellPadding="2">
                                                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                                        <ItemStyle></ItemStyle>
                                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <%-- ------Modified by Kuntal karar on 23.03.2015----pt.51------%>
                                                                    <%-- class="grid_btn"--%>
                                                                    <asp:Button ID="imgBtnDeleteBusinessUnit" runat="server" CssClass="grid_btnDel" Text="Delete"
                                                                        AlternateText="Add Business Unit" BorderStyle="None" CommandName="DELETE" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"BusinessUnitID")%>'>
                                                                    </asp:Button>
                                                                    <%--------------------------------------------------------------------%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Business Unit" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBusinessUnitID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BusinessUnitID") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Business Unit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBusinessUnitName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BusinessUnitName") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Company">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCompanyName2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CompanyName") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                                        </PagerStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Main Content Panel Ends-->
                <input id="hdGridFlag" type="hidden" value="0" name="hdGridFlag" runat="server">
                <input id="hdnUserType" style="width: 24px; height: 17px" type="hidden" size="1"
                    value="0" runat="server" name="hdnUserType"><input id="hdnDeptExst" style="width: 24px;
                        height: 17px" type="hidden" size="1" value="0" name="Hidden1" runat="server"></TD>
                <script type="text/javascript" src="../js/jquery-1.11.0.min.js"></script>
                <script type="text/javascript" src="../js/bootstrap.min.js"></script>
                <script type="text/javascript" src="../js/jquery-ui.js"></script>
                <%--<script type="text/javascript">
                    $(document).ready(function () {
                        setTimeout(function () {
                            $("#<%=txtUserName.ClientID%>").val("");
                            $("#<%=txtPassword.ClientID%>").val("");
                        }, 100);
                    });
                </script>--%>
                <%--added by kuntalkarar on 20thSeptember2016--%>
                <script type="text/javascript">
                    if ('<%=Session["UserEditClick_kk"]%>' == 'False') {
                        $(document).ready(function () {
                            setTimeout(function () {
                                //alert("test");
                                $("#<%=txtUserName.ClientID%>").val("");
                                $("#<%=txtPassword.ClientID%>").val("");
                            }, 100);
                        });
                    }
                    else {

                    }
                </script>
                <%------------------------------------------------------%>
    </form>
</body>
</html>
