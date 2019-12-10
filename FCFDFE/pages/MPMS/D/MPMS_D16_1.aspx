<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D16_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D16_1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title"><!--標題-->
                    招標作業編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <!--網頁內容-->
                        <table class="table table-bordered text-center">
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_PURCH_A_5" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                </td>
                                <td colspan="3" class="text-left">
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10%">
                                    <asp:Label CssClass="control-label" runat="server">申購單位</asp:Label>
                                </td>
                                <td class="text-left" style="width:25%">
                                    <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width:10%">
                                    <asp:Label CssClass="control-label" runat="server">決標原則</asp:Label>
                                </td>
                                <td class="text-left" style="width:35%">
                                    <asp:CheckBoxList ID="chkOVC_BID_METHOD" CssClass="radioButton" RepeatDirection="Vertical" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>定有底價，非複數決標</asp:ListItem>
                                            <asp:ListItem>最有利標</asp:ListItem>
                                            <asp:ListItem>(準用)最有利標、固定價格給付</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td style="width:10%">
                                    <asp:Label CssClass="control-label" runat="server">招標方式</asp:Label>
                                </td>
                                <td class="text-left" style="width:20%">
                                    <asp:DropDownList ID="drpOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-full" OnSelectedIndexChanged="drpOVC_PUR_ASS_VEN_CODE_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">預算金額<br />(採購金額)</asp:Label>
                                </td>
                                <td class="text-left">
                                    <p><asp:Label ID="lblONB_PUR_BUDGET" CssClass="control-label" runat="server"></asp:Label></p>
                                    <p><asp:Label ID="lblONB_PUR_RATE" CssClass="control-label" Visible="false" runat="server"></asp:Label></p>
                                    <p><asp:TextBox ID="txtOVC_BUDGET_BUY" CssClass="tb tb-full" runat="server"></asp:TextBox></p>
                                    <p><asp:Label CssClass="control-label" runat="server">若採購金額為空白，預設值為"同上"</asp:Label></p>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">公告日期</asp:Label>
                                </td>
                                <td colspan="2" class="text-left">
                                    <div id="divOVC_DANNOUNCE" visible="false" runat="server">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DANNOUNCE" CssClass="tb tb-s position-left" data-date-format="yyyy-mm-dd" OnTextChanged="txtOVC_DANNOUNCE_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblOVC_DANNOUNCE" CssClass="control-label text-red" Text="(限制性招標本欄免填)" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">開標主持人</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label  CssClass="control-label" runat="server">核定文號</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_PUR_APPROVE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">領標及<br>投標期限</asp:Label>
                                </td>
                                <td colspan="2" class="text-left">
                                    <div class="input-append datepicker position-left">
                                        <asp:TextBox ID="txtOVC_DBID_LIMIT" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div>
                                    <asp:TextBox ID="txtOVC_LIMIT_HOUR" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                    <asp:Label Text="時" CssClass="control-label position-left" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtOVC_LIMIT_MIN" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                    <asp:Label Text="分" CssClass="control-label position-left" runat="server"></asp:Label>
                                </td>
                                <td rowspan="2" class="text-left">   
                                    <asp:RadioButtonList ID="rdoOVC_CHAIRMAN" OnSelectedIndexChanged="rdoOVC_CHAIRMAN_SelectedIndexChanged" AutoPostBack="true" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem>處長</asp:ListItem>
                                        <asp:ListItem>副處長</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RadioButton ID="rdoOVC_CHAIRMAN_Other" OnCheckedChanged="rdoOVC_CHAIRMAN_Other_CheckedChanged" AutoPostBack="true" CssClass="radioButton rb-complex position-left" runat="server" />
                                        <asp:TextBox ID="txtOVC_CHAIRMAN_Other" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">押標金額度</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:DropDownList ID="drpOVC_BID_MONEY" OnSelectedIndexChanged="drpOVC_BID_MONEY_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-full" runat="server">
                                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                            <asp:ListItem>按廠商報價</asp:ListItem>
                                            <asp:ListItem>定額</asp:ListItem>
                                            <asp:ListItem>免繳</asp:ListItem>
                                        </asp:DropDownList>
                                    <asp:Label ID="lblOVC_BID_MONEY_1" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtOVC_BID_MONEY" Visible="false" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                    <asp:Label ID="lblOVC_BID_MONEY_2" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">開標時間</asp:Label>
                                </td>
                                <td colspan="2" class="text-left">
                                    <div id="div1" runat="server">
                                        <asp:Label ID="lblOVC_DOPEN" CssClass="control-label position-left" Visible="false" runat="server" ></asp:Label>
                                        <div id="divOVC_DOPEN" visible="false" runat="server">
                                            <div class="input-append datepicker position-left">
                                                    <asp:TextBox ID="txtOVC_DOPEN" CssClass="tb tb-s position-left text-change" AutoPostBack="true" OnTextChanged="txtOVC_DOPEN_TextChanged" runat="server"></asp:TextBox>
                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                            <asp:TextBox ID="txtOVC_OPEN_HOUR" CssClass="tb tb-xs position-left" AutoPostBack="true" OnTextChanged="txtOVC_OPEN_HOUR_TextChanged" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label position-left" Text="時" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtOVC_OPEN_MIN" CssClass="tb tb-xs position-left" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label position-left" Text="分" runat="server"></asp:Label><br /><br />
                                        </div>

                                        <div>
                                            <asp:Label ID="errorOVC_DOPEN" CssClass="control-label text-red" Text="　請輸入開標時間(日期)" Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="errorOVC_OPEN_HOUR" CssClass="control-label text-red" Text="　請輸入開標時間(時)" Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="errorOVC_OPEN_MIN" CssClass="control-label text-red" Text="　請輸入開標時間(分)" Visible="false" runat="server"></asp:Label>
                                        </div>

                                        <asp:Label CssClass="control-label position-left" runat="server">第</asp:Label>
                                        <asp:Label ID="lblONB_TIMES" CssClass="control-label position-left" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label position-left" runat="server">次開標</asp:Label><br />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">等標期</asp:Label></td>
                                <td class="text-left">
                                    <asp:TextBox ID="txtOVC_DWAIT" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">日</asp:Label><br />
                                    <asp:Label ID="lblOVC_DWAIT" CssClass="control-label text-red" runat="server"></asp:Label></td>
                                <td><asp:Label CssClass="control-label" runat="server">開標地點</asp:Label></td>
                                <td colspan="3"><asp:TextBox ID="txtOVC_PLACE" CssClass="tb tb-full" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">說明</asp:Label></td>
                                <td colspan="5" class="text-left">
                                    <p>一、</p>
                                    <p><asp:DropDownList ID="drpOVC_DESC_1" OnSelectedIndexChanged="drpOVC_DESC_1_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-full" runat="server">
                                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                            <asp:ListItem>本案奉核依政府採購法第００條規定，採００招標、不分段開標、總價報價、總價決標方式辦理，並採訂定底價，以合於招標文件規定，且在底價以內之最低標者為得標廠商。（俟案況填）</asp:ListItem>
                                            <asp:ListItem>本案依「政府採購法」第OO條第O項第O款以限制性招標洽OOOOO公司以議價、不分段開標、訂有底價、總價報價、總價決標方式辦理。</asp:ListItem>
                                        </asp:DropDownList></p>
                                    <p>二、</p>
                                    <p><asp:DropDownList ID="drpOVC_DESC_2" OnSelectedIndexChanged="drpOVC_DESC_2_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-full" runat="server">
                                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                            <asp:ListItem>本案為００金額以上之採購，法定等標期不得少於○○日，因０００（俟情況填：已辦理公開閱覽或開放電子投、領標，依招標期限標準第九條規定縮短○日；因案屬專業服務案，考量廠商須投注較多之作業人力或考量廣增商源，故適予延長等標期），排定等標期為○○日。</asp:ListItem>
                                            <asp:ListItem>依子法「招標期限標準」第6條規定，等標期由機關合理訂定之，經協調OOOOOO公司等標期定為O日，故廠商投標截止期限為O年O月O日O時前；另依政府採購法施行細則第54條規定及商綜處訂頒之「限制性招標（議價案）底價訂定作業程序」，訂定底價前應先參考廠商之報價，並已協調商綜處表示配合辦理。故議價時間訂於O年O月O日O時。</asp:ListItem>
                                        </asp:DropDownList></p>
                                    <p>三、其他：</p>
                                    <p><asp:DropDownList ID="drpOVC_DESC_3" OnSelectedIndexChanged="drpOVC_DESC_3_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-full" runat="server">
                                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                            <asp:ListItem>本案屬條件簡單者，依採購評選委員會組織準則第三條規定免於招標前成立評選委員會。</asp:ListItem>
                                            <asp:ListItem>本案經奉核定，採購評選委員會名單採公告方式辦理。</asp:ListItem>
                                            <asp:ListItem>本案評選時間訂於０年０月０日００時辦理。</asp:ListItem>
                                            <asp:ListItem>本案為查核以上之工程採購，於０至０（民眾意見送達期限至０止）公開閱覽，閱覽期間並無民眾提陳意見。</asp:ListItem>
                                            <asp:ListItem>本案依政府採購公告及公報發行辦法第11條第2項規定，本案於於招標公告公開預算金額。</asp:ListItem>
                                            <asp:ListItem>本案所有工程預算單價（含總表、詳細價目表、資源統計表、單價分析表），將依政府採購法第二十七條第三項規定於招標公告中一併公開。</asp:ListItem>
                                            <asp:ListItem>本案為避免廠商報價浮濫，擬於招標文件中公開預算。</asp:ListItem>
                                            <asp:ListItem>本案採開口契約之作業模式，決標後以各項單價計列契約方式辦理，並以實作數量計價付款，依原公告預算(預估總價)或招標文件載明之預估數量製作契約，並以全案預估總價為採購上限。</asp:ListItem>
                                            <asp:ListItem>本案預算尚未奉核，奉准依「國軍採購作業規定」第41點及「預算法」第54條第1項第2款第2目規定，先行辦理招標及逕行決標、簽約作業。</asp:ListItem>
                                            <asp:ListItem>本案預算尚未奉核，奉准依「國軍採購作業規定」第41點先行開標並保留決標，俟預算奉核後再行決標、簽約；如預算未奉核定，則依政府採購法第48條第1項第7款規定不予決標。</asp:ListItem>
                                        </asp:DropDownList></p>
                                    <p><asp:TextBox ID="txtOVC_DESC_1" TextMode="MultiLine" Rows="3" CssClass="textarea tb-full" runat="server"></asp:TextBox></p>
                                    <p><asp:TextBox ID="txtOVC_DESC_2" TextMode="MultiLine" Rows="3" CssClass="textarea tb-full" runat="server"></asp:TextBox></p>
                                    <p><asp:TextBox ID="txtOVC_DESC_3" TextMode="MultiLine" Rows="12" CssClass="textarea tb-full" runat="server"></asp:TextBox></p>
                                    <p><asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label text-blue" Visible="false" runat="server"></asp:Label></p></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">會辦意見</asp:Label></td>
                                <td colspan="5" class="text-left">
                                    <asp:TextBox ID="txtOVC_MEETING" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label></td>
                                <td colspan="5" class="text-left">
                                    <asp:TextBox ID="txtOVC_DRAFT_COMM" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>


                        <table class="table table-bordered">
                            <tr>
                                <td colspan="3" style="text-align:right;Width:50%;border-right:none">
                                    <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label>
                                </td>
                                <td colspan="3" style="text-align:left;Width:50%;border-left: none;">
                                    <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none">
                                        <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div>
                                </td>
                            </tr>
                        </table>

                        <div style="text-align:center"> 
                            <asp:Button ID="btnSave" CssClass="btn-default btnw2" Text="存檔" OnClick="btnSave_Click" runat="server"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnBack" CssClass="btn-default" Text="回上一頁" OnClick="btnBack_Click" runat="server"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnReturnM" CssClass="btn-default btnw6" Text="回主流程畫面" OnClick="btnReturnM_Click" runat="server" /><br /><br />
                            <div id="divBtn" visible="false" runat="server">
                                <asp:Button ID="btnFixNotice" CssClass="btn-default" Text="公告稿修正作業" OnClick="btnFixNotice_Click" runat="server" />
                            </div>
                        </div>

                        <div>
                            <div class="subtitle">預覽列印</div>
                            <div class="text-center diva">
                                <table>
                                    <tr>
                                        <td style="width:150px"><asp:LinkButton ID="lbtnToWordD16_1_1" OnClick="lbtnToWordD16_1_1_Click" runat="server">簽辦單.doc</asp:LinkButton></td>
                                        <td style="width:150px"><asp:LinkButton ID="lbtnToWordD16_1_2" OnClick="lbtnToWordD16_1_2_Click" runat="server">開標通知單(上呈版).doc</asp:LinkButton></td>
                                        <td style="width:150px"><asp:LinkButton ID="lbtnToWordD16_1_3" OnClick="lbtnToWordD16_1_3_Click" runat="server">開標通知單(正式版).doc</asp:LinkButton></td>
                                        <td style="width:150px"><asp:LinkButton ID="lbtnToWordD16_1_4" OnClick="lbtnToWordD16_1_4_Click" runat="server">招標文件封面.doc</asp:LinkButton></td>
                                    </tr>
                                    <tr>
                                        <td><asp:LinkButton ID="lbtnToWordD16_1_1_pdf" OnClick="lbtnToWordD16_1_1_pdf_Click" runat="server">簽辦單.pdf</asp:LinkButton></td>
                                        <td><asp:LinkButton ID="lbtnToWordD16_1_2_pdf" OnClick="lbtnToWordD16_1_2_pdf_Click" runat="server">開標通知單(上呈版).pdf</asp:LinkButton></td>
                                        <td><asp:LinkButton ID="lbtnToWordD16_1_3_pdf" OnClick="lbtnToWordD16_1_3_pdf_Click" runat="server">開標通知單(正式版).pdf</asp:LinkButton></td>
                                        <td><asp:LinkButton ID="lbtnToWordD16_1_4_pdf" OnClick="lbtnToWordD16_1_4_pdf_Click" runat="server">招標文件封面.pdf</asp:LinkButton></td>
                                    </tr>
                                    <tr>
                                        <td><asp:LinkButton ID="lbtnToWordD16_1_1_odt" OnClick="lbtnToWordD16_1_1_odt_Click" runat="server">簽辦單.odt</asp:LinkButton></td>
                                        <td><asp:LinkButton ID="lbtnToWordD16_1_2_odt" OnClick="lbtnToWordD16_1_2_odt_Click" runat="server">開標通知單(上呈版).odt</asp:LinkButton></td>
                                        <td><asp:LinkButton ID="lbtnToWordD16_1_3_odt" OnClick="lbtnToWordD16_1_3_odt_Click" runat="server">開標通知單(正式版).odt</asp:LinkButton></td>
                                        <td><asp:LinkButton ID="lbtnToWordD16_1_4_odt" OnClick="lbtnToWordD16_1_4_odt_Click" runat="server">招標文件封面.odt</asp:LinkButton></td>
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
