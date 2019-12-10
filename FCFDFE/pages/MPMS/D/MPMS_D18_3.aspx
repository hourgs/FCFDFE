<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D18_3.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D18_3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title"><!--標題-->
                    截標審查作業編輯(廠商編輯)
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->

                <div class="panel-body" style=" border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered" style="text-align:center">
                                <tr>
                                    <td style="width:10%"><asp:Label CssClass="control-label" runat="server" Text="購案編號"></asp:Label></td>
                                    <td style="width:40%" class="text-left">
                                        <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_PURCH" Style="display:none" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_PUR_AGENCY" Style="display:none" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_PURCH_5" Style="display:none"  CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td style="width:10%"><asp:Label CssClass="control-label" runat="server" Text="購案名稱"></asp:Label></td>
                                    <td style="width:40%" class="text-left"><asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="開標時間"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>&nbsp;
                                        <asp:Label ID="lblOVC_OPEN_HOUR" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                        <asp:Label ID="lblOVC_OPEN_MIN" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">分 (第</asp:Label>
                                        <asp:Label ID="lblONB_TIMES" CssClass="control-label text-red" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">次) </asp:Label>
                                        <asp:Label CssClass="control-label text-red" runat="server">組別：</asp:Label>
                                        <asp:Label ID="lblONB_GROUP" CssClass="control-label text-red" runat="server"></asp:Label>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="投標段次"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="決標原則"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_BID_METHOD_1" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="，"></asp:Label>
                                        <asp:Label ID="lblOVC_BID_METHOD_2" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="，"></asp:Label>
                                        <asp:Label ID="lblOVC_BID_METHOD_3" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="招標方式"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">(原：</asp:Label>
                                        <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">)　</asp:Label>
                                        <asp:Label CssClass="control-label text-red" runat="server">簽辦：</asp:Label>
                                        <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE_Sign" CssClass="control-label text-red" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" Text="公告日期"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_DANNOUNCE" runat="server"></asp:Label></td>
                                    <td><asp:Label runat="server" Text="投標期限"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_DBID_LIMIT_H_M" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table><br>

                            
                            <!--頁籤－開始-->
                                <header class="panel-heading">
                                    <ul class="nav nav-tabs">
                                        <!--各頁籤-->
                                        <li class="active"><!--起始選取頁籤，只有一個-->
                                            <a data-toggle="tab" href="#TabSite">現場投標廠商編輯</a>
                                        </li>
                                        <li class=""><!--尚未選取頁籤-->
                                            <a data-toggle="tab" href="#TabCommunication">通信投標廠商編輯</a>
                                        </li>
                                        <li class=""><!--尚未選取頁籤-->
                                            <a data-toggle="tab" href="#TabElectronic">電子投標廠商編輯</a>
                                        </li>
                                    </ul>
                                </header>

                                <div class="panel-body tab-body">
                                    <div class="tab-content">
                                        <!--各標籤之頁面-->
                                        <div id="TabSite" class="tab-pane active"><!--起始選取頁面，只有一個-->
                                            <!-- 現場投標廠商編輯 -->
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvTBM1313_Site" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="gvTBM1313_RowCommand" runat="server">
 		     		                                    <Columns>
                                                            <asp:TemplateField HeaderText="作業" ItemStyle-Width="16%">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnModify" Text="異動" CommandName="Modify_Site" CssClass="btn-default btnw2" runat="server" />
                                                                    <asp:Button ID="btnDel" Text="刪除" CommandName="Delete_Site" CssClass="btn-warning btnw2" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="統一編號" ItemStyle-Width="14%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_CST" Text='<%# Bind("OVC_VEN_CST") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="廠商名稱" ItemStyle-Width="16%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_TITLE" Text='<%# Bind("OVC_VEN_TITLE") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="廠商電話" DataField="OVC_VEN_TEL" ItemStyle-CssClass="text-center" ItemStyle-Width="14%"/>
                                                            <asp:TemplateField HeaderText="投標時間" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_DBID" Text='<%# Bind("OVC_DBID") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblOVC_BID_HOUR" Text='<%# Bind("OVC_BID_HOUR") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                                                    <asp:Label ID="lblOVC_BID_MIN" Text='<%# Bind("OVC_BID_MIN") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" runat="server">分</asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="審查" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" Text="編輯" CommandName="Edit_Site" CssClass="btn-default btnw2" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="審查表">
                                                                <ItemTemplate>
                                                                    <p><asp:Button ID="btnWordD18_3_1" Text="公開招標一次投、不分段開標.doc" CommandName="WordD18_3_1" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_1_odt" Text="公開招標一次投、不分段開標.odt" CommandName="WordD18_3_1_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_2" Text="公開一次投分段開標.doc" CommandName="WordD18_3_2" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_2_odt" Text=".odt" CommandName="WordD18_3_2_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_3" Text="公開分段投標.doc" CommandName="WordD18_3_3" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_3_odt" Text=".odt" CommandName="WordD18_3_3_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_4" Text="選擇性招標投標.doc" CommandName="WordD18_3_4" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_4_odt" Text=".odt" CommandName="WordD18_3_4_odt" CssClass="btn-default" runat="server" /></p>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                	                                    </Columns>
	   		                                        </asp:GridView>

                                                    <table class="table table-bordered text-center">
                                                        <tr>
                                                            <td>
                                                                <asp:Label Text="作業" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="統一編號" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="廠商名稱" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="廠商電話" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="投標時間" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:16%">
                                                                <asp:Button ID="btnSave_Site" Text="存檔" OnClick="btnSaveVendor_Click" CssClass="btn-default btnw2" runat="server" />
                                                                <asp:Button ID="btnClear_Site" Text="清除" OnClick="btnClear_Click" CssClass="btn-default btnw2" runat="server" /><br><br>
                                                                <asp:Button ID="btnSaveAndInsert_Site" Text="存檔並寫入廠商檔" OnClick="btnSaveAndInsert_Click" CssClass="btn-default" runat="server" />
                                                            </td>
                                                            <td style="width:14%">
                                                                <asp:DropDownList ID="ddlOVC_VEN_CST_Site" OnSelectedIndexChanged="ddlOVC_VEN_CST_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-s" runat="server"></asp:DropDownList><br />
                                                                <asp:TextBox ID="txtOVC_VEN_CST_Site" CssClass="tb tb-s" runat="server"></asp:TextBox><br />
                                                                <asp:Button ID="btnSearch_Site" CssClass="btn-default" Text="廠商查詢" OnClick="btnOVC_VEN_CST_Click" runat="server" />
                                                            </td>
                                                            <td style="width:16%">
                                                                <asp:TextBox ID="txtOVC_VEN_TITLE_Site" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                            <td style="width:14%">
                                                                <asp:TextBox ID="txtOVC_VEN_TEL_Site" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                            <td colspan="2" style="width:40%">
                                                                <div class="input-append datepicker" >
                                                                    <asp:TextBox ID="txtOVC_DBID_Site" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                                <asp:TextBox ID="txtOVC_BID_HOUR_Site" CssClass='tb tb-xs position-left' runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="position-left" runat="server">時</asp:Label>
                                                                <asp:TextBox ID="txtOVC_BID_MIN_Site" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="position-left" runat="server">分</asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>    
                                                
                                            </asp:UpdatePanel>
                                        </div>


                                        <div id="TabCommunication" class="tab-pane"><!--尚未選取頁面-->
                                            <!-- 通信投標廠商編輯 -->
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvTBM1313_Comm" OnRowCommand="gvTBM1313_RowCommand" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
 		     		                                    <Columns>
                                                            <asp:TemplateField HeaderText="作業" ItemStyle-Width="16%">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnModify" CommandName="Modify_Comm" CssClass="btn-default btnw2" Text="異動" runat="server" />
                                                                    <asp:Button ID="btnDel" CommandName="Delete_Comm" CssClass="btn-warning btnw2" Text="刪除" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="統一編號" ItemStyle-Width="14%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_CST" Text='<%# Bind("OVC_VEN_CST") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="廠商名稱" ItemStyle-Width="16%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_TITLE" Text='<%# Bind("OVC_VEN_TITLE") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="廠商電話" DataField="OVC_VEN_TEL" ItemStyle-CssClass="text-center" ItemStyle-Width="14%" />
                                                            <asp:TemplateField HeaderText="投標時間" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_DBID" Text='<%# Bind("OVC_DBID") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblOVC_BID_HOUR" Text='<%# Bind("OVC_BID_HOUR") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                                                    <asp:Label ID="lblOVC_BID_MIN" Text='<%# Bind("OVC_BID_MIN") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" runat="server">分</asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="審查" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" Text="編輯" CommandName="Edit_Comm" CssClass="btn-default btnw2" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="審查表">
                                                                <ItemTemplate>
                                                                    <p><asp:Button ID="btnWordD18_3_1" Text="公開招標一次投、不分段開標.doc" CommandName="WordD18_3_1" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_1_odt" Text="公開招標一次投、不分段開標.odt" CommandName="WordD18_3_1_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_2" Text="公開一次投分段開標.doc" CommandName="WordD18_3_2" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_2_odt" Text=".odt" CommandName="WordD18_3_2_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_3" Text="公開分段投標.doc" CommandName="WordD18_3_3" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_3_odt" Text=".odt" CommandName="WordD18_3_3_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_4" Text="選擇性招標投標.doc" CommandName="WordD18_3_4" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_4_odt" Text=".odt" CommandName="WordD18_3_4_odt" CssClass="btn-default" runat="server" /></p>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                	                                    </Columns>
	   		                                        </asp:GridView>

                                                    <table class="table table-bordered text-center">
                                                        <tr>
                                                            <td>
                                                                <asp:Label Text="作業" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="統一編號" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="廠商名稱" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="廠商電話" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="投標時間" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:16%">
                                                                <asp:Button ID="btnSave_Comm" Text="存檔" OnClick="btnSaveVendor_Click" CssClass="btn-default btnw2" runat="server" />
                                                                <asp:Button ID="btnClear_Comm" Text="清除" OnClick="btnClear_Click" CssClass="btn-default btnw2" runat="server" /><br><br>
                                                                <asp:Button ID="btnSaveAndInsert_Comm" Text="存檔並寫入廠商檔" OnClick="btnSaveAndInsert_Click" CssClass="btn-default" runat="server"  />
                                                            </td>
                                                            <td style="width:14%">
                                                                <asp:DropDownList ID="ddlOVC_VEN_CST_Comm" OnSelectedIndexChanged="ddlOVC_VEN_CST_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                                <asp:TextBox ID="txtOVC_VEN_CST_Comm" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                                <asp:Button ID="btnSearch_Comm" CssClass="btn-default" Text="廠商查詢" OnClick="btnOVC_VEN_CST_Click" runat="server" />
                                                            </td>
                                                            <td style="width:16%">
                                                                <asp:TextBox ID="txtOVC_VEN_TITLE_Comm" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                            <td style="width:14%">
                                                                <asp:TextBox ID="txtOVC_VEN_TEL_Comm" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                            <td style="width:40%" colspan="2">
                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtOVC_DBID_Comm" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                                <asp:TextBox ID="txtOVC_BID_HOUR_Comm" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="position-left" runat="server">時</asp:Label>
                                                                <asp:TextBox ID="txtOVC_BID_MIN_Comm" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="position-left" runat="server">分</asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>


                                        <div id="TabElectronic" class="tab-pane"><!--尚未選取頁面-->
                                            <!-- 電子投標廠商編輯 -->
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvTBM1313_Elec" OnRowCommand="gvTBM1313_RowCommand" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
 		     		                                    <Columns>
                                                            <asp:TemplateField HeaderText="作業" ItemStyle-Width="16%">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnModify" CommandName="Modify_Elec" CssClass="btn-default btnw2" Text="異動" runat="server" />
                                                                    <asp:Button ID="btnDel" CommandName="Delete_Elec" CssClass="btn-warning btnw2" Text="刪除" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="統一編號" ItemStyle-Width="14%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_CST" Text='<%# Bind("OVC_VEN_CST") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="廠商名稱" ItemStyle-Width="16%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_TITLE" Text='<%# Bind("OVC_VEN_TITLE") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="廠商電話" DataField="OVC_VEN_TEL" ItemStyle-CssClass="text-center" ItemStyle-Width="14%"/>
                                                            <asp:TemplateField HeaderText="投標時間" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_DBID" Text='<%# Bind("OVC_DBID") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblOVC_BID_HOUR" Text='<%# Bind("OVC_BID_HOUR") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                                                    <asp:Label ID="lblOVC_BID_MIN" Text='<%# Bind("OVC_BID_MIN") %>'  CssClass="control-label" runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" runat="server">分</asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="審查" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" Text="編輯" CommandName="Edit_Elec" CssClass="btn-default btnw2" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="審查表">
                                                                <ItemTemplate>
                                                                    <p><asp:Button ID="btnWordD18_3_1" Text="公開招標一次投、不分段開標.doc" CommandName="WordD18_3_1" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_1_odt" Text="公開招標一次投、不分段開標.odt" CommandName="WordD18_3_1_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_2" Text="公開一次投分段開標.doc" CommandName="WordD18_3_2" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_2_odt" Text=".odt" CommandName="WordD18_3_2_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_3" Text="公開分段投標.doc" CommandName="WordD18_3_3" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_3_odt" Text=".odt" CommandName="WordD18_3_3_odt" CssClass="btn-default" runat="server" /></p>
                                                                    <p><asp:Button ID="btnWordD18_3_4" Text="選擇性招標投標.doc" CommandName="WordD18_3_4" CssClass="btn-default" runat="server" />
                                                                    <asp:Button ID="btnWordD18_3_4_odt" Text=".odt" CommandName="WordD18_3_4_odt" CssClass="btn-default" runat="server" /></p>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                	                                    </Columns>
	   		                                        </asp:GridView>

                                                    <table class="table table-bordered text-center">
                                                        <tr>
                                                            <td>
                                                                <asp:Label Text="作業" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="統一編號" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="廠商名稱" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="廠商電話" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="投標時間" CssClass="control-label" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:16%">
                                                                <asp:Button ID="btnSave_Elec" Text="存檔" OnClick="btnSaveVendor_Click" CssClass="btn-default btnw2" runat="server" />
                                                                <asp:Button ID="btnClear_Elec" Text="清除" OnClick="btnClear_Click" CssClass="btn-default btnw2" runat="server" /><br><br>
                                                                <asp:Button ID="btnSaveAndInsert_Elec" Text="存檔並寫入廠商檔" OnClick="btnSaveAndInsert_Click" CssClass="btn-default" runat="server" />
                                                            </td>
                                                            <td style="width:14%">
                                                                <asp:DropDownList ID="ddlOVC_VEN_CST_Elec" OnSelectedIndexChanged="ddlOVC_VEN_CST_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                                <asp:TextBox ID="txtOVC_VEN_CST_Elec" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                                <asp:Button ID="btnSearch_Elec" CssClass="btn-default" Text="廠商查詢" OnClick="btnOVC_VEN_CST_Click" runat="server" />
                                                            </td>
                                                            <td style="width:16%">
                                                                <asp:TextBox ID="txtOVC_VEN_TITLE_Elec" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                            <td style="width:14%">
                                                                <asp:TextBox ID="txtOVC_VEN_TEL_Elec" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                            <td style="width:40%" colspan="2">
                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtOVC_DBID_Elec" CssClass="tb tb-s position-left"  runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                                <asp:TextBox ID="txtOVC_BID_HOUR_Elec" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="position-left" runat="server">時</asp:Label>
                                                                <asp:TextBox ID="txtOVC_BID_MIN_Elec" CssClass="tb tb-xs position-left" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="position-left" runat="server">分</asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            <!--頁籤－結束-->

                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:50%" class="text-right"><asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none">
                                            <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center">
                                <asp:Button ID="btnSave" Text="存檔" OnClick="btnSave_Click" CssClass="btn-default btnw2" runat="server" />
                                <asp:Button ID="btnReturn" CssClass="btn-default" OnClick="btnReturn_Click" Text="回開標紀錄作業編輯" runat="server" />
                                <asp:Button ID="btnReturnR" CssClass="btn-default" OnClick="btnReturnR_Click" Text="回開標紀錄選擇畫面" runat="server" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default" OnClick="btnReturnM_Click" Text="回主流程畫面" runat="server" />
                            </div>
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

