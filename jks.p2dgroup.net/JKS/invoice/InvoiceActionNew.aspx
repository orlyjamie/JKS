<%@ Page Language="c#" CodeBehind="InvoiceActionNew.aspx.cs" AutoEventWireup="false"
    Inherits="CBSolutions.ETH.Web.ETC.invoice.InvoiceActionNew" %>

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
        function CloseWindow() {
            if ('<%=iCurrentStatusID%>' == '4') {
                alert('Sorry, cannot manipulate this invoice, it is completed.')
                window.close();
            }
        }

        function doHourglass() {
            document.body.style.cursor = 'wait';
        }
        function windowclose() {
            window.close();
        }

        function SubmitForm() {
            window.opener.document.forms[0].submit();
        }

        function CaptureClose() {

            document.body.style.cursor = 'wait';

            window.opener.doRefesh();

            opener.location.reload(true);
            alert('ffff');
        }

        function CheckInvoiceDesc() {
            if (document.getElementById("txtDescription").value == '') {
                alert('Please add an invoice description.');
                return false;
            }
            return true;
        }
        function CheckOpenValid() {
            if (document.getElementById("ddldept").selectedIndex == 0) {
                alert('Please select department');
                return false;
            }
            if (document.getElementById("ddlApprover1").selectedIndex == 0 && document.getElementById("ddlApprover2").selectedIndex == 0 && document.getElementById("ddlApprover3").selectedIndex == 0 && document.getElementById("ddlApprover4").selectedIndex == 0 && document.getElementById("ddlApprover5").selectedIndex == 0) {
                alert('Please select at least one Approver');
                return false;
            }
            return true;
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
                                <tr>
                                    <td style="width: 43px">
                                    </td>
                                    <td class="NormalBody" style="width: 129px">
                                    </td>
                                    <td style="width: 250px">
                                    </td>
                                    <td class="NormalBody" style="width: 138px">
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
                                        <b>Approval Path</b>
                                    </td>
                                    <td>
                                        <span onmouseover="Tip('<%=AuthorisationStringToolTips%>', FADEIN, 1000, PADDING, 10);">
                                            <asp:Label ID="lblApprovalPath" Style="cursor: hand" runat="server" CssClass="NormalBody"></asp:Label></span>
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
                                    <td class="NormalBody" style="width: 138px">
                                        <b>Department</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDepartment" runat="server" CssClass="NormalBody"></asp:Label>
                                    </td>
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
                                        <asp:Label ID="lblcreditnoteno" runat="server" Visible="False" CssClass="NormalBody"
                                            ForeColor="Red"></asp:Label><asp:LinkButton ID="lnkCrn" runat="server" Visible="False"
                                                CssClass="NormalBody" ForeColor="Red"></asp:LinkButton>
                                        <%=GetCreditLinks()%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="30">
                        <td style="height: 2px" align="center">
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
                            <asp:DataGrid ID="grdList" runat="server" Width="896px" ShowFooter="True" Font-Names="Verdana,Tahoma,Arial"
                                AutoGenerateColumns="False" GridLines="Vertical" CellPadding="2" BackColor="White"
                                BorderColor="#999999" BorderWidth="1px" BorderStyle="None">
                                <FooterStyle ForeColor="Black" BackColor="White"></FooterStyle>
                                <AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
                                <ItemStyle Font-Size="X-Small" Font-Names="Verdana" ForeColor="Black" BackColor="White">
                                </ItemStyle>
                                <HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="White"
                                    BackColor="#3399CC"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn HeaderText="Line No">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Font-Bold="True"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Company Name">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlBuyerCompanyCode" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged_ddlBuyerCompanyCode"
                                                runat="server" Font-Size="XX-Small">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Department">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDepartment1" OnSelectedIndexChanged="SelectedIndexChanged_ddlDepartment"
                                                AutoPostBack="True" Font-Size="XX-Small" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Nominal Code">
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged_ddlNominalCode"
                                                Font-Size="XX-Small" ID="ddlNominalCode1">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Code Description">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCodingDescription1" OnSelectedIndexChanged="SelectedIndexChanged_ddlCodingDescription"
                                                AutoPostBack="True" Font-Size="XX-Small" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Project Code">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlProject1" Font-Size="XX-Small" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr>
                                                    <td nowrap>
                                                        <asp:Label ID="Label1" runat="server" CssClass="NormalBody">	Total Net Value for Coding:</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>
                                                        <br>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>
                                                        <asp:Label ID="Label2" runat="server" CssClass="NormalBody">Net Invoice Total:</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Net value for coding">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNetVal" Text='<%# DataBinder.Eval(Container, "DataItem.NetValue") %>'
                                                runat="server">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr>
                                                    <td nowrap>
                                                        <asp:Label ID="lblNetVal" runat="server" CssClass="NormalBody"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>
                                                        <br>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>
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
                                <tr>
                                    <td style="width: 206px" align="right">
                                    </td>
                                    <td style="width: 150px" align="center">
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 206px" align="right">
                                    </td>
                                    <td style="width: 150px" align="left">
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                            </table>
                            <table class="tlbborder" style="width: 904px; height: 253px">
                                <% if (TypeUser > 1)
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
                                    <td style="width: 206px; height: 14px" align="right">
                                    </td>
                                    <td style="width: 526px; height: 14px" align="left">
                                    </td>
                                    <td style="width: 138px; height: 14px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 206px; height: 25px" align="right">
                                        <b class="NormalBody">Approval Path&nbsp; :</b>
                                    </td>
                                    <td style="width: 526px; height: 25px" align="left">
                                        <asp:DropDownList ID="ddlApprover1" TabIndex="4" runat="server" CssClass="MyInput"
                                            Width="96px" DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlApprover2" TabIndex="4" runat="server" CssClass="MyInput"
                                            Width="96px" DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlApprover3" TabIndex="4" runat="server" CssClass="MyInput"
                                            Width="96px" DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlApprover4" TabIndex="4" runat="server" CssClass="MyInput"
                                            Width="96px" DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlApprover5" TabIndex="4" runat="server" CssClass="MyInput"
                                            Width="96px" DataValueField="GroupName" DataTextField="GroupName">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 138px; height: 25px">
                                    </td>
                                </tr>
                                <% }%>
                                <tr>
                                    <td style="width: 206px; height: 12px" align="right">
                                    </td>
                                    <td style="width: 526px; height: 12px" align="left">
                                    </td>
                                    <td style="width: 138px; height: 12px">
                                    </td>
                                </tr>
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
                                <tr>
                                    <td style="width: 206px; height: 10px" align="right">
                                    </td>
                                    <td style="width: 526px; height: 10px" align="left">
                                    </td>
                                    <td style="width: 138px; height: 10px">
                                    </td>
                                </tr>
                                <%
                                    if (RejectOpenFields == 1)
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
                                <tr>
                                    <td style="width: 206px" align="right">
                                    </td>
                                    <td style="width: 526px" align="left">
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 206px" align="right">
                                        <b class="NormalBody"><b class="NormalBody">Invoice Description</b>&nbsp; : <b class="NormalBody">
                                            <font color="#ff3300">*</font></b></b>
                                    </td>
                                    <td style="width: 526px" align="left">
                                        <asp:TextBox ID="txtDescription" runat="server" Width="432px" Height="38px" TextMode="MultiLine"
                                            MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 206px" align="right">
                                    </td>
                                    <td style="width: 526px" align="left">
                                        <span class="NormalBody" style="color: #ff0000"><b>* = Mandatory Field</b></span>
                                    </td>
                                    <td style="width: 138px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnCancel" runat="server" CssClass="ButtonCssNew" BorderWidth="0px"
                                            BorderStyle="None" CausesValidation="False" Text="Cancel"></asp:Button>
                                    </td>
                                    <td style="width: 526px" align="center">
                                        <asp:Button ID="btnReject" CssClass="ButtonCssNew" BorderStyle="None" CausesValidation="False"
                                            Text="Reject" runat="server" ToolTip="Reject"></asp:Button><asp:Button ID="btnReopen"
                                                CssClass="ButtonCssNew" BorderStyle="None" CausesValidation="False" Text="Reopen"
                                                runat="server" ToolTip="Reopen"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnApprove" CssClass="ButtonCssNew" BorderStyle="None" CausesValidation="False"
                                            Text="Approve" runat="server" ToolTip="Approve"></asp:Button><asp:Button ID="btnOpen"
                                                CssClass="ButtonCssNew" BorderStyle="None" CausesValidation="False" Text="Open"
                                                runat="server" ToolTip="Open"></asp:Button>
                                    </td>
                                    <td style="width: 138px" align="left">
                                        <asp:Button ID="btndelete" CssClass="ButtonCssNew" BorderStyle="None" CausesValidation="False"
                                            Text="Delete" runat="server" ToolTip="Delete"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 206px" align="right">
                                    </td>
                                    <td style="width: 526px" align="center">
                                        <asp:Label ID="lblErrorMsg" runat="server" Visible="False" Font-Bold="True" Width="314px"
                                            ForeColor="Red" Font-Size="8"></asp:Label>
                                    </td>
                                    <td style="width: 138px" align="left">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="bottom">
                        <td align="center" width="20%">
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
