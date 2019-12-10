<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E22.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E22" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->
                    免稅申請
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GV_FREE_TAX" DataKeyNames="OVC_PURCH" CssClass="table data-table table-striped border-top text-center" OnRowDataBound="GV_FREE_TAX_RowDataBound" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button ID="btnModify" CssClass="btn-success btnw2" Text="異動" OnClick="btnModify_Click" CommandName="DataModify" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="序號" DataField="" />
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="交貨批次" DataField="ONB_SHIP_TIMES" />
                                    <asp:BoundField HeaderText="稅賦類別" DataField="OVC_DUTY_KIND" />
                                    <asp:BoundField HeaderText="申請次數" DataField="ONB_NO" />
                                    <asp:BoundField HeaderText="貨品名稱" DataField="OVC_GOOD_DESC" />
                                </Columns>
                            </asp:GridView>
                            <div style="margin-top: 2%;">
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <th><asp:Label CssClass="control-label" runat="server">序號</asp:Label></th>
                                        <th><asp:Label CssClass="control-label" runat="server">購案編號</asp:Label></th>
                                        <th><asp:Label CssClass="control-label" runat="server">交貨批次</asp:Label></th>
                                        <th><asp:Label CssClass="control-label" runat="server">進口稅</asp:Label></th>
                                        <th><asp:Label CssClass="control-label" runat="server">營業稅</asp:Label></th>
                                        <th><asp:Label CssClass="control-label" runat="server">貨物稅</asp:Label></th>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="Num" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td><asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td><asp:TextBox ID="txtONB_SHIP_TIMES" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                        <td><asp:Button ID="btnKAA" CssClass="btn-default btnw2" OnClick="btnKAA_Click" runat="server" Text="新增" /></td>
                                        <td><asp:Button ID="btnKBA" CssClass="btn-default btnw2" OnClick="btnKBA_Click" runat="server" Text="新增" /></td>
                                        <td><asp:Button ID="btnKCA" CssClass="btn-default btnw2" OnClick="btnKCA_Click" runat="server" Text="新增" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="text-center">
                                <asp:Button Style="" ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                <asp:Button Style="" ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
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
