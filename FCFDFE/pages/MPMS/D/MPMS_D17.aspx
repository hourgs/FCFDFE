<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D17.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D17" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    疑義、異議
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <div class="subtitle" style="text-align:center;"><asp:Label ID="lblOVC_PURCH_A_5" runat="server"></asp:Label></div>
                <div class="panel-body" id="divForm" visible="false" runat="server">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <!--頁籤－開始-->
                            <header class="panel-heading">
                                <ul class="nav nav-tabs" ID="myTabs">
                                    <!--各頁籤-->
                                    <li ID="page1" class="active">
                                        <a data-toggle="tab" href="#TabPage1">疑義電傳表</a>
                                    </li>
                                    <li ID="page2">
                                        <a data-toggle="tab" href="#TabPage2">異議電傳表</a>
                                    </li>
                                    <li ID="page3">
                                        <a data-toggle="tab" href="#TabPage3">傳真申請表</a>
                                    </li>
                                </ul>
                            </header>
                            <div class="panel-body tab-body">
                                <div class="tab-content">
                                    <!--各標籤之頁面-->
                                    <div id="TabPage1" class="tab-pane active"><!--起始選取頁面-->
                                    <!-- 疑義電傳表  -->
                                        <table class="table table-bordered text-center">
                                            <tr><td>請選擇疑義電傳表內容</td></tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="page1_Select1" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                        <asp:ListItem>【該公司所陳雖已逾疑義期限，仍請針對疑義事項逐條提供釋疑（如表一）】</asp:ListItem>
                                                        <asp:ListItem>【請即針對疑義事項逐條提供釋疑（如表一）】</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:CheckBox ID="page1_Select2" Text="如逾時未回覆，本室將逕依政府採購法第48條第1項第1款規定，不予開標。" Checked="true" Visible="false" CssClass="radioButton" runat="server" />
                                                    <asp:CheckBox ID="page1_Select3" Text="五、如無法依前述說明二至四時限內完成澄復及修訂，請先行函文或傳真查告後續處理原則(如不予開標)。" Checked="true" Visible="false" CssClass="radioButton" runat="server" />
                                                </td>
                                            </tr>
                                        </table>

                                        <asp:GridView ID="gv" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="gv_RowCommand" runat="server" >
         		                            <Columns>
                                                <asp:TemplateField HeaderText="開標時間">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_DOPEN_H_M" Text='<%# Eval("OVC_DOPEN_H_M") %>' CssClass="control-label" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_DOPEN" Text='<%# Eval("OVC_DOPEN") %>' Style="display:none" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_OPEN_HOUR" Text='<%# Eval("OVC_OPEN_HOUR") %>' Style="display:none" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_OPEN_MIN" Text='<%# Eval("OVC_OPEN_MIN") %>' Style="display:none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="開標次數">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblONB_TIMES" Text='<%# Eval("ONB_TIMES") %>' CssClass="control-label" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="輸出Word檔">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnWordD17_1" CommandName="WordD17_1" cssclass="btn-default" Text="疑義電傳表.doc" runat="server"/>
                                                        <asp:Button ID="btnWordD17_1_pdf" CommandName="WordD17_1_pdf" cssclass="btn-default" Text=".pdf" runat="server"/>
                                                        <asp:Button ID="btnWordD17_1_odt" CommandName="WordD17_1_odt" cssclass="btn-default" Text=".odt" runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
            	                            </Columns>
    	                                </asp:GridView>
                                    </div>



                                    <div id="TabPage2" class="tab-pane">
                                    <!-- 異議電傳表  -->
                                        <table class="table table-bordered text-center">
                                            <tr><td>請選擇異議電傳表內容</td></tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="page2_Select1" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                        <asp:ListItem>【該公司所陳雖已逾異議期限，仍請針對異議事項逐條提供處理結果（如表一）】</asp:ListItem>
                                                        <asp:ListItem>【請即針對異議事項逐條提供處理結果（如表一）】</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:CheckBox ID="page2_Select2" Text="如逾時未回覆，本室將逕依政府採購法第48條第1項第1款規定，不予開標(建議載明)。" Checked="true" Visible="false" CssClass="radioButton" runat="server" />
                                                    <asp:CheckBox ID="page2_Select3" Text="五、如無法依前述說明二至四時限內完成澄復及修訂，請先行函文或傳真查告後續處理原則(如不予開標)。" Checked="true" Visible="false" CssClass="radioButton" runat="server" />
                                                </td>
                                            </tr>
                                        </table>

                                        <asp:GridView ID="gv2" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="gv_RowCommand" runat="server" >
         		                            <Columns>
                                                <asp:TemplateField HeaderText="開標時間">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_DOPEN_H_M" Text='<%# Eval("OVC_DOPEN_H_M") %>' CssClass="control-label" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_DOPEN" Text='<%# Eval("OVC_DOPEN") %>' Style="display:none" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_OPEN_HOUR" Text='<%# Eval("OVC_OPEN_HOUR") %>' Style="display:none" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_OPEN_MIN" Text='<%# Eval("OVC_OPEN_MIN") %>' Style="display:none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="開標次數">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblONB_TIMES" Text='<%# Eval("ONB_TIMES") %>' CssClass="control-label" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="輸出Word檔">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnWordD17_2" CommandName="WordD17_2" cssclass="btn-default" Text="異議電傳表.doc" runat="server"/>
                                                        <asp:Button ID="btnWordD17_2_pdf" CommandName="WordD17_2_pdf" cssclass="btn-default" Text=".pdf" runat="server"/>
                                                        <asp:Button ID="btnWordD17_2_odt" CommandName="WordD17_2_odt" cssclass="btn-default" Text=".odt" runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
            	                            </Columns>
    	                                </asp:GridView>
                                    </div>
                                    
                                    
                                    
                                    
                                    <div id="TabPage3" class="tab-pane">
                                    <!-- 傳真申請表 -->
                                        <asp:GridView ID="gv3" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="gv_RowCommand" runat="server" >
         		                            <Columns>
                                                <asp:TemplateField HeaderText="開標時間" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_DOPEN_H_M" Text='<%# Eval("OVC_DOPEN_H_M") %>' CssClass="control-label" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_DOPEN" Text='<%# Eval("OVC_DOPEN") %>' Style="display:none" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_OPEN_HOUR" Text='<%# Eval("OVC_OPEN_HOUR") %>' Style="display:none" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_OPEN_MIN" Text='<%# Eval("OVC_OPEN_MIN") %>' Style="display:none" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="開標次數">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblONB_TIMES" Text='<%# Eval("ONB_TIMES") %>' CssClass="control-label" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="輸出Word檔">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnWordD17_3" CommandName="WordD17_3" cssclass="btn-default" Text="傳真申請表.doc" runat="server"/>
                                                        <asp:Button ID="btnWordD17_3_pdf" CommandName="WordD17_3_pdf" cssclass="btn-default" Text=".pdf" runat="server"/>
                                                        <asp:Button ID="btnWordD17_3_odt" CommandName="WordD17_3_odt" cssclass="btn-default" Text=".odt" runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
            	                            </Columns>
    	                                </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--頁籤－結束-->

                        <div class="subtitle">廠商資料暨處理情形(上傳功能) </div>
                        <p><asp:Label CssClass="control-label" runat="server">疑義檔案總數：</asp:Label>
                        <asp:Label ID="lblTotalFile" CssClass="control-label" runat="server"></asp:Label></p>
                        <p><asp:Label CssClass="control-label" runat="server">異議檔案總數：</asp:Label>
                        <asp:Label ID="lblTotalFile_2" CssClass="control-label" runat="server"></asp:Label></p>
                        
                        <table class="table table-bordered text-center">
                            <%--疑義--%>
                            <tr>
                                <td colspan="2"><asp:Label CssClass="control-label" Font-Size="Large" runat="server">疑義</asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="td-inner-table">
                                    <asp:GridView ID="gvFiles" CssClass="table data-table table-striped border-top text-center table-inner" AutoGenerateColumns="False" OnRowCommand="gvFiles_RowCommand" runat="server" >
 		     		                    <Columns>
                                            <asp:TemplateField HeaderText="作業">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDelete" cssclass="btn-default" Text="刪除" CommandName="DeleteFile" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server"/>
                                                    <asp:Label ID="lblFileName" Text='<%# Eval("FileName") %>' Style="display:none" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:HyperLinkField DataTextField="LinkFileName" HeaderText="上傳之檔案名稱" />
                                            <asp:BoundField DataField="Time" HeaderText="時間" />     
                                              <asp:TemplateField HeaderText="檔案大小">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("FileSize") %>' runat="server"></asp:Label> KB
                                                </ItemTemplate>
                                            </asp:TemplateField>
                	                    </Columns>
	   		                        </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td><asp:Button ID="btnUpload" CssClass="btn-default btnw2" Text="上傳" OnClick="btnUpload_Click" runat="server"/></td>
                                <td style="width:80%"><asp:FileUpload ID="FileUpload" title="瀏覽..." runat="server" /></td>
                            </tr>
                            <tr><td colspan="2"></td></tr>
                            <%--異議--%>
                            <tr>
                                <td colspan="2"><asp:Label CssClass="control-label" Font-Size="Large" runat="server">異議</asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="td-inner-table">
                                    <asp:GridView ID="gvFiles_2" CssClass="table data-table table-striped border-top text-center table-inner" AutoGenerateColumns="False" OnRowCommand="gvFiles_2_RowCommand" runat="server" >
 		     		                    <Columns>
                                            <asp:TemplateField HeaderText="作業">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDelete" cssclass="btn-default" Text="刪除" CommandName="DeleteFile" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server"/>
                                                    <asp:Label ID="lblFileName" Text='<%# Eval("FileName") %>' Style="display:none" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:HyperLinkField DataTextField="LinkFileName" HeaderText="上傳之檔案名稱" />
                                            <asp:BoundField DataField="Time" HeaderText="時間" />     
                                              <asp:TemplateField HeaderText="檔案大小">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("FileSize") %>' runat="server"></asp:Label> KB
                                                </ItemTemplate>
                                            </asp:TemplateField>
                	                    </Columns>
	   		                        </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td><asp:Button ID="btnUpload_2" CssClass="btn-default btnw2" Text="上傳" OnClick="btnUpload_2_Click" runat="server"/></td>
                                <td style="width:80%"><asp:FileUpload ID="FileUpload_2" title="瀏覽..." runat="server" /></td>
                            </tr>
                        </table>

                        <div style="text-align:center">
                            <asp:Button ID="btnBack" CssClass="btn-default" Text="回上一頁" OnClick="btnBack_Click" runat="server"/>
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
