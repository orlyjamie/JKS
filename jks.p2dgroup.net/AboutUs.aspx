<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AboutUs.aspx.cs" Inherits="CBSolutions.ETH.Web.AboutUs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;" />
    <meta name="description" content="">
    <meta name="keyword" content="Outsourcing solutions,Expenses software">
    <meta name="author" content="P2D Group">
    <link rel="shortcut icon" href="../../assets/ico/favicon.ico">
    <title>Outsourcing solutions|Expenses software - P2D Group</title>
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
        media="screen" />
    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="../../assets/js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
</head>
<body>
    <form id="Form1" method="post" runat="server">
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
											<a href="Default.aspx" title="HOME">HOME</a></li>
										<li class="active">
											<a href="javascript:void(0)" title="ABOUT US">ABOUT US</a></li>
										<li class="dropdown">
											<a class="dropdown-toggle js-activated" data-toggle="dropdown" href="" title="SOLUTIONS">SOLUTIONS
												<span class="caret"></span></a>
											<ul class="dropdown-menu">
												<li>
													<a href="Solutions_Einvoicing.aspx" title="E-invoicing">E-invoicing</a>
                                                    </li>
												<li>
													<a href="Solutions_ISOCR.aspx" title="Invoice Scanning & OCR">Invoice Scanning &amp; OCR</a>
                                                    </li>
												<li>
													<a href="Solutions_Staff.aspx" title="Staff Expense Claims">Staff Expense Claims</a>
                                                    </li>
											</ul>
                                            </li>
										<li>
											<a href="Resources.aspx" title="RESOURCES">RESOURCES</a></li>
										<li>
											<a href="Partner.aspx" title="PARTNERS">PARTNERS</a></li>
										<li>
											<a href="Customers.aspx" title="CUSTOMERS">CUSTOMERS</a></li>
										<li>
											<a href="News.aspx" title="NEWS">NEWS</a></li>
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
									<img class="img_ban" src="assets/img/ban/ban_about.jpg" alt="About Us::P2D Papper 2 Data">
									<h1>Award-Winning Cloud Solutions</h1>
								</div>
							</div>
						</div>
					</section>
            <!------------------------------ END: Banner Section ------------------------------>
            <!------------------------------ START: Container Section ------------------------------>
            <!---------- START: First Section ---------->
            <section id="page">
						<div class="row">
							<div class="container-fluid">
								<!---------- START: Black Section ---------->
								<div class="container bg_cover bg_row_about_black om-animation-enabled">
									<div class="contentInner txt_white">
										Why would any company want to build its own telephone or postal network just to 
										communicate with its customers and suppliers? Why should the principle be any 
										different for sending and receiving documents, such as invoices, 
										electronically?
										<br>
										<br>
										It therefore makes little sense for any company, either large, small, public or 
										private to invest heavily in an internal invoice processing system. The P2D 
										Document Network is a fully-managed service which through economies of scale 
										delivers a suite of highly sophisticated invoice processing solutions at an 
										incredibly low transactional cost, just like paying for a telephone call or 
										postage stamp. Crucially, P2D offers e-invoicing and scanning/OCR as an 
										integrated solution, providing 100% automation of invoice processing. Buyers 
										can select from a range of ancilliary services such as bespoke approvals 
										workflows and purchase order matching, and suppliers are offered a variety of 
										ways to send invoices electronically to suit their organisational size and 
										capabilities.
									</div>
								</div>
								<!---------- END: Black Section ---------->
							</div>
						</div>
					</section>
            <!---------- END: First Section ---------->
            <!---------- START: Second Section ---------->
            <section id="page">
						<div class="row">
							<div class="container-fluid">
								<div class="col-lg-12">
									<div class="contentInner">
										<p>
											<!--<img class="img-thumbnail_lft pull-left" src="assets/img/side_pic01.jpg" alt="" >-->
											<strong><a href="http://www.p2dgroup.net" target="_blank">P2D</a></strong> is a unique business services company specializing in 
											document solutions that turn 'paper to data'.
											<br>
											<br>
											It was established as a direct consequence of the fast-changing regulations and 
											corporate attitudes towards electronic document management and electronic 
											trading that have taken place with the advent of the 21st century.
											<br>
											<br>
											The European Union, for example, estimates that every invoice received costs 
											nearly £1 in data entry costs alone, with a total processing cost typically of 
											£2 to £6, and in some cases considerably more. A recent report by 
											PricewaterhouseCoopers stated that companies could increase their net profit by 
											up to 4% by adopting electronic invoicing. Indeed, the EU has now passed 
											legislation that is set to have a dramatic effect on the use of e-invoicing. 
											From 2004, it was no longer be necessary to store a paper invoice in the 
											country where the transaction took place. This legislation was passed with the 
											deliberate intention of persuading companies to invoice electronically.
										</p>
										<!---------- END: White Section ---------->
										<br>
										<!---------- START: Testimonials Section ---------->
										<!-- Carousel
                                        ================================================== -->
										<div id="testimonialCarousel" class="carousel slide" data-ride="carousel">
											<div class="carousel-inner">
												<div class="item active">
													<div class="carousel-caption_inner">
														<i class="fa fa-quote-left"></i>The P2D Document Network, a global e-trading 
														hub, allows members to freely exchange documents, such as invoices and orders, 
														whether in electronic or paper form. P2D's philosophy is to provide 
														state-of-the-art document solutions at an affordable price to large, medium, 
														and small-sized companies alike, via its economies of scale. <i class="fa fa-quote-right">
														</i>
													</div>
												</div>
												<div class="item">
													<div class="carousel-caption_inner">
														<i class="fa fa-quote-left"></i>The P2D Document Network, a global e-trading 
														hub, allows members to freely exchange documents, such as invoices and orders, 
														whether in electronic or paper form. P2D's philosophy is to provide 
														state-of-the-art document solutions at an affordable price to large, medium, 
														and small-sized companies alike, via its economies of scale. <i class="fa fa-quote-right">
														</i>
													</div>
												</div>
											</div>
										</div>
										<!-- /.carousel -->
										<!---------- END: Testimonials Section ---------->
										<br>
										P2D outperforms the market on both price and performance. Its solutions are 
										flexible and configurable – we provide not only the sending and receiving of 
										documents but automation of any associated process that the client may require, 
										from document imaging, approvals workflows and matching, through to full 
										e-procurement. Indeed, for most companies, such change needs to be managed in 
										controlled increments. P2D acknowledges this, and provides solutions that scale 
										according to its clients' timescales and budgets. Gone are the days where 
										companies need to buy ERP-based procurement modules that cost millions and take 
										years to implement, and then only ever use a fraction of the functionality. 
										P2D's solutions are developed outside client's firewalls, sending data securely 
										and directly into their systems – it is outsourced data capture at its most 
										sophisticated.
									</div>
								</div>
							</div>
						</div>
					</section>
            <!---------- END: Second Section ---------->
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
										<a href="javascript:void(0)" class="current" title="About Us"><i class="glyphicon glyphicon-play"></i>
											About Us</a></li>
									<li>
										<a href="Solutions_Einvoicing.aspx" title="Solutions"><i class="glyphicon glyphicon-play"></i>Solutions</a></li>
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
    </TD></TR></TD></TR></TD></TR>
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

            /*Tab Section Style*/
            $(".tab-01").click(function () {
                $('.tab-content_01').removeClass('hide');
                $(".tab-content_01").fadeIn(1000);
                $('.tab-content_02').addClass('hide');
            });
            $(".tab-02").click(function () {
                //$('.tab-content_02').removeClass('hide');
                //$(".tab-content_02").fadeIn(1500);
                $('.tab-content_01').addClass('hide');
            });

            /*Testimonials Slider*/
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
