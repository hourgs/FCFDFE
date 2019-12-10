<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E34.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E34" %>
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
                <header  class="title text-red">
                    <!--標題-->契約修訂
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table id="tb" class="table table-bordered text-center" runat="server">
                                <tr>
                                    <td style="width:165px;" class="text-center"><asp:Label CssClass="control-label" Text="購案編號" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label CssClass="control-label" ID="labOVC_PURCH" runat="server"></asp:Label></td>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="修約次數" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label CssClass="control-label" ID="labONB_TIMES" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="計畫申購單位" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label CssClass="control-label" ID="labOVC_PUR_NSECTION" runat="server"></asp:Label></td>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="購案名稱" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label CssClass="control-label" ID="labOVC_PUR_IPURCH" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="契約金額" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label CssClass="control-label" Text="新台幣" runat="server"></asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" ID="labONB_MONEY" runat="server"></asp:Label></td>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="決標日期" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label CssClass="control-label" ID="labOVC_DBID" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="廠商名稱" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label CssClass="control-label" ID="labOVC_VEN_TITLE" runat="server"></asp:Label></td>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="簽約日期" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label CssClass="control-label" ID="labOVC_DCONTRACT" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="修約日期" runat="server"></asp:Label></td>
                                    <td colspan="5" class="text-left">
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DMODIFY" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear" CssClass="btn-default btnw4 position-left" OnClick="btnClear_Click" runat="server" Text="清除日期" /></td>
                                </tr>
                                <tr>
                                    <td rowspan="4" class="text-center"><asp:Label CssClass="control-label" Text="修約原因" runat="server"></asp:Label></td>
                                    <td class="text-left"><asp:RadioButton ID="rb01" CssClass="radioButton" Text="契約項目追加(減)" OnCheckedChanged="rb01_CheckedChanged" AutoPostBack="true" runat="server"/></td>
                                    <td class="text-left"><asp:RadioButton ID="rb02" CssClass="radioButton" Text="數量追加(減)" OnCheckedChanged="rb02_CheckedChanged" AutoPostBack="true" runat="server"/></td>
                                    <td class="text-left"><asp:RadioButton ID="rb03" CssClass="radioButton" Text="材料規格變更" OnCheckedChanged="rb03_CheckedChanged" AutoPostBack="true" runat="server"/></td>
                                    <td class="text-left"><asp:RadioButton ID="rb04" CssClass="radioButton" Text="履約期限變更" OnCheckedChanged="rb04_CheckedChanged" AutoPostBack="true" runat="server"/></td>
                                    <td class="text-left"><asp:RadioButton ID="rb05" CssClass="radioButton" Text="物價指數調整" OnCheckedChanged="rb05_CheckedChanged" AutoPostBack="true" runat="server"/></td>
                                </tr>
                                <tr>
                                    <td class="text-left"><asp:RadioButton ID="rb06" CssClass="radioButton" Text="價金給付條件" OnCheckedChanged="rb06_CheckedChanged" AutoPostBack="true" runat="server"/></td>
                                    <td class="text-left"><asp:RadioButton ID="rb07" CssClass="radioButton" Text="驗收條件" OnCheckedChanged="rb07_CheckedChanged" AutoPostBack="true" runat="server"/></td>
                                    <td class="text-left"></td>
                                    <td class="text-left"></td>
                                    <td class="text-left"></td>
                                </tr>
                                <tr>
                                    <td colspan="5" class="text-left"><asp:RadioButton ID="rb08" CssClass="radioButton" Text="其他契約條款變更(空白欄位輸入)" OnCheckedChanged="rb08_CheckedChanged" AutoPostBack="true" runat="server"/><br />
                                        <asp:TextBox ID="txtOVC_REASON_MODIFY" CssClass="tb tb-full" Height="80px" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" class="text-left">
                                        <asp:Label CssClass="control-label" Text="變更金額：" runat="server"></asp:Label>
                                        <asp:RadioButtonList ID="rdOVC_KIND" CssClass="radioButton" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Value="+">金額增加</asp:ListItem>
                                            <asp:ListItem Value="-">金額減少</asp:ListItem>
                                        </asp:RadioButtonList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" Text="金額：" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtONB_MODIFIED" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="品項類別" runat="server"></asp:Label></td>
                                    <td colspan="2" class="text-left">
                                        <asp:RadioButtonList ID="rbOVC_MAIN_SUB" CssClass="radioButton" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Value="1">主要品項</asp:ListItem>
                                            <asp:ListItem Value="2">次要品項</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td colspan="1" class="text-left"></td>
                                    <td colspan="2" class="text-left"></td>
                                </tr>
                                <tr>
                                    <td class="text-center"><asp:Label CssClass="control-label" Text="採購契約變更或加減價核准監辦備查規定一覽表" runat="server"></asp:Label></td>
                                    <td colspan="5" class="text-left">
                                        <asp:DropDownList ID="drpOVC_LAW_NO" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="1">第一項</asp:ListItem>
                                            <asp:ListItem Value="2">第二項</asp:ListItem>
                                            <asp:ListItem Value="3">第三項</asp:ListItem>
                                            <asp:ListItem Value="4">第四項</asp:ListItem>
                                            <asp:ListItem Value="5">第五項</asp:ListItem>
                                            <asp:ListItem Value="6">第六項</asp:ListItem>
                                            <asp:ListItem Value="7">第七項</asp:ListItem>
                                        </asp:DropDownList>
                                        <br /><br />
                                        <table class="table table-bordered text-center">
                                            <tr>
                                                <td><asp:Label CssClass="control-label" Text="項次" runat="server"></asp:Label></td>
                                                <td><asp:Label CssClass="control-label" Text="契約變更或加減價情形" runat="server"></asp:Label></td>
                                                <td><asp:Label CssClass="control-label" Text="核准規定" runat="server"></asp:Label></td>
                                                <td><asp:Label CssClass="control-label" Text="監辦規定" runat="server"></asp:Label></td>
                                                <td><asp:Label CssClass="control-label" Text="備查規定" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td ><asp:Label CssClass="control-label" Text="一" runat="server"></asp:Label></td>
                                                <td style="width:180px;"><asp:Label CssClass="control-label" Text="辦理契約變更，不論原契約金額大小，其變更部分之累計金額未達公告金額。" runat="server"></asp:Label></td>
                                                <td style="width:180px;"><asp:Label CssClass="control-label" Text="一、適用未達公告金額採購招標辦法之核准規定。" runat="server"></asp:Label>
                                                    <br /><br />
                                                    <asp:Label CssClass="control-label" Text="二、有政府採購法（以下簡稱採購法）第七十二條第二項減價收受之情形者，從其規定。" runat="server"></asp:Label>
                                                </td>
                                                <td style="width:180px;"><asp:Label CssClass="control-label" Text="採購法第十三條第二項監辦規定" runat="server"></asp:Label></td>
                                                <td style="width:180px;"><asp:Label CssClass="control-label" Text="查核金額以上之採購，補具辦理結果之相關文件送上級機關備查。上級機關得決定免送備查。契約價金變更後屬查核金額以上之採購者，除本表2第四項之情形外，亦同。" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnBack" CssClass="btn-success" OnClick="btnBack_Click" runat="server" Text="回上一頁" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReturn" CssClass="btn-success" OnClick="btnReturn_Click" runat="server" Text="回主流程" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnSave" CssClass="btn-success" runat="server" OnClick="btnSave_Click" Text="存檔" />
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
