<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SupplierMapping.aspx.cs" Inherits="NewLook.NewLook_ScanQC_SupplierMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Supplier Mapping</title>
    <link href="../custom_css/ScanQC.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.0.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="supplier-mapping">
        <div class="floater">
            <div class="topper">Supplier Mapping</div>
            <div class="center">
                <div>
                    <span>Buyer</span>
                    <asp:Label ID="lblBuyer" runat="server"></asp:Label>
                </div>
                <div>
                    <span>Supplier</span>
                    <asp:Label ID="lblSupplier" runat="server"></asp:Label>
                </div>
                <div>
                    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false" 
                        GridLines="None" CellSpacing="0" CellPadding="1" DataKeyNames="MAPPING ID">
                        <Columns>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LHS">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtLHS" runat="server" Text='<%#Eval("LHS")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RHS">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRHS" runat="server" Text='<%#Eval("RHS")%>' ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="bottom">
                <div class="blocks">
                    <asp:Button ID="btnDeleteLines" runat="server" Text="Delete Lines" CssClass="red-button" UseSubmitBehavior="false" />
                </div>
                <div class="blocks">
                    <asp:Button ID="btnAddLine" runat="server" Text="Add Line" CssClass="blue-button" UseSubmitBehavior="false" />
                </div>
                <div class="blocks">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="dark-blue-button" />
                </div>
                <div class="blocks">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="sky-button" UseSubmitBehavior="false" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
