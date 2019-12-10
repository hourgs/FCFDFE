<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B12_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B12_1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->複製購案主畫面
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">原購案編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnQuery" cssclass="btn-success btnw2" runat="server" OnClick="btnQuery_Click" Text="查詢" />
                                        <asp:Button ID="btnReset" cssclass="btn-success btnw2" runat="server" OnClick="btnReset_Click" Text="清除" />
                                        <asp:Button ID="btnReQuery" cssclass="btn-success btnw4" runat="server" OnClick="btnReQuery_Click" Text="重新尋找" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">採購單位地區及方式</asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_PUR_AGENCY" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">申購單位(代碼)-申購人(電話)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblBrackets_1" CssClass="control-label" runat="server" Text="("></asp:Label>
                                        <asp:Label ID="lblOVC_PUR_SECTION" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblBrackets_2" CssClass="control-label" runat="server" Text=")"></asp:Label>
                                        <asp:Label ID="lblConnect" CssClass="control-label" runat="server" Text="-"></asp:Label>
                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblBrackets_3" CssClass="control-label" runat="server" Text="("></asp:Label>
                                        <asp:Label ID="lblOVC_PUR_IUSER_PHONE_EXT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblBrackets_4" CssClass="control-label" runat="server" Text=")"></asp:Label><%--1301PLAN  OVC_PUR_IUSER_PHONE_EXT--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">複製之新購案編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtNewOVC_PURCH" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnConfirm" cssclass="btn-success btnw4" runat="server" OnClick="btnConfirm_Click" Text="確認複製" />
                                        <asp:Label ID="lblDescription" CssClass="control-label" runat="server">(購案編號第一組至第三組合計七碼)</asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:HyperLink ID="lnkNumDesc" runat="server" NavigateUrl="~/pages/MPMS/B/1.htm">軍事機關採購作業規定附表一之(一) 軍事機關財務勞務採購購案編號說明表</asp:HyperLink><br>
                            <asp:HyperLink ID="lnkCodeDesc" runat="server" NavigateUrl="~/pages/MPMS/B/2.htm">軍事機關採購作業規定附表一之(二) 軍事機關個申購單位購案編號第一組代字對照表</asp:HyperLink>
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
