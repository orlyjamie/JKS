<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="TestTiff.Print" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="js/jquery-1.3.1.min.js"></script>
    <link rel="stylesheet" href="css/style.css" type="text/css" media="screen, projection" />
    <style type="text/css">
        body
        {
            font-size: 12px;
            font-family: Verdana;
        }
        
        #divPrint
        {
            display: none;
        }
        
        @media print
        {
            #non-printable
            {
                display: none;
            }
            #divPrint
            {
                display: block;
            }
        }
        
        #divProgress
        {
            margin-top: 5px;
            background-color: #79B933;
            height: 20px;
            width: 250px;
            text-align: center;
            color: white;
        }
        
        input, select
        {
            border: solid 1px #CDCDCD;
            font-size: 12px;
            font-family: Verdana;
        }
        
        /* You can define your own or change width and height values as required */
        
        .A4Page
        {
            width: 8.27in;
            height: 11.69in;
            float: left;
            position: relative;
        }
        
        .LegalPage
        {
            width: 8.5in;
            height: 14in;
            float: left;
            position: relative;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="non-printable">
        <table>
            <tr>
                <td>
                    Pages (from : to)
                </td>
                <td>
                    <input type="text" id="txtFrom" style="width: 50px" value="1" />&nbsp;:&nbsp;<input
                        type="text" id="txtTo" style="width: 50px" />
                </td>
            </tr>
            <tr>
                <td>
                    Print Quality
                </td>
                <td>
                    <input type="text" id="txtZoom" style="width: 50px" value="100" />%
                </td>
            </tr>
            <tr>
                <td>
                    Paper
                </td>
                <td>
                    <select id="drpPaper" style="width: 50px">
                        <option value="">Auto</option>
                        <option value="A4Page" selected>A4</option>
                        <option value="LegalPage">Legal</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="button" onclick="DoPrint();this.disabled='disabled';this.value='Starting...'"
                        value="Print" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divProgress">
                        &nbsp;</div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divPrint">
    </div>
    </form>
    <script type="text/javascript" language="javascript">
        var totalp = 0;
        var steps = 1;

        function PrintPages(from, to, zoom) {
            if (from > to) {
                alert("wrong page range");
                return;
            }
            totalp = 0;

            var objdivPrint = document.getElementById("divPrint");
            var token = getQueryVariable("token");

            steps = Math.abs(parseInt($("#divProgress").width())) / ((to - from) + 1);
            document.getElementById("divProgress").style.visibility = "visible";
            document.getElementById("divProgress").style.width = "0px";

            for (var i = from; i <= to; i++) {
                var pgImg = document.createElement("IMG");
                pgImg.id = "img" + i;
                pgImg.src = '<%= Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath %>' + "TifViewer.axd?token=" + token + "&zoom=" + zoom + "&page=" + i;

                objdivPrint.appendChild(pgImg);
                pgImg.onload = function () { LoadCount((to - from) + 1) };
            }
        }

        function LoadCount(t) {
            totalp = totalp + 1;
            document.getElementById("divProgress").style.width = parseInt($("#divProgress").width()) + steps + "px";
            document.getElementById("divProgress").innerHTML = "Page " + totalp;

            var cssPaper = $('#drpPaper').val();

            if (totalp == t) {
                var objdivPrint = document.getElementById("divPrint");
                var x = objdivPrint.childNodes.length;

                for (var i = x - 1; i > -1; i--) {
                    if ('IMG' == objdivPrint.childNodes[i].tagName) {

                        if (cssPaper.length > 0) {
                            objdivPrint.childNodes[i].className = cssPaper;
                        }
                    }
                }

                $('#non-printable').remove(); // IE 9
                setTimeout("self.focus(); window.print(); self.parent.tb_remove();", 2000);
            }
        }

        function DoPrint() {
            ClearControls();

            var startPage = document.getElementById("txtFrom").value;
            var endPage = document.getElementById("txtTo").value;
            var zoomLevel = document.getElementById("txtZoom").value;

            PrintPages(startPage, endPage, zoomLevel);
        }

        function ClearControls() {
            var objdivPrint = document.getElementById("divPrint");
            var x = objdivPrint.childNodes.length;

            for (var i = x - 1; i > -1; i--) {
                if ('undefined' != objdivPrint.childNodes[i].id) {
                    var objToRemove = document.getElementById(objdivPrint.childNodes[i].id);
                    if (null != objToRemove) {
                        objdivPrint.removeChild(objToRemove);
                    }
                }
            }
        }

        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == variable) {
                    return pair[1];
                }
            }
        }

        var p = getQueryVariable("printpage");
        if (typeof (p) != "undefined") {
            document.getElementById("non-printable").style.display = "none";
            var zoom = getQueryVariable("printzoom");

            if (typeof (zoom) == "undefined") {
                zoom = "100";
            }

            PrintPages(p, p, zoom);
        }
        else {
            document.getElementById("txtTo").value = getQueryVariable("printtotal");
            document.getElementById("divProgress").style.visibility = "hidden";
        }             
    </script>
</body>
</html>
