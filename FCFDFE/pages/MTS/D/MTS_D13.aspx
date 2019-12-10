<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_D13.aspx.cs" Inherits="FCFDFE.pages.MTS.D.MTS_D13" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
                    <asp:Panel ID="PanelStatistics" runat="server">
                        <section class="panel">
                            <header class="title">
                                保費統計查詢
                            </header>
                            <asp:Panel ID="PnMessageS" runat="server"></asp:Panel>
                            <!--預留空間，未來做錯誤訊息顯示。-->
                            <div class="panel-body" style="border: solid 2px;">
                                <div class="form" style="border: 5px;">
                                    <div class="cmxform form-horizontal tasi-form">
                                        <table class="table table-bordered">
                                            <tr>
                                                <td style="text-align: center; vertical-align: middle;">
                                                    <asp:Label CssClass="control-label" runat="server">結報年度</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="drpOdtApplyDate" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>內容1</asp:ListItem>
                                                        <asp:ListItem>內容1</asp:ListItem>
                                                    </asp:DropDownList>&nbsp;&nbsp;
                                        <asp:Button ID="btnQuery" OnClick="btnQuery_Click" CssClass="btn-success btnw2" runat="server" Text="查詢" />&nbsp;&nbsp;
                                        <asp:Button ID="btnChangeToDetails" OnClick="btnChangeToDetails_Click" CssClass="btn-success" runat="server" Text="切換至保費明細查詢" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:GridView ID="GV_TBGMT_INSRATE" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_INSRATE_PreRender" runat="server">
                                            <Columns>
                                                <asp:BoundField DataField="OVC_CLASS_NAME" HeaderText="軍種" />
                                                <asp:BoundField DataField="ODT_SHOULD" HeaderText="應付金額" />
                                                <asp:BoundField DataField="ODT_YES" HeaderText="已付金額" />
                                                <asp:BoundField DataField="ODT_NO" HeaderText="未付金額" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <footer class="panel-footer" style="text-align: center;">
                                <!--網頁尾-->
                            </footer>
                        </section>
                    </asp:Panel>

                    <asp:Panel ID="PanelDetails" runat="server">
                        <section class="panel">
                            <header class="title">
                                保費明細查詢
                            </header>
                            <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                            <!--預留空間，未來做錯誤訊息顯示。-->
                            <div class="panel-body" style="border: solid 2px;">
                                <div class="form" style="border: 5px;">
                                    <div class="cmxform form-horizontal tasi-form">
                                        <table class="table table-bordered">
                                            <tr>
                                                <td style="text-align: center; vertical-align: middle;">
                                                    <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <div>
                                                        <asp:Label CssClass="control-label position-left" runat="server">起始日期&nbsp;&nbsp;</asp:Label>
                                                        <div class="input-append datepicker">
                                                            <asp:TextBox ID="txtOdtInvDate1" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                                        </div>
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div>
                                                        <asp:Label CssClass="control-label position-left" runat="server">結束日期&nbsp;&nbsp;</asp:Label>
                                                        <div class="input-append datepicker">
                                                            <asp:TextBox ID="txtOdtInvDate2" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="text-align: center; vertical-align: middle;">
                                                    <asp:Label CssClass="control-label" runat="server">區分</asp:Label>
                                                </td>
                                                <td colspan="2" style="text-align: center; vertical-align: middle;">
                                                    <asp:DropDownList ID="drpIsPaid" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>不限定</asp:ListItem>
                                                        <asp:ListItem>未申請</asp:ListItem>
                                                        <asp:ListItem>未付款</asp:ListItem>
                                                        <asp:ListItem>已付款</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: center; vertical-align: middle;">
                                                    <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                                </td>
                                                <td colspan="2" style="text-align: center; vertical-align: middle;">
                                                    <asp:TextBox ID="txtOvcPurchNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; vertical-align: middle;">
                                                    <asp:Label CssClass="control-label" runat="server">進出口</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="drpImportOrExport" CssClass="tb tb-s" runat="server">
                                                        <asp:ListItem>不限定</asp:ListItem>
                                                        <asp:ListItem>進口</asp:ListItem>
                                                        <asp:ListItem>出口</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: center; vertical-align: middle;">
                                                    <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                                </td>
                                                <td colspan="5">
                                                    <asp:DropDownList ID="drpOvcMilitaryType" CssClass="tb tb-m" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="text-align: center;">
                                            <asp:Button ID="btnQuery1" OnClick="btnQuery1_Click" CssClass="btn-success btnw2" runat="server" Text="查詢" />
                                            <asp:Button ID="btnChangeToStatistics" OnClick="btnChangeToStatistics_Click" CssClass="btn-success" runat="server" Text="切換至保費統計查詢" /><br />
                                            <br />
                                        </div>
                                        <asp:GridView ID="GV_TBGMT_INSRATE_DETAIL" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_INSRATE_DETAIL_PreRender" runat="server">
                                            <Columns>
                                                <asp:BoundField DataField="OVC_INN_NO" HeaderText="投保通知書編號" />
                                                <asp:BoundField DataField="OVC_EDF_NO" HeaderText="提單編號" />
                                                <asp:BoundField DataField="OVC_INF_NO" HeaderText="結報申請單編號" />
                                                <asp:BoundField DataField="ONB_INS_AMOUNT" HeaderText="保費金額" />
                                                <asp:BoundField DataField="OVC_COMPANY" HeaderText="保險公司" />
                                                <asp:BoundField DataField="OVC_CLASS_NAME" HeaderText="軍種" />
                                                <asp:BoundField DataField="OVC_PURCH_NO" HeaderText="案號" />
                                                <asp:BoundField DataField="OVC_IS_PAID" HeaderText="區分" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <footer class="panel-footer" style="text-align: center;">
                                <!--網頁尾-->
                            </footer>
                        </section>
                    </asp:Panel>
        </div>
    </div>
</asp:Content>
