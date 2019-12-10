<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_B13.aspx.cs" Inherits="FCFDFE.pages.CIMS.B.CIMS_B13" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 800px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>底價表上傳查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="P1" runat="server">
                                <table class="table table-bordered text-left" style="width: 800px; margin: auto">
                                    <tr>
                                        <td>【報表名稱】
                                            <asp:TextBox ID="year" runat="server" Width="25px" MaxLength="2"></asp:TextBox>年度 底價表上傳現況表 
                                            <asp:Button ID="btnquery" runat="server" CssClass="btn-success btnw5" Text="查詢" OnClick="btnquery_Click" />
                                            <asp:LinkButton ID="export" Text='列印表單' runat="server" OnClick="export_Click" Visible="false"></asp:LinkButton>

                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="P2" runat="server">
                                <asp:GridView ID="GV_TBM1301" CssClass="table data-table table-striped border-top table-bordered" AutoGenerateColumns="false" runat="server" OnPreRender="GV_TBM1301_PreRender">
                                    <Columns>
                                        <asp:BoundField HeaderText="項次" DataField="RANK" ItemStyle-Width="8%" />
                                        <asp:BoundField HeaderText="購購案號" DataField="OVC_PURCH" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="採購單位" DataField="OVC_PUR_AGENCY" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="組" DataField="OVC_ITEM" ItemStyle-Width="6%" />
                                        <asp:BoundField HeaderText="次" DataField="OVC_TIMES" ItemStyle-Width="6%" />
                                        <asp:BoundField HeaderText="附件序號" DataField="OVC_SUB" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="檔案名稱" DataField="OVC_FILE_NAME" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="商情承辦人" DataField="Undertaker" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="上傳者" DataField="OVC_UP_USER" ItemStyle-Width="10%" />
                                    </Columns>
                                </asp:GridView>

                            </asp:Panel>
                            <br />
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
