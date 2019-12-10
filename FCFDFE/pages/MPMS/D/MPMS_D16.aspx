<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D16.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D16" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    招標作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <div class="panel-body" id="divForm" visible="false" runat="server">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="gvANNOUNCE" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="gvANNOUNCE_RowCommand" runat="server" >
         		                <Columns>
                                    <asp:TemplateField HeaderText="作業" ItemStyle-Width="13%">
                                        <ItemTemplate>
                                            <asp:Button ID="btnChange" CommandName="Change" cssclass="btn-default" Text="異動" runat="server"/>
                                            <asp:Button ID="btnDelete" CommandName="DeleteRow" cssclass="btn-warning" Text="刪除" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_PURCH_A_5" Text='<%# Eval("OVC_PURCH_A_5") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="公告日" DataField="OVC_DANNOUNCE" ItemStyle-CssClass="text-center"/>
                                    <asp:TemplateField HeaderText="開標時間" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_DOPEN_H_M" Text='<%# Eval("OVC_DOPEN_H_M") %>' runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_DOPEN" Text='<%# Eval("OVC_DOPEN") %>' Style="display:none" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_OPEN_HOUR" Text='<%# Eval("OVC_OPEN_HOUR") %>' Style="display:none" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_OPEN_MIN" Text='<%# Eval("OVC_OPEN_MIN") %>' Style="display:none" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="修正開標時間" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModOVC_DOPEN_H_M" Text='<%# Eval("OVC_DOPEN_H_M_MOD") %>' runat="server"></asp:Label>
                                            <asp:Label ID="lblmodOVC_DOPEN" Text='<%# Eval("OVC_DOPEN_N") %>' Style="display:none" runat="server"></asp:Label>
                                            <asp:Label ID="lblmodOVC_OPEN_HOUR" Text='<%# Eval("OVC_OPEN_HOUR_N") %>' Style="display:none" runat="server"></asp:Label>
                                            <asp:Label ID="lblmodOVC_OPEN_MIN" Text='<%# Eval("OVC_OPEN_MIN_N") %>' Style="display:none" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="開標次數" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblONB_TIMES" Text='<%# Eval("ONB_TIMES") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="招標方式" DataField="OVC_PUR_ASS_VEN_CODE" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="開標結果" DataField="OVC_RESULT" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="其他作業" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Button ID="btnToD16_2" CommandName="ToD16_2" cssclass="btn-default" Text="公告稿修正" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
            	                </Columns>
    	                    </asp:GridView>
                            <table class="table table-bordered">
                                <tr>
                                    <td colspan="3" style="text-align:right;Width:50%;border-right:none">
                                        <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label>
                                    </td>
                                    <td colspan="3" style="text-align:left;Width:50%;border-left: none;">
                                        <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none">
                                            <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center">
                                <asp:Button ID="btnSave" CssClass="btn-default" Text ="存檔" OnClick="btnSave_Click" runat="server" />&nbsp;&nbsp;
                                <asp:Button ID="btnNew" cssclass="btn-default" Text="新增招標資料" OnClick="btnNew_Click" runat="server"/>&nbsp;&nbsp;
                                <asp:Button ID="btnBack" CssClass="btn-default" Text="回上一頁" OnClick="btnBack_Click" runat="server"/>
                            </div>
                        </div>
                        <div>
                            <br />
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td colspan="2"><div class="subtitle">採購發包階段上傳檔案 </div></td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="td-inner-table">
                                        <asp:GridView ID="gvFiles" CssClass="table data-table table-striped border-top text-center table-inner" Width="100%" AutoGenerateColumns="False" runat="server" >
 		     		                        <Columns>
                                                <asp:HyperLinkField DataTextField="FileName" HeaderText="上傳之檔案名稱" />
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
                                    <td style="width:80%">
                                        <asp:FileUpload ID="FileUpload" title="瀏覽..." runat="server" /></td>
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
