<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F14_3.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F14_3" %>
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
                    機場港口資料-新增
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">代碼</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:TextBox ID="txtPortCdeSn" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">中文名稱</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:TextBox ID="txtOvcPortChiName" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">英文名稱</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:TextBox ID="txtOvcPortEngName" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">海港或機場</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:DropDownList  ID="drpOvcPortType" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem Selected>海港</asp:ListItem>
                                            <asp:ListItem>機場</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">國內或國外</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:DropDownList  ID="drpOvcIsAbroad" CssClass="tb tb-m  position-left" runat="server">
                                            <asp:ListItem Selected>國內</asp:ListItem>
                                            <asp:ListItem>國外</asp:ListItem>
                                            <asp:ListItem>美加東岸</asp:ListItem>
                                            <asp:ListItem>美加西岸</asp:ListItem>
                                            <asp:ListItem>歐洲</asp:ListItem>
                                            <asp:ListItem>澳洲</asp:ListItem>
                                            <asp:ListItem>東北亞</asp:ListItem>
                                            <asp:ListItem>東南亞</asp:ListItem>
                                            <asp:ListItem>香港</asp:ListItem>
                                            <asp:ListItem>地中海</asp:ListItem>
                                            <asp:ListItem>新加坡</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">航線</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:TextBox ID="txtOVC_ROUTE" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="新增機場港口資料"  OnClick="btnSave_Click"/><br /><br /> 
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
