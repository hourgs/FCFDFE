<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B14_1.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B14_1" %>

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
                    <div>結報申請表-管理</div>
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
                                        <asp:Label CssClass="control-label" runat="server">結報年度</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpODT_APPLY_DATE" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">發文字號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtISSU_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_INF_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">保險費收據號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_INV_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">建檔日期</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:RadioButtonList ID="rdoODT_CREATE_DATE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1" Selected="True">不限定日期</asp:ListItem>
                                            <asp:ListItem Value="2" Text=""></asp:ListItem>
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
                                <asp:Button OnClick="btnQuery_Click" CssClass="btn-success btnw2" Text="查詢" runat="server" /><br />
                                <br />
                            </div>
                            <asp:GridView ID="GVTBGMT_IINF" DataKeyNames="IINF_SN" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" OnPreRender="GVTBGMT_IINF_PreRender" OnRowCommand="GVTBGMT_IINF_RowCommand" OnRowCreated="GVTBGMT_IINF_RowCreated" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="結報申請表編號" DataField="OVC_INF_NO" />
                                    <asp:BoundField HeaderText="案由" DataField="OVC_GIST" />
                                    <asp:BoundField HeaderText="預算科目及編號" DataField="OVC_BUDGET" />
                                    <asp:BoundField HeaderText="用途別" DataField="OVC_PURPOSE_TYPE" />
                                    <asp:BoundField HeaderText="金額" DataField="ONB_AMOUNT" />
                                    <asp:BoundField HeaderText="結報申請日期" DataField="ODT_APPLY_DATE" />
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" Text="修改" CommandName="dataModify" CommandArgument='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-danger btnw2" Text="刪除" CommandName="dataDel" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" CommandArgument='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" Text="列印" CommandName="dataPrint" CommandArgument='<%#drpODT_APPLY_DATE.SelectedItem + "," + txtISSU_NO.Text + "," + txtOVC_INF_NO.Text + "," + txtOVC_INV_NO.Text + "," + drpOVC_MILITARY_TYPE.SelectedItem + "," + txtODT_CREATE_DATE_S.Text + "," + txtODT_CREATE_DATE_E.Text + "," + rdoODT_CREATE_DATE.SelectedValue %>' runat="server" />
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

