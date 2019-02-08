<%@ Page Language="c#" CodeBehind="ImgPopNew.aspx.cs" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.ETC.invoice.ImgPopNew" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ImgPopNew</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="InvView.css" type="text/css" rel="stylesheet"></link>
    <script language="javascript">
        var fullWidth = 0;
        var fullHeight = 0;
        var stdWidth = 0;
        var stdHeight = 0;

        function onunload() {
            window.alert("closing 	ImgForm");

        }

        function doReset() {
            var url = Form1.pbReset.getAttribute("ImgUrl");
            if (url != null) {
                Form1.DocImg.src = url;

            }
        }
        function imgLoad() {
            fullWidth = Form1.DocImg.width;
            fullHeight = Form1.DocImg.height;
            stdWidth = Math.round((210 * window.screen.width) / 320);
            stdHeight = Math.round((fullHeight * stdWidth) / fullWidth);
            Form1.Box.style.zIndex = 100;
            setStretchMode(window.parent.imgStretchMode);
        }
        function setStretchMode(mode) {
            //				window.alert("in setStretchMode");
            window.parent.imgStretchMode = mode;
            switch (mode) {
                case 1:
                case 2:
                case 4:
                    window.onresize();
                    break;
                default:
                    window.parent.imgStretchMode = 0;
                    scaleImage();
            }
        }
        function window.onresize() {
            switch (window.parent.imgStretchMode) {
                case 1:
                    Form1.DocImg.style.width = document.body.clientWidth;
                    Form1.DocImg.style.height = Math.round(fullHeight * parseInt(Form1.DocImg.style.width) / fullWidth);
                    break;
                case 2:
                    Form1.DocImg.style.height = document.body.clientHeight;
                    Form1.DocImg.style.width = Math.round(fullWidth * parseInt(Form1.DocImg.style.height) / fullHeight);
                    break;
                case 4:
                    Form1.DocImg.style.width = document.body.clientWidth;
                    Form1.DocImg.style.height = document.body.clientHeight;
                    break
            }
            //				window.alert("calling showSelection in window.onresize");		
            showSelection();
        }

        function close_validation() {
            var j;
            for (j = 0; j < 30; j++) {
                Form1.pbClose1.click();
            }
        }
        function showSelection() {
            var obj = Form1.Box;
            if (window.parent.refHeight > 0 && window.parent.refWidth > 0 && window.parent.selectTop >= 0 && window.parent.selectLeft >= 0 && window.parent.selectHeight > 0 && window.parent.selectWidth > 0) {
                height = window.parent.selectHeight * Form1.DocImg.height * 1.0 / window.parent.refHeight;
                width = window.parent.selectWidth * Form1.DocImg.width * 1.0 / window.parent.refWidth;
                left = window.parent.selectLeft * Form1.DocImg.width * 1.0 / window.parent.refWidth;

                if (width < 22.0) {
                    right = left + width;
                    left = left - height * 2;
                    width = right + 1.5 * height - left;
                }
                jiggle_x = width * 0.15;
                jiggle_y = height * 0.2;

                obj.style.top = Math.round(window.parent.selectTop * Form1.DocImg.height / window.parent.refHeight - jiggle_y * 2); //JCB S was +7
                obj.style.left = Math.round(left - jiggle_x); // was -6
                obj.style.height = Math.round(height + jiggle_y * 5.3); //JCB 
                obj.style.width = Math.round(width + jiggle_x * 2); //was +16

                var bottom = window.parent.selectBottom; //JCB Feb 22
                obj.style.zIndex = 103;
                obj.style.display = "";
                autoScroll(obj);
            }
            else
                hideSelection();
        }

        function hideSelection() {
            window.parent.clearSelection();
            Form1.Box.style.display = "none";
        }

        function scaleImage() {
            var scale = window.parent.frames("ImgToolNew").Form1.lbScale.options[window.parent.frames("ImgToolNew").Form1.lbScale.selectedIndex].value
            Form1.DocImg.style.width = (stdWidth * (scale / 100));
            Form1.DocImg.style.height = (stdHeight * (scale / 100));
            showSelection()
        }

        function autoScroll(obj) {
            if (obj.style.display != "none") {
                var ct = document.body.scrollTop;
                var cl = document.body.scrollLeft;
                var ch = document.body.clientHeight;
                var cw = document.body.clientWidth;
                var ot = parseInt(obj.style.top);
                var ol = parseInt(obj.style.left);
                var oh = parseInt(obj.style.height);
                var ow = parseInt(obj.style.width);
                var nt = ct;
                var nl = cl;

                if (ch <= oh) {
                    if (ct < ot)
                        nt = ot;
                    else if ((ct + ch) > (ot + oh))
                        nt = ot + oh - ch;
                }
                else {
                    if (ot < ct)
                        nt = ot;
                    else if ((ot + oh) > (ct + ch))
                        nt = ot + oh - ch;
                }

                if (cw <= ow) {
                    if (cl < ol)
                        nl = ol;
                    else if ((cl + cw) > (ol + ow))
                        nl = ol + ow - cw;
                }
                else {
                    if (ol < cl)
                        nl = ol;
                    else if ((ol + ow) > (cl + cw))
                        nl = ol + ow - cw;
                }

                window.scrollTo(nl, nt);
            }
        }
        function onMouseDown() {
            var x = Math.round((window.event.clientX + document.body.scrollLeft) * window.parent.refWidth / Form1.DocImg.width);
            var y = Math.round((window.event.clientY + document.body.scrollTop) * window.parent.refHeight / Form1.DocImg.height);
            window.parent.frames("ImgToolNew").imgMouseClick(x, y);
        }

    </script>
</head>
<body bgcolor="buttonface" onload="doReset();" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <div id="mydiv" style="width: 50px; height: 50px">
        <center>
            <img onmousedown="onMouseDown();" id="DocImg" style="z-index: 101; left: 0px; top: 0px"
                alt="" onload="imgLoad();" galleryimg="false" src="<%=strImg%>">
        </center>
    </div>
    <input id="Box" style="border-right: red thin dashed; border-top: transparent; z-index: 102;
        left: 0px; border-left: transparent; width: 0px; border-bottom: red thin solid;
        position: absolute; top: 0px; height: 0px; background-color: transparent" disabled
        type="text" size="10">
    <asp:Button ID="pbReset" Style="z-index: 103; left: 0px; position: absolute; top: 0px"
        runat="server" BorderStyle="None" BorderWidth="0px" Height="0px" Width="0px"
        EnableViewState="False" Visible="True" Text="R"></asp:Button>
    <asp:Button ID="pbClose1" Style="z-index: 101; left: 0px; position: absolute; top: 0px"
        runat="server" Width="0px" Height="0px" Text="Close"></asp:Button>
    </form>
</body>
</html>
