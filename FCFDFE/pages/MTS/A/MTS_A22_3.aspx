<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A22_3.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A22_3" %>

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
                    <!--標題-->
                    <div>外運資料表-審核</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton CssClass="btn-success btnw8" OnClick="btnBack_Click" Text="回外運資料表管理" runat="server"></asp:LinkButton>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">外運資料表</asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lblOVC_EDF_NO" CssClass="control-label" runat="server"></asp:Label>&emsp;
                                        <asp:Label ID="lbREVIEW" CssClass="text-red control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lbOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">啟運港(機場)</asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lbOVC_START_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">目的港(機場)</asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lbOVC_ARRIVE_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">發貨單位</asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lbOVC_DEPT_CDE" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtOVC_SHIP_FROM" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="13" class="td-vertical"><!--列數-->
                                        <div class="div-vertical">
                                            <asp:Label CssClass="control-label text-vertical-m" Style="height: 190px;" runat="server">收貨單位</asp:Label><!--高度-->
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">CONSIGNEE</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lbOVC_CON_ENG_ADDRESS" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lbOVC_CON_TEL" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">傳真</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lbOVC_CON_FAX" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">NOTIFY PARTY</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lbOVC_NP_ENG_ADDRESS" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lbOVC_NP_TEL" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">傳真</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lbOVC_NP_FAX" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">ALSO NOTIFY<br>PARTY1</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lbOVC_ANP_ENG_ADDRESS" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lbOVC_ANP_TEL" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">傳真</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lbOVC_ANP_FAX" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">ALSO NOTIFY<br>PARTY2</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lbOVC_ANP_ENG_ADDRESS2" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lbOVC_ANP_TEL2" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">傳真</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lbOVC_ANP_FAX2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">付款方式</asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lbOVC_PAYMENT_TYPE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lbOVC_NOTE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">發貨人資訊</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">名字</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_DELIVER_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">手機</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_DELIVER_MOBILE" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍線</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_DELIVER_MILITARY_LINE" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="6" class="text-left">
                                        <asp:CheckBox ID="chkOVC_IS_STRATEGY" CssClass="radioButton" Text="戰略性高科技貨品" runat="server" />
                                        <asp:CheckBox ID="chkOVC_IS_RISK" CssClass="radioButton untrigger text-red" Text="危險品" runat="server" />
                                        <asp:CheckBox ID="chkOVC_IS_ALERTNESS" CssClass="radioButton untrigger text-red" Text="機敏性" runat="server" /><br>
                                        <asp:Panel ID="pnStrategy" runat="server">
                                            <asp:Label CssClass="control-label" Style="padding-right: 8px;" runat="server">有效期限</asp:Label>
                                            <asp:Label ID="lbODT_VALIDITY_DATE" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Style="padding-left: 16px; padding-right: 8px;" runat="server">輸出許可證號碼</asp:Label>
                                            <asp:Label ID="lbOVC_LICENSE_NO" CssClass="control-label" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-left td-inner-table" colspan="6">
                                        <asp:GridView ID="GVTBGMT_EDF_DETAIL" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" 
                                            OnPreRender="GVTBGMT_EDF_DETAIL_PreRender"
                                             OnRowDataBound="GVTBGMT_EDF_DETAIL_RowDataBound" OnRowCreated="GVTBGMT_EDF_DETAIL_RowCreated" runat="server">
                                            <Columns>
                                                <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" />
                                                <asp:BoundField HeaderText="英文品名" DataField="OVC_ENG_NAME" />
                                                <asp:BoundField HeaderText="中文品名" DataField="OVC_CHI_NAME" />
                                                <asp:BoundField HeaderText="料號" DataField="OVC_ITEM_NO" />
                                                <asp:BoundField HeaderText="單號" DataField="OVC_ITEM_NO2" />
                                                <asp:BoundField HeaderText="件號" DataField="OVC_ITEM_NO3" />
                                                <asp:BoundField HeaderText="數量" DataField="ONB_ITEM_COUNT" />
                                                <asp:BoundField HeaderText="單位" DataField="OVC_ITEM_COUNT_UNIT" />
                                                <asp:BoundField HeaderText="重量" DataField="ONB_WEIGHT" />
                                                <asp:BoundField HeaderText="單位" DataField="OVC_WEIGHT_UNIT" />
                                                <asp:BoundField HeaderText="容積" DataField="ONB_VOLUME" />
                                                <asp:BoundField HeaderText="單位" DataField="OVC_VOLUME_UNIT" />
                                                <%--<asp:BoundField HeaderText="體積(長X寬X高)" DataField="ONB_LENGTH" />--%>
                                                <asp:TemplateField HeaderText="體積(長X寬X高)">
                                                    <ItemTemplate>
                                                        <%#Eval("ONB_LENGTH") +" x "+ Eval("ONB_WIDTH") + " x " + Eval("ONB_HEIGHT")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="金額" DataField="ONB_MONEY" />
                                                <asp:BoundField HeaderText="幣別" DataField="OVC_CURRENCY_NAME" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button ID="btnPass" CssClass="btn-success btnw4" OnClick="btnPass_Click" Text="審核通過" runat="server" />&emsp;
                    <asp:Button ID="btnFail" CssClass="btn-danger btnw4" OnClick="btnFail_Click" Text="審核剔退" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>

