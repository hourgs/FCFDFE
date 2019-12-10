<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_A11.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.MPMS_A11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
            //$("#li_MPMS_A").addClass("active");
            //$("#li_MPMS_A11").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <section class="panel">
                        <header class="title">
                            <!--標題-->
                            採購預劃
                        </header>
                        <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                        <!--預留空間，未來做錯誤訊息顯示。-->
                        <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                                <div class="cmxform form-horizontal tasi-form">
                                    <!--網頁內容-->
                                    <div class="subtitle">預劃購案編號賦予</div>
                                    <table id="testTable" class="table table-bordered control-label" style="text-align: center">
                                        <tr>
                                            <td>單位代字(第一組)</td>
                                            <td>計劃年度(第二組)</td>
                                            <td>計劃編號(第三組)</td>
                                            <td>預劃購案編號</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="drpPURCHASE_1" CssClass="tb tb-s" OnSelectedIndexChanged="drpPURCHASE_1_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                <asp:TextBox ID="txtPURCHASE_1" CssClass="tb tb-s" AutoPostBack="true" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                    runat="server"
                                                    ErrorMessage="請輸入2碼英文"
                                                    ForeColor="Red"
                                                    ControlToValidate="txtPURCHASE_1"
                                                    ValidationExpression="^.[A-Za-z]+$">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpSysYear" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPlanNum" CssClass="tb tb-s" OnTextChanged="txtPlanNum_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="valPlanNum"
                                                    runat="server"
                                                    ErrorMessage="請輸入3碼數字"
                                                    ForeColor="Red"
                                                    ControlToValidate="txtPlanNum"
                                                    ValidationExpression="^\d{3}$">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div class="text-center">
                                        <asp:Button ID="btnNew" CssClass="btn-success btnw4" runat="server" Text="新增" OnClick="btnNew_Click" />
                                        <asp:Button ID="btnReset" CssClass="btn-default btnw2" runat="server" Text="取消" OnClick="btnReset_Click" />
                                    </div>
                                    <br />
                                    <div>
                                        <table id="QueryTable" class="table table-bordered text-center" style="text-align: center" runat="server">
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label CssClass="control-label" runat="server">單位代字及年度(4碼)：</asp:Label>
                                                    <asp:TextBox ID="txtPurch" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                    <asp:Button ID="btnPurch" CssClass="btn-success btnw2" Text="查詢" OnClick="btnPurch_Click" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
