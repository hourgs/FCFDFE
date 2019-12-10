<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D26.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D26" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        table tr td{
            border:2px solid black !important;
        }
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
                    購案簽辦作業編輯(限制性招標)
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <table class="table table-bordered text-center">
                            <tr>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">申購單位</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_DEPT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">決標原則</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="OVC_BID" CssClass="control-label" runat="server">最低價,訂有底價,非複數決標</asp:Label>
                                </td>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">招標方式</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">預算金額<br />(採購金額)</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_BUDGET_BUY" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">公告日期</asp:Label>
                                </td>
                                <td colspan="2">
                                   <asp:Label ID="lblOVC_DANNOUNCE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">開標主持人</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class=" ">
                                    <asp:Label  CssClass="control-label" runat="server">履約期限</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PERFORMANCE_LIMIT" CssClass="control-label" runat="server">106年1月1日至106年1月1日</asp:Label>
                                </td>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">發售標單截止日期</asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblOVC_DSELL" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td rowspan="2" class="text-left">
                                    <asp:RadioButtonList ID="rdoOVC_CHAIRMAN" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Value="值">處長</asp:ListItem>
                                        <asp:ListItem Value="值">副處長</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RadioButton ID="rdoOVC_CHAIRMAN_TB" CssClass="radioButton rb-complex position-left" runat="server" />
                                        <asp:TextBox ID="txtOVC_CHAIRMAN_TB" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">押標金額度</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOVC_BID_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">開標時間</asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="control-label" runat="server">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DOPEN" CssClass="tb tb-s position-left" ReadOnly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                         <div class='input-append timepicker'>
                                            <asp:TextBox ID="txtOVC_DOPEN_HM" CssClass='tb tb-s position-left' readonly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-time"></i></span>
                                        </div>
                                        
                                        <div  class='input-append timepicker'>
                                        <asp:TextBox ID="txtONB_TIMES" CssClass="tb tb-s tb tb-s position-left" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label position-left" runat="server">次開標</asp:Label>
                                            </div>
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class=" ">
                                    <asp:Label CssClass="control-label" runat="server">等標期</asp:Label></td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtOVC_DWAIT" CssClass="tb tb-s" runat="server"></asp:TextBox><asp:Label CssClass="control-label" runat="server">日</asp:Label><br /><asp:Label CssClass="control-label text-red" runat="server">系統自動計算：發售標單截止日期-公告日期(亦可自行修訂)</asp:Label></td>
                                <td colspan="3" class=" text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server"><a href="#" target="_blank" style="color:red;text-decoration:underline">法定等標期提示：</a>巨額28日以上</asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">說明</asp:Label></td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtOVC_DESC" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox><br />
                                    <asp:Label CssClass="control-label" runat="server">投、開標方式：</asp:Label><asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">會辦意見</asp:Label></td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtOVC_MEETING" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label></td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtOVC_ADVICE" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td colspan="2" class="text-right no-r">
                                    <asp:Label CssClass="control-label text-red" runat="server">主管批核日</asp:Label>
                                </td>
                                <td colspan="4" class="no-l">
                                    <div class='input-append datetimepicker'>
                                            <asp:TextBox ID="txtOVC_DAPPROVE" CssClass='tb tb-m position-left' readonly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="存檔" />
                            <asp:Button ID="btnReturnP" CssClass="btn-warning btnw6" runat="server" Text="回公告畫面" />
                            <asp:Button ID="btnReturnM" CssClass="btn-warning btnw6" runat="server" Text="回主流程畫面" />
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
