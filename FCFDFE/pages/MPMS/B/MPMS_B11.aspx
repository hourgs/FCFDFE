<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B11.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
        function Check() {
            //
            var hidden = document.getElementById("<%=hidYESNO.ClientID %>");
            if (confirm('是否要由其他購案複製到新編案件?\n\n是：請按確定\n\n否：請按取消')) {
                hidden.value = "ok";
                window.location.href = '~/pages/MPMS/B/MPMS_B12';
            }
            else {
                hidden.value = "cancel";
                window.location.href = '~/pages/MPMS/B/MPMS_B13';
            }
        }
        
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->購案查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <asp:HiddenField id="hidYESNO" runat="server" Value=""/>
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle"> 選擇年度來查詢 </div>
                            <table class="table table-bordered control-label" style="text-align:center">
                                <tr>
                                    <td >請選擇計劃年度(第二組)： 
                                        <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server"></asp:DropDownList> 
                                        <asp:Button ID="btnQuery_OVC_BUDGET" cssclass="btn-success btnw2" OnClick="btnQuery_OVC_BUDGET_Click" runat="server" Text="查詢" />
                                    </td>
                                </tr>
                            </table>
                            <div class="subtitle"> 查詢結果 </div>
                            <asp:GridView ID="GV_OVC_BUDGET" CssClass="table table-striped border-top text-center" DataKeyNames="OVC_PURCH" OnPreRender="GV_OVC_BUDGET_PreRender" OnRowCommand="GV_OVC_BUDGET_RowCommand" OnRowDataBound="GV_OVC_BUDGET_RowDataBound" OnRowCreated="GV_OVC_BUDGET_RowCreated" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                     <asp:TemplateField HeaderText="購案編號" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                           <asp:Label ID ="lblPurch" CssClass="control-label" Text='<%#Eval("OVC_PURCH")%>' runat="server">投標段次：</asp:Label>
                                           <asp:Label CssClass="control-label" Text='<%#Eval("OVC_PUR_AGENCY")%>' runat="server">投標段次：</asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" ItemStyle-Width="35%"/> <%--1301--%>
                                    <asp:BoundField HeaderText="預劃申辦日" DataField="OVC_DAPPLY" ItemStyle-Width="10%"/> <%--1301--%>
                                    <asp:BoundField HeaderText="實際申辦日" DataField="OVC_DPROPOSE" ItemStyle-Width="10%"/> <%--1301PLAN--%>
                                    <asp:BoundField HeaderText="評核單位收辦日" DataField="OVC_DRECEIVE_PAPER" ItemStyle-Width="10%"/> <%--1202--%>
                                    <asp:TemplateField HeaderText="移辦狀況" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Button ID="btnQuery_OVC_BUDGET" CssClass="btn-success" Text="查詢狀況" 
                                                OnClientClick='<%# Eval("OVC_PURCH","javascript:window.open(\"TRANSFORM.aspx?OVC_PURCH={0}\",null,\"toolbar=0,location=0,status=0,menubar=0,width=700,height=500,left=200,top=80\");")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="功能" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btn-row" ID="btnNew" Text="新編" CommandName="DataNew" CommandArgument='<%--#Eval("傳變數")--%>' UseSubmitBehavior="false" OnClientClick="Check()" runat="server"/>
                                            <asp:Button CssClass="btn-success btn-row" ID="btnModify" Text="修改" CommandName="DataModify" CommandArgument='<%--#Eval("傳變數")--%>' UseSubmitBehavior="false" runat="server"/><br>
                                            <asp:Button CssClass="btn-success" ID="btnApplication" Text="申辦" CommandName="DataApplication" CommandArgument='<%--#Eval("傳變數")--%>' UseSubmitBehavior="false" runat="server"/>
                                            <asp:Button CssClass="btn-success" ID="btnClosed" Text="澄覆" CommandName="DataClosed" CommandArgument='<%--#Eval("傳變數")--%>' UseSubmitBehavior="false" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="是否已經建案(Y/N)" DataField="OVC_PURCH_OK" /> <%--1301PLAN--%>
                	            </Columns>
	   		               </asp:GridView>
                </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
