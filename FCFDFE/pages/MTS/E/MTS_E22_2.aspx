<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E22_2.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E22_2" %>
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
                    運費資料-修改
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td colspan="3" style="text-align:center;vertical-align:middle;">
                                        <asp:Button ID="btnlast" cssclass="btn-success btnw4" runat="server" Text="上一筆" />&nbsp;&nbsp;
                                        <asp:Button ID="btnnext" cssclass="btn-success btnw4" runat="server" Text="下一筆" />
                                    </td>
                                    <td colspan="3" style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">目前第</asp:Label>
                                        <asp:Label ID="lblnow" runat="server" Text="0" ForeColor="Red"></asp:Label><asp:Label CssClass="control-label" runat="server">筆/</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">總共</asp:Label>
                                        <asp:Label ID="lblall" runat="server" Text="0" ForeColor="Red"></asp:Label><asp:Label CssClass="control-label" runat="server">筆</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">運費編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                       <asp:Label CssClass="control-label" ID="lblOvcIcsNo" runat="server" Text="ics_noLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtBldSn" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">海空運費</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label CssClass="control-label" runat="server">新台幣</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOnbCarriage" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtInfSn" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">資料修改日期</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" ID="OdtModifyDate" runat="server" Text="datamodifydateLabel"></asp:Label>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">資料建立人員</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" ID="lblOvcCreateId" runat="server" Text="datacreateLabel"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning btnw6" runat="server" Text="更新運費資料" />
                                <asp:Button ID="btnDel" cssclass="btn-danger btnw6" runat="server" Text="刪除運費資料" /><br /><br /> 
                                <asp:Button ID="btnHome" cssclass="btn-success btnw4" runat="server" Text="回主頁" />
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