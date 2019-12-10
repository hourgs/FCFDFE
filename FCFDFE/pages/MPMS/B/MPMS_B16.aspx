<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B16.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B16" %>
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
                    <!--標題-->撤案查詢作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle"> 查詢目前已撤案資料 </div>
                            <table class="table table-bordered" style="text-align:center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">計劃年度(第二組)： </asp:Label>
                                        <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>106</asp:ListItem>
                                        </asp:DropDownList> 
                                        <asp:Button ID="btnQuery" cssclass="btn-success btnw2" OnClick="btnQuery_Click" runat="server" Text="查詢" />
                                    </td>
                                </tr>
                            </table>
                                
                            <div class="subtitle">查詢結果</div>
                            <asp:GridView ID="GV_Content" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" OnPreRender="GV_Content_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                     <asp:TemplateField HeaderText="委購單位">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_PUR_NSECTION" Text='<%# Eval("OVC_PUR_NSECTION") %>' runat="server"></asp:Label><br />
                                                        撤案日:<asp:Label ID="lblOVC_PUR_DCANPO" Text='<%# Eval("OVC_PUR_DCANPO") %>' runat="server"></asp:Label><br />
                                                        撤案原因:<asp:Label ID="lblOVC_PUR_DCANRE" Text='<%# Eval("OVC_PUR_DCANRE") %>' runat="server" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                    <asp:BoundField HeaderText="審查總次數" DataField="ONB_CHECK_TIMES" />
                                    <asp:BoundField HeaderText="最後計評承辦人" DataField="OVC_ASSIGNER" />
                                    <asp:BoundField HeaderText="購案最後階段" DataField="OVC_STATUS" />
                	            </Columns>
	   		               </asp:GridView>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>