<%@ Page Title="" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A21.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A21" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script>
        //var isChanged = true;
        //$(document).ready(function () {
        //    //$('input, textarea, select').not('.SkipChangeValidate input').not('.SkipChangeValidate').change(function () {
        //    //    isChanged = true;
        //    //});
        //    $('.untrigger').bind('click', function () {
        //        isChanged = false;
        //    }).bind('change', function () {
        //        isChanged = false;
        //    });
        //
        //});
        //
        ////沒用window.onbeforeunload = function () {
        ////    if (isChanged) {
        ////        return close();
        ////    }
        ////}
        //$(window).on('beforeunload', function () {
        //    if (isChanged) {
        //        return close();
        //    }
        //});
        //function close() {
        //    $("#<%=btnClear.ClientID%>").click();
        //    return "<%=btnClear.Text%>";
        //}

        function openWin(type) {
            //HiddenField在轉換HTML時會改變ID因此要加MainContent
            //var dept = document.getElementById("MainContent_HFdeptCode").value;
            var dept = $("#<%=HFdeptCode.ClientID%>").val();
            window.open('EDFQUERY.aspx?OVC_REQ_DEPT_CDE=' + dept + '&historyType=' + type, null, 'toolbar=0,location=0,status=0,menubar=0,width=700,height=500,left=200,top=80');
        }
        function OpenWindowItem() {
            var win_width = 600;
            var win_height = 150;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var theURL = '<%=ResolveClientUrl("~/pages/MTS/A/PORTLIST.aspx")%>';
            var newwin = window.open(theURL, 'unitQuery', features);
                }
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>
                        <asp:Label ID="depLabel" CssClass="control-label" runat="server"></asp:Label>外運資料表
                    </div>
                    <asp:HiddenField ID="HFdeptCode" runat="server" />
                    <!--隱藏欄位儲存單位代碼-->
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="pnData" runat="server">
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td colspan="6" class="text-left">
                                        <asp:Label CssClass="control-label" runat="server"  ForeColor="Red">1.外運資料表主檔載入作業說明</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 一 ) 注意：務必優先讀取 <a class="untrigger" href="外運資料表主檔載入作業說明.docx">【外運資料表主檔】載入作業說明 </a>，以便順利作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 二 ) 下載 <a class="untrigger" href="<%=ResolveClientUrl("外運資料表主檔範例.xlsx")%>">外運資料表主檔Excel 檔範例</a>。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 三 ) 按照載入作業說明規定格式編輯Excel檔。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 四 ) 按【瀏覽】選擇要載入的Excel檔。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 五 ) 按 
                                            <a href="javascript:var win=window.open('<%=ResolveClientUrl("~/pages/MTS/F/MTS_F11_2")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=1000,height=700,left=0,top=0');" >【查詢機場或港口代碼】</a>
                                            後檢查載入的資料是否正確。
                                        </asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 六 ) 按 
                                            <a href="javascript:var win=window.open('<%=ResolveClientUrl("~/Content/unitQuery") %>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=400,left=0,top=0');" >【查詢單位代碼】 </a>
                                            後檢查載入資料是否正確。
                                        </asp:Label><br /> 
                                        <asp:Label CssClass="control-label" runat="server">( 七 ) 按【讀取Excel檔內容】後檢查顯示的資料內容是否正確。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 八 ) 如果資料正確無誤請按【確認轉入外運資料表主檔資料】完成仔入作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 九 ) 完成載入外運資料表主檔資料請繼續【2.外運資料表料件明細檔載入】作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 十 ) 或直接點取選
                                            <a href="javascript:var win=window.open('MTS_A22_1','_blank');" >【外運資料管理功能】</a>
                                            繼續外運資料管理作業。
                                        </asp:Label><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="text-left">
                                         <asp:Button ID="btnUpload" cssclass="btn-success untrigger" Text="讀取外運資料表主檔內容" OnClick="btnUpload_Click" runat="server" />
                                        <asp:Label CssClass="control-label" runat="server">檔案(*.xlsx)</asp:Label>
                                    </td>
                                    <td colspan="2" class="text-left">
                                        <asp:FileUpload ID="browse" title="瀏覽" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">外運資料表</asp:Label>
                                    </td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lblOVC_EDF_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-star" runat="server">案號</asp:Label>
                                    </td>
                                    <td colspan="4" class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        <input type="button" class="btn-success btnw4" onclick="openWin('OVC_PURCH_NO');" value="歷史資料" />
                                        <%--<asp:Button ID="btnQueryOVC_PURCH_NO" CssClass="btn-success btnw4" Text="歷史資料" OnClientClick="openWin('OVC_PURCH_NO')" runat="server" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-star" runat="server">啟運港(機場)</asp:Label>
                                    </td>
                                    <td colspan="4" class="text-left">
                                        <asp:DropDownList ID="drpOVC_START_PORT" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-star" runat="server">目的港(機場)</asp:Label>
                                    </td>
