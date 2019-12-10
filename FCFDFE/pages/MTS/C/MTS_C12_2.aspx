<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_C12_2.aspx.cs" Inherits="FCFDFE.pages.MTS.C.MTS_C12_2" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                    國防部國防採購室外購案軍品索賠通知書管理-修改
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnMessageModify" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" style="width: 15%;">
                                         <asp:Label CssClass="control-label" runat="server">通知書編號</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblOVC_CLAIM_NO" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblOVC_CREATE_LOGIN_ID" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">申請單位</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server" />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-success btnw4" OnClientClick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" Text="申請單位" runat="server" />&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-default btnw4" OnClick="btnClearDept_Click" Text="資料清空" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-l text-toUpper" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-success btnw6" OnClick="btnLoad_Click" Text="載入提單資料" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="width: 15%;">
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center" style="width: 15%;">
                                        <asp:Label CssClass="control-label" runat="server">保險公司理賠文號</asp:Label>
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:TextBox ID="txtOVC_INS_COM_MSG" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種索賠日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CLAIM_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">實際理賠金額</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_COMPENSATION_AMOUNT" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>
                                        <asp:DropDownList ID="drpOVC_COMPENSATION_CURRENCY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種索賠字號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CLAIM_MSG_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">匯率</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_COMPENSATION_CURRENCY_RATE" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保單號碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_INN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">實際理賠金額(台幣)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_COMPENSATION_AMOUNT_NTD" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠軍品名稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CLAIM_ITEM" CssClass="textarea tb-full" TextMode="MultiLine" Rows="2" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票銀行</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_BANK" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠軍品數量</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_CLAIM_NUMBER" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">件</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票號碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">索賠軍品總額</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_CLAIM_AMOUNT" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:DropDownList ID="drpOVC_CLAIM_CURRENCY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票抬頭</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_TITLE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠原因</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CLAIM_REASON" CssClass="textarea tb-full" TextMode="MultiLine" Rows="2" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_CHEQUE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">索賠情形</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_CLAIM_CONDITION" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">理賠日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CLAIM_DATE_2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">需備索賠文件</asp:Label>
                                    </td>
                                    <td colspan="3">
                                         <asp:CheckBoxList ID="chkGET_FILE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_NOTE" CssClass="textarea tb-full" TextMode="MultiLine" Rows="2" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-warning" OnClick="btnUpdate_Click" Text="更新索賠通知書" runat="server" /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button CssClass="btn-warning" OnClick="btnBack_Click" Text="回結報申請表管理" runat="server" /><br /><br />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
