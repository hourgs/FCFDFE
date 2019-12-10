<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D11_5.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D11_5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="row">
    <div style="width: 1000px; margin:auto;">
        <section class="panel">
            <header  class="title">
                <!--標題-->
                複數決標案--計畫清單項目分組作業
            </header>
            <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
            <div id="divOVC_PURCH" visible="false" runat="server">
                <table style="width:100%;text-align:left;border:none">
                    <tr>
                        <td style="width:20%">
                            <asp:Label CssClass="control-label text-blue" runat="server">(1)設定購案編號為複數決標</asp:Label><br />
                            <asp:Label CssClass="control-label text-blue" runat="server">　 輸入要作業的購案編號(前三組)</asp:Label>
                        </td>
                        <td style="width:80%">
                            <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-s" runat="server" ></asp:TextBox>&nbsp;
                            <asp:Button ID="btnQuery_OVC_PURCH" CssClass="btn-default btnw4" Text="選擇購案" OnClick="btnQuery_OVC_PURCH_Click" runat="server" />&nbsp;&nbsp;
                            <asp:Button ID="btnBack" CssClass="btn-default btnw4" Text="回主畫面" OnClick="btnBack_Click" runat="server" />
                        </td>
                    </tr>
                </table><br />
            </div>
            

            <div id="divForm" visible="false" runat="server">
                <table class="table table-bordered text-center">
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



                <asp:Panel ID="divChk" runat="server">
                    <asp:CheckBox ID="chkIS_PLURAL_BASIS" Text="本案為複數決標" Checked='<%# Convert.ToBoolean(Eval("IS_PLURAL_BASIS")) %>' runat="server" />
                    <asp:CheckBox ID="chkIS_OPEN_CONTRACT" Text="本案為開放式契約" Checked='<%# Convert.ToBoolean(Eval("IS_OPEN_CONTRACT")) %>' runat="server" />
                    <asp:CheckBox ID="chkIS_JUXTAPOSED_MANUFACTURER" Text="本案為並列得標廠商" Checked='<%# Convert.ToBoolean(Eval("IS_JUXTAPOSED_MANUFACTURER")) %>' runat="server" />&nbsp;
                    <asp:Button ID="btnSave" CssClass="btn-default btnw4" Text="存檔" OnClick="btnSave_Click" runat="server" /><br />
                    <div style="text-align:center;color:forestgreen;font-size:larger;padding:20px;">
                        <asp:Label ID="lblIS_PLURAL_BASIS" Text="本案為非複數決標請先設定為複數決標" CssClass="control-label" Visible="false" runat="server"></asp:Label>
                    </div>
                </asp:Panel>
                <asp:Panel ID="divContent" Visible="false" runat="server">
                    <div>
                        <asp:Label CssClass="control-label text-blue" runat="server">(2)請先輸入要作業的組別：</asp:Label>
                        <asp:DropDownList ID="drpGROUP" CssClass="tb tb-xs" OnSelectedIndexChanged="drpGROUP_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList><br/>
                    </div>
                    <div class="text-center">
                        <asp:Label CssClass="control-label text-red" runat="server">如果要修改第</asp:Label>
                        <asp:Label ID="lblONB_GROUP_PRE" CssClass="control-label text-red" runat="server">1</asp:Label>
                        <asp:Label CssClass="control-label text-red" runat="server">組項次請在此作業</asp:Label>
                    </div>
                    <table class="table table-bordered text-center">
                        <tr>
                            <th colspan="3">
                            <div class="text-left">
                                <asp:Label CssClass="control-label text-blue" runat="server">(3)組別：</asp:Label>
                                <asp:Label ID="lblDrpSelect" CssClass="control-label text-blue" runat="server"> 1 </asp:Label>
                                <asp:Label CssClass="control-label text-blue" runat="server">所選的項目</asp:Label>
                            </div>
                            </th>
                        </tr>
                        <tr>
                            <td style="width:49%;vertical-align:top">
                                <asp:GridView ID="gvGroupLeft" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button ID="btnCancel_LEFT" CssClass="btn-default btnw2" OnClick="btnCancel_LEFT_Click" Text="取消" CommandName="按鈕屬性" runat="server" />
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                        <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                        <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td style="width:2%">

                            </td>
                            <td style="width:49%;vertical-align:top">
                                <asp:GridView ID="gvGroupRight" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="作業">
                                            <ItemTemplate>
                                                <asp:Button ID="btnCancel_Right" CssClass="btn-default btnw2" OnClick="btnCancel_Right_Click" Text="取消" CommandName="按鈕屬性" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                        <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                        <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left" colspan="3">
                                <asp:Label CssClass="control-label" runat="server">分組預算：</asp:Label>
                                <asp:Label ID="lblGRUOP_BUDGE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                
          
                    <div class="text-center">
                        <asp:Label CssClass="control-label text-red" runat="server">如果要新增第</asp:Label>
                        <asp:Label ID ="lblONB_GROUP_PRE_2" CssClass="control-label text-red" runat="server">1</asp:Label>
                        <asp:Label CssClass="control-label text-red" runat="server">組分組項次請在此作業</asp:Label>
                    </div>
                    <table class="table table-bordered text-center">
                        <tr>
                            <th colspan="3">
                                <div class="text-left">
                                <asp:Label CssClass="control-label text-blue" runat="server">(4)計畫清單所有的項目：</asp:Label>
                                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="btn-default btnw4" runat="server" Text="新增內容" />&nbsp;
                                <asp:Button ID="btnReset" OnClick="btnReset_Click" CssClass="btn-default btnw4" runat="server" Text="全部清除" />&nbsp;
                                <asp:Button ID="btnSelectAll" OnClick="btnSelectAll_Click" CssClass="btn-default btnw4" runat="server" Text="選擇全部" />
                                </div>
                            </th>
                        </tr>
                        <tr>
                            <td style="width:49%;vertical-align:top">
                                <asp:GridView ID="gvONB_POI_ICOUNT_LEFT" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:checkbox ID="cbIsGroupLeft" Visible='<%# (Eval("ONB_GROUP_PRE").ToString().Equals(string.Empty)) %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                        <asp:BoundField HeaderText="組別" DataField="ONB_GROUP_PRE" />
                                        <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                        <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td style="width:2%">
                            </td>
                            <td style="width:49%;vertical-align:top">
                                <asp:GridView ID="gvONB_POI_ICOUNT_Right" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:checkbox ID="cbIsGroupRight" Visible='<%# (Eval("ONB_GROUP_PRE").ToString().Equals(string.Empty)) %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                        <asp:BoundField HeaderText="組別" DataField="ONB_GROUP_PRE" />
                                        <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                        <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <footer class="panel-footer" style="text-align: center;">
                <!--網頁尾-->
            </footer>
        </section>
    </div>
</div>
</asp:Content>
