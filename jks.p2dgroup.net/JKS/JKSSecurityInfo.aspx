<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JKSSecurityInfo.aspx.cs"
    Inherits="JKS.SecurityInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>JKS Forgot Password</title>
    <link href="../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript" type="text/javascript">
        function PopulateMessage(OptionID) {
            var vCode = OptionID;
            if (vCode == "-500") {

                alert('Your account has been locked out following 3 successive failed logins. Please request a password reset from your system administrator.');
                setTimeout('RedirectHome()', 2000);
                return;
            }
            else if (vCode == "-501") {
                alert('The answer provided is incorrect. The password reset process will lockout after 3 unsuccessful attempts.');
                setTimeout('RedirectPage()', 2000);
                return;
            }
        }

        function RedirectHome() {

            window.location = "http://JKS.p2dgroup.net";
        }
        function RedirectPage() {

            window.location = "JKSForgotPasswordaspx.aspx";
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td class="NormalBody" style="width: 200px; height: 15px" colspan="2">
                    Question:&nbsp;
                </td>
                <td class="NormalBody" style="width: 300px; height: 15px">
                    <asp:Label runat="server" ID="lblResetQuestion"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="NormalBody" style="width: 200px; height: 16px" colspan="2">
                    Answer:&nbsp;
                </td>
                <td class="NormalBody" style="height: 20px">
                    <asp:TextBox ID="txtResetQuestionAnswer" runat="server" autocomplete="off" AutoCompleteType="None"
                        Width="200" Style="width: 200px; border: 1px solid #ccc;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="NormalBody" colspan="5">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="allbtn" BorderStyle="None" Text="Submit"
                        OnClick="btnSubmit_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
