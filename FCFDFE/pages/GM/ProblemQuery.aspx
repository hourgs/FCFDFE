<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProblemQuery.aspx.cs" Inherits="FCFDFE.pages.GM.ProblemQuery" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
<%--        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });--%>
</script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    問題單查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">問題單單號</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_CLAIM_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">建檔時間</asp:Label>
                                    </td>
                                    <td colspan="7">
                                        <div class="input-append">
                                            <asp:DropDownList ID="YEARLIST" CssClass="tb tb-m" runat="server" ></asp:DropDownList>
                                            <asp:Label CssClass="control-label" Text="年" runat="server"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">子系統名稱</asp:Label>
                                    </td>
                                    <td colspan="7">
                                        <asp:DropDownList ID="drpC_SN_SYS" CssClass="tb tb-m" runat="server" ></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" /><br />
                                <br />
                                 <asp:GridView ID="GVPRO_DATA" DataKeyNames="PRO_SN" CssClass="table data-table table-striped border-top table-bordered" AutoGenerateColumns="false" OnPreRender="GVPRO_DATA_PreRender" OnRowCommand ="GVPRO_DATA_RowCommand" runat="server">
                                        <Columns>
                                            <asp:BoundField HeaderText="GUID" DataField="PRO_SN" Visible="false"/>
                                            <asp:BoundField HeaderText="案號" DataField="PRO_ID" />
                                            <asp:BoundField HeaderText="系統別" DataField="PROSYS_CH" />
                                            <asp:BoundField HeaderText="建單人員" DataField="USER_NAME" />
                                            <asp:BoundField HeaderText="日期" DataField="ODT_CREATE_DATE" />
                                            <asp:TemplateField HeaderText="詳細資料" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnManage" Width ="100px" CssClass="btn-success btnw2" Text="詳細資料" CommandName="btnDetail" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                            </div>
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

