<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D16_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D16_2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        tr td:nth-child(2) {
            text-align:left;
        }
        
        .no-r{
            border-right:0px !important;
        }
        .no-l{
            border-left:0px !important;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title"><!--標題-->
                    公告稿修正作業編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <table class="table table-bordered">
                            <tr>
                                <td colspan="3" style="text-align:right;Width:50%;border-right:none">
                                    <asp:Label CssClass="control-label" runat="server">上一次傳輸日期：</asp:Label>
                                </td>
                                <td colspan="3" style="text-align:left;Width:50%;border-left: none;">
                                    <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none">
                                        <asp:TextBox ID="txtOVC_DSEND" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                        <asp:Label ID="lblOVC_DWORK" Visible="false" runat="server"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>

                        <table class="table table-bordered text-center" style="margin-bottom:0px;">
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【招標單位】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_AGNT_IN" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="txtOVC_AGNT_IN_ID" CssClass="tb tb-l" Style="display:none" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【購案編號】</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH" Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PUR_AGENCY" Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH_5" Style="display:none" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【採購品項】</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【招標方式】</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【承辦人】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_PUR_USER" CssClass="tb tb-m" runat="server"></asp:TextBox>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【承辦人電話】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_USER_CELLPHONE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td style="width:15%">
                                    <asp:Label CssClass="control-label" runat="server">【修正後內容】</asp:Label>
                                </td>
                                <td style="width:85%">
                                    <asp:Label CssClass="control-label" runat="server">原公告內容：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DESC_ORG" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">修正為：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_ITEM_NAME" CssClass="tb tb-full" OnSelectedIndexChanged="drpOVC_ITEM_NAME_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                        <asp:ListItem Value="" Text="請選擇"></asp:ListItem>
                                        <asp:ListItem>•更正標單附件「廠商投標報價單」，廠商須以更正後報價單投標，否則為不合格標。</asp:ListItem>
                                        <asp:ListItem>•本次修訂計更契約附加條款等○處，修訂內容詳如修訂對照表。</asp:ListItem>
                                        <asp:ListItem>原截標時間(○○○年○月○日○○○○時)延至○○○年○月○日○○○○時，原開標時間(○○○年○月○日○○○○時)延至○○○年○月○日○○○○時。</asp:ListItem>
                                        <asp:ListItem>對招標文件內容有疑義之日期，自○○○年○月○日前，延至○○○年○月○日前以書面向本單位提出；對招標文件內容有異議之日期，自○○○年○月○日前，延至○○○年○月○日前以書面向本單位提出。</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtOVC_DESC_MODIFY" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    <table>
                                        <tr>
                                            <td style="width:5%">
                                                <asp:RadioButtonList ID="rdoOVC_DOPEN_MODIFY" CssClass="radioButton rb-complex position-left" runat="server" >
                                                    <asp:ListItem Value="N" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="Y" Text=""></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width:95%" ><br />
                                                <asp:Label CssClass="control-label" runat="server">本案開標日為：</asp:Label>
                                                <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>&nbsp;
                                                <asp:Label ID="lblOVC_OPEN_HOUR" CssClass="control-label" runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                                <asp:Label ID="lblOVC_OPEN_MIN" CssClass="control-label" runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" runat="server">分</asp:Label>
                                                <asp:Label CssClass="control-label" runat="server">維持不變。</asp:Label><br /><br />

                                                <div class="input-append datepicker" >
                                                    <asp:Label CssClass="control-label position-left" runat="server">本案開標日為：</asp:Label>
                                                    <asp:Label ID="lblOVC_DOPEN_input" CssClass="control-label position-left" runat="server"></asp:Label>
                                                    <asp:Label CssClass="control-label position-left" runat="server">，延至</asp:Label>
                                                    <div class="input-append datepicker position-left">
                                                        <asp:TextBox ID="txtOVC_DOPEN_N" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                                        <span class='add-on'><i class="icon-calendar"></i></span>
                                                    </div>
                                                </div>
                                                <asp:TextBox ID="txtOVC_OPEN_HOUR_N" CssClass="tb tb-xs position-left" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                <asp:Label CssClass="control-label position-left" runat="server">時</asp:Label>
                                                <asp:TextBox ID="txtOVC_OPEN_MIN_N" CssClass="tb tb-xs position-left" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                <asp:Label CssClass="control-label position-left" runat="server">分</asp:Label>
                                                <asp:Label CssClass="control-label position-left" runat="server"> 辦理。</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DESC" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【備考】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_MEMO" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="no-bordered no-bordered-seesaw">
                                <td rowspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">【擬辦】</asp:Label></td>
                                <td>
                                    <asp:Label CssClass="control-label position-left text-red" runat="server">1.刊登</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <div class="input-append datepicker position-left">
                                            <asp:TextBox ID="txtOVC_DANNOUNCE" CssClass="tb tb-s position-left text-change" OnTextChanged="txtOVC_DANNOUNCE_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    <asp:Label CssClass="control-label position-left text-red" runat="server">政府採購公報乙日。</asp:Label><br />
                                </td>
                            </tr>
                            <tr class="no-bordered no-bordered-seesaw">
                                <td>
                                    <asp:DropDownList ID="drpOVC_DRAFT_COMM" CssClass="tb tb-full" OnSelectedIndexChanged="drpOVC_DRAFT_COMM_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                        <asp:ListItem Value="" Text="請選擇"></asp:ListItem>
                                        <asp:ListItem>本案招標文件更正事宜，依招標期限標準第七條規定，於等標期截止前變更或補充招標文件內容者，應視需要延長等標期。</asp:ListItem>
                                        <asp:ListItem>本案招標單更正事宜，依招標期限標準第七條第2項規定，本變更非屬重大改變，於原定截止日前五日公告補充招標文件內容，免延長等標期。</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtOVC_DRAFT_COMM" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server" ></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                            <table class="table table-bordered text-center" style="margin-top:0px">
                             <tr class="no-bordered">
                                <td style="width:45%" class="text-right no-r">
                                    <asp:Label CssClass="control-label text-red" runat="server">主官批核日</asp:Label>
                                </td>
                                <td class="no-l">
                                    <div class="input-append datepicker position-left">
                                            <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-default btnw2" Text="存檔" OnClick="btnSave_Click" runat="server" />
                            <asp:Button ID="btnReturnP" CssClass="btn-default btnw6" Text="回招標作業編輯" OnClick="btnReturnP_Click" runat="server" />
                            <asp:Button ID="btnReturnC" CssClass="btn-default btnw8" Text="回招標選擇畫面" OnClick="btnReturnC_Click" runat="server" />
                            <asp:Button ID="btnReturnM" CssClass="btn-default btnw6" Text="回主流程畫面" OnClick="btnReturnM_Click" runat="server" />
                        </div>
                        <div>
                            <div class="subtitle">預覽列印</div>
                            <div class="text-center diva">
                                <asp:LinkButton id="lbtnToWordD16_2" OnClick="lbtnToWordD16_2_Click" runat="server">產出修正公告稿.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton id="lbtnToWordD16_2_pdf" OnClick="lbtnToWordD16_2_pdf_Click" runat="server">產出修正公告稿.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton id="lbtnToWordD16_2_odt" OnClick="lbtnToWordD16_2_odt_Click" runat="server">產出修正公告稿.odt</asp:LinkButton>
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