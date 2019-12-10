<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A27_2.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A27_2" %>
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
                    <div>出口報單建立-Step2 填寫出口報單資料</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnWarning" runat="server"></asp:Panel>
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton CssClass="btn-success btnw8" OnClick="btnBack_Click" Text="回出口報單建立" runat="server"></asp:LinkButton>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
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
                                        <asp:TextBox ID="txtOVC_CLASS_CDE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">類別名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_CLASS_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">報單號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_ECL_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <%--<asp:Button ID="btnBringfield" CssClass="btn-success btnw6" runat="server" Text="帶入下方欄位" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">出口關別</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_EXP_TYPE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船或關代號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_CDE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">裝貨單或收序號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PACK_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">報關日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_EXP_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">貨物存放處所</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_STORED_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="td-inner-table text-left">
                                        <asp:GridView ID="GVTBGMT_EDF_DETAIL" CssClass="table table-striped border-top table-inner" AutoGenerateColumns="false" OnRowCreated="GVTBGMT_EDF_DETAIL_RowCreated" OnPreRender="GVTBGMT_EDF_DETAIL_PreRender" runat="server">
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
                            <div class="text-center">
                                <asp:Button ID="btnNew" CssClass="btn-warning btnw6" Text="新增出口報單" OnClick="btnNew_Click" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <%--<asp:Label CssClass="control-label text-red" runat="server" Text="[messageLabel]"></asp:Label><br>--%>
                    <asp:LinkButton ID="btnBack" Visible="false" OnClick="btnBack_Click" Text="繼續新增出口報單" runat="server"></asp:LinkButton>
                    <%--<asp:HyperLink runat="server" NavigateUrl="~/pages/MTS/A/MTS_A27_1.aspx">繼續新增出口報單</asp:HyperLink>--%>
                </footer>
            </section>
        </div>
    </div>
</asp:Content>