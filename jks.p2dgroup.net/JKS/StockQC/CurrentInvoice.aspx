<%@ Page language="c#" CodeFile="CurrentInvoice.aspx.cs" AutoEventWireup="false" 
Inherits="JKS.CurrentInvoice" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>

<!DOCTYPE html>
<html lang="en">
    <head>
    	<meta charset="utf-8">
    	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    	<meta name="viewport" content="width=device-width, initial-scale=1">
        <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;" />
    	<meta name="description" content="">
    	<meta name="author" content="">        
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
        <title>CurrentInvoice</title>
        <%--Added by kuntal karar on 1stApril2015--pt.47(b)-----%>
     <!----LightBox------>
      <link rel="stylesheet" href="../../LightBoxScripts/style.css" />
    <script type="text/javascript" src="../../LightBoxScripts/tinybox.js"></script>
    <!----->
		 <!-- Bootstrap core CSS -->
        <link href="../custom_css/bootstrap.min.css" rel="stylesheet">
        
        <!-- Custom Global Style -->
        <link href="../custom_css/screen.css" rel="stylesheet">
        <link href='https://fonts.googleapis.com/css?family=Lato:400,300italic,300,100italic,100,400italic,700,700italic,900,900italic' rel='stylesheet' type='text/css'>
        
        
        <!-- Custom Font Icon Style -->
        <link href="../custom_css/font-awesome.css" rel="stylesheet">
        <!-- Custom Responsive Style -->
        <link href="../custom_css/responsive.css" rel="stylesheet">
        
         <!-- Just for debugging purposes. Don't actually copy this line! -->
        <!--[if lt IE 9]><script src="js/ie8-responsive-file-warning.js"></script><![endif]-->
        
        <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
        <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
        <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
       
        <link href="../css/jquery-ui.css" rel="stylesheet">
        <script language="javascript">
            function PopupPic_INV(DocumentID, w, h) {
                var winl = (screen.width - w) / 2;
                var wint = (screen.height - h) / 2;
                winprops = 'resizable=1,height=' + h + ',width=' + w + ',top=' + wint + ',left=' + winl
                window.open("../invoice/ImgPopup_NL_INV.aspx?DocumentID=" + DocumentID, "Image", winprops)
            }

            function PopupPic_CN(DocumentID, w, h) {
                var winl = (screen.width - w) / 2;
                var wint = (screen.height - h) / 2;
                winprops = 'resizable=1,height=' + h + ',width=' + w + ',top=' + wint + ',left=' + winl
                window.open("../creditnotes/ImgPopup_NL_CN.aspx?DocumentID=" + DocumentID, "Image", winprops)
            }

            function fn_Validate() {
                if (document.all.ddlActionStatus.selectedIndex != 0 && document.all.ddlDocStatus.selectedIndex != 0) {
                    alert('Please select either doc status or action status.');
                    return (false);
                }
                document.body.style.cursor = 'wait';
            }
            function doHourglass() {
                document.body.style.cursor = 'wait';
            }		
		</script>
         
         
    </head>

	<body  onbeforeunload="javascript:doHourglass();" onunload="javascript:doHourglass();" >
		<form id="Form1" method="post" runat="server">
          <asp:Button ID="btnProcess" runat="server" Visible="False"></asp:Button>
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
            <uc1:menuUserNL id="MenuUserNL1" runat="server"></uc1:menuUserNL>
           <div class="login_bg">
                <div class="current_comp">
                    <div class="form-horizontal form_section">
                    <div class="row">
                                <div class="col-md-4">
                                <div class="col-md-12">
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                            Company Name</label>
                                        <div class="col-lg-7">
                                            <div class="row">
                                                <asp:DropDownList ID="ddlCompany" TabIndex="4" runat="server" CssClass="form-control inpit_select" AutoPostBack="True"
												DataValueField="CompanyID" DataTextField="CompanyName"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                            Supplier Name</label>
                                        <div class="col-lg-6">
                                            <div class="row">
                                                <asp:DropDownList ID="ddlSupplier" TabIndex="4" runat="server" CssClass="form-control inpit_select" DataValueField="CompanyID"
												DataTextField="CompanyName" Visible="false"></asp:DropDownList>
                                                <input type="text" id="txtSupplier" runat="server" class="form-control inpit_select" />
                                                <asp:HiddenField ID="HdSupplierId" runat="server" Value="" />    
                                                <asp:HiddenField ID="HdSupplierName" runat="server" Value="" />            
                                            </div>
                                        </div><input id="cbSupplier" runat="server" type="checkbox" style="margin-top:3px; float:left; margin-left:10px;" title="wildcard search" /> 
                                    </div>
                                    <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Vendor Class</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlVendorClass" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        AutoPostBack="True" DataValueField="New_VendorClass" DataTextField="New_VendorClass">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Doc Type</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlDocType" runat="server" CssClass="form-control inpit_select">
                                                        <asp:ListItem Value="" Selected="True">--Select Doc Type--</asp:ListItem>
                                                        <asp:ListItem Value="INV">Invoice</asp:ListItem>
                                                        <asp:ListItem Value="CRN">Credit Note</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    <div class="form-group form-group2" style="display: none">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                            Action Status</label>
                                        <div class="col-lg-7">
                                            <div class="row">                                                
                                                <asp:DropDownList ID="ddlActionStatus" TabIndex="4" runat="server"  CssClass="form-control inpit_select"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">Doc Status</label>
                                            <div class="col-lg-7">
                                            <div class="row"> 
                                            <asp:DropDownList ID="ddlDocStatus" TabIndex="4" runat="server" CssClass="form-control inpit_select" DataValueField="StatusID"
												DataTextField="Status"></asp:DropDownList>
                                             </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2">                                        
                                        <asp:label id="lblMessage" runat="server" CssClass="col-lg-12 control-label" ForeColor="Red" BorderStyle="None" ></asp:label>                                            
                                    </div>
                                </div>
                                </div>                                
                                <div class="col-md-4">
                                <div class="col-md-12">
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                           Doc No. </label>
                                        <div class="col-lg-6">
                                            <div class="row">
                                              <asp:textbox id="txtInvoiceNo" runat="server" CssClass="form-control inpit_select"></asp:textbox> 
                                              <asp:HiddenField ID="hdInvoiceNo" runat="server" Value="" />  
                                              <asp:HiddenField ID="hdInvoiceNoTxt" runat="server" Value="" />  
                                            </div>
                                        </div><input id="cbInvoiceNo"  runat="server" type="checkbox" style="margin-top:3px; float:left; margin-left:10px;" title="wildcard search"/>
                                    </div>
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                          PO No.</label>
                                        <div class="col-lg-7">
                                            <div class="row">
                                                <asp:textbox id="txtPONo" runat="server" CssClass="form-control inpit_select"></asp:textbox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Business Unit</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="form-control inpit_select">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                         Department</label>
                                        <div class="col-lg-7">
                                            <div class="row">
                                                <asp:DropDownList ID="ddldept" Runat="server"  CssClass="form-control inpit_select" AutoPostBack="True" DataValueField="DepartmentID"
												DataTextField="Department"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2">
                                            <label for="inputEmail" class="col-lg-5 control-label label_text">
                                                Nominal</label>
                                            <div class="col-lg-7">
                                                <div class="row">
                                                    <asp:DropDownList ID="ddlNominal" TabIndex="4" runat="server" CssClass="form-control inpit_select"
                                                        AutoPostBack="True" DataValueField="NominalCodeID" DataTextField="NominalName" Visible="false">
                                                    </asp:DropDownList>
                                                    <input type="text" id="txtNominal" runat="server" class="form-control inpit_select" />
                                                    <asp:HiddenField ID="hdNominalCodeId" runat="server" Value="" />  
                                                </div>
                                            </div>
                                        </div>
                                    
                                    <div class="form-group form-group2">
                                        <label for="inputEmail" class="col-lg-5 control-label label_text">
                                            &nbsp;</label>
                                         <div class="col-lg-7">
                                            <div class="row">
                                            <asp:Button ID="btnSearch"  Runat="server" CssClass="sub_down btn-primary btn-group-justified" Text="Search" BorderStyle="None" ></asp:Button>
                                             </div>
                                         </div>
                                    </div>
                                </div>
                                </div>
                                <div class="col-md-4">
                                <div class="col-md-12">
                                     <div class="form-group form-group2">
                                        <label  class="col-lg-5 control-label label_text">
                                          Doc Date From</label>
                                        <div class="col-lg-7">
                                            <div class="row  cal_img">
                                               <asp:TextBox id="txtFromDate" CssClass="form-control inpit_select" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="form-group form-group2">
                                        <label  class="col-lg-5 control-label label_text">
                                          Doc Date To</label>
                                        <div class="col-lg-7">
                                            <div class="row cal_img"">
                                               <asp:TextBox id="txtToDate" CssClass="form-control inpit_select" runat="server"></asp:TextBox>
                                               <asp:Label ID="lblMsg" runat="server" CssClass="MyInput" BorderStyle="None" ForeColor="Red" Visible="False"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="form-group form-group2">
                                        <label  class="col-lg-5 control-label label_text">
                                            Net Total From</label>
                                        <div class="col-lg-7">
                                            <div class="row">
                                                <asp:TextBox ID="textRange1" runat="server" class="form-control inpit_select"></asp:TextBox>
                                                <asp:Label ID="lblMsg1" runat="server" CssClass="MyInput" BorderStyle="None" ForeColor="Red" Visible="False"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group2">
                                        <label  class="col-lg-5 control-label label_text">
                                            Net Total To</label>
                                        <div class="col-lg-7">
                                            <div class="row">
                                               <asp:TextBox ID="textRange2" runat="server"  class="form-control inpit_select"></asp:TextBox>
                                            </div>
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
                        <div id="divP2DLogo" runat="server" class="p2d_logo" visible="false">
                                <asp:Image ID="imgP2DLogo" runat="server" ImageUrl="~/JKS/images/p2d_logo.png" ImageAlign="Middle" />
                                </div>

							<asp:datagrid id="grdInvCur" OnItemDataBound="grdInvCur_ItemDataBound" runat="server" AllowSorting="True" 
								AutoGenerateColumns="False" GridLines="None" CellPadding="3"
								BorderWidth="1px" BorderStyle="None" CssClass="listingArea" Width="100%" PageSize="15" 
                                AllowPaging="True">                               
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
							<ItemStyle></ItemStyle>
							<HeaderStyle CssClass="tableHd"></HeaderStyle>
							<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Doc No">
								<ItemTemplate>
									<%#Getredirecturl(DataBinder.Eval(Container.DataItem,"DocType"),DataBinder.Eval(Container.DataItem,"ReferenceNo"),DataBinder.Eval(Container.DataItem,"InvoiceID"))%>
								</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Pass To History" Visible="False">
								<ItemTemplate>
									<a href='#' onclick="<%#GetLogURL(DataBinder.Eval(Container.DataItem,"InvoiceID"))%>"> <b> Invoice Log History </b></A>
								</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Edit/Update" Visible="False">
								<ItemTemplate>
									<A href='#' onclick="<%#GetURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"),DataBinder.Eval(Container.DataItem,"Supplier"),DataBinder.Eval(Container.DataItem,"InvoiceDate"),DataBinder.Eval(Container.DataItem,"DocStatus"))%>"> <b> Edit/Update </b></A>
								</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Action">
								<ItemTemplate>
									<A href='#' onclick="<%#GetPOActionURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"),DataBinder.Eval(Container.DataItem,"Supplier"))%>"> <b> Action </b></A>
								</ItemTemplate>
								</asp:TemplateColumn>
												
								<asp:TemplateColumn HeaderText="Status History">
								<ItemTemplate>
                                <%--------Added by kuntal karar on 1stApril2015--pt.47(b)----------%>
                                               <a id="aStatusHistory" runat="server" href='#'><b>Status History </b></a>

									<%--<A href='#' onclick="<%#GetStatusURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"))%>"> <b> Status History </b></A>--%>
                                <%-------------------------------------------------------------------------%>
								</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="DocType" HeaderText="Doc Type"></asp:BoundColumn>
								<asp:BoundColumn DataField="ReferenceNo" Visible="false" HeaderText="Doc No."></asp:BoundColumn>
								<asp:BoundColumn DataField="Supplier" HeaderText="Supplier"></asp:BoundColumn>
								<asp:BoundColumn DataField="VendorCode" HeaderText="Vendor ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="DocStatus" HeaderText="Doc Status"></asp:BoundColumn>
								<asp:BoundColumn DataField="ActionStatus" HeaderText="Action Status" Visible="False"></asp:BoundColumn>												
					              <%--  <asp:TemplateColumn HeaderText="Payment Due Date">
						                <ItemTemplate>
							                <asp:Label runat="server" Text='<%# GetFormattedDate(DataBinder.Eval(Container.DataItem,"PaymentDueDate")) %>' ID="lblPaymentDueDate" >
							                </asp:Label>
						                </ItemTemplate>
					                </asp:TemplateColumn>	--%>	
                                    <asp:TemplateColumn HeaderText="Attachments">
                                            <ItemStyle HorizontalAlign="Left" Wrap="True" CssClass="Width110"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Repeater ID="rptAttachment" runat="server" OnItemDataBound="rptAttachment_ItemDataBound">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAttachmentImagePath" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem,"ImagePath")%>'></asp:Label>
                                                        <asp:Label ID="lblAttachmentArchiveImagePath" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem,"ArchiveImagePath")%>'></asp:Label>
                                                        <asp:Button ID="btnAttachment" runat="server" Text="Button" CssClass="repeaterItem"
                                                            CommandArgument='<%#DataBinder.Eval(Container.DataItem,"DocumentID")%>'  OnClick="btnrptAttachment_Click" />
                                                        <asp:Label ID="lblDocumentID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem,"DocumentID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>									
								<asp:BoundColumn DataField="Currency"  HeaderText="Currency"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="ParentRowFlag" HeaderText="ParentRowFlag"></asp:BoundColumn>
								<asp:BoundColumn DataField="Net" HeaderText="Net" ItemStyle-HorizontalAlign=Right></asp:BoundColumn>
								<asp:BoundColumn DataField="VAT" HeaderText="VAT" ItemStyle-HorizontalAlign=Right></asp:BoundColumn>
								<asp:BoundColumn DataField="Total" HeaderText="Gross" ItemStyle-HorizontalAlign=Right></asp:BoundColumn>
								<asp:BoundColumn DataField="Comment" HeaderText="Comment" Visible="False"></asp:BoundColumn>
												
								<asp:TemplateColumn HeaderText="Link To Image" Visible="False">
								<ItemTemplate>
									<a	href ='#' onclick="<%#GetDocumentWithPath(DataBinder.Eval(Container.DataItem,"DocAttachments"))%>"
									<b>Scanned Image</b></a>
								</ItemTemplate>
								</asp:TemplateColumn>
												
								<asp:BoundColumn Visible="False" DataField="InvoiceDate" SortExpression="InvoiceDate" HeaderText="Invoice Date"
								DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="DeliveryDate" SortExpression="DeliveryDate" HeaderText="Delivery Date"
								DataFormatString="{0:dd-MM-yyyy}"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Upload Docs">
									<ItemTemplate>
										<asp:HyperLink ID="hpDoc" Runat="server"><b>Upload</b></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>												
							<asp:TemplateColumn HeaderText="">
							<HeaderStyle ></HeaderStyle>
							<ItemStyle ></ItemStyle>											
							<ItemTemplate>												
							<A href='#' onclick="<%# GetAPCommentsURL(DataBinder.Eval(Container.DataItem,"InvoiceID"),DataBinder.Eval(Container.DataItem,"DocType"),DataBinder.Eval(Container.DataItem,"ReferenceNo"),DataBinder.Eval(Container.DataItem,"DocStatus"))%>"><img id="imgComment" runat="server" border="0"></A>
							</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
							Mode="NumericPages"></PagerStyle>
					</asp:datagrid>
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
        jQuery('#textRange1').on('keydown', function (e) {
            if (e.which >= 48 && e.which <= 57)
                return true;
            else if (e.which == 190)
                return true;
            else if (e.which >= 96 && e.which <= 105)
                return true;
            else if (e.which == 110)
                return true;
            else if (e.which == 8 || e.which == 46)
                return true;
            else
                return false;
        });
    </script>
    <script type="text/javascript">
        jQuery('#textRange2').on('keydown', function (e) {
            if (e.which >= 48 && e.which <= 57)
                return true;
            else if (e.which == 190)
                return true;
            else if (e.which >= 96 && e.which <= 105)
                return true;
            else if (e.which == 110)
                return true;
            else if (e.which == 8 || e.which == 46)
                return true;
            else
                return false;

        });

    </script>

    <script type="text/javascript">
        jQuery('#txtFromDate').on('keydown', function (e) {
            if (e.which == 8 || e.which == 46)
                return true;
            else
                return false;
        });
      </script>
    <script type="text/javascript">
        jQuery('#txtToDate').on('keydown', function (e) {
            if (e.which == 8 || e.which == 46)
                return true;
            else
                return false;
        });
      </script>

    <script type="text/javascript">
        $(document).ready(function () {
            Date.format = 'dd/mm/yyyy';
            $("#txtFromDate").datepicker({
                dateFormat: 'dd/mm/yy',
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,
                showOn: "button",
                buttonImage: "../images/DatePick.png",
                buttonImageOnly: true,
                buttonText: "Select date",
                onSelect: function (selected) {
                    $("#txtToDate").datepicker("option", "minDate", selected)
                }
            });
            $("#txtToDate").datepicker({
                dateFormat: 'dd/mm/yy',
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,
                showOn: "button",
                buttonImage: "../images/DatePick.png",
                buttonImageOnly: true,
                buttonText: "Select date",
                onSelect: function (selected) {
                    $("#txtFromDate").datepicker("option", "maxDate", selected)
                }
            });
        });

    </script>
   

    <script type="text/javascript">
        $(function () {
            var ID;

            $("#txtSupplier").autocomplete({
                source: function (request, response) {

                    var varCompanyID = document.getElementById("ddlCompany").value;
                    var usrString = document.getElementById('txtSupplier').value;
                    var varuserId = '<%= Session["userId"]%>';
                    var varUserTypeID = '<%= Session["UserTypeID"]%>';
                    ID = this.element.attr("id");
                    var param = { CompanyID: varCompanyID, userId: varuserId, userTypeID: varUserTypeID, UserString: $('#txtSupplier').val() };
                    $.ajax({
                        url: "CurrentInvoice.aspx/GetSupplier",
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
                    document.getElementById('<%=HdSupplierName.ClientID%>').value = i.item.label;
                },
                change: function (event, ui) {
                    var aCheckbox = document.getElementById('cbSupplier');
                    if (aCheckbox.checked) {
                        if (!ui.item) {
                            document.getElementById('<%=HdSupplierId.ClientID%>').value = "";
                        }

                    }
                    if ((document.getElementById('cbSupplier').checked == false) && (document.getElementById('<%=HdSupplierId.ClientID%>').value == "")) {
                        document.getElementById('<%=txtSupplier.ClientID%>').value = "";
                    }

                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });        
    </script>

    <script type="text/javascript">
        $(function () {
            var ID;

            $("#txtInvoiceNo").autocomplete({
                source: function (request, response) {

                    var varCompanyID = document.getElementById("ddlCompany").value;
                    var varDocType = document.getElementById("ddlDocType").value;
                    var usrString = document.getElementById('txtInvoiceNo').value;
                    ID = this.element.attr("id");

                    var param = { CompanyID: varCompanyID, DocType: varDocType, UserString: $('#txtInvoiceNo').val() };
                    $.ajax({
                        url: "CurrentInvoice.aspx/FetchInvoiceNo",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item
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
                    document.getElementById('<%=hdInvoiceNo.ClientID%>').value = i.item.label;
                    document.getElementById('<%=hdInvoiceNoTxt.ClientID%>').value = i.item.label;
                },
                change: function (event, ui) {
                    var aCheckbox = document.getElementById('cbInvoiceNo');
                    if (aCheckbox.checked) {
                        if (!ui.item) {
                            document.getElementById('<%=hdInvoiceNo.ClientID%>').value = "";
                        }

                    }
                    if ((document.getElementById('cbInvoiceNo').checked == false) && (document.getElementById('<%=hdInvoiceNo.ClientID%>').value == "")) {
                        document.getElementById('<%=txtInvoiceNo.ClientID%>').value = "";
                    }

                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });        
    </script>

    <script type="text/javascript">
        $(function () {
            var ID;

            $("#txtNominal").autocomplete({
                source: function (request, response) {

                    // var varCompanyID = e.options[e.selectedIndex].value;
                    var varCompanyID = document.getElementById("ddlCompany").value;
                    var usrString = document.getElementById('txtNominal').value;
                    ID = this.element.attr("id");
                    var param = { CompanyID: varCompanyID, UserString: $('#txtNominal').val() };
                    $.ajax({
                        url: "CurrentInvoice.aspx/GetNominalName",
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

                    document.getElementById('<%=hdNominalCodeId.ClientID%>').value = i.item.NominalCodeID;
                    //                     alert(document.getElementById('<%=hdNominalCodeId.ClientID%>').value);

                },
                change: function (event, ui) {
                    if (!ui.item) {
                        document.getElementById('<%=hdNominalCodeId.ClientID%>').value = "";
                        document.getElementById('<%=txtNominal.ClientID%>').value = "";
                    }
                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });        
    </script>

    <script type="text/javascript">
        SelectTab("Matching");
    </script>
		</form>
	</body>
</html>

