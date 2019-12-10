<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D33.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D33" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                    購案開標紀錄分送作業               
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <table class="table text-center">
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                               <td>
                                    <asp:Label CssClass="control-label" runat="server">開標時間</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">開標地點</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" CssClass="control-label" runat="server">第</asp:Label>
                                    <asp:Label ID="lblOVC_ROOM" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="Label2" CssClass="control-label" runat="server">開標室</asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">主標人</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_CHAIRMAN" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                        </table>

                        <div class="title text-center text-red">
                            <asp:Label CssClass="control-label" runat="server">先選擇欲通知之參標廠商</asp:Label>
                        </div>
                        <div class=" text-center">
                            <asp:Label CssClass="control-label text-red" runat="server">(讀取投標廠商報價比較資料)</asp:Label>
                        </div>

                        <%--<table class="table table-bordered text-left">
                            <tr>
                                <th>
                                    <asp:Label CssClass="control-label" runat="server">選擇</asp:Label></th>
                                <th>
                                    <asp:Label CssClass="control-label text-red" runat="server">投標廠商名稱</asp:Label></th>
                                <th>
                                    <asp:Label ID="Label3" CssClass="control-label" runat="server">廠商地址</asp:Label></th>
                                <th>
                                    <asp:Label ID="Label1" CssClass="control-label" runat="server">最後通知時間</asp:Label></th>
                                <th>
                                    <asp:Label ID="Label2" CssClass="control-label" runat="server">承辦人</asp:Label></th>
                            </tr>
                        </table>--%>
                        <asp:GridView ID="GV_info" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_PURCH" OnRowCommand="GV_info_RowCommand" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="作業">
                                <ItemTemplate>
                   		    <asp:Button ID="btnMove" CssClass="btn-danger btnw2" Text="異動" CommandName="按鈕屬性" runat="server" />

                                </ItemTemplate>
                                <ItemTemplate>
                   		    <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="按鈕屬性" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="編號" DataField="ONB_NO" />
                            <asp:BoundField HeaderText="單位(名稱)" DataField="OVC_RECEIVE_UNIT" />
                            <asp:BoundField HeaderText="級職" DataField="OVC_TITLE" />
                            <asp:BoundField HeaderText="姓名" DataField="OVC_NAME" />
                            <asp:BoundField HeaderText="簽收時間" DataField="OVC_TIME" />
                            <asp:BoundField HeaderText="備考" DataField="OVC_REMARK" />
                        </Columns>
                    </asp:GridView>
                        <table class="table table-bordered text-center">
                            <tr>
                                <td style="width:30%"><asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="儲存" /><!--黃色-->
                   		            <asp:Button ID="btnClear" CssClass="btn-danger btnw2" runat="server" Text="清除" /><!--紅色-->
                    		 </td>
                                <td style="width:10%"><asp:Label CssClass="control-label" runat="server">自動編號</asp:Label></td>
                                <td style="text-align:left;width:20%"><asp:DropDownList ID="drpOVC_RECEIVE_UNIT" CssClass="tb tb-s" runat="server">
                                       <asp:ListItem Value="default">請選擇</asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    <asp:TextBox ID="txtOVC_RECEIVE_UNIT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                                <td style="width:10%"><asp:TextBox ID="txtOVC_TITLE" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                <td style="width:10%"><asp:TextBox ID="txtOVC_NAME" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                <td style="width:10%"><asp:TextBox ID="txtOVC_TIME" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                <td style="width:10%"><asp:TextBox ID="txtOVC_REMARK" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                            </tr>
                            </table>
                        <div class="text-left">
                            <asp:Label ID="Label3" CssClass="control-label" runat="server">註：</asp:Label><br />
                            <asp:Label ID="Label4" CssClass="control-label" runat="server">一、以空心章蓋於紀錄文件正中央，以利識別。</asp:Label><br />
                            <asp:Label ID="Label5" CssClass="control-label" runat="server">二、如得標廠商有一家以上情形者，本表可自行延伸級增列編號。</asp:Label>
                        </div>
                        <br />
                        <div class="text-center">
                            <asp:Button ID="btnReturnR" CssClass="btn-warning " runat="server" Text="回開標紀錄畫面" />
                            <asp:Button ID="btnReturnM" CssClass="btn-warning " runat="server" Text="回主流程畫面" />
                            <a href="#" style="text-decoration:underline; color:red; font-size:18px;">開標記錄分送登記表預覽列印</a>
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
