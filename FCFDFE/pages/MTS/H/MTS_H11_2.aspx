<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MTS_H11_2.aspx.cs" Inherits="FCFDFE.pages.MTS.H.MTS_H11_2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="Mosaddek" />
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina" />
    <%--<link rel="shortcut icon" href="img/favicon.html">--%>

    <title>國防部</title>

    <!-- Bootstrap core CSS -->
    <link href="~/assets/css/bootstrap.css" rel="stylesheet" />
    <link href="~/assets/css/bootstrap-reset.css" rel="stylesheet" />
    <!--external css-->
    <link href="~/assets/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen"/>
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
    <script type="text/javascript">
        function unLoad()
        {
            //window.opener.location.href = window.opener.location.href;
            window.opener.location.reload();
        }
    </script>
</head>
<body onunload="unLoad();">
    <form id="form1" runat="server">
        <div class="row"> <!--週報表提單查詢-->
            <div style="width: 1000px; margin:auto;">
                <section class="panel">
                    <header class="title">
                        <asp:Label ID="lblDEPT" runat="server"></asp:Label>
                        <asp:Label ID="lblOVC_SECTION" runat="server"></asp:Label>
                        <asp:Label Text="接轉作業週報表" runat="server"></asp:Label>
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                    <div class="panel-body" style=" border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <table class="table table-bordered">
                                    <tr>
                                        <td class="text-center" style="vertical-align: middle;" colspan="2">
                                            <asp:Label CssClass="control-label" runat="server">資料時間</asp:Label>
                                        </td>
                                        <td colspan="7">
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_WEEK_DATE1" CssClass="tb tb-date" AutoPostBack="true" OnTextChanged="txtODT_WEEK_DATE1_TextChanged" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <%--<asp:TextBox ID="txtOdtWeekDate1" CssClass="tb tb-s" runat="server"  AutoPostBack="true" OnTextChanged="txtOdtWeekDate1_TextChanged"></asp:TextBox>--%>
                                            <asp:Label CssClass="control-label" Text="(週三)" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text=" - " runat="server"></asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_WEEK_DATE2" CssClass="tb tb-date" AutoPostBack="true" OnTextChanged="txtODT_WEEK_DATE2_TextChanged" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <%--<asp:TextBox ID="txtOdtWeekDate2" CssClass="tb tb-s" runat="server" AutoPostBack="true" OnTextChanged="txtOdtWeekDate2_TextChanged"></asp:TextBox>--%>
                                            <asp:Label CssClass="control-label" Text="(週二)" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" runat="server">（</asp:Label>
                                            <%--<asp:TextBox  ID="txtOdtMonth" CssClass="tb tb-xs" runat="server"></asp:TextBox>--%>
                                            <asp:Label ID="lblODT_MONTH" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" runat="server">月份</asp:Label>&nbsp;&nbsp;
                                            <asp:Label CssClass="control-label" runat="server">第</asp:Label>
                                            <%--<asp:TextBox  ID="txtOdtWeek" CssClass="tb tb-xs" runat="server"></asp:TextBox>--%>
                                            <asp:Label ID="lblODT_WEEK" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" runat="server">週）</asp:Label>
                                            <asp:Button cssclass="btn-success btnw2" Text="上週" OnClick="Button_lastweek_Click" runat="server" /> 
                                            <asp:Button cssclass="btn-success btnw2" Text="下週" OnClick="Button_nextweek_Click" runat="server" /> 
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="GV_BLD" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnRowCommand="GV_BLD_RowCommand" OnPreRender="GV_BLD_PreRender" runat="server">
                                    <Columns>
                                        <asp:BoundField DataField="OVC_BLD_NO" HeaderText="提單編號" />
                                        <asp:BoundField DataField="OVC_CHI_NAME" HeaderText="品名" />
                                        <asp:BoundField DataField="ONB_QUANITY" HeaderText="箱件" />
                                        <asp:BoundField DataField="ONB_VOLUME" HeaderText="噸位" />
                                        <asp:BoundField DataField="OVC_RECEIVE_DEPT" HeaderText="接收單位" />
                                        <asp:BoundField DataField="ODT_IMPORT_DATE" HeaderText="進口日期" />
                                        <asp:BoundField DataField="ODT_PASS_DATE" HeaderText="通關日期" />
                                        <asp:BoundField DataField="ODT_STORED_DATE" HeaderText="進倉日期" />
                                        <asp:BoundField DataField="ODT_TRANSFER_DATE" HeaderText="清運日期" />
                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:Button CssClass="btn-success btnw2" Text="新增" CommandName="btnNew" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
    
    <!-- js placed at the end of the document so the pages load faster -->
    <%--<script src="<%=ResolveClientUrl("~/assets/js/jquery.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery-1.8.3.min.js")%>"></script>--%>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery-3.2.1.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/bootstrap.min.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery.scrollTo.min.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery.nicescroll.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveClientUrl("~/assets/js/jquery.sparkline.js")%>" type="text/javascript"></script>
    <script src="<%=ResolveClientUrl("~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/owl.carousel.js")%>" ></script>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery.customSelect.min.js")%>" ></script>
    
    <!--custom switch-->
    <script src="<%=ResolveClientUrl("~/assets/js/bootstrap-switch.js")%>"></script>
    <!--custom tagsinput-->
    <script src="<%=ResolveClientUrl("~/assets/js/jquery.tagsinput.js")%>"></script>
    <!--custom checkbox & radio-->
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/js/ga.js")%>"></script>
    <!--picker-->
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-datepicker/js/bootstrap-datepicker.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-daterangepicker/date.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-daterangepicker/daterangepicker.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-colorpicker/js/bootstrap-colorpicker.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/ckeditor/ckeditor.js")%>"></script>
    <!--dataTable-->
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/data-tables/jquery.dataTables.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/data-tables/DT_bootstrap.js")%>"></script>
    <!--fileinput-->
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/js/bootstrap-fileinput.js")%>"></script>

    <!--common script for all pages-->
    <script src="<%=ResolveClientUrl("~/assets/js/common-scripts.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/common.js")%>"></script>
    <!--script for this page only-->
    <script src="<%=ResolveClientUrl("~/assets/js/dynamic-table.js")%>"></script>
    <!--script for this page-->
    <script src="<%=ResolveClientUrl("~/assets/js/sparkline-chart.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/easy-pie-chart.js")%>"></script>
    <script src="form-component.js"></script>
</body>
</html>
