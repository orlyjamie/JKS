<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>

<%@ Page Language="c#" CodeFile="EditSupplier.aspx.cs" AutoEventWireup="false" Inherits="JKS.EditSupplier" %>

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
    <title>P2D Network - Edit Supplier</title>
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

    <script language="javascript" src="js_common_function.js"></script>

    <link href="../css/jquery-ui.css" rel="stylesheet">

    <script language="javascript">

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
            debugger;
            document.body.style.cursor = 'wait';
            if (document.getElementById("txtSupplier").value == "") {
                alert('Please insert supplier company name.');
                document.getElementById("txtSupplier").focus();
                return (false);
            }
            //            if (document.getElementById("txtAddress1").value == "") {
            //                alert('Please insert address1.');
            //                document.getElementById("txtAddress1").focus();
            //                return (false);
            //            }
            //            if (document.getElementById("thtPostCode").value == "") {
            //                alert('Please insert postcode.');
            //                document.getElementById("thtPostCode").focus();
            //                return (false);
            //            }
            //            if (trim(document.all.ddlCountry[document.all.ddlCountry.selectedIndex].text) == "Select Country") {
            //                alert('Please select country.');
            //                document.getElementById("ddlCountry").focus();
            //                return (false);
            //            }

            if (document.getElementById("TxtVendorID").value == "") {
                alert('Please insert vendorid.');
                document.getElementById("TxtVendorID").focus();
                return (false);
            }
            if (document.getElementById("thtVendorClass").value == "") {
                alert('Please insert vendor class.');
                document.getElementById("thtVendorClass").focus();
                return (false);
            }
            if (document.getElementById("txtEmail").value != "") {
                var email = /^[a-zA-Z0-9_\.\-]+@[a-zA-Z0-9\-]+\.[a-zA-Z0-9\-\.]+$/;
                if (!email.test(document.getElementById("txtEmail").value)) {
                    alert("Please enter valid format for email.");
                    document.getElementById("txtEmail").focus();
                    return false;
                }
            }
            //Commented By Mainak,2018-11-6
//            if (document.all.ddlNominalCode1[document.all.ddlNominalCode1.selectedIndex].text == "Select Stock NominalCode"
//					&& document.all.ddlNominalCode2[document.all.ddlNominalCode2.selectedIndex].text == "Select EXP NominalCode") {
//                alert('Please select at least one default Nominal Code.');
//                return false;
//            }
            //Added By Mainak, 2018-11-6
            var ddlDepartment = document.getElementById("ddlExpDept");
            if (ddlDepartment.value == "0") {
                //If the "Please Select" option is selected display error.
                alert("Please select Department");
                return false;
            }

            if (document.getElementById("ddlPreApprove").options[document.getElementById("ddlPreApprove").selectedIndex].value == "1") {
                if (document.getElementById("ddlNominalCode2").options[document.getElementById("ddlNominalCode2").selectedIndex].value == "0") {
                    alert('EXP Nominal must be selected if EXP Pre-Approve is Yes.');
                    return false;
                }
            }
            return true;
            document.body.style.cursor = 'wait';

        }	
    </script>

