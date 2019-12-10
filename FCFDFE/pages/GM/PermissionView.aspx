<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PermissionView.aspx.cs" Inherits="FCFDFE.pages.GM.PermissionView" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>--%>
                    <section class="panel">
                        <header class="title">
                            <!--標題-->
                            <asp:Label CssClass="control-label" runat="server">各權限可查看頁面</asp:Label>
                        </header>
                        <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <!--預留空間，未來做錯誤訊息顯示。-->
                        <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                                <div class="cmxform form-horizontal tasi-form">
                                    <!--網頁內容-->
                                    <asp:Panel ID="PnMessage_Permission" runat="server"></asp:Panel>
                                    <table class="table text-center">
                                        <tr>
                                            <td style="width: 12%">
                                                <asp:Label CssClass="control-label subtitle" runat="server">權限角色</asp:Label>
                                            </td>
                                            <td style="width: 28%">
                                                <asp:Label CssClass="control-label" runat="server">系統別</asp:Label>
                                                <asp:DropDownList ID="drpC_SN_SYS" CssClass="tb tb-m" OnSelectedIndexChanged="drpC_SN_SYS_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                            </td>
                                            <td style="width: 35%">
                                                <asp:Label CssClass="control-label" runat="server">使用者角色</asp:Label>
                                                <asp:DropDownList ID="drpC_SN_ROLE" CssClass="tb tb-m" OnSelectedIndexChanged="drpC_SN_ROLE_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                            </td>
                                            <td style="width: 25%">
                                                <asp:Label CssClass="control-label" runat="server">使用者權限</asp:Label>
                                                <asp:DropDownList ID="drpC_SN_AUTH" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <hr>
                                    <table class="table text-center">
                                        <tr>
                                            <td style="width: 12%">
                                                <asp:Label CssClass="control-label subtitle" runat="server">畫面模組</asp:Label>
                                            </td>
                                            <td style="width: 28%">
                                                <asp:Label CssClass="control-label" runat="server">系統別</asp:Label>
                                                <%--<asp:DropDownList ID="drpM_SN_PAR" CssClass="tb tb-m" AutoPostBack="true" OnSelectedIndexChanged="drpM_SN_PAR_SelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                                <asp:DropDownList ID="drpM_System" CssClass="tb tb-m" OnSelectedIndexChanged="drpM_System_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                            </td>
                                            <td style="width: 35%">
                                                <asp:Panel ID="pnModel" runat="server">
                                                    <asp:Label CssClass="control-label" runat="server">模組名稱</asp:Label>
                                                    <%--<asp:DropDownList ID="drpM_NAME" CssClass="tb tb-m" runat="server"></asp:DropDownList>--%>
                                                    <asp:DropDownList ID="drpM_Model" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                                </asp:Panel>
                                            </td>
                                            <td style="width: 25%">
                                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br>
                                    <table class="table text-center">
                                        <tr>
                                            <td style="width: 45%">
                                                <asp:Label CssClass="control-label" runat="server">可設定功能項</asp:Label><br>
                                                <asp:ListBox ID="lstSetFunction" OnSelectedIndexChanged="lstSetFunction_SelectedIndexChanged" AutoPostBack="true" Width="75%" Font-Size="Larger" Rows="10" runat="server"></asp:ListBox>
                                                <asp:ListBox ID="lstAdd" Visible="false" runat="server"></asp:ListBox>
                                                <asp:ListBox ID="lstDel" Visible="false" runat="server"></asp:ListBox>
                                            </td>
                                            <td style="width: 10%">
                                                <asp:Label CssClass="control-label" runat="server">➯</asp:Label>
                                            </td>
                                            <td style="width: 45%">
                                                <asp:Label CssClass="control-label" runat="server">已設定功能項</asp:Label><br>
                                                <asp:ListBox ID="lstUseFunction" OnSelectedIndexChanged="lstUseFunction_SelectedIndexChanged" AutoPostBack="true" Width="75%" Font-Size="Larger" Rows="10" runat="server"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="text-center">
                                        <asp:Button ID="btnSave" CssClass="btn-warning btnw2" Text="確認" OnClick="btnSave_Click" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <footer class="panel-footer text-center">
                            <!--網頁尾-->
                        </footer>
                    </section>
                <%--</ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>
