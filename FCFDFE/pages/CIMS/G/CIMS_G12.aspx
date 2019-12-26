<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_G12.aspx.cs" Inherits="FCFDFE.pages.CIMS.G.CIMS_G12" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 600px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>執行現況查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <asp:Panel ID="Panel1" runat="server">
                     <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">輸入欲搜尋的年度執行現況，按下[匯出資料]即可匯出</div>
                            <table class="table table-bordered" style="text-align:left">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtNow" CssClass="tb tb-s" runat="server" MaxLength="2">
                                        </asp:TextBox >
                                        <asp:Label CssClass="control-label" runat="server">&ensp;年度&ensp;國防採購室執行現況表</asp:Label>
                                    </td>
                                    <td class="text-center"><asp:Button ID="btnQuery6" cssclass="btn-success btnw4" runat="server" Text="查詢" OnClick="btnQuery6_Click" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DRECEIVE" CssClass="tb tb-s" runat="server" MaxLength="2">
                                        </asp:TextBox >
                                        <asp:Label CssClass="control-label" runat="server">&ensp;年度&ensp;收辦案數之明細</asp:Label>
                                    </td>
                                    <td class="text-center"><asp:Button ID="btnQuery1" cssclass="btn-success btnw4" runat="server" Text="匯出資料" OnClick="btnQuery1_Click" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DBID1" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox >
                                        <asp:Label CssClass="control-label" runat="server">&ensp;年度&ensp;決標案數之明細</asp:Label>
                                    </td>
                                    <td class="text-center"><asp:Button ID="btnQuery2" cssclass="btn-success btnw4" runat="server" Text="匯出資料" OnClick="btnQuery2_Click" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CONTRACT_START" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox >
                                        <asp:Label CssClass="control-label" runat="server">&ensp;年度&ensp;履約案數之明細</asp:Label>
                                    </td>
                                    <td class="text-center"><asp:Button ID="btnQuery3" cssclass="btn-success btnw4" runat="server" Text="匯出資料" OnClick="btnQuery3_Click" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DOPEN" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox >
                                        <asp:Label CssClass="control-label" runat="server">&ensp;年度&ensp;未決標購案統計表</asp:Label>
                                    </td>
                                    <td class="text-center"><asp:Button ID="btnQuery4" cssclass="btn-success btnw4" runat="server" Text="匯出資料" OnClick="btnQuery4_Click" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DBID2" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox >
                                        <asp:Label CssClass="control-label" runat="server">&ensp;年度&ensp;決標購案統計表</asp:Label>
                                    </td>
                                    <td class="text-center"><asp:Button ID="btnQuery5" cssclass="btn-success btnw4" runat="server" Text="匯出資料" OnClick="btnQuery5_Click" /></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" Visible="false">
                    <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">
                                <asp:Label ID="Title1" runat="server" Text="Label"></asp:Label></div>
                            <table class="table table-bordered" style="text-align:left">
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">單位</asp:Label>
                                    </td>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">代字</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收辦案數</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">決標案數</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">履約案數</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">決標率(%)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">陸軍總部</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">TA~TZ</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_TAtoTZ_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_TAtoTZ_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_TAtoTZ_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_TAtoTZ_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">海軍總部</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">PA~PZ</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_PAtoPZ_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_PAtoPZ_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_PAtoPZ_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_PAtoPZ_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">空軍總部</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">EA~EZ</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_EAtoEZ_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_EAtoEZ_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_EAtoEZ_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_EAtoEZ_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">聯勤司令部</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">FA~FZ<br/>GA~GZ</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_FAtoFZandGAtoGZ_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_FAtoFZandGAtoGZ_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_FAtoFZandGAtoGZ_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_FAtoFZandGAtoGZ_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">後備司令部</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">HA</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HA_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HA_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HA_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HA_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">憲兵司令部</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">HB</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HB_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HB_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HB_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HB_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">採購室</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">AG</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_AG_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_AG_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_AG_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_AG_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">生產製造中心</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">JA~JZ</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_JAtoJZ_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_JAtoJZ_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_JAtoJZ_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_JAtoJZ_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">規格鑑測中心</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">LA~LZ</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_LAtoLZ_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_LAtoLZ_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_LAtoLZ_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_LAtoLZ_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">中科院</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">XA~XZ<br/>YA~YZ<br/>BA~BZ</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_XAtoXZandYAtoYZandBAtoBZ_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_XAtoXZandYAtoYZandBAtoBZ_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_XAtoXZandYAtoYZandBAtoBZ_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_XAtoXZandYAtoYZandBAtoBZ_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">軍事情報局</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">HI</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HI_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HI_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HI_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HI_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">電訊發展室</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">HP</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HP_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HP_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HP_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HP_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">通訊資訊指揮部</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">HK</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HK_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HK_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HK_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_HK_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">中央單位</asp:Label>
                                    </td>
                                    <td style="background-color:gray">
                                        <asp:Label CssClass="control-label" runat="server">AA~AE、AI、AK<br/>AM、AO~AR<br/>CA~CH、CJ</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_other_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_other_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_other_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txt_other_4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center" style="background-color:gray" colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">合計</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txttotal_1"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txttotal_2"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txttotal_3"></asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server" ID="txttotal_4"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="panel-footer" style="text-align: center;">
                                <!--網頁尾-->
                                <asp:Button ID="btnRETURN" CssClass="btn-warning btnw2" runat="server" Text="返回" OnClick="btnRETURN_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                </asp:Panel>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
