<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F15_1.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F15_1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .aspNetDisabled {
            width: 50px;
            display: inline-block;
            padding: 4px 8px;
            margin-bottom: 0;
            font-size: 14px;
            font-weight: normal;
            line-height: 1.428571429;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            cursor: pointer;
            border: 1px solid transparent;
            border-radius: 4px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            -o-user-select: none;
            user-select: none;
            background-color: gray !important;
            border-color: gray !important;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    貨幣資料
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div style="text-align: center;">
                                <asp:Button ID="btnSave" CssClass="btn-warning" runat="server" Text="新增貨幣資料" Visible="false" OnClick="btnSave_Click" /><br />
                                <br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CURRENCY" DataKeyNames="OVC_CURRENCY_CODE" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CURRENCY_PreRender" OnRowCommand="GV_TBGMT_CURRENCY_RowCommand" RowStyle-HorizontalAlign="Center" OnRowDataBound="GV_TBGMT_CURRENCY_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="TODAY" HeaderText="資料建立日期" ReadOnly="true" />
                                    <asp:BoundField DataField="OVC_CURRENCY_CODE" HeaderText="幣別代碼" ReadOnly="true" />
                                    <asp:BoundField DataField="OVC_CURRENCY_NAME" HeaderText="幣別名稱" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="與新台幣兌換比例">
                                        <ItemTemplate>
                                            <asp:Label ID="lblONB_RATE" Text='<%#( Eval("ONB_RATE").ToString() )%>' runat="server" />
                                            <asp:TextBox ID="txtONB_RATE" Text='<%#( Eval("ONB_RATE").ToString() )%>' CssClass="tb tb-m" Visible="false" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="管理">
                                        <ItemTemplate>
                                            <asp:Button ID="btnedit" CssClass="btn-success btnw2" Text="編輯" CommandName="DataEdit" Enabled='<%# Eval("OVC_CURRENCY_NAME").ToString()!="台幣" %>' runat="server" />
                                            <asp:Button ID="btnupdate" CssClass="btn-success btnw2" Text="更新" CommandName="DataSave" Visible="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="DataDelete" runat="server" Enabled='<%# Eval("OVC_CURRENCY_NAME").ToString()!="台幣" %>' OnClientClick="if (confirm('確定刪除?') == false) return false;" />
                                            <asp:Button ID="btncancel" CssClass="btn-success btnw2" Text="取消" CommandName="DataCancel" Visible="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <asp:Button ID="btnBack" CssClass="btn-default" Text="回上一頁" OnClick="btnBack_Click" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
