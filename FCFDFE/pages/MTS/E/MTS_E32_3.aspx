<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E32_3.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E32_3" %>
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
                    接轉作業費結報申請表-刪除
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">編號</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcTofNo" CssClass="control-label " runat="server" Text="mnoLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblMilitaryType" CssClass="control-label " runat="server" Text="mmilitaryLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">進出口</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcIeType" CssClass="control-label " runat="server" Text="ieLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcBudget" CssClass="control-label " runat="server" Text="budgetLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcPurposeType" CssClass="control-label " runat="server" Text="purposeLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">摘要</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcAbstract" CssClass="control-label " runat="server" Text="abstractLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">金額</asp:Label>
                                    </td>
                                    <td style="width:800px;">           
                                        <asp:Label CssClass="control-label" runat="server">新台幣</asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblOnbAmount" CssClass="control-label " runat="server" Text="amountLabel"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcNote" CssClass="control-label " runat="server" Text="noteLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcPlnContent" CssClass="control-label " runat="server" Text="planLabel"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnDel" OnClick="btnDel_Click" cssclass="btn-danger btnw2" runat="server" Text="刪除" />
                                    <asp:Button ID="btnHome" OnClick="btnHome_Click" cssclass="btn-warning btnw" runat="server" Text="回首頁" /> 
                            </div>
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

