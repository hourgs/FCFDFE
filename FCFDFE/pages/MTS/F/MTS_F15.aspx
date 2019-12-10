<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F15.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F15" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    幣別幣值資料維護
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width: 200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">日期</asp:Label>
                                    </td>
                                    <td style="width: 800px;" colspan="3">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOdtCreateDate1" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOdtCreateDate2" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnQuery" CssClass="btn-success" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                                <br />
                                <br />
                                <asp:Button ID="btnSave" CssClass="btn-success" runat="server" Text="維護貨幣資料" OnClick="btnSave_Click" />
                                <asp:Button ID="brnAdd" CssClass="btn-success" runat="server" Text="新增貨幣資料" OnClick="brnAdd_Click" />
                                <br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CURRENCY" DataKeyNames="OVC_CURRENCY_CODE" CssClass="table data-table2 table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CURRENCY_PreRender" OnRowCommand="GV_TBGMT_CURRENCY_RowCommand" RowStyle-HorizontalAlign="Center" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="ODT_DATE" HeaderText="日期" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="true" />
                                    <asp:BoundField DataField="OVC_CURRENCY_CODE" HeaderText="幣別代碼" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="幣別名稱">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_CURRENCY_NAME" Text='<%#( Eval("OVC_CURRENCY_NAME").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOVC_CURRENCY_NAME" Text='<%#( Eval("OVC_CURRENCY_NAME").ToString() )%>' CssClass="tb tb-s" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="與新台幣兌換比例">
                                        <ItemTemplate>
                                            <asp:Label ID="lblONB_RATE" Text='<%#( Eval("ONB_RATE").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtONB_RATE" Text='<%#( Eval("ONB_RATE").ToString() )%>' CssClass="tb tb-s" runat="server" />
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
