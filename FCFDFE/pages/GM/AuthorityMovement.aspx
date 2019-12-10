<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuthorityMovement.aspx.cs" Inherits="FCFDFE.pages.GM.AuthorityMovement" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script>
        function clearEND_DATE() {
            $("#<%=txtEND_DATE.ClientID%>").val("");
        }
   </script>
    <%--<script>
        //// 透過PageRequestManager的物件，我們可以在add_pageLoaded的事件後重新註冊jQuery的方法內容
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(datepicker);
    </script>--%>
    <div class="row">
        <div style="width: 800px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    系統使用者權限移轉與複製
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered" style="width:700px;">
                                <tr>
                                    <td>
                                        <asp:Label cssclass="control-label text-blue" runat="server">單位代碼：</asp:Label>
                                        <asp:TextBox id="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <input type="button" value="單位查詢" class="btn-success" onclick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" />
                                        <asp:TextBox id="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center" style="margin-bottom: 50px;">
                                <asp:Button cssclass="btn-success" OnClick="btnQuery_Click" runat="server" Text="查詢"/>
                            </div>
                            <asp:Panel ID="PnMessage_Shift" runat="server"></asp:Panel>
                            <table class="table table-bordered"> 
                                <tr class="no-bordered">
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label" runat="server">結束日期</asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtEND_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                        <%--<asp:Button cssclass="btn-success" OnClick="btnClear_Click" runat="server" Text="清空"/>--%>
                                        <input class="btn-success" type="button" onclick="clearEND_DATE()" value="清空" />
                                        <asp:Label CssClass="control-label text-red textSpace-l" runat="server">若未填寫，則表示此轉移為永久有效。</asp:Label>
                                    </td>
                                </tr> 
                                <tr class="no-bordered no-bordered-seesaw text-center">
                                    <td class="text-right" style="width: 170px;">
                                        <asp:Label CssClass="control-label" runat="server">原有權限人員</asp:Label>
                                    </td>
                                    <td style="width: 250px;">
                                       <asp:TextBox ID="txtACCOUNT_ID_ORI" CssClass="tb tb-m" Visible="false" runat="server"></asp:TextBox>
                                       <asp:TextBox ID="txtACCOUNT_Name_ORI" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 50px;">
                                        <asp:Label CssClass="control-label" runat="server">◀▶</asp:Label>
                                    </td>
                                    <td class="text-right" style="width: 150px;">
                                        <asp:Label CssClass="control-label" runat="server">新設權限人員</asp:Label>
                                    </td>
                                    <td style="width: 250px;">
                                       <asp:TextBox ID="txtACCOUNT_ID" CssClass="tb tb-m" Visible="false" runat="server"></asp:TextBox>
                                       <asp:TextBox ID="txtACCOUNT_Name" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="no-bordered no-bordered-seesaw text-center">
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label" runat="server">選取人員</asp:Label>
                                    </td>
                                    <td>
                                       <asp:ListBox ID="lstACCOUNT_ID_ORI" CssClass="tb tb-m" OnSelectedIndexChanged="lstACCOUNT_ID_ORI_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:ListBox>
                                    </td>
                                    <td>
                                        
                                    </td>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label" runat="server">選取人員</asp:Label>
                                    </td>
                                    <td>
                                       <asp:ListBox ID="lstACCOUNT_ID" CssClass="tb tb-m" OnSelectedIndexChanged="lstACCOUNT_ID_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center" style="margin-bottom: 20px;">
                                <asp:Button cssclass="btn-warning" Text="確定" OnClick="btnSave_Click" runat="server" />
                            </div>
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

