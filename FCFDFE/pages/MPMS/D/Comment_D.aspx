<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Comment_D.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.Comment_D" %>

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
    <div style=" position:relative;">
    <form id="form1" style="overflow:auto;" runat="server">
        <div class="row">
            <div style="width: 95%; margin: auto;">
                <section class="panel">
                    <header class="title">
                       審查意見
                    </header>
                      <asp:Repeater ID="Repeater_Header"  OnItemDataBound="Repeater_Header_ItemDataBound" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table class="table table-bordered table-striped">
                                        <tr>
                                            <td colspan="4" style="text-align:center">
                                                <asp:HiddenField id ="hidOVC_AUDIT_UNIT" Value='<%# Bind("OVC_AUDIT_UNIT")%>' runat="server" />
                                                <asp:Label CssClass="control-label" Text="審查單位--" runat="server"></asp:Label>
                                                <asp:Label ID="lblUnitName" CssClass="control-label" Text='<%# Bind("OVC_USR_ID")%>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text="(審查者： " runat="server"></asp:Label>
                                                <asp:Label ID="lblName" CssClass="control-label" Text='<%# Bind("OVC_AUDITOR")%>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text=") -- 電話：" runat="server"></asp:Label>
                                                <asp:Label ID="lblPhone" CssClass="control-label" Text='<%# Bind("IUSER_PHONE") %>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text="作業天數：" runat="server"></asp:Label>
                                                <asp:Label ID="lblPROCESSDATE" CssClass="control-label" Text='<%# Bind("PROCESS") %>' runat="server"></asp:Label>
                                             </td>
                                         </tr>
                                      <asp:Repeater ID="Repeater_Content" OnItemDataBound="Repeater_Content_ItemDataBound"  runat="server">
                                       <HeaderTemplate>
                                       </HeaderTemplate>
                                        <ItemTemplate>
                                                 <tr>
                                                   <td rowspan="2" style="width:12%">
                                                       <asp:Label CssClass="control-label" Text="審查意見(" runat="server"></asp:Label>
                                                       <asp:Label ID ="lblNO" CssClass="control-label" Text='<%# Bind("ONB_NO") %>' runat="server"></asp:Label>
                                                       <asp:Label CssClass="control-label" Text=")" runat="server"></asp:Label>
                                                   </td>
                                                   <td style="width:38%">
                                                       <asp:Label ID="lblOVC_CONTENT" CssClass="control-label" Text='<%# Bind("OVC_CONTENT") %>' runat="server"></asp:Label>
                                                   </td>
                                                   <td id="cellTitle" rowspan="2" style="width:10%" runat="server">
                                                       <asp:Label CssClass="control-label" Text="澄覆意見：" runat="server"></asp:Label>
                                                   </td>
                                                   <td id="cellContent" rowspan="2" style="width:40%" runat="server">
                                                       <asp:Label ID="OVC_CHECK_REASON" CssClass="control-label" Text='<%# Bind("OVC_RESPONSE") %>' runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                                <tr>
                                                   <td style="width:38%">
                                                       <asp:Label ID="lblOVC_RESPONSE" CssClass="control-label" Text='<%# Bind("OVC_CHECK_REASON") %>' runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                      </asp:Repeater>
                                    </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                    <footer class="panel-footer" style="text-align: center;">
                    </footer>
                </section>
            </div>
        </div>
    </form>
    </div>
</body>

</html>
