<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_C13_1.aspx.cs" Inherits="FCFDFE.pages.MTS.C.MTS_C13_1" %>

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
                    保險理賠建立與管理-查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnMessageQuery" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" style="width: 10%;">
                                        <asp:Label CssClass="control-label" runat="server">索賠情形</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_CLAIM_CONDITION" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td class="text-center" style="width: 10%;">
                                        <asp:Label CssClass="control-label" runat="server">申請單位</asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server" />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-success btnw4" OnClientClick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" Text="申請單位" runat="server" />&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-default btnw4" OnClick="btnClearDept_Click" Text="資料清空" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">通知書編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CLAIM_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CLAIM_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" /><br />
                                <br />
                                <asp:GridView ID="GV_TBGMT_CLAIM" DataKeyNames="CLAIM_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false"
                                    OnPreRender="GV_TBGMT_CLAIM_PreRender" OnRowCommand="GV_TBGMT_CLAIM_RowCommand" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="通知書編號" DataField="OVC_CLAIM_NO" />
                                        <asp:BoundField HeaderText="單位名稱" DataField="OVC_ONNAME" />
                                        <asp:BoundField HeaderText="索賠日期" DataField="ODT_CLAIM_DATE"/>
                                        <asp:BoundField HeaderText="索賠字號" DataField="OVC_CLAIM_MSG_NO" />
                                        <asp:BoundField HeaderText="索賠情形" DataField="OVC_CLAIM_CONDITION"/>
                                        <asp:BoundField HeaderText="保單號碼" DataField="OVC_INN_NO" />
                                        <asp:BoundField HeaderText="軍品名稱" DataField="OVC_CLAIM_ITEM" />
                                        <asp:BoundField HeaderText="軍品數量" DataField="ONB_CLAIM_NUMBER" />
                                        <asp:BoundField HeaderText="軍品總額" DataField="ONB_CLAIM_AMOUNT" DataFormatString="{0:N2}" />
                                        <asp:BoundField HeaderText="申請者" DataField="OVC_CREATE_LOGIN_ID" />
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Button CssClass="btn-success" Text="管理" CommandName="btnManage" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
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

