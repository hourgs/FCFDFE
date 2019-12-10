<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_H10.aspx.cs" Inherits="FCFDFE.pages.CIMS.H.CIMS_H10" %>
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
                    <asp:Label CssClass="control-label" runat="server">系統相關環境參數設定</asp:Label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->                    
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width:50%" colspan="2" class="text-center"><asp:Label CssClass="subtitle" runat="server">底價表上傳檢核</asp:Label></td>
                                    <td style="width:50%" colspan="2" class="text-center"><asp:Label CssClass="subtitle" runat="server">工程會決標資料管理</asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width:18%"><asp:Button ID="B13" runat="server" Text="底價表上傳查詢" OnClick="B13_Click" /></td>
                                    <td class="text-left" style="width:32%">查詢年度內已決標購案底價表上傳情形</td>
                                    <td style="width:18%"><asp:Button ID="C11" runat="server" Text="工程會決標資料上傳" OnClick="C11_Click" /></td>
                                    <td class="text-left" style="width:32%">工程會決標資料上傳</td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="text-center"><asp:Label CssClass="subtitle" runat="server">廠商資料管理</asp:Label></td>
                                    <td colspan="2" class="text-center"><asp:Label CssClass="subtitle" runat="server">物價指數維護</asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width:18%" rowspan="2"><asp:Button ID="F11" runat="server" Text="廠商資料管理" OnClick="F11_Click" /></td>
                                    <td class="text-left" style="width:32%" rowspan="2">廠商資料的新增、轉換及異動</td>
                                    <td style="width:18%" ><asp:Button ID="D11" runat="server" Text="物價指數片語上傳" OnClick="D11_Click" /></td>
                                    <td class="text-left" style="width:32%"></td>
                                </tr>
                                <tr>
                                    <td style="width:18%" ><asp:Button ID="D12" runat="server" Text="物價指數資料上傳" OnClick="D12_Click" /></td>
                                    <td class="text-left" style="width:32%""></td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="text-center"><asp:Label CssClass="subtitle" runat="server">稅率稅則管理</asp:Label></td>

                                </tr>
                                <tr>
                                    <td style="width:18%"><asp:Button ID="H11" runat="server" Text="稅率稅則資料上傳" OnClick="H11_Click" /></td>
                                    <td class="text-left" style="width:32%">1.中文貨名上傳。2.英文貨名上傳。3.稅則資料(1、3欄)。4.稅則資料(2欄) </td>
                                </tr>
                            </table>
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