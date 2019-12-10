<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_H12.aspx.cs" Inherits="FCFDFE.pages.MTS.H.MTS_H12" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <asp:Label ID="lblOVC_SECTION" runat="server"></asp:Label>
                    <asp:Label ID="lblDEPT" runat="server"></asp:Label>
                    <asp:DropDownList  ID="drpOVC_SECTION" CssClass="tb tb-s" runat="server">
                        <asp:ListItem>基隆地區</asp:ListItem>
                        <asp:ListItem>桃園地區</asp:ListItem>
                        <asp:ListItem>高雄分遣組</asp:ListItem>
                        <asp:ListItem Value="採購中心" Selected="True">國防採購室</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label Text="接轉作業月報表" runat="server"></asp:Label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">月份</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList  ID="drpOdtYear" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label textSpace-r" runat="server">年</asp:Label>
                                        <asp:DropDownList  ID="drpOdtMonth" CssClass="tb drp-day" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label textSpace-r" runat="server">月</asp:Label>
                                        <asp:Button cssclass="btn-success btnw2" runat="server" Text="查詢"  OnClick="btnQuery_Click"/>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnData" runat="server">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" style="width:100px;">
                                        <asp:Label CssClass="control-label" runat="server">空運架次</asp:Label>
                                    </td>
                                    <td colspan="2">
                                         <asp:TextBox ID="txtONB_SHIP_AIR" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center" style="width:100px;">
                                        <asp:Label CssClass="control-label" runat="server">海運航次</asp:Label>
                                    </td>
                                    <td colspan="2">
                                         <asp:TextBox ID="txtONB_SHIP_SEA" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center" style="width:100px;">
                                        
                                    </td>
                                    <td colspan="2" class="text-center">
                                         <asp:Button ID="btnStatistic" cssclass="btn-success btnw4" runat="server" Text="重新統計" OnClick="btnStatistic_Click"/> 
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">解密日期</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_OPEN_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">發文日期</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_DOC_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">發文字號</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOVC_DOC_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備註</asp:Label>
                                    </td>
                                    <td colspan="8">
                                        <asp:TextBox ID="txtOVC_NOTE" CssClass="textarea tb-full" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                        <%--<asp:ListBox ID="lstOVC_NOTE" CssClass="tb tb-full" runat="server"></asp:ListBox>--%>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnUpdate" cssclass="btn-success" Text="更新月報表" OnClick="btnUpdate_Click" runat="server"/>
                                <asp:Button ID="btnPrint" cssclass="btn-warning" Text="列印月報表" OnClick="btnPrint_Click" runat="server"/>
                                <asp:Button ID="btnQuery" cssclass="btn-success" Text="差異數查詢" OnClick="btnQueryDiff_Click" runat="server"/>
                            </div>
                            <div class="text-right" style="margin-top: 20px;">
                                <asp:Label CssClass="control-label text-red" style="margin-right: 30px;" Font-Bold="True" runat="server">重量單位：KG，體積單位：CBM</asp:Label>
                                <asp:Button ID="btnStatisticItem" cssclass="btn-success" Visible="false" Text="重新統計細項資料" OnClick="btnStatisticItem_Click" runat="server"/>
                                <asp:Button ID="btnSummaryItem" cssclass="btn-warning" Visible="false" Text="彙總" OnClick="btnSummaryItem_Click" runat="server"/>
                            </div>
                            <asp:GridView ID="GV_TBGMT_MRP_ITEM" CssClass="table data-table table-striped border-top table-bordered" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_MRP_ITEM_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_MILITARY" HeaderText="軍種" />
                                    <asp:BoundField DataField="OVC_IE" HeaderText="進出口" />
                                    <asp:BoundField DataField="ONB_QUANITY_M" HeaderText="軍售件數" />
                                    <asp:BoundField DataField="ONB_WEIGHT_M" HeaderText="軍售重量" />
                                    <asp:BoundField DataField="ONB_VOLUME_M" HeaderText="軍售體積" />
                                    <asp:BoundField DataField="ONB_BLD_M" HeaderText="軍售報關" />
                                    <asp:BoundField DataField="ONB_QUANITY_C" HeaderText="商購件數" />
                                    <asp:BoundField DataField="ONB_WEIGHT_C" HeaderText="商購重量" />
                                    <asp:BoundField DataField="ONB_VOLUME_C" HeaderText="商購體積" />
                                    <asp:BoundField DataField="ONB_BLD_C" HeaderText="商購報關" />
                                    <asp:BoundField DataField="ONB_QUANITY_O" HeaderText="其他件數" />
                                    <asp:BoundField DataField="ONB_WEIGHT_O" HeaderText="其他重量" />
                                    <asp:BoundField DataField="ONB_VOLUME_O" HeaderText="其他體積" />
                                    <asp:BoundField DataField="ONB_BLD_O" HeaderText="其他報關" />
                                </Columns>
                            </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
