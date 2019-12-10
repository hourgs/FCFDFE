<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Phrases.aspx.cs" Inherits="FCFDFE.pages.GM.Phrases" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("#testTable").hide();
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>--%>
                    <section class="panel">
                        <header class="title">
                            <!--標題-->
                            <asp:Label CssClass="control-label" runat="server">國軍採購管理資訊系統<br>作業階段片語維護</asp:Label>
                        </header>
                        <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <!--預留空間，未來做錯誤訊息顯示。-->
                        <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                                <div class="cmxform form-horizontal tasi-form">
                                    <!--網頁內容-->
                                    <table id="testTable" class="table" style="width: 70%;">
                                        <tr>
                                            <td class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">請選擇片語類別:</asp:Label>
                                            </td>
                                            <td class="text-left" style="width: 50%">
                                                <asp:DropDownList ID="drpMainItem" CssClass="tb tb-l" runat="server">
                                                    <asp:ListItem></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnback" CssClass="btn-success btnw4" Text="回上一頁" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table" style="width: 50%">
                                        <tr>
                                            <td class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">請輸入片語內容:</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:TextBox ID="txtOVC_PHR_DESC_Query" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPartQuery" CssClass="btn-success btnw4" Text="部分查詢" OnClick="btnPartQuery_Click" runat="server" />
                                            </td>
                                        </tr>
                                    </table>

                                    <asp:Panel ID="PnMessage_Save" runat="server"></asp:Panel>
                                    <!--預留空間，未來做錯誤訊息顯示。-->

                                    <asp:GridView ID="GridView_SubItem" DataKeyNames="OVC_PHR_ID" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowCommand="GridView_SubItem_RowCommand" OnPreRender="GridView_SubItem_PreRender" runat="server">
                                        <Columns>
                                            <asp:BoundField HeaderText="片語類別" DataField="OVC_PHR_CATE" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="片語代碼" DataField="OVC_PHR_ID" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="片語內容" DataField="OVC_PHR_DESC" HeaderStyle-Width="30%" />
                                            <asp:BoundField HeaderText="片語說明" DataField="OVC_PHR_REMARK" HeaderStyle-Width="25%" />
                                            <asp:BoundField HeaderText="負責單位" DataField="OVC_USR_ID" HeaderStyle-Width="10%" />
                                            <asp:TemplateField HeaderText="作業" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnModify" CssClass="btn-success btnw2" Text="異動" CommandName="DataModify" runat="server" />
                                                    <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="DataDel" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                    <table class="table table-bordered text-center">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtOVC_PHR_CATE" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOVC_PHR_ID" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOVC_PHR_DESC" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOVC_PHR_REMARK" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOVC_USR_ID" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSave" CssClass="btn-warning btnw2" OnClick="btnSave_Click" Text="儲存" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <footer class="panel-footer text-center">
                            <!--網頁尾-->
                        </footer>
                    </section>
                <%--</ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnback" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnPartQuery" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>
