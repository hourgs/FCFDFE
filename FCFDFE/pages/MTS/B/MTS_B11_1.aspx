<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B11_1.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B11_1" %>
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
                    <div>投保通知書 新增Step1 選擇提單</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                        <asp:regularexpressionvalidator id="Regularexpressionvalidator1" controltovalidate="txtOVC_BLD_NO" errormessage="請輸入英文或數字" validationexpression="^[A-Za-z0-9-]*$" ClientIDMode="AutoID" runat="server"></asp:regularexpressionvalidator>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">承運航商</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">啟運日期</asp:Label>
                                    </td>
                                    <td class="text-left" colspan="3">
                                        <%--<asp:RadioButtonList ID="rdoODT_START_DATE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1" Selected="True">不限定日期</asp:ListItem>
                                            <asp:ListItem Value="2" Text=""></asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_START_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_START_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">需辦投保</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_STATUS" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">海空運別</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SEA_OR_AIR" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server"/>
                                <asp:Button CssClass="btn-success" Text="直接新增投保通知書" OnClick="btnNew_Click" runat="server"/>
                            </div><br />
                            <asp:GridView ID="GVTBGMT_BLD" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" 
                                OnPreRender="GVTBGMT_BLD_PreRender" OnRowCommand="GVTBGMT_BLD_RowCommand" OnRowDataBound="GVTBGMT_BLD_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a id="hrefQuote" href="javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="承運航商" DataField="OVC_SHIP_COMPANY" />
                                    <asp:BoundField HeaderText="海空運別" DataField="OVC_SEA_OR_AIR" />
                                    <asp:BoundField HeaderText="船機名稱" DataField="OVC_SHIP_NAME" />
                                    <asp:BoundField HeaderText="船機航次" DataField="OVC_VOYAGE" />
                                    <asp:BoundField HeaderText="啟運日期" DataField="ODT_START_DATE"/>
                                    <asp:BoundField HeaderText="啟運港埠" DataField="OVC_START_PORT" />
                                    <asp:TemplateField HeaderText="投保通知書" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-warning btnw2" Text="建立" CommandName="dataNew" CommandArgument="" runat="server"/>
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
