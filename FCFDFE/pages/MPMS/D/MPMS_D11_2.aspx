<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D11_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D11_2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>購案各階段資料</title>
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
        function CloseWindow()
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
                        購案各階段資料
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                    <div class="text-center" style="padding-bottom:10px">
                        <asp:Button ID="btnClose" CssClass="btn-default btnw4" OnClick="btnClose_Click" Text="關閉視窗" runat="server"/>
                    </div>
                    <div class="panel-body" style=" border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <!--網頁內容-->
                                <asp:GridView ID="gvSTATUS" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH_A_5" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="收案次數" DataField="ONB_TIMES" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="購案階段" DataField="OVC_STATUS_Desc" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="階段開始日" DataField="OVC_DBEGIN" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField HeaderText="階段結束日" DataField="OVC_DEND" ItemStyle-CssClass="text-center" />
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
