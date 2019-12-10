<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E15_1.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E15_1" %>
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
                    運費支付管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:140px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請日期</asp:Label>
                                    </td>
                                    <td style="width:475px;">                         
                                        <asp:Label CssClass="control-label position-left" runat="server">&nbsp;&nbsp;</asp:Label>
                                                <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOdtApplyDate" CssClass="tb tb-s position-left text-change" runat="server" ></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td style="width:100px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td style="width:285px;">
                                        <asp:DropDownList  ID="drpOvcMilitaryType" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:140px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">承運航商</asp:Label>
                                    </td>
                                    <td style="width:475px;">
                                        <asp:DropDownList ID="drpOvcCompany" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>中華航空</asp:ListItem>
                                            <asp:ListItem>長榮航空</asp:ListItem>
                                            <asp:ListItem>長榮海運</asp:ListItem>
                                            <asp:ListItem>陽明海運</asp:ListItem>
                                            <asp:ListItem>非合約航商</asp:ListItem>
                                        </asp:DropDownList>&nbsp;&nbsp;
                                    </td>
                                    <td style="width:100px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">付款狀況</asp:Label>
                                    </td>
                                    <td style="width:285px;">
                                        <asp:DropDownList  ID="drpOvcIsPaid" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>未付款</asp:ListItem>
                                            <asp:ListItem>已付款</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:140px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td style="width:475px;">
                                        <asp:TextBox ID="txtOvcInfNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:100px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">進出口別</asp:Label>
                                    </td>
                                    <td style="width:285px;">
                                        <asp:DropDownList  ID="drpOvcImpOrExp" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>進口</asp:ListItem>
                                            <asp:ListItem>出口</asp:ListItem>
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success" runat="server" Text="查詢" OnClick="btnQuery_Click" /> <br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CINF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CINF_PreRender" OnRowCommand="GV_TBGMT_CINF_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_INF_NO" HeaderText="結報申請表編號" />
                                    <asp:BoundField DataField="OVC_GIST" HeaderText="案由" />
                                    <asp:BoundField DataField="OVC_BUDGET" HeaderText="預算科目及編號" />
                                    <asp:BoundField DataField="OVC_PURPOSE_TYPE" HeaderText="用途別" />
                                    <asp:BoundField DataField="ONB_AMOUNT" HeaderText="金額" />
                                    <asp:BoundField DataField="OVC_BUDGET_INF_NO" HeaderText="預算通知書編號" />
                                    <asp:BoundField DataField="OVC_INV_NO" HeaderText="發票號碼" />
                                    <asp:BoundField DataField="OVC_IS_PAID" HeaderText="已付款與否" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnModify" CssClass="btn-warning" Text="修改" CommandName="btnModify" runat="server"/>
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
