<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A22_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A22_1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script>
        //港埠查詢視窗function 設定位置、大小等
        function OpenWindowItem() {
            var win_width = 600;
            var win_height = 150;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var theURL = '<%=ResolveClientUrl("~/pages/MTS/A/PORTLIST.aspx")%>';
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>外運資料表-管理</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                        <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                                <div class="cmxform form-horizontal tasi-form">
                                    <!--網頁內容-->
                                    <table class="table table-bordered text-center">
                                        <tr>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">申請單位</asp:Label>
                                            </td>
                                            <td class="text-left">
                                               
                                                <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server" />
                                                <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnQueryOVC_REQ_DEPT_CDE" CssClass="btn-success btnw4" OnClientClick="OpenWindow('txtOVC_DEPT_CDE','txtOVC_ONNAME')" Text="單位查詢" runat="server" />
                                                <asp:Button ID="btnReset" CssClass="btn-default btnw4" OnClick="btnReset_Click" Text="資料清空" runat="server" /><br>
                                                <%--<asp:Label ID="lblOVC_DEPT_CDE" CssClass="control-label" runat="server"></asp:Label>--%>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:TextBox ID="txtOVC_EDF_NO" CssClass="tb tb-l text-toUpper" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">啟運港(機場)</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:DropDownList ID="drpOVC_START_PORT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">目的港(機場)</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:TextBox ID="txtOVC_PORT_CDE" CssClass="tb tb-m" hidden="true" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="txtOVC_CHI_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                <input type="button" value="速查" class="btn-success" onclick="OpenWindowItem()" />
                                                <asp:Button ID="btnResetPort" cssclass="btn-default btnw4" Text="資料清空" OnClick="btnResetPort_Click" runat="server"/>
                                            </td>
                                            <%--<td class="text-left">
                                                <asp:DropDownList ID="drpOVC_ARRIVE_PORT" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">戰略性高科技貨品</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:DropDownList ID="drpOVC_IS_STRATEGY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">審核狀況</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:DropDownList ID="drpOVC_REVIEW_STATUS" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">申請年度(民國年)</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:TextBox ID="txtODT_RECEIVE_DATE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-l text-toUpper" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label" runat="server">單號</asp:Label>
                                            </td>
                                            <td class="text-left">
                                                <asp:TextBox ID="txtOVC_ITEM_NO2" CssClass="tb tb-l text-toUpper" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                                    <%--<asp:Button ID="btnQueryOVC_EDF_NO" OnClick="btnQueryOVC_EDF_NO_Click"  CssClass="btn-success btnw8" runat="server" Text="外運單號歷史查詢" />--%>
                                    </div>
                                    <br />
                                    <asp:GridView ID="GVTBGMT_EDF" DataKeyNames="EDF_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowCommand="GVTBGMT_EDF_RowCommand" OnPreRender="GVTBGMT_EDF_PreRender" OnRowDataBound="GVTBGMT_EDF_RowDataBound" runat="server">
                                        <Columns>
                                            <asp:TemplateField HeaderText="項次" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField HeaderText="外運資料表編號" DataField="OVC_EDF_NO" ControlStyle-Width="8%" />--%>
                                            <asp:TemplateField HeaderText="外運資料表編號" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <!--EDF顯示新方法-->
                                                    <asp:HyperLink ID="hlkOVC_EDF_NO" Text='<%# Eval("OVC_EDF_NO")%>' runat="server"></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
                                            <asp:BoundField HeaderText="啟運港(機場)" DataField="OVC_START_PORT" />
                                            <asp:BoundField HeaderText="目的港(機場)" DataField="OVC_ARRIVE_PORT" />
                                            <asp:BoundField HeaderText="發貨單位" DataField="OVC_DEPT_CDE" />
                                            <asp:BoundField HeaderText="申請地區" DataField="OVC_REQ_DEPT_CDE" />
                                            <asp:BoundField HeaderText="申請人" DataField="OVC_CREATE_LOGIN_ID" />
                                            <asp:BoundField HeaderText="申請日期" DataField="ODT_RECEIVE_DATE"/>
                                            <asp:BoundField HeaderText="戰略性" DataField="OVC_IS_STRATEGY" />
                                            <asp:BoundField HeaderText="審核狀況" DataField="OVC_REVIEW_STATUS" />
                                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnModify" CssClass="btn-success btnw2" Text="修改" CommandName="btnModify" CommandArgument='' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="btnDel" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" CommandArgument='' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnCheck" CssClass="btn-success btnw2" Text="審核" CommandName="btnCheck" CommandArgument='' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnPrint" CssClass="btn-success btnw2" Text="列印" CommandName="btnPrint" CommandArgument='' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
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
