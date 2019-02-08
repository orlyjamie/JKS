<%@ Page Language="c#" CodeBehind="InvoiceAction.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.invoice.InvoiceAction" EnableEventValidation="true" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>P2D Network - Pass/Approve Invoice</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Utilities/ETH.css" type="text/css" rel="stylesheet">
    <script language="javascript">
		function CloseWindow()
		{
			if ('<%=iCurrentStatusID%>' == '4')
			{
			alert ('Sorry, cannot manipulate this invoice, it is completed.')
			window.close();
			}
		}
		function getQueryVariable(variable) 
		{
			var query = window.location.search.substring(1);
			var vars = query.split("&");
			for (var i=0;i<vars.length;i++) {
				var pair = vars[i].split("=");
				if (pair[0] == variable) {
				return pair[1];
				}
			} 
		}
		function doHourglass()
			{
				document.body.style.cursor = 'wait';				 
			}
		function windowclose()
		{		
			window.close();	
		}	
		
		function SubmitForm()
		{
			window.opener.document.forms[0].submit();
		}
		
		function CaptureClose(sInvoiceID,sDocType)
		{		
		
			document.body.style.cursor = 'wait';
			var id=getQueryVariable("InvoiceID");
			var docType=getQueryVariable("DocType");
			var sValue="id="+id+"|docType="+docType;
			
			//window.opener.__doPostBack('btnCloseAction',sValue);
		}
		
		function CheckOpenValid(TypeUser)
		{	
		    var uType=TypeUser	
		    //alert(document.getElementById("ddldept").options[document.getElementById("ddldept").selectedIndex].text!="Select")
			if(document.getElementById("ddldept").options[document.getElementById("ddldept").selectedIndex].text=="Select")
			{
				alert('Please select department');
				return false;
			}
		
			if(document.getElementById("ddlApprover1").selectedIndex == 0 && document.getElementById("ddlApprover2").selectedIndex == 0 && document.getElementById("ddlApprover3").selectedIndex == 0)
			{
				alert('Please select at least one Approvers');
				return false;
			}
			
			return true;
		}
		
		function DocURLJS()
		{			
			var invID = '<%= Request.QueryString["InvoiceID"] %>';
			var iRetUrl = '../Current/CurrentStatus.aspx';
			location.href="../invoice/InvoiceFileManager_NL.aspx?From=GMGRadio&InvoiceID="+invID+"&ReturnUrl="+iRetUrl;
		}
		
		function CountLeft(field, max) 
		{
			if (field.value.length > max)
			field.value = field.value.substring(0, max);
			
		}
		
		function GoToStockQC()
		{
			var strInvoiceID='';
			strInvoiceID=<%=Request.QueryString["InvoiceID"]%>;
			window.open('../../ETC/StockQC/InvoiceAction.aspx?InvoiceID='+strInvoiceID,'a','width=810,height=680,scrollbars=1,resizable=1');
		}
		
    </script>
