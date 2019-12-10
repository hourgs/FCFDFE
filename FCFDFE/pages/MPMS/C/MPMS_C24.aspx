<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C24.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C24" %>

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
                    計畫評核系統<br />
                    登錄購案核批資訊(轉送採購發包單位　或　轉呈上一級評核單位)
                </header>
                <table class="table table-bordered" style="text-align: center">
                    <tr>
                        <td>
                            <asp:Label CssClass="control-label" runat="server">計劃年度(第二組)： </asp:Label>
                            <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server">
                                <asp:ListItem Value="值">106</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnWait" CssClass="btn-success btnw" runat="server" OnClick="btnWait_Click" Text="待移訂約查詢" />
                            <asp:Button ID="btnQuery" CssClass="btn-success btnw4" OnClick="btnQuery_Click" runat="server" Text="全部查詢" />
                            <asp:Button ID="btnRM" CssClass="btn-success btnw4" runat="server" Text="回主畫面" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="control-label text-red" runat="server">(已經完成綜簽且為確認審者資料才顯示)</asp:Label>
                                <asp:GridView ID="GV_OVC" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false" 
                                    OnPreRender="GV_OVC_PreRender" OnRowCommand="GV_OVC_RowCommand" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="作業">
                                            <ItemTemplate>
                                                <asp:Button ID="btnDo" CssClass="btn-danger btnw2" Text="查詢" CommandName="btnDo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                           <asp:Label ID="lblOVC_PURCH" CssClass="control-label" Text='<%# "" + Eval("OVC_PURCH")+ Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                        <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                        <asp:BoundField HeaderText="委購單位" DataField="OVC_PUR_NSECTION" />
                                        <asp:BoundField HeaderText="審查次數" DataField="ONB_CHECK_TIMES" />
                                        <asp:BoundField HeaderText="分派日" DataField="OVC_DRECEIVE" />
                                        <asp:BoundField HeaderText="確認審完成日" DataField="OVC_DRESULT" />
                                        <asp:TemplateField HeaderText="清除核定日期、文號">
                                            <ItemTemplate>
                                                <asp:Button ID="btnClear" CssClass="btn-danger btnw" Text="清除核定日期、文號" 
                                                    CommandName="btnClear" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                </div>
            </section>
            </div>
        </div>
    </div>
</asp:Content>
