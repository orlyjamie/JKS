


function printSelection(divcontent, reportcaption) {
    var content = document.getElementById(divcontent).innerHTML;
    var chart = document.getElementById(divchart).innerHTML;
     //document.getElementById('dvChartControl').style.display=none;
   
    var pwin = window.open('', 'print_content', 'width=100,height=100');

    var now = new Date();
    var currentDate = now.getMonth() + "/" + now.getDate() + "/" + now.getYear();

    pwin.document.open();
    pwin.document.write('<html><body onload="window.print()">');
    pwin.document.write('<table width="595" border="1" cellspacing="0" cellpadding="0" style="border: 1px solid #dfdfdf;">');
    pwin.document.write('<tr>');
    pwin.document.write('<td>');
    pwin.document.write('<table width="595" border="0" cellspacing="0" cellpadding="0">');
    pwin.document.write('<tr>');
    pwin.document.write('<td>');
    pwin.document.write('<div style="width: 595px; position: relative;">');
    pwin.document.write('<div style="position: absolute; right: 10px; bottom: 30px; text-align: right; font-family: Arial, Helvetica, sans-serif;font-size: 15px; font-weight: bold;">' + reportcaption);
    pwin.document.write('<br />');
    pwin.document.write('<span style="font-size: 14px; font-weight: normal; color: #c15b0e">' + getCalendarDate());
    pwin.document.write('</span>');
    pwin.document.write('</div>');
  
    pwin.document.write('</div>');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('<tr>');
    pwin.document.write('<td>');
    pwin.document.write('<table width="595" border="0" cellspacing="0" cellpadding="0">');
    pwin.document.write('<tr>');
    pwin.document.write('<td>');
    pwin.document.write('<table width="530" border="0" cellspacing="0" cellpadding="0" align="center" style="border: 1px solid #dfdfdf;">');
    
    pwin.document.write('</table>');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('<tr>');
    pwin.document.write('<td>&nbsp;');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('<tr>');
    pwin.document.write('<td>' + content);
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('</table>');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('<tr>');
    pwin.document.write('<td>&nbsp;');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('</table>');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('</table>');  
    pwin.document.write('</body></html>');
    pwin.document.close();
    setTimeout(function () { pwin.close(); }, 2000);
}
function printSelection(divcontent, divchart, reportcaption, innerpage) {

    var content = document.getElementById(divcontent).innerHTML;
    var chart = document.getElementById(divchart).innerHTML;
   // document.getElementById('dvChartControl').style.display = none;
    var pwin = window.open('', 'print_content', 'width=100,height=100');

    var now = new Date();
    var currentDate = now.getMonth() + "/" + now.getDate() + "/" + now.getYear();

    pwin.document.open();
    pwin.document.write('<html><body onload="window.print()">');
    pwin.document.write('<table width="595" border="1" cellspacing="0" cellpadding="0" style="border: 1px solid #dfdfdf;">');
    pwin.document.write('<tr>');
    pwin.document.write('<td>');
    pwin.document.write('<table width="595" border="0" cellspacing="0" cellpadding="0">');
    pwin.document.write('<tr>');
    pwin.document.write('<td>');
    pwin.document.write('<div style="width: 595px; position: relative;">');
    pwin.document.write('<div style="position: absolute; right: 10px; bottom: 30px; text-align: right; font-family: Arial, Helvetica, sans-serif;font-size: 15px; font-weight: bold;">' + reportcaption);
    pwin.document.write('<br />');
    pwin.document.write('<span style="font-size: 14px; font-weight: normal; color: #c15b0e">' + getCalendarDate());
    pwin.document.write('</span>');
    pwin.document.write('</div>');
    pwin.document.write('<div>');
    pwin.document.write('<img src="../../images/header.png" alt="" border="0" usemap="#Map" />');
    pwin.document.write('</div>');
    pwin.document.write('</div>');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('<tr>');
    pwin.document.write('<td>');
    pwin.document.write('<table width="595" border="0" cellspacing="0" cellpadding="0">');
    pwin.document.write('<tr>');
    pwin.document.write('<td>');
    pwin.document.write('<table width="530" border="0" cellspacing="0" cellpadding="0" align="center" style="border: 1px solid #dfdfdf;">');
    pwin.document.write('<tr>');
    pwin.document.write('<td align="center" style="padding: 10px;">' + chart);
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('</table>');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('<tr>');
    pwin.document.write('<td>&nbsp;');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('<tr>');
    pwin.document.write('<td>' + content);
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('</table>');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('<tr>');
    pwin.document.write('<td>&nbsp;');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('</table>');
    pwin.document.write('</td>');
    pwin.document.write('</tr>');
    pwin.document.write('</table>');
    pwin.document.write('<map name="Map" id="Map">');
    pwin.document.write('<area shape="rect" coords="15,8,317,86" href="#" title="Kingfisher" />');
    pwin.document.write('</map></body></html>');
    pwin.document.close();
    setTimeout(function () { pwin.close(); }, 2000);
}
function getCalendarDate() {
    var months = new Array(13);
    months[0] = "January";
    months[1] = "February";
    months[2] = "March";
    months[3] = "April";
    months[4] = "May";
    months[5] = "June";
    months[6] = "July";
    months[7] = "August";
    months[8] = "September";
    months[9] = "October";
    months[10] = "November";
    months[11] = "December";
    var now = new Date();
    var monthnumber = now.getMonth();
    var monthname = months[monthnumber];
    var monthday = now.getDate();
    var year = now.getYear();
    if (year < 2000) { year = year + 1900; }
    var dateString = monthname + ' - ' + monthday + ', ' + year;
    return dateString;
} 