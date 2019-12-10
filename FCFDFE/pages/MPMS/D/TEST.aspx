<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TEST.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.TEST" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            
                           

                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <table >
                                <tr><td colspan="2"><asp:CheckBoxList ID="CheckBoxList1" runat="server">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                    </asp:CheckBoxList></td>
                               <td ><asp:CheckBoxList ID="CheckBoxList2" runat="server" RepeatDirection="Horizontal">
                                   <asp:ListItem>1</asp:ListItem>
                                   <asp:ListItem>2</asp:ListItem>
                                   <asp:ListItem>3</asp:ListItem>
                                   <asp:ListItem>4</asp:ListItem>
                                   <asp:ListItem>5</asp:ListItem>
                    </asp:CheckBoxList></td></tr> 
                   </table>
                    
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
