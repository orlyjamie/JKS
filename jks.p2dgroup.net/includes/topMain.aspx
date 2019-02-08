<link href="style.css" rel="stylesheet" type="text/css">
<STYLE>
.menu {}
.submenu {position:absolute;}
</STYLE>

<SCRIPT>
var cm=null;
document.onclick = new Function("show(null)")
function getPos(el,sProp) {
	var iPos = 0
	while (el!=null) {
		iPos+=el["offset" + sProp]
		el = el.offsetParent
	}
	return iPos

}

function show(el,m) {
	if (m) {
		m.style.display='';
		m.style.pixelLeft = getPos(el,"Left")
		m.style.pixelTop = getPos(el,"Top") + el.offsetHeight
	}
	if ((m!=cm) && (cm)) cm.style.display='none'
	cm=m
}
</SCRIPT>

</head>

<DIV ID="ds1" CLASS="submenu" STYLE="display:none">
<table width="120" border="0" cellspacing="0" cellpadding="0">
<tr><td colspan="3"><img src="images/blank.gif"></td></tr>
<tr>
<td bgcolor="#78A6DA"><img src="images/blank.gif" width="7" height="18"></td>
<td bgcolor="#333333"><img src="images/blank.gif" width="7" height="1"></td>
<td bgcolor="#333333"><a class="form" href="profile.aspx">Company Profile</a></td></tr>
<tr><td colspan="3"><img src="images/blank.gif"></td></tr>
<tr>
<td bgcolor="#0D85C7"><img src="images/blank.gif" width="7" height="18"></td>
<td bgcolor="#333333"><img src="images/blank.gif" width="7" height="1"></td>
<td bgcolor="#333333"><a class="form" href="executives.aspx">Executive Team</a></td></tr>
</table>
</DIV>

<DIV ID="ds2" CLASS="submenu" STYLE="display:none">
<table width="190" border="0" cellspacing="0" cellpadding="0">
<tr><td colspan="3"><img src="images/blank.gif"></td></tr>
<tr>
<td bgcolor="#78A6DA"><img src="images/blank.gif" width="7" height="18"></td>
<td bgcolor="#333333"><img src="images/blank.gif" width="7" height="1"></td>
<td bgcolor="#333333"><a class="form" href="doc_network.aspx">The P2D Document Network</a></td></tr>

<tr><td colspan="3"><img src="images/blank.gif"></td></tr>
<tr>
<td bgcolor="#0D85C7"><img src="images/blank.gif" width="7" height="18"></td>
<td bgcolor="#333333"><img src="images/blank.gif" width="7" height="1"></td>
<td bgcolor="#333333"><a class="form" href="doc_imaging.aspx">Document Imaging </a></td></tr>

<tr><td colspan="3"><img src="images/blank.gif"></td></tr>
<tr>
<td bgcolor="#78A6DA"><img src="images/blank.gif" width="7" height="18"></td>
<td bgcolor="#333333"><img src="images/blank.gif" width="7" height="1"></td>
<td bgcolor="#333333"><a class="form" href="doc_management.aspx">Document Management </a></td></tr>
</table>
</DIV>

<DIV ID="ds3" CLASS="submenu" STYLE="display:none">
<table width="150" border="0" cellspacing="0" cellpadding="0">
<tr><td colspan="3"><img src="images/blank.gif"></td></tr>
<tr>
<td bgcolor="#78A6DA"><img src="images/blank.gif" width="7" height="18"></td>
<td bgcolor="#333333"><img src="images/blank.gif" width="7" height="1"></td>
<td bgcolor="#333333"><a class="form" href="development.aspx">Development Centre</a></td></tr>

<tr><td colspan="3"><img src="images/blank.gif"></td></tr>
<tr>
<td bgcolor="#0D85C7"><img src="images/blank.gif" width="7" height="18"></td>
<td bgcolor="#333333"><img src="images/blank.gif" width="7" height="1"></td>
<td bgcolor="#333333"><a class="form" href="expertise.aspx">Expertise </a></td></tr>

<tr><td colspan="3"><img src="images/blank.gif"></td></tr>
<tr>
<td bgcolor="#78A6DA"><img src="images/blank.gif" width="7" height="18"></td>
<td bgcolor="#333333"><img src="images/blank.gif" width="7" height="1"></td>
<td bgcolor="#333333"><a class="form" href="network.aspx">Network Hosting</a></td></tr>
</table>
</DIV>



<body bgcolor="#DBDBDB" leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0" marginwidth="0" marginheight="0">
	<table height="100%" width="100%" border="0" cellspacing="0" cellpadding="0">
	<tr><td valign="top" align="center">
		<table width="100%" border="0" align="center" bgcolor="#989899" cellspacing="0" cellpadding="0">
		<tr><td bgcolor="#FFFFFF">
			<!-- start main table -->
			<table width="100%" border="0" cellspacing="0" cellpadding="0">
			<tr><td>
				<!-- top table start -->
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
				<tr><td>
						<table width="100%" border="0" cellspacing="0" cellpadding="0">
						<tr>
						<td><a href="#"><img src="images/logo.gif" alt="Paper2Data document solutions" border="0"></a></td>
						<td align=right>
								<OBJECT classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"
		codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0"
		WIDTH="500" HEIGHT="53" id="images/top" ALIGN="" VIEWASTEXT>
		<PARAM NAME=movie VALUE="images/top.swf"> <PARAM NAME=quality VALUE=high> <PARAM NAME=bgcolor VALUE=#000000> <EMBED src="images/top.swf" quality=high bgcolor=#ffffff  WIDTH="500" HEIGHT="53" NAME="top" ALIGN=""
		TYPE="application/x-shockwave-flash" PLUGINSPAGE="https://www.macromedia.com/go/getflashplayer"></EMBED>
		</OBJECT>
						</td>
						</tr>
						</table>
				
				
				</td></tr>
				<tr><td><img src="images/blank.gif" width="1" height="1"></td></tr>
				<tr><td>
					<table border="0" cellspacing="0" cellpadding="0">
					<tr>
					<td><img src="images/nav1.gif"></td>
					<td bgcolor="#606264"><img src="images/blank.gif" width="65" height="12"></td>
					<td><img src="images/blank.gif"></td>
					<td><a href="default.aspx"><img src="images/home.gif" border="0"></a></td>
					<td><img src="images/navspace.gif"></td>
					<td><img src="images/aboutus.gif" border="0" onmouseover="show(this,ds1);" ></td>
					<td><img src="images/navspace.gif"></td>
					<td><img src="images/solutions.gif" border="0" onmouseover="show(this,ds2);" ></td>
					<td><img src="images/navspace.gif"></td>
					<td><a href="partners.aspx"><img src="images/partners.gif" border="0"></a></td>
					<td><img src="images/navspace.gif"></td>
					<td><img src="images/resources.gif" border="0" onmouseover="show(this,ds3);" ></td>
					<td><img src="images/navspace.gif"></td>
					<td><a href="customers.aspx"><img src="images/customers.gif" border="0"></a></td>
					<td><img src="images/navspace.gif"></td>
					<td><a href="contactus.aspx"><img src="images/contactus.gif" border="0"></a></td>
					<td><img src="images/navspace.gif"></td>
					<td width=100% bgcolor="#606264"><img src="images/blank.gif" width="33" height="12"></td>
					</tr>
					</table>

				</td></tr>
				<tr><td><img src="images/blank.gif" width="1" height="1"></td></tr>
				</table>
				<!-- top table end -->
			</td></tr>
			

			
			<!-- end top panel -->