<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13_8_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13_8_2" %>
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
                    <!--標題-->條文內容編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="control-label text-red" Font-Size="Medium" runat="server" Text="目前編輯："></asp:Label>
                                 <asp:Label ID="lblDOCLAWNAME" Font-Size="Medium" CssClass="control-label text-red" runat="server"></asp:Label>
                            </div>
                            <div class="text-center">
                                 <asp:Button ID="btn_Return" CssClass="btn-success btnw4" Text="回上一頁" OnClick="btn_Return_Click" Enabled="true" runat="server" />
                            </div>
                            <asp:GridView ID="GV_DOCCONTENT" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="序號" DataField="DOC_CON_NO"/>
                                    <asp:TemplateField ItemStyle-Width="15%" HeaderText="編輯功能">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_TempSave" CssClass="btn-success btnw2" Text="暫存" OnClick="btn_TempSave_Click" Enabled="true" runat="server" />
                                            <asp:Button ID="btn_CommitSave" CssClass="btn-success btnw2" Text="確認" OnClick="btn_CommitSave_Click" Enabled="true" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="編輯狀態" DataField="DOC_COMMIT" />
                                    <asp:TemplateField HeaderText="條文內容">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDocContent" CssClass="tb tb-full" TextMode="MultiLine" Text='<%# Bind("DOC_CON") %>'  Rows=<%# Eval("DOC_CON").ToString().Length/50 +1 %> Height="100%" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:BoundField HeaderText="備註" DataField="DOC_CON_DESC" />
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
