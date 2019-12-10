<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OnlineUserNumber.aspx.cs" Inherits="FCFDFE.pages.GM.OnlineUserNumber" %>
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
                    線上使用者列表
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            
                            <asp:GridView ID="GV_ONLINE" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_ONLINE_PreRender" runat="server">
                                <Columns>
                                    <%--<asp:BoundField DataField="logintime" HeaderText="登入時間" />--%>
                                    <asp:BoundField DataField="ip" HeaderText="IP位址" />
                                    <asp:BoundField DataField="user" HeaderText="帳號名稱" />
                                    <asp:BoundField DataField="time" HeaderText="最後存取時間" />
                                   <%-- <asp:BoundField DataField="username" HeaderText="最後存取時間" />--%>
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
