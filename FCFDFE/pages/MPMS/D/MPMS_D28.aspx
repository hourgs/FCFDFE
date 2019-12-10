<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D28.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D28" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                    <h1>購案開標通知作業編輯</h1>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <asp:GridView ID="GV_info" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_PURCH" OnRowCommand="GV_info_RowCommand" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="作業">
                                <ItemTemplate>
                                    <asp:Button ID="btnDo" CssClass="btn-danger btnw2" Text="作業" CommandName="按鈕屬性" runat="server" />
                                </ItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="按鈕屬性" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="開標次數" DataField="ONB_TIMES" />
                            <asp:BoundField HeaderText="開標時間" DataField="OVC_DOPEN" />
                            <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                        </Columns>
                    </asp:GridView>
                    <div class="text-center">
                        <asp:Label CssClass="control-label text-red" runat="server">註：開標通知預設會顯示本案已簽辦之開標時間組別0(不分組)之資料，無須分組者請直接『作業』</asp:Label><br />
                        <asp:Label CssClass="control-label text-red" runat="server">註：需分組者，請至下方選擇本案已簽辦之開標日期、輸入組別後，按『新增』！</asp:Label>
                    </div>
                    <br />
                    <div class="title text-center"><asp:Label CssClass="control-label" runat="server">請選擇本案已簽辦之開標時間，並決定其組別 </asp:Label>
</div>
                    <table class="table table-bordered" style="text-align: center">
                        <tr>
                            <th>
                                <asp:Label CssClass="control-label text-red" runat="server">作業</asp:Label>
                            </th>
                            <th>
                                <asp:Label CssClass="control-label" runat="server">開標次數</asp:Label>
                            </th>
                            <th>
                                <asp:Label CssClass="control-label" runat="server">開標時間</asp:Label>
                            </th>
                            <th>
                                <asp:Label CssClass="control-label  " runat="server">組別</asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnAdd" CssClass="btn-warning btnw2" runat="server" Text="新增" />
                            </td>
                            <td>
                                <asp:Label ID="lblONB_TIMES" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtONB_GROUP" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                <asp:Label CssClass="control-label" runat="server">(請輸入組別：0表示不分組。)</asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <br />
                <div class="text-center">
                    <asp:Button ID="btnReturn" CssClass="btn-warning btnw4" runat="server" Text="上一頁" />
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->

                </footer>
            </section>
        </div>
    </div>
</asp:Content>
