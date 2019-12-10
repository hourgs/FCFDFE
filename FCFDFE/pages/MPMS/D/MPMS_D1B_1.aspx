<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D1B_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D1B_1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>開標結果報告表</title>
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
                        國防部國防採購室<br />
                        <asp:Label ID="lblOVC_PURCH" runat="server"></asp:Label>案開標結果報告表
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                   <%-- <div class="text-center" style="padding-bottom: 10px">
                        <asp:Button ID="btnClose" CssClass="btn-default btnw4" OnClientClick="window.close();" Text="關閉視窗" runat="server" />
                    </div>--%>
                    <div class="panel-body" id="divForm" visible="false" runat="server">
                        <div class="form">
                            <div class="cmxform form-horizontal tasi-form">
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="開標時間"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_DOPEN" CssClass="control-label position-left" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_OPEN_HOUR" CssClass="control-label position-left" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_OPEN_MIN" CssClass="control-label position-left" runat="server"></asp:Label>&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="申購單位"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="標的名稱"></asp:Label></td>
                                        <td colspan="3" class="text-left">
                                            <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label position-left" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="採購金額"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_PUR_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_BUDGET_BUY" CssClass="control-label text-red" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" runat="server" Text="(預算金額+後續擴充金額)"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="預算金額"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblONB_PUR_BUDGET" runat="server" CssClass="control-label" Text=""></asp:Label>
                                            <asp:TextBox ID="txtONB_PUR_BUDGET" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="得標商"></asp:Label></td>
                                        <td colspan="3" class="text-left">
                                            <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="決標金額"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_RESULT_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblONB_BID_RESULT" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="底價金額"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_BID_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblONB_BID_BUDGET" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="標餘款"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:Label ID="lblONB_REMAIN_BUDGET" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="開標次數"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:Label ID="lblONB_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="說明"></asp:Label></td>
                                        <td class="text-left" colspan="3">

                                            <asp:TextBox ID="txtOVC_DESC" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="200px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="text-left" colspan="4">
                                            <asp:Label CssClass="control-label" runat="server" Text="會辦意見："></asp:Label>
                                            <asp:TextBox ID="txtOVC_MEETING" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="90px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="text-left" colspan="4">
                                            <asp:Label CssClass="control-label" runat="server" Text="擬辦："></asp:Label>
                                            <asp:TextBox ID="txtOVC_ADVICE" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="90px"></asp:TextBox>
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
