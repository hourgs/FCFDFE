<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E26.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E26" %>
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
                    運費統計查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOdtInvDate" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnStasticQuery" cssclass="btn-success btnw2" runat="server" Text="查詢" /><br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CINF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CINF_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="軍種" />
                                    <asp:BoundField DataField="" HeaderText="應付金額" />
                                    <asp:BoundField DataField="" HeaderText="已付金額" />
                                    <asp:BoundField DataField="" HeaderText="未付金額" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <header  class="title">
                    運費明細查詢
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div>
                                        <asp:Label CssClass="control-label position-left" runat="server">起始日期&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtInvDate1" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        </div>
                                        <div>
                                        <asp:Label CssClass="control-label position-left"  runat="server">結束日期&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtInvDate2" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        </div>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOvcMilitaryType" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>&nbsp;&nbsp;
                                        <asp:DropDownList ID="drpOvcMilitaryType2" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">進出口</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpImportOrExport" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">區分</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpOvcType" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnDetailQuery" cssclass="btn-success btnw2" runat="server" Text="查詢" /><br /><br /> 
                            </div>
                            <asp:GridView ID="GV_TBGMT_CINF2" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CINF2_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="提單編號" />
                                    <asp:BoundField DataField="" HeaderText="結報申請單編號" />
                                    <asp:BoundField DataField="" HeaderText="運費編號" />
                                    <asp:BoundField DataField="" HeaderText="運費金額" />
                                    <asp:BoundField DataField="" HeaderText="航商" />
                                    <asp:BoundField DataField="" HeaderText="軍種" />
                                    <asp:BoundField DataField="" HeaderText="區分" />
                                </Columns>
                            </asp:GridView>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSwitch" cssclass="btn-success" runat="server" Text="切換至運費明細查詢" />
                                <asp:Button ID="btnPrint" cssclass="btn-success btwn2" runat="server" Text="列印" /><br />
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
