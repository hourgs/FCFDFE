<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B12_3.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B12_3" %>
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
                    <div>投保通知書-刪除</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">投保通知書編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_IINN_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td class="text-left">  
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">文號</asp:Label>
                                    </td>
                                    <td class="text-left"> 
                                        <asp:Label ID="lblISSU_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">物資名稱及數量</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_CHI_NAME" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">/</asp:Label>
                                        <asp:Label ID="lblONB_QUANITY" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">件</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">物資價值</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_ITEM_VALUE" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblONB_CARRIAGE_CURRENCY" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">投保金額</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_INS_AMOUNT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblONB_CARRIAGE_CURRENCY2" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">投保日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_INS_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">交貨和保險條件</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_DELIVERY_CONDITION" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_INS_CONDITION" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍售或商購</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCHASE_TYPE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">保險費率</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_INS_RATE" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">保單號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblPOLICY_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">承運商</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_SHIP_COMPANY" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">啟運港口</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_START_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">啟運時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_START_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">目的港口</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ARRIVE_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">進口時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_IMPORT_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">保費支付方法</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" style="display: block;" runat="server">一、保費向本室收取</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">二、保費逕向</asp:Label>
                                        <asp:Label ID="lblOVC_PAYMENT_TYPE" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">收取</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_MILITARY_TYPE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">保費</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_FINAL_INS_AMOUNT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">(台幣)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="text-left">
                                        <asp:Label CssClass="control-label" style="display: block;" runat="server">備考：</asp:Label>
                                        <asp:Label ID="lblOVC_NOTE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button CssClass="btn-danger btnw8" Text="刪除投保通知書" OnClick="btnDel_Click" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server"/>
                    <asp:Button CssClass="btn-success btnw8" Text="回首頁" OnClick="btnHome_Click" runat="server"/>
                </footer>
            </section>
        </div>
    </div>
</asp:Content>

