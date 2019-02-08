<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CombindWindow.aspx.cs" Inherits="CombindWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <script src="../js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var h = $(this).height();
            h = h - 10;
            $(".menu").height(h);
        });
    </script>
</head>
<body onunload="javascript:CaptureClose();">
    <form id="form1" runat="server">
    <div>
        <%-- <iframe class="left" src=""
            style="top: 100; left: 150;" scrolling="no" frameborder="0"
            marginheight="0px" marginwidth="0px" id="TiffWindow" runat="server"></iframe>--%>
        <iframe src="" name="menu" scrolling="yes" frameborder="0" marginheight="0px" marginwidth="0px"
            id="ActionWindow" runat="server" width="100%" class="menu"></iframe>
        <asp:Button ID="btnNextAction" runat="server" Visible="False"></asp:Button>
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
            // var name = "Sample";
            // alert("Test window.opener:" + name);
            // name = window.opener.name;
            // alert("CombindWindow Opener:" + name);

            window.opener.__doPostBack('btnProcess', '');

        }
    </script>
</body>
</html>
