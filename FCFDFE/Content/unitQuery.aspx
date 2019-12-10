<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="unitQuery.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.unitQuery" %>

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

    <title>單位查詢</title>

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
    
    <script src="<%=ResolveClientUrl("~/assets/js/jquery-3.2.1.js")%>"></script>

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
      <script src="~/assets/js/html5shiv.js"></script>
      <script src="~/assets/js/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <script>
        function reval() {
            //var thedrpQuery = document.getElementById("<%=drpQuery.ClientID%>");
            //var selectedOption = $("#<%=drpQuery.ClientID%> option:selected");
            var queryValue = document.getElementById("<%=txtValue.ClientID%>").value;
            var queryText = document.getElementById("<%=txtText.ClientID%>").value;
            var unitsession = '<%= Session["unitquery"] == null ? "query1" : Session["unitquery"].ToString()%>';
            var CDE = '<%=CDE%>';
            var NAME = '<%=NAME%>';

            window.opener.document.getElementById('MainContent_' + CDE).value = queryValue;
            window.opener.document.getElementById('MainContent_' + NAME).value = queryText;

            //if ((CDE != "undefined" & CDE != "") & (NAME != "undefined" & NAME != "")) {
            //    window.opener.document.getElementById('MainContent_' + CDE).value = queryValue;
            //    window.opener.document.getElementById('MainContent_' + NAME).value = queryText;
            //}
            //else {
            //    if (unitsession == "query1") {
            //        window.opener.document.getElementById('MainContent_txtOVC_DEPT_CDE').value = queryValue;
            //        window.opener.document.getElementById('MainContent_txtOVC_ONNAME').value = queryText;
            //    }
            //    else if (unitsession == "query2") {
            //        window.opener.document.getElementById('MainContent_txtOVC_AUDIT_UNIT').value = queryValue;
            //        window.opener.document.getElementById('MainContent_txtOVC_AUDIT_UNIT_1').value = queryText;
            //    }
            //    else if (unitsession == "query3") {
            //        window.opener.document.getElementById('MainContent_txtOVC_PURCHASE_UNIT').value = queryValue;
            //        window.opener.document.getElementById('MainContent_txtOVC_PURCHASE_UNIT_1').value = queryText;
            //    }
            //    else if (unitsession == "query4") {
            //        window.opener.document.getElementById('MainContent_txtOVC_CONTRACT_UNIT').value = queryValue;
            //        window.opener.document.getElementById('MainContent_txtOVC_CONTRACT_UNIT_1').value = queryText;
            //    }
            //    else if (unitsession == "query5") {
            //        window.opener.document.getElementById('MainContent_txtOVC_AGENT_UNIT').value = queryValue;
            //        window.opener.document.getElementById('MainContent_txtOVC_AGENT_UNIT_exp').value = queryText;
            //    } else {
            //        window.opener.document.getElementById('MainContent_txtOVC_DEPT_CDE').value = queryValue;
            //        window.opener.document.getElementById('MainContent_txtOVC_ONNAME').value = queryText;
            //    }
            //}
            window.close();
        }
   </script>
    <form id="form1" runat="server" >
        <div class="row">
        <div style="width: 600px; margin:auto;">
            <section>
                <div>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <div class="form" style="text-align: center;">
                            <div class="cmxform form-horizontal tasi-form">
                                <asp:TextBox ID="txtValue" CssClass="tb tb-s" hidden="true" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtText" CssClass="tb tb-s" hidden="true" runat="server"></asp:TextBox>
                                <table class="text-left" style="margin-top: 20px; line-height: 45px;">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">代碼(編號)：</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOVC_DEPT_CDE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                        <td rowspan="2">
                                            <asp:Button OnClick="btnQuery_Click" CssClass="btn-warning" runat="server" Text="查詢" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">中文名稱：</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                        <%--<td>
                                            <asp:Button ID="btnQuery" OnClick="btnQuery_Click" cssclass="btn-warning" runat="server" Text="名稱查詢"/>&nbsp;&nbsp;
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DropDownList ID="drpQuery" CssClass="tb tb-l" OnSelectedIndexChanged="drpQuery_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <%--<asp:Button OnClientClick="reval()" OnClick="btnSave_Click" CssClass="btn-warning" runat="server" Text="確定"/>--%>
                                            <asp:Button OnClientClick="reval()" CssClass="btn-warning" runat="server" Text="確定"/>
                                        </td>
                                    </tr>
                                </table>
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