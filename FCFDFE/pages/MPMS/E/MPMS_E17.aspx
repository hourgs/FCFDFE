<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E17.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E17" %>
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
                    <!--標題-->交貨暨驗收情形
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                                <asp:GridView ID="GV_TBMDELIVERY_ITEM" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_TBMDELIVERY_ITEM_PreRender" OnRowDataBound="GV_TBMDELIVERY_ITEM_RowDataBound" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="作業">
                                            <ItemTemplate>
                                                <div class="control-group">
                                                    <asp:Button ID="btnTakeOver" CssClass="btn-success btnw4 text-center" OnClick="btnTakeOver_Click" Text="修改" runat="server" />
                                                    <asp:Button ID="btnDel" CssClass="btn-danger btnw4 text-center" OnClick="btnDel_Click" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" Text="刪除" runat="server" />
                                                </div>
                                                    <div class="control-group">
                                                    <asp:Button ID="btnApply" CssClass="btn-success btnw4 text-center" OnClick="btnApply_Click" Text="檢驗申請" runat="server" />
                                                <asp:Button ID="btnSettlement" CssClass="btn-success btnw4 text-center" OnClick="btnSettlement_Click" Text="結算證明" runat="server" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="交貨批次" DataField="ONB_SHIP_TIMES" />
                                        <asp:BoundField HeaderText="契約交貨日期" DataField="OVC_DELIVERY_CONTRACT" />
                                        <asp:BoundField HeaderText="實際交貨日期" DataField="OVC_DELIVERY" />
                                        <asp:BoundField HeaderText="會驗日期" DataField="OVC_DJOINCHECK" />
                                        <asp:BoundField HeaderText="結報日期" DataField="OVC_DPAY" />
                	                </Columns>
	   		                    </asp:GridView>
                        <div class="text-center">
                            <asp:Label CssClass="control-label text-blue" runat="server" Text="合約交貨批次：" Font-Size="Larger"></asp:Label>
                            <asp:Label ID="lblONB_DELIVERY_TIMES" CssClass="control-label text-blue" runat="server" Font-Size="Larger"></asp:Label>
                            <asp:Label CssClass="control-label text-blue" runat="server" Text="批" Font-Size="Larger"></asp:Label>
                        </div>
                            <br><br>
                            <asp:GridView ID="GV_NewTBMDELIVERY_ITEM" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_NewTBMDELIVERY_ITEM_PreRender" OnRowDataBound="GV_NewTBMDELIVERY_ITEM_RowDataBound" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                   		                    <asp:Button ID="btnNew" CssClass="btn-success btnw4 text-center" OnClick="btnNew_Click" Text="新增" runat="server" UseSubmitBehavior="True" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="交貨批次">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBox1" CssClass="tb tb-s" runat="server">1</asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="契約交貨日期" DataField="" />
                                    <asp:BoundField HeaderText="實際交貨日期" DataField="" />
                                    <asp:BoundField HeaderText="會驗日期" DataField="" />
                                    <asp:BoundField HeaderText="結報日期" DataField="" />
                	            </Columns>
	   		                </asp:GridView>
                        <div class="text-center">
                            <asp:Label CssClass="control-label text-red" runat="server" Text="(如果新增按鈕或修改、刪除按鈕不能執行，請檢察契約接管的主官核批日是否沒有輸入！)"></asp:Label>
                        </div>
                        <p></p>
                        <div class="text-center">
                            <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回主流程" />
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
