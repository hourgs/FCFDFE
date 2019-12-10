<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E1C.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E1C" %>
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
                <header  class="title text-blue">
                    <!--標題-->
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="control-label" runat="server" Text="購案編號：" Font-Size="Large"></asp:Label>
                                <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="title text-blue">
                                <asp:Label CssClass="subtitle" runat="server" Text="代管現金"></asp:Label>
                            </div>
                            <br>
                            <asp:GridView ID="GV_TBMMANAGE_CASH" CssClass=" table data-table table-striped border-top text-center" OnRowDataBound="GV_TBMMANAGE_CASH_RowDataBound" OnPreRender="GV_TBMMANAGE_CASH_PreRender" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業" HeaderStyle-Width="180px">
                                        <ItemTemplate>
                   		                    <asp:Button ID="btnTakeOver" CssClass="btn-success btnw4 text-center" OnClick="btnTakeOver_Click" Text="修改" runat="server" />
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw4 text-center" OnClick="btnDel_Click" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" Text="刪除" runat="server" /><br>
                                            <asp:Label ID="labSN" Text='<%# Bind("OVC_SN") %>' runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="類別" DataField="OVC_KIND" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="合約編號" DataField="OVC_PURCH_6" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="款項所有權人" DataField="OVC_OWN_NAME" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="合計金額" DataField="ONB_ALL_MONEY" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="收入單通知編號" DataField="OVC_RECEIVE_NO" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="收入日期" DataField="OVC_DRECEIVE" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="退還單通知編號" DataField="OVC_BACK_NO" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="退還日期" DataField="OVC_DBACK" ItemStyle-CssClass="text-center" />
                	            </Columns>
	   		                </asp:GridView>
                            <br>
                            <div class="text-center">
                                <asp:Button ID="btnCashNew" CssClass="btn-success btnw4" OnClick="btnCashNew_Click" runat="server" Text="新增" />
                                <asp:Button ID="btnCashReturnMain" CssClass="btn-success btnw4" OnClick="btnCashReturnMain_Click" runat="server" Text="回主流程" />
                            </div>
                            <br>
                            <div class="title text-blue">
                                <asp:Label CssClass="subtitle" runat="server" Text="代管保證文件"></asp:Label>
                            </div>
                            <br>
                            <asp:GridView ID="GV_TBMMANAGE_PROM" CssClass=" table data-table table-striped border-top text-center" OnRowDataBound="GV_TBMMANAGE_PROM_RowDataBound" OnPreRender="GV_TBMMANAGE_PROM_PreRender" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業" HeaderStyle-Width="180px">
                                        <ItemTemplate>
                   		                    <asp:Button ID="btnTakeOver" CssClass="btn-success btnw4 text-center" OnClick="btnTakeOver_Click1" Text="修改" runat="server" />
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw4 text-center" OnClick="btnDel_Click1" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" Text="刪除" runat="server" /><br>
                                            <asp:Label ID="labSN" Text='<%# Bind("OVC_SN") %>' runat="server" Visible="false"></asp:Label>    
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="類別" DataField="OVC_KIND" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="合約編號" DataField="OVC_PURCH_6" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="保證人" DataField="OVC_OWN_NAME" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="合計金額" DataField="ONB_ALL_MONEY" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="收入單通知編號" DataField="OVC_RECEIVE_NO" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="收入日期" DataField="OVC_DRECEIVE" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="退還單通知編號" DataField="OVC_BACK_NO" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="退還日期" DataField="OVC_DBACK" ItemStyle-CssClass="text-center" />
                	            </Columns>
	   		                </asp:GridView>
                            <br>
                            <div class="text-center">
                                <asp:Button ID="btnPromNew" CssClass="btn-success btnw4" OnClick="btnPromNew_Click" runat="server" Text="新增" />
                                <asp:Button ID="btnPromReturnMain" CssClass="btn-success btnw4" OnClick="btnPromReturnMain_Click" runat="server" Text="回主流程" />
                            </div>
                            <br>
                            <div class="title text-blue">
                                <asp:Label CssClass="subtitle" runat="server" Text="代管有價證券"></asp:Label>
                            </div>
                            <br>
                            <asp:GridView ID="GV_TBMMANAGE_STOCK" CssClass=" table data-table table-striped border-top text-center" OnRowDataBound="GV_TBMMANAGE_STOCK_RowDataBound" OnPreRender="GV_TBMMANAGE_STOCK_PreRender" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業" HeaderStyle-Width="180px">
                                        <ItemTemplate>
                   		                    <asp:Button ID="btnTakeOver" CssClass="btn-success btnw4 text-center" OnClick="btnTakeOver_Click2" Text="修改" runat="server" />
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw4 text-center" OnClick="btnDel_Click2" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" Text="刪除" runat="server" /><br>
                                            <asp:Label ID="labSN" Text='<%# Bind("OVC_SN") %>' runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="類別" DataField="OVC_KIND" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="合約編號" DataField="OVC_PURCH_6" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="有價證券所有權人" DataField="OVC_OWN_NAME" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="合計金額" DataField="ONB_ALL_MONEY" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="收入單通知編號" DataField="OVC_RECEIVE_NO" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="收入日期" DataField="OVC_DRECEIVE" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="退還單通知編號" DataField="OVC_BACK_NO" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="退還日期" DataField="OVC_DBACK" ItemStyle-CssClass="text-center" />
                	            </Columns>
	   		                </asp:GridView>
                            <br>
                            <div class="text-center">
                                <asp:Button ID="btnStockNew" CssClass="btn-success btnw4" OnClick="btnStockNew_Click" runat="server" Text="新增" />
                                <asp:Button ID="btnStockReturnMain" CssClass="btn-success btnw4" OnClick="btnStockReturnMain_Click" runat="server" Text="回主流程" />
                            </div>
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
