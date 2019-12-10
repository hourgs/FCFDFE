<%@ Page Title="" Language="C#" MasterPageFile="~/Super.Master" AutoEventWireup="true" CodeBehind="Super_IP_BLACKLIST.aspx.cs" Inherits="FCFDFE.pages.Sup.Super_IP_BLACKLIST" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row" style="background-color: red;">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                                    <!--標題-->
                                    網路黑名單維護
                                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="BlackList_Query" runat="server">
                                <header class="title">
                                    <!--標題-->
                                    黑名單查詢
                                </header>
                                <table class="table table-bordered" style="width: 700px;">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label text-blue" runat="server">使用者IP</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIP" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label text-blue" runat="server">封鎖日期</asp:Label>
                                        </td>
                                        <td>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txt_SDATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <asp:Label CssClass="control-label" runat="server">至</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txt_EDATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>

                                        </td>
                                    </tr>
                                </table>
                                <div class="text-center">
                                    <asp:Button ID="query" CssClass="btn-success" OnClick="query_Click" runat="server" Text="查詢" />
                                    <asp:Button ID="BL_ADD" CssClass="btn-success" OnClick="BL_ADD_Click" runat="server" Text="新增封鎖IP" />
                                </div>

                                <div class="subtitle">查詢結果</div>
                                <asp:GridView ID="GV_BlackList" CssClass=" table data-table table-striped border-top table-bordered" DataKeyNames="SN" AutoGenerateColumns="false" OnPreRender="GV_BlackList_PreRender" OnRowCommand="GV_BlackList_RowCommand" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="封鎖IP/網段" DataField="IP" />
                                        <asp:BoundField HeaderText="封鎖日期" DataField="DATE" />
                                        <asp:BoundField HeaderText="封鎖理由" DataField="REASON" />
                                        <asp:TemplateField HeaderText="選擇">
                                            <ItemTemplate>
                                                <asp:Button CssClass="btn-danger btnw2" CommandName="DataDel" runat="server" Text="刪除" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <asp:Panel ID="BlackList_Add" runat="server" Visible="false">
                                <header class="title">
                                    <!--標題-->
                                    黑名單新增
                                </header>
                                <table class="table table-bordered" style="width: 700px;">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label text-blue" runat="server">封鎖IP/網段</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIP_add" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label text-blue" runat="server">封鎖原因</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtREASON" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div class="text-center">
                                    <asp:Button ID="add" CssClass="btn-success" OnClick="add_Click" runat="server" Text="新增" />
                                    <asp:Button ID="return" CssClass="btn-success" OnClick="return_Click" runat="server" Text="回查詢頁面" />
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
