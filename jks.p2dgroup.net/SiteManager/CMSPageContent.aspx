<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CMSPageContent.aspx.cs" Inherits="SiteManager_CMSPageContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>CMSPageContent</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <script type="text/javascript" src="tiny_mce/tiny_mce.js"></script>
    <script type="text/javascript">
        tinyMCE.init({
            forced_root_block: false,
            force_br_newlines: true,
            force_p_newlines: false,
            // General options
            mode: "textareas",
            theme: "advanced",
            plugins: "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",

            // Theme options
            file_browser_callback: "myFileBrowser",
            theme_advanced_buttons1: "save,|,bold,italic,underline,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,code,|,forecolor,backcolor",
            theme_advanced_buttons3: "tablecontrols,|,hr,|,sub,sup,|,image,media",
            theme_advanced_buttons4: "",
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: true,

            // Example content CSS (should be your site CSS)
            content_css: "css/content.css",

            // Drop lists for link/image/media/template dialogs
            template_external_list_url: "lists/template_list.js",
            external_link_list_url: "lists/link_list.js",
            external_image_list_url: "lists/image_list.js",
            media_external_list_url: "lists/media_list.js",

            // Replace values for the template plugin
            template_replace_values: {
                username: "Some User",
                staffid: "991234"
            }
        });
    </script>
    <script language="javascript" type="text/javascript">
        function myFileBrowser(field_name, url, type, win) {

            // alert("Field_Name: " + field_name + "nURL: " + url + "nType: " + type + "nWin: " + win); // debug/testing

            /* If you work with sessions in PHP and your client doesn't accept cookies you might need to carry
            the session name and session ID in the request string (can look like this: "?PHPSESSID=88p0n70s9dsknra96qhuk6etm5").
            These lines of code extract the necessary parameters and add them back to the filebrowser URL again. */

            //var cmsURL = window.location.toString();    // script URL - use an absolute path!
            var cmsURL = "tiny_mce/plugins/advimage/ImageBrowser.aspx?type=image";    // script URL - use an absolute path!
            if (cmsURL.indexOf("?") < 0) {
                //add the type as the only query parameter
                cmsURL = cmsURL + "?type=" + type;
            }
            else {
                //add the type as an additional query parameter
                // (PHP session ID is now included if there is one at all)
                cmsURL = cmsURL + "&type=" + type;
            }

            tinyMCE.activeEditor.windowManager.open({
                file: cmsURL,
                title: 'My File Browser',
                width: 400,  // Your dimensions may differ - toy around with them!
                height: 100,
                resizable: "yes",
                inline: "yes",  // This parameter only has an effect if you use the inlinepopups plugin!
                close_previous: "no"
            }, {
                window: win,
                input: field_name
            });
            return false;
        }
    </script>
    <style type="text/css">
        #main-content TABLE TD
        {
            padding-bottom: 4px !important;
            line-height: 1.3em;
            padding-left: 4px !important;
            padding-right: 4px !important;
            padding-top: 4px !important;
        }
        #main-content TABLE TH
        {
            padding-bottom: 4px !important;
            line-height: 1.3em;
            padding-left: 4px !important;
            padding-right: 4px !important;
            padding-top: 4px !important;
        }
    </style>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%">
        <tr>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="tblCMSPageContent" border="0" cellspacing="1" cellpadding="1" width="100%">
                    <tr>
                        <td style="height: 16px" class="PageHeader" colspan="4">
                            <asp:Label ID="lblConfirmation" runat="server" CssClass="PageHeader">
								Add Page Content
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16%; height: 14px" class="GridCaption">
                            Page Name:
                        </td>
                        <td style="height: 14px">
                            <asp:DropDownList ID="ddlPageName" runat="server" CssClass="NormalBody" Width="200"
                                AutoPostBack="True" DataTextField="PageTitle" DataValueField="PageID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16%; height: 14px" class="GridCaption">
                            Page Order:
                        </td>
                        <td style="height: 14px">
                            <asp:TextBox ID="txtPageOrder" runat="server" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 16%; height: 14px" class="GridCaption">
                            Contents:
                        </td>
                        <td style="height: 14px">
                            <asp:TextBox ID="txtContents" runat="server" CssClass="mceEditorDec large-input"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" class="GridCaption">
                            IsActive :
                        </td>
                        <td style="height: 14px">
                            <asp:CheckBox ID="chkIsActive" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 14px" class="GridCaption">
                        </td>
                        <td style="height: 14px">
                            <asp:Button ID="btnSubmit" runat="server" Style="borderstyle: None" CssClass="ButtonCss"
                                Text="Submit" Width="100" Height="24"></asp:Button>
                            <asp:Button ID="btnReset" runat="server" Style="borderstyle: None" CssClass="ButtonCss"
                                Text="Reset" Width="100" Height="24"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Label runat="server" ID="lblMessage"></asp:Label>
    <div>
        <table>
            <tr>
                <td>
                    <asp:DataGrid ID="dgvCMSPageContent" runat="server" Width="896px" BorderStyle="None"
                        Height="88px" BorderColor="#999999" BackColor="White" BorderWidth="1px" CellPadding="3"
                        GridLines="Vertical" AutoGenerateColumns="False" Font-Names="verdana" Font-Size="10pt">
                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                        <ItemStyle Font-Size="Smaller" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                        </ItemStyle>
                        <HeaderStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                            BackColor="#3399CC"></HeaderStyle>
                        <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="PageContentID" HeaderText="PageContentID">
                                <HeaderStyle Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Contents" HeaderText="Contents"></asp:BoundColumn>
                            <asp:BoundColumn DataField="PageOrder" HeaderText="Page Order">
                                <HeaderStyle Width="300px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PageTitle" HeaderText="Page Name">
                                <HeaderStyle Width="300px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnEdit" runat="server" CommandName="ED" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PageContentID")%>'>Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnDelete" runat="server" CommandName="DEL" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PageContentID")%>'>Delete</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
                        </PagerStyle>
                    </asp:DataGrid>
                </td>
                </TD>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
