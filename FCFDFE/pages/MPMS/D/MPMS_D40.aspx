<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D40.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D40" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <style>
        .subtitle1 {
            font-size: 18px;
            text-align: center;
            padding-bottom: 10px;
        }
        div > a {
            padding:10px;
        }
    </style>
    <div class="row">
        <div style="width: 1200px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    購案契約及附件分配編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                                <div class="subtitle1 text-red">
                                    購案契約編號：<asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                </div>
                                <asp:GridView ID="GV_info"  CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_NOTICE_TITLE" OnRowCommand="GV_info_RowCommand" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="作業">
                                            <ItemTemplate>
                                                <asp:Button CssClass="btn-default btnw2" Text="異動" CommandName="btnMove" runat="server" />
                                                <asp:Button  CssClass="btn-warning btnw2" Text="刪除" CommandName="btnDel"  OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="分配單位" DataField="OVC_NOTICE_TITLE" />
                                        <asp:BoundField HeaderText="分配契約及附件" DataField="OVC_ATTACH_NAME" />
                                        <asp:BoundField HeaderText="備考" DataField="OVC_MEMO" />
                                    </Columns>
                                </asp:GridView>

                                <table class="table table-bordered text-left">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn-default btn2 " runat="server" Text="存檔" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpOVC_NOTICE_TITLE" AutoPostBack="true" OnSelectedIndexChanged="drpOVC_NOTICE_TITLE_SelectedIndexChanged" CssClass="tb tb-s" runat="server">
                                       <asp:ListItem Value="值">請選擇</asp:ListItem>
                                    </asp:DropDownList>
                                            <asp:TextBox ID="txtOVC_NOTICE_TITLE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            
                                            <asp:DropDownList ID="drpOVC_ATTACH_NAME" CssClass="tb tb-s" runat="server">
                                       <asp:ListItem Value="值">請選擇</asp:ListItem>
                                    </asp:DropDownList>
                                            <asp:TextBox ID="txtNum" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:Label ID="Label1" CssClass="control-label" runat="server">份</asp:Label>
                                            <asp:Button ID="btnAdd" OnClick="btnAdd_Click" CssClass="btn-default btn2 " runat="server" Text="加入" /><br />
                                            <asp:ListBox ID="lst" CssClass="tb tb-full" AutoPostBack="true" runat="server"></asp:ListBox>
                                        </td>
                                        <td style="width:25%">
                                            <asp:TextBox ID="txtOVC_MEMO" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="text-left">
                                            <asp:Label CssClass="text-red control-label" runat="server">國防部軍備局採購中心履約驗結處：</asp:Label><br />
                                            <asp:Label CssClass="control-label" runat="server">採購計畫未辦理修訂，應以原核定計畫清單正本移交，則以修訂後計畫清單移交</asp:Label>
                                        </td>
                                    </tr>
                                </table>

                                <br />
                                
                                <div class="text-center" style="margin-bottom:10px;">
                                    <asp:Button ID="btnReturnS" CssClass="btn-default" Text="回契約製作選擇畫面" OnClick="btnReturnS_Click" runat="server"/>
                                    <asp:Button ID="btnReturnM" CssClass="btn-default" Text="回主流程畫面" OnClick="btnReturnM_Click" runat="server"/>
                                </div>

                                <div class="subtitle">
                                    <asp:Label  CssClass="control-label" runat="server">預覽列印</asp:Label>
                                </div>
                                <div class="text-center">
                                    <asp:LinkButton ID="LinkButton1" OnClick="LinkButton1_Click" runat="server">1.契約及附件分配表</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton2" OnClick="LinkButton2_Click" runat="server">2.契約分送書函(稿)</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton3" OnClick="LinkButton3_Click" runat="server">3.契約分送會辦單</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton4" OnClick="LinkButton4_Click" runat="server">4.契約分送通知單</asp:LinkButton>
                                </div>

                            </div>
                        </div>
                        <footer class="panel-footer" style="text-align: center;">
                            <!--網頁尾-->
                        </footer></div></div>
            </section>
        </div>
    </div>
</asp:Content>
