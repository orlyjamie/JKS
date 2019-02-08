<%@ Page Language="c#" CodeBehind="InvView.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.invoice.InvView" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>InvView</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="InvView.css" type="text/css" rel="stylesheet"></link>
    <script language="JScript" src="InvuNumbers.js"></script>
    <script language="JScript" src="Utility.js"></script>
    <script>

        var currentTab = "";
        var DataTbl;
        var item_value_object = null;

        var HighlightBoxSelections;
        //</LINK>
        function window.ondeactivate() {
            window.alert("ondeactivate");
        }

        function after_timeout() {
            window.alert("wait over");

        }

        function now_closing_down() {
            //			window.alert("a message");
            Form1.pbreallyclosing.click();
        }

        function outer_closing() {
            Form1.pbClose1.click();
        }

        function outer_closing2() {
            //			window.alert("from outer frame");
            var s = "After The Invoice Extraction Details that you may have edited have not been saved.\nClick OK to save them (not to Archive) before Quitting.\nOtherwise click Cancel";
            if (window.confirm(s) == false) {
                window.alert("about to call pbClose1.click");
                //				Sleep(5000);
                window.setTimeout(window.alert("after_timeout"), 5000, "JScript");

                //				Form1.pbClose1.click();	

            }
            else {
                //				window.alert("doing the else");
                var warning_count = Form1.TextBoxGlobals.value;
                if (warning_count == "2") {
                    window.alert("This invoice has already been stored in the Permanent Repository or Deleted. Load another.");
                }
                else {
                    Form1.TextBoxGlobals.value = "";
                    Form1.TextBoxGlobals2.value = "";

                    var lines = DataTbl.getAttribute("ImgLines");
                    DataTbl.value = lines;
                    //Form1.submit();
                    Form1.pbSave.click();
                    window.parent.ImgToolbar.doc_opened = "no";
                }
            }
        }


        function window.onunload() {
            //			if(window.ImgToolbar.doc_opened=="yes")
            //				window.alert("closing invView");


        }

        function GetCookie(sName) {
            var aCookie = document.cookie.split("; ");
            for (var i = 0; i < aCookie.length; i++) {
                var aCrumb = aCookie[i].split("=");
                if (sName == aCrumb[0])
                    return unescape(aCrumb[1]);
            }
            return null;
        }


        function init() {
            DataTbl = document.getElementById('tbData');
            //alert(DataTbl.getAttribute("ImgPage"));
            var tab_choice = GetCookie("Tabchoice");
            //			window.alert(tab_choice);
            if (tab_choice == null) {
                tab_choice = "Summary";
                SetCookie("Tabchoice", tab_choice);
            }
            setViewTab(tab_choice);
            var type = DataTbl.getAttribute("ImgType");
            var docid = DataTbl.getAttribute("ImgDoc");
            var path = DataTbl.getAttribute("ImgPath");
            var page = DataTbl.getAttribute("ImgPage");
            var pages = DataTbl.getAttribute("ImgPages");

            window.parent.setInvoiceView(docid, page, pages, type, path);
            var AddDiv = document.getElementById("divAddresses");
            var ShowAddresses;
            //			window.alert("In View");
            SetCookie("DocSelection", "2");
            ShowAddresses = GetCookie("Addresses");
            if (ShowAddresses != null) {
                if (ShowAddresses == "0")
                    AddDiv.style.display = '';
                else AddDiv.style.display = 'none';
            }
            else AddDiv.style.display = '';
            var lines = DataTbl.getAttribute("ImgLines");
            DataTbl.value = lines;
            var width = DataTbl.getAttribute("ImgWidth");
            var height = DataTbl.getAttribute("ImgHeight");
            document.getElementById("Summary").style.position = "absolute";
            document.getElementById("Details").style.position = "absolute";
            document.body.scroll = "no";
            window.onresize();

            /*
            window.alert("Invoice loaded: \ntype = " + type + "\ndocid=" + docid 
            + "\npath=" + path + "\npage=" + page + "\npages=" + pages
            + "\nlines=" + lines + "\nwidth=" + width + "\nheight=" + height);
            */
        }

        function setViewTab(tab) {
            if (currentTab != "")
                hideObj(currentTab);
            currentTab = tab;
            showObj(tab);
            sizeToBody(tab);
            //window.parent.InvTabs.tabSet(tab);	//sourayan 30-09-2009


        }



        function window.onresize() {
            if (currentTab != "")
                sizeToBody(currentTab);
        }

        function hideObj(objId) {
            document.getElementById(objId).style.dispaly = 'none';
            document.getElementById(objId).style.visibility = 'hidden';
        }
        function showObj(objId) {
            document.getElementById(objId).style.display = "";
            document.getElementById(objId).style.visibility = "visible";
        }
        function sizeToBody(objId) {
            document.getElementById(objId).style.width = document.body.clientWidth;
            document.getElementById(objId).style.height = document.body.clientHeight;
            /*
            var bw = document.body.clientWidth;
            var bh = document.body.clientHeight;
            var ol = document.getElementById(objId).style.left;
            var ot = document.getElementById(objId).style.top;
            window.alert("size to bw=" + w + " bh=" + h  + "\not=" + ot + " ol=" + ol);
			
            var w = parseInt(document.body.clientWidth) - parseInt(document.getElementById(objId).style.left);
            var h = parseInt(document.body.clientHeight) - parseInt(document.getElementById(objId).style.top);
            window.alert("size to w=" + w + " h=" + h);
            document.getElementById(objId).style.width = parseInt(document.body.clientWidth) - parseInt(document.getElementById(objId).style.left);
            document.getElementById(objId).style.height = parseInt(document.body.clientHeight) - parseInt(document.getElementById(objId).style.top);	
            */
        }

        function remove_spaces() {
            var string2 = remove_spaces.arguments[0];
            var string3 = ""
            for (var j = 0; j < string2.length; j++) {
                var achar = string2.charAt(j);
                if (achar == ' ')
                    achar = '%20';
                string3 += achar;

            }
            return (string3);
        }

        // Tony's imported functions.
        function EventRow() {
            return GetContainerElement(window.event.srcElement, "TR");
        }

        function ElementCell(el) {
            return GetContainerElement(el, "TD");
        }

        function ElementRow(el) {
            return GetContainerElement(el, "TR");
        }

        function GetContainerElement(el, type) {
            var e = el;
            if (type != undefined && type != "")
                while (e != null && e.tagName != type) e = e.parentElement;
            else
                e = e.parentElement;
            return e;
        }

        function ElementParent(el, type) {
            var e = el.parentElement;
            if (type != undefined && type != "")
                while (e != null && e.tagName != type) e = e.parentElement;
            return e;
        }

        function firstChild(el, type) {
            if (el.children.length > 0) {
                if (type == undefined || type == '') return el.children(0);
                for (var i = 0; i < el.children.length; i++)
                    if (el.children(i).tagName == type) return el.children(i);
            }
            return null;
        }

        function Hide(el) {
            el.style.display = "none";
        }

        function Show(el) {
            el.style.display = "";
        }

        // End of Tony's imported functions.

        function key_down() {
            //			window.event.Cancel=true;
            //			return;
            //			window.alert("start of key_down");
            //			window.alert(window.event.keyCode);
            //			window.event.returnValue=false;
            //			window.alert(window.event.returnValue);
            if (window.event.keyCode == 13) {
                //				window.event.cancelBubble=true;
                window.event.returnValue = false;
                //				window.alert("is equal to enter");
            }
            else {
                //				window.event.cancelBubble=false;
                window.event.returnValue = true;
                //				window.alert("is not equal to enter");
            }
            //			window.parent.Invoice.update_tots_item_inserts();
            //			return;
        }

        function key_down_details() {
            //			window.alert("start of key_down_details");
            //			window.alert(window.event.keyCode);
            var obj = window.event.srcElement;
            var astring = obj.id;
            //			window.alert(astring);
            //			string astring=obj.ToString();
            //			window.alert(astring);
            if (window.event.keyCode == 13 && astring != "_iddesc_0") {
                window.event.returnValue = false;
                //				window.alert("cancel character");
            }
            else {
                window.event.returnValue = true;
                //				 window.alert("do not cancel character");
            }
            //			window.parent.Invoice.update_tots_item_inserts();
            return;
        }


        function onDoubleClick() {
            //			window.alert("start of doubleclick");
            var idnum = onDoubleClick.arguments[0]
            var txtbxname = "";
            //			document.all.InvoiceHeader.rows(1).cells(0).tbInvoiceNo.Value="test";
            //			var norows = document.all.InvoiceHeader.rows.length;
            //			var norows=5;
            //			window.alert(document.all.InvoiceHeader.rows(1).cells(1).innerHTML);	
            //			window.alert(document.all.InvoiceHeader.rows(1).cells(1).firstChild.value);	
            //			document.all.InvoiceHeader.rows(1).cells(1).firstChild.value="1234";			

            //			Form1.InvoiceHeader.tbInvoiceNo.Value="test";
            //			document.all.InvoiceHeader.rows(1).cells(1).innerHTML.value="1234";
            //			return;

            var object1;
            var object2;
            if (idnum == 20) {
                var cell = GetContainerElement(window.event.srcElement, "TD")
                var row = EventRow();
                var no_children = row.children.length;
                object1 = row.children(1);
                var no_children2 = object1.children.length;
                //				window.alert(no_children2);
                object2 = object1.children(0);
                if (object1 != cell)
                    return; // if not column 2
                //				object2.value="xxx";

                //				window.alert(object2.tagName);
                //				object1.text="xxxx";
                //				window.alert(no_children);		
                //				window.alert("in onDoubleClick");
            }


            var in_string = "";
            switch (idnum) {
                case 0: txtbxname = "tbType";
                    in_string = document.all.InvoiceHeader.rows(0).cells(1).firstChild.value; break;
                case 1: txtbxname = "tbInvoiceNo";
                    in_string = document.all.InvoiceHeader.rows(1).cells(1).firstChild.value; break;
                case 2: txtbxname = "tbInvoiceDate";
                    in_string = document.all.InvoiceHeader.rows(2).cells(1).firstChild.value; break;
                case 3: txtbxname = "tbTaxPointDate";
                    in_string = document.all.InvoiceHeader.rows(3).cells(1).firstChild.value; break;
                case 4: txtbxname = "tbOrderNo";
                    in_string = document.all.InvoiceHeader.rows(4).cells(1).firstChild.value; break;
                case 5: txtbxname = "tbVendorRef";
                    in_string = document.all.InvoiceHeader.rows(5).cells(1).firstChild.value; break;
                case 6: txtbxname = "tbSupplierCode";
                    in_string = document.all.InvoiceHeader.rows(0).cells(3).firstChild.value; break;
                case 7: txtbxname = "tbCurrency";
                    in_string = document.all.InvoiceHeader.rows(1).cells(3).firstChild.value; break;
                case 8: txtbxname = "tbNet";
                    in_string = document.all.InvoiceHeader.rows(2).cells(3).firstChild.value; break;
                case 9: txtbxname = "tbDiscount";
                    in_string = document.all.InvoiceHeader.rows(3).cells(3).firstChild.value; break;
                case 10: txtbxname = "tbVat";
                    in_string = document.all.InvoiceHeader.rows(4).cells(3).firstChild.value; break;
                case 11: txtbxname = "tbTotal";
                    in_string = document.all.InvoiceHeader.rows(5).cells(3).firstChild.value; break;
                case 12: txtbxname = "tbPONo";
                    in_string = document.all.InvoiceHeader.rows(6).cells(1).firstChild.value; break;
                case 20: txtbxname = "ItemTable";
                    in_string = ""; break;
            }
            //			window.alert(txtbxname);
            var mystring = "?textbox=" + into_url_escape_codes(txtbxname) + "&instring=" + into_url_escape_codes(in_string);
            var retVal;
            //			mystring2=remove_spaces(mystring);			
            if (idnum > 5)
                retVal = window.showModalDialog("PopUpListing.aspx" + mystring, window, "dialogHeight: 530px; dialogWidth: 450px; dialogTop: 10px; dialogLeft: 50px; edge: Raised; center: No; dialogHide: Yes; help: No; resizable: Yes; status: No;");
            else retVal = window.showModalDialog("PopUpListing.aspx" + mystring, window, "dialogHeight: 530px; dialogWidth: 450px; dialogTop: 10px; dialogLeft: 550px; edge: Raised; center: No; dialogHide: Yes; help: No; resizable: Yes; status: No;");
            //			Form1.InvoiceHeader.tbInvoiceNo.Value=retVal;
            if (retVal == "")
                return;
            switch (idnum) {
                case 0: document.all.InvoiceHeader.rows(0).cells(1).firstChild.value = retVal; break;
                case 1: document.all.InvoiceHeader.rows(1).cells(1).firstChild.value = retVal; break;
                case 2: document.all.InvoiceHeader.rows(2).cells(1).firstChild.value = retVal; break;
                case 3: document.all.InvoiceHeader.rows(3).cells(1).firstChild.value = retVal; break;
                case 4: document.all.InvoiceHeader.rows(4).cells(1).firstChild.value = retVal; break;
                case 5: document.all.InvoiceHeader.rows(5).cells(1).firstChild.value = retVal; break;
                case 6: document.all.InvoiceHeader.rows(0).cells(3).firstChild.value = retVal; break;
                case 7: document.all.InvoiceHeader.rows(1).cells(3).firstChild.value = retVal; break;
                case 8: document.all.InvoiceHeader.rows(2).cells(3).firstChild.value = retVal; break;
                case 9: document.all.InvoiceHeader.rows(3).cells(3).firstChild.value = retVal; break;
                case 10: document.all.InvoiceHeader.rows(4).cells(3).firstChild.value = retVal; break;
                case 11: document.all.InvoiceHeader.rows(5).cells(3).firstChild.value = retVal; break;
                case 12: document.all.InvoiceHeader.rows(6).cells(1).firstChild.value = retVal; break;
                case 20: object2.value = retVal; break;
            }
        }


        function onItemFocus(value) {
            var HighlightBoxSelections = GetCookie("HighlightBoxSelections");
            if (HighlightBoxSelections == true)
                return;
            var cell = GetContainerElement(window.event.srcElement, "TD")
            var row = EventRow();
            var no_children = row.children.length;
            //				window.alert(no_children);
            object1 = row.children(5);
            if (object1 == cell) {
                var no_children2 = object1.children.length;
                //				window.alert(no_children2);
                object2 = object1.children(0);
                //				cell.border=4;	
                //				for(j=0; j<object2.attributes.length; j++)
                //					window.alert(object2.attributes(j).name);	
                //				object2.height=	object2.height*1.2;
                object2.style.border = "solid medium lime";

                if (item_value_object != null)
                    item_value_object.style.border = "solid thin blue";
                item_value_object = object2;

                //				object2.borderColor=red;	
                //				cell.borderColor=red;
                //				window.alert(object2.type);
                //				window.alert(object2.value);
                //				object2.color=red;				
                ////				if(object1!=cell)
                //					return; // if not column 2
                //				object2.value="xxx";

                //				window.alert(object2.tagName);
                //				object1.text="xxxx";
                //				window.alert(no_children);		
                //				window.alert("in onDoubleClick");
            }


            //  if (window.event.srcElement.isTextEdit) 
            //     oSource = window.event.<STRONG>srcElement</STRONG>.parentTextEdit;

            window.parent.onObjFocus(window.event.srcElement, DataTbl);
            return;
        }

        function into_url_escape_codes(instring) {
            var string2 = into_url_escape_codes.arguments[0];
            var string3 = "";
            var sub;
            for (var j = 0; j < string2.length; j++) {
                var achar = string2.charAt(j);
                sub = achar;
                switch (achar) {
                    case ' ': sub = '%20'; break;
                    case '<': sub = '%3C'; break;
                    case '>': sub = '%3E'; break;
                    case '#': sub = '%23'; break;
                    case '%': sub = '%25'; break;
                    case '{': sub = '%7B'; break;
                    case '}': sub = '%7D'; break;
                    case '|': sub = '%7C'; break;
                    case '\\': sub = '%5C'; break;
                    case '^': sub = '%5E'; break;
                    case '~': sub = '%7E'; break;
                    case '[': sub = '%5B'; break;
                    case ']': sub = '%5D'; break;
                    case '`': sub = '%60'; break;
                    case ';': sub = '%3B'; break;
                    case '/': sub = '%2F'; break;
                    case '?': sub = '%3F'; break;
                    case ':': sub = '%3A'; break;
                    case '@': sub = '%40'; break;
                    case '=': sub = '%3D'; break;
                    case '&': sub = '%26'; break;
                    case '$': sub = '%24'; break;
                }
                string3 += sub;

            }
            return (string3);
        }

        function update_tots_item_inserts() {
            //			return;
            if (Form1.TextBox3.value != "" && Form1.tbSupplierCode.value != "") {
                if (Form1.TextBox3.value == "*") {
                    var s1 = "The asterisk in   Map to new Supplier   will list all mappings in the database.\n Click OK if this is what you want, or else Cancel, clear the box, and Update again.";
                    if (window.confirm(s1) == false)
                        return;
                }
                else {
                    var s = "A non-empty box for   Map to new Supplier   will record a mapping in the database.\n Click OK if this is what you want, or else Cancel, clear the box, and Update again.";
                    if (window.confirm(s) == false)
                        return;
                }

                var mystring = "?lhs=" + into_url_escape_codes(Form1.TextBox3.value) + "&rhs=" + into_url_escape_codes(Form1.tbSupplierCode.value);
                var retVal;
                //				window.alert(mystring);
                //				var with_escapes=into_url_escape_codes(mystring);
                //				window.alert(with_escapes);
                retVal = window.showModalDialog("SupplierMappingsDlg.aspx" + mystring, window, "dialogHeight: 470px; dialogWidth: 400px; dialogTop: 10px; dialogLeft: 150px; edge: Raised; center: No; dialogHide: Yes; help: No; resizable: No; status: No;");
                Form1.TextBox3.value = "";
                return;

            }
            else
                if (Form1.TextBox3.value != "" && Form1.tbSupplierCode.value == "") {
                    window.alert("You have entered a string for    Map to new Supplier\nbut you must also add a SupplierCode from the drop-down.");
                    return;
                }

            var warning_count = Form1.TextBoxGlobals.value;
            if (warning_count == "2") {
                window.alert("You are wasting your time editing.\nThis invoice has already been stored in the Permanent Repository or Deleted. Load another.");
            }
            else if (warning_count == "1")
                Form1.TextBoxGlobals.value = "";
            else {
                var lines = DataTbl.getAttribute("ImgLines");
                DataTbl.value = lines;
                //Form1.submit();
                Form1.TextBoxGlobals2.value = "1";
                Form1.pbUpdate.click();
            }
        }

        function start_invoice_dialog_box_again() {
            var ibatch = window.parent.ImgToolbar.sbatch;
            var idivision = window.parent.ImgToolbar.sdivision;
            var itype = window.parent.ImgToolbar.stype;
            //				window.alert(ibatch);
            var mystring = "?batch=" + into_url_escape_codes(ibatch) + "&division=" + into_url_escape_codes(idivision) + "&type=" + into_url_escape_codes(itype);
            //			mystring2=remove_spaces(mystring);
            //			window.alert(mystring2);
            mystring2 = mystring;
            var myObject = new Object();
            myObject.batch = ibatch;
            myObject.division = idivision;
            myObject.type = itype;
            //			var height=window.screenTop;
            var height = screen.availHeight - 10;
            var width = screen.availWidth;
            var x_dis = Math.floor(width * 0.56);
            //			window.alert(x_dis);
            var dialog_left = x_dis.toString(10) + "px;";
            var dialog_height = height.toString(10) + "px;";
            //			window.alert(dialog_height);
            var parameter_string = "dialogHeight: " + dialog_height + " dialogWidth: 280px; dialogTop: 10px; dialogLeft: " + dialog_left + " edge: Raised; center: No; help: No; resizable: No; status: No; "
            //			window.alert(parameter_string);
            //			var retVal=window.showModalDialog("InvSelectDlg.aspx"+mystring, myObject, "dialogHeight: 710px; dialogWidth: 280px; dialogTop: 30px; dialogLeft: 270px; edge: Raised; center: Yes; help: No; resizable: No; status: No; scroll: No;");
            //			window.alert("About to open dialog box");
            //			Session["doc_load_stage"]="doc_saved";
            var retVal = window.showModalDialog("InvSelectDlg.aspx" + mystring2, myObject, parameter_string);
            //			window.setTimeout("window.showModalDialog("InvSelectDlg.aspx"+mystring2, myObject, parameter_string);",2000);

            //			var retVal=window.showModalDialog("InvSelectDlg.aspx"+mystring, myObject, "dialogHeight: 700px; dialogWidth: 270px; dialogTop: 100px; dialogLeft: 270px; edge: Raised; center: Yes; help: No; resizable: No; status: No; scroll: No;");
            //			window.alert(retVal);

            if (retVal && retVal > 0) {
                //					window.alert("In save");
                window.parent.setInvoice(retVal);
                window.parent.ImgToolbar.doc_opened = "yes";
            }

        }

        function determine_if_supplier_ok() {
            var sup_name = Form1.tbSupplierCode.value;
            //			window.alert(sup_name);
            var is_phone_number = true;
            var is_vat_reg_no = true;
            var is_email_address = 0;
            var is_url = false;
            var length = sup_name.length;
            var a_char;
            var telephones = "0123456789() +";
            var found_one = false;

            if (sup_name == "")
                return (false);
            if (sup_name == "NOT FOUND")
                return (false);
            if (sup_name == "NOT_FOUND")
                return (false);

            for (j = 0; j < length; j++) {
                a_char = sup_name.charAt(j);
                //				window.alert(a_char);
                found_one = false;
                for (k = 0; k < 13; k++)
                    if (a_char == telephones.charAt(k)) {
                        found_one = true;
                        break;
                    }
                if (found_one == false)
                    is_phone_number = false;

                if (j > 1) // first 2 chars vat reg no can be upper case chars.
                {
                    found_one = false;
                    for (k = 0; k < 13; k++)
                        if (a_char == telephones.charAt(k)) {
                            found_one = true;
                            break;
                        }
                    if (found_one == false)
                        is_vat_reg_no = false;
                }
                if (is_email_address == 1 && a_char == '.')
                    is_email_address = 2;

                if (is_email_address == 0 && a_char == '@')
                    is_email_address = 1;


            }
            if (is_email_address == 2 || is_vat_reg_no == true || is_phone_number == true)
                return (false);
            if (sup_name.search("www") != -1 && (sup_name.search(".com") != -1
			    || sup_name.search(".co") != -1))
                return (false);

            return (true);
        }

        function check_balance_and_message() {
            object3 = Details.children(0);
            object4 = object3.children(0);
            var no_children = object4.children.length;
            object5 = object4.children(no_children - 1);
            object6 = object5.children(2);
            object7 = object6.children(0);
            x_string = object7.getAttribute("value");
            y_string = document.all.InvoiceHeader.rows(2).cells(3).firstChild.value;
            x_no_comma = "";
            y_no_comma = "";
            for (j = 0; j < x_string.length; j++) {
                ch = x_string.charAt(j);
                if (ch != ',')
                    x_no_comma = x_no_comma + ch;
            }

            for (j = 0; j < y_string.length; j++) {
                ch = y_string.charAt(j);
                if (ch != ',')
                    y_no_comma = y_no_comma + ch;
            }
            //			window.alert(x_no_comma);
            //			window.alert(y_no_comma);	
            if (x_no_comma != y_no_comma) {
                choice = window.confirm("The Summary Net does not equal the Detail Total.\n"
						+ "Do you want to Save the invoice in this state?");

                return choice;
            }
            return true;

            //			my_string=object6.outerHTML;
            //			window.alert(x_no_comma);
            //			window.alert(y_no_comma);
            //			window.alert(no_children);
            //			var total=0.0;
            //			for(j=0; j<no_children; j++)
            //			{
            //				object1=Details.children(j);
            //				var amount_string=object1.innerHTML;			
            //				window.alert(amount_string);		
            //		object2=object1.children(5);
            //				        in_string=document.all.InvoiceHeader.rows(2).cells(1).firstChild.value; break;
            //			}
            //			object1=row.children(1);
            //			var no_children2=object1.children.length;
            //				window.alert(no_children2);
            //			object2=object1.children(0);
            //			if(object1!=cell)
            //				return; // if not column 2		
            //			var row=Details.firstChild;
            //			window.alert(row.Value);
        }


        function save() {
            //			window.alert("blue button pressed");

            if (window.parent.ImgToolbar.doc_opened == "no") {
                window.alert("No invoice is currently open.\nUse the Dis button to open one, or open a new Batch");
                return;
            }

            if (check_balance_and_message() == false)
                return;


            var choice = false;
            if (Form1.TextBox3.value != "" && Form1.tbSupplierCode.value != "") {
                if (Form1.TextBox3.value == "*") {
                    var s1 = "The asterisk in   Map to new Supplier   will list all mappings in the database.\n Click OK if this is what you want, or else Cancel, clear the box, and save again.";
                    if (window.confirm(s1) == false)
                        return;
                }
                else {
                    var s = "A non-empty box for   Map to new Supplier   will record a mapping in the database.\n Click OK if this is what you want, or else Cancel, clear the box, and save again.";
                    if (window.confirm(s) == false)
                        return;
                }

                var mystring = "?lhs=" + into_url_escape_codes(Form1.TextBox3.value) + "&rhs=" + into_url_escape_codes(Form1.tbSupplierCode.value);
                var retVal;
                //			mystring2=remove_spaces(mystring);			
                retVal = window.showModalDialog("SupplierMappingsDlg.aspx" + mystring, window, "dialogHeight: 470px; dialogWidth: 400px; dialogTop: 10px; dialogLeft: 150px; edge: Raised; center: No; dialogHide: Yes; help: No; resizable: No; status: No;");
                Form1.TextBox3.value = "";
                return;

            }
            else
                if (Form1.TextBox3.value != "" && Form1.tbSupplierCode.value == "") {
                    window.alert("You have entered a string for    Map to new Supplier\nbut you must also add a SupplierCode from the drop-down.");
                    return;
                }

            //			window.alert(Form1.TextBox3.value);

            //			close_validation();
            //			return;
            var warning_count = Form1.TextBoxGlobals.value;
            var warning_count2 = Form1.TextBoxGlobals2.value;
            var resolved = Form1.TextBoxResolved.value;
            var supplier_ok = determine_if_supplier_ok();

            if (warning_count == "2") {
                window.alert("This invoice has already been stored in the Permanent Repository or Deleted. Load another.");
                return;
            }
            if (false && warning_count2 != "1" && resolved == "no")	//JCB Jan 20 09
            {
                choice = window.confirm("The yellow button has not been pressed in order to recalculate the values.\nAre you sure you wish to continue?. If so, press OK otherwise Cancel.");
                if (choice == false)
                    return;
            }
            if (supplier_ok == false) {
                choice = window.confirm("The Supplier name is either blank or an unlikely value.\nClick Cancel to go back and correct it.\nOtherwise Click OK to Save invoice as is.");
                if (choice == false)
                    return;
            }

            Form1.TextBoxGlobals.value = "";
            Form1.TextBoxGlobals2.value = "";

            var lines = DataTbl.getAttribute("ImgLines");
            DataTbl.value = lines;
            //Form1.submit();
            //			window.alert("about to Form1.pbSave.click()");
            Form1.pbSave.click();
            window.parent.ImgToolbar.doc_opened = "no";

            var d = new Date();
            var secs = d.getSeconds();
            //			window.alert(secs);
            for (var i = 0; i < 100000; i++) {
                var d2 = new Date();
                var secs2 = d2.getSeconds();
                if ((secs2 - secs) > 1)
                    break;
                if ((secs - secs2) > 1)
                    break;
            }


            //			window.alert("about to start_invoice_dialog_box_again()");
            //			WScript.Sleep(5000);
            //			for (var i=0; i < 10000; i++)
            //			{
            //				var txt=Form1.tbInvoiceNo.value;
            //				window.alert(txt);
            //			}			
            start_invoice_dialog_box_again();
        }

        function discard_invoice() {
            //			window.alert("discarding edits");
            Form1.TextBoxGlobals.value = "";
            Form1.TextBoxGlobals2.value = "";

            var lines = DataTbl.getAttribute("ImgLines");
            DataTbl.value = lines;
            Form1.pbDiscard2.click();
            window.parent.ImgToolbar.doc_opened = "no";

            var d = new Date();
            var secs = d.getSeconds();
            //			window.alert(secs);
            for (var i = 0; i < 100000; i++) {
                var d2 = new Date();
                var secs2 = d2.getSeconds();
                if ((secs2 - secs) > 1)
                    break;
                if ((secs - secs2) > 1)
                    break;
            }



            start_invoice_dialog_box_again();

        }


        function delete_invoice() {
            var s1 = "Are you sure you wish to delete this invoice? If so, press OK, otherwise Cancel.";
            if (window.confirm(s1) == false)
                return;
            var s2 = "Are you sure you wish to delete this invoice? If so, press OK, otherwise Cancel.";
            if (window.confirm(s2) == false) {
                window.alert("Invoice deletion was ABORTED.");
                return;
            }
            Form1.TextBoxGlobals.value = "2";
            Form1.pbDelete.click();
            window.alert("This invoice has been deleted.");
            window.parent.ImgToolbar.doc_opened = "no";
            start_invoice_dialog_box_again();
        }

        function save_permanently() {
            //			window.alert("in save_permanently");

            if (check_balance_and_message() == false)
                return;
            //			window.alert("red button pressed");
            //			window.alert("ok");
            if (Form1.TextBox3.value != "" && Form1.tbSupplierCode.value != "") {
                if (Form1.TextBox3.value == "*") {
                    var s1 = "The asterisk in   Map to new Supplier   will list all mappings in the database.\n Click OK if this is what you want, or else Cancel, clear the box, and save again.";
                    if (window.confirm(s1) == false)
                        return;
                }
                else {
                    var s = "A non-empty box for   Map to new Supplier   will record a mapping in the database.\n Click OK if this is what you want, or else Cancel, clear the box, and save again.";
                    if (window.confirm(s) == false)
                        return;
                }

                var mystring = "?lhs=" + into_url_escape_codes(Form1.TextBox3.value) + "&rhs=" + into_url_escape_codes(Form1.tbSupplierCode.value);
                var retVal;
                retVal = window.showModalDialog("SupplierMappingsDlg.aspx" + mystring, window, "dialogHeight: 470px; dialogWidth: 400px; dialogTop: 10px; dialogLeft: 150px; edge: Raised; center: No; dialogHide: Yes; help: No; resizable: No; status: No;");
                Form1.TextBox3.value = "";
                return;
            }
            else
                if (Form1.TextBox3.value != "" && Form1.tbSupplierCode.value == "") {
                    window.alert("You have entered a string for    Map to new Supplier\nbut you must also add a SupplierCode from the drop-down.");
                    return;
                }
            //			window.alert("ok2");

            var warning_count = Form1.TextBoxGlobals.value;
            var warning_count2 = Form1.TextBoxGlobals2.value;
            var resolved = Form1.TextBoxResolved.value;
            var supplier_ok = determine_if_supplier_ok();
            if (warning_count == "2") {
                window.alert("This invoice has already been stored in the Permanent Repository or Deleted. Load another.");
                return;
            }
            if (false && warning_count2 != "1" && resolved == "no")	//JCB Jan 20 09
            {
                choice = window.confirm("The yellow button has not been pressed in order to recalculate the values.\nAre you sure you wish to continue?. If so, press OK otherwise Cancel.");
                if (choice == false)
                    return;
            }
            if (supplier_ok == false) {
                choice = window.confirm("The Supplier name is either blank or an unlikely value.\nClick Cancel to go back and correct it.\nOtherwise Click OK to Archive invoice as is.");
                if (choice == false)
                    return;
            }
            //			window.alert("Before question");				
            if (window.confirm("Are you sure you wish to release the invoice as completed? If so, press OK otherwise Cancel.") == false)
                return;
            var lines = DataTbl.getAttribute("ImgLines");
            DataTbl.value = lines;
            //Form1.submit();
            Form1.TextBoxGlobals.value = "2";
            Form1.pbPermanentSave.click();
            window.parent.ImgToolbar.doc_opened = "no";

            for (var i = 0; i < 10000; i++) {
                var txt = Form1.tbInvoiceNo.value;
                //				window.alert(txt);
            }
            var d = new Date();
            var secs = d.getSeconds();
            //			window.alert(secs);
            for (var i = 0; i < 100000; i++) {
                var d2 = new Date();
                var secs2 = d2.getSeconds();
                if ((secs2 - secs) > 1)
                    break;
                if ((secs - secs2) > 1)
                    break;
            }
            start_invoice_dialog_box_again();
            return;
        }

        function submit() {
            var lines = DataTbl.getAttribute("ImgLines");
            DataTbl.value = lines;
            //Form1.submit();
            Form1.pbSubmit.click();
        }

        function sumCells1(prefix, target) {
            window.alert("sumCells called for prefix:" + prefix + " target:" + target);
        }
        function sumCells(prefix, target) {
            var obj = window.event.srcElement;
            var n = new Number(obj.value);
            if (isNaN(n.value)) {
                window.alert("Not a number");
            }
            else
                obj.value = n.text;
            var total = 0;
            var i = 0;
            obj = document.getElementById(prefix + i);
            while (obj != null) {
                itemVal = toFloat(obj.value);
                if (!isNaN(itemVal))
                    total = total + itemVal;
                i++;
                obj = document.getElementById(prefix + i);
            }
            document.getElementById(target).value = formatValue(total);
        }

        // Supplier Address
        // Invoice Address
        // Delivery Address
			
    </script>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" scroll="yes" onload="init();"
    rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <div id="Summary" style="display: none; z-index: 102; visibility: hidden; overflow: auto;
        width: 910px; position: static; height: 291px" align="center" ms_positioning="FlowLayout">
        <div style="font-size: 5pt; color: blue">
            <asp:Button ID="Button20" runat="server" Text="Button" Width="0px" Height="5px">
            </asp:Button></div>
        <table id="InvoiceInsert" style="width: 850px; position: relative; height: 30px"
            cellspacing="0" cellpadding="0" width="845" border="0" onkeydown="key_down();">
            <tr>
                <td style="width: 172px; height: 7px" align="right">
                    &nbsp;Document
                </td>
                <td style="width: 208px; height: 7px" align="right">
                    <asp:TextBox ID="TextBox5" runat="server" Width="195px"></asp:TextBox>
                </td>
                <td style="width: 100px; height: 7px" align="right">
                    Mapping
                </td>
                <td style="width: 18px; height: 7px" align="right">
                </td>
                <td style="height: 7px" align="left">
                    <asp:TextBox ID="TextBox3" runat="server" Width="163px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table id="InvoiceHeader" style="width: 850px; position: relative; height: 118px"
            cellspacing="0" cellpadding="0" width="845" border="0" onkeydown="key_down();">
            <tr>
                <td style="width: 172px; height: 7px" align="right">
                    &nbsp;Doc Type
                </td>
                <td style="width: 163px; height: 7px" align="right">
                    <asp:TextBox ID="ddDocType" ondblclick="onDoubleClick(0);" onfocus="onItemFocus();"
                        runat="server" Width="150px"></asp:TextBox>
                </td>
                <td style="width: 143px; height: 7px" align="right">
                    Supplier
                </td>
                <td style="height: 7px" align="left">
                    <asp:TextBox ID="tbSupplierCode" ondblclick="onDoubleClick(6);" onfocus="onItemFocus();"
                        runat="server" Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 172px; height: 29px" align="right">
                    Doc No
                </td>
                <td style="width: 163px; height: 29px" align="right">
                    <asp:TextBox ID="tbInvoiceNo" ondblclick="onDoubleClick(1);" onfocus="onItemFocus();"
                        runat="server" Width="150px" Wrap="False"></asp:TextBox>
                </td>
                <td style="width: 143px; height: 29px" align="right">
                    Currency
                </td>
                <td style="height: 29px" align="left">
                    <asp:TextBox ID="ddCurrency" ondblclick="onDoubleClick(7);" onfocus="onItemFocus();"
                        runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 172px; height: 24px" align="right">
                    Doc Date
                </td>
                <td style="width: 163px; height: 24px" align="right">
                    <asp:TextBox ID="tbInvoiceDate" ondblclick="onDoubleClick(2);" onfocus="onItemFocus();"
                        runat="server" Width="150px"></asp:TextBox>
                </td>
                <td style="width: 143px; height: 24px" align="right">
                    Net
                </td>
                <td style="height: 24px" align="left">
                    <asp:TextBox ID="tbNet" ondblclick="onDoubleClick(8);" Style="text-align: right"
                        onfocus="onItemFocus();" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 172px" align="right">
                    Tax Point
                </td>
                <td style="width: 163px" align="right">
                    <asp:TextBox ID="tbTaxPoint" ondblclick="onDoubleClick(3);" onfocus="onItemFocus();"
                        runat="server" Width="150px"></asp:TextBox>
                </td>
                <td style="width: 143px" align="right">
                </td>
                <! Was Discount>
                <td align="left">
                    <asp:TextBox ID="tbDiscount" ondblclick="onDoubleClick(9);" Style="text-align: right"
                        onfocus="onItemFocus();" runat="server" Width="150px" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 172px" align="right">
                    VAT No.
                </td>
                <td style="width: 163px" align="right">
                    <asp:TextBox ID="tbVendorRef" ondblclick="onDoubleClick(4);" onfocus="onItemFocus();"
                        runat="server" Width="150px"></asp:TextBox>
                </td>
                <td style="width: 143px" align="right">
                    VAT
                </td>
                <td align="left">
                    <asp:TextBox ID="tbVat" ondblclick="onDoubleClick(10);" Style="text-align: right"
                        onfocus="onItemFocus();" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 172px" align="right">
                </td>
                <! Was Vendor Ref>
                <td style="width: 163px" align="right">
                    <asp:TextBox ID="tbVATRegNo" ondblclick="onDoubleClick(5);" onfocus="onItemFocus();"
                        runat="server" Width="150px" Visible="False"></asp:TextBox>
                </td>
                <td style="width: 143px" align="right">
                    Gross
                </td>
                <td align="left">
                    <asp:TextBox ID="tbGross" ondblclick="onDoubleClick(11);" Style="text-align: right"
                        onfocus="onItemFocus();" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 172px" align="right">
                    PO No.
                </td>
                <td style="width: 163px" align="right">
                    <asp:TextBox ID="tbPONo" ondblclick="onDoubleClick(12);" onfocus="onItemFocus();"
                        runat="server" Width="150px"></asp:TextBox>
                </td>
                <td style="width: 163px" align="right">
                </td>
                <td align="left">
                    <asp:TextBox ID="Textbox10" ondblclick="onDoubleClick(11);" Style="text-align: right"
                        onfocus="onItemFocus();" runat="server" Width="150px" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div id="divAddresses">
            <table id="Table1" style="width: 654px; height: 72px" cellspacing="0" cellpadding="0"
                width="654" border="0">
                <tr>
                    <td style="width: 174px; height: 24px" align="right" visible="false">
                    </td>
                    <td style="width: 442px; height: 24px" align="right">
                        <asp:TextBox ID="tbSuppAddress" onfocus="onItemFocus();" runat="server" Width="430px"
                            Visible="false"></asp:TextBox>
                    </td>
                    <td style="height: 24px">
                        <img id="pbAddresses" height="0" alt="" src="bits\PageBlank.png" width="0" visible="false">
                    </td>
                </tr>
                <tr>
                    <td style="width: 174px; height: 24px" align="right" visible="false">
                    </td>
                    <td style="width: 442px; height: 24px" align="right">
                        <asp:TextBox ID="tbInvoiceAddress" onfocus="onItemFocus();" runat="server" Width="430px"
                            Visible="false"></asp:TextBox>
                    </td>
                    <td style="height: 24px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 174px" align="right" visible="false">
                    </td>
                    <td style="width: 442px" align="right">
                        <asp:TextBox ID="tbDelAddress" onfocus="onItemFocus();" runat="server" Width="430px"
                            Visible="false"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="Details" style="display: none; z-index: 103; visibility: hidden; overflow: auto;
        width: 690px; position: static; height: 293px" align="center" ms_positioning="FlowLayout">
        <asp:Table ID="ItemTable" ondblclick="onDoubleClick(20);" runat="server" Width="651px"
            Height="17px" BorderStyle="Outset" GridLines="Both" onkeypress="key_down_details();">
        </asp:Table>
    </div>
    <div style="font-size: 0pt; color: blue">
        <asp:Button ID="pbreallyclosing" runat="server" Text="Button" Width="0px" Height="0px">
        </asp:Button><asp:TextBox ID="TextBox4" runat="server" Width="0px" Height="0px"></asp:TextBox>
        <asp:TextBox ID="TextBoxGlobals" runat="server" Width="0px" Height="0px" Visible="True"></asp:TextBox><asp:TextBox
            ID="TextBoxGlobals2" runat="server" Width="0px" Height="0px" Visible="True"></asp:TextBox><asp:TextBox
                ID="TextBoxResolved" runat="server" Width="0px" Height="8px" Visible="True"></asp:TextBox></div>
    <div id="Analysis" style="display: none; visibility: hidden; overflow: auto; width: 690px;
        height: 98px" align="center" ms_positioning="FlowLayout">
        <asp:Table ID="AnalysisTable" runat="server" Width="651" BorderStyle="Outset" GridLines="Both"
            BackColor="White">
            <asp:TableRow BorderStyle="Outset">
                <asp:TableHeaderCell BackColor="#FFFF33" Width="100px" Text="Cost Centre"></asp:TableHeaderCell>
                <asp:TableHeaderCell BackColor="#FFFF33" Width="100px" Text="Account Code"></asp:TableHeaderCell>
                <asp:TableHeaderCell BackColor="#FFFF33" Width="300px" Text="Description"></asp:TableHeaderCell>
                <asp:TableHeaderCell BackColor="#FFFF33" Width="100px" Text="Amount"></asp:TableHeaderCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <asp:TextBox ID="tbData" Style="z-index: 102; left: 0px; position: absolute; top: 0px"
        runat="server" Width="0px" Height="0px" Wrap="False"></asp:TextBox>
    <asp:TextBox ID="TextboxClerkId" Style="z-index: 102; left: 0px; position: absolute;
        top: 0px" runat="server" Width="0px" Height="0px" Wrap="False"></asp:TextBox>
    <asp:Button ID="pbSubmit" Style="z-index: 101; left: 0px; position: absolute; top: 0px"
        runat="server" Text="Submit" Width="0px" Height="0px"></asp:Button>
    <asp:TextBox ID="Textbox1" Style="z-index: 102; left: 0px; position: absolute; top: 0px"
        runat="server" Width="0px" Height="0px" Wrap="False"></asp:TextBox>
    <asp:Button ID="pbUpdate" Style="z-index: 101; left: 0px; position: absolute; top: 0px"
        runat="server" Text="Update" Width="0px" Height="0px"></asp:Button>
    <asp:Button ID="pbSave" Style="z-index: 101; left: 0px; position: absolute; top: 0px"
        runat="server" Text="Update" Width="0px" Height="0px"></asp:Button>
    <asp:Button ID="pbDiscard2" Style="z-index: 101; left: 0px; position: absolute; top: 0px"
        runat="server" Text="Update" Width="0px" Height="0px"></asp:Button>
    <asp:Button ID="pbPermanentSave" Style="z-index: 101; left: 0px; position: absolute;
        top: 0px" runat="server" Text="Update" Width="0px" Height="0px"></asp:Button>
    <asp:Button ID="pbClose1" Style="z-index: 101; left: 0px; position: absolute; top: 0px"
        runat="server" Text="Update" Width="0px" Height="0px"></asp:Button>
    <asp:Button ID="pbDelete" Style="z-index: 101; left: 0px; position: absolute; top: 0px"
        runat="server" Text="Delete" Width="0px" Height="0px"></asp:Button>
    </form>
</body>
</html>
