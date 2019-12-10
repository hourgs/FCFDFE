<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="unitquery.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.unitquery" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
         });
         //單位查詢視窗function 設定位置、大小等
         function OpenWindow() {
             var win_width = 600;
             var win_height = 150;
             var PosX = (screen.width - win_width) / 2;
             var PosY = (screen.Height - win_height) / 2;
             features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
             var theURL = '<%=ResolveClientUrl("~/Content/unitQuery.aspx")%>';
             var newwin = window.open(theURL, 'unitQuery', features);
         }
         //單位查詢視窗function結束
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    單位查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <!--所有使用單位查詢的TextBox，id全部都要一致，如id="txtOvcOnname"，另外檢查，此頁面master page
                                            所包的content其ContentPlaceHolderID是否是MainContent，否的話請改成MainContent-->
                                        <!--單位查詢的button的 OnClientClick其中query.aspx的位置請依照情況不同做修改-->
                                        <asp:TextBox id="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnQuery" OnClick="btnQuery_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                        <asp:TextBox id="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;">
                                        <asp:Label CssClass="control-label" runat="server">委託</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <!--所有使用單位查詢的TextBox，id全部都要一致，如id="txtOvcOnname"，另外檢查，此頁面master page
                                            所包的content其ContentPlaceHolderID是否是MainContent，否的話請改成MainContent-->
                                        <!--單位查詢的button的 OnClientClick其中query.aspx的位置請依照情況不同做修改-->
                                        <asp:TextBox id="txtOVC_AGENT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnAugent" onclick="btnAugent_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                        <asp:TextBox id="txtOVC_AGENT_UNIT_exp" CssClass="tb tb-m" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;">
                                        <asp:Label CssClass="control-label" runat="server">最後計畫</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <!--所有使用單位查詢的TextBox，id全部都要一致，如id="txtOvcOnname"，另外檢查，此頁面master page
                                            所包的content其ContentPlaceHolderID是否是MainContent，否的話請改成MainContent-->
                                        <!--單位查詢的button的 OnClientClick其中query.aspx的位置請依照情況不同做修改-->
                                        <asp:TextBox id="txtOVC_AUDIT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnAudit" onclick="btnAudit_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                        <asp:TextBox id="txtOVC_AUDIT_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;">
                                        <asp:Label CssClass="control-label" runat="server">採購發包</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <!--所有使用單位查詢的TextBox，id全部都要一致，如id="txtOvcOnname"，另外檢查，此頁面master page
                                            所包的content其ContentPlaceHolderID是否是MainContent，否的話請改成MainContent-->
                                        <!--單位查詢的button的 OnClientClick其中query.aspx的位置請依照情況不同做修改-->
                                        <asp:TextBox id="txtOVC_PURCHASE_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnPurchase" onclick="btnPurchase_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                        <asp:TextBox id="txtOVC_PURCHASE_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;">
                                        <asp:Label CssClass="control-label" runat="server">履約驗結</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <!--所有使用單位查詢的TextBox，id全部都要一致，如id="txtOvcOnname"，另外檢查，此頁面master page
                                            所包的content其ContentPlaceHolderID是否是MainContent，否的話請改成MainContent-->
                                        <!--單位查詢的button的 OnClientClick其中query.aspx的位置請依照情況不同做修改-->
                                        <asp:TextBox id="txtOVC_CONTRACT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnContract" onclick="btnContract_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                        <asp:TextBox id="txtOVC_CONTRACT_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>

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