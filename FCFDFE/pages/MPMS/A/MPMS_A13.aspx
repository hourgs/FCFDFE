<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_A13.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.MPMS_A13" %>
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
                    <!--標題-->採購預劃購案查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle"> 已完成購案查詢 </div>
                            <table class="table table-bordered" style="text-align:center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">計劃年度(第二組)： </asp:Label>
                                        <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server"></asp:DropDownList> 
                                        <asp:Button ID="btnQuery_OVC_BUDGET_YEAR" cssclass="btn-success btnw2" runat="server" OnClick="btnQuery_OVC_BUDGET_YEAR_Click" Text="查詢" />
                                    </td>
                                </tr>
                            </table>
                                
                            <div class="subtitle">查詢結果</div>
                            <asp:GridView ID="GV_OVC_BUDGET" CssClass=" table data-table table-striped border-top table-bordered " DataKeyNames="OVC_PURCH" OnRowCommand="GV_OVC_BUDGET_RowCommand" OnRowDataBound="GV_OVC_BUDGET_RowDataBound" AutoGenerateColumns="false" OnPreRender="GV_OVC_BUDGET_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:BoundField HeaderText="委購單位" DataField="OVC_AGENT_UNIT" />
                                    <asp:BoundField HeaderText="申請人" DataField="OVC_PUR_USER" />
                                    <asp:BoundField HeaderText="預計呈報日" DataField="OVC_DAPPLY" />
                                    <asp:BoundField HeaderText="管制日/撤案日" DataField="OVC_DAUDIT" />
                                    <asp:BoundField HeaderText="是否建案" DataField="OVC_PURCH_OK" />
                                      
                                    <asp:TemplateField HeaderText="功能" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" ID="btnQuery_FINAL" Text="查詢" CommandName="DataQuery"
                                                CommandArgument="<%# Container.DataItemIndex%>" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                	            </Columns>
	   		               </asp:GridView>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
