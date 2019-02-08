<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solutions_Staff.aspx.cs"
    Inherits="CBSolutions.ETH.Web.Solutions_Staff" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Accounts payable automation software solutions provider - P2D Group</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0;">
    <meta name="description" content="">
    <meta name="keyword" content="Accounts payable automation">
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
										<li class="dropdown">
											<a class="dropdown-toggle js-activated" data-toggle="dropdown" href="" title="SOLUTIONS">SOLUTIONS
												<span class="caret"></span></a>
											<ul class="dropdown-menu">
												<li>
													<a href="Solutions_Einvoicing.aspx" title="E-invoicing">E-invoicing</a>
												<li>
													<a href="Solutions_ISOCR.aspx" title="Invoice Scanning & OCR">Invoice Scanning &amp; OCR</a>
												<li class="active">
													<a href="javascript:void(0)" title="Staff Expense Claims">Staff Expense Claims</a></li>
											</ul>
										<li>
											<a href="Resources.aspx" title="RESOURCES">RESOURCES</a>
										<li>
											<a href="Partner.aspx" title="PARTNERS">PARTNERS</a>
										<li>
											<a href="Customers.aspx" title="CUSTOMERS">CUSTOMERS</a>
										<li>
											<a href="News.aspx" title="NEW">NEWS</a>
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
									<img class="img_ban" src="assets/img/ban/ban_solutions.jpg" alt="Solutions::P2D Papper 2 Data">
									<h1>ONLINE STAFF EXPENSE CLAIM SYSTEMS</h1>
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
												<a href="#tab_a" data-toggle="pill" class="tab-01" title="BACKGROUND">BACKGROUND<span class="li_arrow"></span></a>
											<li>
												<a href="#tab_b" data-toggle="pill" class="tab-02" title="HIGHLIGHTS">HIGHLIGHTS<span class="li_arrow"></span></a></li>
										</ul>
									</div>
									<div class="tab-content col-md-9 height_justified">
										<div class="tab-pane fade active in" id="tab_a">
											<div class="contentInner">
												<p>
													Historically, organisations haven't placed much emphasis on streamlining 
													processes that were not core to their businesses, even with the pressure of 
													regulatory compliance and the need to cut costs across those businesses.
													<br>
													<br>
													One area where organisations continue to spend an inordinate amount of time and 
													money is managing the reimbursement of employee expenses claims.
												</p>
											</div>
										</div>
										<div class="tab-pane fade" id="tab_b">
											<div class="contentInner">
												<ul>
													<li>
													Flexible workflow options e.g. based on cost centre structures, named override 
													approvers, multi-level approvals.
													<li>
													A global system, with claimants assigned a base currency and all foreign spend 
													automatically translated back to domestic values.
													<li>
													System exchange rates can be enforced, and/or the claimant can be allowed to 
													enter their own exchange rate at header level, or manually adjust the 
													translated line values individually.
													<li>
													Automatic calculation of tax values across all international locations.
													<li>
													Compatible for Single Sign On.
													<li>
													Compatible with all browser types &amp; versions, including mobile phones.
													<li>
													Receipts can be held at either header or line level as required.
													<li>
													Optional scanning service via PO Box and automated attachment of receipts to 
													claims.
													<li>
													Claimants can pre-populate claim lines based on previous claims to avoid 
													repetitive data entry where the same items are regularly claimed.
													<li>
													Corporate credit card, travel broker and mobile phone data feeds can be used to 
													pre-populate claim lines.
													<li>
													All mileage rates automatically populated across all international locations.
													<li>
													Expense types can be restricted to be available to only members of certain cost 
													centres and in certain countries.
													<li>
													Mandatory data fields for each expense type are highlighted.
													<li>
													Extensive validations throughout the system, for example, to ensure mandatory 
													are populated and enforce corporate policy.
													<li>
													Individual cost centres can be made exempt from policy rules.
													<li>
													Highlighting of out-of-policy lines throughout the system.
													<li>
													Full audit trail of workflow and user actions.
													<li>
													Specific lines can be rejected by approvers, leaving the remaining claim lines 
													to be approved and paid. Rejected lines are passed back to the claimant for 
													re-submission or deletion.
													<li>
													Users can be assigned as either proxy submitters and/or proxy approvers, where 
													P.A.s may need to create, submit and approve claims on behalf of their 
													managers.
													<li>
													Claims can be charged to an alternative cost centre, at either line or claim 
													level, with the claim automatically routed to the cost centre owner for 
													approval.
													<li>
													Administrators have a facility to log directly into claimant's inboxes for 
													trouble-shooting &amp; support purposes.
													<li>
													Each claimant can be assigned an escalation approver as well as line manager, 
													who will also be able to approve the claim if not already approved within x 
													days, for the purposes of absenteeism. If a cost centre structure is used for 
													workflow, the claim will automatically escalate to the owner of the higher cost 
													centre. Administrators can also manually re-direct claims to an alternative 
													approver and escalation approver.
													<li>
													Finance can check claims after approval and before being passed for payment. 
													They can reject individual lines back to the claimant, correct VAT, and they 
													can mark claims and claimants with highlighted comments e.g. if there is 
													suspicious activity. They can also block claims from being passed for payment.
													<li>
													Email notifications are issued to claimants when lines are rejected and claims 
													are approved. Emails are also issued daily to approvers with outstanding items.
													<li>
													Claimants have full visibility of the detail, attachments, audit trail and 
													progress of their claims, including all historic claims.
													<li>
														Extensive Management Information and Reporting, including the delivery of 
														scheduled tailored reports, ad hoc report generation online, and online summary 
														reporting.</li>
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
									<div class="container bg_cover bg_row_solutions_3rd om-animation-enabled">
										<div class="contentInner txt_white">
											<h3>THE PROBLEM</h3>
											It is estimated that billions of pounds are spent each year on business travel 
											worldwide and entertainment (T&amp;E) by employees whilst representing their 
											companies, but many organisations fail to manage this spend efficiently, 
											relying on multiple processes and solutions. This, together with the process of 
											tracking and reporting expenses costs more than 15% of the total spent. It is 
											imperative that companies find an efficient, cost effective solution as T&amp;E 
											can account for up to 10% of operating expenses, an area that for many is the 
											second largest controllable cost after payroll.
											<br>
											<br>
											Research found that 11% of all approved expense claims do not comply with 
											company policy. This figure rises to approximately 20% of all hotel claims and 
											29% of entertainment claims.
											<br>
											<br>
											Further research found that 15 percent of expense-claiming employees bump up 
											their expenses. Of these, 13 percent increase their claims by up to 25 Percent 
											whilst the remaining 2 percent add on even more.
											<br>
											<br>
											In 2009 over £8.8 billion was paid out by UK organisations to reimburse their 
											employees for expenses incurred – but around £2.1 billion of this should not 
											have been paid as it was for fraudulent and "out-of-policy" claims.
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
										<div class="contentInner clearfix">
											<h3>THE PROPOSITION</h3>
											The introduction of an automated solution will produce positive results for 
											most companies who recognise the need to streamline business processes and 
											increase control which in turn will provide reduced T&amp;E spend through 
											improved efficiency.
											<br>
											<br>
											What if this was augmented by the following?
											<br>
											<br>
											<ul>
												<li>
												Increased control of spending through policy compliance
												<li>
												Measure policy compliance using valuable expense data to drive efficient 
												spending
												<li>
												Dramatically reduce mistakes and fraud
												<li>
												Flexible Audit Tools
												<li>
												Advanced reporting and expense trend analysis
												<li>
												Immediate cash savings on processing costs of up to 80%
												<li>
												Pay for usage pricing plan
												<li>
												Negligible impact on internal IT resource
												<li>
												Rapid deployment
												<li>
													Online claim creation, approval workflow, archival and retrieval</li>
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
									<div class="container bg_cover bg_row_solutions_4th om-animation-enabled">
										<div class="contentInner txt_white">
											<h3>THE SOLUTION</h3>
											Introducing eExpenses, the intuitive and easy-to-use Web based interface that 
											streamlines and automates the entire expense management process with no 
											hardware or software investment or lengthy implementation required. Delivered 
											over the Web – and with nearly 10,000 users around the world – P2D's on-demand 
											solution enables employees to quickly create the expense reports that 
											supervisors, A/P managers, auditors and senior management can easily review, 
											approve, process and audit online at anytime and from anywhere, hassle-free.
											<br>
											<br>
											The system is fully tailored to your exact requirements, including validation 
											against all required company policy and rules.
											<br>
											<br>
											Company credit card, mobile phone and travel broker items can be pre-populated 
											for formal approval by the approving manager within the expense claim, 
											including the ability to credit out-of-policy expenditure.
											<br>
											<br>
											The system handles both multi-currency claims, and different base currencies of 
											users (for example, all figures are converted to a base currency of EUR for 
											claimants domiciled in EUR denominated countries).
											<br>
											<br>
											Expenses provides a comprehensive analysis of spend data for management review, 
											complying with corporate governance and government requirements including the 
											Sarbanes-Oxley Act – the standard by which financial controls for publicly held 
											US companies are measured.
											<br>
											<br>
											The solution is charged for via <a href="http://www.p2dgroup.net/" target="_blank">P2D</a>'s normal policy of 'Pay for Usage', rather 
											than a monthly fee per person, so you are not tied into paying disproportionate 
											amounts for infrequent users; or indeed forced to ring-fence such employees due 
											to cost.
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
									<div class="col-lrg-12">
										<div class="container">
											<div class="contentInner">
												<h3>THE EMPLOYEE</h3>
												The system is simple and easy to use saving the employee valuable time, and 
												making training and roll-out pain free.
												<br>
												Expenses operates on the basis of an 'in-tray' rather than traditional claim 
												'form' thereby allowing users to both enter items as and when they are incurred 
												for submission at a later date, as well as submitting multiple interim claims 
												to expedite approval and payment of urgent items.
												<br>
												<br>
												Previous claim items can be selected for pre-population in the current claim to 
												prevent continual re-keying of the same data.
												<br>
												<br>
												Receipts can either be attached online by the claimant or posted to our 
												scanning bureau where they will be scanned and automatically attached to the 
												claim.
												<br>
												<br>
												Email notifications are issued to the claimant if items have been rejected, or 
												approved for payment.
												<br>
												<br>
												Mileage can be automatically populated if postcodes are mandated to be entered; 
												or mileage can be validated after being entered by the user. Mileage Rates are 
												automatically populated by the system based on defined rules. Users can be 
												blocked from claiming mileage to comply with Corporate Manslaughter Law.
											</div>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- END: Four Section ---------->
                <!---------- START: Five Section ---------->
                <section id="page">
							<div class="row">
								<div class="container-fluid">
									<div class="bg_color_lightGrey clearfix">
										<div class="col-md-6">
											<div class="contentInner">
												<h3>THE APPROVAL</h3>
												Email notifications are issued to the approving manager when there are items to 
												approve.
												<br>
												<br>
												Similar to the principle applied to creating claims in eExpenses, the approval 
												of claims also operates as an in-tray. All claim lines from all sub-ordinates 
												are visible on the screen, thereby allowing the manager to approve all claims 
												via a single click, rather than dipping in and out of multiple claim forms. If 
												items are rejected by the approving manager, they are removed from the claim 
												and posted back to the claimant's in-tray (allowing them to amend and 
												re-submit, or delete), whilst the other acceptable lines are approved for 
												payment.
												<br>
												<br>
												Multiple levels of approval can be accommodated, based on value or any other 
												defined rules.
												<br>
												<br>
												The system automatically redirects claims to cope with absenteeism of approving 
												managers.
											</div>
										</div>
										<div class="col-md-6">
											<div class="contentInner">
												<h3>AUDIT TOOLS</h3>
												Full audit trail and reporting capabilities are provided.
												<br>
												<br>
												All current and historic claims can be retrieved at any time, subject to user 
												access rights. The system lists all claims made by the user and, if that user 
												is also a line manager, all claims made by sub-ordinates.
												<br>
												<br>
												Modules for final checking and approval by Finance are available, including the 
												automatic flagging of out-of-policy items.
												<br>
												<br>
												eExpenses will assist organisations to measure, manage and reduce their carbon 
												emissions by monitoring and reporting on the emissions of all major travel 
												methods by employee.
											</div>
										</div>
									</div>
								</div>
							</div>
						</section>
                <!---------- START: Five Section ---------->
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
                //$('.tab-content_02').removeClass('hide');
                //$(".tab-content_02").fadeIn(1500);
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
