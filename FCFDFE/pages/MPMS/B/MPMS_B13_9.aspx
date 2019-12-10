<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13_9.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13_9" %>
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
                    <!--標題-->採購計畫清單隨案檢附草約編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-red" runat="server" Text="購案編號："></asp:Label>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label text-red" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-red" runat="server" Text="作業"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="內容"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         <asp:Button ID="btn_Del" CssClass="btn-success btnw4" Text="刪除草約" OnClick="btn_Del_Click" runat="server"/>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMemo" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-warning" runat="server" OnClick="btnReturn_Click" Text="回物資申請書編制作業" />
                            </div>
                            <p></p>
                            <table class="table table-bordered">
                               <tr>
                                    <td>
                                       <asp:Label CssClass="control-label" runat="server" Text="作業"></asp:Label>
                                    </td>
                                   <td>
                                       <asp:Label CssClass="control-label" runat="server" Text="內容"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_Save_1" CssClass="btn-success btnw4" OnClick="btn_Save_1_Click" Text="選擇存檔" runat="server"/>
                                    </td>
                                    <td>
                                        本案
                                        <asp:DropDownList ID="drpSuit_1" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>適用</asp:ListItem>
                                            <asp:ListItem>不適用</asp:ListItem>
                                        </asp:DropDownList>
                                         「
                                        <asp:TextBox ID="txtAgency_1" CssClass="tb tb-m" runat="server">非軍事機關(自行輸入)</asp:TextBox>
                                        【招標單位】內購財務，勞務、資訊服務採購契約通用條款。」
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Button ID="btn_Save_2" CssClass="btn-success btnw4" OnClick="btn_Save_2_Click" Text="選擇存檔" runat="server"/>
                                    </td>
                                    <td>
                                        隨案檢附
                                        <asp:DropDownList ID="drpContract_1" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>契約條款</asp:ListItem>
                                        </asp:DropDownList>
                                         乙份（共
                                        <asp:TextBox ID="txtPage_1" CssClass="tb tb-xs" runat="server">1</asp:TextBox>
                                        頁。）
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Button ID="btn_Save_3" CssClass="btn-success btnw4" OnClick="btn_Save_3_Click" Text="選擇存檔" runat="server"/>
                                    </td>
                                    <td>
                                       本案
                                        <asp:DropDownList ID="drpSuit_2" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>適用</asp:ListItem>
                                            <asp:ListItem>不適用</asp:ListItem>
                                        </asp:DropDownList>
                                         「
                                        <asp:TextBox ID="txtAgency_2" CssClass="tb tb-m" runat="server">非軍事機關(自行輸入)</asp:TextBox>
                                        【招標單位】內購財務，勞務、資訊服務採購契約通用條款」，並檢附
                                        <asp:DropDownList ID="drpContract_2" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>契約條款</asp:ListItem>
                                        </asp:DropDownList>
                                        乙份（共
                                        <asp:TextBox ID="txtPage_2" CssClass="tb tb-xs" runat="server">1</asp:TextBox>
                                        頁）。
                                    </td>
                                </tr>
                            </table>
                            
                            <div class="text-center">
                                <asp:Label runat="server" CssClass="control-label text-red" Text="編輯區(如果在標準片語找不到您要的用語請先選擇種類後再在此編輯)"></asp:Label>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Button ID="btnEdit_Save" CssClass="btn-success btnw2" OnClick="btnEdit_Save_Click" Text="存檔" runat="server" /></td>
                                    <td><asp:TextBox ID="txtNewOVC_MEMO" CssClass="tb tb-full"  runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                            <p></p>
                            <table class="table table-bordered">
                                <tr>
                                    <th colspan="2" style="font-size: 24px; color:red">
                                        通用條款下載
                                    </th>
                                </tr>
                                <tr style="text-align:center">
                                    <td>
                                        文件名稱
                                    </td>
                                    <td>
                                        文件下載連結
                                    </td>
                                </tr>
                               <tr>
                                    <td>
                                        財務採購契約修訂草案
                                    </td>
                                    <td>
                                        財務採購契約修訂草案(1041224令發)--限制編輯1050427.doc
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        勞務採購契約修訂草案
                                    </td>
                                    <td>
                                        勞務採購契約修訂草案(1041224令發)--限制編輯1050427.doc
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        資訊服務採購契約通用條款
                                    </td>
                                    <td>
                                        資訊服務採購契約通用條款(1041224令發)--限制編輯1050427.doc
                                    </td>
                                </tr>
                            </table>
                            
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
