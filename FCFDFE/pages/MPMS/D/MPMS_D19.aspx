<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D19.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D19" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    購案開標紀錄作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <asp:GridView ID="gvTbm1303" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="gvTbm1303_RowCommand" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="編號" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNUM" Text='<%# Container.DisplayIndex + 1 %>' CssClass="control-label" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="作業" ItemStyle-Width="24%">
                                <ItemTemplate>
                                    <p><asp:Button ID="btnEnter" CssClass="btn-default btnw2" Text="作業" CommandName="Enter" runat="server" /></p>
                                    <div visible='<%# Container.DisplayIndex + 1 == 1 ? false : true %>' runat="server">
                                        <asp:Label CssClass="control-label" runat="server">由編號：</asp:Label>
                                        <asp:TextBox ID="txtNUM" CssClass="tb tb-xs" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnCopy" CssClass="btn-default btnw4" Text="複製資料" CommandName="Copy" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="購案編號">
                                <ItemTemplate>
                                    <asp:Label ID="lblOVC_PURCH_A_5" Text='<%# Eval("OVC_PURCH_A_5") %>' CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH" Text='<%# Eval("OVC_PURCH") %>' Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH_5" Text='<%# Eval("OVC_PURCH_5") %>' Style="display:none" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="開標次數(組別)" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="lblONB_TIMES" Text='<%# Eval("ONB_TIMES") %>' CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label text-red" runat="server">(</asp:Label>
                                    <asp:Label ID="lblONB_GROUP" Text='<%# Eval("ONB_GROUP") %>' CssClass="control-label text-red" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label text-red" runat="server">)</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="開標時間" ItemStyle-Width="12%">
                                <ItemTemplate>
                                    <asp:Label ID="lblOVC_DOPEN" Text='<%# Eval("OVC_DOPEN") %>' Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_DOPEN_tw" Text='<%# Eval("OVC_DOPEN_tw") %>' runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_OPEN_HOUR" Text='<%# Eval("OVC_OPEN_HOUR") %>' runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                    <asp:Label ID="lblOVC_OPEN_MIN" Text='<%# Eval("OVC_OPEN_MIN") %>' runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label" runat="server">分</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="開標主持人" DataField="OVC_CHAIRMAN" ItemStyle-Width="7%"/>
                            <asp:BoundField HeaderText="開標結果" DataField="OVC_RESULT" />
                            <asp:TemplateField HeaderText="主官批核日">
                                <ItemTemplate>
                                    <asp:Label ID="lblOVC_DAPPROVE" Text='<%# Eval("OVC_DAPPROVE") %>' Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_DAPPROVE_tw" Text='<%# Eval("OVC_DAPPROVE_tw") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="其他作業" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Button ID="btnToD18" CssClass="btn-default" Text="廠商編輯" CommandName="ToD18" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <div class="title text-center text-red">
                        <asp:Label CssClass="control-label" runat="server">選擇【複製資料】可由所輸入的【編號】帶入預設值，節省輸入時間</asp:Label>
                    </div><br />

                    <div class="title text-center"><asp:Label CssClass="control-label" runat="server">請選擇本案已簽辦之開標時間，並決定其組別 </asp:Label></div>
                    <asp:GridView ID="gvTBMRECEIVE_WORK" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="False" OnRowCommand="gvTBMRECEIVE_WORK_RowCommand" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="作業" HeaderStyle-ForeColor="Red">
                                <ItemTemplate>
                                    <asp:Button ID="btnNew" CssClass="btn-default" Text="新增組別" CommandName="New" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="購案編號" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOVC_PURCH_A_5" Text='<%# Bind("OVC_PURCH_A_5") %>' CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH" Text='<%# Bind("OVC_PURCH") %>' CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH_5" Text='<%# Bind("OVC_PURCH_5") %>' CssClass="control-label" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="開標次數">
                                <ItemTemplate>
                                    <asp:Label ID="lblONB_TIMES" Text='<%# Bind("ONB_TIMES") %>' CssClass="control-label" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="開標時間">
                                <ItemTemplate>
                                    <asp:Label ID="lblOVC_DOPEN_H_M" Text='<%# Bind("OVC_DOPEN_H_M") %>' CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_DOPEN" Text='<%# Bind("OVC_DOPEN") %>' Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_OPEN_HOUR" Text='<%# Bind("OVC_OPEN_HOUR") %>' Style="display:none" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_OPEN_MIN" Text='<%# Bind("OVC_OPEN_MIN") %>' Style="display:none" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="組別">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtONB_GROUP" Text="0" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                    <asp:Label Text="0" CssClass="control-label" runat="server"> (請輸入組別：0 表示不分組)</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>


                    <div class="text-center">
                        <asp:Button ID="btnReturn" CssClass="btn-default btnw4" Text="回上一頁" OnClick="btnReturn_Click" runat="server"/>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->

                </footer>
            </section>
        </div>
    </div>
</asp:Content>
