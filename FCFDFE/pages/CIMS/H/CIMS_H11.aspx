<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_H11.aspx.cs" Inherits="FCFDFE.pages.CIMS.H.CIMS_H11" %>

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
                    <div>稅率稅則上傳</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="container">
                                <div style="margin-top: 30px;" class="row">
                                    <table class="table table-bordered control-label text-red">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_CHI_NAME" CssClass="btn-default btnw6" runat="server" Text="中文貨名上傳" OnClick="btn_CHI_NAME_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_ENG_NAME" CssClass="btn-default btnw6" runat="server" Text="英文貨名上傳" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_Tax_inf1" CssClass="btn-default btnw" runat="server" Text="稅則資料(1、3欄)" OnClick="btn_Tax_inf1_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_Tax_inf2" CssClass="btn-default btnw6" runat="server" Text="稅則資料(2欄)" OnClick="btn_Tax_inf2_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <%--<div style="margin-top: 5%" class="text-center">
                                        <asp:Button ID="btnReturn" CssClass="btn-default btnw4" runat="server" Text="取消/返回" />
                                    </div>--%>
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
