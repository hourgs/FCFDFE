<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A28_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A28_1" %>
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
                    <div>出口報單-管理</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:25%;">
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                            </div>
                            <asp:GridView ID="GVTBGMT_ECL" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" OnPreRender="GVTBGMT_ECL_PreRender" OnRowCommand="GVTBGMT_ECL_RowCommand" OnRowDataBound="GVTBGMT_ECL_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a id="hrefQuote" href="javascript:var win=window.open('BLDDATA?id=<%# FCommon.getEncryption(Eval("OVC_BLD_NO").ToString()) %>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="類別代號" DataField="OVC_CLASS_CDE" />
                                    <asp:BoundField HeaderText="類別名稱" DataField="OVC_CLASS_NAME" />
                                    <asp:BoundField HeaderText="報單號碼" DataField="OVC_ECL_NO" />
                                    <asp:BoundField HeaderText="出口關別" DataField="OVC_EXP_TYPE" />
                                    <asp:BoundField HeaderText="船或關代號" DataField="OVC_SHIP_CDE" />
                                    <asp:BoundField HeaderText="裝貨單或收序號" DataField="OVC_PACK_NO" />
                                    <asp:BoundField HeaderText="報關日期" DataField="ODT_EXP_DATE" />
                                    <asp:BoundField HeaderText="貨物存放處所" DataField="OVC_STORED_PLACE" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnModify" CssClass="btn-success btnw2" Text="修改" CommandName="btnModify" CommandArgument='' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="btnDel" CommandArgument='' runat="server"/>
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

