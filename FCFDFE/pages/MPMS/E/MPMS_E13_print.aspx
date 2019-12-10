<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E13_print.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E13_print" %>
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
                    <!--標題-->國防部國防採購室購案契約移轉履約驗結處檢查項目表
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="control-label" runat="server" Text="購案編號：" Font-Size="Large"></asp:Label>
                                <asp:Label ID="Label1" CssClass="control-label" runat="server" Text=""></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label CssClass="control-label" runat="server" Text="品名：" Font-Size="Large"></asp:Label>
                                <asp:Label ID="Label2" CssClass="control-label" runat="server" Text=""></asp:Label>
                                <p></p>
                            </div>
                            <table id="tbItem_1" class="table table-bordered text-center" runat="server">
                                <tr>
                                    <td rowspan="32" class="td-vertical" style="background-color:forestgreen;"><asp:Label CssClass="text-vertical-s" style="height: 190px;" runat="server" ForeColor="White">驗收官檢查項目</asp:Label></td>
                                    <td style="width:100px; background-color:darkblue;"><asp:Label CssClass="control-label" runat="server" ForeColor="White">項次</asp:Label></td>
                                    <td style="background-color:darkblue;"><asp:Label CssClass="control-label" ForeColor="White" runat="server">驗&nbsp;&nbsp;&nbsp;收&nbsp;&nbsp;&nbsp;官&nbsp;&nbsp;&nbsp;檢&nbsp;&nbsp;&nbsp;查&nbsp;&nbsp;&nbsp;項&nbsp;&nbsp;&nbsp;目</asp:Label></td>
                                    <td style="width:120px; background-color:darkblue;"><asp:Label CssClass="control-label" ForeColor="Red" runat="server">是否</asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td rowspan="8"><asp:Label CssClass="control-label" Text="一" runat="server"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="labItem1_1" CssClass="control-label" Text="份數是否足夠？內：正一副三　外：正一副一 　下授:正一副一" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck1_1" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem1_2" CssClass="control-label" Text="有無編訂目錄頁次號？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck1_2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem1_3" CssClass="control-label" Text="塗改處是否有蓋校正章？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck1_3" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem1_4" CssClass="control-label" Text="免關稅" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck1_4" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem1_5" CssClass="control-label" Text="免進口營業稅" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck1_5" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem1_6" CssClass="control-label" Text="免貨物稅" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck1_6" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem1_7" CssClass="control-label" Text="免營業稅" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck1_7" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem1_8" CssClass="control-label" Text="基本資料已輸入購案時程管制紀實系統" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck1_8" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td rowspan="7"><asp:Label CssClass="control-label" Text="二" runat="server"></asp:Label></td>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="合約是否完整？" runat="server" ForeColor="Blue"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem2_1" CssClass="control-label" Text="通用條款" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck2_1" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem2_2" CssClass="control-label" Text="清單" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck2_2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem2_3" CssClass="control-label" Text="型錄" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck2_3" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem2_4" CssClass="control-label" Text="樣品" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck2_4" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem2_5" CssClass="control-label" Text="色樣" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck2_5" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem2_6" CssClass="control-label" Text="其他" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck2_6" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td rowspan="14"><asp:Label CssClass="control-label" Text="三" runat="server"></asp:Label></td>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="是否檢附採購作業相關資料隨合約移轉？" ForeColor="Blue" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_1" CssClass="control-label" Text="核定書及清單全份" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_1" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_2" CssClass="control-label" Text="底價表" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_3" CssClass="control-label" Text="廠商投標報價單" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_3" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_4" CssClass="control-label" Text="開、決標紀錄" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_4" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_5" CssClass="control-label" Text="投標廠商聲明書" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_5" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_6" CssClass="control-label" Text="履保金（函）" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_6" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_7" CssClass="control-label" Text="購案重要事項移交資料" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_7" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_8" CssClass="control-label" Text="信用狀" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_8" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_9" CssClass="control-label" Text="廠商聯絡資料" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_9" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_10" CssClass="control-label" Text="還款保證函" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_10" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_11" CssClass="control-label" Text="廠商報價比較表" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_11" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_12" CssClass="control-label" Text="評審委員會議紀錄" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_12" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem3_13" CssClass="control-label" Text="履保金連帶保證書影本及收繳證明文件" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck3_13" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            <table id="tbItem_2" class="table table-bordered text-center" runat="server">
                                <tr>
                                    <td rowspan="12" class="td-vertical" style="background-color:forestgreen;"><asp:Label CssClass="text-vertical-s" style="height: 190px;" runat="server" ForeColor="White">驗收官檢查項目</asp:Label></td>
                                    <td style="width:100px; background-color:darkblue;"><asp:Label CssClass="control-label" runat="server" ForeColor="White">項次</asp:Label></td>
                                    <td style="background-color:darkblue;"><asp:Label CssClass="control-label" ForeColor="White" runat="server">驗&nbsp;&nbsp;&nbsp;收&nbsp;&nbsp;&nbsp;官&nbsp;&nbsp;&nbsp;檢&nbsp;&nbsp;&nbsp;查&nbsp;&nbsp;&nbsp;項&nbsp;&nbsp;&nbsp;目</asp:Label></td>
                                    <td style="width:120px; background-color:darkblue;"><asp:Label CssClass="control-label" ForeColor="Red" runat="server">是否</asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td rowspan="7"><asp:Label CssClass="control-label" Text="四" runat="server"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="labItem4_1" CssClass="control-label" Text="內容是否有錯（漏）之處？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck4_1" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem4_2" CssClass="control-label" Text="交貨日期是否明確無誤？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck4_2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem4_3" CssClass="control-label" Text="交貨地點是否明確？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck4_3" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem4_4" CssClass="control-label" Text="金額及單、總價是否正確？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck4_4" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem4_5" CssClass="control-label" Text="當年度預算（國防預算）預估是否需辦理保留，如保留是否函發委方？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck4_5" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem4_6" CssClass="control-label" Text="決標紀錄是否有律定型錄納入契約執行之規定？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck4_6" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td class="text-left"><asp:Label ID="labItem4_7" CssClass="control-label" Text="最有利標案合約價格是否有作單價分析？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck4_7" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td rowspan="3"><asp:Label CssClass="control-label" Text="五" runat="server"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="labItem5_1" CssClass="control-label" Text="驗收、檢驗（測試）作業方式是否完整？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck5_1" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem5_2" CssClass="control-label" Text="驗收、檢驗（測試）程序是否完整？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck5_2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:burlywood;">
                                    <td class="text-left"><asp:Label ID="labItem5_3" CssClass="control-label" Text="應附資料（規格）是否完整？" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck5_3" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="background-color:lightblue;">
                                    <td><asp:Label CssClass="control-label" Text="六" runat="server"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="labItem6_1" CssClass="control-label" Text="是否符合履驗下授範圍" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="labCheck6_1" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="td-vertical" style="background-color:darkblue;"><asp:Label CssClass="text-vertical-s" style="height: 50px;" runat="server" ForeColor="Red">綜辦<br />意見</asp:Label></td>
                                    <td colspan="2" style="background-color:burlywood;"><asp:Label ID="labOVC_DO_NAME_RESULT" CssClass="control-label" ForeColor="White" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="background-color:darkblue;"><asp:Label CssClass="control-label" runat="server" ForeColor="White">初&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;審</asp:Label></td>
                                    <td style="background-color:darkblue;"><asp:Label CssClass="control-label" runat="server" ForeColor="White">呈&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;核</asp:Label></td>
                                    <td style="background-color:darkblue;"><asp:Label CssClass="control-label" runat="server" ForeColor="White">批&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;示</asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="height:200px; background-color:lightblue;"><asp:Label CssClass="control-label" runat="server" ForeColor="White"></asp:Label></td>
                                    <td style="height:200px; background-color:lightblue;"><asp:Label CssClass="control-label" runat="server" ForeColor="White"></asp:Label></td>
                                    <td style="height:200px; background-color:lightblue;"><asp:Label CssClass="control-label" runat="server" ForeColor="White"></asp:Label></td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnPrint" CssClass="btn-warning btnw2" OnClick="btnPrint_Click" runat="server" Text="列印" />
                                <iframe id="frame1" runat="server" style="display:none;" ></iframe><br /><br />
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
