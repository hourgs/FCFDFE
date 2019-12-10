<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FListedCommon.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FListedCommon" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .lbl-subtitle {
            padding-left: 20px;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div class="lbl-subtitle">
                        <asp:Label runat="server">國防部</asp:Label>
                        <asp:Label ID="lblYear" runat="server"></asp:Label>
                        <asp:Label runat="server">年度</asp:Label>
                        <asp:Label runat="server">所屬單位購案統計表</asp:Label>
                    </div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GVListed" DataKeyNames="" CssClass="table table-striped border-top table-bordered text-center" AutoGenerateColumns="false" OnPreRender="GVListed_PreRender" OnRowDataBound="GVListed_RowDataBound" runat="server">
                                <Columns>
                                    <%--<asp:BoundField HeaderText="合計" DataField="" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

