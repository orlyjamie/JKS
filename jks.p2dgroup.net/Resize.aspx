<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Resize.aspx.cs" Inherits="TestTiff.Resize" %>

<%@ Register Assembly="AnnotationTiffViewer" Namespace="TiffViewer" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tiff Viewer Resize Demo</title>
    <link rel="stylesheet" href="css/style.css" type="text/css" media="screen, projection" />
</head>
<body>
    <form id="frmResize" runat="server">
    <asp:TiffViewerControl ID="ctlTiff" runat="server" ImageQuality="100" FitType="FitWidth"
        AnnotationEnabled="false" CompressBurnedImage="true" DefaultZoom="50" ShowThumbNails="true"
        ShowCheckBox="false" FastMode="true" AnnKeyboardEnabled="false" CacheClear="true" />
    </form>
    <script language="javascript" type="text/javascript">

        if (typeof jQuery != 'undefined') {
            $(window).load(function () {

                var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
                var w = "innerWidth" in window ? window.innerWidth : document.documentElement.offsetWidth;


                objTiffViewer.ViewerResize(w - 20, h - 20);

            });
        }
    </script>
</body>
</html>
