<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B24_2.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B24_2" %>
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
                    結報申請表-修正
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnMessageEINF" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label CssClass="control-label" ID="lblOVC_INF_NO" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案由</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                        <%--<asp:DropDownList ID="drpOVC_GIST" CssClass="tb tb-m" runat="server"></asp:DropDownList>--%>
                                        <asp:TextBox ID="txtOVC_GIST" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目及編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpOVC_BUDGET" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">未輸入</asp:ListItem>
                                            <asp:ListItem Value="維持門010105">維持門010105</asp:ListItem>
                                            <asp:ListItem Value="投資門150110">投資門150110</asp:ListItem>
                                            <asp:ListItem Value="維持門010106">維持門010106</asp:ListItem>
                                            <asp:ListItem Value="投資門150111">投資門150111</asp:ListItem>
                                        </asp:DropDownList> 
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td colspan="3">
                                         <asp:TextBox ID="txtOVC_PURPOSE_TYPE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請日期</asp:Label>
                                    </td>
                                    <td colspan="3">
                                         <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_APPLY_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">金額</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label CssClass="control-label" runat="server">新台幣</asp:Label>
                                        <asp:TextBox ID="txtONB_AMOUNT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblOVC_BUDGET_INF_NO" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblONB_AMOUNT" CssClass="control-label" Visible="false" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算通知書編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtOVC_BUDGET_INF_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-success" Text="載入預算通知單編號" runat="server" />&nbsp;&nbsp;
                                        <asp:Label ID="lblOvcBudgetInfNo" CssClass="control-label text-red" Text="Label" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收據號碼</asp:Label>
                                    </td>
                                    <td colspan="3">
                                         <asp:TextBox ID="txtOVC_INV_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收據日期</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INV_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtOVC_NOTE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險公司</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpCO_SN" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtOVC_PLN_CONTENT" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button cssclass="btn-warning" OnClick="btnModify_Click" Text="修改結報申請表" runat="server" />
                                <asp:Button cssclass="btn-warning" OnClick="btnBack_Click" Text="回結報申請表管理" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
            
            <section class="panel">
                <asp:Panel ID="pnBottom" runat="server">
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="PnMessageQuery" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:20%;">
                                        <asp:Label CssClass="control-label" runat="server">投保通知書編號</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:25%;">
                                        <asp:TextBox ID="txtOVC_EINN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:30%" rowspan="2">
                                        <asp:DropDownList ID="drpOVC_INF_DATE" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnFilter_Click" Text="過濾" runat="server" />
                                    </td>
                                    <td style="width:25%" rowspan="2">
                                        <asp:Button CssClass="btn-success btnw4" OnClick="btnCheckBox_Click" CommandArgument="true" Text="全部勾選" runat="server" />
                                        <asp:Button CssClass="btn-default btnw4" OnClick="btnCheckBox_Click" CommandArgument="false" Text="全部取消" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td> 
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="text-center">
                                        <asp:DropDownList ID="drpOVC_INF_DATE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <!--<asp:Label CssClass="control-label" runat="server">投保通知書編號</asp:Label>&nbsp;&nbsp;-->
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnFilter_Click" Text="過濾" runat="server" />
                                    </td>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw4" OnClick="btnCheckBox_Click" CommandArgument="true" Text="全部勾選" runat="server" />
                                        <asp:Button CssClass="btn-default btnw4" OnClick="btnCheckBox_Click" CommandArgument="false" Text="全部取消" runat="server" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td colspan="4" class="text-left">
                                        <asp:CheckBoxList ID="chkOVC_EINN_NO" CssClass="radioButton" RepeatDirection="Horizontal" RepeatColumns="5" RepeatLayout="Flow" runat="server"></asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button cssclass="btn-warning" OnClick="btnNew_Click" Text="新增投保通知書" runat="server" /><br /><br />
                            </div>
                            <asp:Panel ID="PnMessageEINN" runat="server"></asp:Panel>
                            <asp:GridView ID="GV_TBGMT_EINN" DataKeyNames="EINN_SN" CssClass="table data-table table-striped border-top" 
                                AutoGenerateColumns="false" OnPreRender="GV_TBGMT_EINN_PreRender" 
                                OnRowCommand="GV_TBGMT_EINN_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="外運資料表編號" DataField="OVC_EDF_NO" />
                                    <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
                                    <%--<asp:BoundField HeaderText="物資名稱及數量" DataField="COUNT" />--%>
                                    <asp:TemplateField HeaderText="物資名稱及數量">
                                        <ItemTemplate>
                                            <%#Eval("OVC_CHI_NAME") +" / 共"+ Eval("COUNT") +"件"%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="物資價值" DataField="ONB_ITEM_VALUE" />
                                    <asp:BoundField HeaderText="保費" DataField="OVC_FINAL_INS_AMOUNT" />
                                    <asp:BoundField HeaderText="運輸工具" DataField="OVC_SHIP_COMPANY" />
                                    <asp:BoundField HeaderText="啟運港口" DataField="OVC_START_PORT" />
                                    <asp:BoundField HeaderText="目的港口" DataField="OVC_ARRIVE_PORT" /> 
                                    <asp:TemplateField HeaderText="" >  
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-danger" Text="刪除" CommandName="dataDel" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
               </asp:Panel>
                <footer class="panel-footer text-center">
                    <!--網頁尾--> 
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
