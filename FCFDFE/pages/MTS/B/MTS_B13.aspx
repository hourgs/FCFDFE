<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B13.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B13" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>結報申請表-新增</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_INF_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">發文字號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtISSU_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預算科目及編號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_BUDGET" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 35%">
                                        <asp:TextBox ID="txtOVC_PURPOSE_TYPE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server">結報申請日期</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 35%">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_APPLY_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預算通知書編號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_BUDGET_INF_NO" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">收據號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_INV_NO" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">收據日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INV_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_NOTE" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">保險公司</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpCO_SN" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_PLN_CONTENT" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button OnClick="btnNew_Click" CssClass="btn-success btnw8" Text="新增結報申請表" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
