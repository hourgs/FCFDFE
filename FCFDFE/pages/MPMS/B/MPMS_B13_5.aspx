<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13_5.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13_5" %>
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
                    <!--標題-->編輯外購物資申請書用途
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td colspan="2" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">購案編號：</asp:Label>
                                        <asp:Label ID="lblPURCHNUM" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">用途編輯</asp:Label>
                                    </td>
                                </tr>
                                <tr class="screentone-gray">
                                    <td style="width:15%"><asp:Label CssClass="control-label" runat="server">作業</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">內容</asp:Label></td>
                                </tr>
                                <tr id="trMemo" runat="server" visible="false">
                                    <td >
                                        <asp:Button ID="BtnChange" CssClass="btn-success btnw2" OnClick="BtnChange_Click" runat="server" Text="異動" />
                                        <asp:Button ID="Btndel" CssClass="btn-success btnw2" runat="server" OnClick="Btndel_Click" Text="刪除" />
                                    </td>
                                    <td><asp:TextBox ID="txtOVC_MEMO" CssClass="tb tb-full" TextMode="MultiLine" Height="100%" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" OnClick="btnReturn_Click" CssClass="btn-success" runat="server" Text="回物資申請書編制作業" /><!--綠色-->
                            </div>
                            <p></p>
                            <asp:Panel ID="pn_Button" HorizontalAlign="Center"  runat="server"></asp:Panel>
                            <p></p>
                            <asp:GridView ID="GV_UseSTANDARD" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_UseSTANDARD_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="10%" HeaderText="選項種類" DataField="" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" HeaderText="選擇" >
                                        <ItemTemplate>
                   		                    <asp:Button ID="BtnSave" CssClass="btn-success btnw4" Text="直接存檔" OnClick ="BtnSave_Click" CommandName="按鈕屬性" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="標準用語" >
                                        <ItemTemplate>
                   		                    <asp:TextBox ID="txtSTANDARD" CssClass="tb tb-full" TextMode="MultiLine" Height="100%" Text='<%# Bind("OVC_MEMO") %>' runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField ItemStyle-Width="15%" HeaderText="備註" DataField="OVC_DESC" />
                	            </Columns>
	   		                </asp:GridView>
                        </div>
                        <div class="text-center">
                            <asp:Label CssClass="control-label subtitle text-red" runat="server">新增區(如果在標準片語找不到您要的用語請先選擇種類後在此編輯)</asp:Label>
                        </div>
                        <table class="table table-bordered text-left">
                            <tr>
                                <td  style="width:15%" class="text-center">
                                    <asp:Button ID="btnNew" CssClass="btn-success btnw2" Text="存檔" OnClick="btnNew_Click" runat="server"/>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtNewMemo" CssClass="tb tb-l" TextMode="MultiLine" Height="100%" Width="100%" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
