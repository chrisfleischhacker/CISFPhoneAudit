<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PhoneList.aspx.cs" Inherits="_PhoneList" StylesheetTheme="SkinFile" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="Server">
    <h1>CISF Phone Audit</h1>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="BodyContent">
        <p>
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </p>
        <p>

            <asp:DataList ID="dlAllPhones" runat="server" CellPadding="4" ForeColor="#333333" Visible="false" OnDeleteCommand="Delete_Click">
                <HeaderTemplate>
                    <table border="0" cellpadding="6" cellspacing="0">
                        <tr>
                            <td>Phone
                            </td>
                            <td>Description
                            </td>
                            <td>Location
                            </td>
                            <td colspan="2">Name
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="row">
                        <td class="row" valign="top">
                            <asp:Label ID="lblIdent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "_Ident") %>' Visible="false"></asp:Label>

                            <%# Eval("_Phone") %>
                        </td>
                        <td class="row" valign="top">
                            <%# Eval("_Description") %>
                        </td>
                        <td class="row" valign="top">
                            <%# Eval("_Location") %>
                        </td>
                        <td class="row" valign="top">
                            <%# Eval("_Name") %>
                        </td>
                        <td class="row" valign="top">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Delete" CssClass="buttonText" runat="server" Text="Delete" CommandName="Delete" />
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="alternateRow">
                        <td class="alternateRow" valign="top">
                            <asp:Label ID="lblIdent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "_Ident") %>' Visible="false"></asp:Label>

                            <%# Eval("_Phone") %>
                        </td>
                        <td class="alternateRow" valign="top">
                            <%# Eval("_Description") %>
                        </td>
                        <td class="alternateRow" valign="top">
                            <%# Eval("_Location") %>
                        </td>
                        <td class="alternateRow" valign="top">
                            <%# Eval("_Name") %>
                        </td>
                        <td class="alternateRow" valign="top">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Delete" CssClass="buttonText" runat="server" Text="Delete" CommandName="Delete" />
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
        </p>
    </div>
</asp:Content>

