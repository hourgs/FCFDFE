<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="STATUS.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.STATUS" %>

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
                       購案各階段資料
                    </header>
                    <asp:GridView ID="TBM1114" CssClass="table data-table table-striped border-top text-center" DataKeyNames="OVC_PURCH" OnPreRender="GV_OVC_BUDGET_PreRender"
                        AutoGenerateColumns="false" runat="server">
                        <Columns>
                             <asp:TemplateField HeaderText="購案編號" >
                                <ItemTemplate>
                                    <asp:Label ID="OVC_Purch" runat="server" Text='<%# "" +Eval("OVC_PURCH") + Eval("OVC_PUR_AGENCY")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <%--1114--%>
                            <asp:BoundField HeaderText="收購次數" DataField="ONB_TIMES" />
                            <%--1114--%>
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                            <%--1114--%>
                            <asp:BoundField HeaderText="購案階段" DataField="OVC_PHR_DESC" />
                            <%--1114--%>
                            <asp:BoundField HeaderText="階段開始日" DataField="OVC_DBEGIN" />
                            <%--1114--%>
                            <asp:BoundField HeaderText="階段結束日" DataField="OVC_DEND" />
                            <%--1114--%>
                        </Columns>
                    </asp:GridView>
                    <footer class="panel-footer" style="text-align: center;">
                    </footer>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
