<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E11.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->履約驗結作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="subtitle" runat="server" Text="計畫年度(第二組)："></asp:Label>
                                <asp:DropDownList ID="drpSelectPlanYear" CssClass="tb tb-s" runat="server" OnSelectedIndexChanged="drpSelectPlanYear_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                <asp:Label CssClass="subtitle text-blue" runat="server" Text="(請先選擇年度)"></asp:Label>
                            </div>
                            <div><asp:Label CssClass="subtitle" runat="server" Text="購案查詢"></asp:Label></div>
                            <div class="text-center">
                                <asp:RadioButtonList ID="rdoSearchBy" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem>以購案編號查詢</asp:ListItem>
                                    <asp:ListItem>以購案名稱查詢</asp:ListItem>
                                    <asp:ListItem>以合約廠商名稱查詢</asp:ListItem>
                                </asp:RadioButtonList><br><br>

                                <asp:Label CssClass="control-label" runat="server" Text="請輸入查詢條件："></asp:Label>
                                <asp:TextBox ID="txtSearchCondition" htmlencode="true"  CssClass="tb tb-l" runat="server"></asp:TextBox><br><br>

                                <asp:Button ID="btnTransfer" CssClass="btn-success" OnClick="btnTransfer_Click" runat="server" Text="查詢購案移辦資料" />
                                <asp:Button ID="btnCurrently" CssClass="btn-success" OnClick="btnCurrently_Click" runat="server" Text="查詢採購目前資料" />
                                <asp:Button ID="btnAll" CssClass="btn-success" OnClick="btnAll_Click" runat="server" Text="年度案件總表" /><br><br>
                            </div>
                            <table class="table no-bordered-seesaw text-center">
                                <tr class="no-bordered-seesaw">
                                    <td style="width:30%" class="no-bordered"></td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label position-left" runat="server"></asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <!--↓日期套件↓-->
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtBuyCaseFrom" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtBuyCaseTo" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                            <asp:Button ID="btnGenerateReport" CssClass="btn-success position-left" OnClick="btnGenerateReport_Click" runat="server" Text="產出購案報表" />

                                    </td>
                                    <td  style="width:15%" class="no-bordered"></td>
                                </tr>
                            </table>
                            <asp:GridView ID="GV_TBMCONTRACT_MODIFY" CssClass="table data-table table-striped border-top text-center" OnRowCreated="GV_TBMCONTRACT_MODIFY_RowCreated" OnPreRender="GV_TBMCONTRACT_MODIFY_PreRender" OnRowDataBound="GV_TBMCONTRACT_MODIFY_RowDataBound" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField HeaderText="履驗承辦人" DataField="OVC_DO_NAME" />
                                    <asp:TemplateField HeaderText="收辦案件數" >
                                        <ItemTemplate>
                                            <asp:Label ID="labOVC_PURCH" Text='<%# Bind("OVC_PURCH") %>' Visible="true" runat="server" />
                                            <asp:Button ID="btnOVC_PURCH" CssClass="btn-success" Text='<%# Bind("OVC_PURCH") %>' OnClick="btnOVC_PURCH_Click" Visible="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField  HeaderText="未結案件數" >
                                        <ItemTemplate>
                                            <asp:Label ID="labOVC_STATUS" Text='<%# Bind("OVC_DCLOSE") %>' runat="server" />
                                            <asp:Button ID="btnOVC_STATUS" CssClass="btn-success" Text='<%# Bind("OVC_DCLOSE") %>' OnClick="btnOVC_STATUS_Click" Visible="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                	            </Columns>
	   		                </asp:GridView>
                            <asp:GridView ID="GV_total" CssClass=" table data-table table-striped border-top text-center" OnPreRender="GV_total_PreRender" OnRowDataBound="GV_total_RowDataBound" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField HeaderText="年度" DataField="" />
                                    <asp:BoundField HeaderText="收辦案件數" DataField="" />
                                    <asp:BoundField HeaderText="未結案件數" DataField="" />
                                </Columns>
                            </asp:GridView>
                            <div class="text-center"><asp:Button ID="btnCaculate" CssClass="btn-success btnw4" OnClick="btnCaculate_Click" Text="統計查詢" runat="server" /></div>
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
