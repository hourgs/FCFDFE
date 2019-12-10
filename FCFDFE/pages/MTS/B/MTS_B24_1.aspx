<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B24_1.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B24_1" %>
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
                <header class="title">
                    結報申請表-管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpODT_APPLY_DATE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_INF_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">建檔日期</asp:Label>
                                    </td>
                                    <td colspan="3">
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
                                <asp:Button cssclass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" /><br /><br />
                            </div>
                            <!--分頁必加AllowPaging="true" ShowFooter="true" OnPageIndexChanging="GV_TBGMT_EINF_PageIndexChanging"-->
                            <%--AllowPaging="true" ShowFooter="true" OnPageIndexChanging="GV_TBGMT_EINF_PageIndexChanging"--%>
                            <asp:GridView ID="GV_TBGMT_EINF" DataKeyNames="EINF_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" 
                                OnPreRender="GV_TBGMT_EINF_PreRender" OnRowCommand="GV_TBGMT_EINF_RowCommand" OnRowDataBound="GV_TBGMT_EINF_RowDataBound"
                                 runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="結報申請表編號" DataField="OVC_INF_NO" />
                                    <asp:BoundField HeaderText="案由" DataField="OVC_GIST" /> 
                                    <asp:BoundField HeaderText="預算科目及編號" DataField="OVC_BUDGET" />
                                    <asp:BoundField HeaderText="用途別" DataField="OVC_PURPOSE_TYPE" />
                                    <asp:BoundField HeaderText="金額" DataField="ONB_AMOUNT" />
                                    <asp:BoundField HeaderText="預算通知單編號" DataField="OVC_BUDGET_INF_NO" />
                                    <asp:BoundField HeaderText="結報申請日期" DataField="ODT_APPLY_DATE" />
                                    <asp:BoundField HeaderText="備考" DataField="OVC_NOTE" />  
                                    <asp:BoundField HeaderText="擬辦" DataField="OVC_PLN_CONTENT" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-warning" Text="修改" CommandName="dataModify" CommandArgument='<%# Container.DataItemIndex%>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                     <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-danger" Text="刪除" OnClientClick="return confirm('確定刪除嗎?')" CommandName="dataDel" CommandArgument='<%# Container.DataItemIndex%>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                     <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success" Text="列印" CommandName="dataPrint" CommandArgument='<%#"EINF" + drpODT_APPLY_DATE.SelectedItem.Text + "," + txtOVC_INF_NO.Text + "," + drpOVC_MILITARY_TYPE.SelectedItem.Text + "," + txtODT_CREATE_DATE_S.Text + "," + txtODT_CREATE_DATE_E.Text%>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                </Columns>
                            </asp:GridView>
                            <%--<div class="page">
                                <asp:Literal ID="ltHtml" runat="server"></asp:Literal>
                            </div>--%>
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
