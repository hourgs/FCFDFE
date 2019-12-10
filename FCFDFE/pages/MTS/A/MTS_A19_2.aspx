<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A19_2.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A19_2" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                    <div>進口軍品運輸交接單-Step2 輸入交接單資料</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">運輸方法</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_IMPORT_TRANS_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server">啟運地點</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 42%">
                                        <asp:TextBox ID="txtOVC_START_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:DropDownList ID="drpOVC_START_PLACE" CssClass="tb tb-m" Visible="false" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server">運達地點</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 28%">
                                        <asp:TextBox ID="txtOVC_ARRIVE_PLACE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">起運時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_START_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">抵運時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_ARRIVE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接收單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:HiddenField ID="txtOVC_RECEIVE_DEPT_CDE" runat="Server" />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button CssClass="btn-success" Text="單位查詢" OnClientClick="OpenWindow('txtOVC_RECEIVE_DEPT_CDE','txtOVC_ONNAME')" runat="server" />
                                        <asp:Button CssClass="btn-default btnw4" Text="資料清空" OnClick="btnResetOVC_DEPT_CDE_CODE_Click" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總件數</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_TOTAL_QUANITY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_QUANITY_UNIT" CssClass="tb tb-s" OnSelectedIndexChanged="drpOVC_QUANITY_UNIT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtOVC_QUANITY_UNIT" CssClass="tb tb-xs" Visible="false" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總體積</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_TOTAL_VOLUME" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_VOLUME_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總重量</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_TOTAL_WEIGHT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_WEIGHT_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機航次</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_VOYAGE" CssClass="tb tb-m" runat="server"></asp:TextBox>
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
                                    <td colspan="4" class="td-inner-table">
                                        <asp:Panel ID="pnIRD_DETAIL" CssClass="text-left" runat="server"></asp:Panel>
                                        <asp:GridView ID="GVTBGMT_IRD_DETAIL" DataKeyNames="OVC_IRDDETAIL_SN" CssClass="table table-striped border-top table-inner" AutoGenerateColumns="false"
                                            OnPreRender="GVTBGMT_IRD_DETAIL_PreRender" OnRowCommand="GVTBGMT_IRD_DETAIL_RowCommand" OnRowDataBound="GVTBGMT_IRD_DETAIL_RowDataBound" runat="server">
                                            <Columns>
                                                <asp:TemplateField HeaderText="項次" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="提單號碼" DataField="OVC_BLD_NO" ReadOnly="true" />--%>
                                                <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <!--BLD顯示新方法-->
                                                        <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="購案號" DataField="OVC_PURCH_NO" />--%>
                                                <asp:TemplateField HeaderText="購案號">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_PURCH_NO" Text='<%#( Eval("OVC_PURCH_NO").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtOVC_PURCH_NO" Text='<%#( Eval("OVC_PURCH_NO").ToString() )%>' CssClass="tb tb-s" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="品名" DataField="OVC_ITEM_TYPE" ReadOnly="true" />
                                                <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" ReadOnly="true" />
                                                <asp:BoundField HeaderText="實收件數" DataField="ONB_ACTUAL_RECEIVE" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="購案號" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-success btnw2" Text="編輯" CommandName="btnModify" runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Button CssClass="btn-success btnw2" Text="更新" CommandName="btnSave" runat="server" />
                                                        <asp:Button CssClass="btn-success btnw2" Text="取消" CommandName="btnCancel" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button CssClass="btn-warning" Text="新增軍品運輸交接單" OnClick="btnNew_Click" runat="server" />
                    <br>
                    <br>
                    <asp:LinkButton OnClick="btnBack_Click" runat="server">繼續新增運輸交接單</asp:LinkButton>
                    <%--<asp:HyperLink NavigateUrl="~/pages/MTS/A/MTS_A19_1.aspx" runat="server">繼續新增運輸交接單</asp:HyperLink>--%>
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
