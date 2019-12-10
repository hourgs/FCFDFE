<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B25.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B25" %>

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
                    國防部國防採購室出口軍品購案投保明細表
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">投保日期</asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoODT_INS_DATE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1" Selected="True">不限定日期</asp:ListItem>
                                            <asp:ListItem Value="2" Text=""></asp:ListItem>
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
                            </table>
                            <div class="text-center">
                                <asp:Button OnClick="btnQuery_Click" CssClass="btn-success btnw2" Text="查詢" runat="server" />
                                <asp:Button ID="btnPrint" OnClick="btnPrint_Click" CssClass="btn-success" Visible="false" Text="列印保險明細表" runat="server" />
                                <br /><br />
                                    <asp:GridView ID="GV_TBGMT_EINN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowDataBound="GV_TBGMT_EINN_RowDataBound" OnPreRender="GV_TBGMT_EINN_PreRender" OnRowCreated="GV_TBGMT_EINN_RowCreated" runat="server">
                                        <Columns>
                                            <asp:BoundField DataField="OVC_EINN_NO" HeaderText="投保通知書編號" />
                                            <asp:BoundField DataField="ODT_APPLY_DATE" HeaderText="結報日期" />
                                            <asp:BoundField DataField="OVC_CLASS_NAME" HeaderText="軍種" />
                                            <asp:BoundField DataField="OVC_SHIP_COMPANY" HeaderText="承保公司" />
                                            <asp:BoundField DataField="ONB_INS_AMOUNT" HeaderText="保費金額(新台幣)" />
                                            <asp:BoundField DataField="ODT_INS_DATE" HeaderText="支付日期" />
                                        </Columns>
                                    </asp:GridView>
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
