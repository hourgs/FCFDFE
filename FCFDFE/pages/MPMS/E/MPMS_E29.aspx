<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E29.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E29" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        tr td:nth-child(2n) {
            text-align: left;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    交貨暨驗收情形
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->

                            <table class="table table-bordered text-center">
                                <tr>
                                    <th colspan="4" style="background: red;">
                                        <asp:Label ForeColor="#ffff37" Font-Size="X-Large" CssClass="control-label" runat="server">請注意！未輸入資料或存檔無法列印各項報表！！</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">批次 </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_SHIP_TIMES" CssClass="tb text-red tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-red" runat="server">複驗次數</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpONB_INSPECT_TIMES" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td>
                                        <asp:Label CssClass="control-label text-red" runat="server">再驗次數 </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpONB_RE_INSPECT_TIMES" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計畫申購單位</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_PUR_AGENCY" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">得標商</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">契約交貨日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DAUDIT" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">實際交貨日期</asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DELIVERY" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="btnC_OVC_DELIVERY" CssClass="btn-default btnw4" OnClick="btnC_OVC_DELIVERY_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">交貨地點</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DELIVERY_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                            <asp:Label CssClass="control-label" runat="server">本次交貨金額</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtONB_MDELIVERY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="letter-spacing: 20px;" colspan="4"><asp:Label CssClass="control-label text-red" Font-Size="X-Large" Font-Bold="true" runat="server">驗收結果</asp:Label></th>
                                </tr>
                                <tr>
                                    <td style="display:none"></td>
                                    <td colspan="4">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:Label CssClass="control-label" runat="server">一、品名數量：</asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="chkOVC_RESULT_1_1" CssClass="radioButton text-blue" RepeatLayout="UnorderedList" OnSelectedIndexChanged="chkOVC_RESULT_1_1_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                    <asp:ListItem Value="值">本次採購品項，經會同各有關單位代表清點大數相符，細數由接收單位會同承商負責點收入庫。</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <br />
                                                <asp:TextBox ID="txtOVC_RESULT_1_1" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display:none"></td>
                                    <td colspan="4">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:Label CssClass="control-label" runat="server">二、抽驗情形：</asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="chkOVC_RESULT_2" CssClass="radioButton text-blue" OnSelectedIndexChanged="chkOVC_RESULT_2_SelectedIndexChanged" AutoPostBack="true" RepeatLayout="UnorderedList" runat="server">
                                                    <asp:ListItem Value="值">本日會同會驗人員依約實施目視清點檢查，經審視承商所交標的外觀完整無破損、數量均與契約相符，並經委購單位確認為無誤。</asp:ListItem>
                                                    <asp:ListItem Value="值">承商依約檢附契約規定所列各項文件，經委方確認相符。</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <br />
                                                <asp:TextBox ID="txtOVC_RESULT_2" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display:none"></td>
                                    <td colspan="4">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:Label CssClass="control-label" runat="server">三、包裝情形：</asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="chkOVC_RESULT_3" CssClass="radioButton text-blue" OnSelectedIndexChanged="chkOVC_RESULT_3_SelectedIndexChanged" AutoPostBack="true" RepeatLayout="UnorderedList" runat="server">
                                                    <asp:ListItem Value="值">包裝情形良好無破損。</asp:ListItem>
                                                    <asp:ListItem Value="值">符合契約包裝規定。</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <br />
                                                <asp:TextBox ID="txtOVC_RESULT_3" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display:none"></td>
                                    <td colspan="4">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:Label CssClass="control-label" runat="server">四、逾期天數及罰鍰：</asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="chkOVC_RESULT_4" CssClass="radioButton text-blue" OnSelectedIndexChanged="chkOVC_RESULT_4_SelectedIndexChanged" AutoPostBack="true" RepeatLayout="UnorderedList" runat="server">
                                                    <asp:ListItem Value="值">自 至 共計逾期 天，每日以契約總價千分之一計罰共計新台幣 萬 元整(計算式：契約總價x逾期天數x罰則=逾期計罰金額)。</asp:ListItem>
                                                    <asp:ListItem Value="值">未逾契約交貨日期</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <br />
                                                <asp:TextBox ID="txtOVC_RESULT_4" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display:none"></td>
                                    <td colspan="4">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:Label CssClass="control-label" runat="server">五、封樣及檢驗單位：</asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="chkOVC_RESULT_5" CssClass="radioButton text-blue" OnSelectedIndexChanged="chkOVC_RESULT_5_SelectedIndexChanged" AutoPostBack="true" RepeatLayout="UnorderedList" runat="server">
                                                    <asp:ListItem Value="值">契約未規定</asp:ListItem>
                                                    <asp:ListItem Value="值">經查承商已備齊半成品，故依據契約規定抽取第 項半成品樣本各三份(抽樣項目及數量詳如清單)。所抽樣本三份，當場會同有關人員簽封，乙份送 依規定實施檢驗，另兩份留存接收單位備驗。</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <br />
                                                <asp:TextBox ID="TextBox4" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display:none"></td>
                                    <td colspan="4">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:Label CssClass="control-label" runat="server">六、其他：</asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="chkOVC_RESULT_6" CssClass="radioButton text-blue" OnSelectedIndexChanged="chkOVC_RESULT_6_SelectedIndexChanged" AutoPostBack="true" RepeatLayout="UnorderedList" runat="server">
                                                    <asp:ListItem Value="值">請使用單位依契約規定實施性能測試，並於完成後出具性能測試報告由委方彙整後函復本中心憑辦。</asp:ListItem>
                                                    <asp:ListItem Value="值">依契約規定實施抽樣後交使用單位實施性能測試，並於完成後出具性能測試函復本中心憑辦。</asp:ListItem>
                                                    <asp:ListItem Value="值">請承商於性能測試合格並經本中心通知後 日曆天內，將採購品項分送至各指定地點，如有逾期依約計罰，俟各接收單位完成接收後請委方復知本中心憑辦</asp:ListItem>
                                                    <asp:ListItem Value="值">本次驗收緊對抽驗部分負責，為抽驗部分如發現品質規格或契約不符或數量短少事概由承商負責調換合格新品或補足之</asp:ListItem>
                                                    <asp:ListItem Value="值">本採購接收暨會驗結果報告單位於驗收時即分送各參與會驗人員，不另行文分發。</asp:ListItem>
                                                    <asp:ListItem Value="值">因檢驗單位與存樣單位相鄰，奉核委請存樣單位送驗。委託送驗前，已請技術代表確認品項數量無誤，包裝(防震)不影響品質，以明責任</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <br />
                                                <asp:TextBox ID="txtOVC_RESULT_6" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display:none"></td>
                                    <td colspan="4"><asp:Label CssClass="control-label text-red" runat="server">驗收意見：</asp:Label>
                                        <asp:TextBox ID="txtOVC_RESULT_SUMMARY" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                            <div class="text-center">
                                        <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                        <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                        <asp:Button ID="btnSave" CssClass="btn-default btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
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
