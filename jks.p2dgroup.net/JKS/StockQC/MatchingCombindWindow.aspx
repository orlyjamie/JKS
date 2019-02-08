<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MatchingCombindWindow.aspx.cs"
    Inherits="ETC_StockQC_MatchingCombindWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        #TiffWindow.left
        {
            width: 57% !important;
            float: left;
            height: 640px;
        }
        
        #ActionWindow.right
        {
            width: 43% !important;
            float: left;
            height: 640px;
        }
    </style>
</head>
<body onunload="javascript:CaptureClose();">
    <form id="form1" runat="server">
    <div>
        <iframe class="left" src="" style="top: 100; left: 150;" scrolling="no" frameborder="0"
            marginheight="0px" marginwidth="0px" id="TiffWindow" runat="server"></iframe>
        <iframe class="right" src="" style="top: 100; left: 805;" name="menu" scrolling="yes"
            frameborder="0" marginheight="0px" marginwidth="0px" id="ActionWindow" runat="server">
        </iframe>
        <br />
    </div>
    </form>
    <script language="javascript" type="text/javascript">

        window.onbeforeunload = WindowCloseHanlder;
        function WindowCloseHanlder() {

            // alert('My CombindWindow is Closing');
            CaptureClose();
        }

        function CaptureClose(sInvoiceID, sDocType) {

            document.body.style.cursor = 'wait';
            window.opener.__doPostBack('btnProcess', '');
            //window.opener.doRefesh();
            //opener.location.reload(true);

        }
    </script>
</body>
</html>
