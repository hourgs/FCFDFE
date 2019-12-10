<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_H11_2.aspx.cs" Inherits="FCFDFE.pages.CIMS.H.CIMS_H12_2" %>

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
                                                <th rowspan="4">上傳檔案注意事項</th>
                                            </tr>
                                            <tr>
                                                <td>1.上傳檔案為<span class="text-red">文字檔</span>(*.txt)，每筆資料長度
                                                    <asp:Label ID="lbl_long" runat="server" Text="Label"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>2.文字檔內的長度順序如 <a href="tax13.htm">格式說明</a></td>
                                            </tr>
                                            <tr>
                                                <td>3.資料<span class="text-red">不要含表頭</span></td>
                                            </tr>
                                        </table>
                                        <hr />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">請選擇上傳檔案</div>
                                    <div class="col-md-9">
                                        <asp:FileUpload ID="btnUPLOAD_IN" title="附件及檔案上傳" runat="server" /></div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <div class="col-md-3">
                                        <asp:Button ID="btnClear" CssClass="btn-success btnw" runat="server" Text="清空資料庫(Option)" /></div>
                                    <div class="col-md-6 text-center">
                                        <asp:Button ID="btnAny" CssClass="btn-success btnw" runat="server" Text="解析檔案內容" />--><asp:Button ID="btnSave" CssClass="btn-success btnw" runat="server" Text="存入資料庫" /></div>
                                    <div class="col-md-3">
                                        <asp:Button ID="btnReturn" CssClass="btn-success btnw" runat="server" Text="取消/返回" /></div>
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
