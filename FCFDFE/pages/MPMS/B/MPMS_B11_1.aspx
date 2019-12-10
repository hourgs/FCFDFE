<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B11_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B11_1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->複製購案主畫面
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <p class="text-center">
                                <br><br>
                                <asp:Button ID="btnReturnMain" cssclass="btn-success btnw4" runat="server" Text="回主畫面" />
                                <asp:Label CssClass="control-label text-blue" runat="server" Text="為縮短作業流程，可從以下條件挑選參考範例後複製購案"></asp:Label>
                                <br>
                            </p>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label text-red" runat="server" Text="選擇"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label text-red" runat="server" Text="條件"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="ChkAll" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left"><asp:Label CssClass="control-label" runat="server" Text="顯示所有可複製之購案範例"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOVC_LAB" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server" Text="採購屬性："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_LAB" CssClass="tb tb-m" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOVC_PUR_ASS_VEN_CODE" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server" Text="招標方式："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-l" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOVC_BID_TIMES" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server" Text="投標段次："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_BID_TIMES" CssClass="tb tb-m" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOVC_BID" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server" Text="決標原則："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_BID" CssClass="tb tb-m" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOVC_PUR_AGENCY" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server" Text="採購途徑："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_PUR_AGENCY" CssClass="tb tb-m" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOVC_AGENT_UNIT" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server" Text="招標單位："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_AGENT_UNIT" CssClass="tb tb-m" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOVC_PURCH" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server" Text="購案編號："></asp:Label>
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label text-blue" runat="server" Text="(購案編號第一組至第三組合計七碼)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOVC_PUR_IPURCH" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server" Text="購案名稱："></asp:Label>
                                        <asp:TextBox ID="txtOVC_PUR_IPURCH" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkOtherAuth" CssClass="radioButton" runat="server" /></td>
                                    <td class="text-left"><asp:Label CssClass="control-label" runat="server" Text="授權開放的其他單位"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnQuery" cssclass="btn-success btnw2" runat="server" Text="尋找" />
                                        <asp:Label CssClass="control-label text-red" runat="server" Text="以上條件左邊打勾者做(AND)交集查詢"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
