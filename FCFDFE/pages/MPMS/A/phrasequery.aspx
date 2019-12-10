<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="phrasequery.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.phrasequery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
         }

         });
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    代碼查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;">
                                        <asp:Label CssClass="control-label" runat="server">片語</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox id="txtFCODE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnQuery"  OnClientClick="OpenPhraseWindow()" cssclass="btn-warning" runat="server" Text="代碼查詢"/>
                                        <asp:TextBox id="txtNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
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
