<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JKSForgotPasswordaspx.aspx.cs"
    Inherits="JKSForgotPasswordaspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>JKS Forgot Password</title>
    <link href="../Utilities/ETH.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
        function PopulateMessage(OptionID) {
            var vCode = OptionID;
            if (vCode == "1") {

                alert('The Username / Email combination is not recognised. Please request a password reset from your system administrator.');
                return;
            }
            else if (vCode == "2") {

                alert('You have not saved a password reset question and answer in the system. Please request a password reset from your system administrator. Then, after logging in, create a security question and answer in the Change Password section of the system.');
                return;
            }
            else if (vCode == "3") {
                alert('Your account has been locked out following 3 successive failed logins. Please request a password reset from your system administrator.');
                return;
            }

        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div>
        <table>
            <tr>
                <td class="NormalBody" style="width: 200px; height: 15px" colspan="2">
                    User Name&nbsp;
                </td>
                <td class="NormalBody" style="width: 300px; height: 15px">
                    <asp:TextBox ID="txtUserName" runat="server" Width="200px" Style="width: 200px; border: 1px solid #ccc;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="NormalBody" style="width: 200px; height: 16px" colspan="2">
                    Email&nbsp;
                </td>
                <td class="NormalBody" style="height: 20px">
                    <asp:TextBox ID="txtEmail" runat="server" Width="200px" Style="width: 200px; border: 1px solid #ccc;"></asp:TextBox>
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
