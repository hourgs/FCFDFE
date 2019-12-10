<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F16.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F16" %>

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
                    接轉單位軍種維護作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width: 200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">單位種類查詢</asp:Label>
                                    </td>
                                    <td style="width: 800px;" colspan="3">
                                        <asp:DropDownList ID="drpOvcClass" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem>全部</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnQuery" CssClass="btn-success" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                                <br />
                                <br />
                                <asp:Button ID="btnSave" CssClass="btn-success" runat="server" Text="新增接轉單位軍種" OnClick="btnSave_Click" />
                                <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button1" CssClass="btn-success" runat="server" Text="維護接轉單位軍種" />--%>
                                <br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_DEPT_CLASS" DataKeyNames="OVC_DEPTCLA_SN" CssClass="table data-table2 table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_DEPT_CLASS_PreRender" OnRowCommand="GV_TBGMT_DEPT_CLASS_RowCommand" runat="server" RowStyle-HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="類別代碼">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_CLASS" Text='<%#( Eval("OVC_CLASS").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOVC_CLASS" Text='<%#( Eval("OVC_CLASS").ToString() )%>' CssClass="tb tb-s" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="類別名稱">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_CLASS_NAME" Text='<%#( Eval("OVC_CLASS_NAME").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOVC_CLASS_NAME" Text='<%#( Eval("OVC_CLASS_NAME").ToString() )%>' CssClass="tb tb-s" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="排序">
                                        <ItemTemplate>
                                            <asp:Label ID="lblONB_SORT" Text='<%#( Eval("ONB_SORT").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtONB_SORT" Text='<%#( Eval("ONB_SORT").ToString() )%>' CssClass="tb tb-s" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="管理">
                                        <ItemTemplate>
                                            <asp:Button ID="btnedit" CssClass="btn-success btnw2" Text="編輯" CommandName="DataEdit" runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnupdate" CssClass="btn-success btnw2" Text="更新" CommandName="DataSave" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="DataDelete" runat="server" OnClientClick="if (confirm('確定刪除?') == false) return false;" />
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
                </footer>
            </section>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            var num = <%=intRowIndex%>;
            $('.data-table2').dataTable({
                "sDom": "<'row'<'col-sm-6'l><'col-sm-6'f>r>t<'row'<'col-sm-6'i><'col-sm-6'p>>",
                "sPaginationType": "bootstrap",
                "oLanguage": {
                    "sLengthMenu": "顯示 _MENU_ 筆記錄",
                    "oPaginate": {
                        "sPrevious": "上頁",
                        "sNext": "下頁"
                    },
                },
                "iDisplayStart": num,
                "bDestroy": true,
                "bSort": false,
                "aoColumnDefs": [{
                    'bSortable': false,
                    'aTargets': [0]
                }]
            });
        });
    </script>
</asp:Content>
