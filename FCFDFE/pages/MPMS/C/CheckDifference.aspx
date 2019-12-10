<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckDifference.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.CheckDifference" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
                        <asp:Label ID="lblOVC_PURCH" Font-Size="X-Large" CssClass="control-label" runat="server"></asp:Label>
                        <asp:Label ID="Label5" Font-Size="X-Large" CssClass="control-label" runat="server">澄覆後比較表</asp:Label>
                    </header>
                    <table id="table1301_History" class=" table data-table" runat="server">
                        <tr>
                            <td colspan="3" class="text-center">
                               <asp:Label ID="Label4"  CssClass="control-label" ForeColor="Red" runat="server">購案基本資料</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="GV_1301_History" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="欄位名稱" HeaderStyle-ForeColor="Red" DataField="ColumnName" />
                                        <asp:BoundField HeaderText="舊版本資料" HeaderStyle-ForeColor="Red" DataField="OldData" />
                                        <asp:BoundField HeaderText="變更後資料" HeaderStyle-ForeColor="Red" DataField="NewData" />
                	                </Columns>
	   		                     </asp:GridView>
                            </td>
                        </tr>
                      </table>
                     <br/>
                     <asp:Repeater ID="rptISOURCE"  runat="server" OnItemDataBound="rptISOURCE_ItemDataBound" >
                    <HeaderTemplate>
                        <table class="table table-bordered table-striped  text-center">
                            <tr>
                                <th colspan="8" class="text-center"> <asp:Label ID="Label4"  CssClass="control-label" ForeColor="Red" runat="server">購案預算主檔</asp:Label></th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th ><asp:Label ID="Label5"  CssClass="control-label" ForeColor="Red" runat="server">變更前後</asp:Label></th>
                            <th ><asp:Label ID="Label1"  CssClass="control-label" ForeColor="Red" runat="server">款源名稱</asp:Label></th>
                            <th ><asp:Label ID="Label2"  CssClass="control-label" ForeColor="Red" runat="server">款源類別</asp:Label></th>
                            <th ><asp:Label ID="Label6"  CssClass="control-label" ForeColor="Red" runat="server">奉准日期</asp:Label></th>
                            <th ><asp:Label ID="Label3"  CssClass="control-label" ForeColor="Red" runat="server">奉准文號</asp:Label></th>
                            <th ><asp:Label ID="Label10"  CssClass="control-label" ForeColor="Red" runat="server">幣別</asp:Label></th>
                            <th ><asp:Label ID="Label14"  CssClass="control-label" ForeColor="Red" runat="server">匯率</asp:Label></th>
                            <th ><asp:Label ID="Label15"  CssClass="control-label" ForeColor="Red" runat="server">金額小計</asp:Label></th>
                        </tr>
                        <tr id="trBefore" runat="server">
                            <td>
                                <asp:Label ID="lbl1" runat="server" ></asp:Label>變更前</td>
                            <td>
                                <asp:Label ID="lbl2" runat="server" Text='<%# Eval("OVC_ISOURCE_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl3" runat="server" Text='<%# Eval("OVC_IKIND_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lblMemoOri" runat="server" Text='<%# Eval("OVC_PUR_DAPPR_PLAN_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text='<%# Eval("OVC_PUR_APPR_PLAN_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text='<%# Eval("OVC_PHR_DESC_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text='<%# Eval("ONB_RATE_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text='<%# Eval("ONB_MONEY_OLD") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblChangeType" runat="server" Text="變更後" ></asp:Label></td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text='<%# Eval("OVC_ISOURCE_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text='<%# Eval("OVC_IKIND_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text='<%# Eval("OVC_PUR_DAPPR_PLAN_NEW") %>'></asp:Label></td>
                             <td>
                                <asp:Label ID="Label20" runat="server" Text='<%# Eval("OVC_PUR_APPR_PLAN_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text='<%# Eval("OVC_PHR_DESC_NEW") %>'></asp:Label></td>
                             <td>
                                <asp:Label ID="Label22" runat="server" Text='<%# Eval("ONB_RATE_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label23" runat="server" Text='<%# Eval("ONB_MONEY_NEW") %>'></asp:Label></td>
                        </tr>
                        </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                    <br/>
                    <asp:Repeater ID="rptDeatilBudget"  runat="server" OnItemDataBound="rptDeatilBudget_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-bordered table-striped  text-center">
                            <tr>
                                <th colspan="8" class="text-center"> <asp:Label ID="Label4"  CssClass="control-label" ForeColor="Red" runat="server">購案預算明細檔</asp:Label></th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th ><asp:Label ID="Label5"  CssClass="control-label" ForeColor="Red" runat="server">變更前後</asp:Label></th>
                            <th ><asp:Label ID="Label1"  CssClass="control-label" ForeColor="Red" runat="server">款源名稱</asp:Label></th>
                            <th ><asp:Label ID="Label2"  CssClass="control-label" ForeColor="Red" runat="server">款源類別</asp:Label></th>
                            <th ><asp:Label ID="Label6"  CssClass="control-label" ForeColor="Red" runat="server">預算科目代號</asp:Label></th>
                            <th ><asp:Label ID="Label3"  CssClass="control-label" ForeColor="Red" runat="server">預算科目名稱</asp:Label></th>
                            <th ><asp:Label ID="Label10"  CssClass="control-label" ForeColor="Red" runat="server">預劃年度</asp:Label></th>
                            <th ><asp:Label ID="Label14"  CssClass="control-label" ForeColor="Red" runat="server">預劃月份</asp:Label></th>
                            <th ><asp:Label ID="Label15"  CssClass="control-label" ForeColor="Red" runat="server">預劃金額</asp:Label></th>
                        </tr>
                        <tr id="trBefore" runat="server">
                            <td>
                                <asp:Label ID="lbl1" runat="server" ></asp:Label>變更前</td>
                            <td>
                                <asp:Label ID="lbl2" runat="server" Text='<%# Eval("OVC_ISOURCE_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl3" runat="server" Text='<%# Eval("OVC_IKIND_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lblMemoOri" runat="server" Text='<%# Eval("OVC_OVC_POI_IBDG_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text='<%# Eval("OVC_OVC_PJNAME_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text='<%# Eval("OVC_OVC_YY_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text='<%# Eval("ONB_OVC_MM_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text='<%# Eval("ONB_MBUD_OLD") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblChangeType" runat="server" Text="變更後" ></asp:Label></td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text='<%# Eval("OVC_ISOURCE_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text='<%# Eval("OVC_IKIND_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text='<%# Eval("OVC_OVC_POI_IBDG_NEW") %>'></asp:Label></td>
                             <td>
                                <asp:Label ID="Label20" runat="server" Text='<%# Eval("OVC_OVC_PJNAME_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text='<%# Eval("OVC_OVC_YY_NEW") %>'></asp:Label></td>
                             <td>
                                <asp:Label ID="Label22" runat="server" Text='<%# Eval("ONB_OVC_MM_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label23" runat="server" Text='<%# Eval("ONB_MBUD_NEW") %>'></asp:Label></td>
                        </tr>
                        </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                    <br/>
                    <asp:Repeater ID="rptATTACH"  runat="server" OnItemDataBound="rptATTACH_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-bordered table-striped  text-center">
                            <tr>
                                <th colspan="6" class="text-center"> <asp:Label ID="Label4"  CssClass="control-label" ForeColor="Red" runat="server">購案附件檔</asp:Label></th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th ><asp:Label ID="Label5"  CssClass="control-label" ForeColor="Red" runat="server">變更前後</asp:Label></th>
                            <th ><asp:Label ID="Label1"  CssClass="control-label" ForeColor="Red" runat="server">種類</asp:Label></th>
                            <th ><asp:Label ID="Label2"  CssClass="control-label" ForeColor="Red" runat="server">附件名稱</asp:Label></th>
                            <th ><asp:Label ID="Label6"  CssClass="control-label" ForeColor="Red" runat="server">檔案名稱</asp:Label></th>
                            <th ><asp:Label ID="Label3"  CssClass="control-label" ForeColor="Red" runat="server">份數</asp:Label></th>
                            <th ><asp:Label ID="Label10"  CssClass="control-label" ForeColor="Red" runat="server">頁數</asp:Label></th>
                        </tr>
                        <tr id="trBefore" runat="server">
                            <td>
                                <asp:Label ID="lbl1" runat="server" ></asp:Label>變更前</td>
                            <td>
                                <asp:Label ID="lbl2" runat="server" Text='<%# Eval("OVC_IKIND_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl3" runat="server" Text='<%# Eval("OVC_ATTACH_NAME_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lblMemoOri" runat="server" Text='<%# Eval("OVC_FILE_NAME_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text='<%# Eval("ONB_QTY_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text='<%# Eval("ONB_PAGES_OLD") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblChangeType" runat="server" Text="變更後" ></asp:Label></td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text='<%# Eval("OVC_IKIND_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text='<%# Eval("OVC_ATTACH_NAME_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text='<%# Eval("OVC_FILE_NAME_NEW") %>'></asp:Label></td>
                             <td>
                                <asp:Label ID="Label20" runat="server" Text='<%# Eval("ONB_QTY_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text='<%# Eval("ONB_PAGES_NEW") %>'></asp:Label></td>
                        </tr>
                        </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                    <br/>
                    <asp:Repeater ID="rptNSTUFF"  runat="server" OnItemDataBound="rptNSTUFF_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-bordered table-striped  text-center">
                            <tr>
                                <th colspan="8" class="text-center"> <asp:Label ID="Label4"  CssClass="control-label" ForeColor="Red" runat="server">購案明細檔</asp:Label></th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th ><asp:Label ID="Label5"  CssClass="control-label" ForeColor="Red" runat="server">變更前後</asp:Label></th>
                            <th ><asp:Label ID="Label1"  CssClass="control-label" ForeColor="Red" runat="server">項次</asp:Label></th>
                            <th ><asp:Label ID="Label2"  CssClass="control-label" ForeColor="Red" runat="server">其他說明</asp:Label></th>
                            <th ><asp:Label ID="Label6"  CssClass="control-label" ForeColor="Red" runat="server">同等品</asp:Label></th>
                            <th ><asp:Label ID="Label3"  CssClass="control-label" ForeColor="Red" runat="server">中文品名</asp:Label></th>
                            <th ><asp:Label ID="Label10"  CssClass="control-label" ForeColor="Red" runat="server">英文品名</asp:Label></th>
                            <th ><asp:Label ID="Label14"  CssClass="control-label" ForeColor="Red" runat="server">料號種類</asp:Label></th>
                            <th ><asp:Label ID="Label15"  CssClass="control-label" ForeColor="Red" runat="server">料號</asp:Label></th>
                        </tr>
                        <tr id="trBefore" runat="server">
                            <td>
                                <asp:Label ID="lbl1" runat="server" ></asp:Label>變更前</td>
                            <td>
                                <asp:Label ID="lbl2" runat="server" Text='<%# Eval("ONB_POI_ICOUNT_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lbl3" runat="server" Text='<%# Eval("OVC_POI_NDESC_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="lblMemoOri" runat="server" Text='<%# Eval("OVC_SAME_QUALITY_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text='<%# Eval("OVC_POI_NSTUFF_CHN_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text='<%# Eval("OVC_POI_NSTUFF_ENG_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text='<%# Eval("NSN_KIND_OLD") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text='<%# Eval("NSN_OLD") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblChangeType" runat="server" Text="變更後" ></asp:Label></td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("ONB_POI_ICOUNT_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text='<%# Eval("OVC_POI_NDESC_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text='<%# Eval("OVC_SAME_QUALITY_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label24" runat="server" Text='<%# Eval("OVC_POI_NSTUFF_CHN_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label25" runat="server" Text='<%# Eval("OVC_POI_NSTUFF_ENG_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label26" runat="server" Text='<%# Eval("NSN_KIND_NEW") %>'></asp:Label></td>
                            <td>
                                <asp:Label ID="Label27" runat="server" Text='<%# Eval("NSN_NEW") %>'></asp:Label></td>
                        </tr>
                        </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                    <br/>
                    <asp:Repeater ID="rptMemo"  runat="server" OnItemDataBound="rptMemo_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-bordered table-striped">
                            <tr>
                                <th colspan="4" class="text-center"> <asp:Label ID="Label4"  CssClass="control-label" ForeColor="Red" runat="server">購案請求事項及計畫清單備註</asp:Label></th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th ><asp:Label ID="Label5"  CssClass="control-label" ForeColor="Red" runat="server">變更前後</asp:Label></th>
                            <th ><asp:Label ID="Label1"  CssClass="control-label" ForeColor="Red" runat="server">類別</asp:Label></th>
                            <th ><asp:Label ID="Label2"  CssClass="control-label" ForeColor="Red" runat="server">項次</asp:Label></th>
                            <th ><asp:Label ID="Label6"  CssClass="control-label" ForeColor="Red" runat="server">內容</asp:Label></th>
                        </tr>
                        <tr id="trBefore" runat="server">
                            <td style="width:10%" class="text-center">
                                <asp:Label ID="lbl1" runat="server" ></asp:Label>變更前</td>
                            <td style="width:35%">
                                <asp:Label ID="lbl2" runat="server" Text='<%# Eval("OVC_MEMO_NAME") %>'></asp:Label></td>
                            <td style="width:5%" class="text-center">
                                <asp:Label ID="lbl3" runat="server" Text='<%# Eval("ONB_NO") %>'></asp:Label></td>
                            <td style="width:50%">
                                <asp:Label ID="lblMemoOri" runat="server" Text='<%# Eval("MEMO_ORIGIN") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width:10%" class="text-center">
                                <asp:Label ID="lblChangeType" runat="server" Text="變更後" ></asp:Label></td>
                            <td style="width:35%">
                                <asp:Label ID="Label11" runat="server" Text='<%# Eval("MEMO_NAME_NEW") %>'></asp:Label></td>
                            <td style="width:5%" class="text-center">
                                <asp:Label ID="Label12" runat="server" Text='<%# Eval("ONB_NO_NEW") %>'></asp:Label></td>
                            <td style="width:50%">
                                <asp:Label ID="Label13" runat="server" Text='<%# Eval("MEMO_NEW") %>'></asp:Label></td>
                        </tr>
                        </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
