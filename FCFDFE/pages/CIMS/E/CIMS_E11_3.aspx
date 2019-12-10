<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_E11_3.aspx.cs" Inherits="FCFDFE.pages.CIMS.E.CIMS_E11_3" %>
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
                    採購合約資訊
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">購案案號(1~3)</asp:Label>
                                    </td>
                                    <td style="width:18%">
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">分約號</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_PURCH_6" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_PUR_IPURCH1301" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">決標日</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_DBID" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">購辦號(5)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH_5" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">組別</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_GROUP" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">得標商編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_VEN_CST" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">得標商名稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">得標商電話</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_VEN_TEL" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">決標金額</asp:Label>
                                    </td>
                                    <td rowspan="2">
                                        <asp:Label ID="lblONB_MONEY" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">合約匯率</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_RATE" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                    <td rowspan="2" style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">決標金額(台幣)</asp:Label>
                                    </td>
                                    <td rowspan="2">
                                        <asp:Label ID="lblONB_MONEY_NTD" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">合約幣別</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CURRENT" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label " runat="server">重要事項記載</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblOVC_CONTRACT_COMM" CssClass="control-label" runat="server" Text="CH99001"></asp:Label>
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
                    開標記錄(可點選開標日期查詢廠商報價資訊)
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:GridView ID="GVTBGMT_1303" DataKeyNames="OVC_DOPEN" OnRowCommand="GVTBGMT_1303_RowCommand" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_1303_PreRender" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="項次" >
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="購案號碼" Visible="false" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="購辦號碼(5)" DataField="OVC_PURCH_5" />
                                    <asp:BoundField HeaderText="開標次數" DataField="ONB_TIMES" />
                                    <asp:TemplateField HeaderText="開標日期" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" DataFormatString="{0:d}" CommandArgument='<%# Eval("OVC_PURCH_5")%>' CommandName="btnDopen" runat="server"><%# Eval("OVC_DOPEN")%></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="開標方式" DataField="OVC_OPEN_METHOD" />
                                    <asp:BoundField HeaderText="開標結果" DataField="OVC_RESULT" />
                                    <asp:BoundField HeaderText="標次類別" DataField="OVC_BID_METHOD" />
                                    <asp:BoundField HeaderText="投標廠商家數" DataField="ONB_BID_VENDORS" />
                                    <asp:BoundField HeaderText="核定底價" DataField="ONB_BID_BUDGET" />
                                    <asp:BoundField HeaderText="決標金額" DataField="ONB_BID_RESULT" />
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
