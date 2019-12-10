<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_C12_3.aspx.cs" Inherits="FCFDFE.pages.MTS.C.MTS_C12_3" %>
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
                    國防部國防採購室外購案軍品索賠通知書管理-刪除
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnMessageDelete" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" style="width: 15%;">
                                         <asp:Label CssClass="control-label" runat="server">通知書編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CLAIM_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">申請單位</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_DEPT_CDE" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_ONNAME" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種索賠日期</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblODT_CLAIM_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種索賠字號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CLAIM_MSG_NO" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保單號碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_INN_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠軍品名稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CLAIM_ITEM" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠軍品數量</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_CLAIM_NUMBER" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠軍品總額</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_CLAIM_AMOUNT" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠原因</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CLAIM_REASON" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_NOTE" CssClass="control-label" runat="server" Text="dnoteLabel"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-danger btnw2" OnClientClick="return confirm('確定刪除此資料?');" OnClick="btnDel_Click" Text="刪除" runat="server" /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button CssClass="btn-warning" OnClick="btnBack_Click" Text="回結報申請表管理" runat="server" /><br /><br />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
