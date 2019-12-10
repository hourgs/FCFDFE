<%@ Page TITLE="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BulletinBoardAdd.aspx.cs" Inherits="FCFDFE.pages.GM.BulletinBoardAdd" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .lbl-system {
            display: block;
            font-size: 18px;
            margin: 10px 3px 5px 3px;

            font-weight: 300;
            text-align: left;
            padding: 5px 0 0 0;
        }
        .chk-item {
            display: block;
            margin: 0 0 10px 20px;
        }
    </style>
    <div class="row">
        <div style="width: 900px; margin: auto;">
                    <section class="panel">
                        <header class="title">
                            公佈欄-新增
                        </header>
                        <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <!--預留空間，未來做錯誤訊息顯示。-->
                        <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                                <div class="cmxform form-horizontal tasi-form">
                                    <asp:Panel ID="PnMessage_Insert" runat="server"></asp:Panel>
                                    <table class="table table-bordered">
                                        <tr>
                                            <td style="width: 100px;">
                                                <asp:Label CssClass="control-label" Text="狀態" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpSTATUS" CssClass="tb tb-s" runat="server">
                                                    <asp:ListItem Text="Y：啟用" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="N：停用" Value="N"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 100px;">
                                                <asp:Label CssClass="control-label" Text="結束日期" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <div class="input-append datepicker">
                                                    <asp:TextBox ID="txtEND_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label CssClass="control-label" Text="標題" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTITLE" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="valTITLE"
                                                    runat="server"
                                                    ControlToValidate="txtTITLE"
                                                    ForeColor="Red"
                                                    ErrorMessage="必填">
                                                </asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label CssClass="control-label" Text="內容" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtCONTEXT" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="valCONTEXT"
                                                    runat="server"
                                                    ControlToValidate="txtCONTEXT"
                                                    ForeColor="Red"
                                                    ErrorMessage="必填">
                                                </asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr class="no-bordered-seesaw">
                                            <td>
                                                <asp:Label CssClass="control-label" Text="閱讀群組" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chk_allcheck" Text="全選" CssClass="radioButton chk-item" OnCheckedChanged="chk_allcheck_CheckedChanged" AutoPostBack="true" runat="server" />
                                                <asp:Panel ID="pn_CheckBox" runat="server"></asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="text-center">
                                        <asp:Button CssClass="btn-warning btnw2" OnClick="btnSave_Click" Text="新增" runat="server" />
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
