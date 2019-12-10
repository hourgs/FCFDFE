<%@ Page Title="" Language="C#" MasterPageFile="~/Super.Master" AutoEventWireup="true" CodeBehind="Super_Purchase_Track.aspx.cs" Inherits="FCFDFE.pages.Sup.Super_Purchase_Track" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="background-color: red;">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    使用者探查
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered" style="width: 700px;">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案案號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIPURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案階段：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpSTATE_TYPE" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="" Selected="True">請選擇</asp:ListItem>
                                            <asp:ListItem Value="計畫評核">計畫評核</asp:ListItem>
                                            <asp:ListItem Value="採購發包">採購發包</asp:ListItem>
                                            <asp:ListItem Value="履約驗結">履約驗結</asp:ListItem>
                                            <asp:ListItem Value="結案">結案</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">目前狀態</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpSTATE_NOW" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="" Selected="True">請選擇</asp:ListItem>
                                            <asp:ListItem Value="待分案">待分案</asp:ListItem>
                                            <asp:ListItem Value="承辦中">承辦中</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">承辦人</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUSER_NOW" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="query" CssClass="btn-success" OnClick="query_Click" runat="server" Text="查詢" /><br />
                            </div>

                            <div class="subtitle">查詢結果</div>
                            <asp:GridView ID="GV_Purchase_Track" CssClass=" table data-table table-striped border-top table-bordered" AutoGenerateColumns="false" OnPreRender="GV_Purchase_Track_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="購案案號" DataField="PURCH" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="IPURCH" />
                                    <asp:BoundField HeaderText="購案階段" DataField="STATE_TYPE" />
                                    <asp:BoundField HeaderText="目前階段" DataField="STATE_NOW" />
                                    <asp:BoundField HeaderText="承辦人" DataField="USER_NOW" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
