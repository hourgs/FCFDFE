<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C18_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C18_2" %>
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
                    <!--標題-->計畫綜審意見編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Repeater id="rpt_Request" runat="server">
                                  <HeaderTemplate>
                                     <table class="table table-bordered text-center">
                                        <tr>
                                           <td colspan="2">
                                            <asp:Label CssClass="control-label text-red" runat="server" Text="購案編號："></asp:Label>
                                            <asp:Label ID="lblOVC_PURCH" CssClass="control-label text-red" runat="server"></asp:Label>
                                            <asp:Label ID="lblbtnText" CssClass="control-label text-red" runat="server"></asp:Label>
                                           </td>
                                        </tr>
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                     <tr>
                                        <td style="width: 15%"> 
                                            委購單位
                                            <br/>
                                            請求事項
                                        </td>
                                        <td><asp:Label ID="lblRequest" runat="server" Text='<%# Bind("OVC_MEMO") %>'></asp:Label> </td>
                                     </tr>
                                  </ItemTemplate>

                                  <FooterTemplate>
                                     </table>
                                  </FooterTemplate>
                             </asp:Repeater>
                             <asp:GridView ID="GV_EDITMEMO" CssClass="table table-bordered text-center" OnRowCommand="GV_EDITMEMO_RowCommand"  AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="15%" HeaderText="作業">
                                        <ItemTemplate>
                                             <asp:Button ID="btn_change" CssClass="btn-success btnw2" CommandName="btn_change"  Text="異動" runat="server"/>
                                             <asp:Button ID="btnDel" CssClass="btn-danger btnw2"  CommandName="btnDel"  Text="刪除" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="85%" HeaderText="內容">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidONB_NO" runat="server" Value='<%# Bind("ONB_NO") %>'/>
                                            <asp:TextBox ID="txtOVC_MEMO_Main" CssClass="tb tb-full" TextMode="MultiLine" Height="100%" Text='<%# Bind("OVC_MEMO") %>' runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                                <asp:Button ID="btnReturn"  CssClass="btn-warning" OnClick="btnReturn_Click" runat="server" Text="回綜簽作業" />
                            </div>
                            <p></p>
                           <asp:Panel ID="pn_Button" HorizontalAlign="Center" runat="server"></asp:Panel>
                            <p></p>
                            <asp:GridView ID="GV_OVC_MEMO" CssClass="table table-striped border-top text-center" OnRowCommand="GV_OVC_MEMO_RowCommand" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="10%" HeaderText="選項種類" DataField="" />
                                    <asp:TemplateField ItemStyle-Width="10%" HeaderText="選擇">
                                        <ItemTemplate>
                                            
                                            <asp:Button ID="btn_save" CssClass="btn-success btnw4" Text="直接存檔"  CommandName="btn_save" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="65%" HeaderText="標準用語">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOVC_MEMO" CssClass="tb tb-full" TextMode="MultiLine" Height="100%" Text='<%# Bind("OVC_MEMO") %>' runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="15%" HeaderText="備註" DataField="OVC_DESC" />
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                                <asp:Label runat="server" CssClass="control-label text-red" Text="編輯區(如果在標準片語找不到您要的用語請先選擇種類後再在此編輯)"></asp:Label>
                            </div>
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btn_EditSave" CssClass="btn-success btnw2" OnClick="btn_EditSave_Click" Text="存檔" runat="server" /></td>
                                        <td>
                                            <asp:TextBox ID="txtNewOVC_MEMO" CssClass="tb tb-full" TextMode="MultiLine" Height="100%" runat="server"></asp:TextBox></td>
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
