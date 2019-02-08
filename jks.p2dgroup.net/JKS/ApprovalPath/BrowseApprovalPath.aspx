<%@ Page Language="c#" CodeFile="BrowseApprovalPath.aspx.cs" AutoEventWireup="false"
    Inherits="JKS.BrowseApprovalPath" %>

<%@ Register TagName="menuUserNL" TagPrefix="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html lang="en">
<head>
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
    <title>P2D Network - Administrator Approver</title>
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
    <script language="JavaScript" src="menu.js"></script>
    <script language="JavaScript" src="menu_items.js"></script>
    <script language="JavaScript" src="menu_tpl.js"></script>
    <link href="../css/jquery-ui.css" rel="stylesheet">
    <link href="menu.css" rel="stylesheet">
    <script language="javascript">
        function doHourglass() {
            document.body.style.cursor = 'wait';
        }	
    </script>
</head>
<body onbeforeunload="javascript:doHourglass();" onunload="javascript:doHourglass();"
    ms_positioning="GridLayout">
    <form id="Form2" method="post" runat="server">
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
                <menuUserNL:menuUserNL ID="MenuUserNL1" runat="server"></menuUserNL:menuUserNL>
                <div class="login_bg">
                    <div class="current_comp">
                        <div class="form-horizontal form_section">
                            <div class="row">
                                <div class="col-md-12">
                                    <%--<div class="PageHeader">
                                    <asp:Label ID="Label1" runat="server" Width="100%">Approval Path</asp:Label>
                                </div>--%>
                                    <div class="col-md-4">
                                        <div class="col-md-12">
                                            <div class="form-group form-group2">
                                                <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                    Company</label>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <asp:DropDownList ID="ddlCompany" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                            DataValueField="CompanyID" DataTextField="CompanyName" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group form-group2">
                                                <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                    Supplier Name</label>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <asp:DropDownList ID="ddlSupplier" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                            DataValueField="CompanyID" DataTextField="CompanyName" Visible="false">
                                                        </asp:DropDownList>
                                                        <input type="text" id="txtSupplier" runat="server" class="form-control inpit_select" />
                                                        <asp:HiddenField ID="HdSupplierId" runat="server" Value="" />
                                                        <asp:HiddenField ID="HdSupplierName" runat="server" Value="" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group form-group2">
                                                <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                    Vendor Class</label>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <asp:DropDownList ID="ddlVendor" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                            DataTextField="VendorClass" DataValueField="VendorClass">
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
                                                    &nbsp;
                                                </label>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <asp:Button ID="btnSearch" CssClass="sub_down btn-primary btn-group-justified" BorderStyle="None"
                                                            Text="Search" runat="server"></asp:Button>
                                                    </div>
                                                </div>
                                                <asp:Label ID="lblMessage" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
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
                                                    <asp:Button ID="btnCoding" CssClass="sub_down btn-primary btn-group-justified" BorderStyle="None"
                                                        Text="New Coding" runat="server"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                &nbsp;
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <input class="sub_down btn-primary btn-group-justified" type="button" value="Coding Flags"
                                                        onclick="window.open('CodingFlags.aspx','crnpopups','width=940,height=380,scrollbars=1,resizable=1');">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                &nbsp;
                                            </label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:Button ID="btnAddNew" CssClass="sub_down btn-primary btn-group-justified" runat="server"
                                                        Text="New Path" BorderStyle="None"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container" style="padding: 0 !important;">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:DataGrid ID="grdApprover" runat="server" AutoGenerateColumns="False" GridLines="None"
                                CellPadding="3" CssClass="listingArea" Width="100%">
                                <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle></ItemStyle>
                                <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="SupplierCompany" HeaderText="Supplier Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="New_VendorClass" HeaderText="Vendor Class"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="NetFrom" HeaderText="Net From"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="NetTo" HeaderText="Net To"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Department" HeaderText="Department"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="BusinessUnit" HeaderText="Business Unit"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ApproverPath" SortExpression="ApproverPaths" HeaderText="Approval Path">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ApproverPaths" SortExpression="ApproverPaths" Visible="False"
                                        HeaderText="Approval Path"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Edit">
                                        <HeaderStyle Width="50px"></HeaderStyle>
                                        <ItemTemplate>
                                            <a href='EditApproval.aspx?ApproverID=<%#DataBinder.Eval(Container.DataItem,"ApprovalPathsTableID")%>'
                                                class="grid_btn" borderstyle="None">Select </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Delete">
                                        <HeaderStyle HorizontalAlign="Left" Width="80px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                                        <ItemTemplate>
                                            <%-- <a id="lnkDelete" href='<%# DataBinder.Eval(Container, "DataItem.ApprovalPathsTableID") %>'
                                                onclick="return confirm('Are you sure that you want to delete?')" onserverclick="DeleteItem"
                                                runat="server" class="grid_btn"><strong>Delete</strong></a>--%>
                                            <%--Changed By Soumyajit On 9.3.15--%>
                                            <a id="lnkDelete" href='<%# DataBinder.Eval(Container, "DataItem.ApprovalPathsTableID") %>'
                                                onclick="return confirm('Are you sure that you want to delete?')" onserverclick="DeleteItem"
                                                runat="server" class="grid_btnDel"><strong>Delete</strong></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
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
                        url: "BrowseApprovalPath.aspx/GetSupplier",
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
    <script type="text/javascript">
        SelectTab("ApprovalPath");
    </script>
    </form>
</body>
</html>
