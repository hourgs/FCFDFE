<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E21_2.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E21_2" %>
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
                    提單資料-管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">      
                                <tr>
                                    <td colspan="3" style="text-align:center;vertical-align:middle;">
                                        <asp:Button ID="btnlast" cssclass="btn-success btnw4" runat="server" Text="上一筆" />&nbsp;&nbsp;
                                        <asp:Button ID="btnnext" cssclass="btn-success btnw4" runat="server" Text="下一筆" />
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
                                        <asp:DropDownList ID="drpOvcShipCompany" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">海空運別</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOvcSeaOrAir" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
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
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                            <asp:Label runat="server" CssClass="control-label position-left"  Text="(日期格式:yyyy-mm-dd)" ForeColor="Red"></asp:Label>
                                        </div>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                       <asp:Label CssClass="control-label" runat="server"> 啟運港埠</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOvcStartPort" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">預估抵運日期</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtPlnArriveDate" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                            <asp:Label runat="server" CssClass="control-label position-left"  Text="(日期格式:yyyy-mm-dd)" ForeColor="Red"></asp:Label>
                                        </div>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">實際抵運日期</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtActArriveDate" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                            <asp:Label runat="server" CssClass="control-label position-left"  Text="(日期格式:yyyy-mm-dd)" ForeColor="Red"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">抵運港埠</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOvcArrivePort" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOvcMilitaryType" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">件數/實際件數</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOnbQuanity" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp/&nbsp
                                        <asp:TextBox  CssClass="tb tb-s" runat="server"></asp:TextBox>
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
                                        <asp:TextBox  CssClass="tb tb-s" runat="server"></asp:TextBox>
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
                                        <asp:TextBox  CssClass="tb tb-s" runat="server"></asp:TextBox>
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
                                        <asp:Label CssClass="control-label" runat="server">運費</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOnbCarriage" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">運費幣別</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOnbCarriageCurrency" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Button ID="btnCalculate" cssclass="btn-success btnw4" runat="server" Text="運費計算" />
                                    </td>
                                    <td colspan="5">
                                        <asp:Label CssClass="control-label" runat="server">換算運費&nbsp;&nbsp;新台幣&nbsp;</asp:Label>
                                        <asp:TextBox ID="txtNTDollar" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">元&nbsp;&nbsp;折扣數:&nbsp;</asp:Label>
                                        <asp:TextBox ID="txtDiscount" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp; %</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label"  ID="lblComputeresult" runat="server" Text="computeresultLabel" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label"  ID="lblOvcIcsNo" runat="server" Text="ICS_NO"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Label ID="lblfotter" CssClass="control-label text-red"  runat="server" Text="Label" ></asp:Label>&nbsp;
                                <asp:Button ID="btnSave" cssclass="btn-warning btnw6" runat="server" Text="修改運費檔" />
                                <asp:Button ID="btnBring" cssclass="btn-success btnw6" runat="server" Text="帶入運費檔" />
                                <asp:Button ID="btnNoneed" cssclass="btn-success btnw6" runat="server" Text="無須計算運費" /><br /><br />
                                <asp:Button ID="btnHome" cssclass="btn-success btnw4" runat="server" Text="回主頁" /> 
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
