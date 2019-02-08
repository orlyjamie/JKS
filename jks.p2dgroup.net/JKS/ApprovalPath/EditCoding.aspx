<%@ Page Language="c#" CodeFile="EditCoding.aspx.cs" AutoEventWireup="false" Inherits="JKS.EditCoding" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html lang="en">
<head>
    <%-- --%>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.22/jquery-ui.js"></script>
    <link rel="Stylesheet" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/themes/redmond/jquery-ui.css" />
    <%-- --%>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta charset="utf-8">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta https-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;" />
    <meta name="description" content="">
    <meta name="author" content="">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="https://schemas.microsoft.com/intellisense/ie5">
    <title>P2D Network - Browse Users</title>
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="https://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
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
    <script language="javascript" src="js_common_function.js"></script>
    <script language="JavaScript" src="menu.js"></script>
    <script language="JavaScript" src="menu_items.js"></script>
    <script language="JavaScript" src="menu_tpl.js"></script>
    <script language="javascript">


        new menu(MENU_ITEMS, MENU_TPL);


        function ConfirmResubmit() {
            var x;
            var r = confirm('This code combination is already in the system. Please continue if you wish to change the Coding Description.');
            if (r == true) {
                document.getElementById("btnConfirm").click();
            }
            else {
            }
        }

        function ltrim(str) {
            var whitespace = new String(" \t\n\r");
            var s = str;
            if (whitespace.indexOf(s.charAt(0)) != -1) {
                var j = 0, i = s.length;
                while (j < i && whitespace.indexOf(s.charAt(j)) != -1)
                    j++;
                s = s.substring(j, i);
            }
            return s;
        }

        function rtrim(str) {
            var whitespace = new String(" \t\n\r");
            var s = str;
            if (whitespace.indexOf(s.charAt(s.length - 1)) != -1) {
                var i = s.length - 1;
                while (i >= 0 && whitespace.indexOf(s.charAt(i)) != -1)
                    i--;
                s = s.substring(0, i + 1);
            }
            return s;
        }

        function trim(str) {
            return rtrim(ltrim(str));
        }


        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
        function fn_Validate() {
            document.body.style.cursor = 'wait';

            var abc = trim(document.all.ddlVendor[document.all.ddlVendor.selectedIndex].text);

            if (document.all.ddlVendor.selectedIndex == 0) {
                alert('Please select vendor.');
                document.getElementById("ddlVendor").focus();
                return (false);
            }

            if (document.all.ddldept.selectedIndex == 0) {
                alert('Please select department.');
                document.getElementById("ddldept").focus();
                return (false);
            }
            if (document.all.ddlNominal.selectedIndex == 0) {
                alert('Please select nominal code.');
                document.getElementById("ddlNominal").focus();
                return (false);
            }
            var xyz = trim(document.all.ddlVendor[document.all.ddlVendor.selectedIndex].text);

            if (xyz == 'EXP') {
                if (document.getElementById("txtNetFrom").value == "") {
                    alert('Please insert NetFrom.');
                    document.getElementById("txtNetFrom").focus();
                    return (false);
                }
                if (isNaN(document.getElementById("txtNetFrom").value)) {
                    alert('Please insert numeric NetFrom.');
                    document.getElementById("txtNetFrom").focus();
                    return (false);
                }

                if (document.getElementById("txtNetTo").value == "") {
                    alert('Please insert NetTo.');
                    document.getElementById("txtNetTo").focus();
                    return (false);
                }
                if (isNaN(document.getElementById("txtNetTo").value)) {
                    alert('Please insert numeric NetTo.');
                    document.getElementById("txtNetTo").focus();
                    return (false);
                }
            }

            if (xyz == "EXP") {
                if (document.all.ddlApprover1.selectedIndex == 0) {
                    alert('Please select Approver1.');
                    document.getElementById("ddlApprover1").focus();
                    return (false);
                }

                if (document.all.ddlApprover2.selectedIndex != 0) {
                    if (document.all.ddlApprover1.selectedIndex == 0) {
                        alert('Please select Approver1.');
                        document.getElementById("ddlApprover1").focus();
                        return (false);
                    }
                }
                if (document.all.ddlApprover3.selectedIndex != 0) {
                    if (document.all.ddlApprover2.selectedIndex == 0) {
                        alert('Please select Approver2.');
                        document.getElementById("ddlApprover2").focus();
                        return (false);
                    }
                }
                if (document.all.ddlApprover4.selectedIndex != 0) {
                    if (document.all.ddlApprover3.selectedIndex == 0) {
                        alert('Please select Approver3.');
                        document.getElementById("ddlApprover3").focus();
                        return (false);
                    }
                }
                if (document.all.ddlApprover5.selectedIndex != 0) {
                    if (document.all.ddlApprover4.selectedIndex == 0) {
                        alert('Please select Approver4.');
                        document.getElementById("ddlApprover4").focus();
                        return (false);
                    }
                }
                if (document.all.ddlApprover6.selectedIndex != 0) {
                    if (document.all.ddlApprover5.selectedIndex == 0) {
                        alert('Please select Approver5.');
                        document.getElementById("ddlApprover5").focus();
                        return (false);
                    }
                }
                if (document.all.ddlApprover7.selectedIndex != 0) {
                    if (document.all.ddlApprover6.selectedIndex == 0) {
                        alert('Please select Approver6.');
                        document.getElementById("ddlApprover6").focus();
                        return (false);
                    }
                }
                if (document.all.ddlApprover8.selectedIndex != 0) {
                    if (document.all.ddlApprover7.selectedIndex == 0) {
                        alert('Please select Approver7.');
                        document.getElementById("ddlApprover7").focus();
                        return (false);
                    }
                }
                if (document.all.ddlApprover9.selectedIndex != 0) {
                    if (document.all.ddlApprover8.selectedIndex == 0) {
                        alert('Please select Approver8.');
                        document.getElementById("ddlApprover8").focus();
                        return (false);
                    }
                }
            }
            document.body.style.cursor = 'wait';

        }	
    </script>
