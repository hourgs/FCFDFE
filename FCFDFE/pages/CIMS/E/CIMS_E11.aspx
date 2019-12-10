<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_E11.aspx.cs" Inherits="FCFDFE.pages.CIMS.E.CIMS_E11" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1100px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>歷史商情查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">請選填完查詢條件後，按下[查詢]即可查詢(查詢範圍為已決標之購案)</div>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td width="10%">
                                        <asp:Label CssClass="control-label" runat="server">品名</asp:Label></td>
                                    <td width="40%">
                                        <asp:TextBox ID="txtOVC_POI_NSTUFF_CHN" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                    <td width="10%">
                                        <asp:Label CssClass="control-label" runat="server">料號</asp:Label></td>
                                    <td width="40%">
                                        <asp:TextBox ID="txtNSN" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">決標日期區間</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtovc_aq_s" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">&ensp;至&ensp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtovc_aq_e" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">件號</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_POI_IREF" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">廠商名稱</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-l" runat="server">
                                        </asp:TextBox></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_PUR_IPURCH" CssClass="tb tb-l" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label></td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-l" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                            </table>
                            <div style="text-align: center">
                                <asp:Button ID="btnQuery" OnClick="btnQuery_Click" CssClass="btn-success btnw2" runat="server" Text="查詢" />
                                &emsp;
                                  <asp:Button ID="btnReset" OnClick="btnReset_Click" CssClass="btn-default btnw2" runat="server" Text="清除" /><br />
                                <br />
                            </div>
                            <asp:Panel ID="pnQuery" runat="server">
                                <div class="cmxform form-horizontal tasi-form" style="line-height: 0">
                                    <!--網頁內容-->
                                    <table class="table table-bordered text-center " style="font-size: 10px;">
                                        <tr style="background-color: #f0f0f0; font-weight: 800">
                                            <td rowspan="2" style="width: 5%">
                                                <asp:Label runat="server">項次</asp:Label></td>
                                            <td style="width: 10%">
                                                <asp:Label runat="server">購案案號(1~3)</asp:Label></td>
                                            <td style="width: 8%">
                                                <asp:Label runat="server">採購地區(4)</asp:Label></td>
                                            <td style="width: 20%">
                                                <asp:Label runat="server">料號</asp:Label></td>
                                            <td style="width: 20%">
                                                <asp:Label runat="server">件號</asp:Label></td>
                                            <td style="width: 7%" rowspan="2">
                                                <asp:Label runat="server">合約數量</asp:Label></td>
                                            <td style="width: 5%" rowspan="2">
                                                <asp:Label runat="server">單位</asp:Label></td>
                                            <td style="width: 8%">
                                                <asp:Label runat="server">合約單價</asp:Label></td>
                                            <td style="width: 10%">
                                                <asp:Label runat="server">得標商編號</asp:Label></td>
                                            <td style="width: 7%">
                                                <asp:Label runat="server">報價折幅</asp:Label></td>
                                        </tr>
                                        <tr style="background-color: #f0f0f0; font-weight: 800">
                                            <td>
                                                <asp:Label runat="server">分約號(6)</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server">採購項次</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server">中文品名</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server">購案名稱</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server">決標日</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server">廠商名稱</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server">標價折幅</asp:Label></td>
                                        </tr>
                                        <asp:DataList ID="ItemsList" RepeatDirection="Vertical" RepeatLayout="Flow" RepeatColumns="1" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td rowspan="2" style="width: 5%">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OVC_NUMBER") %>'>1</asp:Label></td>
                                                    <td style="width: 10%">
                                                        <asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "OVC_PURCH") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "OVC_PURCH_URL") %>' runat="server"></asp:HyperLink></td>
                                                    <td style="width: 8%">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OVC_PUR_AGENCY") %>'>L</asp:Label></td>
                                                    <td style="width: 20%">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NSN") %>'>123</asp:Label></td>
                                                    <td style="width: 20%">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OVC_POI_IREF") %>'>123</asp:Label></td>
                                                    <td style="width: 7%" rowspan="2">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ONB_POI_QORDER_CONT") %>'>3.0</asp:Label></td>
                                                    <td style="width: 5%" rowspan="2">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OVC_POI_IUNIT") %>'>EA2</asp:Label></td>
                                                    <td style="width: 8%; text-align: right">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ONB_POI_MPRICE_CONT") %>'>16,065</asp:Label></td>
                                                    <td style="width: 10%">
                                                        <asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "OVC_VEN_CST") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "OVC_VEN_CST_URL") %>' runat="server"></asp:HyperLink></td>
                                                    <td style="width: 7%">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "QUOTE_PRICE") %>'>0%</asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "OVC_PURCH_6") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "OVC_PURCH_6_URL") %>' runat="server"></asp:HyperLink></td>
                                                    <td>
                                                        <asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "ONB_POI_ICOUNT") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "ONB_POI_ICOUNT_URL") %>' runat="server"></asp:HyperLink></td>
                                                    <td style="text-align: left">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OVC_POI_NSTUFF_CHN") %>'>103年度國軍歷史文物館全館大理石面研磨(含樓梯)</asp:Label></td>
                                                    <td style="text-align: left">
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OVC_PUR_IPURCH") %>'>103年度國防部博愛營區暨與營區環境清潔等6項</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OVC_DBID") %>'>2014-01-07</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OVC_VEN_TITLE") %>'>暐得實業有限公司</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MARKED_PRICE") %>'>0%</asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </table>
                                </div>
                            </asp:Panel>
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
