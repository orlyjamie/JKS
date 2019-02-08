<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceActionTiffViewer.aspx.cs"
    Inherits="JKS.InvoiceActionTiffViewer" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Pass/Approve Invoice</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <!----LightBox------>
    <link rel="stylesheet" href="../../LightBoxScripts/style.css" />
    <script type="text/javascript" src="../../LightBoxScripts/tinybox.js"></script>
    <!----->
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet" />
    <script src="../../GoogleAjax/jquery.min.js" type="text/javascript"></script>
    <script src="../../GoogleAjax/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../../GoogleAjax/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .box
        {
            display: none;
            width: 100%;
        }
        a:hover + .box, .box:hover
        {
            display: block;
            position: absolute;
            z-index: 100;
            left: -380px;
            top: 20px;
        }
        .table1
        {
            border-collapse: collapse;
            font-family: verdana,Tahoma,Arial;
            font-size: 10px;
            text-align: center;
            width: 100%;
            border: 1px solid #ccc;
        }
        .table1 tr td
        {
            border: medium none;
            padding-bottom: 5px;
            padding-top: 5px;
        }
        .table1 th
        {
            background-color: #3399cc;
            color: #fff;
            font: bold 10px Verdana;
            text-decoration: none;
            padding: 5px;
            border: medium none;
        }
    </style>
    <!--<style type="text/css">
			.hover_box{
			
			}
			
		.hover_box .box
			{
				display: none;
				width: 500px;
			}
			
		.hover_box	a:hover + .box, .hover_box .box:hover
			{
				display: block;
				position: absolute;
				z-index: 100;
				left: -370px;
            top:-200px;
			}
			.hover_box .box iframe{
			width:500px;
			background-color:#ddd;
			padding:0;
			}
		</style>-->


        <%--Added By KD 06/12/2018 --%>
    <style type="text/css">
        body
        {
            font-family: Arial;
            
        }
       /* table
        {
            border: 1px solid #ccc;
        }
        table th
        {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
        }
        table th, table td
        {
            padding: 5px;
            border-color: #ccc;
        }*/
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border: 3px solid #0DA9D0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            text-align: center;
            padding: 5px;
        }
        .modalPopup .footer
        {
            padding: 3px;
        }
        /*.modalPopup .button
        {
            height: 23px;
            color: White;
            line-height: 23px;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
            background-color: #9F9F9F;
            border: 1px solid #5C5C5C;
        }*/
        /*added kd on 06-12-2018--%>*/
