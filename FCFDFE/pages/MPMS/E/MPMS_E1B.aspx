<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E1B.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E1B" %>
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
                    <!--標題-->結算驗收證明
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td  style="background-color:red;color:yellow;"><asp:Label CssClass="control-label" runat="server" Text="請注意！未輸入資料或存檔無法列印各項報表！！" Font-Size="Large"></asp:Label></td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:25%"><asp:Label CssClass="control-label" runat="server" Text="結算日期"></asp:Label></td>
                                    <td style="width:25%"><asp:Label ID="lblOVC_DPAY" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td style="width:25%"><asp:Label CssClass="control-label" runat="server" Text="結算次數"></asp:Label></td>
                                    <td style="width:25%"><asp:Label ID="lblSettlementTimes" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="工作分支計畫(預算科目)名稱及代號"></asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_PJNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtOVC_POI_IBDG" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="預算用途科目名稱及代號"></asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_IBDG_USE_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtOVC_IBDG_USE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="支出事由"></asp:Label></td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_PAY_REASON" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="標的名稱及數量摘要"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:Label ID="lblOVC_PUR_IPURCH_ENG" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="案號及合約號"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商名稱"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="計畫申購單位"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:Label ID="lblOVC_USER_FAX" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="簽約日期"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label ID="lblSignedDate" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="決標日期"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_DBID" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="契約金額"></asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="lblOVC_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtONB_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="採購金額"></asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:CheckBoxList ID="chkOVC_BUDGET_BUY" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>未達公告金額</asp:ListItem>
                                            <asp:ListItem>公告金額以上未達查核金額</asp:ListItem>
                                            <asp:ListItem>查核金額以上未達巨額</asp:ListItem>
                                            <asp:ListItem>巨額</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="履約期限"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:TextBox ID="txtOVC_PERFORMANCE_LIMIT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="履約地點"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_PERFORMANCE_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="完成履約日期"></asp:Label></td>
                                    <td colspan="2" class="text-left">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DRECEIVE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="btnResetOVC_DRECEIVE" CssClass="btn-default btnw4 position-left" OnClick="btnResetOVC_DRECEIVE_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="開始驗收日期"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DINSPECT_BEGINT" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="btnResetOVC_DINSPECT_BEGIN" CssClass="btn-default btnw4 position-left" OnClick="btnResetOVC_DINSPECT_BEGIN_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="驗收完畢/合格日期"></asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DINSPECT_END" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="btnResetOVC_DINSPECT_END" CssClass="btn-default btnw4 position-left" OnClick="btnResetOVC_DINSPECT_END_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3" colspan="2"><asp:Label CssClass="control-label" runat="server" Text="逾期違約金"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="逾期總天數"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_DELAY_DAYSY" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="逾期應計違約金天數"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_PUNISH_DAYS" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="准延天數"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_NO_PUNISH_DAYS" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="核准文號"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_NO_PUNISH_DAYS" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="逾期違約金額"></asp:Label></td>
                                    <td colspan="3" class="text-left"><asp:TextBox ID="txtONB_DELAY_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="其他違約金"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:TextBox ID="txtONB_OTHER_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td rowspan="4" class="td-vertical"><asp:Label CssClass="control-label text-vertical-m" style="height: 190px;" runat="server" Text="增減價款"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="次別"></asp:Label></td>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="第一次"></asp:Label></td>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="第二次"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="類別"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="金額"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="核准文號"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="金額"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="核准文號"></asp:Label></td>
                                </tr>
                                 <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="增加金額"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_ADD_MONEY_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_ADD_ACCORDING_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_ADD_MONEY_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_ADD_ACCORDING_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                     <asp:TextBox ID="Total1" CssClass="tb tb-s" Visible="false" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="減少金額"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_MINS_MONEY_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_MINS_ACCORDING_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_MINS_MONEY_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_MINS_ACCORDING_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <asp:TextBox ID="Total2" CssClass="tb tb-s" Visible="false" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="驗收扣款"></asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:TextBox ID="txtOVC_DPAY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Label ID="Label39" CssClass="control-label" runat="server" Text="(不包括逾期違約金及其他違約金)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="驗收金額"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:TextBox ID="txtONB_INSPECT_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="結算總價"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:TextBox ID="txtONB_PAY_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td rowspan="2" class="td-vertical"><asp:Label CssClass="control-label text-vertical-m" style="height: 190px;" runat="server" Text="驗收意見"></asp:Label></td>
                                    <td class="td-vertical"><asp:Label CssClass="control-label text-vertical-m" style="height: 190px;" runat="server" Text="驗收結果"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:TextBox ID="txtOVC_ADVICE" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="td-vertical"><asp:Label CssClass="control-label text-vertical-m" style="height: 190px;" runat="server" Text="綜合鑑定"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:TextBox ID="txtOVC_SUMMARY" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="160px"></asp:TextBox></td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                <asp:Button ID="btnSave" CssClass="btn-warning btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
                                <br><br>
                                <asp:LinkButton ID="LinkButton1" CssClass="text-red" OnClick="LinkButton1_Click" runat="server">結算驗收證明書預覽列印(簽).doc</asp:LinkButton>
                                <asp:HyperLink ID="HyperLink1" CssClass="text-red" Visible="false" runat="server">結算驗收證明書預覽列印(簽)</asp:HyperLink>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton2" CssClass="text-red" OnClick="LinkButton2_Click" runat="server">結算驗收證明書預覽列印.doc</asp:LinkButton>
                                <asp:HyperLink ID="HyperLink2" CssClass="text-red" Visible="false" runat="server">結算驗收證明書預覽列印</asp:HyperLink>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton3" CssClass="text-red" OnClick="LinkButton3_Click" runat="server">單據黏貼預覽列印.doc</asp:LinkButton>
                                <asp:HyperLink ID="HyperLink3" CssClass="text-red" Visible="false" runat="server">單據黏貼預覽列印</asp:HyperLink>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton4" CssClass="text-red" OnClick="LinkButton4_Click" runat="server">減價收簽辦單預覽列印.doc</asp:LinkButton>
                                <asp:HyperLink ID="HyperLink4" CssClass="text-red" Visible="false" runat="server">減價收簽辦單預覽列印</asp:HyperLink>
                                <br />
                                <asp:LinkButton ID="LinkButton5" CssClass="text-red" OnClick="LinkButton5_Click" runat="server">結算驗收證明書預覽列印(簽).pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton6" CssClass="text-red" OnClick="LinkButton6_Click" runat="server">結算驗收證明書預覽列印.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton7" CssClass="text-red" OnClick="LinkButton7_Click" runat="server">單據黏貼預覽列印.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton8" CssClass="text-red" OnClick="LinkButton8_Click" runat="server">減價收簽辦單預覽列印.pdf</asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="LinkButton9" CssClass="text-red" OnClick="LinkButton9_Click" runat="server">結算驗收證明書預覽列印(簽).odt</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton10" CssClass="text-red" OnClick="LinkButton10_Click" runat="server">結算驗收證明書預覽列印.odt</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton11" CssClass="text-red" OnClick="LinkButton11_Click" runat="server">單據黏貼預覽列印.odt</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton12" CssClass="text-red" OnClick="LinkButton12_Click" runat="server">減價收簽辦單預覽列印.odt</asp:LinkButton>
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
