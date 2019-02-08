<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="menuUserNL.ascx.cs"
    Inherits="CBSolutions.ETH.Web.ETC.Utilities.menuUserNL" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="JavaScript" type="text/JavaScript">
<!--
    function MM_swapImgRestore() { //v3.0
        var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
    }

    function MM_preloadImages() { //v3.0
        var d = document; if (d.images) {
            if (!d.MM_p) d.MM_p = new Array();
            var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
                if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
        }
    }

    function MM_findObj(n, d) { //v4.01
        var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
            d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
        }
        if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
        for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
        if (!x && d.getElementById) x = d.getElementById(n); return x;
    }

    function MM_swapImage() { //v3.0
        var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
            if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
    }
    function fnOpen() {
        //alert('Action executed – please check the results');
        // alert('The export has started. Depending on the number of invoices it may take up to 30 minutes to complete. Please do NOT press the export button again during this period.');
        document.getElementById("MenuUserNL1_btnExportInvoice").click();
        // alert('Call');
        // window.location.href='../History/ExportInvoice.aspx';
    }
    function fnExportInvoice() {
        alert('The export has started. Depending on the number of invoices it may take up to 30 minutes to complete. Please do NOT press the export button again during this period.');
        window.location.href = '../History/ExportInvoice.aspx';
    }
