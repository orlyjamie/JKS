<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ajax.aspx.cs" Inherits="TestTiff.Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AnnotationTiffViewer" Namespace="TiffViewer" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tiff Viewer Ajax Demo</title>
    <link rel="stylesheet" href="css/style.css" type="text/css" media="screen, projection" />
    <style type="text/css">
        .buttonColor
        {
            border: solid 1px #ccc;
            padding: 5px;
            margin-top: 5px;
            background-color: #79B933;
            color: #fff;
        }
        
        body
        {
            font-family: Verdana;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="frmAjax" runat="server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ctlScriptManager" />
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div id="divTools" style="height: 50px; width: 99%; border: solid 1px #ccc;" onmousedown="Wait(true);">
                &nbsp;<asp:Button ID="btnTiff" runat="server" Text="OPEN TIF" CssClass="buttonColor"
                    OnClick="btnTiff_Click" />
                <div id="msg">
                    Please click button to open file</div>
                &nbsp;
            </div>
            <asp:TiffViewerControl ID="ctlTiff" runat="server" ImageQuality="100" FitType="FitWidth"
                ViewerHeight="600" ViewerWidth="1024" AnnotationEnabled="false" CompressBurnedImage="true"
                DefaultZoom="50" ShowThumbNails="true" ShowCheckBox="false" FastMode="true" AnnKeyboardEnabled="false"
                CacheClear="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <script language="javascript" type="text/javascript">

        function Wait(doWait) {

            if (doWait) {
                $("#msg").text("Loading....");
                $("#divTools :input").css("background-color", "#dcdcdc");
            }
            else {
                $("#msg").text("Done!");
                $("#divTools :input").css("background-color", "#79B933");
            }

            return true;
        }

    </script>
</body>
</html>
