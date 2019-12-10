<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptSubUnit.aspx.cs" Inherits="FCFDFE.pages.GM.DeptSubUnit" %>

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
    <style>
        th{
            background-color:turquoise;
            color:white;
            font-size:18px;
            font-weight:bold ;
        }
    </style>
    <form id="form1" runat="server">
        <div class="row">
            <div style="width: 95%; margin: auto;">
                <section class="panel">
                    <header class="title">
                        <asp:Label ID="lblTitleUnit" ForeColor="Red" CssClass="control-label" runat="server"></asp:Label>
                        <asp:Label ID="Label2" ForeColor="Red" CssClass="control-label" runat="server">所屬之下轄單位一覽表</asp:Label>
                    </header>
                    <div>
                        <div>
                            <asp:ListView ID="Querylist" runat="server" OnItemDataBound="Querylist_ItemDataBound" OnDataBound="Querylist_DataBound">
                                <layouttemplate>
                                   <table class="table table-bordered text-center">
                                       <tr runat="server">
                                           <th runat="server"><asp:Label CssClass="th_label" runat="server">單位代碼</asp:Label></th>
                                           <th runat="server"><asp:Label CssClass="th_label" runat="server">單位名稱</asp:Label></th>
                                          <th runat="server"><asp:Label CssClass="th_label" runat="server">採購單位</asp:Label></th>
                                          <th runat="server"><asp:Label CssClass="th_label" runat="server">採購上級單位代碼(名稱)<br>所屬之採購單位代碼(名稱)</asp:Label></th>
                                          <th runat="server"><asp:Label CssClass="th_label" runat="server">採購單位類別<br>接轉單位類別</asp:Label></th>
                                           <th runat="server"><asp:Label CssClass="th_label" runat="server">單位狀況</asp:Label></th>
                                       </tr>
                                       <tr runat="server" id="itemPlaceholder" ></tr>
                                       <tr>
                                           <td colspan="5" class="text-right">
                                               <asp:Label ID="Label1" CssClass="control-label" ForeColor="Red" runat="server">合計</asp:Label>
                                           </td>
                                           <td>
                                               <asp:Label ID="lblSum" CssClass="control-label" ForeColor="Red" runat="server"></asp:Label>
                                           </td>
                                       </tr>
                                   </table >
                                   
                               </layouttemplate>
                                   <ItemTemplate>
                                     <tr>
                                     <td><asp:Label CssClass="control-label" runat="server" Text='<%#Eval("OVC_DEPT_CDE")%>'></asp:Label></td>
                                     <td><asp:Label CssClass="control-label" runat="server" Text='<%#Eval("OVC_ONNAME")%>'></asp:Label></td>
                                     <td>
                                        <asp:Label ID="lblPURCHASE_OK" CssClass="control-label" runat="server" Text='<%#Eval("OVC_PURCHASE_OK")%>'></asp:Label>
                                       
                                     </td>
                                     <td>
                                        <asp:Label CssClass="control-label" runat="server" Text='<%#Eval("OVC_TOP_DEPT")%>'></asp:Label>(
                                        <asp:Label CssClass="control-label" ForeColor="Red" runat="server" Text='<%#Eval("OVC_TOP_DEPT_NAME")%>'></asp:Label>)
                                        </br>
                                        <asp:Label CssClass="control-label" runat="server" Text='<%#Eval("OVC_PURCHASE_DEPT")%>'></asp:Label>(
                                        <asp:Label CssClass="control-label" ForeColor="Red" runat="server" Text='<%#Eval("OVC_PURCHASE_DEPT_NAME")%>'></asp:Label>)
                                     </td>
                                     <td>
                                        <asp:Label CssClass="control-label" runat="server" Text='<%#Eval("OVC_CLASS_NAME")%>'></asp:Label>
                                        </br>
                                        <asp:Label CssClass="control-label" runat="server" Text='<%#Eval("OVC_CLASS2_NAME")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_ENABLE" CssClass="control-label" runat="server" Text='<%#Eval("OVC_ENABLE")%>'></asp:Label>
                                    </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                    <footer class="panel-footer text-center">
                    </footer>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
