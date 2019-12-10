<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulletinBoardMemo.aspx.cs" Inherits="FCFDFE.pages.GM.BulletinBoardMemo" %>

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
                        <asp:Label ID="Label2" CssClass="control-label" runat="server">公佈欄</asp:Label>
                    </header>
                    <table  class="table table-bordered">
                        <tr>
                            <td style="width: 100px;">
                                <asp:Label CssClass="control-label" runat="server">標題</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTITLE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td style="width: 100px;">
                                <asp:Label CssClass="control-label" runat="server">發表者</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAUTHOR_ID" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">發表日期</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSTART_DATE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">結束日期</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEND_DATE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                          <tr>
                            <td>
                                <asp:Label CssClass="control-label" runat="server">內容</asp:Label>
                            </td>
                            <td colspan="3">
                                 <asp:TextBox ID="txtCONTEXT" TextMode="MultiLine" Rows="20" CssClass="textarea tb-full" runat="server"></asp:TextBox>
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
