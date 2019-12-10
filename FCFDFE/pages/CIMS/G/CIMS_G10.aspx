<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_G10.aspx.cs" Inherits="FCFDFE.pages.CIMS.G.CIMS_G10" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 800px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>報表查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Label CssClass="subtitle" runat="server">分案查尋</asp:Label>
                            <table class="table table-bordered text-left">
                                <tr >
                                    <td style="width:20%;" class="text-center">
                                        <asp:Button ID="G11" runat="server" Text="統計報表" CssClass="btn-info btnw4" OnClick="G11_Click"/></td>
                                    <td >分案查詢及列表統計</td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label CssClass="subtitle" runat="server">案數統計(週)</asp:Label>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width:20%;" class="text-center">
                                        <asp:Button ID="G12" runat="server" Text="統計報表" CssClass="btn-info btnw4" OnClick="G12_Click"/></td>
                                    <td>週報、中心會報案數統計<br/>
                                        註:<br/>
                                        購辦排標統計表<br/>
                                        *開標預劃期程表<br/>
                                        *決標購案統計表</td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label CssClass="subtitle" runat="server">執行現況(年度)</asp:Label>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width:20%;" class="text-center">
                                        <asp:Button ID="G13" runat="server" Text="統計報表" CssClass="btn-info btnw4" OnClick="G13_Click"/></td>
                                    <td>年度收辦、決（廢）標統計查詢<br/>
                                        註:<br/>
                                        XX年度國防部採購室收辦購案執行現況表<br/>
                                        XX年度購辦部核購案無法於年度內完成決標簽約個案統計表<br/>
                                        *XX年度決標購案統計表</td>
                                </tr>
                            </table>
                            <br />
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
