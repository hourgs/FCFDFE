<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A13_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A13_1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script type="text/javascript"> 
        function openJobshow(news_id) {
            window.open('wk_Jobshow.aspx?rid=' + news_id, 'width=230,height=220', '');

        };
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>作業時程管制簿-Step1 選擇提單</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 35%">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 35%">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">航次</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_VOYAGE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">機敏軍品</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_IS_SECURITY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                            </div>
                            <br>
                            <asp:GridView ID="GVTBGMT_BLD" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_BLD_PreRender" OnRowCommand="GVTBGMT_BLD_RowCommand" OnRowDataBound="GVTBGMT_BLD_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<asp:LinkButton ID="btnOVC_BLD_NO" runat="server"></asp:LinkButton>--%>
                                            <%--<a id="hrefQuote" href="javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="承運航商" DataField="OVC_SHIP_COMPANY" />
                                    <asp:BoundField HeaderText="海空運別" DataField="OVC_SEA_OR_AIR" />
                                    <asp:BoundField HeaderText="船機名稱" DataField="OVC_SHIP_NAME" />
                                    <asp:BoundField HeaderText="船機航次" DataField="OVC_VOYAGE" />
                                    <asp:BoundField HeaderText="啟運港埠" DataField="OVC_START_PORT" />
                                    <asp:BoundField HeaderText="抵達港埠" DataField="OVC_ARRIVE_PORT" />
                                    <asp:BoundField HeaderText="件數" DataField="ONB_QUANITY" />
                                    <asp:BoundField HeaderText="體積" DataField="ONB_VOLUME" />
                                    <asp:BoundField HeaderText="重量" DataField="ONB_WEIGHT" />
                                    <asp:BoundField HeaderText="運費" DataField="ONB_CARRIAGE" />
                                    <asp:BoundField HeaderText="機敏軍品" DataField="OVC_IS_SECURITY" />
                                    <asp:TemplateField HeaderText="管理簿" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnNew" CssClass="btn-warning btnw2" Text="建立" CommandName="btnNew" CommandArgument="<%# Container.DataItemIndex%>" runat="server" />
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
