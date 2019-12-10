<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_A12.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.MPMS_A12" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
          <%--   <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>--%>
            <section class="panel">
                <header class="title">
                    採購預劃購案編輯
                </header>

                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="form-group">
                                <div class="col-lg-7">
                                    <asp:Label CssClass="control-label" runat="server">預劃購案編號：</asp:Label>
                                    <asp:Label ID="lblPrePurNum" CssClass="control-label" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-5">
                                    <asp:Label CssClass="control-label" runat="server">採購單位地區及方式：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_PUR_AGENCY" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_PUR_AGENCY_SelectedIndexChanged" AutoPostBack="True" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-7">
                                    <asp:Label CssClass="control-label" runat="server">美軍案號：</asp:Label>
                                    <asp:Label ID="lblOVC_PURCH_MILITARY" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtOVC_PURCH_MILITARY" CssClass="tb tb-m" AutoPostBack="true" runat="server"></asp:TextBox>
                                    <asp:Label ID="lblDescription" CssClass="control-label" runat="server" AutoPostBack="true" Text="(TWxxxx)"></asp:Label>
                                </div>
                                <div class="col-lg-5">
                                    <asp:Label CssClass="control-label" runat="server">軍售案類別：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Text="不可空白" Value=""></asp:ListItem>
                                        <asp:ListItem Text="個別式" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="開放式" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-7">
                                    <asp:Label CssClass="control-label" runat="server">購案名稱：</asp:Label>
                                    <asp:TextBox ID="txtOVC_PUR_IPURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-lg-5">
                                    <asp:Label CssClass="control-label" runat="server">招標方式：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-m" AutoPostBack="True" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-7">
                                    <asp:Label CssClass="control-label" runat="server">專案代號(適用裝備)：</asp:Label>
                                    <asp:TextBox ID="txtOVC_PUR_PROJE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">採購屬性：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_LAB" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem>內容1</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-5">
                                    <asp:Label CssClass="control-label" runat="server">計畫性質：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_PLAN_PURCH" CssClass="tb tb-m" runat="server">
                                        <asp:ListItem>內容1</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-7">
                                    <asp:Label CssClass="control-label" runat="server">核定權責：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_PUR_APPROVE_DEP" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    <asp:Button ID="btnOVC_PUR_APPROVE_DEP" CssClass="btn-success btnw6" Text="核定權責說明" Onclick="btnOVC_PUR_APPROVE_DEP_Click" runat="server" />
                                </div>
                                <div class="col-lg-5">
                                    <asp:Label CssClass="control-label" runat="server">是否為1月1日須執行之購案：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_ON_SCHEDULE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-3">
                                    <asp:Label CssClass="control-label" runat="server">承辦人姓名：</asp:Label>
                                    <asp:TextBox ID="txtOVC_PUR_USER" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-lg-9">
                                    <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                                    <asp:Label CssClass="control-label" runat="server">軍線1：</asp:Label>
                                    <asp:TextBox ID="txtOVC_PUR_IUSER_PHONE_EXT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">軍線2：</asp:Label>
                                    <asp:TextBox ID="txtOVC_PUR_IUSER_PHONE_EXT_1" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">自動：</asp:Label>
                                    <asp:TextBox ID="txtOVC_PUR_IUSER_PHONE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" runat="server">承辦人手機：</asp:Label>
                                    <asp:TextBox ID="txtOVC_USER_CELLPHONE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">承辦人E-MAIL：</asp:Label>
                                    <asp:TextBox ID="txtEMAIL_ACCOUNT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">傳真號碼：</asp:Label>
                                    <asp:TextBox ID="txtOVC_PUR_IUSER_FAX" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" runat="server">本案委託購案，委託單位：</asp:Label>
                                    <asp:TextBox id="txtOVC_AGENT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valOVC_AGENT_UNIT" ControlToValidate="txtOVC_AGENT_UNIT"  runat="server" Forecolor="Red" ErrorMessage="必填"></asp:RequiredFieldValidator>
                                    <asp:Button ID="btnQuery_OVC_AGENT_UNIT" OnClick="btnQuery_OVC_AGENT_UNIT_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                    <asp:Button ID="btnexp_OVC_AGENT_UNIT" CssClass="btn-success btnw6" onclick="btnexp_OVC_AGENT_UNIT_Click" runat="server" Text="委託單位說明" />
                                    <asp:TextBox id="txtOVC_AGENT_UNIT_exp" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                             
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" ForeColor="Red" runat="server">最後計畫評核(審查)單位：</asp:Label>
                                    <asp:TextBox id="txtOVC_AUDIT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valOVC_AUDIT_UNIT" ControlToValidate="txtOVC_AUDIT_UNIT"  runat="server" Forecolor="Red" ErrorMessage="必填"></asp:RequiredFieldValidator>
                                    <asp:TextBox id="txtOVC_AUDIT_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnAudit" onclick="btnAudit_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" runat="server">採購發包單位：</asp:Label>
                                    <asp:TextBox id="txtOVC_PURCHASE_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valOVC_PURCHASE_UNIT" ControlToValidate="txtOVC_PURCHASE_UNIT"  runat="server" Forecolor="Red" ErrorMessage="必填"></asp:RequiredFieldValidator>
                                    <asp:TextBox id="txtOVC_PURCHASE_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnQueryOVC_PURCHASE_UNIT" onclick="btnQueryOVC_PURCHASE_UNIT_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                </div>
                                <div class="col-lg-12">
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" runat="server">履約驗結單位：</asp:Label>
                                    <asp:TextBox id="txtOVC_CONTRACT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valOVC_CONTRACT_UNIT" ControlToValidate="txtOVC_CONTRACT_UNIT"  runat="server" Forecolor="Red" ErrorMessage="必填"></asp:RequiredFieldValidator>
                                    <asp:TextBox id="txtOVC_CONTRACT_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnQueryOVC_CONTRACT_UNIT" onclick="btnQueryOVC_CONTRACT_UNIT_Click" OnClientClick="OpenWindow()" cssclass="btn-warning" runat="server" Text="單位查詢"/>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label ID="Label3" CssClass="control-label" runat="server" Text="(若仍由採購中心下授委辦單位履約者，仍請選定採購中心為履約驗結單位)" ForeColor="Red"></asp:Label>
                                </div>
                            </div>

                            <div class="form-group">
                                <%--<div class="col-lg-12 form-group-inner">
                                    <asp:Label CssClass="control-label position-left" runat="server">預計申辦日期：</asp:Label>
                                    <div class="input-append datepicker position-left">
                                        <asp:TextBox ID="txtOVC_DPROPOSE" CssClass="tb tb-s position-left text-change" OnTextChanged="txtOVC_DPROPOSE_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                    </div>
                                    <asp:Label CssClass="control-label" runat="server">(西元年，例如:2000-01-01)</asp:Label>
                                </div>--%>
                                <div class="col-lg-12 form-group-inner">
                                    <asp:Label CssClass="control-label position-left" runat="server">預計申辦日期：</asp:Label>
                                    <div class="input-append datepicker">
                                        <asp:TextBox ID="txtOVC_DPROPOSE" CssClass="tb tb-s position-left text-change" OnTextChanged="txtOVC_DPROPOSE_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                    </div>
                                    <asp:Label CssClass="control-label" runat="server">(西元年，例如:2000-01-01)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" runat="server">審核天數：</asp:Label>
                                    <asp:TextBox ID="txtONB_REVIEW_DAYS" CssClass="tb tb-s" OnTextChanged="txtONB_REVIEW_DAYS_TextChanged" AutoPostBack="True" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(日曆天)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" runat="server">預計核定日：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s" AutoPostBack="True" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(=預計呈報日期+審核天數，由系統幫您算出)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" runat="server">招標天數：</asp:Label>
                                    <asp:DropDownList ID="drpOVC_TENDER_DAYS" CssClass="tb tb-m" onSelectedIndexChanged="drpOVC_TENDER_DAYS_SelectedIndexChanged" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    <asp:Label CssClass="control-label" runat="server">(日曆天)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label CssClass="control-label" runat="server">預計簽約日：</asp:Label>
                                    <asp:TextBox ID="txtOVC_DCONTRACT" CssClass="tb tb-s" AutoPostBack="True" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(=預計核定日期+招標天數，由系統幫您算出)</asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label CssClass="control-label" Style="float: left;" ForeColor="Red" runat="server">交貨天數</asp:Label>
                                <div class="form-group col-lg-12" style="float: left;">
                                    <div class="col-lg-12">
                                        <asp:RadioButton ID="rdoONB_DELIVER_DAYS" CssClass="radioButton rb-complex position-left" runat="server" />
                                        <asp:Label CssClass="control-label position-left" runat="server">交貨天數：</asp:Label>
                                        <asp:TextBox ID="txtONB_DELIVER_DAYS" CssClass="tb tb-s position-left" OnTextChanged="txtONB_DELIVER_DAYS_TextChanged" AutoPostBack="True" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label position-left" runat="server">或 </asp:Label>
                                        <asp:RadioButton ID="rdoONB_DELIVER_DATE" CssClass="radioButton rb-complex position-left" runat="server" />
                                        <asp:Label CssClass="control-label position-left" runat="server">限定交貨日期：</asp:Label>
                                        <div class="input-append datepicker">
                                        <asp:TextBox ID="txtONB_DELIVER_DATE" CssClass="tb tb-s position-left text-change" OnTextChanged="txtONB_DELIVER_DATE_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">(二擇一)</asp:Label><br><br>
                                    </div>
                                    <div class="col-lg-12">
                                        <asp:Label CssClass="control-label" runat="server">驗結天數：</asp:Label>
                                        <asp:DropDownList ID="drpOVC_RECEIVE_DAYS" CssClass="tb tb-m" AutoPostBack="True" onSelectedIndexChanged="drpOVC_RECEIVE_DAYS_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">(日曆天)</asp:Label>
                                    </div>
                                    <div class="col-lg-12">
                                        <asp:Label CssClass="control-label" runat="server">預計結案日：</asp:Label>
                                        <asp:TextBox ID="txtOVC_DCLOSE" CssClass="tb tb-s" disabled="true" AutoPostBack="True" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">(=預計簽約日期+交貨天數+驗結天數，由系統幫您算出)</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                   		<asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="儲存" OnClick="btnSave_Click" /><!--黃色-->
                        <asp:Button ID="btnDel" cssclass="btn-danger btnw2" CommandName="DataDel" runat="server" Text="刪除" OnClick="btnDel_Click" /><!--紅色-->
                   		<asp:Button ID="btnReset" CssClass="btn-default btnw2" Onclick="btnReset_Click" runat="server" Text="清除" /><!--灰色-->
                </footer>
            </section>
            <asp:Panel ID="divCheck" runat="server">
                                <section class="panel">
                                <header class="title">
                                    預劃年度、科目及金額
                                </header>
                            <asp:Panel ID="Panel2" runat="server"></asp:Panel>
                            <!--預留空間，未來做錯誤訊息顯示。-->
                            <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                            <asp:GridView ID="GV_TBM1231_PLAN" DataKeyNames="OVC_P_SN" CssClass="table data-table table-striped border-top table-bordered" OnPreRender="GV_TBM1231_PLAN_PreRender" OnRowCommand="GV_TBM1231_PLAN_RowCommand" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="選擇">
                                        <ItemTemplate>
                                            <asp:Button cssclass="btn-danger btnw2" CommandName="DataDel" runat="server" Text="刪除" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="預劃年度" DataField="OVC_BUDGET_YEAR" />
                                    <asp:BoundField HeaderText="月份" DataField="OVC_BUDGET_MONTH" />
                                    <asp:BoundField HeaderText="預算科目代號" DataField="OVC_POI_IBDG" />
                                    <asp:BoundField HeaderText="預算科目名稱" DataField="OVC_PJNAME" />
                                    <asp:BoundField HeaderText="預劃金額(新台幣)" DataField="ONB_MONEY" />
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
                                <header class="title">
                                    預劃年度、科目及金額
                                </header>
                            <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                            <!--預留空間，未來做錯誤訊息顯示。-->
                            <div class="panel-body" style="border: solid 2px;">
                            <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="PnMessage_AccountAuth" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">選擇</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預劃年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">月份</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預算科目代號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預算科目名稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">預劃金額(新台幣)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnNew" CssClass="btn-success" runat="server" Text="新增" OnClick="btnNew_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_BUDGET_MONTH" CssClass="tb tb-s" runat="server" AutoPostBack="True">
                                            <asp:ListItem>01</asp:ListItem>
                                            <asp:ListItem>02</asp:ListItem>
                                            <asp:ListItem>03</asp:ListItem>
                                            <asp:ListItem>04</asp:ListItem>
                                            <asp:ListItem>05</asp:ListItem>
                                            <asp:ListItem>06</asp:ListItem>
                                            <asp:ListItem>07</asp:ListItem>
                                            <asp:ListItem>08</asp:ListItem>
                                            <asp:ListItem>09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoBudgetType" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" onselectedindexchanged="rdoBudgetType_SelectedIndexChanged" runat="server">
                                            <asp:ListItem Value="1">國防預算</asp:ListItem>
                                            <asp:ListItem Value="2">非國防預算</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:DropDownList ID="drpOVC_POI_IBDG" CssClass="tb tb-m" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpOVC_POI_IBDG_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_PJNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_MONEY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
            </asp:Panel>
           <%--         </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnOVC_PUR_APPROVE_DEP" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnQuery_OVC_AGENT_UNIT" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnexp_OVC_AGENT_UNIT" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAudit" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnQueryOVC_PURCHASE_UNIT" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnQueryOVC_CONTRACT_UNIT" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="txtOVC_DPROPOSE" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>
