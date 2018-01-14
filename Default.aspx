<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" StylesheetTheme="SkinFile" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" Runat="Server">
    <h1>CISF Phone Audit</h1>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="BodyContent" >
        <p>The purpose of this audit is to confirm what telephone extensions are currently in use in buildings 2025, 1840 and 1470.&nbsp; If you have a desk phone, facimilie machine, polycom, or other business phone line or extension located within these buildings, please confirm these telephone numbers.&nbsp; The scope of this audit does not include cell phones or personal telephones. If you are a manager and have personnel on TDY, please enter phone numbers on their behalf.&nbsp; Unconfirmed extensions will be disconnected on 1 July, 2013</p>
        <p>
            <asp:PlaceHolder ID="phForm" runat="server"></asp:PlaceHolder>            

                <asp:DataList ID="dlMyPhones" runat="server" CellPadding="4" ForeColor="#333333" Visible="true" OnDeleteCommand="Delete_Click">
                    <HeaderTemplate>
                        <table border="0" cellpadding="6" cellspacing="0">
                            <tr>
                                <td>Description
                                </td>
                                <td>Phone
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Add" runat="server" CssClass="buttonText" Text="  Add  " OnClick="Add_Click"  />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblError" runat="server" CssClass="error" Text=""></asp:Label>
                                </td>
                            </tr>
                    </HeaderTemplate>            
                    <ItemTemplate>
                        <tr class="row">
                            <td class="row" valign="top">
                                <asp:Label ID="lblIdent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "_Ident") %>' Visible="false"></asp:Label>
                                <%# Eval("_Description") %>
                            </td>
                            <td class="row" valign="top">
                                <%# Eval("_Phone") %>
                            </td>
                            <td class="row" valign="top">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Delete" CssClass="buttonText" runat="server" Text="Delete" CommandName="Delete" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternateRow">
                            <td class="alternateRow" valign="top">
                                <asp:Label ID="lblIdent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "_Ident") %>' Visible="false"></asp:Label>
                                <%# Eval("_Description") %>
                            </td>
                            <td class="alternateRow" valign="top">
                                <%# Eval("_Phone") %>
                            </td>
                            <td class="alternateRow" valign="top">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Delete" CssClass="buttonText" runat="server" Text="Delete" CommandName="Delete"/>
                            </td>
                            
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:DataList>
        </p>
        <p align="center">
            <input type="button" value="Done" onclick="javascript:window.close();">
        </p>
    </div>
</asp:Content>

