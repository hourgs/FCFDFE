<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_D11_2.aspx.cs" Inherits="FCFDFE.pages.MTS.D.MTS_D11_2" %>

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
                            結報申請表-修改
                        </header>
                        <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <!--預留空間，未來做錯誤訊息顯示。-->
                        <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                                <div class="cmxform form-horizontal tasi-form">
                                    <div class="text-right">
                                        <asp:LinkButton OnClick="btnBack_Click" Text="回結報申請表管理" runat="server"></asp:LinkButton>
                                    </div>
                                    <table class="table table-bordered">
                                        <tr>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Label CssClass="control-label " runat="server">結報申請表編號</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblOvcInfNo" CssClass="control-label" runat="server" Text="OVCINFNOLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Label CssClass="control-label" runat="server">付款狀況</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rdoOvcIsPaid" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                    <asp:ListItem>未付款</asp:ListItem>
                                                    <asp:ListItem>已付款</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="text-align: center;">
                                        <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn-warning btnw2" runat="server" Text="修改" /><br />
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
