<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_D12_1.aspx.cs" Inherits="FCFDFE.pages.MTS.D.MTS_D12_1" %>

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
                    出口結報申請表-管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server">結報年度</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOdtApplyDate" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>&nbsp;&nbsp;<asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOvcInfNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server">付款狀況</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOvcIsPaid" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="8">
                                        <asp:DropDownList ID="drpOvcClassName" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnQuery" OnClick="btnQuery_Click" CssClass="btn-success btnw2" runat="server" Text="查詢" /><br />
                                <br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_EINF" DataKeyNames="OVC_INF_NO" OnRowCommand="GV_TBGMT_EINF_RowCommand" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_EINF_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_INF_NO" HeaderText="結報申請表編號" />
                                    <asp:BoundField DataField="OVC_GIST" HeaderText="案由" />
                                    <asp:BoundField DataField="OVC_BUDGET" HeaderText="預算科目及編號" />
                                    <asp:BoundField DataField="OVC_PURPOSE_TYPE" HeaderText="用途別" />
                                    <asp:BoundField DataField="ONB_AMOUNT" HeaderText="金額" />
                                    <asp:BoundField DataField="OVC_BUDGET_INF_NO" HeaderText="預算通知書編號" />
                                    <asp:BoundField DataField="OVC_INV_NO" HeaderText="發票號碼" />
                                    <asp:BoundField DataField="OVC_IS_PAID" HeaderText="已付款與否" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSave" CssClass="btn-warning" Text="修改" CommandName="btnModify" runat="server" />
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
