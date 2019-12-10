<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C22.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C22" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    會辦意見編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="title text-center">
                                <asp:Label CssClass=" text-red"  runat="server">購案編號：</asp:Label>
                                <asp:Label ID="lblPurchNum" CssClass=" text-red" runat="server"></asp:Label>
                                <asp:Label CssClass=" text-red" runat="server">審查次數：</asp:Label>
                                <asp:Label ID="lblCheckTimes" CssClass=" text-red" runat="server"></asp:Label>
                            </div>
                            <asp:GridView ID="GV_OVC" CssClass=" table data-table table-striped border-top " OnRowCommand="GV_OVC_RowCommand"
                                    AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button ID="btnM" CssClass="btn-danger btnw2" Text="異動" CommandName="btnM" runat="server" />
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="btnDel" runat="server" />
                                            <asp:HiddenField ID="hidONB_NO" Value='<%# Bind("ONB_NO") %>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="單位" DataField="OVC_ORG" />
                                    <asp:BoundField HeaderText="審查意見" DataField="OVC_MEMO" />
                                    <asp:BoundField HeaderText="辦理情形" DataField="OVC_PERFORM" />
                                </Columns>
                            </asp:GridView>
                            <div class="text-center" style="margin: 5% auto">
                                <asp:Button ID="btnReturn" CssClass="btn-warning btnw6" OnClick="btnReturn_Click" runat="server" Text="回綜簽作業" />
                            </div>
                            <div class="title text-center">
                                <asp:Label CssClass=" text-red"  runat="server">編輯區</asp:Label>
                            
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td rowspan="3"><asp:Button ID="btnSave" CssClass="btn-warning btnw6" OnClick="btnSave_Click" runat="server" Text="存檔" /></td>
                                    <td>審查單位</td>
                                    <td><asp:TextBox ID="txtunit" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>審查意見</td>
                                    <td><asp:TextBox ID="txtMes" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>辦理情形</td>
                                    <td><asp:TextBox ID="txtdo" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
