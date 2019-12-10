<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E32_1.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E32_1" %>
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
                    <asp:Label ID="lblDept" runat="server" Text="Label"></asp:Label>
                    &nbsp;結報申請表管理-查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">申請表編號</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                        <asp:TextBox ID="txtOvcTofNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">是否付款</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                        <asp:DropDownList  ID="drpOvcIsPaid" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>已付款</asp:ListItem>
                                            <asp:ListItem>未付款</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                        <asp:DropDownList  ID="drpOvcBudget" CssClass="tb tb-m  position-left" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>維持門010105</asp:ListItem>
                                            <asp:ListItem>維持門010106</asp:ListItem>
                                            <asp:ListItem>投資門150110</asp:ListItem>
                                            <asp:ListItem>投資門150111</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                        <asp:DropDownList  ID="drpOvcPurposeType" CssClass="tb tb-m  position-left" runat="server">
                                            <asp:ListItem Value="不限定">不限定</asp:ListItem>
                                            <asp:ListItem Value="0294">運費0294</asp:ListItem>
                                            <asp:ListItem Value="0291">國內旅遊0291</asp:ListItem>
                                            <asp:ListItem Value="0203">郵電費0203</asp:ListItem>
                                            <asp:ListItem Value="0271">物品費0271</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">申請日期</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOvcApplyDate1" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                      
                                        <asp:Label CssClass="control-label " runat="server">&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOvcApplyDate2" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:CheckBoxList ID="chkOdtApplyDate" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Selected="true">不限定日期</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" OnClick="btnQuery_Click" cssclass="btn-warning" runat="server" Text="查詢" /><br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_TOF" OnRowCommand="GV_TBGMT_TOF_RowCommand" DataKeyNames="OVC_TOF_NO" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_TOF_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_TOF_NO" HeaderText="申請表編號" />
                                    <asp:BoundField DataField="OVC_BUDGET" HeaderText="預算科目" />
                                    <asp:BoundField DataField="OVC_PURPOSE_TYPE" HeaderText="用途別" />
                                    <asp:BoundField DataField="OVC_ABSTRACT" HeaderText="摘要" />
                                    <asp:BoundField DataField="ONB_AMOUNT" HeaderText="金額" />
                                    <asp:BoundField DataField="OVC_NOTE" HeaderText="備考" />
                                    <asp:BoundField DataField="OVC_SECTION" HeaderText="申請單位" />
                                    <asp:BoundField DataField="ODT_APPLY_DATE" HeaderText="申請日期" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="OVC_APPLY_ID" HeaderText="申請者" />
                                    <asp:BoundField DataField="OVC_IS_PAID" HeaderText="付款" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnModify" CssClass="btn-warning" Text="修改" CommandName="btnModify" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnDel" CssClass="btn-danger" Text="刪除" CommandName="btnDel" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnPrint" CssClass="btn-success" Text="列印" CommandName="btnPrint" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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

