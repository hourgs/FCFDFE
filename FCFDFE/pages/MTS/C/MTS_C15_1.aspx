<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_C15_1.aspx.cs" Inherits="FCFDFE.pages.MTS.C.MTS_C15_1" %>
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
                    國防部國訪採購室外購案軍品保留索賠權及理賠申請-查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnMessageQuery" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" style="width: 15%;">
                                        <asp:Label CssClass="control-label text-star" runat="server">保留索賠權編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_RECLAIM_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center" style="width: 15%;">
                                        <asp:Label CssClass="control-label" runat="server">申請單位</asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server" />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" TextMode="Search" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-success btnw4" OnClientClick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" Text="申請單位" runat="server" />&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-default btnw4" OnClick="btnClearDept_Click" Text="資料清空" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">作業進度</asp:Label>
                                    </td>
                                    <td>
                                       <asp:DropDownList ID="drpOVC_APPROVE_STATUS" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">申請日期</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_APPLY_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_APPLY_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                 <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" /><br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CLAIM_RESERVE" DataKeyNames="RECLAIM_SN" CssClass="table data-table table-striped border-top table-word" AutoGenerateColumns="false"
                                OnPreRender="GV_TBGMT_CLAIM_RESERVE_PreRender" OnRowCommand="GV_TBGMT_CLAIM_RESERVE_RowCommand" OnRowDataBound="GV_TBGMT_CLAIM_RESERVE_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="保留索賠權編號" DataField="OVC_RECLAIM_NO" />
                                    <asp:BoundField HeaderText="申請單位" DataField="OVC_ONNAME" />
                                    <asp:BoundField HeaderText="提單編號" DataField="OVC_BLD_NO" />
                                    <asp:BoundField HeaderText="軍種保留索賠權文號" DataField="OVC_CLAIM_MSG_NO" />
                                    <asp:BoundField HeaderText="軍種保留索賠權日期" DataField="OVC_CLAIM_DATE"/>
                                    <asp:BoundField HeaderText="軍品名稱" DataField="OVC_CLAIM_ITEM" />
                                    <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
                                    <asp:BoundField HeaderText="損失原因" DataField="OVC_CLAIM_REASON" />
                                    <asp:BoundField HeaderText="索賠件數" DataField="ONB_CLAIM_NUMBER" />
                                    <asp:BoundField HeaderText="索賠金額" DataField="ONB_CLAIM_AMOUNT" />
                                    <asp:BoundField HeaderText="作業進度" DataField="OVC_APPROVE_STATUS" />
                                    <asp:BoundField HeaderText="備註" DataField="OVC_NOTE" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-warning" Text="修改" CommandName="btnModify" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText=""  >
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                     <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center" >
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpAction"  CssClass="tb" style="width: 76px; margin-bottom: 5px;" runat="server"></asp:DropDownList>
                                            <asp:Button CssClass="btn-success" Text="列印" CommandName="btnPrint" CommandArgument='<%#txtOVC_RECLAIM_NO.Text + "," + txtOVC_DEPT_CDE.Value + "," + txtOVC_APPLY_DATE_S.Text + "," + txtOVC_APPLY_DATE_E.Text + "," + drpOVC_APPROVE_STATUS.SelectedValue + "," + txtOVC_BLD_NO.Text %>' runat="server"/>
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
