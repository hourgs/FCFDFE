<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F13_3.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F13_3" %>
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
                    保險費率資料維護-新增
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險公司</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:DropDownList ID="drpOvcCompany" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險費種類</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <%--<asp:TextBox ID="txtOvcInsType" CssClass="tb tb-m " Visible="false" runat="server"></asp:TextBox>--%>
                                        <asp:CheckBox ID="chkFULL_INSURANCE" Text="全險" CssClass="radioButton" OnCheckedChanged="INSURANCE_CheckedChanged" AutoPostBack="true" runat="server" />
                                        <asp:TextBox ID="txtFULL_INSURANCE" CssClass="tb tb-s " OnTextChanged="INSURANCE_CheckedChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkINSIDE_LAND_INSURANCE" Text="在台內陸險" OnCheckedChanged="INSURANCE_CheckedChanged" AutoPostBack="true" CssClass="radioButton" runat="server" />
                                        <asp:TextBox ID="txtINSIDE_LAND_INSURANCE" CssClass="tb tb-s " OnTextChanged="INSURANCE_CheckedChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                        <br /><br />
                                        <asp:CheckBox ID="chkOUTSIDE_LAND_INSURANCE" Text="在外內陸險" OnCheckedChanged="INSURANCE_CheckedChanged" AutoPostBack="true" CssClass="radioButton" runat="server" />
                                        <asp:TextBox ID="txtOUTSIDE_LAND_INSURANCE" CssClass="tb tb-s " OnTextChanged="INSURANCE_CheckedChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkMILITARY_INSURANCE" Text="兵險及罷工險" CssClass="radioButton" OnCheckedChanged="INSURANCE_CheckedChanged" AutoPostBack="true" runat="server" />
                                        <asp:TextBox ID="txtMILITARY_INSURANCE" CssClass="tb tb-s " OnTextChanged="INSURANCE_CheckedChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險費率%</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <%--<asp:TextBox ID="txtOvcInsRate" CssClass="tb tb-m " Visible="false" runat="server"></asp:TextBox>--%>
                                        <asp:Label CssClass="control-label" runat="server">Ex Work：</asp:Label>
                                        <asp:TextBox ID="txtEX_WORK" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">FBO、FCA、CPT、CFR：</asp:Label>
                                        <asp:TextBox ID="txtFBO_FCA_CPT_CFR" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                        <asp:TextBox ID="txtOTHER" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">%</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險開始日</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                       </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">保險結束日</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtEndDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                       </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">排序</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:TextBox ID="txtOnbIrSort" CssClass="tb tb-xs " TextMode="Number" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" OnClick="btnSave_Click" Text="新增保險費率資料" /><br /><br />
                                <asp:Button ID="btnHome" cssclass="btn-success" runat="server" OnClick="btnHome_Click" Text="回首頁" /><br />
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
