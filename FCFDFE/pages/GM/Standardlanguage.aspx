<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Standardlanguage.aspx.cs" Inherits="FCFDFE.pages.GM.Standardlanguage" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                    <asp:Label CssClass="control-label" runat="server">標準用語編輯</asp:Label>
                </header>
                <asp:Panel ID="PnMessage_Save" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                   <td rowspan="6" class="td-vertical">
                                       <asp:Label CssClass="control-label text-vertical-s" style="height: 150px;" runat="server">標準用語種類</asp:Label>
                                   </td>
                                   <td rowspan="4">
                                       <asp:Label CssClass="control-label" runat="server">物資申請書</asp:Label>
                                   </td>
                                   <td>
                                       <asp:Label CssClass="control-label" runat="server">外購用述說明</asp:Label>
                                   </td>
                                   <td class="text-left">
                                       <asp:Button ID="btnM11" CssClass="btn-success btnw11" Text="物資申請書外購用述說明" OnClick="btnM11_Click" runat="server"/>
                                   </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">外購理由說明</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Button ID="btnM21" CssClass="btn-success btnw11" Text="物資申請書外購理由說明" OnClick="btnM21_Click" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">請求事項</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Button ID="btnM3all" CssClass="btn-success btnw12" Text="物資申請書請求事項(B、L、P案)" OnClick="btnM3all_Click" runat="server"/>
                                        <asp:Button ID="btnF3all" CssClass="btn-success btnw12" Text="物資申請書請求事項(M、S案)" OnClick="btnF3all_Click" runat="server"/>
                                        <asp:Button ID="btnW3all" CssClass="btn-success btnw20" Text="物資申請書請求事項(A、C、E、F、W案)" OnClick="btnW3all_Click" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Button ID="btnM4all" CssClass="btn-success btnw12" Text="物資申請書備考(B、L、P案)" OnClick="btnM4all_Click" runat="server" />
                                        <asp:Button ID="btnF4all" CssClass="btn-success btnw12" Text="物資申請書備考(M、S案)" OnClick="btnF4all_Click" runat="server" />
                                        <asp:Button ID="btnW4all" CssClass="btn-success btnw20" Text="物資申請書備考(A、C、E、F、W案)" OnClick="btnW4all_Click" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計畫清單</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備註</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Button ID="btnD5all" CssClass="btn-success btnw12" Text="計畫清單備註(B、L、P案)" OnClick="btnD5all_Click" runat="server" />
                                        <asp:Button ID="btnF5all" CssClass="btn-success btnw12" Text="計畫清單備註(M、S案)" OnClick="btnF5all_Click" runat="server" />
                                        <asp:Button ID="btnW5all" CssClass="btn-success btnw20" Text="計畫清單備註(A、C、E、F、W案)" OnClick="btnW5all_Click" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">採購計畫</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">核定事項</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Button ID="btnA1all" CssClass="btn-success btnw12" Text="核定事項(B、L、P案)" OnClick="btnA1all_Click" runat="server" />
                                        <asp:Button ID="btnA2all" CssClass="btn-success btnw12" Text="核定事項(A、C、E、F、W案)" OnClick="btnA2all_Click" runat="server" />
                                        <asp:Button ID="btnA3all" CssClass="btn-success btnw20" Text="核定事項(M案)" OnClick="btnA3all_Click" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnResetTop" CssClass="btn-default btnw4" Text="取消編輯" OnClick="btnResetTop_Click" runat="server"/><br><br>
                                <asp:Label CssClass="control-label subtitle" runat="server">目前編輯標準用語類別 : 物資申請書外購用述說明</asp:Label>
                                <asp:Panel ID="pn_Button" runat="server"></asp:Panel>
                                <asp:Button ID="btnReset" CssClass="btn-default btnw4" Text="取消編輯" OnClick="btnReset_Click" runat="server"/>      
                            </div>
                            <asp:GridView ID="GV_TBMStandardItem" DataKeyNames="OVC_NO" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowCommand="GV_TBMStandardItem_RowCommand" OnPreRender="GV_TBMStandardItem_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="選項種類" DataField="OVC_MEMO_NAME" HeaderStyle-Width="20%"/>
                                    <asp:TemplateField HeaderText="選擇" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                   		                    <asp:Button ID="DataModify" CssClass="btn-success btnw2" Text="編輯" CommandName ="DataModify" runat="server"/>
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName ="DataDel" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="標準用語" DataField="OVC_MEMO" HeaderStyle-Width="40%"/>
                                    <asp:BoundField HeaderText="適用案別" DataField="OVC_PUR_AGENCY" HeaderStyle-Width="15%"/>
                                    <asp:BoundField HeaderText="備註" DataField="OVC_DESC" HeaderStyle-Width="10%"/>
                	            </Columns>
	   		                </asp:GridView>
                            <br>
                            <div class="text-center">
                                <asp:Label CssClass="control-label subtitle text-red" runat="server">新增區(如果在標準片語找不到您要的用語請在此新增)</asp:Label>
                            </div>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td rowspan="3" class="text-center" style="width:15%">
                                        <asp:Button ID="btnNew" CssClass="btn-success btnw2" Text="新增" OnClick="btnNew_Click" runat="server"/>
                                    </td>
                                    <td style="width:15%"><asp:Label CssClass="control-label" runat="server">標準用語 :</asp:Label></td>
                                    <td style="width:70%">
                                        <asp:TextBox ID="txtOVC_MEMO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">備註 :</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DESC" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" >適用案別 :</asp:Label></td>
                                    <td>
                                        <asp:Panel ID="pn_CheckBox" runat="server"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
