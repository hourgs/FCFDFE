<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E32_2.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E32_2" %>
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
                    接轉作業費結報申請表-修改
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">編號</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcTofNo" CssClass="control-label" runat="server" Text="mnoLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblMilitaryType" CssClass="control-label" runat="server" Text="mmilitaryLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">進出口</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:DropDownList  ID="drpOvcIeType" CssClass="tb tb-s  position-left" runat="server">
                                            <asp:ListItem>進口</asp:ListItem>
                                            <asp:ListItem>出口</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:DropDownList  ID="drpOvcBudget" CssClass="tb tb-m  position-left" runat="server">
                                            <asp:ListItem>未設定</asp:ListItem>
                                            <asp:ListItem>維持門010105</asp:ListItem>
                                            <asp:ListItem>維持門010106</asp:ListItem>
                                            <asp:ListItem>投資門150110</asp:ListItem>
                                            <asp:ListItem>投資門150111</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:DropDownList  ID="drpOvcPurposeType" CssClass="tb tb-m  position-left" runat="server">
                                            <asp:ListItem Value="0294">運費0294</asp:ListItem>
                                            <asp:ListItem Value="0291">國內旅遊0291</asp:ListItem>
                                            <asp:ListItem Value="0203">郵電費0203</asp:ListItem>
                                            <asp:ListItem Value="0271">物品費0271</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">摘要</asp:Label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:TextBox ID="txtOvcAbstract" CssClass="tb tb-full" runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">金額</asp:Label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label CssClass="control-label" runat="server">新台幣</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOnbAmount" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:TextBox ID="txtOvcNote" CssClass="tb tb-full" runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:TextBox ID="txtOvcPlnContent" CssClass="tb tb-full" runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" cssclass="btn-warning btnw" runat="server" Text="修改接轉作業結報申請表" /> 
                                 <asp:Button ID="btnHome" OnClick="btnHome_Click" cssclass="btn-warning btnw" runat="server" Text="回首頁" /> 
                                 
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

