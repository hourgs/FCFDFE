<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A19_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A19_1" %>
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
                    <!--標題-->
                    <div>進口軍品運輸交接單-Step1 選擇提單/箱號</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:25%" >
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-s text-toUpper" runat ="server"></asp:TextBox>
                                    </td>
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">接收單位</asp:Label>
                                    </td>
                                    <td class="text-left" style="width:45%">
                                        <asp:HiddenField ID="txtOVC_DEPT_CDE" runat=Server/>
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button CssClass="btn-success" Text="單位查詢" OnClientClick="OpenWindow('txtOVC_DEPT_CDE','txtOVC_ONNAME')" runat="server"/>
                                        <asp:Button cssclass="btn-default btnw4" Text="資料清空" OnClick="btnResetOVC_DEPT_CDE_CODE_Click" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button cssclass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                            </div>
                            <hr />
                            <div class="text-center">
                                <asp:Button cssclass="btn-success" Text="加入軍品運輸交接單" OnClick="btnNew_Click" runat="server" />
                            </div>
                            <asp:GridView ID="GVTBGMT_IHO" DataKeyNames="OVC_IRDDETAIL_SN" CssClass="table data-table table-striped border-top text-center" style="margin-top: 20px;" AutoGenerateColumns="false" OnPreRender="GVTBGMT_IHO_PreRender" OnRowDataBound="GVTBGMT_IHO_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="選取" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" CssClass="radioButton rb-complex" Text="" CommandName="" CommandArgument='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a href="javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="購案號" DataField="OVC_PURCH_NO" />
                                    <asp:BoundField HeaderText="品名" DataField="OVC_CHI_NAME" />
                                    <asp:BoundField HeaderText="分運單位" DataField="OVC_ONNAME" />
                                    <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" />
                                    <asp:BoundField HeaderText="實收件數" DataField="ONB_ACTUAL_RECEIVE" />
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                            </div>
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