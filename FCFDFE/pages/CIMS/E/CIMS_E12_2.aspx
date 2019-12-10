<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_E12_2.aspx.cs" Inherits="FCFDFE.pages.CIMS.E.CIMS_E12_2" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>

</script>
    <style>
        tr td:nth-child(1){
            text-align:center;
        }
        tr:not(:first-child) th{
            background-color: #FFFF00;
        }
    </style>
    <div class="row">
        <div style="width: 900px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>常用網址</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table style="font-size: 20px" border="1">
                                <tr>
                                    <th style="width: 3%">
                                        <asp:Label ForeColor="Red" runat="server" Text='項次'></asp:Label></th>
                                    <th style="width: 3%">
                                        <asp:Label ForeColor="Black" runat="server" Text='網路別'></asp:Label></th>
                                    <th style="width: 40%">
                                        <asp:Label ForeColor="Red" runat="server" Text='連結網址'></asp:Label></th>
                                    <th style="width: 40%">
                                        <asp:Label ForeColor="Black" runat="server" Text='網址內容概述'></asp:Label></th>
                                    <th style="width: 10%">
                                        <asp:Label ForeColor="Black" runat="server" Text='備註'></asp:Label></th>
                                </tr>
                                <asp:Literal id="Literal1"  runat="server"/>
                            </table>
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