//-->
</script>
<body onload="MM_preloadImages('../../images/currentNL_.gif','../../images/historyNL_.gif','../../images/changepasswordNL_.gif','../../images/companymanagementNL_.gif','../../images/thresholdNL_.gif','../../images/usermanagementNL_.gif','../../images/branchmanagementNL_.gif','../../images/tradingrelationshipNL_.gif','../../images/downloaddatabaseNL_.gif','../../images/StockQCNL_.gif','../../images/ScanQCNL_.gif')">
    <%-- <table width="100" border="0" cellspacing="0" cellpadding="0">
        <% if (Convert.ToInt32(Session["UserTypeID"]) != 11)
           {%>
        <tr>
            <td height="25" align="left" valign="top">
                <a href="../Current/CurrentStatus.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image1','','../../images/currentNL_.gif',1)">
                    <img src="../../images/currentNL.gif" alt="Current" name="Image1" width="152" height="23"
                        border="0"></a>
            </td>
        </tr>
        <tr>
            <td height="25" align="left" valign="top">
                <a href="../StockQC/CurrentInvoice.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image10','','../../images/matching_.gif',1)">
                    <img src="../../images/matching.gif" alt="Stock QC" name="Image10" width="152" height="23"
                        border="0"></a>
            </td>
        </tr>
        <tr>
            <td height="25" align="left" valign="top">
                <a href="../History/etchistory.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image2','','../../images/historyNL_.gif',1)">
                    <img src="../../images/historyNL.gif" alt="History" name="Image2" width="152" height="23"
                        border="0"></a>
            </td>
        </tr>
        <% } %>

        <% if (Convert.ToInt32(Session["UserTypeID"]) == 11 || Convert.ToInt32(Session["UserTypeID"]) == 12 || Convert.ToInt32(Session["UserTypeID"]) == 3)
           {%>
        <tr>
            <td height="25" align="left" valign="top">
                <a href="../CMS/CMS.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image21','','../../images/cms_.gif',1)">
                    <img src="../../images/cms_.gif" alt="Weekly Trading Report" name="Image21" width="152"
                        height="23" border="0"></a>
            </td>
        </tr>
        <tr>
            <td height="25" align="left" valign="top">
                <a href="../CMS/CMSHistoryNew.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image22','','../../images/CMShis_.gif',1)">
                    <img src="../../images/CMShis_.gif" alt="WTR History" name="Image22" width="152"
                        height="23" border="0"></a>
            </td>
        </tr>
        <% } %>

        <tr>
            <td height="25" align="left" valign="top">
                <a href="../User/changepassword.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image3','','../../images/changepasswordNL_.gif',1)">
                    <img src="../../images/changepasswordNL.gif" alt="Change Password" name="Image3"
                        width="152" height="23" border="0"></a>
            </td>
        </tr>

        <% if (Convert.ToInt32(Session["UserTypeID"]) < 11)
           {%>
            <% if (Convert.ToInt32(Session["UserTypeID"]) > 2)
               {%>
            <tr>
                <td height="25" align="left" valign="top">
                    <a href="../Supplier/BrowseSupplier.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image20','','../../images/Supplier-Management_.gif',1)">
                        <img src="../../images/SupplierManagement.gif" alt="Supplier Management" name="Image20"
                            width="152" height="23" border="0"></a>
                </td>
            </tr>
            <tr>
                <td height="25" align="left" valign="top">
                    <a href="../User/UserBrowse.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image6','','../../images/usermanagementNL_.gif',1)">
                        <img src="../../images/usermanagementNL.gif" alt="User Management" name="Image6"
                            width="152" height="23" border="0"></a>
                </td>
            </tr>
            <tr>
                <td height="25" align="left" valign="top">
                    <a href="../downloaddb/d_main.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','../../images/datafiles_.gif',1)">
                        <img src="../../images/datafiles.gif" alt="Download Database" name="Image9" width="152"
                            height="23" border="0"></a>
                </td>
            </tr>
            <% } %>

            <% if (Convert.ToInt32(Session["UserTypeID"]) > 1)
               {%>
            <tr>
                <td height="25" align="left" valign="top">
                    <a href="../StockQC/ScanQCDoc.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','../../images/ScanQCNL_.gif',1)">
                        <img src="../../images/ScanQCNL.gif" alt="Scan QC" name="Image11" width="152" height="23"
                            border="0"></a>
                </td>
            </tr>
            <% } %>

            <% if (Convert.ToInt32(Session["UserTypeID"]) == 3)
               {%>
            <tr>
                <td height="25" align="left" valign="top">
                    <a href="../Reconciliation/RecReport.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','../../images/reconciliationreport_.gif',1)">
                        <img src="../../images/reconciliationreport.gif" alt="Reconciliation" name="Image12"
                            width="152" height="23" border="0"></a>
                </td>
            </tr>
            <% } %>

            <% if (Convert.ToInt32(Session["UserTypeID"]) > 2)
              {%>
                <tr>
                    <td height="25" align="left" valign="top">
                        <a href="../ApprovalPath/BrowseApprovalPath.aspx" onmouseout="MM_swapImgRestore()"
                            onmouseover="MM_swapImage('Image13','','../../images/setapprovers2.gif',1)">
                            <img src="../../images/setapprovers1.gif" alt="Set Approval Path" name="Image13"
                                width="152" height="23" border="0">
                        </a>
                    </td>
                </tr>
                <tr>
                    <td height="25" align="left" valign="top">
                        <a href="../downloadDB/ExportInvoice.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','../../images/InvoiceDataExport.gif',1)">
                            <img src="../../images/InvoiceDataExport.gif" alt="Invoice Data Export" name="Image13"
                                width="152" height="23" border="0"></a>
                    </td>
                </tr>
           <% } %>
        <% } %>
        <% if (Convert.ToInt32(Session["UserTypeID"]) == 3)
           {%>
        <tr>
            <td height="25" align="left" valign="top">
                <a onclick="javascript:fnOpen();" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image15','','../../images/exportInvoices.gif',1)">
                    <img src="../../images/exportInvoices.gif" alt="Export Invoices" name="Image14" width="152"
                        height="23" border="0"></a>
            </td>
        </tr>
        <tr>
            <td height="25" align="left" valign="top">
                <a href="../CMS/wtrexport.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image22','','../../images/wtrexport.gif',1)">
                    <img src="../../images/wtrexport.gif" alt="WTR Export" name="Image22" width="152"
                        height="23" border="0"></a>
            </td>
        </tr>
        <% } %>
    </table>--%>
    <!-------------------- START: Navigation -------------------->
    <div class="container">
        <div class="row">
            <nav class="navbar navbar-inverse main_nav" role="navigation">
                            <div class="navbar-header">
                                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                                    <span class="sr-only">Toggle navigation</span>
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                </button>
                                <!--<a class="navbar-brand" href="#">Menu</a>-->
                            </div>
                            <div class="collapse navbar-collapse">
                                <ul class="nav navbar-nav">
                                <% if (Session["FirstLoginPageVisited"] == "Yes")
                                   {%>
                                     <li id="li1"><a href="#">CURRENT</a></li>
                                     <li id="li2"><a href="#">HISTORY</a></li>
                                        <% } %>
                        <% else
                                   { %>

                                  <% if (Convert.ToInt32(Session["UserTypeID"]) != 11)
                                     {%>
                                    <li id="liCurrent"><a href="../Current/CurrentStatus.aspx">CURRENT</a></li>
                                     <li id="liHistory"><a href="../History/history.aspx">HISTORY</a></li>
                                  <% } %>
                         <% } %>


                               <% if (Convert.ToInt32(Session["UserTypeID"]) < 11)
                                  {%>


                                

                               <% if (Convert.ToInt32(Session["UserTypeID"]) > 1)
                                  {%>


                                  <% if (Session["FirstLoginPageVisited"] == "Yes")
                                     {%>
                                           <li id="li3"><a href="#">SCAN QC</a></li>
                                     <% } %>
                                    <% else
                                     { %>
                                            <li id="liScanQCDoc"><a href="../ScanQC/ScanQCDoc.aspx">SCAN QC</a></li>
                                            <%--../StockQC/ScanQCDoc.aspx--%>
                                     <% } %>



                              <% } %>


                               
                                        <%-- Modified for AP user by kuntalkarar on 27thMay2017--%>
                              <% if (Convert.ToInt32(Session["UserTypeID"]) >= 2)
                                 {%>


                                           <% if (Session["FirstLoginPageVisited"] == "Yes")
                                              {%>
                                            <li id="li4"><a  href="#" >SUPPLIERS</a></li>
                                              <% } %>
                                            <% else
                                              { %>
                                             <li id="liSupplier"><a  href="../Supplier/BrowseSupplier.aspx" >SUPPLIERS</a></li>
                                              <% } %>
                                           
                                           
                                           
                                <% } %>


                                   <% if (Convert.ToInt32(Session["UserTypeID"]) > 2)
                                      {%>



                                          <% if (Session["FirstLoginPageVisited"] == "Yes")
                                             {%>
                                                        <li id="li5"><a  href="#" >USERS</a></li>
                                                <% } %>
                                                 <% else
                                             { %>
                                                        <li id="liUser"><a  href="../User/UserBrowse.aspx" >USERS</a></li>
                                               <% } %>
                                        
                                        
                                        
                                 <% } %>


                                         <%-- Modified for AP user by kuntalkarar on 27thMay2017--%>
                                          <% if (Convert.ToInt32(Session["UserTypeID"]) >= 2)
                                             {%>



                                            <% if (Session["FirstLoginPageVisited"] == "Yes")
                                               {%>
                                                  <li id="li6"> <a href="#">APPROVAL PATHS</a> </li>
                                           <% } %>                                       
                                        <% else
                                               { %>
                                                       <li id="liApprovalPath"> <a href="../ApprovalPath/BrowseApprovalPath.aspx">APPROVAL PATHS</a> </li>
                                           <% } %>                                  

                                      
                                      
                                      
                                       <% } %>





                                        <%-- Modified for AP user by kuntalkarar on 27thMay2017--%>
                                       <% if (Convert.ToInt32(Session["UserTypeID"]) != 1 && Convert.ToInt32(Session["UserTypeID"]) != 2)   // If UserType = User|| AP user  then REC REPORT tab doesn't shown.
                                          { %>





                                           <% if (Session["FirstLoginPageVisited"] == "Yes")
                                              {%>
                                     <li id="li7"><a href="#">REC REPORT</a></li>
                                         <% } %>
                                        <% else
                                              { %>
                                               <li id="liRecReport"><a href="../Reconciliation/RecReport.aspx">REC REPORT</a></li>
                                         <% } %>

                                       
                                       
                                        
                                        <% } %>




                                       <% if (Convert.ToInt32(Session["UserTypeID"]) > 2)
                                          {%>



                                           <% if (Session["FirstLoginPageVisited"] == "Yes")
                                              {%>
                                                <li id="li8"><a href="#">DATA FILES</a></li>
                                           <% } %>
                                        <% else
                                              { %>
                                               <li id="liDataFile"><a href="../downloaddb/d_main.aspx">DATA FILES</a></li>
                                           <% } %>



                                  <% } %>


                                       <%--<% if (Convert.ToInt32(Session["UserTypeID"]) > 2)
                                          {%>
                                          <li><a href="../downloadDB/ExportInvoice.aspx">Invoice Data Export</a></li>
                                       <% } %>--%>





                                <% } %>
                                <%-- Modified for AP user by kuntalkarar on 27thMay2017--%>
                                <% if (Convert.ToInt32(Session["UserTypeID"]) == 3 || Convert.ToInt32(Session["UserTypeID"]) == 2)
                                   {%>
                                   <%--<li id="liExport"><a  onclick="javascript:fnOpen();">EXPORT</a></li>--%>



                                    <% if (Session["FirstLoginPageVisited"] == "Yes")
                                       {%>
                                             <li id="li9"><a href="#">EXPORT</a></li>
                                            <% } %>                                                   
                                            <% else
                                       { %>
                                              <li id="liExport"><a href="../History/ExportInvoiceNew.aspx">EXPORT</a></li>
                                           <% } %>
                               
                               
                               
                                <% } %>

                                 <%--<% if (Convert.ToInt32(Session["UserTypeID"]) != 11)
                                    {%>
                                      <% if (Convert.ToInt32(Session["UserTypeID"]) != 1)     // If UserType = User then MATCHING tab doesn't shown.
                                         { %>

                                         <% if (Session["FirstLoginPageVisited"] == "Yes")
                                            {%>
                                             <li id="li10"><a href="#">MATCHING</a></li>
                                            <% } %>
                                             <% else
                                            { %>
                                               <li id="liMatching"><a href="../StockQC/CurrentInvoice.aspx">MATCHING</a></li>
                                           <% } %>

                                  <% } %>
                                <% } %>--%>


                                             <% if (Session["FirstLoginPageVisited"] == "Yes")
                                                {%>
                                                     <li id="li11"><a href="#">PASSWORD</a></li>
                                             <% } %>
                                              <% else
                                                { %>
                                              <li id="liPassword"><a href="../User/changepassword.aspx">PASSWORD</a></li>
                                           <% } %>


                                 <li><a href="../../JKSDefault.aspx" class="nobg">LOG OFF</a></li>

                                <%-- <% if (Convert.ToInt32(Session["UserTypeID"]) == 3)
                                   {%>
                                <li><a href="../CMS/wtrexport.aspx">WTR Export</a></li> 
                                <% } %>--%>
                                   
                                   
                                </ul>
                            </div><!--/.nav-collapse -->
                        </nav>
        </div>
        <% if (Session["FirstLoginPageVisited"] != "Yes")
           {%>
        <asp:Button Style="visibility: hidden; display: none;" ID="btnExportInvoice" runat="server">
        </asp:Button>
        <% } %>
    </div>
    <!-------------------- END: Navigation -------------------->
    <script type="text/javascript">
        function SelectTab(tab) {

            if (tab == "Current") {
                document.getElementById("liCurrent").className = "active";
            }
            else if (tab == "History") {
                document.getElementById("liHistory").className = "active";
            }
            else if (tab == "ScanQCDoc") {
                document.getElementById("liScanQCDoc").className = "active";
            }
            else if (tab == "Supplier") {
                document.getElementById("liSupplier").className = "active";
            }
            else if (tab == "User") {
                document.getElementById("liUser").className = "active";
            }
            else if (tab == "ApprovalPath") {
                document.getElementById("liApprovalPath").className = "active";
            }
            else if (tab == "RecReport") {
                document.getElementById("liRecReport").className = "active";
            }
            else if (tab == "Password") {
                document.getElementById("liPassword").className = "active";
            }
            else if (tab == "DataFile") {
                document.getElementById("liDataFile").className = "active";
            }
            else if (tab == "ExportInvoice") {
                document.getElementById("liExport").className = "active";
            }
            else if (tab == "Matching") {
                document.getElementById("liMatching").className = "active";
            }
        }
    </script>
</body>
