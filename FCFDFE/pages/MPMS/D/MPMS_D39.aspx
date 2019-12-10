<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D39.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D39" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .subtitle {
            font-size: 18px;
            text-align: center;
            padding-bottom: 10px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                    契約草稿製作(單價製作)    
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="subtitle text-red">
                            購案契約編號：<asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                            合約商：<asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server"></asp:Label>
                            組別：<asp:Label ID="lblONB_GROUP" CssClass="control-label" runat="server"></asp:Label>
                        </div>
                        <asp:GridView ID="GV_info" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_PURCH" runat="server">
                            <Columns>
                                <asp:BoundField HeaderText="合約項次" DataField="ONB_ICOUNT" />
                                <asp:BoundField HeaderText="名稱" DataField="OVC_PUR_IPURCH" />
                                <asp:BoundField HeaderText="料號" DataField="NSN" />
                                <asp:BoundField HeaderText="單位" DataField="OVC_POI_IUNIT" />
                                <asp:TemplateField HeaderText="數量">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtaddress" CssClass="tb tb-l" Text='<%# Eval("ONB_POI_QORDER_CONT") %>' runat="server"></asp:TextBox></td>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="預算單價" DataField="ONB_POI_MPRICE_PLAN" />
                                <asp:TemplateField HeaderText="合約單價">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtaddress" CssClass="tb tb-l" Text='<%# Eval("ONB_POI_MPRICE_CONT") %>' runat="server"></asp:TextBox></td>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="總價">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtaddress" CssClass="tb tb-l" Text="0" runat="server"></asp:TextBox><!--待確認--></td>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table class="table table-bordered text-left">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" CssClass="control-label" runat="server">預算：</asp:Label>
                                    <asp:TextBox ID="txtONB_PUR_BUDGET" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label2" CssClass="control-label" runat="server">合約金額：</asp:Label>
                                    <asp:TextBox ID="txtONB_MONEY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label3" CssClass="control-label" runat="server">試算結果：</asp:Label>
                                    <asp:TextBox ID="txtResult" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label4" CssClass="control-label" runat="server">折讓：</asp:Label>
                                    <asp:TextBox ID="txtONB_MONEY_DISCOUNT" CssClass="tb tb-s" runat="server"></asp:TextBox>

                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="btnTS" CssClass="btn-default btn2 " runat="server" Text="試算" />
                            <asp:Button ID="btnAS" CssClass="btn-default" runat="server" Text="依決標比率自動計算合約單價" />
                            <asp:Button ID="btnSave" CssClass="btn-default" OnClick="btnSave_Click" runat="server" Text="存檔" />
                        </div>
                        <br />
                        <div class="text-center">
                            <a style="color: red; text-decoration: underline; font-size: 18px;" href="#" id="Download">下載合約明細</a>
                            <asp:Button ID="btnLoad" CssClass="btn-default" runat="server" Text="載入合約單價資料" /><br />
                            <a style="color: red; text-decoration: underline; font-size: 18px;" href="#" id="PDF_print" visible="false" runat="server">契約草稿明細預覽列印(pdf檔)</a>
                            <a style="color: red; text-decoration: underline; font-size: 18px;" href="#" id="WORD_print" visible="false" runat="server">契約草稿明細預覽列印(word檔)</a>
                            <asp:LinkButton ID="LinkButton1" CssClass="text-red" OnClick="LinkButton1_Click" runat="server">契約草稿明細預覽列印(pdf檔)</asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" CssClass="text-red" OnClick="LinkButton2_Click" runat="server">契約草稿明細預覽列印(word檔)</asp:LinkButton>
                        </div>
                        <div class="text-center">
                            <br />
                            <asp:Button ID="btnDo" CssClass="btn-default" Text="明細製作" OnClick="btnDo_Click" runat="server" />
                            <asp:Button ID="btnReturn" CssClass="btn-default" Text="回契約草稿製作畫面" OnClick="btnReturn_Click" runat="server" />
                            <asp:Button ID="btnReturnM" CssClass="btn-default" Text="回主流程畫面" OnClick="btnReturnM_Click" runat="server" />
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
