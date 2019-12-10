<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D16_5.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D16_5" %>

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
                    <!--標題--><h1>
                    購案公告修正作業編輯</h1>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="col-lg-offset-4">
                            <asp:Label CssClass="control-label position-left" runat="server">傳輸日期：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <!--↓日期套件↓-->
                                        <div class="input-append date" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DSEND" CssClass="tb tb-s position-left" readonly="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <!--↑日期套件↑-->
                        </div>
                        <table class="table table-bordered text-center" style="margin-bottom:0px;">
                            <tr class="no-bordered">
                                <td style="width: 30%">
                                    <asp:Label CssClass="control-label" runat="server">【招標單位】</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_AGNT_IN" CssClass="control-label" runat="server">NC06001L074</asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td style="width: 30%">
                                    <asp:Label CssClass="control-label" runat="server">【購案編號】</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">NC06001L074</asp:Label></td>
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
                                <td><asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label></td>
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
                                    <asp:TextBox ID="txtOVC_PUR_IUSER_PHONE_EXT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【修正後內容】</asp:Label></td>
                                <td><asp:Label CssClass="control-label" runat="server">原公告內容：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DESC_ORG" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">修正為：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DESC_MODIFY" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    <div style="display:inline-flex">
                                    <asp:RadioButton ID="rdoOVC_DOPEN" CssClass="radioButton rb-complex" runat="server" />
                                        <asp:Label CssClass="control-label" runat="server">本案開標日為：</asp:Label>
                                        <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">維持不變。</asp:Label>
                                        </div>
                                    <br />
                                    <div style="display:inline-flex">
                                    <asp:RadioButton ID="rdoOVC_DOPEN_input" CssClass="radioButton rb-complex position-left" runat="server" />
                                        <asp:Label CssClass="control-label" runat="server">本案開標日為：</asp:Label>
                                        <asp:Label ID="lblOVC_DOPEN_input" CssClass="control-label position-left" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">，延至</asp:Label>
                                        <div class='input-append datetimepicker'>
                                            <asp:TextBox ID="txtOVC_DOPEN" CssClass='tb tb-m position-left' readonly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">辦理。</asp:Label>
                                        </div>
                                    <br />
                                    <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DESC" CssClass="tb tb-full" runat="server"></asp:TextBox>
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
                                <td><asp:Label CssClass="control-label position-left text-red" runat="server">1.刊登</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <!--↓日期套件↓-->
                                        <div class="input-append date" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DANNOUNCE" CssClass="tb tb-s position-left" readonly="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <!--↑日期套件↑-->
                                        <asp:Label CssClass="control-label position-left text-red" runat="server">政府採購公報乙日。</asp:Label><br />
                                    </td>
                            </tr>
                            <tr class="no-bordered no-bordered-seesaw">
                                <td>
                                    <asp:Label CssClass="control-label position-left text-red" runat="server">2.奉核後辦理後續公告事宜。</asp:Label></td>
                            </tr>
                            </table>
                            <table class="table table-bordered text-center" style="margin-top:0px">
                             <tr class="no-bordered">
                                <td style="width:45%" class="text-right no-r">
                                    <asp:Label CssClass="control-label text-red" runat="server">主管批核日</asp:Label>
                                </td>
                                <td class="no-l">
                                    <div class='input-append datetimepicker'>
                                            <asp:TextBox ID="txtOVC_DAPPROVE" CssClass='tb tb-m position-left' readonly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="存檔" />
                            <asp:Button ID="btnReturnC" CssClass="btn-warning btnw8" runat="server" Text="回修正公告選擇畫面" />
                            <asp:Button ID="btnReturnP" CssClass="btn-warning btnw6" runat="server" Text="回公告畫面" />
                            <asp:Button ID="btnReturnM" CssClass="btn-warning btnw6" runat="server" Text="回主流程畫面" />
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

