<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A28_3.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A28_3" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->
                    <div>出口報單-刪除</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnDelete" runat="server"></asp:Panel>
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton CssClass="btn-success btnw8" OnClick="btnBack_Click" Text="回出口報單管理" runat="server"></asp:LinkButton>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width:25%;">
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">類別代號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_CLASS_CDE" CssClass="control-label" runat="server" Text="[dcodeLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">類別名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_CLASS_NAME" CssClass="control-label" runat="server" Text="[dnameLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">報單號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ECL_NO" CssClass="control-label" runat="server" Text="[declLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">出口關別</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_EXP_TYPE" CssClass="control-label" runat="server" Text="[dtypeLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船或關代號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_SHIP_CDE" CssClass="control-label" runat="server" Text="[dshipLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">裝貨單或收序號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PACK_NO" CssClass="control-label" runat="server" Text="[dpackLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">報關日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_EXP_DATE" CssClass="control-label" runat="server" Text="[dexpDetailLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">貨物存放處所</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_STORED_PLACE" CssClass="control-label" runat="server" Text="[dplaceLabel]"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="td-inner-table text-left">
                                        <asp:GridView ID="GVTBGMT_EDF_DETAIL" CssClass="table table-striped border-top text-center table-inner" AutoGenerateColumns="false" OnPreRender="GVTBGMT_EDF_DETAIL_PreRender" runat="server">
                                            <Columns>
                                                <asp:TemplateField HeaderText="項次" ItemStyle-CssClass="text-center" >
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="英文品名" DataField="OVC_ENG_NAME" />
                                                <asp:BoundField HeaderText="中文品名" DataField="OVC_CHI_NAME" />
                                                <asp:BoundField HeaderText="料號" DataField="OVC_ITEM_NO" />
                                                <asp:BoundField HeaderText="單號" DataField="OVC_ITEM_NO2" />
                                                <asp:BoundField HeaderText="件號" DataField="OVC_ITEM_NO3" />
                                                <asp:BoundField HeaderText="數量" DataField="ONB_ITEM_COUNT" />
                                                <asp:BoundField HeaderText="箱件" DataField="OVC_ITEM_COUNT_UNIT" />
                                                <asp:BoundField HeaderText="重量" DataField="ONB_WEIGHT" />
                                                <asp:BoundField HeaderText="單位" DataField="OVC_WEIGHT_UNIT" />
                                                <asp:BoundField HeaderText="容積" DataField="ONB_VOLUME" />
                                                <asp:BoundField HeaderText="單位" DataField="OVC_VOLUME_UNIT" />
                                                <asp:BoundField HeaderText="體積(長X寬X高)" DataField="ONB_BULK" />
                                                <asp:BoundField HeaderText="金額" DataField="ONB_MONEY" />
                                                <asp:BoundField HeaderText="幣別" DataField="OVC_CURRENCY" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <asp:Button ID="btnDel" CssClass="btn-danger btnw6" Text="刪除出口報單" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" OnClick="btnDel_Click" runat="server" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
