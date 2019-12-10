<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryCompany.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.QueryCompany" %>

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

    <title>單位查詢</title>

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


</head>
<body>
    <script>
        function reval() {
            window.close();
        }
    </script>
    <script>
        function reval2(ventitle, vencst, venaddress) {
            var NAME = '<%=NAME%>';
            var CST = '<%=CST%>';
            var ADDRESS = '<%=ADDRESS%>';

            var Title = ventitle;
            var Cst = vencst;
            var Address = venaddress;

            if (window.opener.document.getElementById(NAME) != null)
                window.opener.document.getElementById(NAME).value = Title;
            if (window.opener.document.getElementById(CST) != null)
                window.opener.document.getElementById(CST).value = Cst;
            if (window.opener.document.getElementById(ADDRESS) != null)
                window.opener.document.getElementById(ADDRESS).value = Address;
            
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
                        <div class="form" style="text-align: center;">
                            <div class="cmxform form-horizontal tasi-form">
                                <header class="title">
                                    <asp:Label runat="server" Text="本合約之合約商資料"></asp:Label><br />
                                </header>
                                <br />
                                <asp:Button OnClientClick="reval()"  CssClass="btn-warning" runat="server" Text="離開"/>
                                <br />
                                <asp:GridView ID="GVTBGMT_1302" DataKeyNames="OVC_PURCH" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowDataBound="GVTBGMT_1302_RowDataBound" OnPreRender="GVTBGMT_1302_PreRender" runat="server">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnSelect" Text="選擇" CssClass="btn-default" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="合約編號" DataField="OVC_PURCH_6" />
                                        <asp:BoundField HeaderText="合約商統一編號" DataField="OVC_VEN_CST" />
                                        <asp:BoundField HeaderText="合約商名稱" DataField="OVC_VEN_TITLE" />
                                        <asp:BoundField HeaderText="合約商電話" DataField="OVC_VEN_TEL" />
                                        <asp:BoundField HeaderText="合約商地址" DataField="OVC_ADDRESS" />
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
