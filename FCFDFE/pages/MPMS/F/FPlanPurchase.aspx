<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FPlanPurchase.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FPlanPurchase" %>

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
        }
        .lbl-s {
            display: inline-block;
            width: 100px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:Label CssClass="control-label lbl-title" runat="server"><%=strDEPT_Name%></asp:Label>
                    <asp:Label CssClass="control-label lbl-title" runat="server">採購發包系統</asp:Label>
                    <asp:Label CssClass="control-label lbl-title text-red" runat="server">統計分析報表主畫面</asp:Label>
                    <%--<asp:Button ID="btnGohome" CssClass="btn-success btnw4" runat="server" Text="回主畫面" />--%><!--綠色-->
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnQuery" runat="server">
                            <!--網頁內容-->
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_PerPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpYear_PerPur" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購時程管制總表</asp:Label>
                                        （<asp:RadioButtonList ID="rdoType" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="All" Selected="True">全部</asp:ListItem>
                                            <asp:ListItem Value="Reserve">保留決標</asp:ListItem>
                                        </asp:RadioButtonList>）
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Uncheck_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpYear_Uncheck" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購已收辦未訂約統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_InnerPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtPurSta1_L" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtPurSta2_L" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">內購案標餘款情形統計表</asp:Label>
                                        <asp:Label CssClass="control-label text-blue" runat="server">註:以開標日期(即決標日期)作為查詢期間</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_OuterPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtPurSta1_W" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtPurSta2_W" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">外購案標餘款情形統計表</asp:Label>
                                        <asp:Label CssClass="control-label text-blue" runat="server">註:以開標日期(即決標日期)作為查詢期間</asp:Label>
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
