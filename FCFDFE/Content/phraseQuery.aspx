<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="phraseQuery.aspx.cs" Inherits="FCFDFE.Content.phraseQuery" %>

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

    <title>片語查詢</title>

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
        function reval()
        {
            var thedrpQuery = document.getElementById("<%=drpQuery.ClientID%>");
            var phrasesession = '<%= Session["phrasequery"] == null ? "query1": Session["phrasequery"].ToString()%>';

                if (phrasesession == "query1") {
                    window.opener.document.getElementById('MainContent_txtOVC_FCODE_IN').value = drpQueryCode.value;
                }
                else {
                    window.opener.document.getElementById('MainContent_txtOVC_FCODE').value = drpQueryCode.value;
                    window.opener.document.getElementById('MainContent_txtNAME').value = drpQueryText.value;
                }

            window.close();
        }
   </script>
    <form id="form1" runat="server" >
        <div class="row">
        <div style="width: 600px; margin:auto;">
            <section>
                <br /><br />
                <div>
                        <div class="form" style="text-align: center;">
                            <div class="cmxform form-horizontal tasi-form">
                                <table style="text-align: left; line-height: 45px;">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">代碼(編號)查詢&nbsp;</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="drpQueryCode" hidden="true"  CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:TextBox ID="txtCodeQuery" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;</td>
                                        <td>
                                            <asp:Button ID="btnCodeQuery" OnClick="btnCodeQuery_Click" CssClass="btn-warning" runat="server" Text="代碼查詢" />&nbsp;&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">&nbsp;&nbsp;中文名稱查詢&nbsp;</asp:Label></td>
                                        <td><asp:TextBox ID="txtQuery" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="drpQueryText" hidden="true" CssClass="tb tb-s"  runat="server"></asp:TextBox>
                                            <asp:Button ID="btnQuery" OnClick="btnQuery_Click" cssclass="btn-warning" runat="server" Text="名稱查詢"/>&nbsp;&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DropDownList ID="drpQuery" CssClass="tb tb-l" AutoPostBack="true" OnSelectedIndexChanged="drpQuery_SelectedIndexChanged" runat="server"></asp:DropDownList>&nbsp;&nbsp;</td>
                                        <td>
                                            <asp:Button ID="btnSave" OnClientClick="reval()" OnClick="btnSave_Click" CssClass="btn-warning" runat="server" Text="確定"/></td>
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
