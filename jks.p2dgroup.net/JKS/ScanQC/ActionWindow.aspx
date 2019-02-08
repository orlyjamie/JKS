<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActionWindow.aspx.cs" Inherits="NewLook.NewLook_ScanQC_ActionWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ScanQC Action Window</title>
    <link href="../css/jquery-ui.css" rel="stylesheet" />
    <link href="../custom_css/jquery.modal.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.js" type="text/javascript"></script>
    <script src="../js/jquery.modal.js" type="text/javascript"></script>
    <style type="text/css">
        #TiffWindow.left
        {
            width: 57% !important;
            float: left;
        }
        .tiffHeight
        {
            height:66% !important; /*!important;*/
        }
        .actionHeight
        {
            height:300px !important; /*!important;*/
        }
        #ActionWindow.right
        {
            width: 43% !important;
            float: left;
        }
        /*#SupplierModal { left: 38%; position: absolute; top: 25%; z-index: -1; }*/
        @media (max-width: 800px)
        {
            #TiffWindow.left
            {
                width: 100% !important;
                float: none !important;
            }
        
            #ActionWindow.right
            {
                width: 100% !important;
                float: none !important;
            }
        }
    </style>
    <script type="text/javascript">
        
        
        function refreshParentOnUnload() {
            $.ajax({
                async: false,
                type: "POST",
                url: "ActionWindow.aspx/PageUnload",
                data: "{'docID': '<%=docID%>'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //alert(response.d);
                    if (response.d == "True") {
                        localStorage.setItem("selected", "divHeader_clicker");
                        if (window.opener != null) {
                            window.opener.location = window.opener.location;
                        }
                    }
                    else {
                        alert(response.d);
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        };
        //
        function refreshParentOnLoad() {
            if (window.opener != null) {
                window.opener.location = window.opener.location;
            }
        };
        //
        function setHeight() {
            var documentHeight = 0;

            documentHeight = $(document).height() - 20;

            //if (documentHeight > (screen.height - 145)) {
            //    documentHeight = screen.height - 145;
            //}

            var documentWidth = $(document).width();

            if (documentWidth <= 800) {
                documentHeight = documentHeight / 2;
            }

            var $TiffWindow = $("#<%=TiffWindow.ClientID%>");
            var $ActionWindow = $("#<%=ActionWindow.ClientID%>");

            var h = documentHeight;
            var type = '<%=Session["type"]%>';
            //alert(type);
            if (type == "old") {
                $TiffWindow.css("height", h + "px");
            }
            else {
                var winH = getUrlParameter('winH');
                var h1 = winH - (300+100);
                //alert(winH);
                $TiffWindow.css("height", h1 + "px");
                var $TiffWindow = $("#<%=TiffWindow.ClientID%>"); //Tiff Window object of iframe

                //set MySplitter width and height
                var $MySplitter = $TiffWindow.contents().find("#MySplitter"); //container of the tiff image
                $MySplitter.css("height", (h1-100));
            }
            $ActionWindow.css("height", h + "px");
            
        };
        function setWidth() {
            var docWidth = $(document).width;
            var $TiffWindow = $("#<%=TiffWindow.ClientID%>");
            $TiffWindow.css("width", "100% !important");
        }
        function getUrlParameter(name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        };
        //
        function TiffResize() {
            var $TiffWindow = $("#<%=TiffWindow.ClientID%>"); //Tiff Window object of iframe

            //set MySplitter width and height
            var $MySplitter = $TiffWindow.contents().find("#MySplitter"); //container of the tiff image
            var $nav = $TiffWindow.contents().find("#nav"); //nav bar that contain tools

            var th = $TiffWindow.height(); //height of tiff window iframe
            var nh = $nav.height();
            var sh = th - (nh + 35); //extra buffer height deducted from tiff height
            //alert(sh);

            $MySplitter.css("width", "100% !important");
            $MySplitter.height(sh);
            //set MySplitter width and height

            //set RightPane width
            var $LeftPane = $MySplitter.find("#LeftPane");
            var $splitter = $MySplitter.find(".splitbarV");
            var $RightPane = $MySplitter.find("#RightPane");
            var tw = $MySplitter.width();
            var w1 = $LeftPane.width();
            var w2 = $splitter.width();
            var w3 = tw - (w1 + w2) - 2; //deducted 2 for (1600x900, 1440x900, 1400x1050, 1360x768) in IE
            $RightPane.css("width", w3 + "px");
            //set RightPane width

            //set tiffviewer property to fit width, incase it is fit height
            var $1stli = $nav.children().eq(0);
            var $FitUl = $1stli.children().eq(1);
            var $FitWidthLi = $FitUl.children().eq(1);
            var $FitWidtha = $FitWidthLi.children().eq(0);
            $FitWidtha.click();
            //set tiffviewer property to fit width, incase it is fit height

            //resize the selected tiff
            var $SelectedThumb = $LeftPane.find(".SelectedThumb");
            $SelectedThumb.click();
            //resize the selected tiff
        };
        //
        function DelayedSizeChange() {
            var $TiffWindow = $("#<%=TiffWindow.ClientID%>"); //Tiff Window object of iframe
            var $MySplitter = $TiffWindow.contents().find("#MySplitter"); //container of the tiff image

            var th = $TiffWindow.height(); //height of tiff window iframe.
            var sh = $MySplitter.height(); //height of tiff holder in TiffWindow iframe.

            th = th - 35;

            var tw = $TiffWindow.width(); //width of tiff window iframe.
            var sw = $MySplitter.width(); //width of tiff holder in TiffWindow iframe.

            var $LeftPane = $MySplitter.find("#LeftPane");
            var $splitter = $MySplitter.find(".splitbarV");
            var $RightPane = $MySplitter.find("#RightPane");

            var w1 = $LeftPane.width();
            var w2 = $splitter.width();
            var rw = $RightPane.width();

            sw = sw - (w1 + w2) - 2;

            if ((sh > th) || (sw > rw)) {
                //alert(th);
                //alert(sh);
                //alert(sw);
                //alert(rw);

                setHeight();
                
                var type = '<%=Session["type"]%>';
                if (type == "old") {

                    //TiffResize();
                }
                //else {
                //    setWidth();
                //}
            }
        };
        $(window).ready(function () {
            //hide the modal window.
            $('#SupplierModal').modal('show');
            $('#SupplierModal').find('.close-modal').click();

            $('#DefaultsModal').modal('show');
            $('#DefaultsModal').find('.close-modal').click();
            //hide the modal window.

            var type = '<%=Session["type"]%>';

            setHeight();
            
            setTimeout(function () {
                if (type == "old") {
                    TiffResize();
                }

                setTimeout(function () {
                    DelayedSizeChange();
                }, 4000);
            }, 5000);
            
            //else {
                
            //    setTimeout(function () {

            //        setWidth();

            //        setTimeout(function () {
            //            DelayedSizeChange();
            //        }, 4000);
            //    }, 5000);
            //}

        });
        $(window).resize(function () {

            setHeight();
            var type = '<%=Session["type"]%>';
            if (type == "old") {

                setTimeout(function () {
                    TiffResize();
                }, 1000);
            }
            //else {
                
            //    setTimeout(function () {
            //        setWidth();
            //    }, 1000);
            //}
            
        });
    </script>
</head>
<body onunload="refreshParentOnUnload()" onload="refreshParentOnLoad()">
    <form id="form1" runat="server">
    <div style="height: auto; width: auto;">
        <div>
        <iframe src="" style="top: 100; left: 150;" scrolling="no" frameborder="0"
            marginheight="0px" marginwidth="0px" id="TiffWindow" runat="server">
		</iframe>
        </div>
        <div>
        <iframe src="" style="top: 100; left: 805;" name="menu" scrolling="yes"
            frameborder="0" marginheight="0px" marginwidth="0px" id="ActionWindow" runat="server">
        </iframe>
        </div>
        <input type="button" id="btnHidden" style="display: none" value="click" />
        <br />
    </div>
    <asp:HiddenField ID="hdnWindowHeight" runat="server" />
    <div id="SupplierModal" class="modal fase" role="dialog">
        <div class="modal-dialog ">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-body">
                </div>
            </div>
        </div>
    </div>
    <div id="DefaultsModal" class="modal fase" role="dialog">
        <div class="modal-dialog ">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-body">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>