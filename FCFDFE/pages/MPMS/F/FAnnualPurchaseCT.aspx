<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FAnnualPurchaseCT.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FAnnualPurchaseCT" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .lbl-title {
            display: block;
            font-size: 32px;
        }
        .lbl-subtitle {
            text-align: left;
            padding-left: 20px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:Label CssClass="lbl-title" runat="server"><%=strDEPT_Name%>主官查詢系統</asp:Label>
                    <div class="lbl-subtitle">
                        <asp:Label CssClass="" runat="server">年度購案管制表</asp:Label>
                    </div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnField" runat="server">
                            <!--網頁內容-->
                            <table class="table table-bordered">
                                <tr style="background-color: blue; color: aliceblue;">
                                    <th class="text-center" style="width: 25%">
                                        <asp:Label CssClass="control-label" runat="server">顯示欄位</asp:Label>
                                    </th>
                                    <th class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">過濾條件</asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:CheckBox ID="chkYear" CssClass="radioButton" Checked="true" Text="全選" runat="server" />
                                        <%--<asp:CheckBox ID="chkyear" CssClass="radioButton" Text="：" runat="server"></asp:CheckBox>全選--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PURCH" CssClass="radioButton text-blue" Checked="true" Text="購案編號：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">購案編號：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_IPURCH" CssClass="radioButton text-blue" Checked="true" Text="購案名稱：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">購案名稱：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_SECTION" CssClass="radioButton text-blue" Checked="true" Text="申購單位：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">申購單位：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_SECTION" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-red" runat="server">（含下屬單位）</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_APPROVE_DEP" CssClass="radioButton text-blue" Checked="true" Text="核定權責：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">核定權責：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_APPROVE_DEP" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DAPPLY" CssClass="radioButton text-blue" Checked="true" Text="預劃申購日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_DATE" CssClass="radioButton text-blue" Checked="true" Text="預劃申購日期：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">預劃申購日期：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DAPPLY1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label><!--後方備註文字，跟日期同一行需使用"position-left"之class-->
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DAPPLY2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DPROPOSE" CssClass="radioButton text-blue" Checked="true" Text="申購單位實際申請日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_PUR_DATE" CssClass="radioButton text-blue" Checked="true" Text="申購單位實際申請日期：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">申購單位實際申請日期：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DPROPOSE1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtOVC_PUR_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>--%>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label><!--後方備註文字，跟日期同一行需使用"position-left"之class-->
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DPROPOSE2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtOVC_PUR_DATE2" CssClass="tb tb-date" runat="server"></asp:TextBox>--%>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DRECEIVE" CssClass="radioButton text-blue" Checked="true" Text="計評實際收辦日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_PLAN_DATE" CssClass="radioButton text-blue" Checked="true" Text="計評實際收辦日期：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">計評實際收辦日期：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PLAN_PUR_DAPPROVE" CssClass="radioButton text-blue" Checked="true" Text="預劃核定日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_DINFORM_PLAN" CssClass="radioButton text-blue" Checked="true" Text="預劃核定日期：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">預劃核定日期：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PLAN_PUR_DAPPROVE1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtOVC_DINFORM_PLAN1" CssClass="tb tb-date" runat="server"></asp:TextBox>--%>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label><!--後方備註文字，跟日期同一行需使用"position-left"之class-->
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PLAN_PUR_DAPPROVE2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtOVC_DINFORM_PLAN2" CssClass="tb tb-date" runat="server"></asp:TextBox>--%>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_ALLOW" CssClass="radioButton text-blue" Checked="true" Text="主官核批日：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_DAPPROVE" CssClass="radioButton text-blue" Checked="true" Text="主官核批日：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">主官核批日：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_ALLOW1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-date" runat="server"></asp:TextBox>--%>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label><!--後方備註文字，跟日期同一行需使用"position-left"之class-->
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_ALLOW2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtOVC_DAPPROVE2" CssClass="tb tb-date" runat="server"></asp:TextBox>--%>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_DAPPROVE" CssClass="radioButton text-blue" Checked="true" Text="核定（發文）日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_DNOTICE" CssClass="radioButton text-blue" Checked="true" Text="核定（發文）日期：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">核定（發文）日期：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_DAPPROVE1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtchkOVC_DNOTICE" CssClass="tb tb-date" runat="server"></asp:TextBox>--%>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label><!--後方備註文字，跟日期同一行需使用"position-left"之class-->
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_DAPPROVE2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtchkOVC_DNOTICE2" CssClass="tb tb-date" runat="server"></asp:TextBox>--%>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_APPROVE" CssClass="radioButton text-blue" Checked="true" Text="核定文號：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">核定文號：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_CHECKER" CssClass="radioButton text-blue" Checked="true" Text="計評承辦人：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_PLAN_USER" CssClass="radioButton text-blue" Checked="true" Text="計評承辦人：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">計評承辦人：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHECKER" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtOVC_PUR_USER" CssClass="tb tb-s" runat="server"></asp:TextBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PLAN_PURCH" CssClass="radioButton text-blue" Checked="true" Text="計畫性質：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">計畫性質：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PLAN_PURCH" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_AGENCY" CssClass="radioButton text-blue" Checked="true" Text="採購途徑：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_ITEM" CssClass="radioButton text-blue" Checked="true" Text="採購途徑：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">採購途徑：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_AGENCY" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        <%--<asp:DropDownList ID="drpOVC_ITEM" CssClass="tb tb-l" runat="server"></asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_LAB" CssClass="radioButton text-blue" Checked="true" Text="採購屬性：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">採購屬性：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_LAB" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_ASS_VEN_CODE" CssClass="radioButton text-blue" Checked="true" Text="招標方式：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">招標方式：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkGPA" CssClass="radioButton text-blue" Checked="true" Text="是否適用GPA：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">是否適用GPA：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpGPA" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkGPA_TYPE" CssClass="radioButton text-blue" Checked="true" Text="政府採購協定片語：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkGPA_TYPE" CssClass="radioButton text-blue" Checked="true" Text="GPA片語：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">GPA片語：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PURCHASE_UNIT" CssClass="radioButton text-blue" Checked="true" Text="採購發包單位：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">採購發包單位：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PURCHASE_UNIT" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-red" runat="server">（含下屬單位）</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_NAME" CssClass="radioButton text-blue" Checked="true" Text="採包承辦人：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVE_USER" CssClass="radioButton text-blue" Checked="true" Text="採包承辦人：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">採包承辦人：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_CONTRACT_UNIT" CssClass="radioButton text-blue" Checked="true" Text="履約驗結單位：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">履約驗結單位：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_CONTRACT_UNIT" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-red" runat="server">（含下屬單位）</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DO_NAME" CssClass="radioButton text-blue" Checked="true" Text="履驗承辦人：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">履驗承辦人：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_OPEN_CHECK" CssClass="radioButton text-blue" Checked="true" Text="需公開閱覽：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">需公開閱覽：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_OPEN_CHECK" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_ADVENTAGED_CHECK" CssClass="radioButton text-blue" Checked="true" Text="準用最有利標：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">準用最有利標：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkONB_PUR_BUDGET_NT" CssClass="radioButton text-blue" Checked="true" Text="預算（台幣）：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_BUDGET" CssClass="radioButton text-blue" Checked="true" Text="預算（台幣）：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">預算（台幣）：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIS_PLURAL_BASIS" CssClass="radioButton text-blue" Checked="true" Text="是否複數決標：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">是否複數決標：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpIS_PLURAL_BASIS" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIS_OPEN_CONTRACT" CssClass="radioButton text-blue" Checked="true" Text="是否開放式契：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">是否開放式契：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpIS_OPEN_CONTRACT" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIS_JUXTAPOSED_MANUFACTURER" CssClass="radioButton text-blue" Checked="true" Text="是否並列得標廠商：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">是否並列得標廠商：</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpIS_JUXTAPOSED_MANUFACTURER" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DOPEN_AND_RESULT" CssClass="radioButton text-blue" Checked="true" Text="開標日期及開標結果：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_OPEN_HOUR" CssClass="radioButton text-blue" Checked="true" Text="開標日期及開標結果：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">開標日期及開標結果：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkTTL_BID_GROUP" CssClass="radioButton text-blue" Checked="true" Text="組別（決標組數）：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkONB_GROUP" CssClass="radioButton text-blue" Checked="true" Text="組別（決標組數）：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">組別（決標組數）：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkTTL_PERMI_GROUP" CssClass="radioButton text-blue" Checked="true" Text="核定分組數：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_GROUP" CssClass="radioButton text-blue" Checked="true" Text="核定分組數：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">核定分組數：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DBID" CssClass="radioButton text-blue" Checked="true" Text="決標日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_DOPEN" CssClass="radioButton text-blue" Checked="true" Text="決標日期：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">決標日期：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkONB_MONEY_NT" CssClass="radioButton text-blue" Checked="true" Text="決標金額（台幣【含單價、折扣或其他】）：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkBID_RESULT" CssClass="radioButton text-blue" Checked="true" Text="決標金額（台幣）：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">決標金額（台幣）：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_MEMO" CssClass="radioButton text-blue" Checked="true" Text="決標方式片語：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_BID_METHOD_P" CssClass="radioButton text-blue" Checked="true" Text="計評決標方式片語：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">計評決標方式片語：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkONB_MONEY" CssClass="radioButton text-blue" Checked="true" Text="合約金額（台幣）：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">合約金額（台幣）：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_VEN_TITLE" CssClass="radioButton text-blue" Checked="true" Text="得標廠商：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_VEN" CssClass="radioButton text-blue" Checked="true" Text="得標商：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">得標商：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkONB_DELIVERY_TIMES" CssClass="radioButton text-blue" Checked="true" Text="交貨批次：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkONB_SHIP_TIMES" CssClass="radioButton text-blue" Checked="true" Text="交貨批次：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">交貨批次：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DCONTRACT" CssClass="radioButton text-blue" Checked="true" Text="簽約日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">簽約日期：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DELIVERY_CONTRACT" CssClass="radioButton text-blue" Checked="true" Text="合約交貨日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">合約交貨日期：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DELIVERY" CssClass="radioButton text-blue" Checked="true" Text="實際交貨日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">實際交貨日期：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DINSPECT_END" CssClass="radioButton text-blue" Checked="true" Text="驗結日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_RECEIVE" CssClass="radioButton text-blue" Checked="true" Text="驗結日期：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">驗結日期：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DCLOSE" CssClass="radioButton text-blue" Checked="true" Text="結案日期：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">結案日期：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_DCANPO_YN" CssClass="radioButton text-blue" Checked="true" Text="是否撤案：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">是否撤案：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkAUDITTOTALDAY" CssClass="radioButton text-blue" Checked="true" Text="評核總天數：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_CHECKDAY" CssClass="radioButton text-blue" Checked="true" Text="評核總天數：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">評核總天數：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DRECEIVE_PAPER" CssClass="radioButton text-blue" Checked="true" Text="紙本收文日：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">紙本收文日：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_DRECEIVE2" CssClass="radioButton text-blue" Checked="true" Text="分派日：" runat="server"></asp:CheckBox>
                                        <%--<asp:Label CssClass="control-label" runat="server">分派日：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_ALLOW2" CssClass="radioButton text-blue" Checked="true" Text="主官批核日：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkDAPPROVE" CssClass="radioButton text-blue" Checked="true" Text="主官批核：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">主官批核：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOVC_PUR_DAPPROVE2" CssClass="radioButton text-blue" Checked="true" Text="核定發文日：" runat="server"></asp:CheckBox>
                                        <%--<asp:CheckBox ID="chkOVC_GOOD_DAPPLY" CssClass="radioButton text-blue" Checked="true" Text="核定發文日：" runat="server"></asp:CheckBox>--%>
                                        <%--<asp:Label CssClass="control-label" runat="server">核定發文日：</asp:Label>--%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="text-center">
                                        <asp:Button ID="btnQuery_level" CssClass="btn-success btnw6" OnClick="btnQuery_level_Click" Text="計評階段查詢" runat="server" />
                                        <asp:Button ID="btnQuery_ONB" CssClass="btn-success btnw6" OnClick="btnQuery_ONB_Click" Text="採包階段查詢" runat="server" />
                                        <asp:Button ID="btnQuery_OVC" CssClass="btn-success btnw6" OnClick="btnQuery_OVC_Click" Text="履驗階段查詢" runat="server" />
                                        <asp:Button ID="btnGoBack" CssClass="btn-success btnw4 textSpace-l" OnClick="btnGoBack_Click" Text="回上一頁" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
