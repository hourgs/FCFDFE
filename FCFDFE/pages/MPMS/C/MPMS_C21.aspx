<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C21.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C21" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        th{
            background-color:blue;
            color:aliceblue;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <span id="title_span"><asp:Label ID="lblPurchNum" CssClass="text-red control-label" runat="server"></asp:Label></span>購案審查簽辦表擬辦事項編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-left">
                                <tr>
                                    <th rowspan="5"   style="width: 5%;">
                                        <asp:Label CssClass="control-label" runat="server">擬<br />辦<br />事<br />項</asp:Label>
                                    </th>
                                    <th style="width: 42.5%">
                                        <asp:Label CssClass="control-label" runat="server">擬辦項目</asp:Label>
                                    </th>
                                    <th style="width: 42.5%">
                                        <asp:Label CssClass="control-label" runat="server">說明</asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="text-red control-label" runat="server">一、申請結匯通知書及輸(出)入</asp:Label><br />
                                        <asp:RadioButtonList ID="rdo1" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem Value="1">向金管會銀行局申請結匯通知書，並核准輸(出)入</asp:ListItem>
                                            <asp:ListItem Value="2">僅向金管會銀行局申請結匯款通知書，毋須辦理輸(出)入</asp:ListItem>
                                            <asp:ListItem Value="3">毋須申請結匯通知書，唯須辦理輸(出)入</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt1" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="text-red control-label" runat="server">二、核交下列單位辦理採購</asp:Label><br />
                                        <asp:RadioButtonList ID="rdo2" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem Value="1">中華民國駐美軍事代表團</asp:ListItem>
                                            <asp:ListItem Value="2">採購發包處</asp:ListItem>
                                            <asp:ListItem Value="3">駐歐採購組</asp:ListItem>
                                            <asp:ListItem Value="4" Text=""></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:TextBox ID="txtrdo2_4" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt2" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="text-red control-label" runat="server">三、准以下列招標方式辦理</asp:Label><br />
                                        <asp:RadioButtonList ID="rdo3" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem Value="1">公開招標</asp:ListItem>
                                            <asp:ListItem Value="2">限制性招標</asp:ListItem>
                                            <asp:ListItem Value="3">選擇性招標</asp:ListItem>
                                            <asp:ListItem Value="4">本案依「政府採購法」第105條第1項第4款規定循軍售途徑辦理，得不適用政府採購法招標、決標之規定</asp:ListItem>
                                            <asp:ListItem Value="5">公開取得書面報價或企畫書</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt3" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="text-red control-label" runat="server">四、准以下列採購途徑辦理</asp:Label><br />
                                        <asp:RadioButtonList ID="rdo4" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem Value="值">一般採購</asp:ListItem>
                                            <asp:ListItem Value="值">緊急採購</asp:ListItem>
                                            <asp:ListItem Value="值">軍售採購</asp:ListItem>
                                            <asp:ListItem Value="值">秘密採購</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt4" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                            <!--按鈕區-->
                            <div class="text-center">
                                <asp:Button ID="btnSave" CssClass="btn-warning btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
                                <asp:Button ID="btnReturn" CssClass="btn-warning btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
