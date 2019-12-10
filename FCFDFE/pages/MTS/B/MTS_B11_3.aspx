<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B11_3.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B11_3" %>
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
                    <!--標題-->
                    <div>投保通知書 新增Step2 填寫投保通知書資料</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label text-red" runat="server">（此欄位輸入後僅可刪除，無法進入管理作業修改!）</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">案號或採購文號</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">  
                                        <asp:TextBox ID="txtPURCH_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">物資價值</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:TextBox ID="txtONB_ITEM_VALUE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:DropDownList ID="drpONB_CARRIAGE_CURRENCY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">投保金額</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:TextBox ID="txtONB_INS_AMOUNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:DropDownList ID="drpONB_CARRIAGE_CURRENCY2" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">投保日期</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INS_DATE" CssClass="tb tb-date" OnTextChanged="txtODT_INS_DATE_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">交貨和保險條件</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:DropDownList ID="drpOVC_DELIVERY_CONDITION" CssClass="tb tb-s" OnSelectedIndexChanged="drpOVC_DELIVERY_CONDITION_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label ID="lblOVC_INS_CONDITION" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍售或商購</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:DropDownList ID="drpOVC_PURCHASE_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%">
                                        <asp:Label CssClass="control-label" runat="server">保險公司</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:30%">
                                        <asp:Label ID="lblCompany" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td style="width:20%">
                                        <asp:Label CssClass="control-label" runat="server">保險費率</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:30%">
                                        <asp:TextBox ID="txtONB_INS_RATE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">
                                            附註 : 一、本通知書僅供保險費核計之用<br />&emsp;&emsp;&ensp;
                                            二、如有錯誤請於文到一週內申復
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button CssClass="btn-success" Text="新增投保通知書" OnClick="btnNew_Click" runat="server"/>
                    <asp:Button CssClass="btn-success" Text="回首頁" OnClick="btnHome_Click" runat="server"/>
                </footer>
            </section>
        </div>
    </div>
</asp:Content>

