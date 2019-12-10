<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A27_1.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A27_1" %>
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
                    <div>出口報單建立-Step1 選擇提單</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:25%;"><asp:Label CssClass="control-label" runat="server">提單編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                            </div>
                            <asp:GridView ID="GVTBGMT_BLD" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" OnPreRender="GVTBGMT_BLD_PreRender" OnRowCommand="GVTBGMT_BLD_RowCommand" OnRowDataBound="GVTBGMT_BLD_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a id="hrefQuote" href="javascript:var win=window.open('BLDDATA?id=<%# FCommon.getEncryption(Eval("OVC_BLD_NO").ToString()) %>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="承運航商" DataField="OVC_SHIP_COMPANY" />
                                    <asp:BoundField HeaderText="海空運別" DataField="OVC_SEA_OR_AIR" />
                                    <asp:BoundField HeaderText="船機名稱" DataField="OVC_SHIP_NAME" />
                                    <asp:BoundField HeaderText="船機航次" DataField="OVC_VOYAGE" />
                                    <asp:BoundField HeaderText="啟運船埠" DataField="OVC_START_PORT" />
                                    <asp:BoundField HeaderText="抵運船埠" DataField="OVC_ARRIVE_PORT" />
                                    <asp:BoundField HeaderText="件數" DataField="ONB_QUANITY" />
                                    <asp:BoundField HeaderText="體積" DataField="ONB_VOLUME" />
                                    <asp:BoundField HeaderText="重量" DataField="ONB_WEIGHT" />
                                    <asp:BoundField HeaderText="運費" DataField="ONB_CARRIAGE" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
										<ItemTemplate>
											<asp:button ID="btnNew" CssClass="btn-success btnw2" Text="建立" CommandName="btnNew" CommandArgument='' runat="server"/>
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

