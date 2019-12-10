<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MPMS_D11_3.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D11_3" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>購案撤案作業</title>
    <!-- Bootstrap core CSS -->
    <link href="~/assets/css/bootstrap.css" rel="stylesheet"/>
    <link href="~/assets/css/bootstrap-reset.css" rel="stylesheet"/>
    <!--external css-->
    <link href="~/assets/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="~/assets/css/owl.carousel.css" rel="stylesheet" type="text/css"/>
    <!--picker-->
    <link rel="stylesheet" type="text/css" href="/assets/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="/assets/assets/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css" />
    <link rel="stylesheet" type="text/css" href="/assets/assets/bootstrap-colorpicker/css/colorpicker.css" />
    <link rel="stylesheet" type="text/css" href="/assets/assets/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- Custom styles for this template -->
    <link href="/assets/css/style.css" rel="stylesheet"/>
    <link href="/assets/css/style-responsive.css" rel="stylesheet" />
    

</head>
<body>
    <form id="form2" runat="server">
        <div class="row">
            <div style="width: 1000px; margin:auto;">
                <section class="panel">
                    <header  class="title">
                        <!--標題-->
                        購案撤案作業
                    </header>
                    <div class="panel-body" style=" border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <!--網頁內容-->
                                <div>
                                    <table class="table table-bordered" style="text-align:center">
                                        <tr>
                                            <td><asp:Label CssClass="control-label" Text="計畫年度(第二組)：" runat="server"></asp:Label>
                                                <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                                                <asp:Button ID="btnQuery_OVC_BUDGET_YEAR" cssclass="btn-default btnw2" runat="server" OnClick="btnQuery_OVC_BUDGET_YEAR_Click" Text="查詢" />&nbsp;&nbsp;
                                                <asp:Label CssClass="control-label" Text="目前已撤案資料" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <div class="cmxform form-horizontal tasi-form">
                                        <!--網頁內容-->
                                        <asp:GridView ID="gvRevoked" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                            <Columns>
                                                <asp:HyperLinkField DataNavigateUrlFields="" DataTextField="OVC_PURCH_A" HeaderText="購案編號" />
                                                <asp:HyperLinkField DataNavigateUrlFields="" DataTextField="OVC_PUR_IPURCH" HeaderText="購案名稱" />
                                                <asp:TemplateField HeaderText="委購單位">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_PUR_NSECTION" Text='<%# Eval("OVC_PUR_NSECTION") %>' runat="server"></asp:Label><br />
                                                        撤案日：<asp:Label ID="lblOVC_PUR_DCANPO" CssClass="control-label text-red" Text='<%# Eval("OVC_PUR_DCANPO") %>' runat="server"></asp:Label><br />
                                                        撤案原因：<asp:Label ID="lblOVC_PUR_DCANRE" CssClass="control-label text-red" Text='<%# Eval("OVC_PUR_DCANRE") %>' runat="server" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="審查總次數" DataField="ONB_CHECK_TIMES" ItemStyle-CssClass="text-center" />
                                                <asp:BoundField HeaderText="最後計評承辦人" DataField="OVC_ASSIGNER" ItemStyle-CssClass="text-center" />
                                                <asp:TemplateField HeaderText="購案最後階段">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_STATUS_Desc" Text='<%# Eval("OVC_STATUS_Desc") %>' runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_STATUS" Text='<%# Eval("OVC_STATUS") %>' Visible="false" runat="server"></asp:Label>
                                                        <asp:Label ID="lblOVC_REMARK" CssClass="control-label text-red" Text='<%# Eval("OVC_REMARK") %>' runat="server" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
