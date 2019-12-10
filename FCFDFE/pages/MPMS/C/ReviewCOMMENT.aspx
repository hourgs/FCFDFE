<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewCOMMENT.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.ReviewCOMMENT" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<<head runat="server">
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
    <div style=" position:relative;">
    <form id="form1" style="overflow:auto;" runat="server">
        <div class="row">
            <div style="width: 95%; margin: auto;">
                <section class="panel">
                    <header class="title">
                        <asp:Label ID="lblTitleUnit" CssClass="control-label" runat="server"></asp:Label>
                        <asp:Label ID="Label2" CssClass="control-label" runat="server">採購計畫申請書</asp:Label>
                    </header>
                      <table  class="table table-bordered">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" CssClass="control-label" runat="server">購案編號</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" CssClass="control-label" runat="server">審查次數</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" CssClass="control-label" runat="server">分派日</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" CssClass="control-label" runat="server">確認審</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" CssClass="control-label" runat="server">審查綜簽日</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" CssClass="control-label" runat="server">主辦單位</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" CssClass="control-label" runat="server">主辦人</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                <asp:Label ID="lblcheckStatus" CssClass="control-label" ForeColor="Red" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCHECK_TIMES" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDRECIVE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCHECK_OK" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDRESULT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCHECK_UNIT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblOVC_CHECKER" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                           <tr>
                         <td>
                             <asp:Label ID="Label11" ForeColor="Red" CssClass="control-label" runat="server">擬辦事項</asp:Label>
                         </td>
                         <td colspan="6">
                             <asp:GridView ID="GV_ADVICE" CssClass="table table-bordered" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="擬辦項目" >
                                    <ItemTemplate>
                   		                <asp:Label ID="lblOVC_ITEM" CssClass="control-label" Text='<%# Bind("OVC_ITEM")%>' runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_ITEM_ADVICE" CssClass="control-label" Text='<%# Bind("OVC_ITEM_ADVICE")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField> 
                                    <asp:BoundField HeaderText="說明" DataField="OVC_ITEM_DESC" />
                	            </Columns>
	   		                 </asp:GridView>
                           </td>
                        </tr>
                        <tr>
                            <td>
                                 <asp:Label ID="Label3" ForeColor="Red" CssClass="control-label" runat="server">綜審意見</asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:Label ID="lblAllComment"  runat="server"></asp:Label>
                            </td>
                        </tr>
                          <tr>
                            <td>
                                 <asp:Label ID="Label6" ForeColor="Red" CssClass="control-label" runat="server">核定事項</asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:Label ID="lblApproveComent"  runat="server"></asp:Label>
                            </td>
                        </tr>
                     </table>
                     <br/>
                    <div><asp:Label ID="Label14" ForeColor="Red" CssClass="control-label" runat="server">委購單位已上傳檔案</asp:Label></div>
                      <asp:GridView ID="GV_ATTACH" CssClass="table data-table table-striped border-top text-center" DataKeyNames="OVC_PURCH"
                        AutoGenerateColumns="false" runat="server">
                        <Columns>
                            <asp:BoundField HeaderText="主件名稱" DataField="OVC_IKIND" />
                            <asp:BoundField HeaderText="附件名稱" DataField="OVC_ATTACH_NAME" />
                            <asp:BoundField HeaderText="份數" DataField="ONB_QTY" />
                            <asp:BoundField HeaderText="相對之上傳檔案" DataField="OVC_FILE_NAME" />
                            <asp:BoundField HeaderText="頁數" DataField="ONB_PAGES" />
                        </Columns>
                    </asp:GridView>
                    <asp:Repeater ID="Repeater_Header"  OnItemDataBound="Repeater_Header_ItemDataBound" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table class="table table-bordered table-striped">
                                        <tr>
                                            <td colspan="4" style="text-align:center">
                                                <asp:HiddenField id ="hidOVC_AUDIT_UNIT" Value='<%# Bind("OVC_AUDIT_UNIT")%>' runat="server" />
                                                <asp:Label CssClass="control-label" Text="審查單位--" runat="server"></asp:Label>
                                                <asp:Label ID="lblUnitName" CssClass="control-label" Text='<%# Bind("OVC_USR_ID")%>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text="(審查者： " runat="server"></asp:Label>
                                                <asp:Label ID="lblName" CssClass="control-label" Text='<%# Bind("OVC_AUDITOR")%>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text=") -- 電話：" runat="server"></asp:Label>
                                                <asp:Label ID="lblPhone" CssClass="control-label" Text='<%# Bind("IUSER_PHONE") %>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text="作業天數：" runat="server"></asp:Label>
                                                <asp:Label ID="lblPROCESSDATE" CssClass="control-label" Text='<%# Bind("PROCESS") %>' runat="server"></asp:Label>
                                             </td>
                                         </tr>
                                      <asp:Repeater ID="Repeater_Content" OnItemDataBound="Repeater_Content_ItemDataBound"  runat="server">
                                       <HeaderTemplate>
                                       </HeaderTemplate>
                                        <ItemTemplate>
                                                 <tr>
                                                   <td rowspan="2" style="width:12%">
                                                       <asp:Label CssClass="control-label" Text="審查意見(" runat="server"></asp:Label>
                                                       <asp:Label ID ="lblNO" CssClass="control-label" Text='<%# Bind("ONB_NO") %>' runat="server"></asp:Label>
                                                       <asp:Label CssClass="control-label" Text=")" runat="server"></asp:Label>
                                                   </td>
                                                   <td style="width:38%">
                                                       <asp:Label ID="lblOVC_CONTENT" CssClass="control-label" Text='<%# Bind("OVC_CONTENT") %>' runat="server"></asp:Label>
                                                   </td>
                                                   <td id="cellTitle" rowspan="2" style="width:10%" runat="server">
                                                       <asp:Label CssClass="control-label" Text="澄覆意見：" runat="server"></asp:Label>
                                                   </td>
                                                   <td id="cellContent" rowspan="2" style="width:40%" runat="server">
                                                       <asp:Label ID="OVC_CHECK_REASON" CssClass="control-label" Text='<%# Bind("OVC_RESPONSE") %>' runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                                <tr>
                                                   <td style="width:38%">
                                                       <asp:Label ID="lblOVC_RESPONSE" CssClass="control-label" Text='<%# Bind("OVC_CHECK_REASON") %>' runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                      </asp:Repeater>
                                    </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                    <footer class="panel-footer" style="text-align: center;">
                    </footer>
                </section>
            </div>
        </div>
    </form>
    </div>
</body>
</html>
