<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BLDDATA.aspx.cs" Inherits="FCFDFE.pages.MTS.E.BLDDATA" %>

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
            <div style="width: 500px; margin:auto;">
                <section class="panel">                          
                    <table class="table table-bordered text-center">                             
                        <tr>
                            <td style="width:25%"><asp:Label CssClass="control-label" runat="server">提單編號</asp:Label></td>
                            <td colspan="3" class="text-left" style="width:75%">
                                <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:25%"><asp:Label CssClass="control-label" runat="server">承運航商</asp:Label></td>
                            <td class="text-left" style="width:25%">
                                <asp:Label ID="lblOVC_SHIP_COMPANY" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td style="width:25%"><asp:Label CssClass="control-label" runat="server">海空運別</asp:Label></td>
                            <td class="text-left" style="width:25%">
                                <asp:Label ID="lblOVC_SEA_OR_AIR" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_SHIP_NAME" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td><asp:Label CssClass="control-label" runat="server">船機航次</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_VOYAGE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">啟運日期</asp:Label></td>
                            <td class="text-left">
                                    <asp:Label ID="lblODT_START_DATE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td><asp:Label CssClass="control-label" runat="server">啟運港埠</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_START_PORT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">預估抵運日期</asp:Label></td>
                            <td class="text-left">                                  
                                    <asp:Label ID="lblODT_PLN_ARRIVE_DATE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td><asp:Label CssClass="control-label" runat="server">實際抵運日期</asp:Label>
                            </td>
                            <td class="text-left">
                                    <asp:Label ID="lblODT_ACT_ARRIVE_DATE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">抵運港埠</asp:Label></td>
                            <td colspan="3" class="text-left">
                                <asp:Label ID="lblOVC_ARRIVE_PORT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">件數</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblONB_QUANITY" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td><asp:Label CssClass="control-label" runat="server">計量單位</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_QUANITY_UNIT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">體積/佔艙體積</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblONB_VOLUME" CssClass="control-label" runat="server"></asp:Label>/ 
                                <asp:Label ID="lblONB_ON_SHIP_VOLUME" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td><asp:Label CssClass="control-label" runat="server">計量單位</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_VOLUME_UNIT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">重量</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblONB_WEIGHT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td><asp:Label CssClass="control-label" runat="server">計量單位</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_WEIGHT_UNIT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">運費</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblONB_CARRIAGE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td><asp:Label CssClass="control-label" runat="server">運費幣別</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblONB_CARRIAGE_CURRENCY" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">單位名稱</asp:Label></td>
                            <td colspan="3" class="text-left">
                                <asp:Label ID="lblOVC_MILITARY_TYPE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">資料傳送日期</asp:Label></td>
                            <td colspan="3" class="text-left">
                                <asp:Label ID="lblODT_CREATE_DATE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label CssClass="control-label" runat="server">資料修改日期</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblODT_MODIFY_DATE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td><asp:Label CssClass="control-label" runat="server">資料建立人員</asp:Label></td>
                            <td class="text-left">
                                <asp:Label ID="lblOVC_CREATE_LOGIN_ID" CssClass="control-label" runat="server"></asp:Label>
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
