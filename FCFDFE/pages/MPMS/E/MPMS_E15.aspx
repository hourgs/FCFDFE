<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E15.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E15" %>
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
                    <!--標題-->
                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label text-red" runat="server" Text=""></asp:Label>
                    購案明細表
                </header>   
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                                <div class="text-center">
                                    <asp:Label CssClass="control-label" runat="server" Text="購案編號：" Font-Size="Large"></asp:Label>
                                    <asp:Label ID="Label1" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    <p></p>
                                </div>
                                <asp:GridView ID="GV_TBMANNOUNCE_OPEN" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_TBMANNOUNCE_OPEN_PreRender" OnRowDataBound="GV_TBMANNOUNCE_OPEN_RowDataBound" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="作業">
                                            <ItemTemplate>
                   		                        <asp:Button ID="btnChange" CssClass="btn-success btnw2 text-center" Text="異動" OnClick="btnChange_Click" Visible="false" runat="server" />
                                                <asp:Button ID="btnSaveOVC_CONTRACT_END" CssClass="btn-warning text-center" Text="儲存結束日" OnClick="btnSaveOVC_CONTRACT_END_Click" Visible="false" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="收辦日" DataField=""/>
                                        <asp:TemplateField HeaderText="結束日">
                                            <ItemTemplate>
                                                <asp:Panel ID="panDate" Visible="true" runat="server">
                                                    <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_CONTRACT_END" CssClass="tb tb-s" Visible="true" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                    </div>
                                                <!--↑日期套件↑-->
                                                    <asp:Label CssClass="control-label position-left" runat="server"></asp:Label><!--後方備註文字，跟日期同一行需使用"position-left"之class-->
                                                </asp:Panel>
                                                <asp:Label ID="labOVC_CONTRACT_END" Visible="false" Text="" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="承辦人" DataField="" />
                                        <asp:BoundField HeaderText="購案階段" DataField="" />
                                        <asp:BoundField HeaderText="作業工數" DataField="" />
                	                </Columns>
	   		                    </asp:GridView>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
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
