<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D36.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D36" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .no-r {
            border-right: 0px !important;
        }

        .no-l {
            border-left: 0px !important;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <h1>契約製作編輯</h1>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="subtitle text-red">步驟1：契約製作</div>
                        <div class="text-center">
                            <asp:Label CssClass="control-label" runat="server">契約草稿、明細及單價等作業</asp:Label>
                            <asp:GridView ID="GV_TBM1302" CssClass="table data-table table-striped border-top" DataKeyNames="ONB_GROUP" OnPreRender="GV_TBM1302_PreRender" OnRowCommand="GV_TBM1302_RowCommand" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button  CssClass="btn-default btnw2" Text="異動" CommandName="btnMove" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="購案編號">
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH") %>' runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_5") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="合約編號" DataField="OVC_PURCH_6" />
                                    <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                                    <asp:BoundField HeaderText="得標商名稱" DataField="OVC_VEN_TITLE" />
                                    <asp:BoundField HeaderText="電話" DataField="OVC_VEN_TEL" />
                                    <asp:TemplateField HeaderText="簽約日(已移履約)">
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" Text='<%# Eval("OVC_DCONTRACT") %>' runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text="(" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text=")" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="text-center">
                            <asp:Label CssClass="control-label" runat="server">請選擇已簽辦之開標紀錄及開標結果</asp:Label>
                            <asp:GridView ID="GV_TBM1303" DataKeyNames="ONB_GROUP" CssClass="table data-table table-striped border-top" OnPreRender="GV_TBM1303_PreRender" OnRowCommand="GV_TBM1303_RowCommand" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-default btnw2" Text="新增" CommandName="btnNew" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="開標日" DataField="OVC_DOPEN" />
                                    <asp:BoundField HeaderText="開標次數" DataField="ONB_TIMES" />
                                    <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                                    <asp:BoundField HeaderText="開標結果" DataField="OVC_RESULT" />
                                    <asp:BoundField HeaderText="決標日" DataField="OVC_DBID" />
                                    <asp:BoundField HeaderText="主官批核日" DataField="OVC_DAPPROVE" />
                                </Columns>
                            </asp:GridView>

                            <div class="text-center">
                                <p>↓</p>
                            </div>
                        </div>
                        <div class="subtitle text-red">步驟2：合約移履歷</div>
                        <div class="text-center">
                            <asp:Label CssClass="control-label" runat="server">請勾選（單筆或多筆）可移履約驗結單位之合約，再按『移履約』！
                            </asp:Label>
                            <asp:GridView ID="GV_agreen" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_agreen_PreRender" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="勾選">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" Visible='<%#  Eval("OVC_DSEND").ToString() != "" ? false:true %>' runat="server" />
                                            <asp:Label  runat="server" Text="已於" Visible='<%#  Eval("OVC_DSEND").ToString() != "" ? true:false %>'></asp:Label>
                                            <asp:Label  runat="server" Text='<%# Eval("OVC_DSEND") %>' Visible='<%#  Eval("OVC_DSEND").ToString() != "" ? true:false %>'></asp:Label>
                                            <asp:Label  runat="server" Text="移履約" Visible='<%#  Eval("OVC_DSEND").ToString() != "" ? true:false %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="履約單位收案狀況">
                                        <ItemTemplate>
                                            <asp:Label  runat="server" Text="履約已於" Visible='<%#  Eval("OVC_DRECEIVE").ToString() != "" ? true:false %>'></asp:Label>
                                            <asp:Label  runat="server" Text='<%# Eval("OVC_DRECEIVE") %>' Visible='<%#  Eval("OVC_DRECEIVE").ToString() != "" ? true:false %>'></asp:Label>
                                            <asp:Label  runat="server" Text="收辦" Visible='<%#  Eval("OVC_DRECEIVE").ToString() != "" ? true:false %>'></asp:Label>
                                            <asp:Label  runat="server" Text="履約尚未收辦" Visible='<%#  Eval("OVC_DRECEIVE").ToString() != "" ? false:true %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="履約單位收案狀況" DataField="OVC_DSEND" />
                                    <asp:BoundField HeaderText="履約單位（由計評單位核定）" DataField="OVC_CONTRACT_UNIT" />
                                    <asp:TemplateField HeaderText="購案編號">
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH") %>' runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_5") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="合約編號" DataField="OVC_PURCH_6" />
                                    <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                                    <asp:BoundField HeaderText="得標商名稱" DataField="OVC_VEN_TITLE" />
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                                <asp:Button ID="btnSAll" OnClick="btnSAll_Click" CssClass="btn-default" runat="server" Text="勾選全部" />
                                <asp:Button ID="btnClear" OnClick="btnClear_Click" CssClass="btn-default" runat="server" Text="清除全部" />
                                <asp:Button ID="btnMove" OnClick="btnMove_Click" CssClass="btn-default" runat="server" Text="移履約" />
                            </div>
                            <div class="text-center">
                                <p>↓</p>
                            </div>
                        </div>
                        <div class="subtitle text-red">步驟3：輸入全案（案號：<asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>）採購發包階段結束日<asp:Label CssClass="text-blue control-label" runat="server">（所有合約皆已移至履約單位才可輸入）</asp:Label></div>
                        <table class="table table-bordered text-center" style="margin-top: 0px">
                            <tr class="no-bordered">
                                <td style="width: 45%" class="text-right no-r">
                                    <asp:Label CssClass="control-label text-red" runat="server">購定階段結束日</asp:Label>
                                </td>
                                <td class="no-l">
                                    <div class='input-append datetimepicker'>
                                        <asp:TextBox ID="txtOVC_DEND" CssClass='tb tb-m position-left' runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div><asp:Button ID="btnSAVE" OnClick="btnSAVE_Click" CssClass="btn-default" runat="server" Text="存檔" />
                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="brnReturn" CssClass="btn-default" OnClick="brnReturn_Click" runat="server" Text="回上一頁" />
                        </div>
                        <div>
                            <asp:Label CssClass="control-label" runat="server">註１：結束日存檔成功後，購案不再於採包系統中顯示（除非合約經履驗單位退案）！</asp:Label><br />
                            <asp:Label CssClass="control-label" runat="server">註２：請確定步驟２須移送之合約，已顯示「已於年．月．日移履約」，再輸入結束日！</asp:Label><br />
                            <asp:Label CssClass="control-label" runat="server">註３：若合約曾經履驗單位退案，重新移送該筆合約後，請再次輸入結束日！</asp:Label><br />
                            <asp:Label CssClass="control-label" runat="server">註４：合約已建立者原則上不可刪除（合約為履驗系統資料來源）未移履約前如須刪除，請至開標紀錄第二頁刪除得標廠商對應之契約尾號即可，為該筆合約須重新製作！</asp:Label>
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
