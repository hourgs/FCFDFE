<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C16_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C16_1" %>
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
                    <!--標題-->聯審小組改分作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Button ID="btnBack" CssClass="btn-success btnw4" Text="回上一頁" OnClick="btnBack_Click" runat="server" />
                            </div>
                             <asp:GridView ID="GV_NOT" CssClass=" table data-table table-striped border-top " 
                                AutoGenerateColumns="false" OnPreRender="GV_NOT_PreRender" OnRowDataBound="GV_NOT_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="作業" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-warning btnw4" ID="btnSave" Text="確認指派" OnClick="btnSave_Click" Visible="false" runat="server"/>                                            
                                            <asp:Label ID="lblDate" CssClass="control-label" ForeColor="Red" Visible="false" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_PURCH" CssClass="control-label" Text='<%# "" +Eval("OVC_PURCH") + Eval("OVC_PUR_AGENCY")%>'  runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:BoundField HeaderText="委購單位" DataField="OVC_PUR_NSECTION" />
                                    <asp:BoundField HeaderText="審查次數" DataField="ONB_CHECK_TIMES" />
                                    <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="分派日<br/>回覆日" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblvDate" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hidOVC_DAUDIT_ASSIGN" runat="server" Value='<%#Bind("OVC_DAUDIT_ASSIGN")%>'/>
                                            <asp:HiddenField ID="hidOVC_DAUDIT" runat="server" Value='<%#Bind("OVC_DAUDIT")%>'/>
                                            <asp:HiddenField ID="hidOVC_DRESULT" runat="server" Value='<%#Bind("OVC_DRESULT")%>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="承辦人" >
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpOVC_CHECKER" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>黃XX</asp:ListItem>
                                        </asp:DropDownList>
                                            <asp:Label ID="lblOVC_AUDITOR" CssClass="control-label" Visible="false" Text='<%#Bind("OVC_AUDITOR")%>' runat="server"></asp:Label>
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
