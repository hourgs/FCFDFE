<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_E11_1.aspx.cs" Inherits="FCFDFE.pages.CIMS.E.CIMS_E11_1" %>

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
                    購案基本資訊
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">購案案號(1~3)</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">採購單位地區(4)</asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="lblOVC_AGNT_IN" CssClass="control-label" runat="server" Text="L:國防部國防採購室(內購)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server" Text="Unix資料庫伺服器機組維護等35項"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">申購單位</asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="lblOVC_PUR_SECTION" CssClass="control-label" runat="server" Text="03V00:國防部通信電子資訊參謀次長室"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">採購方式</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_PURCH_KIND" CssClass="control-label" runat="server" Text="1:內購案"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">申購人</asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label" runat="server" Text="鍾博丞"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">電話(軍線)</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblOVC_PUR_IUSER_PHONE_EXT" CssClass="control-label" runat="server" Text="255840"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">電話(自動)</asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="lblOVC_PUR_IUSER_PHONE" CssClass="control-label" runat="server" Text="256513"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">招標</asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width: 10%">
                                        <asp:Label CssClass="control-label" runat="server" Text="招標方式"></asp:Label>
                                    </td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server" Text="A.公開招標"></asp:Label>
                                    </td>
                                    <td rowspan="2" style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">預算總額</asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server" Text="幣別"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CURRENT" CssClass="control-label" runat="server" Text="N:新台幣"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server" Text="匯率"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_RATE" CssClass="control-label" runat="server" Text="1.0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server" Text="決標原則"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_BID" CssClass="control-label" runat="server" Text="1.訂底價,最低標"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server" Text="金額"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_PUR_BUDGET" CssClass="control-label" runat="server" Text="21310261"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server" Text="台幣金額"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_PUR_BUDGT_NTD" CssClass="control-label" runat="server" Text="121310261"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server" Text="物資核定書"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label21" CssClass="control-label" runat="server" Text="顯示內容"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server" Text="採購計劃清單"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label26" CssClass="control-label" runat="server" Text="顯示內容"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server" Text="底價表"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFile" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnBack" OnClick="btnBack_Click" CssClass="btn-warning btnw2" runat="server" Text="返回" /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <header class="title">
                    合約資訊(可點選分約號查詢合約基本資訊)
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:GridView ID="GVTBGMT_1302" DataKeyNames="OVC_PURCH_6" OnRowCommand="GVTBGMT_1302_RowCommand" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_1302_PreRender" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="項次" >
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="分約號(6)" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" CommandName="btnPurch6" runat="server"><%# Eval("OVC_PURCH_6")%></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="分約號(6)" DataField="OVC_PURCH_6" />
                                    <asp:BoundField HeaderText="購辦號碼(5)" DataField="OVC_PURCH_5" />
                                    <asp:BoundField HeaderText="得標商編號" DataField="OVC_VEN_CST" />
                                    <asp:BoundField HeaderText="廠商名稱" DataField="OVC_VEN_TITLE" />
                                    <asp:BoundField HeaderText="決標日" DataField="OVC_DBID" DataFormatString="{0:d}"/>
                                    <asp:BoundField HeaderText="合約幣別" DataField="OVC_CURRENT" />
                                    <asp:BoundField HeaderText="決標金額" DataField="ONB_MONEY" />
                                    <asp:BoundField HeaderText="台幣金額" DataField="ONB_MONEY_NTD" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <header class="title">
                    採購品項(可點選項次查詢採購品項基本資訊)
                </header>
                <asp:Panel ID="Panel2" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:GridView ID="GVTBGMT_1201" DataKeyNames="ONB_POI_ICOUNT" OnRowCommand="GVTBGMT_1201_RowCommand" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_1201_PreRender" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="項次" >
                                        <ItemTemplate>
                                            <asp:LinkButton  CommandName="btnPoiIcount" runat="server"><%# Eval("ONB_POI_ICOUNT")%></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="中文品名" DataField="OVC_POI_NSTUFF_CHN" />
                                    <asp:BoundField HeaderText="料號" DataField="NSN" />
                                    <asp:BoundField HeaderText="件號" DataField="OVC_POI_IREF" />
                                    <asp:BoundField HeaderText="申購數量" DataField="ONB_POI_QORDER_PLAN" />
                                    <asp:BoundField HeaderText="合約數量" DataField="ONB_POI_QORDER_CONT" />
                                    <asp:BoundField HeaderText="預算單價" DataField="ONB_POI_MPRICE_PLAN" />
                                    <asp:BoundField HeaderText="合約單價" DataField="ONB_POI_MPRICE_CONT" />
                                </Columns>
                            </asp:GridView>
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

