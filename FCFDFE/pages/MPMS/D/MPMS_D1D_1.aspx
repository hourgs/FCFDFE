<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D1D_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D1D_1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="Mosaddek" />
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina" />
    <%--<link rel="shortcut icon" href="img/favicon.html">--%>

    <title>單位查詢</title>

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

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
      <script src="~/assets/js/html5shiv.js"></script>
      <script src="~/assets/js/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <script>
        function reval() {

            window.close();
        }



    </script>
    <form id="form1" runat="server">
        <div class="row">
            <div style="width: 600px; margin: auto;">
                <section>
                    <br />
                    <br />
                    <div>
                        <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <div class="form" style="text-align: center;">
                            <header class="title">
                                <asp:Label runat="server">本合約之合約商資料</asp:Label>
                                <asp:Button ID="btnSave" OnClientClick="reval()" CssClass="btn-warning" runat="server" Text="離開" />
                            </header>
                            <div id="divForm" class="cmxform form-horizontal tasi-form" visible="false" runat="server">
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">合約編號</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">合約商統一編號</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">合約商名稱</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">合約商電話</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">合約商地址</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_PURCH_6" CssClass="control-label" runat="server">合約編號</asp:Label>
                                        </td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_VEN_CST" CssClass="control-label" runat="server">合約商統一編號</asp:Label>
                                        </td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server">合約商名稱</asp:Label>
                                        </td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_VEN_TEL" CssClass="control-label" runat="server">合約商電話</asp:Label>
                                        </td>
                                        <td class="text-left">
                                            <asp:Label ID="lblOVC_VEN_ADDRESS" CssClass="control-label" runat="server">合約商地址</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <br />
                </section>
            </div>
        </div>
    </form>
</body>
</html>
