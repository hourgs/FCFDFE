<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F13_4.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F13_4" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script>
        //港埠查詢視窗function 設定位置、大小等
        function OpenWindowItem() {
            var win_width = 600;
            var win_height = 150;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var theURL = '<%=ResolveClientUrl("~/pages/MTS/A/PORTLIST.aspx?OVC_SEA_OR_AIR=空運")%>';
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    空運運費資料維護-新增
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table id="tbAIR" class="table table-bordered" runat="server">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">承運航商</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:300px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">啟運機場</asp:Label>
                                    </td>
                                    <td style="width:700px;">
                                        <asp:TextBox ID="txtOVC_PORT_CDE" CssClass="tb tb-m" hidden="true" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtOVC_CHI_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <input type="button" value="速查" class="btn-success" onclick="OpenWindowItem()" />
                                    </td>
                                     <td style="width:300px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">幣別</asp:Label>
                                    </td>
                                    <td style="width:700px;">
                                        <asp:DropDownList ID="drpONB_CARRIAGE_CURRENCY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:300px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">合約開始日</asp:Label>
                                    </td>
                                    <td style="width:700px;">
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td style="width:300px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">合約結束日</asp:Label>
                                    </td>
                                    <td style="width:700px;">
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtEndDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <asp:Label runat="server">進口/出口：</asp:Label>
                                        <asp:DropDownList ID="drpOVC_IMPORT_EXPORT_1" Width="80px" runat="server">
                                            <asp:ListItem>進口</asp:ListItem>
                                            <asp:ListItem>出口</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label Width="80px" runat="server"></asp:Label>
                                        <asp:Label runat="server">折扣數：</asp:Label>
                                        <asp:TextBox ID="txtONB_DISCOUNT_1" Height="20px" Width="100px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">M-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_M_RATE_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As N-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_N_RATE_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">N-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_N_RATE_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 45 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_45_KG_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+45 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_45_KG_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 100 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_100_KG_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+100 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_100_KG_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 300 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_300_KG_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+300 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_300_KG_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 500 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_500_KG_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+500 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_500_KG_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label CssClass="control-label text-center" runat="server">歐洲線危險品收費：提單</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_EUR_BLD_QUANTITY_1" OnTextChanged="txtONB_DANGER_PRO_COST_TextChanged" AutoPostBack="true" CssClass="tb tb-xs" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label CssClass="control-label" runat="server">X&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;危險品收費</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_EUR_DANGER_PRO_COST_1" OnTextChanged="txtONB_DANGER_PRO_COST_TextChanged" AutoPostBack="true" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="control-label" runat="server">%&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;=</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_EUR_FIN_DANGER_PRO_COST_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label CssClass="control-label text-center" runat="server">美國線危險品收費：提單</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_USA_BLD_QUANTITY_1" OnTextChanged="txtONB_DANGER_PRO_COST_TextChanged" AutoPostBack="true" CssClass="tb tb-xs" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label CssClass="control-label" runat="server">X&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;危險品收費</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_USA_DANGER_PRO_COST_1" OnTextChanged="txtONB_DANGER_PRO_COST_TextChanged" AutoPostBack="true" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="control-label" runat="server">%&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;=</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_USA_FIN_DANGER_PRO_COST_1" CssClass="tb tb-xs" runat="server"></asp:TextBox>

                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <asp:Label runat="server">進口/出口：</asp:Label>
                                        <asp:DropDownList ID="drpOVC_IMPORT_EXPORT_2" Width="80px" runat="server">
                                            <asp:ListItem>進口</asp:ListItem>
                                            <asp:ListItem>出口</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label Width="80px" runat="server"></asp:Label>
                                        <asp:Label runat="server">折扣數：</asp:Label>
                                        <asp:TextBox ID="txtONB_DISCOUNT_2" Height="20px" Width="100px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">M-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_M_RATE_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As N-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_N_RATE_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">N-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_N_RATE_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 45 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_45_KG_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+45 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_45_KG_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 100 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_100_KG_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+100 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_100_KG_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 300 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_300_KG_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+300 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_300_KG_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 500 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_500_KG_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+500 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_500_KG_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label CssClass="control-label text-center" runat="server">歐洲線危險品收費：提單</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_EUR_BLD_QUANTITY_2" OnTextChanged="txtONB_DANGER_PRO_COST_TextChanged" AutoPostBack="true" CssClass="tb tb-xs" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label CssClass="control-label" runat="server">X&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;危險品收費</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_EUR_DANGER_PRO_COST_2" OnTextChanged="txtONB_DANGER_PRO_COST_TextChanged" AutoPostBack="true" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="control-label" runat="server">%&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;=</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_EUR_FIN_DANGER_PRO_COST_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label CssClass="control-label text-center" runat="server">美國線危險品收費：提單</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_USA_BLD_QUANTITY_2" OnTextChanged="txtONB_DANGER_PRO_COST_TextChanged" AutoPostBack="true" CssClass="tb tb-xs" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label CssClass="control-label" runat="server">X&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;危險品收費</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_USA_DANGER_PRO_COST_2" OnTextChanged="txtONB_DANGER_PRO_COST_TextChanged" AutoPostBack="true" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="control-label" runat="server">%&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;=</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtONB_USA_FIN_DANGER_PRO_COST_2" CssClass="tb tb-xs" runat="server"></asp:TextBox>

                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <asp:Label runat="server">進口/出口：</asp:Label>
                                        <asp:DropDownList ID="drpOVC_IMPORT_EXPORT_3" Width="80px" runat="server">
                                            <asp:ListItem>進口</asp:ListItem>
                                            <asp:ListItem>出口</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label Width="80px" runat="server"></asp:Label>
                                        <asp:Label runat="server">折扣數：</asp:Label>
                                        <asp:TextBox ID="txtONB_DISCOUNT_3" Height="20px" Width="100px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">M-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_M_RATE_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As N-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_N_RATE_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">N-Rate</asp:Label>
                                                    <asp:TextBox ID="txtONB_N_RATE_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 45 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_45_KG_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+45 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_45_KG_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 100 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_100_KG_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+100 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_100_KG_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 300 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_300_KG_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+300 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_300_KG_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">As 500 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_AS_500_KG_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">+500 KG</asp:Label>
                                                    <asp:TextBox ID="txtONB_PLUS_500_KG_3" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">備考：</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOVC_REMARK" CssClass="tb tb-full" Height="70px" Width="700px" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" OnClick="btnSave_Click" Text="新增空運運費資料" /><br /><br />
                                <asp:Button ID="btnHome" cssclass="btn-success" runat="server" OnClick="btnHome_Click" Text="回首頁" /><br />
                            </div>
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