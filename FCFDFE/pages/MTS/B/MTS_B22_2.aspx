<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B22_2.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B22_2" %>
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
                    <asp:Label ID="lbltitle" runat="server">投保通知書管理-修改</asp:Label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">投保通知書編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_EINN_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_EDF_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案號或採購文號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">物資名稱及數量</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CHI_NAME" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" Text="/" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_ITEM_COUNT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" Text="件" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">物資價值</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_ITEM_VALUE" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblONB_CARRIAGE_CURRENCY_Text" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:HiddenField ID="lblONB_CARRIAGE_CURRENCY" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">投保金額</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_INS_AMOUNT" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblONB_CARRIAGE_CURRENCY2_Text" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:HiddenField ID="lblONB_CARRIAGE_CURRENCY2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">投保日期</asp:Label>
                                    </td>
                                    <td>
                                        <%--<div class="input-append date" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtInsDate" CssClass="tb tb-s" readonly="true" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>--%>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INS_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險條件</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="chkOVC_INS_CONDITION" CssClass="radioButton" OnSelectedIndexChanged="chkOVC_INS_CONDITION_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:CheckBoxList> 
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險費率</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_INS_RATE" CssClass="tb tb-s" OnTextChanged="txtONB_INS_RATE_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <asp:Label Text="%" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">運輸工具</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_SEA_OR_AIR" CssClass="control-label" Text="SeaOrAirLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">起運港口</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_START_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">目的港口</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_ARRIVE_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">CessionNo</asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:Label ID="lblOVC_CESSION_NO" CssClass="control-label" runat="server"></asp:Label>--%>
                                        <asp:TextBox ID="txtOVC_CESSION_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保費 支付 方法</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" Text="保費逕向" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtOVC_PAYMENT_TYPE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label  CssClass="control-label" Text="收取" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" Text="(若空白表示項本室收取)" ForeColor="Red" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保費</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_FINAL_INS_AMOUNT" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" Text="(台幣)" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" Text="附註：" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" Text="一、本通知書僅供保險費核計之用" runat="server"></asp:Label><br />
                                        <asp:Label CssClass="control-label" Text="附註：" ForeColor="White" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" Text="二、如有錯誤請於文到一周內申復" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnSave" cssclass="btn-warning" OnClick="btnSave_Click" Text="更新投保通知書" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
