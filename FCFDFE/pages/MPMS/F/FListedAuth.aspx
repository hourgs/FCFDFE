<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FListedAuth.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FListedAuth" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1280px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <h1>國防部06年度所屬單位購案統計表</h1>
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
                                    <th class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">B(非部核)國防部授權單位自行核定權責</asp:Label>
                                    </th>
                                    <th class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">C(非部核)國防部授權單位自行下受該定權責</asp:Label>
                                    </th>
                                    <th class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">X(非部核)其他核定權責</asp:Label>
                                    </th>
                                    <th class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">合計</asp:Label>
                                    </th>
                                </tr>
                                <tr style="background-color: orange;">
                                    <td>中央</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr style="background-color: skyblue;">
                                    <td>陸軍</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr style="background-color: orange;">
                                    <td>海軍</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr style="background-color: skyblue;">
                                    <td>空軍</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr style="background-color: orange;">
                                    <td>聯勤</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr style="background-color: skyblue;">
                                    <td>後備</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr style="background-color: orange;">
                                    <td>憲兵</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr style="background-color: skyblue;">
                                    <td>其他</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr style="background-color: orange;">
                                    <td>合計</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                            <h2><asp:Label CssClass="control-label text-red" runat="server">查詢條件：核定權責</asp:Label><br>
                            <asp:Label CssClass="control-label text-red" runat="server">說明:在預劃檔內年度所有資料皆會被按【核定權責 】依照單位檔內【單位類別】排列統計出來，案件不管分幾組以一案計算。</asp:Label></h2>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
