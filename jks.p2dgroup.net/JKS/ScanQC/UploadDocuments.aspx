<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadDocuments.aspx.cs" Inherits="NewLook.NewLook_ScanQC_UploadDocuments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Upload Files</title>
        <script src="../js/jquery-1.11.0.min.js" type="text/javascript"></script>
        <style type="text/css">
            @font-face {
            font-family: 'franklin_gothic_bookregular';
            src: url('../dist/fonts/frabk-webfont.eot');
            src: url('../dist/fonts/frabk-webfont.eot?#iefix') format('embedded-opentype'),
            url('../dist/fonts/frabk-webfont.woff') format('woff'),
            url('../dist/fonts/frabk-webfont.ttf') format('truetype'),
            url('../dist/fonts/frabk-webfont.svg#franklin_gothic_bookregular') format('svg');
            font-weight: normal;
            font-style: normal;	
            }
            @font-face {
            font-family: 'franklin_gothic_medium_condRg';
            src: url('../dist/fonts/framdcn-webfont.eot');
            src: url('../dist/fonts/framdcn-webfont.eot?#iefix') format('embedded-opentype'),
            url('../dist/fonts/framdcn-webfont.woff') format('woff'),
            url('../dist/fonts/framdcn-webfont.ttf') format('truetype'),
            url('../dist/fonts/framdcn-webfont.svg#franklin_gothic_medium_condRg') format('svg');
            font-weight: normal;
            font-style: normal;	
            }	
            @font-face {
            font-family: 'franklin_gothic_bookregular';
            src: url('../dist/fonts/frabk-webfont.eot');
            src: url('../dist/fonts/frabk-webfont.eot?#iefix') format('embedded-opentype'),
            url('../dist/fonts/frabk-webfont.woff') format('woff'),
            url('../dist/fonts/frabk-webfont.ttf') format('truetype'),
            url('../dist/fonts/frabk-webfont.svg#franklin_gothic_bookregular') format('svg');
            font-weight: normal;
            font-style: normal;
            }
            .hidden{ display: none; visibility: hidden; }
            .shown{ display: block; visibility: visible; }
            a{ color: #0081c5; }
            a:hover{ color: Blue; }
            body { color: #848484; font-family: verdana,Tahoma,Arial; font-size: 11px; background-color: #3399cc; }
            .upload_files { width: 100%; height:95%; margin-top:5%; }
            .upload_files .container { border: 1px solid black; margin: 0 auto; padding: 24px; width: 40%; background-color: #fff; }
            .upload_files .container .select_batch {}
            .upload_files .container .select_batch .header { font-size: 14px; font-weight: bold; padding-bottom: 8px; padding-top: 2px; }
            .upload_files .container .select_batch .fields { margin-bottom: 5px; margin-top: 5px; }
            .upload_files .container .select_batch .fields span { display: inline-block; width: 30%; text-align: left; }
            .upload_files .container .select_batch .fields div { display: inline-block; width: 69%; text-align: right; } 
            .upload_files .container .select_batch .fields div select { border: 1px solid #ccc !important; border-radius: 3px; color: #666666 !important; font-size: 11px; height: 28px; width: 75%; }
            .upload_files .container .select_batch .fields div .ddlBatchType { color: Red !important; font-size: 15px; font-weight: bold; }
            .upload_files .container .uploads { margin-bottom: 10px; margin-top: 10px; }
            .upload_files .container .uploads .fields { margin-bottom: 5px; margin-top: 5px; }
            .upload_files .container .uploads .fields span { display: inline-block; width: 30%; text-align: left; }
            .upload_files .container .uploads .fields div { display: inline-block; width: 69%; text-align: right; } 
            .upload_files .container .uploads .fields div input[type='file'] { border: 1.5px solid #cccccc; border-radius: 3px; color: #666666 !important; font-size: 11px; width: 75%; }
            .upload_files .container .buttons { text-align: center; margin-bottom: 5px; margin-top: 5px; }
            .upload_files .container .buttons .RedButton { background-color: #3cbc3c; border: 0 none; color: #ffffff; cursor: pointer; font-family: "franklin_gothic_medium_condRg"; font-size: 16px; font-weight: 400; height: 35px; text-transform: uppercase; width: 30%; }
            .upload_files .container .buttons .GreenButton { background-color: #ff0000; border: 0 none; color: #ffffff; cursor: pointer; font-family: "franklin_gothic_medium_condRg"; font-size: 16px; font-weight: 400; height: 35px; text-transform: uppercase; width: 30%; }
            .upload_files .container .buttons .GrayButton { background-color: #acacac; border: 0 none; color: #ffffff; cursor: pointer; font-family: "franklin_gothic_medium_condRg"; font-size: 16px; font-weight: 400; height: 35px; text-transform: uppercase; width: 30%; margin: 0 auto; }
            .upload_files .container .buttons .space { width: 10%; display: inline-block; }
            /*.upload_files .container .buttons input[type='submit']:hover, input[type='button']:hover { background-color: #3276b1; }*/
            .upload_files .container .info { margin: 0px auto; padding-top: 20px; text-align: justify; width: 70%; }
            .uploading { background-color: gray; display: inline-block; height: 100%; left: 0; position: absolute; text-align: center; top: 0; width: 100%; opacity: .75;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=50)"; /* IE 8 */ filter: alpha(opacity=50); /* IE 5-7 */ -moz-opacity: 0.5; /* Netscape */ -khtml-opacity: 0.5; /* Safari 1.x */ }
            .uploading .container { position: relative; top: 35%; }
            .uploading .container .head { color: #0000ff; font-size: 18px; font-weight: bold; padding-bottom: 2px; text-shadow: 2px 2px #acacac; }
            .uploading .container .body {}
            .uploading .container .body img { width: 95px; }
        </style>
        <script type="text/javascript">
            //Show confirm dialog and take result to perform upload
            function ConfirmZip() {
                var result = confirm('Warning: you are uploading a zip file. \nThis must only contain pdf, tif, jpeg or png files. \nAny other file types will not be processed. Do you wish to continue?');
                //alert(result);
                setTimeout(function () {
                    var tf = '';
                    if (result)
                        $("#<%=btnUploader.ClientID%>").click();
                }, 100);
            };
            //Show the spinning gif image
            function ShowUploading() {
                setTimeout(function () {
                    $(".uploading").addClass("shown");
                    $(".uploading").removeClass("hidden");
                }, 0);
            };
        </script>
    </head>
    <body>
        <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div class="upload_files">
                <div class="container">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="select_batch">
                                <div class="header">
                                    Select Batch:
                                </div>
                                <div class="fields">
                                    <span>Company</span>
                                    <div>
                                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="ddlCompany"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="fields">
                                    <span>Batch Type</span>
                                    <div>
                                        <asp:DropDownList ID="ddlBatchType" runat="server" CssClass="ddlBatchType"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="uploads">
                        <div class="fields">
	                        <span>File 1: </span>
	                        <div><asp:FileUpload ID="FileUpload1" runat="server" /></div>
                        </div>
                        <div class="fields">
	                        <span>File 2: </span>
	                        <div><asp:FileUpload ID="FileUpload2" runat="server" /></div>
                        </div>
                        <div class="fields">
	                        <span>File 3: </span>
	                        <div><asp:FileUpload ID="FileUpload3" runat="server" /></div>
                        </div>
                        <div class="fields">
	                        <span>File 4: </span>
	                        <div><asp:FileUpload ID="FileUpload4" runat="server" /></div>
                        </div>
                        <div class="fields">
	                        <span>File 5: </span>
	                        <div><asp:FileUpload ID="FileUpload5" runat="server" /></div>
                        </div>
                    </div>
                    <div class="buttons">
                        <asp:Button ID="btnBack" runat="server" Text="Back" UseSubmitBehavior="false" CssClass="GreenButton"></asp:Button>
                        <span class="space"></span>
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" UseSubmitBehavior="true" CssClass="RedButton"></asp:Button>
                        <asp:Button ID="btnUploader" runat="server" Text="Uploader" UseSubmitBehavior="false" CssClass="GrayButton"></asp:Button>
                    </div>
                    <div class="info">
                        This section allows you to upload the following file types for processing in P2D: pdf, tif, jpeg and png. You can use this function instead of emailing.
                    </div>
                </div>
            </div>
            <div class="uploading hidden">
                <div class="container">
                    <div class="head">
                        Uploading...
                    </div>
                    <div class="body">
                        <img src="../images/TLoading1.gif" alt="Loading..." />
                    </div>
                </div>                
            </div>
        </form>
    </body>
</html>
