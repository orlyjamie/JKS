<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TiffViewerDefault.aspx.cs"
    Inherits="TestTiff.TiffViewerDefault" EnableEventValidation="false" %>

<%@ Register Assembly="AnnotationTiffViewer" Namespace="TiffViewer" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tiff Viewer Annotation </title>
    <asp:Literal ID="dt" runat="server" />
    <link rel="stylesheet" href="css/style.css" type="text/css" media="screen, projection" />
    <link rel="stylesheet" href="css/thickbox.css" type="text/css" />
    <style type="text/css">
        #ctlTiff
        {
            padding: 10px;
            background-color: #DCDCDC;
            display: block;
            border-radius: 10px;
            margin-bottom: 20px;
        }
        
        .button
        {
            border: solid 1px #ccc;
            padding: 5px;
            margin-top: 5px;
        }
        
        body
        {
            font-family: Verdana;
            font-size: 12px;
        }
        
        #divProperties
        {
            font-size: 11px;
            display: none;
        }
        
        #divProperties input[type=text]
        {
            width: 50px;
            font-size: 10px;
        }
        
        div.color_picker
        {
            height: 16px;
            width: 16px;
            padding: 0 !important;
            border: 1px solid #ccc;
            cursor: pointer;
            line-height: 16px;
            float: right;
        }
        
        div#color_selector
        {
            width: 110px;
            position: absolute;
            border: 1px solid #598FEF;
            background-color: #EFEFEF;
            padding: 2px;
            z-index: 9999;
        }
        
        div#color_custom
        {
            width: 100%;
            float: left;
        }
        div#color_custom label
        {
            font-size: 95%;
            color: #2F2F2F;
            margin: 5px 2px;
            width: 25%;
        }
        div#color_custom input
        {
            margin: 5px 2px;
            padding: 0;
            font-size: 95%;
            border: 1px solid #000;
            width: 65%;
        }
        
        div.color_swatch
        {
            height: 12px;
            width: 12px;
            border: 1px solid #000;
            margin: 2px;
            float: left;
            cursor: pointer;
            line-height: 12px;
        }
        
        div.controlset
        {
            display: block;
            padding: 0.25em 0;
        }
        
        #customAnn a
        {
            font-size: 10px;
            white-space: nowrap;
        }
        
        .selectedAnn
        {
            background-image: url(../images/selected.png);
            background-repeat: no-repeat;
            background-position: 50% 50%;
            border: solid 1px Yellow;
        }
        
        #divUpload
        {
            visibility: hidden;
        }
        #nav
        {
            margin-top: 5px;
        }
    </style>
