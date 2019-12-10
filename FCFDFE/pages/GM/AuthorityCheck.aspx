<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuthorityCheck.aspx.cs" Inherits="FCFDFE.pages.GM.AuthorityCheck" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    權限核定
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered" style="width: 700px;">
                                <tr class="no-bordered no-bordered-seesaw">
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">系統別：</asp:Label>
                                        <asp:DropDownList ID="drpC_SN_SYS" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位代碼：</asp:Label>
                                        <asp:TextBox ID="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <input type="button" value="單位查詢" class="btn-success" onclick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">姓名：</asp:Label>
                                        <asp:TextBox ID="txtUSER_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success" OnClick="btnQuery_Click" runat="server" Text="查詢" /><br />
                            </div>
                            <div class="text-center" style="margin-top: 30px;">
                                <asp:Label cssclass="control-label text-red" runat="server">尚未開放使用</asp:Label>
                            </div>
                            <asp:GridView ID="GV_ACCOUNT_AUTH_Un" DataKeyNames="AA_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowCommand="GV_ACCOUNT_AUTH_Un_RowCommand" OnPreRender="GV_ACCOUNT_AUTH_Un_PreRender" OnRowDataBound="GV_ACCOUNT_AUTH_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="DEPT_SN" HeaderText="單位" />
                                    <asp:BoundField DataField="USER_ID" HeaderText="帳號" />
                                    <asp:BoundField DataField="USER_NAME" HeaderText="姓名" />
                                    <asp:BoundField DataField="IUSER_PHONE" HeaderText="電話(軍線)" />
                                    <asp:BoundField DataField="C_SN_SYS" HeaderText="系統別" />
                                    <asp:BoundField DataField="C_SN_ROLE" HeaderText="使用者權限" />
                                    <asp:BoundField DataField="C_SN_AUTH" HeaderText="使用者角色" />
                                    <asp:BoundField DataField="C_SN_SUB" HeaderText="隸屬單位" />
                                    <%--<asp:BoundField DataField="IS_UPLOAD" HeaderText="上傳功能" />--%>
                                    <%--<asp:BoundField DataField="IS_ENABLE" HeaderText="開放使用" />--%>
                                    <asp:TemplateField HeaderText="上傳功能" HeaderStyle-Width="6em">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIS_UPLOAD" CssClass="control-label" Text='<%#( Eval("IS_UPLOAD").ToString() )%>' Visible="false" runat="server"></asp:Label>
                                            <%--<asp:RadioButtonList ID="rdoIS_UPLOAD" CssClass="radioButton" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>--%>
                                            <asp:RadioButtonList ID="rdoIS_UPLOAD" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server"></asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="開放使用" HeaderStyle-Width="7em">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIS_ENABLE" CssClass="control-label" Text='<%#( Eval("IS_ENABLE").ToString() )%>' Visible="false" runat="server"></asp:Label>
                                            <asp:RadioButtonList ID="rdoIS_ENABLE" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server"></asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="作業" HeaderStyle-Width="9em" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button Text="存檔" CssClass="btn-warning" CommandName="DataSave" runat="server" />
                                            <asp:Button Text="刪除" CssClass="btn-danger" CommandName="DataDel" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                                <asp:Label cssclass="control-label text-red" runat="server">已開放使用</asp:Label>
                            </div>
                            <div class="text-center">
                                <asp:Label CssClass="control-label text-red" runat="server">注意!若預期之使用者帳號未出現，請先確認該使用者所屬單位已經設為「採購單位」</asp:Label>
                            </div>
                            <asp:GridView ID="GV_ACCOUNT_AUTH" DataKeyNames="AA_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowCommand="GV_ACCOUNT_AUTH_RowCommand" OnPreRender="GV_ACCOUNT_AUTH_PreRender" OnRowDataBound="GV_ACCOUNT_AUTH_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="DEPT_SN" HeaderText="單位" />
                                    <asp:BoundField DataField="USER_ID" HeaderText="帳號" />
                                    <asp:BoundField DataField="USER_NAME" HeaderText="姓名" />
                                    <asp:BoundField DataField="IUSER_PHONE" HeaderText="電話(軍線)" />
                                    <asp:BoundField DataField="C_SN_SYS" HeaderText="系統別" />
                                    <asp:BoundField DataField="C_SN_ROLE" HeaderText="使用者權限" />
                                    <asp:BoundField DataField="C_SN_AUTH" HeaderText="使用者角色" />
                                    <asp:BoundField DataField="C_SN_SUB" HeaderText="隸屬單位" />
                                    <%--<asp:BoundField DataField="IS_UPLOAD" HeaderText="上傳功能" />--%>
                                    <%--<asp:BoundField DataField="IS_ENABLE" HeaderText="開放使用" />--%>
                                    <asp:TemplateField HeaderText="上傳功能" HeaderStyle-Width="6em">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIS_UPLOAD" CssClass="control-label" Text='<%#( Eval("IS_UPLOAD").ToString() )%>' Visible="false" runat="server"></asp:Label>
                                            <%--<asp:RadioButtonList ID="rdoIS_UPLOAD" CssClass="radioButton" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>--%>
                                            <asp:RadioButtonList ID="rdoIS_UPLOAD" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server"></asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="開放使用" HeaderStyle-Width="7em">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIS_ENABLE" CssClass="control-label" Text='<%#( Eval("IS_ENABLE").ToString() )%>' Visible="false" runat="server"></asp:Label>
                                            <asp:RadioButtonList ID="rdoIS_ENABLE" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server"></asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="作業" HeaderStyle-Width="7em" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSave" Text="存檔" CssClass="btn-warning" CommandName="DataSave" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
