<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A23_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A23_1" %>
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
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>出口物資訂艙單-Step1 選擇外運資料表</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">申請地區</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server"/>
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnQueryOVC_DEPT_CDE" OnClientClick="OpenWindow('txtOVC_DEPT_CDE','txtOVC_ONNAME')" CssClass="btn-success" Text="單位" runat="server"/>
                                        <asp:Button ID="btnResettxtOVC_DEPT_CDE" cssclass="btn-default btnw4" Text="資料清空" OnClick="btnResettxtOVC_DEPT_CDE_Click" runat="server"/>
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
                                        <asp:DropDownList ID="drpOVC_START_PORT" CssClass="tb tb-s" runat="server">
                                        </asp:DropDownList>
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
                                        <asp:Label CssClass="control-label" runat="server">訂艙狀況</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpEDF_SN" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Text="不限定" Selected="True" Value=""></asp:ListItem>
                                            <asp:ListItem Text="未訂艙" Value="0" ></asp:ListItem>
                                            <asp:ListItem Text="已訂艙" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">申請日期區間</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_APPLY_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_APPLY_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server"/>&emsp;
                                <asp:Button ID="btnNew" CssClass="btn-success btnw8" Text="新增星光指揮部訂艙單" OnClick="btnNew_Click" runat="server"/>
                            </div>
                            <div style="margin-top: 20px;">
                            <asp:GridView ID="GVTBGMT_EDF" DataKeyNames="EDF_SN" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" OnPreRender="GVTBGMT_EDF_PreRender" OnRowCommand="GVTBGMT_EDF_RowCommand" OnRowDataBound="GVTBGMT_EDF_RowDataBound" runat="server">
                                <Columns>
                                    <%--<asp:BoundField HeaderText="外運資料表編號" DataField="OVC_EDF_NO" />--%>
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
                                    <asp:BoundField HeaderText="申請人" DataField="OVC_CREATE_ID" />
                                    <asp:BoundField HeaderText="申請日期" DataField="ODT_RECEIVE_DATE"/>
                                    <asp:BoundField HeaderText="戰略性" DataField="OVC_IS_STRATEGY" />
                                    <asp:BoundField HeaderText="審核狀況" DataField="OVC_REVIEW_STATUS" />
                                    <asp:TemplateField HeaderText="訂艙單" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnNew" CssClass="btn-success btnw2" Text="建立" CommandName="btnNew" CommandArgument='' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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

