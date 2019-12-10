<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D35_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D35_1" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>採購發包無法決標公告作業</title>
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
                        國防部國防採購室購案無法決標公告稿
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                    <%-- <div class="text-center" style="padding-bottom: 10px">
                        <asp:Button ID="btnClose" CssClass="btn-default btnw4" OnClientClick="window.close();" Text="關閉視窗" runat="server" />
                    </div>--%>
                    <div class="panel-body" id="divForm" visible="false" runat="server">
                        <div class="form">
                            <div class="cmxform form-horizontal tasi-form">
                                <table class="table no-bordered-seesaw text-center">
                                    <tr class="no-bordered-seesaw">
                                        <td style="width: 40%" class="text-right no-bordered">
                                            <asp:Label CssClass="control-label" runat="server">傳輸日期 :</asp:Label></td>
                                        <td class="no-bordered">
                                            <asp:Label ID="lblDSEND" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                        <table class="table table-bordered text-center" style="margin-bottom: 0px;">
                            <tr class="no-bordered">
                                <td style="width: 30%">
                                    <asp:Label CssClass="control-label" runat="server">案號</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">NC06001L074</asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td style="width: 30%">
                                    <asp:Label CssClass="control-label" runat="server">標的名稱</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server">NC06001L074</asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">招標方式</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">投標廠商家數</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblONB_BID_VENDORS" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">開標日期</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">最近一次公告日期</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_DANNOUNCE_LAST" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">無法決標理由</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_RESULT_REASON" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">是否繼續辦理採購</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_CONTINUE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="no-bordered">
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">附加說明</asp:Label></td>
                                <td>
                                    <asp:Label ID="lblOVC_MEMO" CssClass="control-label" runat="server"></asp:Label>
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
