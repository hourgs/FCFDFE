<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C1A.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C1A" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    撤案作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle text-red">注意：必須由目前購案之承辦單位才可撤案 </div>
                            <table class="table table-bordered" style="text-align: center">
                                <tr>
                                    <td style="width: 30%">
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button ID="BtnQuery_OVC_PURCH" CssClass="btn-success btnw2" OnClick="BtnQuery_OVC_PURCH_Click" runat="server" Text="查詢" />
                                        <asp:Button ID="btnReset" CssClass="btn-default btnw2" OnClick="btnReset_Click" runat="server" Text="清除" /><br />
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">申購單位(代碼)--申購人(電話)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">目前階段</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_STATUS" CssClass="control-label" runat="server">採購發包階段</asp:Label>
                                    </td>
                                </tr>
                                <tr id="trCancelDate" visible="false" runat="server">
                                    <td>
                                        <asp:Label CssClass="control-label position-left" runat="server">撤案日期：</asp:Label>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DCANCEL" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">撤案原因：</asp:Label>
                                        <asp:TextBox ID="txtOVC_PUR_DCANRE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trAPPROVE" visible="false" runat="server">
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">核定日期：</asp:Label>
                                        <asp:Label ID="lblOVC_PUR_DAPPROVE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">核定文號：</asp:Label>
                                        <asp:Label ID="lblOVC_PUR_APPROVE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center" style="margin-bottom: 15px;">
                                <asp:Button ID="btnSave" CssClass="btn-warning btnw4" OnClick="btnSave_Click" runat="server" Text="確認撤案" />
                            </div>
                            <div style="width: 50%; margin: 0 auto;">
                                <div class="subtitle">選擇年度來查詢 </div>
                                <table class="table table-bordered" style="text-align: center">
                                    <tr>
                                        <td>請選擇計劃年度(第二組)： 
                                        <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>106</asp:ListItem>
                                        </asp:DropDownList>
                                            <asp:Button ID="btnQuery_OVC_YEAR" CssClass="btn-success btnw2" OnClick="btnQuery_OVC_YEAR_Click" runat="server" Text="查詢" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="subtitle">查詢結果 </div>
                            <asp:GridView ID="GV_OVC" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false" OnPreRender="GV_OVC_PreRender" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                           <asp:Label ID="lblOVC_CHECKER" CssClass="control-label" Text='<%# "" + Eval("OVC_PURCH")+ Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                                    <asp:TemplateField ItemStyle-Width="40%" HeaderText="委購單位" >
                                        <ItemTemplate>
                                           <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" Text='<%#Bind("OVC_PUR_NSECTION") %>' runat="server"></asp:Label>
                                            </br>
                                            <asp:Label ID="lblOVC_PUR_DCANPO" CssClass="control-label" ForeColor="Red" Text='<%#Bind("OVC_PUR_DCANPO") %>' runat="server"></asp:Label>
                                            </br>
                                            <asp:Label ID="lblOVC_PUR_DCANRE" CssClass="control-label" ForeColor="Red" Text='<%#Bind("OVC_PUR_DCANRE") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="總審查次數" DataField="ONB_CHECK_TIMES" />
                                    <asp:BoundField ItemStyle-Width="10%" HeaderText="最後計評承辦人" DataField="OVC_CHECKER" />
                                    <asp:BoundField HeaderText="最後分派日期" DataField="OVC_DRECEIVE" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
