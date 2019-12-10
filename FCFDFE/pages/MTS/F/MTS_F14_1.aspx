<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F14_1.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F14_1" %>
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
                    機場港口中英文資料維護
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">海港或機場</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:DropDownList  ID="drpOvcPortType" OnSelectedIndexChanged="drpOvcPortType_SelectedIndexChanged"  AutoPostBack="true" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem Selected="True">海港</asp:ListItem>
                                            <asp:ListItem>機場</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="text-center">
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">航線</asp:Label>
                                    </td>
                                    <td colspan="3">
                                       <asp:DropDownList  ID="drpOVC_ROUTE" CssClass="tb tb-s position-left" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success" runat="server" Text="查詢"  OnClick="btnQuery_Click"/> <br /><br />
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="新增機場港口資料" OnClick="btnSave_Click"/>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnRoute" cssclass="btn-warning" runat="server" Text="航線設定/查詢" OnClick="btnRoute_Click" /><br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_PORTS" DataKeyNames="OVC_PORT_CDE"  CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_PORTS_PreRender"  OnRowCommand="GV_TBGMT_PORTS_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_PORT_CDE" HeaderText="代碼" />
                                    <asp:BoundField DataField="OVC_PORT_CHI_NAME" HeaderText="中文名稱" />
                                    <asp:BoundField DataField="OVC_PORT_ENG_NAME" HeaderText="英文名稱" />
                                    <asp:BoundField DataField="OVC_PORT_TYPE" HeaderText="海港或機場" />
                                    <asp:BoundField DataField="OVC_IS_ABROAD" HeaderText="國外或國內" />
                                    <asp:BoundField DataField="OVC_ROUTE" HeaderText="航線" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnManagement" CssClass="btn-success" Text="管理" CommandName="btnManage" runat="server"/>
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
