<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E16.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E16" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <asp:Panel ID="panS" runat="server">
                <section class="panel">
                    <header class="title">
                        運費統計查詢
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                    <!--預留空間，未來做錯誤訊息顯示。-->
                    <div class="panel-body" style="border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <table class="table table-bordered">
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle;">
                                            <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="drpOdtInvDate" CssClass="tb tb-s" runat="server">
                                                <asp:ListItem>內容1</asp:ListItem>
                                                <asp:ListItem>內容1</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnQuery" CssClass="btn-success btnw2" runat="server" OnClick="btnQuery_Click" Text="查詢" />&nbsp;&nbsp;
                                <asp:Button ID="btnToD" CssClass="btn-success btnw" runat="server" OnClick="btnToD_Click" Text="切換至運費明細查詢" /><br />
                                    <br />
                                </div>
                                <asp:GridView ID="GV_TBGMT_CINF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CINF_PreRender" runat="server">
                                    <Columns>
                                        <asp:BoundField DataField="OVC_CLASS_NAME" HeaderText="軍種" />
                                        <asp:BoundField DataField="ODT_SHOULD" HeaderText="應付金額" />
                                        <asp:BoundField DataField="ODT_YES" HeaderText="已付金額" />
                                        <asp:BoundField DataField="ODT_NO" HeaderText="未付金額" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <footer class="panel-footer" style="text-align: center;">
                        <!--網頁尾-->
                    </footer>
                </section>
            </asp:Panel>
            <asp:Panel ID="panD" runat="server">
                <section class="panel">
                    <header class="title">
                        運費明細查詢
                    </header>
                    <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                    <!--預留空間，未來做錯誤訊息顯示。-->
                    <div class="panel-body" style="border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <table class="table table-bordered">
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle;">
                                            <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label CssClass="control-label position-left" runat="server">起始日期&nbsp;&nbsp;</asp:Label>

                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtOdtInvDate1" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                            <br />
                                            <asp:Label CssClass="control-label position-left" runat="server">結束日期&nbsp;&nbsp;</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtOdtInvDate2" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                        <td style="text-align: center; vertical-align: middle;">
                                            <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="drpOvcMilitaryType" CssClass="tb tb-m" runat="server">
                                                <asp:ListItem>內容1</asp:ListItem>
                                                <asp:ListItem>內容1</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle;">
                                            <asp:Label CssClass="control-label" runat="server">航商</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="drpOvcShipCompany" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem Text="不限定" Value="不限定" ></asp:ListItem>
                                            <asp:ListItem Text="中華航空" Value="中華航空"></asp:ListItem>
                                            <asp:ListItem Text="長榮航空" Value="長榮航空"></asp:ListItem>
                                            <asp:ListItem Text="長榮海運" Value="長榮海運"></asp:ListItem>
                                            <asp:ListItem Text="陽明海運" Value="陽明海運"></asp:ListItem>
                                            <asp:ListItem Text="非合約航商" Value="非合約航商"></asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center; vertical-align: middle;">
                                            <asp:Label CssClass="control-label" runat="server">區分</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="drpOvcType" CssClass="tb tb-s" runat="server">
                                                <asp:ListItem>不限定</asp:ListItem>
                                                <asp:ListItem>未申請</asp:ListItem>
                                                <asp:ListItem>已付款</asp:ListItem>
                                                <asp:ListItem>未付款</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnQuery1" CssClass="btn-success btnw2" runat="server" OnClick="btnQuery1_Click" Text="查詢" />&nbsp;&nbsp;
                                <asp:Button ID="btnToS" CssClass="btn-success btnw" OnClick="btnToS_Click" runat="server" Text="切換至運費統計查詢" /><br />
                                    <br />
                                    <asp:Button ID="btnPrint" CssClass="btn-success btnw2" runat="server" OnClick="btnPrint_Click" Text="列印" /><br />
                                    <br />
                                </div>
                                <asp:GridView ID="GV_TBGMT_DETAIL_CINF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_DETAIL_CINF_PreRender" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
										<ItemTemplate>
											<a href="javascript:var win=window.open('../A/BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');">
                                                <%# Eval("OVC_BLD_NO")%></a>
										</ItemTemplate>
									</asp:TemplateField>
                                        <asp:BoundField DataField="OVC_INF_NO" HeaderText="結報申請單編號" />
                                        <asp:BoundField DataField="OVC_ICS_NO" HeaderText="運費編號" />
                                        <asp:BoundField DataField="OVC_INLAND_CARRIAGE" HeaderText="運費金額" />
                                        <asp:BoundField DataField="OVC_SHIP_COMPANY" HeaderText="航商" />
                                        <asp:BoundField DataField="OVC_CLASS_NAME" HeaderText="軍種" />
                                        <asp:BoundField DataField="OVC_IS_PAID" HeaderText="區分" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <footer class="panel-footer" style="text-align: center;">
                        <!--網頁尾-->
                    </footer>
                </section>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
