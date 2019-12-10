<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E25_2.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E25_2" %>
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
                    運費結報申請表
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label CssClass="control-label" ID="lblOvcInfNo" runat="server" Text="INFNOLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">已付款與否</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:RadioButtonList ID="rdoOvcIsPaid" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>未付款</asp:ListItem>
                                            <asp:ListItem>已付款</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">付款日期</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtPaidDate" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="修改" /> <br /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <header  class="title">
                    結報申請表
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案由</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:Label CssClass="control-label" ID="lblOvcGist" runat="server" Text="GISTLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目及編號</asp:Label>
                                    </td>
                                    <td style="width:250px;">
                                        <asp:Label CssClass="control-label" ID="lblOvcBudgetInfNo" runat="server" Text="BugGetLabel"></asp:Label>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">海空運別</asp:Label>
                                    </td>
                                    <td style="width:350px;">
                                        <asp:Label CssClass="control-label" ID="lblOvcSeaOrAir" runat="server" Text="SeaOrAirLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td style="width:250px;">
                                        <asp:Label CssClass="control-label" ID="lblOvcPurposeType" runat="server" Text="PurposeTypeLabel"></asp:Label>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請日期</asp:Label>
                                    </td>
                                    <td style="width:350px;">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtApplyDate" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">金額</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:Label CssClass="control-label" runat="server" Text="新台幣"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" ID="lblOnbAmount1" runat="server" Text="AmountLabel"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server" Text="元"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算通知單編號</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:Label CssClass="control-label" ID="lblOvcBudgetInfNo1" runat="server" Text="BudGetINFNOLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收據號碼</asp:Label>
                                    </td>
                                    <td style="width:250px;">
                                        <asp:Label CssClass="control-label" ID="lblOvcInvNo" runat="server" Text="INFNOLabel"></asp:Label>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收據日期</asp:Label>
                                    </td>
                                    <td style="width:350px;">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtInvDate" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:Label CssClass="control-label" ID="lblOvcNote" runat="server" Text="NoteLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:Label CssClass="control-label" ID="lblOvcPlnContent" runat="server" Text="PLNContentLabel"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="GV_TBGMT_CINF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CINF_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="運費編號" />
                                    <asp:BoundField DataField="" HeaderText="提單編號" />
                                    <asp:BoundField DataField="" HeaderText="海空運費" />
                                    <asp:BoundField DataField="" HeaderText="結報申請表編號" />
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

