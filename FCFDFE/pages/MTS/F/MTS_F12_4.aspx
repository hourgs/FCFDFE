<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F12_4.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F12_4" %>
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
                    保險公司資料維護-修改/刪除
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
                                       <asp:TextBox ID="txtOvcCompany" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">排序</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:TextBox ID="txtOnbCoSort" CssClass="tb tb-s " TextMode="Number" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">開始日期</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                       </div>
                                       <asp:Label runat="server" CssClass="control-label position-left"  Text="(日期格式:yyyy-mm-dd)" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結束日期</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtEndDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                       </div>
                                       <asp:Label runat="server" CssClass="control-label position-left"  Text="(日期格式:yyyy-mm-dd)" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" OnClick="btnSave_Click" Text="更新公司資料" /> 
                                <asp:Button ID="btnDel" cssclass="btn-danger" runat="server" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" OnClick="btnDel_Click" Text="刪除公司資料" />
                                 <asp:Button ID="btnHome" cssclass="btn-success" runat="server" OnClick="btnHome_Click" Text="回首頁" />
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

