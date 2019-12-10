<%@ Page Title="" Language="C#" MasterPageFile="~/Super.Master" AutoEventWireup="true" CodeBehind="Super_Purchase_History_2.aspx.cs" Inherits="FCFDFE.pages.Sup.Super_Purchase_History_2" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    廠商資訊
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">統一編號</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_CST" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">廠商名稱</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">廠商簡稱</asp:Label>
                                    </td>
                                    <td style="width:18%">
                                        <asp:Label ID="lblOVC_NVEN" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">地址一</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_VEN_ADDRESS" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">地址二</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_VEN_ADDRESS_1" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">建檔日</asp:Label>
                                    </td>
                                    <td style="width:18%">
                                        <asp:Label ID="lblOVC_PUR_CREATE" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">電話號碼</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_VEN_ITEL" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">傳真</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_FAX_NO" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">負責人/聯絡人</asp:Label>
                                    </td>
                                    <td style="width:18%">
                                        <asp:Label ID="lblOVC_BOSS" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">經濟部編號</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblGINGE_VEN_CST" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">國管代號</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblMIXED_CAGE" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">國管回饋日</asp:Label>
                                    </td>
                                    <td style="width:18%">
                                        <asp:Label ID="lblCAGE_DATE" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">有效日期</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_DMANAGE_BEGIN" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">終止日期</asp:Label>
                                    </td>
                                    <td style="width:23%">
                                        <asp:Label ID="lblOVC_DMANAGE_END" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">最後復權日</asp:Label>
                                    </td>
                                    <td style="width:18%">
                                        <asp:Label ID="lblOVC_DRECOVERY" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: middle; width:12%">
                                        <asp:Label CssClass="control-label " runat="server">主要營業項目</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblOVC_MAIN_PRODUCT" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btnBack" OnClick="btnBack_Click" CssClass="btn-warning btnw2" runat="server" Text="返回" /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <header class="title">
                    歷史得標紀錄
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:GridView ID="GVTBGMT_1302" DataKeyNames="OVC_PURCH" OnRowCommand="GVTBGMT_1302_RowCommand" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_1302_PreRender" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="項次" >
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="採購案號(1~3)" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1"  CommandName="btnPurch" runat="server"><%# Eval("OVC_PURCH")%></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH1301" />
                                    <asp:TemplateField HeaderText="分約號(6)" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton2" CommandArgument='<%# Eval("OVC_PURCH_6")%>' CommandName="btnPurch6" runat="server"><%# Eval("OVC_PURCH_6")%></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="購辦號(5)" DataField="OVC_PURCH_5" />
                                    <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                                    <asp:BoundField HeaderText="決標日" DataField="OVC_DBID" DataFormatString="{0:d}"/>
                                    <asp:BoundField HeaderText="決標金額" DataField="ONB_MONEY" />
                                </Columns>
                            </asp:GridView>
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
