<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B15.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B15" %>

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
                    <div>國防部國防採購室進口軍品購案投保明細表</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr >
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td style="width:35%" class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td style="width:35%" class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">投保日期</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:RadioButtonList ID="rdoODT_INS_DATE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1" Selected="True">不限定日期</asp:ListItem>
                                            <asp:ListItem Value="2" Text=""> </asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INS_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INS_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">建檔日期</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:RadioButtonList ID="rdoODT_CREATE_DATE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1" Selected="True">不限定日期</asp:ListItem>
                                            <asp:ListItem Value="2" Text=""> </asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CREATE_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CREATE_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button OnClick="btnQuery_Click" CssClass="btn-success btnw2" Text="查詢" runat="server" />
                                <asp:Button ID="btnPrint" OnClick="btnPrint_Click" CssClass="btn-success" Text="列印投保明細表" Visible="false" runat="server" />
                            </div>
                            <asp:GridView ID="GVTBGMT_IINN" CssClass="table data-table table-striped border-top text-center data-table" style="margin-top: 20px;" AutoGenerateColumns="false" OnPreRender="GVTBGMT_IINN_PreRender" OnRowDataBound="GVTBGMT_IINN_RowDataBound" OnRowCreated="GVTBGMT_IINN_RowCreated" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="投保通知書編號" DataField="OVC_IINN_NO" />
                                    <asp:BoundField HeaderText="軍種" DataField="OVC_MILITARY_TYPE" />
                                    <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
                                    <%--<asp:BoundField HeaderText="提單號碼" DataField="OVC_BLD_NO" />--%>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <!--BLD顯示新方法-->
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="保費金額(新台幣)" DataField="ONB_INS_AMOUNT" />
                                    <asp:BoundField HeaderText="承保公司" DataField="ODT_INS_DATE" />
                                    <asp:BoundField HeaderText="交貨條件" DataField="OVC_DELIVERY_CONDITION" />
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

