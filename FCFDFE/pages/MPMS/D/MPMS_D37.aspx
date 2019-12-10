<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D37.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D37" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script>
        function OpenD371() {
            var win_width = 1200;
            var win_height = 1000;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var OVC_PURCH = '<%=sOVC_PURCH%>';
            var ONB_GROUP = '<%=sONB_GROUP%>';
            var theURL = '<%=ResolveClientUrl("MPMS_D37_1.aspx?OVC_PURCH=")%>' + OVC_PURCH + '&ONB_GROUP=' + ONB_GROUP;
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>
    <script>
        function OpenD372() {
            var win_width = 800;
            var win_height = 500;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var OVC_PURCH = '<%=ssOVC_PURCH%>';
            var OVC_PURCH_5 = '<%=ssOVC_PURCH_5%>';
            var theURL = '<%=ResolveClientUrl("MPMS_D37_2.aspx?OVC_PURCH=")%>' + OVC_PURCH + '&OVC_PURCH_5=' + OVC_PURCH_5;
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>
    <style>
        .tr1 td:nth-child(2n+2) {
            text-align: left;
        }

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
                    契約草稿製作          
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="subtitle text-red">
                            購案契約編號：<asp:Label ID="lblInfo_PurchNo" CssClass="control-label" runat="server"></asp:Label>
                            合約商：<asp:Label ID="lblInfo_Contractor" CssClass="control-label" runat="server"></asp:Label>
                            組別：<asp:Label ID="lblInfo_Group" CssClass="control-label" runat="server"></asp:Label>
                        </div>
                        <table class="table table-bordered text-center tr1">
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">契約編號</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_PURCH_6" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    組別
                                    <asp:TextBox ID="txtONB_GROUP" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">名稱</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">預算金額</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpONB_GROUP_BUDGET" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="值">N.新台幣</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtONB_PUR_BUDGET" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    元整
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">決標日期</asp:Label>
                                </td>
                                <td style="width: 45%">
                                    <div class="input-append datepicker">
                                        <asp:TextBox ID="txtOVC_DBID" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div>
                                    <asp:Label CssClass="control-label position-left" runat="server">開標日期：</asp:Label>
                                    <div class="input-append datepicker">
                                        <asp:TextBox ID="txtOVC_DOPEN" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div>
                                </td>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">簽約日期</asp:Label>
                                </td>
                                <td>
                                    <div class="input-append datepicker">
                                        <asp:TextBox ID="txtOVC_DCONTRACT" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">契約金額</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpONB_MCONTRACT" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="值">N.新台幣</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtONB_MCONTRACT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    元整
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">折讓金額</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtONB_MONEY_DISCOUNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">交貨地點</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_RECEIVE_PLACE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">交貨時間</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_SHIP_TIMES" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">付款方式</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_PAYMENT" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">交貨批次</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtONB_DELIVERY_TIMES" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 12%">
                                    <asp:Label CssClass="control-label" runat="server">廠商統一編號</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_VEN_CST" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    <asp:LinkButton ID="LinkButton2" CssClass="text-red" OnClientClick="OpenD372()" runat="server">查詢投標廠商資料</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">廠商名稱</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">廠商傳真</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_VEN_FAX" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">廠商EMAIL</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_VEN_EMAIL" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">負責人</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_VEN_BOSS" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">廠商電話</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_VEN_TEL" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">聯絡人</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_VEN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">連絡人手機</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_VEN_CELLPHONE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">廠商地址</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtOVC_VEN_ADDRESS" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">重要事項</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtOVC_CONTRACT_COMM" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>


                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-default btn2 " OnClick="btnSave_Click" runat="server" Text="存檔" />
                            <asp:Button ID="btnRefresh" CssClass="btn-default " OnClick="btnRefresh_Click" runat="server" Text="重新讀取開標級開標結果呈核資料" />
                            <asp:Button ID="btnDetail" CssClass="btn-default " OnClick="btnDetail_Click" runat="server" Text="明細製作" />
                            <asp:Button ID="btnPrice" CssClass="btn-default " OnClick="btnPrice_Click" runat="server" Text="單價製作" />
                            <asp:LinkButton ID="LinkButton1" CssClass="text-red" OnClientClick="OpenD371()" runat="server">契約草稿預覽列印</asp:LinkButton>
                        </div>
                        <br />
                        <div class="text-center">
                            <asp:Button ID="btnReturnS" OnClick="btnReturnS_Click" CssClass="btn-default" runat="server" Text="回契約製作選擇畫面" />
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
