<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PORTLIST.aspx.cs" Inherits="FCFDFE.pages.MTS.A.PORTLIST" %>

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

    <title>港埠查詢</title>

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

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
      <script src="~/assets/js/html5shiv.js"></script>
      <script src="~/assets/js/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <script>
        function reval() {
            //var thedrpQuery = document.getElementById("<%=drpQueryCHI.ClientID%>");
            var drpQueryText = document.getElementById("drpQueryText");
            var drpQueryValue = document.getElementById("drpQueryValue");

            window.opener.document.getElementById('MainContent_txtOVC_CHI_NAME').value = drpQueryText.value;
            window.opener.document.getElementById('MainContent_txtOVC_PORT_CDE').value = drpQueryValue.value;
            window.close();
        }
        //function reval2() {
            //var thedrpQuery = document.getElementById("<%=drpQueryENG.ClientID%>");
                    //window.opener.document.getElementById('MainContent_txtOVC_CHI_NAME').value = drpQueryText2.value;
                    //window.opener.document.getElementById('MainContent_txtOVC_PORT_CDE').value = drpQueryValue2.value;
                    //window.close();
                //}
   </script>
    <form id="form1" runat="server" >
        <div class="row">
        <div style="width: 540px; margin:auto;">
            <section>
                <div>
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                            <asp:TextBox ID="drpQueryText" CssClass="tb tb-s" hidden="true" runat="server"></asp:TextBox>
                            <asp:TextBox ID="drpQueryValue" CssClass="tb tb-s" hidden="true" runat="server"></asp:TextBox>
                            <table class="table text-center" style="margin-top: 20px;">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">中文：</asp:Label>
                                        <asp:TextBox ID="txtQueryCHI" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td rowspan="2">
                                        <asp:Button CssClass="btn-warning" OnClick="btnQuery_Click" Text="查詢" runat="server"/>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpQueryCHI" CssClass="tb tb-m" AutoPostBack="true" OnSelectedIndexChanged="drpQuery_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                    </td>
                                    <td rowspan="2">
                                        <asp:Button OnClientClick="reval()" CssClass="btn-warning" runat="server" Text="確定"/><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">英文：</asp:Label>
                                        <asp:TextBox ID="txtQueryENG" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpQueryENG" CssClass="tb tb-m" AutoPostBack="true" OnSelectedIndexChanged="drpQuery_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>

                            <%--<asp:Button ID="btnQuery" OnClick="btnQuery_Click" cssclass="btn-warning" runat="server" Text="查詢"/>&nbsp;&nbsp;
                            <asp:Button ID="btnSave" OnClientClick="reval()" CssClass="btn-warning" runat="server" Text="確定"/><br />
                             <asp:TextBox ID="drpQueryText2" CssClass="tb tb-s" hidden="true" runat="server"></asp:TextBox>
                            <asp:TextBox ID="drpQueryValue2" CssClass="tb tb-s" hidden="true" runat="server"></asp:TextBox>
                            <asp:Button ID="btnQuery2" OnClick="btnQuery2_Click" cssclass="btn-warning" runat="server" Text="查詢"/>&nbsp;&nbsp;
                            <asp:DropDownList ID="drpQuery2" CssClass="tb tb-m" AutoPostBack="true" OnSelectedIndexChanged="drpQuery2_SelectedIndexChanged" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                            <asp:Button ID="btnSave2" OnClientClick="reval2()" CssClass="btn-warning" runat="server" Text="確定"/>--%>
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
