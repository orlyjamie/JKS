<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoCompletePopup.aspx.cs"
    Inherits="ETC_AutoCompletePopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>P2D Network - AutoComplete</title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtAutoComplete.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "AutoCompletePopup.aspx/GetCombinedDescription",
                        data: "{ 'Filter': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('#')[0],
                                    val: item.split('#')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    alert("Selection:" + i.item.val);
                    $("#<%=hdnCodingDescriptionID.ClientID %>").val(i.item.val);
                },
                minLength: 2
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtAutoComplete" runat="server" Width="300px"> </asp:TextBox>
        <asp:HiddenField ID="hdnCodingDescriptionID" runat="server" />
    </div>
    </form>
</body>
</html>
