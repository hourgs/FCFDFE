<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BulletinBoard.aspx.cs" Inherits="FCFDFE.pages.GM.BulletinBoard" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
         $(document).ready(function () {
             $("<%=strMenuName%>").addClass("active");
             $("<%=strMenuNameItem%>").addClass("active");
         });
    </script>
    <meta http-equiv="x-ua-compatible" content="IE=11" />
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    公佈欄
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:GridView ID="GV_TBGMT_BULLENTIN" DataKeyNames="BB_SN" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="GV_TBGMT_BULLENTIN_RowCommand" OnPreRender="GV_TBGMT_BULLENTIN_PreRender" OnRowDataBound="GV_TBGMT_BULLENTIN_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="標題" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_Print" Text='<%# Bind("TITLE") %>' runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="TITLE" HeaderText="標題" />--%>
                                    <asp:BoundField DataField="AUTHOR_ID" HeaderText="發表者" />
                                    <%--<asp:BoundField DataField="STATUS" HeaderText="狀態" />--%>
                                    <asp:BoundField DataField="START_DATE" HeaderText="發表日期" />
                                    <asp:BoundField DataField="END_DATE" HeaderText="結束日期" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button Text="刪除" CssClass="btn-danger" CommandName="DataDel" OnClientClick="if (confirm('確定刪除此公告?') == false) return false;" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>