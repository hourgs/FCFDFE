<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D11_6.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D11_6" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <title>採購發包處採購時程管制表</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div style="width: 1000px; margin:auto;">
                <section class="panel">
                    <header  class="title">
                        <!--標題-->
                        採購發包處採購時程管制表
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                    <div class="panel-body" id="divForm" style=" border: solid 2px;" visible="false" runat="server">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                             <!--網頁內容-->
                                <table class="table table-bordered text-center">
                                    <caption style="text-align:center">
                                        <asp:Label CssClass="control-label" Text="案號：" Font-Size="Large" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" Text="" Font-Size="Large" runat="server"></asp:Label>
                                    </caption>
                                    <tbody>
                                    <tr>
                                         <td rowspan="5" class="td-vertical">
                                            <asp:Label CssClass="control-label text-vertical-m text-green" style="height: 190px;" Width="20px" Font-Bold="True" runat="server">基本資料</asp:Label></td>
                                        <td><asp:Label CssClass="control-label" Text="計畫申請單位" Font-Bold="True" runat="server"></asp:Label></td>
                                            <td colspan="5" class="text-left"><asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="購案名稱" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="5" class="text-left"><asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="招標方式" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_PUR_ASS_VEN_CODE" runat="server"></asp:Label></td>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="預算年度" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_ISOURCE" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="預算金額" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_PUR_CURRENT" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" ID="lblONB_PUR_BUDGET" runat="server"></asp:Label></td>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="交貨天數" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblONB_DELIVER_DAYS" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="核定日期" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_PUR_DAPPROVE" runat="server"></asp:Label></td>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="收辦日期" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DBEGIN" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td rowspan="4" class="td-vertical">
                                            <asp:Label CssClass="control-label text-vertical-m text-green" style="height: 190px;" Width="20px" Font-Bold="True" runat="server">時程管制</asp:Label></td>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="等標期" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DWAIT" runat="server"></asp:Label>天</td>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="類別" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DWAIT_KIND" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="決標方式" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_MEMO" runat="server"></asp:Label></td>
                                        <td colspan="2"><asp:Label CssClass="control-label" Text="招標關鍵日" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DBID" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td rowspan="2"><asp:Label CssClass="control-label" Text="計畫申購" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" Text="預劃" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DAPPLY" runat="server"></asp:Label></td>
                                        <td rowspan="2"><asp:Label CssClass="control-label" Text="採購發包" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" Text="預劃" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_PRE_DAPPROVE" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="實際" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DPROPOSE" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" Text="實際" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                         <td rowspan="6" class="td-vertical">
                                            <asp:Label CssClass="control-label text-vertical-m text-green" style="height: 190px;" Width="20px" Font-Bold="True" runat="server">進度管制</asp:Label></td>
                                        <td><asp:Label CssClass="control-label text-red" Text="程序" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label text-red" Text="預定日期" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label text-red" Text="實際日期" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3"><asp:Label CssClass="control-label text-red" Font-Bold="True" Text="調整原因" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="標單製作" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td></td>
                                        <td></td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="公告邀商" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DOPEN" runat="server"></asp:Label></td>
                                        <td></td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="第一次開標" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DOPEN_1" runat="server"></asp:Label></td>
                                        <td></td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="第二次開標" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DOPEN_2" runat="server"></asp:Label></td>
                                        <td></td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="合約簽訂" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_PRE_CONTRACR" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DCONTRACT" runat="server"></asp:Label></td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="7"><asp:Label CssClass="control-label" Text="評核重要事項" Font-Bold="True" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="7"><asp:Label CssClass="control-label" ID="lbl" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="7"><asp:Label CssClass="control-label" Text="採購重要事項" Font-Bold="True" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="7"><asp:Label CssClass="control-label" ID="lblOVC_COMM" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="7"><asp:Label CssClass="control-label" Text="檢討與因應措施" Font-Bold="True" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="7"><asp:Label CssClass="control-label" ID="lblOVC_COMM_REASON" runat="server"></asp:Label></td>
                                    </tr>
                                </tbody>
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
