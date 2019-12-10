<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E18.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E18" %>
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
                    <!--標題-->檢驗申請
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                                    <asp:GridView ID="GV_TBMAPPLY_INSPECT_NEW" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_TBMAPPLY_INSPECT_NEW_PreRender" OnRowDataBound="GV_TBMAPPLY_INSPECT_NEW_RowDataBound" runat="server" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="動作">
                                                <ItemTemplate>
                   		                            <asp:Button ID="btnTakeOver" CssClass="btn-success btnw4 text-center" Text="修改" OnClick="btnTakeOver_Click" runat="server" />
                                                    <asp:Button ID="btnDel" CssClass="btn-danger btnw4 text-center" Text="刪除" OnClick="btnDel_Click" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="合約編號" DataField="OVC_PURCH" />
                                            <asp:BoundField HeaderText="批次" DataField="ONB_TIMES" />
                                            <asp:BoundField HeaderText="復驗次數" DataField="ONB_INSPECT_TIMES" />
                                            <asp:BoundField HeaderText="再驗次數" DataField="ONB_RE_INSPECT_TIMES" />
                                            <asp:BoundField HeaderText="申請日期" DataField="OVC_DAPPLY" />
                                            <asp:BoundField HeaderText="檢驗單位" DataField="OVC_INSPECT_UNIT" />
                	                    </Columns>
	   		                        </asp:GridView>
                            <br><br>
                                    <asp:GridView ID="GV_TBMAPPLY_INSPECT" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_TBMAPPLY_INSPECT_PreRender" OnRowDataBound="GV_TBMAPPLY_INSPECT_RowDataBound" runat="server" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="動作">
                                                <ItemTemplate>
                   		                            <asp:Button ID="btnNew" CssClass="btn-success btnw2 text-center" Text="新增" OnClick="btnNew_Click" runat="server" UseSubmitBehavior="True" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="合約編號" DataField="" ItemStyle-CssClass="text-center" /><asp:TemplateField HeaderText="批次">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtONB_TIMES" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="複驗次數">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtONB_INSPECT_TIMES" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="再驗次數">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtONB_RE_INSPECT_TIMES" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="申請日期">
                                                <ItemTemplate>
                                                    <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DAPPLY" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="檢驗單位">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpOVC_INSPECT_UNIT" CssClass="tb tb-m" runat="server"></asp:DropDownList>
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
