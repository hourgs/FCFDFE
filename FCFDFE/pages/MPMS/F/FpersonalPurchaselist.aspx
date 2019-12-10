<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FpersonalPurchaselist.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FpersonalPurchaselist" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .lbl-title {
            display: block;
            font-size: 32px;
        }
        .lbl-subtitle {
            text-align: left;
            padding-left: 20px;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:Label CssClass="lbl-title" runat="server">主官查詢系統</asp:Label>
                    <asp:Button CssClass="btn-success btnw4" OnClick="btnGoBack_Click" Text="回上一頁" runat="server" />
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GVpersonal" DataKeyNames="OVC_PURCH" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVpersonal_PreRender" OnRowDataBound="GVpersonal_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="PrintSupPDF" CommandArgument='<%#Eval("OVC_PURCH")%>' OnCommand="GVpersonal_Command" Text='<%# "" + Eval("PURCH") %>' runat="server" /><!--黃色-->
                                            <asp:Label ID="lblPURCH" CssClass="control-label" Text='<%# "" + Eval("PURCH") %>' Visible="false" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="購案名稱" >
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="btnOVCLIST_PRINT" CommandArgument='<%#Eval("OVC_PURCH")%>' OnCommand="GVpersonal_Command" Text='<%# "" + Eval("OVC_PUR_IPURCH") %>' runat="server" /><!--黃色-->
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="委購單位" DataField="OVC_PUR_NSECTION" />
                                    <asp:TemplateField HeaderText="審查次數" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblONB_CHECK_TIMES" CssClass="control-label" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="分派日" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_DRECEIVE" CssClass="control-label" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="回覆日" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_DAUDIT" CssClass="control-label" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="作業天數" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblWorkDay" CssClass="control-label" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="承辦人" DataField="OVC_AUDITOR" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