</head>
<body onbeforeunload="javascript:doHourglass();" onunload="javascript:doHourglass();"
    ms_positioning="GridLayout">
    <form id="Form2" name="Form2" method="post" runat="server">
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
                              <asp:Label ID="Label1" runat="server" Width="100%">Add New Coding</asp:Label></div>--%>
                                <div align="center">
                                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label></div>
                                <div class="col-xs-12 col-sm-6">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-7 control-label label_text">
                                                Company
                                            </label>
                                            <div class="col-xs-12 col-sm-5">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlCompany" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        DataValueField="CompanyID" DataTextField="CompanyName" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-7 control-label label_text">
                                                Department
                                            </label>
                                            <div class="col-xs-12 col-sm-5">
                                                <div class="row">
                                                    <asp:TextBox ID="txtDept" runat="server" CssClass="form-control inpit_select" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-7 control-label label_text">
                                                Nominal
                                            </label>
                                            <div class="col-xs-12 col-sm-5">
                                                <div class="row">
                                                    <asp:TextBox ID="txtNominal" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-7 control-label label_text">
                                                Coding Description
                                            </label>
                                            <div class="col-xs-12 col-sm-5">
                                                <div class="row">
                                                    <asp:TextBox ID="txtCodingDes" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2" style="display: none">
                                            <label class="col-xs-12 col-sm-7 control-label label_text">
                                                Project / Capex Code
                                            </label>
                                            <div class="col-xs-12 col-sm-5">
                                                <div class="row">
                                                    <asp:TextBox ID="txtProject" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-7 control-label label_text">
                                                AP &amp; Admin Only
                                            </label>
                                            <div class="col-xs-12 col-sm-5">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlAPAdmin" runat="server" CssClass="form-control inpit_select">
                                                        <asp:ListItem Value="Select">Select</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-7 control-label label_text">
                                                &nbsp;
                                            </label>
                                            <div class="col-xs-12 col-sm-5">
                                                <div class="row">
                                                    <asp:Button ID="btnSubmit" CssClass="sub_down btn-primary btn-group-justified" runat="server"
                                                        Text="Submit" BorderStyle="None"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-xs-12 col-sm-7 control-label label_text">
                                                &nbsp;
                                            </label>
                                            <div class="col-xs-12 col-sm-5">
                                                <div class="row">
                                                    <a href="BrowseApprovalPath.aspx"></a>
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="sub_down btn-primary btn-group-justified"
                                                        BorderStyle="None" Text="Cancel" CausesValidation="False"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <asp:Label ID="lblConfirm" runat="server" Text="0" Visible="False"></asp:Label>
                                            <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" Style="visibility: hidden">
                                            </asp:Button>
                                        </div>
                                    </div>
                                    <table width="100%">
                                        <tr>
                                            <td style="height: 245px" valign="top" align="left">
                                                <a href="BrowseApprovalPath.aspx"></a>
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
    </div>
    <script src="../js/jquery-1.11.0.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../js/jquery-ui.js"></script>
    </form>
</body>
</html>
