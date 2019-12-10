<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F18.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F18" %>

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
                    清運方式資料維護
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width: 200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">資料建立日期</asp:Label>
                                    </td>
                                    <td style="width: 800px;" colspan="3">
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">&nbsp;&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtEndDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                         
                                        <asp:CheckBoxList ID="chkOdtApplyDate" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>不限定日期</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnQuery" CssClass="btn-success" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                                <br />
                                <br />
                                <asp:Button ID="btnNew" CssClass="btn-success" runat="server" Text="新增清運方式資料" OnClick="btnNew_Click" />
                                <br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CLEAN" DataKeyNames="CL_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CLEAN_PreRender" OnRowCommand="GV_TBGMT_CLEAN_RowCommand" runat="server" RowStyle-HorizontalAlign="Center" >
                                <Columns>
                                    <asp:TemplateField HeaderText="資料建立日期">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#( Eval("ODT_CREATE_DATE").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%#( Eval("ODT_CREATE_DATE").ToString() )%>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="清運方式">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_CLASS_NAME" Text='<%#( Eval("OVC_WAY").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOVC_CLASS_NAME" Text='<%#( Eval("OVC_WAY").ToString() )%>' CssClass="tb tb-s" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="資料建立人員">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#( Eval("OVC_CREATE_ID").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%#( Eval("OVC_CREATE_ID").ToString() )%>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="資料修改日期">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#( Eval("ODT_MODIFY_DATE").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%#( Eval("ODT_MODIFY_DATE").ToString() )%>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="資料修改人員">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#( Eval("OVC_MODIFY_LOGIN_ID").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%#( Eval("OVC_MODIFY_LOGIN_ID").ToString() )%>' runat="server" />
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
</asp:Content>
