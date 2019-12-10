<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D14_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D14_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    採購發包-階段作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" id="divForm" visible="false" runat="server">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle" style="text-align:center;color:blue"> 購案明細表（承辦人：<asp:Label ID="lblOVC_DO_NAME" runat="server"></asp:Label>）</div>
                            <asp:GridView ID="gvSTATUS" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="gvSTATUS_RowCommand" runat="server" >
     		                    <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button ID="btnChange" CommandName="Change" cssclass="btn-default" Text="異動" Visible='<%#  Convert.ToInt16(Eval("OVC_STATUS")) <= 20 ? false:true %>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="購案編號">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_PURCH" Text='<%# Eval("OVC_PURCH_A_5") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="收辦日" DataField="OVC_DBEGIN" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="結束日" DataField="OVC_DEND" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="購案階段">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_STATUS_Desc" CssClass="control-label text-red" Text='<%# Eval("OVC_STATUS_Desc") %>' runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_STATUS" Text='<%# Eval("OVC_STATUS") %>' Style="display:none" runat="server"></asp:Label>
                                            <asp:Label ID="lblONB_TIMES" Text='<%# Eval("ONB_TIMES") %>' Style="display:none" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="作業天數">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_REMARK" CssClass="control-label text-red" Text='<%# Eval("WorkDays") %>' runat="server" ></asp:Label>
                                            <asp:Label ID="lblDate_Flag" Text=" ╳" ForeColor="#ff00ff" Font-Bold="True" Visible='<%# Convert.ToString(Eval("Date_Flag"))=="1" ? true:false %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
        	                    </Columns>
                            </asp:GridView>
                            <div style="text-align:center"> 
                                <asp:Button ID="btnBack" CssClass="btn-default" OnClick="btnBack_Click" Text="回上一頁" runat="server"/>
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
