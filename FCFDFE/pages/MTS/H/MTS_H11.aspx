<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_H11.aspx.cs" Inherits="FCFDFE.pages.MTS.H.MTS_H11" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
         });

         function openPage() {
             var message = "";
             var date = $("#<%=txtODT_WEEK_DATE1.ClientID%>").val();
             //var date = "<%=txtODT_WEEK_DATE1.Text%>";
             var auth = <%=intAuth%>;
             var section = "";
             if (auth == 2)
                 //section = $("#<%=drpOVC_SECTION.ClientID%> option:selected").val();
                 section = "<%=drpOVC_SECTION.SelectedValue%>";
             else if (auth == 1)
                 //section = $("#<%=lblOVC_SECTION.ClientID%>").val();
                 section = "<%=lblOVC_SECTION.Text%>";
             //var id = "<%=txtOVC_BLD_NO.Text%>";
             var id = $("#<%=txtOVC_BLD_NO.ClientID%>").val();
             if (date == "")
                 message += "請選擇 資料時間－前者！\n";
             if (section == "")
                 message += "接轉地區不存在！\n";
             if (id == "")
                 message += "請輸入 提單編號！\n";
             if (message == "")
                 var win = window.open("MTS_H11_2?date1=" + date + "&section=" + section + "&id=" + id, null, 'width=1200,height=700,left=0,top=0');
             else
                 alert(message);
         }
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <asp:Label ID="lblDEPT" runat="server"></asp:Label>
                    <asp:DropDownList  ID="drpOVC_SECTION" CssClass="tb tb-s" OnSelectedIndexChanged="drpOVC_SECTION_SelectedIndexChanged" AutoPostBack="true" runat="server">
                           <asp:ListItem Selected="True">基隆地區</asp:ListItem>
                            <asp:ListItem>桃園地區</asp:ListItem>
                            <asp:ListItem>高雄分遣組</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblOVC_SECTION" Visible="false" runat="server"></asp:Label>
                    <asp:Label Text="接轉作業週報表" runat="server"></asp:Label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnData" runat="server">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" colspan="2">
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
                                <tr>
                                    <td class="text-center" style="vertical-align: middle; width: 120px;">
                                        <asp:Label CssClass="control-label" runat="server">空運架次</asp:Label>
                                    </td>
                                    <td colspan="2">
                                         <asp:TextBox ID="txtONB_SHIP_AIR" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center" style="vertical-align: middle; width: 120px;">
                                        <asp:Label CssClass="control-label" runat="server">海運航次</asp:Label>
                                    </td>
                                    <td colspan="2">
                                         <asp:TextBox ID="txtONB_SHIP_SEA" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center" style="vertical-align: middle; width: 120px;">
                                        
                                    </td>
                                    <td colspan="2" class="text-center">
                                         <asp:Button CssClass="btn-success btnw4" Text="重新統計" OnClick="btnStatistic_Click" runat="server" /> 
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">空運報關數</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtONB_BLD_AIR" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">海運報關數</asp:Label>
                                    </td>
                                    <td colspan="2">
                                         <asp:TextBox ID="txtONB_BLD_SEA" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">

                                    </td>
                                    <td colspan="2">

                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">貨櫃20呎</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtONB_20_COUNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">貨櫃40呎</asp:Label>
                                    </td>
                                    <td colspan="2">
                                         <asp:TextBox ID="txtONB_40_COUNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">貨櫃45呎</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtONB_45_COUNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">接轉件數</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtONB_QUANITY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">接轉重量KG</asp:Label>
                                    </td>
                                    <td colspan="2">
                                         <asp:TextBox ID="txtONB_WEIGHT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">接轉體積CBM</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtONB_VOLUME" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">建制輸具車次</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtONB_TRANS_DEFAULT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">委商輸具車次</asp:Label>
                                    </td>
                                    <td colspan="2">
                                         <asp:TextBox ID="txtONB_TRANS_SUPPLIER" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">鐵運車次</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtONB_TRANS_TRAIN" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                            <table class="table">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單編號：</asp:Label>
                                        <%--<asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m" OnTextChanged="txtOVC_BLD_NO_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                        <asp:Button cssclass="btn-success btnw2" Text="查詢"  OnClick="btnQuery_Click" style="margin-left: 20px;" OnClientClick="openPage()" runat="server"/>
                                        <%--<asp:Button cssclass="btn-success btnw2" Text="查詢"  OnClick="btnQuery_Click" style="margin-left: 20px;" runat="server"/>--%>
                                        <%--<input class="btn-success btnw2" value="查詢" type="button" onclick="openPage()" />--%>
                                        <asp:Button cssclass="btn-warning btnw2" Text="新增"  OnClick="btnNew_Click" runat="server"/>
                                        <asp:Button cssclass="btn-danger btnw2" Text="刪除"  OnClick="btnDel_Click" runat="server"/>
                                        <asp:Button cssclass="btn-warning btnw2" Text="更新"  OnClick="btnUpdate_Click" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備考：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOVC_NOTE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="GV_WRP_BLD" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_WRP_BLD_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_BLD_NO" HeaderText="提單編號" />
                                    <asp:BoundField DataField="OVC_CHI_NAME" HeaderText="品名" />
                                    <asp:BoundField DataField="ONB_QUANITY" HeaderText="箱件" />
                                    <asp:BoundField DataField="ONB_VOLUME" HeaderText="噸位" />
                                    <asp:BoundField DataField="OVC_RECEIVE_DEPT" HeaderText="接收單位" />
                                    <%--<asp:BoundField DataField="OVC_DEPT_NAME" HeaderText="接收單位" />--%>
                                    <asp:BoundField DataField="ODT_IMPORT_DATE" HeaderText="進口日期" />
                                    <asp:BoundField DataField="ODT_PASS_DATE" HeaderText="通關日期" />
                                    <asp:BoundField DataField="ODT_STORED_DATE" HeaderText="進倉日期" />
                                    <asp:BoundField DataField="ODT_TRANSFER_DATE" HeaderText="清運日期" />
                                    <asp:BoundField DataField="OVC_NOTE" HeaderText="備考" />
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                                <asp:Button cssclass="btn-warning btnw6" Text="更新週報表"  OnClick="btnUpdateWeek_Click" runat="server"/> 
                                <asp:Button ID="btnPrint" cssclass="btn-success btnw6" Text="列印週報表"  OnClick="btnPrint_Click" runat="server"/>
                            </div>
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