</head>
<body onbeforeunload="doHourglass(); " bgcolor="#ffffff" leftmargin="0" topmargin="0"
    onload="javascript:CloseWindow();" onunload="javascript:CaptureClose();">
    <script language="javascript" src="../../dalkia/Supplier/wz_tooltip.js"></script>
    <form id="Form2" style="z-index: 102; left: 0px" method="post" runat="server">
    <table style="width: 280px; height: 495px" width="100%">
        <tr>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" cellspacing="1" cellpadding="1" width="909" border="0">
                    <tr>
                        <td class="PageHeader" style="height: 21px" colspan="5">
                            <asp:Label ID="lblConfirmation" runat="server" Visible="True" CssClass="PageHeader">Invoice Workflow</asp:Label>
                        </td>
                    </tr>
                    <tr width="100%">
                        <td style="height: 111px" width="100%">
                            <table class="tlbborder" style="width: 904px; height: 64px">
                                <tr class="NewBoldText">
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Document No</b>
                                    </td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b>Current Status</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCurrentStatus" runat="server" CssClass="NormalBody" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Document Date</b>
                                    </td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblInvoiceDate" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b>Business Unit</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBusinessUnit" runat="server" CssClass="NormalBody" Style="cursor: hand"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Supplier Name</b>
                                    </td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblSupplier" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <% if (Convert.ToInt32(Session["UserTypeID"]) != 1)
                                       {%>
                                    <td class="NormalBody" style="width: 138px">
                                        <b>Department</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDepartment" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <% } %>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Buyer Name</b>
                                    </td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblBuyer" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                        <b>
                                            <asp:Label ID="lblCRn" runat="server" CssClass="NormalBody" Width="104px">Credit Note No</asp:Label></b>
                                    </td>
                                    <td class="NormalBody" style="color: #ff0000">
                                        <asp:Label ID="lblcreditnoteno" runat="server" ForeColor="Red" Visible="False" CssClass="NormalBody"></asp:Label><asp:LinkButton
                                            ID="lnkCrn" runat="server" CssClass="NormalBody" ForeColor="Red" Visible="False"></asp:LinkButton>
                                        <%=GetCreditLinks()%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px; display: none">
                                        <asp:LinkButton ID="Linkbutton1" runat="server" CssClass="NormalBody" ForeColor="Red"></asp:LinkButton>
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                    </td>
                                    <td style="width: 250px">
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
                                    </td>
                                    <td class="NormalBody">
                                        <a href='#' onclick="GoToStockQC();" id="lnkVariance" runat="server"><b>Variance against
                                            PO </b></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 161px; height: 3px">
                            <asp:Label ID="lblApprovelMessage" runat="server" Visible="False" CssClass="NormalBody"
                                Font-Bold="True" Width="136px" ForeColor="Red">Approval Completed.</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" valign="top" colspan="5" height="100%">
                            <asp:DataGrid ID="grdList" runat="server" Width="896px" ShowFooter="True" AutoGenerateColumns="False"
                                GridLines="Vertical" CellPadding="0" CellSpacing="0" CssClass="listingArea">
                                <FooterStyle ForeColor="Black" BackColor="White"></FooterStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle BackColor="White"></ItemStyle>
                                <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Line No">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Font-Bold="True"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Company Name">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlBuyerCompanyCode" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged_ddlBuyerCompanyCode"
                                                runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Department">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDepartment1" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged_ddlDepartment"
                                                runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Nominal Code">
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged_ddlNominalCode"
                                                ID="ddlNominalCode1">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Code Description">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCodingDescription1" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged_ddlCodingDescription"
                                                runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Business Unit">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlBusinessUnit" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="PO Number">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPoNumber" Text='<%# DataBinder.Eval(Container, "DataItem.PurOrderNo") %>'
                                                runat="server" Width="70px">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table class="noBorder" id="Table3" cellspacing="0" cellpadding="0" width="100%"
                                                border="0">
                                                <tr>
                                                    <td class="noBorder" nowrap>
                                                        <asp:Label ID="Label1" runat="server" CssClass="NormalBody">Total Net Value for Coding:</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="noBorder" style="padding-right: 0px; padding-left: 6px; padding-bottom: 0px;
                                                        padding-top: 20px" nowrap>
                                                        <asp:Label ID="Label2" runat="server" CssClass="NormalBody">Net Invoice Total:</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Net value for coding">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="120px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNetVal" Text='<%# DataBinder.Eval(Container, "DataItem.NetValue") %>'
                                                runat="server" Width="120px">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table id="Table2" class="noBorder" cellspacing="0" cellpadding="0" width="100%"
                                                border="0">
                                                <tr>
                                                    <td nowrap="nowrap" class="noBorder">
                                                        <asp:Label ID="lblNetVal" runat="server" CssClass="NormalBody"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap" class="noBorder" style="padding-right: 0px; padding-left: 6px;
                                                        padding-bottom: 0px; padding-top: 20px">
                                                        <asp:Label ID="lblNetInvoiceTotal" runat="server" CssClass="NormalBody"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Delete Lines">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBox" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                                    Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr height="10">
                        <td style="height: 118px" align="center">
                            <table style="width: 904px; height: 64px">
                                <tr>
                                    <td style="width: 206px" align="right">
                                        <asp:TextBox Style="display: none" ID="txtNew" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 206px" align="right">
                                        <asp:Button ID="btnRetrieve" runat="server" Visible="False" CssClass="ButtonCssNew"
                                            BorderWidth="0px" BorderStyle="None" CausesValidation="False" Text="Reterieve Coding">
                                        </asp:Button>
                                    </td>
                                    <td style="width: 150px" align="center">
                                        <asp:Button ID="btnAddNew" runat="server" CssClass="ButtonCssNew" BorderWidth="0px"
                                            BorderStyle="None" CausesValidation="False" Text="Add New Line"></asp:Button>
                                    </td>
                                    <td style="width: 138px">
                                        <asp:Button ID="btnDelLine" runat="server" CssClass="ButtonCssNew" BorderWidth="0px"
                                            BorderStyle="None" CausesValidation="False" Text="Delete Line(s)"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                            <table class="tlbborder" style="width: 904px; height: 253px">
                                <% if (TypeUser > 1)
                                   {%>
                                <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                   {%>
                                <tr>
                                    <td style="width: 206px; height: 4px" align="right">
                                        <b class="NormalBody">Department&nbsp; :</b>
                                    </td>
                                    <td style="width: 526px; height: 4px" align="left">
                                        <asp:DropDownList ID="ddldept" runat="server" CssClass="MyInput" Width="200px" AutoPostBack="True"
                                            DataValueField="DepartmentID" DataTextField="Department">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 138px; height: 4px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 206px; height: 13px" align="right">
                                        <b class="NormalBody">Approval Path&nbsp; :</b>
                                    </td>
                                    <td style="width: 526px; height: 13px" align="left">
                                        <asp:DropDownList ID="ddlApprover1" runat="server" CssClass="MyInput" Width="96px"
                                            DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlApprover2" runat="server" CssClass="MyInput" Width="96px"
                                            DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlApprover3" Visible="False" runat="server" CssClass="MyInput"
                                            Width="96px" DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlApprover4" Visible="False" runat="server" CssClass="MyInput"
                                            Width="96px" DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlApprover5" Visible="False" runat="server" CssClass="MyInput"
                                            Width="96px" DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 138px; height: 13px">
                                    </td>
                                </tr>
                                <% }%>
                                <% }%>
                                <tr>
                                    <td style="width: 206px; height: 45px" align="right">
                                        <b class="NormalBody">Comments&nbsp; :</b>
                                    </td>
                                    <td style="width: 526px; height: 45px" align="left">
                                        <asp:TextBox ID="txtComment" runat="server" Width="432px" Height="38px" TextMode="MultiLine"
                                            MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td style="width: 138px; height: 45px">
                                    </td>
                                </tr>
                                <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                   {%>
                                <%
                                       if (RejectOpenFields == 0)
                                       {
                                %>
                                <tr>
                                    <td style="width: 206px; height: 30px" align="right">
                                        <b class="NormalBody">Rejection Code&nbsp; :</b>
                                    </td>
                                    <td style="width: 526px; height: 30px" align="left">
                                        <asp:DropDownList ID="ddlRejection" runat="server" Width="432px" DataValueField="RejectionCodeID"
                                            DataTextField="RejectionCode" Font-Size="XX-Small">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 138px; height: 30px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 206px; height: 42px" align="right">
                                        <b class="NormalBody"><b class="NormalBody">Rejection </b>Comments&nbsp; :</b>
                                    </td>
                                    <td style="width: 526px; height: 42px" align="left">
                                        <asp:TextBox ID="tbRejection" runat="server" Width="432px" Height="34px" TextMode="MultiLine"
                                            MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td style="width: 138px; height: 42px">
                                    </td>
                                </tr>
                                <% } %>
                                <%
                                       if (RejectOpenFields == 1 || ReopenAtApprover == 1 || ((Convert.ToInt32(ViewState["StatusID"]) == 21 || Convert.ToInt32(ViewState["StatusID"]) == 22) & TypeUser > 1))
                                       {
                                %>
                                <tr>
                                    <td style="width: 206px" align="right">
                                        <b class="NormalBody"><b class="NormalBody">CreditNote No</b>&nbsp; :&nbsp;&nbsp;</b>
                                    </td>
                                    <td style="width: 526px" align="left">
                                        <asp:TextBox ID="txtCreditNoteNo" runat="server" Width="192px" Height="20px" TextMode="SingleLine"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblTextReopen" runat="server" CssClass="NormalBody" Font-Bold="True"
                                            Width="152px" ForeColor="Red">Reopen at Approver 1 :</asp:Label><asp:CheckBox ID="chbOpen"
                                                runat="server" CssClass="NormalBody"></asp:CheckBox>
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                                <% } %>
                                <% } %>
                                <tr>
                                    <td style="width: 206px" align="right">
                                    </td>
                                    <td style="width: 526px" align="left">
                                        <span class="NormalBody" style="color: #ff0000"></span>
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center">
                                        <asp:Button ID="btnCancel" runat="server" CssClass="ButtonCssNewAkkeron" BorderWidth="0px"
                                            BorderStyle="None" CausesValidation="False" Text="Cancel"></asp:Button>
                                        <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                           {%>
                                        <asp:Button ID="btnReject" CssClass="ButtonCssNewAkkeron" BorderStyle="None" CausesValidation="False"
                                            Text="Reject" runat="server" ToolTip="Reject"></asp:Button>
                                        <asp:Button ID="btnReopen" CssClass="ButtonCssNewAkkeron" BorderStyle="None" CausesValidation="False"
                                            Text="Reopen" runat="server" ToolTip="Reopen"></asp:Button>
                                        <% } %>
                                        <asp:Button ID="btnApprove" CssClass="ButtonCssNewAkkeron" BorderStyle="None" CausesValidation="False"
                                            Text="Approve" runat="server" ToolTip="Approve"></asp:Button>
                                        <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                           {%>
                                        <asp:Button ID="btnOpen" CssClass="ButtonCssNewAkkeron" BorderStyle="None" CausesValidation="False"
                                            Text="Open" runat="server" ToolTip="Open"></asp:Button>
                                        <% } %>
                                        <% if (!(Convert.ToInt32(ViewState["StatusID"]) == 30))
                                           {%>
                                        <asp:Button ID="btndelete" CssClass="ButtonCssNewAkkeron" BorderStyle="None" CausesValidation="False"
                                            Text="Delete" runat="server" ToolTip="Delete"></asp:Button>
                                        <% } %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 206px" align="right">
                                    </td>
                                    <td style="width: 526px" align="center">
                                        <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="True" Font-Size="8" Width="314px"
                                            ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                    <td style="width: 138px" align="left">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblG2App" Style="display: none" runat="server"></asp:Label>
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="grdFile" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
                    CellPadding="0" CellSpacing="0" CssClass="listingArea">
                    <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                    <ItemStyle></ItemStyle>
                    <HeaderStyle CssClass="tableHd"></HeaderStyle>
                    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="Invoice No." Visible="False">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblInvNo" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceNo") %>'>
                                </asp:Label>
                                <asp:Label runat="server" ID="lblInvID" Text='<%# DataBinder.Eval(Container, "DataItem.InvoiceID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="File Name" Visible="False">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="70%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDocID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.DocumentID") %>'>
                                </asp:Label>
                                <asp:Label ID="lblHidePath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ImagePath") %>'>
                                </asp:Label>
                                <asp:Label ID="lblArchPath" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.archiveImagePath") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="ATTACHMENTS">
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="noBorder"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="HpDownload" CommandArgument="DOW" BorderWidth="0" runat="server">
                                    <asp:Label ID="lblPath" runat="server"></asp:Label></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" HeaderText="Delete">
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="HpDel" CommandArgument="DEL" BorderWidth="0" runat="server"
                                    ImageUrl="../../images/delete.gif"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6" PageButtonCount="20"
                        Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td>
                <p class="normalbody" align="center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="ErrMsg">No Files Found</asp:Label></p>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
