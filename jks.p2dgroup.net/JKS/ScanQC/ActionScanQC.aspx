<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActionScanQC.aspx.cs" Inherits="JKS.JKS_ScanQC_ActionScanQC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Action Scan QC</title>
    <link rel="stylesheet" type="text/css" href="../custom_css/ScanQC.css" />
    <link rel="stylesheet" type="text/css" href="../../Utilities/ETH.css" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />
    <link href="../custom_css/jquery.modal.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.js" type="text/javascript"></script>
    <script src="../js/jquery.modal.js" type="text/javascript"></script>
    <style type="text/css">
        .stretch
        {
            width: 96% !important;
            font-size: 9px; /*Added by Mainak 2017-11-28 */
        }
    </style>
    <script type="text/javascript">
        $(document).load(function () {

        });
        function openLineItem() {
            //alert('abcd');
            if ($("#divLineItems_clicker").hasClass("not_selected")) {
                $("#divHeader_clicker").addClass("not_selected");
                $("#divHeader_clicker").removeClass("selected");
                $("#divLineItems_clicker").addClass("selected");
                $("#divLineItems_clicker").removeClass("not_selected");

                $("#divHeader").addClass("hidden");
                $("#divHeader").removeClass("shown");
                $("#divLineItems").addClass("shown");
                $("#divLineItems").removeClass("hidden");

                $(".header").css("background-color", "#4ba9dc");

                localStorage.setItem("selected", "divLineItems_clicker");
            }
        }
        //code to run on load of the page
        $(document).ready(function () {
            //Click event for Header section
            $("#divHeader_clicker").click(function () {
                if ($("#divHeader_clicker").hasClass("not_selected")) {
                    $("#divHeader_clicker").addClass("selected");
                    $("#divHeader_clicker").removeClass("not_selected");
                    $("#divLineItems_clicker").addClass("not_selected");
                    $("#divLineItems_clicker").removeClass("selected");

                    $("#divHeader").addClass("shown");
                    $("#divHeader").removeClass("hidden");
                    $("#divLineItems").addClass("hidden");
                    $("#divLineItems").removeClass("shown");

                    $(".header").css("background-color", "#3399cc");

                    localStorage.setItem("selected", "divHeader_clicker");
                }
            });
            //Click event for LineItems section
            $("#divLineItems_clicker").click(function () {
                openLineItem();
                //if ($("#divLineItems_clicker").hasClass("not_selected")) {
                //    $("#divHeader_clicker").addClass("not_selected");
                //    $("#divHeader_clicker").removeClass("selected");
                //    $("#divLineItems_clicker").addClass("selected");
                //    $("#divLineItems_clicker").removeClass("not_selected");

                //    $("#divHeader").addClass("hidden");
                //    $("#divHeader").removeClass("shown");
                //    $("#divLineItems").addClass("shown");
                //    $("#divLineItems").removeClass("hidden");

                //    $(".header").css("background-color", "#4ba9dc");

                //    localStorage.setItem("selected", "divLineItems_clicker");
                //}
            });
            //Attach DatePick to txtDocDate
            $("#<%=txtDocDate.ClientID%>").datepicker({
                showOn: 'both',
                dateFormat: 'dd/mm/yy',
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,
                showOn: "button",
                buttonImage: "../images/DatePick.png",
                buttonImageOnly: true,
                buttonText: "Select date"
            });
            ////some validation in txtDocDate
            //$("#<%=txtDocDate.ClientID%>").on('keydown', function (e) {
            //    if (e.which == 8 || e.which == 46)
            //        return true;
            //    else
            //        return false;
            //});
            ////
            //$("#<%=txtSupplier.ClientID%>").on("input", function () {
            //    var v = $("#<%=txtSupplier.ClientID%>").width();
            //    console.log(v);
            //    $(".ui-autocomplete-input").css("width", v + "px !important");
            //});
            //Ajax call for txtSupplier for autocomplete
            $("#<%=txtSupplier.ClientID%>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ActionScanQC.aspx/ListSupplierNames",
                        data: '{"input1":"' + $('#<%=txtSupplier.ClientID%>').val().toString().replace("'", "''") + '", "input2":"' + $('#<%=ddlCompany.ClientID%>').val() + '"}',
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    //label: item
                                    label: item.split('^')[0],
                                    val: item.split('^')[1]
                                }
                            }))
                            //alert("djf");
                            $("#<%=txtValidSupplier.ClientID%>").val('False');
                        },
                        error: function (result) {
                            //alert("No Match Found");
                            $("#<%=txtValidSupplier.ClientID%>").val('False');
                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hfSupplierID.ClientID %>").val(i.item.val);
                    $("#<%=txtValidSupplier.ClientID%>").val('True');
                    SetCurrencyStatus();
                    SetDefaultsLinkColour();
                },
                open: function () {
                    var $txt = $("#<%=txtSupplier.ClientID%>");
                    var w = $txt.width() + 8;
                    var t = $txt.offset().top + $txt.height() + 4;
                    var l = $txt.offset().left;
                    var h = 150;

                    $txt.autocomplete("widget").css("cssText", "height: " + h + "px !important; left: " + l + "px !important; top: " + t + "px !important; width: " + w + "px !important;");
                },
                minLength: 1
            });

            //Mainak, AutoComplete for PO Number
            $(".classPONumber").each(function () {
                //alert("ABCD");
                var thisElement = $(this);
                $(this).autocomplete({
                    source: function (request, response) {
                        //alert('HAHAHHAHAHHA');
                        var batchTypeID = $('#<%=ddlBatchType.ClientID%>').children("option").filter(":selected").val();
                        var buyerID = $('#<%=ddlCompany.ClientID%>').children("option").filter(":selected").val();
                        var supplierID = $('#hfSupplierID').val();
                        var poNoPartial = thisElement.val();
                        //var v = batchTypeID + "()" + buyerID + "()" + supplierID + "()" + poNoPartial;
                        //alert(v);
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "ActionScanQC.aspx/ListPONumber",
                            data: '{"batchTypeID":"' + batchTypeID + '", "buyerID":"' + buyerID + '","supplierID":"' + supplierID + '","poNoPartial":"' + poNoPartial + '"}',
                            dataType: "json",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        //label: item
                                        label: item.split('^')[0],
                                        val: item.split('^')[1]
                                    }
                                }))

                            },
                            error: function (result) {
                            }
                        });
                    },
                    select: function (e, i) {
                    },
                    open: function () {
                        var $txt = thisElement;
                        var w = $txt.width() + 15;
                        var t = $txt.offset().top + $txt.height() + 4;
                        var l = $txt.offset().left;
                        var h = 150;

                        $txt.autocomplete("widget").css("cssText", "height: " + h + "px !important; left: " + l + "px !important; top: " + t + "px !important; width: " + w + "px !important;");
                    },
                    minLength: 1
                });
            });

            //get value from local storage to keep a section selected
            $("#" + localStorage.getItem("selected")).click();
            //set divLineItems_clicker's border when IsLineItem is true in database
            if ($("#<%=txtIsLnItm.ClientID%>").val() == "True") {
                $("#divLineItems_clicker").css("border", "2px solid red");
            };
            //set values and validtions when txtSupplier is out of focus
            $("#<%=txtSupplier.ClientID%>").blur(function () {
                isValidSupplier();
                SetCurrencyStatus();
                SetDefaultsLinkColour();
            });
            //on change of Currency drop down
            $("#<%=ddlCurrency.ClientID%>").change(function () {
                var sel = $(this).find("option:selected").text();
                var $lblAccCur = $("#<%=lblAccCur.ClientID%>")
                var cur = $lblAccCur.html();
                //alert("changed: " + sel + ", cur: " + cur);
                if (sel != cur) {
                    $lblAccCur.addClass("cur-diff");
                    $lblAccCur.removeClass("cur-same");
                }
                else {
                    $lblAccCur.addClass("cur-same");
                    $lblAccCur.removeClass("cur-diff");
                }
            });
            //resize some elements with DoResize function
            setTimeout(DoResize(), 1000);
            //on mouseenter populate currency for future checking
            $("#<%=btnProcess.ClientID%>").mouseenter(function (e) {
                //alert("reached!");
                isValidSupplier();
                SetCurrencyStatus();
                $("#<%=txtSupplier.ClientID%>").focus();
            });
            //on keydown populate currency for future checking
            $("#<%=btnProcess.ClientID%>").keydown(function (e) {
                //alert("reached!");
                isValidSupplier();
                SetCurrencyStatus();
                $("#<%=txtSupplier.ClientID%>").focus();
            });
            //Select all check box for the grid view header
            $(".cbSelectAll_H").children(0).change(function (e) {
                var tf = $(this).is(':checked');
                //alert(tf);
                $(".cbSelectAll_F").children(0).prop('checked', tf);
                $(".cbSelectAll_A").children(0).prop('checked', tf);
                //SelectAllLineItems(tf);
            });
            //Select all check box for the grid view footer
            $(".cbSelectAll_F").children(0).change(function (e) {
                var tf = $(this).is(':checked');
                //alert(tf);
                $(".cbSelectAll_H").children(0).prop('checked', tf);
                $(".cbSelectAll_A").children(0).prop('checked', tf);
                //SelectAllLineItems(tf);
            });
            //Following function is for the functionality of per row of the ListItems grid view (table).
            $("#<%=gvLists.ClientID%> .table-list-item").each(function (e) {
                var c = $("#<%=gvLists.ClientID%> .table-list-item").length;
                var i = 0;
                var v = 0;
                //alert(c);
                //set qty summation to the total section
                var $txtQty = $(this).children().eq(3).find("input[type='text']");
                var $QtyTot = $(".table-list .table-list-footer td").eq(3);
                var vx1 = 0;
                $txtQty.on('blur', function () {
                    v = $(this).val();
                    if (v == ".")
                        v = "";
                    if (v != "")
                        v = ReturnTwoDecimal(v);
                    else
                        v = "0.00";
                    $(this).val(v);
                    for (i = 0; i < c; i++) {
                        var $txtQtyE = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(3).find("input[type='text']");
                        v = $txtQtyE.val();
                        if (v != "") {
                            v = ReturnTwoDecimal(v);
                            vx1 += parseFloat(v);
                        }
                    }
                    //alert(vx1);
                    $QtyTot.html(ReturnTwoDecimal(vx1));
                    vx1 = 0;
                    i = $(this).parent().parent().index() - 1;
                    //PutTotal(i, c);
                });
                //set RATE summation to the total section
                var $txtRate = $(this).children().eq(4).find("input[type='text']");
                $txtRate.on('blur', function () {
                    v = $(this).val();
                    if (v == ".")
                        v = "";
                    if (v != "")
                        v = ReturnTwoDecimal(v);
                    else
                        v = "0.00";
                    $(this).val(v);
                    i = $(this).parent().parent().index() - 1;
                    //PutTotal(i, c);
                });
                //set vat summation to the total section
                var $txtVAT = $(this).children().eq(5).find("input[type='text']");
                var $VATTot = $(".table-list .table-list-footer td").eq(5);
                var vx2 = 0;
                $txtVAT.on('blur', function () {
                    v = $(this).val();
                    if (v == ".")
                        v = "";
                    if (v != "")
                        v = ReturnTwoDecimal(v);
                    else
                        v = "0.00";
                    $(this).val(v);
                    for (i = 0; i < c; i++) {
                        var $txtVATE = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(5).find("input[type='text']");
                        v = $txtVATE.val();
                        if (v != "") {
                            v = ReturnTwoDecimal(v);
                            vx2 += parseFloat(v);
                        }
                    }
                    //alert(vx2);
                    $VATTot.html(ReturnTwoDecimal(vx2));
                    vx2 = 0;
                    i = $(this).parent().parent().index() - 1;
                    //PutTotal(i, c);
                });
                //set val summation to the total section
                var $txtVal = $(this).children().eq(6).find("input[type='text']");
                var $ValTot = $(".table-list .table-list-footer td").eq(6);
                var vx3 = 0;
                $txtVal.on('blur', function () {
                    v = $(this).val();
                    if (v == ".")
                        v = "";
                    if (v != "")
                        v = ReturnTwoDecimal(v);
                    else
                        v = "0.00";
                    $(this).val(v);
                    for (i = 0; i < c; i++) {
                        var $txtValE = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(6).find("input[type='text']");
                        v = $txtValE.val();
                        if (v != "") {
                            v = ReturnTwoDecimal(v);
                            vx3 += parseFloat(v);
                        }
                    }
                    //alert(vx3);
                    $ValTot.html(ReturnTwoDecimal(vx3));
                    vx3 = 0;
                });
                //select all
                var $sel = $(this).children().eq(7).find("input[type='checkbox']");
                $sel.change(function (e1) {
                    var x = 0;
                    var tf = false;
                    for (i = 0; i < c; i++) {
                        var isCh = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(7).find("input[type='checkbox']").prop('checked');
                        //alert(isCh);
                        if (isCh) {
                            x++;
                        }
                    }
                    if (x == c) {
                        tf = true;
                    }
                    else {
                        tf = false;
                    }
                    //alert(tf);
                    $(".cbSelectAll_H").children(0).prop('checked', tf);
                    $(".cbSelectAll_F").children(0).prop('checked', tf);
                });
            });
            //Process Validation Click
            $("#btnProcessValidation").click(function (e) {
                $(this).attr("disabled", "disabled");
                AjaxCall();
                var refreshIntervalId = setInterval(function () {
                    if ($("#<%=txtValidPO.ClientID%>").val() != "") {
                        clearInterval(refreshIntervalId);
                        $("#<%=btnProcess.ClientID%>").click()
                        setTimeout($("#<%=txtValidPO.ClientID%>").val(''), 1000);
                    }
                }, 500);
            });
            //Pop up modal for list of PO Supplier
            $('#btnPopModal').click(function () {
                $("#<%=txtPOSupplier.ClientID%>").val('');
                //Modifeied by Mainak 2017-11-25
                if ($('#<%=ddlCompany.ClientID%>').val() != 29552) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ActionScanQC.aspx/GetSupplierRecordByPONOBID",
                        data: "{'input1':'" + $("#<%=txtPONumber.ClientID%>").val() + "','input2':'" + $("#<%=ddlCompany.ClientID%>").val() + "'}",
                        dataType: "json",
                        success: function (data) {

                            var x = data.d;
                            if (x != "") {
                                $(".popupcontent").html(x);
                                x = $('#divTableContainer').html();
                                window.parent.$('#SupplierModal').find('.modal-body').html(x);
                                window.parent.$('#SupplierModal').modal('show');

                                setTimeout(function () {
                                    window.parent.$('#SupplierModal').find(".close-modal").focus();
                                });
                            }
                            else {
                                $("popupcontent").html('');
                                alert('No supplier(s) found for the PO No. ' + $("#<%=txtPONumber.ClientID%>").val() + '.');
                            }
                        },
                        error: function (result) {
                            $("popupcontent").html('');
                        }
                    });
                }
                else {
                    //alert('Company did not match with expected company.');
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ActionScanQC.aspx/GetPOSuppliers",
                        data: "{'input1':'" + $("#<%=txtPONumber.ClientID%>").val() + "'}",
                        dataType: "json",
                        success: function (data) {

                            var x = data.d;
                            if (x != "") {
                                $(".popupcontent").html(x);
                                x = $('#divTableContainer').html();

                                window.parent.$('#SupplierModal').find('.modal-body').html(x);
                                window.parent.$('#SupplierModal').modal('show');

                                setTimeout(function () {
                                    window.parent.$('#SupplierModal').find(".close-modal").focus();
                                });
                            }
                            else {
                                $("popupcontent").html('');
                                alert('No supplier(s) found for the PO No. ' + $("#<%=txtPONumber.ClientID%>").val() + '.');
                            }
                        },
                        error: function (result) {
                            $("popupcontent").html('');
                        }
                    });
                }
                //End Modifiaction By Mainak 2017-11-27
            });
            //on mouse over show a popup
            $(".info_icon").mouseenter(function () {
                var $m = $(".pop_msg");
                var $t = $(this);
                var $p = $t.parent();
                var l = $t.width() + ($p.width() / 2) - 5;
                var t = $m.height() + ($p.height() * 2) - 5;

                $m.css("right", l);
                $m.css("margin-top", -t);

                $m.css("display", "block");
            });
            //on mouse leave close the popup
            $(".info_icon").mouseleave(function () {
                $(".pop_msg").css("display", "none");
            });
            //highlight option for text boxes
            $("#divHeader input[type=text]").each(function () {
                $(this).on("input", function () {
                    var id = $(this).attr('id');
                    var x = id.indexOf("txtPOSupplier");
                    //alert(x);
                    if (x > -1)
                        return;

                    x = id.indexOf("txtPONumber");
                    //alert(x);
                    if (x > -1 && $("#<%=txtIsPO.ClientID%>").val().toUpperCase() == "FALSE")
                        return;

                    //alert("reached");

                    if ($(this).val() != "")
                        $(this).attr("style", "border-color: #ccc !important");
                    if ($(this).val() == "")
                        $(this).attr("style", "border-color: red !important");
                });
            });
            //highlight option for ddlCurrency box
            $("#<%=ddlCurrency.ClientID%>").on("change", function () {
                if ($(this).val() != "0")
                    $(this).attr("style", "border-color: #ccc !important");
                if ($(this).val() == "0")
                    $(this).attr("style", "border-color: red !important");
            });
            //highlight option for txtDocDate box
            $("#<%=txtDocDate.ClientID%>").on("change", function () {
                if ($(this).val() != "")
                    $(this).attr("style", "border-color: #ccc !important");
                if ($(this).val() == "0")
                    $(this).attr("style", "border-color: red !important");
            });
            //open defaults modal
            $("#lnkDefaults").click(function () {
                //alert('clicked defaults link.');
                if (parseInt($("#<%=hfSupplierID.ClientID%>").val()) <= 0) {
                    alert("Please select a valid supplier from the list.");
                    $("#<%=txtSupplier.ClientID%>").focus();
                    return;
                }
                //alert('input1:' + $("#<%=ddlCompany.ClientID%>").val());
                //alert('input2:' + $("#<%=hfSupplierID.ClientID%>").val());
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "ActionScanQC.aspx/DefaultsDataSet",
                    data: "{'input1':'" + $("#<%=ddlCompany.ClientID%>").val() + "', 'input2':'" + $("#<%=hfSupplierID.ClientID%>").val() + "'}",
                    dataType: "JSON",
                    success: function (data) {
                        var xStr = data.d;
                        //alert(xStr);

                        if (xStr.length > 0) {
                            arr = xStr.split('^');
                            //alert(arr);

                            var BuyerCompany = arr[0].toString();
                            var SupplierCompany = arr[1].toString();
                            var VendorID = arr[2].toString();
                            var VendorClass = arr[3].toString();
                            var StockFBNominalCode = arr[4].toString();
                            var ExpenseNominalCode = arr[5].toString();
                            var AccountCurrency = arr[6].toString();
                            var StockFBNominal = arr[7].toString();
                            var ExpenseNominal = arr[8].toString();

                            var BuyerCompanyID = $("#<%=ddlCompany.ClientID%>").val();
                            var SupplierCompanyID = $("#<%=hfSupplierID.ClientID%>").val();

                            var $divDMC = $(document).find("#divDefaultsModalContent");
                            //alert($divDMC.html());

                            //did not work
                            //$divDMC.find("#txtBuyerCompany").val(BuyerCompany);
                            //$divDMC.find("#txtBuyerCompanyID").val(BuyerCompanyID);
                            //$divDMC.find("#txtSupplierCompany").val(SupplierCompany);
                            //$divDMC.find("#txtSupplierCompanyID").val(SupplierCompanyID);
                            //$divDMC.find("#txtVendorID").val(VendorID);
                            //$divDMC.find("#txtVendorClass").val(VendorClass);
                            //$divDMC.find("#txtStockFBNominal").val(StockFBNominal);
                            //$divDMC.find("#txtStockFBNominalCode").val(StockFBNominalCode);
                            //$divDMC.find("#txtExpenseNominal").val(ExpenseNominal);
                            //$divDMC.find("#txtExpenseNominalCode").val(ExpenseNominalCode);
                            //$divDMC.find("#selAccountCurrency").val(AccountCurrency);
                            //did not work

                            var x = $divDMC.html();

                            x = x.replace("BuyerCompany1", BuyerCompany);
                            x = x.replace("BuyerCompanyID1", BuyerCompanyID);
                            x = x.replace("SupplierCompany1", SupplierCompany);
                            x = x.replace("SupplierCompanyID1", SupplierCompanyID);
                            x = x.replace("VendorID1", VendorID);
                            x = x.replace("VendorClass1", VendorClass);
                            x = x.replace("StockFBNominal1", StockFBNominal);
                            x = x.replace("StockFBNominalCode1", StockFBNominalCode);
                            x = x.replace("ExpenseNominal1", ExpenseNominal);
                            x = x.replace("ExpenseNominalCode1", ExpenseNominalCode);

                            if (AccountCurrency.length == 0)
                                x = x.replace("value='0'>", "value='0' selected='selected'>");
                            else
                                x = x.replace(">" + AccountCurrency, "selected='selected'>" + AccountCurrency);

                            //alert(x);

                            window.parent.$('#DefaultsModal').find('.modal-body').html(x);
                            window.parent.$('#DefaultsModal').modal('show');
                            setTimeout(function () {
                                window.parent.$('#DefaultsModal').find("#btnSave1").focus();
                            }, 100);
                        }
                    },
                    failure: function (data) {

                    },
                    error: function (data) {

                    }
                });
            });
            //Change functions for ddlCompany
            $("#<%=ddlCompany.ClientID%>").change(function () {
                //alert("changed");
                UpdateAqillaSuppliers();
                SetDefaultsLinkColour();
            });
            //Populate Currency for Supplier Defaults Pop-up
            PopulateCurrencyForDefaults();
            ////Trigger change for Company dorpdown box
            //$("#<%=ddlCompany.ClientID%>").trigger("change");
            //Populate list of Aqilla Supplier for the pop-up
            UpdateAqillaSuppliers();
            //Set colour of the defaults link
            SetDefaultsLinkColour();
        });
        //code to run on resize of the page
        $(document).resize(function () {
            //resize some elements with DoResize function
            setTimeout(DoResize(), 1000);
        });
        //redirection of page to the parent page from iframe
        function redirect(theLocation) {
            window.top.location.href = theLocation;
            window.parent.location.href = theLocation;
            window.top.location.replace(theLocation);
        };
        //On PROCESS validation
        function validation() {
            //Make the process button enabled.
            $("#btnProcessValidation").removeAttr("disabled");
            //#region calculate total of value
            var c = $("#gvLists .table-list-item").length;
            //alert(c);
            var x = 0;
            var totVal = 0;
            for (x = 0; x < c; x++) {
                var VALUE = $("#gvLists .table-list-item").eq(x).children(0).find(".VALUE").val();
                if (VALUE == "") VALUE = 0;
                var v = parseFloat(VALUE);
                totVal += v;
            }
            var eVal = ReturnTwoDecimal(totVal);
            $("#<%=txtSum.ClientID%>").val(eVal);
            $("#gvLists .table-list-footer td").eq(5).html(eVal);//Modified by Mainak  2018-07-31, previously it would be eq(6)
            //#endregion calculate total of value

            //#region if batch type is not selected
            if ($("#<%=ddlBatchType.ClientID%>").val() == 0) {
                alert('Please select Batch Type.');
                $("#<%=ddlBatchType.ClientID%>").focus();
                return false;
            }
            //#endregion if batch type is not selected

            //#region if company is not selected
            if ($("#<%=ddlCompany.ClientID%>").val() == 0) {
                alert('Please select Company.');
                $("#<%=ddlCompany.ClientID%>").focus();
                return false;
            }
            //#endregion if company is not selected

            //#region if entered is not a valid supplier
            if ($("#<%=txtSupplier.ClientID%>").val() == "") {
                $("#<%=txtValidSupplier.ClientID%>").val("False");
            }
            var isValid = $("#<%=txtValidSupplier.ClientID%>").val();
            //alert(isValid);
            if (isValid == "False") {
                alert('Please select a valid supplier by entering\r\n the name into the Supplier field to find a match.');
                return false;
            }
            //#endregion if entered is not a valid supplier

            //#region Check if any currency is selected
            var cur = $("#<%=ddlCurrency.ClientID%> option:selected").text();

            if (cur == "") {
                alert("Please select currency.");
                return false;
            }
            //#endregion Check if any currency is selected

            //#region if Selected currency does not match to supplier’s account currency
            if ($("#<%=lblAccCur.ClientID%>").hasClass("cur-diff") == true && $("#<%=lblAccCur.ClientID%>").html() != 'NA') {
                var result = confirm('Selected currency does not match to supplier’s account currency.\r\nAre you sure you want to continue?');
                //alert(result);
                if (result == false)
                    return false;
            }
            //#endregion Selected currency does not match to supplier’s account currency

            //#region if non-standard currency is selected
            var arr = ['GBP', 'EUR', 'USD', 'SGD', 'RMB']; //standard currency
            var tf = false;

            for (i = 0; i < arr.length; i++) {
                //alert(cur + ' ' + arr[i]);
                if (cur == arr[i]) {
                    tf = true; //if standard currency is selected
                    break;
                }
            }

            //alert(tf);

            if (tf == false) {
                var result = confirm('A non-standard currency is selected.\r\nAre you sure you want to continue?');
                //alert(result);
                if (result == false)
                    return false;
            }
            //#endregion if non-standard currency is selected

            var $control = null;
            var text = "";

            //#region if doc number is NOT FOUND
            $control = $("#<%=txtDocNumber.ClientID%>");
            text = $control.val().toUpperCase();
            if (text == "" || text == "NOT FOUND" || text == "NOT_FOUND" || text == "CANCELLED") {
                alert('Please enter invoice / credit note number.');
                $control.focus();
                return false;
            }
            //#endregion if doc number is NOT FOUND

            //#region if doc date is NOT FOUND
            $control = $("#<%=txtDocDate.ClientID%>");
            text = $control.val().toUpperCase();
            if (text == "" || text == "NOT FOUND" || text == "NOT_FOUND" || text == "CANCELLED") {
                alert('Please select date from the calendar.');
                $control.focus();
                return false;
            }
            //#endregion if doc date is NOT FOUND

            //#region if doc date format is not correct
            var date_regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;
            if (!(date_regex.test(text))) {
                alert("Date must be dd/mm/yyyy format. Please select date from the calendar.");
                return false;
            }
            //#endregion if doc date format is not correct

            //#region if net is NOT FOUND
            $control = $("#<%=txtNet.ClientID%>");
            text = $control.val().toUpperCase();
            if (text == "" || text == "NOT FOUND" || text == "NOT_FOUND" || text == "CANCELLED" || text == ".") {
                alert('Please enter Net value.');
                $control.focus();
                return false;
            }
            //#endregion if net is NOT FOUND

            //#region if vat is NOT FOUND
            $control = $("#<%=txtVAT.ClientID%>");
            text = $control.val().toUpperCase();
            if (text == "" || text == "NOT FOUND" || text == "NOT_FOUND" || text == "CANCELLED" || text == ".") {
                alert('Please enter VAT.');
                $control.focus();
                return false;
            }
            //#endregion if vat is NOT FOUND

            //#region if total is NOT FOUND
            $control = $("#<%=txtTotal.ClientID%>");
            text = $control.val().toUpperCase();
            if (text == "" || text == "NOT FOUND" || text == "NOT_FOUND" || text == "CANCELLED" || text == ".") {
                alert('Please enter total value.');
                $control.focus();
                return false;
            }
            //#endregion if total is NOT FOUND

            $control = null;
            text = "";

            //#region Check Total field in header tab is negative
            var tot = parseFloat($("#<%=txtTotal.ClientID%>").val());
            if (tot < 0) {
                alert("Total cannot be negative. Please correct the Doc Type and process as a positive value.");
                return false;
            }
            //#endregion Check Total field in header tab is negative

            //#region if Net + VAT is not equal to Total
            var net = parseFloat($("#<%=txtNet.ClientID%>").val());
            var vat = parseFloat($("#<%=txtVAT.ClientID%>").val());

            var totc = parseFloat(net + vat);
            //alert(net + ' ' + vat + ' ' + tot + ' ' + totc);
            var diff = parseFloat(totc - tot);
            //diff = Round(diff, 2);
            diff = Math.round(diff * 100) / 100;
            //alert(totc + ' - ' + tot + " = " + diff);
            if (diff > 0.01 || diff < -0.01) {
                alert("Net + VAT must equal Total.");
                return false;
            }
            //#endregion if Net + VAT is not equal to Total

            //#region calculation to get day diff. from selected doc date and current date
            var val = $("#<%=txtDocDate.ClientID%>").val();
            var arr = val.split('/');
            //alert(arr[0] +' '+ arr[1] +' '+ arr[2]);             
            //Date is in dd/MM/yyyy format
            //index 0:date, 1:month, 2:year 
            var selDate = new Date(arr[2], arr[1] - 1, arr[0]);
            //alert(selDate);
            var curDate = new Date();
            //alert(selDate + ' ' + curDate);
            var timeDiff = (selDate.getTime() - curDate.getTime());
            //alert(timeDiff);
            var days = Math.ceil(timeDiff / (1000 * 3600 * 24));
            //alert(days);
            //#endregion calculation to get day diff. from selected doc date and current date

            //#region if doc day is more than 1 day
            //alert(days);
            if (parseInt(days) > 0) {
                var result = confirm('Date entered is in the future.\r\nClick Cancel to check that you have not selected\r\nthe payment due date instead of the issue / tax point date.\r\nClick OK if you are sure you want to process the document.');
                //alert(result);
                if (result == false)
                    return false;
            }
            //#endregion if doc day is more than 1 day

            //#region if doc day is more than 6 months
            //alert(days);
            if (parseInt(days) <= -180) {
                var result = confirm('Date entered is more than 6 months ago.\r\nAre you sure you want to continue?');
                //alert(result);
                if (result == false)
                    return false;
            }
            //#endregion if doc day is more than 6 months

            var isLineItems = $("#<%=txtIsLnItm.ClientID%>").val().toUpperCase();
            var IsPO = $("#<%=txtIsPO.ClientID%>").val().toUpperCase();
            var IsDesc = $("#<%=txtIsDesc.ClientID%>").val().toUpperCase();

            var PONumber = $("#<%=txtPONumber.ClientID%>").val().toUpperCase();
            var BuyerID = $("#<%=ddlCompany.ClientID%>").val();
            var Supplier = $("#<%=txtSupplier.ClientID%>").val();

            //#region retrieving values to validate PO validation
            var BlankPO = false;

            $("#<%=gvLists.ClientID%> .table-list-item").each(function () {
                var valx = $(this).children(0).find('.PONO').val(); //.html();
                valx = valx.replace("&nbsp", "").replace(";", "");
                //alert(valx);

                if (valx == "") {
                    BlankPO = true;
                }
                //alert('BlankPO = ' + BlankPO);
            });

            //alert("IsPO = " + IsPO + " PONumber = " + PONumber + " BlankPO = " + BlankPO);
            //#endregion retrieving values to validate PO validation

            //#endregion if combination of retrieved for PO validation value not validated
            //if PONumber is not populated in Header AND Line Items
            if (IsPO == "TRUE" && (PONumber == "NOT FOUND" || PONumber == "NOT_FOUND" || PONumber == "CANCELLED" || PONumber == "") && BlankPO == true) {
                var result = confirm('PO Number is missing on one or more lines.\r\nAre you sure you want to continue?');
                //alert(result);
                if (result == false)
                    return false;
            }
            //#endregion if combination of retrieved value for PO validation not validated

            /*#region If buyer companyid = 29552 and PO = True
            Does any value in any of the following fields [Header ‘PO Number’or 
            Line Items ‘PO No.’] exist in BuyerProdCodes table?*/
            if (IsPO == 'TRUE' && BuyerID == 29552) {
                var res = $("#<%=txtValidPO.ClientID%>").val();
                var arr = res.split("-");
                var tf0 = arr[0]; //is in database?
                var x = arr[1]; //if not in database then which are not
                var tf1 = arr[2]; //does belong to supplier?
                var y = arr[3]; //list of po does not belong to supplier
                //alert('tf0: ' + tf0 + ', x: ' + x + ', tf1: ' + tf1 + ', y: ' + y);
                var resultx = true;
                if (tf0 == 'False' && x != 'NA') {
                    resultx = confirm("PO No. " + x + " cannot be found in the database.\nPlease double-check it is correct before releasing the invoice.\nPress OK to continue to release it or Cancel to allow you to go back and double-check.");
                    if (!resultx)
                        return false;
                }
                //alert(resultx);
                //tf0 == 'True' && 
                if (resultx && tf1 == 'False' && y != 'NA') {
                    resultx = confirm("PO No. " + y + " does not belong to that supplier.\nAre you sure you want to continue?")
                    if (!resultx)
                        return false;
                }
            }
            /*#endregion If buyer companyid = 29552 and PO = True
            Does any value in any of the following fields [Header ‘PO Number’or 
            Line Items ‘PO No.’] exist in BuyerProdCodes table?*/

            //#region retrieving values to validate Description validation
            var BalnkDesc = false;

            $("#gvLists .table-list-item").each(function () {
                var valx = $(this).find('.DESC').val().toUpperCase();
                valx = valx.replace("&nbsp", "").replace(";", "");
                //alert(valx);
                if (valx == "") {
                    BalnkDesc = true;
                }
                //alert('BalnkDesc = ' + BalnkDesc);
            });
            //#endregion retrieving values to validate Description validation

            //#region if combination of retreived value for Description validation not validated
            if (IsDesc == "TRUE" && BalnkDesc == true) {
                var result = confirm('Description is missing on one or more line items.\r\nAre you sure you want to continue?');
                //alert(result);
                if (result == false)
                    return false;
            }
            //#endregion if combination of retreived value for Description validation not validated

            //#region quantity, price and value checking
            var isQty = true;
            var isPrice = true;
            var isValue = true;
            for (x = 0; x < c; x++) {
                var QTY = $("#gvLists .table-list-item").eq(x).children(0).find(".QTY").val();
                var PRICE = $("#gvLists .table-list-item").eq(x).children(0).find(".PRICE").val();
                //alert(QTY + ", " + PRICE);
                if (QTY == "" || parseFloat(QTY) == 0) {
                    isQty = false;
                    break;
                }
                if (PRICE == "") {
                    isPrice = false;
                    break;
                }
            }
            //alert(isQty);
            if (isLineItems == "TRUE") {
                if (!isQty) {
                    alert("Quantity field cannot be blank or 0.");
                    return false;
                }
                if (!isPrice) {
                    alert("Price field cannot be blank.");
                    return false;
                }
                if (isQty && isPrice) {
                    for (x = 0; x < c; x++) {
                        var VALUE = $("#gvLists .table-list-item").eq(x).children(0).find(".VALUE").val();
                        if (VALUE == "") {
                            isValue = false;
                            break;
                        }
                    }
                }
                if (!isValue) {
                    alert("Line item Value field cannot be blank.");
                    return false;
                }
            }
            //#endregion quantity, price and value checking

            //#region Quantity x Price does not equal Value on all lines within 1 decimal place
            if (isValue) {
                var lines = "";
                //alert(c);
                for (x = 0; x < c; x++) {
                    var QTY = $("#gvLists .table-list-item").eq(x).children(0).find(".QTY").val();
                    var PRICE = $("#gvLists .table-list-item").eq(x).children(0).find(".PRICE").val();
                    var VALUE = $("#gvLists .table-list-item").eq(x).children(0).find(".VALUE").val();

                    var xValue = parseFloat(QTY) * parseFloat(PRICE);
                    var yValue = parseFloat(VALUE);

                    xValue = Math.round(xValue * 100) / 100;
                    yValue = Math.round(yValue * 100) / 100;

                    var diff = xValue - yValue;

                    diff = Math.round(diff * 100) / 100;

                    //alert(xValue + " - " + yValue + " = " + diff);

                    if (diff > 0.01 || diff < -0.01) {
                        lines += (x + 1) + "";
                        if (x < c - 1) {
                            lines += ",";
                        }
                    }
                }
                if (lines != "" && isLineItems == "TRUE") {
                    var result = confirm('Line item Quantity x Price does not equal Value on line(s) ' + lines + '.\r\nAre you sure you want to continue?');
                    //alert(result);
                    if (result == false)
                        return false;
                }
            }
            //#endregion Quantity x Price does not equal Value on all lines within 1 decimal place

            //#region if lineitems value sum is not equal to 'net' of header field
            var lineTotal = parseFloat($("#<%=txtSum.ClientID%>").val());
            //var diff = lineTotal - net;
            //if ((diff > 0.01 || diff < -0.01) && isLineItems == "TRUE") {
            if (lineTotal != net && isLineItems == "TRUE") {
                var result = confirm('Net value does not equal sum of line item values.\r\nAre you sure you want to continue?');
                //alert(result);
                if (result == false)
                    return false;
            }
            //#endregion if lineitems value sum is not equal to 'net' of header field

            return true;
        };
        //Update from Ajax Call function for Validation
        function AjaxCall() {
            //alert("AjaxCall");
            //#region for Ajax call to update currency and status
            SetCurrencyStatus();
            //#endregion for Ajax call to update currency and status

            //#region for Ajax call to Check Valid Purchase Order
            var PONumber = $("#<%=txtPONumber.ClientID%>").val().toUpperCase();
            var BuyerID = $("#<%=ddlCompany.ClientID%>").val();
            var Supplier = $("#<%=txtSupplier.ClientID%>").val();
            var LinePOs = "";
            var $dr = $("#<%=gvLists.ClientID%> .table-list-item");
            var c = $dr.length; //count no of data row
            //alert(c);
            var i = 0;
            var $sel = null;
            for (i = 0; i < c; i++) {
                var po = $dr.eq(i).children().eq(0).find("input[type='text']").val();
                LinePOs += po;
                if (i < c - 1)
                    LinePOs += ",";
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ActionScanQC.aspx/CheckValidPurchaseOrder",
                data: "{'input1':'" + PONumber + "', 'input2':'" + LinePOs + "', 'input3':'" + Supplier + "', 'input4':'" + BuyerID + "'}",
                dataType: "json",
                success: function (data) {
                    var x = data.d;
                    //alert(x);
                    $("#<%=txtValidPO.ClientID%>").val(x);
                },
                error: function (result) {
                    var x = "False-NA-False-NA";
                    $("#<%=txtValidPO.ClientID%>").val(x);
                }
            });
            //#endregion for Ajax call to Check Valid Purchase Order
        };
        //Close the Parent window from iframe client
        function CloseParent() {
            window.parent.close();
        };
        //show popup
        function PopupPage(url, winname, w, h, id) {
            if (id == 'Link2') {
                if ($("#<%=txtSupplier.ClientID%>").val() == "") {
                    alert("Supplier field is not populated.");
                    $("#<%=txtSupplier.ClientID%>").focus();
                    return;
                }
            }
            var l = (screen.width - w) / 2; //left of window
            var t = (screen.height - h) / 2; //top of window
            winprops = 'resizable=1,height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ",scrollbars=yes";
            window.open(url, winname, winprops);
        };
        //check for numeric input
        function numaricInputOnly(event) {
            var input = event.which;
            //alert(input);
            var id = event.target.id;
            var text = $("#" + id).val();
            //alert(text);
            //alert(text.indexOf('.'));
            if (text.indexOf('.') > -1 && input == 46) {
                //alert("found");
                event.preventDefault();
                return;
            }
            var fv = text[0];
            //alert(fv);
            if (input == 45) {
                if (fv != "-") {
                    $("#" + id).val("-" + $("#" + id).val());
                    event.preventDefault();
                    return;
                }
                if (text.indexOf('-') > -1) {
                    //alert("found");
                    event.preventDefault();
                    return;
                }
            }
            var arr = [0, 8, 45, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 99, 118, 120];
            var c = arr.length;
            var i = 0;
            var flag = 0;
            for (i = 0; i < c; i++) {
                //alert(input + ", " + arr[i]);
                if (input == arr[i]) {
                    //alert("match");
                    flag = 1
                    break;
                }
            }
            if (flag == 0) {
                //alert("mis match");
                event.preventDefault();
            }
        };
        //confirmation while deletion
        function DoDelete() {
            var result = confirm("Are you sure you want to delete this document?");
            return result;
        };
        //Page resize function
        function DoResize() {
            var $DatepickerTextBox = $(document).find(".hasDatepicker");
            var $DatepickerImageBox = $(document).find(".ui-datepicker-trigger");

            var documentWidth = $(document).width();

            documentWidth = (documentWidth / 43) * 100;

            //alert(documentWidth);

            if (documentWidth <= 550) {
                $DatepickerTextBox.css("width", "91%");
                $DatepickerImageBox.css("top", "14%");
                $DatepickerImageBox.css("right", "5%");
            }
            else if (documentWidth >= 2300) {
                $DatepickerTextBox.css("width", "96%");
                $DatepickerImageBox.css("right", "2%");
            }
            else if (documentWidth >= 1700) {
                $DatepickerTextBox.css("width", "95%");
                $DatepickerImageBox.css("right", "3%");
            }
            else if (documentWidth >= 1400) {
                $DatepickerTextBox.css("width", "94%");
                $DatepickerImageBox.css("right", "4%");
            }
            else {
                //$DatepickerTextBox.removeAttr("style");
            }
        };
        //check and set if supplier is valid or not
        function isValidSupplier() {
            if ($('#<%=txtSupplier.ClientID%>').val().length == 0)
                $("#<%=hfSupplierID.ClientID %>").val("0");
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ActionScanQC.aspx/CheckValidSupplier",
                data: '{"input1":"' + $('#<%=txtSupplier.ClientID%>').val().toString() + '", "input2":"' + $('#<%=ddlCompany.ClientID%>').val() + '"}',
                dataType: "json",
                success: function (data) {
                    var x = data.d;
                    //alert(x);
                    $("#<%=txtValidSupplier.ClientID%>").val(x);
                },
                error: function (result) {
                    $("#<%=txtValidSupplier.ClientID%>").val('False');
                }
            });
        };
        //populate currency with web service
        function SetCurrencyStatus() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ActionScanQC.aspx/GetCurrencyStatus",
                data: '{"input1":"' + $('#<%=txtSupplier.ClientID%>').val().toString().replace("'", "''") + '", "input2":"' + $('#<%=ddlCompany.ClientID%>').val() + '", "input3":"' + $('#<%=ddlCurrency.ClientID%> option:selected').text() + '"}',
                dataType: "json",
                success: function (data) {
                    var x = data.d;
                    //alert(x);
                    var arr = x.split("/");
                    var curr = arr[0];
                    var clas = arr[1];
                    $("#<%=lblAccCur.ClientID%>").html(curr);
                    $("#<%=lblAccCur.ClientID%>").addClass(clas);
                    if (clas == "cur-diff")
                        $("#<%=lblAccCur.ClientID%>").removeClass("cur-same");
                    else
                        $("#<%=lblAccCur.ClientID%>").removeClass("cur-diff");
                },
                error: function (result) {
                    $("#<%=lblAccCur.ClientID%>").html("NA");
                    $("#<%=lblAccCur.ClientID%>").removeClass("cur-diff");
                }
            });
        };
        //select all line itmes function
        function SelectAllLineItems(tf) {
            var $dr = $("#<%=gvLists.ClientID%> .table-list-item");
            var c = $dr.length; //count no of data row
            //alert(c);
            var i = 0;
            var $sel = null;
            for (i = 0; i < c; i++) {
                $sel = $dr.eq(i).children().eq(7).find("input[type='checkbox']");
                $sel.prop("checked", tf);
            }
        };
        //validate if yet imported into the main system or not
        function ValildateDocument(type) {
            var result = "";
            switch (type) {
                case "prev":
                    result = confirm('This will attach a copy of this document onto the previous one i.e. if the two documents have been mistakenly separated in the scanning process. If you proceed, you should then DELETE this document. Do you want to continue?');
                    break;
                case "next":
                    result = confirm('This will attach a copy of this document onto the next one i.e. if the two documents have been mistakenly separated in the scanning process. If you proceed, you should then DELETE this document. Do you want to continue?');
                    break;
            }
            if (result) {
                //alert(result);
                //alert(type);
                AttachDocument(type);
            }
        };
        //function for attachment
        function AttachDocument(type) {
            //alert(type);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ActionScanQC.aspx/AttachDocument",
                data: '{"type":"' + type + '"}',
                dataType: "json",
                success: function (data) {
                    var x = data.d;
                    //alert(x);
                    switch (x) {
                        case "NotFound":
                            //alert(type);
                            switch (type) {
                                case "prev":
                                    alert("The previous document has not yet imported into the main system.\nPlease either try again later or download this image to your computer, DELETE the document in Scan QC, and then attach it to the previous document manually after it has imported into the main system.");
                                    break;
                                case "next":
                                    alert("The next document has not yet imported into the main system.\nPlease either try again later or download this image to your computer, DELETE the document in Scan QC, and then attach it to the next document manually after it has imported into the main system.");
                                    break;
                            }
                            break;
                        case "Found":
                            SaveDocumentData(type);
                            break;
                    }
                },
                error: function (result) {
                }
            });
        };
        //function to save data in database through ajax call
        function SaveDocumentData(type) {
            var BatchID = $("#<%=lblBatchID.ClientID%>").html();
            var BatchName = $("#<%=lblBatchName.ClientID%>").html();
            var FileName = $("#<%=lblFileName.ClientID%>").html();
            var DocID = $("#<%=lblDocID.ClientID%>").html();
            var CompanyID = $("#<%=ddlCompany.ClientID%>").val();
            //alert("BatchID: " + BatchID + "BatchName: " + BatchName + "FileName: " + FileName + "DocID: " + DocID + "CompanyID: " + CompanyID + "type: " + type);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ActionScanQC.aspx/SaveDocumentData",
                data: '{"BatchID":"' + BatchID + '","BatchName":"' + BatchName + '","FileName":"' + FileName + '","DocID":"' + DocID + '","CompanyID":"' + CompanyID + '","type":"' + type + '"}',
                dataType: "json",
                success: function (data) {
                    var x = data.d;
                    if (x == 'True')
                        alert('This document has been successfully re-attached. Therefore you should now DELETE this document.');
                    else
                        alert('Some error occurred, please try again.');
                },
                error: function (result) {
                }
            });
        };
        //function to return Return Two Decimal value of given input
        function ReturnTwoDecimal(v) {
            //alert(v);
            var j = v.toString().indexOf(".");
            if (j == -1) {
                v = v + ".00";
            }
            else {
                var arr = v.toString().split(".");
                var val = arr[0].toString();
                if (val == "")
                    v = "0" + v;

                var vl = arr[1].toString().length;
                if (vl == 1)
                    v = v + "0";
                else
                    v = Math.round(parseFloat(v) * 100) / 100;

                j = v.toString().indexOf(".");
                if (j > -1) {
                    var arr1 = v.toString().split(".");
                    var vl1 = arr1[1].toString().length;
                    if (vl1 == 1) {
                        v = v + "0";
                    }
                }
                if (j == -1) {
                    v = v + ".00";
                }
            }
            //alert(v);
            return v;
        }
        //put calculated value and total in value
        function PutTotal(i, c) {
            var v = "0.00";
            var $txtQtyE = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(3).find("input[type='text']");
            var $txtRateE = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(4).find("input[type='text']");
            var $txtVATE = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(5).find("input[type='text']");
            var $txtValE = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(6).find("input[type='text']");

            if ($txtQtyE.val() == "" || $txtQtyE.val() == ".")
                $txtQtyE.val(v);
            if ($txtRateE.val() == "" || $txtRateE.val() == ".")
                $txtRateE.val(v);
            if ($txtVATE.val() == "" || $txtVATE.val() == ".")
                $txtVATE.val(v);

            v = parseFloat($txtQtyE.val()) * parseFloat($txtRateE.val());
            v = v + parseFloat($txtVATE.val())
            v = ReturnTwoDecimal(v);

            $txtValE.val(v);

            var $ValTot = $(".table-list .table-list-footer td").eq(5);
            var vx3 = 0;

            for (i = 0; i < c; i++) {
                var $txtValE = $("#<%=gvLists.ClientID%> .table-list-item").eq(i).children().eq(6).find("input[type='text']");
                v = $txtValE.val();
                if (v != "") {
                    v = ReturnTwoDecimal(v);
                    vx3 += parseFloat(v);
                }
            }
            //alert(vx3);
            $ValTot.html(ReturnTwoDecimal(vx3));
            vx3 = 0;
        }
        //Show the message when new batch is found and close after 3 sec.
        function showNewBatchMsg(url) {
            var $newbatch = null;

            setTimeout(function () {
                $newbatch.css("display", "none");
            }, 6000);

            setTimeout(function () {
                $newbatch = $(".new-batch");

                $newbatch.css("display", "block");
            }, 3000);
        };
        //Populate Currency for Supplier Defaults Pop-up
        function PopulateCurrencyForDefaults() {
            //alert("PopulateCurrencyForDefaults");
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                url: 'ActionScanQC.aspx/CurrencyForDefaults',
                data: '',
                dataType: 'JSON',
                success: function (msg) {
                    //alert(msg.d.length);
                    $("#selAccountCurrency").append("<option value='0'></option>");
                    for (var i = 0; i < msg.d.length; i++) {
                        //alert(msg.d[i].toString());
                        var arr = msg.d[i].split('^');
                        $("#selAccountCurrency").append("<option value='" + arr[0].toString() + "'>" + arr[1].toString() + "</option>");
                    }
                }
            });
        };
        //Change Defaults link's Colour
        function SetDefaultsLinkColour() {
            if (parseInt($("#<%=hfSupplierID.ClientID%>").val()) <= 0) {
                $("#lnkDefaults").css("color", "red");
                //alert("found 0.");
                return;
            }

            $("#lnkDefaults").css("color", "#0081C5");

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ActionScanQC.aspx/DefaultsDataSet",
                data: "{'input1':'" + $("#<%=ddlCompany.ClientID%>").val() + "', 'input2':'" + $("#<%=hfSupplierID.ClientID%>").val() + "'}",
                dataType: "JSON",
                success: function (data) {
                    var xStr = data.d;
                    //alert(xStr);

                    if (xStr.length > 0) {
                        var arr = xStr.split('^');
                        //alert(arr);        

                        var VendorClass = arr[3].toString();
                        var AccountCurrency = arr[6].toString();
                        var StockFBNominal = arr[7].toString();
                        var ExpenseNominal = arr[8].toString();

                        ////any of the 4 editable fields is blank or NULL in the database
                        //if (VendorClass.length == 0 || StockFBNominal.length == 0 || ExpenseNominal.length == 0 || AccountCurrency.length == 0)
                        //    $("#lnkDefaults").css("color", "red");

                        //After 08-MAR-2017: if Vendor Class and Account Currency are both populated and at least 1 of the nominal fields is populated then it is blue.
                        if (VendorClass.length == 0 || AccountCurrency.length == 0 || (ExpenseNominal.length == 0 && StockFBNominal.length == 0))
                            $("#lnkDefaults").css("color", "red");
                        else
                            id.find("#lnkDefaults").css("color", "#0081C5");
                    }
                },
                failure: function (data) {
                    $("#lnkDefaults").css("color", "red");
                },
                error: function (data) {
                    $("#lnkDefaults").css("color", "red");
                }
            });
        };
        //
        function UpdateAqillaSuppliers() {
            var $aUAS = $("#aUpdateAqillaSuppliers");
            var bcid = $("#<%=ddlCompany.ClientID%>").val();
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'JSON',
                url: 'ActionScanQC.aspx/GetAqillaSupplierURL',
                data: '{"input1":"' + bcid + '"}',
                success: function (data) {
                    var val = data.d;
                    var arr = val.split('^');
                    //alert(val);
                    //alert(arr[0]);
                    if (arr[0] == 'false') {
                        $aUAS.addClass("hidden");
                        $aUAS.removeClass("shown");
                    }
                    if (arr[0] == 'true') {
                        $aUAS.click(function () {
                            val = arr[1];
                            alert('Suppliers are being updated. If the supplier still does not exist in the Supplier list below, please check that it is set up correctly in Aqilla and try again. After successfully selecting the new supplier, please set up the defaults using the link next to the Supplier field.');
                            //alert(val);
                            $.ajax({
                                type: 'POST',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'JSON',
                                url: 'ActionScanQC.aspx/ProcessSupplierResponse',
                                data: '{"input1":"' + val + '", "input2":"' + bcid + '"}',
                                success: function (msg1) {
                                    if (msg1.d == "True")
                                        alert("Suppliers updated successfully.");
                                    else
                                        alert("Something went wrong please consult site admin.");
                                },
                                failure: function (msg1) {
                                    alert(msg1.d);
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Status: " + textStatus); alert("Error: " + errorThrown);
                                }
                            });
                        });
                        $aUAS.addClass("shown");
                        $aUAS.removeClass("hidden");
                    }
                },
                failure: function (msg) {
                    $aUAS.addClass("hidden");
                    $aUAS.removeClass("shown");
                },
                error: function (msg) {
                    $aUAS.addClass("hidden");
                    $aUAS.removeClass("shown");
                }
            });
        };
    </script>
    <style type="text/css">
        .ui-datepicker
        {
            font-size: 10px;
        }
        #ui-id-1
        {
            width: 73% !important;
            font-size: 11px;
        }
        #ui-id-2
        {
            width: 73% !important;
            font-size: 11px;
        }
        .msg
        {
        }
        .msg span
        {
            color: Red;
        }
        .hasDatepicker::-ms-clear
        {
            display: none;
        }
        .text_align_right
        {
            text-align: right;
        }
        .new-batch
        {
            background-color: white;
            height: 32px;
            line-height: 32px;
            margin: 0 auto;
            text-align: center;
            width: 92%;
        }
        .new-batch div
        {
            color: red;
            font-size: 12px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <div class="msg">
            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
        </div>
        <div class="content">
            <div class="scan-qc">
                Scan QC
            </div>
            <div class="table_cont">
                <table>
                    <tr>
                        <td class="label">
                            Doc ID
                        </td>
                        <td class="field">
                            <asp:Label ID="lblDocID" runat="server"></asp:Label>
                        </td>
                        <td class="label">
                            Batch Type
                        </td>
                        <td class="field">
                            <asp:DropDownList ID="ddlBatchType" runat="server" CssClass="ddlBatchType">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            File Name
                        </td>
                        <td class="field">
                            <asp:Label ID="lblFileName" runat="server"></asp:Label>
                        </td>
                        <td class="label">
                            Company
                        </td>
                        <td class="field">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="ddlCompany">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            Batch ID
                        </td>
                        <td class="field">
                            <asp:Label ID="lblBatchID" runat="server"></asp:Label>
                        </td>
                        <td colspan="2" class="link">
                            <a href="JavaScript:void(0);" id="aUpdateAqillaSuppliers">Update Aqilla Suppliers</a>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            Batch Name
                        </td>
                        <td class="field">
                            <asp:Label ID="lblBatchName" runat="server"></asp:Label>
                        </td>
                        <td class="link" colspan="2">
                            <a href="JavaScript:Void(0);" onclick="PopupPage('<%=NewSupplierLink%>', 'NewSupplier', 1200, 600, 'Link1');"
                                id="Link1" target="_top">New Supplier?</a> &nbsp;&nbsp;&nbsp; <a href="JavaScript:Void(0);"
                                    onclick="PopupPage('SupplierMapping.aspx?bcid=<%=buyercompanyid%>&scid=<%=suppliercompanyid%>', 'MapSupplier', 1200, 600, 'Link2');"
                                    id="Link2" target="_top">Supplier Mapping</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content">
            <table class="table-menu" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="tab selected" id="divHeader_clicker">
                            Header
                        </div>
                    </td>
                    <td>
                        <div class="tab not_selected" id="divLineItems_clicker">
                            Line Items
                        </div>
                    </td>
                </tr>
            </table>
            <div class="shown" id="divHeader">
                <table>
                    <tr>
                        <td class="label">
                            Supplier
                        </td>
                        <td colspan="3" class="field1">
                            <asp:TextBox ID="txtSupplier" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="hfSupplierID" runat="server" Value="0" />
                        </td>
                        <td class="tail">
                            <a href="JavaScript:void(0);" id="lnkDefaults">Defaults</a>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            Doc Number
                        </td>
                        <td colspan="3" class="field1">
                            <asp:TextBox ID="txtDocNumber" runat="server"></asp:TextBox>
                        </td>
                        <td class="tail">
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            PO Number
                        </td>
                        <td colspan="3" class="field1">
                            <asp:TextBox ID="txtPONumber" runat="server" CssClass="classPONumber"></asp:TextBox>
                        </td>
                        <td class="tail">
                            <div>
                                <img src="../../images/iInfo.jpg" alt="Info" class="info_icon" />
                                <div class="pop_msg" style="display: none">
                                    PO Number entered here will be automatically applied to any line items where the
                                    PO No. field is blank.
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            PO Supplier
                        </td>
                        <td colspan="3" class="field1">
                            <asp:TextBox ID="txtPOSupplier" runat="server"></asp:TextBox>
                        </td>
                        <td class="tail">
                            <input type="button" id="btnPopModal" value="Find" />
                            <asp:Button ID="btnFind" runat="server" Text="Find" UseSubmitBehavior="false" CssClass="hidden" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            Doc Type
                        </td>
                        <td class="field2">
                            <asp:DropDownList ID="ddlDocType" runat="server">
                                <%--<asp:ListItem Value="" Selected="True">Select Doc Type</asp:ListItem>--%>
                                <asp:ListItem Value="INV">Invoice</asp:ListItem>
                                <asp:ListItem Value="CRN">Credit Note</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="label">
                            Net
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="txtNet" runat="server" class="text_align_right" onkeypress="numaricInputOnly(event);"></asp:TextBox>
                        </td>
                        <td class="tail">
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            Doc Date
                        </td>
                        <td class="field2 cal_img">
                            <asp:TextBox ID="txtDocDate" runat="server"></asp:TextBox>
                        </td>
                        <td class="label">
                            VAT
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="txtVAT" runat="server" class="text_align_right" onkeypress="numaricInputOnly(event);"></asp:TextBox>
                        </td>
                        <td class="tail">
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            Currency
                        </td>
                        <td class="field2">
                            <asp:DropDownList ID="ddlCurrency" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="label">
                            Total
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="txtTotal" runat="server" class="text_align_right" onkeypress="numaricInputOnly(event);"></asp:TextBox>
                        </td>
                        <td class="tail">
                            <asp:Button ID="btnCalc" runat="server" Text="Calc." UseSubmitBehavior="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="">
                            <span class="cur">Account Currency </span>
                        </td>
                        <td colspan="2" class="">
                            <asp:Label ID="lblAccCur" runat="server"></asp:Label>
                        </td>
                        <td class="tail">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" class="hor-sep">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="buttons">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="red-button" UseSubmitBehavior="false" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnDiscard" runat="server" Text="Discard" CssClass="yellow-button"
                                                UseSubmitBehavior="false" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="blue-button" UseSubmitBehavior="false" />
                                        </td>
                                        <td>
                                            <input type="button" value="Process" class="green-button" id="btnProcessValidation" />
                                            <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="hidden" UseSubmitBehavior="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="hor-sep">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="sky-button" UseSubmitBehavior="false" />
                                        </td>
                                        <td colspan="3">
                                            <div class="new-batch" style="display: none">
                                                <div>
                                                    NEW BATCH
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="hor-sep">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="font-size: 10px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lbtnDnldPrev" runat="server" Text="Download previous image"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lbtnDnldThis" runat="server" Text="Download this image"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lbtnDnldNext" runat="server" Text="Download next image"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lbtnAtcPrev" runat="server" Text="Attach to previous"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lbtnSplitAndReprocess" runat="server" Text="Split and re-process this image"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lbtnAtcNext" runat="server" Text="Attach to next"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lbtnOriginalDocument" runat="server" Text="Original Document"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="hidden" id="divLineItems" style="overflow: auto;">
                <asp:GridView ID="gvLists" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                    CssClass="table-list" CellPadding="3" CellSpacing="0" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="PO No." ItemStyle-Width="20%">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPONO" runat="server" Text='<%#Eval("PONO")%>' CssClass="PONO classPONumber fullWidth">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Code" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <%--Modified by Mainak 2017-11-20--%>
                                <asp:TextBox ID="txtBuyerCode" runat="server" Text='<%#(Eval("BUYERCODE").Equals("0.00")? "" : Eval("BUYERCODE"))%>'></asp:TextBox>
                                <%--Ended by Mainak 2017-11-20--%>
                                <asp:Label ID="lblGoodsRecdDetailID" runat="server" Text='<%# Eval("GoodsRecdDetailID")%>'
                                    Visible="false" />
                                <asp:Label ID="lblDeptID" runat="server" Text='<%# Eval("DepartmentID")%>' Visible="false" />
                                <asp:Label ID="lblNominalCodeID" runat="server" Text='<%# Eval("NominalCodeID")%>'
                                    Visible="false" />
                                <asp:Label ID="lblBusinessUnitID" runat="server" Text='<%# Eval("BusinessUnitID")%>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblProjectCode" runat="server" Text='<%# Eval("ProjectCode")%>' Visible="false"></asp:Label><%--Added by Mainak 2017-11-21--%>
                                <asp:Label ID="lblPurOrderLineNo" runat="server" Text='<%# Eval("PurOrderLineNo")%>' Visible="false"></asp:Label><%--Added by Mainak 2018-05-31--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="40%" FooterText="Total">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" Text='<%#Eval("DESC")%>' TextMode="MultiLine"
                                    CssClass="DESC stretch">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty." ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Right" FooterText="0.00">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQTY" runat="server" Text='<%#Eval("QTY")%>' onkeypress="numaricInputOnly(event);"
                                    CssClass="QTY">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Price" ItemStyle-Width="10.5%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPRICE" runat="server" Text='<%#Eval("PRICE")%>' onkeypress="numaricInputOnly(event);"
                                    CssClass="PRICE">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Right" FooterText="0.00">
                            <ItemTemplate>
                                <asp:TextBox ID="txtVALUE" runat="server" Text='<%#Eval("VALUE")%>' onkeypress="numaricInputOnly(event);"
                                    CssClass="VALUE">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VAT" ItemStyle-Width="7.5%" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Right" FooterText="0.00">
                            <ItemTemplate>
                                <asp:TextBox ID="txtVAT" runat="server" Text='<%#Eval("VAT")%>' onkeypress="numaricInputOnly(event);">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectAllH" runat="server" ToolTip="Select All" CssClass="cbSelectAll_H" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" ToolTip="Select" CssClass="cbSelectAll_A" />
                                <asp:TextBox ID="txtPOS" runat="server" Text='<%#Eval("POS")%>' CssClass="hidden"></asp:TextBox>
                                <asp:TextBox ID="txtPAGE" runat="server" Text='<%#Eval("PAGE")%>' CssClass="hidden"></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="cbSelectAllF" runat="server" ToolTip="Select All" CssClass="cbSelectAll_F" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="table-list-header" />
                    <RowStyle CssClass="table-list-item" />
                    <FooterStyle CssClass="table-list-footer" />
                    <%--<AlternatingRowStyle CssClass="table-list-alt-item" />--%>
                </asp:GridView>
                <div class="hor-sep">
                </div>
                <div class="buttons">
                    <table>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnDeleteLines" runat="server" Text="Delete Lines" CssClass="red-button"
                                    UseSubmitBehavior="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnAddLines" runat="server" Text="Add Lines" CssClass="blue-button"
                                    UseSubmitBehavior="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnCalculate" runat="server" Text="Calc." CssClass="yellow-button"
                                    UseSubmitBehavior="False" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="hor-sep">
                </div>
                <div class="add-lines">
                    <span>Add Lines</span>
                    <asp:DropDownList ID="ddlAddLines" runat="server">
                        <asp:ListItem Text="Below" Value="Below"></asp:ListItem>
                        <asp:ListItem Text="Above" Value="Above"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnl1" runat="server">
        <table>
            <tr>
                <td>
                    Valid Supplier:
                </td>
                <td>
                    <asp:TextBox ID="txtValidSupplier" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Value Sum:
                </td>
                <td>
                    <asp:TextBox ID="txtSum" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Is Line Items:
                </td>
                <td>
                    <asp:TextBox ID="txtIsLnItm" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Is PO:
                </td>
                <td>
                    <asp:TextBox ID="txtIsPO" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Is Description:
                </td>
                <td>
                    <asp:TextBox ID="txtIsDesc" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Is Valid PO:
                </td>
                <td>
                    <asp:TextBox ID="txtValidPO" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div style="display: none" id="divTableContainer">
        <div class="popupcontent">
            <%--Table content from ajax call--%>
        </div>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#dataTable1 tr').each(function () {
                    var $td = $(this).children("td");
                    $td.click(function () {
                        var snm = $td.children("span").eq(0).html(); //supplier name
                        var sid = $td.children("span").eq(1).html(); //supplier id
                        var $df = $(document).find('.right');
                        var id = $df.contents();
                        snm = snm.replace(/&amp;/g, '&');
                        //alert(snm);		
                        id.find("#<%=txtSupplier.ClientID%>").val(snm);
                        id.find("#<%=hfSupplierID.ClientID%>").val(sid);
                        id.find("#<%=txtPOSupplier.ClientID%>").val(snm);
                        id.find("#<%=txtValidSupplier.ClientID%>").val('True');
                        $(document).find('.close-modal').click();
                        id.find("#<%=btnFind.ClientID%>").click();
                    });
                });
            });
        </script>
        <style type="text/css">
            .popupcontent
            {
                height: 150px;
                overflow: auto;
                width: 100%;
            }
            #dataTable1
            {
                height: auto;
                width: 100%;
                font-family: verdana,Tahoma,Arial;
                font-size: 12px;
            }
            #dataTable1 tr td
            {
                cursor: pointer;
            }
            .fullWidth
            {
                width: 100%;
            }
        </style>
    </div>
    <div style="display: none" id="divDefaultsModalContent">
        <div id="divContainer">
            <div class="header">
                Supplier Defaults</div>
            <div class="row">
                <div class="label">
                    Buyer Company</div>
                <div class="field">
                    <input type="text" id="txtBuyerCompany" class="type1" value="BuyerCompany1" />
                    <input type="text" id="txtBuyerCompanyID" class="hidden" value="BuyerCompanyID1" />
                </div>
            </div>
            <div class="row">
                <div class="label">
                    Supplier
                </div>
                <div class="field">
                    <input type="text" id="txtSupplierCompany" class="type1" value="SupplierCompany1" />
                    <input type="text" id="txtSupplierCompanyID" class="hidden" value="SupplierCompanyID1" />
                </div>
            </div>
            <div class="row">
                <div class="label">
                    Vendor ID
                </div>
                <div class="field">
                    <input type="text" id="txtVendorID" class="type1" value="VendorID1" />
                </div>
            </div>
            <div class="row">
                <div class="label">
                    Vendor Class
                </div>
                <div class="field">
                    <input type="text" id="txtVendorClass" class="type2" value="VendorClass1" />
                </div>
            </div>
            <div class="row">
                <div class="label">
                    Stock / F&B Nominal
                </div>
                <div class="field">
                    <input type="text" id="txtStockFBNominal" class="type2" value="StockFBNominal1" />
                    <input type="text" id="txtStockFBNominalCode" class="hidden" value="StockFBNominalCode1" />
                </div>
            </div>
            <div class="row">
                <div class="label">
                    Expense Nominal
                </div>
                <div class="field">
                    <input type="text" id="txtExpenseNominal" class="type2" value="ExpenseNominal1" />
                    <input type="text" id="txtExpenseNominalCode" class="hidden" value="ExpenseNominalCode1" />
                </div>
            </div>
            <div class="row">
                <div class="label">
                    Account Currency
                </div>
                <div class="field">
                    <select id="selAccountCurrency" class="type2">
                    </select>
                </div>
            </div>
            <div class="gap">
            </div>
            <div class="buttons">
                <input type="button" id="btnCancel" value="cancel" />
                <input type="button" id="btnSave1" value="save" />
            </div>
        </div>
        <script type="text/javascript">
            //isnumeric
            function isNumeric(val) {
                try {
                    parseInt(val);
                    return true;
                }
                catch (err) {
                    return false;
                }
            }
            //
            $(".type1").attr("readonly", "readonly");
            //
            $("#btnCancel").click(function () {
                $('#DefaultsModal').find('.close-modal').click();
            });
            //
            $("#btnSave1").click(function () {
                var BuyerCompanyID = $('#txtBuyerCompanyID').val();
                var SupplierCompanyID = $('#txtSupplierCompanyID').val();
                var SupplierCompanyName = $('#txtSupplierCompany').val();
                var UserID = '<%=Session["UserID"].ToString()%>';
                var CurrencyTypeID = $('#selAccountCurrency').val();
                var i = 0;
                var c = $('#selAccountCurrency').children('option').length;
                //alert(c);
                var v = "";
                for (i = 0; i < c; i++) {
                    v = $('#selAccountCurrency').children('option').eq(i).attr('value');
                    //alert(v + " = " + CurrencyTypeID);
                    if (v == CurrencyTypeID) {
                        //alert('found ' + i);
                        break;
                    }
                }
                var CurrencyCode = $('#selAccountCurrency').children('option').eq(i).html().trim();
                var VendorID = $('#txtVendorID').val().trim();
                var VendorClass = $('#txtVendorClass').val().trim();
                var Active = 'true';
                var Nominal1 = $('#txtStockFBNominalCode').val().trim();
                var Nominal2 = $('#txtExpenseNominalCode').val().trim();
                var PreApprove = 'false';
                var ApprovalNeeded = '0';

                //alert("Nominal1: " + Nominal1);
                //alert("Nominal2: " + Nominal2);
                //alert("UserID: " + UserID);
                //alert("CurrencyTypeID: " + CurrencyTypeID);
                //alert("CurrencyCode: " + CurrencyCode);

                $.ajax({
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'JSON',
                    url: 'ActionScanQC.aspx/SaveSupplierDefaults',
                    data: '{"BuyerCompanyID":"' + BuyerCompanyID + '", "SupplierCompanyID":"' + SupplierCompanyID + '", "SupplierCompanyName":"' + SupplierCompanyName + '", "UserID":"' + UserID + '", "CurrencyTypeID":"' + CurrencyTypeID + '", "CurrencyCode":"' + CurrencyCode + '", "VendorID":"' + VendorID + '", "VendorClass":"' + VendorClass + '", "Active":"' + Active + '", "Nominal1":"' + Nominal1 + '", "Nominal2":"' + Nominal2 + '", "PreApprove":"' + PreApprove + '", "ApprovalNeeded":"' + ApprovalNeeded + '"}',
                    success: function (msg) {
                        var ret = msg.d.split('^');
                        //alert(ret[0] + ": " + ret[1]);
                        alert(ret[1]);
                        switch (ret[0]) {
                            case "Success":
                                ChangeDefaultsLinkColor();

                                $('#DefaultsModal').find('.close-modal').click();
                                break;
                            case "Failure":
                                break;
                            case "Error":
                                break;
                        }
                    },
                    failure: function (msg) {
                        alert("failure: " + msg.d);
                    },
                    error: function (xhr, err) {
                        //alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
                        //alert("responseText: " + xhr.responseText);
                        //alert("error: " + err);
                        ChangeDefaultsLinkColor();
                        alert("error: " + err + " " + xhr.responseText.split('<br />')[1].split('{')[0]);
                    }
                });
                //$('#DefaultsModal').find('.close-modal').click();
            });
            //
            $('#selAccountCurrency').change(function () {
                var v = $(this).val();
                $(this).children("option").each(function () {
                    if ($(this).attr("value") == v) {
                        //alert("dfd");
                        $(this).attr("selected", "selected");
                    }
                    else {
                        //alert("cfc");
                        $(this).removeAttr("selected");
                    }
                });
            });
            //Populate list of Values
            $("#txtStockFBNominal").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'JSON',
                        url: 'ActionScanQC.aspx/ListNominalName',
                        data: '{"input1":"' + $('#txtBuyerCompanyID').val() + '", "input2":"' + $('#txtStockFBNominal').val().toString().replace("'", "''") + '"}',
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    //label: item
                                    val: item.split('^')[0],
                                    label: item.split('^')[1]
                                }
                            }))
                        },
                        error: function (result) {
                            //alert("No Match Found");
                            $("#txtStockFBNominalCode").val('');
                        }
                    });
                },
                select: function (e, i) {
                    //alert(i.item.val);
                    $("#txtStockFBNominalCode").val(i.item.val);
                },
                open: function () {
                    var $txt = $("#txtStockFBNominal");
                    var w = $txt.width() + 3;
                    var t = $txt.offset().top + $txt.height() + 4;
                    var l = $txt.offset().left;
                    var h = 150;

                    $txt.autocomplete("widget").css("cssText", "height: " + h + "px !important; left: " + l + "px !important; top: " + t + "px !important; width: " + w + "px !important;");
                },
                minLength: 1
            });
            //
            $("#txtStockFBNominal").on('input', function () {
                if ($(this).val().trim().length < 2) {
                    $("#txtStockFBNominalCode").val('');
                }
            });
            //If the value in Stock / F&B Nominal is not from the list 
            //then the entered value should be removed as on 20/Feb/2017.
            $("#txtStockFBNominal").on('blur', function () {
                if ($("#txtStockFBNominalCode").val() == '') {
                    $(this).val('');
                }
            });
            //Populate list of Values
            $("#txtExpenseNominal").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'JSON',
                        url: 'ActionScanQC.aspx/ListNominalName',
                        data: '{"input1":"' + $('#txtBuyerCompanyID').val() + '", "input2":"' + $('#txtExpenseNominal').val().toString().replace("'", "''") + '"}',
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    //label: item
                                    val: item.split('^')[0],
                                    label: item.split('^')[1]
                                }
                            }))
                        },
                        error: function (result) {
                            //alert("No Match Found");
                            $("#txtExpenseNominalCode").val('');
                        }
                    });
                },
                select: function (e, i) {
                    //alert(i.item.val);
                    $("#txtExpenseNominalCode").val(i.item.val);
                },
                open: function () {
                    var $txt = $("#txtExpenseNominal");
                    var w = $txt.width() + 3;
                    var t = $txt.offset().top + $txt.height() + 4;
                    var l = $txt.offset().left;
                    var h = 150;

                    $txt.autocomplete("widget").css("cssText", "height: " + h + "px !important; left: " + l + "px !important; top: " + t + "px !important; width: " + w + "px !important;");
                },
                minLength: 1
            });
            $("#txtExpenseNominal").on('input', function () {
                if ($(this).val().length < 2) {
                    $("#txtExpenseNominalCode").val('');
                }
            });
            //If the value in Expenses Nominal is not from the list 
            //then the entered value should be removed as on 20/Feb/2017.
            $("#txtExpenseNominal").on('blur', function () {
                if ($("#txtExpenseNominalCode").val() == '') {
                    $(this).val('');
                }
            });
            //Set the Defaults Link Colour
            function ChangeDefaultsLinkColor() {
                var AccountCurrency = $('#selAccountCurrency').val().trim();
                var VendorClass = $('#txtVendorClass').val().trim();
                var StockFBNominal = $('#txtStockFBNominalCode').val().trim();
                var ExpenseNominal = $('#txtExpenseNominalCode').val().trim();

                var $df = $(document).find('.right');
                var id = $df.contents();

                //After 08-MAR-2017: if Vendor Class and Account Currency are both populated and at least 1 of the nominal fields is populated then it is blue.
                if (VendorClass.length == 0 || AccountCurrency.length == 0 || (ExpenseNominal.length == 0 && StockFBNominal.length == 0))
                    id.find("#lnkDefaults").css("color", "red");
                else
                    id.find("#lnkDefaults").css("color", "#0081C5");
            }
        </script>
        <style type="text/css">
            #divContainer
            {
                font-family: verdana,Tahoma,Arial;
            }
            #divContainer .header
            {
                font-size: 18px;
                font-weight: bold;
                line-height: 50px;
                color: #3399cc;
            }
            #divContainer .row
            {
                height: 30px;
                line-height: 24px;
                padding-bottom: 2px;
                padding-top: 2px;
                width: 100%;
                display: inline-block;
            }
            #divContainer .row .label
            {
                display: inline-block;
                width: 35%;
                font-size: 10px;
            }
            #divContainer .row .field
            {
                display: inline-block;
                width: 63%;
            }
            #divContainer .row .field input[type='text']
            {
                width: 98%;
                height: 20px;
            }
            #divContainer .row .field select
            {
                width: 100%;
                height: 22px;
            }
            #divContainer .row .field .type1
            {
                border: medium none;
            }
            #divContainer .row .field .type2
            {
                border: 1px solid #ccc;
            }
            #divContainer .gap
            {
                width: 100%;
                height: 20px;
                display: inline-block;
            }
            #divContainer .buttons
            {
                height: 40px;
                line-height: 35px;
                width: 100%;
            }
            #divContainer .buttons input[type='button']
            {
                color: white;
                cursor: pointer;
                font-weight: bold;
                height: 33px;
                text-transform: uppercase;
                width: 115px;
            }
            #divContainer .buttons #btnCancel
            {
                background-color: #ff0000;
                border: 1px solid #ff0000;
                float: left;
            }
            #divContainer .buttons #btnSave1
            {
                background-color: #3cbc3c;
                border: 1px solid #3cbc3c;
                float: right;
            }
            .hidden
            {
                display: none;
                visibility: hidden;
            }
            .shown
            {
                display: block;
                visibility: visible;
            }
        </style>
    </div>
    <style type="text/css">
        .sub_down
        {
            /*width:50% !important;
	        padding-left:15px;
	        padding-right:15px;
	        float:right !important;
	        font-weight:bold !important;
            color:#fff !important;
	
	        min-width:150px;*/
            background-color: #428bca;
            border: 0 none;
            color: #fff !important;
            cursor: pointer;
            float: right;
            font-family: 'franklin_gothic_medium_condRg';
            font-size: 16px;
            font-weight: 400; /*margin-right: 20px;*/
            padding: 6px 30px;
            text-transform: uppercase;
            width: 130px;
            text-decoration: none !important;
            text-align: center !important;
            padding-left: 10px;
            padding-right: 10px;
            margin-right: 16px;
        }
        
        .header-thick
        {
            padding: 6px;
        }
        .footerClass
        {
            border-top: solid;
            border-top-width: 1px;
            border-top-color: inherit;
        }
    </style>
    <div class="modal fade" id="addModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
        aria-hidden="true" style="display: none; width: 80%">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <h1 id="dataToDisplay">
                    </h1>
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
                <div class="row">
                    <div class="PageHeader header-thick">
                        Goods Received / Order Details
                    </div>
                    <div style="width: 100%">
                      <%-- Blocked By sonali 25.6.2018--%>
                        <%--<table style="width: 100%">
                            <tr>
                                <td>
                                    <table cellpadding="10">
                                        <%--Start Change
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>PO Number</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblPO" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>Date</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table cellpadding="10">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>Company</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCompany" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>Currency</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table cellpadding="10">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>Supplier</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSupplier" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>Buyer</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblBuyer" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>--%>
                        <%-- Design Changed By sonali 20.6.2018--%>
                        <table style="width: 100%; margin-top: 10px">
                            <tr cellpadding="10">
                                <td>
                                    <b>PO Number</b>
                                </td>
                                <td>
                                    <b>Company</b>
                                </td>
                                <td>
                                    <b>Supplier</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPO" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCompany" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSupplier" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Date</b>
                                </td>
                                <td>
                                    <b>Currency</b>
                                </td>
                                <td>
                                    <b>Buyer</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblBuyer" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-xs-12 col-sm-12">
                        <div class="form-group form-group2">
                            <div class="">
                                <%--col-xs-12 col-sm-12--%>
                                <div class="row">
                                    <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="false" CssClass="listingArea center-table"
                                        DataKeyNames="GoodsRecdDetailID" ShowFooter="true" GridLines="None" Style="width: 100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Product Code" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProdCode" Text='<%# Bind("ProductCode")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesc" Text='<%# Bind("Description")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right"
                                                FooterStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQty" Text='<%# Bind("Quantity")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblQtySum" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrice" Text='<%# Bind("Rate")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right"
                                                FooterStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblValue" Text='<%# Bind("NetAmt")%>' runat="server"></asp:Label>
                                                    <asp:Label ID="lblBuyerCode" runat="server" Text='<%# Bind("BuyerCode")%>' Visible="false" />
                                                    <asp:Label ID="lblGoodsRecdDetailID" runat="server" Text='<%# Bind("GoodsRecdDetailID")%>'
                                                        Visible="false" />
                                                    <asp:Label ID="lblDeptID" runat="server" Text='<%# Bind("DepartmentID")%>' Visible="false" />
                                                    <asp:Label ID="lblNominalCodeID" runat="server" Text='<%# Bind("NominalCodeID")%>'
                                                        Visible="false" />
                                                    <asp:Label ID="lblBusinessUnitID" runat="server" Text='<%# Eval("BusinessUnitID")%>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProjectCode" runat="server" Text='<%# Eval("ProjectCode")%>' Visible="false"></asp:Label><%--Added by Mainak 2017-11-21--%>
                                                    <asp:Label ID="lblPurOrderLineNo" runat="server" Text='<%# Eval("PurOrderLineNo")%>' Visible="false"></asp:Label><%--Added by Mainak 2018-05-31--%>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblValueSum" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="10%">
                                                <HeaderTemplate>
                                                    <input type="checkbox" class="check-header" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:CheckBox ID="chkProdCode" runat="server" CssClass="prodCodeSelector"/>--%>
                                                    <input type="checkbox" class="prodCodeSelector check-all" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <input type="checkbox" class="check-header" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="row-style"></RowStyle>
                                        <HeaderStyle CssClass="tableHd head-style"></HeaderStyle>
                                        <FooterStyle CssClass="footerClass"></FooterStyle>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <div>
                                    <asp:Button ID="btnAddModal" runat="server" Text="ADD" CssClass="sub_down" UseSubmitBehavior="false"
                                        OnClientClick="getSelectedRowNo();" />
                                    <asp:Button ID="btnReplaceModal" runat="server" Text="REPLACE" CssClass="sub_down"
                                        UseSubmitBehavior="false" OnClientClick="getSelectedRowNo();" />
                                </div>
                            </td>
                        </tr>
                        <%--<br />--%>
                        <tr>
                            <td>
                                <div style="color: red;">
                                    ADD will add these items onto the existing line items on the previous page
                                    <br />
                                    REPLACE will replace the existing line items on the previous page with those selected
                                    here
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
    <asp:Button ID="btnOpenModal" runat="server" OnClick="btnOpenModal_Click" UseSubmitBehavior="false" />
    <asp:HiddenField ID="hdnPurOrderNo" runat="server" />
    <asp:HiddenField ID="hdnRowNo" runat="server" />
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%=btnOpenModal.ClientID%>").style.display = "none";
        });

        $(".classPONumber").on('dblclick', function (e) {
            //$('#addModal').modal('show');
            $(".modal-body #dataToDisplay").text($(e.target).val());
            var poNo = $(e.target).val();
            if (poNo != "") {
                $('#<%=hdnPurOrderNo.ClientID%>').val(poNo);
                $('#<%=btnOpenModal.ClientID%>').click();
                //return false;
                //e.preventDefault();
                //return false;
            }
            else {
                alert("Please select a 'PO Number' before double click");
            }
        });


        function getSelectedRowNo() {
            openLineItem();
            var lastVal = "";
            var count = 0;
            $('.prodCodeSelector').each(function () {
                if (this.checked) {
                    lastVal += count + ',';
                }
                count++;
            });
            $("#<%=hdnRowNo.ClientID%>").val(lastVal);
        }

        $(".check-header").change(function (e) {
            var tf = $(this).is(':checked');
            $(".check-all").prop('checked', tf);
            $(".check-header").prop('checked', tf);
        });

    </script>
</body>
</html>
