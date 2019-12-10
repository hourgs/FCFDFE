<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D37_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D37_2" %>



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

    <title>投標商查詢</title>

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
            <div style="width: 800px; margin: auto;">
                <section>
                    <br />
                    <br />
                    <div>
                        <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <div class="form" style="text-align: center;" id="divForm" visible="false" runat="server">
                            <header class="title">
                                <asp:Label runat="server">本案之投標商資料</asp:Label>
                                <asp:Button ID="btnSave" OnClientClick="reval()" CssClass="btn-warning" runat="server" Text="離開" />
                            </header>
                            <div class="cmxform form-horizontal tasi-form">
                                <asp:GridView ID="GridView" CssClass="table data-table table-striped border-top" OnPreRender="GridView_PreRender" AutoGenerateColumns="false" runat="server">
                                    <Columns>

                                        <asp:BoundField HeaderText="投標商統一編號" DataField="OVC_VEN_CST" />
                                        <asp:BoundField HeaderText="投標商名稱" DataField="OVC_VEN_TITLE" />
                                        <asp:BoundField HeaderText="投標商電話" DataField="OVC_VEN_TEL" />
                                        <asp:BoundField HeaderText="投標審查結果" DataField="OVC_RESULT" />
                                    </Columns>
                                </asp:GridView>
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

