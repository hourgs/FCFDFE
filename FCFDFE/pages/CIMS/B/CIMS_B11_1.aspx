<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_B11_1.aspx.cs" Inherits="FCFDFE.pages.CIMS.B.CIMS_B11_1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 800px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>底價表上傳功能</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PURCH_query" runat="server">
                                <div class="subtitle">購案查詢</div>
                                <table class="table table-bordered text-center" style="width: 400px; margin: auto">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">採購案號</asp:Label></td>
                                        <td class="text-left">
                                            <asp:TextBox ID="txtPURCHquery" CssClass="tb tb-m" runat="server">
                                            </asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnQuery" CssClass="btn-success btnw4" runat="server" Text="查詢" OnClick="btnQuery_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" ID="upload" runat="server" >
                                            <asp:Button ID="btnUpload" CssClass="btn-warning btnw8" runat="server" Text="上傳本案底價表" Onclick="btnUpload_Click"/></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                        </div>
                        <div style="width: 100%;">
                            <asp:Panel ID="QueryResult" runat="server" Visible="false">
                                <div class="subtitle">結果顯示</div>
                                <asp:Table ID="tbQueryResult" border="1" CellSpacing="0" runat="server" Width="100%" class="table table-bordered control-label">
                                    <asp:TableRow>
                                        <asp:TableCell Style="text-align: center;" Width="9%">購案名稱
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" Width="23%">
                                            <asp:Label ID="txtOVC_PUR_IPURCH" runat="server" Text="txt"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: center;" Width="9%">承辦人
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" Width="27%">
                                            <asp:Label ID="txtOVC_PUR_USER" runat="server" Text="Label"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: center;" Width="9%">申購日期
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" Width="23%">
                                            <asp:Label ID="txt_OVC_DPROPOSE" runat="server" Text="Label"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Style="text-align: center;" Width="9%">單位代碼
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" Width="23%">
                                            <asp:Label ID="txtOVC_PUR_SECTION" runat="server" Text="Label"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: center;" Width="9%">單位全銜
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" Width="27%">
                                            <asp:Label ID="txtOVC_PUR_NSECTION" runat="server" Text="Label"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: center;" Width="9%">底價表
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" Width="23%">
                                            <asp:Button ID="Dijiquery" CssClass="btn-success btnw5" runat="server" Text="顯示檔案" Onclick="Dijiquery_Click"/>
                                            <asp:Label ID="Dijintdata" runat="server" Text="無資料" Visible="false"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>

                            <asp:Panel ID="DijaUpload" runat="server" Visible="false">
                                <asp:Table ID="tbDijaUpload" Style="width: 100%;" border="1" CellSpacing="0" runat="server" class="table table-bordered control-label">
                                    <asp:TableRow>
                                        <asp:TableCell Style="text-align: center;">採購案號</asp:TableCell>
                                        <asp:TableCell Style="text-align: left;">
                                            <asp:Label ID="uploadPURCH" runat="server"></asp:Label></asp:TableCell>
                                        <asp:TableCell Style="text-align: center;">購案名稱</asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" colspan="3">
                                            <asp:Label ID="uploadOVC_PUR_IPURCH" runat="server"></asp:Label></asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Style="text-align: center;">附件類別</asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" colspan="5">
                                            <asp:DropDownList ID="uploadOVC_ATTACH_NAME" runat="server">
                                                <asp:ListItem Value="底價表">底價表</asp:ListItem>
                                            </asp:DropDownList></asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Style="text-align: center;">組<br/><font color="red">[若無分組則填0]</font></asp:TableCell>
                                        <asp:TableCell Style="text-align: left;">
                                            <asp:TextBox ID="uploadOVC_ITEM" runat="server"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: center;">次</asp:TableCell>
                                        <asp:TableCell Style="text-align: left;">
                                            <asp:TextBox ID="uploadOVC_TIMES" runat="server"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Style="text-align: center;">附件序號<br/><font color="red">(自編流水號)</font></asp:TableCell>
                                        <asp:TableCell Style="text-align: left;">
                                            <asp:TextBox ID="uploadOVC_SUB" runat="server"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Style="text-align: center;">檔案上傳</asp:TableCell>
                                        <asp:TableCell Style="text-align: left;" colspan="5">
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Style="text-align: center;" colspan="6">
                                            <asp:Button ID="uploadConfirm" CssClass="btn-success btnw5" runat="server" Text="新增" OnClick="uploadConfirm_Click" />
                                            <asp:Button ID="uploadCancel" CssClass="btn-success btnw5" runat="server" Text="返回" OnClick="uploadCancel_Click" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>

                            <asp:Panel ID="Detail" runat="server" Visible="false">
                                <asp:GridView ID="GV_TBM1301" CssClass="table data-table table-striped border-top table-bordered" DataKeyNames="OVC_PURCH,OVC_ITEM,OVC_TIMES,OVC_SUB" AutoGenerateColumns="false" runat="server" OnPreRender="GV_TBM1301_PreRender" OnRowDataBound="GV_TBM1301_RowDataBound" OnRowCommand="GV_TBM1301_RowCommand">
                                    <Columns>
                                        <asp:BoundField HeaderText="項次" DataField="RANK" ItemStyle-Width="8%" />
                                        <asp:BoundField HeaderText="採購案號" DataField="OVC_PURCH" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="附檔種類" DataField="OVC_ATTACH_NAME" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="組" DataField="OVC_ITEM" ItemStyle-Width="6%" />
                                        <asp:BoundField HeaderText="次" DataField="OVC_TIMES" ItemStyle-Width="6%" />
                                        <asp:BoundField HeaderText="附件序號" DataField="OVC_SUB" ItemStyle-Width="10%" />
                                        <asp:TemplateField HeaderText="附檔名稱">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDownload" Text='<%# Eval("OVC_FILE_NAME")%>' CommandName="downloadfile"  runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="指令">
                                            <ItemTemplate>
                                                <asp:Button CssClass="btn-info btnw2" CommandName="Del" runat="server" Text="刪除" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="Button1" CssClass="btn-success btnw5" runat="server" Text="返回" OnClick="uploadCancel_Click" visible="false"/>
                            </asp:Panel>
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
