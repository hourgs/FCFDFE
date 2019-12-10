<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B21_2.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B21_2" %>
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
                    投保通知書-新增-Step2 填寫投保通知書資料
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
                                    <td colspan="5">
                                        <%--<asp:TextBox ID="txtOVC_EINN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>--%>
                                        <asp:Label ID="lblOVC_EINN_NO" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" Text="自動產生" ForeColor="Red" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblOVC_EDF_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案號或採購文號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">物資名稱及數量</asp:Label>
                                    </td>
                                    <td colspan="5">
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
                                    <td colspan="5">
                                        <asp:Label ID="lblONB_ITEM_VALUE" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblONB_CARRIAGE_CURRENCY_Text" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:HiddenField ID="lblONB_CARRIAGE_CURRENCY" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">投保金額</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblONB_INS_AMOUNT" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblONB_CARRIAGE_CURRENCY2_Text" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:HiddenField ID="lblONB_CARRIAGE_CURRENCY2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">投保日期</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <%--<div class="input-append date" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOvcEinnNo1" CssClass="tb tb-s" readonly="true" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>--%>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INS_DATE" CssClass="tb tb-date" OnTextChanged="txtODT_INS_DATE_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險條件</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:CheckBoxList ID="chkOVC_INS_CONDITION" CssClass="radioButton" OnSelectedIndexChanged="chkOVC_INS_CONDITION_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="width:20%">
                                        <asp:Label CssClass="control-label" runat="server">保險公司</asp:Label>
                                    </td>
                                    <td colspan="2" style="width:30%">
                                        <asp:Label ID="lblCompany" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td class="text-center" style="width:20%">
                                        <asp:Label CssClass="control-label" runat="server">保險費率</asp:Label>
                                    </td>
                                    <td colspan="2" style="width:30%">
                                        <asp:TextBox ID="txtONB_INS_RATE" CssClass="tb tb-m" Visible="false" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblONB_INS_RATE" CssClass="control-label" Text="" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">運輸工具</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblOVC_SEA_OR_AIR" CssClass="control-label" Text="SeaOrAirLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">啟運港口</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblOVC_START_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label  CssClass="control-label" runat="server">目的港口</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblOVC_ARRIVE_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險 支付 方法</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label CssClass="control-label" Text="一、保費向本室收取" runat="server"></asp:Label>&nbsp&nbsp;&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" Text="二、保費逕向" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtOVC_PAYMENT_TYPE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label Text="收取" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr> 
                                    <td colspan="6">
                                        <asp:Label CssClass="control-label" Text="附註：" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" Text="一、本通知書僅供保險費核計之用" runat="server"></asp:Label><br />
                                        <asp:Label CssClass="control-label" Text="附註：" ForeColor="White" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" Text="二、如有錯誤請於文到一周內申復" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                               <asp:Button cssclass="btn-warning" OnClick="BtnSave_Click" Text="新增投保通知書" runat="server" /><br /><br />
                               <%--<asp:LinkButton ForeColor="#3333FF" runat="server">繼續新增投保投保通知書</asp:LinkButton><br />--%>
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
