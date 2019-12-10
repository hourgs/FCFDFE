<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F18_1.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F18_1" %>

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
                    清運方式資料-新增
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width: 200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">清運方式</asp:Label>
                                    </td>
                                    <td style="width: 800px;" colspan="3">
                                        <asp:TextBox ID="txtOVC_WAY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">排序</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:TextBox ID="txtONB_SORT" CssClass="tb tb-s " TextMode="Number" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnNew" CssClass="btn-success" runat="server" Text="新增" OnClick="btnNew_Click" />
                                <br />
                                <br />
                                <asp:Button ID="btnBack" CssClass="btn-success" runat="server" Text="回首頁" OnClick="btnBack_Click" />
                                <br />
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
