<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E24_2.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E24_2" %>
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
                    運費 結報申請表-修改
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:Label ID="lblOvcInfNo" CssClass="control-label" runat="server" Text="INFNOLabel"></asp:Label>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案由</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:DropDownList ID="drpOvcGist" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
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
                                        <asp:TextBox ID="OvcPurposeType" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td  style="width:150px;" class="text-center">
                                       <asp:Label CssClass="control-label" runat="server">結報申請日期</asp:Label>
                                    </td>
                                    <td style="width:425px;"colspan="3">
                                       <div>
                                       <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOdtApplyDate" CssClass="tb tb-m position-left" runat="server" readonly="true"></asp:TextBox>
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
                                        <asp:Label CssClass="control-label" runat="server">金額</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:Label CssClass="control-label" runat="server">新台幣</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOnbAmount" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblBudgetInfNo" CssClass="control-label" runat="server" Text="P_BudGetINFNO"></asp:Label>&nbsp;
                                        <asp:Label ID="lblAmount" CssClass="control-label" runat="server" Text="P_Amount"></asp:Label>
                                    </td>                                   
                                </tr>
                                 <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算通知單編號</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:TextBox ID="txtOvcBudgetInfNo" CssClass="tb tb-l" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button ID="btnLoad" cssclass="btn-success" runat="server" Text="載入預算通知單編號" />
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
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="修改結報申請表" /><vr />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;" colspan="5">
                                        <asp:DropDownList ID="drpOvcInfNo" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label"  runat="server">年</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label"  runat="server">投保通知書編號</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOvcInfNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnOther" CssClass="btn-success btnw2" runat="server" Text="過濾" />
                                    </td>
                                    <td style="text-align:center;vertical-align:middle;">
                                        <asp:LinkButton ID="linkAll" runat="server" ForeColor="#6699FF">全部勾選</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton ID="linkCancel" runat="server" ForeColor="#FFCC99">全部取消</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:CheckBoxList ID="chkOvcInfNo" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>未聯繫</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnAdd" cssclass="btn-warning" runat="server" Text="↓加入運費" /><br /><br /> 
                            </div>
                            <asp:GridView ID="GridViewTBGMT_CINF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GridViewTBGMT_CINF_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="運費編號" />
                                    <asp:BoundField DataField="" HeaderText="提單案號" />
                                    <asp:BoundField DataField="" HeaderText="海空運費" />
                                    <asp:BoundField DataField="" HeaderText="捷報申請表編號" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnDel" CssClass="btn-danger" Text="刪除" CommandName="按鈕屬性" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                </Columns>
                            </asp:GridView>
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