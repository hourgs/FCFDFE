<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A24_2.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A24_2" %>
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
                    <div>出口物資訂艙單-修改</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnUpdate" runat="server"></asp:Panel>
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton CssClass="btn-success btnw6" OnClick="btnBack_Click" Text="回訂艙單管理" runat="server"></asp:LinkButton>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_EDF_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">啟運港(機場)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_START_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">目的港(機場)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ARRIVE_PORT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">三軍申購文號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <%--<div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PURCH_MSG_NO1" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>--%>
                                        <asp:DropDownList ID="drpOVC_PURCH_MSG_NO1_Y" CssClass="tb drp-year" OnSelectedIndexChanged="drpOVC_PURCH_MSG_NO1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                        <asp:DropDownList ID="drpOVC_PURCH_MSG_NO1_M" CssClass="tb drp-day" OnSelectedIndexChanged="drpOVC_PURCH_MSG_NO1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">月</asp:Label>
                                        <asp:DropDownList ID="drpOVC_PURCH_MSG_NO1_D" CssClass="tb drp-day" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">日</asp:Label>

                                        <asp:TextBox ID="txtOVC_PURCH_MSG_NO2" CssClass="tb tb-s textSpace-l" runat="server"></asp:TextBox>字&emsp;第
                                        <asp:TextBox ID="txtOVC_PURCH_MSG_NO3" CssClass="tb tb-s" runat="server"></asp:TextBox>號函
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">國防部採購室函文關稅局<br>辦理免稅文號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <%--<div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PROCESS_NO1" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>--%>
                                        <asp:DropDownList ID="drpOVC_PROCESS_NO1_Y" CssClass="tb drp-year" OnSelectedIndexChanged="drpOVC_PROCESS_NO1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                        <asp:DropDownList ID="drpOVC_PROCESS_NO1_M" CssClass="tb drp-day" OnSelectedIndexChanged="drpOVC_PROCESS_NO1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">月</asp:Label>
                                        <asp:DropDownList ID="drpOVC_PROCESS_NO1_D" CssClass="tb drp-day" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">日</asp:Label>

                                        <asp:TextBox ID="txtOVC_PROCESS_NO2" CssClass="tb tb-s textSpace-l" runat="server"></asp:TextBox>字&emsp;第
                                        <asp:TextBox ID="txtOVC_PROCESS_NO3" CssClass="tb tb-s" runat="server"></asp:TextBox>號函
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船(航空)公司</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">S/O No.</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SO_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>&emsp;&emsp;&emsp;
                                        <asp:Label CssClass="control-label" runat="server">環世提單號碼</asp:Label>
                                        <asp:TextBox ID="txt" CssClass="tb tb-m" runat="server"></asp:TextBox><br>
                                        <asp:CheckBox ID="chkbld" CssClass="radioButton text-red" Checked="true" Text="建立提單" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機名</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">航次</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_VOYAGE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">裝貨區分</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_CONTAINER_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>&emsp;&emsp;

                                        <asp:Label CssClass="control-label" runat="server">20公尺 :</asp:Label>
                                        <asp:TextBox ID="txtONB_20_COUNT" CssClass="tb tb-xs" runat="server"></asp:TextBox>&emsp;

                                        <asp:Label CssClass="control-label" runat="server">40公尺 :</asp:Label>
                                        <asp:TextBox ID="txtONB_40_COUNT" CssClass="tb tb-xs" runat="server"></asp:TextBox>&emsp;

                                        <asp:Label CssClass="control-label" runat="server">45公尺 :</asp:Label>
                                        <asp:TextBox ID="txtONB_45_COUNT" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">進艙時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_STORED_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:DropDownList ID="drpODT_STORED_DATE_H" CssClass="tb drp-day" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label textSpace-l textSpace-r" runat="server">:</asp:Label>
                                        <asp:DropDownList ID="drpODT_STORED_DATE_M" CssClass="tb drp-day" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">開航日</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_START_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預計抵達日</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_ACT_ARRIVE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">貨物存放處所</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_CUSTOM_CLR_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">結關時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CUSTOM_CLR_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">進艙廠商</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_STORED_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label textSpace-l" runat="server">註：原欣隆儲運請改選欣隆倉儲物流</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button ID="btnModify" CssClass="btn-warning btnw5" Text="更新訂艙單" OnClick="btnModify_Click" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>