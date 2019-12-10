<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D23.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D23" %>
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
                    <!--標題-->契約製作編輯─移履約
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <!--頁籤－開始-->
                                <header class="panel-heading">
                                    <ul class="nav nav-tabs">
                                        <!--各頁籤-->
                                        <li class="active"><!--起始選取頁籤，只有一個-->
                                            <a data-toggle="tab" href="#TabStep1">步驟1</a>
                                        </li>
                                        <li class=""><!--尚未選取頁籤-->
                                            <a data-toggle="tab" href="#TabStep2">步驟2</a>
                                        </li>
                                        <li class=""><!--尚未選取頁籤-->
                                            <a data-toggle="tab" href="#TabStep3">步驟3</a>
                                        </li>
                                    </ul>
                                </header>
                                <div class="panel-body tab-body">
                                    <div class="tab-content">
                                        <!--各標籤之頁面-->
                                        <div id="TabStep1" class="tab-pane active"><!--起始選取頁面，只有一個-->
                                            <asp:Label CssClass="subtitle text-red" runat="server" Text="契約製作"></asp:Label><br><br>
                                            <div class="text-center">
                                                <asp:Label CssClass="subtitle" runat="server" Text="契約草稿、明細及單價等作業"></asp:Label>
                                            </div>
                                            <asp:GridView ID="GV_ContractDraft" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
 		     		                            <Columns>
                                                    <asp:TemplateField HeaderText="作業">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnChange" CssClass="btn-success btnw2" runat="server" Text="異動" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="購案編號" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="合約編號" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="組別" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="得標商名稱" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="電話" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="簽約日(已移履約)" DataField="" ItemStyle-CssClass="text-center" />
                	                            </Columns>
	   		                                </asp:GridView>
                                            <div class="text-center">
                                                <div>
                                                <asp:Label CssClass="subtitle" runat="server" Text="請選擇已簽辦之開標紀錄及開標結果"></asp:Label>
                                                </div>
                                                <asp:GridView ID="DV_TBM1303" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
 		     		                            <Columns>
                                                    <asp:TemplateField HeaderText="作業">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnNew" CssClass="btn-success btnw2" runat="server" Text="新增" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="開標日" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="開標次數" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="組別" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="開標結果" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="決標日" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="主官核批日" DataField="" ItemStyle-CssClass="text-center" />
                	                            </Columns>
	   		                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div id="TabStep2" class="tab-pane"><!--尚未選取頁面-->
                                            <asp:Label CssClass="subtitle text-red" runat="server" Text="合約移履約"></asp:Label><br><br>
                                            <div class="text-center">
                                                <asp:Label CssClass="subtitle" runat="server" Text="請勾選(單筆多筆)可移履約驗結單位之合約，再按[移履約]！"></asp:Label>
                                            </div>
                                            <asp:GridView ID="GV_TBM1302" CssClass=" table data-table table-striped border-top  text-center" AutoGenerateColumns="false" runat="server">
 		     		                            <Columns>
                                                    <asp:TemplateField HeaderText="勾選">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server"/>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="履約單位收案狀況" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="履約單位(由計評單位核定)" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="購案編號" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="合約編號" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="組別" DataField="" ItemStyle-CssClass="text-center" />
                                                    <asp:BoundField HeaderText="得標商名稱" DataField="" ItemStyle-CssClass="text-center" />
                	                            </Columns>
	   		                                </asp:GridView>
                                            <div class="text-center">
                                                <asp:Button ID="btnCheckAll" CssClass="btn-success btnw4" runat="server" Text="勾選全部" />
                                                <asp:Button ID="btnResetAll" CssClass="btn-default btnw4" runat="server" Text="清除全部" />
                                                <asp:Button ID="btnTransfer" CssClass="btn-success btnw4" runat="server" Text="移履約" />
                                            </div>
                                        </div>
                                        <div id="TabStep3" class="tab-pane"><!--尚未選取頁面-->
                                            <asp:Label CssClass="subtitle text-red" runat="server" Text="輸入全案(案號："></asp:Label>
                                            <asp:Label ID="lblOVC_PURCH_5" CssClass="subtitle text-red" runat="server"></asp:Label>
                                            <asp:Label CssClass="subtitle text-red" runat="server" Text=")採購發包階段結束日"></asp:Label>
                                            <asp:Label CssClass="subtitle text-blue" runat="server" Text="(所有合約皆已移至履約單位才可以輸入)"></asp:Label><br><br>

                                            <table class="table table-bordered text-center">
                                                <tr>
                                                    <td style="width:50%" class="text-right no-bordered" colspan="2"><asp:Label CssClass="control-label" runat="server">購案階段結束日</asp:Label></td>
                                                    <td colspan="2" class="no-bordered"><div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                    <asp:TextBox ID="txtOVC_DEND" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                    </div></td>
                                                </tr>
                                            </table>
                                            <table class="table no-bordered-seesaw text-center">
                                                <tr class="no-bordered-seesaw">
                                                    <td style="width:15%" class="no-bordered"></td>
                                                    <td class="text-left no-bordered">
                                                        <asp:Label CssClass="control-label subtitle" runat="server" Text="註1：結束日存檔成功後，購案不再於購案系統中顯示(除非合約經履驗單位退案)！"></asp:Label><br><br>
                                                        <asp:Label CssClass="control-label subtitle" runat="server" Text="註2：請確定步驟2須移送之合約，已顯示[已於年、月、日移履約]，再輸入結束日！"></asp:Label><br><br>
                                                        <asp:Label CssClass="control-label subtitle" runat="server" Text="註3：若合約曾經呂彥單位退案，重新移送該筆合約後，請再次輸入結束日！"></asp:Label><br><br>
                                                        <asp:Label CssClass="control-label subtitle" runat="server" Text="註4：合約已建立者原則上不可刪除(合約為履驗系統資料來源)，未移履約前如需刪除，"></asp:Label><br>
                                                        <asp:Label CssClass="control-label subtitle" runat="server" Text="請至開標紀錄第二頁刪除得標廠商對應之契約尾號即可，唯該筆合約須重新製作！"></asp:Label>
                                                    </td>
                                                    <td style="width:15%" class="no-bordered"></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            <!--頁籤－結束-->

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
