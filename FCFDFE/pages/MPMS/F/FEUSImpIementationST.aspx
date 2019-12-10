<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FEUSImpIementationST.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FEUSImpIementationST" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .lbl-title {
            display: block;
            font-size: 32px;
        }
        .lbl-subtitle {
            text-align: left;
            padding-left: 20px;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:Label CssClass="lbl-title" runat="server"><%=strDEPT_Name%>主官查詢系統</asp:Label>
                    <div class="lbl-subtitle">
                        <asp:Label CssClass="" runat="server">駐美、歐組購案履驗階段執行現況明細表</asp:Label>
                    </div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkYear" CssClass="radioButton text-blue" Checked="true" Text="年度：" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DELIVERY" CssClass="radioButton text-blue" Text="交貨日期：" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DELIVERY1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DELIVERY2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DJOINCHECK" CssClass="radioButton text-blue" Text="履驗日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_DO_DATE" CssClass="radioButton text-blue" Text="履驗日期：" runat="server"></asp:CheckBox>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DJOINCHECK1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DJOINCHECK2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DPAY" CssClass="radioButton text-blue" Text="最後付款日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_FINAL_PAY" CssClass="radioButton text-blue" Text="最後付款日期：" runat="server"></asp:CheckBox>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DPAY1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DPAY2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DCLOSE" CssClass="radioButton text-blue" Text="驗結日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_RECEIVE" CssClass="radioButton text-blue" Text="驗結日期：" runat="server"></asp:CheckBox>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DCLOSE1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DCLOSE2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkOVC_NOEND" CssClass="radioButton text-blue" Text="久懸未決(逾5年以上)" runat="server"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                                        <asp:Button CssClass="btn-success btnw4 textSpace-l" OnClick="btnGoBack_Click" Text="回上一頁" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
