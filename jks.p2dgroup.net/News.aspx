<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="CBSolutions.ETH.Web.News" %>

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
									<li><a  href="Customers.aspx" title="CUSTOMERS">CUSTOMERS</a></li>
									<li class="active"><a href="News.aspx" title="NEWS" href="javascript:void(0)">NEWS</a></li>
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
									<img class="img_ban" src="assets/img/ban/ban_news.jpg" alt="About Us::P2D Papper 2 Data">
									<h1>Award-Winning Cloud Solutions</h1>
								</div>
							</div>
						</div>
					</section>
            <!------------------------------ END: Banner Section ------------------------------>
            <!------------------------------ START: Container Section ------------------------------>
            <!---------- START: First Section ---------->
            <!---------- END: First Section ---------->
            <!---------- START: Second Section ---------->
            <%-- <section id="page">
						<div class="row">
							<div class="container-fluid">
								<div class="container">
									<div class="contentInner">
										<div id="pagination_list" class="newsarea">
											<h3>NEWS</h3>
                                            <h4 class="color_blue" id="newsGordon">GORDON RAMSAY GROUP SELECTS P2D’S NEW INVOICE PROCESSING SYSTEM</h4>
										     <strong>24<sup>th</sup> February 2015:</strong>The Gordon Ramsay Group comprises of the restaurant business of acclaimed 
                                             chef, restaurateur, TV personality and author Gordon Ramsay.  It employs more than 700 people in 
                                             London where it has a collection of 13 restaurants. The Group has a total of 25 restaurants globally 
                                             and 7 Michelin stars, with international restaurants from Europe to the US and the Middle East. 
                                             Following an assessment of the market, P2D’s latest version invoice processing solution was selected 
                                             by the Gordon Ramsay Group due to its depth of functionality, flexibility, service levels, price point 
                                             and experience in the hospitality sector. Fully rolled out, volumes are expected to be about 20,000 
                                             invoices per month. Robin Colla, P2D’s Chief Executive comments “Following many years of success 
                                             with blue-chip organisations in the retail sector, P2D has now fast become the go-to solution for 
                                             invoice processing in the hospitality market, with the system and service being tailored to the 
                                             specific needs of hospitality operators of all sizes, from start-ups to the likes of the Gordon Ramsay 
                                             Group.” 
                                             <hr>
                                             <h4 class="color_blue" id="newsCTBnw">CTB SIGNS UP FOR P2D’S NEW INVOICE PROCESSING SOLUTION</h4>
                                             <strong>20<sup>th</sup> February 2015:</strong>CTB Accounts is a specialized consultancy providing management accountancy 
                                             services to the restaurant industry. CTB has signed a new long-term contract with P2D to implement 
                                             its new system and services in order to process the invoices for its customer base with a current 
                                             total volume of over 12,000 per month. The new system, built in the latest .net version, delivers a 
                                             multitude of additional functionalities including a new high-speed coding and approval process that 
                                             will deliver substantial incremental labour savings. The scope of works for CTB comprises: scanning, 
                                             intelligent OCR data capture & validation, approvals workflow, online search & retrieval and 
                                             integration with Sage. The P2D service removes all data entry of invoices allowing CTB staff to 
                                             concentrate on value-added activities for their clients, and provides full search and retrieval of 
                                             invoices online to allow them to efficiently handle client queries. Please visit <a href="http://www.ctbaccounts.com/" target="_blank">
											 www.ctbaccounts.com</a>.
                                             for more information on CTB and the services it offers. 
                                             <hr>
                                             <h4 class="color_blue" id="newscommuni">COMMUNICORP SIGNS UP TO THE NEW P2D INVOICE PROCESSING SYSTEM</h4>
                                             <strong>9<sup>th</sup> January 2015:</strong>After nearly 10 years of using the P2D system through various corporate 
                                             restructures and acquisitions of the Real and Smooth Radio brands, Communicorp has re-assessed 
                                             the BPO market and concluded that P2D continues to offer the best solution for price, functionality 
                                             and quality of service. As such, Communicorp has re-signed with P2D and will be delivered its new 
                                             next generation invoice processing system in April 2015. This will be followed by a Single Sign On 
                                             implementation, Web Service integration with Microsoft Great Plains and the implementation of 
                                             P2D’s new Purchase Order Workflow & Matching product. For further information about 
                                             Communicorp, please visit <a href="http://www.communicorp.ie/" target="_blank">
											 www.communicorp.ie</a>.
                                            <hr>
                                            <h4 class="color_blue" id="newsETC">ETC HOSPITALITY ADD THEIR 29<sup>TH</sup> HIGH 
												PROFILE HOSPITALITY OPERATOR ONTO P2D</h4>
											<strong>3<sup>rd</sup> October 2014:</strong> P2D are pleased to announce that 
											ETC Hospitality have won another high-profile restaurant chain as a client of 
											their accounting and consultancy services. ETC has added them into their 
											portfolio of customers, for which they use P2D's invoice processing services. 
											The portfolio now totals 29 major hospitality businesses comprising high street 
											chains, catering companies and restaurant groups. ETC use P2D for a broad range 
											of invoice processing services including: scanning, email processing, 
											intelligent OCR data capture &amp; validation, approvals workflow, purchase 
											order matching, Invoice Analytics, online search &amp; retrieval and web 
											services integration with Aqilla online accounts; as well as a Cash Management 
											System joint venture developed for the hospitality sector. For more about ETC 
											Hospitality and its services please visit <a href="http://www.etchospitality.com/" target="_blank">
											www.etchospitality.com</a>.
											<hr>
											<h4 class="color_blue" id="news01">CTB ADD THEIR 20<sup>TH</sup> HIGH PROFILE RESTAURANT 
												BUSINESS ONTO P2D</h4>
											<strong>16<sup>th</sup> December 2014: </strong>P2D are pleased to announce that 
											CTB Accounts have won another high-profile cafe restaurant group as a client of 
											their accounting and consultancy services. CTB has added them into their 
											portfolio of customers, for which they use P2D's invoice processing services. 
											The portfolio now totals 20 major hospitality businesses comprising major 
											London restaurants / cafe groups. CTB use P2D for a broad range of invoice 
											processing services including: scanning, intelligent OCR data capture &amp; 
											validation, approvals workflow, Invoice Analytics, online search &amp; 
											retrieval and integration with Sage. Please visit <a href="http://www.ctbaccounts.com/" target="_blank">
												www.ctbaccounts.com</a> for more information on CTB and the services it 
											offers.
											<hr>
											<h4 class="color_blue" id="news02">TAILORED RECRUITMENT SERVICES LTD JOINS THE P2D 
												DOCUMENT NETWORK</h4>
											<strong>20<sup>th</sup> June 2014: </strong>Tailored Recruitment Services is a 
											specialist provider of HR Solutions to the UK’s manufacturing and logistics 
											industries. TRS has joined the P2D Document Network for e-invoicing, thereby 
											allowing it to send sales invoices electronically to customers via a single 
											integration to the P2D hub. In turn this allows them to minimise invoicing 
											costs through the removal of paper and postage, and achieve full visibility of 
											their receivables cashflow with the online invoice status information held in 
											the portal. TRS has a range of blue chip clients such as Marks &amp; Spencer, 
											B&amp;Q, Currys, Comet, Adidas, DHL, Pets at Home, NHS, Dunelm Mill, Brakes and 
											The Co-operative Group. For further information see <a href="http://www.tailoredrecruitmentservices.co.uk" target="_blank">
												www.tailoredrecruitmentservices.co.uk.</a>
											<hr>
										
											<h4 class="color_blue" id="news04">ROBIN COLLA SPEAKS TO YOUNG ENTREPRENEURS</h4>
											<strong>12<sup>th</sup> June 2014: </strong>Robin Colla, Chief Executive 
											Officer of P2D, was recently invited by Sherry Coutu CBE, Non-Exec Director at 
											the London Stock Exchange, to talk to young entrepreneurs on behalf of 
											Founders4Schools and in conjunction with the Young Enterprise scheme. Whilst 
											Founders4Schools work with schools and entrepreneurs around the country, this 
											was part of a young enterprise programme that has been sponsored by Virgin 
											Money and allows the very young entrepreneur to create their own business idea 
											and enjoy the excitement and buzz of making money. Robin comments "It was a 
											great pleasure to talk with, and answer questions for, these children who have 
											some great ideas for such a young group. I am sure the programme will be a huge 
											success."
											<hr>
											<h4 class="color_blue" id="news05">THE NEW MOON PUB GROUP IMPLEMENTS THE P2D 
												INVOICE PROCESSING SOLUTION</h4>
											<strong>20<sup>th</sup> May 2014:</strong> The New Moon Pub Group is an 
											expanding collection of high-end pubs developed and operated by industry 
											veterans David Mooney and Paul Newman. New Moon selected the P2D system for 
											invoice processing, including scanning, intelligent OCR data capture &amp; 
											validation, approvals workflow, Invoice Analytics, online search &amp; 
											retrieval and web services integration with Aqilla online accounts. Adopting 
											the P2D system is a strategic financial decision to automate processes and 
											support the aggressive expansion plans of the group. For further information 
											about the New Moon Pub Group, please visit their website: <a href="http://www.newmoonpub.co.uk/" target="_blank">
												www.newmoonpub.co.uk</a>.
											<hr>
											<h4 class="color_blue" id="news06">NEW LOOK ADOPTS P2D'S INVOICE ANALYTICS SERVICE 
												FOR 100% ACCURACY RATES</h4>
											<strong>28<sup>th</sup> April 2014:</strong> P2D has deployed its new Invoice 
											Analytics service into the processing it performs for high-street fashion 
											retailer New Look (<a href="www.newlook.com" target="_blank">www.newlook.com</a>). 
											P2D's invoice processing service already includes several layers of data 
											validation and data quality management which deliver true accuracy rates in 
											excess of 99.99%, something unparalleled in the industry. However, P2D's new 
											Invoice Analytics service compounds these accuracy rates even further by 
											incorporating highly sophisticated intelligence diagnostics on the output data 
											to highlight possible anomalies which are then triple-checked, and if necessary 
											corrected, by the Support team. Robin Colla, P2D's Chief Executive, comments 
											"We have been performing the outsourced processing of New Look's global 
											invoices for more than 8 years now and the relationship continues to go from 
											strength to strength. Our reputation for delivering the highest possible levels 
											of accuracy and service underpin this."
											<hr>
											<h4 class="color_blue" id="news07">STRONG GROWTH PREDICTED IN E-INVOICING</h4>
											<strong>25<sup>th</sup> April 2014:</strong> TechNavio forecasts that the 
											Global e-Invoicing market will grow at a compound annual growth rate of 23.3% 
											over the period 2013 to 2018, driven by a need to automate the invoicing 
											process and reduce operational costs. The report acknowledges an emerging 
											market trend towards Cloud Computing, like the solution offered by P2D, and an 
											increasing adoption by SMEs. It also notes that e-invoicing "offers a number of 
											benefits to organizations including streamlining of accounting processes, 
											reducing paper consumption, offering higher receivables, helping in timely 
											delivery of invoices and faster customer payments, reducing the number of lost 
											invoices, and increasing visibility and control over various processes. 
											Enterprises are increasingly using e-invoicing solutions to simplify and 
											automate the process of creating, sending, and receiving invoices. E-invoicing 
											is a cost-effective solution that eliminates paper-based manual invoicing and 
											replaces it with a more reliable and faster invoicing system." Please contact 
											us for more information on the P2D e-invoicing solutions and to discuss how we 
											can help your organisation, whether large or small, to achieve these goals.
											<hr>
											<h4 class="color_blue" id="news08">ST JAMES'S HOTEL GROUP IMPLEMENTS THE P2D 
												INVOICE PROCESSING SOLUTION</h4>
											<strong>12<sup>th</sup> April 2014:</strong> St James's Hotel Group comprises a 
											collection of 14 large hotels across the UK, formerly known as Forestdale 
											Hotels. Locations include Bristol, Bath, Southampton, Bournemouth, Winchester, 
											Cambridge, Oxford, Barnsley, Darlington, Cirencester, Arundel, Ringwood and 
											Lyndhurst. St James implemented the P2D invoice processing solution integrated 
											with SUN Accounts in April 2014. The service includes scanning, intelligent OCR 
											data capture &amp; validation, approvals workflow, online search &amp; 
											retrieval and Invoice Analytics. The purchase of Forestdale is the first in a 
											series of planned acquisitions for St James's Hotels, which was formed recently 
											by the privately-owned European investor Somerston Group (<a href="http://www.somerston.com/" target="_blank">www.somerston.com</a>).
											<hr>
											<h4 class="color_blue" id="news09">COMMUNICORP SELECTS P2D FOR INVOICE PROCESSING</h4>
											<strong>4<sup>th</sup> April 2014:</strong> Founded in 1989, Communicorp Group 
											Limited operates a portfolio of media and radio assets in Ireland, Europe and 
											Jordan. It is the leading Irish commercial radio broadcaster and it operates 
											the largest independent radio groups in both BBBRaria and Latvia. Communicorp 
											adopted the P2D system for invoice processing in April 2014 following the 
											high-profile UK acquisition of Smooth and Real Radio from Global Radio. The 
											scope of services include: intelligent OCR data capture, approvals workflow, 
											Invoice Analytics, online search &amp; retrieval and integration with Microsoft 
											Great Plains. For further information about Communicorp, please visit <a href="http://www.communicorp.ie/" target="_blank">
												www.communicorp.ie</a>.
											<hr>
											<h4 class="color_blue" id="news10">CO-OP ANNOUNCE PLANS TO ROLLOUT P2D eEXPENSES TO 
												A FURTHER 10,000 USERS</h4>
											<strong>24<sup>th</sup> February 2014:</strong> The Co-operative Group 
											implemented P2D's eExpenses solution in August 2010. Reflecting the project's 
											success and the system's robust capability to handle the huge volume and user 
											base associated with what is one of the largest expense claim system 
											implementations in the UK in many years, the Co-op now announce plans to roll 
											the system out into their 5,000 stores to a further 10,000 employees. With 
											revenues of nearly £14bn, The Co-operative Group are the UK's fifth largest 
											food retailer, the third largest retail pharmacy chain, the number one provider 
											of funeral services and the largest independent travel business (<a href="http://www.co-operative.coop/" target="_blank">www.co-operative.coop</a>). 
											Robin Colla, P2D's Chief Executive, remarks "The Co-operative Group's roll-out 
											plan is testament to the stability of the solution, its ability to deliver on a 
											huge scale, and the unparalleled quality of support P2D provides. We are 
											delighted to be working with the Co-op to enable further automation and cost 
											savings, and look forward to developing a continued relationship with them".
											<hr>
											<h4 class="color_blue" id="news11">FREEDOM HOTEL GROUP SELECTS P2D</h4>
											<strong>21<sup>st</sup> February 2014:</strong> The Freedom Hotel Group forms 
											part of the Crieff Hydro family of hotels. Its hotels are located across 
											Scotland and the North of England. Freedom selected P2D to continue to automate 
											the invoice processing of the hotels recently acquired from the Akkeron Hotel 
											Group. The implemented system interfaces to SUN Accounts, and comprises the 
											following scope: scanning, email processing, intelligent OCR data capture &amp; 
											validation, approvals workflow, Invoice Analytics, and online search &amp; 
											retrieval. For more information about The Freedom Hotel Group, please visit 
											their website <a href="http://www.crieffhydro.com/" target="_blank">www.crieffhydro.com</a>.
											<hr>
											<h4 class="color_blue" id="news12">P2D LAUNCH 24-HOUR TURNAROUND GUARANTEE</h4>
											<strong>1<sup>st</sup> February 2014:</strong> Following the introduction of 
											P2D's specialised invoice processing solution for the hospitality market in 
											2012, and its subsequent adoption by a variety of hotel, restaurant and cafe 
											groups, it has become increasingly important to provide a 24-hour turnaround 
											guarantee. Unlike all other industry sectors which will typically have a 
											regular daily invoice volume, our hospitality customers (in particular the 
											accounting practices with multiple hospitality sub-clients) are prone to truly 
											enormous volume spikes due to the nature of their processes, yet at the same 
											time require all invoices to be scanned, processed and in the accounting system 
											within 24 hours. As part of the delivery of our 100% accuracy guarantee, P2D's 
											Data Validation Team check every piece of data on every invoice (which in 
											itself is something quite unique in the industry). Therefore from a resourcing 
											point of view, managing these huge volume spikes is challenging. To respond to 
											the demands of the hospitality market and tackle this challenge, P2D have 
											created a flexible resourcing model so that the size of the team can expand and 
											contract by up to 3 times on a day-to-day basis to meet demand. James Enstone, 
											P2D's CIO, comments "Managing daily swings in volume of several thousand 
											invoices is quite a challenge so I'm delighted that we have managed to engineer 
											a solution for our clients. A 100% accuracy guarantee coupled with a 24-hour 
											turnaround guarantee really does set us apart in the invoice processing 
											market".
											<hr>
											<h4 class="color_blue" id="news13">SEGMETRIX TO USE P2D'S INVOICE PROCESSING 
												SOLUTION</h4>
											<strong>23<sup>rd</sup> December 2013:</strong> Segmetrix, the B2B segmentation 
											and analytics company, is to use P2D for its invoice processing. Segmetrix has 
											won a string of awards including B2B Marketing Awards Winner 2008, Information 
											Management Innovative Solution Award 2010, and BeyeNetwork Vision Award 2010, 
											enabling clients to integrate their Sales, Marketing and Finance teams around 
											operational goals and provide them with a proven, systematic way of repeating 
											success. The P2D solution will automatically process invoices received by email 
											and seamlessly post them into Twinfield cloud accounting system via web 
											services integration, as well as provide online search &amp; retrieval. 
											Segmetrix is the 4th client introduced to P2D by Plummer Parsons accounting 
											practice.
											<hr>
											<h4 class="color_blue">PLUMMER PARSONS ADDS WILTSHIRE FARM FOODS INTO ITS P2D 
												PORTFOLIO</h4>
											<strong>1<sup>st</sup> November 2013:</strong> After going live in August, 
											Plummer Parsons accounting practice has added Wiltshire Farm Foods into its P2D 
											portfolio for invoice processing. Wiltshire Farm Foods, recognised by its 
											Ronnie Corbett TV adverts, is the UK's leading frozen meals home delivery 
											service. They have been a long-standing client of P2D and requested that they 
											continue to receive the P2D service from their new accountants, Plummer 
											Parsons. Wiltshire Farm Foods receive a service from P2D which comprises the 
											automated processing of purchase invoices sent by both email and paper form, 
											which are automatically coded and posted into Twinfield online accounts, 
											removing the entire manual process for Plummer Parsons and allowing all parties 
											to search for, and view, invoices online.
											<hr>
											<h4 class="color_blue">NEW LOOK ADOPT P2D'S EMAIL INVOICE PROCESSING SERVICE</h4>
											<strong>9<sup>th</sup> September 2013:</strong> Following the launch of P2D's 
											email processing capability in January, we are pleased to announce that New 
											Look, the high street fashion retailer, has adopted the service. P2D has 
											already been processing New Look's global invoice volume for several years via 
											a combination of e-invoicing and OCR. New Look found that it was increasingly 
											receiving more invoices from its suppliers by email, and that using P2D's email 
											processing service was a natural add-on to remove even more paper from the 
											operation, and further reduce internal time and cost. The service allows New 
											Look to email its invoices into different email accounts which each relate to a 
											batch type, and thereby dictate the onward workflow in P2D: for example, 'Goods 
											For Resale' vs. 'Goods Not For Resale' invoices. The invoices received are 
											automatically stripped from the emails and subjected to P2D's award-winning 
											100% accuracy intelligent data capture.
											<hr>
											<h4 class="color_blue">PLUMMER PARSONS ACCOUNTING PRACTICE SELECTS P2D FOR INVOICE 
												PROCESSING</h4>
											<strong>6<sup>th</sup> August 2013:</strong> Plummer Parsons is an accountancy 
											firm with 5 offices in the south of England and celebrated its 50th anniversary 
											in October 2013. Having adopted Twinfield cloud accountancy software, they were 
											looking for a partner that could provide efficient, cost effective invoice 
											automation for their clients and chose P2D as that partner. The service 
											initially went live with 2 clients, Ampere Technical Services &amp; 
											Match2Lists, and incorporates the automated processing of emailed invoices, 
											intelligent data capture &amp; validation, automated coding, online search 
											&amp; retrieval and seamless web services integration with Twinfield. Plummer 
											Parsons will be rolling the system out across other clients in 2014-15. The 
											service delivers significant benefits to both Plummer Parsons and clients 
											alike, as the process is fully managed by P2D. Now, Plummer Parsons only needs 
											to send the invoices to P2D, and flag the transactions for payment in 
											Twinfield; everything else is done! The P2D system is also HMRC approved, and 
											hosts the documents and data for 7 years, thereby also removing paper storage 
											costs. To find out more about Plummer Parsons, please visit <a href="http://www.plummer-parsons.co.uk/" target="_blank">
												www.plummer-parsons.co.uk</a>.
											<hr>
											<h4 class="color_blue">ASOS GO LIVE GLOBALLY WITH P2D'S eEXPENSES SYSTEM</h4>
											<strong>14<sup>th</sup> June 2013:</strong> ASOS PLC is the UK's largest 
											independent online fashion and beauty retailer servicing 241 countries. It 
											generates annual revenues of c. £750m and has a market capitalization of £3bn. 
											ASOS selected P2D's online employee expense claim system which was successfully 
											implemented this month. The system has been rolled out globally across the 
											entire ASOS group structure, and incorporates full international tax 
											calculations. "ASOS is yet another big-ticket retailer to select P2D as a 
											processing partner for a core financial process. Once again this reflects P2D's 
											ability to provide highly secure, highly adaptable solutions to large complex 
											blue-chip corporates, embraced by a truly unbeatable support framework", Robin 
											Colla, P2D Chief Executive. For further innformation about ASOS, visit their 
											website: <a href="http://www.asos.com/" target="_blank">www.asos.com</a>.
											<hr>
											<h4 class="color_blue">P2D LAUNCH INVOICE ANALYTICS PACKAGE TO DELIVER 100% 
												ACCURACY GUARANTEE</h4>
											<strong>7<sup>th</sup> June 2013:</strong> P2D is pleased to announce the 
											launch of its new Invoice Analytics service. The current service already 
											includes several layers of data validation and data quality management which 
											deliver true accuracy rates in excess of 99.99%, something unparalleled in the 
											industry. However, P2D's new Invoice Analytics package compounds these accuracy 
											rates even further through the use of highly sophisticated intelligence 
											diagnostics on the output data to highlight possible anomalies which are then 
											triple-checked, and if necessary corrected, by our Support team. Robin Colla, 
											P2D's CEO, comments "Our ability to deliver the highest levels of accuracy has 
											always set us apart from the rest of the market. The new Invoice Analytics 
											software we have developed takes this even further and allows us to provide a 
											100% accuracy guarantee to our clients, something no other service provider can 
											offer".
											<hr>
											<h4 class="color_blue">P2D LAUNCH SINGLE SIGN ON SERVICES</h4>
											<strong>15<sup>th</sup> May 2013:</strong> In response to the growing trend for 
											companies to configure their network logon via a 'Single Sign On' approach, P2D 
											has developed the capability to deploy its solutions for any SSO method. If you 
											are considering using P2D's service, please contact us and we would be happy to 
											provide full details of our technical capabilities in this area. "We are 
											increasingly being asked by our clients to deploy our solutions via SSO, so 
											have launched a capability to deliver this to any required protocol that a 
											client may have. Much like our ability to integrate with any accounting, ERP or 
											back-end system, P2D always strives to offer the ultimate in flexibility to 
											meet the demands of our existing and future clients", adds James Enstone, P2D's 
											Chief Information Officer.
											<hr>
											<h4 class="color_blue">WARD WILLIAMS CHARTERED ACCOUNTANTS SIGNS UP FOR P2D</h4>
											<strong>30<sup>th</sup> April 2013:</strong> Ward Williams accounting practice 
											has selected P2D's automated invoice processing solution that interfaces with 
											the Twinfield cloud accounting system. Ward Williams has 5 offices in the South 
											of England and is currently undergoing a project to implement Twinfield for its 
											client base. Twinfield is Europe's largest online accounting system. The P2D 
											solution receives invoices from Ward Williams by email, and automatically 
											processes them into Twinfield via a seamless web services integration. This 
											allows Ward Williams to remove the labour-intensive activity of entering and 
											coding invoices manually, and at the same time provides full search and 
											retrieval in the cloud either from P2D or indeed Twinfield itself via a 
											punch-out back to P2D's image respository. Naturally, this also removes the 
											cost of storing paper invoices for 7 years to meet HMRC requirements. For more 
											information on Ward Williams, please visit <a href="http://www.wardwilliams.co.uk/" target="_blank">
												www.wardwilliams.co.uk</a>.
											<hr>
											<h4 class="color_blue">NEW LOOK COMPLETES 'GOODS NOT FOR RESALE' PURCHASE ORDER 
												MIGRATION</h4>
											<strong>26<sup>th</sup> April 2013:</strong> New Look, the high street fashion 
											retailer and a long-standing client of P2D, has gone live with the switchover 
											to purchase ordering for its 'GNFR' suppliers. P2D has supported the New Look 
											project over the last year with a variety of planning activities, system 
											changes and migration work. In turn this means that all New Look suppliers must 
											now quote a valid Purchase Order Number on their invoices. GNFR suppliers that 
											send invoices electronically over the P2D portal will be informed, at the 
											appropriate time, as to when this field will become mandatory to populate in 
											their submissions.
											<hr>
											<h4 class="color_blue">ASTRAL FIRE SIGNS UP FOR E-INVOICING</h4>
											<strong>25<sup>th</sup> April 2013:</strong> Astral Fire &amp; Security has 
											signed up to the P2D Document Network for e-invoicing. It is therefore now 
											capable of sending sales invoices electronically to meet the demands of its 
											clients currently using P2D, and also any future clients. Joining the network 
											allows Astral to promote itself to other clients as having e-invoicing 
											capability via P2D. Astral is a fire and security systems specialist with a 
											range of system services including: Design, Installation, Commissioning, 
											Service, Maintenance, Repair and Upgrade. It has clients within the Retail, 
											Public, Industrial and Leisure sectors such as Whitbread, New Look and Colliers 
											International. See <a href="http://www.astralfireandsecurity.com/" target="_blank">www.astralfireandsecurity.com</a>
											for further details.
											<hr>
											<h4 class="color_blue">TENNALS FACILITIES MANAGEMENT JOIN THE P2D DOCUMENT NETWORK</h4>
											<strong>18<sup>th</sup> April 2013:</strong> Tennals FM is a national provider 
											of facilities management and property maintenance solutions. Tennals work on 
											behalf of a various blue chip organisations undertaking works on a national and 
											regional basis for both single and multi-site delivery. By signing up to P2D 
											for e-invoicing, Tennals can service the demands of its clients for paperless 
											invoicing, as well as reap the benefits of having an online portal to access 
											all sales invoices and monitor invoice status. This in turn allows Tennals to 
											have full transparency of its receivables cashflow, and remove any payment 
											bottlenecks long before the payment due date. Visit Tennals website for more 
											information <a href="http://www.tennalsfm.com/" target="_blank">www.tennalsfm.com</a>.
											<hr>
											<h4 class="color_blue">FACILITIES SERVICES GROUP GO LIVE WITH P2D E-INVOICING</h4>
											<strong>13<sup>th</sup> March 2013:</strong> Facilities Services Group is a 
											market leading integrated facilities management service provider, with a 
											turnover of more than £20m. Facilities Services Group selected P2D to provide 
											an e-invoicing programme of PO Flips, e-invoicing, PO matching and supplier 
											portal to process the invoices it receives in servicing its clients. FSG's 
											clients include Starbucks, Debenhams, Nandos, Travelodge, Dreams, The 
											Co-operative, Serco, Knight Frank, Cancer Research and Majestic Wine among 
											others. The system will allow them to process nearly all invoices straight 
											through, by receiving them electronically with automated validation, matching 
											to PO, and coding; thereby removing the substantial internal cost of processing 
											paper invoices. FSG is now part of the Servest Group; for more information 
											please visit their website <a href="http://servest.co.uk/" target="_blank">www.servest.co.uk</a>.
											<hr>
											<h4 class="color_blue">PICKERINGS LIFTS JOINS THE P2D DOCUMENT NETWORK</h4>
											<strong>6<sup>th</sup> February 2013:</strong> Pickerings Lifts is the UK's 
											leading and largest independent lift manufacturer. With over 150 years of 
											experience, Pickerings designs, installs, maintains and repairs lifts, 
											escalators and loading bay equipment. By signing up to the P2D Document 
											Network, Pickerings is able to send its sales invoices to customers 
											electronically. In turn this provides them with the following key benefits: 
											customer satisfaction / retention; removal of paper and postage costs; 
											avoidance of lost invoices and payment delays; improved cashflow. The P2D 
											system also allows users to search for and retrieve all invoices at any time, 
											as well as monitor their stage of processing / approval. Visit <a href="http://www.pickeringslifts.co.uk/" target="_blank">
												www.pickeringslifts.co.uk</a> for more about Pickerings Lifts.
											<hr>
											<h4 class="color_blue">P2D LAUNCH EMAIL PROCESSING SERVICE</h4>
											<strong>23<sup>rd</sup> January 2013:</strong> P2D is pleased to announce the 
											launch of its new service to automatically process emailed invoices. 
											E-invoicing in its true sense is the transfer of raw data from supplier to 
											buyer. However for low volume, temporary, or ad hoc suppliers emailing invoices 
											tends to be the easiest and therefore preferred approach for many. To embrace 
											this and provide further process automation for its clients, P2D has developed 
											the capability to receive emailed invoices from which the attachments are 
											automatically stripped and subjected to P2D's award-winning 100% accuracy 
											intelligent data capture. The service allows clients to email their invoices 
											into different email accounts which each relate to a batch type, and thereby 
											dictate the onward workflow in P2D. Customers such as Twinfield, ETC 
											Hospitality and Wiltshire Farm Foods have already signed up to the service.
											<hr>
											<h4 class="color_blue">CTB ACCOUNTS SELECTS P2D FOR INVOICE PROCESSING</h4>
											<strong>18th December 2012:</strong> CTB Accounts is a specialized consultancy 
											providing management accountancy services to the restaurant industry. CTB has 
											selected P2D to provide its system and services in order to process the 
											invoices for its customer base with a current total volume of over 10,000 per 
											month. The scope comprises: scanning, intelligent OCR data capture &amp; 
											validation, approvals workflow, online search &amp; retrieval and integration 
											with Sage. The P2D service removes all data entry of invoices allowing CTB 
											staff to concentrate on value-added activities for their clients, and provides 
											full search and retrieval of invoices online to allow them to efficiently 
											handle client queries. The project goes live in January 2013 and will continue 
											to roll-out across all its customers during 2013 compounding the efficiency 
											gains. Please visit <a href="http://www.ctbaccounts.com/" target="_blank">www.ctbaccounts.com</a>
											for more information on CTB and the services it offers.
											<hr>
											<h4 class="color_blue">INTER-CITY GROUP JOINS THE P2D DOCUMENT NETWORK</h4>
											<strong>17<sup>th</sup> December 2012:</strong> Inter-City Group was formed in 
											1981 and has since developed into a major watch, gift and jewellery licensee 
											holder and distributor. Inter-City has ISO certified manufacturing facilities 
											in China and Hong Kong, and offices in Europe and the Far East. By signing up 
											to the P2D Document Network, Inter-City now has the capability to send sales 
											invoices electronically to clients such as New Look, that are immediately 
											integrated into their accounting systems and workflows to ensure there is no 
											risk of delayed payments. Not only does this maximise cashflow but also removes 
											the cost of paper, postage and labour. For more information on Inter-City visit <a href="http://www.icw-watches.co.uk/lo/icw_index.html" target="_blank">
												www.icw-watches.co.uk</a>.
											<hr>
											<h4 class="color_blue">TOTALIS SIGNS UP FOR SALES INVOICE PROCESSING</h4>
											<strong>15<sup>th</sup> October 2012:</strong> Operating throughout the UK and 
											Ireland, Totalis Solutions provides service solutions tailored to meet the 
											needs of a variety of local, national and multi-national clients such as 
											Morrisons, Lidl, Iceland, Poundland, Texaco, William Hill, Belfast City 
											Council, KwikFit and Danke Bank. Totalis has signed up to the P2D Document 
											Network in order to send its sales invoices electronically. Totalis selected an 
											integration method that best suited its internal systems and processes which 
											performs a single link into P2D which is in turn capable of passing on the 
											invoices to multiple customers in their own required format and protocol. Using 
											the network therefore removes a huge duplication of cost in setting up, and 
											supporting, a different interface / integration with each client for 
											e-invoicing. Please see <a href="http://www.totalissolutions.co.uk/" target="_blank">
												www.totalissolutions.co.uk</a>.
											<hr>
											<h4 class="color_blue">MSL PROPERTY CARE JOINS THE P2D DOCUMENT NETWORK</h4>
											<strong>4<sup>th</sup> October 2012:</strong> Through five specialist divisions 
											MSL offer a complete property care solution encompassing reactive and planned 
											maintenance, statutory and environmental compliance assurance programmes and 
											building fabric works. MSL have signed up to the P2D Document Network to 
											service the e-invoicing requests of its customers. This allows them to remit 
											invoices and credit notes through the P2D system via a method of their choice, 
											which are then immediately passed into the customer's PO matching or approvals 
											workflow. Hence there are no longer any lost invoices, delays due to postage, 
											duplicate invoices, and MSL can monitor the approval of all their sales 
											invoices online. For additional information on MSL Property Care please visit <a href="http://www.msl-ltd.co.uk/" target="_blank">
												www.msl-ltd.co.uk</a>.
											<hr>
											<h4 class="color_blue">P2D LAUNCH AQILLA WEB SERVICES</h4>
											<strong>19<sup>th</sup> September 2012:</strong> P2D is pleased to launch its 
											new web service integration with Aqilla, the online accounting system (<a href="http://www.aqilla.com/" target="_blank">www.aqilla.com</a>). 
											The integration provides a seamless link between the two systems for posting 
											invoice and credit note transactions that have been processed within P2D. The 
											web service automatically transfers the invoice data securely over the internet 
											at scheduled times during the day. Duplicate invoices are rejected and marked 
											as such in the P2D workflow together with the rejection reason provided by 
											Aqilla. For invoices that do not require manual approval such as those with 
											purchase order numbers, or Food &amp; Beverage invoices in the hospitality 
											market, they can be processed straight through meaning there is a huge labour 
											and cost reduction for the client who would hitherto have processed everything 
											manually as paper.
											<hr>
											<h4 class="color_blue">ETC AND P2D LAUNCH NEW CASH MANAGEMENT SYSTEM</h4>
											<strong>17<sup>th</sup> September 2012:</strong> ETC Hospitality, leading 
											provider of cloud accounting solutions to the hospitality market, has developed 
											a new cash management system called WTR (Weekly Trading Report) in partnership 
											with P2D. ETC provided a specification of how the system should work to satisfy 
											its clients' needs and P2D developed the system accordingly and integrated it 
											into their invoice processing system. The system has also been built with a 
											standard interface to Aqilla, the online accounting system that ETC use for all 
											of its clients. For more about ETC Hospitality and its service offering, please 
											visit their website <a href="http://www.etchospitality.com/" target="_blank">www.etchospitality.com</a>.
											<hr>
											<h4 class="color_blue">GSH GROUP SIGNS UP FOR SALES INVOICE PROCESSING</h4>
											<strong>27<sup>th</sup> June 2012:</strong> GSH Group has signed up to the P2D 
											Document Network for e-invoicing. GSH is a leading provider of 
											technology-driven facilities and energy management solutions, throughout the 
											UK, Europe, India and US, employing over 2,400 staff. It is therefore now 
											capable of sending sales invoices electronically to meet the global demands of 
											clients using P2D, as well as being able to promote itself to other clients as 
											having e-invoicing capability. In addition to meeting customer requirements the 
											system also allows GSH to monitor search for and retrieve all invoices online, 
											as well as receive updates on their status, thereby allowing them to monitor 
											cashflow. For more on GSH Group, visit <a href="http://www.gshgroup.com/" target="_blank">
												www.gshgroup.com</a>.
											<hr>
											<h4 class="color_blue">SPV GROUP JOINS THE P2D DOCUMENT NETWORK</h4>
											<strong>21<sup>st</sup> June 2012:</strong> SPV Group operates a nationwide 
											service for the refurbishment and maintenance of buildings in the industrial 
											and commercial sectors. SPV has signed up to the P2D Document Network for 
											e-invoicing, thereby allowing it to send sales invoices electronically to 
											customers via a single integration to the P2D hub. In turn this allows them to 
											minimise invoicing costs through the removal of paper and postage, and achieve 
											full visibility of their receivables cashflow with the online invoice status 
											information held in the portal. SPV has a range of high-profile clients such as 
											Lidl, Iceland, Bosch, Esporta, Odeon, Holiday Inn and Wetherspoons. For further 
											information see <a href="http://www.spv-uk.co.uk/" target="_blank">www.spv-uk.co.uk</a>.
											<hr>
										</div>
									</div>
                                    <!--
                                    <div class="pull-right">
										<ul class="pagination pagination-large nomarginTop">
											<li class="disabled">
												<a href="#"><i class="fa fa-angle-double-left"></i>Prev</a>
											<li class="active">
												<a href="#">1
													<span class="sr-only">(current)</span></a>
											<li>
												<a href="#">2</a>
											<li>
												<a href="#">3</a>
											<li>
												<a href="#">4</a>
											<li>
												<a href="#">Next <i class="fa fa-angle-double-right"></i></a>
											</li>
										</ul>
									</div> 
                                    --->
								</div>
							</div>
						</div>
					</section>--%>
            <section id="page">
						<div class="row">
							<div class="container-fluid">
								<div class="container">
									<div class="contentInner">
										<div id="pagination_list" class="newsarea">
											<h3>NEWS</h3>
											<%-------------------------------------------------------%>
											<h4 class="color_blue" id="newsGordon">GORDON RAMSAY GROUP SELECTS P2D’S NEW 
												INVOICE PROCESSING SYSTEM</h4>
											<strong>24<sup>th</sup> February 2015:</strong>The Gordon Ramsay Group 
											comprises of the restaurant business of acclaimed chef, restaurateur, TV 
											personality and author Gordon Ramsay. It employs more than 700 people in London 
											where it has a collection of 13 restaurants. The Group has a total of 25 
											restaurants globally and 7 Michelin stars, with international restaurants from 
											Europe to the US and the Middle East. Following an assessment of the market, 
											P2D’s latest version invoice processing solution was selected by the Gordon 
											Ramsay Group due to its depth of functionality, flexibility, service levels, 
											price point and experience in the hospitality sector. Fully rolled out, volumes 
											are expected to be about 20,000 invoices per month. Robin Colla, P2D’s Chief 
											Executive comments “Following many years of success with blue-chip 
											organisations in the retail sector, P2D has now fast become the go-to solution 
											for invoice processing in the hospitality market, with the system and service 
											being tailored to the specific needs of hospitality operators of all sizes, 
											from start-ups to the likes of the Gordon Ramsay Group.The addition of such a
                                            prestigious client is testament to the quality of the new system we are launching.”
                                            For more information on the Gordon Ramsay Group, please visit www.gordonramsay.com.
											<hr>
											<h4 class="color_blue" id="newsCTBnw">CTB SIGNS UP FOR P2D’S NEW INVOICE PROCESSING 
												SOLUTION</h4>
											<strong>20<sup>th</sup> February 2015:</strong>CTB Accounts is a specialized 
											consultancy providing management accountancy services to the restaurant 
											industry. CTB has signed a new long-term contract with P2D to implement its new 
											system and services in order to process the invoices for its customer base with 
											a current total volume of over 12,000 per month. The new system, built in the 
											latest .net version, delivers a multitude of additional functionalities 
											including a new high-speed coding and approval process that will deliver 
											substantial incremental labour savings. The scope of works for CTB comprises: 
											scanning, intelligent OCR data capture & validation, approvals workflow, online 
											search & retrieval and integration with Sage. The P2D service removes all data 
											entry of invoices allowing CTB staff to concentrate on value-added activities 
											for their clients, and provides full search and retrieval of invoices online to 
											allow them to efficiently handle client queries. Please visit <a href="http://www.ctbaccounts.com/" target="_blank">
												www.ctbaccounts.com</a>. for more information on CTB and the services it 
											offers.
											<hr>
											<h4 class="color_blue" id="newscommuni">COMMUNICORP SIGNS UP TO THE NEW P2D INVOICE 
												PROCESSING SYSTEM</h4>
											<strong>9<sup>th</sup> January 2015:</strong>After nearly 10 years of using the 
											P2D system through various corporate restructures and acquisitions of the Real 
											and Smooth Radio brands, Communicorp has re-assessed the BPO market and 
											concluded that P2D continues to offer the best solution for price, 
											functionality and quality of service. As such, Communicorp has re-signed with 
											P2D and will be delivered its new next generation invoice processing system in 
											April 2015. This will be followed by a Single Sign On implementation, Web 
											Service integration with Microsoft Great Plains and the implementation of P2D’s 
											new Purchase Order Workflow & Matching product. For further information about 
											Communicorp, please visit <a href="http://www.communicorp.ie/" target="_blank">www.communicorp.ie</a>.
											<hr>
											<%-------------------------------------------------------%>
											<h4 class="color_blue" id="news01">CTB ADD THEIR 20TH HIGH PROFILE RESTAURANT 
												BUSINESS ONTO P2D</h4>
											<strong>16<sup>th</sup> December 2014: </strong>P2D are pleased to announce 
											that CTB Accounts have won another high-profile cafe restaurant group as a 
											client of their accounting and consultancy services. CTB has added them into 
											their portfolio of customers, for which they use P2D's invoice processing 
											services. The portfolio now totals 20 major hospitality businesses comprising 
											major London restaurants / cafe groups. CTB use P2D for a broad range of 
											invoice processing services including: scanning, intelligent OCR data capture 
											&amp; validation, approvals workflow, Invoice Analytics, online search &amp; 
											retrieval and integration with Sage. Please visit <a href="http://www.ctbaccounts.com/" target="_blank">
												www.ctbaccounts.com</a> for more information on CTB and the services it 
											offers.
											<hr>
											
											<%------------------------------Start New Changed On 020315--------------%>
											
											<h4 class="color_blue" id="news03">ETC HOSPITALITY ADD THEIR 29<sup>TH</sup> HIGH 
												PROFILE HOSPITALITY OPERATOR ONTO P2D</h4>
											<strong>3<sup>rd</sup> October 2014: </strong>P2D are pleased to announce that 
											ETC Hospitality have won another high-profile restaurant chain as a client of 
											their accounting and consultancy services. ETC has added them into their 
											portfolio of customers, for which they use P2D’s invoice processing services. 
											The portfolio now totals 29 major hospitality businesses comprising high street 
											chains, catering companies and restaurant groups. ETC use P2D for a broad range 
											of invoice processing services including: scanning, email processing, 
											intelligent OCR data capture & validation, approvals workflow, purchase order 
											matching, Invoice Analytics, online search & retrieval and web services 
											integration with Aqilla online accounts; as well as a Cash Management System 
											joint venture developed for the hospitality sector. For more about ETC 
											Hospitality and its services please visit <a href="http://www.etchospitality.com" target="_blank">
												www.etchospitality.com.</a>
												
											<hr>
											<%------------------------------End New Changed On 020315--------------%>
											
											
											
											<h4 class="color_blue" id="news02">TAILORED RECRUITMENT SERVICES LTD JOINS THE P2D 
												DOCUMENT NETWORK</h4>
											<strong>20<sup>th</sup> June 2014: </strong>Tailored Recruitment Services is a 
											specialist provider of HR Solutions to the UK’s manufacturing and logistics 
											industries. TRS has joined the P2D Document Network for e-invoicing, thereby 
											allowing it to send sales invoices electronically to customers via a single 
											integration to the P2D hub. In turn this allows them to minimise invoicing 
											costs through the removal of paper and postage, and achieve full visibility of 
											their receivables cashflow with the online invoice status information held in 
											the portal. TRS has a range of blue chip clients such as Marks &amp; Spencer, 
											B&amp;Q, Currys, Comet, Adidas, DHL, Pets at Home, NHS, Dunelm Mill, Brakes and 
											The Co-operative Group. For further information see <a href="http://www.tailoredrecruitmentservices.co.uk" target="_blank">
												www.tailoredrecruitmentservices.co.uk.</a>
											<hr>
										<%--	<h4 class="color_blue" id="news03">ETC HOSPITALITY ADD THEIR 23<sup>RD</sup> HIGH 
												PROFILE HOSPITALITY OPERATOR ONTO P2D</h4>
											<strong>17<sup>th</sup> June 2014:</strong> P2D are pleased to announce that 
											ETC Hospitality have won another high-profile restaurant chain as a client of 
											their accounting and consultancy services. ETC has added them into their 
											portfolio of customers, for which they use P2D's invoice processing services. 
											The portfolio now totals 23 major hospitality businesses comprising high street 
											chains, catering companies and restaurant groups. ETC use P2D for a broad range 
											of invoice processing services including: scanning, email processing, 
											intelligent OCR data capture &amp; validation, approvals workflow, purchase 
											order matching, Invoice Analytics, online search &amp; retrieval and web 
											services integration with Aqilla online accounts; as well as a Cash Management 
											System joint venture developed for the hospitality sector. For more about ETC 
											Hospitality and its services please visit <a href="http://www.etchospitality.com/" target="_blank">
												www.etchospitality.com</a>.
											<hr>--%>
											<h4 class="color_blue" id="news04">ROBIN COLLA SPEAKS TO YOUNG ENTREPRENEURS</h4>
											<strong>12<sup>th</sup> June 2014: </strong>Robin Colla, Chief Executive 
											Officer of P2D, was recently invited by Sherry Coutu CBE, Non-Exec Director at 
											the London Stock Exchange, to talk to young entrepreneurs on behalf of 
											Founders4Schools and in conjunction with the Young Enterprise scheme. Whilst 
											Founders4Schools work with schools and entrepreneurs around the country, this 
											was part of a young enterprise programme that has been sponsored by Virgin 
											Money and allows the very young entrepreneur to create their own business idea 
											and enjoy the excitement and buzz of making money. Robin comments "It was a 
											great pleasure to talk with, and answer questions for, these children who have 
											some great ideas for such a young group. I am sure the programme will be a huge 
											success."
											<hr>
											<h4 class="color_blue" id="news05">THE NEW MOON PUB GROUP IMPLEMENTS THE P2D 
												INVOICE PROCESSING SOLUTION</h4>
											<strong>20<sup>th</sup> May 2014:</strong> The New Moon Pub Group is an 
											expanding collection of high-end pubs developed and operated by industry 
											veterans David Mooney and Paul Newman. New Moon selected the P2D system for 
											invoice processing, including scanning, intelligent OCR data capture &amp; 
											validation, approvals workflow, Invoice Analytics, online search &amp; 
											retrieval and web services integration with Aqilla online accounts. Adopting 
											the P2D system is a strategic financial decision to automate processes and 
											support the aggressive expansion plans of the group. For further information 
											about the New Moon Pub Group, please visit their website: <a href="http://www.newmoonpub.co.uk/" target="_blank">
												www.newmoonpub.co.uk</a>.
											<hr>
											<h4 class="color_blue" id="news06">NEW LOOK ADOPTS P2D'S INVOICE ANALYTICS SERVICE 
												FOR 100% ACCURACY RATES</h4>
											<strong>28<sup>th</sup> April 2014:</strong> P2D has deployed its new Invoice 
											Analytics service into the processing it performs for high-street fashion 
											retailer New Look (<a href="www.newlook.com" target="_blank">www.newlook.com</a>). 
											P2D's invoice processing service already includes several layers of data 
											validation and data quality management which deliver true accuracy rates in 
											excess of 99.99%, something unparalleled in the industry. However, P2D's new 
											Invoice Analytics service compounds these accuracy rates even further by 
											incorporating highly sophisticated intelligence diagnostics on the output data 
											to highlight possible anomalies which are then triple-checked, and if necessary 
											corrected, by the Support team. Robin Colla, P2D's Chief Executive, comments 
											"We have been performing the outsourced processing of New Look's global 
											invoices for more than 8 years now and the relationship continues to go from 
											strength to strength. Our reputation for delivering the highest possible levels 
											of accuracy and service underpin this."
											<hr>
											<h4 class="color_blue" id="news07">STRONG GROWTH PREDICTED IN E-INVOICING</h4>
											<strong>25<sup>th</sup> April 2014:</strong> TechNavio forecasts that the 
											Global e-Invoicing market will grow at a compound annual growth rate of 23.3% 
											over the period 2013 to 2018, driven by a need to automate the invoicing 
											process and reduce operational costs. The report acknowledges an emerging 
											market trend towards Cloud Computing, like the solution offered by P2D, and an 
											increasing adoption by SMEs. It also notes that e-invoicing "offers a number of 
											benefits to organizations including streamlining of accounting processes, 
											reducing paper consumption, offering higher receivables, helping in timely 
											delivery of invoices and faster customer payments, reducing the number of lost 
											invoices, and increasing visibility and control over various processes. 
											Enterprises are increasingly using e-invoicing solutions to simplify and 
											automate the process of creating, sending, and receiving invoices. E-invoicing 
											is a cost-effective solution that eliminates paper-based manual invoicing and 
											replaces it with a more reliable and faster invoicing system." Please contact 
											us for more information on the P2D e-invoicing solutions and to discuss how we 
											can help your organisation, whether large or small, to achieve these goals.
											<hr>
											<h4 class="color_blue" id="news08">ST JAMES'S HOTEL GROUP IMPLEMENTS THE P2D 
												INVOICE PROCESSING SOLUTION</h4>
											<strong>12<sup>th</sup> April 2014:</strong> St James's Hotel Group comprises a 
											collection of 14 large hotels across the UK, formerly known as Forestdale 
											Hotels. Locations include Bristol, Bath, Southampton, Bournemouth, Winchester, 
											Cambridge, Oxford, Barnsley, Darlington, Cirencester, Arundel, Ringwood and 
											Lyndhurst. St James implemented the P2D invoice processing solution integrated 
											with SUN Accounts in April 2014. The service includes scanning, intelligent OCR 
											data capture &amp; validation, approvals workflow, online search &amp; 
											retrieval and Invoice Analytics. The purchase of Forestdale is the first in a 
											series of planned acquisitions for St James's Hotels, which was formed recently 
											by the privately-owned European investor Somerston Group (<a href="http://www.somerston.com/" target="_blank">www.somerston.com</a>).
											<hr>
											<h4 class="color_blue" id="news09">COMMUNICORP SELECTS P2D FOR INVOICE PROCESSING</h4>
											<strong>4<sup>th</sup> April 2014:</strong> Founded in 1989, Communicorp Group 
											Limited operates a portfolio of media and radio assets in Ireland, Europe and 
											Jordan. It is the leading Irish commercial radio broadcaster and it operates 
											the largest independent radio groups in both BBBRaria and Latvia. Communicorp 
											adopted the P2D system for invoice processing in April 2014 following the 
											high-profile UK acquisition of Smooth and Real Radio from Global Radio. The 
											scope of services include: intelligent OCR data capture, approvals workflow, 
											Invoice Analytics, online search &amp; retrieval and integration with Microsoft 
											Great Plains. For further information about Communicorp, please visit <a href="http://www.communicorp.ie/" target="_blank">
												www.communicorp.ie</a>.
											<hr>
											<h4 class="color_blue" id="news10">CO-OP ANNOUNCE PLANS TO ROLLOUT P2D eEXPENSES TO 
												A FURTHER 10,000 USERS</h4>
											<strong>24<sup>th</sup> February 2014:</strong> The Co-operative Group 
											implemented P2D's eExpenses solution in August 2010. Reflecting the project's 
											success and the system's robust capability to handle the huge volume and user 
											base associated with what is one of the largest expense claim system 
											implementations in the UK in many years, the Co-op now announce plans to roll 
											the system out into their 5,000 stores to a further 10,000 employees. With 
											revenues of nearly £14bn, The Co-operative Group are the UK's fifth largest 
											food retailer, the third largest retail pharmacy chain, the number one provider 
											of funeral services and the largest independent travel business (<a href="http://www.co-operative.coop/" target="_blank">www.co-operative.coop</a>). 
											Robin Colla, P2D's Chief Executive, remarks "The Co-operative Group's roll-out 
											plan is testament to the stability of the solution, its ability to deliver on a 
											huge scale, and the unparalleled quality of support P2D provides. We are 
											delighted to be working with the Co-op to enable further automation and cost 
											savings, and look forward to developing a continued relationship with them".
											<hr>
											<h4 class="color_blue" id="news11">FREEDOM HOTEL GROUP SELECTS P2D</h4>
											<strong>21<sup>st</sup> February 2014:</strong> The Freedom Hotel Group forms 
											part of the Crieff Hydro family of hotels. Its hotels are located across 
											Scotland and the North of England. Freedom selected P2D to continue to automate 
											the invoice processing of the hotels recently acquired from the Akkeron Hotel 
											Group. The implemented system interfaces to SUN Accounts, and comprises the 
											following scope: scanning, email processing, intelligent OCR data capture &amp; 
											validation, approvals workflow, Invoice Analytics, and online search &amp; 
											retrieval. For more information about The Freedom Hotel Group, please visit 
											their website <a href="http://www.crieffhydro.com/" target="_blank">www.crieffhydro.com</a>.
											<hr>
											<h4 class="color_blue" id="news12">P2D LAUNCH 24-HOUR TURNAROUND GUARANTEE</h4>
											<strong>1<sup>st</sup> February 2014:</strong> Following the introduction of 
											P2D's specialised invoice processing solution for the hospitality market in 
											2012, and its subsequent adoption by a variety of hotel, restaurant and cafe 
											groups, it has become increasingly important to provide a 24-hour turnaround 
											guarantee. Unlike all other industry sectors which will typically have a 
											regular daily invoice volume, our hospitality customers (in particular the 
											accounting practices with multiple hospitality sub-clients) are prone to truly 
											enormous volume spikes due to the nature of their processes, yet at the same 
											time require all invoices to be scanned, processed and in the accounting system 
											within 24 hours. As part of the delivery of our 100% accuracy guarantee, P2D's 
											Data Validation Team check every piece of data on every invoice (which in 
											itself is something quite unique in the industry). Therefore from a resourcing 
											point of view, managing these huge volume spikes is challenging. To respond to 
											the demands of the hospitality market and tackle this challenge, P2D have 
											created a flexible resourcing model so that the size of the team can expand and 
											contract by up to 3 times on a day-to-day basis to meet demand. James Enstone, 
											P2D's CIO, comments "Managing daily swings in volume of several thousand 
											invoices is quite a challenge so I'm delighted that we have managed to engineer 
											a solution for our clients. A 100% accuracy guarantee coupled with a 24-hour 
											turnaround guarantee really does set us apart in the invoice processing 
											market".
											<hr>
											<h4 class="color_blue" id="news13">SEGMETRIX TO USE P2D'S INVOICE PROCESSING 
												SOLUTION</h4>
											<strong>23<sup>rd</sup> December 2013:</strong> Segmetrix, the B2B segmentation 
											and analytics company, is to use P2D for its invoice processing. Segmetrix has 
											won a string of awards including B2B Marketing Awards Winner 2008, Information 
											Management Innovative Solution Award 2010, and BeyeNetwork Vision Award 2010, 
											enabling clients to integrate their Sales, Marketing and Finance teams around 
											operational goals and provide them with a proven, systematic way of repeating 
											success. The P2D solution will automatically process invoices received by email 
											and seamlessly post them into Twinfield cloud accounting system via web 
											services integration, as well as provide online search &amp; retrieval. 
											Segmetrix is the 4th client introduced to P2D by Plummer Parsons accounting 
											practice.
											<hr>
											<h4 class="color_blue">PLUMMER PARSONS ADDS WILTSHIRE FARM FOODS INTO ITS P2D 
												PORTFOLIO</h4>
											<strong>1<sup>st</sup> November 2013:</strong> After going live in August, 
											Plummer Parsons accounting practice has added Wiltshire Farm Foods into its P2D 
											portfolio for invoice processing. Wiltshire Farm Foods, recognised by its 
											Ronnie Corbett TV adverts, is the UK's leading frozen meals home delivery 
											service. They have been a long-standing client of P2D and requested that they 
											continue to receive the P2D service from their new accountants, Plummer 
											Parsons. Wiltshire Farm Foods receive a service from P2D which comprises the 
											automated processing of purchase invoices sent by both email and paper form, 
											which are automatically coded and posted into Twinfield online accounts, 
											removing the entire manual process for Plummer Parsons and allowing all parties 
											to search for, and view, invoices online.
											<hr>
											<h4 class="color_blue">NEW LOOK ADOPT P2D'S EMAIL INVOICE PROCESSING SERVICE</h4>
											<strong>9<sup>th</sup> September 2013:</strong> Following the launch of P2D's 
											email processing capability in January, we are pleased to announce that New 
											Look, the high street fashion retailer, has adopted the service. P2D has 
											already been processing New Look's global invoice volume for several years via 
											a combination of e-invoicing and OCR. New Look found that it was increasingly 
											receiving more invoices from its suppliers by email, and that using P2D's email 
											processing service was a natural add-on to remove even more paper from the 
											operation, and further reduce internal time and cost. The service allows New 
											Look to email its invoices into different email accounts which each relate to a 
											batch type, and thereby dictate the onward workflow in P2D: for example, 'Goods 
											For Resale' vs. 'Goods Not For Resale' invoices. The invoices received are 
											automatically stripped from the emails and subjected to P2D's award-winning 
											100% accuracy intelligent data capture.
											<hr>
											<h4 class="color_blue">PLUMMER PARSONS ACCOUNTING PRACTICE SELECTS P2D FOR INVOICE 
												PROCESSING</h4>
											<strong>6<sup>th</sup> August 2013:</strong> Plummer Parsons is an accountancy 
											firm with 5 offices in the south of England and celebrated its 50th anniversary 
											in October 2013. Having adopted Twinfield cloud accountancy software, they were 
											looking for a partner that could provide efficient, cost effective invoice 
											automation for their clients and chose P2D as that partner. The service 
											initially went live with 2 clients, Ampere Technical Services &amp; 
											Match2Lists, and incorporates the automated processing of emailed invoices, 
											intelligent data capture &amp; validation, automated coding, online search 
											&amp; retrieval and seamless web services integration with Twinfield. Plummer 
											Parsons will be rolling the system out across other clients in 2014-15. The 
											service delivers significant benefits to both Plummer Parsons and clients 
											alike, as the process is fully managed by P2D. Now, Plummer Parsons only needs 
											to send the invoices to P2D, and flag the transactions for payment in 
											Twinfield; everything else is done! The P2D system is also HMRC approved, and 
											hosts the documents and data for 7 years, thereby also removing paper storage 
											costs. To find out more about Plummer Parsons, please visit <a href="http://www.plummer-parsons.co.uk/" target="_blank">
												www.plummer-parsons.co.uk</a>.
											<hr>
											<h4 class="color_blue">ASOS GO LIVE GLOBALLY WITH P2D'S eEXPENSES SYSTEM</h4>
											<strong>14<sup>th</sup> June 2013:</strong> ASOS PLC is the UK's largest 
											independent online fashion and beauty retailer servicing 241 countries. It 
											generates annual revenues of c. £750m and has a market capitalization of £3bn. 
											ASOS selected P2D's online employee expense claim system which was successfully 
											implemented this month. The system has been rolled out globally across the 
											entire ASOS group structure, and incorporates full international tax 
											calculations. "ASOS is yet another big-ticket retailer to select P2D as a 
											processing partner for a core financial process. Once again this reflects P2D's 
											ability to provide highly secure, highly adaptable solutions to large complex 
											blue-chip corporates, embraced by a truly unbeatable support framework", Robin 
											Colla, P2D Chief Executive. For further innformation about ASOS, visit their 
											website: <a href="http://www.asos.com/" target="_blank">www.asos.com</a>.
											<hr>
											<h4 class="color_blue">P2D LAUNCH INVOICE ANALYTICS PACKAGE TO DELIVER 100% 
												ACCURACY GUARANTEE</h4>
											<strong>7<sup>th</sup> June 2013:</strong> P2D is pleased to announce the 
											launch of its new Invoice Analytics service. The current service already 
											includes several layers of data validation and data quality management which 
											deliver true accuracy rates in excess of 99.99%, something unparalleled in the 
											industry. However, P2D's new Invoice Analytics package compounds these accuracy 
											rates even further through the use of highly sophisticated intelligence 
											diagnostics on the output data to highlight possible anomalies which are then 
											triple-checked, and if necessary corrected, by our Support team. Robin Colla, 
											P2D's CEO, comments "Our ability to deliver the highest levels of accuracy has 
											always set us apart from the rest of the market. The new Invoice Analytics 
											software we have developed takes this even further and allows us to provide a 
											100% accuracy guarantee to our clients, something no other service provider can 
											offer".
											<hr>
											<h4 class="color_blue">P2D LAUNCH SINGLE SIGN ON SERVICES</h4>
											<strong>15<sup>th</sup> May 2013:</strong> In response to the growing trend for 
											companies to configure their network logon via a 'Single Sign On' approach, P2D 
											has developed the capability to deploy its solutions for any SSO method. If you 
											are considering using P2D's service, please contact us and we would be happy to 
											provide full details of our technical capabilities in this area. "We are 
											increasingly being asked by our clients to deploy our solutions via SSO, so 
											have launched a capability to deliver this to any required protocol that a 
											client may have. Much like our ability to integrate with any accounting, ERP or 
											back-end system, P2D always strives to offer the ultimate in flexibility to 
											meet the demands of our existing and future clients", adds James Enstone, P2D's 
											Chief Information Officer.
											<hr>
											<h4 class="color_blue">WARD WILLIAMS CHARTERED ACCOUNTANTS SIGNS UP FOR P2D</h4>
											<strong>30<sup>th</sup> April 2013:</strong> Ward Williams accounting practice 
											has selected P2D's automated invoice processing solution that interfaces with 
											the Twinfield cloud accounting system. Ward Williams has 5 offices in the South 
											of England and is currently undergoing a project to implement Twinfield for its 
											client base. Twinfield is Europe's largest online accounting system. The P2D 
											solution receives invoices from Ward Williams by email, and automatically 
											processes them into Twinfield via a seamless web services integration. This 
											allows Ward Williams to remove the labour-intensive activity of entering and 
											coding invoices manually, and at the same time provides full search and 
											retrieval in the cloud either from P2D or indeed Twinfield itself via a 
											punch-out back to P2D's image respository. Naturally, this also removes the 
											cost of storing paper invoices for 7 years to meet HMRC requirements. For more 
											information on Ward Williams, please visit <a href="http://www.wardwilliams.co.uk/" target="_blank">
												www.wardwilliams.co.uk</a>.
											<hr>
											<h4 class="color_blue">NEW LOOK COMPLETES 'GOODS NOT FOR RESALE' PURCHASE ORDER 
												MIGRATION</h4>
											<strong>26<sup>th</sup> April 2013:</strong> New Look, the high street fashion 
											retailer and a long-standing client of P2D, has gone live with the switchover 
											to purchase ordering for its 'GNFR' suppliers. P2D has supported the New Look 
											project over the last year with a variety of planning activities, system 
											changes and migration work. In turn this means that all New Look suppliers must 
											now quote a valid Purchase Order Number on their invoices. GNFR suppliers that 
											send invoices electronically over the P2D portal will be informed, at the 
											appropriate time, as to when this field will become mandatory to populate in 
											their submissions.
											<hr>
											<h4 class="color_blue">ASTRAL FIRE SIGNS UP FOR E-INVOICING</h4>
											<strong>25<sup>th</sup> April 2013:</strong> Astral Fire &amp; Security has 
											signed up to the P2D Document Network for e-invoicing. It is therefore now 
											capable of sending sales invoices electronically to meet the demands of its 
											clients currently using P2D, and also any future clients. Joining the network 
											allows Astral to promote itself to other clients as having e-invoicing 
											capability via P2D. Astral is a fire and security systems specialist with a 
											range of system services including: Design, Installation, Commissioning, 
											Service, Maintenance, Repair and Upgrade. It has clients within the Retail, 
											Public, Industrial and Leisure sectors such as Whitbread, New Look and Colliers 
											International. See <a href="http://www.astralfireandsecurity.com/" target="_blank">www.astralfireandsecurity.com</a>
											for further details.
											<hr>
											<h4 class="color_blue">TENNALS FACILITIES MANAGEMENT JOIN THE P2D DOCUMENT NETWORK</h4>
											<strong>18<sup>th</sup> April 2013:</strong> Tennals FM is a national provider 
											of facilities management and property maintenance solutions. Tennals work on 
											behalf of a various blue chip organisations undertaking works on a national and 
											regional basis for both single and multi-site delivery. By signing up to P2D 
											for e-invoicing, Tennals can service the demands of its clients for paperless 
											invoicing, as well as reap the benefits of having an online portal to access 
											all sales invoices and monitor invoice status. This in turn allows Tennals to 
											have full transparency of its receivables cashflow, and remove any payment 
											bottlenecks long before the payment due date. Visit Tennals website for more 
											information <a href="http://www.tennalsfm.com/" target="_blank">www.tennalsfm.com</a>.
											<hr>
											<h4 class="color_blue">FACILITIES SERVICES GROUP GO LIVE WITH P2D E-INVOICING</h4>
											<strong>13<sup>th</sup> March 2013:</strong> Facilities Services Group is a 
											market leading integrated facilities management service provider, with a 
											turnover of more than £20m. Facilities Services Group selected P2D to provide 
											an e-invoicing programme of PO Flips, e-invoicing, PO matching and supplier 
											portal to process the invoices it receives in servicing its clients. FSG's 
											clients include Starbucks, Debenhams, Nandos, Travelodge, Dreams, The 
											Co-operative, Serco, Knight Frank, Cancer Research and Majestic Wine among 
											others. The system will allow them to process nearly all invoices straight 
											through, by receiving them electronically with automated validation, matching 
											to PO, and coding; thereby removing the substantial internal cost of processing 
											paper invoices. FSG is now part of the Servest Group; for more information 
											please visit their website <a href="http://servest.co.uk/" target="_blank">www.servest.co.uk</a>.
											<hr>
											<h4 class="color_blue">PICKERINGS LIFTS JOINS THE P2D DOCUMENT NETWORK</h4>
											<strong>6<sup>th</sup> February 2013:</strong> Pickerings Lifts is the UK's 
											leading and largest independent lift manufacturer. With over 150 years of 
											experience, Pickerings designs, installs, maintains and repairs lifts, 
											escalators and loading bay equipment. By signing up to the P2D Document 
											Network, Pickerings is able to send its sales invoices to customers 
											electronically. In turn this provides them with the following key benefits: 
											customer satisfaction / retention; removal of paper and postage costs; 
											avoidance of lost invoices and payment delays; improved cashflow. The P2D 
											system also allows users to search for and retrieve all invoices at any time, 
											as well as monitor their stage of processing / approval. Visit <a href="http://www.pickeringslifts.co.uk/" target="_blank">
												www.pickeringslifts.co.uk</a> for more about Pickerings Lifts.
											<hr>
											<h4 class="color_blue">P2D LAUNCH EMAIL PROCESSING SERVICE</h4>
											<strong>23<sup>rd</sup> January 2013:</strong> P2D is pleased to announce the 
											launch of its new service to automatically process emailed invoices. 
											E-invoicing in its true sense is the transfer of raw data from supplier to 
											buyer. However for low volume, temporary, or ad hoc suppliers emailing invoices 
											tends to be the easiest and therefore preferred approach for many. To embrace 
											this and provide further process automation for its clients, P2D has developed 
											the capability to receive emailed invoices from which the attachments are 
											automatically stripped and subjected to P2D's award-winning 100% accuracy 
											intelligent data capture. The service allows clients to email their invoices 
											into different email accounts which each relate to a batch type, and thereby 
											dictate the onward workflow in P2D. Customers such as Twinfield, ETC 
											Hospitality and Wiltshire Farm Foods have already signed up to the service.
											<hr>
											<h4 class="color_blue">CTB ACCOUNTS SELECTS P2D FOR INVOICE PROCESSING</h4>
											<strong>18th December 2012:</strong> CTB Accounts is a specialized consultancy 
											providing management accountancy services to the restaurant industry. CTB has 
											selected P2D to provide its system and services in order to process the 
											invoices for its customer base with a current total volume of over 10,000 per 
											month. The scope comprises: scanning, intelligent OCR data capture &amp; 
											validation, approvals workflow, online search &amp; retrieval and integration 
											with Sage. The P2D service removes all data entry of invoices allowing CTB 
											staff to concentrate on value-added activities for their clients, and provides 
											full search and retrieval of invoices online to allow them to efficiently 
											handle client queries. The project goes live in January 2013 and will continue 
											to roll-out across all its customers during 2013 compounding the efficiency 
											gains. Please visit <a href="http://www.ctbaccounts.com/" target="_blank">www.ctbaccounts.com</a>
											for more information on CTB and the services it offers.
											<hr>
											<h4 class="color_blue">INTER-CITY GROUP JOINS THE P2D DOCUMENT NETWORK</h4>
											<strong>17<sup>th</sup> December 2012:</strong> Inter-City Group was formed in 
											1981 and has since developed into a major watch, gift and jewellery licensee 
											holder and distributor. Inter-City has ISO certified manufacturing facilities 
											in China and Hong Kong, and offices in Europe and the Far East. By signing up 
											to the P2D Document Network, Inter-City now has the capability to send sales 
											invoices electronically to clients such as New Look, that are immediately 
											integrated into their accounting systems and workflows to ensure there is no 
											risk of delayed payments. Not only does this maximise cashflow but also removes 
											the cost of paper, postage and labour. For more information on Inter-City visit <a href="http://www.icw-watches.co.uk/lo/icw_index.html" target="_blank">
												www.icw-watches.co.uk</a>.
											<hr>
											<h4 class="color_blue">TOTALIS SIGNS UP FOR SALES INVOICE PROCESSING</h4>
											<strong>15<sup>th</sup> October 2012:</strong> Operating throughout the UK and 
											Ireland, Totalis Solutions provides service solutions tailored to meet the 
											needs of a variety of local, national and multi-national clients such as 
											Morrisons, Lidl, Iceland, Poundland, Texaco, William Hill, Belfast City 
											Council, KwikFit and Danke Bank. Totalis has signed up to the P2D Document 
											Network in order to send its sales invoices electronically. Totalis selected an 
											integration method that best suited its internal systems and processes which 
											performs a single link into P2D which is in turn capable of passing on the 
											invoices to multiple customers in their own required format and protocol. Using 
											the network therefore removes a huge duplication of cost in setting up, and 
											supporting, a different interface / integration with each client for 
											e-invoicing. Please see <a href="http://www.totalissolutions.co.uk/" target="_blank">
												www.totalissolutions.co.uk</a>.
											<hr>
											<h4 class="color_blue">MSL PROPERTY CARE JOINS THE P2D DOCUMENT NETWORK</h4>
											<strong>4<sup>th</sup> October 2012:</strong> Through five specialist divisions 
											MSL offer a complete property care solution encompassing reactive and planned 
											maintenance, statutory and environmental compliance assurance programmes and 
											building fabric works. MSL have signed up to the P2D Document Network to 
											service the e-invoicing requests of its customers. This allows them to remit 
											invoices and credit notes through the P2D system via a method of their choice, 
											which are then immediately passed into the customer's PO matching or approvals 
											workflow. Hence there are no longer any lost invoices, delays due to postage, 
											duplicate invoices, and MSL can monitor the approval of all their sales 
											invoices online. For additional information on MSL Property Care please visit <a href="http://www.msl-ltd.co.uk/" target="_blank">
												www.msl-ltd.co.uk</a>.
											<hr>
											<h4 class="color_blue">P2D LAUNCH AQILLA WEB SERVICES</h4>
											<strong>19<sup>th</sup> September 2012:</strong> P2D is pleased to launch its 
											new web service integration with Aqilla, the online accounting system (<a href="http://www.aqilla.com/" target="_blank">www.aqilla.com</a>). 
											The integration provides a seamless link between the two systems for posting 
											invoice and credit note transactions that have been processed within P2D. The 
											web service automatically transfers the invoice data securely over the internet 
											at scheduled times during the day. Duplicate invoices are rejected and marked 
											as such in the P2D workflow together with the rejection reason provided by 
											Aqilla. For invoices that do not require manual approval such as those with 
											purchase order numbers, or Food &amp; Beverage invoices in the hospitality 
											market, they can be processed straight through meaning there is a huge labour 
											and cost reduction for the client who would hitherto have processed everything 
											manually as paper.
											<hr>
											<h4 class="color_blue">ETC AND P2D LAUNCH NEW CASH MANAGEMENT SYSTEM</h4>
											<strong>17<sup>th</sup> September 2012:</strong> ETC Hospitality, leading 
											provider of cloud accounting solutions to the hospitality market, has developed 
											a new cash management system called WTR (Weekly Trading Report) in partnership 
											with P2D. ETC provided a specification of how the system should work to satisfy 
											its clients' needs and P2D developed the system accordingly and integrated it 
											into their invoice processing system. The system has also been built with a 
											standard interface to Aqilla, the online accounting system that ETC use for all 
											of its clients. For more about ETC Hospitality and its service offering, please 
											visit their website <a href="http://www.etchospitality.com/" target="_blank">www.etchospitality.com</a>.
											<hr>
											<h4 class="color_blue">GSH GROUP SIGNS UP FOR SALES INVOICE PROCESSING</h4>
											<strong>27<sup>th</sup> June 2012:</strong> GSH Group has signed up to the P2D 
											Document Network for e-invoicing. GSH is a leading provider of 
											technology-driven facilities and energy management solutions, throughout the 
											UK, Europe, India and US, employing over 2,400 staff. It is therefore now 
											capable of sending sales invoices electronically to meet the global demands of 
											clients using P2D, as well as being able to promote itself to other clients as 
											having e-invoicing capability. In addition to meeting customer requirements the 
											system also allows GSH to monitor search for and retrieve all invoices online, 
											as well as receive updates on their status, thereby allowing them to monitor 
											cashflow. For more on GSH Group, visit <a href="http://www.gshgroup.com/" target="_blank">
												www.gshgroup.com</a>.
											<hr>
											<h4 class="color_blue">SPV GROUP JOINS THE P2D DOCUMENT NETWORK</h4>
											<strong>21<sup>st</sup> June 2012:</strong> SPV Group operates a nationwide 
											service for the refurbishment and maintenance of buildings in the industrial 
											and commercial sectors. SPV has signed up to the P2D Document Network for 
											e-invoicing, thereby allowing it to send sales invoices electronically to 
											customers via a single integration to the P2D hub. In turn this allows them to 
											minimise invoicing costs through the removal of paper and postage, and achieve 
											full visibility of their receivables cashflow with the online invoice status 
											information held in the portal. SPV has a range of high-profile clients such as 
											Lidl, Iceland, Bosch, Esporta, Odeon, Holiday Inn and Wetherspoons. For further 
											information see <a href="http://www.spv-uk.co.uk/" target="_blank">www.spv-uk.co.uk</a>.
											<hr>
										</div>
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
									<a href="AboutUs.aspx" title="About Us"><i class="glyphicon glyphicon-play"></i>About Us</a></li>
								<li>
									<a href="Solutions_Einvoicing.aspx" title="Solutions"><i class="glyphicon glyphicon-play"></i>Solutions</a></li>
								<li>
									<a href="Resources.aspx" title="Resources"><i class="glyphicon glyphicon-play"></i>Resources</a></li>
								<li>
									<a href="Partner.aspx" title="Partners"><i class="glyphicon glyphicon-play"></i>Partners</a></li>
								<li>
									<a href="Customers.aspx"  title="Customers"><i class="glyphicon glyphicon-play"></i>
										Customers</a></li>
								<li>
									<a href="javascript:void(0)" title="News" class="current"><i class="glyphicon glyphicon-play"></i>News</a></li>
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
    </form>
</body>
</html>
