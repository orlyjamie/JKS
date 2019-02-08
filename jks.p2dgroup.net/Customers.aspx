<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Customers.aspx.cs" Inherits="CBSolutions.ETH.Web.Customers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Purchase ledger automation software from award winning cloud solutions - P2D
        Group</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;">
    <meta name="description" content="">
    <meta name="keyword" content="Purchase ledger automation">
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
										<a href="Default.aspx" title="Home">HOME</a>
                                        </li>
									<li>
										<a href="AboutUs.aspx" title="ABOUT US">ABOUT US</a></li>
									<li class="dropdown">
										<a class="dropdown-toggle js-activated" data-toggle="dropdown" href="" title="SOLUTIONS">SOLUTIONS
											<span class="caret"></span></a>
										<ul class="dropdown-menu">
											<li><a href="Solutions_Einvoicing.aspx" title="E-invoicing" class="active">E-invoicing</a></li>
											<li><a href="Solutions_ISOCR.aspx" title="Invoice Scanning & OCR">Invoice Scanning &amp; OCR</a></li>
											<li><a href="Solutions_Staff.aspx" title="Staff Expense Claims">Staff Expense Claims</a></li>
										</ul>
                                    </li>
									<li><a href="Resources.aspx" title="RESOURCES">RESOURCES</a></li>
									<li><a href="Partner.aspx" title="PARTNERS">PARTNERS</a></li>
									<li class="active">	<a href="javascript:void(0)" title="CUSTOMERS">CUSTOMERS</a></li>
									<li><a href="News.aspx" title="NEWS">NEWS</a></li>
									<li><a href="ContactUs.aspx" title="CONTACT US" class="nobg">CONTACT US</a></li>
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
								<img class="img_ban" src="assets/img/ban/ban_customers.jpg" alt="Resources::P2D Papper 2 Data">
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
							<div class="container">
								<div class="contentInner clearfix">
									<h3>OUR CUSTOMERS</h3>
									<div class="media-list img_listing_lftRgt">
									
									<div class="media">
											<a href="#" class="pull-right col-md-3 nopadding"><img alt="" class="media-object pull-right" src="assets/img/logo_scroll/gordon_logo.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.gordonramsay.com/"  target="_blank">www.gordonramsay.com</a>
												</h4>
												The Gordon Ramsay Group comprises of the restaurant business of acclaimed chef, 
													restaurateur, TV personality and author Gordon Ramsay. It employs more than 700 
													people in London where it has a collection of 13 restaurants. The Group has a 
													total of 25 restaurants globally and 7 Michelin stars, with international 
													restaurants from Europe to the US and the Middle East; and selected P2D’s new 
													invoice processing solution following a BPO market tender, going live with the 
													system in April 2015.
											</div>
										</div>									
						
										<hr>
										<div class="media">
											<a href="#" class="pull-left col-md-3 nopadding"><img alt="" class="media-object pull-left" src="assets/img/logo_scroll/logo48.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.newmoonpub.co.uk/" target="_blank">www.newmoonpub.co.uk</a>
												</h4>
												The New Moon Pub Group is an expanding collection of high-end pubs developed 
												and operated by industry veterans David Mooney and Paul Newman. New Moon has 
												selected the P2D system for invoice processing, including scanning, Intelligent 
												OCR data capture, approvals workflow and web services integration with Aqilla 
												online accounts, going live in April 2014.
											</div>
										</div>
										<!--Right Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-right col-md-3 nopadding"><img alt="" class="media-object pull-right" src="assets/img/logo_scroll/logo47.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.communicorp.ie/" target="_blank">www.communicorp.ie</a>
												</h4>
												Founded in 1989 Communicorp Group Limited operates a portfolio of media and 
												radio assets in Ireland, Europe and Jordan. It is the leading Irish commercial 
												radio broadcaster and it operates the largest independent radio groups in both 
												BBBRaria and Latvia. Communicorp adopted the P2D system for invoice processing 
												in April 2014 following the UK acquisition of Smooth and Real Radio from Global 
												Radio.
											</div>
										</div>
										<!--Right Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-left col-md-3 nopadding"><img alt="" class="media-object pull-left" src="assets/img/logo_scroll/logo17.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.asos.com/" target="_blank">www.asos.com</a>
												</h4>
												Asos PLC is the UK's largest independent online fashion and beauty retailer 
												servicing 241 countries. It generates annual revenues of c. £750m and has a 
												market capitalization of £3bn. Asos selected P2D's online employee expense 
												claim system which was successfully implemented in June 2013. The system has 
												been rolled out globally across the entire Asos group structure, and 
												incorporates full international tax calculations.
											</div>
										</div>
										<!--Left Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-right col-md-3 nopadding"><img alt="" class="media-object pull-right" src="assets/img/logo_scroll/logo18.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.facilities-services-group.co.uk/" target="_blank">www.facilities-services-group.co.uk</a>
												</h4>
												Facilities Services Group is a market leading integrated facilities management 
												service provider, with a turnover of more than £20m. Facilities Services Group 
												selected P2D to provide an e-invoicing programme of PO Flips, e-invoicing, PO 
												matching and supplier portal to process the invoices it receives in servicing 
												its clients. FSG's clients include Starbucks, Debenhams, Nandos, Travelodge, 
												Dreams, The Co-operative, Serco, Knight Frank, Cancer Research and Majestic 
												Wine among others. The project went live in March 2013.
											</div>
										</div>
										<!--Right Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-left col-md-3 nopadding"><img alt="" class="media-object pull-left" src="assets/img/logo_scroll/logo19.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.ctbaccounts.com/" target="_blank">www.ctbaccounts.com</a>
												</h4>
												CTB Accounts is a specialised consultancy providing management accountancy 
												services to the restaurant industry. P2D has partnered with CTB to provide 
												e-invoicing, scanning, OCR &amp; validation, workflow, matching and supplier 
												portal to its customer base with a current total invoice volume of over 10,000 
												per month. The project went live in January 2013 and includes clients such as 
												China Tang (The Dorchester Hotel), Novikov, Bounce, Vinoteca, Bob Bob Ricard 
												and Granger &amp; Co.
											</div>
										</div>
										<!--Left Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-right col-md-3 text-right nopadding"><img alt="" class="media-object pull-right" src="assets/img/logo_scroll/logo20.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.akkeronhotels.com/" target="_blank">www.akkeronhotels.com</a>
												</h4>
												Akkeron Hotels Group currently comprises 70 hotels across the country, many 
												branded as Ramada, Holiday Inn Express and Best Western, with a goal of having 
												150 hotels by the end of 2013. The P2D system was successfully implemented in 
												March 2012, for e-invoicing, scanning, OCR &amp; validation, workflow, matching 
												and supplier portal.
											</div>
										</div>
										<!--Right Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-left col-md-3 nopadding"><img alt="" class="media-object pull-left" src="assets/img/logo_scroll/logo21.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.etchospitality.com/" target="_blank">www.etchospitality.com</a>
												</h4>
												ETC Hospitality is an outsourced cloud accounting company in the hospitality 
												sector, servicing a variety of high street restaurant and hotel chains. P2D has 
												partnered with ETC to provide e-invoicing, scanning, OCR &amp; validation, 
												workflow, matching and supplier portal to its customer base with a current 
												total invoice volume of over 20,000 per month. The first clients went live in 
												February 2012.
											</div>
										</div>
										<!--Left Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-right col-md-3 nopadding"><img alt="" class="media-object pull-right" src="assets/img/logo_scroll/logo22.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="https://www.twinfield.co.uk/" target="_blank">www.twinfield.co.uk</a>
												</h4>
												Twinfield is the European market leader in online accounting, and is used to 
												compile more than 80,000 financial accounts in 22 countries. Twinfield is part 
												of Wolters Kluwer, which has over 18,000 employees and revenues of more than 
												£3bn. P2D has partnered with Twinfield to provide invoice scanning, OCR, and 
												validation services to its UK customer base, commencing in June 2011.
											</div>
										</div>
										<!--Right Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-left col-md-3 nopadding"><img alt="" class="media-object pull-left" src="assets/img/logo_scroll/logo23.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.co-operative.coop/" target="_blank">www.co-operative.coop</a>
												</h4>
												With revenues of nearly £14bn, The Co-operative Group are the UK's fifth 
												largest food retailer, the third largest retail pharmacy chain, the number one 
												provider of funeral services and the largest independent travel business. The 
												Co-operative Group also has strong market positions in banking and insurance. 
												The Group employs 130,000 people, has 5.5 million members and around 4,800 
												retail outlets. The P2D online expense claim system was implemented in August 
												2010 and successfully rolled out across the entire group within 3 months of go 
												live.
											</div>
										</div>
										<!--Left Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-right col-md-3 nopadding"><img alt="" class="media-object pull-right" src="assets/img/logo_scroll/logo24.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.dalkia.co.uk/" target="_blank">www.dalkia.co.uk</a>
												</h4>
												The P2D system has been implemented to process invoices for Boots through its 
												outsource partnership with Dalkia going live in April 2009. The system covers 
												2600 stores and includes e-invoicing, scanning, intelligent OCR, matching to 
												purchase order, approvals workflow and supplier portal. Boots is an 
												international retailer with revenues of over £15bn, 76,000 employees and a 150 
												year heritage. Dalkia is a leading provider of outsourced services, with 55,000 
												staff in 38 countries and sales of €8bn.
											</div>
										</div>
										<!--Right Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-left col-md-3 nopadding"><img alt="" class="media-object pull-left" src="assets/img/logo_scroll/logo25.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.institute.nhs.uk/" target="_blank">www.institute.nhs.uk</a>
												</h4>
												P2D is a preferred IT supplier to the NHS for the development/deployment of web 
												and application development, hardware and project/programme management. P2D 
												also forms part of a small group of selected suppliers required to proactively 
												recommend and implement new IT innovations. This follows P2D's achievement of 
												an Innovation Award from the Department of Trade &amp; Industry in 2005.
											</div>
										</div>
										<!--Left Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-right col-md-3 nopadding"><img alt="" class="media-object pull-right" src="assets/img/logo_scroll/logo26.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.newlook.com/" target="_blank">www.newlook.co.uk</a>
												</h4>
												New Look is an international fashion retailer with sales approaching £1bn per 
												annum. It has several hundred stores in the UK, over 200 in France under the 
												Mim brand, and is expanding further into Europe and the Middle East. New Look 
												went live on the P2D Document Network in 2006 for automation of its purchase 
												ledger via scanning/OCR, e-invoicing, supplier portal and approvals workflow.
											</div>
										</div>
										<!--Right Side Image-->
										<hr>
										<div class="media">
											<a href="#" class="pull-left col-md-3 nopadding"><img alt="" class="media-object pull-left" src="assets/img/logo_scroll/logo27.jpg">
											</a>
											<div class="media-body">
												<h4 class="media-heading">
													<a href="http://www.gmgplc.co.uk/" target="_blank">www.gmgplc.co.uk</a>
												</h4>
												Guardian Media Group PLC is a UK multi-media organization with sales of £635m. 
												Publishing and printing provide their core business but recent investments have 
												taken the group into radio and electronic publishing. It is probably best known 
												for publishing The Guardian, The Observer and Autotrader and its printing 
												interests in the Daily and Sunday Telegraph. GMG is currently rolling out P2D's 
												solutions for document management, imaging, workflow and the P2D Document 
												Network across the group.
											</div>
										</div>
										<!--Left Side Image-->
										<hr>
									</div>
									<br>
									<h3>CUSTOMER COMMENTS</h3>
									<div class="bg_color_lightGrey pad_inner commentsList-group clearfix">
										<h4 class="commentsList-group-heading">
											Ian Butterworth, New Look's Transaction Processing Manager, explains,
										</h4>
										<!--<div class="commentsList-group-item pull-left"> explains,</div>-->
										<p class="commentsList-group-item-text">
											"Handling huge volumes of invoices and processing them efficiently on-time and 
											with the minimum of resources is the Holy Grail for retailers of our size. The 
											P2D solution gives us the ability to manage this workload very easily and our 
											suppliers will benefit from it as well, without having to put in place an 
											expensive EDI system. Critical resources will be freed up and working with New 
											Look made far easier than ever before. What has impressed us more than anything 
											is the solution's flexibility and inherent scalable qualities. It can be 
											expanded to cope with many more suppliers in any country with relative ease 
											which is ideal for our growth plans given we are opening stores in Belgium, 
											France and Middle East."
										</p>
									</div>
									<!--END BG Color LightGrey-->
									<div class="bg_color_blue pad_inner commentsList-group clearfix">
										<h4 class="commentsList-group-heading">
											Adam Williams, Head of Shared Financial Services Centre for the Co-operative's 
											trading businesses said,
										</h4>
										<p class="commentsList-group-item-text">
											"The Co-operative considered various solutions and selected P2D's eExpenses 
											system as it was impressed with how intuitive the software is to use, the 
											simplicity of deployment and customisation, and its comprehensive range of 
											features. P2D also offered a favourable pay-to-use pricing structure, attentive 
											account management, and an informative site visit to another P2D retail 
											customer. The project started in August 2010, with the roll out complete by 
											December 2010."
										</p>
										<h4 class="commentsList-group-heading">
											Williams
											<span> says,</span>
										</h4>
										<p class="commentsList-group-item-text">
											"We've had first class service from P2D and its eExpenses system is an 
											excellent solution. They've been responsive to our needs, delivered quickly and 
											supported us all the way. I have no hesitation recommending the system based on 
											our experiences."
										</p>
									</div>
									<!--END BG Color Blue-->
									<div class="bg_color_lightGrey pad_inner commentsList-group clearfix">
										<h4 class="commentsList-group-heading">
											George Devine, Finance Manager, Mitie.
										</h4>
										<p class="commentsList-group-item-text">
											"I have now been working with P2D for well over a year and I am delighted with 
											both the service and the commitment given to myself and Mitie."
										</p>
									</div>
									<!--END BG Color LightGrey-->
									<div class="bg_color_blue pad_inner commentsList-group clearfix">
										<h4 class="commentsList-group-heading">
											Alan Morgan, Managing Director, ETC Hospitality.
										</h4>
										<p class="commentsList-group-item-text">
											"We approached P2D to offer something that was limited within our sector. We 
											have found them to be outstanding in their approach, work ethic and 
											performance, with delivery far exceeding our expectations. We would highly 
											recommend."
										</p>
									</div>
									<!--END BG Color Blue-->
									<div class="bg_color_lightGrey pad_inner commentsList-group clearfix">
										<h4 class="commentsList-group-heading">
											Ray Barber, AP &amp; AR Manager, New Look.
										</h4>
										<p class="commentsList-group-item-text">
											"We are extremely happy with the service provided by P2D and are currently 
											working with them to look at better ways to handle our processing."
										</p>
									</div>
									<!--END BG Color LightGrey-->
									<div class="bg_color_blue pad_inner commentsList-group clearfix">
										<h4 class="commentsList-group-heading">
											Bob Tims, General Manager at Cambra Styles said,
										</h4>
										<p class="commentsList-group-item-text">
											"Joining the network and getting up to speed with the new system was easy, 
											especially as P2D guided us through the process of getting connected. With up 
											to date processing and payment information online, it self evidently saves 
											time. It has meant we seem to get paid quicker too."
										</p>
									</div>
									<!--END BG Color Blue-->
									<div class="bg_color_lightGrey pad_inner commentsList-group clearfix">
										<h4 class="commentsList-group-heading">
											Terry Theodorou, Director at Lella Brothers said,
										</h4>
										<p class="commentsList-group-item-text">
											"We have found the P2D solution very quick, easy and convenient to use. The 
											capability to have full visibility and status of invoices is a valuable 
											management tool. This solution also allows us the ability to trade with any 
											other users of the network without the need for additional cost and resources."
										</p>
									</div>
									<!--END BG Color LightGrey-->
									<br>
									<h3>OTHER CUSTOMER INCLUDE</h3>
									<div class="row parteners_listBox">
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo31.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo32.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo33.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo34.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo35.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo36.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo37.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo38.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo39.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo40.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo41.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo42.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo43.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo44.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo45.jpg">
											</div>
										</div>
										<div class="col-lg-3 col-md-4 col-xs-6 thumb">
											<div class="thumbnail">
												<img class="img-responsive" src="assets/img/logo_scroll/logo46.jpg">
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</section>
            <!---------- END: First Section ---------->
            <!---------- START: Second Section ---------->
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
								<li>
									<a href="Partner.aspx" title="Partners"><i class="glyphicon glyphicon-play"></i>Partners</a></li>
								<li>
									<a href="javascript:void(0)" class="current" title="Customers"><i class="glyphicon glyphicon-play"></i>
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
    <!--<script src="assets/js/flowtype.js"></script>-->
    <!--<script type="text/javascript">
			$('body').flowtype();
        	$('body').flowtype({
			   minimum   : 500,
			   maximum   : 1200,
			   minFont   : 12,
			   maxFont   : 40,
			   fontRatio : 30
			});
        </script>-->
    </form>
</body>
</html>
