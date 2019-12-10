<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B14_2.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B14_2" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->
                    <div>結報申請表-修正</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="PnMessageIINF" runat="server"></asp:Panel>
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_INF_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">發文字號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtISSU_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預算科目及編號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_BUDGET" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:35%">
                                        <asp:TextBox ID="txtOVC_PURPOSE_TYPE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">結報申請日期</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:35%">
                                       <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_APPLY_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">金額</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">新台幣</asp:Label>
                                        <asp:TextBox ID="txtONB_AMOUNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>
                                        <asp:Label CssClass="control-label" ID="lblOVC_BUDGET_INF_NO" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" ID="lblONB_AMOUNT" runat="server"></asp:Label>
                                    </td>
                                </tr>
                           <%--     <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預算通知書編號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_BUDGET_INF_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        <asp:Button CssClass="btn-success btnw10" Text="載入預算通知單編號" runat="server" />
                                        <asp:Label CssClass="control-label text-red" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">收據號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_INV_NO" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">收據日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                         <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INV_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_NOTE" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">保險公司</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpCO_SN" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_PLN_CONTENT" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-warning" OnClick="btnModify_Click" Text="修改結報申報表" runat="server" />
                                <asp:Button CssClass="btn-success" OnClick="btnBack_Click" Text="回結報申請表管理" runat="server" />
                            </div><br />
                            <asp:Panel ID="PnMessageQuery" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:20%;">
                                        <asp:Label CssClass="control-label" runat="server">投保通知書編號</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:25%;">
                                        <asp:TextBox ID="txtOVC_IINN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:30%" rowspan="2">
                                        <asp:DropDownList ID="drpODT_INS_DATE" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnFilter_Click" Text="過濾" runat="server" />
                                    </td>
                                    <td style="width:25%" rowspan="2">
                                        <asp:Button CssClass="btn-success btnw4" OnClick="btnCheckBox_Click" CommandArgument="true" Text="全部勾選" runat="server" />
                                        <asp:Button CssClass="btn-default btnw4" OnClick="btnCheckBox_Click" CommandArgument="false" Text="全部取消" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td> 
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="text-left">
                                        <asp:CheckBoxList ID="chkOVC_IINN_NO" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="3" runat="server"></asp:CheckBoxList>
                                    </td>
                                </tr> 
                                
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success" OnClick="btnNew_Click" Text="↓新增投保通知書" runat="server" />
                            </div>
                            <br />
                            <asp:Panel ID="PnMessageIINN" runat="server"></asp:Panel>
                            <asp:GridView ID="GV_TBGMT_IINN" DataKeyNames="OVC_IINN_NO" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_IINN_PreRender" OnRowCommand="GV_TBGMT_IINN_RowCommand" OnRowDataBound="GV_TBGMT_IINN_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="投保通知書編號" DataField="OVC_IINN_NO" />
                                    <%--<asp:BoundField HeaderText="提單編號" DataField="OVC_BLD_NO" />--%>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <!--BLD顯示新方法-->
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
                                    <asp:BoundField HeaderText="投保金額" DataField="ONB_INS_AMOUNT" />
                                    <asp:BoundField HeaderText="保費(台幣)" DataField="OVC_FINAL_INS_AMOUNT" />
                                    <asp:BoundField HeaderText="軍種" DataField="OVC_CLASS_NAME" />
                                    <asp:BoundField HeaderText="交貨條件" DataField="OVC_DELIVERY_CONDITION" />
                                    <asp:BoundField HeaderText="軍購或商購" DataField="OVC_PURCHASE_TYPE" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-danger btnw2" Text="刪除" CommandName="dataDel" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
