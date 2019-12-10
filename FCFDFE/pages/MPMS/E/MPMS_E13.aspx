<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E13.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E13" %>
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
                    <!--標題-->契約接管
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="subtitle" runat="server" Text="請注意！未輸入資料或存檔無法列印各項報表！"></asp:Label></td>
                                </tr>
                            </table>
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="購案契約編號(組別)"></asp:Label></td>
                                        <td class="text-left"><asp:Label ID="lblOVC_PURCH" Text="" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="購案名稱"></asp:Label></td>
                                        <td class="text-left"><asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="申購單位(代碼)-申購人(電話)"></asp:Label></td>
                                        <td class="text-left"><asp:Label ID="lblApplyDept" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="契約金額"></asp:Label></td>
                                        <td class="text-left"><asp:Label ID="lblONB_MCONTRACT" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="決標日期"></asp:Label></td>
                                        <td class="text-left"><asp:Label ID="lblOVC_DBID" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="簽約日期"></asp:Label></td>
                                        <td class="text-left"><asp:Label ID="lblOVC_DCONTRACT" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="承包商名稱"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-full" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="display:inline-block" onmouseover="document.getElementById('div1').style.display = 'block';document.getElementById('div2').style.display = 'none';document.getElementById('div3').style.display = 'none';document.getElementById('div4').style.display = 'none';document.getElementById('div5').style.display = 'none';document.getElementById('div6').style.display = 'none';document.getElementById('div7').style.display = 'none';">
                                                <asp:Button ID="btnMargin" CssClass="btn-success btnw6" Text="履約保證金" runat="server" /></div>
                                            <div style="display:inline-block" onmouseover="document.getElementById('div2').style.display = 'block';document.getElementById('div1').style.display = 'none';document.getElementById('div3').style.display = 'none';document.getElementById('div4').style.display = 'none';document.getElementById('div5').style.display = 'none';document.getElementById('div6').style.display = 'none';document.getElementById('div7').style.display = 'none';">
                                                <asp:Button ID="btnOVC_DELIVERY_PERIOD" CssClass="btn-success btnw6" Text="契約交貨時間" runat="server" /></div>
                                            <div style="display:inline-block" onmouseover="document.getElementById('div3').style.display = 'block';document.getElementById('div1').style.display = 'none';document.getElementById('div2').style.display = 'none';document.getElementById('div4').style.display = 'none';document.getElementById('div5').style.display = 'none';document.getElementById('div6').style.display = 'none';document.getElementById('div7').style.display = 'none';">
                                                <asp:Button ID="btnOVC_DELIVERY_PLACE" CssClass="btn-success btnw6" Text="契約交貨地點" runat="server" /></div>
                                            <div style="display:inline-block" onmouseover="document.getElementById('div4').style.display = 'block';document.getElementById('div1').style.display = 'none';document.getElementById('div2').style.display = 'none';document.getElementById('div3').style.display = 'none';document.getElementById('div5').style.display = 'none';document.getElementById('div6').style.display = 'none';document.getElementById('div7').style.display = 'none';">
                                                <asp:Button ID="btnExamine" CssClass="btn-success btnw4" Text="檢驗方式" runat="server" /></div>
                                            <div style="display:inline-block" onmouseover="document.getElementById('div5').style.display = 'block';document.getElementById('div1').style.display = 'none';document.getElementById('div2').style.display = 'none';document.getElementById('div3').style.display = 'none';document.getElementById('div4').style.display = 'none';document.getElementById('div6').style.display = 'none';document.getElementById('div7').style.display = 'none';">
                                                <asp:Button ID="btnOVC_PAYMENT" CssClass="btn-success btnw4" Text="付款方式" runat="server" /></div>
                                            <div style="display:inline-block" onmouseover="document.getElementById('div6').style.display = 'block';document.getElementById('div1').style.display = 'none';document.getElementById('div2').style.display = 'none';document.getElementById('div3').style.display = 'none';document.getElementById('div4').style.display = 'none';document.getElementById('div5').style.display = 'none';document.getElementById('div7').style.display = 'none';">
                                                <asp:Button ID="btnDutyFree" CssClass="btn-success btnw4" Text="免稅方式" runat="server" /></div>
                                            <div style="display:inline-block" onmouseover="document.getElementById('div7').style.display = 'block';document.getElementById('div1').style.display = 'none';document.getElementById('div2').style.display = 'none';document.getElementById('div3').style.display = 'none';document.getElementById('div4').style.display = 'none';document.getElementById('div5').style.display = 'none';document.getElementById('div6').style.display = 'none';">
                                                <asp:Button ID="btnOVC_KIND" CssClass="btn-success btnw4" Text="保固方式" runat="server" /></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div id="div1" style="display: none;">
                                                <p><asp:Label ID="lblONB_MCONTRACTPer1_1" CssClass="control-label text-center" Text="" runat="server"></asp:Label></p>
                                                <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer1_2" CssClass="control-label text-blue" Text="" runat="server"></asp:Label></p>
                                            </div>
                                            <div id="div2" style="display: none;">
                                                <p><asp:Label ID="lblONB_MCONTRACTPer2_1" CssClass="control-label text-center" Text="" runat="server"></asp:Label></p>
                                                <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer2_2" CssClass="control-label text-blue" Text="" runat="server"></asp:Label></p>
                                            </div>
                                            <div id="div3" style="display: none;">
                                                <p><asp:Label ID="lblONB_MCONTRACTPer3_1" CssClass="control-label text-center" Text="" runat="server"></asp:Label></p>
                                                <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer3_2" CssClass="control-label text-blue" Text="" runat="server"></asp:Label></p>
                                            </div>
                                            <div id="div4" style="display: none;">
                                                <p><asp:Label ID="lblONB_MCONTRACTPer4_1" CssClass="control-label text-center" Text="" runat="server"></asp:Label></p>
                                                <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer4_2" CssClass="control-label text-blue" Text="" runat="server"></asp:Label></p>
                                            </div>
                                            <div id="div5" style="display: none;">
                                                <p><asp:Label ID="lblONB_MCONTRACTPer5_1" CssClass="control-label text-center" Text="" runat="server"></asp:Label></p>
                                                <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer5_2" CssClass="control-label text-blue" Text="" runat="server"></asp:Label></p>
                                            </div>
                                            <div id="div6" style="display: none;">
                                                <p><asp:Label ID="lblONB_MCONTRACTPer6_1" CssClass="control-label text-center" Text="" runat="server"></asp:Label></p>
                                                <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer6_2" CssClass="control-label text-blue" Text="" runat="server"></asp:Label></p>
                                            </div>
                                            <div id="div7" style="display: none;">
                                                <p><asp:Label ID="lblONB_MCONTRACTPer7_1" CssClass="control-label text-center" Text="" runat="server"></asp:Label></p>
                                                <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer7_2" CssClass="control-label text-blue" Text="" runat="server"></asp:Label></p>
                                            </div>
                                            <p><asp:Label ID="lblONB_MCONTRACTPer" CssClass="control-label text-center" Text="" runat="server"></asp:Label></p>
                                            <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer2" CssClass="control-label text-blue" Text="" runat="server"></asp:Label></p>
                                        </td>
                                    </tr>
                                </table>
                            <div class="title">
                                <asp:Label CssClass="control-label" runat="server" Text="契約移轉履約驗結處檢查項目表編輯" Font-Size="Large" Font-Bold="True"></asp:Label>
                            </div>
                            <!--頁籤－開始-->
                                <header class="panel-heading">
                                    <ul class="nav nav-tabs">
                                        <!--各頁籤-->
                                        <li class="active"><!--起始選取頁籤，只有一個-->
                                            <a data-toggle="tab" href="#TabFirst">第一頁</a>
                                        </li>
                                        <li class=""><!--尚未選取頁籤-->
                                            <a data-toggle="tab" href="#TabSecond">第二頁</a>
                                        </li>
                                        <li class=""><!--尚未選取頁籤-->
                                            <a data-toggle="tab" href="#TabThird">第三頁</a>
                                        </li>
                                    </ul>
                                </header>
                                <div class="panel-body tab-body">
                                    <div class="tab-content">
                                        <!--各標籤之頁面-->
                                        <div id="TabFirst" class="tab-pane active"><!--起始選取頁面，只有一個-->
                                            <table id="tbItem" class="table table-bordered text-center" runat="server">
                                                <tr>
                                                    <td style="width:70%"><asp:Label ID="Label11" CssClass="control-label" runat="server" Text="驗收官檢查項目"></asp:Label></td>
                                                    <td><asp:Label CssClass="control-label" runat="server" Text="是否"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ1_1" CssClass="control-label" runat="server" Text="份數是否足夠？內：正一副三　外：正一副一 　下授:正一副一"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck1_1" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ1_2" CssClass="control-label" runat="server" Text="有無編訂目錄頁次號？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck1_2" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ1_3" CssClass="control-label" runat="server" Text="塗改處是否有蓋校正章？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck1_3" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ1_4" CssClass="control-label" runat="server" Text="免關稅"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck1_4" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ1_5" CssClass="control-label" runat="server" Text="免進口營業稅"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck1_5" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ1_6" CssClass="control-label" runat="server" Text="免貨物稅"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck1_6" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ1_7" CssClass="control-label" runat="server" Text="免營業稅"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck1_7" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ1_8" CssClass="control-label" runat="server" Text="基本資料已輸入購案時程管制紀實系統"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck1_8" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="TabSecond" class="tab-pane"><!--尚未選取頁面-->
                                            <table class="table table-bordered text-center">
                                                <tr>
                                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="合約是否完整？"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left" style="width:70%"><asp:Label ID="lblQ2_1" CssClass="control-label" runat="server" Text="通用條款"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_1" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_2" CssClass="control-label" runat="server" Text="清單"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_2" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_3" CssClass="control-label" runat="server" Text="型錄"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_3" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_4" CssClass="control-label" runat="server" Text="樣品"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_4" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_5" CssClass="control-label" runat="server" Text="色樣"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_5" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_6" CssClass="control-label" runat="server" Text="其他"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_6" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server" Text="是否檢附採購作業相關資料隨合約移轉？"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_7" CssClass="control-label" runat="server" Text="核定書及清單全份"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_7" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_8" CssClass="control-label" runat="server" Text="底價表"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_8" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_9" CssClass="control-label" runat="server" Text="廠商投標報價單"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_9" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_10" CssClass="control-label" runat="server" Text="開、決標紀錄"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_10" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_11" CssClass="control-label" runat="server" Text="投標廠商聲明書"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_11" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_12" CssClass="control-label" runat="server" Text="履保金（函）"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_12" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_13" CssClass="control-label" runat="server" Text="購案重要事項移交資料"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_13" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_14" CssClass="control-label" runat="server" Text="信用狀"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_14" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_15" CssClass="control-label" runat="server" Text="廠商聯絡資料"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_15" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_16" CssClass="control-label" runat="server" Text="還款保證函"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_16" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_17" CssClass="control-label" runat="server" Text="廠商報價比較表"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_17" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_18" CssClass="control-label" runat="server" Text="評審委員會議紀錄"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_18" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ2_19" CssClass="control-label" runat="server" Text="履保金連帶保證書影本及收繳證明文件"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck2_19" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="TabThird" class="tab-pane"><!--尚未選取頁面-->
                                            <table class="table table-bordered text-center">
                                                <tr>
                                                    <td style="width:70%"><asp:Label ID="Label15" CssClass="control-label" runat="server" Text="驗收官檢查項目"></asp:Label></td>
                                                    <td><asp:Label CssClass="control-label" runat="server" Text="是否"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_1" CssClass="control-label" runat="server" Text="內容是否有錯（漏）之處？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_1" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_2" CssClass="control-label" runat="server" Text="交貨日期是否明確無誤？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_2" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_3" CssClass="control-label" runat="server" Text="交貨地點是否明確？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_3" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_4" CssClass="control-label" runat="server" Text="金額及單、總價是否正確？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_4" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_5" CssClass="control-label" runat="server" Text="當年度預算（國防預算）預估是否需辦理保留，如保留是否函發委方？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_5" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_6" CssClass="control-label" runat="server" Text="決標紀錄是否有律定型錄納入契約執行之規定？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_6" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_7" CssClass="control-label" runat="server" Text="最有利標案合約價格是否有作單價分析？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_7" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_8" CssClass="control-label" runat="server" Text="驗收、檢驗（測試）作業方式是否完整？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_8" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_9" CssClass="control-label" runat="server" Text="驗收、檢驗（測試）程序是否完整？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_9" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_10" CssClass="control-label" runat="server" Text="應附資料（規格）是否完整？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_10" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left"><asp:Label ID="lblQ3_11" CssClass="control-label" runat="server" Text="是否符合履驗下授範圍？"></asp:Label></td>
                                                    <td><asp:DropDownList ID="drpCheck3_11" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                        <asp:ListItem>免審</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                            </table>
                                            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <table class="table table-bordered">
                                                        <tr>
                                                            <td  class="text-center">
                                                                <asp:Label CssClass="control-label" runat="server" Text="綜辦"></asp:Label><br>
                                                                <asp:Label CssClass="control-label" runat="server" Text="意見"></asp:Label>
                                                            </td>
                                                            <td class="text-left">
                                                                <asp:RadioButtonList ID="rdoOVC_DO_NAME_RESULT" OnSelectedIndexChanged="rdoOVC_DO_NAME_RESULT_SelectedIndexChanged" AutoPostBack="true" CssClass="radioButton" runat="server" RepeatLayout="UnorderedList">
                                                                    <asp:ListItem>可接受</asp:ListItem>
                                                                    <asp:ListItem>洽採購發包單位澄清後辦理</asp:ListItem>
                                                                    <asp:ListItem>先行接管補請修正</asp:ListItem>
                                                                </asp:RadioButtonList></td>
                                                        </tr>
                                                        <tr id="reason" visible="false" runat="server">
                                                            <td class="text-center">
                                                                <asp:Label ID="Reason_for_withdrawal" CssClass="control-label" runat="server" Text="退案"></asp:Label><br>
                                                                <asp:Label ID="Reason_for_withdrawal2" CssClass="control-label" runat="server" Text="原因"></asp:Label>
                                                            </td>
                                                            <td class="text-left">
                                                                <asp:TextBox ID="Reason_for_withdrawal_textbox" CssClass="tb tb-full" Height="80px" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <table class="table table-bordered">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label CssClass="control-label text-red position-left" runat="server" Text="採包移送日："></asp:Label>
                                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                                <asp:TextBox ID="txtOVC_DSEND" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                                                 </div>
                                                                <asp:Button ID="btnResetOVC_DSEND" CssClass="btn-default btnw4" OnClick="btnResetOVC_DSEND_Click" runat="server" Text="清除日期" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="Label20" CssClass="control-label text-red position-left" runat="server" Text="收辦日："></asp:Label>
                                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                                <asp:TextBox ID="txtOVC_DRECEIVE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                                                 </div>
                                                                <asp:Button ID="btnResetOVC_DRECEIVE" CssClass="btn-default btnw4" OnClick="btnResetOVC_DRECEIVE_Click" runat="server" Text="清除日期" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label CssClass="control-label text-red position-left" runat="server" Text="履驗承辦人："></asp:Label>
                                                        <asp:DropDownList ID="drpOVC_DO_NAME" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label CssClass="control-label text-red position-left" runat="server" Text="主官核批日："></asp:Label>
                                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                                <asp:TextBox ID="txtOVC_DO_DAPPROVE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                                                 </div>
                                                                <asp:Button ID="btnResetOVC_DO_DAPPROVE" CssClass="btn-default btnw4" OnClick="btnResetOVC_DO_DAPPROVE_Click" runat="server" Text="清除日期" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            <!--頁籤－結束-->
                            <br><br>
                                <div class="text-center">
                                    <asp:Button ID="btnSave" CssClass="btn-warning btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
                                    <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                    <asp:Button ID="Button1" CssClass="btn-default btn" OnClick="LinkButton3_Click" runat="server" Text="檢查表預覽列印" />
                                    <br><br>
                                    <asp:LinkButton ID="LinkButton5" OnClick="LinkButton5_Click" runat="server">列印物資申請書.doc</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton8" OnClick="LinkButton8_Click" runat="server">列印計畫清單.doc</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <!--<asp:LinkButton ID="LinkButton7" OnClick="LinkButton7_Click" runat="server">檢查項目表.doc</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-->
                                    <asp:LinkButton ID="LinkButton4" OnClick="LinkButton4_Click" runat="server">列印時程管制表.doc</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton6" OnClick="LinkButton6_Click" runat="server">列印合約.doc</asp:LinkButton>
                                    <br>
                                    <asp:LinkButton ID="LinkButton1" OnClick="LinkButton1_Click" runat="server">列印物資申請書.pdf</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton2" OnClick="LinkButton2_Click" runat="server">列印計畫清單.pdf</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton11" OnClick="LinkButton11_Click" runat="server">列印時程管制表.pdf</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton12" OnClick="LinkButton12_Click" runat="server">列印合約.pdf</asp:LinkButton>
                                    <br>
                                    <asp:LinkButton ID="LinkButton13" OnClick="LinkButton13_Click" runat="server">列印物資申請書.odt</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton14" OnClick="LinkButton14_Click" runat="server">列印計畫清單.odt</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton17" OnClick="LinkButton17_Click" runat="server">列印時程管制表.odt</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton18" OnClick="LinkButton18_Click" runat="server">列印合約.odt</asp:LinkButton>
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
