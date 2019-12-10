<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E13.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E13" %>
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
                                        </asp:DropDownList>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOvcGist" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目及編號</asp:Label>
                                    </td>
                                    <td style="width:225px;">
                                        <asp:DropDownList ID="drpOvcBudgetInfNo" CssClass="tb tb-m" runat="server">
                                             <asp:ListItem Text="未輸入" Value="未輸入" Selected></asp:ListItem>
                                            <asp:ListItem Text="維持門010105" Value="維持門010105"></asp:ListItem>
                                            <asp:ListItem Text="投資門150110" Value="投資門150110"></asp:ListItem>
                                            <asp:ListItem Text="維持門010106" Value="維持門010106"></asp:ListItem>
                                            <asp:ListItem Text="投資門150111" Value="投資門150111"></asp:ListItem>
                                        </asp:DropDownList>
                                   </td>
                                   <td  style="width:150px;" class="text-center">
                                       <asp:Label CssClass="control-label" runat="server">海空運別</asp:Label>
                                   </td>
                                   <td style="width:150px;">
                                        <asp:DropDownList ID="drpOvcSeaOrAir" CssClass="tb tb-s" runat="server">
                                             <asp:ListItem Text="海運" Value="海運" Selected></asp:ListItem>
                                            <asp:ListItem Text="空運" Value="空運"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:125px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">進出口別</asp:Label>
                                    </td>
                                    <td style="width:150px;">
                                        <asp:DropDownList ID="drpOvcImpOrExp" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Text="進口" Value="進口" Selected></asp:ListItem>
                                            <asp:ListItem Text="出口" Value="出口"></asp:ListItem>
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
                                            <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOvcApplyDate" CssClass="tb tb-s position-left text-change" runat="server" ></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
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
                                         <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOvcInvDate" CssClass="tb tb-s position-left text-change" runat="server" ></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:TextBox ID="OvcNote" CssClass="tb tb-s position-left text-change" runat="server" Rows="3"></asp:TextBox>

        
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">

                                        <asp:TextBox ID="OvcPlnContent" CssClass="tb tb-s position-left text-change" runat="server" Rows="3" Text="擬請准予結報"></asp:TextBox>

                                    </td>                                   
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="新增結報申請表" OnClick="btnSave_Click"/> 
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
