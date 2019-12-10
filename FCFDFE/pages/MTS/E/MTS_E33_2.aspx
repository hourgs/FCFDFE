<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E33_2.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E33_2" %>
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
                    接轉作業結帳支付管理
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
                                        <asp:Label ID="lblOvcTofNo" CssClass="control-label " runat="server" Text="mnoLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblMilitaryType" CssClass="control-label " runat="server" Text="mmilitaryLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">進出口</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcIeType" CssClass="control-label " runat="server" Text="ieLabel"></asp:Label>
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
                                        <asp:Label ID="lblOvcPurposeType" CssClass="control-label " runat="server" Text="purposeLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">摘要</asp:Label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcAbstract" CssClass="control-label " runat="server" Text="abstractLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">金額</asp:Label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label CssClass="control-label" runat="server">新台幣</asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblOnbAmount" CssClass="control-label " runat="server" Text="amountLabel"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcPlnContent" CssClass="control-label " runat="server" Text="planLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">申請單位</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOvcSection" CssClass="control-label " runat="server" Text="sectionLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">申請日期</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:Label ID="lblOdtApplyDate" CssClass="control-label " runat="server" Text="applyDateLabel"></asp:Label>
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
                                        <asp:Label CssClass="control-label" runat="server">付款與否</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <asp:RadioButtonList ID="rdoOvcIsPaid" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>未付款</asp:ListItem>
                                            <asp:ListItem>已付款</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">付款日期</asp:Label>
                                    </td>
                                    <td style="width:800px;">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOdtPaidDate" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning btnw2" OnClick="btnSave_Click" runat="server" Text="更新" /> 
                                <asp:Button ID="btnHome" cssclass="btn-warning btnw4" OnClick="btnHome_Click" runat="server" Text="回首頁" />
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
