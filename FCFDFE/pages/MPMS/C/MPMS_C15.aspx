<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C15.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C15" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 800px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->計劃評核－異動聯審
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <h4> 
                                    <asp:Label CssClass="control-label" ID="Label4" runat="server">購案編號：</asp:Label>
                                    <asp:Label CssClass="control-label" ID="lblOVC_PURCH" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label" ID="Label2" runat="server">審查次數:</asp:Label>
                                    <asp:Label CssClass="control-label" ID="lblONB_CHECK_TIMES" runat="server"></asp:Label>
                                </h4>
                            </div>
                            <table class="table table-bordered text-left">
                                <tr class="no-bordered">
                                    <td>
                                        <asp:Label ID="lblK500" CssClass="control-label" runat="server">法務</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rdo_K500" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">新增</asp:ListItem>
                                            <asp:ListItem Value="2">刪除</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClearK500" CssClass="btn-default btnw4" OnCommand="btnClear_Command" CommandArgument="rdo_K500" runat="server" Text="清除" /><!--灰色-->
                                    </td>
                                </tr>
                                <tr class="no-bordered">
                                    <td>
                                        <asp:Label ID="lblK501" CssClass="control-label" runat="server">接轉</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rdo_K501" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">新增</asp:ListItem>
                                            <asp:ListItem Value="2">刪除</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClearK501" CssClass="btn-default btnw4" OnCommand="btnClear_Command" CommandArgument="rdo_K501" runat="server" Text="清除" /><!--灰色-->
                                    </td>
                                </tr>
                                <tr class="no-bordered">
                                    <td>
                                        <asp:Label ID="lblK502" CssClass="control-label" runat="server">商情</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rdo_K502" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">新增</asp:ListItem>
                                            <asp:ListItem Value="2">刪除</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClearK502" CssClass="btn-default btnw4" OnCommand="btnClear_Command" CommandArgument="rdo_K502" runat="server" Text="清除" /><!--灰色-->
                                    </td>
                                </tr>
                                <tr class="no-bordered">
                                    <td>
                                        <asp:Label ID="lblK503" CssClass="control-label" runat="server">評核</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rdo_K503" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">新增</asp:ListItem>
                                            <asp:ListItem Value="2">刪除</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClearK503" CssClass="btn-default btnw4" OnCommand="btnClear_Command" CommandArgument="rdo_K503" runat="server" Text="清除" /><!--灰色-->
                                    </td>
                                </tr>
                                <tr class="no-bordered">
                                    <td>
                                        <asp:Label ID="lblK504" CssClass="control-label" runat="server">採購</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rdo_K504" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">新增</asp:ListItem>
                                            <asp:ListItem Value="2">刪除</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClearK504" CssClass="btn-default btnw4" OnCommand="btnClear_Command" CommandArgument="rdo_K504" runat="server" Text="清除" /><!--灰色-->
                                    </td>
                                </tr>
                                <tr class="no-bordered">
                                    <td>
                                        <asp:Label ID="lblK505" CssClass="control-label" runat="server">履驗</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rdo_K505" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">新增</asp:ListItem>
                                            <asp:ListItem Value="2">刪除</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClearK505" CssClass="btn-default btnw4" OnCommand="btnClear_Command" CommandArgument="rdo_K505" runat="server" Text="清除" /><!--灰色-->
                                    </td>
                                </tr>
                                
                                <tr class="no-bordered">
                                    <td>
                                        <asp:Label ID="lblK506" CssClass="control-label" runat="server">採包</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rdo_K506" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">新增</asp:ListItem>
                                            <asp:ListItem Value="2">刪除</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClearK506" CssClass="btn-default btnw4" OnCommand="btnClear_Command" CommandArgument="rdo_K506" runat="server" Text="清除" /><!--灰色-->
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnSave" CssClass="btn-success btnw4" OnClick="btnSave_Click" runat="server" Text="存檔" />
                                <asp:Button ID="btnReturn" CssClass="btn-warning btnw4" OnClick="btnReturn_Click" runat="server" Text="回上頁" />
                            </div>
                            <div class="subtitle">已選擇的審查單位</div>
                            <asp:GridView ID="GV_alreadych" CssClass="table data-table border-top text-center" AutoGenerateColumns="false" OnPreRender="GV_alreadych_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="已選擇的審查單位" DataField="OVC_USR_ID" />
                	            </Columns>
	   		               </asp:GridView>
                            <div class="text-center">
                                <asp:Button ID="btn_FIRST_PRINT" CssClass="btn-success" OnClick="btn_FIRST_PRINT_Click" runat="server" Text="列印初審會辦單" />
                            </div>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
