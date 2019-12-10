<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A24_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A24_1" %>
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
                    <div>出口物資訂艙單-管理</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>  
                                    <td><asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_EDF_NO" CssClass="tb tb-l text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">啟運港(機場)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_START_PORT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">目的港(機場)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PORT_CDE" CssClass="tb tb-m" hidden="true" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtOVC_CHI_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <input type="button" value="速查" class="btn-success" onclick="OpenWindowItem()" />
                                        <asp:Button ID="btnResetPort" cssclass="btn-default btnw4" Text="資料清空" OnClick="btnResetPort_Click" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">船(航空)公司</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">SO NO.</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SO_NO" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">進艙廠商</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_STORED_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server"></asp:Label></td>
                                    <td class="text-left">
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                            </div>
                            <div style="margin-top: 20px;">
                            <asp:GridView ID="GVTBGMT_ESO" DataKeyNames="EDF_SN" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" OnPreRender="GVTBGMT_ESO_PreRender" OnRowCommand="GVTBGMT_ESO_RowCommand" OnRowDataBound="GVTBGMT_ESO_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="外運資料表編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <!--EDF顯示新方法-->
                                            <asp:HyperLink ID="hlkOVC_EDF_NO" Text='<%# Eval("OVC_EDF_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a href="javascript: OpenWindow_BLDDATA('<%# FCommon.getEncryption(Eval("OVC_BLD_NO").ToString()) %>');">
                                                <%# Eval("OVC_EDF_NO")%>
                                            </a>--%>
                                            <%--<a href="javascript:var win=window.open('BLDDATA?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=250,top=270');">
                                                <%# Eval("OVC_EDF_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="外運資料表編號" Visible="false" DataField="OVC_EDF_NO" />
                                    <asp:BoundField HeaderText="接轉地區" DataField="OVC_DEPT_CDE" />
                                    <asp:BoundField HeaderText="啟運港(機場)" DataField="OVC_START_PORT" />
                                    <asp:BoundField HeaderText="目的港(機場)" DataField="OVC_ARRIVE_PORT" />
                                    <asp:BoundField HeaderText="航空公司" DataField="OVC_SHIP_COMPANY" />
                                    <asp:BoundField HeaderText="SO No." DataField="OVC_SO_NO" />
                                    <%--<asp:BoundField HeaderText="提單編號" DataField="OVC_BLD_NO" />--%>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <!--BLD顯示新方法-->
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnModify" CssClass="btn-success btnw2" Text="修改" CommandName="btnModify" CommandArgument='' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="btnDel"  CommandArgument='' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="訂艙單" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnPrintB" CssClass="btn-success btnw2" Text="列印" CommandName="btnPrintB" CommandArgument='<%#drpOVC_TRANSER_DEPT_CDE.SelectedValue + "," + drpOVC_START_PORT.SelectedValue + "," + txtOVC_PORT_CDE.Text + "," + drpOVC_SHIP_COMPANY.SelectedValue + "," + txtOVC_SO_NO.Text%>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="裝貨單" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnPrintL" CssClass="btn-success btnw2" Text="列印" CommandName="btnPrintL" CommandArgument='<%#drpOVC_TRANSER_DEPT_CDE.SelectedValue+","+drpOVC_START_PORT.SelectedValue+","+txtOVC_PORT_CDE.Text+","+drpOVC_SHIP_COMPANY.SelectedValue+","+txtOVC_SO_NO.Text %>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>
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

