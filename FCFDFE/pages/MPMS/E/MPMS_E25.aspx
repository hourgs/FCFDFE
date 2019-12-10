<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E25.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E25" %>

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
                <header class="title">
                    <!--標題-->
                    履約督導紀錄
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GV_FREE_TAX_DETAIL" DataKeyNames="OVC_PURCH" CssClass="table data-table table-striped border-top text-center" OnRowDataBound="GV_FREE_TAX_DETAIL_RowDataBound" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="動作">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" CssClass="btn-success btnw2" Text="修改" OnClick="btnEdit_Click" CommandName="DataEdit" runat="server" />
                                            <asp:Button ID="btnDel" CssClass="btn-success btnw2" Text="刪除" OnClick="btnDel_Click" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" CommandName="DataDel" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="分約號" DataField="" />
                                    <asp:BoundField HeaderText="交貨批次" DataField="ONB_TIMES" />
                                    <asp:BoundField HeaderText="督導次數" DataField="ONB_AUDIT" />
                                    <asp:BoundField HeaderText="履驗承辦人" DataField="OVC_DO_NAME" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="" />
                                </Columns>
                            </asp:GridView>

                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Text="動作" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label2" Text="購案編號" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label3" Text="分約號" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label4" Text="交貨批次" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label5" Text="督導次數" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label6" Text="履驗承辦人" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label7" Text="購案名稱" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnNew" CssClass="btn-default btnw2" OnClick="btnNew_Click" runat="server" Text="新增" /></td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH_6" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtONB_TIMES" CssClass="tb tb-s" runat="server">1</asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtONB_AUDIT" CssClass="tb tb-s" runat="server">系統自動計算</asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_DO_NAME" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_TAX_STUFF" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="text-center">
                                        <asp:Button ID="btnReturn" CssClass="btn-default btnw4" runat="server" OnClick="btnReturn_Click" Text="回上一頁" />
                                        <asp:Button ID="btnRM" CssClass="btn-default btnw4" runat="server" OnClick="btnRM_Click" Text="回主流程" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
