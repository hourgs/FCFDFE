<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E23.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E23" %>
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
                    運費 結報申請表-新增
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案由</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:DropDownList ID="drpOvcGist" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOvcShipName" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目及編號</asp:Label>
                                    </td>
                                    <td style="width:225px;">
                                        <asp:DropDownList ID="drpOvcBudgetInfNo" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                   </td>
                                   <td  style="width:150px;" class="text-center">
                                       <asp:Label CssClass="control-label" runat="server">海空運別</asp:Label>
                                   </td>
                                   <td style="width:150px;">
                                        <asp:DropDownList ID="drpOvcSeaOrAir" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:125px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">進出口別</asp:Label>
                                    </td>
                                    <td style="width:150px;">
                                        <asp:DropDownList ID="drpOvcImpOrExp" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">用途別</asp:Label>
                                    </td>
                                    <td style="width:225px;">
                                        <asp:TextBox ID="txtOvcPurposeType" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td  style="width:150px;" class="text-center">
                                       <asp:Label CssClass="control-label" runat="server">結報申請日期</asp:Label>
                                    </td>
                                    <td style="width:425px;"colspan="3">
                                       <div>
                                       <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" >
                                            <asp:TextBox ID="txtOvcApplyDate" CssClass="tb tb-m position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <br /><br /> 
                                        </div>
                                        <div class='input-append timepicker'>
                                            <asp:TextBox ID="txtOvcApplyTime" CssClass='tb tb-s position-left' readonly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-time"></i></span>
                                        </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算通知單編號</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:TextBox ID="txtOvcBudgetInfNo" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收據號碼</asp:Label>
                                    </td>
                                    <td style="width:225px;">
                                        <asp:TextBox ID="txtOvcInvNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td  style="width:150px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">收據日期</asp:Label>
                                    </td>
                                    <td style="width:425px;"colspan="3">
                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOvcInvDate" CssClass="tb tb-m position-left" runat="server" readonly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:ListBox ID="lstOvcNote" CssClass="tb tb-l"  runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:ListBox>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:ListBox ID="lstOvcPlnContent" CssClass="tb tb-l"  runat="server">
                                            <asp:ListItem>擬請准予結報</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:ListBox>
                                    </td>                                   
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="新增結報申請表" /><br />
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
