<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D34.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D34" %>

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
                <header class="title text-blue">
                    <!--標題-->
                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>購案無法決標公告編輯           
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" id="divForm" style="border: solid 2px;" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <asp:GridView ID="GV_info" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_PURCH" OnRowCommand="GV_info_RowCommand" runat="server">
                            <Columns>
                                <asp:TemplateField HeaderText="作業">
                                    <ItemTemplate>
                                        <asp:Button CssClass="btn-default" ID="btnWork" Text="異動" CommandName="btnWork" runat="server" />
                                        <asp:Button CssClass="btn-warning" ID="btnDel" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" Text="刪除" CommandName="btnDel" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="購案編號">
                                            <ItemTemplate>
                                                <asp:Label  CssClass="control-label" Text='<%# Eval("OVC_PURCH") %>' runat="server"></asp:Label>
                                                <asp:Label  CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                                <asp:Label  CssClass="control-label" Text='<%# Eval("OVC_PURCH_5") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                <asp:BoundField HeaderText="開標次數" DataField="ONB_TIMES" />
                                <asp:BoundField HeaderText="開標日期" DataField="OVC_DOPEN" />
                                <asp:BoundField HeaderText="資料傳輸日" DataField="OVC_DSEND" />
                                <asp:BoundField HeaderText="承辦人" DataField="OVC_NAME" />
                            </Columns>
                        </asp:GridView>

                        <div class="text-center">
                            <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="btn-default" runat="server" Text="新增無法決標公告" />
                            <asp:Button ID="btnReturn" OnClick="btnReturn_Click" CssClass="btn-default" runat="server" Text="回開標結果作業" />
                            <asp:Button ID="btnReturnM" OnClick="btnReturnM_Click" CssClass="btn-default" runat="server" Text="回主流程畫面" />
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
