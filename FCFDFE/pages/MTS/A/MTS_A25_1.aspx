<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A25_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A25_1" %>
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
                    <div>提單匯入作業</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">提單編號</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">航次</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_VOYAGE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">機敏軍品</asp:Label></td>
                                    <td class="text-left">
                                        <asp:RadioButtonList ID="rdoOVC_IS_SECURITY" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />&emsp;
                                <asp:Button ID="btnNew" CssClass="btn-success btnw6" Text="直接新增提單" OnClick="btnNew_Click" runat="server" />
                            </div>
                            <div style="margin-top: 20px;">
                            <asp:GridView ID="GVTBGMT_BLD" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" OnPreRender="GVTBGMT_BLD_PreRender" OnRowCommand="GVTBGMT_BLD_RowCommand" OnRowDataBound="GVTBGMT_BLD_RowDataBound" runat="server">
                                <Columns>
                                    <%--<asp:BoundField HeaderText="提單編號" DataField="OVC_BLD_NO" />--%>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <!--BLD顯示新方法-->
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="承運航商" DataField="OVC_SHIP_COMPANY" />
                                    <asp:BoundField HeaderText="海空運別" DataField="OVC_SEA_OR_AIR" />
                                    <asp:BoundField HeaderText="船機名稱" DataField="OVC_SHIP_NAME" />
                                    <asp:BoundField HeaderText="船機航次" DataField="OVC_VOYAGE" />
                                    <asp:BoundField HeaderText="啟運日期" DataField="ODT_START_DATE"/>
                                    <asp:BoundField HeaderText="啟運船埠" DataField="OVC_START_PORT" />
                                    <asp:BoundField HeaderText="預估抵運時間" DataField="ODT_PLN_ARRIVE_DATE"/>
                                    <asp:BoundField HeaderText="實際抵運時間" DataField="ODT_ACT_ARRIVE_DATE"/>
                                    <asp:BoundField HeaderText="抵運船埠" DataField="OVC_ARRIVE_PORT" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnModify" CssClass="btn-success btnw2" Text="管理" CommandName="btnModify" CommandArgument='' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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