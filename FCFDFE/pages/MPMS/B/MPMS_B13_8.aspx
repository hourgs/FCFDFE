<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13_8.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13_8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->計畫清單<br>
                    採購文件選單
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="control-label text-red" Font-Size="Medium" runat="server" Text="購案編號："></asp:Label>
                                 <asp:Label ID="lblOVC_PURCH" Font-Size="Medium" CssClass="control-label text-red" runat="server"></asp:Label>
                                <p></p>
                                <asp:Button ID="btn_Return" CssClass="btn-success" Text="回計畫清單" OnClick="btn_Return_Click" Enabled="true" runat="server" />
                            </div>
                            
                            <asp:GridView ID="GV_DOCTYPE" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="採購文件名稱" DataField="DOC_NAME"/>
                                    <asp:BoundField HeaderText="文件狀況" DataField="PURCH_NO" />
                                    <asp:BoundField HeaderText="編輯狀況" DataField="DOC_NO" />
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidDOC_TYPENO" Value='<%# Bind("DOC_TYPENO") %>' runat="server"/>
                                            <asp:Button ID="btn_save" CssClass="btn-success btnw4" Text="新增" OnClick="btn_save_Click"  runat="server" />
                                            <asp:Button ID="btn_modify" CssClass="btn-success btnw4  disabled" Text="修改" OnClick="btn_modify_Click"  runat="server" />
                                            <asp:Button ID="btn_delete" CssClass="btn-danger btnw4  disabled" Text="刪除" OnClick="btn_delete_Click"  runat="server" />
                                            <asp:Button ID="btn_print" CssClass="btn-success btnw4  disabled" Text="列印" OnClick="btn_print_Click"   runat="server" />
                                        </ItemTemplate>
                                </asp:TemplateField>
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
