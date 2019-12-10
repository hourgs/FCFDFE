<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D31.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D31" %>

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
                    <h1>購案開標紀錄作業編輯</h1>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <asp:GridView ID="GV_info" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_PURCH" OnRowCommand="GV_info_RowCommand" runat="server">
                        <Columns>
                            <asp:BoundField HeaderText="編號" DataField="" />
                            <asp:TemplateField HeaderText="作業">
                                <ItemTemplate>
                                    <asp:Button ID="btnDo" CssClass="btn-danger btnw2" Text="作業" CommandName="按鈕屬性" runat="server" />
                                    <asp:Label CssClass="control-label" runat="server">由編號：</asp:Label>
                                    <asp:TextBox ID="txtNUM" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnCopy" CssClass="btn-danger btnw4" Text="複製資料" CommandName="按鈕屬性" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="開標次數" DataField="ONB_TIMES" />
                            <asp:BoundField HeaderText="開標時間" DataField="OVC_DOPEN" />
                            <asp:BoundField HeaderText="開標主持人" DataField="OVC_CHAIRMAN" />
                            <asp:BoundField HeaderText="開標結果" DataField="OVC_RESULT" />
                            <asp:BoundField HeaderText="主官批核日" DataField="OVC_DAPPROVE" />
                        </Columns>
                    </asp:GridView>

                    <div class="title text-center text-red">
                        <asp:Label CssClass="control-label" runat="server">選擇【複製資料】可由所輸入的【編號】帶入預設值，節省輸入時間</asp:Label>
                    </div>


                    <br />
                    <div class="text-center">
                        <asp:Button ID="btnReturn" CssClass="btn-warning btnw4" runat="server" Text="上一頁" />
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->

                </footer>
            </section>
        </div>
    </div>
</asp:Content>
