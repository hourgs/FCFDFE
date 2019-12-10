<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A23_3.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A23_3" %>
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
                    <div>出口物資訂艙單-Step2 填寫訂艙單資料</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton CssClass="btn-success btnw6" OnClick="btnBack_Click" Text="回訂艙單建立" runat="server"></asp:LinkButton>
                            </div>
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_EDF_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">案號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">啟運港(機場)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_START_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">目的港(機場)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ARRIVE_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label text-star" runat="server">三軍申購文號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_PURCH_MSG_NO1_Y" CssClass="tb drp-year" OnSelectedIndexChanged="drpOVC_PURCH_MSG_NO1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                        <asp:DropDownList ID="drpOVC_PURCH_MSG_NO1_M" CssClass="tb drp-day" OnSelectedIndexChanged="drpOVC_PURCH_MSG_NO1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">月</asp:Label>
                                        <asp:DropDownList ID="drpOVC_PURCH_MSG_NO1_D" CssClass="tb drp-day" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">日</asp:Label>

                                        <asp:TextBox ID="txtOVC_PURCH_MSG_NO2" CssClass="tb tb-s textSpace-l" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">字&emsp;第</asp:Label>
                                        <asp:TextBox ID="txtOVC_PURCH_MSG_NO3" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">號函</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">國防部採購室函文關稅局<br>辦理免稅文號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_PROCESS_NO1_Y" CssClass="tb drp-year" OnSelectedIndexChanged="drpOVC_PROCESS_NO1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                        <asp:DropDownList ID="drpOVC_PROCESS_NO1_M" CssClass="tb drp-day" OnSelectedIndexChanged="drpOVC_PROCESS_NO1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">月</asp:Label>
                                        <asp:DropDownList ID="drpOVC_PROCESS_NO1_D" CssClass="tb drp-day" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">日</asp:Label>

                                        <asp:TextBox ID="txtOVC_PROCESS_NO2" CssClass="tb tb-s textSpace-l" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">字&emsp;第</asp:Label>
                                        <asp:TextBox ID="txtOVC_PROCESS_NO3" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">號函</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">船(航空)公司</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label text-star" runat="server">S/O No.</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SO_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnNew" CssClass="btn-warning btnw5" Text="新增訂艙單" OnClick="btnNew_Click" runat="server"/>
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <asp:LinkButton ID="btnBack" OnClick="btnBack_Click" Visible="false" Text="繼續新增訂艙單" runat="server"></asp:LinkButton>
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
