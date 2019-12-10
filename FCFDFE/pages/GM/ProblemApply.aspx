<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProblemApply.aspx.cs" Inherits="FCFDFE.pages.GM.ProblemApply" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>--%>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    問題(保修)申請單
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="form-group">
                                <table class="table table-bordered text-left" style="margin-bottom: 0;">
                                    <tr>
                                        <td style="width: 75%">
                                            <asp:Label CssClass="control-label" runat="server">1.&nbsp;子系統名稱：</asp:Label>
                                            <asp:DropDownList ID="drpC_SN_SYS" CssClass="tb tb-m" runat="server" ></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">2.&nbsp;日期：</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtDate" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table table-bordered text-left" style="margin-top: 0px;">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label CssClass="control-label" runat="server">3.業管單位及人員、電話：</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">業管單位：</asp:Label>
                                            <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server" />
                                            <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                            <asp:Button ID="btnQuery1" CssClass="btn-success btnw2" OnClientClick="OpenWindow('txtOVC_DEPT_CDE','txtOVC_ONNAME')" OnClick="btnQuery1_Click" runat="server" Text="單位" />
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">人員：</asp:Label>
                                            <asp:TextBox ID="txtPerson" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">電話：</asp:Label>
                                            <asp:TextBox ID="txtPhone" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label CssClass="control-label" runat="server">4.&nbsp;問題描述、影響與需求規格：</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtPRO_DESC" CssClass="tb tb-s " Width="95%" Height="300px" runat="server" TextMode="MultiLine"></asp:TextBox><br />

                                            <span class="control-label text-red">（1~4 &nbsp; 由使用承參提出時填寫）</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="form-group">
                                <table class="table table-bordered text-left">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">5.&nbsp;問題審查與工作指派：</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">
                                        （１）問題種類：
                                        <span class="control-lable text-red">(此部分自動於報表中產生，使用者自行勾選)</span>
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">（２）接獲通知日期：</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_GET_DATE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">（３）預計完成日期：</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_PLAN_DATE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">6.&nbsp;問題處理情形：<span class="control-lable text-red">（欄位不足請自行延伸，或以附件說明）</span></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtQuestionProcess" CssClass="tb tb-s " Width="95%" Height="300px" runat="server" TextMode="MultiLine"></asp:TextBox>&nbsp;&nbsp;                   
                                            <span class="control-label text-red">（5~6 &nbsp; 由維護承商填寫）</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">7.上傳附件</asp:Label>
                                            <asp:FileUpload ID="fuUPLOAD_Problem" title="瀏覽" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="text-center">
                                <asp:Button ID="btnCreate" CssClass="btn-warning" runat="server" Text="產出申請單" OnClick="btnCreate_Click" />
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
