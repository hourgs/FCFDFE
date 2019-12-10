    <%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F13_1.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F13_1" %>
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
                    <asp:Label ID="lblTITLE" CssClass="control-label" Text="保險費率資料維護" runat="server"></asp:Label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險費率與運費折扣資料查詢</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:DropDownList  ID="drpOvcInsType" CssClass="tb tb-s" OnSelectedIndexChanged="drpOvcInsType_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                           <asp:ListItem Selected="True">保險費率</asp:ListItem>
                                           <asp:ListItem>空運運費</asp:ListItem>
                                           <asp:ListItem>海運運費</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="DateClumn" runat="server" >
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label ID="lblDate" CssClass="control-label" runat="server">保險期間</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">&nbsp;&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtEndDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                         
                                        <asp:CheckBoxList ID="chkOdtApplyDate" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>不限定日期</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success" OnClick="btnQuery_Click" Visible="false" runat="server" Text="查詢" /> 
                                <asp:Button ID="btnQuery2" cssclass="btn-success" OnClick="btnQuery2_Click" Visible="false" runat="server" Text="查詢" />
                                <asp:Button ID="btnQuery3" cssclass="btn-success" OnClick="btnQuery3_Click" Visible="false" runat="server" Text="查詢" /><br /><br />
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" OnClick="btnSave_Click" Visible="false" Text="新增保險費率資料" />
                                <asp:Button ID="btnSave2" cssclass="btn-warning" runat="server" OnClick="btnSave2_Click" Visible="false" Text="新增空運運費資料" />
                                <asp:Button ID="btnSave3" cssclass="btn-warning" OnClick="btnSave3_Click" runat="server" Visible="false" Text="新增保險公司" />
                                <asp:Button ID="btnSave4" cssclass="btn-warning" runat="server" OnClick="btnSave4_Click" Visible="false" Text="新增海運運費資料" />
                            </div>
                            <br />
                            <asp:GridView ID="GV_TBGMT_INSRATE" DataKeyNames="INSRATE_SN" CssClass="table data-table table-striped border-top text-center" Visible="false" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_INSRATE_PreRender" OnRowCommand="GV_TBGMT_INSRATE_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_INSCOMPNAY" HeaderText="保險公司" />
                                    <asp:BoundField DataField="Effective_period" HeaderText="保險保險有效期間(起訖)" />
                                    <asp:BoundField DataField="OVC_INS_RATE_1" HeaderText="主險費率" />
                                    <asp:BoundField DataField="OVC_INS_RATE_2" HeaderText="在台內陸險費率" />
                                    <asp:BoundField DataField="OVC_INS_RATE_3" HeaderText="在外內陸險費率" />
                                    <asp:BoundField DataField="OVC_INS_RATE_4" HeaderText="兵險及罷工險費率" />
                                    <asp:BoundField DataField="EX_WORK" HeaderText="Ex Work費率" />
                                    <asp:BoundField DataField="FOB" HeaderText="FOB, FCR, FCA, CPT等費率" />
                                    <asp:BoundField DataField="OVC_INS_RATE" HeaderText="其他(組合)費率" />
                                    <%--<asp:BoundField DataField="OVC_INS_NAME" HeaderText="保險費種類" />
                                    <asp:BoundField DataField="OVC_INS_RATE" HeaderText="保險費率百分比" />
                                    <asp:BoundField DataField="ODT_START_DATE" HeaderText="保險開始日"  DataFormatString = "{0:yyyy/MM/dd}" />
                                    <asp:BoundField DataField="ODT_END_DATE" HeaderText="保險結束日"  DataFormatString = "{0:yyyy/MM/dd}" />
                                    <asp:BoundField DataField="ONB_SORT" HeaderText="排序" />--%>
                                    <asp:BoundField DataField="ODT_CREATE_DATE" HeaderText="資料建立日期" DataFormatString = "{0:yyyy/MM/dd}" />
                                    <asp:BoundField DataField="OVC_CREATE_ID" HeaderText="資料建立人員" />
                                    <asp:BoundField DataField="ODT_MODIFY_DATE" HeaderText="資料修改日期" DataFormatString = "{0:yyyy/MM/dd}" />
                                    <asp:BoundField DataField="OVC_MODIFY_LOGIN_ID" HeaderText="資料修改人員" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnManagement" CssClass="btn-success" Text="管理" CommandName="btnManagement" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="GV_TBGMT_AIR_TRANSPORT" DataKeyNames="AT_SN" CssClass="table data-table table-striped border-top text-center" Visible="false" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_AIR_TRANSPORT_PreRender" OnRowCommand="GV_TBGMT_AIR_TRANSPORT_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_COMPANY" HeaderText="承運航商" />
                                    <asp:BoundField DataField="OVC_START_PORT" HeaderText="啟運機場" />
                                    <asp:BoundField DataField="OVC_CURRENCY_NAME" HeaderText="幣別" />
                                    <asp:BoundField DataField="ONB_DISCOUNT_1" HeaderText="折扣數" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnManagement" CssClass="btn-success" Text="管理" CommandName="btnManagement" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="GV_TBGMT_SEA_TRANSPORT" DataKeyNames="ST_SN" CssClass="table data-table table-striped border-top text-center" Visible="false" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_SEA_TRANSPORT_PreRender" OnRowCommand="GV_TBGMT_SEA_TRANSPORT_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_COMPANY" HeaderText="承運航商" />
                                    <asp:BoundField DataField="OVC_DATE" HeaderText="合約期間(起迄)" />
                                    <asp:BoundField DataField="OVC_START_PORT" HeaderText="啟運港埠" />
                                    <asp:BoundField DataField="OVC_IMPORT_EXPORT_1" HeaderText="進口/出口" />
                                    <asp:BoundField DataField="ONB_DISCOUNT_1" HeaderText="折扣數" />
                                    <asp:BoundField DataField="OVC_ITEM_CATEGORY_1" HeaderText="品項類別" />
                                    <asp:BoundField DataField="OVC_ITEM_CHI_NAME_1" HeaderText="品名(中文)" />
                                    <asp:BoundField DataField="OVC_CURRENCY_NAME" HeaderText="幣別" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnManagement" CssClass="btn-success" Text="管理" CommandName="btnManagement" runat="server"/>
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

