<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FClosedComparisonT.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FClosedComparisonT" %>
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
                        <asp:Label CssClass="" runat="server">澄覆比較表</asp:Label>
                    </div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width: 180px;">
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案編號：</asp:Label>
                                        <asp:Label CssClass="control-label text-red" runat="server">(前七碼)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <%--<asp:Button ID="btnGet" CssClass="btn-success btnw6" runat="server" Text="取得版本時間" />--%>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">版本時間(1)：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="textVersionTime1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                        <asp:DropDownList ID="drpyear1" CssClass="tb tb-l" runat="server">
                                            <asp:ListItem>2017-04-05 14:08:44 下午</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <%--<tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">版本時間(2)：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="textVersionTime2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                        <asp:DropDownList ID="drpyear2" CssClass="tb tb-l" runat="server">
                                            <asp:ListItem>2017-04-05 14:08:44 下午</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td colspan="2" class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                                        <asp:Button CssClass="btn-success btnw4" OnClick="btnGoBack_Click" Text="回上一頁" runat="server" />
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