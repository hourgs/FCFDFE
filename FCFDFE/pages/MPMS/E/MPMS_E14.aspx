<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E14.aspx.cs" EnableEventValidation="false" Inherits="FCFDFE.pages.MPMS.E.MPMS_E14" %>
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
                    <!--標題-->選擇處理購案
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                                    <asp:GridView ID="GV_BuyCaseStage" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_BuyCaseStage_PreRender" OnRowCreated="GV_BuyCaseStage_RowCreated" OnRowDataBound="GV_BuyCaseStage_RowDataBound" runat="server" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="購案編號" >
                                                <ItemTemplate>
                                                    <asp:Label ID="labOVC_PURCH" Text='<%# Bind("OVC_PURCH") %>' runat="server" />
                                                    <asp:Label ID="labOVC_PUR_AGENCY" Text='<%# Bind("OVC_PUR_AGENCY") %>' Visible="false" runat="server" />
                                                    <asp:Label ID="labOVC_PURCH_5" Text='<%# Bind("OVC_PURCH_5") %>' Visible="false" runat="server" />
                                                    <asp:Label ID="labOVC_PURCH_6" Text='<%# Bind("OVC_PURCH_6") %>' Visible="false" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                            <asp:BoundField HeaderText="得標商名稱" DataField="OVC_VEN_TITLE" />
                                            <asp:TemplateField HeaderText="辦理現況" >
                                                <ItemTemplate>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label Text="第" runat="server" />
                                                        <asp:Label ID="labONB_SHIP_TIMES" runat="server" />
                                                        <asp:Label Text="批契約交貨日期：" runat="server" />
                                                        <asp:Label ID="labOVC_DELIVERY_CONTRACT" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label Text="實際交貨日期：" runat="server" />
                                                        <asp:Label ID="labOVC_DELIVERY" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labTimeSpan" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labMain" Text="主要事項記載：" runat="server" />
                                                        <asp:Label ID="labOVC_DELAY_REASON" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labCashT" Text="" runat="server" />
                                                        <asp:Label ID="labCash" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labPromT" Text="" runat="server" />
                                                    </p>
                                                    <p>
                                                        <asp:Label ID="labProm" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labPromT_2" Text="" runat="server" />
                                                    </p>
                                                    <p>
                                                        <asp:Label ID="labProm_2" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labPromT_3" Text="" runat="server" />
                                                    </p>
                                                    <p>
                                                        <asp:Label ID="labProm_3" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labStockT" Text="" runat="server" />
                                                    </p>
                                                    <p>
                                                        <asp:Label ID="labStock" Text="" runat="server" />
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labMargin" Text="" runat="server" />
                                                    </p>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="作業">
                                                <ItemTemplate>
                   		                            <asp:Button ID="BtnSelect" CssClass="btn-success btnw2 text-center" OnClick="BtnSelect_Click" Text="選擇" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                	                    </Columns>
	   		                        </asp:GridView>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
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
