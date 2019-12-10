<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B21_1.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B21_1" %>
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
                    投保通知書-新增-Step1 選擇外運資料表
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtOVC_EDF_NO" CssClass="tb tb-l text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                </tr> 
                            </table>
                            <div class="text-center">
                                <asp:Button cssclass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" /><br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_EDF" DataKeyNames="EDF_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" 
                                OnPreRender="GV_TBGMT_EDF_PreRender" OnRowDataBound="GV_TBGMT_EDF_RowDataBound" OnRowCommand="GV_TBGMT_EDF_RowCommand" runat="server">
                                <Columns>
                                    <%--<asp:BoundField HeaderText="編號" DataField="OVC_EDF_NO" />--%>
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
                                    <asp:BoundField HeaderText="付款方式" DataField="OVC_PAYMENT_TYPE" />
                                    <asp:BoundField HeaderText="戰略性" DataField="OVC_IS_STRATEGY" />
                                    <asp:BoundField HeaderText="審核狀況" DataField="OVC_REVIEW_STATUS" />
                                    <%--<asp:BoundField HeaderText="投保通知書" DataField="" />--%>
                                    <asp:TemplateField HeaderText="投保通知書" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" Text="建立" CommandName="dataNew" runat="server" />
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

