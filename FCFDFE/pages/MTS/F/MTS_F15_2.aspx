<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F15_2.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F15_2" %>

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
                                <asp:Button ID="btnSave" CssClass="btn-warning" runat="server" Text="新增貨幣資料" OnClick="btnSave_Click" /><br />
                                <br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CURRENCY" DataKeyNames="OVC_CURRENCY_CODE" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CURRENCY_PreRender" OnRowCommand="GV_TBGMT_CURRENCY_RowCommand" RowStyle-HorizontalAlign="Center" OnRowDataBound="GV_TBGMT_CURRENCY_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="幣別代碼">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_CURRENCY_CODE" Text='<%#( Eval("OVC_CURRENCY_CODE").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <%--                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOVC_CURRENCY_CODE" Text='<%#( Eval("OVC_CURRENCY_CODE").ToString() )%>' CssClass="tb tb-xs" runat="server" />
                                        </EditItemTemplate>--%>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="幣別名稱">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_CURRENCY_NAME" Text='<%#( Eval("OVC_CURRENCY_NAME").ToString() )%>' Visible="True" runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblOVC_CURRENCY_NAME" Text='<%#( Eval("OVC_CURRENCY_NAME").ToString() )%>' Visible="False" runat="server" />
                                            <asp:TextBox ID="txtOVC_CURRENCY_NAME" Text='<%#( Eval("OVC_CURRENCY_NAME").ToString() )%>' CssClass="tb tb-xs" Visible="True" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="排序">
                                        <ItemTemplate>
                                            <asp:Label ID="lblONB_SORT" Text='<%#( Eval("ONB_SORT").ToString() )%>' Visible="True" runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblONB_SORT" Text='<%#( Eval("ONB_SORT").ToString() )%>' Visible="False" runat="server" />
                                            <asp:TextBox ID="txtONB_SORT" Text='<%#( Eval("ONB_SORT").ToString() )%>' CssClass="tb tb-xs" Visible="True" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="貨幣別">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_TYPE" Text='<%#( Eval("OVC_TYPE").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblOVC_TYPE" Text='<%#( Eval("OVC_TYPE").ToString() )%>' Visible="False" runat="server" />
                                            <asp:DropDownList ID="drpOVC_TYPE" CssClass="tb tb-xs" Visible="True" runat="server">
                                                <asp:ListItem Value="每日">日</asp:ListItem>
                                                <asp:ListItem Value="每週">週</asp:ListItem>
                                                <asp:ListItem Value="每月">月</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="貨幣狀態">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_STATUS" Text='<%#( Eval("OVC_STATUS").ToString() )%>' Visible="True" runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblOVC_STATUS" Text='<%#( Eval("OVC_STATUS").ToString() )%>' Visible="False" runat="server" />
                                            <asp:DropDownList ID="drpOVC_STATUS" CssClass="tb tb-xs" Visible="True" runat="server">
                                                <asp:ListItem Value="Y">Y</asp:ListItem>
                                                <asp:ListItem Value="N">N</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ODT_CREATE_DATE" HeaderText="資料建立日期" ReadOnly="true" />
                                    <asp:BoundField DataField="OVC_CREATE_ID" HeaderText="資料建立人員" ReadOnly="true" />
                                    <asp:BoundField DataField="ODT_MODIFY_DATE" HeaderText="資料修改日期" ReadOnly="true" />
                                    <asp:BoundField DataField="OVC_MODIFY_LOGIN_ID" HeaderText="資料修改人員" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="管理">
                                        <ItemTemplate>
                                            <asp:Button ID="btnedit" CssClass="btn-success btnw2" Text="編輯" CommandName="DataEdit" Enabled='<%# Eval("OVC_CURRENCY_NAME").ToString()!="台幣" %>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnupdate" CssClass="btn-success btnw2" Text="更新" CommandName="DataSave" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="DataDelete" runat="server" Enabled='<%# Eval("OVC_CURRENCY_NAME").ToString()!="台幣" %>' OnClientClick="if (confirm('確定刪除?') == false) return false;" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btncancel" CssClass="btn-success btnw2" Text="取消" CommandName="DataCancel" runat="server" />
                                        </EditItemTemplate>
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
