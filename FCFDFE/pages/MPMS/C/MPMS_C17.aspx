<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C17.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C17" %>

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
                    審查單位－
                    <asp:Label ID="lblTitleUnit" CssClass="control-label" runat="server"></asp:Label>
                    <asp:DropDownList ID="drpAuditUnit" 
                                            OnSelectedIndexChanged="drpAuditUnit_SelectedIndexChanged" 
                                            AutoPostBack="true" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="03">計評</asp:ListItem>
                    </asp:DropDownList>　
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">AA0980l</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td colspan="5" class="text-left">
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server">購案測試01</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">委購單位</asp:Label>
                                    </td>
                                    <td colspan="7" class="text-left">
                                        <asp:Label ID="lblOVC_AGENT_UNIT" CssClass="control-label" runat="server">國防部政務辦公室-陳XX(11 軍線:237543)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查次數</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_CHECK_TIMES" CssClass="control-label" runat="server">1</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">分派日</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_DAUDIT_ASSIGN" CssClass="control-label" runat="server">106年03月03日</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">紙本收文日</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_DRECEIVE_PAPER" CssClass="control-label" runat="server">106年04月04日</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">綜辦單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_CHECK_UNIT" CssClass="control-label" runat="server">國防部國防採購室(簡XX)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_AUDIT_UNIT" CssClass="control-label" runat="server">採購處</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">回覆日</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_DAUDIT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查人</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblUSER_ID" CssClass="control-label" runat="server">布XX</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查類型</asp:Label>
                                    </td>
                                    <td colspan="7" class="text-left">
                                        <asp:DropDownList ID="drpOVC_TITLE" 
                                            OnSelectedIndexChanged="drpOVC_TITLE_SelectedIndexChanged" 
                                            AutoPostBack="true" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>請選擇</asp:ListItem>
                                        </asp:DropDownList>　
                                        <asp:LinkButton ID="btnToMemo" runat="server">顯示物資申請書請求事項</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trItem" visible="false" runat="server">
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查項目</asp:Label>
                                    </td>
                                    <td colspan="7" class="text-left">
                                        <asp:DropDownList ID="drpOVC_ITEM" 
                                            OnSelectedIndexChanged="drpOVC_ITEM_SelectedIndexChanged" 
                                            AutoPostBack="true" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>請選擇</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trDetail" visible="false" runat="server">
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查標題</asp:Label>
                                    </td>
                                    <td colspan="7" class="text-left">
                                        <asp:DropDownList ID="drpOVC_Detail" 
                                            OnSelectedIndexChanged="drpOVC_Detail_SelectedIndexChanged" 
                                            AutoPostBack="true" CssClass ="tb tb-s" runat="server">
                                            <asp:ListItem>請選擇</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trMemo" visible="false" runat="server">
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查意見</asp:Label>
                                    </td>
                                    <td colspan="7" class="text-left">
                                        <asp:TextBox ID="txtOVC_MEMO" CssClass="tb tb-full" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trSave" visible="false" runat="server">
                                    <td colspan="8">
                                        <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn-warning btnw4" runat="server" Text="審查新增" />
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                            <asp:Button ID="btnReturn" OnClick="btnReturn_Click" CssClass="btn-warning btnw4" runat="server" Text="回上頁" />
                            </div>
                            <div>
                                <asp:GridView ID="GV_Comment" CssClass=" table data-table table-striped" 
                                    AutoGenerateColumns="false" OnPreRender="GV_Comment_PreRender" OnRowDataBound="GV_Comment_RowDataBound" runat="server">
                                <Columns>
                                     <asp:TemplateField HeaderText="項次">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="審查類別" DataField="OVC_TITLE_NAME" />
                                    <asp:BoundField HeaderText="審查項目" DataField="OVC_TITLE_ITEM_NAME" />
                                    <asp:BoundField HeaderText="審查標題" DataField="OVC_TITLE_DETAIL_NAME" />
                                    <asp:TemplateField HeaderText="審查內容">
                                        <ItemTemplate>
                                             <asp:TextBox ID="txtGV_MEMO" CssClass="tb tb-full" 
                                                 Text='<%# Bind("OVC_CHECK_REASON") %>' TextMode="MultiLine" 
                                                 Height="100%" Rows=<%# Eval("OVC_CHECK_REASON").ToString().Length/30 +1 %>
                                                 runat="server">
                                             </asp:TextBox>
                                            <asp:HiddenField id="hidONB_NO" Value='<%# Bind("ONB_NO") %>' runat="server"/>
                                             <asp:HiddenField id="hidTITLE" Value='<%# Bind("OVC_TITLE") %>' runat="server"/>
                                             <asp:HiddenField id="hidTITLE_ITEM" Value='<%# Bind("OVC_TITLE_ITEM") %>' runat="server"/>
                                             <asp:HiddenField id="hidTITLE_DETAIL" Value='<%# Bind("OVC_TITLE_DETAIL") %>'  runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="功能">
                                        <ItemTemplate>
                                            <asp:Button ID="btnModifySave" cssclass="btn-success" runat="server" 
                                                OnCommand="btnInGV_Command" CommandName="ModifySave" Text="異動存檔" />
                                            <asp:Button ID="btnDel" cssclass="btn-danger" runat="server" 
                                                OnCommand="btnInGV_Command" CommandName="Del" Text="刪除" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                	            </Columns>
	   		               </asp:GridView>
                                <div class="text-center">
                                <asp:Button ID="btn_TOCOMMENT"  CssClass="btn-warning btnw4 text-center" runat="server" Text="審查意見" />
                                 </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
