<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C20.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C20" %>

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
                    <!--標題-->計畫審查改分作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label ID="Label1" runat="server" Text="計畫年度（第二組）"></asp:Label>
                                <asp:DropDownList ID="drpYEAR" CssClass="tb tb-s" runat="server"></asp:DropDownList> 
                                <asp:Label ID="Label2" runat="server" Text="承辦人 "></asp:Label>
                                <asp:DropDownList ID="drpOVC_CHECKER" CssClass="tb tb-s" runat="server"></asp:DropDownList> 
                                <asp:Button ID="btnYearQuery" CssClass="btn-success btnw2" OnClick="btnYearQuery_Click" Text="查詢" runat="server" />
                            </div>
                            <asp:GridView ID="GV_NOT" CssClass=" table data-table table-striped border-top "  OnRowCommand="GV_NOT_RowCommand"
                                AutoGenerateColumns="false" OnPreRender="GV_NOT_PreRender" OnRowDataBound="GV_NOT_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:BoundField HeaderText="委購單位" DataField="OVC_PUR_NSECTION" />
                                    <asp:BoundField ItemStyle-Width="5%" HeaderText="審查次數" DataField="ONB_CHECK_TIMES" />
                                    <asp:BoundField HeaderText="分派日<br/>回覆日" DataField="OVC_DRECEIVE" HtmlEncode="False" />
                                     <asp:TemplateField HeaderText="作業" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-warning" ID="btnSelect" CommandName="btnSelect" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="確認指派" runat="server"/>
                                            <asp:Label ID="lblDRESULT_TITELE" Visible="false" runat="server" Text="審查綜簽日"></asp:Label>
                                            <br/>
                                            <asp:Label ID="lblOVC_DRESULT" Visible="false" Text='<%#Bind("OVC_DRESULT") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="承辦人" >
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpOVC_CHECKER" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>黃XX</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCHECKER" Visible="false" runat="server" Text='<%#Bind("OVC_CHECKER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                	            </Columns>
	   		               </asp:GridView>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
