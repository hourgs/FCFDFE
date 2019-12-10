<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FpersonalPurchaseST.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FpersonalPurchaseST" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .lbl-title {
            display: block;
            font-size: 32px;
        }
        .lbl-subtitle {
            text-align: center;
            font-size: 20px;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:Label CssClass="lbl-title" runat="server">聯審小組查詢作業</asp:Label>
                    <asp:Button CssClass="btn-default btnw4" OnClick="btnReset_Click" Text="回上一頁" runat="server" />
                    <div class="lbl-subtitle" style="margin-top: 10px;">
                        <asp:Label runat="server">計劃年度(第二組)： </asp:Label>
                        <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_OVC_BUDGET_Click" Text="查詢" runat="server" />
                    </div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GV_CASE" ShowFooter="true" CssClass=" table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_CASE_PreRender" OnRowDataBound="GV_CASE_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="序號" DataField="No" />
                                    <asp:TemplateField HeaderText="承辦人姓名" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblChecker" CssClass="control-label" Text='<%# "" + Eval("Checker") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="審查中案數" DataField="" />--%>
                                    <%--<asp:BoundField HeaderText="已核定案數" DataField="" />--%>
                                    <%--<asp:BoundField HeaderText="已撤案案數" DataField="" />--%>
                                    <asp:TemplateField HeaderText="承辦案數" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblAllCount" CssClass="control-label" Text='<%# "" + Eval("AllCount") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                	            </Columns>
	   		               </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
