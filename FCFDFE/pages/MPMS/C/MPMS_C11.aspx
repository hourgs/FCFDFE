<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C11.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C11" %>
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
                    <!--標題-->計劃審核分案
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
                            <div class="subtitle text-center">尚未分辦購案明細</div>
                            <asp:GridView ID="GV_NOT" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false" OnPreRender="GV_NOT_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:BoundField HeaderText="委購單位" DataField="OVC_PUR_NSECTION" />
                                    <asp:BoundField HeaderText="承辦人(電話)" DataField="OVC_PUR_USER" />
                                    <asp:BoundField HeaderText="申購日(資料傳送日)" DataField="OVC_DPROPOSE" HtmlEncode="False" />
                                    <asp:BoundField HeaderText="申購文號" DataField="OVC_PROPOSE" />
                                    <asp:TemplateField HeaderText="功能" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-warning btnw2" OnClick ="btnModify_Click" ID="btnModify" Text="指派" CommandName="按鈕屬性"
                                                CommandArgument='<%--#Eval("傳變數")--%>' runat="server"/>
                                            <asp:HiddenField ID="hidOVC_PURCH" Value='<%#Bind("OVC_PURCH_ORG") %>' runat="server" />
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