<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestMemo.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.RequestMemo" %>

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
    <title></title>

</head>
<body>
    <script>
        function close() {
            window.close();
        }
    </script>
    <form id="form1" runat="server">
        <div class="row">
            <div style="width: 95%; margin: auto;">
                <section class="panel">
                    <header class="title">
                        <asp:Label ID="lblTitleUnit" CssClass="control-label" runat="server"></asp:Label>
                        <asp:Label ID="Label2" CssClass="control-label" runat="server">購案物資申請書</asp:Label>
                    </header>
                      <table  class="table table-bordered">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" CssClass="control-label" runat="server">請求事項</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRequestMemo" TextMode="MultiLine" Rows="20" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                          <tr>
                            <td>
                                <asp:Label ID="Label3" CssClass="control-label" runat="server">備考</asp:Label>
                            </td>
                            <td>
                                 <asp:TextBox ID="txtMarkMemo" TextMode="MultiLine" Rows="20" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                     </table>
                    <footer class="panel-footer" style="text-align: center;">
                    </footer>
                </section>
            </div>
        </div>
    </form>
</body>

</html>
