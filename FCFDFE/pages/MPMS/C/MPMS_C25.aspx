<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C25.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C25" %>
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
                                <asp:Label ID="Label1" runat="server" Text="計畫年度（第二組）"></asp:Label>
                                <asp:DropDownList ID="drpYEAR" CssClass="tb tb-s" runat="server"></asp:DropDownList> 
                                <asp:Button ID="btnYearQuery" CssClass="btn-success btnw2" OnClick="btnYearQuery_Click" Text="查詢" runat="server" />
                            </div>
                            
                            <asp:GridView ID="GV_NOT" CssClass=" table data-table table-striped border-top" 
                                AutoGenerateColumns="false" OnPreRender="GV_NOT_PreRender" OnRowCommand="GV_NOT_RowCommand" runat="server">
                                 <Columns>
                                    <asp:TemplateField HeaderText="序號" >
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" runat="server" Text="<%# ((GridViewRow) Container).RowIndex +1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="承辦人姓名" DataField="Name" />
                                    <asp:BoundField HeaderText="審查中案數" DataField="DoingCase" />
                                    <asp:BoundField HeaderText="已核定案數" DataField="DoneCase" />
                                    <asp:BoundField HeaderText="承辦案數" DataField="AllCase" />
                                    <asp:TemplateField HeaderText="作業" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-warning btnw2" CommandName="btnSave" ID="btnSave" Text="選擇" runat="server"/>
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
