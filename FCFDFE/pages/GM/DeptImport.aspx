<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeptImport.aspx.cs" Inherits="FCFDFE.pages.GM.DeptImport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .title{
            background-color:turquoise;
            color:white;
            font-size:40px;
            font-weight:bold ;
        }
        .th_title{
            font-size:40px;
            font-weight:bold ;
        }
        .txt{
            font-size:22px;
            color:black;
            margin:20px auto;
        }
        .blue-s{
            color:cornflowerblue;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    全域組織單位－資料載入
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div style="margin:20px auto">
                                <asp:Label CssClass="text-red th_title" runat="server">組織單位載入步驟如下</asp:Label>
                            </div>
                            <div class="txt">
                                <asp:Label runat="server">（一）注意：務必優先讀取 <a href="<%=ResolveClientUrl("~/WordPDFprint/單位資料檔載入作業說明.docx")%>">【組織單位資料】載入作業說明</a>，以便逐作業。</asp:Label><br />
                                <asp:Label runat="server">（二）下載 <a href="<%=ResolveClientUrl("~/WordPDFprint/單位資料檔範例說明.xlsx")%>">組織單位資料檔 Ecel 檔範例說明</a>。</asp:Label><br />
                                <asp:Label runat="server">（三）按照載入作業說明規定格式編輯 Excel 檔。</asp:Label><br />
                                <asp:Label runat="server">（四）按【瀏覽】選擇要載入的 Excel 檔。</asp:Label><br />
                                <asp:Label runat="server">（五）按【<input type="button" value="查詢單位代碼" class="btn-success" onclick="OpenWindow()" />】後檢查載入的資料是否正確。</asp:Label><br />
                                <asp:Label runat="server">（六）查詢<span class="text-red">採購單位類別</span>後檢查載入的資料是否正確。</asp:Label><br />
                                <asp:Label runat="server">（七）查詢<span class="text-red">接轉單位類別</span>後檢查載入的資料是否正確。</asp:Label><br />
                                <asp:Label runat="server">（八）按【讀取 Excel檔內容】後檢查顯示的資料內容是否正確。</asp:Label><br />
                                <asp:Label runat="server">（九）如果資料正確無誤請按【確認轉入全域組織單位維護作業】完成載入作業。</asp:Label><br />
                                <asp:Label runat="server">（十）或直接點選<asp:LinkButton OnClick="btnBack_Click" runat="server">【全域組織單位維護作業功能】</asp:LinkButton>繼續全域組織單位維護作業功能。</asp:Label>
                            </div>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success" OnClick="btnLoadunit_Click" runat="server" Text="讀取組織單位資料檔內容" />
                                <asp:FileUpload ID="FileUploadxls" CssClass="btn-success" title="瀏覽" runat="server" />
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
