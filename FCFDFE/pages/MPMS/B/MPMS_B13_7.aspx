<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13_7.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13_7" %>
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
                    <!--標題-->編輯採購計畫清單備考
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-red" runat="server" Text="購案編號："></asp:Label>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label text-red" runat="server"></asp:Label>
                                        <asp:Label ID="lblbtnText" CssClass="control-label text-red" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         <asp:GridView ID="GV_EDITMEMO" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="作業">
                                                    <ItemTemplate>
                                                         <asp:Button ID="btn_change" CssClass="btn-success btnw2" OnClick="btn_change_Click" Text="異動" runat="server"/>
                                                         <asp:Button ID="btnDel" CssClass="btn-danger btnw2" OnClick="btnDel_Click" Text="刪除" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="85%" HeaderText="內容">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidONB_NO" runat="server" Value='<%# Bind("ONB_NO") %>'/>
                                                        <asp:TextBox ID="txtOVC_MEMO_Main" CssClass="tb tb-full" TextMode="MultiLine" Text='<%# Bind("OVC_MEMO") %>'  Rows=<%# Eval("OVC_MEMO").ToString().Length/70 +1 %> Height="100%" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                    </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" OnClick="btnReturn_Click" CssClass="btn-warning" runat="server" Text="回物資申請書編制作業" />
                            </div>
                            <table  style="border: medium solid #FF0000;">
                               <tr>
                                    <td style="text-align:center">
                                        <div  style="color:blue; font-size:24px; border-style:solid; border-color:red; border-bottom-width: 1px" >必　要　選　項</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="border-style:solid; border-color:red; border-bottom-width: 1px">
                                            <asp:Panel HorizontalAlign="Center" ID="pn_Button" runat="server"></asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <p></p>
                            <asp:Panel HorizontalAlign="Center" ID="pn_Button2" runat="server"></asp:Panel>
                            <p></p>
                            <asp:GridView ID="GV_OVC_ISOURCE" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="150px" HeaderText="選項種類" DataField=""/>
                                    <asp:TemplateField HeaderText="選擇">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSave" CssClass="btn-success btnw4" Text="直接存檔" onClick="btnSave_Click" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="標準用語">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidOVC_CHECK" runat="server" Value='<%# Bind("OVC_CHECK") %>'/>
                                            <asp:TextBox ID="txtOVC_MEMO" CssClass="tb tb-full" Text='<%# Bind("OVC_MEMO") %>' Rows=<%# Eval("OVC_MEMO").ToString().Length/60 +1 %> TextMode="MultiLine" Height="100%" runat="server" ></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="備註" DataField="" />
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                                <asp:Label runat="server" CssClass="control-label text-red" Text="編輯區(如果在標準片語找不到您要的用語請先選擇種類後再在此編輯)"></asp:Label>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Button ID="btnEdit_Save" CssClass="btn-success btnw2" Text="存檔" OnClick="btnEdit_Save_Click" runat="server" /></td>
                                    <td><asp:TextBox ID="txtNewOVC_MEMO" CssClass="tb tb-full" TextMode="MultiLine" Height="100%" runat="server"></asp:TextBox></td>
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
