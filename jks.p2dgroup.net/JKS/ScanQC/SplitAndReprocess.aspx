<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SplitAndReprocess.aspx.cs" Inherits="NewLook.NewLook_ScanQC_SplitAndReprocess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Split and Reprocess</title>
    <style type="text/css">
        body{ color: #848484; font-family: verdana,Tahoma,Arial; font-size: 11px; background-color: #3399cc; }
        .top-sep{ width: 50%; height: 50px; margin: 0 auto; }
        .container{ width: 50%; height: auto; margin: 0 auto; border: 1px solid black; padding: 25px; background-color: #fff; }
        .container .content1{ width: 100%; height: auto; display: inline-block; padding-bottom: 10px; }
        .container .content2{ display: inline-block; height: auto; text-align: center; width: 100%; padding-bottom: 10px; }
        .container .content1 .left{ float: left; height: auto; width: 50%; }
        .container .content1 .right{ float: right; height: auto; width: 49%; }
        .container .content1 .header{ font-size: 14px; font-weight: bold; padding-bottom: 8px; padding-top: 2px; }
        .container .content1 .left .controls{}
        .container .content1 .left .controls div{ padding-top: 2px; padding-bottom: 2px; }
        .container .content1 .left .controls div span{ display: inline-block; width: 40%; }
        .container .content1 .left .controls div input[type="checkbox"]{}
        .container .content1 .right .controls{}
        .container .content1 .right .controls div{ padding-top: 5px; padding-bottom: 5px; }
        .container .content1 .right .controls label{ display: inline-block; width: 30%; }
        .container .content1 .right .controls select{ display: inline-block; width: 68%; }
        .container .content1 .right .controls .ddlBatchType { border: 1px solid #ccc !important;  color: Red !important; font-size: 15px; height: 22px; font-weight: bold; }
        .container .content1 .right .controls .ddlCompany { border: 1px solid #ccc !important;  color: #666666 !important; font-size: 11px; height: 22px; }
        .container .content2 .buttons{ padding-bottom: 2px; padding-top: 2px; }
        .container .content2 .buttons span{ display: inline-block; height: auto; width: 60px; }
        .container .content2 .buttons .button1{ background-color: #3cbc3c; border: medium none; color: #fff; cursor: pointer; font-weight: bold; height: 35px; width: 130px; }
        .container .content2 .buttons .button2{ background-color: #ff0000; border: medium none; color: #fff; cursor: pointer; font-weight: bold; height: 35px; width: 130px; }
        .container .content2 p{ margin: auto; padding-top: 20px; text-align: justify; width: 70%; }
    </style>
    <script src="../js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //
        $(document).ready(function () {

        });
        //
        function ReProcessValidation() {
            var tf = false;
            $("#<%=pnlSelectPages.ClientID%> div").each(function (e) {
                //alert($(this).html())
                var $cb = $(this).find("input[type='checkbox']");
                //alert($cb.prop("checked"));
                if ($cb.prop("checked")) {
                    tf = true;
                }
            });
            if (!tf)
                alert('Please select at least one page.');
            return tf;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top-sep"></div>
        <div class="container">
            <div class="content1">
                <div class="left">
                    <div class="header">
                        Select Pages:
                    </div>
                    <asp:Panel ID="pnlSelectPages" runat="server" CssClass="controls"></asp:Panel>
                </div>
                <div class="right">
                    <div class="header">
                        Select Batch:
                    </div>
                    <div class="controls">
                        <div>
                            <label>Batch Type</label>
                            <asp:DropDownList ID="ddlBatchType" runat="server" CssClass="ddlBatchType"></asp:DropDownList>
                        </div>
                        <div>
                            <label>Company</label>
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="ddlCompany"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="content2">
                <div class="buttons">
                    <asp:Button ID="btnCancel" runat="server" Text="CANCEL" CssClass="button2" />
                    <span></span>
                    <asp:Button ID="btnReProcess" runat="server" Text="RE-PROCESS" CssClass="button1" />
                </div>
                <p>
                    For example, if you want to re-process each page individually as separate Invoices, tick Page 1 and press RE-PROCESS, and repeat this for all other pages. If you tick multiple pages then press RE-PROCESS those pages will be combined into a new document. After you have finished re-processing you should DELETE the original document in the previous window.
                </p>
            </div>
        </div>
    </form>
</body>
</html>
