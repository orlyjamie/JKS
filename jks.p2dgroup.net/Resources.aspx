<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Resources.aspx.cs" Inherits="CBSolutions.ETH.Web.Resources" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Employee expense claim processing|Expense claim approvals|Expense claim workflow
        - P2D Group</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;">
    <meta name="description" content="">
    <meta name="keyword" content="Expense claim process, Employee expense claim processing, Expense claim approvals, Expense claim workflow">
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
									<ul class="nav  navbar-nav">
										<li>
											<a href="Default.aspx" title="HOME">HOME</a>
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
										<li class="active">
											<a href="javascript:void(0)" title="RESOURCES">RESOURCES</a>
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
            <section id="page" class="tab-ban01">
						<div class="row">
							<div class="container">
								<div class="col-lg-12 ban_sub_wrapper">
									<img class="img_ban" src="assets/img/ban/ban_resources.jpg" alt="Resources::P2D Papper 2 Data">
									<h1>Award-Winning Cloud Solutions</h1>
								</div>
							</div>
						</div>
					</section>
            <section id="page" class="tab-ban02" style="display: none;">
						<div class="row">
							<div class="container">
								<div class="col-lg-12 ban_sub_wrapper">
									<img class="img_ban" src="assets/img/ban/ban_resources_summary.jpg" alt="Resources::P2D Papper 2 Data">
									<h1>Award-Winning Cloud Solutions</h1>
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
									<div class="nav-pillswrapper col-md-3 height_justified">
										<ul class="nav nav-pills nav-stacked">
											<li class="active">
												<a href="#tab_a" data-toggle="pill" class="tab-01" title="Network Hosting">Network Hosting<span class="li_arrow"></span></a>
											<li>
												<a href="#tab_b" data-toggle="pill" class="tab-02" title="Summary">Summary<span class="li_arrow"></span></a></li>
										</ul>
									</div>
									<div class="tab-content col-md-9 height_justified">
										<div class="tab-pane fade active in" id="tab_a">
											<div class="contentInner">
												<p>
													<a href="http://www.p2dgroup.net/" target="_blank">P2D</a> client systems are hosted by Rackspace in order to provide the highest 
													possible levels of performance and security.
													<br>
													<br>
													Rackspace has successfully earned SAS 70 Type II and Safe Harbour 
													certifications and is a PCI Security Standards Council Member. SAS 70 
													certification provides industry-standard assurances required by their 
													customers, especially publicly traded companies who fall under the 
													Sarbanes-Oxley regulations and those in the financial, manufacturing and 
													healthcare industries. Safe Harbour certifies that Rackspace provides adequate 
													privacy protection by the standard of the European Commission's directive on 
													Data Protection. Each of their 8 global data centres is ISAE 3402 Type II SOC 1 
													Audited, and Rackspace is also ISO27001 certified.
												</p>
											</div>
										</div>
										<div class="tab-pane fade" id="tab_b">
											<div class="contentInner">
												<p>
													P2D has an ISO certified development centre focused on integration and workflow 
													solutions for members of the P2D Document Network.
													<br>
													<br>
													We have a team of skilled technical architects and developers with an 
													outstanding track record for delivering successful solutions and a wide range 
													of profiles including project managers, project leads, technical leads, 
													software engineers and QA personnel.
													<br>
													<br>
													Apart from strong technical skills, our development staff have an in-depth 
													understanding of Project Management, SDLC and Risk Management concepts. Among 
													the facilities at our development centre are dedicated voice channels, high end 
													internet connectivity, secure data transfer facilities, IP based video 
													conferencing facilities and so on.
												</p>
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
									<div class="container bg_cover bg_row_resources_1st om-animation-enabled">
										<div class="contentInner txt_white">
											With more than 172,000 customers in over 80 different countries, Rackspace is 
											the largest managed hosting company in the world. Their world-class facilities, 
											multi-homed network and 'Fanatical Support' have earned them the reputation as 
											the premier provider of managed hosting solutions, and why Rackspace is a 
											'leader' in the 2011 Gartner Magic Quadrant for managed hosting. Their network 
											is home to companies such as Microsoft, PricewaterhouseCoopers, Canon, KPMG, 
											Vodafone, Samsung, General Electric, Mazda and Accenture.
											<div class="bg_trans_white txt_white move_top5">
												<div class="container-fluid">
													<h3 class="txt_white"><i class="glyphicon glyphicon-ok"></i> NETWORK QUALITY</h3>
													<div class="container-fluid">
														<div class="container-fluid">
															Rackspace's network is 'Cisco Powered', having passed Cisco's compliance audit 
															for security, redundancy and speed. Indeed it is one of only a handful of 
															hosting companies in the world to have this status. The ultra secure and 
															redundant network is built exclusively on hardened Cisco Systems routing, 
															switching and security equipment. The fully switched network is regularly 
															audited for security by Cisco. It is also regularly tested from both inside and 
															outside the network by Rackspace and third party security specialists.
														</div>
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
                <section id="page" name="page">
							<div class="row">
								<div class="container-fluid">
									<div class="container">
										<div class="contentInner clearfix">
											<img src="assets/img/side_pic02.jpg" alt="" class="featurette-image img-responsive pull-right">
											<h3><i class="fa fa-check"></i> CONNECTIVITY</h3>
											Rackspace utilizes connections to multiple bandwith providers to ensure that 
											data reaches the end-user in the fastest, most efficient manner possible. There 
											are peering arrangements with local ISPs to allow fast delivery of data.
											<br>
											<br>
											<h3><i class="fa fa-check"></i> BGP4 ROUTING</h3>
											It runs the Border Gateway Protocol (BGP4) for best case routing. The network 
											employs Cisco GSR 12000 class routers running HSRP (N+1 hot failover) to ensure 
											that data can be routed even in the event of a router failure. The BGP4 
											protocol is a standard that allows for the routing of data sent out from the 
											network. Each packet of data is evaluated and sent over the best route 
											possible. Because of the redundant network architecture, data may be sent via 
											alternative routes even if they are being delivered to the same end user. 
											Should one of the network providers fail, data leaving the network is 
											automatically redirected through another route via a different provider.
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
									<div class="bg_color_lightGrey clearfix">
										<div class="container">
											<div class="contentInner clearfix">
												<h3><i class="fa fa-check"></i> GUARANTEED DELIVERY</h3>
												Providers are paid to ensure all data is delivered to the end-user. Because 
												there are Service Level Agreements with its providers, they are able to 
												guarantee that all data will leave the network at full speed.
												<br>
												<br>
												<h3><i class="fa fa-check"></i> BANDWITH UTILISATION</h3>
												The network currently has considerable excess capacity, even during peak hours. 
												This allows for even the largest spikes in traffic. Network connectivity and 
												new routes are always being added in an effort to make sure content is 
												delivered to users as efficiently as possible. A low bandwith utilization also 
												allows for maximum uptime, even if one of the providers has an outage.
											</div>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: Three Section ---------->
                <!---------- START: Four Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="bg_cover bg_row_resources_2nd om-animation-enabled">
										<div class="contentInner txt_white clearfix">
											<div class="col-md-6 move_bottom30">
												<h3 class="txt_white"><i class="fa fa-check"></i> FIREWALL</h3>
												The hardware Firewall provides robust, enterprise-class, integrated network 
												security, creating a strong multi-layered defensive service for dynamic network 
												environments. The hardware device works with a set of rules, filtering traffic 
												coming through the Internet into our systems. If an incoming packet of data is 
												flagged up by the filters as against the rules that have been set up, it will 
												not be allowed through. These devices add an additional layer of security to 
												the servers, stopping potentially malicious packets from ever reaching the 
												network.
											</div>
											<div class="col-md-6 move_bottom30">
												<h3 class="txt_white"><i class="fa fa-check"></i> DATA CENTRE</h3>
												The data centre has been engineered with fully redundant connectivity, power 
												and HVAC to avoid any single point of failure, and is staffed 24 x 7 by highly 
												trained technical support personnel. And because the data centre is not open to 
												the public, only a handful of level-three technicians are allowed in close 
												physical proximity to the servers. Multiple levels of security are employed to 
												ensure that only Data Centre Operations Engineers are physically allowed near 
												the routers, switches, and servers.
											</div>
											<div class="clearfix"></div>
											<div class="col-lg-12">
												<h3 class="txt_white"><i class="fa fa-check"></i> ANTI - VIRUS</h3>
												An anti-virus solution is one of the most critical, effective and affordable 
												ways to avoid infections from viruses, spyware, adware and potentially unwanted 
												applications. The managed Anti-Virus solution from Rackspace is an advanced 
												technology powered by Sophos that's fully managed by their experts, so our 
												servers get the ultimate level of protection.
											</div>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: Four Section ---------->
                <!---------- START: Five Section ---------->
                <section name="page" id="page">
							<div class="row">
								<div class="container-fluid">
									  <div class="container">
                                <div class="contentInner clearfix">
                                    <h3><i class="fa fa-check"></i> MONITORING</h3>
                                    Rackspace provides a 24 x 7 monitoring service to check the availability on the servers. Service checks are performed at 5 minute intervals to ensure quick identification of problems. Should a device not respond, support engineers are sent an alert via pager and e-mail. Rackspace will investigate the problem immediately, checking the console for the error message and determining the severity of the problem. Rackspace support and Data Centre operations engineers will respond to hardware failures as per their guaranteed 1 hour hardware fix Service Level Agreement which ensures minimal solution downtime. Rackspace also conduct regular internal/external tests and all controls are audited annually as part of their SAS 70 audit. P2d has a full-time administrator and 24 x 7 on-call support.
                                    <br><br>
                                    
                                    <img src="assets/img/side_pic03.jpg" alt="" class="featurette-image img-responsive pull-right">
                                    
                                    <h3><i class="fa fa-check"></i> SECURITY PROCEDURES ARE AS FOLLOWS</h3>
                                    
                                    <div style="overflow:hidden;">
                                        <ul class="pull-left move_lft30">
                                            <li>No Public Access</li>
                                            <li>Employee Background Checks</li>
                                            <li>Video Surveillance</li>
                                            <li>Onsite Security Personnel</li>
                                            <li>Military-Grade Pass Cards</li>
                                        </ul>
                                        
                                        <ul class="pull-left move_lft30">
                                            <li>Biometric Security</li>
                                            <li>HVAC</li>
                                            <li>
                                                Power
                                                <ul>
                                                    <li>UPS Systems</li>
                                                    <li>Diesel Generator Systems</li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    
                                    <br>
                                    
                                    All premises have secured access with RFID entry cards &amp; photographic ID for all staff. Rackspace personnel are required to display their identity badges at all times when onsite at Rackspace data centres and non-data centre facilities. Two-factor authentication is required to gain access to the data centre facilities. Electromechanical locks are controlled by biometric authentication (hand geometry scanner) and key-card/badge. Only authorized Rackspace personnel have access to data centre facilities. Closed circuit video surveillance has been installed at all entrance points on the interior and exterior of the buildings housing data centres. Cameras support data retention for 90 days.
                                    <br><br>
                                    Only Rackspace Data Center employees are allowed to access the server/production floor.  That entry access is controlled by a finger print scanning biometric device and proximity access cards. Roof and exterior walls are heavy duty rated at 130 mph.  There is a heavy duty lightning grid on roof.  All electrical and mechanical equipment is on 3 inch raised concrete pads. Lenel Security Management System is deployed at the data centre with central monitoring capabilities at Rackspace HQ. Alarms are directly connected to the local Fire and Police Departments.
                                    <br><br>
                                    Onsite security personnel monitor each data centre building 24 hours per day, seven days per week. The security team are responsible for making sure that only authorised personnel enter the data centre building. The security personnel provide the first layer of security for access to the data centre.
                                    <br><br>
                                    Building Operations, Security or Data Centre Management review and approve visitor access and issue visitor badges for identification purposes before access is granted to any non-Rackspace employee.  ALL visitors must be escorted.  All visitors sign-in the visitor log book which requires visitors to present a Valid photo ID, reason of visit and a Rackspace POC.  Corporate Risk Management performs a monthly audit of Security and Visitor access logs.
                                </div>
                            </div>
								</div>
							</div>
						</section>
                <!---------- START: Five Section ---------->
                <!---------- START: SIX Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="bg_color_lightGrey clearfix">
										<div class="container">
											<div class="contentInner">
												<h3><i class="fa fa-check"></i> SUPPORT ADMINISTRATION</h3>
												Rackspace policies require users to be specifically authorized to access 
												information and system resources, especially systems that are used to provide 
												support services to customers. The Information Technology Services (ITS) 
												Department is responsible for security administration functions, including 
												assigning/deleting users to internal Rackspace system resources.
												<br>
												<br>
												Rackspace has logically separate networks for all internal traffic, resulting 
												in Rackspace administration of customer environments being performed from 
												specified networks within the Rackspace environment. All Rackspace user access 
												requests follow a documented, formal process and must be approved by a manager 
												or supervisor. Upon termination from Rackspace, employee access is removed from 
												Active Directory.
												<br>
												<br>
												When an employee's job responsibilities change or the employee transfers to a 
												new department, the individual's manager contacts ITS to change the transferred 
												employee's access rights to verify that they are commensurate with the 
												employee's new position. The Human Resources Department generates a listing of 
												all employee terminations (immediately following termination) and forwards this 
												notice to ITS so that the employee's access can be disabled or removed from the 
												appropriate systems. The manager of the terminated employee may also inform ITS 
												of the need to revoke access from a user account.
												<br>
												<br>
												<h3><i class="fa fa-check"></i> DISASTER RECOVERY</h3>
												Server Redundancy: RAID 1 &amp; 5 ensures data is retained and continuously 
												accessible in the event of a hard drive failure. In addition to fault tolerance 
												this also increases disk and therefore application performance. Dual PSUs 
												ensure that in the event a power supply fails on the server, the server will 
												continue to provide service utilising the second PSU. Load Balancing, using 
												various load balancing algorithms, web/application sessions can be distributed 
												across the servers to ensure even distribution of load and to increase solution 
												redundancy and performance, if required.
											</div>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: SIX Section ---------->
            </div>
            <div class="tab-content_02" style="display: none;">
                <!---------- START: First Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="bg_color_lightGrey clearfix">
										<div class="container">
											<div class="contentInner clearfix">
												Our practices comply with BCI standards as well as ISO/IEC 17799:2005. In 
												addition, our projects are managed to PRINCE2 methodologies incorporating risk 
												management. The Rackspace Risk Management Framework is subject to internal 
												audit as part of the ISO 27001 compliant Information Security Management System 
												audits.
												<br>
												<br>
												P2D's scanning bureau is ISO 9001:2008 certified and all work is produced to 
												the BSI guidelines published in BIP0008 ensuring best practice for Legal 
												Admissibility is adhered to.
												<br>
												<br>
												P2D's client delivery and risk management is overseen by the CIO, who was 
												previously Head of Performance Management for Barclays Group IT division where 
												he was a member of the Risk Oversight Committee, with specific responsibility 
												for governance of the group's £350m annual IT project portfolio spend.
												<br>
												<br>
												Please <a href="contact.html" class="txt_deepGrey"><strong>contact us</strong></a>
												for a copy of our detailed Security Policies.
											</div>
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
									<div class="col-lg-12">
										<div class="col-md-2 pull-right"><a href=""><img class="img-responsive" src="assets/img/logo_scroll/logo30.jpg"></a></div>
										<div class="col-md-2 pull-right"><a href=""><img class="img-responsive" src="assets/img/logo_scroll/logo29.jpg"></a></div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: Second Section ---------->
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
										<a href="Solutions_Einvoicing.aspx" title="Solutions"><i class="glyphicon glyphicon-play"></i>Solutions</a></li>
									<li>
										<a href="javascript:void(0)" class="current" title="Resources"><i class="glyphicon glyphicon-play"></i>
											Resources</a></li>
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
                //$('.tab-ban01').removeClass('hide');
                $('.tab-content_01').removeClass('hide');

                //$(".tab-ban01").fadeIn(1000);
                $(".tab-content_01").fadeIn(1000);

                //$('.tab-ban02').addClass('hide');
                $('.tab-content_02').addClass('hide');

            });
            $(".tab-02").click(function () {
                //$('.tab-ban02').removeClass('hide');
                $('.tab-content_02').removeClass('hide');

                //$(".tab-ban02").fadeIn(1000);
                $(".tab-content_02").fadeIn(1500);

                //$('.tab-ban01').addClass('hide');
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
