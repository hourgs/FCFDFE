<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F13_8.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F13_8" %>
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
            var theURL = '<%=ResolveClientUrl("~/pages/MTS/A/PORTLIST.aspx?OVC_SEA_OR_AIR=海運")%>';
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header class="title">
                    <asp:label ID="lblTITLE" CssClass="title" runat="server">海運運費資料維護</asp:label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:300px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">承運航商</asp:Label>
                                    </td>
                                    <td style="width:700px;">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
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
                                        <asp:Label CssClass="control-label" runat="server">啟運港埠</asp:Label>
                                    </td>
                                    <td style="width:700px;">
                                        <asp:TextBox ID="txtOVC_PORT_CDE" CssClass="tb tb-m" hidden="true" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtOVC_CHI_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <input type="button" value="速查" class="btn-success" onclick="OpenWindowItem()" />
                                    </td>
                                     <td style="width:300px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">航線</asp:Label>
                                    </td>
                                    <td style="width:700px;">
                                        <asp:DropDownList ID="drpOVC_ROUTE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:300px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險開始日</asp:Label>
                                    </td>
                                    <td style="width:700px;">
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td style="width:300px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險結束日</asp:Label>
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
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">品項類別</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ITEM_CATEGORY_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">最低運費</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_LOWEST_FREIGHT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">品名(中文)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ITEM_CHI_NAME_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">品名(英文)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ITEM_ENG_NAME_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">重量(W)價格</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_WEIGHT_PRICE_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">容積(M)價格</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_VOLUME_PRICE_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
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
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">品項類別</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ITEM_CATEGORY_2" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">最低運費</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_LOWEST_FREIGHT_2" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">品名(中文)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ITEM_CHI_NAME_2" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">品名(英文)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ITEM_ENG_NAME_2" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">重量(W)價格</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_WEIGHT_PRICE_2" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">容積(M)價格</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_VOLUME_PRICE_2" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="divNew" style="text-align:center;" runat="server">
                                <asp:Button ID="btnNew" cssclass="btn-warning" runat="server" OnClick="btnNew_Click" Text="新增海運運費資料" />
                            </div>
                            <div id="divMod" style="text-align:center;" runat="server">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" OnClick="btnSave_Click" Text="更新海運運費資料" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnDel" cssclass="btn-danger" runat="server" OnClick="btnDel_Click"  OnClientClick="if (confirm('確定刪除?') == false) return false;" Text="刪除海運運費資料" /><br /><br />
                            </div>
                            <br />
                            <div style="text-align:center;">
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