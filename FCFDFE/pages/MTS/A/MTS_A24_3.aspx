<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A24_3.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A24_3" %>
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
                    <div>出口物資訂艙單-刪除</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnDelete" runat="server"></asp:Panel>
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton CssClass="btn-success btnw6" OnClick="btnBack_Click" Text="回訂艙單管理" runat="server"></asp:LinkButton>
                            </div>
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
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server" ></asp:Label>
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
                                    <td><asp:Label CssClass="control-label" runat="server">三軍申購文號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCH_MSG_NO1" CssClass="control-label" runat="server" ></asp:Label>
                                        <asp:Label ID="lblOVC_PURCH_MSG_NO2" CssClass="control-label" runat="server" ></asp:Label>字&emsp;第
                                        <asp:Label ID="lblOVC_PURCH_MSG_NO3" CssClass="control-label" runat="server" ></asp:Label>號函
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">國防部採購室函文關稅局<br>辦理免稅文號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PROCESS_NO1" CssClass="control-label" runat="server" ></asp:Label>
                                        <asp:Label ID="lblOVC_PROCESS_NO2" CssClass="control-label" runat="server" ></asp:Label>字&emsp;第
                                        <asp:Label ID="lblOVC_PROCESS_NO3" CssClass="control-label" runat="server" ></asp:Label>號函
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">船(航空)公司</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_SHIP_COMPANY" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">S/O No.</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_SO_NO" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">船機名</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_SHIP_NAME" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">航次</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_VOYAGE" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">裝貨區分</asp:Label></td>
                                    <td class="text-left">     
                                        <asp:Label ID="lblOVC_CONTAINER_TYPE" CssClass="control-label" runat="server"></asp:Label>&emsp;&emsp;
                                        20公尺 : <asp:Label ID="lblONB_20_COUNT" CssClass="control-label" runat="server" ></asp:Label>&emsp;
                                        40公尺 : <asp:Label ID="lblONB_40_COUNT" CssClass="control-label" runat="server" ></asp:Label>&emsp;
                                        45公尺 : <asp:Label ID="lblONB_45_COUNT" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">進艙時間</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_STORED_DATE" CssClass="control-label" runat="server" ></asp:Label>                                
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">開航日</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_START_DATE" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">預計抵達日</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_ACT_ARRIVE_DATE" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物存放處所</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_STORED_PLACE" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">結關時間</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_CUSTOM_CLR_DATE" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">進艙廠商</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_STORED_COMPANY" CssClass="control-label" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <asp:Button ID="btnDel" CssClass="btn-success btnw5" Text="刪除訂艙單" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" OnClick="btnDel_Click" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>