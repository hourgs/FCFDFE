<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_D11.aspx.cs" Inherits="FCFDFE.pages.CIMS.D.CIMS_D11" %>
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
                    <div>物價指數片語轉檔</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">物價指數片語轉檔</div>
                            <table class="table no-border text-left">
                                <tr>
                                    <td class="text-right" style="width: 40%;">
                                        <asp:Label CssClass="control-label" runat="server">選擇你要上傳的Excel檔案 :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="ful" title="瀏覽..." runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnNew" cssclass="btn-warning btnw6" runat="server" Text="解析檔案內容"/>
                                <asp:Button ID="btnInsert" cssclass="btn-warning btnw6" runat="server" Text="寫入資料庫"/>
                                <asp:Button ID="btnReset" cssclass="btn-default btnw2" runat="server" Text="取消" />
                            </div>
                        </div>
                    </div>
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="subtitle">操作說明</div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="vertical-align:middle; width:25%;">
                                        <asp:Label CssClass="control-label" runat="server">操作程序</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">
                                            1.按下[瀏覽]，選擇欲上傳的Excel檔，上傳檔案為Excel(*.xls)
                                        </asp:Label><br><br>
                                        <asp:Label CssClass="control-label" runat="server">
                                            2.Excel檔內的表頭順序要與下列相符，檔內需含表頭
                                        </asp:Label><br><br>
                                        <table class="table table-bordered" style="text-align:center;">
                                            <tr>
                                                <td><asp:Label CssClass="control-label" runat="server">指數代碼</asp:Label></td>
                                                <td><asp:Label CssClass="control-label" runat="server">基期(單位)</asp:Label></td>
                                                <td><asp:Label CssClass="control-label" runat="server">起始時間點</asp:Label></td>
                                                <td><asp:Label CssClass="control-label" runat="server">最新時間點</asp:Label></td>
                                                <td><asp:Label CssClass="control-label" runat="server">週期</asp:Label></td>
                                                <td><asp:Label CssClass="control-label" runat="server">中文名稱</asp:Label></td>
                                            </tr>
                                        </table>
                                        <asp:Label CssClass="control-label" runat="server">
                                            3.按下[上傳]，即可進行上傳動作
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="subtitle">解析檔案內容</div>
                            <asp:GridView ID="GV_INDEXDESC" DataKeyNames="INDEX_CODE" CssClass="table data-table table-striped border-top table-bordered" OnPreRender="GV_VENAGENT_PreRender" AutoGenerateColumns="false" runat="server" Visible="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="指數代碼" DataField="INDEX_CODE" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="基期(單位)" DataField="UNIT" ItemStyle-Width="15%" />
                                        <asp:BoundField HeaderText="起始時間點" DataField="START_TIME" ItemStyle-Width="15%" />
                                        <asp:BoundField HeaderText="最新時間點" DataField="NEW_TIME" ItemStyle-Width="15%" />
                                        <asp:BoundField HeaderText="週期" DataField="PERIOD" ItemStyle-Width="5%" />
                                        <asp:BoundField HeaderText="中文名稱" DataField="INDEX_DESC" ItemStyle-Width="40%" />
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