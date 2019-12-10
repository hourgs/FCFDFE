<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D38.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D38" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .subtitle{
            font-size:18px;
            text-align:center;
            padding-bottom:10px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                    契約草稿明細編輯作業          
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="subtitle">
                            　得標商：<asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label text-red" runat="server"></asp:Label>
                            契約編號：<asp:Label ID="lblOVC_PURCH_6" CssClass="control-label" runat="server"></asp:Label>
                            組別：<asp:Label ID="lblONB_GROUP" CssClass="control-label text-red" runat="server"></asp:Label><br />
                           <asp:Label CssClass="control-label text-blue" runat="server">選擇本草約之明細</asp:Label>（<asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>）

                        </div>
                        <asp:GridView ID="GV_info" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_PURCH" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="選擇">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" CssClass="radioButton" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" Visible="false" />
                            <asp:BoundField HeaderText="採購號碼" DataField="OVC_PURCH_5" Visible="false" />
                            <asp:BoundField HeaderText="分約號" DataField="OVC_PURCH_6" Visible="false"/>
                            <asp:BoundField HeaderText="原採購項次" DataField="ONB_POI_ICOUNT" />
                            <asp:BoundField HeaderText="合約項次" DataField="ONB_ICOUNT" />
                            <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                            <asp:BoundField HeaderText="得標商統一編號" DataField="OVC_VEN_CST" Visible="false" />
                            <asp:BoundField HeaderText="名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="料號" DataField="NSN" />
                            <asp:BoundField HeaderText="單位" DataField="OVC_POI_IUNIT" />
                            <asp:BoundField HeaderText="採購數量" DataField="ONB_POI_QORDER_PLAN" />
                        </Columns>
                    </asp:GridView>

                        <div class="text-center" style="margin-bottom: 10px;">
                            <asp:Button ID="btnSAll" OnClick="btnSAll_Click" CssClass="btn-default btnw4" runat="server" Text="選擇全部" />
                            <asp:Button ID="btnClear" OnClick="btnClear_Click" CssClass="btn-default btnw4" runat="server" Text="清除全部" />
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn-default btnw4" runat="server" Text="確認存檔" />
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnPrice" OnClick="btnPrice_Click" CssClass="btn-default" runat="server" Text="單價製作" />
                            <asp:Button ID="btnReturn" OnClick="btnReturn_Click" CssClass="btn-default" runat="server" Text="回契約草稿製作畫面" />
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
