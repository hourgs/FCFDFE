<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A14_3.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A14_3" %>

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
                    <div>作業時程管制簿-刪除</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnDelete" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left" style="width: 80%;">
                                        <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機名</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_SHIP_NAME" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">航次</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_VOYAGE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">品名</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_CHI_NAME" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接收單位</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_RECEIVE_DEPT_CODE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">收到貨通知書日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_RECEIVE_INF_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 20%;">
                                        <asp:Label CssClass="control-label" runat="server">清運方式</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_TRANS_TYPE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">進口日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_IMPORT_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">通關日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_PASS_CUSTOM_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">收國外報關文件日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_ABROAD_CUSTOM_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">拆櫃日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_UNPACKING_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">換小提單日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_CHANGE_BLD_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">清運日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_TRANSFER_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">報關日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_CUSTOM_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接收日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_RECEIVE_DATE" CssClass="control-label" runat="server"></asp:Label>
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
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">機敏軍品</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_IS_SECURITY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button ID="btnDel" CssClass="btn-success btnw6" OnClick="btnDel_Click" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" Text="刪除管制簿" runat="server" />
                    <asp:Button ID="btnHome" CssClass="btn-success btnw6" OnClick="btnHome_Click"  Text="回首頁" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
