<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E1A.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E1A" %>
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
                    <!--標題-->結算驗收證明
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                                    <asp:GridView ID="GV_TBMPAY_MONEY_1" CssClass=" table data-table table-striped border-top text-center" OnRowDataBound="GV_TBMPAY_MONEY_1_RowDataBound" OnPreRender="GV_TBMPAY_MONEY_1_PreRender" runat="server" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="作業">
                                                <ItemTemplate>
                   		                            <asp:Button ID="btnTakeOver" CssClass="btn-success btnw4 text-center" OnClick="btnTakeOver_Click" Text="修改" runat="server" />
                                                    <asp:Button ID="btnDel" CssClass="btn-danger btnw4 text-center" OnClick="btnDel_Click" Text="刪除" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" runat="server" /><br>
                                                    </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="合約編號" DataField="OVC_PURCH" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField HeaderText="工作分支計畫(預算科目)" DataField="OVC_POI_IBDG" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField HeaderText="工作分支計畫(名稱)" DataField="OVC_PJNAME" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField HeaderText="結算次數" DataField="ONB_TIMES" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField HeaderText="結算金額" DataField="ONB_PAY_MONEY" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField HeaderText="結算日期" DataField="OVC_DPAY" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" ItemStyle-CssClass="text-center" />
                	                    </Columns>
	   		                        </asp:GridView>
                            <br><br>
                                    <asp:GridView ID="GV_TBMPAY_MONEY_2" CssClass=" table data-table table-striped border-top text-center" OnRowDataBound="GV_TBMPAY_MONEY_2_RowDataBound" OnPreRender="GV_TBMPAY_MONEY_2_PreRender" runat="server" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField HeaderText="購案編號" DataField="" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField HeaderText="合約編號" DataField="" ItemStyle-CssClass="text-center" />
                                            <asp:TemplateField HeaderText="結算次數">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSettlementTimes" CssClass="tb tb-s" runat="server">1</asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="合約商編號(統一編號)" DataField="" ItemStyle-CssClass="text-center" />
                                            <asp:TemplateField HeaderText="工作分支計畫(預算科目)">
                                                <ItemTemplate>
                                                        <asp:Label CssClass="control-label" runat="server" Text="代號"></asp:Label>
                                                        <asp:DropDownList ID="drpOVC_POI_IBDG" CssClass="tb tb-s" OnSelectedIndexChanged="drpOVC_POI_IBDG_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                        <asp:TextBox ID="txtOVC_POI_IBDG" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="功能">
                                                <ItemTemplate>
                   		                            <asp:Button ID="btnNew" CssClass="btn-success btnw4 text-center" OnClick="btnNew_Click" Text="新增" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                	                    </Columns>
	   		                        </asp:GridView>
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
