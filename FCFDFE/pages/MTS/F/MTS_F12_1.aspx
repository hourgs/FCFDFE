<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F12_1.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F12_1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
         });

    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    公司/廠商資料維護
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">公司/廠商種類</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:DropDownList  ID="drpOvcCoType" CssClass="tb tb-s" OnSelectedIndexChanged="drpOvcCoType_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            <asp:ListItem Value="2" Selected="True">進艙廠商</asp:ListItem>
                                            <asp:ListItem Value="1" >保險公司</asp:ListItem>
                                            <asp:ListItem Value="3" >航運廠商</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div id="btn_1" style="text-align:center;" runat="server">
                                <asp:Button ID="btnQuery" cssclass="btn-success" Visible="true" Text="查詢" OnClick="btnQuery_Click" runat="server" /><br /><br />
                                <asp:Button ID="btnSave" cssclass="btn-warning" Visible="true" OnClick="btnSave_Click" Text="新增進艙廠商" runat="server" />
                            </div>
                            <div id="btn_2" style="text-align:center;" runat="server">
                                <asp:Button ID="btnQuery_2" Text="查詢" cssclass="btn-success" OnClick="btnQuery_Click" runat="server" /><br /><br />
                                <asp:Button ID="btnSave_2" Text="新增航運廠商" cssclass="btn-warning" OnClick="btnSave_2_Click" runat="server" />
                            </div>
                            <asp:GridView ID="GV_TBGMT_COMPANY" DataKeyNames="CO_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_COMPANY_PreRender" OnRowCommand="GV_TBGMT_COMPANY_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_COMPANY" HeaderText="進艙廠商" />
                                    <asp:BoundField DataField="OVC_CO_TYPE" HeaderText="種類" />
                                    <asp:BoundField DataField="ONB_CO_SORT" HeaderText="排序" />
                                    <asp:BoundField DataField="ODT_CREATE_DATE" HeaderText="資料建立日期"  DataFormatString = "{0:yyyy/MM/dd}" />
                                    <asp:BoundField DataField="OVC_CREATE_ID" HeaderText="資料建立人員" />
                                    <asp:BoundField DataField="ODT_MODIFY_DATE" HeaderText="資料修改日期"  DataFormatString = "{0:yyyy/MM/dd}" />
                                    <asp:BoundField DataField="OVC_MODIFY_LOGIN_ID" HeaderText="資料修改人員" />
                                    <asp:BoundField DataField="ODT_START_DATE" HeaderText="開始日期"   DataFormatString = "{0:yyyy/MM/dd}"/>
                                    <asp:BoundField DataField="ODT_END_DATE" HeaderText="結束日期"  DataFormatString = "{0:yyyy/MM/dd}" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnManagement" CssClass="btn-success" Text="管理" CommandName="btnManagement" runat="server"/>
                                        </ItemTemplate>
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
