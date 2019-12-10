<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B12.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B12" %>
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
                    <!--標題-->複製購案
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">選擇欲搜尋的條件，案下[搜尋]即可搜尋 </div>
                            <table class="table table-bordered" style="text-align:left;">
                                <tr>
                                    <td style="width:25%;"><asp:Label CssClass="control-label" runat="server">顯示所有可複制之購案範例：</asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheckAll" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">是</asp:ListItem>
                                            <asp:ListItem Value="2">否</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">採購屬性：</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_LAB" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">招標方式：</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">投標段次：</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_BID_TIMES" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">決標原則：</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_BID" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">採購途徑:</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_ITEM" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">招標單位：</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_AGNT_IN" CssClass="tb tb-l" runat="server" OnSelectedIndexChanged="drpOVC_AGNT_IN_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:TextBox ID="txtOVC_AGNT_IN" CssClass="tb tb-l" runat="server" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">購案編號：</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">購案名稱：</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_PUR_IPURCH" CssClass="tb tb-m" runat="server">

                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">授權開放的其它單位：</asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoOtherAuth" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">是</asp:ListItem>
                                            <asp:ListItem Value="2">否</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center" style="letter-spacing:30px;">
                                <asp:Button ID="btnQuery" cssclass="btn-success btnw4" runat="server" OnClick="btnQuery_Click" Text="搜尋" />
                                <asp:Button ID="btnReset" cssclass="btn-default btnw4" runat="server" Text="取消" />
                            </div>
                            <asp:Panel ID="Panel1" runat="server">
                                <div class="subtitle"> 查詢結果 </div>
                                <asp:GridView ID="GV_OVC_BUDGET" CssClass=" table data-table table-striped border-top table-bordered " DataKeyNames="OVC_PURCH" OnPreRender="GV_OVC_BUDGET_PreRender" OnRowCommand="GV_OVC_BUDGET_RowCommand" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="選擇" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" ID="btnCopy" Text="複製"  CommandName="DataCopy" CommandArgument="<%# Container.DataItemIndex%>" UseSubmitBehavior="false" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" /> <%--1301plan--%>
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" /> <%--1301plan--%>
                                    <asp:BoundField HeaderText="採購屬性" DataField="OVC_LAB" /> <%--1301plan--%> 
                                    <asp:BoundField HeaderText="招標方式" DataField="OVC_PUR_ASS_VEN_CODE" /> <%--1301plan--%> 
                                    <asp:BoundField HeaderText="投標段次" DataField="OVC_BID_TIMES" /> <%--1301--%> 
                                    <asp:BoundField HeaderText="決標原則" DataField="OVC_BID" /> <%--1301--%> 
                                    <asp:BoundField HeaderText="採購途徑" DataField="OVC_ITEM" /> <%--1301 OVC_PURCH_KIND 內外購別(1->內2->外)--%> 
                                    <asp:BoundField HeaderText="招標單位" DataField="OVC_AGNT_IN" /> <%--1301 採購單位--%> 
                	            </Columns>
	   		               </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
