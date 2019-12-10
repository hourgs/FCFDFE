<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A2A_2.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A2A_2" %>
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
                    <div>軍品出口作業時程管制表-修改</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton CssClass="btn-success btnw8" OnClick="btnBack_Click" Text="回時程管制表管理" runat="server"></asp:LinkButton>
                            </div>
                            <table class="table table-bordered text-center">
                                <%--<tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">出口案號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">委運出口單位函文<br>申請時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_REQUIRE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:TextBox ID="txtOVC_REQUIRE_MSG_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接轉組接獲委運單位出口文件時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_RECEIVE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:TextBox ID="txtOVC_RECEIVE_MSG_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">採購室函文關務署<br>辦理免稅時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_PROCESS_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:TextBox ID="txtOVC_PROCESS_MSG_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">「戰略性高科技貨口輸入許可證」收辦時間<br>(非屬高科技品項申請鑑定報告或函文說明)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                       <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_STRATEGY_PROCESS_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">「戰略性高科技貨口輸入許可證」效期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                       <asp:Label ID="lblODT_VALIDITY_DATE" Visible="false" CssClass="control-label" runat="server"></asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_VALIDITY_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電請委運單位配合辦理進倉時間<br>(或空軍出口軍品送抵時間)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_TEL_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">進倉時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_STORED_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">報關時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_CUSTOM_DATE" Visible="false" CssClass="control-label" runat="server"></asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CUSTOM_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">通關時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_PASS_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">航輪(貨機)啟航時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_SHIP_START_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">出口案正本提單函復時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_RETURN_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">未依時限辦理說明</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOVC_DELAY_DESCRIPTION" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button ID="btnModify" CssClass="btn-warning btnw7" Text="更新時程管制表" OnClick="btnModify_Click" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:FileUpload ID="FileUpload" title="瀏覽..." runat="server" />
                    <asp:Button ID="btnUpload" CssClass="btn-warning btnw7" Text="上傳提單" OnClick="btnUpload_Click" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>