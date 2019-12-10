<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D18_7.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D18_7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <asp:Panel ID="PnMessage_Top" runat="server"></asp:Panel>
        <div style="width: 1000px; margin: auto;" id="divForm" visible="false" runat="server">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    採購開標結果作業編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="購案編號"></asp:Label></td>
                                    <td class="text-left" style="width: 32%">
                                        <asp:Label ID="lblOVC_PURCH2" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="標的名稱"></asp:Label></td>
                                    <td style="width: 48%" class="text-left">
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label position-left" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="申購單位"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="開標時間"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_DOPEN_tw" CssClass="control-label position-left" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_DOPEN" CssClass="control-label position-left" Visible="false" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_OPEN_HOUR" CssClass="control-label position-left" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_OPEN_MIN" CssClass="control-label position-left" runat="server"></asp:Label>
                                        <asp:Panel ID="PanelDBID" Visible="false" runat="server">
                                            <asp:Label CssClass="control-label position-left" runat="server">　　決標日：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                            <!--↓日期套件↓-->
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtOVC_DBID" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="採購金額"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_CURRENT_1" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_BUDGET_BUY" CssClass="control-label text-red" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="(預算金額+後續擴充金額)"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="預算金額"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_CURRENT_2" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtONB_PUR_BUDGET" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="得標商"></asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_VEN_TITLE" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpOVC_VEN_TITLE_SelectedIndexChanged" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="決標金額"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_RESULT_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_RESULT_CURRENT_id" CssClass="control-label" Visible="false" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtONB_BID_RESULT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="底價金額"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtONB_BID_BUDGET" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="標餘款"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_REMAIN_BUDGET" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="開標次數"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">第</asp:Label>
                                        <asp:Label ID="lblONB_TIMES" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">次開標</asp:Label>
                                        <asp:Label ID="lblONB_GROUP" CssClass="control-label" Visible="false" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="說明"></asp:Label></td>
                                    <td class="text-left" colspan="3">
                                        <asp:TextBox ID="txtOVC_DESC" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="90px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="text-left" colspan="4">
                                        <asp:Label CssClass="control-label" runat="server" Text="辦理情形："></asp:Label>
                                        <asp:TextBox ID="txtOVC_MEETING" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="90px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-left" colspan="4">
                                        <p><asp:Label CssClass="control-label" runat="server" Text="擬辦："></asp:Label></p>
                                        <asp:CheckBoxList ID="chkOVC_DRAFT" CssClass="radioButton" RepeatDirection="Vertical" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>附呈契約草稿，洽得標廠商辦理簽約事宜。</asp:ListItem>
                                            <asp:ListItem>檢附開標紀錄、正本核對表、基本資料表、聲明書、投標須知（以上均影本）函送申購單位辦理訂約。</asp:ListItem>
                                            <asp:ListItem>本案辦理保留決標，俟預算奉核後辦理決標、簽約事宜；如預算未奉核定，則依政府採購法第48條第1項第7款規定不予決標。</asp:ListItem>
                                            <asp:ListItem>刊登決標公告事宜。</asp:ListItem>
                                            <asp:ListItem>函請申購單位檢討澄清後重新委辦並刊登無法決標公告。</asp:ListItem>
                                            <asp:ListItem>本案已多次流、廢標，除續辦招標外擬依「國軍採購作業規定」第133點，函請申購單位併行檢討相關事項。</asp:ListItem>
                                            <asp:ListItem>上傳招標公告，並排訂於  年  月  日（星期  ）傳輸公告、  年  月  日（星期  ）刊登公報、  年  月  日（星期  ）上午    時截標、  年  月  日（星期  ）下午  時開標；依「招標期限標準」第八條規定，第2次招標之等標期得縮短為7天，另依「政府採購法」第四十八條第二項及「機關辦理採購之廠商家數一覽表」規定，第2次招標不受3家廠商限制（第2次開標主持人：      ）。</asp:ListItem>
                                            <asp:ListItem>通知未得標廠商決標及審查結果。</asp:ListItem>
                                            <asp:ListItem>俟申購單位完成評選作業後，函覆本室憑辦後續作業。</asp:ListItem>
                                            <asp:ListItem>其他: (內容由各承參自訂)如:評選委員會以書面審查進行評分，廠商實施簡報。符合招標文件規定之廠商出席評選委員會備詢，評選時間及地點由人事參謀次長室以書面另行通知。</asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:TextBox ID="txtOVC_DRAFT" Visible="false" CssClass="tb tb-full" TextMode="MultiLine" Height="90px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table no-bordered-seesaw">
                                <tr class="no-bordered-seesaw">
                                    <td style="width: 50%" class="text-right no-bordered">
                                        <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>


                            <div style="text-align: center">
                                <asp:Button ID="btnToDefault" OnClick="btnToDefault_Click" CssClass="btn-default" Text="回開標紀錄選擇畫面" runat="server" />
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn-default btnw2" Text="存檔" runat="server" />
                                <asp:Button ID="btnReRead" OnClick="btnReRead_Click" CssClass="btn-default" Text="重新讀取開標紀錄說明" runat="server" />
                                <asp:LinkButton ID="lbtnToWord_D1B" OnClick="lbtnToWord_D1B_Click" runat="server">開標結果報告表預覽列印.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnToWord_D1B_odt" OnClick="lbtnToWord_D1B_odt_Click" runat="server">開標結果報告表預覽列印.odt</asp:LinkButton>
                                <br/><br/>

                                <asp:Button ID="btnOVC_RESULT_REASON" OnClick="btnOVC_RESULT_REASON_Click" CssClass="btn-default" Text="開標結果通知作業" runat="server" />
                                <asp:Button ID="btnTBMANNOUNCE_VENDOR" OnClick="btnTBMANNOUNCE_VENDOR_Click" CssClass="btn-default" Text="決標公告作業" runat="server" />
                                <asp:Button ID="btnTBMRESULT_ANNOUNCE_NONE" OnClick="btnTBMRESULT_ANNOUNCE_NONE_Click" CssClass="btn-default" Text="無法決標公告作業" runat="server" /><br />
                            </div>
                            <div>
                                <asp:Label CssClass="control-label text-blue" runat="server" Text="開標日期"></asp:Label>
                                <asp:Label ID="lblOVC_DOPEN_1" CssClass="control-label text-blue" runat="server"></asp:Label>
                                <asp:Label ID="lblOVC_OPEN_HOUR_1" CssClass="control-label text-blue" runat="server"></asp:Label>
                                <asp:Label ID="lblOVC_OPEN_MIN_1" CssClass="control-label text-blue" runat="server"></asp:Label>
                                <asp:Label CssClass="control-label text-blue" runat="server" Text="之得標商(決標金以契約之合約金額為主)"></asp:Label>
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="購案編號(組別)"></asp:Label></td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="契約編號"></asp:Label></td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="開標日期"></asp:Label></td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="決標日期"></asp:Label></td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="得標廠商"></asp:Label></td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商編號"></asp:Label></td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="決標金額"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblOVC_PURCH_6" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblOVC_DOPEN_2" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblOVC_DBID" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblOVC_VEN_CST" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblONB_BID_RESULT" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                </table>
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
