<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Partner.aspx.cs" Inherits="CBSolutions.ETH.Web.Partner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>P2D Paper 2 Data :: Award-winning Document Solutions</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="../../assets/ico/favicon.ico">
    <!-- Bootstrap core CSS -->
    <link href="dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom Global Style -->
    <link href="custom_css/screen.css" rel="stylesheet">
    <link href='http://fonts.googleapis.com/css?family=Lato:400,300italic,300,100italic,100,400italic,700,700italic,900,900italic'
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
											<a href="Default.aspx" title="Home">HOME</a>
										<li>
											<a href="AboutUs.aspx" title="ABOUT US">ABOUT US</a>
										<li class="dropdown">
											<a class="dropdown-toggle js-activated" data-toggle="dropdown" href="" title="SOLUTIONS">SOLUTIONS
												<span class="caret"></span></a>
											<ul class="dropdown-menu">
												<li>
													<a href="Solutions_Einvoicing.aspx" title="E-invoicing" class="active">E-invoicing</a>
												<li>
													<a href="Solutions_ISOCR.aspx" title="Invoice Scanning & OCR">Invoice Scanning &amp; OCR</a>
												<li>
													<a href="Solutions_Staff.aspx" title="Staff Expense Claims">Staff Expense Claims</a></li>
											</ul>
										<li>
											<a href="Resources.aspx" title="RESOURCES">RESOURCES</a>
										<li class="active">
											<a href="javascript:void(0)" title="PARTNERS">PARTNERS</a>
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
									<img class="img_ban" src="assets/img/ban/ban_partners.jpg" alt="Resources::P2D Papper 2 Data">
									<h1>Award-Winning Cloud Solutions</h1>
								</div>
							</div>
						</div>
					</section>
            <!------------------------------ END: Banner Section ------------------------------>
            <!------------------------------ START: Container Section ------------------------------>
            <!---------- START: First Section ---------->
            <section id="page">
						<DIV class="row">
							<DIV class="container-fluid">
								<DIV class="bg_cover bg_row_partners_1st om-animation-enabled" style="Z-INDEX: 0">
									<DIV class="contentInner txt_white clearfix">
										<div class="col-md-9">
											P2D is committed to the development of its distribution channel and is always 
											actively seeking new resellers. If you are interested in the following benefits 
											associated with being a P2D reseller and would like more information please <a href="contact.html" class="btn_lrgtxt_white">
												Contact Us</a>.
											<br>
											<br>
											<h3>OUR PARTNERSHIP FEATURES AND BENEFITS</h3>
											<ul class="list_whitecolor">
												<li>
												No joining fee
												<li>
												Generous Annuity income
												<li>
												No implementation effort required
												<li>
												Ability to win new clients
												<li>
												Cross-sell into new clients with your existing solutions
												<li>
												Cross-sell additional document types e.g. orders, remittance advices etc to 
												build further income streams
												<li>
												Qualified leads provided
												<li>
												Full sales support provided
												<li>
												Automated client billing
												<li>
													Automated reseller billing</li>
											</ul>
										</div>
										<DIV class="col-md-3">
											<DIV class="white_trans_box login_formBox">
												<h3>PARTNER LOG IN</h3>
												<div class="form-horizontal">
													<div class="form-group">
														<label class="col-lg-12 control-label" for="inputEmail">User Name</label>
														<div class="col-lg-12">
															<asp:textbox id="txtPartnerUserName" runat="server" CssClass="form-control" MaxLength="25" EnableViewState="true"></asp:textbox>
														</div>
													</div>
													<div class="form-group">
														<label class="col-lg-12 control-label" for="inputPassword">Password</label>
														<div class="col-lg-12">
															<asp:textbox id="txtPartnerPassword" runat="server" CssClass="form-control" MaxLength="25" EnableViewState="true"
																TextMode="Password"></asp:textbox>
														</div>
													</div>
													<div class="form-group">
														<div class="col-lg-offset-4 col-lg-10">
															<asp:Button ID="btnPartnerLogIn" runat="server" CssClass="btn btn-default btn-grey" Text="SUBMIT"></asp:Button>
														</div>
													</div>
												</div>
											</DIV>
										</DIV>
									</DIV>
								</DIV>
							</DIV>
						</DIV>
        </div>
    </div>
    <!-------------- END: Site Wrapper ------------------------------------------------->
    <div class="site">
        <div class="container mainWrapper nopadding">
            <div class="row">
                <div class="container-fluid">
                    <div class="container">
                        <div class="contentInner clearfix">
                            <h3>
                                SOME OF OUR TECHNOLOGY AND RESELLER PARTNERS</h3>
                            <div class="row parteners_listBox">
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <img src="assets/img/logo_scroll/logo05.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <img src="assets/img/logo_scroll/logo06.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <%--<img src="assets/img/logo_scroll/logo07.jpg" class="img-responsive">--%>
                                        <img src="assets/img/logo_scroll/ipswitch_200x100_logo.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <img src="assets/img/logo_scroll/logo08.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <img src="assets/img/logo_scroll/logo09.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <img src="assets/img/logo_scroll/logo10.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <%--<img src="assets/img/logo_scroll/logo11.jpg" class="img-responsive">--%>
                                        <img src="assets/img/logo_scroll/cloudbuy_200x100_logo.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <img src="assets/img/logo_scroll/logo12.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <%--<img src="assets/img/logo_scroll/logo13.jpg" class="img-responsive">--%>
                                        <img src="assets/img/logo_scroll/sage_200x100_logo.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <%--<img src="assets/img/logo_scroll/logo14.jpg" class="img-responsive">--%>
                                        <img src="assets/img/logo_scroll/lawson_200x100_logo.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <img src="assets/img/logo_scroll/logo15.jpg" class="img-responsive">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                    <div class="thumbnail">
                                        <img src="assets/img/logo_scroll/logo16.jpg" class="img-responsive">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </SECTION>
            <!---------- END: Second Section ---------->
            <!---------- START: Three Section ---------->
            <!---------- END: Three Section ---------->
            <!---------- START: Four Section ---------->
            <!---------- END: Four Section ---------->
            <!---------- START: Five Section ---------->
            <!---------- START: Five Section ---------->
            <!---------- START: SIX Section ---------->
            <!---------- END: SIX Section ---------->
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
									<a href="Solutions_Einvoicing.aspx" title="Solutions"><i class="glyphicon glyphicon-play"></i>Solutions</a></li>
								<li>
									<a href="Resources.aspx" title="Resources"><i class="glyphicon glyphicon-play"></i>Resources</a></li>
								<li >
									<a href="javascript:void(0)" title="Partners" class="current"><i class="glyphicon glyphicon-play"></i>Partners</a></li>
								<li>
									<a href="Customers.aspx"  title="Customers"><i class="glyphicon glyphicon-play"></i>
										Customers</a></li>
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
						<div class="col-xs-6 f_col">©2015 paper2data. All Rights Reserved</div>
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
