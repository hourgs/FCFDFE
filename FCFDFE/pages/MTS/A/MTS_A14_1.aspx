<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A14_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A14_1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>作業時程管制簿管理</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width: 10%">
                                        <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 45%">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 10%">
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 35%">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">航次</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_VOYAGE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接收單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:HiddenField ID="txtOVC_DEPT_CDE" runat=Server />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnQueryOVC_DEPT_CDE" OnClientClick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" CssClass="btn-success" Text="單位查詢" runat="server" />
                                        <asp:Button ID="btnResettxtOVC_DEPT_CDE" CssClass="btn-default btnw4" Text="資料清空" OnClick="btnResettxtOVC_DEPT_CDE_Click" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">機敏軍品</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_IS_SECURITY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                            </div>
                            <br>
                            <asp:GridView ID="GVTBGMT_ICR" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top dt-fixed" AutoGenerateColumns="false" OnPreRender="GVTBGMT_ICR_PreRender" OnRowCommand="GVTBGMT_ICR_RowCommand" OnRowDataBound="GVTBGMT_ICR_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a ID="link" href="javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');" >
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
                                    <asp:BoundField HeaderText="品名" DataField="OVC_CHI_NAME" />
                                    <asp:BoundField HeaderText="接收單位" DataField="OVC_RECEIVE_DEPT_CODE" />
                                    <asp:BoundField HeaderText="收到貨通知書日期" DataField="ODT_RECEIVE_INF_DATE"/>
                                    <asp:BoundField HeaderText="進口日期" DataField="ODT_IMPORT_DATE" />
                                    <asp:BoundField HeaderText="收國外報相關文件日期" DataField="ODT_ABROAD_CUSTOM_DATE"/>
                                    <asp:BoundField HeaderText="換小提單日期" DataField="ODT_CHANGE_BLD_DATE"/>
                                    <asp:BoundField HeaderText="報關日期" DataField="ODT_CUSTOM_DATE" />
                                    <asp:BoundField HeaderText="機敏軍品" DataField="OVC_IS_SECURITY" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnModify" CssClass="btn-warning btnw2" Text="修改" CommandName="btnModify"
                                                CommandArgument="<%# Container.DataItemIndex%>" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="btnDel"
                                                CommandArgument="<%# Container.DataItemIndex%>" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                                <asp:Button ID="btnPrint" CssClass="btn-success btnw2" OnClick="btnPrint_Click" Text="列印" Visible="false" runat="server" />
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
