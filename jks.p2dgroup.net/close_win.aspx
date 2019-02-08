<%@ Page CodeBehind="close_win.aspx.cs" Language="c#" AutoEventWireup="false" Inherits="CBSolutions.ETH.Web.close_win" %>

<html>
<head>
    <script language="javascript">
        function CloseWin() {
            alert('Sorry, your session is expired. Closing application.');
            window.close();
            //window.location.href="default.aspx";
        }
    </script>
</head>
<body onload="javascript:CloseWin();">
</body>
</html>
