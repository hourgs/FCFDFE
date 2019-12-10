<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B17.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B17" %>
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
                    <!--標題-->更改購案案號主畫面
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">購案編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnQuery" cssclass="btn-success btnw2" runat="server" OnClick="btnQuery_Click" Text="查詢" />
                                        <asp:Button ID="btnReset" cssclass="btn-success btnw2" runat="server" OnClick="btnReset_Click" Text="清除" />
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
                                <tr id="drpRow" runat="server" visible="false">
                                    <td><asp:Label CssClass="control-label" runat="server">更改之後新購案編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpTO_PHRID" runat="server"></asp:DropDownList>
                                        <asp:Button ID="btnModify" cssclass="btn-success btnw4" runat="server" OnClick="btnModify_Click" Text="確認更改" />
                                        <br>
                                        <asp:Label CssClass="control-label" runat="server">請確認更改後之購案號，已於採購預劃系統完成作業</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnReturn" cssclass="btn-success btnw4" runat="server" Text="回主畫面" OnClick="btnReturn_Click"/>
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
