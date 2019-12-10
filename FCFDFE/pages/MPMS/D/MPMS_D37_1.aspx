<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D37_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D37_1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>採購發包決標公告作業</title>
    <!-- Bootstrap core CSS -->
    <link href="~/assets/css/bootstrap.css" rel="stylesheet" />
    <link href="~/assets/css/bootstrap-reset.css" rel="stylesheet" />
    <!--external css-->
    <link href="~/assets/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="~/assets/css/owl.carousel.css" rel="stylesheet" type="text/css" />
    <!--picker-->
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-colorpicker/css/colorpicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- Custom styles for this template -->
    <link href="~/assets/css/style.css" rel="stylesheet" />
    <link href="~/assets/css/style-responsive.css" rel="stylesheet" />

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body>
    <script>
        function close() {
            window.close();
        }
    </script>
    <form id="form1" runat="server">
        <div class="row">
            <div style="width: 1000px; margin: auto;">
                <section class="panel">
                    <header class="title">
                        <!--標題-->
                        國防部國防採購室購案契約草稿
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                    <%-- <div class="text-center" style="padding-bottom: 10px">
                        <asp:Button ID="btnClose" CssClass="btn-default btnw4" OnClientClick="window.close();" Text="關閉視窗" runat="server" />
                    </div>--%>
                    <div class="panel-body" id="divForm" visible="false" runat="server">
                        <div class="form">
                            <div class="cmxform form-horizontal tasi-form">
                                <div class="subtitle text-red">
                            購案契約編號：<asp:Label ID="lblInfo_PurchNo" CssClass="control-label" runat="server"></asp:Label>
                            合約商：<asp:Label ID="lblInfo_Contractor" CssClass="control-label" runat="server"></asp:Label>
                            組別：<asp:Label ID="lblInfo_Group" CssClass="control-label" runat="server"></asp:Label>
                        </div>
                        <table class="table table-bordered text-center tr1">
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">契約編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH_6" CssClass="control-label" runat="server"></asp:Label>
                                    組別
                                    <asp:Label ID="lblONB_GROUP" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">名稱</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">預算金額</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblONB_GROUP_BUDGET" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblONB_PUR_BUDGET" CssClass="control-label" runat="server"></asp:Label>
                                    元整
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">決標日期</asp:Label>
                                </td>
                                <td style="width: 45%">
                                    <asp:Label ID="lblOVC_DBID" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label position-left" runat="server">開標日期：</asp:Label>
                                    <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">簽約日期</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_DCONTRACT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">契約金額</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblONB_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblONB_MCONTRACT" CssClass="control-label" runat="server"></asp:Label>
                                    元整
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">折讓金額</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblONB_MONEY_DISCOUNT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">交貨地點</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_RECEIVE_PLACE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">交貨時間</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_SHIP_TIMES" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">付款方式</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PAYMENT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">交貨批次</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblONB_DELIVERY_TIMES" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 12%">
                                    <asp:Label CssClass="control-label" runat="server">廠商統一編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_CST" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">廠商名稱</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">廠商傳真</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_FAX" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">廠商EMAIL</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_EMAIL" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">負責人</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_BOSS" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">廠商電話</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_TEL" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">聯絡人</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_NAME" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">連絡人手機</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_CELLPHONE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">廠商地址</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblOVC_VEN_ADDRESS" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label CssClass="control-label" runat="server">重要事項</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblOVC_CONTRACT_COMM" CssClass="control-label" runat="server"></asp:Label>
                                </td>
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
    </form>
</body>
</html>
