<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B23.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B23" %>
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
                    結報申請表-新增
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案由</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
<%--                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>&nbsp;&nbsp;--%>
                                        <asp:TextBox ID="txtOVC_GIST" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目及編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpOVC_BUDGET" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">未輸入</asp:ListItem>
                                            <asp:ListItem Value="維持門010105">維持門010105</asp:ListItem>
                                            <asp:ListItem Value="投資門150110">投資門150110</asp:ListItem>
                                            <asp:ListItem Value="維持門010106">維持門010106</asp:ListItem>
                                            <asp:ListItem Value="投資門150111">投資門150111</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td colspan="2">
                                       <asp:TextBox ID="txtOVC_PURPOSE_TYPE" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請日期</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_APPLY_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算通知單編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtOvcBudgetInfNo" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-success " Text="載入預算通知單編號" runat="server" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收據號碼</asp:Label>
                                    </td>
                                    <td colspan="2">
                                       <asp:TextBox ID="txtOVC_INV_NO" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收據日期</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INV_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtOVC_NOTE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        <%--<asp:ListBox ID="lstOVC_NOTE" CssClass="tb tb-m"  runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:ListBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險公司</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpCO_SN" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtOVC_PLN_CONTENT" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        <%--<asp:ListBox ID="lstOvcPlnContent" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:ListBox>--%>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button cssclass="btn-warning" OnClick="btnSave_Click" Text="新增結報申請表" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
