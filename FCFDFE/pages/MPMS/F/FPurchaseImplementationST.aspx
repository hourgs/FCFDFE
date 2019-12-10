<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FPurchaseImplementationST.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FPurchaseImplementationST" %>
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
                        <asp:Label CssClass="" runat="server">購案執行進度明細表</asp:Label>
                    </div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnField" runat="server">
                            <!--網頁內容-->
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:25%">
                                        <asp:CheckBox ID="chkYear" CssClass="radioButton text-blue" Checked="true" Text="年度：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">年度：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_SECTION" CssClass="radioButton text-blue" Text="申購單位：" runat="server"></asp:CheckBox>
                                        <asp:Label CssClass="control-label" runat="server">：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_SECTION" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-red" runat="server">（含下屬單位）</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_APPROVE_DEP" CssClass="radioButton text-blue" Text="核定權責：" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_APPROVE_DEP" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_LAB" CssClass="radioButton text-blue" Text="採購屬性：" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_LAB" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_AGENCY" CssClass="radioButton text-blue" Text="採購途徑：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_ITEM" CssClass="radioButton text-blue" Text="採購途徑：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">採購途徑：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_AGENCY" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        <%--<asp:DropDownList ID="drpOVC_ITEM" CssClass="tb tb-l" runat="server"></asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_ASS_VEN_CODE" CssClass="radioButton text-blue" Text="招標方式：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">招標方式：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PLAN_PURCH" CssClass="radioButton text-blue" Text="計畫性質：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">計畫性質：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PLAN_PURCH" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DPROPOSE" CssClass="radioButton text-blue" Text="申購日期：" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DPROPOSE1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DPROPOSE2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_DAPPROVE" CssClass="radioButton text-blue" Text="核定日期：" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_DAPPROVE1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_DAPPROVE2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td colspan="2" class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                                        <asp:Button CssClass="btn-success btnw4 textSpace-l" OnClick="btnGoBack_Click" Text="回上一頁" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
