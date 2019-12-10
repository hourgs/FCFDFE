<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F15_3.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F15_3" %>
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
                    貨幣資料-新增
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">幣別代碼</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:TextBox ID="txtCurrencyCode" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">幣別名稱</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:TextBox ID="txtCurrencyName" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">貨幣別</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:DropDownList ID="drpOvcType" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Selected>每日</asp:ListItem>
                                            <asp:ListItem>每週</asp:ListItem>
                                            <asp:ListItem>每月</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">貨幣狀態</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:DropDownList  ID="drpOvcStatus" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem Selected>Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">排序</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:TextBox ID="txtOnbSort" CssClass="tb tb-xs " runat="server" TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="新增"  OnClick="btnSave_Click"/><br /><br /> 
                                <asp:Button ID="btnHome" cssclass="btn-success" runat="server" Text="回首頁"  OnClick="btnHome_Click"/><br />
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
