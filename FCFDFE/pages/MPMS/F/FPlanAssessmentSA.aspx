<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FPlanAssessmentSA.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FPlanAssessmentSA" %>

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
        }
        .lbl-s {
            display: inline-block;
            width: 100px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:Label CssClass="control-label lbl-title" runat="server"><%=strDEPT_Name%></asp:Label>
                    <asp:Label CssClass="control-label lbl-title" runat="server">計畫評核系統</asp:Label>
                    <asp:Label CssClass="control-label lbl-title text-red" runat="server">統計分析報表主畫面</asp:Label>
                    <%--<asp:Button ID="btnGohome" CssClass="btn-success btnw4" runat="server" Text="回主畫面" />--%><!--綠色-->
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" style="width: 70px">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_AnnualPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度購案管制表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_PurImple_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案執行進度明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_AllUnitPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">各單位購案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_EUSImple_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">駐美、歐組購案履驗階段執行現況明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_ClosedCompar_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">澄覆比較表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_PurchaseNum_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案編號：</asp:Label>
                                        <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_PerPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpYear_PerPur" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                        <asp:Label CssClass="control-label text-blue" runat="server">個人承辦案資料明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_PurSum_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td style="width: 15px">
                                        <asp:DropDownList ID="drpYear_PurSum" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                    </td>
                                    <td style="width: 70%;">
                                        <asp:Label CssClass="control-label text-blue" runat="server">呈報日期自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_PurSum1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_PurSum2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">部核定購案統計總表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_OpenRead_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear_OpenRead" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">呈報日期自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_OpenRead1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_OpenRead2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">辦理公開閱覽購案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_UndonePurSta_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear_UndonePurSta" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">呈報日期自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_UndonePurSta1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_UndonePurSta2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">尚未完成資訊審查作業購案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_MostFavS_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear_MostFavS" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">呈報日期自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_MostFavS1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_MostFavS2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採用最有利標購案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_ApprovedPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear_ApprovedPur" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">呈報日期自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_ApprovedPur1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_ApprovedPur2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">已核定購案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_CasePur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear_CasePur" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">呈報日期自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_CasePur1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_CasePur2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">辦理撤案購案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_UnitPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpYear_UnitPur" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">呈報日期自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_UnitPur1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_UnitPur2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位屬性：</asp:Label>

                                        <asp:DropDownList ID="drp_Unit" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_MNDPur_Click" Text="查詢" runat="server" /><!--綠色-->
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="drpYear_MNDPur" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度</asp:Label>
                                        <asp:Label CssClass="control-label text-blue" runat="server">國防部所屬單位委託採購中心辦理購案暨委製案件採購作業節點管制表</asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery16_Click" Text="查詢" runat="server" />
                                    </td>
                                    <td colspan="2">
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkYear16" CssClass="radioButton text-blue" Checked="true" Text="年度：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdo_Year" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server"></asp:Label>--%>
                                            </div>
                                            <asp:DropDownList ID="drpYear" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_PUR_APPROVE_DEP" CssClass="radioButton text-blue" Text="採購權責：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_PUR_APPROVE_DEP" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">採購權責：</asp:Label>--%>
                                            </div>
                                            <asp:DropDownList ID="drpOVC_PUR_APPROVE_DEP" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_LAB" CssClass="radioButton text-blue" Text="採購屬性：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_LAB" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">採購屬性：</asp:Label>--%>
                                            </div>
                                            <asp:DropDownList ID="drpOVC_LAB" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_PUR_AGENCY" CssClass="radioButton text-blue" Text="採購途徑：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_ITEM" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">採購途徑：</asp:Label>--%>
                                            </div>
                                            <%--<asp:DropDownList ID="drpOVC_ITEM" CssClass="tb tb-l" runat="server"></asp:DropDownList>--%>
                                            <asp:DropDownList ID="drpOVC_PUR_AGENCY" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_PUR_ASS_VEN_CODE" CssClass="radioButton text-blue" Text="招標方式：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_PUR_ASS_VEN_CODE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">招標方式：</asp:Label>--%>
                                            </div>
                                            <asp:DropDownList ID="drpOVC_PUR_ASS_VEN_CODE" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_PLAN_PURCH" CssClass="radioButton text-blue" Text="計畫性質：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_PLAN_PURCH" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">計畫性質：</asp:Label>--%>
                                            </div>
                                            <asp:DropDownList ID="drpOVC_PLAN_PURCH" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_PUR_IPURCH" CssClass="radioButton text-blue" Text="購案名稱：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_PUR_IPURCH" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">購案名稱：</asp:Label>--%>
                                            </div>
                                            <asp:TextBox ID="txtOVC_PUR_IPURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            <%--<asp:DropDownList ID="drpOVC_PUR_IPURCH" CssClass="tb tb-l" runat="server"></asp:DropDownList>--%>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_DPROPOSE" CssClass="radioButton text-blue" Text="申購日期：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_DPROPOSE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">申購日期：</asp:Label>--%>
                                            </div>
                                            <asp:Label CssClass="control-label text-blue" runat="server">自：</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtOVC_DPROPOSE1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtOVC_DPROPOSE2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_PUR_DAPPROVE" CssClass="radioButton text-blue" Text="核定日期：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_PUR_DAPPROVE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">核定日期：</asp:Label>--%>
                                            </div>
                                            <asp:Label CssClass="control-label text-blue" runat="server">自：</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtOVC_PUR_DAPPROVE1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtOVC_PUR_DAPPROVE2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_PUR_LEVEL" CssClass="radioButton text-blue" Text="採購級距：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdoOVC_PUR_LEVEL" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">採購級距：</asp:Label>--%>
                                            </div>
                                            <asp:DropDownList ID="drpOVC_PUR_LEVEL" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery17_Click" Text="查詢" runat="server" />
                                    </td>
                                    <td colspan="2">
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkYear17" CssClass="radioButton text-blue" Checked="true" Text="年度：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdo_section_Year" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">年度：</asp:Label>--%>
                                            </div>
                                            <asp:DropDownList ID="drpYear1" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                            <asp:Label CssClass="control-label" runat="server">～</asp:Label>
                                            <asp:DropDownList ID="drpYear2" CssClass="tb drp-year" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chk_OVC_PUR_IPURCH17" CssClass="radioButton text-blue" Text="購案名稱：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdo_sOVC_PUR_IPURCH" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">購案名稱：</asp:Label>--%>
                                            </div>
                                            <asp:TextBox ID="txtOVC_PUR_IPURCH17" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            <%--<asp:DropDownList ID="drp_sOVC_PUR_IPURCH" CssClass="tb tb-l" runat="server"></asp:DropDownList>--%>
                                        </div>
                                            
                                        <div class="col-lg-12 form-group-space-b">
                                            <div class="lbl-s">
                                                <asp:CheckBox ID="chkOVC_PUR_TYPE" CssClass="radioButton text-blue" Text="採購品項：" runat="server" />
                                                <%--<asp:RadioButtonList ID="rdo_sOVC_PUR_TYPE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>--%>
                                                <%--<asp:Label CssClass="control-label text-blue" runat="server">採購品項：</asp:Label>--%>
                                            </div>
                                            <asp:TextBox ID="txtOVC_PUR_TYPE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            <%--<asp:DropDownList ID="drp_sOVC_PUR_TYPE" CssClass="tb tb-l" runat="server"></asp:DropDownList>--%>
                                        </div>
                                    </td>
                                </tr>

<%--                                <tr>
                                    <td rowspan="10" style="width: 135px;">
                                        <asp:Button ID="btnQuery_All" CssClass="btn-success btnw2" Text="查詢" runat="server" />
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdo_Year" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drp_Year" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_PUR_APPROVE_DEP" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購權責：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drpOVC_PUR_APPROVE_DEP" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_LAB" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購屬性：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drpOVC_LAB" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_ITEM" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購途徑：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drpOVC_ITEM" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_PUR_ASS_VEN_CODE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">招標方式：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drpVC_PUR_ASS_VEN_CODE" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_PLAN_PURCH" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">計畫性質：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drpOVC_PLAN_PURCH" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_PUR_IPURCH" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案名稱：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drpOVC_PUR_IPURCH" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_DPROPOSE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">申購日期：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left">
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DPROPOSE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DPROPOSE2" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_PUR_DAPPROVE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">核定日期：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left">
                                        <asp:Label CssClass="control-label text-blue" runat="server">自</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_DAPPROVE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_DAPPROVE2" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label text-blue" runat="server">止</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdoOVC_PUR_LEVEL" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購級距：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drpOVC_PUR_LEVEL" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3">
                                        <asp:Button ID="btnQuery_section" CssClass="btn-success btnw2" Text="查詢" runat="server" />
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdo_section_Year" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">年度：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drp_section_year" CssClass="tb tb-xs" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">～</asp:Label>
                                        <asp:DropDownList ID="drp_section_year2" CssClass="tb tb-xs" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdo_sOVC_PUR_IPURCH" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">購案名稱：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drp_sOVC_PUR_IPURCH" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:RadioButtonList ID="rdo_sOVC_PUR_TYPE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購品項：</asp:Label>
                                    </td>
                                    <td class="no-bordered text-left text-blue">
                                        <asp:DropDownList ID="drp_sOVC_PUR_TYPE" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>--%>
                            </table>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
