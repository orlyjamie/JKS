<%@ Page Language="c#" CodeBehind="ActionCredit.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.creditnotes.ActionCredit" %>

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
		
		function SubmitForm()
		{
			window.opener.document.forms[0].submit();
		}
		
//		function CaptureClose()
//		{
//			alert('CreditNote Deleted Successfully.');			
//			window.opener.Form1.btnSearch.click();
//			self.close();
//		}



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
		function CaptureClose()
		{
			
			
			document.body.style.cursor = 'wait';
					var id=getQueryVariable("InvoiceID");				
					var docType=getQueryVariable("DocType");
					var sValue="id="+id+"|docType="+docType;
				
					window.opener.__doPostBack('btnCloseAction',sValue); 
		}


		function ApproveClose()
		{			
			alert('CreditNote Approved Successfully.');			
			window.opener.Form1.btnSearch.click();
			self.close();
		}
		function unload()
		{
		    window.opener.Form1.btnSearch.click();
		}
		function CheckOpenValid()
		{		
			if(document.getElementById("ddldept").selectedIndex == 0)
			{
				alert('Please select department');
				return false;
			}
			
			return true;
		}
		
		
		function GoToStockQC()
		{
			var strInvoiceID='';
			strInvoiceID=<%=Request.QueryString["InvoiceID"]%>;
			window.open('../../ETC/StockQC/CreditStkAction.aspx?InvoiceID='+strInvoiceID,'a','width=900,height=640,resizable=1');
		}
    </script>