</head>
<body onbeforeunload="javascript:doHourglass();" onunload="javascript:doHourglass();"
    ms_positioning="GridLayout">

    <script language="javascript" src="wz_tooltip.js"></script>

    <form id="Form2" name="Form2" method="post" runat="server">
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
                <uc1:menuusernl id="MenuUserNL1" runat="server"></uc1:menuusernl>
                <div class="login_bg">
                    <div class="current_comp fixed_height">
                        <div class="form-horizontal form_section">
                            <div class="row">
                                <%--<div class="PageHeader">
                                    <asp:Label ID="Label1" runat="server" Width="100%">Add New Supplier</asp:Label>
                                </div>--%>
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
                                <asp:Label ID="Label2" runat="server"></asp:Label>
                                <div class="col-xs-12 col-sm-6">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Company Name <font color="red">*</font></label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtSupplier" TabIndex="1" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Address1
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtAddress1" TabIndex="3" runat="server" CssClass="form-control inpit_select"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Address2
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtAddress2" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Address3
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtAddress3" TabIndex="5" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Post Code
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="thtPostCode" TabIndex="6" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                County
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlCounty" TabIndex="6" runat="server" CssClass="form-control inpit_select"
                                                        DataTextField="County" DataValueField="CountyID">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Country
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlCountry" TabIndex="7" runat="server" CssClass="form-control inpit_select"
                                                        DataTextField="Country" DataValueField="CountryID">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Telephone No
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtTelephone" TabIndex="8" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Email ID
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtEmail" TabIndex="9" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Vat No
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="txtVatNo" TabIndex="10" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <%-----------------------Added by Mainak 2018-09-21------------------------- --%>
                                        <div style="height:28px;" class="form-group form-group2" >
                                            
                                        </div>
                                        <%------------------------------------------------------------------------------------------------%>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                &nbsp;
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                               <%-- Commented By Mainak ,2018-11-6--%>
                                                    <%--<asp:Button ID="btnSave" CssClass="sub_down btn-primary btn-group-justified" runat="server"
                                                        Text="Save" BorderStyle="None"></asp:Button><a href="BrowseApprovalPath.aspx"></a>--%>
                                                        <%-- Added By Mainak ,2018-11-6--%>
                                                        <a href="BrowseSupplier.aspx" style="background-color: #ff0000;" class="sub_down btn-primary btn-group-justified center_alin">
                                                        Cancel</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6">
                                    <div class="col-md-12">
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Buyer Company <font color="#ff0000">*</font>
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlCompany" TabIndex="2" runat="server" CssClass="form-control inpit_select"
                                                        AutoPostBack="True" DataTextField="CompanyName" DataValueField="CompanyID" Style="color: red !important;">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Vendor ID <font color="#ff0000">*</font>
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="TxtVendorID" TabIndex="11" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Vendor Class <font color="#ff0000">*</font><a href="#"><span onmouseover="Tip('If you are adding a new vendor class for a STOCK supplier you must please inform P2D at <u>support@p2dgroup.com </u>', FADEIN, 1000, PADDING, 10);">
                                                    [i]</span></a>
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="thtVendorClass" TabIndex="12" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Vendor Group
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:TextBox ID="thtVendorGroup" TabIndex="13" runat="server" CssClass="form-control inpit_select"
                                                        MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <%--    Moved here by kuntal karar on 03.03.2015 from below section--%>
                                            <asp:TextBox ID="txtApCompanyID" TabIndex="14" CssClass="form-control inpit_select"
                                                runat="server" MaxLength="50" Visible="False"></asp:TextBox>
                                            <%--------------------------------------------------------------%>
                                        </div>
                                        <%---------------------------Modified by kuntal karar on 03.03.2015 ----------------------%>
                                        <%--  <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                AP CompanyID
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                   
                                                </div>
                                            </div>
                                        </div>--%>
                                        <%-------------------------------------------------------------------------------%>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Account Currency
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="cboCurrencyType" runat="server" CssClass="form-control inpit_select">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Stock Nominal
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlNominalCode1" runat="server" CssClass="form-control inpit_select"
                                                        Visible="false">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNominal1" runat="server" class="form-control inpit_select" />
                                                    <asp:HiddenField ID="hdNominalCodeId1" runat="server" Value="" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                EXP Nominal
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlNominalCode2" runat="server" CssClass="form-control inpit_select"
                                                        Visible="false">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNominal2" runat="server" class="form-control inpit_select" />
                                                    <asp:HiddenField ID="hdNominalCodeId2" runat="server" Value="" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Active
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlStatus" TabIndex="15" runat="server" CssClass="form-control inpit_select">
                                                        <asp:ListItem Value="1" Selected="True">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                EXP Pre-Approve
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlPreApprove" TabIndex="15" runat="server" CssClass="form-control inpit_select">
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <%-----------------------Added by Kuntal Karar on 03.03.2015------------------------- --%>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Stock Approval Needed
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlFnBNeed" TabIndex="15" runat="server" CssClass="form-control inpit_select">
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <%------------------------------------------------------------------------------------------------%>
                                        <%-----------------------Added by Mainak 2018-09-21------------------------- --%>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                Department <font color="#ff0000">*</font>
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlExpDept" TabIndex="16" runat="server" CssClass="form-control inpit_select">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <%------------------------------------------------------------------------------------------------%>
                                        <div class="form-group form-group2">
                                            <label class="col-lg-5 control-label label_text">
                                                &nbsp;
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                               <%-- Commented By Mainak,2018-11-6--%>
                                                    <%--<a href="BrowseSupplier.aspx" class="sub_down btn-primary btn-group-justified center_alin">
                                                        Cancel</a>--%>
                                                        <%-- Added By Mainak ,2018-11-6--%>
                                                        <asp:Button ID="btnSave" CssClass="sub_down btn-primary btn-group-justified" runat="server"
                                                        Text="Save" BorderStyle="None" style="background-color: #3cbc3c;" ></asp:Button><a href="BrowseApprovalPath.aspx"></a>
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
        </div>
    </div>

    <script src="../js/jquery-1.11.0.min.js"></script>

    <script src="../js/bootstrap.min.js"></script>

    <script src="../js/jquery-ui.js"></script>

    <script type="text/javascript">
        $(function () {
            var ID;

            $("#txtNominal1").autocomplete({
                source: function (request, response) {

                    // var varCompanyID = e.options[e.selectedIndex].value;
                    var varCompanyID = document.getElementById("ddlCompany").value;
                    var usrString = document.getElementById('txtNominal1').value;
                    ID = this.element.attr("id");
                    var param = { CompanyID: varCompanyID, UserString: $('#txtNominal1').val() };
                    $.ajax({
                        url: "EditSupplier.aspx/GetNominalName",
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
                                    NominalCodeID: item.split('#')[0]
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

                    document.getElementById('<%=hdNominalCodeId1.ClientID%>').value = i.item.NominalCodeID;
                    //                     alert(document.getElementById('<%=hdNominalCodeId1.ClientID%>').value);

                },
                change: function (event, ui) {
                    if (!ui.item) {
                        document.getElementById('<%=hdNominalCodeId1.ClientID%>').value = "";
                        document.getElementById('<%=txtNominal1.ClientID%>').value = "";
                    }
                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });        
    </script>

    <script type="text/javascript">
        $(function () {
            var ID;

            $("#txtNominal2").autocomplete({
                source: function (request, response) {

                    // var varCompanyID = e.options[e.selectedIndex].value;
                    var varCompanyID = document.getElementById("ddlCompany").value;
                    var usrString = document.getElementById('txtNominal2').value;
                    ID = this.element.attr("id");
                    var param = { CompanyID: varCompanyID, UserString: $('#txtNominal2').val() };
                    $.ajax({
                        url: "EditSupplier.aspx/GetNominalName",
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
                                    NominalCodeID: item.split('#')[0]
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

                    document.getElementById('<%=hdNominalCodeId2.ClientID%>').value = i.item.NominalCodeID;
                    //                     alert(document.getElementById('<%=hdNominalCodeId2.ClientID%>').value);

                },
                change: function (event, ui) {
                    if (!ui.item) {
                        document.getElementById('<%=hdNominalCodeId2.ClientID%>').value = "";
                        document.getElementById('<%=txtNominal2.ClientID%>').value = "";
                    }
                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });        
    </script>

    </form>
</body>
</html>
