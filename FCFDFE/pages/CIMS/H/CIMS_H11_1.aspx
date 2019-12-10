<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_H11_1.aspx.cs" Inherits="FCFDFE.pages.CIMS.H.CIMS_H12_1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        ul li {
            margin-top: 5px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>稅率稅則查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="container">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <table class="table table-bordered control-label">
                                            <tr>
                                                <th rowspan="5">
                                                    檔案注意事項</th>
                                            </tr>
                                            <tr>
                                                <td>1.
                                                    檔案為 Excel檔 (*.xls)</td>
                                            </tr>
                                            <tr>
                                                <td>2.Excel 檔內的順序要與下列相符<br /><img src="../../../images/CIMS/EXCEL.png"/></td>
                                            </tr>
                                            <tr>
                                                <td>3. 資料要含表頭<br /><img src="../../../images/CIMS/excel_2.png"/></td>
                                            </tr>
                                            <tr>
                                                <td>4.檔案中的<span class="text-red">代碼及名稱</span>的欄位格式均設為<span class="text-red">文字型態</span></td>
                                            </tr>
                                        </table>
                                        <hr />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">請選擇上傳檔案</div>
                                    <div class="col-md-9"><asp:FileUpload ID="btnUPLOAD_IN" title="附件及檔案上傳" runat="server" /></div>
                                </div>
                                <div class="row" style="margin-top:10px;">
                                    <div class="col-md-3"><asp:Button ID="btnClear" CssClass="btn-success btnw" runat="server" Text="清空資料庫(Option)" /></div>
                                    <div class="col-md-6 text-center"><asp:Button ID="btnAny" CssClass="btn-success btnw" runat="server" Text="解析檔案內容" />--><asp:Button ID="btnSave" CssClass="btn-success btnw" runat="server" Text="存入資料庫" /></div>
                                    <div class="col-md-3"><asp:Button ID="btnReturn" CssClass="btn-success btnw" runat="server" Text="取消/返回" /></div>
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
