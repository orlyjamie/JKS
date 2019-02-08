<%@ Page Language="c#" CodeBehind="WeeklySummary.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.CMS.WeeklySummary" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="bannerUM" Src="../../Utilities/bannerETC.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>WTR - Weekly Summary</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <script type="text/javascript" src="jquery-1.7.2.js"></script>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <style type="text/css">
        *
        {
            font-size: 11px;
            margin: 0px;
        }
        BODY
        {
            font: 11px Arial, Helvetica, sans-serif;
            color: #000;
        }
        INPUT
        {
            border-right: #000 1px solid;
            padding-right: 2px;
            border-top: #000 1px solid;
            margin-top: 2px;
            margin-bottom: 2px;
            font: 11px/19px Arial, Helvetica, sans-serif;
            border-left: #000 1px solid;
            width: 89px;
            border-bottom: #000 1px solid;
            height: 19px;
            background-color: #f2f2f2;
            text-align: right;
        }
        #wraper
        {
            margin: 0px auto;
            width: 872px;
        }
        .hed
        {
            margin-left: 162px;
            width: 710px;
        }
        .ButtonCss_CMS
        {
            border-right: medium none;
            background-position: center top;
            border-top: medium none;
            font-size: 11px;
            float: none;
            background-image: url(../../images/button_bg.gif);
            border-left: medium none;
            width: 89px;
            cursor: hand;
            color: #ffffff;
            border-bottom: medium none;
            background-repeat: no-repeat;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            height: 23px;
            background-color: #0081c5;
            text-align: center;
            text-decoration: none;
        }
        .btn_col
        {
            border-right: medium none;
            border-top: medium none;
            margin-bottom: 4px;
            font: 11px/19px Arial, Helvetica, sans-serif;
            border-left: medium none;
            width: 89px;
            color: #fff;
            border-bottom: medium none;
            height: 19px;
            background-color: #538ed5;
            text-align: center;
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
            border-right: #000 1px solid;
            border-top: #000 1px solid;
            border-left: #000 1px solid;
            width: 89px;
            line-height: 18px;
            border-bottom: #000 1px solid;
            height: 19px;
            background-color: #fff;
        }
        .hed2
        {
            float: left;
            width: 422px;
        }
        .hed3
        {
            float: left;
            width: 602px;
        }
        .hed4
        {
            margin-top: 0px;
            float: right;
            width: 200px;
        }
        .fon
        {
            color: #000;
            font-siz: 11px;
        }
        .Redmark
        {
            color: #ff0505;
        }
        .Greenmark
        {
            color: #07ac26;
        }
    </style>
    <script language="javascript">
        /************************ Created by: Rinku Santra *****************************/
        jQuery(document).ready(function () {
            var UserTypeID = '<%=Session["UserTypeID"] %>';
            var weekstartdate = '<%=Session["weekstartdate"] %>';
            if (weekstartdate != "" && (parseInt(UserTypeID) == 11 || parseInt(UserTypeID) == 12)) {
                jQuery("#wraper input[type='text']").attr("readonly", "readonly");
                jQuery("#btnClosed2").hide();
                jQuery("#btnSave").hide();
            }
        });
        function fn_Closed() {
            var bool = false;
            if (document.getElementById("txtMondayStatus").value != "BALANCED" || document.getElementById("txtTuesdayStatus").value != "BALANCED" ||
			document.getElementById("txtWednessdayStatus").value != "BALANCED" || document.getElementById("txtThursdayStatus").value != "BALANCED" ||
			document.getElementById("txtFridayStatus").value != "BALANCED" || document.getElementById("txtSaturdayStatus").value != "BALANCED" ||
			document.getElementById("txtSundayStatus").value != "BALANCED") {
                bool = confirm('One or more days are not balanced, are you sure you wish to continue?');
                if (bool)
                    return true;
            }

            if (document.getElementById("txtMondayTotalGross").value == "0.00" || document.getElementById("txtTuesdayTotalGross").value == "0.00" ||
			document.getElementById("txtWednesdayTotalGross").value == "0.00" || document.getElementById("txtThursdayTotalGross").value == "0.00" ||
			document.getElementById("txtFridayTotalGross").value == "0.00" || document.getElementById("txtSaturdayTotalGross").value == "0.00" ||
			document.getElementById("txtSundayTotalGross").value == "0.00") {
                bool = confirm('Total Sales are zero for one or more days, are you sure you wish to continue?');
                if (bool)
                    return true;
            }
            var Variance = '<%=ViewState["chkVariance"]%>';
            if (Variance == "1") {
                bool = confirm('Petty cash receipts do not balance on one or more days, are you sure you wish to continue?');
                if (bool)
                    return true;
            }
            if (document.getElementById("txtMondayTotalCover").value == "0" || document.getElementById("txtTuesdayTotalCover").value == "0" ||
			document.getElementById("txtWednesdayTotalCover").value == "0" || document.getElementById("txtThursdayTotalCover").value == "0" ||
			document.getElementById("txtFridayTotalCover").value == "0" || document.getElementById("txtSaturdayTotalCover").value == "0" ||
			document.getElementById("txtSundayTotalCover").value == "0") {
                bool = confirm('Covers have not been entered on one or more days, are you sure you wish to continue?');
                if (bool)
                    return true;
            }
            bool = confirm('Are you sure you want to close the week?');
            if (bool)
                return true;
            else
                return false;
        }
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
                        //var val1=val.split(".");
                        //if(val1[1].length>10)
                        //{    					
                        //if(keyunicode=="8"||keyunicode=="9"||keyunicode=="35"||keyunicode=="36"
                        //||keyunicode=="37"||keyunicode=="38"||keyunicode=="39"||keyunicode=="40"
                        //||keyunicode=="46")
                        //{
                        return true;
                        //}
                        // else
                        //{
                        //    return false;
                        // }
                        // }
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

        function sumStock1() {

            var stock1 = roundNumber((
			parseFloat(document.getElementById("txtStock1Closing").value == "" ? 0 : document.getElementById("txtStock1Closing").value) -
			parseFloat(document.getElementById("txtStock1Opening").value == "" ? 0 : document.getElementById("txtStock1Opening").value)), 2);
            if (parseInt(stock1) >= 0) {
                document.getElementById("txtStock1Movement").value = stock1;
                document.getElementById("txtStock1Movement").className = "blackmark";
            }
            else {
                document.getElementById("txtStock1Movement").className = "Redmark";
                document.getElementById("txtStock1Movement").value = '( ' + roundNumber((parseFloat(stock1) * (-1)), 2) + ' )';
            }

            var stock2 = roundNumber((
			parseFloat(document.getElementById("txtStock2Closing").value == "" ? 0 : document.getElementById("txtStock2Closing").value) -
			parseFloat(document.getElementById("txtStock2Opening").value == "" ? 0 : document.getElementById("txtStock2Opening").value)), 2);
            if (parseInt(stock2) >= 0) {
                document.getElementById("txtStock2Movement").value = stock2;
                document.getElementById("txtStock2Movement").className = "blackmark";
            }
            else {
                document.getElementById("txtStock2Movement").className = "Redmark";
                document.getElementById("txtStock2Movement").value = '( ' + roundNumber((parseFloat(stock2) * (-1)), 2) + ' )';
            }

            var stock3 = roundNumber((
			parseFloat(document.getElementById("txtStock3Closing").value == "" ? 0 : document.getElementById("txtStock3Closing").value) -
			parseFloat(document.getElementById("txtStock3Opening").value == "" ? 0 : document.getElementById("txtStock3Opening").value)), 2);
            if (parseInt(stock3) >= 0) {
                document.getElementById("txtStock3Movement").value = stock3;
                document.getElementById("txtStock3Movement").className = "blackmark";
            }
            else {
                document.getElementById("txtStock3Movement").className = "Redmark";
                document.getElementById("txtStock3Movement").value = '( ' + roundNumber((parseFloat(stock3) * (-1)), 2) + ' )';
            }
            var stock4 = roundNumber((
			parseFloat(document.getElementById("txtStock4Closing").value == "" ? 0 : document.getElementById("txtStock4Closing").value) -
			parseFloat(document.getElementById("txtStock4Opening").value == "" ? 0 : document.getElementById("txtStock4Opening").value)), 2);
            if (parseInt(stock4) >= 0) {
                document.getElementById("txtStock4Movement").value = stock4;
                document.getElementById("txtStock4Movement").className = "blackmark";
            }
            else {
                document.getElementById("txtStock4Movement").className = "Redmark";
                document.getElementById("txtStock4Movement").value = '( ' + roundNumber((parseFloat(stock4) * (-1)), 2) + ' )';
            }

            document.getElementById("txtTotalOpening").value = roundNumber((
			parseFloat(document.getElementById("txtStock1Opening").value == "" ? 0 : document.getElementById("txtStock1Opening").value) +
			parseFloat(document.getElementById("txtStock2Opening").value == "" ? 0 : document.getElementById("txtStock2Opening").value) +
			parseFloat(document.getElementById("txtStock3Opening").value == "" ? 0 : document.getElementById("txtStock3Opening").value) +
			parseFloat(document.getElementById("txtStock4Opening").value == "" ? 0 : document.getElementById("txtStock4Opening").value)), 2);

            document.getElementById("txtTotalClosing").value = roundNumber((
			parseFloat(document.getElementById("txtStock1Closing").value == "" ? 0 : document.getElementById("txtStock1Closing").value) +
			parseFloat(document.getElementById("txtStock2Closing").value == "" ? 0 : document.getElementById("txtStock2Closing").value) +
			parseFloat(document.getElementById("txtStock3Closing").value == "" ? 0 : document.getElementById("txtStock3Closing").value) +
			parseFloat(document.getElementById("txtStock4Closing").value == "" ? 0 : document.getElementById("txtStock4Closing").value)), 2);

            var stock5 = roundNumber((
			parseFloat(document.getElementById("txtTotalClosing").value == "" ? 0 : document.getElementById("txtTotalClosing").value) -
			parseFloat(document.getElementById("txtTotalOpening").value == "" ? 0 : document.getElementById("txtTotalOpening").value)), 2);
            if (parseInt(stock5) >= 0) {
                document.getElementById("txtStockTotalMovement").className = "blackmark";
                document.getElementById("txtStockTotalMovement").value = stock5;
            }
            else {
                document.getElementById("txtStockTotalMovement").className = "Redmark";
                document.getElementById("txtStockTotalMovement").value = '( ' + roundNumber((parseFloat(stock5) * (-1)), 2) + ' )';
            }
        }
        function Labour1() {
            document.getElementById("txtTotalLabour1").value = roundNumber((
			parseFloat(document.getElementById("txtLabour1").value == "" ? 0 : document.getElementById("txtLabour1").value) +
			parseFloat(document.getElementById("txtNationalInsurance1").value == "" ? 0 : document.getElementById("txtNationalInsurance1").value)), 2);
            TotalCalculationofLabour();
        }
        function Labour2() {
            document.getElementById("txtTotalLabour2").value = roundNumber((
			parseFloat(document.getElementById("txtLabour2").value == "" ? 0 : document.getElementById("txtLabour2").value) +
			parseFloat(document.getElementById("txtNationalInsurance2").value == "" ? 0 : document.getElementById("txtNationalInsurance2").value)), 2);
            TotalCalculationofLabour();
        }
        function Labour3() {
            document.getElementById("txtTotalLabour3").value = roundNumber((
			parseFloat(document.getElementById("txtLabour3").value == "" ? 0 : document.getElementById("txtLabour3").value) +
			parseFloat(document.getElementById("txtNationalInsurance3").value == "" ? 0 : document.getElementById("txtNationalInsurance3").value)), 2);
            TotalCalculationofLabour();
        }
        function Labour4() {

            document.getElementById("txtTotalLabour4").value = roundNumber((
			parseFloat(document.getElementById("txtLabour4").value == "" ? 0 : document.getElementById("txtLabour4").value) +
			parseFloat(document.getElementById("txtNationalInsurance4").value == "" ? 0 : document.getElementById("txtNationalInsurance4").value)), 2);
            TotalCalculationofLabour();
        }
        function Labour5() {
            document.getElementById("txtTotalLabour5").value = roundNumber((
			parseFloat(document.getElementById("txtLabour5").value == "" ? 0 : document.getElementById("txtLabour5").value) +
			parseFloat(document.getElementById("txtNationalInsurance5").value == "" ? 0 : document.getElementById("txtNationalInsurance5").value)), 2);
            TotalCalculationofLabour();
        }
        function TotalCalculationofLabour() {
            document.getElementById("txtTotalLabour").value = roundNumber((
			parseFloat(document.getElementById("txtLabour1").value == "" ? 0 : document.getElementById("txtLabour1").value) +
			parseFloat(document.getElementById("txtLabour2").value == "" ? 0 : document.getElementById("txtLabour2").value) +
			parseFloat(document.getElementById("txtLabour3").value == "" ? 0 : document.getElementById("txtLabour3").value) +
			parseFloat(document.getElementById("txtLabour4").value == "" ? 0 : document.getElementById("txtLabour4").value) +
			parseFloat(document.getElementById("txtLabour5").value == "" ? 0 : document.getElementById("txtLabour5").value)), 2);
            document.getElementById("txtTotalNationalInsurance").value = roundNumber((
			parseFloat(document.getElementById("txtNationalInsurance1").value == "" ? 0 : document.getElementById("txtNationalInsurance1").value) +
			parseFloat(document.getElementById("txtNationalInsurance2").value == "" ? 0 : document.getElementById("txtNationalInsurance2").value) +
			parseFloat(document.getElementById("txtNationalInsurance3").value == "" ? 0 : document.getElementById("txtNationalInsurance3").value) +
			parseFloat(document.getElementById("txtNationalInsurance4").value == "" ? 0 : document.getElementById("txtNationalInsurance4").value) +
			parseFloat(document.getElementById("txtNationalInsurance5").value == "" ? 0 : document.getElementById("txtNationalInsurance5").value)), 2);
            document.getElementById("txtTotalLabourCost").value = roundNumber((
			parseFloat(document.getElementById("txtTotalLabour").value == "" ? 0 : document.getElementById("txtTotalLabour").value) +
			parseFloat(document.getElementById("txtTotalNationalInsurance").value == "" ? 0 : document.getElementById("txtTotalNationalInsurance").value)), 2);
            if (document.getElementById("txtTotalLabourCost").value != "0.00" && document.getElementById("txtTotalTotalNet").value != "0.00") {
                document.getElementById("txtTotalLabourPercentage").value = roundNumber((
			    parseFloat((parseFloat(document.getElementById("txtTotalLabourCost").value == "" ? 0 : document.getElementById("txtTotalLabourCost").value) /
			    parseFloat(document.getElementById("txtTotalTotalNet").value == "" ? 0 : document.getElementById("txtTotalTotalNet").value)) * 100)), 2);
            }
        }
        function roundNumber(num, dec) {
            var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);
            result = parseFloat(result).toFixed(2);
            return result;
        }

        function FloatOpening() {

            document.getElementById("txtFloatClosing").value = roundNumber((
			parseFloat(document.getElementById("txtFloatOpening").value == "" ? 0 : document.getElementById("txtFloatOpening").value) +
			parseFloat(document.getElementById("txtFloatIncrease").value == "" ? 0 : document.getElementById("txtFloatIncrease").value)), 2);
            document.getElementById("txtFloatVariance").value = roundNumber((
			parseFloat(document.getElementById("txtFloatClosing").value == "" ? 0 : document.getElementById("txtFloatClosing").value) -
			parseFloat(document.getElementById("txtFixedFloat").value == "" ? 0 : document.getElementById("txtFixedFloat").value)), 2);

        }
        function FloatFixed() {
            document.getElementById("txtFloatVariance").value = roundNumber((
			parseFloat(document.getElementById("txtFloatClosing").value == "" ? 0 : document.getElementById("txtFloatClosing").value) -
			parseFloat(document.getElementById("txtFixedFloat").value == "" ? 0 : document.getElementById("txtFixedFloat").value)), 2);

        }
        function SafeOpening() {

            document.getElementById("txtSafeClosing").value = roundNumber((
			parseFloat(document.getElementById("txtSafeOpening").value == "" ? 0 : document.getElementById("txtSafeOpening").value) +
			parseFloat(document.getElementById("txtSafeIncrease").value == "" ? 0 : document.getElementById("txtSafeIncrease").value)), 2);
            document.getElementById("txtSafeVariance").value = roundNumber((
			parseFloat(document.getElementById("txtSafeClosing").value == "" ? 0 : document.getElementById("txtSafeClosing").value) -
			parseFloat(document.getElementById("txtFixedSafe").value == "" ? 0 : document.getElementById("txtFixedSafe").value)), 2);
        }
        function SafeFixed() {

            document.getElementById("txtSafeVariance").value = roundNumber((
			parseFloat(document.getElementById("txtSafeClosing").value == "" ? 0 : document.getElementById("txtSafeClosing").value) -
			parseFloat(document.getElementById("txtFixedSafe").value == "" ? 0 : document.getElementById("txtFixedSafe").value)), 2);

        }
        function FinalTotalCalculation() {
            daysCalculation();
            sumStock1();
            Labour1();
            Labour2();
            Labour3();
            Labour4();
            Labour5();
            FloatOpening();
            SafeOpening();
        }
        function daysCalculation() {
            document.getElementById("txtTotalCover1").value =
			parseFloat(document.getElementById("txtMondayCover1").value == "-" ? 0 : document.getElementById("txtMondayCover1").value) +
			parseFloat(document.getElementById("txtTuesdayCover1").value == "-" ? 0 : document.getElementById("txtTuesdayCover1").value) +
			parseFloat(document.getElementById("txtWednesdayCover1").value == "-" ? 0 : document.getElementById("txtWednesdayCover1").value) +
			parseFloat(document.getElementById("txtThursdayCover1").value == "-" ? 0 : document.getElementById("txtThursdayCover1").value) +
			parseFloat(document.getElementById("txtFridayCover1").value == "-" ? 0 : document.getElementById("txtFridayCover1").value) +
			parseFloat(document.getElementById("txtSaturdayCover1").value == "-" ? 0 : document.getElementById("txtSaturdayCover1").value) +
			parseFloat(document.getElementById("txtSundayCover1").value == "-" ? 0 : document.getElementById("txtSundayCover1").value);

            document.getElementById("txtTotalCover2").value =
			parseFloat(document.getElementById("txtMondayCover2").value == "-" ? 0 : document.getElementById("txtMondayCover2").value) +
			parseFloat(document.getElementById("txtTuesdayCover2").value == "-" ? 0 : document.getElementById("txtTuesdayCover2").value) +
			parseFloat(document.getElementById("txtWednesdayCover2").value == "-" ? 0 : document.getElementById("txtWednesdayCover2").value) +
			parseFloat(document.getElementById("txtThursdayCover2").value == "-" ? 0 : document.getElementById("txtThursdayCover2").value) +
			parseFloat(document.getElementById("txtFridayCover2").value == "-" ? 0 : document.getElementById("txtFridayCover2").value) +
			parseFloat(document.getElementById("txtSaturdayCover2").value == "-" ? 0 : document.getElementById("txtSaturdayCover2").value) +
			parseFloat(document.getElementById("txtSundayCover2").value == "-" ? 0 : document.getElementById("txtSundayCover2").value); ;

            document.getElementById("txtTotalCover3").value =
			parseFloat(document.getElementById("txtMondayCover3").value == "-" ? 0 : document.getElementById("txtMondayCover3").value) +
			parseFloat(document.getElementById("txtTuesdayCover3").value == "-" ? 0 : document.getElementById("txtTuesdayCover3").value) +
			parseFloat(document.getElementById("txtWednesdayCover3").value == "-" ? 0 : document.getElementById("txtWednesdayCover3").value) +
			parseFloat(document.getElementById("txtThursdayCover3").value == "-" ? 0 : document.getElementById("txtThursdayCover3").value) +
			parseFloat(document.getElementById("txtFridayCover3").value == "-" ? 0 : document.getElementById("txtFridayCover3").value) +
			parseFloat(document.getElementById("txtSaturdayCover3").value == "-" ? 0 : document.getElementById("txtSaturdayCover3").value) +
			parseFloat(document.getElementById("txtSundayCover3").value == "-" ? 0 : document.getElementById("txtSundayCover3").value);

            document.getElementById("txtMondayTotalCover").value =
			parseFloat(document.getElementById("txtMondayCover1").value == "-" ? 0 : document.getElementById("txtMondayCover1").value) +
			parseFloat(document.getElementById("txtMondayCover2").value == "-" ? 0 : document.getElementById("txtMondayCover2").value) +
			parseFloat(document.getElementById("txtMondayCover3").value == "-" ? 0 : document.getElementById("txtMondayCover3").value);

            document.getElementById("txtTuesdayTotalCover").value =
			parseFloat(document.getElementById("txtTuesdayCover1").value == "-" ? 0 : document.getElementById("txtTuesdayCover1").value) +
			parseFloat(document.getElementById("txtTuesdayCover2").value == "-" ? 0 : document.getElementById("txtTuesdayCover2").value) +
			parseFloat(document.getElementById("txtTuesdayCover3").value == "-" ? 0 : document.getElementById("txtTuesdayCover3").value);

            document.getElementById("txtWednesdayTotalCover").value =
			parseFloat(document.getElementById("txtWednesdayCover1").value == "-" ? 0 : document.getElementById("txtWednesdayCover1").value) +
			parseFloat(document.getElementById("txtWednesdayCover2").value == "-" ? 0 : document.getElementById("txtWednesdayCover2").value) +
			parseFloat(document.getElementById("txtWednesdayCover3").value == "-" ? 0 : document.getElementById("txtWednesdayCover3").value);

            document.getElementById("txtThursdayTotalCover").value =
			parseFloat(document.getElementById("txtThursdayCover1").value == "-" ? 0 : document.getElementById("txtThursdayCover1").value) +
			parseFloat(document.getElementById("txtThursdayCover2").value == "-" ? 0 : document.getElementById("txtThursdayCover2").value) +
			parseFloat(document.getElementById("txtThursdayCover3").value == "-" ? 0 : document.getElementById("txtThursdayCover3").value);

            document.getElementById("txtFridayTotalCover").value =
			parseFloat(document.getElementById("txtFridayCover1").value == "-" ? 0 : document.getElementById("txtFridayCover1").value) +
			parseFloat(document.getElementById("txtFridayCover2").value == "-" ? 0 : document.getElementById("txtFridayCover2").value) +
			parseFloat(document.getElementById("txtFridayCover3").value == "-" ? 0 : document.getElementById("txtFridayCover3").value);

            document.getElementById("txtSaturdayTotalCover").value =
			parseFloat(document.getElementById("txtSaturdayCover1").value == "-" ? 0 : document.getElementById("txtSaturdayCover1").value) +
			parseFloat(document.getElementById("txtSaturdayCover2").value == "-" ? 0 : document.getElementById("txtSaturdayCover2").value) +
			parseFloat(document.getElementById("txtSaturdayCover3").value == "-" ? 0 : document.getElementById("txtSaturdayCover3").value);

            document.getElementById("txtSundayTotalCover").value =
			parseFloat(document.getElementById("txtSundayCover1").value == "-" ? 0 : document.getElementById("txtSundayCover1").value) +
			parseFloat(document.getElementById("txtSundayCover2").value == "-" ? 0 : document.getElementById("txtSundayCover2").value) +
			parseFloat(document.getElementById("txtSundayCover3").value == "-" ? 0 : document.getElementById("txtSundayCover3").value);

            document.getElementById("txtTotalCover").value =
			parseFloat(document.getElementById("txtTotalCover1").value == "" ? 0 : document.getElementById("txtTotalCover1").value) +
			parseFloat(document.getElementById("txtTotalCover2").value == "" ? 0 : document.getElementById("txtTotalCover2").value) +
			parseFloat(document.getElementById("txtTotalCover3").value == "" ? 0 : document.getElementById("txtTotalCover3").value);

            document.getElementById("txtTotalField1").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField1").value == "-" ? 0 : document.getElementById("txtMondayField1").value) +
			parseFloat(document.getElementById("txtTuesdayField1").value == "-" ? 0 : document.getElementById("txtTuesdayField1").value) +
			parseFloat(document.getElementById("txtWednesdayField1").value == "-" ? 0 : document.getElementById("txtWednesdayField1").value) +
			parseFloat(document.getElementById("txtThursdayField1").value == "-" ? 0 : document.getElementById("txtThursdayField1").value) +
			parseFloat(document.getElementById("txtFridayField1").value == "-" ? 0 : document.getElementById("txtFridayField1").value) +
			parseFloat(document.getElementById("txtSaturdayField1").value == "-" ? 0 : document.getElementById("txtSaturdayField1").value) +
			parseFloat(document.getElementById("txtSundayField1").value == "-" ? 0 : document.getElementById("txtSundayField1").value)), 2);


            document.getElementById("txtTotalField2").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField2").value == "-" ? 0 : document.getElementById("txtMondayField2").value) +
			parseFloat(document.getElementById("txtTuesdayField2").value == "-" ? 0 : document.getElementById("txtTuesdayField2").value) +
			parseFloat(document.getElementById("txtWednesdayField2").value == "-" ? 0 : document.getElementById("txtWednesdayField2").value) +
			parseFloat(document.getElementById("txtThursdayField2").value == "-" ? 0 : document.getElementById("txtThursdayField2").value) +
			parseFloat(document.getElementById("txtFridayField2").value == "-" ? 0 : document.getElementById("txtFridayField2").value) +
			parseFloat(document.getElementById("txtSaturdayField2").value == "-" ? 0 : document.getElementById("txtSaturdayField2").value) +
			parseFloat(document.getElementById("txtSundayField2").value == "-" ? 0 : document.getElementById("txtSundayField2").value)), 2);

            document.getElementById("txtTotalField3").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField3").value == "-" ? 0 : document.getElementById("txtMondayField3").value) +
			parseFloat(document.getElementById("txtTuesdayField3").value == "-" ? 0 : document.getElementById("txtTuesdayField3").value) +
			parseFloat(document.getElementById("txtWednesdayField3").value == "-" ? 0 : document.getElementById("txtWednesdayField3").value) +
			parseFloat(document.getElementById("txtThursdayField3").value == "-" ? 0 : document.getElementById("txtThursdayField3").value) +
			parseFloat(document.getElementById("txtFridayField3").value == "-" ? 0 : document.getElementById("txtFridayField3").value) +
			parseFloat(document.getElementById("txtSaturdayField3").value == "-" ? 0 : document.getElementById("txtSaturdayField3").value) +
			parseFloat(document.getElementById("txtSundayField3").value == "-" ? 0 : document.getElementById("txtSundayField3").value)), 2);

            document.getElementById("txtTotalField4").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField4").value == "-" ? 0 : document.getElementById("txtMondayField4").value) +
			parseFloat(document.getElementById("txtTuesdayField4").value == "-" ? 0 : document.getElementById("txtTuesdayField4").value) +
			parseFloat(document.getElementById("txtWednesdayField4").value == "-" ? 0 : document.getElementById("txtWednesdayField4").value) +
			parseFloat(document.getElementById("txtThursdayField4").value == "-" ? 0 : document.getElementById("txtThursdayField4").value) +
			parseFloat(document.getElementById("txtFridayField4").value == "-" ? 0 : document.getElementById("txtFridayField4").value) +
			parseFloat(document.getElementById("txtSaturdayField4").value == "-" ? 0 : document.getElementById("txtSaturdayField4").value) +
			parseFloat(document.getElementById("txtSundayField4").value == "-" ? 0 : document.getElementById("txtSundayField4").value)), 2);

            document.getElementById("txtTotalField5").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField5").value == "-" ? 0 : document.getElementById("txtMondayField5").value) +
			parseFloat(document.getElementById("txtTuesdayField5").value == "-" ? 0 : document.getElementById("txtTuesdayField5").value) +
			parseFloat(document.getElementById("txtWednesdayField5").value == "-" ? 0 : document.getElementById("txtWednesdayField5").value) +
			parseFloat(document.getElementById("txtThursdayField5").value == "-" ? 0 : document.getElementById("txtThursdayField5").value) +
			parseFloat(document.getElementById("txtFridayField5").value == "-" ? 0 : document.getElementById("txtFridayField5").value) +
			parseFloat(document.getElementById("txtSaturdayField5").value == "-" ? 0 : document.getElementById("txtSaturdayField5").value) +
			parseFloat(document.getElementById("txtSundayField5").value == "-" ? 0 : document.getElementById("txtSundayField5").value)), 2);


            document.getElementById("txtTotalField6").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField6").value == "-" ? 0 : document.getElementById("txtMondayField6").value) +
			parseFloat(document.getElementById("txtTuesdayField6").value == "-" ? 0 : document.getElementById("txtTuesdayField6").value) +
			parseFloat(document.getElementById("txtWednesdayField6").value == "-" ? 0 : document.getElementById("txtWednesdayField6").value) +
			parseFloat(document.getElementById("txtThursdayField6").value == "-" ? 0 : document.getElementById("txtThursdayField6").value) +
			parseFloat(document.getElementById("txtFridayField6").value == "-" ? 0 : document.getElementById("txtFridayField6").value) +
			parseFloat(document.getElementById("txtSaturdayField6").value == "-" ? 0 : document.getElementById("txtSaturdayField6").value) +
			parseFloat(document.getElementById("txtSundayField6").value == "-" ? 0 : document.getElementById("txtSundayField6").value)), 2);


            document.getElementById("txtTotalField7").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField7").value == "-" ? 0 : document.getElementById("txtMondayField7").value) +
			parseFloat(document.getElementById("txtTuesdayField7").value == "-" ? 0 : document.getElementById("txtTuesdayField7").value) +
			parseFloat(document.getElementById("txtWednesdayField7").value == "-" ? 0 : document.getElementById("txtWednesdayField7").value) +
			parseFloat(document.getElementById("txtThursdayField7").value == "-" ? 0 : document.getElementById("txtThursdayField7").value) +
			parseFloat(document.getElementById("txtFridayField7").value == "-" ? 0 : document.getElementById("txtFridayField7").value) +
			parseFloat(document.getElementById("txtSaturdayField7").value == "-" ? 0 : document.getElementById("txtSaturdayField7").value) +
			parseFloat(document.getElementById("txtSundayField7").value == "-" ? 0 : document.getElementById("txtSundayField7").value)), 2);


            document.getElementById("txtTotalField8").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField8").value == "-" ? 0 : document.getElementById("txtMondayField8").value) +
			parseFloat(document.getElementById("txtTuesdayField8").value == "-" ? 0 : document.getElementById("txtTuesdayField8").value) +
			parseFloat(document.getElementById("txtWednesdayField8").value == "-" ? 0 : document.getElementById("txtWednesdayField8").value) +
			parseFloat(document.getElementById("txtThursdayField8").value == "-" ? 0 : document.getElementById("txtThursdayField8").value) +
			parseFloat(document.getElementById("txtFridayField8").value == "-" ? 0 : document.getElementById("txtFridayField8").value) +
			parseFloat(document.getElementById("txtSaturdayField8").value == "-" ? 0 : document.getElementById("txtSaturdayField8").value) +
			parseFloat(document.getElementById("txtSundayField8").value == "-" ? 0 : document.getElementById("txtSundayField8").value)), 2);


            document.getElementById("txtTotalField9").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField9").value == "-" ? 0 : document.getElementById("txtMondayField9").value) +
			parseFloat(document.getElementById("txtTuesdayField9").value == "-" ? 0 : document.getElementById("txtTuesdayField9").value) +
			parseFloat(document.getElementById("txtWednesdayField9").value == "-" ? 0 : document.getElementById("txtWednesdayField9").value) +
			parseFloat(document.getElementById("txtThursdayField9").value == "-" ? 0 : document.getElementById("txtThursdayField9").value) +
			parseFloat(document.getElementById("txtFridayField9").value == "-" ? 0 : document.getElementById("txtFridayField9").value) +
			parseFloat(document.getElementById("txtSaturdayField9").value == "-" ? 0 : document.getElementById("txtSaturdayField9").value) +
			parseFloat(document.getElementById("txtSundayField9").value == "-" ? 0 : document.getElementById("txtSundayField9").value)), 2);


            document.getElementById("txtTotalField10").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField10").value == "-" ? 0 : document.getElementById("txtMondayField10").value) +
			parseFloat(document.getElementById("txtTuesdayField10").value == "-" ? 0 : document.getElementById("txtTuesdayField10").value) +
			parseFloat(document.getElementById("txtWednesdayField10").value == "-" ? 0 : document.getElementById("txtWednesdayField10").value) +
			parseFloat(document.getElementById("txtThursdayField10").value == "-" ? 0 : document.getElementById("txtThursdayField10").value) +
			parseFloat(document.getElementById("txtFridayField10").value == "-" ? 0 : document.getElementById("txtFridayField10").value) +
			parseFloat(document.getElementById("txtSaturdayField10").value == "-" ? 0 : document.getElementById("txtSaturdayField10").value) +
			parseFloat(document.getElementById("txtSundayField10").value == "-" ? 0 : document.getElementById("txtSundayField10").value)), 2);


            document.getElementById("txtTotalField11").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField11").value == "-" ? 0 : document.getElementById("txtMondayField11").value) +
			parseFloat(document.getElementById("txtTuesdayField11").value == "-" ? 0 : document.getElementById("txtTuesdayField11").value) +
			parseFloat(document.getElementById("txtWednesdayField11").value == "-" ? 0 : document.getElementById("txtWednesdayField11").value) +
			parseFloat(document.getElementById("txtThursdayField11").value == "-" ? 0 : document.getElementById("txtThursdayField11").value) +
			parseFloat(document.getElementById("txtFridayField11").value == "-" ? 0 : document.getElementById("txtFridayField11").value) +
			parseFloat(document.getElementById("txtSaturdayField11").value == "-" ? 0 : document.getElementById("txtSaturdayField11").value) +
			parseFloat(document.getElementById("txtSundayField11").value == "-" ? 0 : document.getElementById("txtSundayField11").value)), 2);


            document.getElementById("txtTotalField12").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField12").value == "-" ? 0 : document.getElementById("txtMondayField12").value) +
			parseFloat(document.getElementById("txtTuesdayField12").value == "-" ? 0 : document.getElementById("txtTuesdayField12").value) +
			parseFloat(document.getElementById("txtWednesdayField12").value == "-" ? 0 : document.getElementById("txtWednesdayField12").value) +
			parseFloat(document.getElementById("txtThursdayField12").value == "-" ? 0 : document.getElementById("txtThursdayField12").value) +
			parseFloat(document.getElementById("txtFridayField12").value == "-" ? 0 : document.getElementById("txtFridayField12").value) +
			parseFloat(document.getElementById("txtSaturdayField12").value == "-" ? 0 : document.getElementById("txtSaturdayField12").value) +
			parseFloat(document.getElementById("txtSundayField12").value == "-" ? 0 : document.getElementById("txtSundayField12").value)), 2);


            document.getElementById("txtTotalField13").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField13").value == "-" ? 0 : document.getElementById("txtMondayField13").value) +
			parseFloat(document.getElementById("txtTuesdayField13").value == "-" ? 0 : document.getElementById("txtTuesdayField13").value) +
			parseFloat(document.getElementById("txtWednesdayField13").value == "-" ? 0 : document.getElementById("txtWednesdayField13").value) +
			parseFloat(document.getElementById("txtThursdayField13").value == "-" ? 0 : document.getElementById("txtThursdayField13").value) +
			parseFloat(document.getElementById("txtFridayField13").value == "-" ? 0 : document.getElementById("txtFridayField13").value) +
			parseFloat(document.getElementById("txtSaturdayField13").value == "-" ? 0 : document.getElementById("txtSaturdayField13").value) +
			parseFloat(document.getElementById("txtSundayField13").value == "-" ? 0 : document.getElementById("txtSundayField13").value)), 2);


            document.getElementById("txtTotalField14").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField14").value == "-" ? 0 : document.getElementById("txtMondayField14").value) +
			parseFloat(document.getElementById("txtTuesdayField14").value == "-" ? 0 : document.getElementById("txtTuesdayField14").value) +
			parseFloat(document.getElementById("txtWednesdayField14").value == "-" ? 0 : document.getElementById("txtWednesdayField14").value) +
			parseFloat(document.getElementById("txtThursdayField14").value == "-" ? 0 : document.getElementById("txtThursdayField14").value) +
			parseFloat(document.getElementById("txtFridayField14").value == "-" ? 0 : document.getElementById("txtFridayField14").value) +
			parseFloat(document.getElementById("txtSaturdayField14").value == "-" ? 0 : document.getElementById("txtSaturdayField14").value) +
			parseFloat(document.getElementById("txtSundayField14").value == "-" ? 0 : document.getElementById("txtSundayField14").value)), 2);

            document.getElementById("txtTotalField15").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField15").value == "-" ? 0 : document.getElementById("txtMondayField15").value) +
			parseFloat(document.getElementById("txtTuesdayField15").value == "-" ? 0 : document.getElementById("txtTuesdayField15").value) +
			parseFloat(document.getElementById("txtWednesdayField15").value == "-" ? 0 : document.getElementById("txtWednesdayField15").value) +
			parseFloat(document.getElementById("txtThursdayField15").value == "-" ? 0 : document.getElementById("txtThursdayField15").value) +
			parseFloat(document.getElementById("txtFridayField15").value == "-" ? 0 : document.getElementById("txtFridayField15").value) +
			parseFloat(document.getElementById("txtSaturdayField15").value == "-" ? 0 : document.getElementById("txtSaturdayField15").value) +
			parseFloat(document.getElementById("txtSundayField15").value == "-" ? 0 : document.getElementById("txtSundayField15").value)), 2);

            document.getElementById("txtTotalPettyCashTotal").value = roundNumber((
			parseFloat(document.getElementById("txtMondayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtMondayPettyCashTotal").value) +
			parseFloat(document.getElementById("txtTuesdayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtTuesdayPettyCashTotal").value) +
			parseFloat(document.getElementById("txtWednesdayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtWednesdayPettyCashTotal").value) +
			parseFloat(document.getElementById("txtThursdayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtThursdayPettyCashTotal").value) +
			parseFloat(document.getElementById("txtFridayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtFridayPettyCashTotal").value) +
			parseFloat(document.getElementById("txtSaturdayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtSaturdayPettyCashTotal").value) +
			parseFloat(document.getElementById("txtSundayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtSundayPettyCashTotal").value)), 2);

            document.getElementById("txtMondayTotalNet").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField1").value == "-" ? 0 : document.getElementById("txtMondayField1").value) +
			parseFloat(document.getElementById("txtMondayField2").value == "-" ? 0 : document.getElementById("txtMondayField2").value) +
			parseFloat(document.getElementById("txtMondayField3").value == "-" ? 0 : document.getElementById("txtMondayField3").value) +
			parseFloat(document.getElementById("txtMondayField4").value == "-" ? 0 : document.getElementById("txtMondayField4").value) +
			parseFloat(document.getElementById("txtMondayField5").value == "-" ? 0 : document.getElementById("txtMondayField5").value) +
			parseFloat(document.getElementById("txtMondayField6").value == "-" ? 0 : document.getElementById("txtMondayField6").value) +
			parseFloat(document.getElementById("txtMondayField7").value == "-" ? 0 : document.getElementById("txtMondayField7").value)), 2);

            document.getElementById("txtTuesdayTotalNet").value = roundNumber((
			parseFloat(document.getElementById("txtTuesdayField1").value == "-" ? 0 : document.getElementById("txtTuesdayField1").value) +
			parseFloat(document.getElementById("txtTuesdayField2").value == "-" ? 0 : document.getElementById("txtTuesdayField2").value) +
			parseFloat(document.getElementById("txtTuesdayField3").value == "-" ? 0 : document.getElementById("txtTuesdayField3").value) +
			parseFloat(document.getElementById("txtTuesdayField4").value == "-" ? 0 : document.getElementById("txtTuesdayField4").value) +
			parseFloat(document.getElementById("txtTuesdayField5").value == "-" ? 0 : document.getElementById("txtTuesdayField5").value) +
			parseFloat(document.getElementById("txtTuesdayField6").value == "-" ? 0 : document.getElementById("txtTuesdayField6").value) +
			parseFloat(document.getElementById("txtTuesdayField7").value == "-" ? 0 : document.getElementById("txtTuesdayField7").value)), 2);


            document.getElementById("txtWednesdayTotalNet").value = roundNumber((
			parseFloat(document.getElementById("txtWednesdayField1").value == "-" ? 0 : document.getElementById("txtWednesdayField1").value) +
			parseFloat(document.getElementById("txtWednesdayField2").value == "-" ? 0 : document.getElementById("txtWednesdayField2").value) +
			parseFloat(document.getElementById("txtWednesdayField3").value == "-" ? 0 : document.getElementById("txtWednesdayField3").value) +
			parseFloat(document.getElementById("txtWednesdayField4").value == "-" ? 0 : document.getElementById("txtWednesdayField4").value) +
			parseFloat(document.getElementById("txtWednesdayField5").value == "-" ? 0 : document.getElementById("txtWednesdayField5").value) +
			parseFloat(document.getElementById("txtWednesdayField6").value == "-" ? 0 : document.getElementById("txtWednesdayField6").value) +
			parseFloat(document.getElementById("txtWednesdayField7").value == "-" ? 0 : document.getElementById("txtWednesdayField7").value)), 2);


            document.getElementById("txtThursdayTotalNet").value = roundNumber((
			parseFloat(document.getElementById("txtThursdayField1").value == "-" ? 0 : document.getElementById("txtThursdayField1").value) +
			parseFloat(document.getElementById("txtThursdayField2").value == "-" ? 0 : document.getElementById("txtThursdayField2").value) +
			parseFloat(document.getElementById("txtThursdayField3").value == "-" ? 0 : document.getElementById("txtThursdayField3").value) +
			parseFloat(document.getElementById("txtThursdayField4").value == "-" ? 0 : document.getElementById("txtThursdayField4").value) +
			parseFloat(document.getElementById("txtThursdayField5").value == "-" ? 0 : document.getElementById("txtThursdayField5").value) +
			parseFloat(document.getElementById("txtThursdayField6").value == "-" ? 0 : document.getElementById("txtThursdayField6").value) +
			parseFloat(document.getElementById("txtThursdayField7").value == "-" ? 0 : document.getElementById("txtThursdayField7").value)), 2);


            document.getElementById("txtFridayTotalNet").value = roundNumber((
			parseFloat(document.getElementById("txtFridayField1").value == "-" ? 0 : document.getElementById("txtFridayField1").value) +
			parseFloat(document.getElementById("txtFridayField2").value == "-" ? 0 : document.getElementById("txtFridayField2").value) +
			parseFloat(document.getElementById("txtFridayField3").value == "-" ? 0 : document.getElementById("txtFridayField3").value) +
			parseFloat(document.getElementById("txtFridayField4").value == "-" ? 0 : document.getElementById("txtFridayField4").value) +
			parseFloat(document.getElementById("txtFridayField5").value == "-" ? 0 : document.getElementById("txtFridayField5").value) +
			parseFloat(document.getElementById("txtFridayField6").value == "-" ? 0 : document.getElementById("txtFridayField6").value) +
			parseFloat(document.getElementById("txtFridayField7").value == "-" ? 0 : document.getElementById("txtFridayField7").value)), 2);



            document.getElementById("txtSaturdayTotalNet").value = roundNumber((
			parseFloat(document.getElementById("txtSaturdayField1").value == "-" ? 0 : document.getElementById("txtSaturdayField1").value) +
			parseFloat(document.getElementById("txtSaturdayField2").value == "-" ? 0 : document.getElementById("txtSaturdayField2").value) +
			parseFloat(document.getElementById("txtSaturdayField3").value == "-" ? 0 : document.getElementById("txtSaturdayField3").value) +
			parseFloat(document.getElementById("txtSaturdayField4").value == "-" ? 0 : document.getElementById("txtSaturdayField4").value) +
			parseFloat(document.getElementById("txtSaturdayField5").value == "-" ? 0 : document.getElementById("txtSaturdayField5").value) +
			parseFloat(document.getElementById("txtSaturdayField6").value == "-" ? 0 : document.getElementById("txtSaturdayField6").value) +
			parseFloat(document.getElementById("txtSaturdayField7").value == "-" ? 0 : document.getElementById("txtSaturdayField7").value)), 2);


            document.getElementById("txtSundayTotalNet").value = roundNumber((
			parseFloat(document.getElementById("txtSundayField1").value == "-" ? 0 : document.getElementById("txtSundayField1").value) +
			parseFloat(document.getElementById("txtSundayField2").value == "-" ? 0 : document.getElementById("txtSundayField2").value) +
			parseFloat(document.getElementById("txtSundayField3").value == "-" ? 0 : document.getElementById("txtSundayField3").value) +
			parseFloat(document.getElementById("txtSundayField4").value == "-" ? 0 : document.getElementById("txtSundayField4").value) +
			parseFloat(document.getElementById("txtSundayField5").value == "-" ? 0 : document.getElementById("txtSundayField5").value) +
			parseFloat(document.getElementById("txtSundayField6").value == "-" ? 0 : document.getElementById("txtSundayField6").value) +
			parseFloat(document.getElementById("txtSundayField7").value == "-" ? 0 : document.getElementById("txtSundayField7").value)), 2);

            document.getElementById("txtMondayTotalGross").value = roundNumber((
			parseFloat(document.getElementById("txtMondayTotalNet").value == "-" ? 0 : document.getElementById("txtMondayTotalNet").value) +
			parseFloat(document.getElementById("txtMondayVat").value == "-" ? 0 : document.getElementById("txtMondayVat").value)), 2);

            document.getElementById("txtTuesdayTotalGross").value = roundNumber((
			parseFloat(document.getElementById("txtTuesdayTotalNet").value == "-" ? 0 : document.getElementById("txtTuesdayTotalNet").value) +
			parseFloat(document.getElementById("txtTuesdayVat").value == "-" ? 0 : document.getElementById("txtTuesdayVat").value)), 2);

            document.getElementById("txtWednesdayTotalGross").value = roundNumber((
			parseFloat(document.getElementById("txtWednesdayTotalNet").value == "-" ? 0 : document.getElementById("txtWednesdayTotalNet").value) +
			parseFloat(document.getElementById("txtWednesdayVat").value == "-" ? 0 : document.getElementById("txtWednesdayVat").value)), 2);

            document.getElementById("txtThursdayTotalGross").value = roundNumber((
			parseFloat(document.getElementById("txtThursdayTotalNet").value == "-" ? 0 : document.getElementById("txtThursdayTotalNet").value) +
			parseFloat(document.getElementById("txtThursdayVat").value == "-" ? 0 : document.getElementById("txtThursdayVat").value)), 2);

            document.getElementById("txtFridayTotalGross").value = roundNumber((
			parseFloat(document.getElementById("txtFridayTotalNet").value == "-" ? 0 : document.getElementById("txtFridayTotalNet").value) +
			parseFloat(document.getElementById("txtFridayVat").value == "-" ? 0 : document.getElementById("txtFridayVat").value)), 2);

            document.getElementById("txtSaturdayTotalGross").value = roundNumber((
			parseFloat(document.getElementById("txtSaturdayTotalNet").value == "-" ? 0 : document.getElementById("txtSaturdayTotalNet").value) +
			parseFloat(document.getElementById("txtSaturdayVat").value == "-" ? 0 : document.getElementById("txtSaturdayVat").value)), 2);

            document.getElementById("txtSundayTotalGross").value = roundNumber((
			parseFloat(document.getElementById("txtSundayTotalNet").value == "-" ? 0 : document.getElementById("txtSundayTotalNet").value) +
			parseFloat(document.getElementById("txtSundayVat").value == "-" ? 0 : document.getElementById("txtSundayVat").value)), 2);

            document.getElementById("txtMondayFieldTotal").value = roundNumber((
			parseFloat(document.getElementById("txtMondayField8").value == "-" ? 0 : document.getElementById("txtMondayField8").value) +
			parseFloat(document.getElementById("txtMondayField9").value == "-" ? 0 : document.getElementById("txtMondayField9").value) +
			parseFloat(document.getElementById("txtMondayField10").value == "-" ? 0 : document.getElementById("txtMondayField10").value) +
			parseFloat(document.getElementById("txtMondayField11").value == "-" ? 0 : document.getElementById("txtMondayField11").value) +
			parseFloat(document.getElementById("txtMondayField12").value == "-" ? 0 : document.getElementById("txtMondayField12").value) +
			parseFloat(document.getElementById("txtMondayField13").value == "-" ? 0 : document.getElementById("txtMondayField13").value) +
			parseFloat(document.getElementById("txtMondayField14").value == "-" ? 0 : document.getElementById("txtMondayField14").value) +
			parseFloat(document.getElementById("txtMondayField15").value == "-" ? 0 : document.getElementById("txtMondayField15").value) +
			parseFloat(document.getElementById("txtMondayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtMondayPettyCashTotal").value)), 2);

            document.getElementById("txtTuesdayFieldTotal").value = roundNumber((
			parseFloat(document.getElementById("txtTuesdayField8").value == "-" ? 0 : document.getElementById("txtTuesdayField8").value) +
			parseFloat(document.getElementById("txtTuesdayField9").value == "-" ? 0 : document.getElementById("txtTuesdayField9").value) +
			parseFloat(document.getElementById("txtTuesdayField10").value == "-" ? 0 : document.getElementById("txtTuesdayField10").value) +
			parseFloat(document.getElementById("txtTuesdayField11").value == "-" ? 0 : document.getElementById("txtTuesdayField11").value) +
			parseFloat(document.getElementById("txtTuesdayField12").value == "-" ? 0 : document.getElementById("txtTuesdayField12").value) +
			parseFloat(document.getElementById("txtTuesdayField13").value == "-" ? 0 : document.getElementById("txtTuesdayField13").value) +
			parseFloat(document.getElementById("txtTuesdayField14").value == "-" ? 0 : document.getElementById("txtTuesdayField14").value) +
			parseFloat(document.getElementById("txtTuesdayField15").value == "-" ? 0 : document.getElementById("txtTuesdayField15").value) +
			parseFloat(document.getElementById("txtTuesdayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtTuesdayPettyCashTotal").value)), 2);

            document.getElementById("txtWednesdayFieldTotal").value = roundNumber((
			parseFloat(document.getElementById("txtWednesdayField8").value == "-" ? 0 : document.getElementById("txtWednesdayField8").value) +
			parseFloat(document.getElementById("txtWednesdayField9").value == "-" ? 0 : document.getElementById("txtWednesdayField9").value) +
			parseFloat(document.getElementById("txtWednesdayField10").value == "-" ? 0 : document.getElementById("txtWednesdayField10").value) +
			parseFloat(document.getElementById("txtWednesdayField11").value == "-" ? 0 : document.getElementById("txtWednesdayField11").value) +
			parseFloat(document.getElementById("txtWednesdayField12").value == "-" ? 0 : document.getElementById("txtWednesdayField12").value) +
			parseFloat(document.getElementById("txtWednesdayField13").value == "-" ? 0 : document.getElementById("txtWednesdayField13").value) +
			parseFloat(document.getElementById("txtWednesdayField14").value == "-" ? 0 : document.getElementById("txtWednesdayField14").value) +
			parseFloat(document.getElementById("txtWednesdayField15").value == "-" ? 0 : document.getElementById("txtWednesdayField15").value) +
			parseFloat(document.getElementById("txtWednesdayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtWednesdayPettyCashTotal").value)), 2);

            document.getElementById("txtThursdayFieldTotal").value = roundNumber((
			parseFloat(document.getElementById("txtThursdayField8").value == "-" ? 0 : document.getElementById("txtThursdayField8").value) +
			parseFloat(document.getElementById("txtThursdayField9").value == "-" ? 0 : document.getElementById("txtThursdayField9").value) +
			parseFloat(document.getElementById("txtThursdayField10").value == "-" ? 0 : document.getElementById("txtThursdayField10").value) +
			parseFloat(document.getElementById("txtThursdayField11").value == "-" ? 0 : document.getElementById("txtThursdayField11").value) +
			parseFloat(document.getElementById("txtThursdayField12").value == "-" ? 0 : document.getElementById("txtThursdayField12").value) +
			parseFloat(document.getElementById("txtThursdayField13").value == "-" ? 0 : document.getElementById("txtThursdayField13").value) +
			parseFloat(document.getElementById("txtThursdayField14").value == "-" ? 0 : document.getElementById("txtThursdayField14").value) +
			parseFloat(document.getElementById("txtThursdayField15").value == "-" ? 0 : document.getElementById("txtThursdayField15").value) +
			parseFloat(document.getElementById("txtThursdayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtThursdayPettyCashTotal").value)), 2);


            document.getElementById("txtFridayFieldTotal").value = roundNumber((
			parseFloat(document.getElementById("txtFridayField8").value == "-" ? 0 : document.getElementById("txtFridayField8").value) +
			parseFloat(document.getElementById("txtFridayField9").value == "-" ? 0 : document.getElementById("txtFridayField9").value) +
			parseFloat(document.getElementById("txtFridayField10").value == "-" ? 0 : document.getElementById("txtFridayField10").value) +
			parseFloat(document.getElementById("txtFridayField11").value == "-" ? 0 : document.getElementById("txtFridayField11").value) +
			parseFloat(document.getElementById("txtFridayField12").value == "-" ? 0 : document.getElementById("txtFridayField12").value) +
			parseFloat(document.getElementById("txtFridayField13").value == "-" ? 0 : document.getElementById("txtFridayField13").value) +
			parseFloat(document.getElementById("txtFridayField14").value == "-" ? 0 : document.getElementById("txtFridayField14").value) +
			parseFloat(document.getElementById("txtFridayField15").value == "-" ? 0 : document.getElementById("txtFridayField15").value) +
			parseFloat(document.getElementById("txtFridayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtFridayPettyCashTotal").value)), 2);

            document.getElementById("txtSaturdayFieldTotal").value = roundNumber((
			parseFloat(document.getElementById("txtSaturdayField8").value == "-" ? 0 : document.getElementById("txtSaturdayField8").value) +
			parseFloat(document.getElementById("txtSaturdayField9").value == "-" ? 0 : document.getElementById("txtSaturdayField9").value) +
			parseFloat(document.getElementById("txtSaturdayField10").value == "-" ? 0 : document.getElementById("txtSaturdayField10").value) +
			parseFloat(document.getElementById("txtSaturdayField11").value == "-" ? 0 : document.getElementById("txtSaturdayField11").value) +
			parseFloat(document.getElementById("txtSaturdayField12").value == "-" ? 0 : document.getElementById("txtSaturdayField12").value) +
			parseFloat(document.getElementById("txtSaturdayField13").value == "-" ? 0 : document.getElementById("txtSaturdayField13").value) +
			parseFloat(document.getElementById("txtSaturdayField14").value == "-" ? 0 : document.getElementById("txtSaturdayField14").value) +
			parseFloat(document.getElementById("txtSaturdayField15").value == "-" ? 0 : document.getElementById("txtSaturdayField15").value) +
			parseFloat(document.getElementById("txtSaturdayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtSaturdayPettyCashTotal").value)), 2);

            document.getElementById("txtSundayFieldTotal").value = roundNumber((
			parseFloat(document.getElementById("txtSundayField8").value == "-" ? 0 : document.getElementById("txtSundayField8").value) +
			parseFloat(document.getElementById("txtSundayField9").value == "-" ? 0 : document.getElementById("txtSundayField9").value) +
			parseFloat(document.getElementById("txtSundayField10").value == "-" ? 0 : document.getElementById("txtSundayField10").value) +
			parseFloat(document.getElementById("txtSundayField11").value == "-" ? 0 : document.getElementById("txtSundayField11").value) +
			parseFloat(document.getElementById("txtSundayField12").value == "-" ? 0 : document.getElementById("txtSundayField12").value) +
			parseFloat(document.getElementById("txtSundayField13").value == "-" ? 0 : document.getElementById("txtSundayField13").value) +
			parseFloat(document.getElementById("txtSundayField14").value == "-" ? 0 : document.getElementById("txtSundayField14").value) +
			parseFloat(document.getElementById("txtSundayField15").value == "-" ? 0 : document.getElementById("txtSundayField15").value) +
			parseFloat(document.getElementById("txtSundayPettyCashTotal").value == "-" ? 0 : document.getElementById("txtSundayPettyCashTotal").value)), 2);



            var str1 =
			parseFloat(document.getElementById("txtMondayFieldTotal").value == "" ? 0 : document.getElementById("txtMondayFieldTotal").value) -
			parseFloat(document.getElementById("txtMondayTotalGross").value == "" ? 0 : document.getElementById("txtMondayTotalGross").value);
            if (parseInt(str1) >= 0) {
                document.getElementById("txtMondayFieldVariance").value = roundNumber(str1, 2);
                document.getElementById("txtMondayStatus").className = "Redmark";
                document.getElementById("txtMondayStatus").value = "OVER-BANKED";
                if (parseInt(str1) == 0) {
                    document.getElementById("txtMondayStatus").className = "Greenmark";
                    document.getElementById("txtMondayStatus").value = "BALANCED";
                }
            }
            else {
                document.getElementById("txtMondayStatus").className = "Redmark";
                document.getElementById("txtMondayStatus").value = "UNDER-BANKED";
                document.getElementById("txtMondayFieldVariance").className = "Redmark";
                document.getElementById("txtMondayFieldVariance").value = "( " + roundNumber((
				parseFloat(document.getElementById("txtMondayTotalGross").value == "" ? 0 : document.getElementById("txtMondayTotalGross").value) -
				parseFloat(document.getElementById("txtMondayFieldTotal").value == "" ? 0 : document.getElementById("txtMondayFieldTotal").value)), 2) + " )";

            }

            var str2 =
			parseFloat(document.getElementById("txtTuesdayFieldTotal").value == "" ? 0 : document.getElementById("txtTuesdayFieldTotal").value) -
			parseFloat(document.getElementById("txtTuesdayTotalGross").value == "" ? 0 : document.getElementById("txtTuesdayTotalGross").value);
            if (parseInt(str2) >= 0) {
                document.getElementById("txtTuesdayFieldVariance").value = roundNumber(str2, 2);
                document.getElementById("txtTuesdayStatus").className = "Redmark";
                document.getElementById("txtTuesdayStatus").value = "OVER-BANKED";
                if (parseInt(str2) == 0) {
                    document.getElementById("txtTuesdayStatus").className = "Greenmark";
                    document.getElementById("txtTuesdayStatus").value = "BALANCED";
                }
            }
            else {
                document.getElementById("txtTuesdayStatus").className = "Redmark";
                document.getElementById("txtTuesdayStatus").value = "UNDER-BANKED";

                document.getElementById("txtTuesdayFieldVariance").className = "Redmark";
                document.getElementById("txtTuesdayFieldVariance").value = "( " + roundNumber((
				parseFloat(document.getElementById("txtTuesdayTotalGross").value == "" ? 0 : document.getElementById("txtTuesdayTotalGross").value) -
				parseFloat(document.getElementById("txtTuesdayFieldTotal").value == "" ? 0 : document.getElementById("txtTuesdayFieldTotal").value)), 2) + " )";

            }

            var str3 =
			parseFloat(document.getElementById("txtWednesdayFieldTotal").value == "" ? 0 : document.getElementById("txtWednesdayFieldTotal").value) -
			parseFloat(document.getElementById("txtWednesdayTotalGross").value == "" ? 0 : document.getElementById("txtWednesdayTotalGross").value);
            if (parseInt(str3) >= 0) {
                document.getElementById("txtWednesdayFieldVariance").value = roundNumber(str3, 2);
                document.getElementById("txtWednessdayStatus").className = "Redmark";
                document.getElementById("txtWednessdayStatus").value = "OVER-BANKED";
                if (parseInt(str3) == 0) {
                    document.getElementById("txtWednessdayStatus").className = "Greenmark";
                    document.getElementById("txtWednessdayStatus").value = "BALANCED";
                }
            }
            else {
                document.getElementById("txtWednessdayStatus").className = "Redmark";
                document.getElementById("txtWednessdayStatus").value = "UNDER-BANKED";

                document.getElementById("txtWednesdayFieldVariance").className = "Redmark";
                document.getElementById("txtWednesdayFieldVariance").value = "( " + roundNumber((
				parseFloat(document.getElementById("txtWednesdayTotalGross").value == "" ? 0 : document.getElementById("txtWednesdayTotalGross").value) -
				parseFloat(document.getElementById("txtWednesdayFieldTotal").value == "" ? 0 : document.getElementById("txtWednesdayFieldTotal").value)), 2) + " )";

            }

            var str4 =
			parseFloat(document.getElementById("txtThursdayFieldTotal").value == "" ? 0 : document.getElementById("txtThursdayFieldTotal").value) -
			parseFloat(document.getElementById("txtThursdayTotalGross").value == "" ? 0 : document.getElementById("txtThursdayTotalGross").value);
            if (parseInt(str4) >= 0) {
                document.getElementById("txtThursdayFieldVariance").value = roundNumber(str4, 2);

                document.getElementById("txtThursdayStatus").className = "Redmark";
                document.getElementById("txtThursdayStatus").value = "OVER-BANKED";
                if (parseInt(str4) == 0) {
                    document.getElementById("txtThursdayStatus").className = "Greenmark";
                    document.getElementById("txtThursdayStatus").value = "BALANCED";
                }
            }
            else {
                document.getElementById("txtThursdayStatus").className = "Redmark";
                document.getElementById("txtThursdayStatus").value = "UNDER-BANKED";

                document.getElementById("txtThursdayFieldVariance").className = "Redmark";
                document.getElementById("txtThursdayFieldVariance").value = "( " + roundNumber((
				parseFloat(document.getElementById("txtThursdayTotalGross").value == "" ? 0 : document.getElementById("txtThursdayTotalGross").value) -
				parseFloat(document.getElementById("txtThursdayFieldTotal").value == "" ? 0 : document.getElementById("txtThursdayFieldTotal").value)), 2) + " )";

            }

            var str5 =
			parseFloat(document.getElementById("txtFridayFieldTotal").value == "" ? 0 : document.getElementById("txtFridayFieldTotal").value) -
			parseFloat(document.getElementById("txtFridayTotalGross").value == "" ? 0 : document.getElementById("txtFridayTotalGross").value);
            if (parseInt(str5) >= 0) {
                document.getElementById("txtFridayFieldVariance").value = roundNumber(str5, 2);

                document.getElementById("txtFridayStatus").className = "Redmark";
                document.getElementById("txtFridayStatus").value = "OVER-BANKED";
                if (parseInt(str5) == 0) {
                    document.getElementById("txtFridayStatus").className = "Greenmark";
                    document.getElementById("txtFridayStatus").value = "BALANCED";
                }
            }
            else {
                document.getElementById("txtFridayStatus").className = "Redmark";
                document.getElementById("txtFridayStatus").value = "UNDER-BANKED";

                document.getElementById("txtFridayFieldVariance").className = "Redmark";
                document.getElementById("txtFridayFieldVariance").value = "( " + roundNumber((
				parseFloat(document.getElementById("txtFridayTotalGross").value == "" ? 0 : document.getElementById("txtFridayTotalGross").value) -
				parseFloat(document.getElementById("txtFridayFieldTotal").value == "" ? 0 : document.getElementById("txtFridayFieldTotal").value)), 2) + " )";

            }

            var str6 =
			parseFloat(document.getElementById("txtSaturdayFieldTotal").value == "" ? 0 : document.getElementById("txtSaturdayFieldTotal").value) -
			parseFloat(document.getElementById("txtSaturdayTotalGross").value == "" ? 0 : document.getElementById("txtSaturdayTotalGross").value);
            if (parseInt(str6) >= 0) {
                document.getElementById("txtSaturdayFieldVariance").value = roundNumber(str6, 2);


                document.getElementById("txtSaturdayStatus").className = "Redmark";
                document.getElementById("txtSaturdayStatus").value = "OVER-BANKED";
                if (parseInt(str6) == 0) {
                    document.getElementById("txtSaturdayStatus").className = "Greenmark";
                    document.getElementById("txtSaturdayStatus").value = "BALANCED";
                }
            }
            else {
                document.getElementById("txtSaturdayStatus").className = "Redmark";
                document.getElementById("txtSaturdayStatus").value = "UNDER-BANKED";

                document.getElementById("txtSaturdayFieldVariance").className = "Redmark";
                document.getElementById("txtSaturdayFieldVariance").value = "( " + roundNumber((
				parseFloat(document.getElementById("txtSaturdayTotalGross").value == "" ? 0 : document.getElementById("txtSaturdayTotalGross").value) -
				parseFloat(document.getElementById("txtSaturdayFieldTotal").value == "" ? 0 : document.getElementById("txtSaturdayFieldTotal").value)), 2) + " )";

            }
            var str7 =
			parseFloat(document.getElementById("txtSundayFieldTotal").value == "" ? 0 : document.getElementById("txtSundayFieldTotal").value) -
			parseFloat(document.getElementById("txtSundayTotalGross").value == "" ? 0 : document.getElementById("txtSundayTotalGross").value);
            if (parseInt(str7) >= 0) {
                document.getElementById("txtSundayFieldVariance").value = roundNumber(str7, 2);

                document.getElementById("txtSundayStatus").className = "Redmark";
                document.getElementById("txtSundayStatus").value = "OVER-BANKED";
                if (parseInt(str7) == 0) {
                    document.getElementById("txtSundayStatus").className = "Greenmark";
                    document.getElementById("txtSundayStatus").value = "BALANCED";
                }
            }
            else {
                document.getElementById("txtSundayStatus").className = "Redmark";
                document.getElementById("txtSundayStatus").value = "UNDER-BANKED";


                document.getElementById("txtSundayFieldVariance").className = "Redmark";
                document.getElementById("txtSundayFieldVariance").value = "( " + roundNumber((
				parseFloat(document.getElementById("txtSundayTotalGross").value == "" ? 0 : document.getElementById("txtSundayTotalGross").value) -
				parseFloat(document.getElementById("txtSundayFieldTotal").value == "" ? 0 : document.getElementById("txtSundayFieldTotal").value)), 2) + " )";

            }
            var str8 = parseFloat(str1) + parseFloat(str2) + parseFloat(str3) + parseFloat(str4) + parseFloat(str5) + parseFloat(str6) + parseFloat(str7);
            if (parseInt(str8) >= 0) {
                document.getElementById("txtTotalFieldVariance").value = roundNumber(str8, 2);
            }
            else {
                document.getElementById("txtTotalFieldVariance").className = "Redmark";
                document.getElementById("txtTotalFieldVariance").value = "( " + (parseFloat(roundNumber(str8, 2)) * (-1)) + " )";

            }

            document.getElementById("txtTotalFieldTotal").value = roundNumber((
			parseFloat(document.getElementById("txtMondayFieldTotal").value == "" ? 0 : document.getElementById("txtMondayFieldTotal").value) +
			parseFloat(document.getElementById("txtTuesdayFieldTotal").value == "" ? 0 : document.getElementById("txtTuesdayFieldTotal").value) +
			parseFloat(document.getElementById("txtWednesdayFieldTotal").value == "" ? 0 : document.getElementById("txtWednesdayFieldTotal").value) +
			parseFloat(document.getElementById("txtThursdayFieldTotal").value == "" ? 0 : document.getElementById("txtThursdayFieldTotal").value) +
			parseFloat(document.getElementById("txtFridayFieldTotal").value == "" ? 0 : document.getElementById("txtFridayFieldTotal").value) +
			parseFloat(document.getElementById("txtSaturdayFieldTotal").value == "" ? 0 : document.getElementById("txtSaturdayFieldTotal").value) +
			parseFloat(document.getElementById("txtSundayFieldTotal").value == "" ? 0 : document.getElementById("txtSundayFieldTotal").value)), 2);

            document.getElementById("txtTotalTotalGross").value = roundNumber((
			parseFloat(document.getElementById("txtMondayTotalGross").value == "" ? 0 : document.getElementById("txtMondayTotalGross").value) +
			parseFloat(document.getElementById("txtTuesdayTotalGross").value == "" ? 0 : document.getElementById("txtTuesdayTotalGross").value) +
			parseFloat(document.getElementById("txtWednesdayTotalGross").value == "" ? 0 : document.getElementById("txtWednesdayTotalGross").value) +
			parseFloat(document.getElementById("txtThursdayTotalGross").value == "" ? 0 : document.getElementById("txtThursdayTotalGross").value) +
			parseFloat(document.getElementById("txtFridayTotalGross").value == "" ? 0 : document.getElementById("txtFridayTotalGross").value) +
			parseFloat(document.getElementById("txtSaturdayTotalGross").value == "" ? 0 : document.getElementById("txtSaturdayTotalGross").value) +
			parseFloat(document.getElementById("txtSundayTotalGross").value == "" ? 0 : document.getElementById("txtSundayTotalGross").value)), 2);

            document.getElementById("txtTotalVat").value = roundNumber((
			parseFloat(document.getElementById("txtMondayVat").value == "-" ? 0 : document.getElementById("txtMondayVat").value) +
			parseFloat(document.getElementById("txtTuesdayVat").value == "-" ? 0 : document.getElementById("txtTuesdayVat").value) +
			parseFloat(document.getElementById("txtWednesdayVat").value == "-" ? 0 : document.getElementById("txtWednesdayVat").value) +
			parseFloat(document.getElementById("txtThursdayVat").value == "-" ? 0 : document.getElementById("txtThursdayVat").value) +
			parseFloat(document.getElementById("txtFridayVat").value == "-" ? 0 : document.getElementById("txtFridayVat").value) +
			parseFloat(document.getElementById("txtSaturdayVat").value == "-" ? 0 : document.getElementById("txtSaturdayVat").value) +
			parseFloat(document.getElementById("txtSundayVat").value == "-" ? 0 : document.getElementById("txtSundayVat").value)), 2);

            document.getElementById("txtTotalTotalNet").value = roundNumber((
			parseFloat(document.getElementById("txtMondayTotalNet").value == "" ? 0 : document.getElementById("txtMondayTotalNet").value) +
			parseFloat(document.getElementById("txtTuesdayTotalNet").value == "" ? 0 : document.getElementById("txtTuesdayTotalNet").value) +
			parseFloat(document.getElementById("txtWednesdayTotalNet").value == "" ? 0 : document.getElementById("txtWednesdayTotalNet").value) +
			parseFloat(document.getElementById("txtThursdayTotalNet").value == "" ? 0 : document.getElementById("txtThursdayTotalNet").value) +
			parseFloat(document.getElementById("txtFridayTotalNet").value == "" ? 0 : document.getElementById("txtFridayTotalNet").value) +
			parseFloat(document.getElementById("txtSaturdayTotalNet").value == "" ? 0 : document.getElementById("txtSaturdayTotalNet").value) +
			parseFloat(document.getElementById("txtSundayTotalNet").value == "" ? 0 : document.getElementById("txtSundayTotalNet").value)), 2)

            if (document.getElementById("txtMondayTotalNet").value != "0.00" && document.getElementById("txtMondayTotalCover").value != "0") {
                var per1 = parseFloat(
				parseFloat(document.getElementById("txtMondayTotalNet").value == "" ? 0 : document.getElementById("txtMondayTotalNet").value) /
				parseFloat(document.getElementById("txtMondayTotalCover").value == "" ? 0 : document.getElementById("txtMondayTotalCover").value));
                document.getElementById("txtMondayAverageCover").value = roundNumber(per1, 2);
            }
            else {
                document.getElementById("txtMondayAverageCover").value = "0.00";
            }
            if (document.getElementById("txtTuesdayTotalNet").value != "0.00" && document.getElementById("txtTuesdayTotalCover").value != "0") {
                var per2 = parseFloat(
				parseFloat(document.getElementById("txtTuesdayTotalNet").value == "" ? 0 : document.getElementById("txtTuesdayTotalNet").value) /
				parseFloat(document.getElementById("txtTuesdayTotalCover").value == "" ? 0 : document.getElementById("txtTuesdayTotalCover").value));
                document.getElementById("txtTuesdayAverageCover").value = roundNumber(per2, 2);
            }
            else {
                document.getElementById("txtTuesdayAverageCover").value = "0.00";
            }
            if (document.getElementById("txtWednesdayTotalNet").value != "0.00" && document.getElementById("txtWednesdayTotalCover").value != "0") {
                var per3 = parseFloat(
				parseFloat(document.getElementById("txtWednesdayTotalNet").value == "" ? 0 : document.getElementById("txtWednesdayTotalNet").value) /
				parseFloat(document.getElementById("txtWednesdayTotalCover").value == "" ? 0 : document.getElementById("txtWednesdayTotalCover").value));
                document.getElementById("txtWednesdayAverageCover").value = roundNumber(per3, 2);
            }
            else {
                document.getElementById("txtWednesdayAverageCover").value = "0.00";
            }
            if (document.getElementById("txtThursdayTotalNet").value != "0.00" && document.getElementById("txtThursdayTotalCover").value != "0") {
                var per4 = parseFloat(
				parseFloat(document.getElementById("txtThursdayTotalNet").value == "" ? 0 : document.getElementById("txtThursdayTotalNet").value) /
				parseFloat(document.getElementById("txtThursdayTotalCover").value == "" ? 0 : document.getElementById("txtThursdayTotalCover").value));
                document.getElementById("txtThursdayAverageCover").value = roundNumber(per4, 2);
            }
            else {
                document.getElementById("txtThursdayAverageCover").value = "0.00";
            }
            if (document.getElementById("txtFridayTotalNet").value != "0.00" && document.getElementById("txtFridayTotalCover").value != "0") {
                var per5 = (
				parseFloat(document.getElementById("txtFridayTotalNet").value == "" ? 0 : document.getElementById("txtFridayTotalNet").value) /
				parseFloat(document.getElementById("txtFridayTotalCover").value == "" ? 0 : document.getElementById("txtFridayTotalCover").value));
                document.getElementById("txtFridayAverageCover").value = roundNumber(per5, 2);
            }
            else {
                document.getElementById("txtFridayAverageCover").value = "0.00";
            }
            if (document.getElementById("txtSaturdayTotalNet").value != "0.00" && document.getElementById("txtSaturdayTotalCover").value != "0") {
                var per6 = parseFloat(
				parseFloat(document.getElementById("txtSaturdayTotalNet").value == "" ? 0 : document.getElementById("txtSaturdayTotalNet").value) /
				parseFloat(document.getElementById("txtSaturdayTotalCover").value == "" ? 0 : document.getElementById("txtSaturdayTotalCover").value));
                document.getElementById("txtSaturdayAverageCoverame").value = roundNumber(per6, 2);
            }
            else {
                document.getElementById("txtSaturdayAverageCoverame").value = "0.00";
            }
            if (document.getElementById("txtSundayTotalNet").value != "0.00" && document.getElementById("txtSundayTotalCover").value != "0") {
                var per7 = parseFloat(
				parseFloat(document.getElementById("txtSundayTotalNet").value == "" ? 0 : document.getElementById("txtSundayTotalNet").value) /
				parseFloat(document.getElementById("txtSundayTotalCover").value == "" ? 0 : document.getElementById("txtSundayTotalCover").value));
                document.getElementById("txtSundayAverageCover").value = roundNumber(per7, 2);
            }
            else {
                document.getElementById("txtSundayAverageCover").value = "0.00";
            }
            if (document.getElementById("txtTotalTotalNet").value != "0.00" && document.getElementById("txtTotalCover").value != "0") {
                var per8 = parseFloat(
				parseFloat(document.getElementById("txtTotalTotalNet").value == "" ? 0 : document.getElementById("txtTotalTotalNet").value) /
				parseFloat(document.getElementById("txtTotalCover").value == "" ? 0 : document.getElementById("txtTotalCover").value));
                document.getElementById("txtAverageCover").value = roundNumber(per8, 2);
            }
            else {
                document.getElementById("txtAverageCover").value = "0.00";
            }
        }
		
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:Panel ID="Panel2" Style="z-index: 102; left: 0px" runat="server" Width="100%">
        <uc1:bannerUM ID="bannerUM1" runat="server"></uc1:bannerUM>
    </asp:Panel>
    <table id="Table1" style="width: 895px; height: 1200px">
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
                <table class="fon" id="wraper" style="width: 895px; height: 1200px" cellpadding="7">
                    <tr>
                        <td>
                            <table class="hed" cellspacing="0" cellpadding="0" style="width: 700px;">
                                <tr>
                                    <td align="right">
                                        <strong>Company</strong>
                                    </td>
                                    <td align="right">
                                        <input id="txtCompany" style="width: 125px; font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="Company" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Week No</strong>
                                    </td>
                                    <td align="right">
                                        <input id="txtWeekNo" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="WeekNo" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Date Closed</strong>
                                    </td>
                                    <td align="right">
                                        <input id="txtDateClosed" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="DateClosed" runat="server">
                                    </td>
                                    <td style="width: 63px" align="right">
                                        <strong>Status</strong>
                                    </td>
                                    <td align="right">
                                        <input id="txtStatus" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="Status" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <strong>Department</strong>
                                    </td>
                                    <td align="right">
                                        <input id="txtDepartment" style="width: 125px; font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="Department" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Period No</strong>
                                    </td>
                                    <td align="right">
                                        <input id="txtPeriodNo" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="PeriodNo" runat="server">
                                    </td>
                                    <td align="right">
                                        <strong>Closed By</strong>
                                    </td>
                                    <td align="right">
                                        <input id="txtClosedBy" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="Closedby" runat="server">
                                    </td>
                                    <td style="width: 63px">
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnClosed2" Text="Close Week" runat="server" CssClass="ButtonCss_CMS">
                                        </asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="151">
                                        <strong>Day</strong>
                                    </td>
                                    <td>
                                        <input class="ButtonCss_CMS" id="btnMonday" readonly type="button" value="MONDAY"
                                            name="MONDAY" runat="server">
                                    </td>
                                    <td>
                                        <input class="ButtonCss_CMS" id="btnTuesday" readonly type="button" value="TUESDAY"
                                            name="TUESDAY" runat="server">
                                    </td>
                                    <td>
                                        <input class="ButtonCss_CMS" id="btnWednessday" readonly type="button" value="WEDNESDAY"
                                            name="WEDNESDAY" runat="server">
                                    </td>
                                    <td>
                                        <input class="ButtonCss_CMS" id="btnThursday" readonly type="button" value="THURSDAY"
                                            name="THURSDAY" runat="server">
                                    </td>
                                    <td>
                                        <input class="ButtonCss_CMS" id="btnFriday" readonly type="button" value="FRIDAY"
                                            name="FRIDAY" runat="server">
                                    </td>
                                    <td>
                                        <input class="ButtonCss_CMS" id="btnSaturday" readonly type="button" value="SATURDAY"
                                            name="SATURDAY" runat="server">
                                    </td>
                                    <td>
                                        <input class="ButtonCss_CMS" id="btnSunday" readonly type="button" value="SUNDAY"
                                            name="SUNDAY" runat="server">
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtTotal" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" value="TOTAL" name="TOTAL">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Date</strong>
                                    </td>
                                    <td>
                                        <input id="txtMonday" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="txtMonday" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesday" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="txtTuesday" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednessday" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="txtWednessday" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTursday" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="txtTursday" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFriday" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="txtFriday" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturday" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="txtSaturday" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSunday" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" name="txtSunday" runat="server">
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Status</strong>
                                    </td>
                                    <td>
                                        <input id="txtMondayStatus" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" value="OVER-BANKED" name="txtMondayStatus" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayStatus" style="font-weight: bold; float: none; text-align: center"
                                            type="text" value="UNDER-BANKED" name="txtTuesdayStatus" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednessdayStatus" style="font-weight: bold; float: none; text-align: center"
                                            type="text" value="BALANCED" name="txtWednessdayStatus" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayStatus" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" value="BALANCED" name="txtThursdayStatus" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayStatus" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" value="BALANCED" name="txtFridayStatus" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayStatus" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" value="BALANCED" name="txtSaturdayStatus" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayStatus" style="font-weight: bold; float: none; text-align: center"
                                            readonly type="text" value="BALANCED" name="txtSundayStatus" runat="server">
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblCovers1" Font-Italic="True" runat="server">Covers1</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayCover1" readonly type="text" value="-" name="txtMondayCover1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayCover1" readonly type="text" value="-" name="txtTuesdayCover1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayCover1" readonly type="text" value="-" name="txtWednesdayCover1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayCover1" readonly type="text" value="-" name="txtThursdayCover1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayCover1" readonly type="text" value="-" name="txtFridayCover1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayCover1" readonly type="text" value="-" name="txtSaturdayCover1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayCover1" readonly type="text" value="-" name="txtSundayCover1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalCover1" style="font-weight: bold" readonly type="text" value="-"
                                            name="txtTotalCover1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblCovers2" Font-Italic="True" runat="server">Covers2</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayCover2" readonly type="text" value="-" name="txtMondayCover2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayCover2" readonly type="text" value="-" name="txtTuesdayCover2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayCover2" readonly type="text" value="-" name="txtWednesdayCover2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayCover2" readonly type="text" value="-" name="txtThursdayCover2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayCover2" readonly type="text" value="-" name="txtFridayCover2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayCover2" readonly type="text" value="-" name="txtSaturdayCover2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayCover2" readonly type="text" value="-" name="txtSundayCover2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalCover2" style="font-weight: bold" readonly type="text" value="-"
                                            name="txtTotalCover2" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblCovers3" Font-Italic="True" runat="server">Covers3</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayCover3" readonly type="text" value="-" name="txtMondayCover3"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayCover3" readonly type="text" value="-" name="txtTuesdayCover3"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayCover3" readonly type="text" value="-" name="txtWednesdayCover3"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayCover3" readonly type="text" value="-" name="txtThursdayCover3"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayCover3" readonly type="text" value="-" name="txtFridayCover3"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayCover3" readonly type="text" value="-" name="txtSaturdayCover3"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayCover3" readonly type="text" value="-" name="txtSundayCover3"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalCover3" style="font-weight: bold" readonly type="text" value="-"
                                            name="txtTotalCover3" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Total Covers</strong>
                                    </td>
                                    <td>
                                        <input id="txtMondayTotalCover" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtMondayTotalCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayTotalCover" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTuesdayTotalCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayTotalCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtWednesdayTotalCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayTotalCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtThursdayTotalCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayTotalCover" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtFridayTotalCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayTotalCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtSaturdayTotalCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayTotalCover" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtSundayTotalCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalCover" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalCover" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Net Average Spend</strong>
                                    </td>
                                    <td>
                                        <input id="txtMondayAverageCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtMondayAverageCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayAverageCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtTuesdayAverageCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayAverageCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtWednesdayAverageCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayAverageCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtThursdayAverageCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayAverageCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtFridayAverageCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayAverageCoverame" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtSaturdayAverageCoverame" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayAverageCover" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtSundayAverageCover" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtAverageCover" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtAverageCover" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblFiled1" Font-Italic="True" runat="server">filed1</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField1" readonly type="text" value="-" name="txtMondayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField1" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField1" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField1" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField1" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField1" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField1" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField1" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField2" Font-Italic="True" runat="server">Field2</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField2" readonly type="text" value="-" name="txtMondayField2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField2" readonly type="text" value="-" name="txtTuesdayField2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField2" readonly type="text" value="-" name="txtWednesdayField2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField2" readonly type="text" value="-" name="txtThursdayField2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField2" readonly type="text" value="-" name="txtFridayField2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField2" readonly type="text" value="-" name="txtSaturdayField2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField2" readonly type="text" value="-" name="txtSundayField2"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField2" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField2" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField3" Font-Italic="True" runat="server">Field3</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField3" readonly type="text" value="-" name="txtMondayField3"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField3" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField3" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField3" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField3" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField3" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField3" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField3" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField4" Font-Italic="True" runat="server">Field4</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField4" readonly type="text" value="-" name="txtMondayField4"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField4" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField4" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField4" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField4" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField4" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField4" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField4" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField5" Font-Italic="True" runat="server">Field5</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField5" readonly type="text" value="-" name="txtMondayField5"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField5" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField5" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField5" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField5" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField5" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField5" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField5" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField6" Font-Italic="True" runat="server">Field6</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField6" readonly type="text" value="-" name="txtMondayField6"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField6" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField6" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField6" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField6" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField6" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField6" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField6" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField7" Font-Italic="True" runat="server">Field7</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField7" readonly type="text" value="-" name="txtMondayField7"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField7" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField7" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField7" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField7" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField7" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField7" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField7" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Total Net Sales</strong>
                                    </td>
                                    <td>
                                        <input id="txtMondayTotalNet" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtMondayTotalNet" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayTotalNet" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTuesdayTotalNet" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayTotalNet" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtWednesdayTotalNet" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayTotalNet" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtThursdayTotalNet" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayTotalNet" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtFridayTotalNet" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayTotalNet" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtSaturdayTotalNet" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayTotalNet" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtSundayTotalNet" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalTotalNet" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalTotalNet" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        VAT
                                    </td>
                                    <td>
                                        <input id="txtMondayVat" readonly type="text" value="-" name="txtMondayVat" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayVat" readonly type="text" value="-" name="txtTuesdayVat" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayVat" readonly type="text" value="-" name="txtWednesdayVat"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayVat" readonly type="text" value="-" name="txtThursdayVat" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayVat" readonly type="text" value="-" name="txtFridayVat" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayVat" readonly type="text" value="-" name="txtSaturdayVat" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayVat" readonly type="text" value="-" name="txtSundayVat" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalVat" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalTotalNet" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Total Gross Sales</strong>
                                    </td>
                                    <td>
                                        <input id="txtMondayTotalGross" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtMondayTotalGross" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayTotalGross" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTuesdayTotalGross" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayTotalGross" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtWednesdayTotalGross" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayTotalGross" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtThursdayTotalGross" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayTotalGross" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtFridayTotalGross" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayTotalGross" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtSaturdayTotalGross" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayTotalGross" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtSundayTotalGross" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalTotalGross" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalTotalNet" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField8" Font-Italic="True" runat="server">Field8</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField8" readonly type="text" value="-" name="txtMondayField8"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField8" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField8" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField8" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField8" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField8" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField8" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField8" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField9" Font-Italic="True" runat="server">Field9</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField9" readonly type="text" value="-" name="txtMondayField9"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField9" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField9" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField9" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField9" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField9" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField9" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField9" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField10" Font-Italic="True" runat="server">Field10</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField10" readonly type="text" value="-" name="txtMondayField10"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField10" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField10" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField10" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField10" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField10" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField10" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField10" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField11" Font-Italic="True" runat="server">Field11</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField11" readonly type="text" value="-" name="txtMondayField11"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField11" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField11" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField11" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField11" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField11" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField11" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField11" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField12" Font-Italic="True" runat="server">Field12</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField12" readonly type="text" value="-" name="txtMondayField12"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField12" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField12" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField12" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField12" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField12" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField12" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField12" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField13" Font-Italic="True" runat="server">Field13</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField13" readonly type="text" value="-" name="txtMondayField13"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField13" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField13" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField13" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField13" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField13" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField13" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField13" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField14" Font-Italic="True" runat="server">Field14</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField14" readonly type="text" value="-" name="txtMondayField14"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField14" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField14" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField14" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField14" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField14" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField14" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField14" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <asp:Label ID="lblField15" Font-Italic="True" runat="server">Field15</asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtMondayField15" readonly type="text" value="-" name="txtMondayField15"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayField15" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayField15" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayField15" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayField15" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayField15" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayField15" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalField15" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        Petty Cash Total
                                    </td>
                                    <td>
                                        <input id="txtMondayPettyCashTotal" readonly type="text" value="-" name="txtMondayPettyCashTotal"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayPettyCashTotal" readonly type="text" value="-" name="txtTuesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayPettyCashTotal" readonly type="text" value="-" name="txtWednesdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayPettyCashTotal" readonly type="text" value="-" name="txtThursdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayPettyCashTotal" readonly type="text" value="-" name="txtFridayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayPettyCashTotal" readonly type="text" value="-" name="txtSaturdayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayPettyCashTotal" readonly type="text" value="-" name="txtSundayField1"
                                            runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalPettyCashTotal" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>TOTAL</strong>
                                    </td>
                                    <td>
                                        <input id="txtMondayFieldTotal" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtMondayFieldTotal" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayFieldTotal" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTuesdayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayFieldTotal" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtWednesdayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayFieldTotal" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtThursdayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayFieldTotal" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtFridayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayFieldTotal" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtSaturdayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayFieldTotal" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtSundayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalFieldTotal" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        Variance
                                    </td>
                                    <td>
                                        <input id="txtMondayFieldVariance" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtMondayFieldVariance" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTuesdayFieldVariance" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtTuesdayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtWednesdayFieldVariance" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtWednesdayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtThursdayFieldVariance" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtThursdayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtFridayFieldVariance" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtFridayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSaturdayFieldVariance" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtSaturdayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtSundayFieldVariance" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtSundayField1" runat="server">
                                    </td>
                                    <td>
                                        <input id="txtTotalFieldVariance" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtTotalField1" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hed2">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="btn_col03" align="left" width="151">
                                        <strong>STOCK</strong>
                                    </td>
                                    <td align="center">
                                        Opening
                                    </td>
                                    <td align="center">
                                        Closing
                                    </td>
                                    <td align="center">
                                        Movement
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <asp:Label ID="lblStock1" Font-Italic="True" runat="server">Stock1</asp:Label>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtStock1Opening" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:sumStock1();" type="text" name="txtStock1Opening" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtStock1Closing" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:sumStock1();" type="text" name="txtStock1Closing" runat="server">
                                    </td>
                                    <td align="left">
                                        <input id="txtStock1Movement" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtStock1Movement" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <asp:Label ID="lblStock2" Font-Italic="True" runat="server">Stock2</asp:Label>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtStock2Opening" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:sumStock1();" type="text" name="txtStock2Opening" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtStock2Closing" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:sumStock1();" type="text" name="txtStock2Closing" runat="server">
                                    </td>
                                    <td align="left">
                                        <input id="txtStock2Movement" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtStock2Movement" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <asp:Label ID="lblStock3" Font-Italic="True" runat="server">Stock3</asp:Label>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtStock3Opening" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:sumStock1();" type="text" name="txtStock3Opening" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtStock3Closing" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:sumStock1();" type="text" name="txtStock3Closing" runat="server">
                                    </td>
                                    <td align="left">
                                        <input id="txtStock3Movement" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtStock3Movement" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <asp:Label ID="lblStock4" Font-Italic="True" runat="server">Stock4</asp:Label>
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtStock4Opening" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:sumStock1();" type="text" name="txtStock4Opening" runat="server">
                                    </td>
                                    <td align="left">
                                        <input class="box_input" id="txtStock4Closing" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:sumStock1();" type="text" name="txtStock4Closing" runat="server">
                                    </td>
                                    <td align="left">
                                        <input id="txtStock4Movement" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtStock4Movement" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="151">
                                        <strong>TOTAL</strong>
                                    </td>
                                    <td align="left">
                                        <input id="txtTotalOpening" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalOpening" runat="server">
                                    </td>
                                    <td align="left">
                                        <input id="txtTotalClosing" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalClosing" runat="server">
                                    </td>
                                    <td align="left">
                                        <input id="txtStockTotalMovement" style="font-weight: bold" readonly type="text"
                                            value="0" name="txtStockTotalMovement" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hed3">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="btn_col03" width="151">
                                        <strong>LABOUR</strong>
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
                                    <td width="151">
                                        <asp:Label ID="lblLabour1" Font-Italic="True" runat="server">Labour1</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtLabour1" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour1();" type="text" name="txtLabour1" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        <asp:Label ID="lblLabour4" Font-Italic="True" runat="server">Labour4</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtLabour4" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour4();" type="text" name="txtLabour4" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        National Insurance
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtNationalInsurance1" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour1();" type="text" name="txtNationalInsurance1" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        National Insurance
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtNationalInsurance4" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour4();" type="text" name="txtNationalInsurance4" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Total</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotalLabour1" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalLabour1" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        Total
                                    </td>
                                    <td>
                                        <input id="txtTotalLabour4" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalLabour4" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hed3">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="btn_col03" width="151">
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
                                    <td width="151">
                                        <asp:Label ID="lblLabour2" Font-Italic="True" runat="server">Labour2</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtLabour2" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour2();" type="text" name="txtLabour2" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        <asp:Label ID="lblLabour5" Font-Italic="True" runat="server">Labour5</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtLabour5" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour5();" type="text" name="txtLabour5" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        National Insurance
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtNationalInsurance2" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour2();" type="text" name="txtNationalInsurance2" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        National Insurance
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtNationalInsurance5" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour5();" type="text" name="txtNationalInsurance5" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Total</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotalLabour2" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalLabour2" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        Total
                                    </td>
                                    <td>
                                        <input id="txtTotalLabour5" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtTotalLabour5" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hed3">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="btn_col03" width="151">
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
                                    <td width="151">
                                        <asp:Label ID="lblLabour3" Font-Italic="True" runat="server">Labour3</asp:Label>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtLabour3" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour3();" type="text" name="txtLabour3" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        <strong>Total Labour</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotalLabour" style="font-weight: bold" readonly type="text" name="txtTotalLabour"
                                            runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        National Insurance
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtNationalInsurance3" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:Labour3();" type="text" name="txtNationalInsurance3" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        <strong>Total National Insurance</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotalNationalInsurance" style="font-weight: bold" readonly type="text"
                                            name="txtTotalNationalInsurance" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Total</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotalLabour3" style="font-weight: bold" readonly type="text" name="txtTotalLabour3"
                                            runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        <strong>Total Labour Cost</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotalLabourCost" style="font-weight: bold" readonly type="text" name="txtTotalLabourCost"
                                            runat="server">
                                    </td>
                                </tr>
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
                                        <strong>Total Labour %</strong>
                                    </td>
                                    <td>
                                        <input id="txtTotalLabourPercentage" style="font-weight: bold" readonly type="text"
                                            name="txtTotalLabourPercentage" runat="server">
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
                                        <strong>Starters in Week</strong>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtStartersinWeek" type="text" name="txtStartersinWeek"
                                            onkeydown="javascript:return ValidationForCover(this,event);;" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        <strong>Leavers in Week</strong>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtLeaversinWeek" type="text" name="txtLeaversinWeek"
                                            onkeydown="javascript:return ValidationForCover(this,event);;" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hed3">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="btn_col03" width="151">
                                        <strong>FLOAT</strong>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td class="btn_col03" align="left" width="177">
                                        <strong>SAFE</strong>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Opening</strong>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtFloatOpening" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:FloatOpening();" type="text" name="txtFloatOpening" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        <strong>Opening</strong>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtSafeOpening" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:SafeOpening();" type="text" name="txtSafeOpening" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        Increase / Decrease
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtFloatIncrease" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:FloatOpening();" type="text" name="txtFloatIncrease" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        Increase / Decrease
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtSafeIncrease" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:SafeOpening();" type="text" name="txtSafeIncrease" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Closing</strong>
                                    </td>
                                    <td>
                                        <input id="txtFloatClosing" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtFloatClosing" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        Closing
                                    </td>
                                    <td>
                                        <input id="txtSafeClosing" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtSafeClosing" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        <strong>Fixed Float</strong>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtFixedFloat" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:FloatFixed();" type="text" name="txtFixedFloat" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        <strong>Fixed Safe</strong>
                                    </td>
                                    <td>
                                        <input class="box_input" id="txtFixedSafe" onkeydown="javascript:return ValidationForTwoDecimalPoint(this,event);;"
                                            onkeyup="javascript:SafeFixed();" type="text" name="txtFixedSafe" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="151">
                                        Variance
                                    </td>
                                    <td>
                                        <input id="txtFloatVariance" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtFloatVariance" runat="server">
                                    </td>
                                    <td width="92">
                                        &nbsp;
                                    </td>
                                    <td align="left" width="177">
                                        Variance
                                    </td>
                                    <td>
                                        <input id="txtSafeVariance" style="font-weight: bold" readonly type="text" value="0"
                                            name="txtSafeVariance" runat="server">
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
                                            Saved By
                                        </td>
                                        <td>
                                            <input id="txtSavedBy" style="font-weight: bold; float: none; text-align: center"
                                                readonly type="text" name="name" runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="151">
                                            Save Date
                                        </td>
                                        <td>
                                            <input id="SaveDate" style="font-weight: bold; float: none; text-align: center" readonly
                                                type="text" name="name" runat="server">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
