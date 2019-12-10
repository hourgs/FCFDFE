<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserLoginLog.aspx.cs" Inherits="FCFDFE.pages.GM.UserLoginLog" %>

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
                            使用者登入紀錄
                        </header>
                        <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <!--預留空間，未來做錯誤訊息顯示。-->
                        <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                                <div class="cmxform form-horizontal tasi-form">
                                    <div>
                                        <table class="text-center">
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">登入時間：</asp:Label>
                                                <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                    <asp:TextBox ID="txtDateStart" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                    <asp:Label CssClass="control-label" runat="server">～</asp:Label>
                                                <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                    <asp:TextBox ID="txtDateEnd" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                    <asp:Button ID="btnLoginDate" CssClass="btn-success btnw2" runat="server" OnClick="btnLoginDate_Click" Text="查詢" />
                                                </td>
                                            </tr>
                                        </table>
                                        </div>
                                    <asp:GridView ID="GV_ACCOUNT" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_ACCOUNT_PreRender" runat="server">
                                        <Columns>
                                            <asp:BoundField DataField="USER_NAME" HeaderText="帳號名稱" />
                                            <asp:BoundField DataField="LOGIN_TIME" HeaderText="登入時間" />
                                            <asp:BoundField DataField="LOGOUT_TIME" HeaderText="登出時間" />
                                            <asp:BoundField DataField="IP_ADDRESS" HeaderText="IP位址" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <footer class="panel-footer text-center">
                            <!--網頁尾-->
                        </footer>
                    </section>
                <%--</ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>
