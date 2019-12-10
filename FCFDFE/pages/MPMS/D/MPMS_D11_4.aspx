<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D11_4.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D11_4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    複數決標案-預算分組勾稽作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" id="divForm" style="border: solid 2px;" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                                <table class="table table-bordered" style="text-align:center">
                                    <tr>
                                        <td>輸入要作業的購案編號(前三組) &nbsp;&nbsp;
                                            <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-s" runat="server" ></asp:TextBox>
                                            <asp:Button ID="btnQuery_OVC_PURCH" CssClass="btn-default btnw4" Text="選擇購案" OnClick="btnQuery_OVC_PURCH_Click" runat="server" />&nbsp;&nbsp;
                                            <asp:Button ID="btnBack" CssClass="btn-default btnw4" Text="回主畫面" OnClick="btnBack_Click" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            <div id="divContent" visible="false" runat="server">
                                <table class="table table-bordered text-center" runat="server">
                                    <tr>
                                        <td><asp:Label Text="購案編號" Font-Bold="True" runat="server" ></asp:Label></td>
                                        <td><asp:Label Text="購案名稱" Font-Bold="True" runat="server" ></asp:Label></td>
                                        <td><asp:Label Text="申購人" Font-Bold="True" runat="server" ></asp:Label></td>
                                        <td><asp:Label Text="申購人電話" Font-Bold="True" runat="server" ></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblOVC_PURCH" runat="server" ></asp:Label></td>
                                        <td><asp:Label ID="lblOVC_PUR_IPURCH" runat="server" ></asp:Label></td>
                                        <td><asp:Label ID="lblOVC_PUR_USER" runat="server" ></asp:Label></td>
                                        <td><asp:Label ID="lblOVC_PUR_IUSER_PHONE_EXT" runat="server" ></asp:Label></td>
                                    </tr>
                                </table>
                                <div style="text-align:center" runat="server">
                                    <asp:Label ID="lblSubtitle" CssClass="control-label text-red" runat="server"></asp:Label>
                                </div>

                                <div style="text-align:center">
                                    <asp:GridView ID="gvTBM1118_1" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnRowCommand="gvTBM1118_1_RowCommand" runat="server">
                                        <Columns>
                                            <asp:TemplateField HeaderText="作業">
                                                <ItemTemplate>
                                                   <asp:Button ID="btnEnter" CssClass="btn-default btnw4" Text="確定" CommandName="Enter" CommandArgument='<%# Container.DataItemIndex %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="採包分組">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSaved" Text="已儲存：" Visible="false" runat="server" ></asp:Label>
                                                    <asp:Label ID="lblONB_GROUP" Text='<%# Eval("ONB_GROUP") %>' Visible="false" runat="server" ></asp:Label>&nbsp;
                                                    <asp:DropDownList ID="drpONB_GROUP" CssClass="tb tb-xs" runat="server"></asp:DropDownList>&nbsp;&nbsp;--->                       
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="計畫編制分組" DataField="ONB_GROUP_PRE" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField HeaderText="分組預算" DataField="ONB_GROUP_BUDGET" ItemStyle-CssClass="text-center" DataFormatString="{0:n}" />
                                        </Columns>
                                    </asp:GridView><br />
                                
                                    <div style="text-align:center">
                                        <h4>計畫清單所有的項目</h4>
                                        <table class="table table-bordered text-center" >
                                            <tr>
                                                <td style="width:49%;vertical-align:top">
                                                    <asp:GridView ID="gvTBM1201_odd" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="False" runat="server">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" ItemStyle-CssClass="text-center" ></asp:BoundField>
                                                            <asp:BoundField HeaderText="計畫編制分組" DataField="ONB_GROUP_PRE" ItemStyle-CssClass="text-center" ></asp:BoundField>
                                                            <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" ItemStyle-CssClass="text-center" ></asp:BoundField>
                                                            <asp:TemplateField HeaderText="廠牌/規格">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_BRAND" Text='<%# Eval("OVC_BRAND") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                    <asp:Label ID="lblOVC_MODEL" Text='<%# Eval("OVC_MODEL") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                                <td style="width:2%;""></td>
                                                <td style="width:49%;vertical-align:top">
                                                    <asp:GridView ID="gvTBM1201_even" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="False" runat="server">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" ItemStyle-CssClass="text-center" > </asp:BoundField>
                                                            <asp:BoundField HeaderText="計畫編制分組" DataField="ONB_GROUP_PRE" ItemStyle-CssClass="text-center" ></asp:BoundField>
                                                            <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" ItemStyle-CssClass="text-center" ></asp:BoundField>
                                                            <asp:TemplateField HeaderText="廠牌/規格">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_BRAND" Text='<%# Eval("OVC_BRAND") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                    <asp:Label ID="lblOVC_MODEL" Text='<%# Eval("OVC_MODEL") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
