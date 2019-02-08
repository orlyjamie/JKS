<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solutions_Einvoicing.aspx.cs"
    Inherits="CBSolutions.ETH.Web.Solutions_Einvoicing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Invoice discounting removing costly and error prone data entry - P2D Group
    </title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;">
    <meta name="description" content="">
    <meta name="keyword" content="Invoice discounting">
    <meta name="author" content="P2D Group">
    <link rel="shortcut icon" href="../../assets/ico/favicon.ico">
    <!-- Bootstrap core CSS -->
    <link href="dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom Global Style -->
    <link href="custom_css/screen.css" rel="stylesheet">
    <link href='https://fonts.googleapis.com/css?family=Lato:400,300italic,300,100italic,100,400italic,700,700italic,900,900italic'
        rel='stylesheet' type='text/css'>
    <!-- Custom Font Icon Style -->
    <link href="custom_css/font-awesome.css" rel="stylesheet">
    <!-- Custom Responsive Style -->
    <link href="custom_css/responsive.css" rel="stylesheet">
    <!-- Custom FancyBox Style -->
    <link rel="stylesheet" type="text/css" href="custom_css/jquery.fancybox.css?v=2.1.5"
        media="screen">
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]>
<SCRIPT src="../../assets/js/ie8-responsive-file-warning.js"></SCRIPT>
<![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
<SCRIPT src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></SCRIPT>

<SCRIPT 
src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></SCRIPT>
<![endif]-->
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <!-------------- START: Site Wrapper ------------------------------------------------->
    <div class="site">
        <div class="container mainWrapper nopadding">
            <!------------------------------ START: Header ------------------------------>
            <header id="header">
						<div class="container">
							<!-------------------- START: Top Section -------------------->
							<div class="row h_top">
								<div class="col-lg-4 h_logo"><a href="Default.aspx" target="_self" title="P2D PAPER 2 DATA"><img src="assets/img/logo_h.png" alt=""></a></div>
								<!--START Header Top Links-->
								<div class="col-lg-8 h_top_links_area">
									<ul class="h_top_links">
										<li class="h_top_links_1st">
											<i class="fa fa-phone"></i>44 (0) 1189 255425</li>
										<li class="h_top_links_2nd noborder nopaddingRgt">
											<a class="fancybox" href="#inline1" title="LOGIN">LOGIN</a></li>
									</ul>
								</div>
								<!--START Login Popup Style-->
								<div id="inline1" class="containerInner pop-wrapper affix_600" style="DISPLAY: none">
									<div class="col-lg-12">
										<div class="modal-header">
											<h2 class="modal-title">USER LOG IN</h2>
										</div>
										<div class="modal-body clearfix">
											<div class="form-horizontal form_section">
												<div class="col-lg-12">
													<div class="form-group">
														<label for="inputEmail" class="col-lg-3 control-label">Network ID</label>
														<div class="col-lg-6">
															<asp:textbox id="txtNetworkID" runat="server" CssClass="form-control" MaxLength="25" EnableViewState="true"></asp:textbox>
														</div>
													</div>
													<div class="form-group">
														<label for="inputEmail" class="col-lg-3 control-label">Username</label>
														<div class="col-lg-6">
															<asp:textbox id="txtUserName" runat="server" CssClass="form-control" MaxLength="25" EnableViewState="true"></asp:textbox>
														</div>
													</div>
													<div class="form-group">
														<label for="inputPassword" class="col-lg-3 control-label">Password</label>
														<div class="col-lg-6">
															<asp:textbox id="txtPassword" runat="server" CssClass="form-control" MaxLength="25" EnableViewState="true"
																TextMode="Password"></asp:textbox>
														</div>
													</div>
												</div>
												<!--Contact Form 1st col-->
											</div>
										</div>
										<div class="modal-footer">
											<asp:Button ID="btnSalesLoginSubmit" runat="server" CssClass="btn btn-primary" Text="SUBMIT"></asp:Button>
										</div>
									</div>
								</div>
								<!--END Login Popup Style-->
								<!--END Header Top Links-->
							</div>
							<!-------------------- END: Top Section -------------------->
						</div>
					</header>
            <!------------------------------ END: Header ------------------------------>
            <!-------------------- START: Navigation -------------------->
            <div class="container">
                <div class="row">
                    <nav class="navbar navbar-inverse main_nav" role="navigation">
								<div class="navbar-header">
									<a href="javaScript:void(0);" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
										<span class="sr-only">Toggle navigation</span>
										<span class="icon-bar"></span>
										<span class="icon-bar"></span>
										<span class="icon-bar"></span>
									</a>
									<!--<a class="navbar-brand" href="#">Menu</a>-->
								</div>
								<div class="collapse navbar-collapse">
									<ul class="nav navbar-nav">
										<li>
											<a href="Default.aspx" title="HOME">HOME</a>
										<li>
											<a href="AboutUs.aspx" title="ABOUT US">ABOUT US</a>
										<li class="dropdown active">
											<a class="dropdown-toggle js-activated" data-toggle="dropdown" href="" title="SOLUTIONS">SOLUTIONS
												<span class="caret"></span></a>
											<ul class="dropdown-menu">
												<li class="active">
													<a href="javascript:void(0)" class="active" title="E-invoicing">E-invoicing</a>
												<li>
													<a href="Solutions_ISOCR.aspx" title="Invoice Scanning & OCR">Invoice Scanning &amp; OCR</a>
												<li>
													<a href="Solutions_Staff.aspx" title="Staff Expense Claims">Staff Expense Claims</a></li>
											</ul>
										<li>
											<a href="Resources.aspx" title="RESOURCES">RESOURCES</a>
										<li>
											<a href="Partner.aspx" title="PARTNERS">PARTNERS</a>
										<li>
											<a href="Customers.aspx" title="CUSTOMERS">CUSTOMERS</a>
										<li>
											<a href="News.aspx" title="NEWS">NEWS</a>
										<li>
											<a href="ContactUs.aspx" class="nobg" title="CONTACT US">CONTACT US</a></li>
									</ul>
								</div> <!--/.nav-collapse -->
							</nav>
                </div>
            </div>
            <!-------------------- END: Navigation -------------------->
            <!------------------------------ START: Banner Section ------------------------------>
            <section id="page">
						<div class="row">
							<div class="container">
								<div class="col-lg-12 ban_sub_wrapper">
									<img class="img_ban" src="assets/img/ban/ban_solutions_einvoicing.jpg" alt="Solutions::P2D Papper 2 Data">
									<h1>A GLOBAL E-INVOICING NETWORK</h1>
								</div>
							</div>
						</div>
					</section>
            <!------------------------------ END: Banner Section ------------------------------>
            <!------------------------------ START: Container Section ------------------------------>
            <!---------- START: TAB Section ---------->
            <section id="page">
						<div class="row">
							<div class="container-fluid">
								<!--START Tab Pills -->
								<div class="tabbable sidebar_pillsNav">
									<div id="side" class="nav-pillswrapper col-md-3 height_justified">
										<ul class="nav nav-pills nav-stacked">
											<li class="active">
												<a href="#tab_a" data-toggle="pill" class="tab-01" title="OVERVIEW">OVERVIEW<span class="li_arrow"></span></a>
											<li>
												<a href="#tab_b" data-toggle="pill" class="tab-02" title="BUSINESS CASE">BUSINESS CASE<span class="li_arrow"></span></a></li>
										</ul>
									</div>
									<div id="main" class="tab-content col-md-9 height_justified">
										<div class="tab-pane fade active in" id="tab_a">
											<div class="contentInner">
												<p>
													The <a href="http://www.p2dgroup.net" target="_blank">P2D Document Network</a> is the only solution of its kind in the world. It is a 
													revolutionary concept that combines state-of-the-art electronic trading and 
													document imaging technologies.
													<br>
													<br>
													Using award-winning technology, it is a global network that offers any size of 
													company the ability to exchange documents with its trading partners 
													electronically, without the barriers of capital expenditure. It enables members 
													to fully automate their document processing functions regardless of whether 
													those documents are in paper or electronic form, and does not require any 
													changes to their internal systems. Members receive the benefits of shared 
													technology, paying for usage only, and can scale their solution to include a 
													variety of document types and e-procurement features over time.
												</p>
											</div>
										</div>
										<div class="tab-pane fade" id="tab_b">
											<div class="contentInner">
												<h3>E-INVOICING HUB</h3>
												<ul>
													<li>
														E-invoicing, generically, <strong>removes manual data entry, queries and thus 
															resource cost.</strong>
													<li>
														In addition, the P2D hub approach <strong>removes the huge administrative cost of 
															recruiting suppliers</strong>
													(contacting, enrolling, integrating with, and providing post-live support for) 
													which would need to be performed internally with direct customer-supplier 
													e-invoicing relationships.
													<li>
														The P2D solution <strong>removes the need to invest in an ERP e-invoicing module,</strong>
													but can equally work alongside it to sweep up residual paper flow.
													<li>
														ERP e-invoicing normally requires the supplier to send XML, but not all 
														companies can do so, particularly smaller ones. By allowing suppliers to send 
														invoices electronically to the hub in a variety of ways leads to <strong>enhanced 
															supplier adoption.</strong>
													<li>
														Integration in a format / protocol of the client's choice, performed 
														outside-the-firewall with no changes required to back-end systems, <strong>significantly 
															reduces implementation costs.</strong>
													<li>
														Invoices can be pre-validated for duplicates, correct mathematics, order 
														numbers/product codes etc before submission, which <strong>further reduces manual 
															intervention and resource cost.</strong></li>
												</ul>
											</div>
										</div>
									</div>
								</div> <!-- /.tabbable -->
							</div>
						</div>
					</section>
            <!---------- END: TAB Section ---------->
            <div class="tab-content_01">
                <!---------- START: First Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="container bg_cover bg_row_solutions_5th om-animation-enabled">
										<div class="contentInner txt_white">
											The P2D Document Network is an electronic exchange for documents such as 
											invoices, orders, remittance advices and so on, and automates manual processing 
											with a secure, and tax compliant service. It allows documents to be transmitted 
											directly between suppliers' and buyers' systems - removing costly and error 
											prone data entry, without the installation of hardware or software. Buyers also 
											have the option to process paper invoices over the network too, with our 
											scanning module that turns 'paper to data'. This way, companies can crystallize 
											the benefits immediately. Therefore it allows a managed plan of creating the 
											fully paperless office, in terms of document management, document imaging and 
											in turn electronic document exchange.
											<br>
											<br>
											As an answer to the high cost and difficulty of enrolling trading partners 
											(especially those with low volumes) into traditional EDI methods, the P2D 
											Document Network receives invoices in a number of different standard formats 
											(edifact, xml, csv etc) from which the sender can choose the most appropriate 
											to their business. It then reformats the data to the recipient's specific 
											system requirements and integrates those electronic documents directly into its 
											back-end systems down a secure pipe. Documents can be released directly to 
											internal workflow if required, or workflow can be developed within the hub for 
											approvals etc.
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: First Section ---------->
                <!---------- START: Second Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="contentInner clearfix">
										<div class="col-xs-6">
											<h3>FEATURES</h3>
											<ul>
												<li>
												E-invoicing Hub
												<li>
												Integrated scanning &amp; intelligent OCR
												<li>
												Bespoke workflows &amp; document matching
												<li>
												Supplier Recruitment Programme
												<li>
												Supplier Portal
												<li>
												PO flips
												<li>
												Powered by SAP Netweaver
												<li>
												On a certified 'Cisco-powered' network
												<li>
												Industry-strength security &amp; encryption
												<li>
												Tax compliant, global solution
												<li>
												Full audit capabilities
												<li>
												'Outside-the-firewall' integration
												<li>
													Negligible impact on internal IT resource</li>
											</ul>
										</div>
										<div class="col-xs-6">
											<h3>BENEFITS</h3>
											<ul>
												<li>
												No investment spend
												<li>
												Immediate cost savings
												<li>
												Instant processing
												<li>
												Reduced queries &amp; disputes
												<li>
												Avoidance of lost invoices
												<li>
												Improved cashflow management
												<li>
												Established and growing network
												<li>
												Strengthened trading relationships
												<li>
												Improved reporting, forecasting &amp; governance
												<li>
												Leverage current systems &amp; processes
												<li>
												Scalable for other document types and modules
												<li>
													Scalable for use with other customers</li>
											</ul>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: Second Section ---------->
            </div>
            <div class="tab-content_02" style="display: none;">
                <!---------- START: First Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="container bg_cover bg_row_solutions_6th om-animation-enabled">
										<div class="contentInner txt_white">
											<h3>PARALLEL SCANNING / OCR</h3>
											<ul class="list_whitecolor">
												<li>
												Web forms, CSV/XML file uploads, bespoke integrations or free software plug-ins 
												for Sage/Pegasus/QuickBooks.
												<li>
													The hub provides embedded intelligent OCR, requiring no fine-tuning or onsite 
													installation, which is capable of detailed line extraction out-of-the-box. This <strong>
														removes the need to invest in a separate OCR solution.</strong>
												<li>
												Parallel OCR scanning fully automates the data capture on all invoices and thus 
												delivers a solid business case and cost savings, rather than relying on 
												estimates of how many suppliers and what associated invoice volume will engage 
												in e-invoicing.
												<li>
													Invoices can be scanned onsite, or via a bureau; and the QC module (to check 
													extracted data against the images) is deployed over the web; which <strong>provides 
														the flexibility to control processes</strong>
												according to clients' preferences.
												<li>
													Supporting documents can be scanned with invoices, and images are hosted 
													offsite which <strong>removes the need to purchase and support servers.</strong></li>
											</ul>
											<br>
											<h3>DOCUMENT MANAGEMENT</h3>
											<ul class="list_whitecolor">
												<li>
													The hub provides online e-invoice and image retrieval, configured access 
													rights, and the ability to attach supporting documents to invoices, which <strong>removes 
														the need to invest in a separate document management system</strong>
												for the Finance Department. Images can also be retrieved from within the ERP 
												system.
												<li>
													The hub's inbuilt document management system, of course <strong>provides 
														governance, compliance and audit benefits.</strong> The system is approved 
													by HMRC, and naturally satisfies the requirements of, for example, Sarbanes 
													Oxley. This is further reinforced if workflow is also adopted, providing a 
													consolidated view of the approval/process status of every invoice.</li>
											</ul>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: First Section ---------->
                <!---------- START: Second Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="container">
										<div class="contentInner">
											<h3>WORKFLOW / MATCHING</h3>
											<ul>
												<li>
													Where invoices do not have purchase orders, and need manual approval, P2D 
													builds bespoke electronic workflow within the hub to facilitate this, if 
													required. In addition to the functionalities mentioned above, this further <strong>enhances 
														the ability to meet supplier payment SLAs</strong> and <strong>achieve early 
														payment discounts.</strong>
												<li>
													P2D also can develop <strong>automated matching solutions to deliver additional 
														resource/cost reduction.</strong>
												<li>
													By building bespoke anciliary process solutions this <strong>removes the need to 
														invest in separate workflow solutions;</strong>
												or develop and support them within the ERP.
												<li>
													Bespoke development also allows companies to <strong>optimize processes</strong>
													according to how they need to operate, rather than being shoe-horned into a 
													prescriptive way of working as is often the case with off-the-shelf solutions 
													and ERP modules. This leads to <strong>greater control of operational risk.</strong></li>
											</ul>
											<br>
											<h3>COMMERCIAL</h3>
											<ul>
												<li>
													The P2D solution is charged entirely on a low pay-as-you-go transaction fee 
													basis which typically <strong>provides immediate and considerable cash savings.</strong>
												<li>
													Moreover, the charges are payable only upon go live which <strong>removes 
														commercial risk.</strong></li>
											</ul>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: Second Section ---------->
                <!---------- START: Three Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="bg_color_lightGrey">
										<div class="container">
											<div class="contentInner clearfix">
												<h3>SUPPLIER PORTAL</h3>
												<ul>
													<li>
														Providing suppliers with online access to invoice approvals audit trail and 
														payment status <strong>reduces queries and resource cost</strong> for both the 
														Finance Department and internal buyers.</li>
												</ul>
												<br>
												<h3>SCALABILITY</h3>
												<ul>
													<li>
														The solution can be used for the distribution of remittance advices, sales 
														invoices and orders: such <strong>scalability can further reduce costs.</strong></li>
												</ul>
												<br>
												<h3>SUPPLIER BENEFITS</h3>
												<ul>
													<li>
														By adopting e-invoicing, suppliers can strengthen long-term relationships and 
														also win new customers leading to <strong>enhanced future revenue.</strong>
													<li>
														Suppliers can choose from a variety of methods to send invoices, as defined 
														above, which <strong>removes the need to invest in changing their systems</strong>
													to service client e-invoicing requests.
													<li>
														The hub is an open exchange which, through a single connection, <strong>removes 
															considerable duplication and cost</strong>
													of servicing multiple client requests for e-invoicing.
													<li>
														Suppliers benefit from immediate, guaranteed and confirmed delivery of 
														invoices, and can track the approval and payment status of invoices online, 
														leading to <strong>improved cashflow management and governance.</strong>
													<li>
														According to Forrester Research a sales invoice typically costs £1.29 to send 
														to a customer. Suppliers would <strong>achieve cash savings</strong> using the 
														P2D system. To augment this, suppliers can use the system to send invoices 
														electronically to all customers, regardless of whether they are on the P2D 
														network.</li>
												</ul>
											</div>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: Three Section ---------->
            </div>
            <!------------------------------ END: Container Section ------------------------------>
            <!------------------------------ START: Footer ------------------------------>
            <footer id="footer">
						<!--START: Footer Top-->
						<div class="container f_top">
							<div class="col-xs-6 col-sm-3 f_col f_col_first">
								<h3>LINKS</h3>
								<ul class="f_links">
									<li>
										<a href="Default.aspx" title="Home"><i class="glyphicon glyphicon-play"></i>Home</a></li>
									<li>
										<a href="AboutUs.aspx" title="About Us"><i class="glyphicon glyphicon-play"></i>About Us</a></li>
									<li>
										<a href="javascript:void(0)" class="current" title="Solutions"><i class="glyphicon glyphicon-play"></i>
											Solutions</a></li>
									<li>
										<a href="Resources.aspx" title="Resources"><i class="glyphicon glyphicon-play"></i>Resources</a></li>
									<li>
										<a href="Partner.aspx" title="Partners"><i class="glyphicon glyphicon-play"></i>Partners</a></li>
									<li>
										<a href="Customers.aspx" title="Customers"><i class="glyphicon glyphicon-play"></i>Customers</a></li>
									<li>
										<a href="News.aspx" title="News"><i class="glyphicon glyphicon-play"></i>News</a></li>
									<li>
										<a href="ContactUs.aspx" title="Contact Us"><i class="glyphicon glyphicon-play"></i>Contact Us</a></li>
								</ul>
							</div> <!--Footer Links-->
							<div class="col-xs-6 col-sm-3 f_col f_col_second">
								<h3>CONTACT US</h3>
								<address>Soane Point, 6-8 Market Place
									<br>
									Reading, RG1 2EG</address>
								<!--<abbr title="Telephone">--> Tel: &nbsp; &nbsp;<!--</abbr>--> <a href="tel:+01189255425">
									01189 255 425</a>
								<br>
								Mail: &nbsp; <a href="mailto:info@p2dgroup.com">info@p2dgroup.com</a>
							</div> <!--Footer Contact Us-->
							<div class="col-xs-6 col-sm-3 f_col f_col_third">
								<h3>LOCATE US</h3>
								<p>
									<iframe width="221" height="77" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"
										src="https://maps.google.co.in/maps?f=q&amp;source=s_q&amp;hl=en&amp;geocode=&amp;q=Soane+Point,+6-8+Market+Place+Reading,+RG1+2EG&amp;aq=&amp;sll=22.675949,88.368056&amp;sspn=0.701989,1.352692&amp;ie=UTF8&amp;hq=Soane+Point,&amp;hnear=8+Market+Pl,+Reading+RG1,+United+Kingdom&amp;ll=51.455596,-0.969208&amp;spn=0.007407,0.021136&amp;t=m&amp;z=14&amp;iwloc=A&amp;cid=1257220563783862559&amp;output=embed"></iframe>
								</p>
							</div> <!--Footer Locate Us-->
							<div class="col-xs-6 col-sm-3 f_col f_col_fourth">
								<h3>FOLLOW US</h3>
								<ul class="f_social">
									<li>
										<a href="https://www.linkedin.com/company/p2d-uk-limited?trk=company_name" title="Linkedin"><i class="fa fa-linkedin">
											</i>Linkedin</a></li>
									<li>
										<a href="https://twitter.com/@RobinColla_P2D" title="Twitter"><i class="fa fa-twitter"></i>Twitter</a></li>
								</ul>
								<p><img src="assets/img/f_logo.jpg" title="P2D" alt=""></p>
							</div> <!--Footer Follow Us-->
						</div>
						<!--END: Footer Top-->
						<!--START: Footer Bottom-->
						<div class="container f_bottom">
							<div class="col-xs-6 f_col">© 2015 paper2data. All Rights Reserved</div>
							<div class="col-xs-6 f_col text-right">
								Design and Developed by <a href="http://vnsinfo.com.au/">Vision &amp; Solutions Pty 
									Ltd</a>
							</div>
						</div>
						<!--END: Footer Bottom-->
					</footer>
            <!------------------------------ END: Footer ------------------------------>
        </div>
        <!--END: Container-->
    </div>
    <!-------------- END: Site Wrapper ------------------------------------------------->
    <!-- Bootstrap core JavaScript
        ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="js/jquery-1.11.0.min.js"></script>
    <script src="dist/js/bootstrap.min.js"></script>
    <script src="js/bootstrap-hover-dropdown.min.js"></script>
    <script type="text/javascript" src="js/modernizr.custom.99711.js"></script>
    <!--Logo Scroll Js-->
    <!--<script src="assets/js/jquery.flexisel.js"></script>-->
    <!-- Add fancyBox main JS -->
    <script type="text/javascript" src="js/jquery.fancybox.pack.js?v=2.1.5"></script>
    <!--Tab Js-->
    <script src="js/tab.js"></script>
    <script type="text/javascript">
        $(window).load(function () {
            $("#logoscroll").flexisel({
                visibleItems: 4,
                animationSpeed: 1000,
                autoPlay: false,
                autoPlaySpeed: 3000,
                pauseOnHover: true,
                enableResponsiveBreakpoints: true,
                responsiveBreakpoints: {
                    portrait: {
                        changePoint: 480,
                        visibleItems: 1
                    },
                    landscape: {
                        changePoint: 640,
                        visibleItems: 1
                    },
                    tablet: {
                        changePoint: 768,
                        visibleItems: 1
                    }
                }
            });

        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            /*Fancy Popups*/
            $('.fancybox').fancybox();

            /*Navigation Style*/
            $('.js-activated').dropdownHover().dropdown();

            $(".tab-01").click(function () {
                $('.tab-content_01').removeClass('hide');
                $(".tab-content_01").fadeIn(1000);
                $('.tab-content_02').addClass('hide');
            });
            $(".tab-02").click(function () {
                $('.tab-content_02').removeClass('hide');
                $(".tab-content_02").fadeIn(1500);
                $('.tab-content_01').addClass('hide');
            });


            $("#testimonialCarousel").carousel({
                interval: 8000,
                pause: false,
                wrap: true
            });

        });
    </script>
    </form>
</body>
</html>
