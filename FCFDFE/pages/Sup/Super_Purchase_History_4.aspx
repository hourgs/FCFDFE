<%@ Page Title="" Language="C#" MasterPageFile="~/Super.Master" AutoEventWireup="true" CodeBehind="Super_Purchase_History_4.aspx.cs" Inherits="FCFDFE.pages.Sup.Super_Purchase_History_4" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    採購品項基本資訊
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">購案案號(1~3)</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_PUR_IPURCH1301" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">採購項次</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblONB_POI_ICOUNT" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">財務分類編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_FCODE" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">中文品名</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblOVC_POI_NSTUFF_CHN" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">英文品名</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_POI_NSTUFF_ENG" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">料號種類</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblNSN_KIND" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">料號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNSN" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">廠牌</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblOVC_BRAND" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">型號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_MODEL" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">件號</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblOVC_POI_IREF" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">同等品</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_SAME_QUALITY" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; width: 14%">
                                        <asp:Label CssClass="control-label " runat="server">單位</asp:Label>
                                    </td>
                                    <td style="width: 14%">
                                        <asp:Label ID="lblOVC_POI_IUNIT" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width: 14%">
                                        <asp:Label CssClass="control-label " runat="server">初次採購</asp:Label>
                                    </td>
                                    <td style="width: 14%">
                                        <asp:Label ID="lblOVC_FIRST_BUY" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width: 14%">
                                        <asp:Label CssClass="control-label " runat="server">以往購案號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_POI_IPURCH_BEF" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">申購數量</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblONB_POI_QORDER_PLAN" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">合約數量</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_POI_QORDER_CONT" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">預算單價</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblONB_POI_MPRICE_PLAN" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">合約單價</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_POI_MPRICE_CONT" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">規格</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblOVC_POI_NDESC" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnBack" OnClick="btnBack_Click" CssClass="btn-warning btnw2"  runat="server" Text="返回" /><br />
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
