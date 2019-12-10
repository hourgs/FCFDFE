<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E32.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E32" %>

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
                <header class="title">
                    <!--標題-->
                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                    履驗交貨明細
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                                    <asp:GridView ID="GV_FREE_TAX_DETAIL" CssClass="table data-table table-striped border-top text-center" OnRowDataBound="GV_FREE_TAX_DETAIL_RowDataBound" AutoGenerateColumns="false" runat="server">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="動作">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnDel" CssClass="btn-default btnw4" Text="刪除本項" OnClick="btnDel_Click" CommandName="DataSelect" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="合約項次" DataField="ONB_ICOUNT" />
                                                    <asp:BoundField HeaderText="中文名稱/英文名稱" DataField="" />
                                                    <asp:BoundField HeaderText="交貨數量" DataField="ONB_QDELIVERY" />
                                                    <asp:BoundField HeaderText="交貨單價" DataField="ONB_MDELIVERY" />
                                                    <asp:BoundField HeaderText="單價總價" DataField="" />
                                                </Columns>
                                            </asp:GridView>

                                    <asp:GridView ID="GV_CHOOSE_FREE_TAX_DETAIL" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_CHOOSE_FREE_TAX_DETAIL_RowCreated" OnRowDataBound="GV_CHOOSE_FREE_TAX_DETAIL_RowDataBound" runat="server" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="動作">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnChoose" CssClass="btn-default btnw4" OnClick="btnChoose_Click" Text="選擇本項" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="合約項次" DataField="ONB_POI_ICOUNT" />
                                            <asp:BoundField HeaderText="中文名稱/英文名稱" DataField="OVC_POI_NSTUFF_CHN" />
                                            <asp:TemplateField HeaderText="交貨數量">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCount"  Text='<%# Bind("ONB_POI_QORDER_CONT") %>' CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="交貨單價">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPrice"  Text='<%# Bind("ONB_POI_MPRICE_CONT") %>' CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="單價總價" DataField="" />
                                        </Columns>
                                    </asp:GridView>
                            <table class="table table-bordered text-center" visible="false" runat="server">
                                <tr>
                                    <th colspan="6" style="background: #ffff37;">
                                        <asp:Label ForeColor="red" Font-Size="X-Large" CssClass="control-label" runat="server"><span>AA05003</span>履驗交貨明細</asp:Label></th>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">動作</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">合約項次</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">中文名稱/英文名稱</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">交貨數量</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">交貨單價</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">單價總價</asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="btnDo" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblOVC_PRUCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label id="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label id="lblOVC_PUR_IPURCH_ENG" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td><asp:TextBox ID="txtONB_QDELIVERY" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                    <td><asp:TextBox ID="txtONB_MDELIVERY" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                    <td><asp:Label ID="lblsum" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                            </table>

                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                            </div>
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
