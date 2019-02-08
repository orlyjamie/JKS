<%@ Page Language="c#" AutoEventWireup="false" CodeFile="ImageBrowser.aspx.cs" Inherits="tiny_mce_plugins_advimage_ImageBrowser"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="../../tiny_mce_popup.js">
    </script>

    <script language="javascript" type="text/javascript">

        var FileBrowserDialogue = {
            init: function() {
                // Here goes your code for setting your custom things onLoad.
                var URL = '<%=strURL %>';//document.my_form.my_field.value;
                var win = tinyMCEPopup.getWindowArg("window");

                // insert information now
                win.document.getElementById(tinyMCEPopup.getWindowArg("input")).value = URL;

                // are we an image browser
                if (typeof (win.ImageDialog) != "undefined") {
                    // we are, so update image dimensions and preview if necessary
                    if (win.ImageDialog.getImageData) win.ImageDialog.getImageData();
                    if (win.ImageDialog.showPreviewImage) win.ImageDialog.showPreviewImage(URL);
                }

                // close popup window
                tinyMCEPopup.close();
            },
            mySubmit: function() {

            }
        }

        //tinyMCEPopup.onInit.add(FileBrowserDialogue.init, FileBrowserDialogue);
        <%=strDone %>

    </script>

</head>
<body style="padding-top: 30px;">
    <%-- <form name="my_form">
    <input type="file" name="my_field" />
    <input type="button" value="Submit" onclick="FileBrowserDialogue.mySubmit();" />
    </form>--%>
    <center>
        <form name="my_form" method="post" enctype="multipart/form-data">
        <input type="file" name="my_field" size="50" />
        <input type="submit" value="Upload" />
        </form>
    </center>
</body>
</html>
