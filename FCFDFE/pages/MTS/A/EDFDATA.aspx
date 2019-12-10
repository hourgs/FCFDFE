<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDFDATA.aspx.cs" Inherits="FCFDFE.pages.MTS.A.EDFDATA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"/>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content=""/>
    <meta name="author" content="Mosaddek"/>
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina"/>
    <%--<link rel="shortcut icon" href="img/favicon.html">--%>

    <!-- Bootstrap core CSS -->
    <link href="~/assets/css/bootstrap.css" rel="stylesheet"/>
    <link href="~/assets/css/bootstrap-reset.css" rel="stylesheet"/>
    <!--external css-->
    <link href="~/assets/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="~/assets/css/owl.carousel.css" rel="stylesheet" type="text/css"/>
    <!--picker-->
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-colorpicker/css/colorpicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- Custom styles for this template -->
    <link href="~/assets/css/style.css" rel="stylesheet"/>
    <link href="~/assets/css/style-responsive.css" rel="stylesheet" />
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
</head>
<body>
    <script>
        function close() {
            window.close();
        }
    </script>
    <form id="form1" runat="server" >
        <div class="row">
            <div style="width: 800px; margin:auto; margin-top: 30px;">
                <section class="panel">
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                    <table class="table table-bordered text-center">
                        <tr>
                            <td colspan="2">
                                <asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label>
                            </td>
                            <td colspan="4" class="text-left">
                                <asp:Label ID="lblOVC_EDF_NO" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                            </td>
                            <td colspan="4" class="text-left">
                                <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label CssClass="control-label" runat="server">啟運港(機場)</asp:Label>
                            </td>
                            <td colspan="4" class="text-left">
                                <asp:Label ID="lblOVC_START_PORT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label CssClass="control-label" runat="server">目的港(機場)</asp:Label>
                            </td>
                            <td colspan="4" class="text-left">
                                <asp:Label ID="lblOVC_ARRIVE_PORT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label CssClass="control-label" runat="server">發貨單位</asp:Label>
                            </td>
                            <td colspan="4" class="text-left dt-fixed">
                                <asp:Label ID="lblOVC_SHIP_FROM" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="13" class="td-vertical"><!--列數-->
                                <div class="div-vertical">
                                    <asp:Label CssClass="control-label text-vertical-m" Style="height: 190px;" runat="server">收貨單位</asp:Label><!--高度-->
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <asp:Label CssClass="control-label" runat="server">CONSIGNEE</asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label>
                            </td>
                            <td colspan="3" class="text-left dt-fixed">
                                <asp:Label ID="lblOVC_CON_ENG_ADDRESS" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_CON_TEL" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">傳真</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_CON_FAX" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <asp:Label CssClass="control-label" runat="server">NOTIFY PARTY</asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label>
                            </td>
                            <td colspan="3" class="text-left dt-fixed">
                                <asp:Label ID="lblOVC_NP_ENG_ADDRESS" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_NP_TEL" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">傳真</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_NP_FAX" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <asp:Label CssClass="control-label" runat="server">ALSO NOTIFY<br>PARTY1</asp:Label><br>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label>
                            </td>
                            <td colspan="3" class="text-left dt-fixed">
                                <asp:Label ID="lblOVC_ANP_ENG_ADDRESS" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_ANP_TEL" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">傳真</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_ANP_FAX" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <asp:Label CssClass="control-label" runat="server">ALSO NOTIFY<br>PARTY2</asp:Label><br>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">地址(英)</asp:Label>
                            </td>
                            <td colspan="3" class="text-left dt-fixed">
                                <asp:Label ID="lblOVC_ANP_ENG_ADDRESS2" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_ANP_TEL2" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">傳真</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_ANP_FAX2" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <asp:Label CssClass="control-label" runat="server">發貨人資訊</asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">名字</asp:Label>
                            </td>
                            <td colspan="3" class="text-left">
                                <asp:Label ID="lblOVC_DELIVER_NAME" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">手機</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_DELIVER_MOBILE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">軍線</asp:Label>
                            </td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_DELIVER_MILITARY_LINE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">付款方式</asp:Label>
                            </td>
                            <td colspan="4" class="text-left">
                                <asp:Label ID="lblOVC_PAYMENT_TYPE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                            </td>
                            <td colspan="4" class="text-left dt-fixed">
                                <asp:Label ID="lblOVC_NOTE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" class="text-left">
                                <asp:CheckBox ID="chkOVC_IS_STRATEGY" CssClass="radioButton untrigger" Text="戰略性高科技貨品" Enabled="false" runat="server" /><br>
                                <asp:Panel id="pnStrategy" style="margin: 5px 15px;" Visible="false" runat="server">
                                    <asp:Label CssClass="control-label" Style="padding-right: 8px;" runat="server">有效期限</asp:Label>
                                    <asp:Label ID="lblODT_VALIDITY_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label" Style="padding-left: 16px; padding-right: 8px;" runat="server">輸出許可證號碼</asp:Label>
                                    <asp:Label ID="lblOVC_LICENSE_NO" CssClass="control-label" runat="server"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-left td-inner-table" colspan="6">
                                <asp:GridView ID="GV_TBGMT_EDF_DETAIL" DataKeyNames="EDF_DET_SN" 
                                    CssClass="table table-striped border-top text-center table-inner dt-fixed" 
                                    OnRowDataBound="GV_TBGMT_EDF_DETAIL_RowDataBound"
                                    OnPreRender="GV_TBGMT_EDF_DETAIL_PreRender"
                                    ShowFooter="true" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" />
                                        <asp:BoundField HeaderText="英文品名" DataField="OVC_ENG_NAME" />
                                        <asp:BoundField HeaderText="中文品名" DataField="OVC_CHI_NAME" />
                                        <asp:BoundField HeaderText="料號" DataField="OVC_ITEM_NO" />
                                        <asp:BoundField HeaderText="單號" DataField="OVC_ITEM_NO2" />
                                        <asp:BoundField HeaderText="件號" DataField="OVC_ITEM_NO3" />
                                        <asp:BoundField HeaderText="數量" DataField="ONB_ITEM_COUNT" />
                                        <asp:BoundField HeaderText="單位" DataField="OVC_ITEM_COUNT_UNIT" />
                                        <asp:BoundField HeaderText="重量" DataField="ONB_WEIGHT" />
                                        <asp:BoundField HeaderText="單位" DataField="OVC_WEIGHT_UNIT" />
                                        <asp:BoundField HeaderText="容積" DataField="ONB_VOLUME" />
                                        <asp:BoundField HeaderText="單位" DataField="OVC_VOLUME_UNIT" />
                                        <%--<asp:BoundField HeaderText="體積(長X寬X高)"  />--%>
                                        <asp:TemplateField HeaderText="體積(長X寬X高)">
                                            <ItemTemplate>
                                                <%#Eval("ONB_LENGTH") +" x "+ Eval("ONB_WIDTH") + " x " + Eval("ONB_HEIGHT")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="金額" DataField="ONB_MONEY" />
                                        <asp:BoundField HeaderText="幣別" DataField="OVC_CURRENCY_NAME" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <footer class="panel-footer text-center">
                        <%--<input type="button" class="btn-default btnw4" onclick="close();" value ="關閉視窗" />--%>
                        <asp:Button CssClass="btn-default btnw4" OnClick="Unnamed_Click" OnClientClick="close()" Text="關閉視窗" runat="server"/>
                    </footer>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
