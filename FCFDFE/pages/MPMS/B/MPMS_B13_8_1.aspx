<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13_8_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13_8_1" %>
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
                    <!--標題-->採購文件編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="control-label text-red" Font-Size="Medium" runat="server" Text="目前編輯："></asp:Label>
                                 <asp:Label ID="lblDOCTYPE" Font-Size="Medium" CssClass="control-label text-red" runat="server"></asp:Label>
                            </div>
                            <div class="text-center">
                                 <asp:Button ID="btn_Return" CssClass="btn-success btnw4" Text="回上一頁" OnClick="btn_Return_Click" Enabled="true" runat="server" />
                            </div>
                            <asp:GridView ID="GV_DOCLAW" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="序號" DataField="DOC_LAW_NO"/>
                                    <asp:BoundField HeaderText="契約條文" DataField="DOC_LAW_NAME" />
                                    <asp:BoundField HeaderText="編輯狀態" DataField="DOC_NO" />
                                    <asp:TemplateField HeaderText="編輯內文">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_Modify" CssClass="btn-success btnw4" Text="編輯內文" OnClick="btn_Modify_Click" Enabled="true" runat="server" />
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
