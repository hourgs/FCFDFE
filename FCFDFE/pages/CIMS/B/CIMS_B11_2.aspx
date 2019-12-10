<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_B11_2.aspx.cs" Inherits="FCFDFE.pages.CIMS.B.CIMS_B11_2" %>
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
                    <div>底價表上傳功能</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-left" >
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">採購案號</asp:Label></td>
                                    <td><asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">[lblOVC_PURCH]</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label></td>
                                    <td><asp:Label ID="lblOVC_ATTACH_NAME" CssClass="control-label" runat="server">[lblOVC_ATTACH_NAME]</asp:Label></td>
                                </tr>
                            </table>
                            <br />
                            <table class="table table-bordered text-left" >
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">組<br>(若無分組則填0)</asp:Label></td>
                                    <td><asp:TextBox ID="txtOVC_ITEM" CssClass="tb tb-s" runat="server">    
                                        </asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server">次</asp:Label></td>
                                    <td><asp:TextBox ID="txtOVC_TIMES" CssClass="tb tb-s" runat="server">    
                                        </asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server">附件序號<br>(自編流水號)</asp:Label></td>
                                    <td><asp:TextBox ID="txtOVC_SUB" CssClass="tb tb-s" runat="server">    
                                        </asp:TextBox></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <asp:Button ID="btnNew" CssClass="btn-warning btnw4" runat="server" Text="上傳檔案" />
                    &emsp;
                    <asp:Button ID="btnReset" CssClass="btn-default btnw2" runat="server" Text="取消" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>

