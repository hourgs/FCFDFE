<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E24.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E24" %>

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
                <header class="title text-red">
                    <!--標題-->
                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                    履驗申請免稅明細
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="GV_FREE_TAX_DETAIL" CssClass="table data-table table-striped border-top text-center" OnRowDataBound="GV_FREE_TAX_DETAIL_RowDataBound" AutoGenerateColumns="false" runat="server">
                                        <Columns>
                                            <asp:TemplateField HeaderText="動作">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDel" CssClass="btn-default btnw4" Text="刪除本項" CommandName="DataSelect" OnClick="btnDel_Click" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="合約項次" DataField="ONB_ICOUNT" />
                                            <asp:BoundField HeaderText="中英文名稱//型號" DataField="" />
                                            <asp:BoundField HeaderText="數量" DataField="ONB_QDELIVERY" />
                                            <asp:BoundField HeaderText="單價" DataField="ONB_MDELIVERY" />
                                            <asp:BoundField HeaderText="國內接收地點" DataField="OVC_SHIP_PLACE" />
                                        </Columns>
                                    </asp:GridView>
                                    <div class="text-center">
                                        <asp:Button ID="btnRE" CssClass="btn-default " OnClick="btnRE_Click" runat="server" Text="回編輯畫面" />
                                    </div>
                                    <p></p>
                                    <asp:GridView ID="GV_CHOOSE_FREE_TAX_DETAIL" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_CHOOSE_FREE_TAX_DETAIL_RowCreated" OnRowDataBound="GV_CHOOSE_FREE_TAX_DETAIL_RowDataBound" runat="server" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="動作">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnChoose" CssClass="btn-default btnw4" Text="選擇本項" OnClick="btnChoose_Click" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="合約項次" DataField="ONB_POI_ICOUNT" />
                                            <asp:BoundField HeaderText="中英文名稱//型號" DataField="OVC_POI_NSTUFF_CHN" />
                                            <asp:TemplateField HeaderText="數量">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCount" Text='<%# Bind("ONB_POI_QORDER_CONT") %>' CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="單價">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPrice" Text='<%# Bind("ONB_POI_MPRICE_CONT") %>' CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="國內接收地點">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPlace" Text="台北市漢口街2段85號" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table class="table table-bordered text-center" visible="false" runat="server">
                                <tr>
                                    <th colspan="6" style="background: #ffff37;">
                                        <asp:Label ForeColor="red" Font-Size="Large" CssClass="control-label" runat="server">請選擇合約明細</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">動作</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">合約項次</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">中英文品名//型號</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">數量</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">單價</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">國內接收地點</asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSelect" CssClass="btn-default btnw4" runat="server" Text="選擇本項" /></td>
                                    <td>
                                        <asp:Label ID="lblONB_ICOUNT" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_TAX_STUFF" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_TAX_QUALITY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_UNIT_PRICE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_SHIP_PLACE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
