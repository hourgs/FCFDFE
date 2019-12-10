<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountCheck.aspx.cs" Inherits="FCFDFE.pages.GM.AccountCheck" %>

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
                    帳號管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered" style="width: 700px;">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位代碼：</asp:Label>
                                        <asp:TextBox ID="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <input type="button" value="單位查詢" class="btn-success" onclick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">姓名：</asp:Label>
                                        <asp:TextBox ID="txtUSER_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">帳號狀態：</asp:Label>
                                        <asp:DropDownList ID="drpACCOUNT_STATUS" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="2">請選擇</asp:ListItem>
                                            <asp:ListItem Value="1">使用中</asp:ListItem>
                                            <asp:ListItem Value="0">停用中</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success" OnClick="btnQuery_Click" runat="server" Text="查詢" /><br />
                            </div>
                            <asp:GridView ID="GV_AccountCheck" DataKeyNames="AC_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowCommand="GV_AccountCheck_RowCommand" OnPreRender="GV_AccountCheck_PreRender" OnRowDataBound="GV_AccountCheck_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="DEPT_SN" HeaderText="單位" />
                                    <asp:BoundField DataField="USER_ID" HeaderText="帳號" />
                                    <asp:BoundField DataField="USER_NAME" HeaderText="姓名" />
                                    <asp:BoundField DataField="IUSER_PHONE" HeaderText="電話(軍線)" />
                                    <asp:TemplateField HeaderText="帳號狀態" HeaderStyle-Width="10em">
                                        <ItemTemplate>
                                            <asp:Label ID="lblACCOUNT_STATUS" CssClass="control-label" Text='<%#( Eval("ACCOUNT_STATUS").ToString() )%>' Visible="false" runat="server"></asp:Label>
                                            <asp:RadioButtonList ID="rdoACCOUNT_STATUS" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                <asp:ListItem Value="1">使用中</asp:ListItem>
                                                <asp:ListItem Value="0">停用中</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="作業" HeaderStyle-Width="8em" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSave" Text="存檔" CssClass="btn-warning" CommandName="DataSave" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
