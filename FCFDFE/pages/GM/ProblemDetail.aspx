<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ProblemDetail.aspx.cs" Inherits="FCFDFE.pages.GM.ProblemDetail" %>

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
                                        <td >
                                            <asp:Label CssClass="control-label" runat="server">1.&nbsp;子系統名稱：</asp:Label>
                                            <asp:Label ID="labC_SN_SYS" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">2.&nbsp;日期：</asp:Label>
                                            <asp:Label ID="labDate" CssClass="control-label" runat="server"></asp:Label>
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
                                            <asp:Label ID="labOVC_ONNAME" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">人員：</asp:Label>
                                            <asp:Label ID="labPerson" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">電話：</asp:Label>
                                            <asp:Label ID="labPhone" CssClass="control-label" runat="server"></asp:Label>
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
                                            <asp:Label ID="labPRO_DESC" CssClass="control-label"    runat="server" TextMode="MultiLine"></asp:Label><br />
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
                                                <asp:Label ID="labODT_GET_DATE" CssClass="control-label" runat="server"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">（３）預計完成日期：</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:Label ID="labODT_PLAN_DATE" CssClass="control-label" runat="server"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">6.&nbsp;問題處理情形：</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labQuestionProcess" CssClass="control-label" runat="server" ></asp:Label>&nbsp;&nbsp;                   
                                        </td>
                                    </tr>
<%--                                    <tr>
                                        <td>7.上傳附件<asp:FileUpload ID="fuUPLOAD_Problem" Width="40%" runat="server" /></td>
                                    </tr>--%>
                                </table>
                            </div>
                            <div class="text-center">
                                <asp:Button ID="btnCreate" CssClass="btn-warning" runat="server" Text="產出申請單" OnClick ="btnCreate_Click" />
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

