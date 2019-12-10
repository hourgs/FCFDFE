<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F12_3.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F12_3" %>
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
                    <asp:Label ID="lblOVC_COMPANY_title" CssClass="control-label" runat="server"></asp:Label>
                    資料-新增
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
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">排序</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                       <asp:TextBox ID="txtOnbCoSort" CssClass="tb tb-s " TextMode="Number" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" OnClick="btnSave_Click" Text="新增" /><br /><br />
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