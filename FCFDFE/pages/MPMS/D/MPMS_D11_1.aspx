<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D11_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D11_1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>購案移辦資料</title>
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
    

</head>
<body>
    <script>
        function close()
        {
            window.close();
        }
    </script>
    <form id="form1" runat="server">
        <div class="row">
            <div style="width: 1000px; margin:auto;">
                <section class="panel">
                    <header  class="title">
                        <!--標題-->
                        <asp:Label ID="lblOVC_PURCH_A" runat="server"></asp:Label>
                        <asp:Label runat="server">購案移辦資料</asp:Label>
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                    <div class="text-center" style="padding-bottom:10px">
                        <asp:Button ID="btnClose" CssClass="btn-default btnw4" OnClick="btnClose_Click" Text="關閉視窗" runat="server" />
                    </div>
                    <div class="panel-body">
                        <div class="form">
                            <div class="cmxform form-horizontal tasi-form">
                                <asp:GridView ID="gvShiftDo" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH_A" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="移辦日期" DataField="OVC_DATE" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="移辦人" DataField="OVC_USER" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="移辦單位名稱" DataField="OVC_FROM_UNIT_NAME" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="接收單位名稱" DataField="OVC_TO_UNIT_NAME" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="備註" DataField="OVC_REMARK" ItemStyle-CssClass="text-center" />
                                    </Columns>
                                </asp:GridView>
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
