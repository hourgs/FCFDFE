<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D1C_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D1C_1" %>

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
                        國防部國防採購室購案決標公告稿
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
                                            <asp:Label CssClass="control-label" runat="server">傳輸日期 :</asp:Label><asp:Label ID="Label1" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td class="no-bordered">
                                            <asp:Label ID="lblDSEND" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>

                                <table class="table table-bordered">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="契約編號："></asp:Label>
                                            <!--購案編號?契約編號?-->
                                            <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="標的名稱："></asp:Label>
                                            <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="部分決標項目或數量："></asp:Label>
                                            <asp:Label ID="lblOVC_PART_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="招標方式："></asp:Label>
                                            <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="執行現況："></asp:Label>
                                            <asp:Label ID="lblOVC_STATUS" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="決標日期："></asp:Label>
                                            <asp:Label ID="lblOVC_DBID" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="預算金額："></asp:Label>
                                            <asp:Label ID="lblOVC_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblONB_BUDGET" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="底價金額或評審委員會建議金額："></asp:Label>
                                            <asp:Label ID="lblOVC_BID_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblONB_BID_BUDGET" CssClass="control-label" runat="server"></asp:Label>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="總決標金額："></asp:Label>
                                            <asp:Label ID="lblOVC_RESULT_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblONB_BID_RESULT" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="投標廠商家數："></asp:Label>
                                            <asp:Label ID="lblONB_BID_VENDORS" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="得標廠商家數："></asp:Label>
                                            <asp:Label ID="lblONB_RESULT_VENDORS" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="未得標廠商代碼及名稱："></asp:Label>
                                            <asp:Label ID="lblOVC_NONE_VENDORS" CssClass="control-label text-red" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <asp:Label CssClass="control-label text-blue subtitle" runat="server" Text="得標廠商資料" Font-Bold="True"></asp:Label>
                                </div>
                                <table class="table table-bordered">
                                    <tr>
                                        <td class="text-center">
                                            <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label text-red" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商代碼："></asp:Label>
                                            <asp:Label ID="lblOVC_VEN_CST" CssClass="control-label" runat="server"></asp:Label>
                                            <!--找不到-->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商名稱："></asp:Label>
                                            <asp:Label ID="lblOVC_VEN_TITLE_1" CssClass="control-label" runat="server"></asp:Label>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商地址："></asp:Label>
                                            <asp:Label ID="lblOVC_VEN_ADDRESS" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商電話："></asp:Label>
                                            <asp:Label ID="lblOVC_VEN_TEL" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="雇用員工總人數是否超過一百人："></asp:Label>
                                            <asp:Label ID="lblOVER" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Panel ID="Panel1" runat="server">
                                                <asp:Label CssClass="control-label" runat="server" Text="：雇用員工總人數"></asp:Label>
                                                <asp:Label ID="lblONB_EMPLOYEES" CssClass="control-label" runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" runat="server" Text="人，已僱用身心障礙人士"></asp:Label>
                                                <asp:Label ID="lblONB_EMPLOYEES_SPECIAL" CssClass="control-label" runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" runat="server" Text="人，已僱用原住民人士"></asp:Label>
                                                <asp:Label ID="lblONB_EMPLOYEES_ABORIGINAL" CssClass="control-label" runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" runat="server" Text="人。"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="得標商類別："></asp:Label><br>
                                            <!--組別-->
                                            <asp:Label ID="lblVEN_KIND" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="併列共同供應商個別廠商之決標金額："></asp:Label>
                                            <asp:Label ID="lblONB_BID_RESULT_MERG" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="是否為中小企業："></asp:Label>
                                            <asp:Label ID="lblOVC_MIDDLE_SMALL" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="決標金額："></asp:Label>
                                            <asp:Label ID="lblONB_BID_RESULT_1" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="預估分包予中小企業之金額："></asp:Label>
                                            <asp:Label ID="lblONB_BID_JOB" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="原產地國別或得標廠商國別："></asp:Label>
                                            <asp:Label ID="lblOVC_VEN_COUNTRY" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="#得標廠商有採購法第103條第2項規定情形之上級機關核准文號："></asp:Label>
                                            <asp:Label ID="lblOVC_VEN_103_2" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_BID_METHOD_1" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="[附加說明]"></asp:Label>
                                            <asp:Label ID="lblOVC_DESC" CssClass="control-label" runat="server"></asp:Label>
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
