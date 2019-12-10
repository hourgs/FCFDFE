<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FUnitPurchaseST.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FUnitPurchaseST" %>

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
                        <asp:Label CssClass="" runat="server">各單位購案統計表</asp:Label>
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
                                    <td style="width:25%" class="text-blue text-right">

                                        <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-blue text-right">
                                        <asp:Label CssClass="control-label" runat="server">統計方式</asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:RadioButtonList ID="rdoMethod" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="OVC_PUR_APPROVE_DEP" Selected="True">核定權責</asp:ListItem>
                                            <asp:ListItem Value="OVC_PUR_AGENCY">採購方式</asp:ListItem>
                                            <asp:ListItem Value="OVC_PUR_ASS_VEN_CODE">招標方式</asp:ListItem>
                                            <asp:ListItem Value="OVC_LAB">採購屬性</asp:ListItem>
                                            <asp:ListItem Value="OVC_PLAN_PURCH">計畫性質</asp:ListItem>
                                            <asp:ListItem Value="AllUnit">全部單位</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                                        <asp:Button CssClass="btn-success btnw6" OnClick="btnGoBack_Click" Text="回上一頁" runat="server" />
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
