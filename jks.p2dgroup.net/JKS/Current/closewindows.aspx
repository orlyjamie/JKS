<%@ Page Language="C#" AutoEventWireup="true" CodeFile="closewindows.aspx.cs" Inherits="JKS_Current_closewindows" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%-- <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".white_content").fadeIn("slow");
            $("#closing").click(function () {
                $(".white_content").fadeOut("slow");
                window.parent.close();
            });
        });
    </script>
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet" />
    <style>
        b
        {
            padding-right: 1px;
            padding-bottom: 25px;
            padding-left: 1px;
        }
        
        
        .fallbtn_ActionWindow
        {
            color: #fff !important;
            font-weight: bold !important;
            text-decoration: none !important;
            background-color: #2491cf !important;
            border-color: #357ebd !important;
            height: 24px;
            line-height: 20px;
            margin: 5px;
            width: 110px;
            font-size: 11px !important;
        }
        
        
        .boxclose
        {
            color: #fff;
            border: 1px solid #AEAEAE;
            border-radius: 30px;
            background: #605F61;
            font-size: 31px;
            font-weight: bold;
            display: inline-block;
            line-height: 0px;
            padding: 11px 3px;
        }
        .white_content
        {
            display: block;
            position: absolute;
            top: 25%;
            left: 25%;
            width: 50%;
            height: 50%;
            padding: 16px;
            border: 16px solid blue;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Panel ID="Panel1" BorderStyle="Solid" runat="server" Width="400px" Height="200px">
            <table width="100%">
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td cellspacing="__designer:mapid=&quot;4&quot;">
                        <br />
                        <br />
                        <p style="text-align: left;">
                            <b>You have already actioned this document. Please press ok to close the window to allow
                                you to resume.</b></p>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <p id="closing" class="fallbtn_ActionWindow" style="text-align: center;">
                            OK</p>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
