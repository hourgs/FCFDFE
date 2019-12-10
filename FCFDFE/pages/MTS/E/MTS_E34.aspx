<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E34.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E34" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <asp:Panel ID="PanelS" runat="server">
            <section class="panel">
                <header  class="title">
                    接轉作業查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOdtApplyDate" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" OnClick="btnQuery_Click" cssclass="btn-success btnw2" runat="server" Text="查詢" />&nbsp;&nbsp;
                                <asp:Button ID="btnToD" OnClick="btnToD_Click" cssclass="btn-success" runat="server" Text="切換至接轉作業費明細查詢" /><br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_TOF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_TOF_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_SECTION" HeaderText="組別" />
                                    <asp:BoundField DataField="ONB_AMOUNT" HeaderText="應付金額" />
                                    <asp:BoundField DataField="ONB_AMOUNT_YES" HeaderText="已付金額" />
                                    <asp:BoundField DataField="ONB_AMOUNT_NO" HeaderText="未付金額" />
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
            <asp:Panel ID="PanelD" runat="server">
            <section class="panel">
                <header  class="title">
                    接轉作業明細查詢
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">                               
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOdtApplyDate2" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">組別</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpOvcSection" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>桃園地區</asp:ListItem>
                                            <asp:ListItem>基隆地區</asp:ListItem>
                                            <asp:ListItem>高雄接轉組</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOvcPurposeType" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem Value="不限定">不限定</asp:ListItem>
                                            <asp:ListItem Value="0294">運費0294</asp:ListItem>
                                            <asp:ListItem Value="0291">國內旅遊0291</asp:ListItem>
                                            <asp:ListItem Value="0203">郵電費0203</asp:ListItem>
                                            <asp:ListItem Value="0271">物品費0271</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">付款</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpOvcIsPaid" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>已付款</asp:ListItem>
                                            <asp:ListItem>未付款</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button OnClick="btndDetailQuery_Click" ID="btndDetailQuery" cssclass="btn-success btnw2" runat="server" Text="查詢" /><br /><br />  
                            </div>
                            <asp:GridView ID="GV_TBGMT_DETAIL" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_DETAIL_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_TOF_NO" HeaderText="申請表編號" />
                                    <asp:BoundField DataField="OVC_SECTION" HeaderText="申請地區" />
                                    <asp:BoundField DataField="ODT_APPLY_DATE" HeaderText="申請日期" />
                                    <asp:BoundField DataField="OVC_PURPOSE_TYPE" HeaderText="用途別" />
                                    <asp:BoundField DataField="OVC_ABSTRACT" HeaderText="摘要" />
                                    <asp:BoundField DataField="ONB_AMOUNT" HeaderText="金額" />
                                    <asp:BoundField DataField="OVC_NOTE" HeaderText="備考" />
                                    <asp:BoundField DataField="OVC_IS_PAID" HeaderText="付款" />
                                </Columns>
                            </asp:GridView>
                            <div style="text-align:center;">
                                 <asp:Button ID="btnToS" OnClick="btnToS_Click" cssclass="btn-success" runat="server" Text="切換至接轉作業費查詢" />
                                 <asp:Button ID="btnPrint" OnClick="btnPrint_Click" cssclass="btn-success btnw2" runat="server" Text="列印" /><br /><br />
                            </div>
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
