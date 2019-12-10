<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C12.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C12" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    計劃評核－指派承辦人
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">計劃評核－指派承辦人</div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">AA09876L</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案申請(代碼)-申購人(電話)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label" runat="server">國防部政務辦公室-黃XX(01 軍線:234561)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">綜辦單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_SECTION" CssClass="control-label" runat="server">國防部國防採購室</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">分案人</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ASSIGNER" CssClass="control-label" runat="server">計評分案者</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">承辦人</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_CHECKER" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem>黃XX</asp:ListItem>
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">紙本收文日</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <!--↓日期套件↓-->
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DRECEIVE_PAPER" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on">
                                                <i class="icon-calendar"></i>
                                            </div>
                                        </div>
                                        <!--↑日期套件↑-->
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">分派日</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <!--↓日期套件↓-->
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DAUDIT_ASSIGN" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on">
                                                <i class="icon-calendar"></i>
                                            </div>
                                        </div>
                                        <!--↑日期套件↑-->
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查次數</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_CHECK_TIMES" CssClass="tb tb-s" runat="server">1</asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div Class="text-center">
                                <asp:Button ID="btnSave" CssClass="btn-warning btnw4" OnClick="btnSave_Click" runat="server" Text="儲存" /><!--黃色-->
                            </div>
                            <div class="subtitle">結果顯示</div>
                            <asp:GridView ID="GV_PLAN_CHECK" CssClass=" table data-table table-striped border-top " OnRowCommand="GV_PLAN_CHECK_RowCommand" AutoGenerateColumns="false" OnPreRender="GV_PLAN_CHECK_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="審查次數" DataField="ONB_CHECK_TIMES" />
                                    <asp:BoundField HeaderText="審查綜簽日" DataField="OVC_DRESULT" />
                                    <asp:BoundField HeaderText="綜辦單位" DataField="OVC_ONNAME" />
                                    <asp:BoundField HeaderText="綜辦單位分案人" DataField="OVC_ASSIGNER" />
                                    <asp:BoundField HeaderText="分派日" DataField="OVC_DRECEIVE_CHINESE" />
                                    <asp:BoundField HeaderText="綜辦單位承辦人" DataField="OVC_CHECKER" />
                                    <asp:TemplateField HeaderText="功能">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" CommandName="btnSelect" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" cssclass="btn-danger" runat="server" Text="刪除" />
                                            <asp:HiddenField ID ="hidDRecive" runat="server" Value='<%#Bind("OVC_DRECEIVE") %>'/>
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
