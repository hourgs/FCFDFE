<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D16_4.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D16_4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
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
                <header class="title text-blue">
                    <!--標題-->
                    <h1>購案公告修正作業編輯</h1>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="col-lg-offset-4">
                            <asp:Label CssClass="control-label position-left" runat="server">傳輸日期：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                            <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                <asp:TextBox ID="txtOVC_DSEND" CssClass="tb tb-s position-left" ReadOnly="true" runat="server"></asp:TextBox>
                                <span class="add-on"><i class="icon-calendar"></i></span>
                            </div>
                        </div>
                        <table class="table table-bordered text-center" style="margin-bottom:0px;">
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【招標單位】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_AGNT_IN" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
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
                                    <asp:TextBox ID="txtOVC_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【承辦人電話】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_TELEPHONE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td style="width:15%">
                                    <asp:Label CssClass="control-label" runat="server">【修正後內容】</asp:Label>
                                </td>
                                <td style="width:85%">
                                    <asp:Label CssClass="control-label" runat="server">原公告內容：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DESC_ORG" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">修正為：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DESC_MODIFY" CssClass="tb tb-full" runat="server"></asp:TextBox>
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

                                                <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:Label CssClass="control-label position-left" runat="server">本案開標日為：</asp:Label>
                                                    <asp:Label ID="lblOVC_DOPEN_input" CssClass="control-label position-left" runat="server"></asp:Label>
                                                    <asp:Label CssClass="control-label position-left" runat="server">，延至</asp:Label>
                                                    <asp:TextBox ID="txtOVC_DOPEN_N" CssClass="tb tb-s position-left" readonly="true" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:TextBox ID="txtOVC_OPEN_HOUR_N" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                                <asp:Label CssClass="control-label position-left" runat="server">時</asp:Label>
                                                <asp:TextBox ID="txtOVC_OPEN_MIN_N" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                                <asp:Label CssClass="control-label position-left" runat="server">分</asp:Label>
                                                <asp:Label CssClass="control-label position-left" runat="server">辦理。</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DESC" TextMode="MultiLine" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【備考】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_MEMO" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="no-bordered no-bordered-seesaw">
                                <td rowspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">【擬辦】</asp:Label></td>
                                <td>
                                    <asp:Label CssClass="control-label position-left text-red" runat="server">1.刊登</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DANNOUNCE" CssClass="tb tb-s position-left" readonly="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    <asp:Label CssClass="control-label position-left text-red" runat="server">政府採購公報乙日。</asp:Label><br />
                                </td>
                            </tr>
                            <tr class="no-bordered no-bordered-seesaw">
                                <td>
                                    <asp:Label ID="lblOVC_DRAFT_COMM" CssClass="control-label position-left text-red" runat="server"></asp:Label></td>
                            </tr>
                            </table>
                            <table class="table table-bordered text-center" style="margin-top:0px">
                             <tr class="no-bordered">
                                <td style="width:45%" class="text-right no-r">
                                    <asp:Label CssClass="control-label text-red" runat="server">主管批核日</asp:Label>
                                </td>
                                <td class="no-l">
                                    <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                        <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left" readonly="true" runat="server"></asp:TextBox>
                                        <span class="add-on"><i class="icon-calendar"></i></span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-warning btnw2" Text="存檔" OnClick="btnSave_Click" runat="server" />
                            <asp:Button ID="btnReturnC" CssClass="btn-warning btnw8" Text="回修正公告選擇畫面" OnClick="btnReturnC_Click" runat="server" />
                            <asp:Button ID="btnReturnP" CssClass="btn-warning btnw6" Text="回公告畫面" OnClick="btnReturnP_Click" runat="server" />
                            <asp:Button ID="btnReturnM" CssClass="btn-warning btnw6" Text="回主流程畫面" OnClick="btnReturnM_Click" runat="server" />
                            <br /><br />
                            <a style="color:red;text-decoration:underline;font-size:18px;" href="#" id="MP_print">修正公告預覽列印</a>
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