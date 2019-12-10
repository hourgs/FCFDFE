<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A17_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A17_1" %>

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
                    <div>進口物資管制接配紀錄表-Step1 選擇提單</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width: 25%">
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
                            <asp:GridView ID="GV_TBGMT_BLD" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top" style="margin-top: 20px;" AutoGenerateColumns="false" OnRowCommand="GV_TBGMT_BLD_RowCommand" OnPreRender="GV_TBGMT_BLD_PreRender" OnRowDataBound="GV_TBGMT_BLD_RowDataBound" runat="server">
                                <Columns>
                                    <%--<asp:HyperLinkField HeaderText ="提單編號" Text="OVC_BLD_NO" DataTextField="OVC_BLD_NO" DataNavigateUrlFields="OVC_BLD_NO" DataNavigateUrlFormatString="BLDDATA?OVC_BLD_NO={0}"   
                                NavigateUrl="javascript:window.open('BLDDATA?OVC_BLD_NO={0}','','toolbar=0,width=600,height=500')" Target="_blank"/>--%>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a href="javascript:var win=window.open('BLDDATA?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=250,top=270');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="提單號碼" DataField="OVC_BLD_NO" />--%>
                                    <asp:BoundField HeaderText="承運航商" DataField="OVC_SHIP_COMPANY" />
                                    <asp:BoundField HeaderText="船機名稱" DataField="OVC_SHIP_NAME" />
                                    <asp:BoundField HeaderText="船機航次" DataField="OVC_VOYAGE" />
                                    <asp:BoundField HeaderText="啟運船埠" DataField="OVC_START_PORT" />
                                    <asp:BoundField HeaderText="抵運船埠" DataField="OVC_ARRIVE_PORT" />
                                    <asp:BoundField HeaderText="件數" DataField="ONB_QUANITY" />
                                    <asp:BoundField HeaderText="體積" DataField="ONB_VOLUME" />
                                    <asp:BoundField HeaderText="重量" DataField="ONB_WEIGHT" />
                                    <asp:BoundField HeaderText="運費" DataField="ONB_CARRIAGE" />
                                    <asp:TemplateField HeaderText="接配紀錄表" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnNew" CssClass="btn-success btnw2" Text="建立" CommandName="DataCreate" runat="server" />
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

