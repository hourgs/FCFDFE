<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D17_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D17_1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title"><!--標題-->
                    疑義、異議
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <table class="table table-bordered text-center">
                        <tr><td>請選擇疑義電傳表內容</td></tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rdo1" OnSelectedIndexChanged="rdoOVC_CHAIRMAN_SelectedIndexChanged" AutoPostBack="true" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem>【該公司所陳雖已逾疑義期限，仍請針對疑義事項逐條提供釋疑（如表一）】</asp:ListItem>
                                        <asp:ListItem>【請即針對疑義事項逐條提供釋疑（如表一）】</asp:ListItem>
                                    </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>


                    <div>
                        <div class="subtitle">預覽列印</div>
                        <div class="text-center diva">
                            <asp:Button ID="btnD17_1" CssClass="btn-default" Text="疑義電傳表" OnClick="btnD17_1_Click" runat="server" />
                            <asp:Button ID="btnD17_2" CssClass="btn-default" Text="異議電傳表" OnClick="btnD17_2_Click" runat="server" />
                            <asp:Button ID="btnD17_3" CssClass="btn-default" Text="傳真申請表" OnClick="btnD17_3_Click" runat="server" />
                        </div>
                    </div>
                </div><br />
                
                <div class="text-center">
                    <asp:Button ID="btnReturn" CssClass="btn-default btnw4" Text="回上一頁" OnClick="btnReturn_Click" runat="server" />
                </div><br />
                <footer class="panel-footer" style="text-align: center;">
                <!--網頁尾-->

                </footer>
            </section>
        </div>
    </div>
</asp:Content>
