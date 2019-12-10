<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A18_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A18_1" %>

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
                    <!--標題-->
                    <div>進口物資管制接配紀錄表管理</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                            </div>
                            <asp:GridView ID="GVTBGMT_IRD" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top" style="margin-top: 20px;" AutoGenerateColumns="false" OnRowCommand="GVTBGMT_IRD_RowCommand" OnPreRender="GVTBGMT_IRD_PreRender" OnRowDataBound="GVTBGMT_IRD_RowDataBound" runat="server">
                                <Columns>
                                    <%--<asp:BoundField HeaderText="提單號碼" DataField="OVC_BLD_NO" />--%>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a href="javascript:var win=window.open('BLDDATA?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=250,top=270');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="進口日期" DataField="ODT_ARRIVE_PORT_DATE"/>
                                    <asp:BoundField HeaderText="提領/拆櫃日期" DataField="ODT_CLEAR_DATE"/>
                                    <asp:BoundField HeaderText="貨櫃號碼" DataField="OVC_CONTAINER_NO" />
                                    <asp:BoundField HeaderText="實收" DataField="ONB_ACTUAL_RECEIVE" />
                                    <asp:BoundField HeaderText="溢卸" DataField="ONB_OVERFLOW" />
                                    <asp:BoundField HeaderText="短少" DataField="ONB_LESS" />
                                    <asp:BoundField HeaderText="破損" DataField="ONB_BROKEN" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnModify" CssClass="btn-warning btnw2" Text="修改" CommandName="DataEdit" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" CssClass="btn-danger btnw2" Text="刪除" CommandName="DataDelete" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnPrint" CssClass="btn-success btnw2"  Text="列印" CommandName="Print" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="短溢卸" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnPrintOverFlow" CssClass="btn-success btnw2"  Text="列印" CommandName="PrintOver" 
                                                CommandArgument='OVC_BLD_NO+","+ODT_ARRIVE_PORT_DATE' runat="server"  />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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

