<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D24.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D24" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        tr td:nth-child(1) {
            text-align: right;
        }

        tr td:nth-child(2) {
            text-align: left;
        }

        #GAP_print {
            text-decoration: underline;
            color: red;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                    購案適用GAP英文公告稿作業編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <table class="table table-bordered text-center">
                            <tr class="no-bordered">
                                <td style="width: 30%">
                                    <asp:Label CssClass="control-label" runat="server">【Solicitation Number】</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">NC06001L074</asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Entity Code】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_ENTITY_CODE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【The Times for publication】</asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="drpONB_TIMES" CssClass="tb tb-l" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Whether GAP】</asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="drpOVC_WHETHER_GPA" CssClass="tb tb-m" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Procuring Entity】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_PROCURING_ENTITY" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Address of Procuring Entity】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_PROCURING_ADDRESS" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Subject of Procurement】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_PUR_IPURCH_ENG" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Attribute of Procurement】</asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="drpOVC_TARGET_KIND" CssClass="tb tb-l" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【For Information Call】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Tel. No.】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_TELEPHONE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Procuring document sale price and The way of Pay】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_BID_SELL" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Address of obtain Procuring】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_DOC_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Closing Date for Receipt of Tenders】</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOVC_DBID_LIMIT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">【Additional Description】</asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="drpOVC_DESC" CssClass="tb tb-full" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="存檔" />
                            <asp:Button ID="btnReturnP" CssClass="btn-warning btnw6" runat="server" Text="回公告畫面" />
                            <asp:Button ID="btnReturnM" CssClass="btn-warning btnw6" runat="server" Text="回主流程畫面" />
                            <a href="#" id="GAP_print">GAP英文公告稿預覽列印</a>
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
