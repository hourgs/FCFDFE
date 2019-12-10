<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FListedKind.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FListedKind" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <h1>國防部06年度憲兵單位所屬單位購案統計表</h1>
                    <h3 style="float:right;">印表日期：<asp:Label CssClass="control-label" runat="server">2017-04-20</asp:Label></h3>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr style="background-color: blue; color: white;">
                                    <th class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種\核定權責</asp:Label>
                                    </th>
                                    <th class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">A(部核)國防部核定權責</asp:Label>
                                    </th>
                                </tr>
                                <tr style="background-color: orange;">
                                    <td>國防部憲兵指揮部</td>
                                    <td>1</td>
                                </tr>
                                <tr style="background-color: skyblue;">
                                    <td>國防部憲兵指揮部</td>
                                    <td>1</td>
                                </tr>
                                <tr style="background-color: orange;">
                                    <td>合計</td>
                                    <td>2</td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
