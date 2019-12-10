<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="codequery.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.codequery" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <!--所有使用單位查詢的TextBox，id全部都要一致，如id="txtOvcOnname"，另外檢查，此頁面master page
                                            所包的content其ContentPlaceHolderID是否是MainContent，否的話請改成MainContent-->
                                        <!--單位查詢的button的 OnClientClick其中query.aspx的位置請依照情況不同做修改-->
                                        <asp:TextBox id="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnQuery" onclick="btnQuery_Click" OnClientClick="OpenCode()" cssclass="btn-warning" runat="server" Text="代碼查詢"/>
                                        <asp:TextBox id="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td  style="text-align:center;">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox id="txtOVC_AUDIT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="Button1" ONCLICK="Button1_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                        <asp:TextBox id="txtOVC_AUDIT_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>--%>
                                <%--<tr>
                                    <td  style="text-align:center;">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox id="TextBox3" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Button ID="Button2" onclick="Button2_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                        <asp:TextBox id="TextBox4" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>--%>
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
