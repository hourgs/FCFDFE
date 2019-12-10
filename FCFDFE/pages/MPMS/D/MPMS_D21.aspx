<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D21.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D21" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->契約草稿製作
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table no-bordered-seesaw text-center">
                                <tr class="no-bordered-seesaw">
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" Text="購案契約編號：" Font-Size="Large"></asp:Label>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server" Font-Size="Large"></asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" Text="合約商：" Font-Size="Large"></asp:Label>
                                        <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server" Font-Size="Large"></asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" Text="組別：" Font-Size="Large"></asp:Label>
                                        <asp:Label ID="lblONB_GROUP" CssClass="control-label" runat="server" Font-Size="Large"></asp:Label><br>
                                    </td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="購案編號"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_PURCH_1" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="契約編號"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH_6" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" Text="組別："></asp:Label>
                                        <asp:TextBox ID="txtONB_GROUP" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="名稱"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="預算金額"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_BUD_CURRENT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtONB_BUD_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="元整"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="決標日期"></asp:Label></td>
                                    <td class="text-left">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="TextBox1" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="簽約日期"></asp:Label></td>
                                    <td class="text-left">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DBID" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="開標日期"></asp:Label></td>
                                    <td class="text-left">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DOPEN" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="折讓金額"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_MONEY_DISCOUNT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="契約金額"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_CURRENT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtONB_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="元整"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="交貨時間"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_SHIP_TIMES" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="交貨地點"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_RECEIVE_PLACE" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="交貨批次"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_DELIVERY_TIMES" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="付款方式"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_PAYMENT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商名稱"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商統一編號"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_VEN_CST" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:HyperLink ID="InkTBM1313" runat="server">查詢投標商資料</asp:HyperLink>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商EMAIL"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_EMAIL" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商傳真"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_FAX" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商電話"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_TEL" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="負責人"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_BOSS" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="聯絡人手機"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_CELLPHONE" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="聯絡人"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td></td>
                                    <td class="text-left"></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商地址"></asp:Label></td>
                                    <td class="text-left" colspan="3"><asp:TextBox ID="txtOVC_VEN_ADDRESS" CssClass="tb tb-full" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="重要事項"></asp:Label></td>
                                    <td class="text-left" colspan="3"><asp:TextBox ID="txtOVC_CONTRACT_COMM" CssClass="tb tb-full" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                            <table class="table no-bordered-seesaw text-center">
                                        <tr class="no-bordered-seesaw">
                                            <td style="width:25%" class="no-bordered"></td>
                                            <td class="no-bordered">
                                                <asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="存檔" />
                                                <asp:Button visible="false" ID="btnReRead" CssClass="btn-success" runat="server" Text="重新讀取開標結果呈核資料" />
                                                <asp:Button visible="false" ID="btnDetails" CssClass="btn-success btnw4" runat="server" Text="明細製作" />
                                                <asp:Button visible="false" ID="btnUnitPrice" CssClass="btn-success btnw4" runat="server" Text="單價製作" />
                                            </td>
                                            <td style="width:25%" class="text-right no-bordered"><asp:HyperLink ID="InkPreview" runat="server">契約草稿預覽列印</asp:HyperLink></td>
                                        </tr>
                                    </table>
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
