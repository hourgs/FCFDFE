<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E21.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E21" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    待交貨
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">待交貨</div>
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="text-center" style="border:1px solid black; padding:10px; margin-bottom:2%;">
                                        <asp:Button style="margin:0 2rem;" ID="btnBid" CssClass="btn-success btn-lg" OnClick="btnBid_Click" runat="server" Text="申辦免稅" />
                                        <asp:Button style="margin:0 2rem;" ID="btnSup" CssClass="btn-success btn-lg" OnClick="btnSup_Click" runat="server" Text="履約督導" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="text-center">
                            <asp:Button style="justify-content: center;" ID="btnReturn" CssClass="btn-default btn-lg" OnClick="btnReturn_Click" runat="server" Text="回主流程" />
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
