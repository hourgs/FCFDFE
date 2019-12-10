<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_B12.aspx.cs" Inherits="FCFDFE.pages.CIMS.B.CIMS_B12" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 800px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>底價表查詢功能</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form text-center" >
                            <!--網頁內容-->
                            <div class="subtitle">全文檢索</div>
                            <asp:Label CssClass="control-label" runat="server">底價表全文搜尋 :</asp:Label> &ensp;
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width:30%"><asp:Label CssClass="control-label" runat="server">採購案號</asp:Label></td>
                                    <td class="text-left" style="width:70%">
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:30%"><asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label></td>
                                    <td class="text-left" style="width:70%">
                                        <asp:TextBox ID="txtOVC_PUR_IPURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:30%"><asp:Label CssClass="control-label" runat="server">承辦人</asp:Label></td>
                                    <td class="text-left" style="width:70%">
                                        <asp:TextBox ID="txtOVC_PUR_USER" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">申購日期</asp:Label></td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_OVC_DPROPOSE_SDATE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                            <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                        </div>                                      
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_OVC_DPROPOSE_EDATE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">單位全銜</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PUR_NSECTION" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnQuery" cssclass="btn-success btnw2" runat="server" Text="搜尋" Onclick="btnQuery_Click"/>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="Detail" runat="server" Visible="false">
                                <asp:GridView ID="GV_TBM1301" CssClass="table data-table table-striped border-top table-bordered" DataKeyNames="OVC_PURCH" AutoGenerateColumns="false" runat="server" OnPreRender="GV_TBM1301_PreRender" OnRowCommand="GV_TBM1301_RowCommand">
                                    <Columns>
                                        <asp:BoundField HeaderText="項次" DataField="RANK" ItemStyle-Width="4%" />
                                        <asp:BoundField HeaderText="採購案號" DataField="OVC_PURCH" ItemStyle-Width="12%" />
                                        <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" ItemStyle-Width="26%" />
                                        <asp:BoundField HeaderText="承辦人" DataField="OVC_PUR_USER" ItemStyle-Width="6%" />
                                        <asp:BoundField HeaderText="申購日期" DataField="OVC_DPROPOSE" ItemStyle-Width="16%"  DataFormatString="{0:d}" />
                                        <asp:BoundField HeaderText="單位代碼" DataField="OVC_PUR_SECTION" ItemStyle-Width="8%" />
                                        <asp:BoundField HeaderText="單位全銜" DataField="OVC_PUR_NSECTION" ItemStyle-Width="24%" />
                                        <asp:TemplateField HeaderText="查詢">
                                            <ItemTemplate>
                                                <asp:Button CssClass="btn-info btnw2" CommandName="Check" runat="server" Text="查詢" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="Button1" CssClass="btn-success btnw5" runat="server" Text="返回" visible="false"/>
                            </asp:Panel>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
