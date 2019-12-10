<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="COMMENT.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.COMMENT" %>

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
                       計畫審查作業－審查單位(
                        <asp:Label ID="lblTitleUnit" CssClass="control-label" runat="server"></asp:Label>
                        )
                    </header>
                      <table  class="table table-bordered">
                        <tr>
                            <td colspan="2" class="text-center">
                               <asp:Label ID="Label4" CssClass="control-label" runat="server">購案編號：</asp:Label>
                               <asp:Label ID="lbl_OVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                               <asp:Label ID="Label1" CssClass="control-label" runat="server">&emsp;收案次數：</asp:Label>
                               <asp:Label ID="lblCheckTimes" CssClass="control-label" runat="server"></asp:Label>
                               <asp:Label ID="Label3" CssClass="control-label" runat="server">&emsp;審查人：</asp:Label>
                               <asp:Label ID="lblUserName" CssClass="control-label" runat="server"></asp:Label>
                               <asp:Label ID="Label6" CssClass="control-label" runat="server">&emsp;回覆日：</asp:Label>
                               <asp:Label ID="lblOVC_DAUDIT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                         <asp:Repeater ID="RPT_COMMENT" runat="server">
                            <ItemTemplate>
                               <tr>
                                    <td rowspan="2">
                                        <asp:Label ID="Label8" CssClass="control-label position-left" runat="server">審查意見(</asp:Label>
                                        <asp:Label ID="lblONB" CssClass="control-label position-left" runat="server" Text='<%#Bind("ONB_NO") %>'></asp:Label>
                                        <asp:Label ID="Label2" CssClass="control-label position-left" runat="server">)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_TITLE_NAME" CssClass="control-label" runat="server" Text='<%#Bind("OVC_TITLE_NAME") %>'></asp:Label>
                                        <asp:Label ID="Label10" CssClass="control-label" runat="server">&emsp;&emsp;</asp:Label>
                                        <asp:Label ID="lblOVC_TITLE_ITEM_NAME" CssClass="control-label" runat="server" Text='<%#Bind("OVC_TITLE_ITEM_NAME") %>'></asp:Label>
                                        <asp:Label ID="Label11" CssClass="control-label" runat="server">&emsp;&emsp;</asp:Label>
                                        <asp:Label ID="lblOVC_TITLE_DETAIL_NAME" CssClass="control-label" runat="server" Text='<%#Bind("OVC_TITLE_DETAIL_NAME") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOVC_CHECK_REASON" CssClass="control-label" runat="server" Text='<%#Bind("OVC_CHECK_REASON") %>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                             
                        </asp:Repeater>
                     </table>
                    <footer class="panel-footer" style="text-align: center;">
                    </footer>
                </section>
            </div>
        </div>
    </form>
</body>

</html>
