<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_H12_1.aspx.cs" Inherits="FCFDFE.pages.CIMS.H.CIMS_H11_1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>機動稅率查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="container">
                                <div style="margin-top: 30px;" class="row">
                                    <table class="table table-bordered control-label text-left">
                                        <tr>
                                            <td style="width: 25%" class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">稅則號別：</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTax" CssClass="tb tb-m" AutoPostBack="true" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">進出口日期：</asp:Label>
                                            </td>
                                            <td>
                                                <!--↓日期套件↓-->
                                                <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                    <asp:TextBox ID="txtIO_date" CssClass="tb tb-m position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <!--↑日期套件↑-->
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="margin-top: 5%" class="text-center">
                                        <asp:Button ID="btnQuery" CssClass="btn-default btnw4" runat="server" Text="查詢" />
                                        <asp:Button ID="btnClear" CssClass="btn-default btnw4" runat="server" Text="清除條件" />
                                        <asp:Button ID="btnReturn" CssClass="btn-default btnw4" runat="server" Text="取消/返回"/>
                                    </div>
                                    <div>
                                        <asp:Label CssClass="control-label text-center" runat="server">稅則稅率查詢結果</asp:Label>
                                        <asp:GridView runat="server">
                                        </asp:GridView>
                                    </div>
                                    <div class="row">
                                            <asp:Label CssClass="control-label text-blue" runat="server">　【查詢使用說明】</asp:Label>
                                        <div>
                                            <div class="col-md-8">
                                                <ol>
                                                    <li><asp:Label CssClass="control-label text-blue" runat="server">請輸入<span class="text-red">稅則號別</span>十一碼代碼(例如0309923201)。</asp:Label></li>
                                                    <li><asp:Label CssClass="control-label text-blue" runat="server">請輸入<span class="text-red">進出口日期</span>。</asp:Label></li>
                                                    <li><asp:Label CssClass="control-label text-blue" runat="server">請點選<span class="text-red">『查詢』</span>鍵，送出資料並等待回應。</asp:Label></li>
                                                </ol>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
