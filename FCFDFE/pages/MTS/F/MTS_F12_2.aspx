<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F12_2.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F12_2" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
         });
        var numberElements = document.getElementsByClassName("number");
        // Loop through each one
        for (var i = 0; i < numberElements.length; i++) {
            // Get your current element
            numberElements[i].type = 'number';
        }
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <asp:Label ID="lblOVC_COMPANY_title" CssClass="control-label" runat="server"></asp:Label>
                    資料維護-修改/刪除
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label ID="lblOVC_COMPANY" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:TextBox ID="txtOvcCompany" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trContract" visible="false" runat="server">
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label ID="lblContract" CssClass="control-label" runat="server">合約航商/非合約航商</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:DropDownList ID="drpContract" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Selected="True">合約航商</asp:ListItem>
                                        <asp:ListItem>非合約航商</asp:ListItem></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trTransport" visible="false" runat="server">
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label ID="lblTransport" CssClass="control-label" runat="server">空運/海運</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:DropDownList ID="drpTransport" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem>空運</asp:ListItem>
                                        <asp:ListItem>海運</asp:ListItem></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">排序</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:TextBox ID="txtOnbCoSort" CssClass="tb tb-s" TextMode="Number" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trStartDate" visible="true" runat="server">
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">開始日期</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtStartDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                       </div>
                                    </td>
                                </tr>
                                <tr id="trEndDate" visible="true" runat="server">
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結束日期</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOdtEndDate" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                       </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" OnClick="btnSave_Click" Text="更新資料" /> 
                                <asp:Button ID="btnDel" cssclass="btn-danger" runat="server" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" OnClick="btnDel_Click" Text="刪除資料" />
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

