<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E12.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E12" %>
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
                    <!--標題-->選擇尚未接管的案號
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GV_TakeOver" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_TakeOver_PreRender" OnRowDataBound="GV_TakeOver_RowDataBound" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                            <asp:Label ID="labOVC_PURCH" Text='<%# Bind("OVC_PURCH") %>' runat="server" />
                                            <asp:Label ID="labOVC_PUR_AGENCY" Text='<%# Bind("OVC_PUR_AGENCY") %>' Visible="false" runat="server" />
                                            <asp:Label ID="labOVC_PURCH_5" Text='<%# Bind("OVC_PURCH_5") %>' Visible="false" runat="server" />
                                            <asp:Label ID="labOVC_PURCH_6" Text='<%# Bind("OVC_PURCH_6") %>' Visible="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                                    <asp:BoundField HeaderText="委辦單位" DataField="OVC_PUR_NSECTION" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:BoundField HeaderText="得標商名稱" DataField="OVC_VEN_TITLE" />
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                   		                    <asp:Button ID="BtnTakeOver" CssClass="btn-success btnw2 text-center" Text="接管" OnClick="BtnTakeOver_Click" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
