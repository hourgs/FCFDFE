<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_D13.aspx.cs" Inherits="FCFDFE.pages.CIMS.D.CIMS_D13" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 800px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>物價指數查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">輸入關鍵字元，例如[石油]，即可查詢石油相關之物價指數</div>
                            <table class="table no-border text-left">
                                <tr>
                                    <td class="text-right" style="width: 15%;">
                                        <asp:Label CssClass="control-label" runat="server">輸入關鍵字</asp:Label>
                                    </td>
                                    <td style="width: 75%;">
                                        <asp:TextBox ID="TextBox1" runat="server"  CssClass="tb tb-1" ></asp:TextBox>
                                    </td>
                                    <td style="width: 10%;">
                                        <asp:Button ID="button1" cssclass="btn-success btnw4" runat="server" Text="查詢" OnClick="button1_Click"/>
                                    </td>
                                </tr>
                            </table>
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