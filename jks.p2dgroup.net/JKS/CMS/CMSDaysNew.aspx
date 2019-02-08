<%@ Page Language="c#" CodeBehind="CMSDaysNew.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.CMS.CMSDaysNew" %>

<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>WTR – Daily Summary</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <script type="text/javascript" src="jquery-1.7.2.js"></script>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="../../Utilities/main.js"></script>
    <script src="../../Utilities/style.js" type="text/javascript"></script>
    <style type="text/css">
        *
        {
            margin: 0px;
            font-size: 11px;
        }
        BODY
        {
            font: 11px Arial, Helvetica, sans-serif;
            color: #000;
        }
        INPUT
        {
            border-bottom: #000 1px solid;
            text-align: right;
            border-left: #000 1px solid;
            background-color: #f2f2f2;
            margin-top: 2px;
            width: 89px;
            padding-right: 2px;
            font: 11px/19px Arial, Helvetica, sans-serif;
            margin-bottom: 2px;
            height: 19px;
            border-top: #000 1px solid;
            border-right: #000 1px solid;
        }
        #wraper
        {
            margin: 0px auto;
            width: 872px;
        }
        .ButtonCss_CMS
        {
            background-image: url(../../images/button_bg.gif);
            border-bottom: medium none;
            text-align: center;
            border-left: medium none;
            background-color: #0081c5;
            width: 89px;
            background-repeat: no-repeat;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            background-position: center top;
            float: none;
            height: 23px;
            color: #ffffff;
            font-size: 11px;
            border-top: medium none;
            cursor: hand;
            border-right: medium none;
            text-decoration: none;
        }
        .fon
        {
            font: 11px Arial, Helvetica, sans-serif;
            color: #000;
        }
        .hed
        {
            width: 721px;
            margin-left: 62px;
        }
        .btn_col
        {
            border-bottom: #000 1px solid;
            text-align: center;
            border-left: #000 1px solid;
            background-color: #538ed5;
            width: 89px;
            font: 11px/19px Arial, Helvetica, sans-serif;
            margin-bottom: 4px;
            height: 19px;
            color: #fff;
            border-top: #000 1px solid;
            border-right: #000 1px solid;
        }
        .btn_col2
        {
            background-color: #fff !important;
            color: #000 !important;
        }
        .btn_col01
        {
            font: 11px/19px Arial, Helvetica, sans-serif;
            color: #ff0000;
        }
        .btn_col02
        {
            font: 11px/19px Arial, Helvetica, sans-serif;
            color: #00b050;
        }
        .btn_col03
        {
            color: #0032df;
        }
        .box_input
        {
            border-bottom: #000 1px solid;
            border-left: #000 1px solid;
            line-height: 19px;
            background-color: #fff;
            width: 89px;
            height: 19px;
            border-top: #000 1px solid;
            border-right: #000 1px solid;
        }
        .box_input1
        {
            border-bottom: #000 0px solid;
            text-align: left;
            border-left: #000 0px solid;
            line-height: 19px;
            background-color: #fff;
            width: 89px;
            height: 19px;
            border-top: #000 0px solid;
            font-weight: bold;
            border-right: #000 0px solid;
        }
        .hed2
        {
            width: 654px;
            float: left;
        }
        .hed3
        {
            width: 830px;
            float: left;
        }
        .hed4
        {
            width: 252px;
            float: right;
            margin-right: 206px;
        }
        .wid
        {
            width: 223px;
        }
        .Redmark
        {
            color: #ff0505;
        }
        .Greenmark
        {
            color: #07ac26;
        }
        .blackmark
        {
            color: #000;
        }
    </style>
    <script language="javascript">
        jQuery(document).ready(function () {
            var UserTypeID = '<%=Session["UserTypeID"] %>';
            if (parseInt(UserTypeID) == 11 || parseInt(UserTypeID) == 12) {
                jQuery("#wraper input[type='text']").attr("readonly", "readonly");
                jQuery("#btnSave").hide();
            }
            var Department = '<%=Session["DepartmentID"]%>';
            var arrDepartment = Department.split(",");
            if (arrDepartment.length > 1) {
                jQuery("#wraper input[type='text']").attr("readonly", "readonly");
                jQuery("#btnClosed2").hide();
                jQuery("#btnSave").hide();
            }
        });
        function ValidationForTwoDecimalPoint(str, event) {
            var val = str.value;
            var keyunicode = event.charCode || event.keyCode || event.which;
            if ((parseInt(keyunicode) > 47 && parseInt(keyunicode) < 58) || (parseInt(keyunicode) > 95 && parseInt(keyunicode) < 106) ||
	            keyunicode == "8" || keyunicode == "9" || keyunicode == "35" || keyunicode == "36"
			    || keyunicode == "37" || keyunicode == "38" || keyunicode == "39" || keyunicode == "40"
			    || keyunicode == "46" || keyunicode == "190" || keyunicode == "110") {
                if (val.indexOf(".") != -1) {
                    if (keyunicode == "190" || keyunicode == "110")
                    { return false; }
                    else {
                        // var val1=val.split(".");
                        //if(val1[1].length>1)
                        //{    					
                        //					        if(keyunicode=="8"||keyunicode=="9"||keyunicode=="35"||keyunicode=="36"
                        //					        ||keyunicode=="37"||keyunicode=="38"||keyunicode=="39"||keyunicode=="40"
                        //					        ||keyunicode=="46")
                        //					        {
                        return true;
                        //					        }
                        //					        else
                        //					        {
                        //						        return false;
                        //					        }
                        //}
                        // else
                        // return true;
                    }
                }
                else
                    return true;
            }
            else
                return false;

        }


        function ValidationForCover(str, event) {

            var keyunicode = event.charCode || event.keyCode || event.which;
            if ((parseInt(keyunicode) > 47 && parseInt(keyunicode) < 58) || (parseInt(keyunicode) > 95 && parseInt(keyunicode) < 106) ||
			            keyunicode == "8" || keyunicode == "9" || keyunicode == "35" || keyunicode == "36"
					    || keyunicode == "37" || keyunicode == "38" || keyunicode == "39" || keyunicode == "40"
					    || keyunicode == "46")
                return true;
            else
                return false;
        }
        function SumCovers() {

            document.getElementById("txtTotalCovers").value =
		parseFloat(document.getElementById("txtCovers1").value == "" ? 0 : document.getElementById("txtCovers1").value) +
		parseFloat(document.getElementById("txtCovers2").value == "" ? 0 : document.getElementById("txtCovers2").value) +
		parseFloat(document.getElementById("txtCovers3").value == "" ? 0 : document.getElementById("txtCovers3").value);
        }

        function SumFields1() {

            document.getElementById("txtNetTotal").value = roundNumber((
		parseFloat(document.getElementById("txtField1").value == "" ? 0 : document.getElementById("txtField1").value) +
		parseFloat(document.getElementById("txtField2").value == "" ? 0 : document.getElementById("txtField2").value) +
		parseFloat(document.getElementById("txtField3").value == "" ? 0 : document.getElementById("txtField3").value) +
		parseFloat(document.getElementById("txtField4").value == "" ? 0 : document.getElementById("txtField4").value) +
		parseFloat(document.getElementById("txtField5").value == "" ? 0 : document.getElementById("txtField5").value) +
		parseFloat(document.getElementById("txtField6").value == "" ? 0 : document.getElementById("txtField6").value) +
		parseFloat(document.getElementById("txtField7").value == "" ? 0 : document.getElementById("txtField7").value)), 2);

            document.getElementById("txtTotal1").value = roundNumber((
		parseFloat(document.getElementById("txtNetTotal").value == "" ? 0 : document.getElementById("txtNetTotal").value) +
		parseFloat(document.getElementById("txtVAT").value == "" ? 0 : document.getElementById("txtVAT").value)), 2);
            if (document.getElementById("txtVAT").value != "" && document.getElementById("txtNetTotal").value != "0.00") {
                document.getElementById("perVat").className = "Redmark";
                document.getElementById("perVat").value = roundNumber(
			(parseFloat(document.getElementById("txtVAT").value == "" ? 0 : document.getElementById("txtVAT").value) /
			parseFloat(document.getElementById("txtNetTotal").value == "" ? 0 : document.getElementById("txtNetTotal").value)) * 100, 2)
                document.getElementById("perVat").value = parseFloat(document.getElementById("perVat").value == "" ? 0 : document.getElementById("perVat").value) + '%';
            }
            else
                document.getElementById("perVat").value = "";


            SumFields2();
        }


        function SumFields2() {

            document.getElementById("txtTotal2").value = roundNumber((
		parseFloat(document.getElementById("txtField8").value == "" ? 0 : document.getElementById("txtField8").value) +
		parseFloat(document.getElementById("txtField9").value == "" ? 0 : document.getElementById("txtField9").value) +
		parseFloat(document.getElementById("txtField10").value == "" ? 0 : document.getElementById("txtField10").value) +
		parseFloat(document.getElementById("txtField11").value == "" ? 0 : document.getElementById("txtField11").value) +
		parseFloat(document.getElementById("txtField12").value == "" ? 0 : document.getElementById("txtField12").value) +
		parseFloat(document.getElementById("txtField13").value == "" ? 0 : document.getElementById("txtField13").value) +
		parseFloat(document.getElementById("txtField14").value == "" ? 0 : document.getElementById("txtField14").value) +
		parseFloat(document.getElementById("txtField15").value == "" ? 0 : document.getElementById("txtField15").value) +
		parseFloat(document.getElementById("txtPettyCashTotal").value == "" ? 0 : document.getElementById("txtPettyCashTotal").value)), 2);

            document.getElementById("txtStatusCal").value = "0.00"
            var value =
		parseFloat(document.getElementById("txtTotal2").value == "" ? 0 : document.getElementById("txtTotal2").value) -
		parseFloat(document.getElementById("txtTotal1").value == "" ? 0 : document.getElementById("txtTotal1").value);
            if (parseFloat(value) < 0) {
                document.getElementById("txtStatus1").className = "Redmark";
                document.getElementById("txtStatusCal").className = "Redmark";
                document.getElementById("txtStatusCal").value = '( ' + roundNumber((parseFloat(value) * (-1)), 2) + ' )';
                document.getElementById("txtStatus1").value = '<<<UNDER-BANKED';
                document.getElementById("txtStatus").className = "Redmark";
                document.getElementById("txtStatus").value = 'UNDER-BANKED';
            }
            else {
                document.getElementById("txtStatus1").className = "Redmark";
                document.getElementById("txtStatusCal").className = "blackmark";
                document.getElementById("txtStatusCal").value = roundNumber(value, 2);
                document.getElementById("txtStatus1").value = '<<<OVER-BANKED';
                document.getElementById("txtStatus").className = "Redmark";
                document.getElementById("txtStatus").value = 'OVER-BANKED';
                if (parseFloat(value) == 0) {
                    document.getElementById("txtStatus1").className = "Greenmark";
                    document.getElementById("txtStatusCal").className = "blackmark";
                    document.getElementById("txtStatus1").value = '<<<BALANCED';
                    document.getElementById("txtStatus").className = "Greenmark";
                    document.getElementById("txtStatus").value = 'BALANCED';
                }
            }

            SumNet();
        }
        function SumNet() {

            document.getElementById("txtTotalNet").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash1Net").value == "" ? 0 : document.getElementById("txtPettycash1Net").value) +
		parseFloat(document.getElementById("txtPettycash2Net").value == "" ? 0 : document.getElementById("txtPettycash2Net").value) +
		parseFloat(document.getElementById("txtPettycash3Net").value == "" ? 0 : document.getElementById("txtPettycash3Net").value) +
		parseFloat(document.getElementById("txtPettycash4Net").value == "" ? 0 : document.getElementById("txtPettycash4Net").value) +
		parseFloat(document.getElementById("txtPettycash5Net").value == "" ? 0 : document.getElementById("txtPettycash5Net").value) +
		parseFloat(document.getElementById("txtPettycash6Net").value == "" ? 0 : document.getElementById("txtPettycash6Net").value) +
		parseFloat(document.getElementById("txtPettycash7Net").value == "" ? 0 : document.getElementById("txtPettycash7Net").value) +
		parseFloat(document.getElementById("txtPettycash8Net").value == "" ? 0 : document.getElementById("txtPettycash8Net").value) +
		parseFloat(document.getElementById("txtPettycash9Net").value == "" ? 0 : document.getElementById("txtPettycash9Net").value) +
		parseFloat(document.getElementById("txtPettycash10Net").value == "" ? 0 : document.getElementById("txtPettycash10Net").value) +
		parseFloat(document.getElementById("txtPettycash11Net").value == "" ? 0 : document.getElementById("txtPettycash11Net").value) +
		parseFloat(document.getElementById("txtPettycash12Net").value == "" ? 0 : document.getElementById("txtPettycash12Net").value) +
		parseFloat(document.getElementById("txtPettycash13Net").value == "" ? 0 : document.getElementById("txtPettycash13Net").value) +
		parseFloat(document.getElementById("txtPettycash14Net").value == "" ? 0 : document.getElementById("txtPettycash14Net").value) +
		parseFloat(document.getElementById("txtPettycash15Net").value == "" ? 0 : document.getElementById("txtPettycash15Net").value)), 2);

            CalculateGross();
            SumGross();

            var values =
		parseFloat(document.getElementById("txtPettyCashTotal").value == "" ? 0 : document.getElementById("txtPettyCashTotal").value) -
		parseFloat(document.getElementById("txtTotalNet").value == "" ? 0 : document.getElementById("txtTotalNet").value);
            if (parseFloat(values) < 0) {
                document.getElementById("txtVariance").value = '( ' + roundNumber((parseFloat(values) * (-1)), 2) + ' )';
                document.getElementById("txtVariance").className = "Redmark";
                document.getElementById("txtMssg").value = '<<<UNDERSTATED RECEIPTS';
                document.getElementById("txtMssg").className = "Redmark";

            }
            else if (parseFloat(values) > 0) {
                document.getElementById("txtVariance").value = roundNumber(parseFloat(values), 2);
                document.getElementById("txtVariance").className = "Redmark";
                document.getElementById("txtMssg").value = '<<<OVERSTATED RECEIPTS';
                document.getElementById("txtMssg").className = "Redmark";

            }
            else {
                document.getElementById("txtVariance").value = "0.00";
                document.getElementById("txtVariance").className = "blackmark";
                document.getElementById("txtMssg").value = '';
                document.getElementById("txtMssg").className = "blackmark";
            }


        }
        function SumVat() {

            document.getElementById("txtTotalVAT").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash1VAT").value == "" ? 0 : document.getElementById("txtPettycash1VAT").value) +
		parseFloat(document.getElementById("txtPettycash2VAT").value == "" ? 0 : document.getElementById("txtPettycash2VAT").value) +
		parseFloat(document.getElementById("txtPettycash3VAT").value == "" ? 0 : document.getElementById("txtPettycash3VAT").value) +
		parseFloat(document.getElementById("txtPettycash4VAT").value == "" ? 0 : document.getElementById("txtPettycash4VAT").value) +
		parseFloat(document.getElementById("txtPettycash5VAT").value == "" ? 0 : document.getElementById("txtPettycash5VAT").value) +
		parseFloat(document.getElementById("txtPettycash6VAT").value == "" ? 0 : document.getElementById("txtPettycash6VAT").value) +
		parseFloat(document.getElementById("txtPettycash7VAT").value == "" ? 0 : document.getElementById("txtPettycash7VAT").value) +
		parseFloat(document.getElementById("txtPettycash8VAT").value == "" ? 0 : document.getElementById("txtPettycash8VAT").value) +
		parseFloat(document.getElementById("txtPettycash9VAT").value == "" ? 0 : document.getElementById("txtPettycash9VAT").value) +
		parseFloat(document.getElementById("txtPettycash10VAT").value == "" ? 0 : document.getElementById("txtPettycash10VAT").value) +
		parseFloat(document.getElementById("txtPettycash11VAT").value == "" ? 0 : document.getElementById("txtPettycash11VAT").value) +
		parseFloat(document.getElementById("txtPettycash12VAT").value == "" ? 0 : document.getElementById("txtPettycash12VAT").value) +
		parseFloat(document.getElementById("txtPettycash13VAT").value == "" ? 0 : document.getElementById("txtPettycash13VAT").value) +
		parseFloat(document.getElementById("txtPettycash14VAT").value == "" ? 0 : document.getElementById("txtPettycash14VAT").value) +
		parseFloat(document.getElementById("txtPettycash15VAT").value == "" ? 0 : document.getElementById("txtPettycash15VAT").value)), 2);

            CalculateGross();
            SumGross();
        }
        function SumGross() {

            document.getElementById("txtTotalGross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash1Gross").value == "" ? 0 : document.getElementById("txtPettycash1Gross").value) +
		parseFloat(document.getElementById("txtPettycash2Gross").value == "" ? 0 : document.getElementById("txtPettycash2Gross").value) +
		parseFloat(document.getElementById("txtPettycash3Gross").value == "" ? 0 : document.getElementById("txtPettycash3Gross").value) +
		parseFloat(document.getElementById("txtPettycash4Gross").value == "" ? 0 : document.getElementById("txtPettycash4Gross").value) +
		parseFloat(document.getElementById("txtPettycash5Gross").value == "" ? 0 : document.getElementById("txtPettycash5Gross").value) +
		parseFloat(document.getElementById("txtPettycash6Gross").value == "" ? 0 : document.getElementById("txtPettycash6Gross").value) +
		parseFloat(document.getElementById("txtPettycash7Gross").value == "" ? 0 : document.getElementById("txtPettycash7Gross").value) +
		parseFloat(document.getElementById("txtPettycash8Gross").value == "" ? 0 : document.getElementById("txtPettycash8Gross").value) +
		parseFloat(document.getElementById("txtPettycash9Gross").value == "" ? 0 : document.getElementById("txtPettycash9Gross").value) +
		parseFloat(document.getElementById("txtPettycash10Gross").value == "" ? 0 : document.getElementById("txtPettycash10Gross").value) +
		parseFloat(document.getElementById("txtPettycash11Gross").value == "" ? 0 : document.getElementById("txtPettycash11Gross").value) +
		parseFloat(document.getElementById("txtPettycash12Gross").value == "" ? 0 : document.getElementById("txtPettycash12Gross").value) +
		parseFloat(document.getElementById("txtPettycash13Gross").value == "" ? 0 : document.getElementById("txtPettycash13Gross").value) +
		parseFloat(document.getElementById("txtPettycash14Gross").value == "" ? 0 : document.getElementById("txtPettycash14Gross").value) +
		parseFloat(document.getElementById("txtPettycash15Gross").value == "" ? 0 : document.getElementById("txtPettycash15Gross").value)), 2);

        }
        function CalculateGross() {
            document.getElementById("txtPettycash1Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash1VAT").value == "" ? 0 : document.getElementById("txtPettycash1VAT").value) +
		parseFloat(document.getElementById("txtPettycash1Net").value == "" ? 0 : document.getElementById("txtPettycash1Net").value)), 2);

            document.getElementById("txtPettycash2Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash2VAT").value == "" ? 0 : document.getElementById("txtPettycash2VAT").value) +
		parseFloat(document.getElementById("txtPettycash2Net").value == "" ? 0 : document.getElementById("txtPettycash2Net").value)), 2);

            document.getElementById("txtPettycash3Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash3VAT").value == "" ? 0 : document.getElementById("txtPettycash3VAT").value) +
		parseFloat(document.getElementById("txtPettycash3Net").value == "" ? 0 : document.getElementById("txtPettycash3Net").value)), 2);

            document.getElementById("txtPettycash4Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash4VAT").value == "" ? 0 : document.getElementById("txtPettycash4VAT").value) +
		parseFloat(document.getElementById("txtPettycash4Net").value == "" ? 0 : document.getElementById("txtPettycash4Net").value)), 2);

            document.getElementById("txtPettycash5Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash5VAT").value == "" ? 0 : document.getElementById("txtPettycash5VAT").value) +
		parseFloat(document.getElementById("txtPettycash5Net").value == "" ? 0 : document.getElementById("txtPettycash5Net").value)), 2);

            document.getElementById("txtPettycash6Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash6VAT").value == "" ? 0 : document.getElementById("txtPettycash6VAT").value) +
		parseFloat(document.getElementById("txtPettycash6Net").value == "" ? 0 : document.getElementById("txtPettycash6Net").value)), 2);

            document.getElementById("txtPettycash7Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash7VAT").value == "" ? 0 : document.getElementById("txtPettycash7VAT").value) +
		parseFloat(document.getElementById("txtPettycash7Net").value == "" ? 0 : document.getElementById("txtPettycash7Net").value)), 2);

            document.getElementById("txtPettycash8Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash8VAT").value == "" ? 0 : document.getElementById("txtPettycash8VAT").value) +
		parseFloat(document.getElementById("txtPettycash8Net").value == "" ? 0 : document.getElementById("txtPettycash8Net").value)), 2);

            document.getElementById("txtPettycash9Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash9VAT").value == "" ? 0 : document.getElementById("txtPettycash9VAT").value) +
		parseFloat(document.getElementById("txtPettycash9Net").value == "" ? 0 : document.getElementById("txtPettycash9Net").value)), 2);

            document.getElementById("txtPettycash10Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash10VAT").value == "" ? 0 : document.getElementById("txtPettycash10VAT").value) +
		parseFloat(document.getElementById("txtPettycash10Net").value == "" ? 0 : document.getElementById("txtPettycash10Net").value)), 2);

            document.getElementById("txtPettycash11Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash11VAT").value == "" ? 0 : document.getElementById("txtPettycash11VAT").value) +
		parseFloat(document.getElementById("txtPettycash11Net").value == "" ? 0 : document.getElementById("txtPettycash11Net").value)), 2);

            document.getElementById("txtPettycash12Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash12VAT").value == "" ? 0 : document.getElementById("txtPettycash12VAT").value) +
		parseFloat(document.getElementById("txtPettycash12Net").value == "" ? 0 : document.getElementById("txtPettycash12Net").value)), 2);

            document.getElementById("txtPettycash13Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash13VAT").value == "" ? 0 : document.getElementById("txtPettycash13VAT").value) +
		parseFloat(document.getElementById("txtPettycash13Net").value == "" ? 0 : document.getElementById("txtPettycash13Net").value)), 2);

            document.getElementById("txtPettycash14Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash14VAT").value == "" ? 0 : document.getElementById("txtPettycash14VAT").value) +
		parseFloat(document.getElementById("txtPettycash14Net").value == "" ? 0 : document.getElementById("txtPettycash14Net").value)), 2);

            document.getElementById("txtPettycash15Gross").value = roundNumber((
		parseFloat(document.getElementById("txtPettycash15VAT").value == "" ? 0 : document.getElementById("txtPettycash15VAT").value) +
		parseFloat(document.getElementById("txtPettycash15Net").value == "" ? 0 : document.getElementById("txtPettycash15Net").value)), 2);


        }
        function roundNumber(num, dec) {
            var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);
            result = parseFloat(result).toFixed(2);
            return result;
        }

        function FinalCal() {
            SumCovers();
            SumFields1();
            SumFields2();
            SumNet();
            SumVat();
            SumGross();
            validationLabel();
        }
        function FinalCal2() {

            alert('Sales VAT % is not between 15 and 20%');
            FinalCal();


        }

        function fnCheck() {
            var bool = true;
            var UserTypeID = '<%=Session["UserTypeID"] %>';
            if (parseInt(UserTypeID) == 11 || parseInt(UserTypeID) == 12) {
                bool = false;
            }
            var Department = '<%=Session["DepartmentID"]%>';
            var arrDepartment = Department.split(",");
            if (arrDepartment.length > 1) {
                bool = false;
            }
            if (bool == true) {
                var r = confirm("If you have not pressed Save, changes will be lost. Are you sure you wish to continue?")
                if (r == true) {
                    window.location.href = 'WeeklySummary.aspx';
                }
            }
            else {
                window.location.href = 'WeeklySummary.aspx';
            }
        }
        function validationLabel() {

            if (document.getElementById("lblPettycash1").innerHTML == "") {
                document.getElementById("txtPettycash1Net").readOnly = true;
                document.getElementById("txtPettycash1VAT").readOnly = true;
                document.getElementById("txtPettycash1Gross").readOnly = true;
                document.getElementById("txtPettycash1Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash2").innerHTML == "") {
                document.getElementById("txtPettycash2Net").readOnly = true;
                document.getElementById("txtPettycash2VAT").readOnly = true;
                document.getElementById("txtPettycash2Gross").readOnly = true;
                document.getElementById("txtPettycash2Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash3").innerHTML == "") {
                document.getElementById("txtPettycash3Net").readOnly = true;
                document.getElementById("txtPettycash3VAT").readOnly = true;
                document.getElementById("txtPettycash3Gross").readOnly = true;
                document.getElementById("txtPettycash3Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash4").innerHTML == "") {
                document.getElementById("txtPettycash4Net").readOnly = true;
                document.getElementById("txtPettycash4VAT").readOnly = true;
                document.getElementById("txtPettycash4Gross").readOnly = true;
                document.getElementById("txtPettycash4Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash5").innerHTML == "") {
                document.getElementById("txtPettycash5Net").readOnly = true;
                document.getElementById("txtPettycash5VAT").readOnly = true;
                document.getElementById("txtPettycash5Gross").readOnly = true;
                document.getElementById("txtPettycash5Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash6").innerHTML == "") {
                document.getElementById("txtPettycash6Net").readOnly = true;
                document.getElementById("txtPettycash6VAT").readOnly = true;
                document.getElementById("txtPettycash6Gross").readOnly = true;
                document.getElementById("txtPettycash6Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash7").innerHTML == "") {
                document.getElementById("txtPettycash7Net").readOnly = true;
                document.getElementById("txtPettycash7VAT").readOnly = true;
                document.getElementById("txtPettycash7Gross").readOnly = true;
                document.getElementById("txtPettycash7Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash8").innerHTML == "") {
                document.getElementById("txtPettycash8Net").readOnly = true;
                document.getElementById("txtPettycash8VAT").readOnly = true;
                document.getElementById("txtPettycash8Gross").readOnly = true;
                document.getElementById("txtPettycash8Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash9").innerHTML == "") {
                document.getElementById("txtPettycash9Net").readOnly = true;
                document.getElementById("txtPettycash9VAT").readOnly = true;
                document.getElementById("txtPettycash9Gross").readOnly = true;
                document.getElementById("txtPettycash9Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash10").innerHTML == "") {
                document.getElementById("txtPettycash10Net").readOnly = true;
                document.getElementById("txtPettycash10VAT").readOnly = true;
                document.getElementById("txtPettycash10Gross").readOnly = true;
                document.getElementById("txtPettycash10Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash11").innerHTML == "") {
                document.getElementById("txtPettycash11Net").readOnly = true;
                document.getElementById("txtPettycash11VAT").readOnly = true;
                document.getElementById("txtPettycash11Gross").readOnly = true;
                document.getElementById("txtPettycash11Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash12").innerHTML == "") {
                document.getElementById("txtPettycash12Net").readOnly = true;
                document.getElementById("txtPettycash12VAT").readOnly = true;
                document.getElementById("txtPettycash12Gross").readOnly = true;
                document.getElementById("txtPettycash12Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash13").innerHTML == "") {
                document.getElementById("txtPettycash13Net").readOnly = true;
                document.getElementById("txtPettycash13VAT").readOnly = true;
                document.getElementById("txtPettycash13Gross").readOnly = true;
                document.getElementById("txtPettycash13Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash14").innerHTML == "") {
                document.getElementById("txtPettycash14Net").readOnly = true;
                document.getElementById("txtPettycash14VAT").readOnly = true;
                document.getElementById("txtPettycash14Gross").readOnly = true;
                document.getElementById("txtPettycash14Description").readOnly = true;
            }
            if (document.getElementById("lblPettycash15").innerHTML == "") {
                document.getElementById("txtPettycash15Net").readOnly = true;
                document.getElementById("txtPettycash15VAT").readOnly = true;
                document.getElementById("txtPettycash15Gross").readOnly = true;
                document.getElementById("txtPettycash15Description").readOnly = true;
            }
            if (document.getElementById("lblFiled1").innerHTML == "") {
                document.getElementById("txtField1").readOnly = true;
            }
            if (document.getElementById("lblField2").innerHTML == "") {
                document.getElementById("txtField2").readOnly = true;
            }
            if (document.getElementById("lblField3").innerHTML == "") {
                document.getElementById("txtField3").readOnly = true;
            }
            if (document.getElementById("lblField4").innerHTML == "") {
                document.getElementById("txtField4").readOnly = true;
            }
            if (document.getElementById("lblField5").innerHTML == "") {
                document.getElementById("txtField5").readOnly = true;
            }
            if (document.getElementById("lblField6").innerHTML == "") {
                document.getElementById("txtField6").readOnly = true;
            }
            if (document.getElementById("lblField7").innerHTML == "") {
                document.getElementById("txtField7").readOnly = true;
            }
            if (document.getElementById("lblField8").innerHTML == "") {
                document.getElementById("txtField8").readOnly = true;
            }
            if (document.getElementById("lblField9").innerHTML == "") {
                document.getElementById("txtField9").readOnly = true;
            }
            if (document.getElementById("lblField10").innerHTML == "") {
                document.getElementById("txtField10").readOnly = true;
            }
            if (document.getElementById("lblField11").innerHTML == "") {
                document.getElementById("txtField11").readOnly = true;
            }
            if (document.getElementById("lblField12").innerHTML == "") {
                document.getElementById("txtField12").readOnly = true;
            }
            if (document.getElementById("lblField13").innerHTML == "") {
                document.getElementById("txtField13").readOnly = true;
            }
            if (document.getElementById("lblField14").innerHTML == "") {
                document.getElementById("txtField14").readOnly = true;
            }
            if (document.getElementById("lblField15").innerHTML == "") {
                document.getElementById("txtField15").readOnly = true;
            }
        }
        function getQuerystring(key, default_) {
            if (default_ == null) default_ = "";
            key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
            var qs = regex.exec(window.location.href);
            if (qs == null)
                return default_;
            else
                return qs[1];
        }
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:Panel ID="Panel2" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
    </asp:Panel>
    <table id="Table1" style="width: 888px; height: 1200px">
        <tr>
            <td valign="top" width="150" bgcolor="gainsboro">
                <table id="Table2">
                    <tr>
                        <td>
                            <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table class="fon" id="wraper" cellpadding="7">
                    <tr>
                        <td>
                            <table class="hed" cellspacing="0" cellpadding="0" style="width: 700px">
                                <tr>
                                    <td align="right">
                                        <strong>Company</strong>
                                    </td>
                                    <td width="8">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input id="txtCompany" style="text-align: center; width: 125px; float: none; font-weight: bold"
                                            readonly name="Company" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Week No</strong>
                                    </td>
                                    <td width="8">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input id="txtWeekNo" style="text-align: center; float: none; font-weight: bold"
                                            readonly name="WeekNo" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Day</strong>
                                    </td>
                                    <td width="8">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input class="btn_col01" id="txtMonday" style="text-align: center; float: none; font-weight: bold"
                                            readonly name="Monday" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Status </strong>
                                    </td>
                                    <td width="8">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input class="btn_col01" id="txtStatus" style="text-align: center; float: none; font-weight: bold"
                                            readonly name="Status" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <strong>Department </strong>
                                    </td>
                                    <td width="8">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input id="txtDepartment" style="text-align: center; width: 125px; float: none; font-weight: bold"
                                            readonly name="Department" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Period No</strong>
                                    </td>
                                    <td width="8">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input id="txtPeriodNo" style="text-align: center; float: none; font-weight: bold"
                                            readonly name="PeriodNo" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Date</strong>
                                    </td>
                                    <td width="8">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input id="txtDate" style="text-align: center; float: none; font-weight: bold" readonly
                                            name="Date" runat="server">
                                    </td>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td width="8">
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="180">
                                        <asp:Label ID="lblCovers1" runat="server" Font-Italic="True">Covers1</asp:Label>
                                    </td>
                                    <td colspan="8">
                                        <input class="box_input" id="txtCovers1" onkeydown="javascript:return ValidationForCover(this,event);"
                                            onkeyup="javascript:SumCovers();" name="Covers1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="180">
                                        <asp:Label ID="lblCovers2" runat="server" Font-Italic="True">Covers2</asp:Label>
                                    </td>
                                    <td colspan="8">
                                        <input class="box_input" id="txtCovers2" onkeydown="javascript:return ValidationForCover(this,event);"
                                            onkeyup="javascript:SumCovers();" name="Covers2" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="180">
                                        <asp:Label ID="lblCovers3" runat="server" Font-Italic="True">Covers3</asp:Label>
                                    </td>
                                    <td colspan="8">
                                        <input class="box_input" id="txtCovers3" onkeydown="javascript:return ValidationForCover(this,event);"
                                            onkeyup="javascript:SumCovers();" name="Covers3" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="180">
                                        <strong>Total Covers</strong>
                                    </td>
                                    <td colspan="8">
                                        <input id="txtTotalCovers" style="font-weight: bold" readonly value="0" name="TotalCovers"
                                            runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hed3">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="151">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <asp:Label ID="lblFiled1" runat="server" Font-Italic="True">filed1</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField1" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields1();" name="Field1" runat="server">
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        <asp:Label ID="lblField8" runat="server" Font-Italic="True">Field8</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField8" name="Field8" runat="server" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <asp:Label ID="lblField2" runat="server" Font-Italic="True">Field2</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField2" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields1();" name="Field2" runat="server">
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        <asp:Label ID="lblField9" runat="server" Font-Italic="True">Field9</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField9" name="Field9" runat="server" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <asp:Label ID="lblField3" runat="server" Font-Italic="True">Field3</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField3" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields1();" name="Field3" runat="server">
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        <asp:Label ID="lblField10" runat="server" Font-Italic="True">Field10</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField10" name="Field10" runat="server" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <asp:Label ID="lblField4" runat="server" Font-Italic="True">Field4</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField4" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields1();" name="Field4" runat="server">
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        <asp:Label ID="lblField11" runat="server" Font-Italic="True">Field11</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField11" name="Field11" runat="server" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <asp:Label ID="lblField5" runat="server" Font-Italic="True">Field5</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField5" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields1();" name="Field5" runat="server">
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        <asp:Label ID="lblField12" runat="server" Font-Italic="True">Field12</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField12" name="Field12" runat="server" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <asp:Label ID="lblField6" runat="server" Font-Italic="True">Field6</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField6" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields1();" name="Field6" runat="server">
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        <asp:Label ID="lblField13" runat="server" Font-Italic="True">Field13</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField13" name="Field13" runat="server" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <asp:Label ID="lblField7" runat="server" Font-Italic="True">Field7</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField7" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields1();" name="Field7" runat="server">
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        <asp:Label ID="lblField14" runat="server" Font-Italic="True">Field14</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField14" name="Field14" runat="server" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <strong>Net Total</strong>
                                    </td>
                                    <td>
                                        <input id="txtNetTotal" style="font-weight: bold" readonly value="0" name="NetTotal"
                                            runat="server">
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        <asp:Label ID="lblField15" runat="server" Font-Italic="True">Field15</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtField15" name="Field15" runat="server" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        VAT
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtVAT" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumFields1();" name="VAT" runat="server">
                                    </td>
                                    <td width="50">
                                        <input style="border-bottom: #000 0px solid; text-align: left; border-left: #000 0px solid;
                                            background-color: #fff; border-top: #000 0px solid; font-weight: bold; border-right: #000 0px solid"
                                            id="perVat" readonly name="PerVat" runat="server">
                                    </td>
                                    <td align="left" width="300">
                                        Petty Cash Total
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtPettyCashTotal" name="PettyCashTotal" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumFields2();">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        <strong>Total</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotal1" style="font-weight: bold" readonly value="0" name="Total" runat="server">
                                    </td>
                                    <td width="50">
                                    </td>
                                    <td align="left" width="300">
                                        <strong>Total</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotal2" style="font-weight: bold" readonly value="0" name="Total" runat="server">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="300">
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td width="50">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="300">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input id="txtStatusCal" style="font-weight: bold" readonly value="0" name="txtStatusCal"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtStatus1" style="border-bottom: #000 0px solid; text-align: left; border-left: #000 0px solid;
                                            background-color: #fff; width: 200px; border-top: #000 0px solid; font-weight: bold;
                                            border-right: #000 0px solid" readonly>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hed2">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" width="151">
                                        <strong>Petty Cash</strong>
                                    </td>
                                    <td align="center">
                                        <strong>Net</strong>
                                    </td>
                                    <td align="center">
                                        <strong>VAT</strong>
                                    </td>
                                    <td align="center">
                                        <strong>Gross</strong>
                                    </td>
                                    <td align="center">
                                        <strong>Description</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash1Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash1" runat="server" Font-Italic="True">Pettycash1</asp:Label>
                                        </span>
                                        <div id="TipLayer" style="visibility: hidden; position: absolute; z-index: 1000;
                                            top: -100;">
                                        </div>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash1Net" name="txtPettycash1Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash1VAT" name="txtPettycash1VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash1Gross" name="txtPettycash1Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash1Description" name="txtPettycash1Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash2Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash2" runat="server" Font-Italic="True">Pettycash2</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash2Net" name="txtPettycash2Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash2VAT" name="txtPettycash2VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash2Gross" name="txtPettycash2Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash2Description" name="txtPettycash2Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash3Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash3" runat="server" Font-Italic="True">Pettycash3</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash3Net" name="txtPettycash3Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash3VAT" name="txtPettycash3VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash3Gross" name="txtPettycash3Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash3Description" name="txtPettycash3Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash4Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash4" runat="server" Font-Italic="True">Pettycash4</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash4Net" name="txtPettycash4Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash4VAT" name="txtPettycash4VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash4Gross" name="txtPettycash4Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash4Description" name="txtPettycash4Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash5Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash5" runat="server" Font-Italic="True">Pettycash5</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash5Net" name="txtPettycash5Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash5VAT" name="txtPettycash5VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash5Gross" name="txtPettycash5Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash5Description" name="txtPettycash5Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash6Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash6" runat="server" Font-Italic="True">Pettycash6</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash6Net" name="txtPettycash6Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash6VAT" name="txtPettycash6VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash6Gross" name="txtPettycash6Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash6Description" name="txtPettycash6Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash7Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash7" runat="server" Font-Italic="True">Pettycash7</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash7Net" name="txtPettycash7Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash7VAT" name="txtPettycash7VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash7Gross" name="txtPettycash7Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash7Description" name="txtPettycash7Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash8Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash8" runat="server" Font-Italic="True">Pettycash8</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash8Net" name="txtPettycash8Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash8VAT" name="txtPettycash8VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash8Gross" name="txtPettycash8Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash8Description" name="txtPettycash8Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash9Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash9" runat="server" Font-Italic="True">Pettycash9</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash9Net" name="txtPettycash9Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash9VAT" name="txtPettycash9VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash9Gross" name="txtPettycash9Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash9Description" name="txtPettycash9Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash10Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash10" runat="server" Font-Italic="True">Pettycash10</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash10Net" name="txtPettycash10Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash10VAT" name="txtPettycash10VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash10Gross" name="txtPettycash10Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash10Description" name="txtPettycash10Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash11Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash11" runat="server" Font-Italic="True">Pettycash11</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash11Net" name="txtPettycash11Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash11VAT" name="txtPettycash11VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash11Gross" name="txtPettycash11Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash11Description" name="txtPettycash11Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash12Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash12" runat="server" Font-Italic="True">Pettycash12</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash12Net" name="txtPettycash12Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash12VAT" name="txtPettycash12VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash12Gross" name="txtPettycash12Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash12Description" name="txtPettycash12Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash13Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash13" runat="server" Font-Italic="True">Pettycash13</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash13Net" name="txtPettycash13Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash13VAT" name="txtPettycash13VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash13Gross" name="txtPettycash13Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash13Description" name="txtPettycash13Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash14Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash14" runat="server" Font-Italic="True">Pettycash14</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash14Net" name="txtPettycash14Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash14VAT" name="txtPettycash14VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input id="txtPettycash14Gross" name="txtPettycash14Gross" readonly onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash14Description" name="txtPettycash14Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <span onmouseover="stm(['','<table class=NormalBody border=0 style=color:white><tr><td>'+'<%= strPettycash15Info %>'+'</td></tr></table>'],Style[9])"
                                            onmouseout="htm()">
                                            <asp:Label ID="lblPettycash15" runat="server" Font-Italic="True">Pettycash15</asp:Label>
                                        </span>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash15Net" name="txtPettycash15Net" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumNet();">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtPettycash15VAT" name="txtPettycash15VAT" runat="server"
                                            onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);" onkeyup="javascript:SumVat();">
                                    </td>
                                    <td align="left">
                                        <input readonly id="txtPettycash15Gross" name="txtPettycash15Gross" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);"
                                            onkeyup="javascript:SumGross();" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="wid box_input" id="txtPettycash15Description" name="txtPettycash15Description"
                                            style="text-align: left" runat="server" maxlength="200">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <strong>Total</strong>
                                    </td>
                                    <td align="left">
                                        <input id="txtTotalNet" style="font-weight: bold" readonly value="0" name="txtPettycash1Net"
                                            runat="server">
                                    </td>
                                    <td align="left">
                                        <input id="txtTotalVAT" style="font-weight: bold" readonly value="0" name="txtPettycash1VAT"
                                            runat="server">
                                    </td>
                                    <td align="left">
                                        <input id="txtTotalGross" style="font-weight: bold" readonly value="0" name="txtPettycash1Gross"
                                            runat="server">
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <strong>Variance</strong>
                                    </td>
                                    <td align="left">
                                        <input id="txtVariance" style="font-weight: bold" readonly value="0" name="Variance"
                                            runat="server">
                                    </td>
                                    <td align="left" colspan="3">
                                        <input id="txtMssg" readonly style="border-bottom: #000 0px solid; text-align: left;
                                            border-left: #000 0px solid; background-color: #fff; width: 176px; height: 19px;
                                            border-top: #000 0px solid; font-weight: bold; border-right: #000 0px solid">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="hed4">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <input class="ButtonCss_CMS" id="btnBack" type="button" value="Back" name="btnBack"
                                                runat="server" onclick="javascript:return fnCheck();">
                                        </td>
                                        <td width="151">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <input class="ButtonCss_CMS" id="btnSave" type="button" value="Save" name="Save"
                                                runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="151">
                                            &nbsp;
                                        </td>
                                        <td width="151">
                                            Saved By
                                        </td>
                                        <td>
                                            <input id="txtSavedBy" style="text-align: center; float: none; font-weight: bold"
                                                readonly name="SavedBy" runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="151">
                                            &nbsp;
                                        </td>
                                        <td width="151">
                                            Save Date
                                        </td>
                                        <td>
                                            <input id="txtSaveDate" style="text-align: center; float: none; font-weight: bold"
                                                readonly name="SaveDate" runat="server">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
