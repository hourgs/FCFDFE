<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F14_4.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F14_4" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                    航線設定/查詢頁面
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-center" style="width:650px">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">港埠：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPortChiName" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">航線：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_ROUTE" CssClass="tb tb-m position-left" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSearch" cssclass="btn-success" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkPort" Text="顯示所有港埠資料" CssClass="radioButton position-left" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table class="table text-center">
                                <tr>
                                    <td style="width: 45%">
                                        <asp:Label CssClass="control-label" runat="server">港埠清單</asp:Label><br>
                                        <asp:ListBox ID="lstSetFunction" OnSelectedIndexChanged="lstSetFunction_SelectedIndexChanged" AutoPostBack="true" Width="75%" Font-Size="Larger" Rows="10" runat="server"></asp:ListBox>
                                        <asp:ListBox ID="lstAdd" Visible="false" runat="server"></asp:ListBox>
                                        <asp:ListBox ID="lstDel" Visible="false" runat="server"></asp:ListBox>
                                    </td>
                                    <td style="width: 10%">
                                        <asp:Label CssClass="control-label" runat="server">➯</asp:Label>
                                    </td>
                                    <td style="width: 45%">
                                        <asp:Label CssClass="control-label" runat="server">歸屬航線</asp:Label><br>
                                        <asp:ListBox ID="lstUseFunction" OnSelectedIndexChanged="lstUseFunction_SelectedIndexChanged" AutoPostBack="true" Width="75%" Font-Size="Larger" Rows="10" runat="server"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="確認" OnClick="btnSave_Click" /><br /><br />
                                <asp:Button ID="btnHome" cssclass="btn-success" runat="server" Text="回首頁" OnClick="btnHome_Click" /><br />
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

