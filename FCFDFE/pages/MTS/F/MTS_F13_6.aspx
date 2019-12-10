<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F13_6.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F13_6" %>

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
                    運費折扣維護-修改/刪除
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width: 200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">運費名稱</asp:Label>
                                    </td>
                                    <td style="width: 800px;" colspan="3">
                                        <asp:TextBox ID="txtOvcCarrName" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">運費百分比</asp:Label>
                                    </td>
                                    <td style="width: 800px;" colspan="3">
                                        <asp:TextBox ID="txtOvcCarrRate" CssClass="tb tb-m " TextMode="Number" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">運費型態</asp:Label>
                                    </td>
                                    <td style="width: 800px;" colspan="3">
                                        <asp:DropDownList ID="drpOvcCarrType" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem Selected="True" Value="0">海運</asp:ListItem>
                                            <asp:ListItem Value="1">空運</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">排序</asp:Label>
                                    </td>
                                    <td style="width: 800px;" colspan="3">
                                        <asp:TextBox ID="txtOvcCarrdSort" CssClass="tb tb-xs " TextMode="Number" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnSave" CssClass="btn-warning" runat="server" Text="更新保險費率資料" OnClick="btnSave_Click" />
                                <asp:Button ID="btnDel" CssClass="btn-danger" runat="server" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" Text="刪除保險費率資料" OnClick="btnDel_Click" /><br />
                                <br />
                                <asp:Button ID="btnHome" CssClass="btn-success" runat="server" OnClick="btnHome_Click" Text="回首頁" />
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
