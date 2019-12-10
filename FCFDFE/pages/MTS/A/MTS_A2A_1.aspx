<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A2A_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A2A_1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1100px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>軍品出口作業時程管制表-管理</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:25%;">
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">年月日查詢<br>(接獲委運單位函文時間)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_REQUIRE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                            </div><br>
                            <asp:GridView ID="GVTBGMT_ETR" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false"
                                OnPreRender="GVTBGMT_ETR_PreRender" OnRowCommand="GVTBGMT_ETR_RowCommand" OnRowDataBound="GVTBGMT_ETR_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a href="javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="接獲委運單位函文時間" DataField="ODT_REQUIRE_DATE" />
                                    <asp:BoundField HeaderText="收文時間" DataField="ODT_RECEIVE_DATE" />
                                    <asp:BoundField HeaderText="辦理免稅時間" DataField="ODT_PROCESS_DATE" />
                                    <asp:BoundField HeaderText="高科技許可證收辦時間" DataField="ODT_STRATEGY_PROCESS_DATE" />
                                    <asp:BoundField HeaderText="電請辦理進倉時間" DataField="ODT_TEL_DATE" />
                                    <asp:BoundField HeaderText="通關時間" DataField="ODT_PASS_DATE" />
                                    <asp:BoundField HeaderText="啟航時間" DataField="ODT_SHIP_START_DATE" />
                                    <asp:BoundField HeaderText="提單函復時間" DataField="ODT_RETURN_DATE" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnModify" CssClass="btn-success btnw2" Text="修改" CommandName="btnModify" CommandArgument='' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="btnDel" CommandArgument='' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnPrint" CssClass="btn-success btnw2" Text="列印" CommandName="btnPrint" CommandArgument='' runat="server"/>
                                            <asp:button ID="btnPrint_2" CssClass="btn-success btnw4" Text="下載提單" CommandName="btnPrint_2" CommandArgument='' runat="server"/>
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
