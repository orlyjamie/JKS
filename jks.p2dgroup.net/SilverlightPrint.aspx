<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SilverlightPrint.aspx.cs"
    Inherits="TestTiff.SilverlightPrint" %>

<%@ Register Assembly="PrintServerControl" Namespace="PrintServerControl" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Print</title>
    <style type="text/css">
        #slHost
        {
            border: 1px solid #DCDCDC;
        }
        
        #form1
        {
            width: 100%;
            height: 100%;
        }
        
        body
        {
            height: 100%;
            width: 100%;
            margin: 0px;
        }
    </style>
</head>
<body onload="document.getElementById('slHost').style.height = (window.screen.height) + 'px';">
    <form id="form1" runat="server">
    <cc1:PrintControl ID="ctlPrint" runat="server" />
    </form>
</body>
</html>
