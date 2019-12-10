<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A1A_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A1A_1" %>

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
                    <div>進口軍品運輸交接單</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width: 10%">
                                        <asp:Label CssClass="control-label" runat="server">交換單號碼</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 40%">
                                        <asp:TextBox ID="txtOVC_IHO_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 10%">
                                        <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 40%">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
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
                                        <asp:Label CssClass="control-label" runat="server">接收單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="Server" />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button CssClass="btn-success" Text="單位查詢" OnClientClick="OpenWindow('txtOVC_DEPT_CDE','txtOVC_ONNAME')" runat="server" />
                                        <asp:Button CssClass="btn-default btnw4" Text="資料清空" OnClick="btnResetOVC_DEPT_CDE_CODE_Click" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">起運時間</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_START_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_START_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">抵運時間</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_ARRIVE_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_ARRIVE_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                            </div>
                            <br />
                            <asp:GridView ID="GVTBGMT_IHO" DataKeyNames="OVC_IHO_NO" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_IHO_PreRender" OnRowCommand="GVTBGMT_IHO_RowCommand" OnRowDataBound="GVTBGMT_IHO_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="交接單編號" DataField="OVC_IHO_NO" />
                                    <%--<asp:BoundField HeaderText="提單號碼" DataField="OVC_BLD_NO" />--%>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <!--BLD顯示新方法-->
                                            <%--<asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>--%>
                                            <asp:HiddenField ID="lblOVC_BLD_NO" Value='<%# Eval("OVC_BLD_NO")%>' runat="server" />
                                            <asp:Panel ID="pnOVC_BLD_NO" runat="server"></asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="運輸方法" DataField="OVC_INLAND_TRANS_TYPE" />
                                    <asp:BoundField HeaderText="起運地點" DataField="OVC_START_PLACE" />
                                    <asp:BoundField HeaderText="運達地點" DataField="OVC_ARRIVE_PLACE" />
                                    <asp:BoundField HeaderText="接收單位" DataField="OVC_RECEIVE_DEPT_CDE" />
                                    <asp:BoundField HeaderText="接轉地區" DataField="OVC_TRANSER_DEPT_CDE" />
                                    <asp:BoundField HeaderText="起運時間" DataField="ODT_START_DATE" />
                                    <asp:BoundField HeaderText="抵運時間" DataField="ODT_ARRIVE_DATE" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-warning btnw2" Text="修改" CommandName="btnModify" CommandArgument='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-danger btnw2" Text="刪除" CommandName="btnDel" CommandArgument='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" Text="列印" CommandName="PrintOver" CommandArgument='<%#Eval("OVC_BLD_NO")+","+drpOVC_TRANSER_DEPT_CDE.SelectedValue+","+txtOVC_ONNAME.Text %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
