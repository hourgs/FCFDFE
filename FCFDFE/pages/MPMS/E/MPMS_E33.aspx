<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E33.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E33" %>
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
                <header  class="title text-red">
                    <!--標題-->契約修訂
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GV_TBMCONTRACT_MODIFY" CssClass=" table data-table table-striped border-top text-center" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="動作">
                                        <ItemTemplate>
                   		                    <asp:Button ID="btnTakeOver" CssClass="btn-success btnw4 text-center" OnClick="btnTakeOver_Click" Text="修改" runat="server" />
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw4 text-center" OnClick="btnDel_Click" Text="刪除" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="分約號" DataField="OVC_PURCH_6" />
                                    <asp:BoundField HeaderText="修約次數" DataField="ONB_TIMES" />
                                    <asp:BoundField HeaderText="修約日期" DataField="OVC_DMODIFY" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                	            </Columns>
	   		                </asp:GridView>
                            <div class="text-center">
                                <asp:Button ID="btnNew" CssClass="btn-success" OnClick="btnNew_Click" runat="server" Text="新增" />
                                <asp:Button ID="btnBack" CssClass="btn-success" OnClick="btnBack_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnReturn" CssClass="btn-success" OnClick="btnReturn_Click" runat="server" Text="回主流程" />
                            </div>
                            <br />
                            <div class="text-center">
                                <asp:Button Visible="false" ID="btnPrint" CssClass="btn-success" runat="server" OnClick="btnPrint_Click" Text="列印廠商資料更動簽辦單.doc" />
                                <asp:LinkButton runat="server" OnClick="btnPrint_Click">列印廠商資料更動簽辦單.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton runat="server" OnClick="Unnamed_Click">列印廠商資料更動簽辦單.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton runat="server" OnClick="Unnamed_Click1">列印廠商資料更動簽辦單.odt</asp:LinkButton>
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
