<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D35.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D35" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
         tr td:nth-child(2) {
            text-align: left;
        }

        .no-r {
            border-right: 0px !important;
        }

        .no-l {
            border-left: 0px !important;
        }
    </style>
    <script>
        function OpenD351() {
            var win_width = 1200;
            var win_height = 1000;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var OVC_PURCH = '<%=sOVC_PURCH%>';
            var OVC_PURCH_5 = '<%=sOVC_PURCH_5%>';
            var theURL = '<%=ResolveClientUrl("MPMS_D35_1.aspx?OVC_PURCH=")%>' + OVC_PURCH + '&OVC_PURCH_5=' + OVC_PURCH_5;
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                    <h1>無法決標公告編輯</h1>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" id="divForm" style="border: solid 2px;" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <table class="table no-bordered-seesaw text-center">
                            <tr class="no-bordered-seesaw">
                                <td style="width: 40%" class="text-right no-bordered">
                                    <asp:Label CssClass="control-label" runat="server">傳輸日期 :</asp:Label></td>
                                <td class="no-bordered">
                                    <div class="input-append datepicker">
                                        <asp:TextBox ID="txtDSEND" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                        <asp:Label CssClass="control-label position-left" runat="server">(上一次傳輸日期 : </asp:Label>
                                        <asp:Label ID="lblDSEND" CssClass="control-label position-left" runat="server">)</asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table class="table table-bordered text-center" style="margin-bottom: 0px;">
                            <tr class="no-bordered">
                                <td style="width: 30%">
                                    <asp:Label CssClass="control-label" runat="server">案號</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">NC06001L074</asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td style="width: 30%">
                                    <asp:Label CssClass="control-label" runat="server">標的名稱</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server">NC06001L074</asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">招標方式</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">投標廠商家數</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtONB_BID_VENDORS" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">開標日期</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">最近一次公告日期</asp:Label></td>
                                <td>
                                    <div class='input-append datetimepicker'>
                                        <asp:TextBox ID="txtOVC_DANNOUNCE_LAST" CssClass='tb tb-m position-left' runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">無法決標理由</asp:Label></td>
                                <td class="text-left">
                                    <asp:RadioButtonList ID="rdoOVC_RESULT_REASON" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Selected="True" Value="Stream">流標</asp:ListItem>
                                        <asp:ListItem Value="Wamark">廢標</asp:ListItem>
                                        <asp:ListItem Value="No48th">依政府採購法第四十八條第一項各款不予開標</asp:ListItem>
                                        <asp:ListItem Value="No50th">依政府採購法第五十條第二項撤銷決標</asp:ListItem>
                                        <asp:ListItem Value="Cancel">取消採購</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">是否繼續辦理採購</asp:Label></td>
                                <td>
                                    <asp:RadioButtonList ID="rdoOVC_CONTINUE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Selected="True" Value="Y">是</asp:ListItem>
                                        <asp:ListItem Value="N">否</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">附加說明</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_MEMO" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table class="table no-bordered-seesaw text-center">
                            <tr class="no-bordered-seesaw">
                                <td style="width: 50%" class="text-right no-bordered">
                                    <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                <td class="no-bordered">
                                    <div class="input-append datepicker">
                                        <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn-default btnw2" runat="server" Text="存檔" />
                            <asp:Button ID="btnReturnE" OnClick="btnReturnE_Click" CssClass="btn-default" runat="server" Text="回採購開標結果作業編輯畫面" />
                            <asp:Button ID="btnReturnS" OnClick="btnReturnS_Click" CssClass="btn-default " runat="server" Text="回採購開標結果無法公告選擇畫面" />
                            <asp:Button ID="btnReturnM" OnClick="btnReturnM_Click" CssClass="btn-default btnw6" runat="server" Text="回主流程畫面" />
                            <br />
                            <br />
                            <asp:LinkButton ID="LinkButton1" CssClass="text-red" OnClientClick="OpenD351()" runat="server">無法決標公告稿預覽列印</asp:LinkButton>
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