</head>
<body>
    <form id="frmTiff" runat="server">
    <!-- Top menu start -->
    <div style="z-index: 999;">
        <ul id="nav">
            <li><a href="javascript:void(0);">
                <img border="0" title="Fit Type" src="images/fit-type.png" />Fit Type</a>
                <ul>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.SetFitType('FitHeight');">
                        <img border="0" title="Fit Height" src="images/fit-height.png" />Fit Height</a></li>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.SetFitType('FitWidth');">
                        <img border="0" title="Fit Width" src="images/fit-width.png" />Fit Width</a></li>
                </ul>
            </li>
            <li><a href="javascript:void(0);">
                <img border="0" title="Zoom" src="images/zoom.png" />Zoom</a>
                <ul>
                    <%--<li><a href="javascript:void(0);" onclick="objTiffViewer.Zoom(25);">
                        <img border="0" title="Zoom 25%" src="images/percentage.png" />25</a></li>--%>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.Zoom(50);">
                        <img border="0" title="Zoom 50%" src="images/percentage.png" />200</a></li>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.Zoom(75);">
                        <img border="0" title="Zoom 75%" src="images/percentage.png" />300</a></li>
                    <%-- <li><a href="javascript:void(0);" onclick="objTiffViewer.Zoom(100);">
                        <img border="0" title="Actual Size" src="images/actual-size.png" />Actual Size</a></li>--%>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.ZoomInOut(true);">
                        <img border="0" title="Zoom In" src="images/zoom-in.png" />Zoom In</a></li>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.ZoomInOut(false);">
                        <img border="0" title="Zoom Out" src="images/zoom-out.png" />Zoom Out</a></li>
                </ul>
            </li>
            <li><a href="javascript:void(0);">
                <img border="0" title="Rotate" src="images/rotate.png" />Rotate</a>
                <ul>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.RotatePage(90);">
                        <img border="0" title="Rotate 90" src="images/90.png" />90</a></li>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.RotatePage(180);">
                        <img border="0" title="Rotate 180" src="images/180.png" />180</a></li>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.RotatePage(270);">
                        <img border="0" title="Rotate 270" src="images/270.png" />270</a></li>
                </ul>
            </li>
            <li><a href="javascript:void(0);">
                <img border="0" title="Tools" src="images/tools.png" />Tools</a>
                <ul>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.SetToolType('Pan');">
                        <img border="0" title="Pan" src="images/tools-pan.png" />Pan</a></li>
                    <li><a href="javascript:void(0);" onclick="objTiffViewer.SetToolType('Zoom');">
                        <img border="0" title="Zoom" src="images/tools-zoom.png" />Zoom</a></li>
                    <%-- <li><a href="javascript:void(0);" onclick="objTiffViewer.SetToolType('Magnify');">
                        <img border="0" title="Magnify" src="images/tools-magnify.png" />Magnify</a></li>      --%>
                </ul>
            </li>
            <li>&nbsp;&nbsp;</li>
        </ul>
    </div>
    <!-- Top menu end -->
    <div style="z-index: 99; float: left; margin-top: 10px; width: 100%; height: 100%;">
        <asp:TiffViewerControl ID="ctlTiff" runat="server" ImageQuality="100" ViewerHeight="600"
            ThumbHeight="60" ThumbWidth="45" ViewerWidth="715" FitType="FitWidth" CompressBurnedImage="true"
            DefaultZoom="50" ShowThumbNails="true" AnnKeyboardEnabled="false" FastMode="true"
            AnnMouseCursor="images/pen.cur" AnnResizeColor="orange" AnnOnDeleteAnnotation="DeleteFn"
            AnnOnCreateAnnotation="CreateFn" AnnOnDoubleClickAnnotation="PropertiesFn" CacheClear="true"
            CacheForwarding="false" CacheZoom="100" CachePageCount="3" CacheDelay="3" />
        <!-- The main tiff control end -->
        <input type="text" id="debug" style="overflow: scroll; height: 15px; width: 90%;
            display: none;" onclick="this.value = ''" />
        <b>
            <%--
            <% = ctlTiff.LicenseInfo %>--%>
        </b>
    </div>
    <!-- Annotation properties window start -->
    <div id="divProperties">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                Back color:
                            </td>
                            <td>
                                <div class="controlset">
                                    <input type="text" id="annbackColor" /><input id="color1" type="text" name="color1"
                                        value="#FFFFFF" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Border color:
                            </td>
                            <td>
                                <div class="controlset">
                                    <input type="text" id="annborderColor" /><input id="color2" type="text" name="color2"
                                        value="#000000" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Border width:
                            </td>
                            <td>
                                <input type="text" id="annborderWidth" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Show Border:
                            </td>
                            <td>
                                <input type="text" id="annshowBorder" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Opacity:
                            </td>
                            <td>
                                <input type="text" id="annOpactiy" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Rotate:
                            </td>
                            <td>
                                <input type="text" id="annRotate" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Can Rotate:
                            </td>
                            <td>
                                <input type="text" id="annCanRotate" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                Title:
                            </td>
                            <td>
                                <input type="text" id="annTitle" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Show Title:
                            </td>
                            <td>
                                <input type="text" id="annshowTitle" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Title color:
                            </td>
                            <td>
                                <input type="text" id="anntitleColor" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Title Font Size:
                            </td>
                            <td>
                                <input type="text" id="anntitleFontSize" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Note/Url:
                            </td>
                            <td>
                                <input type="text" id="annNote" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Show Note:
                            </td>
                            <td>
                                <input type="text" id="annshowNote" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Text align:
                            </td>
                            <td>
                                <input type="text" id="anntextAlign" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <b>Arrow</b> Direction:
                            </td>
                            <td>
                                <input type="text" id="annarrowDirection" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Line</b> Vertical:
                            </td>
                            <td>
                                <input type="text" id="annlineVertical" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Burn:
                            </td>
                            <td>
                                <input type="text" id="annBurn" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Locked:
                            </td>
                            <td>
                                <input type="text" id="annLocked" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                z-Index:
                            </td>
                            <td>
                                <input type="text" id="annzindex" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>
                        <tr>
                            <td>
                                <b>Annotations</b><br />
                                <select id="lstAnnotations" size="2" name="annotations" style="height: 150px; width: 90px">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" value="Delete" onclick="DeleteAnnotation();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <input type="button" id="btnApply" onclick="ApplyFn();" value="Save Properties" />
    </div>
    <!-- Annotation properties window end -->
    <!-- Insert tiff starts -->
    <div id="divUpload" title="Insert Tiff">
        <asp:FileUpload ID="txtUpload" runat="server" />&nbsp;
        <asp:Button ID="btnUpload" runat="server" OnClientClick="javascript:return false;"
            Text="Upload & Insert File!" />
        Insert after page:&nbsp;<asp:TextBox ID="txtIndex" runat="server" Width="30px"></asp:TextBox>
        <span style="font-size: 10px">(Existing changes like rotation, annotation, deletion
            will be saved.)</span>
    </div>
    <!-- Insert tiff ends -->
    </form>
    <script type="text/javascript" src="js/thickbox-compressed.js"></script>
    <script type="text/javascript" language="javascript" src="js/menu.js"></script>
    <script type="text/javascript" src="js/gen_validatorv2.js"></script>
    <script type="text/javascript" src="js/jquery.colorPicker.js"></script>
    <!-- Javascript methods start -->
    <script language="javascript" type="text/javascript">

        function ShowEventLog(log) {
            document.getElementById("debug").value = log + "..." + document.getElementById("debug").value;
        }

        function objTiffViewer_PageOpening(page) {
            ShowEventLog('Page ' + page + ' opening now');
        }

        function objTiffViewer_PageOpened(page) {
            ShowEventLog('Page ' + page + ' opened');
        }

        function objTiffViewer_ThumbClicked(page) {
            ShowEventLog('Thumb ' + page + ' clicked');
        }

        function objTiffViewer_Status(active) {
            if (active) {
                ShowEventLog('Viewer ready');
                // $("#nav").show(); // you can have your own logic here

            }
            else {
                ShowEventLog('Viewer busy');
                // $("#nav").hide();
            }
        }

        function ShowUpload() {

            $('#divUpload').css('visibility', 'visible');

            $('#divUpload').dialog({ width: 450, height: 180 });

            $('#divUpload').parent().appendTo($('form:first'));

            var cp = objTiffViewer.GetCurrentPage();
            $('#txtIndex').val(cp);

            return false;
        }

        function SaveTiff() {
            objTiffViewer.SaveChanges($('#chkAnn').is(':checked'), $('#chkCompress').is(':checked'));
        }

        $(document).ready(function () {

            $('#<%= btnUpload.ClientID %>').on('click', function (e) {

                this.disabled = 'disabled';

                e.preventDefault();
                var fileInput = $('#<%= txtUpload.ClientID %>');

                var indexToInsert = parseInt($('#txtIndex').val());

                if (fileInput.val().length == 0) {
                    alert("No file to upload");
                    this.disabled = '';
                    return false;
                }

                var ext = fileInput.val().split('.').pop().toUpperCase();

                if (ext == "TIF" || ext == "TIFF") {
                }
                else {
                    alert("Only tif / tiff files allowed");
                    this.disabled = '';
                    return false;
                }

                this.value = "Inserting...";


                var fileData = fileInput.prop("files")[0];   // Getting the properties of file from file field
                var formData = new window.FormData();        // Creating object of FormData class
                formData.append("file", fileData);           // Appending parameter named file with properties of file_field to form_data

                $.ajax({
                    url: 'FileUpload.ashx',
                    data: formData,
                    processData: false,
                    contentType: false,
                    type: 'POST',
                    async: false,
                    success: function (data) {

                        var obj = $.parseJSON(data);

                        if (obj.StatusCode == "OK") {

                            SaveTiff();
                            objTiffViewer.InsertTiff(obj.Uploaded, indexToInsert);

                            $('#divUpload').dialog('close');
                        }
                        else {
                            alert("There was a problem uploading the file.");
                        }
                    },
                    error: function (errorData) {
                        alert("There was a problem uploading the file.");
                    }
                });

                this.disabled = '';
                this.value = "Upload & Insert";
                return false;
            });
        });

        function DeleteSelected() {
            var sel = objTiffViewer.GetSelected();
            if (sel.length == 0) {
                objTiffViewer.DeletePage();
            }
            else {
                for (var i = 0; i < sel.length; i++) {
                    objTiffViewer.DeletePage(sel[i]);
                }
            }
        }

        function CleanUpSelected() {

            var borderRemoval = true;
            var deskewDesp = true;
            var punchHole = true;

            var sel = objTiffViewer.GetSelected();
            if (sel.length == 0) {
                objTiffViewer.DocumentCleanUp(borderRemoval, deskewDesp, punchHole);
            }
            else {
                for (var i = 0; i < sel.length; i++) {
                    objTiffViewer.DocumentCleanUp(borderRemoval, deskewDesp, punchHole, sel[i]);
                }
            }
        }

        function CreateAnnot() {
            var annType = $('#AnnSelect').val();
            if (null != objAnnotationUI) {
                objAnnotationUI.annotationType = annType;
            }
        }

        function AddRect() {
            var objRect = new RectangleAnnotation({ left: 10, top: 10, width: 300, height: 100, backColor: 'orange', opacity: 70 });
            objAnnotationUI.AddAnnotation(null, objRect, null);
        }

        function AddEllipse() {
            var objEllipse = new EllipseAnnotation({ left: 10, top: 200, width: 200, borderWidth: 4, height: 100, borderColor: 'black', backColor: 'red' });
            objAnnotationUI.AddAnnotation(null, objEllipse, null);
        }

        function AddNote() {
            var objNote = new NoteAnnotation({ left: 350, top: 10, width: 200, borderWidth: 2, height: 100, borderColor: 'red', note: 'Here is some error!', backColor: '#ffffff' });
            objAnnotationUI.AddAnnotation(null, objNote, null);
        }

        function AddArrow() {
            var objArrow = new ArrowAnnotation({ left: 250, top: 200, width: 100, borderWidth: 4, height: 100, borderColor: 'red', arrowDirection: 'S' });
            objAnnotationUI.AddAnnotation(null, objArrow, null);
        }

        function AddStamp() {
            var objStamp = new StampAnnotation({ left: 350, top: 300, width: 300, height: 100, title: 'APPROVED', borderWidth: 4 });
            objAnnotationUI.AddAnnotation(null, objStamp, null);
        }

        function DeleteFn() {

            $("#lstAnnotations option[value='" + objAnnotationUI.GetSelectedAnnotation() + "']").remove();

            $('#annlineVertical').val("");
            $('#annarrowDirection').val("");

            $('#annId').val("");
            $('#annType').val("");
            $('#annLeft').val("");
            $('#annTop').val("");
            $('#annWidth').val("");
            $('#annHeight').val("");

            $('#annborderColor').val("");
            $('#annbackColor').val("");
            $('#annborderWidth').val("");

            $('#annOpactiy').val("");
            $('#annborderWidth').val("");
            $('#annshowBorder').val("");

            $('#annRotate').val("");
            $('#annCanRotate').val("");

            $('#annTitle').val("");
            $('#annshowTitle').val("");

            $('#anntitleColor').val("");
            $('#anntitleFontSize').val("");

            $('#annNote').val("");
            $('#annshowNote').val("");

            $('#annBurn').val("");
            $('#annLocked').val("");

            $('#annzindex').val("");
            $('#annBase64').val("");

            $('#anntextAlign').val("");
        }

        function LoadFn() {
            var ann = objAnnotationUI.GetAnnotationById(objAnnotationUI.GetSelectedAnnotation());

            if (null != ann) {
                $('#annlineVertical').val("false");
                $('#annarrowDirection').val("");

                $('#annId').val(ann.annId);
                $('#annType').val(ann.annType);

                $('#annWidth').val(ann.GetWidth());
                $('#annHeight').val(ann.GetHeight());

                $('#annborderColor').val(ann.GetBorderColor());
                $('#annbackColor').val(ann.GetBackColor());

                $('#color1_color_picker').css("background-color", ann.GetBackColor());
                $('#color2_color_picker').css("background-color", ann.GetBorderColor());

                $('#color1').val(ann.GetBackColor());
                $('#color2').val(ann.GetBorderColor());

                $('#annborderWidth').val(ann.GetBorderWidth());

                $('#annOpactiy').val(ann.GetOpacity());
                $('#annborderWidth').val(ann.GetBorderWidth());
                $('#annshowBorder').val(ann.GetShowBorder());

                $('#annRotate').val(ann.GetRotate());
                $('#annCanRotate').val(ann.GetCanRotate());

                $('#annTitle').val(ann.GetTitle());
                $('#annshowTitle').val(ann.GetShowTitle());

                $('#anntitleColor').val(ann.GetTitleColor());
                $('#anntitleFontSize').val(ann.GetTitleFontSize());

                $('#annNote').val(ann.GetNote());
                $('#annshowNote').val(ann.GetShowNote());

                $('#annBurn').val(ann.GetBurn());
                $('#annLocked').val(ann.GetLocked());

                $('#annzindex').val(ann.GetzIndex());
                $('#annBase64').val(Base64.encode(ann.toString()));

                $('#anntextAlign').val(ann.GetTextAlign());

                switch (ann.annType) {

                    case "line":
                        $('#annlineVertical').val(ann.GetLineVertical());
                        break;
                    case "arrow":
                        $('#annarrowDirection').val(ann.GetArrowDirection());
                        break;
                }
            }

            $("#lstAnnotations").val(objAnnotationUI.GetSelectedAnnotation());
        }

        function ApplyFn() {

            if (frmvalidator.ValidateAll()) {

                var ann = objAnnotationUI.GetAnnotationById(objAnnotationUI.GetSelectedAnnotation());

                if (null != ann) {
                    ann.SetBorderColor($('#annborderColor').val());
                    ann.SetBackColor($('#annbackColor').val());
                    ann.SetBorderWidth($('#annborderWidth').val());

                    ann.SetOpacity($('#annOpactiy').val());
                    ann.SetShowBorder($('#annshowBorder').val().toString().toLowerCase() == 'true');

                    ann.SetCanRotate($('#annCanRotate').val().toString().toLowerCase() == 'true');
                    ann.SetRotate($('#annRotate').val());

                    ann.SetShowTitle($('#annshowTitle').val().toString().toLowerCase() == 'true');
                    ann.SetTitle($('#annTitle').val());

                    ann.SetTitleColor($('#anntitleColor').val());
                    ann.SetTitleFontSize($('#anntitleFontSize').val());

                    ann.SetNote($('#annNote').val());
                    ann.SetShowNote($('#annshowNote').val().toString().toLowerCase() == 'true');

                    ann.SetBurn($('#annBurn').val().toString().toLowerCase() == 'true');
                    ann.SetLocked($('#annLocked').val().toString().toLowerCase() == 'true');
                    ann.SetzIndex(parseInt($('#annzindex').val()));

                    switch (ann.annType) {

                        case "line":
                            ann.SetLineVertical($('#annlineVertical').val().toString().toLowerCase() == 'true');
                            break;
                        case "arrow":
                            ann.SetArrowDirection($('#annarrowDirection').val());
                            break;
                        case "note":
                            ann.SetTextAlign($('#anntextAlign').val());
                            break;
                    }

                    ann.Paint();
                }

                $('#divProperties').css("display", "none");
                $('#divProperties').dialog('close');

                objTiffViewer.SaveAnnotations();

            }
        }

        function CreateFn() {
            var addedAnn = objAnnotationUI.GetSelectedAnnotation();
            $("#lstAnnotations").append($('<option></option>').attr('value', addedAnn).text(addedAnn));
        }

        function PropertiesFn() {
            LoadFn();

            $('#divProperties').css("display", "");


            $("#divProperties").dialog({
                height: 300, width: 570, modal: true,
                title: 'Annotation Properties'
            });

        }

        function DeleteAnnotation() {
            var selected = $('#lstAnnotations').find(':selected').text();
            objAnnotationUI.DeleteAnnotation(selected);
            $('#lstAnnotations').remove("selected");
        }

        $('#lstAnnotations').click(function () {

            var selected = $(this).find(':selected').text();
            objAnnotationUI.SelectAnnotation(selected);
            LoadFn();

        });

        $('#divProperties').css("display", "none");

        // Validations

        var frmvalidator = new Validator();

        frmvalidator.addValidation("annOpactiy", "req", "Please enter opacity");
        frmvalidator.addValidation("annOpactiy", "num", "Please enter a valid opacity 0 to 100");
        frmvalidator.addValidation("annOpactiy", "lessthan=101", "Please enter a valid opacity 0 to 100");
        frmvalidator.addValidation("annOpactiy", "greaterthan=-1", "Please enter a valid opacity 0 to 100");

        frmvalidator.addValidation("annbackColor", "req", "Please enter background color. You can use transparent i.e #000000FF");
        frmvalidator.addValidation("annborderColor", "req", "Please enter border color");

        frmvalidator.addValidation("annborderWidth", "req", "Please enter border width");
        frmvalidator.addValidation("annborderWidth", "num", "Please enter a valid border width");

        frmvalidator.addValidation("annshowBorder", "bool", "Please enter true or false for show border");

        frmvalidator.addValidation("annRotate", "req", "Please enter rotate angle");
        frmvalidator.addValidation("annRotate", "num", "Please enter a valid rotate angle");

        frmvalidator.addValidation("annCanRotate", "bool", "Please enter true or false for show border");
        frmvalidator.addValidation("annshowTitle", "bool", "Please enter true or false for show title");
        frmvalidator.addValidation("annshowNote", "bool", "Please enter true or false for show note");
        frmvalidator.addValidation("annlineVertical", "bool", "Please enter true or false for line vertical");
        frmvalidator.addValidation("annBurn", "bool", "Please enter true or false for burn annotation");
        frmvalidator.addValidation("annLocked", "bool", "Please enter true or false for lock annotation");

        frmvalidator.addValidation("anntitleFontSize", "req", "Please enter title font size");
        frmvalidator.addValidation("anntitleFontSize", "num", "Please enter a valid title font size");

        frmvalidator.addValidation("annzindex", "req", "Please enter zIndex");
        frmvalidator.addValidation("annzindex", "num", "Please enter a valid zIndex");


        $(document).ready(function () {
            $('#color1').colorPicker();
            $('#color2').colorPicker();
        });

        $('#color1').change(function () {
            $('#annbackColor').val($('#color1').val());
        });

        $('#color2').change(function () {
            $('#annborderColor').val($('#color2').val());
        });

        $(window).load(function () {

            objTiffViewer.ThumbFreeze(true);

            $("#LeftPane").width($("#LeftPane").width() - 10);

            $("#RightPane").width($("#RightPane").width() + 10);

        });

    </script>
    <!-- Javascript methods end -->
</body>
</html>
