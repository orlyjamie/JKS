<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="menuUser.ascx.cs" Inherits="CBSolutions.ETH.Web.menuUser"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:HyperLink ID="hypCreateInvoice" ImageUrl="../images/SendInvoiceE.jpg" runat="server"
    NavigateUrl="../Invoice/default.aspx">Create/ Upload Invoice</asp:HyperLink>
<asp:HyperLink ID="hypSalesInvoiceHistory" ImageUrl="../images/SalesInvoiceE.jpg"
    runat="server" NavigateUrl="../Invoice/SalesInvoiceHistory.aspx">Sales Invoice History</asp:HyperLink>
<asp:HyperLink ID="hypSalesOrders" ImageUrl="../images/salesorder.jpg" runat="server"
    NavigateUrl="../SalesOrders/SalesInvoiceHistory_PO.aspx">Sales Orders</asp:HyperLink>
<asp:HyperLink ID="hypPassInv" ImageUrl="../images/purchaseinvoice.gif" runat="server"
    NavigateUrl="../Invoice/PassInvoice.aspx">Pass Invoice</asp:HyperLink>
<asp:HyperLink ID="hypPurchaseInvoiceHistory" ImageUrl="../images/purchaseinvoicehistory.gif"
    runat="server" NavigateUrl="../Invoice/PurchaseInvoiceHistory.aspx">Purchase Invoice History</asp:HyperLink>
<% if (Convert.ToInt32(Session["CompanyTypeID"]) == 0)
   {%>
<asp:HyperLink ID="hypUserManagement" ImageUrl="../images/UserManagementE.jpg" runat="server"
    NavigateUrl="../User/UserCompanyBrowse.aspx">Users Management</asp:HyperLink><asp:HyperLink
        ID="Hyperlink5" ImageUrl="../images/CompanyManagementE.jpg" runat="server" NavigateUrl="../Company/CompanyRedirect.aspx"
        Visible="True"> Company Management</asp:HyperLink><asp:HyperLink ID="Hyperlink6"
            ImageUrl="../images/BranchManagementE.jpg" runat="server" NavigateUrl="../Branch/BranchBrowse.aspx">Branch Management</asp:HyperLink>
<% }
   else
   {%>
<% if (Convert.ToInt32(Session["CompanyTypeID"]) == 2)
   {%>
<% if (Convert.ToInt32(Session["UserTypeID"]) > 1)
   {%>
<asp:HyperLink ID="Hyperlink4" ImageUrl="../images/UserManagementE.jpg" runat="server"
    NavigateUrl="../User/UserCompanyBrowse.aspx">Users Management</asp:HyperLink>
<%}
   }%>
<% if (Convert.ToInt32(Session["GMG"]) == 1)
   {%>
<% if (Convert.ToInt32(Session["UserTypeID"]) >= 3)
   {%>
<asp:HyperLink ID="Hyperlink1" ImageUrl="../images/UserManagementE.jpg" runat="server"
    NavigateUrl="../User/UserBrowse.aspx">Users Management</asp:HyperLink>
<asp:HyperLink ID="hypCompanyManagement" ImageUrl="../images/CompanyManagementE.jpg"
    runat="server" NavigateUrl="../Company/CompanyRedirect.aspx" Visible="False"> Company Management</asp:HyperLink>
<asp:HyperLink ID="hypBranchManagement" ImageUrl="../images/BranchManagementE.jpg"
    runat="server" NavigateUrl="../Branch/BranchBrowse.aspx">Branch Management</asp:HyperLink>
<asp:HyperLink ID="hypThreshold" ImageUrl="../images/threshold.gif" runat="server"
    NavigateUrl="../Company/threshold.aspx" Visible="true"> Set Threshold Values</asp:HyperLink>
<%}%>
<% }
   else
   {%>
<asp:HyperLink ID="Hyperlink2" ImageUrl="../images/CompanyManagementE.jpg" runat="server"
    NavigateUrl="../Company/CompanyRedirect.aspx" Visible="False"> Company Management</asp:HyperLink>
<asp:HyperLink ID="Hyperlink3" ImageUrl="../images/BranchManagementE.jpg" runat="server"
    NavigateUrl="../Branch/BranchBrowse.aspx">Branch Management</asp:HyperLink>
<%}%>
<%}%>
<asp:HyperLink ID="hypSupplierRelation" ImageUrl="../images/TradingRelationE.jpg"
    runat="server" NavigateUrl="../Company/TradingRelation.aspx" Visible="False">Trading Relations</asp:HyperLink>