</head>
<body bgcolor="#ffffff" leftmargin="0" topmargin="0" onload="javascript:CloseWindow();"
    onunload="javascript:unload();">
    <form id="Form2" style="z-index: 102; left: 0px" method="post" runat="server">
    <table style="width: 909px; height: 504px" width="909">
        <tr>
            <td valign="top">
                <!-- Main Content Panel Starts-->
                <table id="Table1" style="width: 872px; height: 503px" cellspacing="1" cellpadding="1"
                    width="872" border="0">
                    <tr>
                        <td class="PageHeader" style="height: 21px" colspan="5">
                            <asp:Label ID="lblConfirmation" runat="server" Visible="True" CssClass="PageHeader">Invoice WorkFlow</asp:Label>
                        </td>
                    </tr>
                    <tr width="100%">
                        <td style="height: 111px" width="100%">
                            <table class="tlbborder" style="width: 1040px; height: 111px">
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                    </td>
                                    <td style="width: 225px">
                                    </td>
                                    <td class="NormalBody" style="width: 169px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="NewBoldText">
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Document No</b>
                                    </td>
                                    <td style="width: 225px">
                                        <asp:Label ID="lblRefernce" runat="server" CssClass="NormalBody" Width="216px"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 169px">
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
                                    <td style="width: 225px">
                                        <asp:Label ID="lblInvoiceDate" runat="server" CssClass="NormalBody" Width="224px"></asp:Label>
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
                                    <td style="width: 225px">
                                        <asp:Label ID="lblSupplier" runat="server" CssClass="NormalBody" Width="216px"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 169px">
                                        <b>Department</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDepartment" runat="server" CssClass="NormalBody" Width="208px"></asp:Label>
                                        <% if (TypeUser >= 2 && Convert.ToInt32(ViewState["StatusID"]) == 20)
                                           { %>
                                        <asp:DropDownList ID="ddldept" runat="server" CssClass="MyInput" Width="200px" DataValueField="DepartmentID"
                                            DataTextField="Department">
                                        </asp:DropDownList>
                                        <% } %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                        <b>Buyer Name</b>
                                    </td>
                                    <td style="width: 225px">
                                        <asp:Label ID="lblBuyer" runat="server" CssClass="NormalBody" Width="208px"></asp:Label>
                                    </td>
                                    <td class="NormalBody" style="width: 169px">
                                        <b>
                                            <asp:Label ID="lblCRn" runat="server" CssClass="NormalBody" Width="160px">Associated Invoice No</asp:Label>
                                        </b>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnEditAssociatedInvoiceNo" runat="server" CssClass="ButtonCssNew"
                                                        Width="100px" BorderWidth="0px" BorderStyle="None" Text="Submit" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbcreditnoteno" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblcreditnoteno" runat="server" CssClass="NormalBody" Width="216px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                    </td>
                                    <td style="width: 225px">
                                    </td>
                                    <td class="NormalBody" style="width: 169px">
                                        <a onclick="GoToStockQC();" href="#" id="lnkVariance" runat="server"><b>Variance against
                                            PO </b></a>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" style="width: 161px; height: 3px">
                            <asp:Label ID="lblApprovelMessage" runat="server" Visible="False" CssClass="NormalBody"
                                Width="136px" Font-Bold="True" ForeColor="Red">Approval Completed.</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBody" valign="top" colspan="5" height="60">
                            <asp:DataGrid ID="grdList" runat="server" Width="896px" ShowFooter="True" CssClass="listingArea"
                                AutoGenerateColumns="False" GridLines="Vertical" CellPadding="2">
                                <FooterStyle ForeColor="Black" BackColor="White"></FooterStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle></ItemStyle>
                                <HeaderStyle CssClass="tableHd"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Sl No">
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
                                    <asp:TemplateColumn HeaderText="Net value for coding" ItemStyle-Width="60px" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNetVal" Text='<%# DataBinder.Eval(Container, "DataItem.NetValue") %>'
                                                runat="server">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr>
                                                    <td nowrap style="border: none">
                                                        <asp:Label ID="lblNetVal" runat="server" CssClass="NormalBody"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap style="border: none">
                                                        <br>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap style="border: none">
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
                        <td style="height: 43px" align="center">
                            <table style="width: 100%; height: 78px">
                                <tr>
                                    <td style="width: 25%" align="center">
                                    </td>
                                    <td style="width: 25%" align="center">
                                    </td>
                                    <td style="width: 25%" align="center">
                                        <asp:Button ID="btnAddNew" runat="server" CssClass="ButtonCssNew" BorderWidth="0px"
                                            BorderStyle="None" Text="Add New Line" CausesValidation="False"></asp:Button>
                                    </td>
                                    <td style="width: 25%" align="center">
                                        <asp:Button ID="btnDelLine" runat="server" CssClass="ButtonCssNew" BorderWidth="0px"
                                            BorderStyle="None" Text="Delete Line(s)" CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%" align="center">
                                    </td>
                                    <td style="width: 25%" align="left" colspan="2" rowspan="3">
                                        <asp:TextBox ID="txtComment" runat="server" Style="width: 368px; height: 50px" TextMode="MultiLine"
                                            MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td style="width: 25%" align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%" align="right" class="NormalBody">
                                        <b>Comments</b>
                                    </td>
                                    <td style="width: 25%" align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%" align="center">
                                    </td>
                                    <td style="width: 25%" align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 27%" align="center">
                                    </td>
                                    <td colspan="3" style="text-align: center;" align="center">
                                        <asp:Button ID="btnCancel" runat="server" CssClass="ButtonCssNew" BorderWidth="0px"
                                            BorderStyle="None" Text="Cancel" CausesValidation="False"></asp:Button>
                                        <asp:Button ID="btnReject" CssClass="ButtonCssNew" Visible="False" runat="server"
                                            BorderStyle="None" Text="Reject" CausesValidation="False" ToolTip="Reject"></asp:Button>
                                        <% if (TypeUser != 1)
                                           { %>
                                        <asp:Button ID="btndelete" CssClass="ButtonCssNew" BorderStyle="None" ToolTip="Delete"
                                            Text="Delete/Archive" runat="server" CausesValidation="False"></asp:Button>
                                        <% } %>
                                        <% if (TypeUser != 1)
                                           { %>
                                        <asp:Button ID="btnApprove" CssClass="ButtonCssNew" BorderStyle="None" ToolTip="Approve"
                                            Text="Approve" runat="server"></asp:Button>
                                        <% } %>
                                        <% if (TypeUser == 1 && (Convert.ToInt32(ViewState["StatusID"]) == 20 || Convert.ToInt32(ViewState["StatusID"]) == 21))
                                           { %>
                                        <asp:Button ID="btnSubmit" runat="server" CssClass="ButtonCssNew" BorderWidth="0px"
                                            BorderStyle="None" Text="Approve" CausesValidation="False"></asp:Button>
                                        <% } %>
                                        <% if (TypeUser >= 2 && Convert.ToInt32(ViewState["StatusID"]) == 20)
                                           { %>
                                        <asp:Button ID="btnOpen" CssClass="ButtonCssNew" BorderStyle="None" ToolTip="Open"
                                            Text="Open" runat="server" CausesValidation="False"></asp:Button>
                                        <% } %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 322px" align="right">
                                    </td>
                                    <td style="width: 322px" align="right" colspan="2">
                                        <asp:Label ID="lblErrorMsg" runat="server" Width="322px" Font-Bold="True" ForeColor="Red"
                                            Font-Size="smaller"></asp:Label>
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 322px" align="center">
                                    </td>
                                    <td style="width: 322px" align="center">
                                    </td>
                                    <td style="width: 150px" align="left">
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 322px" align="center">
                                    </td>
                                    <td style="width: 322px" align="center">
                                    </td>
                                    <td align="left">
                                    </td>
                                    <td style="width: 138px" align="center">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 30px;">
                            <asp:DataGrid ID="grdFile" runat="server" CssClass="listingArea" CellPadding="0"
                                GridLines="Vertical" AutoGenerateColumns="False" CellSpacing="0">
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
                                            <asp:Label runat="server" ID="lblInvID" Text='<%# DataBinder.Eval(Container, "DataItem.creditnoteID") %>'>
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
                                                <asp:Label ID="lblPath" runat="server"></asp:Label></asp:Label></asp:LinkButton>
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
                                <asp:Label ID="lblMsg" runat="server" CssClass="ErrMsg">No Files Found</asp:Label>
                            </p>
                        </td>
                    </tr>
                    <tr valign="bottom">
                        <td align="center" width="20%">
                            <a onclick="javascript:window.close();" href="#"></a>&nbsp;
                        </td>
                    </tr>
                </table>
                <!-- Main Content Panel Ends-->
            </td>
        </tr>
    </table>
    </form>
    </TD></TR></TBODY></TABLE>
</body>
</html>
