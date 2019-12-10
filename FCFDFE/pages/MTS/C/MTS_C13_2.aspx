<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_C13_2.aspx.cs" Inherits="FCFDFE.pages.MTS.C.MTS_C13_2" %>
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
                    保險理賠建立與管理-修改
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
                                    <td style="width: 35%;">
                                        <asp:Label ID="lblOVC_CLAIM_NO" CssClass="control-label" runat="server"></asp:Label>
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
                                        <asp:Label CssClass="control-label" runat="server">理賠日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_COMPENSATION_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">實際理賠金額</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_COMPENSATION_AMOUNT_2" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>
                                        <asp:DropDownList ID="drpOVC_COMPENSATION_CURRENCY_2" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">理賠文號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_COMPENSATION_MSG_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
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
                                        <asp:Label CssClass="control-label" runat="server">理賠金額</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_COMPENSATION_AMOUNT" CssClass="tb tb-m" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:DropDownList ID="drpOVC_COMPENSATION_CURRENCY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
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
                                    <td  class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠情形</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_CLAIM_CONDITION" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票銀行</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_BANK" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票號碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票抬頭</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_TITLE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_CHEQUE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
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
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-warning btnw2" OnClick="btnSave_Click" Text="更新" runat="server" /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button CssClass="btn-warning" OnClick="btnBack_Click" Text="回前頁" runat="server" /><br /><br />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
