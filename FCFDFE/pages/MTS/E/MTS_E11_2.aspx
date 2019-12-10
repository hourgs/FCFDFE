<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E11_2.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E11_2" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
         $(document).ready(function () {
<%--             $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");--%>
         });
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    提單資料-管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:panel ID="panel1" runat="server" >
                                <table class="table table-bordered">
                                    <tr>
                                        <td colspan="3" style="text-align:center;vertical-align:middle;">
                                            <asp:Button ID="btnlast" cssclass="btn-success btnw4" OnClick="btnlast_onclick" runat="server" Text="上一筆" />&nbsp;&nbsp;
                                            <asp:Button ID="btnnext" cssclass="btn-success btnw4" OnClick="btnnext_onclick" runat="server" Text="下一筆" />
                                        </td>
                                        <td colspan="3" style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">目前第</asp:Label>
                                            <asp:Label ID="lblnow" runat="server" Text="0" ForeColor="Red"></asp:Label><asp:Label CssClass="control-label" runat="server">筆/</asp:Label>
                                            <asp:Label CssClass="control-label" runat="server">總共</asp:Label>
                                            <asp:Label ID="lblall" runat="server" Text="0" ForeColor="Red"></asp:Label><asp:Label CssClass="control-label" runat="server">筆</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:Label ID="lblOvcBldNo" CssClass="control-label" runat="server" Text="bldnoLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">承運航商</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcShipCompany" CssClass="tb tb-s" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">海空運別</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcSeaOrAir" CssClass="tb tb-s" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">航機名稱</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcShipName" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">航機航次</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcVoyage" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                           <asp:Label CssClass="control-label" runat="server"> 啟運日期</asp:Label>
                                        </td>
                                        <td colspan="2">         
                                            <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-MM-dd" data-date-viewmode="years">
                                                <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s" runat="server" readonly="true"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                           <asp:Label CssClass="control-label" runat="server"> 啟運港埠</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcStartPort"  CssClass="tb tb-m" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">預估抵運日期</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-MM-dd" data-date-viewmode="years">
                                                <asp:TextBox ID="txtOdtPlnArriveDate"  CssClass="tb tb-s" runat="server" readonly="true"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">實際抵運日期</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-MM-dd" data-date-viewmode="years">
                                                <asp:TextBox ID="txtOdtActArriveDate" CssClass="tb tb-s" runat="server" readonly="true"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">抵運港埠</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcArrivePort" CssClass="tb tb-m" runat="server">
                                            </asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcMilitaryType" CssClass="tb tb-s" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">件數/實際件數</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOnbQuanity" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp/&nbsp
                                            <asp:TextBox ID="txtOnbRealQuanity" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcQuanityUnit" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">體積/實際體積</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOnbVolume" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp/&nbsp
                                            <asp:TextBox ID="txtOnbRealVolume" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcVolumeUnit" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">重量/實際重量</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOnbWeight" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp/&nbsp
                                            <asp:TextBox ID="txtOnbRealWeight" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOvcWeightUnit" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" DataFormatString="{0:#,##0}" runat="server">運費</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOnbCarriage" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">運費幣別</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOnbCarriageCurrency" CssClass="tb tb-s" runat="server">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                        <td colspan="6" style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label"  ID="lblComputeresult" runat="server" Text="computeresultLabel" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
                                            <asp:Label CssClass="control-label"  ID="lblOvcIcsNo" runat="server" Text="ICS_NO"></asp:Label>
                                        </td>
                                    </tr>--%>
                                </table>
                            </asp:panel>
                            <asp:panel ID="Sea_Panel" style="display: none;" runat="server">
                                <table class="table table-bordered">
                                    <tr>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">最後離境/抵運港口</asp:Label>
                                        </td>
                                        <td colspan="2" style="text-align:center;vertical-align:middle;">
                                            <asp:DropDownList ID="DropOvcPortChiName" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                        </td>
                                        <td colspan="2" style="text-align:center;vertical-align:middle; width: 64px;">
                                            <asp:Label CssClass="control-label" ID="txtFINAL_DATE" runat="server">最後離境日期/結關前一日</asp:Label>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <div class="input-append datepicker" >
                                            <asp:TextBox ID="TextFinalDate" CssClass="tb tb-m position-left text-change" FormatString = "{0:yyyy/MM/dd}" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td rowspan="5" style="vertical-align:middle;text-align:justify;text-justify:distribute-all-lines;text-align-last:justify">
                                            <asp:Label CssClass="control-label" runat="server">細    項<br/>(由上而下<br/>依序輸入)</asp:Label>
                                        </td>
                                        <td colspan="2" style="text-align:center;vertical-align:middle; height: 44px;">
                                            <asp:Label CssClass="control-label" runat="server">品名分類</asp:Label>
                                        </td>
                                        <td style="text-align:center;vertical-align:middle; width: 64px; height: 44px;">
                                            <asp:Label CssClass="control-label s" runat="server">體積(噸)</asp:Label>
                                        </td>
                                        <td style="text-align:center;vertical-align:middle; height: 44px;">
                                            <asp:Label CssClass="control-label tb-s" runat="server">重量(噸)</asp:Label>
                                        </td>
                                        <td style="text-align:center;vertical-align:middle; height: 44px;">
                                            <asp:Label CssClass="control-label" runat="server">運費</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align:center;vertical-align:middle;">
                                            <asp:DropDownList CssClass="tb tb-m" ID="DropOvcChiName" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:TextBox ID="Volume1" CssClass="tb tb-s" Text="0" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:TextBox ID="Weight1" CssClass="tb tb-s" Text="0" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="2" style="text-align:left;vertical-align:middle;">
                                            <asp:Label ID="Freight1" runat="server" Text="元" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align:center;vertical-align:middle;">
                                            <asp:DropDownList CssClass="tb tb-m" ID="DropOvcChiName2" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:TextBox ID="Volume2" CssClass="tb tb-s" Text="0" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:TextBox ID="Weight2" CssClass="tb tb-s" Text="0" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="2" style="text-align:left;vertical-align:middle;">
                                            <asp:Label ID="Freight2" runat="server" Text="元"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align:center;vertical-align:middle;">
                                            <asp:DropDownList CssClass="tb tb-m" ID="DropOvcChiName3" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:TextBox ID="Volume3" CssClass="tb tb-s" Text="0" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:TextBox ID="Weight3" CssClass="tb tb-s" Text="0" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="2" style="text-align:left;vertical-align:middle;">
                                            <asp:Label ID="Freight3" runat="server" Text="元"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align:center;vertical-align:middle;">
                                            <asp:DropDownList CssClass="tb tb-m" ID="DropOvcChiName4" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:TextBox ID="Volume4" CssClass="tb tb-s" Text="0" runat="server"></asp:TextBox>
                                        </td>
                                        <td  style="text-align:center;vertical-align:middle;">
                                            <asp:TextBox ID="Weight4" CssClass="tb tb-s" Text="0" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="2" style="text-align:left;vertical-align:middle;">
                                            <asp:Label ID="Freight4" runat="server"  Text="元"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">佔艙費</asp:Label>
                                        </td>
                                        <td colspan="5" style="vertical-align:middle;">
                                            <asp:TextBox ID="textONB_ON_SHIP_QUANTITY"  CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label"  runat="server">(數量) X </asp:Label>
                                            <asp:TextBox ID="textONB_ON_SHIP_COST" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server">(單價) = </asp:Label>
                                            <asp:Label CssClass="control-label" ID="txtONB_ON_SHIP_TOTAL" runat="server">元</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:center;vertical-align:middle;" >
                                             <asp:Button ID="btnShipping" cssclass="btn-success btnw4" OnClick="btnShipping_onclick" runat="server" Text="運費計算" />
                                        </td>
                                        <td colspan="5" style="vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">換算單價 新台幣</asp:Label>
                                            <asp:TextBox ID="txtTotalNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server">元 折扣數:</asp:Label>
                                            <asp:TextBox ID="txtShipDiscount" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td colspan="6">
                                            <asp:Panel ID="massage" runat="server"></asp:Panel>
                                         </td>
                                    </tr>
                                </table>
                            </asp:panel>
                            <asp:Panel ID="Port_Panel" style="display: none;" runat="server">
                                <table class="table table-bordered">
                                    <tr>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:Label  CssClass="control-label" runat="server">合約航線</asp:Label>
                                        </td>
                                        <td colspan="2" style="text-align:left;vertical-align:middle;">
                                            <asp:DropDownList ID="DropOvcShipComapany" CssClass="tb tb-s" runat="server">
                                                <asp:ListItem Text="非合約航線"></asp:ListItem>
                                                <asp:ListItem Text="合約航線"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">經高雄或台北</asp:Label>
                                        </td>
                                        <td colspan="2" style="text-align:left;vertical-align:middle;">
                                            <asp:DropDownList ID="DropPassedBy" CssClass="tb tb-s" runat="server">
                                                <asp:ListItem Text="是"></asp:ListItem>
                                                <asp:ListItem Text="否"></asp:ListItem>
                                            </asp:DropDownList> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">危險品處理費</asp:Label>
                                        </td>
                                        <td colspan="5" style="vertical-align:middle;">
                                            <asp:TextBox ID="DangerousFee" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:DropDownList ID="DropOvcCurrencyCode" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:center;vertical-align:middle;">
                                            <asp:Button ID="btnFreight" cssclass="btn-success btnw4" OnClick="btnFreight_onclick" runat="server" Text="運費計算" />
                                        </td>
                                        <td colspan="5" style="vertical-align:middle;">
                                            <asp:Label CssClass="control-label" runat="server">換算運費 新台幣</asp:Label>
                                            <asp:TextBox ID="totalNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server">元 折扣數:</asp:Label>
                                            <asp:TextBox ID="txtFlyDiscount" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td colspan="6">
                                            <asp:Panel ID="flymassage" runat="server"></asp:Panel>
                                         </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                             <asp:Panel ID="msg" runat="server"></asp:Panel>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning btnw6" runat="server" OnClick="btnSave_onclick" Text="修改運費檔" />
                                <asp:Button ID="btnBring" cssclass="btn-success btnw6" runat="server" OnClick="btnBring_onclick" Text="帶入運費檔" />
                                <asp:Button ID="btnNoneed" cssclass="btn-success btnw6" runat="server" OnClick="btnNoneed_onclick" Text="無須計算運費" /><br /><br />
                                <asp:Button ID="btnHome" cssclass="btn-success btnw4" OnClick="btnHone_onclick" runat="server" Text="回主頁" /> 
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->  
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