.modalPopup .button {
    color: White;
    line-height: 23px;
    text-align: center;
    font-weight: bold;
    cursor: pointer;
    background: url(../images/close.png) no-repeat;
    position: absolute;
    top: -15px;
    left: -15px;
    width: 30px;
    height: 30px;
    cursor: pointer;
    border: none;

}
  
  
  .modalPopup {
    
    width: 580px;
     height: 390px;
}      
        .modalPopup td
        {
            text-align: center;
        }
         .PopUpHeader
        {
            text-align: left !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
		function CloseWindow()
		{
			if ('<%=iCurrentStatusID%>' == '4')
			{
			alert ('Sorry, cannot manipulate this invoice, it is completed.')
			window.close();
            // closeTiffViewerWindow();
			}
		}
		function getQueryVariable(variable) 
		{
			var query = window.location.search.substring(1);
			var vars = query.split("&");
			for (var i=0;i<vars.length;i++) {
				var pair = vars[i].split("=");
				if (pair[0] == variable) {
				return pair[1];
				}
			} 
		}
		function doHourglass()
			{
				document.body.style.cursor = 'wait';				 
			}
		function windowclose()
		{          
            parent.window.close();
		}	
		
		function SubmitForm()
		{
			window.opener.document.forms[0].submit();
		}
      
		function CheckOpenValid(TypeUser)
		{	
		    var uType=TypeUser;		  
			if(document.getElementById("ddldept").options[document.getElementById("ddldept").selectedIndex].text=="Select")
			{
				alert('Please select department');
				return false;
			}
		
			if(document.getElementById("ddlApprover1").selectedIndex == 0 && document.getElementById("ddlApprover2").selectedIndex == 0 && document.getElementById("ddlApprover3").selectedIndex == 0)
			{
				//alert('Please select at least one Approvers');
                //changed on 270215
                alert('Please select at least one approver');
				return false;
			}
			
			return true;
		}
		
		function DocURLJS()
		{			
			var invID = '<%= Request.QueryString["InvoiceID"] %>';
			var iRetUrl = '../Current/CurrentStatus.aspx';
			location.href="../invoice/InvoiceFileManager_NL.aspx?From=GMGRadio&InvoiceID="+invID+"&ReturnUrl="+iRetUrl;
		}
		
		function CountLeft(field, max) 
		{
			if (field.value.length > max)
			field.value = field.value.substring(0, max);
			
		}
		
		function GoToStockQC()
		{
			var strInvoiceID='';
            //miodified by kuntalkarar on 12thJanuary2017
			strInvoiceID=<%=Session["InvoiceId_GoToStockQC"]%>;//<%=Request.QueryString["InvoiceID"]%>;
            window.open('../../JKS/StockQC/InvoiceAction.aspx?InvoiceID='+strInvoiceID,'a','width=810,height=680,scrollbars=1,resizable=1');
           // window.open('../../Communicorp/StockQC/InvoiceAction.aspx?InvoiceID='+strInvoiceID,'a','width=810,height=680,scrollbars=1,resizable=1');
		}
       function closeTiffViewerWindow()
       {
        window.open('../../TiffViewerDefault.aspx?ID=0&Type=INV','TiffViewer','width=650,height=450,top=100,left=150,scrollbars=1,resizable=1');
       }
//        function CaptureClose(sInvoiceID, sDocType) {

//            document.body.style.cursor = 'wait';
//           // var id = getQueryVariable("InvoiceID");
//           // var docType = getQueryVariable("DocType");
//           // var sValue = "id=" + id + "|docType=" + docType;
//           // var name = "Sample";
//          //  alert("Test window.opener:" + name);
//          //  name =window.parent.opener.name;
//          //  alert("window.opener:" + name);
//           //   alert("window.opener2:" + window.opener.name);
//          //  window.parent.opener.__doPostBack('btnProcess', '');
//          //  window.opener.__doPostBack('btnProcess', '');
//          //  window.top.__doPostBack('btnCloseAction', sValue);
//        }


        
		
    </script>
    <script type="text/javascript">

           // added kd for prorate issue on 28/01/2019
        function myFunction() {
    
            var x = document.getElementById("grdList_txtLineDescription_0");
            var value = x.value;
           
            if(value.length>0) {
             <%Session["lineDescription"] = "' + value + '"; %>;
            
             }
             else
             {
//             <%Session["lineDescription"] = null; %>;
              
             }
          
   
   
        }





        function getcodedetails() {
            var CodingDescriptionID;
            var DepartmentCodeID;
            var NominalCodeID;
            var ID;
            $(".input_Auto_Complete").autocomplete({

                source: function (request, response) {
                    var element = this.element;
                    ID = this.element.attr("id");
                    var split = ID.split('_');
                    var GridName = split[0];
                    var ControlName = split[1];
                    var ControlIndex = split[2];
                    var DropDownID = 'grdList_ddlBuyerCompanyCode_' + ControlIndex;
                    CodingDescriptionID = 'grdList_hdnCodingDescriptionID_' + ControlIndex;
                    DepartmentCodeID = 'grdList_hdnDepartmentCodeID_' + ControlIndex; ;
                    NominalCodeID = 'grdList_hdnNominalCodeID_' + ControlIndex;

                    var e = document.getElementById(DropDownID);
                    var strDropDownValue = e.options[e.selectedIndex].value;
                    if (strDropDownValue == "0" || strDropDownValue == "--Select--") {
                        alert("Please select Company");
                        document.getElementById(ID).value = "";
                        return;
                    }

                    $.ajax({
                        url: "InvoiceActionTiffViewer.aspx/GetCombinedDescription",
                        data: "{ 'Filter': '" + request.term + "','BuyerCompanyID': '" + strDropDownValue + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            var IsValidSource = JSON.stringify(data.d);

                            if (IsValidSource == 'null') {
                                $(".ui-autocomplete").empty();
                                $(".ui-autocomplete").hide();
                                // document.getElementById(ID).value = "";
                                // document.getElementById(CodingDescriptionID).value = "";
                                // document.getElementById(DepartmentCodeID).value = "";
                                // document.getElementById(NominalCodeID).value = "";

                            }
                            response($.map(data.d, function (item) {

                                return {
                                    label: item.split('#')[0],
                                    val: item.split('#')[1],
                                    DepartmentCodeID: item.split('#')[2],
                                    NominalCodeID: item.split('#')[3]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });

                },
                select: function (e, i) {

                    document.getElementById(CodingDescriptionID).value = i.item.val;
                    document.getElementById(DepartmentCodeID).value = i.item.DepartmentCodeID;
                    document.getElementById(NominalCodeID).value = i.item.NominalCodeID;
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        document.getElementById(ID).value = "";
                        document.getElementById(CodingDescriptionID).value = "";
                        document.getElementById(DepartmentCodeID).value = "";
                        document.getElementById(NominalCodeID).value = "";
                    }
                },

                cacheLength: 0,
                minLength: 2
            });
        }
    </script>
    <script type="text/javascript">
        function AutoCloseAlert(msg) {
            //var msg = "Line Data Save Successfully.";
            var duration = 2500;
            var el = document.createElement("div");
            el.setAttribute("style", "position:absolute;top:40%; left:30%; background-color: #eee; color: #222; font-weight: 600; border: 1px solid #ddd; fint-size: 15px; padding:20px 30px;");
            el.innerHTML = msg;
            setTimeout(function () {
                el.parentNode.removeChild(el);
            }, duration);
            document.body.appendChild(el);
        }
    </script>
    <%--Added By Rimi on 21stJuly2015 For Loader--%>
    <style type="text/css">
        .loader
        {
            background-color: transparent;
            position: fixed;
            left: 0px;
            top: 10px;
            width: 100%;
            height: 100%;
            z-index: 1000;
            background: url('../images/loader.gif') 50% 50% no-repeat;
        }
    </style>
    <%-- Added by Mainak 2018-04-06--%>
    <style type="text/css">
        .checkbox
        {
            list-style: none;
            margin-left: -30px;
        }
        .checkbox li label
        {
            margin-left: 10px;
            top: -2px;
            margin-bottom: 30px !important;
            position: relative;
        }
        .web_dialog_overlay
        {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101;
            display: none;
        }
        .web_dialog
        {
            display: none;
            position: fixed;
            width: 250px;
            height: 65%;
            top: 30%;
            left: 50%;
            margin-left: -190px;
            margin-top: -100px;
            background-color: #ffffff;
            border: 2px solid #336699;
            padding: 0px;
            z-index: 102; /*font-family: Verdana;
            font-size: 10pt;*/
        }
        .web_dialog_title
        {
            border-bottom: solid 2px #336699;
            background-color: #336699;
            padding: 4px;
            color: White;
            font-weight: bold;
            font-size: 10pt;
        }
        .web_dialog_title a
        {
            color: White;
            text-decoration: none;
        }
        .align_right
        {
            text-align: right;
        }
        
        .linkBtn
        {
            overflow: visible; /* Shrinkwrap the text in IE7- */
            margin: 0;
            padding: 0;
            border: 0;
            color: #3399cc; /*Match your link colour */
            background: transparent;
            font: inherit; /* Inherit font settings (doesn’t work in IE7-) */
            line-height: normal; /* Override line-height to avoid spacing issues */
            text-decoration: underline; /* Make it look linky */
        }
        
        /*For Release button Added by Mainak 2018-08-16*/
        .btnReleaseOk
        {
            color: #fff;
            background-color: #5bc0de;
            border-color: #46b8da;
        }
        
        .btnNormal
        {
            height: 25px;
            display: inline-block;
            padding: 2px 12px;
            margin-bottom: 0;
            font-size: 11px;
            font-weight: 400;
            line-height: 1.42857143;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
        }
        
        .web_dialog_overlay_Release
        {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101; /*display: none;*/
        }
        .web_dialog_Release
        {
            /*display: none;*/
            position: fixed;
            width: 300px;
            height: 18%;
            top: 14%;
            left: 50%;
            margin-left: -190px;
            margin-top: -100px;
            background-color: #ffffff;
            padding: 0px;
            z-index: 102; /* border: 2px solid #336699;
             font-family: Verdana;
            font-size: 10pt;*/
        }
        .web_dialog_title_Release
        {
            /*border-bottom: solid 2px #336699;
            background-color: #336699;
            font-weight: bold;*/
            font-weight: bold;
            padding: 0px;
            color: #848484;
            font-size: 11px;
        }
        .web_dialog_title_Release a
        {
            color: White;
            text-decoration: none;
        }
        .align_right
        {
            text-align: right;
        }
        
        
        /*Added by Mainak 2018-09-08*/
        
        /*For Release button Added by Mainak 2018-08-16*/
        .btnApproveOk
        {
            color: #fff;
            background-color: #5bc0de;
            border-color: #46b8da;
        }
        
        .btnNormal
        {
            height: 25px;
            display: inline-block;
            padding: 2px 12px;
            margin-bottom: 0;
            font-size: 11px;
            font-weight: 400;
            line-height: 1.42857143;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
        }
        
        .web_dialog_overlay_Approve
        {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101; /*display: none;*/
        }
        .web_dialog_Approve
        {
            /*display: none;*/
            position: fixed;
            width: 300px;
            height: 18%;
            top: 14%;
            left: 50%;
            margin-left: -190px;
            margin-top: -100px;
            background-color: #ffffff;
            padding: 0px;
            z-index: 102; /* border: 2px solid #336699;
             font-family: Verdana;
            font-size: 10pt;*/
        }
        .web_dialog_title_Approve
        {
            /*border-bottom: solid 2px #336699;
            background-color: #336699;
            font-weight: bold;*/
            font-weight: bold;
            padding: 0px;
            color: #848484;
            font-size: 11px;
        }
        .web_dialog_title_Approve a
        {
            color: White;
            text-decoration: none;
        }
        .align_right
        {
            text-align: right;
        }
         .PopUpHeader
        {
            text-align: left !important;
        }
    </style>
    <%-- Ended by Mainak 2018-04-06--%>
    <%-- <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>--%>
    <script type="text/javascript">

        $(window).load(function () {
            $(".loader").fadeOut("slow");

            $("#btnCancel").click(function () {
                setTimeout(function () {
                    $(button).attr('disabled', 'disabled');
                }, 1);

                // $('#btnCancel').prop("disabled", true);

                $(".loader").fadeOut("slow");
                $(".loader").fadeIn("slow");
                // $('#btnCancel').prop("disabled", false);

            })

            $("#btnApprove").click(function () {

                // $('#btnApprove').prop("disabled", true);

                setTimeout(function () {
                    $(button).attr('disabled', 'disabled');
                }, 1);

                $(".loader").fadeOut("slow");
                $(".loader").fadeIn("slow");
                // $('#btnApprove').prop("disabled", false);

            })

            $("#btndelete").click(function () {

                //$('#btndelete').prop("disabled", true);
                setTimeout(function () {
                    $(button).attr('disabled', 'disabled');
                }, 1);
                $(".loader").fadeOut("slow");
                $(".loader").fadeIn("slow");
                // $('#btndelete').prop("disabled", false);

            })

            $("#btnReject").click(function () {

                //$('#btnReject').prop("disabled", true);
                setTimeout(function () {
                    $(button).attr('disabled', 'disabled');
                }, 1);
                $(".loader").fadeOut("slow");
                $(".loader").fadeIn("slow");
                // $('#btnReject').prop("disabled", false);


            })

            $("#btnOpen").click(function () {

                // $('#btnOpen').prop("disabled", true);
                setTimeout(function () {
                    $(button).attr('disabled', 'disabled');
                }, 1);
                $(".loader").fadeOut("slow");
                $(".loader").fadeIn("slow");
                //$('#btnOpen').prop("disabled", false);

            })


            $("#btnReopen").click(function () {

                //$('#btnReopen').prop("disabled", true);
                setTimeout(function () {
                    $(button).attr('disabled', 'disabled');
                }, 1);
                $(".loader").fadeOut("slow");
                $(".loader").fadeIn("slow");
                //$('#btnReopen').prop("disabled", false);

            })

        })

       
    </script>
    <%--Added By Rimi on 21stJuly2015 For Loader End--%>
</head>
<body onbeforeunload="doHourglass();" bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <script language="javascript" src="../Supplier/wz_tooltip.js"></script>
    <%--Added By Rimi on 21stJuly2015 For Loader --%>
    <div class="loader">
        Loading,please wait .......</div>
    <%--Added By Rimi on 21stJuly2015 For Loader End--%>
    <form id="Form2" style="z-index: 102; left: 0px"  method="post"
    runat="server">
    <%-- autocomplete="off" ,Added by Mainak 2018-09-22--%>
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
       <input autocomplete="off" name="hidden" type="text" style="display: none;" />
        <%--Added by Mainak 2018-09-22--%>
        <asp:HiddenField ID="hdnVRflag" runat="server" />
        <tr>
            <td style="width: 57%" valign="top">
                <iframe runat="server" id="TiffWindow" width="100%" height="786px" name="menu" scrolling="yes"
                    frameborder="0" marginheight="0px" marginwidth="0px"></iframe>
            </td>
            <td style="width: 43%" valign="top">
                <asp:UpdatePanel ID="up1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td valign="top">
                                    <!-- Main Content Panel Starts-->
                                    <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td class="PageHeaderAction" style="height: 21px">
                                                <div class="tophead">
                                                    <asp:Label ID="lblConfirmation" runat="server" Visible="True" CssClass="PageHeaderAction">Invoice Workflow</asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%">
                                                <table class="tlbborder" style="width: 100%; height: 64px">
                                                    <tr class="NewBoldText">
                                                        <td class="NormalBody">
                                                            <b>Document No</b>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody"></asp:Label>
                                                        </td>
                                                        <td class="NormalBody">
                                                            <b>Current Status</b>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCurrentStatus" runat="server" CssClass="NormalBody" Font-Bold="True"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <!--<td style="width: 43px">
                                    </td>-->
                                                        <td class="NormalBody">
                                                            <b>Document Date</b>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblInvoiceDate" runat="server" CssClass="NormalBody"></asp:Label>
                                                        </td>
                                                        <td class="NormalBody">
                                                            <b>Business Unit</b>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblBusinessUnit" runat="server" CssClass="NormalBody" Style="cursor: hand"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="NormalBody">
                                                            <b>Supplier Name</b>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSupplier" runat="server" CssClass="NormalBody"></asp:Label>
                                                        </td>
                                                        <%--modified by subhrajyoti on 27.03.2015--%>
                                                        <%-- <% if (Convert.ToInt32(Session["UserTypeID"]) != 1)
                                       {%>--%>
                                                        <td class="NormalBody">
                                                            <b>Department</b>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDepartment" runat="server" CssClass="NormalBody"></asp:Label>
                                                        </td>
                                                        <%--<% } %>--%>
                                                        <%----------------------------------------------------%>
                                                    </tr>
                                                    <tr>
                                                        <td class="NormalBody">
                                                            <b>Buyer Name</b>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblBuyer" runat="server" CssClass="NormalBody"></asp:Label>
                                                        </td>
                                                        <td class="NormalBody">
                                                            <b>
                                                                <asp:Label ID="lblCRn" runat="server" CssClass="NormalBody" Width="104px">Credit Note No</asp:Label>
                                                            </b>
                                                        </td>
                                                        <td class="NormalBody">
                                                            <asp:Label ID="lblcreditnoteno" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
                                                            <asp:LinkButton ID="lnkCrn" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:LinkButton>
                                                            <span style="color: red;">
                                                                <%=GetCreditLinks()%>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 43px; display: none">
                                                            <asp:LinkButton ID="Linkbutton1" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:LinkButton>
                                                        </td>
                                                        <td class="NormalBody">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="NormalBody">
                                                        </td>
                                                        <td class="NormalBody">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="NormalBody" style="height: 3px">
                                                <asp:Label ID="lblApprovelMessage" runat="server" Visible="False" CssClass="NormalBody"
                                                    Font-Bold="True" Width="136px" ForeColor="Red">Approval Completed.</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--work-->
                                            <td class="NormalBody" valign="top" height="100%">
                                                <%--   blocked by subhrajyoti on 13thjune2015--%>
                                                <asp:Repeater runat="server" ID="grdList" OnItemCommand="grdList_ItemCommand" OnItemCreated="grdList_ItemCreated">
                                                    <%--OnItemCreated="grdList_ItemCreted"--%>
                                                    <HeaderTemplate>
                                                        <table class="head-box" width="100%" border="0" cellpadding="0" cellspacing="0">
                                                            <thead>
                                                                <tr>
                                                                    <th align="center" width="13%" colspan="1">
                                                                        Line No.
                                                                    </th>
                                                                    <th align="center" width="29%" colspan="1">
                                                                        Company
                                                                    </th>
                                                                    <th align="center" width="29%" colspan="1">
                                                                        <%--Modified by Mainak 2018-08-08--%>
                                                                        <%-- Business Unit--%>
                                                                        Internal Order
                                                                    </th>
                                                                    <th align="center" width="15%" colspan="1">
                                                                        PO No.
                                                                    </th>
                                                                    <th align="center" width="15%" colspan="1">
                                                                        Delete Lines
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <div style="border: 1px solid #ccc; width: 99.6%;">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table class="tablebox" width="100%" border="0" cellpadding="0" cellspacing="0">
                                                            <tbody class="custom_listingArea">
                                                                <tr class="tableHd rowset">
                                                                    <td width="10%" colspan="1" style="text-align: center;">
                                                                        <asp:Label ID="lblLineNo" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td width="30%" colspan="1">
                                                                        <asp:DropDownList ID="ddlBuyerCompanyCode" AutoPostBack="True" CssClass="textfields-option"
                                                                            OnSelectedIndexChanged="SelectedIndexChanged_ddlBuyerCompanyCode" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="30%" colspan="1">
                                                                        <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="textfields-option">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="15%" colspan="1">
                                                                        <asp:TextBox ID="txtPoNumber" CssClass="textfields" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'
                                                                            runat="server">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td width="15%" colspan="1" align="center" style="text-align: center;">
                                                                        <asp:CheckBox ID="chkBox" runat="server"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="rowset">
                                                                    <td align="center" width="10%">
                                                                        <span class="coding">Coding:</span>
                                                                    </td>
                                                                    <td align="left" width="60%" colspan="2">
                                                                        <asp:TextBox ID="txtAutoCompleteCodingDescription" autocomplete="off"  CssClass="input_Auto_Complete textfields-option"
                                                                            runat="server" Width="99.5%" Text='<%# DataBinder.Eval(Container, "DataItem.CodingDescription") %>'
                                                                            onkeypress=" getcodedetails()" onpaste="return false" oncontextmenu="return false"> 
                                                                        </asp:TextBox><%-- Modified by Mainak 2018-04-17--%>
                                                                        <asp:HiddenField ID="hdnCodingDescriptionID" runat="server" />
                                                                        <asp:HiddenField ID="hdnDepartmentCodeID" runat="server" />
                                                                        <asp:HiddenField ID="hdnNominalCodeID" runat="server" />
                                                                    </td>
                                                                    <td align="center" width="15%" colspan="1" class="value" style="padding-left: 10px;">
                                                                        Net:
                                                                    </td>
                                                                    <td align="center" width="15%" colspan="1">
                                                                        <asp:TextBox ID="txtNetVal" CssClass="textfields-option align-right" Text='<%# GetDecimalFormattedValue(DataBinder.Eval(Container, "DataItem.NetValue")) %>'
                                                                            runat="server" OnTextChanged="txtNetVal_TextChanged" AutoPostBack="true" ToolTip="Please click out of the field after editing the Net value in order to update variances, before editing another field">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <!---Addition Start on 13th March 2015--->
                                                                <tr class="rowset">
                                                                    <td align="center" width="10%">
                                                                        <span class="coding">Description:</span>
                                                                    </td>
                                                                    <td align="left" width="60%" colspan="2">
																<!---	onkeyup="myFunction()" added by kd for prorate issue on 29jan 2019 --->
                                                                        <asp:TextBox ID="txtLineDescription" onkeyup="myFunction()"  CssClass="textfields-option" runat="server"
                                                                            Width="99.5%">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td align="center" width="15%" colspan="1" class="value" style="padding-left: 10px;">
                                                                        VAT:
                                                                    </td>
                                                                    <td align="center" width="15%" colspan="1">
                                                                        <asp:TextBox ID="txtLineVAT" CssClass="textfields-option align-right" runat="server"
                                                                            ToolTip="Please click out of the field after editing the VAT value in order to update variances, before editing another field"
                                                                            OnTextChanged="txtLineVAT_TextChanged" AutoPostBack="true" Text='<%# GetDecimalFormattedValue(DataBinder.Eval(Container, "DataItem.VAT")) %>'>
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <table class="tablebox colored" width="100%" border="0" cellpadding="0" cellspacing="0">
                                                            <tbody class="custom_listingArea">
                                                                <tr class="tableHd rowset">
                                                                    <td align="center" width="10%" style="text-align: center;">
                                                                        <asp:Label ID="lblLineNo" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td align="center" width="30%">
                                                                        <asp:DropDownList ID="ddlBuyerCompanyCode" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged_ddlBuyerCompanyCode"
                                                                            runat="server" CssClass="textfields-option">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="center" width="30%">
                                                                        <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="textfields-option">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="center" width="15%">
                                                                        <asp:TextBox ID="txtPoNumber" CssClass="textfields" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'
                                                                            runat="server">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td align="center" width="15%" style="text-align: center;">
                                                                        <asp:CheckBox ID="chkBox" runat="server"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="rowset">
                                                                    <td align="center" width="10%">
                                                                        <span class="coding">Coding:</span>
                                                                    </td>
                                                                    <td align="left" width="60%" colspan="2">
                                                                        <asp:TextBox ID="txtAutoCompleteCodingDescription"  CssClass="input_Auto_Complete textfields-option"
                                                                            runat="server" Width="99.5%" Text='<%# DataBinder.Eval(Container, "DataItem.CodingDescription") %>'
                                                                            onkeypress=" getcodedetails()" onpaste="return false" oncontextmenu="return false"> 
                                                                        </asp:TextBox><%-- Modified by Mainak 2018-04-17--%>
                                                                        <asp:HiddenField ID="hdnCodingDescriptionID" runat="server" />
                                                                        <asp:HiddenField ID="hdnDepartmentCodeID" runat="server" />
                                                                        <asp:HiddenField ID="hdnNominalCodeID" runat="server" />
                                                                    </td>
                                                                    <td align="center" width="15%" colspan="1" class="value" style="padding-left: 10px;">
                                                                        Net:
                                                                    </td>
                                                                    <td align="center" width="15%" colspan="1">
                                                                        <asp:TextBox ID="txtNetVal" CssClass="textfields-option align-right" Text='<%# GetDecimalFormattedValue(DataBinder.Eval(Container, "DataItem.NetValue")) %>'
                                                                            runat="server" OnTextChanged="txtNetVal_TextChanged" AutoPostBack="true" ToolTip="Please click out of the field after editing the Net value in order to update variances, before editing another field">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <!---Addition Start on 13th March 2015--->
                                                                <tr class="rowset">
                                                                    <td align="center" width="10%">
                                                                        <span class="coding">Description:</span>
                                                                    </td>
                                                                    <td align="left" width="60%" colspan="2">
                                                                        <asp:TextBox ID="txtLineDescription"  CssClass="textfields-option" runat="server" 
                                                                            Width="99.5%">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td align="center" width="15%" colspan="1" class="value" style="padding-left: 10px;">
                                                                        VAT:
                                                                    </td>
                                                                    <td align="center" width="15%" colspan="1">
                                                                        <asp:TextBox ID="txtLineVAT" CssClass="textfields-option align-right" runat="server"
                                                                            ToolTip="Please click out of the field after editing the VAT value in order to update variances, before editing another field"
                                                                            OnTextChanged="txtLineVAT_TextChanged" AutoPostBack="true" Text='<%# GetDecimalFormattedValue(DataBinder.Eval(Container, "DataItem.VAT")) %>'>
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <!-- Addition End on 13th March 2015-->
                                                            </tbody>
                                                        </table>
                                                    </AlternatingItemTemplate>
                                                    <FooterTemplate>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <thead>
                                                        <tr>
                                                            <%--------modified by kuntal karar on 23rd MAR2015----pt.49(a)-------------%>
                                                            <td colspan="1" style="padding: 3px 2px;" nowrap>
                                                                <%--class="bottomBdr"--%>
                                                                <asp:Label ID="Label2" runat="server" CssClass="NormalBody"><b>Invoice Net:</b></asp:Label>
                                                            </td>
                                                            <td colspan="1" nowrap="nowrap" class="colored" style="text-align: right;">
                                                                <%--class="colored bottomBdr"--%>
                                                                <asp:Label ID="lblNetInvoiceTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                                            </td>
                                                            <td width="25%;" style="padding: 3px 2px; text-align: right;" nowrap>
                                                                <%--class="bottomBdr"--%>
                                                                <asp:Label ID="Label1" runat="server" CssClass="NormalBody"><b>Total Coding Net:</b></asp:Label>
                                                            </td>
                                                            <td colspan="1" nowrap="nowrap" style="text-align: center;">
                                                                <%--class="bottomBdr"--%>
                                                                <asp:Label ID="lblNetVal" runat="server" CssClass="NormalBody"></asp:Label>
                                                            </td>
                                                            <td width="25%;" style="padding: 3px 55px 3px 0; text-align: right" nowrap>
                                                                <%--class="bottomBdr"--%>
                                                                <asp:Label ID="lblVarianceHeading" runat="server" CssClass="NormalBody"><b>Net Variance:</b></asp:Label>
                                                            </td>
                                                            <td colspan="1" nowrap="nowrap" style="text-align: right; padding-right: 6px;">
                                                                <asp:Label ID="lblVariance" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                                            </td>
                                                            <%--class="bottomBdr"--%>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="1" class="noBorder" style="padding: 3px 2px;" nowrap>
                                                                <asp:Label ID="lblVatHeading" runat="server" CssClass="NormalBody"><b>VAT:</b></asp:Label>
                                                            </td>
                                                            <td colspan="1" nowrap="nowrap" class="colored bottomBdr" style="text-align: right;">
                                                                <asp:Label ID="lblVat" runat="server" CssClass="NormalBody"></asp:Label>
                                                            </td>
                                                            <!--Added by Mrinal on 13th March 2015---->
                                                            <td width="25%;" style="padding: 3px 2px; text-align: right;" nowrap>
                                                                <%--class="bottomBdr"--%>
                                                                <asp:Label ID="lblTotalCodingVAT" runat="server" CssClass="NormalBody"><b>Total Coding VAT:</b></asp:Label>
                                                            </td>
                                                            <td colspan="1" nowrap="nowrap" style="text-align: center;">
                                                                <%--class="bottomBdr"--%>
                                                                <asp:Label ID="lblTotalCodingVATValue" runat="server" CssClass="NormalBody"></asp:Label>
                                                            </td>
                                                            <td width="25%;" style="padding: 3px 55px 3px 0; text-align: right" nowrap>
                                                                <%--class="bottomBdr"--%>
                                                                <asp:Label ID="lblVATVarianceHeading" runat="server" CssClass="NormalBody"><b>VAT Variance:</b></asp:Label>
                                                            </td>
                                                            <td colspan="1" nowrap="nowrap" style="text-align: right; padding-right: 6px;">
                                                                <%--class="bottomBdr"--%>
                                                                <asp:Label ID="lblVATVariance" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:Label>
                                                            </td>
                                                            <!---Addition End----->
                                                            <!-----------------------Modification End--------------------------------->
                                                        </tr>
                                                        <tr>
                                                            <td class="noBorder" style="padding: 3px 2px;" nowrap>
                                                                <asp:Label ID="lblTotalHeading" runat="server" CssClass="NormalBody"><b>Total:</b></asp:Label>
                                                            </td>
                                                            <td colspan="1" nowrap="nowrap" class="colored" style="text-align: right;">
                                                                <%--class="colored bottomBdr" --%>
                                                                <asp:Label ID="lblTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                                            </td>
                                                            <td colspan="1" align="left">
                                                                <asp:Label ID="lblCurrencyCode" runat="server" CssClass="NormalBody gbp"></asp:Label>
                                                            </td>
                                                            <td colspan="1" align="left">
                                                                <asp:Label ID="lblDuplicate" runat="server" CssClass="dupval" Text="DUPLICATE" ForeColor="Red"></asp:Label>
                                                            </td>
                                                            <td colspan="2" align="left" style="text-align: right; position: relative">

                                                           <%-- Blocked By kd--%> 
                                                               <%-- <a id="aInvoiceStatusLog" href='#' runat="server" style="font-size: 11px; font-weight: 600;
                                                                    padding: 0 8px; color: #3399cc;" class="st-hist">Status History</a>--%>

                                                                     <asp:LinkButton ID="aStatusHistory" OnClick="Popup_Click"  runat="server" style="font-size: 11px; font-weight: 600;
                                                                    padding: 0 8px; color: #3399cc;" class="st-hist"><b>Status History</b></asp:LinkButton>
                                                                <%--<div class="box">
                                                <iframe id="iframeInvoiceStatusLog" runat="server" src=""  style="width: 545px; height: 350px;margin-left:10px;"
                                                    scrolling="No"></iframe>
                                            </div>--%>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <!--Work End-->
                                        </tr>
                                        <tr height="10">
                                            <td style="height: 118px">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" height="40px;">
                                                    <tr>
                                                        <td style="text-align: center;" align="left" colspan="5">
                                                            <asp:Label ID="lblmessage" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px" align="left" colspan="1">
                                                            <a id="aEditData" href='#' runat="server" style="font-size: 11px; font-weight: 600;
                                                                padding: 0 8px; color: #3399cc;" target="_blank">Edit Data</a>
                                                        </td>
                                                        <td style="width: 55px;" align="center" colspan="1">
                                                            <%-- Modified by Mainak 2018-04-06--%>
                                                            <asp:Button Text="Prorate" runat="server" ID="btnShowModal" class="linkBtn" Style="font-size: 11px;
                                                                font-weight: 600; color: #3399cc; margin-top: 17px;" CausesValidation="False" OnClientClick="ShowDialog1(true);return false;" />
																 <%-- OnClientClick added by kd on 21-01-2019 for prorate --%>
                                                            <%-- <asp:Button Text="Sample Dialog" runat="server" ID="btnShowSimple" Visible="false" />--%>
                                                            <br />
                                                            <br />
                                                            <%--<div id="output">
                                                            </div>--%>
                                                            <div id="overlay" class="web_dialog_overlay">
                                                            </div>
                                                            <div id="dialog" class="web_dialog" style="overflow: scroll;">
                                                                <table style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                                                                    <tr>
                                                                        <td class="web_dialog_title" style="font-family: verdana,Tahoma,Arial !important;
                                                                            font-size: 10pt !important;">
                                                                            Prorate Coding
                                                                        </td>
                                                                        <td class="web_dialog_title align_right" style="font-family: verdana,Tahoma,Arial !important;
                                                                            font-size: 10pt !important;">
                                                                            <a href="#" id="btnClose" onclick="HideDialog()" >Close</a> <%-- onclick added by kd on 21-01-2019 for prorate --%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" style="padding-left: 13px; font-family: verdana,Tahoma,Arial !important;
                                                                            font-size: 11px !important;">
                                                                            &nbsp;<b>Select Department </b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="chkDepartment" class="checkbox" runat="server" DataValueField="DepartmentID"
                                                                                DataTextField="Department" RepeatLayout="OrderedList" Style="font-family: verdana,Tahoma,Arial !important;
                                                                                font-size: 11px !important;">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;&nbsp;<input type="checkbox" id="select_all" style="font-family: verdana,Tahoma,Arial !important;
                                                                                font-size: 11px !important;" />&nbsp; <span style="font-family: verdana,Tahoma,Arial !important;
                                                                                    font-size: 11px !important; top: -2px;">Select All</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: center;">
                                                                            <asp:Button Text="Submit" runat="server" ID="btnProrateSubmit" CssClass="allbtn_ActionWindow"
                                                                                BorderWidth="0px" BorderStyle="None" CausesValidation="False" OnClick="btnProrateSubmit_Click" />
                                                                        </td>
                                                                        <%--<td style="text-align: center;">
                                                                            <asp:Button Text="Cancel" runat="server" ID="btnClose" />
                                                                        </td>--%>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                        <td style="width: 55px" align="right" colspan="1">
                                                            <asp:TextBox Style="display: none" ID="txtNew" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 150px" align="center" colspan="1">
                                                            <asp:Button ID="btnAddNew" runat="server" CssClass="allbtn_ActionWindow" BorderWidth="0px"
                                                                BorderStyle="None" CausesValidation="False" Text="Add New Line"></asp:Button>
                                                        </td>
                                                        <td style="width: 138px">
                                                            <asp:Button ID="btnDelLine" runat="server" CssClass="allbtn_ActionWindow" BorderWidth="0px"
                                                                BorderStyle="None" CausesValidation="False" Text="Delete Line(s)"></asp:Button>
                                                        </td>
                                                        <td style="width: 138px">
                                                            <asp:Button ID="btnSaveLine" runat="server" CssClass="allbtn_ActionWindow" BorderWidth="0px"
                                                                BorderStyle="None" Style="float: right;" CausesValidation="False" Text="Save">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--------------Modified by Kuntal Karar--on 23rdMAR2015---pt.49(b)---------%>
                                                <div style="width: 100%;">
                                                    <%--border: 1px solid;--%>
                                                    <%-------------------------------------------------------------------------------------%>
                                                    <table class="tlbborder" style="width: 100%; height: 253px">
                                                        <% if (TypeUser > 1)
                                                           {%>
                                                        <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                                           {%>
                                                        <tr>
                                                            <td style="height: 4px; width: 20%" align="left">
                                                                <b class="NormalBody">Department:</b>
                                                            </td>
                                                            <td style="height: 4px" align="left">
                                                                <asp:DropDownList ID="ddldept" runat="server" CssClass="MyInput txtselectbar" AutoPostBack="True"
                                                                    DataValueField="DepartmentID" DataTextField="Department" Style="width: 80px;">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="height: 4px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 13px" align="left">
                                                                <b class="NormalBody">Approval Path:</b>
                                                            </td>
                                                            <%--modified by rimi on 12thJjune2015--%>
                                                            <td style="height: 13px" align="left" colspan="2">
                                                                <asp:DropDownList ID="ddlApprover1" runat="server" CssClass="txtselectbar" DataValueField="GroupName"
                                                                    DataTextField="GroupName" Style="width: 90px;">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlApprover2" runat="server" CssClass="txtselectbar" DataValueField="GroupName"
                                                                    DataTextField="GroupName" Style="width: 90px;">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlApprover3" Visible="False" runat="server" CssClass="txtselectbar"
                                                                    DataValueField="GroupName" DataTextField="GroupName" Style="width: 90px;">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlApprover4" Visible="False" runat="server" CssClass="txtselectbar"
                                                                    DataValueField="GroupName" DataTextField="GroupName" Style="width: 100px;">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="height: 13px">
                                                                <asp:DropDownList ID="ddlApprover5" Visible="False" runat="server" DataValueField="GroupName"
                                                                    DataTextField="GroupName" Style="display: none; width: 90px;">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <%------------------------------------------%>
                                                        </tr>
                                                        <% }%>
                                                        <% }%>
                                                        <tr>
                                                            <td style="height: 45px" align="left">
                                                                <b class="NormalBody">Comments:</b>
                                                            </td>
                                                            <td style="height: 45px" align="left">
                                                                <asp:TextBox ID="txtComment" CssClass="txt_area" runat="server" Height="35px" TextMode="MultiLine"
                                                                    MaxLength="200" Style="height: 38px; border: 1px solid #ccc;"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 138px; height: 45px">
                                                            </td>
                                                        </tr>
                                                        <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                                           {%>
                                                        <%
                                                               if (RejectOpenFields == 0)
                                                               {
                                                        %>
                                                        <%-------Blocked by kuntal karar on 15thMarch2016-------%>
                                                        <%--- <tr>
                                        
                                        <td align="left">
                                            
                                            <b class="NormalBody">Rejection Code:</b>
                                        </td>
                                        <td align="left">
                                           
                                            <asp:DropDownList ID="ddlRejection" runat="server" CssClass="txtselectbar" DataValueField="RejectionCodeID"
                                                DataTextField="RejectionCode" Font-Size="XX-Small" Style="font-family: verdana,Tahoma,Arial !important;
                                                font-size: 11px !important; color: #848484; width: 74.5%">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 138px;">
                                           
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td style="height: 42px" align="left">
                                            <b class="NormalBody"><b class="NormalBody">Rejection </b>Comments:</b>
                                        </td>
                                        <td style="width: 526px; height: 42px" align="left">
                                            <asp:TextBox ID="tbRejection" CssClass="txt_area" runat="server" Height="34px" TextMode="MultiLine"
                                                MaxLength="200"></asp:TextBox>
                                        </td>
                                        <td style="width: 138px; height: 42px">
                                        </td>
                                    </tr>
                                    ---%>
                                                        <% } %>
                                                        <%
                                                               if (RejectOpenFields == 1 || ReopenAtApprover == 1 || ((Convert.ToInt32(ViewState["StatusID"]) == 21 || Convert.ToInt32(ViewState["StatusID"]) == 22) & TypeUser > 1))
                                                               {
                                                        %>
                                                        <%-------Blocked by kuntal karar on 15thMarch2016-------%>
                                                        <%---
                                    <tr>
                                        <td align="left" valign="top">
                                            <b class="NormalBody"><b class="NormalBody">Credit Note No</b>:</b>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtCreditNoteNo" CssClass="txt_area" runat="server" Height="20px"
                                                TextMode="SingleLine"></asp:TextBox>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblTextReopen" runat="server" CssClass="NormalBody" Font-Bold="True"
                                                Width="152px" ForeColor="Red">Reopen at Approver 1 :</asp:Label>
                                            <asp:CheckBox ID="chbOpen" runat="server" CssClass="NormalBody"></asp:CheckBox>
                                        </td>
                                        <td style="width: 138px">
                                        </td>
                                    </tr>
                                                        --%>
                                                        <% } %>
                                                        <%-------Added by kuntal karar on 15thMarch2016-------%>
                                                        <tr id="TR_RejectionCode" runat="server" visible="false">
                                                            <td align="left">
                                                                <%--style="height: 30px"--%>
                                                                <b class="NormalBody">Rejection Code:</b>
                                                            </td>
                                                            <td align="left">
                                                                <%--style="height: 30px"--%>
                                                                <asp:DropDownList ID="ddlRejection" runat="server" CssClass="txtselectbar" DataValueField="RejectionCodeID"
                                                                    DataTextField="RejectionCode" Font-Size="XX-Small" Style="font-family: verdana,Tahoma,Arial !important;
                                                                    font-size: 11px !important; color: #848484; width: 74.5%">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 138px;" align="left">
                                                                <%--height: 30px--%>
                                                            </td>
                                                            <%--------------------------------------------------------------------------%>
                                                        </tr>
                                                        <tr id="TR_RejectionComments" runat="server" visible="false">
                                                            <td style="height: 42px" align="left">
                                                                <b class="NormalBody"><b class="NormalBody">Rejection </b>Comments:</b>
                                                            </td>
                                                            <td style="width: 526px; height: 42px" align="left">
                                                                <asp:TextBox ID="tbRejection" CssClass="txt_area" runat="server" Height="34px" TextMode="MultiLine"
                                                                    MaxLength="200"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 138px; height: 42px">
                                                            </td>
                                                        </tr>
                                                        <tr id="TR_CreditNoteNo" runat="server" visible="false">
                                                            <td align="left" valign="top">
                                                                <b class="NormalBody"><b class="NormalBody">Credit Note No</b>:</b>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtCreditNoteNo" CssClass="txt_area" runat="server" Height="20px"
                                                                    TextMode="SingleLine"></asp:TextBox>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lblTextReopen" runat="server" CssClass="NormalBody" Font-Bold="True"
                                                                    Width="152px" ForeColor="Red">Reopen at Approver 1 :</asp:Label>
                                                                <asp:CheckBox ID="chbOpen" runat="server" CssClass="NormalBody"></asp:CheckBox>
                                                            </td>
                                                            <td style="width: 138px">
                                                            </td>
                                                        </tr>
                                                        <%-------Addition Ends by kuntal karar on 15thMarch2016-------%>
                                                        <% } %>
                                                        <%--  <tr>
                                        <td align="right">
                                        </td>
                                        <td align="left">
                                            <span class="NormalBody" style="color: #ff0000"></span>
                                        </td>
                                        <td style="width: 138px">
                                        </td>
                                    </tr>--%>
                                                        <tr>
                                                            <td colspan="3" style="text-align: center">
                                                                <asp:Button ID="btnCancel" runat="server" CssClass="allbtn_ActionWindow" BorderWidth="0px"
                                                                    BorderStyle="None" CausesValidation="False" Text="Cancel"></asp:Button>
                                                                <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                                                   {%>
                                                                <asp:Button ID="btnReject" CssClass="btnRejected_ActionWindow" BorderStyle="None"
                                                                    CausesValidation="False" Text="Reject" runat="server" ToolTip="Reject"></asp:Button>
                                                                <asp:Button ID="btnReopen" CssClass="allbtn_ActionWindow" BorderStyle="None" CausesValidation="False"
                                                                    Text="Reopen" runat="server" ToolTip="Reopen"></asp:Button>
                                                                <% } %>
                                                                <asp:Button ID="btnApprove" CssClass="btnApprove_ActionWindow" BorderStyle="None"
                                                                    CausesValidation="False" Text="Approve" runat="server" ToolTip="Approve"></asp:Button>
                                                                <%--Added By Mainak 2018-09-11--%>
                                                                <div id="overlayApprove" class="web_dialog_overlay_Approve" runat="server">
                                                                </div>
                                                                <div id="dialogApprove" class="web_dialog_Approve" runat="server">
                                                                    <table style="width: 95%; border: 0px; text-align: left; margin-left: 10px; margin-right: 10px;"
                                                                        cellpadding="3" cellspacing="0">
                                                                        <tr>
                                                                            <td class="web_dialog_title_Approve">
                                                                                <br />
                                                                                Confirmation
                                                                            </td>
                                                                            <td class="web_dialog_title_Approve align_right">
                                                                                <a href="#" id="A1"></a>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="font-size: 11px;">
                                                                                <br />
                                                                                Net has been entered as zero in the coding lines. Are you sure you wish to continue?
                                                                                <br />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right;">
                                                                            </td>
                                                                            <td style="text-align: center;">
                                                                                <asp:Button Text="OK" runat="server" ID="btnApproveOk" class="btnNormal btnReleaseOk"
                                                                                    CausesValidation="False" OnClick="btnApproveOk_Click" Width="70px" />
                                                                                <asp:Button Text="Cancel" class="btnNormal" runat="server" ID="btnApproveCancel"
                                                                                    CausesValidation="False" OnClick="btnApproveCancel_Click" Width="70px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                                                   {%>
                                                                <asp:Button ID="btnOpen" CssClass="allbtn_ActionWindow" BorderStyle="None" CausesValidation="False"
                                                                    Text="Open" runat="server" ToolTip="Open"></asp:Button>
                                                                <% } %>
                                                                <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                                                   {%>
                                                                <asp:Button ID="btndelete" CssClass="btnDelete_ActionWindow" BorderStyle="None" CausesValidation="False"
                                                                    Text="Delete" runat="server" ToolTip="Delete"></asp:Button>
                                                                <% } %>
                                                                <%--(Rematch-->Reprocess) Modified by Mainak 2018-09-20--%>
                                                                 <asp:Button ID="btnRematch" runat="server" CssClass="Rematch_button" BorderWidth="0px"
                                                                    BorderStyle="None" CausesValidation="False" Text="Reprocess" OnClick="btnRematch_Click">
                                                                </asp:Button>
                                                                <%--Added by Mainak 2018-08-10--%>
                                                                <asp:Button ID="btnRelease" runat="server" CssClass="Rematch_button" BorderWidth="0px"
                                                                    BorderStyle="None" CausesValidation="False" Text="Release" OnClick="btnRelease_Click">
                                                                </asp:Button>
                                                                <div id="overlay1" class="web_dialog_overlay_Release" runat="server">
                                                                </div>
                                                                <div id="dialog1" class="web_dialog_Release" runat="server">
                                                                    <table style="width: 95%; border: 0px; text-align: left; margin-left: 10px; margin-right: 10px;"
                                                                        cellpadding="3" cellspacing="0">
                                                                        <%--<tr>
                                                                                <td class="web_dialog_title_Release">
                                                                                    
                                                                                </td>
                                                                                <td class="web_dialog_title_Release align_right">
                                                                                    <a href="#" id="A1"></a>
                                                                                </td>
                                                                            </tr>--%>
                                                                        <tr>
                                                                            <td class="web_dialog_title_Release">
                                                                                <br />
                                                                                Confirmation
                                                                            </td>
                                                                            <td class="web_dialog_title_Release align_right">
                                                                                <a href="#" id="A2"></a>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="font-size: 11px;">
                                                                                <br />
                                                                                Important: before releasing, are you sure that after editing this document it matches
                                                                                to the order(s)?
                                                                                <br />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right;">
                                                                            </td>
                                                                            <td style="text-align: center;">
                                                                                <asp:Button Text="OK" runat="server" ID="btnReleaseOk" class="btnNormal btnReleaseOk"
                                                                                    CausesValidation="False" OnClick="btnReleaseOk_Click" Width="70px" />
                                                                                <asp:Button Text="Cancel" class="btnNormal" runat="server" ID="btnReleaseCancel"
                                                                                    CausesValidation="False" OnClick="btnReleaseCancel_Click" Width="70px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                            </td>
                                                            <td align="center" style="font-size: 10px">
                                                                <a href='#' onclick="GoToStockQC();" id="lnkVariance" runat="server" visible="true">
                                                                    <b>Variance against PO </b></a>
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                        <%--Added By Mainak 2018-05-26--%>
                                                        <tr>
                                                            <td align="right">
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblRejectionCode" runat="server" Text="" Style="font-size: 11px !important;
                                                                    font-family: verdana,Tahoma,Arial !important; color: Red;"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                        <%--Ended--%>
                                                        <tr>
                                                            <td align="right">
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="True" Font-Size="8" ForeColor="Red"
                                                                    Visible="False"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lblG2App" Style="display: none" runat="server"></asp:Label>
                                    <!-- Main Content Panel Ends-->
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--following grid view is for list of downloadable files. added by koushik das as on 16-Sep-2016--%>
                                    <asp:GridView ID="gvFileLinks" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                                        CssClass="table1" CellPadding="0" CellSpacing="0">
                                        <AlternatingRowStyle BackColor="LightCyan"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="PURCHASE ORDERS">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkFile" runat="server" Text='<%#Eval("FileName")%>' OnClick="lnkFile_Click"></asp:LinkButton>
                                                    <asp:Label ID="lblPath" runat="server" Text='<%#Eval("FilePath")%>' Style="display: none;"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                    <iframe id="frm" runat="server" frameborder="0" src="FileDownload.aspx" width="100%"
                                        height="auto" scrolling="no"></iframe>
                                    <asp:DataGrid ID="grdFile" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
                                        CellPadding="0" CellSpacing="0" CssClass="listingArea" Width="100%" OnItemCommand="grdFile_ItemCommand1"
                                        OnItemDataBound="grdFile_ItemDataBound1" Visible="false">
                                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                        <ItemStyle></ItemStyle>
                                        <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Invoice No." Visible="False">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblInvNo" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceNo") %>'>
                                                    </asp:Label>
                                                    <asp:Label runat="server" ID="lblInvID" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="File Name" Visible="False">
                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="70%"></HeaderStyle>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.DocumentID") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lblHidePath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ImagePath") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lblArchPath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.archiveImagePath") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="ATTACHMENTS">
                                                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="noBorder"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="HpDownload" CommandArgument="DOW" BorderWidth="0" runat="server">
                                                        <asp:Label ID="lblPath" runat="server"></asp:Label>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn Visible="False" HeaderText="Delete">
                                                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="HpDel" CommandArgument="DEL" BorderWidth="0" runat="server"
                                                        ImageUrl="../../images/delete.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                                            Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p class="normalbody" align="center">
                                        <asp:Label ID="lblMsg" runat="server" CssClass="ErrMsg">No Files Found</asp:Label>
                                    </p>
                                </td>
                            </tr>
                        </table>

                        <!--Popup For INV , Added By Kd 06.12.2018 -->
                        <div>
                            <asp:LinkButton Text="" ID="lnkFake" runat="server" />
                            <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake"
                                CancelControlID="btnClose1" BackgroundCssClass="modalBackground">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                                <div class="header">
                                    Details
                                </div>
                                <div class="body">
                                    <table class="NormalBody">
                                        <tr>
                                            <td style="padding: 1px 0;" class="PopUpHeader">
                                                Approval Path:
                                                <asp:Label ID="lblauthstring" CssClass="NormalBody" runat="server"></asp:Label>
                                            </td>
                                            <td style="padding: 10px 0;" class="PopUpHeader">
                                                Department:
                                                <asp:Label ID="Label3" runat="server" CssClass="NormalBody"></asp:Label>
                                            </td>
                                            <td style="padding: 10px 0;" class="PopUpHeader">
                                                Business Unit:
                                                <asp:Label ID="Label4" runat="server" CssClass="NormalBody"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" colspan="3">
                                                <asp:DataGrid ID="dgSalesCallDetails" runat="server" PageSize="8" AllowPaging="True"
                                                    AutoGenerateColumns="False" GridLines="Vertical" OnPageIndexChanged="dgSalesCallDetails_PageIndexChanged1"
                                                    CellPadding="0" CellSpacing="0" BorderWidth="1px" BorderStyle="None" Width="100%"
                                                    CssClass="listingArea">
                                                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                                                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                                    <ItemStyle></ItemStyle>
                                                    <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                                    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundColumn Visible="False" DataField="RID" HeaderText="RID">
                                                            <HeaderStyle Width="120px"></HeaderStyle>
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="InvoiceID" HeaderText="InvoiceID">
                                                            <HeaderStyle Width="300px"></HeaderStyle>
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="InvoiceNo" HeaderText="Credit Note No" Visible="False">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserName" HeaderText="User Name"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserCode" HeaderText="UserID"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="GroupName" HeaderText="Group Name"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Status" HeaderText="Action Status"></asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="UserTypeID" HeaderText="UserType ID">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ActionDate" HeaderText="Action Date" ItemStyle-Width="69px">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DocStatus" HeaderText="Doc Status"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Comments" HeaderText="Comments"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="rejectioncode" HeaderText="Rejection Code"></asp:BoundColumn>
                                                    </Columns>
                                                    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                                                    </PagerStyle>
                                                </asp:DataGrid>
                                                <!-- Main Content Panel Ends-->
                                                <asp:Label ID="Label5" runat="server" ForeColor="Red" CssClass="NormalBody"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Main Content Panel Ends-->
                                </div>
                                <div class="footer" align="center">
                                    <asp:Button ID="btnClose1" runat="server" Text="" CssClass="button" />
                                </div>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAddNew" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelLine" EventName="Click" />
                       <%-- <asp:AsyncPostBackTrigger ControlID="btnSaveLine" EventName="Click" />--%> <%--commented by kd on 21-01-2019--%>
                        <%-- <asp:AsyncPostBackTrigger ControlID="btnCancel"  EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btndelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnReopen" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnReject" EventName="Click" />--%>
						<asp:PostBackTrigger ControlID="btnSaveLine" /> <%--added by kd on 21-01-2019--%>
                        <asp:PostBackTrigger ControlID="btnRematch" />
                        <asp:PostBackTrigger ControlID="btnRelease" />
                        <asp:PostBackTrigger ControlID="btnCancel" />
                        <asp:PostBackTrigger ControlID="btndelete" />
                        <asp:PostBackTrigger ControlID="btnReopen" />
                        <asp:PostBackTrigger ControlID="btnReject" />
                        <asp:PostBackTrigger ControlID="btnOpen" />
                        <asp:PostBackTrigger ControlID="btnApprove" />
                        <asp:PostBackTrigger ControlID="gvFileLinks" />
                        <asp:PostBackTrigger ControlID="btnApproveOk" />
                         <%--Added by Mainak 2018-09-11--%>
                        <asp:PostBackTrigger ControlID="btnApproveOk" />
                        <%--Added by Mainak 2018-09-20--%>
                        <asp:PostBackTrigger ControlID="btnReleaseOk" />
                        <%--<asp:PostBackTrigger ControlID="btnProrateSubmit" />--%>
                        <%--<asp:AsyncPostBackTrigger ControlID="txtLineDescription" eventname="TextChanged"/>--%>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
    <script language="javascript" type="text/javascript">

        window.onbeforeunload = WindowCloseHanlder;
        function WindowCloseHanlder() {
            // alert('My Window is Closing');
            //    CaptureClose();

        }        
    </script>
    <script type="text/javascript">
        function openJS() {
            // alert('Window Loaded Mrinal');
        }
        function closeJS() {
            //alert('Window closed Mrinal')
        }
    </script>
    <%--Added by Mainak 2018-04-06--%>
    <script type="text/javascript">

        $(document).ready(function () {

            $("#chkDepartment").attr("checked", false);
            $("#btnShowModal").click(function (e) {
                $("#txtComment").focus();
                ShowDialog(true);
                e.preventDefault();
            });

            $("#btnClose").click(function (e) {
                HideDialog();
                e.preventDefault();
            });

            $("#btnProrateSubmit").click(function (e) {
                if ($("#chkDepartment").find("input:checked").length <= 0) {
                    alert("Please select Department")
                    ShowDialog(true);
                    e.preventDefault();
                }
            });

        });

        function ShowDialog(modal) {
            $("#overlay").show();
            $("#dialog").fadeIn(300);

            if (modal) {
                $("#overlay").unbind("click");
            }
            else {
                $("#overlay").click(function (e) {
                    HideDialog();
                });
            }
        }

        function HideDialog() {
            $("#overlay").hide();
            $("#dialog").fadeOut(300);
        }          
//        ShowDialog1 added by kd on 16jan2019
        function ShowDialog1(modal) {
//            debugger;
            $("#overlay").show();
            $("#dialog").fadeIn(300);

            if (modal) {
                $("#overlay").unbind("click");
               
            }
            else {
                $("#overlay").click(function (e) {
                    HideDialog();
                });
            }
        }
        
//        function CloseDialog1() {
//            $("#overlay").hide();
//            $("#dialog").fadeOut(300);
//        }     
    </script>
    <script type="text/javascript">
        //select all checkboxes
        $(document).ready(function () {
            var length = $("#chkDepartment").find("input:checkbox").length;

            var $chkDepartment = $(this).find("#<%=chkDepartment.ClientID%>");
            $(this).find('#select_all').change(function (e) {
                //alert('changed');
                var state = $(this).attr('checked');
                // alert(state);
                $chkDepartment.children("li").each(function () {
                    //alert($(this).index());
                    $(this).children("input[type='checkbox']").attr('checked', state);
                });

            });


            $chkDepartment.children("li").each(function () {
                var $chkBx = $(this).children("input[type='checkbox']");

                $chkBx.change(function () {
                    var c = $chkDepartment.children("li").length;
                    var i = 0;
                    var cnt = 0;

                    for (i = 0; i < c; i++) {
                        $chkBx = $chkDepartment.children("li").children("input[type='checkbox']").eq(i);

                        if ($chkBx.attr('checked') == true) {
                            cnt++;
                        }
                    }

                    if (cnt == c) {
                        $(document).find('#select_all').attr('checked', true);
                    }
                    else {
                        $(document).find('#select_all').attr('checked', false);
                    }
                });
            });

            //            $chkDepartment.children("li").change(function () {
            //                var length1 = $("#chkDepartment").find("input:checked").length;
            //                //alert($(this).index());
            //                var state1 = $(this).children("input[type='checkbox']").attr('checked');

            //                if (length == length1) {
            //                   
            //                    $(document).find('#select_all').attr('checked', state1);
            //                }
            //                else {
            //                    $(document).find('#select_all').attr('checked', false);
            //                }
            //            });

        });        
    </script>
    <%--Ended by Mainak 2018-04-06--%>
</body>
</html>