<%--                                    <td colspan="4" class="text-left">
                                        <asp:TextBox ID="drpOVC_ARRIVE_PORTText" CssClass="tb tb-s" hidden="true" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtPortQuery" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnPortQuery" CssClass="btn-warning" OnClick="btnPortQuery_Click" Text="查詢" runat="server" />
                                        <asp:DropDownList ID="drpOVC_ARRIVE_PORT" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_ARRIVE_PORT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                    </td>--%>
                                    <td colspan="4" class="text-left">
                                        <asp:TextBox ID="txtOVC_PORT_CDE" CssClass="tb tb-m" hidden="true" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtOVC_CHI_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <input type="button" value="速查" class="btn-success" onclick="OpenWindowItem()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-star" runat="server">發貨單位</asp:Label>
                                    </td>
                                    <td colspan="4" class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_FROM" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                        <%--<asp:Button ID="btnQueryOVC_SHIP_FROM" CssClass="btn-success btnw4" Text="歷史資料" OnClientClick="openWin('OVC_SHIP_FROM')" runat="server" />--%>
                                        <input type="button" class="btn-success btnw4" onclick="openWin('OVC_SHIP_FROM')" value="歷史資料" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="13" class="td-vertical"><!--列數-->
                                        <div class="div-vertical">
                                            <asp:Label CssClass="control-label text-vertical-m" Style="height: 190px;" runat="server">收貨單位</asp:Label><!--高度-->
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label text-star" runat="server">CONSIGNEE</asp:Label><br>
                                        <%--<asp:Button ID="btnQueryCon" CssClass="btn-success btnw4" Text="歷史資料" OnClientClick="openWin('CONSIGNEE')" runat="server" />--%>
                                        <input type="button" class="btn-success btnw4" onclick="openWin('CONSIGNEE')" value="歷史資料" />
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_CON_ENG_ADDRESS" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_CON_TEL" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">傳真</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_CON_FAX" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">NOTIFY PARTY</asp:Label><br>
                                        <%--<asp:Button ID="btnQueryNotify" CssClass="btn-success btnw4" Text="歷史資料" OnClientClick="openWin('NOTIFYPARTY')" runat="server" />--%>
                                        <input type="button" class="btn-success btnw4" onclick="openWin('NOTIFYPARTY')" value="歷史資料" />
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_NP_ENG_ADDRESS" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_NP_TEL" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">傳真</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_NP_FAX" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">ALSO NOTIFY<br>PARTY1</asp:Label><br>
                                        <%--<asp:Button ID="btnQueryAlsoPar1" CssClass="btn-success btnw4" Text="歷史資料" OnClientClick="openWin('ALSONOTIFYPARTY1')" runat="server" />--%>
                                        <input type="button" class="btn-success btnw4" onclick="openWin('ALSONOTIFYPARTY1')" value="歷史資料" />
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_ANP_ENG_ADDRESS" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_ANP_TEL" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">傳真</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_ANP_FAX" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">ALSO NOTIFY<br>PARTY2</asp:Label><br>
                                        <%--<asp:Button ID="btnQueryAlsoPar2" CssClass="btn-success btnw4" Text="歷史資料" OnClientClick="openWin('ALSONOTIFYPARTY2')" runat="server" />--%>
                                        <input type="button" class="btn-success btnw4" onclick="openWin('ALSONOTIFYPARTY2')" value="歷史資料" />
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_ANP_ENG_ADDRESS2" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_ANP_TEL2" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">傳真</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_ANP_FAX2" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label text-star" runat="server">發貨人資訊</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">名字</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_DELIVER_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">手機</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_DELIVER_MOBILE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍線</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_DELIVER_MILITARY_LINE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-star" runat="server">付款方式</asp:Label>
                                    </td>
                                    <td colspan="4" class="text-left">
                                        <%--<asp:RadioButton ID="rdoOVC_PAYMENT_TYPE_PREPAID" Checked="true" CssClass="radioButton rb-complex" GroupName="Rdaio" Value="PREPAID" Text="PREPAID預付(軍種年度運保費項下支付)" AutoPostBack="true" OnCheckedChanged="rdoOVC_PAYMENT_TYPE_PREPAID_CheckedChanged" runat="server" />--%>
                                        <%--<asp:RadioButton ID="rdoOVC_PAYMENT_TYPE_COLLECT" CssClass="radioButton rb-complex" GroupName="Rdaio" Value="COLLECT" Text="COLLECT到付(收貨人支付)" AutoPostBack="true" OnCheckedChanged="rdoOVC_PAYMENT_TYPE_COLLECT_CheckedChanged" runat="server" />--%>
                                        <asp:RadioButtonList ID="rdoOVC_PAYMENT_TYPE" CssClass="radioButton untrigger" OnSelectedIndexChanged="rdoOVC_PAYMENT_TYPE_SelectedIndexChanged" AutoPostBack="true" RepeatLayout="UnorderedList" runat="server"></asp:RadioButtonList>
                                        <asp:RadioButtonList ID="rdoOVC_IS_PAY" CssClass="radioButton" style="margin-left: 20px;" Visible="false" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:RadioButtonList ID="rdoOVC_PAYMENT_TYPE_Other" CssClass="radioButton" style="margin-left: 60px;" Visible="false" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList><br>
                                        <asp:Label CssClass="control-label text-red" runat="server">
                                            清關作業等相關費用由受貨廠商支付(All the expense at destination <br>for taking delivery og goods is receiver's responsibility.)
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td colspan="4" class="text-left">
                                        <asp:TextBox ID="txtOVC_NOTE" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                        <%--<asp:Button ID="btnQueryOVC_NOTE" CssClass="btn-success btnw4" Text="歷史資料" OnClientClick="openWin('QueryOVC_NOTE')" runat="server" />--%>
                                        <input type="button" class="btn-success btnw4" onclick="openWin('QueryOVC_NOTE')" value="歷史資料" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" class="text-left">
                                        <asp:CheckBox ID="chkOVC_IS_STRATEGY" CssClass="radioButton untrigger" OnCheckedChanged="chkOVC_IS_STRATEGY_CheckedChanged" AutoPostBack="true" Text="戰略性高科技貨品" runat="server" />
                                        <asp:CheckBox ID="chkOVC_IS_RISK" CssClass="radioButton untrigger text-red" Text="危險品" runat="server" />
                                        <asp:CheckBox ID="chkOVC_IS_ALERTNESS" CssClass="radioButton untrigger text-red" Text="機敏性" runat="server" /><br>
                                        <asp:Panel id="pnStrategy" style="margin: 5px 15px;" Visible="false" runat="server">
                                            <asp:Label CssClass="control-label text-star" Style="padding-right: 8px;" runat="server">有效期限</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_VALIDITY_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <asp:Label CssClass="control-label text-star" Style="padding-left: 16px; padding-right: 8px;" runat="server">輸出許可證號碼</asp:Label>
                                            <asp:TextBox ID="txtOVC_LICENSE_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnUpdate" CssClass="btn-warning btnw7 untrigger" OnClick="btnUpdate_Click" Text="儲存外運資料表" runat="server" />
                            </div>
                            <br />
                            <table id="tbDETAIL" visible="false" class="table table-bordered text-center" runat="server">
                                <tr>
                                    <td colspan="6" class="text-left">
                                        <asp:Label CssClass="control-label" runat="server"  ForeColor="Red">2.外運資料表料件明細檔載入作業說明</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 一 ) 注意：務必優先讀取 
                                            <a class="untrigger" href="外運資料表料件明細檔載入作業說明.docx">【外運資料表料件明細檔】載入作業說明</a>
                                            ，以便順利作業。
                                        </asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 二 ) 下載  
                                            <a class="untrigger" href="外運資料表料件明細檔範例.xlsx">外運資料表料件明細檔Excel檔範例</a>
                                            。
                                        </asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 三 ) 按照載入作業說明規定格式編輯Excel檔。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 四 ) 按【瀏覽】選擇要載入的Excel檔。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 五 ) 按【讀取Excel檔內容】後檢查顯示的資料內容是否正確。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 六 ) 如果資料正確無誤請按【確認轉入外運資料表料件明細檔資料】完成載入作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 七 ) 完成載入外運資料表料件明細檔資料請點選
                                            <a href="javascript:var win=window.open('MTS_A22_1','_blank');" >【外運資料管理功能】</a>
                                            繼續外運資料管理作業。
                                        </asp:Label><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="text-left">
                                        <asp:Button cssclass="btn-success untrigger" runat="server" Text="讀取外運資料表料件明細檔內容" OnClick="btnUpload2_Click" />
                                        <asp:Label CssClass="control-label" runat="server">檔案(*.xlsx)</asp:Label>
                                    </td>
                                    <td colspan="2" class="text-left">
                                        <asp:FileUpload ID="browse2" title="瀏覽" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-left td-inner-table" colspan="6">
                                        <asp:GridView ID="GV_TBGMT_EDF_DETAIL" DataKeyNames="EDF_DET_SN" 
                                            CssClass="table table-striped border-top text-center table-inner" 
                                            OnRowCommand="GV_TBGMT_EDF_DETAIL_RowCommand" 
                                            OnRowDataBound="GV_TBGMT_EDF_DETAIL_RowDataBound"
                                            OnPreRender="GV_TBGMT_EDF_DETAIL_PreRender"
                                            ShowFooter="true" AutoGenerateColumns="false" runat="server">
                                            <Columns>
                                                <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" />
                                                <asp:BoundField HeaderText="英文品名" DataField="OVC_ENG_NAME" />
                                                <asp:BoundField HeaderText="中文品名" DataField="OVC_CHI_NAME" />
                                                <asp:BoundField HeaderText="料號" DataField="OVC_ITEM_NO" />
                                                <asp:BoundField HeaderText="單號" DataField="OVC_ITEM_NO2" />
                                                <asp:BoundField HeaderText="件號" DataField="OVC_ITEM_NO3" />
                                                <asp:BoundField HeaderText="數量" DataField="ONB_ITEM_COUNT" />
                                                <asp:BoundField HeaderText="單位" DataField="OVC_ITEM_COUNT_UNIT" />
                                                <asp:BoundField HeaderText="重量" DataField="ONB_WEIGHT" />
                                                <asp:BoundField HeaderText="單位" DataField="OVC_WEIGHT_UNIT" />
                                                <asp:BoundField HeaderText="容積" DataField="ONB_VOLUME" />
                                                <asp:BoundField HeaderText="單位" DataField="OVC_VOLUME_UNIT" />
                                                <%--<asp:BoundField HeaderText="體積(長X寬X高)"  />--%>
                                                <asp:TemplateField HeaderText="體積(長X寬X高)">
                                                    <ItemTemplate>
                                                        <%#Eval("ONB_LENGTH") +" x "+ Eval("ONB_WIDTH") + " x " + Eval("ONB_HEIGHT")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="金額" DataField="ONB_MONEY" />
                                                <asp:BoundField HeaderText="幣別" DataField="OVC_CURRENCY" />
                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnDel" CssClass="btn-danger btnw2 untrigger" Text="刪除" CommandName="Del" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" class="td-inner-table text-left">
                                        <div style="margin: 20px 0 10px 10px;">
                                            <asp:Button ID="btnNewMP" Visible="false" CssClass="btn-warning btnw4 untrigger" OnClick="btnNewMP_Click" Text="新增料件" runat="server" />
                                        </div>
                                        <asp:Panel ID="PnMessage_Item" runat="server"></asp:Panel>
                                        <asp:Panel ID="pnNewItem" runat="server">
                                            <table class="table table-bordered text-center table-inner">
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label text-star" runat="server">箱號</asp:Label>
                                                    </td>
                                                    <td colspan="5" class="text-left">
                                                        <asp:TextBox ID="txtOVC_BOX_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label text-star" runat="server">英文品名</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtOVC_ENG_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label text-star" runat="server">中文品名</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtOVC_ITEM_CHI_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">料號</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtOVC_ITEM_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">單號</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtOVC_ITEM_NO2" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">件號</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtOVC_ITEM_NO3" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label text-star" runat="server">數量</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtONB_ITEM_COUNT" CssClass="tb tb-s" OnTextChanged="txtONB_ITEM_COUNT_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                                        <asp:DropDownList ID="drpOVC_ITEM_COUNT_UNIT" CssClass="tb tb-s" OnSelectedIndexChanged="drpOVC_ITEM_COUNT_UNIT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                        <asp:TextBox ID="txtOVC_ITEM_COUNT_UNIT" CssClass="tb tb-xs" Visible="false" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">體積</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtONB_LENGTH" CssClass="tb tb-xs" runat="server"></asp:TextBox>X
                                                        <asp:TextBox ID="txtONB_WIDTH" CssClass="tb tb-xs" runat="server"></asp:TextBox>X
                                                        <asp:TextBox ID="txtONB_HEIGHT" CssClass="tb tb-xs" runat="server"></asp:TextBox>公分
                                                        <%--<asp:Label ID="lbAlart" CssClass="text-red" runat="server"></asp:Label>--%>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">容積</asp:Label><br>
                                                        <asp:Button CssClass="btn-success btnw5 untrigger" OnClick="convertToVolume_Click" Text="由體積換算" runat="server" />
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:Panel ID="pnVolume" runat="server"></asp:Panel>
                                                        <asp:TextBox ID="txtONB_VOLUME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        <asp:DropDownList ID="drpOVC_VOLUME_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">重量</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtONB_WEIGHT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        <asp:Label ID="lbOVC_WEIGHT_UNIT" Text="公斤" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label text-star" runat="server">單價</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtPrice" CssClass="tb tb-m" OnTextChanged="txtONB_MONEY_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label text-star" runat="server">總金額</asp:Label>
                                                    </td>
                                                    <td colspan="2" class="text-left">
                                                        <asp:TextBox ID="txtONB_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        <asp:DropDownList ID="drpOVC_CURRENCY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="text-center" style="margin: 10px;">
                                                <asp:Button CssClass="btn-warning btnw2 untrigger" OnClick="btnSave_Click" Text="確定" runat="server" />&emsp;
                                                <asp:Button CssClass="btn-default btnw2 untrigger" OnClick="btnReset_Click" Text="取消" runat="server" />
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <%--<asp:Label CssClass="control-label text-red" runat="server" Text="[itemLabel]"></asp:Label><br>--%>
                    <%--<asp:Button ID="btnNew" CssClass="btn-warning btnw7" OnClick="btnNew_Click" Text="新增外運資料表" runat="server" />--%>
                    <asp:Button ID="btnNew" visible="false" CssClass="btn-default" OnClick="btnNew_Click" Text="新建外運資料" runat="server" />
                    <asp:Button ID="btnClear" CssClass="btn-danger untrigger hidden" OnClick="btnClear_Click" Text="清除新建" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>

