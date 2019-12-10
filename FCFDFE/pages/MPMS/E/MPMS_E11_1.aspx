<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E11_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E11_1" %>
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
                    <!--標題-->購案報表
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GV_Reports" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_Reports_PreRender" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:BoundField HeaderText="購案承辦人" DataField="OVC_PUR_USER" />
                                    <asp:BoundField HeaderText="核定文號" DataField="OVC_PUR_APPROVE" />
                                    <asp:BoundField HeaderText="核定日期" DataField="OVC_PUR_DAPPROVE" />
                                    <asp:BoundField HeaderText="履驗承辦人" DataField="OVC_DO_NAME" />
                                    <asp:BoundField HeaderText="履驗狀況" DataField="OVC_STATUS" />
                                    <asp:BoundField HeaderText="結案日" DataField="OVC_DCLOSE" />
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
