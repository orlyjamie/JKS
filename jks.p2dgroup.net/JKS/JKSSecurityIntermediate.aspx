<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JKSSecurityIntermediate.aspx.cs"
    Inherits="JKS.JKSSecurityIntermediate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        function AlertMessage() {
            alert('A new temporary password has been emailed to you. You will be required to change it when you login.');
            setTimeout('Redirect()', 2000);
        }
        function Redirect() {

            // window.location="https://www.p2dgroup.net/JKSdefault.aspx";
            window.location = "http://JKS.p2dgroup.net";
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
