<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A18_3.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A18_3" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <asp:Panel ID="PnMessage_Script" runat="server"></asp:Panel>

    <div class="row">
        <div style="width: 1000px; margin:auto;">
        <section class="panel">
            <header  class="title">
                <!--標題-->
                <div>進口物資管制接配紀錄表-刪除</div>
            </header>
            <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
            <div class="panel-body" style=" border: solid 2px;">
                <div class="form" style="border: 5px;">
                    <div class="cmxform form-horizontal tasi-form">
                        <!--網頁內容-->
                        <table class="table table-bordered text-center">
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">應收件數/單位</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblONB_QUANITY" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_QUANITY_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">船(機)名</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_SHIP_NAME" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">重量/單位</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblONB_WEIGHT" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_WEIGHT_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">航次</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_VOYAGE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">體積/單位</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblONB_VOLUME" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_VOLUME_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">購案號</asp:Label>
                                </td>
                                <td colspan="3" class="text-left">
                                    <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">貨櫃號碼</asp:Label><br><br>
                                </td>
                                <td colspan="3" class="td-inner-table">
                                    <asp:GridView ID="GVTBGMT_CTN" DataKeyNames="OVC_CONTAINER_NO" CssClass="table table-striped border-top table-inner" AutoGenerateColumns="false" OnPreRender="GVTBGMT_CTN_PreRender" runat="server">
                                        <Columns>
                                            <asp:BoundField HeaderText="貨櫃號碼" DataField="OVC_CONTAINER_NO"></asp:BoundField>
                                            <asp:BoundField HeaderText="尺寸" DataField="ONB_SIZE"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">進口日期</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblODT_ARRIVE_PORT_DATE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">提單/拆櫃日期</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblODT_CLEAR_DATE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">清驗情形</asp:Label>
                                </td>
                                <td colspan="3" class="td-inner-table">
                                    <table class="table table-bordered table-inner">
                                        <tr>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">實收</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">溢卸</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">短少</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">破損</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblONB_ACTUAL_RECEIVE" CssClass="control-label" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblONB_OVERFLOW" CssClass="control-label" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblONB_LESS" CssClass="control-label" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblONB_BROKEN" CssClass="control-label" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                </td>
                                <td colspan="3" class="text-left">
                                    <asp:Label ID="lblOVC_NOTE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">分運明細</asp:Label>
                                </td>
                                <td colspan="3" class="td-inner-table">
                                    <asp:GridView ID="GVTBGMT_DETAIL" DataKeyNames="OVC_IRDDETAIL_SN" CssClass="table table-striped border-top table-inner" AutoGenerateColumns="false" OnPreRender="GVTBGMT_DETAIL_PreRender" runat="server">
                                        <Columns>
                                            <asp:BoundField HeaderText="分運單位" DataField="OVC_ONNAME" />
                                            <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" />
                                            <asp:BoundField HeaderText="實收" DataField="ONB_ACTUAL_RECEIVE" />
                                            <asp:BoundField HeaderText="溢卸" DataField="ONB_OVERFLOW" />
                                            <asp:BoundField HeaderText="短少" DataField="ONB_LESS" />
                                            <asp:BoundField HeaderText="破損" DataField="ONB_BROKEN" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button CssClass="btn-warning" Text="刪除接配紀錄表" OnClick="btnDelete_Click" runat="server" />
                            <asp:Button CssClass="btn-warning" Text="回首頁" OnClick="btnHome_Click" runat="server" />

                        </div>
                    </div>
                </div>
            </div>
            <footer class="panel-footer text-center">
                <!--網頁尾-->
            </footer>
        </section>
        </div>
    </div>
</asp:Content>
