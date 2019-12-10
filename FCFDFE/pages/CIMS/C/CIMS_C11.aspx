<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_C11.aspx.cs" Inherits="FCFDFE.pages.CIMS.C.CIMS_C11" %>
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
                    <div>工程會上傳</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="subtitle">系統說明</div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">本功能是上傳公共工程委員會決標資訊，以供查詢。</asp:Label></td>
                                    
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="subtitle">操作說明</div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td width="25%"><asp:Label CssClass="control-label" runat="server">操作程序</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">1.按下[瀏覽]，選擇欲上傳的Excel檔，上傳檔案為Excel(*.xls)</asp:Label><br><br>
                                        <asp:Label CssClass="control-label" runat="server">2.Excel檔內第一列為標題列，第二列為開始才是資料起始列</asp:Label><br><br>
                                        <table class="table table-bordered text-center">
                                            <tr>
                                                <td><asp:Label CssClass="control-label" runat="server">廠商名稱</asp:Label></td>
                                                <td><asp:Label CssClass="control-label" runat="server">是否為得標廠商</asp:Label></td>
                                                <td><asp:Label CssClass="control-label" runat="server">廠商英文名稱</asp:Label></td>
                                                <td><asp:Label CssClass="control-label" runat="server">......</asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            
                        </div>
                    </div>
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="subtitle">操作說明</div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td width="25%"><asp:Label CssClass="control-label" runat="server">轉檔程式</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">3.按「檔案載入」鍵，進行資料讀取。</asp:Label><br><br>
                                        <asp:LinkButton ID="LinkButton1" CssClass="control-label" runat="server" OnClick="LinkButton1_Click">【2016】FD工程會決標資訊轉檔程式</asp:LinkButton><br><br>
                                    </td>
                                </tr>
                            </table>
                            
                        </div>
                    </div>
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <div class="subtitle">工程會檔案上傳</div>
                            <table class="table no-border text-left">
                                <tr>
                                    <td class="text-right" style="width: 40%;">
                                        <asp:Label CssClass="control-label" runat="server">選擇你要上傳的Excel檔案 :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="ful" title="瀏覽..."  runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnNew" cssclass="btn-warning btnw2" OnClick="btnNew_OnClick" runat="server" Text="上傳" />
                                <asp:Button ID="btnReset" cssclass="btn-default btnw2" runat="server" Text="取消" />
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="subtitle" align="center">Excel範例圖 </div>
                            <asp:Image ID="imgExample" class="img img-full" runat="server" />
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

