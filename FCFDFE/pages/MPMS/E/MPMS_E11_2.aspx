<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E11_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E11_2" %>
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
                    <!--標題-->購案移辦資料
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GV_Transferring" CssClass=" table data-table table-striped border-top text-center" OnRowDataBound="GV_Transferring_RowDataBound" OnPreRender="GV_Transferring_PreRender" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                            <asp:Label ID="labOVC_PURCH" Text='<%# Bind("OVC_PURCH") %>' Visible="false" runat="server" />
                                            <asp:Label ID="labOVC_PUR_AGENCY" Text='<%# Bind("OVC_PUR_AGENCY") %>' Visible="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="合約編碼" DataField="OVC_PURCH_6" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:BoundField HeaderText="廠商名稱" DataField="OVC_VEN_TITLE" />
                                    <asp:BoundField HeaderText="移辦日期" DataField="OVC_DATE" />
                                    <asp:BoundField HeaderText="移辦人" DataField="OVC_USER" />
                                    <asp:BoundField HeaderText="移辦單位名稱" DataField="OVC_FROM_UNIT_NAME" />
                                    <asp:BoundField HeaderText="接收單位名稱" DataField="OVC_TO_UNIT_NAME" />
                                    <asp:BoundField HeaderText="備註" DataField="OVC_REMARK" />
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