<asp:HyperLink ID="hypCustomerRelation" ImageUrl="../images/BranchRelationE.jpg"
    runat="server" NavigateUrl="../Branch/BranchRelation.aspx" Visible="False">Branch  Relations</asp:HyperLink>
<asp:HyperLink ID="hypDownloadDatabase" ImageUrl="../images/DownloadDatabase.jpg"
    runat="server" NavigateUrl="../downloaddb/d_main.aspx" Visible="False">Download Database</asp:HyperLink>
<asp:HyperLink ID="hypDownloadData" ImageUrl="../images/DownloadDatabase.gif" runat="server"
    NavigateUrl="../downloaddb/d_main.aspx">Download Data</asp:HyperLink>
<!--
<asp:hyperlink id="hypUserTradingRelation" ImageUrl="../images/TradingRelationE.jpg" runat="server"
	NavigateUrl="../User/UserTradingRelation.aspx">Trading Relations</asp:hyperlink>
	
-->
<asp:HyperLink ID="hypApproveInv" ImageUrl="../images/ApproInv.jpg" runat="server"
    NavigateUrl="../Invoice/ApproveSalesInvoice.aspx" Visible="False">Approve Invoice</asp:HyperLink>
<asp:HyperLink ID="hypRelationshipManager" ImageUrl="../images/relationshipmanager.gif"
    runat="server" NavigateUrl="../sales/rel_manager.aspx" Visible="False">Relationship Manager</asp:HyperLink>
<asp:HyperLink ID="hypSalesPerson" ImageUrl="../images/salesperson.gif" runat="server"
    NavigateUrl="../sales/add_edit_saleperson.aspx" Visible="False">Sales Person</asp:HyperLink>
<asp:HyperLink ID="hypCreateCreditNotes" ImageUrl="../images/sendcreditnotes.gif"
    runat="server" NavigateUrl="../creditnotes/default_CN.aspx">Create/ Upload Credit Notes</asp:HyperLink>
<asp:HyperLink ID="hypCreateCreditNotesHistory" ImageUrl="../images/creditnoteslog.gif"
    runat="server" NavigateUrl="../creditnotes/SalesInvoiceHistory_CN.aspx">Credit Notes History</asp:HyperLink>
<asp:HyperLink ID="hypCreditNotesCurrent" ImageUrl="../images/creditnotescurrent.gif"
    runat="server" NavigateUrl="../creditnotes/PassInvoice_CN.aspx">Pass Credit Note</asp:HyperLink>
<asp:HyperLink ID="hypCreditNotesHistory" ImageUrl="../images/creditnoteshistory.gif"
    runat="server" NavigateUrl="../creditnotes/PurchaseInvoiceHistory_CN.aspx">Credit Note History</asp:HyperLink>
<asp:HyperLink ID="hypDebitNote" ImageUrl="../images/debitnoteslog.gif" runat="server"
    NavigateUrl="../debitnote/debitnotehistory.aspx">Debit Note History</asp:HyperLink>
<asp:HyperLink ID="hypChangePassword" ImageUrl="../images/changepassword.gif" runat="server"
    NavigateUrl="../User/changepassword.aspx">Change Password</asp:HyperLink>
<asp:HyperLink ID="hypExport" ImageUrl="../images/export.gif" runat="server" NavigateUrl="../utilities/export.aspx">Export</asp:HyperLink>
<asp:HyperLink ID="hypImport" ImageUrl="../images/import.gif" runat="server" NavigateUrl="../utilities/import.aspx">Import</asp:HyperLink>
<% if (Convert.ToInt32(Session["CompanyTypeID"]) == 1)
   {%>
<% if (Convert.ToInt32(Session["UserTypeID"]) == 3 || Convert.ToInt32(Session["UserTypeID"]) == 2)
   {%>
<asp:HyperLink ID="HyperLink9" ImageUrl="../images/scanqc.gif" NavigateUrl="../invoice/ScanQCDoc.aspx"
    runat="server"></asp:HyperLink>
<%}%>
<%}%>
<% if (Convert.ToInt32(Session["CompanyTypeID"]) == 2)
   {%>
<asp:HyperLink ID="hypP2DInvoice" ImageUrl="../images/P2DInvoice.jpg" runat="server"
    NavigateUrl="../Billing/BillingLists.aspx">HyperLink</asp:HyperLink>
<%}%>
