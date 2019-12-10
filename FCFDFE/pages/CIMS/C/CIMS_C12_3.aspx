<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIMS_C12_3.aspx.cs" Inherits="FCFDFE.pages.CIMS.C.CIMS_C12_3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"/>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content=""/>
    <meta name="author" content="Mosaddek"/>
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina"/>
    <%--<link rel="shortcut icon" href="img/favicon.html">--%>

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
    <title></title>
    
</head>
<body>
    <script>
        function close() {
            window.close();
        }
    </script>
    <form id="form1" runat="server" >
        <div class="row">
            <div style="width: 800px; margin:auto;">
                <section class="panel">                          
                    <table class="table table-bordered text-center">                             
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">A.公告屬性</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_A" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">B.招標機關</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_B" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">C.案號</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_C" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">D.標的名稱</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_D" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">E.標的分類)</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_E" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">F.預算金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_F" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">G.金額級距</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_G" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">H.招標方式</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_H" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">I.決標方式</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_I" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">J.底價金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_J" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">K.底價金額公開</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_K" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">L.決標金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_L" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">M.決標金額公開</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_M" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">N.得標廠商數</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_N" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">O.廠商名稱</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_O" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">P.廠商統編</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_P" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">Q.採購法58條規定</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_Q" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">R.限制性招標依據之法條</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_R" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">S.公告日期</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_S" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">T.決標日期 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_T" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">U.底價除以預算金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_U" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">V.決標除以底價金額 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_V" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">W.決標除以預算金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_W" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">X.決標登入日期 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_X" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">Y.投標廠商家數</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_Y" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">Z.:序號 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_Z" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AA.未得標廠商名稱</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AA" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AB.廠商決標金額 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AB" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>                           
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AD.開標日期</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_AD" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                                           
                    </table>                       
                    <footer class="panel-footer" style="text-align: center;">
                        <asp:Button ID="btnclose" CssClass="btn-default btnw4" OnClientClick="close()" OnClick="btnclose_Click" Text="關閉視窗" runat="server"/>
                    </footer>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
