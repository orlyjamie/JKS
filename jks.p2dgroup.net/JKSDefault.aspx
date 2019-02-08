<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JKSDefault.aspx.cs"
    Inherits="JKSDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>DEFAULT - LOGIN</title>
    <link rel="stylesheet" type="text/css" href="Utilities/ETCLoginStyle.css" media="all" />
    <link href='https://fonts.googleapis.com/css?family=Lato:400,300italic,300,100italic,100,400italic,700,700italic,900,900italic'
        rel='stylesheet' type='text/css' />
    <script type="text/javascript" language="javascript">
        function LoginFailureMessage() {
            alert("Your account has been locked out after 6 failed login attempts. Please contact your system administrator for a password reset.");
        }
    </script>
</head>
<body class="login_bg">
    <form id="Form1" method="post" runat="server">
    <div class="login_box">
        <img src="JKS/images/JKS_logo.png" alt="JKS" height="70px" /><br />
        <br />
        <p>
            Invoice &nbsp;&nbsp; Processing</p>
        <div class="form_sect">
            <label>
                Username</label>
            <asp:TextBox ID="txtUserName" TabIndex="1" runat="server">
            </asp:TextBox><asp:RequiredFieldValidator ID="Rfvusername" runat="server" ControlToValidate="txtUserName"
                ErrorMessage="Please enter username" CssClass="error"></asp:RequiredFieldValidator>
        </div>
        <div class="form_sect">
            <label>
                Password</label>
            <asp:TextBox ID="txtPassword" TabIndex="2" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="Rfvpassword" runat="server" ControlToValidate="txtpassword"
                ErrorMessage="Please enter password" CssClass="error"></asp:RequiredFieldValidator>
            <asp:Label ID="txtNetworkID" runat="server" Visible="False"></asp:Label>
        </div>
        <div class="fp">
            <a href="JKS/JKSForgotPasswordaspx.aspx">Forgotten Password</a>
            <asp:Button ID="btnlogin" CssClass="login_btn" TabIndex="3" runat="server" ToolTip="Login"
                Text="Login" OnClick="btnlogin_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
            <div class="spacer">
            </div>
            <asp:Label ID="lblValidateMessage" runat="server" ForeColor="Red" CssClass="error"
                Visible="False">Invalid security information. Please try again.[Username case sensitive]</asp:Label>
        </div>
    </div>
    <div class="spacer">
    </div>
    <div class="foot_logo">
        <img src="images/LogoLoginP2D.jpg" alt="Paper2Data document solutions" border="0" />
    </div>
    </form>
</body>
</html>
