<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D16_3.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D16_3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .no-r{
            border-right:0px !important;
        }
        .no-l{
            border-left:0px !important;
        }
        .diva a{
            padding:5px;
            font-size:18px;
            text-decoration:underline;
            color:red;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                    購案簽辦作業編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <table class="table table-bordered text-center">
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_PURCH_A_5" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH" Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PUR_AGENCY" Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH_5" Style="display:none" runat="server"></asp:Label>
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
                                <td class="text-left" style="width:20%">
                                    <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width:10%">
                                    <asp:Label CssClass="control-label" runat="server">決標原則</asp:Label>
                                </td>
                                <td class="text-left" style="width:30%">
                                    <asp:Label ID="lblOVC_BID_METHOD_1" CssClass="control-label" runat="server"></asp:Label>, 
                                    <asp:Label ID="lblOVC_BID_METHOD_2" CssClass="control-label" runat="server"></asp:Label>, 
                                    <asp:Label ID="lblOVC_BID_METHOD_3" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width:10%">
                                    <asp:Label CssClass="control-label" runat="server">招標方式</asp:Label>
                                </td>
                                <td class="text-left" style="width:20%">
                                    <asp:DropDownList ID="ddlOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-full" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">預算金額<br />(採購金額)</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblONB_PUR_BUDGET" CssClass="control-label" runat="server"></asp:Label><br />
                                    (<asp:Label ID="lblOVC_BUDGET_BUY" CssClass="control-label" runat="server"></asp:Label>)<br />
                                    <asp:Label ID="lblONB_PUR_RATE" CssClass="control-label" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">公告日期</asp:Label>
                                </td>
                                <td colspan="2" class="text-left">
                                    <div id="divOVC_DANNOUNCE" visible="false" runat="server">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DANNOUNCE" CssClass="tb tb-s position-left" data-date-format="yyyy-mm-dd" ReadOnly="true" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblOVC_DANNOUNCE" CssClass="control-label text-red"  Text="(限制性招標本欄免填)" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">開標主持人</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label  CssClass="control-label" runat="server">履約期限</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_PERFORMANCE_LIMIT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">發售標單<br>截止日期</asp:Label>
                                </td>
                                <td colspan="2" class="text-left">
                                    <div id="divOVC_DSELL" visible="false" runat="server">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DSELL" CssClass="tb tb-s position-left" data-date-format="yyyy-mm-dd" ReadOnly="true" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:TextBox ID="txtOVC_SELL_HOUR" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label position-left" runat="server">時</asp:Label>
                                        <asp:TextBox ID="txtOVC_SELL_MIN" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label position-left" runat="server">分</asp:Label>
                                    </div>
                                    <asp:Label ID="lblOVC_DSELL" Text="(限制性招標本欄免填)" CssClass="control-label text-red" Visible="false" runat="server"></asp:Label>
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
                                    <asp:TextBox ID="txtOVC_BID_MONEY" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">開標時間</asp:Label>
                                </td>
                                <td colspan="2" class="text-left">
                                    <div class="input-append datepicker">
                                        <asp:TextBox ID="txtOVC_DOPEN" CssClass="tb tb-s position-left" data-date-format="yyyy-mm-dd" ReadOnly="true" runat="server"></asp:TextBox>
                                        <span class="add-on"><i class="icon-calendar"></i></span>
                                        
                                    </div>
                                    <asp:TextBox ID="txtOVC_OPEN_HOUR" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label position-left" runat="server">時</asp:Label>
                                    <asp:TextBox ID="txtOVC_OPEN_MIN" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label position-left" runat="server">分</asp:Label><br />
                                    <asp:RequiredFieldValidator ID="rfvOVC_DOPEN" ControlToValidate="txtOVC_DOPEN" ErrorMessage="請輸入開標日期" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvOVC_OPEN_HOUR" ControlToValidate="txtOVC_OPEN_HOUR" ErrorMessage="請輸入開標日期(時)" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvOVC_OPEN_MIN" ControlToValidate="txtOVC_OPEN_MIN" ErrorMessage="請輸入開標日期(分)" ForeColor="Red" runat="server"></asp:RequiredFieldValidator><br />

                                    <asp:Label CssClass="control-label position-left" runat="server">　第</asp:Label>
                                    <asp:TextBox ID="txtONB_TIMES" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label position-left" runat="server">次開標</asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvONB_TIMES" ControlToValidate="txtONB_TIMES" ErrorMessage="請輸入開標次數" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">等標期</asp:Label></td>
                                <td colspan="2" class="text-left">
                                    <asp:TextBox ID="txtOVC_DWAIT" CssClass="tb tb-s" runat="server"></asp:TextBox><asp:Label CssClass="control-label" runat="server">日</asp:Label><br />
                                    <asp:Label ID="lblOVC_DWAIT" CssClass="control-label text-red" runat="server"></asp:Label></td>
                                <td colspan="3" class="text-left">
                                    <asp:Label ID="lblDwaitTip" CssClass="control-label text-red" Visible="false" runat="server"><a href="#" target="_blank" style="color:blue;text-decoration:underline">法定等標期提示：</a>巨額28日以上</asp:Label>
                                    <asp:Label ID="lblDwaitTip_Restrict" CssClass="control-label text-red" Text="限制性議價合理定訂" Visible="false" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">說明</asp:Label></td>
                                <td colspan="5" class="text-left">
                                    <asp:TextBox ID="txtOVC_DESC" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    <asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label text-blue" Visible="false" runat="server"></asp:Label></td>
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
                                    <asp:TextBox ID="txtOVC_ADVICE" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox></td>
                            </tr>
                            
                                    
                                
                        </table>

                        <table class="table table-bordered" style="border:none">
                                        <tr>
                                            <td style="width:50%" class="text-right"><asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                            <td style="width:50%" class="text-left">
                                                <div class="input-append date datepicker">
                                                    <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left" data-date-format="yyyy-mm-dd" runat="server" readonly="true"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-warning btnw2" Text="存檔" OnClick="btnSave_Click" runat="server" />
                            <asp:Button ID="btnReturnP" CssClass="btn-warning btnw6" Text="回公告畫面" OnClick="btnReturnP_Click" runat="server" />
                            <asp:Button ID="btnReturnM" CssClass="btn-warning btnw6" Text="回主流程畫面" OnClick="btnReturnM_Click" runat="server" />
                        </div>
                        
                <div>
                    <div class="subtitle">預覽列印</div>
                    <div class="text-center diva">
                        <a href="SignedHtml">1. 簽辦表(html格式)</a><a href="SignedWord">簽辦表(Word格式)</a>
                        <a href="Processing">2. 會辦單</a>
                        <a href="Notificate">3. 開標通知函</a>
                        <a href="Distribute">4. 開標分送通知單</a>
                        <a href="Verificate">5. 物資核定書</a>
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

