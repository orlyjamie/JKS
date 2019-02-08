<%@ Page Language="c#" CodeFile="EditApproval.aspx.cs" AutoEventWireup="false" Inherits="JKS.EditApproval" %>

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
    <link href="../css/jquery-ui.css" rel="stylesheet">
    <link href="menu.css" rel="stylesheet">
    <script language="javascript">
        function ltrim(str)
        /***
        PURPOSE: Remove leading blanks from string.
        ***/
        {
            var whitespace = new String(" \t\n\r");
            var s = str; // new String(str);
            if (whitespace.indexOf(s.charAt(0)) != -1) {
                var j = 0, i = s.length;
                while (j < i && whitespace.indexOf(s.charAt(j)) != -1)
                    j++;
                s = s.substring(j, i);
            }
            return s;
        }

        function rtrim(str)
        /***
        PURPOSE: Remove trailing blanks from our string.
        ***/
        {
            var whitespace = new String(" \t\n\r");
            var s = str; //new String(str);
            if (whitespace.indexOf(s.charAt(s.length - 1)) != -1) {
                var i = s.length - 1;       // Get length of string
                while (i >= 0 && whitespace.indexOf(s.charAt(i)) != -1)
                    i--;
                s = s.substring(0, i + 1);
            }
            return s;
        }

        function trim(str)
        /***
        PURPOSE: Remove trailing and leading blanks from our string.
        ***/
        {
            return rtrim(ltrim(str));
        }

        //==============================================================================
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
        function fn_Validate() {
            //validation added after 28-Mar-2017
            //Add additional validation into the SAVE function if ‘Net From’ or ‘Net To’ 
            //is not populated “Please enter Net From and Net To”
            if ($("#<%=txtNetFrom.ClientID%>").val().length == 0) {
                alert("Please enter 'Net From' and 'Net To'.");
                $("#<%=txtNetFrom.ClientID%>").focus();
                return (false);
            }

            if ($("#<%=txtNetTo.ClientID%>").val().length == 0) {
                alert("Please enter 'Net From' and 'Net To'.");
                $("#<%=txtNetTo.ClientID%>").focus();
                return (false);
            }

            if ($("#<%=ddlCompany.ClientID%>").val() == 0) {
                alert("You must select a child company.");
                $("#<%=ddlCompany.ClientID%>").focus();
                return (false);
            }

            //validation added after 28-Mar-2017
            //Add additional validation into the SAVE function if ‘Net To’ is 
            //populated as 0. “Net To cannot be zero”...
            if (parseFloat($("#<%=txtNetTo.ClientID%>").val()) == 0) {
                alert("Net To cannot be zero.");
                $("#<%=txtNetTo.ClientID%>").focus();
                return (false);
            }

            if (trim(document.all.ddlApprover1[document.all.ddlApprover1.selectedIndex].text) == "Select"
				&& trim(document.all.ddlApprover2[document.all.ddlApprover2.selectedIndex].text) == "Select"
				&& trim(document.all.ddlApprover3[document.all.ddlApprover3.selectedIndex].text) == "Select"
				&& trim(document.all.ddlApprover4[document.all.ddlApprover4.selectedIndex].text) == "Select"
				&& trim(document.all.ddlApprover5[document.all.ddlApprover5.selectedIndex].text) == "Select"
				&& trim(document.all.ddlApprover6[document.all.ddlApprover6.selectedIndex].text) == "Select"
				&& trim(document.all.ddlApprover7[document.all.ddlApprover7.selectedIndex].text) == "Select"
				&& trim(document.all.ddlApprover8[document.all.ddlApprover8.selectedIndex].text) == "Select"
				&& trim(document.all.ddlApprover9[document.all.ddlApprover9.selectedIndex].text) == "Select"
				 ) {
                alert('Please select at least one approver.');
                return (false);
            }
            return (true);
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
                                    <asp:Label ID="Label1" runat="server" Width="100%">Approval Path</asp:Label></div>--%>
                                <div class="col-xs-12 col-sm-6">
                                    <table width="100%">
                                        <tr>
                                            <td style="height: 245px" valign="top" align="left">
                                                <table style="width: 784px; height: 336px">
                                                    <div align="center">
                                                        <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label></div>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 33px" align="left">
                                                            Company
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 33px" align="left">
                                                            <asp:DropDownList ID="ddlCompany" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="CompanyID" DataTextField="CompanyName" AutoPostBack="True" Style="color: red !important;">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="NormalBody" style="height: 33px; color: Red !important;">
                                                            Approver1
                                                        </td>
                                                        <td class="NormalBody" style="height: 33px" align="left">
                                                            <asp:DropDownList ID="ddlApprover1" TabIndex="1" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 28px" align="left">
                                                            Supplier Name
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 28px" align="left">
                                                            <asp:DropDownList ID="ddlSupplier" TabIndex="2" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="CompanyID" DataTextField="CompanyName" AutoPostBack="True" Visible="false">
                                                            </asp:DropDownList>
                                                            <input type="text" id="txtSupplier" runat="server" class="form-control inpit_select" />
                                                            <asp:HiddenField ID="HdSupplierId" runat="server" Value="" />
                                                        </td>
                                                        <td class="NormalBody" style="height: 28px">
                                                            Approver2
                                                        </td>
                                                        <td class="NormalBody" style="height: 28px" align="left">
                                                            <asp:DropDownList ID="ddlApprover2" TabIndex="3" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 29px" align="left">
                                                            Vendor Class
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 29px" align="left">
                                                            <asp:DropDownList ID="ddlVendor" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                                DataTextField="VendorClass" DataValueField="VendorClass">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="NormalBody" style="height: 29px">
                                                            Approver3
                                                        </td>
                                                        <td class="NormalBody" style="height: 29px" align="left">
                                                            <asp:DropDownList ID="ddlApprover3" TabIndex="4" runat="server" Width="" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 31px; color: Red !important;"
                                                            align="left" valign="middle">
                                                            Net From
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 31px;" align="left">
                                                            <asp:TextBox ID="txtNetFrom" runat="server" CssClass="form-control inpit_select"
                                                                TabIndex="5"></asp:TextBox>
                                                        </td>
                                                        <td class="NormalBody" style="height: 31px">
                                                            Approver4
                                                        </td>
                                                        <td class="NormalBody" style="height: 31px" align="left">
                                                            <asp:DropDownList ID="ddlApprover4" TabIndex="6" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 30px; color: Red !important;"
                                                            align="left" valign="middle">
                                                            Net To
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 30px" align="left">
                                                            <asp:TextBox ID="txtNetTo" runat="server" CssClass="form-control inpit_select" TabIndex="7"></asp:TextBox>
                                                        </td>
                                                        <td class="NormalBody" style="height: 30px">
                                                            Approver5
                                                        </td>
                                                        <td class="NormalBody" style="height: 30px" align="left">
                                                            <asp:DropDownList ID="ddlApprover5" TabIndex="8" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 28px" align="left">
                                                            Department
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 28px" align="left">
                                                            <asp:DropDownList ID="ddlDepartment" TabIndex="9" runat="server" CssClass="form-control inpit_select"
                                                                DataTextField="Department" DataValueField="DepartmentID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="NormalBody" style="height: 28px">
                                                            Approver6
                                                        </td>
                                                        <td class="NormalBody" style="height: 28px" align="left">
                                                            <asp:DropDownList ID="ddlApprover6" TabIndex="10" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 26px" align="left">
                                                            Business Unit
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 26px" align="left">
                                                            <asp:DropDownList ID="ddlBusinessUnit" TabIndex="11" runat="server" CssClass="form-control inpit_select"
                                                                DataTextField="BusinessUnitName" DataValueField="BusinessUnitID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="NormalBody" style="height: 26px">
                                                            Approver7
                                                        </td>
                                                        <td class="NormalBody" style="height: 26px" align="left">
                                                            <asp:DropDownList ID="ddlApprover7" TabIndex="12" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 28px" align="left">
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 28px" align="left">
                                                        </td>
                                                        <td class="NormalBody" style="height: 28px">
                                                            Approver8
                                                        </td>
                                                        <td class="NormalBody" style="height: 28px" align="left">
                                                            <asp:DropDownList ID="ddlApprover8" TabIndex="13" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="NormalBody" style="width: 104px; height: 31px" align="left">
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px; height: 31px" align="left">
                                                        </td>
                                                        <td class="NormalBody" style="height: 31px">
                                                            Approver9
                                                        </td>
                                                        <td class="NormalBody" style="height: 31px" align="left">
                                                            <asp:DropDownList ID="ddlApprover9" TabIndex="14" runat="server" CssClass="form-control inpit_select"
                                                                DataValueField="UserGroupID" DataTextField="UserGroupName">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="NormalBody" style="width: 104px" align="left">
                                                        </td>
                                                        <td class="NormalBody" style="width: 273px" align="left">
                                                        </td>
                                                        <td class="NormalBody">
                                                        </td>
                                                        <td class="NormalBody" align="left">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="NormalBody" style="width: 104px" align="left">
                                                        </td>
                                                        <td align="justify">
                                                            <a href="BrowseApprovalPath.aspx"></a>
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="sub_down btn-danger btn-group-justified"
                                                                Text="Cancel" BorderStyle="None" CausesValidation="False" BorderWidth="0px" Height="33px"
                                                                Style="background-color: Red;"></asp:Button> <%--Modified by Mainak 2018-11-29--%>
                                                        </td>
                                                        <td align="justify">
                                                            <asp:Button ID="btnSave" CssClass="sub_down btn-success btn-group-justified" Height="33px"
                                                                BorderStyle="None" Text="Save" runat="server" Style="background-color:Green;">
                                                            </asp:Button> <%--Modified by Mainak 2018-11-29--%>
                                                        </td>
                                                        <td class="NormalBody" align="left">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="NormalBody">
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="color: red ! important;">
                                        Mandatory
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
        $(function () {
            var ID;

            $("#txtSupplier").autocomplete({
                source: function (request, response) {

                    // var varCompanyID = e.options[e.selectedIndex].value;
                    var varCompanyID = document.getElementById("ddlCompany").value;
                    var usrString = document.getElementById('txtSupplier').value;

                    ID = this.element.attr("id");
                    var param = { CompanyID: varCompanyID, UserString: $('#txtSupplier').val() };
                    $.ajax({
                        url: "EditApproval.aspx/GetSupplier",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {

                            var IsValidSource = JSON.stringify(data.d);

                            if (IsValidSource == 'null') {
                                $(".ui-autocomplete").empty();
                                $(".ui-autocomplete").hide();
                            }

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('#')[1],
                                    CompanyID: item.split('#')[0]
                                }
                            }))
                        },

                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message)
                            // console.log("Ajax Error!");  
                        }
                    });
                },
                select: function (e, i) {
                    document.getElementById('<%=HdSupplierId.ClientID%>').value = i.item.CompanyID;

                },
                change: function (event, ui) {
                    if (!ui.item) {
                        document.getElementById('<%=HdSupplierId.ClientID%>').value = "";
                        document.getElementById('<%=txtSupplier.ClientID%>').value = "";
                    }
                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });        
    </script>
    </form>
</body>
</html>
