<%@ Page Title="" Language="C#" MasterPageFile="~/Super.Master" AutoEventWireup="true" CodeBehind="Super_Purchase_History_3_1.aspx.cs" Inherits="FCFDFE.pages.Sup.Super_Purchase_History_3_1" %>

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
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">購案案號(1~3)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">購辦號(5)</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_PURCH_5" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;" >
                                        <asp:Label CssClass="control-label " runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_PUR_IPURCH1301" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">開標日期</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">開標次數</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblONB_TIMES" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;" >
                                        <asp:Label CssClass="control-label " runat="server">開標地點</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_PLACE" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3" style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">開標方式</asp:Label>
                                    </td>
                                    <td rowspan="3">
                                        <asp:Label ID="lblOVC_OPEN_METHOD" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td rowspan="3" style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">標次類別</asp:Label>
                                    </td>
                                    <td rowspan="3">
                                        <asp:Label ID="lbl_OVC_BID_METHOD" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_OVC_METHOD_1" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>

                                    <td style="text-align: center; vertical-align: middle;" rowspan="3" >
                                        <asp:Label CssClass="control-label " runat="server">投標廠商家數</asp:Label>
                                    </td>
                                    <td rowspan="3" colspan="2">
                                        <asp:Label ID="lblONB_BID_VENDORS" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_OVC_METHOD_2" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_OVC_METHOD_3" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">審查結果</asp:Label>
                                    </td>
                                    <td rowspan="2">
                                        <asp:Label ID="lblONB_RESULT_OK_NOTOK" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">開標結果</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_RESULT" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;" rowspan="2" >
                                        <asp:Label CssClass="control-label " runat="server">金額</asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label  CssClass="control-label" runat="server">核定底價</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_BID_BUDGET" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">開標結果說明</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_RESULT_REASON" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">決標</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_BID_RESULT" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">開標後審查結果</asp:Label>
                                    </td>
                                    <td colspan="7">
                                        <asp:Label ID="lblONB_OK_NOTOK" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr><!--欄位尚未確定-->
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">開標主持人</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CHAIRMAN" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; ">
                                        <asp:Label CssClass="control-label " runat="server">開標承辦人</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_NAME" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;" >
                                        <asp:Label CssClass="control-label " runat="server">主官核批日</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_DAPPROVE" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnBack" OnClick="btnBack_Click" CssClass="btn-warning btnw2" runat="server" Text="返回" /><br />
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
