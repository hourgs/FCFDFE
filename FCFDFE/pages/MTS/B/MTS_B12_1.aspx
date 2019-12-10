<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B12_1.aspx.cs" EnableEventValidation="false" Inherits="FCFDFE.pages.MTS.B.MTS_B12_1" %>
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
                    <div>投保通知書-管理</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">投保通知書編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_IINN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                            </div>
                            <asp:GridView ID="GVTBGMT_IINN" DataKeyNames="OVC_IINN_NO" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" OnPreRender="GVTBGMT_IINN_PreRender" OnRowCommand="GVTBGMT_IINN_RowCommand" OnRowDataBound="GVTBGMT_IINN_RowDataBound" runat="server">
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
                                    <asp:BoundField HeaderText="軍種" DataField="OVC_MILITARY_TYPE" />
                                    <asp:BoundField HeaderText="交貨條件" DataField="OVC_DELIVERY_CONDITION" />
                                    <asp:BoundField HeaderText="軍購或商購" DataField="OVC_PURCHASE_TYPE" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" Text="修改" CommandName="dataModify" CommandArgument='' UseSubmitBehavior="False" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-danger btnw2" Text="刪除" CommandName="dataDel" CommandArgument='' UseSubmitBehavior="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" Text="列印" CommandName="dataPrint" CommandArgument='' UseSubmitBehavior="false" runat="server" />
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
