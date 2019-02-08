<%@ Page Language="c#" CodeBehind="ReconciliationRpt.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.Reconciliation.ReconciliationRpt" %>

<%@ Register TagPrefix="uc1" TagName="menuUserNL" Src="../Utilities/menuUserNL.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../../Utilities/bannerETC.ascx" %>
<%@ Import Namespace="System.IO" %>
<html>
<head>
    <title>P2D Network - ReconciliationReport</title>
    <script language="javascript" src="../../WinOpener.js"></script>
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
        function PopupPic(DocumentID, w, h) {
            var winl = (screen.width - w) / 2;
            var wint = (screen.height - h) / 2;
            winprops = 'height=' + h + ',width=' + w + ',top=' + wint + ',left=' + winl + 'resizable=0'
            window.open("ImgPopup_NL_INV.aspx?DocumentID=" + DocumentID, "Image", winprops)
        } 
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <form id="Form2" method="post" runat="server">
    <asp:Panel ID="Panel3" Style="z-index: 102; left: 0px" runat="server" Width="100%"
        CssClass="Banner">
        <uc1:banner ID="Banner2" runat="server"></uc1:banner>
    </asp:Panel>
    <table width="100%">
        <tr>
            <td valign="top" width="150" bgcolor="gainsboro">
                <table>
                    <tr>
                        <td>
                            <uc1:menuUserNL ID="MenuUserNL1" runat="server"></uc1:menuUserNL>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="2" cellpadding="2" width="100%">
                    <tr>
                        <td class="PageHeader" width="100%">
                            Reconciliation Report&nbsp;
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td>
                            <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td class="NormalBody" nowrap width="20%">
                                        Select Reconciliation Date :
                                    </td>
                                    <td nowrap width="80%">
                                        <asp:DropDownList ID="ddlday" runat="server" Width="65px" CssClass="MyInput" AutoPostBack="False">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMonth" runat="server" Width="65px" CssClass="MyInput" AutoPostBack="False">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlYear" runat="server" Width="65px" CssClass="MyInput" AutoPostBack="False">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnGetReconciliation" runat="server" Width="120px" CssClass="ButtonCss"
                                            Text="Get Reconciliation"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" nowrap width="20%">
                                    </td>
                                    <td class="NormalBody" nowrap width="80%">
                                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" nowrap width="20%">
                                        <asp:Label ID="lbl" runat="server">IPE rejections</asp:Label>
                                    </td>
                                    <td class="NormalBody" nowrap width="80%">
                                        <asp:Label ID="lblIPErejections" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" nowrap width="20%">
                                        <asp:Label ID="Label1" runat="server">Balance of rejections</asp:Label>
                                    </td>
                                    <td class="NormalBody" nowrap width="80%">
                                        <asp:Label ID="lblBalanceofrejections" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" nowrap width="20%" height="14">
                                        <asp:Label ID="Label2" runat="server">Change from previous</asp:Label>
                                    </td>
                                    <td class="NormalBody" nowrap width="80%" height="14">
                                        <asp:Label ID="lblChangefromprevious" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" nowrap width="20%" height="14">
                                        Total :
                                    </td>
                                    <td class="NormalBody" nowrap width="80%" height="14">
                                        <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" nowrap width="20%">
                                    </td>
                                    <td class="NormalBody" nowrap width="80%">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBody" nowrap align="left" width="20%" colspan="2">
                                        <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                                            <tr class="NormalBody">
                                                <td width="30%">
                                                    <strong></strong>
                                                </td>
                                                <td>
                                                    <strong>Stock</strong>
                                                </td>
                                                <td>
                                                    <strong>Expense</strong>
                                                </td>
                                                <td>
                                                    <strong>Total</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Scanned Docs In
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblScannedDocsIn_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblScannedDocsIn_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblScannedDocsIn_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%" height="20">
                                                    E-docs In
                                                </td>
                                                <td class="NormalBody" height="20">
                                                    <asp:Label ID="lblEdocsIn_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="20">
                                                    <asp:Label ID="lblEdocsIn_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="20">
                                                    <asp:Label ID="lblEdocsIn_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    InvUpload
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblInvUpload_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblInvUpload_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="InvUpload_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Debit Notes
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblDebitNotes_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblDebitNotes_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblDebitNotes_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Total :
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal1" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal2" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal3" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Received</SPAN>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblReceived_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblReceived_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblReceived_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%" height="17">
                                                    Approved
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblApproved_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblApproved_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblApproved_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%" height="17">
                                                    Deleted
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblDeleted_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblDeleted_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblDeleted_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Rejected
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblRejected_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblRejected_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblRejected_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Total&nbsp; :
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal4" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal5" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal6" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Rec :
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal7" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal8" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal9" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Registration
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblRegistration_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblRegistration_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblRegistration_tot" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Rec
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal10" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal11" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal12" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%" height="17">
                                                    Docs Approved/Rejected
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblDocsApprovedRejected_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblDocsApprovedRejected_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblTotal13" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%" height="17">
                                                    Authorization
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblAuthorization_st" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblAuthorization_ex" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody" height="17">
                                                    <asp:Label ID="lblTotal14" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                    Rec
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal15" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal16" runat="server">0</asp:Label>
                                                </td>
                                                <td class="NormalBody">
                                                    <asp:Label ID="lblTotal17" runat="server">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%" height="15">
                                                </td>
                                                <td class="NormalBody" height="15">
                                                </td>
                                                <td class="NormalBody" height="15">
                                                </td>
                                                <td class="NormalBody" height="15">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBody" width="30%">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                                <td class="NormalBody">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
