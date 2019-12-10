<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_E12_1.aspx.cs" Inherits="FCFDFE.pages.CIMS.E.CIMS_E12_1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active"); 
                }

</script>
    <style>
        tr:nth-child(odd) {
                display: none;
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
                            <asp:DataList Font-Size="20px" ID="DataList1" runat="server" GridLines="Both" RepeatDirection="Horizontal" RepeatColumns="5">
                                <HeaderTemplate>
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
                                    <tr>
                                    </tr>
                                    <tr>
                                        <th colspan="5" style="background-color: #FFFF00">
                                            <asp:Label ForeColor="Black" runat="server" Text='軍網類'></asp:Label></th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align:center">
                                            <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CL_ORD") %>'></asp:Label>

                                        </td>
                                        <td style="text-align:center">
                                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CL_LAN") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <a href="<%# DataBinder.Eval(Container,"DataItem.CL_LINK") %>">
                                                <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CL_TITLE") %>'></asp:Label></a>
                                            <br />
                                            <asp:Label runat="server">【</asp:Label><asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CL_LINK") %>'></asp:Label><asp:Label runat="server">】</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CL_DESC") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CL_UPLOADLINK") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:DataList>
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
