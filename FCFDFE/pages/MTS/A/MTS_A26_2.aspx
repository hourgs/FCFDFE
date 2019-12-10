<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A26_2.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A26_2" %>
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
            var theURL = '<%=ResolveClientUrl("~/pages/MTS/A/PORTLIST.aspx?OVC_SEA_OR_AIR="+drpOVC_SEA_OR_AIR.SelectedValue)%>';
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>提單資料-管理</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnMessage_Modify" runat="server"></asp:Panel>
                            <asp:Panel ID="pnDate" runat="server">
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td colspan="2">
                                    <asp:Button ID="btnPrev" cssclass="btn-success btnw4" OnClick="btnPrev_Click" runat="server" Text="上一筆" />
                                    <asp:Button ID="btnNext" cssclass="btn-success btnw4" OnClick="btnNext_Click" runat="server" Text="下一筆" />
                                    </td>
                                    <td colspan="2">
                                        目前第<asp:Label ID="lblnow" CssClass="control-label" runat="server" Text="0"></asp:Label>筆/總共
                                        <asp:Label ID="lblall" CssClass="control-label" runat="server" Text="0"></asp:Label>筆
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">提單編號</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server" Text="[bldnoLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">承運航商</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" OnSelectedIndexChanged="drpPORT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">海空運別</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SEA_OR_AIR" CssClass="tb tb-s" OnSelectedIndexChanged="drpPORT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">船機航次</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_VOYAGE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">啟運日期</asp:Label></td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_START_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">啟運港埠</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_START_PORT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">預估抵運日期</asp:Label></td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_PLN_ARRIVE_DATE" CssClass="tb tb-date" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">實際抵運日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_ACT_ARRIVE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">抵達港埠</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_PORT_CDE" CssClass="tb tb-m" hidden="true" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtOVC_CHI_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <input type="button" value="速查" class="btn-success" onclick="OpenWindowItem()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">件數</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_QUANITY" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">計量單位</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_QUANITY_UNIT" CssClass="tb tb-s" OnSelectedIndexChanged="drpOVC_QUANITY_UNIT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtOVC_QUANITY_UNIT" CssClass="tb tb-xs" Visible="false" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">體積/佔艙體積</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_VOLUME" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox> / 
                                        <asp:TextBox ID="txtONB_ON_SHIP_VOLUME" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">計量單位</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_VOLUME_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">重量</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_WEIGHT" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">計量單位</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_WEIGHT_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">物資價值</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_ITEM_VALUE" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server">幣別</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpONB_CARRIAGE_CURRENCY_I" CssClass="tb tb-s" runat="server">  
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">運費</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_CARRIAGE" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server">運費幣別</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpONB_CARRIAGE_CURRENCY" CssClass="tb tb-s" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">軍種</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server">  
                                        </asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">機敏軍品</asp:Label></td>
                                    <td class="text-left">
                                        <asp:RadioButtonList ID="rdoOVC_IS_SECURITY" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">資料傳送日期</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblODT_CREATE_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">資料修改日期</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_MODIFY_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">資料建立人員</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_CREATE_LOGIN_ID" CssClass="control-label" runat="server" Text="[datacreateLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button ID="btnModify1" cssclass="btn-warning btnw2" OnClick="btnModify1_Click" runat="server" Text="更新" />
                                        <asp:Button ID="btnDel" cssclass="btn-danger btnw2" OnClick="btnDel_Click" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" Text="刪除" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label CssClass="subtitle" runat="server" Text="完成出口作業"></asp:Label>
                                        <asp:Button cssclass="btn-warning btnw6 textSpace-l" OnClick="btnCopyInfo_Click" runat="server" Text="載入上方資訊" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">實際啟運日期</asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtREAL_START_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">實際體積</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtREAL_VOLUME" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">計量單位</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpREAL_OVC_VOLUME_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">實際重量</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtREAL_WEIGHT" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">計量單位</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpREAL_OVC_WEIGHT_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                            <div class="text-center">
                                <asp:Button ID="btnModify2" CssClass="btn-warning btnw2" Text="更新" OnClick="btnModify2_Click" runat="server"/>
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button cssclass="btn-success btnw3" OnClick="btnBack_Click" Text="回首頁" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
