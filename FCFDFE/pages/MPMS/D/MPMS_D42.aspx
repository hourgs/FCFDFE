<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D42.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D42" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->審查紀錄查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered" style="text-align:center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">計劃年度(第二組)： </asp:Label>
                                        <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">採購單位地區：</asp:Label>
                                        <asp:TextBox ID="txtOVC_PUR_APPROVE" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnQuery" CssClass="btn-success btnw4" OnClick="btnQuery_Click" runat="server" Text="查詢" /><!--綠色-->
                                    </td>
                                </tr>
                            </table>
                                
                            <div class="subtitle">查詢結果</div>
                            <asp:GridView ID="GV_Query_PLAN" CssClass=" table data-table table-striped border-top" AutoGenerateColumns="false" 
                                DataKey="OVC_PURCH" OnPreRender="GV_Query_PLAN_PreRender" OnRowCommand="GV_Query_PLAN_RowCommand" runat="server">
                                <Columns>
                                     <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                            <asp:Label ID="btnPurch" CssClass="control-label" runat="server" Text ='<%# "" +Eval("OVC_PURCH") + Eval("OVC_PUR_AGENCY")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="功能" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" ID="btnToAUDIT" CommandArgument='<%#Eval("OVC_PURCH")%>' CommandName="btnToAUDIT"
                                                Text="查看" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                	            </Columns>
	   		               </asp:GridView>
                    </div>
                        <div class="text-center">
                            <asp:Button ID="btnReturn" Text="回上一頁" OnClick="btnReturn_Click" CssClass="btn-success btnw4" runat="server" />
                        </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
