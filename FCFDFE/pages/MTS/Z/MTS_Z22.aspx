<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_Z22.aspx.cs" Inherits="FCFDFE.pages.MTS.Z.MTS_Z22" %>
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
                    群組使用功能維護
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">              
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">群組</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:DropDownList  ID="drpGroupName" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label text-red" ID="lblMessage" runat="server" Text="messageLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">功能</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:ListBox ID="lstOvcFuncNameAdd" CssClass="tb tb-full"  runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Button ID="btnAdd" cssclass="btn-success btnw4" runat="server" Text="←加入" /><br /><br />
                                        <asp:Button ID="btnDel" cssclass="btn-success btnw4" runat="server" Text="→移除" />
                                    </td>
                                    <td style="width:300px;">
                                        <asp:ListBox ID="lstOvcFuncNameDel" CssClass="tb tb-full"  runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success" runat="server" Text="送出"/><br /><br /> 
                                <asp:Label CssClass="control-label text-blue" runat="server">群組使用功能列表</asp:Label><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_FUNCS" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_FUNCS_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="群組名稱" />
                                    <asp:BoundField DataField="" HeaderText="功能名稱" />
                                    <asp:BoundField DataField="" HeaderText="功能網址" />
                                </Columns>
                            </asp:GridView>
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
