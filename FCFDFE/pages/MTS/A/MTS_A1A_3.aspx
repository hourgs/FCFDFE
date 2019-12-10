<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A1A_3.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A1A_3" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>進口軍品運輸交接單-刪除</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnModify" runat="server"></asp:Panel>
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton OnClick="btnBack_Click" Text="回交接單管理" runat="server"></asp:LinkButton>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">交接單編號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left" style="width:85%">
                                        <asp:Label ID="lblOVC_IHO_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">清運方法</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblbOVC_TRANS_TYPE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">起運地點</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:35%">
                                        <asp:Label ID="lblOVC_START_PLACE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">運達地點</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:35%">
                                        <asp:Label ID="lblOVC_ARRIVE_PLACE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">起運時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_START_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">抵運時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_ARRIVE_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接收單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_RECEIVE_DEPT_CDE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_TRANSER_DEPT_CDE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總件數</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_TOTAL_QUANITY" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_QUANITY_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總體積</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_TOTAL_VOLUME" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_VOLUME_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總重量</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_TOTAL_WEIGHT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_WEIGHT_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_SHIP_NAME" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機航次</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_VOYAGE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                	<td>
                                        <asp:Label CssClass="control-label" runat="server">點收情形</asp:Label>
                                    </td>
                                    <td colspan="3" class="td-inner-table">
                                        <table class="table table-bordered text-center table-inner">
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">超出</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">短少</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">破損</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">實收</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblONB_OVERFLOW" CssClass="control-label" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblONB_LESS" CssClass="control-label" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblONB_BROKEN" CssClass="control-label" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblONB_ACTUAL_RECEIVE" CssClass="control-label" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_NOTE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="td-inner-table text-left">
                                        <asp:GridView ID="GVTBGMT_IRD_DETAIL" CssClass="table table-striped border-top table-inner text-center" AutoGenerateColumns="false" OnPreRender="GVTBGMT_IRD_DETAIL_PreRender" runat="server">
                                            <Columns>
                                                <asp:TemplateField HeaderText="項次" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="提單號碼" DataField="OVC_BLD_NO" />
                                                <asp:BoundField HeaderText="購案號" DataField="OVC_PURCH_NO" />
                                                <asp:BoundField HeaderText="品名" DataField="OVC_CHI_NAME" />
                                                <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" />
                                                <asp:BoundField HeaderText="實收件數" DataField="ONB_ACTUAL_RECEIVE" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-danger" Text="刪除軍品運輸交接單" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" OnClick="btnDel_Click" runat="server"/>
                                <asp:Button CssClass="btn-warning" Text="回首頁" OnClick="btnBack_Click" runat="server"/>
                            </div>
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
