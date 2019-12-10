<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D29.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D29" %>

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
                <header class="title text-blue">
                    <!--標題-->
                    <h1>購案截標審查作業編輯</h1>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="text-center">
                        <asp:Label CssClass="control-label" runat="server">請先選擇本案已簽辦之開標日期</asp:Label>
                    </div>
                    <asp:GridView ID="GV_info" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_PURCH" OnRowCommand="GV_info_RowCommand" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="作業">
                                <ItemTemplate>
                                    <asp:Button ID="btnDo" CssClass="btn-danger btnw2" Text="作業" CommandName="按鈕屬性" runat="server" />
                                </ItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="按鈕屬性" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="開標次數" DataField="ONB_TIMES" />
                            <asp:BoundField HeaderText="開標時間" DataField="OVC_DOPEN" />
                            <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div class="text-center">
                    <asp:Button ID="btnReturn" CssClass="btn-warning btnw4" runat="server" Text="上一頁" />
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->

                </footer>
            </section>
        </div>
    </div>
</asp:Content>

