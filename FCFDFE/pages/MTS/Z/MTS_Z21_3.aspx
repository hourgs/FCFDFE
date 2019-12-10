<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_Z21_3.aspx.cs" Inherits="FCFDFE.pages.MTS.Z.MTS_Z21_3" %>
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
                    帳號管理-查詢<br /><br />
                    <asp:Button ID="btnModify" cssclass="btn-success btnw4" runat="server" Text="修改頁" />
                    <asp:Button ID="btnAdd" cssclass="btn-warning btnw4" runat="server" Text="新增頁" />                    
                    <asp:Button ID="Button1" cssclass="btn-success btnw4" runat="server" Text="查詢頁" />
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">              
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">使用狀態</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:DropDownList  ID="drpAccountStatus" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">帳號</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:TextBox ID="txtAcId" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">姓名關鍵字</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:TextBox ID="txtUserName" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">單位</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:TextBox ID="txtDeptSn" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button ID="btnDeptSn" cssclass="btn-success btnw2" runat="server" Text="單位" />&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" cssclass="btn-default btnw4" runat="server" Text="資料清空" />
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success" runat="server" Text="送出"/><br /><br />
                            </div>
                            <asp:GridView ID="GV_ACCOUNT" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_ACCOUNT_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="單位" />
                                    <asp:BoundField DataField="" HeaderText="姓名" />
                                    <asp:BoundField DataField="" HeaderText="帳號" />
                                    <asp:BoundField DataField="" HeaderText="電話" />
                                    <asp:BoundField DataField="" HeaderText="狀態" />
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
